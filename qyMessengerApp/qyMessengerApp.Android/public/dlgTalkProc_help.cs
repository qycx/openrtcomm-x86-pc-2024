using Android.Content;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace qy.Droid
{
	partial class qyFuncs_Droid
	{
		public static unsafe int findTalker(CCtxQyMc pQyMcParam, MSGR_ADDR* pAddr, uint unused, ref object phWnd)
		{
			//AFX_MANAGE_STATE(AfxGetStaticModuleState());

			int iErr = -1;
			CCtxQyMc pQyMc = (CCtxQyMc)pQyMcParam;

			COMMON_PARAM commonParam1;
			MIS_CNT pMisCnt = null;
			int iRet;
			object hWnd = null;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			/*
			if (!pAddr) return -1;

			//  2012/04/23
			assert(!pAddr->uiTranNo_shadow);

			//
			if (!(pMisCnt = getMisCntByName(pProcInfo, pAddr->misServName))) goto errLabel;

			MACRO_makeCommonParam3((void*)pAddr, (void*)unused, 0, commonParam1);
			iRet = qTraverse(&pMisCnt->talkingFriendQ, tmpHandler_findTalker, 0, &commonParam1);
			if (iRet <= 0) goto errLabel;
			hWnd = (HWND)commonParam1.p2;
			*/

			iErr = 0;
		errLabel:
			if (0 == iErr)
			{
				phWnd = hWnd;
			}
			return iErr;
		}

		public static unsafe int findTalker(CCtxQyMc pQyMcParam, MIS_CNT pMisCntParam, QY_MESSENGER_ID pIdInfo, ref object phWnd)
		{
			//AFX_MANAGE_STATE(AfxGetStaticModuleState());

			int iErr = -1;
			CCtxQyMc pQyMc = (CCtxQyMc)pQyMcParam;
			MIS_CNT pMisCnt = (MIS_CNT)pMisCntParam;
			COMMON_PARAM commonParam1;
			int iRet;
			object hWnd = null;



			if (null == pMisCnt || 0 == pIdInfo.ui64Id) return -1;

			//MACRO_makeCommonParam3((void*)pMisCnt->cfg.misServName, (void*)pIdInfo, 0, commonParam1);
			//iRet = qTraverse(&pMisCnt->talkingFriendQ, tmpHandler_findTalker_simple, 0, &commonParam1);
			//if (iRet <= 0) goto errLabel;
			//hWnd = (HWND)commonParam1.p2;
			try
			{
				int index = pMisCnt.talkingFriendQ.FindIndex(x => x.addr.idInfo.ui64Id == pIdInfo.ui64Id);
				if (index < 0) goto errLabel;
				hWnd = pMisCnt.talkingFriendQ[index].hWnd;
			}
			catch (ArgumentNullException e)
			{
				goto errLabel;
			}
			catch (Exception e)
			{
				goto errLabel;

			}

			iErr = 0;
		errLabel:
			if (0 == iErr)
			{
				phWnd = hWnd;
			}
			return iErr;
		}

		//
		public static unsafe ushort dlgTalk_get_usLayoutType_local(object hDlgTalk)
		{

			ushort usLayoutType_local = 0;

			/*
			CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;

			CHelp_getDlgTalkVar help_getDlgTalkVar;
			DLG_TALK_var* pm_var = (DLG_TALK_var*)help_getDlgTalkVar.getVar(hDlgTalk, _T("dlgTalk_get_usLayoutType_local"));
			if (!pm_var) goto errLabel;


			//  2013/02/28
			DLG_TALK_var* pMgrVar = pm_var;
			if (!isTalkerShadowMgr(pm_var->addr))
			{
				TALKER_shadow* pTalkerShadow = (TALKER_shadow*)pm_var->pShadowInfo;
				if (!pTalkerShadow) goto errLabel;
				if (!IsWindow(pTalkerShadow->hMgr)) goto errLabel;
				//  hMgr  =  pTalkerShadow->hMgr;		//  2013/02/15
				pMgrVar = (DLG_TALK_var*)help_getDlgTalkVar_mgr.getVar(pTalkerShadow->hMgr, _T("dlgTalk_get_usLayoutType_local, hMgr"));
				if (!pMgrVar) goto errLabel;
			}
			TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)pMgrVar->pShadowInfo;
			if (!pShadowMgr) goto errLabel;

			usLayoutType_local = pShadowMgr->av.usLayoutType_local;
			*/

		errLabel:
			return usLayoutType_local;
		}




		//
		public static unsafe TableItem_dlgTalk_msg findTaskInProcessQ(DLG_TALK_var m_var, int iTaskId, bool bNextAv, bool bToShareScreen)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			//
			TableItem_dlgTalk_msg pMsg = null;
			//TableItem_dlgTalk_msg pQNode = null;

			if (null == m_var.m_pProcessQ) return null;
			List<TableItem_dlgTalk_msg> m_pProcessQ =m_var.m_pProcessQ;
			bool bFound = false;


			foreach (var pQNode in m_pProcessQ)
			{
				pMsg = pQNode;
				if (pMsg.uiMsgType == Consts.CONST_misMsgType_task)
				{
					if (iTaskId!=0)
					{
						if (iTaskId == pMsg.iTaskId)
						{
							bFound = true;
							break;
						}
					}
					else if (bNextAv)
					{
						if (pMsg.uiTaskType == Consts.CONST_imTaskType_transferAvInfo
							//&& qyFuncs.bTaskAlive(pMsg.iStatus)
							)
						{
							bFound = true;
							break;
						}
					}
					else if (bToShareScreen)
					{       //  2011/03/12
						if ((pMsg.uiTaskType == Consts.CONST_imTaskType_shareScreen
							|| pMsg.uiTaskType == Consts.CONST_imTaskType_remoteAssist       //  2015/11/05
							)
							//&& qyFuncs.bTaskAlive(pMsg.iStatus)
							&& pMsg.ui64Id_from == pMisCnt.idInfo.ui64Id)
						{
							bFound = true;
							break;
						}
					}
				}
			}
			//if (!pQNode) return NULL;
			if (!bFound) return null;

			//return &pMsg->task;
			return pMsg;
		}


		//

		public static unsafe int dlgTalk_qPostMsg(object hDlgTalk, ref MIS_MSGU  pMsgU, uint size)
		{
			int iErr = -1;

			CCtxQyMc pQyMc = g.g_pQyMc;		
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();		
			if (null==pProcInfo) return -1;

			//
			QY_MESSENGER_ID idInfo_from;idInfo_from.ui64Id = 0;
			Int64 tStartTime = 0;
			uint uiTranNo = 0;
			ushort usCode = 0;
			uint uiContentType = 0;
			string msg = "";
			int iTaskId = 0;
			int iStatus = 0;


            //
            //	using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;
            {
				
				DLG_TALK_var pVar = (DLG_TALK_var)dlgTalk.m_var;
				if (null == pVar)
				{
					//  2014/01/24	 
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("Gui"), NULL, _T(""), _T(""), _T("dlgTalk_qPostMsg failed,  getDlgTalkVar is null"));
					return -1;
				}
				if (null == pVar.m_pProcessQ)
				{
					//  2014/01/24	 
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("dlgTalk_qPostMsg failed,  pVar->m_ProcessQ is null"));
					return -1;
				}
				if (0 == size) return -1;

#if __DEBUG__
				//traceLog(  _T(  "dlgTalk_qPostMsg not finished"  )  );
#endif

				fixed (MIS_MSGU* pMsg = &pMsgU)
				{
					switch (pMsg->uiType)
					{
						case Consts.CONST_misMsgType_task:
							idInfo_from.ui64Id = pMsg->task.data.route.idInfo_from.ui64Id;
							tStartTime = pMsg->task.tStartTime;
							uiTranNo = pMsg->task.uiTranNo;
							iTaskId = pMsg->task.iTaskId;
							iStatus = pMsg->task.iStatus;

							//
							if (0 != pMsg->task.iTaskId)
							{
								switch (pMsg->task.uiTaskType)
								{
									case Consts.CONST_imTaskType_sendFile:
									case Consts.CONST_imTaskType_recvFile:
										{
											/*
											PARAM_PROC_ROW param;
											memset(&param, 0, sizeof(param));
											param.iTaskId = pMsg->task.iTaskId;
											qRemoveMsg(pVar->m_pProcessQ, tmpHandler_bRemoveMsgInProcessQ, hDlgTalk, &param);
											*/
										}
										break;
									default:
										{
											//  2016/02/17					  
											TableItem_dlgTalk_msg tmppMsg = findTaskInProcessQ(pVar, pMsg->task.iTaskId, false, false);
											//					  
											if (tmppMsg != null)
											{
#if false
											if (tmppMsg.iStatus == pMsg->task.iStatus) return -1;
											//
											if (qyFuncs.bTaskAlive(tmppMsg.iStatus)) return -1;
											//
											if (tmppMsg.iStatus == Consts.CONST_imTaskStatus_autoCanceledByReceiver)
											{
												/*
												PARAM_PROC_ROW param;
												memset(&param, 0, sizeof(param));
												param.iTaskId = pMsg->task.iTaskId;
												qRemoveMsg(pVar->m_pProcessQ, tmpHandler_bRemoveMsgInProcessQ, hDlgTalk, &param);
												*/
												break;
											}
#endif
												//
												return -1;
											}
											break;

										}
								}
							}
							//
							IM_CONTENTU* pContent;
							pContent = qyFuncs.M_getMsgContent(pMsg->task.uiType, pMsg->task.ucFlg, &pMsg->task.data);
							//
							uiContentType = pContent->uiType;
							//
							switch (pContent->uiType)
							{
								case Consts.CONST_imCommType_transferAvInfo:
									/*
									 {
										CQyMalloc mallocObj_pMsgTask;
										MIS_MSG_TASK* pMsgTask = (MIS_MSG_TASK*)mallocObj_pMsgTask.mallocf(sizeof(MIS_MSG_TASK));
										//
										if (sizeof(TRANSFER_AV_task) > sizeof(IM_CONTENTU))
										{
											showInfo_open(0, 0, _T("dlgTalk_qPostMsg failed: sizeof(  TRANSFER_AV_task  ) too big"));
											return -1;
										}
										int maxLen = offsetof(MIS_MSG_TASK, data) + offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_AV_task);
										memset(pMsgTask, 0, maxLen);
										//
										int msgLen = offsetof(MIS_MSG_TASK, data) + pMsg->task.lenInBytes;
										memcpy(pMsgTask, &pMsg->task, msgLen);
										//
										IM_CONTENTU* pContent_new = M_getMsgContent(pMsgTask->ucFlg, &pMsgTask->data);
										//
										pContent_new->transferAvInfo.tmp_usSubtype = CONST_transferAvInfo_subtype_task;
										//
										pMsgTask->lenInBytes = offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_AV_task);
										//
										msgLen = offsetof(MIS_MSG_TASK, data) + pMsgTask->lenInBytes; ;
										return  ::qPostMsg(pMsgTask, msgLen, pVar->m_pProcessQ);

									}
									*/
									msg = "视频通话";
									break;
								default:
									break;
							}
							//
							break;
						case Consts.CONST_misMsgType_talk:
							break;
						case Consts.CONST_misMsgType_taskStatus:
#if __DEBUG__
						traceLog(_T("dlgTalk_qPostMsg: msgTaskStatus"));
#endif
							break;
						case Consts.CONST_misMsgType_applyForTalkerShadow_qmc:
							break;
						default:
#if __DEBUG__
						traceLog(_T("dlgTalk_qPostMsg: unexpected msgType, %d"), pMsg->uiType);
#endif
							break;
					}
				}


				//return  ::qPostMsg(pQElem, size, pVar->m_pProcessQ);
				GuiShare.pf_talk_addNewMsg(hDlgTalk, idInfo_from, tStartTime,uiTranNo, usCode, uiContentType, msg,iTaskId);
			}

			iErr = 0;
		errLabel:
			return iErr;
		}
		


		//
		public static unsafe int removeTalker(object pQyMcParam, object hDlgTalk, uint unused)
		{

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			if (null == pMisCnt) return -1;

			//
			COMMON_PARAM commonParam1;
			//MACRO_makeCommonParam3((void*)pAddr, (void*)unused, 0, commonParam1);

			//qRemoveMsg(&pMisCnt->talkingFriendQ, tmpHandler_findTalker, NULL, &commonParam1);
			pMisCnt.talkingFriendQ.RemoveAll(item => item.hWnd == hDlgTalk);

			return 0;
		}


		//
		public static int printTalkers(string str)
		{
#if DEBUG
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            if (null == pMisCnt) return -1;

			string total_str = "";

			int i;
			for  (  i  = 0;i <pMisCnt.talkingFriendQ.Count;i++)
			{
				var item = pMisCnt.talkingFriendQ[i];
				Activity_dlgTalk dlgTalk = (Activity_dlgTalk)item.hWnd;
				int ii = 0;// dlgTalk.m_var.tmp_hTools_dlgTalk_av.GetLength(0);

				//
				string str1  =  "printTalkers[" + i + "]:" + item.addr.idInfo.ui64Id  ;
				str1 += " m_var.addr:" + dlgTalk.m_var.addr.idInfo.ui64Id;
				//
				if (dlgTalk.m_var.m_share_eglContext != null) str1 += "share_egl ok";
				else str1 += "share_egl null";

				//
				total_str += str1;

			}

            qyFuncs.showInfo_open(0,"",total_str  +  " " + str);

#endif
            return 0;

		}



		//
		public static unsafe bool bDone_retrieveAllImObjRules(MIS_CNT pMisCnt)
		{
			return true;
			//
			return pMisCnt.refreshImObjRules.tLastRefreshedTime != 0 ? true : false;
		}



		//
		public static unsafe bool bDlgTalkActive(object hDlgTalk)
        {
			CCtxQyMc pQyMc = (CCtxQyMc)g.g_pQyMc;
			MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

			if (pQyMc.gui.hLastActiveTalk == hDlgTalk) return true;
			return false;
		}

		//
		public static unsafe int talkToMessenger(object pQyMcParam, MSGR_ADDR* pAddr, bool bNeedNotShowWnd, bool bActivateWnd, ref PARAM_talkToMessenger param)
		{

			int iErr = -1;
			CCtxQyMc pQyMc = (CCtxQyMc)pQyMcParam;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null == pProcInfo) return -1;

			string str;

			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

			//HWND hWnd = NULL;
			int iRet;
			//HWND hFore = NULL;
			//HWND hParent = NULL;

			//MACRO_CDlgTalk* pDlgTalk = NULL;

			int nStep = 0;

			if (null == pAddr) return -1;

			//
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("talkToMessenger %I64u, %d: enters"), pAddr->idInfo.ui64Id, pAddr->uiTranNo_shadow);



			//  2014/04/29
			if (!bDone_retrieveAllImObjRules(pMisCnt))
			{
				str = ("talkToMessenger failed: not all imObjRules retrieved");
				qyFuncs.showInfo_open(0, null, str);

				//
				return -1;
			}

			//  2011/03/01
			int index = pMisCnt.talkingFriendQ.FindIndex(x => x.addr.idInfo.ui64Id == pAddr->idInfo.ui64Id);
			if (index >= 0)
			{
				if (bNeedNotShowWnd)
				{
					//phWnd = pMisCnt.talkingFriendQ[index].hWnd;
					iErr = 0; goto errLabel;
				}

				if ( bDlgTalkActive(pMisCnt.talkingFriendQ[index].hWnd))
				{
					Activity_dlgTalk wnd = (Activity_dlgTalk)pMisCnt.talkingFriendQ[index].hWnd;
					//
					qyFuncs.traceLog("need to show dlgTalk");
					//
					iErr = 0; goto errLabel;
				}

				//
				Activity_dlgTalk dlgTalk = (Activity_dlgTalk)pMisCnt.talkingFriendQ[index].hWnd;
				dlgTalk.doFinish("talkToMessenger, ");
			}

			//
			Context context = mainActivity;// Android.App.Application.Context;
			//
			Intent intent = new Intent(context, typeof(Activity_dlgTalk));
			//
			intent.PutExtra("ui64Id", (long)pAddr->idInfo.ui64Id);
			//
			intent.PutExtra("info", "haha");
			//
			intent.PutExtra("wParam", param.wParam);
			intent.PutExtra("lParam", param.lParam);
			//
			mainActivity.StartActivity(intent);



			iErr = 0;

		errLabel:

			if (0 == iErr)
			{
				//
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("talkToMessenger %I64u, %d: leaves %s, nStep %d"), pAddr->idInfo.ui64Id, pAddr->uiTranNo_shadow,	   iErr ? _T("failed") : _T("OK"), nStep);


			}
				return iErr;

			


		}


		public static unsafe int dlgTalk_set_newMsgArrives(object hDlgTalk, DLG_TALK_var pm_var, bool bOn)
        {
			return 0;

        }

		
		//
		public static unsafe bool dlgTalk_canSpeak(object hDlgTalk_mgr)
		{
			bool bRet = false;


            //
            //using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk_mgr = (Activity_dlgTalk)hDlgTalk_mgr;

            {
                ;
				DLG_TALK_var pm_var = dlgTalk_mgr.m_var;
				//(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk_mgr, _T("dlgTalk_canSpeak"));
				if (null==pm_var) return false;
				DLG_TALK_var  m_var = pm_var;
				//
				if (!isTalkerShadowMgr(ref m_var.addr)) return false;
				//
				CCtxQyMc pQyMc = g.g_pQyMc;
				CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //  (  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
				MIS_CNT pMisCnt = pProcInfo.pMisCnt;
				MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
				QY_SHARED_OBJ pSharedObj = null;
				SHARED_OBJ_USR pSharedObjUsr = null;

				if (!m_var.av.taskInfo.bTaskExists) return false;
				QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller,pProcInfo, m_var.av.taskInfo.iIndex_taskInfo,"dlgTalk_canSpeak");
				if (null==pTaskInfo) goto errLabel;
				QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
				if (pTaskData == null) goto errLabel;
				if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
				QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
				//DLG_TALK_videoConference pVc = pTc.videoConference;


				if (null==(pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv))) goto errLabel;
				if (pSharedObj.var.iIndex_curUsr != m_var.av.iIndex_usr_localAv) goto errLabel;        //  2010/09/07
				if (null==(pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, m_var.av.iIndex_usr_localAv))) goto errLabel;

				//  if  (  m_var.av.taskInfo.ucbVideoConference  )  
				{   //  2010/09/04
					RES_obj_simple ros = new RES_obj_simple();
					if (!qyFuncs.isMemFromActive(pTc, pMisCnt.idInfo, ref ros,null) 
						//|| pTc.videoConference.bNeedRefresh_activeMems_from
						) goto errLabel;
				}

				if (pTc.ucbNotSendData) goto errLabel;

				if (0==pTaskInfo.var.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id && 0==pTaskInfo.var.curRoute_sendLocalAv.route.idInfo_to.ui64Id) goto errLabel;

			}

			bRet = true;
		errLabel:
			return bRet;

		}

		public static unsafe int askToRefreshLayout_mgr(object hDlgTalk)
        {
			return -1;
        }


	}
}
