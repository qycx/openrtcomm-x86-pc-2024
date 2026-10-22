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

namespace qy.Droid
{

    //
    public class ExtractMp3FramesTest
    {
        private static string TAG = "ExtractMp3FramesTest";
        private static bool VERBOSE = true;//false;           // lots of logging

        // where to find files (note: requires WRITE_EXTERNAL_STORAGE permission)
        //private static  File FILES_DIR = Android.OS.Environment.GetExternalStorageDirectory();
        private static File FILES_DIR = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
        private static string INPUT_FILE = "source.mp4";
        private static int MAX_FRAMES = 100;//10;       // stop extracting after this many

        //
        public Activity mainActivity;
        public ExtractMpegFramesWrapper m_wrapper;
        public Thread m_th;


        /** test entry point */
        public void testExtractMpegFrames(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
            ExtractMpegFramesWrapper.runTest(this);
        }

        /**
         * Wraps extractMpegFrames().  This is necessary because SurfaceTexture will try to use
         * the looper in the current thread if one exists, and the CTS tests create one on the
         * test thread.
         *
         * The wrapper propagates exceptions thrown by the worker thread back to the caller.
         */
        public class ExtractMpegFramesWrapper : Java.Lang.Object, IRunnable
        {
            private Throwable mThrowable;
            private ExtractMp3FramesTest mTest;

            private ExtractMpegFramesWrapper(ExtractMp3FramesTest test)
            {
                mTest = test;
            }

            public void Run()
            {
                try
                {
                    mTest.extractMpegFrames(mTest.mainActivity);

                }
                catch (Throwable th)
                {
                    mThrowable = th;
                }

                return;
            }

            /** Entry point. */
            public static void runTest(ExtractMp3FramesTest obj)
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
                obj.m_wrapper = new ExtractMpegFramesWrapper(obj);
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
        private void extractMpegFrames(Activity mainActivity)
        {
            MediaCodec decoder = null;
            CodecOutputSurface outputSurface = null;
            MediaExtractor extractor = null;
            int saveWidth = 640;
            //int saveHeight = 368;// 480;
            int saveHeight = 360;// 480;

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
                Context context = mainActivity;

                //
                AssetManager assets = context.Assets;
                //
                AssetFileDescriptor afd = assets.OpenFd("dumpPcm.mp3");



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
                    //Log.d(TAG, "Video size is " + format.getInteger(MediaFormat.KEY_WIDTH) + "x" +             format.getInteger(MediaFormat.KEY_HEIGHT));
                }

                // Could use width/height from the MediaFormat to get full-size frames.
                //outputSurface = new CodecOutputSurface(saveWidth, saveHeight);

                // Create a MediaCodec decoder, and configure it with the MediaFormat from the
                // extractor.  It's very important to use the format from the extractor because
                // it contains a copy of the CSD-0/CSD-1 codec-specific data chunks.
                string mime = format.GetString(MediaFormat.KeyMime);
                decoder = MediaCodec.CreateDecoderByType(mime);
                decoder.Configure(format, null, null, 0);
                decoder.Start();

                doExtract(extractor, trackIndex, decoder, outputSurface);
            }
            finally
            {
                // release everything we grabbed
                if (outputSurface != null)
                {
                    outputSurface.release();
                    outputSurface = null;
                }
                if (decoder != null)
                {
                    try
                    {
                        decoder.Stop();
                    }
                    catch (IllegalStateException)
                    {

                    }

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
        static void doExtract(MediaExtractor extractor, int trackIndex, MediaCodec decoder,
                CodecOutputSurface outputSurface)
        {
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
                        int sampleRate = newFormat.GetInteger(MediaFormat.KeySampleRate);
                        int nChannels = newFormat.GetInteger(MediaFormat.KeyChannelCount);
                        //
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
                            //putPcmData(chunkPcm);
                            GuiShare.pf_dumpData(chunkPcm, 0, 0, true, false, "dec.pcm");

                        }
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
                            outputSurface.drawImage(true);

                            if (decodeCount < MAX_FRAMES)
                            {
#if true
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
            public STextureRender mTextureRender;
            public SurfaceTexture mSurfaceTexture;
            public Surface mSurface;

            private EGLDisplay mEGLDisplay = EGL14.EglNoDisplay;
            private EGLContext mEGLContext = EGL14.EglNoContext;
            private EGLSurface mEGLSurface = EGL14.EglNoSurface;
            int mWidth;
            int mHeight;

            //private Object mFrameSyncObject = new Object();     // guards mFrameAvailable
            public System.Threading.EventWaitHandle evt_mFrameSyncObject = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, new string("frameSync"));

            //
            private bool mFrameAvailable;

            //
            private ByteBuffer mPixelBuf;                       // used by saveFrame()

            /**
             * Creates a CodecOutputSurface backed by a pbuffer with the specified dimensions.  The
             * new EGL context and surface will be made current.  Creates a Surface that can be passed
             * to MediaCodec.configure().
             */
            public CodecOutputSurface(int width, int height)
            {
                if (width <= 0 || height <= 0)
                {
                    throw new IllegalArgumentException();
                }
                mWidth = width;
                mHeight = height;

                eglSetup();
                makeCurrent();
                setup();
            }

            /**
             * Creates interconnected instances of TextureRender, SurfaceTexture, and Surface.
             */
            private void setup()
            {
                mTextureRender = new STextureRender();
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
                mSurfaceTexture.SetOnFrameAvailableListener(this);

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
                mEGLContext = EGL14.EglCreateContext(mEGLDisplay, configs[0], EGL14.EglNoContext,
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

                mSurface.Release();

                // this causes a bunch of warnings that appear harmless but might confuse someone:
                //  W BufferQueue: [unnamed-3997-2] cancelBuffer: BufferQueue has been abandoned!
                //mSurfaceTexture.release();

                mTextureRender = null;
                mSurface = null;
                mSurfaceTexture = null;
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
                bDbg = true;
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
                string str;

                if (VERBOSE)
                {
                    str = string.Format("new frame available");
                    qyFuncs.traceLog(str);
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
                    throw new RuntimeException(msg + ": EGL error: 0x" + Integer.ToHexString(error));
                }
            }

        }


        /**
         * Code for rendering a texture onto a surface using OpenGL ES 2.0.
         */
        public class STextureRender
        {
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
            private int mTextureID = -12345;
            private int muMVPMatrixHandle;
            private int muSTMatrixHandle;
            private int maPositionHandle;
            private int maTextureHandle;

            public STextureRender()
            {
                mTriangleVertices = ByteBuffer.AllocateDirect(
                        mTriangleVerticesData.Length * FLOAT_SIZE_BYTES)
                        .Order(ByteOrder.NativeOrder()).AsFloatBuffer();
                mTriangleVertices.Put(mTriangleVerticesData).Position(0);

                Android.Opengl.Matrix.SetIdentityM(mSTMatrix, 0);
            }

            public int getTextureId()
            {
                return mTextureID;
            }

            /**
             * Draws the external texture in SurfaceTexture onto the current EGL surface.
             */
            public void drawFrame(SurfaceTexture st, bool invert)
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

            /**
             * Initializes GL state.  Call this after the EGL surface has been created and made current.
             */
            public void surfaceCreated()
            {
                mProgram = createProgram(VERTEX_SHADER, FRAGMENT_SHADER);
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

                mTextureID = textures[0];
                GLES20.GlBindTexture(GLES11Ext.GlTextureExternalOes, mTextureID);
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
                mProgram = createProgram(VERTEX_SHADER, fragmentShader);
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

            private int createProgram(string vertexSource, string fragmentSource)
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
    }




    //
}

