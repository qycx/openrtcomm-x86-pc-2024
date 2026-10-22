using Android.App;
using Android.Content;
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
using File = Java.IO.File;
using IOException = Java.IO.IOException;
namespace qy.Droid
{

#if false
    package android.media.cts;

    import android.media.MediaCodec;
    import android.media.MediaCodecInfo;
    import android.media.MediaFormat;
    import android.media.MediaMuxer;
    import android.opengl.EGL14;
    import android.opengl.EGLConfig;
    import android.opengl.EGLContext;
    import android.opengl.EGLDisplay;
    import android.opengl.EGLExt;
    import android.opengl.EGLSurface;
    import android.opengl.GLES20;
    import android.os.Environment;
    import android.test.AndroidTestCase;
    import android.util.Log;
    import android.view.Surface;

    import java.io.File;
    import java.io.IOException;
    import java.nio.ByteBuffer;

    //20131106: removed hard-coded "/sdcard"
    //20131205: added alpha to EGLConfig
#endif

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
    public class EncodeAndMuxTest //extends AndroidTestCase
    {
    private static  string TAG = "EncodeAndMuxTest";
    private static  bool VERBOSE = false;           // lots of logging

    // where to put the output file (note: /sdcard requires WRITE_EXTERNAL_STORAGE permission)
    private static  File OUTPUT_DIR = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);

        // parameters for the encoder
        private static  string MIME_TYPE = "video/avc";    // H.264 Advanced Video Coding
    private static  int FRAME_RATE = 15;               // 15fps
    private static  int IFRAME_INTERVAL = 10;          // 10 seconds between I-frames
    private static  int NUM_FRAMES = 30;               // two seconds of video

    // RGB color values for generated frames
    private static  int TEST_R0 = 0;
    private static  int TEST_G0 = 136;
    private static  int TEST_B0 = 0;
    private static  int TEST_R1 = 236;
    private static  int TEST_G1 = 50;
    private static  int TEST_B1 = 186;

    // size of a frame, in pixels
    private int mWidth = -1;
    private int mHeight = -1;
    // bit rate, in bits per second
    private int mBitRate = -1;

    // encoder / muxer state
    private MediaCodec mEncoder;
    private CodecInputSurface mInputSurface;
    private MediaMuxer mMuxer;
    private int mTrackIndex;
    private bool mMuxerStarted;

    // allocate one of these up front so we don't need to do it every time
    private MediaCodec.BufferInfo mBufferInfo;


    /**
     * Tests encoding of AVC video from a Surface.  The output is saved as an MP4 file.
     */
    public void testEncodeVideoToMp4()
    {
        // QVGA at 2Mbps
        mWidth = 320;
        mHeight = 240;
        mBitRate = 2000000;

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

    /**
     * Configures encoder and muxer state, and prepares the input Surface.
     */
    private void prepareEncoder()
    {
        mBufferInfo = new MediaCodec.BufferInfo();

        MediaFormat format = MediaFormat.CreateVideoFormat(MIME_TYPE, mWidth, mHeight);

        // Set some properties.  Failing to specify some of these can cause the MediaCodec
        // configure() call to throw an unhelpful exception.
        format.SetInteger(MediaFormat.KeyColorFormat,
                (int)MediaCodecCapabilities.Formatsurface);
        format.SetInteger(MediaFormat.KeyBitRate, mBitRate);
        format.SetInteger(MediaFormat.KeyFrameRate, FRAME_RATE);
        format.SetInteger(MediaFormat.KeyIFrameInterval, IFRAME_INTERVAL);
        if (VERBOSE) Android.Util.Log.Debug(TAG, "format: " + format);

        // Create a MediaCodec encoder, and configure it with our format.  Get a Surface
        // we can use for input and wrap it with a class that handles the EGL work.
        //
        // If you want to have two EGL contexts -- one for display, one for recording --
        // you will likely want to defer instantiation of CodecInputSurface until after the
        // "display" EGL context is created, then modify the eglCreateContext call to
        // take eglGetCurrentContext() as the share_context argument.
        mEncoder = MediaCodec.CreateEncoderByType(MIME_TYPE);
        mEncoder.Configure(format, null, null, MediaCodecConfigFlags.Encode);
        mInputSurface = new CodecInputSurface(mEncoder.CreateInputSurface());
        mEncoder.Start();

        // Output filename.  Ideally this would use Context.getFilesDir() rather than a
        // hard-coded output directory.
        string outputPath = new File(OUTPUT_DIR,
                "test1." + mWidth + "x" + mHeight + ".mp4").ToString();
        Android.Util.Log.Debug(TAG, "output file is " + outputPath);


        // Create a MediaMuxer.  We can't add the video track and start() the muxer here,
        // because our MediaFormat doesn't have the Magic Goodies.  These can only be
        // obtained from the encoder after it has started processing data.
        //
        // We're not actually interested in multiplexing audio.  We just want to convert
        // the raw H.264 elementary stream we get from MediaCodec into a .mp4 file.
        try
        {
            mMuxer = new MediaMuxer(outputPath, MuxerOutputType.Mpeg4);
        }
        catch (IOException ioe)
        {
            throw new RuntimeException("MediaMuxer creation failed", ioe);
        }

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
        if (mMuxer != null)
        {
            mMuxer.Stop();
            mMuxer.Release();
            mMuxer = null;
        }
    }

    /**
     * Extracts all pending data from the encoder.
     * <p>
     * If endOfStream is not set, this returns when there is no more data to drain.  If it
     * is set, we send EOS to the encoder, and then iterate until we see EOS on the output.
     * Calling this with endOfStream set should be done once, right before stopping the muxer.
     */
    private void drainEncoder(bool endOfStream)
    {
         int TIMEOUT_USEC = 10000;
        if (VERBOSE) Android.Util.Log.Debug(TAG, "drainEncoder(" + endOfStream + ")");

        if (endOfStream)
        {
            if (VERBOSE) Android.Util.Log.Debug(TAG, "sending EOS to encoder");
            mEncoder.SignalEndOfInputStream();
        }

        ByteBuffer[] encoderOutputBuffers = mEncoder.GetOutputBuffers();
        while (true)
        {
            int encoderStatus = mEncoder.DequeueOutputBuffer(mBufferInfo, TIMEOUT_USEC);
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

                // now that we have the Magic Goodies, start the muxer
                mTrackIndex = mMuxer.AddTrack(newFormat);
                mMuxer.Start();
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
                ByteBuffer encodedData = encoderOutputBuffers[encoderStatus];
                if (encodedData == null)
                {
                    throw new RuntimeException("encoderOutputBuffer " + encoderStatus +
                            " was null");
                }

                if ((mBufferInfo.Flags & MediaCodecBufferFlags.CodecConfig) != 0)
                {
                    // The codec config data was pulled out and fed to the muxer when we got
                    // the INFO_OUTPUT_FORMAT_CHANGED status.  Ignore it.
                    if (VERBOSE) Android.Util.Log.Debug(TAG, "ignoring BUFFER_FLAG_CODEC_CONFIG");
                    mBufferInfo.Size = 0;
                }

                if (mBufferInfo.Size != 0)
                {
                    if (!mMuxerStarted)
                    {
                        throw new RuntimeException("muxer hasn't started");
                    }

                    // adjust the ByteBuffer values to match BufferInfo (not needed?)
                    encodedData.Position(mBufferInfo.Offset);
                    encodedData.Limit(mBufferInfo.Offset + mBufferInfo.Size);

                    mMuxer.WriteSampleData(mTrackIndex, encodedData, mBufferInfo);
                    if (VERBOSE) Android.Util.Log.Debug(TAG, "sent " + mBufferInfo.Size + " bytes to muxer");
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
    private  class CodecInputSurface
    {
        private static  int EGL_RECORDABLE_ANDROID = 0x3142;

        private EGLDisplay mEGLDisplay = EGL14.EglNoDisplay;
        private EGLContext mEGLContext = EGL14.EglNoContext;
        private EGLSurface mEGLSurface = EGL14.EglNoSurface;

        private Surface mSurface;

        /**
         * Creates a CodecInputSurface from a Surface.
         */
        public CodecInputSurface(Surface surface)
        {
            if (surface == null)
            {
                throw new NullPointerException();
            }
            mSurface = surface;

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
            mEGLContext = EGL14.EglCreateContext(mEGLDisplay, configs[0], EGL14.EglNoContext,
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
                throw new RuntimeException(msg + ": EGL error: 0x" + Integer.ToHexString(error));
            }
        }
    }
}

}

