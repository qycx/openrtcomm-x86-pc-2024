using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		//public static unsafe bool isMemFromActive(DLG_TALK_videoConference pVc, QY_MESSENGER_ID idInfo_from, RES_obj_simple* pResObj_from, int* pIndex_activeMems_from)
		public static unsafe bool isMemFromActive(QMC_taskData_conf pTc, QY_MESSENGER_ID idInfo_from, ref RES_obj_simple pResObj_from, int* pIndex_activeMems_from)
		{

			int i;

			//DLG_TALK_videoConference* pVc = (DLG_TALK_videoConference*)pDLG_TALK_videoConference;
			QY_MESSENGER_ID* pIdInfo_from = &idInfo_from;

			//  2014/11/30
			if (null == pIdInfo_from || 0 == pIdInfo_from->ui64Id)
			{
				qyFuncs.showInfo_open(0, null, _T("Note: isMemFromActive returns false, for idInfo_from is 0"));
#if __DEBUG__
				traceLog(_T("isMemFromActive failed"));
#endif
				return false;
			}

			RES_obj_simple resObj_from;
			qyFuncs.mymemset((IntPtr)(byte*)&resObj_from, 0, sizeof(RES_obj_simple));
			//if (null != pResObj_from)
			{
				resObj_from = pResObj_from;
			}

			//
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();
			for (i = 0; i < videoConference.activeMems_from.Length; i++)
			{
				DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[i];
				if (pIdInfo_from->ui64Id == pActiveMem_from.avStream.idInfo.ui64Id
					&& resObj_from.uiObjType == pActiveMem_from.avStream.obj.resObj.uiObjType
					&& resObj_from.usIndex_obj == pActiveMem_from.avStream.obj.resObj.usIndex_obj)
				{
					break;
				}
			}
			if (i == videoConference.activeMems_from.Length) return false;

			if (null != pIndex_activeMems_from) *pIndex_activeMems_from = i;

			return true;
		}

		//
		//
		//public static unsafe int addTo_activeMems_from(MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, AV_stream_simple* pAss, DLG_TALK_videoConference pVc)
		public static unsafe int addTo_activeMems_from(MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, AV_stream_simple* pAss, QMC_taskData_conf pTc)
		{
			int iErr = -1;

			if (null == pAss) return -1;

			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			if (isMemFromActive(pTc, pIdInfo, ref pAss->resObj, null)) return 0;
			int i;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from;
			for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
			{
				pActiveMem_from = videoConference.activeMems_from[i];
				if (pActiveMem_from.avStream.idInfo.ui64Id == 0) break;
			}
			if (i == videoConference.usCntLimit_activeMems_from)
			{
				qyFuncs.showInfo_open(0, null, _T("addTo_activeMems_from failed, too many activeMems"));
				return -1;
			}
			pActiveMem_from = videoConference.activeMems_from[i];

			pActiveMem_from.avStream.idInfo = pIdInfo;
			pActiveMem_from.avStream.obj = *pAss;

			//
			fixed (char* pDesc = pActiveMem_from.desc.desc)
			{
				qyFuncs.M_getActiveMemDesc(pMisCnt, pActiveMem_from.avStream.idInfo, pDesc, Consts.cntof_desc);
			}


			iErr = 0;


			return iErr;
		}

		//
		//public static unsafe int removeFrom_activeMems_from(MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, AV_stream_simple* pAss, DLG_TALK_videoConference pVc)
		public static unsafe int removeFrom_activeMems_from(MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, ref AV_stream_simple pAss, QMC_taskData_conf pTc)
		{
			int iErr = -1;

			//if (null == pAss) return -1;

			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			if (!isMemFromActive(pTc, pIdInfo, ref pAss.resObj, null)) return 0;
			int i;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from;
			for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
			{
				pActiveMem_from = videoConference.activeMems_from[i];
				if (pActiveMem_from.avStream.idInfo.ui64Id == pIdInfo.ui64Id
					&& pActiveMem_from.avStream.obj.resObj.uiObjType == pAss.resObj.uiObjType
					&& pActiveMem_from.avStream.obj.resObj.usIndex_obj == pAss.resObj.usIndex_obj) break;
			}
			if (i == videoConference.usCntLimit_activeMems_from)
			{
				qyFuncs.showInfo_open(0, null, _T("addTo_activeMems_from failed, too many activeMems"));
				return -1;
			}
			pActiveMem_from = videoConference.activeMems_from[i];

			//memset(pActiveMem_from, 0, sizeof(pActiveMem_from[0]));
			pActiveMem_from.memset0();

			iErr = 0;


			return iErr;
		}


		//
		//
		public static unsafe bool isActiveMemsFull(CTX_caller ctxCaller,int iTaskId,bool bSpeaker)
		{
			bool bRet = false;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			int index_taskInfo = getQmcTaskInfoIndexBySth(ctxCaller,pProcInfo, iTaskId);
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,pProcInfo, index_taskInfo,"isActiveMemsFull");
			if (null==pTaskInfo) goto errLabel;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskInfo.getTaskData();
			if (pTc.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			int i;
			string str;


			//					  
			if (!bSpeaker)
			{
				//
				for (i = 0; i < videoConference.activeMems_from.Length; i++)
				{
					if (0==videoConference.activeMems_from[i].avStream.idInfo.ui64Id) break;
				}
				if (i < videoConference.activeMems_from.Length)
				{
					goto errLabel;
				}

				//
			}
			else
			{
				//
				int nSpeakers = 0;
				//
				for (i = 0; i < videoConference.activeMems_from.Length; i++)
				{
					if (0!=videoConference.activeMems_from[i].avStream.idInfo.ui64Id
						&& 0==videoConference.activeMems_from[i].avStream.obj.resObj.uiObjType)
					{
						nSpeakers++;
					}
				}

				//
				if (videoConference.usMaxSpeakers > 0)
				{
					if (nSpeakers < videoConference.usMaxSpeakers) goto errLabel;
				}

				//
			}

			//					  	
			bRet = true;

		errLabel:
			return bRet;

		}

		//
		public static unsafe bool isActiveMem(CTX_caller ctxCaller, int iTaskId, QY_MESSENGER_ID idInfo)
		{
			bool bRet = false;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			if (idInfo.ui64Id == 0) return false;

			//
			int index_taskInfo = getQmcTaskInfoIndexBySth(ctxCaller, pProcInfo, iTaskId);
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, index_taskInfo, "isActiveMemsFull");
			if (null == pTaskInfo) goto errLabel;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskInfo.getTaskData();
			if (pTc.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			int i;
			string str;



			//					  
			
				//
				for (i = 0; i < videoConference.activeMems_from.Length; i++)
				{
					if (idInfo.ui64Id== videoConference.activeMems_from[i].avStream.idInfo.ui64Id
						&& 0 == videoConference.activeMems_from[i].avStream.obj.resObj.uiObjType)
					{
						break;
					}
				}

				if (i == videoConference.activeMems_from.Length) goto errLabel;


				//
			

			//					  	
			bRet = true;

		errLabel:
			return bRet;

		}



	}
}
