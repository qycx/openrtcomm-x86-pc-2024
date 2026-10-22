using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static unsafe int dlgTalk_procTask1(object hCurDlgTalk, int iTaskId, ushort usOp, string paramStrBuf, ref PARAM_dlgTalk_procTask pParam)
        {
            int iErr = -1;
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            bool bDbg = false;
            string str;

#if DEBUG
            //bDbg = true;
#endif

            if (bDbg)
            {
                qyFuncs.showInfo_open(0, null, "dlgTalk_procTask1 enters");
                //qyFuncs.showAllTc(mainActivity.var_common.ctxCaller, "dlgTalk_procTask1 enters");
            }


            //
            if (null == hCurDlgTalk) return -1;

            // using (CHelp_getDlgTalkVar help_getDlgTalkVar_cur = new CHelp_getDlgTalkVar())
            Activity_dlgTalk cur_dlgTalk = (Activity_dlgTalk)hCurDlgTalk;
            {
                ;
                DLG_TALK_var pCurVar = cur_dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar_cur.getVar(hCurDlgTalk);
                if (null == pCurVar) return -1;

                object hDlgTalk = hCurDlgTalk;
                /*
				if (!isTalkerShadowMgr(ref pCurVar.addr))
				{
					TALKER_shadow* pShadow = (TALKER_shadow*)pCurVar->pShadowInfo;
					if (!pShadow) return -1;
					hDlgTalk = pShadow->hMgr;
				}
				if (!hDlgTalk) return -1;
				*/

                //
                //using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
                Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

                {
                    ;
                    DLG_TALK_var pm_var = dlgTalk.m_var;
                    //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                    if (null == pm_var) return -1;
                    DLG_TALK_var m_var = pm_var;
                    //
                    int lenInBytes = 0;
                    //
                    object pDb = null;
                    int iDbType = pQyMc.cfg.db.iDbType;
                    using (CQyMalloc mallocObj_pRcd = new CQyMalloc())
                    {
                        ;
                        IM_TASK_RCD* pRcd = (IM_TASK_RCD*)mallocObj_pRcd.mallocf(sizeof(IM_TASK_RCD));
                        if (null == pRcd) return -1;
                        ref IM_TASK_RCD rcd = ref *pRcd;
                        IM_CONTENTU* pContent = null;
                        //
                        char* pT;
                        //
                        //TCHAR tBuf[256];
                        int nItem;
                        //TCHAR sqlBuf[CONST_maxSqlBufLen + 1] = _T("");
                        //  DWORD							dwProcessId;
                        int iStatus = 0;
                        int iStatus_old = 0;

                        int i;

                        //
                        if (0 == iTaskId) return -1;

                        //  2012/05/01
                        if (!isTalkerShadowMgr(ref m_var.addr)) return -1;
                        //TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)m_var.pShadowInfo;
                        //if (!pShadowMgr) return -1;


                        using (CQnmDb db = new CQnmDb())
                        {
                            ;

                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                            pDb = db.m_pDbMem.pDb;

                            int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(mainActivity.var_common.ctxCaller, pProcInfo, iTaskId);
                            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "dlgTalk_procTask1");
                            if (pTaskInfo == null) goto errLabel;
                            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                            if (pTaskData == null) goto errLabel;

                            //
                            fixed (MIS_MSG_TASK* pMsgTask = &pTaskData.msgU.task)
                            {
                                if (pMsgTask->uiType != Consts.CONST_misMsgType_task)
                                {
                                    goto errLabel;
                                }
                                /*

								//
								if (!(pMsgTask = findTaskInProcessQ(&m_var, iTaskId)))
								{
	# if  __DEBUG__
									traceLogA("procQmInternalUrl failed: send again, findTaskInProcessQ failed");
	#endif
									goto errLabel;
								}
								*/


                                qyFuncs.msg2TaskRcd(pMisCnt, ref *pMsgTask, 0, ref *pRcd);
                                pContent = (IM_CONTENTU*)&pMsgTask->data.buf;

                                /*
								if (usOp == CONST_imOp_openFile)
								{
									if ((pT = _tcsrchr(rcd.content, _T('\\'))))
									{
										*pT = 0;
										if (bDir(rcd.content))
										{       //  ÊÇÄ¿Â¼²Å´ò¿ª£¬·ñÔò£¬²»´ò¿ª¡£·ÀÖ¹ÊÇ¿ÉÖ´ÐÐÎÄ¼þ
											ShellExecute(null, _T("open"), rcd.content, _T(""), null, SW_SHOW);
										}
									}
									iErr = 0; goto errLabel;
								}
								*/


                                switch (usOp)
                                {
                                    case Consts.CONST_imOp_send_permit:
                                        {                     //  

                                            if (rcd.uiContentType == Consts.CONST_imCommType_transferAvInfo)
                                            {

#if __DEBUG__
												traceLogA("procQmInternalUrl: send transferAvInfo again.");
#endif
                                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                                if (pProcInfo.processQ_media.qTraverse(mainActivity.var_common.ctxCaller, qyFuncs.tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsgTask, param) <= 0)
                                                {
                                                    goto errLabel;
                                                }
                                            }

                                            iStatus = Consts.CONST_imTaskStatus_req;
                                            qyFuncs.traceLog("##175  " + iStatus);
                                            //
                                            /*
											for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
											{
												if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
												{
													CHelp_getDlgTalkVar help_getDlgTalkVar;
													dlgTalk_procTask_display_op(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
												}
											}
											//					  
											dlgTalk_procTask_display_op(hDlgTalk, &m_var, iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
											*/

                                            //					  
                                            lenInBytes = (int)(pMsgTask->lenInBytes - (uint)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf"));
                                            if (lenInBytes < 0) goto errLabel;
                                            //
                                            //  2015/09/07
                                            uint uiChannelType = pMsgTask->uiChannelType;
                                            //  2015/09/07
                                            uiChannelType = 0;
                                            //
                                            QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;
                                            QY_MESSENGER_ID idInfo_dst = rcd.idInfo_recv;
                                            if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, pMsgTask->usCode, pMsgTask->tStartTime, pMsgTask->uiTranNo, 0, pMsgTask->iTaskId, pMsgTask->uiTaskType, (byte*)&pMsgTask->data.buf, (uint)lenInBytes, idInfo_logicalPeer.ui64Id, pMsgTask->idInfo_taskSender.ui64Id, pMsgTask->idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;

                                        }
                                        break;
                                    case Consts.CONST_imOp_recv_accept:
                                    case Consts.CONST_imOp_recvFile_saveas:
                                    case Consts.CONST_imOp_dual_permit:

                                        if (0 != dlgTalk_procTask_recv_accept(hCurDlgTalk, pCurVar, hDlgTalk, pm_var, iTaskId, usOp, paramStrBuf, ref rcd, pMsgTask, pContent, ref iStatus, ref iStatus_old, g_dbFuncs, pDb, ref pParam)) goto errLabel;

                                        break;

                                    case Consts.CONST_imOp_recv_deny:
                                    case Consts.CONST_imOp_recv_cancel:
                                        {
                                            //
#if DEBUG
                                            qyFuncs.showInfo_open(0, _T(""), _T("dlgTalk_procTask1: recv_cancel"));
#endif
                                            //
                                            if (usOp == Consts.CONST_imOp_recv_deny)
                                            {
                                                iStatus = Consts.CONST_imTaskStatus_deniedByReceiver;
                                            }
                                            else
                                            {
                                                if (pParam.bAutoCancel)
                                                {
                                                    iStatus = Consts.CONST_imTaskStatus_autoCanceledByReceiver;
                                                }
                                                else
                                                {
                                                    iStatus = Consts.CONST_imTaskStatus_canceledByReceiver;
                                                    qyFuncs.traceLog("##234");
                                                }
                                            }
                                            //
                                            iStatus_old = pMsgTask->iStatus;
                                            //
                                            if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, rcd.id)) goto errLabel;
                                            pMsgTask->iStatus = iStatus;    //  2009/10/17
                                            qyFuncs.mytime(ref pMsgTask->tLastModifiedTime);   //  2014/05/01

                                            //
                                            /*
											for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
											{
												if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
												{
													CHelp_getDlgTalkVar help_getDlgTalkVar;

													dlgTalk_procTask_display_op(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
												}
											}
											//
											dlgTalk_procTask_display_op(hDlgTalk, &m_var, iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
											*/

                                            //
                                            //  if  (  rcd.uiContentType  ==  CONST_imCommType_transferAvInfo  )  {
                                            //	  freeCapImageByTaskId(  pDlg,  0,  rcd.id,  &pDlg->m_var.av.peerImages  );
                                            //  }					  

                                            //  
                                            TASK_PROC_REQ taskProcReq;
                                            QY_MESSENGER_ID idInfo_dst;

                                            qyFuncs.mymemset((IntPtr)(byte*)&taskProcReq, 0, sizeof(TASK_PROC_REQ));
                                            taskProcReq.uiType = Consts.CONST_imCommType_taskProcReq;
                                            taskProcReq.usOp = usOp;
                                            taskProcReq.tStartTime_org = rcd.tSendTime;
                                            taskProcReq.uiTranNo_org = rcd.uiTranNo;
                                            taskProcReq.uiSerialNo_org = rcd.uiSerialNo;
                                            taskProcReq.uiContentType_org = rcd.uiContentType;

                                            //  2014/11/22
                                            idInfo_dst.ui64Id = rcd.idInfo_send.ui64Id;
                                            //					  	
                                            lenInBytes = sizeof(TASK_PROC_REQ);
                                            //					
                                            Int64 tStartTran; uint uiTranNo;
                                            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
                                            //
                                            QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;
                                            if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, pMsgTask->usCode, tStartTran, uiTranNo, 0, iTaskId, rcd.uiMode, (byte*)&taskProcReq, (uint)lenInBytes, idInfo_logicalPeer.ui64Id, pMsgTask->idInfo_taskSender.ui64Id, pMsgTask->idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, pMsgTask->uiChannelType, null, false)) goto errLabel;

                                            //
                                            /*
											if (pContent->uiType == CONST_imCommType_transferAvInfo)
											{
												TRANSFER_AV_INFO* pTransferAvInfo = &pContent->transferAvInfo;
												//
												PROC_TASK_AV* pTask = get_transferAvInfo_pTask(pTransferAvInfo, _T(""));
												if (!pTask) goto errLabel;
												//
												if (pTask->dlgTalkInProxy.bExists)
												{
													memset(&taskProcReq, 0, sizeof(taskProcReq));
													taskProcReq.uiType = CONST_imCommType_taskProcReq;
													taskProcReq.usOp = usOp;

													taskProcReq.tStartTime_org = pTask->dlgTalkInProxy.proxy.tStartTran;
													taskProcReq.uiTranNo_org = pTask->dlgTalkInProxy.proxy.uiTranNo;
													taskProcReq.uiSerialNo_org = 0;
													taskProcReq.uiContentType_org = pContent->uiType;
													//
													idInfo_dst.ui64Id = m_var.addr.idInfo.ui64Id;
													//
													lenInBytes = sizeof(taskProcReq);
													//					
													MACRO_prepareForTran();
													//
													if (postMsgTask2Mgr_mc(m_var.pMisCnt, CONST_misMsgType_task, 0, pMsgTask->usCode, tStartTran, uiTranNo, 0, iTaskId, rcd.uiMode, (char*)&taskProcReq, lenInBytes, &m_var.addr.idInfo, &pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, &idInfo_dst, pMsgTask->uiChannelType, null, FALSE)) goto errLabel;

												}
											}
											*/

                                            //  
                                            //dlgTalk_freeCapImageBySth_mgr(hDlgTalk, rcd.id, null);

                                            //  2009/02/20
                                            if (m_var.av.taskInfo.bTaskExists)
                                            {
                                                if (m_var.av.taskInfo.iTaskId == rcd.id)
                                                {
                                                    dlgTalk_closeTaskAv(hDlgTalk, m_var, "dlgTalk_procTask1.recv_cancel");
                                                }
                                            }
                                            //  2013/11/18
                                            /*
											if (pShadowMgr->remoteAssist.iTaskId == rcd.id)
											{
												SendMessage(pShadowMgr->remoteAssist.hWnd_remoteAssist, WM_CLOSE, 0, 0);
												pShadowMgr->remoteAssist.hWnd_remoteAssist = null;
												pShadowMgr->remoteAssist.iTaskId = 0;
											}
											*/

                                            //  2010/12/23
                                            chkResources(hDlgTalk, true);

                                            //  2012/03/24
                                            qmcApplyForChkingTasks_gui(pQyMc);
                                            //
                                        }
                                        //
                                        break;
                                    case Consts.CONST_imOp_send_cancel:
                                        {
                                            //
#if __DEBUG__
											showInfo_open(0, _T("dlgTalk_procTask1"), _T("send_cancel"));
#endif
                                            //
                                            iStatus = Consts.CONST_imTaskStatus_canceledBySender;
                                            iStatus_old = pMsgTask->iStatus;
                                            //
                                            if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, rcd.id)) goto errLabel;
                                            pMsgTask->iStatus = iStatus;    //  2009/10/17
                                            qyFuncs.mytime(ref pMsgTask->tLastModifiedTime);   //  2014/05/01

                                            //
                                            /*
											for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
											{
												if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
												{
													CHelp_getDlgTalkVar help_getDlgTalkVar;

													dlgTalk_procTask_display_op(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
												}
											}
											dlgTalk_procTask_display_op(hDlgTalk, &m_var, iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
											*/

                                            //  
                                            TASK_PROC_REQ taskProcReq;
                                            qyFuncs.mymemset((IntPtr)(byte*)&taskProcReq, 0, sizeof(TASK_PROC_REQ));
                                            taskProcReq.uiType = Consts.CONST_imCommType_taskProcReq;
                                            taskProcReq.usOp = usOp;
                                            taskProcReq.tStartTime_org = rcd.tSendTime;
                                            taskProcReq.uiTranNo_org = rcd.uiTranNo;
                                            taskProcReq.uiSerialNo_org = rcd.uiSerialNo;
                                            taskProcReq.uiContentType_org = rcd.uiContentType;
                                            //
                                            lenInBytes = sizeof(TASK_PROC_REQ);
                                            //
                                            Int64 tStartTran; uint uiTranNo;
                                            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);

                                            //  2015/09/07
                                            uint uiChannelType = pMsgTask->uiChannelType;
                                            //  2015/09/07
                                            uiChannelType = 0;
                                            //
                                            QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;
                                            QY_MESSENGER_ID idInfo_dst = rcd.idInfo_recv;
                                            if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, pMsgTask->usCode, tStartTran, uiTranNo, 0, iTaskId, rcd.uiMode, (byte*)&taskProcReq, (uint)lenInBytes, idInfo_logicalPeer.ui64Id, pMsgTask->idInfo_taskSender.ui64Id, pMsgTask->idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, uiChannelType, null, false))
                                            {
                                                goto errLabel;
                                            }

                                            //  
                                            //  freeCapImageByTaskId(  hDlgTalk,  0,  rcd.id,  &m_var.av.peerImages  );
                                            dlgTalk_freeCapImageBySth_mgr(hDlgTalk, rcd.id, null);
                                            int idc = 0;// m_var.av.idc_me;  //  0
                                            dlgTalk_freeCapImageByTaskId_me_mgr(hDlgTalk, idc, rcd.id);


                                            //  2009/02/20
                                            if (m_var.av.taskInfo.bTaskExists)
                                            {
                                                if (m_var.av.taskInfo.iTaskId == iTaskId)
                                                {
                                                    dlgTalk_closeTaskAv(hDlgTalk, m_var, "dlgTalk_procTask1.send_cancel");
                                                    //  2014/10/09
                                                    dlgTalk_freeCapImages_bgWall_mgr(hDlgTalk, 0);
                                                }
                                            }

                                            //  2011/10/10
                                            chkResources(hDlgTalk, true);

                                            //  2012/03/24
                                            qmcApplyForChkingTasks_gui(pQyMc);

                                            //
                                        }
                                        //
                                        break;

                                    default:
#if DEBUG
                                        str = string.Format("procQmInternalUrl: unprocessed op {0}", usOp);
                                        qyFuncs.traceLog(str);
#endif
                                        goto errLabel;


                                }


                            }
                        }
                    }



                    iErr = 0;
                errLabel:

                    dlgTalk_displayAvStatus(hDlgTalk, m_var, 0, 0, null);

                    if (0 != iErr)
                    {
                    }

                    //
                    qyFuncs.dyn_d3dWall_refreshWallMemByTalker(hDlgTalk);

                    //  201
                    refreshTalkToInfo_mgr(hDlgTalk);
                }
            }

            //
            if (bDbg)
            {
                qyFuncs.showInfo_open(0, null, "dlgTalk_procTask1 leaves");
                //qyFuncs.showAllTc(mainActivity.var_common.ctxCaller, "dlgTalk_procTask1 leaves");
            }



            return iErr;

        }


        //
        public static unsafe int dlgTalk_procTask_recv_accept(object hCurDlgTalk, DLG_TALK_var pCurVar, object hMgr, DLG_TALK_var pMgrVar, int iTaskId, ushort usOp, string paramStrBuf,
    ref IM_TASK_RCD rcd, MIS_MSG_TASK* pMsgTask, IM_CONTENTU* pContent,
    ref int iStatus, ref int iStatus_old,
    QM_dbFuncs g_dbFuncs,
    object pDb,
    //TALKER_shadow_mgr* pShadowMgr,
    ref PARAM_dlgTalk_procTask pParam
    )
        {
            int iErr = -1;

            object hDlgTalk = hMgr;
            DLG_TALK_var m_var = pMgrVar;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            int i;
            int lenInBytes;

            switch (rcd.uiContentType)
            {
                case Consts.CONST_imCommType_transferFileReq:
                    return dlgTalk_procTask_transferFileReq(hCurDlgTalk, ref pCurVar, hMgr, ref pMgrVar, iTaskId, usOp, paramStrBuf, ref rcd, pMsgTask, pContent, ref iStatus, ref iStatus_old, g_dbFuncs, pDb);
                    break;
                case Consts.CONST_imCommType_transferAvInfo:
                    return dlgTalk_procTask_transferAvInfo(hCurDlgTalk, pCurVar, hMgr, pMgrVar, iTaskId, usOp, paramStrBuf, ref rcd, pMsgTask, pContent, ref iStatus, ref iStatus_old, g_dbFuncs, pDb, ref pParam);
                    break;
                default:
                    break;
            }




            iErr = 0;
        errLabel:
            return iErr;

        }


    }
}