using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
{

		public static unsafe int confStarter_fillGrpMems(object hDlgTalk, DLG_TALK_var pm_var)
		{
			int iErr = -1;
			DLG_TALK_var  m_var = pm_var;

			/*
			QY_MC* pQyMc = QY_GET_GBUF();
			MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();
			CQySyncCnt syncCnt;
			PARAM_task_info param;
			int i;

			//
			if (GetCurrentThreadId() != pQyMc->gui.ctx_gui_thread.dwThreadId)
			{
				showInfo_open(0, 0, _T("confStart_fillGrpMems failed, not mainThread"));
				return -1;
			}

			//
			if (!isTalkerShadowMgr(pm_var->addr)) return -1;

			//
# ifdef  __DEBUG__
#if  10
		traceLog(  _T(  "confStart_fillGrpMems called"  )  );
#endif
#endif


			//
			if (syncToGetTaskInfo(&m_var, syncCnt, param)) goto errLabel;

			QMC_taskData_conf* pTaskData = param.pTaskData;

			if (!pTaskData->bInited)
			{

				void* pDb = NULL;
# ifndef  __NOTSUPPORT_DB__
				CQnmDb db;
				if (!db.getAvailableDb(pQyMc->iDsnIndex_mainSys)) goto errLabel;
				pDb = db.m_pDbMem->pDb;
#endif

				QY_MESSENGER_ID* pIdInfo_grp = &m_var.addr.idInfo;
				int n = getnGrpMems_isCli(pDb, pQyMc->cfg.db.iDbType, pIdInfo_grp);
				if (n <= 0)
				{
					goto errLabel;
				}
				if (n > CONST_qmObjQMaxCnt_misClient)
				{
					showInfo_open(0, 0, _T("too big nGrpMems"));
					goto errLabel;
				}
				//
				pTaskData->usCnt_pMems = n;
# ifdef  __BGWALL_skip_me__
				pTaskData->usCnt_pMems--;
				if (!pTaskData->usCnt_pMems) goto errLabel;
#endif
				//
				int len;
				len = pTaskData->usCnt_pMems * sizeof(CONF_mem);
				MACRO_safeFree(pTaskData->pMems);
				pTaskData->pMems = (CONF_mem*)mymalloc(len);
				if (!pTaskData->pMems) goto errLabel;
				memset(pTaskData->pMems, 0, len);

				//
#if  0  //  2017/12/26
		int  cnt  =  0;
		enumGrpMems_isCli(  pDb,  pQyMc->cfg.db.iDbType,  pIdInfo_grp,  tmpHandler_AddGrpMemToTaskdata,  &cnt,  pTaskData  );
#endif

				//  
				pTaskData->usMaxToDisplay = 3;
				pTaskData->usMaxToDisplay = min(pTaskData->usCnt_pMems, pTaskData->usMaxToDisplay);
				//
				pTaskData->usPollingIntervalInS = DEFAULT_usPollingIntervalInS;

				//
				memset(&pTaskData->cur_toDisplay, 0, sizeof(pTaskData->cur_toDisplay)  );

				//
				pTaskData->bInited = TRUE;

			}

			*/

			iErr = 0;
		errLabel:
			return iErr;
		}


		public static unsafe int doCmd_startAvCall(object hParent, object hCurTalk, int level, bool b3D, bool ucbAvConsole, PARAM_startAvCall  * pParam)
		{
			int iErr = -1;

			
			object hMgr = null;
			DLG_TALK_var pMgrVar = null;

			DLG_TALK_var pCurVar = null;
			//
			bool bTaskStarted = false;          //  2014/10/31

			//  
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null == pProcInfo) return -1;
			FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
			if (null == pFuncs) return -1;
			MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			PARAM_startAvCall param;
			qyFuncs.mymemset((IntPtr)(byte*)&param, 0, sizeof(PARAM_startAvCall));
			if (pParam != null) param = *pParam;

			//
			ushort usConfType = 0;
			bool bNoPrompt = false;
			int iCapUsage = 0;

			//
			qyFuncs.traceLog("doCmd_startAvCall called");

            //
            //using (CHelp_getDlgTalkVar help_getDlgTalkVar_cur = new CHelp_getDlgTalkVar())

            {
               // CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;


				//
				VIDEO_CONFERENCE_CFG videoConferenceCfg;
				bool bVideoConference = false;
				int iIndex_sharedObj = -1;
				int iIndex_sharedObjUsr = -1;
				//  
				QY_SHARED_OBJ pSharedObj = null;


				bool bFirstUsr = false;
				int iTaskId = 0;                //  2014/10/29




				using (CQySyncObj syncObj = new CQySyncObj(), syncObj_sync_taskInfo = new CQySyncObj())
				{                       //  2016/03/15

					//
					TMP_tBuf128 tBuf;
                    //TCHAR tBuf[256] = _T("");

                    Activity_dlgTalk curTalk = (Activity_dlgTalk)hCurTalk;

					//
					pCurVar = curTalk.m_var;
					//(DLG_TALK_var)help_getDlgTalkVar_cur.getVar(hCurTalk);
					if (null == pCurVar) return -1;
					hMgr = hCurTalk;
					pMgrVar = pCurVar;
					//

					//

					//  2009/10/11	
					//TCHAR tHint[128] = _T("");
					TMP_tHintBuf tHint;
					//
					if (pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp)
					{
						//
						if (!pQyMc.appParams.canStartConf)							{
							//
								usConfType = 0;

							//
							bNoPrompt = bNoPrompt_selectAvCompressor(Consts.CONST_capType_av, 0);
							if (pQyMc.appParams.bNoPrompt)
							{
								bNoPrompt = true;
							}

							//
							b3D = true;

							//
							if (!bNoPrompt)
							{
								//if (viewDlgSelectAvCompressor(hParent, hCurTalk, CONST_capType_av, 0, FALSE, b3D, usConfType) != IDOK) goto errLabel;
							}

							uint uiCapType = Consts.CONST_capType_av;
							uint uiSubCapType = 0;
							iCapUsage = Consts.CONST_capUsage_3dConf;
							//
							int iResId = 0;// gAvParams.iResId_policyAvLevelTable;


							//
							if (0==level) level = getPolicyAvLevel(pQyMc, uiCapType, uiSubCapType, iCapUsage);
							//if (!qyGetDesByType(getResTable(0, &pQyMc->cusRes, iResId), level)) level = getDefaultPolicyAvLevel(pQyMc, uiCapType, uiSubCapType, iCapUsage);


							//
							int conf_iFourcc = get_conf_iFourcc();
							ushort usMaxSpeakers = (ushort)get_conf_usMaxSpeakers();


							//
							sendConfReq(null, hMgr, pMgrVar.addr.idInfo, 0, (ushort)level, conf_iFourcc, usMaxSpeakers);

							//
							return 0;
						}


					}

					//  2015/07/29

					//
					if (null == pMisCnt) goto errLabel;
					//
					bool ucbNotUseAvCall = false;
					//

					//



					//
					if (pMgrVar.av.taskInfo.bTaskExists)
					{
						qyFuncs.showInfo_open(0, null, ("doCmd_startAvCall failed, av.bTaskExists is true"));
						goto errLabel;
					}

					//  2017/08/25
					usConfType = 0;

					//


					//
					bNoPrompt = bNoPrompt_selectAvCompressor(Consts.CONST_capType_av, 0);

					//  2018/10/28

					//
					fixed (char* pName = pProcInfo.cfg.mutexName_syncStartAv)
					{
						if (0!=syncObj.sync(new string(pName), pMisCnt.hMutex_syncStartAv)) goto errLabel;
					}

					//  2014/12/24
					if (pMgrVar.av.taskInfo.bTaskExists)
					{
						qyFuncs.showNotification_open(0, 0, 0, ("av is started already"));
						goto errLabel;
					}
					//
					/*
					if (0!=qyFuncs.syncMtCnt_wLock_wait(pMgrVar.av.syncMtCnt_taskInfo, null, null,syncObj_sync_taskInfo, null, ("doCmd_startAvCall")))
					{
						goto errLabel;
					}
					*/
					//  2014/10/31
					//assert(!pMgrVar.av.taskInfo.bTaskExists);
					//memset(&pMgrVar.av.taskInfo, 0, sizeof(pMgrVar.av.taskInfo));
					pMgrVar.av.taskInfo.memset0();
					pMgrVar.av.taskInfo.bTaskExists = true;
					//
					bTaskStarted = true;

					//  2016/03/06
					exitTalkerThread(pProcInfo, pMgrVar.talkerThreadInfo);
					if (0!=initTalkerThread(pProcInfo, hMgr, pMgrVar, pMgrVar.talkerThreadInfo))
					{
						goto errLabel;
					}

					//
					if (0!=qyFuncs.getNewSharedObjUsr_localAv(pProcInfo, hMgr, Consts.CONST_qyWndContentType_talker, &iIndex_sharedObj, &iIndex_sharedObjUsr, &bFirstUsr))
					{
						//  2017/02/12
#if false
						_sntprintf(tHint, mycountof(tHint), getResStr(0, &pQyMc.cusRes, CONST_resId_notStartAv));
						showNotification(0, 0, 0, 0, 0, 0, tHint);
						//
						if (!bUseD3DFullScreen())
						{
							qyFuncs.myMessageBox(hParent, tHint, ("www.qycx.com"), MB_OK);
						}
#endif
			goto errLabel;          //  ÕâÀï¹Ì¶¨ÓÃ0
					}
					//
					pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
					if (null==pSharedObj)
					{
						qyFuncs.showInfo_open(0, null, ("startAvCall failed: getSharedObj return  null"));
						goto errLabel;
					}
					//
					SHARED_OBJ_USR pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, iIndex_sharedObjUsr);
					if (null==pSharedObjUsr)
					{
						goto errLabel;
					}
					//
					pMgrVar.av.ucbGot_iIndex_sharedObj_localAv = true;
					pMgrVar.av.iIndex_sharedObj_localAv = iIndex_sharedObj;
					pMgrVar.av.iIndex_usr_localAv = iIndex_sharedObjUsr;

					//  2011/08/07
					AV_COMPRESSOR_CFG avCompressor_req=new AV_COMPRESSOR_CFG();
					if (0!=myGetAvCompressorCfg(Consts.CONST_capType_av, 0, 0, level, ref avCompressor_req)) goto errLabel;
					AV_COMPRESSOR_CFG avCompressor_conf=new AV_COMPRESSOR_CFG();    //  2014/08/15
															//  2014/12/15
					iCapUsage = b3D ? Consts.CONST_capUsage_3dConf : Consts.CONST_capUsage_conf;
					//
					if (0!=myGetAvCompressorCfg(Consts.CONST_capType_av, 0, iCapUsage, level, ref avCompressor_conf)) goto errLabel;

					//  2014/10/07
					TALKER_policy policy;
					getTalkerPolicy(pMgrVar.addr.idInfo, out policy);
					//


					//
					//pMgrVar.m_layout.ucbShowBgWall_starter  =  policy.ucbShowBgWall_starter;
					//pMgrVar.m_layout.usPollingIntervalInS1  =  pShadowMgr->bgWall.bgWallCfg.usPollingIntervalInS;
					//pMgrVar.m_layout.bgWallLayoutCfg.usRows_bg = pShadowMgr->bgWall.bgWallCfg.bgWallLayoutCfg.usRows_bg;
					//pMgrVar.m_layout.bgWallLayoutCfg.usCols_bg = pShadowMgr->bgWall.bgWallCfg.bgWallLayoutCfg.usCols_bg;
					//  2009/06/04
					//  memcpy(  &videoCompressor,  &avCompressor_req.video,  sizeof(  videoCompressor  )  );
					switch (pMgrVar.addr.uiObjType)
					{
						case Consts.CONST_objType_imGrp:
							if (0!=getVideoConferenceCfg(null, "", "", pMgrVar.addr.idInfo, out videoConferenceCfg)) goto errLabel;
							//  2014/12/05
							//
							bVideoConference = true;

							//
							if (policy.ucbConfCtrl_starter)
							{
								pMgrVar.av.taskInfo.ucbConfCtrl = true;
								//
								//resetGrpConfCtrl(hMgr, &pShadowMgr->confCtrl.grpConfCtrl);
							}
							break;
						default:
							break;
					}


					//
					if (bFirstUsr)
					{
						CAP_procInfo_recordSound pCapAudio = null;         //  2013/03/19
						CAP_procInfo_video pCapBmp = null;

						//
						pCapAudio = (CAP_procInfo_recordSound)qyFuncs.getCapAudioBySth(pProcInfo, pSharedObj.var.iIndex_capAudio, 0);
						if (null==pCapAudio) goto errLabel;
						pCapBmp = (CAP_procInfo_video)qyFuncs.getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
						if (null==pCapBmp) goto errLabel;

						//
						pCapAudio.iIndex_sharedObj = iIndex_sharedObj;
						pCapAudio.uiTranNo_sharedObj = pSharedObj.var.uiTranNo;

						//
						pCapBmp.iIndex_sharedObj = iIndex_sharedObj;
						pCapBmp.uiTranNo_sharedObj = pSharedObj.var.uiTranNo;


						//  2013/03/19. 这里要清理一下
						pCapAudio.uiType = 0;
						pCapBmp.uiType = 0;

						//  2013/09/20
						qyFuncs.resetChromaKey();

						{     //  老的工作模式. 2013/03/19

							//
							if (true)
							{

								uint uiRecordType = Consts.CONST_recordType_android;//pProcInfo.cfg.policy.audio.uiRecordType
								if (0 == startLocalAudioRecorder(uiRecordType, null, "", iIndex_sharedObj, ref avCompressor_req.audio))
								{
									//  pMgrVar.av.ucbLocalAudioRecorderStarted  =  true;
								}
#if __DEBUG__
					//  if  (  !pMgrVar.av.ucbLocalAudioRecorderStarted  )  traceLogA(  "CDlgTalk::Onstartavcall: startLocalAudioRecorder failed."  );
#endif
							}
							else
							{
#if __DEBUG__
					traceLog(_T("Warning: NotRecordSound is true"));
#endif
							}

							//  
							//if (pQyMc.iCustomId != Consts.CONST_qyCustomId_hzj								&& 1)
							if(true)
							{

								//  2011/07/27						
								if (0!=qyFuncs.newstartQThreadToShareAv(pProcInfo, iIndex_sharedObj, false))
								{
									goto errLabel;
								}

								//
								if (!pProcInfo.av.localAv.videoCaptureProcInfo.bCapDevConnected)
								{

									{

										//TCHAR displayName[MAX_monikerDisplayNameLen + 1] = _T("");
										string displayName = "";

										//
										avCompressor_req.video.shared_eglContext = pMgrVar.m_share_eglContext;

										//if (getMonikerDisplayName(getMoniker_v(pMgrVar.pCapStuff, pMgrVar.iMenuId_selectedVideo), displayName, mycountof(displayName))) displayName[0] = 0;
										uint uiCamCapType = pProcInfo.cfg.policy.video.uiCamCapType;
										if (0==qyFuncs.openLocalVideo(pProcInfo, uiCamCapType, displayName, hMgr,Consts.CONST_qyWndContentType_talker, ref avCompressor_req.video, iIndex_sharedObj))
										{
											//  pMgrVar.av.u.av.uc.ucbLocalVideoOpen  =  true;				
										}
										//if (!pMgrVar.pCapStuff) dlgTalk_make_capStuff(hMgr, pMgrVar);

#if __DEBUG__
									//  if  (  !pMgrVar.av.ucbLocalVideoOpen  )  traceLogA(  "CDlgTalk::Onstartavcall: openLocalVideo failed."  );
#endif
									}
								}
								if (pProcInfo.av.localAv.videoCaptureProcInfo.bCapDevConnected)
								{
									pProcInfo.av.localAv.videoCaptureProcInfo.curhWnd = hMgr;

									//
									//  2014/09/23. 建立多流						
									level = 0;
									if (0 != makeSlaveStream(hMgr, iIndex_sharedObj, level)) goto errLabel;

								}
							}

						}
					}

					//  2014/10/29
					{
						object pDb = null;
						using (CQnmDb db = new CQnmDb())
						{
							if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
							pDb = db.m_pDbMem.pDb;

							iTaskId = qyFuncs.newTaskId(pDb, pQyMc.cfg.db.iDbType);
							pMgrVar.av.taskInfo.iTaskId = iTaskId;
						}
					}
					//  2014/10/30
					if (pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp)
					{
						if (b3D)
						{
#if false
							if (makeMosaicStream(hMgr, iTaskId, level, &avCompressor_conf, &pMgrVar.av.taskInfo.mosaicMaker))
							{
								goto errLabel;
							}
							//
							QMC_mosaic_maker* pMosaic; pMosaic = &pMgrVar.av.taskInfo.mosaicMaker;
							QY_SHARED_OBJ* tmp_pSharedObj;
							if (pMosaic->video.var.ucbLocalVideoOpen)
							{
								tmp_pSharedObj = getSharedObjByIndex(pProcInfo, pMosaic->video.var.iIndex_sharedObj);
								if (tmp_pSharedObj)
								{
									addToMsgrs_sendLocalAv(pProcInfo, pMgrVar.pMisCnt, &pMgrVar.addr.idInfo, true, &tmp_pSharedObj.curRoute_sendLocalAv);
								}
							}
							if (pMosaic->resource.var.ucbLocalVideoOpen)
							{
								tmp_pSharedObj = getSharedObjByIndex(pProcInfo, pMosaic->resource.var.iIndex_sharedObj);
								if (tmp_pSharedObj)
								{
									addToMsgrs_sendLocalAv(pProcInfo, pMgrVar.pMisCnt, &pMgrVar.addr.idInfo, true, &tmp_pSharedObj.curRoute_sendLocalAv);
								}
							}
		
#endif
		}
					}

					//  2016/03/14
					P_sendAvInfo_taskInfo pst=new P_sendAvInfo_taskInfo();
					//qyFuncs.mymemset((IntPtr)(byte*)&pst, 0, sizeof(P_sendAvInfo_taskInfo));
					//  2017/08/02
					pst.i.ucbVideoConference = (pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp);
					//  2017/07/17
					pst.i.ucbAvConsole = ucbAvConsole;
					//  2017/08/25
					pst.i.usConfType = usConfType;
					//
					pst.i.shared_eglContext = pMgrVar.m_share_eglContext;
		
					//  2017/09/06	
					//
					dlgTalk_init_respInfo(hMgr, pMgrVar, pst.i.ucbVideoConference);


					//
					switch (pMgrVar.addr.uiObjType)
					{
						case Consts.CONST_objType_imGrp:

							ref QMC_mosaic_maker pMosaic = ref pMgrVar.av.taskInfo.mosaicMaker;
							
								if (0!=qyFuncs.procSendAvInfo_conf(ref mainActivity.var_common.ctxCaller,pMisCnt, hMgr, true, Consts.CONST_imTaskType_transferAvInfo, &videoConferenceCfg, &policy, ref avCompressor_conf, ref pMosaic, ref pProcInfo.av.localAv.videoCaptureProcInfo.compressVideo, ref pProcInfo.av.localAv.recordSoundProcInfo.compressAudio, iIndex_sharedObj, iIndex_sharedObjUsr, null, null, &iTaskId, ref pMgrVar.addr, ref pst)) goto errLabel;
							
							break;
						case Consts.CONST_objType_tmpGrp:
#if __DEBUG__
						traceLog(_T("Not supported: tmpGrp."));
#endif
							goto errLabel;
							break;
						default:
							//				   
							if (0!=qyFuncs.procSendAvInfo(ref mainActivity.var_common.ctxCaller,pMisCnt, hMgr, true, Consts.CONST_imTaskType_transferAvInfo, ref pProcInfo.av.localAv.videoCaptureProcInfo.compressVideo, ref pProcInfo.av.localAv.recordSoundProcInfo.compressAudio, iIndex_sharedObj, iIndex_sharedObjUsr, null, null, &iTaskId, ref pMgrVar.addr, ref pst)) goto errLabel;

							break;
					}

					//  2016/03/14
#if true
					DLG_TALK_var m_var = pMgrVar;
					//MIS_CNT  *  pMisCnt  =  pMgrVar.pMisCnt;
					ref MSGR_ADDR pAddr = ref pMgrVar.addr;
					int lenInBytes = 0;
					//
					if (!m_var.av.taskInfo.bTaskExists)
					{  //  2014/10/31
						qyFuncs.showInfo_open(0, null, ("procSendAvInfo failed, taskInfo.bTaskExists is false"));
						goto errLabel;
					}

					//
					m_var.av.taskInfo.iTaskId = pst.o.iTaskId;
					//  m_var.av.taskInfo.iIndex_sharedObj  =  iIndex_sharedObj;
					m_var.av.taskInfo.idInfo_starter.ui64Id = pst.o.idInfo_starter.ui64Id;

					m_var.av.taskInfo.tStartTime_org = pst.o.tStartTime_org;
					m_var.av.taskInfo.uiTranNo_org = pst.o.uiTranNo_org;
					m_var.av.taskInfo.uiContentType_org = pst.o.uiContentType_org;

					m_var.av.taskInfo.ucbStarter = pst.o.ucbStarter;                                    //  2010/08/31
					m_var.av.taskInfo.ucbVideoConference = pst.o.ucbVideoConference;                    //  2010/08/31
																										//  m_var.av.taskInfo.ucbVideoConferenceStarter  =  pContent->myTask.ucbVideoConferenceStarter;

					//  2017/08/25
					m_var.av.taskInfo.usConfType = usConfType;

					//
					m_var.av.taskInfo.uiInitW = pst.o.uiInitW;
					m_var.av.taskInfo.uiInitH = pst.o.uiInitH;

					//  2014/09/25
					m_var.av.taskInfo.iIndex_taskInfo = pst.o.iIndex_taskInfo;


					//  2010/08/31
					if (!m_var.av.taskInfo.ucbVideoConference)
					{
						//  m_var.av.taskInfo.bNeedAutoRequestToSpeak  =  true;

						//
						m_var.av.taskInfo.bShow_localVideo = true;
					}

					//
					//
					fixed (TALK_drawLayout* pLayout = &m_var.m_drawLayout)
					{
						qyFuncs.mymemset((IntPtr)pLayout, 0, sizeof(TALK_drawLayout));
					}

					//
					//
					QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller,pProcInfo, m_var.av.taskInfo.iIndex_taskInfo,"doCmd_startAvCall");
					if (null==pTaskInfo) goto errLabel;
					QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
					if (null == pTaskData) goto errLabel;
					if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
					QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
					DLG_TALK_videoConference videoConference = pTc.getVideoConference();

					//
					if (m_var.av.taskInfo.ucbVideoConference)
					{
						pTc.grp.idInfo_initiator.ui64Id = param.idInfo_initiator.ui64Id;
					}

					//
					uint uiTaskType = Consts.CONST_imTaskType_transferAvInfo;

					//
					if (uiTaskType == Consts.CONST_imTaskType_transferAvInfo)
					{
						if (!m_var.av.taskInfo.ucbVideoConference)
						{
							if (0!=qyFuncs.addToMsgrs_sendLocalAv(pProcInfo, pMisCnt, m_var.addr.idInfo, false, ref pTaskInfo.var.curRoute_sendLocalAv)) goto errLabel;
						}
					}


					//
					if (true)
					{
						if (uiTaskType != Consts.CONST_imTaskType_viewDynBmp)
						{   //  2014/09/26
							m_var.av.taskInfo.local_avStream = pst.o.local_avStream;
						}
					}

						//  				
						if (!m_var.av.taskInfo.ucbVideoConference
						//||  pQyMc.iCustomId  ==  CONST_qyCustomId_hzj  
						)
					{
						//  2018/10/28. hzj需要自动发言
						//  2013/04/09. 在一对一时,自动将自己加入话筒列表,以便显示.				
						DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = null;

						pActiveMem_from = videoConference.activeMems_from[0];
						if (pActiveMem_from.avStream.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
						{
							fixed (char* pDesc=pActiveMem_from.desc.desc)
							{
								qyFuncs.getTalkerDesc(pMisCnt, pActiveMem_from.avStream.idInfo, pDesc, Consts.cntof_desc, null, 0);
								//_sntprintf(m_var.videoConference.activeMems_from[0].desc, mycountof(m_var.videoConference.activeMems_from[0].desc), _T("%s(%I64u)"), pMisCnt.displayName, pMisCnt.idInfo.ui64Id);
							}
						}
#if __DEBUG__
					traceLog(_T("Note: 	在一对一时,自动将自己加入话筒列表,以便显示."));
#endif
					}
#if false
					if (!m_var.av.taskInfo.ucbVideoConference)
					{
						//DLG_TALK_videoConferenceActiveMemFrom* pActiveMem_from = null;
						//  2013/04/10
						//if (null!=pAddr)
						{
							DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = ref pTc.videoConference.activeMems_from[1];
							{
								pActiveMem_from.avStream.idInfo.ui64Id = pAddr.idInfo.ui64Id;
								//_sntprintf(pActiveMem_from->desc, mycountof(pActiveMem_from->desc), _T("%s(%I64u)"), _T(""), pAddr.idInfo.ui64Id);
							}
						}
					}
#endif

					//
					if (pMgrVar.iTalkerSubType != Consts.CONST_talkUsage_doConf)
					{
						if (pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp)
						{
							if (0 != confStarter_fillGrpMems(hMgr, pMgrVar))
							{
								qyFuncs.showInfo_open(0, null, ("doCmd_startAvCall: confStart_fillGrpMems failed"));
								goto errLabel;
						}
						}
					}
#endif

					//  2016/03/15
#if false
					if (syncMtCnt_start(&pMgrVar.av.syncMtCnt_taskInfo, pFuncs.isCliHelp.pf_getuiNextTranNo))
					{
						goto errLabel;
					}
		
#endif


					//
					if (bFirstUsr)
					{
						if (pProcInfo.av.localAv.videoCaptureProcInfo.bCapDevConnected)
						{
							qyFuncs.startCapture(hMgr, Consts.CONST_qyWndContentType_talker, pProcInfo.av.localAv.videoCaptureProcInfo);
						}
					}

					//
					qyFuncs.setCurSharedObjUsr_localAv(pProcInfo, iIndex_sharedObj, hMgr, "doCmd_startAvCall.725");
					//  2015/05/23
					if (b3D)
					{
						pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, iIndex_sharedObjUsr);
						if (null==pSharedObjUsr) goto errLabel;
						if (pSharedObjUsr.hWnd_starter != hMgr) goto errLabel;
						pSharedObjUsr.bIn3DConf = true;
					}

					//
					qyFuncs.resetTalkerList_mgr(hMgr);

				}

			}

			//			
			//
			setProcessingAvTask(pMgrVar.av.taskInfo.iIndex_taskInfo, pMgrVar.av.taskInfo.iTaskId);

			//
			qyFuncs.bChkMessengerChannelsReady(pQyMc);

			//
#if DEBUG
			if (pMgrVar.av.taskInfo.bTaskExists)
			{
				//qyFuncs.tmp_showTc( ref mainActivity.var_common.ctxCaller,pMgrVar.av.taskInfo.iTaskId, "doCmd_startAvCall");
			}
#endif


			//
			iErr = 0;
errLabel:

//closeInstantAssistant(hMgr);        //  2011/10/04

if (0!=iErr)
{
if (bTaskStarted)
{   //  2014/10/31
	if (null!=pMgrVar)
	{
	}
}

//	

}

			if (pMgrVar != null)
			{
				GuiShare.pf_talk_addNewMsg(hCurTalk, pMgrVar.av.taskInfo.idInfo_starter, pMgrVar.av.taskInfo.tStartTime_org, pMgrVar.av.taskInfo.uiTranNo_org, 0, 0, "视频呼叫", pMgrVar.av.taskInfo.iTaskId);
			}

			/*



			if (pMgrVar)
			{
			dlgTalk_displayAvStatus(hMgr, *pMgrVar, 0, 0, 0);
			}

			//  sizeAllControls_dlgTalk(  hMgr,  m_var,  null  );						//  
			askToRefreshLayout_mgr(hMgr);

			//  2013/07/20
			if (pMgrVar)
			{
			if (isTalkerShadowMgr(pMgrVar.addr))
			{
				//  d3dWall_refreshImgs(  hMgr  );
				dyn_d3dWall_refreshWallMemByTalker(hMgr);
			}
			}

			//  2017/07/07
			refreshTalkToInfo_mgr(hMgr);
					*/

			return iErr;
}


		public static int makeSlaveStream(object hDlg, int iIndex_sharedObj_master, int level)
        {
			qyFuncs.traceLog("not finished");

			return 0;
        }

		//
		public static int setProcessingAvTask(int iIndex_taskInfo, int iTaskId)
		{
			QY_MC_mainWndVar mainWndVar = (QY_MC_mainWndVar)GuiShare.pf_mainWnd_getVar();

            if (mainWndVar.processingAvTask.bTaskExists)
            {
				qyFuncs.traceLog("processingAvTask.bTaskExists already is true");
            }

			mainWndVar.processingAvTask.bTaskExists = true;
			mainWndVar.processingAvTask.index_taskInfo = iIndex_taskInfo;
			mainWndVar.processingAvTask.iTaskId = iTaskId;

			return 0;
		}

		public static unsafe int clearProcessingAvTask()
		{
			QY_MC_mainWndVar mainWndVar = (QY_MC_mainWndVar)GuiShare.pf_mainWnd_getVar();

			fixed(PROCESSING_avTask * p = &mainWndVar.processingAvTask)
            {
				qyFuncs.mymemset((IntPtr)p, 0, sizeof(PROCESSING_avTask));
            }
			return 0;


		}


	}
}
