using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	unsafe struct TMP_buf_myDRAW_VIDEO_DATA_pkts_toShareVideo_post
	{
		public fixed byte buf[Consts.bufSize_myDRAW_VIDEO_DATA_pkts_toShareVideo_post];
	};


	partial class qyFuncs
{
		public static unsafe void mcThreadProc_toShareVideo_post(object lpParameter)
		{
			int iIndex_sharedObj = (int)lpParameter;

			//
			bool bDbg = false;
			string str;

			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			if (null==pQyMc)
			{
				return;
			}
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pSci->pVar;
			if (null==pProcInfo) return;

			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) return;

			CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
			if (null==pCapProcInfo) return;

			//  2010/12/28
			pCapProcInfo.compressVideo.debugInfo.nStep = 101;

			//  2009/12/24

			//
			uint dwThreadId = myGetCurrentThreadId();
			CTX_MC_thread ctx = new CTX_MC_thread();
			ctx.ctxCaller.type = Consts.CONST_ctxCallerType_thread;
			ctx.ctxCaller.dwThreadId = myGetCurrentThreadId();
			fixed (char* pName = ctx.ctxCaller.tNameBuf.tNameBuf) {
				mytcsncpy(pName, "thread_toShareV", Consts.cntof_ctxCaller_tNameBuf);					
			}

			//
			int i;

			//  uint					uiBufSize					=	sizeof(  myDRAW_VIDEO_DATA  );
			//myDRAW_VIDEO_DATA pkts[CONST_pkts_toShareVideo_post];
			TMP_buf_myDRAW_VIDEO_DATA_pkts_toShareVideo_post buf_pkts;
			//uint uiLens[Consts.CONST_pkts_toShareVideo_post];     //  要增加一个qGetMsgs，将pPkts和pktLens作为参数输入。2011/07/15
			uint[] uiLens = new uint[Consts.CONST_pkts_toShareVideo_post];
			int nLens = uiLens.Length;// mycountof(uiLens);      //  一次最多处理的包数。
			int nPkts;                      //								pPkts[]中有几个包是有效的。 2011/07/15

			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
			if (null==pMisCnt) return;


			

			//
			using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc()) {
				MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(sizeof(MIS_MSGU));
				if (null==pMsgBuf) return;


				uint dwTickCnt;

				//  2010/09/18
				//  TMP_MEMORY_GRP					tmpMemoryGrp;
				//  bool							bInited_tmpMemoryGrp		=	false;
				int tmpiRet;
				bool tmpbErr;


				//
				//traceLogA("mcThreadProc_toShareBmp_post %S enters", pCapProcInfo.postThread.q2.cfg.name);

				pCapProcInfo.postThread.bQuit = false;
				pCapProcInfo.postThread.bRunning = true;


				//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "%s, %I64u starts"  ),  pPlay->q2.cfg.name,  pPlayer->idInfo_recorder.ui64Id  );


				/////////////////

				for (; !pCapProcInfo.postThread.bQuit;)
				{

					//  2010/12/28
					pCapProcInfo.compressVideo.debugInfo.nStep = 105;

					//
					/*
					uint dwEvt = MsgWaitForMultipleObjects(
									  pCapProcInfo.postThread.q2.cfg.usCnt_hEvents,    //  mycountof(  pPlay->hEvents  ),				// Number of events.
									   pCapProcInfo.postThread.q2.hEvents,                         // Location of handles.
									   false,                                       // Wait for all?
									   5000,                                    // How long to wait?
									   0);                                      // Any message is an 
																				// event.
					if (dwEvt == WAIT_FAILED) goto errLabel;
					*/
					myWaitOne(pCapProcInfo.postThread.q2.hEvent0,5000);
					//
					dwTickCnt = myGetTickCount(null);

					/*
					if (dwEvt != WAIT_TIMEOUT)
					{
						dwEvt -= WAIT_OBJECT_0;
						switch (dwEvt)
						{
							case 0:
								//  dwTickCnt_lastData  =  dwTickCnt;		//  audio data received
								//  traceLogA(  "mcThreadProcInfo_toShareBmp: event 0."  );
								break;
							case 1:
								traceLogA("mcThreadProcInfo_toShareBmp: event 1.");
								break;
							default:
								traceLogA("unknown event");
								goto errLabel;
								break;
						}
					}
					*/


					for (; !pCapProcInfo.postThread.bQuit;)
					{

						//
						if (isQ2Empty(pCapProcInfo.postThread.q2))
						{
# if  __DEBUG__
							//  traceLog(  _T(  "mcThreadProc_toShareBmp: break if q is empty"  )  );
#endif
							break;
						}

						nPkts = 0;
						{
							//CQyMalloc mallocObj_tmpPkts[CONST_pkts_toShareVideo_post];
							using (CQyMalloc_array mallocObj_tmpPkts = new CQyMalloc_array(Consts.CONST_pkts_toShareVideo_post)) {

								//  pktLen  =  uiBufSize;  
								nLens = uiLens.Length;// mycountof(uiLens);
								{
									using (CQyQ2Help q2Help = new CQyQ2Help())
									{
										if (0!=qGetMsgPkg(q2Help.getQ_toGetMsg(pCapProcInfo.postThread.q2), (IntPtr)buf_pkts.buf, Consts.bufSize_myDRAW_VIDEO_DATA_pkts_toShareVideo_post, uiLens, (uint*)&nLens))
										{
											break;
										}
									}
								}
								for (i = 0; i < nLens; i++) {
									nPkts += (int)(uiLens[i] / sizeof(myDRAW_VIDEO_DATA));
									if (0!=(uiLens[i] % sizeof(myDRAW_VIDEO_DATA))) {
# if  __DEBUG__
										traceLog(_T("vpp: uiLen err"));
										assert(0);
#endif
									}
								}
								myDRAW_VIDEO_DATA* pkts = (myDRAW_VIDEO_DATA*)buf_pkts.buf;


								tmpbErr = false;    //  注意：这里如果attach出错，要继续attach, 但是最后要continue. 以防止pPkts[i]没有机会释放内存
													//
								for (i = 0; i < nPkts; i++)
								{
									myDRAW_VIDEO_DATA* pPkt = &pkts[i];
									if (IntPtr.Zero!=pPkt->memory.m_pBuf)
									{
										if (0!=mallocObj_tmpPkts.mems[i].attach(ref pPkt->memory))
										{
											traceLogA("mcThreadProc_toShareBmp: mallocObj.attach failed");
											freeMemory(ref pPkt->memory);
											tmpbErr = true;
											continue;
										}
									}
								}
								if (tmpbErr) continue;

								if (0==nPkts)
								{
# if  __DEBUG__
									assert(0);          //  应该不能运行到这里
									traceLog(_T("mcThreadProc_toShareBmp: no pkts read."));
#endif
									break;
								}

								//  traceLogA(  "playVideo: msg reaved, pktLen %d, dataLen %d",  pktLen,  pPkt->head.uiLen  );
								tmpbErr = false;
								for (i = 0; i < nPkts; i++)
								{
									//
#if DEBUG
									ref myDRAW_VIDEO_DATA tmp_pkt = ref pkts[i];
#endif

									//  2014/02/24
									if (pkts[i].usPktResType == Consts.CONST_pktResType_sharedTex)
									{
										continue;
									}
									//
									if (pkts[i].uiSize_dec < pCapProcInfo.compressVideo.vh_decompress.bih.biSizeImage)
									{
#if _DEBUG__
		traceLogA(("toShareVideo_post err: pkt.uiSize_dec %d <  sizeImage %d. skipped"), pkts[i].uiSize_dec, pCapProcInfo.compressVideo.vh_decompress.bih.biSizeImage);
#endif
										str=string.Format("toShareVideo_post err: pkt.uiSize_dec {0} !=  sizeImage {1}. skipped", pkts[i].uiSize_dec, pCapProcInfo.compressVideo.vh_decompress.bih.biSizeImage);
										qyFuncs.showInfo_open(0, null, str);
										
										//
										tmpbErr = true;
										break;
									}
								}
								if (tmpbErr) continue;

# if  __DEBUG__
#if  true
								//  traceLogA(  "mcThreadProc_toShareBmp: pkt.SampleTime %d",  pPkt->head.uiSampleTimeInMs  );				  
								//  traceLog(  _T(  "mcThreadProc_post: nPkts %d"  ),  nPkts  );
								M_pkts_showNo_encVideo_post(  pkts,  nPkts  );
								//
								if  (  pSharedObj.bSlave  )  {
									int  i;
									i  =  0;
								}
#endif
#endif

								SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
								if (null==pSharedObjUsr)
								{
# if  __DEBUG__
									traceLog(_T("mcThread_toShareBmp: pSharedObjUsr is null,  iIndex_sharedObj %d"), iIndex_sharedObj);
#endif
									continue;
								}

								//
								//  myDRAW_VIDEO_DATA  *  pPkt  =  pPkts[0];
								//  

#if  true						
				   		//  if  (  nPkts  !=  1  )  
						{			
							uint	dwTickCnt1	=	myGetTickCount(null  );
							for  (  i  =  0;  i  <  nPkts;  i  ++  )  {							
								 pkts[i].debugInfo.usPostInMs  =  (ushort)(dwTickCnt1  -  pkts[i].debugInfo.dwTickCnt_start);							
							}


						}
#endif

								for (i = 0; i < nPkts; i++)
								{
									pProcInfo.toShareVideo_post(ref ctx.ctxCaller,pCapProcInfo.uiType, pSharedObj.var.iIndex_capBmp, &pkts[i], 1, ref pCapProcInfo.compressVideo, pSharedObj, pMsgBuf);
								}

								//  2014/09/23
								bool bNoDrawForSlave = true;
# if  __DEBUG__	//  for test							
#endif
								if (bNoDrawForSlave)
								{
									if (pSharedObj.var.bSlave)
									{
										continue;
									}
									if (pSharedObj.var.usSubtype == Consts.CONST_sharedObjSubtype_mosaic)
									{
										continue;
									}
								}

								//  
								bool bPktRedirected = false;
								pProcInfo.drawLocalVideoData(pSharedObjUsr.uiTranNo_openAvDev_v, pkts, nPkts * sizeof(myDRAW_VIDEO_DATA), &bPktRedirected, pCapProcInfo.uiType, pSharedObj.var.iIndex_capBmp, pSharedObjUsr.iTaskId, null, null);
								if (bPktRedirected)
								{
									for (i = 0; i < nPkts; i++)
									{
										mallocObj_tmpPkts.mems[i].detach();
									}
								}

								//
# if  __DEBUG__
								if (!bPktRedirected)
								{
									M_pkts_showNo(pkts, nPkts, _T("encVideo_post free"));
								}
#endif

							}
						}

						continue;
					}

					continue;
				}
			}

			

	 traceLogA("mcThreadProc_toShareBmp will quit");

errLabel:

//  2010/12/28
pCapProcInfo.compressVideo.debugInfo.nStep = 199;

//
pCapProcInfo.postThread.bRunning = false;


//traceLogA("mcThreadProc_toShareBmp_post %S leaves", pCapProcInfo.postThread.q2.cfg.name);

return;

}

}
}
