using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

partial class Consts
{
    public const int cntof_filePath = Consts.MAX_PATH + 1;
}

namespace qy
{
    unsafe struct TMP_filePath
    {
        fixed char filePath[Consts.cntof_filePath];
    };

    partial class qyFuncs
    {
        public static unsafe bool canSend(QY_SHARED_OBJ pSharedObj, QMC_taskData_conf pTc)
        {
            bool bRet = true;

            switch (pSharedObj.var.uiType)
            {
                case 0:
                    if (pTc.ucbNotSendData)
                    {
                        bRet = false;
                    }
                    break;
                default:
                    break;
            }

            return bRet;
        }


        //  int  tmpHandler_findOrgMsgAndProcess_mis_procTaskAudioData(  MC_VAR_isCli  *  pProcInfo,  MSGR_ADDR  *  pAddr,  int  iTaskId,  MIS_MSG_TASK  *  pMsgTask,  TRANSFER_AUDIO_DATA  *  pTransferAudioData,  AV_TRAN_INFO  *  pTranInfo,  PROC_TASK_AV  *  pTask  )
        public static unsafe int tmpHandler_findOrgMsgAndProcess_mis_procTaskAudioData(ref CTX_caller ctxCaller, CCtxQmc pProcInfo, ref MSGR_ADDR pAddr, int iTaskId, ref MIS_MSG_TASK pMsgTask, ref TRANSFER_AUDIO_DATA pTransferAudioData1, ref IM_CONTENTU pContentElem, ref PROC_TASK_AV pTask)
        {
            int iErr = -1;
            int index = 0;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "findOrgMsg_procTaskAudioData");
            if (null == pTaskInfo) return -1;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;
            if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();


            fixed (TRANSFER_AUDIO_DATA* pTransferAudioData = &pTransferAudioData1)
            {

                //
                TRANSFER_AUDIO_dataMemHead* pTransferAudioData_mems = (TRANSFER_AUDIO_dataMemHead*)pTransferAudioData->buf_TRANSFER_AUDIO_dataMemHead_mems;

                //
                if (pTask.ucbVideoConferenceStarter)
                {
                    /*
                    int index_activeMems_from = 0;

                    //  search in activeMems_from first
                    for (index_activeMems_from = 0; index_activeMems_from < pTask.usCntLimit_activeMems_from; index_activeMems_from++)
                    {
                        if (pTask.activeMems_from[index_activeMems_from].idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) break;
                    }
                    //  use searched index to process video
                    if (index_activeMems_from < pTask.usCntLimit_activeMems_from)
                    {
                        index = pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                        if (index >= 0 && index < pTask.usCntLimit_mems_from)
                        {
                            TASK_AV_FROM* pFrom = &pTask.pMems_from[index];
                            //
                            if (pFrom->idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id
                                && pFrom->audio.ta_recvd.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                            {
    # if  __DEBUG__
                                //  traceLogA(  "AudioData: usCntLimit_mems_from %d, usCntLimit_activeMems_from %d, video conference starter",  pTransferAvInfoElem->myTask.usCntLimit_mems_from,  pTransferAvInfoElem->myTask.usCntLimit_activeMems_from  );
    #endif
                                //  if  (  procTaskAudio(  pProcInfo,  &pMsgElem->addr_logicalPeer,  pMsgTask,  &pContent->transferAudioData,  &pTransferAvInfoElem->tranInfo,  &pTransferAvInfoElem->myTask,  index,  index_activeMems_from  )  )  goto  errLabel;

                                pTransferAudioData->tmpInternal.param.index_pMems_from = index;
                                pTransferAudioData->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                QIS_DISPATCH* pDispatch = MACRO_getDispatch(*pTask, index_activeMems_from);
                                if (pDispatch)
                                {
                                    q2PostMsgAndTrigger(pMsgTask, offsetof(MIS_MSG_TASK, data) + pMsgTask->lenInBytes, &pDispatch->audio.q2);
                                }

                                iErr = 1; goto errLabel;
                            }
                        }
                    }
                    */
                }
                else if (pTask.bVideoConference())
                {
                    //
                    if (pMsgTask.bFrom_sharedObj)
                    {
                        QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pMsgTask.index_sharedObj);
                        if (null == pSharedObj) goto errLabel;
                        //
                        int i;
                        SHARED_OBJ_USR pSou;
                        for (i = 0; i < (pSharedObj.usrs.Length); i++)
                        {
                            pSou = pSharedObj.usrs[i];
                            if (0 == pSou.iTaskId) continue;
                            if (pSou.iTaskId != iTaskId) continue;

                            //						
                            if (pMsgTask.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                            {

                                //  traceLog(  _T(  "audio data sent by myself"  )  );
                                bool bSend = true;

                                bSend = canSend(pSharedObj, pTc);

                                if (bSend)
                                {
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        q2PostMsg((IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes, pTask.mixer.pOutputQ2);
                                    }
                                    MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
                                    if (null != pChannel)
                                    {

                                        //
                                        chkToApplyForChannel(pProcInfo, pChannel, Consts.CONST_channelType_realTimeMedia);

                                        //
                                        q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);
                                    }
                                }

                                //							
                                iErr = 1; goto errLabel;

                            }

                            //						
                            break;
                        }
                    }



#if false
				if (pMsgTask->iTaskId == iTaskId)
				{
					//  2011/01/23				  
					if (pMsgTask->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
					{
						//  traceLog(  _T(  "audio data sent by myself"  )  );

						q2PostMsg((IntPtr)pMsgTask, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask->lenInBytes, pTask.mixer.pOutputQ2);
						MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
						if (pChannel!=null) q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);

						//
						iErr = 1; goto errLabel;
					}
				}
#endif


                    //
                    if (pTask.mem0_from.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                        && pTask.mem0_from.audio.ta_recvd.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                    {
                        int i = 0;
                        int iPos = 0;
                        TRANSFER_AUDIO_dataMemHead* pMem = null;
                        int index_activeMems_from = 0;
                        uint usIndex_pMems_from = 0;
                        QY_TRANSFORM pTransform = null;
                        //MIS_MSG_TASK* pMsgTask_toTransform = null;
                        TRANSFER_AUDIO_DATA* pNew = null;

                        if (null == pTask.audio.pMsgU_toTransform)
                        {
                            traceLogA("findOrgMsg failed. pMsgU_toTransform is null");
                            goto errLabel;
                        }
                        fixed (MIS_MSG_TASK* pMsgTask_toTransform = &pTask.audio.pMsgU_toTransform.msgU.task)
                        {

                            if (0 == pTransferAudioData->usCnt)
                            {
                                pTransferAudioData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                pTransferAudioData_mems[0].uiLen = pTransferAudioData->uiLen;
                            }
                            if (pTransferAudioData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
					//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: audio len %d"  ),  pTransferAudioData->uiLen  );
#endif


                            for (iPos = 0, i = 0; i < pTransferAudioData->usCnt; iPos = (int)(iPos + pTransferAudioData_mems[i].uiLen), i++)
                            {
                                pMem = &pTransferAudioData_mems[i];
                                if (0 == pMem->uiLen)
                                {
                                    //
#if __DEBUG__
							//traceLogA(  "findOrgMsg: transferAudioMem.uiLen is 0. skipped."  );
#endif
                                    //
                                    continue;
                                }

                                if (iPos + pTransferAudioData_mems[i].uiLen > pTransferAudioData->uiLen)
                                {
#if __DEBUG__
							traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                    goto errLabel;
                                }

                                if (0 == pMem->idInfo.ui64Id) pMem->idInfo.ui64Id = pMsgTask.data.route.idInfo_from.ui64Id;
                                for (index_activeMems_from = 0; index_activeMems_from < videoConference.usCntLimit_activeMems_from; index_activeMems_from++)
                                {
                                    if (pMem->idInfo.ui64Id == videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id
                                        && pMem->uiTranNo_openAvDev == videoConference.activeMems_from[index_activeMems_from].avStream.obj.tranInfo.audio.uiTranNo_openAvDev) break;
                                }
                                if (index_activeMems_from == videoConference.usCntLimit_activeMems_from)
                                {
#if __DEBUG__
							traceLogA("findOrgMsg_procTaskAudioData: can't get index_activeMems_from, skipped.");
#endif
                                    uint uiStep = 0;
                                    qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref uiStep, (""), pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("findOrgMsg: audio, can't get index_activeMems_from, skipped."));

                                    //  2009/06/01
                                    if (pTask.bVideoConference() && !pTask.ucbVideoConferenceStarter)
                                    {
                                        //MIS_CNT* pMisCnt = (MIS_CNT*)pMsgTask->pMisCnt;
                                        pMisCnt.refreshVideoConferenceActiveMems.iTaskId = iTaskId;
                                        pMisCnt.refreshVideoConferenceActiveMems.bNeedRefreshed = true;
                                    }

                                    continue;
                                }
                                //
                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];
                                //  2014/12/12

                                //
                                MACRO_memset_misMsgTask(ref *pMsgTask_toTransform);
                                fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                {
                                    mymemcpy((IntPtr)pMsgTask_toTransform, (IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data"));
                                }
                                pMsgTask_toTransform->data.route.idInfo_from.ui64Id = videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id;
                                pMsgTask_toTransform->data.route.idInfo_to.ui64Id = pMsgTask.data.route.idInfo_to.ui64Id;
                                pNew = (TRANSFER_AUDIO_DATA*)&pMsgTask_toTransform->data.buf;
                                TRANSFER_AUDIO_dataMemHead* pNew_mems = (TRANSFER_AUDIO_dataMemHead*)pNew->buf_TRANSFER_AUDIO_dataMemHead_mems;
                                mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));
                                pNew->uiType = pTransferAudioData->uiType;
                                pNew->uiTranNo_openAvDev = pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev;
                                mymemcpy((IntPtr)(byte*)&pNew_mems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_AUDIO_dataMemHead));
                                pNew->usCnt = 1;
                                pNew->uiLen = pMem->uiLen;
                                mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferAudioData->buf + iPos), pNew->uiLen);

                                //										
                                pNew->tmpInternal.param.index_pMems_from = -1;// pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                                pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                //
                                pMsgTask_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf") + pNew->uiLen;

                                pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                if (null != pTransform)
                                {
                                    //  qPostMsgAndTrigger_toDo(  pMsgTask,  offsetof(  MIS_MSG_TASK,  data  )  +  pMsgTask->lenInBytes,  &pTransform->video  );
                                    q2PostMsgAndTrigger((IntPtr)pMsgTask_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask_toTransform->lenInBytes, pTransform.audio.q2);
                                }
                            }
                        }

                        iErr = 1; goto errLabel;
                    }

                }
                else
                {
                    //
                    if (pMsgTask.bFrom_sharedObj)
                    {
                        QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pMsgTask.index_sharedObj);
                        if (null == pSharedObj) goto errLabel;
                        int i;
                        for (i = 0; i < pSharedObj.usrs.Length; i++)
                        {
                            SHARED_OBJ_USR pSou = pSharedObj.usrs[i];
                            if (0 == pSou.iTaskId) continue;
                            if (pSou.iTaskId != iTaskId) continue;
                            //
                            if (pMsgTask.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                            {
                                //  traceLog(  _T(  "audio data sent by myself"  )  );

                                bool bSend = true;
                                bSend = canSend(pSharedObj, pTc);

                                if (bSend)
                                {
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        q2PostMsg((IntPtr)pMsgTask0, (uint)((int)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes), pTask.mixer.pOutputQ2);
                                    }
                                    MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
                                    if (null != pChannel)
                                    {

                                        //
                                        chkToApplyForChannel(pProcInfo, pChannel, Consts.CONST_channelType_realTimeMedia);

                                        //
                                        q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);
                                    }
                                }
                                //
                                iErr = 1; goto errLabel;

                            }


                            break;
                        }
                    }





                    //
#if false
				if  (pMsgTask->iTaskId  ==  iTaskId  )  {
				  //  2011/01/23				  
				  if  (pMsgTask->data.route.idInfo_from.ui64Id  ==  pMisCnt.idInfo.ui64Id  )  {
					   //  traceLog(  _T(  "audio data sent by myself"  )  );

					   q2PostMsg((IntPtr)pMsgTask, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data")  +  pMsgTask->lenInBytes, pTask.mixer.pOutputQ2  );
		MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
					   if  (null!=pChannel  ) q2PostMsgAndTrigger(  IntPtr.Zero,  0,  pChannel.outputQ2  );

		//
		iErr  =  1;  goto  errLabel;				   
				  }
}
#endif

                    //  2014/11/21
                    if (pTask.applyForProxy.bRecvd_proxyReply)
                    {       //  applier for proxy
                        if (pTask.applyForProxy.proxyReply.proxy.uiTranNo_openAvDev_a == pTransferAudioData->uiTranNo_openAvDev)
                        {
#if __DEBUG__
		traceLog(_T("findOrgMsg: applyForProxy: my pkt is sent back. skipped"));
#endif
                            iErr = 1; goto errLabel;
                        }
                    }


                    //  2014/11/21
                    if (!pTask.receiver.bRecvd_proxiedTranInfo)
                    {  //  一般性的数据包处理，或者是proxy

                        //  search in mems_from directly
                        for (index = 0; index < videoConference.usCntLimit_activeMems_from; index++)
                        {
                            DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index];

                            //
                            if (pActiveMem_from.avStream.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                                && pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                            {
#if __DEBUG__
			//  traceLogA(  "AudioData: usCntLimit_mems_from %d",  pTransferAvInfoElem->myTask.usCntLimit_mems_from  ); 
#endif
                                //  if  (  procTaskAudio(  pProcInfo,  &pMsgElem->addr_logicalPeer,  pMsgTask,  &pContent->transferAudioData,  &pTransferAvInfoElem->tranInfo,  &pTransferAvInfoElem->myTask,  index,  0  )  )  goto  errLabel;

                                pTransferAudioData->tmpInternal.param.index_pMems_from = index;
                                pTransferAudioData->tmpInternal.param.index_activeMems_from = 0;

                                //  2014/11/21
                                if (pTask.doProxy.bExists_avProxy)
                                {

                                    int index_activeMems_from = 0;

                                    //
                                    QIS_DISPATCH pDispatch = MACRO_getDispatch(ref pTask, index_activeMems_from);
                                    if (null != pDispatch)
                                    {
                                        fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                        {
                                            q2PostMsgAndTrigger((IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes, pDispatch.audio.q2);
                                        }
                                    }
                                    iErr = 1; goto errLabel;
                                }

                                //
                                QY_TRANSFORM pTransform = MACRO_getTransform(ref pTask, pTransferAudioData->tmpInternal.param.index_activeMems_from);
                                if (null != pTransform)
                                {
                                    //  qPostMsgAndTrigger_toDo(  pMsgTask,  offsetof(  MIS_MSG_TASK,  data  )  +  pMsgTask->lenInBytes,  &pTransform->audio  );
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        q2PostMsgAndTrigger((IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes, pTransform.audio.q2);
                                    }
                                }

                                iErr = 1; goto errLabel;
                            }
                        }
                        //
                    }
                    else
                    {  //  receiver from proxy				  			 
                        if (pTask.mem0_from.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                            && pTask.mem0_from.audio.ta_recvd.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                        {
                            //  2014/11/21
                            if (pTask.receiver.proxied_from.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                            {
#if __DEBUG__
			traceLog(_T("findOrgMsg: a receiver: my pkt is sent back. skipped"));
#endif
                                iErr = 1; goto errLabel;
                            }

                            //
                            int i = 0;
                            int iPos = 0;
                            TRANSFER_AUDIO_dataMemHead* pMem = null;
                            int index_activeMems_from = 0;
                            uint usIndex_pMems_from = 0;
                            QY_TRANSFORM pTransform = null;
                            //MIS_MSG_TASK* pMsgTask_toTransform = null;
                            TRANSFER_AUDIO_DATA* pNew = null;

                            if (null == pTask.video.pMsgU_toTransform)
                            {
                                traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
                                goto errLabel;
                            }
                            fixed (MIS_MSG_TASK* pMsgTask_toTransform = &pTask.video.pMsgU_toTransform.msgU.task)
                            {
                                ;

                                if (0 == pTransferAudioData->usCnt)
                                {
                                    pTransferAudioData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                    pTransferAudioData_mems[0].uiLen = pTransferAudioData->uiLen;
                                }
                                if (pTransferAudioData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
		//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: video len %d"  ),  pTransferVideoData->uiLen  );
#endif


                                for (iPos = 0, i = 0; i < pTransferAudioData->usCnt; iPos = (int)(iPos + pTransferAudioData_mems[i].uiLen), i++)
                                {
                                    pMem = &pTransferAudioData_mems[i];
                                    if (0 == pMem->uiLen)
                                    {
                                        traceLogA("findOrgMsg: transferAudioMem.uiLen is 0. skipped.");
                                        continue;
                                    }

                                    if (iPos + pTransferAudioData_mems[i].uiLen > pTransferAudioData->uiLen)
                                    {
                                        traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
                                        goto errLabel;
                                    }

                                    //if  (  !pMem->idInfo.ui64Id  )  pMem->idInfo.ui64Id  =  pMsgTask->data.route.idInfo_from.ui64Id;

                                    //											
                                    usIndex_pMems_from = 1;
                                    TASK_AV_FROM pFrom = null;
                                    pFrom = getTaskAvFrom(ref pTask, (int)usIndex_pMems_from, _T(""));
                                    if (null == pFrom) goto errLabel;

                                    //
                                    MACRO_memset_misMsgTask(ref *pMsgTask_toTransform);
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        mymemcpy((IntPtr)pMsgTask_toTransform, (IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data"));
                                    }
                                    pMsgTask_toTransform->data.route.idInfo_from.ui64Id = pFrom.idInfo.ui64Id;
                                    pMsgTask_toTransform->data.route.idInfo_to.ui64Id = pMsgTask.data.route.idInfo_to.ui64Id;
                                    pNew = (TRANSFER_AUDIO_DATA*)&pMsgTask_toTransform->data.buf;
                                    TRANSFER_AUDIO_dataMemHead* pNew_mems = (TRANSFER_AUDIO_dataMemHead*)pNew->buf_TRANSFER_AUDIO_dataMemHead_mems;
                                    mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));
                                    pNew->uiType = pTransferAudioData->uiType;
                                    pNew->uiTranNo_openAvDev = pFrom.audio.ta_recvd.uiTranNo_openAvDev;
                                    mymemcpy((IntPtr)(byte*)&pNew_mems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_AUDIO_dataMemHead));
                                    pNew->usCnt = 1;
                                    pNew->uiLen = pMem->uiLen;
                                    mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferAudioData->buf + iPos), pNew->uiLen);

                                    //										
                                    pNew->tmpInternal.param.index_pMems_from = (int)usIndex_pMems_from;
                                    pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                    //
                                    pMsgTask_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf") + pNew->uiLen;

                                    pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                    if (null != pTransform)
                                    {
                                        //  
                                        q2PostMsgAndTrigger((IntPtr)pMsgTask_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask_toTransform->lenInBytes, pTransform.audio.q2);
                                    }

                                    //  2014/11/11
                                    continue;

                                }
                            }

                            iErr = 1; goto errLabel;
                        }

                        //
                        iErr = 0; goto errLabel;
                    }
                }

            }

            iErr = 0;
        errLabel:
            return iErr;

        }

        //
        public static unsafe int tmpHandler_findOrgMsgAndProcess_mis_procInputAudioData(CTX_caller ctxCaller, CCtxQmc pProcInfo, ref MSGR_ADDR pAddr, int iTaskId, ref MIS_MSG_INPUT pMsgInput, ref TRANSFER_AUDIO_DATA pTransferAudioData1, ref IM_CONTENTU pContentElem, ref PROC_TASK_AV pTask)
        {
            int iErr = -1;
            int index = 0;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            //
            bool bDbg = true;
            string str;

            //
            QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "findOrgMsg_procInputAudioData");
            if (null == pTaskInfo) return -1;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) return -1;
            if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            //
#if DEBUG
            //chkTc(pTc, "find_procInputAudioData enters");
#endif
            fixed (TRANSFER_AUDIO_DATA* pTransferAudioData = &pTransferAudioData1)
            {

                //
                TRANSFER_AUDIO_dataMemHead* pTransferAudioData_mems = (TRANSFER_AUDIO_dataMemHead*)pTransferAudioData->buf_TRANSFER_AUDIO_dataMemHead_mems;
                int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;

                //
                if (pTask.ucbVideoConferenceStarter)
                {
                    /*
                    int index_activeMems_from = 0;

                    //  search in activeMems_from first
                    for (index_activeMems_from = 0; index_activeMems_from < pTask.usCntLimit_activeMems_from; index_activeMems_from++)
                    {
                        if (pTask.activeMems_from[index_activeMems_from].idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) break;
                    }
                    //  use searched index to process video
                    if (index_activeMems_from < pTask.usCntLimit_activeMems_from)
                    {
                        index = pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                        if (index >= 0 && index < pTask.usCntLimit_mems_from)
                        {
                            TASK_AV_FROM* pFrom = &pTask.pMems_from[index];
                            //
                            if (pFrom->idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id
                                && pFrom->audio.ta_recvd.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                            {
    #if __DEBUG__
                                //  traceLogA(  "AudioData: usCntLimit_mems_from %d, usCntLimit_activeMems_from %d, video conference starter",  pTransferAvInfoElem->myTask.usCntLimit_mems_from,  pTransferAvInfoElem->myTask.usCntLimit_activeMems_from  );
    #endif
                                //  if  (  procTaskAudio(  pProcInfo,  &pMsgElem->addr_logicalPeer,  pMsgTask,  &pContent->transferAudioData,  &pTransferAvInfoElem->tranInfo,  &pTransferAvInfoElem->myTask,  index,  index_activeMems_from  )  )  goto  errLabel;

                                pTransferAudioData->tmpInternal.param.index_pMems_from = index;
                                pTransferAudioData->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                QIS_DISPATCH* pDispatch = MACRO_getDispatch(*pTask, index_activeMems_from);
                                if (pDispatch)
                                {
                                    q2PostMsgAndTrigger(pMsgTask, offsetof(MIS_MSG_TASK, data) + pMsgTask->lenInBytes, &pDispatch->audio.q2);
                                }

                                iErr = 1; goto errLabel;
                            }
                        }
                    }
                    */
                }
                else if (pTask.bVideoConference())
                {

                    if (pMsgInput.iTaskId == iTaskId)
                    {
                        //  2011/01/23				  
                        if (pMsgInput.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                        {
                            //  traceLog(  _T(  "audio data sent by myself"  )  );

                            fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                            {
                                q2PostMsg((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pTask.mixer.pOutputQ2);
                            }
                            MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
                            if (pChannel != null) q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);

                            //
                            iErr = 1; goto errLabel;
                        }
                    }

                    if (pTask.mem0_from.idInfo.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                        && pTask.mem0_from.audio.ta_recvd.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                    {
                        int i = 0;
                        int iPos = 0;
                        TRANSFER_AUDIO_dataMemHead* pMem = null;
                        int index_activeMems_from = 0;
                        uint usIndex_pMems_from = 0;
                        QY_TRANSFORM pTransform = null;
                        //MIS_MSG_INPUT* pMsgInput_toTransform = null;
                        TRANSFER_AUDIO_DATA* pNew = null;

                        if (null == pTask.audio.pMsgU_toTransform)
                        {
                            traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
                            goto errLabel;
                        }
                        fixed (MIS_MSG_INPUT* pMsgInput_toTransform = &pTask.audio.pMsgU_toTransform.msgU.input)
                        {

                            if (0 == pTransferAudioData->usCnt)
                            {
                                pTransferAudioData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                pTransferAudioData_mems[0].uiLen = pTransferAudioData->uiLen;
                            }
                            if (pTransferAudioData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
					//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: audio len %d"  ),  pTransferAudioData->uiLen  );
#endif


                            for (iPos = 0, i = 0; i < pTransferAudioData->usCnt; iPos = (int)(iPos + pMem->uiLen), i++)
                            {
                                pMem = getMem(pTransferAudioData_mems, bufSize, i);// &pTransferAudioData_mems[i];
                                if (0 == pMem->uiLen)
                                {
                                    //
#if __DEBUG__
							//traceLogA(  "findOrgMsg: transferAudioMem.uiLen is 0. skipped."  );
#endif
                                    //
                                    continue;
                                }

                                if (iPos + pMem->uiLen > pTransferAudioData->uiLen)
                                {
#if __DEBUG__
							traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                    goto errLabel;
                                }

                                if (0 == pMem->idInfo.ui64Id) pMem->idInfo.ui64Id = pMsgInput.data.route.idInfo_from.ui64Id;
                                //
                                for (index_activeMems_from = 0; index_activeMems_from < videoConference.usCntLimit_activeMems_from; index_activeMems_from++)
                                {
                                    if (pMem->idInfo.ui64Id == videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id
                                        && pMem->uiTranNo_openAvDev == videoConference.activeMems_from[index_activeMems_from].avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
                                    {
                                        break;
                                    }
                                }
                                if (index_activeMems_from == videoConference.usCntLimit_activeMems_from)
                                {
#if __DEBUG__
							traceLogA("findOrgMsg_procTaskAudioData: can't get index_activeMems_from, skipped.");
#endif
                                    uint step = 0;
                                    qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("findOrgMsg: audio, can't get index_activeMems_from, skipped."));

                                    //
                                    iErr = Consts.CONST_qyRet_cantGet_index_activeMems_from; goto errLabel;

#if false
							//  2009/06/01
							if (pTask.ucbVideoConference && !pTask.ucbVideoConferenceStarter)
							{
								//MIS_CNT* pMisCnt = (MIS_CNT*)pMsgTask->pMisCnt;
								pMisCnt.refreshVideoConferenceActiveMems.iTaskId = iTaskId;
								pMisCnt.refreshVideoConferenceActiveMems.bNeedRefreshed = true;
							}

							continue;
#endif
                                }
                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


                                //
                                if (pProcInfo.cfg.debugStatusInfo.ucb_chkInputPts)
                                {
                                    if (0 != pActiveMem_from.last_iPts_a)
                                    {
                                        if (pMem->uiPts != pActiveMem_from.last_iPts_a + 1)
                                        {
                                            str = string.Format("a.act[{0}],{1}:pts err, mem.pts {2}, last_iPts {3}", index_activeMems_from,
                                                pActiveMem_from.avStream.idInfo.ui64Id,
                                                pMem->uiPts,
                                                pActiveMem_from.last_iPts_a);
                                            showInfo_open(0, "", str);

                                        }
                                    }

#if false  //  true
							uint dwTickCnt = myGetTickCount(null);
							int iDiffInMs = (int)(dwTickCnt - pActiveMem_from.dwTickCnt_showPts_a);
							if (Math.Abs(iDiffInMs) > 1000)
							{
								pActiveMem_from.dwTickCnt_showPts_a = dwTickCnt;
								//
								pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
								int nQNodes = 0;
								if (pProcInfo.cfg.b_useAudioQ2_toDecA)
								{
									nQNodes = getQ2Nodes(pTransform.audio.q2);
								}
								else
                                {
									nQNodes = pTransform.audio.qBuf2.getQ2Nodes();
                                }
								//
								str = string.Format("a.act[{0}],{1}: mem.pts {2}, tn {3}, trans.qNodes {4}.", index_activeMems_from,				
									pActiveMem_from.avStream.idInfo.ui64Id,
									pMem->uiPts,
									pMsgInput->uiTranNo,
									nQNodes
									);
								showInfo_open(0, "", str);
							}

#endif

                                }
                                //							
                                pActiveMem_from.last_iPts_a = (int)pMem->uiPts;

                                //
                                if (pActiveMem_from.avStream.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                                {
                                    //skip me
                                    continue;
                                }

                                //
                                MACRO_memset_misMsgInput(pMsgInput_toTransform);
                                fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                {
                                    mymemcpy((IntPtr)pMsgInput_toTransform, (IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
                                }
                                pMsgInput_toTransform->data.route.idInfo_from.ui64Id = videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id;
                                pMsgInput_toTransform->data.route.idInfo_to.ui64Id = pMsgInput.data.route.idInfo_to.ui64Id;
                                pNew = (TRANSFER_AUDIO_DATA*)&pMsgInput_toTransform->data.buf;
                                TRANSFER_AUDIO_dataMemHead* pNew_mems = (TRANSFER_AUDIO_dataMemHead*)pNew->buf_TRANSFER_AUDIO_dataMemHead_mems;
                                mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));
                                pNew->uiType = pTransferAudioData->uiType;
                                pNew->uiTranNo_openAvDev = pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev;
                                mymemcpy((IntPtr)(byte*)&pNew_mems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_AUDIO_dataMemHead));
                                pNew->usCnt = 1;
                                pNew->uiLen = pMem->uiLen;
                                mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferAudioData->buf + iPos), pNew->uiLen);

                                //										
                                pNew->tmpInternal.param.index_pMems_from = -1;// pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                                pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                //
                                pMsgInput_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf") + pNew->uiLen;

                                pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                if (null != pTransform)
                                {
                                    //  
                                    if (pProcInfo.cfg.b_useAudioQ2_toDecA)
                                    {
                                        q2PostMsgAndTrigger((IntPtr)pMsgInput_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput_toTransform->lenInBytes, pTransform.audio.q2);
                                    }
                                    else
                                    {
                                        pTransform.audio.qBuf2.q2PostMsgAndTrigger((IntPtr)pMem, (IntPtr)pNew->buf, pNew->uiLen);
                                    }
                                }
                            }
                        }

                        //
                        iErr = 1; goto errLabel;
                    }

                }
                else
                {

                    if (pMsgInput.iTaskId == iTaskId)
                    {
                        //  2011/01/23				  
                        if (pMsgInput.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                        {
                            //  traceLog(  _T(  "audio data sent by myself"  )  );

                            fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                            {
                                q2PostMsg((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pTask.mixer.pOutputQ2);
                            }
                            MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_realTimeMedia);
                            if (null != pChannel) q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);

                            //
                            iErr = 1; goto errLabel;
                        }
                    }

                    //  2014/11/21
                    if (pTask.applyForProxy.bRecvd_proxyReply)
                    {       //  applier for proxy
                        if (pTask.applyForProxy.proxyReply.proxy.uiTranNo_openAvDev_a == pTransferAudioData->uiTranNo_openAvDev)
                        {
#if __DEBUG__
		traceLog(_T("findOrgMsg: applyForProxy: my pkt is sent back. skipped"));
#endif
                            iErr = 1; goto errLabel;
                        }
                    }


                    //  2014/11/21
                    if (!pTask.receiver.bRecvd_proxiedTranInfo)
                    {  //  一般性的数据包处理，或者是proxy

                        //  search in mems_from directly
                        for (index = 0; index < videoConference.usCntLimit_activeMems_from; index++)
                        {
                            DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index];

                            //
                            if (pActiveMem_from.avStream.idInfo.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                                && pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                            {
#if DEBUG
                                ref TRANSFER_AUDIO_DATA tmp_pTransferAudioData = ref *pTransferAudioData;
                                if (false)
                                {
                                    //  traceLogA(  "AudioData: usCntLimit_mems_from %d",  pTransferAvInfoElem->myTask.usCntLimit_mems_from  ); 
                                    int ii;
                                    if (pTransferAudioData->usCnt == 0)
                                    {
                                        traceLog("transAudioData.usCnt is 0");
                                    }

                                    for (ii = 0; ii < pTransferAudioData->usCnt; ii++)
                                    {
                                        str = string.Format("findOrg: audio.mems[{0}], pts {1}", ii, pTransferAudioData_mems[ii].uiPts);
                                        traceLog(str);

                                    }
                                }
#endif
                                //  if  (  procTaskAudio(  pProcInfo,  &pMsgElem->addr_logicalPeer,  pMsgTask,  &pContent->transferAudioData,  &pTransferAvInfoElem->tranInfo,  &pTransferAvInfoElem->myTask,  index,  0  )  )  goto  errLabel;

                                pTransferAudioData->tmpInternal.param.index_pMems_from = -1;
                                pTransferAudioData->tmpInternal.param.index_activeMems_from = index;

                                //  2014/11/21
                                if (pTask.doProxy.bExists_avProxy)
                                {

                                    int index_activeMems_from = 0;

                                    //
                                    QIS_DISPATCH pDispatch = MACRO_getDispatch(ref pTask, index_activeMems_from);
                                    if (null != pDispatch)
                                    {
                                        fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                        {
                                            q2PostMsgAndTrigger((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pDispatch.audio.q2);
                                        }
                                    }
                                    iErr = 1; goto errLabel;
                                }

                                //
                                QY_TRANSFORM pTransform = MACRO_getTransform(ref pTask, pTransferAudioData->tmpInternal.param.index_activeMems_from);
                                if (null != pTransform)
                                {
                                    if (pProcInfo.cfg.b_useAudioQ2_toDecA)
                                    {
                                        //  qPostMsgAndTrigger_toDo(  pMsgTask,  offsetof(  MIS_MSG_TASK,  data  )  +  pMsgTask->lenInBytes,  &pTransform->audio  );
                                        fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                        {
                                            q2PostMsgAndTrigger((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pTransform.audio.q2);
                                        }
                                    }
                                    else
                                    {
                                        TRANSFER_AUDIO_dataMemHead* pMem;
                                        int iPos;
                                        int i;
                                        for (iPos = 0, i = 0; i < pTransferAudioData->usCnt; iPos = (int)(iPos + pMem->uiLen), i++)
                                        {
                                            pMem = getMem(pTransferAudioData_mems, bufSize, i);// &pTransferAudioData_mems[i];
                                            if (pMem == null) goto errLabel;
#if DEBUG
                                            ref TRANSFER_AUDIO_dataMemHead tmp_pMem = ref *pMem;
#endif
                                            if (0 == pMem->uiLen)
                                            {
                                                //
#if __DEBUG__
							//traceLogA(  "findOrgMsg: transferAudioMem.uiLen is 0. skipped."  );
#endif
                                                //
                                                continue;
                                            }

                                            if (iPos + pMem->uiLen > pTransferAudioData->uiLen)
                                            {
#if __DEBUG__
							traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                                goto errLabel;
                                            }

                                            if (0 == pMem->idInfo.ui64Id) pMem->idInfo.ui64Id = pMsgInput.data.route.idInfo_from.ui64Id;

                                            //
                                            if (0 == pMem->uiTranNo_openAvDev) pMem->uiTranNo_openAvDev = pTransferAudioData->uiTranNo_openAvDev;
                                            byte* data = pTransferAudioData->buf + iPos;
                                            //
                                            pTransform.audio.qBuf2.q2PostMsgAndTrigger((IntPtr)pMem, (IntPtr)data, pMem->uiLen);
                                            //
                                        }

                                        //
                                    }
                                }

                                iErr = 1; goto errLabel;
                            }
                        }
                        //
                    }
                    else
                    {  //  receiver from proxy				  			 
                        if (pTask.mem0_from.idInfo.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                            && pTask.mem0_from.audio.ta_recvd.uiTranNo_openAvDev == pTransferAudioData->uiTranNo_openAvDev)
                        {
                            //  2014/11/21
                            if (pTask.receiver.proxied_from.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                            {
#if __DEBUG__
			traceLog(_T("findOrgMsg: a receiver: my pkt is sent back. skipped"));
#endif
                                iErr = 1; goto errLabel;
                            }

                            //
                            int i = 0;
                            int iPos = 0;
                            TRANSFER_AUDIO_dataMemHead* pMem = null;
                            int index_activeMems_from = 0;
                            uint usIndex_pMems_from = 0;
                            QY_TRANSFORM pTransform = null;
                            //MIS_MSG_INPUT* pMsgInput_toTransform = null;
                            TRANSFER_AUDIO_DATA* pNew = null;

                            if (null == pTask.video.pMsgU_toTransform)
                            {
                                traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
                                goto errLabel;
                            }
                            fixed (MIS_MSG_INPUT* pMsgInput_toTransform = &pTask.video.pMsgU_toTransform.msgU.input)
                            {

                                if (0 == pTransferAudioData->usCnt)
                                {
                                    pTransferAudioData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                    pTransferAudioData_mems[0].uiLen = pTransferAudioData->uiLen;
                                }
                                if (pTransferAudioData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
		//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: video len %d"  ),  pTransferVideoData->uiLen  );
#endif


                                for (iPos = 0, i = 0; i < pTransferAudioData->usCnt; iPos = (int)(iPos + pTransferAudioData_mems[i].uiLen), i++)
                                {
                                    pMem = &pTransferAudioData_mems[i];
                                    if (0 == pMem->uiLen)
                                    {
                                        traceLogA("findOrgMsg: transferAudioMem.uiLen is 0. skipped.");
                                        continue;
                                    }

                                    if (iPos + pTransferAudioData_mems[i].uiLen > pTransferAudioData->uiLen)
                                    {
                                        traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
                                        goto errLabel;
                                    }

                                    //if  (  !pMem->idInfo.ui64Id  )  pMem->idInfo.ui64Id  =  pMsgTask->data.route.idInfo_from.ui64Id;
#if DEBUG
                                    str = string.Format("audioData.mems[{0}] pts {1}", i, pMem->uiPts);
                                    traceLog(str);
#endif

                                    //											
                                    usIndex_pMems_from = 1;
                                    TASK_AV_FROM pFrom = null;
                                    pFrom = getTaskAvFrom(ref pTask, (int)usIndex_pMems_from, _T(""));
                                    if (null == pFrom) goto errLabel;

                                    //
                                    MACRO_memset_misMsgInput(pMsgInput_toTransform);
                                    fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                    {
                                        mymemcpy((IntPtr)pMsgInput_toTransform, (IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
                                    }
                                    pMsgInput_toTransform->data.route.idInfo_from.ui64Id = pFrom.idInfo.ui64Id;
                                    pMsgInput_toTransform->data.route.idInfo_to.ui64Id = pMsgInput.data.route.idInfo_to.ui64Id;
                                    pNew = (TRANSFER_AUDIO_DATA*)&pMsgInput_toTransform->data.buf;
                                    TRANSFER_AUDIO_dataMemHead* pNew_mems = (TRANSFER_AUDIO_dataMemHead*)pNew->buf_TRANSFER_AUDIO_dataMemHead_mems;
                                    mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));
                                    pNew->uiType = pTransferAudioData->uiType;
                                    pNew->uiTranNo_openAvDev = pFrom.audio.ta_recvd.uiTranNo_openAvDev;
                                    mymemcpy((IntPtr)(byte*)&pNew_mems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_AUDIO_dataMemHead));
                                    pNew->usCnt = 1;
                                    pNew->uiLen = pMem->uiLen;
                                    mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferAudioData->buf + iPos), pNew->uiLen);

                                    //										
                                    pNew->tmpInternal.param.index_pMems_from = (int)usIndex_pMems_from;
                                    pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                    //
                                    pMsgInput_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf") + pNew->uiLen;

                                    pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                    if (null != pTransform)
                                    {
                                        //  
                                        q2PostMsgAndTrigger((IntPtr)pMsgInput_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput_toTransform->lenInBytes, pTransform.audio.q2);
                                    }

                                    //  2014/11/11
                                    continue;

                                }
                            }

                            iErr = 1; goto errLabel;
                        }

                        //
                        iErr = 0; goto errLabel;
                    }
                }

            }

            iErr = 0;
        errLabel:

#if DEBUG
            //chkTc(pTc, "find_procInputAudioData leaves");
#endif

            return iErr;

        }


        //
        public static unsafe int tmpHandler_findOrgMsgAndProcess_mis_procTaskVideoData(ref CTX_caller ctxCaller, CCtxQmc pProcInfo, ref MSGR_ADDR pAddr, int iTaskId, ref MIS_MSG_TASK pMsgTask, ref TRANSFER_VIDEO_DATA pTransferVideoData1, ref IM_CONTENTU pContentElem, ref PROC_TASK_AV pTask)
        {
            int iErr = -1;
            int index = 0;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            fixed (TRANSFER_VIDEO_DATA* pTransferVideoData = &pTransferVideoData1)
            {


                TRANSFER_VIDEO_dataMemHead* pTransferVideoDataMems = (TRANSFER_VIDEO_dataMemHead*)pTransferVideoData->buf_TRANSFER_VIDEO_dataMemHead_mems;
                TRANSFER_VIDEO_dataMemHead* pNewMems = null;
                //
                bool bDbg = false;
                string str;

                //
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "findOrgMsg_procTaskVideoData");
                if (null == pTaskInfo) return -1;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null) return -1;
                if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
                QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                //
                if (pTask.ucbVideoConferenceStarter)
                {
                    /*

            if (pContentElem->uiType != CONST_imCommType_transferAvInfo)
            {
    #if __DEBUG__
                traceLog(_T("procTaskVideoData failed, contentType err"));
    #endif
                goto errLabel;
            }

            //  2014/11/11
            if (pMsgTask->iTaskId == iTaskId)
            {
                if (pMsgTask->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                {
                    if (pTransferVideoData->uiTranNo_openAvDev == pContentElem->transferAvInfo.confMosaicTranInfo_video.video.uiTranNo_openAvDev
                        || pTransferVideoData->uiTranNo_openAvDev == pContentElem->transferAvInfo.confMosaicTranInfo_resource.video.uiTranNo_openAvDev)
                    {
                        //
                        dispatch_outputConfVideo(pTask, pMsgTask, pTransferVideoData, &pMisCnt.idInfo, pTransferVideoData->uiTranNo_openAvDev);
                        //			
                        iErr = 1; goto errLabel;
                    }
                }
            }

            //
            int index_activeMems_from = 0;

            //  search in activeMems_from first
            for (index_activeMems_from = 0; index_activeMems_from < pTask.usCntLimit_activeMems_from; index_activeMems_from++)
            {
                if (pTask.activeMems_from[index_activeMems_from].idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) break;
            }
            //  use searched index to process video
            if (index_activeMems_from < pTask.usCntLimit_activeMems_from)
            {
                index = pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                if (index >= 0 && index < pTask.usCntLimit_mems_from)
                {
                    TASK_AV_FROM* pFrom = &pTask.pMems_from[index];
                    //
                    if (pFrom->idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id
                        && pFrom->video.tv_recvd.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                    {
    # ifdef  __DEBUG__		
                        //  traceLogA(  "VideoData: usCntLimit_mems_from %d, usCntLimit_activeMems_from %d, video conference starter",  pTransferAvInfoElem->myTask.usCntLimit_mems_from,  pTransferAvInfoElem->myTask.usCntLimit_activeMems_from  );
    #endif

                    //
                    pTransferVideoData->tmpInternal.param.index_pMems_from = index;
                        pTransferVideoData->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                        //
                        QIS_DISPATCH* pDispatch = MACRO_getDispatch(*pTask, index_activeMems_from);
                        if (pDispatch)
                        {
                            q2PostMsgAndTrigger(pMsgTask, offsetof(MIS_MSG_TASK, data) + pMsgTask->lenInBytes, &pDispatch->video.q2);
                        }

                        iErr = 1; goto errLabel;
                    }
                }
            }
                    */
                }
                else if (pTask.bVideoConference())
                {
                    //
                    //
                    if (pMsgTask.bFrom_sharedObj)
                    {
                        QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pMsgTask.index_sharedObj);
                        if (null == pSharedObj) goto errLabel;
                        int i;
                        for (i = 0; i < (pSharedObj.usrs.Length); i++)
                        {
                            SHARED_OBJ_USR pSou = pSharedObj.usrs[i];
                            if (0 == pSou.iTaskId) continue;
                            if (pSou.iTaskId != iTaskId) continue;

                            //					   
                            if (pMsgTask.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                            {
                                //  traceLog(  _T(  "video data sent by myself"  )  );

                                bool bSend = true;
                                //
                                bSend = canSend(pSharedObj, pTc);

                                if (bSend)
                                {
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        q2PostMsg((IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes, pTask.photomosaic.pOutputQ2);
                                    }
                                    MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_media);
                                    if (null != pChannel)
                                    {

                                        //
                                        chkToApplyForChannel(pProcInfo, pChannel, Consts.CONST_channelType_media);

                                        //
                                        q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);
                                    }
                                }

                                //
                                iErr = 1; goto errLabel;

                            }

                            //
                            break;
                        }
                    }



                    //
#if false
				if (pMsgTask->iTaskId == iTaskId)
		{
			//  2011/01/23				  
			if (pMsgTask->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
			{
				//  traceLog(  _T(  "video data sent by myself"  )  );

				q2PostMsg((IntPtr)pMsgTask, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask->lenInBytes, pTask.photomosaic.pOutputQ2);
				MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_media);
				if (null!=pChannel) q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);

				//
				iErr = 1; goto errLabel;
			}
		}
#endif

                    //
                    if (pTask.mem0_from.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                && pTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                    {
                        int i = 0;
                        int iPos = 0;
                        TRANSFER_VIDEO_dataMemHead* pMem = null;
                        int index_activeMems_from = 0;
                        uint usIndex_pMems_from = 0;
                        QY_TRANSFORM pTransform = null;
                        //MIS_MSG_TASK* pMsgTask_toTransform = null;
                        TRANSFER_VIDEO_DATA* pNew = null;

                        if (null == pTask.video.pMsgU_toTransform)
                        {
#if __DEBUG__
				traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
#endif
                            goto errLabel;
                        }
                        fixed (MIS_MSG_TASK* pMsgTask_toTransform = &pTask.video.pMsgU_toTransform.msgU.task)
                        {

                            if (0 == pTransferVideoData->usCnt)
                            {
                                pTransferVideoData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                pTransferVideoDataMems[0].uiLen = pTransferVideoData->uiLen;
                            }
                            if (pTransferVideoData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
			//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: video len %d"  ),  pTransferVideoData->uiLen  );
#endif


                            for (iPos = 0, i = 0; i < pTransferVideoData->usCnt; iPos = (int)(iPos + pTransferVideoDataMems[i].uiLen), i++)
                            {
                                pMem = &pTransferVideoDataMems[i];
                                if (0 == pMem->uiLen)
                                {
                                    //
#if __DEBUG__
					//traceLogA(  "findOrgMsg: transferVideoMem.uiLen is 0. skipped."  );
#endif
                                    //
                                    continue;
                                }

                                if (iPos + pTransferVideoDataMems[i].uiLen > pTransferVideoData->uiLen)
                                {
#if __DEBUG__
					traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                    goto errLabel;
                                }

                                if (0 == pMem->idInfo.ui64Id) pMem->idInfo.ui64Id = pMsgTask.data.route.idInfo_from.ui64Id;
                                //

                                //
                                for (index_activeMems_from = 0; index_activeMems_from < videoConference.usCntLimit_activeMems_from; index_activeMems_from++)
                                {
                                    if (pMem->idInfo.ui64Id == videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id
                                                                && pMem->uiTranNo_openAvDev == videoConference.activeMems_from[index_activeMems_from].avStream.obj.tranInfo.video.uiTranNo_openAvDev) break;
                                }
                                if (index_activeMems_from == videoConference.usCntLimit_activeMems_from)
                                {
#if __DEBUG__
	traceLogA("findOrgMsg_procTaskVideoData: can't get index_activeMems_from, skipped.");
#endif
                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("findOrgMsg: video, can't get index_activeMems_from, skipped."));

                                    //  2009/06/01
                                    if (pTask.bVideoConference() && !pTask.ucbVideoConferenceStarter)
                                    {
                                        //MIS_CNT* pMisCnt = (MIS_CNT*)pMsgTask->pMisCnt;
                                        pMisCnt.refreshVideoConferenceActiveMems.iTaskId = iTaskId;
                                        pMisCnt.refreshVideoConferenceActiveMems.bNeedRefreshed = true;
                                    }

                                    continue;
                                }

                                //
                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];




                                //
                                MACRO_memset_misMsgTask(ref *pMsgTask_toTransform);
                                fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                {
                                    mymemcpy((IntPtr)pMsgTask_toTransform, (IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data"));
                                }
                                pMsgTask_toTransform->data.route.idInfo_from.ui64Id = videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id;
                                pMsgTask_toTransform->data.route.idInfo_to.ui64Id = pMsgTask.data.route.idInfo_to.ui64Id;
                                pNew = (TRANSFER_VIDEO_DATA*)&pMsgTask_toTransform->data.buf;
                                mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));
                                pNew->uiType = pTransferVideoData->uiType;
                                pNew->uiTranNo_openAvDev = pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev;
                                pNewMems = (TRANSFER_VIDEO_dataMemHead*)pNew->buf_TRANSFER_VIDEO_dataMemHead_mems;
                                mymemcpy((IntPtr)(byte*)&pNewMems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_VIDEO_dataMemHead));
                                pNew->usCnt = 1;
                                pNew->uiLen = pMem->uiLen;
                                mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferVideoData->buf + iPos), pNew->uiLen);

                                //  2017/09/05

                                //										
                                pNew->tmpInternal.param.index_pMems_from = -1;// pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                                pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                //
                                pMsgTask_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") + pNew->uiLen;

                                pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                if (pTransform != null)
                                {
                                    //  
                                    q2PostMsgAndTrigger((IntPtr)pMsgTask_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask_toTransform->lenInBytes, pTransform.v_preTransThread.q2);
                                }

                                //  2014/11/11
                                continue;
                            }
                        }

                        iErr = 1; goto errLabel;
                    }
                }   //  end of processing confMem
                else
                {  //
                   // 
                    if (pMsgTask.bFrom_sharedObj)
                    {
                        QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pMsgTask.index_sharedObj);
                        if (null == pSharedObj) goto errLabel;
                        int i;
                        for (i = 0; i < pSharedObj.usrs.Length; i++)
                        {
                            SHARED_OBJ_USR pSou = pSharedObj.usrs[i];
                            if (0 == pSou.iTaskId) continue;
                            if (pSou.iTaskId != iTaskId) continue;

                            if (pMsgTask.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                            {
                                //  traceLog(  _T(  "video data sent by myself"  )  );				  

                                bool bSend = false;
                                bSend = canSend(pSharedObj, pTc);

                                if (bSend)
                                {
                                    if (isEmpty_ROUTE_sendLocalAv(ref pTaskInfo.var.curRoute_sendLocalAv)) bSend = false;
                                }

                                if (bSend)
                                {

                                    //
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        q2PostMsg((IntPtr)pMsgTask0, (uint)(Marshal.OffsetOf<MIS_MSG_TASK>("data")) + pMsgTask.lenInBytes, pTask.photomosaic.pOutputQ2);
                                    }
                                    MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_media);
                                    if (null != pChannel)
                                    {

                                        //
                                        chkToApplyForChannel(pProcInfo, pChannel, Consts.CONST_channelType_media);

                                        //
                                        q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);
                                    }


                                }

                                //				  
                                iErr = 1; goto errLabel;
                            }


                            break;
                        }

                    }


#if false
				if (pMsgTask->iTaskId == iTaskId)
	{
		//  2011/01/23				  
		if (pMsgTask->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
		{
						//  traceLog(  _T(  "video data sent by myself"  )  );
						if (bDbg)
						{
							int nQNodes = getQ2Nodes(pTask.photomosaic.pOutputQ2);
							str = string.Format("tmp_find_procTaskVideo: to send video data, task.outputQ2.nQNodes {0} ", nQNodes);
							traceLog(str);
						}

			//
			q2PostMsg((IntPtr)pMsgTask, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask->lenInBytes, pTask.photomosaic.pOutputQ2);
			MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_media);
						if (pChannel != null)
						{
							//q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);
							q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.toSendQ2);
						}

			//
			iErr = 1; goto errLabel;
		}
	}
#endif

                    //  2014/11/21
                    if (pTask.applyForProxy.bRecvd_proxyReply)
                    {       //  applier for proxy
                        if (pTask.applyForProxy.proxyReply.proxy.uiTranNo_openAvDev_v == pTransferVideoData->uiTranNo_openAvDev)
                        {
#if __DEBUG__
			traceLog(_T("findOrgMsg: applyForProxy: my pkt is sent back. skipped"));
#endif
                            iErr = 1; goto errLabel;
                        }
                    }

                    //  2014/11/21
                    if (!pTask.receiver.bRecvd_proxiedTranInfo)
                    {  //  一般性的数据包处理，或者是proxy

                        //  search in mems_from directly
                        for (index = 0; index < videoConference.usCntLimit_activeMems_from; index++)
                        {

                            DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index];

                            //
                            if (pActiveMem_from.avStream.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                    && pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                            {
#if __DEBUG__
				//  traceLogA(  "VideoData: usCntLimit_mems_from %d",  pTransferAvInfoElem->myTask.usCntLimit_mems_from  );
#endif
                                //  


                                //
                                pTransferVideoData->tmpInternal.param.index_pMems_from = -1;
                                pTransferVideoData->tmpInternal.param.index_activeMems_from = index;

                                //  2014/11/21
                                if (pTask.doProxy.bExists_avProxy)
                                {

                                    int index_activeMems_from = 0;

                                    //
                                    QIS_DISPATCH pDispatch = MACRO_getDispatch(ref pTask, index_activeMems_from);
                                    if (pDispatch != null)
                                    {
                                        fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                        {
                                            q2PostMsgAndTrigger((IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes, pDispatch.video.q2);
                                        }
                                    }
                                    iErr = 1; goto errLabel;
                                }

                                //
                                QY_TRANSFORM pTransform = MACRO_getTransform(ref pTask, pTransferVideoData->tmpInternal.param.index_activeMems_from);
                                if (pTransform != null)
                                {
                                    //  
                                    QY_Q2 pQ2 = pTransform.v_preTransThread.q2;
                                    if (null == pTransform.v_preTransThread.hThread
                                        && null != pTransform.video.hThread)
                                    {
                                        pQ2 = pTransform.video.q2;
                                    }

                                    //
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        q2PostMsgAndTrigger((IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask.lenInBytes, pQ2);
                                    }
                                }
                                iErr = 1; goto errLabel;
                            }
                        }
                        //
                    }
                    else
                    {  //  receiver from proxy				  			 
                        if (pTask.mem0_from.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                            && pTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                        {
                            //  2014/11/21
                            if (pTask.receiver.proxied_from.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                            {
#if __DEBUG__
				traceLog(_T("findOrgMsg: v receiver: my pkt is sent back. skipped"));
#endif
                                iErr = 1; goto errLabel;
                            }

                            //
                            int i = 0;
                            int iPos = 0;
                            TRANSFER_VIDEO_dataMemHead* pMem = null;
                            int index_activeMems_from = 0;
                            uint usIndex_pMems_from = 0;
                            QY_TRANSFORM pTransform = null;
                            //MIS_MSG_TASK* pMsgTask_toTransform = null;
                            TRANSFER_VIDEO_DATA* pNew = null;

                            if (null == pTask.video.pMsgU_toTransform)
                            {
#if __DEBUG__
				traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
#endif
                                goto errLabel;
                            }
                            fixed (MIS_MSG_TASK* pMsgTask_toTransform = &pTask.video.pMsgU_toTransform.msgU.task)
                            {

                                if (0 == pTransferVideoData->usCnt)
                                {
                                    pTransferVideoData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                    pTransferVideoDataMems[0].uiLen = pTransferVideoData->uiLen;
                                }
                                if (pTransferVideoData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
			//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: video len %d"  ),  pTransferVideoData->uiLen  );
#endif


                                for (iPos = 0, i = 0; i < pTransferVideoData->usCnt; iPos = (int)(iPos + pTransferVideoDataMems[i].uiLen), i++)
                                {
                                    pMem = &pTransferVideoDataMems[i];
                                    if (0 == pMem->uiLen)
                                    {
#if __DEBUG__
					traceLogA("findOrgMsg: transferVideoMem.uiLen is 0. skipped.");
#endif
                                        continue;
                                    }

                                    if (iPos + pTransferVideoDataMems[i].uiLen > pTransferVideoData->uiLen)
                                    {
#if __DEBUG__
					traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                        goto errLabel;
                                    }

                                    //if  (  !pMem->idInfo.ui64Id  )  pMem->idInfo.ui64Id  =  pMsgTask->data.route.idInfo_from.ui64Id;

                                    //											
                                    usIndex_pMems_from = 1;
                                    TASK_AV_FROM pFrom = null;
                                    pFrom = getTaskAvFrom(ref pTask, (int)usIndex_pMems_from, _T(""));
                                    if (null == pFrom) goto errLabel;

                                    //
                                    MACRO_memset_misMsgTask(ref *pMsgTask_toTransform);
                                    fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
                                    {
                                        mymemcpy((IntPtr)pMsgTask_toTransform, (IntPtr)pMsgTask0, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data"));
                                    }
                                    pMsgTask_toTransform->data.route.idInfo_from.ui64Id = pFrom.idInfo.ui64Id;
                                    pMsgTask_toTransform->data.route.idInfo_to.ui64Id = pMsgTask.data.route.idInfo_to.ui64Id;
                                    pNew = (TRANSFER_VIDEO_DATA*)&pMsgTask_toTransform->data.buf;
                                    mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));
                                    pNew->uiType = pTransferVideoData->uiType;
                                    pNew->uiTranNo_openAvDev = pFrom.video.tv_recvd.uiTranNo_openAvDev;
                                    pNewMems = (TRANSFER_VIDEO_dataMemHead*)pNew->buf_TRANSFER_VIDEO_dataMemHead_mems;
                                    mymemcpy((IntPtr)(byte*)&pNewMems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_VIDEO_dataMemHead));
                                    pNew->usCnt = 1;
                                    pNew->uiLen = pMem->uiLen;
                                    mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferVideoData->buf + iPos), pNew->uiLen);

                                    //										
                                    pNew->tmpInternal.param.index_pMems_from = (int)usIndex_pMems_from;
                                    pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                    //
                                    pMsgTask_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") + pNew->uiLen;

                                    pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                    if (pTransform != null)
                                    {
                                        //  
                                        q2PostMsgAndTrigger((IntPtr)pMsgTask_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgTask_toTransform->lenInBytes, pTransform.v_preTransThread.q2);
                                    }

                                    //  2014/11/11
                                    continue;

                                }
                            }

                            iErr = 1; goto errLabel;
                        }

                        //
                        iErr = 0; goto errLabel;
                    }
                }

            }

            iErr = 0;
        errLabel:
            return iErr;
        }



        //
        public static unsafe int tmpHandler_findOrgMsgAndProcess_mis_procInputVideoData(CTX_caller ctxCaller, CCtxQmc pProcInfo, ref MSGR_ADDR pAddr, int iTaskId, ref MIS_MSG_INPUT pMsgInput, ref TRANSFER_VIDEO_DATA pTransferVideoData1, ref IM_CONTENTU pContentElem, ref PROC_TASK_AV pTask)
        {
            int iErr = -1;
            int index = 0;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            fixed (TRANSFER_VIDEO_DATA* pTransferVideoData = &pTransferVideoData1)
            {

                TRANSFER_VIDEO_dataMemHead* pTransferVideoDataMems = (TRANSFER_VIDEO_dataMemHead*)pTransferVideoData->buf_TRANSFER_VIDEO_dataMemHead_mems;
                int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
                TRANSFER_VIDEO_dataMemHead* pNewMems = null;
                //
                bool bDbg = false;
                string str;

                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "findOrgMsg_procInputVideoData");
                if (null == pTaskInfo) return -1;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null) return -1;
                if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
                QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                //
#if DEBUG
                //chkTc(pTc, "find_procInputVideoData enters");
#endif

                //
                if (pTask.ucbVideoConferenceStarter)
                {
                    /*

            if (pContentElem->uiType != CONST_imCommType_transferAvInfo)
            {
    #if __DEBUG__
                traceLog(_T("procTaskVideoData failed, contentType err"));
    #endif
                goto errLabel;
            }

            //  2014/11/11
            if (pMsgTask->iTaskId == iTaskId)
            {
                if (pMsgTask->data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                {
                    if (pTransferVideoData->uiTranNo_openAvDev == pContentElem->transferAvInfo.confMosaicTranInfo_video.video.uiTranNo_openAvDev
                        || pTransferVideoData->uiTranNo_openAvDev == pContentElem->transferAvInfo.confMosaicTranInfo_resource.video.uiTranNo_openAvDev)
                    {
                        //
                        dispatch_outputConfVideo(pTask, pMsgTask, pTransferVideoData, &pMisCnt.idInfo, pTransferVideoData->uiTranNo_openAvDev);
                        //			
                        iErr = 1; goto errLabel;
                    }
                }
            }

            //
            int index_activeMems_from = 0;

            //  search in activeMems_from first
            for (index_activeMems_from = 0; index_activeMems_from < pTask.usCntLimit_activeMems_from; index_activeMems_from++)
            {
                if (pTask.activeMems_from[index_activeMems_from].idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) break;
            }
            //  use searched index to process video
            if (index_activeMems_from < pTask.usCntLimit_activeMems_from)
            {
                index = pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                if (index >= 0 && index < pTask.usCntLimit_mems_from)
                {
                    TASK_AV_FROM* pFrom = &pTask.pMems_from[index];
                    //
                    if (pFrom->idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id
                        && pFrom->video.tv_recvd.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                    {
    # ifdef  __DEBUG__		
                        //  traceLogA(  "VideoData: usCntLimit_mems_from %d, usCntLimit_activeMems_from %d, video conference starter",  pTransferAvInfoElem->myTask.usCntLimit_mems_from,  pTransferAvInfoElem->myTask.usCntLimit_activeMems_from  );
    #endif

                    //
                    pTransferVideoData->tmpInternal.param.index_pMems_from = index;
                        pTransferVideoData->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                        //
                        QIS_DISPATCH* pDispatch = MACRO_getDispatch(*pTask, index_activeMems_from);
                        if (pDispatch)
                        {
                            q2PostMsgAndTrigger(pMsgTask, offsetof(MIS_MSG_TASK, data) + pMsgTask->lenInBytes, &pDispatch->video.q2);
                        }

                        iErr = 1; goto errLabel;
                    }
                }
            }
                    */
                }
                else if (pTask.bVideoConference())
                {

                    //
                    if (pMsgInput.iTaskId == iTaskId)
                    {
                        //  2011/01/23				  
                        if (pMsgInput.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                        {
                            //  traceLog(  _T(  "video data sent by myself"  )  );

                            fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                            {
                                q2PostMsg((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pTask.photomosaic.pOutputQ2);
                            }
                            MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_media);
                            if (null != pChannel) q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);

                            //
                            iErr = 1; goto errLabel;
                        }
                    }

                    //
#if DEBUG
                    ref MIS_MSG_INPUT tmp_pMsgInput = ref pMsgInput;
                    ref TRANSFER_VIDEO_DATA tmp_pTransferVideoData = ref *pTransferVideoData;
#endif

                    //
                    if (pTask.mem0_from.idInfo.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                        && pTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                    {
                        int i = 0;
                        int iPos = 0;
                        TRANSFER_VIDEO_dataMemHead* pMem = null;
                        int index_activeMems_from = 0;
                        uint usIndex_pMems_from = 0;
                        QY_TRANSFORM pTransform = null;
                        //MIS_MSG_INPUT* pMsgInput_toTransform = null;
                        TRANSFER_VIDEO_DATA* pNew = null;

                        if (null == pTask.video.pMsgU_toTransform)
                        {
#if __DEBUG__
				traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
#endif
                            goto errLabel;
                        }
                        fixed (MIS_MSG_INPUT* pMsgInput_toTransform = &pTask.video.pMsgU_toTransform.msgU.input)
                        {

                            if (0 == pTransferVideoData->usCnt)
                            {
                                pTransferVideoData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
                                pTransferVideoDataMems[0].uiLen = pTransferVideoData->uiLen;
                            }
                            if (pTransferVideoData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if DEBUG
                            //  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: video len %d"  ),  pTransferVideoData->uiLen  );

#endif


                            for (iPos = 0, i = 0; i < pTransferVideoData->usCnt; iPos = (int)(iPos + pMem->uiLen), i++)
                            {
                                pMem = getMem(pTransferVideoDataMems, bufSize, i);// &pTransferVideoDataMems[i];
                                if (0 == pMem->uiLen)
                                {
                                    //
#if __DEBUG__
					//traceLogA(  "findOrgMsg: transferVideoMem.uiLen is 0. skipped."  );
#endif
                                    //
                                    continue;
                                }

                                if (iPos + pMem->uiLen > pTransferVideoData->uiLen)
                                {
#if __DEBUG__
					traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                    goto errLabel;
                                }

                                if (0 == pMem->idInfo.ui64Id) pMem->idInfo.ui64Id = pMsgInput.data.route.idInfo_from.ui64Id;

                                //
                                if (videoConference.usCntLimit_activeMems_from > videoConference.activeMems_from.Length)
                                {
                                    str = string.Format("usCntLimit_activeMems_from {0}, activeMems.length {1}", videoConference.usCntLimit_activeMems_from, videoConference.activeMems_from.Length);
                                    int ii = 0;
                                }

                                //
                                for (index_activeMems_from = 0; index_activeMems_from < videoConference.usCntLimit_activeMems_from; index_activeMems_from++)
                                {
                                    if (pMem->idInfo.ui64Id == videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id
                                        && pMem->uiTranNo_openAvDev == videoConference.activeMems_from[index_activeMems_from].avStream.obj.tranInfo.video.uiTranNo_openAvDev) break;
                                }
                                if (index_activeMems_from == videoConference.usCntLimit_activeMems_from)
                                {
                                    //pAddr->idInfo.ui64Id;
                                    str = string.Format("findOrgMsg: video, conf{0}, from {1}, can't get index_activeMems_from, skipped.", pAddr.idInfo.ui64Id, pMem->idInfo.ui64Id);
                                    //
                                    showInfo_open(0, null, str);

                                    //
                                    iErr = Consts.CONST_qyRet_cantGet_index_activeMems_from; goto errLabel;
                                }
                                //
                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];

                                //
                                if (pProcInfo.cfg.debugStatusInfo.ucb_chkInputPts)
                                {
                                    if (0 != pActiveMem_from.last_iPts_v)
                                    {
                                        if (pMem->uiPts != pActiveMem_from.last_iPts_v + 1)
                                        {
                                            str = string.Format("v.act[{0}],{1}:pts err, mem.pts {2}, last_iPts {3}", index_activeMems_from,
                                                pActiveMem_from.avStream.idInfo.ui64Id,
                                                pMem->uiPts,
                                                pActiveMem_from.last_iPts_v);
                                            showInfo_open(0, "", str);

                                        }
                                    }
                                    //
#if DEBUG
#if false
							uint dwTickCnt = myGetTickCount(null);
							int iDiffInMs = (int)(dwTickCnt - pActiveMem_from.dwTickCnt_showPts_v);
							if (Math.Abs(iDiffInMs) > 1000)
							{
								pActiveMem_from.dwTickCnt_showPts_v = dwTickCnt;
								//
								str = string.Format("v.act[{0}],{1}: mem.pts {2}, tn {3}.", index_activeMems_from,				
									pActiveMem_from.avStream.idInfo.ui64Id,
									pMem->uiPts,
									pMsgInput->uiTranNo				
									);
								showInfo_open(0, "", str);
							}

#endif
#endif
                                }
                                //							
                                pActiveMem_from.last_iPts_v = (int)pMem->uiPts;


                                //
                                MACRO_memset_misMsgInput(pMsgInput_toTransform);
                                fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                {
                                    mymemcpy((IntPtr)pMsgInput_toTransform, (IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
                                }
                                pMsgInput_toTransform->data.route.idInfo_from.ui64Id = videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id;
                                pMsgInput_toTransform->data.route.idInfo_to.ui64Id = pMsgInput.data.route.idInfo_to.ui64Id;
                                pNew = (TRANSFER_VIDEO_DATA*)&pMsgInput_toTransform->data.buf;
                                mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));
                                pNew->uiType = pTransferVideoData->uiType;
                                pNew->uiTranNo_openAvDev = pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev;
                                pNewMems = (TRANSFER_VIDEO_dataMemHead*)pNew->buf_TRANSFER_VIDEO_dataMemHead_mems;
                                mymemcpy((IntPtr)(byte*)&pNewMems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_VIDEO_dataMemHead));
                                pNew->usCnt = 1;
                                pNew->uiLen = pMem->uiLen;
                                mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferVideoData->buf + iPos), pNew->uiLen);

                                //  2017/09/05

                                //										
                                pNew->tmpInternal.param.index_pMems_from = -1;// pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
                                pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

                                //
                                pMsgInput_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") + pNew->uiLen;

                                pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
                                if (pTransform != null)
                                {
                                    //
                                    if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                                    {
                                        if (isQ2Warning(pTransform.v_preTransThread.q2, 10))
                                        {

                                            QY_Q2 a = pTransform.v_preTransThread.q2;
                                            QY_Q2_cfg b = a.cfg;
                                            str = "tmp_findOrg_procInputVideoData: " + new string(b.name) + " is full, empty it";
                                            emptyQ2(pTransform.v_preTransThread.q2);
                                            qyFuncs.traceLog(str);
                                        }
                                    }


                                    //  
                                    if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                                    {
                                        q2PostMsgAndTrigger((IntPtr)pMsgInput_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput_toTransform->lenInBytes, pTransform.v_preTransThread.q2);
                                    }
                                    else
                                    {
                                        //
                                        if (false)
                                        {
                                            string fn = Consts.CONST_fn_dump_h264_enc;
                                            //GuiShare.pf_dumpData1(IntPtr.Zero, 0, 0, 0, false, false, fn);
                                            GuiShare.pf_dumpData1((IntPtr)pNew->buf, (int)pMem->uiLen, 0, 0, true, false, fn);
                                        }

                                        //
                                        pTransform.v_preTransThread.qBuf2.q2PostMsgAndTrigger((IntPtr)pMem, (IntPtr)pNew->buf, pNew->uiLen);

                                    }
                                }

                                //  2014/11/11
                                continue;
                            }
                        }

                        iErr = 1; goto errLabel;
                    }
                }   //  end of processing confMem
                else
                {  //  

                    if (pMsgInput.iTaskId == iTaskId)
                    {
                        //  2011/01/23				  
                        if (pMsgInput.data.route.idInfo_from.ui64Id == pMisCnt.idInfo.ui64Id)
                        {
                            //  traceLog(  _T(  "video data sent by myself"  )  );
                            if (bDbg)
                            {
                                int nQNodes = getQ2Nodes(pTask.photomosaic.pOutputQ2);
                                str = string.Format("tmp_find_procTaskVideo: to send video data, task.outputQ2.nQNodes {0} ", nQNodes);
                                traceLog(str);
                            }

                            //
                            fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                            {
                                q2PostMsg((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pTask.photomosaic.pOutputQ2);
                            }
                            MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_media);
                            if (pChannel != null)
                            {
                                //q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.outputQ2);
                                q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.toSendQ2);
                            }

                            //
                            iErr = 1; goto errLabel;
                        }
                    }

                    //  2014/11/21
                    if (pTask.applyForProxy.bRecvd_proxyReply)
                    {       //  applier for proxy
                        if (pTask.applyForProxy.proxyReply.proxy.uiTranNo_openAvDev_v == pTransferVideoData->uiTranNo_openAvDev)
                        {
#if __DEBUG__
			traceLog(_T("findOrgMsg: applyForProxy: my pkt is sent back. skipped"));
#endif
                            iErr = 1; goto errLabel;
                        }
                    }

                    //  2014/11/21
                    if (!pTask.receiver.bRecvd_proxiedTranInfo)
                    {  //  一般性的数据包处理，或者是proxy
#if DEBUG
                        //DLG_TALK_videoConference vc = pTc.videoConference;
#endif

                        //  search in mems_from directly
                        for (index = 0; index < videoConference.usCntLimit_activeMems_from; index++)
                        {
                            DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index];
                            //
                            ref MIS_MSG_INPUT tmp_msgInput = ref pMsgInput;
                            ref TRANSFER_VIDEO_DATA tmp_transferVideoData = ref *pTransferVideoData;

                            //
                            if (pActiveMem_from.avStream.idInfo.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                                && pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
                            {
#if __DEBUG__
				//  traceLogA(  "VideoData: usCntLimit_mems_from %d",  pTransferAvInfoElem->myTask.usCntLimit_mems_from  );
#endif
                                //  


                                //
                                pTransferVideoData->tmpInternal.param.index_pMems_from = -1;// index;
                                pTransferVideoData->tmpInternal.param.index_activeMems_from = index;

                                //  2014/11/21
                                if (pTask.doProxy.bExists_avProxy)
                                {

                                    int index_activeMems_from = 0;

                                    //
                                    QIS_DISPATCH pDispatch = MACRO_getDispatch(ref pTask, index_activeMems_from);
                                    if (pDispatch != null)
                                    {
                                        fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                        {
                                            q2PostMsgAndTrigger((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pDispatch.video.q2);
                                        }
                                    }
                                    iErr = 1; goto errLabel;
                                }

                                //
                                QY_TRANSFORM pTransform = null;
                                pTransform = MACRO_getTransform(ref pTask, pTransferVideoData->tmpInternal.param.index_activeMems_from);
                                //pTransform = pTask.getLastTransform();
                                if (pTransform != null)
                                {
                                    //if (pTransform.index_activeMems_from_decV_bak == index)
                                    {
                                        //  
                                        QY_Q2 pQ2 = pTransform.v_preTransThread.q2;
                                        //
#if false
								if (null == pTransform.v_preTransThread.hThread
									&& null != pTransform.video.hThread)
								{
									pQ2 = pTransform.video.q2;
								}
#endif

                                        //
                                        //q2PostMsgAndTrigger((IntPtr)pMsgInput, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput->lenInBytes, pQ2);

                                        ///////////////////////////////////
                                        ///
                                        {
                                            if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                                            {
                                                //  qPostMsgAndTrigger_toDo(  pMsgTask,  offsetof(  MIS_MSG_TASK,  data  )  +  pMsgTask->lenInBytes,  &pTransform->audio  );
                                                fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
                                                {
                                                    q2PostMsgAndTrigger((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput.lenInBytes, pQ2);
                                                }
                                            }
                                            else
                                            {
                                                TRANSFER_VIDEO_dataMemHead* pMem;
                                                int iPos;
                                                int i;
                                                for (iPos = 0, i = 0; i < pTransferVideoData->usCnt; iPos = (int)(iPos + pMem->uiLen), i++)
                                                {
                                                    pMem = getMem(pTransferVideoDataMems, bufSize, i);// &pTransferAudioData_mems[i];
                                                    if (pMem == null) goto errLabel;
#if DEBUG
                                                    ref TRANSFER_VIDEO_dataMemHead tmp_pMem = ref *pMem;
#endif
                                                    if (0 == pMem->uiLen)
                                                    {
                                                        //
#if __DEBUG__
							//traceLogA(  "findOrgMsg: transferAudioMem.uiLen is 0. skipped."  );
#endif
                                                        //
                                                        continue;
                                                    }

                                                    if (iPos + pMem->uiLen > pTransferVideoData->uiLen)
                                                    {
#if __DEBUG__
							traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
                                                        goto errLabel;
                                                    }

                                                    if (0 == pMem->idInfo.ui64Id) pMem->idInfo.ui64Id = pMsgInput.data.route.idInfo_from.ui64Id;

                                                    //
                                                    if (0 == pMem->uiTranNo_openAvDev) pMem->uiTranNo_openAvDev = pTransferVideoData->uiTranNo_openAvDev;
                                                    byte* data = pTransferVideoData->buf + iPos;
                                                    //
                                                    pTransform.v_preTransThread.qBuf2.q2PostMsgAndTrigger((IntPtr)pMem, (IntPtr)data, pMem->uiLen);
                                                    //
                                                }

                                                //
                                            }
                                        }



                                        ///////////////////////////////////

                                    }
                                }
                                iErr = 1; goto errLabel;
                            }
                        }
                        //
                    }
                    else
                    {
                        //
                        goto errLabel;

#if false
					//  receiver from proxy				  			 
					if (pTask.mem0_from.idInfo.ui64Id == pMsgInput->data.route.idInfo_from.ui64Id
						&& pTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev == pTransferVideoData->uiTranNo_openAvDev)
					{
						//  2014/11/21
						if (pTask.receiver.proxied_from.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
						{
#if __DEBUG__
				traceLog(_T("findOrgMsg: v receiver: my pkt is sent back. skipped"));
#endif
							iErr = 1; goto errLabel;
						}

						//
						int i = 0;
						int iPos = 0;
						TRANSFER_VIDEO_dataMemHead* pMem = null;
						int index_activeMems_from = 0;
						uint usIndex_pMems_from = 0;
						QY_TRANSFORM pTransform = null;
						MIS_MSG_INPUT* pMsgInput_toTransform = null;
						TRANSFER_VIDEO_DATA* pNew = null;

						if (null == pTask.video.pMsgU_toTransform)
						{
#if __DEBUG__
				traceLogA("findOrgMsg failed. pMsgTask_toTransform is null");
#endif
							goto errLabel;
						}
						pMsgInput_toTransform = (MIS_MSG_INPUT*)pTask.video.pMsgU_toTransform;

						if (0 == pTransferVideoData->usCnt)
						{
							pTransferVideoData->usCnt = 1;  //  ¶ÔÀÏµÄÊý¾Ý°ü£¬Õâ¸öusCntÃ»Ìî£¬°üº¬Ò»¸öÊý¾Ý
							pTransferVideoDataMems[0].uiLen = pTransferVideoData->uiLen;
						}
						if (pTransferVideoData->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

#if __DEBUG__
			//  qyShowInfo1(  CONST_qyShowType_debug,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "findOrgMsg: video len %d"  ),  pTransferVideoData->uiLen  );
#endif


						for (iPos = 0, i = 0; i < pTransferVideoData->usCnt; iPos = (int)(iPos + pTransferVideoDataMems[i].uiLen), i++)
						{
							pMem = &pTransferVideoDataMems[i];
							if (0 == pMem->uiLen)
							{
#if __DEBUG__
					traceLogA("findOrgMsg: transferVideoMem.uiLen is 0. skipped.");
#endif
								continue;
							}

							if (iPos + pTransferVideoDataMems[i].uiLen > pTransferVideoData->uiLen)
							{
#if __DEBUG__
					traceLogA("findOrgMsg failed: iPos + mem.uiLen > data.uiLen");
#endif
								goto errLabel;
							}

							//if  (  !pMem->idInfo.ui64Id  )  pMem->idInfo.ui64Id  =  pMsgTask->data.route.idInfo_from.ui64Id;

							//											
							usIndex_pMems_from = 1;
							TASK_AV_FROM pFrom = null;
							pFrom = getTaskAvFrom(ref pTask, (int)usIndex_pMems_from, _T(""));
							if (null == pFrom) goto errLabel;

							//
							MACRO_memset_misMsgInput(pMsgInput_toTransform);
							mymemcpy((IntPtr)pMsgInput_toTransform, (IntPtr)pMsgInput, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
							pMsgInput_toTransform->data.route.idInfo_from.ui64Id = pFrom.idInfo.ui64Id;
							pMsgInput_toTransform->data.route.idInfo_to.ui64Id = pMsgInput->data.route.idInfo_to.ui64Id;
							pNew = (TRANSFER_VIDEO_DATA*)pMsgInput_toTransform->data.buf;
							mymemset((IntPtr)pNew, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));
							pNew->uiType = pTransferVideoData->uiType;
							pNew->uiTranNo_openAvDev = pFrom.video.tv_recvd.uiTranNo_openAvDev;
							pNewMems = (TRANSFER_VIDEO_dataMemHead*)pNew->buf_TRANSFER_VIDEO_dataMemHead_mems;
							mymemcpy((IntPtr)(byte*)&pNewMems[0], (IntPtr)pMem, (uint)sizeof(TRANSFER_VIDEO_dataMemHead));
							pNew->usCnt = 1;
							pNew->uiLen = pMem->uiLen;
							mymemcpy((IntPtr)pNew->buf, (IntPtr)(pTransferVideoData->buf + iPos), pNew->uiLen);

							//										
							pNew->tmpInternal.param.index_pMems_from = (int)usIndex_pMems_from;
							pNew->tmpInternal.param.index_activeMems_from = index_activeMems_from;

							//
							pMsgInput_toTransform->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") + pNew->uiLen;

							pTransform = MACRO_getTransform(ref pTask, index_activeMems_from);
							if (pTransform != null)
							{
								//  
								q2PostMsgAndTrigger((IntPtr)pMsgInput_toTransform, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput_toTransform->lenInBytes, pTransform.v_preTransThread.q2);
							}

							//  2014/11/11
							continue;

						}

						iErr = 1; goto errLabel;
					}

#endif
                        //
                        iErr = 0; goto errLabel;
                    }
                }

            }

            iErr = 0;
        errLabel:

#if DEBUG
            //chkTc(pTc, "find_procInputVideoData leaves");
#endif

            return iErr;
        }






        //
        public static unsafe int procTaskProcReq_applyForKeyFrame(CCtxQmc pProcInfo, ref MSGR_ADDR pAddr, int iTaskId, ref MIS_MSG_TASK pMsgTask, ref TASK_PROC_REQ pTaskProcReq, ref IM_CONTENTU pContentElem, ref PROC_TASK_AV pTask)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;



            //
            showInfo_open(0, null, "procTaskProcReq_applyForKeyFrame called");

            //
            if (pTask.ucbVideoConferenceStarter)
            {
#if false
#if __DEBUG__
				traceLog(_T("Not finished. for conf starter"));
#endif
				//
				if (pTaskProcReq->vStream.uiTranNo_openAvDev == pTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev)
				{
					//
					setFlg_forceKeyFrame(pProcInfo, pTask.iIndex_sharedObj);
					//
					iErr = 0; goto errLabel;
				}

				//  2014/12/06
				if (pContentElem->uiType != CONST_imCommType_transferAvInfo)
				{
					goto errLabel;
				}
				TRANSFER_AV_INFO* pTransferAvInfo = &pContentElem->transferAvInfo;
				//
				if (pTaskProcReq->vStream.uiTranNo_openAvDev == pTransferAvInfo->confMosaicTranInfo_video.video.uiTranNo_openAvDev)
				{
					//
					setFlg_forceKeyFrame(pProcInfo, pTask.confMosaicMaker.iIndex_sharedObj_video);
					//
					iErr = 0; goto errLabel;
				}
				//
				if (pTaskProcReq->vStream.uiTranNo_openAvDev == pTransferAvInfo->confMosaicTranInfo_resource.video.uiTranNo_openAvDev)
				{
					//
					setFlg_forceKeyFrame(pProcInfo, pTask.confMosaicMaker.iIndex_sharedObj_resource);
					//
					iErr = 0; goto errLabel;
				}

				// others
#if __DEBUG__
				traceLog(_T("Not finished. for conf starter"));
#endif
				showInfo_open(0, 0, _T("Not finished: procTaskProcReq_applyForKeyFrame, conf starter: for others"));
				goto errLabel;
#endif
            }
            else if (pTask.bVideoConference())
            {
                setFlg_forceKeyFrame(pProcInfo, pTask.iIndex_sharedObj);
            }
            else
            {
                setFlg_forceKeyFrame(pProcInfo, pTask.iIndex_sharedObj);
            }



            iErr = 0;
        errLabel:
            return iErr;

        }

        public static unsafe int tmpHandler_find_procTaskVideoDataResp(CCtxQmc pProcInfo, ref MIS_MSG_TASK pMsgElem, ref MIS_MSG_INPUT pMsgInput, ref TRANSFER_VIDEO_dataResp pResp)
        {
            int iErr = -1;
            ref IM_CONTENTU pContentElem = ref pMsgElem.data.buf;
            int index = 0;
            //
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            /*
			//
			if (pContentElem->uiType == CONST_imCommType_transferAvInfo)
			{
				AV_TRAN_INFO* pTranInfo = &pContentElem->transferAvInfo.tranInfo;
				PROC_TASK_AV* pTask = get_transferAvInfo_pTask(&pContentElem->transferAvInfo, _T(""));
				if (!pTask) goto errLabel;

				//
				if (pTask.ucbVideoConferenceStarter)
				{

					//
					if (pResp->idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
					{
						if (pResp->uiTranNo_openAvDev == pContentElem->transferAvInfo.confMosaicTranInfo_video.video.uiTranNo_openAvDev
							|| pResp->uiTranNo_openAvDev == pContentElem->transferAvInfo.confMosaicTranInfo_resource.video.uiTranNo_openAvDev)
						{
							int ii = 0;

							if (procTaskVideoDataResp(pProcInfo, pTranInfo, pTask, pMsgInput, pResp)) goto errLabel;

							iErr = 1; goto errLabel;

						}
					}

					//
					int index_activeMems_from = 0;

					//  search in activeMems_from first
					for (index_activeMems_from = 0; index_activeMems_from < pTask.usCntLimit_activeMems_from; index_activeMems_from++)
					{
						if (pTask.activeMems_from[index_activeMems_from].idInfo.ui64Id == pResp->idInfo.ui64Id) break;
					}
					//  use searched index to process video
					if (index_activeMems_from < pTask.usCntLimit_activeMems_from)
					{
						index = pTask.activeMems_from[index_activeMems_from].usIndex_pMems_from;
						if (index >= 0 && index < pTask.usCntLimit_mems_from)
						{
							TASK_AV_FROM* pFrom = &pTask.pMems_from[index];

							if (pFrom->idInfo.ui64Id == pResp->idInfo.ui64Id
							&& pFrom->video.tv_recvd.uiTranNo_openAvDev == pResp->uiTranNo_openAvDev)
							{
								int ii = 0;

								if (procTaskVideoDataResp(pProcInfo, pTranInfo, pTask, pMsgInput, pResp)) goto errLabel;

								iErr = 1; goto errLabel;

							}

						}

					}

				}


				//
				if (pMsgElem->data.route.idInfo_from.ui64Id == pMsgInput->data.route.idInfo_to.ui64Id
					&& pTranInfo->video.uiTranNo_openAvDev == pResp->uiTranNo_openAvDev)
				{

					//  OutputDebugString(  _T(  "VideoData Resp\n"  )  );


					if (procTaskVideoDataResp(pProcInfo, pTranInfo, pTask, pMsgInput, pResp)) goto errLabel;


					iErr = 1; goto errLabel;

				}
			}
			else if (pContentElem->uiType == CONST_imCommType_transferAvReplyInfo)
			{
				AV_TRAN_INFO* pTranInfo = &pContentElem->transferAvReplyInfo.tranInfo;
				PROC_TASK_AV* pTask = get_transferAvReplyInfo_pTask(&pContentElem->transferAvReplyInfo, _T(""));
				if (!pTask) goto errLabel;

				if (pMsgElem->data.route.idInfo_to.ui64Id == pMsgInput->data.route.idInfo_from.ui64Id
					&& pTranInfo->video.uiTranNo_openAvDev == pResp->uiTranNo_openAvDev)
				{
					//  OutputDebugString(  _T(  "VideoData Resp, ½«µ÷ÕûÊÓÆµ·¢ËÍÆµÂÊ\n"  )  );

					if (procTaskVideoDataResp(pProcInfo, pTranInfo, pTask, pMsgInput, pResp)) goto errLabel;

					iErr = 1; goto errLabel;

				}

			}
			*/



            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        public static bool bInActiveMems(CCtxQmc pProcInfo, QY_MESSENGER_ID idInfo, uint tn_a, uint tn_v, TASK_transferAv task)
        {
            bool bRet = false;
            //task.myTask.ucbVideoConference
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(null, pProcInfo, task.m_index_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskInfo.getTaskData();
            if (pTc == null) goto errLabel;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();
            if (videoConference == null) goto errLabel;

            int i;
            for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
            {
                DLG_TALK_videoConferenceActiveMemFrom activeMem = videoConference.activeMems_from[i];
                if (idInfo.ui64Id == activeMem.avStream.idInfo.ui64Id)
                {
                    if (tn_a != 0
                        && tn_a == activeMem.avStream.obj.tranInfo.audio.uiTranNo_openAvDev)
                    {
                        bRet = true; goto errLabel;
                    }
                    if (tn_v != 0
                        && tn_v == activeMem.avStream.obj.tranInfo.video.uiTranNo_openAvDev)
                    {
                        bRet = true; goto errLabel;
                    }
                }
                //
                continue;
            }

        //
        errLabel:
            return bRet;
        }

        public static unsafe bool b_tmpHandler_findOrgMsgAndProc_is(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
        {
            int tmpiRet = tmpHandler_findOrgMsgAndProc_is(ctxCaller, ref pMsg_toBeSearchedParam, p1Param, taskElemParam);
            return (tmpiRet != 0);
        }


        //
        public static unsafe int tmp_fwrite(ref TRANSFER_FILEDATA_RESP pFileDataResp, TASK_transferFile task)
        {
            int iErr = -1;
            string str;
            fixed (byte* pFileDataResp_buf = pFileDataResp.buf)
            {
                if (1 != GuiShare.pf_fwrite(pFileDataResp_buf, pFileDataResp.uiLen, 1, task.myTask.fp))
                {
                    traceLogA("fwite failed");
                    str = string.Format("fwite err");
                    showInfo_open(0, null, str);
                    goto errLabel;
                }
            }

            iErr = 0;

        errLabel:
            return iErr;

        }

        public static unsafe int  tmp_tfopen(ref IM_CONTENTU pContentElem, TASK_transferFile task)
        {
            fixed (char* fileName = pContentElem.transferFileReq.fullFilePath)
            {
                task.myTask.fp = GuiShare.pf_tfopen(new string(fileName), _T("rb"));
            }

            return  0;
        }

        public static unsafe int tmp_fread(ref TRANSFER_FILEDATA_RESP pFileDataResp_o, int nRead, TASK_transferFile task)
        {
            int iErr = -1;

            fixed (byte* buf = pFileDataResp_o.buf)
            {
                if (1 != GuiShare.pf_fread(buf, nRead, 1, task.myTask.fp)) goto errLabel;
            }

            iErr = 0;
            errLabel:
            return iErr;
        }



        public static unsafe string tmp_get_fullFilePath(ref IM_CONTENTU pContentElem)
        {
            string str="";
            fixed (char* fileName = pContentElem.transferFileReq.fullFilePath)
            {
                str = new string(fileName);
            }
            return str;
                
        }

        public static unsafe string tmp_get_fileName(ref IM_CONTENTU pContentElem)
        {
            string str = "";
            fixed (char* fileName = pContentElem.transferFileReq.fileName)
            {
                str = new string(fileName);
            }
            return str;

        }

        public static unsafe int tmp_qPost_msgInput_taskProcReq(ref MIS_MSG_INPUT pMsgInput)        
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
         
            fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)                                                                        
            {
                                                          
                qPostMsg((IntPtr)pMsgInput0, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)sizeof(TASK_PROC_REQ), pQyMc.gui.processQ);
    
            }

            //
            return 0;

}

        //
        public static unsafe int tmp_qPost_msgInput(ref MIS_MSG_INPUT pMsgInput, uint len, GENERIC_Q pQ)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;

            fixed (MIS_MSG_INPUT* pMsgInput0 = &pMsgInput)
            {

                qPostMsg((IntPtr)pMsgInput0, len, pQ);

            }

            //
            return 0;

        }

        public static unsafe int tmp_qPost_msgTask(ref MIS_MSG_TASK pMsgTask, uint len, GENERIC_Q pQ)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;

            fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
            {

                qPostMsg((IntPtr)pMsgTask0, len, pQ);

            }

            //
            return 0;

        }


        //
        //public static unsafe int tmpHandler_findOrgMsgAndProc_is(CTX_caller ctxCaller, object pMsg_toBeSearchedParam, object p1Param, object taskElemParam)
        public static  int tmpHandler_findOrgMsgAndProc_is(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)            
        {

            int iErr = -1;

            bool bDbg = false;
            string str;

#if DEBUG
            bDbg = true;
#endif

            //
            ref MIS_MSGU pMsg_toBeSearched = ref pMsg_toBeSearchedParam;
            //IntPtr p1 = (IntPtr)p1Param;
            ref PARAM_findOrgMsg p1 = ref p1Param;
            //  p1
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            int tmpiRet = 0;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;
            MIS_CNT pMisCntElem = pProcInfo.pMisCnt;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            //
            TASK_common taskElem = (TASK_common)taskElemParam;
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            //
            if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
                //traceLog("for test, tmpHandler_findOrgMsgAndProc_is return 0");
                return 0;
            }
#if DEBUG
            if (pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is)
            {
                bDbg = true;
            }
#endif

            //
            if (bDbg)
            {
                /*
                fixed (char* pName = ctxCaller.tNameBuf.tNameBuf)
                {
                    str = string.Format("tmpHandler_findOrgMsg enters, {0}", new string(pName));
                }
                qyFuncs.showInfo_open(0, null, str);
                qyFuncs.showAllTc(ctxCaller, str);
                */
                qyFuncs.traceLog("findOrgMsg enters.2764");
            }


            //
            //fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
            ref MIS_MSGU pMsgElemParam = ref pTaskData.msgU;
            {

                switch ((pMsgElemParam).uiType)
                {
                    case Consts.CONST_misMsgType_req:

                        /*
                        {
                            ref MIS_MSG_REQ pMsgElem = ref pMsgElemParam.req;
                            //MIS_MSGU* pMsgU = (MIS_MSGU*)pMsgElem;
                            //IM_CONTENTU* pContentElem = M_getMsgContent(pMsgElem->uiType,  pMsgElem->ucFlg, &pMsgElem->data);
                            ref IM_CONTENTU pContentElem = ref pMsgElem.data;// M_getMsgContent(pMsgElem->uiType, pMsgElem->ucFlg, &pMsgElem->data);
                            //
                            switch (pMsg_toBeSearched.uiType)
                            {
                                case Consts.CONST_misMsgType_input:
                                    {
                                        ref MIS_MSG_INPUT pMsgInput = ref pMsg_toBeSearched.input;
                                        //
                                        if (pMsgElem.tStartTime == pMsgInput.tStartTime
                                            && pMsgElem.uiTranNo == pMsgInput.uiTranNo)
                                        {
                                            pMsgInput.usCode_org = pMsgElem.usCode;
                                            iErr = 1; goto errLabel;        //  ÕÒµ½ÁË
                                        }
                                        //
                                    }
                                    break;
                                default:
                                    break;
                            }
                            //
                        }
                        */
                        iErr  =  findOrgMsg_req(ctxCaller, ref pMsg_toBeSearchedParam, p1Param, taskElemParam);  goto errLabel;


                        break;
                    case Consts.CONST_misMsgType_task:

                        iErr = findOrgMsg_task(ctxCaller, ref pMsg_toBeSearchedParam, p1Param, taskElemParam); goto errLabel;

                        break;
                    default:
                        break;
                }
            }




            iErr = 0;
        errLabel:

            //
            if (bDbg)
            {
                str = "tmpHandler_findOrgMsg leaves, iErr " + iErr;
                /*
                fixed (char* pName = ctxCaller.tNameBuf.tNameBuf)
                {
                    str = string.Format("tmpHandler_findOrgMsg leaves, {0}", new string(pName));
                }
                */
                //qyFuncs.showAllTc(ctxCaller, str);
                qyFuncs.traceLog( str);

            }


            return iErr;
        }


        public static int  findOrgMsg_req(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
        {

            int iErr = -1;

            bool bDbg = false;
            string str;

#if DEBUG
            //bDbg = true;
#endif

            //
            ref MIS_MSGU pMsg_toBeSearched = ref pMsg_toBeSearchedParam;
            //IntPtr p1 = (IntPtr)p1Param;
            ref PARAM_findOrgMsg p1 = ref p1Param;
            //  p1
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            int tmpiRet = 0;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;
            MIS_CNT pMisCntElem = pProcInfo.pMisCnt;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            //
            TASK_common taskElem = (TASK_common)taskElemParam;
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            //
            if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
                //traceLog("for test, tmpHandler_findOrgMsgAndProc_is return 0");
                return 0;
            }
#if DEBUG
            if (pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is)
            {
                bDbg = true;
            }
#endif

            //
            if (bDbg)
            {
                /*
                fixed (char* pName = ctxCaller.tNameBuf.tNameBuf)
                {
                    str = string.Format("tmpHandler_findOrgMsg enters, {0}", new string(pName));
                }
                qyFuncs.showInfo_open(0, null, str);
                qyFuncs.showAllTc(ctxCaller, str);
                */
            }


            //
            //fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
            ref MIS_MSGU pMsgElemParam = ref pTaskData.msgU;

            ref MIS_MSG_REQ pMsgElem = ref pMsgElemParam.req;
    //MIS_MSGU* pMsgU = (MIS_MSGU*)pMsgElem;
    //IM_CONTENTU* pContentElem = M_getMsgContent(pMsgElem->uiType,  pMsgElem->ucFlg, &pMsgElem->data);
    ref IM_CONTENTU pContentElem = ref pMsgElem.data;// M_getMsgContent(pMsgElem->uiType, pMsgElem->ucFlg, &pMsgElem->data);
    //
    switch (pMsg_toBeSearched.uiType)
    {
        case Consts.CONST_misMsgType_input:
            {
                ref MIS_MSG_INPUT pMsgInput = ref pMsg_toBeSearched.input;
                //
                if (pMsgElem.tStartTime == pMsgInput.tStartTime
                    && pMsgElem.uiTranNo == pMsgInput.uiTranNo)
                {
                    pMsgInput.usCode_org = pMsgElem.usCode;
                    iErr = 1; goto errLabel;        //  ÕÒµ½ÁË
                }
                //
            }
            break;
        default:
            break;
    }
            //
            iErr = 0;
        errLabel:
            return iErr;
}


        public static int findOrgMsg_task(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
            {

                int iErr = -1;

                bool bDbg = false;
                string str;

#if DEBUG
                bDbg = true;
#endif

                //
                ref MIS_MSGU pMsg_toBeSearched = ref pMsg_toBeSearchedParam;
                //IntPtr p1 = (IntPtr)p1Param;
                ref PARAM_findOrgMsg p1 = ref p1Param;
                //  p1
                //
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                if (null == pProcInfo) return -1;
                int tmpiRet = 0;
                QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
                if (null == pDbFuncs) goto errLabel;
                QM_dbFuncs g_dbFuncs = pDbFuncs;
                MIS_CNT pMisCntElem = pProcInfo.pMisCnt;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;


                //
                TASK_common taskElem = (TASK_common)taskElemParam;
                QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
                if (pTaskInfo == null) goto errLabel;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null) goto errLabel;

                //
                if (pProcInfo.cfg.ucb_USE_specialDbg)
                {
                    //traceLog("for test, tmpHandler_findOrgMsgAndProc_is return 0");
                    return 0;
                }
#if DEBUG
                if (pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is)
                {
                    bDbg = true;
                }
            //
            if (p1Param.bBreak)
            {
                int ii = 0;
            }
#endif

                //
                if (bDbg)
                {
                qyFuncs.traceLog("findOrgMsg_task enters. search msg.type "  +  pMsg_toBeSearched.uiType);

                }


                //
                //fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
                ref MIS_MSGU pMsgElemParam = ref pTaskData.msgU;

                {
                    ref MIS_MSG_TASK pMsgElem = ref pMsgElemParam.task;
                            ref IM_CONTENTU pContentElem = ref pMsgElem.data.buf;
                            //
#if DEBUG
                            ref IM_CONTENTU tmp_pContentElem = ref pContentElem;
                if  (p1Param.bBreak)
                {
                    int ii = 0;
                }
#endif
                            //
                            switch (pMsg_toBeSearched.uiType)
                            {
                                case Consts.CONST_misMsgType_task:

                        
                                    iErr = findOrgMsg_task_searchTask(ctxCaller, ref pMsg_toBeSearchedParam, p1Param, taskElemParam);  goto errLabel;


                                    break;
                                case Consts.CONST_misMsgType_input:

                        iErr = findOrgMsg_task_searchInput(ctxCaller, ref pMsg_toBeSearchedParam, p1Param, taskElemParam);goto errLabel;

                                    break;
                                default:
                                    break;

                            }
                            //
                        }

            iErr = 0;
        errLabel:

            if  (  bDbg)
            {
                qyFuncs.traceLog("findOrgMsg_task leaves");

            }

            return iErr;
                        
        }

        public static int findOrgMsg_task_searchTask(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
            {

                int iErr = -1;

                bool bDbg = false;
                string str;

#if DEBUG
                bDbg = true;
#endif

                //
                ref MIS_MSGU pMsg_toBeSearched = ref pMsg_toBeSearchedParam;
                //IntPtr p1 = (IntPtr)p1Param;
                ref PARAM_findOrgMsg p1 = ref p1Param;
                //  p1
                //
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                if (null == pProcInfo) return -1;
                int tmpiRet = 0;
                QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
                if (null == pDbFuncs) goto errLabel;
                QM_dbFuncs g_dbFuncs = pDbFuncs;
                MIS_CNT pMisCntElem = pProcInfo.pMisCnt;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;


                //
                TASK_common taskElem = (TASK_common)taskElemParam;
                QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
                if (pTaskInfo == null) goto errLabel;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null) goto errLabel;

                //
                if (pProcInfo.cfg.ucb_USE_specialDbg)
                {
                    //traceLog("for test, tmpHandler_findOrgMsgAndProc_is return 0");
                    return 0;
                }
#if DEBUG
                if (pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is)
                {
                    bDbg = true;
                }
#endif

            //
            if (bDbg)
            {
                qyFuncs.traceLog("findOrgMsg_task_searchTask enters");
                //
                

            }


                //
                //fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
                ref MIS_MSGU pMsgElemParam = ref pTaskData.msgU;

            ref MIS_MSG_TASK pMsgElem = ref pMsgElemParam.task;
            ref IM_CONTENTU pContentElem = ref pMsgElem.data.buf;


            {
                

                ref MIS_MSG_TASK pMsgTask = ref pMsg_toBeSearched.task;
                ref IM_CONTENTU pContent = ref pMsgTask.data.buf;

                //
#if __DEBUG__   //  2014/08/28
										if (pContent->uiType == CONST_imCommType_taskProcReq)
										{
											if (pContent->taskProcReq.usOp == CONST_imOp_recv_applyForKeyFrame)
											{
												int i;
												i = 9;
											}
										}
#endif

                //
                if (!isUcFlgResp(pMsgTask.ucFlg))
                {
                    if (pContentElem.uiType == Consts.CONST_imCommType_transferFileReq)
                    {
                        if (pContent.uiType == Consts.CONST_imCommType_transferFileReplyReq)
                        {   //  ÕâÊÇÔÚfile·¢ËÍ¶Ë£¬½ÓÊÕ»Ø¸´À´Ê±µÄ¼ìË÷
                            if (pMsgElem.tStartTime == pContent.transferFileReplyReq.tStartTime_org
                                   && pMsgElem.uiTranNo == pContent.transferFileReplyReq.uiTranNo_org)
                            {
                                //
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - replyReq is matched");
#endif
                                //
                                pMsgTask.addr_logicalPeer.idInfo.ui64Id = pMsgElem.addr_logicalPeer.idInfo.ui64Id;
                                //
                                iErr = 1; goto errLabel;
                            }
                        }
                        if (pContent.uiType == Consts.CONST_imCommType_transferFileReq)
                        {
                            if (pMsgElem.tStartTime == pMsgTask.tStartTime
                                && pMsgElem.uiTranNo == pMsgTask.uiTranNo)
                            {
                                iErr = 1; goto errLabel;
                            }

                        }
                        if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                        {   //  file·¢ËÍ¶Ë£¬×Ô¼ºÓÖ×·¼ÓÁË±ÈÈçÈ¡ÏûµÈ²Ù×÷ºóµÄ¹¤×÷  
                            if (pMsgElem.tStartTime == pContent.taskProcReq.tStartTime_org
                                && pMsgElem.uiTranNo == pContent.taskProcReq.uiTranNo_org)
                            {
                                //
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq is matched");
#endif
                                //
                                iErr = 1; goto errLabel;
                            }
                        }
                    }
                    if (pContentElem.uiType == Consts.CONST_imCommType_transferFileReplyReq)
                    {   //  file½ÓÊÕ¶Ë
                        if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                        {
                            if (pContentElem.transferFileReplyReq.tStartTime_org == pContent.taskProcReq.tStartTime_org
                                && pContentElem.transferFileReplyReq.uiTranNo_org == pContent.taskProcReq.uiTranNo_org)
                            {
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq matched");
#endif
                                iErr = 1; goto errLabel;
                            }
                        }
                    }
                    //  
                    if (pContentElem.uiType == Consts.CONST_imCommType_transferAvInfo)
                    {
                        //  media
                        tmpiRet = findOrgMsg_task_searchTask_av(ctxCaller, ref pMsg_toBeSearchedParam, p1Param, taskElemParam);
                        if (tmpiRet != 0)
                        {
                            iErr = tmpiRet; goto errLabel;
                        }

                    }
                    //  2012/04/19
                    if (pContentElem.uiType == Consts.CONST_imCommType_transferGpsInfo)
                    {                               //  media·¢Æð¶Ë
                                                    //
                    }

                }
                
                //
            }


            iErr = 0;
        errLabel:
            return iErr;
                        
        }

        public static int findOrgMsg_task_searchInput(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
        {

            int iErr = -1;

            bool bDbg = false;
            string str;

#if DEBUG
            //bDbg = true;
#endif

            //
            ref MIS_MSGU pMsg_toBeSearched = ref pMsg_toBeSearchedParam;
            //IntPtr p1 = (IntPtr)p1Param;
            ref PARAM_findOrgMsg p1 = ref p1Param;
            //  p1
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            int tmpiRet = 0;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;
            MIS_CNT pMisCntElem = pProcInfo.pMisCnt;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            //
            TASK_common taskElem = (TASK_common)taskElemParam;
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            //
            if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
                //traceLog("for test, tmpHandler_findOrgMsgAndProc_is return 0");
                return 0;
            }
#if DEBUG
            if (pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is)
            {
                bDbg = true;
            }
            if (p1Param.bBreak)
            {
                int ii = 0;
            }
#endif

            //
            if (bDbg)
            {
                /*
                fixed (char* pName = ctxCaller.tNameBuf.tNameBuf)
                {
                    str = string.Format("tmpHandler_findOrgMsg enters, {0}", new string(pName));
                }
                qyFuncs.showInfo_open(0, null, str);
                qyFuncs.showAllTc(ctxCaller, str);
                */
            }


            //
            //fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
            ref MIS_MSGU pMsgElemParam = ref pTaskData.msgU;

            ref MIS_MSG_TASK pMsgElem = ref pMsgElemParam.task;
            ref IM_CONTENTU pContentElem = ref pMsgElem.data.buf;


            {
                ref MIS_MSG_INPUT pMsgInput = ref pMsg_toBeSearched.input;
                                        ref IM_CONTENTU pContent = ref pMsgInput.data.buf;// M_getMsgContent(pMsgInput->uiType,pMsgInput->ucFlg, &pMsgInput->data);
                                        //
                                        //if (pMsgInput->pMisCnt != pMisCntElem) break;

                                        //									 
#if DEBUG   //  2014/08/28
                                        ref IM_CONTENTU tmp_pContent = ref pContent;

                                        //
                                        if  (p1Param.bBreak)
                                        {
                                            int ii = 0;
                                        }

                                        if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                                        {
                                            if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForKeyFrame)
                                            {
                                                int i;
                                                i = 9;
                                            }
                                            if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForOrgReq)
                                            {
                                                int i;
                                                i = 9;
                                            }
                                        }
#endif

                                        //
                                        if (isUcFlgResp(pMsgInput.ucFlg) && !isUcFlgResp(pMsgElem.ucFlg))
                                        {   //  ½ÓÊÕÎÄ¼þµÄ´¦Àí
                                            switch (pContentElem.uiType)
                                            {
                                                case Consts.CONST_imCommType_transferAvInfo:
                                                    {
                                                        if (!pTaskInfo.var.m_bReplyTask)
                                                        {
                                                            ref TRANSFER_AV_INFO pTransferAvInfoElem = ref pContentElem.transferAvInfo;
                                                            TASK_transferAv task = (TASK_transferAv)taskElem;
                                                            ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvInfo_pTask(pTransferAvInfoElem, _T(""));
                                                                                                         //if (!pTaskElem) goto errLabel;
                                                                                                         //
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferVideoDataResp)
                                                            {
                                                                tmpiRet = tmpHandler_find_procTaskVideoDataResp(pProcInfo, ref pMsgElem, ref pMsgInput, ref pContent.transferVideoDataResp);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }
                                                            }
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAudioDataResp)
                                                            {
                                                                if (pMsgElem.data.route.idInfo_from.ui64Id == pMsgInput.data.route.idInfo_to.ui64Id
                                                                    && pTransferAvInfoElem.ass.tranInfo.audio.uiTranNo_openAvDev == pContent.transferAudioDataResp.uiTranNo_openAvDev)
                                                                {
                                                                    //  OutputDebugString(  _T(  "AudioData Resp, ½«µ÷ÕûÒôÆµ·¢ËÍÆµÂÊ\n"  )  );

                                                                    if (0 != procTaskAudioDataResp(pProcInfo, ref pTransferAvInfoElem.ass.tranInfo, ref pTaskElem, ref pMsgInput, ref pContent.transferAudioDataResp)) goto errLabel;

                                                                    iErr = 1; goto errLabel;
                                                                }
                                                            }
                                                            //
                                                        }
                                                        else
                                                        {
                                                            //case Consts.CONST_imCommType_transferAvReplyInfo:
                                                            TASK_transferAv task = (TASK_transferAv)taskElem;
                                                            ref TRANSFER_AV_replyInfo pTransferAvReplyInfoElem1 = ref pContentElem.transferAvReplyInfo;
                                                            ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvReplyInfo_pTask(pTransferAvProcInfoElem, _T(""));
                                                                                                         //if (!pTaskElem) goto errLabel;
                                                            AV_TRAN_INFO tmp_avTranInfo = pTaskElem.reply_avTranInfo;
                                                            ref AV_TRAN_INFO pAvTranInfoElem = ref tmp_avTranInfo;// &pTransferAvReplyInfoElem1->tranInfo;

                                                            //
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferVideoDataResp)
                                                            {
                                                                tmpiRet = tmpHandler_find_procTaskVideoDataResp(pProcInfo, ref pMsgElem, ref pMsgInput, ref pContent.transferVideoDataResp);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }
                                                            }
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAudioDataResp)
                                                            {
                                                                if (pMsgElem.data.route.idInfo_to.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                                                                    && pAvTranInfoElem.audio.uiTranNo_openAvDev == pContent.transferAudioDataResp.uiTranNo_openAvDev)
                                                                {
                                                                    //  OutputDebugString(  _T(  "AudioDataResp, ½«µ÷ÕûÒôÆµ·¢ËÍÆµÂÊ\n"  )  );

                                                                    if (0 != procTaskAudioDataResp(pProcInfo, ref pAvTranInfoElem, ref pTaskElem, ref pMsgInput, ref pContent.transferAudioDataResp)) goto errLabel;

                                                                    iErr = 1; goto errLabel;
                                                                }
                                                            }
                                                            //
                                                        }
                                                    }
                                                    break;
                                                case Consts.CONST_imCommType_transferGpsInfo:
                                                    {
                                                    }
                                                    break;
                                                case Consts.CONST_imCommType_transferFileReq:
                                                    {
                                                        TASK_transferFile task = (TASK_transferFile)taskElem;
                                                        if (pTaskData.uiType != Consts.CONST_taskDataType_transferFile) goto errLabel;
                                                        QMC_taskData_transferFile pTt = (QMC_taskData_transferFile)pTaskData;
                                                        //
#if DEBUG
                                                        ref MIS_MSG_INPUT tmp_pMsgInput = ref pMsgInput;
#endif
                                                        //
                                                        if ((pMsgInput.tStartTime == task.myTask.reply_info.tStartTime_reply
                                                                 && pMsgInput.uiTranNo == task.myTask.reply_info.uiTranNo_reply)
                                                             || (pMsgInput.tStartTime == task.myTask.reply_info.tStartTime_lastFileDataReq
                                                             && pMsgInput.uiTranNo == task.myTask.reply_info.uiTranNo_lastFileDataReq)
                                                             )
                                                        {
                                                            traceLogA("");

                                                            if (null != p1
                                                                   //&&  pContentElem->uiType  ==  CONST_imCommType_transferFileReplyReq
                                                                   && pContent.uiType == Consts.CONST_imCommType_simpleResp
                                                                   && pContent.simpleResp.uiContentType_org == task.myTask.reply_info.replyReq.uiType)
                                                            {
                                                                ref TRANSFER_FILE_REPLYREQ pFileReplyReq = ref task.myTask.reply_info.replyReq;
                                                                {
                                                                    ;//pContentElem->transferFileReplyReq;

                                                                    if (null == p1.pFileDataReq_o || 0 == task.myTask.ui64FileLen || 0 == task.myTask.uiSizePerSnd_internal)
                                                                    {
#if __DEBUG__
																	traceLogA((char*)"tmpHandler_findOrgMsgAndProc_is failed: Ð£ÑéÌõ¼þ²»Âú×ã1");
#endif
                                                                        goto errLabel;
                                                                    }
                                                                    ref TRANSFER_FILEDATA_REQ pFileDataReq_o = ref p1.pFileDataReq_o.transferFileDataReq;


                                                                    //  ÏÈ½«uiSizePerSndºÍÇëÇó±êÊ¶¸ø¸³ÉÏdataReq
                                                                    pFileDataReq_o.uiSizePerSnd = task.myTask.uiSizePerSnd_internal;
                                                                    pFileDataReq_o.tStartTime_org = pFileReplyReq.tStartTime_org;
                                                                    pFileDataReq_o.uiTranNo_org = pFileReplyReq.uiTranNo_org;
                                                                    pFileDataReq_o.uiSerialNo_org = pFileReplyReq.uiSerialNo_org;
                                                                    //
                                                                    pFileDataReq_o.uiDataBlockIndex = 0;   //  Õâ±íÃ÷ÊÇ´ÓÍ·¿ªÊ¼

                                                                    //
                                                                    task.myTask.reply_info.tStartTime_lastFileDataReq = pFileDataReq_o.tmpInternal.tStartTime_new;
                                                                    task.myTask.reply_info.uiTranNo_lastFileDataReq = pFileDataReq_o.tmpInternal.uiTranNo_new;

                                                                    //		
                                                                    str = string.Format("findOrgMsg: start to retrieve file. total {0} bytes. perSnd {1}", task.myTask.ui64FileLen, pFileDataReq_o.uiSizePerSnd);
                                                                    showInfo_open(0, null, str);

                                                                    //
                                                                    iErr = 1; goto errLabel;
                                                                }
                                                            }

                                                            if (null != p1
                                                                   //&&  pContentElem->uiType  ==  CONST_imCommType_transferFileReplyReq  
                                                                   && pContent.uiType == Consts.CONST_imCommType_transferFileDataResp)
                                                            {
                                                                //
                                                                ref TRANSFER_FILE_REPLYREQ pFileReplyReq = ref task.myTask.reply_info.replyReq;
                                                                {
                                                                    ;//pContentElem->transferFileReplyReq;
                                                                    ref TRANSFER_FILEDATA_RESP pFileDataResp = ref pContent.transferFileDataResp;
                                                                    UInt64 ui64offset = 0;
                                                                    int nWrite = 0;

                                                                    if (null == p1.pContent_o || 0 == task.myTask.ui64FileLen || 0 == task.myTask.uiSizePerSnd_internal || Consts.bufSize_transferFileDataResp < task.myTask.uiSizePerSnd_internal)
                                                                    {
#if __DEBUG__
																	traceLogA((char*)"tmpHandler_findOrgMsgAndProc_is failed: ");
#endif
                                                                        goto errLabel;
                                                                    }
                                                                    ref IM_CONTENTU pContent_o = ref p1.pContent_o.imContentU;


                                                                    if (pContent_o.uiType == Consts.CONST_imCommType_transferFileEndReq)
                                                                    {
                                                                        ref TRANSFER_FILEEND_REQ pFileEndReq_o = ref pContent_o.transferFileEndReq;

                                                                        //  2015/09/08
                                                                        if (pFileEndReq_o.tmpInternal.bChkToRemove_i)
                                                                        {
                                                                            //pFileReplyReq->myTask.bNeedRemoved  =  TRUE;
                                                                            pTaskInfo.var.bNeedRemoved = true;
                                                                            //  2015/09/08
                                                                            pFileEndReq_o.tmpInternal.bNeedRemoved_o = true;
                                                                        }

                                                                        //  
                                                                        pFileEndReq_o.tStartTime_org = pFileReplyReq.tStartTime_org;
                                                                        pFileEndReq_o.uiTranNo_org = pFileReplyReq.uiTranNo_org;
                                                                        pFileEndReq_o.uiSerialNo_org = pFileReplyReq.uiSerialNo_org;

                                                                        iErr = 1; goto errLabel;
                                                                    }

                                                                    if (pContent_o.uiType == Consts.CONST_imCommType_transferFileDataReq)
                                                                    {

                                                                        ref TRANSFER_FILEDATA_REQ pFileDataReq_o = ref pContent_o.transferFileDataReq;

                                                                        //  ÏÈ½«uiSizePerSndºÍÇëÇó±êÊ¶¸ø¸³ÉÏdataReq
                                                                        pFileDataReq_o.uiSizePerSnd = task.myTask.uiSizePerSnd_internal;
                                                                        pFileDataReq_o.tStartTime_org = pFileReplyReq.tStartTime_org;
                                                                        pFileDataReq_o.uiTranNo_org = pFileReplyReq.uiTranNo_org;
                                                                        pFileDataReq_o.uiSerialNo_org = pFileReplyReq.uiSerialNo_org;

#if false//0
													 if  (  pFileDataResp->uiDataBlockIndex  >  pFileReplyReq->uiLastDataBlockIndex  +  1  )  {
														 pFileReplyReq->usErrors  ++  ;	//  ÕâÀïÊÇÓÃÔÚ¶ÔÓÚÀûÓÃÐ­Òé½øÐÐ¹¥»÷Ê±£¬ÅÐ¶Ï´íÎóµ½Ò»¶¨³Ì¶È£¬¼´ÖÕÖ¹´«Êä
														 //  ÖØÐÂ½«uiLastDataBlockIndex¸³¸øreq, ·¢¸ø¿Í»§¶Ë
														 pFileDataReq_o->uiDataBlockIndex  =  pFileReplyReq->uiLastDataBlockIndex;
														 iErr  =  1;  goto  errLabel;

													 }
#endif    //  2009/10/20

                                                                        //
                                                                        if (pProcInfo.cfg.debugStatusInfo.ucbViewFileStatus)
                                                                        {
                                                                            str = string.Format("get pFileDataResp->uiDataBlockIndex %d, %d bytes. tn %d", pFileDataResp.uiDataBlockIndex, pFileDataResp.uiLen, pMsgInput.uiTranNo);
                                                                            showInfo_open(0, null, str);
                                                                        }


                                                                        //
                                                                        {    //  2009/10/20. check whether resp is ok												 
                                                                            bool bRespOk = true;
                                                                            if (0 == pFileDataResp.uiDataBlockIndex)
                                                                            {
                                                                                if (0 != task.myTask.uiLastDataBlockIndex) bRespOk = false;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (pFileDataResp.uiDataBlockIndex != task.myTask.uiLastDataBlockIndex + 1) bRespOk = false;
                                                                            }
                                                                            if (!bRespOk)
                                                                            {
                                                                                //
#if __DEBUG__
																				traceLogA("pFileDataResp->uiDataBlockIndex err");
#endif
                                                                                str = string.Format("Err: pFileDataResp->uiDataBlockIndex %d, fileReplyReq.uiLastDataBlockIndex %d",
                                                                                            pFileDataResp.uiDataBlockIndex, task.myTask.uiLastDataBlockIndex);
                                                                                showInfo_open(0, null, str);
                                                                                //															 
                                                                                task.myTask.usErrors++;
                                                                                //
                                                                                goto errLabel;
                                                                            }
                                                                        }


                                                                        ui64offset = ((UInt64)pFileDataResp.uiDataBlockIndex) * task.myTask.uiSizePerSnd_internal;
                                                                        if (ui64offset >= task.myTask.ui64FileLen)
                                                                        {
                                                                            str = string.Format("ui64offset >= pFileReplyReq->ui64FileLen err");
                                                                            showInfo_open(0, null, str);
                                                                            goto errLabel;
                                                                        }

                                                                        //  ÕâÀïÎª½«µÃµ½µÄÊý¾Ý±£´æ³ÉÎÄ¼þ
                                                                        if (null == task.myTask.fp)
                                                                        {
                                                                            //TCHAR tmpFilePath[MAX_PATH + 1] = _T("");
                                                                            string tmpFilePath = "";
                                                                            //
                                                                            //_sntprintf(tmpFilePath, mycountof(tmpFilePath), _T("%s.tmp"), task->myTask.fullFilePath_save);
                                                                            tmpFilePath = task.myTask.fullFilePath_save + ".tmp";
                                                                            //
                                                                            task.myTask.fp = GuiShare.pf_tfopen(tmpFilePath, _T("wb"));
                                                                            if (null == task.myTask.fp)
                                                                            {
#if __DEBUG__
																				traceLogA((char*)"ÎÄ¼þ½¨Á¢Ê§°Ü");
#endif
                                                                                //
                                                                                //TCHAR tBuf[128];
                                                                                str = string.Format("fileDataResp: fopen %s failed", tmpFilePath);
                                                                                showInfo_open(0, null, str);
                                                                                
                                                                                showNotification(0, 0, 0, 0, 0, 0, str);
                                                                                //
                                                                                goto errLabel;
                                                                            }
                                                                            //
                                                                            pTt.dwStartTickCnt = myGetTickCount(null);
                                                                            pTt.dwLastReportTickCnt = pTt.dwStartTickCnt;
                                                                            task.myTask.ucPercent = 0;
                                                                            //
                                                                            //  
                                                                            //
                                                                            showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, pMsgElem.idInfo_taskReceiver.ui64Id, false, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, Consts.CONST_imTaskStatus_receiving, 0, 0, null, null);

                                                                        }

                                                                        //  
                                                                        //  if  (  fseek(  pFileReplyReq->fp,  (  long  )ui64offset,  SEEK_SET  )  )  
                                                                        if (0 != GuiShare.pf_fseeki64(task.myTask.fp, (Int64)ui64offset, 0//SEEK_SET
                                                                                                                              ))
                                                                        {
                                                                            traceLogA("tmpHandler_findOrgMsgAndProc_is failed:  _fseeki64 err");
                                                                            str = string.Format(("_fseeki64 err"));
                                                                            showInfo_open(0, null, str);
                                                                            goto errLabel;
                                                                        }

                                                                        nWrite = (int)Math.Min(task.myTask.ui64FileLen - ui64offset, task.myTask.uiSizePerSnd_internal);
                                                                        if (pFileDataResp.uiLen != nWrite)
                                                                        {
                                                                            traceLogA("pFileDataResp->uiLen  !=  nWrite");
                                                                            str = string.Format("pFileDataResp->uiLen!=nWrite err,%d,%I64u,%I64u,%d,%d", nWrite, task.myTask.ui64FileLen, ui64offset, task.myTask.uiSizePerSnd_internal, pFileDataResp.uiLen);
                                                                            showInfo_open(0, null, str);
                                                                            goto errLabel;
                                                                        }

                                                                        tmp_fwrite(ref pFileDataResp, task);

                                                                        //
                                                                        task.myTask.uiLastDataBlockIndex = pFileDataResp.uiDataBlockIndex;


                                                                        //  
                                                                        uint dwCurTickCnt;
                                                                        //
                                                                        dwCurTickCnt = myGetTickCount(null);
                                                                        if (dwCurTickCnt > pTt.dwLastReportTickCnt + 1000)
                                                                        {   //  
                                                                            pTt.dwLastReportTickCnt = dwCurTickCnt;
                                                                            //
                                                                            float f = 0;
                                                                            byte ucPercent;
                                                                            //
                                                                            f = (float)task.myTask.uiLastDataBlockIndex * task.myTask.uiSizePerSnd_internal;
                                                                            f = f / task.myTask.ui64FileLen;
                                                                            ucPercent = (byte)(f * 100);

                                                                            //  if  (  ucPercent  !=  pFileReplyReq->ucPercent  )  
                                                                            {   //  ±¨¸æ×´Ì¬

                                                                                showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, pMsgElem.idInfo_taskReceiver.ui64Id, false, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, Consts.CONST_imTaskStatus_receiving, ucPercent, task.myTask.uiLastDataBlockIndex * task.myTask.uiSizePerSnd_internal, null, null);

                                                                            }
                                                                        }


                                                                        pFileDataReq_o.uiDataBlockIndex = task.myTask.uiLastDataBlockIndex + 1;


                                                                        if (pFileDataReq_o.uiDataBlockIndex * task.myTask.uiSizePerSnd_internal >= task.myTask.ui64FileLen)
                                                                        {
                                                                            //  Òª°ÑÁÙÊ±ÎÄ¼þÒÆ³ÉÕýÊ½ÎÄ¼þ
                                                                            //TCHAR tmpFilePath[MAX_PATH + 1] = _T("");
                                                                            string tmpFilePath;
                                                                            //
                                                                            str = string.Format(("all file data received. %I64u bytes. to copyfile"), task.myTask.ui64FileLen);
                                                                            //
                                                                            if (null != task.myTask.fp)
                                                                            {
#if __DEBUG__
																				traceLogA("tmpHandler_findOrgMsgAndProc_is: before fclose");
#endif
                                                                                GuiShare.pf_fclose(task.myTask.fp); task.myTask.fp = null;
#if __DEBUG__
																				traceLogA((char*)"tmpHandler_findOrgMsgAndProc_is: after fclose");
#endif
                                                                            }
                                                                            //
                                                                            //_sntprintf(tmpFilePath, mycountof(tmpFilePath), _T("%s.tmp"), task->myTask.fullFilePath_save);
                                                                            //fixed (char* fullFilePath_save = task.myTask.fullFilePath_save)
                                                                            string fullFilePath_save = task.myTask.fullFilePath_save;
                                                                            {
                                                                               // tmpFilePath = new string(fullFilePath_save) + ".tmp";
                                                                               tmpFilePath = fullFilePath_save+ ".tmp";
                                                                            }
                                                                            //
                                                                            if (!GuiShare.pf_CopyFile(tmpFilePath, task.myTask.fullFilePath_save, false))
                                                                            {
                                                                                //TCHAR tBuf[256] = _T("");
#if __DEBUG__
																				traceLog((TCHAR*)_T("CopyFile %s->%s failed"), tmpFilePath, task->myTask.fullFilePath_save);
#endif
                                                                                str = string.Format(("copyfile failed. %s->%s"), tmpFilePath, task.myTask.fullFilePath_save);
                                                                                showInfo_open(0, null, str);
                                                                                str = string.Format(("Copy failed. %s"), task.myTask.fileName_transferred);
                                                                                showNotification(0, 0, 0, 0, 0, 0, str);
                                                                                goto errLabel;
                                                                            }
                                                                            GuiShare.pf_DeleteFile(tmpFilePath);

                                                                            {
                                                                                object pDb = null;

                                                                                using (CQnmDb db = new CQnmDb())
                                                                                {
                                                                                    ;
                                                                                    if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                                                                    pDb = db.m_pDbMem.pDb;

                                                                                    if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, Consts.CONST_imTaskStatus_recvFinished, pMsgElem.iTaskId)) goto errLabel;
                                                                                }
                                                                            }

                                                                            showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, pMsgElem.idInfo_taskReceiver.ui64Id, false, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, Consts.CONST_imTaskStatus_recvFinished, 100, task.myTask.ui64FileLen, task.myTask.fileName_transferred, task.myTask.fullFilePath_save);

                                                                            //
                                                                            pFileDataReq_o.tmpInternal.ucbFileEnd = true;
                                                                            traceLogA("bFileEnd is true");

                                                                            //
                                                                            str = string.Format("Transfering file ends.");
                                                                            showInfo_open(0, null, str);

                                                                        }

                                                                        //
                                                                        if (!pFileDataReq_o.tmpInternal.ucbFileEnd)
                                                                        {
                                                                            task.myTask.reply_info.tStartTime_lastFileDataReq = pFileDataReq_o.tmpInternal.tStartTime_new;
                                                                            task.myTask.reply_info.uiTranNo_lastFileDataReq = pFileDataReq_o.tmpInternal.uiTranNo_new;
                                                                        }

                                                                        //
                                                                        iErr = 1; goto errLabel;
                                                                    }

                                                                    traceLogA("");
                                                                }
                                                            }

                                                            iErr = 1; goto errLabel;
                                                        }
                                                        //
                                                    }

                                                    break;

                                                default:
                                                    break;
                                            }

                                            if (pMsgInput.tStartTime == pMsgElem.tStartTime
                                                && pMsgInput.uiTranNo == pMsgElem.uiTranNo)
                                            {
                                                traceLogA("");

                    
                                                /////////////////////////
                                                iErr = 1; goto errLabel;
                                            }
                                        }
                                        if (!isUcFlgResp(pMsgInput.ucFlg) && !isUcFlgResp(pMsgElem.ucFlg))
                                        {
                                            switch (pContentElem.uiType)
                                            {
                                                case Consts.CONST_imCommType_transferAvInfo:
                                                    {
                                                        if (!pTaskInfo.var.m_bReplyTask)
                                                        {
                                                            ref TRANSFER_AV_INFO pTransferAvInfoElem = ref pContentElem.transferAvInfo;
                                                            TASK_transferAv task = (TASK_transferAv)taskElem;
                                                            ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvInfo_pTask(pTransferAvInfoElem, _T(""));

                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                                                            {
                                                                if (pMsgElem.tStartTime == pContent.transferAvReplyInfo.tStartTime_org
                                                                    && pMsgElem.uiTranNo == pContent.transferAvReplyInfo.uiTranNo_org)
                                                                {
                                                                    //OutputDebugString(_T("tmpHandler_findOrgMsgAndProc_is . msg - reply is matched\n"));            //  

                                                                    procTaskAvReply(pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pTransferAvInfoElem.ass.tranInfo, ref pTaskElem, ref pMsgInput);

                                                                    //
                                                                    iErr = 1; goto errLabel;
                                                                }
                                                            }
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferVideoData)
                                                            {                           //  
                                                                                        //
                                                                tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procInputVideoData(ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgInput, ref pContent.transferVideoData, ref pContentElem, ref pTaskElem);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }
                                                            }
                                                            //////////////////////////
                                                            ///
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAudioData)
                                                            {
                                                                //
                                                                tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procInputAudioData(ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgInput, ref pContent.transferAudioData, ref pContentElem, ref pTaskElem);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }
                                                            }
                                                            //
                                                            if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                                                            {
                                                                if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForKeyFrame)
                                                                {
                                                                    int ii = 0;


                                                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("recv applyforKeyFrame from %I64u, tn_openAvDev %d. matched iTaskId %d"), pMsgInput->data.route.idInfo_from.ui64Id, pContent->taskProcReq.vStream.uiTranNo_openAvDev, pMsgElem->iTaskId);

                                                                    bool tmp_bFound = false;
                                                                    //
                                                                    if (pTaskElem.ucbVideoConferenceStarter)
                                                                    {
                                                                        goto errLabel;
                                                                    }
                                                                    else
                                                                    {
#if DEBUG
                                                                        ref IM_CONTENTU tmp_pContent1 = ref pContent;
                                                                        ref IM_CONTENTU tmp_pContentElem1 = ref pContentElem;

#endif
                                                                        if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
                                                                        QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                                                                        DLG_TALK_videoConference videoConference = pTc.getVideoConference();
                                                                        if (videoConference == null) goto errLabel;

                                                                        for (int i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
                                                                        {
                                                                            DLG_TALK_videoConferenceActiveMemFrom activeMem = videoConference.activeMems_from[i];
                                                                            if (activeMem.avStream.idInfo.ui64Id == pMisCnt.idInfo.ui64Id
                                                                                && activeMem.avStream.obj.tranInfo.video.uiTranNo_openAvDev == pContent.taskProcReq.vStream.uiTranNo_openAvDev)
                                                                            {
                                                                                tmp_bFound = true;
                                                                                break;

                                                                            }
                                                                        }

                                                                        //
                                                                        if (tmp_bFound)
                                                                        {

#if __DEBUG__
																traceLogA("findOrgMsgAndProcess_mis: get a applyForKeyFrame msg");
#endif

                                                                            //
                                                                            tmpiRet = procTaskProcReq_applyForKeyFrame(pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgElem, ref pContent.taskProcReq, ref pContentElem, ref pTaskElem);
                                                                            if (tmpiRet != 0)
                                                                            {
                                                                                iErr = tmpiRet; goto errLabel;
                                                                            }

                                                                            iErr = 1; goto errLabel;
                                                                        }
                                                                        //

                                                                    }
                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //case Consts.CONST_imCommType_transferAvReplyInfo:
                                                            //if (pContentElem->uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                                                            //  media»Ø¸´¶Ë
                                                            TASK_transferAv task = (TASK_transferAv)taskElem;
                                                            //TRANSFER_AV_replyInfo* pTransferAvReplyInfoElem = &pContentElem->transferAvReplyInfo;
                                                            ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvReplyInfo_pTask(pTransferAvReplyInfoElem, _T(""));
                                                                                                         //if (!pTaskElem) goto errLabel;
                                                            int index = 0;

                                                            //  ÒòÎªaudio¶ÔÊµÊ±ÐÔÒªÇó×î¸ß£¬ËùÒÔ·ÅÔÚµÚÒ»Î»±È½Ï
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAudioData)
                                                            {
                                                                //
                                                                tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procInputAudioData(ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgInput, ref pContent.transferAudioData, ref pContentElem, ref pTaskElem);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }

                                                            }
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferVideoData)
                                                            {                           //  ¶Ô·½µÄÊý¾Ý
                                                                                        //
                                                                tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procInputVideoData(ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgInput, ref pContent.transferVideoData, ref pContentElem, ref pTaskElem);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }

                                                            }

                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAvInfo)
                                                            {       //  ÖØ·¢Ê±ÕâÀïÒªÐ£ÑéµÄ
                                                                if (pMsgElem.data.route.idInfo_to.ui64Id == pMsgInput.data.route.idInfo_from.ui64Id
                                                                    && pContentElem.transferAvReplyInfo.tStartTime_org == pMsgInput.tStartTime
                                                                    && pContentElem.transferAvReplyInfo.uiTranNo_org == pMsgInput.uiTranNo)
                                                                {
                                                                    //
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - req is matched");
#endif
                                                                    //
                                                                    iErr = 1; goto errLabel;
                                                                }
                                                            }
                                                            if (pContent.uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                                                            {                           //  ´¦Àí
                                                                if (pContentElem.transferAvReplyInfo.tStartTime_org == pContent.transferAvReplyInfo.tStartTime_org
                                                                    && pContentElem.transferAvReplyInfo.uiTranNo_org == pContent.transferAvReplyInfo.uiTranNo_org)
                                                                {
                                                                    //
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is . msg - procReq matched.");
#endif
                                                                    //
                                                                    iErr = 1; goto errLabel;
                                                                }
                                                            }
                                                            if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                                                            {   //
                                                                //
                                                                if (pMsgElem.tStartTime == pContent.taskProcReq.tStartTime_org
                                                                    && pMsgElem.uiTranNo == pContent.taskProcReq.uiTranNo_org)
                                                                //if (pContentElem->transferAvReplyInfo.tStartTime_org == pContent->taskProcReq.tStartTime_org
                                                                //&& pContentElem->transferAvReplyInfo.uiTranNo_org == pContent->taskProcReq.uiTranNo_org)
                                                                {
                                                                    //
#if DEBUG
                                                                    traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq is matched");
#endif
                                                                    //
                                                                    iErr = 1; goto errLabel;
                                                                }
                                                                //
                                                                if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForKeyFrame)
                                                                {

                                                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("recv applyforKeyFrame from %I64u, tn_openAvDev %d. matched iTaskId %d"), pMsgInput->data.route.idInfo_from.ui64Id, pContent->taskProcReq.vStream.uiTranNo_openAvDev, pMsgElem->iTaskId);

                                                                    bool tmp_bFound = false;
                                                                    //
                                                                    if (pTaskElem.ucbVideoConferenceStarter)
                                                                    {
                                                                        goto errLabel;
                                                                    }
                                                                    else
                                                                    {
#if DEBUG
                                                                        ref IM_CONTENTU tmp_pContent1 = ref pContent;
                                                                        ref IM_CONTENTU tmp_pContentElem1 = ref pContentElem;

#endif
                                                                        //
                                                                        QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, 0);
                                                                        if (pSharedObj != null)
                                                                        {
                                                                            int ii;
                                                                            for (ii = 0; ii < pSharedObj.usrs.Length; ii++)
                                                                            {
                                                                                if (pContent.taskProcReq.vStream.uiTranNo_openAvDev == pSharedObj.usrs[ii].uiTranNo_openAvDev_v)
                                                                                {
                                                                                    tmp_bFound = true;
                                                                                    break;
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                    //
                                                                    if (tmp_bFound)
                                                                    {

#if __DEBUG__
																traceLogA("findOrgMsgAndProcess_mis: get a applyForKeyFrame msg");
#endif

                                                                        //
                                                                        tmpiRet = procTaskProcReq_applyForKeyFrame(pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId,ref pMsgElem, ref pContent.taskProcReq, ref pContentElem, ref pTaskElem);
                                                                        if (tmpiRet != 0)
                                                                        {
                                                                            iErr = tmpiRet; goto errLabel;
                                                                        }

                                                                        iErr = 1; goto errLabel;
                                                                    }
                                                                    //
                                                                }



                                                                ///////////////
                                                            }

                                                        }  //  ÒÔÉÏÎªmedia»Ø¸´¶Ë¡£

                                                    }
                                                    break;
                                                case Consts.CONST_imCommType_transferFileReq:
                                                    {
                                                        if (pContent.uiType == Consts.CONST_imCommType_transferFileReplyReq)
                                                        {
                                                            ref TRANSFER_FILE_REPLYREQ pReply = ref pContent.transferFileReplyReq;
                                                            if (pReply.tStartTime_org == pMsgElem.tStartTime
                                                                && pReply.uiTranNo_org == pMsgElem.uiTranNo)
                                                            {
                                                                iErr = 1; goto errLabel;
                                                            }

                                                        }

                                                    }
                                                    break;
                                                default:
                                                    break;

                                            }

                                            //  ·¢ËÍÎÄ¼þ
                                            switch (pContent.uiType)
                                            {

                                                case Consts.CONST_imCommType_transferFileDataReq:
                                                    if (pContentElem.uiType == Consts.CONST_imCommType_transferFileReq
                                                        && pContent.transferFileDataReq.tStartTime_org == pMsgElem.tStartTime
                                                             && pContent.transferFileDataReq.uiTranNo_org == pMsgElem.uiTranNo)
                                                    {
                                                        //
                                                        TASK_transferFile task = (TASK_transferFile)taskElem;
                                                        //
                                                        ref TRANSFER_FILE_REQ pFileReq = ref pContentElem.transferFileReq;
                                                        ref TRANSFER_FILEDATA_REQ pFileDataReq = ref pContent.transferFileDataReq;
                                                        UInt64 ui64offset = 0;
                                                        int nRead = 0;

                                                        if  (  null==p1.pFileDataResp_o)
                                                        {
                                                            goto errLabel;
                                                        }

                                                        ref TRANSFER_FILEDATA_RESP pFileDataResp_o = ref p1.pFileDataResp_o.transferFileDataResp;

                                                        if (null == p1.pFileDataResp_o || 0 == pFileReq.ui64FileLen || 0 == pFileDataReq.uiSizePerSnd || Consts.bufSize_transferFileDataResp < pFileDataReq.uiSizePerSnd)
                                                        {
#if __DEBUG__
															traceLogA("tmpHandler_findOrgMsgAndProc_is failed: sizeof(  buf  )  <  uiSizePerSnd");
#endif
                                                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("findOrgMsg error: fileDataReq, sizeof(  buf  ) %d < sizePerSnd [%d]"), sizeof(pFileDataResp_o->buf),  pFileDataReq->uiSizePerSnd  );
                                                            goto errLabel;
                                                        }

                                                        ui64offset = ((UInt64)pFileDataReq.uiDataBlockIndex) * pFileDataReq.uiSizePerSnd;
                                                        if (ui64offset >= pFileReq.ui64FileLen)
                                                        {
#if __DEBUG__
															traceLog(_T("findOrgMsg error: fileDataReq, blockIndex [%d] * sizePerSnd [%d] > fileLen [%I64u]"), pFileDataReq->uiDataBlockIndex, pFileDataReq->uiSizePerSnd, pFileReq->ui64FileLen);
#endif
                                                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("findOrgMsg error: fileDataReq, blockIndex [%d] * sizePerSnd [%d] > fileLen [%I64u]"), pFileDataReq->uiDataBlockIndex, pFileDataReq->uiSizePerSnd, pFileReq->ui64FileLen);
                                                            goto errLabel;
                                                        }


                                                        //  ÕâÀï¼ÆËãÒ»ÏÂÒÑ¾­·¢ËÍµÄ°Ù·Ö±È
                                                        uint dwCurTickCnt;
                                                        //
                                                        dwCurTickCnt = myGetTickCount(null);
                                                        if (dwCurTickCnt > pFileReq.dwLastReportTickCnt + 3000)
                                                        {   //  ÎªÁË²»Ó°Ïì½çÃæ²Ù×÷£¬Òª¹ý3Ãë²ÅÄÜ¼ÆËãÒ»´Î
                                                            pFileReq.dwLastReportTickCnt = dwCurTickCnt;
                                                            //
                                                            float f = 0;
                                                            byte ucPercent;
                                                            //
                                                            f = (float)pFileDataReq.uiDataBlockIndex * pFileDataReq.uiSizePerSnd;
                                                            f = f / pFileReq.ui64FileLen;
                                                            ucPercent = (byte)(f * 100);

                                                            //  if  (  ucPercent  !=  pFileReq->ucPercent  )  
                                                            {   //  ±¨¸æ×´Ì¬

                                                                showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, pMsgElem.idInfo_taskReceiver.ui64Id, false, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, Consts.CONST_imTaskStatus_sending, ucPercent, pFileDataReq.uiDataBlockIndex * pFileDataReq.uiSizePerSnd, null, null);

                                                            }
                                                        }

#if __DEBUG__
														traceLogA("");
#endif

                                                        if (null == task.myTask.fp)
                                                        {
                                                            tmp_tfopen(ref pContentElem, task);
                                                            if (null == task.myTask.fp)
                                                            {
#if __DEBUG__
																traceLogA("tmpHandler_findOrgMsgAndProc_is failed: _tfopen {%S} err", pContentElem->transferFileReq.fullFilePath);
#endif
                                                                goto errLabel;
                                                            }
                                                        }
                                                        //  ÕâÀïÓ¦¸ÃÓÃ64Î»µÄ²Ù×÷ÒÔ¿ÉÒÔ²Ù×÷´óÎÄ¼þ£¬µ«ÊÇÔÝÊ±ÏÈÓÃ32Î»µÄ
                                                        //  if  (  fseek(  pFileReq->fp,  (  long  )ui64offset,  SEEK_SET  )  )  
                                                        if (0 != GuiShare.pf_fseeki64(task.myTask.fp, (Int64)ui64offset, 0))
                                                        {
#if __DEBUG__
															traceLogA("tmpHandler_findOrgMsgAndProc_is failed:  _fseeki64 err");
#endif
                                                            goto errLabel;
                                                        }
                                                        nRead = (int)Math.Min(pFileReq.ui64FileLen - ui64offset, pFileDataReq.uiSizePerSnd);

                                                        //
                                                        //if (1 != GuiShare.pf_fread(pFileDataResp_o.buf, nRead, 1, task.myTask.fp)) goto errLabel;
                                                        if (tmp_fread(ref pFileDataResp_o,  nRead, task) != 0) goto errLabel;

                                                        //
                                                        pFileDataResp_o.uiLen = (uint)nRead;
                                                        //
                                                        pFileDataResp_o.uiDataBlockIndex = pFileDataReq.uiDataBlockIndex;

                                                        iErr = 1; goto errLabel;
                                                    }
                                                    break;
                                                case Consts.CONST_imCommType_transferFileEndReq:
                                                    if (pContentElem.uiType == Consts.CONST_imCommType_transferFileReq                    //  ·¢ËÍ·½´¦Àí
                                                        && pContent.transferFileEndReq.tStartTime_org == pMsgElem.tStartTime
                                                             && pContent.transferFileEndReq.uiTranNo_org == pMsgElem.uiTranNo)
                                                    {
                                                        TASK_transferFile task = (TASK_transferFile)taskElem;
                                                        //
                                                        int iStatus = Consts.CONST_imTaskStatus_sendFinished;
                                                        //
#if __DEBUG__
														traceLogA("transferFileEnd ");
#endif
                                                        //
                                                        if (p1.pContent_o != null)
                                                        {
                                                            ref IM_CONTENTU pContent_o = ref p1.pContent_o.imContentU;

                                                            if (pContent_o.uiType == Consts.CONST_imCommType_transferFileEndResp)
                                                            {
                                                                ref TRANSFER_FILEEND_RESP pFileEndResp_o = ref pContent_o.transferFileEndResp;
                                                                //														   
                                                                if (!pContentElem.transferFileReq.bGrpTask)
                                                                {
                                                                    //  2015/09/08
                                                                    if (pFileEndResp_o.tmpInternal.bChkToRemove_i)
                                                                    {
                                                                        task.myTask.bNeedRemoved = true;
                                                                        //
                                                                        pFileEndResp_o.tmpInternal.bNeedRemoved_o = true;
                                                                    }
                                                                    //
                                                                    object pDb = null;
                                                                    using (CQnmDb db = new CQnmDb())
                                                                    {
                                                                        ;

                                                                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                                                        pDb = db.m_pDbMem.pDb;
                                                                        if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsgElem.iTaskId)) goto errLabel;
                                                                    }
                                                                    //
                                                                    //fixed (char* fileName = pContentElem.transferFileReq.fullFilePath)
                                                                    string fileName = tmp_get_fullFilePath(ref pContentElem);
                                                                    {
                                                                        showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, pMsgElem.idInfo_taskReceiver.ui64Id, false, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, iStatus, 100, pContentElem.transferFileReq.ui64FileLen, fileName, fileName);
                                                                    }

                                                                }
                                                                else
                                                                {   //  ¶Ô·Ö×é£¬ÓÐÊ²Ã´ÈËÊÕµ½ÁËÎÄ¼þ£¬Ò²ÏÔÊ¾Ò»ÏÂ
                                                                    //fixed (char* fileName = pContentElem.transferFileReq.fullFilePath)
                                                                    string fileName = tmp_get_fullFilePath(ref pContentElem);
                                                                    {

                                                                        showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, pMsgInput.addr_logicalPeer.idInfo.ui64Id, true, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, iStatus, 100, pContentElem.transferFileReq.ui64FileLen, fileName, fileName);
                                                                    }

                                                                }

                                                            }
                                                        }
                                                        //
                                                        iErr = 1; goto errLabel;
                                                    }
                                                    break;

                                                case Consts.CONST_imCommType_taskProcReq:
                                                    if (pContentElem.uiType == Consts.CONST_imCommType_transferFileReplyReq       //  ½ÓÊÕ·½´¦Àí
                                                        && pContent.taskProcReq.tStartTime_org == pContentElem.transferFileReplyReq.tStartTime_org
                                                             && pContent.taskProcReq.uiTranNo_org == pContentElem.transferFileReplyReq.uiTranNo_org)
                                                    {
                                                        int iStatus = Consts.CONST_imTaskStatus_canceledBySender;
                                                        //	
#if __DEBUG__
														traceLogA("transferFileProcReq ");
#endif
                                                        //
                                                        if (p1.pContent_o != null)
                                                        {
                                                            ref IM_CONTENTU pContent_o = ref p1.pContent_o.imContentU;

                                                            if (pContent_o.uiType == Consts.CONST_imCommType_taskProcResp)
                                                            {
                                                                ref TASK_PROC_RESP pFileProcResp = ref pContent_o.taskProcResp;

                                                                pFileProcResp.bNeedRemoved = true;

                                                                //									
                                                                object pDb = null;
                                                                using (CQnmDb db = new CQnmDb())
                                                                {
                                                                    ;

                                                                    if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                                                    pDb = db.m_pDbMem.pDb;
                                                                    if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsgElem.iTaskId)) goto errLabel;
                                                                }
                                                                //
#if !DEBUG
																//showTaskStatus(pMisCnt, pMsgElem->addr_logicalPeer.idInfo, pMsgElem->idInfo_taskSender, null,false, Consts.CONST_imCommType_transferFileReq, pMsgElem->iTaskId, iStatus, 0, pContentElem->transferFileReplyReq.ui64FileLen, new string(pContentElem->transferFileReplyReq.fileName_transferred), new string(pContentElem->transferFileReplyReq.fullFilePath_save));
#endif
                                                            }
                                                        }
                                                        //
                                                        iErr = 1; goto errLabel;
                                                    }
                                                    if (pContentElem.uiType == Consts.CONST_imCommType_transferFileReq)
                                                    {       //  ·¢ËÍ·½´¦Àí
                                                        if (pContent.taskProcReq.tStartTime_org == pMsgElem.tStartTime
                                                            && pContent.taskProcReq.uiTranNo_org == pMsgElem.uiTranNo)
                                                        {
                                                            int iStatus = Consts.CONST_imTaskStatus_canceledByReceiver;
                                                            qyFuncs.traceLog("##4148" + iStatus);

                                                            //
#if __DEBUG__
															traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq found");
#endif
                                                            //
                                                            if (p1.pContent_o != null)
                                                            {
                                                                ref IM_CONTENTU pContent_o = ref p1.pContent_o.imContentU;

                                                                if (pContent_o.uiType == Consts.CONST_imCommType_taskProcResp)
                                                                {
                                                                    ref TASK_PROC_RESP pFileProcResp = ref pContent_o.taskProcResp;

                                                                    if (!pContentElem.transferFileReq.bGrpTask)
                                                                    {
                                                                        pFileProcResp.bNeedRemoved = true;

                                                                        //
                                                                        object pDb = null;
                                                                        using (CQnmDb db = new CQnmDb())
                                                                        {
                                                                            ;
                                                                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                                                            pDb = db.m_pDbMem.pDb;
                                                                            if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsgElem.iTaskId)) goto errLabel;
                                                                        }
                                                                        //
                                                                        //fixed (char* fileName = pContentElem.transferFileReq.fileName)
                                                                        string fileName = tmp_get_fileName(ref pContentElem);
                                                                        {
                                                                            //fixed (char* fullFilePath = pContentElem.transferFileReq.fullFilePath)
                                                                            string fullFilePath = tmp_get_fullFilePath(ref pContentElem);
                                                                            {
                                                                                showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, 0, false, Consts.CONST_imCommType_transferFileReq, pMsgElem.iTaskId, iStatus, 0, 0, fileName, fullFilePath);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //
                                                            iErr = 1; goto errLabel;
                                                        }
                                                    }
                                                    //
                                                    //  transferAvInfo, 
                                                    //
                                                    if (pContentElem.uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                                                    {       //  ½ÓÊÕ·½´¦Àí
                                                        TASK_transferAv task = (TASK_transferAv)taskElem;
                                                        ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvReplyInfo_pTask(&pContentElem->transferAvReplyInfo, _T(""));
                                                                                                     //if (!pTaskElem) goto errLabel;
                                                                                                     //
                                                        if (pContent.taskProcReq.tStartTime_org == pContentElem.transferAvReplyInfo.tStartTime_org
                                                             && pContent.taskProcReq.uiTranNo_org == pContentElem.transferAvReplyInfo.uiTranNo_org)
                                                        {
                                                            int iStatus = Consts.CONST_imTaskStatus_canceledBySender;
                                                            //
#if __DEBUG__
															traceLogA("taskProcReq ");
#endif
                                                            //
                                                            if (p1.pContent_o != null)
                                                            {
                                                                ref IM_CONTENTU pContent_o = ref p1.pContent_o.imContentU;

                                                                if (pContent_o.uiType == Consts.CONST_imCommType_taskProcResp)
                                                                {
                                                                    ref TASK_PROC_RESP pFileProcResp = ref pContent_o.taskProcResp;

                                                                    pFileProcResp.bNeedRemoved = true;

                                                                    //															   
                                                                    object pDb = null;
                                                                    using (CQnmDb db = new CQnmDb())
                                                                    {
                                                                        ;

                                                                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                                                        pDb = db.m_pDbMem.pDb;
                                                                        if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsgElem.iTaskId)) goto errLabel;
                                                                    }
                                                                    //
                                                                    showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, 0, false, Consts.CONST_imCommType_transferAvInfo, pMsgElem.iTaskId, iStatus, 0, 0, null, null);
                                                                }
                                                            }
                                                            //
                                                            iErr = 1; goto errLabel;
                                                        }

                                                        //  2014/08/26
                                                        if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForKeyFrame)
                                                        {

                                                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("recv applyforKeyFrame from %I64u."), pMsgInput->data.route.idInfo_from.ui64Id);

                                                            //
                                                            if (pContent.taskProcReq.vStream.uiTranNo_openAvDev == pContentElem.transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev)
                                                            {

#if DEBUG
                                                                traceLogA("findOrgMsgAndProcess_mis: get a applyForKeyFrame msg");
#endif

                                                                //
                                                                tmpiRet = procTaskProcReq_applyForKeyFrame(pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgElem, ref pContent.taskProcReq, ref pContentElem, ref pTaskElem);
                                                                if (0 != tmpiRet)
                                                                {
                                                                    iErr = tmpiRet; goto errLabel;
                                                                }

                                                                iErr = 1; goto errLabel;
                                                            }
                                                        }

                                                    }
                                                    if (pContentElem.uiType == Consts.CONST_imCommType_transferAvInfo)
                                                    {       //  ·¢ËÍ·½´¦Àí
                                                            //
                                                        if (!pTaskInfo.var.m_bReplyTask)
                                                        {
                                                            TASK_transferAv task = (TASK_transferAv)taskElem;
                                                            ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvInfo_pTask(&pContentElem->transferAvInfo, _T(""));
                                                                                                         //if (!pTaskElem) goto errLabel;
                                                                                                         //
                                                            if (pContent.taskProcReq.tStartTime_org == pMsgElem.tStartTime
                                                                && pContent.taskProcReq.uiTranNo_org == pMsgElem.uiTranNo)
                                                            {
                                                                //
#if DEBUG
                                                                traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - taskProcReq. task found");
#endif

                                                                //
                                                                if (p1.pContent_o != null)
                                                                {
                                                                    ref IM_CONTENTU pContent_o = ref p1.pContent_o.imContentU;

                                                                    if (pContent_o.uiType == Consts.CONST_imCommType_taskProcResp)
                                                                    {
                                                                        ref TASK_PROC_RESP pFileProcResp = ref pContent_o.taskProcResp;

                                                                        pFileProcResp.idInfo_logicalPeer.ui64Id = pMsgElem.addr_logicalPeer.idInfo.ui64Id;

                                                                        if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForChkTaskAlive)
                                                                        {
                                                                            //
                                                                            qyFuncs.traceLog("findOrgMsgAndProc: taskProcReq. usOp recv_applyForChkTaskAlive, task found");
                                                                            //
                                                                            iErr = 1; goto errLabel;
                                                                        }

                                                                        int iStatus = Consts.CONST_imTaskStatus_canceledByReceiver;
                                                                        qyFuncs.traceLog("##4288");
                                                                        //  2014/12/03
                                                                        bool bCanCancel = true;
                                                                        if (pContentElem.transferAvInfo.bGrpTask)
                                                                        {
                                                                            bCanCancel = false;
                                                                        }
                                                                        if (pTaskElem.doProxy.bExists_avProxy)
                                                                        {
                                                                            bCanCancel = false;
                                                                        }

                                                                        //  2017/09/11
                                                                        if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_cancel)
                                                                        {
                                                                            tmp_procTask_recv_cancel(pProcInfo, ref pTaskElem, ref pMsgInput, ref pContent.taskProcReq);
                                                                        }

                                                                        //
                                                                        //if  (  !pContentElem->transferAvInfo.bGrpTask  )  
                                                                        if (bCanCancel)
                                                                        {
                                                                            pFileProcResp.bNeedRemoved = true;

                                                                            //  2014/12/03
#if false
																		TCHAR tBuf[128] = _T("");
																		_sntprintf(tBuf, mycountof(tBuf), _T("findOrgMsg: taskProcReq from receiver %I64u, to cancel transferAvInfo"), pMsgInput->data.route.idInfo_from.ui64Id);
#if __DEBUG__
																		traceLog(_T("%s"), tBuf);
#endif
																		showInfo_open(0, 0, tBuf);
#endif
                                                                            //
                                                                            object pDb = null;
                                                                            using (CQnmDb db = new CQnmDb())
                                                                            {
                                                                                ;

                                                                                if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                                                                pDb = db.m_pDbMem.pDb;
                                                                                if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsgElem.iTaskId)) goto errLabel;
                                                                            }
                                                                            //
                                                                            showTaskStatus(pMisCnt, pMsgElem.addr_logicalPeer.idInfo.ui64Id, pMsgElem.idInfo_taskSender.ui64Id, 0, false, pContentElem.uiType, pMsgElem.iTaskId, iStatus, 0, 0, null, null);
                                                                        }
                                                                    }
                                                                }
                                                                //
                                                                iErr = 1; goto errLabel;
                                                            }
                                                            //  2014/08/26
                                                            if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForKeyFrame)
                                                            {

                                                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("recv applyforKeyFrame from %I64u, tn_openAvDev %d. matched iTaskId %d"), pMsgInput->data.route.idInfo_from.ui64Id, pContent->taskProcReq.vStream.uiTranNo_openAvDev, pMsgElem->iTaskId);

                                                                bool tmp_bFound = false;
                                                                //
                                                                if (pTaskElem.ucbVideoConferenceStarter)
                                                                {
                                                                    //  2014/08/28. 暂时采用直接模式
                                                                    if (pContent.taskProcReq.vStream.uiTranNo_openAvDev == pTaskElem.mem0_from.video.tv_recvd.uiTranNo_openAvDev)
                                                                    {
                                                                        tmp_bFound = true;
                                                                    }
                                                                    //  2014/12/06
                                                                    if (pContent.taskProcReq.vStream.uiTranNo_openAvDev == pContentElem.transferAvInfo.confMosaicTranInfo_video.video.uiTranNo_openAvDev
                                                                        || pContent.taskProcReq.vStream.uiTranNo_openAvDev == pContentElem.transferAvInfo.confMosaicTranInfo_resource.video.uiTranNo_openAvDev)
                                                                    {
                                                                        tmp_bFound = true;

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (pContent.taskProcReq.vStream.uiTranNo_openAvDev == pContentElem.transferAvInfo.ass.tranInfo.video.uiTranNo_openAvDev)
                                                                    {
                                                                        tmp_bFound = true;
                                                                    }
                                                                }
                                                                //
                                                                if (tmp_bFound)
                                                                {

#if __DEBUG__
																traceLogA("findOrgMsgAndProcess_mis: get a applyForKeyFrame msg");
#endif

                                                                    //
                                                                    tmpiRet = procTaskProcReq_applyForKeyFrame(pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgElem, ref pContent.taskProcReq, ref pContentElem, ref pTaskElem);
                                                                    if (tmpiRet != 0)
                                                                    {
                                                                        iErr = tmpiRet; goto errLabel;
                                                                    }

                                                                    iErr = 1; goto errLabel;
                                                                }
                                                                //
                                                            }
                                                            //  2014/10/09
                                                            if (pContent.taskProcReq.usOp == Consts.CONST_imOp_stopSending
                                                                || pContent.taskProcReq.usOp == Consts.CONST_imOp_requestToSend)
                                                            {
                                                                if (0 != pContent.taskProcReq.viewDynBmp.uiObjType)
                                                                {
                                                                    if (pContentElem.transferAvInfo.ass.resObj.uiObjType == pContent.taskProcReq.viewDynBmp.uiObjType
                                                                        && pContentElem.transferAvInfo.ass.resObj.usIndex_obj == pContent.taskProcReq.viewDynBmp.usIndex_obj)
                                                                    {
#if __DEBUG__
																	traceLog(_T("findOrgMsgAndProc: get taskProcReq msg"));
#endif

                                                                        pMsgInput.iTaskId = pMsgElem.iTaskId;
                                                                        pMsgInput.addr_logicalPeer.idInfo.ui64Id = pMsgElem.addr_logicalPeer.idInfo.ui64Id;

                                                                        //
                                                                        //pContent->taskProcReq.hWnd_task = pTaskElem.hWnd_task;

                                                                        //
                                                                        tmp_qPost_msgInput_taskProcReq(ref pMsgInput);
                                                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                                                        iErr = 1; goto errLabel;
                                                                    }
                                                                }
                                                            }
                                                            //  2009/10/18
                                                            if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForOrgReq)
                                                            {

                                                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("recv applyforOrgReq from %I64u. _tn %d"), pMsgInput->data.route.idInfo_from.ui64Id, pMsgInput->uiTranNo);
                                                                //task.myTask.ucbVideoConference

                                                                bool tmp_bMatched = false;
                                                                if (pContent.taskProcReq.uiContentType_org == Consts.CONST_imCommType_transferAudioData)
                                                                {
                                                                    if (task.myTask.bVideoConference())
                                                                    {
                                                                        if (pContent.taskProcReq.uiTranNo_org == pContentElem.transferAvInfo.ass.tranInfo.audio.uiTranNo_openAvDev)
                                                                        {
                                                                            tmp_bMatched = true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (bInActiveMems(pProcInfo, pMisCnt.idInfo, pContent.taskProcReq.uiTranNo_org, 0, task))
                                                                        {
                                                                            tmp_bMatched = true;
                                                                        }
                                                                    }
                                                                    if (tmp_bMatched)
                                                                    {
#if DEBUG
                                                                        traceLogA("findOrgMsgAndProcess_mis: get a applyForOrgReq msg");
#endif
                                                                        pMsgInput.iTaskId = pMsgElem.iTaskId;
                                                                        //  2014/11/18
                                                                        if (pContentElem.transferAvInfo.idInfo_logicalPeer.ui64Id != 0)
                                                                        {
                                                                            pMsgInput.addr_logicalPeer.idInfo.ui64Id = pContentElem.transferAvInfo.idInfo_logicalPeer.ui64Id;
                                                                        }
                                                                        else
                                                                        {
                                                                            pMsgInput.addr_logicalPeer.idInfo.ui64Id = pMsgElem.addr_logicalPeer.idInfo.ui64Id;
                                                                        }

                                                                        //
                                                                        //
                                                                        traceLog("post req applyForOrgReq for audioData to gui");

                                                                        //
                                                                        tmp_qPost_msgInput_taskProcReq(ref pMsgInput);
                                                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                                                        iErr = 1; goto errLabel;
                                                                    }
                                                                }
                                                                //  2010/12/27
                                                                if (pContent.taskProcReq.uiContentType_org == Consts.CONST_imCommType_transferVideoData)
                                                                {
                                                                    if (task.myTask.bVideoConference())
                                                                    {
                                                                        if (pContent.taskProcReq.uiTranNo_org == pContentElem.transferAvInfo.ass.tranInfo.video.uiTranNo_openAvDev)
                                                                        {
                                                                            tmp_bMatched = true;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (bInActiveMems(pProcInfo, pMisCnt.idInfo, 0, pContent.taskProcReq.uiTranNo_org, task))
                                                                        {
                                                                            tmp_bMatched = true;
                                                                        }
                                                                    }
                                                                    if (tmp_bMatched)
                                                                    {
#if DEBUG
                                                                        traceLogA("findOrgMsgAndProcess_mis: get a applyForOrgReq msg. taskFound");
#endif
                                                                        //
                                                                        if (pMsgElem.uiTaskType == Consts.CONST_imTaskType_shareDynBmp)
                                                                        {
                                                                            if (pTaskElem.iIndex_sharedObj != 0)
                                                                            {
                                                                                QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pTaskElem.iIndex_sharedObj);
                                                                                if (pSharedObj != null)
                                                                                {
                                                                                    removeFromMsgrs_sendLocalAv(pProcInfo, pMisCnt, ref pMsgInput.data.route.idInfo_from, ref pTaskInfo.var.curRoute_sendLocalAv);
                                                                                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("findOrgMsgAndProc: shareDynBmp, index_sharedObj %d, removeFromMsgrs_sendLocalAv, %I64u"), pTaskElem->iIndex_sharedObj, pMsgInput->data.route.idInfo_from);
                                                                                    iErr = 1; goto errLabel;
                                                                                }
                                                                            }
                                                                        }

                                                                        //
                                                                        pMsgInput.iTaskId = pMsgElem.iTaskId;
                                                                        //  2014/11/18
                                                                        if (pContentElem.transferAvInfo.idInfo_logicalPeer.ui64Id != 0)
                                                                        {
                                                                            pMsgInput.addr_logicalPeer.idInfo.ui64Id = pContentElem.transferAvInfo.idInfo_logicalPeer.ui64Id;
                                                                        }
                                                                        else
                                                                        {
                                                                            pMsgInput.addr_logicalPeer.idInfo.ui64Id = pMsgElem.addr_logicalPeer.idInfo.ui64Id;
                                                                        }

                                                                        //
                                                                        traceLog("post req applyForOrgReq for videoData to gui");

                                                                        //
                                                                        tmp_qPost_msgInput_taskProcReq(ref pMsgInput);
                                                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                                                        iErr = 1; goto errLabel;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForOrgReq)
                                                            {
                                                                QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, 0);
                                                                if (null != pSharedObj)
                                                                {
                                                                    int ii = 0;
#if DEBUG
                                                                    ref IM_CONTENTU tmp_pContent1 = ref pContent;

#endif
                                                                    bool tmp_bMatched = false;

                                                                    if (pContent.taskProcReq.uiContentType_org == Consts.CONST_imCommType_transferAudioData)
                                                                    {

                                                                        ii = 0;
                                                                    }
                                                                    else if (pContent.taskProcReq.uiContentType_org == Consts.CONST_imCommType_transferVideoData)
                                                                    {
                                                                        ii = 1;
                                                                        for (ii = 0; ii < pSharedObj.usrs.Length; ii++)
                                                                        {
                                                                            if (pContent.taskProcReq.uiTranNo_org == pSharedObj.usrs[ii].uiTranNo_openAvDev_v)
                                                                            {
                                                                                tmp_bMatched = true;
                                                                                break;
                                                                            }
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        traceLog("taskProcReq.uiContentType_org err");
                                                                    }

                                                                    //
                                                                    if (tmp_bMatched)
                                                                    {
                                                                        //
                                                                        pMsgInput.iTaskId = pMsgElem.iTaskId;
                                                                        //  2014/11/18
                                                                        if (pContentElem.transferAvInfo.idInfo_logicalPeer.ui64Id != 0)
                                                                        {
                                                                            pMsgInput.addr_logicalPeer.idInfo.ui64Id = pContentElem.transferAvInfo.idInfo_logicalPeer.ui64Id;
                                                                        }
                                                                        else
                                                                        {
                                                                            pMsgInput.addr_logicalPeer.idInfo.ui64Id = pMsgElem.addr_logicalPeer.idInfo.ui64Id;
                                                                        }

                                                                        //
                                                                        showInfo_open(0, null, "post req applyForOrgReq for videoData to gui");

                                                                        //
                                                                        tmp_qPost_msgInput_taskProcReq(ref pMsgInput);
                                                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                                                        iErr = 1; goto errLabel;


                                                                    }

                                                                }

                                                            }

                                                        }
                                                    }

                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        //
                                    }

            iErr = 0;
        errLabel:
            return iErr;

        }


        public static unsafe int findOrgMsg_task_searchTask_av(CTX_caller ctxCaller, ref MIS_MSGU pMsg_toBeSearchedParam, PARAM_findOrgMsg p1Param, object taskElemParam)
        {
            int iErr = -1;

            bool bDbg = false;
            string str;

#if DEBUG
            bDbg = true;
#endif

            //
            ref MIS_MSGU pMsg_toBeSearched = ref pMsg_toBeSearchedParam;
            //IntPtr p1 = (IntPtr)p1Param;
            ref PARAM_findOrgMsg p1 = ref p1Param;
            //  p1
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            int tmpiRet = 0;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;
            MIS_CNT pMisCntElem = pProcInfo.pMisCnt;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            //
            TASK_common taskElem = (TASK_common)taskElemParam;
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, taskElem.m_index_taskInfo, "findOrgMsg");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            //
            if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
                //traceLog("for test, tmpHandler_findOrgMsgAndProc_is return 0");
                return 0;
            }
#if DEBUG
            if (pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is)
            {
                bDbg = true;
            }
#endif

            //
            if (bDbg)
            {
                qyFuncs.traceLog("findOrgMsg_task_searchTask_av enters");
                //
               // System.Environment.Exit(0);

            }


            //
            //fixed (MIS_MSGU* pMsgElemParam = &pTaskData.msgU)
            ref MIS_MSGU pMsgElemParam = ref pTaskData.msgU;

            ref MIS_MSG_TASK pMsgElem = ref pMsgElemParam.task;
            ref IM_CONTENTU pContentElem = ref pMsgElem.data.buf;


            


                ref MIS_MSG_TASK pMsgTask = ref pMsg_toBeSearched.task;
                ref IM_CONTENTU pContent = ref pMsgTask.data.buf;


                if (!pTaskInfo.var.m_bReplyTask)
                        {
                            TASK_transferAv task = (TASK_transferAv)taskElem;
                            ref TRANSFER_AV_INFO pTransferAvInfoElem = ref pContentElem.transferAvInfo;
                            ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvInfo_pTask(pTransferAvInfoElem, _T(""));
                                                                         //if (!pTaskElem) goto errLabel;
                            int index = 0;

                            if (pContent.uiType == Consts.CONST_imCommType_transferAvInfo)
                            {                               //  
                                ref TRANSFER_AV_INFO pTransferAvInfo = ref pContent.transferAvInfo;
                                //
                                if (pMsgElem.tStartTime == pMsgTask.tStartTime
                                    && pMsgElem.uiTranNo == pMsgTask.uiTranNo)
                                {
                                    bool canGet = false;
                                    //
                                    if (pTaskElem.ucbVideoConferenceStarter) canGet = true;
                                    else if (bApplyForAvProxy(pMisCntElem, ref pTransferAvInfoElem)) canGet = true;
                                    else
                                    {
#if __DEBUG__
															//  traceLog(  _T(  "findOrgMsg: 
															//  让talk来校验
#endif
                                        canGet = true;
                                    }
                                    //
#if __DEBUG__
														if (!canGet)
														{
															traceLog(_T("findOrgMsg: can't get req again, canGot is false"));
														}
#endif
                                    //
                                    if (canGet)
                                    {
                                        //  note: 2014/11/21. 这里不再直接覆盖输入内容了，而是要求一定要用param_findOrgMsg来输出. 不知道对原来的有没有影响 
                                        //  memcpy(  &pContent->transferAvInfo.tranInfo,  &pTransferAvInfoElem->tranInfo,  sizeof(  pContent->transferAvInfo.tranInfo  )  );
                                        //
#if __DEBUG__
															traceLogA("findOrgMsgAndProc_mis: get req again");
#endif
                            
                            //                             
                            //PARAM_findOrgMsg pParam = (PARAM_findOrgMsg)p1;
                            if (p1Param != null)
                            {
                                if (p1Param.pMsgTask_o != null)
                                {
                                    int len = (int)((uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgElem.lenInBytes);
                                    //
                                    fixed (MIS_MSGU* pMsgTask_o = &p1Param.pMsgTask_o.msgU)
                                    {
                                        fixed (MIS_MSG_TASK* pMsgTask_i = &pMsgElem)
                                        {
                                            mymemcpy((IntPtr)pMsgTask_o, (IntPtr)pMsgTask_i, (uint)len);
                                        }
                                    }

                                }
                            }
                                    }

                                    iErr = 1; goto errLabel;
                                }
#if false
                    //  2014/11/21
                    if (pMsgElem.iTaskId == pMsgTask.iTaskId)
                                {
                                    if (pTransferAvInfoElem.proxiedTranInfo.idInfo.ui64Id != 0 && pTransferAvInfoElem.proxiedTranInfo.uiObjType != 0)
                                    {
                                        if (pTransferAvInfoElem.proxiedTranInfo.idInfo.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id)
                                        {
                                            if (pTransferAvInfoElem.proxiedTranInfo.ti.audio.uiTranNo_openAvDev != 0 || 0 != pTransferAvInfoElem.proxiedTranInfo.ti.video.uiTranNo_openAvDev)
                                            {
                                                bool bNotMatched = false;
                                                if (pTransferAvInfoElem.proxiedTranInfo.ti.audio.uiTranNo_openAvDev != 0)
                                                {
                                                    if (pTransferAvInfoElem.proxiedTranInfo.ti.audio.uiTranNo_openAvDev != pTransferAvInfo.ass.tranInfo.audio.uiTranNo_openAvDev) bNotMatched = true;
                                                }
                                                if (pTransferAvInfoElem.proxiedTranInfo.ti.video.uiTranNo_openAvDev != 0)
                                                {
                                                    if (pTransferAvInfoElem.proxiedTranInfo.ti.video.uiTranNo_openAvDev != pTransferAvInfo.ass.tranInfo.video.uiTranNo_openAvDev) bNotMatched = true;
                                                }
                                                if (!bNotMatched)
                                                {
#if __DEBUG__
																		traceLog(_T("findOrgMsgAndProc_mis: get req content"));
#endif
                                                    PARAM_findOrgMsg pParam = (PARAM_findOrgMsg)p1;
                                                    if (pParam != null)
                                                    {
                                                        if (pParam.pMsgTask_o != null)
                                                        {
                                                            int len = (int)((int)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsgElem.lenInBytes);

                                                            pParam.pMsgTask_o.msgU.task = pMsgElem;
                                                        }
                                                    }
                                                    iErr = 1; goto errLabel;
                                                }
                                            }
                                        }
                                    }
                                }
#endif
                                //  2014/11/21
                                if (bApplyForAvProxy(pMisCntElem, ref pTransferAvInfoElem)
                                    && pTransferAvInfo.proxiedTranInfo.idInfo.ui64Id != 0
                                    && pTransferAvInfo.proxiedTranInfo.uiObjType != 0)
                                {
                                    if (pTransferAvInfo.proxiedTranInfo.idInfo.ui64Id == pMisCntElem.idInfo.ui64Id)
                                    {
                                        if (pTransferAvInfo.proxiedTranInfo.ti.audio.uiTranNo_openAvDev != 0 || pTransferAvInfo.proxiedTranInfo.ti.video.uiTranNo_openAvDev != 0)
                                        {
                                            bool bNotMatched = false;
                                            if (pTransferAvInfo.proxiedTranInfo.ti.audio.uiTranNo_openAvDev != 0)
                                            {
                                                if (pTransferAvInfo.proxiedTranInfo.ti.audio.uiTranNo_openAvDev != pTransferAvInfoElem.ass.tranInfo.audio.uiTranNo_openAvDev) bNotMatched = true;
                                            }
                                            if (pTransferAvInfo.proxiedTranInfo.ti.video.uiTranNo_openAvDev != 0)
                                            {
                                                if (pTransferAvInfo.proxiedTranInfo.ti.video.uiTranNo_openAvDev != pTransferAvInfoElem.ass.tranInfo.video.uiTranNo_openAvDev) bNotMatched = true;
                                            }
                                            if (!bNotMatched)
                                            {
#if __DEBUG__
																	traceLog(_T("proxy reply returned"));
#endif
                                                PARAM_findOrgMsg pParam = (PARAM_findOrgMsg)p1;
                                                if (pParam != null)
                                                {
                                                    if (pParam.bNeedUpdate)
                                                    {
                                                        pTaskElem.applyForProxy.bRecvd_proxyReply = true;
                                                        //
                                                        pTaskElem.applyForProxy.proxyReply.proxy.uiTranNo_openAvDev_a = pTransferAvInfo.ass.tranInfo.audio.uiTranNo_openAvDev;
                                                        pTaskElem.applyForProxy.proxyReply.proxy.uiTranNo_openAvDev_v = pTransferAvInfo.ass.tranInfo.video.uiTranNo_openAvDev;
                                                    }
                                                }
                                                iErr = 1; goto errLabel;
                                            }
                                        }
                                    }
                                }

                            }

                            if (pContent.uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                            {
                                if (pMsgElem.tStartTime == pContent.transferAvReplyInfo.tStartTime_org
                                    && pMsgElem.uiTranNo == pContent.transferAvReplyInfo.uiTranNo_org)
                                {
                                    //OutputDebugString(_T("tmpHandler_findOrgMsgAndProc_is . msg - reply is matched\n"));            //  

                                    old_procTaskAvReply(pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pTransferAvInfoElem.ass.tranInfo, ref pTaskElem, ref pMsgTask);

                                    //
                                    iErr = 1; goto errLabel;
                                }
                            }

                            //  											 
                            if (pContent.uiType == Consts.CONST_imCommType_transferVideoData)
                            {                           //  
                                                        //
                                tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procTaskVideoData(ref ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgTask, ref pContent.transferVideoData, ref pContentElem, ref pTaskElem);
                                if (0 != tmpiRet)
                                {
                                    iErr = tmpiRet; goto errLabel;
                                }
                            }
                            if (pContent.uiType == Consts.CONST_imCommType_transferAudioData)
                            {
                                //
                                tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procTaskAudioData(ref ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgTask, ref pContent.transferAudioData, ref pContentElem, ref pTaskElem);
                                if (0 != tmpiRet)
                                {
                                    iErr = tmpiRet; goto errLabel;
                                }
                            }
                            if (pContent.uiType == Consts.CONST_imCommType_remoteAssistReq)
                            {
                            }

                            if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                            {   //  file·
                                if (pMsgElem.iTaskId != 0 && pMsgElem.iTaskId == pMsgTask.iTaskId)
                                {
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq.iTaskId [%d] is matched", pMsgTask->iTaskId);
#endif
                                    //
                                    iErr = 1; goto errLabel;
                                }

                                if (pMsgElem.tStartTime == pContent.taskProcReq.tStartTime_org
                                    && pMsgElem.uiTranNo == pContent.taskProcReq.uiTranNo_org)
                                {
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq is matched");
#endif
                                    //
                                    iErr = 1; goto errLabel;
                                }
                            }
                        }
                        else
                        {

                            //  ÒÔÉÏÎªmedia·¢Æð¶Ë

                            //if (pContentElem->uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                            {                           //  media»Ø¸´¶Ë
                                TASK_transferAv task = (TASK_transferAv)taskElem;
                                //TRANSFER_AV_replyInfo* pTransferAvReplyInfoElem = &pContentElem->transferAvReplyInfo;
                                ref TRANSFER_AV_INFO pTransferAvInfoElem = ref pContentElem.transferAvInfo;

                                ref PROC_TASK_AV pTaskElem = ref task.myTask;// get_transferAvReplyInfo_pTask(pTransferAvReplyInfoElem, _T(""));
                                                                             //if (!pTaskElem) goto errLabel;
                                int index = 0;

                                //  ÒòÎªaudio¶ÔÊµÊ±ÐÔÒªÇó×î¸ß£¬ËùÒÔ·ÅÔÚµÚÒ»Î»±È½Ï
                                if (pContent.uiType == Consts.CONST_imCommType_transferAudioData)
                                {
                                    //
                                    tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procTaskAudioData(ref ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgTask, ref pContent.transferAudioData, ref pContentElem, ref pTaskElem);
                                    if (0 != tmpiRet)
                                    {
                                        iErr = tmpiRet; goto errLabel;
                                    }

                                }
                                if (pContent.uiType == Consts.CONST_imCommType_transferVideoData)
                                {                           //  ¶Ô·½µÄÊý¾Ý
                                                            //
                                    tmpiRet = tmpHandler_findOrgMsgAndProcess_mis_procTaskVideoData(ref ctxCaller, pProcInfo, ref pMsgElem.addr_logicalPeer, pMsgElem.iTaskId, ref pMsgTask, ref pContent.transferVideoData, ref pContentElem, ref pTaskElem);
                                    if (0 != tmpiRet)
                                    {
                                        iErr = tmpiRet; goto errLabel;
                                    }

                                }

                                if (pContent.uiType == Consts.CONST_imCommType_transferAvInfo)
                                {       //  ÖØ·¢Ê±ÕâÀïÒªÐ£ÑéµÄ
                                    if (pMsgElem.data.route.idInfo_to.ui64Id == pMsgTask.data.route.idInfo_from.ui64Id
                                        && pContentElem.transferAvReplyInfo.tStartTime_org == pMsgTask.tStartTime
                                        && pContentElem.transferAvReplyInfo.uiTranNo_org == pMsgTask.uiTranNo)
                                    {
                                        //
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - req is matched");
#endif
                                        //
                                        iErr = 1; goto errLabel;
                                    }
                                }
                                if (pContent.uiType == Consts.CONST_imCommType_transferAvReplyInfo)
                                {                           //  ´¦Àí
                                    if (pMsgElem.tStartTime == pContent.transferAvReplyInfo.tStartTime_org
                                        && pMsgElem.uiTranNo == pContent.transferAvReplyInfo.uiTranNo_org)
                                    //if (pContentElem->transferAvReplyInfo.tStartTime_org == pContent->transferAvReplyInfo.tStartTime_org
                                    //	&& pContentElem->transferAvReplyInfo.uiTranNo_org == pContent->transferAvReplyInfo.uiTranNo_org)
                                    {
                                        //
#if DEBUG
                                        traceLogA("tmpHandler_findOrgMsgAndProc_is . msg - avReplyInfo matched.");
#endif
                                        //
                                        iErr = 1; goto errLabel;
                                    }
                                }
                                if (pContent.uiType == Consts.CONST_imCommType_taskProcReq)
                                {   //  
                                    if (pContentElem.transferAvReplyInfo.tStartTime_org == pContent.taskProcReq.tStartTime_org
                                        && pContentElem.transferAvReplyInfo.uiTranNo_org == pContent.taskProcReq.uiTranNo_org)
                                    {
                                        //
#if __DEBUG__
														traceLogA("tmpHandler_findOrgMsgAndProc_is: msg - procReq is matched");
#endif
                                        //
                                        iErr = 1; goto errLabel;
                                    }
                                }

                            }  //  ÒÔÉÏÎªmedia»Ø¸´¶Ë¡£
                        }

            iErr = 0;
        errLabel:
            return iErr;

        }


    }
}
