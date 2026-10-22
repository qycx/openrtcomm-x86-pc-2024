using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

partial class Consts
{
    public const int MIN_uiSizePerSnd_file = (256 * 1024);


    public const int MIN_taskTimeoutInS = 5;

    public const int MIN_uiSizePerSnd_file_big = MIN_uiSizePerSnd_file;
    public const int MIN_uiSizePerSnd_file_mid = 128 * 1024;
    public const int MIN_uiSizePerSnd_file_small = 64 * 1024;


}

namespace qy
{
    partial class qyFuncs
    {
        public static unsafe object mcTask_new(int iAllocType, int size, ref QELEM_mcTask p)
        {
            return 1;
        }

        public static unsafe void mcTask_free(ref QELEM_mcTask p)                              //  2010/04/24

        {
            return;
        }



        public static unsafe int initDispatch_qis(CCtxQmc pProcInfo, MSGR_ADDR* pAddr_logicalPeer, int iTaskId, AV_TRAN_INFO* pTranInfo, ref PROC_TASK_AV pTask)
        {
            int iErr = -1;
            /*
			QY_MC* pQyMc = (QY_MC*)pProcInfo->pQyMc;
			int i;
			int len;

			if (!pTask->usCntLimit_dispatchs) return 0;
			if (pTask->pDispatchs) return -1;

			len = pTask->usCntLimit_dispatchs * sizeof(QIS_DISPATCH);
			pTask->pDispatchs = (QIS_DISPATCH*)mymalloc(len);
			if (!pTask->pDispatchs) goto errLabel;
			memset(pTask->pDispatchs, 0, len);

			for (i = 0; i < pTask->usCntLimit_dispatchs; i++)
			{
				QIS_DISPATCH* p = &pTask->pDispatchs[i];
				GENERIC_Q_CFG tmpCfg;

				memcpy(&tmpCfg, &pProcInfo->cfg.dispatchQ, sizeof(tmpCfg));
				_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-a-%d-%d"), tmpCfg.name, iTaskId, i);
				_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-a-%d-%d"), tmpCfg.mutexName_prefix, iTaskId, i);

				if (initQyQ2(&tmpCfg, &pQyMc->cfg.rwLockParam, mycountof(p->audio.q2.hEvents), NULL, mallocMemory, mymalloc, 0, 0, freeMemory, myfree, NULL, &p->audio.q2)) goto errLabel;
				p->audio.bQInited = TRUE;

				//
				memcpy(&tmpCfg, &pProcInfo->cfg.dispatchQ, sizeof(tmpCfg));
				_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-v-%d-%d"), tmpCfg.name, iTaskId, i);
				_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-v-%d-%d"), tmpCfg.mutexName_prefix, iTaskId, i);
				if (initQyQ2(&tmpCfg, &pQyMc->cfg.rwLockParam, mycountof(p->video.q2.hEvents), NULL, mallocMemory, mymalloc, 0, 0, freeMemory, myfree, NULL, &p->video.q2)) goto errLabel;
				p->video.bQInited = TRUE;

				p->pProcInfo = pProcInfo;
				p->pAddr_logicalPeer = pAddr_logicalPeer;
				p->iTaskId = iTaskId;
				p->pTranInfo = pTranInfo;
				p->pTask = pTask;
				//
				p->index_activeMems_from = i;

			}

			if (startDispatchThreads(pTask)) goto errLabel;

			*/

            iErr = 0;
        errLabel:

            if (0 != iErr)
            {
                exitDispatch_qis(ref pTask);
            }

            return iErr;
        }


        public static unsafe void exitDispatch_qis(ref PROC_TASK_AV pTask)
        {
            int i;

            /*
			if (!pTask->pDispatchs) return;

			stopDispatchThreads(pTask);

			for (i = 0; i < pTask->usCntLimit_dispatchs; i++)
			{
				QIS_DISPATCH* p = &pTask->pDispatchs[i];

				//
				if (p->audio.bQInited) exitQyQ2(&p->audio.q2);
				if (p->video.bQInited) exitQyQ2(&p->video.q2);
			}

			free(pTask->pDispatchs); pTask->pDispatchs = NULL;
			*/

            return;
        }



        public static int initMisMsg(object p_msg)
        {
            //

            //
            int iErr = -1;
            //void* pMsgParam = (void*)p_msg;
            TASK_common pTaskParam = (TASK_common)p_msg;
            string str;

            // 
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;

            //
            CTX_caller ctxCaller = new CTX_caller();
            //mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));
            ctxCaller.dwThreadId = myGetCurrentThreadId();
            ctxCaller.bTmp = true;

            //
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTaskParam.m_index_taskInfo, "initMisMsg");
            if (pTaskInfo == null) return -1;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) return -1;

            //
            //fixed (MIS_MSGU* pMsg = &pTaskData.msgU)
            ref MIS_MSGU pMsg = ref pTaskData.msgU;
            {
                ;
                //  int								index;
                int i;

#if DEBUG
                qyFuncs.showInfo_open(0, "", "initMisMsg enters");
#endif
                /*
				if (myGetCurrentThreadId() != pProcInfo.dwThreadId_mgr)
				{
					showInfo_open(0, 0, _T("initMisMsg failed, not thread_mgr"));
# if  __DEBUG__
					MACRO_qyAssert(0, _T("initMisMsg failed, not thread_mgr"));
#endif
					return -1;
				}
				*/

                //
                switch (pMsg.uiType)
                {
                    case Consts.CONST_misMsgType_task:
                        {
                            ref IM_CONTENTU pContent = ref pMsg.task.data.buf;// M_getMsgContent(pMsg->task.uiType,pMsg->task.ucFlg, &pMsg->task.data);
                            //if (null == pContent) goto errLabel;

                            //
                            qyFuncs.traceLog("initMisMsg.182");

                            //
                            switch (pContent.uiType)
                            {
                                case Consts.CONST_imCommType_transferAvInfo:
                                    {
                                        if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
                                        QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                                        DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                                        //								
                                        TASK_transferAv task = (TASK_transferAv)pTaskParam;
                                        PROC_TASK_AV pTask = task.myTask;
                                        if (null == pTask) goto errLabel;

                                        //
                                        if (pTask.iIndex_taskInfo != task.m_index_taskInfo)
                                        {
                                            int ii = 0;
                                        }
                                        //								
                                        pTask.iIndex_taskInfo = task.m_index_taskInfo;


                                        //
                                        qyFuncs.traceLog("initMisMsg.208");

                                        //
                                        if (!pTaskInfo.var.m_bReplyTask)
                                        {
                                            //
                                            if (pContent.transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
                                            {
                                                //  2016/04/18
#if __DEBUG__
											MACRO_qyAssert(0, _T("transferAvInfo.subtype err, not task"));
#endif
                                                goto errLabel;
                                            }
                                            //
                                            //TRANSFER_av_task* pTransferAvTask = (TRANSFER_av_task*)pContent;
                                            //
                                            ref AV_TRAN_INFO pAvTran = ref pContent.transferAvInfo.ass.tranInfo;

                                            //  2014/08/04
                                            pTask.saveVideo.uiObjType = pContent.transferAvInfo.ass.resObj.uiObjType;          //  2014/08/08
                                            pTask.saveVideo.usIndex_obj = pContent.transferAvInfo.ass.resObj.usIndex_obj;
                                            pTask.saveVideo.ucbSaveVideo_req = byte2bool(pContent.transferAvInfo.viewDynBmp.ucbSaveVideo);

                                            //
                                            if (0 == videoConference.usCntLimit_mems_from || videoConference.usCntLimit_mems_from > Consts.MAX_mems_taskAv) goto errLabel;

                                            //  2008/11/16
#if false
										HDC hScreenDC =::GetDC(NULL);
										pTask.remoteAssist.maxxScreen = GetDeviceCaps(hScreenDC, HORZRES);
										pTask.remoteAssist.maxyScreen = GetDeviceCaps(hScreenDC, VERTRES);
								::		ReleaseDC(NULL, hScreenDC);
										//
										if (!pTask.remoteAssist.maxxScreen || !pTask.remoteAssist.maxyScreen) goto errLabel;
#endif

                                            //  2009/02/16
                                            //pTask.pMems_from = (TASK_AV_FROM*)mymalloc(pTask.usCntLimit_mems_from * sizeof(TASK_AV_FROM));
#if false
											pTc.videoConference.pMems_from = new TASK_AV_FROM[pTc.videoConference.usCntLimit_mems_from];
											if (null == pTc.videoConference.pMems_from)
											{
												//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initMisMsg: malloc for avInfo.pMems_from failed"));
												goto errLabel;
											}
											//memset(pTask.pMems_from, 0, pTask.usCntLimit_mems_from * sizeof(TASK_AV_FROM));
											for (i = 0; i < pTc.videoConference.usCntLimit_mems_from; i++)
											{
												//pTc.videoConference.pMems_from[i] = new TASK_AV_FROM();
											}
#endif


                                            //
                                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initMisMsg: %s usCntLimit_mems %d, usCntLimit_activeMems %d"), pTask.ucbVideoConferenceStarter ? _T("Video conference starter:") : _T(""), pTask.usCntLimit_mems_from, pTask.usCntLimit_activeMems_from);

                                            //  2010/08/31
                                            //memcpy(&pTask.pMems_from[0], &pTask.mem0_from, sizeof(pTask.pMems_from[0])  );
                                            //pTask.mem0_from.copyTo(pTc.videoConference.pMems_from[0]);

                                            //
                                            qyFuncs.traceLog("initMisMsg.271");


                                            //
                                            if (pTask.ucbVideoConferenceStarter)
                                            {   //  

                                                //  2009/02/17
                                                //  memcpy(  &pTask.pMems_from[0],  &pTask.mem0_from,  sizeof(  pTask.pMems_from[0]  )  );

                                                //
                                                if (pTask.videoConferenceCfg.iCols * pTask.videoConferenceCfg.iRows != videoConference.usCntLimit_mems_from) goto errLabel;
                                                //  if  (  !pTask.videoConferenceCfg.iWidth_pic  ||  !pTask.videoConferenceCfg.iHeight_pic  )  goto  errLabel;
                                                //pTc->videoConference.usCntLimit_activeMems_from = pTask.videoConferenceCfg.usCntLimit_activeMems_from;
                                                if (0 == videoConference.usCntLimit_activeMems_from || videoConference.usCntLimit_activeMems_from > Consts.MAX_activeMems_from)
                                                {
                                                    //traceLogA("initMisMsg: usCntLimit_activeMems_from error, %d", pTask.usCntLimit_activeMems_from);
                                                    goto errLabel;
                                                }

                                                //
                                                //  if  (  makeBmpInfoHeader_rgb(  24,  pTask.videoConferenceCfg.iWidth_pic,  pTask.videoConferenceCfg.iHeight_pic,  &pContent->transferAvInfo.tranInfo.video.vh_decompress.bih  )  )  goto  errLabel;


                                                //  pBmpInfo_input	=	&pContent->transferAvInfo.tranInfo.video.vh_decompress.bih;

                                                //  fccHandler  =  fourccStr2i(  pContent->transferAvInfo.tranInfo.video.compressor.fourccStr  );

                                            }

                                            //  if  (  pTask.ucbVideoConferenceStarter  )  
                                            {   //  2011/01/23.

                                                //
                                                GENERIC_Q_CFG qCfg;
                                                //memcpy(&qCfg, &pProcInfo->cfg.outputQ_toMix_audio, sizeof(qCfg));
                                                qCfg = pProcInfo.cfg.outputQ_toMix_audio;
                                                //  2012/03/24
                                                //_sntprintf(qCfg.name, mycountof(qCfg.name), _T("%s%u"), qCfg.name, pMsg->task.iTaskId);
                                                //str = string.Format("{0}{1}", new string(qCfg.name), pMsg->task.iTaskId);
                                                str = "";
                                                //mytcsncpy(qCfg.name, str, Consts.cntof_qCfg_name);
                                                //_sntprintf(qCfg.mutexName_prefix, mycountof(qCfg.mutexName_prefix), _T("%s%u"), qCfg.mutexName_prefix, pMsg->task.iTaskId);
                                                //str = string.Format("{0}{1}", new string(qCfg.mutexName_prefix), pMsg->task.iTaskId);
                                                //mytcsncpy(qCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

                                                //if (!(pTask.mixer.pOutputQ2 = (QY_Q2*)mymalloc(sizeof(QY_Q2)))) goto errLabel;
                                                pTask.mixer.pOutputQ2 = new QY_Q2();
                                                //memset(pTask.mixer.pOutputQ2, 0, sizeof(QY_Q2));
                                                if (0 != initQyQ2(qCfg, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pTask.mixer.pOutputQ2)) goto errLabel;

                                                //  2009/07/26
                                                //memcpy(&qCfg, &pProcInfo->cfg.outputQ_toMix_video, sizeof(qCfg));
                                                qCfg = pProcInfo.cfg.outputQ_toMosaic_video;
                                                //  2012/03/24
                                                //_sntprintf(qCfg.name, mycountof(qCfg.name), _T("%s%u"), qCfg.name, pMsg->task.iTaskId);
                                                //str = string.Format("{0}{1}", new string(qCfg.name), pMsg->task.iTaskId);
                                                //mytcsncpy(qCfg.name, str, Consts.cntof_qCfg_name);
                                                //_sntprintf(qCfg.mutexName_prefix, mycountof(qCfg.mutexName_prefix), _T("%s%u"), qCfg.mutexName_prefix, pMsg->task.iTaskId);
                                                //str = string.Format("{0}{1}", new string(qCfg.mutexName_prefix), pMsg->task.iTaskId);
                                                //mytcsncpy(qCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

                                                //if (!(pTask.photomosaic.pOutputQ2 = (QY_Q2*)mymalloc(sizeof(QY_Q2)))) goto errLabel;
                                                pTask.photomosaic.pOutputQ2 = new QY_Q2();
                                                //memset(pTask.photomosaic.pOutputQ2, 0, sizeof(QY_Q2));
                                                if (0 != initQyQ2(qCfg, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pTask.photomosaic.pOutputQ2)) goto errLabel;

                                                //
                                            }

                                            //
                                            qyFuncs.traceLog("initMisMsg.341");

                                            //
                                            pTask.index_trans = GuiShare.pf_newIndex_qisTrans();
                                            pTask.trans = GuiShare.pf_getQisTransByIndex(pTask.index_trans);
                                            if (pTask.trans == null)
                                            {
                                                qyFuncs.showInfo_open(0, "", "initMisMsg.newIndex_qisTrans failed");
                                                goto errLabel;
                                            }

                                            //
                                            int tt = 0;
                                            videoConference.uiTranNo_lastModified_pMems_from = getuiNextTranNo(null, 0, ref tt);


                                            //  2009/05/12. init transforms
                                            switch (pContent.transferAvInfo.uiTaskType)
                                            {
                                                case Consts.CONST_imTaskType_transferAvInfo:
                                                    if (pTask.bVideoConference())
                                                    {
                                                        pTask.trans.usCntLimit_transforms = videoConference.usCntLimit_activeMems_from;
                                                    }
                                                    else
                                                    {
                                                        // add one more trans to dec video
                                                        pTask.trans.usCntLimit_transforms = (ushort)(videoConference.usCntLimit_activeMems_from + 1);
                                                    }
                                                    //
                                                    break;
                                                case Consts.CONST_imTaskType_viewDynBmp:           //  2009/09/08
                                                    pTask.trans.usCntLimit_transforms = videoConference.usCntLimit_activeMems_from;
                                                    break;
                                                default:
                                                    break;
                                            }
                                            if (pTask.doProxy.bExists_avProxy)
                                            {
                                                pTask.trans.usCntLimit_transforms = 1;
                                            }
                                            if (0 != initTransforms(pProcInfo, ref pMsg.task.addr_logicalPeer, pMsg.task.iTaskId, pContent.transferAvInfo.uiTaskType, ref pTask, pTaskParam.m_index_taskInfo)) goto errLabel;
                                            if (0 != startTransformThreads(pProcInfo, pContent.transferAvInfo.uiTaskType, ref pTask, "initMisMsg ")) goto errLabel;

                                            //
                                            qyFuncs.traceLog("initMisMsg.377");

                                            //
                                            if (pTask.ucbVideoConferenceStarter)
                                            {
                                                //  2011/12/08
                                                pTask.usCntLimit_dispatchs = pTask.trans.usCntLimit_transforms;
                                            }
                                            if (pTask.doProxy.bExists_avProxy)
                                            {  //  2014/11/21
                                                pTask.usCntLimit_dispatchs = pTask.trans.usCntLimit_transforms;
                                            }

                                            //  2014/11/21
                                            if (0 != pTask.usCntLimit_dispatchs)
                                            {
                                                //  2009/06/17				
                                                //if (0 != initDispatch_qis(pProcInfo, &pMsg.task.addr_logicalPeer, pMsg->task.iTaskId, &pContent->transferAvInfo.ass.tranInfo, ref pTask)) goto errLabel;
                                            }

                                            //  2009/10/15
                                            //_sntprintf(pTask.debugHint, mycountof(pTask.debugHint), _T("avReq: taskId %d, %s"), pMsg->task.iTaskId, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, CONST_resId_imTaskTypeTable), pContent->transferAvInfo.uiTaskType));
                                            //  traceLogA(  "%S",  pTask.debugHint  );

                                            //  2018/10/30
                                            if (pContent.transferAvInfo.uiTaskType == Consts.CONST_imTaskType_transferAvInfo)
                                            {
                                                GuiShare.pf_PostMessage(pTask.hWnd_task, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_misMsgInited, 0);
                                            }

                                            //
                                            qyFuncs.traceLog("initMisMsg.408");


                                            //
                                            break;
                                        }

                                        //
                                        qyFuncs.traceLog("initMisMsg.416");


                                        //case Consts.CONST_imCommType_transferAvReplyInfo:
                                        {
                                            //
                                            AV_TRAN_INFO tmp_avTranInfo = task.myTask.reply_avTranInfo;
                                            ref AV_TRAN_INFO pAvTran = ref tmp_avTranInfo;// pContent->transferAvReplyInfo.tranInfo;
                                                                                    //
                                                                                    //if (!pTask) goto errLabel;

                                            //
                                            if (videoConference.usCntLimit_mems_from < 2 || videoConference.usCntLimit_mems_from > Consts.MAX_mems_taskAv)
                                            {
                                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initMisMsg failed, transferAvReply, usCntLimit_mems_from %d, err"), pTask.usCntLimit_mems_from);
                                                goto errLabel;
                                            }

                                            //  2009/02/16
#if false
											//pTask.pMems_from = (TASK_AV_FROM*)mymalloc(pTask.usCntLimit_mems_from * sizeof(TASK_AV_FROM));
											pTc.videoConference.pMems_from = new TASK_AV_FROM[pTc.videoConference.usCntLimit_mems_from];
											if (null == pTc.videoConference.pMems_from)
											{
												//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initMisMsg: malloc for avReplyInfo.pMems_from failed"));
												goto errLabel;
											}
											//memset(pTask.pMems_from, 0, pTask.usCntLimit_mems_from * sizeof(TASK_AV_FROM));
											for (i = 0; i < pTc.videoConference.usCntLimit_mems_from; i++)
											{
												pTc.videoConference.pMems_from[i] = new TASK_AV_FROM();
											}
#endif

                                            //
                                            if (pTask.bVideoConference()
                                                || pTask.receiver.bRecvd_proxiedTranInfo)
                                            {
                                                //  2009/05/31											
                                                pTask.audio.pMsgU_toTransform = new CMIS_MSGU();// (MIS_MSGU*)mymalloc(sizeof(MIS_MSGU));
                                                if (null == pTask.audio.pMsgU_toTransform) goto errLabel;
                                                MACRO_memset_misMsgTask(ref pTask.audio.pMsgU_toTransform.msgU.task);

                                                pTask.video.pMsgU_toTransform = new CMIS_MSGU();// (MIS_MSGU*)mymalloc(sizeof(MIS_MSGU));
                                                if (null == pTask.video.pMsgU_toTransform) goto errLabel;
                                                MACRO_memset_misMsgTask(ref pTask.video.pMsgU_toTransform.msgU.task);
                                            }

                                            //  2009/02/17
                                            //memcpy(&pTask.pMems_from[0], &pTask.mem0_from, sizeof(pTask.pMems_from[0])  );

                                            //  2014/11/21
                                            if (pTask.receiver.bRecvd_proxiedTranInfo)
                                            {
#if false
												if (pTc.videoConference.usCntLimit_mems_from < 2) goto errLabel;
												//memcpy(&pTask.pMems_from[1], &pTask.receiver.proxied_from, sizeof(pTask.pMems_from[1])  );
												pTask.receiver.proxied_from.copyTo(pTc.videoConference.pMems_from[1]);
#endif
                                            }

                                            //
                                            //
                                            qyFuncs.traceLog("initMisMsg.480");


                                            //  2011/01/26
                                            //  if  (  pTask.ucbVideoConference  )  
                                            {   //  2011/01/23.

                                                //
                                                GENERIC_Q_CFG qCfg;
                                                //memcpy(&qCfg, &pProcInfo->cfg.outputQ_toMix_audio, sizeof(qCfg));
                                                qCfg = pProcInfo.cfg.outputQ_toMix_audio;
                                                //  2012/03/24
                                                //_sntprintf(qCfg.name, mycountof(qCfg.name), _T("%s%u"), qCfg.name, pMsg->task.iTaskId);
                                                /*
                                                str = string.Format("{0}{1}", new string(qCfg.name), pMsg->task.iTaskId);
                                                mytcsncpy(qCfg.name, str, Consts.cntof_q2Cfg_name);
                                                //_sntprintf(qCfg.mutexName_prefix, mycountof(qCfg.mutexName_prefix), _T("%s%u"), qCfg.mutexName_prefix, pMsg->task.iTaskId);
                                                str = string.Format("{0}{1}", new string(qCfg.mutexName_prefix), pMsg->task.iTaskId);
                                                mytcsncpy(qCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                                                */


                                                //if (!(pTask.mixer.pOutputQ2 = (QY_Q2*)mymalloc(sizeof(QY_Q2)))) goto errLabel;
                                                pTask.mixer.pOutputQ2 = new QY_Q2();
                                                //memset(pTask.mixer.pOutputQ2, 0, sizeof(QY_Q2));
                                                if (0 != initQyQ2(qCfg, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pTask.mixer.pOutputQ2)) goto errLabel;

                                                //
                                                qyFuncs.traceLog("initMisMsg.505");

                                                //  2009/07/26
                                                //memcpy(&qCfg, &pProcInfo->cfg.outputQ_toMix_video, sizeof(qCfg));
                                                qCfg = pProcInfo.cfg.outputQ_toMosaic_video;
                                                //  2012/03/24
                                                //_sntprintf(qCfg.name, mycountof(qCfg.name), _T("%s%u"), qCfg.name, pMsg->task.iTaskId);
                                                /*
                                                str = string.Format("{0}{1}", new string(qCfg.name), pMsg->task.iTaskId);
                                                mytcsncpy(qCfg.name, str, Consts.cntof_q2Cfg_name);
                                                //_sntprintf(qCfg.mutexName_prefix, mycountof(qCfg.mutexName_prefix), _T("%s%u"), qCfg.mutexName_prefix, pMsg->task.iTaskId);
                                                str = string.Format("{0}{1}", new string(qCfg.mutexName_prefix), pMsg->task.iTaskId);
                                                mytcsncpy(qCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                                                */


                                                //if (!(pTask.photomosaic.pOutputQ2 = (QY_Q2*)mymalloc(sizeof(QY_Q2)))) goto errLabel;
                                                pTask.photomosaic.pOutputQ2 = new QY_Q2();
                                                //memset(pTask.photomosaic.pOutputQ2, 0, sizeof(QY_Q2));
                                                if (0 != initQyQ2(qCfg, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pTask.photomosaic.pOutputQ2)) goto errLabel;

                                                //
                                            }

                                            //
                                            qyFuncs.traceLog("initMisMsg.529");

                                            //
                                            pTask.index_trans = GuiShare.pf_newIndex_qisTrans();
                                            pTask.trans = GuiShare.pf_getQisTransByIndex(pTask.index_trans);
                                            if  (  pTask.trans==null)
                                            {
                                                qyFuncs.showInfo_open(0, "", "initMisMsg.newIndex_qisTrans failed");
                                                goto errLabel;
                                            }

                                            //  2009/05/12. init transforms
                                            //
                                            switch (pContent.transferAvInfo.uiTaskType)
                                            {
                                                case Consts.CONST_imTaskType_transferAvInfo:
                                                    pTask.trans.usCntLimit_transforms = videoConference.usCntLimit_activeMems_from;
                                                    break;
                                                default:
                                                    pTask.trans.usCntLimit_transforms = 1;
                                                    break;
                                            }

                                            //
                                            //
                                            qyFuncs.traceLog("initMisMsg.544");

                                            //
                                            try
                                            {
                                                if (0 != initTransforms(pProcInfo, ref pMsg.task.addr_logicalPeer, pMsg.task.iTaskId, pMsg.task.uiTaskType, ref pTask, pTaskParam.m_index_taskInfo)) goto errLabel;
                                                if (0 != startTransformThreads(pProcInfo, pMsg.task.uiTaskType, ref pTask, "initMisMsg haha")) goto errLabel;
                                            }
                                            catch (Exception e)
                                            {
                                                qyFuncs.showInfo_open(0, "", "initMisMsg.554, initTransforms exception");
                                                goto errLabel;
                                            }

                                            //
                                            qyFuncs.traceLog("initMisMsg.551");


                                            //  2009/10/15
                                            //_sntprintf(pTask.debugHint, mycountof(pTask.debugHint), _T("avReply: taskId %d"), pMsg->task.iTaskId);
                                            //  traceLogA(  "%S",  pTask.debugHint  );

                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        break;
                    default:
                        break;
                }
            }

            iErr = 0;
        errLabel:

            if (iErr != 0)
            {
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("initMisMsg failed, to call exitMisMsg"));
                //
                exitMisMsg(p_msg);
            }


            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initMisMsg leaves with %s"), iErr ? _T("Failed") : _T("OK"));

            qyFuncs.showInfo_open(0,"","initMisMsg leaves with " + ((iErr != 0) ? "failed" : "OK"));

            //
            return iErr;
        }

        public static void exitMisMsg(object p_msg)
        {

            //void* pMsgParam = (void*)p_msg;

            TASK_common pTaskParam = (TASK_common)p_msg;
            string str;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return;

            //
            CTX_caller ctxCaller = new CTX_caller();
            //mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));
            ctxCaller.type = Consts.CONST_ctxCallerType_thread;
            ctxCaller.dwThreadId = myGetCurrentThreadId();
            ctxCaller.bTmp = true;

            //
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTaskParam.m_index_taskInfo, "exitMisMsg");
            if (pTaskInfo == null) return;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) return;

            //
            //fixed (MIS_MSGU* pMsg = &pTaskData.msgU)
            ref MIS_MSGU pMsg = ref pTaskData.msgU;            {

                int index;
                int i;

#if __DEBUG__
				traceLogA("exitMisMsg enters");
#endif
                showInfo_open(0, null, "exitMisMsg enters");

                //
                /*
				if (myGetCurrentThreadId() != pProcInfo.dwThreadId_mgr)
				{
					showInfo_open(0, null, _T("exitMisMsg failed, not thread_mgr"));
#if __DEBUG__
					MACRO_qyAssert(0, _T("exitMisMsg failed, not thread_mgr"));
#endif
				}
				*/

                //
                switch (pMsg.uiType)
                {
                    case Consts.CONST_misMsgType_task:
                        {
                            ref IM_CONTENTU pContent = ref pMsg.task.data.buf; ;// M_getMsgContent(pMsg->task.uiType,pMsg->task.ucFlg, &pMsg->task.data);
                            //if (null == pContent) goto errLabel;

                            //
#if DEBUG
#endif

                            //
                            //printMisMsg(null, null, pMsg);

                            //
                            switch (pContent.uiType)
                            {
                                case Consts.CONST_imCommType_transferFileReq:
                                    {
                                        TASK_transferFile task = (TASK_transferFile)pTaskParam;
                                        PROC_TASK_FILE pTask = task.myTask;
                                        if (pTask == null) break;
                                        if (pTask.fp != null)
                                        {
#if __DEBUG__
										traceLogA("exitMisMsg: transferFileReq:{%S}", pContent->transferFileReq.fullFilePath);
#endif
                                            GuiShare.pf_fclose(pTask.fp);
                                            pTask.fp = null;
                                        }

                                        traceLog("exitMisMsg: transferFileReq finished");
                                    }
                                    break;
                                case Consts.CONST_imCommType_transferFileReplyReq:
                                    /*
									if (pContent->transferFileReplyReq.fp)
									{
#if __DEBUG__
										traceLogA("exitMisMsg: transferFileReplyReq:{%S}", pContent->transferFileReplyReq.fullFilePath_save);
#endif
										fclose(pContent->transferFileReplyReq.fp);
									}
									*/
                                    traceLog("exitMisMsg: transferFileReplyReq not finished");

                                    break;
                                case Consts.CONST_imCommType_transferAvInfo:
                                    {
                                        TASK_transferAv task = (TASK_transferAv)pTaskParam;
                                        PROC_TASK_AV pTask = task.myTask;

                                        //PROC_TASK_AV	*	pTask  =  get_transferAvReplyInfo_pTask(  &pContent->transferAvReplyInfo,  _T(  ""  )  );  //  &pContent->transferAvReplyInfo.myTask;
                                        if (null == pTask) break;

                                        if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
                                        QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                                        DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                                        //
                                        if (!pTaskInfo.var.m_bReplyTask)
                                        {

                                            if (pContent.transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
                                            {
#if __DEBUG__
											MACRO_qyAssert(0, _T("transferAvInfo.subtype err, not task"));
#endif
                                                break;
                                            }
                                            //TRANSFER_av_task* pTransferAvTask = (TRANSFER_av_task*)pContent;

                                            //  2008/11/20¡£°´Àí£¬Ö»ÓÐÔ¶³ÌÐ­Öú²Å¿ÉÒÔÇåÀí¡£µ«ÊÇËÆºõÒ²Ã»ÓÐÊ²Ã´Ó°Ïì¡£
                                            clearShiftKeys();

                                            //  2010/09/05
                                            QY_SHARED_OBJ pSharedObj = null;
                                            if (null != (pSharedObj = getSharedObjByIndex(pProcInfo, pTask.iIndex_sharedObj)))
                                            {
                                            }

                                            //  2009/06/17
                                            exitDispatch_qis(ref pTask);

                                            //  2009/05/12
                                            exitTransforms(pProcInfo, ref pTask);
                                            GuiShare.pf_freeIndex_qisTrans(pTask.index_trans);  pTask.index_trans = -1;
                                            pTask.trans = null;

                                            //
                                            for (index = 0; index < videoConference.usCntLimit_mems_from; index++)
                                            {
                                                //  exitTaskAvFrom_video(  pProcInfo,  &pContent->transferAvInfo.myTask.pMems_from[index]  );
                                                //  exitTaskAvFrom_audio(  pProcInfo,  &pContent->transferAvInfo.myTask.pMems_from[index]  );
                                            }

                                            //  if  (  pTask.mixer.pMsgTask_toCompress  )  free(  pTask.mixer.pMsgTask_toCompress  );

                                            //  if  (  pTask.mixer.pOutputQ  )  genericQFree(  pTask.mixer.pOutputQ  );
                                            if (null != pTask.mixer.pOutputQ2)
                                            {
                                                exitQyQ2(pTask.mixer.pOutputQ2);
                                                //MACRO_safeFree(pTask.mixer.pOutputQ2);
                                                pTask.mixer.pOutputQ2 = null;
                                            }
                                            if (null != pTask.photomosaic.pOutputQ2)
                                            {       //  2009/07/26
                                                exitQyQ2(pTask.photomosaic.pOutputQ2);
                                                //MACRO_safeFree(pTask.photomosaic.pOutputQ2);
                                                pTask.photomosaic.pOutputQ2 = null;
                                            }

                                            //  2008/10/06
                                            if (!pTask.doProxy.bExists_avProxy)
                                            {       //  2014/11/20
                                                if (pContent.transferAvInfo.uiTaskType != Consts.CONST_imTaskType_transferAvInfo)
                                                {   //  
                                                    askSharedObjToStop(pProcInfo, pSharedObj, pQyMc.gui.hMainWnd, _T("exitMisMsg avInfo"));
                                                }
                                            }

                                            //  2009/05/31
                                            //IntPtr tmpp = (IntPtr)pTask.audio.pMsgU_toTransform;
                                            //MACRO_safeFree(ref tmpp); pTask.audio.pMsgU_toTransform = (MIS_MSGU*)tmpp;
                                            pTask.audio.pMsgU_toTransform = null;
                                            //tmpp = (IntPtr)pTask.video.pMsgU_toTransform;
                                            //MACRO_safeFree(ref tmpp); pTask.video.pMsgU_toTransform = (MIS_MSGU*)tmpp;
                                            pTask.video.pMsgU_toTransform = null;

                                            //  2009/02/16
                                            if (null != videoConference.pMems_from1)
                                            {
                                                //myfree(pTask.pMems_from);
                                                videoConference.pMems_from1 = null;
                                            }

                                        }

                                        else
                                        {
                                            //case Consts.CONST_imCommType_transferAvReplyInfo:
                                            {
                                                if (null == pTask) break;

                                                //  2008/11/20¡£°´Àí£¬Ö»ÓÐÔ¶³ÌÐ­Öú²Å¿ÉÒÔÇåÀí¡£µ«ÊÇËÆºõÒ²Ã»ÓÐÊ²Ã´Ó°Ïì¡£
                                                clearShiftKeys();

                                                //  2010/09/05

                                                //  2009/05/12
                                                exitTransforms(pProcInfo, ref pTask);
                                                GuiShare.pf_freeIndex_qisTrans(pTask.index_trans);  pTask.index_trans = -1;
                                                pTask.trans = null;

                                                //
                                                for (index = 0; index < videoConference.usCntLimit_mems_from; index++)
                                                {
                                                    //  exitTaskAvFrom_video(  pProcInfo,  &pContent->transferAvReplyInfo.myTask.pMems_from[index]  );									 
                                                    //  exitTaskAvFrom_audio(  pProcInfo,  &pContent->transferAvReplyInfo.myTask.pMems_from[index]  );									 
                                                }

                                                //
                                                if (null != pTask.mixer.pOutputQ2)
                                                {
                                                    exitQyQ2(pTask.mixer.pOutputQ2);
                                                    //MACRO_safeFree(pTask.mixer.pOutputQ2);
                                                    pTask.mixer.pOutputQ2 = null;
                                                }
                                                if (null != pTask.photomosaic.pOutputQ2)
                                                {       //  2009/07/26
                                                    exitQyQ2(pTask.photomosaic.pOutputQ2);
                                                    //MACRO_safeFree(pTask.photomosaic.pOutputQ2);
                                                    pTask.photomosaic.pOutputQ2 = null;
                                                }

                                                //  2009/05/31
                                                //IntPtr tmpp = (IntPtr)pTask.audio.pMsgU_toTransform;
                                                //MACRO_safeFree(ref tmpp); pTask.audio.pMsgU_toTransform = (MIS_MSGU*)tmpp;
                                                pTask.audio.pMsgU_toTransform = null;
                                                //tmpp = (IntPtr)pTask.video.pMsgU_toTransform;
                                                //MACRO_safeFree(ref tmpp); pTask.video.pMsgU_toTransform = (MIS_MSGU*)tmpp;
                                                pTask.video.pMsgU_toTransform = null;
                                                //  2009/02/16
                                                if (null != videoConference.pMems_from1)
                                                {
                                                    //free(pTask.pMems_from);
                                                    videoConference.pMems_from1 = null;
                                                }
                                            }
                                        }
                                    }
                                    //
                                    break;
                                default:
                                    break;
                            }

                        }
                        break;
                    default:
                        break;
                }

            }

        errLabel:

            //
            showInfo_open(0, null, _T("exitMisMsg leaves"));
            qyFuncs.traceLog("exitMisMsg leaves");

            //
            return;
        }

        //  int  initTaskAvFrom_audio(  MC_VAR_isCli  *  pProcInfo,  TASK_AV_FROM  *  pFrom  )
        public static   int initTaskAvFrom_audio(CCtxQmc pProcInfo, DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from, int index_activeMems_from, QY_TRANSFORM pTransform)
        {
            int iErr = -1;
            
            //return -1;

            //TCHAR tHintBuf[256] = _T("");
            int tmpiRet = -1;
            int i;

            CCtxQyMc pQyMc = g.g_pQyMc;//QY_GET_GBUF(  );
            if (null == pQyMc) return -1;
            ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
            //if (!pQmcCfg) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
            if (null == pTask)
            {  //  2014/08/04
                uint step = 0;
                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, "", pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("initTaskAvFrom_audio failed: pTask is null."));
                return -1;
            }
            TRANSFORM_AUDIO_procInfo pTransformAudio = pTransform.audio;


            //
            /*
            WAVEFORMATEX* pWf_compress = null;
            WAVEFORMATEX* pWf_pcm = null;
            WAVEFORMATEX* pWf_decompress = null;
            */

            /*
			MMRESULT mmr;
			HACMDRIVER had = null;
			HACMSTREAM hstr1 = null;
			HACMSTREAM hstr2 = null;
			*/

            //TCHAR tBuf[128];

            //
#if __DEBUG__
			traceLogA("initTaskAvFrom_audio: %I64u enters", pFrom.idInfo.ui64Id);
#endif

            //
            if (pTransform.audio.ucbInited)
            {   //  2009/05/29
                traceLogA("initTaskAvFrom_audio failed: already inited.");
                uint step = 0;
                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, "", pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("initTaskAvFrom_audio failed: already inited."));
                goto errLabel;
            }

            if (!pTransform.audio.ucbInited)
            {
                if (0 == pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
                {
                    uint step = 0;
                    qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, "", pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("initTaskAvFrom_audio failed: uiTranNo_openAvDev is 0, audio data can't be accepted."));
                    goto errLabel;
                }

                try
                {
                    //  2009/05/28
                    {
                        using (CQySyncObj syncObj = new CQySyncObj())
                        {
                            ;
                            //TCHAR mutexName[128];
                            TMP_tBuf128 mutexName;
                            int cntof_mutexName = Consts.cntof_tBuf128;
                            //  2011/07/03
                            //  _sntprintf(  mutexName,  mycountof(  mutexName  ),  _T(  "%s%d"  ),  pProcInfo->cfg.mutexName_syncActiveMem_prefix,  pTransform.iTaskId  );
                            //M_getMutexName_syncActiveMem(ref pQmcCfg, pTransform.iTaskId, mutexName.tBuf, cntof_mutexName);
                            M_getMutexName_syncActiveMem(ref pQmcCfg, pTransform.iTaskId, ref mutexName);
                            //
                            if (0 != syncObj.sync("", pMisCnt.hMutex_syncActiveMem, 1000)) goto errLabel;

                            pTransform.audio.idInfo.ui64Id = pActiveMem_from.avStream.idInfo.ui64Id;
                            //  2016/12/23
                            pTransform.audio.ta = pActiveMem_from.avStream.obj.tranInfo.audio;

                            //
#if __DEBUG__
#endif
                        }
                    }
                }
                catch (Exception e)
                {
                    goto errLabel;
                }

                //  2015/09/11
                /*
                 * fixed (MY_DEBUG_STEP* pdebugStep = &pTransform.audio.debugStep)
                {
                    mymemset((IntPtr)pdebugStep, 0, sizeof(MY_DEBUG_STEP));
                }
                */
                pTransform.audio.debugStep = new MY_DEBUG_STEP();
                /*
                fixed (TRANSFORM_AUDIO_procInfo_status* pstatus = &pTransform.audio.status)
                {
                    mymemset((IntPtr)pstatus, 0, sizeof(TRANSFORM_AUDIO_procInfo_status));
                }
                */
                pTransform.audio.status = new TRANSFORM_AUDIO_procInfo_status();
                //
                //fixed (TRANSFORM_AUDIO_procInfo_decInfo* pdecInfo = &pTransform.audio.decInfo)
                //{
                //    mymemset((IntPtr)pdecInfo, 0, sizeof(TRANSFORM_AUDIO_procInfo_decInfo));
                //}
                pTransform.audio.decInfo_unused.memset0();


                //
                M_setMyStep(ref pTransform.audio.debugStep, 33);

                //
                if (0 == pActiveMem_from.avStream.obj.tranInfo.audio.compressor.ucCompressors) pTransform.audio.ta.compressor.ucCompressors = Consts.CONST_audioCompressors_acm;      //  2009/03/30

                //  2014/03/25
                //memset(&pTransform.audio.u, 0, sizeof(pTransform.audio.u)  );
                pTransform.audio.u_qoi.myQoi.common.memset0();


                //
                if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_acm)
                {
                    /*

# ifdef  __SUPPORT_acmToDec__
					HACMDRIVERID hadid = null;

# ifndef  __WINCE__
					hadid = find_driver(pFrom.audio.ta_recvd.ah_compress.wfx.wFormatTag, pFrom.audio.ta_recvd.ah_compress.wfx.nChannels, pFrom.audio.ta_recvd.ah_compress.wfx.nSamplesPerSec, pFrom.audio.ta_recvd.ah_compress.wfx.wBitsPerSample);
					if (hadid == null)
					{
# ifdef  __DEBUG__
						traceLogA("initTaskAvFrom_audio failed. No driver found.");
#endif
												//
												_sntprintf(tBuf, mycountof(tBuf), _T("initTaskAvFrom_audio failed. No driver found. wFormagTag %d"), pFrom.audio.ta_recvd.ah_compress.wfx.wFormatTag);
						showInfo_open(0, 0, tBuf);
						//
						goto errLabel;
					}
# ifdef  __DEBUG__
					traceLogA("Driver found ( hadid: %4.4lxh)\n", hadid);
#endif

					//  ÕÒµ½¸ø¶¨±êÇ©µÄ¶ÔÓ¦µÄ¸ñÊ½
					pWf_compress = get_driver_format(hadid, pFrom.audio.ta_recvd.ah_compress.wfx.wFormatTag, pFrom.audio.ta_recvd.ah_compress.wfx.nChannels, pFrom.audio.ta_recvd.ah_compress.wfx.nSamplesPerSec, pFrom.audio.ta_recvd.ah_compress.wfx.wBitsPerSample);
					if (!pWf_compress) goto errLabel;
# ifdef  __DEBUG__
					traceLogA("Driver format:%u bits,  %lu samples ps, %d channels\n ", pWf_compress->wBitsPerSample, pWf_compress->nSamplesPerSec, pWf_compress->nChannels);
#endif

					//  ÕÒµ½¸ø¶¨±êÇ©¶ÔÓ¦µÄÇý¶¯
					pWf_pcm = get_driver_suggestedFormat(hadid, pWf_compress, WAVE_FORMAT_PCM, pFrom.audio.ta_recvd.ah_compress.wfx.nChannels, pFrom.audio.ta_recvd.ah_compress.wfx.nSamplesPerSec, pFrom.audio.ta_recvd.ah_compress.wfx.wBitsPerSample);
					if (!pWf_pcm) goto errLabel;
# ifdef  __DEBUG__
					traceLogA("PCM  format: %u bits,  %lu samples ps, %d channels\n", pWf_pcm->wBitsPerSample, pWf_pcm->nSamplesPerSec, pWf_pcm->nChannels);
#endif

					//
					pWf_decompress = (WAVEFORMATEX*)mymalloc(sizeof(WAVEFORMATEX));
					if (!pWf_decompress) goto errLabel;
					if (!pFrom.audio.ta_recvd.ah_decompress.wfx.nChannels || !pFrom.audio.ta_recvd.ah_decompress.wfx.nSamplesPerSec || !pFrom.audio.ta_recvd.ah_decompress.wfx.wBitsPerSample)
					{
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initTaskAvFrom_audio: wave fomat error, use default."));
						makeWaveFormat_pcm(DEFAULT_nChannels_pcm_av_old, DEFAULT_iHertz_pcm_av_old, DEFAULT_wBitsPerSample_pcm_av_old, &pTransform.audio.ta.ah_decompress.wfx);
					}
					memcpy(pWf_decompress, &pFrom.audio.ta_recvd.ah_decompress.wfx, sizeof(WAVEFORMATEX));
#else
					switch (pFrom.audio.ah_compress_recvd.wfx.wFormatTag)
					{
						case WAVE_FORMAT_GSM610:
							GSM610WAVEFORMAT* pGsm610; pGsm610 = (GSM610WAVEFORMAT*)&pFrom.audio.ah_compress_recvd.wfx;
							pWf_compress = (WAVEFORMATEX*)mymalloc(sizeof(pFrom.audio.ah_compress_recvd.wfx) + pFrom.audio.ah_compress_recvd.wfx.cbSize  );
							if (!pWf_compress) goto errLabel;
							memcpy(pWf_compress, &pFrom.audio.ah_compress_recvd.wfx, sizeof(pFrom.audio.ah_compress_recvd.wfx) + pFrom.audio.ah_compress_recvd.wfx.cbSize  );
							pWf_pcm = (WAVEFORMATEX*)mymalloc(sizeof(WAVEFORMATEX));
							if (!pWf_pcm) goto errLabel;
							makeWaveFormat_pcm(1, 8000, 16, pWf_pcm);
							pWf_decompress = (WAVEFORMATEX*)mymalloc(sizeof(WAVEFORMATEX));
							if (!pWf_decompress) goto errLabel;
							memcpy(pWf_decompress, &pFrom.audio.ah_decompress_recvd.wfx, sizeof(WAVEFORMATEX));
							break;
						default:
							traceLog(_T("initTaskAvFrom_audio failed: wFormatTag not supported"));
							goto errLabel;
					}
#endif

					if (!pWf_compress->nAvgBytesPerSec) goto errLabel;
					if (!pWf_pcm->nAvgBytesPerSec) goto errLabel;
					if (pWf_decompress->wFormatTag != WAVE_FORMAT_PCM) goto errLabel;
					if (!pWf_decompress->nAvgBytesPerSec) goto errLabel;

					try
					{
						mmr = acmStreamOpen(&hstr1, had, pWf_compress, pWf_pcm, null, null, 0, ACM_STREAMOPENF_NONREALTIME);
					}
# ifdef  __useMfc__
					catch (CException* e)
					{
						e->Delete();
						mmr = -1;
					}
#endif
					catch (  ...  )  {
						mmr = -1;
					}
					if (mmr)
					{
						hstr1 = null; goto errLabel;
					}
					try
					{
						mmr = acmStreamOpen(&hstr2, had, pWf_pcm, pWf_decompress, null, null, 0, ACM_STREAMOPENF_NONREALTIME);
					}
# ifdef  __useMfc__
					catch (CException* e)
					{
						e->Delete();
						mmr = -1;
					}
#endif
					catch (  ...  )  {
						mmr = -1;
					}
					if (mmr)
					{
						hstr2 = null; goto errLabel;
					}

					//
					pTransform.audio.u.acm.hadid = hadid;
					pTransform.audio.u.acm.hstr1 = hstr1; hstr1 = null;
					pTransform.audio.u.acm.hstr2 = hstr2; hstr2 = null;
					pTransform.audio.pWf_compress = pWf_compress; pWf_compress = null;
					pTransform.audio.pWf_pcm = pWf_pcm; pWf_pcm = null;
					pTransform.audio.pWf_decompress = pWf_decompress; pWf_decompress = null;
#else
					if (initDecompressAudio_filterGraph(&pFrom.audio.compressor, &pTransform.audio.ah_compress, &pTransform.audio.ah_decompress, &pTransform.audio.u.dmo))
					{
						traceLogA("initTaskAvFrom_audio: initDecompressAudio failed.");
						goto errLabel;
					}
#endif
					*/
                    traceLog("acm not supported");
                    goto errLabel;
                }
                else if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_dmo)
                {
                    traceLog("dmo not supported");
                    goto errLabel;

                }
                else
                {
                    CUS_MODULE_compress pModule = null;
                    uint uiModuleType = 0;             //  2012/09/09


                    /*
						switch (pTransform.audio.ta.compressor.ucCompressors)
						{
							case Consts.CONST_audioCompressors_ipp:
								//pModule  =  pQyMc->cusModules.pIpp;
								uiModuleType = CONST_moduleType_ipp;    //  2014/09/01
								break;
							case CONST_audioCompressors_ffmpeg:
								uiModuleType = CONST_moduleType_ipp;
								break;
							case CONST_audioCompressors_vorbis:
								uiModuleType = CONST_moduleType_qdc_vorbis_dec; //  2016/12/05
								break;
							case CONST_audioCompressors_custom:
								// pModule  =  getCusModule(  pProcInfo->pQyMc,  CONST_moduleType_compress  );								
								uiModuleType = CONST_moduleType_compress;
								break;
							default:
								traceLogA("initTaskAvFrom_audio: ucCompressors %d, error", pTransform.audio.ta.compressor.common.ucCompressors);
								goto errLabel;
								break;
						}
					*/
                    uiModuleType = Consts.CONST_moduleType_ffmpeg;
                    //
                    adjustModuleType_decA(ref uiModuleType);
                    //
                    M_getCompressModule(pQyMc.cusModules, uiModuleType, ref pModule); //  2012/09/09
                    if (null == pModule || null == pModule.pf_qdcInitDecompressAudio)
                    {
                        /*
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s pModule or initDecA is null. moduleType %d"), tHintBuf, uiModuleType);
#if  true
				  //
				  TCHAR  tBuf[128];
				  _sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "decA: wFormatTag: 0x%x not supported"  ),  (  int  )pTransform.audio.ta.ah_compress.wfx.wFormatTag  );
				  showNotification(  0,  0,  0,  0,  0,  0,  tBuf  );
#endif
						*/

                        goto errLabel;
                    }
                    //  2016/12/05
                    pTransform.audio.uiModuleType_toDec = uiModuleType;

                    ///////////////////////////
                    //  2016/12/10
                    //  2014/03/25
                    AUDIO_COMPRESSOR_CFG compressor = new AUDIO_COMPRESSOR_CFG();
                    //memcpy(&compressor, &pFrom.audio.ta_recvd.compressor, sizeof(compressor));
                    compressor.common.head = pActiveMem_from.avStream.obj.tranInfo.audio.compressor;


                    //compressor.common.pCurPkt1 = pTransformAudio.pCurPkt;

                    //compressor.common.size_pCurPkt = sizeof(myPLAY_AUDIO_DATA);
                    //
                    //compressor.common.pInDataCacheQ  =  pTransformAudio.pInDataCacheQ;
                    //
                    compressor.common.pAudioQ2 = pTransformAudio.q2;
                    //
                    //compressor.common.pTmpDataQ  =  pTransformAudio.pTmpDataQ;
                    //
                    compressor.common.pParent_transform = pTransform;
                    compressor.common.uiTransformType_pParent = Consts.CONST_uiTransformType_dec;

                    //
                    compressor.common.pf_bTransformAudioQuit = bTransformAudioQuit;
                    //compressor.common.pf_transformGetSpsPps  =  transformGetSpsPps;
                    //compressor.common.pf_transformGetWfx = (PF_commonHandler)transformGetWfx;
                    //compressor.common.pf_getNextAPkt = getNextAPkt;


                    //compressor.common.pf_parseVPkt_i  =  dec_parseEncVPkt_i;
                    //compressor.common.pf_parseDecAPkt_o = parseDecAPkt_o;


                    //compressor.common.pf_detachPkt = detachAPkt;
                    //compressor.common.pf_clean_myPLAY_AUDIO_DATA = (PF_clean_myPLAY_AUDIO_DATA)clean_myPLAY_AUDIO_DATA;


                    //  2014/09/07
                    compressor.common.iDecParam_index_pMems_from = -1;// index_pMems_from;
                                                                      //  2014/07/09
                    compressor.common.dec.pf_smplBitstreamReader_ReadNextFrame_func = smplBitstreamReader_ReadNextFrame_func_a;



                    //  2015/01/17
#if false
					compressor.common.dec.pf_smplPCMWriter_WriteNextFrame_pre = smplPCMWriter_WriteNextFrame_pre;   //  2015/01/17
						compressor.common.dec.pf_smplPCMWriter_WriteNextFrame0 = smplPCMWriter_WriteNextFrame0;         //  2014/07/23
						compressor.common.dec.pf_smplPCMWriter_WriteNextFrame1 = smplPCMWriter_WriteNextFrame1;         //  2014/07/23
																														//
						compressor.common.dec.pf_smplPCMWriter_WriteNextFrame_all = smplPCMWriter_WriteNextFrame_all;       //  2014/07/23
#endif

                    //
                    //compressor.common.dec.pf_updateDecVideoParam  =  updateDecVideoParam;
                    //
                    //compressor.common.dec.pf_postAPktToRender = (PF_commonHandler)postAPktToRender;


                    /////////////////////////

                    //  2016/12/25
                    pTransformAudio.u_qoi.myQoi.common.uiType = Consts.CONST_qoiType_decA;



                    //  2016/12/25				
                    //pProcInfo->set_qoi_funcs(&pTransformAudio.u.qoi.myQoi);



                    //
                    if (0 != pModule.pf_qdcInitDecompressAudio(ref compressor, ref pTransform.audio.ta.ah_compress, ref pTransform.audio.ta.ah_decompress, ref pTransform.audio.u_qoi.myQoi.common))
                    {
                        traceLogA("initTaskAvFrom_video: initDmoDecompressVideo failed.");
                        goto errLabel;
                    }
                }
                //
                //  if  (  !(  pTransferAudioData  =  (  TRANSFER_AUDIO_DATA  *  )mymalloc(  sizeof(  TRANSFER_AUDIO_DATA  )  )  )  )  goto  errLabel;
                //  memset(  pTransferAudioData,  0,  sizeof(  TRANSFER_AUDIO_DATA  )  );

                //  pFrom.audio.pDecompressBuf_transferAudioData  =  pTransferAudioData;	pTransferAudioData  =  null;

                //  2015/05/19
                if (pTask.saveVideo.ucbSaveVideo_req)
                {
                    /*
						pTransformAudio.saveAudio.uiSize_pSaveAudioPkt = sizeof(SAVE_audio_pkt);
						if (pTransformAudio.saveAudio.pSaveAudioPkt)
						{
							qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initTaskAvFrom_audio failed: pSaveAudioPkt is not null."));
							goto errLabel;
						}
						pTransformAudio.saveAudio.pSaveAudioPkt = mymalloc(pTransformAudio.saveAudio.uiSize_pSaveAudioPkt);
						if (!pTransformAudio.saveAudio.pSaveAudioPkt) goto errLabel;
						memset(pTransformAudio.saveAudio.pSaveAudioPkt, 0, pTransformAudio.saveAudio.uiSize_pSaveAudioPkt);
					*/
                }


                //
                pTransform.audio.index_activeMems_from = index_activeMems_from;

                pTransform.audio.ucbInited = true;
            }


            iErr = 0;

        errLabel:

            /*
					if (hstr1)
					{
						acmStreamClose(hstr1, 0); hstr1 = null;
					}
					if (hstr2)
					{
						acmStreamClose(hstr2, 0); hstr2 = null;
					}
			*/

            /*
            if (null != pWf_compress) myfree((IntPtr)pWf_compress);
            if (null != pWf_pcm) myfree((IntPtr)pWf_pcm);
            if (null != pWf_decompress) myfree((IntPtr)pWf_decompress);
            */

            //  if  (  pTransferAudioData  )  free(  pTransferAudioData  );

            if (0 != iErr)
            {   //  2012/03/20
                exitTaskAvFrom_audio(pProcInfo, pTransform, "initTaskAvFrom_audio failed");
            }

            //  if  (  iErr  ||  tHintBuf[0]  )  
            {
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initTaskAvFrom_audio %d: %I64u %s: %s"), pTransform.index_activeMems_from, pFrom.idInfo.ui64Id, (iErr ? _T("failed") : _T("")), tHintBuf);
            }

            //traceLogA("initTaskAvFrom_audio: %I64u leaves", pFrom.idInfo.ui64Id);

            return iErr;
        }







        //  void  exitTaskAvFrom_audio(  MC_VAR_isCli  *  pProcInfo,  TASK_AV_FROM  *  pFrom  )
        public static unsafe void exitTaskAvFrom_audio(CCtxQmc pProcInfo, QY_TRANSFORM pTransform, string hint)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;// QY_GET_GBUF();
                                       //  if  (  !pFrom  ||  !pFrom.idInfo.ui64Id  )  return;
            int i;

            //
            if (hint == null) hint = "";


            //
            CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
            if (null == pCusModules)
            {
                //MACRO_qyAssert(0, _T("exitTaskAvFrom_video: pCusModules is null"));
                return;
            }

            //  traceLogA(  "exitTaskAvFrom_audio: %I64u enters",  pFrom.idInfo.ui64Id  );

            //  if  (  pFrom.audio.ucbInited  )  
            {
                if (0 == pTransform.audio.ta.compressor.ucCompressors) ;
                else if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_acm)
                {
                    /*
# ifdef  __SUPPORT_acmToDec__
                    if (pTransform.audio.u.acm.hstr1)
                    {
                        acmStreamClose(pTransform.audio.u.acm.hstr1, 0); pTransform.audio.u.acm.hstr1 = null;
                    }
                    if (pTransform.audio.u.acm.hstr2)
                    {
                        acmStreamClose(pTransform.audio.u.acm.hstr2, 0); pTransform.audio.u.acm.hstr2 = null;
                    }
#else
            exitDecompress_filterGraph(&pTransform.audio.u.dmo);
#endif
                    */
                    traceLog("acm not supported");
                }
                else if (pTransform.audio.ta.compressor.ucCompressors == Consts.CONST_audioCompressors_dmo)
                {
                    //exitDecompressAudio_dmo(&pTransform.audio.u.dmo);
                    traceLog("dmo not supported");
                }
                else
                {
                    CUS_MODULE_compress pModule = null;
                    /*
                    switch  (  pTransform.audio.ta.compressor.common.ucCompressors  )  {
                            case  CONST_audioCompressors_ipp:
                                  pModule  =  pQyMc->cusModules.pIpp;
                                  break;
                            case  CONST_audioCompressors_vorbis:	//  2016/12/05
                                  pModule  =  pQyMc->cusModules.pQdc_vorbis_dec;
                                  break;
                            case  CONST_audioCompressors_custom:
                                  pModule  =  getCusModule(  &pQyMc->cusModules,  CONST_moduleType_compress  );
                                  break;
                            default:
                                traceLogA(  "exitTaskAvFrom_audio: ucCompressors error, %d",  pTransform.audio.ta.compressor.common.ucCompressors  );
                                qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "exitTaskAvFrom_audio: ucCompressors error, %d"  ),  (  int  )pTransform.audio.ta.compressor.common.ucCompressors  );
                                   break;
                    }
                    */
                    M_getCompressModule(pCusModules, pTransform.audio.uiModuleType_toDec, ref pModule);    //  2012/09/09


                    if (null != pModule)
                    {
                        if (null != pModule.pf_qdcExitDecompressAudio)
                        {
                            pModule.pf_qdcExitDecompressAudio(ref pTransform.audio.u_qoi.myQoi.common);
                        }
                        //

                    }
                }

                //  2015/05/19
                MACRO_safeFree(ref pTransform.audio.saveAudio.pSaveAudioPkt);

                //  2010/05/04
                //memset(&pTransform.audio.u, 0, sizeof(pTransform.audio.u)  );
                pTransform.audio.u_qoi.myQoi.common.memset0();

                //
                IntPtr tmpp;
                /*
                if (null != pTransform.audio.pWf_compress)
                {
                    tmpp = (IntPtr)pTransform.audio.pWf_compress;
                    M_free(ref tmpp); pTransform.audio.pWf_compress = (WAVEFORMATEX*)tmpp;
                }
                if (null != pTransform.audio.pWf_pcm)
                {
                    tmpp = (IntPtr)pTransform.audio.pWf_pcm;
                    M_free(ref tmpp); pTransform.audio.pWf_pcm = (WAVEFORMATEX*)tmpp; ;
                }
                if (null != pTransform.audio.pWf_decompress)
                {
                    tmpp = (IntPtr)pTransform.audio.pWf_decompress;
                    M_free(ref tmpp); pTransform.audio.pWf_decompress = (WAVEFORMATEX*)tmpp;
                }

                if (null != pTransform.audio.pBuf_pcm)
                {
                    tmpp = (IntPtr)pTransform.audio.pBuf_pcm;
                    M_free(ref tmpp); pTransform.audio.pBuf_pcm = (byte*)tmpp;
                }
                */

                //  if  (  pFrom.audio.pDecompressBuf_transferAudioData  )  M_free(  pFrom.audio.pDecompressBuf_transferAudioData  );

                //  2012/03/19
                fixed (TRANSFORM_AUDIO_procInfo_status* pstatus = &pTransform.audio.status)
                {
                    mymemset((IntPtr)pstatus, 0, sizeof(TRANSFORM_AUDIO_procInfo_status));
                }

                //
                pTransform.audio.ucbInited = false;
            }

            //  errLabel:

            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("exitTaskAvFrom_audio %d"), pTransform.index_activeMems_from);
            int index_activeMems_from = pTransform.index_activeMems_from;
            string str;
            str = string.Format("exitTaskAvFrom_audio {0}. {1}", index_activeMems_from, hint);
            qyFuncs.showInfo_open(0, null, str);

            //  traceLogA(  "exitTaskAvFrom_audio: %I64u leaves",  pFrom.idInfo.ui64Id  );
            traceLogA("exitTaskAvFrom_audio leaves");


            return;
        }


        //
        public static unsafe int msgInput_ntoh(CTX_MC_thread pCtx,MIS_CNT  pMisCnt, ref MIS_MSGU  pMsgU,  ref MIS_MSG_INPUT pMsgInput, ref int msgLen )
        {
            int iErr = -1;
            TMP_tHintBuf tHintBuf;

          
            //
            if  (  pMsgInput.ucbConvrted)
            {
                goto errLabel;
            }

            //
            if (!pMsgInput.ucbConvrted)
            {


                if (null == pCtx.pMsgBuf_forInput)
                {
                    qyFuncs.showInfo_open(0, "TAG", "msgInput_ntoh failed, null == p_Ctx.pMsgBuf_forInput");
                    goto errLabel;
                }
                fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
                {
                    MIS_MSG_INPUT* pMsg = (MIS_MSG_INPUT*)pMsgBuf_forInput;
                    if (null == pMsg)
                    {
#if DEBUG
                        traceLog("procMsgInput_other_req failed: ctx.pMsgInput is null");
#endif
                        goto errLabel;

                    }


                    MACRO_memset_misMsgInput(pMsg);
                    fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                    {
                        mymemcpy((IntPtr)pMsg, (IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
                    }
                    msgLen = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data");

                    if (isUcFlgTalkData(pMsgInput.ucFlg) || isUcFlgRouteTalkData(pMsgInput.ucFlg))
                    {
                        MIS_MSG_routeTalkData* pAddrTalkData1 = &pMsg->data;
                        //
                        fixed (MIS_MSG_routeTalkData* data = &pMsgInput.data)
                        {
                            if (0 != qyntohRouteTalkData(pMsgInput.ucFlg, (byte*)data, pMsgInput.lenInBytes, &pAddrTalkData1->route, (IM_CONTENTU*)&pAddrTalkData1->buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
                            {
                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo.str_who_showInfo, 0, (""), (""), ("    qyntohRouteTalkData failed. %s"), tHintBuf);
                                goto errLabel;
                            }
                        }
                        //if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    %s"), tHintBuf);
                        //
                        msgLen += (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
                        //
                        if (0 != getIdInfo_logicalPeer(pMisCnt, ref pAddrTalkData1->route, ref pMsg->addr_logicalPeer)) goto errLabel;
                        //
                        //pContent = (IM_CONTENTU*)&pAddrTalkData->buf;
                    }
                    else
                    {
                        fixed (MIS_MSG_routeTalkData* data = &pMsgInput.data)
                        {
                            if (0 != qyntohRouteTalkData(pMsgInput.ucFlg, (byte*)data, pMsgInput.lenInBytes, null, (IM_CONTENTU*)&pMsg->data, tHintBuf.tHintBuf, Consts.cntof_tHintBuf)) goto errLabel;
                        }
                        //if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    %s"), tHintBuf);
                        //
                        //pContent = (IM_CONTENTU*)&pMsg->data;
                    }
                    pMsg->ucbConvrted = true;

                    //

                    //
                    //pMsgU = ref *pCtx.pMsgBuf_forInput;
                    //pMsgInput = ref *pMsg;
                }

            }


            iErr = 0;
        errLabel:


            

            return iErr;
        }


        public static unsafe int sizeof_MIS_MSG_routeTalkData()
        {
            return sizeof(MIS_MSG_routeTalkData);
        }

        public static unsafe int sizeof_StatusConfLayoutParam()
        {
            return sizeof(StatusConfLayoutParam);
        }
        public static unsafe int sizeof_TASK_INTERACTION_REQ()
        {
            return sizeof(TASK_INTERACTION_REQ);
        }

        public static unsafe int sizeof_TRANSFER_FILEEND_REQ()
        {
            return sizeof(TRANSFER_FILEEND_REQ);
        }

        public static unsafe int sizeof_TRANSFER_FILEEND_RESP()
        {
            return sizeof(TRANSFER_FILEEND_RESP);
        }

        public static unsafe int sizeof_TRANSFER_AUDIO_DATA()
        {
            return sizeof(TRANSFER_AUDIO_DATA);
        }

        public static unsafe int sizeof_TRANSFER_VIDEO_DATA()
        {
            return sizeof(TRANSFER_VIDEO_DATA);
        }

        public static unsafe int sizeof_ConfNvrInfo()
        {
            return sizeof(ConfNvrInfo);
        }

        public static unsafe int sizeof_CONF_LAYOUT()
        {
            return sizeof(CONF_LAYOUT);
            return 0;
        }

        public static unsafe int sizeof_CONF_KEY()
        {
            return sizeof(CONF_KEY);
        }



        //////////////////
        ///
        //public static unsafe int procMsgInput_other_req(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_MSG_INPUT* pMsgInput, uint lenInBytes_msg, CTX_MC_thread pCtx, MIS_CHANNEL pChannel,string hint)
        public static int procMsgInput_other_req(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU  pMsgU, uint lenInBytes_msg, CTX_MC_thread pCtx, MIS_CHANNEL pChannel, string hint)
        {
            int iErr = -1;


            //
#if DEBUG
            qyFuncs.traceLog("procMsgInput_other_req enters");
#endif

            //
            if (  pMsgU.uiType!=Consts.CONST_misMsgType_input)
            {
                return -1;
            }
            ref MIS_MSG_INPUT pMsgInput = ref pMsgU.input;


            //
            int dataLen = 0;
            int len = 0;
            MIS_CNT pMisCnt = null;
            //MIS_MSG_routeTalkData* pAddrTalkData = null;
            int msgLen = 0;
            //IM_CONTENTU* pContent = null;
            uint uiChannelType = Consts.CONST_channelType_robot;
            //  CWnd					*	pMainWnd		=	AfxGetApp(  )->m_pMainWnd;

            //
            string str;




            //
            //using (CQyMalloc mallocObj = new CQyMalloc())
            {
                //
                int lenInBytes = 0;
                //
                //TCHAR tHintBuf[255 + 1] = _T("");
                //TMP_tHintBuf tHintBuf;
                bool bRemoveIfErr = true;
                //  2008/11/19
                //MSG_ROUTE route1;
                byte ucFlg = 0;

                QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
                if (null == pDbFuncs) goto errLabel;
                QM_dbFuncs g_dbFuncs = pDbFuncs;

                pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;// pMsgInput->pMisCnt;

                if (isUcFlgResp(pMsgInput.ucFlg)) goto errLabel;

                //  	 
                if (!pMsgInput.ucbConvrted)
                {
                    //
                    if  (  pChannel.uiType==Consts.CONST_channelType_realTimeMedia)
                    {
                        int ii = 0;
                    }

                    int tmpiRet = -1;
                    tmpiRet = msgInput_ntoh(pCtx, pMisCnt, ref pMsgU,  ref pMsgInput, ref msgLen);
                    if  (  tmpiRet !=0)
                    {
                        goto errLabel;
                    }
                    //
                    {
                        pMsgU = ref pCtx.pMsgBuf_forInput.msgU;
                        pMsgInput = ref pMsgU.input; 

                    }

                    /*
                    MIS_MSG_INPUT* pMsg = (MIS_MSG_INPUT*)pCtx.pMsgBuf_forInput;
                    if (null == pMsg)
                    {
#if DEBUG
                        traceLog("procMsgInput_other_req failed: ctx.pMsgInput is null");
#endif
                        return -1;
                    }


                    MACRO_memset_misMsgInput(pMsg);
                    fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                    {
                        mymemcpy((IntPtr)pMsg, (IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
                    }
                    msgLen = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data");

                    if (isUcFlgTalkData(pMsgInput.ucFlg) || isUcFlgRouteTalkData(pMsgInput.ucFlg))
                    {
                        MIS_MSG_routeTalkData * pAddrTalkData1 = &pMsg->data;
                        //
                        fixed (MIS_MSG_routeTalkData* data = &pMsgInput.data)
                        {
                            if (0 != qyntohRouteTalkData(pMsgInput.ucFlg, (byte*)data, pMsgInput.lenInBytes, &pAddrTalkData1->route, (IM_CONTENTU*)&pAddrTalkData1->buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
                            {
                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo.str_who_showInfo, 0, (""), (""), ("    qyntohRouteTalkData failed. %s"), tHintBuf);
                                goto errLabel;
                            }
                        }
                        //if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    %s"), tHintBuf);
                        //
                        msgLen += (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
                        //
                        if (0 != getIdInfo_logicalPeer(pMisCnt, ref pAddrTalkData1->route, ref pMsg->addr_logicalPeer)) goto errLabel;
                        //
                        //pContent = (IM_CONTENTU*)&pAddrTalkData->buf;
                    }
                    else
                    {
                        fixed (MIS_MSG_routeTalkData* data = &pMsgInput.data)
                        {
                            if (0 != qyntohRouteTalkData(pMsgInput.ucFlg, (byte*)data, pMsgInput.lenInBytes, null, (IM_CONTENTU*)&pMsg->data, tHintBuf.tHintBuf, Consts.cntof_tHintBuf)) goto errLabel;
                        }
                        //if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    %s"), tHintBuf);
                        //
                        //pContent = (IM_CONTENTU*)&pMsg->data;
                    }
                    pMsg->ucbConvrted = true;

                    //
                    pMsgU = ref *pCtx.pMsgBuf_forInput;
                    pMsgInput = ref *pMsg;
                    */
                }

                ref MIS_MSG_routeTalkData pAddrTalkData = ref pMsgInput.data;// MACRO_msgRouteTalkData(pMsgInput->ucFlg, &pMsgInput->data);
                /*
                if (null == pAddrTalkData)
                {
#if __DEBUG__
					traceLogA("procMsgInput_other_req failed: pAddrTalkData null");
#endif
                    goto errLabel;
                }
                */
                //
                ref IM_CONTENTU pContent = ref pMsgInput.data.buf;// M_getMsgContent(pMsgInput->uiType,pMsgInput->ucFlg, &pMsgInput->data);
                //if (null == pContent) goto errLabel;


                if (0 != pAddrTalkData.route.debugInfo.tTime_start)
                {
                    pAddrTalkData.route.debugInfo.dwTickCnt_peer_recved = myGetTickCount(null);
                    pAddrTalkData.route.debugInfo.tTime_peer_recved = pMsgInput.tRecvTime;
                }

                //
                /*
				if (pQyMc->env.bDebugInfo && pProcInfo->cfg.debugStatusInfo.ucbViewIoStatus)
				{
					TCHAR who_from[32 + 1] = _T("");
					TCHAR tBuf[512] = _T("");
					LPCTSTR where_showInfo = qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType);
					int iAfter = GetTickCount() - pMsgInput->dwTickCnt_start;
					//
					_sntprintf(who_from, mycountof(who_from), _T("%I64u"), pAddrTalkData->route.idInfo_from.ui64Id);
					if (isUcFlgResp(pMsgInput->ucFlg)) _sntprintf(tBuf, mycountof(tBuf), _T("  tn %d, resp: %s, dataLen %d, iAfter_recvd %d."), pMsgInput->uiTranNo, qyGetDesByType1(CONST_qyRcTable, pMsgInput->usCode), pMsgInput->lenInBytes, iAfter);
					else _sntprintf(tBuf, mycountof(tBuf), _T(" tn %d, req: %s, dataLen %d, iAfter_recvd %d."), pMsgInput->uiTranNo, qyGetDesByType1(CONST_qyCmdTable, pMsgInput->usCode), pMsgInput->lenInBytes, iAfter);
					if (pAddrTalkData->route.debugInfo.index) _sntprintf(tBuf, mycountof(tBuf), _T("%s index %d."), tBuf, pMsg->data.route.debugInfo.index);
					if (pContent->uiType == CONST_imCommType_transferVideoData)
					{
						_sntprintf(tBuf, mycountof(tBuf), _T("%s cnt %d."), tBuf, (int)pContent->transferVideoData.usCnt);
					}
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), who_from, pProcInfo->who_showInfo, where_showInfo, _T(""), _T("%s"), tBuf);
				}
				*/

                //  ÏÂÃæ´¦ÀíÇëÇó
                //
                switch (pContent.uiType)
                {

                    /*
					case CONST_imCommType_transferFileReq:
						{
							TRANSFER_FILE_REQ* pReq = NULL;
							//
							pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_FILE_REQ)  );
							//
							pReq = (TRANSFER_FILE_REQ*)&pContent->transferFileReq;
							//
							//  ÏÂÃæÊÇÄÚ²¿´¦Àí±äÁ¿
							lstrcpyn(pReq->fullFilePath, pReq->fileName, mycountof(pReq->fullFilePath));
							//
							pMsgInput->uiType = CONST_misMsgType_task;
							//
							pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
							pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
							//
							len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;
							//
							if (procMsgTask_mc_robot(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
						}
						break;
					case CONST_imCommType_transferFileReplyReq:
						{
							TRANSFER_FILE_REPLYREQ* pReq = NULL;
							//
							pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_FILE_REPLYREQ)  );
							//
							pReq = (TRANSFER_FILE_REPLYREQ*)&pContent->transferFileReplyReq;
							//
							pMsgInput->uiType = CONST_misMsgType_task;
							//
							pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
							pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
							//
							len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

							if (procMsgTask_mc_robot(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
						}
						break;
					*/
                    case Consts.CONST_imCommType_transferFileDataReq:
                        /*
                        {
                            //  TRANSFER_FILEDATA_REQ		req;
                            //
                            //  memset(  &req,  0,  sizeof(  req  )  );
                            //  if  (  stream2Data(  &ptr,  &size,  tmpHandler_stream2TransferFileDataReq,  0,  &req  )  )  goto  errLabel;
                            //  memcpy(  pContent,  &req,  min(  sizeof(  pContent[0]  ),  sizeof(  req  )  )  );
                            // 
                            pMsgInput->lenInBytes = (uint)Math.Min(sizeof(MIS_MSG_routeTalkData), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof(TRANSFER_FILEDATA_REQ));

                            TRANSFER_FILEDATA_RESP resp;
                            //
                            mymemset((IntPtr)(byte*)&resp, 0, (int)Marshal.OffsetOf<TRANSFER_FILEDATA_RESP>("buf"));
                            resp.uiType = Consts.CONST_imCommType_transferFileDataResp;

                            if (null == pCtx.pProcessQ) goto errLabel;
                            //
                            //printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
                            //
                            traceLogA("Now print pMsgInput");
                            printMisMsg(null, null, (MIS_MSGU*)pMsgInput);

                            if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsgInput, (IntPtr)(byte*)&resp) <= 0)
                            {
                                traceLogA("Ã»ÓÐÕÒµ½Ïà¹ØµÄÈÎÎñ½Úµã»ò´¦Àí³ö´í£¬²»ÄÜ¼ÌÐø");
                                goto errLabel;
                            }
                            lenInBytes = sizeof(TRANSFER_FILEDATA_RESP);

                            //  2009/02/25
                            startChannel(pProcInfo, pMisCnt, pChannel, _T("proc transferFileDataReq"));

                            //
                            ucFlg = 0;
                            mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
                            route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
                            if (true)
                            {
                                ucFlg |= Consts.CONST_commFlg_routeTalkData;
                                route.debugInfo.dwTickCnt_me_start = myGetTickCount(null);
                                Int64 tNow; mytime(&tNow); route.debugInfo.tTime_start = tNow;
                            }
                            ucFlg |= Consts.CONST_commFlg_resp;        //  ×¢Òâ£¬ÕâÀïÊÇÏìÓ¦
                            route.debugInfo.index = (int)resp.uiDataBlockIndex;

                            if (0 != postMsg2Mgr_mc(null, pMisCnt, &route, Consts.CONST_misMsgType_outputTask, ucFlg, Consts.CONST_qyCmd_sendRobotTaskData, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)&resp, (uint)lenInBytes, &pMsgInput->addr_logicalPeer.idInfo, &pAddrTalkData->route.idInfo_from, uiChannelType, null, false)) goto errLabel;

                        }
                        */
                        break;

                    case Consts.CONST_imCommType_transferFileEndReq:
                        {

                            //  TRANSFER_FILEEND_REQ    req;

                            traceLogA("transferFileEndReq");

                            //  memset(  &req,  0,  sizeof(  req  )  );
                            //  if  (  stream2Data(  &ptr,  &size,  tmpHandler_stream2TransferFileEndReq,  0,  &req  )  )  goto  errLabel;                                 
                            //  memcpy(  pContent,  &req,  min(  sizeof(  pContent[0]  ),  sizeof(  req  )  )  );

                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_TRANSFER_FILEEND_REQ());

                            if (null == pCtx.pProcessQ) goto errLabel;
                            //
                            //printQ_mis(  &pCtx->pProcessQ->m_q,  0  );

                            //
                            //printMisMsg(null, null, (MIS_MSGU*)pMsgInput);

                            CTRANSFER_FILEEND_RESP resp = new CTRANSFER_FILEEND_RESP();
                            //
                            //mymemset((IntPtr)(byte*)&resp, 0, sizeof(TRANSFER_FILEEND_RESP));
                            resp.transferFileEndResp.uiType = Consts.CONST_imCommType_transferFileEndResp;
                            //
                            PARAM_findOrgMsg param = new PARAM_findOrgMsg();
                            param.pTransferFileEndResp = resp;
                            //
                            if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param) <= 0) break;
                            if (resp.transferFileEndResp.tmpInternal.bNeedRemoved_o)
                            {
                                //  pCtx->pProcessQ->qRemoveMsg(  tmpHandler_findOrgMsgAndProc_is,  pMsgInput,  (IntPtr)0  );
                                qmcApplyForRemovingInvalidTasks(pProcInfo, 0);  //  2015/09/07
                            }

                            //
                            //printQ_mis(  &pCtx->pProcessQ->m_q,  0  );

#if __DEBUG__
							traceLogA("end");
#endif

                        }
                        break;
                    /*
                case CONST_imCommType_taskProcReq:
                    {

                        //  TRANSFER_FILE_PROCREQ		req;

# ifdef  __DEBUG__
                        traceLogA("procMsgInput_other_req: taskProcReq");
#endif

                        //  memset(  &req,  0,  sizeof(  req  )  );
                        //  if  (  stream2Data(  &ptr,  &size,  tmpHandler_stream2TransferFileProcReq,  0,  &req  )  )  goto  errLabel;
                        //  memcpy(  pContent,  &req,  min(  sizeof(  pContent[0]  ),  sizeof(  req  )  )  );

                        pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_FILE_PROCREQ)  );

                        if (!pCtx->pProcessQ)
                        {
# ifdef  __DEBUG__
                            traceLogA("procMsgInput_other_req error: taskProcReq: pProcessQ is null");
#endif
                            goto errLabel;
                        }
# ifdef  __DEBUG__
                        //  printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
                        //  printMisMsg(  0,  0,  (  MIS_MSGU  *  )pMsgInput  );
#endif


#if  0  //  2014/08/28
              if  (  pContent->taskProcReq.uiTaskType  ==  CONST_imTaskType_viewDynBmp   )  {		//  2009/09/11

                  pMsgInput->uiType  =  CONST_misMsgType_task;
                  pMsgInput->uiTaskType  =  pContent->taskProcReq.uiTaskType;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id  =  pAddrTalkData->route.idInfo_from.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id  =  pAddrTalkData->route.idInfo_to.ui64Id;
                  //
                  len  =  offsetof(  MIS_MSG_TASK,  data  )  +  pMsgInput->lenInBytes;

                  if  (  qPostMsg(  pMsgInput,  len,  &pQyMc->gui.processQ  )  )  {
                      traceLogA(  "mcThreadProc_mis_work(  ): qPostMsg(  ) failed"  );
                      goto  errLabel;
                  }
                  PostMessage(  pQyMc->gui.hMainWnd,  CONST_qyWm_postComm,  CONST_qyWmParam_msgArrive,  0  );    
                  break;
              }
#endif

                        TASK_PROC_RESP resp;
                        //  
                        memset(&resp, 0, sizeof(resp));
                        resp.uiType = CONST_imCommType_taskProcResp;
                        if (pCtx->pProcessQ->qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsgInput, &resp) <= 0)
                        {
                            //  
                            //  
                            if (pContent->taskProcReq.usOp == CONST_imOp_send_cancel)
                            {
                                void* pDb = NULL;
                                CE_TASK_MEM taskMem;
                                int iStatus = CONST_imTaskStatus_canceledBySender;
                                CQyString subject;
                                CQyString content;

# ifndef  __NOTSUPPORT_DB__
                                CQnmDb db;
                                if (!db.getAvailableDb(pQyMc->iDsnIndex_mainSys)) goto errLabel;
                                pDb = db.m_pDbMem->pDb;

                                IM_TASK_RCD rcd;
                                if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pSci->cfg.iDbType, FALSE, pMisCnt->cfg.misServName, &pAddrTalkData->route.idInfo_from, (time_t)pContent->taskProcReq.tStartTime_org, pContent->taskProcReq.uiTranNo_org, 0, &rcd)) goto errLabel;

                                memset(&taskMem, 0, sizeof(taskMem));
                                taskMem.iStatus = rcd.iStatus;
                                taskMem.iTaskId = rcd.id;
                                taskMem.idInfo_send.ui64Id = rcd.idInfo_send.ui64Id;
                                taskMem.idInfo_recv.ui64Id = rcd.idInfo_recv.ui64Id;
                                subject = rcd.subject;
                                content = rcd.content;
#else
                                if (getCeTaskMem(pDb, pSci->cfg.iDbType, pMisCnt, &pAddrTalkData->route.idInfo_from, pContent->taskProcReq.tStartTime_org, pContent->taskProcReq.uiTranNo_org, 0, &taskMem)) goto errLabel;
#endif

                                //  2014/11/22
                                switch (taskMem.iStatus)
                                {
                                    case CONST_imTaskStatus_applyToRecv:
                                    case CONST_imTaskStatus_resp:
                                    case CONST_imTaskStatus_waitToRecv:
                                        break;
                                    default:
                                        iErr = 0; goto errLabel;
                                        break;  //  
                                }

                                //  
                                if (g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, taskMem.iTaskId)) goto errLabel;
                                //
#if  0	//  2014/11/22. 在代理的工作模式下，不能用route得到logicalpeer
                      if  (  !pMsgInput->addr_logicalPeer.idInfo.ui64Id  )  {	//  
                          if  (  getIdInfo_logicalPeer(  pSci,  pMisCnt,  &pMsgInput->data.route,  &pMsgInput->addr_logicalPeer  )  )  {
# ifdef  __DEBUG__
                                      traceLogA(  "procMsgInput_other_req: getIdInfo_logicalPeer failed."  );  
#endif
                                goto errLabel;
                            }
                        }
#endif

                        //
                        showTaskStatus(pMisCnt, &pMsgInput->addr_logicalPeer.idInfo, &taskMem.idInfo_send, &taskMem.idInfo_recv, FALSE, pContent->taskProcReq.uiContentType_org, taskMem.iTaskId, iStatus, 0, 0, subject, content);
                        // 

                    }

                    break;      //  
            }

            if (pContent->taskProcReq.usOp == CONST_imOp_recv_applyForChkTaskAlive)
            {

                qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("Resp to %I64u, task is alive"), pMsgInput->data.route.idInfo_from.ui64Id);

                resp.tStartTime_org = pContent->taskProcReq.tStartTime_org;
                resp.uiTranNo_org = pContent->taskProcReq.uiTranNo_org;
                resp.usCmd_org = pMsgInput->usCode;
                resp.usOp_org = pContent->taskProcReq.usOp;
                //
                lenInBytes = sizeof(resp);
                //
                ucFlg = 0;
                ucFlg |= CONST_commFlg_resp;        //  ×¢Òâ£¬ÕâÀïÊÇÏìÓ¦

                if (postMsg2Mgr_mc(pMisCnt, NULL, CONST_misMsgType_outputTask, ucFlg, CONST_qyRc_ok, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (char*)&resp, lenInBytes, &pMsgInput->addr_logicalPeer.idInfo, &pAddrTalkData->route.idInfo_from, pMsgInput->uiChannelType, pCtx->pMsgBuf, FALSE)) goto errLabel;

                break;
            }

            if (resp.bNeedRemoved)
            {
#if  0	//  2012/03/24. 这里不直接删除任务,都统一转移到chkTasks, 以便异步处理.
                        pCtx->pProcessQ->qRemoveMsg(  tmpHandler_findOrgMsgAndProc_is,  pMsgInput,  (IntPtr)0  );
#endif
# ifdef  __DEBUG__
                traceLog(_T("Need removed"));
#endif
            }

# ifdef  __DEBUG__
            //  printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
#endif

        }
        break;

        case  CONST_imCommType_rtcCallReq: {

            traceLogA("rtc not supported");

        }
        break;

        case  CONST_imCommType_transferAvInfo: {
            TRANSFER_AV_INFO* pReq = NULL;
            //
            if (pChannel->uiType == CONST_channelType_realTimeMedia) {  //  realTimeMedia 
                qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("Error: %s ÊÕµ½ %I64u µÄ %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pMsgInput->data.route.idInfo_from.ui64Id, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                goto errLabel;
            }
            //  2014/04/28
            qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T("procMsgInput_other_req"), _T(": recv transferAvInfo from %I64u"), pMsgInput->data.route.idInfo_from.ui64Id);

            //
            pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_AV_INFO)  );
            //
            pReq = (TRANSFER_AV_INFO*)&pContent->transferAvInfo;
            //  2014/09/28
            if (pReq->idInfo_logicalPeer.ui64Id)
            {
                if (pContent->transferAvInfo.uiTaskType == CONST_imTaskType_viewDynBmp)
                {

                    pMsgInput->uiType = CONST_misMsgType_task;
                    pMsgInput->uiTaskType = pContent->transferAvInfo.uiTaskType;

                    //	
                    pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                    pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                    //
                    len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

                    if (pReq->idInfo_logicalPeer.ui64Id)
                    {
                        if (!pMisCnt) goto errLabel;
                        if (pReq->idInfo_logicalPeer.ui64Id == pMisCnt->idInfo.ui64Id)
                        {
                            showInfo_open(0, 0, _T("procMsgInput_other_req failed, viewDynBmp, req.idInfo_logicalPeer is me"));
                            goto errLabel;
                        }
                        pMsgInput->addr_logicalPeer.idInfo.ui64Id = pReq->idInfo_logicalPeer.ui64Id;
                    }
                    else
                    {
                        pMsgInput->addr_logicalPeer.idInfo.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                    }

                    if (qPostMsg(pMsgInput, len, &pQyMc->gui.processQ))
                    {
                        traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
                        goto errLabel;
                    }
                    PostMessage(pQyMc->gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);

                    iErr = 0; goto errLabel;
                }
            }

            //
#if  0	//  2014/11/19. 不能修改req.idInfo_logicalPeer以及to,from。因为在avproxy时，需要用这几个参数来判断是否需要proxy. 处理时要利用addr_logicalPeer或者req.idInfo_logicalPeer
              if  (  pReq->idInfo_logicalPeer.ui64Id  )  {
                  traceLogA(  "procMsgInput_other_req: transferAvInfo, use idInfo_logicalPeer to set idInfo_to"  );
                  pAddrTalkData->route.idInfo_to.ui64Id  =  pReq->idInfo_logicalPeer.ui64Id;
                  pReq->idInfo_logicalPeer.ui64Id  =  0;
              }
#endif
            //
#if  10
              if  (  pReq->idInfo_logicalPeer.ui64Id  )  {
                  if  (  !pMisCnt  )  goto  errLabel;
                  if  (  pReq->idInfo_logicalPeer.ui64Id  ==  pMisCnt->idInfo.ui64Id  )  {
                      showInfo_open(  0,  0,  _T(  "procMsgInput_other_req failed, req.idInfo_logicalPeer is me"  )  );
                      goto  errLabel;
                  }
                  pMsgInput->addr_logicalPeer.idInfo.ui64Id  =  pReq->idInfo_logicalPeer.ui64Id;
              }
#endif
            //
            pMsgInput->uiType = CONST_misMsgType_task;
            //
            pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
            pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
            //
            len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;
            //
            if (procMsgTask_mc_media(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
        }
        break;

        case  CONST_imCommType_transferAvReplyInfo:
        {
            TRANSFER_AV_replyInfo* pReq = NULL;
            //
            if (pChannel->uiType == CONST_channelType_realTimeMedia)
            {   //  realTimeMedia 
                qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("Error: %s ÊÕµ½ %I64u µÄ %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pMsgInput->data.route.idInfo_from.ui64Id, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                goto errLabel;
            }
            //
            pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_AV_replyInfo)  );
            //
            pReq = (TRANSFER_AV_replyInfo*)&pContent->transferAvReplyInfo;
            //
            pMsgInput->uiType = CONST_misMsgType_task;
            //
            pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
            pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
            //
            len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

            if (procMsgTask_mc_media(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
        }
        break;
                */
                    case Consts.CONST_imCommType_transferVideoData:
                        {
                            //TRANSFER_VIDEO_DATA* pReq = null;

                            //
                            bRemoveIfErr = false;

                            //  2009/02/25
                            startChannel(pProcInfo, pMisCnt, pChannel, _T("proc transferVideoData"));

                            //
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_TRANSFER_VIDEO_DATA());
                            //
                            ref TRANSFER_VIDEO_DATA pReq = ref pContent.transferVideoData;
                            //
                            //pMsgInput->uiType = CONST_misMsgType_task;
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (int)pMsgInput.lenInBytes;

#if __DEBUG__
				static int kk = 0; kk++;
				//  traceLogA(  "here  kk %d",  kk  );
				if (0)
				{   // for test
					char timeBuf[CONST_qyTimeLen + 1] = "";
					//
					getTimelStr(pMsgInput->tStartTime, timeBuf, mycountof(timeBuf));
					//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  (  ""  ),  pProcInfo->who_showInfo,  NULL,  _T(  ""  ),  _T(  ""  ),  _T(  "%I64uµÄ%s, %s, tn [%d], avDev [%d], usFrames [%d]"  ),  pMsgInput->data.route.idInfo_from.ui64Id,  qyGetDesByType1(  getResTable(  0,  &pQyMc->cusRes,  CONST_resId_qyCommTypeTable  ),  pContent->uiType  ),  CQyString(  timeBuf  +  8  ),  pMsgInput->uiTranNo,  pContent->transferVideoData.uiTranNo_openAvDev,  pContent->transferVideoData.stat.usFrames_perSecond  );
				}
#endif

                            if (0 != procMsgInput_mc_media(pQyMc, pProcInfo, ref pMsgU, (uint)len, true, ref pCtx, pChannel))
                            {
                                qyFuncs.traceLog("procMsgInput_other_req.2061, procMsgInput_mc_media failed");
                                    
                                goto errLabel;
                            }

                            //
#if DEBUG
                            qyFuncs.traceLog("procMsgInput_other_req.2054, after procMsgInput_mc_media ");
#endif
                        }
                        break;

                    case Consts.CONST_imCommType_transferAudioData:
                        {
                            //TRANSFER_AUDIO_DATA* pReq = null;

                            //
                            bRemoveIfErr = false;

                            //  2009/02/25
                            startChannel(pProcInfo, pMisCnt, pChannel, _T("proc transferVideoData"));

                            //
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_TRANSFER_AUDIO_DATA());
                            //
                            ref TRANSFER_AUDIO_DATA pReq = ref pContent.transferAudioData;
                            //
                            //pMsgInput->uiType = CONST_misMsgType_task;
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (int)pMsgInput.lenInBytes;

                            if (0 != procMsgInput_mc_media(pQyMc, pProcInfo, ref pMsgU, (uint)len, true, ref pCtx, pChannel)) goto errLabel;
                        }
                        break;
                    /*
        case  CONST_imCommType_remoteAssistReq:
        {
            REMOTE_ASSIST_REQ* pReq = NULL;

            //
            bRemoveIfErr = FALSE;

            //
            pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(REMOTE_ASSIST_REQ)  );
            //
            pReq = &pContent->remoteAssistReq;
            //
            pMsgInput->uiType = CONST_misMsgType_task;
            //
            pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
            pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
            //
            len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

            //   2017/08/24
            if (pReq->idInfo_imGrp_related.ui64Id) pMsgInput->addr_logicalPeer.idInfo.ui64Id = pReq->idInfo_imGrp_related.ui64Id;
            else pMsgInput->addr_logicalPeer.idInfo.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;

            //
            if (procMsgTask_mc_media(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
        }
        break;
                */
                    case Consts.CONST_imCommType_taskInteractionReq:
                        {
                            //TASK_INTERACTION_REQ* pReq = null;

                            //
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)(Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf")) + sizeof_TASK_INTERACTION_REQ());
                            //
                            ref TASK_INTERACTION_REQ pReq = ref pContent.taskInteractionReq;
                            //
                            //pMsgInput->uiType = CONST_misMsgType_task;
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)((int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes);

                            if (0 != pReq.idInfo_imGrp_related.ui64Id) pMsgInput.addr_logicalPeer.idInfo.ui64Id = pReq.idInfo_imGrp_related.ui64Id;
                            else pMsgInput.addr_logicalPeer.idInfo.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;

                            if (0 != tmp_qPost_msgInput(ref pMsgInput, (uint)len, pQyMc.gui.processQ))
                            {
#if __DEBUG__
					traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                goto errLabel;
                            }
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        }
                        break;

                    case Consts.CONST_imCommType_confKey:
                        {
                            //CONF_KEY* pReq = null;

                            //
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_CONF_KEY());
                            //
                            ref CONF_KEY pReq = ref pContent.confKey;
                            //
                            //pMsgInput->uiType = CONST_misMsgType_task;
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (int)pMsgInput.lenInBytes;

                            if (0 != pReq.idInfo_imGrp_related.ui64Id) pMsgInput.addr_logicalPeer.idInfo.ui64Id = pReq.idInfo_imGrp_related.ui64Id;
                            else pMsgInput.addr_logicalPeer.idInfo.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;

                            //
#if DEBUG
                            ref CONF_KEY tmpConfKey = ref pReq;
                            ref MIS_MSG_INPUT tmpMsgInput = ref pMsgInput;
#endif

                            //
                            if (0 != tmp_qPost_msgInput(ref pMsgInput, (uint)len, pQyMc.gui.processQ))
                            {
                                traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
                                goto errLabel;
                            }
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        }
                        break;
                    case Consts.CONST_imCommType_statusConfLayoutParam:
                        {
                            //StatusConfLayoutParam* pReq = null;

                            //
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_StatusConfLayoutParam());
                            //
                            ref StatusConfLayoutParam pReq = ref pContent.statusConfLayoutParam;


                            //
                            qyFuncs.traceLog("iChannel:" + pReq.confLayoutParam.ipcParam.iChannel);
                            qyFuncs.traceLog("enlargeParam:" + pReq.confLayoutParam.enlargeParam.ui64Id);
                            qyFuncs.traceLog("bAmplification:" + pReq.confLayoutParam.respVideoStatus.bAmplification);


                            //
                            //pMsgInput->uiType = Consts.CONST_misMsgType_task;
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)((int)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgInput.lenInBytes);
                            //len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

                            if (0 != pReq.idInfo_imGrp_related.ui64Id) pMsgInput.addr_logicalPeer.idInfo.ui64Id = pReq.idInfo_imGrp_related.ui64Id;
                            else pMsgInput.addr_logicalPeer.idInfo.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;

                            if (0 != tmp_qPost_msgInput(ref pMsgInput, (uint)len, pQyMc.gui.processQ))
                            {
                                // traceLogA((char*)"mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
                                goto errLabel;
                            }
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        }
                        break;
                    case Consts.CONST_imCommType_confLayout:
                        {
                            //CONF_LAYOUT* pReq = null;

                            //
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_CONF_LAYOUT());
                            //
                            ref CONF_LAYOUT pReq = ref pContent.confLayout;
                            //
                            //pMsgInput->uiType = Consts.CONST_misMsgType_task;
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)((int)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgInput.lenInBytes);

                            if (0 != pReq.idInfo_imGrp_related.ui64Id) pMsgInput.addr_logicalPeer.idInfo.ui64Id = pReq.idInfo_imGrp_related.ui64Id;
                            else pMsgInput.addr_logicalPeer.idInfo.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;

                            if (0 != tmp_qPost_msgInput(ref pMsgInput, (uint)len, pQyMc.gui.processQ))
                            {
#if __DEBUG__
					traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                goto errLabel;
                            }
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
                        }
                        break;
                    case Consts.CONST_imCommType_confNvrInfo:
                        {
                            //ConfNvrInfo* pReq = null;

                            //(int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf")
                            pMsgInput.lenInBytes = (uint)Math.Min(sizeof_MIS_MSG_routeTalkData(), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof_ConfNvrInfo());
                            //
                            ref ConfNvrInfo pReq = ref pContent.confNvrInfo;
                            //
                            {
                                ref ConfNvrInfo tmpReq = ref pReq;

                                /*
                                fixed (byte* pBuf = tmpReq.nvrInfo.buf_ipcInfo)
                                {
                                    IpcInfo* pMem = getMem((IpcInfo*)pBuf, Consts.bufSize_ipcInfos, 0);
                                    string str1 = new string(pMem->name);

                                    //
                                    int ii = 0;
                                }
                                */

                            }
                            //
                            pMsgInput.idInfo_taskSender.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;
                            pMsgInput.idInfo_taskReceiver.ui64Id = pAddrTalkData.route.idInfo_to.ui64Id;
                            //
                            len = (int)((int)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgInput.lenInBytes);

                            if (0 != pReq.idInfo_imGrp_related.ui64Id) pMsgInput.addr_logicalPeer.idInfo.ui64Id = pReq.idInfo_imGrp_related.ui64Id;
                            else pMsgInput.addr_logicalPeer.idInfo.ui64Id = pAddrTalkData.route.idInfo_from.ui64Id;

                            if (0 != tmp_qPost_msgInput(ref pMsgInput, (uint)len, pQyMc.gui.processQ))
                            {
                                goto errLabel;
                            }
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
                        }
                        break;
                    //
                    /*
              case  CONST_imCommType_confState:
              {
                  CONF_state* pReq = NULL;

                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(CONF_state)  );
                  //
                  pReq = &pContent->confState;
                  //
                  pMsgInput->uiType = CONST_misMsgType_task;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                  //
                  len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

                  if (pReq->idInfo_imGrp_related.ui64Id) pMsgInput->addr_logicalPeer.idInfo.ui64Id = pReq->idInfo_imGrp_related.ui64Id;
                  else pMsgInput->addr_logicalPeer.idInfo.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;

                  if (qPostMsg(pMsgInput, len, &pQyMc->gui.processQ))
                  {
  # ifdef  __DEBUG__
                      traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
  #endif
                      goto errLabel;
                  }
                  PostMessage(pQyMc->gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
              }
              break;

              case  CONST_imCommType_queryCustomerServiceOfficerReq:
              {       //  2011/04/06
                  QUERY_customerServiceOfficer_req* pReq = NULL;
                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(QUERY_customerServiceOfficer_req)  );
                  //
                  pReq = &pContent->queryCustomerServiceOfficerReq;
                  //
                  if (10)
                  {
                      //  对视频咨询请求，一般不在数据库里有用户信息，需要在这里将用户信息导入内存队列。

                      QY_MESSENGER_INFO obj;
                      QM_OBJQ_MEM mem;
                      QMEM_MESSENGER_CLI* pQMem;

                      lstrcpyn(obj.misServName, pMisCnt->cfg.misServName, mycountof(obj.misServName));
                      obj.uiType = CONST_objType_messenger;
                      obj.idInfo.ui64Id = pMsgInput->data.route.idInfo_from.ui64Id;

                      if (obj.idInfo.ui64Id == pMisCnt->idInfo.ui64Id) goto errLabel;

                      if (procQmObjQ(pSci, (QM_OBJQ*)pMisCnt->pObjQ, NULL, 0, obj.uiType, &obj, &mem)) goto errLabel;
                      if (!mem.pQMemObj) goto errLabel;

                      pQMem = (QMEM_MESSENGER_CLI*)mem.pQMemObj;
                      if (pQMem)
                      {

                          if (pQMem->uiType != obj.uiType)
                          {
                              goto errLabel;
                          }

                          safeWcsnCpy(pReq->talkerDesc, pQMem->talkerDesc, mycountof(pQMem->talkerDesc));
                          if (!pQMem->displayName[0]) safeWcsnCpy(pReq->talkerDesc, pQMem->displayName, mycountof(pQMem->displayName));
                      }
                  }
                  //
                  pMsgInput->uiType = CONST_misMsgType_task;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                  //
                  len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

  #if  0
                    if  (  pReq->idInfo_imGrp_related.ui64Id  )  pMsgInput->addr_logicalPeer.idInfo.ui64Id  =  pReq->idInfo_imGrp_related.ui64Id;
                    else  pMsgInput->addr_logicalPeer.idInfo.ui64Id  =  pAddrTalkData->route.idInfo_from.ui64Id; 
  #endif

                  // 
                  unsigned  short usRespCode = CONST_qyRc_unknown;
                  void* pDb = NULL;

  # ifndef  __NOTSUPPORT_DB__
                  CQnmDb db;
                  if (!db.getAvailableDb(pQyMc->iDsnIndex_mainSys)) goto errLabel;
                  pDb = db.m_pDbMem->pDb;
  #endif

                  IM_TASK_RCD rcd;
                  int tmpiRet;

                  //  2009/10/18
                  //  pMsg->uiChannelType  =  pChannel->uiType;

                  //
                  pMsgInput->uiTaskType = CONST_imTaskType_queryCustomerServiceOfficer;
                  pMsgInput->iStatus = CONST_imTaskStatus_resp;

                  //
                  if (0!=msg2TaskRcd(pMisCnt, pMsgInput, usRespCode, &rcd))
                  {
                      traceLogA("msg2TaskRcd failed"); goto errLabel;
                  }
                  tmpiRet = storeTaskInDb(pDb, pQyMc->cfg.db.iDbType, pMisCnt, 0, &rcd, FALSE);
                  if (tmpiRet < 0)
                  {
                      traceLogA("mcThreadProc_mis_work(  ): storeTaskInDb failed"); goto errLabel;
                  }

                  pMsgInput->iTaskId = rcd.id;
                  pMsgInput->uiTaskMode_unused = rcd.uiMode;


                  if (qPostMsg(pMsgInput, len, &pQyMc->gui.processQ))
                  {
                      traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
                      goto errLabel;
                  }
                  PostMessage(pQyMc->gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
              }
              break;
              case  CONST_imCommType_queryCustomerServiceOfficerReplyReq:
              {       //  2011/04/06
                  QUERY_customerServiceOfficer_replyReq* pReq = NULL;
                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(QUERY_customerServiceOfficer_replyReq)  );
                  //
                  pReq = &pContent->queryCustomerServiceOfficerReplyReq;

                  if (1)
                  {
                      //  对视频咨询请求，一般不在数据库里有用户信息，需要在这里将用户信息导入内存队列。

                      QY_MESSENGER_INFO obj;
                      QM_OBJQ_MEM mem;
                      QMEM_MESSENGER_CLI* pQMem;

                      lstrcpyn(obj.misServName, pMisCnt->cfg.misServName, mycountof(obj.misServName));
                      obj.uiType = CONST_objType_messenger;
                      obj.idInfo.ui64Id = pMsgInput->data.route.idInfo_from.ui64Id;

                      if (obj.idInfo.ui64Id == pMisCnt->idInfo.ui64Id) goto errLabel;

                      if (procQmObjQ(pSci, (QM_OBJQ*)pMisCnt->pObjQ, NULL, 0, obj.uiType, &obj, &mem)) goto errLabel;
                      if (!mem.pQMemObj) goto errLabel;

                      pQMem = (QMEM_MESSENGER_CLI*)mem.pQMemObj;
                      if (pQMem)
                      {

                          if (pQMem->uiType != obj.uiType)
                          {
                              goto errLabel;
                          }

                          safeWcsnCpy(pReq->talkerDesc, pQMem->talkerDesc, mycountof(pQMem->talkerDesc));
                          if (!pQMem->displayName[0]) safeWcsnCpy(pReq->talkerDesc, pQMem->displayName, mycountof(pQMem->displayName));
                      }
                  }

                  //
                  pProcInfo->customerService.called.idInfo.ui64Id = pMsgInput->data.route.idInfo_from.ui64Id;
                  safeWcsnCpy(pReq->talkerDesc, pProcInfo->customerService.called.talkerDesc, mycountof(pProcInfo->customerService.called.talkerDesc));
                  notifyProgressEnd(pReq->uiTranNo_org, 0);

                  //
                  //  len  =  offsetof(  MIS_MSG_TASK,  data  )  +  pMsgInput->lenInBytes;

  #if  0
                    if  (  qPostMsg(  pMsgInput,  len,  &pQyMc->gui.processQ  )  )  {
                        traceLogA(  "mcThreadProc_mis_work(  ): qPostMsg(  ) failed"  );
                        goto  errLabel;
                    }
                    PostMessage(  pQyMc->gui.hMainWnd,  CONST_qyWm_postComm,  CONST_qyWmParam_msgArrive,  0  );    
  #endif

              }
              break;

                    //  2012/04/20				
              case  CONST_imCommType_transferGpsInfo:
              {
                  TRANSFER_GPS_INFO* pReq = NULL;
                  //
                  if (pChannel->uiType == CONST_channelType_realTimeMedia)
                  {   //  realTimeMediaÖ»ÊÇ¸¨ÖúÏß³Ì£¬Ö»ÄÜ½ÓÊÕÊý¾Ý¡£
                      qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("Error: %s ÊÕµ½ %I64u µÄ %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pMsgInput->data.route.idInfo_from.ui64Id, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                      goto errLabel;
                  }
                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_GPS_INFO)  );
                  //
                  pReq = (TRANSFER_GPS_INFO*)&pContent->transferGpsInfo;
                  //
  #if  0
                    if  (  pReq->idInfo_logicalPeer.ui64Id  )  {
                        traceLogA(  "procMsgInput_other_req: transferAvInfo, use idInfo_logicalPeer to set idInfo_to"  );
                        pAddrTalkData->route.idInfo_to.ui64Id  =  pReq->idInfo_logicalPeer.ui64Id;
                        pReq->idInfo_logicalPeer.ui64Id  =  0;
                    }
  #endif
                  //
                  pMsgInput->uiType = CONST_misMsgType_task;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                  //
                  len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;
                  //
                  if (procMsgTask_mc_media(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
              }
              break;

              case  CONST_imCommType_transferGpsReplyInfo:
              {
                  TRANSFER_GPS_replyInfo* pReq = NULL;
                  //
                  if (pChannel->uiType == CONST_channelType_realTimeMedia)
                  {   //  realTimeMediaÖ»ÊÇ¸¨ÖúÏß³Ì£¬Ö»ÄÜ½ÓÊÕÊý¾Ý¡£
                      qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("Error: %s ÊÕµ½ %I64u µÄ %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pMsgInput->data.route.idInfo_from.ui64Id, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                      goto errLabel;
                  }
                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_GPS_replyInfo)  );
                  //
                  pReq = (TRANSFER_GPS_replyInfo*)&pContent->transferGpsReplyInfo;
                  //
                  pMsgInput->uiType = CONST_misMsgType_task;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                  //
                  len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

                  if (procMsgTask_mc_media(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
              }
              break;

              case  CONST_imCommType_transferGpsData:
              {
                  TRANSFER_GPS_DATA* pReq = NULL;

                  //
                  bRemoveIfErr = FALSE;

                  //  2009/02/25
                  startChannel(pProcInfo, pMisCnt, pChannel, _T("proc transferGpsData"));

                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(TRANSFER_GPS_DATA)  );
                  //
                  pReq = &pContent->transferGpsData;
                  //
                  pMsgInput->uiType = CONST_misMsgType_task;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                  //
                  len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

  # ifdef  __DEBUG__
                  static int kk = 0; kk++;
                  //  traceLogA(  "here  kk %d",  kk  );
  #endif

                  if (procMsgTask_mc_media(pQyMc, pSci, pProcInfo, pMsgInput, len, TRUE, pCtx, pChannel)) goto errLabel;
              }
              break;

                    //
              case  CONST_imCommType_ptzControlReq:
              {       //  2012/08/06
                  PTZ_control_req* pReq = NULL;

                  //
                  pReq = &pContent->ptzControlReq;
                  if (pReq->ucbReply)
                  {       //  这里要处理回复

                      //
                      pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(PTZ_control_req)  );
                      //
                      pMsgInput->uiType = CONST_misMsgType_task;
                      //
                      pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                      pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                      //
                      len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

                      pMsgInput->addr_logicalPeer.idInfo.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;

                      if (qPostMsg(pMsgInput, len, &pQyMc->gui.processQ))
                      {
                          traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
                          goto errLabel;
                      }
                      PostMessage(pQyMc->gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);

                      break;
                  }


                  //
                  QIS_ptz_wnd* pPtzWnd = &pProcInfo->ptzWnd;
                  int i;
                  //
                  if (!pPtzWnd->m_var.cfg.bCfgInited)
                  {
                      getPtzSettings(&pPtzWnd->m_var.cfg);
                  }
                  if (!pPtzWnd->m_var.cfg.bCfgInited) goto errLabel;

                  //
                  if (pQyMc->iCustomId == CONST_qyCustomId_bjyz)
                  {
                      //  2017/08/29
                  }
                  else
                  {
                      for (i = 0; i < mycountof(pPtzWnd->m_var.cfg.idInfos_allowed); i++)
                      {
                          if (!pPtzWnd->m_var.cfg.idInfos_allowed[i].ui64Id) continue;
                          if (pAddrTalkData->route.idInfo_from.ui64Id == pPtzWnd->m_var.cfg.idInfos_allowed[i].ui64Id) break;
                      }
                      if (i == mycountof(pPtzWnd->m_var.cfg.idInfos_allowed))
                      {
                          qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    procMsgInput_other_req: ptzControlReq, denied, %I64u"), pAddrTalkData->route.idInfo_from.ui64Id);
                          showNotification(NULL, 0, &pAddrTalkData->route.idInfo_from, 0, 0, 0, _T("ptzReq denied"));
                          goto errLabel;
                      }
                  }
                  qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    procMsgInput_other_req: ptzControlReq, permitted, %I64u"), pAddrTalkData->route.idInfo_from.ui64Id);
                  showNotification(NULL, 0, &pAddrTalkData->route.idInfo_from, 0, 0, 0, _T("ptzReq permitted"));



                  PTZ_control_req reply;
                  memset(&reply, 0, sizeof(reply));
                  reply.uiType = pReq->uiType;
                  reply.ucbReply = TRUE;

                  //
                  reply.u.reply.tStartTime_org = pMsgInput->tStartTime;
                  reply.u.reply.uiTranNo_org = pMsgInput->uiTranNo;
                  //
                  lstrcpyn(reply.u.reply.tHint, pPtzWnd->m_var.cfg.tHint, mycountof(reply.u.reply.tHint));

                  i = 0;
                  reply.mems[i].nAddr = pPtzWnd->m_var.cfg.m_nAddr;
                  reply.mems[i].nPelcoType = pPtzWnd->m_var.cfg.m_pelcoType;
                  i++;

                  reply.usCnt = i;

                  MACRO_prepareForTran();

                  lenInBytes = sizeof(reply);

                  //
                  if (postMsg2Mgr_mc(pMisCnt, NULL, CONST_misMsgType_outputTask, 0, CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, (char*)&reply, sizeof(reply), 0, &pAddrTalkData->route.idInfo_from, pChannel->uiType, NULL, FALSE)) traceLogA("postMsg2Mgr_mc failed");


              }
              break;
              case  CONST_imCommType_ptzControlCmd:
              {       //  2012/08/06
                  PTZ_control_cmd* pReq = NULL;

                  pReq = &pContent->ptzControlCmd;

                  //
                  QIS_ptz_wnd* pPtzWnd = &pProcInfo->ptzWnd;
                  int i;

                  //
                  if (!pPtzWnd->m_var.cfg.bCfgInited) goto errLabel;

                  //
                  if (pQyMc->iCustomId == CONST_qyCustomId_bjyz)
                  {
                      //  2017/08/26
                  }
                  else
                  {
                      //  
                      for (i = 0; i < mycountof(pPtzWnd->m_var.cfg.idInfos_allowed); i++)
                      {
                          if (!pPtzWnd->m_var.cfg.idInfos_allowed[i].ui64Id) continue;
                          if (pAddrTalkData->route.idInfo_from.ui64Id == pPtzWnd->m_var.cfg.idInfos_allowed[i].ui64Id) break;
                      }
                      if (i == mycountof(pPtzWnd->m_var.cfg.idInfos_allowed))
                      {
                          qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    procMsgInput_other_req: ptzControlCmd, denied, %I64u"), pAddrTalkData->route.idInfo_from.ui64Id);
                          showNotification(NULL, 0, &pAddrTalkData->route.idInfo_from, 0, 0, 0, _T("ptzCmd denied"));
                          goto errLabel;
                      }
                  }
                  TCHAR tBuf[128] = _T("");
                  QY_DMITEM* pTable = getResTable(0, &pQyMc->cusRes, CONST_resId_objTypeTable);
                  if (!pTable) goto errLabel;
                  _sntprintf(tBuf, mycountof(tBuf), _T("    procMsgInput_other_req: ptzControlCmd on camera (%s,%d.%d) permitted, from %I64u"), qyGetDesByType1(pTable, pReq->cmdInfo.obj.uiObjType), (int)pReq->cmdInfo.obj.usIndex_obj, (int)pReq->cmdInfo.obj.usHelp_subIndex, pAddrTalkData->route.idInfo_from.ui64Id);
                  showInfo_open(0, _T(""), tBuf);
                  showNotification(NULL, 0, &pAddrTalkData->route.idInfo_from, 0, 0, 0, _T("ptzCmd permitted"));

                  //
                  pMsgInput->lenInBytes = min(sizeof(pMsgInput->data),  offsetof(MIS_MSG_routeTalkData, buf) + sizeof(PTZ_control_cmd)  );
                  //
                  pMsgInput->uiType = CONST_misMsgType_task;
                  //
                  pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
                  pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
                  //
                  len = offsetof(MIS_MSG_TASK, data) + pMsgInput->lenInBytes;

                  pMsgInput->addr_logicalPeer.idInfo.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;

                  if (qPostMsg(pMsgInput, len, &pQyMc->gui.processQ))
                  {
  # ifdef  __DEBUG__
                      traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
  #endif
                      goto errLabel;
                  }
                  PostMessage(pQyMc->gui.hMainWnd, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);


              }
              break;
                      */

                    //


                    default:
                        //traceLogA("procMsgInput_other_req: unprocessed contentType %d [%S]", pContent->uiType, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, CONST_resId_qyCommTypeTable), pContent->uiType));
                        str = string.Format("procMsgInput_other_req: unprocessed contentType {0}", pContent.uiType);
                        qyFuncs.showInfo_open(0, null, str);
                        //
                        break;
                }
            }

            iErr = 0;

        errLabel:

            if (iErr != 0)
            {
#if false      //  2011/10/28. 出错就清理队列容易导致功能不稳定                                      
#endif
            }

#if DEBUG
            qyFuncs.traceLog("procMsgInput_other_req leaves. iErr " + iErr  + " " + hint );
#endif


            return iErr;
        }


        public static unsafe int procMsgInput_other_resp(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU pMsgU, uint lenInBytes_msg, CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;

            if  ( pMsgU.uiType!=Consts.CONST_misMsgType_input)
            {
                return -1;
            }
            ref MIS_MSG_INPUT pMsgInput = ref pMsgU.input;

            //
            int dataLen = 0;
            MIS_CNT pMisCnt = null;
            uint uiChannelType = Consts.CONST_channelType_robot;

            //
            string str;


            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                //
                int lenInBytes = 0;
                //
                //TCHAR tHintBuf[255 + 1] = _T("");
                TMP_tHintBuf tHintBuf;
                //TCHAR tBuf[255 + 1] = _T("");
                TMP_tBuf128 tBuf128;
                bool bRemoveIfErr = true;
                //  2008/11/19
                MSG_ROUTE route;
                byte ucFlg = 0;


#if __DEBUG__
				if (sizeof(pMsgInput->data)  < sizeof(MIS_MSG_TALKDATA)  )  {
					traceLogA("procMsgInput_mgr_resp:  pMsgInput->buf size {%d}  >=  sizeof(  IM_CONTENTU  ) {%d}", sizeof(pMsgInput->data),  sizeof(IM_CONTENTU)  );
					goto errLabel;
				}
#endif

                pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;

                if (!isUcFlgResp(pMsgInput.ucFlg)) goto errLabel;


                if (!pMsgInput.ucbConvrted)
                {
                    fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
                    {
                        MIS_MSG_INPUT* pMsg = (MIS_MSG_INPUT*)pMsgBuf_forInput;
                        if (null == pMsg)
                        {
#if DEBUG
                            str = ("procMsgInput_other_resp failed: ctx.pMsgInput is null, need to malloc");
                            showInfo_open(0, null, str);
#endif
                            return -1;
                        }

                        MACRO_memset_misMsgInput(pMsg);
                        fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                        {
                            mymemcpy((IntPtr)pMsg, (IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
                        }

                        if (isUcFlgTalkData(pMsgInput.ucFlg) || isUcFlgRouteTalkData(pMsgInput.ucFlg))
                        {
                            MIS_MSG_routeTalkData* pAddrTalkData1 = null;
                            //IM_CONTENTU* pContent1 = null;

                            pAddrTalkData1 = (MIS_MSG_routeTalkData*)&pMsg->data;
                            //
                            fixed (MIS_MSG_routeTalkData* data = &pMsgInput.data)
                            {
                                if (0 != qyntohRouteTalkData(pMsgInput.ucFlg, (byte*)data, pMsgInput.lenInBytes, &pMsg->data.route, (IM_CONTENTU*)&pMsg->data.buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
                                {
                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    qyntohRouteTalkData failed. %s"), tHintBuf);
                                    goto errLabel;
                                }
                            }
                            //if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("    %s"), tHintBuf);
                            //
                            if (0 != getIdInfo_logicalPeer(pMisCnt, ref pAddrTalkData1->route, ref pMsg->addr_logicalPeer)) goto errLabel;
                            //			 
                        }
                        else
                        {
                            if (pMsgInput.usCode == Consts.CONST_qyRc_servBusy)
                            {
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Server returns: %s"), qyGetDesByType1(CONST_qyRcTable, pMsgInput->usCode));
                                iErr = 0; goto errLabel;
                            }
                            if (pMsgInput.lenInBytes < sizeof(uint))
                            {   //  Ö¸Ã»ÓÐcontentType
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Failed:  Server returns: %s"), qyGetDesByType1(CONST_qyRcTable, pMsgInput->usCode));
                                goto errLabel;
                            }

                            fixed (MIS_MSG_routeTalkData* data = &pMsgInput.data)
                            {
                                if (0 != qyntohRouteTalkData(pMsgInput.ucFlg, (byte*)data, pMsgInput.lenInBytes, null, (IM_CONTENTU*)&pMsg->data, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
                                {
                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("qyntohRouteTalkData failed. %s"), tHintBuf);
                                    goto errLabel;
                                }
                            }

                            //if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("%s"), tHintBuf);
                            //		   
                        }
                        pMsg->ucbConvrted = true;
                        //
                        pMsgU = ref pCtx.pMsgBuf_forInput.msgU;
                        pMsgInput = ref *pMsg;
                    }
                }

                //MIS_MSG_routeTalkData* pAddrTalkData = null;
                //IM_CONTENTU* pContent = null;

                ref MIS_MSG_routeTalkData pAddrTalkData = ref pMsgInput.data;// MACRO_msgRouteTalkData(pMsgInput.ucFlg, &pMsgInput->data);
                /*
                if (null == pAddrTalkData)
                {
                    traceLogA("procMsgInput_other_resp failed: pAddrTalkData is null");
                    goto errLabel;
                }
                */

                //
                ref IM_CONTENTU pContent = ref pMsgInput.data.buf;// M_getMsgContent(pMsgInput->uiType,pMsgInput->ucFlg, &pMsgInput->data);
                //if (null == pContent) goto errLabel;


#if __DEBUG__
				pAddrTalkData->route.debugInfo.dwTickCnt_resp_recved = myGetTickCount();
#endif

                //
                /*
				if (pQyMc->env.bDebugInfo && pProcInfo->cfg.debugStatusInfo.ucbViewIoStatus)
				{
					TCHAR who_from[32 + 1] = _T("");
					LPCTSTR where_showInfo = qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType);
					int iAfter = GetTickCount() - pMsgInput->dwTickCnt_start;
					//
					_sntprintf(who_from, mycountof(who_from), _T("%I64u"), pAddrTalkData->route.idInfo_from.ui64Id);
					if (isUcFlgResp(pMsgInput->ucFlg)) _sntprintf(tBuf, mycountof(tBuf), _T("  tn %d, resp: %s, dataLen %d, iAfter_recvd %d."), pMsgInput->uiTranNo, qyGetDesByType1(CONST_qyRcTable, pMsgInput->usCode), pMsgInput->lenInBytes, iAfter);
					else _sntprintf(tBuf, mycountof(tBuf), _T(" tn %d, req: %s, dataLen %d, iAfter_recvd %d."), pMsgInput->uiTranNo, qyGetDesByType1(CONST_qyCmdTable, pMsgInput->usCode), pMsgInput->lenInBytes, iAfter);
					if (pMsgInput->data.route.debugInfo.index) _sntprintf(tBuf, mycountof(tBuf), _T("%s index %d"), tBuf, pMsg->data.route.debugInfo.index);

					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), who_from, pProcInfo->who_showInfo, where_showInfo, _T(""), _T("%s"), tBuf);
				}
				*/

                //  ÏÂÃæ´¦ÀíÏìÓ¦
                //	
                switch (pContent.uiType)
                {
                    case Consts.CONST_imCommType_simpleResp:
                        {

                            //traceLogA("procMsgInput_robot_resp: %S, %S", qyGetDesByType1(CONST_qyCmdTable, pContent->simpleResp.usCmd_org), qyGetDesByType1(CONST_qyRcTable, pMsgInput->usCode));

                            //if (null == pAddrTalkData) goto errLabel;
                            if (null == pCtx.pProcessQ) goto errLabel;

                            //
                            //printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
                            //
                            //printMisMsg(null, null, (MIS_MSGU*)pMsgInput);

                            switch (pContent.simpleResp.usCmd_org)
                            {
                                case Consts.CONST_qyCmd_sendRobotTaskReply:
                                    {
                                        //
                                        Int64 tStartTran; uint uiTranNo;
                                        MACRO_prepareForTran(out tStartTran, out uiTranNo);

                                        //
                                        CTRANSFER_FILEDATA_REQ req=new CTRANSFER_FILEDATA_REQ();

                                        //mymemset((IntPtr)(byte*)&req, 0, sizeof(TRANSFER_FILEDATA_REQ));
                                        req.transferFileDataReq.uiType = Consts.CONST_imCommType_transferFileDataReq;
                                        //
                                        req.transferFileDataReq.tmpInternal.tStartTime_new = tStartTran;
                                        req.transferFileDataReq.tmpInternal.uiTranNo_new = uiTranNo;
                                        //
                                        PARAM_findOrgMsg param3 = new PARAM_findOrgMsg();
                                        param3.pFileDataReq_o = req;

                                        if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param3) <= 0) goto errLabel;
                                        lenInBytes = sizeof(TRANSFER_FILEDATA_REQ);

                                        //
                                        //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), ("procMsgInput_robot_resp"), ("start to retrieve file. perSnd %d"), req.uiSizePerSnd);

#if __DEBUG__
										traceLogA("");
#endif
                                        if (0 != startChannel(pProcInfo, pMisCnt, pChannel, ("robot channel")))
                                        {
                                            traceLogA("robot Channel");
                                            goto errLabel;
                                        }

                                        //
                                        MSG_ROUTE msgRoute = new MSG_ROUTE();
                                        if (0 != postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_outputTask, 0, Consts.CONST_qyCmd_sendRobotTaskData, pMsgInput.tStartTime, pMsgInput.uiTranNo, 0, (byte*)&req, (uint)lenInBytes, pMsgInput.addr_logicalPeer.idInfo.ui64Id, pAddrTalkData.route.idInfo_from.ui64Id, uiChannelType, null, false)) goto errLabel;
                                    }
                                    break;
                                case Consts.CONST_qyCmd_sendMedia:
                                    {

                                        if (pContent.simpleResp.uiContentType_org == Consts.CONST_imCommType_transferAvReplyInfo)
                                        {
                                            if (!isRcOk(pMsgInput.usCode))
                                            {
                                                traceLogA("rc is not ok"); goto errLabel;
                                            }

                                            PARAM_findOrgMsg param3 = new PARAM_findOrgMsg();

                                            if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param3) <= 0) goto errLabel;

                                            traceLogA("");
                                            if (0 != startChannel(pProcInfo, pMisCnt, pChannel, ("")))
                                            {
                                                traceLogA("start Media Channel Failed."); goto errLabel;
                                            }
                                        }

                                        //  traceLogA(  "ÕâÀï£¬ËÆºõÓ¦¸Ã×öÊÓÆµ´¦Ê¼»¯µÄ¹¤×÷¡£µÈÆô¶¯ÁËÊÓÆµ¼à¿Ø£¬ÔÚÓÐÊÓÆµÊý¾ÝÊ±£¬×ÔÈ»£¬¾Í·¢ËÍ¹ýÈ¥ÁË"  );

                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        break;
                    case Consts.CONST_imCommType_taskProcResp:
                        {

                            bRemoveIfErr = false;

                            //if (null == pAddrTalkData) goto errLabel;
                            if (null == pCtx.pProcessQ) goto errLabel;

#if false
				   if  (  pContent->taskProcResp.idInfo_logicalPeer.ui64Id  )  {
					   traceLogA(  "procMsgInput_other_resp: taskProcResp, use idInfo_logicalPeer to set idInfo_to"  );
					   pAddrTalkData->route.idInfo_to.ui64Id  =  pContent->taskProcResp.idInfo_logicalPeer.ui64Id;
					   pContent->taskProcResp.idInfo_logicalPeer.ui64Id  =  0;				  
					   //
					   if  (  getIdInfo_logicalPeer(  pSci,  pMisCnt,  &pAddrTalkData->route,  &pMsgInput->addr_logicalPeer  )  )  goto  errLabel;				 
				   }
#endif

#if true
                            if (0 != pContent.taskProcResp.idInfo_logicalPeer.ui64Id)
                            {
                                if (pContent.taskProcResp.idInfo_logicalPeer.ui64Id == pMisCnt.idInfo.ui64Id)
                                {
                                    //  2015/06/08
                                    pMsgInput.addr_logicalPeer.idInfo.ui64Id = pMsgInput.data.route.idInfo_from.ui64Id;
                                    pContent.taskProcResp.idInfo_logicalPeer.ui64Id = 0;
                                }
                                else
                                {
                                    pMsgInput.addr_logicalPeer.idInfo.ui64Id = pContent.taskProcResp.idInfo_logicalPeer.ui64Id;
                                }
                            }
#endif


                            if (0 != tmp_qPost_msgInput(ref pMsgInput, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)sizeof(TASK_PROC_RESP), pQyMc.gui.processQ))
                            {
#if __DEBUG__
								traceLogA("procMsgInput_other_resp(  ): qPostMsg(  ) failed");
#endif
                                goto errLabel;
                            }
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        }
                        break;
                    case Consts.CONST_imCommType_transferFileDataResp:
                        {
                            //
#if __DEBUG__
							traceLogA("procMsgInput_robot_resp:  transferFileDataResp");
#endif

                            //
                            bRemoveIfErr = false;   //  2010/12/05

                            if (null == pCtx.pProcessQ) goto errLabel;
                            //
                            //printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
                            //
                            //printMisMsg(null, null, (MIS_MSGU*)pMsgInput);

                            CTRANSFER_FILEDATA_REQ req=new CTRANSFER_FILEDATA_REQ();
                            //mymemset((IntPtr)(byte*)&req, 0, sizeof(TRANSFER_FILEDATA_REQ));
                            req.transferFileDataReq.uiType = Consts.CONST_imCommType_transferFileDataReq;

                            PARAM_findOrgMsg param3 = new PARAM_findOrgMsg();
                            param3.pFileDataReq_o = req;

                            if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param3) <= 0) goto errLabel;
                            lenInBytes = sizeof(TRANSFER_FILEDATA_REQ);

                            startChannel(pProcInfo, pMisCnt, pChannel, ("proc transferFileDataResp"));

                            //
                            ucFlg = 0;
                            mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
                            route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
                            if (true)
                            {
                                ucFlg |= Consts.CONST_commFlg_routeTalkData;
                                route.debugInfo.dwTickCnt_me_start1 = myGetTickCount64(null);
                                Int64 tNow=0; mytime(ref tNow); route.debugInfo.tTime_start = tNow;
                            }

                            if (!req.transferFileDataReq.tmpInternal.ucbFileEnd)
                            {

                                //  2008/11/19.
                                route.debugInfo.index = (int)req.transferFileDataReq.uiDataBlockIndex;

                                if (0 != postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref route, Consts.CONST_misMsgType_outputTask, ucFlg, Consts.CONST_qyCmd_sendRobotTaskData, pMsgInput.tStartTime, pMsgInput.uiTranNo, 0, (byte*)&req, (uint)lenInBytes, pMsgInput.addr_logicalPeer.idInfo.ui64Id, pAddrTalkData.route.idInfo_from.ui64Id, uiChannelType, null, false)) goto errLabel;
                                break;
                            }

#if __DEBUG__
							traceLogA("procMsgInput_other_resp: send transferFileEndReq");
#endif
                            TRANSFER_FILEEND_REQ endReq;
                            mymemset((IntPtr)(byte*)&endReq, 0, sizeof(TRANSFER_FILEEND_REQ));
                            endReq.uiType = Consts.CONST_imCommType_transferFileEndReq;
                            //  2015/09/07
                            endReq.tmpInternal.bChkToRemove_i = true;
                            //
                            PARAM_findOrgMsg param4 = new PARAM_findOrgMsg();
                            param4.pTransferFileEndReq = &endReq;

                            //  if  (  pCtx->pProcessQ->qRemoveMsg(  tmpHandler_findOrgMsgAndProc_is,  pMsgInput,  &endReq  )  )  goto  errLabel;
                            if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param4) <= 0) goto errLabel;
                            //  2015/09/08
                            if (endReq.tmpInternal.bNeedRemoved_o)
                            {
                                qmcApplyForRemovingInvalidTasks(pProcInfo, 0);
                            }
                            //
                            lenInBytes = sizeof(TRANSFER_FILEEND_REQ);
                            //
                            route.debugInfo.index = -1; // 

                            if (0 != postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref route, Consts.CONST_misMsgType_outputTask, ucFlg, Consts.CONST_qyCmd_sendRobotTaskData, pMsgInput.tStartTime, pMsgInput.uiTranNo, 0, (byte*)&endReq, (uint)lenInBytes, pMsgInput.addr_logicalPeer.idInfo.ui64Id, pAddrTalkData.route.idInfo_from.ui64Id, uiChannelType, null, false)) goto errLabel;
                        }
                        break;

                    case Consts.CONST_imCommType_transferVideoDataResp:

                        bRemoveIfErr = false;

                        if (null == pCtx.pProcessQ) goto errLabel;

                        PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                        if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param) <= 0)
                        {
#if __DEBUG__
							showInfo_open(0, 0, _T("Err: videoDataResp not mateched"));
#endif
                            goto errLabel;
                        }

                        break;

                    case Consts.CONST_imCommType_transferAudioDataResp:

                        bRemoveIfErr = false;

                        if (null == pCtx.pProcessQ) goto errLabel;

                        PARAM_findOrgMsg param1 = new PARAM_findOrgMsg();
                        if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param1) <= 0)
                        {
#if __DEBUG__
			showInfo_open(0, 0, _T("Err: audioDataResp not mateched"));
#endif
                            goto errLabel;
                        }

                        break;

                    case Consts.CONST_imCommType_transferGpsDataResp:
                        bRemoveIfErr = false;

                        if (null == pCtx.pProcessQ) goto errLabel;

                        PARAM_findOrgMsg param2 = new PARAM_findOrgMsg();

                        if (pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param2) <= 0)
                        {
#if __DEBUG__
							showInfo_open(0, 0, _T("Err: gpsDataResp not mateched"));
#endif
                            goto errLabel;
                        }

                        break;

                    default:
#if __DEBUG__
						_sntprintf(tBuf, mycountof(tBuf), _T("procMsgInput_other_resp(  ):  unprocessed contentType %d, cmd_org %s\n"), pContent->uiType, qyGetDesByType1(CONST_qyCmdTable, pMsgInput->usCode_org));
						OutputDebugString(tBuf);
#endif
                        break;
                }
            }


            //
            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }
            return iErr;
        }

        //
        public static unsafe int tmpHandler_restartRobotTask(CTX_caller ctxCaller, ref MIS_MSGU p0, PARAM_findOrgMsg p1, TASK_common taskElem)
        {
            int iErr = -1;
            COMMON_PARAM pCommonParam0 = (COMMON_PARAM)p1.commonParam0;
            CCtxQyMc pQyMc = (CCtxQyMc)pCommonParam0.p0;
            //QY_SERVICEGUI_INFO* pSci = (QY_SERVICEGUI_INFO*)pCommonParam0->p1;
            MIS_CNT pMisCnt = (MIS_CNT)pCommonParam0.p2;
            //
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, taskElem.m_index_taskInfo, "");
            if (null == pTaskInfo) return -1;

            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) return -1;

            string str;


            fixed (MIS_MSGU* pMsgElem = &pTaskData.msgU)
            {

                //
                if (pTaskInfo.var.bNeedRemoved
                    || pTaskInfo.var.bTaskInvalid)
                {
                    // skipped			 
                    return 0;
                }

                //
                if (pMsgElem->uiType == Consts.CONST_misMsgType_task)
                {
                    IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsgElem->task.data.buf;

                    //if  (  pContent->uiType  ==  CONST_imCommType_transferFileReplyReq  )  
                    if (pContent->uiType == Consts.CONST_imCommType_transferFileReq
                        && pTaskInfo.var.m_bReplyTask)
                    {
                        TASK_transferFile task = (TASK_transferFile)taskElem;
                        QMC_taskData_transferFile pTt = (QMC_taskData_transferFile)pTaskInfo.getTaskData();

                        uint dwTickCnt = myGetTickCount(null);
                        int iDiffInMs = (int)(dwTickCnt - pTt.dwLastReportTickCnt);
                        if (iDiffInMs > Consts.MIN_taskTimeoutInS * 1000)
                        {

                            //
                            task.myTask.nTimes_toRestart++;

                            //
                            if (task.myTask.uiLastDataBlockIndex < 30)
                            {
                                if (task.myTask.nTimes_toRestart > 3)
                                {
                                    task.myTask.uiSizePerSnd_internal = task.myTask.uiSizePerSnd_internal / 2;
                                    //
                                    //
                                    int minVal = Consts.MIN_uiSizePerSnd_file_big;
                                    //				
                                    if (task.myTask.nTimes_toRestart > 9)
                                    {
                                        minVal = Consts.MIN_uiSizePerSnd_file_small;
                                    }
                                    else if (task.myTask.nTimes_toRestart > 6)
                                    {
                                        minVal = Consts.MIN_uiSizePerSnd_file_mid;
                                    }
                                    //
                                    if (task.myTask.uiSizePerSnd_internal < minVal)
                                    {
                                        task.myTask.uiSizePerSnd_internal = (uint)minVal;
                                    }

                                    //
                                    //TCHAR tBuf[128];
                                    str = string.Format("nTimes_toRestart %d, sizePerSnd adjusted to %d", task.myTask.nTimes_toRestart, task.myTask.uiSizePerSnd_internal);
                                    showInfo_open(0, null, str);
                                }
                            }

                            //
                            TRANSFER_FILEDATA_REQ req;
                            int lenInBytes;
                            uint uiChannelType = Consts.CONST_channelType_talking;        //  »Ö¸´²Ù×÷ÓÃtalkingÍ¨µÀ¡£ÒòÎª¿ÉÄÜrobotÍ¨µÀËÀÁË

                            mymemset((IntPtr)(byte*)&req, 0, sizeof(TRANSFER_FILEDATA_REQ));
                            req.uiType = Consts.CONST_imCommType_transferFileDataReq;

                            req.uiSizePerSnd = task.myTask.uiSizePerSnd_internal;
                            req.tStartTime_org = task.myTask.reply_info.replyReq.tStartTime_org;
                            req.uiTranNo_org = task.myTask.reply_info.replyReq.uiTranNo_org;
                            req.uiSerialNo_org = task.myTask.reply_info.replyReq.uiSerialNo_org;

                            //
                            if (0 == task.myTask.uiLastDataBlockIndex) req.uiDataBlockIndex = 0;
                            else req.uiDataBlockIndex = task.myTask.uiLastDataBlockIndex + 1;

                            //
#if false//0
					__int64 tStartTime  =  task->myTask.reply_info.tStartTime_reply;
					uint uiTranNo  =  task->myTask.reply_info.uiTranNo_reply;
#endif
                            //
                            Int64 tStartTran; uint uiTranNo;
                            MACRO_prepareForTran(out tStartTran, out uiTranNo);
                            //
                            QY_MESSENGER_ID idInfo_dst = pMsgElem->task.data.route.idInfo_from;

                            //
                            str = string.Format("restart robot task %d times, uiDataBlockIndex %d, sizePerSnd %d. to %I64u. tn %d, channel %s",
                                task.myTask.nTimes_toRestart, req.uiDataBlockIndex, req.uiSizePerSnd, idInfo_dst.ui64Id, uiTranNo, qyGetDesByType1(CONST_channelTypeTable, uiChannelType));
                            showInfo_open(0, null, str);

                            //
                            task.myTask.reply_info.tStartTime_lastFileDataReq = tStartTran;
                            task.myTask.reply_info.uiTranNo_lastFileDataReq = uiTranNo;

                            //
                            lenInBytes = sizeof(TRANSFER_FILEDATA_REQ);
                            //
                            MSG_ROUTE msgRoute = new MSG_ROUTE();
                            if (0 != postMsg2Mgr_mc(null, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_outputTask, 0, Consts.CONST_qyCmd_sendRobotTaskData, tStartTran, uiTranNo, 0, (byte*)&req, (uint)lenInBytes, pMsgElem->task.addr_logicalPeer.idInfo.ui64Id, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;
                        }
                    }
                }
            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        public static unsafe int restartRobotTask(CCtxQyMc pQyMc, MIS_CNT pMisCnt, CTX_MC_thread pCtx)
        {
            COMMON_PARAM commonParam = new COMMON_PARAM();
            MIS_CHANNEL pChannel = null;

            if (null == pMisCnt) return -1;

            pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_talking);        //  ×¢Òâ£ºÕâÀïÊ¹ÓÃµÄÊÇtalkingÍ¨µÀ¡£ÒòÎª»Ö¸´ÈÎÎñµÄÖ¸ÁîÐèÒªÔÚtalking·¢ËÍ¡£
            if (!bChannelOk(pChannel)) return -1;

            MACRO_makeCommonParam3(pQyMc, 0, pMisCnt, commonParam);


            CMIS_MSGU p0_unused = new CMIS_MSGU();
            PARAM_findOrgMsg param = new PARAM_findOrgMsg();
            param.commonParam0 = commonParam;

            pCtx.pProcessQ.qTraverse(null, tmpHandler_restartRobotTask, ref p0_unused.msgU, param);

            return 0;
        }


        //
        public static unsafe void mcThreadProc_mis_robot(object lpParameter)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            int loopCtrl = 0;
            int i;
            uint dwThreadId = myGetCurrentThreadId();
            MIS_MSGU* pMsg = null;
            uint len;
            uint dwRet;
            //  long							lPrev;
            CTX_MC_thread ctx;
            bool bMsgArrive = false;
            //
            uint uiChannelType = Consts.CONST_channelType_robot;

            string str;

            //
#if __DEBUG__
			traceLogA((char*)"mcThreadProc_mis_robot:  enters");
#endif

            //
            //mymemset(&ctx, 0, sizeof(ctx));
            ctx = new CTX_MC_thread();
            ctx.pProcessQ = pProcInfo.processQ_media; //  robot;

            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                ;
                if (null == (pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;

                for (; !pQyMc.bQuit; loopCtrl++)
                {

                    //dwRet = WaitForSingleObject(pProcInfo->robotQ.hSemaTrigger, 3500);
                    //if (dwRet == WAIT_FAILED) goto errLabel;
                    myWaitOne(pProcInfo.robotQ.hSemaTrigger, 3500);

                    //
                    bMsgArrive = false;

                    for (; !pQyMc.bQuit;)
                    {
                        len = (uint)sizeof(MIS_MSGU);
                        if (0 != qGetMsg(pProcInfo.robotQ, (IntPtr)pMsg, &len)) break;
                        mymemset((IntPtr)(((byte*)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));

                        bMsgArrive = true;

                        switch (pMsg->uiType)
                        {
                            case Consts.CONST_misMsgType_input:
                                {
                                    MIS_CNT pMisCnt = pProcInfo.pMisCnt;// (MIS_CNT*)pMsg->input.pMisCnt;
                                    MIS_CHANNEL pChannel = null;

                                    if (null == pMisCnt)
                                    {
                                        traceLogA("mcThreadProc_mis_robot failed: pMisCnt is null");
                                        break;
                                    }
                                    str = string.Format("mcThreadProc_mis_robot: {0} {1}", isUcFlgResp(pMsg->input.ucFlg) ? _T("resp") : _T("req"), qyGetDesByType1(CONST_qyCmdTable, pMsg->input.usCode));
                                    traceLog(str);

                                    //
                                    if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) break;

                                    if (isUcFlgResp(pMsg->input.ucFlg))
                                    {
                                        if (0 != procMsgInput_other_resp(pQyMc, pProcInfo, ref *pMsg, len, ctx, pChannel)) break;
                                    }
                                    else
                                    {
                                        if (0 != procMsgInput_other_req(pQyMc, pProcInfo, ref *pMsg, len, ctx, pChannel, "robot.3411")) break;
                                    }

                                }
                                break;
                            case Consts.CONST_misMsgType_req:
                                break;
                            case Consts.CONST_misMsgType_talk:
                                traceLogA("mcThreadProc_mis_robot: msg_talk ");
                                break;
                            case Consts.CONST_misMsgType_task:
                                {
                                    MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                                    MIS_CHANNEL pChannel = null;

                                    if (null == pMisCnt) break;
                                    if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) break;

                                    if (0 != procMsgTask_mc_robot(pQyMc, pProcInfo, &pMsg->task, len, false, ctx, pChannel)) break;

                                    //printMisMsg(null, null, pMsg);

                                }
                                break;
                            case Consts.CONST_misMsgType_applyForChkChannels_qmc:
                                {
                                    MIS_CNT pMisCnt = pProcInfo.pMisCnt;// (MIS_CNT*)pMsg->applyForChkChannels_qmc.pMisCnt;
                                    if (null == pMisCnt) break;

                                    //  if  (  pMsg->applyForChkChannels_qmc.bTaskExists  )  
                                    {
                                        startChannel(pProcInfo, pMisCnt, getChannelByType(pMisCnt, Consts.CONST_channelType_robot), null);

                                        //  if  (  pMsg->applyForChkChannels_qmc.bTaskNeedRestart  )  
                                        {
                                            traceLog(_T("robot: to restart robot task"));
                                            str = string.Format("robot: to restart tasks");
                                            showInfo_open(0, null, str);

                                            restartRobotTask(pQyMc, pMisCnt, ctx);
                                        }

                                    }

                                }
                                break;
#if false//0
					   case  CONST_misMsgType_applyForChkTasks_qmc:		//  2009/09/10
						     //qisChkTasks(  pProcInfo,  CONST_channelType_robot  );
						     break;
#endif
                            case Consts.CONST_misMsgType_outputTask:
                            default:
                                str = string.Format("µÃµ½Î´´¦ÀíµÄÏûÏ¢: %S", qyGetDesByType1(qyFuncs.CONST_misMsgTypeTable, pMsg->uiType));
                                showInfo_open(0, null, str);
                                break;
                        }


                        //  dwRet  =  WaitForSingleObject(  pProcInfo->robotQ.hSemaTrigger,  50  );
                        //  if  (  dwRet  ==  WAIT_FAILED  )  goto  errLabel;			   
                    }


                    if (0 == (loopCtrl % 5))
                    {

                        //  ÏÂÃæ×¢Òâ°Ñ²»ÓÃµÄÍ¨µÀ¸ø»ØÊÕ»ØÀ´
                        //  for  (  i  =  0;  i  <  mycountof(  pProcInfo->misCnts  );  i  ++  )  
                        for (i = 0; i < 1; i++)
                        {
                            MIS_CNT pMisCnt = pProcInfo.pMisCnt;// getMisCntByIndex(0, pProcInfo, i);  //&pProcInfo->misCnts[i]; 
                            MIS_CHANNEL pChannel = null;
                            //
                            if (null == pMisCnt) continue;
                            //
                            if (0 == pMisCnt.cfg.dynCfg.cnt.usCntPort) continue;

                            if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) continue;
                            //
                            if (null == pChannel.hThread_io) continue;

                            qyFuncs.waitForThread(ref pChannel.hThread_io, 0);
                            if (null == pChannel.hThread_io)
                            {
                                str = string.Format("%s is closed", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));
                                showInfo_open(0, null, str);
                            }


                        }

                        //  ¼ÙÉèÖÐ¼ä¶ÏÏßÁË£¬»òÆäËûÔ­Òò£¬ÔòÒª»Ö¸´ÈÎÎñ
                        //  qTraverse(  &processQ,  (  PF_commonHandler  )tmpHandler_procRobotTask,  0,  0  );
                    }



                }
            }

            iErr = 0;

        errLabel:

            //  for  (  i  =  0;  i  <  mycountof(  pProcInfo->misCnts  );  i  ++  )  
            for (i = 0; i < 1; i++)
            {
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;// getMisCntByIndex(0, pProcInfo, i);  //  &pProcInfo->misCnts[i]; 
                                                    //
                if (null == pMisCnt) continue;
                //
                if (0 == pMisCnt.cfg.dynCfg.cnt.usCntPort) continue;

                waitForChannel(pProcInfo, pMisCnt, Consts.CONST_channelType_robot, true);
            }


            if (pProcInfo != null) pProcInfo.status.threadsStatus.ulbRobotQuit = true;

            traceLogA("mcThreadProc_mis_robot:  leaves");

            //return iErr;
            return;
        }


        //
        public static unsafe int procMsgTask_mc_robot(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_MSG_TASK* pMsg, uint lenInBytes_msg, bool bMsgInput, CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;
            int dataLen = 0;
            uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            uint uiChannelType = Consts.CONST_channelType_talking;
            IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->data.buf;
            object hMainWnd = pQyMc.gui.hMainWnd;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            ushort usRespCode;
            string str;

            //
            using (CQyMalloc mallocObj_rcd = new CQyMalloc())
            {
                ;
                IM_TASK_RCD* pRcd = (IM_TASK_RCD*)mallocObj_rcd.mallocf(sizeof(IM_TASK_RCD));
                if (null == pRcd) return -1;
                ref IM_TASK_RCD rcd = ref *pRcd;
                int tmpiRet;
                //
                int lenInBytes = 0;
                QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
                if (null == pDbFuncs) return -1;
                QM_dbFuncs g_dbFuncs = pDbFuncs;


                if (null == hMainWnd) return -1;
                if (null == pMisCnt) return -1;

                mymemset((IntPtr)pRcd, 0, sizeof(IM_TASK_RCD));

#if false
	 if  (  pCtx->pProcessQ->m_q.cfg.uiMaxQNodes  &&  pCtx->pProcessQ->m_q.uiQNodes  >=  pCtx->pProcessQ->m_q.cfg.uiMaxQNodes  )  {
		 traceLogA(  (char*)  "procMsgTask_mc_robot: processQ has max qNodes"  );
	 }
#endif

                //
                if (0 == pMsg->addr_logicalPeer.idInfo.ui64Id)
                {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                    if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg->data.route, ref pMsg->addr_logicalPeer))
                    {
                        traceLogA("procMsgTask_mc_robot: getIdInfo_logicalPeer failed.");
                        goto errLabel;
                    }
                }


                if (!isUcFlgResp(pMsg->ucFlg))
                {

                    //
                    //  ×¢Òâ£ºÒÔÏÂ£¬bMsgInputÎªFALSEÊ±£¬Îª´ÓÄÚ²¿·¢³ö¡£ÎªTRUEÊ±£¬´ÓÍâ²¿ÊÕÈë£¬ÓÉmgrÖÐ×ª¹ýÀ´
                    //

                    //  Ö÷¶¯·¢³ö
                    //
                    if (!bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendRobotTask)
                    {

                        switch (pContent->uiType)
                        {
                            case Consts.CONST_imCommType_transferFileReq:
                                
                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg, param) <= 0)
                                {
#if false
									//
									//  ÒªÊÇÔÚprocessQÀïÃ»ÓÐ£¬ÄÇÃ´¾Í¼ÓÈë¶ÓÁÐ; 
									//
									pMsg->idInfo_taskSender.ui64Id = pMsg->data.route.idInfo_from.ui64Id;
									pMsg->idInfo_taskReceiver.ui64Id = pMsg->data.route.idInfo_to.ui64Id;
									//
									QM_OBJQ_MEM mem;
									QMEM_MESSENGER_CLI* pQMemObj;
									if (findQMemByKey(0, pMisCnt->pObjQ, &pMsg->idInfo_taskReceiver, &mem))
									{
#if DEBUG
										traceLog((TCHAR*)_T("procMsgTask_mc_robot, not input transferFileReq， findQMemByKey failed"));
#endif
										//										
										showInfo_open(0, 0, _T("procMsgTask_mc_robot, not input transferFileReq， findQMemByKey failed"));
										//
										goto errLabel;
									}
									pQMemObj = (QMEM_MESSENGER_CLI*)mem.pQMemObj;
									if (!pQMemObj) goto errLabel;
									if (pQMemObj->uiType == CONST_objType_imGrp) pContent->transferFileReq.bGrpTask = TRUE; //  2007/09/05, ¸³ÉÏ×é±êÊ¶
																															//
																															//
									if (pCtx->pProcessQ->qPostMsg(pMsg, lenInBytes_msg))
									{
#if __DEBUG__
										traceLog((TCHAR*)_T("procMsgTask_mc_robot, not input transferFileReq， qPostMsg failed"));
#endif
										//
										showInfo_open(0, 0, _T("procMsgTask_mc_robot, not input transferFileReq， qPostMsg failed"));
										//
										goto errLabel;
									}
								//  applyToSend¸ÄÎªwaitToSend, 2007/07/18
#endif
                                    goto errLabel;
                                }

                                /*
								traceLogA(  (char*)  "·"  );
								if  (  !pChannel->hThread_io  )  {
									if  (  startMcIoThread(  pSci,  pMisCnt,  pChannel->uiType,  _T(  "ÒÔ·¢ËÍÎÄ¼þ"  )  )  )  {
										traceLogA(  (char*)  "robot Channel"  );  
										goto  errLabel;
									}
								}
								*/

                                break;
                            case Consts.CONST_imCommType_taskProcReq:
                                switch (pContent->taskProcReq.usOp)
                                {
                                    case Consts.CONST_imOp_send_cancel:
                                    case Consts.CONST_imOp_recv_cancel:

                                        //  2015/09/08
                                        //  pCtx->pProcessQ->qRemoveMsg(  tmpHandler_findOrgMsgAndProc_is,  pMsg,  0  );
                                        //  
                                        showInfo_open(0, null, _T("Not finished. kkkkkkk"));
                                        goto errLabel;

                                        break;
                                    default:
                                        break;
                                }

                                break;
                            default:
#if __DEBUG__
								traceLogA((char*)"pContent->uiType is %d", pContent->uiType);
#endif
                                goto errLabel;
                                break;
                        }

                        //  Í¨¹ýMGR·¢³öÈ¥
                        MSG_ROUTE msgRoute = new MSG_ROUTE();
                        //  if  (  postMsg2Mgr_mc(  pQyMc,  pSci,  pMisCnt,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->uiSeqNo,  (  char  *  )&stream,  lenInBytes,  &pMsg->idInfo_logicalPeer,  &pMsg->data.addr.idInfo_to,  0  )  )  goto  errLabel;
                        if (0 != postMsg2Mgr_mc(null, pMisCnt, ref msgRoute, pMsg->uiType, pMsg->ucFlg, pMsg->usCode, pMsg->tStartTime, pMsg->uiTranNo, pMsg->usSeqNo, (byte*)&pMsg->data.buf, pMsg->lenInBytes - (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf"), pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->data.route.idInfo_to.ui64Id, 0, null, false)) goto errLabel;

                        iErr = 0; goto errLabel;
                    }


                    //  ½ÓÊÕÎÄ¼þµÄÇëÇó°ü, Î´È·ÈÏ£»Ó¦×ª·¢µ½½çÃæÈ¥ÏÔÊ¾
                    //
                    if (bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendRobotTask)
                    {

                        usRespCode = Consts.CONST_qyRc_unknown;
                        object pDb = null;

                        using (CQnmDb db = new CQnmDb())
                        {
                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                            pDb = db.m_pDbMem.pDb;

                            //  2009/10/18
                            pMsg->uiChannelType = pChannel.uiType;

                            //
                            switch (pContent->uiType)
                            {
                                case Consts.CONST_imCommType_transferFileReq:
                                    pMsg->uiTaskType = Consts.CONST_imTaskType_recvFile;
                                    pMsg->iStatus = Consts.CONST_imTaskStatus_applyToRecv;
                                    break;
                                default:
                                    str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent->uiType);
                                    traceLog(str);
                                    goto errLabel;
                                    break;
                            }



                            //  ÕâÀïÒªÈë¿â£¬È»ºóÔÙÍ¨Öª½çÃæ

                            if (0!=msg2TaskRcd(pMisCnt, ref *pMsg, usRespCode, ref *pRcd))
                            {
                                traceLogA("msg2TaskRcd failed"); goto errLabel;
                            }
                            tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref *pRcd, false, "");
                            if (tmpiRet < 0)
                            {
                                traceLogA("mcThreadProc_mis_work(  ): storeTaskInDb failed"); goto errLabel;
                            }
                            if (tmpiRet > 0)
                            {   //  ÕâÀï±íÃ÷Ô­À´ÒÑ¾­ÓÐ¼ÇÂ¼ÁË, Òª¶Ôrcd.iStatus×ö·ÖÎö£¬Èç¹ûÒÑ¾­È¡ÏûÁË£¬ÄÇÃ´¾ÍÒª·¢ËÍÒ»¸öÒÑÈ¡ÏûµÄÏìÓ¦¸ø¶Ô·½
                                switch (pContent->uiType)
                                {
                                    case Consts.CONST_imCommType_transferFileReq:
                                        if (rcd.iStatus != Consts.CONST_imTaskStatus_applyToRecv && rcd.iStatus != Consts.CONST_imTaskStatus_waitToRecv)
                                        {
                                            IM_SIMPLE_RESP respSimple;
                                            //
                                            MACRO_makeImSimpleResp(pMsg->usCode, pContent->uiType, rcd.iStatus, out respSimple);
                                            //
                                            postMsgTask2Mgr_mc(null, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->idInfo_taskSender.ui64Id, pMsg->idInfo_taskReceiver.ui64Id, pMsg->data.route.idInfo_from.ui64Id, uiChannelType, null, false);

                                            traceLogA("ÕâÀïÊÇËµÃ÷´ËÈÎÎñÒÑ½áÊøÁË£¬²»¹ÜÊ²Ã´½á¹û¡£²é³ö½á¹û·¢»ØÈ¥");

                                            iErr = 0; goto errLabel;

                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        //  ×¢Òâ£¬ÕâÀïÒª½«Êý¾Ý¿âÖÐÏàÓ¦µÄID¸ø¸³ÉÏ£¬2007/07/01
                        pMsg->iTaskId = rcd.id;
                        pMsg->uiTaskMode_unused = rcd.uiMode;


                        traceLogA("");


                        if (0 != qPostMsg((IntPtr)pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                        {
                            traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
                        }

                        GuiShare.pf_PostMessage(hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        iErr = 0; goto errLabel;

                    }


                    if (!bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendRobotTaskReply)
                    {

#if __DEBUG__
						traceLogA((char*)"talking");
#endif

                        switch (pContent->uiType)
                        {
                            case Consts.CONST_imCommType_transferFileReplyReq:
                                if (pContent->transferFileReplyReq.usOp != Consts.CONST_imOp_recv_accept)
                                {
                                    //pCtx->pProcessQ->qRemoveMsg(  tmpHandler_findOrgMsgAndProc_is,  pMsg,  0  );	//  ²»ÔÊÐí´«ËÍ					
                                    //  2015/09/08
                                    showInfo_open(0, null, _T("Not finished. kkkk"));
                                    goto errLabel;
                                }
                                else
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    //  ¼ÓÈë¶ÓÁÐ¡£Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
                                    if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg, param) <= 0)
                                    {
                                        //
                                        //  ÒªÊÇÔÚprocessQÀïÃ»ÓÐ£¬ÄÇÃ´¾Í¼ÓÈë¶ÓÁÐ; 
                                        //
                                        pMsg->idInfo_taskSender.ui64Id = pMsg->data.route.idInfo_to.ui64Id;
                                        pMsg->idInfo_taskReceiver.ui64Id = pMsg->data.route.idInfo_from.ui64Id;
                                        //
                                        if (0 != pCtx.pProcessQ.qPostMsg((IntPtr)pMsg, lenInBytes_msg))
                                        {
                                            traceLogA("procMsgTask_mc_robot: 2. ÕâÀï¼Ó²»ÁËÈÎÎñÁË£¬Ó¦¸ÃÍ¨Öª¹ÜÀíÏß³Ì£¬ËµÈÎÎñ¶ÓÁÐÂúÁË¡£²»ÄÜÔÙ¼ÓÁË");
                                            goto errLabel;
                                        }
                                    }

                                    //  2008/11/18
                                    startChannel(pProcInfo, pMisCnt, pChannel, _T("send transferFileReply"));

                                }
                                break;
                            default:
                                str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent->uiType);
                                traceLog(str);
                                goto errLabel;
                                break;
                        }

                        MSG_ROUTE msgRoute = new MSG_ROUTE();
                        if (0 != postMsg2Mgr_mc(null, pMisCnt, ref msgRoute, pMsg->uiType, pMsg->ucFlg, pMsg->usCode, pMsg->tStartTime, pMsg->uiTranNo, pMsg->usSeqNo, (byte*)&pMsg->data.buf, pMsg->lenInBytes - (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf"), pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->data.route.idInfo_to.ui64Id, 0, null, false)) goto errLabel;

                        //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                        iErr = 0; goto errLabel;

                    }



                    //  ·¢ËÍÎÄ¼þµÄ»Ø¸´°ü£¬´ÓmgrÖÐÀ´
                    if (bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendRobotTaskReply)
                    {
                        IM_SIMPLE_RESP respSimple;

                        using (CQnmDb db = new CQnmDb())
                        {
                            ;
                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;

                            if (pContent->uiType == Consts.CONST_imCommType_transferFileReplyReq)
                            {
                                //  char					timeBuf[CONST_qyTimeLen  +  1]  =  "";

                                //  ¶Ô·½²»ÔÊÐí
                                if (pContent->transferFileReplyReq.usOp != Consts.CONST_imOp_recv_accept) goto errLabel;

                                //  
                                if (!g_dbFuncs.pf_bGetImTaskRcdBySth(db.m_pDbMem.pDb, pQyMc.cfg.db.iDbType, false, "", pMisCnt.idInfo, pContent->transferFileReplyReq.tStartTime_org, pContent->transferFileReplyReq.uiTranNo_org, 0, ref *pRcd))
                                {
                                    traceLogA("procMsgTask_mc_robot failed: bGetImTaskRcdBySth failed");
                                    goto errLabel;
                                }

                                //
                                //printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
                                //
                                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                //  
                                if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg, param) <= 0)
                                {
                                    traceLogA("procMsgTask_mc_robot failed: ");
                                    //  postMsg2Mgr...
                                    goto errLabel;
                                }

                            }
                            else
                            {
                                str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent->uiType);
                                traceLog(str);
                                goto errLabel;
                            }
                        }

                        startChannel(pProcInfo, pMisCnt, pChannel, _T("recv transferFileReply"));

                        //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                        usRespCode = Consts.CONST_qyRc_ok;
                        uiChannelType = Consts.CONST_channelType_robot;

                        MACRO_makeImSimpleResp(pMsg->usCode, pContent->uiType, 0, out respSimple);

                        postMsgTask2Mgr_mc(null, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->idInfo_taskSender.ui64Id, pMsg->idInfo_taskReceiver.ui64Id, pMsg->data.route.idInfo_from.ui64Id, uiChannelType, null, false);

                        iErr = 0; goto errLabel;

                    }   //  

                    traceLogA("procMsgTask_mc_robot:  Óöµ½ÁËÎÞ·¨½âÊÍµÄÇé¿ö£¬´íÎó"); goto errLabel;


                }
                else
                {

                    //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                    str = string.Format("µÃµ½%I64uµÄÏìÓ¦%s", pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(CONST_qyRcTable, pMsg->usCode));
                    showInfo_open(0, null, str);

                    if (!isRcOk(pMsg->usCode)) goto errLabel;

                    traceLogA("procMsgTask_mc_robot:  ¶ÔÓÚÏìÓ¦°ü£¬»¹Ã»ÓÐ´¦ÀíÄØ");
                }
            }


            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

            return iErr;
        }


    }
}
