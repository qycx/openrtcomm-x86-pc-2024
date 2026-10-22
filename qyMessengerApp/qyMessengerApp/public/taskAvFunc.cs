using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe TASK_AV_FROM getTaskAvFrom(QY_TRANSFORM pTransform, int index_pMems_from, int index_activeMems_from, string hint)
		{
			TASK_AV_FROM pFrom = null;
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return null;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();

			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,pProcInfo, pTask.iIndex_taskInfo,"getTaskAvFrom");
			if (null==pTaskInfo) return null;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return null;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return null;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;


			//
			if (null==hint) hint = _T("");
#if false
			//
			if (pTransform.bMosaicVideo)
			{
				pFrom = pTask.confMosaicTrans.confMosaic_video.taskAvFrom;
			}
			else if (pTransform.bMosaicResource)
			{
				pFrom = pTask.confMosaicTrans.confMosaic_resource.taskAvFrom;
			}
			else
			{
				if (index_pMems_from < 0 || index_pMems_from >= pTc.videoConference.usCntLimit_mems_from)
				{
#if __DEBUG__
					traceLog(_T("getTaskAvFrom failed, index_pMems_from err, %s"), hint);
#endif
					return null;
				}
				//
				if (index_activeMems_from != pTransform.index_activeMems_from)
				{
#if __DEBUG__
					traceLog(_T("getTaskAvFrom failed, index_activeMems_from unmatched, %s"), hint);
#endif
					return null;
				}
				//
				if (pTask.ucbVideoConferenceStarter)
				{
					if (index_activeMems_from < 0 || index_activeMems_from >= pTc.videoConference.usCntLimit_activeMems_from)
					{
#if __DEBUG__
						traceLog(_T("getTaskAvFrom failed, index_activeMems_from err, %s"), hint);
#endif
						return null;
					}
				}
				//
				pFrom = pTc.videoConference.pMems_from[index_pMems_from];
			}
#endif

			return pFrom;
		}


		public static unsafe TASK_AV_FROM getTaskAvFrom(ref PROC_TASK_AV pTask, int index_pMems_from, string hint)
	{
		TASK_AV_FROM pFrom = null;


			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));


			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,pProcInfo, pTask.iIndex_taskInfo,"getTaskAvFrom");
			if (null == pTaskInfo) return null;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (null == pTaskData) return null;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return null;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;


			//
			if (null==hint) hint = _T("");

#if false
			//
			{
			if (index_pMems_from < 0 || index_pMems_from >= pTc.videoConference.usCntLimit_mems_from)
			{
#if __DEBUG__
				traceLog(_T("getTaskAvFrom failed, index_pMems_from err, %s"), hint);
#endif
				return null;
			}
			//
			pFrom = pTc.videoConference.pMems_from[index_pMems_from];
		}
#endif

		return pFrom;
	}

}
}
