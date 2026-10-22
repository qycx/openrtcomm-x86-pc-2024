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
using Thread = Java.Lang.Thread;
namespace qy.Droid
{


#if false
    package android.media.cts;

    import android.graphics.SurfaceTexture;
    import android.hardware.Camera;
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
    import android.opengl.GLES11Ext;
    import android.opengl.GLES20;
    import android.opengl.Matrix;
    import android.os.Environment;
    import android.test.AndroidTestCase;
    import android.util.Log;
    import android.view.Surface;

    import java.io.File;
    import java.io.IOException;
    import java.nio.ByteBuffer;
    import java.nio.ByteOrder;
    import java.nio.FloatBuffer;
#endif

    //20131106: removed unnecessary glFinish(), removed hard-coded "/sdcard"
    //20131205: added alpha to EGLConfig
    //20131210: demonstrate un-bind and re-bind of texture, for apps with shared EGL contexts
    //20140123: correct error checks on glGet*Location() and program creation (they don't set error)

    /**
     * Record video from the camera preview and encode it as an MP4 file.  Demonstrates the use
     * of MediaMuxer and MediaCodec with Camera input.  Does not record audio.
     * <p>
     * Generally speaking, it's better to use MediaRecorder for this sort of thing.  This example
     * demonstrates one possible advantage: editing of video as it's being encoded.  A GLES 2.0
     * fragment shader is used to perform a silly color tweak every 15 frames.
     * <p>
     * This uses various features first available in Android "Jellybean" 4.3 (API 18).  There is
     * no equivalent functionality in previous releases.  (You can send the Camera preview to a
     * byte buffer with a fully-specified format, but MediaCodec encoders want different input
     * formats on different devices, and this use case wasn't well exercised in CTS pre-4.3.)
     * <p>
     * The output file will be something like "/sdcard/test.640x480.mp4".
     * <p>
     * (This was derived from bits and pieces of CTS tests, and is packaged as such, but is not
     * currently part of CTS.)
     */
    public class CameraToMpegTest //extends AndroidTestCase
    {
    private static  string TAG = "CameraToMpegTest";
    private static  bool VERBOSE = false;           // lots of logging

    // where to put the output file (note: /sdcard requires WRITE_EXTERNAL_STORAGE permission)
    private static  File OUTPUT_DIR = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);

    // parameters for the encoder
    private static  string MIME_TYPE = "video/avc";    // H.264 Advanced Video Coding
    private static  int FRAME_RATE = 30;               // 30fps
    private static  int IFRAME_INTERVAL = 5;           // 5 seconds between I-frames
    private static  long DURATION_SEC = 28;             // 8 seconds of video

    // Fragment shader that swaps color channels around.
    private static  string SWAPPED_FRAGMENT_SHADER =
            "#extension GL_OES_EGL_image_external : require\n" +
            "precision mediump float;\n" +
            "varying vec2 vTextureCoord;\n" +
            "uniform samplerExternalOES sTexture;\n" +
            "void main() {\n" +
            "  gl_FragColor = texture2D(sTexture, vTextureCoord).gbra;\n" +
            "}\n";

    // encoder / muxer state
    private MediaCodec mEncoder;
    private CodecInputSurface mInputSurface;
    private MediaMuxer mMuxer;
    private int mTrackIndex;
    private bool mMuxerStarted;

    // camera state
    private Android.Hardware.Camera mCamera;
    private SurfaceTextureManager mStManager;

    // allocate one of these up front so we don't need to do it every time
    private MediaCodec.BufferInfo mBufferInfo;

    /** test entry point */
    public void testEncodeCameraToMp4() //throws Throwable
    {
        CameraToMpegWrapper.runTest(this);
    }

    /**
     * Wraps encodeCameraToMpeg().  This is necessary because SurfaceTexture will try to use
     * the looper in the current thread if one exists, and the CTS tests create one on the
     * test thread.
     *
     * The wrapper propagates exceptions thrown by the worker thread back to the caller.
     */
    private  class CameraToMpegWrapper :Java.Lang.Object,IRunnable
    {
        private Throwable mThrowable;
    private CameraToMpegTest mTest;

    private CameraToMpegWrapper(CameraToMpegTest test)
    {
        mTest = test;
    }

        public void Run()
    {
        try
        {
            mTest.encodeCameraToMpeg();
        }
        catch (Throwable th)
        {
            mThrowable = th;
        }
    }

    /** Entry point. */
    public static void runTest(CameraToMpegTest obj) //throws Throwable
    {
        CameraToMpegWrapper wrapper = new CameraToMpegWrapper(obj);
    Thread th = new Thread(wrapper, "codec test");
    th.Start();
            //th.Join();
            if (wrapper.mThrowable != null) {
                throw wrapper.mThrowable;
            }
        }
    }

    /**
     * Tests encoding of AVC video from Camera input.  The output is saved as an MP4 file.
     */
    private void encodeCameraToMpeg()
{
    // arbitrary but popular values
    int encWidth = 640;
    int encHeight = 480;
    int encBitRate = 6000000;      // Mbps
    Android.Util.Log.Debug(TAG, MIME_TYPE + " output " + encWidth + "x" + encHeight + " @" + encBitRate);

    try
    {
        prepareCamera(encWidth, encHeight);
        prepareEncoder(encWidth, encHeight, encBitRate);
        mInputSurface.makeCurrent();
        prepareSurfaceTexture();

        mCamera.StartPreview();

        long startWhen = Java.Lang.JavaSystem.NanoTime();
        long desiredEnd = startWhen + DURATION_SEC * 1000000000L;
        Android.Graphics.SurfaceTexture st = mStManager.getSurfaceTexture();
        int frameCount = 0;

        while (Java.Lang.JavaSystem.NanoTime() < desiredEnd)
        {
            // Feed any pending encoder output into the muxer.
            drainEncoder(false);

            // Switch up the colors every 15 frames.  Besides demonstrating the use of
            // fragment shaders for video editing, this provides a visual indication of
            // the frame rate: if the camera is capturing at 15fps, the colors will change
            // once per second.
            if ((frameCount % 15) == 0)
            {
                string fragmentShader = null;
                if ((frameCount & 0x01) != 0)
                {
                    fragmentShader = SWAPPED_FRAGMENT_SHADER;
                }
                mStManager.changeFragmentShader(fragmentShader);
            }
            frameCount++;

            // Acquire a new frame of input, and render it to the Surface.  If we had a
            // GLSurfaceView we could switch EGL contexts and call drawImage() a second
            // time to render it on screen.  The texture can be shared between contexts by
            // passing the GLSurfaceView's EGLContext as eglCreateContext()'s share_context
            // argument.
            mStManager.awaitNewImage();
            mStManager.drawImage();

            // Set the presentation time stamp from the SurfaceTexture's time stamp.  This
            // will be used by MediaMuxer to set the PTS in the video.
            if (VERBOSE)
            {
                Android.Util.Log.Debug(TAG, "present: " +
                        ((st.Timestamp - startWhen) / 1000000.0) + "ms");
            }
            mInputSurface.setPresentationTime(st.Timestamp);

            // Submit it to the encoder.  The eglSwapBuffers call will block if the input
            // is full, which would be bad if it stayed full until we dequeued an output
            // buffer (which we can't do, since we're stuck here).  So long as we fully drain
            // the encoder before supplying additional input, the system guarantees that we
            // can supply another frame without blocking.
            if (VERBOSE) Android.Util.Log.Debug(TAG, "sending frame to encoder");
            mInputSurface.swapBuffers();
        }

        // send end-of-stream to encoder, and drain remaining output
        drainEncoder(true);
    }
    finally
    {
        // release everything we grabbed
        releaseCamera();
        releaseEncoder();
        releaseSurfaceTexture();
    }
}

/**
 * Configures Camera for video capture.  Sets mCamera.
 * <p>
 * Opens a Camera and sets parameters.  Does not start preview.
 */
private void prepareCamera(int encWidth, int encHeight)
{
    if (mCamera != null)
    {
        throw new RuntimeException("camera already initialized");
    }

    Android.Hardware.Camera.CameraInfo info = new Android.Hardware.Camera.CameraInfo();

    // Try to find a front-facing camera (e.g. for videoconferencing).
    int numCameras = Android.Hardware.Camera.NumberOfCameras;
    for (int i = 0; i < numCameras; i++)
    {
        Android.Hardware.Camera.GetCameraInfo(i, info);
        if (info.Facing == Android.Hardware.CameraFacing.Front)
        {
            mCamera = Android.Hardware.Camera.Open(i);
            break;
        }
    }
    if (mCamera == null)
    {
        Android.Util.Log.Debug(TAG, "No front-facing camera found; opening default");
        mCamera = Android.Hardware.Camera.Open();    // opens first back-facing camera
    }
    if (mCamera == null)
    {
        throw new RuntimeException("Unable to open camera");
    }

    Android.Hardware.Camera.Parameters parms = mCamera.GetParameters();

    choosePreviewSize(parms, encWidth, encHeight);
    // leave the frame rate set to default
    mCamera.SetParameters(parms);

    Android.Hardware.Camera.Size size = parms.PreviewSize;
    Android.Util.Log.Debug(TAG, "Camera preview size is " + size.Width + "x" + size.Height);
}

/**
 * Attempts to find a preview size that matches the provided width and height (which
 * specify the dimensions of the encoded video).  If it fails to find a match it just
 * uses the default preview size.
 * <p>
 * TODO: should do a best-fit match.
 */
private static void choosePreviewSize(Android.Hardware.Camera.Parameters parms, int width, int height)
{
    // We should make sure that the requested MPEG size is less than the preferred
    // size, and has the same aspect ratio.
    Android.Hardware.Camera.Size ppsfv = parms.PreferredPreviewSizeForVideo;
    if (VERBOSE && ppsfv != null)
    {
        Android.Util.Log.Debug(TAG, "Camera preferred preview size for video is " +
                ppsfv.Width + "x" + ppsfv.Height);
    }

    foreach ( Android.Hardware.Camera.Size size in parms.SupportedPreviewSizes) {
    if (size.Width == width && size.Height == height)
    {
        parms.SetPreviewSize(width, height);
        return;
    }
}

Android.Util.Log.Warn(TAG, "Unable to set preview size to " + width + "x" + height);
if (ppsfv != null)
{
    parms.SetPreviewSize(ppsfv.Width, ppsfv.Height);
}
    }

    /**
     * Stops camera preview, and releases the camera to the system.
     */
    private void releaseCamera()
{
    if (VERBOSE) Android.Util.Log.Debug(TAG, "releasing camera");
    if (mCamera != null)
    {
        mCamera.StopPreview();
        mCamera.Release();
        mCamera = null;
    }
}

/**
 * Configures SurfaceTexture for camera preview.  Initializes mStManager, and sets the
 * associated SurfaceTexture as the Camera's "preview texture".
 * <p>
 * Configure the EGL surface that will be used for output before calling here.
 */
private void prepareSurfaceTexture()
{
    mStManager = new SurfaceTextureManager();
    Android.Graphics.SurfaceTexture st = mStManager.getSurfaceTexture();
    try
    {
        mCamera.SetPreviewTexture(st);
    }
    catch (IOException ioe)
    {
        throw new RuntimeException("setPreviewTexture failed", ioe);
    }
}

/**
 * Releases the SurfaceTexture.
 */
private void releaseSurfaceTexture()
{
    if (mStManager != null)
    {
        mStManager.release();
        mStManager = null;
    }
}

/**
 * Configures encoder and muxer state, and prepares the input Surface.  Initializes
 * mEncoder, mMuxer, mInputSurface, mBufferInfo, mTrackIndex, and mMuxerStarted.
 */
private void prepareEncoder(int width, int height, int bitRate)
{
    mBufferInfo = new MediaCodec.BufferInfo();

    MediaFormat format = MediaFormat.CreateVideoFormat(MIME_TYPE, width, height);

    // Set some properties.  Failing to specify some of these can cause the MediaCodec
    // configure() call to throw an unhelpful exception.
    format.SetInteger(MediaFormat.KeyColorFormat,
            //(int)MediaCodecInfo.CodecCapabilities.COLORFormatSurface
            (int)Android.Media.MediaCodecCapabilities.Formatsurface
            );
            format.SetInteger(MediaFormat.KeyBitRate, bitRate);
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
    mEncoder.Configure(format, null, null, 
        //MediaCodec.ConfigureFlagEncode
        MediaCodecConfigFlags.Encode
        );
    mInputSurface = new CodecInputSurface(mEncoder.CreateInputSurface());
    mEncoder.Start();

    // Output filename.  Ideally this would use Context.getFilesDir() rather than a
    // hard-coded output directory.
    string outputPath = new File(OUTPUT_DIR,
            //"test." + width + "x" + height + ".mp4").ToString();
            "test.mp4").ToString();
            Android.Util.Log.Info(TAG, "Output file is " + outputPath);


    // Create a MediaMuxer.  We can't add the video track and start() the muxer here,
    // because our MediaFormat doesn't have the Magic Goodies.  These can only be
    // obtained from the encoder after it has started processing data.
    //
    // We're not actually interested in multiplexing audio.  We just want to convert
    // the raw H.264 elementary stream we get from MediaCodec into a .mp4 file.
    try
    {
        mMuxer = new MediaMuxer(outputPath, 
            //MediaMuxer.OutputFormat.MuxerOutputMpeg4
            MuxerOutputType.Mpeg4
            );
    }
    catch (IOException ioe)
    {
        throw new RuntimeException("MediaMuxer creation failed", ioe);
    }

    mTrackIndex = -1;
    mMuxerStarted = false;
}

/**
 * Releases encoder resources.
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
 * Extracts all pending data from the encoder and forwards it to the muxer.
 * <p>
 * If endOfStream is not set, this returns when there is no more data to drain.  If it
 * is set, we send EOS to the encoder, and then iterate until we see EOS on the output.
 * Calling this with endOfStream set should be done once, right before stopping the muxer.
 * <p>
 * We're just using the muxer to get a .mp4 file (instead of a raw H.264 stream).  We're
 * not recording audio.
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
 * Holds state associated with a Surface used for MediaCodec encoder input.
 * <p>
 * The constructor takes a Surface obtained from MediaCodec.createInputSurface(), and uses
 * that to create an EGL window surface.  Calls to eglSwapBuffers() cause a frame of data to
 * be sent to the video encoder.
 * <p>
 * This object owns the Surface -- releasing this will release the Surface too.
 */
public class CodecInputSurface
{
    private static int EGL_RECORDABLE_ANDROID = 0x3142;

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


/**
 * Manages a SurfaceTexture.  Creates SurfaceTexture and TextureRender objects, and provides
 * functions that wait for frames and render them to the current EGL surface.
 * <p>
 * The SurfaceTexture can be passed to Camera.setPreviewTexture() to receive camera output.
 */
private  class SurfaceTextureManager:Java.Lang.Object,Android.Graphics.SurfaceTexture.IOnFrameAvailableListener
{
        private Android.Graphics.SurfaceTexture mSurfaceTexture;
private CameraToMpegTest.STextureRender mTextureRender;

            private Java.Lang.Object mFrameSyncObject = null;// new Java.Lang.Object();     // guards mFrameAvailable
private bool mFrameAvailable;

/**
 * Creates instances of TextureRender and SurfaceTexture.
 */
public SurfaceTextureManager()
{
    mTextureRender = new CameraToMpegTest.STextureRender();
    mTextureRender.surfaceCreated();

    if (VERBOSE) Android.Util.Log.Debug(TAG, "textureID=" + mTextureRender.getTextureId());
    mSurfaceTexture = new Android.Graphics.SurfaceTexture(mTextureRender.getTextureId());

    // This doesn't work if this object is created on the thread that CTS started for
    // these test cases.
    //
    // The CTS-created thread has a Looper, and the SurfaceTexture constructor will
    // create a Handler that uses it.  The "frame available" message is delivered
    // there, but since we're not a Looper-based thread we'll never see it.  For
    // this to do anything useful, OutputSurface must be created on a thread without
    // a Looper, so that SurfaceTexture uses the main application Looper instead.
    //
    // Java language note: passing "this" out of a constructor is generally unwise,
    // but we should be able to get away with it here.
    mSurfaceTexture.SetOnFrameAvailableListener(this);
}

public void release()
{
    // this causes a bunch of warnings that appear harmless but might confuse someone:
    //  W BufferQueue: [unnamed-3997-2] cancelBuffer: BufferQueue has been abandoned!
    //mSurfaceTexture.release();

    mTextureRender = null;
    mSurfaceTexture = null;
}

/**
 * Returns the SurfaceTexture.
 */
public Android.Graphics.SurfaceTexture getSurfaceTexture()
{
    return mSurfaceTexture;
}

/**
 * Replaces the fragment shader.
 */
public void changeFragmentShader(string fragmentShader)
{
    mTextureRender.changeFragmentShader(fragmentShader);
}

/**
 * Latches the next buffer into the texture.  Must be called from the thread that created
 * the OutputSurface object.
 */
public void awaitNewImage()
{
    int TIMEOUT_MS = 2500;

    //synchronized(mFrameSyncObject) 
                    {
        while (!mFrameAvailable)
        {
            try
            {
                            // Wait for onFrameAvailable() to signal us.  Use a timeout to avoid
                            // stalling the test if it doesn't arrive.
                            //mFrameSyncObject.Wait(TIMEOUT_MS);
                            Thread.Sleep(TIMEOUT_MS);
                if (!mFrameAvailable)
                {
                    // TODO: if "spurious wakeup", continue while loop
                    throw new RuntimeException("Camera frame wait timed out");
                }
            }
            catch (InterruptedException ie)
            {
                // shouldn't happen
                throw new RuntimeException(ie);
            }
        }
        mFrameAvailable = false;
    }

    // Latch the data.
    mTextureRender.checkGlError("before updateTexImage");
    mSurfaceTexture.UpdateTexImage();
}

/**
 * Draws the data from SurfaceTexture onto the current EGL surface.
 */
public void drawImage()
{
    mTextureRender.drawFrame(mSurfaceTexture);
}


        public void OnFrameAvailable(Android.Graphics.SurfaceTexture st)
{
    if (VERBOSE) Android.Util.Log.Debug(TAG, "new frame available");
    //synchronized(mFrameSyncObject) 
                    {
        if (mFrameAvailable)
        {
            throw new RuntimeException("mFrameAvailable already set, frame could be dropped");
        }
        mFrameAvailable = true;
        //mFrameSyncObject.notifyAll();
    }
}
    }


    /**
     * Code for rendering a texture onto a surface using OpenGL ES 2.0.
     */
    private  class STextureRender
{
    private static  int FLOAT_SIZE_BYTES = 4;
    private static  int TRIANGLE_VERTICES_DATA_STRIDE_BYTES = 5 * FLOAT_SIZE_BYTES;
    private static  int TRIANGLE_VERTICES_DATA_POS_OFFSET = 0;
    private static  int TRIANGLE_VERTICES_DATA_UV_OFFSET = 3;
    private  float[] mTriangleVerticesData = {
                // X, Y, Z, U, V
                -1.0f, -1.0f, 0, 0.0f, 0.0f,
                 1.0f, -1.0f, 0, 1.0f, 0.0f,
                -1.0f,  1.0f, 0, 0.0f, 1.0f,
                 1.0f,  1.0f, 0, 1.0f, 1.0f,
        };

    private FloatBuffer mTriangleVertices;

    private static  string VERTEX_SHADER =
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

        Matrix.SetIdentityM(mSTMatrix, 0);
    }

    public int getTextureId()
    {
        return mTextureID;
    }

    public void drawFrame(Android.Graphics.SurfaceTexture st)
    {
        checkGlError("onDrawFrame start");
        st.GetTransformMatrix(mSTMatrix);

        // (optional) clear to green so we can see if we're failing to set pixels
        GLES20.GlClearColor(0.0f, 1.0f, 0.0f, 1.0f);
        GLES20.GlClear(GLES20.GlDepthBufferBit | GLES20.GlColorBufferBit);

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

        Matrix.SetIdentityM(mMVPMatrix, 0);
        GLES20.GlUniformMatrix4fv(muMVPMatrixHandle, 1, false, mMVPMatrix, 0);
        GLES20.GlUniformMatrix4fv(muSTMatrixHandle, 1, false, mSTMatrix, 0);

        GLES20.GlDrawArrays(GLES20.GlTriangleStrip, 0, 4);
        checkGlError("glDrawArrays");

        // IMPORTANT: on some devices, if you are sharing the external texture between two
        // contexts, one context may not see updates to the texture unless you un-bind and
        // re-bind it.  If you're not using shared EGL contexts, you don't need to bind
        // texture 0 here.
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
            Android.Util.Log.Error(TAG, "Could not compile shader " + shaderType + ":");
            Android.Util.Log.Error(TAG, " " + GLES20.GlGetShaderInfoLog(shader));
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
            Android.Util.Log.Error(TAG, "Could not create program");
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
            Android.Util.Log.Error(TAG, "Could not link program: ");
            Android.Util.Log.Error(TAG, GLES20.GlGetProgramInfoLog(program));
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




}