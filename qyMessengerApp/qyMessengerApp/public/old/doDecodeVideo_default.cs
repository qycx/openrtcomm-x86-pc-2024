using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

#if false
		public static unsafe int doDecodeVideo_default(QY_TRANSFORM pTransform, int index_pMems_from, int index_activeMems_from, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );

			MSGR_ADDR* pAddr = pTransform.pAddr_logicalPeer;
			if (null==pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null==pTranInfo_unused) return -1;
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return -1;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,pProcInfo, pTask.iIndex_taskInfo,"doDecodeVideo_default");
			if (null==pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


			//  2014/05/08
			//_sntprintf(pTransform.video.debugStep.showInfo.tWhere, mycountof(pTransform.video.debugStep.showInfo.tWhere), _T("tt=%d"), iTaskId);
			pTransform.video.debugStep.showInfo.uiStep = 0;

			//
			M_setMyStep(ref pTransform.video.debugStep, 21);

			//  if  (  !pFrom->video.ucbInited  )  
			if (!pTransform.video.ucbInited)
			{
				if (0!=initTaskAvFrom_video(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform.iTaskId, 0, false, pTransform)) goto errLabel;
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("procTaskVideo: initTaskAvFrom_video %d"), index_pMems_from);
			}
			if (!pTransform.video.ucbInited)
			{
				traceLogA("procTaskVideo failed: video is not inited");
				goto errLabel;
			}

			int old_pktId = 0;
			//
			for (; ; )
			{
				if (bTransformVideoQuit(pTransform, null, null))
				{
#if __DEBUG__
					traceLog(_T("doTrans_default failed：quit is true"));
#endif
					goto errLabel;
				}
				//
				M_setMyStep(ref pTransform.video.debugStep, 100);  //  2014/07/17
																	//
				for (; ; )
				{
					if (bTransformVideoQuit(pTransform, null, null)) goto errLabel;
					//
					M_setMyStep(ref pTransform.video.debugStep, 105);  //  2014/07/17
																		//
					if (0==getNextVPkt((IntPtr)pTransform.video.pCurPkt, pTransform.video.pInDataCacheQ, pTransform.video.q2)) break;
					//
					M_setMyStep(ref pTransform.video.debugStep, 106);  //  2014/07/17
																		//
					mySleep(100);
				}
				//
				M_setMyStep(ref pTransform.video.debugStep, 108);  //  2014/07/17
																	//
				byte* pInput = null;
				uint uiInputSize = 0;
				uint uiSampleTimeInMs = 0;
				uint uiPts = 0;    //  2015/02/09
											//  2015/02/24
				SMPL_bsRead_param param;
				mymemset((IntPtr)(byte*)&param, 0, sizeof(SMPL_bsRead_param));
				//
				if (0!=old_dec_parseEncVPkt_i(Consts.CONST_uiTransformType_dec, pTransform, pTransform.video.pCurPkt, &old_pktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts, &param))
				{
#if __DEBUG__
					traceLog(_T("doDecodeVideo_default failed, parsePkt failed"));
#endif
					goto errLabel;
				}

				//			  	
				M_setMyStep(ref pTransform.video.debugStep, 109);  //  2014/07/17


				//
				myDRAW_VIDEO_DATA* pPkt = pTransform.video.pCurPkt;

				doDecVideoPkt(pTransform, pPkt, pMsgBuf_unused);

				//
				M_setMyStep(ref pTransform.video.debugStep, 110);  //  2014/07/17


				//  2014/08/11
				if (pTransform.video.status.ucbDecFailed)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("doDecodeVideo_default: decFailed, need to reinited. %d"), index_pMems_from);
					goto errLabel;
				}

				//  2014/04/07
				if (pTransform.video.status.ucbNeedReinited)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("doDecodeVideo_default: need to reinited. %d"), index_pMems_from);
					goto errLabel;
				}

				//
				bool bPktsRedirected = false;
				//
				postVPktToRender(pTransform, pPkt, &bPktsRedirected);
				if (bPktsRedirected)
				{
					detachVPkt(pPkt, null);
				}
				else
				{
					clean_myDRAW_VIDEO_DATA((IntPtr)pTransform.video.pCurPkt, _T(""));
				}

				//
				continue;
			}


			iErr = 0;
		errLabel:

			exitTaskAvFrom_video(pProcInfo, pTransform,"deDocdeVideo_default");

			return iErr;
		}
#endif

	}
}
