using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Qycx.Gleslibrary;
using Java.IO;
using Java.Lang;
using Java.Nio;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thread = Java.Lang.Thread;


using File = Java.IO.File;
using IOException = Java.IO.IOException;
namespace qy.Droid
{


    /**
     * Generate an MP4 file using OpenGL ES drawing commands.  Demonstrates the use of MediaMuxer
     * and MediaCodec with Surface input.
     * <p>
     * This uses various features first available in Android "Jellybean" 4.3 (API 18).  There is
     * no equivalent functionality in previous releases.
     * <p>
     * (This was derived from bits and pieces of CTS tests, and is packaged as such, but is not
     * currently part of CTS.)
     */
    public class encObj_mediaCodec //extends AndroidTestCase
    {
        private static string TAG = "EncodeAndMuxTest";
        private static bool VERBOSE = false;           // lots of logging

        // where to put the output file (note: /sdcard requires WRITE_EXTERNAL_STORAGE permission)
        private static File OUTPUT_DIR = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);

        // parameters for the encoder
        //private static string MIME_TYPE = "video/avc";    // H.264 Advanced Video Coding
        private string mimeType = "video/avc";    // H.264 Advanced Video Coding

        //
        private static int FRAME_RATE = 30;//15;               // 15fps
        private static int IFRAME_INTERVAL = 10;          // 10 seconds between I-frames
        private static int NUM_FRAMES = 15 * 30;               // two seconds of video

        // RGB color values for generated frames
        private static int TEST_R0 = 0;
        private static int TEST_G0 = 136;
        private static int TEST_B0 = 0;
        private static int TEST_R1 = 236;
        private static int TEST_G1 = 50;
        private static int TEST_B1 = 186;

        //
        //
        public EGLContext m_sharedEglContext;
        public QOI_common m_qoi;
        public int m_index_sharedObj;

        //
        public bool m_bQuit;


        // size of a frame, in pixels
        public int mWidth = -1;
        public int mHeight = -1;
        // bit rate, in bits per second
        public int mBitRate = -1;

        //
        public int m_iFourcc_o = 0;


        //
        public bool m_bEnd;

        //
        public TMP_buf1024 m_tmpBuf_spspps;
        public int m_len_spspps;
        public bool m_bGot_spspps;


        // encoder / muxer state
        private MediaCodec mEncoder;
        public CodecInputSurface mInputSurface;
        //
        private MediaMuxer mMuxer;
        private int mTrackIndex;
        private bool mMuxerStarted;

        // allocate one of these up front so we don't need to do it every time
        private MediaCodec.BufferInfo mBufferInfo;

        //
        //
        public bool m_bTest;

        //
        public encObj_mediaCodec_Wrapper m_wrapper;
        public Thread m_th;

        //
        public SimpleTexture m_simpleTex_o;


        //
        public encObj_mediaCodec()
        {
            int ii = 0;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
#if DEBUG
            //m_bTest = true;

            //
            if (pProcInfo.cfg.debugStatusInfo.bPrint_encObj_mediaCodec)
            {
                VERBOSE = true;
            }

            // for test
            encObj_mediaCodec.IFRAME_INTERVAL = 1000;
#endif

        }

        public bool bQuit()
        {
            bool bRet = false;
            string str;

            if (qyFuncs.bCapBmpQuit(this.m_qoi.cfg.v.pParent_transform, null, null))
            {
                bRet = true; goto errLabel;
            }
            if (m_bQuit)
            {
                str = string.Format("encObj_mediaCode.m_bQuit is true");
                qyFuncs.showInfo_open(0, null, str);
                //
                bRet = true; goto errLabel;
            }

        errLabel:
            return bRet;
        }


        /**
         * Tests encoding of AVC video from a Surface.  The output is saved as an MP4 file.
         */
        public void testEncodeVideoToMp4()
        {
            // QVGA at 2Mbps
            mWidth = 320;
            mHeight = 240;
            mBitRate = 2000000;

            //
            try
            {
                prepareEncoder();
                mInputSurface.makeCurrent();

                for (int i = 0; i < NUM_FRAMES; i++)
                {
                    // Feed any pending encoder output into the muxer.
                    drainEncoder(false);

                    // Generate a new frame of input.
                    generateSurfaceFrame(i);
                    mInputSurface.setPresentationTime(computePresentationTimeNsec(i));

                    // Submit it to the encoder.  The eglSwapBuffers call will block if the input
                    // is full, which would be bad if it stayed full until we dequeued an output
                    // buffer (which we can't do, since we're stuck here).  So long as we fully drain
                    // the encoder before supplying additional input, the system guarantees that we
                    // can supply another frame without blocking.
                    if (VERBOSE) Android.Util.Log.Debug(TAG, "sending frame " + i + " to encoder");
                    mInputSurface.swapBuffers();
                }

                // send end-of-stream to encoder, and drain remaining output
                drainEncoder(true);
            }
            finally
            {
                // release encoder, muxer, and input Surface
                releaseEncoder();
            }

            // To test the result, open the file with MediaExtractor, and get the format.  Pass
            // that into the MediaCodec decoder configuration, along with a SurfaceTexture surface,
            // and examine the output with glReadPixels.
        }

        //
        byte[] m_pInput = new byte[100000];


        public class encObj_mediaCodec_Wrapper : Java.Lang.Object, IRunnable
        {
            private Throwable mThrowable;
            private encObj_mediaCodec mTest;



            public encObj_mediaCodec_Wrapper(encObj_mediaCodec test)
            {
                mTest = test;
            }

            public void Run()
            {
                string str;

                //
                try
                {
                    mTest.prepareEncoder();
                    mTest.mInputSurface.makeCurrent();

                    for (int i = 0; ; i++)
                    {
                        //if (i >= NUM_FRAMES) break;
                        if (this.mTest.bQuit())
                        {
                            break;
                        }
#if false
                        if (qyFuncs.bCapBmpQuit(this.mTest.m_qoi.cfg.v.pParent_transform, null, null))
                        {
                            break;
                        }
                        if (mTest.m_bQuit)
                        {
                            str = string.Format("encObj_mediaCodec.Run break, m_bQuit is true");
                            qyFuncs.showInfo_open(0, null, str);
                            //
                            break;
                        }
#endif


                        // Feed any pending encoder output into the muxer.
                        mTest.drainEncoder(false);

                        // Generate a new frame of input.
                        mTest.generateSurfaceFrame(i);
                        mTest.mInputSurface.setPresentationTime(computePresentationTimeNsec(i));

                        // Submit it to the encoder.  The eglSwapBuffers call will block if the input
                        // is full, which would be bad if it stayed full until we dequeued an output
                        // buffer (which we can't do, since we're stuck here).  So long as we fully drain
                        // the encoder before supplying additional input, the system guarantees that we
                        // can supply another frame without blocking.
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "sending frame " + i + " to encoder");

                        //
                        mTest.mInputSurface.swapBuffers();
                    }

                    // send end-of-stream to encoder, and drain remaining output
                    mTest.drainEncoder(true);
                }
                catch (Java.Lang.Exception e)
                {
                    qyFuncs.showInfo_open(0, "", "encObj_mediaCodeWrap.271. exception");
                }
                finally
                {
                    //
                    qyFuncs.showInfo_open(0, null, "finally encObj.run, before releaseEncoder");

                    // release encoder, muxer, and input Surface
                    mTest.releaseEncoder();
                }

                //
                mTest.m_bEnd = true;

                //
                qyFuncs.mySetEvent((System.Threading.EventWaitHandle)mTest.m_qoi.hEvent_notifyToQuit);

                //
                str = string.Format("mediaCodec.wrapper.Run ends.");
                qyFuncs.showInfo_open(0, null, str);

                //
                return;
            }

            /** Entry point. */
        }


        //
        int enc_muxer_new()
        {
            if (m_bTest)
            {
                string outputPath = new File(OUTPUT_DIR,
            "test2." + mWidth + "x" + mHeight + ".mp4").ToString();
                Android.Util.Log.Debug(TAG, "output file is " + outputPath);

                try
                {
                    mMuxer = new MediaMuxer(outputPath, MuxerOutputType.Mpeg4);
                }
                catch (IOException ioe)
                {
                    throw new RuntimeException("MediaMuxer creation failed", ioe);
                }
            }
            else
            {

            }

            return 0;
        }

        int enc_muxer_start(MediaFormat newFormat)
        {
            if (m_bTest)
            {
                mTrackIndex = mMuxer.AddTrack(newFormat);
                mMuxer.Start();
            }


            return 0;
        }

        unsafe int enc_muxer_writeSampleData(ByteBuffer encodedData, MediaCodec.BufferInfo mBufferInfo)
        {
            bool bDbg = false;
            string str;

            //
            bool keyFrame = (mBufferInfo.Flags & MediaCodecBufferFlags.KeyFrame) != 0;
            if (keyFrame)
            {
                str = string.Format("enc.get a key frame");
                qyFuncs.showInfo_open(0, null, str);
            }

            //
            if (m_bTest)
            {
                mMuxer.WriteSampleData(mTrackIndex, encodedData, mBufferInfo);
                if (VERBOSE) Android.Util.Log.Debug(TAG, "sent " + mBufferInfo.Size + " bytes to muxer");
            }
            else
            {
                try
                {
#if false
                    int pos = encodedData.Position();
                    byte* pInput = (byte*)(encodedData.GetDirectBufferAddress() + pos);
                    uint inputSize = (uint)(encodedData.Limit() - pos);
#endif
                    //
#if DEBUG
                    qyFuncs.traceLog("enc_muxer_writeSampleData.368. mBufferInfo.size: " + mBufferInfo.Size);
#endif


                    //
                    //byte[] pInput = new byte[mBufferInfo.Size];
                    if (m_pInput.Length < mBufferInfo.Size)
                    {
                        m_pInput = new byte[mBufferInfo.Size];
                    }

                    byte[] pInput = m_pInput;

                    //
#if DEBUG
                    qyFuncs.traceLog("encMuxer_writeSampleData.376");
#endif

                    //
                    encodedData.Get(pInput, 0, mBufferInfo.Size);

                    //
#if DEBUG
                    qyFuncs.traceLog("encMuxer_writeSampleData.384");
#endif

                    //
                    int iSampleInMs = (int)(mBufferInfo.PresentationTimeUs / 1000.0f);
                    uint uiPts = 0;
                    myDRAW_VIDEO_DATA tmpPkt;

                    //
                    byte ucbKeyFrame = (byte)(keyFrame ? 1 : 0);

                    //qyFuncs.doPostEnc_mediaSdk_new(ref this.m_qoi, pInput, inputSize, iSampleInMs, uiPts, &tmpPkt);
                    qyFuncs.doPostEnc_mediaSdk_new(ref this.m_qoi, pInput, (uint)mBufferInfo.Size, iSampleInMs, uiPts, ucbKeyFrame, &tmpPkt);

                    //
#if DEBUG
                    if (true)
                    {
                        qyFuncs.traceLog("enc_muxer_writeSampleData: after doPostEnc_msdk_new");
                    }
#endif
                }
                catch (Java.Lang.Exception e)
                {
                    int ii = 0;
                    qyFuncs.showInfo_open(0, "", "enc_muxer_writeSampleData exception");

                }

            }


            //
            return 0;
        }

        int enc_muxer_exit()
        {
            if (m_bTest)
            {
                if (mMuxer != null)
                {
                    mMuxer.Stop();
                    mMuxer.Release();
                    mMuxer = null;
                }
            }
            //
            if (this.m_simpleTex_o != null)
            {
                this.m_simpleTex_o.destroy(); this.m_simpleTex_o = null;
            }


            return 0;
        }

        /**
         * Configures encoder and muxer state, and prepares the input Surface.
         */
        public void prepareEncoder()
        {
            mBufferInfo = new MediaCodec.BufferInfo();

            //
            if (m_iFourcc_o == Consts.CONST_fourcc_HEVC)
            {
                mimeType = Consts.MIME_TYPE_hevc;
            }

            //
            MediaFormat format = MediaFormat.CreateVideoFormat(mimeType, mWidth, mHeight);

            // Set some properties.  Failing to specify some of these can cause the MediaCodec
            // configure() call to throw an unhelpful exception.
            format.SetInteger(MediaFormat.KeyColorFormat,
                    (int)MediaCodecCapabilities.Formatsurface);
            format.SetInteger(MediaFormat.KeyBitRate, mBitRate);
            format.SetInteger(MediaFormat.KeyFrameRate, FRAME_RATE);
            format.SetInteger(MediaFormat.KeyIFrameInterval, IFRAME_INTERVAL);
            if (VERBOSE) Android.Util.Log.Debug(TAG, "format: " + format);

            //
            format.SetInteger(MediaFormat.KeyBitrateMode, (int)Android.Media.BitrateMode.Cbr);



            // Create a MediaCodec encoder, and configure it with our format.  Get a Surface
            // we can use for input and wrap it with a class that handles the EGL work.
            //
            // If you want to have two EGL contexts -- one for display, one for recording --
            // you will likely want to defer instantiation of CodecInputSurface until after the
            // "display" EGL context is created, then modify the eglCreateContext call to
            // take eglGetCurrentContext() as the share_context argument.
            mEncoder = MediaCodec.CreateEncoderByType(mimeType);
            mEncoder.Configure(format, null, null, MediaCodecConfigFlags.Encode);
            mInputSurface = new CodecInputSurface(mEncoder.CreateInputSurface(), null, this.m_sharedEglContext);
            mEncoder.Start();

            // Output filename.  Ideally this would use Context.getFilesDir() rather than a
            // hard-coded output directory.



            // Create a MediaMuxer.  We can't add the video track and start() the muxer here,
            // because our MediaFormat doesn't have the Magic Goodies.  These can only be
            // obtained from the encoder after it has started processing data.
            //
            // We're not actually interested in multiplexing audio.  We just want to convert
            // the raw H.264 elementary stream we get from MediaCodec into a .mp4 file.
#if false
            try
            {
                mMuxer1 = new MediaMuxer(outputPath, MuxerOutputType.Mpeg4);
            }
            catch (IOException ioe)
            {
                throw new RuntimeException("MediaMuxer creation failed", ioe);
            }
#endif
            //
            enc_muxer_new();

            //
            mTrackIndex = -1;
            mMuxerStarted = false;
        }

        /**
         * Releases encoder resources.  May be called after partial / failed initialization.
         */
        private void releaseEncoder()
        {
            if (VERBOSE) Android.Util.Log.Debug(TAG, "releasing encoder objects");
            if (mEncoder != null)
            {
                mEncoder.Stop();
                mEncoder.Release();
                mEncoder = null;
            }
            if (mInputSurface != null)
            {
                mInputSurface.release();
                mInputSurface = null;
            }
#if false
            if (mMuxer1 != null)
            {
                mMuxer1.Stop();
                mMuxer1.Release();
                mMuxer1 = null;
            }
#endif
            enc_muxer_exit();
        }

        /**
         * Extracts all pending data from the encoder.
         * <p>
         * If endOfStream is not set, this returns when there is no more data to drain.  If it
         * is set, we send EOS to the encoder, and then iterate until we see EOS on the output.
         * Calling this with endOfStream set should be done once, right before stopping the muxer.
         */
        private unsafe void drainEncoder(bool endOfStream)
        {
            int iErr = -1;
            int TIMEOUT_USEC = 10000;
            if (VERBOSE) Android.Util.Log.Debug(TAG, "drainEncoder.529(" + endOfStream + ")");

            if (endOfStream)
            {
                if (VERBOSE) Android.Util.Log.Debug(TAG, "sending EOS to encoder");
                mEncoder.SignalEndOfInputStream();
            }

            //
#if DEBUG
            //qyFuncs.traceLog("drainEncoder.539");
#endif

            //
            ByteBuffer[] encoderOutputBuffers = mEncoder.GetOutputBuffers();
            while (true)
            {
                //
#if DEBUG
                //qyFuncs.traceLog("drainEncoder.548");
#endif

                //
                int encoderStatus = mEncoder.DequeueOutputBuffer(mBufferInfo, TIMEOUT_USEC);

                //
#if DEBUG
                //qyFuncs.traceLog("drainEncoder.556, encoderStatus:" + encoderStatus);
#endif

                //
                if (encoderStatus == (int)MediaCodecInfoState.TryAgainLater)
                {
                    // no output available yet
                    if (!endOfStream)
                    {
                        break;      // out of while
                    }
                    else
                    {
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "no output available, spinning to await EOS");
                    }
                }
                else if (encoderStatus == (int)MediaCodecInfoState.OutputBuffersChanged)
                {
                    // not expected for an encoder
                    encoderOutputBuffers = mEncoder.GetOutputBuffers();
                }
                else if (encoderStatus == (int)MediaCodecInfoState.OutputFormatChanged)
                {
                    // should happen before receiving buffers, and should only happen once
                    if (mMuxerStarted)
                    {
                        throw new RuntimeException("format changed twice");
                    }
                    MediaFormat newFormat = mEncoder.OutputFormat;
                    Android.Util.Log.Debug(TAG, "encoder output format changed: " + newFormat);

                    //
#if true
                    MediaFormat tmpFormat = mEncoder.OutputFormat;

                    //
                    if (m_iFourcc_o == Consts.CONST_fourcc_HEVC)
                    {




                        //
                        ByteBuffer spsBb = tmpFormat.GetByteBuffer("csd-0");
                        //
                        int len = spsBb.Remaining();
                        byte[] sps = new byte[len];
                        spsBb.Get(sps);

                        //
                        fixed (byte* p = m_tmpBuf_spspps.buf)
                        {
                            if (len > Consts.MAX_spsPpsLen) goto errLabel;
                            System.Runtime.InteropServices.Marshal.Copy(sps, 0, (IntPtr)(byte*)p, len);


                            this.m_len_spspps = len;
                            this.m_bGot_spspps = true;
                            //
                            //qyFuncs.mySetEvent((System.Threading.EventWaitHandle)this.m_qoi.hEvent_notifyToQuit);
                        }


                    }
                    else
                    {

                        //
                        VH_stream vs;
                        qyFuncs.mymemset((IntPtr)(byte*)&vs, 0, sizeof(VH_stream));

                        //
                        ByteBuffer spsBb = tmpFormat.GetByteBuffer("csd-0");
                        //
                        int len = spsBb.Remaining();
                        byte[] sps = new byte[len];
                        spsBb.Get(sps);
                        if (len > Consts.MAX_spsPpsLen) goto errLabel;
                        System.Runtime.InteropServices.Marshal.Copy(sps, 0, (IntPtr)(byte*)vs.mem0.spsPps, len);
                        vs.mem0.usLen = (ushort)len;

                        //
                        ByteBuffer ppsBb = tmpFormat.GetByteBuffer("csd-1");
                        len = ppsBb.Remaining();
                        byte[] pps = new byte[len];
                        ppsBb.Get(pps);
                        if (len > Consts.MAX_spsPpsLen) goto errLabel;
                        System.Runtime.InteropServices.Marshal.Copy(pps, 0, (IntPtr)(byte*)vs.mem1.spsPps, len);
                        vs.mem1.usLen = (ushort)len;

                        //
                        vs.usCnt = 2;

                        //
                        fixed (byte* p = m_tmpBuf_spspps.buf)
                        {
                            len = Consts.cntof_buf1024;
                            if (0 != qyFuncs.makeSpsPps(&vs, p, (uint*)&len)) goto errLabel;
                            this.m_len_spspps = len;
                            this.m_bGot_spspps = true;
                            //
                            qyFuncs.mySetEvent((System.Threading.EventWaitHandle)this.m_qoi.hEvent_notifyToQuit);
                        }
                    }
#endif


                    // now that we have the Magic Goodies, start the muxer
#if false
                    mTrackIndex = mMuxer1.AddTrack(newFormat);
                    mMuxer1.Start();
#endif
                    //
                    enc_muxer_start(newFormat);
                    //
                    mMuxerStarted = true;
                }
                else if (encoderStatus < 0)
                {
                    Android.Util.Log.Warn(TAG, "unexpected result from encoder.dequeueOutputBuffer: " +
                            encoderStatus);
                    // let's ignore it
                }
                else
                {
                    //
#if DEBUG
                    //qyFuncs.traceLog("drainEncoder.683, encoderStatus " + encoderStatus);
                    if (encoderOutputBuffers == null)
                    {
                        qyFuncs.traceLog("drainEncoder.686, encoderOutputBuffers is null");
                    }
                    if (encoderOutputBuffers.Length == 0)
                    {
                        qyFuncs.traceLog("drainEncoder.686, encoderOutputBuffers.length is 0");

                    }
#endif

                    //
                    if (encoderStatus == 0)
                    {
                        int ii = 0;
                    }

                    //
                    ByteBuffer encodedData = encoderOutputBuffers[encoderStatus];
                    if (encodedData == null)
                    {
                        //
#if DEBUG
                        qyFuncs.traceLog("drainEncoder.686, runtimeException");
#endif

                        //
                        throw new RuntimeException("encoderOutputBuffer " + encoderStatus +
                                " was null");
                    }

                    //
#if DEBUG
                    qyFuncs.traceLog("drainEncoder.723.");
#endif

                    //
                    if ((mBufferInfo.Flags & MediaCodecBufferFlags.CodecConfig) != 0)
                    {
                        //
#if DEBUG
                        qyFuncs.traceLog("drainEncoder.763");
#endif

                        // The codec config data was pulled out and fed to the muxer when we got
                        // the INFO_OUTPUT_FORMAT_CHANGED status.  Ignore it.
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "ignoring BUFFER_FLAG_CODEC_CONFIG");

                        //
                        mBufferInfo.Size = 0;
                    }

                    //
#if DEBUG
                    qyFuncs.traceLog("drainEncoder.737.");
#endif


                    if (mBufferInfo.Size != 0)
                    {
                        if (!mMuxerStarted)
                        {
                            throw new RuntimeException("muxer hasn't started");
                        }

                        // adjust the ByteBuffer values to match BufferInfo (not needed?)
                        encodedData.Position(mBufferInfo.Offset);
                        encodedData.Limit(mBufferInfo.Offset + mBufferInfo.Size);

#if false
                        mMuxer1.WriteSampleData(mTrackIndex, encodedData, mBufferInfo);
                        if (VERBOSE) Android.Util.Log.Debug(TAG, "sent " + mBufferInfo.Size + " bytes to muxer");
#endif
                        enc_muxer_writeSampleData(encodedData, mBufferInfo);

                        //
#if DEBUG
                        qyFuncs.traceLog("drainEncoder.742, after enc_muxer_writeSampleData");
#endif
                    }

                    mEncoder.ReleaseOutputBuffer(encoderStatus, false);

                    if ((mBufferInfo.Flags & MediaCodecBufferFlags.EndOfStream) != 0)
                    {
                        if (!endOfStream)
                        {
                            Android.Util.Log.Warn(TAG, "reached end of stream unexpectedly");
                        }
                        else
                        {
                            if (VERBOSE) Android.Util.Log.Debug(TAG, "end of stream reached");
                        }
                        break;      // out of while
                    }
                }
            }


            iErr = 0;
        errLabel:

            //
            if (VERBOSE)
            {
                qyFuncs.traceLog("drainEncoder leaves");
            }

            //
            return;
        }

        /**
         * Generates a frame of data using GL commands.  We have an 8-frame animation
         * sequence that wraps around.  It looks like this:
         * <pre>
         *   0 1 2 3
         *   7 6 5 4
         * </pre>
         * We draw one of the eight rectangles and leave the rest set to the clear color.
         */
        private void generateSurfaceFrame(int frameIndex)
        {
            frameIndex %= 8;

            //
            bool bDbg = false;
            string str;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();




            //
            bool bTest_draw = false;
            bTest_draw = true;
            if (bTest_draw)
            {
                //
                uint dwTickCnt = qyFuncs.myGetTickCount(null);
                //
                test_draw();
                //
                if (bDbg)
                {
                    qyFuncs.traceLog("after test_draw");
                }

                //
                uint dwTickCnt_end = qyFuncs.myGetTickCount(null);
                //
                if (bDbg)
                {
                    str = string.Format("generateSurfaceFrame, {0}ms", dwTickCnt_end - dwTickCnt);
                    qyFuncs.showInfo_open(0, null, str);
                }

                //
                return;
            }


            //
            int startX, startY;
            if (frameIndex < 4)
            {
                // (0,0) is bottom-left in GL
                startX = frameIndex * (mWidth / 4);
                startY = mHeight / 2;
            }
            else
            {
                startX = (7 - frameIndex) * (mWidth / 4);
                startY = 0;
            }

            GLES20.GlClearColor(TEST_R0 / 255.0f, TEST_G0 / 255.0f, TEST_B0 / 255.0f, 1.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);

            GLES20.GlEnable(GLES20.GlScissorTest);
            GLES20.GlScissor(startX, startY, mWidth / 4, mHeight / 2);
            GLES20.GlClearColor(TEST_R1 / 255.0f, TEST_G1 / 255.0f, TEST_B1 / 255.0f, 1.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);
            GLES20.GlDisable(GLES20.GlScissorTest);
        }


        //


        //
        unsafe int test_draw()
        {
            int iErr = -1;

            bool bDbg = false;
            string str;

            //
            Context context = qyMessengerApp.Droid.MyApplication.Context;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, this.m_index_sharedObj);
            if (pSharedObj == null) goto errLabel;

            for (; ; )
            {
                //if (qyFuncs.bCapBmpQuit(this.m_qoi.cfg.v.pParent_transform, null, null))                  
                if (this.bQuit())
                {
                    str = string.Format("test_draw failed: bCapBmpQuit is true");
                    qyFuncs.showInfo_open(0, null, str);
                    goto errLabel;
                }
                //
                ref QOI_common pQdcObjInfo = ref this.m_qoi;
                CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)pQdcObjInfo.cfg.v.pParent_transform;
                int nQNodes = qyFuncs.getQ2Nodes(pCapProcInfo.postThread.q2);
                //
#if DEBUG
                if (false)
                {
                    str = "capProcInfo.postThread.q2 " + nQNodes;
                    qyFuncs.showInfo_open(0, "", str);
                }
#endif
                //
                if (nQNodes > 20)
                {
                    qyFuncs.mySleep(16);
                    continue;
                }

                //
                if (pSharedObj.var.bFrameAvail) break;

                qyFuncs.myWaitOne(pSharedObj.var.hEvt_notifyFrameAvail, 33);
                continue;
            }
            //
            pSharedObj.var.bFrameAvail = false;

            //
            int old_lPktId = 0;
            byte* pInput = null;
            uint uiInputSize = 0;
            bool bNeed_forceKeyFrame = false;
            //
            if (m_qoi.cfg.v.uiTransformType_pParent == Consts.CONST_uiTransformType_encVideo)
            {
                CAP_procInfo_bmpCommon pCapBmp = (CAP_procInfo_bmpCommon)m_qoi.cfg.v.pParent_transform;

                if (pCapBmp.compressVideo.encInfo.dwTickCnt_startToForceKeyFrame != 0)
                {
                    uint dwTickCnt = qyFuncs.myGetTickCount(null);
                    if (dwTickCnt - pCapBmp.compressVideo.encInfo.dwTickCnt_startToForceKeyFrame < Consts.CONST_timeInMs_forceKeyFrame)
                    {
                        bNeed_forceKeyFrame = true;
                        //
                        //qyFuncs.showInfo_open(  0,  null,  (  "enc_parseOrgVPkt_i: bNeed_forceKeyFrame true"  )  );
                        //
                        if (dwTickCnt - m_qoi.var.encInfo.dwLastTickCnt_insertKeyFrame > 1000)
                        {
                            m_qoi.var.encInfo.dwLastTickCnt_insertKeyFrame = dwTickCnt;
                            //
                            qyFuncs.showInfo_open(0, null, ("hdEnc_conf: insertKeyFrame"));
                            //
                            //this->InsertKeyFrame();
                            if ((int)Build.VERSION.SdkInt >= 23)
                            {
                                Bundle params1 = new Bundle();

                                params1.PutInt(MediaCodec.ParameterKeyRequestSyncFrame, 0);
                                this.mEncoder.SetParameters(params1);
                            }
                        }


                    }
                }
            }



            //
            using (CQySyncObj syncObj = new CQySyncObj())
            {
                if (0 != syncObj.sync("", pSharedObj.var.hMutex_syncEncFrame))
                {
                    str = string.Format("test_draw failed, mutex sync failed");
                    qyFuncs.showInfo_open(0, null, str);
                    goto errLabel;
                }

                //
                int w = this.mWidth;
                int h = this.mHeight;

                //
                int texId = 0;
                //texId=camera_RenderThread.m_test_textureId;
                texId = pSharedObj.var.m_o_texId;

                //
                if (this.m_simpleTex_o == null)
                {
                    this.m_simpleTex_o = new SimpleTexture(context, texId, false, "enc.881");
                }

                //
#if DEBUG
                //qyFuncs.traceLog( "encObj_mediaCodec.test_draw.886");
#endif

                //
                this.m_simpleTex_o.resize(w, h, 0);
                this.m_simpleTex_o.draw(0);

                //
            }

        errLabel:

#if DEBUG
            qyFuncs.traceLog("encObj_mediaCodec.test_draw.899");
#endif

            return 0;
        }


        /**
         * Generates the presentation time for frame N, in nanoseconds.
         */
        private static long computePresentationTimeNsec(int frameIndex)
        {
            long ONE_BILLION = 1000000000;
            return frameIndex * ONE_BILLION / FRAME_RATE;
        }


        /**
         * Holds state associated with a Surface used for MediaCodec encoder input.
         * <p>
         * The constructor takes a Surface obtained from MediaCodec.createInputSurface(), and uses that
         * to create an EGL window surface.  Calls to eglSwapBuffers() cause a frame of data to be sent
         * to the video encoder.
         * <p>
         * This object owns the Surface -- releasing this will release the Surface too.
         */
        public class CodecInputSurface
        {
            public object m_p0;
            //
            public EGLContext m_sharedEglContext1;

            //
            private static int EGL_RECORDABLE_ANDROID = 0x3142;

            private EGLDisplay mEGLDisplay = EGL14.EglNoDisplay;
            private EGLContext mEGLContext = EGL14.EglNoContext;
            private EGLSurface mEGLSurface = EGL14.EglNoSurface;

            private Surface mSurface;

            /**
             * Creates a CodecInputSurface from a Surface.
             */
            public CodecInputSurface(Surface surface, object p0, EGLContext sharedEglContext)
            {
                if (surface == null)
                {
                    throw new NullPointerException();
                }
                mSurface = surface;

                //
                m_p0 = p0;
                m_sharedEglContext1 = sharedEglContext;

                //
                eglSetup();
            }

            /**
             * Prepares EGL.  We want a GLES 2.0 context and a surface that supports recording.
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
                    throw new RuntimeException("unable to initialize EGL14");
                }

                // Configure EGL for recording and OpenGL ES 2.0.
                int[] attribList = {
                    EGL14.EglRedSize, 8,
                    EGL14.EglGreenSize, 8,
                    EGL14.EglBlueSize, 8,
                    EGL14.EglAlphaSize, 8,
                    EGL14.EglRenderableType, EGL14.EglOpenglEs2Bit,
                    EGL_RECORDABLE_ANDROID, 1,
                    EGL14.EglNone
            };
                EGLConfig[] configs = new EGLConfig[1];
                int[] numConfigs = new int[1];
                EGL14.EglChooseConfig(mEGLDisplay, attribList, 0, configs, 0, configs.Length,
                        numConfigs, 0);
                checkEglError("eglCreateContext RGB888+recordable ES2");

                // Configure context for OpenGL ES 2.0.
                int[] attrib_list = {
                    EGL14.EglContextClientVersion, 2,
                    EGL14.EglNone
            };
                //
                EGLContext share_context = EGL14.EglNoContext;
                //
                share_context = this.m_sharedEglContext1;
                //
                mEGLContext = EGL14.EglCreateContext(mEGLDisplay, configs[0], share_context,
                        attrib_list, 0);
                checkEglError("eglCreateContext");

                // Create a window surface, and attach it to the Surface we received.
                int[] surfaceAttribs = {
                    EGL14.EglNone
            };
                mEGLSurface = EGL14.EglCreateWindowSurface(mEGLDisplay, configs[0], mSurface,
                        surfaceAttribs, 0);
                checkEglError("eglCreateWindowSurface");
            }

            /**
             * Discards all resources held by this class, notably the EGL context.  Also releases the
             * Surface that was passed to our constructor.
             */
            public void release()
            {
                if (mEGLDisplay != EGL14.EglNoDisplay)
                {
                    EGL14.EglMakeCurrent(mEGLDisplay, EGL14.EglNoSurface, EGL14.EglNoSurface,
                            EGL14.EglNoContext);
                    EGL14.EglDestroySurface(mEGLDisplay, mEGLSurface);
                    EGL14.EglDestroyContext(mEGLDisplay, mEGLContext);
                    EGL14.EglReleaseThread();
                    EGL14.EglTerminate(mEGLDisplay);
                }

                mSurface.Release();

                mEGLDisplay = EGL14.EglNoDisplay;
                mEGLContext = EGL14.EglNoContext;
                mEGLSurface = EGL14.EglNoSurface;

                mSurface = null;
            }

            /**
             * Makes our EGL context and surface current.
             */
            public void makeCurrent()
            {
                EGL14.EglMakeCurrent(mEGLDisplay, mEGLSurface, mEGLSurface, mEGLContext);
                checkEglError("eglMakeCurrent");
            }

            /**
             * Calls eglSwapBuffers.  Use this to "publish" the current frame.
             */
            public bool swapBuffers()
            {
                bool result = EGL14.EglSwapBuffers(mEGLDisplay, mEGLSurface);
                checkEglError("eglSwapBuffers");
                return result;
            }

            /**
             * Sends the presentation time stamp to EGL.  Time is expressed in nanoseconds.
             */
            public void setPresentationTime(long nsecs)
            {
                EGLExt.EglPresentationTimeANDROID(mEGLDisplay, mEGLSurface, nsecs);
                checkEglError("eglPresentationTimeANDROID");
            }

            /**
             * Checks for EGL errors.  Throws an exception if one is found.
             */
            private void checkEglError(string msg)
            {
                int error;
                if ((error = EGL14.EglGetError()) != EGL14.EglSuccess)
                {
                    qyFuncs.traceLog("zzzzzbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
                    throw new RuntimeException(msg + ": EGL error: 0x" + Integer.ToHexString(error));
                }
            }
        }
    }


}

