using Android;
using Android.Content;
using Android.Content.Res;
using File = Java.IO.File;
using Android.Media;
using Thread = Java.Lang.Thread;
using Java.Lang;
using Java.Nio;
using Android.App;
using System;


namespace qy.Droid
{

    //
    public class decObj_a_mediaCodec
    {
        private static string TAG = "decObj_a_mediaCodec";
        private static bool VERBOSE = false;//false;           // lots of logging

        // where to find files (note: requires WRITE_EXTERNAL_STORAGE permission)
        //private static  File FILES_DIR = Android.OS.Environment.GetExternalStorageDirectory();
        private static File FILES_DIR = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
        private static string INPUT_FILE = "source.mp4";
        private static int MAX_FRAMES = 100;//10;       // stop extracting after this many

        //
        public QOI_common m_qoi;

        //
        public TRANSFORM_AUDIO_procInfo_decInfo m_decInfo = new TRANSFORM_AUDIO_procInfo_decInfo();


        //
        public bool m_bEnd;


        //
        //
        public int m_old_lPktId;		//  用来防止getNextPkt出错，得到的是重复包

        //
        public bool m_bTest;

        //
        public decObj_a_mediaCodec_Wrapper m_wrapper;
        public Thread m_th;

        //
        byte[] m_tmpDataBuf = new byte[Consts.bufSize_transferAudioData];

        //
#if DEBUG
        MediaExtractor m_test_extractor = null;
        int m_test_trackIndex = 0;
#endif
        //
        MediaFormat m_dec_format = null;


        /** test entry point */
        public void testExtractMpegFrames(Activity mainActivity)
        {
            //this.mainActivity = mainActivity;
            decObj_a_mediaCodec_Wrapper.runTest(this);
        }

        /**
         * Wraps extractMpegFrames().  This is necessary because SurfaceTexture will try to use
         * the looper in the current thread if one exists, and the CTS tests create one on the
         * test thread.
         *
         * The wrapper propagates exceptions thrown by the worker thread back to the caller.
         */
        public class decObj_a_mediaCodec_Wrapper : Java.Lang.Object, IRunnable
        {
            private Throwable mThrowable;
            private decObj_a_mediaCodec mTest;

            public decObj_a_mediaCodec_Wrapper(decObj_a_mediaCodec test)
            {
                mTest = test;
            }

            public void Run()
            {
                try
                {
                    mTest.decFrames();

                }
                catch (Throwable th)
                {
                    qyFuncs.showInfo_open(0, null, "decObj.run, catch throwalbe");

                    mThrowable = th;
                }

                //
                mTest.m_bEnd = true;

                //
                qyFuncs.mySetEvent((System.Threading.EventWaitHandle)mTest.m_qoi.hEvent_notifyToQuit);

                //
                string str;
                str = string.Format("a mediaCodec.wrapper.Run ends.");
                qyFuncs.showInfo_open(0, null, str);

                return;
            }

            /** Entry point. */
            public static void runTest(decObj_a_mediaCodec obj)
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
                obj.m_wrapper = new decObj_a_mediaCodec_Wrapper(obj);
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
        private void decFrames()
        {
            MediaCodec decoder = null;
            MediaExtractor extractor = null;
            int saveWidth = 640;
            //int saveHeight = 368;// 480;
            int saveHeight = 360;// 480;

            try
            {
                dec_init();

                //
                MediaFormat format = this.m_dec_format;

                // Could use width/height from the MediaFormat to get full-size frames.
                //outputSurface = new CodecOutputSurface(saveWidth, saveHeight);

                // Create a MediaCodec decoder, and configure it with the MediaFormat from the
                // extractor.  It's very important to use the format from the extractor because
                // it contains a copy of the CSD-0/CSD-1 codec-specific data chunks.
                string mime = format.GetString(MediaFormat.KeyMime);
                decoder = MediaCodec.CreateDecoderByType(mime);
                decoder.Configure(format, null, null, 0);
                decoder.Start();

                doDec(this, decoder);
            }
            finally
            {
                // release everything we grabbed
                if (decoder != null)
                {
                    decoder.Stop();
                    decoder.Release();
                    decoder = null;
                }
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
                AssetFileDescriptor afd = assets.OpenFd("dumpPcm.mp3");

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
                    //Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }

                //
                m_test_trackIndex = trackIndex;

                //
                m_dec_format = format;

                //
                //m_iW = format.GetInteger(MediaFormat.KeyWidth);
                //m_iH = format.GetInteger(MediaFormat.KeyHeight);
#endif
            }
            else
            {
                //int width = this.m_iW;
                //int height = this.m_iH;
                MediaFormat format = MediaFormat.CreateAudioFormat(Consts.MIME_TYPE_mp3, 16000, 1);

#if false
                ByteBuffer bb_extraData;
                int sizeof_spspps = 1024;
                bb_extraData = ByteBuffer.AllocateDirect(sizeof_spspps);
                if (bb_extraData == null) goto errLabel;
                //
                uint len = (uint)bb_extraData.Capacity();
                //                            
                //if (0 != this.m_qoi.cfg.v.pf_transformGetSpsPps(this.m_qoi.cfg.v.uiTransformType_pParent, this.m_qoi.cfg.v.pParent_transform, this.m_qoi.cfg.v.iDecParam_index_pMems_from, (byte*)bb_extraData.GetDirectBufferAddress(), &len)) goto errLabel;

                //
                format.SetByteBuffer("csd-0", bb_extraData);

                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }
#endif

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
        static unsafe int dec_ReadSampleData(decObj_a_mediaCodec dm, ByteBuffer inputBuf, int offset, ref long presentationTimeUs)
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
#if DEBUG
            if (bDbg)
            {
                str = string.Format("decA.dec_ReadSampleData enters");
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
#if true
                //fixed (int* pold_lPktId = &dm.m_old_lPktId)
                ref int pold_lPktId = ref dm.m_old_lPktId;
                {
                    nBytesRead = (uint)dm.m_tmpDataBuf.Length;
                    //
                    if (0 != dm.m_qoi.cfg.a.dec.pf_smplBitstreamReader_ReadNextFrame_func(ref dm.m_qoi, ref pold_lPktId, uiToInMs, dm.m_tmpDataBuf, ref nBytesRead, ref uiSampleTimeInMs, dm))
                    {
                        str = string.Format("decA.dec_ReadSmpleData：sampleBitstreamReader failed");
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
#endif
                //
                chunkSize = (int)nBytesRead;
                presentationTimeUs = uiSampleTimeInMs * 1000;

                //
#if DEBUG
#endif

            }

        errLabel:


#if DEBUG
            if (bDbg)
            {
                str = string.Format("decA.dec_ReadSampleData leaves. chunkSize {0}", chunkSize);
                qyFuncs.showInfo_open(0, null, str);
            }
#endif

            ///
            return chunkSize;
        }


        //
        static int dec_advance(decObj_a_mediaCodec dm)
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
        static unsafe int dec_putPcmData(decObj_a_mediaCodec dm,byte[]pcm, int iSampleTimeInMs)
        {
            int iErr = -1;
            string str;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            myPLAY_AUDIO_DATA pkt;
            qyFuncs.mymemset((IntPtr)(byte*)&pkt, 0, sizeof(myPLAY_AUDIO_DATA));

            myPLAY_AUDIO_DATA* pPkt_dec = &pkt;
            //
            //mymemset((IntPtr)(byte*)&pPkt_dec->head, 0, sizeof(TRANSFER_AUDIO_dataMemHead));   //  2015/02/14
            QY_TRANSFORM pTransform = (QY_TRANSFORM)dm.m_qoi.cfg.a.pParent_transform;
#if false
            if (0 != pMem->idInfo.ui64Id)
            {
                pPkt_dec->head.idInfo.ui64Id = pMem->idInfo.ui64Id;
                if (pPkt_dec->head.idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
                else pPkt_dec->head.tmpInternal.uiTranNo_org = 0;
            }
            else
            {
                pPkt_dec->head.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
                pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
            }
            //  2017/10/13
            pPkt_dec->head.uiPts = pMem->uiPts;
#endif
            pPkt_dec->head.idInfo.ui64Id = pTransform.audio.idInfo.ui64Id;
            pPkt_dec->head.uiTranNo_openAvDev = pTransform.audio.ta.uiTranNo_openAvDev;

            //
            int nAvgBytes = 16000 * 2 * 1;
            if (nAvgBytes == 0) goto errLabel;
            pPkt_dec->head.uiSampleTimeInMs = (uint)(1000.0f * dm.m_qoi.var.decInfo.nBytes_deced / (float)nAvgBytes);
            dm.m_qoi.var.decInfo.nBytes_deced += pcm.Length;



            //GuiShare.pf_dumpData(chunkPcm, 0, 0, true, false, "dec_pc.pcm");
            //
            //str = string.Format("doDec: get pcm {0} bytes, st {1}", pcm.Length, pPkt_dec->head.uiSampleTimeInMs);
            //qyFuncs.showInfo_open(0, null, str);

            //
            pPkt_dec->head.uiLen = (uint)pcm.Length;

            //
            if (0 == qyFuncs.chkFromPlayerIndex_a(Consts.CONST_uiTransformType_dec, pTransform, pPkt_dec, null))
            {
                qyFuncs.playAudioData(pProcInfo, (int)pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, pcm, pPkt_dec->head.uiLen, pTransform.iIndex_player);
            }

            //
            iErr = 0;


        errLabel:
            return iErr;


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
                if (mime.StartsWith("audio/"))
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
        static void doDec(decObj_a_mediaCodec dm, MediaCodec decoder)
        {
            string str;

            int TIMEOUT_USEC = 10000;
            ByteBuffer[]
            decoderInputBuffers = decoder.GetInputBuffers();
            ByteBuffer[] decoderOutputBuffers = decoder.GetOutputBuffers();
            //
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
                        //int chunkSize = extractor.ReadSampleData(inputBuf, 0);
                        //
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
                        int sampleRate = newFormat.GetInteger(MediaFormat.KeySampleRate);
                        int nChannels = newFormat.GetInteger(MediaFormat.KeyChannelCount);
                        //
                        //
                        int ii = 0;
                    }
                    else if (decoderStatus < 0)
                    {
                        Android.Util.Log.Error(TAG,"unexpected result from decoder.dequeueOutputBuffer: " + decoderStatus);
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

                        bool doRender = false;// (info.Size != 0);

                        if (info.Size != 0)
                        {
                            ByteBuffer outputBuffer;
                            outputBuffer = decoderOutputBuffers[decoderStatus];
                            byte[] chunkPcm;
                            chunkPcm = new byte[info.Size];
                            outputBuffer.Get(chunkPcm);
                            outputBuffer.Clear();
                            //
                            int iSampleInMs = (int)(info.PresentationTimeUs / 1000.0f);
                            //
                            dec_putPcmData(dm,chunkPcm,iSampleInMs);

                            //
                        }
                        // As soon as we call releaseOutputBuffer, the buffer will be forwarded
                        // to SurfaceTexture to convert to a texture.  The API doesn't guarantee
                        // that the texture will be available before the call returns, so we
                        // need to wait for the onFrameAvailable callback to fire.
                        decoder.ReleaseOutputBuffer(decoderStatus, doRender);
                        if (doRender)
                        {

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


 

    }




    //
}

