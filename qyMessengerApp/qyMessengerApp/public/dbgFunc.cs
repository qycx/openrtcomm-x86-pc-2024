using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		/*
		public static int chkTc(QMC_taskData_conf pTc,string hint)
		{
			string str;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();
			string str1 = string.Format("requestMem.length {0}", videoConference.requestingMems_from.Length);

			if (videoConference.usCntLimit_activeMems_from > videoConference.activeMems_from.Length)
			{
				str = string.Format("chkTc 0 {0},usCntLimit {1}, length {2} ", hint, videoConference.usCntLimit_activeMems_from, videoConference.activeMems_from.Length);

				int ii = 0;
			}
			if (videoConference.activeMems_from.Length > Consts.MAX_activeMems_from)
			{
				str = string.Format("chkTc 1 {0},usCntLimit {1}, length {2} ", hint, videoConference.usCntLimit_activeMems_from, videoConference.activeMems_from.Length);

				int ii = 0;
			}
			if (videoConference.activeMems_from.Length == 0)
            {
				str = string.Format("chkTc 2 {0},usCntLimit {1}, length {2} ", hint, videoConference.usCntLimit_activeMems_from, videoConference.activeMems_from.Length);

				int ii = 0;
            }
			if (videoConference.requestingMems_from.Length > 20)
            {
				int ii = 1;
            }
			if (videoConference.requestingMems_from.Length == 0)
            {
				int ii = 0;
            }

			return 0;
		}

		//
		public static unsafe int tmpHandler_chkAllTc(CTX_caller ctxCaller, ref MIS_MSGU p0, PARAM_findOrgMsg p1Param, object taskElemParam)
		{
			string hint = (string)p1Param.hint;
			TASK_common task = (TASK_common)taskElemParam;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, task.m_index_taskInfo, "");
			if (pTaskInfo == null) goto errLabel;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) goto errLabel;

			if (pTaskData.uiType == Consts.CONST_taskDataType_conf)
			{
				QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;


				chkTc(pTc,hint);


			}



		errLabel:
			return 0;

		}



		public static int chkAllTc(string hint)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			if (pQyMc == null) return -1;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (pProcInfo == null) return -1;

			//
			CMIS_MSGU p0 = new CMIS_MSGU();
			PARAM_findOrgMsg param = new PARAM_findOrgMsg();
			param.hint = hint;

			//
			pProcInfo.processQ_media.qTraverse(null, tmpHandler_chkAllTc, ref p0.msgU, param);


			return 0;

		}
		*/


#if false
		public class Param_getTaskStatus
		{
			public int iTaskId;
			public string status;
		}

		public static unsafe int tmpHandler_getTaskStatus(CTX_caller ctxCaller, object p0, object p1Param, object taskElemParam)
		{
			int iErr = -1;
			Param_getTaskStatus param = (Param_getTaskStatus)p0;
			//string hint = (string)p1Param;
			TASK_common taskElem = (TASK_common)taskElemParam;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "");
			if (pTaskInfo == null) goto errLabel;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) goto errLabel;
			if (pTaskData.uiType == Consts.CONST_taskDataType_conf)
			{
				//
				if (pTaskInfo.var.iTaskId == param.iTaskId)
				{
					TASK_transferAv task = (TASK_transferAv)taskElem;
					int i;
					string str = "";
                    for (i = 0; i < task.myTask.usCntLimit_transforms; i++)
                    {
						str += string.Format("{0},", task.myTask.pTransforms[i].video.hThread == null ? "0" : "1");

                    }
					param.status = str;
					iErr = 1; goto errLabel;

				}

			}

			//
			iErr = 0;

			errLabel:
			return iErr;
		}


            //
            public static string getTaskStatus(int iTaskId)
        {
			string str;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			Param_getTaskStatus param = new Param_getTaskStatus();
			param.iTaskId = iTaskId;
			pProcInfo.processQ_media.qTraverse(null, tmpHandler_getTaskStatus, param, null);

			str = param.status;

			return str;
        }
#endif

		/*
		//
#if true //DEBUG
		public static unsafe int tmp_showTc(ref CTX_caller ctxCaller, int iTaskId, string hint)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			if (null == hint) hint = "";

			//
			int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(ctxCaller, pProcInfo, iTaskId);
			QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, index_taskInfo, "tmp_showTc");
			if (pTaskInfo == null) return -1;

			string str;

			//
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			//chkTc(pTc,hint);

			//
			string str1 = "";
#if DEBUG
#endif
			//
			str = string.Format("tmp_showTc called. index_taskInfo {0}, usCntLimit_activeMems {1}, length {2}. requestMems.length {3}， {4} ", index_taskInfo, videoConference.usCntLimit_activeMems_from, videoConference.activeMems_from.Length, videoConference.requestingMems_from.Length,  hint);
			//
			str += str1;
			//
			qyFuncs.showInfo_open(0, null, str);


			//
			str = "Tc: ";

			//
			fixed(MIS_MSG_TASK * pMsgTask = &pTaskData.msgU.task)
            {
				if (pMsgTask->uiType == Consts.CONST_misMsgType_task)
                {
					MIS_MSGU* pMsgU = (MIS_MSGU*)pMsgTask;
					IM_CONTENTU* pContent = M_getMsgContent(pMsgTask->uiType,  pMsgTask->ucFlg, &pMsgTask->data);
					if (pContent != null)
                    {
						if (pContent->uiType == Consts.CONST_imCommType_transferAvInfo)
                        {
							uint tn_a = pContent->transferAvInfo.ass.tranInfo.audio.uiTranNo_openAvDev;
							uint tn_v = pContent->transferAvInfo.ass.tranInfo.video.uiTranNo_openAvDev;
							//
							str += string.Format("a {0},v {1}.", tn_a, tn_v);
                        }
                    }
                }

            }

			//
			int i;
			for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
			{
				DLG_TALK_videoConferenceActiveMemFrom pActiveMem = videoConference.activeMems_from[i];
				//
				if (pActiveMem.avStream.idInfo.ui64Id != 0)
				{
					str += string.Format("act[{0}]: {1} {2} {3}, ", i, pActiveMem.avStream.idInfo.ui64Id, pActiveMem.avStream.obj.tranInfo.audio.uiTranNo_openAvDev,pActiveMem.avStream.obj.tranInfo.video.uiTranNo_openAvDev);
				}
			}
			//
			str += "";// getTaskStatus(iTaskId);
			//
			str += hint;
			//
			//str += string.Format(" gui {0}, mgr {1},v {2},a {3},w {4}", pProcInfo.dwThreadId_gui,  pProcInfo.dwThreadId_mgr, pProcInfo.dwThreadId_media, pProcInfo.dwThreadId_realTimeMedia,pProcInfo.dwThreadId_work);
			//
			mySYSTEMTIME when;
			qyFuncs.myGetLocalTime(out when);
			qyFuncs.showState_open(null,"", str,0,0,ref when,1,0);



			iErr = 0;
		errLabel:
			return iErr;
		}
#endif

		//
		public static int showAllTc(CTX_caller ctxCaller, string hint)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			int i;
			QMC_TASK_INFO pTaskInfo;

			if (hint == null) hint = "";
			string str;

#if true //DEBUG

			traceLog("showAllTc enters");

			//
			for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
			{
				pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, i, "showAllTc");
				if (!pTaskInfo.bUsed) continue;
				QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
				if (pTaskData == null) continue;
				if (pTaskData.uiType != Consts.CONST_taskDataType_conf) continue;
				//
				if (!qyFuncs.qmcTaskInfo_bAlive(ctxCaller, pProcInfo, i)) continue;
				//
				str = string.Format("{0}", hint);
				tmp_showTc(ref ctxCaller, pTaskInfo.var.iTaskId, str);
				//
				continue;
			}

			traceLog("showAllTc leaves");


#endif
			//
			return 0;
		}

*/


	}



}
