using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace qy
{
	public delegate void PF_threadProc(object obj);





	partial class qyFuncs
    {
		//
		public static QY_DMITEM[] CONST_sharedObjTypeTable = new QY_DMITEM[5]
	{
	 new QY_DMITEM(  Consts.CONST_sharedObjType_av,             (  "av"  )   ),
	 new QY_DMITEM(  Consts.CONST_sharedObjType_mediaFile,      (  "mediaFile"  )    ),
	 new QY_DMITEM(  Consts.CONST_sharedObjType_mediaDevice,    (  "mediaDevice"  )  ),
	 new QY_DMITEM(  Consts.CONST_sharedObjType_webcam_vOnly,   (  "webcam_vOnly"  )     ),
	 new QY_DMITEM(  -1),
	 };

		public static unsafe int setPktInputInfo(CCtxQmc pProcInfoCommon, ref VIDEO_input_info pInput, ushort usFrames_perSecond_expected, bool bNoSkip, void* pmyDRAW_VIDEO_DATA)
		{
			int iErr = -1;
			string str;

			//
			if (null==pProcInfoCommon)
			{
				//MACRO_qyAssert(0, _T("setPktInputInfo failed, pProcInfo is null"));
				return -1;
			}

			//
			Int64 tStartTran=0; mytime(ref tStartTran);
			//TCHAR tBuf[128] = _T("");

			//
			//VIDEO_input_info  *  pInput  =  &pVc->compressVideo.input;
			//unsigned  short  usFrames_perSecond_expected  =  pVc->compressVideo.usFrames_perSecond_expected;

			//
			if (pInput.tStartTran != tStartTran)
			{
				pInput.usFps = pInput.usFramesThisSecond;
				pInput.usFramesThisSecond = 1;
				pInput.tStartTran = tStartTran;
			}
			else
			{
				pInput.usFramesThisSecond++;
			}

			//
			int index = pInput.usFramesThisSecond - 1;

			//  2011/08/31
			if (!bNoSkip)
			{
				//  2009/10/29
				//  int	index  =  pCompressVideo->usFramesThisSecond_input  -  nPkts  +  ii;
				//COMPRESS_VIDEO	*	pCompressVideo  =  &pVc->compressVideo;


				int tmpusFps_input = pInput.usFps; //  _input;
				int tmpuiFps_expected = usFrames_perSecond_expected;
				if (0==tmpuiFps_expected) tmpuiFps_expected = 1;

				if (tmpusFps_input > tmpuiFps_expected)
				{

					bool bSkipped = false;
					int k = 0;
					//
					if (tmpuiFps_expected > 2) k = (tmpusFps_input + 2) / tmpuiFps_expected;
					else k = tmpusFps_input / tmpuiFps_expected;

# if  __DEBUG__
					//  traceLog(  _T(  "k %d, fps_input %d"  ),  k,  tmpusFps_input  );
#endif

					if (k > 1)
					{   //  如果要发送的帧数是小部分，则选择数要发送的帧
						if (0!=(index % k))
						{
							bSkipped = true;
						}
						else
						{   //  2012/03/16
							int fps_real = 0;
							//
							if (k > 2) fps_real = (tmpusFps_input + 2) / k;
							else fps_real = tmpusFps_input / k;

							int index_real = index / k;
							if (fps_real > tmpuiFps_expected)
							{
								int nThrows = fps_real - tmpuiFps_expected;
								k = fps_real / nThrows;
								if (fps_real > nThrows * k) k++;
								if (0==(index_real % k))
								{
									bSkipped = true;
								}
							}
						}
					}
					else
					{   //  如果要发送的帧数是大多数，则选择数丢弃帧
						int nThrows = tmpusFps_input - tmpuiFps_expected;
						k = tmpusFps_input / nThrows;
						if (tmpusFps_input > nThrows * k) k++;
						if (0==(index % k))
						{
							bSkipped = true;
						}
					}

					//
					if (bSkipped)
					{


						goto errLabel;
					}
				}
			}

			//
			//
			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pmyDRAW_VIDEO_DATA;
			if (null!=pPkt)
			{
				pPkt->input.tStartTran = tStartTran;                                                    //  2011/08/30
																										//  pPkt->input.usIndexThisSecond  =  pInput.usFramesThisSecond  -  1;
			}


			iErr = 0;
		errLabel:

			//
			bool bNoShow = false;
# if  __DEBUG__
			if (_tcsstr(pInput.tBuf, _T("BuffCB_av")))
			{
				bNoShow = true;
			}
#endif

			//
			ref QMC_cfg_debugStatusInfo pCfg_debugStatusInfo = ref pProcInfoCommon.cfg.debugStatusInfo;

			//
			if (0!=iErr)
			{
# if  __DEBUG__
				//  traceLog(  _T(  "setPktInputInfo:  pkt[%d] skipped"  ),  index  );
#endif
				if ( pCfg_debugStatusInfo.ucbShowPktInputInfo && !bNoShow)
				{
					//_sntprintf(tBuf, mycountof(tBuf), _T("setPktInputInfo:  pkt[%d] skipped"), index);
					str=string.Format("setPktInputInfo:  pkt[{0}] skipped", index);
					//
					//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s, usFramesThisSecond %d"  ),  tBuf,  (  int  )pInput.usFramesThisSecond  );
					str+=string.Format(  " usFramesThisSecond {0}",  (  int  )pInput.usFramesThisSecond  );
					//
					//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s, %s"  ),  tBuf,  (  int  )pInput.tBuf  );

					//
					uint step = 0;
					qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), ("IsClient"), null, _T(""), _T(""), str);
				}
			}
# if  DEBUG
			if (0==iErr)
			{
				if (pCfg_debugStatusInfo.ucbShowPktInputInfo && !bNoShow)
				{
					//_sntprintf(tBuf, mycountof(tBuf), _T("setPktInputInfo:  pkt[%d] ok"), index);
					str=string.Format("setPktInputInfo:  pkt[{0}] ok", index);
					//
					//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s, usFramesThisSecond %d"  ),  tBuf,  (  int  )pInput.usFramesThisSecond  );
					str+=string.Format(  " usFramesThisSecond {0}", (  int  )pInput.usFramesThisSecond  );

					//
					//_sntprintf(tBuf, mycountof(tBuf), _T("%s, %s"), tBuf, pInput.tBuf);

					//
					uint step = 0;
					qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), _T("IsClient"), null, _T(""), _T(""), str);
				}
			}
#endif

			return iErr;
		}

		//
		public static unsafe int stopQThread(QY_qThreadProcInfo_common pThread, string hint)
		{
			//TCHAR tBuf[256];

			if (null==hint) hint = ("");

			if (pThread.hThread!=null)
			{
				int nTries = 200;
				int i;
				for (i = 0; i < nTries; i++)
				{
					if (null==pThread.hThread) break;

					//
					pThread.bQuit = true;
					myEventSet(pThread.q2.hEvent0);
					//waitForThread(ref pThread.hThread, 100);
					GuiShare.pf_waitForThread1(ref pThread.hThread, 100);
				}
				//
				//_sntprintf(tBuf, mycountof(tBuf), _T("stopQ2Thread: %s, %s, wait for thread %d, nStep %d, elapse %dms. "), hint, pThread.q2.cfg.name, pThread.dwThreadId, pThread.debugStep.nStep_debug, GetTickCount() - pThread.debugStep.dwTickCnt_step_debug);
				//  _sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s  errs: notAccept %d, Input %d, output %d, index %d."  ),  tBuf,  p->video.debugStep.uiCnt_DMO_E_NOTACCEPTING,  p->video.debugStep.nErrs_ProcessInput,  p->video.debugStep.nErrs_ProcessOutput,  p->video.debugStep.nErrs_pOutputBuffers_index  ); 

# if  __DEBUG__
				traceLogA("%S", tBuf);
#endif
				//
				if (null!=pThread.hThread)
				{
					//qyExitProcess(tBuf);
					int ii = 0;
				}
			}

			return 0;
		}


		//
		public static int startQThread(PF_threadProc lpStartAddress, object lpParameter, object pThreadClass, QY_qThreadProcInfo_common pQThread)
		{
			int iErr = -1;
			uint dwThreadDaemonId = 0;

			//
			if (null!=lpStartAddress)
			{
				/*
				pQThread.hThread = CreateThread(NULL, 0, lpStartAddress, lpParameter, CREATE_SUSPENDED, &dwThreadDaemonId);
				if (!pQThread.hThread)
				{
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("startQThread failed. CreateThread failed"));
					goto errLabel;
				}
				pQThread.dwThreadId = dwThreadDaemonId;
				if (ResumeThread(pQThread.hThread) == -1) goto errLabel;
				*/
#if false
				pQThread.hThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(lpStartAddress));
				pQThread.hThread.Start(lpParameter);
#endif
				GuiShare.pf_startThread1(ref pQThread.hThread, lpStartAddress, lpParameter);

			}
			else
			{
				/*
				CQyThread* pThread = (CQyThread*)pThreadClass;
				if (!pThread) goto errLabel;
				pThread->CreateThread();

				pQThread.hThread = pThread->m_hThread;
				//  qThread.hEvent_winThreadEnd  =  CreateEvent(  NULL,  FALSE,  FALSE,  NULL  );			  
				pQThread.dwThreadId = pThread->m_nThreadID;

				pThread->ResumeThread();
				*/
			}

			iErr = 0;

		errLabel:

			return iErr;
		}



	}
}
