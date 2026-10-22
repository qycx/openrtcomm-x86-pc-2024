


using Java.Lang;
using Math = Java.Lang.Math;

namespace qy.Droid
{

    partial class qyFuncs_Droid
    {




		public static unsafe int dlgTalk_procTask_transferFileReq(object hCurDlgTalk, ref DLG_TALK_var pCurVar, object hMgr, ref DLG_TALK_var pMgrVar, int iTaskId, ushort usOp, string paramStrBuf,
	ref IM_TASK_RCD  	rcd, MIS_MSG_TASK* pMsgTask_unused, IM_CONTENTU* pContent,
	ref int    iStatus, ref int    iStatus_old,
	QM_dbFuncs     	g_dbFuncs,
	object pDb //,
	//TALKER_shadow_mgr* pShadowMgr
			)
		{
			int iErr = -1;

			object hDlgTalk = hMgr;
			ref DLG_TALK_var  m_var = ref pMgrVar;

			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			int i;
			int lenInBytes;

			string str;

			//
			if (null==pMsgTask_unused)
			{
				return -1;
			}

			//
			if (rcd.uiContentType != Consts.CONST_imCommType_transferFileReq) return -1;

			//	
			int iIndex_taskInfo = -1;

			iIndex_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null,pProcInfo, iTaskId);
			if (iIndex_taskInfo < 0)
			{
#if false
				iIndex_taskInfo = newTaskInfoIndex(pProcInfo, CONST_taskDataType_transferFile, hMgr, iTaskId, _T("dlgTalk_procTask_transferFileReq"));
				if (iIndex_taskInfo < 0)
				{
					traceLogA((char*)"dlgTalk_procTask_transferAvInfo failed, index_taskInfo <0");
					goto errLabel;
				}
				QMC_TASK_INFO* pTaskInfo = (QMC_TASK_INFO*)getQmcTaskInfoByIndex(pProcInfo, iIndex_taskInfo);
				int tmpLen = pMsgTask->lenInBytes + offsetof(MIS_MSG_TASK, data);
				memcpy(&pTaskInfo->var.pTaskData->msgU.task, pMsgTask, tmpLen);
#endif
				goto errLabel;

			}
			//	
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null,pProcInfo, iIndex_taskInfo,"");
			QMC_taskData_common taskData = pTaskInfo.getTaskData();
			fixed (MIS_MSG_TASK*pMsgTask = &taskData.msgU.task) {;
				if (pMsgTask->uiType != Consts.CONST_misMsgType_task)
				{
					goto errLabel;
				}
				if (pMsgTask->iTaskId != iTaskId)
				{
					return -1;
				}




				//
				switch (usOp)
				{
					case Consts.CONST_imOp_recv_accept:
					case Consts.CONST_imOp_recvFile_saveas:
					case Consts.CONST_imOp_dual_permit:
						{

							//  
							bool ucbSendLocalAv = false;
							int iIndex_sharedObj = -1;
							int iIndex_sharedObjUsr = -1;
							QY_SHARED_OBJ pSharedObj = null;
							int iIndex_capBmp = -1;
							CAP_procInfo_video pCapBmp = null;
							int iIndex_capAudio = -1;
							CAP_procInfo_audioCommon pCapAudio = null;
							int iIndex_shmCmd = -1;
							//QM_SHM_CMD pShmCmd = null;
							bool bFirstUsr = false;
							SHARED_OBJ_USR pSharedObjUsr = null;
							TALKER_policy talkerPolicy;

							//  2011/08/08
							//AV_COMPRESSOR_CFG avCompressor_reply;       //  响应要取得compressor, 目前只有av才有需要. 2012/02/16
																		//  if  (  myGetAvCompressorCfg(  CONST_capType_av,  0,  CONST_capUsage_conf,  0,  &avCompressor_reply  )  )  goto  errLabel;
							//if (0!=myGetAvCompressorCfg(Consts.CONST_capType_av, 0, 0, 0, ref avCompressor_reply)) goto errLabel;    //  2014/08/15

							getTalkerPolicy(m_var.addr.idInfo, out talkerPolicy);
							//m_var.m_layout.ucbNeverJoinInBgWall  =  talkerPolicy.ucbNeverJoinInBgWall;
							//m_var.m_layout.usPollingIntervalInS1  =  pShadowMgr->bgWall.bgWallCfg.usPollingIntervalInS;
#if false
							m_var.m_layout.bgWallLayoutCfg.usRows_bg = pShadowMgr->bgWall.bgWallCfg.bgWallLayoutCfg.usRows_bg;
							m_var.m_layout.bgWallLayoutCfg.usCols_bg = pShadowMgr->bgWall.bgWallCfg.bgWallLayoutCfg.usCols_bg;
#endif
							//
							if (rcd.uiContentType == Consts.CONST_imCommType_transferFileReq)
							{
								string pathCStr="";
								//TCHAR tFileBuf[256] = _T("");
								//
								if (qyFuncs.bSupported_fileServer(pQyMc))
								{
#if false
									QY_MESSENGER_ID idInfo_peer;
									if (pContent->transferFileReq.idInfo_logicalPeer.ui64Id) idInfo_peer.ui64Id = pContent->transferFileReq.idInfo_logicalPeer.ui64Id;
									else idInfo_peer.ui64Id = rcd.idInfo_recv.ui64Id;
									//
									QY_MESSENGER_ID idInfo_sender;
									idInfo_sender.ui64Id = rcd.idInfo_send.ui64Id;
									//
									//  if  (  getFilePath_fileServer(  &rcd.idInfo_send,  &idInfo_peer,  rcd.subject,  tFileBuf, mycountof(  tFileBuf  )  )  )  goto  errLabel;
									TCHAR tPath[MAX_PATH] = _T("");
									if (getPath_fileServer(&rcd.idInfo_send, &idInfo_peer, tPath, mycountof(tPath))) goto errLabel;
									if (tTrailDir(tPath, mycountof(tPath))) goto errLabel;
									//
									TCHAR tFile_part[256] = _T("");
									TCHAR tFile_postfix[24] = _T("");
									safeTcsnCpy(rcd.subject, tFile_part, mycountof(tFile_part));
									TCHAR* pT = _tcsrchr(tFile_part, _T('.'));
									if (pT)
									{
										*pT = 0; pT++;
										safeTcsnCpy(pT, tFile_postfix, mycountof(tFile_postfix));
										tTrim(tFile_postfix);
									}
									//
									time_t t; time(&t);
									char filePrefix[CONST_qyTimeLen + 1] = "";
									if (getFilePrefix_fileServer(t, filePrefix, mycountof(filePrefix))) goto errLabel;
									//
									int iii;
									int max_iii = 255;
									for (iii = 0; iii < max_iii; iii++)
									{
										_sntprintf(tFileBuf, mycountof(tFileBuf), _T("%s%S_qm%I64u_%s"), tPath, filePrefix, idInfo_sender.ui64Id, tFile_part);

										if (iii)
										{
											_sntprintf(tFileBuf, mycountof(tFileBuf), _T("%s_%d"), tFileBuf, iii);
										}
										if (tFile_postfix[0]) _sntprintf(tFileBuf, mycountof(tFileBuf), _T("%s.%s"), tFileBuf, tFile_postfix);
										//
										DWORD dwAttrs;
										dwAttrs = GetFileAttributes(tFileBuf);
										if (dwAttrs != INVALID_FILE_ATTRIBUTES) continue;
										//
										TCHAR tmpBuf[MAX_PATH] = _T("");
										_sntprintf(tmpBuf, mycountof(tmpBuf), _T("%s.tmp"), tFileBuf);
										dwAttrs = GetFileAttributes(tFileBuf);
										if (dwAttrs != INVALID_FILE_ATTRIBUTES) continue;
										//
										break;
									}
									if (iii == max_iii)
									{
										showInfo_open(0, 0, _T("dlgTalk_procTask_transferFileReq failed, no valid iii can be used"));
										goto errLabel;
									}
									//
									pathCStr = CQyString(tFileBuf);
#endif
								}
								else
								{
									//pathCStr = CQyString(pProcInfo->cfg.policy.dirToSaveFile) + CQyString(rcd.subject);
									fixed (char* pDir = pProcInfo.cfg.policy.dirToSaveFile)
									{
										fixed (char* psubject = rcd.subject)
										{
											//
											str = new string(pDir);
											//
											pathCStr = "" + new string(pDir) + new string(psubject);
										}
									}
								}
								if (usOp == Consts.CONST_imOp_recvFile_saveas)
								{
									//
#if false
									if (qyBrowseForFile(hDlgTalk, FALSE, _T("*"), 0, _T("File format(*.*)\0*.*\0\0"), NULL, rcd.subject, pQyMc, pathCStr))
									{
										goto errLabel;
										//break;
									}

									usOp = CONST_imOp_recv_accept;      //  saveAs, CONST_imOp_recvFile_accept
#endif
									goto errLabel;
								}
								qyFuncs.safeTcsnCpy(pathCStr, pContent->transferFileReq.fullFilePath, Consts.MAX_PATH+1);
								fixed (char* pcontent = rcd.content)
								{
									qyFuncs.mylstrcpyn(pcontent, pathCStr, Consts.cntof_content);
								}
							}
							else if (rcd.uiContentType == Consts.CONST_imCommType_transferAvInfo)
							{
								//
							}
							else if (rcd.uiContentType == Consts.CONST_imCommType_queryCustomerServiceOfficerReq)
							{
								;
							}
							else
							{
# if  __DEBUG__
								traceLog((TCHAR*)_T("dlgTalk_procTask failed: unprocessed contentType, %d"), rcd.uiContentType);
#endif
									goto errLabel;
							}

							//
							//if  (  !(  pMsgTask  =  findTaskInProcessQ(  &m_var,  iTaskId,  0  )  )  )  goto  errLabel;
							if (null==pMsgTask) goto errLabel;

							// 
							iStatus = Consts.CONST_imTaskStatus_waitToRecv;
							iStatus_old = pMsgTask->iStatus;
							//					
							if (0!=g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, rcd.id)) goto errLabel;
							pMsgTask->iStatus = iStatus;

							//
							/*
							for  (  i  =  0;  i  <  mycountof(  pShadowMgr->shadows  );  i  ++  )  {
								 if  (  pShadowMgr->shadows[i].hShadow  &&  isTalkerSubtype_canInput(  pShadowMgr->shadows[i].iTalkerSubType  )  )  {
									 CHelp_getDlgTalkVar	help_getDlgTalkVar;
									 dlgTalk_procTask_display_op(  pShadowMgr->shadows[i].hShadow,  (  DLG_TALK_var	*	)help_getDlgTalkVar.getVar(  pShadowMgr->shadows[i].hShadow  ),  iTaskId,  usOp,  pMsgTask,  rcd,  iStatus_old,  iStatus,  paramStrBuf  );
								 }
							}
							//
							dlgTalk_procTask_display_op(  hDlgTalk,  &m_var,  iTaskId,  usOp,  pMsgTask,  rcd,  iStatus_old,  iStatus,  paramStrBuf  );
							*/
							dlgTalk_procTask_display_op_mgr(hDlgTalk, iTaskId, usOp, pMsgTask, ref rcd, iStatus_old, iStatus, paramStrBuf);


							//
							if (rcd.uiContentType == Consts.CONST_imCommType_transferFileReq)
							{
								/*
								CQyMalloc						mallocObj_transferFileReplyReq;
								TRANSFER_FILE_REPLYREQ	*	pTransferFileReplyReq	=	(  TRANSFER_FILE_REPLYREQ  *  )mallocObj_transferFileReplyReq.mallocf(  sizeof(  TRANSFER_FILE_REPLYREQ  )  );
								if  (  !pTransferFileReplyReq  )  goto  errLabel;
								TRANSFER_FILE_REPLYREQ	&	transferFileReplyReq	=	*pTransferFileReplyReq;
								*/
								TASK_transferFile task=new TASK_transferFile();
								//memset(&task, 0, sizeof(task));
								//
								task.m_index_taskInfo = iIndex_taskInfo;
								pTaskInfo.var.m_bReplyTask = true;
								//
								TRANSFER_FILE_REPLYREQ transferFileReplyReq;
								ref PROC_TASK_FILE  transferFileReplyReq_myTask = ref task.myTask;

								//
								qyFuncs.mymemset((System.IntPtr)(byte*)&transferFileReplyReq, 0, sizeof(TRANSFER_FILE_REPLYREQ));
								transferFileReplyReq.uiType = Consts.CONST_imCommType_transferFileReplyReq;
								transferFileReplyReq.usOp = usOp;
								transferFileReplyReq.tStartTime_org = rcd.tSendTime;
								transferFileReplyReq.uiTranNo_org = rcd.uiTranNo;
								transferFileReplyReq.uiSerialNo_org = rcd.uiSerialNo;
								//
								task.myTask.ui64FileLen = pContent->transferFileReq.ui64FileLen;    //  _tcstol(  rcd.propStrs[0],  &pT,  10  );
																									//_sntprintf(task.myTask.fileName_transferred, mycountof(task.myTask.fileName_transferred), _T("%s"), rcd.subject);
								fixed (char* psubject = rcd.subject)
								{
									task.myTask.fileName_transferred = new string(psubject);
								}
								fixed (char* pcontent = rcd.content)
								{
									//_sntprintf(task.myTask.fullFilePath_save, mycountof(task.myTask.fullFilePath_save), _T("%s"), rcd.content);
									task.myTask.fullFilePath_save = new string(pcontent);
								}
								//  2014/07/13
								uint uiSizePerSnd_internal = 0;
								if (pProcInfo.processQ_media.isEmpty()) uiSizePerSnd_internal = pProcInfo.cfg.uiSizePerSnd_file_noAv;
								else uiSizePerSnd_internal = pProcInfo.cfg.uiSizePerSnd_file_avExists;
								task.myTask.uiSizePerSnd_internal = (uint)Math.Min(uiSizePerSnd_internal, pContent->transferFileReq.uiSizePerSnd_suggest); //  PDEFAULT_usSizePerSnd_file;
																																				//
								transferFileReplyReq_myTask.hWnd_task = hDlgTalk;       //  2009/10/20
																						//
								lenInBytes = sizeof(TRANSFER_FILE_REPLYREQ);
                                //		
                                System.Int64 tStartTran; uint uiTranNo;			
								qyFuncs.MACRO_prepareForTran(out tStartTran,out uiTranNo);

								//
								task.myTask.reply_info.tStartTime_reply = tStartTran;
								task.myTask.reply_info.uiTranNo_reply = uiTranNo;
								task.myTask.reply_info.replyReq = transferFileReplyReq;

								//						
								if (0 != pProcInfo.processQ_media.qPostMsg(task, 0))
								{
									goto errLabel;
								}

								//
								uint uiChannelType = Consts.CONST_channelType_robot;
								//
								uiChannelType = 0;
								//
								QY_MESSENGER_ID idInfo_logicalPeer=m_var.addr.idInfo;
								QY_MESSENGER_ID idInfo_dst = rcd.idInfo_send;
								//
								if (0!=qyFuncs.postMsgTask2Mgr_mc(null,pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendRobotTaskReply, tStartTran, uiTranNo, 0, iTaskId, rcd.uiMode, (byte*)&transferFileReplyReq, (uint)lenInBytes, 
									idInfo_logicalPeer.ui64Id, pMsgTask->idInfo_taskSender.ui64Id, pMsgTask->idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;

							}
							else if (rcd.uiContentType == Consts.CONST_imCommType_transferAvInfo)
							{

								//
							}
							else if (rcd.uiContentType == Consts.CONST_imCommType_queryCustomerServiceOfficerReq)
							{       //  2011/04/08
									//
#if false
								QM_OBJQ_MEM mem;
								QMEM_MESSENGER_CLI* pQMemObj;

								if (findQMemByKey(pProcInfo->pSgiParam, (QM_OBJQ*)m_var.pMisCnt->pObjQ, &m_var.addr.idInfo, &mem))
								{
									traceLog((TCHAR*)_T("dlgTalk_procTask failed, findQMemByKey failed."));
									goto errLabel;
								}
								if (!mem.pQMemObj) goto errLabel;

								pQMemObj = (QMEM_MESSENGER_CLI*)mem.pQMemObj;
								pQMemObj->customerService.rules.uiCmd_autoAnswer_av = CONST_ruleCmd_permit;             //  这里要把视频客服的策略设置成允许，否则就不能自动接收了
								pQMemObj->customerService.rules.uiCmd_autoAnswer_screen = CONST_ruleCmd_permit;

								//
								QUERY_customerServiceOfficer_replyReq replyReq;

								memset(&replyReq, 0, sizeof(replyReq));
								replyReq.uiType = CONST_imCommType_queryCustomerServiceOfficerReplyReq;
								//
								replyReq.tStartTime_org = pMsgTask->tStartTime;
								replyReq.uiTranNo_org = pMsgTask->uiTranNo;
								//
								safeWcsnCpy(m_var.pMisCnt->talkerDesc, replyReq.talkerDesc, mycountof(replyReq.talkerDesc));
								//
								lenInBytes = sizeof(replyReq);
								//					
								MACRO_prepareForTran();
								//
								if (postMsgTask2Mgr_mc(m_var.pMisCnt, CONST_misMsgType_task, 0, CONST_qyCmd_sendTaskReply, tStartTran, uiTranNo, 0, iTaskId, rcd.uiMode, (char*)&replyReq, lenInBytes, &m_var.addr.idInfo, &pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, &rcd.idInfo_send, 0, NULL, FALSE)) goto errLabel;
#endif

							}
							else
							{
# if  __DEBUG__
								traceLog((TCHAR*)_T("dlgTalk_procTask failed, unprocessed contentType %d"), rcd.uiContentType);
#endif
								goto errLabel;
							}
							//

							//  2010/12/24
							chkResources(hDlgTalk, false);

						}

						break;
					default:
						break;
				}
			}

			iErr = 0;
		errLabel:
			return iErr;

		}




	}




}