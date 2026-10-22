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


		//
		public static unsafe int initTalkerThread(CCtxQmc pProcInfo, object hDlgTalk_mgr,  DLG_TALK_var pMgrVar, TALKER_threadProcInfo p)
		{
			int iErr = -1;

			/*

			GENERIC_Q_CFG tmpCfg;

			CCtxQmcTmpl* pCtx = pProcInfo;
			RW_lock_param* pQyMc_rwLockParam = pCtx->get_qyMc_rwLockParam();
			if (!pQyMc_rwLockParam) return -1;
			QMC_cfg* pQmcCfg = (QMC_cfg*)pCtx->get_qmc_cfg();
			if (!pQmcCfg) return -1;
			GENERIC_Q_CFG* p_cfg_talkerThreadQ = &pQmcCfg->talkerThreadQ;//Ctx->get_cfg_transformQ(  );
			if (!p_cfg_talkerThreadQ) return -1;
			QMC_status* pStatus = pCtx->get_qmc_status();
			if (!pStatus) return -1;


			//
			if (!hDlgTalk_mgr || !pMgrVar) return -1;
			if (!p) return -1;

			//
			p->hDlgTalk_mgr = hDlgTalk_mgr;
			p->pMgrVarParam = pMgrVar;


			//	
			//  2014/0804		
			{
				memcpy(&tmpCfg, p_cfg_talkerThreadQ, sizeof(tmpCfg));
				_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%I64u"), tmpCfg.name, pMgrVar->addr.idInfo.ui64Id);
				_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%I64u"), tmpCfg.mutexName_prefix, pMgrVar->addr.idInfo.ui64Id);
				//  
				if (initQyQ2(&tmpCfg, pQyMc_rwLockParam, mycountof(p->q2.hEvents), NULL, mallocMemory, mymalloc, 0, 0, freeMemory, myfree, NULL, &p->q2)) goto errLabel;
				p->bQInited = TRUE;
			}

			//		 
			//  2014/08/04
#if  10
		 {
			 DWORD  dwThreadDaemonId;
			 //
			 if  (  !p->hThread  )  {
				 //
				 p->bQuit  =  FALSE;
				 p->hThread  =  CreateThread(  NULL,  0,  mcThreadProc_talkerProc,  p,  CREATE_SUSPENDED,  &dwThreadDaemonId  );
				 if  (  !p->hThread  )  goto  errLabel;					
				 p->dwThreadId  =  dwThreadDaemonId;			
				 if  (  ResumeThread(  p->hThread  )  ==  -1  )  goto  errLabel;	

				 //  2012/03/25
				 InterlockedIncrement(  &pStatus->nThreads_talkerProc  );

			 }
		 }
#endif


			//
			*/

			iErr = 0;
		errLabel:
			return iErr;

		}


		//
		public static unsafe  int exitTalkerThread(CCtxQmc pProcInfo, TALKER_threadProcInfo p)
		{
			/*

			CCtxQmcTmpl* pCtx = pProcInfo;
			if (!pCtx) return -1;
			QMC_status* pStatus = pCtx->get_qmc_status();
			if (!pStatus) return -1;
			TCHAR tBuf[128] = _T("exitTalkerThread");

			//2014/08/04	
			if (p->hThread)
			{
				p->bQuit = TRUE;
				//
				SetEvent(p->q2.hEvents[0]);
				waitForObject(&p->hThread, 20000);
				//
				if (p->hThread)
				{
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T("Failed"), _T("WaitFor talkerThread"));
				}
				else InterlockedDecrement(&pStatus->nThreads_talkerProc);
			}
			if (p->hThread)
			{
				p->bQuit = TRUE;
				//
				SetEvent(p->q2.hEvents[0]);
				waitForObject(&p->hThread, 20000);
				//
				//tmp_getHint_video(  pTask,  p,  tBuf,  mycountof(  tBuf  )  );
				//
				if (p->hThread)
				{
					qyExitProcess(tBuf);
				}
				else InterlockedDecrement(&pStatus->nThreads_talkerProc);
			}



			//  2014/08/04	
			if (p->bQInited)
			{
				exitQyQ2(&p->q2);
				p->bQInited = FALSE;
			}
			*/

			return 0;
		}


	}
}