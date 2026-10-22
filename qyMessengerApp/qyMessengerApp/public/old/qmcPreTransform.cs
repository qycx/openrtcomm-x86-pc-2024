using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const int DEFAULT_dwToInMs = 5000;

}

namespace qy
{
#if false
	unsafe struct TMP_buf_myDRAW_VIDEO_DATA_pkts_preTransformVideo
	{
		public fixed byte buf[Consts.bufSize_myDRAW_VIDEO_DATA_pkts_preTransformVideo];
	};
#endif

	partial class qyFuncs
	{



		//
#if false
		public static unsafe int doPreTransformTaskVideo(QY_TRANSFORM pTransform, MIS_MSG_INPUT* pMsgTask, myDRAW_VIDEO_DATA* pkts, CQyMalloc_array mallocObj_pkts, int maxPkts, int* pnPkts, ref CTX_mc_pretrans pCtx)
		{
			int iErr = -1;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null == pMisCnt) return -1;
			//
			MSGR_ADDR* pAddr = pTransform.pAddr_logicalPeer;
			int iTaskId = pTransform.iTaskId;
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,pProcInfo, pTask.iIndex_taskInfo,"doPreTransformTaskVideo");
			if (null==pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
#if DEBUG
			chkTc(pTc, "doPreTrans enters");
#endif


			//  2012/03/18
			//QY_qThreadProcInfo_common		
			PRETRANS_procInfo pPreTransformVideo = pTransform.v_preTransThread;

			//
			int tmpiRet = 0;
			//  
			//char timeBuf[32] = "";
			TMP_timeBuf timeBuf;
			//TCHAR tHintBuf[256] = _T("");
			TMP_tHintBuf tHintBuf;

			//
			IM_CONTENTU* pContent = (IM_CONTENTU*)pMsgTask->data.buf;
			if (pContent->uiType != Consts.CONST_imCommType_transferVideoData) return -1;
			TRANSFER_VIDEO_DATA* pTransferVideoData = &pContent->transferVideoData;

			//  2014/11/11
			//TASK_AV_FROM pFrom = null;
			int index_pMems_from = 0;
			int index_activeMems_from = 0;

			index_pMems_from = pContent->transferVideoData.tmpInternal.param.index_pMems_from;
			index_activeMems_from = pTransform.index_activeMems_from;// pContent->transferVideoData.tmpInternal.param.index_activeMems_from;

			//
#if false
			pFrom = getTaskAvFrom(pTransform, index_pMems_from, index_activeMems_from, _T("pretrans"));
			if (null == pFrom) return -1;
#endif
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];



			//
			int i;
			uint curTickCnt = myGetTickCount(null);

			//TCHAR where_showInfo[32] = _T(""); _sntprintf(where_showInfo, mycountof(where_showInfo), _T("pretransV %d"), pTransform.index_activeMems_from);

			//  BYTE				*	pData_allocated		=	NULL;

			if (null == pAddr) return -1;


			//
			if (0 != pMsgTask->data.route.debugInfo.debug_myDRAW_VIDEO_DATA.dwTickCnt_start)
			{
				pMsgTask->data.route.debugInfo.debug_myDRAW_VIDEO_DATA.usPreTransInMs = (ushort)(curTickCnt - pMsgTask->data.route.debugInfo.debug_myDRAW_VIDEO_DATA.dwTickCnt_start);
			}
			if (0 != pMsgTask->dwTickCnt_start)
			{   //  2009/03/22
				int iElapse = (int)(curTickCnt - pMsgTask->dwTickCnt_start);
				if (iElapse > pProcInfo.cfg.policy.usTimeoutInMsToSwitch)
				{
					/*
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%I64u, %dms, skipped."), pFrom.idInfo.ui64Id, iElapse);

					DEBUG_myDRAW_VIDEO_DATA* pDebugPkt = &pMsgTask->data.route.debugInfo.debug_myDRAW_VIDEO_DATA;
					if (pDebugPkt->dwTickCnt_start)
					{
						pDebugPkt->usTransInMs = curTickCnt - pDebugPkt->dwTickCnt_start;
						_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s tE %d, e %d, p %d, d %d, o %d, tr %d"), tHintBuf, (int)pDebugPkt->usToEncInMs, (int)pDebugPkt->usEncedInMs, (int)pDebugPkt->usPostInMs, (int)pDebugPkt->usToDispatchInMs, (int)pDebugPkt->usOutputInMs, (int)pDebugPkt->usTransInMs);
					}
					*/

					goto errLabel;
				}
			}

			//  MACRO_setMyStep(  pTransform.video.debugStep,  11  );
			//MACRO_setMyStep(pPreTransformVideo->debugStep, 11);



			//  check
			if (pMsgTask->data.route.idInfo_from.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
				|| pTransferVideoData->uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev)
			{
#if __DEBUG__
				traceLogA("procTaskVideo: idInfo_from %I64u, videoData.uiTranNo %d !=  pFrom %I64u, %d", pMsgTask->data.route.idInfo_from.ui64Id, pTransferVideoData->uiTranNo_openAvDev, pFrom.idInfo.ui64Id, pFrom.video.tv_recvd.uiTranNo_openAvDev);
#endif
				goto errLabel;
			}

			//
			TRANSFER_VIDEO_dataMemHead* pTransferVideoData_mems = (TRANSFER_VIDEO_dataMemHead*)pTransferVideoData->buf_TRANSFER_VIDEO_dataMemHead_mems;
			TRANSFER_VIDEO_dataMemHead* pMem;
			//
			if (0 == pTransferVideoData->usCnt)
			{
				pTransferVideoData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
				pTransferVideoData_mems[0].uiLen = pTransferVideoData->uiLen;
			}
			if (pTransferVideoData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

			//  MACRO_setMyStep(  pTransform.video.debugStep,  31  );
			//MACRO_setMyStep(pPreTransformVideo->debugStep, 31);


#if false
			//if  (  !pTask->ucbVideoConference  )  
			{
				//	
				for (i = 0; i < pTransferVideoData->usCnt; i++)
				{
					pMem = &pTransferVideoData_mems[i];

					//
					pFrom.video.usCnt_pkts++;
					//  pFrom.video.usCnt_pkts  +=  pTransferVideoData->usCnt;		//  2011/01/26
					if (0 == pMem->stat.tSelectTime) continue;


					//
					if (  //pFrom.video.tv_recvd.uiTranNo_openAvDev  !=  pTransferVideoData->uiTranNo_openAvDev  ||  
						   0 == pFrom.video.tSelectTime || pFrom.video.tSelectTime != pMem->stat.tSelectTime)
					{
						pTransferVideoData->tmpInternal.result.usRespCode = Consts.CONST_qyRc_ok;

						//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s tSelectTime changed."), tHintBuf);

						//
						//pFrom.video.tv_recvd.uiTranNo_openAvDev  =  pTransferVideoData->uiTranNo_openAvDev;
						pFrom.video.tSelectTime = (Int64)pMem->stat.tSelectTime;
						pFrom.video.dwTickCnt_selectTime = curTickCnt;
						//  pFrom.video.usCnt_pkts  =  1;
						pFrom.video.usCnt_pkts = pTransferVideoData->usCnt;
						//		
					}
					else
					{
						if (pFrom.video.dwTickCnt_selectTime != curTickCnt)
						{
							pTransferVideoData->tmpInternal.result.fFps = (float)(pFrom.video.usCnt_pkts * 1000.0f / (curTickCnt - pFrom.video.dwTickCnt_selectTime));
						}

						pTransferVideoData->tmpInternal.result.usRespCode = Consts.CONST_qyRc_ok;

					}
					pTransferVideoData->tmpInternal.result.tSelectTime = pFrom.video.tSelectTime;  //  ½«ÕâÐ©·¢»Ø¡£2008/12/05
					pTransferVideoData->tmpInternal.result.usCnt_pkts = pFrom.video.usCnt_pkts;

					//
					/*
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s, %I64u, %d: recvd.pkts %d, expected. %d"), tHintBuf, pFrom.idInfo.ui64Id, pFrom.video.tv_recvd.uiTranNo_openAvDev, (long)pFrom.video.usCnt_pkts, (long)pMem->stat.usCnt_pkts);
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s, recvd.fps %.2f, expected. %d"), tHintBuf, pTransferVideoData->tmpInternal.result.fFps, (long)pMem->stat.usFps);
					//
					if (pMem->stat.usCnt_pkts == (ushort  )pFrom.video.usCnt_pkts  )  {
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s ok."), tHintBuf, pMem->stat.usFps);
				}
			else
				{
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s err"), tHintBuf);
				}
					*/

					//
					pTransferVideoData->tmpInternal.result.usDiff_pkts = (ushort)(pMem->stat.usCnt_pkts - pFrom.video.usCnt_pkts);

					//

				}
			}
#endif

			//  MACRO_setMyStep(  pTransform.video.debugStep,  41  );
			//MACRO_setStep(pPreTransformVideo->debugStep,  41  );

#if __DEBUG__
		if  (pProcInfo->cfg.debugStatusInfo.ucbPrint_v_in  )  
		{
			 traceLog(_T(  "transV: from %I64u, tn %d, usCnt %d, uiLen %d"  ),  pFrom.idInfo.ui64Id,  pMsgTask->uiTranNo,  pTransferVideoData->usCnt,  pTransferVideoData->uiLen  );
		}
#endif



			//  注意这里的nPkts. 如果是stream需要splitter的话，不能让*pnPkts增加，因为*pnPkts会用来表明有多少个包是分配好内存的。
			//  
			//  2011/12/09
			//  int  &  nPkts  =  *pnPkts;		
			int nPkts = *pnPkts;

			//
			bool bDecompressOk;
			int iPos;
			//TRANSFER_VIDEO_dataMemHead* pMem;

			//  2011/12/08
			myDRAW_VIDEO_DATA* pPkt_dec = null;

			uint uiSize_buf = 0;
			uint uiSize_dec = 0;

			//
			//  2015/10/04. 对共享纹理，不需要为图像数据分配内存
			bool bPkt_sharedTex = false;
			if (isPkt_sharedTex(pTransform))
			{
				bPkt_sharedTex = true;
			}

			//
			if (!bPkt_sharedTex)
			{
				int bits = 24;
				bits = 12;//
				//
				uiSize_dec = (uint)M_sizeImage( bits,pActiveMem_from.avStream.obj.tranInfo.video.vh_decompress.bih.biWidth, pActiveMem_from.avStream.obj.tranInfo.video.vh_decompress.bih.biHeight);
				//
				if (0 == uiSize_dec)
				{  //  2014/02/26
					uiSize_dec = (uint)M_sizeImage(bits,pTransform.video.tv.vh_decompress.bih.biWidth, pTransform.video.tv.vh_decompress.bih.biHeight);
				}
				//  
				if (pActiveMem_from.avStream.obj.tranInfo.video.compressor.ucCompressors != Consts.CONST_videoCompressors_stream)
				{
					if (0 == uiSize_dec)
					{
#if DEBUG
			traceLog(_T("doPreTransformTaskVideo failed: uiSize_dec err, uiSize_dec is 0"));
#endif
						goto errLabel;
					}
				}
				if (uiSize_dec > pProcInfo.cfg.uiBufSize_myDrawVideoData) uiSize_dec = pProcInfo.cfg.uiBufSize_myDrawVideoData;
			}

			pTransferVideoData_mems = (TRANSFER_VIDEO_dataMemHead*)pTransferVideoData->buf_TRANSFER_VIDEO_dataMemHead_mems;
			int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
			//
			for (iPos = 0, i = 0; i < pTransferVideoData->usCnt; iPos = (int)(iPos + pMem->uiLen), i++)
			{
				pMem = getMem(pTransferVideoData_mems, bufSize, i);// &pTransferVideoData_mems[i];

				//  MACRO_setMyStep(  pTransform.video.debugStep,  51  );
				//MACRO_setStep(pPreTransformVideo->debugStep, 51);

#if true  //def  __DEBUG__
				//  2015/01/15
				if (0 != pMem->uiPts)
				{
					if (0 == pTransform.v_preTransThread.helpInfo.pts.uiPts_lastRecvd)
					{
						pTransform.v_preTransThread.helpInfo.pts.uiPts_first = pMem->uiPts;
						pTransform.v_preTransThread.helpInfo.pts.dwTickCnt_start = myGetTickCount(null);
					}
					else
					{
						
						 
						   //TCHAR  tBuf[128];						
						   if  (  pMem->uiPts  !=  pTransform.v_preTransThread.helpInfo.pts.uiPts_lastRecvd  +  1  )  {
							string str;
							str=string.Format(  "pretrans err: pts err. pkt.pts {0}, last {1}",  pMem->uiPts,  pTransform.v_preTransThread.helpInfo.pts.uiPts_lastRecvd  );								
							   showInfo_open(  0,  null,  str  );								  
							   //  2015/02/20							  
						   }
						
						//
#if __DEBUG__
#endif
					}
					pTransform.v_preTransThread.helpInfo.pts.uiPts_lastRecvd = pMem->uiPts;
				}
#endif

				//
				if (iPos + pMem->uiLen > pTransferVideoData->uiLen)
				{
#if __DEBUG__
	traceLogA("procTaskVideo: Êý¾Ý³¤¶È²»¶Ô");
#endif
					goto errLabel;
				}
				if (0 == pMem->uiLen)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("transformTaskVideo warning: pMem->uiLen is 0, skipped"));
					continue;
				}
				if (0 != pMem->uiTranNo_openAvDev
					&& pMem->uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev)
				{       //  2012/01/09
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("transformTaskVideo warning: pMem->tn %d != pFrom.tn %d, skipped"), pMem->uiTranNo_openAvDev, pFrom.video.tv_recvd.uiTranNo_openAvDev);
					continue;
				}

				//
				bDecompressOk = false;

				if (0 != pMem->uiSampleTimeInMs)
				{
					/*
	int index_dmo = 0;
	if (index_dmo < 0 || index_dmo >= mycountof(pFrom.iDiffInMs_tStartAudios)) goto errLabel;

	pTransferVideoData_mems[i].uiSampleTimeInMs = (int)pTransferVideoData_mems[i].uiSampleTimeInMs - pFrom.sth.iDiffInMs_tStartAudios[index_dmo];    //  2009/05/26
					*/

#if __DEBUG__
#endif
				}

				//  
				if (bUseSplitter(pActiveMem_from.avStream.obj.tranInfo.video.compressor.ucCompressors, pActiveMem_from.avStream.obj.tranInfo.video.vh_stream.dwFourcc))
				{

					//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("doPretrans failed, bUseSpl is true"));
					goto errLabel;

					//
					continue;
				}


				//
				if (nPkts >= maxPkts) goto errLabel;
				pPkt_dec = &pkts[nPkts];
				mymemset((IntPtr)pPkt_dec, 0, sizeof(myDRAW_VIDEO_DATA));
				//
				//memcpy(&pPkt_dec->head, pMem, sizeof(pPkt_dec->head)  );
				pPkt_dec->head = *pMem;
				//
				pPkt_dec->decInfo.index_pMems_from = index_pMems_from;
				//  2016/12/27
				//  2014/09/04
				pPkt_dec->uiTaskType = pTransform.uiTaskType;  //  2014/10/01
															   //  2014/10/12
				if (pTransform.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
				{
					//pPkt_dec->decInfo.dynBmp.usIndex_obj  =  pFrom.video.dynBmp.usIndex_obj;
					MIS_MSG_TASK* pMsgTask_req = (MIS_MSG_TASK*)pTransform.pMsgTask;
					if (null != pMsgTask_req)
					{
						IM_CONTENTU* pContent_req = M_getMsgContent(pMsgTask_req->ucFlg, &pMsgTask_req->data);
						switch (pContent_req->uiType)
						{
							case Consts.CONST_imCommType_transferAvInfo:
								pPkt_dec->decInfo.dynBmp.resObj.uiObjType = pContent_req->transferAvInfo.ass.resObj.uiObjType;
								pPkt_dec->decInfo.dynBmp.resObj.usIndex_obj = pContent_req->transferAvInfo.ass.resObj.usIndex_obj;
								pPkt_dec->decInfo.dynBmp.idInfo.ui64Id = pMsgTask_req->data.route.idInfo_to.ui64Id;
								//
								//  2015/11/08
								if (pPkt_dec->decInfo.dynBmp.resObj.uiObjType == Consts.CONST_objType_screen)
								{
									pPkt_dec->decInfo.ucbUnresizable = true;
								}
								break;
							default:
								break;
						}
					}
				}
				//  2015/10/23
				if (isAvFlg_unresizable(pActiveMem_from.avStream.obj.tranInfo.taskAvProps.v.ucAvFlg))
				{
					pPkt_dec->decInfo.ucbUnresizable = true;
				}
				//
				//memcpy(&pPkt_dec->debugInfo, &pMsgTask->data.route.debugInfo.debug_myDRAW_VIDEO_DATA, sizeof(pPkt_dec->debugInfo)  );
				pPkt_dec->debugInfo = pMsgTask->data.route.debugInfo.debug_myDRAW_VIDEO_DATA;
				//  2015/02/24
				pPkt_dec->debugInfo.dwTickCnt_proc_io_read = pMsgTask->dwTickCnt_start;
				//
				pPkt_dec->debugInfo.dwTickCnt_doPretrans = myGetTickCount(null);
				//  2015/09/05
				pPkt_dec->debugInfo.uiTranNo_msgTask = pMsgTask->uiTranNo;
				//  2014/03/26
				M_set_pktId_alloc(pPkt_dec);
				//
				pPkt_dec->uiSize_dec = uiSize_dec;
				pPkt_dec->uiLen_enc = pMem->uiLen;
				uiSize_buf = pPkt_dec->uiSize_dec + pPkt_dec->uiOffset_enc + pPkt_dec->uiLen_enc;
				//
				pPkt_dec->head.uiLen = uiSize_buf;
				//
				if (0 != mallocObj_pkts.mems[nPkts].get(ref pPkt_dec->memory))
				{
					if (IntPtr.Zero == mallocObj_pkts.mems[nPkts].mallocf((int)uiSize_buf))
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("doPreTransV failed: malloc failed,uiSize_buf%d"), uiSize_buf);
						//  2014/05/12
						pCtx.status.errFlgs.bErrExists_mallocfFailed = true;
						//
						goto errLabel;
					}
					if (0 != mallocObj_pkts.mems[nPkts].get(ref pPkt_dec->memory)) goto errLabel;
				}
				if (pPkt_dec->memory.uiBufSize != uiSize_buf) goto errLabel;
				mymemcpy((IntPtr)((byte*)pPkt_dec->memory.m_pBuf + pPkt_dec->uiSize_dec + pPkt_dec->uiOffset_enc), (IntPtr)(pTransferVideoData->buf + iPos), pPkt_dec->uiLen_enc);
				nPkts++;
			}


			//  2014/03/04	
			if (0 != nPkts)
			{
				if (bUseSplitter(pActiveMem_from.avStream.obj.tranInfo.video.compressor.ucCompressors, pActiveMem_from.avStream.obj.tranInfo.video.vh_stream.dwFourcc))
				{
					//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("doPretrans failed, bUseSpl is true"));
					goto errLabel;
				}
			}

			//
			*pnPkts = nPkts;


			//
			iErr = 0;

		errLabel:

			//  MACRO_setMyStep(  pTransform.video.debugStep,  181  );
			//MACRO_setStep(pPreTransformVideo->debugStep, 181);


			//  
			if (iErr != 0)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, where_showInfo, _T(""), _T("tn %d, %s. %s"), pMsgTask->uiTranNo, (iErr ? _T("failed") : _T("OK")), tHintBuf);
#if __DEBUG__
	//traceLog(  _T(  "transformTaskVideo: %s"  ),  tHintBuf  );
#endif
			}


			if (0 == iErr)
			{
				//if  (  !pTask->ucbVideoConference  )  
				{
					mySendAvResp(ref pCtx.ctxCaller, pProcInfo, pMsgTask, (MIS_MSGU*)pCtx.pMsgBuf);
				}
			}


			//  MACRO_setMyStep(  pTransform.video.debugStep,  199  );
			M_setStep(ref pPreTransformVideo.debugStep, 198);

			//
#if DEBUG
			chkTc(pTc, "doPreTrans leaves");
#endif

			//
			return iErr;
		}


		public static unsafe void mcThreadProc_preTransformVideo(object lpParameter)
		{
			int iErr = -1;


			QY_TRANSFORM pTransform = (QY_TRANSFORM)lpParameter;
			if (null == pTransform) return;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null == pProcInfo) return;
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null == pTask) return;

			//QY_qThreadProcInfo_common
			PRETRANS_procInfo pPreTransformVideo = pTransform.v_preTransThread;

			//
			//DYN_LIB_DX* pDynLib = (DYN_LIB_DX*)((QY_DYN_LIBS*)pQyMc->env.pDynLibs)->pLib_dx;
			//  int								i;

			using (CQyMalloc mallocObj_msgs = new CQyMalloc()) {;
				MIS_MSGU* msgs = null;

				IntPtr[] pMsgs = new IntPtr[Consts.CONST_pMsgs_preTransformVideo];
				uint[] msgLens = new uint[Consts.CONST_pMsgs_preTransformVideo];

				int maxMsgs = Consts.CONST_pMsgs_preTransformVideo;            //  一次最多处理的包数。
				int nMsgs;
				int i;

				IM_CONTENTU* pContent = null;

				uint dwToInMs = Consts.DEFAULT_dwToInMs;

				uint dwTickCnt_lastVideoData = myGetTickCount(null); ;                       // 

				//CQyMalloc							mallocObj_pPkt_dec;

				using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc()) {;
					MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(sizeof(MIS_MSGU));
					if (null == pMsgBuf) return;

					//

					//
					//myDRAW_VIDEO_DATA pkts[CONST_pkts_preTransformVideo];
					TMP_buf_myDRAW_VIDEO_DATA_pkts_preTransformVideo buf_pkts;
					myDRAW_VIDEO_DATA* pkts = (myDRAW_VIDEO_DATA*)buf_pkts.buf;
					int nPkts = 0;

					bool bPktsRedirected;

					//CQyCoInit coInit;       //  2010/06/07

					CTX_mc_pretrans ctx;        //  2014/03/07

					//
					//memset(&ctx, 0, sizeof(ctx));
					ctx = new CTX_mc_pretrans();

					traceLogA("mcThreadProc_preTransformVideo enters");

					//MACRO_setStep(pPreTransformVideo.debugStep, CONST_threadStep_start);


					//  2014/05/08
					//_sntprintf(pPreTransformVideo.debugStep.showInfo.tWhere, mycountof(pPreTransformVideo.debugStep.showInfo.tWhere), _T("pretransV %d:%d"), pTransform.iTaskId, pTransform.index_activeMems_from);
					pPreTransformVideo.debugStep.showInfo.uiStep = 0;

					//
					//
					ctx.pMsgBuf = (IntPtr)pMsgBuf;

					//
					pPreTransformVideo.bRunning = true;
					//  memset(  &pPreTransformVideo.m_var,  0,  sizeof(  pPreTransformVideo.m_var  )  );

#if __DEBUG__
		traceLog(_T("Note: 这里最好能根据情况，将maxMsgs自动调整，对每个transform,分配10个misMsgU, 是很浪费的"));
#endif

					if (null == (msgs = (MIS_MSGU*)mallocObj_msgs.mallocf(Consts.CONST_iAllocType_longTimeMsg, maxMsgs * sizeof(MIS_MSGU))))
					{
						iErr = -100;
						goto errLabel;
					}
					for (i = 0; i < maxMsgs; i++)
					{
						pMsgs[i] = (IntPtr)(byte*)&msgs[i];
					}
					//
					if (pTask.ucbVideoConferenceStarter)
					{
						//  if  (  !(  pPreTransformVideo.pMsgTask_toCompress_videoConference  =  (  MIS_MSG_TALK  *  )mallocObj_pMsgTask_toCompress.malloc(  sizeof(  MIS_MSG_TALK  )  )  )  )  goto  errLabel;
					}

					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", _T("IsClient"), 0, pPreTransformVideo.debugStep.showInfo.tWhere, _T(""), _T("%s %d starts"), pPreTransformVideo.q2.cfg.name, pTransform.index_activeMems_from);

					for (; !pPreTransformVideo.bQuit;)
					{

						if (0 == dwToInMs)
						{
							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Warning: mcThreadProc_transformVideo: dwToInMs %d"), dwToInMs);
							dwToInMs = Consts.DEFAULT_dwToInMs;
						}

						//  2009/07/14
						//MACRO_setStep(pPreTransformVideo.debugStep, CONST_threadStep_toWait);

						//
						uint dwTickCnt0 = myGetTickCount(null);
						uint dwTickCnt;
						/*
						uint dwEvt = MsgWaitForMultipleObjects(
											 pPreTransformVideo.q2.cfg.usCnt_hEvents,      // mycountof(  pPreTransformVideo.hEvents  ),		// Number of events.
										   pPreTransformVideo.q2.hEvents,              // Location of handles.
										   false,                                       // Wait for all?
										   dwToInMs,                                    // How long to wait?
										   0);                                      // Any message is an 
																					// event.
						if (dwEvt == WAIT_FAILED)
						{
							iErr = -110;
							goto errLabel;
						}
						//
						dwTickCnt = myGetTickCount();

						if (dwEvt != WAIT_TIMEOUT)
						{
							dwEvt -= WAIT_OBJECT_0;
							switch (dwEvt)
							{
								case 0:
									dwTickCnt_lastVideoData = dwTickCnt;        //  audio data received
									break;
								case 1:
									traceLogA("transformVideo: event 1.");
									break;
								default:
									traceLogA("unknown event");
									iErr = -120;
									goto errLabel;
									break;
							}
						}
						*/
						myWaitOne(pPreTransformVideo.q2.hEvent0, (int)dwToInMs);

						//
						for (; !pPreTransformVideo.bQuit;)
						{

							//  2011/12/06
							if (isQ2Empty(pPreTransformVideo.q2)) break;

							//
							//MACRO_setStep(pPreTransformVideo.debugStep, CONST_threadStep_toGetMsg);


							for (i = 0; i < maxMsgs; i++)
							{
								msgLens[i] = (uint)sizeof(MIS_MSGU);
							}
							nMsgs = maxMsgs;
							//  len  =  sizeof(  pMsg[0]  );
							{
								using (CQyQ2Help q2Help = new CQyQ2Help())
								{
									;
									if (0 != qGetMsgs(q2Help.getQ_toGetMsg(pPreTransformVideo.q2), pMsgs, (uint[])msgLens, (uint*)&nMsgs)) break;
								}
							}

							//
							for (i = 0; i < nMsgs; i++) {
								mymemset((IntPtr)(((byte*)(pMsgs[i])) + msgLens[i]), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - msgLens[i]));
							}

#if __DEBUG__
				//  traceLog(  _T(  "preTransV: nMsgs %d"  ),  nMsgs  );
#endif
							if (0 == nMsgs) continue;

							for (i = 0; i < nMsgs; i++)
							{
								MIS_MSGU* pMsg = (MIS_MSGU*)pMsgs[i];

								//
								switch (pMsg->uiType) {
									/*
									case Consts.CONST_misMsgType_task:
										{
											//CQyMalloc mallocObj_pkts[CONST_pkts_preTransformVideo];
											using (CQyMalloc_array mallocObj_pkts = new CQyMalloc_array(Consts.CONST_pkts_preTransformVideo))
											{


												//MACRO_setStep(pPreTransformVideo.debugStep, CONST_threadStep_msgGot);

												//  2014/05/12
												if (0!=ctx.status.ucMsgSkipped_ifMallocFailed) {
#if __DEBUG__
									traceLog(_T("pretransV: msg skipped if mallocFailed"));
#endif
													ctx.status.ucMsgSkipped_ifMallocFailed--;
													if (0==ctx.status.ucMsgSkipped_ifMallocFailed) {
														//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, pPreTransformVideo.debugStep.showInfo.tWhere, _T(""), _T("pretransV: errExists_mallofFailed, nMsgSkipped %d. proc again"), (int)ctx.status.ucMsgSkipped_ifMallocFailed);
													}
													continue;
												}

												//
												nPkts = 0;

												//
												mymemset((IntPtr)(byte*)&ctx.status.errFlgs, 0, sizeof(CTX_mc_pretrans_status_errFlgs));
												//
												//doPreTransformTaskVideo(pTransform, &pMsg->task, pkts, mallocObj_pkts, mycountof(mallocObj_pkts) - nPkts, &nPkts, &ctx);
												doPreTransformTaskVideo(pTransform, &pMsg->task, pkts, mallocObj_pkts, Consts.CONST_pkts_preTransformVideo - nPkts, &nPkts, ref ctx);
												if (ctx.status.errFlgs.bErrExists_mallocfFailed)
												{
													ctx.status.ucMsgSkipped_ifMallocFailed = 15;    //  10;
													//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, pPreTransformVideo.debugStep.showInfo.tWhere, _T(""), _T("pretransV: errExists_mallofFailed, nMsgSkipped %d"), (int)ctx.status.ucMsgSkipped_ifMallocFailed);
													continue;
												}

												//
												M_setStep(ref pPreTransformVideo.debugStep, Consts.CONST_threadStep_msgProced);

												if (0==nPkts) break;


												//  2016/08/29									  
												if (isQ2Warning(pTransform.video.q2, 1))
												{
													int kk;
													uint len;
													int tmpiRet;
													//TCHAR tBuf[128] = _T("");
													int nQ2Nodes;

													nQ2Nodes = getQ2Nodes(pTransform.video.q2);
													//_sntprintf(tBuf, mycountof(tBuf), _T("too many pkts in transQ. %d nodes. try to remove some freom q"), nQ2Nodes);
													//showInfo_open(0, 0, tBuf);
													//
													for (kk = 0; kk < 300; kk++)
													{
														nQ2Nodes = getQ2Nodes(pTransform.video.q2);
														if (nQ2Nodes < pTransform.video.q2.cfg.uiMaxQNodes * 2 / 3)
														{
															break;
														}
														len = (uint)sizeof(MIS_MSGU);
														tmpiRet = q2GetMsg(pTransform.video.q2, ctx.pMsgBuf, &len);
														if (0!=tmpiRet)
														{
															break;
														}
													}
													//
													nQ2Nodes = getQ2Nodes(pTransform.video.q2);
													//_sntprintf(tBuf, mycountof(tBuf), _T("Removing finished. %d left"), nQ2Nodes);
													//showInfo_open(0, 0, tBuf);
												}

												//


												//
												//  这里要送去解码
												int j, k;
												int nPlay;
												for (j = 0; j < nPkts;)
												{
													nPlay = Math.Min(nPkts - j, Consts.CONST_pkts_out_preTransformVideo);
													bPktsRedirected = false;
													if (0==q2PostMsgAndTrigger((IntPtr)(byte*)&pkts[j], (uint)(nPlay * sizeof(myDRAW_VIDEO_DATA)), pTransform.video.q2))
													{
														bPktsRedirected = true;
													}
													if (bPktsRedirected)
													{
														for (k = j; k < j + nPlay; k++)
														{
															mallocObj_pkts.mems[k].detach();
														}
													}
													j += nPlay;
												}
											}
										}
										break;
									*/
									case Consts.CONST_misMsgType_input:
										{
											//CQyMalloc mallocObj_pkts[CONST_pkts_preTransformVideo];
											using (CQyMalloc_array mallocObj_pkts = new CQyMalloc_array(Consts.CONST_pkts_preTransformVideo))
											{


												//MACRO_setStep(pPreTransformVideo.debugStep, CONST_threadStep_msgGot);

												//  2014/05/12
												if (0 != ctx.status.ucMsgSkipped_ifMallocFailed)
												{
#if __DEBUG__
									traceLog(_T("pretransV: msg skipped if mallocFailed"));
#endif
													ctx.status.ucMsgSkipped_ifMallocFailed--;
													if (0 == ctx.status.ucMsgSkipped_ifMallocFailed)
													{
														//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, pPreTransformVideo.debugStep.showInfo.tWhere, _T(""), _T("pretransV: errExists_mallofFailed, nMsgSkipped %d. proc again"), (int)ctx.status.ucMsgSkipped_ifMallocFailed);
													}
													continue;
												}

												//
												nPkts = 0;

												//
												fixed (CTX_mc_pretrans_status_errFlgs* perrFlgs = &ctx.status.errFlgs)
												{
													mymemset((IntPtr)(byte*)perrFlgs, 0, sizeof(CTX_mc_pretrans_status_errFlgs));
												}
												//
												//doPreTransformTaskVideo(pTransform, &pMsg->task, pkts, mallocObj_pkts, mycountof(mallocObj_pkts) - nPkts, &nPkts, &ctx);
												doPreTransformTaskVideo(pTransform, &pMsg->input, pkts, mallocObj_pkts, Consts.CONST_pkts_preTransformVideo - nPkts, &nPkts, ref ctx);
												if (ctx.status.errFlgs.bErrExists_mallocfFailed)
												{
													ctx.status.ucMsgSkipped_ifMallocFailed = 15;    //  10;
																									//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, pPreTransformVideo.debugStep.showInfo.tWhere, _T(""), _T("pretransV: errExists_mallofFailed, nMsgSkipped %d"), (int)ctx.status.ucMsgSkipped_ifMallocFailed);
													continue;
												}

												//
												M_setStep(ref pPreTransformVideo.debugStep, Consts.CONST_threadStep_msgProced);

												if (0 == nPkts) break;


												//  2016/08/29									  
												if (isQ2Warning(pTransform.video.q2, 1))
												{
													int kk;
													uint len;
													int tmpiRet;
													//TCHAR tBuf[128] = _T("");
													int nQ2Nodes;

													nQ2Nodes = getQ2Nodes(pTransform.video.q2);
													//_sntprintf(tBuf, mycountof(tBuf), _T("too many pkts in transQ. %d nodes. try to remove some freom q"), nQ2Nodes);
													//showInfo_open(0, 0, tBuf);
													//
													for (kk = 0; kk < 300; kk++)
													{
														nQ2Nodes = getQ2Nodes(pTransform.video.q2);
														if (nQ2Nodes < pTransform.video.q2.cfg.uiMaxQNodes * 2 / 3)
														{
															break;
														}
														len = (uint)sizeof(MIS_MSGU);
														tmpiRet = q2GetMsg(pTransform.video.q2, ctx.pMsgBuf, &len);
														if (0 != tmpiRet)
														{
															break;
														}
													}
													//
													nQ2Nodes = getQ2Nodes(pTransform.video.q2);
													//_sntprintf(tBuf, mycountof(tBuf), _T("Removing finished. %d left"), nQ2Nodes);
													//showInfo_open(0, 0, tBuf);
												}

												//


												//
												//  这里要送去解码
												int j, k;
												int nPlay;
												for (j = 0; j < nPkts;)
												{
													nPlay = Math.Min(nPkts - j, Consts.CONST_pkts_out_preTransformVideo);
													bPktsRedirected = false;
													if (0 == q2PostMsgAndTrigger((IntPtr)(byte*)&pkts[j], (uint)(nPlay * sizeof(myDRAW_VIDEO_DATA)), pTransform.video.q2))
													{
														bPktsRedirected = true;
													}
													if (bPktsRedirected)
													{
														for (k = j; k < j + nPlay; k++)
														{
															mallocObj_pkts.mems[k].detach();
														}
													}
													j += nPlay;
												}
											}
										}
										break;

									default:
										break;
								}

							}
						}
					}

				}
			}
			iErr = 0;

		errLabel:

			//
			M_setStep(ref pPreTransformVideo.debugStep, Consts.CONST_threadStep_toExit);
			pPreTransformVideo.debugStep.iErr_thread = iErr;


			//
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", _T("IsClient"), 0, _T(""), _T(""), _T("%s %d ends."), pPreTransformVideo.q2.cfg.name, pTransform.index_activeMems_from);


			pPreTransformVideo.bRunning = false;

			//


			//
			M_setStep(ref pPreTransformVideo.debugStep, Consts.CONST_threadStep_end);

#if __DEBUG__
traceLogA("mcThreadProc_transformVideo %S leaves", pPreTransformVideo.q2.cfg.name);
#endif


			return;

		}
#endif

	}
}
