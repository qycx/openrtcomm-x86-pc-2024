
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Qycx.Gleslibrary;
using Java.IO;
using Java.Lang;
using Java.Util.Concurrent;
using qy;
using qy.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hardware = Android.Hardware;
using Thread = Java.Lang.Thread;
using Opengl = Android.Opengl;
using IOException = Java.IO.IOException;
using qyMessengerApp.Android;
using AndroidX.Fragment.App;

namespace qyMessengerApp.Droid
{
    public class Fragment_test2 : Fragment, TextureView.ISurfaceTextureListener, SeekBar.IOnSeekBarChangeListener
    {
        static string TAG = "fg_dlgTalk_av1";


        private static  int DEFAULT_ZOOM_PERCENT = 0;      // 0-100
    private static  int DEFAULT_SIZE_PERCENT = 50;     // 0-100
    private static  int DEFAULT_ROTATE_PERCENT = 0;    // 0-100

        // Requested values; actual may differ.
        private static int REQ_CAMERA_WIDTH = 480;//1280;
    private static int REQ_CAMERA_HEIGHT = 640;//720;
    private static  int REQ_CAMERA_FPS = 30;


        //
        View m_fg_view;
        //
        public MyTextureView mTextureView;

        private RenderThread mRenderThread;

        // Receives messages from renderer thread.
        private MainHandler mHandler;

        // User controls.
        private SeekBar mZoomBar;
        private SeekBar mSizeBar;
        private SeekBar mRotateBar;

        // These values are passed to us by the camera/render thread, and displayed in the UI.
        // We could also just peek at the values in the RenderThread object, but we'd need to
        // synchronize access carefully.
        private int mCameraPreviewWidth, mCameraPreviewHeight;
        private float mCameraPreviewFps;
        private int mRectWidth, mRectHeight;
        private int mZoomWidth, mZoomHeight;
        private int mRotateDeg;



        //
        //
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            //
            View view = inflater.Inflate(Resource.Layout.layout_fg_test, container, false);

            //
            m_fg_view = view;

            //
            mHandler = new MainHandler(this);

            //
            mTextureView = (MyTextureView)view.FindViewById(Resource.Id.texture);

            //
            mTextureView.SurfaceTextureListener = this;

            //
            mZoomBar = (SeekBar)view.FindViewById(Resource.Id.tfcZoom_seekbar);
            mSizeBar = (SeekBar)view.FindViewById(Resource.Id.tfcSize_seekbar);
            mRotateBar = (SeekBar)view.FindViewById(Resource.Id.tfcRotate_seekbar);
            mZoomBar.Progress=(DEFAULT_ZOOM_PERCENT);
            mSizeBar.Progress=(DEFAULT_SIZE_PERCENT);
            mRotateBar.Progress=(DEFAULT_ROTATE_PERCENT);
            mZoomBar.SetOnSeekBarChangeListener(this);
            mSizeBar.SetOnSeekBarChangeListener(this);
            mRotateBar.SetOnSeekBarChangeListener(this);



            //
            mRenderThread = new RenderThread(mHandler);
            mRenderThread.Start();


            //
            mRenderThread.waitUntilReady();

            

            //
            Button btn_request = (Button)view.FindViewById(Resource.Id.btn_request);
            btn_request.Text = "test fragment";

            btn_request.Click += Btn_request_Click;

            
            
            //
            return view;

        }

        private void Btn_request_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int ii = 0;
        }

        //
        public void OnSurfaceTextureAvailable(SurfaceTexture? surface, int width, int height)
        {
            //throw new NotImplementedException();
            this.mRenderThread.mHandler.sendSurfaceAvailable(surface, true);

        errLabel:
            return;
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            //throw new NotImplementedException();
#if false
                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (syncObj.sync("", this.hMutex) != 0) goto errLabel;
                    mSurfaceTexture = null;
                }

                //
                if (Fragment_dlgTalk_av.sReleaseInCallback)
                {
                    //Log.i(TAG, "Allowing TextureView to release SurfaceTexture");
                }


            errLabel:
                return Fragment_dlgTalk_av.sReleaseInCallback;
#endif
            //
            if (this.mRenderThread != null)
            {
                this.mRenderThread.mHandler.sendSurfaceDestroyed();
            }

            //
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            //throw new NotImplementedException();
            int ii = 0;
            this.mRenderThread.mHandler.sendSurfaceChanged(0, width, height);
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            //throw new NotImplementedException();
        }



        //
        // SeekBar.OnSeekBarChangeListener
        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (mRenderThread == null)
            {
                // Could happen if we programmatically update the values after setting a listener
                // but before starting the thread.  Also, easy to cause this by scrubbing the seek
                // bar with one finger then tapping "recents" with another.
                Log.Warn(TAG, "Ignoring onProgressChanged received w/o RT running");
                return;
            }
            RenderHandler rh = mRenderThread.getHandler();

            // "progress" ranges from 0 to 100
            if (seekBar == mZoomBar)
            {
                //Log.v(TAG, "zoom: " + progress);
                rh.sendZoomValue(progress);
            }
            else if (seekBar == mSizeBar)
            {
                //Log.v(TAG, "size: " + progress);
                rh.sendSizeValue(progress);
            }
            else if (seekBar == mRotateBar)
            {
                //Log.v(TAG, "rotate: " + progress);
                string str;
                str = string.Format("rotate {0}", progress);
                qyFuncs.showInfo_open(0, null, str);

                //
                rh.sendRotateValue(progress);
            }
            else
            {
                throw new RuntimeException("unknown seek bar");
            }

            // If we're getting preview frames quickly enough we don't really need this, but
            // we don't want to have chunky-looking resize movement if the camera is slow.
            // OTOH, if we get the updates too quickly (60fps camera?), this could jam us
            // up and cause us to run behind.  So use with caution.
            rh.sendRedraw();
        }

        // SeekBar.OnSeekBarChangeListener
    public void OnStartTrackingTouch(SeekBar seekBar) { }
           // SeekBar.OnSeekBarChangeListener
    public void OnStopTrackingTouch(SeekBar seekBar) { }

        //
        public bool OnTouchEvent(MotionEvent e)
        {
            float x = e.GetX();
            float y = e.GetY();

            switch (e.Action)
            {
                case MotionEventActions.Move:
                case MotionEventActions.Down:
                    //Log.v(TAG, "onTouchEvent act=" + e.getAction() + " x=" + x + " y=" + y);
                    if (mRenderThread != null)
                    {
                        RenderHandler rh = mRenderThread.getHandler();
                        rh.sendPosition((int)x, (int)y);

                        // Forcing a redraw can cause sluggish-looking behavior if the touch
                        // events arrive quickly.
                        //rh.sendRedraw();
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        /**
         * Updates the current state of the controls.
         */
        private void updateControls()
        {
            string str = string.Format("CameraParams {0} {1} {2}", mCameraPreviewWidth,
                    mCameraPreviewHeight, mCameraPreviewFps);
            TextView tv = (TextView)m_fg_view.FindViewById(Resource.Id.tfcCameraParams_text);
            tv.Text=(str);

            str = string.Format("RectSize {0} {1}", mRectWidth, mRectHeight);
            tv = (TextView)m_fg_view.FindViewById(Resource.Id.tfcRectSize_text);
            tv.Text=(str);

            str = string.Format("ZoomArea {0} {1}", mZoomWidth, mZoomHeight);
            tv = (TextView)m_fg_view.FindViewById(Resource.Id.tfcZoomArea_text);
            tv.Text=(str);
        }



#if true

        //
        public class MainHandler : Handler
    {
        private const  int MSG_SEND_CAMERA_PARAMS0 = 0;
    private const  int MSG_SEND_CAMERA_PARAMS1 = 1;
    private const  int MSG_SEND_RECT_SIZE = 2;
    private const  int MSG_SEND_ZOOM_AREA = 3;
    private const  int MSG_SEND_ROTATE_DEG = 4;

            //private WeakReference<TextureFromCameraActivity> mWeakActivity;
            Fragment_test2 m_fg;

    public MainHandler(Fragment_test2 fg)
    {
            //mWeakActivity = new WeakReference<TextureFromCameraActivity>(activity);
            m_fg = (Fragment_test2)fg;
    }

    /**
     * Sends the updated camera parameters to the main thread.
     * <p>
     * Call from render thread.
     */
    public void sendCameraParams(int width, int height, float fps)
    {
        // The right way to do this is to bundle them up into an object.  The lazy
        // way is to send two messages.
        SendMessage(ObtainMessage(MSG_SEND_CAMERA_PARAMS0, width, height));
        SendMessage(ObtainMessage(MSG_SEND_CAMERA_PARAMS1, (int)(fps * 1000), 0));
    }

    /**
     * Sends the updated rect size to the main thread.
     * <p>
     * Call from render thread.
     */
    public void sendRectSize(int width, int height)
    {
        SendMessage(ObtainMessage(MSG_SEND_RECT_SIZE, width, height));
    }

    /**
     * Sends the updated zoom area to the main thread.
     * <p>
     * Call from render thread.
     */
    public void sendZoomArea(int width, int height)
    {
        SendMessage(ObtainMessage(MSG_SEND_ZOOM_AREA, width, height));
    }

    /**
     * Sends the updated zoom area to the main thread.
     * <p>
     * Call from render thread.
     */
    public void sendRotateDeg(int rot)
    {
        SendMessage(ObtainMessage(MSG_SEND_ROTATE_DEG, rot, 0));
    }

        public override void HandleMessage(Message msg)
    {
                Fragment_test2 activity = m_fg;
        if (activity == null)
        {
            //Log.d(TAG, "Got message for dead activity");
            return;
        }

#if true
            switch (msg.What)
        {
            case MSG_SEND_CAMERA_PARAMS0:
                {
                    activity.mCameraPreviewWidth = msg.Arg1;
                    activity.mCameraPreviewHeight = msg.Arg2;
                    break;
                }
            case MSG_SEND_CAMERA_PARAMS1:
                {
                    activity.mCameraPreviewFps = msg.Arg1 / 1000.0f;
                    activity.updateControls();
                    break;
                }
            case MSG_SEND_RECT_SIZE:
                {
                    activity.mRectWidth = msg.Arg1;
                    activity.mRectHeight = msg.Arg2;
                    activity.updateControls();
                    break;
                }
            case MSG_SEND_ZOOM_AREA:
                {
                    activity.mZoomWidth = msg.Arg1;
                    activity.mZoomHeight = msg.Arg2;
                    activity.updateControls();
                    break;
                }
            case MSG_SEND_ROTATE_DEG:
                {
                    activity.mRotateDeg = msg.Arg1;
                    activity.updateControls();
                    break;
                }
            default:
                throw new RuntimeException("Unknown message " + msg.What);
        }
#endif
    }
}


/**
* Thread that handles all rendering and camera operations.
*/
public class RenderThread : Thread, SurfaceTexture.IOnFrameAvailableListener
    {
        // Object must be created on render thread to get correct Looper, but is used from
        // UI thread, so we need to declare it volatile to ensure the UI thread sees a fully
        // constructed object.
        public RenderHandler mHandler;

    // Used to wait for the thread to start.
    private object mStartLock = new object();
    private bool mReady = false;

    private MainHandler mMainHandler;

            //
            bool m_bUse_camera1 = false;
                        
            //
    private Hardware.Camera mCamera1;

            //
            public MyCamera myCamera;


            //
            private int mCameraPreviewWidth, mCameraPreviewHeight;

            //
    private EglCore mEglCore;
    private WindowSurface mWindowSurface;
    private int mWindowSurfaceWidth;
    private int mWindowSurfaceHeight;

    // Receives the output from the camera preview.
    private SurfaceTexture mCameraTexture;

    // Orthographic projection matrix.
    private float[] mDisplayProjectionMatrix = new float[16];

    private Texture2dProgram mTexProgram;
    private  static ScaledDrawable2d mRectDrawable =
                new ScaledDrawable2d(Drawable2d.Prefab.Rectangle);
    private  Sprite2d mRect = new Sprite2d(mRectDrawable);

    private int mZoomPercent = DEFAULT_ZOOM_PERCENT;
    private int mSizePercent = DEFAULT_SIZE_PERCENT;
    private int mRotatePercent = DEFAULT_ROTATE_PERCENT;
    private float mPosX, mPosY;


            //
            public TMP_frameBuffer tmpFrameBuffer = new TMP_frameBuffer();

    /**
     * Constructor.  Pass in the MainHandler, which allows us to send stuff back to the
     * Activity.
     */
    public RenderThread(MainHandler handler)
    {
        mMainHandler = handler;
    }

    /**
     * Thread entry point.
     */
        public override void Run()
    {
        Looper.Prepare();

        // We need to create the Handler before reporting ready.
        mHandler = new RenderHandler(this);
#if false
                //synchronized(mStartLock) 
                {
            mReady = true;
            //mStartLock.notify();    // signal waitUntilReady()
        }
#endif

        // Prepare EGL and open the camera before we start handling messages.
        mEglCore = new EglCore(null, 0);
        openCamera(REQ_CAMERA_WIDTH, REQ_CAMERA_HEIGHT, REQ_CAMERA_FPS);

                //



                //
        Looper.Loop();

                //
        Log.Debug(TAG, "looper quit");
                if (this.tmpFrameBuffer.bDone_createEnv)
                {
                    this.tmpFrameBuffer.deleteEnv();
                    this.tmpFrameBuffer.bDone_createEnv = false;
                }

                //
        releaseCamera();
        releaseGl();
        mEglCore.Release();

        //synchronized(mStartLock) 
                    {
            mReady = false;
        }

                //
                qyFuncs.showInfo_open(0, null, "renderThread.run ends");

            }

            //
            public int notifyCameraOpen()
    {          
                //
                mReady = true;

                //
                if (!this.m_bUse_camera1)
                {
                    mCameraPreviewWidth = myCamera.mVideoSize.Width;
                    mCameraPreviewHeight = myCamera.mVideoSize.Height;
                    int thousandFps = 30000;
                    //
                    mMainHandler.sendCameraParams(mCameraPreviewWidth, mCameraPreviewHeight,
                            thousandFps / 1000.0f);
                }


                //
                return 0;     
    }

    /**
     * Waits until the render thread is ready to receive messages.
     * <p>
     * Call from the UI thread.
     */
    public void waitUntilReady()
    {
        //synchronized(mStartLock) 
                    {
            while (!mReady)
            {
                try
                {
                            //mStartLock.wait();
                            qyFuncs.mySleep(1);
                }
                catch (InterruptedException ie) { /* not expected */ }
            }
        }
    }

    /**
     * Shuts everything down.
     */
    public void shutdown()
    {
        Log.Debug(TAG, "shutdown");
        Looper.MyLooper().Quit();
    }

    /**
     * Returns the render thread's Handler.  This may be called from any thread.
     */
    public RenderHandler getHandler()
    {
        return mHandler;
    }

    /**
     * Handles the surface-created callback from SurfaceView.  Prepares GLES and the Surface.
     */
    public void surfaceAvailable(SurfaceTexture st, bool newSurface)
    {
                //Surface surface = holder.getSurface();
                //mWindowSurface = new WindowSurface(mEglCore, surface, false);
                mWindowSurface = new WindowSurface(mEglCore, st);
        mWindowSurface.MakeCurrent();

        // Create and configure the SurfaceTexture, which will receive frames from the
        // camera.  We set the textured rect's program to render from it.
        mTexProgram = new Texture2dProgram(Texture2dProgram.ProgramType.TextureExt);
        int textureId = mTexProgram.CreateTextureObject();
        mCameraTexture = new SurfaceTexture(textureId);
        mRect.SetTexture(textureId);

        //if (!newSurface)
        {
            // This Surface was established on a previous run, so no surfaceChanged()
            // message is forthcoming.  Finish the surface setup now.
            //
            // We could also just call this unconditionally, and perhaps do an unnecessary
            // bit of reallocating if a surface-changed message arrives.
            mWindowSurfaceWidth = mWindowSurface.Width;
            mWindowSurfaceHeight = mWindowSurface.Height;
            finishSurfaceSetup();
        }

        mCameraTexture.SetOnFrameAvailableListener(this);

                //
                this.setRotate(25);
    }

    /**
     * Releases most of the GL resources we currently hold (anything allocated by
     * surfaceAvailable()).
     * <p>
     * Does not release EglCore.
     */
    private void releaseGl()
    {
        GlUtil.CheckGlError("releaseGl start");

        if (mWindowSurface != null)
        {
            mWindowSurface.Release();
            mWindowSurface = null;
        }
        if (mTexProgram != null)
        {
            mTexProgram.Release();
            mTexProgram = null;
        }
        GlUtil.CheckGlError("releaseGl done");

        mEglCore.MakeNothingCurrent();
    }

    /**
     * Handles the surfaceChanged message.
     * <p>
     * We always receive surfaceChanged() after surfaceCreated(), but surfaceAvailable()
     * could also be called with a Surface created on a previous run.  So this may not
     * be called.
     */
    public void surfaceChanged(int width, int height)
    {
        Log.Debug(TAG, "RenderThread surfaceChanged " + width + "x" + height);

        mWindowSurfaceWidth = width;
        mWindowSurfaceHeight = height;
        finishSurfaceSetup();
    }

    /**
     * Handles the surfaceDestroyed message.
     */
    public void surfaceDestroyed()
    {
        // In practice this never appears to be called -- the activity is always paused
        // before the surface is destroyed.  In theory it could be called though.
        Log.Debug(TAG, "RenderThread surfaceDestroyed");
        releaseGl();
    }

    /**
     * Sets up anything that depends on the window size.
     * <p>
     * Open the camera (to set mCameraAspectRatio) before calling here.
     */
    private void finishSurfaceSetup()
    {
        int width = mWindowSurfaceWidth;
        int height = mWindowSurfaceHeight;
        Log.Debug(TAG, "finishSurfaceSetup size=" + width + "x" + height +
                " camera=" + mCameraPreviewWidth + "x" + mCameraPreviewHeight);

        // Use full window.
        GLES20.GlViewport(0, 0, width, height);

        // Simple orthographic projection, with (0,0) in lower-left corner.
        Opengl.Matrix.OrthoM(mDisplayProjectionMatrix, 0, 0, width, 0, height, -1, 1);

        // Default position is center of screen.
        mPosX = width / 2.0f;
        mPosY = height / 2.0f;

        updateGeometry();

        // Ready to go, start the camera.
        Log.Debug(TAG, "starting camera preview");
                if (m_bUse_camera1)
                {
                    try
                    {
                        mCamera1.SetPreviewTexture(mCameraTexture);
                    }
                    catch (IOException ioe)
                    {
                        throw new RuntimeException(ioe);
                    }
                    mCamera1.StartPreview();
                }
                else
                {
                    qyFuncs.traceLog("not finished, startPreview");
                    myCamera.startPreview(mCameraTexture);
                }
    }

    /**
     * Updates the geometry of mRect, based on the size of the window and the current
     * values set by the UI.
     */
    private void updateGeometry()
    {
        int width = mWindowSurfaceWidth;
        int height = mWindowSurfaceHeight;

        int smallDim = Java.Lang.Math.Min(width, height);
        // Max scale is a bit larger than the screen, so we can show over-size.
        float scaled = smallDim * (mSizePercent / 100.0f) * 1.25f;
        float cameraAspect = (float)mCameraPreviewWidth / mCameraPreviewHeight;
        int newWidth = Java.Lang.Math.Round(scaled * cameraAspect);
        int newHeight = Java.Lang.Math.Round(scaled);

        float zoomFactor = 1.0f - (mZoomPercent / 100.0f);
        int rotAngle = Java.Lang.Math.Round(360 * (mRotatePercent / 100.0f));

        mRect.SetScale(newWidth, newHeight);
        mRect.SetPosition(mPosX, mPosY);
       mRect.Rotation=(rotAngle);
        mRectDrawable.setScale(zoomFactor);

        mMainHandler.sendRectSize(newWidth, newHeight);
        mMainHandler.sendZoomArea(Java.Lang.Math.Round(mCameraPreviewWidth * zoomFactor),
                Java.Lang.Math.Round(mCameraPreviewHeight * zoomFactor));
        mMainHandler.sendRotateDeg(rotAngle);
    }

            int cnt;

       // SurfaceTexture.OnFrameAvailableListener; runs on arbitrary thread
        public  void OnFrameAvailable(SurfaceTexture surfaceTexture)
    {
                //
                bool bDbg = false;
                string str;

                if (bDbg)
                {
                    str = string.Format("onFrameAvail {0}", cnt);
                    qyFuncs.showInfo_open(0, null, str);
                }
                cnt++;

                //
        mHandler.sendFrameAvailable();
    }

    /**
     * Handles incoming frame of data from the camera.
     */
    public void frameAvailable()
    {
        mCameraTexture.UpdateTexImage();

                //
                int w = mCameraPreviewWidth;
                int h = mCameraPreviewHeight;

                //
                //qyFuncs_Droid.myGLClearError();
                bool bRtt = true;// false;// true;

                //
                if (!this.tmpFrameBuffer.bDone_createEnv)
                {
                    this.tmpFrameBuffer.createEnv(w, h,"fg_test2.frameAvail");
                    this.tmpFrameBuffer.bDone_createEnv = true;
                }
                //
                if (bRtt)
                {
                    this.tmpFrameBuffer.bindFrameBufferInfo();
                    //
                    GLES20.GlViewport(0, 0, w, h);

                }

                //      
                draw();

                //
                if (bRtt)
                {
                    this.tmpFrameBuffer.unbindFrameBufferInfo();
                }

                //
#if false
                Bitmap bmp = qyFuncs_Droid.texture2Bmp(this.tmpFrameBuffer.mTexture[0], w, h);
                GuiShare.pf_dumpBmp(bmp, "tmpTex");
                bmp.Recycle();
#endif

                //
#if DEBUG
                //Fragment_test2.m_test_textureId = this.tmpFrameBuffer.mTexture[0];
#endif


                //
                //this.tmpFrameBuffer.deleteEnv();

                //
                int ii = 0;

    }

    /**
     * Draws the scene and submits the buffer.
     */
    public void draw()
    {
        GlUtil.CheckGlError("draw start");

        GLES20.GlClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        GLES20.GlClear(GLES20.GlColorBufferBit);
        mRect.Draw(mTexProgram, mDisplayProjectionMatrix);
        mWindowSurface.SwapBuffers();

        GlUtil.CheckGlError("draw done");
    }

    public void setZoom(int percent)
    {
        mZoomPercent = percent;
        updateGeometry();
    }

    public void setSize(int percent)
    {
        mSizePercent = percent;
        updateGeometry();
    }

    public void setRotate(int percent)
    {
        mRotatePercent = percent;
        updateGeometry();
    }

    public void setPosition(int x, int y)
    {
        mPosX = x;
        mPosY = mWindowSurfaceHeight - y;   // GLES is upside-down
        updateGeometry();
    }

            private void openCamera(int desiredWidth, int desiredHeight, int desiredFps)
            {
                if ( m_bUse_camera1)
                {
                    openCamera1(desiredWidth, desiredHeight, desiredFps);
                    //
                    this.notifyCameraOpen();
                }
                else
                {
                    this.myCamera = new MyCamera(this.mHandler);                    
                    //
                    this.myCamera.openCamera(desiredWidth, desiredHeight,true);
                }
            }

            private void releaseCamera()
            {
                if (m_bUse_camera1)
                {
                    releaseCamera1();
                }
                else
                {
                    if (this.myCamera != null)
                    {
                        myCamera.closeCamera();
                    }
                }
            }


    /**
     * Opens a camera, and attempts to establish preview mode at the specified width
     * and height with a fixed frame rate.
     * <p>
     * Sets mCameraPreviewWidth / mCameraPreviewHeight.
     */
    private void openCamera1(int desiredWidth, int desiredHeight, int desiredFps)
    {
        if (mCamera1 != null)
        {
            throw new RuntimeException("camera already initialized");
        }

        Hardware.Camera.CameraInfo info = new Hardware.Camera.CameraInfo();

        // Try to find a front-facing camera (e.g. for videoconferencing).
        int numCameras = Hardware.Camera.NumberOfCameras;
        for (int i = 0; i < numCameras; i++)
        {
            Hardware.Camera.GetCameraInfo(i, info);
            if (info.Facing == Hardware.CameraFacing.Front)
            {
                mCamera1 =Hardware.Camera.Open(i);
                break;
            }
        }
        if (mCamera1 == null)
        {
            Log.Debug(TAG, "No front-facing camera found; opening default");
            mCamera1 = Hardware.Camera.Open();    // opens first back-facing camera
        }
        if (mCamera1 == null)
        {
            throw new RuntimeException("Unable to open camera");
        }

       Hardware.Camera.Parameters parms = mCamera1.GetParameters();

        CameraUtils.choosePreviewSize(parms, desiredWidth, desiredHeight);

        // Try to set the frame rate to a constant value.
        int thousandFps = CameraUtils.chooseFixedPreviewFps(parms, desiredFps * 1000);

        // Give the camera a hint that we're recording video.  This can have a big
        // impact on frame rate.
        parms.SetRecordingHint(true);

        mCamera1.SetParameters(parms);

        int[] fpsRange = new int[2];
        Hardware.Camera.Size mCameraPreviewSize = parms.PreviewSize;
        parms.GetPreviewFpsRange(fpsRange);
        string previewFacts = mCameraPreviewSize.Width + "x" + mCameraPreviewSize.Height;
        if (fpsRange[0] == fpsRange[1])
        {
            previewFacts += " @" + (fpsRange[0] / 1000.0) + "fps";
        }
        else
        {
            previewFacts += " @[" + (fpsRange[0] / 1000.0) +
                    " - " + (fpsRange[1] / 1000.0) + "] fps";
        }
        Log.Info(TAG, "Camera config: " + previewFacts);

        mCameraPreviewWidth = mCameraPreviewSize.Width;
        mCameraPreviewHeight = mCameraPreviewSize.Height;
        mMainHandler.sendCameraParams(mCameraPreviewWidth, mCameraPreviewHeight,
                thousandFps / 1000.0f);
    }

    /**
     * Stops camera preview, and releases the camera to the system.
     */
    private void releaseCamera1()
    {
        if (mCamera1 != null)
        {
            mCamera1.StopPreview();
            mCamera1.Release();
            mCamera1 = null;
            Log.Debug(TAG, "releaseCamera -- done");
        }
    }

            //
        }


        /**
         * Handler for RenderThread.  Used for messages sent from the UI thread to the render thread.
         * <p>
         * The object is created on the render thread, and the various "send" methods are called
         * from the UI thread.
         */
        public class RenderHandler :Handler
{
        private const  int MSG_SURFACE_AVAILABLE = 0;
private const  int MSG_SURFACE_CHANGED = 1;
private const  int MSG_SURFACE_DESTROYED = 2;
private const  int MSG_SHUTDOWN = 3;
private const  int MSG_FRAME_AVAILABLE = 4;
private const  int MSG_ZOOM_VALUE = 5;
private const  int MSG_SIZE_VALUE = 6;
private const  int MSG_ROTATE_VALUE = 7;
private const  int MSG_POSITION = 8;
private const  int MSG_REDRAW = 9;

// This shouldn't need to be a weak ref, since we'll go away when the Looper quits,
// but no real harm in it.
private RenderThread mWeakRenderThread;

/**
 * Call from render thread.
 */
public RenderHandler(RenderThread rt)
{
                mWeakRenderThread = rt;// new WeakReference<RenderThread>(rt);
}

/**
 * Sends the "surface available" message.  If the surface was newly created (i.e.
 * this is called from surfaceCreated()), set newSurface to true.  If this is
 * being called during Activity startup for a previously-existing surface, set
 * newSurface to false.
 * <p>
 * The flag tells the caller whether or not it can expect a surfaceChanged() to
 * arrive very soon.
 * <p>
 * Call from UI thread.
 */
public void sendSurfaceAvailable(SurfaceTexture holder, bool newSurface)
{
    SendMessage(ObtainMessage(MSG_SURFACE_AVAILABLE,
            newSurface ? 1 : 0, 0, holder));
}

/**
 * Sends the "surface changed" message, forwarding what we got from the SurfaceHolder.
 * <p>
 * Call from UI thread.
 */
public void sendSurfaceChanged( int format, int width,
                int height) {
    // ignore format
    SendMessage(ObtainMessage(MSG_SURFACE_CHANGED, width, height));
}

/**
 * Sends the "shutdown" message, which tells the render thread to halt.
 * <p>
 * Call from UI thread.
 */
public void sendSurfaceDestroyed()
{
    SendMessage(ObtainMessage(MSG_SURFACE_DESTROYED));
}

/**
 * Sends the "shutdown" message, which tells the render thread to halt.
 * <p>
 * Call from UI thread.
 */
public void sendShutdown()
{
    SendMessage(ObtainMessage(MSG_SHUTDOWN));
}

/**
 * Sends the "frame available" message.
 * <p>
 * Call from UI thread.
 */
public void sendFrameAvailable()
{
    SendMessage(ObtainMessage(MSG_FRAME_AVAILABLE));
}

/**
 * Sends the "zoom value" message.  "progress" should be 0-100.
 * <p>
 * Call from UI thread.
 */
public void sendZoomValue(int progress)
{
    SendMessage(ObtainMessage(MSG_ZOOM_VALUE, progress, 0));
}

/**
 * Sends the "size value" message.  "progress" should be 0-100.
 * <p>
 * Call from UI thread.
 */
public void sendSizeValue(int progress)
{
    SendMessage(ObtainMessage(MSG_SIZE_VALUE, progress, 0));
}

/**
 * Sends the "rotate value" message.  "progress" should be 0-100.
 * <p>
 * Call from UI thread.
 */
public void sendRotateValue(int progress)
{
    SendMessage(ObtainMessage(MSG_ROTATE_VALUE, progress, 0));
}

/**
 * Sends the "position" message.  Sets the position of the rect.
 * <p>
 * Call from UI thread.
 */
public void sendPosition(int x, int y)
{
    SendMessage(ObtainMessage(MSG_POSITION, x, y));
}

/**
 * Sends the "redraw" message.  Forces an immediate redraw.
 * <p>
 * Call from UI thread.
 */
public void sendRedraw()
{
    SendMessage(ObtainMessage(MSG_REDRAW));
}


            //
           public override void HandleMessage(Message msg)
            {
                int what = msg.What;
    //Log.d(TAG, "RenderHandler [" + this + "]: what=" + what);

    RenderThread renderThread = mWeakRenderThread;
    if (renderThread == null)
    {
        Log.Warn(TAG, "RenderHandler.handleMessage: weak ref is null");
        return;
    }

    switch (what)
    {
        case MSG_SURFACE_AVAILABLE:
            renderThread.surfaceAvailable((SurfaceTexture)msg.Obj, msg.Arg1 != 0);
            break;
        case MSG_SURFACE_CHANGED:
            renderThread.surfaceChanged(msg.Arg1, msg.Arg2);
            break;
        case MSG_SURFACE_DESTROYED:
            renderThread.surfaceDestroyed();
            break;
        case MSG_SHUTDOWN:
            renderThread.shutdown();
            break;
        case MSG_FRAME_AVAILABLE:
            renderThread.frameAvailable();
            break;
        case MSG_ZOOM_VALUE:
            renderThread.setZoom(msg.Arg1);
            break;
        case MSG_SIZE_VALUE:
            renderThread.setSize(msg.Arg1);
            break;
        case MSG_ROTATE_VALUE:
            renderThread.setRotate(msg.Arg1);
            break;
        case MSG_POSITION:
            renderThread.setPosition(msg.Arg1, msg.Arg2);
            break;
        case MSG_REDRAW:
            renderThread.draw();
            break;
                    
        case Consts.CONST_msg_cameraOpen:                       
             renderThread.notifyCameraOpen();
             break;

        default:
            throw new RuntimeException("unknown message " + what);
    }
}
    
        
        
        }


#endif


                //
                public int doClean_ifActivityIsFinishing()
        {
            //
            qyFuncs.showInfo_open(0, null, "fg_dlgTalk_av2.doClean_ifActivityIsFinished enters");

            //
            if (mRenderThread != null)
            {
                RenderHandler rh = mRenderThread.getHandler();
                if (rh != null)
                {
                    rh.sendShutdown();
                }
                try
                {
                    mRenderThread.Join();
                }
                catch (InterruptedException ie)
                {

                }
                mRenderThread = null;
            }

            //
            qyFuncs.showInfo_open(0, null, "fg_dlgTalk_av2.doClean_ifActivityIsFinished leaves");

            return 0;
        }




        //
    }

}


