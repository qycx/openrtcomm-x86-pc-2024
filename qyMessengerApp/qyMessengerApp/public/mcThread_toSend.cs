using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int tmpHandler_toSend(object p0, object p1, object pMsgElemParam)
		{
			int iErr = -1;
			COMMON_PARAM pCommonParam0 = (COMMON_PARAM)p0;
			COMMON_PARAM pCommonParam1 = (COMMON_PARAM)p1;

			
			CCtxQmc pProcInfo = (CCtxQmc)pCommonParam0.p0;
			MIS_CNT pMisCnt = (MIS_CNT)pCommonParam0.p1;
			MIS_CHANNEL pChannel = (MIS_CHANNEL)pCommonParam0.p2;
			//
			TRAN_INFO_MIS* pTranInfo = (TRAN_INFO_MIS*)(IntPtr)pCommonParam1.p0;
			MIS_MSG_TASK* pMsgTask_o = &pTranInfo->pMsg_o->task;

			int lenInBytes;

			if (null==pProcInfo || null==pMisCnt || null==pChannel || null==pMsgTask_o)
			{
				showInfo_open(0, null, ("tmpHandler_toSend failed: pMisCnt or pChannel or pMsgTask_o is null"));
				return -1;
			}

			string where_showInfo; where_showInfo = qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType);

			switch (((MIS_MSGU*)(IntPtr)pMsgElemParam)->uiType)
			{
				case Consts.CONST_misMsgType_task:
				case Consts.CONST_misMsgType_outputTask:
					{
						MIS_MSG_TASK* pMsgElem = (MIS_MSG_TASK*)(IntPtr)pMsgElemParam;

						pMsgElem->nTimes_tryToSend++;

						//
						//  				  
						if (pMsgElem->data.route.debugInfo.dwTickCnt_me_start1!=0)
						{   //  2008/11/20
							pMsgElem->dwTickCnt_start1 = pMsgElem->data.route.debugInfo.dwTickCnt_me_start1;
							pMsgElem->dwTickCnt_toSend = myGetTickCount(null);        //  2011/01/22
						}
						if (pMsgElem->data.route.debugInfo.tTime_start!=0)
						{
							if (isUcFlgResp(pMsgElem->ucFlg)) pMsgElem->data.route.debugInfo.usElapsed_peer_resp_toSend = (ushort  )(myGetTickCount(null) - pMsgElem->data.route.debugInfo.dwTickCnt_peer_recved);
					  else
							{
								pMsgElem->data.route.debugInfo.usElapsed_toSend = (ushort  )(myGetTickCount(null) - pMsgElem->data.route.debugInfo.dwTickCnt_me_start1);
								//  
							}
							//
							pMsgElem->index_debugInfo = pMsgElem->data.route.debugInfo.index;
						}

						//  
						pMsgElem->data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;


						mymemcpy((IntPtr)pMsgTask_o, (IntPtr)pMsgElem, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data"));
						lenInBytes = sizeof(MIS_MSG_routeTalkData  );
						if (0!=qyhtonRouteTalkData(&pMsgElem->data.route, (byte*)&pMsgElem->data.buf, 0, true, pMsgElem->ucFlg, (byte*)&pMsgTask_o->data, (uint *  ) & lenInBytes,  null,  0  )  )  {
							//traceLog(_T("tmpHandler_toSend: qyhtonRouteTalkData failed"));
							showInfo_open(0, null, ("tmpHandler_toSend failed: htonRouteTalkData failed"));
							goto errLabel;
						}
						pMsgTask_o->lenInBytes = (uint)lenInBytes;
						lenInBytes += (int)Marshal.OffsetOf<MIS_MSG_TASK>("data");

						if (0!=postMsg2OutputQ_mc(pMisCnt, (MIS_MSGU*)pMsgTask_o, (uint)lenInBytes, pChannel))
						{
							//  
							showInfo_open(0, null, ("tmpHandler_toSend failed: postMsg2OutputQ_mc failed"));
							goto errLabel;
						}


						pMsgElem->tStartTime_processed = pTranInfo->tNow;
						pMsgElem->uiTranNo_processed = pTranInfo->uiTranNo;

						pTranInfo->nNodes_processed++;


						iErr = 0; goto errLabel;

					}
					break;
				default:
					//qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo.str_who_showInfo, 0, (""), (""), ("tmpHandler_toSend failed: toSendQÀïÓÐÎ´´¦ÀíµÄÏûÏ¢ %s"), qyGetDesByType1(CONST_misMsgTypeTable, ((MIS_MSGU*)pMsgElemParam)->uiType));
					goto errLabel;
					break;
			}


			uint uiMaxSendMsgs = pTranInfo->uiMaxSendMsgs!=0 ? pTranInfo->uiMaxSendMsgs : pProcInfo.cfg.uiMaxSendMsgs;   //  2011/01/23
			if (pTranInfo->nNodes_processed >= uiMaxSendMsgs)
			{
				iErr = 1; goto errLabel;    //  
			}

			

			iErr = 0;
		errLabel:
			return iErr;

		}

		public static unsafe int tmpHandler_needRemoveMsg_cli(object p0, object p1, object p2)
		{
			int iErr = -1;
			//  p0	 	 
			COMMON_PARAM pCommonParam1 = (COMMON_PARAM)p1;
			MIS_MSGU* pMsgElem = (MIS_MSGU*)(IntPtr)p2;
			//
			TRAN_INFO_MIS* pTranInfo = (TRAN_INFO_MIS*)(IntPtr)pCommonParam1.p0;

			switch (pMsgElem->uiType)
			{
				case Consts.CONST_misMsgType_task:
				case Consts.CONST_misMsgType_outputTask:
					break;
				default:
					traceLogA("tmpHandler_needRemoveMsg_cli: msgType error.");
					goto errLabel;
					break;
			}

			if (pMsgElem->task.nTimes_tryToSend!=0)
			{           //  Õâ¸öÌõ¼þÊÇÎªÁËÈ·±£Ö»ÄÜÉ¾³ýÒÑ·¢¹ýµÄ¡£
				iErr = 1; goto errLabel;
			}

			iErr = 0;
		errLabel:
			return iErr;
		}

		public static unsafe bool b_tmpHandler_needRemoveMsg_cli(object p0, object p1, object p2)
        {
			int iRet = tmpHandler_needRemoveMsg_cli(p0, p1, p2);
			return iRet != 0 ? true : false;
        }


		public static unsafe int tmpInitMsgMixer(CCtxQmc pProcInfo, MIS_CNT pMisCnt, MIS_MSG_TASK* pMsgTask_av, AV_TRAN_INFO* pAvTranInfo, ref PROC_TASK_AV pTask, int iContentType, MIS_MSG_TASK* pMsgTask_toBeMixed, MIS_MSG_TASK* pMsgMixer)
		{
			int iErr = -1;
			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo,"tmpInitMsgMixer");
			if (null==pTaskInfo) return -1;

			//
			if (0!=pMsgMixer->uiType) return -1;

			pMsgMixer->uiType = Consts.CONST_misMsgType_task;
			//pMsgMixer->pMisCnt = pMisCnt;

			pMsgMixer->ucFlg = Consts.CONST_commFlg_talkData;  //  CONST_commFlg_talkData;
			pMsgMixer->ucFlg |= Consts.CONST_commFlg_routeTalkData;        //  2011/01/26

			pMsgMixer->usCode = Consts.CONST_qyCmd_sendMedia;

			Int64 tStartTran;uint uiTranNo;
			MACRO_prepareForTran(out tStartTran,out uiTranNo);

			pMsgMixer->tStartTime = tStartTran;
			pMsgMixer->uiTranNo = uiTranNo;

			//  
			pMsgMixer->uiChannelType = pMsgTask_av->uiChannelType;

			//
			switch (iContentType)
			{
				case Consts.CONST_imCommType_transferAudioData:
					{
						TRANSFER_AUDIO_DATA* pMixer = (TRANSFER_AUDIO_DATA*)&pMsgMixer->data.buf;
						//
						IM_CONTENTU* pContent_toBeMixed = M_getMsgContent(pMsgTask_toBeMixed->uiType,  pMsgTask_toBeMixed->ucFlg, &pMsgTask_toBeMixed->data);
						if (pContent_toBeMixed->uiType != iContentType)
						{
							showInfo_open(0, null, _T("tmpInitMsgMixer failed, content_toBeMixed.type != contentType"));
							goto errLabel;
						}

						//
						mymemset((IntPtr)pMixer, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));
						pMixer->uiType = Consts.CONST_imCommType_transferAudioData;

						//
						//pMixer->uiTranNo_openAvDev = pAvTranInfo->audio.uiTranNo_openAvDev;
						pMixer->uiTranNo_openAvDev = pContent_toBeMixed->transferAudioData.uiTranNo_openAvDev;

						//
						if (0==pContent_toBeMixed->transferAudioData.uiTranNo_openAvDev)
						{
							showInfo_open(0, null, _T("tmpInitMsgMixer err: pContent_toBeMixed->transferAudioData.uiTranNo_openAvDev is 0"));
						}


						//
						bool ucbNeedShowFrameInfo = false;

# if  __DEBUG__		
						//  for test
						ucbNeedShowFrameInfo = TRUE;
#endif

					}
					break;
				case Consts.CONST_imCommType_transferVideoData:
					{
						TRANSFER_VIDEO_DATA* pMixer = (TRANSFER_VIDEO_DATA*)&pMsgMixer->data.buf;
						//  2016/03/28
						IM_CONTENTU* pContent_toBeMixed = M_getMsgContent(pMsgTask_toBeMixed->uiType,pMsgTask_toBeMixed->ucFlg, &pMsgTask_toBeMixed->data);
						if (pContent_toBeMixed->uiType != iContentType)
						{
							showInfo_open(0, null, _T("tmpInitMsgMixer failed, content_toBeMixed.type != contentType"));
							goto errLabel;
						}
						IM_CONTENTU* pContent_av = M_getMsgContent(pMsgTask_av->uiType,pMsgTask_av->ucFlg, &pMsgTask_av->data);

						//
						mymemset((IntPtr)pMixer, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));
						pMixer->uiType = (uint)iContentType;

						//
						//pMixer->uiTranNo_openAvDev = pAvTranInfo->video.uiTranNo_openAvDev;
						pMixer->uiTranNo_openAvDev = pContent_toBeMixed->transferVideoData.uiTranNo_openAvDev;

						//
						if (0==pContent_toBeMixed->transferVideoData.uiTranNo_openAvDev)
						{
							showInfo_open(0, null, _T("tmpInitMsgMixer err: pContent_toBeMixed->transferVideoData.uiTranNo_openAvDev is 0"));
						}


						//  2016/03/28
#if false
						if (0!=pContent_toBeMixed->transferVideoData.uiTranNo_openAvDev)
						{
							if (pMsgTask_av->uiTaskType == Consts.CONST_imTaskType_shareDynBmp)
							{
								if (pContent_av->uiType == Consts.CONST_imCommType_transferAvInfo)
								{
									//
									if (pContent_av->transferAvInfo.viewDynBmp.resObj.uiObjType == Consts.CONST_objType_avStream)
									{
										//			
										if (pContent_toBeMixed->transferVideoData.uiTranNo_openAvDev != pMixer->uiTranNo_openAvDev)
										{
											pMixer->uiTranNo_openAvDev = pContent_toBeMixed->transferVideoData.uiTranNo_openAvDev;
										}
										//
									}
								}
							}
						}
#endif

						//
						bool ucbNeedShowFrameInfo = false;

#if __DEBUG__
						//  for test
						//  ucbNeedShowFrameInfo  =  TRUE;
#endif


					}
					break;
				default:
					break;
			}

			//
			if (pTask.bVideoConference())
			{
				if (!pTask.ucbVideoConferenceStarter)
				{
					pMsgMixer->data.route.idInfo_to.ui64Id = pMsgTask_av->data.route.idInfo_from.ui64Id;
				}
				else
				{
					pMsgMixer->data.route.idInfo_to.ui64Id = pMsgTask_av->addr_logicalPeer.idInfo.ui64Id;
				}
			}
			else if (pTask.doProxy.bExists_avProxy)
			{   //  2014/11/21
				pMsgMixer->data.route.idInfo_to.ui64Id = pMsgTask_av->addr_logicalPeer.idInfo.ui64Id;
			}
			else
			{
				//memcpy(&pMsgMixer->data.route, &pMsgTask_toBeMixed->data.route, sizeof(pMsgMixer->data.route)  );
				pMsgMixer->data.route = pTaskInfo.var.curRoute_sendLocalAv.route;
			}

			//
			if (pMsgTask_toBeMixed->data.route.debugInfo.dwTickCnt_me_start1!=0)
			{   //  2011/01/24
				pMsgMixer->dwTickCnt_start1 = pMsgTask_toBeMixed->data.route.debugInfo.dwTickCnt_me_start1;
				pMsgMixer->dwTickCnt_toSend = myGetTickCount(null);       //  2011/01/22
			}


			iErr = 0;
		errLabel:
			return iErr;
		}



		public static unsafe int tmpHandler_toMixer_audio(object p0, object p1, object pMsgElemParam)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			COMMON_PARAM pCommonParam0 = (COMMON_PARAM)p0;
			COMMON_PARAM pCommonParam1 = (COMMON_PARAM)p1;

			COMMON_PARAM pCommonParam0_toSend = (COMMON_PARAM)pCommonParam0.p0;
			//MIS_MSG_TASK* pMsgTask_av = (MIS_MSG_TASK*)pCommonParam0.p1;
			TASK_common taskElem = (TASK_common)pCommonParam0.p1;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo, taskElem.m_index_taskInfo,"tmpHandler_toMixer_audio");
			if (pTaskInfo == null) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			//
			fixed (MIS_MSG_TASK* pMsgTask_av = &pTaskData.msgU.task)
			{
				if (pMsgTask_av->uiType != Consts.CONST_misMsgType_task)
				{
					return -1;
				}

				//CCtxQmc pProcInfo = (CCtxQmc)pCommonParam0_toSend.p0;
				//MIS_CNT pMisCnt = (MIS_CNT)pCommonParam0_toSend.p1;
				MIS_CHANNEL pChannel = (MIS_CHANNEL)pCommonParam0_toSend.p2;

				//CCtxQyMc pQyMc = g.g_pQyMc;// (QY_MC*)pProcInfo->pQyMc;

				IM_CONTENTU* pContent_av = (IM_CONTENTU*)&pMsgTask_av->data.buf;
				AV_TRAN_INFO* pAvTran = null;
				AV_TRAN_INFO tmp_avTranInfo;
				PROC_TASK_AV pTask = null;

				if (pContent_av->uiType == Consts.CONST_imCommType_transferAvInfo)
				{
					//
					if (!pTaskInfo.var.m_bReplyTask)
					{
						if (pContent_av->transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
						{
#if __DEBUG__
					MACRO_qyAssert(0, _T("transferAv.subtype err, not task"));
#endif
							return -1;
						}
						TASK_transferAv task = (TASK_transferAv)taskElem;
						//
						//TRANSFER_AV_task* pTransferAvTask = (TRANSFER_AV_task*)pContent_av;
						//
						pAvTran = &pContent_av->transferAvInfo.ass.tranInfo;
						pTask = task.myTask;// &pTransferAvTask->myTask;
					}
                    else { 

					//else if (pContent_av->uiType == Consts.CONST_imCommType_transferAvReplyInfo)
					
						TASK_transferAv task = (TASK_transferAv)taskElem;
						pTask = task.myTask;// get_transferAvReplyInfo_pTask(&pContent_av->transferAvReplyInfo, _T(""));  //  &pContent_av->transferAvReplyInfo.myTask;							
						if (null == pTask) return -1;
						tmp_avTranInfo = pTask.reply_avTranInfo;
						pAvTran = &tmp_avTranInfo;// &pContent_av->transferAvReplyInfo.tranInfo;

					}
				}
				if (null==pAvTran) return -1;


				TRAN_INFO_MIS* pTranInfo = (TRAN_INFO_MIS*)(IntPtr)pCommonParam1.p0;
				MIS_MSG_TASK* pMsgTask_o = &pTranInfo->pMsg_o->task;

				MIS_MSG_TASK* pMsgMixer = (MIS_MSG_TASK*)(IntPtr)pCommonParam1.p1;
				TRANSFER_AUDIO_DATA* pMixer = (TRANSFER_AUDIO_DATA*)&pMsgMixer->data.buf;


				switch (((MIS_MSGU*)(IntPtr)pMsgElemParam)->uiType)
				{
					case Consts.CONST_misMsgType_task:
						{
							MIS_MSG_TASK* pMsgElem = (MIS_MSG_TASK*)(IntPtr)pMsgElemParam;
							//MIS_CNT* pMisCntElem = (MIS_CNT*)pMsgElem->pMisCnt;
							IM_CONTENTU* pContentElem = (IM_CONTENTU*)&pMsgElem->data.buf;

							pMsgElem->nTimes_tryToSend++;

							switch (pContentElem->uiType)
							{
								case Consts.CONST_imCommType_transferAudioData:
									{

										if (0==pMsgMixer->uiType)
										{
											tmpInitMsgMixer(pProcInfo, pMisCnt, pMsgTask_av, pAvTran, ref pTask, Consts.CONST_imCommType_transferAudioData, pMsgElem, pMsgMixer);
										}
										if (pMixer->uiTranNo_openAvDev != pContentElem->transferAudioData.uiTranNo_openAvDev)
										{

											if (0!=pMixer->usCnt)
											{
												if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
												{
													//qyShowInfo1(  CONST_qyShowType_detail,  0,  "",  pProcInfo->who_showInfo,  NULL,  _T(  ""  ),  _T(  ""  ),  _T(  "tmpHadler_toMixer: flush mixer data. mixer.usCnt %d, mixer.uiLen %d,  mem.len %d."  ),  pMixer->usCnt,  pMixer->uiLen,  pContentElem->transferAudioData.mems[i].uiLen  );
												}
												tmpHandler_toSend(pCommonParam0_toSend, p1, (IntPtr)pMsgMixer);
											}
											//
											MACRO_memset_misMsgTask(ref *pMsgMixer);
											if (0==pMsgMixer->uiType)
											{
												tmpInitMsgMixer(pProcInfo, pMisCnt, pMsgTask_av, pAvTran, ref pTask, Consts.CONST_imCommType_transferAudioData, pMsgElem, pMsgMixer);
											}

										}


										//  È»ºó°ÑÊý¾Ý¼Óµ½»º³åÀï
										if (pMixer->usCnt > Consts.CONST_maxTransferAvDataMems)
										{                       //  Êµ¼ÊÕâÀïÊÇ²»¶ÔµÄ£¬°´Àí²»ÄÜÓÐÕâÑùµÄÇé¿ö³öÏÖ, ÒòÎª
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer failed:  pMixer->usCnt %d >= mems %d."), pMixer->usCnt, mycountof(pMixer->mems));
											goto errLabel;
										}
										if (0==pContentElem->transferAudioData.usCnt)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer:  transferAudioData.usCnt is 0."));
											break;
										}
										if (pContentElem->transferAudioData.usCnt > Consts.CONST_maxTransferAvDataMems)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer failed:  transferAudioData.usCnt %d >= mems %d."), pContentElem->transferAudioData.usCnt, mycountof(pContentElem->transferAudioData.mems));
											goto errLabel;
										}
										if (pContentElem->transferAudioData.uiLen > Consts.bufSize_transferAudioData)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer failed:  transferAudioData.uiLen %d too big."), pContentElem->transferAudioData.uiLen);
											goto errLabel;
										}

										TRANSFER_AUDIO_dataMemHead* pContentElem_transferAudioData_mems = (TRANSFER_AUDIO_dataMemHead*)pContentElem->transferAudioData.buf_TRANSFER_AUDIO_dataMemHead_mems;
										int i;
										TRANSFER_AUDIO_dataMemHead* pMem;
										int iPos;

										for (iPos = 0, i = 0; i < pContentElem->transferAudioData.usCnt; iPos += (int)pContentElem_transferAudioData_mems[i].uiLen, i++)
										{
											//
											if (iPos + pContentElem_transferAudioData_mems[i].uiLen > pContentElem->transferAudioData.uiLen)
											{
												traceLogA("tmpHandler_toMixer failed: iPos + mems[i].uiLen > content.uiLen");
												goto errLabel;
											}

											//
											if (pMixer->usCnt == Consts.CONST_maxTransferAvDataMems || pContentElem_transferAudioData_mems[i].uiLen + pMixer->uiLen > Consts.bufSize_transferAudioData)
											{

												if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
												{
													//qyShowInfo1(CONST_qyShowType_detail, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer: flush mixer data. mixer.usCnt %d, mixer.uiLen %d,  mem.len %d."), pMixer->usCnt, pMixer->uiLen, pContentElem->transferAudioData.mems[i].uiLen);
												}

												tmpHandler_toSend(pCommonParam0_toSend, p1, (IntPtr)pMsgMixer);
												MACRO_memset_misMsgTask(ref *pMsgMixer);
												if (0==pMsgMixer->uiType)
												{
													tmpInitMsgMixer(pProcInfo, pMisCnt, pMsgTask_av, pAvTran, ref pTask, Consts.CONST_imCommType_transferAudioData, pMsgElem, pMsgMixer);
												}
											}

											TRANSFER_AUDIO_dataMemHead* pMixer_mems = (TRANSFER_AUDIO_dataMemHead*)pMixer->buf_TRANSFER_AUDIO_dataMemHead_mems;
											//
											pMem = &pMixer_mems[pMixer->usCnt];
											//  memset(  pMem,  0,  sizeof(  pMem[0]  )  );
											//  pMem->idInfo.ui64Id  =  pContentElem->transferAudioData.mems[i].idInfo.ui64Id;									 	
											mymemcpy((IntPtr)pMem, (IntPtr)(byte*)&pContentElem_transferAudioData_mems[i], (uint)sizeof(TRANSFER_AUDIO_dataMemHead));
											if (0!=pMsgElem->data.route.debugInfo.dwTickCnt_peer_recved)
											{   //  Õâ¸öÌõ¼þÅÅ³ýÁË±¾µØ·¢¹ýÀ´µÄÒôÆµ								
												pMem->tmpInternal.dwTickCnt_videoConferenceRecved = pMsgElem->data.route.debugInfo.dwTickCnt_peer_recved;
											}
											else
											{
												pMem->tmpInternal.dwTickCnt_videoConferenceRecved = myGetTickCount(null);
											}
											pMem->tmpInternal.uiTranNo_org = pMsgElem->uiTranNo;

											//  pMem->uiLen  =  min(  sizeof(  pMem->buf  ),  pContentElem->transferAudioData.mems[0].uiLen  );			//  ÕâÀïÔÝÊ±Ö»¿¼ÂÇÖ±½Ó·¢ËÍµÄÇé¿ö£¬ËùÒÔÖ»ÓÐ1¸öÊý¾Ý°ü
											//  memcpy(  pMem->buf,  pContentElem->transferAudioData.mems[0].buf,  pMem->uiLen  );
											//
											mymemcpy((IntPtr)(pMixer->buf + pMixer->uiLen), (IntPtr)(pContentElem->transferAudioData.buf + iPos), pMem->uiLen);
											pMixer->uiLen += pMem->uiLen;

											pMixer->usCnt++;

										}

										if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
										{
											//qyShowInfo1(CONST_qyShowType_detail, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer ok: mixer.usCnt %d, mixer.uiLen %d"), pMixer->usCnt, pMixer->uiLen);
										}

									}
									break;
								default:
									break;
							}


							pMsgElem->tStartTime_processed = pTranInfo->tNow;
							pMsgElem->uiTranNo_processed = pTranInfo->uiTranNo;

							pTranInfo->nNodes_processed++;

						}
						break;
					default:
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("ÔÚtmpHandler_toMixerÀï³öÏÖÁËÎ´´¦ÀíµÄÏûÏ¢ÀàÐÍ, %s"), qyGetDesByType1(CONST_misMsgTypeTable, ((MIS_MSGU*)pMsgElemParam)->uiType));
						goto errLabel;
						break;

				}

				if (pTranInfo->nNodes_processed >= pProcInfo.cfg.uiMaxSendMsgs)
				{
					iErr = 1; goto errLabel;
				}
			}

	iErr  =  0;
errLabel:
	return  iErr;
}



//
public static unsafe int tmpHandler_toMixer_video(object p0, object p1, object pMsgElemParam)
{
	int iErr = -1;

	CCtxQyMc pQyMc = g.g_pQyMc;// (QY_MC*)pProcInfo->pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			COMMON_PARAM pCommonParam0 = (COMMON_PARAM)p0;
	COMMON_PARAM pCommonParam1 = (COMMON_PARAM)p1;

	COMMON_PARAM pCommonParam0_toSend = (COMMON_PARAM)pCommonParam0.p0;
			//MIS_MSG_TASK* pMsgTask_av = (MIS_MSG_TASK*)pCommonParam0.p1;
			TASK_common taskElem = (TASK_common)pCommonParam0.p1;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo, taskElem.m_index_taskInfo,"tmpHandler_toMixer_video");
			if (pTaskInfo == null) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;

			//
			fixed (MIS_MSG_TASK* pMsgTask_av = &pTaskData.msgU.task)
			{
				if(pMsgTask_av->uiType!=Consts.CONST_misMsgType_task)
                {
					return -1;
                }

				//CCtxQmc pProcInfo = (CCtxQmc)pCommonParam0_toSend.p0;
				//MIS_CNT pMisCnt = (MIS_CNT)pCommonParam0_toSend.p1;
				MIS_CHANNEL pChannel = (MIS_CHANNEL)pCommonParam0_toSend.p2;

				
				IM_CONTENTU* pContent_av = (IM_CONTENTU*)&pMsgTask_av->data.buf;
				AV_TRAN_INFO* pAvTran = null;
				AV_TRAN_INFO tmp_avTranInfo;
				PROC_TASK_AV pTask = null;

				if (pContent_av->uiType == Consts.CONST_imCommType_transferAvInfo)
				{
					if (!pTaskInfo.var.m_bReplyTask)
					{
						TASK_transferAv task = (TASK_transferAv)taskElem;
						//
						pAvTran = &pContent_av->transferAvInfo.ass.tranInfo;
						pTask = task.myTask;// get_transferAvInfo_pTask(&pContent_av->transferAvInfo, _T(""));  //  &pTransferAvTask->myTask;
						if (null == pTask) return -1;
					}
					else
					//else if (pContent_av->uiType == Consts.CONST_imCommType_transferAvReplyInfo)
					{
						TASK_transferAv task = (TASK_transferAv)taskElem;

						pTask = task.myTask;// get_transferAvReplyInfo_pTask(&pContent_av->transferAvReplyInfo, _T(""));   //  &pContent_av->transferAvReplyInfo.myTask;							
						if (null == pTask) return -1;
						//pAvTran = &pContent_av->transferAvReplyInfo.tranInfo;
						tmp_avTranInfo = pTask.reply_avTranInfo;
						pAvTran = &tmp_avTranInfo;
					}
				}
				if (null==pAvTran) return -1;

				TRAN_INFO_MIS* pTranInfo = (TRAN_INFO_MIS*)(IntPtr)pCommonParam1.p0;
				MIS_MSG_TASK* pMsgTask_o = &pTranInfo->pMsg_o->task;

				MIS_MSG_TASK* pMsgMixer = (MIS_MSG_TASK*)(IntPtr)pCommonParam1.p1;
				TRANSFER_VIDEO_DATA* pMixer = (TRANSFER_VIDEO_DATA*)&pMsgMixer->data.buf;


				switch (((MIS_MSGU*)(IntPtr)pMsgElemParam)->uiType)
				{
					case Consts.CONST_misMsgType_task:
						{
							MIS_MSG_TASK* pMsgElem = (MIS_MSG_TASK*)(IntPtr)pMsgElemParam;
							//MIS_CNT* pMisCntElem = (MIS_CNT*)pMsgElem->pMisCnt;
							IM_CONTENTU* pContentElem = (IM_CONTENTU*)&pMsgElem->data.buf;

							pMsgElem->nTimes_tryToSend++;

							switch (pContentElem->uiType)
							{
								case Consts.CONST_imCommType_transferVideoData:
									{

										if (0==pMsgMixer->uiType)
										{
											tmpInitMsgMixer(pProcInfo, pMisCnt, pMsgTask_av, pAvTran, ref pTask, Consts.CONST_imCommType_transferVideoData, pMsgElem, pMsgMixer);
											//
										}
										if (pMixer->uiTranNo_openAvDev != pContentElem->transferVideoData.uiTranNo_openAvDev)
										{
											if (0!=pMixer->usCnt)
											{
												if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewVideoStatus)
												{
													//qyShowInfo1(  CONST_qyShowType_detail,  0,  "",  pProcInfo->who_showInfo,  NULL,  _T(  ""  ),  _T(  ""  ),  _T(  "tmpHadler_toMixer_video: flush mixer data. mixer.usCnt %d, mixer.uiLen %d,  mem.len %d."  ),  pMixer->usCnt,  pMixer->uiLen,  pContentElem->transferVideoData.mems[i].uiLen  );
												}

												tmpHandler_toSend(pCommonParam0_toSend, p1, (IntPtr)pMsgMixer);
											}

											MACRO_memset_misMsgTask(ref *pMsgMixer);
											if (0==pMsgMixer->uiType)
											{
												tmpInitMsgMixer(pProcInfo, pMisCnt, pMsgTask_av, pAvTran, ref pTask, Consts.CONST_imCommType_transferVideoData, pMsgElem, pMsgMixer);
											}
										}


										//  È»ºó°ÑÊý¾Ý¼Óµ½»º³åÀï
										if (pMixer->usCnt > Consts.CONST_maxTransferAvDataMems)
										{                       //  Êµ¼ÊÕâÀïÊÇ²»¶ÔµÄ£¬°´Àí²»ÄÜÓÐÕâÑùµÄÇé¿ö³öÏÖ, ÒòÎª
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer_video failed:  pMixer->usCnt %d >= mems %d."), pMixer->usCnt, mycountof(pMixer->mems));
											goto errLabel;
										}
										if (0==pContentElem->transferVideoData.usCnt)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer_video:  transferVideoData.usCnt is 0."));
											break;
										}
										if (pContentElem->transferVideoData.usCnt > Consts.CONST_maxTransferAvDataMems)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer_video failed:  transferVideoData.usCnt %d >= mems %d."), pContentElem->transferVideoData.usCnt, mycountof(pContentElem->transferVideoData.mems));
											goto errLabel;
										}
										if (pContentElem->transferVideoData.uiLen > Consts.bufSize_transferAudioData)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer_video failed:  transferVideoData.uiLen %d too big."), pContentElem->transferVideoData.uiLen);
											goto errLabel;
										}

										TRANSFER_VIDEO_dataMemHead* pContentElem_transferVideoData_mems = (TRANSFER_VIDEO_dataMemHead*)pContentElem->transferVideoData.buf_TRANSFER_VIDEO_dataMemHead_mems;
										int i;
										TRANSFER_VIDEO_dataMemHead* pMem;
										int iPos;

										for (iPos = 0, i = 0; i < pContentElem->transferVideoData.usCnt; iPos += (int)pContentElem_transferVideoData_mems[i].uiLen, i++)
										{
											//
											if (iPos + pContentElem_transferVideoData_mems[i].uiLen > pContentElem->transferVideoData.uiLen)
											{
												traceLogA("tmpHandler_toMixer_video failed: iPos + mems[i].uiLen > content.uiLen");
												goto errLabel;
											}

											//
											ref TRANSFER_VIDEO_DATA tmp_pMixer = ref *pMixer;
											if  (  pMixer->usCnt  ==  160 )
											{
												int ii = 0;
											}

											//
											if (pMixer->usCnt == Consts.CONST_maxTransferAvDataMems
												|| pContentElem_transferVideoData_mems[i].uiLen + pMixer->uiLen > Consts.bufSize_transferVideoData
																				 //||  pContentElem->transferVideoData.stat.tSelectTime		//  2011/01/25. 对需要对方发送响应包的请求包，应改独立发送。所以先发送其他的包，然后独立发送此包
																				 )
											{
												if (pMixer->usCnt != 0)
												{
													if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewVideoStatus)
													{
														//qyShowInfo1(CONST_qyShowType_detail, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer_video: flush mixer data. mixer.usCnt %d, mixer.uiLen %d,  mem.len %d."), pMixer->usCnt, pMixer->uiLen, pContentElem->transferVideoData.mems[i].uiLen);
													}

													tmpHandler_toSend(pCommonParam0_toSend, p1, (IntPtr)pMsgMixer);
												}

												MACRO_memset_misMsgTask(ref *pMsgMixer);
												if (0==pMsgMixer->uiType)
												{
													tmpInitMsgMixer(pProcInfo, pMisCnt, pMsgTask_av, pAvTran, ref pTask, Consts.CONST_imCommType_transferVideoData, pMsgElem, pMsgMixer);
												}
											}

											//
											TRANSFER_VIDEO_dataMemHead* pMixer_mems = (TRANSFER_VIDEO_dataMemHead*)pMixer->buf_TRANSFER_VIDEO_dataMemHead_mems;
											pMem = &pMixer_mems[pMixer->usCnt];
											mymemcpy((IntPtr)pMem, (IntPtr)(byte*)&pContentElem_transferVideoData_mems[i], (uint)sizeof(TRANSFER_VIDEO_dataMemHead));

											//
											mymemcpy((IntPtr)(pMixer->buf + pMixer->uiLen), (IntPtr)(pContentElem->transferVideoData.buf + iPos), pMem->uiLen);
											pMixer->uiLen += pMem->uiLen;

											pMixer->usCnt++;

											//  2011/01/26
											if  (  pMixer->usCnt  ==  161)
											{
												int ii = 0;
											}
#if __DEBUG__
pTask->photomosaic.stat.usCnt_pkts++;
//
//
if (pMem->stat.tSelectTime)
{
	int ii = 0;
	//showInfo_open(  0,  0,  _T(  "mixer: tSelectTime is not 0"  )  );
}

#endif

											//								 		
											//  2011/01/25. 对需要对方发送统计响应包的请求包，单独发送此包
											//
											continue;
										}

										if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewVideoStatus)
										{
											//qyShowInfo1(CONST_qyShowType_detail, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("tmpHadler_toMixer ok: mixer.usCnt %d, mixer.uiLen %d, tn_openAv_org %d"), pMixer->usCnt, pMixer->uiLen, pMixer->uiTranNo_openAvDev);
										}

									}
									break;
								default:
									break;
							}


							pMsgElem->tStartTime_processed = pTranInfo->tNow;
							pMsgElem->uiTranNo_processed = pTranInfo->uiTranNo;

							pTranInfo->nNodes_processed++;

						}
						break;
					default:
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, null, _T(""), _T(""), _T("ÔÚtmpHandler_toMixerÀï³öÏÖÁËÎ´´¦ÀíµÄÏûÏ¢ÀàÐÍ, %s"), qyGetDesByType1(CONST_misMsgTypeTable, ((MIS_MSGU*)pMsgElemParam)->uiType));
						goto errLabel;
						break;

				}

				if (pTranInfo->nNodes_processed >= pProcInfo.cfg.uiMaxSendMsgs)
				{
					iErr = 1; goto errLabel;
				}
			}

iErr = 0;
errLabel:
return iErr;
}



		public static unsafe int tmpHandler_toSendMedia(CTX_caller ctxCaller, ref MIS_MSGU param_0_unused, PARAM_findOrgMsg param_1, object taskElemParam)
		{
			int iErr = -1;

			//
			bool bDbg = false;
#if DEBUG
			//bDbg = true;
#endif

			//
			COMMON_PARAM pCommonParam0 = (COMMON_PARAM)param_1.commonParam0;
			COMMON_PARAM pCommonParam1 = (COMMON_PARAM)param_1.commonParam1;

			//
			object p0 = pCommonParam0;
			object p1 = pCommonParam1;

			//
			CCtxQmc pProcInfo = (CCtxQmc)pCommonParam0.p0;
			MIS_CNT pMisCnt = (MIS_CNT)pCommonParam0.p1;
			MIS_CHANNEL pChannel = (MIS_CHANNEL)pCommonParam0.p2;
			//
			TRAN_INFO_MIS* pTranInfo = (TRAN_INFO_MIS*)(IntPtr)pCommonParam1.p0;
			MIS_MSG_TASK* pMsgTask_o = &pTranInfo->pMsg_o->task;

			MIS_MSG_TASK* pMsgMixer = (MIS_MSG_TASK*)(IntPtr)pCommonParam1.p1;
			IM_CONTENTU* pContent_mixer = (IM_CONTENTU*)&pMsgMixer->data.buf;

			COMMON_PARAM commonParam0=new COMMON_PARAM();
			int tmpiRet = -1;

			if (null==pProcInfo || null==pMisCnt || null==pChannel || null==pMsgTask_o || null==pMsgMixer)
			{
				traceLogA("tmpHandler_toSendMedia failed. params error.");
				return -1;
			}

			string where_showInfo; where_showInfo = qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType);

			TASK_common taskElem = (TASK_common)taskElemParam;
			QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo, taskElem.m_index_taskInfo,"tmpHandler_toSendMedia");
			if (pTaskInfo == null) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;

			//
			if (bDbg)
            {
				//qyFuncs.showAllTc(ctxCaller, where_showInfo + " tmpHandler_toSendMedia enters");
            }

			//
			fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
			{
				
				//
				switch (((MIS_MSGU*)(IntPtr)pMsgElemParam)->uiType)
				{
					case Consts.CONST_misMsgType_task:
						{
							MIS_MSG_TASK* pMsgElem = (MIS_MSG_TASK*)(IntPtr)pMsgElemParam;
							//MIS_CNT* pMisCntElem = (MIS_CNT*)pMsgElem->pMisCnt;
							IM_CONTENTU* pContentElem = (IM_CONTENTU*)&pMsgElem->data.buf;
							//

							//if (pMisCntElem == pMisCnt)
							{
								if (pChannel.uiType == Consts.CONST_channelType_realTimeMedia)
								{
									AV_TRAN_INFO* pAvTranInfo = null;
									AV_TRAN_INFO tmp_avTranInfo;
									PROC_TASK_AV pTask = null;

									if (pContentElem->uiType == Consts.CONST_imCommType_transferAvInfo)
									{
										if (!pTaskInfo.var.m_bReplyTask)
										{
											TASK_transferAv task = (TASK_transferAv)taskElem;
											pAvTranInfo = &pContentElem->transferAvInfo.ass.tranInfo;
											pTask = task.myTask;// get_transferAvInfo_pTask(&pContentElem->transferAvInfo, _T("tmpHandler_toSendMedia"));  //  &pContentElem->transferAvInfo.myTask;						  
											if (null == pTask) goto errLabel;
										}
										else
										{


											//else if (pContentElem->uiType == Consts.CONST_imCommType_transferAvReplyInfo)

											TASK_transferAv task = (TASK_transferAv)taskElem;
											pTask = task.myTask;// get_transferAvReplyInfo_pTask(&pContentElem->transferAvReplyInfo, _T(""));  //  &pContentElem->transferAvReplyInfo.myTask;						  							        
											if (null == pTask) goto errLabel;
											//pAvTranInfo = &pContentElem->transferAvReplyInfo.tranInfo;
											tmp_avTranInfo = pTask.reply_avTranInfo;
											pAvTranInfo = &tmp_avTranInfo;
										}
									}

									if (null != pAvTranInfo)
									{
										using (CQyQ2Help q2Help = new CQyQ2Help())
										{
											;
											GENERIC_Q pOutputQ_audio = q2Help.getQ_toGetMsg(pTask.mixer.pOutputQ2);

											if (null==pOutputQ_audio)
											{               //  ²»ÊÇÊÓÆµ·þÎñÆ÷£¬¾ÍÃ»ÓÐÊä³ö¶ÓÁÐ¡£
												iErr = 0; goto errLabel;
											}

											MACRO_memset_misMsgTask(ref *pMsgMixer); //  Çå¿Õ

											//MACRO_makeCommonParam3(p0, (IntPtr)pMsgElem, 0, commonParam0);
									MACRO_makeCommonParam3(p0, taskElem, 0, commonParam0);
									tmpiRet = qTraverse(pOutputQ_audio, tmpHandler_toMixer_audio, commonParam0, p1);
											if (tmpiRet < 0)
											{
												//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, where_showInfo, _T(""), _T("tmpHandler_toMixer_audio failed. ½«empty¶ÓÁÐ"));
												emptyGenericQ(pOutputQ_audio);
												iErr = 0; goto errLabel;
											}

											//  ÕâÀï,»¹Òª°ÑÎ´·¢ËÍµÄÊý¾Ý°ü·Åµ½·¢ËÍ¶ÓÁÐÀï."  );
											if (pMsgMixer->uiType == Consts.CONST_misMsgType_task
												&& pContent_mixer->uiType == Consts.CONST_imCommType_transferAudioData
												  && pContent_mixer->transferAudioData.usCnt!=0)
											{
												tmpHandler_toSend(p0, p1, (IntPtr)pMsgMixer);
											}

											if (qRemoveMsg(pOutputQ_audio, b_tmpHandler_needRemoveMsg_cli, 0, p1) < 0)
											{
												traceLogA("Çå³ýÀ¬»ø°ü³ö´í¡£skipped.");
											}

											if (pTranInfo->nNodes_processed >= pProcInfo.cfg.uiMaxSendMsgs)
											{
												iErr = 1; goto errLabel;    //  Õâ±íÃ÷ÒÑ¾­´¦ÀíÍêÁË, Ò»¹²´Õ¹»ÁËÈô¸É°ü·ÅÈë·¢ËÍ¶ÓÁÐ
											}

											iErr = 0; goto errLabel;

										}
									}
								}

								if (pChannel.uiType == Consts.CONST_channelType_media)
								{
									AV_TRAN_INFO* pAvTranInfo = null;
									AV_TRAN_INFO tmp_avTranInfo;
									PROC_TASK_AV pTask = null;

									if (pContentElem->uiType == Consts.CONST_imCommType_transferAvInfo)
									{
										if (!pTaskInfo.var.m_bReplyTask)
										{
											TASK_transferAv task = (TASK_transferAv)taskElem;
											pAvTranInfo = &pContentElem->transferAvInfo.ass.tranInfo;
											pTask = task.myTask;// get_transferAvInfo_pTask(&pContentElem->transferAvInfo, _T(""));    //  &pContentElem->transferAvInfo.myTask;						  
											if (null == pTask) goto errLabel;
										}
										else { 	
										
											TASK_transferAv task = (TASK_transferAv)taskElem;
											pTask = task.myTask;// get_transferAvReplyInfo_pTask(&pContentElem->transferAvReplyInfo, _T(""));  //  &pContentElem->transferAvReplyInfo.myTask;						  							        
											if (null == pTask) goto errLabel;
											//pAvTranInfo = &pContentElem->transferAvReplyInfo.tranInfo;
											tmp_avTranInfo = pTask.reply_avTranInfo;
											pAvTranInfo = &tmp_avTranInfo;
										}
									}

									//
									if (null!=pAvTranInfo)
									{
										using (CQyQ2Help q2Help = new CQyQ2Help())
										{
											;
											GENERIC_Q pOutputQ_video = q2Help.getQ_toGetMsg(pTask.photomosaic.pOutputQ2);

											if (null==pOutputQ_video)
											{               //  ²»ÊÇÊÓÆµ·þÎñÆ÷£¬¾ÍÃ»ÓÐÊä³ö¶ÓÁÐ¡£
												iErr = 0; goto errLabel;
											}

											MACRO_memset_misMsgTask(ref *pMsgMixer); //  Çå¿Õ

											//MACRO_makeCommonParam3(p0, pMsgElem, 0, commonParam0);
											MACRO_makeCommonParam3(p0, taskElem, 0, commonParam0);
											tmpiRet = qTraverse(pOutputQ_video, tmpHandler_toMixer_video, commonParam0, p1);
											if (tmpiRet < 0)
											{
												//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, where_showInfo, _T(""), _T("tmpHandler_toMixer_video failed. ½«empty¶ÓÁÐ"));
												emptyGenericQ(pOutputQ_video);
												iErr = 0; goto errLabel;
											}

											//  ÕâÀï,»¹Òª°ÑÎ´·¢ËÍµÄÊý¾Ý°ü·Åµ½·¢ËÍ¶ÓÁÐÀï."  );
											if (pMsgMixer->uiType == Consts.CONST_misMsgType_task
												&& pContent_mixer->uiType == Consts.CONST_imCommType_transferVideoData
												  && pContent_mixer->transferVideoData.usCnt!=0)
											{
												//
												ref TRANSFER_VIDEO_DATA tmp_pContent_mixer = ref pContent_mixer->transferVideoData;

												//
												tmpHandler_toSend(p0, p1, (IntPtr)pMsgMixer);
											}

											if (qRemoveMsg(pOutputQ_video, b_tmpHandler_needRemoveMsg_cli, 0, p1) < 0)
											{
												traceLogA("Çå³ýÀ¬»ø°ü³ö´í¡£skipped.");
											}

											if (pTranInfo->nNodes_processed >= pProcInfo.cfg.uiMaxSendMsgs)
											{
												iErr = 1; goto errLabel;    //  Õâ±íÃ÷ÒÑ¾­´¦ÀíÍêÁË, Ò»¹²´Õ¹»ÁËÈô¸É°ü·ÅÈë·¢ËÍ¶ÓÁÐ
											}

											iErr = 0; goto errLabel;
										}
									}
								}
							}

							iErr = 0; goto errLabel;

						}
						break;
					default:
						break;
				}
			}
			
			

			iErr = 0;
		errLabel:

			//
			if (bDbg)
			{
				//qyFuncs.showAllTc(ctxCaller, where_showInfo + " tmpHandler_toSendMedia leaves");
			}



			return iErr;

		}

		public static unsafe void mcThreadProc_mis_toSend(object lpParameter)
		{
			MIS_CHANNEL pChannel = (MIS_CHANNEL)lpParameter;
			if (null==pChannel)
			{
				return;
			}
			MIS_CNT pMisCnt = pChannel.pMisCnt;
			if (null==pMisCnt) return;
			CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
			if (null==pProcInfo) return;
			CCtxQyMc pQyMc = pProcInfo.pQyMc;
			if (null==pQyMc) return;
			//
			int loopCtrl = 0;
			int i;
			int j;
			uint dwThreadId = myGetCurrentThreadId();
			uint dwRet;

			
			//  unsigned  int					len;
			MIS_MSGU* pMsg_o = null;
			//  int							iRet;
			//  BOOL							bThreadIdFound		=	false;
			COMMON_PARAM commonParam0=new COMMON_PARAM();
			COMMON_PARAM commonParam1=new COMMON_PARAM();
			//  nNodes_processed
			int tmpiRet = -1;
			TRAN_INFO_MIS tranInfo;
			//
			MIS_MSG_TASK* pMsgMixer = null;

			//
			bool bDbg = false;
			string str;

#if DEBUG
			//bDbg = true;
#endif

			//
			string where_showInfo; where_showInfo = string.Format("{0}_toS", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));

			//
			//
			CTX_MC_thread ctx = new CTX_MC_thread();
			ctx.ctxCaller.type = Consts.CONST_ctxCallerType_thread;
			ctx.ctxCaller.dwThreadId = myGetCurrentThreadId();
			fixed (char* pName = ctx.ctxCaller.tNameBuf.tNameBuf)
			{
				mytcsncpy(pName, where_showInfo, Consts.cntof_ctxCaller_tNameBuf);
			}


			//
			showInfo_open(0, where_showInfo, _T("starts"));

			//
			using (CQyMalloc mallocObj = new CQyMalloc())
			{
				using (CQyMalloc mallocObj_mixer = new CQyMalloc())
				{

					if (null == (pMsg_o = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;
					switch (pChannel.uiType)
					{
						case Consts.CONST_channelType_realTimeMedia:
						case Consts.CONST_channelType_media:
							if (null==(pMsgMixer = (MIS_MSG_TASK*)mallocObj_mixer.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSG_TASK)))) goto errLabel;
							break;
						default:
							break;
					}




					//
					GENERIC_Q pToSendQ = null; //  2011/01/23


					for (; bChannelOk(pChannel); loopCtrl++)
					{

						/*
						dwRet = WaitForSingleObject(pChannel->toSendQ2.hEvents[0], 3500);   //  µÈ·¢ËÍÖ¸Áî
						if (dwRet == WAIT_FAILED) goto errLabel;                            //  ÎªÁË·ÀÖ¹¶ª°ü£¬ÕâÀï²»ÅÐ¶Ï³¬Ê±£¬ÒÔoutputQÀïµÄÊý¾Ý°üÎª×¼
						if (dwRet == WAIT_TIMEOUT) continue;
						*/
						myWaitOne(pChannel.toSendQ2.hEvent0, 3500);

						//
						q2AvoidTriggerTooHigh(null, pChannel.toSendQ2);

						//
						if (bDbg)
                        {
							int nNodes = getQ2Nodes(pChannel.outputQ2);
							str=string.Format("{0}; after wait, nQNodes {1}",where_showInfo, nNodes);
							traceLog(str);
                        }

						//
						if (getQ2Nodes_toPostMsg(pChannel.outputQ2) > 40) continue;                   //  Êä³ö¶ÔÁÐÀï³¬¹ý20¸öÊý¾Ý°ü£¬¾Í²»ÓÃ×ª»»ÁË

						mymemset((IntPtr)(byte*)&tranInfo, 0, sizeof(TRAN_INFO_MIS));
						int tt = 0;
						tranInfo.uiTranNo = getuiNextTranNo(null, 0, ref tt);
						if (tranInfo.uiTranNo == -1) goto errLabel;
						mytime(ref tranInfo.tNow);
						tranInfo.pMsg_o = pMsg_o;

						MACRO_makeCommonParam3(pProcInfo, pMisCnt, pChannel, commonParam0);
						MACRO_makeCommonParam3((IntPtr)(byte*)&tranInfo, (IntPtr)pMsgMixer, 0, commonParam1);

						if (pChannel.uiType == Consts.CONST_channelType_realTimeMedia)
						{
							//
							if (bDbg)
							{
								qyFuncs.showInfo_open(0, where_showInfo, "before traverse");
							}

							//
							ref MIS_MSGU msg_unused = ref *pMsg_o;
							PARAM_findOrgMsg param = new PARAM_findOrgMsg();
							param.commonParam0 = commonParam0;
							param.commonParam1 = commonParam1;
							//
							tmpiRet = pProcInfo.processQ_media.qTraverse(ctx.ctxCaller,  tmpHandler_toSendMedia, ref msg_unused, param);
							if (tmpiRet < 0)
							{
								qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("mcThreadProc_mis_toSend: tmpHandler_toSendMedia failed."));
								goto errLabel;
							}

							//
							if (bDbg)
							{
								qyFuncs.showInfo_open(0, where_showInfo, "after traverse");
							}


						}
						if (pChannel.uiType == Consts.CONST_channelType_media)
						{
							if (bDbg)
							{
								qyFuncs.showInfo_open(0, where_showInfo, "before traverse");
							}

							//
							CMIS_MSGU p0 = new CMIS_MSGU();
							PARAM_findOrgMsg param = new PARAM_findOrgMsg();
							param.commonParam0 = commonParam0;
							param.commonParam1 = commonParam1;

							tmpiRet = pProcInfo.processQ_media.qTraverse(ctx.ctxCaller, tmpHandler_toSendMedia, ref p0.msgU, param);
							if (tmpiRet < 0)
							{
								qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("mcThreadProc_mis_toSend: tmpHandler_toSendMedia failed."));
								goto errLabel;
							}

							//
							if (bDbg)
							{
								qyFuncs.showInfo_open(0, where_showInfo, "after traverse");
							}

						}


						//  printQ_mis(  pToSendQ  );

						tranInfo.uiMaxSendMsgs = 4; //  
						do
						{
							if (null==pToSendQ || isQEmpty(pToSendQ))
							{
								using (CQyQ2Help q2Help = new CQyQ2Help())
								{									;
									pToSendQ = q2Help.getQ_toGetMsg(pChannel.toSendQ2);
									if (null == pToSendQ) break;
								}

								if (isQEmpty(pToSendQ)) break;
							}

							tmpiRet = qTraverse(pToSendQ, tmpHandler_toSend, commonParam0, commonParam1);
							if (tmpiRet < 0)
							{
								qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("mcThreadProc_mis_toSend: tmpHandler_toSendMedia failed."));
								goto errLabel;
							}
							if (qRemoveMsg(pToSendQ, b_tmpHandler_needRemoveMsg_cli, 0, commonParam1) < 0)
							{
								traceLog(("qRemoveMsg pToSendQ failed, skipped."));
							}
						}
						while (tranInfo.nNodes_processed < pProcInfo.cfg.uiMaxSendMsgs);

						//  printQ_mis(  &pChannel->toSendQ  );	 

						
					}

					
				}
			}

		errLabel:

			//
			showInfo_open(0, where_showInfo, _T("ends"));


			if (null!=pMisCnt)
			{
				pChannel.status.ulbSessionErr = true;
				pChannel.status.ulbToSendQuit = true;
			}
			Console.WriteLine("msgThreadProc_mis_toSend:  leaves");

			return;
		}

	}
}
