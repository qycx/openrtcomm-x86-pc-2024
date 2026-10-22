using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe bool canSave_fourcc(int iFourcc)
		{
			switch (iFourcc)
			{
				case Consts.CONST_fourcc_h264:
				case Consts.CONST_fourcc_vp80:
					return true;
					break;
				default:
					break;
			}
			return false;
		}

		public static unsafe void mcThreadProc_saveAv(object lpParameter)
	{
		int iErr = -1;

			/*
		QY_TRANSFORM* pTransform = (QY_TRANSFORM*)lpParameter;
		if (!pTransform) return -1;
		MC_VAR_isCli* pProcInfo = (MC_VAR_isCli*)pTransform->pProcInfo;
		if (!pProcInfo) return -1;
		PROC_TASK_AV* pTask = (PROC_TASK_AV*)pTransform->pTask;
		if (!pTask) return -1;

		SAVE_av_procInfo* pSave = &pTransform->saveThread;

		QY_MC* pQyMc = (QY_MC*)pProcInfo->pQyMc;
		DYN_LIB_DX* pDynLib = (DYN_LIB_DX*)((QY_DYN_LIBS*)pQyMc->env.pDynLibs)->pLib_dx;
		//  int								i;

		int i;


#define DEFAULT_dwToInMs		5000
		DWORD dwToInMs = DEFAULT_dwToInMs;

		DWORD dwTickCnt_lastVideoData = GetTickCount(); ;                       // 

		CQyMalloc mallocObj_pSvp;
		SAVE_av_pktU* pSp = (SAVE_av_pktU*)mallocObj_pSvp.mallocf(sizeof(SAVE_av_pktU));
		if (!pSp) return -1;



		BOOL bPktsRedirected;

		CQyCoInit coInit;       //  2010/06/07

		CTX_mc_saveAv ctx;      //  2014/03/07

		//
		memset(&ctx, 0, sizeof(ctx));

		traceLogA("mcThreadProc_saveAv enters");

		MACRO_setStep(pSave->debugStep, CONST_threadStep_start);


		//  2014/05/08
		_sntprintf(pSave->debugStep.showInfo.tWhere, mycountof(pSave->debugStep.showInfo.tWhere), _T("saveAv %d:%d"), pTransform->iTaskId, pTransform->index_activeMems_from);
		pSave->debugStep.showInfo.uiStep = 0;

		//
		ctx.uiBufSize_pBuf = 1024;
		ctx.pBuf_forSaving = (unsigned  char*  )mymalloc(ctx.uiBufSize_pBuf);
		if (!ctx.pBuf_forSaving) goto errLabel;

		//
		pSave->bRunning = TRUE;
		//  memset(  &pSave->m_var,  0,  sizeof(  pSave->m_var  )  );

# ifdef  __DEBUG__
		traceLog(_T("Note: 这里最好能根据情况，将maxMsgs自动调整，对每个transform,分配10个misMsgU, 是很浪费的"));
#endif

		//
		if (pTask->ucbVideoConferenceStarter)
		{
			//  if  (  !(  pSave->pMsgTask_toCompress_videoConference  =  (  MIS_MSG_TALK  *  )mallocObj_pMsgTask_toCompress.malloc(  sizeof(  MIS_MSG_TALK  )  )  )  )  goto  errLabel;
		}

		qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, pSave->debugStep.showInfo.tWhere, _T(""), _T("%s %d starts"), pSave->q2.cfg.name, pTransform->index_activeMems_from);

		for (; !pSave->bQuit;)
		{

			if (!dwToInMs)
			{
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Warning: mcThreadProc_transformVideo: dwToInMs %d"), dwToInMs);
				dwToInMs = DEFAULT_dwToInMs;
			}

			//  2009/07/14
			MACRO_setStep(pSave->debugStep, CONST_threadStep_toWait);

			//
			DWORD dwTickCnt0 = GetTickCount();
			DWORD dwTickCnt;
			DWORD dwEvt = MsgWaitForMultipleObjects(
								 pSave->q2.cfg.usCnt_hEvents,       // mycountof(  pSave->hEvents  ),		// Number of events.
							   pSave->q2.hEvents,               // Location of handles.
							   FALSE,                                       // Wait for all?
							   dwToInMs,                                    // How long to wait?
							   0);                                      // Any message is an 
																		// event.
			if (dwEvt == WAIT_FAILED)
			{
				iErr = -110;
				goto errLabel;
			}
			//
			dwTickCnt = GetTickCount();

			if (dwEvt != WAIT_TIMEOUT)
			{
				dwEvt -= WAIT_OBJECT_0;
				switch (dwEvt)
				{
					case 0:
						dwTickCnt_lastVideoData = dwTickCnt;        //  audio data received
						break;
					case 1:
						traceLogA("transformVideo: event 1.");
						break;
					default:
						traceLogA("unknown event");
						iErr = -120;
						goto errLabel;
						break;
				}
			}

			if (isQ2Empty(&pSave->q2)) continue;

			BOOL bExists_svp = FALSE;
			for (; !pSave->bQuit;)
			{

				//
				MACRO_setStep(pSave->debugStep, CONST_threadStep_toGetMsg);


				dyn_doSaveAv(pTransform, pSp, &bExists_svp, &ctx);
				if (bExists_svp) continue;

				if (isQ2Empty(&pSave->q2)) break;

			}
		}

		iErr = 0;

	errLabel:

		//
		MACRO_setStep(pSave->debugStep, CONST_threadStep_toExit);
		pSave->debugStep.iErr_thread = iErr;

#if  0
	 //  2014/03/04
	qdcExitSplInfo(  &g_splInfo  );
	bInited_splInfo  =  FALSE;
#endif

		//
		qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", _T("IsClient"), 0, _T(""), _T(""), _T("%s %d ends."), pSave->q2.cfg.name, pTransform->index_activeMems_from);


		pSave->bRunning = FALSE;

		//  2015/05/09
		MACRO_safeFree(ctx.pBuf_forSaving);

		//
		MACRO_setStep(pSave->debugStep, CONST_threadStep_end);

		traceLogA("mcThreadProc_transformVideo %S leaves", pSave->q2.cfg.name);
			*/

		return;
	}

}
}
