using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {

		public static unsafe int showTaskStatusFunc(object pMisCnt_unused, ulong pIdInfo_logicalPeer, ulong pIdInfo_taskSender, ulong  pIdInfo_taskReceiver_unused, bool bImGrpMem, ushort cmdCode, Int64 tStartTime, uint uiTranNo, uint uiContentType, int iTaskId, int iStatus, bool ucbNotification, ulong pIdInfo_from, byte ucPercent, UInt64  ui64Len_recvd, string fileName, string fullFilePath)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			//  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
			MIS_MSG_taskStatus msgTaskStatus;

			//  if  (  !pIdInfo_taskSender->ui64Id  ||  !pIdInfo_taskReceiver->ui64Id  )  {
			//  	traceLogA(  "showTaskStatus err:"  );  return  -1;
			//  }

			//  报告状态
			mymemset((IntPtr)(byte*)&msgTaskStatus, 0, sizeof(MIS_MSG_taskStatus));
			msgTaskStatus.uiType = Consts.CONST_misMsgType_taskStatus;
			//msgTaskStatus.pMisCnt = pMisCnt;
			if (pIdInfo_logicalPeer!=null) msgTaskStatus.idInfo_logicalPeer.ui64Id = pIdInfo_logicalPeer;
			//
			if (null!=pIdInfo_taskSender) msgTaskStatus.idInfo_taskSender.ui64Id = pIdInfo_taskSender;
			//if (null!=pIdInfo_taskReceiver) msgTaskStatus.idInfo_taskReceiver.ui64Id = pIdInfo_taskReceiver->ui64Id;
			//
			msgTaskStatus.bImGrpMem = bImGrpMem;            //  
															//
			msgTaskStatus.cmdCode = cmdCode;                //  2009/10/18
			msgTaskStatus.tStartTime = tStartTime;      //  2009/10/18
			msgTaskStatus.uiTranNo = uiTranNo;          //  2009/10/18
			msgTaskStatus.uiContentType = uiContentType;    //  
			msgTaskStatus.iTaskId = iTaskId;
			msgTaskStatus.iStatus = iStatus;
			if (ucbNotification)
			{
				msgTaskStatus.ucbNotification = true;
				if (pIdInfo_from!=null) msgTaskStatus.u_notification.idInfo_from.ui64Id = pIdInfo_from;
				mytcsncpy(msgTaskStatus.u_notification.content, fileName, Consts.cntof_content);
			}
			else
			{
				msgTaskStatus.u_transferFileStatus.ucPercent = ucPercent;
				msgTaskStatus.u_transferFileStatus.ui64Len_recvd = ui64Len_recvd;
				if (fileName!=null) mylstrcpyn(msgTaskStatus.u_transferFileStatus.fileName, fileName, Consts.cntof_fileName);
				if (fullFilePath!=null) mylstrcpyn(msgTaskStatus.u_transferFileStatus.fullFilePath, fullFilePath, Consts.MAX_PATH + 1);
			}

			//
			qPostMsg((IntPtr)(byte*)&msgTaskStatus, (uint)sizeof(MIS_MSG_taskStatus), pQyMc.gui.processQ);
			GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

			return 0;
		}



		//
		//
		public static unsafe int showNotification_open(Int64 tStartTime, uint uiTranNo, uint uiContentType, string hint)
		{
			return showTaskStatusFunc(null, 0, 0, 0, false, 0, tStartTime, uiTranNo, uiContentType, 0, 0, true, 0, 0, 0, hint, null);
		}


		//

		public static unsafe int showNotification(object pMisCnt, ulong pIdInfo_logicalPeer, ulong pIdInfo_from, Int64 tStartTime, uint uiTranNo, uint uiContentType, string hint)
		{
			return showTaskStatusFunc(pMisCnt, pIdInfo_logicalPeer, 0, 0, false, 0, tStartTime, uiTranNo, uiContentType, 0, 0, true, pIdInfo_from, 0, 0, hint, null);
		}


		//


		//
		// int  showTaskStatusFunc(  void  *  pMisCnt,  QY_MESSENGER_ID  *  pIdInfo_logicalPeer,  QY_MESSENGER_ID  *  pIdInfo_taskSender,  QY_MESSENGER_ID  *  pIdInfo_taskReceiver,  bool  bImGrpMem,  unsigned  short  cmdCode,  Int64  tStartTime,  uint  uiTranNo,  uint  uiContentType,  int  iTaskId,  int  iStatus,  unsigned  char  ucbNotification,  QY_MESSENGER_ID	*	pIdInfo_from,  unsigned  char  ucPercent,  unsigned  __int64  ui64Len_recvd,  LPCTSTR  fileName,  LPCTSTR  fullFilePath  );

		public static unsafe int showTaskStatus(object pMisCnt, ulong pIdInfo_logicalPeer, ulong pIdInfo_taskSender, ulong pIdInfo_taskReceiver_unused, bool bImGrpMem, uint uiContentType, int iTaskId, int iStatus, byte ucPercent, UInt64  ui64Len_recvd, string fileName, string fullFilePath)
		{
			return showTaskStatusFunc(pMisCnt, pIdInfo_logicalPeer, pIdInfo_taskSender, 0,bImGrpMem, 0, 0, 0, uiContentType, iTaskId, iStatus, false, 0, ucPercent, ui64Len_recvd, fileName, fullFilePath);
		}

		public static unsafe int showTaskStatus(object pMisCnt, ulong pIdInfo_logicalPeer, ulong pIdInfo_taskSender, ulong pIdInfo_taskReceiver_unused, bool bImGrpMem, ushort cmdCode, Int64 tStartTime, uint uiTranNo, uint uiContentType, int iTaskId, int iStatus)
		{
			return showTaskStatusFunc(pMisCnt, pIdInfo_logicalPeer, pIdInfo_taskSender, 0, bImGrpMem, cmdCode, tStartTime, uiTranNo, uiContentType, iTaskId, iStatus, false, 0, 0, 0, null, null);
		}

    }
}
