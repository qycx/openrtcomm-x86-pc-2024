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
	public static unsafe int refreshTalkToInfo_mgr(object hDlgTalk_mgr)
        {
            return -1;
        }


public static unsafe int dlgTalk_doLayout(object hDlgTalk)
        {
            return -1;
        }


public static unsafe int dlgTalk_procTask_display_op_mgr(object hDlgTalk_mgr, int iTaskId, ushort usOp, MIS_MSG_TASK* pMsgTask, ref IM_TASK_RCD  rcd, int iStatus_old, int iStatus, string paramStrBuf)
        {
            return -1;
        }

		//
		public static unsafe int procSendFile(string fullFilePath, string fileName, void* pPROC_offlineRes_u, object m_hWnd, DLG_TALK_var pDLG_TALK_var, int* piTaskId)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
			if (null==pFuncs) return -1;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			DLG_TALK_var pm_var = (DLG_TALK_var)pDLG_TALK_var;
			if (null==pm_var) return -1;
			DLG_TALK_var  m_var = pm_var;

			//QY_MC* pQyMc = QY_GET_GBUF();
			//IHTMLDocument2* pDoc = NULL;
			//TCHAR tBuf[256] = _T("");
			UInt64 ui64FileLen = 0;
			CQyMalloc mallocObj;
			//  unsigned  char		ucFlg		=	0;


			if (null==fullFilePath) return -1;
#if false
			if (GetFileAttributes(fullFilePath) == -1)
			{
				traceLogA("procSendFile: file not exists, %S", fullFilePath);
				return -1;
			}
#endif

			//  ÕâÀï£¬ÔÝÊ±²ÉÓÃ32Î»µÄÎÄ¼þ´óÐ¡¡£ÒÔºó¸ÄÎª64Î»µÄ£¬ÒÔÖ§³Ö´óÎÄ¼þ
#if false
		struct _stat64 statBuf;
	if  (_tstat64(fullFilePath,  &statBuf  )  )  return  -1;
	ui64FileLen  =  statBuf.st_size;
#endif
			ui64FileLen = qyFuncs_Droid.getFileLen(fullFilePath);


	//
	if(ui64FileLen==0) {
		qyFuncs.showNotification_open(0,0,0,_T( "send failed, fileLen is 0" ) );
		return -1;
	}


	//
	//
	int iTaskId = 0;
			
			//
			{
				object pDb = null;
				using (CQnmDb db = new CQnmDb())
				{
					;
					if (null==db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
					pDb = db.m_pDbMem.pDb;

					iTaskId = qyFuncs.newTaskId(pDb, pQyMc.cfg.db.iDbType);
				}
			}
int iTaskId_suggested = iTaskId;

int iIndex_taskInfo = -1;

uint uiTaskDataType = 0;
uiTaskDataType = Consts.CONST_taskDataType_transferFile;
iIndex_taskInfo = qyFuncs.newTaskInfoIndex(null,pProcInfo, uiTaskDataType, m_hWnd, iTaskId_suggested, _T("procSendFile"));
if (iIndex_taskInfo < 0)
{
	qyFuncs.traceLogA("procSendAvInfoFunc faile, newTaskInfoIndex failed");
	goto errLabel;
}
QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null,pProcInfo, iIndex_taskInfo,"");
if (pTaskInfo == null) goto errLabel;




			Int64 tStartTran;uint uiTranNo;
qyFuncs.MACRO_prepareForTran(out tStartTran,out uiTranNo);

			//  ÏÈ·ÅÈëÊý¾Ý¿âÖÐ£¬·¢±éÖØ·¢ºÍ¼ÇÂ¼×´Ì¬
			QMC_taskData_common taskData = pTaskInfo.getTaskData();
            //MIS_MSG_TASK				*	pMsg	=	(  MIS_MSG_TASK  *  )mallocObj.mallocf(  sizeof(  MIS_MSG_TASK  )  );
            ref MIS_MSGU pMsgU = ref taskData.msgU;
            fixed (MIS_MSG_TASK* pMsg = &taskData.msgU.task) {
				if (null==pMsg) goto errLabel;
				TRANSFER_FILE_REQ* pContent;
				uint lenInBytes;
				IM_TASK_RCD rcd;
				//
				qyFuncs.MACRO_memset_misMsgTask(ref *pMsg);
				pMsg->uiType = Consts.CONST_misMsgType_task;
				//
				//pMsg->pMisCnt = m_var.pMisCnt;
				//
				pMsg->ucFlg = Consts.CONST_commFlg_talkData;
				//
				pMsg->usCode = Consts.CONST_qyCmd_sendRobotTask;
				pMsg->tStartTime = tStartTran;
				pMsg->uiTranNo = uiTranNo;
				//
				pContent = (TRANSFER_FILE_REQ*)&pMsg->data.buf;
				qyFuncs.mymemset((IntPtr)pContent, 0, sizeof(TRANSFER_FILE_REQ));
				pContent->uiType = Consts.CONST_imCommType_transferFileReq;
				qyFuncs.mylstrcpyn(pContent->fullFilePath, fullFilePath, Consts.MAX_PATH+1);
				if (null!=fileName && fileName.Length!=0)
				{
					qyFuncs.safeTcsnCpy(fileName, pContent->fileName, Consts.cntof_fileName);
				}
				else
				{
					string dirName=null;
					string finalFileName=null;
					if (0!=getDirAndFinalName(new string(pContent->fullFilePath), ref dirName, ref finalFileName)) goto errLabel;
					qyFuncs.safeTcsnCpy(finalFileName, pContent->fileName, Consts.cntof_fileName);
				}
				pContent->ui64FileLen = ui64FileLen;
				//  2014/07/13
				uint uiSizePerSnd_internal = 0;
				if (pProcInfo.processQ_media.isEmpty()) uiSizePerSnd_internal = pProcInfo.cfg.uiSizePerSnd_file_noAv;
				else uiSizePerSnd_internal = pProcInfo.cfg.uiSizePerSnd_file_avExists;
				pContent->uiSizePerSnd_suggest = uiSizePerSnd_internal;

				//  2015/08/05
#if false
				if (pPROC_offlineRes_u)
				{
					PROC_offlineRes_u* pProcOfflineRes = (PROC_offlineRes_u*)pPROC_offlineRes_u;
					if (pProcOfflineRes->common.usSubtype == CONST_procOfflineResSubtype_upload)
					{
						pContent->usSubtype = pProcOfflineRes->common.usSubtype;
						pContent->idInfo_logicalPeer.ui64Id = pProcOfflineRes->upload.idInfo_logicalPeer.ui64Id;
						//
						pContent->hTool_offlineRes = pProcOfflineRes->upload.hTool_offlineRes;
					}
					else if (pProcOfflineRes->common.usSubtype == CONST_procOfflineResSubtype_download)
					{
						pContent->usSubtype = pProcOfflineRes->common.usSubtype;
						pContent->idInfo_logicalPeer.ui64Id = pProcOfflineRes->download.idInfo_logicalPeer.ui64Id;
					}
				}
#endif



				TASK_transferFile task = new TASK_transferFile();
				//memset(&task, 0, sizeof(task));
				task.m_index_taskInfo = iIndex_taskInfo;

				PROC_TASK_FILE  pContent_myTask = task.myTask;



				//  2009/10/20
				pContent_myTask.hWnd_task = m_hWnd;

				//  2007/07/18
				pMsg->uiTaskType = Consts.CONST_imTaskType_sendFile;
				//
				pMsg->idInfo_taskSender.ui64Id = pMisCnt.idInfo.ui64Id;
				pMsg->idInfo_taskReceiver.ui64Id = m_var.addr.idInfo.ui64Id;
				//
				pMsg->iStatus = Consts.CONST_imTaskStatus_waitToSend;


				//  ÕâÀïµÄ¸³ÖµËÆºõÓ¦ÔÚ·¢ËÍÊ±Ê¹ÓÃ, 2007/07/24
				pMsg->data.route.idInfo_from.ui64Id = pMsg->idInfo_taskSender.ui64Id;
				pMsg->data.route.idInfo_to.ui64Id = pMsg->idInfo_taskReceiver.ui64Id;
				//

				lenInBytes = (uint)sizeof(TRANSFER_FILE_REQ);
				pMsg->lenInBytes = (uint)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + lenInBytes;

				rcd = new IM_TASK_RCD();
				if (0!=qyFuncs.msg2TaskRcd(pMisCnt, ref *pMsg, Consts.CONST_qyRc_unknown, ref rcd)) goto errLabel;

				{
					object pDb = null;
					using (CQnmDb db = new CQnmDb())
					{
						;
						//
						if (null==db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
						pDb = db.m_pDbMem.pDb;
						//
						if (qyFuncs.storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, iTaskId_suggested, ref rcd, false,"") < 0) goto errLabel;
					}
				}

				//
				//  2013/02/13
				if (0!=iTaskId_suggested)
				{       //  2014/10/29
					if (rcd.id != iTaskId_suggested)
					{
# if  __DEBUG__
						qyFuncs.traceLog(_T("procSendFile failed, rcd.id != taskInfo.iTaskId"));
#endif
						goto errLabel;
					}
				}

				//
				QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;

				//
				pMsg->iTaskId = rcd.id;
				pMsg->uiTaskMode_unused = rcd.uiMode;
				pMsg->uiChannelType = Consts.CONST_channelType_robot;  //  2009/10/17

				//
				pMsg->addr_logicalPeer.idInfo = idInfo_logicalPeer;

				//
				PARAM_PROC_ROW param=new PARAM_PROC_ROW();
				//qyFuncs.mymemset((IntPtr)(byte*)&param, 0, sizeof(param));
				param.bScrollIntoView = true;
				param.bNewMsg = true;
				if (0!=tmpHandler_showMsg(m_hWnd, param, ref pMsgU)) goto errLabel;

				//MIS_MSGU  *  pMsgU  =  (  MIS_MSGU *)pMsg;
				if (0!=dlgTalk_qPostMsg(m_hWnd, ref pMsgU, (uint)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsg->lenInBytes)) goto errLabel;

				//	
				if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
				{
					pContent->bGrpTask = true; //  2007/09/05, ¸³ÉÏ×é±êÊ¶
				}


				//
				if (0 != pProcInfo.processQ_media.qPostMsg(task, 0))
				{
					qyFuncs.traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
					goto errLabel;
				}



				//  mgr
				uint uiChannelType = Consts.CONST_channelType_robot;
				//
				uiChannelType = 0;
				//
				QY_MESSENGER_ID idInfo_dst = m_var.addr.idInfo;
				if (0!=qyFuncs.postMsgTask2Mgr_mc(null,pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendRobotTask, tStartTran, uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (byte*)pContent, lenInBytes, idInfo_logicalPeer.ui64Id, pMsg->idInfo_taskSender.ui64Id, pMsg->idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;

				//  2015/08/06
				if (null!=piTaskId) *piTaskId = rcd.id;
			}

errLabel:
return 0;
}



}
}

