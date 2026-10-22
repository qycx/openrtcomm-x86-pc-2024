
//

//
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using qyMessengerApp.Droid;
using Res = Android.Content.Res;
using qy;
using qy.Droid;
using Android.Opengl;
using Google.Android.Material.BottomSheet;
using Android.Util;
using Android.Content.PM;
using Android.Media;
using AndroidX.Annotations;
using Android.Content.Res;
using Android.Animation;
using qyMessengerApp.Android;
using AndroidX.ConstraintLayout.Widget;

using static Android.Service.Voice.VoiceInteractionSession;
using static AndroidX.RecyclerView.Widget.RecyclerView;
using System.Diagnostics;



//
namespace qyMessengerApp.Droid
{
    [Activity(Label = "Activity_dlgTalk_av", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Keyboard)]
    public class Activity_dlgTalk_av : BaseActivity, PopupWindow.IOnDismissListener

    {
        //
        public Activity_dlgTalk mOwner_hTalk;

        public bool showBackSheetDialog = false;
        //
#if DEBUG
        public Fragment_test m_fg_test;
        public Fragment_test2 m_fg_test2;
#endif
        //
        public Fragment_dlgTalk_av2 m_fg_dlgTalk_av2;

        //
        public bool m_bGot_closeWndMsg;

        //
        BottomSheetDialog m_bsd_askToStopAv;

        //
        public int m_iCmd;
        public int m_index_taskInfo;

        //
        public uint uiLastTickCnt_touch;

        //
        public MyHandler mHandler;
        public Handler bottomBarHandler = new Handler(Looper.MainLooper);
        public Action runnableHideBottomBar;
        public Action runnableShowBottomBar;

        //
        public int tn_dlg;

        //
        public bool m_bSurfaceTextureSizeChanged;
        public bool menuInScreen = true;
        public bool firstStartMenuInScreen = true;
        //
        public bool m_bTalkDraw_needRestart;
        public bool m_bDlgTalk_av_readyFor_talkDrawRestart;

        //
        public int m_nErrs_hTalk_av_not_matched;
        public static volatile bool sReleaseInCallback = true;

        //
        //
        public bool m_bNeedDoTask_av_accept;


        //
        public class MyHandler : Handler
        {
            Activity_dlgTalk_av mOwner;

            //
            public MyHandler(Activity_dlgTalk_av owner)
            {
                mOwner = owner;
            }


            public override void HandleMessage(Message msg)
            {
                //base.HandleMessage(msg);

                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


                switch (msg.What)
                {
                    case Consts.CONST_qyWm_postComm:

                        //
                        switch (msg.Arg1)
                        {
                            case Consts.CONST_qyWmParam_closeWnd:
                                qyFuncs.traceLog("收到退出消息");
                                //
                                mOwner.m_bGot_closeWndMsg = true;

                                mOwner.m_fg_dlgTalk_av2.do_request(false);
                                qyFuncs.traceLog("发送取消发言");
                                //
                                if (mOwner.mOwner_hTalk.m_var.av.bAutoCancel_lostVideo)
                                {
                                    mOwner.mOwner_hTalk.m_var.av.bDoneOnce_autoCancel_lostVideo = true;
                                }
                                //
                                if (mOwner.mOwner_hTalk.m_var.av.taskInfo.bTaskExists)
                                {
                                    //
                                    this.mOwner.pauseTaskAv("dlgTalk_av.handler.HandleMessage, get a msg closeWnd");
                                    //
                                }
                                qyFuncs.traceLog("发送Activity_dlgTalk_av  clean 消息");

                                GuiShare.pf_PostMessage_new(mOwner, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_doClean, 0);
                                break;
                            case Consts.CONST_qyWmParam_doClean:
                                {
                                    if (this.mOwner.mOwner_hTalk.m_var.av.taskInfo.bTaskExists)
                                    {

                                        //qyFuncs_Droid.dlgTalk_cancelTasks(mOwner_hTalk, mOwner_hTalk.m_var, mOwner_hTalk.m_var.av.taskInfo.iTaskId, true);
                                        ushort usOp = 0;
                                        if (this.mOwner.mOwner_hTalk.m_var.av.taskInfo.ucbStarter) usOp = Consts.CONST_imOp_send_cancel;
                                        else usOp = Consts.CONST_imOp_recv_cancel;
                                        //
                                        PARAM_dlgTalk_procTask param = new PARAM_dlgTalk_procTask();
                                        if (this.mOwner.mOwner_hTalk.m_var.av.bAutoCancel_lostVideo)
                                        {
                                            param.bAutoCancel = true;
                                        }
                                        qyFuncs.traceLog("调用 dlgTalk_procTask1 开始");
                                        qyFuncs_Droid.dlgTalk_procTask1(this.mOwner.mOwner_hTalk, this.mOwner.mOwner_hTalk.m_var.av.taskInfo.iTaskId, usOp, "", ref param);
                                        qyFuncs.traceLog("调用 dlgTalk_procTask1 结束");
                                    }

                                    qyFuncs.traceLog("收到Activity_dlgTalk_av  doFinish 消息");
                                    this.mOwner.doFinish("BtnYes_Click");

                                }
                                break;
                            case Consts.CONST_qyWmParam_doTask_av_accept:
                                {
                                    int iTaskId = msg.Arg2;
                                    //
                                    if (mOwner.m_index_taskInfo > 0)
                                    {
                                        qyFuncs.showInfo_open(0, "", "wmParam_doTask_av_accept skipped, dlgTalk_av.m_index_taskInfo >0");
                                        break;
                                    }
                                    int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
                                    if (index_taskInfo <= 0)
                                    {
                                        qyFuncs.showInfo_open(0, "", "dlgTalk_av.wmParam_doTask_av_accept failed, index_taskInfo <=0");
                                        break;
                                    }
                                    //
                                    qyFuncs.showInfo_open(0, "", "dlgTalk_av.wmParam_doTask_av_accept");
                                    //
                                    mOwner.m_index_taskInfo = index_taskInfo;
                                    mOwner.m_iCmd = tmpResource.IDC_BUTTON_av_accept;
                                    mOwner.mOwner_hTalk.m_var.av.bNeedDoTaskAv = true;
                                    //
                                    mOwner.m_bNeedDoTask_av_accept = true;
                                }
                                break;

                            default:
                                break;
                        }
                        break;
                    default:
                        break;

                }

                return;
            }

            //
        }

      
            //
            public int gui_onTimer1(object p0, object p1, object p2)
        {
            string str;

            //
#if DEBUG
            if (false)
            {
                str = "dlgTalk_av: gui_onTimer1";
                qyFuncs.showInfo_open(0, "", str);
                return 0;
            }
#endif


            if (m_fg_dlgTalk_av2 != null)
            {
                m_fg_dlgTalk_av2.mgui_onTimer(p0, p1, p2);
            }

            //
#if DEBUG
            //
#if false
            int i;
            int cnt = 0;
            for ( i=0;i< mOwner_hTalk.m_var.tmp_hTools_dlgTalk_av.GetLength(0); i++)
            {
                if (mOwner_hTalk.m_var.tmp_hTools_dlgTalk_av[i] != null)
                {
                    cnt++;
                }
            }
            mOwner_hTalk.m_var.cnt_tmpTools = cnt;
            str = string.Format("tmpTools {0}", cnt);
            qyFuncs.showInfo_open(0, null, str);
#endif

#endif



            //
            return 0;
        }

        // 当用户进行任何操作时，重置计时器

        private Activity_dlgTalk_av mActivity;

        protected override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            qyFuncs.traceLog("Activity_dlgTalk_av  OnCreate");

            int iErr = -1;
            //
            string str;
            mActivity = this;
            //
            mTag = "dlgTalk_av";

            //
            this.mHandler = new MyHandler(this);

            //
            int tt = 0;
            this.tn_dlg = (int)qyFuncs.getuiNextTranNo(null, 0, ref tt);

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            //
            Intent intent = this.Intent;
            int iCmd = intent.GetIntExtra("iCmd", 0);
            UInt64 ui64Id = (UInt64)intent.GetLongExtra("ui64Id", 0);
            int index_task = intent.GetIntExtra("index_qmcTaskInfo", 0);
            //
            Console.WriteLine("ui64Id {0}", ui64Id);
            QY_MESSENGER_ID idInfo; idInfo.ui64Id = ui64Id;
            object hTalk = null;
            if (0 != GuiShare.pf_findTalker(pQyMc, pMisCnt, idInfo, ref hTalk))
            {
                goto errLabel;
            }
            mOwner_hTalk = (Activity_dlgTalk)hTalk;
            mOwner_hTalk.m_hTool_dlgTalk_av = this;
            DLG_TALK_var pMgrVar = mOwner_hTalk.m_var;
            //
            if (0 != addTo_tmpTools(mOwner_hTalk, pMgrVar, "dlgTalk_av.OnCreate"))
            {
                goto errLabel;
            }


            pQyMc.fake_bFolder = false;
            //
            if (pQyMc.iManufacturer == Consts.manufacturer_huaWei
                && pQyMc.iModel == Consts.model_huaWei_TAHAN00m)
            {
                DisplayMetrics dm = new DisplayMetrics();

                this.WindowManager.DefaultDisplay.GetMetrics(dm);
                //
                if (0 != dm.WidthPixels && 0 != dm.HeightPixels)
                {
                    float fAspect = 0.0f;
                    if (dm.WidthPixels > dm.HeightPixels)
                    {
                        fAspect = dm.WidthPixels / (dm.HeightPixels * 1.0f);
                    }
                    else
                    {
                        fAspect = dm.HeightPixels / (dm.WidthPixels * 1.0f);
                    }
                    //
                    int ii = 0;
                    if (fAspect > 1.6)
                    {
                        pQyMc.fake_bFolder = true;
                    }
                }

            }



            //
            m_iCmd = iCmd;
            m_index_taskInfo = index_task;


            // Create your application here
            SetContentView(Resource.Layout.layout_activity_dlgTalk_av);


            //

            m_fg_dlgTalk_av2 = Fragment_dlgTalk_av2.NewInstance();
            runnableHideBottomBar = new Action(() =>
              {
                  if (null != m_fg_dlgTalk_av2 && menuInScreen)
                  {
                      m_fg_dlgTalk_av2.more_tv.Activated = false;
                      m_fg_dlgTalk_av2.more_tv.Text = "更多";
                      m_fg_dlgTalk_av2.more_info_menu.Visibility = ViewStates.Invisible;
                      animatorHideMenu();
                      m_fg_dlgTalk_av2.showOperationRelativeLayout(false);
                      m_fg_dlgTalk_av2.nvr_controlSetNormal();
                  }

              });
            runnableShowBottomBar = new Action(() =>
              {
                  if (null != m_fg_dlgTalk_av2 && !menuInScreen)
                  {
                      menuInScreen = true;
                      if (firstStartMenuInScreen)
                      {
                          animatorShowMenu(0);
                      }
                      else
                      {
                          animatorShowMenu(500);
                      }
                      firstStartMenuInScreen = false;

                  }

              });
            if (Resources.Configuration.Orientation == Res.Orientation.Landscape)
            {
                Window.AddFlags(WindowManagerFlags.Fullscreen);
                Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            }
            //
#if DEBUG
#if false
            m_fg_test = new Fragment_test();
            m_fg_test2 = new Fragment_test2();
#endif
#endif
            //

            //
            //
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //

            //.Add(Resource.Id.container,m_fg_test)
            //.Add(Resource.Id.container, m_fg_test2)
            if (m_fg_dlgTalk_av2 != null)
            {
                ft.Replace(Resource.Id.container, m_fg_dlgTalk_av2);
            }
            //              
            ft.Commit();



            //
            this.m_tn_dbg = 999;
            //
            gui_onTimer1(null, null, null);
            //setTimer(1000, gui_onTimer1);


            //  clear
            m_nErrs_hTalk_av_not_matched = 0;


            //
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);


            //
            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                doFinish("dlgTalk_av.OnCreate failed");
            }

        }

        private void animatorHideMenu()
        {
            if (m_fg_dlgTalk_av2.popupWindow != null)
            {
                m_fg_dlgTalk_av2.popupWindow.Dismiss();
            }
            qyFuncs.traceLog("bottomMenu隐藏");
            menuInScreen = false;
            ObjectAnimator animatorTopMenu = ObjectAnimator.OfFloat(m_fg_dlgTalk_av2.topMenu, "translationY", m_fg_dlgTalk_av2.topMenu.Height, 0);
            animatorTopMenu.SetDuration(500);    // 动画持续时间，单位是毫秒
            animatorTopMenu.Start();

            ObjectAnimator animatorBottomMenu = ObjectAnimator.OfFloat(m_fg_dlgTalk_av2.bottomMenu, "translationY", -m_fg_dlgTalk_av2.bottomMenu.Height, 0);
            animatorBottomMenu.SetDuration(500);    // 动画持续时间，单位是毫秒
            animatorBottomMenu.Start();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Res.Orientation.Landscape)
            {
                Window.AddFlags(WindowManagerFlags.Fullscreen);
                Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            }
            else if (newConfig.Orientation == Res.Orientation.Portrait)
            {
                Window.AddFlags(WindowManagerFlags.ForceNotFullscreen);
                Window.ClearFlags(WindowManagerFlags.Fullscreen);
            }
            //
            //m_fg_dlgTalk_av2.trigKeyFrame();
        }

        protected override void OnResume()
        {
            base.OnResume();
            qyFuncs.traceLog("Activity_dlgTalk_av  OnResume");

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            pProcInfo.av.m_bInSleepMode = false;

            //
            DLG_TALK_var m_var = mOwner_hTalk.m_var;

            //
            if (m_var.m_bNeedResumeTaskAv)
            {
                resumeTaskAv("OnResume");
            }

            //
        }
        public override bool DispatchTouchEvent(MotionEvent e)

        {
            bottomBarHandler.RemoveCallbacks(runnableHideBottomBar);
            if (e.Action == MotionEventActions.Up)
            {
                qyFuncs.traceLog("DispatchTouchEvent");
                bottomBarHandler.RemoveCallbacks(runnableHideBottomBar);
                bottomBarHandler.PostDelayed(runnableHideBottomBar, 8000);
                if (null != m_fg_dlgTalk_av2 && !menuInScreen)
                {
                    qyFuncs.traceLog("bottomMenu显示");

                    animatorShowMenu(500);
                    return true;
                }

            }



            uiLastTickCnt_touch = qyFuncs.myGetTickCount(null);

            return base.DispatchTouchEvent(e);

        }

        public void animatorShowMenu(int animiatorDuration)
        {
            qyFuncs.traceLog("bottomMenu显示");
            menuInScreen = true;
            ObjectAnimator animatorbottomMenu = ObjectAnimator.OfFloat(m_fg_dlgTalk_av2.bottomMenu, "translationY", 0, -m_fg_dlgTalk_av2.bottomMenu.Height);
            animatorbottomMenu.SetDuration(animiatorDuration);    // 动画持续时间，单位是毫秒
            animatorbottomMenu.Start();

            ObjectAnimator animatorTopMenu = ObjectAnimator.OfFloat(m_fg_dlgTalk_av2.topMenu, "translationY", 0, m_fg_dlgTalk_av2.topMenu.Height);
            animatorTopMenu.SetDuration(animiatorDuration);    // 动画持续时间，单位是毫秒
            animatorTopMenu.Start();
        }

        //
        int doClean(string str)
        {

            int ii = 0;
            //
            Window.ClearFlags(WindowManagerFlags.KeepScreenOn);

            //
            killTimer("Act_dlgTalk_av.doClean");

            //

            //
#if DEBUG
            if (m_fg_test != null)
            {
                m_fg_test.doClean_ifActivityIsFinishing();
                //
            }
            //
            if (m_fg_test2 != null)
            {
                m_fg_test2.doClean_ifActivityIsFinishing();
            }
#endif
            //
            if (m_fg_dlgTalk_av2 != null)
            {
                m_fg_dlgTalk_av2.doClean_ifActivityIsFinishing();
            }


            //
            //removeFrom_tmpTools(mOwner_hTalk.m_var, "OnPause");
            //
            return 0;

        }
        protected override void OnStop()
        {
            /*  if (null != m_bsd_askToStopAv)
              {
                  m_bsd_askToStopAv.Dismiss();
              }*/

            //  手工取消要清除状态
            this.mOwner_hTalk.m_var.av.m_bState_canSpeak = false;
            if (null != mHandler)
            {
                //  mHandler.RemoveCallbacksAndMessages(null);

            }

            GuiShare.pf_PostMessage_new(this, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);
            qyFuncs.traceLog("activity_dlgTalk_av 进入后台 发送退会消息");
            qyFuncs.traceLog("Activity_dlgTalk_av  OnStop");
            base.OnStop();
        }

        protected override void OnPause()
        {
            qyFuncs.traceLog("Activity_dlgTalk_av  OnPause");
            //
            string str;
            str = string.Format("dlgTalk_av: onPause, {0}", IsFinishing ? "IsFinishing" : "");
            qyFuncs.showInfo_open(0, null, str);
            if (IsFinishing)
            {
                //
                doClean("OnPause,isFinishing");
            }

            //
            base.OnPause();




        }

        protected override void OnDestroy()
        {
            bottomBarHandler.RemoveCallbacksAndMessages(null);
            qyFuncs.traceLog("Activity_dlgTalk_av  OnDestroy");
            qyFuncs.showInfo_open(0, null, "dlgTalk_av.OnDestroy enters");

            //
            doClean("onDestroy");

            //
            if (mOwner_hTalk != null)
            {
                removeFrom_tmpTools(mOwner_hTalk, mOwner_hTalk.m_var, "OnDestroy");
            }
            if (null != mHandler)
            {
                mHandler.RemoveCallbacksAndMessages(null);
                qyFuncs.traceLog("mHandler.RemoveCallbacksAndMessages(null);");
                mHandler = null;
            }

            qyFuncs.showInfo_open(0, null, "dlgTalk_av.OnDestroy leaves");
            //
            base.OnDestroy();
            CCtxQyMc pQyMc = g.g_pQyMc;
            ((MainActivity)pQyMc.gui.hMainWnd).canReceiveMessage = true;


        }

        //
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            //
            //横竖屏切换前调用，保存用户想要保存的数据，以下是样例
            outState.PutString("name", "yoosir");
            int ii = 0;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            pProcInfo.av.m_bInSleepMode = true;

            //
            //pauseTaskAv("OnSaveInstanceState");

        }


        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            //
            if (savedInstanceState != null)
            {
                String name = savedInstanceState.GetString("name");
                //
                int ii = 0;

                //
            }
        }


        //
        public int doAutoCancel()
        {

            DLG_TALK_var m_var = this.mOwner_hTalk.get_pm_var();
            string str;

            //
            str = "dlgTalk_av.doAutoCancel, " + this.tn_dlg;
            qyFuncs.showInfo_open(0, "", str);


            //
            /*
            if (m_var.av.taskInfo.bTaskExists)
            {
                //
                if (m_var.av.bAutoCancel_lostVideo)
                {
                    //
                    //uint dwTickCnt = qyFuncs.myGetTickCount(null);
                    //int iElapseInMs = (int)(dwTickCnt - m_var.av.dwTickCnt_autoCancel_lostVideo);
                    //if (Java.Lang.Math.Abs(iElapseInMs) < 3000)
                    {
                        if (!m_var.av.bDoneOnce_autoCancel_lostVideo)
                        {

                            //  自动关闭视频窗口
                            GuiShare.pf_PostMessage_new(this, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);

                        }
                        //
                        str = "talk_av " + tn_dlg + " onBackPressed autoCancel";
                        qyFuncs.showInfo_open(0, "", str);
                        //
                        return;
                    }
                }
            }
            */

            //  自动关闭视频窗口
            GuiShare.pf_PostMessage_new(this, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);




            //
            return 0;
        }


        //
        public override void OnBackPressed()
        {
            qyFuncs.traceLog("OnBackPressed" + m_fg_dlgTalk_av2.currentVideoStatus);

            if (m_fg_dlgTalk_av2.currentVideoStatus == 0)
            {
                //
                exitDlgTalkAv();


                // base.OnBackPressed();
            }
            else
            {
                m_fg_dlgTalk_av2.resetCurrentVideoChanel();
            }

        }

        public void exitDlgTalkAv()
        {
            if (!showBackSheetDialog)
            {
                BtnYes_Click(null, null);
                return;
            }

            DLG_TALK_var m_var = this.mOwner_hTalk.get_pm_var();
            string str;

            qyFuncs.traceLog("exitDlgTalkAv()点击退出");

            //
            // if (m_var.av.taskInfo.bTaskExists)
            //    {
            qyFuncs.traceLog("m_bsd_askToStopAv.SetContentView(view)点击退出");

            View view = this.LayoutInflater.Inflate(Resource.Layout.layout_yesCancel, null);
            this.m_bsd_askToStopAv = new BottomSheetDialog(this);
            m_bsd_askToStopAv.SetContentView(view);
            //
            BottomSheetBehavior behavior = BottomSheetBehavior.From((View)view.Parent);
            behavior.State = BottomSheetBehavior.StateExpanded;

            //
            TextView tvTitle = (TextView)view.FindViewById(Resource.Id.title);
            tvTitle.Text = "清扬视频";
            TextView tvContent = (TextView)view.FindViewById(Resource.Id.content);
            tvContent.Text = "要停止视频通话吗？";
            //
            Button btnYes = (Button)view.FindViewById(Resource.Id.yes);
            btnYes.Click += BtnYes_Click;
            Button btnCancel = (Button)view.FindViewById(Resource.Id.cancel);
            btnCancel.Click += BtnCancel_Click;
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc != null)
            {
                btnYes.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_yes);
                btnCancel.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_cancel);
                //
                m_bsd_askToStopAv.Show();

                //
                str = "talk_av " + tn_dlg + " onBackPressed, after ask";
                qyFuncs.showInfo_open(0, "", str);
            }
            //  }

            str = "talk_av " + tn_dlg + " onBackPressed";
            qyFuncs.showInfo_open(0, "", str);
        }


        //
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            //
            if (hasFocus)
            {
                hideSystemUI();
            }
            else
            {
                showSystemUI();
            }
        }

        public void hideSystemUI()
        {
            // Enables regular immersive mode.
            // For "lean back" mode, remove SYSTEM_UI_FLAG_IMMERSIVE.
            // Or for "sticky immersive," replace it with SYSTEM_UI_FLAG_IMMERSIVE_STICKY
            /*     View decorView = this.Window.DecorView;
                 decorView.SystemUiVisibility=(StatusBarVisibility)(
                         SystemUiFlags.Immersive
                         // Set the content to appear under the system bars so that the
                         // content doesn't resize when the system bars hide and show.
                         | SystemUiFlags.LayoutStable
                         | SystemUiFlags.LayoutHideNavigation
                         | SystemUiFlags.LayoutFullscreen
                         // Hide the nav bar and status bar
                         | SystemUiFlags.HideNavigation
                         | SystemUiFlags.Fullscreen);*/
        }

        // Shows the system bars by removing all the flags
        // except for the ones that make the content appear under the system bars.
        public void showSystemUI()
        {
            /*  View decorView = Window.DecorView;
              decorView.SystemUiVisibility=(StatusBarVisibility)(
                      SystemUiFlags.LayoutStable
                      | SystemUiFlags.LayoutHideNavigation
                      | SystemUiFlags.LayoutFullscreen);*/
        }


        //
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            m_bsd_askToStopAv.Dismiss();
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //
#if false
            if(this.mOwner_hTalk.m_var.av.taskInfo.bTaskExists)
            {
                //qyFuncs_Droid.dlgTalk_cancelTasks(mOwner_hTalk, mOwner_hTalk.m_var, mOwner_hTalk.m_var.av.taskInfo.iTaskId, true);
                ushort usOp = 0;
                if (mOwner_hTalk.m_var.av.taskInfo.ucbStarter) usOp = Consts.CONST_imOp_send_cancel;
                else usOp = Consts.CONST_imOp_recv_cancel;
                //
                PARAM_dlgTalk_procTask param = new PARAM_dlgTalk_procTask();
                //
                qyFuncs_Droid.dlgTalk_procTask1(mOwner_hTalk, mOwner_hTalk.m_var.av.taskInfo.iTaskId, usOp, "", ref param);
            }
            //
            this.doFinish("BtnYes_Click");
#endif
            //
            if (showBackSheetDialog)
            {
                m_bsd_askToStopAv.Dismiss();
            }


            //  手工取消要清除状态
            this.mOwner_hTalk.m_var.av.m_bState_canSpeak = false;
            if (null != mHandler)
            {
                //  mHandler.RemoveCallbacksAndMessages(null);

            }

            //
            qyFuncs.showInfo_open(0, "", "BtnYes_Click");

            //
            GuiShare.pf_PostMessage_new(this, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);
            qyFuncs.traceLog("发送点击退出msg");


        }

        //
        private int addTo_tmpTools(Activity_dlgTalk activity_DlgTalk,DLG_TALK_var pMgrVar, string hint)
        {
            int iErr = -1;
            string str;

            if (hint == null) hint = "";

            /*
            int i;
            for (i = 0; i < pMgrVar.tmp_hTools_dlgTalk_av.GetLength(0); i++)
            {
                if (pMgrVar.tmp_hTools_dlgTalk_av[i] == null)
                {
                    pMgrVar.tmp_hTools_dlgTalk_av[i] = this;
                    break;
                }
            }
            if (i == pMgrVar.tmp_hTools_dlgTalk_av.GetLength(0))
            {
                str = string.Format("dlgTalk_av.OnCreate failed, too many tmp_hTools");
                qyFuncs.showInfo_open(0, null, str);
                //
                goto errLabel;
            }
            */
            if  (activity_DlgTalk.tmp_hTool_dlgTalk_av!=null)
            {
                str = string.Format("dlgTalk_av.OnCreate failed, too many tmp_hTools");
                qyFuncs.showInfo_open(0, null, str);
                //
                goto errLabel;

            }
            activity_DlgTalk.tmp_hTool_dlgTalk_av = this;


            //
            iErr = 0;
        errLabel:
            return iErr;
        }

        int removeFrom_tmpTools(Activity_dlgTalk activity_DlgTalk,DLG_TALK_var pMgrVar, string hint)
        {
            int iErr = -1;

            string str;

            if (hint == null) hint = "";

            int i;
            /*
            for (i = 0; i < pMgrVar.tmp_hTools_dlgTalk_av.GetLength(0); i++)
            {
                if (pMgrVar.tmp_hTools_dlgTalk_av[i] == this)
                {
                    pMgrVar.tmp_hTools_dlgTalk_av[i] = null;
                    pMgrVar.tmp_share_eglContexts[i] = null;
                    //
                    break;
                }
            }
            */
            if  (activity_DlgTalk.tmp_hTool_dlgTalk_av==this)
            {
                activity_DlgTalk.tmp_hTool_dlgTalk_av = null;
                pMgrVar.tmp_share_eglContext = null;
            }

            //
            //mOwner_hTalk.gui_PostMessage(Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_chkTalkDraw, 0);
            mOwner_hTalk.doChkTalkDraw();


            //
            iErr = 0;
        errLabel:
            return iErr;
        }

        //
        public static int tmpHandler_pauseTaskAv(CTX_caller ctxCaller, ref MIS_MSGU p0, PARAM_findOrgMsg p1, object p2)
        {
            int iErr = -1;
            //int iTaskId = (int)p0;
            int iTaskId = p1.iTaskId;
            //p1
            TASK_common taskMem = (TASK_common)p2;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskMem.m_index_taskInfo, "tmpHandler_pauseTaskAv");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            if (pTaskData.msgU.uiType == Consts.CONST_misMsgType_task
                && pTaskData.msgU.task.iTaskId == iTaskId)
            {
                TASK_transferAv task = (TASK_transferAv)taskMem;
                //
                qyFuncs.stopTransformThreads(pProcInfo, ref task.myTask, "tmpHandler_pauseTaskAv");


                //
                iErr = 1; goto errLabel;

            }

            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public int pauseTaskAv(string hint)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            string str;

            //
            if (hint == null) hint = "";

            //
            DLG_TALK_var m_var = mOwner_hTalk.m_var;
            object hTalk = mOwner_hTalk;
            int iWndContentType = Consts.CONST_qyWndContentType_talker;

            //
            str = string.Format("pauseTaskAv enters, {0}", hint);
            qyFuncs.showInfo_open(0, null, str);

            //
            if (!m_var.av.taskInfo.bTaskExists)
            {
                qyFuncs.showInfo_open(0, "", "pauseTaskAv returns when no meeting");
                return -1;
            }
            //
            if (m_var.av.ucbGot_iIndex_sharedObj_localAv)
            {
                QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                if (pSharedObj == null) goto errLabel;
                if (pSharedObj.var.bCapDev)
                {
                    CAP_procInfo_bmpCommon pCapBmp = qyFuncs.getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
                    if (pCapBmp == null) goto errLabel;


                    qyFuncs.stopCapture(hTalk, iWndContentType, (CAP_procInfo_video)pCapBmp);


                    qyFuncs.closeLocalVideo(hTalk, iWndContentType, m_var.av.iIndex_sharedObj_localAv);

                }

                // audio
                pProcInfo.stopLocalAudioRecorder(m_var.av.iIndex_sharedObj_localAv, 100);

            }

            //
            CMIS_MSGU p0 = new CMIS_MSGU();
            PARAM_findOrgMsg param = new PARAM_findOrgMsg();
            param.iTaskId = m_var.av.taskInfo.iTaskId;

            //
            //pProcInfo.processQ_media.qTraverse_mono(null, tmpHandler_pauseTaskAv, m_var.av.taskInfo.iTaskId, null);
            pProcInfo.processQ_media.qTraverse_mono(null, tmpHandler_pauseTaskAv, ref p0.msgU, param);


            //
            m_var.m_bNeedResumeTaskAv = true;


            //
            iErr = 0;
        errLabel:

            //
            qyFuncs.showInfo_open(0, null, "pauseTaskAv leaves");

            //
            //mHandler.RemoveCallbacksAndMessages(null);
            //qyFuncs_Droid.doGC("pauseTaskAv " + hint);

            //
            return iErr;
        }


        //
        public static int tmpHandler_resumeTaskAv(CTX_caller ctxCaller, ref MIS_MSGU p0, PARAM_findOrgMsg p1, object p2)
        {
            int iErr = -1;
            //int iTaskId = (int)p0;
            int iTaskId = p1.iTaskId;
            //p1
            TASK_common taskMem = (TASK_common)p2;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskMem.m_index_taskInfo, "tmpHandler_resumeTaskAv");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            if (pTaskData.msgU.uiType == Consts.CONST_misMsgType_task
                && pTaskData.msgU.task.iTaskId == iTaskId)
            {
                TASK_transferAv task = (TASK_transferAv)taskMem;
                //
                qyFuncs.startTransformThreads(pProcInfo, task.myTask.uiTaskType, ref task.myTask, "tmpHandler_resumeTaskAv");


                //
                iErr = 1; goto errLabel;

            }

            iErr = 0;
        errLabel:

            return iErr;
        }


        public int resumeTaskAv(string hint)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            DLG_TALK_var m_var = mOwner_hTalk.m_var;
            object hTalk = mOwner_hTalk;
            int iWndContentType = Consts.CONST_qyWndContentType_talker;

            string str;

            if (hint == null) hint = "";

            //
            str = string.Format("resumeTaskAv enters, {0}", hint);
            qyFuncs.showInfo_open(0, null, str);

            //
            m_var.m_bNeedResumeTaskAv = false;

            //
            if (!m_var.av.taskInfo.bTaskExists)
            {
                qyFuncs.showInfo_open(0, null, "resumeTaskAv returns when no meeting ");
                return -1;
            }

            //
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;
            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;

            //
            pTc.shared_eglContext = m_var.m_share_eglContext;

            //
            //
            if (m_var.av.ucbGot_iIndex_sharedObj_localAv)
            {
                QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                if (pSharedObj == null) goto errLabel;

                //
                int iIndex_sharedObjUsr = 0;
                SHARED_OBJ_USR pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, iIndex_sharedObjUsr);
                if (null == pSharedObjUsr) goto errLabel;
                pSharedObjUsr.uiTranNo_openAvDev_v = 0;

                //
                if (pSharedObj.var.bCapDev)
                {
                    CAP_procInfo_bmpCommon pCapBmp = qyFuncs.getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
                    if (pCapBmp == null) goto errLabel;
                    //
                    uint uiCamCapType = Consts.CONST_camCapType_android;
                    VIDEO_COMPRESSOR_CFG compressor = pCapBmp.compressVideo.compressor;
                    compressor.shared_eglContext = pTc.shared_eglContext;

                    if (0 != qyFuncs.openLocalVideo(pProcInfo, uiCamCapType, "", hTalk, iWndContentType, ref compressor, m_var.av.iIndex_sharedObj_localAv))
                    {
                        str = string.Format("resumeTaskAv failed, openLocalVideo failed");
                        qyFuncs.showInfo_open(0, "", str);
                        goto errLabel;
                    }
                    //
                    qyFuncs.startCapture(hTalk, iWndContentType, (CAP_procInfo_video)pCapBmp);

                    //
                    ref COMPRESS_VIDEO pCompressVideo = ref pCapBmp.compressVideo;

                    if (0 != pCompressVideo.uiTranNo_openAvDev_org)
                    {   //  
                        pSharedObjUsr.uiTranNo_openAvDev_v = (uint)(pCompressVideo.uiTranNo_openAvDev_org + iIndex_sharedObjUsr); //  pSharedObj->iIndex_curUsr;
                    }

                    //
                    m_var.av.taskInfo.local_avStream.tranInfo.video.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;
                    m_var.av.taskInfo.local_avStream.tranInfo.video.compressor = pCompressVideo.compressor.common.head;
                    m_var.av.taskInfo.local_avStream.tranInfo.video.vh_decompress = pCompressVideo.vh_decompress;
                    m_var.av.taskInfo.local_avStream.tranInfo.video.vh_compress = pCompressVideo.vh_compress;
                    m_var.av.taskInfo.local_avStream.tranInfo.video.vh_stream = pCompressVideo.vh_stream;

                    //
                }

                // audio                  
                CAP_procInfo_audioCommon pCapAudio = qyFuncs.getCapAudioBySth(pProcInfo, pSharedObj.var.iIndex_capAudio, 0);
                AUDIO_COMPRESSOR_CFG aCompressor = pCapAudio.compressAudio.compressor;
                if (0 == qyFuncs_Droid.startLocalAudioRecorder(pProcInfo.cfg.policy.audio.uiRecordType, null, "", m_var.av.iIndex_sharedObj_localAv, ref aCompressor))
                { }


            }

            //
            CMIS_MSGU p0 = new CMIS_MSGU();
            PARAM_findOrgMsg p1 = new PARAM_findOrgMsg();
            p1.iTaskId = m_var.av.taskInfo.iTaskId;


            //
            //pProcInfo.processQ_media.qTraverse_mono(null, tmpHandler_resumeTaskAv, m_var.av.taskInfo.iTaskId, null);
            pProcInfo.processQ_media.qTraverse_mono(null, tmpHandler_resumeTaskAv, ref p0.msgU, p1);


            //
            Activity_dlgTalk dlgTalk = mOwner_hTalk;
            bool canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);

            if (canSpeak)
            {
                //
                qyFuncs_Droid.dlgTalk_requestToSpeak(dlgTalk, canSpeak);

                bool bState_canSpeak = canSpeak;

                //
                this.m_fg_dlgTalk_av2.updateRequestButtonStatus();

            }

            this.m_fg_dlgTalk_av2.trigKeyFrame();

            //
            iErr = 0;
        errLabel:

            qyFuncs.showInfo_open(0, null, "resumeTaskAv leaves with " + iErr);

            return iErr;
        }

        //
        public int save_dlgTalk_share_eglContext(EGLContext share_eglContent, string hint)
        {
            int iErr = -1;
            string str;

            if (hint == null) hint = "";

            //
            Activity_dlgTalk dlgTalk = null;
            Activity_dlgTalk_av act = (Activity_dlgTalk_av)this;// mOwner.Activity;
            if (act != null)
            {
                dlgTalk = act.mOwner_hTalk;
                DLG_TALK_var m_var = dlgTalk.m_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;

                //
                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (0 != syncObj.sync("", pMisCnt.hMutex_syncShareEglContext)) goto errLabel;

                    int i;
                    /*
                    for (i = 0; i < dlgTalk.m_var.tmp_hTools_dlgTalk_av.GetLength(0); i++)
                    {
                        if (dlgTalk.m_var.tmp_hTools_dlgTalk_av[i] == this)
                        {
                            dlgTalk.m_var.tmp_share_eglContexts[i] = share_eglContent;
                            break;
                        }

                    }
                    */
                    if  (  dlgTalk.tmp_hTool_dlgTalk_av==this)
                    {
                        dlgTalk.m_var.tmp_share_eglContext = share_eglContent;
                    }

                }
            }

            //
            iErr = 0;
        errLabel:

            str = string.Format("save_dlgTalk_share_eglContext called, {0} {1} {2}", (share_eglContent != null) ? "egl" : "null", (iErr == 0) ? "ok" : "failed", hint);
            qyFuncs.showInfo_open(0, null, str);

            return iErr;
        }



        //
        public void changeCamera()
        {
            Activity_dlgTalk dlgTalk = this.mOwner_hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;

            //
            if (this.m_bGot_closeWndMsg)
            {
                return;
            }

            //如果是竖屏切换到全屏
            if (m_var.av.taskInfo.bTaskExists)
            {
                //
                pauseTaskAv("changeCamera");
                //

                m_var.av.taskInfo.bUse_backCamera = !m_var.av.taskInfo.bUse_backCamera;
                //

                resumeTaskAv("changeCamera");

                if (m_var.av.taskInfo.bUse_backCamera)
                {
                    m_fg_dlgTalk_av2.btnChangeCamera.SetTextColor(Resources.GetColor(Resource.Color.av_button_active_text_color));
                    m_fg_dlgTalk_av2.btnChangeCamera.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_active_color));
                }
                else
                {
                    m_fg_dlgTalk_av2.btnChangeCamera.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_normal_color));
                    m_fg_dlgTalk_av2.btnChangeCamera.SetTextColor(Resources.GetColor(Resource.Color.av_button_normal_text_color));

                }
            }

            return;
        }


        //
        public void changeLocalVideo()
        {
            Activity_dlgTalk dlgTalk = this.mOwner_hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;

            //如果是竖屏切换到全屏
            if (m_var.av.taskInfo.bTaskExists)
            {
                m_var.av.taskInfo.bShow_localVideo = !m_var.av.taskInfo.bShow_localVideo;
            }
            if (m_var.av.taskInfo.bShow_localVideo)
            {
                m_fg_dlgTalk_av2.btnLocalVideo.Text = "关闭本地视频";
                m_fg_dlgTalk_av2.btnLocalVideo.SetTextColor(Resources.GetColor(Resource.Color.av_button_active_text_color));
                m_fg_dlgTalk_av2.btnLocalVideo.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_active_color));
            }
            else
            {
                m_fg_dlgTalk_av2.btnLocalVideo.Text = "本地视频";
                m_fg_dlgTalk_av2.btnLocalVideo.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_normal_color));
                m_fg_dlgTalk_av2.btnLocalVideo.SetTextColor(Resources.GetColor(Resource.Color.av_button_normal_text_color));

            }
            //               
            return;
        }

        //
        public static int initVideoCapDev(object hTalk, int iWndContentType, string displayName_selected, ref BITMAPINFOHEADER pBih_suggested, CAP_procInfo_bmpCommon pCapBmp)
        {
            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (!GuiShare.pf_IsWindow(dlgTalk.m_hTool_dlgTalk_av)) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;

            //

            if (dlgTalk_av.m_fg_dlgTalk_av2 != null)
            {
                return Fragment_dlgTalk_av2.initVideoCapDev(hTalk, iWndContentType, displayName_selected, ref pBih_suggested, pCapBmp);
            }

            return -1;
        }

        public static int exitVideoCapDev(object hTalk, int iWndContentType)
        {
            int iErr = -1;

            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (null == dlgTalk.m_hTool_dlgTalk_av) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;

            //

            if (dlgTalk_av.m_fg_dlgTalk_av2 != null)
            {
                return Fragment_dlgTalk_av2.exitVideoCapDev(hTalk, iWndContentType);
            }

            return -1;

        }

        public static bool bGetCapturePara(object hTalk, int iWndContentType, ref QY_VIDEO_HEADER pVh_org)
        {
            bool bRet = false;

            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return false;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (!GuiShare.pf_IsWindow(dlgTalk.m_hTool_dlgTalk_av)) return false;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;


            if (dlgTalk_av.m_fg_dlgTalk_av2 != null)
            {
                return Fragment_dlgTalk_av2.bGetCapturePara(hTalk, iWndContentType, ref pVh_org);
            }

            return false;

        }

        public static int startCapture(object hTalk, int iWndContentType)
        {
            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (!GuiShare.pf_IsWindow(dlgTalk.m_hTool_dlgTalk_av)) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;


            if (dlgTalk_av.m_fg_dlgTalk_av2 != null)
            {
                return Fragment_dlgTalk_av2.startCapture(hTalk, iWndContentType);
            }

            return 0;
        }

        public static int stopCapture(object hTalk, int iWndContentType)
        {
            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (!GuiShare.pf_IsWindow(dlgTalk.m_hTool_dlgTalk_av)) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;

            //

            if (dlgTalk_av.m_fg_dlgTalk_av2 != null)
            {
                return Fragment_dlgTalk_av2.stopCapture(hTalk, iWndContentType);
            }

            return 0;
        }

        //
        public int doTask_av(int iCmd, int iTaskId)
        {
            Activity_dlgTalk_av dlgTalk_av = this;
            //

            if (dlgTalk_av.m_fg_dlgTalk_av2 != null)
            {
                return dlgTalk_av.m_fg_dlgTalk_av2.doTask_av(iCmd, iTaskId);
            }

            return -1;
        }

        public void OnDismiss()
        {
            m_fg_dlgTalk_av2.OnDismiss();
        }


    }
}


