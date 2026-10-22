using Android.Hardware;
using Android.Content;
using Android.Graphics;
using Opengl=Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Qycx.Gleslibrary;
using Java.IO;
using Java.Lang;
using qy;
using qy.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hardware = Android.Hardware;
using Thread = Java.Lang.Thread;
using Android.Opengl;
using IOException = Java.IO.IOException;
namespace qyMessengerApp.Droid
{
    /**
* Thread that handles all rendering and camera operations.
*/
    public class camera_RenderThread : Thread, SurfaceTexture.IOnFrameAvailableListener
    {
        string TAG = "camera_RenderThread";

        //
        private static int DEFAULT_ZOOM_PERCENT = 0;      // 0-100
        private static int DEFAULT_SIZE_PERCENT = 50;     // 0-100
        private static int DEFAULT_ROTATE_PERCENT = 0;    // 0-100

        // Requested values; actual may differ.
        private static int REQ_CAMERA_WIDTH = 480;//1280;
        private static int REQ_CAMERA_HEIGHT = 640;//720;
        private static int REQ_CAMERA_FPS = 30;

        //
        private const int MSG_SEND_CAMERA_PARAMS0 = 0;
        private const int MSG_SEND_CAMERA_PARAMS1 = 1;
        private const int MSG_SEND_RECT_SIZE = 2;
        private const int MSG_SEND_ZOOM_AREA = 3;
        private const int MSG_SEND_ROTATE_DEG = 4;



        // Object must be created on render thread to get correct Looper, but is used from
        // UI thread, so we need to declare it volatile to ensure the UI thread sees a fully
        // constructed object.
        public camera_RenderHandler mHandler;

        // Used to wait for the thread to start.
        private object mStartLock = new object();
        private bool mReady = false;

        //
        private bool mThreadEnd = false;

        //
        private Handler mMainHandler;
        EGLContext mEglContext_shared;
        int miIndex_sharedObj;

        //
        //bool m_bOrientation_landscape;
        bool m_bFront;
        //
        SurfaceOrientation m_surfaceOrientation;


        //
        bool m_bUse_camera1 = false;

        //
        private Hardware.Camera mCamera1;

        //
        public MyCamera myCamera;


        //
        public int mCameraPreviewWidth, mCameraPreviewHeight;

        //
        private EglCore mEglCore;
        private WindowSurface mWindowSurface;
        private int mWindowSurfaceWidth;
        private int mWindowSurfaceHeight;

        // Receives the output from the camera preview.
        private SurfaceTexture mCameraTexture;

        // Orthographic projection matrix.
        private float[] mDisplayProjectionMatrix = new float[16];
        private float[] mDisplayProjectionMatrix_invert = new float[16];


        private Texture2dProgram mTexProgram;
        private static ScaledDrawable2d mRectDrawable =
                    new ScaledDrawable2d(Drawable2d.Prefab.Rectangle);
        private Sprite2d mRect = new Sprite2d(mRectDrawable);

        private int mZoomPercent = DEFAULT_ZOOM_PERCENT;
        private int mSizePercent = DEFAULT_SIZE_PERCENT;
        private int mRotatePercent = DEFAULT_ROTATE_PERCENT;
        private float mPosX, mPosY;

        //
        //
#if false
        public static int m_test_textureId;
        public static int m_iW_tex;
        public static int m_iH_tex;
#endif

        //
        public TMP_frameBuffer tmpFrameBuffer_0 = new TMP_frameBuffer();        // camera_w*camera_h
        public TMP_frameBuffer tmpFrameBuffer_int = new TMP_frameBuffer();      // camera_big*camera_big
        public TMP_frameBuffer tmpFrameBuffer_rotate = new TMP_frameBuffer();  // 
        public TMP_frameBuffer tmpFrameBuffer_o = new TMP_frameBuffer();       // h*w 

        //
        public SimpleTexture m_simpleTex_0_int;
        public SimpleTexture m_simpleTex_rotate;
        public SimpleTexture m_simpleTex_o;


        /**
         * Constructor.  Pass in the MainHandler, which allows us to send stuff back to the
         * Activity.
         */
        public camera_RenderThread(Handler mainHandler, EGLContext eglContext_shared, int index_sharedObj, bool bFront, SurfaceOrientation surfaceOrientation)
        {
            mMainHandler = mainHandler;
            mEglContext_shared = eglContext_shared;
            //
            miIndex_sharedObj = index_sharedObj;
            //
            m_bFront = bFront;
            //
            m_surfaceOrientation = surfaceOrientation;

            return;
        }

        /**
         * Thread entry point.
         */
        public override void Run()
        {
            Looper.Prepare();

            // We need to create the Handler before reporting ready.
            mHandler = new camera_RenderHandler(this);
#if false
                //synchronized(mStartLock) 
                {
            mReady = true;
            //mStartLock.notify();    // signal waitUntilReady()
        }
#endif

            // Prepare EGL and open the camera before we start handling messages.
            EGLContext eglContext = null;
            eglContext = this.mEglContext_shared;
            try
            {
                mEglCore = new EglCore(eglContext, 0);
            }
            catch (System.Exception e)
            {
                qyFuncs.showInfo_open(0, "", "camera_Render.Run failed, new EglCore exception");
                goto errLabel;
            }
            ///
            int reqW = REQ_CAMERA_WIDTH;
            int reqH = REQ_CAMERA_HEIGHT;
            int reqFps = REQ_CAMERA_FPS;

            //
            reqW = 1080;
            reqH = 1920;
            //

            openCamera(reqW, reqH, reqFps, m_bFront);

            //



            //
            Looper.Loop();

        //
        errLabel:

            //
            Log.Debug(TAG, "looper quit");
            if (this.tmpFrameBuffer_0.bDone_createEnv)
            {
                this.tmpFrameBuffer_0.deleteEnv();
                this.tmpFrameBuffer_0.bDone_createEnv = false;
            }
            if (this.tmpFrameBuffer_o.bDone_createEnv)
            {
                this.tmpFrameBuffer_o.deleteEnv();
                this.tmpFrameBuffer_o.bDone_createEnv = false;
            }
            //
            if (this.tmpFrameBuffer_rotate.bDone_createEnv)
            {
                this.tmpFrameBuffer_rotate.deleteEnv();
                this.tmpFrameBuffer_rotate.bDone_createEnv = false;
            }

            //
            if (this.tmpFrameBuffer_int.bDone_createEnv)
            {
                this.tmpFrameBuffer_int.deleteEnv();
                this.tmpFrameBuffer_int.bDone_createEnv = false;
            }

            //
            if (this.m_simpleTex_0_int != null)
            {
                this.m_simpleTex_0_int.destroy(); this.m_simpleTex_0_int = null;
            }
            if (this.m_simpleTex_rotate != null)
            {
                this.m_simpleTex_rotate.destroy(); this.m_simpleTex_rotate = null;
            }
            if (this.m_simpleTex_o != null)
            {
                this.m_simpleTex_o.destroy(); this.m_simpleTex_o = null;
            }

            //
            releaseCamera();
            releaseGl();
            if (mEglCore != null)
            {
                mEglCore.Release();
            }

            //synchronized(mStartLock) 
            {
                mReady = false;
            }

            //
            mThreadEnd = true;

            //
            qyFuncs.showInfo_open(0, null, "renderThread.run ends");

        }

        //
        public int notifyCameraOpen()
        {

            //
            if (!this.m_bUse_camera1)
            {
                mCameraPreviewWidth = myCamera.mVideoSize.Width;
                mCameraPreviewHeight = myCamera.mVideoSize.Height;
                int thousandFps = 30000;
                //
                sendCameraParams(mMainHandler, mCameraPreviewWidth, mCameraPreviewHeight,
                        thousandFps / 1000.0f);
            }

            //
            mReady = true;

            //
            return 0;
        }

        /**
         * Waits until the render thread is ready to receive messages.
         * <p>
         * Call from the UI thread.
         */
        public int waitUntilReady()
        {
            //synchronized(mStartLock) 
            {
                while (!mReady)
                {
                    //
                    if (mThreadEnd) return -1;

                    //
                    try
                    {
                        //mStartLock.wait();
                        qyFuncs.mySleep(1);
                    }
                    catch (InterruptedException ie) { /* not expected */ }
                }
            }

            return 0;
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
        public camera_RenderHandler getHandler()
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

            //
            mCameraTexture.SetDefaultBufferSize(this.mCameraPreviewWidth, this.mCameraPreviewHeight);

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
            //this.setRotate(25);
        }

        /**
         * Releases most of the GL resources we currently hold (anything allocated by
         * surfaceAvailable()).
         * <p>
         * Does not release EglCore.
         */
        private void releaseGl()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;

            if (!pQyMc.b_isHarmony)
            {
                GlUtil.CheckGlError("releaseGl start");
            }

            //
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
            if (!pQyMc.b_isHarmony)
            {
                GlUtil.CheckGlError("releaseGl done");
            }

            //
            if (mEglCore != null)
            {
                mEglCore.MakeNothingCurrent();
            }
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
            //
            Opengl.Matrix.OrthoM(mDisplayProjectionMatrix_invert, 0, 0, width, height, 0, -1, 1);


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

            //
            newWidth = width;
            newHeight = height;


            //
            mRect.SetScale(newWidth, newHeight);
            mRect.SetPosition(mPosX, mPosY);
            mRect.Rotation = (rotAngle);
            mRectDrawable.setScale(zoomFactor);

            sendRectSize(mMainHandler, newWidth, newHeight);
            sendZoomArea(mMainHandler, Java.Lang.Math.Round(mCameraPreviewWidth * zoomFactor),
                    Java.Lang.Math.Round(mCameraPreviewHeight * zoomFactor));
            sendRotateDeg(mMainHandler, rotAngle);
        }

        int cnt;

        // SurfaceTexture.OnFrameAvailableListener; runs on arbitrary thread
        public void OnFrameAvailable(SurfaceTexture surfaceTexture)
        {
            //
            bool bDbg = false;
            string str;

            //
            //
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
            int iErr = -1;

            bool bDbg = false;
            string str;
            bool bDumpTex = false;

#if DEBUG
            //bDumpTex = true;

            //
            qyFuncs.traceLog("renderThread.frameAvail enters");

#endif

            //
            mCameraTexture.UpdateTexImage();

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, this.miIndex_sharedObj);
            if (pSharedObj == null) goto errLabel;


            //
            int w = this.mWindowSurfaceWidth;// mCameraPreviewWidth;
            int h = this.mWindowSurfaceHeight;// mCameraPreviewHeight;

            //
            //qyFuncs_Droid.myGLClearError();
            bool bRtt = true;

            //
            if (!this.tmpFrameBuffer_0.bDone_createEnv)
            {
                this.tmpFrameBuffer_0.createEnv(w, h, "camera 0");
                this.tmpFrameBuffer_0.bDone_createEnv = true;
            }

            //
            using (CQySyncObj syncObj = new CQySyncObj())
            {

                //
                if (bRtt)
                {
                    this.tmpFrameBuffer_0.bindFrameBufferInfo();
                    //
                    GLES20.GlViewport(0, 0, w, h);

                }

                //      
                draw(bRtt);

                //
                if (bRtt)
                {
                    this.tmpFrameBuffer_0.unbindFrameBufferInfo();
                }

                //

                //
                if (bDumpTex)
                {
                    Bitmap bmp = qyFuncs_Droid.texture2Bmp(this.tmpFrameBuffer_0.mTexture[0], w, h);
                    GuiShare.pf_dumpBmp(bmp, "tmpTex_0");
                    bmp.Recycle();
                }

                //
                if (bRtt)
                {
                    int wh = Java.Lang.Math.Max(w, h);

                    if (!this.tmpFrameBuffer_int.bDone_createEnv)
                    {
                        this.tmpFrameBuffer_int.createEnv(wh, wh, "camera.int");
                        this.tmpFrameBuffer_int.bDone_createEnv = true;
                    }

                    this.tmpFrameBuffer_int.bindFrameBufferInfo();
                    GLES20.GlViewport(0, 0, wh, wh);

                    //
                    Context context = qyMessengerApp.Droid.MyApplication.Context;
                    int texId = tmpFrameBuffer_0.mTexture[0];

                    //
                    //
                    if (this.m_simpleTex_0_int == null)
                    {
                        this.m_simpleTex_0_int = new SimpleTexture(context, texId, false, "camera0->int");
                        //
                        this.m_simpleTex_0_int.resize(wh, wh, 0);
                    }
                    //
                    this.m_simpleTex_0_int.draw(0);


                    //
                    this.tmpFrameBuffer_int.unbindFrameBufferInfo();

                    //
                    if (bDumpTex)
                    {
                        Bitmap bmp = qyFuncs_Droid.texture2Bmp(this.tmpFrameBuffer_int.mTexture[0], wh, wh);
                        GuiShare.pf_dumpBmp(bmp, "tmpTex_int");
                        bmp.Recycle();
                    }

                    //////////////////////////////
                    //
                    if (!this.tmpFrameBuffer_rotate.bDone_createEnv)
                    {
                        this.tmpFrameBuffer_rotate.createEnv(wh, wh, "camera.rotate");
                        this.tmpFrameBuffer_rotate.bDone_createEnv = true;
                    }

                    this.tmpFrameBuffer_rotate.bindFrameBufferInfo();
                    GLES20.GlViewport(0, 0, wh, wh);

                    //
                    texId = tmpFrameBuffer_int.mTexture[0];

                    //
                    int tex_o_w = 0;
                    int tex_o_h = 0;
                    //
                    {
                        if (this.m_simpleTex_rotate == null)
                        {
                            this.m_simpleTex_rotate = new SimpleTexture(context, texId, false, "camera.rotate");
                        }
                        //
                        int angle_z = 90;
                        //
                        int ii = this.myCamera.mSensorOrientation;
                        //
                        qyFuncs_Droid.get_tex_o_info(myCamera.mSensorOrientation, m_surfaceOrientation, mCameraPreviewWidth, mCameraPreviewHeight, ref angle_z, ref tex_o_w, ref tex_o_h);
                        //
                        this.m_simpleTex_rotate.resize(wh, wh, angle_z);
                        this.m_simpleTex_rotate.draw(angle_z);

#if false
                        if (this.m_simpleTex_rotate != null)
                        {
                            this.m_simpleTex_rotate.destroy(); this.m_simpleTex_rotate = null;
                        }
#endif
                    }

                    //
                    this.tmpFrameBuffer_rotate.unbindFrameBufferInfo();


                    if (bDumpTex)
                    {
                        Bitmap bmp = qyFuncs_Droid.texture2Bmp(this.tmpFrameBuffer_rotate.mTexture[0], wh, wh);
                        GuiShare.pf_dumpBmp(bmp, "tmpTex_rotate");
                        bmp.Recycle();
                    }

                    ////////////////////////////
                    ///

                    //
                    if (0 != syncObj.sync("", pSharedObj.var.hMutex_syncEncFrame))
                    {
                        str = string.Format("camera.frameAvail: sync failed");
                        qyFuncs.showInfo_open(0, null, str);
                        //
                        goto errLabel;
                    }

                    //
                    int o_w = tex_o_w;// mCameraPreviewHeight;
                    int o_h = tex_o_h;// mCameraPreviewWidth;

                    //
                    if (!this.tmpFrameBuffer_o.bDone_createEnv)
                    {
                        this.tmpFrameBuffer_o.createEnv(o_w, o_h, "camera.o");
                        this.tmpFrameBuffer_o.bDone_createEnv = true;
                    }

                    this.tmpFrameBuffer_o.bindFrameBufferInfo();
                    GLES20.GlViewport(0, 0, o_w, o_h);

                    //
                    texId = tmpFrameBuffer_rotate.mTexture[0];

                    //
                    //

                    if (this.m_simpleTex_o == null)
                    {
                        this.m_simpleTex_o = new SimpleTexture(context, texId, false, "camera->o");
                    }
                    //
                    this.m_simpleTex_o.resize(o_w, o_h, 0);
                    this.m_simpleTex_o.draw(0);

#if false
                        if (this.m_simpleTex_o != null)
                        {
                            this.m_simpleTex_o.destroy(); this.m_simpleTex_o = null;
                        }
#endif


                    //
                    this.tmpFrameBuffer_o.unbindFrameBufferInfo();


                    if (bDumpTex)
                    {
                        Bitmap bmp = qyFuncs_Droid.texture2Bmp(this.tmpFrameBuffer_o.mTexture[0], o_w, o_h);
                        GuiShare.pf_dumpBmp(bmp, "tmpTex_o");
                        bmp.Recycle();
                    }







                    //
                    //
                    if (pSharedObj.var.m_o_texId != this.tmpFrameBuffer_o.mTexture[0])
                    {
                        pSharedObj.var.m_o_texId = this.tmpFrameBuffer_o.mTexture[0];
                    }
                    if (pSharedObj.var.m_o_iW != tex_o_w)
                    {
                        pSharedObj.var.m_o_iW = tex_o_w;
                    }
                    if (pSharedObj.var.m_o_iH != tex_o_h)
                    {
                        pSharedObj.var.m_o_iH = tex_o_h;
                    }


                    //
                }


                //                
#if DEBUG
#if false
                camera_RenderThread.m_test_textureId = this.tmpFrameBuffer_o.mTexture[0];
                camera_RenderThread.m_iW_tex = mCameraPreviewHeight; ;
                camera_RenderThread.m_iH_tex = mCameraPreviewWidth; ;
#endif


                //
                if (false)
                {
                    str = string.Format("camera.frameAvail. frame ok");
                    qyFuncs.showInfo_open(0, null, str);
                }
#endif


            }

            //
            pSharedObj.var.bFrameAvail = true;
            qyFuncs.mySetEvent(pSharedObj.var.hEvt_notifyFrameAvail);

            //
            //this.tmpFrameBuffer.deleteEnv();

            //
            iErr = 0;

        errLabel:

            //
#if DEBUG
            qyFuncs.traceLog("renderThread.frameAvail leaves");
#endif


            //
            return;

        }

        /**
         * Draws the scene and submits the buffer.
         */
        public void draw(bool bRtt)
        {
            GlUtil.CheckGlError("draw start");

            GLES20.GlClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);
            //
            if (!bRtt)
            {
                mRect.Draw(mTexProgram, mDisplayProjectionMatrix);
            }
            else
            {
                //mRect.Draw(mTexProgram, mDisplayProjectionMatrix_invert);
                mRect.Draw(mTexProgram, mDisplayProjectionMatrix);
            }
            //
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

        private void openCamera(int desiredWidth, int desiredHeight, int desiredFps, bool bFront)
        {
            if (m_bUse_camera1)
            {
                qyFuncs.traceLog("openCamera1(desiredWidth, desir");
                openCamera1(desiredWidth, desiredHeight, desiredFps);
                //
                this.notifyCameraOpen();
            }
            else
            {
                qyFuncs.traceLog("openCamera(desiredWidth, desiredHeight");
                try
                {
                    this.myCamera = new MyCamera(this.mHandler);
                    //
                    this.myCamera.openCamera(desiredWidth, desiredHeight, bFront);
                }
                catch(Java.Lang.Exception e)
                {
                    qyFuncs.showInfo_open(0, "", "camera_RenderThread.openCamera, exception");
                }
            }
        }

        private void releaseCamera()
        {
            if (m_bUse_camera1)
            {
                qyFuncs.traceLog(" releaseCamera1();");
                releaseCamera1();
            }
            else
            {
                if (this.myCamera != null)
                {
                    qyFuncs.traceLog(".closeCamera();");

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
            sendCameraParams(mMainHandler, mCameraPreviewWidth, mCameraPreviewHeight,
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
        public void sendCameraParams(Handler mainHandler, int width, int height, float fps)
        {
            // The right way to do this is to bundle them up into an object.  The lazy
            // way is to send two messages.
            mainHandler.SendMessage(mainHandler.ObtainMessage(MSG_SEND_CAMERA_PARAMS0, width, height));
            mainHandler.SendMessage(mainHandler.ObtainMessage(MSG_SEND_CAMERA_PARAMS1, (int)(fps * 1000), 0));
        }

        /**
 * Sends the updated rect size to the main thread.
 * <p>
 * Call from render thread.
 */
        public void sendRectSize(Handler mainHandler, int width, int height)
        {
            mainHandler.SendMessage(mainHandler.ObtainMessage(MSG_SEND_RECT_SIZE, width, height));
        }

        /**
         * Sends the updated zoom area to the main thread.
         * <p>
         * Call from render thread.
         */
        public void sendZoomArea(Handler mainHandler, int width, int height)
        {
            mainHandler.SendMessage(mainHandler.ObtainMessage(MSG_SEND_ZOOM_AREA, width, height));
        }

        /**
         * Sends the updated zoom area to the main thread.
         * <p>
         * Call from render thread.
         */
        public void sendRotateDeg(Handler mainHandler, int rot)
        {
            mainHandler.SendMessage(mainHandler.ObtainMessage(MSG_SEND_ROTATE_DEG, rot, 0));
        }


        //


        //
        /**
     * Handler for RenderThread.  Used for messages sent from the UI thread to the render thread.
     * <p>
     * The object is created on the render thread, and the various "send" methods are called
     * from the UI thread.
     */
        public class camera_RenderHandler : Handler
        {
            string TAG = "camera_RenderHandler";

            //
            private const int MSG_SURFACE_AVAILABLE = 0;
            private const int MSG_SURFACE_CHANGED = 1;
            private const int MSG_SURFACE_DESTROYED = 2;
            private const int MSG_SHUTDOWN = 3;
            private const int MSG_FRAME_AVAILABLE = 4;
            private const int MSG_ZOOM_VALUE = 5;
            private const int MSG_SIZE_VALUE = 6;
            private const int MSG_ROTATE_VALUE = 7;
            private const int MSG_POSITION = 8;
            private const int MSG_REDRAW = 9;

            // This shouldn't need to be a weak ref, since we'll go away when the Looper quits,
            // but no real harm in it.
            private camera_RenderThread mWeakRenderThread;

            /**
             * Call from render thread.
             */
            public camera_RenderHandler(camera_RenderThread rt)
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
            public void sendSurfaceChanged(int format, int width,
                            int height)
            {
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

                camera_RenderThread renderThread = mWeakRenderThread;
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
                        bool bRtt = true;
                        //
                        renderThread.draw(bRtt);
                        break;

                    case Consts.CONST_msg_cameraOpen:
                        renderThread.notifyCameraOpen();
                        break;

                    default:
                        throw new RuntimeException("unknown message " + what);
                }
            }



        }


        //
    }

}