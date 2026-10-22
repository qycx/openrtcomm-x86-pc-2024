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
		public static unsafe int startLocalAudioRecorder(uint uiRecordType, object pGuid, string capDevDesc, int iIndex_sharedObj, ref AUDIO_COMPRESSOR_CFG pCompressor)
        {
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null==pProcInfo) return -1;
			uint dwThreadDaemonId = 0;
			bool bAudioCompressInited = false;

			bool bDbg = true;
			string str;

			//
#if DEBUG
			qyFuncs.traceLogA(("startLocalAudioRecorder enters.\n"));

			//
			if (pProcInfo.cfg.mSpecialDbg.bNo_audioRecord)
			{
				qyFuncs.showInfo_open(0, null, "for test, startLocalAudioRecorder rturns -1");
				return -1;
			}

#endif


			/*
			if (!pProcInfo.ucb_doInitRecordSound)
			{
				MODULE_camCap* pCamCap = (MODULE_camCap*)getCusModule(&pQyMc.cusModules, CONST_moduleType_camCap);
				if (pCamCap
					&& pCamCap->pf_doInitRecordSound
					&& pCamCap->pf_doExitRecordSound)
				{
					pCamCap->pf_doInitRecordSound();
				}
				pProcInfo.ucb_doInitRecordSound = true;
			}
			*/

			//  2013/08/02
			QY_qThreadProcInfo_common pRecThread = pProcInfo.av.localAv.recordSoundProcInfo.recThread;

			//

			//
			QY_SHARED_OBJ pSharedObj;
			pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) goto errLabel;
			pSharedObj.var.bSoundRecorder = true;


			//  2013/03/18
			if (pProcInfo.av.localAv.recordSoundProcInfo.bCapDevConnected) goto errLabel;

			//
			pProcInfo.av.localAv.recordSoundProcInfo.uiType = Consts.CONST_capType_av;
			pProcInfo.av.localAv.recordSoundProcInfo.iIndex_sharedObj = iIndex_sharedObj;
			pProcInfo.av.localAv.recordSoundProcInfo.uiRecordType = uiRecordType;
			//
			/*
			pProcInfo.av.localAv.recordSoundProcInfo.capDevDesc[0] = 0;
			pProcInfo.av.localAv.recordSoundProcInfo.aec_rendDevDesc[0] = 0;
			safeTcsnCpy(capDevDesc, pProcInfo.av.localAv.recordSoundProcInfo.capDevDesc, mycountof(pProcInfo.av.localAv.recordSoundProcInfo.capDevDesc));
			*/

			//  2009/05/24
			//  if  (  !pCompressor  )  pCompressor  =  &pProcInfo.cfg.policy.taskAv_pc.avCompressor.audio;

			WAVEFORMATEX* pWf_input = null;
			WAVEFORMATEX wf_input;
			qyFuncs.mymemset((IntPtr)(byte*)&wf_input, 0, sizeof(WAVEFORMATEX));

			//  2013/03/18
			switch (uiRecordType)
			{
				case Consts.CONST_recordType_directX:
					/*
					TCHAR* pDisplayName;
					pDisplayName = (TCHAR*)pGuid;
					if (initAudioCapDev_dx(null, pDisplayName, null, pCompressor, false, pQyMc.gui.hMainWnd, iIndex_sharedObj, &pProcInfo.av.localAv.recordSoundProcInfo.u.dx.pAudioCapDev)) goto errLabel;
					pProcInfo.av.localAv.recordSoundProcInfo.bCapDevConnected = true;

					if (!bGetCapturePara_audioCapdev((MC_VAR_common*)pProcInfo, pSharedObj.iIndex_capAudio, sizeof(MIS_MSGU), pProcInfo.av.localAv.recordSoundProcInfo.u.dx.pAudioCapDev, &pProcInfo.av.localAv.recordSoundProcInfo.wf_org, null)) goto errLabel;
					pWf_input = &pProcInfo.av.localAv.recordSoundProcInfo.wf_org;
					*/
					break;
				case Consts.CONST_recordType_android:
					if (0!=initAudioCapDev_android(null, "", null, ref pCompressor, false, pQyMc.gui.hMainWnd, iIndex_sharedObj, ref pProcInfo.av.localAv.recordSoundProcInfo.u_android.pAudioCapDev)) goto errLabel;
					pProcInfo.av.localAv.recordSoundProcInfo.bCapDevConnected = true;

					if (!bGetCapturePara_audioCapdev_android(pProcInfo, pSharedObj.var.iIndex_capAudio, (uint)sizeof(MIS_MSGU), pProcInfo.av.localAv.recordSoundProcInfo.u_android.pAudioCapDev, ref pProcInfo.av.localAv.recordSoundProcInfo.wf_org, null,ref pProcInfo.av.localAv.recordSoundProcInfo.iRecorder_audioSessionId)) goto errLabel;
					wf_input = pProcInfo.av.localAv.recordSoundProcInfo.wf_org;
					pWf_input = &wf_input;


					break;
				default:
					break;
			}
			if (null==pWf_input)
			{
				//memcpy(&pProcInfo.av.localAv.recordSoundProcInfo.wf_org, &pCompressor->wf_decompress_default, sizeof(pProcInfo.av.localAv.recordSoundProcInfo.wf_org)  );
				pProcInfo.av.localAv.recordSoundProcInfo.wf_org = pCompressor.wf_decompress_default;
				wf_input = pProcInfo.av.localAv.recordSoundProcInfo.wf_org;
				pWf_input = &wf_input;
			}

			//
			qyFuncs.traceLog("startLocalAudioRecorder.125, before initCompressAudio");

			//  2013/08/02
			CAP_procInfo_recordSound pCapAudio = null;
			pCapAudio = pProcInfo.av.localAv.recordSoundProcInfo;

			//	
			//  2016/12/10
			pCompressor.pAudioQ2 = pCapAudio.thread.q2;
			pCompressor.pParent_transform = pCapAudio;
			//
			if (0!=qyFuncs.initCompressAudio(pProcInfo, ref *pWf_input, ref pCompressor, ref pProcInfo.av.localAv.recordSoundProcInfo.compressAudio))
			{
# if  DEBUG
				qyFuncs.traceLog(_T("startLocalAudioRecorder failed, initCompressAudio failed"));
#endif
				//
				goto errLabel;
			}
			bAudioCompressInited = true;

			//
			qyFuncs.traceLog("startLocalAudioRecorder.144, after initCompressAudio");


			//  2013/08/02
			GENERIC_Q_CFG tmpCfg;
			//  uint	dwThreadDaemonId;
			//TCHAR tName[128];
			string str_tName;

			if (true)
			{
				//
				//memcpy(&tmpCfg, &pProcInfo.cfg.toShareAudioQ, sizeof(tmpCfg));
				tmpCfg=pProcInfo.cfg.toShareAudioQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}-%{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				qyFuncs.mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);

				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				qyFuncs.mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
				//_sntprintf(tName, mycountof(tName), _T("%s%u"), CONST_evtName_qm_data_a, pSharedObj.uiTranNo);
				str_tName=string.Format("{0}{1}", Consts.CONST_evtName_qm_data_a, pSharedObj.var.uiTranNo);
				if (0!=qyFuncs.initQyQ2(tmpCfg, pQyMc.cfg.rwLockParam, 2, str_tName, qyFuncs.mallocMemory, qyFuncs.mymalloc, null, null, qyFuncs.freeMemory, qyFuncs.myfree, (PF_qElemRemove)qyFuncs.clean_myPLAY_AUDIO_DATA, pCapAudio.thread.q2)) goto errLabel;
				pCapAudio.thread.bQInited = true;

				//  2017/01/29
				//memcpy(&tmpCfg, &pProcInfo.cfg.appQ, sizeof(tmpCfg));
				tmpCfg=pProcInfo.cfg.appQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				qyFuncs.mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				qyFuncs.mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
				//_sntprintf(tName, mycountof(tName), _T("%s%u"), CONST_evtName_qm_app, pSharedObj.uiTranNo);
				str_tName=string.Format("{0}{1}", Consts.CONST_evtName_qm_app, pSharedObj.var.uiTranNo);
				if (0!=qyFuncs.initQyQ2(tmpCfg, pQyMc.cfg.rwLockParam, 2, str_tName, qyFuncs.mallocMemory, qyFuncs.mymalloc, null, null, qyFuncs.freeMemory, qyFuncs.myfree, (PF_qElemRemove)qyFuncs.clean_myPLAY_AUDIO_DATA, pCapAudio.appThread.q2)) goto errLabel;
				pCapAudio.appThread.bQInited = true;

                //
                qyFuncs.traceLog("startLocalAudioRecorder.188, after initCompressAudio");


                //
                if (null==pCapAudio.thread.hThread)
				{
					//
					pCapAudio.thread.bQuit = false;
					/*
					pCapAudio.thread.hThread = CreateThread(null, 0, mcThreadProc_toShareAudio, (LPVOID)iIndex_sharedObj, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pCapAudio.thread.hThread) goto errLabel;
					pCapAudio.thread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(pCapAudio.thread.hThread) == -1) goto errLabel;
					*/
#if false
					pCapAudio.thread.hThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(qyFuncs.mcThreadProc_toShareAudio));
					pCapAudio.thread.hThread.Start(iIndex_sharedObj);
#endif
					GuiShare.pf_startThread1(ref pCapAudio.thread.hThread, qyFuncs.mcThreadProc_toShareAudio, iIndex_sharedObj);
				}

				//  2017/01/29
				if (null==pCapAudio.appThread.hThread)
				{
					//
					pCapAudio.appThread.bQuit = false;
					/*
					pCapAudio.appThread.hThread = CreateThread(null, 0, mcThreadProc_app, (LPVOID)iIndex_sharedObj, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pCapAudio.appThread.hThread) goto errLabel;
					pCapAudio.appThread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(pCapAudio.appThread.hThread) == -1) goto errLabel;
					*/
#if false
					pCapAudio.appThread.hThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(qyFuncs.mcThreadProc_app));
					pCapAudio.appThread.hThread.Start(iIndex_sharedObj);
#endif
					GuiShare.pf_startThread1(ref pCapAudio.appThread.hThread, qyFuncs.mcThreadProc_app, iIndex_sharedObj);
				}

			}

			//
            qyFuncs.traceLog("startLocalAudioRecorder.230");


            //  2017/06/19
            pRecThread.bQuit = false;
			pRecThread.bRunning = false;

			//
			switch (uiRecordType)
			{
				/*
				case Consts.CONST_recordType_aec:      //  2013/05/22
					{						
						//
						//
						if (pGuid) pProcInfo->av.localAv.recordSoundProcInfo.guid_aDevice = *(GUID*)pGuid;      //  2011/10/24
						else memset(&pProcInfo->av.localAv.recordSoundProcInfo.guid_aDevice, 0, sizeof(pProcInfo->av.localAv.recordSoundProcInfo.guid_aDevice)  );

#if true
				  //
				  //CAP_procInfo_recordSound  *  pRecordSoundProcInfo  =  &pProcInfo->av.localAv.recordSoundProcInfo;
				  
				  //  Create  the thread
				  //QY_qThreadProcInfo_common  *  pRecThread  =  &pRecordSoundProcInfo->recThread;
				  //pRecThread->bQuit  =  FALSE;
				  pRecThread->hThread  =  CreateThread(  NULL,  128*1024,  aecRecThreadFunc,  NULL,  STACK_SIZE_PARAM_IS_A_RESERVATION,  NULL  );
				  if  (  !pRecThread->hThread  )  {
					  //CHECK_START_RECORDING_HR(  E_FAIL,  (  _T(  "Failed: CreateThread() in aec_snd_StartRecording\n"  ),  hr  )  );
					  goto  errLabel;	
				  }
#endif

					}
				   break;

				
				case Consts.CONST_recordType_directX:      //  2013/03/18

					if (startAudioCapDev_dx(pProcInfo.av.localAv.recordSoundProcInfo.u.dx.pAudioCapDev)) goto errLabel;

					break;
				case Consts.CONST_recordType_webRtc:
					//
					TCHAR tBuf[128];
					_sntprintf(tBuf, mycountof(tBuf), _T("startLocalAudioRecorde: before startRecord_webRtc"));
					showInfo_open(0, 0, tBuf);
					//
					if (startRecord_webRtc()) goto errLabel;
					//
					_sntprintf(tBuf, mycountof(tBuf), _T("startLocalAudioRecorde: after startRecord_webRtc"));
					showInfo_open(0, 0, tBuf);
					//
					break;
				*/
				case Consts.CONST_recordType_android:

                    //
                    qyFuncs.traceLog("startLocalAudioRecorder.287, before startAudioCapDev_android");

					//
					if (0 != startAudioCapDev_android(pProcInfo.av.localAv.recordSoundProcInfo.u_android.pAudioCapDev))
					{
                        //
                        qyFuncs.traceLog("startLocalAudioRecorder.293, startAudioCapDev_android failed");

                        goto errLabel;
					}

                    //
                    //
                    qyFuncs.traceLog("startLocalAudioRecorder.294, after startAudioCapDev_android");


                    //pRecThread.hThread = new CreateThread(NULL, 128 * 1024, aecRecThreadFunc, NULL, STACK_SIZE_PARAM_IS_A_RESERVATION, NULL);
#if false
					pRecThread.hThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(recThreadFunc_android));
					if (null==pRecThread.hThread)
					{
						//CHECK_START_RECORDING_HR(  E_FAIL,  (  _T(  "Failed: CreateThread() in aec_snd_StartRecording\n"  ),  hr  )  );
						goto errLabel;
					}
					pRecThread.hThread.Start();
#endif
                    GuiShare.pf_startThread1(ref pRecThread.hThread, recThreadFunc_android, 0);

					//
					break;

				case Consts.CONST_recordType_custom:
#if __DEBUG__
					traceLog(_T("Not finished"));
#endif
					goto errLabel;
					break;
				default:
#if __DEBUG__
					traceLog(_T("unknown uiRecordType %d"), uiRecordType);
#endif
					goto errLabel;
			}

			//  2013/05/24




			iErr = 0;

		errLabel:

			if (0!=iErr)
			{
				stopLocalAudioRecorder(pProcInfo, iIndex_sharedObj, 1);
			}

		//
			qyFuncs.traceLog(("startLocalAudio leaves.\n"));

			//
			return iErr;

		}





		//
		public static unsafe int stopLocalAudioRecorder(CCtxQmc pProcInfo, int index_sharedObj, int nTries)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			bool bDbg = false;
			string str;

#if DEBUG
			bDbg = true;
#endif

			if (null==pProcInfo) return -1;

			//
			str  = _T("stopLocalAudioRecorder enters.");
			qyFuncs.showInfo_open(0, null, str);
#if DEBUG
			qyFuncs.traceLog(str);
#endif


			//
			QY_qThreadProcInfo_common pRecThread = pProcInfo.av.localAv.recordSoundProcInfo.recThread;


			//
			pRecThread.bQuit = true;

			//  2013/03/19
			switch (pProcInfo.av.localAv.recordSoundProcInfo.uiRecordType)
			{
				case Consts.CONST_recordType_dSound:

					//  if  (  !nTries  )  nTries  =  10;	
					nTries = 30;    //  这里必须停下来

					int i;
					for (i = 0; i < nTries; i++)
					{
						if (!qyFuncs.bQThreadExists(pRecThread)) break;

						//pRecThread.bQuit  =  true;		
						//if (pProcInfo.av.localAv.recordSoundProcInfo.recThread.hEvent0!=null) SetEvent(pProcInfo.av.localAv.recordSoundProcInfo.hEvents[0]);
						//if (pProcInfo.cfg.policy.audio.uiRecordType == CONST_recordType_waveform) PostThreadMessage(pRecThread.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_endThread, 0);

						qyFuncs.waitForQThread(pRecThread, 1000);
					}
					if (i == nTries)
					{   //  2013/08/02		
						//qyExitProcess(_T("stopLocalAudioRecorder failed"));
					}
					//
					break;
				case Consts.CONST_recordType_directX:
					//exitAudioCapDev_dx(&pProcInfo.av.localAv.recordSoundProcInfo.u.dx.pAudioCapDev);
					pProcInfo.av.localAv.recordSoundProcInfo.bCapDevConnected = false;
					break;
				case Consts.CONST_recordType_webRtc:
					//stopRecord_webRtc();
					break;
				case Consts.CONST_recordType_android:
					stopAudioCapDev_android(pProcInfo.av.localAv.recordSoundProcInfo.u_android.pAudioCapDev);
					//
					nTries = 30;    //  这里必须停下来

					for (i = 0; i < nTries; i++)
					{
						if (!qyFuncs.bQThreadExists(pRecThread)) break;

						//pRecThread.bQuit  =  true;		
						//if (pProcInfo.av.localAv.recordSoundProcInfo.recThread.hEvent0!=null) SetEvent(pProcInfo.av.localAv.recordSoundProcInfo.hEvents[0]);
						//if (pProcInfo.cfg.policy.audio.uiRecordType == CONST_recordType_waveform) PostThreadMessage(pRecThread.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_endThread, 0);

						qyFuncs.waitForQThread(pRecThread, 1000);
					}
					if (i == nTries)
					{   //  2013/08/02		
						//qyExitProcess(_T("stopLocalAudioRecorder failed"));
					}
					//
					exitAudioCapDev_android(ref pProcInfo.av.localAv.recordSoundProcInfo.u_android.pAudioCapDev);
					//
					pProcInfo.av.localAv.recordSoundProcInfo.bCapDevConnected = false;
					//
					break;
				default:
					break;
			}

			//  2013/08/02
			CAP_procInfo_audioCommon pU = (CAP_procInfo_audioCommon)pProcInfo.av.localAv.recordSoundProcInfo;

			//  2017/01/29
			qyFuncs.stopQThread(pU.appThread, _T("stopLocalAudioRecoder"));
			//  2009/12/07	
			qyFuncs.stopQThread(pU.thread, _T("stopLocalAudioRecorder"));

			//  2017/01/29
			if (pU.appThread.bQInited)
			{
				qyFuncs.exitQyQ2(pU.appThread.q2); pU.appThread.bQInited = false;
			}
			if (pU.thread.bQInited)
			{
				qyFuncs.exitQyQ2(pU.thread.q2); pU.thread.bQInited = false;
			}

			//
			if (!qyFuncs.bQThreadExists(pRecThread))
			{
				qyFuncs.exitCompressAudio(pProcInfo, ref pProcInfo.av.localAv.recordSoundProcInfo.compressAudio);
			}

			//  2013/05/24
			//stopLocalAudioPlayer(pProcInfo, 10);

			//
			str  =  _T("stopLocalAudioRecorder leaves.");
			qyFuncs.showInfo_open(0, null, str);

			//
			return 0;
		}

	}
}