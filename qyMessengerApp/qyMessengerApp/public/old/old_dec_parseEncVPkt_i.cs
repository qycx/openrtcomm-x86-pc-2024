using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

#if false//DEBUG
		public static unsafe int old_dec_parseEncVPkt_i(uint uiTransformType, object pParent_transform, void* pPkt_myDRAW_VIDEO_DATA, int* p_old_lPktId_alloc, byte** ppInput, uint* puiSize, uint* puiSampleTimeInMs, uint* puiPts, SMPL_bsRead_param* pParam)
		{
			int iErr = -1;
			CCtxQmc pProcInfo = null;    //  QY_GET_procInfo_isCli(  );
										 //TCHAR tBuf[256] = _T("");
			string str;


			//  myDRAW_VIDEO_DATA  *  pPkt  =  pTransform.video.pCurPkt;
			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pPkt_myDRAW_VIDEO_DATA;

			//  检查一下。每个包都只能处理一次，否则，容易死循环
			if (null == p_old_lPktId_alloc) return -1;
			if (0 == pPkt->lPktId_alloc)
			{
				showInfo_open(0, null, _T("failed: lPktId_alloc 0"));
				return -1;
			}
			if (pPkt->lPktId_alloc == *p_old_lPktId_alloc)
			{
				showInfo_open(0, null, _T("failed: lPkdId_alloc==old"));
				return -1;
			}
			*p_old_lPktId_alloc = pPkt->lPktId_alloc;

			//
			if (uiTransformType != Consts.CONST_uiTransformType_dec)
			{
				//MACRO_qyAssert(0, _T("dec_parseEncVPkt failed, transformType err"));
				return -1;
			}
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pParent_transform;
			CCtxQyMc pQyMc = g.g_pQyMc;
			pProcInfo = pQyMc.get_pProcInfo();// pTransform.pProcInfo;
			if (null == pProcInfo) return -1;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;

			//  2015/06/18
			ref QMC_status pQmcStatus = ref pProcInfo.status;

			//  2014/04/22
			if (uiTransformType == Consts.CONST_uiTransformType_dec)
			{
				//QY_TRANSFORM pTransform = (QY_TRANSFORM)pParent_transform;

				//			
				if (pTransform.video.decInfo.iNextIndex_preDec >= Consts.MAX_cnt_preDec_array)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("dec_parseEncVPkt_i failed, iNextIndex_preDec too large"));
					goto errLabel;
				}

				//
				int iNextIndex_preDec = pTransform.video.decInfo.iNextIndex_preDec;
				int iLastIndex_preDec = 0;
				if (0 == iNextIndex_preDec) iLastIndex_preDec = Consts.MAX_cnt_preDec_array - 1;
				else iLastIndex_preDec = iNextIndex_preDec - 1;

				//  2015/05/12
#if true  //def  __DEBUG__
				if (0 == pPkt->head.uiSampleTimeInMs)
				{
					showInfo_open(0, null, _T("Warning: dec_parseEncVPkt_i: uiSampleTimeInMS is 0"));
				}
#endif

				//
				if (0 != pPkt->head.uiSampleTimeInMs
					&& pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iLastIndex_preDec] != pPkt->head.uiSampleTimeInMs)
				{       //  很多包是数据控制包，不是图像数据，时间戳和图像包一样，所以不能入队列。2014/07/11
						//				  
#if __DEBUG__
#endif
					//  2015/06/18
					if (pTransform.video.bUseDecTool)
					{
						if (0 != pPkt->head.uiPts)
						{
							if (0 != pTransform.video.decInfo.pts.uiPts_lastRecvd)
							{
								if (pPkt->head.uiPts != pTransform.video.decInfo.pts.uiPts_lastRecvd + 1)
								{
									pTransform.transPtsInfo.v.uiCnt_ptsErr++;
									int iDiff = (int)(pPkt->head.uiPts - (pTransform.video.decInfo.pts.uiPts_lastRecvd + 1));
									if (iDiff > 0)
									{
										pTransform.transPtsInfo.v.uiCnt_notRecvd_pts += (int)iDiff;
									}
								}
							}
							//
							uint dwTickCnt = myGetTickCount(null);
							int iElapseInMs = (int)(dwTickCnt - pTransform.transPtsInfo.v.dwTickCnt_calc_start);
							if (iElapseInMs > 1000)
							{
								//
								if (pQmcCfg.debugStatusInfo.ucbShowPreDecV_pts)
								{
									/*
									TCHAR tBuf[128];
									_sntprintf(tBuf, mycountof(tBuf), _T("ptsErr: cnt %d/s. taskId %d"), pTransform.transPtsInfo.v.uiCnt_ptsErr, pTransform.iTaskId);
									showInfo_open(0, 0, tBuf);
									*/
								}
								//
								if (0 != pTransform.transPtsInfo.v.uiCnt_ptsErr)
								{  //  应该报警，													 
								   //  在procInfo里加一个
									Interlocked.Add(ref pQmcStatus.ptsStat.transPtsInfo_sum.v.uiCnt_ptsErr, pTransform.transPtsInfo.v.uiCnt_ptsErr);
									Interlocked.Add(ref pQmcStatus.ptsStat.transPtsInfo_sum.v.uiCnt_notRecvd_pts, pTransform.transPtsInfo.v.uiCnt_notRecvd_pts);
								}

								//
								fixed (TRANS_pts_info_v* pv = &pTransform.transPtsInfo.v)
								{
									mymemset((IntPtr)pv, 0, sizeof(TRANS_pts_info_v));
								}
								pTransform.transPtsInfo.v.dwTickCnt_calc_start = dwTickCnt;
							}
						}
					}

					//  
					//  注意这里，使用decTool的时候，这里不校验pts, 让dvt来校验. 2015/02/09
					//
					if (!pTransform.video.bUseDecTool)
					{

						//  2015/01/15
						if (0 != pPkt->head.uiPts)
						{
							if (0 == pTransform.video.decInfo.pts.uiPts_lastRecvd)
							{
								pTransform.video.decInfo.pts.uiPts_first = pPkt->head.uiPts;
								pTransform.video.decInfo.pts.dwTickCnt_start = myGetTickCount(null);
							}
							else
							{
								if (pPkt->head.uiPts != pTransform.video.decInfo.pts.uiPts_lastRecvd + 1)
								{
									//
									str = string.Format("dec_parseEncVPkt_i failed: pts err. pkt.pts {0}, last {1}", pPkt->head.uiPts, pTransform.video.decInfo.pts.uiPts_lastRecvd);
									showInfo_open(0, null, str);
									//  2015/02/20
									goto errLabel;
								}
								//
							}
							//pTransform.video.decInfo.pts.uiPts_lastRecvd  =  pPkt->head.uiPts;		
						}
					}
					//  2015/06/18
					if (0 != pPkt->head.uiPts)
					{
						pTransform.video.decInfo.pts.uiPts_lastRecvd = pPkt->head.uiPts;
					}

					//
					//
					int d_pre = 0;
					int d_pretrans = 0;
					int e_pretrans = 0;
					//
					if (pQmcCfg.debugStatusInfo.ucbShowPreDecVStatus)
					{
						//
#if __DEBUG__
						traceLog(_T("dec_parseEncVPkt_i: pkt.sm %dms. iDiff %dms"), pPkt->head.uiSampleTimeInMs, pPkt->head.uiSampleTimeInMs - pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iLastIndex_preDec]);
#endif
						//  2015/02/24
						d_pre = (int)(pPkt->head.uiSampleTimeInMs - pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iLastIndex_preDec]);
						d_pretrans = (int)(pPkt->debugInfo.dwTickCnt_doPretrans - pTransform.video.decInfo.dwTickCnt_doPretrans[iLastIndex_preDec]);
						e_pretrans = (int)(myGetTickCount(null) - pPkt->debugInfo.dwTickCnt_doPretrans);
						//
						//_sntprintf(tBuf, mycountof(tBuf), _T("pts %d. sm %dms. d_pre %dms, i_pre %d. lDiff_pre_post %d. d_preTrans %d"), pPkt->head.uiPts, pPkt->head.uiSampleTimeInMs, d_pre, iNextIndex_preDec, pTransform.video.decInfo.lDiff_pre_post, d_pretrans);
					}
					pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iNextIndex_preDec] = (int)pPkt->head.uiSampleTimeInMs;
					pTransform.video.decInfo.uiPts_preDec_array[iNextIndex_preDec] = pPkt->head.uiPts; //  2015/02/19
																									   //  2015/02/24
					pTransform.video.decInfo.dwTickCnt_doPretrans[iNextIndex_preDec] = pPkt->debugInfo.dwTickCnt_doPretrans;
					//
					{
						using (CQySyncObj syncObj = new CQySyncObj())
						{
							;
							//TCHAR tName[128];
							TMP_tBuf128 tName;
							int cntof_tName = Consts.cntof_tBuf128;

							//_sntprintf(tName, mycountof(tName), _T("%s%d"), CONST_mutexName_syncIndex_dec, pTransform.iTaskId);
							str = string.Format("{0}{1}", Consts.CONST_mutexName_syncIndex_dec, pTransform.iTaskId);
							mytcsncpy(tName.tBuf, str, cntof_tName);
							if (0 != syncObj.sync(new string(tName.tBuf), pMisCnt.hMutex_syncIndex_dec)) goto errLabel;

							pTransform.video.decInfo.iNextIndex_preDec++;
							if (pTransform.video.decInfo.iNextIndex_preDec >= Consts.MAX_cnt_preDec_array)
							{
								pTransform.video.decInfo.iNextIndex_preDec = pTransform.video.decInfo.iNextIndex_preDec % Consts.MAX_cnt_preDec_array;
							}
							if (pTransform.video.decInfo.lDiff_pre_post < Consts.MAX_cnt_preDec_array)
							{
								pTransform.video.decInfo.lDiff_pre_post++;
							}
						}
					}
					//
					pTransform.video.decInfo.cnt_preDec++;
					//
					if (pQmcCfg.debugStatusInfo.ucbShowPreDecVStatus)
					{  //  2015/02/24
					   //_sntprintf(tBuf, mycountof(tBuf), _T("%s, cnt_pre %d, e_pretrans %d. transQ %d+%d"), tBuf, pTransform.video.decInfo.cnt_preDec, e_pretrans, pTransform.video.q2.qs[0].uiQNodes, pTransform.video.q2.qs[1].uiQNodes);
					   //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T("preDec"), _T(""), _T("%s"), tBuf);
					}
				}
			}

			//	 
			//		
			if (pPkt->head.uiLen > pPkt->memory.uiBufSize
				|| pPkt->uiSize_dec + pPkt->uiOffset_enc + pPkt->uiLen_enc > pPkt->head.uiLen)
			{
#if __DEBUG__
				traceLog(_T("doDecVideo: uiSize_dec or uiLen error"));
#endif
				return -1;
			}
			byte* encData = (byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec + pPkt->uiOffset_enc;
			myDRAW_VIDEO_DATA* pPkt_dec = pPkt;

			//
			int index_pMems_from = pPkt->decInfo.index_pMems_from;

			//
			if (uiTransformType == Consts.CONST_uiTransformType_dec)
			{
				pTransform = (QY_TRANSFORM)pParent_transform;

				PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
				if (null == pTask) goto errLabel;

				int index_activeMems_from = pTransform.index_activeMems_from;
#if false
				TASK_AV_FROM pFrom = null;

			//
			pFrom = getTaskAvFrom(pTransform, index_pMems_from, index_activeMems_from, _T("dec_parseEncVPkt_i"));
			if (null==pFrom) return -1;
#endif
				//
				//
				CTX_caller ctxCaller = new CTX_caller();
				//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));
				ctxCaller.type = Consts.CONST_ctxCallerType_thread;
				ctxCaller.dwThreadId = myGetCurrentThreadId();
				ctxCaller.bTmp = true;

				//
				QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, (CCtxQmc)pProcInfo, pTask.iIndex_taskInfo, "dec_parseVEncPkt_i");
				if (null == pTaskInfo) goto errLabel;
				QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
				if (pTaskData == null) goto errLabel;
				if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
				QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
				DLG_TALK_videoConference videoConference = pTc.getVideoConference();

				//
#if DEBUG
				chkTc(pTc, "dec_parseEncVPkt");
#endif

				//
				if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) goto errLabel;
				DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];



				//
				if (pTransform.video.ucbInited)
				{
					if (pTransform.video.index_activeMems_from != index_activeMems_from
						|| pTransform.video.idInfo.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
						|| pTransform.video.tv.uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev
						|| pTransform.video.status.ucbDecFailed                                            //  2012/03/19
						)
					{
						//  2014/04/07
						pTransform.video.status.ucbNeedReinited = true;
						//
						/*
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("dec_parseEncVPkt_i failed: trans.from %d,id %I64u, tn %d, decFailed %d != %d,%I64u,%d,%d"),
							pTransform.video.index_pMems_from, pTransform.video.idInfo.ui64Id, pTransform.video.tv.uiTranNo_openAvDev, (int)pTransform.video.status.ucbDecFailed,
							index_pMems_from, pFrom.idInfo.ui64Id, pFrom.video.tv_recvd.uiTranNo_openAvDev, 0);
						*/
						goto errLabel;
					}

				}

				//  2014/12/06
				uint dwTickCnt = myGetTickCount(null);
				bool bNeedChk = false;
				if (0 == pTransform.video.decInfo.cnt_postDec) bNeedChk = true;
				else
				{
					if (dwTickCnt < pTransform.video.decInfo.dwLastTickCnt_postDec + 2000)
					{
						pTransform.video.decInfo.nTimes_needKeyFrame = 0;
					}
					else
					{
						pTransform.video.decInfo.nTimes_needKeyFrame++;
						if (pTransform.video.decInfo.nTimes_needKeyFrame > 20)
						{
							bNeedChk = true;
						}
					}
				}

				//  2014/08/26
				if (bNeedChk)
				{

					//
					chkToForceKeyFrame(ref ctxCaller, pTransform);

					//
#if __DEBUG__
				//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "dec_parseEncVPkt_i : cnt_preDec %d, cnt_postDec %d"  ),  pTransform.video.decInfo.cnt_preDec,  pTransform.video.decInfo.cnt_postDec  );
#endif


				}

				//  2014/

			}


			//
			*ppInput = (byte*)encData;
			*puiSize = pPkt->uiLen_enc;
			//  2015/01/28
			if (null != puiSampleTimeInMs)
			{
				*puiSampleTimeInMs = pPkt->head.uiSampleTimeInMs;
			}
			//  2015/02/09
			if (null != puiPts)
			{
				*puiPts = pPkt->head.uiPts;
			}
			//  2015/02/24
			if (null != pParam)
			{
				pParam->dwTickCnt_doPretrans = pPkt->debugInfo.dwTickCnt_doPretrans;
			}


			iErr = 0;
		errLabel:

			return iErr;
		}

#endif

	}
}
