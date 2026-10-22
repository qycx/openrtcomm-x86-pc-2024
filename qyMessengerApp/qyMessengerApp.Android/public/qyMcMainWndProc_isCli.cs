using Android.App;
using Android.Widget;
using Java.Lang;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static unsafe int initVar_onCreate_mainFrame(IntPtr p0, object hMainWnd, ref QY_MC_mainWndVar pVar)
        {
            int iErr = -1;

#if __DEBUG__
			traceLogA("initVar_onCreate_mainFrame enters.");
#endif

            //
            if (null == hMainWnd) return -1;

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (null == pQyMc) return -1;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            if (pProcInfo.m_iCtxType != Consts.CONST_ctxType_qmc) return -1;

            //
            pProcInfo.dwThreadId_gui = qyFuncs.myGetCurrentProcessId();
            //
            pVar.ctxCaller = new CTX_caller();
            pVar.ctxCaller.type = Consts.CONST_ctxCallerType_thread;
            pVar.ctxCaller.dwThreadId = qyFuncs.myGetCurrentThreadId();


            //
            {

                //
                pVar.pMsgBuf_msgArrive = new CMIS_MSGU();// (MIS_MSGU*)qyFuncs.mymalloc(sizeof(MIS_MSGU));    //  .pVar  =  pSci->pVar;
                if (null == pVar.pMsgBuf_msgArrive) goto errLabel;


            }


            //pVar->uiTimerId = SetTimer(hMainWnd, 1, 1000, NULL);
            GuiShare.pf_mainWnd_setTimer();

            iErr = 0;
        errLabel:

            if (0 != iErr) exitVar_onDestroy_mainFrame(p0, hMainWnd, ref pVar);

            return iErr;
        }

        public static unsafe int exitVar_onDestroy_mainFrame(IntPtr p0, object hMainWnd, ref QY_MC_mainWndVar pVar)
        {
            int iErr = -1;

            if (null == hMainWnd)
            {
                qyFuncs.traceLogA("exitVar_onDestroy_mainFrame failed: pMainWnd or pVar is null");
                return -1;
            }

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (null == pQyMc) return -1;


            qyFuncs.traceLogA("exitVar_onDestroy_mainFrame enters.");
            qyFuncs.showInfo_open(0, null, ("exitVar_onDestroy_mainFrame enters."));

            //
            //if (pVar->uiTimerId) KillTimer(hMainWnd, pVar->uiTimerId);
            GuiShare.pf_mainWnd_killTimer();


            //


            //
            if (pVar.pMsgBuf_msgArrive != null)
            {
                //qyFuncs.myfree((IntPtr)pVar.pMsgBuf_msgArrive); 
                pVar.pMsgBuf_msgArrive = null;
            }

            //
            qyFuncs.traceLogA("exitVar_onDestroy_mainFrame leaves.");
            qyFuncs.showInfo_open(0, null, ("exitVar_onDestroy_mainFrame leaves."));


            return 0;
        }



        //public static unsafe int tmpHandler_findMissingOrgReq(object p0, object pMsgTaskStatusParam, object pQElem)
        public static unsafe int tmpHandler_findMissingOrgReq(object p0, ref MIS_MSG_taskStatus pMsgTaskStatus, object pQElem)
        {
            int iRet = -1;
            //  p0;
            //MIS_MSG_taskStatus* pMsgTaskStatus = (MIS_MSG_taskStatus*)(IntPtr)pMsgTaskStatusParam;
            //
            //
            // 
            int index_taskInfo = (int)pQElem;
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            QY_MESSENGER_ID idInfo_logicalPeer;
            idInfo_logicalPeer.ui64Id = 0;

            //
#if DEBUG
            ref MIS_MSG_taskStatus tmp_pMsgTaskStatus = ref pMsgTaskStatus;
#endif


            /*
			BOOL bMatched = FALSE;

			if (pMsg->uiType == CONST_misMsgType_talkingFriend_qmc)
			{

				MIS_MSG_TALKINGFRIEND_QMC* pMem = &pMsg->talkingFriend_qmc;

				if (isTalkerShadowMgr(pMem->addr))
				{
					SendMessage(pMem->hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsgTaskStatus);
					if (pMsgTaskStatus->ucbFound)
					{
						iRet = 1; goto errLabel;
					}
				}

			}
			*/
            //if (!qyFuncs.qmcTaskInfo_bAlive(mainActivity.var_common.ctxCaller,pProcInfo, index_taskInfo)) goto errLabel;
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "tmpHandler_findMissingOrgReq");
            if (pTaskInfo.bUsed == false) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            fixed (MIS_MSGU* tmp_pMsg = &pTaskData.msgU)
            {
                if (tmp_pMsg->uiType != Consts.CONST_misMsgType_task) goto errLabel;
                IM_CONTENTU* pContent = (IM_CONTENTU*)&tmp_pMsg->task.data.buf;
                if (pContent->uiType != Consts.CONST_imCommType_transferAvInfo) goto errLabel;
                AV_TRAN_INFO* pAvTranInfo = &pContent->transferAvInfo.ass.tranInfo;
                //
                if (pMsgTaskStatus.uiContentType == Consts.CONST_imCommType_transferAudioData
                    && pMsgTaskStatus.uiTranNo == pAvTranInfo->audio.uiTranNo_openAvDev)
                {
                    idInfo_logicalPeer.ui64Id = tmp_pMsg->task.addr_logicalPeer.idInfo.ui64Id;
                    //
                    iRet = 1; goto errLabel;
                }
                if (pMsgTaskStatus.uiContentType == Consts.CONST_imCommType_transferVideoData
                    && pMsgTaskStatus.uiTranNo == pAvTranInfo->video.uiTranNo_openAvDev)
                {
                    idInfo_logicalPeer.ui64Id = tmp_pMsg->task.addr_logicalPeer.idInfo.ui64Id;
                    //
                    iRet = 1; goto errLabel;
                }
            }

            iRet = 0;
        errLabel:
            //
            if (iRet > 0)
            {
                string str;
                str = string.Format("err_missingOrgReq: taskInfo {0} found, from {1}, peer {2}. ", index_taskInfo, pMsgTaskStatus.idInfo_taskSender.ui64Id, idInfo_logicalPeer.ui64Id);
                if (logCount < 10)
                {
                    ++logCount;
                }
                else
                {
                    logCount = 0;
                    qyFuncs.showInfo_open(0, null, str);
                }
            }

            //
            return iRet;
        }

        //
        public static unsafe int sizeof_MIS_MSGU()
        {
            return sizeof(MIS_MSGU);
        }


        public static unsafe int sizeof_IM_GRP_INFO()
        {
            return sizeof(IM_GRP_INFO);
        }

        public static unsafe int sizeof_TASK_PROC_REQ()
        {
            return sizeof(TASK_PROC_REQ);
        }

        //
        public static unsafe int qGetMsg(GENERIC_Q pQ, ref MIS_MSGU pMsgU, ref uint puiSize1)
        {
            fixed( MIS_MSGU  *  p = &pMsgU )
            {
                fixed (uint* puiSize = &puiSize1)
                {
                    return qyFuncs.qGetMsg(pQ, (IntPtr)p, puiSize);
                }
            }

        }

        public static unsafe string get_grpInfo_name(ref IM_GRP_INFO grpInfo)
        {
            fixed (char* name = grpInfo.name)
            {
                return new string(name);
            }
        }
        //m_str_title = new string (grpInfo.name);

        public static unsafe string get_refInfo_syr(ref QY_MESSENGER_REGINFO regInfo)
        {
            fixed (char* syr = regInfo.syr)
            {
                return new string(syr);
            }
        }


        //
        static int logCountA = 0;
        static int logCount = 0;

        static int logCatB = 0;
        public static   int mainWnd_OnQyPostComm(object hMainWnd, ref QY_MC_mainWndVar var, int wParam, int lParam)
        {
            int lRet = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (null == pQyMc) return -1;
            CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            if (pProcInfo.m_iCtxType != Consts.CONST_ctxType_qmc) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            bool bDbg = false;
            string str;

#if DEBUG
            //bDbg = true;

            //
            if (bDbg)
            {
                qyFuncs_Droid.printTalkers("mainWnd_OnQyPostComm enters");
            }
#endif

            //  char				buf[1024]		=	"";
            uint len;
            int i;

            //
            if (pQyMc.bGuiQuit)
            {
                qyFuncs.showInfo_open(0, null, ("mainWnd_OnQyPostComm failed, bGuiQuit is true"));
                return -1;
            }
            if (pQyMc.bPrepareToGuiQuit)
            {
                qyFuncs.showInfo_open(0, null, ("mainWnd_OnQyPostComm failed, bPrepareToGuiQuit is true"));
                return -1;
            }

            //
            if (bDbg)
            {
                //qyFuncs.showInfo_open(0, null, "mainWnd_OnQyPostComm enters");
                //qyFuncs.showAllTc(mainActivity.var_common.ctxCaller, "mainWnd_OnQyPostComm enters");
            }

            //
            switch (wParam)
            {
                /*
				case CONST_qyWmParam_setActiveWindow:
					if (lParam)  ::PostMessage((HWND)lParam, CONST_qyWm_postComm, wParam, 0);
					break;
				*/
                case Consts.CONST_qyWmParam_msgArrive:
                    {
                        //  CQyMalloc			mallocObj;
                        ref MIS_MSGU pMsgU = ref var.pMsgBuf_msgArrive.msgU; //  (  MIS_MSGU  *  )mallocObj.malloc(  sizeof(  MIS_MSGU  )  );
                        //fixed (MIS_MSGU* pMsg = &pMsgU)
                        {
                            //if (null == pMsg) goto errLabel;

                            for (; !pQyMc.bQuit;)
                            {
                                //MACRO_memset_misMsg(pMsg);
                                len = (uint)sizeof_MIS_MSGU();
                                //if (0 != qyFuncs.qGetMsg(pQyMc.gui.processQ, (IntPtr)pMsg, &len)) break;
                                if (0 != qGetMsg(pQyMc.gui.processQ, ref pMsgU, ref len)) break;
                                switch (pMsgU.uiType)
                                {
                                    case Consts.CONST_misMsgType_talk:
                                    case Consts.CONST_misMsgType_task:
                                        {
                                            object hWnd = null;
                                            MSGR_ADDR addr_logicalPeer;
                                            bool bNeedNotShowWnd = false;

                                            //qyFuncs.mymemset((IntPtr)(byte*)&addr_logicalPeer, 0, sizeof(MSGR_ADDR));
                                            addr_logicalPeer = new MSGR_ADDR();
                                            if (pMsgU.uiType == Consts.CONST_misMsgType_talk)
                                            {
                                                //
                                                if (null == (pMisCnt = (MIS_CNT)pProcInfo.pMisCnt)) goto errLabel;
                                                //
                                                if (0 != qyFuncs.myGetPeer("", ref pMsgU.talk.data.route, ref pMsgU.talk.addr_logicalPeer, ref addr_logicalPeer)) goto errLabel;

                                                //										
                                                //IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->talk.data.buf;
                                                ref IM_CONTENTU tmp_pContent = ref pMsgU.talk.data.buf;

                                                //  2017/07/17
                                                if (tmp_pContent.uiType == Consts.CONST_imCommType_htmlContent)
                                                {
                                                    /*
                                                    if (pProcInfo->avConsole.bGotMsg_avConsole)
                                                    {
                                                        //
                                                        __declspec(dllexport)  int doAvConsole_cmd_recvMsg(__int64 ui64Id_from, __int64 ui64Id_to, TCHAR* tTxt);

                                                        doAvConsole_cmd_recvMsg(pMsg->talk.data.route.idInfo_from.ui64Id, pMsg->talk.data.route.idInfo_to.ui64Id, pContent->html.wBuf);
                                                        //
                                                        showInfo_open(0, 0, _T("avConsole: bGotMsg_avConsole is true, doAvConsole_cmd_recvMsg called"));
                                                        //
                                                        break;
                                                    }
                                                    */
                                                    GuiShare.pf_gui_procMsg(ref pMsgU);
                                                }


                                            }
                                            else if (pMsgU.uiType == Consts.CONST_misMsgType_task)
                                            {
                                                //
                                                if (null == (pMisCnt = (MIS_CNT)pProcInfo.pMisCnt)) goto errLabel;
                                                //
                                                if (0 != qyFuncs.myGetPeer("", ref pMsgU.task.data.route, ref pMsgU.task.addr_logicalPeer, ref addr_logicalPeer)) goto errLabel;

                                                //
                                                //IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->task.data.buf;
                                                ref IM_CONTENTU tmp_pContent = ref pMsgU.task.data.buf;

                                                //
#if DEBUG
                                                if (tmp_pContent.uiType == Consts.CONST_imCommType_imGrp)
                                                {
                                                    int ii = 0;
                                                }
                                                if (tmp_pContent.uiType == Consts.CONST_imCommType_confLayout)
                                                {
                                                    int ii = 0;
                                                }
#endif

                                                //  2009/09/07
                                                /*
                                                if (pMsg->task.uiTaskType == CONST_imTaskType_viewDynBmp
                                                    || pMsg->task.uiTaskType == CONST_imTaskType_viewGps)
                                                {
                                                    //  2014/09/28			
                                                    if (pContent->uiType == CONST_imCommType_transferAvInfo)
                                                    {
                                                        if (pContent->transferAvInfo.idInfo_logicalPeer.ui64Id)
                                                        {
                                                            if (findTalker(pQyMc, pMisCnt, &pContent->transferAvInfo.idInfo_logicalPeer, &hWnd))
                                                            {
    # if  __DEBUG__
                                                                traceLog(_T("mainWnd_OnPostComm: retrieveDynBmps conf findTalker failed"));
    #endif
                                                                goto errLabel;
                                                            }
                                                               ::SendMessage(hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                            break;
                                                        }
                                                    }

                                                    //
                                                    if (!IsWindow(pProcInfo->hWnd_shareDynBmps))
                                                    {
                                                        traceLogA("Not finished. hWnd_sharedWebcams is not valid");
                                                        break;
                                                    }
                                                       ::SendMessage(pProcInfo->hWnd_shareDynBmps, CONST_qyWm_comm, 0, (LPARAM)pMsg);

                                                    break;
                                                }
                                                */

                                                //  2015/06/06
                                                /*
                                                if (pContent->uiType == CONST_imCommType_ptzControlReq
                                                    && pContent->ptzControlReq.ucbReply)
                                                {
                                                    if (findTalker(pQyMc, pMisCnt, &addr_logicalPeer.idInfo, &hWnd))
                                                    {
    # ifdef  __DEBUG__
                                                        traceLog(_T("mainWnd_OnPostComm: reply for ptzControlReq, findTalker failed"));
    #endif
                                                        goto errLabel;
                                                    }
                                                       ::SendMessage(hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                    break;
                                                }

                                                //  2012/08/08
                                                if (pContent->uiType == CONST_imCommType_ptzControlCmd)
                                                {
    # ifdef  __DEBUG__
                                                    traceLog(_T("mainWnd_OnPostComm: ptzControlCmd"));
    #endif
                                                    //  2016/07/20
                                                    if (pContent->ptzControlCmd.cmdInfo.obj.uiObjType == CONST_objType_rtspStream)
                                                    {
                                                        SendMessage(pProcInfo->hWnd_shareDynBmps, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                        break;
                                                    }

                                                    //
                                                    PARAM_viewLocalPtzControl param = { 0 };
                                                    //
                                                    doRemotePtzCmd(NULL, &pContent->ptzControlCmd, &param, &pProcInfo->ptzWnd);

                                                    break;
                                                }
                                                */
                                                //  2015/08/05
                                                if (tmp_pContent.uiType == Consts.CONST_imCommType_transferFileReq
                                                    && tmp_pContent.transferFileReq.usSubtype == Consts.CONST_procOfflineResSubtype_download)
                                                {
                                                    if (0 != GuiShare.pf_findTalker(pQyMc, pMisCnt, addr_logicalPeer.idInfo, ref hWnd))
                                                    {
#if __DEBUG__
													traceLog(_T("mainWnd_OnPostComm: transferFileReq download, findTalker failed"));
#endif
                                                        goto errLabel;
                                                    }

                                                    //									 
                                                    if (0 != GuiShare.pf_dlgTalk_qPostMsg(hWnd, ref pMsgU, len)) goto errLabel;


                                                    GuiShare.pf_PostMessage(hWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
                                                    //
                                                    break;
                                                }
                                                //
                                                if (0 == pMsgU.task.iTaskId)
                                                {

#if __DEBUG__
												traceLog(_T("mainWnd_OnQyPostComm: task.iTaskId is 0"));
#endif

                                                    //
                                                    bNeedNotShowWnd = true;
                                                }

                                            }
                                            else
                                                goto errLabel;

                                            //
#if false
										//
										hWnd = null;

										//
										if (null == hWnd)
										{

											if (0 != pProcInfo.tryToTalkToMessenger_any(addr_logicalPeer.idInfo.ui64Id, bNeedNotShowWnd, false, ref hWnd)) goto errLabel;

										}

										//									 
										/*
										if (0 != dlgTalk_qPostMsg(hWnd, (IntPtr)pMsg, len)) goto errLabel;
										GuiShare.pf_PostMessage(hWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
										*/
										GuiShare.pf_gui_dlgTalk_procMsg(hWnd, (IntPtr)pMsg);
#endif

                                        }
                                        break;

                                    case Consts.CONST_misMsgType_input:    //  2014/09/05
                                        {
                                            object hWnd = null;
                                            MSGR_ADDR addr_logicalPeer;

                                            //IM_CONTENTU* pContent = qyFuncs.M_getMsgContent(pMsg->input.uiType,pMsg->input.ucFlg, &pMsg->input.data);
                                            ref IM_CONTENTU tmp_pContent = ref pMsgU.input.data.buf;

                                            //
#if DEBUG
                                            if (tmp_pContent.uiType == Consts.CONST_imCommType_confKey)
                                            {
                                                int ii = 0;
                                            }
                                            if (tmp_pContent.uiType == Consts.CONST_imCommType_confLayout)
                                            {
                                                //ref IM_CONTENTU tmpContent = ref *pContent;
                                                //
                                                int ii = 0;

                                            }
                                            if (tmp_pContent.uiType == Consts.CONST_imCommType_confNvrInfo)
                                            {
                                                int ii = 0;
                                            }
                                            if (tmp_pContent.uiType == Consts.CONST_imCommType_statusConfLayoutParam)
                                            {
                                                int ii = 0;
                                            }
#endif

                                            //  2014/05/05
                                            bool bMsgProcessed = false;
                                            switch (tmp_pContent.uiType)
                                            {
                                                case Consts.CONST_imCommType_transferAvInfo:
                                                    bMsgProcessed = true;
                                                    //
                                                    mainWnd_procMsgInput_transferAvInfo(hMainWnd, ref var, ref pMsgU.input);

                                                    break;
                                                case Consts.CONST_imCommType_transferAvReplyInfo:
                                                    bMsgProcessed = true;
                                                    //
                                                    mainWnd_procMsgInput_transferAvReplyInfo(hMainWnd, ref var, ref pMsgU.input);
                                                    //
                                                    break;
                                                case Consts.CONST_imCommType_transferFileReq:
                                                    bMsgProcessed = true;
                                                    //
                                                    mainWnd_procMsgInput_transferFileReq(hMainWnd, ref var, ref pMsgU.input);
                                                    //
                                                    break;

                                                /*
                                            case Consts.CONST_imCommType_htmlContent:  //  2014/07/12
                                                bMsgProcessed = true;

                                                if (!(isUcFlgTalkData(pMsg->input.ucFlg) || isUcFlgRouteTalkData(pMsg->input.ucFlg)))
                                                {
    #if __DEBUG__
                                                    traceLog(_T("mainWnd_OnPostComm: error: resp html not from others"));
    #endif
                                                    goto errLabel;
                                                }
                                                if (isUcFlgResp(pMsg->input.ucFlg))
                                                {
                                                    if (findTalker(pQyMc, pMisCnt, &pMsg->input.data.route.idInfo_from, &hWnd)) goto errLabel;
                                                       ::SendMessage(hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                    break;
                                                }
    #if __DEBUG__
                                                traceLog(_T("Err: request html is not processed here"));
    #endif
                                                break;
                                                */
                                                case Consts.CONST_imCommType_taskProcReq:  //  2014/10/09

                                                    object hTalk = null;
                                                    if (0 == findTalker(pQyMc, pMisCnt, pMsgU.input.addr_logicalPeer.idInfo, ref hTalk))
                                                    {

                                                        bMsgProcessed = true;
                                                        //
                                                        //::SendMessage(pContent->taskProcReq.hWnd_task, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                        dlgTalk_OnQyComm(hTalk, 0, ref pMsgU);

                                                    }
                                                    break;
                                                /*
                                            case Consts.CONST_imCommType_procOfflineRes:  //  2015/08/03
                                                bMsgProcessed = TRUE;

                                                //  2015/06/05
    #if __DEBUG__
                                                traceLog(_T("mainWnd_OnPostComm: get procOfflineRes from %I64u, bResp %d"), pMsg->input.data.route.idInfo_from.ui64Id, pContent->procOfflineResU.common.ucbResp);
    #endif

                                                //
                                                if (!(isUcFlgTalkData(pMsg->input.ucFlg) || isUcFlgRouteTalkData(pMsg->input.ucFlg)))
                                                {
    #if __DEBUG__
                                                    traceLog(_T("mainWnd_OnPostComm: error: req retrieveDynBmps not from others"));
    #endif
                                                    goto errLabel;
                                                }
                                                //
                                                if (pContent->procOfflineResU.common.usSubtype == CONST_procOfflineResSubtype_download)
                                                {
                                                    if (pProcInfo->talkToMessenger(pMsg->input.data.route.idInfo_from.ui64Id, FALSE, FALSE, &hWnd))
                                                    {
    #if __DEBUG__
                                                        traceLog(_T("mainWnd_OnPostComm: procOfflineRes req talkToMessenger failed"));
    #endif
                                                        goto errLabel;
                                                    }
                                                }
                                                else
                                                {
                                                    if (findTalker(pQyMc, pMisCnt, &pMsg->input.data.route.idInfo_from, &hWnd))
                                                    {
    # ifdef  __DEBUG__
                                                        traceLog(_T("mainWnd_OnPostComm: procOfflineRes req findTalker failed"));
    #endif
                                            goto errLabel;
                                                    }
                                                }
                                                   //
                                                   ::SendMessage(hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);

                                                //												   
                                                break;
                                            case CONST_imCommType_imGrp:  //  2017/07/26
                                                bMsgProcessed = TRUE;
                                                //
                                                if ((isUcFlgTalkData(pMsg->input.ucFlg) || isUcFlgRouteTalkData(pMsg->input.ucFlg)))
                                                {
                                                    //  说明时从其他的客户端发过来，通知临时组变化的。直接发到 dlgAvRes_mem_hzj上去显示分组成员。
                                                    //traceLog(  _T(  "not finished. need to send to dlgAvRes_mem_hzj"  )  );
                                                    //
                                                    if (!IsWindow(pProcInfo->hWall_hzj))
                                                    {
                                                        traceLog(_T("hWall_hzj is invalid. imGrp can't be processed"));
                                                        break;
                                                    }
                                                       //
                                                       ::SendMessage(pProcInfo->hWall_hzj, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                    //
                                                    break;
                                                }

                                                //
                                                hWnd = pProcInfo->cfgGrp.hWnd_cfgGrp;
                                                //
                                                if (IsWindow(hWnd))
                                                {
                                                       ::SendMessage(hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                    break;
                                                }
                                                //
                                                notifyProgressEnd(pMsg->input.uiTranNo, 0);

                                                //
                                                break;
                                                */

                                                default:
                                                    break;
                                            }
                                            if (bMsgProcessed) break;

                                            //
                                            if (0 == pMsgU.input.addr_logicalPeer.idInfo.ui64Id)
                                            {
                                                /*
                                                switch (pContent->uiType)
                                                {
                                                    case Consts.CONST_imCommType_refreshDynBmps:
                                                    case Consts.CONST_imCommType_verifyViewDynBmp:
                                                           ::SendMessage(pProcInfo->hWnd_shareDynBmps, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                        break;
                                                    case Consts.CONST_imCommType_retrieveCustomerServiceObjListReq:
                                                        //
    # if  __DEBUG__
                                                        traceLog(_T("Not finished, CONST_imCommType_retrieveCustomerServiceObjListReq"));
                                                        for (i = 0; i < pContent->retrieveCustomerServiceObjListReq.usCnt; i++)
                                                        {
                                                            traceLog(_T("mems[%d]: %I64u, %s"), i, pContent->retrieveCustomerServiceObjListReq.mems[i].idInfo.ui64Id, pContent->retrieveCustomerServiceObjListReq.mems[i].displayName);
                                                        }
    #endif
                                                        //
                                                        for (i = 0; i < mycountof(pProcInfo->customerService.objs); i++)
                                                        {
                                                            if (i < pContent->retrieveCustomerServiceObjListReq.usCnt)
                                                            {
                                                                memcpy(&pProcInfo->customerService.objs[i], &pContent->retrieveCustomerServiceObjListReq.mems[i], sizeof(pProcInfo->customerService.objs[i])  );

                                                            }
                                                                else
                                                            {
                                                                        memset(&pProcInfo->customerService.objs[i], 0, sizeof(pProcInfo->customerService.objs[i])  );
                                                            }
                                                        }
                                                        PostMessage(pQyMc->gui.hMainWnd, WM_COMMAND, MAKEWPARAM(ID_refreshLayout, 0), 0);
                                                        break;
                                                default:
                                                             TCHAR tBuf[128];
                                                            _sntprintf(tBuf, mycountof(tBuf), _T("mainWnd_OnPostComm: msgInput, unprocessed content.type %d"), pContent->uiType);
                                                            showInfo_open(0, 0, tBuf);
                                                            break;
                                                }
                                                */
                                                break;
                                            }

                                            //
                                            //qyFuncs.mymemset((IntPtr)(byte*)&addr_logicalPeer, 0, sizeof(MSGR_ADDR));
                                            addr_logicalPeer = new MSGR_ADDR();
                                            //
                                            //qyFuncs.mylstrcpyn(addr_logicalPeer.misServName1, "", Consts.CONST_maxMisServNameLen + 1);
                                            //
                                            addr_logicalPeer.idInfo.ui64Id = pMsgU.input.addr_logicalPeer.idInfo.ui64Id;        //  ÔÚÈ¡webMessengerµÄ×´Ì¬Ê±£¬»áÓÃµ½Õâ¸öÀàÐÍ¡£
                                                                                                                                //  Ã»ÓÐ¶ÀÁ¢Æô¶¯´°¿ÚµÄÄÜÁ¦£¬µ«ÊÇ¿ÉÒÔ·¢ÏÖ´°¿Ú£¬È»ºó½«ÏûÏ¢ËÍµ½´°¿ÚÄÇÀï¡£


                                            hWnd = null;

                                            //  2013/09/09

                                            //
                                            if (null == hWnd)
                                            {
                                                //  2014/01/14
                                                bool bNeedNotShowWnd = false;
                                                if (0 != pProcInfo.tryToTalkToMessenger_any(addr_logicalPeer.idInfo.ui64Id, bNeedNotShowWnd, false, ref hWnd))
                                                {
                                                    str = string.Format("mainWnd_onQyPostComm. trytoTalkToMessenger_any {0} failed", addr_logicalPeer.idInfo.ui64Id);
                                                    qyFuncs.traceLog(str);
                                                    //
                                                    goto errLabel;
                                                }
                                            }

                                            //::SendMessage(hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                            GuiShare.pf_gui_dlgTalk_procMsg(hWnd, ref pMsgU);

                                        }

                                        break;



                                    case Consts.CONST_misMsgType_taskStatus:
                                        {
                                            object hWnd = null;
                                            MSGR_ADDR addr_logicalPeer;
                                            //  QY_MESSENGER_ID			idInfo_peer;


                                            if (pMsgU.taskStatus.ucbNotification)
                                            {
                                                displayNotification(pProcInfo, ref pMsgU.taskStatus);
                                                break;
                                            }

                                            int ii = 0;

                                            //
                                            if (pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_canceledByReceiver)
                                            {
                                                int iii = 0;
                                            }


                                            //
                                            if (pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_canceledBySender
                                            || pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_canceledByReceiver)
                                            {
                                                int jj = 0;
                                                int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(mainActivity.var_common.ctxCaller, pProcInfo, pMsgU.taskStatus.iTaskId);
                                                if (index_taskInfo < 0)
                                                {
                                                    goto errLabel;
                                                }
                                                QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "mainWnd_onqyPostComm");
                                                if (pTaskInfo == null) goto errLabel;
                                                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                                                if (pTaskData == null) goto errLabel;
                                                //
                                                if (pTaskData.msgU.uiType != Consts.CONST_misMsgType_task) goto errLabel;
                                                if (pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_canceledBySender)
                                                {
                                                    if (pTaskData.msgU.task.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                                                    {
                                                        goto errLabel;
                                                    }
                                                }
                                                //
                                                do_closeTask1(pTaskInfo.var.iTaskId, true, "");
                                                //
                                                break;
                                            }


                                            //qyFuncs.mymemset((IntPtr)(byte*)&addr_logicalPeer, 0, sizeof(MSGR_ADDR));
                                            addr_logicalPeer = new MSGR_ADDR();
                                            //lstrcpyn(addr_logicalPeer.misServName, pMisCnt->cfg.misServName, mycountof(addr_logicalPeer.misServName));
                                            addr_logicalPeer.idInfo.ui64Id = pMsgU.taskStatus.idInfo_logicalPeer.ui64Id;


                                            //
                                            bool bNeeded_applyForOrgReq = false;

                                            if (pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_err_cantGet_index_activeMems_from)
                                            {
                                                bNeeded_applyForOrgReq = true;
                                            }
                                            else
                                            {

                                                if (pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_err_missingOrgReq)
                                                {       //  
                                                    /*
                                                qTraverse(&pMisCnt->talkingFriendQ, tmpHandler_findMissingOrgReq, 0, pMsg);

                                                //
                                                if (pMsg->taskStatus.ucbFound)
                                                {
                                                    addr_logicalPeer.idInfo.ui64Id = pMsg->taskStatus.idInfo_logicalPeer.ui64Id;
                                                }
                                                    */
                                                    for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
                                                    {
                                                        if (tmpHandler_findMissingOrgReq(null, ref pMsgU.taskStatus, i) > 0)
                                                        {
                                                            //
                                                            //str = string.Format("err_missingOrgReq: taskInfo {0} found", i);
                                                            //qyFuncs.showInfo_open(0,null,str);



                                                            //
                                                            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, i, "");
                                                            if (pTaskInfo == null) goto errLabel;
                                                            if (!pTaskInfo.bUsed) goto errLabel;
                                                            pTaskInfo.var.dwTickCnt_recv_lastRefreshed1 = qyFuncs.myGetTickCount(null);
                                                            //
                                                            {
                                                                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                                                                if (pTaskData == null) goto errLabel;

                                                                //
                                                                MSGR_ADDR addr = pTaskData.msgU.task.addr_logicalPeer;

                                                                //
                                                                //
                                                                pMsgU.taskStatus.idInfo_logicalPeer = addr.idInfo;
                                                                pMsgU.taskStatus.iTaskId = pTaskInfo.var.iTaskId;


                                                                //
                                                                //fixed (MIS_MSGU* tmp_pMsg = &pTaskData.msgU)
                                                                ref MIS_MSGU tmp_pMsg = ref pTaskData.msgU;
                                                                {
                                                                    if (tmp_pMsg.uiType != Consts.CONST_misMsgType_task) goto errLabel;

                                                                    //
                                                                    bool bMeInitiator = false;
                                                                    //IM_CONTENTU* pContent = qyFuncs.M_getMsgContent(tmp_pMsg.task.uiType,tmp_pMsg.task.ucFlg, ref tmp_pMsg.task.data);
                                                                    ref IM_CONTENTU pContent = ref tmp_pMsg.task.data.buf;
                                                                    ref IM_CONTENTU tmp_pContent = ref pContent;
                                                                    if (pContent.uiType == Consts.CONST_imCommType_transferAvInfo)
                                                                    {
                                                                        if (pContent.transferAvInfo.confCfg.idInfo_initiator.ui64Id == pMisCnt.idInfo.ui64Id)
                                                                        {
                                                                            bMeInitiator = true;
                                                                        }

                                                                    }

                                                                    //
                                                                    if (logCountA < 10)
                                                                    {
                                                                        ++logCountA;
                                                                    }
                                                                    else
                                                                    {
                                                                        logCountA = 0;

                                                                        str = "err_missing: it is msgTask. status is " + tmp_pMsg.task.iStatus + " peer：" + addr.idInfo.ui64Id;
                                                                        qyFuncs.showInfo_open(0, "", str);

                                                                    }
                                                                    //


                                                                    //
                                                                    bool bTryToRemove = false;
                                                                    int maxWaits = 48;
                                                                    //
#if DEBUG
                                                                    if (pProcInfo.cfg.debugStatusInfo.bNoRemove_err_missing)
                                                                    {
                                                                        //maxWaits = 24;
                                                                    }
#endif

                                                                    //
                                                                    if (tmp_pMsg.task.iStatus == Consts.CONST_imTaskStatus_autoCanceledByReceiver)
                                                                    {
                                                                        //  qyFuncs.traceLog("##795");
                                                                        bTryToRemove = true;

                                                                        //
                                                                        qyFuncs.showInfo_open(0, "", "err_missing: autoCanceled, so set bTryToRemove to true. peer:" + addr.idInfo.ui64Id);

                                                                    }
                                                                    //  
                                                                    if (qyFuncs.bTaskAlive(tmp_pMsg.task.iStatus)
                                                                        //&&  tmp_pMsg->task.iStatus  !=  Consts.CONST_imTaskStatus_resp   
                                                                        )
                                                                    {
                                                                        uint dwTickCnt = qyFuncs.myGetTickCount(null);
                                                                        if (0 == pTaskInfo.var.tmpErrMissing.dwTickCnt_startToCalc)
                                                                        {
                                                                            pTaskInfo.var.tmpErrMissing.dwTickCnt_startToCalc = dwTickCnt;
                                                                            pTaskInfo.var.tmpErrMissing.nTimes_errMissing = 0;
                                                                            //
                                                                            qyFuncs.showInfo_open(0, "", "err_missing: tickCnt_start is 0");
                                                                        }
                                                                        else
                                                                        {
                                                                            int iDiffInMs = (int)(dwTickCnt - pTaskInfo.var.tmpErrMissing.dwTickCnt_startToCalc);
                                                                            int maxWaitsInMs = 5000;    //  3000
                                                                            if (System.Math.Abs(iDiffInMs) > maxWaitsInMs)
                                                                            {
                                                                                qyFuncs.showInfo_open(0, "", "err_missing: iDiffInMs > " + maxWaitsInMs + ". nTimes " + pTaskInfo.var.tmpErrMissing.nTimes_errMissing + " reset");
                                                                                //
                                                                                pTaskInfo.var.tmpErrMissing.dwTickCnt_startToCalc = dwTickCnt;
                                                                                pTaskInfo.var.tmpErrMissing.nTimes_errMissing = 0;
                                                                                //

                                                                            }
                                                                        }
                                                                        pTaskInfo.var.tmpErrMissing.nTimes_errMissing++;
                                                                        //
                                                                        if (pTaskInfo.var.tmpErrMissing.nTimes_errMissing >= maxWaits
                                                                            && !bMeInitiator)
                                                                        {
                                                                            //  这表明任务不完整

                                                                            bTryToRemove = true;

                                                                            //
                                                                            qyFuncs.showInfo_open(0, "", "err_missing: nTimes_err: " + pTaskInfo.var.tmpErrMissing.nTimes_errMissing + " so set bTryToRemove to true");


                                                                            //
                                                                            if (logCountC < maxWaits)
                                                                            {
                                                                                ++logCountC;
                                                                            }
                                                                            else
                                                                            {
                                                                                logCountC = 0;
                                                                                qyFuncs.showInfo_open(0, "", "err_missing: nTimes >= " + maxWaits);
                                                                            }

                                                                        }
                                                                    }
                                                                    //
                                                                    if (bTryToRemove)
                                                                    {
                                                                        if (logCountD < maxWaits)
                                                                        {
                                                                            ++logCountD;
                                                                        }
                                                                        else
                                                                        {
                                                                            logCountD = 0;
                                                                            qyFuncs.showInfo_open(0, "", "bTryToRemove err_missing 880");

                                                                        }
                                                                        //
                                                                        int iTaskId_av = 0;
                                                                        QY_MESSENGER_ID idInfo_a; idInfo_a.ui64Id = 0;
                                                                        //  if (!bExists_activeTaskAv(ref iTaskId_av, ref idInfo_a))
                                                                        //  {

                                                                        object hTalk = null;
                                                                        if (GuiShare.pf_findTalker(pQyMc, pMisCnt, addr.idInfo, ref hTalk) == 0
                                                                            && qyFuncs_Droid.bDlgTalkActive(hTalk)
                                                                            )
                                                                        {

                                                                            Activity_dlgTalk activity_DlgTalk = (Activity_dlgTalk)hTalk;

                                                                            //
                                                                            qyFuncs.showInfo_open(0, "", "after findTalker.bTalkActive, bPrepare: " + activity_DlgTalk.m_var.av.taskInfo.bPrepare + " peer: " + addr.idInfo.ui64Id);

                                                                            //
                                                                            uint currentTickCount = qyFuncs.myGetTickCount(null);
                                                                            //  currentTickCount - activity_DlgTalk.m_var.tickCountOnCreate > 1500 ||
                                                                            if (!activity_DlgTalk.m_var.av.taskInfo.bPrepare)
                                                                            {

                                                                                qyFuncs.showInfo_open(0, "", "talker is active. bNeedRemoved_toRecover set to true");


                                                                                pTaskInfo.var.bNeedRemoved_toRecover = true;

                                                                                //
                                                                                pProcInfo.m_bNeedToRemoveTaskAtOnce = true;
                                                                            }



                                                                        }
                                                                        //  }

                                                                    }

                                                                }
                                                            }
                                                            //
                                                            break;
                                                        }
                                                    }
                                                    if (i == pProcInfo.cfg.usMaxCnt_taskInfos)
                                                    {
                                                        bNeeded_applyForOrgReq = true;

                                                    }
                                                }

                                                //
                                                GuiShare.pf_gui_procMsg(ref pMsgU);

                                            }


                                            if (bNeeded_applyForOrgReq)
                                            {

                                                //
                                                //TCHAR tBuf[128];
                                                //
                                                TASK_PROC_REQ taskProcReq;
                                                int lenInBytes;
                                                //qyFuncs.mymemset((IntPtr)(byte*)&taskProcReq, 0, sizeof(TASK_PROC_REQ));
                                                taskProcReq = new TASK_PROC_REQ();
                                                taskProcReq.uiType = Consts.CONST_imCommType_taskProcReq;
                                                taskProcReq.usOp = Consts.CONST_imOp_recv_applyForOrgReq;
                                                //  taskProcReq.tStartTime_org  =  0;
                                                taskProcReq.uiTranNo_org = pMsgU.taskStatus.uiTranNo;
                                                //  taskProcReq.uiSerialNo_org  =  0;
                                                taskProcReq.uiContentType_org = pMsgU.taskStatus.uiContentType;
                                                //	
                                                lenInBytes = sizeof_TASK_PROC_REQ();
                                                //				
                                                //				
                                                //				
                                                Int64 tStartTran; uint uiTranNo;
                                                qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);

                                                //  2017/10/11
                                                if (pMsgU.taskStatus.iStatus == Consts.CONST_imTaskStatus_err_cantGet_index_activeMems_from)
                                                {
                                                    str = _T("cantGet_index_activeMems_from. ");
                                                }
                                                else
                                                {
                                                    str = _T("taskAvOrgReq not found. ");
                                                }
                                                //
                                                str += string.Format("send taskProcReq to {0}. tn {1}. kkkk", pMsgU.taskStatus.idInfo_taskSender.ui64Id, uiTranNo);
#if DEBUG
                                                if (logCatB < 6)
                                                {
                                                    ++logCatB;
                                                }
                                                else
                                                {
                                                    qyFuncs.traceLog(str);
                                                    logCatB = 0;

                                                }

#endif
                                                qyFuncs.showInfo_open(0, null, str);

                                                //
                                                if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, pMsgU.taskStatus.cmdCode, tStartTran, uiTranNo, 0, 0, 0, ref taskProcReq, (uint)lenInBytes, addr_logicalPeer.idInfo.ui64Id, 0, 0, pMsgU.taskStatus.idInfo_taskSender.ui64Id, 0, false)) goto errLabel;

                                                //  2017/10/03
                                                pProcInfo.status.dwTickCnt_sendTo_applyForOrgReq = qyFuncs.myGetTickCount(null);

                                            }




                                        }
                                        break;
                                    /*
                                                               case  CONST_misMsgType_refreshContactsStatus_qmc:
                                                               case  CONST_misMsgType_refreshMeStatus_qmc:
                                {
                                                                     //  QY_WMBUF
                                                                     ::SendMessage(pMsg->refreshContactsStatus_qmc.hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                }
                                break;
                                                               case  CONST_misMsgType_getDaemonStatus:

                                                                     ::SendMessage(pMsg->getDaemonStatus.hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                break;
                                    */

                                    case Consts.CONST_misMsgType_applyForPlayer_qmc:
                                        pProcInfo.doApplyForPlayer(hMainWnd, ref pMsgU);
                                        break;
                                    /*

                                                                     //  2011/12/10
                                                               case  CONST_misMsgType_notifyTaskEnd_qmc:

                                                                     ::SendMessage(pMsg->notifyTaskEnd.hWnd, CONST_qyWm_comm, 0, (LPARAM)pMsg);
                                                                    break;
                                                                */

                                    default:
                                        //traceLogA("CMainFrame::OnQyPostComm: msgArrive:  unprocessed: msgType: %d", pMsg->uiType);
                                        break;
                                }
                            }
                        }
                    }
                    break;

                case Consts.CONST_qyWmParam_task_end:
#if __DEBUG__
traceLog(_T("CMainFrame: task end, to tryToFreeSharedObjs"));
#endif
                    qyFuncs.tryToFreeSharedObjs(pProcInfo);
                    break;

                case Consts.CONST_qyWmParam_chkTask:
#if __DEBUG__
traceLog(_T("CMainFrame: to chk task"));
#endif
                    qyFuncs.qmcChkTasks_gui(pProcInfo);
                    break;

                case Consts.CONST_qyWmParam_toStartLocalAudioPlayer:      //  2013/05/26
#if __DEBUG__
traceLog(_T("MainFrm: get Msg toStartLocalAudioPlayer"));
#endif
                    //startLocalAudioPlayer(pProcInfo);
                    break;
                case Consts.CONST_qyWmParam_showHint:
                    {
                        Toast.MakeText(mainActivity, "修改密码成功", ToastLength.Long).Show();
                    }
                    break;

                default:
#if __DEBUG__
							traceLog(_T("mainWnd_OnQyPostComm: unknown wParam"));
#endif
                    break;
            }


            lRet = Consts.QY_RC_WMOK;
        errLabel:

            //
            if (bDbg)
            {
                //qyFuncs.showInfo_open(0, null, "mainWnd_OnQyPostComm leaves");
                //qyFuncs.showAllTc(mainActivity.var_common.ctxCaller, "mainWnd_OnQyPostComm leaves");

                qyFuncs_Droid.printTalkers("mainWnd_OnQyPostComm leaves");

            }

            return lRet;
        }


        //
        static int logCountC = 0;
        static int logCountD = 0;

        //
        public static unsafe int mainWnd_showStatus(ref QY_MC_mainWndVar var)
        {
            //
            showState_sys();
            //
            showState_players();

            //
            //test_showState();

#if true
            if (0 == (var.loopCtrl % 3))
            {
                showState_tasks(ref var);
                showState_talks();
                showState_qs();
            }
#endif

            //
            return 0;
        }

        //
        //
        private static ActivityManager.MemoryInfo getAvailableMemory()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            //
            ActivityManager activityManager = (ActivityManager)mainActivity.GetSystemService(Activity.ActivityService);
            ActivityManager.MemoryInfo memoryInfo = new ActivityManager.MemoryInfo();
            activityManager.GetMemoryInfo(memoryInfo);
            return memoryInfo;
        }

        public static unsafe int showState_sys()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            int iPos = 0;

            //
            mySYSTEMTIME when;
            qyFuncs.myGetLocalTime(out when);

            Runtime rt = Runtime.GetRuntime();
            long javaMaxHeapSize = rt.MaxMemory();
            long javaTotalHeapSize = rt.TotalMemory();
            long javaFreeHeapSize = rt.FreeMemory();
            //
            long nativeHeap = Android.OS.Debug.NativeHeapSize;
            long nativeAllocHeap = Android.OS.Debug.NativeHeapAllocatedSize;
            long nativeFreeSize = Android.OS.Debug.NativeHeapFreeSize;

            //
            ActivityManager.MemoryInfo memoryInfo = getAvailableMemory();

            int iM = 1024 * 1024;

            string str;
            str = string.Format("maxHeap {0}M, ", javaMaxHeapSize / 1024 / 1024);
            //
            str += string.Format("totalHeap {0}M, freeHeap {1}M.", javaTotalHeapSize / iM, javaFreeHeapSize / iM);
            //
            str += string.Format(" total {0}M,avail {1}M", memoryInfo.TotalMem / iM, memoryInfo.AvailMem / iM);
            qyFuncs.showState_open(0, "", str, Consts.CONST_stateType_sys, 0, ref when, iPos, 0); iPos++;
            //
            str = "";
            str += string.Format("nativeHeap {0}M,nativeAlloc {1}M,nativeFree {2}M", nativeHeap / iM, nativeAllocHeap / iM, nativeFreeSize / iM);
            qyFuncs.showState_open(0, "", str, Consts.CONST_stateType_sys, 0, ref when, iPos, 0); iPos++;

            //
            str = "";
            MIS_CHANNEL channel = qyFuncs.getChannelByType(pMisCnt, Consts.CONST_channelType_talking);
            if (!qyFuncs.bChannelOk(channel))
            {
                str += ("t not ok");
            }

            channel = qyFuncs.getChannelByType(pMisCnt, Consts.CONST_channelType_media);
            if (!qyFuncs.bChannelOk(channel))
            {
                str += (" v not ok");
            }
            channel = qyFuncs.getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
            if (!qyFuncs.bChannelOk(channel))
            {
                str += (" a not ok");
            }
            if (str != null)
            {
                str = "channel " + str + " ";
            }
            //
            if (str != null)
            {
                qyFuncs.showState_open(0, "", str, Consts.CONST_stateType_sys, 0, ref when, iPos, 0); iPos++;
            }


            //
            string tServ;
            fixed (byte* pIp = pMisCnt.server.ip)
            {
                tServ = qyFuncs.mytoString(pIp);
            }

            //
            str = string.Format("FPS:{0:000.0} IO:{1,6:0.0},{2,6:0.0} t({3},{4,3}) a({5,5:0.0},{6,5:0.0} {7},{8}) v({9},{10} {11},{12}) f({13},{14} {15},{16}) CPU {17}% {18}. s({19})",
    pProcInfo.status.render.fps,
    pProcInfo.status.netStat.ins.uiInSpeedInKbps / 1000.0f, pProcInfo.status.netStat.ins.uiOutSpeedInKbps / 1000.0f,
    pProcInfo.av.talk.bChannelReady ? _T("  ") : _T("no"),
    pProcInfo.av.talk.iThreadsStatus,
    pProcInfo.status.netStat.uiInSpeedInKbps_a / 1000.0f, pProcInfo.status.netStat.uiOutSpeedInKbps_a / 1000.0f,
    pProcInfo.av.audio.bChannelReady ? _T("  ") : _T("no"),
    pProcInfo.av.audio.iThreadsStatus,
    pProcInfo.status.netStat.uiInSpeedInKbps_v / 1000.0f, pProcInfo.status.netStat.uiOutSpeedInKbps_v / 1000.0f,
    pProcInfo.av.video.bChannelReady ? _T("  ") : _T("no"),
    pProcInfo.av.video.iThreadsStatus,
    pProcInfo.status.netStat.uiInSpeedInKbps_f / 1000.0f, pProcInfo.status.netStat.uiOutSpeedInKbps_f / 1000.0f,
    pProcInfo.av.robot.bChannelReady ? _T("  ") : _T("no"),
    pProcInfo.av.robot.iThreadsStatus,
    pProcInfo.status.cpu.iCpuUsage,
    tServ,
    qyFuncs.showInfo_getQ2Nodes()
    );
            qyFuncs.showState_open(0, "", str, Consts.CONST_stateType_sys, 0, ref when, iPos, 0); iPos++;

            //
            str = "trans idle:";
            int i;
            for  (i=0;i<g_transes.Length;i++)
            {
                if (g_transes[i].bUsed) continue;
                str += i;
            }
            qyFuncs.showState_open(0, "", str, Consts.CONST_stateType_sys, 0, ref when, iPos, 0); iPos++;




            //
            qyFuncs.showState_open(0, "", str, Consts.CONST_stateType_sys, 0, ref when, -1, 0);

            //
            return 0;

        }

        //

        //
        public static unsafe int tmp_showState_task(CTX_caller ctxCaller, ref MIS_MSGU p0Param, PARAM_findOrgMsg p1Param, object taskElemParam)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            Param_tmp_showState_task param = (Param_tmp_showState_task)p1Param.param_tmp_showState_task;
            // p1Param
            //
            TASK_common taskElem = (TASK_common)taskElemParam;
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            string str;

            //
            fixed (MIS_MSG_TASK* pMsgTask = &pTaskData.msgU.task)
            {
                IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsgTask->data.buf;
                if (pContent->uiType == Consts.CONST_imCommType_transferAvInfo)
                {
                    if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
                    QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                    DLG_TALK_videoConference videoConference = pTc.getVideoConference();
                    //
                    TASK_transferAv task = (TASK_transferAv)taskElem;
                    PROC_TASK_AV pTaskAv = task.myTask;
                    str = string.Format("activeMems {0}, maxSpeakers {1}, limit_trans {2}", (int)videoConference.usCntLimit_activeMems_from, (int)pContent->transferAvInfo.confCfg.usMaxSpeakers, (int)pTaskAv.trans.usCntLimit_transforms);
                    qyFuncs.showState_open(0, "", str, param.usStateType, param.usStateSubtype, ref param.when, param.iPos, pTaskInfo.var.iTaskId); param.iPos++;

                }


            }


            iErr = 0;
        errLabel:

            return iErr;
        }


        //
#if DEBUG
#if false
		static int n_test =  0;
		public static int test_showState()
        {
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

			//
			mySYSTEMTIME when;
			qyFuncs.myGetLocalTime(out when);

			int nTotal = n_test % 13;
			n_test++;
			//
			int iPos = 0;
			int iTaskId = 0;
			string str="";

			int i;
			for  (i = 0; i < nTotal; i++)
            {
				str = "haha" + i;
				iTaskId = nTotal;
				qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, iTaskId); iPos++;
			}
			iPos = -1;
			qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, iTaskId); iPos++;

			//
			return 0;
        }
		
#endif
#endif

        //
        //5
        public static unsafe int showState_tasks(ref QY_MC_mainWndVar var)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QMC_TASK_INFO pTaskInfo = null;

            //
#if DEBUG
#endif


            //
            mySYSTEMTIME when;
            qyFuncs.myGetLocalTime(out when);

            //
            int iPos = 0;
            int i;
            string str;
            int index_taskInfo;
            uint uiTickCnt_now = qyFuncs.myGetTickCount(null);

            //


            //
            for (index_taskInfo = 0; index_taskInfo < pProcInfo.cfg.usMaxCnt_taskInfos; index_taskInfo++)
            {
                pTaskInfo = pProcInfo.getQmcTaskInfoByIndex(var.ctxCaller, index_taskInfo, "mainWnd_chk_tasks");
                if (!pTaskInfo.bUsed) continue;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null) continue;
                //
                str = string.Format("task[{0}]: ", index_taskInfo);
                //
                fixed (MIS_MSG_TASK* pMsgTask = &pTaskData.msgU.task)
                {
                    if (pMsgTask->uiType != Consts.CONST_misMsgType_task)
                    {
                        continue;
                    }

                    //
                    str += string.Format("{0}->{1}, peer {2}. ", pMsgTask->data.route.idInfo_from.ui64Id, pMsgTask->data.route.idInfo_to.ui64Id, pMsgTask->addr_logicalPeer.idInfo.ui64Id);

                    //
                    IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsgTask->data.buf;
                    //
                    switch (pContent->uiType)
                    {
                        case Consts.CONST_imCommType_transferAvInfo:
                            str += "av";
                            //
                            //
                            qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, pTaskInfo.var.iTaskId); iPos++;

                            //
                            //
                            str = "";
                            str += string.Format("tn_a {0}, tn_v {1}", pContent->transferAvInfo.ass.tranInfo.audio.uiTranNo_openAvDev, pContent->transferAvInfo.ass.tranInfo.video.uiTranNo_openAvDev);

                            qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, pTaskInfo.var.iTaskId); iPos++;

                            //
                            if (pTaskData.uiType != Consts.CONST_taskDataType_conf) break;
                            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                            int a;
                            for (a = 0; a < videoConference.activeMems_from.Length; a++)
                            {
                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[a];
                                //
                                if (pActiveMem_from.avStream.idInfo.ui64Id != 0)
                                {
                                    str = "";
                                    str += string.Format("act{0}:{1}:", a, pActiveMem_from.avStream.idInfo.ui64Id);
                                    str += string.Format("a({0},{1}),", pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev,
                                        (int)pActiveMem_from.avStream.obj.tranInfo.audio.compressor.ucCompressors);
                                    str += string.Format("v({0},{1})", pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev,
                                        (int)pActiveMem_from.avStream.obj.tranInfo.video.compressor.ucCompressors);
                                    
                                    //
                                    int iFourcc = (int)pActiveMem_from.avStream.obj.tranInfo.video.vh_stream.dwFourcc;
                                    
                                    TMP_buf5 fourccStr;
                                    //byte fourccStr[4 + 1] = "";
                                    qyFuncs.iFourcc2Str(iFourcc, fourccStr.buf, Consts.cntof_buf5);
                                    str += qyFuncs.mytoString(fourccStr.buf);
                                    //
                                    qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, pTaskInfo.var.iTaskId); iPos++;

                                }
                            }

                            //
                            Param_tmp_showState_task param = new Param_tmp_showState_task();
                            param.usStateType = Consts.CONST_stateType_tasks;
                            param.usStateSubtype = 0;
                            param.when = when;
                            param.iPos = iPos;

                            //
                            CMIS_MSGU p0 = new CMIS_MSGU();
                            PARAM_findOrgMsg p1 = new PARAM_findOrgMsg();
                            p1.param_tmp_showState_task = param;

                            //
                            pProcInfo.processQ_media.qTraverse(null, tmp_showState_task, ref p0.msgU,p1); iPos = param.iPos;

                            //
                            break;
                        default:
                            str += "other";

                            //
                            qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, pTaskInfo.var.iTaskId); iPos++;

                            break;
                    }









                    //
                    if (qyFuncs.qmcTaskInfo_bAlive(var.ctxCaller, pProcInfo, index_taskInfo))
                    {
                        str = "task alive.";
                    }
                    else
                    {
                        str = "task not alive. ";
                        if (pTaskInfo.var.bClosed) str += "closed.";
                    }
                    //
                    str += _T(" iStatus ") + (pMsgTask->iStatus) + _T(" ");
                    str += qyFuncs.qyGetDesByType1(qyFuncs.CONST_qyStatusTable_en, pMsgTask->iStatus);
                    str += _T(".");

                    //
                    int iElapseInMs;
                    iElapseInMs = Java.Lang.Math.Abs((int)(uiTickCnt_now - pTaskInfo.var.dwTickCnt_recv_lastRefreshed1));
                    str += string.Format(" lastRecv: {0}ms elapsed", iElapseInMs);
                    qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, pTaskInfo.var.iTaskId); iPos++;

                }

                //
                continue;
            }

            //
            str = string.Format("process_mediaQ.nQNodes {0}", pProcInfo.processQ_media.m_var.uiQNodes);
            qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, 0); iPos++;

            //
            TMP_texes texes = pProcInfo.av.texes;
            TMP_task_texes taskTexes = texes.taskAvTexes;
            ushort usCnt_activeMems = taskTexes.usCnt_activeMems;
            if (usCnt_activeMems > taskTexes.tti_peers.Length) goto errLabel;
            str = "tti:";
            for (i = 0; i < usCnt_activeMems; i++)
            {
                TMP_tex_info tti = taskTexes.tti_peers[i];
                if (tti.idInfo.ui64Id == 0) continue;
                str += "[" + i + "] " + tti.idInfo.ui64Id + "(" + tti.uiObjType + "), ";
            }
            qyFuncs.showState_open(0, "showTasks", str, Consts.CONST_stateType_tasks, 0, ref when, iPos, 0); iPos++;


            //
            qyFuncs.showState_open(0, "showTasks", "", Consts.CONST_stateType_tasks, 0, ref when, -1, 0);


#if false

				if (0 == (var.loopCtrl % 2))
			{
				qyFuncs.showState_open(0, ("kk"), "kk", Consts.CONST_stateType_tasks, 0, ref when, 0);
				qyFuncs.showState_open(0, ("kk"), "kk1", Consts.CONST_stateType_tasks, 0, ref when,1);
				qyFuncs.showState_open(0, ("kk"), "kk1", Consts.CONST_stateType_tasks, 0, ref when,-1);
			}
			else
            {
				qyFuncs.showState_open(0, ("mm"), "mm", Consts.CONST_stateType_tasks, 0, ref when,0);
				qyFuncs.showState_open(0, ("mm"), "mm1", Consts.CONST_stateType_tasks, 0, ref when,1);
				qyFuncs.showState_open(0, ("mm"), "mm2", Consts.CONST_stateType_tasks, 0, ref when,2);
				qyFuncs.showState_open(0, ("mm"), "mm2", Consts.CONST_stateType_tasks, 0, ref when,-1);


			}
#endif

        errLabel:

            return 0;
        }


        //
        public static unsafe int showState_players()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            int i;
            CMyDb pDb = (CMyDb)pQyMc.gui.pDb;
            if (pDb == null) return -1;

            //
            mySYSTEMTIME when;
            qyFuncs.myGetLocalTime(out when);

            //
            int iPos = 0;
            //int i;
            string str;


            //
            QY_PLAYER pPlayer;
            PLAY_AUDIO_procInfo pPlayAudio;
            PLAY_VIDEO_procInfo pPlayVideo;

            //  comment: player是一个数组,在pProcInfo.av.pPlayers[]. pProcInfo是一个全局的结构
            //
            for (i = 0; i < pProcInfo.av.usCnt_players; i++)
            {
                pPlayer = pProcInfo.av.pPlayers[i];
                pPlayAudio = pProcInfo.av.pPlayers[i].audio;
                pPlayVideo = pProcInfo.av.pPlayers[i].video;

                if (!pPlayer.audio.bRunning && !pPlayer.video.bRunning) continue;

                str = "";
                str += string.Format("player[{0}] {1}:", i, pPlayer.idInfo_recorder.ui64Id);

                if (pPlayer.audio.bRunning)
                {
                    int n = 0;
                    if (pProcInfo.cfg.b_useQ2_toPlayA)
                    {
                        n = qyFuncs.getQ2Nodes(pPlayAudio.q2);
                    }
                    else
                    {
                        n = pPlayAudio.qBuf2.getQ2Nodes();
                    }
                    str += string.Format("a.qNodes {0}", n);
                }

                //
                qyFuncs.showState_open(0, "showPlayers", str, Consts.CONST_stateType_players, 0, ref when, iPos, 0); iPos++;

            }

            //
            qyFuncs.showState_open(0, "showPlayers", "", Consts.CONST_stateType_players, 0, ref when, -1, 0);

            return 0;
        }



        //
        public static unsafe int showState_talks()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            int i;

            //
            mySYSTEMTIME when;
            qyFuncs.myGetLocalTime(out when);

            //
            int iPos = 0;
            //int i;
            string str;

            for (i = 0; i < pMisCnt.talkingFriendQ.Count; i++)
            {
                Activity_dlgTalk dlgTalk = (Activity_dlgTalk)pMisCnt.talkingFriendQ[i].hWnd;
                MSGR_ADDR addr = pMisCnt.talkingFriendQ[i].addr;

                //
                str = "";
                str += string.Format("talk: {0}", addr.idInfo.ui64Id);
                qyFuncs.showState_open(0, "showTalks", str, Consts.CONST_stateType_talks, 0, ref when, iPos, 0); iPos++;

                //
                str = "";
                str += string.Format("taskExists {0},", dlgTalk.m_var.av.taskInfo.bTaskExists);
                str += string.Format("taskId {0}", dlgTalk.m_var.av.taskInfo.iTaskId);
                qyFuncs.showState_open(0, "showTalks", str, Consts.CONST_stateType_talks, 0, ref when, iPos, 0); iPos++;


            }

            qyFuncs.showState_open(0, "showTalks", "", Consts.CONST_stateType_talks, 0, ref when, -1, 0);

            return 0;
        }


        //
        public static unsafe int showState_qs()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            int i;
            CMyDb pDb = (CMyDb)pQyMc.gui.pDb;
            if (pDb == null) return -1;

            //
            mySYSTEMTIME when;
            qyFuncs.myGetLocalTime(out when);

            //
            int iPos = 0;
            //int i;
            string str;


            str = "";
            str += string.Format("qyImTaskTab: qNodes {0}/maxQNodes {1}", pDb.m_var_pQ_qyImTaskTab.uiQNodes, pDb.m_var_pQ_qyImTaskTab.cfg.uiMaxQNodes);
            qyFuncs.showState_open(0, "showQs", str, Consts.CONST_stateType_qs, 0, ref when, iPos, 0); iPos++;


            qyFuncs.showState_open(0, "showQs", "", Consts.CONST_stateType_qs, 0, ref when, -1, 0);

            return 0;
        }


        //
        public static int mainWnd_OnTimer(object hMainWnd, ref QY_MC_mainWndVar var, uint nIDEvent)
        {
            int iErr = -1;


            CCtxQyMc pQyMc = g.g_pQyMc;
            if (null == pQyMc) return -1;

            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            if (pProcInfo.m_iCtxType != Consts.CONST_ctxType_qmc)
            {
                return -1;
            }
            //
            bool bDbg = false;
            string str;

            //
#if DEBUG
            //bDbg = true;
#endif
            //bDbg = true;

            //			
            var.loopCtrl++;

            //
            if (bDbg)
            {
                qyFuncs.showInfo_open(0, null, "mainWnd_OnTimer enters");
                //qyFuncs.showAllTc(var.ctxCaller, "mainWnd_OnTimer enters");
            }
#if DEBUG
#if false
			str = string.Format("mainWnd_OnTimer {0}", var.loopCtrl);
			qyFuncs.showInfo_open(0, null, str);
#endif
#endif



            //
            /*
			if (pQyMc->bQuit)
			{       //  Èç¹û£¬ÒªÍË³öÁË£¬¾Í·¢¸ö¹Ø±ÕÏûÏ¢°É£®
				SendMessage(hMainWnd, WM_CLOSE, 0, 0);
				iErr = CONST_qyRet_quit; goto errLabel;
			}
			if (pQyMc->bGuiQuit)
			{   //  ÕâÀïÊÇ£¬Òª¿´ÓÐÃ»ÓÐ½çÃæÔªËØÐèÒªÇå³ý¡£ÒªÊÇÃ»ÓÐ£¬¾Í½«bQuitÉèÎªtrue

				pQyMc->usTimes_tryToQuit++;

				if (isIsCli(pQyMc))
				{

					//  
					stopAll_avRecord();
					stopAll_sharedObjs(pProcInfo);

					closeOtherPlayers(pProcInfo);

					if (pQyMc->usTimes_tryToQuit < 7)
					{

						//  avRecord
						if (!bNoSharedObjExists(pProcInfo))
						{
							OutputDebugString(_T("Shared Objects still exist.\n"));
							qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("Gui"), NULL, _T(""), _T(""), _T("SharedObjects still exits. %d times"), pQyMc->usTimes_tryToQuit);

							//  ÊÔÍ¼»ØÊÕ×ÊÔ´
							tryToFreeSharedObjs(pProcInfo);

							iErr = CONST_qyRet_quit; goto errLabel;
						}

					}

				}

				pQyMc->bQuit = TRUE;
				SendMessage(hMainWnd, WM_CLOSE, 0, 0);
				iErr = CONST_qyRet_quit; goto errLabel;
			}
			*/



            {
                //
                if (0 == (var.loopCtrl % 5)) qyFuncs.bChkMessengerChannelsReady(pQyMc);


                if (null != pProcInfo)
                {

                    /*
					if (!(siCnt % pProcInfo->cfg.loopCtrl_chkPlayers)
						|| pProcInfo->av.bNeedChkPlayers)
					{
						pProcInfo->av.bNeedChkPlayers = FALSE;  //  2018/06/16
																//
						chkPlayers(pProcInfo);
					}
					//
					if (GetTickCount() - pProcInfo->status.notification.dwTickCnt_lastDisplayed > 900) displayNotification(NULL, NULL);
					*/
                    //

                    //
                    if (0 != pProcInfo.waitToQuit.dwTickCnt_start)
                    {
                        if (qyFuncs.myGetTickCount(null) - pProcInfo.waitToQuit.dwTickCnt_start > pProcInfo.waitToQuit.dwTimeInMs)
                        {
                            qyFuncs.traceLog(_T("It's time to quit"));
                            qyFuncs.notifyPanicToApp(pQyMc, -1, pProcInfo.waitToQuit.hint);
                        }
                    }



                    //  2011/03/30
                    mainWnd_chk_tasks(hMainWnd, ref var);

#if __isCli__
					//  2012/08/01

					//  2013/03/07
					if (!(siCnt % 5))
					{
						qisChkDb_gui_myDb();
						//  2014/12/22
						if (pProcInfo->status.sysErr.bFailed_newSharedObj)
						{
							pProcInfo->status.sysErr.bFailed_newSharedObj = FALSE;
							//
							showInfo_open(0, 0, _T("mainWnd_timer: bFailed_newSharedObj true. to chk sharedObj"));
							//  2009/12/28
							PostMessage(pQyMc->gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_task_end, 0);
						}
					}

					//  2013/08/07

					//  2013/08/01
					//   2017/09/13
					if (!(siCnt % 10))
					{
						chkSharedObjs(pProcInfo);
					}

#endif
                    //
                    MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                    //
#if false
					mySYSTEMTIME st;
					qyFuncs.myGetLocalTime(out st);
					str = string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);
					qyFuncs.traceLog(str);
#endif

                    //
#if false
					str = string.Format("test: {0} times ", var.loopCtrl);
					qyFuncs.showInfo_open(0, _T(  "ontimer"  ), str);
#endif


                }

                //
                mainWnd_showStatus(ref var);

                //
#if DEBUG
                if (pProcInfo.cfg.mSpecialDbg.bDoGC_mainWnd_OnTimer)
                {

                    qyFuncs.showInfo_open(0, null, "for test, doGC");
                    qyFuncs_Droid.doGC("mainWnd_OnTimer");
                }
#endif

            }

            iErr = 0;

        errLabel:

            //
            if (bDbg)
            {
                //qyFuncs.showAllTc(var.ctxCaller, "mainWnd_OnTimer leaves");
                qyFuncs.showInfo_open(0, null, "mainWnd_OnTimer leaves");
            }

            //
            return iErr;
        }

        public static unsafe int displayNotification(CCtxQmc pProcInfo, ref MIS_MSG_taskStatus pStatus)
        {
            int iErr = -1;
            //
            if (null == pProcInfo) return -1;

            //
            //pProcInfo->status.notification.dwTickCnt_lastDisplayed = GetTickCount();
            fixed (MIS_MSG_taskStatus* pStatus0 = &pStatus)
            {
                GuiShare.pf_gui_displayNotification(pProcInfo, (IntPtr)pStatus0);
            }


            iErr = 0;

        errLabel:

            return 0;   //  iErr;

        }

        //
        public static unsafe int qmcApplyForChkingTasks_gui(CCtxQyMc pQyMc)   //  2009/09/10
        {
#if DEBUG
            qyFuncs.traceLog(_T("qisApplyForChkingTasks_gui called"));
#endif

            if (null != pQyMc)
            {
                GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_chkTask, 0);
            }

            return 0;
        }


    }
}



