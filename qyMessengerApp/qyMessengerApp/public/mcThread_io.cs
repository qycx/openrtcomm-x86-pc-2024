using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
    {


        public static unsafe void mcThreadProc_mis_snd(object lpParameter)
        {
            MIS_CHANNEL pChannel = (MIS_CHANNEL)lpParameter;
            if (null == pChannel)
            {
                return;
            }
            MIS_CNT pMisCnt = pChannel.pMisCnt;
            if (null == pMisCnt) return;
            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            if (null == pProcInfo) return;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;
            if (null == pQyMc) return;
            //
            int loopCtrl = 0;
            //int i;
            //int j;
            uint dwThreadId = myGetCurrentThreadId();
            uint dwRet;

            uint len;

            MIS_MSGU* pMsg = null;

            QY_BUF buf_help = new QY_BUF();

            //
            int maxEncBufSize = 1500000;
            byte[] encBuf = new byte[maxEncBufSize];
            RecyclableMemoryStreamManager recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            int iRet;
            //  BOOL							bThreadIdFound				=	FALSE;

            //TCHAR tBuf[256];
            bool bChkTimeoutInSToSwitch = false;
            ushort usTimeoutInMsToSwitch = 0;
            int iElapse = 0;

            uint dwTickCnt_toGetMsg;
            uint dwTickCnt_startToSend;
            uint dwTickCnt_lastEndSending_totalMsg = 0;


            //
            bool bDbg = false;
            string str;

            //
            traceLogA("msgThreadProc_mis_snd:  enters");

            string where_showInfo; where_showInfo = string.Format("{0}_S", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));
            showInfo_open(0, where_showInfo, "starts");

            //
            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                using (CQyMalloc mallocObj_buf_help = new CQyMalloc())
                {

                    //  2009/12/18. channel must be found first.
                    if (null == (pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;
                    buf_help.len = (uint)sizeof(MIS_MSG_routeTalkData);
                    if (null == (buf_help.data = (byte*)mallocObj_buf_help.mallocf(Consts.CONST_iAllocType_longTimeMsg, (int)buf_help.len))) goto errLabel;


                    //
                    switch (pChannel.uiType)
                    {       //  2008/11/20
                        case Consts.CONST_channelType_media:
                            usTimeoutInMsToSwitch = pProcInfo.cfg.policy.usTimeoutInMsToSwitch;
                            bChkTimeoutInSToSwitch = true;
                            break;
                        case Consts.CONST_channelType_realTimeMedia:
                            usTimeoutInMsToSwitch = pProcInfo.cfg.policy.usTimeoutInMsToSwitch_rt;
                            bChkTimeoutInSToSwitch = true;
                            break;
                        default:
                            break;
                    }

                    //  pChannel->status.nMsgSent  =  0;

                    GENERIC_Q pOutputQ = null; //  2011/01/23

                    for (; bChannelOk(pChannel); loopCtrl++)
                    {
                        //
#if DEBUG
                        if (pChannel.uiType == Consts.CONST_channelType_media)
                        {
#if false
							int nQNodes = getQ2Nodes(pChannel.outputQ2);
							str = string.Format("thread_snd: before getMsg, outputQ.nodes {0}",nQNodes);
							traceLog(str);
#endif
                        }
#endif

                        //
                        if (isQ2Empty(pChannel.outputQ2))
                        {

                            q2PostMsgAndTrigger((IntPtr)0, 0, pChannel.toSendQ2);

                            /*
							dwRet = WaitForSingleObject(pChannel->outputQ2.hEvents[0], 3500);   //  µÈ·¢ËÍÖ¸Áî
							if (dwRet == WAIT_FAILED) goto errLabel;                            //  ÎªÁË·ÀÖ¹¶ª°ü£¬ÕâÀï²»ÅÐ¶Ï³¬Ê±£¬ÒÔoutputQÀïµÄÊý¾Ý°üÎª×¼
							if (dwRet == WAIT_TIMEOUT) continue;                                //  Ã»ÓÐÈË´¥·¢£¬ËùÒÔ³¬Ê±
							*/
                            myWaitOne(pChannel.outputQ2.hEvent0, 3500);

                            //
                            if (pProcInfo.cfg.debugStatusInfo.ucbViewIoStatus)
                            {
                                showInfo_open(0, where_showInfo, _T("after waitForSingleObject"));
                            }

                        }

                        for (; bChannelOk(pChannel);)
                        {

                            q2AvoidTriggerTooHigh(null, pChannel.outputQ2);

                            //
                            uint dwTickCnt_toWaitForSock = myGetTickCount(null);

                            //
                            iRet = GuiShare.pf_qyWaitForWrite(ref pChannel.sock, pQyMc.cfg.to.iSelect);
                            if (iRet < 0)
                            {
                                traceLogA("qyWaitForWrite failed");
                                goto errLabel;      //  ³ö´í
                            }
                            if (0 == iRet)
                            {
                                uint tmp_tickCnt = myGetTickCount(null);
                                int iDiffInMs = (int)(tmp_tickCnt - dwTickCnt_toWaitForSock);
                                //
                                str = string.Format("qyWaitForWrite returns 0. iDiffInMs {0}ms, continue", iDiffInMs);
                                showInfo_open(0, where_showInfo, str);
                                //
                                continue;        //  ³¬Ê±
                            }

                            //
                            dwTickCnt_toGetMsg = myGetTickCount(null);

                            //
                            int nQNodes = getQ2Nodes(pChannel.outputQ2);
                            //if  (  nQNodes  <  MAX_outputQ2Nodes_toPostQ  )  
                            //
                            int nQNodes_toTrigger = 2;
                            //
                            //
                            if (nQNodes < nQNodes_toTrigger)
                            {
                                q2PostMsgAndTrigger(IntPtr.Zero, 0, pChannel.toSendQ2);
                            }


                            //  
                            len = (uint)sizeof(MIS_MSGU);
                            if (0 != q2GetMsg(pChannel.outputQ2, (IntPtr)pMsg, &len)) break;

                            mymemset((IntPtr)(((byte*)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));

                            //  2009/02/25
                            dwTickCnt_startToSend = myGetTickCount(null);

                            //  traceLogA(  "mcThreadproc_mis_snd: µÃµ½Ò»ÌõÏûÏ¢[%S]",  qyGetDesByType1(  CONST_misMsgTypeTable,  pMsg->uiType  )  ); 

                            switch (pMsg->uiType)
                            {
                                case Consts.CONST_misMsgType_req:

                                    {
                                        str = "";
                                        //str = string.Format("snd: %s,  tn %d"), qyGetDesByType1(CONST_qyCmdTable, pMsg->req.usCode), pMsg->req.uiTranNo);
                                        uint step = 0;
                                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, "", str);
                                    }

                                    //
                                    if (0 != encryptData(ref pChannel.commEncCtx, ref buf_help, encBuf, ref recyclableMemoryStreamManager, (byte*)&pMsg->req.data, &pMsg->req.lenInBytes, (uint)sizeof(MIS_MSG_routeTalkData)))
                                    {
                                        uint step = 0;
                                        qyShowInfo1(Consts.CONST_qyShowType_warning, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, ("snd req: encryptData failed"), "");
                                        goto errLabel;
                                    }

                                    if (0 != qySendMsg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to, pMsg->req.ucFlg, pMsg->req.usCode, pMsg->req.tStartTime, pMsg->req.uiTranNo, pMsg->req.usSeqNo, (byte*)&pMsg->req.data, pMsg->req.lenInBytes))
                                    {
                                        traceLogA("qySendMsg failed");
                                        goto errLabel;
                                    }
                                    //  2009/10/11
                                    pChannel.status.netStat.ui64MsgDataLenSent += (ulong)(sizeof(QY_MSG_HEAD) + pMsg->req.lenInBytes);
                                    break;
                                case Consts.CONST_misMsgType_talk:
                                    {

                                        //
                                        {
                                            str = "";
                                            if (!isUcFlgResp(pMsg->task.ucFlg))
                                            {
                                                //str = string.Format("snd: tn %d, %s: len %d"), pMsg->task.uiTranNo, qyGetDesByType1(CONST_qyCmdTable, pMsg->task.usCode), pMsg->task.lenInBytes);
                                                uint step = 0;
                                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, (""), str);
                                            }
                                            else
                                            {
                                                uint step = 0;
                                                //str = string.Format("snd: tn %d, resp: %s "), pMsg->task.uiTranNo, qyGetDesByType1(CONST_qyRcTable, pMsg->task.usCode));
                                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, (""), str);
                                            }
                                        }
                                        //						
                                        if (0 != encryptData(ref pChannel.commEncCtx, ref buf_help, encBuf, ref recyclableMemoryStreamManager, (byte*)&pMsg->talk.data, &pMsg->talk.lenInBytes, (uint)sizeof(MIS_MSG_routeTalkData)))
                                        {
                                            uint step = 0;
                                            qyShowInfo1(Consts.CONST_qyShowType_warning, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, (""), ("snd talk: encryptData failed"));
                                            goto errLabel;
                                        }

                                        if (0 != qySendMsg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to, pMsg->talk.ucFlg, pMsg->talk.usCode, pMsg->talk.tStartTime, pMsg->talk.uiTranNo, pMsg->talk.usSeqNo, (byte*)&pMsg->talk.data, pMsg->talk.lenInBytes))
                                        {
                                            traceLogA("qySendMsg failed");
                                            goto errLabel;
                                        }
                                        //  2009/10/11
                                        pChannel.status.netStat.ui64MsgDataLenSent += (ulong)(sizeof(QY_MSG_HEAD) + pMsg->talk.lenInBytes);
                                    }
                                    break;
                                case Consts.CONST_misMsgType_task:
                                case Consts.CONST_misMsgType_outputTask:

                                    {

                                        //  2008/11/20
                                        if (bChkTimeoutInSToSwitch)
                                        {
                                            if (pMsg->task.dwTickCnt_start1 != 0)
                                            {
                                                iElapse = (int)(myGetTickCount64(null) - pMsg->task.dwTickCnt_start1);
                                                //
                                                if (true)
                                                {
                                                    qyFuncs.showInfo_open(0, "", "snd task elapse " + iElapse +"ms");
                                                }
                                                //
                                                if (iElapse > usTimeoutInMsToSwitch)
                                                {
                                                    bool bTaskFound = false;
                                                    //
#if false
													if (pMsg->task.iTaskId!=0)
									{   //  2010/09/23
										if (pChannel.uiType == Consts.CONST_channelType_media)
										{
											int i;
											for (i = 0; i < pProcInfo.cfg.usMaxCnt_pSharedObjs; i++)
											{
												QY_SHARED_OBJ* tmppSharedObj = getSharedObjByIndex(pProcInfo, i);
												SHARED_OBJ_USR* pSharedObjUsr = getSharedObjUsr(tmppSharedObj, tmppSharedObj->iIndex_curUsr);
												if (pSharedObjUsr
													&& pSharedObjUsr->iTaskId == pMsg->task.iTaskId)
												{
													CAP_procInfo_bmpU* pCapProcInfo = getCapBmpBySth(pProcInfo, tmppSharedObj->iIndex_capBmp, 0);
													if (pCapProcInfo)
													{
														COMPRESS_VIDEO* pCompressVideo = &pCapProcInfo->common.compressVideo;
														if (pCompressVideo)
														{
															pCompressVideo->usTimes_failure += MAX_failures_frames_perSecond + 1;
															bTaskFound = TRUE;
														}
													}
												}
											}
										}
									}
#endif
                                                    //
                                                    str = "";
#if false
													_sntprintf(tBuf, mycountof(tBuf), _T("snd: %dms elapsed, skipped. tn %d, %s."), iElapse, pMsg->task.uiTranNo, bTaskFound ? _T("to adjust speed.") : _T(""));
									if (pMsg->task.dwTickCnt_toSend)
									{
										_sntprintf(tBuf, mycountof(tBuf), _T("%s -2Snd %dms."), tBuf, pMsg->task.dwTickCnt_toSend - pMsg->task.dwTickCnt_start);
									}
									_sntprintf(tBuf, mycountof(tBuf), _T("%s last->s %dms."), tBuf, dwTickCnt_startToSend - pChannel->status.dwTickCnt_lastEndSending);
#endif
                                                    str = string.Format("snd: {0}ms elapsed, skipped. tn {1}, ", iElapse, pMsg->task.uiTranNo);

                                                    uint step = 0;
                                                    qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, (""), str);
                                                    break;
                                                }
                                            }
                                        }

                                        //
                                        if (0 != encryptData(ref pChannel.commEncCtx, ref buf_help, encBuf, ref recyclableMemoryStreamManager, (byte*)&pMsg->task.data, &pMsg->task.lenInBytes, (uint)sizeof(MIS_MSG_routeTalkData)))
                                        {
                                            uint step = 0;
                                            qyShowInfo1(Consts.CONST_qyShowType_warning, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, (""), ("snd task: encryptData failed"));
                                            goto errLabel;
                                        }

                                        uint uiSizePerSnd_media = pMisCnt.cfg.uiSizePerSnd_media;     //  2011/01/30
                                        if (0 == uiSizePerSnd_media) uiSizePerSnd_media = Consts.MIN_uiSizePerSnd_media;

                                        {       //  
                                            byte* p = null;
                                            uint nLeft = 0;
                                            uint uiSeqNo = 0;
                                            uint nSend = uiSizePerSnd_media;
                                            byte ucFlg = 0;
                                            //  TCHAR				tBuf[255  +  1]		=	_T(  ""  );
                                            //
                                            p = (byte*)&pMsg->task.data;
                                            nLeft = pMsg->task.lenInBytes;

                                            for (uiSeqNo = 0; nLeft != 0 && bChannelOk(pChannel); uiSeqNo++)
                                            {
                                                nSend = Math.Min(nSend, nLeft);
                                                //
                                                ucFlg = (byte)((nLeft > nSend) ? (pMsg->task.ucFlg | Consts.CONST_commFlg_moreData) : pMsg->task.ucFlg);
                                                //
#if DEBUG
                                                if (true)
                                                {
                                                    if (nLeft > nSend)
                                                    {
                                                        //OutputDebugString(_T("\n"));
                                                    }
                                                    if (bDbg)
                                                    {
                                                        //_sntprintf(tBuf, mycountof(tBuf), _T("before qySendMsg: uiSeqNo [%d], nSend [%d], lenInBytes [%d]\n"), uiSeqNo, nSend, pMsg->task.lenInBytes);
                                                        str = string.Format("before qySendMsg: uiSeqNo [{0}], nSend [{1}], lenInBytes [{2}]\n", uiSeqNo, nSend, pMsg->task.lenInBytes);
                                                        uint step = 0;
                                                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, (""), str);

                                                        //OutputDebugString(tBuf);
                                                    }
                                                }
#endif
                                                //
                                                if (0 != qySendMsg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to, ucFlg, pMsg->task.usCode, pMsg->task.tStartTime, pMsg->task.uiTranNo, (ushort)uiSeqNo, p, nSend))
                                                {
                                                    traceLogA("qySendMsg failed");
                                                    goto errLabel;
                                                }
                                                //  2009/10/11
                                                pChannel.status.netStat.ui64MsgDataLenSent += (ulong)(sizeof(QY_MSG_HEAD) + nSend);
                                                //
                                                //
                                                pChannel.status.dwTickCnt_lastEndSending = myGetTickCount(null);   //  2011/01/23

                                                //
                                                nLeft -= nSend;
                                                p += nSend;
                                                continue;
                                            }
                                        }

                                        //  2011/01/22
                                        ulong dwTickCnt_endSending = myGetTickCount(null);
                                        if (bDbg)
                                        {

                                            //if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewIoStatus)
                                            {
                                                if (!isUcFlgResp(pMsg->task.ucFlg))
                                                {
                                                    //_sntprintf(tBuf, mycountof(tBuf), _T("snd: tn %d, %s: len %d."), pMsg->task.uiTranNo, qyGetDesByType1(CONST_qyCmdTable, pMsg->task.usCode), pMsg->task.lenInBytes);
                                                    str = string.Format("snd req: tn {0}, {1}: len {2}.", pMsg->task.uiTranNo, qyGetDesByType1(qyFuncs.CONST_qyCmdTable, pMsg->task.usCode), pMsg->task.lenInBytes);
                                                }
                                                else
                                                {
                                                    //_sntprintf(tBuf, mycountof(tBuf), _T("snd: tn %d, resp: %s."), pMsg->task.uiTranNo, qyGetDesByType1(CONST_qyRcTable, pMsg->task.usCode));
                                                    str = string.Format("snd resp: tn {0}, resp: {1}.", pMsg->task.uiTranNo, qyGetDesByType1(CONST_qyRcTable, pMsg->task.usCode));
                                                }
                                                //  if  (  pMsg->task.index_debugInfo  )  _sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s index %d."  ),  tBuf,  pMsg->task.index_debugInfo  );
#if false
												if (pMsg->task.dwTickCnt_start)
												{
													_sntprintf(tBuf, mycountof(tBuf), _T("%s s-snt %d."), tBuf, dwTickCnt_endSending - pMsg->task.dwTickCnt_start);
												}
												if (pMsg->task.dwTickCnt_toSend)
												{
													_sntprintf(tBuf, mycountof(tBuf), _T("%s -2Snd %u."), tBuf, pMsg->task.dwTickCnt_toSend - pMsg->task.dwTickCnt_start);
												}
												_sntprintf(tBuf, mycountof(tBuf), _T("%s l-m-s-end %d, %d, %d."), tBuf, dwTickCnt_toGetMsg - pChannel->status.dwTickCnt_lastEndSending, dwTickCnt_startToSend - dwTickCnt_toGetMsg, dwTickCnt_endSending - dwTickCnt_startToSend);
												_sntprintf(tBuf, mycountof(tBuf), _T("%s nQ %d."), tBuf, getQ2Nodes_toGetMsg(&pChannel->outputQ2));
#endif

                                                uint step = 0;
                                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, where_showInfo, _T(""), str);
                                            }
                                        }



                                    }
                                    break;
                                default:

                                    //qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("msThreadProc_mis_snd: Î´´¦ÀíµÄÏûÏ¢%S"), qyGetDesByType1(CONST_misMsgTypeTable, pMsg->uiType));
                                    //traceLogA("err,  [%S]", qyGetDesByType1(CONST_misMsgTypeTable, pMsg->uiType));
                                    goto errLabel;
                                    break;
                            }

                            //  pChannel->status.netStat.nMsgSent  ++  ;
                            pChannel.status.dwTickCnt_lastEndSending = myGetTickCount(null);   //  2011/01/23

                            dwTickCnt_lastEndSending_totalMsg = pChannel.status.dwTickCnt_lastEndSending;


                        }

                    }
                }


            }


        errLabel:


            //
            showInfo_open(0, where_showInfo, "ends");


            if (null != pMisCnt)
            {
                pChannel.status.ulbSessionErr = true;
                pChannel.status.ulbSndQuit = true;
            }
            Console.WriteLine("msgThreadProc_mis_snd:  leaves");

            return;
        }



        public class CTX_thread_io : CTX_qm_thread
        {
            //public CTX_qm_thread common;

            //
            public Thread hThread_toSend;                                  //  2008/05/23
                                                                           //DWORD dwThreadId_toSend;

            //  socket机制使用
            public Thread hThread_snd;
            //DWORD dwThreadId_snd;
        };


        //
        public static unsafe void mcThreadProc_mis_io(object lpParameter)
        {
            int iErr = -1;
            //QY_SERVICEGUI_INFO	*	pSci				=	(  QY_SERVICEGUI_INFO  *  )lpParameter;
            MIS_CHANNEL pChannel = (MIS_CHANNEL)lpParameter;
            if (null == pChannel)
            {
                return;
            }
            MIS_CNT pMisCnt = (MIS_CNT)pChannel.pMisCnt;
            if (null == pMisCnt) return;
            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            if (null == pProcInfo) return;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;
            if (null == pQyMc) return;


            int i;
            int j;
            uint dwThreadId = myGetCurrentThreadId();
            //BOOL						bThreadIdFound		=	FALSE;
            uint dwThreadDaemonId;
            int iRet;
            QY_MSG_HEAD msgHead;
            MIS_MSG_INPUT* pMsg = null;
            //
            QY_BUF buf_help = new QY_BUF();

            //
            RecyclableMemoryStreamManager recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();

            //
            int tmpiRet = -1;
            bool bCryptInited = false;

            CTX_thread_io ctx;
            //mymemset((IntPtr)&ctx, 0, sizeof(ctx));
            ctx = new CTX_thread_io();
            //
            string where_showInfo; where_showInfo = qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType) + "_R";

            //traceLogA("mcThreadProc_mis_io enters");


            using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
            {
                using (CQyMalloc mallocObj = new CQyMalloc())
                {
                    using (CQyMalloc mallocObj_buf_help = new CQyMalloc())
                    {


                        //MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                        CMIS_MSGU pMsgBuf = new CMIS_MSGU();// (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                        if (null == pMsgBuf) return;
                        if (null == (pMsg = (MIS_MSG_INPUT*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSG_INPUT)))) goto errLabel;
                        buf_help.len = (uint)sizeof(MIS_MSG_routeTalkData);
                        if (null == (buf_help.data = (byte*)mallocObj_buf_help.mallocf(Consts.CONST_iAllocType_longTimeMsg, (int)buf_help.len))) goto errLabel;

                        if (pChannel.bSessionOpen)
                        {
                            goto errLabel;
                        }



                        //  ÏÈ½¨Á¢¼ÓÃÜ»·¾³¡£2008/06/09

                        //memset(&pChannel.commEncCtx, 0, sizeof(pChannel.commEncCtx)  );
                        pChannel.commEncCtx = new ENC_CTX_session();
                        //memcpy(&pChannel->commEncCtx.common, &pMisCnt->commEncCtx.common, sizeof(pChannel->commEncCtx.common)  );
                        pChannel.commEncCtx.common = pMisCnt.commEncCtx.common;
                        if (0 != initCrypt_qmc(pProcInfo, pMisCnt, Consts.CONST_cspContainer_qmc, Consts.DEFAULT_cspProvider, (pChannel.uiType == Consts.CONST_channelType_talking), ref pChannel.commEncCtx)) goto errLabel;
                        bCryptInited = true;
                        //
                        //_sntprintf(pChannel->commEncCtx.mutexName_syncCrypt, mycountof(pChannel->commEncCtx.mutexName_syncCrypt), _T("qmcCrypt%u"), getuiNextTranNo(0, 0, 0));
                        {
                            int tt = 0;
                            string str = string.Format(("qmcCrypt{0}"), getuiNextTranNo(null, 0, ref tt));
                            fixed (char* pName = pChannel.commEncCtx.m_var.mutexName_syncCrypt)
                            {
                                mytcsncpy(pName, str, Consts.cntof_mutexName);
                            }
                        }


                        //  
                        for (i = 0; i < Consts.CONST_maxTriesToStartSession_mis; i++)
                        {
                            RESULT_startChannel result;
                            if (0 == tryToStartChannel(pMisCnt, pChannel, ref recyclableMemoryStreamManager,ref result)) break;
                        }
                        if (i == Consts.CONST_maxTriesToStartSession_mis)
                        {
                            qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("can't start session"));
                            goto errLabel;
                        }
                        pChannel.status.ulbSessionErr = false;
                        pChannel.status.dwTickCnt_lastStartToRecv = pChannel.status.dwTickCnt_lastEndSending = myGetTickCount(null);        //  2009/02/25


                        //
                        bChkMessengerChannelsReady(pQyMc);

                        //
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("auth OK"));

                        //

                        //  
                        /*
                        ctx.hThread_toSend = CreateThread(NULL, 0, mcThreadProc_mis_toSend, pChannel, CREATE_SUSPENDED, &dwThreadDaemonId);
                        if (!ctx.hThread_toSend) goto errLabel;
                        ctx.dwThreadId_toSend = dwThreadDaemonId;
                        if (ResumeThread(ctx.hThread_toSend) == -1) goto errLabel;
                        */
                        //
                        ctx.hThread_toSend = new Thread(new ParameterizedThreadStart(mcThreadProc_mis_toSend));
                        ctx.hThread_toSend.Start(pChannel);

                        //
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("Function toSend is ready"));

                        //  Æô¶¯·¢ËÍÏß³Ì
                        /*
						ctx.hThread_snd = CreateThread(NULL, 0, mcThreadProc_mis_snd, pChannel, CREATE_SUSPENDED, &dwThreadDaemonId);
						if (!ctx.hThread_snd) goto errLabel;
						ctx.dwThreadId_snd = dwThreadDaemonId;
						if (ResumeThread(ctx.hThread_snd) == -1)
						{
							traceLogA("ResumeThread snd failed"); goto errLabel;
						}
						*/
                        ctx.hThread_snd = new Thread(new ParameterizedThreadStart(mcThreadProc_mis_snd));
                        ctx.hThread_snd.Start(pChannel);
                        //
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("Function send is ready"));


                        //  pChannel->status.nMsgRecved  =  0;



                        for (; bChannelOk(pChannel);)
                        {   //  Õâ¸öÑ­»·ÊÇ°´ÒªÇó¶ÁÒ»¸öÏûÏ¢

                            iRet = GuiShare.pf_qyWaitForRead(ref pChannel.sock, 2500);
                            if (iRet < 0) goto errLabel;
                            if (0 == iRet) continue;

                            //  2009/02/25
                            pChannel.status.dwTickCnt_lastStartToRecv = myGetTickCount(null);

                            MACRO_memset_misMsgInput(pMsg);
                            //  ÕâÀïÊÇÕæÓÐÊý¾ÝÀ´ÁË
                            if (0 != qyRecvMsg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to, &msgHead, (byte*)&pMsg->data, (uint)sizeof(MIS_MSG_routeTalkData)))
                            {
                                Console.WriteLine("mcThreadProc_mis_io: qyRecvMsg(  ) failed");
                                goto errLabel;
                            }

                            pMsg->tStartTime = msgHead.i64StartTime;
                            pMsg->uiTranNo = msgHead.uiTranNo;
                            pMsg->usSeqNo = msgHead.usSeqNo;
                            pMsg->ucFlg = msgHead.ucFlg;
                            pMsg->usCode = msgHead.usCode;
                            pMsg->uiChannelType = pChannel.uiType;     //  2008/04/17
                            pMsg->lenInBytes = msgHead.uiLen;

                            /*
			if (pQyMc.env.bDebugInfo && pProcInfo.cfg.debugStatusInfo.ucbViewIoStatus)
			{
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, where_showInfo, _T(""), _T("recv: len %d, tn %d, sn %d"), pMsg->lenInBytes, pMsg->uiTranNo, pMsg->usSeqNo);
			}
							*/

                            if (isUcFlgMoreData(pMsg->ucFlg))
                            {
                                if (0 != procMisMsg_moreData(pChannel.inCacheQ, pMsg, null)) goto errLabel;
                                continue;
                            }
                            if (pMsg->usSeqNo != 0)
                            {
                                bool bMsgSkipped = false;       //  ÊÇ·ñ´Ë°üÐèÒªºöÂÔ?
                                                                //
                                if (0 != procMisMsg_moreData(pChannel.inCacheQ, pMsg, &bMsgSkipped)) goto errLabel;
                                if (bMsgSkipped) continue;
                                //
                                //  traceLogA(  "µÃµ½Ò»¸ö´ó°ü£¬³¤¶ÈÎª%d, tStartTime is %d, uiTranNo is %d",  pMsg->lenInBytes,  pMsg->tStartTime,  pMsg->uiTranNo  );
                            }

                            //  traceLogA(  "mcThreadProc_mis_io: get, ³¤¶È %d, tStartTime is %d, uiTranNo is %d",  pMsg->lenInBytes,  pMsg->tStartTime,  pMsg->uiTranNo  );
                            pChannel.status.netStat.ui64MsgDataLenRecvd += (ulong)sizeof(QY_MSG_HEAD) + pMsg->lenInBytes;     //  2009/10/10

                            if (0 != decryptData(ref pChannel.commEncCtx, ref buf_help, ref recyclableMemoryStreamManager,  (byte*)&pMsg->data, &pMsg->lenInBytes))
                            {
                                qyShowInfo1(Consts.CONST_qyShowType_warning, pProcInfo.str_who_showInfo, null, where_showInfo, ("io: decryptData failed."));
                                goto errLabel;
                            }

                            //  pChannel->status.netStat.nMsgRecvd  ++  ;
#if DEBUG
                            ref MIS_MSG_INPUT tmp_pMsgInput = ref *pMsg;
                            if  (  tmp_pMsgInput.usCode  == Consts.CONST_qyCmd_sendVDevReq  )
                            {
                                int ii = 0;
                            } 
#endif

                            /*
                # ifdef  __DEBUG__  
                            static int kk = 0; kk++;
                            //  traceLogA(  "get code %S,  msgHead.usLen %d,  kk  =  %d",  qyGetDesByType1(  CONST_qyCmdTable,  msgHead.usCode  ),  msgHead.usLen,  kk  );
                            if (0)
                            {
                                //
                                if (isUcFlgResp(msgHead.ucFlg)) qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, where_showInfo, _T(""), _T("  tn %d, ¶Áµ½ÏìÓ¦ %s, dataLen %d"), msgHead.uiTranNo, qyGetDesByType1(CONST_qyRcTable, msgHead.usCode), pMsg->lenInBytes);
                                else qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, where_showInfo, _T(""), _T(" tn %d, ¶Áµ½ÇëÇó %s, dataLen %d"), msgHead.uiTranNo, qyGetDesByType1(CONST_qyCmdTable, msgHead.usCode), pMsg->lenInBytes);
                            }
                #endif
                                                                */

                            MSG_ROUTE msgRoute = new MSG_ROUTE();
                            if (0 != (tmpiRet = pProcInfo.postMsg2Mgr_mc(ctx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_input, msgHead.ucFlg, msgHead.usCode, msgHead.i64StartTime, msgHead.uiTranNo, 0, (byte*)&pMsg->data, pMsg->lenInBytes, 0, 0, pChannel.uiType, pMsgBuf, false)))
                            {
                                if (tmpiRet == Consts.CONST_qyRet_qFull)
                                {
                                    switch (pChannel.uiType)
                                    {
                                        case Consts.CONST_channelType_media:
                                        case Consts.CONST_channelType_realTimeMedia:
                                            qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("postMsg2Mgr_mc returns qFull, skipped."));
                                            continue;
                                        default:
                                            break;
                                    }
                                }
                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("postMsg2Mgr_mc failed."));
                                goto errLabel;
                            }


                        }


                    }
                }
            }

            iErr = 0;

        errLabel:

            //if  (  bThreadIdFound  )  
            {

                pChannel.status.ulbSessionErr = true;  //  ´Ë±êÖ¾Î»Í¨ÖªrecvºÍsndÁ½Ïß³ÌÍË³ö

                MACRO_qyNullThread(ref ctx.hThread_toSend);
                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("Function toSend is closed"));
                MACRO_qyNullThread(ref ctx.hThread_snd);
                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("Function send is closed"));

                if (pChannel.bSessionOpen)
                {
                    //traceLogA("to close session");
                    qnmCloseSession_msg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to);
                    pChannel.bSessionOpen = false;
                    //traceLogA("session closed");

                    qyShowInfo1(Consts.CONST_qyShowType_qwmComm, pProcInfo.str_who_showInfo, null, where_showInfo, ("Session is closed"));

                }
                if (bCryptInited) exitCrypt_qmc(pProcInfo, pMisCnt, ref pChannel.commEncCtx);

                pChannel.status.ulbIoQuit = true;
            }



            //traceLogA("mcThreadProc_mis_io leaves");

            return;
        }



    }
}
