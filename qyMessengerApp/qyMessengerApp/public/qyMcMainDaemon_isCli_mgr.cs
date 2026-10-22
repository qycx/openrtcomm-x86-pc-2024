using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Runtime.InteropServices;



partial class Consts
{

	//
	public const int cntof_tHintBuf = 255 + 1;
	public const int cntof_tBuf128 = 128;
	public const int cntof_tBuf1024 = 1024;

}



//
namespace qy
{

	public unsafe struct TMP_tHintBuf
	{
		public fixed char tHintBuf[Consts.cntof_tHintBuf];
	}
	public unsafe struct TMP_tBuf128
	{
		public fixed char tBuf[Consts.cntof_tBuf128];
	}

	public unsafe struct TMP_tBuf1024
    {
		public fixed char tBuf[Consts.cntof_tBuf1024];
    }

	partial class qyFuncs
{


        //public static unsafe int procMsgReq_mgr(CCtxQmc pProcInfo, MIS_MSG_REQ* pMsgReq, uint lenInBytes_msg, ref CTX_MC_thread pCtx)
        public static unsafe int procMsgReq_mgr(CCtxQmc pProcInfo, MIS_MSGU * pMsgU, uint lenInBytes_msg, ref CTX_MC_thread pCtx)
        {
			int iErr = -1;

			if (pMsgU->uiType != Consts.CONST_misMsgType_req) {
				return -1;
			}
			MIS_MSG_REQ* pMsgReq = &pMsgU->req;


            CCtxQyMc pQyMc = (CCtxQyMc)pProcInfo.pQyMc;
			//QY_SERVICEGUI_INFO* pSci = (QY_SERVICEGUI_INFO*)pProcInfo->pSgiParam;
			int dataLen = 0;
			uint uiMsgLen = (uint)Marshal.OffsetOf<MIS_MSG_REQ>("data");
			//char tBuf[1024] = _T("");
			IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsgReq->data;
			int lenInBytes = 0;



			//  CQyMalloc					mallocObj;
			fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
			{
				MIS_MSG_REQ* pMsg = (MIS_MSG_REQ*)pMsgBuf_forInput;   //  mallocObj.malloc(  sizeof(  MIS_MSG_REQ  )  );
				if (null == pMsg) return -1;
				MIS_CHANNEL pChannel = null;


				//  traceLogA(  "procMsgReq_mgr£º·¢ËÍÖ®Ç°Ó¦¸Ã´¦ÀíÊý¾Ý£¬±ÈÈç£º×ª»»Êý¾Ý×Ö½ÚË³ÐòµÈ: ´¦ÀíÇëÇó %S ",  qyGetDesByType1(  CONST_qyCmdTable,  pMsg->usCode  )  );

				mymemcpy((IntPtr)pMsg, (IntPtr)pMsgReq, (uint)Marshal.OffsetOf<MIS_MSG_REQ>("data"));
				lenInBytes = sizeof(MIS_MSG_routeTalkData);

				switch (pMsgReq->usCode)
				{
					case Consts.CONST_qyCmd_refreshImObjListReq:
						//				   
						if ((dataLen = qyhtonImContentU(0, (IM_CONTENTU*)&pMsgReq->data)) < 0) goto errLabel;
						break;
					/*
				case CONST_qyCmd_refreshRecentFriendsReq:
					switch (pContent->uiType)
					{
						case CONST_imCommType_refreshWebContactsInfo:
							if (qyhtonRouteTalkData(NULL, (char*)&pMsgReq->data, 0, TRUE, pMsgReq->ucFlg, (char*)&pMsg->data, (unsigned  int *  ) & lenInBytes,  NULL,  0  )  )  goto errLabel;
							pMsg->lenInBytes = lenInBytes;
							lenInBytes += offsetof(MIS_MSG_REQ, data);
							//
							dataLen = pMsg->lenInBytes; //  ÕâÊÇÎªÁË¼æÈÝÐÔ¡£µÈ¹ýÒ»Õó£¬½«ÉÏÃæºÍÏÂÃæµÄ¼¯³ÉÆðÀ´¡£2007/12/22
							pMsgReq = pMsg;
							break;
						case CONST_imCommType_refreshRecentFriendsReq:

# ifdef  __DEBUG__
							{
								REFRESH_RECENTFRIENDS_REQ* p = (REFRESH_RECENTFRIENDS_REQ*)&pMsgReq->data;
								int i;
								//
								_sntprintf(tBuf, mycountof(tBuf), _T("uiType is %d, tLast is %I64u, usCnt is %d"), p->uiType, p->tLastRefreshedTime_misServ, p->usCnt);
								for (i = 0; i < p->usCnt; i++)
								{
									_sntprintf(tBuf, mycountof(tBuf), _T("%s,µÚ[%d]¸ö,id %I64u, t %I64u"), tBuf, i, p->mems[i].idInfo.ui64Id, p->mems[i].tLastRefreshedTime_misServ);
								}
								//
								qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T(""));
								qyShowInfo1(CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("tn %d,  %s"), pMsgReq->uiTranNo, tBuf);
								//
								if (p->usCnt) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("usCnt %d, tn %d"), p->usCnt, pMsgReq->uiTranNo);
							}
#endif

							if ((dataLen = qyhtonRefreshRecentFriendsReq(0, (REFRESH_RECENTFRIENDS_REQ*)&pMsgReq->data)) < 0) goto errLabel;
							break;
						default:
							break;
					}
					break;
					*/
					case Consts.CONST_qyCmd_sendVDevReq:
						if (0 != qyhtonRouteTalkData(null, (byte*)&pMsgReq->data, 0, true, pMsgReq->ucFlg, (byte*)&pMsg->data, (uint*)&lenInBytes, null, 0)) goto errLabel;
						pMsg->lenInBytes = (uint)lenInBytes;
						lenInBytes += (int)Marshal.OffsetOf<MIS_MSG_REQ>("data");
						//
						dataLen = (int)pMsg->lenInBytes; //  2007/12/22
						pMsgReq = pMsg;
						break;
					case Consts.CONST_qyCmd_getCfgs:
					default:
						switch (pContent->uiType)
						{
							case 0:
								//  
								break;
							case Consts.CONST_imCommType_getCfgsReq:
							case Consts.CONST_imCommType_retrieveToPaths:
							default:
								if (0 != qyhtonRouteTalkData(null, (byte*)&pMsgReq->data, 0, true, pMsgReq->ucFlg, (byte*)&pMsg->data, (uint*)&lenInBytes, null, 0)) goto errLabel;
								pMsg->lenInBytes = (uint)lenInBytes;
								lenInBytes += (int)Marshal.OffsetOf<MIS_MSG_REQ>("data");
								//
								dataLen = (int)pMsg->lenInBytes; //  2007/12/22
								pMsgReq = pMsg;
								//
								//  traceLogA(  "procMsgReq_mgr:  CONST_qyCmd_getCfgs:  contentType is %d",  pContent->uiType  );
								break;
						}
						break;

				}

				pMsgReq->lenInBytes = (uint)dataLen;
				//
				uiMsgLen += (uint)dataLen;


				if (null == (pChannel = getChannelByType(pProcInfo.pMisCnt, Consts.CONST_channelType_talking))) goto errLabel;

				if (0 != postMsg2OutputQ_mc(pProcInfo.pMisCnt, (MIS_MSGU*)pMsgReq, uiMsgLen, pChannel))
				{
					//  qyShowInfo1(  CONST_qyShowType_debugWarning,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  ""  )  );
					traceLogA("postMsg2OutputQ_mc failed");
					goto errLabel;
				}

			}	

			iErr = 0;

		errLabel:

			if (0!=iErr)
			{
			}

			return iErr;
		}



        //static unsafe int procMsgTalk_mc_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_CNT pMisCnt, MIS_MSG_TALK* pMsgTalk, uint lenInBytes_msg, ref CTX_MC_thread pCtx)
        static unsafe int procMsgTalk_mc_mgr(CCtxQyMc pQyMc, CCtxQmc pProcInfo, MIS_CNT pMisCnt, MIS_MSGU  *  pMsgU, uint lenInBytes_msg, ref CTX_MC_thread pCtx)
        {
			int iErr = -1;

			if  (  pMsgU->uiType!=Consts.CONST_misMsgType_talk)
			{
				return -1;
			}
			MIS_MSG_TALK* pMsgTalk = &pMsgU->talk;


            int dataLen = 0;
			//  unsigned  int		uiMsgLen			=	offsetof(  MIS_MSG_TALK,  data  )  +  offsetof(  MIS_MSG_TALKDATA,  buf  );
			int lenInBytes;
			//  unsigned  int			uiChannelType		=	CONST_channelType_talking;
			if (null==pMisCnt) return -1;
			MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_talking);
			if (null==pChannel) return -1;

			//
			using (CQyMalloc mallocObj = new CQyMalloc()) {
				MIS_MSG_TALK* pMsg = (MIS_MSG_TALK*)mallocObj.mallocf(sizeof(MIS_MSG_TALK));
				if (null==pMsg) return -1;
				IM_CONTENTU* pContent = null;

				traceLogA("procMsgTalk_mc_mgr:  ");

				//  idInfo_to.ui64IdÎª0±íÊ¾Ö±½Ó´Ó±¾»úµÄ¶ÌÐÅÄ£¿é·¢ËÍ
				if (0==pMsgTalk->data.route.idInfo_to.ui64Id)
				{
					switch (pMsgTalk->data.route.uiDevType_to)
					{
						case 0:
							qPostMsgAndTrigger((IntPtr)pMsgTalk, lenInBytes_msg, pProcInfo.workQ);
							break;
						default:
							break;
					}
					//
					iErr = 0; goto errLabel;
				}

				//  ÔÚ·¢Íù·þÎñÆ÷Ê±£¬Ó¦½«idInfo_fromÒ²Éè³É×Ô¼ºµÄÖµ, 2008/02/16
				if (0==pMsgTalk->data.route.idInfo_from.ui64Id)
				{
					pMsgTalk->data.route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
				}


				switch (pMsgTalk->usCode)
				{
					case Consts.CONST_qyCmd_talkTo:
					case Consts.CONST_qyCmd_sendTask:
						break;
					case Consts.CONST_qyCmd_sendTaskReply:
						if (null==(pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_robot))) goto errLabel;
						break;
					default:
						//traceLogA("procMsgTalk_mgr: Î´´¦ÀíµÄmsg, %S", qyGetDesByType1(CONST_qyCmdTable, pMsgTalk->usCode));
						goto errLabel;
				}


				mymemcpy((IntPtr)pMsg, (IntPtr)pMsgTalk, (uint)Marshal.OffsetOf<MIS_MSG_TALK>("data"));
				lenInBytes = sizeof(MIS_MSG_routeTalkData);
				if (0!=qyhtonRouteTalkData(&pMsgTalk->data.route, (byte*)&pMsgTalk->data.buf, 0, true, pMsgTalk->ucFlg, (byte*)&pMsg->data, (uint *  ) & lenInBytes,  null,  0  )  )  goto errLabel;
				pMsg->lenInBytes = (uint)lenInBytes;
				lenInBytes += (int)Marshal.OffsetOf<MIS_MSG_TALK>("data");

				if (0!=postMsg2OutputQ_mc(pMisCnt, (MIS_MSGU*)pMsg, (uint)lenInBytes, pChannel))
				{
					//  qyShowInfo1(  CONST_qyShowType_debugWarning,  0,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "·ÅÈë·¢ËÍ¶ÓÁÐÊ§°Ü"  )  );
					traceLogA("postMsg2OutputQ_mc failed");
					goto errLabel;
				}

			}

			iErr = 0;

		errLabel:

			return iErr;
		}







		static unsafe void mcThreadProc_mis_mgr(object  lpParameter)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			int loopCtrl = 0;
			//DWORD dwThreadId = GetCurrentThreadId();

			MIS_MSGU* pMsg = null;
			//CMIS_MSGU pMsgU = null;

			uint len;
			uint dwRet;
			//  long							lPrev;
			CTX_MC_thread ctx;
			//
			uint uiChannelType = Consts.CONST_channelType_talking;

			//
			pProcInfo.dwThreadId_mgr = myGetCurrentThreadId();

			//
			//mymemset(&ctx, 0, sizeof(ctx));
			ctx = new CTX_MC_thread();
			ctx.pProcessQ = pProcInfo.processQ_media;  //  2015/09/06
			using (CQyMalloc mallocObj_pMsgBuf_forInput = new CQyMalloc()) {
				using (CQyMalloc mallocObj = new CQyMalloc()) {

					ctx.pMsgBuf_forInput = new CMIS_MSGU();// (MIS_MSGU*)mallocObj_pMsgBuf_forInput.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
					if (null == ctx.pMsgBuf_forInput) goto errLabel;


					if (null==(pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;
					//pMsgU = new CMIS_MSGU();
					//ref MIS_MSGU pMsg = ref pMsgU.msgU;

					//  2015/09/10
					//ctx.pProcessQ.m_var.dwThreadId_mgr = GetCurrentThreadId();

					//
					for (; !pProcInfo.bQuit_threadProc_isCli; loopCtrl++)
					{

						/*
						dwRet = WaitForSingleObject(pProcInfo->mgrQ.hEvent, 3500);
						if (dwRet == WAIT_FAILED)
						{
							goto errLabel;
						}
						*/
						myWaitOne(pProcInfo.mgrQ.hEvent, 3500);
						//
						for (; !pProcInfo.bQuit_threadProc_isCli;)
						{
							//fixed (MIS_MSGU* pMsg = &pMsgU.msgU)
							{
								len = (uint)sizeof(MIS_MSGU);

								if (qGetMsg(pProcInfo.mgrQ, (IntPtr)pMsg, &len) != 0) break;
								//mymemset((IntPtr)(((byte *)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));


								//
#if DEBUG
								qyFuncs.traceLog("mcThreadProc_mis_mgr, get msg.type " + pMsg->uiType);
#endif

								//
								switch (pMsg->uiType)
								{
									case Consts.CONST_misMsgType_input:
										{
											MIS_CNT pMisCnt = null;
											MIS_CHANNEL pChannel = null;

											pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;// getMisCntBySth( pProcInfoMsg->input.pMisCnt;
											if (null == pMisCnt)
											{
												break;
											}
											if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) break;

											if (isUcFlgResp(pMsg->input.ucFlg))
											{
												if (0 != procMsgInput_mgr_resp(pQyMc, pProcInfo, pMsg, len, ref ctx, pChannel)) break;
											}
											else
											{
												//
												if  (  pMsg->input.usCode  ==  Consts.CONST_qyCmd_sendVDevReq)
												{
													int iii = 0;
												}
												if (pMsg->input.usCode==Consts.CONST_qyCmd_sendTask)
												{
													int iii = 0;
												}

												//
												if (0 != procMsgInput_mgr_req(pQyMc, pProcInfo, pMsg, len, ref ctx, pChannel)) break;

												//
#if DEBUG
												qyFuncs.traceLog("mcThreadProc_mis_mgr: after procMsgInput_mgr_req");
#endif

											}

											//
										}
										break;


									case Consts.CONST_misMsgType_req:


										if (procMsgReq_mgr(pProcInfo, pMsg, len, ref ctx) != 0) break;

										break;

									case Consts.CONST_misMsgType_talk:


										if (0 != procMsgTalk_mc_mgr(pQyMc, pProcInfo, pProcInfo.pMisCnt, pMsg, len, ref ctx)) break;

										break;

									case Consts.CONST_misMsgType_task:
										{
											MIS_CNT pMisCnt = pProcInfo.pMisCnt;
											MIS_CHANNEL pChannel;

											if (null == pMisCnt)
											{
												break;
											}
											if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) break;


											//  if  (  procMsgTask2Output_mc(  pProcInfo,  (  MIS_CNT  *  )pMsg->task.pMisCnt,  &pMsg->task,  len,  pChannel,  _T(  "thread_mgr"  )  )  )  break;
											if (0 != procMsgTask_mc_mgr(pQyMc, pProcInfo, ref *pMsg, len, false, ref ctx, pChannel)) break;


										}
										break;

									/*
							case CONST_misMsgType_applyForRemovingInvalidTasks_qmc:     //  2009/09/10
																						//
								qmcRemoveInvalidTasks(pProcInfo);
								break;
								*/

									default:
										uint ii;
										ii = pMsg->uiType;
										break;
								}

							}
							//  dwRet  =  WaitForSingleObject(  pProcInfo->mgrQ.hSemaTrigger,  50  );
							//  if  (  dwRet  ==  WAIT_FAILED  )  goto  errLabel;			   

							continue;
							}


						//  if  (  !(  loopCtrl  %  20  )  )  traceLogA(  "ÕâÀï»¹Ó¦¸ÃÓÐÒ»¸ö¶¨ÆÚ£¨±ÈÈç30·ÖÖÓÒ»´ÎµÄ£¬ÇåÀíprocessQµÄ¹¤×÷£¬ÒÔ·ÀÖ¹processQÀïÓÐµÄ½ÚµãÊ§È¥±»´¦ÀíµÄ¿ÉÄÜµÄ¡£¾ÍÒª°ÑÕâÐ©À¬»ø½ÚµãÇåµô"  );

						//
						continue;

						}
				}
			}

			iErr = 0;

		errLabel:

			//
			//showInfo_open(0, 0, _T("thread_mgr: start to empty taskQ"));

			//
			/*
			if (ctx.pProcessQ)
			{
				ctx.pProcessQ->emptyQ();
			}
			*/

			//
			//showInfo_open(0, 0, _T("thread_mgr: taskQ is empty"));

			//
			pProcInfo.status.threadsStatus.ulbMgrQuit = true;

//traceLogA("msgThreadProc_mis_mgr:  leaves");

//return iErr;
return;
		}


        //static unsafe int procMsgInput_mgr_resp(CCtxQyMc pQyMcParam, CCtxQmc pProcInfo, MIS_MSG_INPUT* pMsgInput, uint lenInBytes_msg, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        static unsafe int procMsgInput_mgr_resp(CCtxQyMc pQyMcParam, CCtxQmc pProcInfo, MIS_MSGU  *  pMsgU, uint lenInBytes_msg, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
			int iErr = -1;

			if  (  pMsgU->uiType!=Consts.CONST_misMsgType_input)
			{
				return -1;
			}
			MIS_MSG_INPUT* pMsgInput = &pMsgU->input;


            CCtxQyMc pQyMc = (CCtxQyMc)pQyMcParam;

			//  int						dataLen			=	0;
			int msgLen = 0;
			MIS_CNT pMisCnt = null;
			//MIS_MSG_routeTalkData	*	pAddrTalkData	=	NULL;		
			IM_CONTENTU* pContent = null;
			//
			string str;

			//  CQyMalloc					mallocObj;
			fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
			{
				MIS_MSG_INPUT* pMsg = (MIS_MSG_INPUT*)pMsgBuf_forInput;   //  (  MIS_MSG_INPUT  *  )mallocObj.malloc(  sizeof(  MIS_MSG_INPUT  )  );
				if (null == pMsg)
				{
					traceLogA("procMsgInput_mgr_resp failed: pMsgBuf is null");
					return -1;
				}
				//
				//TCHAR tHintBuf[255 + 1] = _T("");
				TMP_tHintBuf tHintBuf;



				pMisCnt = pProcInfo.pMisCnt;

				if (!isUcFlgResp(pMsgInput->ucFlg)) goto errLabel;

				//  traceLogA(  ""  );
				if (!pMsgInput->ucbConvrted)
				{

					MACRO_memset_misMsgInput(pMsg);
					mymemcpy((IntPtr)pMsg, (IntPtr)pMsgInput, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
					msgLen = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data");

					if (isUcFlgTalkData(pMsgInput->ucFlg) || isUcFlgRouteTalkData(pMsgInput->ucFlg))
					{
						MIS_MSG_routeTalkData* pAddrTalkData = (MIS_MSG_routeTalkData*)&pMsg->data;
						//
						if (0 != qyntohRouteTalkData(pMsgInput->ucFlg, (byte*)&pMsgInput->data, pMsgInput->lenInBytes, &pAddrTalkData->route, (IM_CONTENTU*)&pAddrTalkData->buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf)) goto errLabel;
						uint step = 0;
						if (tHintBuf.tHintBuf[0] != 0) qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), (""), null, (""), (""), new string(tHintBuf.tHintBuf));
						//
						msgLen += (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
						pContent = (IM_CONTENTU*)&pAddrTalkData->buf;
					}
					else
					{
						if (0 == pMsgInput->lenInBytes)
						{
							traceLogA("procMsgInput_mgr_resp failed: msgInput.lenInBytes is 0");
							goto errLabel;
						}

						if (0 != qyntohRouteTalkData(pMsgInput->ucFlg, (byte*)&pMsgInput->data, pMsgInput->lenInBytes, null, (IM_CONTENTU*)&pMsg->data.buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf)) goto errLabel;
						uint step = 0;
						if (tHintBuf.tHintBuf[0] != 0) qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), (""), null, (""), (""), new string(tHintBuf.tHintBuf));
						//
						msgLen += (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
						pContent = (IM_CONTENTU*)&pMsg->data.buf;
					}
					pMsg->ucbConvrted = true;

					//	
					pMsgInput = pMsg;
				}

				//


				//
				pContent = M_getMsgContent(pMsgInput->uiType, pMsgInput->ucFlg, &pMsgInput->data);
				if (null == pContent)
				{
					traceLog(("procMsgInput_mgr_resp failed: pContent is null"));
					goto errLabel;
				}

				//
				bool bNeedProcess = false;

				//  ÏÂÃæ´¦ÀíÏìÓ¦
				//	
				switch (pContent->uiType)
				{

					case Consts.CONST_imCommType_simpleResp:
						{
							//
							str = string.Format("    simpleResp¦%s,  %d", qyGetDesByType1(CONST_qyRcTable, pMsgInput->usCode), pMsgInput->uiTranNo);
							showInfo_open(0, null, str);


							//
							if (null == pCtx.pProcessQ) goto errLabel;

							//
							msgLen += sizeof(IM_SIMPLE_RESP);
							switch (pContent->simpleResp.usCmd_org)
							{
								case Consts.CONST_qyCmd_sendRobotTaskReply:
									qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.robotQ);
									break;
								case Consts.CONST_qyCmd_sendMedia:
									{
										q2PostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.mediaQ2);
									}
									break;

								case Consts.CONST_qyCmd_refreshImObjListReq:
									qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.schedulerQ);
									break;

								default:
									//
									bNeedProcess = true;

#if __DEBUG__
										traceLogA("procMsgInput_mgr_resp: simpleResp, unprocessed cmd %S", qyGetDesByType1(Consts.CONST_qyCmdTable, pContent->simpleResp.usCmd_org));
#endif
									break;
							}

						}
						break;

					/*
				 case  Consts.CONST_imCommType_taskProcResp:
	{

		msgLen += sizeof(pContent->taskProcResp  );
		switch (pContent->taskProcResp.usCmd_org)
		{
			case Consts.CONST_qyCmd_sendMedia:
				q2PostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2);
				break;
			default:
				//
				bNeedProcess = TRUE;

	# ifdef  __DEBUG__
				traceLogA("procMsgInput_mgr_resp: taskProcResp, unprocessed cmd %S", qyGetDesByType1(Consts.CONST_qyCmdTable, pContent->taskProcResp.usCmd_org));
	#endif
				break;
		}

	}
	break;
					*/

					//  case  Consts.CONST_imCommType_intervalParams:
					case Consts.CONST_imCommType_getCfgsReq:
						if (!isRcOk(pMsgInput->usCode)) goto errLabel;
						//
						msgLen += sizeof(QIS_getCfgs_req);
						//
						if (0 != qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.schedulerQ))
						{
							traceLogA("qPostMsgAndTrigger failed");
							goto errLabel;
						}
						//
						break;

					case Consts.CONST_imCommType_retrieveImGrpListResp:
						{
							ref IM_CONTENTU tmp_pContent = ref *pContent;

							if (!isRcOk(pMsgInput->usCode)) goto errLabel;
							//
							msgLen += sizeof(RETRIEVE_IMGRPLIST_RESP);
							//
							if (0 != qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.schedulerQ))
							{
								traceLogA("qPostMsgAndTrigger failed"); goto errLabel;
							}
						}
						break;
					case Consts.CONST_imCommType_retrieveImGrpMemListResp:
						if (!isRcOk(pMsgInput->usCode)) goto errLabel;
						//
						msgLen += sizeof(RETRIEVE_IMGRPMEMLIST_RESP);
						//
						if (0 != qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.schedulerQ))
						{
							traceLogA("qPostMsgAndTrigger failed"); goto errLabel;
						}
						break;
					case Consts.CONST_imCommType_retrieveContactListResp:
						if (!isRcOk(pMsgInput->usCode)) goto errLabel;
						//
						msgLen += sizeof(RETRIEVE_CONTACTLIST_RESP);
						//
						if (0 != qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.schedulerQ))
						{
							traceLogA("qPostMsgAndTrigger failed"); goto errLabel;
						}
						break;
					case Consts.CONST_imCommType_retrieveImObjListEndResp:
						if (!isRcOk(pMsgInput->usCode)) goto errLabel;
						//
						msgLen += sizeof(RETRIEVE_IMOBJLISTEND_RESP);
						//
						if (0 != qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.schedulerQ))
						{
							traceLogA("qPostMsgAndTrigger failed"); goto errLabel;
						}
						break;
					/*
			 case  Consts.CONST_imCommType_refreshRecentFriendsResp:
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->refreshRecentFriendsResp  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->displayQ))
{
	traceLogA("qPostMsgAndTrigger failed"); goto errLabel;
}
break;
			 case  Consts.CONST_imCommType_refreshWebContactsInfo:
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->refreshWebContactsInfo  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->workQ)) goto errLabel;

break;
			 case  Consts.CONST_imCommType_htmlContent:
if (!isRcOk(pMsgInput->usCode)) goto errLabel;

//  qyShowInfo1(  Consts.CONST_qyShowType_debugWarning,  0,  (  ""  ),  pProcInfo->who_showInfo,  _T(  ""  ),  _T(  "procMsgInput_mgr_resp"  ),  _T(  "resp of html. %s"  ),  CQyString(  (  char  *  )&pMsgInput->data  )  );

//  2014/07/12
msgLen += sizeof(pContent->html  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->workQ)) goto errLabel;

break;			 
			 case  Consts.CONST_imCommType_vDevComs:
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->vDevComs  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->workQ)) goto errLabel;

break;
			 case  Consts.CONST_imCommType_retrieveSmPolicy:
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->retrieveSmPolicy  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->workQ)) goto errLabel;

break;
			 case  Consts.CONST_imCommType_retrievePhoneMsgrs:
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->retrievePhoneMsgrs  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->schedulerQ)) goto errLabel;

break;
				   //
				   //
			 case  Consts.CONST_imCommType_refreshDynBmps:     //  2009/08/31
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->refreshDynBmps  );
//
#if  0	//  2011/10/31. 直接发到gui比较简洁
				   if  (  qPostMsgAndTrigger(  pMsg,  msgLen,  &pProcInfo->workQ  )  )  goto  errLabel;				  
#endif

qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ);
PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
break;
			 case  Consts.CONST_imCommType_verifyViewDynBmp:       //  2011/10/31
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->verifyViewDynBmp  );

//
qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ);
PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
break;
			 case  Consts.CONST_imCommType_retrieveDynBmps:    //  2009/09/05
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->retrieveDynBmps  );

//
if (isUcFlgTalkData(pMsgInput->ucFlg) || isUcFlgRouteTalkData(pMsgInput->ucFlg))
{
	//
	qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ);
	PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
	break;
}
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->schedulerQ)) goto errLabel;
break;
			 case  Consts.CONST_imCommType_refreshImObjRulesReq:   //  2010/08/15
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->refreshImObjRulesReq  );
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->schedulerQ)) goto errLabel;
break;
			 case  Consts.CONST_imCommType_retrieveCustomerServiceObjListReq:      //  2011/04/04
if (!isRcOk(pMsgInput->usCode)) goto errLabel;
//
msgLen += sizeof(pContent->retrieveCustomerServiceObjListReq  );
//
qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ);
PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
break;
			 case  Consts.CONST_imCommType_procOfflineRes:  //  2015/08/02
													 //if  (  !isRcOk(  pMsgInput->usCode  )  )  goto  errLabel;
													 //
msgLen += sizeof(pContent->procOfflineResU  );
//
//
if (isUcFlgTalkData(pMsgInput->ucFlg) || isUcFlgRouteTalkData(pMsgInput->ucFlg))
{
	//
	qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ);
	PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
	break;
}
//
if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->schedulerQ)) goto errLabel;
break;
			 case  Consts.CONST_imCommType_imGrp:  //  2017/07/26
msgLen += sizeof(pContent->imGrpEx  );
//
qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ);
PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
break;
					*/
					default:
						//
						bNeedProcess = true;
						//
						{

							//traceLog(_T("procMsgInput_mgr_resp(  ):  unprocessed imCommType[%d] - %s"), pContent->uiType, qyGetDesByType1(Consts.CONST_qyCmdTable, pMsgInput->usCode_org));
							//qyShowInfo1(Consts.CONST_qyShowType_debugWarning, 0, (""), pProcInfo->who_showInfo, _T(""), _T(""), _T(""), _T("procMsgInput_mgr_resp: unprocessed imCommType %d - %s"), pContent->uiType, qyGetDesByType1(Consts.CONST_qyCmdTable, pMsgInput->usCode_org));
						}
						break;
				}
				//
				if (!bNeedProcess)
				{
					iErr = 0; goto errLabel;
				}

				/*
		   //
		   switch (pContent->uiType)
		   {

			   case Consts.CONST_imCommType_simpleResp:
				   {
					   //
					   qyShowInfo1(Consts.CONST_qyShowType_debug, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("    simpleResp¦%s,  %d"), qyGetDesByType1(Consts.CONST_qyRcTable, pMsgInput->usCode), pMsgInput->uiTranNo);

					   msgLen += sizeof(pContent->simpleResp  );
					   switch (pContent->simpleResp.usCmd_org)
					   {
						   case Consts.CONST_qyCmd_sendRobotTaskReply:
							   qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->robotQ);
							   break;
						   case Consts.CONST_qyCmd_sendMedia:
							   {
								   CQyQ2Help q2Help;

								   q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2);
							   }
							   break;
						   case Consts.CONST_qyCmd_refreshImObjListReq:
							   qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->schedulerQ);
							   break;
						   default:
		   # ifdef  __DEBUG__
							   traceLogA("procMsgInput_mgr_resp: simpleResp, unprocessed cmd %S", qyGetDesByType1(Consts.CONST_qyCmdTable, pContent->simpleResp.usCmd_org));
		   #endif
							   break;
					   }

				   }
				   break;

			   case Consts.CONST_imCommType_taskProcResp:
				   {   //  2009/10/19

					   msgLen += sizeof(pContent->taskProcResp  );
					   switch (pContent->taskProcResp.usCmd_org)
					   {
						   case Consts.CONST_qyCmd_sendMedia:
							   q2PostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2);
							   break;
						   default:
							   traceLogA("procMsgInput_mgr_resp: taskProcResp, unprocessed cmd %S", qyGetDesByType1(Consts.CONST_qyCmdTable, pContent->taskProcResp.usCmd_org));
							   break;
					   }

				   }
				   break;
			   case Consts.CONST_imCommType_transferFileDataResp:
				   //
				   msgLen += sizeof(pContent->transferFileDataResp  );
				   //
				   if (qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->robotQ)) goto errLabel;

				   break;


			   default:
				   break;
		   }

		   */
			}

		iErr = 0;

errLabel:

if (0!=iErr)
{
}
return iErr;
}




        //int  procMsgInput_mgr_req(  MC_VAR_isCli  *  pProcInfo,  MIS_MSG_INPUT  *  pMsgInput,  unsigned  int  lenInBytes_msg,  CTX_MC_THREAD  *  pCtx  )
        static unsafe int procMsgInput_mgr_req(CCtxQyMc pQyMcParam, CCtxQmc pProcInfo, MIS_MSGU  *  pMsgU, uint lenInBytes_msg, ref CTX_MC_thread pCtx, MIS_CHANNEL pChannel)
        {
            int iErr = -1;

            if (  pMsgU->uiType!=Consts.CONST_misMsgType_input)
            {
				return -1;                
            }
			MIS_MSG_INPUT* pMsgInput = &pMsgU->input;

			//
            CCtxQyMc pQyMc = (CCtxQyMc)pQyMcParam;
            //QY_SERVICEGUI_INFO* pSci = (QY_SERVICEGUI_INFO*)pSgiParam;
            bool bDbg = false;
            string str;
#if DEBUG
            bDbg = true;
#endif
            //
            int len;
            //
            //char tHintBuf[255 + 1] = _T("");
            TMP_tHintBuf tHintBuf;
            //TCHAR tBuf[128] = _T("");
            TMP_tBuf128 tBuf;


            if (isUcFlgResp(pMsgInput->ucFlg)) return -1;


			//  
#if DEBUG
			qyFuncs.traceLog("procMsgInput_mgr_req enters");
#endif

			//  2017/10/11  

			//	
			//  2015/09/07
			bool bNeedProcess = false;


            //
            switch (pMsgInput->usCode)
            {
                case Consts.CONST_qyCmd_talkTo:
					{
						//  CQyMalloc		mallocObj;
						fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
						{
							MIS_MSGU* pMsg1 = pMsgBuf_forInput;    //  (  MIS_MSGU  *  )mallocObj.malloc(  sizeof(  MIS_MSGU  )  );
							if (null == pMsg1)
							{
								goto errLabel;
							}

							MACRO_memset_misMsgTalk(&pMsg1->talk);

							pMsg1->talk.uiType = Consts.CONST_misMsgType_talk;
							pMsg1->talk.ucFlg = pMsgInput->ucFlg;
							//
							pMsg1->talk.usCode = pMsgInput->usCode;
							pMsg1->talk.tStartTime = pMsgInput->tStartTime;
							pMsg1->talk.uiTranNo = pMsgInput->uiTranNo;
							pMsg1->talk.usSeqNo = pMsgInput->usSeqNo;
							//
							//pMsg->talk.pMisCnt = pMsgInput->pMisCnt;
							//
							pMsg1->talk.tRecvTime = pMsgInput->tRecvTime;
							//				  
							if (0 != qyntohRouteTalkData(pMsgInput->ucFlg, (byte*)&pMsgInput->data, pMsgInput->lenInBytes, &pMsg1->talk.data.route, (IM_CONTENTU*)&pMsg1->talk.data.buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
							{
								//showInfo_open(0, 0, _T("procMsgInput_mgr_req failed, qyntohRouteTalkData failed"));
								goto errLabel;
							}
							//if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T(""), 0, _T(""), _T(""), _T("    %s"), tHintBuf);

							IM_HTML_CONTENT* pHtmlContent;

							pHtmlContent = (IM_HTML_CONTENT*)&pMsg1->talk.data.buf;
							len = (int)Marshal.OffsetOf<IM_HTML_CONTENT>("wBuf") + Math.Min(Consts.cntof_imHtmlContent_wBuf * sizeof(char), (mywcslen(pHtmlContent->wBuf) + 1) * sizeof(char));

							//
							{
								//traceLogA("talk str is [%S]", pHtmlContent->wBuf);
							}
							//
							pMsg1->talk.lenInBytes = (uint)(len + (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf"));

							len = (int)pMsg1->talk.lenInBytes + (int)Marshal.OffsetOf<MIS_MSG_TALK>("data");
							if (0 != qPostMsgAndTrigger((IntPtr)pMsg1, (uint)len, pProcInfo.workQ)) goto errLabel;

						}
					}
                    break;
                case Consts.CONST_qyCmd_sendTask:
                case Consts.CONST_qyCmd_sendTaskReply:


                    if (0 != qPostMsgAndTrigger((IntPtr)pMsgInput, lenInBytes_msg, pProcInfo.workQ)) goto errLabel;

                    break;

                default:
                    bNeedProcess = true;
                    break;
            }
            if (!bNeedProcess)
            {
                iErr = 0; goto errLabel;
            }




            //
            MIS_CNT pMisCnt = null;
            MIS_MSG_routeTalkData* pAddrTalkData = null;
            int msgLen = 0;
            IM_CONTENTU* pContent = null;

			//  CQyMalloc					mallocObj;	
			fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
			{
				MIS_MSG_INPUT* pMsg = (MIS_MSG_INPUT*)pMsgBuf_forInput;   //  (  MIS_MSG_INPUT  *  )mallocObj.malloc(  sizeof(  MIS_MSG_INPUT  )  );
				if (null == pMsg)
				{
					goto errLabel;
				}


				//
				int lenInBytes = 0;
				//
				bool bRemoveIfErr = true;
				//  2008/11/19
				MSG_ROUTE route;
				byte ucFlg = 0;

				QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
				if (null == pDbFuncs) goto errLabel;
				QM_dbFuncs g_dbFuncs = pDbFuncs;


				//if (!pMsgInput->pMisCnt) return -1;

				//if (sizeof(pMsgInput->data)  < sizeof(MIS_MSG_routeTalkData)  )  {
				//	goto errLabel;
				//}


				pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;

				if (isUcFlgResp(pMsgInput->ucFlg)) goto errLabel;

				//  traceLogA(  ""  );

				//
				if  (  pMsgInput->usCode==Consts.CONST_qyCmd_sendVDevReq )
				{
					int ii = 0;
				}


				//
				if (!pMsgInput->ucbConvrted)
				{

					MACRO_memset_misMsgInput(pMsg);
					mymemcpy((IntPtr)pMsg, (IntPtr)pMsgInput, (uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data"));
					msgLen = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data");

					if (isUcFlgTalkData(pMsgInput->ucFlg) || isUcFlgRouteTalkData(pMsgInput->ucFlg))
					{
						pAddrTalkData = (MIS_MSG_routeTalkData*)&pMsg->data;
						//
						if (0 != qyntohRouteTalkData(pMsgInput->ucFlg, (byte*)&pMsgInput->data, pMsgInput->lenInBytes, &pAddrTalkData->route, (IM_CONTENTU*)&pAddrTalkData->buf, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
						{
							//showInfo_open(0, 0, _T("procMsgInput_mgr_req failed, qyntohRouteTalkData failed"));
							goto errLabel;
						}
						//if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T(""), 0, _T(""), _T(""), _T("    %s"), tHintBuf);
						//
						msgLen += (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
						//
						pContent = (IM_CONTENTU*)&pAddrTalkData->buf;
					}
					else
					{
						if (0 == pMsgInput->lenInBytes)
						{
							goto errLabel;
						}

						if (0 != qyntohRouteTalkData(pMsgInput->ucFlg, (byte*)&pMsgInput->data, pMsgInput->lenInBytes, null, (IM_CONTENTU*)&pMsg->data, tHintBuf.tHintBuf, Consts.cntof_tHintBuf))
						{
							//showInfo_open(0, 0, _T("procMsgInput_mgr_req failed, qyntohRouteTalkData failed sssssss"));
							goto errLabel;
						}
						//if (tHintBuf[0]) qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T(""), 0, _T(""), _T(""), _T("    %s"), tHintBuf);
						//
						pContent = (IM_CONTENTU*)&pMsg->data;
					}
					pMsg->ucbConvrted = true;

					//
					pMsgInput = pMsg;

				}

				pAddrTalkData = MACRO_msgRouteTalkData(pMsgInput->ucFlg, &pMsgInput->data);
				if (null == pAddrTalkData)
				{
					goto errLabel;
				}
				//
				pContent = M_getMsgContent(pMsgInput->uiType, pMsgInput->ucFlg, &pMsgInput->data);
				if (null == pContent)
				{
					goto errLabel;
				}


				//  2017/10/11  
				/*
				if (pQyMc->env.bDebugInfo && pProcInfo->cfg.debugStatusInfo.ucbViewMgrStatus)
				{
					_sntprintf(tBuf, mycountof(tBuf), _T("procMsgInput_mgr_req: tn %d. after ntoh"), pMsgInput->uiTranNo);
					showInfo_open(0, 0, tBuf);
				}
				*/
				//
#if DEBUG
				if  (  pContent->uiType==Consts.CONST_imCommType_confKey )
				{
					int ii = 0;
				}
#endif


				//
				bNeedProcess = false;

				//
				switch (pContent->uiType)
				{


					case Consts.CONST_imCommType_transferFileReq:
						{
							TRANSFER_FILE_REQ* pReq = null;
							//
							pMsgInput->lenInBytes = Math.Min((uint)sizeof(MIS_MSG_routeTalkData), (uint)(Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof(TRANSFER_FILE_REQ)));
							//
							pReq = (TRANSFER_FILE_REQ*)&pContent->transferFileReq;
							//
							//  ÏÂÃæÊÇÄÚ²¿´¦Àí±äÁ¿
							mylstrcpyn(pReq->fullFilePath, pReq->fileName, Consts.MAX_PATH + 1);
							//
							//pMsgInput->uiType = Consts.CONST_misMsgType_task;
							//
							pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
							pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
							//
							len = (int)((uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput->lenInBytes);
							//
							if (0 != procMsgInput_mc_mgr(pQyMc, pProcInfo, ref *pMsgInput, (uint)len, true, ref pCtx, pChannel, "procMsgInput_mgr_req.1095")) goto errLabel;
						}
						break;

					case Consts.CONST_imCommType_transferFileReplyReq:
						{
							TRANSFER_FILE_REPLYREQ* pReq = null;
							//
							pMsgInput->lenInBytes = Math.Min((uint)sizeof(MIS_MSG_routeTalkData), (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)sizeof(TRANSFER_FILE_REPLYREQ));
							//
							pReq = (TRANSFER_FILE_REPLYREQ*)&pContent->transferFileReplyReq;
							//
							//pMsgInput->uiType = Consts.CONST_misMsgType_task;
							//
							pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
							pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
							//
							len = (int)((uint)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput->lenInBytes);

							if (0 != procMsgInput_mc_mgr(pQyMc, pProcInfo, ref *pMsgInput, (uint)len, true, ref pCtx, pChannel, "procMsgInput_mgr_req.1114")) goto errLabel;
						}
						break;

					case Consts.CONST_imCommType_transferFileDataReq:
						{

							msgLen += sizeof(TRANSFER_FILEDATA_REQ);
							if (0 != qPostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.robotQ)) goto errLabel;

#if false//0
				  //  TRANSFER_FILEDATA_REQ		req;
				  //
				  //  memset(  &req,  0,  sizeof(  req  )  );
				  //  if  (  stream2Data(  &ptr,  &size,  tmpHandler_stream2TransferFileDataReq,  0,  &req  )  )  goto  errLabel;
				  //  memcpy(  pContent,  &req,  min(  sizeof(  pContent[0]  ),  sizeof(  req  )  )  );
				  // 
				  pMsgInput->lenInBytes  =  min(  sizeof(  pMsgInput->data  ),  offsetof(  MIS_MSG_routeTalkData,  buf  )  +  sizeof(  TRANSFER_FILEDATA_REQ  )  );

				  TRANSFER_FILEDATA_RESP		resp;
				  //
				  memset(  &resp,  0, sizeof(  resp  )  );
				  resp.uiType  =  Consts.CONST_imCommType_transferFileDataResp;

				  if  (  !pCtx->pProcessQ  )  goto  errLabel;
				  printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
				  traceLogA(  "Now print pMsgInput"  );
				  printMisMsg(  0,  0,  (  MIS_MSGU  *  )pMsgInput  );

				  if  (  pCtx->pProcessQ->qTraverse(  tmpHandler_findOrgMsgAndProc_is,  pMsgInput,  &resp  )  <=  0  )  {
					  traceLogA(  "Ã»ÓÐÕÒµ½Ïà¹ØµÄÈÎÎñ½Úµã»ò´¦Àí³ö´í£¬²»ÄÜ¼ÌÐø"  );
					  goto  errLabel;				   
				  }
				  lenInBytes  =  sizeof(  resp  );
				  
				  //  2009/02/25
				  startChannel(  pProcInfo,  pMisCnt,  pChannel,  _T(  "proc transferFileDataReq"  )  );    

				  //
				  ucFlg  =  0;
				  memset(  &route,  0,  sizeof(  route  )  );
				  route.idInfo_from.ui64Id  =  pMisCnt->idInfo.ui64Id;
				  if  (  1  )  {
					   ucFlg  |=  Consts.CONST_commFlg_routeTalkData;
					   route.debugInfo.dwTickCnt_me_start  =  GetTickCount(  );	
					   time_t	tNow;	mytime(  &tNow  );  route.debugInfo.tTime_start  =  tNow;
				  }
				  ucFlg  |=  Consts.CONST_commFlg_resp;		//  ×¢Òâ£¬ÕâÀïÊÇÏìÓ¦
				  route.debugInfo.index  =  resp.uiDataBlockIndex;

				  if  (  postMsg2Mgr_mc(  pMisCnt,  &route,  Consts.CONST_misMsgType_outputTask,  ucFlg,  Consts.CONST_qyCmd_sendRobotTaskData, pMsgInput->tStartTime,  pMsgInput->uiTranNo,  0,  (  char  *  )&resp,  lenInBytes,  &pMsgInput->addr_logicalPeer.idInfo,  &pAddrTalkData->route.idInfo_from,  uiChannelType,  NULL,  FALSE  )  )  goto  errLabel;
#endif
						}
						break;
					case Consts.CONST_imCommType_transferFileEndReq:
						{

							//  TRANSFER_FILEEND_REQ    req;

							traceLogA("transferFileEndReq");

							//  memset(  &req,  0,  sizeof(  req  )  );
							//  if  (  stream2Data(  &ptr,  &size,  tmpHandler_stream2TransferFileEndReq,  0,  &req  )  )  goto  errLabel;                                 
							//  memcpy(  pContent,  &req,  min(  sizeof(  pContent[0]  ),  sizeof(  req  )  )  );

							pMsgInput->lenInBytes = (uint)Math.Min(sizeof(MIS_MSG_routeTalkData), (int)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof(TRANSFER_FILEEND_REQ));

							if (null == pCtx.pProcessQ) goto errLabel;
							//
							//printQ_mis(  &pCtx->pProcessQ->m_q,  0  );

							//
							//printMisMsg(null, null, (MIS_MSGU*)pMsgInput);

							CTRANSFER_FILEEND_RESP resp = new CTRANSFER_FILEEND_RESP();
							//
							//mymemset((IntPtr)(byte*)&resp, 0, sizeof(TRANSFER_FILEEND_RESP));
							resp.transferFileEndResp.uiType = Consts.CONST_imCommType_transferFileEndResp;
							//  2015/09/08
							resp.transferFileEndResp.tmpInternal.bChkToRemove_i = true;
							//
							PARAM_findOrgMsg param = new PARAM_findOrgMsg();
							param.pTransferFileEndResp = resp;
							//
							if (pCtx.pProcessQ.qTraverse(null, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsgInput, param) <= 0) break;
							if (resp.transferFileEndResp.tmpInternal.bNeedRemoved_o)
							{
								//pCtx->pProcessQ->qRemoveMsg(  tmpHandler_findOrgMsgAndProc_is,  pMsgInput,  (IntPtr)0  );
								qmcApplyForRemovingInvalidTasks(pProcInfo, 0);  //  2015/09/07
							}

							//
							//printQ_mis(  &pCtx->pProcessQ->m_q,  0  );

							traceLogA("Î´Íê³É£ºÊµ¼ÊÉÏÕâÀï»¹Ó¦¸Ã·¢Ò»¸öendÏìÓ¦°ü");

						}
						break;


					case Consts.CONST_imCommType_taskProcReq:
						{

							//  
#if DEBUG
							ref IM_CONTENTU tmp_pContent = ref *pContent;
							str = string.Format("procMsgInput_mgr_req: taskProcReq,usOp {0} {1}", pContent->taskProcReq.usOp, qyGetDesByType1(CONST_imOpTable_en, pContent->taskProcReq.usOp));
							traceLog(str);
							int iii = 0;
							//
							switch (pContent->taskProcReq.usOp)
							{
								case Consts.CONST_imOp_recv_cancel:
									break;
								case Consts.CONST_imOp_send_cancel:
									break;
								case Consts.CONST_imOp_recv_applyForKeyFrame:
									break;
								default:
									break;
							}

#endif
							//
							if (pContent->taskProcReq.usOp == Consts.CONST_imOp_recv_applyForKeyFrame)
							{
								str = "get applyForKeyFrame from " + pMsgInput->data.route.idInfo_from.ui64Id + ", tn " + pMsgInput->uiTranNo;
								showInfo_open(0, null, str);
							}



							//
							pMsgInput->lenInBytes = Math.Min((uint)sizeof(MIS_MSG_routeTalkData), (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + (uint)sizeof(TASK_PROC_REQ));

							if (null == pCtx.pProcessQ)
							{
#if __DEBUG__
				traceLogA("procMsgInput_mgr_req error: taskProcReq: pProcessQ is null");
#endif
								goto errLabel;
							}

							//
							bool bBreak = false;
#if DEBUG
							//  printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
							//  printMisMsg(  0,  0,  (  MIS_MSGU  *  )pMsgInput  );
							if (pContent->taskProcReq.usOp == Consts.CONST_imOp_recv_cancel)
							{
								int ii = 0;
								bBreak = true;
							}
							if (pContent->taskProcReq.usOp == Consts.CONST_imOp_recv_applyForOrgReq)
							{
								int ii = 0;
							}
#endif

							//
							CIM_CONTENTU contentU = new CIM_CONTENTU();
							ref TASK_PROC_RESP resp = ref contentU.imContentU.taskProcResp;
                            //  
                            //mymemset((IntPtr)(byte*)&resp, 0, sizeof(TASK_PROC_RESP));
							resp.uiType = Consts.CONST_imCommType_taskProcResp;

							//
							PARAM_findOrgMsg param = new PARAM_findOrgMsg();
							//param.pTaskProcResp = &resp;
							param.pContent_o = contentU;
							param.bBreak = bBreak;

							int tmpiRet;
							tmpiRet = pCtx.pProcessQ.qTraverse(pCtx.ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsgInput, param);


							//  
							//  
							if (pContent->taskProcReq.usOp == Consts.CONST_imOp_send_cancel)
							{
								object pDb = null;
								CE_TASK_MEM taskMem;
								int iStatus = Consts.CONST_imTaskStatus_canceledBySender;
								string subject;
								string content;

								//
								using (CQnmDb db = new CQnmDb())
								{
									if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
									pDb = db.m_pDbMem.pDb;

									IM_TASK_RCD rcd = new IM_TASK_RCD();
									if (!g_dbFuncs.pf_bGetImTaskRcdBySth(pDb, pQyMc.cfg.db.iDbType, false, "", pAddrTalkData->route.idInfo_from, (Int64)pContent->taskProcReq.tStartTime_org, pContent->taskProcReq.uiTranNo_org, 0, ref rcd)) goto errLabel;

									mymemset((IntPtr)(byte*)&taskMem, 0, sizeof(CE_TASK_MEM));
									taskMem.iStatus = rcd.iStatus;
									taskMem.iTaskId = rcd.id;
									taskMem.idInfo_send.ui64Id = rcd.idInfo_send.ui64Id;
									taskMem.idInfo_recv.ui64Id = rcd.idInfo_recv.ui64Id;
									subject = new string(rcd.subject);
									content = new string(rcd.content);

									//  2014/11/22
									switch (taskMem.iStatus)
									{
										case Consts.CONST_imTaskStatus_applyToRecv:
										case Consts.CONST_imTaskStatus_resp:
										case Consts.CONST_imTaskStatus_waitToRecv:
											break;
										default:
											iErr = 0; goto errLabel;
											break;  //  
									}

									//  
									if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, taskMem.iTaskId)) goto errLabel;
									//
								}
								//

								showTaskStatus(pMisCnt, pMsgInput->addr_logicalPeer.idInfo.ui64Id, taskMem.idInfo_send.ui64Id, taskMem.idInfo_recv.ui64Id, false, pContent->taskProcReq.uiContentType_org, taskMem.iTaskId, iStatus, 0, 0, subject, content);

								//  2015/10/08  
								break;
							}



							if (tmpiRet > 0)
							{
								if (pContent->taskProcReq.usOp == Consts.CONST_imOp_recv_applyForChkTaskAlive)
								{


									str = string.Format("Resp to {0}, task is alive", pMsgInput->data.route.idInfo_from.ui64Id);
									uint step = 0;
									qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, _T(""), _T(""), str);
									//
									//
									resp.tStartTime_org = pContent->taskProcReq.tStartTime_org;
									resp.uiTranNo_org = pContent->taskProcReq.uiTranNo_org;
									resp.usCmd_org = pMsgInput->usCode;
									resp.usOp_org = pContent->taskProcReq.usOp;
									//
									lenInBytes = sizeof(TASK_PROC_RESP);
									//
									ucFlg = 0;
									ucFlg |= Consts.CONST_commFlg_resp;        //  ×¢Òâ£¬ÕâÀïÊÇÏìÓ¦

									MSG_ROUTE msgRoute = new MSG_ROUTE();
									fixed (TASK_PROC_RESP* pResp = &resp)
									{
										if (0 != postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_outputTask, ucFlg, Consts.CONST_qyRc_ok, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pResp, (uint)lenInBytes, pMsgInput->addr_logicalPeer.idInfo.ui64Id, pAddrTalkData->route.idInfo_from.ui64Id, pMsgInput->uiChannelType, pCtx.pMsgBuf, false)) goto errLabel;
									}

									break;
								}

								if (resp.bNeedRemoved)
								{
#if DEBUG
									traceLog(_T("Need removed"));
#endif
								}


#if __DEBUG__
						  //  printQ_mis(  &pCtx->pProcessQ->m_q,  0  );
#endif
							}

							iErr = 0; goto errLabel;
						}
						break;


					case Consts.CONST_imCommType_transferAvInfo:
						{
							TRANSFER_AV_INFO* pReq = null;
							//
							//  2014/04/28
							//qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T("procMsgInput_mgr_req"), _T(": recv transferAvInfo from %I64u..... tn %d"), pMsgInput->data.route.idInfo_from.ui64Id, pMsgInput->uiTranNo);

							//
							pMsgInput->lenInBytes = (uint)Math.Min((int)sizeof(MIS_MSG_routeTalkData), (int)(Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof(TRANSFER_AV_INFO)));
							//
							pReq = (TRANSFER_AV_INFO*)&pContent->transferAvInfo;
							//  2014/09/28
							if (0 != pReq->idInfo_logicalPeer.ui64Id)
							{
								if (pContent->transferAvInfo.uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
								{

									//pMsgInput->uiType = Consts.CONST_misMsgType_task;
									pMsgInput->uiTaskType = pContent->transferAvInfo.uiTaskType;

									//	
									pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
									pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
									//
									len = (int)((int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsgInput->lenInBytes);

									if (0 != pReq->idInfo_logicalPeer.ui64Id)
									{
										if (null == pMisCnt) goto errLabel;
										if (pReq->idInfo_logicalPeer.ui64Id == pMisCnt.idInfo.ui64Id)
										{
											showInfo_open(0, null, ("procMsgInput_other_req failed, viewDynBmp, req.idInfo_logicalPeer is me"));
											goto errLabel;
										}
										pMsgInput->addr_logicalPeer.idInfo.ui64Id = pReq->idInfo_logicalPeer.ui64Id;
									}
									else
									{
										pMsgInput->addr_logicalPeer.idInfo.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
									}

									if (0 != qPostMsg((IntPtr)pMsgInput, (uint)len, pQyMc.gui.processQ))
									{
										traceLogA("mcThreadProc_mis_work(  ): qPostMsg(  ) failed");
										goto errLabel;
									}
									GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);

									iErr = 0; goto errLabel;
								}
							}

							//
							//
#if true
							if (0 != pReq->idInfo_logicalPeer.ui64Id)
							{
								if (null == pMisCnt) goto errLabel;
								if (pReq->idInfo_logicalPeer.ui64Id == pMisCnt.idInfo.ui64Id)
								{
									showInfo_open(0, null, ("procMsgInput_other_req failed, req.idInfo_logicalPeer is me"));
									goto errLabel;
								}
								pMsgInput->addr_logicalPeer.idInfo.ui64Id = pReq->idInfo_logicalPeer.ui64Id;
							}
#endif
							//
							//pMsgInput->uiType = Consts.CONST_misMsgType_task;
							//
							pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
							pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
							//
							len = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (int)pMsgInput->lenInBytes;
							//
							//  
							//if (procMsgTask_mc_mgr(pQyMc, pProcInfo, pMsgInput, len, true, pCtx, pChannel)) goto errLabel;
							if (0 != procMsgInput_mc_mgr(pQyMc, pProcInfo, ref *pMsgInput, (uint)len, true, ref pCtx, pChannel, "procMsgInput_mgr_req.1456")) goto errLabel;
						}
						break;

					case Consts.CONST_imCommType_transferAvReplyInfo:
						{
							TRANSFER_AV_replyInfo* pReq = null;
							//
							if (pChannel.uiType == Consts.CONST_channelType_realTimeMedia)
							{   //  realTimeMedia 
								//qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, NULL, _T(""), _T(""), _T("Error: %s ÊÕµ½ %I64u µÄ %s"), qyGetDesByType1(Consts.CONST_channelTypeTable, pChannel->uiType), pMsgInput->data.route.idInfo_from.ui64Id, qyGetDesByType1(getResTable(0, &pQyMc->cusRes, Consts.CONST_resId_qyCommTypeTable), pContent->uiType));
								goto errLabel;
							}
							//
							pMsgInput->lenInBytes = (uint)Math.Min(sizeof(MIS_MSG_routeTalkData), (int)(Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + sizeof(TRANSFER_AV_replyInfo)));
							//
							pReq = (TRANSFER_AV_replyInfo*)&pContent->transferAvReplyInfo;
							//
							//pMsgInput->uiType = Consts.CONST_misMsgType_task;
							//
							pMsgInput->idInfo_taskSender.ui64Id = pAddrTalkData->route.idInfo_to.ui64Id;
							pMsgInput->idInfo_taskReceiver.ui64Id = pAddrTalkData->route.idInfo_from.ui64Id;
							//
							len = (int)Marshal.OffsetOf<MIS_MSG_INPUT>("data") + (int)pMsgInput->lenInBytes;

							if (0 != procMsgInput_mc_mgr(pQyMc, pProcInfo, ref *pMsgInput, (uint)len, true, ref pCtx, pChannel, "procMsgInput_mgr_req.1481")) goto errLabel;
						}
						break;


					case Consts.CONST_imCommType_transferVideoData:
						{
							//CQyQ2Help q2Help;

#if __DEBUG__
	traceLogA("mm");
#endif

							msgLen += sizeof(TRANSFER_VIDEO_DATA);
							if (0 != q2PostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.mediaQ2)) goto errLabel;
						}
						break;

					case Consts.CONST_imCommType_transferAudioData:
						{
							//CQyQ2Help q2Help;

							msgLen += sizeof(TRANSFER_AUDIO_DATA);
							if (0 != q2PostMsgAndTrigger((IntPtr)pMsg, (uint)msgLen, pProcInfo.realTimeMediaQ2)) goto errLabel;
						}
						break;


					default:
						bNeedProcess = true;
						break;
				}
				if (!bNeedProcess)
				{
					iErr = 0; goto errLabel;
				}




				//		
				switch (pContent->uiType)
				{
					/*
		case Consts.CONST_imCommType_transferAvInfo:
			{
				CQyQ2Help q2Help;
				//
				msgLen += sizeof(pContent->transferAvInfo  );
				//
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferAvReplyInfo:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferAvReplyInfo  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferVideoData:
			{
				CQyQ2Help q2Help;

	# ifdef  __DEBUG__
				traceLogA("mm");
	#endif

				msgLen += sizeof(pContent->transferVideoData  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferVideoDataResp:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferVideoDataResp  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferAudioData:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferAudioData  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->realTimeMediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferAudioDataResp:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferAudioDataResp  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->realTimeMediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_remoteAssistReq:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->remoteAssistReq  );
				//  if  (  q2Help.qPostMsgAndTrigger(  pMsg,  msgLen,  &pProcInfo->mediaQ2  )  )  goto  errLabel;				   				
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->rtOpQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_taskProcReq:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->taskProcReq  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_taskProcResp:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->taskProcResp  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferGpsInfo:
			{   //  2012/04/20
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferGpsInfo  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferGpsReplyInfo:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferGpsReplyInfo  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_transferGpsData:
			{
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->transferGpsData  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_ptzControlReq:
			{   //  2012/08/06
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->ptzControlReq  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;
		case Consts.CONST_imCommType_ptzControlCmd:
			{   //  2012/08/06
				CQyQ2Help q2Help;

				msgLen += sizeof(pContent->ptzControlCmd  );
				if (q2Help.qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->mediaQ2)) goto errLabel;
			}
			break;

		//  2014/05/03
		case Consts.CONST_imCommType_retrieveDynBmps:
	# ifdef  __DEBUG__
			traceLog(_T("procMsgInput_mgr_req: retrieveDynBmps"));
	#endif
			msgLen += sizeof(pContent->retrieveDynBmps  );
			if (qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ)) goto errLabel;
			PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
			//
			break;

		//  2015/08/02
		case Consts.CONST_imCommType_procOfflineRes:
	# ifdef  __DEBUG__
			traceLog(_T("procMsgInput_mgr_req: procOfflineRes"));
	#endif
			msgLen += sizeof(pContent->procOfflineResU  );
			//
			if (pContent->procOfflineResU.common.usSubtype == Consts.CONST_procOfflineResSubtype_download)
			{
				if (qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ)) goto errLabel;
				PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);
				break;
			}
			//
			qPostMsgAndTrigger(pMsg, msgLen, &pProcInfo->schedulerQ);
			//
			break;

		//  2018/10/27
		case Consts.CONST_imCommType_imGrp:
	# ifdef  __DEBUG__
			traceLog(_T("procMsgInput_mgr_req: imGrp"));
	#endif
			msgLen += sizeof(pContent->imGrpEx  );
			//
			if (qPostMsg(pMsg, msgLen, &pQyMc->gui.processQ)) goto errLabel;
			PostMessage(pQyMc->gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_msgArrive, 0);


			break;
					*/

					default:
						str = string.Format("procMsgInput_mgr_req: unprocessed contentType {0}", pContent->uiType);
#if __DEBUG__
		traceLog(_T("procMsgInput_mgr_req: unprocessed contentType %d"), pContent->uiType);
#endif
						showInfo_open(0, null, str);
						break;



				}

			}

            iErr = 0;

		errLabel:

#if DEBUG
			qyFuncs.traceLog("procMsgInput_mgr_req leaves " + iErr);
#endif

			return iErr;
        }

    }
}


