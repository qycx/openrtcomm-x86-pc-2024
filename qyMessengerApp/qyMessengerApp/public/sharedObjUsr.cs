using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{


		public static unsafe int getNewSharedObjUsrFunc(CCtxQmc pProcInfo, object hWnd, int iWndContentType, int iIndex_sharedObj, bool bLocalAv, int* piIndex_sharedObjUsr, bool* pbFirstUsr)
		{
			int iErr = -1;
			//MC_VAR_isCli				*		pProcInfo			=	QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;
			CCtxQyMc pQyMc = g.g_pQyMc;//QY_GET_GBUF(  );
			if (null==pQyMc) return -1;
			//
			int index = iIndex_sharedObj;   //  0;
											//
			QY_sharedObj_sync pSharedObjInfo = null;
			QY_SHARED_OBJ pSharedObj = null;
			bool bFirstUsr = false;

			if (!GuiShare.pf_IsWindow(hWnd)) return -1;

			pSharedObjInfo = getSharedObjSyncByIndex(pProcInfo, index);
			if (null==pSharedObjInfo) return -1;
			pSharedObj = pSharedObjInfo.sharedObj;
			//
			if (pSharedObj.bUsed)
			{
				if (pSharedObj.var.bQuit)
				{
					showInfo_open(0, null, ("getNewSharedObjUsrFunc failed, sharedObj.bQuit is true"));
					showNotification_open(0, 0, 0, ("getNewSharedObjUsrFunc failed, sharedObj.bQuit is true"));
					return -1;
				}
			}

			if (!pSharedObj.bUsed)
			{
				//memset(pSharedObj, 0, sizeof(pSharedObj[0]));
				pSharedObjInfo.sharedObj.memset0();
				pSharedObj = pSharedObjInfo.sharedObj;
				//
				int tt = 0;
				pSharedObj.var.uiTranNo = getuiNextTranNo(null, 0, ref tt);
				pSharedObj.bUsed = true;

				//
				//pSharedObj.pProcInfo = pProcInfo;                  //  2015/10/15

				//
				pSharedObj.var.dwTickCnt_start = myGetTickCount(null);       //  2011/08/01

				//
				pSharedObj.var.iIndex_curUsr = -1;                     //  2010/09/07

				//  2014/09/30
				syncMtCnt_start(pSharedObjInfo.syncMtCnt, getuiNextTranNo);

				//
				bFirstUsr = true;
			}

			int i;
			for (i = 0; i < pSharedObj.usrs.Length; i++)
			{
				if (pSharedObj.usrs[i].hWnd_starter == hWnd) break;
			}
			if (i == (pSharedObj.usrs.Length))
			{
				for (i = 0; i < (pSharedObj.usrs.Length); i++)
				{
					if (!GuiShare.pf_IsWindow(pSharedObj.usrs[i].hWnd_starter)) break;
				}
				if (i == (pSharedObj.usrs.Length)) return -1;
				//
				/*
				if (pQyMc->iCustomId != CONST_qyCustomId_hzj)
				{
					//
					if (!bFirstUsr)
					{  //  2017/02/12
						showInfo_open(0, 0, _T("getNewSharedObjUsrFunc failed, not first usr"));
						return -1;
					}
					//
				}
				*/
			}
			//
			//memset(pSharedObjUsr, 0, sizeof(pSharedObjUsr[0]));
			pSharedObj.usrs[i].memset0();
			SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, i);
			pSharedObjUsr.hWnd_starter = hWnd;
			pSharedObjUsr.iWndContentType = iWndContentType;

			//
			if (null!=piIndex_sharedObjUsr) *piIndex_sharedObjUsr = i;
			if (null!=pbFirstUsr) *pbFirstUsr = bFirstUsr;

			//
			iErr = 0;

			return iErr;    //  index;
		}


		//
		public static unsafe int getNewSharedObjUsr_localAv(CCtxQmc pProcInfo, object hWnd, int iWndContentType, int* piIndex_sharedObj, int* piIndex_sharedObjUsr, bool* pbFirstUsr)
		{
			if (null==piIndex_sharedObj || null==piIndex_sharedObjUsr) return -1;

			int iIndex_sharedObj_localAv = 0;
			bool bLocalAv = true;
			if (0!=getNewSharedObjUsrFunc(pProcInfo, hWnd, iWndContentType, iIndex_sharedObj_localAv, bLocalAv, piIndex_sharedObjUsr, pbFirstUsr)) return -1;
			//
			*piIndex_sharedObj = iIndex_sharedObj_localAv;

			return 0;
		}

		//
		public static unsafe int getNewSharedObjUsr(CCtxQmc pProcInfo, object hWnd, int iWndContentType, int iIndex_sharedObj, int* piIndex_sharedObjUsr, bool* pbFirstUsr)
		{
			bool bLocalAv = false;

			return getNewSharedObjUsrFunc(pProcInfo, hWnd, iWndContentType, iIndex_sharedObj, bLocalAv, piIndex_sharedObjUsr, pbFirstUsr);
		}

		public static unsafe int freeSharedObjUsrFunc(CCtxQmc pMC_VAR_isCli, int index, bool bLocalAv, object hWnd,int iWndContentType )
		{
			int iErr = -1;

			//
			CCtxQmc pProcInfo = (CCtxQmc)pMC_VAR_isCli;

			//
			if (index != 0)
			{
				traceLog(("freeSharedObjUsr_localAv failed, index error"));
				return -1;
			}
			//  int								index				=	0;
			//  MC_VAR_isCli				*		pProcInfo			=	QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;
			CCtxQyMc pQyMc = g.g_pQyMc;//QY_GET_GBUF(  );
			if (null==pQyMc) return -1;
			//
			//QY_sharedObj_sync		*		pSharedObjInfos		=	NULL;
			QY_SHARED_OBJ pSharedObj = null;

			//if (!GuiShare.pf_IsWindow(hWnd)) return -1;
			if (hWnd == null) return -1;

			//pSharedObjInfos  =  (  QY_sharedObj_sync  *  )pProcInfo->pSharedObjSyncs;
			//pSharedObj  =  &pSharedObjInfos[index].sharedObj;
			pSharedObj = getSharedObjByIndex(pProcInfo, index);
			if (null==pSharedObj) return -1;

			//
			//if (pSharedObj.bQuit) return -1;

			if (!pSharedObj.bUsed) return -1;

			int i;
			for (i = 0; i < pSharedObj.usrs.Length; i++)
			{
				if (pSharedObj.usrs[i].hWnd_starter == hWnd)
				{
					//memset(&pSharedObj.usrs[i], 0, sizeof(pSharedObj.usrs[i])  );
					pSharedObj.usrs[i].memset0();

			if (pSharedObj.var.iIndex_curUsr == i) pSharedObj.var.iIndex_curUsr = -1;     //  2010/09/07  
		}
	}

	//  check if sharedObjUsr exists.
	for  (i  =  0;  i<pSharedObj.usrs.Length;  i  ++  )  {
		 if  (pSharedObj.usrs[i].hWnd_starter!=null  )  break;
	}
if (i == pSharedObj.usrs.Length)
{
				//askSharedObjToStop(pProcInfo, pSharedObj, pQyMc.gui.hMainWnd, ("freeSharedObjUsr_localAv"));
				freeSharedObjAndSlave(hWnd,iWndContentType, index);
}

return 0;
 }


  //
  public static int freeSharedObjUsr_localAv(CCtxQmc pMC_VAR_isCli, int index, object hWnd,int iWndContentType)
{
	bool bLocalAv = true;
	return freeSharedObjUsrFunc(pMC_VAR_isCli, index, bLocalAv, hWnd,iWndContentType);
}

//
public static int freeSharedObjUsr(CCtxQmc pMC_VAR_isCli, int index, object hWnd,int iWndContentType)
{
	bool bLocalAv = false;
	return freeSharedObjUsrFunc(pMC_VAR_isCli, index, bLocalAv, hWnd,iWndContentType);
}


		//

		//  2011/03/16
		public static unsafe int resetCurSharedObjUsrFunc(CCtxQmc pProcInfo, int index_sharedObj, bool bLocalAv)
		{
			if (null==pProcInfo) return -1;
			//
			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, index_sharedObj);
			if (null==pSharedObj) return -1;

			/*
			HWND hWnd_starter_old = NULL;
			SHARED_OBJ_USR* pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj->iIndex_curUsr);
			if (pSharedObjUsr && IsWindow(pSharedObjUsr->hWnd_starter))
			{
				QY_WMBUF_COMM wmBuf;
				memset(&wmBuf, 0, sizeof(wmBuf));

				wmBuf.uiType = CONST_qyWmParam_requestToSpeak;
				wmBuf.u.requestToSpeak.uiCmd = ID_stopSpeaking;
				SendMessage(pSharedObjUsr->hWnd_starter, CONST_qyWm_comm, 0, (LPARAM) & wmBuf);
				//
				hWnd_starter_old = pSharedObjUsr->hWnd_starter;
			}
			*/

			pSharedObj.var.iIndex_curUsr = -1;

			/*
			if (IsWindow(hWnd_starter_old))
			{
				//  InvalidateRect(  hWnd_starter_old,  NULL,  TRUE  );
				PostMessage(hWnd_starter_old, CONST_qyWm_postComm, CONST_qyWmParam_curSharedObjUsrChanged_mgr, 0);
			}
			*/

			return 0;
		}


		//
		public static unsafe int resetCurSharedObjUsr_localAv(CCtxQmc pMC_VAR_isCli, int index_sharedObj)
		{
			bool bLocalAv = true;
			return resetCurSharedObjUsrFunc(pMC_VAR_isCli, index_sharedObj, bLocalAv);
		}

		//
		public static unsafe int resetCurSharedObjUsr(CCtxQmc pMC_VAR_isCli, int index_sharedObj)
		{
			bool bLocalAv = false;
			return resetCurSharedObjUsrFunc(pMC_VAR_isCli, index_sharedObj, bLocalAv);
		}



	}
}
