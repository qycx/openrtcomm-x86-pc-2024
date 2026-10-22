using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
	partial class qyFuncs_Droid
{

		public static unsafe int ttl_readPkt(CTalkDraw.TalkTextureLoader ttl)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			QY_PLAYER pPlayer = null;
			PLAY_VIDEO_procInfo pPlay = null;



			//
			bool bDbg = false;
			if (!ttl.m_bMe)
			{
				bDbg = true;
			}
			else
			{
				//bDbg = true;
			}

			//
			PLAYER_ID playerId = ttl.m_drawMem.param.playerId;

			//
			CTalkDraw myDraw = (CTalkDraw)ttl.mMyDraw;


			//
			uint dwThreadId = qyFuncs.myGetCurrentThreadId();

			//
			CQyTick tickObj = new CQyTick();
			uint dwTgt_lastData = qyFuncs.M_GetTgt_video(tickObj); ;                       // 

			//
			int nPkts;

			MIS_CNT pMisCnt = pProcInfo.pMisCnt;


			//
			//  #define		DEFAULT_dwToInMs_playVideo		200
			const int DEFAULT_dwToInMs_playVideo = 33;// 210;
													  //#define		DEFAULT_dwToInMs_playVideo			100
			const int CONST_video_skippedInterval = 1;  //  2	//  4
			const int CONST_video_ahead = 0;    //  1	//  2	//  10			//  提前量

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
				int i = 0;

				pPlayer = qyFuncs.getPlayerByIndex(pProcInfo, playerId.index_player);
				if (pPlayer == null) return -1;
				//pPlayer = pProcInfo.av.localAv.player;
				pPlay = pPlayer.video;

				//  2016/02/18
			}

			//
#if DEBUG
			if (bDbg)
			{
				fixed (char* pName = pPlay.q2.cfg.name)
				{
					str = string.Format("ttl_readPkt {0} enters", new string(pName));
				}
				qyFuncs.showInfo_open(0, null, str);
			}
#endif


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



				//
				for (; !pPlay.bQuit && !myDraw.bQuit;)
				{

					//  2015/09/12
					dwToInMs = DEFAULT_dwToInMs_playVideo;
					//if  (  pPlay.m_var.status.fpsInfo.avgTimePerFrameInMs_real  >  10  )  dwToInMs  =  pPlay.m_var.status.fpsInfo.avgTimePerFrameInMs_real;

					qyFuncs.myWaitOne(pPlay.displayQ2.hEvent0, (int)dwToInMs);

					//
					dwTgt_internal = qyFuncs.M_GetTgt_video(tickObj);

					//
					/*
					if (dwTgt_internal - dwTgt_lastData > (uint)(pProcInfo.cfg.usTimeoutInMs_player_v))
					{
#if DEBUG
						//traceLogA("video data may not arrives, player exits. timeout is %d", pProcInfo.cfg.usTimeoutInMs_player_v);
						qyFuncs.traceLog("video data may not arrives,ttl_readPkt exits");
#endif
						break;
					}
					*/
					//
#if DEBUG
					if (ttl.m_bMe)
					{

					}
					else
					{
						int ii = 0;
					}
#endif
					//
					if (ttl.bDrawModified())
					{
						str = string.Format("ttl_readPkt will quit: myDraw.bLayoutModified");
						qyFuncs.showInfo_open(0, null, str);
						//
						goto errLabel;
					}
					//
					/*
					str=string.Format("playVideo: here,playerId{0},{1}",playerId.index_player,playerId.uiTranNo_player);
					qyFuncs.traceLog(str);
					*/

					//
					for (; !pPlay.bQuit && !myDraw.bQuit;)
					{
						//  traceLogA(  "playVideo: msg reaved"  );
						MIS_MSG_procVideo_qmc msg_pkt;
						int len_pkt = sizeof(MIS_MSG_procVideo_qmc);
						{
							if (0 != qyFuncs.q2GetMsg(pPlay.displayQ2, (IntPtr)(byte*)&msg_pkt, (uint*)&len_pkt))
							{
								//
								if (bDbg)
								{
									str = string.Format("ttl_readPkt: displayQ2 is empty. {0}", ttl.m_tag);
									qyFuncs.showInfo_open(0, null, str);
								}
								//
								dwToInMs = DEFAULT_dwToInMs_playVideo;
								break;
							}
						}
						myDRAW_VIDEO_DATA* pPkts = (myDRAW_VIDEO_DATA*)&msg_pkt.pkt;
						//  注意：在上面qGetMsg和下面的attach之间，不要添加代码，否则，内存就失去管理了

#if DEBUG
						if (bDbg)
						{
							//str=string.Format(  "playVideo: pkt.SampleTime %d, pktLen %d",  pPkt->head.uiSampleTimeInMs,  pktLen  );			
							str = string.Format("playVideo: get msg ok");
							qyFuncs.showInfo_open(0, null, str);
						}
#endif

						using (CQyMalloc_array mallocObj_tmpPkts = new CQyMalloc_array(1))
						{
							//CQyMalloc mallocObj_tmpPkts[CONST_pkts_playVideo];


							bool tmpbErr = false;   //  注意：这里如果attach出错，要继续attach, 但是最后要continue. 以防止pPkts[i]没有机会释放内存
													//
							nPkts = 1;
							for (int i = 0; i < nPkts; i++)
							{
								myDRAW_VIDEO_DATA* pPkt = &pPkts[i];
								if (pPkt->memory.m_pBuf != null)
								{
									if (0 != mallocObj_tmpPkts.mems[i].attach(ref pPkt->memory))
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





							//  
							dwTgt_lastData = qyFuncs.M_GetTgt_video(tickObj);       //  video data received

							//
							for (int iIndex = 0; iIndex < nPkts; iIndex++)
							{
								myDRAW_VIDEO_DATA* pPkt = &pPkts[iIndex];
								int pktLen = sizeof(myDRAW_VIDEO_DATA);
#if DEBUG
								ref myDRAW_VIDEO_DATA tmp_pPkt = ref *pPkt;
#endif

								//
								if (bDbg)
								{
									int nnn = qyFuncs.getQ2Nodes(pPlayer.video.displayQ2);
									str = string.Format("mcThreadProc_playVideo,pktNo {0}, nNodes {1}\n", pPkt->debugInfo.lPktNo, nnn);
									qyFuncs.showInfo_open(0, null, str);
								}


								//  2015/03/12. 对本地视频，伪造一个pts. 等回头在包生成处产生pts才对
								if (0 == pPlayer.idInfo_recorder.ui64Id)
								{
									if (0 == pPkt->head.uiPts)
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
									|| 0 == pPkt->bih.biSizeImage)
								{
#if DEBUG
									str = string.Format("playVideo err: pkt {0}x{1} !=  player {2}x{3}. skipped", (int)pPkt->bih.biWidth, (int)pPkt->bih.biHeight, (int)pPlay.vh.bih.biWidth, (int)pPlay.vh.bih.biHeight);
									qyFuncs.traceLog(str);
#endif
									//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playVideo err: pkt %d,%d !=  player %d,%d. skipped"), (int)pPkt->bih.biWidth, (int)pPkt->bih.biHeight, (int)pPlay.vh.bih.biWidth, (int)pPlay.vh.bih.biHeight);
									continue;
								}

								//
								if (pPkt->usPktResType != Consts.CONST_pktResType_sharedTex)
								{
									if (pPkt->bih.biCompression != Consts.BI_RGB)
									{
										/*
										if (pPkt->bih.biSizeImage > bih.biSizeImage)
										{
#if __DEBUG__
			assert(0);
#endif
											continue;
										}
										*/
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
												/*
												qyFuncs.mymemcpy((IntPtr)tmpRgbBuf, pPkt->memory.m_pBuf, (uint)pPkt->bih.biSizeImage);

												pQyMc.yv12ToRgb24((byte*)tmpRgbBuf, (byte*)pPkt->memory.m_pBuf, pPkt->bih.biWidth, pPkt->bih.biHeight, false);//pPlayer.video.ucbFlippedImg  );								 
												*/
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


												GuiShare.pf_i420ToRgb((IntPtr)pPkt->memory.m_pBuf, ttl.m_yuvRgbInfo, 32, true, pPkt->bih.biWidth, pPkt->bih.biHeight, false);

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
												ttl.tmpFillTex(pPkt->bih.biWidth, pPkt->bih.biHeight);

												//
												if (bDbg)
												{
													qyFuncs.showInfo_open(0, null, "ttl_readPkt");
												}


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
									else
									{
									}
								}

								//  2015/09/11
								pPkt->debugInfo.dwTickCnt_playPkt_start = qyFuncs.myGetTickCount(null);








								continue;
							}

						}
						continue;
					}
				}
			}
			//
			str = string.Format("ttl_readPkt will quit,pPlay.bQuit {0}. {1}", pPlay.bQuit, ttl.m_tag);
			qyFuncs.showInfo_open(0, null, str);

		errLabel:


			if (!ttl.m_bMe)
			{
				int ii = 0;
			}
			fixed (char* pName = pPlayer.video.q2.cfg.name)
			{
				str = string.Format("{0}, {1} ends.", new string(pName), pPlayer.idInfo_recorder.ui64Id);
				qyFuncs.showInfo_open(0, null, str);
			}


			//traceLogA("playVideo %S leaves", pPlay.q2.cfg.name);

			return 0;

		}

	}
}