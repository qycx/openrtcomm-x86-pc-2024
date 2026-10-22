using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using qy;
using qy.Droid;
using Thread = Java.Lang.Thread;
//
using Com.Qycx.Gleslibrary;
using Android.Opengl;
using Java.Nio;
using Android.Graphics;


//
namespace qyMessengerApp.Droid
{
    public class dlgTalk_av_Renderer : Thread, TextureView.ISurfaceTextureListener
    {
        //
        //
        Fragment mOwner;
        int miFgType;

        //public Object mLock = new Java.Lang.Object();
        public System.Threading.Mutex hMutex = new System.Threading.Mutex();
        private SurfaceTexture mSurfaceTexture;
        private EglCore mEglCore;
        private bool mDone;

        //
        System.Threading.EventWaitHandle mhEvt_toNotifyTexAvail;

        //
        public dlgTalk_av_Renderer(Fragment fg,int iFgType) : base("Renderer") 
        {
            mOwner = fg;
            miFgType = iFgType;
            //
            return;
        }

        public unsafe int init()
        {
            int iErr = -1;
            int tt = 0;
            string pName = "notifiTexAvail" + qyFuncs.getuiNextTranNo(null, 0, ref tt);
            //mhEvt_toNotifyTexAvail=new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, new string(pName));
            mhEvt_toNotifyTexAvail = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);


            iErr = 0;
            return iErr;
        }

        public int exit()
        {
            if (null!=this.mhEvt_toNotifyTexAvail )
            {
                this.mhEvt_toNotifyTexAvail.Close();
                this.mhEvt_toNotifyTexAvail = null;
            }

            return 0;
        }



        public override void Run()
        {
            while (true)
            {
                SurfaceTexture surfaceTexture = null;

                //synchronized(mLock)
                bool bReady = true;
                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (syncObj.sync("", this.hMutex) != 0)
                    {
                        goto errLabel;
                    }

                    //                        
                    if (!mDone && (surfaceTexture = mSurfaceTexture) == null)
                    {
                        bReady = false;
                    }
                    if (mDone) break;
                }

                if (!bReady)
                {
                    int ms = 100;
                    qyFuncs.myWaitOne(this.mhEvt_toNotifyTexAvail, ms);
                    continue;
                }

                //
                EGLContext eglContext_shared = null;
                //
                mEglCore = new EglCore(eglContext_shared, EglCore.FlagTryGles3);
                WindowSurface windowSurface = new WindowSurface(mEglCore, surfaceTexture);
                windowSurface.MakeCurrent();

                //
               doAnimation(windowSurface);

                //
                windowSurface.Release();
                mEglCore.Release();
                
                //
                if (!Fragment_dlgTalk_av2.sReleaseInCallback)
                {
                    //Log.i(TAG, "Releasing SurfaceTexture in renderer thread");
                    surfaceTexture.Release();
                }

            }

        errLabel:
            return;
        }

        //
        
  


        //
        private void doAnimation(WindowSurface eglSurface)
        {
            int BLOCK_WIDTH = 80;
            int BLOCK_SPEED = 2;
            float clearColor = 0.0f;
            int xpos = -BLOCK_WIDTH / 2;
            int xdir = BLOCK_SPEED;
            int width = eglSurface.Width;
            int height = eglSurface.Height;

            //
            string str;

            //
           CTalkDraw myDraw = new CTalkDraw((Activity_dlgTalk_av)mOwner.Activity,0,eglSurface);

            //
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)mOwner.Activity;

            ////
            if (0 != myDraw.init()) goto errLabel;
            if (0!= myDraw.onSize(width, height)) goto errLabel;

            //
#if DEBUG
            qyFuncs.traceLog("talk_av_renderer.doAnimation.163 enters");

            //
            
#endif

            //
            while (true)
            {
                //
                qyFuncs.traceLog("renderer.doAnimation.173");

                //
                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (syncObj.sync("", this.hMutex) != 0)
                    {
                        goto errLabel;
                    }
                    SurfaceTexture surfaceTexture = mSurfaceTexture;
                    if (mDone || surfaceTexture == null)
                    {
                        goto errLabel;
                    }
                }

                //
                qyFuncs.traceLog("renderer.doAnimation.190");


                //
                if (dlgTalk_av.m_bSurfaceTextureSizeChanged)
                {
                    //
                    dlgTalk_av.m_bSurfaceTextureSizeChanged = false;

                    //
                    str = string.Format("doAnimation: bSurfaceTextureSizeChanged is true,so needRestart");
                    qyFuncs.showInfo_open(0, null, str);

                    //
                    dlgTalk_av.m_bDlgTalk_av_readyFor_talkDrawRestart = false;
                    dlgTalk_av.m_bTalkDraw_needRestart = true;
                    //
                    if (miFgType == Consts.CONST_iFgType_dlgTalk_av2)
                    {
                        Fragment_dlgTalk_av2 fg = (Fragment_dlgTalk_av2)mOwner;
                        fg.mMainHandler.send_MSG_talkDraw_needRestart();
                    }
                }
                if ( dlgTalk_av.m_bTalkDraw_needRestart)
                {
                    if (dlgTalk_av.m_bDlgTalk_av_readyFor_talkDrawRestart)
                    {
                        str = string.Format("doAnimation will stop: m_bDlgTalk_av_readyFor_talkDrawRestart is true");
                        qyFuncs.showInfo_open(0, null, str);
                        //
                        dlgTalk_av.m_bDlgTalk_av_readyFor_talkDrawRestart = false;
                        dlgTalk_av.m_bTalkDraw_needRestart = false;
                        //
                        goto errLabel;

                    }
                }

                //
                //string str = string.Format("clearColor {0}", clearColor);
                //qyFuncs.traceLog(str);

                //
                //GLES20.GlClearColor(clearColor, clearColor, clearColor, 1.0f);
                //GLES20.GlClear(GLES20.GlColorBufferBit);

                /*
                GLES20.GlEnable(GLES20.GlScissorTest);
                GLES20.GlScissor(xpos, height / 4, BLOCK_WIDTH, height / 2);
                GLES20.GlClearColor(1.0f, 0.0f, 0.0f, 1.0f);
                GLES20.GlClear(GLES20.GlColorBufferBit);
                GLES20.GlDisable(GLES20.GlScissorTest);
                */

                //
                qyFuncs.traceLog("render.doAnimation, before myDraw.draw");

                //
                // for test
                if (false)
                {
                    qyFuncs.mySleep(10);
                    continue;
                }
                

                //
                myDraw.draw();

                //
                qyFuncs.traceLog("renderer.doAnimation.after myDraw.draw");

                //
                eglSurface.SwapBuffers();

                //


                /*
                //
                clearColor += 0.015625f;
                if (clearColor > 1.0f)
                {
                    clearColor = 0.0f;

                }
                xpos += xdir;
                if (xpos <= -BLOCK_WIDTH / 2 || xpos >= width - BLOCK_WIDTH / 2)
                {
                    xdir = -xdir;
                }
                */

                //
                //
                qyFuncs.traceLog("renderer.doAnimation.264");


                //
                continue;
            }

        errLabel:

            if ( myDraw!=null)
            {
                myDraw.exit();
            }

            //
#if DEBUG
            qyFuncs.traceLog("talk_av_renderer.doAnimation.261, leaves");
#endif


            //
            return;
        }

        //
        public void halt()
        {
            //
            qyFuncs.showInfo_open(0, null, "dlgTalk_av_renderer.halt");

            //
            using (CQySyncObj syncObj = new CQySyncObj())
            {
                if (syncObj.sync("", this.hMutex) != 0) goto errLabel;
                //
                mDone = true;
            }

        errLabel:

            //
            //qyFuncs.showInfo_open(0, null, "dlgTalk_av_renderer.halt leaves");

            return;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            //throw new NotImplementedException();
            using (CQySyncObj syncObj = new CQySyncObj())
            {
                if (syncObj.sync("", this.hMutex) != 0)
                {
                    goto errLabel;
                }
                mSurfaceTexture = surface;
            }
            qyFuncs.mySetEvent(this.mhEvt_toNotifyTexAvail);

            //
        errLabel:
            return;
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            //throw new NotImplementedException();
            using (CQySyncObj syncObj = new CQySyncObj())
            {
                if (syncObj.sync("", this.hMutex) != 0) goto errLabel;
                mSurfaceTexture = null;
            }

            //
            if (Fragment_dlgTalk_av2.sReleaseInCallback)
            {
                //Log.i(TAG, "Allowing TextureView to release SurfaceTexture");
            }


        errLabel:
            return Fragment_dlgTalk_av2.sReleaseInCallback;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            //throw new NotImplementedException();
            int ii = 0;

            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)this.mOwner.Activity;
            //
            dlgTalk_av.m_bSurfaceTextureSizeChanged = true;

            return;
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            //throw new NotImplementedException();
        }
    }

}