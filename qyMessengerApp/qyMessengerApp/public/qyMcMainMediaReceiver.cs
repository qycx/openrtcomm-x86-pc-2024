using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int old_mySendAvResp(ref CTX_caller ctxCaller,CCtxQmc pProcInfo, MIS_MSG_TASK* pMsg, CMIS_MSGU pMsgBuf)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null == pMisCnt) return -1;

			IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->data.buf;
			int lenInBytes;
			ushort usRespCode = Consts.CONST_qyRc_unknown;
			//TCHAR tBuf[128];
			int i;

			//  2017/09/04
			if (pMsg->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
			{
				return 0;
			}

			//
			switch (pContent->uiType)
			{
				case Consts.CONST_imCommType_transferVideoData:
					{
						TRANSFER_VIDEO_dataMemHead* pContent_transferVideoData_mems = (TRANSFER_VIDEO_dataMemHead*)pContent->transferVideoData.buf_TRANSFER_VIDEO_dataMemHead_mems;
						for (i = 0; i < pContent->transferVideoData.usCnt; i++)
						{
							TRANSFER_VIDEO_dataMemHead* pMem = &pContent_transferVideoData_mems[i];
							if (0 == pMem->stat.tSelectTime) continue;
							//				  
							{

								MSG_ROUTE route;
								byte ucFlg = Consts.CONST_commFlg_resp;
								TRANSFER_VIDEO_dataResp resp;

								mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
								//memcpy(&route.debugInfo, &pMsg->data.route.debugInfo, sizeof(route.debugInfo));
								route.debugInfo = pMsg->data.route.debugInfo;

								//
								ucFlg |= Consts.CONST_commFlg_routeTalkData;

								mymemset((IntPtr)(byte*)&resp, 0, sizeof(TRANSFER_VIDEO_dataResp));
								resp.uiType = Consts.CONST_imCommType_transferVideoDataResp;
								//
								resp.idInfo.ui64Id = pMem->idInfo.ui64Id;
								resp.uiTranNo_openAvDev = pMem->uiTranNo_openAvDev;
								if (0 == resp.uiTranNo_openAvDev)
								{
									resp.uiTranNo_openAvDev = pContent->transferVideoData.uiTranNo_openAvDev;
								}
								//
								resp.usFps = (ushort)pContent->transferVideoData.tmpInternal.result.fFps;
								resp.tSelectTime = pContent->transferVideoData.tmpInternal.result.tSelectTime;
								resp.usCnt_pkts = pContent->transferVideoData.tmpInternal.result.usCnt_pkts;
								resp.usDiff_pkts = pContent->transferVideoData.tmpInternal.result.usDiff_pkts;

								//
								lenInBytes = sizeof(TRANSFER_VIDEO_dataResp);

								usRespCode = pContent->transferVideoData.tmpInternal.result.usRespCode;

								if (0!=postMsg2Mgr_mc(ctxCaller,pMisCnt, ref route, Consts.CONST_misMsgType_outputTask, ucFlg, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, (byte*)&resp, (uint)lenInBytes, pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->data.route.idInfo_from.ui64Id, pMsg->uiChannelType, pMsgBuf, false)) goto errLabel;

								//  2017/09/05
#if __DEBUG__
#endif

							}
						}
					}
					break;
				case Consts.CONST_imCommType_transferAudioData:
					if (0 != pContent->transferAudioData.tLastTime_showFrameInfo_020534)
					{

						MSG_ROUTE route;
						byte ucFlg = Consts.CONST_commFlg_resp;
						TRANSFER_AUDIO_dataResp resp;

						mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
						//memcpy(  &route.debugInfo,  &pMsg->data.route.debugInfo,  sizeof(route.debugInfo  )  );
						route.debugInfo = pMsg->data.route.debugInfo;

						ucFlg |= Consts.CONST_commFlg_routeTalkData;

						mymemset((IntPtr)(byte*)&resp, 0, sizeof(TRANSFER_AUDIO_dataResp));
						resp.uiType = Consts.CONST_imCommType_transferAudioDataResp;
						resp.uiTranNo_openAvDev = pContent->transferAudioData.uiTranNo_openAvDev;
						resp.usCnt_pkts = pContent->transferAudioData.tmpInternal.result.usCnt_pkts;

						lenInBytes = sizeof(TRANSFER_AUDIO_dataResp);

						usRespCode = pContent->transferAudioData.tmpInternal.result.usRespCode;

						if (0!=postMsg2Mgr_mc(ctxCaller, pMisCnt, ref route, Consts.CONST_misMsgType_outputTask, ucFlg, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, (byte*)&resp, (uint)lenInBytes, pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->data.route.idInfo_from.ui64Id, pMsg->uiChannelType, pMsgBuf, false)) goto errLabel;

					}
					break;
				case Consts.CONST_imCommType_transferGpsData:       //  2012/04/24
					/*
				  if  (pContent->transferGpsData.tLastTime_showFrameInfo_020534  )  {

					  MSG_ROUTE route;
	unsigned char ucFlg = CONST_commFlg_resp;
	TRANSFER_GPS_dataResp resp;
	unsigned short usRespCode;

	memset(  &route,  0,  sizeof(route  )  );
	memcpy(  &route.debugInfo,  &pMsg->data.route.debugInfo,  sizeof(route.debugInfo  )  );

	ucFlg  |=  CONST_commFlg_routeTalkData;
										   
					  memset(  &resp,  0,  sizeof(resp  )  );
	resp.uiType  =  CONST_imCommType_transferGpsDataResp;
							   
					  lenInBytes  =  sizeof(resp  );
				
					  usRespCode  =  CONST_qyRc_ok;

					  if  (postMsg2Mgr_mc(pMisCnt,  &route,  CONST_misMsgType_outputTask,  ucFlg,  usRespCode, pMsg->tStartTime,  pMsg->uiTranNo,  0,  (char*  )&resp,  lenInBytes,  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_from,  pMsg->uiChannelType,  pMsgBuf,  false  )  )  goto  errLabel;

				  }
					*/
					break;

				default:
					break;
			}

			iErr = 0;
		errLabel:
			return iErr;
		}

		//
		public static unsafe int mySendAvResp(ref CTX_caller ctxCaller,CCtxQmc pProcInfo, MIS_MSG_INPUT* pMsg, CMIS_MSGU pMsgBuf)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null == pMisCnt) return -1;

			IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->data.buf;
			int lenInBytes;
			ushort usRespCode = Consts.CONST_qyRc_unknown;
			//TCHAR tBuf[128];
			int i;

			//  2017/09/04
			if (pMsg->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
			{
				return 0;
			}

			//
			switch (pContent->uiType)
			{
				case Consts.CONST_imCommType_transferVideoData:
					{
						TRANSFER_VIDEO_dataMemHead* pContent_transferVideoData_mems = (TRANSFER_VIDEO_dataMemHead*)pContent->transferVideoData.buf_TRANSFER_VIDEO_dataMemHead_mems;
						for (i = 0; i < pContent->transferVideoData.usCnt; i++)
						{
							TRANSFER_VIDEO_dataMemHead* pMem = &pContent_transferVideoData_mems[i];
							if (0 == pMem->stat.tSelectTime) continue;
							//				  
							{

								MSG_ROUTE route;
								byte ucFlg = Consts.CONST_commFlg_resp;
								TRANSFER_VIDEO_dataResp resp;

								mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
								//memcpy(&route.debugInfo, &pMsg->data.route.debugInfo, sizeof(route.debugInfo));
								route.debugInfo = pMsg->data.route.debugInfo;

								//
								ucFlg |= Consts.CONST_commFlg_routeTalkData;

								mymemset((IntPtr)(byte*)&resp, 0, sizeof(TRANSFER_VIDEO_dataResp));
								resp.uiType = Consts.CONST_imCommType_transferVideoDataResp;
								//
								resp.idInfo.ui64Id = pMem->idInfo.ui64Id;
								resp.uiTranNo_openAvDev = pMem->uiTranNo_openAvDev;
								if (0 == resp.uiTranNo_openAvDev)
								{
									resp.uiTranNo_openAvDev = pContent->transferVideoData.uiTranNo_openAvDev;
								}
								//
								resp.usFps = (ushort)pContent->transferVideoData.tmpInternal.result.fFps;
								resp.tSelectTime = pContent->transferVideoData.tmpInternal.result.tSelectTime;
								resp.usCnt_pkts = pContent->transferVideoData.tmpInternal.result.usCnt_pkts;
								resp.usDiff_pkts = pContent->transferVideoData.tmpInternal.result.usDiff_pkts;

								//
								lenInBytes = sizeof(TRANSFER_VIDEO_dataResp);

								usRespCode = pContent->transferVideoData.tmpInternal.result.usRespCode;

								if (0 != postMsg2Mgr_mc(ctxCaller, pMisCnt, ref route, Consts.CONST_misMsgType_outputTask, ucFlg, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, (byte*)&resp, (uint)lenInBytes, pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->data.route.idInfo_from.ui64Id, pMsg->uiChannelType, pMsgBuf, false)) goto errLabel;

								//  2017/09/05
#if __DEBUG__
#endif

							}
						}
					}
					break;
				case Consts.CONST_imCommType_transferAudioData:
					if (0 != pContent->transferAudioData.tLastTime_showFrameInfo_020534)
					{

						MSG_ROUTE route;
						byte ucFlg = Consts.CONST_commFlg_resp;
						TRANSFER_AUDIO_dataResp resp;

						mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
						//memcpy(  &route.debugInfo,  &pMsg->data.route.debugInfo,  sizeof(route.debugInfo  )  );
						route.debugInfo = pMsg->data.route.debugInfo;

						ucFlg |= Consts.CONST_commFlg_routeTalkData;

						mymemset((IntPtr)(byte*)&resp, 0, sizeof(TRANSFER_AUDIO_dataResp));
						resp.uiType = Consts.CONST_imCommType_transferAudioDataResp;
						resp.uiTranNo_openAvDev = pContent->transferAudioData.uiTranNo_openAvDev;
						resp.usCnt_pkts = pContent->transferAudioData.tmpInternal.result.usCnt_pkts;

						lenInBytes = sizeof(TRANSFER_AUDIO_dataResp);

						usRespCode = pContent->transferAudioData.tmpInternal.result.usRespCode;

						if (0 != postMsg2Mgr_mc(ctxCaller,pMisCnt, ref route, Consts.CONST_misMsgType_outputTask, ucFlg, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, (byte*)&resp, (uint)lenInBytes, pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->data.route.idInfo_from.ui64Id, pMsg->uiChannelType, pMsgBuf, false)) goto errLabel;

					}
					break;
				case Consts.CONST_imCommType_transferGpsData:       //  2012/04/24
					/*
				  if  (pContent->transferGpsData.tLastTime_showFrameInfo_020534  )  {

					  MSG_ROUTE route;
	unsigned char ucFlg = CONST_commFlg_resp;
	TRANSFER_GPS_dataResp resp;
	unsigned short usRespCode;

	memset(  &route,  0,  sizeof(route  )  );
	memcpy(  &route.debugInfo,  &pMsg->data.route.debugInfo,  sizeof(route.debugInfo  )  );

	ucFlg  |=  CONST_commFlg_routeTalkData;
										   
					  memset(  &resp,  0,  sizeof(resp  )  );
	resp.uiType  =  CONST_imCommType_transferGpsDataResp;
							   
					  lenInBytes  =  sizeof(resp  );
				
					  usRespCode  =  CONST_qyRc_ok;

					  if  (postMsg2Mgr_mc(pMisCnt,  &route,  CONST_misMsgType_outputTask,  ucFlg,  usRespCode, pMsg->tStartTime,  pMsg->uiTranNo,  0,  (char*  )&resp,  lenInBytes,  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_from,  pMsg->uiChannelType,  pMsgBuf,  false  )  )  goto  errLabel;

				  }
					*/
					break;

				default:
					break;
			}

			iErr = 0;
		errLabel:
			return iErr;
		}


		//
		public static unsafe int playAudioData(CCtxQmc pProcInfo, int iSampleTimeInMs, uint uiPts, IntPtr pInput, uint inputLen, int iIndex_player)
        {
			if (pInput == null || inputLen == 0) return -1;
			byte[] tmp_pInput = new byte[inputLen];
			System.Runtime.InteropServices.Marshal.Copy(pInput, tmp_pInput, 0, (int)inputLen);
			return playAudioData(pProcInfo, iSampleTimeInMs, uiPts, tmp_pInput, inputLen, iIndex_player);

        }

		//
		public static unsafe int playAudioData(CCtxQmc pProcInfo, int iSampleTimeInMs, uint uiPts, byte[] pInput, uint inputLen, int iIndex_player)
		{
			int iErr = -1;
			CCtxQyMc  pQyMc = g.g_pQyMc;

			//
			if (pInput == null) return -1;
			if (inputLen == 0) return -1;
			//if (pInput.Length != inputLen) return -1;
			if (pInput.Length < inputLen)
			{
				qyFuncs.showInfo_open(0, "", "playAudioData failed,pInput.Length<inputLen");
				return -1;
			}


            //
            QY_PLAYER pPlayer;
			pPlayer = getPlayerByIndex(pProcInfo, iIndex_player);
			if (null == pPlayer) return -1;

			if (inputLen > pProcInfo.cfg.uiBufSize_myPlayAudioData)
			{
				goto errLabel;
			}

			//
			if (pProcInfo.cfg.b_useQ2_toPlayA)
			{

				//CQyMalloc				mallocObj_pkt;
				using (CQyMalloc mallocObj = new CQyMalloc())
				{
					;
					myPLAY_AUDIO_DATA pkt;  //  2015/02/14
					myPLAY_AUDIO_DATA* p;
					int len;


					len = sizeof(myPLAY_AUDIO_DATA);//,  buf  );
					p = &pkt;//  (  myPLAY_AUDIO_DATA  *  )mallocObj_pkt.mallocf(  len  );
					if (null == p) goto errLabel;
					mymemset((IntPtr)p, 0, len);//offsetof(  myPLAY_AUDIO_DATA,  buf  )  );
												//  p->uiTranNo_openAvDev  =  pPlayer.audio.uiTranNo;
												//
												//  2016/12/10
					uint uiSize_dec = inputLen;
					//  2011/12/06		
					uint uiSize_enc = 0;   //  sizeof(  TRANSFER_AUDIO_DATA  );
					uiSize_enc = 1;                 //  2017/10/14. 因为uiSize_dec可能为0， 所以这里要加一个字节，避免head.uiLen为0
													//
													//  p->head.uiLen  =  dwNumBytes;
					p->head.uiLen = uiSize_dec + uiSize_enc;    //  2016/12/10
					p->uiSize_dec = uiSize_dec;                 //  2016/12/10
																//
					p->head.uiSampleTimeInMs = (uint)iSampleTimeInMs;
					//  p->head.uiSampleTimeInMs_org  =  iSampleTimeInMs_org;
					p->head.uiPts = uiPts;                      //  2017/10/13

					//
					//
					if (null == mallocObj.mallocf((int)p->head.uiLen)) goto errLabel;
					if (0 != mallocObj.get(ref p->memory)) goto errLabel;
					//
					//mymemcpy(p->memory.m_pBuf, (IntPtr)pInput, p->uiSize_dec);  //  p->head.uiLen  );
					System.Runtime.InteropServices.Marshal.Copy(pInput, 0, p->memory.m_pBuf, (int)p->uiSize_dec);

					//  	
					if (0 != q2PostMsg((IntPtr)p, (uint)sizeof(myPLAY_AUDIO_DATA), pPlayer.audio.q2))
					{
						//  if  (  !pQyMc.bUseDirectSound_toPlay  )  			
						if (pQyMc.uiAPlayType == Consts.CONST_aPlayType_wave)
						{
							//PostThreadMessage(pPlayer.audio.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
						}
						goto errLabel;
					}
					mallocObj.detach();

					//  2010/05/03
					//  if  (  !pQyMc.bUseDirectSound_toPlay  )  		
					if (pQyMc.uiAPlayType == Consts.CONST_aPlayType_wave)
					{
						//PostThreadMessage(pPlayer.audio.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
					}
				}
			}
			else
			{
				QY_qBuf2.HeadData_playA headData;
				qyFuncs.mymemset((IntPtr)(byte*)&headData, 0, sizeof(QY_qBuf2.HeadData_playA));
				headData.uiPts = uiPts;
				headData.uiSize_dec = inputLen;
				if (0 != pPlayer.audio.qBuf2.q2PostMsg((IntPtr)(byte*)&headData, pInput, inputLen))
                {
					goto errLabel;
                }

            }

			iErr = 0;
		errLabel:

			//p->memory.m_pBuf  =  null;		//  shall be reset

			return iErr;

		}



		//
		public static unsafe int playLocalAudioData(CCtxQmc pProcInfo, int iTaskId, uint uiTranNo, ref WAVEFORMATEX wf, int iSampleTimeInMs, byte* pInput, uint inputLen, uint uiCapType, int iIndex_capAudio)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;

			fixed (WAVEFORMATEX* pWf = &wf) {


				if (null==pWf) return -1;
				if (0==uiTranNo)
				{
					traceLogA("playLocalAudioData failed, uiTranNo is 0");
					return -1;
				}

				QY_SHARED_OBJ pSharedObj = null;
				CAP_procInfo_bmpCommon pCapBmp = null;
				CAP_procInfo_audioCommon pCapAudio = null;
				QY_PLAYER pPlayer = null;

				pCapAudio = getCapAudioBySth(pProcInfo, iIndex_capAudio, 0);
				if (null==pCapAudio) goto errLabel;
				pSharedObj = getSharedObjByIndex(pProcInfo, pCapAudio.iIndex_sharedObj);
				if (null==pSharedObj) goto errLabel;
				pCapBmp = getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);

				SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
				if (null==pSharedObjUsr) goto errLabel;

				//
				pPlayer = getPlayerByIndex(pProcInfo, pSharedObj.var.iIndex_localPlayer);
				if (null!=pPlayer)
				{
					if (pPlayer.iTaskId != iTaskId
						|| pPlayer.idInfo_recorder.ui64Id != 0
						|| pPlayer.audio.uiTranNo != uiTranNo)
					{
						traceLogA("drawLocalVideoData: not correct player");
						pSharedObj.var.iIndex_localPlayer = -1;
						pPlayer = null;
					}
				}

				if (null==pPlayer)
				{
					int k;

					for (k = 0; k < pProcInfo.av.usCnt_players; k++)
					{

						//  2015/05/22
						if (pProcInfo.av.pPlayers[k].iTaskId != iTaskId) continue;     //  2010/09/07

						//
						if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == 0
							 && pProcInfo.av.pPlayers[k].audio.uiTranNo == uiTranNo)
						{
							if (null==pCapBmp) break;

							//  2010/09/04
							if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == 0
								&& pProcInfo.av.pPlayers[k].video.uiTranNo == pSharedObjUsr.uiTranNo_openAvDev_v)
							{
								break;
							}

						}
					}

					if (k < pProcInfo.av.usCnt_players)
					{
						pSharedObj.var.iIndex_localPlayer = k;
						pPlayer = getPlayerByIndex(pProcInfo, k);
						if (null==pPlayer) goto errLabel;
					}
				}

				//  
				if (null==pPlayer || !pPlayer.audio.bRunning)
				{


					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playLocalAudioData : me,%d,%d apply for player"), uiTranNo, 0);

					//  ÕâÀï×é°ü·¢¸øÖ÷Ïß³Ì¡£
					MIS_MSG_applyForPlayer_qmc msg;
					mymemset((IntPtr)(byte*)&msg, 0, sizeof(MIS_MSG_applyForPlayer_qmc));
					msg.uiType = Consts.CONST_misMsgType_applyForPlayer_qmc;
					msg.iTaskId = iTaskId;
					//  msg.idInfo_recorder.ui64Id  =  0;
					msg.audio.uiTranNo = uiTranNo;
					//  msg.audio.usIndex  =  0;
					//mymemcpy(&msg.audio.ah, pWf, Math.Min(sizeof(msg.audio.ah), sizeof(WAVEFORMATEX) + pWf->cbSize));
					msg.audio.ah.wfx = *pWf;
					//
					//memcpy(&msg.audio.playCfg, &pCapAudio.compressAudio.compressor.playCfg, sizeof(msg.audio.playCfg)); //  2012/01/04
					msg.audio.playCfg = pCapAudio.compressAudio.compressor.playCfg;

					//
					if (null!=pCapBmp)
					{
						//  msg.video.uiTranNo  =  pCapBmp->common.compressVideo.uiTranNo_openAvDev;
						msg.video.uiTranNo = pSharedObjUsr.uiTranNo_openAvDev_v;                       //  2010/09/04
																										//  msg.video.usIndex  =  0;
																										//
																										//  memcpy(  &msg.video.vh.bih,  &pCapBmp->common.compressVideo.vh_decompress.bih,  sizeof(  msg.video.vh.bih  )  );
						if (0!=getBih_capBmpToPlay(pCapBmp, &msg.video.vh.bih)) goto errLabel;
						//
						//msg.video.ucbFlippedImg  =  pCapBmp->common.compressVideo.compressor.common.ucbOutputFlippedImg;	//  2011/12/11
						//
						msg.video.usFps = pCapBmp.compressVideo.compressor.common.head.usMaxFps_toShareBmp;      //  2015/02/20
					}

					//					  
					qPostMsg((IntPtr)(byte*)&msg, (uint)sizeof(MIS_MSG_applyForPlayer_qmc), pQyMc.gui.processQ);

					GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
					iErr = 0; goto errLabel;
				}

				if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playLocalAudioData : me, %d,%d plays"), uiTranNo, 0);
				}

				{
					using (CQyMalloc mallocObj = new CQyMalloc())
					{
						;
						myPLAY_AUDIO_DATA pkt;
						myPLAY_AUDIO_DATA* pPkt = &pkt;         //  =	(  myPLAY_AUDIO_DATA  *  )mallocObj.malloc(  offsetof(  myPLAY_AUDIO_DATA,  buf  )  +  len  );
						if (null == pPkt) goto errLabel;
						myPLAY_AUDIO_DATA* p = pPkt;

						if (inputLen > pProcInfo.cfg.uiBufSize_myPlayAudioData)
						{
							goto errLabel;
						}

						//
						mymemset((IntPtr)pPkt, 0, sizeof(myPLAY_AUDIO_DATA));
						pPkt->head.idInfo.ui64Id = 0;
						pPkt->head.uiSampleTimeInMs = (uint)iSampleTimeInMs;

						//
						uint uiSize_dec = inputLen;
						uint uiSize_enc = 0;
						//
						pPkt->head.uiLen = uiSize_dec + uiSize_enc;
						pPkt->uiSize_dec = uiSize_dec;
						//
						if (null == mallocObj.mallocf((int)p->head.uiLen)) goto errLabel;
						if (0!=mallocObj.get(ref p->memory)) goto errLabel;
						mymemcpy(p->memory.m_pBuf, (IntPtr)pInput, p->uiSize_dec);  //  p->head.uiLen  );



						//
						/*
						if  (  pPkt->head.uiSampleTimeInMs  <  pPlayer.audio.m_var.uiSampleTimeInMs  )  {		//  move backward the media file. need to clear up the old media data
							emptyQ2(  &pPlayer.audio.q2  );
							pPlayer.audio.m_var.uiSampleTimeInMs  =  0;
						}
						if  (  pPkt->head.uiSampleTimeInMs  >  pPlayer.audio.m_var.uiSampleTimeInMs_expectedToRecv  +  3000  )  {		//  move forward the media file. need to clear up the old media data
							traceLogA(  "pPkt->uiSampleTimeInMs %d, expected %d.",  pPkt->head.uiSampleTimeInMs,  pPlayer.audio.m_var.uiSampleTimeInMs_expectedToRecv  );
							emptyQ2(  &pPlayer.video.q2  );
							pPlayer.video.m_var.uiSampleTimeInMs_newRecvd  =  0;
							emptyQ2(  &pPlayer.audio.q2  );
							pPlayer.audio.m_var.uiSampleTimeInMs  =  0;
						}
						//
						if  (  pPkt->head.uiSampleTimeInMs  )  {
							if  (  pPlayer.audio.waveFormatEx.nAvgBytesPerSec  )  {
								pPlayer.audio.m_var.uiSampleTimeInMs_expectedToRecv  =  (  uint  )(  pPkt->head.uiSampleTimeInMs  +  pPkt->head.uiLen  *  1000.  /  pPlayer.audio.waveFormatEx.nAvgBytesPerSec  );
							}
						}
						*/

						uint dwTickCnt_now = myGetTickCount(null);

						//  2009/04/06
						pPlayer.audio.m_var.uiSampleTimeInMs_newRecvd = pPkt->head.uiSampleTimeInMs;
						pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd = dwTickCnt_now;

						if (0 != pPkt->head.uiSampleTimeInMs && getQ2Nodes_toGetMsg(pPlayer.audio.q2) > 1)
						{
							int iAbs = Math.Abs((int)(pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd - pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv)); //  The iAbs is needed because uiSampleTimeInMs may be a future value.
							if (pPlayer.audio.m_var.uiSampleTimeInMs_newRecvd + iAbs < pPlayer.audio.m_var.uiSampleTimeInMs)
							{           //  move backward the media file. need to clear up the old media data
										//traceLogA("pkt.uiSampleTimeInMs %d + iAbs %d < player.%d. trigger to empty old data", pPkt->head.uiSampleTimeInMs, iAbs, pPlayer.audio.m_var.uiSampleTimeInMs);
										//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playAudioData: pkt.uiSampleTimeInMs %d + abs %d < player.%d. trigger to empty old data"), pPlayer.audio.m_var.uiSampleTimeInMs_newRecvd, iAbs, pPlayer.audio.m_var.uiSampleTimeInMs);

								q2TriggerToProcess(pPlayer.audio.q2);
							}
							if (0 != pPlayer.audio.m_var.uiSampleTimeInMs)
							{
								if (pPkt->head.uiSampleTimeInMs > pPlayer.audio.m_var.uiSampleTimeInMs + 10000)
								{   //  2009/05/25. too old audio data
									//traceLogA("pkt.uiSampleTimeInMs %d > player.%d  +  10000. trigger to empty old data", pPkt->head.uiSampleTimeInMs, pPlayer.audio.m_var.uiSampleTimeInMs);
									q2TriggerToProcess(pPlayer.audio.q2);
								}
							}
							//  2012/01/26
							if (0 != pPlayer.audio.playCfg.usMaxTimeInMsToPlay)
							{   //  2009/06/03
								//TCHAR tBuf[256] = _T("");
								int nWaitingInMs_audio = (int)MACRO_getnWaitingInMs_audio_syncRecv(pPlayer.audio, dwTickCnt_now);
								if (nWaitingInMs_audio > 0 && nWaitingInMs_audio > pPlayer.audio.playCfg.usMaxTimeInMsToPlay + pPlayer.audio.playCfg.usMinTimeInMsOfAudioDataPlaying)
								{

#if true
									//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "chkFromPlayerIndex_a: nWaitingInMs_audio %d > uiMaxTimeToPlay %d + playing %d. trigger to empty old data"  ),  nWaitingInMs_audio,  pPlayer.audio.playCfg.usMaxTimeInMsToPlay,  pPlayer.audio.playCfg.usMinTimeInMsOfAudioDataPlaying  );
#if __DEBUG__
								traceLogA(  "%S",  tBuf  );
#endif
									//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s"), tBuf);
#endif

#if __DEBUG__
#endif

									q2TriggerToProcess(pPlayer.audio.q2);
								}
							}
						}

						//
						if (0 != q2PostMsg((IntPtr)pPkt, (uint)sizeof(myPLAY_AUDIO_DATA), pPlayer.audio.q2))
						{
							//  if  (  !pQyMc.bUseDirectSound_toPlay  ) 				
							if (pQyMc.uiAPlayType == Consts.CONST_aPlayType_wave)  //  2017/02/16 				
							{
								//PostThreadMessage(pPlayer.audio.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
							}
							goto errLabel;
						}
						mallocObj.detach();
					}
				}

				//  2010/05/03
				//  if  (  !pQyMc.bUseDirectSound_toPlay  ) 		
				if (pQyMc.uiAPlayType == Consts.CONST_aPlayType_wave)
				{
					//PostThreadMessage(pPlayer.audio.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
				}

			}

			iErr = 0;
		errLabel:
			return iErr;
		}

		//
		public static unsafe int chkFromPlayerIndex_a(uint uiTransformType, QY_TRANSFORM pQY_TRANSFORM, myPLAY_AUDIO_DATA* p, void* p2)
		{
			int iErr = -1;

			//
			if (uiTransformType != Consts.CONST_uiTransformType_dec)
			{
				return -1;
			}

			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;
			if (null==pTransform) return -1;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();// (MC_VAR_isCli*)pTransform.pProcInfo;
			if (null==pProcInfo) return -1;
			int iTaskId = pTransform.iTaskId;

			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return -1;

			//
			fixed (WAVEFORMATEX* pWf = &pTransform.audio.ta.ah_decompress.wfx) {;

				MIS_CNT pMisCnt = pProcInfo.pMisCnt;//->getMisCntByName(_T(""));
				if (null == pMisCnt) return -1;

				QY_MESSENGER_ID idInfo_recorder;
				uint uiTranNo = 0;
				ushort usIndex = 0;
				int iPos = 0;
				//TCHAR tBuf[256] = _T("");


				if (null==pWf) return -1;

				/*
				//  idInfo_recorder.ui64Id  =  pReq->mems[i].idInfo.ui64Id  ?  pReq->mems[i].idInfo.ui64Id  :  pIdInfo_from->ui64Id;
				if  (  pReq->mems[i].idInfo.ui64Id  )  {
					idInfo_recorder.ui64Id  =  pReq->mems[i].idInfo.ui64Id;

					 if  (  idInfo_recorder.ui64Id  ==  pIdInfo_from->ui64Id  )  uiTranNo  =  pReq->uiTranNo_openAvDev;
					 else  uiTranNo  =  0;
					}
				else  {
					   idInfo_recorder.ui64Id  =  pIdInfo_from->ui64Id;
					   uiTranNo  =  pReq->uiTranNo_openAvDev;
				}
				*/
				idInfo_recorder.ui64Id = p->head.idInfo.ui64Id;
				uiTranNo = p->head.uiTranNo_openAvDev;      //  2016/12/16   
				if (0==uiTranNo) uiTranNo = p->head.tmpInternal.uiTranNo_org;     //  2016/12/16

				//  skip myself
				if (idInfo_recorder.ui64Id == pMisCnt.idInfo.ui64Id)
				{
					//
# if  __DEBUG__  //  2017/08/16
					traceLog(_T("chkFromPlayerIndex_a: idInfo_recorder is myslef, skipped"));
#endif
					//
					//  iErr  =  0;  
					goto errLabel;
				}
				//  2015/05/19
				if (pTransform.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
				{
					if (!pProcInfo.cfg.ucbPlayRemoteAudio_viewDynBmp)
					{
						//  skipped
						goto errLabel;
					}
				}


				//
				//


#if true       //  2016/02/15
				//PROC_TASK_AV  *  pTask  =  (  PROC_TASK_AV  *  )pTransform.pTask;
				if (null==pTask) goto errLabel;


				//if (pQyMc.iCustomId != Consts.CONST_qyCustomId_hzj) 
				{

					//  2015/02/18							
					//  2010/09/02							
					bool bNoSharedObj; bNoSharedObj = false;
					if (pTask.doProxy.bExists_avProxy) bNoSharedObj = true;

					//			
					if (!bNoSharedObj) {
						QY_SHARED_OBJ pSharedObj = null;
						pSharedObj = getSharedObjByIndex(pProcInfo, pTask.iIndex_sharedObj);
						if (null!=pSharedObj) {
							SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
							if (null!=pSharedObjUsr) {
								if (pSharedObjUsr.iTaskId != pTransform.iTaskId) {
# if  __DEBUG__
									traceLog(_T("chkFromPlayerIndex_a: sharedObjUsr isnt matched. skipped"));
#endif
									goto errLabel;
								}
							}
						}
					}

					//
				}
#endif



				//
				QY_PLAYER pPlayer;
				pPlayer = getPlayerByIndex(pProcInfo, pTransform.iIndex_player);
				if (null!=pPlayer)
				{
					if (pPlayer.iTaskId != iTaskId
						|| pPlayer.idInfo_recorder.ui64Id != idInfo_recorder.ui64Id
						|| pPlayer.audio.uiTranNo != uiTranNo)
					{
						traceLogA("drawVideoData: not correct player");
						pTransform.iIndex_player = -1;
						pPlayer = null;
					}
				}

				if (null==pPlayer)
				{
					int k;
					//  
					for (k = 0; k < pProcInfo.av.usCnt_players; k++)
					{

						if (pProcInfo.av.pPlayers[k].iTaskId != iTaskId) continue;     //  2010/09/07

						if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == idInfo_recorder.ui64Id
							&& pProcInfo.av.pPlayers[k].audio.uiTranNo == uiTranNo)
						{
							if (pTransform.video == null) break;
							if (0==pTransform.video.tv.uiTranNo_openAvDev) break;

							if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == idInfo_recorder.ui64Id
								&& pProcInfo.av.pPlayers[k].video.uiTranNo == pTransform.video.tv.uiTranNo_openAvDev)
							{
								break;
							}
						}
					}
					if (k < pProcInfo.av.usCnt_players)
					{
						pTransform.iIndex_player = k;
						pPlayer = getPlayerByIndex(pProcInfo, k);
						if (null==pPlayer) goto errLabel;
					}
				}
				//  2017/07/03
				if (null==pPlayer || null==pPlayer.audio.uiPlayType || !pPlayer.audio.bRunning)
				{


					//  ÕâÀï×é°ü·¢¸øÖ÷Ïß³Ì¡£
					MIS_MSG_applyForPlayer_qmc msg;
					mymemset((IntPtr)(byte*)&msg, 0, sizeof(MIS_MSG_applyForPlayer_qmc));
					msg.uiType = Consts.CONST_misMsgType_applyForPlayer_qmc;
					//
					msg.iTaskId = iTaskId;                              //  2009/09/17
					msg.idInfo_logicalPeer = pTransform.pAddr_logicalPeer1.idInfo;
					//
					msg.idInfo_recorder.ui64Id = idInfo_recorder.ui64Id;
					msg.audio.uiTranNo = uiTranNo;
					//memcpy(&msg.audio.ah, pWf, min(sizeof(msg.audio.ah), sizeof(WAVEFORMATEX) + pWf->cbSize));
					msg.audio.ah.wfx= *pWf;

#if false //  2011/12/25
			memcpy(  &msg.audio.playCfg,  &pTask.audio.playCfg,  sizeof(  msg.audio.playCfg  )  );	//  2009/06/14
#else
					//tmpGetMaxAudioPlayCfg(&pTask.audio.playCfg, &pTransform.audio.ta.compressor.playCfg, &msg.audio.playCfg);
#endif

					if (pTransform.video != null)
					{
						if (0 != pTransform.video.tv.uiTranNo_openAvDev)
						{
							msg.video.uiTranNo = pTransform.video.tv.uiTranNo_openAvDev;
							//memcpy(&msg.video.vh.bih, &pTransform.video.tv.vh_decompress.bih, sizeof(msg.video.vh.bih));
							msg.video.vh.bih = pTransform.video.tv.vh_decompress.bih;
							//msg.video.ucbFlippedImg  =  pTransform.video.tv.compressor.common.ucbOutputFlippedImg;		//  2011/12/11
							msg.video.usFps = pTransform.video.tv.compressor.usMaxFps_toShareBmp;                   //  2015/02/20

							//  2015/05/20
							if (0 == msg.video.vh.bih.biWidth || 0 == msg.video.vh.bih.biHeight)
							{
								if (0 == msg.idInfo_recorder.ui64Id)
								{
									string str;
									str = string.Format("chkFromPlayerIndex_a failed, idInfo 0, trans.v.tv.vh_dec.bih: {0}X{1}", msg.video.vh.bih.biWidth, msg.video.vh.bih.biHeight);
									showInfo_open(0, null, str);
									goto errLabel;
								}
							}
						}
					}

					//  2009/09/17
					//  msg.iTaskId  =
# if  __DEBUG__
					if (!msg.video.vh.bih.biWidth)
					{
						int ii;
						ii = 0;
					}
#endif

					//
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("chkFromPlayerIndex_a : %I64u,%d,%d apply for player"), idInfo_recorder.ui64Id, msg.audio.uiTranNo, msg.video.uiTranNo);

					//					  
					qPostMsg((IntPtr)(byte*)&msg, (uint)sizeof(MIS_MSG_applyForPlayer_qmc), pQyMc.gui.processQ);
					GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

					iErr = -2; goto errLabel;
				}

				if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playAudioData : %I64u plays"), idInfo_recorder.ui64Id);
				}

				TRANSFER_AUDIO_dataMemHead* pPkt;
				pPkt = &p->head;
				//

				uint dwTickCnt_now = myGetTickCount(null);

				//  2009/04/06
				pPlayer.audio.m_var.uiSampleTimeInMs_newRecvd = pPkt->uiSampleTimeInMs;
				pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd = dwTickCnt_now;

				if (0!=pPkt->uiSampleTimeInMs && getQ2Nodes_toGetMsg(pPlayer.audio.q2) > 1)
				{
					int iAbs = Math.Abs((int)(pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd - pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv)); //  The iAbs is needed because uiSampleTimeInMs may be a future value.
					if (pPlayer.audio.m_var.uiSampleTimeInMs_newRecvd + iAbs < pPlayer.audio.m_var.uiSampleTimeInMs)
					{           //  move backward the media file. need to clear up the old media data
						//traceLogA("pkt.uiSampleTimeInMs %d  +  iAbs %d < player.%d. trigger to empty old data", pPkt->uiSampleTimeInMs, iAbs, pPlayer.audio.m_var.uiSampleTimeInMs);
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playAudioData: pkt.uiSampleTimeInMs %d + abs %d < player.%d. trigger to empty old data"), pPkt->uiSampleTimeInMs, iAbs, pPlayer.audio.m_var.uiSampleTimeInMs);

						q2TriggerToProcess(pPlayer.audio.q2);
					}
					if (0!=pPlayer.audio.m_var.uiSampleTimeInMs)
					{
						if (pPkt->uiSampleTimeInMs > pPlayer.audio.m_var.uiSampleTimeInMs + 10000)
						{   //  2009/05/25. too old audio data
							/*
							_sntprintf(tBuf, mycountof(tBuf), _T("playAudioData: pkt.uiSampleTimeInMs %d > player.%d  +  10000. trigger to empty old data"), pPkt->uiSampleTimeInMs, pPlayer.audio.m_var.uiSampleTimeInMs);
							traceLogA("%S", tBuf);
							qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s"), tBuf);
							*/

							q2TriggerToProcess(pPlayer.audio.q2);
						}
					}

					//  if  (  pTask.audio.playCfg.uiMaxTimeInMsToPlay  )  
					if (0!=pPlayer.audio.playCfg.usMaxTimeInMsToPlay)
					{   //  2009/06/03
						int nWaitingInMs_audio = (int)MACRO_getnWaitingInMs_audio_syncRecv(pPlayer.audio, dwTickCnt_now);
						if (nWaitingInMs_audio > 0 && nWaitingInMs_audio > pPlayer.audio.playCfg.usMaxTimeInMsToPlay + pPlayer.audio.playCfg.usMinTimeInMsOfAudioDataPlaying)
						{


# if  __DEBUG__
							if (pPlayer.audio.playCfg.usMaxTimeInMsToPlay != pTask.audio.playCfg.usMaxTimeInMsToPlay)
							{
								traceLogA("playAudioData err: player.audio.m_var.uiMaxTimeInMsToPlay != task.audio.playCfg.*");
							}
							if (pPlayer.audio.playCfg.usMaxTimeInMsToBeRemoved != pTask.audio.playCfg.usMaxTimeInMsToBeRemoved)
							{
								traceLogA("playAudioData err: player.audio.m_var.uiMaxTimeInMsToBeRemoved != task.audio.playCfg.*");
							}
#endif

							q2TriggerToProcess(pPlayer.audio.q2);
						}
					}
				}

			}

iErr = 0;
errLabel:
return iErr;
	
}



		//
		public static unsafe int transformTaskAudioPkt(QY_TRANSFORM pTransform, TRANSFER_AUDIO_dataMemHead *pMemHead, byte * pInput1,  myPLAY_AUDIO_DATA* pPkt_dec, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;// (QY_MC*)pProcInfo.pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			bool bDbg = true;
			string str;

			//
			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			int iTaskId = pTransform.iTaskId;
			//AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			//  myPLAY_AUDIO_DATA	*	pPkt_dec			=	(  myPLAY_AUDIO_DATA  *  )&pTransform.audio.pkt_dec;
			//  MIS_MSG_TASK		*	pMsgTask_toCompress	=	(  MIS_MSG_TASK  *  )pTransform.audio.pMsgTask_toCompress_videoConference;

			//
			int index_activeMems_from = pTransform.index_activeMems_from;

			//
			CTX_caller ctxCaller = new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "transformTaskAudioPkt");
			if (null == pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (null == pTaskData) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null == pMisCnt) return -1;
			int tmpiRet = 0;
			//char timeBuf[32] = "";
			//TCHAR tHintBuf[256] = _T("");
#if false
			if (null == pReq || 0 == pReq->usCnt || pReq->usCnt > Consts.CONST_maxTransferAvDataMems) return -1;
			TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
#endif
			int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
			//if (null == pPktInfo_toDec) return -1;

			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null == pCusModules) return -1;

			//
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


			//
			//int index = index_pMems_from;
			uint curTickCnt = myGetTickCount(null);

			//TCHAR where_showInfo[32] = _T(""); _sntprintf(where_showInfo, mycountof(where_showInfo), _T("transA %d"), pTransform.index_activeMems_from);

			//if (null == pAddr) return -1;


			//
#if false
			if (pMsgTask->tStartTime < pFrom.tStartTime_task)
			{
				//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s tStartTime < tStartTime_task. skipped."), tHintBuf); 
				goto errLabel;
			}
#endif
			//

			//
#if false
			if (pTransform.audio.ucbInited)
			{
				if (pTransform.audio.index_activeMems_from != index_activeMems_from
					|| pTransform.audio.idInfo.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
					|| pTransform.audio.ta.uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev
					|| pTransform.audio.status.ucbDecFailed            //  2012/03/19
						)
				{
					//traceLogA("procTaskAudio: transform.index_pMems_from %d, uiTranNo_openAvDev %d != %d, %d", pTransform.audio.index_pMems_from, pTransform.audio.ta.uiTranNo_openAvDev, index_pMems_from, pFrom.audio.ta_recvd.uiTranNo_openAvDev);
					exitTaskAvFrom_audio(pProcInfo, pTransform, "transformTaskAudioPkt enters");
				}
			}
			else
			{
				exitTaskAvFrom_audio(pProcInfo, pTransform, "transformTaskAudioPkt enters");
			}

			//  2009/04/13
			if (!pTransform.audio.ucbInited)
			{
				if (0 != initTaskAvFrom_audio(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform)) goto errLabel;
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("procTaskAudio: initTaskAvFrom_audio %d"), index_pMems_from);
			}
#endif
			if (!pTransform.audio.ucbInited)
			{
				traceLogA("procTaskAudio failed: ³õÊ¼»¯Î´Íê³É¡£");
				goto errLabel;
			}

			//  check
#if false
			if (pMsgTask->data.route.idInfo_from.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
				|| pReq->uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
			{
				str = string.Format("transformTaskAudioPkt failed: idInfo_from {0}, audioData.uiTranNo {1} !=  pActive {2}, {3}", pMsgTask->data.route.idInfo_from.ui64Id, pReq->uiTranNo_openAvDev, pActiveMem_from.avStream.idInfo.ui64Id, pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev);
				showInfo_open(0, null, str);
				//
				goto errLabel;
			}
#endif


#if false
			if (!pTask.ucbVideoConference)
			{
				//
				pFrom.audio.usCnt_pkts++;
				if (0!=pReq->tLastTime_showFrameInfo_020534)
				{
					if (0==pFrom.audio.tLastTime_showFrameInfo_020534)
					{   //  ÕâÊÇÔÚµÚÒ»´ÎµÄÊ±ºò£¬Ã»¿ªÊ¼¼ÆÊý£¬ÏìÓ¦OK¡£
						pReq->tmpInternal.result.usRespCode = Consts.CONST_qyRc_ok;

						//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("×¼±¸Ö¡¼ÆÊý."));

						//  pFrom.audio.uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;	//  2013/08/04. 不能修改pFrom.audio.uiTranNo_openAvDev
					}
					else if (pFrom.audio.ta_recvd.uiTranNo_openAvDev == pReq->uiTranNo_openAvDev
							   && pFrom.audio.tLastTime_showFrameInfo_020534 == pReq->tLastTime_showFrameInfo_020534
								   && pFrom.audio.usCnt_pkts >= pReq->usCnt_pkts)
					{
						pReq->tmpInternal.result.usRespCode = Consts.CONST_qyRc_ok;

						//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("pkts %d, Ò»ÖÂ."), pReq->usCnt_pkts);
					}
					else
					{
						/*
						CQyString str;
						CQyString str_expected;

						pReq->tmpInternal.result.usRespCode = CONST_qyRc_err;

						getTimelStr(pFrom.audio.tLastTime_showFrameInfo_020534, timeBuf, mycountof(timeBuf)); str = CQyString(timeBuf);
						getTimelStr((time_t)pReq->tLastTime_showFrameInfo_020534, timeBuf, mycountof(timeBuf)); str_expected = CQyString(timeBuf);
						_sntprintf(tHintBuf, mycountof(tHintBuf), _T("Error: ( %d, %s, %d pkts ) != ÆÚÍû  ( %d, %s, %d pkts )."), pFrom.audio.ta_recvd.uiTranNo_openAvDev, str, pFrom.audio.usCnt_pkts, pReq->uiTranNo_openAvDev, str_expected, pReq->usCnt_pkts);
						*/

						//  pFrom.audio.uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;			
					}
					pReq->tmpInternal.result.usCnt_pkts = pFrom.audio.usCnt_pkts;  //  Òª½«·¢ËÍµÄÖ¡Êý·¢»Ø·¢ËÍ·½¡£

					pFrom.audio.usCnt_pkts = 0;
					pFrom.audio.tLastTime_showFrameInfo_020534 = pMsgTask->tStartTime;
				}

			}
#endif

			//  ÏÂÃæ¿ªÊ¼½âÑ¹Ëõ
			uint dwBytes_pcm;
			TRANSFER_AUDIO_dataMemHead* pMem;
			//int iPos;
			fixed (WAVEFORMATEX* pWfx_toPlay = &pTransform.audio.ta.ah_decompress.wfx) { }


			//  TRANSFER_AUDIO_DATA			*	pReq_toCompress;	pReq_toCompress  =  null;

			/*
			if  (  pTask.ucbVideoConferenceStarter  )  {
				if  (  !pMsgTask_toCompress  )  goto  errLabel;
				pReq_toCompress  =  (  TRANSFER_AUDIO_DATA  *  )pMsgTask_toCompress->data.buf;

				pMsgTask_toCompress->uiType  =  CONST_misMsgType_task;
				//
				memset(  pReq_toCompress,  0,  offsetof(  TRANSFER_AUDIO_DATA,  buf  )  );
				pReq_toCompress->uiType =  pReq->uiType;
				pReq_toCompress->uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;
			}
			*/

			//  memset(  pFrom.audio.pDecompressBuf_transferAudioData,  0,  offsetof(  TRANSFER_AUDIO_DATA,  mems  )  );
			//  pFrom.audio.pDecompressBuf_transferAudioData->uiType  =  pReq->uiType;
			//  pFrom.audio.pDecompressBuf_transferAudioData->uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;

#if false
			if (0 == pReq->usCnt || pReq->usCnt > Consts.CONST_maxTransferAvDataMems)
			{
#if __DEBUG__
				traceLogA("procTaskAudio failed: pReq->usCnt %d error", pReq->usCnt);
#endif
				goto errLabel;
			}
#endif


			//for  (  iPos  =  0,  i  =  0;  i  <  pReq->usCnt;  iPos  +=  pReq->mems[i].uiLen,  i  ++  )  
			for (int kk = 0; kk < 1; kk++)
			{
#if false
				if (!pPktInfo_toDec->bGot_msg) goto errLabel;
				if (!pPktInfo_toDec->info.bGot_pkt) goto errLabel;
				//
				//  2017/10/14
				iPos = pPktInfo_toDec->info.iPos;
				//if (pPktInfo_toDec->info.index_mems < 0 || pPktInfo_toDec->info.index_mems >= pReq->usCnt) goto errLabel;
				int i;
				i = pPktInfo_toDec->info.index_mems;
#endif

				//
				//pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

				//
				pMem = pMemHead;

				//
#if DEBUG
				ref TRANSFER_AUDIO_dataMemHead tmp_pMem = ref *pMem;
#endif



				//
#if false
				if (iPos + pMem->uiLen > pReq->uiLen)
				{
#if __DEBUG__
					traceLogA("procTasAudio: ");
#endif
					goto errLabel;
				}
#endif
				if (0 == pMem->uiLen)
				{
#if __DEBUG__
					traceLogA("procTaskAudio warning: mem.uiLen is 0");
#endif
					continue;
				}
				if (0 != pMem->uiTranNo_openAvDev)
				{   //  2013/08/04. 会议时,参会人如果第2次进入会议,那么需要区分前后2次的音频流是不同的,靠的就是tn
					if (pMem->uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
					{
#if __DEBUG__
						traceLog(_T("transformTaskAudio: err, mem.uiTranNo_openAvDev %d != from.audio. %d. skipped"), pMem->uiTranNo_openAvDev, pFrom.audio.ta_recvd.uiTranNo_openAvDev);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("transformTaskAudio: err, %I64u, mem.uiTranNo_openAvDev %d != from.audio. %d. skipped"), pMem->idInfo.ui64Id, pMem->uiTranNo_openAvDev, pFrom.audio.ta_recvd.uiTranNo_openAvDev);
						continue;
					}
				}

				//
#if __DEBUG__
#endif

				//
				//memset(  pPkt_dec,  0,  offsetof(  myPLAY_AUDIO_data,  buf  )  );
				mymemset((IntPtr)(byte*)&pPkt_dec->head, 0, sizeof(TRANSFER_AUDIO_dataMemHead));   //  2015/02/14
				if (0 != pMem->idInfo.ui64Id)
				{
					pPkt_dec->head.idInfo.ui64Id = pMem->idInfo.ui64Id;
#if false
					if (pPkt_dec->head.idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
					else pPkt_dec->head.tmpInternal.uiTranNo_org = 0;
#endif
					pPkt_dec->head.tmpInternal.uiTranNo_org = pMem->uiTranNo_openAvDev;
				}
				else
				{
					goto errLabel;
#if false
					pPkt_dec->head.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
					pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
#endif
				}
				//  2017/10/13
				pPkt_dec->head.uiPts = pMem->uiPts;

				//  2015/05/19
				if (pTask.saveVideo.ucbSaveVideo_req)
				{
					//
					//(  BYTE  *  )pReq->buf  +  iPos,  pMem->uiLen,  pMem->uiSampleTimeInMs,  pMem->ui_rtTimeLen
					//
					byte* data = pInput1;// pReq->buf + iPos;
					int len = (int)pMem->uiLen;
					postAPktToSave(Consts.CONST_uiTransformType_dec, pTransform, pMem->uiSampleTimeInMs, 0, data, len);
				}



				//
				if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_acm)
				{

					/*
# ifdef  __SUPPORT_acmToDec__
				dwBytes_pcm = pMem->uiLen * pTransform.audio.pWf_pcm->nAvgBytesPerSec / pTransform.audio.pWf_compress->nAvgBytesPerSec;
				if (!pTransform.audio.pBuf_pcm || pTransform.audio.dwBufSize_pcm <= dwBytes_pcm)
				{
					if (pTransform.audio.pBuf_pcm)
					{
						free(pTransform.audio.pBuf_pcm); pTransform.audio.pBuf_pcm = null;
					}
					dwBytes_pcm = dwBytes_pcm * 3 / 2 + 256;
					if (!(pTransform.audio.pBuf_pcm = (char*)mymalloc(dwBytes_pcm)))
					{
						traceLogA("procTaskAudio: malloc for pBuf_pcm failed.");
						goto errLabel;
					}
					pTransform.audio.dwBufSize_pcm = dwBytes_pcm;
				}

				//
				//  pPkt_dec->head.uiSampleTimeInMs  =  pMem->uiSampleTimeInMs;
				pPkt_dec->head.uiLen = pProcInfo.cfg.uiBufSize_myPlayAudioData;    //  mycountof(  pPkt_dec->buf  );

				if (convertAudio(pTransform.audio.u.acm.hstr1, pTransform.audio.u.acm.hstr2, pTransform.audio.pWf_compress, (BYTE*)pReq->buf + iPos, pMem->uiLen, pTransform.audio.pWf_pcm, (BYTE*)pTransform.audio.pBuf_pcm, pTransform.audio.dwBufSize_pcm, pTransform.audio.pWf_decompress, (BYTE*)pPkt_dec->memory.m_pBuf, (uint*)&pPkt_dec->head.uiLen)) goto errLabel;

				//  pFrom.audio.pDecompressBuf_transferAudioData->usCnt  ++  ;

				//  pWfx_toPlay  =  pTransform.audio.pWf_decompress;
				pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;
# ifdef  __DEBUG__
				//  traceLogA(  "transformA: uiSampleTimeInMs %d",  pPkt_dec->head.uiSampleTimeInMs  );
#endif

#if  0
			   playAudioData(  pProcInfo,  pMisCnt,  pTransform.iTaskId,  pTask,  pWfx_toPlay,  pPkt_dec,  pFrom  );
#else
				if (!chkFromPlayerIndex_a(CONST_uiTransformType_dec, pTransform, pPkt_dec, 0))
				{
					playAudioData(pProcInfo, pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, (BYTE*)pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
				}
#endif

				if (!pTask.ucbVideoConferenceStarter) continue;

				continue;

#else
				MY_DMO_INFO* pDmoInfo = null;
				int index_dmo = 0;
				if (pFrom.ucbVideoConferenceStarter) index_dmo = pMem->usIndex;

				pDmoInfo = &pTransform.audio.u.dmo;
				pDmoInfo->pDebugStep = &pTransform.audio.debugStep;

				//  pWfx_toPlay  =  &pFrom.audio.ah_decompress_recvd.wfx;
				pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;

				if (chkFromPlayerIndex_a(pProcInfo, pMisCnt, pTransform.iTaskId, pTask, pWfx_toPlay, pPkt_dec, pFrom)) continue;
				if (pDmoInfo->iIndex_player != pFrom.iIndex_player) pDmoInfo->iIndex_player = pFrom.iIndex_player;

# ifdef  __DEBUG__
				if (pProcInfo.cfg.debugStatusInfo.ucbPrint_a_in)
				{
					traceLogA("transformA: uiSampleTimeInMs %d. req.usCnt %d, req.len %d", pMem->uiSampleTimeInMs, pReq->usCnt, pReq->uiLen);
				}
#endif

				if (decompressAudio_filterGraph(pDmoInfo, &pFrom.audio.ah_compress_recvd, (BYTE*)pReq->buf + iPos, pMem->uiLen, pMem->uiSampleTimeInMs, pMem->ui_rtTimeLen)) goto errLabel;

				//  if  (  pFrom.iDiffInMs_tStartAudios[index_dmo]  !=  pDmoInfo->iDiffInMs_tStartStream  )  pFrom.iDiffInMs_tStartAudios[index_dmo]  =  pDmoInfo->iDiffInMs_tStartStream;

#endif
					*/
					traceLog("acm not supported");
					goto errLabel;
				}
				else if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_dmo)
				{
					/*
				MY_DMO_INFO* pDmoInfo = null;
				int index_dmo = 0;
#if  0
				   if  (  pFrom.ucbVideoConferenceStarter  )  index_dmo  =  pMem->usIndex;
#endif

				pDmoInfo = &pTransform.audio.u.dmo;

				//  2011/07/03
				//  if  (  decompressAudio_dmo(  pDmoInfo,  &pFrom.audio.ah_compress_recvd,  (  BYTE  *  )pReq->buf  +  iPos,  pMem->uiLen,  pMem->uiSampleTimeInMs,  pMem->ui_rtTimeLen  )  )  goto  errLabel;
				if (decompressAudio_dmo(pDmoInfo, &pTransform.audio.ta.ah_compress, (BYTE*)pReq->buf + iPos, pMem->uiLen, pMem->uiSampleTimeInMs, pMem->ui_rtTimeLen))
				{
					//
					//  此标志位表明需要重新初始化transform. 2012/03/19
					pTransform.audio.status.ucbDecFailed = TRUE;
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s %s"), tHintBuf, _T("decompressAudio_dmo failed."));
					//
					goto errLabel;
				}

				if (pFrom.iDiffInMs_tStartAudios[index_dmo] != pDmoInfo->iDiffInMs_tStartStream) pFrom.iDiffInMs_tStartAudios[index_dmo] = pDmoInfo->iDiffInMs_tStartStream;

				int j;
				for (j = 0; ; j++)
				{

					pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;
					pPkt_dec->head.uiLen = pProcInfo.cfg.uiBufSize_myPlayAudioData;    //  mycountof(  pPkt_dec->buf  );

					if (getDmoOutputByIndex(pDmoInfo, j, (BYTE*)pPkt_dec->memory.m_pBuf, &pPkt_dec->head.uiLen, &pPkt_dec->head.uiSampleTimeInMs, &pPkt_dec->head.ui_rtTimeLen)) break;

					//
					pFrom.iSampleTimeInMs_audios[index_dmo] = pPkt_dec->head.uiSampleTimeInMs;     //  2010/06/20

					//
					//  pWfx_toPlay  =  &pFrom.audio.ah_decompress_recvd.wfx;
					//  playAudioData(  pProcInfo,  pMisCnt,  pTransform.iTaskId,  pTask,  pWfx_toPlay,  pPkt_dec,  pFrom  );
					if (!chkFromPlayerIndex_a(CONST_uiTransformType_dec, pTransform, pPkt_dec, 0))
					{
						playAudioData(pProcInfo, pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, (BYTE*)pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
					}

					if (!pTask.ucbVideoConferenceStarter) continue;

					continue;
				}
					*/
					traceLog("dmo not supported");
					goto errLabel;

				}
				else
				{
					CUS_MODULE_compress pModule = null;
					/*
					switch  (  pTransform.audio.ta.compressor.common.ucCompressors  )  {
							case  CONST_audioCompressors_ipp:
								  pModule  =  pQyMc->cusModules.pIpp;
								  break;
							case  CONST_audioCompressors_custom:
								  pModule	=	getCusModule(  &pQyMc->cusModules,  CONST_moduleType_compress  );
								  break;
							default:
								traceLogA(  "procTaskAudio: ucComprssors error, %d",  pFrom.audio.ta_recvd.compressor.common.ucCompressors  );
								qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "procTaskAudio: ucCompressors error, %d"  ),  (  int  )pFrom.audio.ta_recvd.compressor.common.ucCompressors  );
								   goto  errLabel;
								   break;
					}
					*/
					M_getCompressModule(pCusModules, pTransform.audio.uiModuleType_toDec, ref pModule);

					if (null == pModule
							|| null == pModule.pf_qdcDecompressAudio
							 || null == pModule.pf_qdcGetOutputByIndex) goto errLabel;


					ref QOI_common pDmoInfo = ref pTransform.audio.u_qoi.myQoi.common;   //  .qdcObjInfo;
					int index_dmo = 0;

					//
					if (pProcInfo.cfg.debugStatusInfo.ucb__Dump_mp3_dec__)
					{
						//
						byte* audio = pInput1;// pReq->buf + iPos;
						int len = (int)pMem->uiLen;
						//
						if (bDbg)
						{
							str = string.Format("transformTaskAudioPkt: idInfo {0}, pts {1}, len {2}", pTransform.audio.idInfo.ui64Id, pMem->uiPts, len);
							traceLog(str);
						}
						//
						GuiShare.pf_dumpData1((IntPtr)audio, len, 0, 0, true, false, Consts.CONST_fn_dump_mp3_dec);


					}


					//  2011/07/03
					//  if  (  pModule->compress.pf_qdcDecompressAudio(  pDmoInfo,  &pFrom.audio.ah_compress_recvd,  (  BYTE  *  )pReq->buf  +  iPos,  pMem->uiLen,  pMem->uiSampleTimeInMs,  pMem->ui_rtTimeLen  )  )  goto  errLabel;
					if (pModule.pf_qdcDecompressAudio(ref pDmoInfo, ref pTransform.audio.ta.ah_compress, (IntPtr)(byte*)pInput1, pMem->uiLen, (int)pMem->uiSampleTimeInMs, pMem->ui_rtTimeLen) < 0) goto errLabel;
					//if (pActiveMem_from.sth.iDiffInMs_tStartAudios[index_dmo] != pDmoInfo.var.iDiffInMs_tStartAudio) pFrom.sth.iDiffInMs_tStartAudios[index_dmo] = pDmoInfo.var.iDiffInMs_tStartAudio;

					//  2017/10/14
					bool bDecDataGot = false;

					//
					int j;
					for (j = 0; ; j++)
					{
						pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;
						pPkt_dec->head.uiLen = pProcInfo.cfg.uiBufSize_myPlayAudioData;    //  mycountof(  pPkt_dec->buf  );

						if (0 != pModule.pf_qdcGetOutputByIndex(ref pDmoInfo, j, pPkt_dec->memory.m_pBuf, ref pPkt_dec->head.uiLen, ref pPkt_dec->head.uiSampleTimeInMs, ref pPkt_dec->head.ui_rtTimeLen)) break;

						//
						if (pProcInfo.cfg.debugStatusInfo.ucb__Dump_deced_pcm__)
						{
							GuiShare.pf_dumpData1(pPkt_dec->memory.m_pBuf, (int)pPkt_dec->head.uiLen, 0, 0, true, false, "dump_deced_pcm.pcm");
							str = string.Format("deced_pcm {0}", j);
							traceLog(str);
							if (j == 119)
							{
								int kk1 = 0;
							}
						}

						//
						bDecDataGot = true;

						//  pWfx_toPlay  =  &pFrom.audio.ah_decompress_recvd.wfx;
						//  playAudioData(  pProcInfo,  pMisCnt,  pTransform.iTaskId,  pTask,  pWfx_toPlay,  pPkt_dec,  pFrom  );
						if (0 == chkFromPlayerIndex_a(Consts.CONST_uiTransformType_dec, pTransform, pPkt_dec, null))
						{
							playAudioData(pProcInfo, (int)pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
						}

						if (!pTask.ucbVideoConferenceStarter) continue;

						//  addToReq_toCompress_audio(  pProcInfo,  pTranInfo,  pTask,  index_activeMems_from,  pPkt_dec,  pMsgTask_toCompress  );

						continue;
					}

					//  2017/10/14
					if (!bDecDataGot)
					{  //  2017/10/14. 为了让player能校验uiPts, 故要发个空包过去。
						pPkt_dec->head.uiLen = 0;
						//
						playAudioData(pProcInfo, (int)pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
						//
						continue;
					}
				}
			}

			iErr = 0;

		errLabel:


			//  if  (  pWf_compress  )  free(  pWf_compress  );
			//  if  (  pWf_pcm  )  free(  pWf_pcm  );
			//  if  (  pWf_decompress  )  free(  pWf_decompress  );

			//  if  (  pTransferAudioData  )  free(  pTransferAudioData  );


			if (0 == iErr)
			{
				if (!pTask.bVideoConference())
				{
					//mySendAvResp(pProcInfo, pMsgTask, pMsgBuf  );
				}
			}

			//if (iErr || tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, where_showInfo, _T(""), _T("%I64u %s: %s"), pMsgTask->data.route.idInfo_from.ui64Id, (iErr ? _T("failed") : _T("")), tHintBuf);

			return iErr;

		}



		//
		public static unsafe int transformTaskAudioPkt_useAudioQ2(QY_TRANSFORM pTransform, MIS_MSG_INPUT* pMsgTask, TRANSFER_AUDIO_DATA* pReq, PKT_info_toDec* pPktInfo_toDec, int index_pMems_from, int index_activeMems_from, myPLAY_AUDIO_DATA* pPkt_dec, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;// (QY_MC*)pProcInfo.pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			bool bDbg = true;
			string str;

			//
			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			int iTaskId = pTransform.iTaskId;
			//AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			//  myPLAY_AUDIO_DATA	*	pPkt_dec			=	(  myPLAY_AUDIO_DATA  *  )&pTransform.audio.pkt_dec;
			//  MIS_MSG_TASK		*	pMsgTask_toCompress	=	(  MIS_MSG_TASK  *  )pTransform.audio.pMsgTask_toCompress_videoConference;

			//
			CTX_caller ctxCaller = new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "transformTaskAudioPkt");
			if (null == pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (null == pTaskData) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null == pMisCnt) return -1;
			int tmpiRet = 0;
			//char timeBuf[32] = "";
			//TCHAR tHintBuf[256] = _T("");
			if (null == pReq || 0 == pReq->usCnt || pReq->usCnt > Consts.CONST_maxTransferAvDataMems) return -1;
			TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
			int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
			if (null == pPktInfo_toDec) return -1;

			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null == pCusModules) return -1;

			//
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


			//
			//int index = index_pMems_from;
			uint curTickCnt = myGetTickCount(null);

			//TCHAR where_showInfo[32] = _T(""); _sntprintf(where_showInfo, mycountof(where_showInfo), _T("transA %d"), pTransform.index_activeMems_from);

			//if (null == pAddr) return -1;


			//
#if false
			if (pMsgTask->tStartTime < pFrom.tStartTime_task)
			{
				//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s tStartTime < tStartTime_task. skipped."), tHintBuf); 
				goto errLabel;
			}
#endif
			//

			//
			if (pTransform.audio.ucbInited)
			{
				if (pTransform.audio.index_activeMems_from != index_activeMems_from
					|| pTransform.audio.idInfo.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
					|| pTransform.audio.ta.uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev
					|| pTransform.audio.status.ucbDecFailed            //  2012/03/19
						)
				{
					//traceLogA("procTaskAudio: transform.index_pMems_from %d, uiTranNo_openAvDev %d != %d, %d", pTransform.audio.index_pMems_from, pTransform.audio.ta.uiTranNo_openAvDev, index_pMems_from, pFrom.audio.ta_recvd.uiTranNo_openAvDev);
					exitTaskAvFrom_audio(pProcInfo, pTransform, "transformTaskAudioPkt enters");
				}
			}
			else
			{
				exitTaskAvFrom_audio(pProcInfo, pTransform, "transformTaskAudioPkt enters");
			}

			//  2009/04/13
			if (!pTransform.audio.ucbInited)
			{
				if (0 != initTaskAvFrom_audio(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform)) goto errLabel;
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("procTaskAudio: initTaskAvFrom_audio %d"), index_pMems_from);
			}
			if (!pTransform.audio.ucbInited)
			{
				traceLogA("procTaskAudio failed: ³õÊ¼»¯Î´Íê³É¡£");
				goto errLabel;
			}

			//  check
			if (pMsgTask->data.route.idInfo_from.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
				|| pReq->uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
			{
				str = string.Format("transformTaskAudioPkt failed: idInfo_from {0}, audioData.uiTranNo {1} !=  pActive {2}, {3}", pMsgTask->data.route.idInfo_from.ui64Id, pReq->uiTranNo_openAvDev, pActiveMem_from.avStream.idInfo.ui64Id, pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev);
				showInfo_open(0, null, str);
				//
				goto errLabel;
			}


#if false
			if (!pTask.ucbVideoConference)
			{
				//
				pFrom.audio.usCnt_pkts++;
				if (0!=pReq->tLastTime_showFrameInfo_020534)
				{
					if (0==pFrom.audio.tLastTime_showFrameInfo_020534)
					{   //  ÕâÊÇÔÚµÚÒ»´ÎµÄÊ±ºò£¬Ã»¿ªÊ¼¼ÆÊý£¬ÏìÓ¦OK¡£
						pReq->tmpInternal.result.usRespCode = Consts.CONST_qyRc_ok;

						//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("×¼±¸Ö¡¼ÆÊý."));

						//  pFrom.audio.uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;	//  2013/08/04. 不能修改pFrom.audio.uiTranNo_openAvDev
					}
					else if (pFrom.audio.ta_recvd.uiTranNo_openAvDev == pReq->uiTranNo_openAvDev
							   && pFrom.audio.tLastTime_showFrameInfo_020534 == pReq->tLastTime_showFrameInfo_020534
								   && pFrom.audio.usCnt_pkts >= pReq->usCnt_pkts)
					{
						pReq->tmpInternal.result.usRespCode = Consts.CONST_qyRc_ok;

						//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("pkts %d, Ò»ÖÂ."), pReq->usCnt_pkts);
					}
					else
					{
						/*
						CQyString str;
						CQyString str_expected;

						pReq->tmpInternal.result.usRespCode = CONST_qyRc_err;

						getTimelStr(pFrom.audio.tLastTime_showFrameInfo_020534, timeBuf, mycountof(timeBuf)); str = CQyString(timeBuf);
						getTimelStr((time_t)pReq->tLastTime_showFrameInfo_020534, timeBuf, mycountof(timeBuf)); str_expected = CQyString(timeBuf);
						_sntprintf(tHintBuf, mycountof(tHintBuf), _T("Error: ( %d, %s, %d pkts ) != ÆÚÍû  ( %d, %s, %d pkts )."), pFrom.audio.ta_recvd.uiTranNo_openAvDev, str, pFrom.audio.usCnt_pkts, pReq->uiTranNo_openAvDev, str_expected, pReq->usCnt_pkts);
						*/

						//  pFrom.audio.uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;			
					}
					pReq->tmpInternal.result.usCnt_pkts = pFrom.audio.usCnt_pkts;  //  Òª½«·¢ËÍµÄÖ¡Êý·¢»Ø·¢ËÍ·½¡£

					pFrom.audio.usCnt_pkts = 0;
					pFrom.audio.tLastTime_showFrameInfo_020534 = pMsgTask->tStartTime;
				}

			}
#endif

			//  ÏÂÃæ¿ªÊ¼½âÑ¹Ëõ
			uint dwBytes_pcm;
			TRANSFER_AUDIO_dataMemHead* pMem;
			int iPos;
			fixed (WAVEFORMATEX* pWfx_toPlay = &pTransform.audio.ta.ah_decompress.wfx) { }


			//  TRANSFER_AUDIO_DATA			*	pReq_toCompress;	pReq_toCompress  =  null;

			/*
			if  (  pTask.ucbVideoConferenceStarter  )  {
				if  (  !pMsgTask_toCompress  )  goto  errLabel;
				pReq_toCompress  =  (  TRANSFER_AUDIO_DATA  *  )pMsgTask_toCompress->data.buf;

				pMsgTask_toCompress->uiType  =  CONST_misMsgType_task;
				//
				memset(  pReq_toCompress,  0,  offsetof(  TRANSFER_AUDIO_DATA,  buf  )  );
				pReq_toCompress->uiType =  pReq->uiType;
				pReq_toCompress->uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;
			}
			*/

			//  memset(  pFrom.audio.pDecompressBuf_transferAudioData,  0,  offsetof(  TRANSFER_AUDIO_DATA,  mems  )  );
			//  pFrom.audio.pDecompressBuf_transferAudioData->uiType  =  pReq->uiType;
			//  pFrom.audio.pDecompressBuf_transferAudioData->uiTranNo_openAvDev  =  pReq->uiTranNo_openAvDev;

			if (0 == pReq->usCnt || pReq->usCnt > Consts.CONST_maxTransferAvDataMems)
			{
#if __DEBUG__
				traceLogA("procTaskAudio failed: pReq->usCnt %d error", pReq->usCnt);
#endif
				goto errLabel;
			}


			//for  (  iPos  =  0,  i  =  0;  i  <  pReq->usCnt;  iPos  +=  pReq->mems[i].uiLen,  i  ++  )  
			for (int kk = 0; kk < 1; kk++)
			{
				if (!pPktInfo_toDec->bGot_msg) goto errLabel;
				if (!pPktInfo_toDec->info.bGot_pkt) goto errLabel;
				//
				//  2017/10/14
				iPos = pPktInfo_toDec->info.iPos;
				if (pPktInfo_toDec->info.index_mems < 0 || pPktInfo_toDec->info.index_mems >= pReq->usCnt) goto errLabel;
				int i;
				i = pPktInfo_toDec->info.index_mems;

				//
				pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

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
					continue;
				}
				if (0 != pMem->uiTranNo_openAvDev)
				{   //  2013/08/04. 会议时,参会人如果第2次进入会议,那么需要区分前后2次的音频流是不同的,靠的就是tn
					if (pMem->uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
					{
#if __DEBUG__
						traceLog(_T("transformTaskAudio: err, mem.uiTranNo_openAvDev %d != from.audio. %d. skipped"), pMem->uiTranNo_openAvDev, pFrom.audio.ta_recvd.uiTranNo_openAvDev);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("transformTaskAudio: err, %I64u, mem.uiTranNo_openAvDev %d != from.audio. %d. skipped"), pMem->idInfo.ui64Id, pMem->uiTranNo_openAvDev, pFrom.audio.ta_recvd.uiTranNo_openAvDev);
						continue;
					}
				}

				//
#if __DEBUG__
#endif

				//
				//memset(  pPkt_dec,  0,  offsetof(  myPLAY_AUDIO_data,  buf  )  );
				mymemset((IntPtr)(byte*)&pPkt_dec->head, 0, sizeof(TRANSFER_AUDIO_dataMemHead));   //  2015/02/14
				if (0 != pMem->idInfo.ui64Id)
				{
					pPkt_dec->head.idInfo.ui64Id = pMem->idInfo.ui64Id;
					if (pPkt_dec->head.idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
					else pPkt_dec->head.tmpInternal.uiTranNo_org = 0;
				}
				else
				{
					pPkt_dec->head.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
					pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
				}
				//  2017/10/13
				pPkt_dec->head.uiPts = pMem->uiPts;

				//  2015/05/19
				if (pTask.saveVideo.ucbSaveVideo_req)
				{
					//
					//(  BYTE  *  )pReq->buf  +  iPos,  pMem->uiLen,  pMem->uiSampleTimeInMs,  pMem->ui_rtTimeLen
					//
					byte* data = pReq->buf + iPos;
					int len = (int)pMem->uiLen;
					postAPktToSave(Consts.CONST_uiTransformType_dec, pTransform, pMem->uiSampleTimeInMs, 0, data, len);
				}



				//
				if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_acm)
				{

					/*
# ifdef  __SUPPORT_acmToDec__
				dwBytes_pcm = pMem->uiLen * pTransform.audio.pWf_pcm->nAvgBytesPerSec / pTransform.audio.pWf_compress->nAvgBytesPerSec;
				if (!pTransform.audio.pBuf_pcm || pTransform.audio.dwBufSize_pcm <= dwBytes_pcm)
				{
					if (pTransform.audio.pBuf_pcm)
					{
						free(pTransform.audio.pBuf_pcm); pTransform.audio.pBuf_pcm = null;
					}
					dwBytes_pcm = dwBytes_pcm * 3 / 2 + 256;
					if (!(pTransform.audio.pBuf_pcm = (char*)mymalloc(dwBytes_pcm)))
					{
						traceLogA("procTaskAudio: malloc for pBuf_pcm failed.");
						goto errLabel;
					}
					pTransform.audio.dwBufSize_pcm = dwBytes_pcm;
				}

				//
				//  pPkt_dec->head.uiSampleTimeInMs  =  pMem->uiSampleTimeInMs;
				pPkt_dec->head.uiLen = pProcInfo.cfg.uiBufSize_myPlayAudioData;    //  mycountof(  pPkt_dec->buf  );

				if (convertAudio(pTransform.audio.u.acm.hstr1, pTransform.audio.u.acm.hstr2, pTransform.audio.pWf_compress, (BYTE*)pReq->buf + iPos, pMem->uiLen, pTransform.audio.pWf_pcm, (BYTE*)pTransform.audio.pBuf_pcm, pTransform.audio.dwBufSize_pcm, pTransform.audio.pWf_decompress, (BYTE*)pPkt_dec->memory.m_pBuf, (uint*)&pPkt_dec->head.uiLen)) goto errLabel;

				//  pFrom.audio.pDecompressBuf_transferAudioData->usCnt  ++  ;

				//  pWfx_toPlay  =  pTransform.audio.pWf_decompress;
				pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;
# ifdef  __DEBUG__
				//  traceLogA(  "transformA: uiSampleTimeInMs %d",  pPkt_dec->head.uiSampleTimeInMs  );
#endif

#if  0
			   playAudioData(  pProcInfo,  pMisCnt,  pTransform.iTaskId,  pTask,  pWfx_toPlay,  pPkt_dec,  pFrom  );
#else
			if (!chkFromPlayerIndex_a(CONST_uiTransformType_dec, pTransform, pPkt_dec, 0))
				{
					playAudioData(pProcInfo, pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, (BYTE*)pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
				}
#endif

				if (!pTask.ucbVideoConferenceStarter) continue;

				continue;

#else
				MY_DMO_INFO* pDmoInfo = null;
				int index_dmo = 0;
				if (pFrom.ucbVideoConferenceStarter) index_dmo = pMem->usIndex;

				pDmoInfo = &pTransform.audio.u.dmo;
				pDmoInfo->pDebugStep = &pTransform.audio.debugStep;

				//  pWfx_toPlay  =  &pFrom.audio.ah_decompress_recvd.wfx;
				pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;

				if (chkFromPlayerIndex_a(pProcInfo, pMisCnt, pTransform.iTaskId, pTask, pWfx_toPlay, pPkt_dec, pFrom)) continue;
				if (pDmoInfo->iIndex_player != pFrom.iIndex_player) pDmoInfo->iIndex_player = pFrom.iIndex_player;

# ifdef  __DEBUG__
				if (pProcInfo.cfg.debugStatusInfo.ucbPrint_a_in)
				{
					traceLogA("transformA: uiSampleTimeInMs %d. req.usCnt %d, req.len %d", pMem->uiSampleTimeInMs, pReq->usCnt, pReq->uiLen);
				}
#endif

				if (decompressAudio_filterGraph(pDmoInfo, &pFrom.audio.ah_compress_recvd, (BYTE*)pReq->buf + iPos, pMem->uiLen, pMem->uiSampleTimeInMs, pMem->ui_rtTimeLen)) goto errLabel;

				//  if  (  pFrom.iDiffInMs_tStartAudios[index_dmo]  !=  pDmoInfo->iDiffInMs_tStartStream  )  pFrom.iDiffInMs_tStartAudios[index_dmo]  =  pDmoInfo->iDiffInMs_tStartStream;

#endif
					*/
					traceLog("acm not supported");
					goto errLabel;
				}
				else if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_dmo)
				{
					/*
				MY_DMO_INFO* pDmoInfo = null;
				int index_dmo = 0;
#if  0
				   if  (  pFrom.ucbVideoConferenceStarter  )  index_dmo  =  pMem->usIndex;
#endif

				pDmoInfo = &pTransform.audio.u.dmo;

				//  2011/07/03
				//  if  (  decompressAudio_dmo(  pDmoInfo,  &pFrom.audio.ah_compress_recvd,  (  BYTE  *  )pReq->buf  +  iPos,  pMem->uiLen,  pMem->uiSampleTimeInMs,  pMem->ui_rtTimeLen  )  )  goto  errLabel;
				if (decompressAudio_dmo(pDmoInfo, &pTransform.audio.ta.ah_compress, (BYTE*)pReq->buf + iPos, pMem->uiLen, pMem->uiSampleTimeInMs, pMem->ui_rtTimeLen))
				{
					//
					//  此标志位表明需要重新初始化transform. 2012/03/19
					pTransform.audio.status.ucbDecFailed = TRUE;
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s %s"), tHintBuf, _T("decompressAudio_dmo failed."));
					//
					goto errLabel;
				}

				if (pFrom.iDiffInMs_tStartAudios[index_dmo] != pDmoInfo->iDiffInMs_tStartStream) pFrom.iDiffInMs_tStartAudios[index_dmo] = pDmoInfo->iDiffInMs_tStartStream;

				int j;
				for (j = 0; ; j++)
				{

					pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;
					pPkt_dec->head.uiLen = pProcInfo.cfg.uiBufSize_myPlayAudioData;    //  mycountof(  pPkt_dec->buf  );

					if (getDmoOutputByIndex(pDmoInfo, j, (BYTE*)pPkt_dec->memory.m_pBuf, &pPkt_dec->head.uiLen, &pPkt_dec->head.uiSampleTimeInMs, &pPkt_dec->head.ui_rtTimeLen)) break;

					//
					pFrom.iSampleTimeInMs_audios[index_dmo] = pPkt_dec->head.uiSampleTimeInMs;     //  2010/06/20

					//
					//  pWfx_toPlay  =  &pFrom.audio.ah_decompress_recvd.wfx;
					//  playAudioData(  pProcInfo,  pMisCnt,  pTransform.iTaskId,  pTask,  pWfx_toPlay,  pPkt_dec,  pFrom  );
					if (!chkFromPlayerIndex_a(CONST_uiTransformType_dec, pTransform, pPkt_dec, 0))
					{
						playAudioData(pProcInfo, pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, (BYTE*)pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
					}

					if (!pTask.ucbVideoConferenceStarter) continue;

					continue;
				}
					*/
					traceLog("dmo not supported");
					goto errLabel;

				}
				else
				{
					CUS_MODULE_compress pModule = null;
					/*
					switch  (  pTransform.audio.ta.compressor.common.ucCompressors  )  {
							case  CONST_audioCompressors_ipp:
								  pModule  =  pQyMc->cusModules.pIpp;
								  break;
							case  CONST_audioCompressors_custom:
								  pModule	=	getCusModule(  &pQyMc->cusModules,  CONST_moduleType_compress  );
								  break;
							default:
								traceLogA(  "procTaskAudio: ucComprssors error, %d",  pFrom.audio.ta_recvd.compressor.common.ucCompressors  );
								qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "procTaskAudio: ucCompressors error, %d"  ),  (  int  )pFrom.audio.ta_recvd.compressor.common.ucCompressors  );
								   goto  errLabel;
								   break;
					}
					*/
					M_getCompressModule(pCusModules, pTransform.audio.uiModuleType_toDec, ref pModule);

					if (null == pModule
							|| null == pModule.pf_qdcDecompressAudio
							 || null == pModule.pf_qdcGetOutputByIndex) goto errLabel;


					ref QOI_common pDmoInfo = ref pTransform.audio.u_qoi.myQoi.common;   //  .qdcObjInfo;
					int index_dmo = 0;

					//
					if (pProcInfo.cfg.debugStatusInfo.ucb__Dump_mp3_dec__)
					{
						//
						byte* audio = pReq->buf + iPos;
						int len = (int)pMem->uiLen;
						//
						if (bDbg)
						{
							str = string.Format("transformTaskAudioPkt: idInfo {0}, pts {1}, len {2}", pTransform.audio.idInfo.ui64Id, pMem->uiPts, len);
							traceLog(str);
						}
						//
						GuiShare.pf_dumpData1((IntPtr)audio, len, 0, 0, true, false, Consts.CONST_fn_dump_mp3_dec);


					}


					//  2011/07/03
					//  if  (  pModule->compress.pf_qdcDecompressAudio(  pDmoInfo,  &pFrom.audio.ah_compress_recvd,  (  BYTE  *  )pReq->buf  +  iPos,  pMem->uiLen,  pMem->uiSampleTimeInMs,  pMem->ui_rtTimeLen  )  )  goto  errLabel;
					if (pModule.pf_qdcDecompressAudio(ref pDmoInfo, ref pTransform.audio.ta.ah_compress, (IntPtr)(byte*)(pReq->buf + iPos), pMem->uiLen, (int)pMem->uiSampleTimeInMs, pMem->ui_rtTimeLen) < 0) goto errLabel;
					//if (pActiveMem_from.sth.iDiffInMs_tStartAudios[index_dmo] != pDmoInfo.var.iDiffInMs_tStartAudio) pFrom.sth.iDiffInMs_tStartAudios[index_dmo] = pDmoInfo.var.iDiffInMs_tStartAudio;

					//  2017/10/14
					bool bDecDataGot = false;

					//
					int j;
					for (j = 0; ; j++)
					{
						pPkt_dec->head.uiSampleTimeInMs = pMem->uiSampleTimeInMs;
						pPkt_dec->head.uiLen = pProcInfo.cfg.uiBufSize_myPlayAudioData;    //  mycountof(  pPkt_dec->buf  );

						if (0 != pModule.pf_qdcGetOutputByIndex(ref pDmoInfo, j, pPkt_dec->memory.m_pBuf, ref pPkt_dec->head.uiLen, ref pPkt_dec->head.uiSampleTimeInMs, ref pPkt_dec->head.ui_rtTimeLen)) break;

						//
						if (pProcInfo.cfg.debugStatusInfo.ucb__Dump_deced_pcm__)
						{
							GuiShare.pf_dumpData1(pPkt_dec->memory.m_pBuf, (int)pPkt_dec->head.uiLen, 0, 0, true, false, "dump_deced_pcm.pcm");
							str = string.Format("deced_pcm {0}", j);
							traceLog(str);
							if (j == 119)
							{
								int kk1 = 0;
							}
						}

						//
						bDecDataGot = true;

						//  pWfx_toPlay  =  &pFrom.audio.ah_decompress_recvd.wfx;
						//  playAudioData(  pProcInfo,  pMisCnt,  pTransform.iTaskId,  pTask,  pWfx_toPlay,  pPkt_dec,  pFrom  );
						if (0 == chkFromPlayerIndex_a(Consts.CONST_uiTransformType_dec, pTransform, pPkt_dec, null))
						{
							playAudioData(pProcInfo, (int)pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
						}

						if (!pTask.ucbVideoConferenceStarter) continue;

						//  addToReq_toCompress_audio(  pProcInfo,  pTranInfo,  pTask,  index_activeMems_from,  pPkt_dec,  pMsgTask_toCompress  );

						continue;
					}

					//  2017/10/14
					if (!bDecDataGot)
					{  //  2017/10/14. 为了让player能校验uiPts, 故要发个空包过去。
						pPkt_dec->head.uiLen = 0;
						//
						playAudioData(pProcInfo, (int)pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, pPkt_dec->memory.m_pBuf, pPkt_dec->head.uiLen, pTransform.iIndex_player);
						//
						continue;
					}
				}
			}

			iErr = 0;

		errLabel:


			//  if  (  pWf_compress  )  free(  pWf_compress  );
			//  if  (  pWf_pcm  )  free(  pWf_pcm  );
			//  if  (  pWf_decompress  )  free(  pWf_decompress  );

			//  if  (  pTransferAudioData  )  free(  pTransferAudioData  );


			if (0 == iErr)
			{
				if (!pTask.bVideoConference())
				{
					//mySendAvResp(pProcInfo, pMsgTask, pMsgBuf  );
				}
			}

			//if (iErr || tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, where_showInfo, _T(""), _T("%I64u %s: %s"), pMsgTask->data.route.idInfo_from.ui64Id, (iErr ? _T("failed") : _T("")), tHintBuf);

			return iErr;

		}


	}
}
