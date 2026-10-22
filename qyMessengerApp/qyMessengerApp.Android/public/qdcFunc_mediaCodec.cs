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
		//  mediaSdk的处理和其他的解压缩不同，是一直自己循环。为了避免从其他的解压缩处理中间跳到这里，需要先exitTaskVideo。清理一下
		public static unsafe int doDecodeVideo_mediaCodec(QY_TRANSFORM pTransform, int index_activeMems_from, IntPtr pMsgBufParam_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;

			bool bDbg = false;
			string str;

			//
			MIS_MSGU* pMsgBuf_unused = (MIS_MSGU*)pMsgBufParam_unused;

			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null==pCusModules)
			{
# if  __DEBUG__
				traceLog(_T("doDecV_msdk failed, pCusModules is null"));
#endif
				return -1;
			}
			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			//if (null==pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			//AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			//if (!pTranInfo_unused) return -1;
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return -1;

			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null,(CCtxQmc)pProcInfo, pTask.iIndex_taskInfo,"doDecodeVideo_mediaCodec");
			if (null==pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];
			//
			qyFuncs.exitTaskAvFrom_video(pProcInfo, pTransform,"doDecodeVideo_mediaCodec before init");


			qyFuncs.M_setMyStep(ref pTransform.video.debugStep, 21);

			//  if  (  !pFrom->video.ucbInited  )  
			if (!pTransform.video.ucbInited)
			{
				uint uiModuleType_suggested = Consts.CONST_moduleType_mediaSdk_dec;
				bool bUseDecTool = false;
				if (0!=qyFuncs.initTaskAvFrom_video(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform.iTaskId, uiModuleType_suggested, bUseDecTool, pTransform)) goto errLabel;
				//
				str=string.Format("procTaskVideo: initTaskAvFrom_video {0}", index_activeMems_from);
				qyFuncs.showInfo_open(0, null, str);
			}
			if (!pTransform.video.ucbInited)
			{
				qyFuncs.traceLogA("procTaskVideo failed: video is not inited");
				goto errLabel;
			}

			//
			if (pTransform.video.uiModuleType_toDec != Consts.CONST_moduleType_mediaCodec)
			{
# if  __DEBUG__
				traceLog(_T("doTransformVideo_mediaSdk failed, uiModuleType_toDec is not mediaSdk_dec"));
#endif
				goto errLabel;
			}

			//
			CUS_MODULE_compress pModule = null;
			qyFuncs.M_getCompressModule(pCusModules, pTransform.video.uiModuleType_toDec, ref pModule);
			if (pModule == null) goto errLabel;

			//
			if (null==pModule.pf_qdcDecompressVideoRun)
			{
				goto errLabel;
			}

			try  {
				if (0!=pModule.pf_qdcDecompressVideoRun(ref pTransform.video.u_qoi.myQoi.common)) goto errLabel;
			}
			catch(Exception e)  {
# if  __DEBUG__
				traceLog(_T("qdcDecompressVideoRun exception"));
#endif
				goto errLabel;
			}


			iErr = 0;
		errLabel:
			qyFuncs.traceLog("TAG", "exitTaskAvFrom_video");
			//  2015/01/17
#if false
			if (pTransform.video.debugStep.dec.sts_SyncOperation324 == -17)
			{
				qyFuncs.showNotification_open(0, 0, 0, _T("Note: SyncOperation returns DEVICE_FAILED"));
			}
#endif

			//
			qyFuncs.exitTaskAvFrom_video(pProcInfo, pTransform,"doDecodeVideo_mediaCodec quit");

			return iErr;
		}


		//
		//
		public static unsafe int doEncodeVideo_mediaCodec(CAP_procInfo_bmpCommon pCapProcInfo, MIS_CNT pMisCnt, IntPtr pMsgBufParam)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			MIS_MSGU* pMsgBuf = (MIS_MSGU*)pMsgBufParam;

			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null == pCusModules)
			{
#if __DEBUG__
				traceLog(_T("doDecV_msdk failed, pCusModules is null"));
#endif
				return -1;
			}


			//
#if false
			switch (pCapProcInfo->compressVideo.compressor.common.ucCompressors)
			{
				case CONST_videoCompressors_ipp:
				case CONST_videoCompressors_hwAccl:
					//case  CONST_videoCompressors_cuda:
					break;
				default:
#if __DEBUG__
					traceLog(_T("doEncodeVideo_mediaSdk failed, uiModuleType_toDec is not mediaSdk_dec"));
#endif
					goto errLabel;
					break;
			}
#endif

			//
			CUS_MODULE_compress pModule = null;
			qyFuncs.M_getCompressModule(pCusModules, Consts.CONST_moduleType_mediaCodec, ref pModule);
			if (pModule == null) goto errLabel;

			if (null==pModule.pf_qdcCompressVideoRun)
			{
				goto errLabel;
			}

			try  {
				if (0!=pModule.pf_qdcCompressVideoRun(ref pCapProcInfo.compressVideo.u_qoi.myQoi.common)) goto errLabel;
			}
			catch(Exception e)
			{
# if  __DEBUG__
				traceLog(_T("doEncodeVideo_mediaSdk exception"));
#endif
				goto errLabel;
			}

			iErr = 0;

		errLabel:
			return iErr;
		}


		//
		public static unsafe int doDecodeAudio_mediaCodec(QY_TRANSFORM pTransform, int index_pMems_from1, int index_activeMems_from, IntPtr pMsgBufParam_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
			if (null == pProcInfo) return -1;

			string str;


			MIS_MSGU* pMsgBuf_unused = (MIS_MSGU*)pMsgBufParam_unused;

			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null == pCusModules)
			{
#if __DEBUG__
				traceLog(_T("doDecV_msdk failed, pCusModules is null"));
#endif
				return -1;
			}
			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			//if (null == pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			/*
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null == pTranInfo_unused) return -1;
			*/
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null == pTask) return -1;

			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null, (CCtxQmc)pProcInfo, pTask.iIndex_taskInfo, "");
			if (null == pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (null == pTaskData) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];
			//
			qyFuncs.exitTaskAvFrom_audio(pProcInfo, pTransform,"doDecodeAudio_mediaCodec enters");


			qyFuncs.M_setMyStep(ref pTransform.audio.debugStep, 21);

			//  if  (  !pFrom->video.ucbInited  )  
			if (!pTransform.audio.ucbInited)
			{
				uint uiModuleType_suggested = Consts.CONST_moduleType_mediaCodec;
				bool bUseDecTool = false;
				if (0!=qyFuncs.initTaskAvFrom_audio(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform)) goto errLabel;
				//
				str=string.Format("procTaskVideo: initTaskAvFrom_audio %d", index_activeMems_from);
				qyFuncs.showInfo_open(0, null, str);
			}
			if (!pTransform.audio.ucbInited)
			{
				qyFuncs.traceLogA("procTaskVideo failed: video is not inited");
				goto errLabel;
			}

			//
			if (pTransform.audio.uiModuleType_toDec != Consts.CONST_moduleType_mediaCodec)
			{
# if  __DEBUG__
				traceLog(_T("doTransformVideo_mediaSdk failed, uiModuleType_toDec is not mediaSdk_dec"));
#endif
				goto errLabel;
			}

			//
			CUS_MODULE_compress pModule = null;
			qyFuncs.M_getCompressModule(pCusModules, pTransform.audio.uiModuleType_toDec, ref pModule);
			if (pModule == null) goto errLabel;

			//
			if (null == pModule.pf_qdcDecompressAudioRun)
			{
				goto errLabel;
			}


			try  {
				if (0!=pModule.pf_qdcDecompressAudioRun(ref pTransform.audio.u_qoi.myQoi.common)) goto errLabel;
			}
			catch(Exception e){
# if  __DEBUG__
				traceLog(_T("qdcDecompressVideoRun exception"));
#endif
				goto errLabel;
			}


			iErr = 0;
		errLabel:

			//  2015/01/17

			//
			qyFuncs.exitTaskAvFrom_audio(pProcInfo, pTransform,"doDecodeAudio_mediaCodec leaves");

			//
			return iErr;
		}



	}
}


