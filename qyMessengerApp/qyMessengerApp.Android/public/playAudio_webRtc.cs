
using Java.Lang;
using qy;
using Math = Java.Lang.Math;

namespace qy.Droid
{
    partial class qyFuncs_Droid
	{

		public static void 		M_chk_pts(PLAY_AUDIO_procInfo pPlayerAudio, ref myPLAY_AUDIO_DATA pkt  )	

				
		{  
			/*
			if  (pPlayAudio->lowLatency.ptsInfo.uiPts_lastRecvd  &&  pkt.head.uiPts != pPlayAudio->lowLatency.ptsInfo.uiPts_lastRecvd + 1 )  {		\
						pPlayAudio->lowLatency.ptsInfo.nErr_ptsMissed  ++  ;			\
					    if  (pProcInfo->cfg.debugStatusInfo.ucbShowPlayAudioStatus  )  {			\
							_sntprintf(tBuf, mycountof(tBuf  ), _T(  "my_inStream: pts missed: pts_lastRecvd %d, pkt.pts %d. nErr_ptsMissed %d"  ),  pPlayAudio->lowLatency.ptsInfo.uiPts_lastRecvd,  pkt.head.uiPts, pPlayAudio->lowLatency.ptsInfo.nErr_ptsMissed  );	\
						   showInfo_open(  0,  0, tBuf  );						\
						}														\
					}															\
					pPlayAudio->lowLatency.ptsInfo.uiPts_lastRecvd = pkt.head.uiPts;	\
			*/
				
		}

		public static void FillMemory(System.IntPtr Destination, int Length, int Fill) {
			qyFuncs.mymemset((Destination), (Fill), (Length));
		}


		//
		public static unsafe int my_inStream(QY_PLAYER pPlayer, byte * buf, int bufLen)
		{
			int nRead = 0;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc  pProcInfo = pQyMc.get_pProcInfo();
			//QY_PLAYER pPlayer = (QY_PLAYER)p0;
			PLAY_AUDIO_procInfo pPlayAudio = pPlayer.audio;
			//byte* buf = (byte*)(System.IntPtr)p1;
			//int bufLen = (int)p2;

			//
			//TCHAR tBuf[128];
			bool bDbg = false;
			string str;

#if DEBUG
			bDbg = true;
#endif

#if true //10
			
			//
			if  (  pPlayAudio.cache.pLeft  !=  pPlayAudio.cache.pBuf  )  {						   	
				if  (  0!=pPlayAudio.cache.nLeft  )  qyFuncs.mymemcpy(  (System.IntPtr)pPlayAudio.cache.pBuf,  (System.IntPtr)pPlayAudio.cache.pLeft,  pPlayAudio.cache.nLeft  );				   
				pPlayAudio.cache.pLeft  =  pPlayAudio.cache.pBuf; 			   
			}

			//
#if DEBUG
#if false
		if  (  pPlayer->idInfo_recorder.ui64Id  ==  110  )  {
			DWORD  dwTickCnt  =  GetTickCount(  );
			TCHAR  tBuf[128];
			//
			int  nNodes =  pPlayAudio->q2.qs[0].uiQNodes  +  pPlayAudio->q2.qs[1].uiQNodes;
			//
			_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "110. myInstream. %d. nLeft %d, bufLen %d, nNodes %d\n"  ),  dwTickCnt,  pPlayAudio->cache.nLeft,  bufLen,  nNodes  );
			OutputDebugString(  tBuf  );
		}
#endif
			if (bDbg)
			{
#if false
				str = string.Format("my_inStream enters");
				qyFuncs.showInfo_open(0, "", str);
#endif
			}
			//
#endif

			//
			int nQNodes_left = 0;
			if (pProcInfo.cfg.b_useAudioQ2_toDecA)
			{
				nQNodes_left = qyFuncs.getQ2Nodes(pPlayAudio.q2);
			}
			else
            {
				nQNodes_left = pPlayAudio.qBuf2.getQ2Nodes();
            }
			//
			bool bAdjust = false;

			//  2017/06/21
			uint dwTickCnt = qyFuncs.myGetTickCount(null);
			int iElapseInMs = (int)(dwTickCnt - pPlayAudio.lowLatency.dwTgt_start1);
			int nIntervalInMs_toAdjust = 5000;
			nIntervalInMs_toAdjust  =  4000;
			//
			iElapseInMs = Math.Abs(iElapseInMs);
			//
			bool bNeedReset = false;


			//
			int iElapseInMs_stat = Math.Abs((int)(dwTickCnt - pPlayAudio.lowLatency.stat.dwTickCnt_start));
			int nIntervalInMs_stat = 30000;
			if ( iElapseInMs_stat > nIntervalInMs_stat)
            {
				//
				pPlayAudio.lowLatency.stat.iIntervalInMs_xql_mql=(uint)Math.Abs((int)(pPlayAudio.lowLatency.stat.dwTickCnt_xql-pPlayAudio.lowLatency.stat.dwTickCnt_mql));

				if (bDbg)
				{

					str = string.Format("my_in:{0}:int_xm {1}，xql {2},mql {3}.", 
						pPlayer.idInfo_recorder.ui64Id,
						pPlayAudio.lowLatency.stat.iIntervalInMs_xql_mql,
						pPlayAudio.lowLatency.stat.xql,
						pPlayAudio.lowLatency.stat.mql
						);

					qyFuncs.showInfo_open(0, "", str);					
				}

				//
				pPlayAudio.lowLatency.stat.dwTickCnt_start = dwTickCnt;
				//
				pPlayAudio.lowLatency.stat.mql = nQNodes_left;
				pPlayAudio.lowLatency.stat.dwTickCnt_mql=dwTickCnt;
				pPlayAudio.lowLatency.stat.xql = nQNodes_left;
				pPlayAudio.lowLatency.stat.dwTickCnt_xql = dwTickCnt;
				//
            }
			else { 
				 if ( nQNodes_left < pPlayAudio.lowLatency.stat.mql)
                {
					pPlayAudio.lowLatency.stat.mql = nQNodes_left;
					pPlayAudio.lowLatency.stat.dwTickCnt_mql = dwTickCnt;
                }
				 if (nQNodes_left > pPlayAudio.lowLatency.stat.xql)
                {
					pPlayAudio.lowLatency.stat.xql = nQNodes_left;
					pPlayAudio.lowLatency.stat.dwTickCnt_xql = dwTickCnt;
                }
			}

			//
			int default_max_mql_ok = 4;
			int default_min_mql_ok = 3;
			int default_max_mql_ok_inMs = 150;
			int default_min_mql_ok_inMs = 100;
			int default_xql_ok_inMs = 2500; //  2000;

#if false
			if ((int)Android.OS.Build.VERSION.SdkInt < 30)
            {
				nIntervalInMs_toAdjust = 20000;
				//
				default_max_mql_ok = 6;
				default_min_mql_ok = 5;
				default_max_mql_ok_inMs = 220;// 300;
				default_min_mql_ok_inMs = 180;
				// default_xql_ok_inMs
			}
#endif



			//
#if false
			if (iElapseInMs > nIntervalInMs_toAdjust + 1500)
			{
				//bNeedReset = true;

				//
				if ( bDbg)
                {
#if false
					str = string.Format("my_inStream: iElapse > nInt_toAdjust + 1000. bNeedReset set to true");
					qyFuncs.showInfo_open(0, "", str);
#endif
                }
				//
			}
			else
#endif
			{

				//			
				if (pPlayAudio.lowLatency.mql > nQNodes_left)
				{
					pPlayAudio.lowLatency.mql = nQNodes_left;
					//
#if __DEBUG__
#if false
						if  (  pProcInfo->cfg.debugStatusInfo.ucbShowPlayAudioStatus  )  {
							_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "mql set to %d"  ),  pPlayAudio->lowLatency.mql  );
							showInfo_open(  0,  0,  tBuf  );
						}
#endif
#endif
				}
				if (pPlayAudio.lowLatency.xql < nQNodes_left) pPlayAudio.lowLatency.xql = nQNodes_left;

				//			
				if (iElapseInMs > nIntervalInMs_toAdjust)
				{
					//
					if (bDbg)
                    {
#if false
						str = string.Format("my_inStream: iElapseInMs > nInt_toAdjust");
						qyFuncs.showInfo_open(0, "", str);
#endif
                    }


					//
					int max_mql_ok = default_max_mql_ok;
					int min_mql_ok = default_min_mql_ok;
					//
					if (0!=pPlayAudio.lowLatency.avg_pkt.nAvg_pkt_inMs)
					{
						int max_mql_ok_inMs = default_max_mql_ok_inMs;
						int min_mql_ok_inMs = default_min_mql_ok_inMs;
						//
						int xql_ok_inMs = default_xql_ok_inMs; //  2000;
							
						//
						int diff_xql_mql = pPlayAudio.lowLatency.xql - pPlayAudio.lowLatency.mql;
						//
						int tmp_max_mql_ok = max_mql_ok_inMs / pPlayAudio.lowLatency.avg_pkt.nAvg_pkt_inMs;
						int tmp_min_mql_ok = min_mql_ok_inMs / pPlayAudio.lowLatency.avg_pkt.nAvg_pkt_inMs;
						pPlayAudio.lowLatency.xql_ok = xql_ok_inMs / pPlayAudio.lowLatency.avg_pkt.nAvg_pkt_inMs;
						//
						if (tmp_max_mql_ok > max_mql_ok) max_mql_ok = tmp_max_mql_ok;
						if (tmp_min_mql_ok > min_mql_ok) min_mql_ok = tmp_min_mql_ok;
						//
						pPlayAudio.lowLatency.max_mql_ok = max_mql_ok;
						pPlayAudio.lowLatency.min_mql_ok = min_mql_ok;
						//
						if (pProcInfo.cfg.debugStatusInfo.ucbShowPlayAudioStatus)
						{   //  2017/10/13
#if false
							str=string.Format("my_inStream: {0}, max_m_ok {1}, min_m_ok {2}. mql {3}.int_adj {4}.", 
								pPlayer.idInfo_recorder.ui64Id,
								max_mql_ok, min_mql_ok, pPlayAudio.lowLatency.mql,
								nIntervalInMs_toAdjust);
							//tmp_max_mql_ok %d, tmp_min_mql_ok %d, tmp_xql_ok %d. tmp_max_mql_ok,  tmp_min_mql_ok,  pPlayAudio->lowLatency.xql_ok,  
							qyFuncs.showInfo_open(0, "", str);
#endif
						}
					}
					//
					if (pPlayAudio.lowLatency.mql > max_mql_ok)
					{
						pPlayAudio.lowLatency.bNeedAdjust = true;

						//
#if false
						qyFuncs.showInfo_open(0, "", "mql>max_mql_ok, bNeedAdjust set to true");
#endif
					}
					//
					if (pPlayAudio.lowLatency.bNeedAdjust)
					{
						if (pPlayAudio.lowLatency.mql <= min_mql_ok)
						{
							pPlayAudio.lowLatency.bNeedAdjust = false;
						}
					}
					//
					if (pPlayAudio.lowLatency.bNeedAdjust)
					{
						if (dwTickCnt - pPlayAudio.lowLatency.dwTickCnt_last_adjust > 200)
						{
							bAdjust = true;
						}
					}

					//
					if (bDbg)
					{
#if false
						str = string.Format("bNeedAdjust {0}", pPlayAudio.lowLatency.bNeedAdjust);
						qyFuncs.showInfo_open(0, "", str);
#endif
					}


					//
					if (!pPlayAudio.lowLatency.bNeedAdjust)
					{
						bNeedReset = true;

						//
						if (bDbg)
                        {
#if false
							str = string.Format("my_inStream: bNeedAdjust is false, bNeedReset set to true");
							qyFuncs.showInfo_open(0, "", str);
#endif
                        }
					}


					//
					if (bAdjust)
					{
						//
						int nSkipped = pPlayAudio.lowLatency.mql - min_mql_ok;
						if (nSkipped > 0)
						{
							//nSkipped = nSkipped / 2;
							//
							//if (nSkipped > 1) nSkipped = 1;
							nSkipped = 1;
							//		
							int i;
							for (i = 0; i < nSkipped; i++)
							{
								bool bSkipped = false;
								//
								if (pProcInfo.cfg.b_useQ2_toPlayA)
								{
									myPLAY_AUDIO_DATA pkt;
									uint pktLen = (uint)sizeof(myPLAY_AUDIO_DATA);
									if (0 == qyFuncs.q2GetMsg(pPlayAudio.q2, (System.IntPtr)(byte*)&pkt, &pktLen))
									{
										//
										M_chk_pts(pPlayAudio, ref pkt);
										//
										qyFuncs.clean_myPLAY_AUDIO_DATA((System.IntPtr)(byte*)&pkt, _T("PlayAudio, bAdjust, to remove old data"));
										//
										bSkipped = true;
										//			
									}
								}
								else
                                {
									int len = (int)(pPlayAudio.cache.uiBufSize - pPlayAudio.cache.nLeft);

									QY_qBuf2.HeadData_playA headData;
									if ( 0==pPlayAudio.qBuf2.q2GetMsg((System.IntPtr)(byte*)&headData, (System.IntPtr)(pPlayAudio.cache.pBuf + pPlayAudio.cache.nLeft), (uint*)&len))
                                    {
										bSkipped = true;
                                    }
                                }
								//
								if (bSkipped)
								{
									if (!pProcInfo.cfg.debugStatusInfo.ucb_notPrint_my_inStream)
									{
										str = string.Format("my_inStream:{0},adjust,remove old,mql {1}.max_mql_ok {2}.nSkip {3}.",
											pPlayer.idInfo_recorder.ui64Id, pPlayAudio.lowLatency.mql, pPlayAudio.lowLatency.max_mql_ok, nSkipped);
										qyFuncs.showInfo_open(0, "", str);
									}
								}

							}
						}
						//
						pPlayAudio.lowLatency.dwTickCnt_last_adjust = dwTickCnt;
					}

				}
			}

			//
			if (bNeedReset)
			{
				pPlayAudio.lowLatency.dwTgt_start1 = dwTickCnt;
				//		
				//
				pPlayAudio.lowLatency.last_mql = pPlayAudio.lowLatency.mql;
				pPlayAudio.lowLatency.last_xql = pPlayAudio.lowLatency.xql;
				//		   
				pPlayAudio.lowLatency.mql = nQNodes_left;
				pPlayAudio.lowLatency.xql = nQNodes_left;
				//
				//  2017/10/13
				//memset(&pPlayAudio->lowLatency.ptsInfo, 0, sizeof(pPlayAudio->lowLatency.ptsInfo)  );
				pPlayAudio.lowLatency.ptsInfo = new PLAY_AUDIO_procInfo.PLAY_AUDIO_procInfo_lowLatency.PLAY_AUDIO_procInfo_ptsInfo();
				//memset(&pPlayAudio->lowLatency.avg_pkt, 0, sizeof(pPlayAudio->lowLatency.avg_pkt)  );
				pPlayAudio.lowLatency.avg_pkt = new PLAY_AUDIO_procInfo.PLAY_AUDIO_procInfo_lowLatency.PLAY_AUDIO_procInfo_avg_pkt();
				//
				if (pProcInfo.cfg.debugStatusInfo.ucbShowPlayAudioStatus)
				{
					str=string.Format("my_inStream: {0}, need Reset. nIntervalInMs_toAdjust {1}ms", pPlayer.idInfo_recorder.ui64Id, nIntervalInMs_toAdjust);
					qyFuncs.showInfo_open(0, null, str);
				}
			}




			//  2017/06/21. to remove too old data
#if false
			int xql_ok = 100;    //  15;	//  6;
								//
			if (pPlayAudio.lowLatency.xql_ok > xql_ok)
			{
				xql_ok = pPlayAudio.lowLatency.xql_ok;
			}
			//
			while (qyFuncs.getQ2Nodes(pPlayer.audio.q2) > xql_ok)
			{
				myPLAY_AUDIO_DATA pkt;
				uint pktLen = (uint)sizeof(myPLAY_AUDIO_DATA);
				if (0!=qyFuncs.q2GetMsg(pPlayAudio.q2, (System.IntPtr)(byte*)&pkt, &pktLen)) break;
				//
				M_chk_pts(pPlayAudio, ref pkt);
				//
				qyFuncs.clean_myPLAY_AUDIO_DATA((System.IntPtr)(byte*)&pkt, _T("PlayAudio,to remove old data"));
				//
				str=string.Format("my_inStream:{0},remove old data, xql_ok {1}, qNodes {2}.", pPlayer.idInfo_recorder.ui64Id, xql_ok, qyFuncs.getQ2Nodes(pPlayer.audio.q2));
				qyFuncs.showInfo_open(0, "", str);
				//
				continue;
			}
#endif

			//
#if __DEBUG__
#if false
				_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "my_inStream: n_left %d"  ),  nQNodes_left  );
				showInfo_open(  0,  0,  tBuf  );
#endif
#endif


			//
			while (pPlayAudio.cache.nLeft < bufLen)
			{
				//
				uint uiPts = 0;        //  2017/10/13
				uint uiDataLen = 0;    //  2017/10/13
									   //
				if (pProcInfo.cfg.b_useQ2_toPlayA)
				{
					myPLAY_AUDIO_DATA pkt;
					uint pktLen = (uint)sizeof(myPLAY_AUDIO_DATA);
					if (0 != qyFuncs.q2GetMsg(pPlayAudio.q2, (System.IntPtr)(byte*)&pkt, &pktLen)) break;
					if (System.IntPtr.Zero == pkt.memory.m_pBuf) break;
					//
					uiPts = pkt.head.uiPts;         //  2017/10/13
					uiDataLen = pkt.uiSize_dec;     //  2017/10/13
					M_chk_pts(pPlayAudio, ref pkt); //  2017/10/13
													//
					int len = (int)(pPlayAudio.cache.uiBufSize - pPlayAudio.cache.nLeft);
					len = (int)Math.Min(len, pkt.uiSize_dec);
					qyFuncs.mymemcpy((System.IntPtr)(pPlayAudio.cache.pBuf + pPlayAudio.cache.nLeft), pkt.memory.m_pBuf, (uint)len);
					uiDataLen = (uint)len;
					qyFuncs.clean_myPLAY_AUDIO_DATA((System.IntPtr)(byte*)&pkt, _T("PlayAudio"));
				}
				else
                {
					QY_qBuf2.HeadData_playA headData;
					int len = (int)(pPlayAudio.cache.uiBufSize - pPlayAudio.cache.nLeft);
					if (0 != pPlayAudio.qBuf2.q2GetMsg((System.IntPtr)(byte*)&headData, (System.IntPtr)(pPlayAudio.cache.pBuf + pPlayAudio.cache.nLeft), (uint*)&len)) break;
					//
					if (len != headData.uiSize_dec)
                    {
						int ii = 0;
                    }
					//
					uiPts = headData.uiPts;
					uiDataLen = headData.uiSize_dec;
                }
				//
				pPlayAudio.cache.nLeft += uiDataLen;// (uint)len;
				//
				pPlayAudio.lowLatency.avg_pkt.nBytes_ptks += (int)uiDataLen;
				pPlayAudio.lowLatency.avg_pkt.n_pkts++;
				if (0!=pPlayAudio.ah.wfx.nAvgBytesPerSec)
				{
					pPlayAudio.lowLatency.avg_pkt.nAvg_pkt_inMs = (int)(pPlayAudio.lowLatency.avg_pkt.nBytes_ptks * 1000.0f  / pPlayAudio.lowLatency.avg_pkt.n_pkts / pPlayAudio.ah.wfx.nAvgBytesPerSec);
				}
				//
				if (pProcInfo.cfg.debugStatusInfo.ucbShowPlayAudioStatus)
				{
					//  2017/10/13
#if false
					//
					int dataInMs = 0;
					if (0!=pPlayAudio.ah.wfx.nAvgBytesPerSec)
					{
						dataInMs = (int)(uiDataLen * 1000 / pPlayAudio.ah.wfx.nAvgBytesPerSec);
					}
					//
					str = "";
					str=string.Format(  "my_inStream:{0}:pts {1},dataLen {2},nQNodes {3},mql {4}.",  pPlayer.idInfo_recorder.ui64Id,  uiPts, uiDataLen,  nQNodes_left,  pPlayAudio.lowLatency.mql  );
					//str=string.Format("my_inStream: %I64u: nErr_pts %d, elapseI_last_dangerMql %dms, uiPts %d, dataInMs %dms. mql %d, xql %d, nQNodes_left %d. "), pPlayer->idInfo_recorder.ui64Id, pPlayer->audio.lowLatency.ptsInfo.nErr_ptsMissed, iElapseInMs_last_dangerousMql, uiPts, dataInMs, pPlayAudio->lowLatency.mql, pPlayAudio->lowLatency.xql, getQ2Nodes(&pPlayer->audio.q2));
					qyFuncs.showInfo_open(0, "", str);
#endif
					//
				}
				//
				pPlayAudio.cache.dwTickCnt_lastAudioData = qyFuncs.myGetTickCount(null);

			}

			//
			if (0!=pPlayAudio.cache.nLeft)
			{
				//
				int len = (int)Math.Min(bufLen, pPlayAudio.cache.nLeft);
				qyFuncs.mymemcpy((System.IntPtr)buf, (System.IntPtr)pPlayAudio.cache.pLeft, (uint)len);
				//
				//QY_MC* pQyMc = QY_GET_GBUF();
#if false
				if (pQyMc.appParams.bConfServer)
				{
					int wBits = 16;
					FillMemory(buf, len, (wBits == 8) ? 128 : 0);
				}
#endif
				//
				pPlayAudio.cache.pLeft += len;
				pPlayAudio.cache.nLeft -= (uint)len;
				nRead = len; goto errLabel;
			}
#endif

			//
			if (dwTickCnt - pPlayAudio.cache.dwTickCnt_lastAudioData > (uint)pProcInfo.cfg.usTimeoutInMs_player_a)
			{
				//
				str=string.Format("{0}, {1},{2}: no data arrives too long, need stop running", pPlayer.idInfo_recorder.ui64Id, pPlayAudio.uiTranNo, pPlayer.video.uiTranNo);

				qyFuncs.showInfo_open(0, _T("my_inStream"), str);

				//  2018/06/16
				pPlayAudio.bNeedStop_webRtc_my_inStream = true;
				//pProcInfo.ctrl.bNeedChkPlayers = true;

				//
				nRead = 0; goto errLabel;
			}

			//
			int wBits = 16;
			FillMemory((System.IntPtr)buf, bufLen, (wBits == 8) ? 128 : 0);
			nRead = bufLen;

			//  2017/10/14
			if (!pProcInfo.cfg.debugStatusInfo.ucb_notPrint_my_inStream  
				)
			{
				str=string.Format("{0}, fill sicence data because no audio data, {1} bytes", pPlayer.idInfo_recorder.ui64Id, nRead);
				qyFuncs.showInfo_open(0, "", str);
# if  __DEBUG__
				//traceLog((TCHAR*)_T("my_inStream: %s"), tBuf);
#endif
			}

		//
		errLabel:

			if (0==nRead)
			{
				int ii = 1;
			}
#if DEBUG
			if (bDbg)
			{
#if false

				str = string.Format("my_inStream leaves,nRead {0}", nRead);
				qyFuncs.showInfo_open(0, "", str);
#endif
			}
#endif

				return nRead;
		}


	}


}