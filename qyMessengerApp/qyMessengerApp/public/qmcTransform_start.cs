using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
{
		public static bool isTooManyThreads()
        {
			return false;
        }

		//


		// 
		public static unsafe int startTransformThread(CCtxQmc pProcInfo, uint uiTaskType, ref PROC_TASK_AV pTask, QY_TRANSFORM p)
		{
			int iErr = -1;
			uint dwThreadDaemonId;
			//MC_VAR_isCli	*	pProcInfo	=	QY_GET_procInfo_isCli(  );

			ref QMC_status pStatus = ref pProcInfo.status;//->get_qmc_status();
														  //if (!pStatus) return -1;

			//TCHAR tHintBuf[128] = _T("");

			//  QY_TRANSFORM	*	p	=	&pTask.pTransforms[i];

			string str;
			str = string.Format("startTransformThread called, {0}",p.index_activeMems_from);
			showInfo_open(0, null, str);


			//  2014/08/04
			if (pTask.saveVideo.ucbSaveVideo_req)
			{
				if (null==p.saveThread.hThread)
				{
					//
					p.saveThread.bQuit = false;
					/*
					p.saveThread.hThread = CreateThread(NULL, 0, mcThreadProc_saveAv, p, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!p.saveThread.hThread) goto errLabel;
					p.saveThread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(p.saveThread.hThread) == -1) goto errLabel;
					*/
#if false
					p.saveThread.hThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(mcThreadProc_saveAv));
					p.saveThread.hThread.Start(p);
#endif
					GuiShare.pf_startThread1(ref p.saveThread.hThread, mcThreadProc_saveAv, p);

					//  2012/03/25
					//InterlockedIncrement(&pStatus->nThreads_dec);
					Interlocked.Increment(ref pStatus.nThreads_dec);

				}
			}

			//if  (  !bVideoOnly  )  
			{
				if (null==p.audio.hThread)
				{
					//
					p.audio.bQuit = false;
					/*
					p.audio.hThread = CreateThread(NULL, 0, mcThreadProc_decAudio, p, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!p.audio.hThread) goto errLabel;
					p.audio.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(p.audio.hThread) == -1) goto errLabel;
					*/
#if false
					p.audio.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_decAudio));
					p.audio.hThread.Start(p);
#endif
					GuiShare.pf_startThread1(ref p.audio.hThread, mcThreadProc_decAudio, p);

					//  2012/03/25
					//InterlockedIncrement(&pStatus->nThreads_dec);
					Interlocked.Increment(ref pStatus.nThreads_dec);

				}
				if (p.bNeed_decV)
				{
#if false
					if (null == p.video.hThread)
					{
						//
						p.video.bQuit = false;
						/*
						p.video.hThread = CreateThread(NULL, 0, mcThreadProc_decVideo, p, CREATE_SUSPENDED, &dwThreadDaemonId);
						if (!p.video.hThread) goto errLabel;
						p.video.dwThreadId = dwThreadDaemonId;
						if (ResumeThread(p.video.hThread) == -1) goto errLabel;
						*/
						p.video.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_decVideo));
						p.video.hThread.Start(p);

						//  2012/03/25
						//InterlockedIncrement(&pStatus->nThreads_dec);
						Interlocked.Increment(ref pStatus.nThreads_dec);

					}
#endif
					//
					startTransformThread_v(pProcInfo, p,"called in startTrans");
				}
				//
#if false//true/
				//  2011/12/09
				if (null==p.v_preTransThread.hThread)
				{
					//
					p.v_preTransThread.bQuit = false;
					/*
					p.v_preTransThread.hThread = CreateThread(NULL, 0, mcThreadProc_preTransformVideo, p, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!p.v_preTransThread.hThread) goto errLabel;
					p.v_preTransThread.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(p.v_preTransThread.hThread) == -1) goto errLabel;
					*/
					p.v_preTransThread.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_preTransformVideo));
					p.v_preTransThread.hThread.Start(p);

					//  2012/03/25
					//InterlockedIncrement(&pStatus->nThreads_dec);
					Interlocked.Increment(ref pStatus.nThreads_dec);

				}
#endif
				//
			}


			iErr = 0;

		errLabel:
			return iErr;

		}

		public static unsafe int startTransformThreads(CCtxQmc pProcInfo, uint uiTaskType, ref PROC_TASK_AV pTask,string hint)
		{
			int iErr = -1;
			int i;
			uint dwThreadDaemonId;

			//TCHAR tHintBuf[128] = _T("");

			//BOOL				bVideoOnly		=	false;
			if (hint == null) hint = "";

			//
			if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
				traceLog("For test, startTransformThread returns 0");
				return 0;
            }

			//
			string str;
			str = string.Format("startTransformThreads called, {0}", hint);
			showInfo_open(0, null, str);

			//
			if (isTooManyThreads())
			{
				//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("too many dec threads"));
				goto errLabel;
			}

			//
			for (i = 0; i < pTask.trans.usCntLimit_transforms; i++)
			{
				QY_TRANSFORM p = pTask.trans.pTransforms[i];

				//
				if (0!=startTransformThread(pProcInfo, uiTaskType, ref pTask, p)) goto errLabel;

			}

			//  2014/11/11
#if false
			if (pTask.confMosaicTrans.bExists_confMosaic)
			{
				if (0!=startTransformThread(pProcInfo, uiTaskType, ref pTask, pTask.confMosaicTrans.confMosaic_video1.transform)) goto errLabel;
				if (0!=startTransformThread(pProcInfo, uiTaskType, ref pTask, pTask.confMosaicTrans.confMosaic_resource.transform)) goto errLabel;

			}
#endif

			//
			iErr = 0;
		errLabel:

			OutputDebugString(_T("startTransformTrheads leaves with "));
			//OutputDebugString(iErr ? _T("failed") : _T("Ok"));
			OutputDebugString(_T("\n"));

			if (0!=iErr)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("startTransformThreads failed, %s"), tHintBuf);
			}

			return iErr;
		}



	}
}
