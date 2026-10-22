
using qyMessengerApp.Android;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Thread = Java.Lang.Thread;
using AndroidX.Fragment.App;
using Java.Lang;
using AndroidX.Core.App;
using qy;
using qy.Droid;


using Google.Android.Material.BottomSheet;
using Android.Media;
using Android.Graphics;
using Android.Util;
using static Android.Icu.Text.CaseMap;
using System.Security.Cryptography;
using AndroidX.ViewPager.Widget;
using String = Java.Lang.String;
using Org.Aviran.Cookiebar2;
using Org.Aviran.Cookiebar2;
using Android.App;
using System;


//
namespace qyMessengerApp.Droid
{
    [Activity(Label = "@string/appName", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    //public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    public class MainActivity : qmcBaseActivity, View.IOnClickListener, ViewPager.IOnPageChangeListener
    {


        public QY_MC_mainWndVar var_common;
        //ZONE_objs_info wall;
        

        //
        public string m_str_title;

        //
        public RelativeLayout m_layout_total;
        private TextView txt_topbar;
        private TextView txt_msg;
        private TextView txt_contact;
        private TextView txt_me;
        private ViewPager vPager;

        //Fragment Object
        //private MyFragment fg1, fg2, fg3, fg4;
        private FragmentPagerAdapter_main mAdapter;

        public const int PAGE_ONE = 0;
        public const int PAGE_TWO = 1;
        public const int PAGE_THREE = 2;

        //
        public MyHandler mHandler = null;

        //
#if DEBUG
        public MyThread m_hThread_test;
        public bool bQuit_test;
#endif


        //
        //public DLG_TALK_videoConference[] m_tmp_videoConferences = new DLG_TALK_videoConference[Consts.DEFAULT_usMaxCnt_taskInfos];
        public QMC_taskData_common[] m_tmp_taskDatas = new QMC_taskData_common[Consts.DEFAULT_usMaxCnt_taskInfos];
        public MyThread[] m_tmp_threads = new MyThread[Consts.DEFAULT_usMaxCnt_androidThreads];
        public System.Threading.Mutex hMutex_syncThreads = new System.Threading.Mutex();

        //
        public class MyHandler : Handler
        {
            MainActivity mOwner;

            public MyHandler(MainActivity owner)
            {
                mOwner = owner;
            }

            public override async void HandleMessage(Message msg)
            {

                CCtxQyMc pQyMc = g.g_pQyMc;
                if (pQyMc == null)
                {
                    if (msg.What == Consts.CONST_qyWm_postComm
                        && msg.Arg1 == Consts.CONST_qyWmParam_closeWnd)
                    {
                        qyFuncs.traceLog("get a msg: closeWnd. to doFinish if pQyMc is null");

                        mOwner.doFinish("MyHandler.get a msg closeWnd");
                    }
                    //
                    return;
                }
                MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;
                //
                //qyFuncs.showInfo_open(0, null, "mainWnd.MyHandler.HandleMessage");

                //
                switch (msg.What)
                {
                    case Consts.CONST_qyWm_postComm:

                        //
                        switch (msg.Arg1)
                        {
                            case Consts.CONST_qyWmParam_login_canceled:
                                //if ( !pQyMc.bLogon )


                                qyFuncs.traceLog("get a msg: login canceled");
                                mainWnd.doFinish("login canceled");
                                break;
                            case Consts.CONST_qyWmParam_closeWnd:
                                {
                                    //
                                    if (!pQyMc.bLogon)
                                    {


                                        GuiShare.pf_PostMessage_new(mainWnd, Consts.CONST_qyWndContentType_mainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_login_canceled, 0);
                                        break;
                                    }
                                    //
                                    if (!mainWnd.m_bReadyToQuit)
                                    {

                                        if (mainWnd.isActive)
                                        {
                                            if (mainWnd.m_bsd_askToQuit != null)
                                            {
                                                mainWnd.m_bsd_askToQuit.Dismiss();
                                                mainWnd.m_bsd_askToQuit = null;
                                            }
                                            View view = mainWnd.LayoutInflater.Inflate(Resource.Layout.layout_yesCancel, null);
                                            mainWnd.m_bsd_askToQuit = new BottomSheetDialog(mainWnd);
                                            mainWnd.m_bsd_askToQuit.SetContentView(view);
                                            //
                                            BottomSheetBehavior behavior = BottomSheetBehavior.From((View)view.Parent);
                                            behavior.State = BottomSheetBehavior.StateExpanded;

                                            //
                                            TextView tvTitle = (TextView)view.FindViewById(Resource.Id.title);
                                            tvTitle.Text = "清扬视频";
                                            TextView tvContent = (TextView)view.FindViewById(Resource.Id.content);
                                            tvContent.Text = "要退出吗？";
                                            //
                                            Button btnYes = (Button)view.FindViewById(Resource.Id.yes);
                                            btnYes.Click += BtnYes_Click;
                                            Button btnCancel = (Button)view.FindViewById(Resource.Id.cancel);
                                            btnCancel.Click += BtnCancel_Click; ;
                                            //
                                            btnYes.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_yes);
                                            btnCancel.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_cancel);

                                            //
                                            mainWnd.m_bsd_askToQuit.Show();

                                        }

                                        break;
                                    }


                                    //
                                    if (mainWnd.m_progressDialog != null) break;


                                    //
                                    qyFuncs.startToExitQyMc(pQyMc);
                                    //
                                    mainWnd.m_progressDialog = new ProgressDialog(mainWnd);
                                    mainWnd.m_progressDialog.Max = (100); // Progress Dialog Max Value
                                    mainWnd.m_progressDialog.SetMessage(qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_waitToQuit)); // Setting Message
                                    mainWnd.m_progressDialog.SetTitle(qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_progress)); // Setting Title
                                    mainWnd.m_progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal); // Progress Dialog Style Horizontal
                                    mainWnd.m_progressDialog.Show(); // Display Progress Dialog
                                    mainWnd.m_progressDialog.SetCancelable(false);

                                    //
                                    mainWnd.m_progressHandler = new Tmp_progressHandler(mainWnd.m_progressDialog);
                                    //
                                    mainWnd.m_progressThread = new Tmp_progressThread(mainWnd.m_progressHandler);
                                    mainWnd.m_progressThread.Start();
                                }
                                break;
                            case Consts.CONST_qyWmParam_threadProc_isCli_end:
                                {
                                    mainWnd.doClean("after msg threadProc_isCli_end");

                                    if (mainWnd.m_progressThread != null)
                                    {
                                        mainWnd.m_progressThread.bQuit = true;

                                        try
                                        {
                                            mainWnd.m_progressThread.Stop();
                                        }
                                        catch (UnsupportedOperationException ie)
                                        {

                                        }

                                        mainWnd.m_progressThread = null;
                                    }
                                    //
                                    if (mainWnd.m_progressDialog != null)
                                    {
                                        mainWnd.m_progressDialog.Dismiss();
                                        mainWnd.m_progressDialog = null;
                                    }
                                    //

                                    mainWnd.m_progressHandler = null;
                                    //
                                    mainWnd.doFinish("after msg threadProc_isCli_end");
                                }
                                break;

                            case Consts.CONST_qyWmParam_imObj_needRefreshed:

                                if (mainWnd.mAdapter.mFragment_imObj1 != null)
                                {
                                    mainWnd.mAdapter.mFragment_imObj1.fillList_imObj();
                                    mainWnd.mAdapter.mFragment_imObj1.display_imObjList();
                                }
                                break;
                            case Consts.CONST_qyWmParam_allContactsRetrieved:
                                //
                                // qyFuncs.showNotification_open(0, 0, 0, qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_allImObjectsRetrieved));
                                //
                                qyFuncs.traceLog(qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_allImObjectsRetrieved));

                                if (null != mainWnd.mAdapter && mainWnd.mAdapter.mFragment_msg1 != null)
                                {
                                    mainWnd.mAdapter.mFragment_msg1.loadRecentContacts();
                                }


                                break;
                            case Consts.CONST_qyWmParam_msgArrive:
                            default:
                                qyFuncs_Droid.mainWnd_OnQyPostComm(pQyMc.gui.hMainWnd, ref mainWnd.var_common, msg.Arg1, msg.Arg2);
                                break;
                        }

                        break;
                    default:
                        break;
                }
                //
                return;
            }

            private void BtnCancel_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                mOwner.m_bsd_askToQuit.Dismiss();
            }

            private void BtnYes_Click(object sender, EventArgs e)
            {
                //throw new NotImplementedException();
                mOwner.m_bsd_askToQuit.Dismiss();

                //
                mOwner.m_bReadyToQuit = true;

                //

                GuiShare.pf_PostMessage_new(mOwner, Consts.CONST_qyWndContentType_mainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);
                //  System.Environment.Exit(0);
            }

            //


        }
        //




        //
        public int gui_PostMessage(object wnd, int msgId, int wParam, int lParam)
        {
#if false
            CCtxQyMc pQyMc = g.g_pQyMc;

            if (wnd == pQyMc.gui.hMainWnd)
            {
                MainActivity mainWnd = (MainActivity)wnd;
                Message msg = mainWnd.mHandler.ObtainMessage();
                msg.What = msgId;
                msg.Arg1 = wParam;
                msg.Arg2 = lParam;
                mainWnd.mHandler.SendMessage(msg);
                return 0;
            }


            return 0;
#endif
            //
            return gui_PostMessage_new(wnd, 0, msgId, wParam, lParam);
        }

        //
        public int gui_PostMessage_new(object wnd, int iWndContentType, int msgId, int wParam, int lParam)
        {

            Handler handler = null;

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc != null)
            {
                if (wnd == pQyMc.gui.hMainWnd)
                {
                    MainActivity mainWnd = (MainActivity)wnd;
                    handler = mainWnd.mHandler;
                }
            }

            if (handler == null)
            {
                switch (iWndContentType)
                {
                    case Consts.CONST_qyWndContentType_mainWnd:
                        {
                            MainActivity mainWnd = (MainActivity)wnd;
                            handler = mainWnd.mHandler;
                        }
                        break;
                    case Consts.CONST_qyWndContentType_dlgTalk_av:
                        {
                            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)wnd;
                            handler = dlgTalk_av.mHandler;
                        }
                        break;
                    case Consts.CONST_qyWndContentType_talker:
                        {
                            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)wnd;
                            handler = dlgTalk.mHandler;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (handler != null)
            {
                Message msg = handler.ObtainMessage();
                msg.What = msgId;
                msg.Arg1 = wParam;
                msg.Arg2 = lParam;
                handler.SendMessage(msg);
                return 0;
            }

            return 0;
        }



        public bool gui_IsWindow(object hWnd)
        {
            if (hWnd == null) return false;
            Activity wnd = (Activity)hWnd;
            return !wnd.IsFinishing;
        }

        public object mainWnd_getVar()
        {
            return this.var_common;
        }








        /////////////////////
        //
        public static unsafe int gui_procMsg(ref MIS_MSGU pMsgU)
        {
            int iErr = -1;

            string str;
            bool bDbg = false;

            //
            fixed (MIS_MSGU* pMsg = &pMsgU)
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
                QY_MESSENGER_ID addr_logicalPeer_idInfo; addr_logicalPeer_idInfo.ui64Id = 0;

                //
                if (bDbg)
                {
                    str = string.Format("gui_procMsg enters");
                    qyFuncs.showInfo_open(0, null, str);
                }

                //
                ref MIS_MSGU tmp_pMsg = ref *pMsg;
                //
                switch (pMsg->uiType)
                {
                    case Consts.CONST_misMsgType_talk:
                        addr_logicalPeer_idInfo = pMsg->talk.addr_logicalPeer.idInfo;
                        break;
                    case Consts.CONST_misMsgType_input:
                        {
                            if (!(qyFuncs.isUcFlgTalkData(pMsg->input.ucFlg) || qyFuncs.isUcFlgRouteTalkData(pMsg->input.ucFlg)))
                            {
                                goto errLabel;
                            }
                            addr_logicalPeer_idInfo = pMsg->input.addr_logicalPeer.idInfo;
                        }
                        break;
                    case Consts.CONST_misMsgType_taskStatus:
                        {
                            addr_logicalPeer_idInfo = pMsg->taskStatus.idInfo_logicalPeer;
                        }
                        break;
                    default:
                        goto errLabel;
                        break;


                }

                if (mainActivity.mAdapter.mFragment_msg1 != null)
                {
                    mainActivity.mAdapter.mFragment_msg1.gui_procMsg(ref pMsgU);
                }

                //
                object hTalk = null;
                if (0 == GuiShare.pf_findTalker(pQyMc, pMisCnt, addr_logicalPeer_idInfo, ref hTalk))
                {
                    Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
                    dlgTalk.gui_procMsg(ref pMsgU); ;

                }
            }

            iErr = 0;
        errLabel:

            //
            if (bDbg)
            {
                str = string.Format("gui_procMsg leaves, iErr {0}", iErr);
                qyFuncs.showInfo_open(0, null, str);
            }

            //
            return iErr;

        }



        public static unsafe int gui_dlgTalk_procMsg(object hTalkParam, ref MIS_MSGU pMsgParam)
        {
            if (hTalkParam == null) return -1;
            Activity_dlgTalk hTalk = (Activity_dlgTalk)hTalkParam;
            return hTalk.gui_procMsg(ref pMsgParam);
        }

        //
        public static unsafe int tmp_notify_avCall_ifBackgrounded()
        {
            // int iErr = -1;
            // CCtxQyMc pQyMc = g.g_pQyMc;
            //  MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            qyFuncs.traceLog("后台开启通知");
            // mainActivity.showTipNotification();





            /*            string title = "Notification";
                        string text = "notification text";

                        //Intent intent=new Intent(this,)
                        PendingIntent pIntent = Activity_notification.getDismissIntent(0, 1, mainActivity);
                        if (pIntent == null) return -1;
                        PendingIntent dismissIntnt = Activity_notification.getDismissIntent(0, 2, mainActivity);
                        if (dismissIntnt == null) return -1;

                        NotificationCompat.Builder builder = new NotificationCompat.Builder(mainActivity);
                        builder.SetSmallIcon(Resource.Drawable.Vegetables);
                        builder.SetTicker("Notification Ticker");
                        builder.SetContentTitle("Notification");
                        builder.SetContentText("Notification Text");
                        builder.AddAction(Resource.Drawable.icon, "Close", dismissIntnt);
                        builder.AddAction(Resource.Drawable.Vegetables, "Accept", pIntent);

                        builder.SetContentIntent(pIntent);
                        builder.SetAutoCancel(true);
                        builder.SetDefaults((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate));
                        //
                        builder.SetOngoing(true);
                        //
                        NotificationManager manager = (NotificationManager)mainActivity.GetSystemService(NotificationService);
                        Notification notification = builder.Build();
                        //
                        notification.Flags |= NotificationFlags.Insistent;
                        //
                        manager.Notify(0, notification);*/

            return 0;

        }

        //



        public static unsafe int tmp_notify_cancel_ifBackgrounded()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            NotificationManager manager = (NotificationManager)mainActivity.GetSystemService(NotificationService);
            manager.CancelAll();

            //

            //


            iErr = 0;
        errLabel:

            return iErr;

        }


        public static unsafe int tmp_notify_avCall_onlySound()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            string title = "Notification";
            string text = "notification text";

            //Intent intent=new Intent(this,)

            NotificationCompat.Builder builder = new NotificationCompat.Builder(mainActivity);
            builder.SetSmallIcon(Resource.Drawable.Vegetables);
            //builder.SetTicker("Notification Ticker");
            //builder.SetContentTitle("Notification");
            //builder.SetContentText("Notification Text");

            builder.SetAutoCancel(false);
            builder.SetDefaults((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate));
            //
            builder.SetOngoing(true);
            //
            NotificationManager manager = (NotificationManager)mainActivity.GetSystemService(NotificationService);
            Notification notification = builder.Build();
            //
            notification.Flags |= NotificationFlags.Insistent;
            //
            manager.Notify(0, notification);

            return 0;

        }

        public static unsafe int tmp_notify_avCall_onlySound_cancel()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            NotificationManager manager = (NotificationManager)mainActivity.GetSystemService(NotificationService);
            manager.CancelAll();

            return 0;

        }



        class CTmpDismiss : Java.Lang.Object, ICookieBarDismissListener
        {
            public void OnDismiss(int p0)
            {
                //throw new NotImplementedException();
                switch (p0)
                {
                   
                    case CookieBarDismissListenerDismissType.DurationComplete:
                    case CookieBarDismissListenerDismissType.UserDismiss:
                        gui_notify_doCmd(Consts.CONST_imOp_recv_cancel);
                        break;
                    case CookieBarDismissListenerDismissType.UserActionClick:
                        break;
                    default:
                        break;
                }

            }
        }

        class TmpCustomViewInitializer : Java.Lang.Object, CookieBar.ICustomViewInitializer
        {
            class Tmp_OnClickListener : Java.Lang.Object, View.IOnClickListener
            {
                public void OnClick(View v)
                {
                    CCtxQyMc pQyMc = g.g_pQyMc;
                    MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

                    //throw new NotImplementedException();
                    string str = string.Format("OnClick: id {0}", v.Id);
                    qyFuncs.traceLog(str);
                    //
                    if (v.Id == Resource.Id.btn_accept)
                    {
                        //
                        gui_notify_doCmd(Consts.CONST_imOp_recv_accept);
                    }
                    else if (v.Id == Resource.Id.btn_cancel)
                    {
                        //
                        gui_notify_doCmd(Consts.CONST_imOp_recv_cancel);
                    }
                }
            }
            public void InitView(View p0)
            {
                //throw new NotImplementedException();
                Button btnAccept = (Button)p0.FindViewById(Resource.Id.btn_accept);
                Button btnCancel = (Button)p0.FindViewById(Resource.Id.btn_cancel);
                //
                CCtxQyMc pQyMc = g.g_pQyMc;
                if (pQyMc == null) goto errLabel;
                btnAccept.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_accept);
                btnCancel.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_cancel);

                //
                Tmp_OnClickListener btn_listener = new Tmp_OnClickListener();
                Tmp_OnClickListener btn_listener1 = new Tmp_OnClickListener();

                btnAccept.SetOnClickListener(btn_listener);
                //
                btnCancel.SetOnClickListener(btn_listener1);
            errLabel:
                return;
            }
        }

        //
        public static int getCallerDesc(ref string title, ref string desc)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) return -1;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            if (mainActivity == null) return -1;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, mainActivity.var_common.notifyTaskStatus.index_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;

            QMC_taskData_common taskData = pTaskInfo.getTaskData();
            if (taskData == null) goto errLabel;
            if (taskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
            QMC_taskData_conf pTc = (QMC_taskData_conf)taskData;
            if (pTc.msgU.uiType != Consts.CONST_misMsgType_task) goto errLabel;
            QY_MESSENGER_ID idInfo_logicalPeer = pTc.msgU.task.addr_logicalPeer.idInfo;

            uint uiObjType = 0;
            string grpName = "";
            UInt64 idInfo_creator = 0;
            string dw = "";
            string bm = "";
            string syr = "";

            //
            if (0 != qyFuncs.getTalkerDesc(idInfo_logicalPeer, ref uiObjType, ref grpName, ref idInfo_creator, ref dw, ref bm, ref syr)) goto errLabel;

            title = "";

            //
            if (uiObjType == Consts.CONST_objType_imGrp)
            {
                title = grpName;
            }
            else
            {
                title = dw + bm + syr;
            }

            //
            desc = "视频通话";

            iErr = 0;
        errLabel:

            return iErr;
        }

        private static CookieBar cookieBar;
        //
        public static unsafe int tmp_notify_avCall_ifForegrounded(Activity hParent)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            if (!((MainActivity)pQyMc.gui.hMainWnd).canReceiveMessage)
            {
                return -1;
            }
            qyFuncs.traceLog("响铃通知avCall");
            if (null != ((MyApplication)(hParent.Application)).mediaPlayer)
            {

                qyFuncs.traceLog("播放铃声");
                ((MyApplication)(hParent.Application)).mediaPlayer.Start();


            }

            /*            AlertDialog.Builder builder = null;
                        builder = new AlertDialog.Builder(hParent);
                        LayoutInflater inflater = (LayoutInflater)hParent.GetSystemService(Android.Content.Context.LayoutInflaterService);
                        View view_custom = inflater.Inflate(Resource.Layout.layout_alert1, null, false);
                        builder.SetView(view_custom);
                        builder.SetCancelable(false);
                        AlertDialog m_alert = builder.Create();
                        Window wnd = m_alert.Window;
                        wnd.SetGravity(GravityFlags.Top);
                        WindowManagerLayoutParams lp = wnd.Attributes;
                        lp.Y = hParent.Resources.GetDimensionPixelOffset(Resource.Dimension.notification_dialog_margin_top);
                        wnd.Attributes = lp;*/
            if (cookieBar != null)
            {
                CookieBar.Dismiss(hParent);
                cookieBar = null;
            }
            CookieBar.Builder builder = CookieBar.Build(hParent);
            builder.SetCustomView(Resource.Layout.notify_avCall);
            builder.SetCustomViewInitializer(new TmpCustomViewInitializer());
            string title = "";
            string desc = "";
            getCallerDesc(ref title, ref desc);
            builder.SetTitle(title);
            builder.SetMessage(desc);
            builder.SetCookiePosition(CookieBar.Top);  // Cookie will be displayed at the bottom
            builder.SetDuration(pProcInfo.cfg.uiTimeoutInMs_waitForProcessingOf_avTask + 5000);
            builder.SetCookieListener(new CTmpDismiss());
            builder.SetEnableAutoDismiss(true);
            cookieBar = builder.Show();                              // of the screen



            if (!pProcInfo.cfg.notify_avCall_bNoSound)
            {
                tmp_notify_avCall_onlySound();
            }


            return 0;
        }

        public static unsafe int tmp_notify_cancel_ifForegrounded(Activity activity)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            CookieBar.Dismiss(activity);
            if (null != ((MyApplication)(mainActivity.Application)).mediaPlayer)
            {

                qyFuncs.traceLog("停止铃声");
                ((MyApplication)(mainActivity.Application)).mediaPlayer.Pause();


            }
            //
            tmp_notify_avCall_onlySound_cancel();

            return 0;
        }


        public static unsafe int gui_notify_cancel()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            if (mainActivity.var_common.notifyTaskStatus.curNotify.bShown)
            {
                if (mainActivity.var_common.notifyTaskStatus.curNotify.bForegrounded) tmp_notify_cancel_ifForegrounded((Activity)mainActivity.var_common.notifyTaskStatus.curNotify.hParent);
                else tmp_notify_cancel_ifBackgrounded();
                mainActivity.var_common.notifyTaskStatus.curNotify.bShown = false;
            }

            return 0;

        }

        //
        public static unsafe int gui_notify_chk()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) return -1;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            //
            Activity hParent = (Activity)pQyMc.gui.hActiveWnd;
            if (null == hParent) hParent = mainActivity;

            //
            bool bNeedShow = false;
            //
            if (mainActivity.var_common.notifyTaskStatus.bExists_task
                && mainActivity.var_common.notifyTaskStatus.bAvCall
                && qyFuncs.qmcTaskInfo_bAlive(null, pProcInfo, mainActivity.var_common.notifyTaskStatus.index_taskInfo)
                )
            {
                bNeedShow = true;
            }
            qyFuncs.traceLog("gui_notify_chk enters, bNeedShow :" + bNeedShow);
            //
            if (!bNeedShow)
            {
                if (mainActivity.var_common.notifyTaskStatus.curNotify.bShown)
                {
                    qyFuncs.traceLog("gui_notify_cancel(); 852");
                    gui_notify_cancel();
                }
                iErr = 0; goto errLabel;
            }
            //
            if (mainActivity.var_common.notifyTaskStatus.curNotify.bShown)
            {
                if (pQyMc.gui.bForeground == mainActivity.var_common.notifyTaskStatus.curNotify.bForegrounded
                    && hParent == mainActivity.var_common.notifyTaskStatus.curNotify.hParent)
                {
                    iErr = 0; goto errLabel;
                }
                qyFuncs.traceLog("gui_notify_cancel(); 865");

                gui_notify_cancel();

            }
            //
            mainActivity.var_common.notifyTaskStatus.curNotify.bForegrounded = pQyMc.gui.bForeground;
            mainActivity.var_common.notifyTaskStatus.curNotify.hParent = hParent;


            if (mainActivity.var_common.notifyTaskStatus.curNotify.bForegrounded) tmp_notify_avCall_ifForegrounded(hParent);
            else tmp_notify_avCall_ifBackgrounded();
            mainActivity.var_common.notifyTaskStatus.curNotify.bShown = true;



            iErr = 0;
        errLabel:

#if DEBUG
            qyFuncs.traceLog("gui_notify_chk leaves");
#endif

            return iErr;
        }



        public static unsafe int gui_notify_doCmd(ushort usOp)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            string str = string.Format("gui_notify_doCmd: iCmd {0}", usOp);
            qyFuncs.traceLog(str);
            qyFuncs.showInfo_open(0, "", str);

            //
            if (!mainActivity.var_common.notifyTaskStatus.bExists_task) return -1;
            int index_taskInfo = mainActivity.var_common.notifyTaskStatus.index_taskInfo;
            QMC_TASK_INFO pTaskInfo = pProcInfo.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, index_taskInfo, "gui_notify_doCmd");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;
            if (pTaskData.msgU.uiType != Consts.CONST_misMsgType_task) goto errLabel;

            switch (usOp)
            {
                case Consts.CONST_imOp_recv_cancel:
                    qyFuncs_Droid.do_cancelTask1(pTaskInfo.var.iTaskId, false, "gui_notify_doCmd");
                    mainActivity.var_common.notifyTaskStatus.bExists_task = false;


                    gui_notify_chk();
                    break;
                case Consts.CONST_imOp_recv_accept:
                    {
                        qyFuncs.traceLog("收到会议通知Consts.CONST_imOp_recv_accept");


                        MSGR_ADDR addr = pTaskData.msgU.task.addr_logicalPeer;
                        object hTalk = null;
                        if (GuiShare.pf_findTalker(pQyMc, pMisCnt, addr.idInfo, ref hTalk) == 0
                            && qyFuncs_Droid.bDlgTalkActive(hTalk))
                        {
                            qyFuncs.traceLog("GuiShare.pf_findTalker(pQyMc, pMisCnt, addr.idInfo, ref hTalk) == 0 && qyFuncs_Droid.bDlgTalkActive(hTalk) 为ture");
                            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;

                            ((MyApplication)(mainActivity.Application)).mediaPlayer.Pause();

                            // shall send message to dlgTalk
                            dlgTalk.gui_PostMessage(Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_doTask, usOp);

                            break;
                        }
                        PARAM_talkToMessenger param = new PARAM_talkToMessenger();
                        //
                        param.wParam = Consts.CONST_qyWmParam_doTask;
                        param.lParam = usOp;
                        //
                        qyFuncs_Droid.talkToMessenger(pQyMc, &addr, false, true, ref param);

                    }
                    break;

                default:

                    break;

            }

            //
            //qyFuncs.mainWnd_procTask(mainActivity.var_common.notifyTaskStatus.index_taskInfo, usOp);


            iErr = 0;
        errLabel:

            return iErr;

        }


        //
        public unsafe int gui_onTimer(object p0, object p1, object p2)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            bool bDbg = false;

            //mainActivity.var_common.loopCtrl1++;

            //
#if DEBUG
            if (bDbg)
            {
                qyFuncs_Droid.printTalkers("mainWnd.gui_onTimer enters");
            }
#endif

            //
#if false
                string str = string.Format("mainWnd: timer: {0} times", mainActivity.var_common.loopCtrl);
                qyFuncs.traceLog(str);
#endif


            //
            qyFuncs_Droid.mainWnd_OnTimer(mainActivity, ref mainActivity.var_common, 0);


            //
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (pProcInfo.bEnd_threadProc_isCli)
            {
                qyFuncs.showInfo_open(0, null, "gui_onTimer: threadProc_isCli_end is true, trigger to doClean");

                //
                this.gui_PostMessage(this, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_threadProc_isCli_end, 0);
            }

            //                
            mAdapter.mFragment_msg1.gui_onTimer(0, 0, 0);
            mAdapter.mFragment_imObj1.gui_onTimer(0, 0, 0);

            this.mAdapter.mFragment_me.gui_onTimer(null, null, null);




            //
            iErr = 0;
        errLabel:

            //
#if DEBUG
            if (bDbg)
            {
                qyFuncs_Droid.printTalkers("mainWnd.gui_onTimer leaves");
            }
#endif


            return iErr;
        }


        //
        //
        public static unsafe int mainWnd_setTimer()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

           mainActivity.setTimer(1000, mainActivity.gui_onTimer);

            //
            return 0;
        }

        public static unsafe void mainWnd_killTimer()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            mainActivity.killTimer("mainWnd_killTimer");
        }

        //
        unsafe int doTest()
        {
            return 0;
        }

        //
#if DEBUG
        void tmp_threadProc_test(object param)
        {
            int loopCtrl = 0;
            string str;

#if false
            for (;!this.bQuit_test ; )
            {
                int intervalInMs = 10;
                //
                Thread.Sleep(intervalInMs);
                str = "test" + loopCtrl;
                qyFuncs.showInfo_open(0, null, str);
                loopCtrl++;
            }
#endif

            return;
        }
#endif




        //
        private void OnAndroidEnvironmentUnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs unhandledExceptionEventArgs)
        {
            int ii = 0;
            //var newExc = new Java.Lang.Exception("OnAndroidEnvironmentUnhandledExceptionRaiser", unhandledExceptionEventArgs.Exception);
            //LogUnhandledException(newExc);
            return;
        }


        /*
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }*/
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

            //
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //
            int iErr = -1;

            //
            this.mTag = "mainActivity";

            //
            mHandler = new MyHandler(this);

            //
            qyFuncs_Droid.GuiShare_init(this);

            //
            qyFuncs_Droid.perpare_init();

            //
            string str;
            //
            str = ("mainWnd.OnCreate enters");
            qyFuncs.traceLog(str);


            //
            AndroidEnvironment.UnhandledExceptionRaiser += OnAndroidEnvironmentUnhandledExceptionRaiser;


            //
            this.var_common = new QY_MC_mainWndVar();
            //
            qy.qyFuncs.test();

            //
            qy.Droid.qyFuncs_Droid.InitInstance((IntPtr)null, 0);

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            pQyMc.gui.hMainWnd = this;



            //
#if DEBUG
            //    GuiShare.pf_testInMain(pQyMc);
#endif


            //
            mAdapter = new FragmentPagerAdapter_main(SupportFragmentManager);

            //
            bindViews();


            //
            qyFuncs_Droid.initVar_onCreate_mainFrame(IntPtr.Zero, this, ref this.var_common);


            //
            this.m_str_title = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_appName);

            //
            startActivity_login();

            //
#if DEBUG
            this.m_hThread_test = new MyThread(0, tmp_threadProc_test, 0);
            m_hThread_test.Start();
#endif

            //
            //
#if DEBUG
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            qyFuncs.traceLog("For debug: mainWnd, KeepScreenOn");
#endif
            //   bindNotificationIntent();
            //
            iErr = 0;


        errLabel:

            //
            str = string.Format("mainWnd.OnCreate leaves");
            qyFuncs.showInfo_open(0, null, str);

            return;
        }
        private void showTipNotification()
        {
            string title = "";
            string desc = "";
            getCallerDesc(ref title, ref desc);
            var builder = new NotificationCompat.Builder(this, "清扬通信")
                 .SetContentTitle(title)
             .SetContentText(desc)
           .SetSmallIcon(Resource.Drawable.notify_avCall)
               .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.notify_avCall))
           .SetTimeoutAfter(30000)
           .SetAutoCancel(true)
             .SetDeleteIntent(deletePendingIntent)
           .SetPriority((int)NotificationPriority.High)
           .SetVisibility((int)NotificationVisibility.Public)
                 .AddAction(builderAccept)
              //       .SetContentIntent(reorderToFrontIntent)
              .AddAction(builderCancel);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(notificationAvCallId, builder.Build());


        }
        private AcceptReceiver acceptReceiver = new AcceptReceiver();
        private CancelReceiver cancelReceiver = new CancelReceiver();
        private bool isActive;
        public bool canReceiveMessage = true;
        private int notificationAvCallId = 0;
        private PendingIntent acceptPendingIntent;
        private PendingIntent cancelPendingIntent;
        private PendingIntent deletePendingIntent;
        //   private PendingIntent reorderToFrontIntent;
        private NotificationCompat.Action builderAccept;
        private NotificationCompat.Action builderCancel;
        private void bindNotificationIntent()
        {
            var acceptIntent = new Intent(this, typeof(AcceptIntentService));
            acceptIntent.PutExtra("notificationId", notificationAvCallId);
            acceptPendingIntent = PendingIntent.GetService(this, 0, acceptIntent, PendingIntentFlags.Immutable);

            var cancelIntent = new Intent(this, typeof(CancelIntentService));
            cancelIntent.PutExtra("notificationId", notificationAvCallId);

            cancelPendingIntent = PendingIntent.GetService(this, 0, cancelIntent, PendingIntentFlags.Immutable);

            var deleteIntent = new Intent(this, typeof(NotificationDismissedReceiver));
            deleteIntent.PutExtra("notificationId", notificationAvCallId);

            deletePendingIntent = PendingIntent.GetBroadcast(this, 0, deleteIntent, PendingIntentFlags.Immutable);

            builderAccept = new NotificationCompat.Action.Builder(Resource.Drawable.icon, new String("接受"), acceptPendingIntent).Build();
            builderCancel = new NotificationCompat.Action.Builder(Resource.Drawable.icon, new String("取消"), cancelPendingIntent).Build();




            // reorderToFrontIntent = PendingIntent.GetActivity(this, 0, intent, 0);

        }

        //
        int requestCode_qmcLogin = 7829;

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == requestCode_qmcLogin)
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                if (!pQyMc.bLogon)
                {
                    string str;
                    str = string.Format("OnActivityResult: not login, so will quit. resultCode {0}", resultCode);
                    qyFuncs.traceLog(str);
                    qyFuncs.showInfo_open(0, null, str);
                    //
                    GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_login_canceled, 0);
                    return;
                }


            }
            if (requestCode == 1234)
            {
            }
        }
        int startActivity_login()
        {
            int iErr = -1;

            Intent intent;
            //intent = new Intent(Application.Context, typeof(Activity_qmcLogin));
            intent = new Intent(this, typeof(Activity_qmcLogin));
            //intent = new Intent(this, typeof(Activity_qmcLogin));

            //
            qyFuncs.traceLog("startActivity_login called");


            //intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
            try
            {
                StartActivityForResult(intent, requestCode_qmcLogin);
            }
            catch (Java.Lang.Exception e)
            {
                goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;

        }

        private void bindViews()
        {

            //
            m_layout_total = (RelativeLayout)FindViewById(Resource.Id.layout_total);
            //
            //
            txt_topbar = (TextView)FindViewById(Resource.Id.txt_topbar);
            txt_msg = (TextView)FindViewById(Resource.Id.txt_msg);
            txt_contact = (TextView)FindViewById(Resource.Id.txt_contact);
            txt_me = (TextView)FindViewById(Resource.Id.txt_me);

            //
            //            
            txt_msg.SetOnClickListener(this);
            txt_contact.SetOnClickListener(this);
            txt_me.SetOnClickListener(this);



            vPager = (ViewPager)FindViewById(Resource.Id.vpager);

            //
            //

            //
            vPager.Adapter = mAdapter;


            //vPager.SetCurrentItem(0,true);
            vPager.AddOnPageChangeListener(this);
            //
            txt_msg.PerformClick();
        }

        private void clearSelected()
        {
            txt_msg.Selected = false;
            txt_contact.Selected = false;
            txt_me.Selected = false;

        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.txt_msg:
                    clearSelected();
                    txt_msg.Selected = true;
                    vPager.SetCurrentItem(PAGE_ONE, false);
                    break;
                case Resource.Id.txt_contact:
                    clearSelected();
                    txt_contact.Selected = true;
                    vPager.SetCurrentItem(PAGE_TWO, false);
                    break;
                case Resource.Id.txt_me:
                    clearSelected();
                    txt_me.Selected = true;
                    vPager.SetCurrentItem(PAGE_THREE, false);

                    //
#if DEBUG
                    testSth();
#endif
                    //
                    break;
                default:
                    break;
            }
        }




        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            //
            System.Console.WriteLine("mainWnd.OnBackPressed");

            //
            this.gui_PostMessage_new(this, Consts.CONST_qyWndContentType_mainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);

            //doClean("mainWnd.OnBackPressed");

            //
            //base.OnBackPressed();
        }

        protected override void OnResume()
        {
            base.OnResume();
            isActive = true;
            //  IntentFilter acceptFilter = new IntentFilter("com.qycx.ACTION_ACCEPT_DONE");
            //   IntentFilter cancelFilter = new IntentFilter("com.qycx.ACTION_CANCEL_DONE");
            //  RegisterReceiver(acceptReceiver, acceptFilter);
            //  RegisterReceiver(cancelReceiver, cancelFilter);
            System.Console.WriteLine("mainWnd: onResume enters");

            //
            gui_notify_chk();

            //
            System.Console.WriteLine("mainWnd.OnResume leaves");

        }

        //
        int saveRecentContacts()
        {
            if (null != this.mAdapter.mFragment_msg1)
            {
                this.mAdapter.mFragment_msg1.saveRecentContacts();
            }

            //
            return 0;
        }


        //
        void doClean(string hint)
        {
            string str;

            if (hint == null) hint = "";

            str = string.Format("doClean {0}", hint);

            qyFuncs.showInfo_open(0, null, str + " enters");

            //
            if (g.g_pQyMc != null)
            {
                //
                saveRecentContacts();

                //
                qyFuncs_Droid.exitVar_onDestroy_mainFrame(IntPtr.Zero, this, ref var_common);
                //
                //qy.Droid.qyFuncs_Droid.ExitInstance((IntPtr)null);
            }

            //
#if DEBUG
            this.bQuit_test = true;
            if (this.m_hThread_test != null)
            {
                this.m_hThread_test.Join();
                this.m_hThread_test = null;
            }
#endif


            //
            qyFuncs.showInfo_open(0, null, str + " leaves");

            return;
        }

        protected override void OnPause()
        {
            //
            qyFuncs.traceLog("mainWnd: OnPause");

            //
            if (IsFinishing)
            {
                //
#if false
                qyFuncs.showInfo_open(0,null,"mainWnd.OnPause, isFinishing, enters");
                
                //
                qyFuncs_Droid.exitVar_onDestroy_mainFrame(IntPtr.Zero, this, ref var_common);
                //
                qy.qyFuncs.ExitInstance((IntPtr)null);

                //
                qyFuncs.showInfo_open(0, null, "mainWnd.OnPause, isFinishing, leaves");
#endif
                doClean("mainWnd.OnPause,isFinishing");
            }

            base.OnPause();
            isActive = false;

        }


        protected override void OnStop()
        {
            int i = 1;
            qyFuncs.traceLog("mainWnd.OnStop enters");

            //
            base.OnStop();

            //
            qyFuncs.traceLog("MainWnd.OnStop leaves");
        }

        protected override void OnDestroy()
        {
            qyFuncs.traceLog("MainActivity  OnDestroy");

            int i = 1;
            bool bDbg = false;
            string str;
            //  UnregisterReceiver(acceptReceiver);
            //  UnregisterReceiver(cancelReceiver);
            //
            str = string.Format("MainActivity.OnDestroy");
            qyFuncs.showInfo_open(0, null, str);

            //
            doClean("mainWnd.OnDestroy");

            //
            qy.Droid.qyFuncs_Droid.ExitInstance((IntPtr)null);

            //
            qyFuncs_Droid.prepare_exit();


            //
            if (m_layout_total != null)
            {
                m_layout_total.Dispose();
            }
            if (txt_topbar != null)
            {
                txt_topbar.Dispose();
            }
            if (txt_msg != null)
            {
                txt_msg.Dispose();
            }
            if (txt_contact != null)
            {
                txt_contact.Dispose();
            }
            if (txt_me != null)
            {
                txt_me.Dispose();
            }
            if (vPager != null)
            {
                //
                vPager.Adapter = null;
                //
                vPager.Dispose();
            }
            if (this.mAdapter != null)
            {
                this.mAdapter.doClean();
                
            }

            //
            if (null != m_progressHandler)
            {
                //  m_progressHandler.RemoveCallbacksAndMessages(null);
            }
            if (null != mHandler)
            {
                // mHandler.RemoveCallbacksAndMessages(null);
            }

            base.OnDestroy();


            qyFuncs_Droid.doGC("mainWnd.OnDestroy");

            System.Environment.Exit(0);
        }

        //
#if DEBUG
        public int testSth()
        {
            //DecodeAACAsyn kk = new DecodeAACAsyn(this);
            //kk.Start();

            return 0;
        }
#endif


        //
        void ViewPager.IOnPageChangeListener.OnPageScrollStateChanged(int state)
        {
            //throw new NotImplementedException();
            if (state == 2)
            {
                switch (vPager.CurrentItem)
                {
                    case PAGE_ONE:
                        clearSelected();
                        txt_msg.Selected = true;
                        break;
                    case PAGE_TWO:
                        clearSelected();
                        txt_contact.Selected = true;
                        break;
                    case PAGE_THREE:
                        clearSelected();
                        txt_me.Selected = true;
                        break;
                }


            }
        }
        void ViewPager.IOnPageChangeListener.OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            //throw new NotImplementedException();
        }

        void ViewPager.IOnPageChangeListener.OnPageSelected(int position)
        {
            //throw new NotImplementedException();
        }

        //
        //
        BottomSheetDialog m_bsd_askToQuit;
        bool m_bReadyToQuit = false;

        //
        ProgressDialog m_progressDialog;
        Tmp_progressThread m_progressThread;
        Tmp_progressHandler m_progressHandler;
        public int textureId;
        public int textureIw;
        public int textureIh;
         

        class Tmp_progressHandler : Handler
        {
            public ProgressDialog mOwner;

            public Tmp_progressHandler(ProgressDialog owner)
            {
                mOwner = owner;
            }

            public override void HandleMessage(Message msg)
            {
                base.HandleMessage(msg);
                //
                mOwner.IncrementProgressBy(2);
            }

        }



        class Tmp_progressThread : Thread
        {
            public Tmp_progressHandler m_progressHandler;
            public bool bQuit;

            public Tmp_progressThread(Tmp_progressHandler progressHandler)
            {
                m_progressHandler = progressHandler;
            }


            public override void Run()
            {
                //base.Run();
                for (; ; )
                {
                    if (bQuit) break;
                    //
                    Thread.Sleep(200);
                    //m_progressHandler.SendMessage(m_progressHandler.ObtainMessage());
                    m_progressHandler.mOwner.IncrementProgressBy(2);
                    //
                    if (m_progressHandler.mOwner.Progress == m_progressHandler.mOwner.Max)
                    {

                    }

                    //
                    continue;
                }

                return;
            }


        }


    }
}