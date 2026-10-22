using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		//
		public static unsafe int old_procTaskAvReply(CCtxQmc pProcInfo, ref MSGR_ADDR pAddr_logicalPeer, int iTaskId, ref AV_TRAN_INFO pAvTran, ref PROC_TASK_AV pTask, ref MIS_MSG_TASK pMsgTask)
		{
			int iErr = -1;
			int index = 0;
			ref IM_CONTENTU pContent = ref pMsgTask.data.buf;
			//
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			/*

			//  2015/02/16
# ifdef  __DEBUG__
#if  0 
			traceLog(  _T(  "TEST: procTaskAvReply is not valid now"  )  );
			showInfo_open(  0,  0,  _T(  "TEST: procTaskAvReply is not valid now"  )  );
			return  -1;
#endif
#endif


			//
			if (pContent->uiType != CONST_imCommType_transferAvReplyInfo) return -1;
			//
			if (pContent->transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev && pAvTran->audio.uiTranNo_openAvDev)
			{

			}

			//
			pMsgTask->addr_logicalPeer.idInfo.ui64Id = pAddr_logicalPeer->idInfo.ui64Id;

			//  2015/02/17
			BOOL bNewFrom = FALSE;

			//
			for (index = 0; index < pTask.usCntLimit_mems_from; index++)
			{
				if (pTask.pMems_from[index].idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) break;
			}
			if (index < pTask.usCntLimit_mems_from)
			{   //  ÒÑ¾­ÓÐÁË							
				if (pTask.pMems_from[index].video.tv_recvd.uiTranNo_openAvDev == pContent->transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev
					&& pTask.pMems_from[index].audio.ta_recvd.uiTranNo_openAvDev == pContent->transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev)
				{
# ifdef  __DEBUG__
					traceLogA("findOrgMsgAndProcess_mis .");
#endif
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply"), _T(""), _T(" %I64u, mem_from'av matched, OK"), pMsgTask->data.route.idInfo_from.ui64Id);

					iErr = 0; goto errLabel;
				}

				//
				//  stopTransformThreads(  pTask  );
				//  
				//  exitTaskAvFrom_video(  pProcInfo,  &pTask.pMems_from[index]  );
				//  exitTaskAvFrom_audio(  pProcInfo,  &pTask.pMems_from[index]  );

				qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply"), _T(""), _T(" %I64u, mem_from'av not matched, will fix it"), pMsgTask->data.route.idInfo_from.ui64Id);
			}
			else
			{
				//  ÏÂÃæÕÒ¸ö¿ÕµÄÎ»ÖÃ
				for (index = 0; index < pTask.usCntLimit_mems_from; index++)
				{
					if (!pTask.pMems_from[index].idInfo.ui64Id) break;
				}
				if (index == pTask.usCntLimit_mems_from)
				{
# ifdef  __DEBUG__
					traceLogA("myTask");
#endif
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply"), _T(""), _T(" %I64u, no free index in mems_from, failed."), pMsgTask->data.route.idInfo_from.ui64Id);

					iErr = 0; goto errLabel;
				}

				//  2015/02/18
				bNewFrom = true;
			}

			//  2011/07/03
			//  stopTransformThreads(  pTask  );

			//  2014/11/12
			TASK_AV_FROM* pFrom = &pTask.pMems_from[index];

			{
				CQySyncObj syncObj;
				TCHAR mutexName[128];
				//  2011/07/03
				//  _sntprintf(  mutexName,  mycountof(  mutexName  ),  _T(  "%s%d"  ),  pProcInfo->cfg.mutexName_syncActiveMem_prefix,  iTaskId  );
				M_getMutexName_syncActiveMem(&pProcInfo->cfg, iTaskId, mutexName, mycountof(mutexName));
				uint uiMilliSeconds = 1000;
				//  2015/02/17. 这是个快速处理的函数，不能允许等待
				uiMilliSeconds = 0;
				//
				if (syncObj.sync(mutexName, uiMilliSeconds))
				{
					showInfo_open(0, 0, _T("procTaskAvReply failed, sync failed"));
					goto errLabel;
				}

				//  2015/02/18
				if (bNewFrom)
				{
					if (pFrom->idInfo.ui64Id)
					{
						showInfo_open(0, 0, _T("procTaskAvReply failed, from is not a new one"));
						goto errLabel;
					}
				}

				//
				pFrom->idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
				pFrom->tStartTime_task = pMsgTask->tStartTime;                  //  Æô¶¯Ê±¼ä
																				//
				pFrom->video.tv_recvd.uiTranNo_openAvDev = pContent->transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev;                         //  ½ÓÊÕÊý¾ÝÊ±ÒªÓÃ
																																					//
#if  0	//  2014/11/07
		 pFrom->video.usCapDriverIndex  =  0;  //  2014/11/07. pContent->transferAvReplyInfo.tranInfo.video.usCapDriverIndex;
#endif

				//  safeStrnCpy(  pContent->transferAvReplyInfo.tranInfo.video.fourccStr,  pTransferAvInfoElem->myTask.pMems_from[index].video.fourccStr,  mycountof(  pTransferAvInfoElem->myTask.pMems_from[index].video.fourccStr  )  );
				memcpy(&pFrom->video.tv_recvd.compressor, &pContent->transferAvReplyInfo.tranInfo.video.compressor, sizeof(pFrom->video.tv_recvd.compressor)  );

				memcpy(&pFrom->video.tv_recvd.vh_compress, &pContent->transferAvReplyInfo.tranInfo.video.vh_compress, sizeof(pFrom->video.tv_recvd.vh_compress)  );
				//  2014/03/31
				memcpy(&pFrom->video.tv_recvd.vh_stream, &pContent->transferAvReplyInfo.tranInfo.video.vh_stream, sizeof(pFrom->video.tv_recvd.vh_stream)  );
				//
				if (bSupported_sendVhDec())
				{
					memcpy(&pFrom->video.tv_recvd.vh_decompress, &pContent->transferAvReplyInfo.tranInfo.video.vh_decompress, sizeof(pFrom->video.tv_recvd.vh_decompress)  );
				}
				//
				pFrom->audio.ta_recvd.uiTranNo_openAvDev = pContent->transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev;
				//
				memcpy(&pFrom->audio.ta_recvd.compressor, &pContent->transferAvReplyInfo.tranInfo.audio.compressor, sizeof(pFrom->audio.ta_recvd.compressor)  );
				memcpy(&pFrom->audio.ta_recvd.ah_compress, &pContent->transferAvReplyInfo.tranInfo.audio.ah_compress, sizeof(pFrom->audio.ta_recvd.ah_compress)  );
				memcpy(&pFrom->audio.ta_recvd.ah_decompress, &pContent->transferAvReplyInfo.tranInfo.audio.ah_decompress, sizeof(pFrom->audio.ta_recvd.ah_decompress)  );

				//  2014/09/04
				//pFrom->video.dynBmp.usIndex_obj  =  pContent->transferAvReplyInfo.dynBmp.usIndex_obj;
				pFrom->viewDynBmp_reply.ucbSaveVideo_reply = pContent->transferAvReplyInfo.viewDynBmpReply.ucbSaveVideo;

				//  2015/10/23
				pFrom->taskAvProps = pContent->transferAvReplyInfo.tranInfo.taskAvProps;
			}
			//
			
			//  2009/02/22
			pTask.uiTranNo_lastModified_pMems_from = getuiNextTranNo(0, 0, 0);

			//
			TCHAR tBuf[128];
			_sntprintf(tBuf, mycountof(tBuf), _T(" %I64u, index_pMems_from %d, avReply OK."), pMsgTask->data.route.idInfo_from.ui64Id, index);
			if (pFrom->viewDynBmp_reply.ucbSaveVideo_reply)
			{
				_sntprintf(tBuf, mycountof(tBuf), _T("%s save"), tBuf);
			}
			qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply:"), _T(""), _T(" %s"), tBuf);

			//  2012/04/01
			int i;
			for (i = 0; i < pTask.usCntLimit_activeMems_from; i++)
			{
				if (pTask.activeMems_from[i].idInfo.ui64Id == pFrom->idInfo.ui64Id)
				{       //  当已经申请发言的人中途退出又进入时,需要重新刷新话筒参数
					PostMessage(pTask.hWnd_task, CONST_qyWm_postComm, CONST_qyWmParam_bNeedRefresh_activeMems_from, iTaskId);
				}
			}

			//  2010/07/19
			int iStatus = pContent->transferAvReplyInfo.tranInfo.audio.ah_compress.wfx.wFormatTag ? CONST_imTaskStatus_dualByReceiver : CONST_imTaskStatus_acceptedByReceiver;
			showTaskStatus(pMsgTask->pMisCnt, &pAddr_logicalPeer->idInfo, &pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, pTask.ucbVideoConference, CONST_imCommType_transferAvInfo, iTaskId, iStatus, 0, 0, 0, 0);


			//
			OutputDebugString(_T("procTaskAvReply ok\n"));

			//
			iErr = 0;

		errLabel:

			if (!iErr)
			{
				//  2011/07/03

				//  2017/09/10
				TASK_respInfo* pRespInfo = get_respInfo(pProcInfo, pTask.iIndex_sharedObj, pTask.iIndex_sharedObjUsr);
				if (pRespInfo)
				{
					TASK_respInfo_mem* pRespInfoMem = get_respInfoMem(pProcInfo, pRespInfo, &pMsgTask->data.route.idInfo_from, pTask.uiTaskType);
					if (pRespInfoMem)
					{
						pRespInfoMem->var.avReply.tRecvTime_lastResp = pMsgTask->tRecvTime;
						//
						time_t tNow; time(&tNow);
						unsigned  short usState = get_respInfoMem_state(pMisCnt, tNow, pRespInfoMem, NULL);
						//
						if (pRespInfoMem->var.usState_byDlgTalk != usState)
						{
							pRespInfoMem->var.bNeedCalc_byProcTaskDataResp = true;
							pRespInfo->bNeedCalc_byProcTaskDataResp = true;
						}
#if  0
				 time_t  t;  time(&t  );
				 int  iDiff  =  t  -  pMsgTask->tRecvTime;
				 int  ii  =  0;
#endif
					}
				}
			}
		*/

			return iErr;
		}



		//
		public static unsafe int procTaskAvReply(CCtxQmc pProcInfo,ref  MSGR_ADDR pAddr_logicalPeer, int iTaskId, ref AV_TRAN_INFO pAvTran, ref PROC_TASK_AV pTask, ref MIS_MSG_INPUT pMsgInput)
		{
			int iErr = -1;

			bool bDbg = false;
			string str;

			//
			int index = 0;
			ref IM_CONTENTU pContent = ref pMsgInput.data.buf;
			//
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,pProcInfo, pTask.iIndex_taskInfo,"procTaskAvReply");
			if (null==pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from;


			//  2015/02/16
#if __DEBUG__
#endif


			//
			if (pContent.uiType != Consts.CONST_imCommType_transferAvReplyInfo) return -1;
			//
			if (0!=pContent.transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev && 0!=pAvTran.audio.uiTranNo_openAvDev)
			{

			}

			//
			pMsgInput.addr_logicalPeer.idInfo.ui64Id = pAddr_logicalPeer.idInfo.ui64Id;

			//
			pMsgInput.iTaskId = iTaskId;

			//
			if (!pTask.bVideoConference())
			{
				int i;
				for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
				{
					if (videoConference.activeMems_from[i].avStream.idInfo.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
						&& videoConference.activeMems_from[i].avStream.obj.resObj.uiObjType == 0)
					{
						break;
					}
				}

				//
				if (i < videoConference.usCntLimit_activeMems_from)
				{			
					pActiveMem_from = videoConference.activeMems_from[i];
					if (pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev == pContent.transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev
						&& pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev == pContent.transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev)
					{

#if __DEBUG__
						traceLogA("findOrgMsgAndProcess_mis .");
#endif
						str=string.Format("procTaskAvReply: {0}, mem_from'av matched, OK", pMsgInput.data.route.idInfo_from.ui64Id);
						showInfo_open(0, null, str);

						//
						iErr = 0; goto errLabel;
					}
					//
					pActiveMem_from.avStream.obj.tranInfo = pContent.transferAvReplyInfo.tranInfo;
					//
					str=string.Format("procTaskAvReply: {0}, OK", pMsgInput.data.route.idInfo_from.ui64Id);
					showInfo_open(0, null, str);

					//
					iErr = 0; goto errLabel;

				}
				for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
				{
					if (0==videoConference.activeMems_from[i].avStream.idInfo.ui64Id) break;
				}
				if (i == videoConference.usCntLimit_activeMems_from)
				{
					str  =  string.Format("procTaskAvReply: {0}, no free index in activeMems_from, failed.", pMsgInput.data.route.idInfo_from.ui64Id);
					showInfo_open(0, null, str);

					//
					iErr = 0; goto errLabel;
				}

				//DLG_TALK_videoConferenceActiveMemFrom*
				pActiveMem_from = videoConference.activeMems_from[i];
				pActiveMem_from.avStream.idInfo = pMsgInput.data.route.idInfo_from;
				pActiveMem_from.avStream.obj.tranInfo = pContent.transferAvReplyInfo.tranInfo;
				//
				fixed (char* pDesc = pActiveMem_from.desc.desc)
				{
					M_getActiveMemDesc(pMisCnt, pActiveMem_from.avStream.idInfo, pDesc, Consts.cntof_desc);
				}
				//		
				str=string.Format("procTaskAvReply: {0}, OK", pMsgInput.data.route.idInfo_from.ui64Id);
				showInfo_open(0, null, str);

				//
				iErr = 0; goto errLabel;
			}



#if false
			//  2015/02/17
			bool bNewFrom = false;

			//
			for (index = 0; index < pTc.videoConference.usCntLimit_mems_from; index++)
			{
				if (pTc.videoConference.pMems_from[index].idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) break;
			}
			if (index < pTc.videoConference.usCntLimit_mems_from)
			{   //  ÒÑ¾­ÓÐÁË							
				if (pTc.videoConference.pMems_from[index].video.tv_recvd.uiTranNo_openAvDev == pContent->transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev
					&& pTc.videoConference.pMems_from[index].audio.ta_recvd.uiTranNo_openAvDev == pContent->transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev)
				{
#if __DEBUG__
					traceLogA("findOrgMsgAndProcess_mis .");
#endif
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply"), _T(""), _T(" %I64u, mem_from'av matched, OK"), pMsgTask->data.route.idInfo_from.ui64Id);

					iErr = 0; goto errLabel;
				}

				//
				//  stopTransformThreads(  pTask  );
				//  
				//  exitTaskAvFrom_video(  pProcInfo,  &pTask.pMems_from[index]  );
				//  exitTaskAvFrom_audio(  pProcInfo,  &pTask.pMems_from[index]  );

				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply"), _T(""), _T(" %I64u, mem_from'av not matched, will fix it"), pMsgTask->data.route.idInfo_from.ui64Id);
			}
			else
			{
				//  ÏÂÃæÕÒ¸ö¿ÕµÄÎ»ÖÃ
				for (index = 0; index < pTc.videoConference.usCntLimit_mems_from; index++)
				{
					if (0==pTc.videoConference.pMems_from[index].idInfo.ui64Id) break;
				}
				if (index == pTc.videoConference.usCntLimit_mems_from)
				{
#if __DEBUG__
					traceLogA("myTask");
#endif
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply"), _T(""), _T(" %I64u, no free index in mems_from, failed."), pMsgTask->data.route.idInfo_from.ui64Id);

					iErr = 0; goto errLabel;
				}

				//  2015/02/18
				bNewFrom = true;
			}

			//  2011/07/03
			//  stopTransformThreads(  pTask  );

			//  2014/11/12
			TASK_AV_FROM pFrom = pTc.videoConference.pMems_from[index];

			//
			using (CQySyncObj syncObj = new CQySyncObj()) 
			{
				TMP_tBuf128 mutexName;
				int cntof_mutexName = Consts.cntof_tBuf128;
				//TCHAR mutexName[128];
				//  2011/07/03
				//  _sntprintf(  mutexName,  mycountof(  mutexName  ),  _T(  "%s%d"  ),  pProcInfo->cfg.mutexName_syncActiveMem_prefix,  iTaskId  );
				M_getMutexName_syncActiveMem(ref pProcInfo.cfg, iTaskId, mutexName.tBuf, cntof_mutexName);
				uint uiMilliSeconds = 1000;
				//  2015/02/17. 这是个快速处理的函数，不能允许等待
				uiMilliSeconds = 0;
				//
				if (0!=syncObj.sync(new string(mutexName.tBuf), pMisCnt.hMutex_syncActiveMem, uiMilliSeconds))
				{
					showInfo_open(0, null, _T("procTaskAvReply failed, sync failed"));
					goto errLabel;
				}

				//  2015/02/18
				if (bNewFrom)
				{
					if (0!=pFrom.idInfo.ui64Id)
					{
						showInfo_open(0, null, _T("procTaskAvReply failed, from is not a new one"));
						goto errLabel;
					}
				}

				//
				pFrom.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
				pFrom.tStartTime_task = pMsgTask->tStartTime;                  //  Æô¶¯Ê±¼ä
																				//
				pFrom.video.tv_recvd.uiTranNo_openAvDev = pContent->transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev;                         //  ½ÓÊÕÊý¾ÝÊ±ÒªÓÃ
																																				   //

				//  safeStrnCpy(  pContent->transferAvReplyInfo.tranInfo.video.fourccStr,  pTransferAvInfoElem->myTask.pMems_from[index].video.fourccStr,  mycountof(  pTransferAvInfoElem->myTask.pMems_from[index].video.fourccStr  )  );
				//memcpy(&pFrom.video.tv_recvd.compressor, &pContent->transferAvReplyInfo.tranInfo.video.compressor, sizeof(pFrom.video.tv_recvd.compressor)  );
				pFrom.video.tv_recvd.compressor = pContent->transferAvReplyInfo.tranInfo.video.compressor;

				//memcpy(&pFrom.video.tv_recvd.vh_compress, &pContent->transferAvReplyInfo.tranInfo.video.vh_compress, sizeof(pFrom.video.tv_recvd.vh_compress)  );
				pFrom.video.tv_recvd.vh_compress=pContent->transferAvReplyInfo.tranInfo.video.vh_compress;
				//  2014/03/31
				//memcpy(&pFrom.video.tv_recvd.vh_stream, &pContent->transferAvReplyInfo.tranInfo.video.vh_stream, sizeof(pFrom.video.tv_recvd.vh_stream)  );
				pFrom.video.tv_recvd.vh_stream=pContent->transferAvReplyInfo.tranInfo.video.vh_stream;
				//
				if (bSupported_sendVhDec())
				{
					//memcpy(&pFrom.video.tv_recvd.vh_decompress, &pContent->transferAvReplyInfo.tranInfo.video.vh_decompress, sizeof(pFrom.video.tv_recvd.vh_decompress)  );
					pFrom.video.tv_recvd.vh_decompress=pContent->transferAvReplyInfo.tranInfo.video.vh_decompress;
				}
				//
				pFrom.audio.ta_recvd.uiTranNo_openAvDev = pContent->transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev;
				//
				//memcpy(&pFrom.audio.ta_recvd.compressor, &pContent->transferAvReplyInfo.tranInfo.audio.compressor, sizeof(pFrom.audio.ta_recvd.compressor)  );
				pFrom.audio.ta_recvd.compressor = pContent->transferAvReplyInfo.tranInfo.audio.compressor;

				//memcpy(&pFrom.audio.ta_recvd.ah_compress, &pContent->transferAvReplyInfo.tranInfo.audio.ah_compress, sizeof(pFrom.audio.ta_recvd.ah_compress)  );
				pFrom.audio.ta_recvd.ah_compress = pContent->transferAvReplyInfo.tranInfo.audio.ah_compress;
				//memcpy(&pFrom.audio.ta_recvd.ah_decompress, &pContent->transferAvReplyInfo.tranInfo.audio.ah_decompress, sizeof(pFrom.audio.ta_recvd.ah_decompress)  );
				pFrom.audio.ta_recvd.ah_decompress=pContent->transferAvReplyInfo.tranInfo.audio.ah_decompress;

				//  2014/09/04
				//pFrom.video.dynBmp.usIndex_obj  =  pContent->transferAvReplyInfo.dynBmp.usIndex_obj;
				pFrom.viewDynBmp_reply.ucbSaveVideo_reply = byte2bool(pContent->transferAvReplyInfo.viewDynBmpReply.ucbSaveVideo);

				//  2015/10/23
				pFrom.taskAvProps = pContent->transferAvReplyInfo.tranInfo.taskAvProps;
			}
			//
			
			//  2009/02/22
			pTc.videoConference.uiTranNo_lastModified_pMems_from = getuiNextTranNo(null, 0, null);

			//
#if false
			TCHAR tBuf[128];
			_sntprintf(tBuf, mycountof(tBuf), _T(" %I64u, index_pMems_from %d, avReply OK."), pMsgTask->data.route.idInfo_from.ui64Id, index);
			if (pFrom.viewDynBmp_reply.ucbSaveVideo_reply)
			{
				_sntprintf(tBuf, mycountof(tBuf), _T("%s save"), tBuf);
			}
			qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("procTaskAvReply:"), _T(""), _T(" %s"), tBuf);
#endif

			//  2012/04/01
			/*
			int i;
			for (i = 0; i < pTask.usCntLimit_activeMems_from; i++)
			{
				if (pTask.activeMems_from[i].idInfo.ui64Id == pFrom.idInfo.ui64Id)
				{       //  当已经申请发言的人中途退出又进入时,需要重新刷新话筒参数
					//PostMessage(pTask.hWnd_task, CONST_qyWm_postComm, CONST_qyWmParam_bNeedRefresh_activeMems_from, iTaskId);
				}
			}
			*/

			//  2010/07/19
			int iStatus = pContent->transferAvReplyInfo.tranInfo.audio.ah_compress.wfx.wFormatTag!=0 ? Consts.CONST_imTaskStatus_dualByReceiver : Consts.CONST_imTaskStatus_acceptedByReceiver;
			showTaskStatus(pMisCnt, pAddr_logicalPeer->idInfo, pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, pTask.ucbVideoConference, Consts.CONST_imCommType_transferAvInfo, iTaskId, iStatus, 0, 0, null, null);
#endif


			//
			OutputDebugString(_T("procTaskAvReply ok\n"));

			//
			iErr = 0;

		errLabel:

			if (0==iErr)
			{
				//  2011/07/03
#if false
				//  2017/09/10
				TASK_respInfo* pRespInfo = get_respInfo(pProcInfo, pTask.iIndex_sharedObj, pTask.iIndex_sharedObjUsr);
				if (pRespInfo)
				{
					TASK_respInfo_mem* pRespInfoMem = get_respInfoMem(pProcInfo, pRespInfo, &pMsgTask->data.route.idInfo_from, pTask.uiTaskType);
					if (pRespInfoMem)
					{
						pRespInfoMem->var.avReply.tRecvTime_lastResp = pMsgTask->tRecvTime;
						//
						Int64 tNow; time(&tNow);
						ushort usState = get_respInfoMem_state(pMisCnt, tNow, pRespInfoMem, NULL);
						//
						if (pRespInfoMem->var.usState_byDlgTalk != usState)
						{
							pRespInfoMem->var.bNeedCalc_byProcTaskDataResp = true;
							pRespInfo->bNeedCalc_byProcTaskDataResp = true;
						}
					}
				}
#endif
			}


			return iErr;
		}





		//
		public static unsafe int procTaskAudioDataResp(CCtxQmc pProcInfo, ref AV_TRAN_INFO pTranInfo, ref PROC_TASK_AV pTask, ref MIS_MSG_INPUT pMsgInput, ref TRANSFER_AUDIO_dataResp pResp)
		{
			int iErr = -1;

			/*
			char timeBuf[32] = "";
			TCHAR tHintBuf[255 + 1] = _T("AudioDataResp:");
			//
			Int64 tSelectTime = 0;
			int i;
			unsigned  short usDelay = 0;

			QY_SHARED_OBJ* pSharedObj = getSharedObjByIndex(pProcInfo, pTask.iIndex_sharedObj);        //  ÕâÀïÓ¦¸ÃÓÃindexÀ´»ñÈ¡
			if (!pSharedObj) return -1;
			COMPRESS_AUDIO* pCompressAudio = NULL;

			//
			if (pSharedObj->bSoundRecorder) pCompressAudio = &pProcInfo->av.localAv.recordSoundProcInfo.compressAudio;
			else
			{
				CAP_procInfo_audioU* pCapProcInfo = getCapAudioBySth(pProcInfo, pSharedObj->iIndex_capAudio, 0);
				if (!pCapProcInfo)
				{
# ifdef  __DEBUG__
					traceLogA("AudioDataResp failed: getCapAudioByIndex( %d ) return NULL.", pSharedObj->iIndex_capAudio);
#endif
			return -1;
				}
				pCompressAudio = &pCapProcInfo->common.compressAudio;
			}
			if (!pCompressAudio) return -1;

			//
			SHARED_OBJ_USR* pSharedObjUsr = getSharedObjUsr(pSharedObj, pTask.iIndex_sharedObjUsr);
			if (!pSharedObjUsr) return -1;
			//
			TASK_respInfo* pRespInfo = &pSharedObjUsr->taskRespInfo;
			TASK_respInfo_mem* pMem = NULL;

			//
			tSelectTime = pCompressAudio->tSelectTime;

			//  showDebugInfo(  pProcInfo,  _T(  "AudioResp"  ),  pMsgInput  );


			//
			getTimelStr(pMsgInput->tStartTime, timeBuf, mycountof(timeBuf));

			//	
			if (pMsgInput->tRecvTime < pMsgInput->tStartTime)
			{
				qyShowInfo1(CONST_qyShowType_debug, &pMsgInput->uiStep, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("procTaskAudioDataResp: recvTime < startTime [%s], skipped"), CQyString(timeBuf));
				iErr = 0; goto errLabel;
			}
			if (pMsgInput->tStartTime < tSelectTime)
			{
				pProcInfo->av.localAv.recordSoundProcInfo.usTimes_failure++;
				qyShowInfo1(CONST_qyShowType_debug, &pMsgInput->uiStep, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("procTaskAudioDataResp: startTime [%s] < selectTime, skipped"), CQyString(timeBuf));
				iErr = 0; goto errLabel;
			}

			_sntprintf(tHintBuf, mycountof(tHintBuf), _T("AudioDataResp: %I64u, time-consuming %ds, usCnt_pkts %d"), pMsgInput->data.route.idInfo_from.ui64Id, pMsgInput->tRecvTime - pMsgInput->tStartTime, pResp->usCnt_pkts);
			//  traceLogA(  (  "Audio: tn %d, time-consuming %ds, usCnt_pkts %d"  ),  pResp->uiTranNo_openAvDev,  pMsgInput->tRecvTime  -  pMsgInput->tStartTime,  pResp->usCnt_pkts  );


			if (!isRcOk(pMsgInput->usCode))
			{
				pProcInfo->av.localAv.recordSoundProcInfo.usTimes_failure++;
				qyShowInfo1(CONST_qyShowType_debug, &pMsgInput->uiStep, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("AudioDataResp: rc %d not ok, startTime [%s], error"), pMsgInput->usCode, CQyString(timeBuf));
			}
			else
			{

				usDelay = (unsigned  short  )(pMsgInput->tRecvTime - pMsgInput->tStartTime);

				//
				pMem = get_respInfoMem(pProcInfo, pRespInfo, &pMsgInput->data.route.idInfo_from, pTask.uiTaskType);
				//
				if (!pMem)
				{
					showInfo_open(0, _T("procTaskAudioDataResp"), _T("failed: pMem is null"));
					goto errLabel;
				}

				//
				if (usDelay != pMem->var.audio.usDelay)
				{
					if (pMem->var.audio.usDelay
						&& usDelay > pMem->var.audio.usDelay + 1)
					{
						pProcInfo->av.localAv.recordSoundProcInfo.usTimes_failure++;

						qyShowInfo1(CONST_qyShowType_debug, &pMsgInput->uiStep, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("AudioDataResp: time-consuming [%d] increases, error"), usDelay);
					}

					pMem->var.audio.usDelay = usDelay;
				}
			}

			pCompressAudio->ucTimeoutInS_showFrameInfoResp = (pCompressAudio->ucTimeoutInS_showFrameInfoResp + (unsigned  char  )(pMsgInput->tRecvTime - pMsgInput->tStartTime) + 3  )  / 2;
			//
			pCompressAudio->tRecvTime_resp_fromCli = pMsgInput->tRecvTime;

			*/

			iErr = 0;
		errLabel:

			//if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, &pMsgInput->uiStep, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("%s"), tHintBuf);

			return iErr;

		}


		public static unsafe int tmp_procTask_recv_cancel(CCtxQmc pProcInfo, ref PROC_TASK_AV pTask, ref MIS_MSG_INPUT pMsgInput, ref TASK_PROC_REQ pTaskProcReq)
		{
			int iErr = -1;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			qyFuncs.showInfo_open(0,"","tmp_procTask_recv_cancel called");

			/*

			TASK_respInfo* pRespInfo = get_respInfo(pProcInfo, pTask.iIndex_sharedObj, pTask.iIndex_sharedObjUsr);
			if (!pRespInfo) return -1;
			//  
			TASK_respInfo_mem* pRespInfoMem = NULL;

			pRespInfoMem = get_respInfoMem(pProcInfo, pRespInfo, &pMsgInput->data.route.idInfo_from, pTask.uiTaskType);
			if (!pRespInfoMem) return -1;

			pRespInfoMem->var.taskProcReq_cancel.tRecvTime_lastResp = pMsgInput->tRecvTime;
			//
			time_t tNow; time(&tNow);
			unsigned  short usState = get_respInfoMem_state(pMisCnt, tNow, pRespInfoMem, NULL);
			//
			if (pRespInfoMem->var.usState_byDlgTalk != usState)
			{
				pRespInfoMem->var.bNeedCalc_byProcTaskDataResp = true;
				pRespInfo->bNeedCalc_byProcTaskDataResp = true;
			}
			*/


			iErr = 0;
			return iErr;
		}


	}
}
