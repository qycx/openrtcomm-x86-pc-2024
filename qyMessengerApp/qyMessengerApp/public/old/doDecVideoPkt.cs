using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
#if false
		public static unsafe int doDecVideoPkt(QY_TRANSFORM pTransform, myDRAW_VIDEO_DATA* pPkt, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;
			int i;
			int j;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();   //  QY_GET_procInfo_isCli(  );
			if (null == pProcInfo) return -1;
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;

			MSGR_ADDR* pAddr = pTransform.pAddr_logicalPeer;
			if (null == pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null == pTranInfo_unused) return -1;
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null == pTask) return -1;

			//
			CTX_caller ctxCaller = new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, (CCtxQmc)pProcInfo, pTask.iIndex_taskInfo, "doDecVideoPkt");
			if (null == pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();



			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null == pCusModules)
			{
#if __DEBUG__
				traceLog(_T("doDecV_pkt failed, pCusModules is null"));
#endif
				return -1;
			}


			int tmpiRet;
			//TCHAR tHintBuf[128] = _T("");
			BITMAPINFOHEADER bih_enc;
			bool bDbg = true;
			string str;

			//  2014/08/26
			uint uiTransformType = Consts.CONST_uiTransformType_dec;

			//
			for (i = 0; !pTransform.video.bQuit && i < 1; i++)
			{
				//  myDRAW_VIDEO_DATA  *  pPkt  =  &pkts[i];
				int index_pMems_from = pPkt->decInfo.index_pMems_from;
				int index_activeMems_from = pTransform.index_activeMems_from;      //  2014/11/12

				//		
				if (pPkt->head.uiLen > pPkt->memory.uiBufSize
					|| pPkt->uiSize_dec + pPkt->uiOffset_enc + pPkt->uiLen_enc > pPkt->head.uiLen)
				{
#if __DEBUG__
					traceLog(_T("doDecVideo: uiSize_dec or uiLen error"));
#endif
					return -1;
				}
				byte* encData = (byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec + pPkt->uiOffset_enc;
				myDRAW_VIDEO_DATA* pPkt_dec = pPkt;

#if false
				TASK_AV_FROM pFrom = null;

			pFrom = getTaskAvFrom(pTransform, index_pMems_from, index_activeMems_from, _T("doDecVideoPkt"));
			if (null==pFrom) return -1;
#endif
				//
				if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
				DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


				//
				uint curTickCnt = myGetTickCount(null);

#if __DEBUG__
#endif

				//
				if (0 != pPkt->debugInfo.dwTickCnt_start)
				{
					if (!pTask.ucbLocalSource_rtspStream)
					{
						int iElapse = (int)(curTickCnt - pPkt->debugInfo.dwTickCnt_start);
						if (iElapse > pQmcCfg.policy.usTimeoutInMsToSwitch)
						{

							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%I64u, %dms, skipped."), pFrom.idInfo.ui64Id, iElapse);

							DEBUG_myDRAW_VIDEO_DATA* pDebugPkt = &pPkt->debugInfo;

							pDebugPkt->usTransInMs = (ushort)(curTickCnt - pDebugPkt->dwTickCnt_start);
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s tE %d, e %d, p %d, d %d, o %d, pt %d, tr %d"), tHintBuf, (int)pDebugPkt->usToEncInMs, (int)pDebugPkt->usEncedInMs, (int)pDebugPkt->usPostInMs, (int)pDebugPkt->usToDispatchInMs, (int)pDebugPkt->usOutputInMs, (int)pDebugPkt->usPreTransInMs, (int)pDebugPkt->usTransInMs);

							goto errLabel;
						}
					}
				}

				//
				if (!pTransform.video.ucbInited)
				{
					traceLogA("procTaskVideo failed: video is not inited");
					goto errLabel;
				}

				//
				if (pTransform.video.ucbInited)
				{
					if (pTransform.video.index_activeMems_from != index_activeMems_from
						|| pTransform.video.idInfo.ui64Id != pActiveMem_from.avStream.idInfo.ui64Id
						|| pTransform.video.tv.uiTranNo_openAvDev != pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev
						|| pTransform.video.status.ucbDecFailed                                            //  2012/03/19
						)
					{
						//  2014/04/07
						pTransform.video.status.ucbNeedReinited = true;
						//
						/*
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("decVideoPkt failed: trans.from %d,id %I64u, tn %d, decFailed %d != %d,%I64u,%d,%d"),
							pTransform.video.index_pMems_from, pTransform.video.idInfo.ui64Id, pTransform.video.tv.uiTranNo_openAvDev, (int)pTransform.video.status.ucbDecFailed,
							index_pMems_from, pFrom.idInfo.ui64Id, pFrom.video.tv_recvd.uiTranNo_openAvDev, 0);
						*/
						goto errLabel;
					}
				}



				//  2011/12/14
				if (!pTask.ucbLocalSource_rtspStream)
				{
					if (pPkt->uiSize_dec < pTransform.video.tv.vh_decompress.bih.biSizeImage)
					{
#if DEBUG
						str = string.Format(("doDecVideoPkt err: pkt.uiSize_dec {0} <  sizeImage {1}. skipped"), pPkt->uiSize_dec, pTransform.video.tv.vh_decompress.bih.biSizeImage);
						traceLog(str);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("mcThreadProc_toShareBmp err: pkt.uiSize_dec %d !=  sizeImage %d. skipped"), pPkt->uiSize_dec, pTransform.video.tv.vh_decompress.bih.biSizeImage);
						goto errLabel;
					}
				}

				//  2014/08/07
				if (pTask.saveVideo.ucbSaveVideo_req)
				{
					if (!canSave_fourcc(pTransform.video.tv.vh_compress.bih.biCompression))
					{
						if (myGetTickCount(null) - pTask.saveVideo.dwLastTickCnt_notification > 3000)
						{
							/*
						TCHAR tBuf[128];
						char fourccStr[4 + 1];
						iFourcc2Str(pTransform.video.tv.vh_compress.bih.biCompression, fourccStr, mycountof(fourccStr));
						_sntprintf(tBuf, mycountof(tBuf), _T("%I64u's video format [%s] can't be saved"), pFrom.idInfo.ui64Id, CQyString(fourccStr));
						showNotification(0, 0, 0, 0, 0, 0, tBuf);
							*/
							pTask.saveVideo.dwLastTickCnt_notification = myGetTickCount(null);
						}
					}
				}


				//
				bool bDecompressOk = false;

				//
				pTransform.video.tv.vh_compress.bih.biSizeImage = (int)pPkt->uiLen_enc;     //  pPkt->head.uiLen;
				pTransform.video.tv.vh_decompress.bih.biSizeImage = (int)pPkt->uiSize_dec;  //  pProcInfo.cfg.uiBufSize_myDrawVideoData;	//  mycountof(  pPkt_dec->buf  );

				if (pTransform.video.tv.compressor.ucCompressors == Consts.CONST_videoCompressors_vcm)
				{


					//
					traceLog(_T("vcm is not supported in winCe"));

					//  2009/07/27
					if (!bDecompressOk)
					{       //  ½âÑ¹ËõÊ§°Ü²»ÄÜÍË³ö¡£ÒòÎªÓÐÊ±»áÓÐÊý¾Ý°ü´íµÄÇé¿ö¡£
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s %s"), tHintBuf, _T("Decompress failed."));
						iErr = 0; goto errLabel;
					}


					//
					continue;

				}
				else if (pTransform.video.tv.compressor.ucCompressors == Consts.CONST_videoCompressors_dmo)
				{
					int index_dmo = 0;


					//
					{
						//if  (  decompressVideo_filterGraph(  pDmoInfo,  (  BITMAPINFO  *  )&pTransform.video.tv.vh_compress.bih,  (  BYTE  *  )encData,  pPkt->head.uiSampleTimeInMs,  0  )  )  
						{
#if __DEBUG__
							traceLogA("procTaskVideo: dmoDecompressVideo failed.");
#endif
							//  此标志位表明需要重新初始化transform. 2012/03/19
							pTransform.video.status.ucbDecFailed = true;
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s %s"), tHintBuf, _T("decompressVideo_fg failed."));
							//
							goto errLabel;
						}
					}




					bDecompressOk = true;
					continue;

				}
				//  2014/03/17. Note: 对流来说，不能随意丢包是解开包的重要前提。同时，因为流的特殊性，不能提前知道大小，所以不能用同步函数pf_qdcDecompressVideoAndOutput。所以这里可能需要停止使用了
				else if (true && pTransform.video.uiModuleType_toDec == Consts.CONST_moduleType_ipp)
				{
#if false
					CUS_MODULE_compress pModule = null;
					BITMAPINFOHEADER bih;

					//
					M_getCompressModule(pCusModules, pTransform.video.uiModuleType_toDec, ref pModule);

					//				 
					if (null==pModule
						|| null==pModule.pf_qdcDecompressVideoAndOutput)
					{
#if __DEBUG__
						traceLog(_T("pf_qdcDecompressVideoAndOutput not ok"));
#endif
						goto errLabel;
					}

					int index_dmo = 0;

					M_setMyStep(ref pTransform.video.debugStep, 61);

					//  QDC_OBJ_INFO	*	pDmoInfo	=  MACRO_getDmo(  pTransform.video.u.custom.usCnt,  pTransform.video.u.custom.qdcObjInfos,  index_dmo  );
					ref QOI_common pDmoInfo = ref pTransform.video.u_qoi.myQoi.common;
					//if (!pDmoInfo) goto errLabel;
					//pDmoInfo.var.pDebugStep = &pTransform.video.debugStep;

					//  20134/02/26
					//  if  (  pTransform.video.ucCompressors  ==  CONST_videoCompressors_stream  )  
					{
						if (!pTransform.video.status.ucbDecOk)
						{
							//
#if __DEBUG__
#if true
										   char  fourccStr[4  +  1];
										   iFourcc2Str(  pTransform.video.tv.vh_stream.dwFourcc,  fourccStr,  mycountof(  fourccStr  )  );
										   traceLog(  _T(  "fourcc %S"  ),  fourccStr  );
#endif
#endif
							//
							if (pTransform.video.tv.vh_stream.dwFourcc == Consts.CONST_fourcc_h264)
							{
								//  将sps,pps输入解码
								int j;
								for (j = 0; j < pTransform.video.tv.vh_stream.usCnt; j++)
								{
									if (j >= Consts.cntof_VH_stream_mems) break;
									//
									VH_stream_mem tmp_mem;
									VH_stream_mem* pVh_stream_mem = &tmp_mem;
									switch (j)
                                    {
										case 0:
											tmp_mem = pTransform.video.tv.vh_stream.mem0;
											break;
										case 1:
											tmp_mem=pTransform.video.tv.vh_stream.mem1;
											break;
										case 2:
											tmp_mem = pTransform.video.tv.vh_stream.mem2;
											break;
									}
									//
									BITMAPINFOHEADER bih_enc;
									//memcpy(&bih_enc, &pTransform.video.tv.vh_compress.bih, sizeof(bih_enc));
									bih_enc = pTransform.video.tv.vh_compress.bih;
									bih_enc.biSizeImage = pVh_stream_mem->usLen;
									//
									makeBmpInfoHeader_yv12(12, pTransform.video.tv.vh_decompress.bih.biWidth, pTransform.video.tv.vh_decompress.bih.biHeight, &bih);
									if (bih.biSizeImage > pPkt_dec->uiSize_dec)
									{
#if __DEBUG__
										traceLog(_T("doDecVideo err: bih.biSizeImg %d > uiSize_dec %d"), bih.biSizeImage, pPkt_dec->uiSize_dec);
#endif
										//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("doDecVideo err: bih.biSizeImg %d > uiSize_dec %d"), bih.biSizeImage, pPkt_dec->uiSize_dec);
									}
									//
									bih.biSizeImage = (int)Math.Min(bih.biSizeImage, pPkt_dec->uiSize_dec);
									//
#if __DEBUG__
									if (pQmcCfg->debugStatusInfo.ucb__Dump_h264_dec__)
									{
										static BOOL bReset = FALSE;
										char* video = pTransform.video.tv.vh_stream.mems[j].spsPps;
										int len = pTransform.video.tv.vh_stream.mems[j].usLen;
										dumpVideo_init(&bReset, NULL, _T(CONST_fn_dump_h264_dec), video, len);
									}
#endif
									//  2014/08/04
									if (pTask.saveVideo.ucbSaveVideo_req)
									{
										//
										byte* video = pVh_stream_mem->spsPps;
										int len = pVh_stream_mem->usLen;
										postVPktToSave(Consts.CONST_uiTransformType_dec, pTransform, index_pMems_from, 0, 0, video, len);
									}
									//
									tmpiRet = pModule.pf_qdcDecompressVideoAndOutput(pDmoInfo, (BITMAPINFOHEADER*)&bih_enc, (byte*)pVh_stream_mem->spsPps, (BITMAPINFO*)&bih, (BYTE*)pPkt_dec->memory.m_pBuf);
									if (tmpiRet < 0)
									{
										if (tmpiRet == Consts.CONST_qdcRet_exception)
										{   //  2014/08/11
											pTransform.video.status.ucbDecFailed = TRUE;
											showInfo_open(0, 0, _T("decV returns exception"));
											showNotification(0, 0, 0, 0, 0, 0, _T("decV exception"));
											goto errLabel;
										}
										if (tmpiRet != Consts.CONST_qdcRet_bufTooSmall)
										{
#if __DEBUG__
											traceLogA("doDecVideo: qdcDecompressVideo spsPps failed.");
#endif
											goto errLabel;
										}
										showInfo_open(0, 0, _T("decV returns bufTooSmall"));
									}
									//
									if (0!=bih_enc.biWidth && 0!=bih_enc.biHeight)
									{
										//
										if (0!=pTransform.video.tv.vh_compress.bih.biWidth && 0!=pTransform.video.tv.vh_compress.bih.biHeight)
										{
											if (pTransform.video.tv.vh_compress.bih.biWidth != bih_enc.biWidth)
											{
												goto errLabel;
											}
											if (pTransform.video.tv.vh_compress.bih.biHeight != bih_enc.biHeight)
											{
												goto errLabel;
											}
										}
										else
										{
											pTransform.video.tv.vh_compress.bih.biWidth = bih_enc.biWidth;
											pTransform.video.tv.vh_compress.bih.biHeight = bih_enc.biHeight;
										}
										//
										if (0!=pTransform.video.tv.vh_decompress.bih.biWidth && 0!=pTransform.video.tv.vh_decompress.bih.biHeight)
										{
											if (pTransform.video.tv.vh_decompress.bih.biWidth != bih_enc.biWidth)
											{
												goto errLabel;
											}
											if (pTransform.video.tv.vh_decompress.bih.biHeight != bih_enc.biHeight)
											{
												goto errLabel;
											}
										}
										else
										{
											pTransform.video.tv.vh_decompress.bih.biWidth = bih_enc.biWidth;
											pTransform.video.tv.vh_decompress.bih.biHeight = bih_enc.biHeight;
										}
									}
								}
							}
						}
					}

					makeBmpInfoHeader_yv12(12, pTransform.video.tv.vh_decompress.bih.biWidth, pTransform.video.tv.vh_decompress.bih.biHeight, &bih);
					if (bih.biSizeImage > pPkt_dec->uiSize_dec)
					{
#if __DEBUG__
						traceLog(_T("doDecVideo failed: bih.biSizeImage %d > pkt_dec.uiSize_dec %d"), bih.biSizeImage, pPkt_dec->uiSize_dec);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("doDecVideo err: bih.biSizeImg %d > uiSize_dec %d"), bih.biSizeImage, pPkt_dec->uiSize_dec);
					}
					bih.biSizeImage = (int)Math.Min(bih.biSizeImage, pPkt_dec->uiSize_dec);
#if __DEBUG__
					if (pQmcCfg->debugStatusInfo.ucb__Dump_h264_dec__)
					{
						char* video = encData;
						int len = pTransform.video.tv.vh_compress.bih.biSizeImage;
						dumpVideo(NULL, _T(CONST_fn_dump_h264_dec), video, len);
					}
#endif
					//  2014/08/04
					if (pTask.saveVideo.ucbSaveVideo_req)
					{
						//
						byte* video = encData;
						int len = pTransform.video.tv.vh_compress.bih.biSizeImage;
						postVPktToSave(Consts.CONST_uiTransformType_dec, pTransform, index_pMems_from, pPkt->head.uiSampleTimeInMs, pPkt->head.uiPts, video, len);
					}
					//						
					tmpiRet = pModule.pf_qdcDecompressVideoAndOutput(ref pDmoInfo, ref pTransform.video.tv.vh_compress.bih, (IntPtr)encData, ref bih, (IntPtr)pPkt_dec->memory.m_pBuf);
					if (tmpiRet < 0)
					{
						if (tmpiRet == Consts.CONST_qdcRet_exception)
						{   //  2014/08/11								   
							pTransform.video.status.ucbDecFailed = true;
							showInfo_open(0, 0, _T("decV returns exception"));
							showNotification(0, 0, 0, 0, 0, 0, _T("decV exception"));
							goto errLabel;
						}
						if (tmpiRet != Consts.CONST_qdcRet_bufTooSmall)
						{
#if __DEBUG__
							traceLogA("procTaskVideo: dmoDecompressVideo failed.");
#endif
							goto errLabel;
						}
						showInfo_open(0, 0, _T("decV returns bufTooSmall"));
					}
					if (tmpiRet <= 0) continue;
					pTransform.video.status.ucbDecOk = true;

					M_setMyStep(ref pTransform.video.debugStep, 171);

					//
					ushort usIndex = (ushort)index_activeMems_from;    //  pPkt->head.usIndex;
																		//  
					if (0!=chkFromPlayerIndex_v(uiTransformType, pTransform, pPkt, null)) continue;

					//
#if __DEBUG__
					//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  (  ""  ),  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "transformTaskVideo: decompress %d -> %d"  ),  pTransform.video.vh_compress.bih.biSizeImage,  pTransform.video.vh_decompress.bih.biSizeImage  );
#endif

					//
					if (0==bih.biSizeImage) continue;

					//  2009/05/27
					//memcpy(&pPkt_dec->bih, &bih, sizeof(pPkt_dec->bih)  );
					pPkt_dec->bih= bih;

			//
			M_setMyStep(ref pTransform.video.debugStep, 175);

					bDecompressOk = true;
					continue;
#endif
					traceLog("ipp is not supported here");
					goto errLabel;
				}
				else
				{
					CUS_MODULE_compress pModule = null;
					BITMAPINFOHEADER bih;
					//
					M_getCompressModule(pCusModules, pTransform.video.uiModuleType_toDec, ref pModule);
					switch (pTransform.video.uiModuleType_toDec)
					{
						case Consts.CONST_moduleType_ipp:
						case Consts.CONST_moduleType_mediaSdk_dec:
							makeBmpInfoHeader_yv12(12, pTransform.video.tv.vh_decompress.bih.biWidth, pTransform.video.tv.vh_decompress.bih.biHeight, &bih);
							break;
						case Consts.CONST_moduleType_compress:
							makeBmpInfoHeader_rgb(24, pTransform.video.tv.vh_decompress.bih.biWidth, pTransform.video.tv.vh_decompress.bih.biHeight, &bih);
							break;
						case Consts.CONST_moduleType_ffmpeg:
							makeBmpInfoHeader_i420(12, pTransform.video.tv.vh_decompress.bih.biWidth, pTransform.video.tv.vh_decompress.bih.biHeight, &bih);
							break;
						default:
#if DEBUG
							str = string.Format("doDecVideoPkt: moduleType_toDec error, %{0}", pTransform.video.uiModuleType_toDec);
							traceLogA(str);
#endif
							//
							goto errLabel;
					}

					//				 
					if (null == pModule
						|| null == pModule.pf_qdcDecompressVideo
						|| null == pModule.pf_qdcGetOutputByIndex) goto errLabel;

					int index_dmo = 0;

					M_setMyStep(ref pTransform.video.debugStep, 61);

					//  QDC_OBJ_INFO	*	pDmoInfo	=  MACRO_getDmo(  pTransform.video.u.custom.usCnt,  pTransform.video.u.custom.qdcObjInfos,  index_dmo  );
					ref QOI_common pDmoInfo = ref pTransform.video.u_qoi.myQoi.common;
					//if (null==pDmoInfo) goto errLabel;
					//pDmoInfo.var.pDebugStep = &pTransform.video.debugStep;


					//  20134/02/26
					//if (pTransform.video.tv.compressor.common.ucCompressors == Consts.CONST_videoCompressors_stream)
					{
						if (!pTransform.video.status.ucbSpsPpsImported)
						{
							if (pTransform.video.tv.vh_stream.dwFourcc == Consts.CONST_fourcc_h264)
							{
								//  将sps,pps输入解码
								//int j;
								for (j = 0; j < pTransform.video.tv.vh_stream.usCnt; j++)
								{
									if (j >= Consts.cntof_VH_stream_mems) break;
									//
									VH_stream_mem tmpMem;
									VH_stream_mem* pVh_stream_mem = &tmpMem;
									switch (j)
									{
										case 0:
											tmpMem = pTransform.video.tv.vh_stream.mem0;
											break;
										case 1:
											tmpMem = pTransform.video.tv.vh_stream.mem1;
											break;
										case 2:
											tmpMem = pTransform.video.tv.vh_stream.mem2;
											break;
										default:
											goto errLabel;
									}
									//
									if (0 == pVh_stream_mem->usLen) continue;
									//
									//memcpy(&bih_enc, &pTransform.video.tv.vh_compress.bih, sizeof(bih_enc));
									bih_enc = pTransform.video.tv.vh_compress.bih;
									bih_enc.biSizeImage = pVh_stream_mem->usLen;
									//
#if DEBUG
									if (pQmcCfg.debugStatusInfo.ucb__Dump_h264_dec__)
									{
										if (!pTransform.video.status.bDumpStarted)
										{
											GuiShare.pf_dumpData1(IntPtr.Zero, 0, 0, 0, false, false, Consts.CONST_fn_dump_h264_dec);
											pTransform.video.status.bDumpStarted = true;
										}
										byte* video = pVh_stream_mem->spsPps;
										int len = pVh_stream_mem->usLen;
										GuiShare.pf_dumpData1((IntPtr)video, len, 0, 0, true, false, Consts.CONST_fn_dump_h264_dec);
									}
#endif

									//
									if (pModule.pf_qdcDecompressVideo(ref pDmoInfo, ref bih_enc, (IntPtr)pVh_stream_mem->spsPps, (int)pPkt->head.uiSampleTimeInMs) < 0)
									{
#if __DEBUG__
									traceLogA("doDecVideo: qdcDecompressVideo spsPps failed.");
#endif
										showInfo_open(null, null, _T("decV spspps failed"));
										goto errLabel;
									}
									//
									if (0 != bih_enc.biWidth && 0 != bih_enc.biHeight)
									{
										//
										if (0 != pTransform.video.tv.vh_compress.bih.biWidth && 0 != pTransform.video.tv.vh_compress.bih.biHeight)
										{
											if (pTransform.video.tv.vh_compress.bih.biWidth != bih_enc.biWidth)
											{
												goto errLabel;
											}
											if (pTransform.video.tv.vh_compress.bih.biHeight != bih_enc.biHeight)
											{
												goto errLabel;
											}
										}
										else
										{
											pTransform.video.tv.vh_compress.bih.biWidth = bih_enc.biWidth;
											pTransform.video.tv.vh_compress.bih.biHeight = bih_enc.biHeight;
										}
										//
										if (0 != pTransform.video.tv.vh_decompress.bih.biWidth && 0 != pTransform.video.tv.vh_decompress.bih.biHeight)
										{
											if (pTransform.video.tv.vh_decompress.bih.biWidth != bih_enc.biWidth)
											{
												goto errLabel;
											}
											if (pTransform.video.tv.vh_decompress.bih.biHeight != bih_enc.biHeight)
											{
												goto errLabel;
											}
										}
										else
										{
											pTransform.video.tv.vh_decompress.bih.biWidth = bih_enc.biWidth;
											pTransform.video.tv.vh_decompress.bih.biHeight = bih_enc.biHeight;
										}


									}

								}

							}

							//
							pTransform.video.status.ucbSpsPpsImported = true;
						}

					}

#if DEBUG
					if (pQmcCfg.debugStatusInfo.ucb__Dump_h264_dec__)
					{
						//
						byte* video = encData;
						int len = pTransform.video.tv.vh_compress.bih.biSizeImage;
						//
						if (bDbg)
						{
							str = string.Format("doDecVideoPkt: idInfo {0}, pts {1}, len {2}", pPkt->head.idInfo.ui64Id, pPkt->head.uiPts, len);
							traceLog(str);
						}
						//
						GuiShare.pf_dumpData1((IntPtr)video, len, 0, 0, true, false, Consts.CONST_fn_dump_h264_dec);
					}

#endif

					//memcpy(&bih_enc, &pTransform.video.tv.vh_compress.bih, sizeof(bih_enc));
					bih_enc = pTransform.video.tv.vh_compress.bih;

					//
					if ((tmpiRet = pModule.pf_qdcDecompressVideo(ref pDmoInfo, ref bih_enc, (IntPtr)encData, (int)pPkt->head.uiSampleTimeInMs)) < 0)
					{
						traceLogA("procTaskVideo: dmoDecompressVideo failed.");
						showInfo_open(0, null, _T("decV failed"));
						goto errLabel;
					}
					//  if  (  !tmpiRet  )  continue;
					//				   										   
					if (0 != bih_enc.biWidth && 0 != bih_enc.biHeight)
					{
						//
						if (0 != pTransform.video.tv.vh_compress.bih.biWidth && 0 != pTransform.video.tv.vh_compress.bih.biHeight)
						{
							if (pTransform.video.tv.vh_compress.bih.biWidth != bih_enc.biWidth)
							{
								goto errLabel;
							}
							if (pTransform.video.tv.vh_compress.bih.biHeight != bih_enc.biHeight)
							{
								goto errLabel;
							}
						}
						else
						{
							pTransform.video.tv.vh_compress.bih.biWidth = bih_enc.biWidth;
							pTransform.video.tv.vh_compress.bih.biHeight = bih_enc.biHeight;
						}
						//
						if (0 != pTransform.video.tv.vh_decompress.bih.biWidth && 0 != pTransform.video.tv.vh_decompress.bih.biHeight)
						{
							if (pTransform.video.tv.vh_decompress.bih.biWidth != bih_enc.biWidth)
							{
								goto errLabel;
							}
							if (pTransform.video.tv.vh_decompress.bih.biHeight != bih_enc.biHeight)
							{
								goto errLabel;
							}
						}
						else
						{
							pTransform.video.tv.vh_decompress.bih.biWidth = bih_enc.biWidth;
							pTransform.video.tv.vh_decompress.bih.biHeight = bih_enc.biHeight;
						}
					}

					//
					M_setMyStep(ref pTransform.video.debugStep, 171);

					//
					if (!pTask.ucbLocalSource_rtspStream)
					{
						ushort usIndex = (ushort)index_activeMems_from;    //  pPkt->head.usIndex
																		   //  
						if (0 != chkFromPlayerIndex_v(uiTransformType, pTransform, pPkt, null))
						{
							int ii = 1;
							continue;
						}
					}
					//
					for (j = 0; ; j++)
					{

						M_setMyStep(ref pTransform.video.debugStep, 172);


						//
						pTransform.video.tv.vh_decompress.bih.biSizeImage = (int)pPkt->uiSize_dec;  //  pProcInfo.cfg.uiBufSize_myDrawVideoData;	//  mycountof(  pPkt_dec->buf  );
						uint uiDataSize = (uint)pPkt_dec->uiSize_dec;// pTransform.video.tv.vh_decompress.bih.biSizeImage;
						uint uiSampleTimeInMs = 0;
						uint rt = 0;
						if (0 != pModule.pf_qdcGetOutputByIndex(ref pDmoInfo, j, pPkt_dec->memory.m_pBuf, ref uiDataSize, ref uiSampleTimeInMs, ref rt))
						{
							break;
						}
						pTransform.video.tv.vh_decompress.bih.biSizeImage = (int)bih.biSizeImage;// uiDataSize;

						//
						pTransform.video.status.ucbDecOk = true;

						//
#if DEBUG
						//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  (  ""  ),  pProcInfo.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "transformTaskVideo: decompress %d -> %d"  ),  pTransform.video.vh_compress.bih.biSizeImage,  pTransform.video.vh_decompress.bih.biSizeImage  );
#endif



						//			
						if (0 == pTransform.video.tv.vh_decompress.bih.biSizeImage) continue;

						//  2009/05/27


						//memcpy(&pPkt_dec->bih, &bih, sizeof(pPkt_dec->bih)  );
						pPkt_dec->bih = bih;
						if (pPkt_dec->bih.biSizeImage > uiDataSize)
						{
							traceLog("doDecVideoPkt: uiDataSize too small, not a corrent pkt");
							pPkt_dec->bih.biSizeImage = 0;
						}
						else
						{
							int jj = 0;
							//
#if DEBUG
							//GuiShare.pf_dumpData1(pPkt_dec->memory.m_pBuf, (int)uiDataSize, bih.biWidth, bih.biHeight, false, true, "doDecVideoPkt.yuv");
#endif

						}

						//
						M_setMyStep(ref pTransform.video.debugStep, 175);


						//	drawVideoData(  pProcInfo,  pTransferVideoData->mems[i].uiSampleTimeInMs,  (  BITMAPINFO  *  )&bih,  (  BYTE  *  )pPkt_dec->buf,  pPkt_dec->head.uiLen,  pTransform.iIndex_player  );
						continue;

					}


					bDecompressOk = true;
					continue;

				}


			}

			iErr = 0;
		errLabel:

			M_setMyStep(ref pTransform.video.debugStep, 181);


			if (0 != iErr)
			{
				uint uiTranNo = 0;
				//qyShowInfo1(CONST_qyShowType_qwmComm,  0,  "", pProcInfo.who_showInfo,  0, _T(  ""  ),  _T(  ""  ),  _T(  "tn %d, %s. %s"  ),  uiTranNo,  (iErr? _T(  "failed"  )  :  _T(  "OK"  )  ),  tHintBuf  );
#if __DEBUG__
				traceLog(_T(  "transformTaskVideo: %s"  ),  tHintBuf  );
#endif
			}

			M_setMyStep(ref pTransform.video.debugStep, 199);

			return iErr;
		}
#endif

	}
}
