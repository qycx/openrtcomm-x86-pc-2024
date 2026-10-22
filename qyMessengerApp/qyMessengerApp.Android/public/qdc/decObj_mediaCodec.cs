using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Media;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thread = Java.Lang.Thread;


using File = Java.IO.File;
using qyMessengerApp.Droid;

//
namespace qy.Droid
{

    //
    public class decObj_mediaCodec
    {
        private static string TAG = "ExtractMpegFramesTest";
        private static bool VERBOSE = false;           // lots of logging

        // where to find files (note: requires WRITE_EXTERNAL_STORAGE permission)
        //private static  File FILES_DIR = Android.OS.Environment.GetExternalStorageDirectory();
        private static File FILES_DIR = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
        private static string INPUT_FILE = "source.mp4";
        private static int MAX_FRAMES = 100;//10;       // stop extracting after this many

        //
        public EGLContext m_sharedEglContext;
        public QOI_common m_qoi;

        //

        //
        public int m_iW;
        public int m_iH;
        //
        public int m_iFourcc_i;

        //
        public bool m_bEnd;


        //
        //
        public int m_old_lPktId;		//  用来防止getNextPkt出错，得到的是重复包


        //
        public string mTag;
        public bool m_bTest;

        //
        public decObj_mediaCodec_Wrapper m_wrapper;
        public Thread m_th;

        //
        byte[] m_tmpDataBuf = new byte[Consts.bufSize_transferVideoData];


        //
        //public static int m_test_texId;
        //public static CodecOutputSurface m_outputSurface;
        //public static int m_w, m_h;

        //
#if DEBUG
#if false
        public static int m_test_texture;
        public static int m_tex_w;
        public static int m_tex_h;
#endif
#endif


        /** test entry point */
        public void testExtractMpegFrames()
        {
            //
            decObj_mediaCodec_Wrapper.runTest(this);
        }

        /**
         * Wraps extractMpegFrames().  This is necessary because SurfaceTexture will try to use
         * the looper in the current thread if one exists, and the CTS tests create one on the
         * test thread.
         *
         * The wrapper propagates exceptions thrown by the worker thread back to the caller.
         */
        public class decObj_mediaCodec_Wrapper : Java.Lang.Object, IRunnable
        {
            private Throwable mThrowable;
            private decObj_mediaCodec mTest;

            public decObj_mediaCodec_Wrapper(decObj_mediaCodec test)
            {
                mTest = test;
            }

            public void Run()
            {
                try
                {
                    //mTest.extractMpegFrames();
                    //
                    mTest.decFrames();

                }
                /*
                catch (Throwable th)
                {
                    qyFuncs.showInfo_open(0, null, "decObj.run, catch throwalbe");

                    //
                    mThrowable = th;
                }
                */
                catch(Java.Lang.Exception )
                {
                    qyFuncs.showInfo_open(0, null, "decObj.run, catch exception");
                }

                //
                mTest.m_bEnd = true;

                //
                qyFuncs.mySetEvent((System.Threading.EventWaitHandle)mTest.m_qoi.hEvent_notifyToQuit);

                //
                string str;
                str = string.Format("mediaCodec.wrapper.Run ends.");
                qyFuncs.showInfo_open(0, null, str);

                //
                return;
            }

            /** Entry point. */
            public static void runTest(decObj_mediaCodec obj)
            {

#if false
                //ExtractMpegFramesWrapper wrapper = new ExtractMpegFramesWrapper(obj);

                //Thread th = new Thread(wrapper, "codec test");

                //
                th.Start();
                th.Join();
                if (wrapper.mThrowable != null)
                {
                    throw wrapper.mThrowable;
                }
#endif
                //
                obj.m_wrapper = new decObj_mediaCodec_Wrapper(obj);
                obj.m_th = new Thread(obj.m_wrapper, "codec test");
                obj.m_th.Start();

            }
        }

        /**
         * Tests extraction from an MP4 to a series of PNG files.
         * <p>
         * We scale the video to 640x480 for the PNG just to demonstrate that we can scale the
         * video with the GPU.  If the input video has a different aspect ratio, we could preserve
         * it by adjusting the GL viewport to get letterboxing or pillarboxing, but generally if
         * you're extracting frames you don't want black bars.
         */
        private void extractMpegFrames()
        {
            MediaCodec decoder = null;
            CodecOutputSurface outputSurface = null;
            MediaExtractor extractor = null;
            int saveWidth = 640;
            //int saveHeight = 368;// 480;
            int saveHeight = 360;// 480;

            //
#if DEBUG
            saveWidth = 360;
            saveHeight = 800;
#endif


            //
            try
            {
#if false
                File inputFile = new File(FILES_DIR, INPUT_FILE);   // must be an absolute path
                                                            // The MediaExtractor error messages aren't very useful.  Check to see if the input
                                                            // file exists so we can throw a better one if it's not there.
        if (!inputFile.canRead())
        {
            throw new FileNotFoundException("Unable to read " + inputFile);
        }
#endif
                Context context = qyMessengerApp.Droid.MyApplication.Context;// mainActivity;

                //
                AssetManager assets = context.Assets;
                //
                AssetFileDescriptor afd = assets.OpenFd("vid.mp4");



                //
                extractor = new MediaExtractor();
                extractor.SetDataSource(afd);
                int trackIndex = selectTrack(extractor);
                if (trackIndex < 0)
                {
                    //throw new RuntimeException("No video track found in " + inputFile);
                }
                extractor.SelectTrack(trackIndex);

                MediaFormat format = extractor.GetTrackFormat(trackIndex);
                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }

                //
                //decObj_mediaCodec.m_w = saveWidth;
                //decObj_mediaCodec.m_h = saveHeight;

                // Could use width/height from the MediaFormat to get full-size frames.
                outputSurface = new CodecOutputSurface(this, this.m_sharedEglContext, saveWidth, saveHeight, this.mBackgroundHandler);

                //
                outputSurface.mTextureRender.tmpFrameBuffer.createEnv(saveWidth, saveHeight, "dec.render");

                // Create a MediaCodec decoder, and configure it with the MediaFormat from the
                // extractor.  It's very important to use the format from the extractor because
                // it contains a copy of the CSD-0/CSD-1 codec-specific data chunks.
                string mime = format.GetString(MediaFormat.KeyMime);
                decoder = MediaCodec.CreateDecoderByType(mime);
                decoder.Configure(format, outputSurface.getSurface(), null, 0);
                decoder.Start();

                doExtract(extractor, trackIndex, decoder, outputSurface);
            }
            finally
            {
                // release everything we grabbed
                //
                outputSurface.mTextureRender.tmpFrameBuffer.deleteEnv();

                //
                if (outputSurface != null)
                {
                    outputSurface.release();
                    outputSurface = null;
                }
                if (decoder != null)
                {
                    decoder.Stop();
                    decoder.Release();
                    decoder = null;
                }
                if (extractor != null)
                {
                    extractor.Release();
                    extractor = null;
                }
            }
        }


        //
#if DEBUG
        MediaExtractor m_test_extractor = null;
        int m_test_trackIndex = 0;
#endif
        //
        MediaFormat m_dec_format = null;

        //
        // for onFrameAvail
        public HandlerThread mBackgroundThread;
        public Handler mBackgroundHandler;

        //
        public void startBackgroundThread()
        {
            mBackgroundThread = new HandlerThread("OnFrameAvailBackground");
            mBackgroundThread.Start();
            mBackgroundHandler = new Handler(mBackgroundThread.Looper);
        }

        public void stopBackgroundThread()
        {
            if (mBackgroundThread != null)
            {
                mBackgroundThread.QuitSafely();
            }
            try
            {
                if (mBackgroundThread != null)
                {
                    try
                    {
                        mBackgroundThread.Join();

                    }
                    catch (UnsupportedOperationException ie)
                    {

                    }





                }
                mBackgroundThread = null;
                mBackgroundHandler = null;
            }
            catch (InterruptedException e)
            {
                e.PrintStackTrace();
            }
        }


        //
        int get_tti(CCtxQmc pProcInfo, ref TMP_tex_info tti)
        {
            int iErr = -1;
            decObj_mediaCodec dm = this;

            if (dm.m_qoi.cfg.v.uiTransformType_pParent == Consts.CONST_uiTransformType_dec)
            {
                QY_TRANSFORM pTransform = (QY_TRANSFORM)dm.m_qoi.cfg.v.pParent_transform;
                if (pTransform.index_activeMems_from < 0 || pTransform.index_activeMems_from >= pProcInfo.av.texes.taskAvTexes.tti_peers.Length) goto errLabel;
                tti = pProcInfo.av.texes.taskAvTexes.tti_peers[pTransform.index_activeMems_from];

            }

            iErr = 0;
        errLabel:

            return iErr;
        }

        //
        int removeFromTTi(int texId)
        {
            return 0;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //ref TMP_texes texes = ref pProcInfo.av.texes;
            TMP_tex_info tti = null;// ref texes.tti_peer;
            get_tti(pProcInfo, ref tti);
            if (tti == null) return -1;

            if (texId == 0) return -1;

            if (tti.texId == texId)
            {
                tti.texId = 0;
            }


            return 0;
        }



        //
        private void decFrames()
        {
            MediaCodec decoder = null;
            CodecOutputSurface outputSurface = null;
            //MediaExtractor extractor = null;
            int saveWidth = 640;
            int saveHeight = 360;

            //


            //
            try
            {
#if false
                File inputFile = new File(FILES_DIR, INPUT_FILE);   // must be an absolute path
                                                            // The MediaExtractor error messages aren't very useful.  Check to see if the input
                                                            // file exists so we can throw a better one if it's not there.
        if (!inputFile.canRead())
        {
            throw new FileNotFoundException("Unable to read " + inputFile);
        }
#endif

                //
#if false
                Context context = qyMessengerApp.Droid.MyApplication.Context;// mainActivity;

                //
                AssetManager assets = context.Assets;
                //
                AssetFileDescriptor afd = assets.OpenFd("vid.mp4");



                //
                extractor = new MediaExtractor();
                extractor.SetDataSource(afd);
                int trackIndex = selectTrack(extractor);
                if (trackIndex < 0)
                {
                    //throw new RuntimeException("No video track found in " + inputFile);
                }
                extractor.SelectTrack(trackIndex);

                MediaFormat format = extractor.GetTrackFormat(trackIndex);
                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }
#endif
                //
                dec_init();

                //
                saveWidth = this.m_iW;
                saveHeight = this.m_iH;

                //
                MediaFormat format = this.m_dec_format;

                //
#if DEBUG
#endif

                //
                startBackgroundThread();

                // Could use width/height from the MediaFormat to get full-size frames.
                outputSurface = new CodecOutputSurface(this, this.m_sharedEglContext, saveWidth, saveHeight, this.mBackgroundHandler);

                //
                outputSurface.mTextureRender.tmpFrameBuffer.createEnv(saveWidth, saveHeight, "dec.render");

                // Create a MediaCodec decoder, and configure it with the MediaFormat from the
                // extractor.  It's very important to use the format from the extractor because
                // it contains a copy of the CSD-0/CSD-1 codec-specific data chunks.
                string mime = format.GetString(MediaFormat.KeyMime);
                decoder = MediaCodec.CreateDecoderByType(mime);
                //
                /* if (!qyFuncs_Droid.isHarmonyOSa())
                 {
                     format.SetInteger(MediaFormat.KeyLowLatency, 1);
                 }*/
                //
                decoder.Configure(format, outputSurface.getSurface(), null, 0);
                decoder.Start();

                //doExtract(extractor, trackIndex, decoder, outputSurface);
                doDec(this, decoder, outputSurface);

                //
                int ii = 0;
            }
            catch(Java.Lang.Exception e)
            {
                int ii = 0;
                qyFuncs.showInfo_open(0, "", "mediaCodec.decFrames exception");
                //
            }
            finally
            {
                //
                string str;
                str = string.Format("dec.decFrames, finally, {0}", this.mTag);
                qyFuncs.showInfo_open(0, null, str);

                // release everything we grabbed
                //
                if (outputSurface != null)
                {
                    //
                    removeFromTTi(outputSurface.mTextureRender.tmpFrameBuffer.mTexture[0]);
                    //
                    outputSurface.mTextureRender.tmpFrameBuffer.deleteEnv();
                }

                //
                if (outputSurface != null)
                {
                    outputSurface.release();
                    outputSurface = null;
                }
                if (decoder != null)
                {
                    decoder.Stop();
                    decoder.Release();
                    decoder = null;
                }
                //
                stopBackgroundThread();

                //
#if false
                if (extractor != null)
                {
                    extractor.Release();
                    extractor = null;
                }
#endif
                //
                dec_exit();
            }
        }



        /**
         * Selects the video track, if any.
         *
         * @return the track index, or -1 if no video track is found.
         */
        private int selectTrack(MediaExtractor extractor)
        {
            // Select the first video track we find, ignore the rest.
            int numTracks = extractor.TrackCount;
            for (int i = 0; i < numTracks; i++)
            {
                MediaFormat format = extractor.GetTrackFormat(i);
                string mime = format.GetString(MediaFormat.KeyMime);
                if (mime.StartsWith("video/"))
                {
                    if (VERBOSE)
                    {
                        //Log.d(TAG, "Extractor selected track " + i + " (" + mime + "): " + format);
                    }
                    return i;
                }
            }

            return -1;
        }

        /**
         * Work loop.
         */
        static void doExtract(MediaExtractor extractor, int trackIndex, MediaCodec decoder,
                CodecOutputSurface outputSurface)
        {
            int TIMEOUT_USEC = 10000;
            ByteBuffer[]
            decoderInputBuffers = decoder.GetInputBuffers();
            MediaCodec.BufferInfo info = new MediaCodec.BufferInfo();
            int inputChunk = 0;
            int decodeCount = 0;
            long frameSaveTime = 0;

            bool outputDone = false;
            bool inputDone = false;
            while (!outputDone)
            {
                if (VERBOSE) Android.Util.Log.Debug(TAG, "loop");

                // Feed more data to the decoder.
                if (!inputDone)
                {
                    int inputBufIndex = decoder.DequeueInputBuffer(TIMEOUT_USEC);
                    if (inputBufIndex >= 0)
                    {
                        ByteBuffer inputBuf = decoderInputBuffers[inputBufIndex];
                        // Read the sample data into the ByteBuffer.  This neither respects nor
                        // updates inputBuf's position, limit, etc.
                        int chunkSize = extractor.ReadSampleData(inputBuf, 0);
                        if (chunkSize < 0)
                        {
                            // End of stream -- send empty frame with EOS flag set.
                            decoder.QueueInputBuffer(inputBufIndex, 0, 0, 0L,
                                    MediaCodecBufferFlags.EndOfStream);
                            inputDone = true;
                            //if (VERBOSE) Log.d(TAG, "sent input EOS");
                        }
                        else
                        {
                            if (extractor.SampleTrackIndex != trackIndex)
                            {
                                //Log.w(TAG, "WEIRD: got sample from track " +                            extractor.getSampleTrackIndex() + ", expected " + trackIndex);
                            }
                            long presentationTimeUs = extractor.SampleTime;
                            decoder.QueueInputBuffer(inputBufIndex, 0, chunkSize,
                                    presentationTimeUs, 0 /*flags*/);
                            if (VERBOSE)
                            {
                                //Log.d(TAG, "submitted frame " + inputChunk + " to dec, size=" +                  chunkSize);
                            }
                            inputChunk++;
                            extractor.Advance();
                        }
                    }
                    else
                    {
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "input buffer not available");
                    }
                }

                if (!outputDone)
                {
                    int decoderStatus = decoder.DequeueOutputBuffer(info, TIMEOUT_USEC);
                    if (decoderStatus == (int)MediaCodecInfoState.TryAgainLater)
                    {
                        // no output available yet
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "no output from decoder available");
                    }
                    else if (decoderStatus == (int)MediaCodecInfoState.OutputBuffersChanged)
                    {
                        // not important for us, since we're using Surface
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "decoder output buffers changed");
                    }
                    else if (decoderStatus == (int)MediaCodecInfoState.OutputFormatChanged)
                    {
                        MediaFormat newFormat = decoder.OutputFormat;
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "decoder output format changed: " + newFormat);
                        //
                        int width = newFormat.GetInteger(MediaFormat.KeyWidth);
                        int height = newFormat.GetInteger(MediaFormat.KeyHeight);
                        //
                        int cropLeft = newFormat.GetInteger("crop-left");
                        int cropRight = newFormat.GetInteger("crop-right");
                        int cropTop = newFormat.GetInteger("crop-top");
                        int cropBottom = newFormat.GetInteger("crop-bottom");
                        int cropWidth = cropRight - cropLeft + 1;
                        int cropHeight = cropBottom - cropTop + 1;
                        //
                        int ii = 0;
                    }
                    else if (decoderStatus < 0)
                    {
                        Android.Util.Log.Error(TAG, "unexpected result from decoder.dequeueOutputBuffer: " + decoderStatus);
                        goto errLabel;
                    }
                    else
                    { // decoderStatus >= 0
                      //if (VERBOSE) Log.d(TAG, "surface decoder given buffer " + decoderStatus +             " (size=" + info.size + ")");
                        if ((info.Flags & MediaCodecBufferFlags.EndOfStream) != 0)
                        {
                            //if (VERBOSE) Log.d(TAG, "output EOS");
                            outputDone = true;
                        }

                        bool doRender = (info.Size != 0);

                        // As soon as we call releaseOutputBuffer, the buffer will be forwarded
                        // to SurfaceTexture to convert to a texture.  The API doesn't guarantee
                        // that the texture will be available before the call returns, so we
                        // need to wait for the onFrameAvailable callback to fire.
                        decoder.ReleaseOutputBuffer(decoderStatus, doRender);
                        if (doRender)
                        {
                            if (VERBOSE)
                            {
                                Android.Util.Log.Debug(TAG, "awaiting decode of frame " + decodeCount);
                            }
                            //
                            outputSurface.awaitNewImage();

                            //
                            //ExtractMpegFramesTest.m_test_texId  =  outputSurface.mTextureRender.getTextureId();

                            //
                            outputSurface.drawImage(true);

                            //
                            if (decodeCount < MAX_FRAMES)
                            {
#if false
                                //File outputFile = new File(FILES_DIR,string.Format("frame-%02d.png", decodeCount));
                                File outputFile = new File(FILES_DIR, string.Format("frame-{0}.png", decodeCount));
                                long startWhen = JavaSystem.NanoTime();
                    outputSurface.saveFrame(outputFile.ToString());
                    frameSaveTime += Java.Lang.JavaSystem.NanoTime() - startWhen;
#endif
                            }
                            decodeCount++;
                        }
                    }
                }

                //
                continue;
            }

            int numSaved = (MAX_FRAMES < decodeCount) ? MAX_FRAMES : decodeCount;
        //Log.d(TAG, "Saving " + numSaved + " frames took " +    (frameSaveTime / numSaved / 1000) + " us per frame");

        errLabel:
            return;
        }


        //
        public unsafe int dec_init()
        {
            int iErr = -1;

            //

            //
            if (m_bTest)
            {
#if DEBUG
                Context context = qyMessengerApp.Droid.MyApplication.Context;

                //
                AssetManager assets = context.Assets;
                //
                AssetFileDescriptor afd = assets.OpenFd("vid.mp4");

                //
                m_test_extractor = new MediaExtractor();
                MediaExtractor extractor = m_test_extractor;
                extractor.SetDataSource(afd);
                int trackIndex = selectTrack(extractor);
                if (trackIndex < 0)
                {
                    //throw new RuntimeException("No video track found in " + inputFile);
                }
                extractor.SelectTrack(trackIndex);

                MediaFormat format = extractor.GetTrackFormat(trackIndex);
                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }

                //
                m_test_trackIndex = trackIndex;

                //
                m_dec_format = format;

                //
                m_iW = format.GetInteger(MediaFormat.KeyWidth);
                m_iH = format.GetInteger(MediaFormat.KeyHeight);
#endif
            }
            else
            {
                int width = this.m_iW;
                int height = this.m_iH;

                //
                string mime = Consts.MIME_TYPE_avc;
                //
                if (m_iFourcc_i == Consts.CONST_fourcc_HEVC)
                {
                    mime = Consts.MIME_TYPE_hevc;
                }



                MediaFormat format = MediaFormat.CreateVideoFormat(mime, width, height);

                ByteBuffer bb_extraData;
                int sizeof_spspps = 1024;
                bb_extraData = ByteBuffer.AllocateDirect(sizeof_spspps);
                if (bb_extraData == null) goto errLabel;
                //
                uint len = (uint)bb_extraData.Capacity();
                //                            
                if (0 != this.m_qoi.cfg.v.pf_transformGetSpsPps(this.m_qoi.cfg.v.uiTransformType_pParent, this.m_qoi.cfg.v.pParent_transform, this.m_qoi.cfg.v.iDecParam_index_pMems_from, (byte*)bb_extraData.GetDirectBufferAddress(), &len)) goto errLabel;

                //
                format.SetByteBuffer("csd-0", bb_extraData);

                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }

                //
                m_dec_format = format;

                //
            }

            iErr = 0;
        errLabel:

            string str;
            str = string.Format("decObj_mediaCodec.dec_init leaves, iErr {0}", iErr);
            qyFuncs.showInfo_open(0, null, str);

            //
            return iErr;
        }

        public int dec_exit()
        {
            if (m_bTest)
            {
#if DEBUG
                MediaExtractor extractor = m_test_extractor;
                if (extractor != null)
                {
                    extractor.Release();
                    extractor = null;
                }
                m_test_extractor = extractor;
#endif
            }
            else
            {

            }

            //
            return 0;
        }


        //
#if false
        static unsafe int old_dec_ReadSampleData(decObj_mediaCodec dm, ByteBuffer inputBuf, int offset, ref long presentationTimeUs)
        {
            int chunkSize = -1;

            //
            bool bDbg = false;
            string str;

            //
#if DEBUG
            //bDbg = true;
#endif

            //
            if (dm.m_bTest)
            {
#if DEBUG
                chunkSize = dm.m_test_extractor.ReadSampleData(inputBuf, offset);
                presentationTimeUs = dm.m_test_extractor.SampleTime;
#endif
            }
            else
            {
                uint uiToInMs = 0;

                uint nBytesRead = (uint)inputBuf.Capacity();
                uint uiSampleTimeInMs = 0;
                fixed (int* pold_lPktId = &dm.m_old_lPktId)
                {
                    if (0 != dm.m_qoi.cfg.v.dec.pf_smplBitstreamReader_ReadNextFrame_func(ref dm.m_qoi, pold_lPktId, uiToInMs, (byte*)(inputBuf.GetDirectBufferAddress()), &nBytesRead, &uiSampleTimeInMs))
                    {
                        str = string.Format("dec_ReadSmpleData：sampleBitstreamReader failed");
                        qyFuncs.showInfo_open(0, null, str);
                        //
                        goto errLabel;
                    }
                    if (nBytesRead == 0)
                    {
                        int ii = 0;
                    }
                }
                inputBuf.Limit((int)nBytesRead);
                chunkSize = (int)nBytesRead;
                presentationTimeUs = uiSampleTimeInMs * 1000;

                //
#if DEBUG
                if (bDbg)
                {
                    str = string.Format("dec_ReadSampleData called. chunkSize {0}", chunkSize);
                    qyFuncs.showInfo_open(0, null, str);

                    //
                }
#endif

            }

        errLabel:

            ///
            return chunkSize;
        }
#endif

        //
        static unsafe int dec_ReadSampleData(decObj_mediaCodec dm, ByteBuffer inputBuf, int offset, ref long presentationTimeUs)
        {
            int chunkSize = -1;

            //
            bool bDbg = false;
            string str;

            //
#if DEBUG
            //bDbg = true;
            //
            if (bDbg)
            {
                str = string.Format("decV.dec_readSampleData starts");
                qyFuncs.showInfo_open(0, null, str);
            }

#endif

            //
            if (dm.m_bTest)
            {
#if DEBUG
                chunkSize = dm.m_test_extractor.ReadSampleData(inputBuf, offset);
                presentationTimeUs = dm.m_test_extractor.SampleTime;
#endif
            }
            else
            {
                uint uiToInMs = 0;

                uint nBytesRead = (uint)inputBuf.Capacity();
                uint uiSampleTimeInMs = 0;
                //fixed (int* pold_lPktId = &dm.m_old_lPktId)
                ref int pold_lPktId = ref dm.m_old_lPktId;
                {
                    nBytesRead = (uint)dm.m_tmpDataBuf.Length;
                    //
                    if (0 != dm.m_qoi.cfg.v.dec.pf_smplBitstreamReader_ReadNextFrame_func(ref dm.m_qoi, ref pold_lPktId, uiToInMs, dm.m_tmpDataBuf, ref nBytesRead, ref uiSampleTimeInMs,dm))
                    {
                        str = string.Format("decV.dec_ReadSmpleData：sampleBitstreamReader failed");
                        qyFuncs.showInfo_open(0, null, str);
                        //
                        goto errLabel;
                    }
                    if (nBytesRead == 0)
                    {
                        int ii = 0;
                    }
                    //inputBuf.Limit((int)nBytesRead);
                    inputBuf.Clear();
                    inputBuf.Put(dm.m_tmpDataBuf, 0, (int)nBytesRead);


                }

                //
                chunkSize = (int)nBytesRead;
                presentationTimeUs = uiSampleTimeInMs * 1000;

                //
#if DEBUG
                if (bDbg)
                {
                    str = string.Format("dec_ReadSampleData called. chunkSize {0}", chunkSize);
                    qyFuncs.showInfo_open(0, null, str);

                    //
                }
#endif

            }

        errLabel:

#if DEBUG
            if (bDbg)
            {
                str = string.Format("decV.dec_ReadSampleData leaves. chunkSize {0}", chunkSize);
                qyFuncs.showInfo_open(0, null, str);

                //
            }
#endif

            ///
            return chunkSize;
        }


        //
        static int dec_advance(decObj_mediaCodec dm)
        {
            if (dm.m_bTest)
            {
#if DEBUG
                dm.m_test_extractor.Advance();
#endif
            }
            else
            {

            }

            //
            return 0;
        }


        //
        static void doDec(decObj_mediaCodec dm, MediaCodec decoder,
        CodecOutputSurface outputSurface)
        {
            int TIMEOUT_USEC = 10000;
            ByteBuffer[]
            decoderInputBuffers = decoder.GetInputBuffers();
            MediaCodec.BufferInfo info = new MediaCodec.BufferInfo();
            int inputChunk = 0;
            int decodeCount = 0;
            long frameSaveTime = 0;

            //
            qyFuncs.showInfo_open(0, null, "dec.doDec enters");

            //
            bool outputDone = false;
            bool inputDone = false;
            while (!outputDone)
            {
                if (VERBOSE) Android.Util.Log.Debug(TAG, "loop");

                //  2023/10/03
                if (inputDone)
                {
                    break;
                }

                // Feed more data to the decoder.
                if (!inputDone)
                {
                    int inputBufIndex = decoder.DequeueInputBuffer(TIMEOUT_USEC);
                    if (inputBufIndex >= 0)
                    {
                        ByteBuffer inputBuf = decoderInputBuffers[inputBufIndex];
                        // Read the sample data into the ByteBuffer.  This neither respects nor
                        // updates inputBuf's position, limit, etc.
                        //int chunkSize = extractor.ReadSampleData(inputBuf, 0);
                        int chunkSize = 0;
                        long presentationTimeUs = 0;
                        chunkSize = dec_ReadSampleData(dm, inputBuf, 0, ref presentationTimeUs);
                        //
                        if (chunkSize < 0)
                        {
                            // End of stream -- send empty frame with EOS flag set.
                            decoder.QueueInputBuffer(inputBufIndex, 0, 0, 0L,
                                    MediaCodecBufferFlags.EndOfStream);
                            inputDone = true;
                            //if (VERBOSE) Log.d(TAG, "sent input EOS");
                        }
                        else
                        {
#if false
                            if (extractor.SampleTrackIndex != trackIndex)
                            {
                                //Log.w(TAG, "WEIRD: got sample from track " +                            extractor.getSampleTrackIndex() + ", expected " + trackIndex);
                            }
                            long presentationTimeUs = extractor.SampleTime;
#endif
                            //
                            decoder.QueueInputBuffer(inputBufIndex, 0, chunkSize,
                                    presentationTimeUs, 0 /*flags*/);
                            if (VERBOSE)
                            {
                                //Log.d(TAG, "submitted frame " + inputChunk + " to dec, size=" +                  chunkSize);
                            }
                            inputChunk++;
                            //
                            //extractor.Advance();
                            dec_advance(dm);
                        }
                    }
                    else
                    {
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "input buffer not available");
                    }
                }

                if (!outputDone)
                {
                    int decoderStatus = decoder.DequeueOutputBuffer(info, TIMEOUT_USEC);
                    if (decoderStatus == (int)MediaCodecInfoState.TryAgainLater)
                    {
                        // no output available yet
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "no output from decoder available");
                    }
                    else if (decoderStatus == (int)MediaCodecInfoState.OutputBuffersChanged)
                    {
                        // not important for us, since we're using Surface
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "decoder output buffers changed");
                    }
                    else if (decoderStatus == (int)MediaCodecInfoState.OutputFormatChanged)
                    {
                        MediaFormat newFormat = decoder.OutputFormat;
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "decoder output format changed: " + newFormat);
                        //
                        int width = newFormat.GetInteger(MediaFormat.KeyWidth);
                        int height = newFormat.GetInteger(MediaFormat.KeyHeight);
                        //
                        int cropLeft = newFormat.GetInteger("crop-left");
                        int cropRight = newFormat.GetInteger("crop-right");
                        int cropTop = newFormat.GetInteger("crop-top");
                        int cropBottom = newFormat.GetInteger("crop-bottom");
                        int cropWidth = cropRight - cropLeft + 1;
                        int cropHeight = cropBottom - cropTop + 1;
                        //
                        int ii = 0;
                    }
                    else if (decoderStatus < 0)
                    {
                        Android.Util.Log.Error(TAG, "unexpected result from decoder.dequeueOutputBuffer: " + decoderStatus);
                        goto errLabel;
                    }
                    else
                    { // decoderStatus >= 0
                      //if (VERBOSE) Log.d(TAG, "surface decoder given buffer " + decoderStatus +             " (size=" + info.size + ")");
                        if ((info.Flags & MediaCodecBufferFlags.EndOfStream) != 0)
                        {
                            //if (VERBOSE) Log.d(TAG, "output EOS");
                            outputDone = true;
                        }

                        bool doRender = (info.Size != 0);

                        // As soon as we call releaseOutputBuffer, the buffer will be forwarded
                        // to SurfaceTexture to convert to a texture.  The API doesn't guarantee
                        // that the texture will be available before the call returns, so we
                        // need to wait for the onFrameAvailable callback to fire.
                        decoder.ReleaseOutputBuffer(decoderStatus, doRender);

                        //
                        QY_TRANSFORM pTransform = (QY_TRANSFORM)dm.m_qoi.cfg.v.pParent_transform;
                        if (0 != pTransform.video.u_qoi.myQoi.common.var.nDecFrame_needSkipped)
                        {
                            doRender = false;
                            pTransform.video.u_qoi.myQoi.common.var.nDecFrame_needSkipped--;
                        }

                        //
                        if (doRender)
                        {
                            if (VERBOSE)
                            {
                                Android.Util.Log.Debug(TAG, "awaiting decode of frame " + decodeCount);
                            }
                            //
                            outputSurface.awaitNewImage();

                            //
                            //ExtractMpegFramesTest.m_test_texId  =  outputSurface.mTextureRender.getTextureId();

                            //
                            outputSurface.drawImage(true);

                            //
                            if (decodeCount < MAX_FRAMES)
                            {
#if false
                                //File outputFile = new File(FILES_DIR,string.Format("frame-%02d.png", decodeCount));
                                File outputFile = new File(FILES_DIR, string.Format("frame-{0}.png", decodeCount));
                                long startWhen = JavaSystem.NanoTime();
                    outputSurface.saveFrame(outputFile.ToString());
                    frameSaveTime += Java.Lang.JavaSystem.NanoTime() - startWhen;
#endif
                            }
                            decodeCount++;

                            //
                            if  (  true  )
                            {
                                //qyFuncs.showInfo_open(0, "", "decObj_mediaCodec.doDec, decodeCount " + decodeCount);
                            }

                            //
                            int iSampleTimeInMs = (int)(info.PresentationTimeUs / 1000);
                            qyFuncs.chkFromPlayerIndex_v(dm.m_qoi.cfg.v.uiTransformType_pParent, dm.m_qoi.cfg.v.pParent_transform, iSampleTimeInMs);
                        }
                    }
                }

                //
                continue;
            }

            int numSaved = (MAX_FRAMES < decodeCount) ? MAX_FRAMES : decodeCount;
        //Log.d(TAG, "Saving " + numSaved + " frames took " +    (frameSaveTime / numSaved / 1000) + " us per frame");

        errLabel:

            string str;
            str = string.Format("dec.doDec leaves");
            qyFuncs.showInfo_open(0, null, str);

            //
            return;
        }












        /**
      * Holds state associated with a Surface used for MediaCodec decoder output.
      * <p>
      * The constructor for this class will prepare GL, create a SurfaceTexture,
      * and then create a Surface for that SurfaceTexture.  The Surface can be passed to
      * MediaCodec.configure() to receive decoder output.  When a frame arrives, we latch the
      * texture with updateTexImage(), then render the texture with GL to a pbuffer.
      * <p>
      * By default, the Surface will be using a BufferQueue in asynchronous mode, so we
      * can potentially drop frames.
      */
        public class CodecOutputSurface : Java.Lang.Object, SurfaceTexture.IOnFrameAvailableListener
        {
            private static string TAG = "CodecOutputSurface";

            private static bool VERBOSE = false;           // lots of logging

            //
            public object m_p0;
            //
            public EGLContext m_sharedEglContext1;
            //
            public int mWidth;
            public int mHeight;
            //
            Handler mHandler;

            //
            public STextureRender mTextureRender;
            public SurfaceTexture mSurfaceTexture;
            public Surface mSurface;

            private EGLDisplay mEGLDisplay = EGL14.EglNoDisplay;
            private EGLContext mEGLContext = EGL14.EglNoContext;
            private EGLSurface mEGLSurface = EGL14.EglNoSurface;

            //private Object mFrameSyncObject = new Object();     // guards mFrameAvailable
            //public System.Threading.EventWaitHandle evt_mFrameSyncObject = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, new string("frameSync"));
            public System.Threading.EventWaitHandle evt_mFrameSyncObject = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);

            //
            public bool mFrameAvailable;

            //
            private ByteBuffer mPixelBuf;                       // used by saveFrame()

            /**
             * Creates a CodecOutputSurface backed by a pbuffer with the specified dimensions.  The
             * new EGL context and surface will be made current.  Creates a Surface that can be passed
             * to MediaCodec.configure().
             */
            public CodecOutputSurface(object p0, EGLContext sharedEglContext, int width, int height, Handler handler)
            {
                //
                this.m_p0 = p0;
                this.m_sharedEglContext1 = sharedEglContext;
                if (width <= 0 || height <= 0)
                {
                    throw new IllegalArgumentException();
                }
                mWidth = width;
                mHeight = height;
                mHandler = handler;

                //
                eglSetup();
                makeCurrent();
                setup();
            }

            /**
             * Creates interconnected instances of TextureRender, SurfaceTexture, and Surface.
             */
            private void setup()
            {
                mTextureRender = new STextureRender(m_p0, this.mWidth, this.mHeight);
                mTextureRender.surfaceCreated();

                //if (VERBOSE) Log.d(TAG, "textureID=" + mTextureRender.getTextureId());
                mSurfaceTexture = new SurfaceTexture(mTextureRender.getTextureId());

                // This doesn't work if this object is created on the thread that CTS started for
                // these test cases.
                //
                // The CTS-created thread has a Looper, and the SurfaceTexture constructor will
                // create a Handler that uses it.  The "frame available" message is delivered
                // there, but since we're not a Looper-based thread we'll never see it.  For
                // this to do anything useful, CodecOutputSurface must be created on a thread without
                // a Looper, so that SurfaceTexture uses the main application Looper instead.
                //
                // Java language note: passing "this" out of a constructor is generally unwise,
                // but we should be able to get away with it here.
                if (this.mHandler != null) mSurfaceTexture.SetOnFrameAvailableListener(this, this.mHandler);
                else mSurfaceTexture.SetOnFrameAvailableListener(this);

                //
                mSurface = new Surface(mSurfaceTexture);

                mPixelBuf = ByteBuffer.AllocateDirect(mWidth * mHeight * 4);
                mPixelBuf.Order(ByteOrder.LittleEndian);
            }

            /**
             * Prepares EGL.  We want a GLES 2.0 context and a surface that supports pbuffer.
             */
            private void eglSetup()
            {
                mEGLDisplay = EGL14.EglGetDisplay(EGL14.EglDefaultDisplay);
                if (mEGLDisplay == EGL14.EglNoDisplay)
                {
                    throw new RuntimeException("unable to get EGL14 display");
                }
                int[] version = new int[2];
                if (!EGL14.EglInitialize(mEGLDisplay, version, 0, version, 1))
                {
                    mEGLDisplay = null;
                    throw new RuntimeException("unable to initialize EGL14");
                }

                // Configure EGL for pbuffer and OpenGL ES 2.0, 24-bit RGB.
                int[] attribList = {
                    EGL14.EglRedSize, 8,
                    EGL14.EglGreenSize, 8,
                    EGL14.EglBlueSize, 8,
                    EGL14.EglAlphaSize, 8,
                    EGL14.EglRenderableType, EGL14.EglOpenglEs2Bit,
                    EGL14.EglSurfaceType, EGL14.EglPbufferBit,
                    EGL14.EglNone
            };
                EGLConfig[] configs = new EGLConfig[1];
                int[] numConfigs = new int[1];
                if (!EGL14.EglChooseConfig(mEGLDisplay, attribList, 0, configs, 0, configs.Length,
                        numConfigs, 0))
                {
                    throw new RuntimeException("unable to find RGB888+recordable ES2 EGL config");
                }

                // Configure context for OpenGL ES 2.0.
                int[] attrib_list = {
                    EGL14.EglContextClientVersion, 2,
                    EGL14.EglNone
            };
                EGLContext share_context = EGL14.EglNoContext;
                //
                share_context = this.m_sharedEglContext1;
                //
                mEGLContext = EGL14.EglCreateContext(mEGLDisplay, configs[0], share_context,
                        attrib_list, 0);
                checkEglError("eglCreateContext");
                if (mEGLContext == null)
                {
                    throw new RuntimeException("null context");
                }

                // Create a pbuffer surface.
                int[] surfaceAttribs = {
                    EGL14.EglWidth, mWidth,
                    EGL14.EglHeight, mHeight,
                    EGL14.EglNone
            };
                mEGLSurface = EGL14.EglCreatePbufferSurface(mEGLDisplay, configs[0], surfaceAttribs, 0);
                checkEglError("eglCreatePbufferSurface");
                if (mEGLSurface == null)
                {
                    throw new RuntimeException("surface was null");
                }
            }

            /**
             * Discard all resources held by this class, notably the EGL context.
             */
            public void release()
            {
                //
                string str;

                str = string.Format("decObj_mediaCodec.CodecOutputSurface.release enters");
                qyFuncs.showInfo_open(0, null, str);

                //
                if (mEGLDisplay != EGL14.EglNoDisplay)
                {
                    EGL14.EglDestroySurface(mEGLDisplay, mEGLSurface);
                    EGL14.EglDestroyContext(mEGLDisplay, mEGLContext);
                    EGL14.EglReleaseThread();
                    EGL14.EglTerminate(mEGLDisplay);
                }
                mEGLDisplay = EGL14.EglNoDisplay;
                mEGLContext = EGL14.EglNoContext;
                mEGLSurface = EGL14.EglNoSurface;

                if (mSurface != null)
                {
                    mSurface.Release();
                }

                // this causes a bunch of warnings that appear harmless but might confuse someone:
                //  W BufferQueue: [unnamed-3997-2] cancelBuffer: BufferQueue has been abandoned!
                if (mSurfaceTexture != null)
                {
                    mSurfaceTexture.Release();
                }

                //
                if (mTextureRender != null)
                {
                    mTextureRender.Release();
                }

                //
                mTextureRender = null;
                mSurface = null;
                mSurfaceTexture = null;

                //
                str = string.Format("decObj_mediaCodec.CodecOutputSurface.release leaves");
                qyFuncs.showInfo_open(0, null, str);


            }

            /**
             * Makes our EGL context and surface current.
             */
            public void makeCurrent()
            {
                if (!EGL14.EglMakeCurrent(mEGLDisplay, mEGLSurface, mEGLSurface, mEGLContext))
                {
                    throw new RuntimeException("eglMakeCurrent failed");
                }
            }

            /**
             * Returns the Surface.
             */
            public Surface getSurface()
            {
                return mSurface;
            }

            /**
             * Latches the next buffer into the texture.  Must be called from the thread that created
             * the CodecOutputSurface object.  (More specifically, it must be called on the thread
             * with the EGLContext that contains the GL texture object used by SurfaceTexture.)
             */
            public void awaitNewImage()
            {
                int TIMEOUT_MS = 2500;
                //TIMEOUT_MS = 250000;

                //
                bool bDbg = false;
#if DEBUG
                //bDbg = true;
#endif
                string str;
                if (bDbg)
                {
                    str = string.Format("awaitNewImage, mFrameAvailable {0}", mFrameAvailable);
                    qyFuncs.traceLog(str);
                }

                //synchronized(mFrameSyncObject)
                {
                    while (!mFrameAvailable)
                    {
                        try
                        {
                            // Wait for onFrameAvailable() to signal us.  Use a timeout to avoid
                            // stalling the test if it doesn't arrive.
                            //mFrameSyncObject.wait(TIMEOUT_MS);
                            uint dwTickCnt0 = qyFuncs.myGetTickCount(null);
                            qyFuncs.myWaitOne(this.evt_mFrameSyncObject, TIMEOUT_MS);
                            //Thread.Sleep((uint)TIMEOUT_MS);
                            uint dwTickCnt1 = qyFuncs.myGetTickCount(null);
                            int iDiffInMs = (int)(dwTickCnt1 - dwTickCnt0);
                            if (iDiffInMs < 500)
                            {
                                if (!mFrameAvailable)
                                {
                                    continue;
                                }
                            }
                            if (!mFrameAvailable)
                            {
                                // TODO: if "spurious wakeup", continue while loop
                                throw new RuntimeException("frame wait timed out");
                            }
                        }
                        catch (InterruptedException ie)
                        {
                            // shouldn't happen
                            throw new RuntimeException(ie);
                        }
                        //
                        continue;
                    }
                    //
                    if (bDbg)
                    {
                        str = string.Format("awaitNewImage, mFrameAvailable {0}", mFrameAvailable);
                        qyFuncs.traceLog(str);
                    }
                    //
                    mFrameAvailable = false;
                }

                // Latch the data.
                mTextureRender.checkGlError("before updateTexImage");
                mSurfaceTexture.UpdateTexImage();
            }

            /**
             * Draws the data from SurfaceTexture onto the current EGL surface.
             *
             * @param invert if set, render the image with Y inverted (0,0 in top left)
             */
            public void drawImage(bool invert)
            {
                mTextureRender.drawFrame(mSurfaceTexture, invert);
            }

            // SurfaceTexture callback
            public void OnFrameAvailable(SurfaceTexture st)
            {
                myOnFrameAvailable(st);
                return;
            }

            //
            //public abstract void myOnFrameAvailable(SurfaceTexture st);
            public void myOnFrameAvailable(SurfaceTexture st)
            {
                bool bDbg = false;
                string str;

                //
#if DEBUG
                //bDbg = true;
#endif

                //
                if (bDbg)
                {
                    str = string.Format("new frame available");
                    qyFuncs.showInfo_open(0, null, str);
                }

                //synchronized(mFrameSyncObject) 
                {
                    if (mFrameAvailable)
                    {
                        throw new RuntimeException("mFrameAvailable already set, frame could be dropped");
                    }
                    mFrameAvailable = true;
                    //mFrameSyncObject.notifyAll();
                    qyFuncs.mySetEvent(this.evt_mFrameSyncObject);
                }
            }




            /**
             * Saves the current frame to disk as a PNG image.
             */
            public void saveFrame(string filename)
            {
                // glReadPixels gives us a ByteBuffer filled with what is essentially big-endian RGBA
                // data (i.e. a byte of red, followed by a byte of green...).  To use the Bitmap
                // constructor that takes an int[] array with pixel data, we need an int[] filled
                // with little-endian ARGB data.
                //
                // If we implement this as a series of buf.get() calls, we can spend 2.5 seconds just
                // copying data around for a 720p frame.  It's better to do a bulk get() and then
                // rearrange the data in memory.  (For comparison, the PNG compress takes about 500ms
                // for a trivial frame.)
                //
                // So... we set the ByteBuffer to little-endian, which should turn the bulk IntBuffer
                // get() into a straight memcpy on most Android devices.  Our ints will hold ABGR data.
                // Swapping B and R gives us ARGB.  We need about 30ms for the bulk get(), and another
                // 270ms for the color swap.
                //
                // We can avoid the costly B/R swap here if we do it in the fragment shader (see
                // http://stackoverflow.com/questions/21634450/ ).
                //
                // Having said all that... it turns out that the Bitmap#copyPixelsFromBuffer()
                // method wants RGBA pixels, not ARGB, so if we create an empty bitmap and then
                // copy pixel data in we can avoid the swap issue entirely, and just copy straight
                // into the Bitmap from the ByteBuffer.
                //
                // Making this even more interesting is the upside-down nature of GL, which means
                // our output will look upside-down relative to what appears on screen if the
                // typical GL conventions are used.  (For ExtractMpegFrameTest, we avoid the issue
                // by inverting the frame when we render it.)
                //
                // Allocating large buffers is expensive, so we really want mPixelBuf to be
                // allocated ahead of time if possible.  We still get some allocations from the
                // Bitmap / PNG creation.

                mPixelBuf.Rewind();
                GLES20.GlReadPixels(0, 0, mWidth, mHeight, GLES20.GlRgba, GLES20.GlUnsignedByte,
                            mPixelBuf);

                BufferedOutputStream bos = null;
                System.IO.FileStream fos = null;
#if true
                try
                {
                    //
                    fos = new System.IO.FileStream(filename, System.IO.FileMode.OpenOrCreate);
                    //bos = new BufferedOutputStream(new FileOutputStream(filename));
                    bos = new BufferedOutputStream(fos);
                    Bitmap bmp = Bitmap.CreateBitmap(mWidth, mHeight, Bitmap.Config.Argb8888);
                    mPixelBuf.Rewind();
                    bmp.CopyPixelsFromBuffer(mPixelBuf);
                    //bmp.Compress(Bitmap.CompressFormat.Png, 90, bos);
                    bmp.Compress(Bitmap.CompressFormat.Png, 90, fos);
                    bmp.Recycle();
                }
                finally
                {
                    if (bos != null) bos.Close();
                    if (fos != null) fos.Close();
                }
#endif
                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Saved " + mWidth + "x" + mHeight + " frame as '" + filename + "'");
                }
            }

            /**
             * Checks for EGL errors.
             */
            public static void checkEglError(string msg)
            {
                int error;
                if ((error = EGL14.EglGetError()) != EGL14.EglSuccess)
                {
                    qyFuncs.traceLog("zzzzzccccccccccccccccccccccccccccccccc");

                    throw new RuntimeException(msg + ": EGL error: 0x" + Integer.ToHexString(error));
                }
            }

        }


    }

    //
#if false
    public class invalid_CodecOutputSurface:TmpOutputSurface
    {
        public CodecOutputSurface(decObj_mediaCodec dm, EGLContext sharedEglContext, int width, int height):base(dm,sharedEglContext,width,height,dm.mBackgroundHandler)
        {

        }

        //
        public override void myOnFrameAvailable(SurfaceTexture st)
        {
            bool bDbg = false;
            string str;

            //
#if DEBUG
            //bDbg = true;
#endif

            //
            if (bDbg)
            {
                str = string.Format("new frame available");
                qyFuncs.showInfo_open(0, null, str);
            }

            //synchronized(mFrameSyncObject) 
            {
                if (mFrameAvailable)
                {
                    throw new RuntimeException("mFrameAvailable already set, frame could be dropped");
                }
                mFrameAvailable = true;
                //mFrameSyncObject.notifyAll();
                qyFuncs.mySetEvent(this.evt_mFrameSyncObject);
            }
        }


    }
#endif

    //
    public class TMP_frameBuffer
    {
        //
        private int[] mFrameBuffer = new int[1];
        //
        public int[] mTexture = new int[1];

        //
        public bool bDone_createEnv;

        //
        public void createEnv(int width, int height, string hint)
        {
            string str;
            if (hint == null) hint = "";
            str = string.Format("TMP_frameBuffer.createEnv {0}x{1}, {2}", width, height, hint);
            qyFuncs.showInfo_open(0, null, str);


            // 1. 创建FrameBuffer
            GLES20.GlGenFramebuffers(1, mFrameBuffer, 0);

            // 2.1 生成纹理对象
            GLES20.GlGenTextures(1, mTexture, 0);
            //
            qyFuncs.showInfo_open(0, "", "createEnv.GlGenTextures 1");

            // 2.2 绑定纹理对象
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTexture[0]);
            // 2.3 设置纹理对象的相关信息：颜色模式、大小
            GLES20.GlTexImage2D(GLES20.GlTexture2d, 0, GLES20.GlRgba,
                    width, height,
                    0, GLES20.GlRgba, GLES20.GlUnsignedByte, null);
            // 2.4 纹理过滤参数设置
            GLES20.GlTexParameterf(GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlNearest);
            GLES20.GlTexParameterf(GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlLinear);
            GLES20.GlTexParameterf(GLES20.GlTexture2d, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
            GLES20.GlTexParameterf(GLES20.GlTexture2d, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);
            // 2.5 解绑当前纹理，避免后续无关的操作影响了纹理内容
            GLES20.GlBindTexture(GLES20.GlTexture2d, 0);
        }

        public void bindFrameBufferInfo()
        {
            // 1. 绑定FrameBuffer到当前的绘制环境上
            GLES20.GlBindFramebuffer(GLES20.GlFramebuffer, mFrameBuffer[0]);
            // 2. 将纹理对象挂载到FrameBuffer上，存储颜色信息
            GLES20.GlFramebufferTexture2D(GLES20.GlFramebuffer, GLES20.GlColorAttachment0,
                    GLES20.GlTexture2d, mTexture[0], 0);
        }

        public void unbindFrameBufferInfo()
        {
            // 解绑FrameBuffer
            GLES20.GlBindFramebuffer(GLES20.GlFramebuffer, 0);

        }

        public void deleteEnv()
        {
            //
#if false
                Bitmap bmp = qyFuncs_Droid.saveTexture(mTexture[0], 640, 360);
                GuiShare.pf_dumpBmp(bmp, "dec.saveTex");
#endif


            //
            GLES20.GlDeleteFramebuffers(1, mFrameBuffer, 0);
            GLES20.GlDeleteTextures(1, mTexture, 0);
        }

    };


    //
    /**
  * Code for rendering a texture onto a surface using OpenGL ES 2.0.
  */
    public class STextureRender
    {
        private static string TAG = "STextureRender";

        //
        public object m_p0;
        public int m_iW, m_iH;

        //
        public TMP_frameBuffer tmpFrameBuffer = new TMP_frameBuffer();

        //
        private static int FLOAT_SIZE_BYTES = 4;
        private static int TRIANGLE_VERTICES_DATA_STRIDE_BYTES = 5 * FLOAT_SIZE_BYTES;
        private static int TRIANGLE_VERTICES_DATA_POS_OFFSET = 0;
        private static int TRIANGLE_VERTICES_DATA_UV_OFFSET = 3;
        private float[] mTriangleVerticesData = {
                // X, Y, Z, U, V
                -1.0f, -1.0f, 0, 0.0f, 0.0f,
                 1.0f, -1.0f, 0, 1.0f, 0.0f,
                -1.0f,  1.0f, 0, 0.0f, 1.0f,
                 1.0f,  1.0f, 0, 1.0f, 1.0f,
        };

        private FloatBuffer mTriangleVertices;

        private static string VERTEX_SHADER =
                    "uniform mat4 uMVPMatrix;\n" +
                    "uniform mat4 uSTMatrix;\n" +
                    "attribute vec4 aPosition;\n" +
                    "attribute vec4 aTextureCoord;\n" +
                    "varying vec2 vTextureCoord;\n" +
                    "void main() {\n" +
                    "    gl_Position = uMVPMatrix * aPosition;\n" +
                    "    vTextureCoord = (uSTMatrix * aTextureCoord).xy;\n" +
                    "}\n";

        private static string FRAGMENT_SHADER =
                "#extension GL_OES_EGL_image_external : require\n" +
                "precision mediump float;\n" +      // highp here doesn't seem to matter
                "varying vec2 vTextureCoord;\n" +
                "uniform samplerExternalOES sTexture;\n" +
                "void main() {\n" +
                "    gl_FragColor = texture2D(sTexture, vTextureCoord);\n" +
                "}\n";

        private float[] mMVPMatrix = new float[16];
        private float[] mSTMatrix = new float[16];

        private int mProgram;
        //
        private int mVertexShader = -1;
        private int mFragmentShader = -1;
        //
        private int mTextureID = -12345;
        private int muMVPMatrixHandle;
        private int muSTMatrixHandle;
        private int maPositionHandle;
        private int maTextureHandle;

        public STextureRender(object p0, int iW, int iH)
        {
            //
            m_p0 = p0;
            m_iW = iW;
            m_iH = iH;
            //
            mTriangleVertices = ByteBuffer.AllocateDirect(
                    mTriangleVerticesData.Length * FLOAT_SIZE_BYTES)
                    .Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleVertices.Put(mTriangleVerticesData).Position(0);

            Android.Opengl.Matrix.SetIdentityM(mSTMatrix, 0);


            //
            return;
        }

        public int getTextureId()
        {
            return mTextureID;
        }

        /**
         * Draws the external texture in SurfaceTexture onto the current EGL surface.
         */
#if false
            public void old_drawFrame(SurfaceTexture st, bool invert)
            {
                checkGlError("onDrawFrame start");
                st.GetTransformMatrix(mSTMatrix);
                if (invert)
                {
                    mSTMatrix[5] = -mSTMatrix[5];
                    mSTMatrix[13] = 1.0f - mSTMatrix[13];
                }

                // (optional) clear to green so we can see if we're failing to set pixels
                GLES20.GlClearColor(0.0f, 1.0f, 0.0f, 1.0f);
                GLES20.GlClear(GLES20.GlColorBufferBit);

                GLES20.GlUseProgram(mProgram);
                checkGlError("glUseProgram");

                GLES20.GlActiveTexture(GLES20.GlTexture0);
                GLES20.GlBindTexture(GLES11Ext.GlTextureExternalOes, mTextureID);

                mTriangleVertices.Position(TRIANGLE_VERTICES_DATA_POS_OFFSET);
                GLES20.GlVertexAttribPointer(maPositionHandle, 3, GLES20.GlFloat, false,
                        TRIANGLE_VERTICES_DATA_STRIDE_BYTES, mTriangleVertices);
                checkGlError("glVertexAttribPointer maPosition");
                GLES20.GlEnableVertexAttribArray(maPositionHandle);
                checkGlError("glEnableVertexAttribArray maPositionHandle");

                mTriangleVertices.Position(TRIANGLE_VERTICES_DATA_UV_OFFSET);
                GLES20.GlVertexAttribPointer(maTextureHandle, 2, GLES20.GlFloat, false,
                        TRIANGLE_VERTICES_DATA_STRIDE_BYTES, mTriangleVertices);
                checkGlError("glVertexAttribPointer maTextureHandle");
                GLES20.GlEnableVertexAttribArray(maTextureHandle);
                checkGlError("glEnableVertexAttribArray maTextureHandle");

                Android.Opengl.Matrix.SetIdentityM(mMVPMatrix, 0);
                GLES20.GlUniformMatrix4fv(muMVPMatrixHandle, 1, false, mMVPMatrix, 0);
                GLES20.GlUniformMatrix4fv(muSTMatrixHandle, 1, false, mSTMatrix, 0);

                GLES20.GlDrawArrays(GLES20.GlTriangleStrip, 0, 4);
                checkGlError("glDrawArrays");

                GLES20.GlBindTexture(GLES11Ext.GlTextureExternalOes, 0);
            }
#endif




        //
        public void drawFrame(SurfaceTexture st, bool invert)
        {
            string str;

            //
            checkGlError("onDrawFrame start");
            st.GetTransformMatrix(mSTMatrix);
            if (invert)
            {
                mSTMatrix[5] = -mSTMatrix[5];
                mSTMatrix[13] = 1.0f - mSTMatrix[13];
            }

            //
            // 1. 创建FrameBuffer、纹理对象
#if false
                int width = 640;
                int height = 360;
                createEnv(640,360);
#endif
            // 2. 配置FrameBuffer相关的绘制存储信息，并且绑定到当前的绘制环境上
            this.tmpFrameBuffer.bindFrameBufferInfo();
            // 3. 更新视图区域
            GLES20.GlViewport(0, 0, m_iW, m_iH);


            // (optional) clear to green so we can see if we're failing to set pixels
            GLES20.GlClearColor(0.0f, 1.0f, 0.0f, 1.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);

            GLES20.GlUseProgram(mProgram);
            checkGlError("glUseProgram");

            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES11Ext.GlTextureExternalOes, mTextureID);

            mTriangleVertices.Position(TRIANGLE_VERTICES_DATA_POS_OFFSET);
            GLES20.GlVertexAttribPointer(maPositionHandle, 3, GLES20.GlFloat, false,
                    TRIANGLE_VERTICES_DATA_STRIDE_BYTES, mTriangleVertices);
            checkGlError("glVertexAttribPointer maPosition");
            GLES20.GlEnableVertexAttribArray(maPositionHandle);
            checkGlError("glEnableVertexAttribArray maPositionHandle");

            mTriangleVertices.Position(TRIANGLE_VERTICES_DATA_UV_OFFSET);
            GLES20.GlVertexAttribPointer(maTextureHandle, 2, GLES20.GlFloat, false,
                    TRIANGLE_VERTICES_DATA_STRIDE_BYTES, mTriangleVertices);
            checkGlError("glVertexAttribPointer maTextureHandle");
            GLES20.GlEnableVertexAttribArray(maTextureHandle);
            checkGlError("glEnableVertexAttribArray maTextureHandle");

            Android.Opengl.Matrix.SetIdentityM(mMVPMatrix, 0);
            GLES20.GlUniformMatrix4fv(muMVPMatrixHandle, 1, false, mMVPMatrix, 0);
            GLES20.GlUniformMatrix4fv(muSTMatrixHandle, 1, false, mSTMatrix, 0);

            GLES20.GlDrawArrays(GLES20.GlTriangleStrip, 0, 4);
            checkGlError("glDrawArrays");

            GLES20.GlBindTexture(GLES11Ext.GlTextureExternalOes, 0);

            //
            this.tmpFrameBuffer.unbindFrameBufferInfo();

            //
#if DEBUG
#if false
            decObj_mediaCodec.m_test_texture = this.tmpFrameBuffer.mTexture[0];
            //
            decObj_mediaCodec.m_tex_w = m_iW;
            decObj_mediaCodec.m_tex_h = m_iH;
#endif

            //
            if (false)
            {
                str = string.Format("dec.stRender.drawFrame.");
                qyFuncs.showInfo_open(0, null, str);
            }
#endif
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            MainActivity mainActivity = (MainActivity)(pQyMc.gui.hMainWnd);
            decObj_mediaCodec dm = (decObj_mediaCodec)this.m_p0;
            //            
            {
                //
                updateTexInfo(pProcInfo, dm, this.tmpFrameBuffer.mTexture[0], m_iW, m_iH, dm.m_qoi.cfg.v.iTaskId);

                int id = this.tmpFrameBuffer.mTexture[0];
                mainActivity.textureIw= m_iW;
                mainActivity.textureIh= m_iH;
                mainActivity.textureId = id;

                //
                //qyFuncs.traceLog("TAG", "updateTexInfo  mTexture[0] :"+ id);
                int ii = 0;
                //ref TMP_texes texes = ref pProcInfo.av.texes;

                //
#if false
                //
                ref TMP_tex_info tti = ref texes.tti_peer;
                       
                    //                    
                    if (tti.texId != this.tmpFrameBuffer.mTexture[0])                      
                    {
                            tti.texId = this.tmpFrameBuffer.mTexture[0];                       
                    }                        
                    if (tti.iW != m_iW)                    
                    {
                            tti.iW = m_iW;
                     
                    }                        
                    if (tti.iH != m_iH)                     
                    {
                            tti.iH = m_iH;                        
                    }

                    //
                    //tti.iTaskId = dm.m_qoi.cfg.v.iTaskId;
#endif

            }




#if false
                // 7. 删除FrameBuffer、纹理对象
                deleteEnv();
#endif

        }


        //
        public int updateTexInfo(CCtxQmc pProcInfo, decObj_mediaCodec dm, int texId, int w, int h, int iTaskId)
        {
            int iErr = -1;



            if (dm.m_qoi.cfg.v.uiTransformType_pParent == Consts.CONST_uiTransformType_dec)
            {
                QY_TRANSFORM pTransform = (QY_TRANSFORM)dm.m_qoi.cfg.v.pParent_transform;
                if (pTransform.index_activeMems_from < 0 || pTransform.index_activeMems_from >= pProcInfo.av.texes.taskAvTexes.tti_peers.Length) goto errLabel;
                ref TMP_tex_info tti = ref pProcInfo.av.texes.taskAvTexes.tti_peers[pTransform.index_activeMems_from];


                //
                if (tti.texId != texId)
                {
                    tti.texId = texId;
                }
                if (tti.iW != w)
                {
                    tti.iW = w;
                }
                if (tti.iH != h)
                {
                    tti.iH = h;
                }
#if false
                if (tti.iTaskId != iTaskId)
                {
                    tti.iTaskId = iTaskId;
                }
                if (tti.uiObjType != activeMem.avStream.obj.resObj.uiObjType)
                {
                    tti.uiObjType = activeMem.avStream.obj.resObj.uiObjType;
                }
#endif



                ///
                int ii = 0;


            }


            iErr = 0;
        errLabel:
            return iErr;
        }




        /**
         * Initializes GL state.  Call this after the EGL surface has been created and made current.
         */
        public void surfaceCreated()
        {
            mProgram = createProgram(VERTEX_SHADER, FRAGMENT_SHADER, ref this.mVertexShader, ref this.mFragmentShader);
            if (mProgram == 0)
            {
                throw new RuntimeException("failed creating program");
            }

            maPositionHandle = GLES20.GlGetAttribLocation(mProgram, "aPosition");
            checkLocation(maPositionHandle, "aPosition");
            maTextureHandle = GLES20.GlGetAttribLocation(mProgram, "aTextureCoord");
            checkLocation(maTextureHandle, "aTextureCoord");

            muMVPMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "uMVPMatrix");
            checkLocation(muMVPMatrixHandle, "uMVPMatrix");
            muSTMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "uSTMatrix");
            checkLocation(muSTMatrixHandle, "uSTMatrix");

            int[] textures = new int[1];
            GLES20.GlGenTextures(1, textures, 0);
            //
            qyFuncs.showInfo_open(0, "", "surfaceCreated glGenTextures 1");

            //
            mTextureID = textures[0];
            GLES20.GlBindTexture(GLES11Ext.GlTextureExternalOes, mTextureID);
            int i = mTextureID;
            checkGlError("glBindTexture mTextureID");

            GLES20.GlTexParameterf(GLES11Ext.GlTextureExternalOes, GLES20.GlTextureMinFilter,
                    GLES20.GlNearest);
            GLES20.GlTexParameterf(GLES11Ext.GlTextureExternalOes, GLES20.GlTextureMagFilter,
                    GLES20.GlLinear);
            GLES20.GlTexParameteri(GLES11Ext.GlTextureExternalOes, GLES20.GlTextureWrapS,
                    GLES20.GlClampToEdge);
            GLES20.GlTexParameteri(GLES11Ext.GlTextureExternalOes, GLES20.GlTextureWrapT,
                    GLES20.GlClampToEdge);
            checkGlError("glTexParameter");
        }

        public void Release()
        {
            if (this.mProgram > 0)
            {
                GLES20.GlDeleteProgram(this.mProgram);
                mProgram = -1;
            }
            if (this.mVertexShader > 0)
            {
                GLES20.GlDeleteShader(this.mVertexShader);
                this.mVertexShader = -1;
            }
            if (this.mFragmentShader > 0)
            {
                GLES20.GlDeleteShader(this.mFragmentShader);
                this.mFragmentShader = -1;
            }
            if (this.mTextureID > 0)
            {                //
                int[] texes = new int[1];
                texes[0] = mTextureID;
                GLES20.GlDeleteTextures(1, texes, 0);
                mTextureID = 0;
            }


            //int []
            return;
        }

        /**
         * Replaces the fragment shader.  Pass in null to reset to default.
         */
        public void changeFragmentShader(string fragmentShader)
        {
            if (fragmentShader == null)
            {
                fragmentShader = FRAGMENT_SHADER;
            }
            GLES20.GlDeleteProgram(mProgram);
            mProgram = createProgram(VERTEX_SHADER, fragmentShader, ref this.mVertexShader, ref this.mFragmentShader);
            if (mProgram == 0)
            {
                throw new RuntimeException("failed creating program");
            }
        }

        private int loadShader(int shaderType, string source)
        {
            int shader = GLES20.GlCreateShader(shaderType);
            checkGlError("glCreateShader type=" + shaderType);
            GLES20.GlShaderSource(shader, source);
            GLES20.GlCompileShader(shader);
            int[] compiled = new int[1];
            GLES20.GlGetShaderiv(shader, GLES20.GlCompileStatus, compiled, 0);
            if (compiled[0] == 0)
            {
                //Log.e(TAG, "Could not compile shader " + shaderType + ":");
                //Log.e(TAG, " " + GLES20.glGetShaderInfoLog(shader));
                GLES20.GlDeleteShader(shader);
                shader = 0;
            }
            return shader;
        }

        private int createProgram(string vertexSource, string fragmentSource, ref int vertexShader_o, ref int pixShader_o)
        {
            int vertexShader = loadShader(GLES20.GlVertexShader, vertexSource);
            if (vertexShader == 0)
            {
                return 0;
            }
            int pixelShader = loadShader(GLES20.GlFragmentShader, fragmentSource);
            if (pixelShader == 0)
            {
                return 0;
            }

            //
            vertexShader_o = vertexShader;
            pixShader_o = pixShader_o;

            //
            int program = GLES20.GlCreateProgram();
            if (program == 0)
            {
                //Log.e(TAG, "Could not create program");
            }
            GLES20.GlAttachShader(program, vertexShader);
            checkGlError("glAttachShader");
            GLES20.GlAttachShader(program, pixelShader);
            checkGlError("glAttachShader");
            GLES20.GlLinkProgram(program);
            int[] linkStatus = new int[1];
            GLES20.GlGetProgramiv(program, GLES20.GlLinkStatus, linkStatus, 0);
            if (linkStatus[0] != GLES20.GlTrue)
            {
                //Log.e(TAG, "Could not link program: ");
                //Log.e(TAG, GLES20.glGetProgramInfoLog(program));
                GLES20.GlDeleteProgram(program);
                program = 0;
            }



            //
            return program;
        }

        public void checkGlError(string op)
        {
            int error;
            while ((error = GLES20.GlGetError()) != GLES20.GlNoError)
            {
                Android.Util.Log.Error(TAG, op + ": glError " + error);
                throw new RuntimeException(op + ": glError " + error);
            }
        }

        public static void checkLocation(int location, string label)
        {
            if (location < 0)
            {
                throw new RuntimeException("Unable to locate '" + label + "' in program");
            }
        }
    }




    //
}

