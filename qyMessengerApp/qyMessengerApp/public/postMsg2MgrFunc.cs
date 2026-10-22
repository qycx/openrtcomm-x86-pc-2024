using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace qy
{
    partial class qyFuncs
    {
        static int logCountE = 0;
        public static unsafe int postMsg2MgrFunc_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT, ref MSG_ROUTE pRoute, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, ushort usSeqNo, int iTaskId, uint uiTaskType, byte* data, uint dataLen,
                              ulong pIdInfo_logicalPeer1,
                              ulong pIdInfo_taskSender,
                              ulong pIdInfo_taskReceiver,
                              ulong pIdInfo_dst,
                              uint uiChannelType,
                              MIS_MSGU *pMsgBuf, bool bLog)
        {
            int iErr = -1;

            MIS_CNT pMisCnt = (MIS_CNT)pMIS_CNT;
            QY_MESSENGER_ID idInfo_logicalPeer;
            idInfo_logicalPeer.ui64Id = 0;
            if (pIdInfo_logicalPeer1 != null) idInfo_logicalPeer.ui64Id = pIdInfo_logicalPeer1;


            if (null == pMisCnt)
            {
                traceLogA("postMsg2MgrFunc_mc failed: pMisCnt is null.");
                return -1;
            }

            CCtxQyMc pQyMc = (CCtxQyMc)g.g_pQyMc;
            if (null == pQyMc) return -1;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pMisCnt->pProcInfoParam;
            if (null == pProcInfo) return -1;
            //  QY_SERVICEGUI_INFO		*	pSci		=	(  QY_SERVICEGUI_INFO  *  )pProcInfo->pSgiParam;

            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                MIS_MSGU* pMsg = pMsgBuf;
                if (null == pMsg)
                {
                    if (logCountE < 5)
                    {
                        ++logCountE;
                    }
                    else
                    {
                        logCountE = 0;
                        traceLog(("postMsg2MgrFunc_mc: pMsgBuf is null, need to malloc\n"));
                    }

                    //
                    pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_msg, sizeof(MIS_MSGU));
                }
                if (null == pMsg) return -1;
                int len = 0;
                int tmpiRet;

                //  2011/01/23
                bool bMix = false;
                if (uiMisMsgType == Consts.CONST_misMsgType_outputTask_toMix)
                {
                    bMix = true;
                    uiMisMsgType = Consts.CONST_misMsgType_task;
                }

                //
                if (0 != makeMsg2MgrFunc_mc(pMisCnt, ref pRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, usSeqNo, iTaskId, uiTaskType, data, dataLen, idInfo_logicalPeer.ui64Id, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsg, &len))
                {
                    goto errLabel;
                }

                if (pMsg->uiType == Consts.CONST_misMsgType_outputTask)
                {       //  2008/05/13, ¶ÔoutputTask, Ö±½Ó·¢µ½Êä³ö¶ÔÁÐÀïÁË
                    MIS_CHANNEL pChannel = null;

                    if (null == pMisCnt) goto errLabel;
                    if (null == (pChannel = getChannelByType(pMisCnt, pMsg->task.uiChannelType))) goto errLabel;

                    if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsg->task, (uint)len, pChannel, ("postMsg2MgrFunc_mc"))) goto errLabel;

                    iErr = 0; goto errLabel;

                }

                //  2011/01/23
                if (bMix)
                {
                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                    tmpiRet = pProcInfo.processQ_media.qTraverse(ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *pMsg, param);
                    if ( tmpiRet<= 0)
                    {
                        traceLog(("postMsg2MgrFunc_mc: msg_task_toMix"), tmpiRet != 0 ? "failed" : "not found");
                        goto errLabel;
                    }

                    iErr = 0; goto errLabel;
                }

                //  printMisMsg(  0,  0,  &msg  );

                //  2015/09/09


                //
                switch (uiChannelType)
                {
                    case Consts.CONST_channelType_robot:
                        if (0 != (tmpiRet = qPostMsgAndTrigger((IntPtr)pMsg, (uint)len, pProcInfo.robotQ)))
                        {
                            iErr = tmpiRet; goto errLabel;
                        }
                        break;
                    case Consts.CONST_channelType_media:
                        {
                            //  CQyQ2Help	q2Help;

                            if (0 != (tmpiRet = q2PostMsgAndTrigger((IntPtr)pMsg, (uint)len, pProcInfo.mediaQ2)))
                            {
                                iErr = tmpiRet; goto errLabel;
                            }
                        }
                        break;
                    case Consts.CONST_channelType_realTimeMedia:
                        {
                            if (0 != (tmpiRet = q2PostMsgAndTrigger((IntPtr)pMsg, (uint)len, pProcInfo.realTimeMediaQ2)))
                            {
                                iErr = tmpiRet; goto errLabel;
                            }
                        }
                        break;
                    case Consts.CONST_channelType_rtOp:
                        {
                            if (0 != (tmpiRet = q2PostMsgAndTrigger((IntPtr)pMsg, (uint)len, pProcInfo.rtOpQ2)))
                            {
                                iErr = tmpiRet; goto errLabel;
                            }
                        }
                        break;
                    default:
                        if (0 != (tmpiRet = qPostMsgAndTrigger((IntPtr)pMsg, (uint)len, pProcInfo.mgrQ)))
                        {
                            iErr = tmpiRet; goto errLabel;
                        }
                        break;
                }


                if (bLog)
                {   //  2010/07/22
                    postImMsg2Log_isClient(pMsg, len);
                }

            }


            iErr = 0;

        errLabel:
            return iErr;
        }


        public static unsafe int postImMsg2Log_isClient(MIS_MSGU* pMsg, int lenInBytes_msg)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;

            int tmpiRet = -1;


#if true
            qPostMsg((IntPtr)pMsg, (uint)lenInBytes_msg, pProcInfo.imMsgLogQ);

            if (0 != qPostMsgAndTrigger(IntPtr.Zero, 0, pProcInfo.schedulerQ)) goto errLabel;
#endif

            //
            iErr = 0;

            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int makeMsg2MgrFunc_mc(MIS_CNT pMisCnt, ref MSG_ROUTE pRoute, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, ushort usSeqNo, int iTaskId, uint uiTaskType, byte* data, uint dataLen,
                          ulong pIdInfo_logicalPeer,
                           ulong pIdInfo_taskSender,
                          ulong pIdInfo_taskReceiver,
                          ulong pIdInfo_dst,
                          uint uiChannelType, MIS_MSGU* pMsg, int* pMsgLen)
        {
            int iErr = -1;
            int len = 0;

            if (pMisCnt == null)
            {
                traceLog(("makeMsg2MgrFunc_mc failed, pMisCnt is null"));
                return -1;
            }
            if (null == pMsg)
            {
                traceLog(("makeMsg2MgrFunc_mc failed, pMsg is null"));
                return -1;
            }

            //
#if DEBUG
            ref MIS_MSGU tmp_pMsg = ref *pMsg;
#endif

            //
            switch (uiMisMsgType)
            {
                case Consts.CONST_misMsgType_input:
                    //
                    MACRO_memset_misMsgInput(&pMsg->input);
                    pMsg->uiType = uiMisMsgType;

                    //
                    //pMsg->input.pMisCnt = pMisCnt;
                    //
                    pMsg->input.ucFlg = ucFlg;
                    pMsg->input.usCode = usCode;
                    //
                    pMsg->input.tStartTime = tStartTime;
                    pMsg->input.uiTranNo = uiTranNo;
                    pMsg->input.usSeqNo = usSeqNo;
                    //
                    mytime(ref pMsg->input.tRecvTime);                     //  2008/04/05
                    pMsg->input.dwTickCnt_start1 = myGetTickCount64(null);       //  2008/05/23		
                                                                              //
                    pMsg->input.uiChannelType = uiChannelType;              //  2008/04/17
                                                                            //
                    pMsg->input.uiChannelType_input = uiChannelType;        //  2017/10/03
                                                                            //
                                                                            //  pMsg->input.usDataType;
                    pMsg->input.lenInBytes = (uint)Math.Min(sizeof(MIS_MSG_routeTalkData), dataLen);
                    mymemcpy((IntPtr)(byte*)&pMsg->input.data, (IntPtr)data, pMsg->input.lenInBytes);
                    //
                    len = (int)((uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsg->input.lenInBytes);

                    break;
                case Consts.CONST_misMsgType_req:
                    //
                    MACRO_memset_misMsgReq(&pMsg->req);
                    pMsg->uiType = uiMisMsgType;

                    //
                    //pMsg->req.pMisCnt = pMisCnt;
                    //
                    pMsg->req.ucFlg = ucFlg;
                    //
                    pMsg->req.usCode = usCode;
                    //
                    pMsg->req.tStartTime = tStartTime;
                    pMsg->req.uiTranNo = uiTranNo;
                    pMsg->req.usSeqNo = usSeqNo;
                    //
                    pMsg->req.lenInBytes = (uint)Math.Min(sizeof(IM_CONTENTU), dataLen);
                    mymemcpy((IntPtr)(byte*)&pMsg->req.data, (IntPtr)data, pMsg->req.lenInBytes);
                    //
                    len = (int)((uint)Marshal.OffsetOf<MIS_MSG_REQ>("data") + pMsg->req.lenInBytes);
                    break;
                case Consts.CONST_misMsgType_talk:
                    //
                    MACRO_memset_misMsgTalk(&pMsg->talk);
                    pMsg->uiType = uiMisMsgType;

                    //
                    //pMsg->talk.pMisCnt = pMisCnt;
                    //
                    pMsg->talk.ucFlg = (byte)(ucFlg | Consts.CONST_commFlg_talkData);
                    pMsg->talk.usCode = usCode;
                    //
                    pMsg->talk.tStartTime = tStartTime;
                    pMsg->talk.uiTranNo = uiTranNo;
                    pMsg->talk.usSeqNo = usSeqNo;
                    //
                    /*
                    if (pRoute != null) mymemcpy((IntPtr)(byte*)&pMsg->talk.data.route, (IntPtr)pRoute, (uint)sizeof(MSG_ROUTE));
                    else pMsg->talk.data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
                    */
                    if (pRoute.idInfo_from.ui64Id != 0) pMsg->talk.data.route=pRoute;
                    else pMsg->talk.data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;

                    //				   
                    if (pIdInfo_dst != null) pMsg->talk.data.route.idInfo_to.ui64Id = pIdInfo_dst;
                    //
                    if (null != pIdInfo_logicalPeer && pIdInfo_logicalPeer != 0) pMsg->talk.addr_logicalPeer.idInfo.ui64Id = pIdInfo_logicalPeer;  //  ÕâÀï¸³ÉÏÁËÄ¿±ê·½
                    else
                    {
                        traceLogA("msgTalk: no logicalPeer");
                    }
                    //
                    pMsg->talk.uiChannelType = uiChannelType;       //  2008/04/17
                                                                    //
                    len = (int)Math.Min(Consts.bufSize_routeTalkData, dataLen);
                    mymemcpy((IntPtr)(byte*)&pMsg->talk.data.buf, (IntPtr)data, (uint)len);
                    //
                    pMsg->talk.lenInBytes = (uint)((uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + len);
                    //
                    len = (int)((uint)Marshal.OffsetOf<MIS_MSG_TALK>("data") + pMsg->talk.lenInBytes);

                    break;
                case Consts.CONST_misMsgType_task:
                case Consts.CONST_misMsgType_outputTask:
                    //
                    MACRO_memset_misMsgTask(ref pMsg->task);
                    pMsg->uiType = uiMisMsgType;

                    //
                    //pMsg->task.pMisCnt = pMisCnt;
                    //
                    pMsg->task.ucFlg = (byte)(ucFlg | Consts.CONST_commFlg_talkData);
                    pMsg->task.usCode = usCode;
                    //
                    pMsg->task.tStartTime = tStartTime;
                    pMsg->task.uiTranNo = uiTranNo;
                    pMsg->task.usSeqNo = usSeqNo;
                    //
                    pMsg->task.iTaskId = iTaskId;
                    pMsg->task.uiTaskType = uiTaskType;
                    //
                    /*
                    if (pRoute != null) mymemcpy((IntPtr)(byte*)&pMsg->task.data.route, (IntPtr)pRoute, (uint)sizeof(MSG_ROUTE));
                    else pMsg->task.data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
                    */
                    if (pRoute.idInfo_from.ui64Id != 0) pMsg->task.data.route=pRoute;
                    else pMsg->task.data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;

                    //
                    if (pIdInfo_dst != null) pMsg->task.data.route.idInfo_to.ui64Id = pIdInfo_dst;
                    //
                    if (null != pIdInfo_logicalPeer && pIdInfo_logicalPeer != null) pMsg->task.addr_logicalPeer.idInfo.ui64Id = pIdInfo_logicalPeer;  //  ÕâÀï¸³ÉÏÁËÄ¿±ê·½
                    else
                    {
                        //  traceLogA(  "msgTask: no logicalPeer"  );						 
                    }

                    if (pIdInfo_taskSender != null) pMsg->task.idInfo_taskSender.ui64Id = pIdInfo_taskSender;
                    if (pIdInfo_taskReceiver != null) pMsg->task.idInfo_taskReceiver.ui64Id = pIdInfo_taskReceiver;

                    if (0 == pMsg->task.dwTickCnt_start1) pMsg->task.dwTickCnt_start1 = myGetTickCount64(null);   //  2009/03/22
                                                                                                              //
                    pMsg->task.uiChannelType = uiChannelType;       //  2008/04/17

                    //
                    len = (int)Math.Min(Consts.bufSize_routeTalkData, dataLen);
                    mymemcpy((IntPtr)(byte*)&pMsg->task.data.buf, (IntPtr)data, (uint)len);
                    //
                    pMsg->task.lenInBytes = (uint)((uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + len);
                    //
                    len = (int)((uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsg->task.lenInBytes);

                    break;
                default:
                    {
                        string str;
                        //str=string.Format("makeMsg2MgrFunc_mc failed: unkown msg, {0}", qyGetDesByType1(CONST_misMsgTypeTable, pMsg->uiType));
                        traceLog("makeMsg2MgrFunc_mc failed: unkown msgType");
                    }
                    goto errLabel;

            }

            //
            if (0 == len) goto errLabel;

            iErr = 0;

        errLabel:

            if (0 == iErr)
            {
                if (pMsgLen != null)
                {
                    *pMsgLen = len;
                }
            }

            return iErr;
        }


        public static unsafe int procMsgTask2Output_mc(CCtxQmc pProcInfo, MIS_CNT pMisCnt, ref MIS_MSG_TASK pMsgTask, uint lenInBytes_msg, MIS_CHANNEL pChannel, string tHint)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = (CCtxQyMc)pProcInfo.pQyMc;
            ref IM_CONTENTU pContent = ref pMsgTask.data.buf;// M_getMsgContent(pMsgTask->uiType,pMsgTask->ucFlg, &pMsgTask->data);
            if (null == tHint) tHint = ("");

            //
            if (0 == pMsgTask.data.route.idInfo_to.ui64Id)
            {
                traceLogA("procMsgTask2Output_mc failed: route.idInfo_to is 0");
                goto errLabel;
            }
            pMsgTask.data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;


            //  2015/08/22


            //
            bool tmp_bChannelOk = bChannelOk(pChannel);
            if (!tmp_bChannelOk)
            {

                //  2015/08/22
                string str = string.Format("procMsgTask2Output_mc failed: {0} is not ready. {1}", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType), tHint);

                if (logCountF < 6)
                {
                    ++logCountF;
                }
                else
                {
                    qyShowInfo1(Consts.CONST_qyShowType_warning, pProcInfo.str_who_showInfo, null, (""), str);

                    logCountF = 0;

                }

            }
            //if (pContent != null)
            {
                switch (pContent.uiType)
                {
                    case Consts.CONST_imCommType_transferVideoData:
                    case Consts.CONST_imCommType_transferAudioData:
                    case Consts.CONST_imCommType_remoteAssistReq:
                    case Consts.CONST_imCommType_ptzControlReq:                    //  2012/8/07
                    case Consts.CONST_imCommType_ptzControlCmd:
                        if (!tmp_bChannelOk)
                        {
                            goto errLabel;
                        }
                        break;
                    default:
                        break;
                }
            }


            {
                using (CQyQ2Help q2Help = new CQyQ2Help())
                {
                    if (0 != tmp_qPost_msgTask(ref pMsgTask, lenInBytes_msg, q2Help.getQ_toPostMsg(pChannel.toSendQ2)))
                    {
                        if (!isQFull(q2Help.getQ_toPostMsg(pChannel.toSendQ2)))
                        {   //  2008/11/21
                            goto errLabel;
                        }
                        else
                        {
                            switch (pChannel.uiType)
                            {
                                case Consts.CONST_channelType_media:
                                case Consts.CONST_channelType_realTimeMedia:
                                    emptyGenericQ(q2Help.getQ_toPostMsg(pChannel.toSendQ2));
                                    qyShowInfo1(Consts.CONST_qyShowType_warning, pProcInfo.str_who_showInfo, null, (""), ("procMsgTask2Output_mc: emptyQ now"));
                                    break;
                                default:
                                    break;
                            }
                            //  2015/08/22. 队列满时，不能退出，应接着触发发送
                        }
                    }
                }
            }

            if (0 == q2PostMsgAndTrigger((IntPtr)0, 0, pChannel.outputQ2))
            {
                //  2008/11/22
                if (pChannel.outputQ2.ucbTriggerErr) q2AvoidTriggerTooHigh("", pChannel.outputQ2);
            }


            iErr = 0;

        errLabel:

            return iErr;
        }

        static int logCountF = 0;

        public static unsafe int postMsg2Mgr_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT, ref MSG_ROUTE pRoute, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, byte* data, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_dst, uint uiChannelType, CMIS_MSGU pMsgBuf1, bool bLog)
        {
            if (pMsgBuf1 != null)
            {
                fixed (MIS_MSGU* pMsgBuf = &pMsgBuf1.msgU)
                {
                    return postMsg2MgrFunc_mc(ctxCaller, pMIS_CNT, ref pRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, (ushort)uiSeqNo, 0, 0, data, dataLen, pIdInfo_logicalPeer, 0, 0, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);
                }
            }
            else
            {
                MIS_MSGU* pMsgBuf = null;
                return postMsg2MgrFunc_mc(ctxCaller, pMIS_CNT, ref pRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, (ushort)uiSeqNo, 0, 0, data, dataLen, pIdInfo_logicalPeer, 0, 0, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);

            }
        }

        public static unsafe int postMsg2Mgr_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT, ref MSG_ROUTE pRoute, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, ref IM_CONTENTU data, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_dst, uint uiChannelType, CMIS_MSGU pMsgBuf, bool bLog)
        {
            fixed (IM_CONTENTU* data0 = &data)
            {
                if (pMsgBuf != null)
                {
                    fixed (MIS_MSGU* pMsgBuf0 = &pMsgBuf.msgU)
                    {

                        return postMsg2MgrFunc_mc(ctxCaller, pMIS_CNT, ref pRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, (ushort)uiSeqNo, 0, 0, (byte*)data0, dataLen, pIdInfo_logicalPeer, 0, 0, pIdInfo_dst, uiChannelType, pMsgBuf0, bLog);
                    }
                }
                else
                {
                    MIS_MSGU* pMsgBuf0 = null;
                    return postMsg2MgrFunc_mc(ctxCaller, pMIS_CNT, ref pRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, (ushort)uiSeqNo, 0, 0, (byte*)data0, dataLen, pIdInfo_logicalPeer, 0, 0, pIdInfo_dst, uiChannelType, pMsgBuf0, bLog);

                }
            }
        }


        public static unsafe int postMsgTask2Mgr_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, int iTaskId, uint uiTaskType, byte* data, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_taskSender, ulong pIdInfo_taskReceiver, ulong pIdInfo_dst, uint uiChannelType, CMIS_MSGU pMsgBuf, bool bLog)
        {
            MSG_ROUTE msgRoute = new MSG_ROUTE();

            if (pMsgBuf != null)
            {
                fixed (MIS_MSGU* pMsgBuf0 = &pMsgBuf.msgU)
                {
                    return postMsg2MgrFunc_mc(ctxCaller, pMIS_CNT, ref msgRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, (ushort)uiSeqNo, iTaskId, uiTaskType, data, dataLen, pIdInfo_logicalPeer, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsgBuf0, bLog);
                }
            }
            else
            {
                MIS_MSGU* pMsgBuf0 = null;
                return postMsg2MgrFunc_mc(ctxCaller, pMIS_CNT, ref msgRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, (ushort)uiSeqNo, iTaskId, uiTaskType, data, dataLen, pIdInfo_logicalPeer, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsgBuf0, bLog);

            }
        }


        //
        public static unsafe int postMsgTask2Mgr_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, int iTaskId, uint uiTaskType, ref IM_CONTENTU data, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_taskSender, ulong pIdInfo_taskReceiver, ulong pIdInfo_dst, uint uiChannelType, CMIS_MSGU pMsgBuf, bool bLog)
        {
            fixed(IM_CONTENTU*data0=&data)
            {
                if (pMsgBuf != null)
                {
                    //fixed (MIS_MSGU* pMsgBuf0 = &pMsgBuf.msgU)
                    {
                        return postMsgTask2Mgr_mc(ctxCaller, pMIS_CNT, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, uiSeqNo, iTaskId, uiTaskType, (byte*)data0, dataLen, pIdInfo_logicalPeer, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);

                    }
                }
                else
                {
                    //MIS_MSGU *pMsgBuf0 = null;

                    return postMsgTask2Mgr_mc(ctxCaller, pMIS_CNT, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, uiSeqNo, iTaskId, uiTaskType, (byte*)data0, dataLen, pIdInfo_logicalPeer, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);

                }



            }

        }


        //
        public static unsafe int postMsgTask2Mgr_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, int iTaskId, uint uiTaskType, ref TASK_PROC_REQ data1, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_taskSender, ulong pIdInfo_taskReceiver, ulong pIdInfo_dst, uint uiChannelType, bool bLog)
        {
            CMIS_MSGU pMsgBuf = null;
            {
                fixed (TASK_PROC_REQ* data = &data1)
                {
                    return postMsgTask2Mgr_mc(ctxCaller, pMIS_CNT, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, uiSeqNo, iTaskId, uiTaskType, (byte*)data, dataLen, pIdInfo_logicalPeer, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);
                }
            }
        }



    }
}
