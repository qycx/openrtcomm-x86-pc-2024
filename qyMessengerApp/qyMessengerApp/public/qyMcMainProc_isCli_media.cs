using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

partial class Consts
{
    //  2015/09/14
    public const int CONST_pMsgs_media = 10;


}


namespace qy
{
    partial class qyFuncs
    {
        //
        public static unsafe int drawVideoData(CCtxQmc pProcInfo, myDRAW_VIDEO_DATA* pkts, int pktsLen, bool* pbPktsRedirected, QY_TRANSFORM pTransform)
        {
            int iErr = -1;
            if (null == pProcInfo) return -1;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;

            if (null == pTransform) return -1;
            int iIndex_player = pTransform.iIndex_player;



            int i;
            int nPkts = 0;
            nPkts = pktsLen / sizeof(myDRAW_VIDEO_DATA);
            if (0 == nPkts
                || 0 != (pktsLen % sizeof(myDRAW_VIDEO_DATA)))
            {
#if __DEBUG__
				traceLog(_T("drawVideoData pktsLen err"));
#endif
                goto errLabel;
            }

            //  2015/09/06
            uint dwTickCnt = myGetTickCount(null);

            //
            for (i = 0; i < nPkts; i++)
            {

                myDRAW_VIDEO_DATA* p;
                int len;

                //
                p = &pkts[i];
                int iSampleTimeInMs = (int)p->head.uiSampleTimeInMs;
                BITMAPINFOHEADER* pBmpInfo_input = (BITMAPINFOHEADER*)&p->bih;
                byte* pInput = (byte*)p->memory.m_pBuf;
                uint inputLen = (uint)p->bih.biSizeImage;

                if (inputLen > pProcInfo.cfg.uiBufSize_myDrawVideoData)
                {
                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("drawVideoData failed: inputLen %d is too big"), inputLen);
                    goto errLabel;
                }
                if (inputLen != pBmpInfo_input->biSizeImage)
                {
#if __DEBUG__
					traceLog(_T("drawVideoData err: inputLen err"));
#endif
                    goto errLabel;
                }

                //  len  =  offsetof(  myDRAW_VIDEO_DATA,  buf  )  +  pBmpInfo_input->bmiHeader.biSizeImage;
                len = sizeof(myDRAW_VIDEO_DATA);

                //  
                p->head.uiTranNo_openAvDev = pTransform.video.tv.uiTranNo_openAvDev;   //  2015/10/01	//  Player->video.uiTranNo;	//  2014/11/26
                                                                                       //
                                                                                       //  p->uiCapType  =  0;
                                                                                       //  p->iIndex_capBmp  =  0;
                                                                                       //		 	
                p->iTaskId = pTransform.iTaskId;   //  2015/10/01.	//  pPlayer->iTaskId;

                //
                p->head.uiSampleTimeInMs = (uint)iSampleTimeInMs;
                //  p->head.uiSampleTimeInMs_org  =  iSampleTimeInMs_org;

                //  2015/09/06
                p->debugInfo.dwTickCnt_drawVideoData = dwTickCnt;
            }

#if false  //  def  __DEBUG__
		for  (  i  =  0;  i  <  nPkts;  i  ++  )  {
			if  (  !pkts[i].hWnd_task
				&&  !pkts[i].idInfo_peer.ui64Id  )
			{
#if __DEBUG__
						traceLog(  _T(  "drawVideoData: err, pkts[%d].hWnd_task null and idInfo_peer  is 0"  ),  i  );
#endif

			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("drawVideoData: err, pkts[%d].hWnd_task null and idInfo_peer  is 0"), i);
		}
	}
	//  traceLog(  _T(  "drawVideoData: nPkts %d, iIndex_player %d"  ),  nPkts,  iIndex_player  );
#endif

            //
            QY_PLAYER pPlayer;
            pPlayer = getPlayerByIndex(pProcInfo, iIndex_player);
            if (null == pPlayer) goto errLabel;

            /*
            if  (  !pPlayer.video.bRunning  )  {
                #if  __DEBUG__
                        traceLog(_T(  "drawVideoData failed: play.video.bRuning is false"  )  );
                #endif
                goto  errLabel;
            }
            */

            //
            if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
            {
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("drawVideoData : me, %d,%d plays"), pPlayer->video.uiTranNo, 0);
            }


            //  这里转发的是图像数据的地址，在mcThreadProc_playVideo和其他处理里，要注意这是地址，并且，在接力棒的最后一棒，要注意释放掉。
            if (null == pbPktsRedirected || *pbPktsRedirected)
            {
#if __DEBUG__
	traceLogA("drawVideoData: pbPktRedirected error");
#endif
                goto errLabel;
            }
            //
            if (pPlayer.video.m_var.status.bNoMsgAndWait)
            {
                if (0 != q2PostMsgAndTrigger((IntPtr)pkts, (uint)pktsLen, pPlayer.video.q2)) goto errLabel;
            }
            else
            {
                if (0 != q2PostMsg((IntPtr)pkts, (uint)pktsLen, pPlayer.video.q2)) goto errLabel;
            }
            //  pkt redirected
            *pbPktsRedirected = true;

            iErr = 0;

        errLabel:


            //  if  (  pData_allocated  )  free(  pData_allocated  );

            return iErr;
        }


        //
        public static unsafe int postToDraw(CCtxQmc pProcInfo, uint uiTranNo, myDRAW_VIDEO_DATA* pPkt, int pktLen, bool* pbPktRedirected, uint uiCapType, int iIndex_capBmp, QY_MESSENGER_ID pIdInfo_sender, MIS_CNT pMisCnt, QY_MESSENGER_ID* pIdInfo_peer, int iTaskId, object hWnd_task, PLAYER_ID* pPlayerId)
        {
            int iErr = -1;
            //if (!pProcInfo) return -1;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;

            bool bDbg = false;
            string str;

            //  2016/02/18
            if (null == pPlayerId) return -1;
            int index_player = pPlayerId->index_player;
            //
            if (index_player < 0 || index_player >= pProcInfo.av.usCnt_players) return -1;
            QY_PLAYER pPlayer = pProcInfo.av.pPlayers[index_player];
            PLAY_VIDEO_procInfo pPlay = pPlayer.video;

            //  2015/10/01
            ushort usIndex = pPkt->usIndex_activeMems_from;


            //  CQyQ2Help					q2Help;
            object hWnd_display = null;

            int iSampleTimeInMs = (int)pPkt->head.uiSampleTimeInMs;
            //uint				uiPts						=	pPkt->head.uiPts;		//  2015/02/19
            //
            if (pPkt->usPktResType != Consts.CONST_pktResType_sharedTex)
            {
                if (null == pPkt->memory.m_pBuf)
                {
#if __DEBUG__
					traceLogA("postToDraw failed: pkt.pBuf is null");
#endif
                    showInfo_open(0, ("postToDraw"), (" failed, pkt.pBuf is null"));
                    return -1;
                }
            }
            //
            BITMAPINFOHEADER* pBmpInfo_input = (BITMAPINFOHEADER*)&pPkt->bih;
            //if (!pBmpInfo_input) return -1;


#if __DEBUG__
			if (pProcInfo.cfg.debugStatusInfo.ucbPrint_v_draw)
			{
				traceLog(_T("postToDraw: SampleTime %d, dwTickCnt %d, SampleTime_newRecvd %d"), iSampleTimeInMs, GetTickCount(), pPlay.m_var.uiSampleTimeInMs_newRecvd);
			}
#endif

            //  2013/07/03
#if true   //         
            //
            if (pPlay.m_var.status.fillD3d.bFilled_d3dData)
            {
                dyn_fillData_d3d_setDataReady(pPkt, index_player);
            }
#endif

            //  2013/10/04	
            bool bD3dOnly = false;
            /*
			GBuf_d3d_isCli* pGBuf = (GBuf_d3d_isCli*)pQyMc.pGBuf_d3d;
			if (pGBuf)
			{
				if (hWnd_task == pGBuf->hWall_d3d)
				{
					bD3dOnly = true;
				}
			}
			*/

            if (!bD3dOnly)
            {   //  2013/10/04

                //
                MIS_MSG_procVideo_qmc msg;
                mymemset((IntPtr)(byte*)&msg, 0, sizeof(MIS_MSG_procVideo_qmc));
                msg.uiType = Consts.CONST_misMsgType_procVideo_qmc;
                //  msg.ucbLocalAv  =  true;
                msg.uiCapType = uiCapType;
                msg.iIndex_capProcInfo = iIndex_capBmp;

                //  2009/04/25
                //if (pIdInfo_sender!=null) 
                msg.idInfo_sender.ui64Id = pIdInfo_sender.ui64Id;
                //  2014/09/04
                //msg.dynBmp.usIndex_obj  =  pPkt->decInfo.dynBmp.usIndex_obj;
                //
                msg.uiTranNo_openAvDev = uiTranNo;
                msg.usIndex = usIndex;

                //
                msg.iTaskId = iTaskId;
                //msg.hWnd_task = hWnd_task;      //  2009/09/14
                //  2016/12/27
                //msg.uiEventId_lastRecvd  =  uiEventId_lastRecvd;

                //
                //if (pMisCnt) msg.pMisCnt = pMisCnt;
                if (null != pIdInfo_peer) msg.idInfo_peer.ui64Id = pIdInfo_peer->ui64Id;

                mymemcpy((IntPtr)(byte*)&msg.bih, (IntPtr)pBmpInfo_input, (uint)sizeof(BITMAPINFOHEADER));

                //  2013/10/08
                //  msg.ucbAlphaed  =  pPkt->ucbAlphaed;

                //
                //  msg.data  =  pData_allocated  =  (  byte  *  )mymalloc(  msg.bih.biSizeImage  );
                /*
				msg.data  =  (  byte  *  )mallocObj.malloc(  msg.bih.biSizeImage  );
				if  (  !msg.data  )  goto  errLabel;
				memcpy(  msg.data,  pInput,  pBmpInfo_input->bmiHeader.biSizeImage  );
				*/
                if (pPkt->usPktResType != Consts.CONST_pktResType_sharedTex)
                {
                    if (pPkt->uiSize_dec < msg.bih.biSizeImage)
                    {
#if __DEBUG__
						traceLog(_T("postToDraw: pkt.uiSize_dec error, pPkt->uiSize_dec %d < biSizeImage %d"), pPkt->uiSize_dec, msg.bih.biSizeImage);
#endif
                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("postToDraw: pkt.uiSize_dec error, pPkt->uiSize_dec %d < biSizeImage %d"), pPkt->uiSize_dec, msg.bih.biSizeImage);
                        goto errLabel;
                    }
                }
                //  
                //  memcpy(  &msg.memory,  &pPkt->memory,  sizeof(  msg.memory  )  );
                mymemcpy((IntPtr)(byte*)&msg.pkt, (IntPtr)pPkt, (uint)sizeof(myDRAW_VIDEO_DATA));

                //
                //msg.playerId.index_player  =  index_player;		//  2009/10/03
                msg.playerId = *pPlayerId;                      //  2016/02/19

                //  2012/11/09
                //M_msg_setNo(*pPkt, msg);

                /*
				//  to get hWnd_display		
				if (hWnd_task && IsWindow(hWnd_task))
				{       //  2009/09/09
					hWnd_display = hWnd_task;
				}
				else if (!msg.idInfo_peer.ui64Id && msg.uiCapType)
				{
					switch (msg.uiCapType)
					{
						case CONST_capType_av:
							{
								CAP_procInfo_video* pVc;
								pVc = (CAP_procInfo_video*)getCapBmpBySth(pProcInfo, msg.iIndex_capProcInfo, 0);
								if (!pVc || pVc->uiType != msg.uiCapType) break;
								if (pVc->curhWnd)
								{
									if (!IsWindow(pVc->curhWnd)) pVc->curhWnd = NULL;
									else hWnd_display = pVc->curhWnd;
								}
							}
							break;
						case CONST_capType_mediaFile:
							{
								CAP_procInfo_mediaFileBmp* pBc;
								pBc = (CAP_procInfo_mediaFileBmp*)getCapBmpBySth(pProcInfo, msg.iIndex_capProcInfo, 0);
								if (!pBc || pBc->uiType != msg.uiCapType) break;
								if (pBc->curhWnd)
								{
									if (!IsWindow(pBc->curhWnd)) pBc->curhWnd = NULL;
									else hWnd_display = pBc->curhWnd;
								}
							}
							break;
						default:
# if  __DEBUG__
							traceLogA("doMainWnd_procVideo: unprocessed capType %d", msg.uiCapType);
#endif
							break;
					}
				}
				else
				{
					if (!IsWindow(pPlay.m_var.hWnd_display))
					{
						HWND hWnd;
						if (findTalker(pQyMc, msg.pMisCnt, &msg.idInfo_peer, &hWnd))
						{
# if  __DEBUG__
							traceLog(_T("postToDraw: findTalker failed, iIndex_player %d, idInfo_peer %I64u."), index_player, msg.idInfo_peer.ui64Id);
#endif
							qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("postToDraw failed: findTalker failed, iIndex_player %d, idInfo_peer %I64u."), index_player, msg.idInfo_peer.ui64Id);
							goto errLabel;
						}
						pPlay.m_var.hWnd_display = hWnd;
					}
					hWnd_display = pPlay.m_var.hWnd_display;
				}
				if (!hWnd_display)
				{
					traceLogA("postToDraw failed. can't get hWnd_display");
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("postToDraw failed. can't get hWnd_display"));
					goto errLabel;
				}
				*/

                //
                if (null == pbPktRedirected || *pbPktRedirected)
                {
                    traceLogA("postToDraw: pbPktRedirected error");
                    goto errLabel;
                }

                bool bSkip = false;
#if DEBUG
                //bSkip = true;
#endif
                //
                if (!bSkip)
                {
                    int nQ2Nodes = getQ2Nodes(pPlay.displayQ2);
                    if (nQ2Nodes > 10)
                    {
                        bSkip = true;
                        str = string.Format("postToDraw: skip the packet if nQNodes {0}>10", nQ2Nodes);
                        showInfo_open(0, null, str);
                    }
                }

                if (!bSkip)
                {
                    //
                    if (0 == q2PostMsgAndTrigger((IntPtr)(byte*)&msg, (uint)sizeof(MIS_MSG_procVideo_qmc), pPlay.displayQ2))
                    {
                        *pbPktRedirected = true;
                    }
                }

                //PostMessage(hWnd_display, CONST_qyWm_postComm, CONST_qyWmParam_guiMsgArrive, index_player);

            }

            //
#if true



            int dwTgt = (int)mytimeGetTime();    //  pPkt->debugInfo.dwTickCnt_postToDraw;	//  timeGetTime(  );
                                                 //
            if (0 != pPlay.m_var.status.dwLastTgt_postToDraw)
            {
                pPlay.m_var.status.nInterval_real_postToDraw = (int)(dwTgt - pPlay.m_var.status.dwLastTgt_postToDraw);
            }
            pPlay.m_var.status.dwLastTgt_postToDraw = (uint)dwTgt;
            uint uiPts_last = pPlay.m_var.status.uiPts_last;
            pPlay.m_var.status.uiPts_last = pPkt->head.uiPts;   //  2015/02/19
                                                                //

            //  2015/09/10
            //
            int nIntervalInMs_fillD3dData = (int)(pPlay.m_var.status.fillD3d.param0.nIntervalInMs_fillData + pPlay.m_var.status.fillD3d.param1.nIntervalInMs_fillData);
            //
            //  2015/09/14
            int nQNodes_left = getQ2Nodes(pPlay.q2);
            //
            /*
            TCHAR  tBuf_node[128]  =  _T(  ""  );
            TCHAR  tBuf_e[128]  =  _T(  ""  );
            int	 nInterval_iSampleTimeInMs  =  iSampleTimeInMs  -  pPlay.m_var.status.iSampleTimeInMs_last;	
            TCHAR  tBuf_other[128]  =  _T(  ""  );
            TCHAR  tBuf_adjust[128]  =  _T(  ""  );
            */
            //
            if (pProcInfo.cfg.debugStatusInfo.ucbShowToDrawStatus)
            {
                /*
		//
		pPlay.m_var.status.iSampleTimeInMs_last  =  iSampleTimeInMs;
		//  2015/09/05
		uint  dwTickCnt  =  GetTickCount(  );
		int  iE_from_read  =  -1;
		if  (  pPkt->debugInfo.dwTickCnt_proc_io_read  )  iE_from_read  =  dwTickCnt  -  pPkt->debugInfo.dwTickCnt_proc_io_read;
		int  iE_from_pretrans  =  -1;
		if  (  pPkt->debugInfo.dwTickCnt_doPretrans  )  iE_from_pretrans  =  dwTickCnt  -  pPkt->debugInfo.dwTickCnt_doPretrans;
		int  iE_from_drawVideoData  =  -1;
		if  (  pPkt->debugInfo.dwTickCnt_drawVideoData  )  iE_from_drawVideoData  =  dwTickCnt  -  pPkt->debugInfo.dwTickCnt_drawVideoData;
		//
		int  iE_from_playPkt_start  =  dwTickCnt  -  pPkt->debugInfo.dwTickCnt_playPkt_start;
		//
		//
		_sntprintf(  tBuf_node,  mycountof(  tBuf_node  ),  _T(  "n_wait %d. bAdj %d. nDelay %d, ah %d, ah_gt %d, bReset %d, mql,xql( %d, %d ), last( %d, %d), ok(%d, %d, %d). nodes %d."  ),  
													pPlay.m_var.status.nTimes_wait,  
													pPlay.m_var.lowLatency.bNeedAdjust,												
													pPlay.m_var.lowLatency.nDelayInMs_lt_mql,
													pPlay.m_var.lowLatency.nAheadInMs,  pPlay.m_var.lowLatency.nAheadInMs_gt_mql,  
													//
													pPlay.m_var.lowLatency.mql.bNeedReset,
													pPlay.m_var.lowLatency.mql.mql,   
													pPlay.m_var.lowLatency.mql.xql,
													pPlay.m_var.lowLatency.mql.last_mql,	
													pPlay.m_var.lowLatency.mql.last_xql,
													//
													pPlay.m_var.lowLatency.mql.mql_ok,
													pPlay.m_var.lowLatency.mql.mql_big_ok,
													pPlay.m_var.lowLatency.mql.xql_ok,
													//
													nQNodes_left
													);
		//
# if  __DEBUG__
		//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s tmp_nInt %d, nAvg_iDiff %d, nKk %d"  ),  tBuf,  pPlay.m_var.status.proc_stat.tmp_nInt,  pPlay.m_var.status.proc_stat.nAvg_iDiff,  pPlay.m_var.status.proc_stat.nKk  );
#endif
			//
			//
				*/
            }
#endif

            //  2013/09/09
            if (pPlay.m_var.status.fillD3d.bFilled_d3dData)
            {
                if (0 != pPlay.m_var.lowLatency.dwToInMs_toPostDraw) pPlay.m_var.status.ucSkipped_for_d3dData = 0;
                else
                {
                    pPlay.m_var.status.ucSkipped_for_d3dData = (byte)(nIntervalInMs_fillD3dData / 16);
                    if (0 != pPlay.m_var.status.ucSkipped_for_d3dData) pPlay.m_var.status.ucSkipped_for_d3dData--;
                }
            }


            ////////////////////////////	
            //
            int tmp_player_usFps = pPlay.usFps;
            if (0 == tmp_player_usFps) tmp_player_usFps = (int)pPlay.m_var.status.fpsInfo.fps_real;
            if (0 == tmp_player_usFps) tmp_player_usFps = 1;
            //TCHAR* tWhere = pPlay.m_var.tWhere;
            {
                //	
                const int CONST_nIntervalInMs_toAdjust_mql_good = 1000; //  1000	//  1500	//  5000				 									 			
                                                                        //const int 		CONST_nIntervalInMs_toAdjust_mql_worst						3000	//  1000	//  1500	//  5000				 									 			

                //  2015/03/02											
                const int M_xql_ok = 25;    //  3		
                const int M_mql_big_ok = 6;
                const int M_mql_ok = 3;
                //
                const int M_xql_ok_sharedTex = 50;  //  3				
                const int M_mql_big_ok_sharedTex = 4;
                const int M_mql_ok_sharedTex = 2;
                //
                const int MIN_mql_ok_sharedTex = 3;

                //
                const int M_xql_ok_15fps = 10;  //  25	//  10	//  1				
                const int M_mql_big_ok_15fps = 4;   //  6
                const int M_mql_ok_15fps = 2;   //  1				


                //											
                int tmp_xql_ok = M_xql_ok_15fps;
                int tmp_mql_big_ok = M_mql_big_ok_15fps;
                int tmp_mql_ok = M_mql_ok_15fps;
                if (tmp_player_usFps > 15)
                {
                    if (pPkt->usPktResType == Consts.CONST_pktResType_sharedTex)
                    {
                        /*
			tmp_xql_ok = M_xql_ok_sharedTex * tmp_player_usFps / 25;
			tmp_mql_big_ok = M_mql_big_ok_sharedTex * tmp_player_usFps / 25;
			tmp_mql_ok = M_mql_ok_sharedTex * tmp_player_usFps / 25;
			//
			if (pPkt->sharedTex.pktSharedTexInfo.usMax_intervalInMs_renderFrame)
			{
				//
				int frameTimeInMs = 1000 / tmp_player_usFps;
				if (!frameTimeInMs) frameTimeInMs = 40;
				//
				int nNeeded = M_get_ucMaxCnt(pPkt->sharedTex.pktSharedTexInfo.usMax_intervalInMs_renderFrame, frameTimeInMs);
				if (pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes <= nNeeded)
				{
					int nDiff = 12; //  10;
					if (pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes > nDiff)
					{
						nNeeded = pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes - nDiff;
					}
				}
				//
				if (tmp_mql_ok < nNeeded) tmp_mql_ok = nNeeded;
				//
				if (tmp_mql_ok > 10) tmp_mql_big_ok = tmp_mql_ok + 1;
				else tmp_mql_big_ok = tmp_mql_ok + 1;
				//
				if (pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes > 3)
				{
					tmp_xql_ok = pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes - 3;
				}
			}
						*/
                    }
                    else
                    {
                        tmp_xql_ok = M_xql_ok * tmp_player_usFps / 25;
                        tmp_mql_big_ok = M_mql_big_ok * tmp_player_usFps / 25;
                        tmp_mql_ok = M_mql_ok * tmp_player_usFps / 25;
                    }
                }
                //

                //
                //  2015/03/12. 对本地视频，为了尽可能快一点，所以要求最低残余包数应到0
                if (0 == pPlayer.idInfo_recorder.ui64Id)
                {
                    if (!pPkt->input.bRtsp)
                    {               //  2015/03/12. 只有纯本地采集卡才能降到0. 
                        tmp_xql_ok = 0;
                        tmp_mql_big_ok = 0;
                        tmp_mql_ok = 0;
                    }
                }

                //  2015/09/16
                if (pPlay.m_var.lowLatency.mql.xql_ok != tmp_xql_ok)
                {
                    pPlay.m_var.lowLatency.mql.xql_ok = tmp_xql_ok;
                }
                if (pPlay.m_var.lowLatency.mql.mql_ok != tmp_mql_ok)
                {
                    pPlay.m_var.lowLatency.mql.mql_ok = tmp_mql_ok;
                }
                if (pPlay.m_var.lowLatency.mql.mql_big_ok != tmp_mql_big_ok)
                {
                    pPlay.m_var.lowLatency.mql.mql_big_ok = tmp_mql_big_ok;
                }

                //  2015/09/15
                int nIntervalInMs_toAdjust_mql = CONST_nIntervalInMs_toAdjust_mql_good;


                //  2015/02/25		
                uint dwCurTgt = (uint)dwTgt;
                if (Math.Abs((long)(dwCurTgt - pPlay.m_var.lowLatency.mql.dwTgt_start1)) > nIntervalInMs_toAdjust_mql)
                {
                    //  2015/09/15
                    //_sntprintf(  tBuf_other,  mycountof(  tBuf_other  ),  _T(  "INT_ADJ %d. "  ),  nIntervalInMs_toAdjust_mql  /  1000  );
                    //
                    if (pPlay.m_var.lowLatency.mql.mql > tmp_mql_ok)
                    {
                        pPlay.m_var.lowLatency.mql.bNeedReset = true;
                        //				
                        //_sntprintf(tBuf_adjust, mycountof(tBuf_adjust), _T("Adj: mql > mql_ok %d."), tmp_mql_ok);
                    }
                    //
                    pPlay.m_var.lowLatency.mql.last_xql = pPlay.m_var.lowLatency.mql.xql;
                    pPlay.m_var.lowLatency.mql.last_mql = pPlay.m_var.lowLatency.mql.mql;
                    //			
                    pPlay.m_var.lowLatency.mql.mql = nQNodes_left;
                    pPlay.m_var.lowLatency.mql.xql = nQNodes_left;
                    pPlay.m_var.lowLatency.mql.dwTgt_start1 = dwCurTgt;
                    //

                }
                else
                {
                    if (nQNodes_left < pPlay.m_var.lowLatency.mql.mql)
                    {
                        pPlay.m_var.lowLatency.mql.mql = nQNodes_left;
                    }
                    if (nQNodes_left > pPlay.m_var.lowLatency.mql.xql)
                    {
                        pPlay.m_var.lowLatency.mql.xql = nQNodes_left;
                    }
                    //				
                    if (pPlay.m_var.lowLatency.mql.bNeedReset)
                    {
                        if (nQNodes_left <= tmp_mql_big_ok)
                        {
                            pPlay.m_var.lowLatency.mql.bNeedReset = false;
                            //
                            //_sntprintf(tBuf_adjust, mycountof(tBuf_adjust), _T("Adj ok: ql <= mql_big_ok %d."), tmp_mql_big_ok);
                        }
                    }
                    //
                    if (nQNodes_left > tmp_xql_ok)
                    {
                        pPlay.m_var.lowLatency.mql.bNeedReset = true;
                        //				
                        //_sntprintf(tBuf_adjust, mycountof(tBuf_adjust), _T("Adj: ql > xql_ok %d."), tmp_xql_ok);
                    }

                }
            }

            //

            if (pProcInfo.cfg.debugStatusInfo.ucbShowToDrawStatus)
            {
                /*
	TCHAR ttt[32] = _T("");
	//
	TCHAR tBuf[256];
	_sntprintf(tBuf, mycountof(tBuf), _T("%I64u: pts %d. nInt %d, real %d. "), pIdInfo_sender->ui64Id, pPkt->head.uiPts, nInterval_iSampleTimeInMs, pPlay.m_var.status.nInterval_real_postToDraw);
	//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s st %d"  ),  tBuf,  iSampleTimeInMs  );
	if (pPkt->usPktResType == CONST_pktResType_sharedTex)
	{
		int real_index = 0;
		if (pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes) real_index = pPkt->sharedTex.pktSharedTexInfo.uiSeqNo % pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes;
		_sntprintf(tBuf, mycountof(tBuf), _T("%s maxTexes %d, shrTex.sn %d, ind %d, maxInt_dec %d."), tBuf, (int)pPkt->sharedTex.pktSharedTexInfo.ucMaxCnt_sharedTexes, pPkt->sharedTex.pktSharedTexInfo.uiSeqNo,
			real_index,
			(int)pPkt->sharedTex.pktSharedTexInfo.usMax_intervalInMs_renderFrame);
	}
	_sntprintf(tBuf, mycountof(tBuf), _T("%s %s"), tBuf, pPlay.m_var.lowLatency.bNoAudio ? _T("NoAudio") : _T("Audio"));
	_sntprintf(tBuf, mycountof(tBuf), _T("%s n_slp %d, to %d. "), tBuf, pPlay.m_var.lowLatency.nTimes_sleep, pPlay.m_var.lowLatency.dwToInMs_toPostDraw);
	_sntprintf(tBuf, mycountof(tBuf), _T("%s %s, %s %s %s %s"), tBuf, tBuf_e, tBuf_other, tBuf_adjust, tBuf_node, ttt);
	qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T("postToDraw: "), tBuf);

	//  2017/10/13
	if (uiPts_last && uiPts_last + 1 != pPkt->head.uiPts)
	{
		_sntprintf(tBuf, mycountof(tBuf), _T("%I64u: pts_missing, uiPts_last %d, pkt.pts %d"), pIdInfo_sender->ui64Id, uiPts_last, pPkt->head.uiPts);
		showInfo_open(0, _T("postToDraw: "), tBuf);
	}
				*/
            }

            //
            //  2016/04/10
            if (pPlay.m_var.lowLatency.bNeedAdjust)
            {
                int nDelay = pPlay.m_var.lowLatency.nDelayInMs_lt_mql;
                if (nDelay > 0)
                {
                    nDelay = Math.Min(nDelay, 20);
                    if (0 != pPlay.m_var.lowLatency.calc.dwTgt_start)
                    {
                        pPlay.m_var.lowLatency.calc.dwTgt_start += (uint)nDelay;
                        //pPlay.m_var.status.dwLastTgt_postToDraw  +=  nDelay;
                    }
                }
                int nAh = pPlay.m_var.lowLatency.nAheadInMs + pPlay.m_var.lowLatency.nAheadInMs_gt_mql;
                if (nAh > 0)
                {
                    //
                    //
                    nAh = Math.Min(nAh, 100);
                    //
                    if (pPlay.m_var.lowLatency.calc.dwTgt_start > nAh)
                    {
                        pPlay.m_var.lowLatency.calc.dwTgt_start -= (uint)nAh;
                        //pPlay.m_var.status.dwLastTgt_postToDraw  -=  nAh;
                    }
                }
            }


            //
            //  2016/04/10	
            if (pPlay.m_var.lowLatency.bAccurate)
            {
                if (0 == pPlay.m_var.lowLatency.calc.dwTgt_start
                    || pPkt->head.uiPts != uiPts_last + 1)
                {
                    pPlay.m_var.lowLatency.calc.dwTgt_start = (uint)dwTgt;
                    pPlay.m_var.lowLatency.calc.uiPts_start = pPkt->head.uiPts;

                    //			
#if __DEBUG__
		if (!pPlay.m_var.lowLatency.calc.dwTgt_start)
		{
			int ii = 1;
		}
#endif

                    //
                    //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, "", pProcInfo.str_who_showInfo, 0, pPlay.m_var.tWhere, _T(""), _T("playVideo: calc_start reset"));
                }
            }



            //
            iErr = 0;

        errLabel:




            return iErr;
        }


        //
        public static unsafe int getBih_capBmpToPlay(CAP_procInfo_bmpCommon pCapBmp, BITMAPINFOHEADER* pBih)
        {
            if (null == pCapBmp || null == pBih) return -1;

            //if  (  !pCapBmp->common.compressVideo.bVideoCompressInited  )  return  -1;
            //
            if (pCapBmp.compressVideo.ucbResizeByEncoder)
            {
                makeBmpInfoHeader_rgb(24, pCapBmp.vh_org.bih.biWidth, pCapBmp.vh_org.bih.biHeight, pBih);
            }
            else
            {
                //memcpy(pBih, &pCapBmp->common.compressVideo.vh_decompress.bih, sizeof(pBih[0]));
                *pBih = pCapBmp.compressVideo.vh_decompress.bih;
            }
            return 0;
        }


        //
        public static unsafe int drawLocalVideoData(CCtxQmc pProcInfo, uint uiTranNo, myDRAW_VIDEO_DATA* pkts, int pktsLen, bool* pbPktsRedirected, uint uiCapType, int iIndex_capBmp, int iTaskId, MIS_CNT pMisCnt, QY_MESSENGER_ID* pIdInfo_peer)
        {
            int iErr = -1;
            if (null == pProcInfo) return -1;
            CCtxQyMc pQyMc = g.g_pQyMc;

            if (null == pkts || 0 == pktsLen) return -1;
            int nPkts = pktsLen / sizeof(myDRAW_VIDEO_DATA);
            if (0 == nPkts) return -1;

            ref BITMAPINFOHEADER pBmpInfo_input = ref pkts[0].bih;
            bool bDbg = false;// true;

            //
            string str;
            str = string.Format("Warning: drawLocalVideoData: unused.");
            qyFuncs.showInfo_open(0, null, str);
            return -1;


            //
            if (uiTranNo == 0)
            {
                traceLog("drawLocalVideoData faild, uiTranNo is 0");
                return -1;
            }
            if (0 == pBmpInfo_input.biWidth
                || 0 == pBmpInfo_input.biHeight
                || 0 == pBmpInfo_input.biSizeImage)
            {
#if __DEBUG__
				traceLog(_T("drawLocalVideoData failed, pBmpInfo_input err"));
#endif
                return -1;
            }

            CAP_procInfo_bmpCommon pCapBmp = null;
            QY_SHARED_OBJ pSharedObj = null;
            CAP_procInfo_audioCommon pCapAudio = null;

            pCapBmp = getCapBmpBySth(pProcInfo, iIndex_capBmp, 0);
            if (null == pCapBmp) goto errLabel;
            pSharedObj = getSharedObjByIndex(pProcInfo, pCapBmp.iIndex_sharedObj);
            if (null == pSharedObj) goto errLabel;
            pCapAudio = getCapAudioBySth(pProcInfo, pSharedObj.var.iIndex_capAudio, 0);


            SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
            if (null == pSharedObjUsr) goto errLabel;

#if DEBUG
            if (true)
            {
                //  SampleTime  =  0;
                if (0 != (pktsLen % sizeof(myDRAW_VIDEO_DATA)))
                {
                    traceLogA("test here");
                }
                //
            }

#endif


            QY_PLAYER pPlayer = null;

            pPlayer = getPlayerByIndex(pProcInfo, pSharedObj.var.iIndex_localPlayer);
            if (pPlayer != null)
            {
                if (pPlayer.idInfo_recorder.ui64Id != 0
                    || pPlayer.video.uiTranNo != uiTranNo)
                {
                    traceLogA("drawLocalVideoData: not correct player");
                    pSharedObj.var.iIndex_localPlayer = -1;
                    pPlayer = null;
                }
            }
            if (null == pPlayer)
            {
                int k;

                for (k = 0; k < pProcInfo.av.usCnt_players; k++)
                {
                    if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == 0
                         && pProcInfo.av.pPlayers[k].video.uiTranNo == uiTranNo)
                    {
                        if (null == pCapAudio) break;

                        //  2010/09/04
                        if (pProcInfo.av.pPlayers[k].idInfo_recorder.ui64Id == 0
                            && pProcInfo.av.pPlayers[k].audio.uiTranNo == pSharedObjUsr.uiTranNo_openAvDev_a)
                        {
                            break;
                        }

                    }
                }

                if (k < pProcInfo.av.usCnt_players)
                {
                    pSharedObj.var.iIndex_localPlayer = k;
                    pPlayer = getPlayerByIndex(pProcInfo, k);
                    if (null == pPlayer) goto errLabel;
                }
            }

            if (null == pPlayer || !pPlayer.video.bRunning)
            {

                //  2014/12/10
                uint dwTickCnt = myGetTickCount(null);
                if (dwTickCnt - pSharedObjUsr.dwLastTickCnt_applyForPlayer < 200)
                {
#if __DEBUG__
		traceLog(_T("drawLocalVideoData failed, can't apply for player, too quickly"));
#endif
                    goto errLabel;
                }
                pSharedObjUsr.dwLastTickCnt_applyForPlayer = dwTickCnt;

                //
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("drawLocalVideoData : me,%d,%d apply for player. index_capBmp %d"), uiTranNo, 0, iIndex_capBmp);

                //  ÕâÀï×é°ü·¢¸øÖ÷Ïß³Ì¡£
                MIS_MSG_applyForPlayer_qmc msg;
                mymemset((IntPtr)(byte*)&msg, 0, sizeof(MIS_MSG_applyForPlayer_qmc));
                msg.uiType = Consts.CONST_misMsgType_applyForPlayer_qmc;
                //  msg.idInfo_recorder.ui64Id  =  0;

                //
                msg.index_sharedObj = pCapBmp.iIndex_sharedObj;
                msg.iTaskId = iTaskId;

                //
                msg.video.uiTranNo = uiTranNo;
                //
                //  memcpy(  &msg.video.vh.bih,  pBmpInfo_input,  sizeof(  msg.video.vh.bih  )  );
                if (0 != getBih_capBmpToPlay(pCapBmp, &msg.video.vh.bih)) goto errLabel;
                //
                //msg.video.ucbFlippedImg  =  pCapBmp->common.compressVideo.compressor.common.ucbOutputFlippedImg;	//  2011/12/11
                //
                msg.video.usFps = pCapBmp.compressVideo.compressor.common.head.usMaxFps_toShareBmp;  //  2015/02/20

                //
                if (null != pCapAudio)
                {
                    //  msg.audio.uiTranNo  =  pCapAudio->common.compressAudio.uiTranNo_openAvDev;
                    msg.audio.uiTranNo = pSharedObjUsr.uiTranNo_openAvDev_a;                           //  2010/09/04
                                                                                                       //memcpy(&msg.audio.ah, &pCapAudio->common.compressAudio.ah_decompress, sizeof(msg.audio.ah));
                    msg.audio.ah = pCapAudio.compressAudio.ah_decompress; ;
                    //
                    //memcpy(&msg.audio.playCfg, &pCapAudio->common.compressAudio.compressor.playCfg, sizeof(msg.audio.playCfg)); //  2012/01/03
                    msg.audio.playCfg = pCapAudio.compressAudio.compressor.playCfg; //  2012/01/03
                }

                //					  
                qPostMsg((IntPtr)(byte*)&msg, (uint)sizeof(MIS_MSG_applyForPlayer_qmc), pQyMc.gui.processQ);

                GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                iErr = 0; goto errLabel;
            }

            if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewAudioStatus)
            {
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("drawLocalVideoData : me, %d,%d plays"), uiTranNo, 0);
            }
            /*
                        string str;
                        str = string.Format("drawLocalVideo: playerId {0},{1}", pSharedObj.iIndex_localPlayer, pPlayer.uiTranNo_player);
                        qyFuncs.traceLog(str);
            */

            //
            int i;
            for (i = 0; i < nPkts; i++)
            {

                myDRAW_VIDEO_DATA* p = &pkts[i];

                p->head.uiTranNo_openAvDev = uiTranNo;
                p->uiCapType = uiCapType;
                p->iIndex_capBmp = iIndex_capBmp;
                p->iTaskId = iTaskId;
                //p->hWnd_task = pSharedObjUsr.hWnd_starter;     //  2009/09/14
                //
                /*
                p->head.uiLen  =  pBmpInfo_input->bmiHeader.biSizeImage;
                p->head.uiSampleTimeInMs  =  iSampleTimeInMs;
                p->head.usIndex  =  0;
                memcpy(  p->buf,  pInput,  p->head.uiLen  );
                */
                if (p->head.uiLen < pBmpInfo_input.biSizeImage)
                {
                    traceLogA("drawLocalVideoData: uiLen error");
                    goto errLabel;
                }

                //
#if DEBUG
                if (bDbg)
                {
                    str = string.Format("drawLocalVideoData: pkt[{0}] pktNo {1}", i, p->debugInfo.lPktNo);
                    traceLog(str);
                }
#endif

                //
                pPlayer.video.m_var.uiSampleTimeInMs_newRecvd = p->head.uiSampleTimeInMs;
                pPlayer.video.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd = myGetTickCount(null);

                if (0 != p->head.uiSampleTimeInMs)
                {
                    if (p->head.uiSampleTimeInMs < pPlayer.video.m_var.uiSampleTimeInMs)
                    {
                        //traceLogA("drawLocalVideoData: pkt.uiSampleTimeInMs %d < player.%d. trigger to empty old data", p->head.uiSampleTimeInMs, pPlayer.video.m_var.uiSampleTimeInMs);
                        q2TriggerToProcess(pPlayer.video.q2);
                    }
                    if (0 != pPlayer.video.m_var.uiSampleTimeInMs)
                    {
                        if (p->head.uiSampleTimeInMs > pPlayer.video.m_var.uiSampleTimeInMs + 10000)
                        {   //  2009/05/25. too old audio data
                            //traceLogA("drawLocalVideoData: pkt.uiSampleTimeInMs %d > player.%d  +  10000. trigger to empty old data", p->head.uiSampleTimeInMs, pPlayer.video.m_var.uiSampleTimeInMs);
                            q2TriggerToProcess(pPlayer.video.q2);
                        }
                    }
                }
            }

#if __DEBUG__
#endif

            //  这里转发的是图像数据的地址，在mcThreadProc_playVideo和其他处理里，要注意这是地址，并且，在接力棒的最后一棒，要注意释放掉。
            if (null == pbPktsRedirected || *pbPktsRedirected)
            {
                traceLogA("drawLocalVideoData: pbPktRedirected error");
                goto errLabel;
            }
            //
            if (pPlayer.video.m_var.status.bNoMsgAndWait)
            {
                if (0 != q2PostMsgAndTrigger((IntPtr)pkts, (uint)pktsLen, pPlayer.video.q2))
                {
                    traceLog("drawLocalVideoData: q2PostMsgAndTrigger failed");
                    goto errLabel;
                }
            }
            else
            {
                if (0 != q2PostMsg((IntPtr)pkts, (uint)pktsLen, pPlayer.video.q2))
                {
                    traceLog("drawLocalVideoData: q2PostMsg failed");
                    goto errLabel;
                }
            }
            //
            *pbPktsRedirected = true;

            iErr = 0;

        errLabel:

            //  if  (  pData_allocated  )  free(  pData_allocated  );

            return iErr;
        }

        public static unsafe int procMsgTask_mc_media(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU pMsgU, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;
            //
            ref MIS_MSG_TASK pMsg = ref pMsgU.task;
            //
            int dataLen = 0;
            uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            ref IM_CONTENTU pContent = ref pMsg.data.buf;
            //  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //  IM_TASK_RCD			rcd;
            int tmpiRet;
            //
            int lenInBytes = 0;
            IM_SIMPLE_RESP respSimple;
            ushort usRespCode = Consts.CONST_qyRc_unknown;
            uint uiChannelType = Consts.CONST_channelType_talking;
            //char timeBuf[CONST_qyTimeLen + 1] = "";
            TMP_timeBuf timeBuf;
            //
            bool bRemoveIfErr = true;       //  ÔÚÔËÐÐ³ö´íÊ±ÊÇ·ñÉ¾³ýÏà¹Ø½Úµã¡£
                                            //
                                            //  TCHAR					sqlBuf[CONST_maxSqlBufLen  +  1]	=	_T(  ""  );
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;


            if (null == pQyMc.gui.hMainWnd) return -1;
            if (null == pMisCnt) return -1;

            //  memset(  &rcd,  0, sizeof(  rcd  )  );

            //  2014/11/22
#if __DEBUG__
			if (pContent->uiType == CONST_imCommType_transferAvInfo)
			{
				int i;
				i = 0;

			}
#endif


            if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
            {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                {
                    switch (pMsg.uiTaskType)
                    {
                        case Consts.CONST_imTaskType_shareDynBmp:
                        case Consts.CONST_imTaskType_shareGps:
                            break;
                        default:
                            {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: getIdInfo_logicalPeer failed.");
#endif
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, NULL, _T(""), _T(""), _T("procMsgTask_mc_media: getIdInfo_logicalPeer failed."));
                                goto errLabel;
                            }
                            break;
                    }
                }
            }


            if (!isUcFlgResp(pMsg.ucFlg))
            {

                if (!bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {       //  Ö÷¶¯·¢³ö

                    switch (pContent.uiType)
                    {

#if false
						case Consts.CONST_imCommType_transferAvInfo:
							if (pContent->transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
							{
#if __DEBUG__
#endif
								goto errLabel;
							}
							TRANSFER_AV_task* pTransferAvTask; pTransferAvTask = (TRANSFER_AV_task*)pContent;

							//
							if (!pCtx.pProcessQ) goto errLabel;

							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo qTraverse_mono failed;");
#endif
								goto errLabel;
							}
							if (!tmpiRet)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, to qPostMsg");
#endif
								if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
								{
									traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
									goto errLabel;
								}
								//
								//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

								//
								if (pTransferAvTask->myTask.ucbVideoConferenceStarter)
								{
									//  ÔÙ²éÑ¯Ò»´ÎprocessQ_media, ÒòÎªÊÓÆµ»áÒéµÄbmpInfo_compressµÈ£¬ÊÇÔÚÈë¶ÓÁÐÊ±²ÅÉú³ÉµÄ¡£Òª´ÓÀïÃæÈ¡³öÀ´
									if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0) goto errLabel;
								}

#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, qPostMsg ok;");
#endif
							}
							//  							

							//  2008/03/21, 
							startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferAvInfo"));

							break;
						case Consts.CONST_imCommType_transferAvReplyInfo:

							if (!pCtx.pProcessQ) goto errLabel;

							//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
							if (pContent->transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
							{
							}
							else
							{
								//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
								tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
								if (tmpiRet < 0)
								{
#if __DEBUG__
									traceLogA("procMsgTask_mc_media: transferAvReplyInfo qTraverse_mono failed.");
#endif
									goto errLabel;
								}
								if (!tmpiRet)
								{
									if (pMsg->iTaskId)
									{       //  2009/09/10
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: transferAvReplyInfo to qPostMsg;");
#endif
										//
										if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
										{
#if __DEBUG__
											traceLogA("procMsgTask_mc_robot: transferAvReplyInfo, qPostMsg failed");
#endif
											goto errLabel;
										}
										//
										//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: avReply ok;");
#endif

										//
										//  2014/11/20								 
										//  post

									}
								}

								startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferAvReplyInfo"));

							}

							break;
#endif
                        case Consts.CONST_imCommType_transferVideoData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("send transferVideoData"));

                                fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param)) <= 0)
                                    {
                                        //traceLogA("procMsgTask_mc_media: ´¦ÀíCONST_imCommType_transferVideoData %s¡£", tmpiRet ? "failed" : "not found");
                                        goto errLabel;
                                    }
                                }

                            }
                            iErr = 0; goto errLabel;
                            break;
                        case Consts.CONST_imCommType_transferAudioData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("send transferAudioData"));

                                fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param)) <= 0)
                                    {
                                        //traceLogA("procMsgTask_mc_media: ´¦ÀíCONST_imCommType_transferAudioData %s¡£", tmpiRet ? "failed" : "not found");
                                        goto errLabel;
                                    }
                                }

                            }
                            iErr = 0; goto errLabel;
                            break;
                        case Consts.CONST_imCommType_taskProcReq:
                            switch (pContent.taskProcReq.usOp)
                            {
                                case Consts.CONST_imOp_send_cancel:
                                case Consts.CONST_imOp_recv_cancel:

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    //  printQ_mis(  &pCtx.processQ  );
                                    //  printQ_mis(  &pCtx.processQ  );

                                    break;
                                default:
                                    break;
                            }

                            //  ÇåÀíÒ»ÏÂ.  2008/10/18
                            //  chkMsgrs_sendLocalAv(  pProcInfo  );
                            //  if  (  bNoMsgrs_sendLocalAv(  pProcInfo,  -1  )  )  {
                            //  if  (  pMainWnd  )  PostMessage(  pMainWnd->m_hWnd,  CONST_qyWm_postComm,  CONST_qyWmParam_applyForClosingAv_qmc,  0  );    
                            //  }

                            break;
                        //

                        /*
					case CONST_imCommType_transferGpsInfo:      //  2012/04/19					

						if (!pCtx.pProcessQ) goto errLabel;

						tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
						if (tmpiRet < 0)
						{
							traceLogA("procMsgTask_mc_media: transferGpsInfo qTraverse_mono failed;");
							goto errLabel;
						}
						if (!tmpiRet)
						{
							traceLogA("procMsgTask_mc_media: transferGpsInfo, to qPostMsg");
							if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
							{
								traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
								goto errLabel;
							}
							//
							//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

							//  if  (  pContent->transferAvInfo.myTask.ucbVideoConferenceStarter  )  
							{
								//  
								if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0) goto errLabel;
							}

							traceLogA("procMsgTask_mc_media: transferGpsInfo, qPostMsg ok;");
						}
						//  							

						//  2008/03/21, 
						startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferGpsInfo"));

						break;

					case CONST_imCommType_transferGpsReplyInfo:

						if (!pCtx.pProcessQ) goto errLabel;

						//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
						if (pContent->transferGpsReplyInfo.usOp != CONST_imOp_recv_accept)
						{
						}
						else
						{
							//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
								traceLogA("procMsgTask_mc_media: transferGpsReplyInfo qTraverse_mono failed.");
								goto errLabel;
							}
							if (!tmpiRet)
							{
								if (pMsg->iTaskId)
								{       //  2009/09/10
									traceLogA("procMsgTask_mc_media: transferGpsReplyInfo to qPostMsg;");
									if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
									{
										traceLogA("procMsgTask_mc_robot: transferGpsReplyInfo, qPostMsg failed");
										goto errLabel;
									}
									//
									//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
									//
									traceLogA("procMsgTask_mc_media: gpsReply ok;");
								}
							}

							startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferGpsReplyInfo"));
						}

						break;
						*/

                        //
                        default:
#if __DEBUG__
							traceLogA("procMsgTask_mc_media failed. unknown pContent->uiType is %d", pContent->uiType);
#endif
                            goto errLabel;
                            break;
                    }

                    //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                    if (0 == pMsg.data.route.idInfo_to.ui64Id)
                    {
                        switch (pMsg.uiTaskType)
                        {
                            case Consts.CONST_imTaskType_shareDynBmp:
                            case Consts.CONST_imTaskType_shareGps:
                                //
                                iErr = 0; goto errLabel;
                                break;
                            default:
                                break;
                        }
                        traceLogA("procMsgTask_mc_media failed, idInfo_to is 0");
                        goto errLabel;
                    }

                    fixed (IM_CONTENTU* pContent0 = &pMsg.data.buf)
                    {
                        //  traceLogA(  "ÐÅÏ¢´ÓtalkingÍ¨µÀÖÐ·¢³ö"  );
                        MSG_ROUTE msgRoute = new MSG_ROUTE();
                        if (0 != postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, pMsg.uiType, pMsg.ucFlg, pMsg.usCode, pMsg.tStartTime, pMsg.uiTranNo, pMsg.usSeqNo, (byte*)&pContent0, (uint)(pMsg.lenInBytes - (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf")), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_to.ui64Id, 0, null, false)) goto errLabel;
                    }

                    //
                    iErr = 0; goto errLabel;

                }


                //  ½ÓÊÕÎÄ¼þµÄÇëÇó°ü, Î´È·ÈÏ£»Ó¦×ª·¢µ½½çÃæÈ¥ÏÔÊ¾
                //
                if (bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {

                    //  2009/10/18
                    pMsg.uiChannelType = pChannel.uiType;

                    //
                    //  ½ÓÊÕÎÄ¼þÊ±£¬ÒªÏÈ¸ø¶Ô·½·¢Ò»¸öokµÄÏìÓ¦£¬ÕâÑù£¬·½±ã¶Ô·½ÖªµÀÍ¨ÐÅÃ»ÎÊÌâ¡£·ñÔò£¬¾ÍÓÖÒªÔÙ·¢Ò»´ÎÁË
                    //
                    switch (pContent.uiType)
                    {
                        case Consts.CONST_imCommType_transferAvInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {
                                    usRespCode = Consts.CONST_qyRc_unknown;
                                    using (CQyMalloc mallocObj_rcd = new CQyMalloc())
                                    {
                                        IM_TASK_RCD* pRcd = (IM_TASK_RCD*)mallocObj_rcd.mallocf(sizeof(IM_TASK_RCD));
                                        if (null == pRcd) goto errLabel;

                                        if (null == pCtx.pProcessQ) goto errLabel;

                                        //
                                        pMsg.uiTaskType = pContent.transferAvInfo.uiTaskType;
                                        pMsg.iStatus = Consts.CONST_imTaskStatus_resp;
                                        qyFuncs.traceLog("##1412");

                                        //  2009/09/07
                                        if (pContent.transferAvInfo.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                                        {
                                            fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                            {
                                                if (0 != qPostMsg((IntPtr)pMsg0, lenInBytes_msg, pQyMc.gui.processQ))
                                                {
#if __DEBUG__
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                    goto errLabel;
                                                }
                                            }
                                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                            iErr = 0; goto errLabel;
                                        }

                                        //  2014/11/21. 这里要把代理的信息更新到起始任务里。
                                        if (0 != pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id && 0 != pContent.transferAvInfo.proxiedTranInfo.uiObjType)
                                        {
                                            if (pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                                            {

                                                PARAM_findOrgMsg param;
                                                mymemset((IntPtr)(byte*)&param, 0, sizeof(PARAM_findOrgMsg));
                                                param.bNeedUpdate = true;

                                                fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                                {
                                                    tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param);
                                                }

                                                iErr = 0; goto errLabel;
                                            }
                                        }

                                        //
                                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                        pDb = db.m_pDbMem.pDb;

                                        //
                                        //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                        {
                                            if (0 != msg2TaskRcd(pMisCnt, ref pMsg, usRespCode, ref *pRcd))
                                            {
#if __DEBUG__
									traceLogA("msg2TaskRcd failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref *pRcd, false, "procMgTask_mc_media kk");
                                        if (tmpiRet < 0)
                                        {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_media(  ): storeTaskInDb failed");
#endif
                                            goto errLabel;
                                        }
                                        //  //  2008/06/04
                                        if (tmpiRet > 0)
                                        {   //  rcd.iStatus
                                            fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                            {
                                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                                tmpiRet = pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param);
                                            }
                                            if (tmpiRet < 0) goto errLabel;
                                            if (tmpiRet > 0)
                                            {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media. hhh");
#endif
                                                iErr = 0; goto errLabel;
                                            }
                                            //  
                                            if (pRcd->iStatus != pMsg.iStatus && !bTaskAlive(pRcd->iStatus))
                                            {
                                                if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, pMsg.iStatus, pRcd->id)) goto errLabel;
                                            }

                                        }

                                        //  2007/07/01						   
                                        pMsg.iTaskId = pRcd->id;
                                        pMsg.uiTaskMode_unused = pRcd->uiMode;

                                        //
                                        fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                        {
                                            if (0 != qPostMsg((IntPtr)pMsg0, lenInBytes_msg, pQyMc.gui.processQ))
                                            {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                        iErr = 0; goto errLabel;
                                    }
                                }
                            }
                            break;
                        case Consts.CONST_imCommType_transferAvReplyInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {
                                    usRespCode = Consts.CONST_qyRc_unknown;

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                    pDb = db.m_pDbMem.pDb;

                                    //  ¶Ô·½²»ÔÊÐí
                                    if (pContent.transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
                                    {
                                        traceLogA("procMsgTask_mc_media, usSp is not accept");
                                        goto errLabel;
                                    }
                                    //
                                    if (pContent.transferAvReplyInfo.usOp == Consts.CONST_imOp_recv_accept)
                                    {
                                        QY_MESSENGER_ID idInfo = pMisCnt.idInfo;
                                        IM_TASK_RCD rcd = new IM_TASK_RCD();
                                        if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, "", idInfo, (Int64)pContent.transferAvReplyInfo.tStartTime_org, pContent.transferAvReplyInfo.uiTranNo_org, 0, ref rcd))
                                        {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media failed: bGetImTaskRcdBySth returns false");
#endif
                                            goto errLabel;
                                        }

                                        //
                                        //printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
                                        //
                                        //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                                        //  
                                        fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                        {
                                            PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                            if (pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param) <= 0)
                                            {
                                                traceLogA("procMsgTask_mc_media failed: ");
                                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                                                //
                                                //  postMsg2Mgr...
                                                goto errLabel;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //traceLogA("pContent->uiType is %d", pContent->uiType);
                                        goto errLabel;
                                    }

                                    //  2013/02/13
                                    if (0 != pContent.transferAvReplyInfo.uiTranNo_org)
                                    {
                                        if (pContent.transferAvReplyInfo.uiTranNo_org == pProcInfo.status.progress.uiTranNo)
                                        {
                                            notifyProgressEnd(pContent.transferAvReplyInfo.uiTranNo_org, 0);
                                        }
                                    }


                                    /*
									traceLogA(  "·¢ËÍÊÓÆµÊý¾ÝµÄÌõ¼þ¾ß±¸ÁË"  );
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_media  ),  _T(  "ÒÔ·¢ËÍÊÓÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯media ChannelÊ§°Ü"  );  goto  errLabel;
									}
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_realTimeMedia  ),  _T(  "ÒÔ·¢ËÍÒôÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯rtMedia ChannelÊ§°Ü"  );  goto  errLabel;
									}
									*/
                                    startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAvReplyInfo"));

                                    //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                                    usRespCode = Consts.CONST_qyRc_ok;

                                    MACRO_makeImSimpleResp(pMsg.usCode, pContent.uiType, 0, out respSimple);

                                    postMsgTask2Mgr_mc(pCtx.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, pChannel.uiType, null, false);

                                    iErr = 0; goto errLabel;
                                }
                            }
                            break;
                        case Consts.CONST_imCommType_transferVideoData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, avProcInfo

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferVideoData"));

                                //  2009/05/12						   
                                pMsg.uiChannelType = pChannel.uiType;

                                fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param)) <= 0)
                                    {
                                        getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
                                        //  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
                                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: %I64u,  tn %d, tn_openAvDev [%d], read from %s"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferVideoData.uiTranNo_openAvDev, qyGetDesByType1(CONST_channelTypeTable, pMsg->uiChannelType_input));
                                        //
                                        //  2010/12/27
                                        //  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt.idInfo.ui64Id  )  
                                        {
                                            //
                                            if (myGetTickCount(null) - pCtx.dwTickCnt_tryToResumeVideoConference > Consts.CONST_nIntervalInMs_tryToResumeVideoTask)
                                            {
                                                pCtx.dwTickCnt_tryToResumeVideoConference = myGetTickCount(null);

#if __DEBUG__
											traceLogA("procMsgTask_mc_media: videoData err, try to find orgReq");
#endif
                                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("procMsgTask_mc_media: %I64u, videoData err, try to find orgReq."), pMsg->data.route.idInfo_from.ui64Id);
                                                //
                                                showTaskStatus(pMisCnt, pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id, 0, false, pMsg.usCode, 0, pContent.transferVideoData.uiTranNo_openAvDev, pContent.uiType, 0, Consts.CONST_imTaskStatus_err_missingOrgReq);
                                            }
                                        }
                                        //
                                        //
                                        goto errLabel;
                                    }
                                }


                                iErr = 0; goto errLabel;

                            }
                            break;

                        case Consts.CONST_imCommType_transferAudioData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, avProcInfo

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAudioData"));

                                //  2009/05/12						   
                                pMsg.uiChannelType = pChannel.uiType;

                                fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param)) <= 0)
                                    {
                                        getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
                                        //  traceLogA(  "procMsgTask_mc_media failed: audio data error"  );
                                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: %I64u. tn %d, tn_openAvDev [%d], read from %s"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferAudioData.uiTranNo_openAvDev, qyGetDesByType1(CONST_channelTypeTable, pMsg->uiChannelType_input));
                                        //
                                        //  2009/10/17
                                        //  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt.idInfo.ui64Id  )  
                                        {
                                            if (myGetTickCount(null) - pCtx.dwTickCnt_tryToResumeVideoConference > 5000)
                                            {
                                                pCtx.dwTickCnt_tryToResumeVideoConference = myGetTickCount(null);
                                                traceLogA("Not finished: procMsgTask_mc_media: audioData err, shall send a request to retrieve original avInfo");
                                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("procMsgTask_mc_media: audioData err, will send a request to retrieve original avInfo"));
                                                //
                                                showTaskStatus(pMisCnt, pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id, 0, false, pMsg.usCode, 0, pContent.transferAudioData.uiTranNo_openAvDev, pContent.uiType, 0, Consts.CONST_imTaskStatus_err_missingOrgReq);
                                            }
                                        }
                                        //
                                        //
                                        goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
                                    }
                                }


                                iErr = 0; goto errLabel;

                            }
                            break;

                        case Consts.CONST_imCommType_remoteAssistReq:

                            bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                            if (null == pCtx.pProcessQ) goto errLabel;

                            startChannel(pProcInfo, pMisCnt, pChannel, ("recv remoteAssistReq"));

                            fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                            {
                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param)) <= 0)
                                {
                                    getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
#if __DEBUG__
								traceLogA("procMsgTask_mc_media failed: remoteAssistReq");
#endif
                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: st [%s], tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), CQyString(timeBuf), pContent->transferVideoData.uiTranNo_openAvDev);
                                    //
                                    goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
                                }
                            }

                            iErr = 0; goto errLabel;

                            break;

#if false
						//  2012/04/20
						case CONST_imCommType_transferGpsInfo:
							{
								ushort usRespCode = CONST_qyRc_unknown;

								if (!pCtx.pProcessQ) goto errLabel;

								//
								pMsg->uiTaskType = pContent->transferGpsInfo.uiTaskType;
								pMsg->iStatus = Consts.CONST_imTaskStatus_resp;

								//  2009/09/07
								if (pContent->transferGpsInfo.uiTaskType == CONST_imTaskType_viewGps)
								{

									if (qPostMsg(pMsg, lenInBytes_msg, &pQyMc.gui.processQ))
									{
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
									}
									PostMessage(pQyMc.gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);

									iErr = 0; goto errLabel;
								}

								//
#if __DEBUG__
								traceLog(_T("procMsgTask_mc_media failed, transferGpsInfo."));
#endif

								goto errLabel;

								//
							}
							break;

						case CONST_imCommType_transferGpsReplyInfo:
							{
								void* pDb = null;
								CQnmDb db;
								ushort usRespCode = CONST_qyRc_unknown;

								if (!pCtx.pProcessQ) goto errLabel;

								if (!db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
								pDb = db.m_pDbMem->pDb;

								//  ¶Ô·½²»ÔÊÐí
								if (pContent->transferGpsReplyInfo.usOp != CONST_imOp_recv_accept)
								{
									traceLogA("procMsgTask_mc_media, usOp is not accept");
									goto errLabel;
								}
								//
								if (pContent->transferGpsReplyInfo.usOp == CONST_imOp_recv_accept)
								{

									if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, pMisCnt.cfg.misServName, &pMisCnt.idInfo, (Int64)pContent->transferGpsReplyInfo.tStartTime_org, pContent->transferGpsReplyInfo.uiTranNo_org, 0, null))
									{
										traceLogA("procMsgTask_mc_media failed: ÕâÀïÃ»ÕÒµ½Ïà¹ØµÄÈÎÎñ£¬Ó¦¸ÃÏò¶Ô·½·¢ËÍÒ»¸ö´íÎóµÄÈÎÎñµÄÏìÓ¦°ü");
										goto errLabel;
									}

									//
									//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
									//
									printMisMsg(0, 0, (MIS_MSGU*)pMsg);

									//  
									if (pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
									{
										traceLogA("procMsgTask_mc_media failed: ");
										qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
										//
										//  postMsg2Mgr...
										goto errLabel;
									}
								}
								else
								{
									traceLogA("pContent->uiType is %d", pContent->uiType);
									goto errLabel;
								}

								/*
								traceLogA(  "·¢ËÍÊÓÆµÊý¾ÝµÄÌõ¼þ¾ß±¸ÁË"  );
								if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_media  ),  _T(  "ÒÔ·¢ËÍÊÓÆµÊý¾Ý"  )  )  )  {
									traceLogA(  "Æô¶¯media ChannelÊ§°Ü"  );  goto  errLabel;
								}
								if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_realTimeMedia  ),  _T(  "ÒÔ·¢ËÍÒôÆµÊý¾Ý"  )  )  )  {
									traceLogA(  "Æô¶¯rtMedia ChannelÊ§°Ü"  );  goto  errLabel;
								}
								*/
								startChannel(pProcInfo, pMisCnt, pChannel, _T("recv transferGpsReplyInfo"));


								iErr = 0; goto errLabel;

							}
							break;

						case CONST_imCommType_transferGpsData:
							{

								bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

								if (!pCtx.pProcessQ) goto errLabel;

								startChannel(pProcInfo, pMisCnt, pChannel, _T("recv transferGpsData"));

								//  2009/05/12						   
								pMsg->uiChannelType = pChannel.uiType;

								if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
								{
									getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
									//  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
									qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: %I64u,  tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferGpsData.uiType);
									//
									//
									goto errLabel;
								}

								iErr = 0; goto errLabel;

							}
							break;
#endif

                        default:
                            //traceLogA("procMsgTask_mc_media failed, bInput, pContent->uiType is %d", pContent->uiType);
                            goto errLabel;
                            break;
                    }

                }   //  ÒÔÉÏÎªÊäÈë


                traceLogA("procMsgTask_mc_media:  Óöµ½ÁËÎÞ·¨½âÊÍµÄÇé¿ö£¬´íÎó"); goto errLabel;


            }
            else
            {

                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                //qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("µÃµ½%I64uµÄÏìÓ¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(CONST_qyRcTable, pMsg->usCode));

                if (!isRcOk(pMsg.usCode)) goto errLabel;

                traceLogA("procMsgTask_mc_media:  ¶ÔÓÚÏìÓ¦°ü£¬»¹Ã»ÓÐ´¦ÀíÄØ");
            }

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

            return iErr;
        }



        //
        //public static unsafe int procMsgInput_mc_media(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_MSG_INPUT* pMsg, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        public static int procMsgInput_mc_media(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU pMsgU, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;
            //
            if (pMsgU.uiType != Consts.CONST_misMsgType_input)
            {
                return -1;
            }
            ref MIS_MSG_INPUT pMsg = ref pMsgU.input;
            //
            int dataLen = 0;
            uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            ref IM_CONTENTU pContent = ref pMsg.data.buf;
            //  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //  IM_TASK_RCD			rcd;
            int tmpiRet;
            //
            int lenInBytes = 0;
            //
            //IM_SIMPLE_RESP respSimple;
            CIM_CONTENTU content = null;

            //
            ushort usRespCode = Consts.CONST_qyRc_unknown;
            uint uiChannelType = Consts.CONST_channelType_talking;
            //char timeBuf[CONST_qyTimeLen + 1] = "";
            TMP_timeBuf timeBuf;
            //
            bool bRemoveIfErr = true;       //  ÔÚÔËÐÐ³ö´íÊ±ÊÇ·ñÉ¾³ýÏà¹Ø½Úµã¡£
                                            //
                                            //  TCHAR					sqlBuf[CONST_maxSqlBufLen  +  1]	=	_T(  ""  );
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            string str;

            //
            if (null == pQyMc.gui.hMainWnd) return -1;
            if (null == pMisCnt) return -1;

            //  memset(  &rcd,  0, sizeof(  rcd  )  );

            //
#if DEBUG
            qyFuncs.traceLog("procMsgInput_mc_media enters");
#endif


            //  2014/11/22
#if __DEBUG__
			if (pContent->uiType == CONST_imCommType_transferAvInfo)
			{
				int i;
				i = 0;

			}
#endif


            if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
            {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                {
                    switch (pMsg.uiTaskType)
                    {
                        case Consts.CONST_imTaskType_shareDynBmp:
                        case Consts.CONST_imTaskType_shareGps:
                            break;
                        default:
                            {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: getIdInfo_logicalPeer failed.");
#endif
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, NULL, _T(""), _T(""), _T("procMsgTask_mc_media: getIdInfo_logicalPeer failed."));
                                goto errLabel;
                            }
                            break;
                    }
                }
            }


#if DEBUG
            qyFuncs.traceLog("procMsgInput_mc_media.1946");
#endif


            if (!isUcFlgResp(pMsg.ucFlg))
            {

                //
#if DEBUG
                qyFuncs.traceLog("procMsgInput_mc_media.1955");
#endif


                //
                if (!bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {       //  Ö÷¶¯·¢³ö

                    switch (pContent.uiType)
                    {

#if false
						case Consts.CONST_imCommType_transferAvInfo:
							if (pContent->transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
							{
#if __DEBUG__
#endif
								goto errLabel;
							}
							TRANSFER_AV_task* pTransferAvTask; pTransferAvTask = (TRANSFER_AV_task*)pContent;

							//
							if (!pCtx.pProcessQ) goto errLabel;

							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo qTraverse_mono failed;");
#endif
								goto errLabel;
							}
							if (!tmpiRet)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, to qPostMsg");
#endif
								if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
								{
									traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
									goto errLabel;
								}
								//
								//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

								//
								if (pTransferAvTask->myTask.ucbVideoConferenceStarter)
								{
									//  ÔÙ²éÑ¯Ò»´ÎprocessQ_media, ÒòÎªÊÓÆµ»áÒéµÄbmpInfo_compressµÈ£¬ÊÇÔÚÈë¶ÓÁÐÊ±²ÅÉú³ÉµÄ¡£Òª´ÓÀïÃæÈ¡³öÀ´
									if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0) goto errLabel;
								}

#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, qPostMsg ok;");
#endif
							}
							//  							

							//  2008/03/21, 
							startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferAvInfo"));

							break;
						case Consts.CONST_imCommType_transferAvReplyInfo:

							if (!pCtx.pProcessQ) goto errLabel;

							//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
							if (pContent->transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
							{
							}
							else
							{
								//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
								tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
								if (tmpiRet < 0)
								{
#if __DEBUG__
									traceLogA("procMsgTask_mc_media: transferAvReplyInfo qTraverse_mono failed.");
#endif
									goto errLabel;
								}
								if (!tmpiRet)
								{
									if (pMsg->iTaskId)
									{       //  2009/09/10
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: transferAvReplyInfo to qPostMsg;");
#endif
										//
										if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
										{
#if __DEBUG__
											traceLogA("procMsgTask_mc_robot: transferAvReplyInfo, qPostMsg failed");
#endif
											goto errLabel;
										}
										//
										//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: avReply ok;");
#endif

										//
										//  2014/11/20								 
										//  post

									}
								}

								startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferAvReplyInfo"));

							}

							break;
#endif
                        case Consts.CONST_imCommType_transferVideoData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("send transferVideoData"));

                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                {
                                    //traceLogA("procMsgTask_mc_media: ´¦ÀíCONST_imCommType_transferVideoData %s¡£", tmpiRet ? "failed" : "not found");
                                    goto errLabel;
                                }

                            }
                            iErr = 0; goto errLabel;
                            break;
                        case Consts.CONST_imCommType_transferAudioData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("send transferAudioData"));

                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();
                                if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                {
                                    //traceLogA("procMsgTask_mc_media: ´¦ÀíCONST_imCommType_transferAudioData %s¡£", tmpiRet ? "failed" : "not found");
                                    goto errLabel;
                                }

                            }
                            iErr = 0; goto errLabel;
                            break;
                        case Consts.CONST_imCommType_taskProcReq:
                            switch (pContent.taskProcReq.usOp)
                            {
                                case Consts.CONST_imOp_send_cancel:
                                case Consts.CONST_imOp_recv_cancel:

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    //  printQ_mis(  &pCtx.processQ  );
                                    //  printQ_mis(  &pCtx.processQ  );

                                    break;
                                default:
                                    break;
                            }

                            //  ÇåÀíÒ»ÏÂ.  2008/10/18
                            //  chkMsgrs_sendLocalAv(  pProcInfo  );
                            //  if  (  bNoMsgrs_sendLocalAv(  pProcInfo,  -1  )  )  {
                            //  if  (  pMainWnd  )  PostMessage(  pMainWnd->m_hWnd,  CONST_qyWm_postComm,  CONST_qyWmParam_applyForClosingAv_qmc,  0  );    
                            //  }

                            break;
                        //

                        /*
					case CONST_imCommType_transferGpsInfo:      //  2012/04/19					

						if (!pCtx.pProcessQ) goto errLabel;

						tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
						if (tmpiRet < 0)
						{
							traceLogA("procMsgTask_mc_media: transferGpsInfo qTraverse_mono failed;");
							goto errLabel;
						}
						if (!tmpiRet)
						{
							traceLogA("procMsgTask_mc_media: transferGpsInfo, to qPostMsg");
							if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
							{
								traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
								goto errLabel;
							}
							//
							//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

							//  if  (  pContent->transferAvInfo.myTask.ucbVideoConferenceStarter  )  
							{
								//  
								if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0) goto errLabel;
							}

							traceLogA("procMsgTask_mc_media: transferGpsInfo, qPostMsg ok;");
						}
						//  							

						//  2008/03/21, 
						startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferGpsInfo"));

						break;

					case CONST_imCommType_transferGpsReplyInfo:

						if (!pCtx.pProcessQ) goto errLabel;

						//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
						if (pContent->transferGpsReplyInfo.usOp != CONST_imOp_recv_accept)
						{
						}
						else
						{
							//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
								traceLogA("procMsgTask_mc_media: transferGpsReplyInfo qTraverse_mono failed.");
								goto errLabel;
							}
							if (!tmpiRet)
							{
								if (pMsg->iTaskId)
								{       //  2009/09/10
									traceLogA("procMsgTask_mc_media: transferGpsReplyInfo to qPostMsg;");
									if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
									{
										traceLogA("procMsgTask_mc_robot: transferGpsReplyInfo, qPostMsg failed");
										goto errLabel;
									}
									//
									//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
									//
									traceLogA("procMsgTask_mc_media: gpsReply ok;");
								}
							}

							startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferGpsReplyInfo"));
						}

						break;
						*/

                        //
                        default:
#if __DEBUG__
							traceLogA("procMsgTask_mc_media failed. unknown pContent->uiType is %d", pContent->uiType);
#endif
                            goto errLabel;
                            break;
                    }

                    //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                    if (0 == pMsg.data.route.idInfo_to.ui64Id)
                    {
                        switch (pMsg.uiTaskType)
                        {
                            case Consts.CONST_imTaskType_shareDynBmp:
                            case Consts.CONST_imTaskType_shareGps:
                                //
                                iErr = 0; goto errLabel;
                                break;
                            default:
                                break;
                        }
                        traceLogA("procMsgTask_mc_media failed, idInfo_to is 0");
                        goto errLabel;
                    }

                    //  traceLogA(  "ÐÅÏ¢´ÓtalkingÍ¨µÀÖÐ·¢³ö"  );
                    //fixed (IM_CONTENTU* buf = &pMsg.data.buf)
                    {
                        MSG_ROUTE msgRoute = new MSG_ROUTE();
                        if (0 != postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, pMsg.uiType, pMsg.ucFlg, pMsg.usCode, pMsg.tStartTime, pMsg.uiTranNo, pMsg.usSeqNo, ref pMsg.data.buf, (uint)(pMsg.lenInBytes - (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf")), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_to.ui64Id, 0, null, false)) goto errLabel;
                    }

                    iErr = 0; goto errLabel;

                }


                //  ½ÓÊÕÎÄ¼þµÄÇëÇó°ü, Î´È·ÈÏ£»Ó¦×ª·¢µ½½çÃæÈ¥ÏÔÊ¾
                //
                if (bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {
#if DEBUG
                    qyFuncs.traceLog("procMsgInput_mc_media.2248. bMsgInput true. content.type " + pContent.uiType);
#endif


                    //  2009/10/18
                    pMsg.uiChannelType = pChannel.uiType;

                    //
                    //  ½ÓÊÕÎÄ¼þÊ±£¬ÒªÏÈ¸ø¶Ô·½·¢Ò»¸öokµÄÏìÓ¦£¬ÕâÑù£¬·½±ã¶Ô·½ÖªµÀÍ¨ÐÅÃ»ÎÊÌâ¡£·ñÔò£¬¾ÍÓÖÒªÔÙ·¢Ò»´ÎÁË
                    //
                    switch (pContent.uiType)
                    {
                        case Consts.CONST_imCommType_transferAvInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {
                                    usRespCode = Consts.CONST_qyRc_unknown;
                                    using (CQyMalloc mallocObj_rcd = new CQyMalloc())
                                    {
                                        //IM_TASK_RCD* pRcd = (IM_TASK_RCD*)mallocObj_rcd.mallocf(sizeof_IM_TASK_RCD());
                                        CIM_TASK_RCD pRcd = new CIM_TASK_RCD();
                                        if (null == pRcd) goto errLabel;

                                        if (null == pCtx.pProcessQ) goto errLabel;

                                        //
                                        pMsg.uiTaskType = pContent.transferAvInfo.uiTaskType;
                                        pMsg.iStatus = Consts.CONST_imTaskStatus_resp;
                                        qyFuncs.traceLog("##1347 " + pMsg.iStatus);
                                        //  2009/09/07
                                        if (pContent.transferAvInfo.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                                        {

                                            //if (0 != qPostMsg((IntPtr)pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                                            if (0 != tmp_qPost_msgInput(ref pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                                            {
#if __DEBUG__
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                goto errLabel;
                                            }
                                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                            iErr = 0; goto errLabel;
                                        }

                                        //  2014/11/21. 这里要把代理的信息更新到起始任务里。
                                        if (0 != pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id && 0 != pContent.transferAvInfo.proxiedTranInfo.uiObjType)
                                        {
                                            if (pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                                            {

                                                PARAM_findOrgMsg param1 = new PARAM_findOrgMsg();
                                                param1.bNeedUpdate = true;

                                                tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param1);

                                                iErr = 0; goto errLabel;
                                            }
                                        }

                                        //
                                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                        pDb = db.m_pDbMem.pDb;

                                        //
                                        if (0 != msg2TaskRcd(pMisCnt, ref pMsg, usRespCode, ref pRcd.imTaskRcd))
                                        {
#if __DEBUG__
									traceLogA("msg2TaskRcd failed");
#endif
                                            goto errLabel;
                                        }
                                        tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref pRcd.imTaskRcd, false, "procMsgInput_mc_media");
                                        if (tmpiRet < 0)
                                        {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_media(  ): storeTaskInDb failed");
#endif
                                            goto errLabel;
                                        }
                                        //  //  2008/06/04
                                        if (tmpiRet > 0)
                                        {   //  rcd.iStatus

                                            PARAM_findOrgMsg param1 = new PARAM_findOrgMsg();

                                            tmpiRet = pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param1);
                                            if (tmpiRet < 0) goto errLabel;
                                            if (tmpiRet > 0)
                                            {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media. hhh");
#endif
                                                iErr = 0; goto errLabel;
                                            }
                                            //  
                                            if (pRcd.imTaskRcd.iStatus != pMsg.iStatus && !bTaskAlive(pRcd.imTaskRcd.iStatus))
                                            {
                                                if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, pMsg.iStatus, pRcd.imTaskRcd.id)) goto errLabel;
                                            }

                                        }

                                        //  2007/07/01						   
                                        pMsg.iTaskId = pRcd.imTaskRcd.id;
                                        pMsg.uiTaskMode_unused = pRcd.imTaskRcd.uiMode;

                                        //
                                        if (0 != tmp_qPost_msgInput(ref pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                                        {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                            goto errLabel;
                                        }
                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                        iErr = 0; goto errLabel;
                                    }
                                }
                            }
                            break;
                        case Consts.CONST_imCommType_transferAvReplyInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {
                                    usRespCode = Consts.CONST_qyRc_unknown;

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                    pDb = db.m_pDbMem.pDb;

                                    //  ¶Ô·½²»ÔÊÐí
                                    if (pContent.transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
                                    {
                                        traceLogA("procMsgTask_mc_media, usSp is not accept");
                                        goto errLabel;
                                    }
                                    //
                                    if (pContent.transferAvReplyInfo.usOp == Consts.CONST_imOp_recv_accept)
                                    {
                                        QY_MESSENGER_ID idInfo = pMisCnt.idInfo;
                                        IM_TASK_RCD rcd = new IM_TASK_RCD();
                                        if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, "", idInfo, (Int64)pContent.transferAvReplyInfo.tStartTime_org, pContent.transferAvReplyInfo.uiTranNo_org, 0, ref rcd))
                                        {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media failed: bGetImTaskRcdBySth returns false");
#endif
                                            goto errLabel;
                                        }

                                        //
                                        //printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
                                        //
                                        //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                                        PARAM_findOrgMsg param1 = new PARAM_findOrgMsg();

                                        //  
                                        if (pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param1) <= 0)
                                        {
                                            traceLogA("procMsgTask_mc_media failed: ");
                                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                                            //
                                            //  postMsg2Mgr...
                                            goto errLabel;
                                        }
                                    }
                                    else
                                    {
                                        //traceLogA("pContent->uiType is %d", pContent->uiType);
                                        goto errLabel;
                                    }

                                    //  2013/02/13
                                    if (0 != pContent.transferAvReplyInfo.uiTranNo_org)
                                    {
                                        if (pContent.transferAvReplyInfo.uiTranNo_org == pProcInfo.status.progress.uiTranNo)
                                        {
                                            notifyProgressEnd(pContent.transferAvReplyInfo.uiTranNo_org, 0);
                                        }
                                    }


                                    /*
									traceLogA(  "·¢ËÍÊÓÆµÊý¾ÝµÄÌõ¼þ¾ß±¸ÁË"  );
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_media  ),  _T(  "ÒÔ·¢ËÍÊÓÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯media ChannelÊ§°Ü"  );  goto  errLabel;
									}
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_realTimeMedia  ),  _T(  "ÒÔ·¢ËÍÒôÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯rtMedia ChannelÊ§°Ü"  );  goto  errLabel;
									}
									*/
                                    startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAvReplyInfo"));

                                    //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                                    usRespCode = Consts.CONST_qyRc_ok;

                                    content = new CIM_CONTENTU();
                                    MACRO_makeImSimpleResp(pMsg.usCode, pContent.uiType, 0, out content.imContentU.simpleResp);

                                    //postMsgTask2Mgr_mc(pCtx.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, (byte*)&respSimple, (uint)sizeof_IM_SIMPLE_RESP(), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, pChannel.uiType, null, false);
                                    postMsgTask2Mgr_mc(pCtx.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, ref content.imContentU, (uint)sizeof_IM_SIMPLE_RESP(), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, pChannel.uiType, null, false);

                                    iErr = 0; goto errLabel;
                                }
                            }
                            break;
                        case Consts.CONST_imCommType_transferVideoData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, avProcInfo

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferVideoData"));

                                //  2009/05/12						   
                                pMsg.uiChannelType = pChannel.uiType;

                                PARAM_findOrgMsg param1 = new PARAM_findOrgMsg();

                                if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param1)) <= 0)
                                {
                                    getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
                                    //  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: %I64u,  tn %d, tn_openAvDev [%d], read from %s"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferVideoData.uiTranNo_openAvDev, qyGetDesByType1(CONST_channelTypeTable, pMsg->uiChannelType_input));
                                    //
                                    //  2010/12/27
                                    //  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt.idInfo.ui64Id  )  
                                    {
                                        //
                                        if (myGetTickCount(null) - pCtx.dwTickCnt_tryToResumeVideoConference > Consts.CONST_nIntervalInMs_tryToResumeVideoTask)
                                        {
                                            pCtx.dwTickCnt_tryToResumeVideoConference = myGetTickCount(null);

                                            str = string.Format("procMsgInput_mc_media.2456: {0}, videoData err, try to find orgReq.", pMsg.data.route.idInfo_from.ui64Id);
                                            if (logCountG < 10)
                                            {
                                                ++logCountG;
                                            }
                                            else
                                            {
                                                logCountG = 0;

                                                showInfo_open(0, null, str);


                                            }
                                            //
                                            int iStatus = Consts.CONST_imTaskStatus_err_missingOrgReq;
                                            //
                                            if (tmpiRet == Consts.CONST_qyRet_cantGet_index_activeMems_from)
                                            {
                                                iStatus = Consts.CONST_imTaskStatus_err_cantGet_index_activeMems_from;
                                            }
                                            //
                                            showTaskStatus(pMisCnt, pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id, 0, false, pMsg.usCode, 0, pContent.transferVideoData.uiTranNo_openAvDev, pContent.uiType, 0, iStatus);
                                        }
                                    }
                                    //
#if DEBUG
                                    str = string.Format("procMsgInput_mc_media.2565: {0}, videoData err.", pMsg.data.route.idInfo_from.ui64Id);

                                    qyFuncs.traceLog(str);
#endif
                                    //
                                    goto errLabel;
                                }


                                iErr = 0; goto errLabel;

                            }
                            break;

                        case Consts.CONST_imCommType_transferAudioData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, avProcInfo

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAudioData"));

                                //  2009/05/12						   
                                pMsg.uiChannelType = pChannel.uiType;

                                PARAM_findOrgMsg param1 = new PARAM_findOrgMsg();
                                param1.hint = "procMsgInput_mc_media.2613, audioData";

                                if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param1)) <= 0)
                                {
                                    getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
                                    //  traceLogA(  "procMsgTask_mc_media failed: audio data error"  );
                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: %I64u. tn %d, tn_openAvDev [%d], read from %s"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferAudioData.uiTranNo_openAvDev, qyGetDesByType1(CONST_channelTypeTable, pMsg->uiChannelType_input));
                                    //
                                    //  2009/10/17
                                    //  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt.idInfo.ui64Id  )  
                                    {
                                        if (myGetTickCount(null) - pCtx.dwTickCnt_tryToResumeVideoConference > 2000)
                                        {
                                            pCtx.dwTickCnt_tryToResumeVideoConference = myGetTickCount(null);

                                            //
                                            str = string.Format("procMsgTask_mc_media: audioData err, shall send a request to retrieve original avInfo. conf{0}, from {1}", pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id);
                                            showInfo_open(0, null, str);

                                            //
                                            int iStatus = Consts.CONST_imTaskStatus_err_missingOrgReq;
                                            //
                                            if (tmpiRet == Consts.CONST_qyRet_cantGet_index_activeMems_from)
                                            {
                                                iStatus = Consts.CONST_imTaskStatus_err_cantGet_index_activeMems_from;
                                            }

                                            //
                                            showTaskStatus(pMisCnt, pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id, 0, false, pMsg.usCode, 0, pContent.transferAudioData.uiTranNo_openAvDev, pContent.uiType, 0, iStatus);
                                        }
                                    }
                                    //
                                    //
                                    goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
                                }


                                iErr = 0; goto errLabel;

                            }
                            break;

                        case Consts.CONST_imCommType_remoteAssistReq:

                            bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                            if (null == pCtx.pProcessQ) goto errLabel;

                            startChannel(pProcInfo, pMisCnt, pChannel, ("recv remoteAssistReq"));

                            PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                            if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                            {
                                getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
#if __DEBUG__
								traceLogA("procMsgTask_mc_media failed: remoteAssistReq");
#endif
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: st [%s], tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), CQyString(timeBuf), pContent->transferVideoData.uiTranNo_openAvDev);
                                //
                                goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
                            }

                            iErr = 0; goto errLabel;

                            break;

#if false
						//  2012/04/20
						case CONST_imCommType_transferGpsInfo:
							{
								ushort usRespCode = CONST_qyRc_unknown;

								if (!pCtx.pProcessQ) goto errLabel;

								//
								pMsg->uiTaskType = pContent->transferGpsInfo.uiTaskType;
								pMsg->iStatus = Consts.CONST_imTaskStatus_resp;

								//  2009/09/07
								if (pContent->transferGpsInfo.uiTaskType == CONST_imTaskType_viewGps)
								{

									if (qPostMsg(pMsg, lenInBytes_msg, &pQyMc.gui.processQ))
									{
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
									}
									PostMessage(pQyMc.gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);

									iErr = 0; goto errLabel;
								}

								//
#if __DEBUG__
								traceLog(_T("procMsgTask_mc_media failed, transferGpsInfo."));
#endif

								goto errLabel;

								//
							}
							break;

						case CONST_imCommType_transferGpsReplyInfo:
							{
								void* pDb = null;
								CQnmDb db;
								ushort usRespCode = CONST_qyRc_unknown;

								if (!pCtx.pProcessQ) goto errLabel;

								if (!db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
								pDb = db.m_pDbMem->pDb;

								//  ¶Ô·½²»ÔÊÐí
								if (pContent->transferGpsReplyInfo.usOp != CONST_imOp_recv_accept)
								{
									traceLogA("procMsgTask_mc_media, usOp is not accept");
									goto errLabel;
								}
								//
								if (pContent->transferGpsReplyInfo.usOp == CONST_imOp_recv_accept)
								{

									if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, pMisCnt.cfg.misServName, &pMisCnt.idInfo, (Int64)pContent->transferGpsReplyInfo.tStartTime_org, pContent->transferGpsReplyInfo.uiTranNo_org, 0, null))
									{
										traceLogA("procMsgTask_mc_media failed: ÕâÀïÃ»ÕÒµ½Ïà¹ØµÄÈÎÎñ£¬Ó¦¸ÃÏò¶Ô·½·¢ËÍÒ»¸ö´íÎóµÄÈÎÎñµÄÏìÓ¦°ü");
										goto errLabel;
									}

									//
									//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
									//
									printMisMsg(0, 0, (MIS_MSGU*)pMsg);

									//  
									if (pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
									{
										traceLogA("procMsgTask_mc_media failed: ");
										qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
										//
										//  postMsg2Mgr...
										goto errLabel;
									}
								}
								else
								{
									traceLogA("pContent->uiType is %d", pContent->uiType);
									goto errLabel;
								}

								/*
								traceLogA(  "·¢ËÍÊÓÆµÊý¾ÝµÄÌõ¼þ¾ß±¸ÁË"  );
								if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_media  ),  _T(  "ÒÔ·¢ËÍÊÓÆµÊý¾Ý"  )  )  )  {
									traceLogA(  "Æô¶¯media ChannelÊ§°Ü"  );  goto  errLabel;
								}
								if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  CONST_channelType_realTimeMedia  ),  _T(  "ÒÔ·¢ËÍÒôÆµÊý¾Ý"  )  )  )  {
									traceLogA(  "Æô¶¯rtMedia ChannelÊ§°Ü"  );  goto  errLabel;
								}
								*/
								startChannel(pProcInfo, pMisCnt, pChannel, _T("recv transferGpsReplyInfo"));


								iErr = 0; goto errLabel;

							}
							break;

						case CONST_imCommType_transferGpsData:
							{

								bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

								if (!pCtx.pProcessQ) goto errLabel;

								startChannel(pProcInfo, pMisCnt, pChannel, _T("recv transferGpsData"));

								//  2009/05/12						   
								pMsg->uiChannelType = pChannel.uiType;

								if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
								{
									getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
									//  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
									qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("%s err: %I64u,  tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc.cusRes, CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferGpsData.uiType);
									//
									//
									goto errLabel;
								}

								iErr = 0; goto errLabel;

							}
							break;
#endif

                        default:
                            //traceLogA("procMsgTask_mc_media failed, bInput, pContent->uiType is %d", pContent->uiType);
                            goto errLabel;
                            break;
                    }

                }   //  ÒÔÉÏÎªÊäÈë


                traceLogA("procMsgTask_mc_media:  Óöµ½ÁËÎÞ·¨½âÊÍµÄÇé¿ö£¬´íÎó"); goto errLabel;


            }
            else
            {

                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                //qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo.who_showInfo, null, _T(""), _T(""), _T("µÃµ½%I64uµÄÏìÓ¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(CONST_qyRcTable, pMsg->usCode));

                if (!isRcOk(pMsg.usCode)) goto errLabel;

                traceLogA("procMsgTask_mc_media:  ¶ÔÓÚÏìÓ¦°ü£¬»¹Ã»ÓÐ´¦ÀíÄØ");
            }

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

#if DEBUG
            qyFuncs.traceLog("procMsgInput_mc_media leaves. iErr " + iErr);
#endif

            return iErr;
        }



        //
        static int logCountG = 0;



        //
        public static  int doMediaMsg(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU pMsg, int msgLen, uint uiChannelType, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;
            int len = msgLen;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

#if true
            //
#if DEBUG
            //chkAllTc("doMisMsg enters");
            qyFuncs.traceLog("doMediaMsg enters. msg.type " + pMsg.uiType);
#endif

            //
            switch (pMsg.uiType)
            {
                case Consts.CONST_misMsgType_input:
                    {
                        //MIS_CNT					*		pMisCnt							=  (  MIS_CNT  *  )pMsg->input.pMisCnt;
                        MIS_CHANNEL pChannel = null;

                        //if  (  !pMisCnt  )  break;
                        //  if  (  !(  pChannel  =  getChannelByType(  pMisCnt,  pMsg->input.uiChannelType  )  )  )  break;
                        if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) break;

                        //
#if DEBUG
                        qyFuncs.traceLog("doMediaMsg.2882. isResp " + isUcFlgResp(pMsg.input.ucFlg));
#endif


                        //
                        if (isUcFlgResp(pMsg.input.ucFlg))
                        {
                            if (0 != procMsgInput_other_resp(pQyMc, pProcInfo, ref pMsg, (uint)len, pCtx, pChannel))
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (0 != procMsgInput_other_req(pQyMc, pProcInfo, ref pMsg, (uint)len, pCtx, pChannel, "doMediaMsg.2866"))
                            {
                                break;
                            }
                        }

                        //
                    }
                    break;
                case Consts.CONST_misMsgType_talk:
#if __DEBUG__
									 traceLogA(  "mcThreadProc_mis_media: msg_talk ÔÝÊ±²»´¦Àí"  );
#endif
                    break;
                case Consts.CONST_misMsgType_task:
                    {
                        //MIS_CNT* pMisCnt = (MIS_CNT*)pMsg->task.pMisCnt;
                        MIS_CHANNEL pChannel = null;

                        //if (!pMisCnt) break;
                        if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) break;

                        if (0 != procMsgTask_mc_media(pQyMc, pProcInfo, ref pMsg, (uint)len, false, ref pCtx, pChannel)) break;

                        //  printMisMsg(  0,  0,  pMsg  );

                    }
                    break;
                case Consts.CONST_misMsgType_applyForChkChannels_qmc:
                    {
                        //MIS_CNT* pMisCnt = (MIS_CNT*)pMsg->applyForChkChannels_qmc.pMisCnt;
                        if (null == pMisCnt) break;

#if DEBUG
                        traceLogA("media: applyForChkChannels");
#endif
                        startChannel(pProcInfo, pMisCnt, getChannelByType(pMisCnt, uiChannelType), ("media: applyForChannels"));

                    }
                    break;
                case Consts.CONST_misMsgType_outputTask:
                default:
#if __DEBUG__
										traceLogA("mcThreadProc_mis_media failed, Î´´¦ÀíµÄÏûÏ¢ÀàÐÍ%d", pMsg->uiType);
#endif
                    break;
            }
#endif

            iErr = 0;
        errLabel:

            //
#if DEBUG
            //chkAllTc("doMisMsg leaves");
            qyFuncs.traceLog("doMediaMsg leaves");
#endif

            return iErr;
        }


        //
        public static unsafe int sizeof_MIS_MSGU()
        {
            return sizeof(MIS_MSGU);
        }



        //  2015/09/13
        public static   void mcThreadProc_mis_media(object lpParameter)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();
            int loopCtrl = 0;
            int i;
            uint dwThreadId = myGetCurrentThreadId();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            uint uiChannelType = Consts.CONST_channelType_media;

            //
            string str;
            string where_showInfo = "videoMgr";

            //
            pProcInfo.dwThreadId_media = myGetCurrentThreadId();

            //
            using (CQyMalloc mallocObj_msgs = new CQyMalloc())
            {
                //MIS_MSGU* msgs = null;


                //MIS_MSGU* pMsgs[Consts.CONST_pMsgs_media];
                //IntPtr[] pMsgs = new IntPtr[Consts.CONST_pMsgs_media];
                CMIS_MSGU[] pMsgs = new CMIS_MSGU[Consts.CONST_pMsgs_media];

                //int msgLens[Consts.CONST_pMsgs_media];
                uint[] msgLens = new uint[Consts.CONST_pMsgs_media];

                //
                int maxMsgs = Consts.CONST_pMsgs_media;            //  一次最多处理的包数。
                uint nMsgs;

                //
                uint dwRet;
                //  long								lPrev;
                //
                CTX_MC_thread ctx = new CTX_MC_thread();

                //
                ctx.ctxCaller.type = Consts.CONST_ctxCallerType_thread;
                ctx.ctxCaller.dwThreadId = myGetCurrentThreadId();
                
                /*
                fixed (char* pNameBuf = ctx.ctxCaller.tNameBuf.tNameBuf)
                {
                    mytcsncpy(pNameBuf, "thread_media", Consts.cntof_ctxCaller_tNameBuf);
                }
                */

                //
                using (CQyMalloc mallocObj_pMsgInputBuf = new CQyMalloc())
                {
                    using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
                    {
                        //
                        //  CWnd						*		pMainWnd						=	AfxGetApp(  )->m_pMainWnd;
                        //
                        QY_Q2 pTriggerQ2 = pProcInfo.mediaQ2;

                        //
                        str = ("mcThreadProc_mis_media.3012:  enters");
                        showInfo_open(0, where_showInfo, str);

                        //memset(&ctx, 0, sizeof(ctx));
                        ctx.pProcessQ = pProcInfo.processQ_media;
                        ctx.pMsgBuf_forInput = new CMIS_MSGU();// (MIS_MSGU*)mallocObj_pMsgInputBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                        if (null == ctx.pMsgBuf_forInput) goto errLabel;
                        ctx.pMsgBuf = new CMIS_MSGU();// (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                        if (null == ctx.pMsgBuf) goto errLabel;

                        //
                        /*
                        if (null == (msgs = (MIS_MSGU*)mallocObj_msgs.mallocf(Consts.CONST_iAllocType_longTimeMsg, maxMsgs * sizeof_MIS_MSGU())))
                        {
                            iErr = -100;
                            goto errLabel;
                        }
                        */
                        for (i = 0; i < maxMsgs; i++)
                        {
                            //pMsgs[i] = (IntPtr)(byte*)&msgs[i];
                            pMsgs[i] = new CMIS_MSGU();
                        }

                        //
#if DEBUG
                        qyFuncs.traceLog("mcThread_media.3035");
#endif

                        //
                        for (; !pQyMc.bQuit; loopCtrl++)
                        {

                            //  if  (  !pMainWnd  )  pMainWnd					=	AfxGetApp(  )->m_pMainWnd;
                            if (null == pQyMc.gui.hMainWnd)
                            {
                                mySleep(1000); continue;
                            }

                            //dwRet = WaitForSingleObject(pTriggerQ2->hEvents[0], 5000);
                            //if (dwRet == WAIT_FAILED) goto errLabel;
                            myWaitOne(pTriggerQ2.hEvent0, 5000);

                            for (; !pQyMc.bQuit;)
                            {
                                //
                                if (isQ2Empty(pTriggerQ2)) break;

                                //
                                for (i = 0; i < maxMsgs; i++)
                                {
                                    msgLens[i] = (uint)sizeof_MIS_MSGU();
                                }
                                nMsgs = (uint)maxMsgs;
                                //  len  =  sizeof(  pMsg[0]  );
                                {
                                    using (CQyQ2Help q2Help = new CQyQ2Help())
                                    {
                                        //if (0 != qGetMsgs(q2Help.getQ_toGetMsg(pTriggerQ2), (IntPtr[])pMsgs, (uint[])msgLens, (uint*)&nMsgs)) break;
                                        if (0 != qGetMsgs(q2Help.getQ_toGetMsg(pTriggerQ2), ref pMsgs, (uint[])msgLens, ref nMsgs)) break;
                                    }
                                }

#if DEBUG
                                qyFuncs.traceLog("mcThread_media.3072. nMsgs " + nMsgs);
#endif


                                //
                                for (i = 0; i < nMsgs; i++)
                                {
                                    //mymemset((IntPtr)(((byte*)(pMsgs[i])) + msgLens[i]), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - msgLens[i]));
                                }

#if DEBUG
					   //  traceLog(  _T(  "preTransV: nMsgs %d"  ),  nMsgs  );
#endif
                                if (0 == nMsgs) continue;

                                for (i = 0; i < nMsgs; i++)
                                {
                                    CMIS_MSGU pMsg = pMsgs[i];
                                    int msgLen = (int)msgLens[i];

                                    //
                                    doMediaMsg(pQyMc, pProcInfo, ref pMsg.msgU, msgLen, uiChannelType, ref ctx);

                                }

                            }

                            if (0 == (loopCtrl % 5))
                            {

                                //  ÏÂÃæ×¢Òâ°Ñ²»ÓÃµÄÍ¨µÀ¸ø»ØÊÕ»ØÀ´
                                //for  (  i  =  0;  i  <  mycountof(  pProcInfo.misCnts  );  i  ++  )  
                                //for (i = 0; i < pProcInfo.ucCnt_misCnts; i++)
                                {

                                    waitForChannel(pProcInfo, pMisCnt, uiChannelType, false);
                                }
                            }

                        }
                    }
                }
            }

            iErr = 0;

        errLabel:


            //for  (  i  =  0;  i  <  mycountof(  pProcInfo.misCnts  );  i  ++  )  
            //for (i = 0; i < pProcInfo.ucCnt_misCnts; i++)
            {

                waitForChannel(pProcInfo, pMisCnt, uiChannelType, true);
            }


            pProcInfo.status.threadsStatus.ulbMediaQuit = true;

            traceLogA("mcThreadProc_mis_media:  leaves");

            return;
        }


    }
}
