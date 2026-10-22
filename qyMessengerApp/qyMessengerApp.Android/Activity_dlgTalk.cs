using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using qy;
using qy.Droid;
using AndroidX.ViewPager.Widget;
using Pub.Devrel.Easypermissions;
using Android.Content.PM;
using static AndroidX.RecyclerView.Widget.RecyclerView;
using Org.Aviran.Cookiebar2;
using Android.Media;
using qyMessengerApp.Android;
using Manifest = Android.Manifest;


//
namespace qyMessengerApp.Droid
{
    [Activity(Label = "Activity_dlgTalk", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Keyboard)]
    public class Activity_dlgTalk : qmcBaseActivity, View.IOnClickListener, ViewPager.IOnPageChangeListener, EasyPermissions.IPermissionCallbacks
    {
        //
        public DLG_TALK_var m_var;
        public Activity_dlgTalk_av tmp_hTool_dlgTalk_av;

        public Activity_dlgTalk_av m_hTool_dlgTalk_av ;

        //private TextView txt_msg;
        //private TextView txt_detail;

        private ViewPager vPager;

        public FragmentPagerAdapter_dlgTalk mAdapter;

        public const int PAGE_ONE = 0;
        //public const int PAGE_TWO = 1;

        public string m_str_title;

        //
        public MyHandler mHandler = null;

        public int respVideoStatusIsenlargement = -1;
        private AppSettingsDialog appSettingsDialog;
        //
        String[] mPermissionList = new String[] { Manifest.Permission.Camera, Manifest.Permission.RecordAudio };
        private static int REQUEST_CODE_CAMERA_RECORDAUDIO_PERMISSION = 11;
        internal int currentSelectQiuJiChnal;

        public void isPermissionOk()
        {

            // bool bRet = false;
            //int ii = 0;
            //
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (!hasAllPermission())
                {
                    // Ask for one permission
                    EasyPermissions.RequestPermissions(
                            this,
                            Resources.GetString(Resource.String.permission_content_camera_mic),
                            REQUEST_CODE_CAMERA_RECORDAUDIO_PERMISSION,
                            mPermissionList);

                }

            }

        }
        public bool hasAllPermission()
        {
            return EasyPermissions.HasPermissions(this, mPermissionList);
        }


        //授权结果，分发下去
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            // Forward results to EasyPermissions
            //跳转到onPermissionsGranted或者onPermissionsDenied去回调授权结果
            int[] gr = new int[grantResults.Length];
            for (int i = 0; i < gr.Length; i++)
            {
                gr[i] = (int)grantResults[i];
            }
            //
            EasyPermissions.OnRequestPermissionsResult(requestCode, permissions, gr, this);
        }

        public void OnPermissionsGranted(int p0, IList<string> p1)
        {


        }

        //拒绝授权
        public void OnPermissionsDenied(int p0, IList<string> p1)
        {
            //Log.i(TAG, "onPermissionsDenied:" + requestCode + ":" + perms.size());
            //    CCtxQyMc pQyMc = g.g_pQyMc;

            //   GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_login_canceled, 0);

            //
            if (EasyPermissions.SomePermissionPermanentlyDenied(this, p1))
            {
                AppSettingsDialog.Builder buider = new AppSettingsDialog.Builder(this);

                foreach (string permission in p1)
                {
                    switch (permission)
                    {
                        case Manifest.Permission.Camera:
                            buider.SetRationale(Resources.GetString(Resource.String.permission_content_camera));
                            goto f;
                        case Manifest.Permission.RecordAudio:
                            buider.SetRationale(Resources.GetString(Resource.String.permission_content_mic));
                            goto f;

                    }
                }

            f: appSettingsDialog = buider.Build();
                appSettingsDialog.Show();
            }


        }

        //
        public static DLG_TALK_var tmp_getDlgTalkVar(object hDlgTalk)
        {

            if (!GuiShare.pf_IsWindow(hDlgTalk)) return null;
            Activity_dlgTalk pDlgTalk = (Activity_dlgTalk)(hDlgTalk);
            if (null == pDlgTalk) return null;

            //return  &(  (  CDlgTalk  *  )pDlgTalk  )->m_var;
            return (pDlgTalk).get_pm_var();
        }

        public DLG_TALK_var get_pm_var()
        {
            return m_var;
        }



        public class MyHandler : Handler
        {
            Activity_dlgTalk m_hDlgTalk = null;
            public MyHandler(Activity_dlgTalk hDlgTalk)
            {
                m_hDlgTalk = hDlgTalk;
            }

            public override void HandleMessage(Message msg)
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                if (pQyMc == null) return;

                string str;

                //
                switch (msg.What)
                {
                    case Consts.CONST_qyWm_postComm:

                        //
                        switch (msg.Arg1)
                        {
                            case Consts.CONST_qyWmParam_doTask:
                                m_hDlgTalk.doTask_av_accept();
                                break;
                            case Consts.CONST_qyWmParam_chkTalkDraw:
                                {
                                    qyFuncs.traceLog("dlgTalk.get a msg chkTalkDraw");
                                    //
                                    m_hDlgTalk.doChkTalkDraw();

                                    //
                                    // reset
                                    m_hDlgTalk.m_var.dwTickCnt_waitForOk_dlgTalk_av1 = 0;

                                }
                                break;
                            case Consts.CONST_qyWmParam_closeWnd:
                                {
                                    str = string.Format("dlgTalk.closeWnd starts");
                                    qyFuncs.showInfo_open(0, null, str);

                                    m_hDlgTalk.doFinish("closeWnd");
                                    //
                                    str = string.Format("dlgTalk.closeWnd ends");
                                    qyFuncs.showInfo_open(0, null, str);
                                }
                                break;
                            case Consts.CONST_qyWmParam_msgArrive:
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

            }

        };

        //
        public int gui_PostMessage(int msgId, int wParam, int lParam)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;


            {
                Message msg = this.mHandler.ObtainMessage();
                msg.What = msgId;
                msg.Arg1 = wParam;
                msg.Arg2 = lParam;
                this.mHandler.SendMessage(msg);
                return 0;
            }


            return 0;
        }




        //
        public unsafe int gui_procMsg(ref MIS_MSGU pMsgParam)
        {
            int iErr = -1;
            ref MIS_MSGU pMsg = ref pMsgParam;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            /*
            switch (pMsg->uiType)
            {
                case Consts.CONST_misMsgType_talk:
                    {
                        IM_CONTENTU* pContent = (IM_CONTENTU*)pMsg->talk.data.buf;
                        switch (pContent->uiType)
                        {
                            case Consts.CONST_imCommType_htmlContent:
                                {
                                    
                                }
                                break;
                        }


                    }
                    break;
                default:
                    break;


            }
            */


            //
            if (mAdapter.mFragment_dlgTalk_msgList1 != null)
            {
                mAdapter.mFragment_dlgTalk_msgList1.gui_procMsg(ref pMsg);
            }




            return iErr;

        }

        //
        public int startActivity_dlgTalk_av(int iCmd, int index_taskInfo, string hint)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            string str;

            //
            //if (this.m_var.m_hTool_dlgTalk_av != null) return -1;

            //int ii = m_var.tmp_hTools_dlgTalk_av.GetLength(0);


            //
            int cnt = 0;
            /*
            int i;
            for (i = 0; i < m_var.tmp_hTools_dlgTalk_av.GetLength(0); i++)
            {
                if (m_var.tmp_hTools_dlgTalk_av[i] != null)
                {
                    cnt++;
                }
            }
            */
            if (this.tmp_hTool_dlgTalk_av != null)
            {
                cnt++;
            }
            if (cnt != 0) return -1;

            //
            uint dwTickCnt = qyFuncs.myGetTickCount(null);
            int iDiffInMs = (int)(dwTickCnt - m_var.dwTickCnt_waitForOk_dlgTalk_av1);
            if (Java.Lang.Math.Abs(iDiffInMs) < 1500)
            {
                str = "startActivity_dlgTalk_av failed,iDiffInMs " + iDiffInMs + ", too quickly";
                qyFuncs.showInfo_open(0, "", str);
                return -1;
            }
            m_var.dwTickCnt_waitForOk_dlgTalk_av1 = dwTickCnt;

            //
            if (index_taskInfo >= 0)
            {
                if (!qyFuncs.qmcTaskInfo_bAlive(null, pProcInfo, index_taskInfo))
                {
                    return -1;
                }
            }


            //
            str = "startActivity_dlgTalk_av " + hint;
            qyFuncs.showInfo_open(0, "", str);
            mainActivity.canReceiveMessage = false;
            //
            Context context = this;
            Intent intent = new Intent(context, typeof(Activity_dlgTalk_av));
            intent.SetFlags(ActivityFlags.ReorderToFront);
            intent.PutExtra("iCmd", iCmd);
            intent.PutExtra("ui64Id", (long)m_var.addr.idInfo.ui64Id);
            //
            intent.PutExtra("index_qmcTaskInfo", index_taskInfo);
            //
            StartActivity(intent);

            //
            iErr = 0;
        errLabel:

            return iErr;
        }

        //
        public int startActivity_dlgTalk_detail()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            //if (this.m_var.m_hTool_dlgTalk_av != null) return -1;

            //
            Context context = this;
            Intent intent = new Intent(context, typeof(Activity_dlgTalk_detail));
            //
            //intent.PutExtra("iCmd", iCmd);
            intent.PutExtra("ui64Id", (long)m_var.addr.idInfo.ui64Id);
            //
            //intent.PutExtra("index_qmcTaskInfo", index_taskInfo);
            //
            StartActivity(intent);

            iErr = 0;
        errLabel:

            return iErr;
        }



        //
        public unsafe int doTask_av_accept()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;


            //
            if (!(mainActivity.var_common.notifyTaskStatus.bExists_task && mainActivity.var_common.notifyTaskStatus.bAvCall))
            {
                // qyFuncs.showNotification_open(0, 0, 0, "Can't start a avTask, no task needs be accepted.");
                qyFuncs.traceLog("Can't start a avTask, no task needs be accepted.");
                goto errLabel;
            }


            QMC_TASK_INFO pTaskInfo = pProcInfo.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, mainActivity.var_common.notifyTaskStatus.index_taskInfo, "doTask_av_accept");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            //            
            if (pTaskData.msgU.uiType != Consts.CONST_misMsgType_task)
            {
                goto errLabel;
            }
            if (pTaskData.msgU.task.addr_logicalPeer.idInfo.ui64Id != this.m_var.addr.idInfo.ui64Id)
            {
                qyFuncs.traceLog("Can't start a avTask, because logicalPeer != talker.");
                //   qyFuncs.showNotification_open(0, 0, 0, "Can't start a avTask, because logicalPeer != talker.");
                goto errLabel;

            }


            //
            if (true)
            {
                /*
                CHelp_getDlgTalkVar help_getDlgTalkVar;
                DLG_TALK_var* pm_var = (DLG_TALK_var*)help_getDlgTalkVar.getVar(hWnd_task);
                if (!pm_var) goto errLabel;
                DLG_TALK_var & m_var = *pm_var;
                */

                //
                object hWnd_task = this;
                fixed (MIS_MSG_TASK* pMsg = &pTaskData.msgU.task)
                {

                    //if (uiTaskType != Consts.CONST_imTaskType_viewDynBmp)
                    {   //  2014/09/26

                        //
                        MIS_MSGU* pMsgU = (MIS_MSGU*)pMsg;
                        if (0 != GuiShare.pf_dlgTalk_qPostMsg(hWnd_task, ref *pMsgU, (uint)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsg->lenInBytes)) goto errLabel;

                        /*
                        //  2016/02/27
                        PARAM_PROC_ROW param;
                        memset(&param, 0, sizeof(param));
                        param.bScrollIntoView = true;
                        if (dyn_tmpHandler_showMsg(hWnd_task, &param, pMsg)) goto errLabel;
                        */
                    }
                }
            }

            //
            m_var.av.bNeedDoTaskAv = true;


            if (hasAllPermission())
            {
                startActivity_dlgTalk_av(tmpResource.IDC_BUTTON_av_accept, mainActivity.var_common.notifyTaskStatus.index_taskInfo, "doTask_av_accept");

            }
            else
            {
                isPermissionOk();
            }


            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public unsafe int doChkTalkDraw()
        {
            int iErr = -1;
            bool bDbg = false;
            string str;

            //
#if DEBUG
            bDbg = true;
#endif

            //
            if (bDbg)
            {
                str = string.Format("dlgTalk.doChkTalkDraw enters");
                qyFuncs.showInfo_open(0, null, str);
            }

            //
            int i;
            int cnt = 0;
            /*
            for (i = 0; i < m_var.tmp_hTools_dlgTalk_av.GetLength(0); i++)
            {
                if (m_var.tmp_hTools_dlgTalk_av[i] != null)
                {
                    cnt++;
                }
            }
            */
            if (this.tmp_hTool_dlgTalk_av != null)
            {
                cnt++;
            }
            //
            if (cnt > 1)
            {
                int ii = 0;
            }
            //
            if (cnt == 0)
            {
                m_hTool_dlgTalk_av = null;
                m_var.m_share_eglContext = null;
                iErr = 0; goto errLabel;
            }
            //
            if (cnt == 1)
            {
                //for (i = 0; i < m_var.tmp_hTools_dlgTalk_av.GetLength(0); i++)
                {
                    /*
                    if (m_var.tmp_hTools_dlgTalk_av[i] != null
                        && m_var.tmp_share_eglContexts[i] != null)
                    */
                    if (this.tmp_hTool_dlgTalk_av != null
                        && m_var.tmp_share_eglContext != null)
                    {
                        //
                        object tmp_hTool_dlgTalk_av = this.tmp_hTool_dlgTalk_av;// m_var.tmp_hTools_dlgTalk_av[i];
                        object tmp_share_eglContext = m_var.tmp_share_eglContext;//.tmp_share_eglContexts[i];
                        //
                        if (tmp_hTool_dlgTalk_av != m_hTool_dlgTalk_av
                            || tmp_share_eglContext != m_var.m_share_eglContext)
                        {
                            m_hTool_dlgTalk_av = (Activity_dlgTalk_av)tmp_hTool_dlgTalk_av;
                            m_var.m_share_eglContext = tmp_share_eglContext;
                            //
                            Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)m_hTool_dlgTalk_av;
                            if (hTalk_av.m_fg_dlgTalk_av2 != null)
                            {
                                hTalk_av.m_fg_dlgTalk_av2.waitTextureViewAvailable(null, null, null);
                            }
                        }
                        //
                        //break;
                    }
                }
                iErr = 0; goto errLabel;
            }


            //
            iErr = 0;
        errLabel:

            //
            if (bDbg)
            {
                str = string.Format("dlgTalk.doChkTalkDraw leaves");
                qyFuncs.showInfo_open(0, null, str);
            }


            return iErr;

        }


        //
        public int doTask_startAvCall()
        {
            int iErr = -1;

            //
            //int ii = m_var.tmp_hTools_dlgTalk_av.GetLength(0);


            //
            m_var.av.bNeedDoTaskAv = true;
            if (hasAllPermission())
            {

                startActivity_dlgTalk_av(tmpResource.IDC_BUTTON_av, -1, "doTask_startAvCall");

            }
            else
            {
                isPermissionOk();
            }

            iErr = 0;

        errLabel:

            return iErr;

        }

        Action a;
        Handler h;
        public unsafe int mgui_onTimer(object p0, object p1, object p2)

        {
            h = new Handler();
            a = new Action(() =>
            {
                string str;
                bool bDbg = false;

                //m_var.loopCtrl_timer++;

#if DEBUG
                if (bDbg)
                {
                    qyFuncs_Droid.printTalkers("talk.gui_onTimer enters");
                }
#endif


                //
                qyFuncs_Droid.dlgTalk_myTimerProc(this, 0, 0, 0);

                //


                //
#if DEBUG
                if (false)
                {
                    str = "dlgTalk.gui_onTimer" + m_var.loopCtrl_timer;
                    qyFuncs.showInfo_open(0, "", str);
                }
#endif


                //
                if (this.mAdapter.mFragment_dlgTalk_msgList1 != null)
                {
                    mAdapter.mFragment_dlgTalk_msgList1.gui_onTimer(0, 0, 0);
                }


                //
                /*
                if (m_var.m_hTool_dlgTalk_av != null)
                {
                    Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)m_var.m_hTool_dlgTalk_av;
                    dlgTalk_av.gui_onTimer(p0, p1, p2);
                }
                */




#if DEBUG
                if (bDbg)
                {
                    qyFuncs_Droid.printTalkers("dlgTalk.gui_onTimer leaves");
                }
#endif

                h.PostDelayed(a, 1000);

            });
            h.PostDelayed(a, 1000);



            return 0;
        }

        //
 
        //
        public static unsafe int talk_updatePlayerInfo(object hTalk, IntPtr pMIS_MSG_applyForPlayer_qmc, PLAYER_ID playerId)
        {
            int iErr = -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hTalk;
            MIS_MSG_applyForPlayer_qmc* pMsg = (MIS_MSG_applyForPlayer_qmc*)pMIS_MSG_applyForPlayer_qmc;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;


            DLG_TALK_var m_var = dlgTalk.m_var;
            bool bChanged = false;

            if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
            if (pMsg->iTaskId != m_var.av.taskInfo.iTaskId) goto errLabel;

            //
            int index_taskInfo = m_var.av.taskInfo.iIndex_taskInfo;
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "do_closeTask");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;
            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            //
            DLG_TALK_videoConferenceActiveMemFrom pActiveMem = null;
            int i;

            //
#if DEBUG
            ref MIS_MSG_applyForPlayer_qmc tmp_pMsg = ref *pMsg;
#endif


            //
            if (pMsg->idInfo_recorder.ui64Id != 0)
            {
                for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
                {
                    pActiveMem = videoConference.activeMems_from[i];
                    if (pActiveMem.avStream.idInfo.ui64Id == pMsg->idInfo_recorder.ui64Id
                        && pActiveMem.avStream.obj.tranInfo.video.uiTranNo_openAvDev == pMsg->video.uiTranNo)
                    {
                        break;
                    }
                }
                if (i == videoConference.usCntLimit_activeMems_from) goto errLabel;
                //
                if (m_var.av.taskInfo.ucbVideoConference)
                {
                    if (0 == pActiveMem.avStream.obj.resObj.uiObjType) goto errLabel;
                }
            }

            //
            if (pMsg->idInfo_recorder.ui64Id == 0)
            {
                if (m_var.m_drawLayout.param.me.param.playerId.index_player != playerId.index_player
                    || m_var.m_drawLayout.param.me.param.playerId.uiTranNo_player != playerId.uiTranNo_player)
                {
                    m_var.m_drawLayout.param.me.param.playerId = playerId;
                    int tt = 0;
                    m_var.m_drawLayout.param.me.tn_modified = qyFuncs.getuiNextTranNo(null, 0, ref tt);
                    bChanged = true;
                }
            }
            else
            {
                if (m_var.m_drawLayout.param.peer.param.playerId.index_player != playerId.index_player
                    || m_var.m_drawLayout.param.peer.param.playerId.uiTranNo_player != playerId.uiTranNo_player)
                {
                    QY_PLAYER pPlayer = qyFuncs.getPlayerByIndex(pProcInfo, playerId.index_player);
                    //
                    m_var.m_drawLayout.param.peer.param.playerId = playerId;
                    int tt = 0;
                    m_var.m_drawLayout.param.peer.tn_modified = qyFuncs.getuiNextTranNo(null, 0, ref tt);
                    bChanged = true;
                }
            }


            iErr = 0;
        errLabel:

            if (iErr == 0)
            {
                if (bChanged)
                {
                    int tt = 0;
                    m_var.m_drawLayout.tn_modified = qyFuncs.getuiNextTranNo(null, 0, ref tt);
                }
            }

            return iErr;
        }

        //
        public unsafe int do_closeTask(int index_taskInfo)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            string str;

            //
            str = string.Format("dlgTalk.do_closeTask enters");
            qyFuncs.showInfo_open(0, null, str);

            //
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "do_closeTask");
            if (pTaskInfo == null) return -1;

            if (m_var.av.taskInfo.bTaskExists
                && m_var.av.taskInfo.iIndex_taskInfo == index_taskInfo)
            {
                //
#if false
                qyFuncs_Droid.dlgTalk_closeTaskAv(this, m_var,"dlgTalk.do_closeTask");
                //
                if (m_var.m_hTool_dlgTalk_av != null)
                {
                    Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)m_var.m_hTool_dlgTalk_av;
                    dlgTalk_av.doFinish("dlgTalk.do_closeTask");
                }
#endif
                //
                if (m_hTool_dlgTalk_av != null)
                {
                    //
                    str = string.Format("trigger to close taskAv");
                    qyFuncs.showInfo_open(0, null, str);
                    //
                    GuiShare.pf_PostMessage_new(m_hTool_dlgTalk_av, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);
                }

                //
                iErr = 0; goto errLabel;
            }

            //            
            qyFuncs_Droid.mainWnd_do_closeTask(index_taskInfo);


            //
            iErr = 0;
        errLabel:

            //
            str = string.Format("dlgTalk.do_closeTask leaves");
            qyFuncs.showInfo_open(0, null, str);

            return iErr;
        }


        //
        public unsafe int findOldAvTaskActive(ref int piTaskId, ref int piElapseInMs)
        {
            int iErr = -1;
            Activity_dlgTalk mhTalk = this;
            //
            if (mhTalk == null) return -1;
            if (mhTalk.m_var == null) return -1;
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            int i;
            string str;

            for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
            {
                QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, i, "");
                if (pTaskInfo == null) goto errLabel;
                if (!pTaskInfo.bUsed) continue;
                QMC_taskData_common taskData = pTaskInfo.getTaskData();
                if (taskData.uiType != Consts.CONST_taskDataType_conf) continue;
                QMC_taskData_conf pTc = (QMC_taskData_conf)taskData;
                fixed (MIS_MSG_TASK* pMsgTask = &pTc.msgU.task)
                {
                    if (pMsgTask->uiType != Consts.CONST_misMsgType_task) continue;
                    //
                    if (pMsgTask->addr_logicalPeer.idInfo.ui64Id == mhTalk.m_var.addr.idInfo.ui64Id
                        && pMsgTask->data.route.idInfo_from.ui64Id != pMisCnt.idInfo.ui64Id)
                    {
                        IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsgTask->data.buf;
                        if (pContent->uiType != Consts.CONST_imCommType_transferAvInfo) continue;

#if DEBUG
                        ref IM_CONTENTU tmp_pContent = ref *pContent;
#endif
                        uint uiTickCnt = qyFuncs.myGetTickCount(null);
                        int iDissInMs = (int)(uiTickCnt - pTaskInfo.var.dwTickCnt_recv_lastRefreshed1);
                        if (Java.Lang.Math.Abs(iDissInMs) < 600)
                        {
                            //str = "任务 " + pTaskInfo.var.iTaskId + " 仍在活跃中";
                            //qyFuncs.showNotification_open(0, 0, 0, str);

                            //
                            piTaskId = pTaskInfo.var.iTaskId;
                            piElapseInMs = iDissInMs;

                            //
                            break;
                        }



                    }
                }
            }
            if (i == pProcInfo.cfg.usMaxCnt_taskInfos) goto errLabel;

            iErr = 0;
        errLabel:
            return iErr;
        }



        //
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            qyFuncs.traceLog("Activity_dlgTalk   OnCreate ");
            // Create your application here
            int iErr = -1;
            string str;

            //
            this.mTag = "dlgTalk";

            //
            Intent intent = this.Intent;
            UInt64 ui64Id = (UInt64)intent.GetLongExtra("ui64Id", 0);
            int wParam = (int)intent.GetIntExtra("wParam", 0);
            int lParam = (int)intent.GetIntExtra("lParam", 0);
            //
            Console.WriteLine("ui64Id {0}", ui64Id);

            //
            mHandler = new MyHandler(this);

            //


            //
            if (m_var != null)
            {
                int iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii = 0;
            }
            m_var = new DLG_TALK_var();
            m_var.tickCountOnCreate = qyFuncs.myGetTickCount(null);

            //
            m_var.m_pProcessQ = new List<TableItem_dlgTalk_msg>();


            //
            //int ii = m_var.tmp_hTools_dlgTalk_av.GetLength(0);



            //        
            m_var.addr.idInfo.ui64Id = ui64Id;

            //            
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) goto errLabel;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            //
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;


            //
            if (m_var.addr.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
            {
                str = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_cantTalkToMySelf) + m_var.addr.idInfo.ui64Id;
                qyFuncs.showInfo_open(0, null, str);
                qyFuncs.showNotification_open(0, 0, 0, str);
                goto errLabel;
            }


            //
            switch (wParam)
            {
                case Consts.CONST_qyWmParam_doTask:
                    m_var.bActivityStarted_toDoTask = true;
                    m_var.toDoTask_usOp = (ushort)lParam;
                    break;
                default:
                    break;
            }

            //
            //

            //
            SetContentView(Resource.Layout.layout_activity_dlgTalk);


            //

            //
            mAdapter = new FragmentPagerAdapter_dlgTalk(SupportFragmentManager);



            bindViews();

            //
            //
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);


            //
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            QY_MESSENGER_ID idInfo = m_var.addr.idInfo;



            QY_MESSENGER_INFO msgrInfo = new QY_MESSENGER_INFO();
            QY_MESSENGER_REGINFO regInfo = new QY_MESSENGER_REGINFO();
            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en,
                "", ref idInfo, 0, ref msgrInfo, ref regInfo))
            {
                str = string.Format("dlgTalk.OnCreate failed, bGetMessengerInfoBySth failed, {0}", idInfo.ui64Id);
                qyFuncs.traceLog(str);
                qyFuncs.showInfo_open(0, null, str);
                //
                goto errLabel;
            }




            //
            m_var.addr.uiObjType = msgrInfo.uiType;

            //
            if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
            {
                IM_GRP_INFO grpInfo = new IM_GRP_INFO(); ;
                if (!pDbFuncs.pf_bGetImGrpInfoBySth(pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, "", ref idInfo, ref grpInfo))
                {
                    //qyFuncs.mymemset((IntPtr)(byte*)&grpInfo, 0, sizeof_IM_GRP_INFO));
                    grpInfo = new IM_GRP_INFO();
                }
                m_str_title = qyFuncs_Droid.get_grpInfo_name(ref grpInfo);// new string(grpInfo.name);
            }
            else
            {
                str = string.Format("{0}", qyFuncs_Droid.get_refInfo_syr(ref regInfo));// new string(regInfo.syr)); ;
                m_str_title = str;
            }


            //
            if (0 != qyFuncs_Droid.dlgTalk_OnInitDialog(this, ref m_var))
            {
                goto errLabel;
            }
            //
            mgui_onTimer(null, null, null);
            //setTimer(1000, gui_onTimer);

            //
            if (m_var.bActivityStarted_toDoTask)
            {
                doTask_av_accept();
            }

            //
            //
            //int iiii = m_var.tmp_hTools_dlgTalk_av.GetLength(0);


            //
            iErr = 0;
        errLabel:
            if (iErr != 0)
            {
                doFinish("dlgTalk.OnCreate failed");
            }

            //
            //
            int iii = 0;
            //int iii = m_var.tmp_hTools_dlgTalk_av.GetLength(0);
            qyFuncs.traceLog("dlgTalk.OnCreate(). ii " + iii);


            //
            return;
        }

        protected override void OnResume()
        {
            base.OnResume();
            //
            bool bDbg = false;
            int iii = 0;// m_var.tmp_hTools_dlgTalk_av.GetLength(0);
            if (bDbg)
            {
                qyFuncs_Droid.printTalkers("talk" + m_var.addr.idInfo.ui64Id + ": onResume enters, ii " + iii);
            }

            //
            CCtxQyMc pQyMc = g.g_pQyMc;

            //
            pQyMc.gui.hLastActiveTalk = this;

            //
            pQyMc.gui.hActiveWnd = this;

            //
            GuiShare.pf_gui_notify_chk();

            //
            iii = 0;// m_var.tmp_hTools_dlgTalk_av.GetLength(0);
            if (bDbg)
            {
                qyFuncs_Droid.printTalkers("talk" + m_var.addr.idInfo.ui64Id + ": onResume leaves ii " + iii);
            }

        }


        protected override void OnPause()
        {
            base.OnPause();
            //
            MyApplication myApplication = (MyApplication)(Application);
            string str;
            if (null != myApplication)
            {
                if (myApplication.mediaPlayer != null)
                {
                    if (myApplication.mediaPlayer.IsPlaying)
                    {
                        qyFuncs.traceLog("停止铃声");
                        myApplication.mediaPlayer.Pause();
                    }
                }

            }
            //
            qyFuncs.traceLog("dlgTalk: onPause");

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc.gui.hActiveWnd == this)
            {
                pQyMc.gui.hActiveWnd = null;
            }

            if (IsFinishing)
            {
                //
                str = string.Format("dlgTalk.OnPause,IsFinishing");
                if (m_var != null) str = string.Format("dlgTalk{0}.OnPause,IsFinishing", m_var.addr.idInfo.ui64Id);
                str += " enters";
                qyFuncs.showInfo_open(0, null, str);

                //
                killTimer("Act_dlgTalk.OnPause.947");

                //
                qyFuncs_Droid.dlgTalk_OnDestroy(this, ref m_var);

                //
                qyFuncs.showInfo_open(0, null, "dlgTalk.OnPause leaves,IsFinishing");
            }

        }

        public override void OnBackPressed()
        {
            Console.WriteLine("dlgTalk.OnBackPressed enters");

            //base.OnBackPressed();
            GuiShare.pf_PostMessage_new(this, Consts.CONST_qyWndContentType_talker, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);

            Console.WriteLine("dlgTalk.OnBackPressed leave");
        }

        //



        private void bindViews()
        {
#if false
            txt_msg = (TextView)FindViewById(Resource.Id.txt_msg);
            txt_detail = (TextView)FindViewById(Resource.Id.txt_detail);

            txt_msg.SetOnClickListener(this);
            txt_detail.SetOnClickListener(this);
#endif

            vPager = (ViewPager)FindViewById(Resource.Id.vpager);

            //
            vPager.Adapter = mAdapter;

            //vPager.SetCurrentItem(0,true);
            vPager.AddOnPageChangeListener(this);
            //
            //txt_msg.PerformClick();

            //
            vPager.SetCurrentItem(PAGE_ONE, false);
        }

        private void clearSelected()
        {
#if false
            txt_msg.Selected = false;
            txt_detail.Selected = false;
#endif

        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.txt_msg:
                    clearSelected();
                    //txt_msg.Selected = true;
                    vPager.SetCurrentItem(PAGE_ONE, false);
                    break;
#if false
                case Resource.Id.txt_detail:
                    clearSelected();
                    txt_detail.Selected = true;
                    vPager.SetCurrentItem(PAGE_TWO, false);
                    break;
#endif
                default:
                    break;
            }
        }

        protected override void OnDestroy()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            CookieBar.Dismiss(mainActivity);
            qyFuncs.traceLog("Activity_dlgTalk  OnDestroy");
            int i = 1;

            string str = "dlgTalk.OnDestroy";
            if (m_var != null)
            {
                str = string.Format("dlgTalk.OnDestroy {0}", m_var.addr.idInfo.ui64Id);
            }
            qyFuncs.showInfo_open(0, null, str + "enters");

            //
            killTimer("dlgTalk.OnDestroy");
            if (h != null)
            {
                h.RemoveCallbacksAndMessages(null);
            }
            //
#if false
            if (this.txt_msg != null)
            {
                this.txt_msg.Dispose();
            }
            if (this.txt_detail != null) {
                txt_detail.Dispose();
            }
#endif

            //
            if (this.vPager != null)
            {
                vPager.Adapter = null;
                vPager.Dispose();
            }
            if (this.mAdapter != null)
            {
                this.mAdapter.Dispose();
            }

            if (mHandler != null)
            {
                mHandler.RemoveCallbacksAndMessages(null);
                mHandler = null;
            }
            //
            base.OnDestroy();

            //
            //qyFuncs_Droid.doGC("dlgTalk.OnDestroy");

            //
            qyFuncs.showInfo_open(0, null, str + "leaves");

        }


        void ViewPager.IOnPageChangeListener.OnPageScrollStateChanged(int state)
        {
            //throw new NotImplementedException();
            if (state == 2)
            {
                switch (vPager.CurrentItem)
                {
                    case PAGE_ONE:
                        clearSelected();
                        //txt_msg.Selected = true;
                        break;
#if false
                    case PAGE_TWO:
                        clearSelected();
                        txt_detail.Selected = true;
                        break;
#endif
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
    }
}