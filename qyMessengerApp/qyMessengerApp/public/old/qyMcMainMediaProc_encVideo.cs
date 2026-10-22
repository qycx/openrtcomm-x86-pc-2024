using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace qy
{


	unsafe struct TMP_buf_myDRAW_VIDEO_DATA_pkts_encVideo
	{
		public fixed byte buf[Consts.bufSize_myDRAW_VIDEO_DATA_pkts_encVideo];
	};

	//
#if false

	partial class qyFuncs
{

		public static unsafe int doEncVideoPkt(CCtxQmc pProcInfo, QY_SHARED_OBJ pSharedObj, CAP_procInfo_bmpCommon pCapBmp, myDRAW_VIDEO_DATA* pPkt1)
		{
			int iErr = -1;

#if DEBUG
			int dwTickCnt_start=0;
			int dwTickCnt_before_compressV = 0;
			int dwTickCnt_compressV = 0;
			uint dwTickCnt_afterEnc = 0;

			dwTickCnt_start = (int)myGetTickCount(null);
#endif
			string str;
			bool bDbg = false;

			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			//
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
			//
			int tmpiRet = 0;

			if (null==pPkt1) return -1;
			//  if  (  !pPkts  ||  !nPkts  )  return  -1;
			myDRAW_VIDEO_DATA* pPkts = pPkt1;
			uint nPkts = 1;


		ref COMPRESS_VIDEO pCompressVideo = ref pCapBmp.compressVideo;

		uint dwCkID = 0;
		uint dwFlags = 0;
		uint lIndex = 0;
			uint dwQuality = 0;// pCompressVideo.u.vcm.iQuality;
		//  int						i;
		uint uiChannelType = Consts.CONST_channelType_media;

		Int64 tStartTran = pPkts[0].input.tStartTran;			//  2011/08/30
	if  (  0==tStartTran  )  {
#if __DEBUG__
				traceLog(_T(  "doEncVideoPkt: tStartTran is 0"  )  );
#endif
		mytime(  &tStartTran  );
	}


	SHARED_OBJ_USR pSharedObjUsr = null;
	if  (  null==pSharedObj  )  goto  errLabel;
	pSharedObjUsr  =  getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr  );
	if  (  null==pSharedObjUsr  )  goto  errLabel;

			//

			//
			if  (pCompressVideo.tLastTime_sendVideo  !=  tStartTran  )  {				//  Õâ±íÃ÷ÊÇÒ»ÃëµÄ¿ªÊ¼				


		//  2011/07/30. 这里计算发送帧速
		pCompressVideo.usFps_toSend  =  pCompressVideo.usFramesThisSecond_toSend;
		//
		uint dwTickCnt; dwTickCnt  =  myGetTickCount(null);
		if  (dwTickCnt<pCompressVideo.dwTickCnt_selectTime  +  500  )  pCompressVideo.fFps_sending  =  pCompressVideo.usFrames_perSecond_expected;
		else  pCompressVideo.fFps_sending  =  (float  ) (pCompressVideo.uiCnt_pkts_fromSelect*  1000.0f  /  (dwTickCnt  -  pCompressVideo.dwTickCnt_selectTime  )  );


		//
		pCompressVideo.tLastTime_sendVideo  =  tStartTran;				
		pCompressVideo.usFramesThisSecond_toSend  =  0;


#if false
				if  (pCompressVideo.ucbFixedRate  )  {  //  ±íÃ÷Òª²ÎÓëÊÓÆµ»áÒé¡£¶ÔÊÓÆµ»áÒé£¬²ÉÓÃ¹Ì¶¨Ö¡ËÙ
				pCompressVideo.ucSeconds_perFrame_expected  =  pCompressVideo.compressor.common.ucSeconds_perFrame;	//  pProcInfo.cfg.policy.video.ucSeconds_perFrame_videoConference;
				pCompressVideo.usFrames_perSecond_expected  =  pCompressVideo.compressor.common.usMaxFps_toShareBmp;	//  pProcInfo.cfg.policy.video.usFrames_perSecond_videoConference;
			}
		else
{
	//  ·ÖÎöÏìÓ¦Çé¿ö
	if (0!=pCompressVideo.tFirstTime_showFrameInfo_withoutResp)
	{

		if (pCompressVideo.tRecvTime_resp_fromCli < pCompressVideo.tFirstTime_showFrameInfo_withoutResp)
		{   //  Î´ÊÕµ½µÄÇé¿öÏÂ

			if (tStartTran - pCompressVideo.tFirstTime_showFrameInfo_withoutResp > pCompressVideo.ucTimeoutInS_showFrameInfoResp)
			{                           //  5Ãë¶¼Ã»ÊÕµ½ÏìÓ¦µÄ

				pCompressVideo.usTimes_failure += (MAX_failures_frames_perSecond + 1) / 2;     //  ¶Ô³¬Ê±£¬Òª¾¡¿ì½µµÍÖ¡ËÙ¡£ÊÇÑÏÖØÎÊÌâ

				if (pCompressVideo.ucTimeoutInS_showFrameInfoResp < MAX_ucTimeoutInS_showFrameInfoResp) pCompressVideo.ucTimeoutInS_showFrameInfoResp++;  //  ×î´óÊÇ30,²»ÄÜ³¬ÁË
																																							//
				if (pCompressVideo.usFrames_perSecond_expected == 1)
				{
					if (pCompressVideo.ucSeconds_perFrame_expected < MAX_ucSeconds_perFrame) pCompressVideo.ucSeconds_perFrame_expected++;                //  ÔÚ³¬Ê±Çé¿öÏÂ£¬ÓÃÕâ¸ö¼ä¸ôÀ´¼õÉÙÊä³ö£¬±ÜÃâÍ¨µÀ¶ÂÈû¡£																  
				}

				//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "Speed down, fps [%d]/s, spf [%ds]"  ),  pCompressVideo.usFrames_perSecond_expected,  pCompressVideo.ucSeconds_perFrame_expected  );

			}
		}
		else
		{                                                       //  ÊÕµ½ÏìÓ¦µÄÇé¿ö
			pCompressVideo.ucSeconds_perFrame_expected = 0;        //  ÊÕµ½ÁËÁ¢¼´¼ä¸ô²ÎÊý½µÎª0						  
		}

	}

	//  µ÷Õû×´Ì¬
	if (pCompressVideo.usTimes_failure)
	{

		if (isVideoNormalState(ref pCompressVideo))
		{
			if (pCompressVideo.usTimes_failure > MAX_failures_frames_perSecond)
			{               //  È¡ÏûÎÈ¶¨Ì¬
				pCompressVideo.usFps_ok = 0;
				pCompressVideo.bTopFrames = 0;
				//
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Try to readjust speed"));
			}
		}

		if (!isVideoNormalState(ref pCompressVideo))
		{
			bool bSelect = false;

			if (pCompressVideo.usTimes_failure > MAX_failures_frames_perSecond)
			{
				if (pCompressVideo.usFrames_perSecond_expected > pCompressVideo.compressor.common.usMinFps_toShareBmp   )
				{                                   //  ±ØÐëÒª±£³ÖÖ¡ËÙÔÚ1/SÒÔÉÏ
					pCompressVideo.usFrames_perSecond_expected--;
				}

				bSelect = TRUE;

				//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "Speed down. set fps %d/s <-> actual %.2f/s"  ),  pCompressVideo.usFrames_perSecond_expected,  pCompressVideo.fFps_sending  );
			}
			if (!bSelect)
			{
				if (pCompressVideo.usTimes_ok >= 5)
				{
					bSelect = TRUE;

					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Try to test speed. %d/s <-> actual %.2f/s )"), pCompressVideo.usFrames_perSecond_expected, pCompressVideo.fFps_sending);
				}
			}

			if (bSelect)
			{
								using (CQySyncObj syncObj = new CQySyncObj())
								{
									if (0 == syncObj.sync("", pMisCnt.hMutex_syncSendAv, 0))
									{

										pCompressVideo.tSelectTime = tStartTran;
										pCompressVideo.dwTickCnt_selectTime = myGetTickCount();
										pCompressVideo.uiCnt_pkts_fromSelect = 0;

										pCompressVideo.usTimes_ok = 0;
										pCompressVideo.usTimes_failure = 0;                                                            //  ÐÂµÄ½µµÍÖ¡ËÙÐèÒª½«´íÎóÇå0
									}
								}
			}
		}
		//
		if (pCompressVideo.usFps_ok == pCompressVideo.usFrames_perSecond_expected)
		{
			pCompressVideo.bTopFrames = true;                                                          //  ±íÃ÷ÊÇ×î¸ß¿ÉÓÃèåËÙ

			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Speed is top. %d/s <-> actual %.2f/s."), pCompressVideo.usFrames_perSecond_expected, pCompressVideo.fFps_sending);
		}
		//
	}
	else
	{     //  Õý³£ÊÕµ½ÁËÏìÓ¦					  	  
		if (!isVideoNormalState(ref pCompressVideo))
		{
			if (pCompressVideo.usTimes_ok >= 10)
			{       //  ÕâÀï£¬Ê¹ÓÃÁË10ÃëÀ´ÅÐ¶ÏÊÇ·ñÆ½ÎÈÆðÀ´¡£
				pCompressVideo.usFps_ok = pCompressVideo.usFrames_perSecond_expected;     //  ÕâËµÃ÷ÊÇ¿ÉÒÔÊ¹ÓÃµÄÖ¡ËÙ¡£µ«ÊÇ»¹Ã»È·¶¨ÊÇ·ñÊÇ×î¸ßµÄÆµÂÊ¡£

				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Speed is ok. input %d/s, expected %d/s, actual %.2f/s. "), pCompressVideo.input.usFps, pCompressVideo.usFrames_perSecond_expected, pCompressVideo.fFps_sending);

				if (!pCompressVideo.bTopFrames)
				{                                                   //  ÊÔÍ¼ÌáÉýÖ¡ËÙ
					if (pCompressVideo.usFrames_perSecond_expected >= pCompressVideo.compressor.common.usMaxFps_toShareBmp)
					{   //  ÔÝÊ±ÏÞÖÆÔÚ30Ö¡/S
						pCompressVideo.bTopFrames = true;
						//									  	
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Speed reaches top. expected %d/s, actual %.2f/s"), pCompressVideo.usFrames_perSecond_expected, pCompressVideo.fFps_sending);
					}
					else if (pCompressVideo.usFrames_perSecond_expected <= (ushort  )(pCompressVideo.fFps_sending + 2.5)  )  {       //  +0.2 ÊÇµ£ÐÄ¸¡µãÊýÓÐÐ¡Êý±»ºöÂÔ¡£					
										using (CQySyncObj syncObj = new CQySyncObj())
										{
											
											if (0 == syncObj.sync("", pMisCnt.hMutex_syncSendAv, 0))
											{
												pCompressVideo.usFrames_perSecond_expected++;
												pCompressVideo.tSelectTime = tStartTran;
												pCompressVideo.dwTickCnt_selectTime = myGetTickCount();
												pCompressVideo.uiCnt_pkts_fromSelect = 0;

												pCompressVideo.usTimes_ok = 0;
												pCompressVideo.usTimes_failure = 0;                                                            //  ÐÂµÄ½µµÍÖ¡ËÙÐèÒª½«´íÎóÇå0

												//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Speed up. expected %d/s, actual %.2f/s"), pCompressVideo.usFrames_perSecond_expected, pCompressVideo.fFps_sending);
											}
										}
					}
				}
			}
		}
	}

	//  if  (  !isVideoNormalState(  pCompressVideo  )  )  ucbNeedShowFrameInfo  =  true;								//  ·ÇÎÈ¶¨Ì¬ÐèÒª²»Í£µÄÈ¥ÎÊ¿Í»§¶Ë¡£Ò»Ö±µ½½¨Á¢ÎÈ¶¨Ì¬ÎªÖ¹
}                                                                                                                   //  ÎÈ¶¨Ì¬¿¿¶¨Ê±Æ÷¶¨ÆÚÎÊ¿Í»§¶Ë, ·ÀÖ¹ÖÐ¼ä³ö´íÁË¡£
#endif

//  2008/12/05, ÔÚµÚÒ»´Î»òÕßuiCnt_pkts_fromSelectÌ«´óÊ±£¬ÒªÖØÐÂÉèÖÃÏÂ
if (0==pCompressVideo.tSelectTime
	|| pCompressVideo.uiCnt_pkts_fromSelect > 32 * 1024
	|| pCompressVideo.tmp_bMediaChannelReady != pProcInfo.bMediaChannelReady()
	//|| pCompressVideo.tmp_ucbNotSendData != pSharedObjUsr.ucbNotSendData
	)
{
					using (CQySyncObj syncObj = new CQySyncObj())
					{
						
						if (0 == syncObj.sync("", pMisCnt.hMutex_syncSendAv, 0))
						{
							pCompressVideo.tSelectTime = tStartTran;
							pCompressVideo.dwTickCnt_selectTime = myGetTickCount(null);
							pCompressVideo.uiCnt_pkts_fromSelect = 0;
							pCompressVideo.fFps_sending = 0;

							pCompressVideo.usTimes_ok = 0;
							pCompressVideo.usTimes_failure = 0;

							pCompressVideo.tmp_bMediaChannelReady = pProcInfo.bMediaChannelReady();
							//pCompressVideo.tmp_ucbNotSendData = pSharedObjUsr.ucbNotSendData;

							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("to select %d/s"), pCompressVideo.usFrames_perSecond_expected);
						}
					}
}
	}

	//  2015/10/14
	QY_Q2 p_mediaQ2 = pProcInfo.get_mediaQ2();
if (null==p_mediaQ2) goto errLabel;
			//
#if false
			if (!pProcInfo.bMediaChannelReady())
{
	//
	if (!pSharedObjUsr.ucbNotSendData
		&& (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id
		|| 0!=pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id))
	{
		MIS_MSG_applyForChkChannels_qmc msg_applyForChkChannels;
		int dwTickCnt; dwTickCnt = (int)myGetTickCount();
		int i;
		//  CQyQ2Help								q2Help;

		if (dwTickCnt - pProcInfo.sdwTickCnt_applyForChkChannels > 4000)
		{
			//for  (  i  =  0;  i  <  mycountof(  pProcInfo.misCnts  );  i  ++  )  
			{

				//  traceLogA(  "toShareBmp: applyForChkChannels"  );
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("toShareBmp: media Channel not Ready, applyForChkChannels"));

				//
				mymemset((IntPtr)(byte*)&msg_applyForChkChannels, 0, sizeof(MIS_MSG_applyForChkChannels_qmc));
				msg_applyForChkChannels.uiType = Consts.CONST_misMsgType_applyForChkChannels_qmc;
				//msg_applyForChkChannels.pMisCnt = pMisCnt;
				//
				if (0!=q2PostMsgAndTrigger((IntPtr)(byte*)&msg_applyForChkChannels, (uint)sizeof(MIS_MSG_applyForChkChannels_qmc), p_mediaQ2))
				{
					//showNotification(0, 0, 0, 0, 0, 0, ("toShareBmp: applyForChkChannels failed"));
				}
			}
			pProcInfo.sdwTickCnt_applyForChkChannels = (uint)dwTickCnt;
		}

	}

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


//
byte ucFlg = Consts.CONST_commFlg_routeTalkData;  //  0;
ushort usIndex_activeMems_from = 0;
uint myuiSampleTimeInMs = 0;//  2009/05/30
int ii;

//  2011/12/24
int index_pReq = 0;
bool bInited_pReq = false;


//
byte ucSeconds_perFrame; ucSeconds_perFrame = pCompressVideo.ucSeconds_perFrame_expected; //  
																									//  if  (  pProcInfo.cfg.policy.video.ucSeconds_perFrame_videoConference  )  ucSeconds_perFrame  =  pProcInfo.cfg.policy.video.ucSeconds_perFrame_videoConference;	//  2010/06/27
																									//

for (ii = 0; ii < nPkts; ii++)
{

	int iSampleTimeInMs = (int)pPkts[ii].head.uiSampleTimeInMs;
	byte* pInput = (byte*)pPkts[ii].memory.m_pBuf;
	BITMAPINFOHEADER* pBmpInfo_input = (BITMAPINFOHEADER*)&pPkts[ii].bih;           //  2011/12/05
	bool bCompressOk = false;

	//
	if ((0==ucSeconds_perFrame || (0!=ucSeconds_perFrame && 0==(tStartTran % ucSeconds_perFrame)))
		//&& !pSharedObjUsr.ucbNotSendData
		&& pCompressVideo.usFramesThisSecond_toSend < pCompressVideo.usFrames_perSecond_expected
		//&& pProcInfo.bMediaChannelReady()  //  2009/02/26
		&& !isQ2Warning(p_mediaQ2, pQmcCfg.envVar.usQNodes_warningInterval_mediaQ)     //  
		)
	{
		//  pCompressVideo.dwTickCnt_lastFrameThisSecond  =  dwTickCnt;
		pCompressVideo.usFramesThisSecond_toSend++;
	}
	else
	{       //  skipped
		continue;
	}


	//  
	pCompressVideo.vh_compress.bih.biSizeImage = pCompressVideo.iCompressBufSize;
	if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_vcm)
	{
		//
	traceLog(("vcm is not supported in winCe"));
						goto errLabel;


	//

}
		else if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_dmo)
{
					goto errLabel;
			      }
		else
{
	CUS_MODULE_compress pModule = null;
	//
	M_getCompressModule(pQyMc.cusModules, pCompressVideo.uiModuleType_toEnc, ref pModule);   //  2012/09/09

	if (null==pModule
		|| null==pModule.pf_qdcCompressVideo
		  || null==pModule.pf_qdcGetOutputByIndex) goto errLabel;


#if DEBUG
					if (bDbg)
					{
						dwTickCnt_before_compressV = (int)myGetTickCount(null);
						str = string.Format("pkt[{0}],pktNo {1}，before_compressV {2}ms", ii, pPkts[ii].debugInfo.lPktNo, dwTickCnt_before_compressV - dwTickCnt_start);
						traceLog(str);
					}
#endif


					//
					if (pModule.pf_qdcCompressVideo(ref pCompressVideo.u_qoi.myQoi.common, ref *pBmpInfo_input, (IntPtr)pInput, iSampleTimeInMs) < 0)
	{
		traceLogA("qdcCompressVideo failed");
		//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Video: qdcCompressVideo failed"));
		goto errLabel;
	}

#if DEBUG
					if (bDbg)
					{
						dwTickCnt_compressV = (int)myGetTickCount(null);
						str = string.Format("pkt[{0}],pktNo {1}，compressV {2}ms", ii, pPkts[ii].debugInfo.lPktNo, dwTickCnt_compressV - dwTickCnt_start);
						traceLog(str);
					}
#endif


					//
					int index_output;
	for (index_output = 0; ;)
	{

		if (index_pReq >= nPkts
			|| index_output > 200)
		{
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("doEncVideo failed: index_pReq >=  nPkts or i too big"));
			goto errLabel;
		}

		//
		TRANSFER_VIDEO_DATA* pReq = (TRANSFER_VIDEO_DATA*)((byte*)pPkts[index_pReq].memory.m_pBuf + pPkts[index_pReq].uiSize_dec);
		if (!bInited_pReq)
		{
			if (pReq!=null)
			{
				//  memset(  pReq,  0,  offsetof(  TRANSFER_VIDEO_DATA,  buf  )  );
				pReq->uiType = Consts.CONST_imCommType_transferVideoData;
				//  pReq->uiTranNo_openAvDev  =  pCompressVideo.uiTranNo_openAvDev;
				pReq->uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;         //  2010/09/04
																						//
			}
			//
			bInited_pReq = true;
		}
						//TRANSFER_VIDEO_dataMemHead mems[Consts.CONST_maxTransferAvDataMems
						TRANSFER_VIDEO_dataMemHead* pMems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
						if (pReq->usCnt >= Consts.CONST_maxTransferAvDataMems)
		{
			//  装不下了
			index_pReq++; bInited_pReq = false;
			continue;
		}
		if (Consts.bufSize_transferVideoData  <= pReq->uiLen  )  {
		//traceLogA("toShareBmp failed: sizeof(  pReq->buf  ) %d  <=  pReq->len %d", sizeof(pReq->buf),  pReq->uiLen  );
		goto errLabel;
	}
	pMems[pReq->usCnt].uiLen = Consts.bufSize_transferVideoData  -pReq->uiLen;
						//  if  (  getDmoOutputByIndex(  &pCompressVideo.u.dmo,  i,  (  BYTE  *  )pCompressVideo.pCompressBuf,  (  uint  *  )&pCompressVideo.vh_compress.bih.biSizeImage,  NULL,  NULL  )  )  break;					
						uint st, rt;
						st = 0;rt = 0;
						tmpiRet = pModule.pf_qdcGetOutputByIndex(ref pCompressVideo.u_qoi.myQoi.common, index_output, (IntPtr)((byte*)pReq->buf + pReq->uiLen), ref pMems[pReq->usCnt].uiLen, ref st, ref rt);
	if (0!=tmpiRet)
	{
		if (tmpiRet == Consts.CONST_qdcRet_bufTooSmall)
		{
			if (0!=pReq->uiLen)
			{
				//  装不下了
				index_pReq++; bInited_pReq = false;
				continue;
			}

			//  如果整个pReq->buf都装不下，那肯定是这个包实在太大了，必须清除
			if (null!=pModule.pf_qdcClearOutputs)
			{
				pModule.pf_qdcClearOutputs(ref pCompressVideo.u_qoi.myQoi.common);
			}
		}
		break;
	}
#if DEBUG
						dwTickCnt_afterEnc = myGetTickCount(null);
	if (pQmcCfg.debugStatusInfo.ucb__Dump_h264_enc__)
	{
							string fn = Consts.CONST_fn_dump_h264_enc;
							byte* pBuffer = pReq->buf + pReq->uiLen;
							int BufferLen = (int)pMems[pReq->usCnt].uiLen;
							/*
		FILE* fp = null;
		fp = fopen(fn, "ab+");
		if (fp)
		{
			//
			fwrite(pBuffer, BufferLen, 1, fp);
			fclose(fp);
		}
		static int uiCnt_fn = 0;
		uiCnt_fn++;
		if (uiCnt_fn == 2000)
		{
			int i;
			i = 1;
		}
							*/
							GuiShare.pf_dumpData1((IntPtr)pBuffer, BufferLen, 0, 0, true, false, fn);

						}
#endif

						//  2008/11/12, 
						if (0!=iSampleTimeInMs)
	{
		//  pReq->mems[0].uiSampleTimeInMs  =  iSampleTimeInMs;				  
		pMems[pReq->usCnt].uiSampleTimeInMs = myuiSampleTimeInMs = (uint)(iSampleTimeInMs - pSharedObj.var.iDiffInMs_tStartAudio);    //  2009/05/26
																																//pReq->mems[pReq->usCnt].uiSampleTimeInMs_org  =  iSampleTimeInMs;									//  2009/05/27
	}
#if false
						if (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id)
	{
		//pReq->mems[pReq->usCnt].usIndex_ii  =  
		usIndex_activeMems_from = (ushort)pSharedObjUsr.iIndex_activeMems_from_videoConferenceStarter;
	}
#endif

	pReq->uiLen += pMems[pReq->usCnt].uiLen;
	pReq->usCnt++;

	//  注意，i只能这里增加，因为如果bufTooSmall,往往会重新读取一次
	index_output++;
}

bCompressOk = true;
			      
		}

		if (!bCompressOk)
{
	traceLog(("toShareBmp failed: compress failed"));
	goto errLabel;
}

#if __DEBUG__
#endif


continue;
	}

#if __DEBUG__
#endif



	iErr = 0;

		errLabel:

#if DEBUG
			if (bDbg)
			{
				uint dwTickCnt_end = myGetTickCount(null);
				int iElapseInMs = (int)(dwTickCnt_end - dwTickCnt_start);
				int iI_afterEnc = (int)(dwTickCnt_afterEnc - dwTickCnt_start);
				str = string.Format("doEncVideoPkt: npkts {0}, pkt.no {1}, eplase {2}ms, i_afterEnc {3}\n", nPkts, pPkts[0].debugInfo.lPktNo, iElapseInMs, iI_afterEnc);
				traceLog(str);
			}
#endif

			return iErr;
}


		public static unsafe int doEncodeVideo_default(CCtxQmc pProcInfo, CAP_procInfo_bmpCommon pCapProcInfo, MIS_CNT pMisCnt, MIS_MSGU* pMsgBuf, bool bPassThrough)
		{
			int iErr = -1;
			bool bDbg = false;


			//MC_VAR_isCli  *  pProcInfo  =  QY_GET_procInfo_isCli(  );

			int iIndex_sharedObj = pCapProcInfo.iIndex_sharedObj;
			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) return -1;



			int old_pktId = 0;
			//
			for (; ; )
			{
				if (bCapBmpQuit(pCapProcInfo, 0, 0))
				{
#if __DEBUG__
					traceLog(_T("doEncodeVideo_default failed：quit is true"));
#endif
					goto errLabel;
				}
				//
				for (; ; )
				{
					if (bCapBmpQuit(pCapProcInfo, 0, 0)) goto errLabel;
					if (0==getNextVPkt((IntPtr)pCapProcInfo.compressVideo.pCurPkt, pCapProcInfo.compressVideo.pInDataCacheQ, pCapProcInfo.thread.q2)) break;
					mySleep(40);
				}
				//
#if true
			  byte  *  pInput  =  null;
			  uint  uiInputSize  =  0;
			  bool  bNeed_forceKeyFrame  =  false;	//  2014/09/25
			  if  (  0!=enc_parseOrgVPkt_i(  Consts.CONST_uiTransformType_encVideo,  pCapProcInfo,  pCapProcInfo.compressVideo.pCurPkt,  &old_pktId,  &pInput,  &uiInputSize,  &bNeed_forceKeyFrame  )  )  {
#if __DEBUG__
						  traceLog(  _T(  "doTrans_default failed, parsePkt failed"  )  );
#endif
				goto errLabel;
			}
#endif

			myDRAW_VIDEO_DATA* pPkt = pCapProcInfo.compressVideo.pCurPkt;

				//
				if (bDbg)
				{
					string str;
					int nQNodes = getQ2Nodes(pCapProcInfo.thread.q2);
					str = string.Format("doEncodeVideo_default:pPkt.lpktNo {0}. nQNodes {1}", pPkt->debugInfo.lPktNo, nQNodes); ;
					traceLog(str);
				}

			//
			uint uiSize_enc;
			if (pPkt->memory.uiBufSize < pPkt->uiSize_dec)
			{
#if __DEBUG__
				traceLog(_T("Err: uiLen < uiSize_dec"));
#endif
				break;
			}
			uiSize_enc = pPkt->memory.uiBufSize - pPkt->uiSize_dec;
			//  if  (  uiSize_enc  !=  sizeof(  TRANSFER_VIDEO_DATA  )  )  
			if (uiSize_enc < sizeof(TRANSFER_VIDEO_DATA))  //  2014/10/01
			{
#if __DEBUG__
				traceLog(_T("Err: uiSize_enc %d <sizeof( TRANSFER_VIDEO_DATA ) %d. uiBufSize %d, uiSize_dec %d, iIndex_sharedObj %d"), uiSize_enc, sizeof(TRANSFER_VIDEO_DATA),
					pPkt->memory.uiBufSize, pPkt->uiSize_dec,
					pCapProcInfo.iIndex_sharedObj);
#endif
				break;
			}
			//  注意：这个初始化非常重要，涉及压缩数据有效性				
			TRANSFER_VIDEO_DATA* pReq = (TRANSFER_VIDEO_DATA*)((byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec);
			mymemset((IntPtr)pReq, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));

			//
#if __DEBUG__
			if (pSharedObj.bSlave)
			{
				int i;
				i = 0;
			}
			//
			if (0)
			{
				traceLog(_T("uiSize_enc %d,sizeof( TRANSFER_VIDEO_DATA ) %d. uiBufSize %d, uiSize_dec %d, iIndex_sharedObj %d"), uiSize_enc, sizeof(TRANSFER_VIDEO_DATA),
					pPkt->memory.uiBufSize, pPkt->uiSize_dec,
					pCapProcInfo.iIndex_sharedObj);


			}
#endif

			//
			SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
			if (null==pSharedObjUsr)
			{
#if __DEBUG__
				traceLog(_T("mcThread_toShareBmp: pSharedObjUsr is null"));
#endif
				continue;
			}

			//
#if true
						uint	dwTickCnt  =  myGetTickCount(  null);
						pPkt->debugInfo.usToEncInMs  =  (ushort)(dwTickCnt  -  pPkt->debugInfo.dwTickCnt_start);
#endif

			//  2016/02/12
			if (bPassThrough)
			{
				//  2016/02/29
				if (pSharedObj.var.uiType != Consts.CONST_sharedObjType_qvcf)
				{
					if (!pSharedObjUsr.bIn3DConf)
					{
						showInfo_open(0, null, ("doEncode_default failed, sharedObjUsr->bIn3DConf changed to false. not matched with bPassThrough"));
						goto errLabel;
					}
				}
			}

			//
#if __DEBUG__
			if (pSharedObj.bSlave)
			{
				int ii;
				ii = 0;
			}
#endif

			//
			//  2015/05/23. 对3d会议的本地流来说，不需要压缩，只用其无压缩视频，所以不需要执行压缩
			//
			if (!bPassThrough)
			{
				doEncVideoPkt(pProcInfo, pSharedObj, pCapProcInfo, pPkt);//  
			}


#if true
						dwTickCnt  =  myGetTickCount( null );
							pPkt->debugInfo.usEncedInMs  =  (ushort)(dwTickCnt  -  pPkt->debugInfo.dwTickCnt_start);
						//  2012/01/06
						bool  ucbFpsUnmatched  =  false;
						if  (  pCapProcInfo.compressVideo.usFrames_perSecond_expected  >=  pCapProcInfo.compressVideo.input.usFps  +  5  )  {		//  如果设置29.但是输入是24,那么也需要警告提示,所以是 >=
							ucbFpsUnmatched  =  true;							
						}
						if  (  pSharedObj.var.status.ucbFpsUnmatched  !=  ucbFpsUnmatched  )  pSharedObj.var.status.ucbFpsUnmatched  =  ucbFpsUnmatched;
#endif

			//
			bool bPktRedirected = false;
			if (0==q2PostMsgAndTrigger((IntPtr)pPkt, (uint)(1 * sizeof(myDRAW_VIDEO_DATA)), pCapProcInfo.postThread.q2))
			{
				bPktRedirected = true;
			}
			if (bPktRedirected)
			{
				detachVPkt(pPkt, null);
			}
			else
			{
				clean_myDRAW_VIDEO_DATA((IntPtr)pCapProcInfo.compressVideo.pCurPkt, (""));
			}


			//
#if __DEBUG__
			if (!bPktRedirected)
			{
				M_pkts_showNo(pkts, nPkts, _T("encVideo free"));
			}
#endif


			//
			continue;
		}



		iErr  =  0;
errLabel:

	return  iErr;
}

	public static unsafe int doEncodeVideo(CCtxQmc pProcInfo, CAP_procInfo_bmpCommon pCapProcInfo, MIS_CNT pMisCnt, MIS_MSGU* pMsgBuf)
		{
			int iErr = -1;
			//MC_VAR_isCli	*	pProcInfo		=	QY_GET_procInfo_isCli(  );
			ref COMPRESS_VIDEO pCompressVideo = ref pCapProcInfo.compressVideo;

			//  2015/05/23
			int iIndex_sharedObj = pCapProcInfo.iIndex_sharedObj;
			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) return -1;

			SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
			if (null==pSharedObjUsr)
			{
#if __DEBUG__
				traceLog(_T("doEncodeVideo: pSharedObjUsr is null"));
#endif
				return -1;
			}
			bool bPathThrough = false;
			if (pSharedObjUsr.bIn3DConf)
			{  //  2015/05/23. 调用此函数只是为了绕过压缩. 在doEncodeVideo_default里也有判断
				bPathThrough = true;
			}
			if (pSharedObj.var.uiType == Consts.CONST_sharedObjType_qvcf)
			{   //  2015/05/23. 本地播放，不需要压缩
				bPathThrough = true;
			}
			if (bPathThrough)
			{
				doEncodeVideo_default(pProcInfo, pCapProcInfo, pMisCnt, pMsgBuf, true);
				return 0;
			}




			//
			switch (pCompressVideo.uiModuleType_toEnc)
			{
				case Consts.CONST_moduleType_mediaCodec:
					return GuiShare.pf_doEncodeVideo_mediaCodec(pCapProcInfo, pMisCnt, (IntPtr)pMsgBuf);
					break;
				default:
					break;
			}


			//	
			doEncodeVideo_default(pProcInfo, pCapProcInfo, pMisCnt, pMsgBuf, false);

			iErr = 0;
		errLabel:
			return iErr;

		}





		//
#if true
		public static unsafe void mcThreadProc_encVideo(object lpParameter)
		{
			int iIndex_sharedObj = (int)lpParameter;

			CCtxQyMc pQyMc = g.g_pQyMc;
			if (null==pQyMc)
			{
				return;
			}
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //  (  MC_VAR_isCli  *  )pSci->pVar;
			if (null==pProcInfo) return;

			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) return;

			CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
			if (null==pCapProcInfo) return;

			//  2010/12/28
			pCapProcInfo.compressVideo.debugInfo.nStep = 101;

			//
			uint dwThreadId = myGetCurrentThreadId();

			//myDRAW_VIDEO_DATA pkts[CONST_pkts_encVideo];
			TMP_buf_myDRAW_VIDEO_DATA_pkts_encVideo buf_pkts;
			//uint uiLens[CONST_pkts_encVideo];      //  
			uint[] uiLens = new uint[Consts.CONST_pkts_encVideo];
			int nLens = uiLens.Length;// mycountof(uiLens);      //  一次最多处理的包数。

			int i;
			int nPkts;                      //								pPkts[]中有几个包是有效的。 2011/07/15

			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null==pMisCnt) return;

			using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc()) {
				MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(sizeof(MIS_MSGU));
				if (null==pMsgBuf) return;


				uint dwTickCnt;

				//  2010/09/18
				//  int							tmpiRet;
				bool tmpbErr;


				//
				//traceLogA("mcThreadProc_toShareBmp %S enters", pCapProcInfo.thread.q2.cfg.name);

				pCapProcInfo.thread.bQuit = false;
				pCapProcInfo.thread.bRunning = true;


				//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "%s, %I64u starts"  ),  pPlay->q2.cfg.name,  pPlayer->idInfo_recorder.ui64Id  );


				/////////////////

				for (; !pCapProcInfo.thread.bQuit;)
				{

					//  2010/12/28
					pCapProcInfo.compressVideo.debugInfo.nStep = 105;

					//
					int toInMs = 5000;
					toInMs = 33;
					myWaitOne(pCapProcInfo.thread.q2.hEvent0, toInMs);
					//
					dwTickCnt = myGetTickCount(null);


					//  2015/10/11
					if (!pCapProcInfo.compressVideo.bVideoCompressInited)
					{
						//
						traceLog("thread_encV: bCompressInited is false, continue");
						//showInfo_open(0,null,("thread_encV: bCompressInited is false, continue"));
						//
						continue;
					}
											
					//					
					doEncodeVideo(pProcInfo, pCapProcInfo, pMisCnt, pMsgBuf);

					//					
					continue;
				}

			}

			traceLogA("mcThreadProc_toShareBmp will quit");

		errLabel:

			//
			clean_myDRAW_VIDEO_DATA((IntPtr)pCapProcInfo.compressVideo.pCurPkt, (""));

			//  2010/12/28
			pCapProcInfo.compressVideo.debugInfo.nStep = 199;

			//
			pCapProcInfo.thread.bRunning = false;


			//traceLogA("mcThreadProc_toShareBmp %S leaves", pCapProcInfo.thread.q2.cfg.name);

			return;

		}
#endif



	}

#endif

}
