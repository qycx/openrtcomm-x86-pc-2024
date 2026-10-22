using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int myWaitForQThread_infinite(QY_qThreadProcInfo_common pQThread, string hint)
		{
			if (null==hint) hint = ("");

			int nTries = 30;    //  这里必须停下来								  				  
			int i;

			for (i = 0; i < nTries; i++)
			{
				if (!bQThreadExists(pQThread)) break;

				pQThread.bQuit = true;
				//
				if (pQThread.q2.hEvent0!=null) myEventSet(pQThread.q2.hEvent0);
				//
				//						
				waitForQThread((QY_qThreadProcInfo_common)pQThread, 1000);

			}

			if (i == nTries)
			{   //  2013/08/02
				/*
				TCHAR tBuf[128];
				_sntprintf(tBuf, mycountof(tBuf), _T("%s: waitFor failed"), hint);
				qyExitProcess(tBuf);
				*/
			}

			return 0;
		}

		public static unsafe bool bPlayAudio_needEnd(QY_PLAYER pPlayer, PLAY_AUDIO_procInfo pPlayAudio)
		{
			bool bRet = false;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			//TCHAR tBuf[128];

			//
			uint dwTickCnt = myGetTickCount(null);

			//
			if (pQyMc.bQuit)
			{
				return true;
			}


			//
			switch (pPlayAudio.uiPlayType)
			{
				case 0:
					break;
				case Consts.CONST_aPlayType_webRtc:
					{
						int nDiff = 0;
						//  2018/06/16
						if (pPlayAudio.bNeedStop_webRtc_my_inStream)
						{
							bRet = true;
							//
							//_sntprintf(tBuf, mycountof(tBuf), _T("bPlayAudio_needEnd: %I64u, bNeedStop is true"), pPlayer->idInfo_recorder.ui64Id);
							//showInfo_open(0, 0, tBuf);
						}
						//
						nDiff = (int)(dwTickCnt - pPlayAudio.dwLastTickCnt_startPlayer);
						nDiff = Math.Abs(nDiff);
						if (nDiff < 500) break;
						//
						nDiff = (int)(dwTickCnt - pPlayAudio.cache.dwTickCnt_lastAudioData);
						nDiff = Math.Abs(nDiff);
						if (nDiff > (uint)pProcInfo.cfg.usTimeoutInMs_player_a)
						{
							pPlayAudio.nTimes_noAudioData_webRtc++;
						}
						else
						{
							pPlayAudio.nTimes_noAudioData_webRtc = 0;
						}
						//
# if  __DEBUG__
						if (pPlayer->idInfo_recorder.ui64Id == 110)
						{
							int ii = 1;
							//
							_sntprintf(tBuf, mycountof(tBuf), _T("lastAudioData %d. nDiff %d\n"), pPlayAudio.cache.dwTickCnt_lastAudioData, nDiff);
							OutputDebugString(tBuf);
						}
#endif

						//
						if (pPlayAudio.nTimes_noAudioData_webRtc > 3)
						{
							bRet = true; break;
						}
					}
					break;
				default:
					//  pPlayAudio.dwLastTickCnt_startPlayer要在启动时赋值
					if (!pPlayAudio.bRunning)
					{
						if (dwTickCnt - pPlayAudio.dwLastTickCnt_startPlayer > 500)  //  线程没启动, 或已停止					
						{
							bRet = true;
							break;
						}
					}
					break;

			}

			//
			return bRet;
		}


		//
		public static unsafe bool bPlayVideo_needEnd(QY_PLAYER pPlayer, PLAY_VIDEO_procInfo pPlayVideo)
		{
			bool bRet = false;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			//TCHAR tBuf[128];

			//
			uint dwTickCnt = myGetTickCount(null);

            if (pQyMc.bQuit)
            {
				return true;
            }


			//
			switch (pPlayVideo.uiPlayType)
			{
				case 0:
					break;
				case Consts.CONST_vPlayType_v:					
				default:
					//  pPlayAudio.dwLastTickCnt_startPlayer要在启动时赋值
					if (!pPlayVideo.bRunning)
					{
						if (dwTickCnt - pPlayVideo.dwLastTickCnt_startPlayer > 500)  //  线程没启动, 或已停止					
						{
							bRet = true;
							break;
						}
					}
					break;

			}

			//
			return bRet;
		}



		public static unsafe int chkPlayers(CCtxQmc pProcInfo)
		{
			int iErr = -1;
			if (null==pProcInfo) return -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			int i;


			//
			string str;
			str=("chkPlayers enters.");
			showInfo_open(0, "", str);

			//
			if (null==pProcInfo.av.pPlayers)
			{
				return 0;
			}

			//
			uint dwTickCnt = myGetTickCount(null);

			//
			QY_PLAYER pPlayer;
			PLAY_AUDIO_procInfo pPlayAudio;
			PLAY_VIDEO_procInfo pPlayVideo;

			//  comment: player是一个数组,在pProcInfo.av.pPlayers[]. pProcInfo是一个全局的结构
			//
			for (i = 0; i < pProcInfo.av.usCnt_players; i++)
			{
				pPlayer = pProcInfo.av.pPlayers[i];
				pPlayAudio = pProcInfo.av.pPlayers[i].audio;
				pPlayVideo = pProcInfo.av.pPlayers[i].video;
				//
				//  comment: 这段是判断哪个player是退出了。如果退出就回收player		 
				//
				dwTickCnt = myGetTickCount(null);

				//
# if  __DEBUG__
				if (pPlayer.idInfo_recorder.ui64Id == 110)
				{
					int ii = 1;
					if (bPlayAudio_needEnd(pPlayer, pPlayAudio))
					{
					}
				}
#endif

				//  2017/06/13
				//if  (  pPlayer.idInfo_recorder.ui64Id  )  
				{
					if (0!=pPlayAudio.uiPlayType)
					{
						if (bPlayAudio_needEnd(pPlayer, pPlayAudio))
						{

							//			
							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("chkPlayers: clean playAudio %I64u, %d,%d"), pPlayer.idInfo_recorder.ui64Id, pPlayAudio.uiTranNo, pPlayer.video.uiTranNo);

							//  回收
							pPlayAudio.bQuit = true;       //  注意：bQuit不能在mcThread_playX里赋值，只能在主线程里赋值
							switch (pPlayAudio.uiPlayType)
							{
								case Consts.CONST_aPlayType_webRtc:
									stopPlayAudio_webRtc(pPlayer);
									break;
								case Consts.CONST_aPlayType_dSound:
									//waitForQThread(  (  QY_qThreadProcInfo_common  *  )pPlayAudio,  INFINITE  );
									myWaitForQThread_infinite((QY_qThreadProcInfo_common)pPlayAudio, ("doApplyForPlayer"));
									break;
								case Consts.CONST_aPlayType_android:
									GuiShare.pf_stopPlayAudio_android(pPlayer);
									break;
								default:
									//MACRO_qyAssert(0, _T("ERROR: unprocessed aplayType"));
									break;
							}
							//
							pPlayAudio.uiPlayType = 0;
						}

					}
					if (0!=pPlayVideo.uiPlayType)
					{
						if (bPlayVideo_needEnd(pPlayer,pPlayVideo))
						{
								//			
								//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("chkPlayers: clean playVideo %I64u, %d, %d"), pPlayer.idInfo_recorder.ui64Id, pPlayer.audio.uiTranNo, pPlayVideo.uiTranNo);

								//  回收
								pPlayVideo.bQuit = true;       //  注意：bQuit不能在mcThread_playX里赋值，只能在主线程里赋值
								switch (pPlayVideo.uiPlayType)
								{
									case Consts.CONST_vPlayType_android:
									//myWaitForQThread_infinite((QY_qThreadProcInfo_common)pPlayVideo, ("doApplyForPlayer"));
									GuiShare.pf_stopPlayVideo_android(pPlayer);
										break;
									default:
										//MACRO_qyAssert(0, _T("ERROR: unprocessed vplayType"));
										break;
								}
								//
								pPlayVideo.uiPlayType = 0;
						}
					}
					//
					if (0==pPlayAudio.uiPlayType && 0==pPlayVideo.uiPlayType)
					{
						pPlayer.idInfo_recorder.ui64Id = 0;                                        //  reset
						pPlayAudio.uiTranNo = 0;
						pPlayVideo.uiTranNo = 0;
						pPlayer.iTaskId = 0;                                                       //  2009/09/17

					}
				}

				//
				continue;

			}

			str = ("chkPlayers leaves.");
			showInfo_open(0, "", str);

			return 0;
		}


		public static unsafe int doApplyForPlayer(object hMainWnd, MIS_MSGU* pMsg)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo; pProcInfo = pQyMc.get_pProcInfo();
			if (null==pProcInfo) goto errLabel;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			int i;
			uint dwThreadDaemonId;
			uint dwTickCnt;

			bool bDbg = false;
			string str;
			//TCHAR tHintBuf[256] = _T("");

#if DEBUG
			//traceLogA("CMainFrame: get applyForPlayer_qmc msg: %I64u", pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id);
			ref MIS_MSGU tmp_pMsg = ref *pMsg;
			//
			if (0==pMsg->applyForPlayer_qmc.video.vh.bih.biWidth)
			{
				int ii;
				ii = 0;
			}
#endif

			// comment: 消息applyForPlayer_qmc里包含idInfo_recorder. audio.uiTranNo,video.uiTranNo来唯一代表一个player.
			if (0==pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id && 0==pMsg->applyForPlayer_qmc.audio.uiTranNo && 0==pMsg->applyForPlayer_qmc.video.uiTranNo)
			{
				//safeTcsnCpy(_T("idInfo and uiTranNo are 0"), tHintBuf, mycountof(tHintBuf));
				goto errLabel;
			}

			//  2012/05/07
			// comment: 这段gps处理暂时没用
			//////////////////
			///
			/*
			if (pMsg->applyForPlayer_qmc.bGps)
			{
				//
				QY_PLAYER_gps* pPlayer_gps;
				for (i = 0; i < pProcInfo.gps.usCnt_players; i++)
				{
					pPlayer_gps = &pProcInfo.gps.pPlayers[i];
					if (pPlayer_gps->idInfo.ui64Id)
					{
						if (GetTickCount() - pPlayer_gps->dwTickCnt_lastRecvd > 30 * 1000)
						{
							pPlayer_gps->idInfo.ui64Id = 0;
							pPlayer_gps->iTaskId = 0;
						}
					}
					if (pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id == pPlayer_gps->idInfo.ui64Id
						&& pMsg->applyForPlayer_qmc.iTaskId == pPlayer_gps->iTaskId)
					{
						break;
					}
				}
				if (i == pProcInfo.gps.usCnt_players)
				{
					//  try to find an unused player
					for (i = 0; i < pProcInfo.gps.usCnt_players; i++)
					{
						pPlayer_gps = &pProcInfo.gps.pPlayers[i];
						if (!pPlayer_gps->idInfo.ui64Id
							&& !pPlayer_gps->iTaskId)
						{
							break;
						}
					}
					if (i == pProcInfo.gps.usCnt_players)
					{
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("too many gps players"));
						goto errLabel;
					}
					pPlayer_gps->idInfo.ui64Id = pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id;
					pPlayer_gps->iTaskId = pMsg->applyForPlayer_qmc.iTaskId;
					pPlayer_gps->dwTickCnt_lastRecvd = GetTickCount();
				}

				//
				iErr = 0; goto errLabel;
			}
			*/
			////////////////////

			//
			QY_PLAYER pPlayer=null;
			PLAY_AUDIO_procInfo pPlayAudio=null;
			PLAY_VIDEO_procInfo pPlayVideo=null;

			//
			chkPlayers(pProcInfo);

			//  comment: player是一个数组,在pProcInfo.av.pPlayers[]. pProcInfo是一个全局的结构
			//
			for (i = 0; i < pProcInfo.av.usCnt_players; i++)
			{
				pPlayer = pProcInfo.av.pPlayers[i];
				pPlayAudio =pProcInfo.av.pPlayers[i].audio;
				pPlayVideo = pProcInfo.av.pPlayers[i].video;
				//
				//  comment: 这段是判断哪个player是退出了。如果退出就回收player		 
				//
				dwTickCnt = myGetTickCount(null);


				//
				if (pMsg->applyForPlayer_qmc.iTaskId == pPlayer.iTaskId
					&& pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id == pPlayer.idInfo_recorder.ui64Id
					&& pMsg->applyForPlayer_qmc.audio.uiTranNo == pPlayAudio.uiTranNo
					&& pMsg->applyForPlayer_qmc.video.uiTranNo == pPlayVideo.uiTranNo)
				{
					break;
				}
			}
			if (i == pProcInfo.av.usCnt_players)
			{

				//  try to find an unused player
				for (i = 0; i < pProcInfo.av.usCnt_players; i++)
				{
					pPlayer = pProcInfo.av.pPlayers[i];
					pPlayAudio = pProcInfo.av.pPlayers[i].audio;
					pPlayVideo = pProcInfo.av.pPlayers[i].video;
					if (0==pPlayer.idInfo_recorder.ui64Id
						&& 0==pPlayAudio.uiTranNo
						&& 0==pPlayVideo.uiTranNo)
					{
						break;
					}
				}
				if (i == pProcInfo.av.usCnt_players)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("too many players"));
					goto errLabel;
				}

				pPlayer.idInfo_recorder.ui64Id = pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id;
				//
				pPlayAudio.uiTranNo = pMsg->applyForPlayer_qmc.audio.uiTranNo;
				//memcpy(&pPlayAudio.ah, &pMsg->applyForPlayer_qmc.audio.ah, sizeof(pPlayAudio.ah)  );
				pPlayAudio.ah = pMsg->applyForPlayer_qmc.audio.ah;
				//  2009/06/14
				//memcpy(&pPlayAudio.playCfg, &pMsg->applyForPlayer_qmc.audio.playCfg, sizeof(pPlayAudio.playCfg)  );
				pPlayAudio.playCfg=pMsg->applyForPlayer_qmc.audio.playCfg;

				//
				pPlayVideo.uiTranNo = pMsg->applyForPlayer_qmc.video.uiTranNo;
				//memcpy(&pPlayVideo.vh, &pMsg->applyForPlayer_qmc.video.vh, sizeof(pPlayVideo.vh)  );
				pPlayVideo.vh=pMsg->applyForPlayer_qmc.video.vh;
				//
				pPlayVideo.usFps = pMsg->applyForPlayer_qmc.video.usFps;   //  2015/0220
																			//
				pPlayer.iTaskId = pMsg->applyForPlayer_qmc.iTaskId;            //  2009/09/17

				//  2016/02/18
				fixed (int* pi = &pQyMc.globals.lTranNo_player)
				{
					pPlayer.uiTranNo_player = getuiNextTranNo(null, 0, ref *pi);
				}

				PLAYER_ID playerId;
				playerId.index_player = i;
				playerId.uiTranNo_player = pPlayer.uiTranNo_player;
				//
				if (pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id != 0) {
					object hTalk = null;
                    //
                    if (pMsg->applyForPlayer_qmc.idInfo_logicalPeer.ui64Id == 0)
                    {
						str = string.Format("doApplyForPlayer failed, idInfo_logicalPeer is 0");
						traceLog(str);
						goto errLabel;
                    }
					//
					if (0 != GuiShare.pf_findTalker(pQyMc, pMisCnt, pMsg->applyForPlayer_qmc.idInfo_logicalPeer, ref hTalk))
					{
						str = string.Format("doApplyForPlayer failed, findTalker {0} failed", pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id);
						traceLog(str);
						//
						goto errLabel;
					}
					GuiShare.pf_talk_updatePlayerInfo(hTalk, (IntPtr)pMsg, playerId);

				//pMsg->applyForPlayer_qmc.ii
				}
				else
                {
					QY_SHARED_OBJ sharedObj = getSharedObjByIndex(pProcInfo, pMsg->applyForPlayer_qmc.index_sharedObj);
					if (sharedObj == null) goto errLabel;
					SHARED_OBJ_USR sou = sharedObj.usrs[0];
					if ( sou.iWndContentType==Consts.CONST_qyWndContentType_talker )
                    {
						object hTalk = sou.hWnd_starter;
						if (hTalk == null) goto errLabel;
						GuiShare.pf_talk_updatePlayerInfo(hTalk, (IntPtr)pMsg, playerId);
                    }
                }
			}
			//  2009/02/09

			//  2014/1210
			dwTickCnt = myGetTickCount(null);
			if (dwTickCnt - pPlayer.dwLastTickCnt_doApplyForPlayer < 200)
			{

				//
				//safeTcsnCpy(_T(" too quickly, skipped"), tHintBuf, mycountof(tHintBuf));

				//
# if  __DEBUG__
				traceLog(_T("doApplyForPlay failed, too quickly"));
#endif
				goto errLabel;
			}
			pPlayer.dwLastTickCnt_doApplyForPlayer = dwTickCnt;

			//

			if (0!=pPlayAudio.uiTranNo
				&& 0==pPlayAudio.uiPlayType)
			{

				//  2013/05/24
				//pPlayAudio.playbackDevDesc[0] = 0;

				//  2017/02/16
				uint uiPlayType = pQyMc.uiAPlayType;

				//
				if (pPlayAudio.ah.wfx.nSamplesPerSec == 16000
					&& pPlayAudio.ah.wfx.nChannels == 1
					&& pPlayAudio.ah.wfx.wBitsPerSample == 16)
				{
					uiPlayType = Consts.CONST_aPlayType_webRtc;
				}

				//  2017/09/24
				/*
				if (bLikeXp())
				{
					uiPlayType = CONST_aPlayType_dSound;
					//
					//showInfo_open(0, _T("doApplyForPlayer"), _T("use dSound for XP to playAudio"));
				}
				*/
				//
				uiPlayType = Consts.CONST_aPlayType_android;

				//
				pPlayAudio.uiPlayType = uiPlayType;
				pPlayAudio.dwLastTickCnt_startPlayer = dwTickCnt;
				pPlayAudio.bQuit = false;
				pPlayAudio.bRunning = false;

				//
				if (uiPlayType == Consts.CONST_aPlayType_webRtc)
				{
					//
					//showInfo_open(0, _T("doApplyForPlayer"), _T("before startPlayAudio_webRtc"));
					//
					startPlayAudio_webRtc(pPlayer);
					//
					//showInfo_open(0, _T("doApplyForPlayer"), _T("after startPlayAudio_webRtc"));
					//
				}
				else if (uiPlayType == Consts.CONST_aPlayType_dSound)  //  2017/02/16
				{
					/*
					exitDSoundToPlayIfNoPlayer(pProcInfo);

					//
					LPGUID lpGuid = NULL;

					//  还要检查是否是会议里的player. 对于不是会议的播放,采用系统缺省设备.
					OLD_audioDevice audioDev;
					if (bTaskId_conference(pPlayer.iTaskId))
					{
						if (!getDefaultPlaybackDevice(&audioDev))
						{
							lpGuid = &audioDev.guid_audio;
							safeTcsnCpy(audioDev.names_audio, pPlayAudio.playbackDevDesc, mycountof(pPlayAudio.playbackDevDesc));
						}
					}

					//
					if (initDSoundToPlay(((QY_DYN_LIBS*)pQyMc->env.pDynLibs)->pLib_dx, lpGuid, hMainWnd, 0))
					{
# if  __DEBUG__
						traceLogA("CMainFrame::OnQyPostComm: initDSoundToPlay failed.");
#endif
						//  goto  errLabel;
					}
					else
					{

						//
# if  __DEBUG__
						traceLogA("CMainFrame: start audio player: %I64u,%d", pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id, pMsg->applyForPlayer_qmc.audio.uiTranNo);
#endif
						if (startQThread(mcThreadProc_playAudio, pProcInfo, NULL, (QY_qThreadProcInfo_common*)pPlayAudio)) goto errLabel;
					}
					*/
				}
				else if (uiPlayType == Consts.CONST_aPlayType_android)					
				{
					GuiShare.pf_startPlayAudio_android(pPlayer);
				}
				else
				{   //  use wave api. 2010/05/03

					/*

					//  if  (  startQThread(  NULL,  0,  new  CQyPlaySound,  (  QY_qThreadProcInfo_common  * )pPlayAudio  )  )  goto  errLabel;

					void* pThreadClass = new CQyPlaySound();
					if (startQThread(NULL, 0, pThreadClass, (QY_qThreadProcInfo_common*)pPlayAudio)) goto errLabel;
					*/
				}
			}
			//
			if (0!=pPlayVideo.uiTranNo
				&& 0==pPlayVideo.uiPlayType)
			{
				uint uiPlayType = Consts.CONST_vPlayType_android;

				//
				pPlayVideo.uiPlayType = uiPlayType;
				pPlayVideo.dwLastTickCnt_startPlayer = dwTickCnt;
				pPlayVideo.bQuit = false;
				pPlayVideo.bRunning = false;


				//
				//traceLogA("CMainFrame: start video player: %I64u,%d", pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id, pMsg->applyForPlayer_qmc.video.uiTranNo);
				//
				/*
				pPlayVideo.hThread = CreateThread(NULL, 0, mcThreadProc_playVideo, pProcInfo, CREATE_SUSPENDED, &dwThreadDaemonId);
				if (!pPlayVideo.hThread)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("applyForPlayer failed. CreateThread video failed"));
					goto errLabel;
				}
				pPlayVideo.dwThreadId = dwThreadDaemonId;
				if (ResumeThread(pPlayVideo.hThread) == -1) goto errLabel;
				*/
				GuiShare.pf_startPlayVideo_android(pPlayer);

			}


			iErr = 0;

		errLabel:

			//  if  (  iErr  )  
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("applyForPlayer %I64u,%d,%d leave, %s. %s"), pMsg->applyForPlayer_qmc.idInfo_recorder.ui64Id, pMsg->applyForPlayer_qmc.audio.uiTranNo, pMsg->applyForPlayer_qmc.video.uiTranNo, iErr ? _T("failed") : _T("OK"), tHintBuf);
			}

			//
			int nThreads_player = 0;
			for (i = 0; i < pProcInfo.av.usCnt_players; i++)
			{
				pPlayer = pProcInfo.av.pPlayers[i];
				if (0!=pPlayer.audio.uiPlayType) nThreads_player++;
				if (0!=pPlayer.video.uiPlayType) nThreads_player++;
			}
			if (bQThreadExists(pProcInfo.av.localAv.player.audio)) nThreads_player++;
			if (bQThreadExists(pProcInfo.av.localAv.player.video)) nThreads_player++;
			if (pProcInfo.status.nThreads_player != nThreads_player) pProcInfo.status.nThreads_player = nThreads_player;


			return iErr;

		}

	}
}
