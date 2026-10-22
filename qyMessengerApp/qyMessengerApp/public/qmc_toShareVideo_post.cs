using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int toShareVideo_post_qmc(ref CTX_caller ctxCaller, CCtxQmc pProcInfo, uint uiCapType, int iIndex_capProcInfo, myDRAW_VIDEO_DATA* pkts, uint nPkts, ref COMPRESS_VIDEO pCompressVideo, QY_SHARED_OBJ pSharedObj, MIS_MSGU* pMsgBuf)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			int tmpiRet = 0;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			bool bDbg = false;
			string str;

			//
			if (null==pkts || 0==nPkts) return -1;

			uint dwCkID = 0;
			uint dwFlags = 0;
			uint lIndex = 0;
			uint dwQuality = 0;// pCompressVideo.u.vcm.iQuality;
			//  int						i;
			uint uiChannelType = Consts.CONST_channelType_media;

			Int64 tStartTran = pkts[0].input.tStartTran;           //  2011/08/30
			if (0==tStartTran)
			{
# if  __DEBUG__
				traceLog(_T("toShareVideo_post: tStartTran is 0"));
#endif
				mytime(ref tStartTran);
			}

			bool ucbNeedShowFrameInfo = false;

			//  2010/09/04
			SHARED_OBJ_USR pSharedObjUsr = null;
			if (null==pSharedObj) goto errLabel;
			pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
			if (null==pSharedObjUsr) goto errLabel;

			//
			if ( bDbg)
            {
				traceLog("toShareVideo_post enters");
            }


			if (pCompressVideo.tLastTime_post != tStartTran)
			{               //  Õâ±íÃ÷ÊÇÒ»ÃëµÄ¿ªÊ¼				

				//
# if  __DEBUG__
				if (!iIndex_capProcInfo)
				{
					int ii = 0;
				}
#endif

				//
				pCompressVideo.tLastTime_post = tStartTran;

				//  
				if ((int)(tStartTran - pCompressVideo.tLastTime_showFrameInfo) > pProcInfo.cfg.usNeedShowFrameInfoIntervalInS)
				{
					//
					ucbNeedShowFrameInfo = true;
				}


				//  2017/09/05
# if  __DEBUG__
#endif

				//
				uint dwTickCnt = myGetTickCount(null);
				int nElapseInMs = (int)(dwTickCnt - pCompressVideo.dwTickCnt_selectTime);

				//  2014/04/26. 2008/12/05, uiCnt_pkts_fromSelect
				if (0==pCompressVideo.tSelectTime
					|| pCompressVideo.uiCnt_pkts_fromSelect > 32 * 1024
					|| pCompressVideo.tmp_bMediaChannelReady != pProcInfo.av.video.bChannelReady
					//|| pCompressVideo.tmp_ucbNotSendData != pSharedObjUsr.ucbNotSendData
					|| Math.Abs(nElapseInMs) > 1 * 60 * 1000 //  2017/09/05
					)
				{
					using (CQySyncObj syncObj = new CQySyncObj())
					{

						fixed (char* pName = pProcInfo.cfg.mutexName_syncSendAv)
						{
							if (0 == syncObj.sync(new string(pName), pMisCnt.hMutex_syncSendAv, 0))
							{
								pCompressVideo.tSelectTime = tStartTran;
								pCompressVideo.dwTickCnt_selectTime = dwTickCnt;
								pCompressVideo.uiCnt_pkts_fromSelect = 0;
								pCompressVideo.fFps_sending = 0;

								pCompressVideo.usTimes_ok = 0;
								pCompressVideo.usTimes_failure = 0;

								pCompressVideo.tmp_bMediaChannelReady = pProcInfo.av.video.bChannelReady;
								//pCompressVideo.tmp_ucbNotSendData = pSharedObjUsr.ucbNotSendData;

								//
								ucbNeedShowFrameInfo = true;

								//
								//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("to select %d/s. index_capBmp %d"), pCompressVideo.usFrames_perSecond_expected, iIndex_capProcInfo);
							}
						}
					}
				}

			}


			//
#if false
			if (!pProcInfo.av.bVideoChannelReady)
			{
				//
				qyShowInfo1(  Consts.CONST_qyShowType_qwmComm,  null,  "",  pProcInfo.str_who_showInfo,  null,  _T(  ""  ),  _T(  ""  ),  _T(  "Warning: toShareV_post: mediaChannel is not ready."  )  );
				//		
				//
				if (!pSharedObjUsr.ucbNotSendData
					&& (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id
					|| 0!=pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id))
				{
					//MIS_CNT pMisCnt = null;
					MIS_MSG_applyForChkChannels_qmc msg_applyForChkChannels;
					uint dwTickCnt; dwTickCnt = (uint)myGetTickCount();
					int i;
					//  CQyQ2Help								q2Help;

					if (Math.Abs(dwTickCnt - pProcInfo.av.sdwTickCnt_applyForChkChannels) > 4000)
					{
						//  for  (  i  =  0;  i  <  mycountof(  pProcInfo.misCnts  );  i  ++  )  
						//for (i = 0; i < pProcInfo.ucCnt_misCnts; i++)
						{
							pMisCnt = pProcInfo.pMisCnt;// getMisCntByIndex(0, pProcInfo, i);  //  &pProcInfo.misCnts[i];
							//if (0==pMisCnt->cfg.dynCfg.cnt.usCntPort) continue;

							//  traceLogA(  "toShareBmp: applyForChkChannels"  );
							qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, "", pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("toShareV_post: media Channel not Ready, applyForChkChannels"));

							//
							mymemset((IntPtr)(byte*)&msg_applyForChkChannels, 0, sizeof(MIS_MSG_applyForChkChannels_qmc));
							msg_applyForChkChannels.uiType = Consts.CONST_misMsgType_applyForChkChannels_qmc;
							//msg_applyForChkChannels.pMisCnt = pMisCnt;
							//
							if (0!=q2PostMsgAndTrigger((IntPtr)(byte*)&msg_applyForChkChannels, (uint)sizeof(MIS_MSG_applyForChkChannels_qmc), pProcInfo.mediaQ2))
							{
								showNotification(0, null, null, 0, 0, 0, ("toShareBmp: applyForChkChannels failed"));
							}
						}
						pProcInfo.av.sdwTickCnt_applyForChkChannels = dwTickCnt;
					}

				}

				//  2015/05/12
#if __DEBUG__
				//traceLog(  _T(  "toShareVideo_post: bMediaChannelReady false. skipped"  )  );
#endif

				//  
				iErr = 0; goto errLabel;
			}
#endif
#if false
			if (pSharedObjUsr.ucbNotSendData)
			{
				iErr = 0; goto errLabel;
			}
#endif
			//  2014/11/16
			if (pSharedObjUsr.ucbInConfMosaic)
			{
#if __DEBUG__
				//traceLog(  _T(  "toShareVideo_post: ucbInConfMosaic true. "  )  );
#endif
				iErr = 0; goto errLabel;
			}

			//
			byte ucFlg = Consts.CONST_commFlg_routeTalkData; //  0;
			ushort usIndex_activeMems_from = 0;
			uint myuiSampleTimeInMs = 0;//  2009/05/30
			int ii;

			TRANSFER_VIDEO_DATA* pReq = null;

#if false
	pReg  =  pCompressVideo.pReqBuf_myVideoCapture;	
	if  (  pReq  )  {
		memset(  pReq,  0,  offsetof(  TRANSFER_VIDEO_DATA,  buf  )  );
		pReq->uiType  =  CONST_imCommType_transferVideoData;
		//  pReq->uiTranNo_openAvDev  =  pCompressVideo.uiTranNo_openAvDev;
		pReq->uiTranNo_openAvDev  =  pSharedObjUsr.uiTranNo_openAvDev_v;			//  2010/09/04
		//
	}
#else
			if (nPkts != 1)
			{
#if DEBUG
				traceLog(_T("toShareVideo_post failed: nPkts must is 1"));
#endif
				goto errLabel;
			}
			myDRAW_VIDEO_DATA* pPkt = &pkts[0];

			//  if  (  pPkt->memory.uiBufSize  !=  pPkt->uiSize_dec  +  sizeof(  TRANSFER_VIDEO_DATA  )  )  
			//if (pPkt->memory.uiBufSize < pPkt->uiSize_dec + sizeof(TRANSFER_VIDEO_DATA))  //  2014/09/30
			if (pPkt->memory.uiBufSize < pPkt->uiSize_dec + (uint)System.Runtime.InteropServices.Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") )  //  2014/09/30
			{
				str=string.Format("toShareVideo_post: uiBufSize err");
				showInfo_open(0, null, str);
				//
				goto errLabel;
			}

			pReq = (TRANSFER_VIDEO_DATA*)((byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec);

#endif


			//				  
			if (0==pReq->usCnt)
			{
				if (bDbg)
				{
					showInfo_open(0, _T("toShareVideo_post_qmc"), _T(" , req.usCnt is 0"));
				}
				//
				goto errLabel;
			}

#if __DEBUG__
#endif


			//  2008/11/22

			//  2017/09/04
#if __DEBUG__
			//
			int i;
			for (i = 0; i < pReq->usCnt; i++)
			{
				TRANSFER_VIDEO_dataMemHead* pMem = &pReq->mems[i];
				//
				pCompressVideo.iVal_dbg++;
				//
				pMem->stat.iVal_dbg = pCompressVideo.iVal_dbg;
				//
			}
			//
#endif


			//  2010/09/12
			//  pCompressVideo.uiCnt_pkts_fromSelect  ++  ;
			pCompressVideo.uiCnt_pkts_fromSelect += pReq->usCnt;
			//  pCompressVideo.usCnt_pkts  ++  ;
			if (ucbNeedShowFrameInfo)
			{
				ucFlg |= Consts.CONST_commFlg_routeTalkData;
				if (pCompressVideo.tRecvTime_resp_fromCli >= pCompressVideo.tFirstTime_showFrameInfo_withoutResp)
				{   //  ÕâÊÇÎªÁË³¬Ê±
					pCompressVideo.tFirstTime_showFrameInfo_withoutResp = tStartTran;
				}
				//  pReq->tLastTime_showFrameInfo_020534  =  pCompressVideo.tLastTime_showFrameInfo;
				//  pReq->usCnt_pkts  =  pCompressVideo.usCnt_pkts;
				//
				TRANSFER_VIDEO_dataMemHead* pMems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
				TRANSFER_VIDEO_dataMemHead* pMem = &pMems[pReq->usCnt - 1];

#if true
		//
		pMem->stat.usCnt_pkts  =  (  ushort  )pCompressVideo.uiCnt_pkts_fromSelect;
		pMem->stat.tSelectTime  =  pCompressVideo.tSelectTime;
		//				  				  
		pMem->stat.usFps  =  (  ushort  )pCompressVideo.fFps_sending;
#endif
				//
				pCompressVideo.tLastTime_showFrameInfo = tStartTran;               //  ÉèÖÃÊ±¼ä´Á
																					//  pCompressVideo.usCnt_pkts  =  0;
																					//
				if (0==iIndex_capProcInfo)
				{
					//TCHAR tBuf[128];
					//_sntprintf(tBuf, mycountof(tBuf), _T("send fps %.2f/s, input fps %d/s,  imageSize_input %d, imageSize %d, index_capBmp %d"), pCompressVideo.fFps_sending, (int)pCompressVideo.input.usFps, pCompressVideo.vh_decompress.bih.biSizeImage, pCompressVideo.vh_compress.bih.biSizeImage, iIndex_capProcInfo);
#if __DEBUG__
					_sntprintf(tBuf, mycountof(tBuf), _T("req.tSelectTime %I64u, uiLen %d, req.usCnt %d, iVal_dbg %d. stat.cnt_pts %d, iIndex_capBmp %d"), pMem->stat.tSelectTime, pReq->uiLen, (int)pReq->usCnt, pMem->stat.iVal_dbg, (int)pMem->stat.usCnt_pkts, iIndex_capProcInfo);
#endif
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), tBuf);
				}
			}



			//  _sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "compress ok, compressedDataLen is %d bytes\n"  ),  pProcInfo.av.iCompressBufSize  );  					  
			//  OutputDebugString(  tBuf  );

			int tt = 0;
			uint uiTranNo; if ((uiTranNo = getuiNextTranNo(null, 0, ref tt)) == -1) goto errLabel;
			int lenInBytes = 0;

#if DEBUG
			if (pProcInfo.cfg.debugStatusInfo.ucbPrint_v_out)
			{
				//traceLog(  _T(  "toShareBmp: tn %d, req.usCnt %d, len %d, mems[0].uiSampleTimeInMs %d, iDiffInMs_toStartAudio %d. iSampleTimeInMs %d"  ),  uiTranNo,  pReq->usCnt,  pReq->uiLen,  pReq->mems[0].uiSampleTimeInMs,  pSharedObj.iDiffInMs_tStartAudio,  pReq->mems[0].uiSampleTimeInMs_org  );
			}
			if (pSharedObj.var.bSlave)
			{
				ii = 0;
			}
			if ( bDbg)
            {
				//traceLog("toShareVideo_post: here");
            }
#endif


			//
			lenInBytes = (int)((int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") + pReq->uiLen);
			//
			bool ucbHaveMsgrs_sendLocalAv; ucbHaveMsgrs_sendLocalAv = false;
			bool ucbFixedRate; ucbFixedRate = false;
			//MIS_CNT pMisCnt; pMisCnt = pProcInfo.pMisCnt;

			// 

	/*if  (  !(pMisCnt  =  getMisCntByIndex(  0, pProcInfo, pSharedObj.curRoute_sendLocalAv.iIndex_misCnt  )  )  )  {
#if __DEBUG__
				traceLogA(  "getMisCntByIndex failed. index %d", pSharedObj.curRoute_sendLocalAv.iIndex_misCnt  );
#endif
		goto  errLabel;
	}
	*/

//if (!pSharedObjUsr.ucbNotSendData)
{

#if false
				pSharedObj.curRoute_sendLocalAv.route.debugInfo.dwTickCnt_me_start = myGetTickCount();
	if (ucbNeedShowFrameInfo)
	{   //  2008/05/16									 
		//  Int64	tNow;	mytime(  &tNow  );  pSharedObj.curRoute_sendLocalAv.route.debugInfo.tTime_start  =  tNow;
		pSharedObj.curRoute_sendLocalAv.route.debugInfo.tTime_start = tStartTran;
	}
#endif

#if false
	  		if  (  pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id  )  {	//  
				if  (  postMsg2MgrFunc_mc(  pMisCnt,  &pSharedObj.curRoute_sendLocalAv.route,  CONST_misMsgType_outputTask_toMix,  ucFlg,  CONST_qyCmd_sendMedia, tStartTran,  uiTranNo,  0,  pSharedObjUsr.iTaskId,  0,  (  char  *  )pReq,  lenInBytes,  &pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to,  null,  null,  0,  uiChannelType,  pMsgBuf,  false  )  )  traceLog(  _T(  "postMsg2MgrFunc_mc failed"  )  );
				}							   
			else  {
				  if  (  pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id  )  {
					  if  (  postMsg2MgrFunc_mc(  pMisCnt,  &pSharedObj.curRoute_sendLocalAv.route,  CONST_misMsgType_outputTask_toMix,  ucFlg,  CONST_qyCmd_sendMedia, tStartTran,  uiTranNo,  0,  pSharedObjUsr.iTaskId,  0,  (  char  *  )pReq,  lenInBytes,  0,  null,  null,  0,  uiChannelType,  pMsgBuf,  false  )  )  traceLog(  _T(  "postMsg2MgrFunc_mc failed"  )  );
				  }				  
			}
#else

#if false
				int msgLen = 0;
	if (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id)
	{   //
					fixed (MSG_ROUTE* pRoute = &pSharedObj.curRoute_sendLocalAv.route)
					{
						if (0!=makeMsg2MgrFunc_mc(pMisCnt, pRoute, Consts.CONST_misMsgType_task, ucFlg, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pSharedObjUsr.iTaskId, 0, (byte*)pReq, (uint)lenInBytes, pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to, null, null, null, uiChannelType, pMsgBuf, &msgLen)) traceLog(("postMsg2MgrFunc_mc failed"));
					}
	}
	else
	{
		if (0!=pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id)
		{
						fixed (ROUTE_sendLocalAv* pRoute = &pSharedObj.curRoute_sendLocalAv)
						{
							QY_MESSENGER_ID idInfo_logicalPeer;idInfo_logicalPeer.ui64Id = 0;
							if (0!=makeMsg2MgrFunc_mc(pMisCnt, &pRoute->route, Consts.CONST_misMsgType_task, ucFlg, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pSharedObjUsr.iTaskId, 0, (byte*)pReq, (uint)lenInBytes,idInfo_logicalPeer, null, null, null, uiChannelType, pMsgBuf, &msgLen)) traceLog(("postMsg2MgrFunc_mc failed"));
						}
		}
	}
	if (msgLen > 0)
	{
		CAP_procInfo_bmpCommon pCapBmp = (CAP_procInfo_bmpCommon)getCapBmpBySth(pProcInfo, pSharedObj.iIndex_capBmp, 0);
		if (null==pCapBmp) goto errLabel;

#if true
					//memcpy(  &pMsgBuf->task.data.route.debugInfo.debug_myDRAW_VIDEO_DATA,  &pPkt->debugInfo,  sizeof(  DEBUG_myDRAW_VIDEO_DATA  )  );
					pMsgBuf->task.data.route.debugInfo.debug_myDRAW_VIDEO_DATA= pPkt->debugInfo;
#endif
					//
					pMsgBuf->task.bFrom_sharedObj = true;
					pMsgBuf->task.index_sharedObj = pSharedObj.index_sharedObj;


					///////////////////
					if ((tmpiRet = pProcInfo.processQ_media.qTraverse(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsgBuf, (IntPtr)0)) <= 0)
		{
#if __DEBUG__
			traceLog(_T("postMsg2MgrFunc_mc: msg_task_toMix"), tmpiRet ? "failed" : "not found");
#endif
		}
	}
#endif

				//

				int msgLen = 0;

				//
				//
				MSG_ROUTE msgRoute = new MSG_ROUTE();
				if (0!=makeMsg2MgrFunc_mc(pMisCnt, ref msgRoute, Consts.CONST_misMsgType_task, ucFlg, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pSharedObjUsr.iTaskId, 0, (byte*)pReq, (uint)lenInBytes, 0, 0, 0, 0, uiChannelType, pMsgBuf, &msgLen)) traceLog(_T("postMsg2MgrFunc_mc failed"));
				//
				if (msgLen > 0)
				{
					//CAP_procInfo_bmpCommon* pCapBmp = (CAP_procInfo_bmpCommon*)getCapBmpBySth(pProcInfo, pSharedObj->iIndex_capBmp, 0);
					//if (!pCapBmp) goto errLabel;

#if true
					 qyFuncs.mymemcpy(  (IntPtr)(byte*)&pMsgBuf->task.data.route.debugInfo.debug_myDRAW_VIDEO_DATA,  (IntPtr)(byte*)&pPkt->debugInfo,  (uint)sizeof(  DEBUG_myDRAW_VIDEO_DATA  )  );
#endif

					//
					pMsgBuf->task.bFrom_sharedObj = true;
					pMsgBuf->task.index_sharedObj = pSharedObj.var.index_sharedObj;

					//
					PARAM_findOrgMsg param = new PARAM_findOrgMsg();

					//
					if ((tmpiRet = pProcInfo.processQ_media.qTraverse(ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *pMsgBuf, param)) <= 0)
					{
#if __DEBUG__
						traceLog(_T("postMsg2MgrFunc_mc: msg_task_toMix"), tmpiRet ? "failed" : "not found");
#endif
					}
				}





#endif

			}

#if __DEBUG__
if (pSharedObjUsr.ucbNotSendData) traceLogA("toShareBmp: not send data");
#endif

			//if (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id || 0!=pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id) ucbHaveMsgrs_sendLocalAv = true;
			//if (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id) ucbFixedRate = true;

			//
			if (ucbFixedRate != pCompressVideo.ucbFixedRate) pCompressVideo.ucbFixedRate = ucbFixedRate;

//


iErr = 0;

errLabel:

			//  if  (  pData_allocated  )  free(  pData_allocated  );
#if DEBUG
			if (bDbg)
			{
				traceLog("toShareVideo_post leaves");
			}
			//  
#endif

			return iErr;

}

}
}
