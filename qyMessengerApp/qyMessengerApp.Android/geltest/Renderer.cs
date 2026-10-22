
using Android.Graphics;
using Java.Lang;
using Thread = Java.Lang.Thread;
using Com.Qycx.Gleslibrary;
using Android.Views;
using qy.Droid;
using qyMessengerApp.Droid;
using qy;
using Android.App;
using System.Threading;
using System;
namespace MyGLE
{
    public class Renderer :Thread, TextureView.ISurfaceTextureListener
    {
        public Activity mOwner;
        public System.Object mLock = new System.Object();
        private SurfaceTexture mSurfaceTexture;
        private EglCore mEglCore;
        private bool mDone;
        private int minFgType;
        System.Threading.EventWaitHandle mhEvt_toNotifyTexAvail;
        public Renderer(Activity owner,int minFgType)
        {
            mOwner = owner;
            this.minFgType = minFgType;
            mhEvt_toNotifyTexAvail = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
        }

        public override void Run()
        {
            while (true)
            {
                SurfaceTexture surfaceTexture = null;
                bool bReady = true;
                lock (mLock)
                {
                    while (!mDone && (surfaceTexture = mSurfaceTexture) == null)
                    {
                        bReady = false;
                        Monitor.Wait(mLock);
                    }
                    if (mDone)
                    {
                        break;
                    }
                }
                if (!bReady)
                {
                    int ms = 100;
                    qyFuncs.myWaitOne(this.mhEvt_toNotifyTexAvail, ms);
                    continue;
                }
                mEglCore = new EglCore(null, EglCore.FlagTryGles3);
                WindowSurface windowSurface = new WindowSurface(mEglCore, mSurfaceTexture);
                windowSurface.MakeCurrent();

                doAnimation(windowSurface);

                windowSurface.Release();
                mEglCore.Release();
                if (!Activity_dlgTalk_av.sReleaseInCallback)
                {
                    surfaceTexture.Release();
                }
            }
        }

        private void doAnimation(WindowSurface eglSurface)
        {
            int BLOCK_WIDTH = 80;
            int BLOCK_SPEED = 2;
            float clearColor = 0.0f;
            int xpos = -BLOCK_WIDTH / 2;
            int xdir = BLOCK_SPEED;
            int width = eglSurface.Width;
            int height = eglSurface.Height;


            //MyDraw myDraw = new MyDraw(mOwner, eglSurface);
            //if (0 != myDraw.Init()) return;
            //if (0 != myDraw.OnSize(width, height)) return;
            Activity_dlgTalk_av activity_DlgTalk_Av = (Activity_dlgTalk_av)mOwner;
            
            activity_DlgTalk_Av.m_fg_dlgTalk_av2.tf_peer = new T1_frm();

            CTalkDraw myDraw = new CTalkDraw(activity_DlgTalk_Av, minFgType, eglSurface);
            if (0 != myDraw.init()) return;
            if (0 != myDraw.onSize(width, height)) return;
            while (true)
            {
                SurfaceTexture surfaceTexture;

                lock (mLock)
                {
                    surfaceTexture = mSurfaceTexture;
                    if (mDone  ||  surfaceTexture == null)
                    {
                        Console.WriteLine("doAnimation exiting");
                        //return;
                        goto errLabel;
                    }
                }
                if (activity_DlgTalk_Av.m_bSurfaceTextureSizeChanged                                      
                    || activity_DlgTalk_Av.m_bNeedDoTask_av_accept)
                {
                    //
                    activity_DlgTalk_Av.m_bSurfaceTextureSizeChanged = false;
                    //
                    activity_DlgTalk_Av.m_bNeedDoTask_av_accept = false;

                    //
                    activity_DlgTalk_Av.m_bDlgTalk_av_readyFor_talkDrawRestart = false;
                    activity_DlgTalk_Av.m_bTalkDraw_needRestart = true;
                    //
                    if (minFgType == Consts.CONST_iFgType_dlgTalk_av2)
                    {
                        Fragment_dlgTalk_av2 fg = activity_DlgTalk_Av.m_fg_dlgTalk_av2;
                        fg.mMainHandler.send_MSG_talkDraw_needRestart();
                    }
                }
                if (activity_DlgTalk_Av.m_bTalkDraw_needRestart)
                {
                    if (activity_DlgTalk_Av.m_bDlgTalk_av_readyFor_talkDrawRestart)
                    {

                        //
                        activity_DlgTalk_Av.m_bDlgTalk_av_readyFor_talkDrawRestart = false;
                        activity_DlgTalk_Av.m_bTalkDraw_needRestart = false;

                        goto errLabel;
                    }
                }

                myDraw.draw();
              // myDraw.Draw();

                eglSurface.SwapBuffers();

                clearColor += 0.015625f;
                if (clearColor > 1.0f)
                {
                    clearColor = 0.0f;
                }

                xpos += xdir;
                if (xpos <= -BLOCK_WIDTH / 2 || xpos >= width - BLOCK_WIDTH / 2)
                {
                    Console.WriteLine("change direction");
                    xdir = -xdir;
                }
            }


            errLabel:
            if (myDraw != null)
            {
                myDraw.exit();
            }

            //
            return;

        }

        public void Halt()
        {
            lock (mLock)
            {
                mDone = true;
                Monitor.Pulse(mLock);
            }
        }
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            lock (mLock)
            {
                mSurfaceTexture = surface;
                Monitor.Pulse(mLock);
            }
            qyFuncs.mySetEvent(this.mhEvt_toNotifyTexAvail);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            lock (mLock)
            {
                mSurfaceTexture = null;
                Monitor.Pulse(mLock);
            }
            return Fragment_dlgTalk_av2.sReleaseInCallback;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {

            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)mOwner;
            //
            dlgTalk_av.m_bSurfaceTextureSizeChanged = true;
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

    }
}
