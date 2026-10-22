using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const int MAX_vpp_helpInfos = 24;

}

namespace qy
{
#if false
	unsafe struct TMP_buf_myDRAW_VIDEO_DATA_pkts_vpp
	{
		public fixed byte buf[Consts.bufSize_myDRAW_VIDEO_DATA_pkts_vpp];
	};

	partial class qyFuncs
{
		public static unsafe void CTX_vpp_free(ref CTX_vpp pCtx)
		{
			//  2015/05/07
			int i;
			/*
			for (i = 0; i < pCtx.ucCnt_vppHelpInfos; i++)
			{
				VPP_helpInfo* pMem = &pCtx.pVppHelpInfos[i];
				MACRO_mysafeFree(ref pMem->pBuf);
			}
			MACRO_mysafeFree(ref (IntPtr)pCtx.pVppHelpInfos);
			*/

			//  2013/03/11
			CTX_addTxt_free(ref pCtx.addTxt);
			//  2014/04/24
			MACRO_mysafeFree(ref pCtx.resize.pBuf);

			MACRO_mysafeFree(ref pCtx.vpp.pVppBuf);

			return;
		}

		public static unsafe int rgb24ToVppBuf(byte* rgb, byte* yuv, int width, int height, bool bFlipImg, int iFourcc)
        {
			return -1;
        }

		public static unsafe int chkResizeBuf(ref CTX_vpp pCtx, CAP_procInfo_bmpCommon pCapBmp)
        {
			return -1;
        }

		public static unsafe void mcThreadProc_vpp(object lpParameter)
		{
			int iIndex_sharedObj = (int)lpParameter;

			CCtxQyMc pQyMc = g.g_pQyMc;
			if (null == pQyMc)
			{
				return;
			}
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null == pProcInfo) return;
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;

			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null == pSharedObj) return;

			CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
			if (null == pCapProcInfo) return;

			//  2013/03/10
			CTX_vpp ctx_vpp;
			//memset(&ctx_vpp, 0, sizeof(ctx_vpp));
			ctx_vpp = new CTX_vpp();
			ctx_vpp.iIndex_sharedObj = iIndex_sharedObj;
			ctx_vpp.vppThread1_proc = new QY_qThreadProcInfo_common();
			ctx_vpp.vppThread2_beforeEnc = new QY_qThreadProcInfo_common();
			//
			ctx_vpp.ucbSingleThread_vpp = true;

			//
			string str;

			//
			ref CTX_vpp pCtx = ref ctx_vpp;

			//  2010/12/28
			pCapProcInfo.compressVideo.debugInfo.nStep = 101;

			//  2009/12/24

			//
			uint dwThreadId = myGetCurrentThreadId();

			//myDRAW_VIDEO_DATA pkts[CONST_pkts_vpp];
			TMP_buf_myDRAW_VIDEO_DATA_pkts_vpp buf_pkts;
			uint[] uiLens = new uint[Consts.CONST_pkts_vpp];       //  要增加一个qGetMsgs，将pPkts和pktLens作为参数输入。2011/07/15
			int nLens = uiLens.Length;// mycountof(uiLens);      //  一次最多处理的包数。
			int i;
			int nPkts;                      //								pPkts[]中有几个包是有效的。 2011/07/15

			MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;//->getMisCntByName(_T(""));
			if (null == pMisCnt) return;

			using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc()) {
				MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(sizeof(MIS_MSGU));
				if (null==pMsgBuf) return;


				uint dwTickCnt;

				//  2010/09/18
				//  int							tmpiRet;
				bool tmpbErr;


				//
				//traceLogA("mcThreadProc_vpp %S enters", pCapProcInfo.vppThread.q2.cfg.name);

				//
				pCapProcInfo.vppThread.bQuit = false;
				pCapProcInfo.vppThread.bRunning = true;


				//  2015/05/07
				pCtx.ucCnt_vppHelpInfos = Consts.MAX_vpp_helpInfos;
				int size = pCtx.ucCnt_vppHelpInfos * sizeof(VPP_helpInfo);
				/*
				pCtx.pVppHelpInfos = (VPP_helpInfo*)mymalloc(size);
				if (null==pCtx.pVppHelpInfos) goto errLabel;
				mymemset((IntPtr)pCtx.pVppHelpInfos, 0, size);
				*/



				////////////////
				//  2015/05/06
				GENERIC_Q_CFG tmpCfg;
				//TCHAR tName[128];
				TMP_tBuf128 tName;
				int cntof_tName = Consts.cntof_tBuf128;
				CAP_procInfo_bmpCommon pCapBmp = (CAP_procInfo_bmpCommon)pCapProcInfo;
				GENERIC_Q_cfgEx qCfgEx;
				uint dwThreadDaemonId;

				//memcpy(&tmpCfg, &pQmcCfg->vppQ, sizeof(tmpCfg));
				tmpCfg = pQmcCfg.vppQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s1-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}1-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);

				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s1-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}1-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

				//_sntprintf(tName, mycountof(tName), _T("%s1-%u"), CONST_evtName_qm_vpp, pSharedObj.uiTranNo);
				str=string.Format("{0}1-{1}", Consts.CONST_evtName_qm_vpp, pSharedObj.var.uiTranNo);
				mytcsncpy(tName.tBuf, str, cntof_tName);

				if (ctx_vpp.vppThread1_proc.bQInited)
				{
					traceLog(("startQThreadToShareAv failed, vppThread1_proc.bQInited is  already true."));
					goto errLabel;
				}
				//  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  mycountof(  pCapBmp->vppThread.q2.hEvents  ),  CQyString(  pQyMc.appParams.appObjPrefix  )  +  tName,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pCapBmp->vppThread.q2  )  )  goto  errLabel;
				//memset(&qCfgEx, 0, sizeof(qCfgEx));
				qCfgEx = new GENERIC_Q_cfgEx();
				//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
				qCfgEx.common = tmpCfg;
				qCfgEx.pfQElemNewEx = mallocMemory;
				qCfgEx.pfQElemNew = mymalloc;
				qCfgEx.pfQElemFreeEx = freeMemory;
				qCfgEx.pfQElemFree = myfree;
				qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
				if (0!=initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, new string(tName.tBuf), ctx_vpp.vppThread1_proc.q2)) goto errLabel;
				ctx_vpp.vppThread1_proc.bQInited = true;

				//  2015/05/06
				//memcpy(&tmpCfg, &pQmcCfg->vppQ, sizeof(tmpCfg));
				tmpCfg = pQmcCfg.vppQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s2-%d"), tmpCfg.name, pSharedObj.uiTranNo);
				str=string.Format("{0}2-{1}", new string(tmpCfg.name), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);

				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s2-%d"), tmpCfg.mutexName_prefix, pSharedObj.uiTranNo);
				str=string.Format("{0}2-{1}", new string(tmpCfg.mutexName_prefix), pSharedObj.var.uiTranNo);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);

				//_sntprintf(tName, mycountof(tName), _T("%s2-%u"), CONST_evtName_qm_vpp, pSharedObj.uiTranNo);
				str=string.Format("{0}2-{1}", Consts.CONST_evtName_qm_vpp, pSharedObj.var.uiTranNo);
				mytcsncpy(tName.tBuf, str, cntof_tName);

				if (ctx_vpp.vppThread2_beforeEnc.bQInited)
				{
					traceLog(("startQThreadToShareAv failed, vppThread2_beforeEnc.bQInited is  already true."));
					goto errLabel;
				}
				//  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  mycountof(  pCapBmp->vppThread.q2.hEvents  ),  CQyString(  pQyMc.appParams.appObjPrefix  )  +  tName,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pCapBmp->vppThread.q2  )  )  goto  errLabel;
				//memset(&qCfgEx, 0, sizeof(qCfgEx));
				qCfgEx = new GENERIC_Q_cfgEx();
				//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
				qCfgEx.common = tmpCfg;
				qCfgEx.pfQElemNewEx = mallocMemory;
				qCfgEx.pfQElemNew = mymalloc;
				qCfgEx.pfQElemFreeEx = freeMemory;
				qCfgEx.pfQElemFree = myfree;
				qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
				if (0!=initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, new string(tName.tBuf), ctx_vpp.vppThread2_beforeEnc.q2)) goto errLabel;
				ctx_vpp.vppThread2_beforeEnc.bQInited = true;


#if false
				//  2015/05/06
				if (!ctx_vpp.vppThread1_proc.hThread)
				{
					//
					ctx_vpp.vppThread1_proc.bQuit = false;
					ctx_vpp.vppThread1_proc.hThread = CreateThread(NULL, 0, mcThreadProc_vpp1_proc, (LPVOID) & ctx_vpp, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!ctx_vpp.vppThread1_proc.hThread) goto errLabel;
					ctx_vpp.vppThread1_proc.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(ctx_vpp.vppThread1_proc.hThread) == -1) goto errLabel;
				}

				//  2015/05/06
				if (!ctx_vpp.vppThread2_beforeEnc.hThread)
				{
					//
					ctx_vpp.vppThread2_beforeEnc.bQuit = false;
					ctx_vpp.vppThread2_beforeEnc.hThread = CreateThread(NULL, 0, mcThreadProc_vpp2_beforeEnc, (LPVOID) & ctx_vpp, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!ctx_vpp.vppThread2_beforeEnc.hThread) goto errLabel;
					ctx_vpp.vppThread2_beforeEnc.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(ctx_vpp.vppThread2_beforeEnc.hThread) == -1) goto errLabel;
				}
#endif


				//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "%s, %I64u starts"  ),  pPlay->q2.cfg.name,  pPlayer->idInfo_recorder.ui64Id  );


				/////////////////

				for (; !pCapProcInfo.vppThread.bQuit;)
				{

					//  2010/12/28
					pCapProcInfo.compressVideo.debugInfo.nStep = 105;

					//
#if false
					uint dwEvt = MsgWaitForMultipleObjects(
									  pCapProcInfo.vppThread.q2.cfg.usCnt_hEvents, //  mycountof(  pPlay->hEvents  ),				// Number of events.
									   pCapProcInfo.vppThread.q2.hEvents,                          // Location of handles.
									   false,                                       // Wait for all?
									   5000,                                    // How long to wait?
									   0);                                      // Any message is an 
																				// event.
					if (dwEvt == WAIT_FAILED) goto errLabel;
#endif
					myWaitOne(pCapProcInfo.vppThread.q2.hEvent0, 5000);
					//
					dwTickCnt = myGetTickCount(null);

#if false
					if (dwEvt != WAIT_TIMEOUT)
					{
						dwEvt -= WAIT_OBJECT_0;
						switch (dwEvt)
						{
							case 0:
								//  dwTickCnt_lastData  =  dwTickCnt;		//  audio data received
								//  traceLogA(  "mcThreadProcInfo_vpp: event 0."  );
								break;
							case 1:
								traceLogA("mcThreadProcInfo_vpp: event 1.");
								break;
							default:
								traceLogA("unknown event");
								goto errLabel;
								break;
						}
					}
#endif


					for (; !pCapProcInfo.vppThread.bQuit;)
					{

						//  2011/07/15, 先将pkts清空,nPkts 赋 0
						nPkts = 0;
						{
							//CQyMalloc mallocObj_tmpPkts[CONST_pkts_vpp];
							using (CQyMalloc_array mallocObj_tmpPkts = new CQyMalloc_array(Consts.CONST_pkts_vpp)) {
								myDRAW_VIDEO_DATA* pkts = (myDRAW_VIDEO_DATA*)buf_pkts.buf;

								if (!isQ2Empty(pCapProcInfo.vppThread.q2))
							{       //  这里要将 qGetMsg 改成 qGetMsgs...

									//  pktLen  =  uiBufSize;  
									nLens = uiLens.Length;// mycountof(uiLens);
								//  2014/04/10
								if (0!=q2GetMsgPkg(pCapProcInfo.vppThread.q2, (IntPtr)buf_pkts.buf, Consts.bufSize_myDRAW_VIDEO_DATA_pkts_vpp, uiLens, (uint*)&nLens)) {
									break;
								}

								for (i = 0; i < nLens; i++)
								{
									nPkts += (int)(uiLens[i] / sizeof(myDRAW_VIDEO_DATA));
									if (0!=(uiLens[i] % sizeof(myDRAW_VIDEO_DATA)))
									{
#if __DEBUG__
										traceLog(_T("vpp: uiLen err"));
										assert(0);
#endif
									}
								}

								tmpbErr = false;    //  注意：这里如果attach出错，要继续attach, 但是最后要continue. 以防止pPkts[i]没有机会释放内存
								for (i = 0; i < nPkts; i++)
								{
									if (pkts[i].memory.m_pBuf!=IntPtr.Zero)
									{
										if (0!=mallocObj_tmpPkts.mems[i].attach(ref pkts[i].memory))
										{
											traceLogA("mcThreadProc_vpp: mallocObj.attach failed");
											freeMemory(ref pkts[i].memory);
											tmpbErr = true;
										}
									}
								}
								if (tmpbErr) continue;

							}

							if (0==nPkts)
							{
#if __DEBUG__
							//assert(  0  );			//  应该不能运行到这里
							//traceLog(  _T(  "mcThreadProc_vpp: no pkts read."  )  );
#endif
								break;
							}

							//  traceLogA(  "playVideo: msg reaved, pktLen %d, dataLen %d",  pktLen,  pPkt->head.uiLen  );
							

							tmpbErr = false;
							for (i = 0; i < nPkts; i++)
							{
								if (pkts[i].uiSize_dec < pCapProcInfo.compressVideo.vh_decompress.bih.biSizeImage)
								{
#if __DEBUG__
								traceLogA(("mcThreadProc_vpp err: pkt.uiSize_dec %d <  sizeImage %d. skipped"), pkts[i].uiSize_dec, pCapProcInfo.compressVideo.vh_decompress.bih.biSizeImage);
#endif
									//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("mcThreadProc_vpp err: pkt.uiSize_dec %d !=  sizeImage %d. skipped"), pkts[i].uiSize_dec, pCapProcInfo.compressVideo.vh_decompress.bih.biSizeImage);
									tmpbErr = true;
									break;
								}
							}
							if (tmpbErr) continue;


							//  2016/04/27
							if (pSharedObj.var.bSlave)
							{
									//
									//TCHAR ttBuf[128] = _T("");

									//
										if (0!=setPktInputInfo(pProcInfo, ref pCapProcInfo.compressVideo.input, pCapProcInfo.compressVideo.usFrames_perSecond_expected, false, null))
										{
											//
											//
											continue;
										}

								// 
							}

							//
							SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
							if (null==pSharedObjUsr)
							{
#if __DEBUG__
								traceLog(_T("mcThread_vpp: pSharedObjUsr is null, index_sharedObj %d"), iIndex_sharedObj);
#endif
								continue;
							}

								//
								//GuiShare.pf_dumpData1((IntPtr)pkts[0].memory.m_pBuf, pkts[0].bih.biSizeImage, pkts[0].bih.biWidth, pkts[0].bih.biHeight, "before vpp.yuv");


								//  
								tmpbErr = false;
							for (i = 0; i < nPkts; i++)
							{
								if (pCtx.ucbSingleThread_vpp)
								{
									if (0!=old_doVpp(pProcInfo, ref ctx_vpp, pSharedObj, pCapProcInfo, &pkts[i], sizeof(myDRAW_VIDEO_DATA)))
									{
#if __DEBUG__
										traceLog(_T("doVpp failed"));
#endif
										tmpbErr = true;
										break;
									}
									//
								}
								else
								{  //  把数据格式转成rgb.
									if (0!=doVpp0(pProcInfo, ref ctx_vpp, pSharedObj, pCapProcInfo, &pkts[i], sizeof(myDRAW_VIDEO_DATA)))
									{
#if __DEBUG__
										traceLog(_T("doVpp failed"));
#endif
										tmpbErr = true;
										break;
									}
								}
							}
							if (tmpbErr)
							{
								showInfo_open(0, null, ("doVpp0 failed, skipped"));
								continue;
							}

								//
								//GuiShare.pf_dumpData1((IntPtr)pkts[0].memory.m_pBuf, pkts[0].bih.biSizeImage, pkts[0].bih.biWidth, pkts[0].bih.biHeight, "vpp.yuv");



							//
							bool bPktsRedirected = false;
							//
							if (pCtx.ucbSingleThread_vpp)
							{
								if (0==q2PostMsgAndTrigger((IntPtr)pkts, (uint)(nPkts * sizeof(myDRAW_VIDEO_DATA)), pCapProcInfo.thread.q2))
								{
									bPktsRedirected = true;
								}
								//
							}
							else
							{  //  2015/05/07. 送给vpp1
								if (0==q2PostMsgAndTrigger((IntPtr)pkts, (uint)(nPkts * sizeof(myDRAW_VIDEO_DATA)), pCtx.vppThread1_proc.q2))
								{
									bPktsRedirected = true;
								}
							}
							//
							if (bPktsRedirected)
							{
								for (i = 0; i < nPkts; i++)
								{
									mallocObj_tmpPkts.mems[i].detach();
								}
							}

#if __DEBUG__
							if (!bPktsRedirected)
							{
								M_pkts_showNo(pkts, nPkts, _T("vpp free"));
							}
#endif
						}
					}

					continue;
				}

				continue;
			}

		}

	 traceLogA("mcThreadProc_vpp will quit");

errLabel:

//  2010/12/28
pCapProcInfo.compressVideo.debugInfo.nStep = 199;

//
pCapProcInfo.vppThread.bRunning = false;


//
string tHint = ("");
stopQThread(ctx_vpp.vppThread1_proc, tHint);   //  2015/05/06		
stopQThread(ctx_vpp.vppThread2_beforeEnc, tHint);  //  2015/05/06

//  2015/05/06	
if (ctx_vpp.vppThread1_proc.bQInited)
{
	exitQyQ2(ctx_vpp.vppThread1_proc.q2); ctx_vpp.vppThread1_proc.bQInited = false;
}
if (ctx_vpp.vppThread2_beforeEnc.bQInited)
{
	exitQyQ2(ctx_vpp.vppThread2_beforeEnc.q2); ctx_vpp.vppThread2_beforeEnc.bQInited = false;
}



CTX_vpp_free(ref ctx_vpp);

//
//traceLogA("mcThreadProc_vpp %S leaves", pCapProcInfo.vppThread.q2.cfg.name);

return ;

}


		public static unsafe int chkVppBuf(ref CTX_vpp pCtx, CAP_procInfo_bmpCommon pCapBmp, int iW_vpp, int iH_vpp, bool bNeedVppBuf, int iFourcc)
		{
			int iErr = -1;

			if (0==iW_vpp || 0==iH_vpp) return -1;

			//	
			if (bNeedVppBuf)
			{
				if (null==pCtx.vpp.pVppBuf || pCtx.vpp.bih_vpp.biWidth != iW_vpp || pCtx.vpp.bih_vpp.biHeight != iH_vpp)
				{
					MACRO_mysafeFree(ref pCtx.vpp.pVppBuf);
					//
					switch (iFourcc)
					{
						case Consts.CONST_fourcc_YV12:
							fixed (BITMAPINFOHEADER* p = &pCtx.vpp.bih_vpp)
							{
								makeBmpInfoHeader_yv12(12, iW_vpp, iH_vpp, p);
							}
							break;
						case Consts.CONST_fourcc_I420:
							fixed (BITMAPINFOHEADER* p = &pCtx.vpp.bih_vpp)
							{
								makeBmpInfoHeader_i420(12, iW_vpp, iH_vpp, p);
							}
							break;
						default:
							goto errLabel;
					}


					//
					pCtx.vpp.iVppBufSize = pCtx.vpp.bih_vpp.biSizeImage;
					pCtx.vpp.pVppBuf = (byte*)mymalloc(pCtx.vpp.iVppBufSize);
					if (null==pCtx.vpp.pVppBuf) goto errLabel;
				}
			}
			else
			{
				MACRO_mysafeFree(ref pCtx.vpp.pVppBuf);
			}

			iErr = 0;
		errLabel:
			return iErr;
		}


	}
#endif

}
