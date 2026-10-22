
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Camera2;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Legacy.App;
using Java.Lang;
using Java.Util;
using Java.Util.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading;
using Android.Hardware.Camera2.Params;
using Android.Graphics;
using qyMessengerApp.Android;
using Action = System.Action;
using qy;
using qy.Droid;
using System;
//
using Com.Qycx.Gleslibrary;
using Android.Opengl;
using Android.Animation;
using Google.Android.Material.BottomSheet;
using AndroidX.RecyclerView.Widget;
using Android.Graphics.Drawables;
using static Android.Views.View;
using static Java.Util.Jar.Attributes;
using System.Runtime.InteropServices;
using Android.Renderscripts;
using AndroidX.ConstraintLayout.Widget;
using System.Security.Cryptography;
using MyGLE;




//
namespace qyMessengerApp.Droid
{
    public class Fragment_dlgTalk_av2 : Fragment, PopupWindow.IOnDismissListener
    {
        //private static String TAG = "Camera2VideoFragment";
        public static int REQUEST_VIDEO_PERMISSIONS = 1;
        public static string FRAGMENT_DIALOG = "dialog";

        //
        /*  public static string[] VIDEO_PERMISSIONS = {
              Manifest.Permission.Camera,
              Manifest.Permission.RecordAudio,
          };*/

        private bool isChangeCamera = false;
        bool canCloseCameraThread = false;
        public Activity_dlgTalk_av mOwner_wnd;

        //
        public MainHandler mMainHandler;

        //
        public bool isFirstOpenCamera;
        public int mLoopCtrl;

        //
        public MyTextureView mTextureView;
       // private dlgTalk_av_Renderer mRenderer;
       private Renderer myRenderer;

        //
        public T1_frm tf_peer;


        //
#if DEBUG
#if false
        public MyTextureView mTv_test;
        public TMP_stListener m_stListener_test;
        public SurfaceTexture m_st_test;
#endif
#endif

        //
        public ConstraintLayout bottomMenu;
        public ConstraintLayout topMenu;
        TextView m_btn_request;
        TextView m_tv_ioStatusOut;
        TextView m_tv_ioStatusIn;
        TextView nvr_channel_name;
        TextView nvr_control;
        TextView back_main_video;
        TextView up_bt;
        TextView down_bt;
        TextView left_bt;
        TextView right_bt;
        TextView long_focus_bt;
        TextView short_focus_bt;
        TextView m_btn_av;
        TextView meeting_launch;
        TextView meeting_account;
        TextView meeting_screen_info;
        TextView meeting_id;


        FrameLayout min_viewport;
        Drawable pull_downDrawable;
        Drawable pull_upDrawable;
        //
        ConstraintLayout left_menu_relative;
        public ConstraintLayout more_info_menu;
        ConstraintLayout right_menu_relative;
        public PopupWindow popupWindow;
        NvrListAdapter nvrListAdapter;
        public TextView btnLocalVideo;
        public TextView more_tv;
        public TextView btnChangeCamera;
        //
#if DEBUG
        public bool m_bTestDec;

        //
       

        //
        public struct TestDec
        {
            public ExtractMp3FramesTest extractMp3FramesTest;
            //
            public decObj_a_mediaCodec decObj_a_mediaCodec;
        };

        public TestDec m_testDec;


        //
        public struct TestEnc
        {
            public CameraToMpegTest cameraToMpegTest;
            //
            public EncodeAndMuxTest encodeAndMuxTest;
            //
            public encObj_mediaCodec encObj_mediaCodec;
        };

        //
        public TestEnc m_testEnc;

#endif

        //
        camera_RenderThread mCameraRenderThread;
        //
        public int mSensorOrientation;

        //
        public int m_st_iW;
        public int m_st_iH;

        //
        public int m_o_iW;
        public int m_o_iH;


        // These values are passed to us by the camera/render thread, and displayed in the UI.
        // We could also just peek at the values in the RenderThread object, but we'd need to
        // synchronize access carefully.
#if true
        private int mCameraPreviewWidth_unused, mCameraPreviewHeight_unused;
        private float mCameraPreviewFps_unused;
        //
        private int mRectWidth, mRectHeight;
        private int mZoomWidth, mZoomHeight;
        private int mRotateDeg;
#endif

        //
        //public static int m_test_textureId;

        //
        //
        public struct Tmp_autoSpeak
        {
            public bool m_bAutoSpeak;
            public int m_nTimes_autoSpeak;
        };

        Tmp_autoSpeak m_autoSpeak;




        //
        public static volatile bool sReleaseInCallback = true;

        //
        public int showBottomMenu()
        {
            ViewStates state_bottomMenu;

            //
            state_bottomMenu = ViewStates.Visible;
            if (this.bottomMenu.Visibility != state_bottomMenu)
            {
                this.bottomMenu.Visibility = state_bottomMenu;
            }

            return 0;
        }


        System.Action a;
        Handler h;
        public unsafe int mgui_onTimer(object p0, object p1, object p2)

        {
             h = new Handler();
             a = new Action(() =>
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                string strOut = string.Format("↑{0:N2}M", pProcInfo.status.netStat.ins.uiOutSpeedInKbps / 1024.0);
                string strIn = string.Format("↓{0:N2}M", pProcInfo.status.netStat.ins.uiInSpeedInKbps / 1024.0);
                m_tv_ioStatusOut.Text = strOut;
                m_tv_ioStatusIn.Text = strIn;
                chkIfAvAlive();
                chkAutoSpeak();
                initMinButtonStates();
                qyFuncs.traceLog("TAG","a");
                h.PostDelayed(a, 1000);

            });
            h.PostDelayed(a, 1000);



            return 0;
        }
        //
        //            public unsafe int gui_onTimer(object p0, object p1, object p2)
        //        {
        //            Activity_dlgTalk_av owner = (Activity_dlgTalk_av)this.Activity;
        //            if (owner == null) return -1;
        //            CCtxQyMc pQyMc = g.g_pQyMc;
        //            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

        //            bool bDbg = false;

        //            //
        //#if DEBUG
        //            bDbg = true;
        //#endif

        //            //
        //            mLoopCtrl++;


        //            //
        //#if DEBUG
        //            //int angle = ((WindowManager)owner.GetSystemService(Context.WindowService)).getDefaultDisplay().getRotation();
        //            //SurfaceOrientation surfaceOrientation = owner.WindowManager.DefaultDisplay.Rotation;
        //            //int angle = (int)surfaceOrientation;
        //            //if (surfaceOrientation==SurfaceOrientation.)

        //            //
        //            qyFuncs.traceLog("fg_dlgTalk_av2. gui_onTimer " + mLoopCtrl);

        //#endif


        //            //
        //            string strOut = string.Format("↑{0:N2}M", pProcInfo.status.netStat.ins.uiOutSpeedInKbps / 1024.0);
        //            string strIn = string.Format("↓{0:N2}M", pProcInfo.status.netStat.ins.uiInSpeedInKbps / 1024.0);

        //            //
        //#if DEBUG
        //            //
        //            qyFuncs.traceLog("fg_dlgTalk_av2.gui_onTimer.249");
        //#endif


        //            //str = string.Format("{0}", surfaceOrientation);
        //            //
        //            m_tv_ioStatusOut.Text = strOut;
        //            m_tv_ioStatusIn.Text = strIn;

        //            if (owner.mOwner_hTalk.m_var.is4k)
        //            {

        //                Toast.MakeText(owner, "不支持4K会议", ToastLength.Short).Show();
        //            }


        //            //
        //#if DEBUG
        //            //
        //            qyFuncs.traceLog("fg_dlgTalk_av2.gui_onTimer.268");
        //#endif



        //            //
        //            if (0 == (mLoopCtrl % 3))
        //            {
        //#if DEBUG
        //                //
        //                qyFuncs.traceLog("fg_dlgTalk_av2.gui_onTimer.278");
        //#endif

        //                //
        //                chkIfAvAlive();

        //#if DEBUG
        //                //
        //                qyFuncs.traceLog("fg_dlgTalk_av2.gui_onTimer.286");
        //#endif

        //                //
        //                chkAutoSpeak();

        //#if DEBUG
        //                //
        //                qyFuncs.traceLog("fg_dlgTalk_av2.gui_onTimer.294");
        //#endif


        //            }

        //            initMinButtonStates();

        //            //





        //            //
        //            //qyFuncs.traceLog("fg_dlgTalk_av onTimer");

        //            return 0;
        //        }


        //
        int m_nLostVideo = 0;

        //
        public int chkIfAvAlive()
        {
            //
            //
            do
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

                //
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, mOwner_wnd.m_index_taskInfo, "");
                if (pTaskInfo == null) break;
                QMC_taskData_common taskData = pTaskInfo.getTaskData();
                if (taskData == null) break;


                //
                uint dwTickCnt = qyFuncs.myGetTickCount(null);
                int iElapseInMs = (int)(dwTickCnt - taskData.tmpStatus.lastTickCnt_dec);
                if (Java.Lang.Math.Abs(iElapseInMs) > 3000)
                {
                    m_nLostVideo++;
                }
                else
                {
                    m_nLostVideo = 0;
                }

#if DEBUG
                string str;
                str = "" + mOwner_wnd.mOwner_hTalk.m_var.addr.idInfo.ui64Id + " tn " + mOwner_wnd.mOwner_hTalk.m_var.addr.uiTranNo_shadow;
                str += " m_nLostVideo " + m_nLostVideo;
                if (Consts.CONST_Status_Window_Function_Open == 0)
                {

                    qyFuncs.traceLog("chkIfAvAlive called: " + str);

                }
#endif
                //
                Activity_dlgTalk_av owner = this.mOwner_wnd;
                Activity_dlgTalk talk = owner.mOwner_hTalk;
                DLG_TALK_var m_var = talk.m_var;

                //
                
                if (owner.m_nErrs_hTalk_av_not_matched > 130 || m_nLostVideo > pProcInfo.cfg.usMaxN_lostVideo)
                {

                    //
                    m_var.av.bAutoCancel_lostVideo = true;
                    m_var.av.dwTickCnt_autoCancel_lostVideo = qyFuncs.myGetTickCount(null);
                    m_var.av.bDoneOnce_autoCancel_lostVideo = false;

                    //
                    qyFuncs.showInfo_open(0, "", "m_nLostVideo > 10, autoCancel");

                    //
                    owner.doAutoCancel();
                }


            } while (false);

            //
            return 0;
        }


        //
        int chkAutoSpeak()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            bool bDbg = false;
            string str;

#if DEBUG
            bDbg = true;
#endif

            //
            if (pProcInfo.av.m_bInSleepMode)
            {
                qyFuncs.showInfo_open(0, "", "chkAutoSpeak: bInSleep true, do nothing");
                return 0;
            }

            //
            Activity_dlgTalk dlgTalk = mOwner_wnd.mOwner_hTalk;
            if (m_autoSpeak.m_bAutoSpeak)
            {
                if (m_autoSpeak.m_nTimes_autoSpeak > 30)
                {
                    m_autoSpeak = new Tmp_autoSpeak();
                    //                        
                    dlgTalk.m_var.av.m_bState_canSpeak = false;
                }
                else
                {
                    m_autoSpeak.m_nTimes_autoSpeak++;


                    bool canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);
                    if (!canSpeak)
                    {
                        //
                        if (bDbg)
                        {
                            str = "do_request: m_nTimes_autoSpeak " + m_autoSpeak.m_nTimes_autoSpeak;
                            qyFuncs.traceLog(str);
                            //   qyFuncs.showNotification_open(0, 0, 0, str);
                        }
                        //
                        do_request(true);
                    }
                    else
                    {
                        //
                        if (bDbg)
                        {
                            str = "canSpeak true, clear m_autoSpeak";
                            qyFuncs.traceLog(str);

                            //    qyFuncs.showNotification_open(0, 0, 0, str);
                        }

                        //
                        m_autoSpeak = new Tmp_autoSpeak();
                    }

                }
            }
            else
            {
                if (dlgTalk.m_var.av.m_bState_canSpeak)
                {
                    bool canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);
                    if (!canSpeak)
                    {
                        m_autoSpeak.m_bAutoSpeak = true;

                    }

                    //
                    //
                    if (bDbg)
                    {
                        str = "";
                        if (!canSpeak)
                        {
                            str += "timer: canSpeak " + canSpeak;
                            str += ". to recover. ";
                        }
                        //
                        if (str.Length > 0)
                        {
                            qyFuncs.traceLog(str);

                            //   qyFuncs.showNotification_open(0, 0, 0, str);
                        }
                    }
                }
                else
                {
                    if (this.mOwner_wnd.mOwner_hTalk.get_pm_var().addr.uiObjType != Consts.CONST_objType_imGrp)
                    {
                        bool canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);
                        if (!canSpeak)
                        {
                            do_request(true);
                        }
                    }

                }
            }

            return 0;

        }

        //
        public int waitTextureViewAvailable(object p0, object p1, object p2)
        {
            int iErr = -1;
            Activity_dlgTalk_av owner = (Activity_dlgTalk_av)this.Activity;
            Activity_dlgTalk hTalk = owner.mOwner_hTalk;

            string str;

            str = string.Format("fg_dlgTalk_av2, waitTextureViewAvailable enters");
            qyFuncs.showInfo_open(0, null, str);

            //
            if (owner.m_bGot_closeWndMsg)
            {
                qyFuncs.showInfo_open(0, "", "fg_dlgTalk_av,waitTextureViewAvail, bGot_closeWndMsg is true");

                return 0;
            }

            //
            if (!mTextureView.IsAvailable)
            {
                qyFuncs.showInfo_open(0, "", "fg_dlgTalk_av,waitTextureViewAvail, textureView.isAvail is false");

                return 0;
            }
            //
            if (hTalk.m_var.m_share_eglContext == null)
            {
                qyFuncs.showInfo_open(0, "", "fg_dlgTalk_av,waitTextureViewAvail, talk.m_share_eglContext is null");
                return 0;
            }

            //
          //  return 0;


            if (hTalk.m_var.av.taskInfo.bTaskExists)
            {
                if (!hTalk.m_var.m_bNeedResumeTaskAv)
                {
                   owner.pauseTaskAv("waitTexViewAvail");

                    //
#if DEBUG

#endif
                }

                if (hTalk.m_var.m_bNeedResumeTaskAv)
                {
                    //
                   owner.resumeTaskAv("waitTexViewAvail");
                    iErr = 0; goto errLabel;
                }

                //
                iErr = 0; goto errLabel;
            }


            //
            if (hTalk.m_var.av.bNeedDoTaskAv)
            {
                //
                hTalk.m_var.av.bNeedDoTaskAv = false;

                //
                int iTaskId = 0;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, mOwner_wnd.m_index_taskInfo, "");
                if (pTaskInfo != null)
                {
                    iTaskId = pTaskInfo.var.iTaskId;
                }



                //
                doTask_av(mOwner_wnd.m_iCmd, iTaskId);


                //
#if DEBUG
                //

                //

#endif

            }

            //
            iErr = 0;

        errLabel:

            str = string.Format("fg_dlgTalk_av2, waitTextureViewAvailable leaves " + iErr);
            qyFuncs.showInfo_open(0, null, str);

            //
            return iErr;
        }

        //


        public unsafe int doTask_av(int iCmd, int iTaskId)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            bool bDbg = false;
            string str;

            //
            bool b3D = false;
            //  2017/07/17
            bool ucbAvConsole = false;
            Activity_dlgTalk_av owner = (Activity_dlgTalk_av)this.Activity;

            //
#if DEBUG
            if (this.m_bTestDec)
            {
                //
                //
                bool bTestDec = false;
                //bTestDec = true;

                if (bTestDec)
                {
                    if (false)
                    {
                        this.m_testDec.extractMp3FramesTest = new ExtractMp3FramesTest();

                        this.m_testDec.extractMp3FramesTest.testExtractMpegFrames(this.mOwner_wnd);
                    }
                    else
                    {
                        this.m_testDec.decObj_a_mediaCodec = new decObj_a_mediaCodec();
                        this.m_testDec.decObj_a_mediaCodec.m_bTest = true;
                        this.m_testDec.decObj_a_mediaCodec.testExtractMpegFrames(this.mOwner_wnd);
                    }

                    return 0;
                }
            }

            //
            bool bTestEnc = false;
            //bTestEnc=true;
            //
            if (bTestEnc)
            {
                if (false)
                {
                    this.m_testEnc.cameraToMpegTest = new CameraToMpegTest();
                    //
                    this.m_testEnc.cameraToMpegTest.testEncodeCameraToMp4();
                }
                else
                {
#if false
                    this.m_testEnc.encodeAndMuxTest = new EncodeAndMuxTest();
                    //
                    this.m_testEnc.encodeAndMuxTest.testEncodeVideoToMp4();
#else

                    //
                    this.m_testEnc.encObj_mediaCodec = new encObj_mediaCodec();
                    this.m_testEnc.encObj_mediaCodec.testEncodeVideoToMp4();
#endif

                }

                return 0;
            }

#endif
            //
            if (!mTextureView.IsAvailable)
            {
                str = string.Format("fg_dlgTalk_av.doTask_av failed, textureView.isAvail is false");
                qyFuncs.showInfo_open(0, null, str);

                goto errLabel;
            }


            //
            switch (iCmd)
            {
                case tmpResource.IDC_BUTTON_av:
                    {
                        qyFuncs_Droid.doCmd_startAvCall(null, mOwner_wnd.mOwner_hTalk, 0, false, false, null);
                    }
                    break;
                case tmpResource.IDC_BUTTON_av_accept:
                    {
                        int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
                        QMC_TASK_INFO pTaskInfo = pProcInfo.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, index_taskInfo, "doTask_av");
                        if (pTaskInfo == null) goto errLabel;

                        PARAM_dlgTalk_procTask param = new PARAM_dlgTalk_procTask();
                        //memset(&param, 0, sizeof(param));
                        param.bManualCmd = true;        //  2017/02/12
                                                        //pFuncs->pf_dlgTalk_procTask1(  hDlgTalk,  pDlgTalkVar->guiData.av.iTaskId,  CONST_imOp_dual_permit,  NULL,  &param  );  
                        qyFuncs_Droid.dlgTalk_procTask1(mOwner_wnd.mOwner_hTalk, pTaskInfo.var.iTaskId, Consts.CONST_imOp_dual_permit, null, ref param);

                        //qyFuncs.traceLog("not finished");

                        //
                        Activity_dlgTalk dlgTalk = (Activity_dlgTalk)mOwner_wnd.mOwner_hTalk;
                        if (dlgTalk != null)
                        {
                            //


                            //
                            if (dlgTalk.m_var.av.bAutoCancel_lostVideo)
                            {
                                dlgTalk.m_var.av.bAutoCancel_lostVideo = false;

                                //
                                if (dlgTalk.m_var.av.m_bState_canSpeak)
                                {
                                    m_autoSpeak.m_bAutoSpeak = true;
                                    m_autoSpeak.m_nTimes_autoSpeak++;

                                    //
                                    do_request(true);
                                }
                            }
                            //
                            if (!dlgTalk.m_var.av.m_bState_canSpeak)
                            {
                                //
                                do_request(false);
                            }
                        }

                    }
                    break;
                default:

                    qyFuncs.traceLog("not finished, unknown iCmd");
                    break;

            }


            //
            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public int updateRequestButtonStatus()
        {
            Activity_dlgTalk dlgTalk = mOwner_wnd.mOwner_hTalk;

            //this.m_btn_request
            bool bState_canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);


            CCtxQyMc pQyMc = g.g_pQyMc;

            //

            if (bState_canSpeak)
            {
                m_btn_request.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_stopSpeaking);
                m_btn_request.SetTextColor(Resources.GetColor(Resource.Color.av_button_active_text_color));
                m_btn_request.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_active_color));
            }
            else
            {
                m_btn_request.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_requestToSpeak);
                m_btn_request.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_normal_color));
                m_btn_request.SetTextColor(Resources.GetColor(Resource.Color.av_button_normal_text_color));

            }
            return 0;
        }




        //
        public static unsafe int initVideoCapDev(object hTalk, int iWndContentType, string displayName_selected, ref BITMAPINFOHEADER pBih_suggested, CAP_procInfo_bmpCommon pCapBmp)

        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            string str;

            //
#if DEBUG
            //
            qyFuncs.traceLog("initVideoCapDev enters"); ;

            //
            if (pProcInfo.cfg.mSpecialDbg.bNo_videoCap)
            {
                qyFuncs.showInfo_open(0, null, "For test: fg_dlgTalk_av2.initVideoCapDev return -1");
                return -1;
            }
#endif

            //
            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;
            if (!GuiShare.pf_IsWindow(dlgTalk.m_hTool_dlgTalk_av)) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;
            if (dlgTalk_av.m_fg_dlgTalk_av2 == null) return -1;
            Fragment_dlgTalk_av2 fg = dlgTalk_av.m_fg_dlgTalk_av2;

            //
            qyFuncs.traceLog("initVideoCapDev.789"); ;

            //
            if (!fg.mTextureView.IsAvailable) return -1;

            //
            int w, h;
            //w = fg.mTextureView.Width;
            //h = fg.mTextureView.Height;
            w = pBih_suggested.biWidth;
            h = pBih_suggested.biHeight;

            //
#if false
            fg.startBackgroundThread();       
            fg.openCamera(w, h);
#endif

            //
            if (null == dlgTalk.m_var.m_share_eglContext) goto errLabel;

            //
            qyFuncs.traceLog("initVideoCapDev.811"); ;

            //
            SurfaceOrientation surfaceOrientation = dlgTalk_av.WindowManager.DefaultDisplay.Rotation;

            //
            bool bFront = true;
            if (m_var.av.taskInfo.bTaskExists) bFront = !m_var.av.taskInfo.bUse_backCamera;
            //
#if DEBUG
            //
#if false
            bFront = false;
            qyFuncs.showInfo_open(0,null,"for test: set bFront false");
#endif
            //
#endif
            //
            pCapBmp.m_bFront = bFront;

            //
            //
            qyFuncs.traceLog("initVideoCapDev.833,before camera_RenderThread"); ;

            //
            fg.mCameraRenderThread = new camera_RenderThread(fg.mMainHandler, (EGLContext)dlgTalk.m_var.m_share_eglContext, pCapBmp.iIndex_sharedObj, bFront, surfaceOrientation);
            fg.mCameraRenderThread.Start();
            //
            if (0 != fg.mCameraRenderThread.waitUntilReady())
            {
                goto errLabel;
            }
            //
            fg.mSensorOrientation = fg.mCameraRenderThread.myCamera.mSensorOrientation;
            //
            //pCapBmp.mSensorOrientation = fg.mSensorOrientation;


            //
            //
            qyFuncs.traceLog("initVideoCapDev.851"); ;


            //fg.me_tex 
            //
            SurfaceTexture st = null;// fg.m_st_test;
            //st=new SurfaceTexture(CTalkDraw.me_texId);
            st = CTalkDraw.me_st;
            if (st == null)
            {
                qyFuncs.showInfo_open(0, "", "initVideoCapDev failed, me_st is null");
                goto errLabel;
            }

            //
            fg.m_st_iW = fg.mCameraRenderThread.mCameraPreviewWidth;
            fg.m_st_iH = fg.mCameraRenderThread.mCameraPreviewHeight;

            //
#if false
            switch (fg.mSensorOrientation)
            {
                case 90:
                case 270:
                    fg.m_o_iW = fg.mCameraRenderThread.mCameraPreviewHeight;
                    fg.m_o_iH = fg.mCameraRenderThread.mCameraPreviewWidth;
                    break;
                default:
                    fg.m_o_iW = fg.mCameraRenderThread.mCameraPreviewWidth;
                    fg.m_o_iH = fg.mCameraRenderThread.mCameraPreviewHeight;
                    break;
            }
#endif
            //
            int angle_z = 0;
            qyFuncs_Droid.get_tex_o_info(fg.mSensorOrientation, surfaceOrientation, fg.mCameraRenderThread.mCameraPreviewWidth, fg.mCameraRenderThread.mCameraPreviewHeight, ref angle_z, ref fg.m_o_iW, ref fg.m_o_iH);

            //
#if DEBUG

#endif

            //
            st.SetDefaultBufferSize(fg.m_st_iW, fg.m_st_iH);

            if (bFront && fg.isFirstOpenCamera)
            {
                fg.mCameraRenderThread.mHandler.sendSurfaceAvailable(st, true);

            }
            else
            {

                fg.mCameraRenderThread.mHandler.sendSurfaceAvailable(st, false);

                fg.isFirstOpenCamera = false;
            }

            //
            qyFuncs.traceLog("initVideoCapDev.910"); ;

            //
            iErr = 0;

        errLabel:

            //
            qyFuncs.traceLog("initVideoCapDev leaves"); ;

            //
            return iErr;
        }

        public static unsafe int exitVideoCapDev(object hTalk, int iWndContentType)
        {
            int iErr = -1;

            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (null == dlgTalk.m_hTool_dlgTalk_av) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;
            if (dlgTalk_av.m_fg_dlgTalk_av2 == null) return -1;
            Fragment_dlgTalk_av2 fg = dlgTalk_av.m_fg_dlgTalk_av2;


#if false
            fg.closeCamera();
            fg.stopBackgroundThread();
#endif


            if (fg.mCameraRenderThread != null)
            {
                camera_RenderThread.camera_RenderHandler rh = fg.mCameraRenderThread.getHandler();
                if (rh != null)
                {
                    rh.sendShutdown();
                }

                try
                {
                    fg.mCameraRenderThread.Join();
                }
                catch (UnsupportedOperationException ie)
                {

                }
                fg.mCameraRenderThread = null;

            }





            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe bool bGetCapturePara(object hTalk, int iWndContentType, ref QY_VIDEO_HEADER pVh_org)
        {
            bool bRet = false;

            if (iWndContentType != Consts.CONST_qyWndContentType_talker) return false;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            if (!GuiShare.pf_IsWindow(dlgTalk.m_hTool_dlgTalk_av)) return false;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;
            if (dlgTalk_av.m_fg_dlgTalk_av2 == null) return false;
            Fragment_dlgTalk_av2 fg = dlgTalk_av.m_fg_dlgTalk_av2;
#if false
            if (fg.mImageReader == null) return false;
#endif

            //
            BITMAPINFOHEADER bih_org;
            qyFuncs.mymemset((IntPtr)(byte*)&bih_org, 0, sizeof(BITMAPINFOHEADER));
            /*
            switch (fg.mSensorOrientation)
            {
                case 90:
                case 270:
                    bih_org.biWidth = fg.mImageReader.Height;
                    bih_org.biHeight = fg.mImageReader.Width;
                    break;
                default:
                    bih_org.biWidth = fg.mImageReader.Width;
                    bih_org.biHeight = fg.mImageReader.Height;
                    break;
            }
            */
#if true
            if (fg.mCameraRenderThread == null) goto errLabel;

            bih_org.biWidth = fg.m_o_iW;
            bih_org.biHeight = fg.m_o_iH;
            //           
            bih_org.biCompression = Consts.CONST_fourcc_I420;
#endif
            pVh_org.bih = bih_org;


            //
            bRet = true;
        errLabel:

            return bRet;
        }

        public static unsafe int startCapture(object hTalk, int iWndContentType)
        {
            return 0;
        }

        public static unsafe int stopCapture(object hTalk, int iWndContentType)
        {
            return 0;
        }



        //
        public static Fragment_dlgTalk_av2 NewInstance()
        {
            return new Fragment_dlgTalk_av2();
        }

        public override unsafe void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            isFirstOpenCamera = true;
            mOwner_wnd = (Activity_dlgTalk_av)this.Activity;



            // Create your fragment here
#if false
            mOnImageAvailableListener = new ImageAvailableListener(this);
            //mSurfaceTextureListener = new dlgTalk_av_SurfaceTextureListener(this);
            mStateCallback = new MyCameraStateCallback(this);
#endif

            //
#if DEBUG
            this.m_bTestDec = true;
            qyFuncs.showInfo_open(0, null, "For test: m_bTestDesc set to true");

            //
            if (m_bTestDec)
            {
#if false
                m_testDec.mTimeAnimator = new TimeAnimator();



                //m_testDec.mCodecWrapper = new MediaCodecWrapper();
                m_testDec.mExtractor = new MediaExtractor();
#endif

            }
#endif

        }


        private unsafe void getMeetingInfo()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            int iIndex_taskInfo = -1;
            ref DLG_TALK_var m_var = ref mOwner_wnd.mOwner_hTalk.m_var;
            if (m_var.av.taskInfo.bTaskExists)
            {
                iIndex_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, m_var.av.taskInfo.iTaskId);
                QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, iIndex_taskInfo, "");
                if (pTaskInfo != null)
                {
                    QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                    if (pTaskData.uiType == Consts.CONST_taskDataType_conf)
                    {
                        QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;

                        //  pTc.grp.idInfo_initiator;//发起人id
                        //qyFuncs.getTalkerDesc(pMisCnt, pActiveMem_from.avStream.idInfo, pDesc, Consts.cntof_desc, null, 0);

                        TMP_desc tmpDesc;
                        char* desc = tmpDesc.desc;


                        qyFuncs.getTalkerDesc(pMisCnt, m_var.av.taskInfo.idInfo_initiator, desc, Consts.cntof_desc, null, 0);
                        qyFuncs.traceLog("会议召集人:" + new string(desc));
                        string meeting_launch_people_str = "会议召集人:" + new string(desc);
                        meeting_launch.Text = meeting_launch_people_str;
                        qyFuncs.traceLog("会议召集人ID:" + m_var.av.taskInfo.idInfo_initiator.ui64Id);
                        if (m_var.av.taskInfo.idInfo_initiator.ui64Id == 0)
                        {
                            meeting_id.Visibility = ViewStates.Invisible;
                            meeting_launch.Visibility = ViewStates.Invisible;
                            back_main_video.Visibility = ViewStates.Invisible;
                            meeting_screen_info.Visibility = ViewStates.Invisible;
                        }
                        string meeting_id_str = "会议召集人ID:" + m_var.av.taskInfo.idInfo_initiator.ui64Id;
                        meeting_id.Text = meeting_id_str;
                        qyFuncs.getTalkerDesc(pMisCnt, pMisCnt.idInfo, desc, Consts.cntof_desc, null, 0);


                        qyFuncs.traceLog("当前登录账号:" + new string(desc));
                        string meeting_account_str = "当前登录账号:" + new string(desc);
                        meeting_account.Text = meeting_account_str;
                        fixed (MIS_MSG_TASK* pMsgTask = &pTc.msgU.task)

                        {
                            if (pMsgTask->uiType == Consts.CONST_misMsgType_task)
                            {
                                IM_CONTENTU* pContent = qyFuncs.M_getMsgContent(pMsgTask->uiType, pMsgTask->ucFlg, &pMsgTask->data);
                                if (null != pContent)
                                {
                                    ref IM_CONTENTU tmp_pContent = ref *pContent;
                                    //
                                    if (pContent->uiType == Consts.CONST_imCommType_transferAvInfo)
                                    {
                                        int width = pContent->transferAvInfo.confMosaicTranInfo_video.video.vh_decompress.bih.biWidth;
                                        int height = pContent->transferAvInfo.confMosaicTranInfo_video.video.vh_decompress.bih.biHeight;
                                        string meeting_screen_info_str = "分辨率:" + width + "x" + height;
                                        meeting_screen_info.Text = meeting_screen_info_str;
                                        qyFuncs.traceLog("px py 分辨率:" + width + "--" + height);
                                    }
                                }
                            }

                        }

                    }

                }

            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = null;
            CCtxQyMc pQyMc = g.g_pQyMc;
            view = inflater.Inflate(Resource.Layout.layout_fg_dlgTalk_av, container, false);

            //
            this.mMainHandler = new MainHandler(this);


            //
            mTextureView = (MyTextureView)view.FindViewById(Resource.Id.texture);
            //
            bottomMenu = (ConstraintLayout)view.FindViewById(Resource.Id.bottomMenu);
            topMenu = (ConstraintLayout)view.FindViewById(Resource.Id.topMenu);
            more_info_menu = (ConstraintLayout)view.FindViewById(Resource.Id.more_info_menu);
            //
            m_btn_request = (TextView)view.FindViewById(Resource.Id.btn_request);
            m_tv_ioStatusOut = (TextView)view.FindViewById(Resource.Id.tv_ioStatus_out);
            m_tv_ioStatusIn = (TextView)view.FindViewById(Resource.Id.tv_ioStatus_in);
            // m_btn_more = (Button)view.FindViewById(Resource.Id.btn_more);
            up_bt = (TextView)view.FindViewById(Resource.Id.up_bt);
            down_bt = (TextView)view.FindViewById(Resource.Id.down_bt);
            left_bt = (TextView)view.FindViewById(Resource.Id.left_bt);
            right_bt = (TextView)view.FindViewById(Resource.Id.right_bt);
            short_focus_bt = (TextView)view.FindViewById(Resource.Id.short_focus_bt);
            long_focus_bt = (TextView)view.FindViewById(Resource.Id.long_focus_bt);
            m_btn_av = (TextView)view.FindViewById(Resource.Id.btn_av);
            nvr_channel_name = (TextView)view.FindViewById(Resource.Id.nvr_channel_name_title);
            nvr_control = (TextView)view.FindViewById(Resource.Id.nvr_control);
            back_main_video = (TextView)view.FindViewById(Resource.Id.back_main_video);
            nvr_control.Click += M_Nvr_control_Click;
            back_main_video.Click += M_Back_Main_Video_Click;
            min_viewport = (FrameLayout)view.FindViewById(Resource.Id.min_viewport);
            btnChangeCamera = (TextView)view.FindViewById(Resource.Id.changeCamera);
            btnChangeCamera.Click += BtnChangeCamera_Click;
            btnLocalVideo = (TextView)view.FindViewById(Resource.Id.localVideo);
            more_tv = (TextView)view.FindViewById(Resource.Id.more_tv);
            meeting_launch = (TextView)view.FindViewById(Resource.Id.meeting_launch);
            meeting_account = (TextView)view.FindViewById(Resource.Id.meeting_account);
            meeting_screen_info = (TextView)view.FindViewById(Resource.Id.meeting_screen_info);
            meeting_id = (TextView)view.FindViewById(Resource.Id.meeting_id);
            btnLocalVideo.Click += BtnLocalVideo_Click; ;
            more_tv.Click += BtnMoreTV_Click;
            btnChangeCamera.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_changeCamera);
            btnLocalVideo.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_localVideo);

            left_menu_relative = (ConstraintLayout)view.FindViewById(Resource.Id.left_menu_relative);
            right_menu_relative = (ConstraintLayout)view.FindViewById(Resource.Id.right_menu_relative);


            //mRenderer = new dlgTalk_av_Renderer(this, Consts.CONST_iFgType_dlgTalk_av2);
            //mRenderer.init();
           //mRenderer.Start();
            myRenderer = new Renderer(Activity, Consts.CONST_iFgType_dlgTalk_av2);
          
            myRenderer.Start();

            Action runnable = null;
            runnable = new Action(() =>
            {


            });
            mOwner_wnd.menuInScreen = false;
            min_viewport.Post(mOwner_wnd.runnableShowBottomBar);
            mOwner_wnd.bottomBarHandler.PostDelayed(mOwner_wnd.runnableHideBottomBar, 8000);
            mTextureView.SurfaceTextureListener = myRenderer;
            mTextureView.Touch += Texture_View_OnTouchListener;
            //
#if DEBUG
#if false
            mTv_test = (MyTextureView)view.FindViewById(Resource.Id.texture_test);
            //
            m_stListener_test = new TMP_stListener(this);
            mTv_test.SurfaceTextureListener = m_stListener_test;
#endif
#endif
            nvr_channel_name.Click += Channel_num_tv_ClickListener;
            up_bt.Touch += Up_bt_OnTouchListener;
            down_bt.Touch += Down_bt_OnTouchListener;
            left_bt.Touch += Left_bt_OnTouchListener;
            right_bt.Touch += Right_bt_OnTouchListener;
            short_focus_bt.Touch += Short_focus_bt_OnTouchListener;
            long_focus_bt.Touch += Long_focus_OnTouchListener;

            m_btn_request.Click += M_btn_request_Click;
            m_btn_av.Click += M_btn_av_Click;
            //  m_btn_more.Click += M_btn_more_Click;
            min_viewport.Click += M_min_viewport_Click;
            //

            if (pQyMc == null) goto errLabel;

            pull_downDrawable = mOwner_wnd.GetDrawable(Resource.Drawable.pull_down);
            pull_downDrawable.SetBounds(0, 0, pull_downDrawable.IntrinsicWidth, pull_downDrawable.IntrinsicHeight);

            pull_upDrawable = mOwner_wnd.GetDrawable(Resource.Drawable.pull_up);
            pull_upDrawable.SetBounds(0, 0, pull_upDrawable.IntrinsicWidth, pull_upDrawable.IntrinsicHeight);


            //
            m_btn_request.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_requestToSpeak);
            m_btn_av.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_cancel_AV);//"Cancel";

            //
            //
            Activity_dlgTalk hTalk = (mOwner_wnd.mOwner_hTalk);
            DLG_TALK_var m_var = hTalk.get_pm_var();

            if (m_var.addr.uiObjType != Consts.CONST_objType_imGrp)
            {
                m_btn_request.Visibility = ViewStates.Invisible;
            }
            this.m_autoSpeak = new Tmp_autoSpeak();
            
        errLabel:
            //
            return view;
        }

        private void M_Back_Main_Video_Click(object sender, EventArgs e)
        {
            resetCurrentVideoChanel();
        }

        private void Channel_num_tv_ClickListener(object sender, EventArgs e)
        {
            qyFuncs.traceLog("Channel_num_tv_ClickListener");
            if (popupWindow.IsShowing)
            {

                popupWindow.Dismiss();
            }
            else
            {
                popupWindow.ShowAsDropDown(nvr_channel_name);
                nvr_channel_name.SetCompoundDrawables(null, null, pull_downDrawable, null);
            }

        }


        private void Long_focus_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                long_focus_bt.Pressed = true;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_ZOOM_IN, Consts.CONST_mouseStatus_lBtnDown);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                long_focus_bt.Pressed = false;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_ZOOM_IN, Consts.CONST_mouseStatus_lBtnUp);

            }
        }

        private void Short_focus_bt_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                short_focus_bt.Pressed = true;


                dlg_YTBtn_remote(Consts.IDC_BUTTON_ZOOM_OUT, Consts.CONST_mouseStatus_lBtnDown);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                short_focus_bt.Pressed = false;


                dlg_YTBtn_remote(Consts.IDC_BUTTON_ZOOM_OUT, Consts.CONST_mouseStatus_lBtnUp);

            }
        }

        private void Right_bt_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                right_bt.Pressed = true;


                dlg_YTBtn_remote(Consts.IDC_BUTTON_RIGHT, Consts.CONST_mouseStatus_lBtnDown);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                right_bt.Pressed = false;
                dlg_YTBtn_remote(Consts.IDC_BUTTON_RIGHT, Consts.CONST_mouseStatus_lBtnUp);

            }
        }

        private void Left_bt_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                left_bt.Pressed = true;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_LEFT, Consts.CONST_mouseStatus_lBtnDown);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                left_bt.Pressed = false;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_LEFT, Consts.CONST_mouseStatus_lBtnUp);

            }
        }
        private void Up_bt_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                up_bt.Pressed = true;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_UP, Consts.CONST_mouseStatus_lBtnDown);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                up_bt.Pressed = false;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_UP, Consts.CONST_mouseStatus_lBtnUp);

            }


        }
        private void Down_bt_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                down_bt.Pressed = true;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_DOWN, Consts.CONST_mouseStatus_lBtnDown);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                down_bt.Pressed = false;

                dlg_YTBtn_remote(Consts.IDC_BUTTON_DOWN, Consts.CONST_mouseStatus_lBtnUp);

            }
        }

        private void Texture_View_OnTouchListener(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)

            {

                Rect centerSurfaceViewPortRect = (Rect)mOwner_wnd.mOwner_hTalk.m_var.av.hDc_av_rect;
                //
                if (centerSurfaceViewPortRect == null)
                {
                    return;
                }
                //
                if (centerSurfaceViewPortRect.Contains((int)(e.Event.GetX()), (int)(e.Event.GetY()) - statusBarHeight))
                {

                    int centerViewPortWidth = centerSurfaceViewPortRect.Right - centerSurfaceViewPortRect.Left;
                    int centerViewPortHeight = centerSurfaceViewPortRect.Bottom - centerSurfaceViewPortRect.Top;

                    qyFuncs.traceLog("zzstar 发送原始坐标" + e.Event.GetX() + "- " + e.Event.GetY());
                    int transformVideoX = (int)((e.Event.GetX() -
                        centerSurfaceViewPortRect.Left) / centerViewPortWidth * 1920);
                    int transformVideoY = (int)((e.Event.GetY() -
                        centerSurfaceViewPortRect.Top - statusBarHeight) / centerViewPortHeight * 1080);

                    ConfLayoutParam layoutParam = new ConfLayoutParam();
                    layoutParam.phoneEnlargeParam.x = (short)transformVideoX;
                    layoutParam.phoneEnlargeParam.y = (short)transformVideoY;
                    qyFuncs.traceLog("zzstar 发送视频映射坐标：" + layoutParam.phoneEnlargeParam.x + "- " + layoutParam.phoneEnlargeParam.y);

                    //组名 
                    QY_MESSENGER_ID idInfo_imGrp_related;
                    idInfo_imGrp_related.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.addr.idInfo.ui64Id;

                    // 我 
                    QY_MESSENGER_ID idInfo_requester;
                    idInfo_requester.ui64Id = 0;

                    //mcu 的
                    QY_MESSENGER_ID idInfo_to;
                    idInfo_to.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.av.taskInfo.idInfo_starter.ui64Id;

                    qyFuncs_Droid.sendConfLayout(mOwner_wnd.mOwner_hTalk, 0, 0, Consts.CONST_imOp_phone_enlargeImg, ref layoutParam, idInfo_imGrp_related, idInfo_requester, idInfo_to, "");

                }

            }

        }



        private void M_min_viewport_Click(object sender, EventArgs e)
        {
            resetCurrentVideoChanel();

        }

        public void resetCurrentVideoChanel()
        {

            ConfLayoutParam layoutParam = new ConfLayoutParam();
            layoutParam.phoneEnlargeParam.x = 0;
            layoutParam.phoneEnlargeParam.y = 0;

            //组名 
            QY_MESSENGER_ID idInfo_imGrp_related;
            idInfo_imGrp_related.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.addr.idInfo.ui64Id;

            // 我 
            QY_MESSENGER_ID idInfo_requester;
            idInfo_requester.ui64Id = 0;

            //mcu 的
            QY_MESSENGER_ID idInfo_to;
            idInfo_to.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.av.taskInfo.idInfo_starter.ui64Id;
            qyFuncs.traceLog("缩小画面");
            qyFuncs_Droid.sendConfLayout(mOwner_wnd.mOwner_hTalk, 0, 0, Consts.CONST_imOp_phone_enlargeChannel, ref layoutParam, idInfo_imGrp_related, idInfo_requester, idInfo_to, "");
        }

        private void BtnLocalVideo_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            qyFuncs.traceLog("changeCamera clicked");

            mOwner_wnd.changeLocalVideo();

            return;
        }
        private void BtnMoreTV_Click(object sender, EventArgs e)
        {

            getMeetingInfo();
            if (more_info_menu.Visibility == ViewStates.Visible)
            {
                more_tv.Activated = false;
                more_tv.Text = "更多";
                more_info_menu.Visibility = ViewStates.Invisible;
            }
            else
            {
                more_tv.Activated = true;
                more_tv.Text = "关闭更多";
                more_info_menu.Visibility = ViewStates.Visible;
            }
        }


        private void BtnChangeCamera_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            qyFuncs.traceLog("changeCamera clicked");
            if (!isChangeCamera)
            {
                isChangeCamera = true;
                mOwner_wnd.changeCamera();
                btnChangeCamera.PostDelayed(new Action(() =>
                {
                    isChangeCamera = false;

                }), 2000
               );
            }


            return;
        }

        private void M_btn_request_Click(object sender, EventArgs e)
        {
            Activity_dlgTalk_av owner = mOwner_wnd;
            Activity_dlgTalk dlgTalk = owner.mOwner_hTalk;


            bool canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);
            do_request(!canSpeak);

            //
            dlgTalk.m_var.av.m_bState_canSpeak = !canSpeak;

            //
            m_autoSpeak = new Tmp_autoSpeak();  //  clear
        }

        private void M_Nvr_control_Click(object sender, EventArgs e)
        {
            if (left_menu_relative.Visibility == ViewStates.Invisible)
            {
                qyFuncs.traceLog("left_menu_relative:Visible");
                left_menu_relative.Visibility = ViewStates.Visible;
                right_menu_relative.Visibility = ViewStates.Visible;
                nvr_controlSetActive();
            }
            else
            {
                qyFuncs.traceLog("left_menu_relative:Invisible");
                left_menu_relative.Visibility = ViewStates.Invisible;
                right_menu_relative.Visibility = ViewStates.Invisible;
                nvr_controlSetNormal();
            }

        }

        public void nvr_controlSetNormal()
        {
            nvr_control.SetBackgroundColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_normal_color));
            nvr_control.SetTextColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_normal_text_color));
        }

        public void nvr_controlSetActive()
        {
            nvr_control.SetTextColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_active_text_color));
            nvr_control.SetBackgroundColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_active_color));
        }

        public void do_request(bool bEnable)
        {
            //throw new NotImplementedException();
            //mIsShutter = true;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            Activity_dlgTalk_av owner = mOwner_wnd;
            Activity_dlgTalk dlgTalk = owner.mOwner_hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;

            //
            if (!m_var.av.taskInfo.bTaskExists) goto errLabel;

            //
            trigKeyFrame();

            //
            qyFuncs.setCurSharedObjUsr_localAv(pProcInfo, m_var.av.iIndex_sharedObj_localAv, dlgTalk, "do_request.1614");


            //
            //bool canSpeak = qyFuncs_Droid.dlgTalk_canSpeak(dlgTalk);

            //
            if (0 != qyFuncs_Droid.dlgTalk_requestToSpeak(dlgTalk, bEnable))
            {
                goto errLabel;
            }

            //
            bool bState_canSpeak = bEnable;// !canSpeak;


            if (bState_canSpeak)
            {
                m_btn_request.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_stopSpeaking);
                m_btn_request.SetTextColor(Resources.GetColor(Resource.Color.av_button_active_text_color));
                m_btn_request.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_active_color));
            }
            else
            {
                m_btn_request.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_requestToSpeak);
                m_btn_request.SetBackgroundColor(Resources.GetColor(Resource.Color.av_button_normal_color));
                m_btn_request.SetTextColor(Resources.GetColor(Resource.Color.av_button_normal_text_color));


            }


        //
        errLabel:
            return;
        }

        public static int tmpHandler_trigKeyFrame(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            TASK_common taskElem = (TASK_common)taskElemParam;
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            if (pTaskInfo.var.iTaskId == p1Param.iTaskId)
            {
                TASK_transferAv task = (TASK_transferAv)taskElem;
                QisTrans trans = task.myTask.trans;
                int i;
                for (i = 0; i < trans.usCntLimit_transforms; i++)
                {
                    if (trans.pTransforms[i].video != null)
                    {
                        trans.pTransforms[i].video.decInfo.bGot_keyFrame = false;
                        trans.pTransforms[i].video.u_qoi.myQoi.common.var.nDecFrame_needSkipped = Consts.MAX_nDecFrames_needSkipped_request;
                    }

                }
            }


        //
        errLabel:
            return 0;
        }

        //
        public int trigKeyFrame()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            Activity_dlgTalk_av owner = mOwner_wnd;
            Activity_dlgTalk dlgTalk = owner.mOwner_hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;

            //
            if (!m_var.av.taskInfo.bTaskExists) goto errLabel;

            CMIS_MSGU cMsg = new CMIS_MSGU();
            PARAM_findOrgMsg param = new PARAM_findOrgMsg();
            param.iTaskId = m_var.av.taskInfo.iTaskId;

            pProcInfo.processQ_media.qTraverse(null, tmpHandler_trigKeyFrame, ref cMsg.msgU, param);


        errLabel:
            return iErr;
        }


        private void M_btn_av_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //mIsShutter = true;
            //
            Activity_dlgTalk_av owner = this.mOwner_wnd;

            //
            owner.mOwner_hTalk.m_var.av.bNeedAvCall = false;

            //
            owner.mOwner_hTalk.m_var.av.bAutoCancel_lostVideo = false;

            //
            qyFuncs.traceLog("M_btn_av_Click()点击退出");
            owner.exitDlgTalkAv();
        }




        public override void OnResume()
        {
            base.OnResume();

            if (mTextureView.IsAvailable)
            {
                int ii = 0;
            }
            else
            {
                int jj = 0;
            }

            
        
           
        }

        public override void OnPause()
        {
            /*
            closeCamera();
            stopBackgroundThread();
            */



            //
            base.OnPause();

        }

        public override void OnDestroyView()
        {
            //
            canCloseCameraThread = true;
            doClean("fg.OnDestroyView");
            if (h != null)
            {
                h.RemoveCallbacksAndMessages(null);
            }
           
            //
            base.OnDestroyView();


        }


        public int doClean(string hint)
        {
            string str;

            if (hint == null) hint = "";

            //
            str = string.Format("fg_dlgTalk_av2.doClean enters {0}", hint);
            qyFuncs.showInfo_open(0, null, str);

            //
            Activity_dlgTalk_av owner = (Activity_dlgTalk_av)this.Activity;
            if (owner != null)
            {
                //
                //owner.killTimer();

                //
                if (!owner.mOwner_hTalk.m_var.m_bNeedResumeTaskAv)
                {
                    qyFuncs_Droid.dlgTalk_closeTaskAv(owner.mOwner_hTalk, owner.mOwner_hTalk.m_var, "fg_dlgTalk_av.doClean_ifActivityIsFinishing");
                }
            }
            //
            if (myRenderer != null)
            {
                myRenderer.Halt();
                //Java.Lang.Thread h = mRenderer;
                //qyFuncs_Droid.waitForThread1(ref h, 10000);
                myRenderer.Join();
                //myRenderer.Exit();
                try
                {
                    //  mRenderer.Stop();
                }
                catch (UnsupportedOperationException e)
                {

                }

                myRenderer = null;// (dlgTalk_av_Renderer)h;
            }

            //
#if DEBUG
            if (owner == null)
            {
                int ii = 0;
            }
#endif

            //
            str = string.Format("fg_dlgTalk_av2.doClean leaves {0}", hint);
            qyFuncs.showInfo_open(0, null, str);

            return 0;
        }


        public int doClean_ifActivityIsFinishing()
        {
            return doClean("doClean_ifActivityIfFinishing");
        }


        public override void OnDestroy()
        {

            base.OnDestroy();
            qyFuncs.traceLog("Fragment_dlgTalk_av2  OnDestroy");
            canCloseCameraThread = true;
            if (mMainHandler != null)
            {
                qyFuncs.traceLog(" mMainHandler.RemoveCallbacksAndMessages(null);");
                //  mMainHandler.RemoveCallbacksAndMessages(null);

            }
            //            

        }


        private int statusBarHeight;

        private void getStatusBarHeight()
        {
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            statusBarHeight = Resources.GetDimensionPixelSize(resourceId);

        }


        /*     public bool shouldShowRequestPermissionRationale(string[] permissions)
             {
                 foreach (string permission in permissions)
                 {
                     if (FragmentCompat.ShouldShowRequestPermissionRationale(this, permission))
                     {
                         return true;
                     }
                 }

                 return false;

             }*/

        /*   public void requestVideoPermissions()
           {
               if (shouldShowRequestPermissionRationale(VIDEO_PERMISSIONS))
               {
                   new ConfirmationDialog().Show(ChildFragmentManager, FRAGMENT_DIALOG);
               }
               else
               {
                   FragmentCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera, Manifest.Permission.RecordAudio, },
                       REQUEST_VIDEO_PERMISSIONS);
               }
           }*/

        /*  public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
          {
              if (requestCode == REQUEST_VIDEO_PERMISSIONS)
              {
                  if (grantResults.Length == VIDEO_PERMISSIONS.Length)
                  {
                      bool bOk = true;
                      foreach (int result in grantResults)
                      {
                          if (result != (int)Permission.Granted)
                          {
                              ErrorDialog.newInstance("This app needs permission form camera and audio record")
                                 .Show(ChildFragmentManager, FRAGMENT_DIALOG);
                              //
                              bOk = false;
                          }

                      }
                      //
                      if (bOk)
                      {

                          //
                          return;
                      }

                  }
                  else
                  {
                      ErrorDialog.newInstance("This app needs permission for camera and audio record")
                          .Show(ChildFragmentManager, FRAGMENT_DIALOG);
                  }

              }
              else
              {
                  base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
              }
          }*/

        public bool hasPermissionsGranted(string[] permissions)
        {
            foreach (string permission in permissions)
            {
                if (AndroidX.Core.App.ActivityCompat.CheckSelfPermission(Activity, permission)
                    != Permission.Granted)
                {
                    return false;
                }
            }
            return true;
        }

        static Size chooseVideoSize(Size[] choices)
        {
            if (choices.Length == 0)
            {
                return new Size(0, 0);
            }
            foreach (Size size in choices)
            {
                if (size.Width == size.Height * 4 / 3 && size.Width <= 1080)
                {
                    return size;
                }
            }

            return choices[choices.Length - 1];

        }

        static Size chooseOptimalSize(Size[] choices, int width, int height, Size aspectRatio)
        {
            List<Size> bigEnough = new List<Size>();
            List<Size> notBigEnough = new List<Size>();
            int w = aspectRatio.Width;
            int h = aspectRatio.Height;

            for (int i = 0; i < choices.Length; i++)
            {
                Size option = choices[i];
                if (option.Height == option.Width * h / w
                    && option.Width >= width && option.Height >= height)
                {
                    bigEnough.Add(option);
                }
            }

            if (bigEnough.Count > 0)
            {
                return (Size)Collections.Min(bigEnough, new CompareSizesByArea());
            }
            else
            {
                return choices[0];
            }


        }

        //
        public string selectCamera(CameraManager manager, bool bFront)
        {
            string sel_id = null;

            string[] cameraIdList = manager.GetCameraIdList();
            if (cameraIdList == null) return null;

            foreach (string cameraId in cameraIdList)
            {
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
                if (bFront)
                {
                    if (((Integer)(characteristics.Get(CameraCharacteristics.LensFacing))).IntValue() == (int)LensFacing.Front)
                    {
                        sel_id = cameraId;
                        break;
                    }
                }
                else
                {
                    if (((Integer)(characteristics.Get(CameraCharacteristics.LensFacing))).IntValue() == (int)LensFacing.Back)
                    {
                        sel_id = cameraId;
                        break;
                    }
                }
            }
            if (sel_id == null)
            {
                if (cameraIdList.Length > 0)
                {
                    sel_id = cameraIdList[0];
                }
            }

            return sel_id;
        }


        //
        private void updateControls()
        {

        }
        public List<IpcInfo> ipcInfoList = new List<IpcInfo>();
        internal unsafe void showNvrList()
        {

            //  right_menu_re.Visibility.
            CCtxQyMc pQyMc = g.g_pQyMc;


            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            int i;
            ref qy.NvrInfo nvrInfo = ref pProcInfo.m_ipcProc.op.nvrInfo;



            ipcInfoList.Clear();
            if (nvrInfo.usCnt != 0)
            {
                initPopWindows();



                fixed (byte* buf_ipcInfo = nvrInfo.buf_ipcInfo)
                {
                    for (i = 0; i < nvrInfo.usCnt; i++)
                    {
                        IpcInfo* pMem = qyFuncs.getMem((IpcInfo*)buf_ipcInfo, Consts.bufSize_ipcInfos, i);
                        ipcInfoList.Add(*pMem);


                    }
                }
                if (nvrInfo.usCnt >= 1)
                {
                    nvr_channel_name.Visibility = ViewStates.Visible;

                    IpcInfo pMem;
                    pMem.iChannel = -1;
                    qyFuncs.mylstrcpyn(pMem.name, "全部球机", 128);
                    ipcInfoList.Insert(0, pMem);
                    nvrListAdapter.NotifyDataSetChanged();
                    popupWindow.ShowAsDropDown(nvr_channel_name);
                }
                nvr_channel_name.SetCompoundDrawables(null, null, pull_downDrawable, null);
            }
        }

        public void initPopWindows()
        {

            View view = LayoutInflater.From(mOwner_wnd).Inflate(Resource.Layout.popwin_layout, null);
            RecyclerView popListView = (RecyclerView)view.FindViewById(Resource.Id.pop_list_view);

            popListView.AddItemDecoration(new DividerItemDecoration(mOwner_wnd, 1));
            nvrListAdapter = new NvrListAdapter(mOwner_wnd, ipcInfoList);
            popListView.SetLayoutManager(new LinearLayoutManager(mOwner_wnd));
            popListView.SetAdapter(nvrListAdapter);
            popupWindow = new PopupWindow(view, nvr_channel_name.Width, mOwner_wnd.Window.DecorView.Height / 4, true);
            popupWindow.AnimationStyle = Resource.Animation.anim_pop;
            //popupWindow.Touchable = true;
            popupWindow.OutsideTouchable = false;
            popupWindow.Focusable = false;
            popupWindow.TouchIntercepted += (sender, args) =>
            {
                args.Handled = false;
            };
            popupWindow.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#50000000")));
            popupWindow.SetOnDismissListener(this);


        }
        public class NvrListAdapter : RecyclerView.Adapter
        {
            private Activity_dlgTalk_av mOwner_wnd;
            private List<IpcInfo> ipcInfoList;
            public NvrListAdapter(Activity_dlgTalk_av mOwner_wnd, List<IpcInfo> ipcInfoList)
            {
                this.ipcInfoList = ipcInfoList;
                this.mOwner_wnd = mOwner_wnd;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.From(mOwner_wnd).Inflate(Resource.Layout.popwin_item, parent, false);
                return new CloudViewHolder(view);
            }

            public unsafe override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {

                IpcInfo ipcInfo = ipcInfoList[position];


                string nvrName = new string(ipcInfo.name);

                ((CloudViewHolder)holder).nvr_name_tv.Text = nvrName;

                qyFuncs.traceLog("nvr_name_tv.Click");
                ((CloudViewHolder)holder).nvr_name_tv.Click += (rander, arg) =>
                {
                    IpcInfo ipcInfo = ipcInfoList[position];
                    int clickItemChannel = ipcInfo.iChannel;

                    qyFuncs.traceLog(position + "position");

                    mOwner_wnd.m_fg_dlgTalk_av2.sendCurrentQiuJIEnableControl(clickItemChannel);
                    if (nvrName.Contains("(球机)"))
                    {
                        mOwner_wnd.m_fg_dlgTalk_av2.showOperationRelativeLayout(true);
                    }
                    else
                    {
                        mOwner_wnd.m_fg_dlgTalk_av2.showOperationRelativeLayout(false);
                        mOwner_wnd.m_fg_dlgTalk_av2.nvr_control.Visibility = ViewStates.Invisible;


                    }
                    mOwner_wnd.m_fg_dlgTalk_av2.popupWindow.Dismiss();
                    mOwner_wnd.m_fg_dlgTalk_av2.nvr_channel_name.Text = nvrName;

                };

            }


            public override int ItemCount => ipcInfoList.Count;
        }
        public unsafe void sendCurrentQiuJIEnableControl(int clickItemChannel)
        {
            //组名 
            QY_MESSENGER_ID idInfo_imGrp_related = new QY_MESSENGER_ID();
            idInfo_imGrp_related.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.addr.idInfo.ui64Id;

            // 我 
            QY_MESSENGER_ID idInfo_requester = new QY_MESSENGER_ID();
            idInfo_requester.ui64Id = 0;

            //mcu 的
            QY_MESSENGER_ID idInfo_to = new QY_MESSENGER_ID();
            idInfo_to.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.av.taskInfo.idInfo_starter.ui64Id;


            CCtxQyMc pQyMc = g.g_pQyMc;


            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            ConfLayoutParam layoutParam = new ConfLayoutParam();
            layoutParam.enlargeParam.usEnlargeType = Consts.CONST_enlargeType_img;

            layoutParam.enlargeParam.ui64Id = pProcInfo.m_ipcProc.op.nvrInfo.ui64Id;
            layoutParam.ipcParam.iChannel = clickItemChannel;

            mOwner_wnd.mOwner_hTalk.m_var.currentSelectQiuJiChannel = clickItemChannel;

            qyFuncs_Droid.sendConfLayout(mOwner_wnd.mOwner_hTalk, 0, 0, Consts.CONST_imOp_enlargeChannel, ref layoutParam, idInfo_imGrp_related, idInfo_requester, idInfo_to, "");
            qyFuncs.traceLog("##球机sendConfLayout sendCurrentQiuJIEnableControl:" + clickItemChannel);
        }

        public unsafe void showOperationRelativeLayout(bool show)
        {
            if (show)
            {
                nvr_control.SetTextColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_active_text_color));
                nvr_control.SetBackgroundColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_active_color));
                mOwner_wnd.m_fg_dlgTalk_av2.nvr_control.Visibility = ViewStates.Visible;
                mOwner_wnd.m_fg_dlgTalk_av2.left_menu_relative.Visibility = ViewStates.Visible;
                mOwner_wnd.m_fg_dlgTalk_av2.right_menu_relative.Visibility = ViewStates.Visible;
            }
            else
            {

                //  mOwner_wnd.m_fg_dlgTalk_av2.nvr_control.Visibility = ViewStates.Invisible;
                nvr_control.SetBackgroundColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_normal_color));
                nvr_control.SetTextColor(mOwner_wnd.Resources.GetColor(Resource.Color.av_button_normal_text_color));
                mOwner_wnd.m_fg_dlgTalk_av2.left_menu_relative.Visibility = ViewStates.Invisible;
                mOwner_wnd.m_fg_dlgTalk_av2.right_menu_relative.Visibility = ViewStates.Invisible;
            }

        }

        public class CloudViewHolder : RecyclerView.ViewHolder
        {

            public TextView nvr_name_tv;
            public CloudViewHolder(View itemView) : base(itemView)
            {
                nvr_name_tv = (TextView)itemView.FindViewById(Resource.Id.nvr_name_tv);

            }
        }
        public unsafe void dlg_YTBtn_remote(int nResourceID, byte ucMouseStatus)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            int uiObjType = 0;
            short usIndex_obj = 0;
            short usHelp_subIndex = 0;
            //
            byte ucCmd = 0;
            byte ucParam = 0;


            QY_MESSENGER_ID idInfo = new QY_MESSENGER_ID();
            idInfo.ui64Id = (ulong)pProcInfo.m_ipcProc.op.nvrInfo.ui64Id;



            int iChannel = mOwner_wnd.mOwner_hTalk.m_var.currentSelectQiuJiChannel;
            qyFuncs.traceLog("##球机dlg_YTBtn_remote" + iChannel);
            QY_MESSENGER_ID idInfo_imGrp_related = new QY_MESSENGER_ID();
            idInfo_imGrp_related.ui64Id = mOwner_wnd.mOwner_hTalk.m_var.addr.idInfo.ui64Id;
            QY_MESSENGER_ID idInfo_to;
            if ((ulong)pProcInfo.m_ipcProc.op.nvrInfo.ui64Id == 0)
            {
                idInfo_to.ui64Id = (ulong)mOwner_wnd.mOwner_hTalk.m_var.currentSelectQiuJiIdInfo;
            }
            else
            {
                idInfo_to.ui64Id = (ulong)pProcInfo.m_ipcProc.op.nvrInfo.ui64Id;
            }



            do
            {
                switch (nResourceID)
                {
                    case Consts.IDC_BUTTON_UP:
                        ucCmd = Consts.CONST_ptzCmd_up;
                        sendPtzReq(uiObjType, usIndex_obj, usHelp_subIndex, idInfo, iChannel, ucCmd, ucParam, ucMouseStatus, idInfo_imGrp_related, idInfo_to);
                        break;
                    case Consts.IDC_BUTTON_DOWN:
                        ucCmd = Consts.CONST_ptzCmd_down;
                        sendPtzReq(uiObjType, usIndex_obj, usHelp_subIndex, idInfo, iChannel, ucCmd, ucParam, ucMouseStatus, idInfo_imGrp_related, idInfo_to);
                        break;
                    case Consts.IDC_BUTTON_LEFT:
                        ucCmd = Consts.CONST_ptzCmd_left;
                        sendPtzReq(uiObjType, usIndex_obj, usHelp_subIndex, idInfo, iChannel, ucCmd, ucParam, ucMouseStatus, idInfo_imGrp_related, idInfo_to);
                        break;
                    case Consts.IDC_BUTTON_RIGHT:
                        ucCmd = Consts.CONST_ptzCmd_right;
                        sendPtzReq(uiObjType, usIndex_obj, usHelp_subIndex, idInfo, iChannel, ucCmd, ucParam, ucMouseStatus, idInfo_imGrp_related, idInfo_to);
                        break;
                    case Consts.IDC_BUTTON_ZOOM_IN:
                        ucCmd = Consts.CONST_ptzCmd_zoomIn;
                        sendPtzReq(uiObjType, usIndex_obj, usHelp_subIndex, idInfo, iChannel, ucCmd, ucParam, ucMouseStatus, idInfo_imGrp_related, idInfo_to);
                        break;
                    case Consts.IDC_BUTTON_ZOOM_OUT:
                        ucCmd = Consts.CONST_ptzCmd_zoomOut;
                        sendPtzReq(uiObjType, usIndex_obj, usHelp_subIndex, idInfo, iChannel, ucCmd, ucParam, ucMouseStatus, idInfo_imGrp_related, idInfo_to);
                        break;

                    default:
                        break;
                }

            } while (false);

            //
            return;
        }
        public unsafe int sendPtzReq(int uiObjType, short usIndex_obj, short usHelp_subIndex, QY_MESSENGER_ID idInfo, int iChannel, byte ucCmd, byte ucParam, byte ucMouseStatus, QY_MESSENGER_ID idInfo_imGrp_related, QY_MESSENGER_ID idInfo_to)
        {
            qyFuncs.traceLog(uiObjType + " eeeee  " + usIndex_obj + "   " + usHelp_subIndex + "   " + idInfo.ui64Id + "   " + iChannel + "   " + ucCmd + "   " + ucParam + "   " + ucMouseStatus + "   " + idInfo_imGrp_related.ui64Id + "   " + idInfo_to.ui64Id);


            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();






            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            int uiChannelType = Consts.CONST_channelType_media;


            PTZ_control_cmd req = new PTZ_control_cmd();
            //
            req.uiType = Consts.CONST_imCommType_ptzControlCmd;


            //
            req.idInfo_imGrp_related = idInfo_imGrp_related;

            //
            req.cmdInfo.obj.uiObjType = uiObjType;
            req.cmdInfo.obj.usIndex_obj = usIndex_obj;
            //  2016/07/24
            //req.cmdInfo.obj.usHelp_subIndex  =  pDlgPtzControl->m_var.remote.remoteCamera.qisResObj.resObj.usHelp_subIndex;
            req.cmdInfo.obj.usHelp_subIndex = usHelp_subIndex;
            //
            req.cmdInfo.idInfo = idInfo;
            req.cmdInfo.iChannel = iChannel;
            //
            req.cmdInfo.ucCmd = ucCmd;
            req.cmdInfo.ucParam = ucParam;
            req.cmdInfo.ucMouseStatus = ucMouseStatus;
            Int64 tStartTran; uint uiTranNo;
            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);

            qyFuncs.traceLog(" req.uiType:" + req.uiType + "   req.idInfo_imGrp_related:" + req.idInfo_imGrp_related.ui64Id + "  req.cmdInfo.obj.uiObjType:" + req.cmdInfo.obj.uiObjType + "  req.cmdInfo.obj.usIndex_obj:" + req.cmdInfo.obj.usIndex_obj + "   req.cmdInfo.obj.usHelp_subIndex:" + req.cmdInfo.obj.usHelp_subIndex


                + "     req.cmdInfo.idInfo:" + req.cmdInfo.idInfo.ui64Id + "   req.cmdInfo.iChannel:" + req.cmdInfo.iChannel + " \n req.cmdInfo.ucCmd:" + req.cmdInfo.ucCmd + "    req.cmdInfo.ucParam: " + req.cmdInfo.ucParam + "   req.cmdInfo.ucMouseStatus:" + req.cmdInfo.ucMouseStatus + "  tStartTran:" + tStartTran + " uiTranNo:" + uiTranNo);
            MSG_ROUTE msgRoute = new MSG_ROUTE();
            if (0 != qyFuncs.postMsg2Mgr_mc(null, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_outputTask, 0, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, (byte*)&req, (uint)sizeof(PTZ_control_cmd), 0, idInfo_to.ui64Id, (uint)uiChannelType, null, false)) ;



            iErr = 0;

        errLabel:

            return iErr;
        }

        public void OnDismiss()
        {
            qyFuncs.traceLog("OnDismiss");
            nvr_channel_name.SetCompoundDrawables(null, null, pull_upDrawable, null);
        }
        public int currentVideoStatus = 0;
        internal void changeCurrentVideoStatus(int bAmplification)
        {

            qyFuncs.traceLog("收到窗口状态:" + bAmplification);
            currentVideoStatus = (int)bAmplification;
            if (bAmplification != 0)//response 画面放大大于0  缩小=0
            {

                min_viewport.Visibility = ViewStates.Visible;

            }
            else
            {
                if (popupWindow != null)
                {
                    popupWindow.Dismiss();
                }

                nvr_channel_name.Visibility = ViewStates.Invisible;
                nvr_channel_name.Text = mOwner_wnd.GetString(Resource.String.nvr_channel_name);
                min_viewport.Visibility = ViewStates.Invisible;
                mOwner_wnd.m_fg_dlgTalk_av2.nvr_control.Visibility = ViewStates.Invisible;
                mOwner_wnd.m_fg_dlgTalk_av2.showOperationRelativeLayout(false);
                changeNvrControlStatus(0);
            }

        }

        internal void initMinButtonStates()

        {
            if (mOwner_wnd.mOwner_hTalk.respVideoStatusIsenlargement != -1)
            {
                if (mOwner_wnd.mOwner_hTalk.respVideoStatusIsenlargement == Consts.Video_Status_Enlargement)
                {
                    min_viewport.Visibility = ViewStates.Visible;
                }
                else
                {
                    currentVideoStatus = 0;//初始画面
                    min_viewport.Visibility = ViewStates.Invisible;

                }
                mOwner_wnd.mOwner_hTalk.respVideoStatusIsenlargement = -1;
            }


        }

        internal void changeNvrControlStatus(int qiujiChannel)
        {
            qyFuncs.traceLog(" changeNvrControlStatus:" + qiujiChannel);

            if (qiujiChannel >= 1)
            {
                nvr_control.Visibility = ViewStates.Visible;

            }
            else
            {

                nvr_control.Visibility = ViewStates.Invisible;
                showOperationRelativeLayout(false);
            }
        }

        public class MainHandler : Handler
        {
            private const int MSG_SEND_CAMERA_PARAMS0 = 0;
            private const int MSG_SEND_CAMERA_PARAMS1 = 1;
            private const int MSG_SEND_RECT_SIZE = 2;
            private const int MSG_SEND_ZOOM_AREA = 3;
            private const int MSG_SEND_ROTATE_DEG = 4;
            //
            private const int MSG_talkDraw_needRestart = 10;

            //private WeakReference<TextureFromCameraActivity> mWeakActivity;
            Fragment_dlgTalk_av2 m_fg;

            public MainHandler(Fragment_dlgTalk_av2 fg)
            {
                //mWeakActivity = new WeakReference<TextureFromCameraActivity>(activity);
                m_fg = fg;
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

            //
            public void send_MSG_talkDraw_needRestart()
            {
                SendMessage(ObtainMessage(MSG_talkDraw_needRestart, 0, 0));
            }



            public override void HandleMessage(Message msg)
            {
                Fragment_dlgTalk_av2 activity = m_fg;
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
                            activity.mCameraPreviewWidth_unused = msg.Arg1;
                            activity.mCameraPreviewHeight_unused = msg.Arg2;
                            break;
                        }
                    case MSG_SEND_CAMERA_PARAMS1:
                        {
                            activity.mCameraPreviewFps_unused = msg.Arg1 / 1000.0f;
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
                    case MSG_talkDraw_needRestart:
                        {
                            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)activity.Activity;
                            if (dlgTalk_av != null)
                            {
                                if (dlgTalk_av.m_bTalkDraw_needRestart)
                                {
                                    dlgTalk_av.pauseTaskAv("fg_dlgTalk_av2.Handler, MSG_talkDraw_needRestart");
                                    //
                                    dlgTalk_av.m_bDlgTalk_av_readyFor_talkDrawRestart = true;
                                }
                            }
                        }
                        break;
                    default:
                        throw new RuntimeException("Unknown message " + msg.What);
                }
#endif
            }
        }

        //

        //

    }
}




