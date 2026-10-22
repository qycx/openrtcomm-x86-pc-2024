using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int newstartQThreadToShareAv(CCtxQmc pProcInfo, int iIndex_sharedObj, bool bAudio)
		{
			int iErr = -1;
			if (null==pProcInfo) return -1;
			CCtxQyMc pQyMc = g.g_pQyMc;//QY_GET_GBUF(  );
			ref QMC_cfg pCfg = ref pProcInfo.cfg;

			//
			QY_SHARED_OBJ pSharedObj = null;

			//  2011/07/27	
			//TCHAR tName[128];
			TMP_tBuf128 tName;
			int cntof_tName = Consts.cntof_tBuf128; 
			GENERIC_Q_CFG tmpCfg;
			GENERIC_Q_cfgEx qCfgEx;
			uint dwThreadDaemonId;
			string str;

			pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) goto errLabel;

			//
			if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
				traceLog("for test, newstartQThreadToShareAv return 0");
				return 0;
            }



			//
			if (bAudio)
			{
				CAP_procInfo_audioCommon pCapAudio = null;

				pCapAudio = (CAP_procInfo_audioCommon)getCapAudioBySth(pProcInfo, pSharedObj.var.iIndex_capAudio, 0);
				if (null==pCapAudio) goto errLabel;

				//  2015/10/11
				if (pCapAudio.thread.bQInited)
				{
# if  __DEBUG__
					//  2015/10/11
					MACRO_qyAssert(0, _T("startQThreadToShareAv failed, capA.bQInited is already true"));
#endif
					showInfo_open(0, null, ("startQThreadToShareAv failed, capA.bQInited is already true"));
					goto errLabel;
				}

				//
				//memcpy(&tmpCfg, &pCfg.toShareAudioQ, sizeof(tmpCfg));
				tmpCfg = pCfg.toShareAudioQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
				//_sntprintf(tName, mycountof(tName), _T("%s%u"), CONST_evtName_qm_data_a, pSharedObj.uiTranNo);
				str=string.Format("{0}{1}", Consts.CONST_evtName_qm_data_a, pSharedObj.var.uiTranNo);
				mytcsncpy(tName.tBuf, str, cntof_tName);

				if (0!=initQyQ2(tmpCfg, pQyMc.cfg.rwLockParam, 2, new string(tName.tBuf), mallocMemory, mymalloc, null, null, freeMemory, myfree, (PF_qElemRemove)clean_myPLAY_AUDIO_DATA, pCapAudio.thread.q2)) goto errLabel;
				pCapAudio.thread.bQInited = true;

				//
				if (null==pCapAudio.thread.hThread)
				{
					//
					pCapAudio.thread.bQuit =false;
					//
					/*
					pCapAudio.thread.hThread = CreateThread(null, 0, mcThreadProc_toShareAudio, (LPVOID)iIndex_sharedObj, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pCapAudio.thread.hThread) goto errLabel;
					pCapAudio.thread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(pCapAudio.thread.hThread) == -1) goto errLabel;
					*/
#if false
					pCapAudio.thread.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_toShareAudio));
					pCapAudio.thread.hThread.Start(iIndex_sharedObj);
#endif
					GuiShare.pf_startThread1(ref pCapAudio.thread.hThread, mcThreadProc_toShareAudio, iIndex_sharedObj);

				}
			}

			//
			{
				CAP_procInfo_bmpCommon pCapBmp = null;

				pCapBmp = (CAP_procInfo_bmpCommon)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
				if (null==pCapBmp) goto errLabel;


				//////////////////////////////////

				//  2011/12/06
				//memcpy(&tmpCfg, &pCfg.toShareVideo_postQ, sizeof(tmpCfg));
				tmpCfg = pCfg.toShareVideo_postQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);

				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

				//_sntprintf(tName, mycountof(tName), _T("%s%u"), CONST_evtName_qm_toShareVideo_post, pSharedObj.uiTranNo);
				str=string.Format("{0}{1}", Consts.CONST_evtName_qm_toShareVideo_post, pSharedObj.var.uiTranNo);
				mytcsncpy(tName.tBuf, str, cntof_tName);
				if (pCapBmp.postThread.bQInited)
				{
#if __DEBUG__
					//  2015/10/11
					MACRO_qyAssert(0, _T("startQThreadToShareAv failed, capV.bQInited is  already true."));
#endif
					showInfo_open(0, null, ("startQThreadToShareAv failed, capV.bQInited is  already true."));
					goto errLabel;
				}
				//  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  mycountof(  pCapBmp.postThread.q2.hEvents  ),  CQyString(  pQyMc.appParams.appObjPrefix  )  +  tName,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pCapBmp.postThread.q2  )  )  goto  errLabel;
				//memset(&qCfgEx, 0, sizeof(qCfgEx));
				qCfgEx = new GENERIC_Q_cfgEx();
				//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
				qCfgEx.common = tmpCfg;
				qCfgEx.pfQElemNewEx = mallocMemory;
				qCfgEx.pfQElemNew = mymalloc;
				qCfgEx.pfQElemFreeEx = freeMemory;
				qCfgEx.pfQElemFree = myfree;
				qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
				if (0!=initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, new string(tName.tBuf), pCapBmp.postThread.q2)) goto errLabel;
				pCapBmp.postThread.bQInited = true;

				//  2010/12/29
				pCapBmp.compressVideo.debugInfo.nStep = 35;


				//
				//memcpy(&tmpCfg, &pCfg.toShareVideoQ, sizeof(tmpCfg));
				tmpCfg = pCfg.toShareVideoQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);

				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

				//_sntprintf(tName, mycountof(tName), _T("%s%u"), CONST_evtName_qm_data_v, pSharedObj.uiTranNo);
				str=string.Format("{0}{1}", Consts.CONST_evtName_qm_data_v, pSharedObj.var.uiTranNo);
				mytcsncpy(tName.tBuf, str, cntof_tName);

				if (pCapBmp.thread.bQInited)
				{
					traceLog(("startQThreadToShareAv failed, bQInited is  already true."));
					goto errLabel;
				}
				//  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  mycountof(  pCapBmp.thread.q2.hEvents  ),  CQyString(  pQyMc.appParams.appObjPrefix  )  +  tName,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pCapBmp.thread.q2  )  )  goto  errLabel;
				//memset(&qCfgEx, 0, sizeof(qCfgEx));
				qCfgEx = new GENERIC_Q_cfgEx();
				//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
				qCfgEx.common = tmpCfg;
				qCfgEx.pfQElemNewEx = mallocMemory;
				qCfgEx.pfQElemNew = mymalloc;
				qCfgEx.pfQElemFreeEx = freeMemory;
				qCfgEx.pfQElemFree = myfree;
				qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
				if (0!=initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, new string(tName.tBuf), pCapBmp.thread.q2)) goto errLabel;
				pCapBmp.thread.bQInited = true;

				//  2010/12/29
				pCapBmp.compressVideo.debugInfo.nStep = 37;


				//  2011/12/04
				//memcpy(&tmpCfg, &pCfg.vppQ, sizeof(tmpCfg));
				tmpCfg = pCfg.vppQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);

				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

				//_sntprintf(tName, mycountof(tName), _T("%s%u"), CONST_evtName_qm_vpp, pSharedObj.uiTranNo);
				str=string.Format("{0}{1}", Consts.CONST_evtName_qm_vpp, pSharedObj.var.uiTranNo);
				mytcsncpy(tName.tBuf, str, cntof_tName);

				if (pCapBmp.vppThread.bQInited)
				{
					traceLog(("startQThreadToShareAv failed, vppThread.bQInited is  already true."));
					goto errLabel;
				}
				//  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  mycountof(  pCapBmp.vppThread.q2.hEvents  ),  CQyString(  pQyMc.appParams.appObjPrefix  )  +  tName,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pCapBmp.vppThread.q2  )  )  goto  errLabel;
				//memset(&qCfgEx, 0, sizeof(qCfgEx));
				qCfgEx = new GENERIC_Q_cfgEx();
				//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
				qCfgEx.common = tmpCfg;
				qCfgEx.pfQElemNewEx = mallocMemory;
				qCfgEx.pfQElemNew = mymalloc;
				qCfgEx.pfQElemFreeEx = freeMemory;
				qCfgEx.pfQElemFree = myfree;
				qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
				if (0!=initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, new string(tName.tBuf), pCapBmp.vppThread.q2)) goto errLabel;
				pCapBmp.vppThread.bQInited = true;
				//



//

				//
				if (null==pCapBmp.postThread.hThread)
				{
					//
					pCapBmp.postThread.bQuit = false;
					/*
					pCapBmp.postThread.hThread = CreateThread(null, 0, mcThreadProc_toShareVideo_post, (LPVOID)iIndex_sharedObj, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pCapBmp.postThread.hThread) goto errLabel;
					pCapBmp.postThread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(pCapBmp.postThread.hThread) == -1) goto errLabel;
					*/
#if false
					pCapBmp.postThread.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_toShareVideo_post));
					pCapBmp.postThread.hThread.Start(iIndex_sharedObj);
#endif
					GuiShare.pf_startThread1(ref pCapBmp.postThread.hThread, mcThreadProc_toShareVideo_post, iIndex_sharedObj);
				}


#if false
				
				//
				if (null==pCapBmp.thread.hThread)
				{
					//
					pCapBmp.thread.bQuit = false;
					/*
					pCapBmp.thread.hThread = CreateThread(null, 0, mcThreadProc_encVideo, (LPVOID)iIndex_sharedObj, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pCapBmp.thread.hThread) goto errLabel;
					pCapBmp.thread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(pCapBmp.thread.hThread) == -1) goto errLabel;
					*/
					pCapBmp.thread.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_encVideo));
					pCapBmp.thread.hThread.Start(iIndex_sharedObj);
				}

				//  2010/12/29
				pCapBmp.compressVideo.debugInfo.nStep = 38;


				//
				if (null==pCapBmp.vppThread.hThread)
				{
					//
					pCapBmp.vppThread.bQuit = false;
					/*
					pCapBmp.vppThread.hThread = CreateThread(null, 0, mcThreadProc_vpp, (LPVOID)iIndex_sharedObj, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pCapBmp.vppThread.hThread) goto errLabel;
					pCapBmp.vppThread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(pCapBmp.vppThread.hThread) == -1) goto errLabel;
					*/
					pCapBmp.vppThread.hThread=new Thread(new ParameterizedThreadStart(mcThreadProc_vpp));
					pCapBmp.vppThread.hThread.Start(iIndex_sharedObj);

				}

#endif


			}

			iErr = 0;

		errLabel:

			return iErr;
		}

	}
}
