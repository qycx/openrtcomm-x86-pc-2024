using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		//
#if false

		public static unsafe int tmpGetMaxAudioPlayCfg( AUDIO_PLAY_CFG pSrc0_param, AUDIO_PLAY_CFG pSrc1_param, ref AUDIO_PLAY_CFG pDst_param)
		{
			AUDIO_PLAY_CFG* pSrc0 = &pSrc0_param;
			AUDIO_PLAY_CFG* pSrc1 = &pSrc1_param;
			fixed (AUDIO_PLAY_CFG* pDst = &pDst_param)
			{
				//
				pDst->usMinTimeInMsOfAudioDataPlaying = Math.Max(pSrc0->usMinTimeInMsOfAudioDataPlaying, pSrc1->usMinTimeInMsOfAudioDataPlaying);                    //  2009/06/14
				pDst->usAudioDataAddedToPlayEveryTime = Math.Max(pSrc0->usAudioDataAddedToPlayEveryTime, pSrc1->usAudioDataAddedToPlayEveryTime);                    //  2009/06/16
				pDst->usNotifyIntervalInMs_needMoreAudioData = Math.Max(pSrc0->usNotifyIntervalInMs_needMoreAudioData, pSrc1->usNotifyIntervalInMs_needMoreAudioData);               //  2009/06/14. = CONST_notifyInterval

				//  2012/01/27
				pDst->usMinTimeInMsToPlay = Math.Max(pSrc0->usMinTimeInMsToPlay, pSrc1->usMinTimeInMsToPlay);                                            //  2012/01/27
																																					//
				pDst->usMaxTimeInMsToPlay = Math.Max(pSrc0->usMaxTimeInMsToPlay, pSrc1->usMaxTimeInMsToPlay);                                            //  2009/06/03
				pDst->usMaxTimeInMsToBeRemoved = Math.Max(pSrc0->usMaxTimeInMsToBeRemoved, pSrc1->usMaxTimeInMsToBeRemoved);                         //  2009/06/03
			}

			return 0;
		}


		public static unsafe int tmp_chkFromPlayerIndex_v(uint uiTranNo_openAvDev, ushort usIndex, ref BITMAPINFOHEADER pBmpInfo_input, int iSampleTimeInMs, int iSampleTimeInMs_org_unused, ref QY_MESSENGER_ID pIdInfo_sender, MIS_CNT pMIS_CNT, ref QY_MESSENGER_ID pIdInfo_peer, int iTaskId, QY_TRANSFORM pTransform, ref int piIndex_player_transform, PROC_TASK_AV pTask, myDRAW_VIDEO_DATA* pPkt)
		{
			int iErr = -1;
			//
			MIS_CNT pMisCnt = (MIS_CNT)pMIS_CNT;
			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//if (!pBmpInfo_input) return -1;
			//if (null==pIdInfo_sender) return -1;

			if (null==pTransform) return -1;
			ref TRAN_audio pTa = ref pTransform.audio.ta;
			ref TRAN_video pTv = ref pTransform.video.tv;

			bool bDbg = true;
			string str;

			//if (!pTa) return -1;
			//if (!pTv) return -1;
			if (null==piIndex_player_transform) return -1;

#if __DEBUG__
			if (0)
			{
				//  SampleTime  =  0;
				traceLogA("test here");
			}
#endif


			QY_PLAYER pPlayer;
			pPlayer = getPlayerByIndex(pProcInfo, piIndex_player_transform);  //  pTransform.iIndex_player  );
			if (null!=pPlayer)
			{
				if (pPlayer.iTaskId != iTaskId
					|| pPlayer.idInfo_recorder.ui64Id != pIdInfo_sender.ui64Id
					|| pPlayer.video.uiTranNo != uiTranNo_openAvDev
					)
				{
					traceLogA("drawVideoData: not correct player");
					piIndex_player_transform = -1;
					pPlayer = null;
				}
			}
			if (null==pPlayer)
			{
				int k;

				for (k = 0; k < pProcInfo.av.usCnt_players; k++)
				{

					if (pProcInfo.av.pPlayers[k].iTaskId != iTaskId) continue;     //  2010/09/07

					if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == pIdInfo_sender.ui64Id
						 && pProcInfo.av.pPlayers[k].video.uiTranNo == uiTranNo_openAvDev)
					{
						if (0==pTa.uiTranNo_openAvDev) break;

						if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == pIdInfo_sender.ui64Id
							&& pProcInfo.av.pPlayers[k].audio.uiTranNo == pTa.uiTranNo_openAvDev)
						{
							break;
						}
					}
				}

				if (k < pProcInfo.av.usCnt_players)
				{
					piIndex_player_transform = k;
					pPlayer = getPlayerByIndex(pProcInfo, k);
					if (null==pPlayer) goto errLabel;
					//
					if ( pPlayer!=null)
                    {
						int ii = 0;
                    }
				}
			}

			if (null==pPlayer || !pPlayer.video.bRunning)
			{


				//  ÕâÀï×é°ü·¢¸øÖ÷Ïß³Ì¡£
				MIS_MSG_applyForPlayer_qmc msg;
				mymemset((IntPtr)(byte*)&msg, 0, sizeof(MIS_MSG_applyForPlayer_qmc));
				msg.uiType = Consts.CONST_misMsgType_applyForPlayer_qmc;
				//
				msg.iTaskId = iTaskId;                              //  2009/09/17
				msg.idInfo_logicalPeer = pIdInfo_peer;
				//
				msg.idInfo_recorder.ui64Id = pIdInfo_sender.ui64Id;
				msg.video.uiTranNo = uiTranNo_openAvDev;
				//memcpy(&msg.video.vh.bih, pBmpInfo_input, sizeof(msg.video.vh.bih));
				msg.video.vh.bih = pBmpInfo_input;
				//msg.video.ucbFlippedImg  =  pTv->compressor.common.ucbOutputFlippedImg;
				msg.video.usFps = pTv.compressor.usMaxFps_toShareBmp;   //  2015/02/20

				//
				if(msg.video.vh.bih.biWidth ==0||msg.video.vh.bih.biHeight==0)
                {
					str = string.Format("tmp_chkFromPlayerIndex_v failed, bih err {0}x{1}", msg.video.vh.bih.biWidth, msg.video.vh.bih.biHeight);
					traceLog(str);
					goto errLabel;
                }


				//
				if (0!=pTa.uiTranNo_openAvDev)
				{
					msg.audio.uiTranNo = pTa.uiTranNo_openAvDev;
					//memcpy(&msg.audio.ah.wfx, &pTa.ah_decompress.wfx, sizeof(msg.audio.ah.wfx));
					msg.audio.ah.wfx = pTa.ah_decompress.wfx;

					//  2009/06/14
					if (pTask!=null)
					{
						//tmpGetMaxAudioPlayCfg( pTask.audio.playCfg,  pTa.compressor.playCfg, ref msg.audio.playCfg);
					}
				}

				//
				str=string.Format("chkFromPlayerIndex_v : {0},{1},{2} apply for player", pIdInfo_sender.ui64Id, msg.audio.uiTranNo, msg.video.uiTranNo);
				traceLog(str);

				//					  
				qPostMsg((IntPtr)(byte*)&msg, (uint)sizeof(MIS_MSG_applyForPlayer_qmc), pQyMc.gui.processQ);

				GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

				iErr = -2; goto errLabel;
			}


			//  2009/05/27

			//
			pPlayer.video.m_var.uiSampleTimeInMs_newRecvd = (uint)iSampleTimeInMs;
			//pPlayer.video.m_var.uiSampleTimeInMs_org_newRecvd  =  iSampleTimeInMs_org;
			pPlayer.video.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd = myGetTickCount(null);

			//  2015/09/30
			//if  (  pPlayer.video.m_var.uiPktResType  !=  p.u

			//
			//  2015/10/01
			//if (pPkt->hWnd_task != pTask->hWnd_task) pPkt->hWnd_task = pTask->hWnd_task;
			if (pPkt->usIndex_activeMems_from != usIndex) pPkt->usIndex_activeMems_from = usIndex;
			//if (null!=pIdInfo_peer)
			{
				if (pPkt->idInfo_peer.ui64Id != pIdInfo_peer.ui64Id) pPkt->idInfo_peer.ui64Id = pIdInfo_peer.ui64Id;
			}
			//  2016/12/27
			//if  (  pPkt->uiEventId_lastRecvd  !=  uiEventId_lastRecvd  )  pPkt->uiEventId_lastRecvd  =  uiEventId_lastRecvd;


			//  2009/05/03
			if (0!=iSampleTimeInMs)
			{

				//  traceLogA(  "drawVideoData: uiSampleTimeInMs %d, uiSampleTimeInMs_org %d",  p->head.uiSampleTimeInMs,  p->head.uiSampleTimeInMs_org  );

				if (iSampleTimeInMs < (int)(pPlayer.video.m_var.uiSampleTimeInMs - 500))
				{

#if __DEBUG__  //  2014/06/14. 在rtsp解码video.264时，这里还有点错。暂时屏蔽showInfo
#endif

					q2TriggerToProcess(pPlayer.video.q2);
				}
				if (0!=pPlayer.video.m_var.uiSampleTimeInMs)
				{
					if (iSampleTimeInMs > (int)(pPlayer.video.m_var.uiSampleTimeInMs + 10000))
					{   //  2009/05/25. too old video data
#if __DEBUG__  //  2014/06/15
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("drawVideoData: pkt.uiSampleTimeInMs %d > player.%d  +  10000. trigger to empty old data"), iSampleTimeInMs, pPlayer.video.m_var.uiSampleTimeInMs);
#endif
						q2TriggerToProcess(pPlayer.video.q2);
					}
				}
			}


			iErr = 0;
		errLabel:
			return iErr;
		}



		//
		//int  chkFromPlayerIndex_v(  QDC_OBJ_INFO  *  pQdcObjInfo,  myDRAW_VIDEO_DATA  *  pPkt,  void  *  p2  )
		public static unsafe int chkFromPlayerIndex_v(uint uiTransformType, QY_TRANSFORM pQY_TRANSFORM, myDRAW_VIDEO_DATA* pPkt, void* p2)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;

			if (uiTransformType != Consts.CONST_uiTransformType_dec) return -1;

			//if  (  !pQdcObjInfo  )  return  -1;
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;    //  pQdcObjInfo->cfg.pParent_transform;
			if (null==pTransform) return -1;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null==pProcInfo) return -1;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			MSGR_ADDR* pAddr = pTransform.pAddr_logicalPeer;
			if (null==pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null==pTranInfo_unused) return -1;
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return -1;

			//
			
			//
			ushort usIndex = (ushort)pTransform.index_activeMems_from;

			//
			ref QY_MESSENGER_ID pIdInfo_sender = ref pTransform.video.idInfo;
			uint uiTranNo_openAvDev = pTransform.video.tv.uiTranNo_openAvDev;

			//
			ref QY_MESSENGER_ID pIdInfo_peer = ref pAddr->idInfo;


			ref BITMAPINFOHEADER pBmpInfo_input = ref pTransform.video.tv.vh_decompress.bih;

			int iSampleTimeInMs = (int)pPkt->head.uiSampleTimeInMs;
			uint uiPts = pPkt->head.uiPts; //  2015/02/19
													//
			uint dwTickCnt_doPretrans = pPkt->debugInfo.dwTickCnt_doPretrans;  //  2015/02/24
																						//  2016/12/27
																						//uint  uiEventId_lastRecvd  =  pPkt->decInfo.uiEventId_lastRecvd;
																						//
			//TCHAR tHint[256] = _T("");

			//  2017/08/17
			if (pIdInfo_sender.ui64Id == pMisCnt.idInfo.ui64Id)
			{
				//
#if __DEBUG__  //  2017/08/16
				traceLog(_T("chkFromPlayerIndex_v: idInfo_sender is myslef. warning"));
#endif
			}


			//  2014/04/22
#if true
		//  2014/04/22		
		int  nIntervalInMs  =  40;		
		int		iFrameRate  =  (int)pTransform.video.decInfo.dFrameRate_dec;
		if  (  0!=iFrameRate  )  {
			nIntervalInMs  =  1000  /  iFrameRate;
		}		

		//									
		//		
		uint  dwCurTickCnt  =  myGetTickCount(null  );
		//
		int	iNextIndex_preDec  =  0;
		int  lDiff_pre_post  =  0;
		{
				using (CQySyncObj syncObj = new CQySyncObj())
				{
					;
					//TCHAR		tName[128];
					TMP_tBuf128 tName;
					int cntof_tName = Consts.cntof_tBuf128;

					//_sntprintf(  tName,  mycountof(  tName  ),  _T(  "%s%d"  ),  CONST_mutexName_syncIndex_dec,  pTransform.iTaskId  );
					if (0!=syncObj.sync(new string(tName.tBuf), pMisCnt.hMutex_syncIndex_dec)) goto errLabel;

					iNextIndex_preDec = pTransform.video.decInfo.iNextIndex_preDec;
					lDiff_pre_post = pTransform.video.decInfo.lDiff_pre_post;

					if (pTransform.video.decInfo.lDiff_pre_post <= 0)
					{
					}
					else
					{
						pTransform.video.decInfo.lDiff_pre_post--;
					}
				}
		}
		//				
		int  iNextIndex_postDec  =  0;
		//
		if  (  lDiff_pre_post  <=  0  ||  lDiff_pre_post  >  Consts.MAX_cnt_preDec_array  )  {
				/*
			_sntprintf(  tHint,  mycountof(  tHint  ),  _T(  "chkFromPlayerIndex_v err: lDiff_pre_post %d"  ),  lDiff_pre_post  );				
			//goto  errLabel;
			showInfo_open(  0,  0,  tHint  );
			tHint[0]  =  0;
				*/
			}
		else  {
			  iNextIndex_postDec  =  (int)(Consts.MAX_cnt_preDec_array  +  iNextIndex_preDec  -  lDiff_pre_post);
		}
		//
		iNextIndex_postDec  =  iNextIndex_postDec  %  Consts.MAX_cnt_preDec_array;

		//		
		iSampleTimeInMs  =  pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iNextIndex_postDec];
		pPkt->head.uiSampleTimeInMs  =  (uint)iSampleTimeInMs;
		//  2015/02/19
		uiPts  =  pTransform.video.decInfo.uiPts_preDec_array[iNextIndex_postDec];
		pPkt->head.uiPts  =  uiPts;
		//  2015/02/24
		dwTickCnt_doPretrans  =  pTransform.video.decInfo.dwTickCnt_doPretrans[iNextIndex_postDec];
		pPkt->debugInfo.dwTickCnt_doPretrans  =  dwTickCnt_doPretrans;
		
		//	
		//TCHAR	tBuf[256]  =  _T(  ""  );			
		int  e_doPretrans  =  0;
		//
		if  (  pProcInfo.cfg.debugStatusInfo.ucbShowPostDecVStatus  )  {	
				/*
			int  iLastIndex_preDec  =  0;							  
			if  (  !iNextIndex_preDec  )  iLastIndex_preDec  =  mycountof(  pTransform.video.decInfo.iSampleTimeInMs_preDec_array  )  -  1;
			else  iLastIndex_preDec  =  iNextIndex_preDec  -  1;							   

			int  st_preDec  =  pTransform.video.decInfo.iSampleTimeInMs_preDec_array[iLastIndex_preDec];
			//  2015/02/24
			e_doPretrans  =  GetTickCount(  )  -  pPkt->debugInfo.dwTickCnt_doPretrans;
			//
			_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "st_po %dms. d_po %dms. st_pre %dms, d_pre_po %dms. i_pre %d, i_po %d, lDiff_pre_post %d. "  ),  iSampleTimeInMs,  iSampleTimeInMs -  pTransform.video.decInfo.iSampleTimeInMs_postDec,  
				st_preDec,  st_preDec  -  iSampleTimeInMs,  iLastIndex_preDec,  iNextIndex_postDec,  lDiff_pre_post  );
				*/
		}
		
		//
		//  2015/02/25
		//
		if  (  lDiff_pre_post  <  pTransform.video.decInfo.lowLatency.min_lDiff_pre_post  			
			||  Math.Abs(  (  int  )(  dwCurTickCnt  -  pTransform.video.decInfo.lowLatency.dwTickCnt_start  )  )  >  15000  )  
		{
			pTransform.video.decInfo.lowLatency.min_lDiff_pre_post  =  (int)lDiff_pre_post;
			pTransform.video.decInfo.lowLatency.dwTickCnt_start  =  dwCurTickCnt;
		}
		//
		//
		bool  bNeedReset_lDiff_pre_post  =  false;
		if  (  pTransform.video.decInfo.lowLatency.min_lDiff_pre_post  >  1
			&&  pTransform.video.decInfo.lowLatency.min_lDiff_pre_post  ==  lDiff_pre_post
			&&  dwCurTickCnt  -  pTransform.video.decInfo.lowLatency.dwTickCnt_start  >  5000  )  
		{
				bNeedReset_lDiff_pre_post  =  true;
				//
				//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "chkFromPlayerIndex_v: min_lDiff_pre_post %d not 0, trigger to reset"  ),  pTransform.video.decInfo.lowLatency.min_lDiff_pre_post  );				
			
		}
		//
		if  (  bNeedReset_lDiff_pre_post  )  {
				using (CQySyncObj syncObj = new CQySyncObj())
				{
					;
					//TCHAR		tName[128];
					TMP_tBuf128 tName;
					int cntof_tName = Consts.cntof_tBuf128;

					//
					int lDiff_pre_post_resetVal = 0;    //  1

					//
					//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "chkFromPlayerIndex_v: lDiff_pre_post %d too big, set to 0"  ),  pTransform.video.decInfo.lDiff_pre_post  );				

					//_sntprintf(  tName,  mycountof(  tName  ),  _T(  "%s%d"  ),  CONST_mutexName_syncIndex_dec,  pTransform.iTaskId  );
					if (0!=syncObj.sync(new string(tName.tBuf), pMisCnt.hMutex_syncIndex_dec)) goto errLabel;

					pTransform.video.decInfo.lDiff_pre_post = lDiff_pre_post_resetVal;
				}
		}

		//
		pTransform.video.decInfo.iSampleTimeInMs_postDec  =  iSampleTimeInMs;				
		pTransform.video.decInfo.cnt_postDec  ++  ;
		//  2014/12/06
		pTransform.video.decInfo.dwLastTickCnt_postDec  =  dwCurTickCnt;
		//		
		//  2015/02/24
		if  (  pProcInfo.cfg.debugStatusInfo.ucbShowPostDecVStatus  )  {
				/*
			_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s, cnt_po %d, e_doPretrans %d. pts %d"  ),  tBuf,  pTransform.video.decInfo.cnt_postDec,  e_doPretrans,  uiPts  );
			qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo.who_showInfo,  0,  _T(  "postDec:"  ),  _T(  ""  ),  _T(  "%s"  ),  tBuf  );							   
				*/
		}


#endif

			//  2016/12/27
			//uint  uiEventId_lastRecvd_unused  =  0;

			//
			//  		
			return tmp_chkFromPlayerIndex_v(uiTranNo_openAvDev, usIndex, ref pBmpInfo_input, iSampleTimeInMs, 0, ref pIdInfo_sender, pMisCnt, ref pIdInfo_peer, iTaskId, pTransform, ref pTransform.iIndex_player, pTask, pPkt);

		errLabel:

			if (0!=iErr)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T("postDec failed:"), _T(""), _T("%s"), tHint);
			}

			return iErr;

		}


#endif

		//
		public static unsafe int chkFromPlayerIndex_v(uint uiTransformType, object pQY_TRANSFORM, int  iSampleTimeInMs)
        {
			int iErr = -1;

			if (uiTransformType != Consts.CONST_uiTransformType_dec) return -1;

			//if  (  !pQdcObjInfo  )  return  -1;
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;    //  pQdcObjInfo->cfg.pParent_transform;
			if (null == pTransform) return -1;

			//
			uint dwCurTickCnt = myGetTickCount(null);

			//
			pTransform.video.decInfo.iSampleTimeInMs_postDec = iSampleTimeInMs;
			pTransform.video.decInfo.cnt_postDec++;
			//  2014/12/06
			pTransform.video.decInfo.dwLastTickCnt_postDec = dwCurTickCnt;

			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			do
			{
				int index_taskInfo = getQmcTaskInfoIndexBySth(null, pProcInfo, pTransform.iTaskId);
				QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
				if (pTaskInfo == null) break;
				if (!pTaskInfo.bUsed) break;
				QMC_taskData_common taskData = pTaskInfo.getTaskData();

				//
				taskData.tmpStatus.lastTickCnt_dec = myGetTickCount(null);

				//
			} while (false);


            //
            iErr = 0;
			return iErr;
        }


	}
}



