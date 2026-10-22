using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

//
using qy;


namespace qy.Droid
{


#if false

	unsafe struct TMP_buf_myDRAW_VIDEO_DATA_pkts_playVideo
	{
		public fixed byte buf[Consts.bufSize_myDRAW_VIDEO_DATA_pkts_playVideo];
	};

	partial class qyFuncs_Droid
{

		//
		public static unsafe int getSampleTimeInMs_needed(CCtxQmc pProcInfo, int uiSampleTimeInMs_audio, uint dwTickCnt_audio_syncAv, int uiSampleTimeInMs_lastVideo, uint dwTickCnt_lastVideo_internal, uint dwTickCnt_now_syncAv, uint dwTickCnt_now_internal, int uiSampleTimeInMs_toPlay, uint* pdwToInMs_toPlay)
		{
			int iErr = -1;
			int uiSampleTimeInMs_now = 0;
			int nWaitInMs = 0;
			int nWaitInMs_internal = 0;
			//  int		dwTickCnt_toPlay_internal		=	0;
			uint dwToInMs_toPlay = 0;

			if (dwTickCnt_now_syncAv > dwTickCnt_audio_syncAv + 10000)
			{       //  2009/05/27
#if __DEBUG__
				//  traceLogA(  "getSampleTimeInMs_needed: audio is lost.dwTickCnt_now %d  -  dwTickCnt_audio %d is %d",  dwTickCnt_now,  dwTickCnt_audio,  dwTickCnt_now  -  dwTickCnt_audio  );
#endif
				nWaitInMs = -1;
			}
			else
			{
				uiSampleTimeInMs_now = (int)(uiSampleTimeInMs_audio + dwTickCnt_now_syncAv - dwTickCnt_audio_syncAv);
				if (uiSampleTimeInMs_toPlay <= uiSampleTimeInMs_now)
				{
					nWaitInMs = 0;
				}
				else
				{
					nWaitInMs = uiSampleTimeInMs_toPlay - uiSampleTimeInMs_now;
				}
			}

			//
			uiSampleTimeInMs_now = (int)(uiSampleTimeInMs_lastVideo + dwTickCnt_now_internal - dwTickCnt_lastVideo_internal);
			nWaitInMs_internal = uiSampleTimeInMs_toPlay - uiSampleTimeInMs_now;
			if (nWaitInMs_internal < 0) nWaitInMs_internal = 0;

			if (nWaitInMs < 0)
			{
				//  
				dwToInMs_toPlay = (uint)nWaitInMs_internal;
			}
			else
			{
				//  
				float fa = 0.4f;
				dwToInMs_toPlay = (uint)(nWaitInMs * fa + nWaitInMs_internal * (1 - fa));
			}


			//  2014/06/24	
			if (pProcInfo.cfg.debugStatusInfo.ucbShowPreDrawStatus)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T("getSTInMs_needed:"), _T(""), _T("st_a %dms, tc_a %dms, st_v %dms, to_toPlay_i %dms, now_i %dms, nWait_a %dms, nWait_i %dms."), uiSampleTimeInMs_audio, dwTickCnt_audio_syncAv, uiSampleTimeInMs_toPlay, dwToInMs_toPlay, dwTickCnt_now_internal, nWaitInMs, nWaitInMs_internal);
			}


			//
			iErr = 0;

		errLabel:

			if (0==iErr)
			{

				//  restricted.
				//if  (  dwTickCnt_toPlay_internal  >  dwTickCnt_now_internal  +  10000  )  
				if (dwToInMs_toPlay > 10000)
				{
#if __DEBUG__
					traceLogA("getSampleTimeInMs_needed: dwToInMs %d. too long. uiSampleTimeInMs_toPlay %d, thrown.", dwToInMs_toPlay, uiSampleTimeInMs_toPlay);
#endif
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("getSTInMs_needed: dwToInMs %d, too long. ST_toPlay %d, ST_audio %d."), dwToInMs_toPlay, uiSampleTimeInMs_toPlay, uiSampleTimeInMs_audio);

					//  dwTickCnt_toPlay_internal  =  dwTickCnt_now_internal;
					dwToInMs_toPlay = 0;
				}

				//  *pTickCnt_toPlay_internal  =  dwTickCnt_toPlay_internal;
				*pdwToInMs_toPlay = dwToInMs_toPlay;
			}
			return iErr;
		}


		//  2015/09/05
		const int CONST_nInMs_toGetFps = 1000;  //  2015/09/05	//  1000

		//
		const int MIN_fps_pts = 5;      //	2015/09/10	//  7		//  2015/06/20	//  10	
		const int MIN_realFps_pts = 5;		//  2015/09/10	//  7		//  2015/06/20	//  10	

		static unsafe int player_get_fps(QY_MESSENGER_ID pIdInfo, ushort usFps_expected, ref TMP_fps_info pFpsInfo, string pHint)
		{
			//if (null==pIdInfo) return -1;

			if (null==pHint) pHint = ("");

			pFpsInfo.iCount++;
			pFpsInfo.curTimeInMs = qyFuncs.mytimeGetTime();

			if (pFpsInfo.curTimeInMs - pFpsInfo.lastTimeInMs > CONST_nInMs_toGetFps)
			{
				pFpsInfo.fps_real = (float)pFpsInfo.iCount * 1000 / (pFpsInfo.curTimeInMs - pFpsInfo.lastTimeInMs);

				//
				if (0!=pFpsInfo.iCount)
				{
					pFpsInfo.avgTimePerFrameInMs = (uint)((pFpsInfo.curTimeInMs - pFpsInfo.lastTimeInMs) / pFpsInfo.iCount);
				}
				//
				if (pFpsInfo.avgTimePerFrameInMs > 200) pFpsInfo.avgTimePerFrameInMs = 200;
				//
				//  2015/09/10
				pFpsInfo.avgTimePerFrameInMs_real = pFpsInfo.avgTimePerFrameInMs;
				//
				//  2015/02/20. 大帧率时，尽可能按发送过来的策略播放。可以精确点
				if (usFps_expected > MIN_fps_pts)
				{
					pFpsInfo.avgTimePerFrameInMs = (uint)(1000 / usFps_expected);
				}

				//
				pFpsInfo.lastTimeInMs = pFpsInfo.curTimeInMs;
				pFpsInfo.iCount = 0;

				//
#if __DEBUG__
				//traceLog(  _T(  "player_get_fps: %f. avgTimePerFrameInMs %dms"  ),  pFpsInfo.fps,  pFpsInfo.avgTimePerFrameInMs   );
#endif
				CCtxQyMc pQyMc = g.g_pQyMc;
				CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
				if (pProcInfo.cfg.debugStatusInfo.ucbShowToDrawStatus)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T("player_get_fps:"), _T(""), _T("%I64u,  %f, expected: avgTimePerFrm %dms. real_avg %dms. %s"), pIdInfo->ui64Id, pFpsInfo.fps_real, pFpsInfo.avgTimePerFrameInMs, pFpsInfo.avgTimePerFrameInMs_real, pHint);
				}


			}

			return 0;
		}

		public static int accurateSleep(uint dwMs)
		{
			if (0==dwMs) return 0;

			//
			Thread.Sleep((int)dwMs);

			return 0;
		}

		//
		public static unsafe void mcThreadProc_playVideo(object lpParameter)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			QY_PLAYER pPlayer = null;
			PLAY_VIDEO_procInfo pPlay = null;

			//
			bool bDbg = false;

			//
			PLAYER_ID playerId;

			//

			/*
            if (!ttl.m_bMe)
            {
				bDbg = true;
			}
			playerId = ttl.m_drawMem.param.playerId;

			//
			CTalkDraw myDraw = (CTalkDraw)ttl.mMyDraw;
			*/


			//
			uint dwThreadId = qyFuncs.myGetCurrentThreadId();

			//
			CQyTick tickObj = new CQyTick() ;
			uint dwTgt_lastData = qyFuncs.M_GetTgt_video(tickObj); ;                       // 

			//
			//myDRAW_VIDEO_DATA pkts[CONST_pkts_playVideo];
			TMP_buf_myDRAW_VIDEO_DATA_pkts_playVideo buf_pkts;
			int len_pkts;
			int nPkts;

			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

		
			//
			//  #define		DEFAULT_dwToInMs_playVideo		200
			const int DEFAULT_dwToInMs_playVideo = 210;
			//#define		DEFAULT_dwToInMs_playVideo			100
			const int CONST_video_skippedInterval = 1;  //  2	//  4
			const int CONST_video_ahead = 0;	//  1	//  2	//  10			//  提前量

			uint dwToInMs = DEFAULT_dwToInMs_playVideo;

			uint dwTgt_internal;
			uint dwTgt_internal_toPlay;
			uint dwTickCnt_syncAv_now;

			bool bPktRedirected = false;

			uint uiSampleTimeInMs_audio = 0;
			uint dwTickCnt_syncAv_audio = 0;

			//
			//  2015/03/12. 本地视频没有做pts,是错的。不过，现在先暂时伪造一个，以便用pts来改善播放效果
			uint tmp_localV_pts_faked = 0;

			//
			string str;
			//byte[] rgb_array = null;
			//byte[] yuv_array = null;

			//

			//
			{
				int i=0;
				/*
				for (i = 0; i < pProcInfo.av.usCnt_players; i++)
				{
					if (dwThreadId == pProcInfo.av.pPlayers[i].video.dwThreadId) break;
				}
				if (i == pProcInfo.av.usCnt_players)
				{
					return -1;
				}
				
				pPlayer = &pProcInfo.av.pPlayers[i];
				*/

				//
				pPlayer = (QY_PLAYER)lpParameter;
				if (pPlayer == null) return;
				pPlay = pPlayer.video;

				playerId = pPlayer.getPlayerId();
				if(playerId.uiTranNo_player==0)
                {
					qyFuncs.traceLog("mcThreadProc_playVideo failed, playerId.tn is 0");
					goto errLabel;
                }


				//  2016/02/18
			}

			//
#if DEBUG
			fixed (char* pName = pPlay.q2.cfg.name)
			{
				str = string.Format("playVideo {0} enters", new string(pName));
				qyFuncs.traceLog(str);
			}
#endif

			//  2014/07/03
			//  TMP_fps_info					fpsInfo  =  {0,};	
			ref TMP_fps_info  fpsInfo = ref pPlay.m_var.status.fpsInfo;
			//memset(&fpsInfo, 0, sizeof(fpsInfo));
			pPlay.m_var.status.fpsInfo = new TMP_fps_info();
			//  2015/09/13
			//bool  &  bNeedAdjust  =  pPlay.m_var.lowLatency.bNeedAdjust;

			//
			switch (pPlayer.video.vh.bih.biCompression)
			{
				case Consts.BI_RGB:
				case Consts.CONST_fourcc_YV12:
				case Consts.CONST_fourcc_I420: //  2014/07/21
					break;
				default:
#if __DEBUG__
					char buf[128];
					traceLogA("playVideo: %s failed. only support play rgb or yv12. ", iFourcc2Str(pPlayer.video.vh.bih.biCompression, buf, mycountof(buf)));
#endif
					goto errLabel;
			}

			/*
			BITMAPINFOHEADER bih;
			qyFuncs.makeBmpInfoHeader_rgb(24, pPlayer.video.vh.bih.biWidth, pPlayer.video.vh.bih.biHeight, &bih);
			//if (memcmp(&bih, &pPlayer.video.vh.bih, sizeof(bih))) memcpy(&pPlayer.video.vh.bih, &bih, sizeof(pPlayer.video.vh.bih)  );
			pPlayer.video.vh.bih = bih;
			*/

			//  2011/12/05
			using (CQyMalloc mallocObj_tmpRgbBuf = new CQyMalloc())
			{
				/*
				byte* tmpRgbBuf = null;

				//  2011/12/05
				tmpRgbBuf = (byte*)mallocObj_tmpRgbBuf.mallocf(bih.biSizeImage);
				if (null == tmpRgbBuf)
				{
					//TCHAR tBuf[128];
					//_sntprintf(tBuf, mycountof(tBuf), _T("playVideo failed: tmpRgbBuf.malloc returns null, bih.biSizeImage %d"), bih.biSizeImage);
					//showInfo_open(0, 0, tBuf);
					goto errLabel;
				}
				*/


				// Create the events. We make an extra one for later use 
				// by the output buffer.

				//pPlay.bQuit  =  false;
				pPlay.dwTickCnt_start = qyFuncs.myGetTickCount(null);    //  M_GetTickCount_video(  tickObj  );		//  2010/05/04
				pPlay.bRunning = true;
				pPlayer.uiSampleTimeInMs_start = 0;
				//memset(&pPlay.m_var, 0, sizeof(pPlay.m_var)  );
				pPlay.m_var = new PLAY_VIDEO_procInfo_var();

				//


				//
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, %I64u starts"), pPlay.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id);


				/////////////////
				qyFuncs.emptyQ2(pPlay.q2);

				//  2015/09/14
				//pPlay.m_var.lowLatency.mql.max_ql_ok_avg_100  =  pPlayer.video.usFps  /  2  *  100;

				//
				for (; !pPlay.bQuit;)
				{

					//  2015/09/12
					dwToInMs = DEFAULT_dwToInMs_playVideo;
					//if  (  pPlay.m_var.status.fpsInfo.avgTimePerFrameInMs_real  >  10  )  dwToInMs  =  pPlay.m_var.status.fpsInfo.avgTimePerFrameInMs_real;

					//
					//  2012/03/01, 设置此标志位,使有数据包来时,可以触发
					pPlay.m_var.status.bNoMsgAndWait = true;
					//
					/*
					uint dwEvt = MsgWaitForMultipleObjects(
										 pPlay.q2.cfg.usCnt_hEvents,   //  mycountof(  pPlay.hEvents  ),				// Number of events.
									   pPlay.q2.hEvents,                           // Location of handles.
									   false,                                       // Wait for all?
									   dwToInMs,                                    // How long to wait?
									   0);                                      // Any message is an 
																				// event.
					if (dwEvt == WAIT_FAILED) goto errLabel;
					*/
					qyFuncs.myWaitOne(pPlay.q2.hEvent0, (int)dwToInMs);
					//  2012/03/01
					pPlay.m_var.status.bNoMsgAndWait = false;

					//  2015/09/11
					pPlay.m_var.status.nTimes_wait++;

					//
					dwTgt_internal = qyFuncs.M_GetTgt_video(tickObj);

					//
					if (dwTgt_internal - dwTgt_lastData > (uint)(pProcInfo.cfg.usTimeoutInMs_player_v))
					{
#if DEBUG
						//traceLogA("video data may not arrives, player exits. timeout is %d", pProcInfo.cfg.usTimeoutInMs_player_v);
						qyFuncs.traceLog("video data may not arrives,player exits");
#endif
						break;
					}
					//
#if DEBUG
#endif
					//
					//
					/*
					str=string.Format("playVideo: here,playerId{0},{1}",playerId.index_player,playerId.uiTranNo_player);
					qyFuncs.traceLog(str);
					*/
					
					//
					/*
					if (dwEvt != WAIT_TIMEOUT)
					{
						dwEvt -= WAIT_OBJECT_0;
						switch (dwEvt)
						{
							case 0:
								//  dwTickCnt_lastData  =  dwTickCnt;		//  audio data received
								break;
							case 1:
								//  traceLogA(  "playVideo: event 1."  );
								if (0)  //  2015/09/13
								{
									//  traceLogA(  "playVideo: event 1... "  );
									if (pPlay.m_var.uiSampleTimeInMs)
									{
										if (pPlay.m_var.uiSampleTimeInMs_newRecvd < pPlay.m_var.uiSampleTimeInMs
											|| pPlay.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd > pPlay.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv + 1000)
										{
											CQyQ2Help q2Help;
											qRemoveMsg(q2Help.getQ_toGetMsg(&pPlay.q2), (PF_bCommonHandler)tmpHandler_bOldVideo, 0, pPlayer);
										}
									}

								}
								break;
							default:
								traceLogA("unknown event");
								goto errLabel;
								break;
						}
					}
					*/

					//
					pPlay.m_var.lowLatency.calc.dwTgt_start = 0;

					//
					for (; !pPlay.bQuit ; )
					{
						//  traceLogA(  "playVideo: msg reaved"  );
						len_pkts = Consts.bufSize_myDRAW_VIDEO_DATA_pkts_playVideo;// sizeof(pkts);
						{
							using (CQyQ2Help q2Help = new CQyQ2Help())
							{
								if (0!=qyFuncs.qGetMsg(q2Help.getQ_toGetMsg(pPlay.q2), (IntPtr)buf_pkts.buf, (uint*)&len_pkts))
								{
									dwToInMs = DEFAULT_dwToInMs_playVideo;
									break;
								}
							}
						}
						myDRAW_VIDEO_DATA* pPkts = (myDRAW_VIDEO_DATA*)buf_pkts.buf;
						//  注意：在上面qGetMsg和下面的attach之间，不要添加代码，否则，内存就失去管理了

#if DEBUG
						//  traceLogA(  "playVideo: pkt.SampleTime %d, pktLen %d",  pPkt->head.uiSampleTimeInMs,  pktLen  );			
						//qyFuncs.traceLog("playVideo: get msg ok");
#endif

						using (CQyMalloc_array mallocObj_tmpPkts = new CQyMalloc_array(Consts.CONST_pkts_playVideo))
						{
							//CQyMalloc mallocObj_tmpPkts[CONST_pkts_playVideo];


							bool tmpbErr = false;   //  注意：这里如果attach出错，要继续attach, 但是最后要continue. 以防止pPkts[i]没有机会释放内存
													//
							nPkts = len_pkts / sizeof(myDRAW_VIDEO_DATA);
							for (int i = 0; i < nPkts; i++)
							{
								myDRAW_VIDEO_DATA* pPkt = &pPkts[i];
								if (pPkt->memory.m_pBuf!=null)
								{
									if (0!=mallocObj_tmpPkts.mems[i].attach(ref pPkt->memory))
									{
#if __DEBUG__
										traceLogA(  "mcThreadProc_playVideo: mallocObj.attach failed"  );
#endif
										qyFuncs.clean_myDRAW_VIDEO_DATA((IntPtr)pPkt, ("playVideo"));
										tmpbErr = true;
										continue;
									}
								}
							}
							if (tmpbErr) continue;


							if (0!=(len_pkts % sizeof(myDRAW_VIDEO_DATA)))
							{
								qyFuncs.qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, "", pProcInfo.str_who_showInfo, null, (""), (""), ("playVideo err: len_pkts err"));
#if __DEBUG__
	assert(0);
#endif
								continue;
								//  应该只发个myDRAW_VIDEO_DATA过来
							}



							//  
							dwTgt_lastData = qyFuncs.M_GetTgt_video(tickObj);       //  video data received

							//
							for (int iIndex = 0; iIndex < nPkts; iIndex++)
							{
								myDRAW_VIDEO_DATA* pPkt = &pPkts[iIndex];
								int pktLen = sizeof(myDRAW_VIDEO_DATA);

								//
								if (bDbg)
								{
									int nnn = qyFuncs.getQ2Nodes(pPlayer.video.q2);
									str = string.Format("mcThreadProc_playVideo,pktNo {0}, nNodes {1}\n", pPkt->debugInfo.lPktNo, nnn);
									qyFuncs.traceLog(str);
								}


								//  2015/03/12. 对本地视频，伪造一个pts. 等回头在包生成处产生pts才对
								if (0==pPlayer.idInfo_recorder.ui64Id)
								{
									if (0==pPkt->head.uiPts)
									{
										pPkt->head.uiPts = tmp_localV_pts_faked;
										tmp_localV_pts_faked++;
									}
								}

								//  2015/10/04
								if (pPkt->usPktResType != Consts.CONST_pktResType_sharedTex)
								{
									//
									if (pPkt->head.uiLen < pPlay.vh.bih.biSizeImage)
									{
#if __DEBUG__
			traceLogA(("playVideo err: pkt.head.uiLen %d <  sizeImage %d. skipped"), pPkt->head.uiLen, pPlay.vh.bih.biSizeImage);
#endif
										//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playVideo err: pkt.head.uiLen %d <  sizeImage %d. skipped"), pPkt->head.uiLen, pPlay.vh.bih.biSizeImage);
										continue;
									}
								}

								//  2011/12/05
								if (pPkt->bih.biWidth != pPlay.vh.bih.biWidth
									|| pPkt->bih.biHeight != pPlay.vh.bih.biHeight
									|| 0==pPkt->bih.biSizeImage)
								{
#if DEBUG
									ref myDRAW_VIDEO_DATA tmpPkt = ref *pPkt;
									str=string.Format("playVideo err: pkt {0}x{1} !=  player {2}x{3}. skipped", (int)pPkt->bih.biWidth, (int)pPkt->bih.biHeight, (int)pPlay.vh.bih.biWidth, (int)pPlay.vh.bih.biHeight);
									qyFuncs.traceLog(str);
#endif
									//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playVideo err: pkt %d,%d !=  player %d,%d. skipped"), (int)pPkt->bih.biWidth, (int)pPkt->bih.biHeight, (int)pPlay.vh.bih.biWidth, (int)pPlay.vh.bih.biHeight);
									continue;
								}

								//
#if false
								if (pPkt->usPktResType != Consts.CONST_pktResType_sharedTex)
								{
									if (pPkt->bih.biCompression != Consts.BI_RGB)
									{
										if (pPkt->bih.biSizeImage > bih.biSizeImage)
										{
#if __DEBUG__
			assert(0);
#endif
											continue;
										}
										//
										if (bDbg)
										{
#if false
												str = string.Format("before yuv2rgb {0}", pPkt->debugInfo.lPktNo);
												qyFuncs.traceLog(str);
#endif
										}
										//
										switch (pPkt->bih.biCompression)
										{
											case Consts.CONST_fourcc_YV12:
												qyFuncs.mymemcpy((IntPtr)tmpRgbBuf, pPkt->memory.m_pBuf, (uint)pPkt->bih.biSizeImage);

												pQyMc.yv12ToRgb24((byte*)tmpRgbBuf, (byte*)pPkt->memory.m_pBuf, pPkt->bih.biWidth, pPkt->bih.biHeight, false);//pPlayer.video.ucbFlippedImg  );								 
												break;
											case Consts.CONST_fourcc_I420:
												//
												int rgbSize = qyFuncs.M_sizeImage(32, pPkt->bih.biWidth, pPkt->bih.biHeight);
												/*
												if (rgb_array == null || rgb_array.Length < rgbSize)
												{
													rgb_array = new byte[rgbSize];
												}
												*/

#if true

												int yuvSize = qyFuncs.M_sizeImage(12, pPkt->bih.biWidth, pPkt->bih.biHeight);

												/*
												if (yuv_array == null || yuv_array.Length < yuvSize)
												{
													yuv_array = new byte[yuvSize];
												}
												*/
												//

												//System.Runtime.InteropServices.Marshal.Copy((IntPtr)tmpRgbBuf, yuv_array, 0, yuvSize);

																								
												//GuiShare.pf_i420ToRgb((IntPtr)pPkt->memory.m_pBuf, ttl.m_yuvRgbInfo, 32, true, pPkt->bih.biWidth, pPkt->bih.biHeight, false);

												//
												//GuiShare.pf_dumpRgb32_1(myDraw.mbbRgb32.GetDirectBufferAddress(), pPkt->bih.biWidth, pPkt->bih.biHeight, "kk.rgb");

												//
												if (bDbg)
												{
#if false
														str = string.Format("after yuv2rgb {0}", pPkt->debugInfo.lPktNo);
														qyFuncs.traceLog(str);
#endif
												}

#else

													g_colorSpace.i420ToRgb(tmpRgbBuf, (byte*)pPkt->memory.m_pBuf, 32, true, pPkt->bih.biWidth, pPkt->bih.biHeight, false);

												//
												str = string.Format("after yuv2rgb {0}", pPkt->debugInfo.lPktNo);
												traceLog(str);
												//
												System.Runtime.InteropServices.Marshal.Copy(pPkt->memory.m_pBuf, rgb_array, 0, rgbSize);
												//
												str = string.Format("before fillTex {0}", pPkt->debugInfo.lPktNo);
												traceLog(str);

#endif

												//
												//ttl.tmpFillTex( rgb_array, pPkt->bih.biWidth,pPkt->bih.biHeight);
												//ttl.tmpFillTex( pPkt->bih.biWidth, pPkt->bih.biHeight);


												break;
											default:
#if __DEBUG__
				traceLog(_T("playVideo err: pPkt->bih err"));
#endif
												continue;
												break;
										}
										//
										//memcpy(&pPkt->bih, &pPlay.vh.bih, sizeof(pPkt->bih)  );
										pPkt->bih = pPlay.vh.bih;
										pPkt->uiSize_dec = (uint)pPlay.vh.bih.biSizeImage;

									}
									}
#endif
					
							

								//  2015/09/11
								pPkt->debugInfo.dwTickCnt_playPkt_start = qyFuncs.myGetTickCount(null);

								//  __declspec(  dllexport  )  int  fillData_d3d(  BITMAPINFOHEADER  *  pBih,  byte  *  pData,  int  iIndex_player,  bool  *  pbFilled  )
								//memset(&pPlay.m_var.status.fillD3d, 0, sizeof(pPlay.m_var.status.fillD3d)  );
								pPlay.m_var.status.fillD3d = new PLAY_VIDEO_procInfo_fillD3d();
								if (0!=pPlay.m_var.status.ucSkipped_for_d3dData)
								{
									pPlay.m_var.status.ucSkipped_for_d3dData--;
									//
									pPlay.m_var.status.fillD3d.bNeedFill = false;                         //
									pPlay.m_var.status.fillD3d.bFilled_d3dData = false;
								}
								else
								{
									pPlay.m_var.status.fillD3d.bNeedFill = true;
									//
									if (0!=qyFuncs.dyn_fillData_d3d(pPkt, playerId.index_player, ref pPlay.m_var.status.fillD3d.param0, ref pPlay.m_var.status.fillD3d.bFilled_d3dData))
									{
										pPlay.m_var.status.fillD3d.bFilled_d3dData = false;
									}
								}

								//
								pPlay.m_var.lowLatency.nTimes_sleep = 0;

								//
								if (0==pPkt->head.uiSampleTimeInMs)
								{
									//  2012/02/06
									dwToInMs = 0;       //  2013/09/09
														//
									pPlay.m_var.lowLatency.dwToInMs_toPostDraw = dwToInMs; //  2013/09/09
																							//
									bPktRedirected = false;
									qyFuncs.postToDraw(pProcInfo, pPkt->head.uiTranNo_openAvDev, pPkt, pktLen, &bPktRedirected, pPkt->uiCapType, pPkt->iIndex_capBmp, pPlayer.idInfo_recorder, pMisCnt, &pPkt->idInfo_peer, pPkt->iTaskId, null, &playerId);
									if (bPktRedirected)
									{
										mallocObj_tmpPkts.mems[iIndex].detach();
									}
									//
									str = "";// new string(pPlayer.video.q2.cfg.name);
									player_get_fps(pPlayer.idInfo_recorder, pPlayer.video.usFps, ref fpsInfo, str);  //  2014/07/03

									continue;
								}



								for (; !pPlay.bQuit;)
								{

									//  2012/01/27
									if (uiSampleTimeInMs_audio != pPlayer.audio.m_var.uiSampleTimeInMs)
									{
										uiSampleTimeInMs_audio = pPlayer.audio.m_var.uiSampleTimeInMs;
										dwTickCnt_syncAv_audio = pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_syncAv;
									}

									//
									dwTickCnt_syncAv_now = qyFuncs.M_GetTickCount_av(tickObj);
									dwTgt_internal = qyFuncs.M_GetTgt_video(tickObj);

									//
									bool bNoAudio = false;


									//  这里通常是没有audio的情况							
									if (dwTickCnt_syncAv_now > dwTickCnt_syncAv_audio + 10000)
									{
										bNoAudio = true;
									}
									//																	 
									//_sntprintf(pPlay.m_var.tWhere, mycountof(pPlay.m_var.tWhere), _T("%I64u %s:"), pPlayer.idInfo_recorder.ui64Id, bNoAudio ? _T("no audio") : _T("av"));
									//TCHAR* tWhere = pPlay.m_var.tWhere;
									string tWhere;

									//  2016/04/10
									pPlay.m_var.lowLatency.bNoAudio = bNoAudio;
									//
									pPlay.m_var.lowLatency.bAccurate = false;
									//  2015/09/13							
									pPlay.m_var.lowLatency.bNeedAdjust = false;

									//  2015/09/14
									int nQNodes_left = (int)(pPlay.q2.qs0.uiQNodes + pPlay.q2.qs1.uiQNodes);
									//
									//pPlay.m_var.lowLatency.nQNodes_left_beforePost  =  nQNodes_left;	//  2015/09/17
									//
									pPlay.m_var.lowLatency.nAheadInMs = 0;
									pPlay.m_var.lowLatency.nAheadInMs_gt_mql = 0;  //  2015/03/02
									pPlay.m_var.lowLatency.nDelayInMs_lt_mql = 0;


									//  2015/09/12
									const int MAX_nTimes_sleep = 4; //  3	//  2

									//
									if (pPlay.m_var.lowLatency.nTimes_sleep > MAX_nTimes_sleep)
									{
										dwToInMs = 0;
										//							   	
										dwTgt_internal_toPlay = dwTgt_internal + dwToInMs;
										//
										if (pProcInfo.cfg.debugStatusInfo.ucbShowPreDrawStatus)
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, tWhere, _T("big nTimes_sleep:"), _T("pkt.sm %dms, play.sm %dms, play.sm_new %dms, dwToInMs %dms"), pPkt->head.uiSampleTimeInMs, pPlay.m_var.uiSampleTimeInMs, pPlay.m_var.uiSampleTimeInMs_newRecvd, dwToInMs);
										}

									}
									else
									{
										uint nInterval_pkt = pPkt->head.uiSampleTimeInMs - pPlayer.video.m_var.uiSampleTimeInMs;
										//
										nInterval_pkt = Math.Min(nInterval_pkt, 200);
										//
										int iMax_toInMs = (int)nInterval_pkt;
										int iElapseInMs = (int)(dwTgt_internal - pPlay.m_var.dwTgt_uiSampleTimeInMs_internal);
										if (iMax_toInMs > iElapseInMs) iMax_toInMs -= iElapseInMs;
										else iMax_toInMs = 0;
										//							
										//int	nQNodes_left  =  pPlay.q2.qs[0].uiQNodes  +  pPlay.q2.qs[1].uiQNodes;
										//
										//int		nAheadInMs  =  0;
										//int		nAheadInMs_min_qNodes_left  =  0;	//  2015/03/02


										//  2013/04/08
										const int MAX_interval_videoOnly_1Node = 600;       //  120		//  200	//  1200
																							//
										const int MAX_interval_videoOnly_manyNodes = 80;        //  160		//  80
																								//
										const int MAX_dwToInMs_videoOnly = 40;      //  2013/08/02. 最慢按25fps算

										//
										const int MAX_interval_videoOnly_1Node_pts = 79;        //  120		//  79
										const int MAX_interval_videoOnly_manyNodes_pts = 79;        //  120		//  79														

										//
										int nReason = 0;

										//								
										//  2015/02/20									
										//if  (  pPkt->head.uiPts  &&  fpsInfo.fps  >  MIN_realFps_pts  &&  pPlayer.video.usFps  >  MIN_fps_pts  )  
										if (0!=pPkt->head.uiPts && pPlayer.video.usFps > MIN_fps_pts)
										{
											//
											//  2016/04/10
											bool bAccurate = false;
											if (pPlayer.video.m_var.status.fpsInfo.fps_real >= 15
												&& pPlayer.video.m_var.lowLatency.mql.mql_ok!=0)
											{
												bAccurate = true;
											}
											if (pPlay.m_var.lowLatency.bAccurate != bAccurate)
											{
												pPlay.m_var.lowLatency.bAccurate = bAccurate;
											}


											//  2015/03/02
											ushort tmp_player_usFps = pPlayer.video.usFps;
											if (0==tmp_player_usFps) tmp_player_usFps = 1;


											//
											uint needed_toPlay = 0;
											uint elapse_real = 0;
											{
												//
												if (bAccurate)
												{
													needed_toPlay = (pPkt->head.uiPts - pPlay.m_var.lowLatency.calc.uiPts_start) * fpsInfo.avgTimePerFrameInMs;    //  (  pPkt->head.uiPts  -  pPlay.m_var.status.uiPts_last  )  *  fpsInfo.avgTimePerFrameInMs;										
													elapse_real = dwTgt_internal - pPlay.m_var.lowLatency.calc.dwTgt_start;  //dwTgt_internal  -  pPlay.m_var.status.dwLastTgt_postToDraw;											  
												}
												else
												{
													needed_toPlay = (pPkt->head.uiPts - pPlay.m_var.status.uiPts_last) * fpsInfo.avgTimePerFrameInMs;
													elapse_real = dwTgt_internal - pPlay.m_var.status.dwLastTgt_postToDraw;
												}
												//
												if (elapse_real > needed_toPlay)
												{
													//
													//
													dwToInMs = 0;
													if (0==pPlay.m_var.lowLatency.nTimes_sleep)
													{
														dwToInMs = fpsInfo.avgTimePerFrameInMs * 4 / 5;
													}
													nReason = 1;
												}
												else
												{
													dwToInMs = needed_toPlay - elapse_real;
												}
												//
#if __DEBUG__
				//  2016/04/11
				pPlay.m_var.lowLatency.dwToInMs_1740 = dwToInMs;
#endif
											}
											//
											int max_to = (int)fpsInfo.avgTimePerFrameInMs;   //  +  3;
											if (dwToInMs > max_to)
											{
												dwToInMs = (uint)max_to;
											}


											//  2015/09/11
											//  2015/09/12
											//#define		CONST_qNodes_left_bottom		2	//  1	

											if (nQNodes_left <= pPlay.m_var.lowLatency.mql.mql_ok)
											{
												if (0!=fpsInfo.avgTimePerFrameInMs_real)
												{
													//
													int tmp_nInt = 0;   //  
																		//
													if (0!=pPlay.m_var.lowLatency.mql.mql_ok)
													{
														tmp_nInt = (int)((pPlay.m_var.lowLatency.mql.mql_ok - nQNodes_left) * (fpsInfo.avgTimePerFrameInMs / 6 + 2));
													}
													//
													int i_add = 0;
													if (0==nQNodes_left)
													{
														if (pPlay.usFps <= 15)
														{
															i_add = (int)fpsInfo.avgTimePerFrameInMs / 3;
														}
														else
														{
															i_add = (int)fpsInfo.avgTimePerFrameInMs / 5; //20;//fpsInfo.avgTimePerFrameInMs_real  /  8;
														}
													}
													else
													{
														if (pPlay.usFps <= 15)
														{
															//  i_add  =  fpsInfo.avgTimePerFrameInMs  /  5;
														}
														else
														{
															i_add = (int)fpsInfo.avgTimePerFrameInMs / 8;
														}
													}
													//
													if (0==pPlayer.idInfo_recorder.ui64Id)
													{
														if (!pPkt->input.bRtsp)
														{               //  2015/03/12. 只有纯本地采集卡才能降到0. 不能补偿
															i_add = 0;
														}
													}
													//
													int nDelay = tmp_nInt + i_add;
													nDelay = Math.Min(nDelay, 20);
													pPlay.m_var.lowLatency.nDelayInMs_lt_mql = nDelay;
													//
													if (pPlay.m_var.lowLatency.nDelayInMs_lt_mql > fpsInfo.avgTimePerFrameInMs / 2)
													{
														pPlay.m_var.lowLatency.nDelayInMs_lt_mql = (int)fpsInfo.avgTimePerFrameInMs / 2;
													}
													//
													if (0!=pPlay.m_var.lowLatency.nDelayInMs_lt_mql)
													{
														pPlay.m_var.lowLatency.bNeedAdjust = true;
													}

												}
											}
											else if (nQNodes_left <= pPlay.m_var.lowLatency.mql.mql_big_ok)
											{
												// ok
												int ii = 0;
											}
											else
											{

												//											   										
												int d_st = 0;


												if (pPlay.m_var.lowLatency.mql.bNeedReset)
												{

													//																			   
													if (bNoAudio)
													{

														//

														//
														int max_ql_ok = pPlay.m_var.lowLatency.mql.xql_ok;
														//
														if (nQNodes_left > max_ql_ok)
														{
															int n = nQNodes_left - max_ql_ok;

															int tmp_i = (int)(n * fpsInfo.avgTimePerFrameInMs / tmp_player_usFps / 2);  //  用2秒消掉多余的包
																																 //
															if (tmp_i < 2) tmp_i = 2;
															//else  if  (  tmp_i  >  15  )  tmp_i  =  15;
															//
															if (0!=tmp_i)
															{
																tmp_i = Math.Min(tmp_i, 100);
																pPlay.m_var.lowLatency.nAheadInMs = tmp_i;
																//
																if (!pPlay.m_var.lowLatency.bNeedAdjust)
																{
																	pPlay.m_var.lowLatency.bNeedAdjust = true;
																	nReason = 5;
																}
															}
														}
													}
													else
													{  // audio

														//
														uint st_audio_now = (dwTickCnt_syncAv_now - dwTickCnt_syncAv_audio) + uiSampleTimeInMs_audio;
														d_st = (int)(st_audio_now - pPkt->head.uiSampleTimeInMs);
														//
														if (d_st > 100)
														{
															pPlay.m_var.lowLatency.bNeedAdjust = true;
															nReason = 14;
															//
															pPlay.m_var.lowLatency.nAheadInMs = d_st / 100 * 3 / 2;
														}
													}

													//									 
													if (0==pPlay.m_var.lowLatency.nAheadInMs)
													{
														//  2015/03/02. 下面这个减少用来防止包的堆积											   
														//  2015/09/17
														int n = nQNodes_left - pPlay.m_var.lowLatency.mql.mql_ok;
														int kk = pPlay.m_var.lowLatency.mql.last_mql - pPlay.m_var.lowLatency.mql.mql_ok;
														if (n > 0 && kk > 0)
														{

															//													
															int tmp_i = 0;
															//
															if (pPlay.usFps <= 15)
															{
																tmp_i = (int)(kk * fpsInfo.avgTimePerFrameInMs / tmp_player_usFps * 2);
																if (0==tmp_i) tmp_i = 1;
															}
															else
															{
																tmp_i = (int)(kk * fpsInfo.avgTimePerFrameInMs / tmp_player_usFps / 2);  //  用2秒消掉多余包														
																if (0==tmp_i) tmp_i = 1;
															}

															//													
															if (0!=tmp_i)
															{
																tmp_i = Math.Min(tmp_i, 10);
																pPlay.m_var.lowLatency.nAheadInMs_gt_mql = tmp_i;
																//
																if (!pPlay.m_var.lowLatency.bNeedAdjust)
																{
																	pPlay.m_var.lowLatency.bNeedAdjust = true;
																	nReason = 20;
																}
															}

														}
													}


												}


											}
											//
											//  2015/0/09/16										 										
											//										 									 //										
											if (pPlay.m_var.lowLatency.bNeedAdjust)
											{

												//  2016/04/11
												if (0!=pPlay.m_var.lowLatency.nDelayInMs_lt_mql)
												{
													dwToInMs += (uint)pPlay.m_var.lowLatency.nDelayInMs_lt_mql;
												}
												//																															
												if (0!=pPlay.m_var.lowLatency.nAheadInMs_gt_mql)
												{
													if (dwToInMs >= pPlay.m_var.lowLatency.nAheadInMs_gt_mql) dwToInMs -= (uint)pPlay.m_var.lowLatency.nAheadInMs_gt_mql;
													else dwToInMs = 0;
												}
												//
												if (0!=pPlay.m_var.lowLatency.nAheadInMs)
												{
													if (dwToInMs > pPlay.m_var.lowLatency.nAheadInMs) dwToInMs = (uint)(dwToInMs - pPlay.m_var.lowLatency.nAheadInMs);
													else dwToInMs = 0;
												}

												//										
												if (pProcInfo.cfg.debugStatusInfo.ucbShowPreDrawStatus)
												{
													//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, tWhere, _T(""), _T("adjusted. nReason %d. nNodes_left %d, nAhead %d, mql %d, nAheadInMs.mql %d, dwToInMs %d"), nReason, nQNodes_left, pPlay.m_var.lowLatency.nAheadInMs, pPlay.m_var.lowLatency.mql.mql, pPlay.m_var.lowLatency.nAheadInMs_gt_mql, dwToInMs);
												}

											}



											//
										}       //  >10fps
										else
										{   //  low fps video


											if (bNoAudio)
											{


												//	
												float f = 0.3f;
												if (nQNodes_left > 0)
												{
													if (pPlay.m_var.uiSampleTimeInMs_newRecvd > pPkt->head.uiSampleTimeInMs + MAX_interval_videoOnly_manyNodes)
													{
														f = 0.17f;
													}
												}
												else
												{
													if (pPlay.m_var.uiSampleTimeInMs_newRecvd > pPkt->head.uiSampleTimeInMs + MAX_interval_videoOnly_1Node)
													{
														f = 0.05f;       //  把过老的数据扔出去									
													}
												}
												//									
												dwToInMs = pPkt->head.uiSampleTimeInMs - pPlay.m_var.uiSampleTimeInMs;
												//
												if (dwToInMs > iElapseInMs) dwToInMs -= (uint)iElapseInMs;
												else dwToInMs = 0;
												//										
												int iMax_dwToInMs_videoOnly = (int)Math.Min(MAX_dwToInMs_videoOnly, fpsInfo.avgTimePerFrameInMs);

												//  
												if (dwToInMs > iMax_dwToInMs_videoOnly) dwToInMs = (uint)(iMax_dwToInMs_videoOnly + (dwToInMs - iMax_dwToInMs_videoOnly) * f);

												//									 
												//
												if (nInterval_pkt > fpsInfo.avgTimePerFrameInMs + 1)
												{
													int iVal = (int)(nInterval_pkt - (fpsInfo.avgTimePerFrameInMs + 1));
													for (; iVal!=0; iVal--)
													{
														if (dwToInMs!=0) dwToInMs--;
														if (iMax_toInMs!=0) iMax_toInMs--;
													}
												}
												else if (nInterval_pkt < fpsInfo.avgTimePerFrameInMs)
												{
													if (nQNodes_left < pPlayer.video.q2.cfg.uiMaxQNodes * 2 / 3)
													{
														dwToInMs++;
													}
												}

												//
												int iMin = (int)Math.Min(iMax_toInMs, fpsInfo.avgTimePerFrameInMs - 3);
												int iMax = (int)Math.Min(iMax_toInMs, fpsInfo.avgTimePerFrameInMs + 2);

												if (dwToInMs < iMin) dwToInMs = (uint)iMin;
												else if (dwToInMs > iMax)
												{
													dwToInMs = (uint)iMax;
												}

												//  2015/06/20
												int tmp_fps_2 = (int)fpsInfo.fps_real / 2;   //  以半秒为界来衰减
												if (nQNodes_left > tmp_fps_2)
												{
													//
													if (tmp_fps_2 <= 0) tmp_fps_2 = 1;
													int n = nQNodes_left / tmp_fps_2;
													//
													if (n <= 0) n = 1;
													dwToInMs /= (uint)n;
												}

												//											 
												if (dwToInMs > 1) dwToInMs -= 1;

												//
												if (pProcInfo.cfg.debugStatusInfo.ucbShowPreDrawStatus)
												{
													//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, tWhere, _T(""), _T("fps_real %.2f. nNodes_left %d, nAhead %d, dwToInMs %d"), fpsInfo.fps_real, nQNodes_left, pPlay.m_var.lowLatency.nAheadInMs, dwToInMs);
												}

											}

											else
											{

												//							
												getSampleTimeInMs_needed(pProcInfo, (int)uiSampleTimeInMs_audio, dwTickCnt_syncAv_audio, (int)pPlay.m_var.uiSampleTimeInMs, pPlay.m_var.dwTgt_uiSampleTimeInMs_internal, dwTickCnt_syncAv_now, dwTgt_internal, (int)pPkt->head.uiSampleTimeInMs, &dwToInMs);
												//
												if (nInterval_pkt > fpsInfo.avgTimePerFrameInMs + 1)
												{
													int iVal = (int)(nInterval_pkt - (fpsInfo.avgTimePerFrameInMs + 1));
													for (; 0!=iVal; iVal--)
													{
														if (0!=dwToInMs) dwToInMs--;
														if (0!=iMax_toInMs) iMax_toInMs--;
													}
												}
												else if (nInterval_pkt < fpsInfo.avgTimePerFrameInMs)
												{
													if (nQNodes_left < pPlayer.video.q2.cfg.uiMaxQNodes * 2 / 3)
													{
														dwToInMs++;
													}
												}
												//
												int iMin = (int)Math.Min(iMax_toInMs, fpsInfo.avgTimePerFrameInMs - 3);
												int iMax = (int)Math.Min(iMax_toInMs, fpsInfo.avgTimePerFrameInMs + 2);

												if (dwToInMs < iMin)
												{
													int iAhead = 0;

													if (0!=nQNodes_left)
													{
														if (0!=iMin)
														{
															iAhead = 1;
														}
													}
													dwToInMs = (uint)(iMin - iAhead);
												}
												else if (dwToInMs > iMax)
												{
													int iDelay = 0;

													//
													if (nQNodes_left < pPlay.q2.cfg.uiMaxQNodes * 2 * 2 / 3)
													{

														if (dwToInMs > fpsInfo.avgTimePerFrameInMs + 3)
														{
															iDelay = 2;
														}
														else
														{
															iDelay = 1;
														}
													}

													dwToInMs = (uint)(iMax + iDelay);
												}
											}
										}

										//								
										dwTgt_internal_toPlay = dwTgt_internal + dwToInMs;

										//								
										if (pProcInfo.cfg.debugStatusInfo.ucbShowPreDrawStatus)
										{
											/*
											qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, tWhere, _T(""), _T("pkt.sm %d, play.sm %d, play.sm_new %d. nQNodes_left %d,  nAheadInMs %d, mql %d, nAheadIn.mql %d, dwToInMs %d. bAdjust %d, nReason %d"),
																																				   pPkt->head.uiSampleTimeInMs, pPlay.m_var.uiSampleTimeInMs, pPlay.m_var.uiSampleTimeInMs_newRecvd,
																																				   nQNodes_left,
																																				   pPlay.m_var.lowLatency.nAheadInMs,
																																				   pPlay.m_var.lowLatency.mql.mql,
																																				   pPlay.m_var.lowLatency.nAheadInMs_gt_mql,
																																				   dwToInMs,
																																				   pPlay.m_var.lowLatency.bNeedAdjust, nReason);
											*/
										}
									}

									if (dwTgt_internal_toPlay <= dwTgt_internal + CONST_video_skippedInterval)
									{
										//  2012/02/07
										//  pPkt->debugInfo.dwTickCnt_postToDraw  =  M_GetTickCount_video(  tickObj  );
										if (dwTgt_internal_toPlay > dwTgt_internal) dwToInMs = dwTgt_internal_toPlay - dwTgt_internal;
										else dwToInMs = 0;      //  2013/09/09						   
																//
										pPlay.m_var.lowLatency.dwToInMs_toPostDraw = dwToInMs; //  2013/09/09
																								//
										bPktRedirected = false;
										qyFuncs.postToDraw(pProcInfo, pPkt->head.uiTranNo_openAvDev, pPkt, pktLen, &bPktRedirected, pPkt->uiCapType, pPkt->iIndex_capBmp, pPlayer.idInfo_recorder, pMisCnt, &pPkt->idInfo_peer, pPkt->iTaskId, null, &playerId);
										if (bPktRedirected)
										{
											mallocObj_tmpPkts.mems[iIndex].detach();

										}

										//  2009/05/26
										pPlay.m_var.uiSampleTimeInMs = pPkt->head.uiSampleTimeInMs;
										pPlay.m_var.dwTgt_uiSampleTimeInMs_internal = dwTgt_internal;  //  M_GetTickCount_video(  tickObj  );
										pPlay.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv = qyFuncs.myGetTickCount(null);
										//
										player_get_fps(pPlayer.idInfo_recorder, pPlayer.video.usFps, ref fpsInfo, "");  //  2014/07/03

										break;
									}


									//
									dwToInMs = dwTgt_internal_toPlay - dwTgt_internal;

									//  2012/01/28, 有一个很小的提前量,因为从恢复休眠到显示,也需要时间
									if (dwToInMs > CONST_video_ahead)
									{
										//
										const int MAX_waitToPlay = 40;  //  125		//  2015/09/11		//  40
																		//
										int iMax_waitToPlay = (int)Math.Min(MAX_waitToPlay, fpsInfo.avgTimePerFrameInMs);
										//
										if (dwToInMs > iMax_waitToPlay)
										{

											//
											pPlay.m_var.lowLatency.nTimes_sleep++;

											//
											if (iMax_waitToPlay > CONST_video_ahead) iMax_waitToPlay -= CONST_video_ahead;
											accurateSleep((uint)iMax_waitToPlay);
											continue;
										}
										else
										{
											accurateSleep(dwToInMs - CONST_video_ahead);
										}
									}


									//  if  (  bPktLeft  )  
									{
										//
										//  pPkt->debugInfo.dwTickCnt_postToDraw  =  M_GetTickCount_video(  tickObj  );
										pPlay.m_var.lowLatency.dwToInMs_toPostDraw = dwToInMs;
										//
										bPktRedirected = false;
										qyFuncs.postToDraw(pProcInfo, pPkt->head.uiTranNo_openAvDev, pPkt, pktLen, &bPktRedirected, pPkt->uiCapType, pPkt->iIndex_capBmp, pPlayer.idInfo_recorder, pMisCnt, &pPkt->idInfo_peer, pPkt->iTaskId, null, &playerId);
										if (bPktRedirected)
										{
											mallocObj_tmpPkts.mems[iIndex].detach();
										}
										//  bPktLeft  =  false;

										//  2009/05/26
										pPlay.m_var.uiSampleTimeInMs = pPkt->head.uiSampleTimeInMs;
										pPlay.m_var.dwTgt_uiSampleTimeInMs_internal = qyFuncs.M_GetTgt_video(tickObj); //  dwTickCnt_toPlay;
										pPlay.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv = qyFuncs.myGetTickCount(null);
										//
										player_get_fps(pPlayer.idInfo_recorder, pPlayer.video.usFps, ref fpsInfo, "");  //  2014/07/03

									}
									break;                 //  一个pkt被播放完了					   
								}
							}

							//
#if __DEBUG__
					        for (int i = 0; i < nPkts; i++)
{
	QY_MEMORY memory;
	if (mallocObj_tmpPkts[i].get(&memory)) continue;
	if (memory.m_pBuf && memory.uiBufSize)
	{
		M_pkt_showNo(pkts[i], _T("playVideo . free"));
	}
}
#endif

						}
					}

					continue;
				}

			}

			//
			str=string.Format("mcThreadProc_playVideo will quit,pPlay.bQuit {0}. index_player {1}",pPlay.bQuit, playerId.index_player);
			qyFuncs.traceLog(str);

errLabel:

pPlay.bRunning = false;


//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, %I64u ends."), pPlayer.video.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id);

//traceLogA("playVideo %S leaves", pPlay.q2.cfg.name);

return;

}



	





	}

#endif

}