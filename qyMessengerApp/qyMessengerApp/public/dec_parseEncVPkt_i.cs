using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int chkToForceKeyFrame( CTX_caller ctxCaller, QY_TRANSFORM pTransform)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;

			//if (null==pTransform.pTranInfo_unused) return -1;
			if (null==pTransform.pTask
				//|| null==pTransform.pMsgTask
				)
			{
				return -1;
			}

			//AV_TRAN_INFO* pAvTran_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(null, pProcInfo, pTransform.index_taskInfo, "");
			if (pTaskInfo == null) goto errLabel;
			if (!pTaskInfo.bUsed) goto errLabel;
			QMC_taskData_common  taskData= pTaskInfo.getTaskData();
			if (taskData.msgU.uiType != Consts.CONST_misMsgType_task) goto errLabel;

			fixed (MIS_MSG_TASK* pMsgTask = &taskData.msgU.task)//(MIS_MSG_TASK*)pTransform.pMsgTask;
			{
				MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;// pMsgTask->pMisCnt;
				if (null == pMisCnt)
				{
					goto errLabel;
				}

				//  2015/02/15
#if false
			if (pTransform.video.decInfo.cnt_preDec > 1
				&& pTransform.video.decInfo.cnt_preDec <= 4)
			{
				iErr = 0; goto errLabel;
			}
#endif

				//
				uint dwTickCnt = myGetTickCount(null);
				int iDiffInMs = (int)(dwTickCnt - pTransform.video.decInfo.applyForKeyFrame.dwLastTickCnt_applyForKeyFrame);
				if (Math.Abs(iDiffInMs) < 1000)
				{
					iErr = 0; goto errLabel;
				}
				pTransform.video.decInfo.applyForKeyFrame.dwLastTickCnt_applyForKeyFrame = dwTickCnt;

				//
				ushort cmdCode = Consts.CONST_qyCmd_sendMedia;
				TASK_PROC_REQ taskProcReq;
				int lenInBytes;
				mymemset((IntPtr)(byte*)&taskProcReq, 0, sizeof(TASK_PROC_REQ));
				taskProcReq.uiType = Consts.CONST_imCommType_taskProcReq;
				//
				taskProcReq.usOp = Consts.CONST_imOp_recv_applyForKeyFrame;
				//
				taskProcReq.usTaskProcObjType = Consts.CONST_usTaskProcObjType_vStream;


				if (pTransform.video.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
				{
					showInfo_open(0, null, _T("chkToForceKeyFrame failed. vStream not deced is mine."));
					goto errLabel;
				}


				// 
				QY_MESSENGER_ID idInfo_dst;
				if (pTask.ucbVideoConferenceStarter)
				{
					idInfo_dst.ui64Id = pTransform.video.idInfo.ui64Id;
					//
					taskProcReq.vStream.uiTranNo_openAvDev = pTransform.video.tv.uiTranNo_openAvDev;
				}
				else if (pTask.bVideoConference())
				{
					//  2014/08/28. 暂时不考虑代理模式，所以直接发给视频源
					idInfo_dst.ui64Id = pTransform.video.idInfo.ui64Id;
					//
					taskProcReq.vStream.uiTranNo_openAvDev = pTransform.video.tv.uiTranNo_openAvDev;
				}
				else
				{
					idInfo_dst.ui64Id = pTransform.video.idInfo.ui64Id;
					//
					taskProcReq.vStream.uiTranNo_openAvDev = pTransform.video.tv.uiTranNo_openAvDev;
				}


				//
				string str;


				//		
				lenInBytes = sizeof(TASK_PROC_REQ);
				//
				Int64 tStartTran; uint uiTranNo;
				MACRO_prepareForTran(out tStartTran, out uiTranNo);

				//
				if (true)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("chkToForceKeyFrame : dst %I64u, cnt_preDec %d, cnt_postDec %d, nTimes_needKeyFrame %d"), idInfo_dst.ui64Id, pTransform.video.decInfo.cnt_preDec, pTransform.video.decInfo.cnt_postDec, pTransform.video.decInfo.nTimes_needKeyFrame);
					str = "chkToForceKeyFrame: dst " + idInfo_dst.ui64Id + ", tn " + uiTranNo;
					str = str + ", cnt_preDec " + pTransform.video.decInfo.cnt_preDec + ", cnt_postDec " + pTransform.video.decInfo.cnt_postDec + ", nTimes_needKeyFrame " + pTransform.video.decInfo.nTimes_needKeyFrame;
					showInfo_open(0, "", str);
				}

				//										 
				if (0 != postMsgTask2Mgr_mc(ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, cmdCode, tStartTran, uiTranNo, 0, 0, 0, (byte*)&taskProcReq, (uint)lenInBytes, pMsgTask->addr_logicalPeer.idInfo.ui64Id, 0, 0, idInfo_dst.ui64Id, 0, pTransform.tmp_pMsgU, false)) goto errLabel;
			}

			//
			iErr = 0;

		errLabel:

			return iErr;

		}

		public static unsafe int M_nalType_h265(byte* p)
		{
			return ((p)[2] == 1 ? ((p)[3] & 0x7e) : ((p)[4] & 0x7e)) >> 1;
		}

        //
        public static unsafe int dec_parseEncVPkt_i_useVideoQ2(uint uiTransformType, object pParent_transform, ref PKT_info_toDec pPktInfo_toDec_param, int* p_old_lPktId_getNewPkt, byte** ppInput, uint* puiSize, uint* puiSampleTimeInMs, uint* puiPts, SMPL_bsRead_param* pParam)
		{
			int iErr = -1;
			CCtxQmc pProcInfo = null;    //  QY_GET_procInfo_isCli(  );
										 //TCHAR tBuf[256] = _T("");
			string str;


			//  myDRAW_VIDEO_DATA  *  pPkt  =  pTransform.video.pCurPkt;
			//myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pPkt_myDRAW_VIDEO_DATA;
			fixed (PKT_info_toDec* pPktInfo_toDec = &pPktInfo_toDec_param)
			{


				//  检查一下。每个包都只能处理一次，否则，容易死循环
				if (null == p_old_lPktId_getNewPkt) return -1;
				if (0 == pPktInfo_toDec->lPktId_getNewPkt)
				{
					showInfo_open(0, null, _T("failed: lPktId_alloc 0"));
					return -1;
				}
				if (pPktInfo_toDec->lPktId_getNewPkt == *p_old_lPktId_getNewPkt)
				{
					showInfo_open(0, null, _T("failed: lPkdId_alloc==old"));
					return -1;
				}
				*p_old_lPktId_getNewPkt = pPktInfo_toDec->lPktId_getNewPkt;

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
				//
				if (!pPktInfo_toDec->bGot_msg) return -1;
				if (!pPktInfo_toDec->info.bGot_pkt) return -1;



				//  2015/09/11
				MIS_MSGU* pMsg = (MIS_MSGU*)pPktInfo_toDec->pMIS_MSGU;
				if (pMsg->uiType != Consts.CONST_misMsgType_input)
				{
					traceLog("dec_parseEncVPkt_i failed, only input msg accepted");
					goto errLabel;
				}
				IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->input.data.buf;
				if (pContent->uiType != Consts.CONST_imCommType_transferVideoData) goto errLabel;

				//
				TRANSFER_VIDEO_DATA* pReq = &pContent->transferVideoData;
				TRANSFER_VIDEO_dataMemHead* pMem;
				int i;
				int iPos;

				//
				iPos = pPktInfo_toDec->info.iPos;
				if (pPktInfo_toDec->info.index_mems < 0 || pPktInfo_toDec->info.index_mems >= pReq->usCnt) goto errLabel;
				i = pPktInfo_toDec->info.index_mems;

				TRANSFER_VIDEO_dataMemHead* pReq_mems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
				int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
				//				
				pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

				//
#if DEBUG
				if  (  false  )
				{
					string str1 = "dec_parseEncVPkt_i:" + "pts " + pMem->uiPts;
					showInfo_open(0, null, str1);
				}
#endif




				//
				if (iPos + pMem->uiLen > pReq->uiLen)
				{
#if __DEBUG__
				traceLogA("procTasAudio: ");
#endif
					goto errLabel;
				}
				if (0 == pMem->uiLen)
				{
#if __DEBUG__
				traceLogA("procTaskAudio warning: mem.uiLen is 0");
#endif
					goto errLabel;
				}


				//
#if DEBUG
				ref MIS_MSGU tmp_pMsg = ref *pMsg;
				ref TRANSFER_VIDEO_DATA tmp_pReq = ref *pReq;
				ref TRANSFER_VIDEO_dataMemHead tmp_pMem = ref *pMem;
#endif
				//
				if  (  pMem->uiTranNo_openAvDev==0)
                {
					pMem->uiTranNo_openAvDev = pReq->uiTranNo_openAvDev;
                }
				if  (pMem->idInfo.ui64Id == 0)
                {
					pMem->idInfo.ui64Id = pMsg->input.data.route.idInfo_from.ui64Id;
                }

                //
                /*
				if (0 != pTransform.video.tv.compressor.ucbNeedKeyFrameFirst)
				{
					if (!pTransform.video.decInfo.bGot_keyFrame)
					{
						if (pMem->ucbKeyFrame == 0)
						{
							showInfo_open(0, null, "dec_parseEncVPkt_i failed, not a keyFrame: need a keyFrame first");
							//
							chkToForceKeyFrame(null, pTransform);
							//
							iErr = Consts.CONST_qyRet_needKeyFrameFirst;
							//
							goto errLabel;
						}
						pTransform.video.decInfo.bGot_keyFrame = true;
					}
				}
				*/
                //
    





                //			
                if (pTransform.video.decInfo.iNextIndex_preDec >= Consts.MAX_cnt_preDec_array)
					{
						showInfo_open(0,null, _T("dec_parseEncVPkt_i failed, iNextIndex_preDec too large"));
						goto errLabel;
					}

					//
					int iNextIndex_preDec = pTransform.video.decInfo.iNextIndex_preDec;
					int iLastIndex_preDec = 0;
					if (0 == iNextIndex_preDec) iLastIndex_preDec = Consts.MAX_cnt_preDec_array - 1;
					else iLastIndex_preDec = iNextIndex_preDec - 1;

					//  2015/05/12
#if true  // def  __DEBUG__
					if (0 == pMem->uiSampleTimeInMs)
					{
						showInfo_open(0, null, _T("Warning: dec_parseEncVPkt_i: uiSampleTimeInMS is 0"));
					}
#endif

                //
#if DEBUG
                if (false)
                {
                    string str1 = "dec_parseEncVPkt_i: 296" + "pts " + pMem->uiPts  +  " sm "  +  pMem->uiSampleTimeInMs;
                    showInfo_open(0, null, str1);
                }
#endif



                //
                if (0 != pMem->uiSampleTimeInMs
						//&& pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iLastIndex_preDec] != pMem->uiSampleTimeInMs
						)
					{       //  很多包是数据控制包，不是图像数据，时间戳和图像包一样，所以不能入队列。2014/07/11
                            //				  
#if DEBUG
                    if (false)
                    {
                        string str1 = "dec_parseEncVPkt_i: 311" + "pts " + pMem->uiPts;
                        showInfo_open(0, null, str1);
                    }
#endif
                    //  2015/06/18


                    //  
                    //  注意这里，使用decTool的时候，这里不校验pts, 让dvt来校验. 2015/02/09
                    //
                    if (!pTransform.video.bUseDecTool)
						{

							//  2015/01/15
							if (0 != pMem->uiPts)
							{
								if (0 == pTransform.video.decInfo.pts.uiPts_lastRecvd)
								{
									pTransform.video.decInfo.pts.uiPts_first = pMem->uiPts;
									pTransform.video.decInfo.pts.dwTickCnt_start = myGetTickCount(null);
								}
								else
								{
									if (pMem->uiPts != pTransform.video.decInfo.pts.uiPts_lastRecvd + 1)
									{
										//
										str = string.Format("dec_parseEncVPkt_i: {0}: pts err, pkt {1}, last {2}", pMem->idInfo.ui64Id,  pMem->uiPts, pTransform.video.decInfo.pts.uiPts_lastRecvd);
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
						if (0 != pMem->uiPts)
						{
							pTransform.video.decInfo.pts.uiPts_lastRecvd = pMem->uiPts;
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
							//d_pre = (int)(pMem->uiSampleTimeInMs - pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iLastIndex_preDec]);
							//d_pretrans = (int)(pMem->debugInfo.dwTickCnt_doPretrans - pTransform.video.decInfo.dwTickCnt_doPretrans[iLastIndex_preDec]);
							//e_pretrans = (int)(myGetTickCount(null) - pMem->debugInfo.dwTickCnt_doPretrans);
							//
							//_sntprintf(tBuf, mycountof(tBuf), _T("pts %d. sm %dms. d_pre %dms, i_pre %d. lDiff_pre_post %d. d_preTrans %d"), pPkt->head.uiPts, pPkt->head.uiSampleTimeInMs, d_pre, iNextIndex_preDec, pTransform.video.decInfo.lDiff_pre_post, d_pretrans);
						}
						//pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iNextIndex_preDec] = (int)pMem->uiSampleTimeInMs;
						//pTransform.video.decInfo.uiPts_preDec_array[iNextIndex_preDec] = pMem->uiPts; //  2015/02/19
																										   //  2015/02/24
						//pTransform.video.decInfo.dwTickCnt_doPretrans[iNextIndex_preDec] = pPkt->debugInfo.dwTickCnt_doPretrans;
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


				//	 
				//
				//
				//
#if false
				if (pPkt->head.uiLen > pPkt->memory.uiBufSize
					|| pPkt->uiSize_dec + pPkt->uiOffset_enc + pPkt->uiLen_enc > pPkt->head.uiLen)
				{
#if __DEBUG__
				traceLog(_T("doDecVideo: uiSize_dec or uiLen error"));
#endif
					return -1;
				}
#endif
				//byte* encData = (byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec + pPkt->uiOffset_enc;
				byte* encData = pReq->buf + iPos;

				//myDRAW_VIDEO_DATA* pPkt_dec = pPkt;

				//
				//int index_pMems_from = pPkt->decInfo.index_pMems_from;

				//
				if (uiTransformType == Consts.CONST_uiTransformType_dec)
				{
					pTransform = (QY_TRANSFORM)pParent_transform;

					PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
					if (null == pTask) goto errLabel;
										int index_activeMems_from = pTransform.get_index_activeMems_from_decV();//.index_activeMems_from;
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

#if false
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
#endif



					//
					if (pTransform.video.ucbInited)
					{
						if (pTransform.video.index_activeMems_from != index_activeMems_from
							|| pTransform.video.idInfo.ui64Id != pMem->idInfo.ui64Id
							|| pTransform.video.tv.uiTranNo_openAvDev != pMem->uiTranNo_openAvDev
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
							str="dec_parseEncVPkt_i failed: trans.activeMem " + pTransform.video.index_activeMems_from  
								+  ",id " + pTransform.video.idInfo.ui64Id
								+  ",tn " + pTransform.video.tv.uiTranNo_openAvDev
								+  ",decFailed "  +  pTransform.video.status.ucbDecFailed 
								+ "!= "  +  index_activeMems_from  + ","  +  pMem->idInfo.ui64Id  + "," +  pMem->uiTranNo_openAvDev+"," + 0;
							showInfo_open(0, null, str);
							goto errLabel;
						}

					}

                    /*
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
						chkToForceKeyFrame(ctxCaller, pTransform);

						//
#if __DEBUG__
				//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "dec_parseEncVPkt_i : cnt_preDec %d, cnt_postDec %d"  ),  pTransform.video.decInfo.cnt_preDec,  pTransform.video.decInfo.cnt_postDec  );
#endif


					}
					*/

					//
                    uint iFourcc_enc = pTransform.video.tv.vh_stream.dwFourcc;
                    if (iFourcc_enc == Consts.CONST_fourcc_HEVC)
                    {

                        //
                        int nalType = M_nalType_h265(encData);

                        //
#if __DEBUG__
#if false
				 _sntprintf(tBuf, mycountof(tBuf), _T("dec_parseEncVPkt_i: nalType %d, pts %d, len %d"), nalType, pPkt->head.uiPts, pPkt->uiLen_enc);
				 showInfo_open(0, 0, tBuf);
#endif
#endif

                        //
                        if (!pTransform.video.decInfo.bGot_keyFrame)
                        {
                            bool ucbKeyFrame = false;
                            //
                            switch (nalType)
                            {
                                case Consts.NAL_IDR_W_RADL:
                                case Consts.NAL_IDR_N_LP:
                                    ucbKeyFrame = true;
                                    break;
                                case Consts.NAL_VPS:
                                    ucbKeyFrame = true;
                                    break;
                                default:
                                    {
                                        int ii = 0;
                                    }
                                    break;
                            }
                            //
                            if (!ucbKeyFrame)
                            {
                                //
								str=("dec_parseEncVPkt_i failed, %I64u, %d, not a keyFrame: need a keyFrame first")+
									pTransform.video.idInfo.ui64Id + " "  + pTransform.video.tv.uiTranNo_openAvDev;
                                showInfo_open(0, "", str);
                                //
                                chkToForceKeyFrame(null,pTransform);
                                //
                                iErr = Consts.CONST_qyRet_needKeyFrameFirst;
                                //
                                goto errLabel;
                            }
                            pTransform.video.decInfo.bGot_keyFrame = true;
                        }
                    }


                    //  2014/

                }


                //
                *ppInput = (byte*)encData;
				*puiSize = pMem->uiLen;// Pkt->uiLen_enc;
				//  2015/01/28
				if (null != puiSampleTimeInMs)
				{
					*puiSampleTimeInMs = pMem->uiSampleTimeInMs;// Pkt->head.uiSampleTimeInMs;
				}
				//  2015/02/09
				if (null != puiPts)
				{
					*puiPts = pMem->uiPts;// pPkt->head.uiPts;
				}
				//  2015/02/24
				if (null != pParam)
				{
					//pParam->dwTickCnt_doPretrans = pPkt->debugInfo.dwTickCnt_doPretrans;
				}

			}

iErr = 0;
errLabel:

return iErr;
}






	}
}
