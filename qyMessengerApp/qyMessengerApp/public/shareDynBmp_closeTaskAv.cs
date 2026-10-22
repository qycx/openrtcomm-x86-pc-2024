using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
	public static unsafe int shareDynBmp_closeTaskAv(ref SHARE_dyn_bmp pSHARE_dyn_bmp)
	{
			int iErr = -1;

			/*
		SHARE_dyn_bmp* pDynBmpMem = (SHARE_dyn_bmp*)pSHARE_dyn_bmp;

# ifdef  __WINCE__
		return -1;
#else
		
		MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();
		if (!pProcInfo) return -1;
		QY_MC* pQyMc = (QY_MC*)pProcInfo->pQyMc;
		FUNCS_for_isCliHelp* pFuncs = QY_GET_FUNCS_for_isCliHelp(pProcInfo);
		if (!pFuncs) return -1;
		CQnmDb db;
		int iDbType = pQyMc->cfg.db.iDbType;
		IM_TASK_RCD rcd;
		CQyMalloc mallocObj;
		MIS_MSG_TASK* pMsgTask = (MIS_MSG_TASK*)mallocObj.mallocf(sizeof(MIS_MSG_TASK));
		if (!pMsgTask) return -1;
		int lenInBytes = 0;

		if (!pDynBmpMem) return -1;

		unsigned  int uiObjType = pDynBmpMem->resObj.uiObjType;

		if (!pDynBmpMem->resObj.usIndex_obj) return 0;

		//
		QM_dbFuncs* pDbFuncs = pQyMc->p_g_dbFuncs;
		if (!pDbFuncs) goto errLabel;
		QM_dbFuncs & g_dbFuncs = *pDbFuncs;

		//  2016/07/20
		QIS_ptz_wnd* pPtzWnd = &pDynBmpMem->ptzWnd;
		if (pPtzWnd->hWnd_localPtzControl)
		{
			if (IsWindow(pPtzWnd->hWnd_localPtzControl))
			{
				SendMessage(pPtzWnd->hWnd_localPtzControl, WM_CLOSE, 0, 0);
			}
			if (!IsWindow(pPtzWnd->hWnd_localPtzControl)) pPtzWnd->hWnd_localPtzControl = NULL;
		}
# ifdef  __DEBUG__
		if (pPtzWnd->hWnd_localPtzControl)
		{
			MACRO_qyAssert(0, _T("sharedDynBmp_closeTaskAv failed, ptzWnd.hWnd is not null"));
		}
#endif

		//
		QY_SHARED_OBJ* pSharedObj;
		pSharedObj = getSharedObjByIndex(pProcInfo, pDynBmpMem->var.iIndex_sharedObj);
		if (!pSharedObj) goto errLabel;


		//
		if (pDynBmpMem->var.ucbLocalVideoOpen
			|| pDynBmpMem->var.ucbUsed_iIndex_sharedObj)        //  2014/09/11
		{
			//  
			//  if  (  pSharedObj->bCapDev  )  memset(  pSharedObj,  0,  sizeof(  pSharedObj[0]  )  );
			//  CWnd  *  pMainWnd  =  AfxGetApp(  )->m_pMainWnd;
			HWND hMainWnd = pQyMc->gui.hMainWnd;
			askSharedObjToStop(pProcInfo, pSharedObj, hMainWnd, _T("shareDynBmp_closeTaskAv"));
			//
		}

		//
		if (!db.getAvailableDb(pQyMc->iDsnIndex_mainSys)) goto errLabel;
		//
		if (!g_dbFuncs.pf_bGetImTaskRcdBySth(db.m_pDbMem->pDb, iDbType, FALSE, NULL, NULL, 0, 0, pDynBmpMem->var.iTaskId, &rcd))
		{
# ifdef  __DEBUG__
			traceLog(_T("dlgShareDynBmps_closeTaskAv: bGetImTaskRcdBySth failed. skipped"));
#endif
		}
		else
		{

			//
			MIS_CNT* pMisCnt = getMisCntByName(pProcInfo, _T(""));

			if (!taskRcd2Msg(pMisCnt, &rcd, pMsgTask)) goto errLabel;

			//  
			unsigned  short usOp = CONST_imOp_send_cancel;

			TASK_PROC_REQ taskProcReq;
			memset(&taskProcReq, 0, sizeof(taskProcReq));
			taskProcReq.uiType = CONST_imCommType_taskProcReq;
			taskProcReq.usOp = usOp;
			taskProcReq.tStartTime_org = rcd.tSendTime;
			taskProcReq.uiTranNo_org = rcd.uiTranNo;
			taskProcReq.uiSerialNo_org = rcd.uiSerialNo;
			taskProcReq.uiContentType_org = rcd.uiContentType;
			//
			lenInBytes = sizeof(taskProcReq);
			//					  
			MACRO_prepareForTran();
			//						  
			if (postMsgTask2Mgr_mc(pMisCnt, CONST_misMsgType_task, 0, pMsgTask->usCode, tStartTran, uiTranNo, 0, rcd.id, rcd.uiType, (char*)&taskProcReq, lenInBytes, NULL, &pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, &rcd.idInfo_recv, pMsgTask->uiChannelType, NULL, 0)) goto errLabel;

#if  0
		  //
		  int						idc				=	m_var.guiData.iIDC_STATIC_pic0;
		  int						idc_dst			=	idc;
		  freeCapImageByTaskId(  m_hWnd,  idc,  rcd.id,  &m_var.images,  M_GetDlgItem(  m_hWnd,  idc_dst  )  );
#endif
		}

		//
		//  memset(  &pShare->mems[index_pShare_mem].var,  0,  sizeof(  pShare->mems[index_pShare_mem].var  )  );
		memset(&pDynBmpMem->var, 0, sizeof(pDynBmpMem->var)  );

		//  2014/09/09
		//mytime(  &m_var.tLastModifiedTime  );
#endif

			*/
			iErr = 0;

	errLabel:

		//qmcApplyForChkingTasks_gui(pQyMc);

		return iErr;
	}

}
}
