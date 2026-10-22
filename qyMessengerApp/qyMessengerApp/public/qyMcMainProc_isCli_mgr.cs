using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

partial class Consts
{
    public const int CONST_nIntervalInMs_tryToResumeVideoTask = 200;		//  5000		//  2014/10/13

}


namespace qy
{
    partial class qyFuncs
    {

        //
        public static unsafe int procMsgInput_mc_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSG_INPUT pMsg, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel,string hint)
        {
            int iErr = -1;
            //



            //
            int dataLen = 0;
            //uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            //IM_CONTENTU* pContent = (IM_CONTENTU*)pMsg->data.buf;
            //  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;

            //  IM_TASK_RCD			rcd;
            int tmpiRet;
            //
            int lenInBytes = 0;
            IM_SIMPLE_RESP respSimple;
            ushort usRespCode = Consts.CONST_qyRc_unknown;
            uint uiChannelType = Consts.CONST_channelType_talking;
            //
            TMP_timeBuf timeBuf;
            //char timeBuf[Consts.CONST_qyTimeLen + 1] = "";
            //
            bool bRemoveIfErr = true;       //  ÔÚÔËÐÐ³ö´íÊ±ÊÇ·ñÉ¾³ýÏà¹Ø½Úµã¡£
                                            //
                                            //  TCHAR					sqlBuf[Consts.CONST_maxSqlBufLen  +  1]	=	(  ""  );
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            //  2015/09/06
#if __DEBUG__
#endif



            //
            if (null == pQyMc.gui.hMainWnd) return -1;
            if (null == pMisCnt) return -1;

            //  memset(  &rcd,  0, sizeof(  rcd  )  );

            //  2014/11/22
#if DEBUG
            //
            qyFuncs.traceLog("procMsgInput_mc_mgr enters");

#endif





            if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
            {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                {
                    switch (pMsg.uiTaskType)
                    {
                        case Consts.CONST_imTaskType_shareDynBmp:
                        case Consts.CONST_imTaskType_shareGps:
                            break;
                        default:
                            {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: getIdInfo_logicalPeer failed.");
#endif
                                uint step = 0;
                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), ("procMsgTask_mc_mgr: getIdInfo_logicalPeer failed."));
                                goto errLabel;
                            }
                            break;
                    }
                }
            }






            if (!isUcFlgResp(pMsg.ucFlg))
            {

                //
                switch (pMsg.usCode)
                {
                    case Consts.CONST_qyCmd_sendMedia:
                        {       //  
                                //
                            if (0 != procMsgInput_mc_media_mgr(pQyMc, pProcInfo, ref pMsg, lenInBytes_msg, bMsgInput, ref pCtx, pChannel, "procMsgInput_mc_mgr.114"))
                            {
                                goto errLabel;
                            }

                            //
                            iErr = 0; goto errLabel;
                        }
                        break;

                    case Consts.CONST_qyCmd_sendRobotTask:
                    case Consts.CONST_qyCmd_sendRobotTaskReply:
                    case Consts.CONST_qyCmd_sendRobotTaskData:
                        {
                            //
                            fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                            {
                                if (0 != procMsgInput_mc_robot_mgr(pQyMc, pProcInfo, pMsg0, lenInBytes_msg, bMsgInput, ref pCtx, pChannel))
                                {
                                    goto errLabel;
                                }
                            }
                            //
                            iErr = 0; goto errLabel;

                        }
                        break;

                    default:
                        break;
                }





                //  2015/09/07
                /*
				if (!bMsgInput)
				{

					if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, pMsg, lenInBytes_msg, pChannel, ("thread_mgr")))
					{
						goto errLabel;
					}

					iErr = 0; goto errLabel;

				}
				*/


#if __DEBUG__
			traceLogA("procMsgTask_mc_mgr failed:  kkkkkkkkk");
#endif
                goto errLabel;


            }
            else
            {
                /*
				if (!bMsgInput)
				{

					if (procMsgTask2Output_mc(pProcInfo, pMisCnt, pMsg, lenInBytes_msg, pChannel, ("thread_mgr")))
					{
						goto errLabel;
					}

					iErr = 0; goto errLabel;
				}
				*/

                //
                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

#if __DEBUG__
			qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, null, (""), (""), ("%I64u¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(Consts.CONST_qyRcTable, pMsg->usCode));
#endif

                if (!isRcOk(pMsg.usCode)) goto errLabel;

#if __DEBUG__
			traceLogA("procMsgTask_mc_media:  ");
#endif

                //
            }



            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

#if DEBUG
            qyFuncs.traceLog("procMsgInput_mc_mgr leaves " + iErr  +  " "  +  hint);
#endif

            return iErr;
        }


        //public static unsafe int procMsgTask_mc_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_MSG_TASK* pMsg, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        public static int procMsgTask_mc_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU  pMsgU, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;
            //
            if  (  pMsgU.uiType!=Consts.CONST_misMsgType_task)
            {
                return -1;
            }
            ref MIS_MSG_TASK pMsg = ref pMsgU.task;


            //
            int dataLen = 0;
            //uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            //IM_CONTENTU* pContent = (IM_CONTENTU*)pMsg->data.buf;
            //  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;

            //  IM_TASK_RCD			rcd;
            int tmpiRet;
            //
            int lenInBytes = 0;
            IM_SIMPLE_RESP respSimple;
            ushort usRespCode = Consts.CONST_qyRc_unknown;
            uint uiChannelType = Consts.CONST_channelType_talking;
            //
            TMP_timeBuf timeBuf;
            //char timeBuf[Consts.CONST_qyTimeLen + 1] = "";
            //
            bool bRemoveIfErr = true;       //  ÔÚÔËÐÐ³ö´íÊ±ÊÇ·ñÉ¾³ýÏà¹Ø½Úµã¡£
                                            //
                                            //  TCHAR					sqlBuf[Consts.CONST_maxSqlBufLen  +  1]	=	(  ""  );
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            //  2015/09/06
#if __DEBUG__
#endif



            //
            if (null == pQyMc.gui.hMainWnd) return -1;
            if (null == pMisCnt) return -1;

            //  memset(  &rcd,  0, sizeof(  rcd  )  );

            //  2014/11/22
#if DEBUG
            //
            qyFuncs.traceLog("procMsgTask_mc_mgr enters");

#endif



            if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
            {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                {
                    switch (pMsg.uiTaskType)
                    {
                        case Consts.CONST_imTaskType_shareDynBmp:
                        case Consts.CONST_imTaskType_shareGps:
                            break;
                        default:
                            {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: getIdInfo_logicalPeer failed.");
#endif
                                uint step = 0;
                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), ("procMsgTask_mc_mgr: getIdInfo_logicalPeer failed."));
                                goto errLabel;
                            }
                            break;
                    }
                }
            }


#if DEBUG
            //
            qyFuncs.traceLog("procMsgTask_mc_mgr.298. isFlgResp "  +  isUcFlgResp(pMsg.ucFlg));

#endif



            if (!isUcFlgResp(pMsg.ucFlg))
            {

                //
                switch (pMsg.usCode)
                {
                    case Consts.CONST_qyCmd_sendMedia:
                        {
                            //
#if DEBUG
                            qyFuncs.traceLog("procMsgTask_mc_mgr.314");
#endif


                            //
                            if (0 != procMsgTask_mc_media_mgr(pQyMc, pProcInfo, ref pMsgU, lenInBytes_msg, bMsgInput, ref pCtx, pChannel))
                            {
                                goto errLabel;
                            }

                            //
                            iErr = 0; goto errLabel;
                        }
                        break;

                    case Consts.CONST_qyCmd_sendRobotTask:
                    case Consts.CONST_qyCmd_sendRobotTaskReply:
                    case Consts.CONST_qyCmd_sendRobotTaskData:
                        {
                            //
                            if (0 != procMsgTask_mc_robot_mgr(pQyMc, pProcInfo, ref pMsgU, lenInBytes_msg, bMsgInput, ref pCtx, pChannel))
                            {
                                goto errLabel;
                            }
                            //
                            iErr = 0; goto errLabel;

                        }
                        break;

                    default:
                        break;
                }





                //  2015/09/07
                if (!bMsgInput)
                {

                    if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsg, lenInBytes_msg, pChannel, ("thread_mgr")))
                    {
                        goto errLabel;
                    }

                    iErr = 0; goto errLabel;

                }


#if __DEBUG__
			traceLogA("procMsgTask_mc_mgr failed:  kkkkkkkkk");
#endif
                goto errLabel;


            }
            else
            {

                if (!bMsgInput)
                {

                    if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsg, lenInBytes_msg, pChannel, ("thread_mgr")))
                    {
                        goto errLabel;
                    }

                    iErr = 0; goto errLabel;
                }

                //
                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

#if __DEBUG__
			qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%I64u¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(Consts.CONST_qyRcTable, pMsg->usCode));
#endif

                if (!isRcOk(pMsg.usCode)) goto errLabel;

#if __DEBUG__
			traceLogA("procMsgTask_mc_media:  ");
#endif

                //
            }



            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }
#if DEBUG
            qyFuncs.traceLog("procMsgTask_mc_mgr leaves "  +  iErr);
#endif

            return iErr;
        }


        //
        public static unsafe int procMsgInput_mc_media_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSG_INPUT pMsg, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel, string hint)
        {
            int iErr = -1;
            //
            int dataLen = 0;
            //uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            ref IM_CONTENTU pContent = ref pMsg.data.buf;
            //  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;

            //  IM_TASK_RCD			rcd;
            int tmpiRet;
            //
            int lenInBytes = 0;
            IM_SIMPLE_RESP respSimple;
            ushort usRespCode = Consts.CONST_qyRc_unknown;
            uint uiChannelType = Consts.CONST_channelType_talking;
            //char timeBuf[Consts.CONST_qyTimeLen + 1] = "";
            TMP_timeBuf timeBuf;
            //
            bool bRemoveIfErr = true;       //  ÔÚÔËÐÐ³ö´íÊ±ÊÇ·ñÉ¾³ýÏà¹Ø½Úµã¡£
                                            //
                                            //  TCHAR					sqlBuf[Consts.CONST_maxSqlBufLen  +  1]	=	(  ""  );
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            //  2015/09/06
#if DEBUG
            qyFuncs.traceLog("procMsgInput_mc_media_mgr enters");
#endif



            //
            if (null == pQyMc.gui.hMainWnd) return -1;
            if (null == pMisCnt) return -1;

            //  memset(  &rcd,  0, sizeof(  rcd  )  );

            //  2014/11/22
#if __DEBUG__
			if (pContent->uiType == Consts.CONST_imCommType_transferAvInfo)
			{
				int i;
				i = 0;

			}
#endif


            if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
            {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                {
                    switch (pMsg.uiTaskType)
                    {
                        case Consts.CONST_imTaskType_shareDynBmp:
                        case Consts.CONST_imTaskType_shareGps:
                            break;
                        default:
                            {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: getIdInfo_logicalPeer failed.");
#endif
                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("procMsgTask_mc_mgr: getIdInfo_logicalPeer failed."));
                                goto errLabel;
                            }
                            break;
                    }
                }
            }




            //
            if (!isUcFlgResp(pMsg.ucFlg))
            {

                /*
				if (!bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendMedia)
				{       //  Ö÷¶¯·¢³ö

					switch (pContent->uiType)
					{

						case Consts.CONST_imCommType_transferAvInfo:
							if (pContent->transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
							{
#if __DEBUG__
								MACRO_qyAssert(0, ("transferAvInfo.subtype err, not task"));
#endif
								goto errLabel;
							}
							TRANSFER_av_task* pTransferAvTask; pTransferAvTask = (TRANSFER_av_task*)pContent;

							//
							if (null==pCtx.pProcessQ) goto errLabel;

							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo qTraverse_mono failed;");
#endif
								goto errLabel;
							}
							if (0==tmpiRet)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, to qPostMsg");
#endif
								if (0!=pCtx.pProcessQ.qPostMsg((IntPtr)pMsg, lenInBytes_msg))
								{
									traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
									goto errLabel;
								}
								//
								//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

								//
								if (pTransferAvTask->myTask.ucbVideoConferenceStarter)
								{
									//  ÔÙ²éÑ¯Ò»´ÎprocessQ_media, ÒòÎªÊÓÆµ»áÒéµÄbmpInfo_compressµÈ£¬ÊÇÔÚÈë¶ÓÁÐÊ±²ÅÉú³ÉµÄ¡£Òª´ÓÀïÃæÈ¡³öÀ´
									if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0) <= 0) goto errLabel;
								}

#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, qPostMsg ok;");
#endif
							}
							//  							

							//  2008/03/21, 
							//startChannel(  pProcInfo,  pMisCnt,  pChannel,  (  "send transferAvInfo"  )  );

							break;
						case Consts.CONST_imCommType_transferAvReplyInfo:

							if (null==pCtx.pProcessQ) goto errLabel;

							//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
							if (pContent->transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
							{
							}
							else
							{
								//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
								tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0);
								if (tmpiRet < 0)
								{
#if __DEBUG__
									traceLogA("procMsgTask_mc_media: transferAvReplyInfo qTraverse_mono failed.");
#endif
									goto errLabel;
								}
								if (0==tmpiRet)
								{
									if (pMsg->iTaskId)
									{       //  2009/09/10
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: transferAvReplyInfo to qPostMsg;");
#endif
										//
										if (0!=pCtx.pProcessQ.qPostMsg((IntPtr)pMsg, lenInBytes_msg))
										{
#if __DEBUG__
											traceLogA("procMsgTask_mc_robot: transferAvReplyInfo, qPostMsg failed");
#endif
											goto errLabel;
										}
										//
										//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: avReply ok;");
#endif

										//
										//  2014/11/20								 
										//  post

									}
								}

								//startChannel(  pProcInfo,  pMisCnt,  pChannel,  (  "send transferAvReplyInfo"  )  );

							}

							break;
						case Consts.CONST_imCommType_transferVideoData:
							{

								bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

								if (null==pCtx.pProcessQ) goto errLabel;

								startChannel(pProcInfo, pMisCnt, pChannel, ("send transferVideoData"));

								if ((tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0)) <= 0)
								{
									//traceLogA("procMsgTask_mc_media: ´¦ÀíConsts.CONST_imCommType_transferVideoData %s¡£", tmpiRet ? "failed" : "not found");
									goto errLabel;
								}

							}
							iErr = 0; goto errLabel;
							break;
						case Consts.CONST_imCommType_transferAudioData:
							{

								bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

								if (null==pCtx.pProcessQ) goto errLabel;

								startChannel(pProcInfo, pMisCnt, pChannel, ("send transferAudioData"));

								if ((tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0)) <= 0)
								{
									//traceLogA("procMsgTask_mc_media: ´¦ÀíConsts.CONST_imCommType_transferAudioData %s¡£", tmpiRet ? "failed" : "not found");
									goto errLabel;
								}

							}
							iErr = 0; goto errLabel;
							break;
						case Consts.CONST_imCommType_taskProcReq:
							switch (pContent->taskProcReq.usOp)
							{
								case Consts.CONST_imOp_send_cancel:
								case Consts.CONST_imOp_recv_cancel:

									if (null==pCtx.pProcessQ) goto errLabel;

									//  printQ_mis(  &pCtx.processQ  );
									//  printQ_mis(  &pCtx.processQ  );

									break;
								default:
									break;
							}

							//  ÇåÀíÒ»ÏÂ.  2008/10/18
							//  chkMsgrs_sendLocalAv(  pProcInfo  );
							//  if  (  bNoMsgrs_sendLocalAv(  pProcInfo,  -1  )  )  {
							//  if  (  pMainWnd  )  PostMessage(  pMainWnd->m_hWnd,  Consts.CONST_qyWm_postComm,  Consts.CONST_qyWmParam_applyForClosingAv_qmc,  0  );    
							//  }

							break;
						//

						case Consts.CONST_imCommType_transferGpsInfo:      //  2012/04/19					

							if (null==pCtx.pProcessQ) goto errLabel;

							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
								traceLogA("procMsgTask_mc_media: transferGpsInfo qTraverse_mono failed;");
								goto errLabel;
							}
							if (0==tmpiRet)
							{
								traceLogA("procMsgTask_mc_media: transferGpsInfo, to qPostMsg");
								if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
								{
									traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
									goto errLabel;
								}
								//
								//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

								//  if  (  pContent->transferAvInfo.myTask.ucbVideoConferenceStarter  )  
								{
									//  
									if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0) <= 0) goto errLabel;
								}

								traceLogA("procMsgTask_mc_media: transferGpsInfo, qPostMsg ok;");
							}
							//  							

							//  2008/03/21, 
							startChannel(pProcInfo, pMisCnt, pChannel, ("send transferGpsInfo"));

							break;

						case Consts.CONST_imCommType_transferGpsReplyInfo:

							if (null==pCtx.pProcessQ) goto errLabel;

							//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
							if (pContent->transferGpsReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
							{
							}
							else
							{
								//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
								tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0);
								if (tmpiRet < 0)
								{
									traceLogA("procMsgTask_mc_media: transferGpsReplyInfo qTraverse_mono failed.");
									goto errLabel;
								}
								if (!tmpiRet)
								{
									if (pMsg->iTaskId)
									{       //  2009/09/10
										traceLogA("procMsgTask_mc_media: transferGpsReplyInfo to qPostMsg;");
										if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
										{
											traceLogA("procMsgTask_mc_robot: transferGpsReplyInfo, qPostMsg failed");
											goto errLabel;
										}
										//
										//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
										traceLogA("procMsgTask_mc_media: gpsReply ok;");
									}
								}

								startChannel(pProcInfo, pMisCnt, pChannel, ("send transferGpsReplyInfo"));
							}

							break;

						case Consts.CONST_imCommType_imGrp:
							{  //  2018/10/27
								int ii = 0;
							}
							break;

						//
						default:
#if __DEBUG__
							traceLogA("procMsgTask_mc_media_mgr failed. unknown pContent->uiType is %d", pContent->uiType);
#endif
							goto errLabel;
							break;
					}

					printMisMsg(0, 0, (MIS_MSGU*)pMsg);

					if (!pMsg->data.route.idInfo_to.ui64Id)
					{
						switch (pMsg->uiTaskType)
						{
							case Consts.CONST_imTaskType_shareDynBmp:
							case Consts.CONST_imTaskType_shareGps:
								//
								iErr = 0; goto errLabel;
								break;
							default:
								break;
						}
						traceLogA("procMsgTask_mc_media failed, idInfo_to is 0");
						goto errLabel;
					}

					//  traceLogA(  "talking"  );
					//if  (  postMsg2Mgr_mc(  pMisCnt,  NULL,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->usSeqNo,  pMsg->data.buf,  pMsg->lenInBytes  -  offsetof(  MIS_MSG_routeTalkData,  buf  ),  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_to,  0,  NULL,  false  )  )  goto  errLabel;
					//  2015/09/06
					if (procMsgTask2Output_mc(pProcInfo, (MIS_CNT*)pMsg->pMisCnt, pMsg, lenInBytes_msg, pChannel, ("thread_mgr"))) goto errLabel;

					//
					iErr = 0; goto errLabel;

				}
				*/


                //  ½ÓÊÕÎÄ¼þµÄÇëÇó°ü, Î´È·ÈÏ£»Ó¦×ª·¢µ½½çÃæÈ¥ÏÔÊ¾
                //
                if (bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {

                    //  2009/10/18
                    pMsg.uiChannelType = pChannel.uiType;

                    //
                    //  ½ÓÊÕÎÄ¼þÊ±£¬ÒªÏÈ¸ø¶Ô·½·¢Ò»¸öokµÄÏìÓ¦£¬ÕâÑù£¬·½±ã¶Ô·½ÖªµÀÍ¨ÐÅÃ»ÎÊÌâ¡£·ñÔò£¬¾ÍÓÖÒªÔÙ·¢Ò»´ÎÁË
                    //
                    switch (pContent.uiType)
                    {
                        case Consts.CONST_imCommType_transferAvInfo:
                            {
                                object pDb = null;
                                CQnmDb db;

                                using (db = new CQnmDb())
                                {

                                    usRespCode = Consts.CONST_qyRc_unknown;
                                    using (CQyMalloc mallocObj_rcd = new CQyMalloc())
                                    {
                                        IM_TASK_RCD* pRcd = (IM_TASK_RCD*)mallocObj_rcd.mallocf(sizeof(IM_TASK_RCD));
                                        if (null == pRcd) goto errLabel;

                                        if (null == pCtx.pProcessQ) goto errLabel;

                                        //
                                        pMsg.uiTaskType = pContent.transferAvInfo.uiTaskType;
                                        pMsg.iStatus = Consts.CONST_imTaskStatus_resp;
                                        qyFuncs.traceLog("##805 " + pMsg.iStatus);

                                        //  2009/09/07
                                        if (pContent.transferAvInfo.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                                        {

                                            fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                                            {
                                                if (0 != qPostMsg((IntPtr)pMsg0, lenInBytes_msg, pQyMc.gui.processQ))
                                                {
#if __DEBUG__
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                    goto errLabel;
                                                }
                                            }
                                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                            iErr = 0; goto errLabel;
                                        }

                                        //  2014/11/21. 这里要把代理的信息更新到起始任务里。
                                        if (0 != pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id && 0 != pContent.transferAvInfo.proxiedTranInfo.uiObjType)
                                        {
                                            if (pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                                            {

                                                PARAM_findOrgMsg param;
                                                mymemset((IntPtr)(byte*)&param, 0, sizeof(PARAM_findOrgMsg));
                                                param.bNeedUpdate = true;

                                                fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                                                {
                                                    tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param);
                                                }

                                                iErr = 0; goto errLabel;
                                            }
                                        }

                                        //
                                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                        pDb = db.m_pDbMem.pDb;

                                        //
                                        //fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                                        {
                                            if (0!=msg2TaskRcd(pMisCnt, ref pMsg, usRespCode, ref *pRcd))
                                            {
#if __DEBUG__
									traceLogA("msg2TaskRcd failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref *pRcd, false, "procMsgInput_mc_media_mgr");
                                        if (tmpiRet < 0)
                                        {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_media(  ): storeTaskInDb failed");
#endif
                                            goto errLabel;
                                        }
                                        //  //  2008/06/04
                                        if (tmpiRet > 0)
                                        {   //  rcd.iStatus
                                            fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                                            {
                                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                                tmpiRet = pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg0, param);
                                            }
                                            if (tmpiRet < 0) goto errLabel;
                                            if (tmpiRet > 0)
                                            {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media. hhh");
#endif
                                                iErr = 0; goto errLabel;
                                            }
                                            //  
                                            if (pRcd->iStatus != pMsg.iStatus && !bTaskAlive(pRcd->iStatus))
                                            {
                                                if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, pMsg.iStatus, pRcd->id)) goto errLabel;
                                            }

                                        }

                                        //  2007/07/01						   
                                        pMsg.iTaskId = pRcd->id;
                                        pMsg.uiTaskMode_unused = pRcd->uiMode;

                                        //
                                        fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                                        {
                                            if (0 != qPostMsg((IntPtr)pMsg0, lenInBytes_msg, pQyMc.gui.processQ))
                                            {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
                                    }
                                }

                                iErr = 0; goto errLabel;

                            }
                            break;
                        case Consts.CONST_imCommType_transferAvReplyInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {

                                    usRespCode = Consts.CONST_qyRc_unknown;

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                    pDb = db.m_pDbMem.pDb;

                                    //  ¶Ô·½²»ÔÊÐí
                                    if (pContent.transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
                                    {
                                        traceLogA("procMsgTask_mc_media, usSp is not accept");
                                        goto errLabel;
                                    }
                                    //
                                    if (pContent.transferAvReplyInfo.usOp == Consts.CONST_imOp_recv_accept)
                                    {
#if false
										QY_MESSENGER_ID idInfo = pMisCnt.idInfo;
										if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, "", &idInfo, (Int64)pContent->transferAvReplyInfo.tStartTime_org, pContent->transferAvReplyInfo.uiTranNo_org, 0, null))
										{
#if __DEBUG__
										traceLogA("procMsgTask_mc_media failed: bGetImTaskRcdBySth returns false");
#endif
											goto errLabel;
										}

										//
										//  printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
										//
										printMisMsg(null, null, (MIS_MSGU*)pMsg);

										//  
										if (pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller,  tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0) <= 0)
										{
											traceLogA("procMsgTask_mc_media failed: ");
											//qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType));
											//
											//  postMsg2Mgr...
											goto errLabel;
										}
#endif
                                        //
                                        //
                                        fixed (MIS_MSG_INPUT* pMsg0 = &pMsg)
                                        {
                                            if (0 != qPostMsg((IntPtr)pMsg0, lenInBytes_msg, pQyMc.gui.processQ))
                                            {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                    }
                                    else
                                    {
                                        //traceLogA("pContent->uiType is %d", pContent->uiType);
                                        goto errLabel;
                                    }

                                    //  2013/02/13
                                    if (0 != pContent.transferAvReplyInfo.uiTranNo_org)
                                    {
                                        if (pContent.transferAvReplyInfo.uiTranNo_org == pProcInfo.status.progress.uiTranNo)
                                        {
                                            notifyProgressEnd(pContent.transferAvReplyInfo.uiTranNo_org, 0);
                                        }
                                    }


                                    /*
									traceLogA(  "·¢ËÍÊÓÆµÊý¾ÝµÄÌõ¼þ¾ß±¸ÁË"  );
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  Consts.CONST_channelType_media  ),  (  "ÒÔ·¢ËÍÊÓÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯media ChannelÊ§°Ü"  );  goto  errLabel;
									}
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  Consts.CONST_channelType_realTimeMedia  ),  (  "ÒÔ·¢ËÍÒôÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯rtMedia ChannelÊ§°Ü"  );  goto  errLabel;
									}
									*/
                                    startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAvReplyInfo"));

                                    //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                                    usRespCode = Consts.CONST_qyRc_ok;

                                    MACRO_makeImSimpleResp(pMsg.usCode, pContent.uiType, 0, out respSimple);

                                    postMsgTask2Mgr_mc(pCtx.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, pChannel.uiType, null, false);

                                }

                                iErr = 0; goto errLabel;

                            }
                            break;
                        /*
					case Consts.CONST_imCommType_transferVideoData:
						{

							bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

							if (!pCtx.pProcessQ) goto errLabel;

							startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferVideoData"));

							//  2009/05/12						   
							pMsg->uiChannelType = pChannel->uiType;

							if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
							{
								getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
								//  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
								qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: %I64u,  tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferVideoData.uiTranNo_openAvDev);
								//
								//  2010/12/27
								//  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt->idInfo.ui64Id  )  
								{
									//
									if (GetTickCount() - pCtx.dwTickCnt_tryToResumeVideoConference > Consts.CONST_nIntervalInMs_tryToResumeVideoTask)
									{
										pCtx.dwTickCnt_tryToResumeVideoConference = GetTickCount();

#if __DEBUG__
										traceLogA("Not finished: procMsgTask_mc_media_mgr: videoData err, shall send a request to retrieve original avInfo");
#endif
										qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("procMsgTask_mc_media_mgr: %I64u, videoData err, will send a request to retrieve original avInfo."), pMsg->data.route.idInfo_from.ui64Id);
										//
										showTaskStatus(pMisCnt, &pMsg->addr_logicalPeer.idInfo, &pMsg->data.route.idInfo_from, NULL, false, pMsg->usCode, 0, pContent->transferVideoData.uiTranNo_openAvDev, pContent->uiType, 0, Consts.CONST_imTaskStatus_err_missingOrgReq);
									}
								}
								//
								//
								goto errLabel;
							}


							iErr = 0; goto errLabel;

						}
						break;

					case Consts.CONST_imCommType_transferAudioData:
						{

							bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

							if (!pCtx.pProcessQ) goto errLabel;

							startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAudioData"));

							//  2009/05/12						   
							pMsg->uiChannelType = pChannel->uiType;

							if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
							{
								getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
								//  traceLogA(  "procMsgTask_mc_media failed: audio data error"  );
								qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: %I64u. tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferAudioData.uiTranNo_openAvDev);
								//
								//  2009/10/17
								//  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt->idInfo.ui64Id  )  
								{
									if (GetTickCount() - pCtx.dwTickCnt_tryToResumeVideoConference > 5000)
									{
										pCtx.dwTickCnt_tryToResumeVideoConference = GetTickCount();
#if __DEBUG__
										traceLogA("Not finished: procMsgTask_mc_media: audioData err, try to find orgReq");
#endif
										qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("procMsgTask_mc_media: audioData err, try to find orgReq"));
										//
										showTaskStatus(pMisCnt, &pMsg->addr_logicalPeer.idInfo, &pMsg->data.route.idInfo_from, NULL, false, pMsg->usCode, 0, pContent->transferAudioData.uiTranNo_openAvDev, pContent->uiType, 0, Consts.CONST_imTaskStatus_err_missingOrgReq);
									}
								}
								//
								//
								goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
							}


							iErr = 0; goto errLabel;

						}
						break;

					case Consts.CONST_imCommType_remoteAssistReq:

						bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

						if (!pCtx.pProcessQ) goto errLabel;

						startChannel(pProcInfo, pMisCnt, pChannel, ("recv remoteAssistReq"));

						if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
						{
							getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
							traceLogA("procMsgTask_mc_media failed: ");
							qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: st [%s], tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), CQyString(timeBuf), pContent->transferVideoData.uiTranNo_openAvDev);
							//
							goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
						}

						iErr = 0; goto errLabel;

						break;

					//  2012/04/20
					case Consts.CONST_imCommType_transferGpsInfo:
						{
							ushort usRespCode = Consts.CONST_qyRc_unknown;

							if (!pCtx.pProcessQ) goto errLabel;

							//
							pMsg->uiTaskType = pContent->transferGpsInfo.uiTaskType;
							pMsg->iStatus = Consts.CONST_imTaskStatus_resp;

							//  2009/09/07
							if (pContent->transferGpsInfo.uiTaskType == Consts.CONST_imTaskType_viewGps)
							{

								if (qPostMsg(pMsg, lenInBytes_msg, &pQyMc->gui.processQ))
								{
									traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
								}
								PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

								iErr = 0; goto errLabel;
							}

							//
#if __DEBUG__
							traceLog(("procMsgTask_mc_media failed, transferGpsInfo."));
#endif

							goto errLabel;

							//
						}
						break;

					case Consts.CONST_imCommType_transferGpsReplyInfo:
						{
							void* pDb = NULL;
							CQnmDb db;

							ushort usRespCode = Consts.CONST_qyRc_unknown;

							if (!pCtx.pProcessQ) goto errLabel;

							if (!db.getAvailableDb(pQyMc->iDsnIndex_mainSys)) goto errLabel;
							pDb = db.m_pDbMem->pDb;

							//  ¶Ô·½²»ÔÊÐí
							if (pContent->transferGpsReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
							{
								traceLogA("procMsgTask_mc_media, usOp is not accept");
								goto errLabel;
							}
							//
							if (pContent->transferGpsReplyInfo.usOp == Consts.CONST_imOp_recv_accept)
							{

								if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc->cfg.db.iDbType, false, pMisCnt->cfg.misServName, &pMisCnt->idInfo, (Int64)pContent->transferGpsReplyInfo.tStartTime_org, pContent->transferGpsReplyInfo.uiTranNo_org, 0, NULL))
								{
									traceLogA("procMsgTask_mc_media failed: ÕâÀïÃ»ÕÒµ½Ïà¹ØµÄÈÎÎñ£¬Ó¦¸ÃÏò¶Ô·½·¢ËÍÒ»¸ö´íÎóµÄÈÎÎñµÄÏìÓ¦°ü");
									goto errLabel;
								}

								//
								//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
								//
								printMisMsg(0, 0, (MIS_MSGU*)pMsg);

								//  
								if (pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
								{
									traceLogA("procMsgTask_mc_media failed: ");
									qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType));
									//
									//  postMsg2Mgr...
									goto errLabel;
								}
							}
							else
							{
								traceLogA("pContent->uiType is %d", pContent->uiType);
								goto errLabel;
							}

							startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferGpsReplyInfo"));


							iErr = 0; goto errLabel;

						}
						break;

					case Consts.CONST_imCommType_transferGpsData:
						{

							bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

							if (!pCtx.pProcessQ) goto errLabel;

							startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferGpsData"));

							//  2009/05/12						   
							pMsg->uiChannelType = pChannel->uiType;

							if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
							{
								getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
								//  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
								qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: %I64u,  tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferGpsData.uiType);
								//
								//
								goto errLabel;
							}

							iErr = 0; goto errLabel;

						}
						break;
						*/

                        default:
#if __DEBUG__
							traceLogA("procMsgTask_mc_media failed, bInput, pContent->uiType is %d", pContent->uiType);
#endif
                            goto errLabel;
                            break;
                    }

                }   //  

                //  2015/




                //  2015/09/07
                if (!bMsgInput)
                {
                    /*

					if (procMsgTask2Output_mc(pProcInfo, pMisCnt, pMsg, lenInBytes_msg, pChannel, ("thread_mgr")))
					{
						goto errLabel;
					}

					iErr = 0; goto errLabel;
					*/
                }


#if __DEBUG__
				traceLogA("procMsgTask_mc_mgr failed:  kkkkkkkkk");
#endif
                goto errLabel;


            }
            else
            {


                //
                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

#if __DEBUG__
				qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%I64u¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(Consts.CONST_qyRcTable, pMsg->usCode));
#endif

                if (!isRcOk(pMsg.usCode)) goto errLabel;

#if __DEBUG__
				traceLogA("procMsgTask_mc_media:  ");
#endif

                //
            }



            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

#if DEBUG
            qyFuncs.traceLog("procMsgInput_mc_media_mgr leaves " + iErr  +  " "  +  hint);
#endif

            return iErr;
        }


        public static unsafe int sizeof_IM_SIMPLE_RESP()
        {
            return sizeof(IM_SIMPLE_RESP);
        }

        public static unsafe int sizeof_IM_TASK_RCD()
        {
            return sizeof(IM_TASK_RCD);
        }

        //
        //public static unsafe int procMsgTask_mc_media_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSG_TASK pMsg, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        public static  int procMsgTask_mc_media_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU pMsgU, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;
            //
            if ( pMsgU.uiType!=Consts.CONST_misMsgType_task)
            {
                return -1;
            }
            ref MIS_MSG_TASK pMsg = ref pMsgU.task;

            //
            int dataLen = 0;
            //uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            ref IM_CONTENTU pContent = ref pMsg.data.buf;
            //  CWnd				*	pMainWnd			=	AfxGetApp(  )->m_pMainWnd;
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;

            //  IM_TASK_RCD			rcd;
            int tmpiRet;
            //
            int lenInBytes = 0;

            //IM_SIMPLE_RESP respSimple;
            CIM_CONTENTU content = null;

            ushort usRespCode = Consts.CONST_qyRc_unknown;
            uint uiChannelType = Consts.CONST_channelType_talking;
            //char timeBuf[Consts.CONST_qyTimeLen + 1] = "";
            TMP_timeBuf timeBuf;
            //
            bool bRemoveIfErr = true;       //  ÔÚÔËÐÐ³ö´íÊ±ÊÇ·ñÉ¾³ýÏà¹Ø½Úµã¡£
                                            //
                                            //  TCHAR					sqlBuf[Consts.CONST_maxSqlBufLen  +  1]	=	(  ""  );
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            //  2015/09/06
#if DEBUG
            qyFuncs.traceLog("procMsgTask_mc_media_mgr enters. content.type "  +  pContent.uiType);
#endif



            //
            if (null == pQyMc.gui.hMainWnd) return -1;
            if (null == pMisCnt) return -1;

            //  memset(  &rcd,  0, sizeof(  rcd  )  );

            //  2014/11/22
#if __DEBUG__
			if (pContent->uiType == Consts.CONST_imCommType_transferAvInfo)
			{
				int i;
				i = 0;

			}
#endif


            if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
            {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                {
                    switch (pMsg.uiTaskType)
                    {
                        case Consts.CONST_imTaskType_shareDynBmp:
                        case Consts.CONST_imTaskType_shareGps:
                            break;
                        default:
                            {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: getIdInfo_logicalPeer failed.");
#endif
                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("procMsgTask_mc_mgr: getIdInfo_logicalPeer failed."));
                                goto errLabel;
                            }
                            break;
                    }
                }
            }



#if true

            //
            if (!isUcFlgResp(pMsg.ucFlg))
            {

                if (!bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {       //  Ö÷¶¯·¢³ö

                    switch (pContent.uiType)
                    {

                        case Consts.CONST_imCommType_transferAvInfo:
                            if (pContent.transferAvInfo.tmp_usSubtype != Consts.CONST_transferAvInfo_subtype_task)
                            {
#if __DEBUG__
								MACRO_qyAssert(0, ("transferAvInfo.subtype err, not task"));
#endif
                                goto errLabel;
                            }
                            //TRANSFER_AV_task* pTransferAvTask; pTransferAvTask = (TRANSFER_AV_task*)pContent;

                            //
                            if (null == pCtx.pProcessQ) goto errLabel;

                            //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                            {
                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                tmpiRet = pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param);
                                if (tmpiRet <= 0)
                                {
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo qTraverse_mono failed;");
#endif
                                    goto errLabel;
                                }
                            }
                            /*
							if (0==tmpiRet)
							{
#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, to qPostMsg");
#endif
								if (pCtx.pProcessQ.qPostMsg((IntPtr)pMsg, lenInBytes_msg))
								{
									traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
									goto errLabel;
								}
								//
								//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

								//
								if (pTransferAvTask->myTask.ucbVideoConferenceStarter)
								{
									//  ÔÙ²éÑ¯Ò»´ÎprocessQ_media, ÒòÎªÊÓÆµ»áÒéµÄbmpInfo_compressµÈ£¬ÊÇÔÚÈë¶ÓÁÐÊ±²ÅÉú³ÉµÄ¡£Òª´ÓÀïÃæÈ¡³öÀ´
									if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0) goto errLabel;
								}

#if __DEBUG__
								traceLogA("procMsgTask_mc_media: transferAvInfo, qPostMsg ok;");
#endif
							}
							*/
                            //  							

                            //  2008/03/21, 
                            //startChannel(  pProcInfo,  pMisCnt,  pChannel,  (  "send transferAvInfo"  )  );

                            break;
                        case Consts.CONST_imCommType_transferAvReplyInfo:

                            if (null == pCtx.pProcessQ) goto errLabel;

                            //  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
                            if (pContent.transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
                            {
                            }
                            else
                            {
                                //
#if DEBUG
                                qyFuncs.traceLog("procMsgTask_mc_media_mgr.1499, avReplyInfo");
#endif


                                //  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ
                                //
                                //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();
                                    param.hint = "procMsgTask_mc_media_mgr.1499. avReplyInfo";

                                    tmpiRet = pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param);
                                }
                                if (tmpiRet <= 0)
                                {
#if __DEBUG__
									traceLogA("procMsgTask_mc_media: transferAvReplyInfo qTraverse_mono failed.");
#endif
                                    goto errLabel;
                                }
                                /*
								if (0==tmpiRet)
								{
									if (0!=pMsg->iTaskId)
									{       //  2009/09/10
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: transferAvReplyInfo to qPostMsg;");
#endif
										//
										if (0!=pCtx.pProcessQ.qPostMsg((IntPtr)pMsg, lenInBytes_msg))
										{
#if __DEBUG__
											traceLogA("procMsgTask_mc_robot: transferAvReplyInfo, qPostMsg failed");
#endif
											goto errLabel;
										}
										//
										//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
#if __DEBUG__
										traceLogA("procMsgTask_mc_media: avReply ok;");
#endif

										//
										//  2014/11/20								 
										//  post

									}
								}*/

                                //startChannel(  pProcInfo,  pMisCnt,  pChannel,  (  "send transferAvReplyInfo"  )  );

                            }

                            break;
                        case Consts.CONST_imCommType_transferVideoData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("send transferVideoData"));

                                //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                    {
                                        //traceLogA("procMsgTask_mc_media: ´¦ÀíConsts.CONST_imCommType_transferVideoData %s¡£", tmpiRet ? "failed" : "not found");
                                        goto errLabel;
                                    }
                                }

                            }
                            iErr = 0; goto errLabel;
                            break;
                        case Consts.CONST_imCommType_transferAudioData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("send transferAudioData"));

                                //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                    {
                                        //traceLogA("procMsgTask_mc_media: ´¦ÀíConsts.CONST_imCommType_transferAudioData %s¡£", tmpiRet ? "failed" : "not found");
                                        goto errLabel;
                                    }
                                }

                            }
                            iErr = 0; goto errLabel;
                            break;
                        case Consts.CONST_imCommType_taskProcReq:
                            switch (pContent.taskProcReq.usOp)
                            {
                                case Consts.CONST_imOp_send_cancel:
                                case Consts.CONST_imOp_recv_cancel:

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    //  printQ_mis(  &pCtx.processQ  );
                                    //  printQ_mis(  &pCtx.processQ  );

                                    break;
                                default:
                                    break;
                            }

                            //  ÇåÀíÒ»ÏÂ.  2008/10/18
                            //  chkMsgrs_sendLocalAv(  pProcInfo  );
                            //  if  (  bNoMsgrs_sendLocalAv(  pProcInfo,  -1  )  )  {
                            //  if  (  pMainWnd  )  PostMessage(  pMainWnd->m_hWnd,  Consts.CONST_qyWm_postComm,  Consts.CONST_qyWmParam_applyForClosingAv_qmc,  0  );    
                            //  }

                            break;
                        //

                        /*
					case Consts.CONST_imCommType_transferGpsInfo:      //  2012/04/19					

						if (!pCtx.pProcessQ) goto errLabel;

						tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
						if (tmpiRet < 0)
						{
							traceLogA("procMsgTask_mc_media: transferGpsInfo qTraverse_mono failed;");
							goto errLabel;
						}
						if (!tmpiRet)
						{
							traceLogA("procMsgTask_mc_media: transferGpsInfo, to qPostMsg");
							if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
							{
								traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
								goto errLabel;
							}
							//
							//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );

							//  if  (  pContent->transferAvInfo.myTask.ucbVideoConferenceStarter  )  
							{
								//  
								if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0) goto errLabel;
							}

							traceLogA("procMsgTask_mc_media: transferGpsInfo, qPostMsg ok;");
						}
						//  							

						//  2008/03/21, 
						startChannel(pProcInfo, pMisCnt, pChannel, ("send transferGpsInfo"));

						break;

					case Consts.CONST_imCommType_transferGpsReplyInfo:

						if (!pCtx.pProcessQ) goto errLabel;

						//  traceLogA(  "Î´Íê³É£¬Òª×¢Òâ£¬»¹ÐèÒªÓÐ¹Ø±ÕÊÓÆµ»á»°µÄ¹¤×÷¡£Ö÷ÒªÖ¸£¬¶Ô hic µÄ ICClose(  ) µÈ"  );
						if (pContent->transferGpsReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
						{
						}
						else
						{
							//  Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
							tmpiRet = pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
							if (tmpiRet < 0)
							{
								traceLogA("procMsgTask_mc_media: transferGpsReplyInfo qTraverse_mono failed.");
								goto errLabel;
							}
							if (!tmpiRet)
							{
								if (pMsg->iTaskId)
								{       //  2009/09/10
									traceLogA("procMsgTask_mc_media: transferGpsReplyInfo to qPostMsg;");
									if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
									{
										traceLogA("procMsgTask_mc_robot: transferGpsReplyInfo, qPostMsg failed");
										goto errLabel;
									}
									//
									//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
									traceLogA("procMsgTask_mc_media: gpsReply ok;");
								}
							}

							startChannel(pProcInfo, pMisCnt, pChannel, ("send transferGpsReplyInfo"));
						}

						break;
						*/

                        case Consts.CONST_imCommType_imGrp:
                            {  //  2018/10/27
                                int ii = 0;
                            }
                            break;

                        //
                        default:
#if __DEBUG__
							traceLogA("procMsgTask_mc_media_mgr failed. unknown pContent->uiType is %d", pContent->uiType);
#endif
                            goto errLabel;
                            break;
                    }

                    //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                    if (0 == pMsg.data.route.idInfo_to.ui64Id)
                    {
                        switch (pMsg.uiTaskType)
                        {
                            case Consts.CONST_imTaskType_shareDynBmp:
                            case Consts.CONST_imTaskType_shareGps:
                                //
                                iErr = 0; goto errLabel;
                                break;
                            default:
                                break;
                        }
                        traceLogA("procMsgTask_mc_media failed, idInfo_to is 0");
                        goto errLabel;
                    }

                    //  traceLogA(  "talking"  );
                    //if  (  postMsg2Mgr_mc(  pMisCnt,  NULL,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->usSeqNo,  pMsg->data.buf,  pMsg->lenInBytes  -  offsetof(  MIS_MSG_routeTalkData,  buf  ),  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_to,  0,  NULL,  false  )  )  goto  errLabel;
                    //  2015/09/06
                    //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                    {
                        if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsgU.task, lenInBytes_msg, pChannel, ("thread_mgr"))) goto errLabel;
                    }

                    //
                    iErr = 0; goto errLabel;

                }


                //  ½ÓÊÕÎÄ¼þµÄÇëÇó°ü, Î´È·ÈÏ£»Ó¦×ª·¢µ½½çÃæÈ¥ÏÔÊ¾
                //
                if (bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendMedia)
                {

                    //  2009/10/18
                    pMsg.uiChannelType = pChannel.uiType;

                    //
                    //  ½ÓÊÕÎÄ¼þÊ±£¬ÒªÏÈ¸ø¶Ô·½·¢Ò»¸öokµÄÏìÓ¦£¬ÕâÑù£¬·½±ã¶Ô·½ÖªµÀÍ¨ÐÅÃ»ÎÊÌâ¡£·ñÔò£¬¾ÍÓÖÒªÔÙ·¢Ò»´ÎÁË
                    //
                    switch (pContent.uiType)
                    {
                        case Consts.CONST_imCommType_transferAvInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {

                                    usRespCode = Consts.CONST_qyRc_unknown;
                                    using (CQyMalloc mallocObj_rcd = new CQyMalloc())
                                    {
                                        ;
                                        //IM_TASK_RCD* pRcd = (IM_TASK_RCD*)mallocObj_rcd.mallocf(sizeof_IM_TASK_RCD());
                                        CIM_TASK_RCD pRcd = new CIM_TASK_RCD();
                                        if (null == pRcd) goto errLabel;

                                        if (null == pCtx.pProcessQ) goto errLabel;

                                        //
                                        pMsg.uiTaskType = pContent.transferAvInfo.uiTaskType;
                                        pMsg.iStatus = Consts.CONST_imTaskStatus_resp;
                                        qyFuncs.traceLog("##1704");

                                        //  2009/09/07
                                        if (pContent.transferAvInfo.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                                        {

                                            //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                            {
                                                if (0 != tmp_qPost_msgTask(ref pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                                                {
#if __DEBUG__
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                    goto errLabel;
                                                }
                                            }
                                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                            iErr = 0; goto errLabel;
                                        }

                                        //  2014/11/21. 这里要把代理的信息更新到起始任务里。
                                        if (0 != pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id && 0 != pContent.transferAvInfo.proxiedTranInfo.uiObjType)
                                        {
                                            if (pContent.transferAvInfo.proxiedTranInfo.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                                            {

                                                PARAM_findOrgMsg param = new PARAM_findOrgMsg(); ;
                                                param.bNeedUpdate = true;
                                                //
                                                //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                                {
                                                    tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param);
                                                }

                                                iErr = 0; goto errLabel;
                                            }
                                        }

                                        //
                                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                        pDb = db.m_pDbMem.pDb;

                                        //
                                        //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                        {
                                            if (0!=msg2TaskRcd(pMisCnt, ref  pMsg, usRespCode, ref pRcd.imTaskRcd))
                                            {
#if __DEBUG__
									traceLogA("msg2TaskRcd failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref pRcd.imTaskRcd, false, "procMsgTask_mc_media_mgr kk");
                                        if (tmpiRet < 0)
                                        {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_media(  ): storeTaskInDb failed");
#endif
                                            goto errLabel;
                                        }
                                        //  //  2008/06/04
                                        if (tmpiRet > 0)
                                        {   //  rcd.iStatus
                                            //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                            {
                                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                                tmpiRet = pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param);
                                            }
                                            if (tmpiRet < 0) goto errLabel;
                                            if (tmpiRet > 0)
                                            {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media. hhh");
#endif
                                                iErr = 0; goto errLabel;
                                            }
                                            //  
                                            if (pRcd.imTaskRcd.iStatus != pMsg.iStatus && !bTaskAlive(pRcd.imTaskRcd.iStatus))
                                            {
                                                if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, pMsg.iStatus, pRcd.imTaskRcd.id)) goto errLabel;
                                            }

                                        }

                                        //  2007/07/01						   
                                        pMsg.iTaskId = pRcd.imTaskRcd.id;
                                        pMsg.uiTaskMode_unused = pRcd.imTaskRcd.uiMode;

                                        //
                                        //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                        {
                                            if (0 != tmp_qPost_msgTask(ref pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                                            {
#if __DEBUG__
									traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
#endif
                                                goto errLabel;
                                            }
                                        }
                                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                                        iErr = 0; goto errLabel;
                                    }
                                }
                            }
                            break;
                        case Consts.CONST_imCommType_transferAvReplyInfo:
                            {
                                object pDb = null;
                                using (CQnmDb db = new CQnmDb())
                                {
                                    ;

                                    usRespCode = Consts.CONST_qyRc_unknown;

                                    if (null == pCtx.pProcessQ) goto errLabel;

                                    if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                                    pDb = db.m_pDbMem.pDb;

                                    //  ¶Ô·½²»ÔÊÐí
                                    if (pContent.transferAvReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
                                    {
                                        traceLogA("procMsgTask_mc_media, usSp is not accept");
                                        goto errLabel;
                                    }
                                    //
                                    if (pContent.transferAvReplyInfo.usOp == Consts.CONST_imOp_recv_accept)
                                    {
                                        QY_MESSENGER_ID idInfo = pMisCnt.idInfo;
                                        IM_TASK_RCD rcd = new IM_TASK_RCD();
                                        if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, "", idInfo, (Int64)pContent.transferAvReplyInfo.tStartTime_org, pContent.transferAvReplyInfo.uiTranNo_org, 0, ref rcd))
                                        {
#if __DEBUG__
										traceLogA("procMsgTask_mc_media failed: bGetImTaskRcdBySth returns false");
#endif
                                            goto errLabel;
                                        }

                                        //
                                        //  printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
                                        //
                                        //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                                        //  
                                        //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                        {
                                            PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                            if (pCtx.pProcessQ.qTraverse_mono(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param) <= 0)
                                            {
                                                traceLogA("procMsgTask_mc_media failed: ");
                                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType));
                                                //
                                                //  postMsg2Mgr...
                                                goto errLabel;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //traceLogA("pContent->uiType is %d", pContent->uiType);
                                        goto errLabel;
                                    }

                                    //  2013/02/13
                                    if (0 != pContent.transferAvReplyInfo.uiTranNo_org)
                                    {
                                        if (pContent.transferAvReplyInfo.uiTranNo_org == pProcInfo.status.progress.uiTranNo)
                                        {
                                            notifyProgressEnd(pContent.transferAvReplyInfo.uiTranNo_org, 0);
                                        }
                                    }


                                    /*
									traceLogA(  "·¢ËÍÊÓÆµÊý¾ÝµÄÌõ¼þ¾ß±¸ÁË"  );
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  Consts.CONST_channelType_media  ),  (  "ÒÔ·¢ËÍÊÓÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯media ChannelÊ§°Ü"  );  goto  errLabel;
									}
									if  (  startChannel(  pProcInfo,  pMisCnt,  getChannelByType(  pMisCnt,  Consts.CONST_channelType_realTimeMedia  ),  (  "ÒÔ·¢ËÍÒôÆµÊý¾Ý"  )  )  )  {
										traceLogA(  "Æô¶¯rtMedia ChannelÊ§°Ü"  );  goto  errLabel;
									}
									*/
                                    startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAvReplyInfo"));

                                    //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                                    usRespCode = Consts.CONST_qyRc_ok;

                                    content = new CIM_CONTENTU();
                                    MACRO_makeImSimpleResp(pMsg.usCode, pContent.uiType, 0, out content.imContentU.simpleResp);

                                    //postMsgTask2Mgr_mc(pCtx.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, (byte*)&respSimple, (uint)sizeof_IM_SIMPLE_RESP(), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, pChannel.uiType, null, false);
                                    postMsgTask2Mgr_mc(pCtx.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, ref content.imContentU, (uint)sizeof_IM_SIMPLE_RESP(), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, pChannel.uiType, null, false);

                                    iErr = 0; goto errLabel;
                                }
                            }
                            break;
                        case Consts.CONST_imCommType_transferVideoData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferVideoData"));

                                //  2009/05/12						   
                                pMsg.uiChannelType = pChannel.uiType;

                                //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                    {
                                        //getTimelStr(pMsg.tStartTime, timeBuf.timeBuf, Consts.CONST_qyTimeLen + 1);
                                        //  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
                                        //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: %I64u,  tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferVideoData.uiTranNo_openAvDev);
                                        //
                                        //  2010/12/27
                                        //  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt->idInfo.ui64Id  )  
                                        {
                                            //
                                            if (myGetTickCount(null) - pCtx.dwTickCnt_tryToResumeVideoConference > Consts.CONST_nIntervalInMs_tryToResumeVideoTask)
                                            {
                                                pCtx.dwTickCnt_tryToResumeVideoConference = myGetTickCount(null);

#if __DEBUG__
											traceLogA("Not finished: procMsgTask_mc_media_mgr: videoData err, shall send a request to retrieve original avInfo");
#endif
                                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("procMsgTask_mc_media_mgr: %I64u, videoData err, will send a request to retrieve original avInfo."), pMsg->data.route.idInfo_from.ui64Id);
                                                //
                                                showTaskStatus(pMisCnt, pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id, 0, false, pMsg.usCode, 0, pContent.transferVideoData.uiTranNo_openAvDev, pContent.uiType, 0, Consts.CONST_imTaskStatus_err_missingOrgReq);
                                            }
                                        }
                                        //
                                        //
                                        goto errLabel;
                                    }
                                }


                                iErr = 0; goto errLabel;

                            }
                            break;

                        case Consts.CONST_imCommType_transferAudioData:
                            {

                                bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                                if (null == pCtx.pProcessQ) goto errLabel;

                                startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferAudioData"));

                                //  2009/05/12						   
                                pMsg.uiChannelType = pChannel.uiType;

                                //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                    {
                                        getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
                                        //  traceLogA(  "procMsgTask_mc_media failed: audio data error"  );
                                        //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("%s err: %I64u. tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferAudioData.uiTranNo_openAvDev);
                                        //
                                        //  2009/10/17
                                        //  if  (  pMsg->data.route.idInfo_to.ui64Id  !=  pMisCnt->idInfo.ui64Id  )  
                                        {
                                            if (myGetTickCount(null) - pCtx.dwTickCnt_tryToResumeVideoConference > 5000)
                                            {
                                                pCtx.dwTickCnt_tryToResumeVideoConference = myGetTickCount(null);
#if __DEBUG__
											traceLogA("Not finished: procMsgTask_mc_media: audioData err, try to find orgReq");
#endif
                                                //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, (""), (""), ("procMsgTask_mc_media: audioData err, try to find orgReq"));
                                                //
                                                showTaskStatus(pMisCnt, pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id, 0, false, pMsg.usCode, 0, pContent.transferAudioData.uiTranNo_openAvDev, pContent.uiType, 0, Consts.CONST_imTaskStatus_err_missingOrgReq);
                                            }
                                        }
                                        //
                                        //
                                        goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
                                    }
                                }

                                iErr = 0; goto errLabel;

                            }
                            break;

                        case Consts.CONST_imCommType_remoteAssistReq:

                            bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

                            if (null == pCtx.pProcessQ) goto errLabel;

                            startChannel(pProcInfo, pMisCnt, pChannel, ("recv remoteAssistReq"));

                            //fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                            {
                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                if ((tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param)) <= 0)
                                {
                                    getTimelStr(pMsg.tStartTime, ref timeBuf, Consts.CONST_qyTimeLen + 1);
                                    traceLogA("procMsgTask_mc_media failed: ");
                                    //qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, null, (""), (""), ("%s err: st [%s], tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), CQyString(timeBuf), pContent->transferVideoData.uiTranNo_openAvDev);
                                    //
                                    goto errLabel;      //  ÀíÓÉÍ¬ÉÏ
                                }
                            }

                            iErr = 0; goto errLabel;

                            break;

                        //  2012/04/20
                        /*
						case Consts.CONST_imCommType_transferGpsInfo:
							{
								usRespCode = Consts.CONST_qyRc_unknown;

								if (null==pCtx.pProcessQ) goto errLabel;

								//
								pMsg->uiTaskType = pContent->transferGpsInfo.uiTaskType;
								pMsg->iStatus = Consts.CONST_imTaskStatus_resp;

								//  2009/09/07
								if (pContent->transferGpsInfo.uiTaskType == Consts.CONST_imTaskType_viewGps)
								{

									if (qPostMsg(pMsg, lenInBytes_msg, &pQyMc->gui.processQ))
									{
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
									}
									PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

									iErr = 0; goto errLabel;
								}

								//
#if __DEBUG__
								traceLog(("procMsgTask_mc_media failed, transferGpsInfo."));
#endif

								goto errLabel;

								//
							}
							break;

						case Consts.CONST_imCommType_transferGpsReplyInfo:
							{
								void* pDb = null;
								CQnmDb db;

								ushort usRespCode = Consts.CONST_qyRc_unknown;

								if (!pCtx.pProcessQ) goto errLabel;

								if (!db.getAvailableDb(pQyMc->iDsnIndex_mainSys)) goto errLabel;
								pDb = db.m_pDbMem->pDb;

								//  ¶Ô·½²»ÔÊÐí
								if (pContent->transferGpsReplyInfo.usOp != Consts.CONST_imOp_recv_accept)
								{
									traceLogA("procMsgTask_mc_media, usOp is not accept");
									goto errLabel;
								}
								//
								if (pContent->transferGpsReplyInfo.usOp == Consts.CONST_imOp_recv_accept)
								{

									if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc->cfg.db.iDbType, false, pMisCnt->cfg.misServName, &pMisCnt->idInfo, (Int64)pContent->transferGpsReplyInfo.tStartTime_org, pContent->transferGpsReplyInfo.uiTranNo_org, 0, null))
									{
										traceLogA("procMsgTask_mc_media failed: ÕâÀïÃ»ÕÒµ½Ïà¹ØµÄÈÎÎñ£¬Ó¦¸ÃÏò¶Ô·½·¢ËÍÒ»¸ö´íÎóµÄÈÎÎñµÄÏìÓ¦°ü");
										goto errLabel;
									}

									//
									//printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
									//
									printMisMsg(null, null, (MIS_MSGU*)pMsg);

									//  
									if (pCtx.pProcessQ.qTraverse_mono(tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0) <= 0)
									{
										traceLogA("procMsgTask_mc_media failed: ");
										//qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, null, (""), (""), ("%s err: "), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType));
										//
										//  postMsg2Mgr...
										goto errLabel;
									}
								}
								else
								{
									traceLogA("pContent->uiType is %d", pContent->uiType);
									goto errLabel;
								}

								startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferGpsReplyInfo"));


								iErr = 0; goto errLabel;

							}
							break;

						case Consts.CONST_imCommType_transferGpsData:
							{

								bRemoveIfErr = false;           //  2008/05/14, ²»ÄÜÒòÎª³ö´í½«É¾³ý½Úµã¡£ÓÐÊ±Êý¾Ý±ÈavProcInfoÅÜµÄ¿ì£¬²»ÄÜÒò´Ë°Ñ½Úµã¸øÉ¾³ýÁË¡£

								if (!pCtx.pProcessQ) goto errLabel;

								startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferGpsData"));

								//  2009/05/12						   
								pMsg->uiChannelType = pChannel->uiType;

								if (tmpiRet = pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
								{
									getTimelStr(pMsg->tStartTime, timeBuf, mycountof(timeBuf));
									//  traceLogA(  "procMsgTask_mc_media failed: video data error"  );
									qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, null, (""), (""), ("%s err: %I64u,  tn %d, tn_openAvDev [%d]"), qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType), pMsg->data.route.idInfo_from.ui64Id, pMsg->uiTranNo, pContent->transferGpsData.uiType);
									//
									//
									goto errLabel;
								}

								iErr = 0; goto errLabel;

							}
							break;
					*/

                        default:
#if __DEBUG__
							traceLogA("procMsgTask_mc_media failed, bInput, pContent->uiType is %d", pContent->uiType);
#endif
                            goto errLabel;
                            break;
                    }

                }   //  

                //  2015/




                //  2015/09/07
                if (!bMsgInput)
                {

                   // fixed (MIS_MSG_TASK* pMsg0 = &pMsg)
                    {
                        if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsg, lenInBytes_msg, pChannel, ("thread_mgr")))
                        {
                            goto errLabel;
                        }
                    }

                    iErr = 0; goto errLabel;

                }


#if __DEBUG__
				traceLogA("procMsgTask_mc_mgr failed:  kkkkkkkkk");
#endif
                goto errLabel;


            }
            else
            {


                //
                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

#if __DEBUG__
				qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, null, (""), (""), ("%I64u¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(Consts.CONST_qyRcTable, pMsg->usCode));
#endif

                if (!isRcOk(pMsg.usCode)) goto errLabel;

#if __DEBUG__
				traceLogA("procMsgTask_mc_media:  ");
#endif

                //
            }

#endif

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

#if DEBUG
            qyFuncs.traceLog("procMsgTask_mc_media_mgr leaves "  +  iErr);
#endif

            return iErr;
        }


        public static unsafe int procMsgInput_mc_robot_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_MSG_INPUT* pMsg, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;
            //
            int dataLen = 0;
            //uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            uint uiChannelType = Consts.CONST_channelType_talking;
            IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->data.buf;
            object hMainWnd = pQyMc.gui.hMainWnd;
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
            ushort usRespCode;
            string str;


#if true
            using (CQyMalloc mallocObj_rcd = new CQyMalloc())
            {
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
                    //  ×¢Òâ£ºÒÔÏÂ£¬bMsgInputÎªfalseÊ±£¬Îª´ÓÄÚ²¿·¢³ö¡£ÎªTRUEÊ±£¬´ÓÍâ²¿ÊÕÈë£¬ÓÉmgrÖÐ×ª¹ýÀ´
                    //

                    //  Ö÷¶¯·¢³ö
                    //
                    if (!bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendRobotTask)
                    {
#if false

						switch (pContent->uiType)
						{
							case Consts.CONST_imCommType_transferFileReq:
								if (pCtx.pProcessQ.qTraverse(null,tmpHandler_findOrgMsgAndProc_is, (IntPtr)pMsg, (IntPtr)0) <= 0)
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
#if __DEBUG__
									traceLog(("procMsgTask_mc_robot, not input transferFileReq， findQMemByKey failed"));
#endif
										//										
										showInfo_open(0, 0, ("procMsgTask_mc_robot, not input transferFileReq， findQMemByKey failed"));
										//
										goto errLabel;
									}
									pQMemObj = (QMEM_MESSENGER_CLI*)mem.pQMemObj;
									if (!pQMemObj) goto errLabel;
									if (pQMemObj->uiType == Consts.CONST_objType_imGrp) pContent->transferFileReq.bGrpTask = true; //  2007/09/05, ¸³ÉÏ×é±êÊ¶
																																   //
																																   //
									if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
									{
#if __DEBUG__
									traceLog(("procMsgTask_mc_robot, not input transferFileReq， qPostMsg failed"));
#endif
										//
										showInfo_open(0, 0, ("procMsgTask_mc_robot, not input transferFileReq， qPostMsg failed"));
										//
										goto errLabel;
									}
									//  applyToSend¸ÄÎªwaitToSend, 2007/07/18
#endif
									//
									traceLogA("procMsgTask_mc_robot_mgr failed: fileReq not found");
									goto errLabel;


								}

								/*
								traceLogA(  "·"  );
								if  (  !pChannel->hThread_io  )  {
									if  (  startMcIoThread(  pSci,  pMisCnt,  pChannel->uiType,  (  "ÒÔ·¢ËÍÎÄ¼þ"  )  )  )  {
										traceLogA(  "robot Channel"  );  
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
										//  printQ_mis(  &pCtx.processQ  );
										pCtx.pProcessQ.qRemoveMsg(null,tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
										//  printQ_mis(  &pCtx.processQ  );

										break;
									default:
										break;
								}

								break;
							default:
								str=string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent->uiType);
								traceLog(str);
								goto errLabel;
								break;
						}

						//  
						//if  (  postMsg2Mgr_mc(  pMisCnt,  null,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->usSeqNo,  (  char  *  )pMsg->data.buf,  pMsg->lenInBytes  -  offsetof(  MIS_MSG_routeTalkData,  buf  ),  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_to,  0,  null,  false  )  )  goto  errLabel;
						//  2015/09/06
						if (procMsgTask2Output_mc(pProcInfo, pMisCnt, pMsg, lenInBytes_msg, pChannel, ("thread_mgr"))) goto errLabel;
#endif

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
                            ;
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
                            tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref *pRcd, false, "procMsgInput_mc_robot_mgr");
                            if (tmpiRet < 0)
                            {
                                traceLogA("mcThreadProc_mis_work(  ): storeTaskInDb failed");
                                goto errLabel;
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

                            //  ×¢Òâ£¬ÕâÀïÒª½«Êý¾Ý¿âÖÐÏàÓ¦µÄID¸ø¸³ÉÏ£¬2007/07/01
                            pMsg->iTaskId = rcd.id;
                            pMsg->uiTaskMode_unused = rcd.uiMode;


                            traceLogA("");

                        }

                        //
                        if (0 != qPostMsg((IntPtr)pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                        {
                            traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
                        }

                        GuiShare.pf_PostMessage(hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        iErr = 0; goto errLabel;

                    }


                    if (!bMsgInput && pMsg->usCode == Consts.CONST_qyCmd_sendRobotTaskReply)
                    {
#if false
						traceLogA("ÎÄ¼þ½ÓÊÕÇëÇóÒÑ±»È·ÈÏ, ÐÅÏ¢´ÓtalkingÍ¨µÀÖÐ·¢³ö");

						switch (pContent->uiType)
						{
							case Consts.CONST_imCommType_transferFileReplyReq:
								if (pContent->transferFileReplyReq.usOp != Consts.CONST_imOp_recv_accept)
								{
									pCtx.pProcessQ.qRemoveMsg(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);  //  ²»ÔÊÐí´«ËÍ					
								}
								else
								{

									//  ¼ÓÈë¶ÓÁÐ¡£Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
									if (pCtx.pProcessQ.qTraverse(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0) <= 0)
									{
										//
										//  ÒªÊÇÔÚprocessQÀïÃ»ÓÐ£¬ÄÇÃ´¾Í¼ÓÈë¶ÓÁÐ; 
										//
										pMsg->idInfo_taskSender.ui64Id = pMsg->data.route.idInfo_to.ui64Id;
										pMsg->idInfo_taskReceiver.ui64Id = pMsg->data.route.idInfo_from.ui64Id;
										//
										if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
										{
											traceLogA("procMsgTask_mc_robot: 2. ÕâÀï¼Ó²»ÁËÈÎÎñÁË£¬Ó¦¸ÃÍ¨Öª¹ÜÀíÏß³Ì£¬ËµÈÎÎñ¶ÓÁÐÂúÁË¡£²»ÄÜÔÙ¼ÓÁË");
											goto errLabel;
										}
									}

									//  2008/11/18
									startChannel(pProcInfo, pMisCnt, pChannel, ("send transferFileReply"));

								}
								break;
							default:
								traceLogA("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent->uiType);
								goto errLabel;
								break;
						}

						//if  (  postMsg2Mgr_mc(  pMisCnt,  null,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->usSeqNo,  pMsg->data.buf,  pMsg->lenInBytes  -  offsetof(  MIS_MSG_routeTalkData,  buf  ),  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_to,  0,  null,  false  )  )  goto  errLabel;
						//  2015/09/06
						if (procMsgTask2Output_mc(pProcInfo, pMisCnt, pMsg, lenInBytes_msg, pChannel, ("thread_mgr"))) goto errLabel;


						printMisMsg(0, 0, (MIS_MSGU*)pMsg);
#endif
                        //
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
                                //  char					timeBuf[Consts.CONST_qyTimeLen  +  1]  =  "";

                                //  ¶Ô·½²»ÔÊÐí
                                if (pContent->transferFileReplyReq.usOp != Consts.CONST_imOp_recv_accept) goto errLabel;

                                //  
                                if (!g_dbFuncs.pf_bGetImTaskRcdBySth(db.m_pDbMem.pDb, pQyMc.cfg.db.iDbType, false, "", pMisCnt.idInfo, (Int64)pContent->transferFileReplyReq.tStartTime_org, pContent->transferFileReplyReq.uiTranNo_org, 0, ref *pRcd))
                                {
                                    traceLogA("procMsgTask_mc_robot failed: bGetImTaskRcdBySth failed");
                                    goto errLabel;
                                }

                                //
                                //printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
                                //
                                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                                //
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
                                //
                                goto errLabel;
                            }

                        }

                        //
                        startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferFileReply"));

                        //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                        usRespCode = Consts.CONST_qyRc_ok;
                        uiChannelType = 0;// Consts.CONST_channelType_robot;

                        MACRO_makeImSimpleResp(pMsg->usCode, pContent->uiType, 0, out respSimple);

                        //
                        str = "send respSimple to " + pMsg->data.route.idInfo_from.ui64Id;
                        showInfo_open(0, null, str);


                        //
                        postMsgTask2Mgr_mc(null, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg->tStartTime, pMsg->uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->idInfo_taskSender.ui64Id, pMsg->idInfo_taskReceiver.ui64Id, pMsg->data.route.idInfo_from.ui64Id, uiChannelType, null, false);

                        iErr = 0; goto errLabel;

                    }   //  

                    traceLogA("procMsgTask_mc_robot:  Óöµ½ÁËÎÞ·¨½âÊÍµÄÇé¿ö£¬´íÎó"); goto errLabel;


                }
                else
                {

                    //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                    str = string.Format("µÃµ½%I64uµÄÏìÓ¦%s", pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(qyFuncs.CONST_qyRcTable, pMsg->usCode));
                    showInfo_open(0, "", str);

                    if (!isRcOk(pMsg->usCode)) goto errLabel;

                    traceLogA("procMsgTask_mc_robot:  ¶ÔÓÚÏìÓ¦°ü£¬»¹Ã»ÓÐ´¦ÀíÄØ");
                }

            }

#endif

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

            return iErr;
        }

        //
        public static unsafe int procMsgTask_mc_robot_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, ref MIS_MSGU pMsgU, uint lenInBytes_msg, bool bMsgInput, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;

            if  (pMsgU.uiType != Consts.CONST_misMsgType_task)
            {
                return -1;
            }
            ref MIS_MSG_TASK pMsg = ref pMsgU.task;
            //
            int dataLen = 0;
            //uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
            uint uiChannelType = Consts.CONST_channelType_talking;
            ref IM_CONTENTU pContent = ref pMsg.data.buf;
            object hMainWnd = pQyMc.gui.hMainWnd;
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
            ushort usRespCode;
            string str;


#if true
            using (CQyMalloc mallocObj_rcd = new CQyMalloc())
            {
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

                //
                mymemset((IntPtr)pRcd, 0, sizeof(IM_TASK_RCD));


                //
                if (0 == pMsg.addr_logicalPeer.idInfo.ui64Id)
                {   //  Ç°ÃæÃ»ÓÐ¸³ÖµµÄ¶¼ÊÇÊÕµ½µÄÏûÏ¢
                    if (0 != getIdInfo_logicalPeer(pMisCnt, ref pMsg.data.route, ref pMsg.addr_logicalPeer))
                    {
                        traceLogA("procMsgTask_mc_robot: getIdInfo_logicalPeer failed.");
                        goto errLabel;
                    }
                }


                if (!isUcFlgResp(pMsg.ucFlg))
                {

                    //
                    //  ×¢Òâ£ºÒÔÏÂ£¬bMsgInputÎªfalseÊ±£¬Îª´ÓÄÚ²¿·¢³ö¡£ÎªTRUEÊ±£¬´ÓÍâ²¿ÊÕÈë£¬ÓÉmgrÖÐ×ª¹ýÀ´
                    //

                    //  Ö÷¶¯·¢³ö
                    //
                    if (!bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendRobotTask)
                    {

                        switch (pContent.uiType)
                        {
                            case Consts.CONST_imCommType_transferFileReq:
                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param) <= 0)
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
#if __DEBUG__
									traceLog(("procMsgTask_mc_robot, not input transferFileReq， findQMemByKey failed"));
#endif
										//										
										showInfo_open(0, 0, ("procMsgTask_mc_robot, not input transferFileReq， findQMemByKey failed"));
										//
										goto errLabel;
									}
									pQMemObj = (QMEM_MESSENGER_CLI*)mem.pQMemObj;
									if (!pQMemObj) goto errLabel;
									if (pQMemObj->uiType == Consts.CONST_objType_imGrp) pContent->transferFileReq.bGrpTask = true; //  2007/09/05, ¸³ÉÏ×é±êÊ¶
																																   //
																																   //
									if (pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
									{
#if __DEBUG__
									traceLog(("procMsgTask_mc_robot, not input transferFileReq， qPostMsg failed"));
#endif
										//
										showInfo_open(0, 0, ("procMsgTask_mc_robot, not input transferFileReq， qPostMsg failed"));
										//
										goto errLabel;
									}
									//  applyToSend¸ÄÎªwaitToSend, 2007/07/18
#endif
                                    //
                                    showInfo_open(0, null, "procMsgTask_mc_robot_mgr failed: fileReq not found");
                                    goto errLabel;
                                }

                                /*
								traceLogA(  "·"  );
								if  (  !pChannel->hThread_io  )  {
									if  (  startMcIoThread(  pSci,  pMisCnt,  pChannel->uiType,  (  "ÒÔ·¢ËÍÎÄ¼þ"  )  )  )  {
										traceLogA(  "robot Channel"  );  
										goto  errLabel;
									}
								}
								*/

                                break;
                            case Consts.CONST_imCommType_taskProcReq:
                                switch (pContent.taskProcReq.usOp)
                                {
                                    case Consts.CONST_imOp_send_cancel:
                                    case Consts.CONST_imOp_recv_cancel:
                                        //  printQ_mis(  &pCtx.processQ  );
                                        //pCtx.pProcessQ.qRemoveMsg(tmpHandler_findOrgMsgAndProc_is, pMsg, (IntPtr)0);
                                        traceLogA("cannot remove task here");
                                        //  printQ_mis(  &pCtx.processQ  );

                                        break;
                                    default:
                                        break;
                                }

                                break;
                            default:
                                str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent.uiType);
                                traceLog(str);
                                goto errLabel;
                                break;
                        }

                        //  
                        //if  (  postMsg2Mgr_mc(  pMisCnt,  null,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->usSeqNo,  (  char  *  )pMsg->data.buf,  pMsg->lenInBytes  -  offsetof(  MIS_MSG_routeTalkData,  buf  ),  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_to,  0,  null,  false  )  )  goto  errLabel;
                        //  2015/09/06
                        if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsgU.task, lenInBytes_msg, pChannel, ("thread_mgr"))) goto errLabel;


                        iErr = 0; goto errLabel;
                    }


                    //  ½ÓÊÕÎÄ¼þµÄÇëÇó°ü, Î´È·ÈÏ£»Ó¦×ª·¢µ½½çÃæÈ¥ÏÔÊ¾
                    //
                    if (bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendRobotTask)
                    {

                        usRespCode = Consts.CONST_qyRc_unknown;
                        object pDb = null;

                        using (CQnmDb db = new CQnmDb())
                        {
                            ;
                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                            pDb = db.m_pDbMem.pDb;

                            //  2009/10/18
                            pMsg.uiChannelType = pChannel.uiType;

                            //
                            switch (pContent.uiType)
                            {
                                case Consts.CONST_imCommType_transferFileReq:
                                    pMsg.uiTaskType = Consts.CONST_imTaskType_recvFile;
                                    pMsg.iStatus = Consts.CONST_imTaskStatus_applyToRecv;
                                    break;
                                default:
                                    str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent.uiType);
                                    traceLog(str);
                                    goto errLabel;
                                    break;
                            }



                            //  ÕâÀïÒªÈë¿â£¬È»ºóÔÙÍ¨Öª½çÃæ

                            if (0!= msg2TaskRcd(pMisCnt, ref pMsg, usRespCode, ref *pRcd))
                            {
                                traceLogA("msg2TaskRcd failed"); goto errLabel;
                            }
                            tmpiRet = storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, 0, ref *pRcd, false, "");
                            if (tmpiRet < 0)
                            {
                                traceLogA("mcThreadProc_mis_work(  ): storeTaskInDb failed");
                                goto errLabel;
                            }
                            if (tmpiRet > 0)
                            {   //  ÕâÀï±íÃ÷Ô­À´ÒÑ¾­ÓÐ¼ÇÂ¼ÁË, Òª¶Ôrcd.iStatus×ö·ÖÎö£¬Èç¹ûÒÑ¾­È¡ÏûÁË£¬ÄÇÃ´¾ÍÒª·¢ËÍÒ»¸öÒÑÈ¡ÏûµÄÏìÓ¦¸ø¶Ô·½
                                switch (pContent.uiType)
                                {
                                    case Consts.CONST_imCommType_transferFileReq:
                                        if (rcd.iStatus != Consts.CONST_imTaskStatus_applyToRecv && rcd.iStatus != Consts.CONST_imTaskStatus_waitToRecv)
                                        {
                                            IM_SIMPLE_RESP respSimple;
                                            //
                                            MACRO_makeImSimpleResp(pMsg.usCode, pContent.uiType, rcd.iStatus, out respSimple);;
                                            //
                                            postMsgTask2Mgr_mc(null, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, uiChannelType, null, false);

                                            traceLogA("ÕâÀïÊÇËµÃ÷´ËÈÎÎñÒÑ½áÊøÁË£¬²»¹ÜÊ²Ã´½á¹û¡£²é³ö½á¹û·¢»ØÈ¥");

                                            iErr = 0; goto errLabel;

                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            //  ×¢Òâ£¬ÕâÀïÒª½«Êý¾Ý¿âÖÐÏàÓ¦µÄID¸ø¸³ÉÏ£¬2007/07/01
                            pMsg.iTaskId = rcd.id;
                            pMsg.uiTaskMode_unused = rcd.uiMode;
                        }

                        traceLogA("");


                        if (0 != tmp_qPost_msgTask(ref pMsg, lenInBytes_msg, pQyMc.gui.processQ))
                        {
                            traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed"); goto errLabel;
                        }

                        GuiShare.pf_PostMessage(hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

                        iErr = 0; goto errLabel;

                    }


                    if (!bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendRobotTaskReply)
                    {

                        traceLogA("ÎÄ¼þ½ÓÊÕÇëÇóÒÑ±»È·ÈÏ, ÐÅÏ¢´ÓtalkingÍ¨µÀÖÐ·¢³ö");

                        switch (pContent.uiType)
                        {
                            case Consts.CONST_imCommType_transferFileReplyReq:
                                if (pContent.transferFileReplyReq.usOp != Consts.CONST_imOp_recv_accept)
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    pCtx.pProcessQ.qRemoveMsg(null, b_tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param);  //  ²»ÔÊÐí´«ËÍ					
                                }
                                else
                                {
                                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                    //  ¼ÓÈë¶ÓÁÐ¡£Ö»ÓÐ¶ÓÁÐÖÐµÄÏûÏ¢²ÅÊÇ¿ÉÒÔ´«µÝÊý¾ÝµÄ								   
                                    if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param) <= 0)
                                    {
                                        //
                                        //  ÒªÊÇÔÚprocessQÀïÃ»ÓÐ£¬ÄÇÃ´¾Í¼ÓÈë¶ÓÁÐ; 
                                        //
                                        pMsg.idInfo_taskSender.ui64Id = pMsg.data.route.idInfo_to.ui64Id;
                                        pMsg.idInfo_taskReceiver.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                        //
                                        if (0 != pCtx.pProcessQ.qPostMsg(pMsg, lenInBytes_msg))
                                        {
                                            traceLogA("procMsgTask_mc_robot: 2. ÕâÀï¼Ó²»ÁËÈÎÎñÁË£¬Ó¦¸ÃÍ¨Öª¹ÜÀíÏß³Ì£¬ËµÈÎÎñ¶ÓÁÐÂúÁË¡£²»ÄÜÔÙ¼ÓÁË");
                                            goto errLabel;
                                        }
                                    }

                                    //  2008/11/18
                                    startChannel(pProcInfo, pMisCnt, pChannel, ("send transferFileReply"));

                                }
                                break;
                            default:
                                str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent.uiType);
                                traceLog(str);
                                goto errLabel;
                                break;
                        }

                        //if  (  postMsg2Mgr_mc(  pMisCnt,  null,  pMsg->uiType,  pMsg->ucFlg,  pMsg->usCode,  pMsg->tStartTime,  pMsg->uiTranNo,  pMsg->usSeqNo,  pMsg->data.buf,  pMsg->lenInBytes  -  offsetof(  MIS_MSG_routeTalkData,  buf  ),  &pMsg->addr_logicalPeer.idInfo,  &pMsg->data.route.idInfo_to,  0,  null,  false  )  )  goto  errLabel;
                        //  2015/09/06
                        if (0 != procMsgTask2Output_mc(pProcInfo, pMisCnt, ref pMsg, lenInBytes_msg, pChannel, ("thread_mgr"))) goto errLabel;


                        //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                        iErr = 0; goto errLabel;

                    }



                    //  ·¢ËÍÎÄ¼þµÄ»Ø¸´°ü£¬´ÓmgrÖÐÀ´
                    if (bMsgInput && pMsg.usCode == Consts.CONST_qyCmd_sendRobotTaskReply)
                    {
                        IM_SIMPLE_RESP respSimple;

                        using (CQnmDb db = new CQnmDb())
                        {
                            ;
                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;

                            if (pContent.uiType == Consts.CONST_imCommType_transferFileReplyReq)
                            {
                                //  char					timeBuf[Consts.CONST_qyTimeLen  +  1]  =  "";

                                //  ¶Ô·½²»ÔÊÐí
                                if (pContent.transferFileReplyReq.usOp != Consts.CONST_imOp_recv_accept) goto errLabel;

                                //  
                                if (!g_dbFuncs.pf_bGetImTaskRcdBySth(db.m_pDbMem.pDb, pQyMc.cfg.db.iDbType, false, "", pMisCnt.idInfo, (Int64)pContent.transferFileReplyReq.tStartTime_org, pContent.transferFileReplyReq.uiTranNo_org, 0, ref *pRcd))
                                {
                                    traceLogA("procMsgTask_mc_robot failed: bGetImTaskRcdBySth failed");
                                    goto errLabel;
                                }

                                //
                                //printQ_mis(  &pCtx.pProcessQ.m_q,  0  );
                                //
                                //printMisMsg(null, null, (MIS_MSGU*)pMsg);

                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();
                                //  
                                if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref pMsgU, param) <= 0)
                                {
                                    traceLogA("procMsgTask_mc_robot failed: ");
                                    //  postMsg2Mgr...
                                    goto errLabel;
                                }

                            }
                            else
                            {
                                str = string.Format("´íÎóµÄÊý¾Ý¸ñÊ½£ºpContent->uiType is %d", pContent.uiType);
                                traceLog(str);
                                goto errLabel;
                            }
                        }

                        startChannel(pProcInfo, pMisCnt, pChannel, ("recv transferFileReply"));

                        //  ÏÂÃæ×¼±¸·¢ËÍ×¼±¸ºÃµÄÏìÓ¦°ü
                        usRespCode = Consts.CONST_qyRc_ok;
                        uiChannelType = Consts.CONST_channelType_robot;

                        MACRO_makeImSimpleResp(pMsg.usCode, pContent.uiType, 0, out respSimple);

                        postMsgTask2Mgr_mc(null, pMisCnt, Consts.CONST_misMsgType_outputTask, Consts.CONST_commFlg_resp, usRespCode, pMsg.tStartTime, pMsg.uiTranNo, 0, pMsg.iTaskId, pMsg.uiTaskType, (byte*)&respSimple, (uint)sizeof(IM_SIMPLE_RESP), pMsg.addr_logicalPeer.idInfo.ui64Id, pMsg.idInfo_taskSender.ui64Id, pMsg.idInfo_taskReceiver.ui64Id, pMsg.data.route.idInfo_from.ui64Id, uiChannelType, null, false);

                        iErr = 0; goto errLabel;

                    }   //  

                    traceLogA("procMsgTask_mc_robot:  Óöµ½ÁËÎÞ·¨½âÊÍµÄÇé¿ö£¬´íÎó"); goto errLabel;


                }
                else
                {

                    //printMisMsg(null, null, (MIS_MSGU*)pMsg);


                    //qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo.str_who_showInfo, null, (""), (""), ("µÃµ½%I64uµÄÏìÓ¦%s"), pMsg->data.route.idInfo_from.ui64Id, qyGetDesByType1(qyFuncs.CONST_qyRcTable, pMsg->usCode));
                    str = string.Format("µÃµ½%I64uµÄÏìÓ¦%s", pMsg.data.route.idInfo_from.ui64Id, qyGetDesByType1(qyFuncs.CONST_qyRcTable, pMsg.usCode));
                    showInfo_open(0, null, str);

                    if (!isRcOk(pMsg.usCode)) goto errLabel;

                    traceLogA("procMsgTask_mc_robot:  ¶ÔÓÚÏìÓ¦°ü£¬»¹Ã»ÓÐ´¦ÀíÄØ");
                }
            }

#endif

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
            }

            return iErr;
        }

    }
}
