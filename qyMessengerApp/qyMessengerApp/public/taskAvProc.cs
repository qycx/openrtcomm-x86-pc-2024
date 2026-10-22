using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int adjustModuleType_decV( ref uint uiModuleType)
        {
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


			//
			switch (uiModuleType)
			{
				case Consts.CONST_moduleType_mediaSdk_dec:
				case Consts.CONST_moduleType_ipp:
					//
					if (pProcInfo.cfg.ucb_USE_Fragment_dlgTalk_av)
					{
						uiModuleType = Consts.CONST_moduleType_ffmpeg;
					}
					else
					{
						uiModuleType = Consts.CONST_moduleType_mediaCodec;
					}
					break;
				default:
					break;
			}

			return 0;
		}

		public static unsafe int adjustModuleType_encV( ref uint uiModuleType)
        {
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            switch (uiModuleType)
            {
                case Consts.CONST_moduleType_ffmpeg:
					 if (!pProcInfo.cfg.ucb_USE_Fragment_dlgTalk_av)
                    {
						uiModuleType = Consts.CONST_moduleType_mediaCodec;
                    }
                    break;
				default:
					break;
			}

			return 0;
        }


		//

		public static unsafe int adjustModuleType_decA(ref uint uiModuleType)
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

#if true
			//
			switch (uiModuleType)
			{
				case Consts.CONST_moduleType_ffmpeg:
					{ 
						uiModuleType = Consts.CONST_moduleType_mediaCodec;
					}
					break;
				default:
					break;
			}
#endif

			return 0;
		}



		//  
		public static unsafe int initTaskAvFrom_video(CCtxQmc pProcInfo, DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from, int index_activeMems_from, int iTaskId, uint uiModuleType_suggested, bool bUseDecTool_suggested, QY_TRANSFORM pTransform)
		{
			int iErr = -1;

			

			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			//
			//TCHAR tHintBuf[256] = _T("");
			int tmpiRet = -1;
			int i;
			//TCHAR tBuf[128];

			string str;

			//
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;

			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null==pCusModules) return -1;

			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask)
			{  //  2014/08/04
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initTaskAvFrom_video failed: pTask is null."));
				return -1;
			}
			TRANSFORM_VIDEO_procInfo pTransformVideo = pTransform.video;

		
			//
#if __DEBUG__
			_sntprintf(tBuf, mycountof(tBuf), _T("initTaskAvFrom_video: %I64u enters"), pFrom.idInfo.ui64Id);
			//
			char buf[128];
#if true
				iFourcc2Str(  pFrom.video.tv_recvd.vh_stream.dwFourcc,  buf,  mycountof(  buf  )  );
				_sntprintf(  tBuf,  mycountof(  tBuf ),  _T(  "%s vh_stream.%S"  ),  tBuf,  buf  );
#else
			iFourcc2Str(pFrom.video.tv_recvd.vh_compress.bih.biCompression, buf, mycountof(buf));
			_sntprintf(tBuf, mycountof(tBuf), _T("%s vh_compress.%S"), tBuf, buf);
#endif
			//
			traceLog(tBuf);
			showInfo_open(0, 0, tBuf);
#endif

			//  2012/03/19
			M_setMyStep(ref pTransformVideo.debugStep, 31);

			//
			if (pTransformVideo.ucbInited)
			{   //  2009/05/29
				traceLogA("initTaskAvFrom_video failed: already inited.");
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initTaskAvFrom_video failed: already inited."));
				goto errLabel;
			}

			if (!pTransformVideo.ucbInited)
			{
				//  DWORD				biSizeImage;

#if __DEBUG__   //  for test. 2015/01/15
				//pTransformVideo.decInfo.pts.uiPts_lastRecvd  =  99;
#endif

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
                            //  _sntprintf(  mutexName,  mycountof(  mutexName  ),  _T(  "%s%d"  ),  pProcInfo->cfg_mutexName_syncActiveMem_prefix,  iTaskId  );
                            //M_getMutexName_syncActiveMem(ref pQmcCfg, iTaskId, mutexName.tBuf, cntof_mutexName);
                            M_getMutexName_syncActiveMem(ref pQmcCfg, iTaskId, ref mutexName);

                            if (0 != syncObj.sync(new string(mutexName.tBuf), pMisCnt.hMutex_syncActiveMem, 1000)) goto errLabel;

							pTransformVideo.idInfo.ui64Id = pActiveMem_from.avStream.idInfo.ui64Id;
							pTransformVideo.tv.uiTranNo_openAvDev = pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev;
							//
							//pTransformVideo.tv.compressor.ucCompressors = pFrom.video.tv_recvd.compressor.ucCompressors;
							//pTransformVideo.tv.compressor.usMaxFps_toShareBmp = pFrom.video.tv_recvd.compressor.usMaxFps_toShareBmp;        //  2014/08/07
							pTransformVideo.tv.compressor = pActiveMem_from.avStream.obj.tranInfo.video.compressor;
																																						   //pTransformVideo.tv.compressor.common.ucbOutputFlippedImg  =  pFrom.video.tv_recvd.compressor.common.ucbOutputFlippedImg;		//  2011/12/11
																																						   //
																																						   //memcpy(&pTransformVideo.tv.vh_compress, &pFrom.video.tv_recvd.vh_compress, sizeof(pTransformVideo.tv.vh_compress)  );
							pTransformVideo.tv.vh_compress = pActiveMem_from.avStream.obj.tranInfo.video.vh_compress;

							//  2014/02/25
							//memcpy(&pTransformVideo.tv.vh_stream, &pFrom.video.tv_recvd.vh_stream, sizeof(pTransformVideo.tv.vh_stream)  );
							pTransformVideo.tv.vh_stream = pActiveMem_from.avStream.obj.tranInfo.video.vh_stream;

							if (0==pTransformVideo.tv.vh_stream.dwFourcc)
							{
								pTransformVideo.tv.vh_stream.dwFourcc = (uint)pActiveMem_from.avStream.obj.tranInfo.video.vh_compress.bih.biCompression;
#if __DEBUG__
							traceLog(_T("vh_stream.dwFourcc is set to vh_compress_recvd"));
#endif
							}

							//  2014/09/23
							if (bSupported_sendVhDec())
							{
								//memcpy(&pTransformVideo.tv.vh_decompress, &pFrom.video.tv_recvd.vh_decompress, sizeof(pTransformVideo.tv.vh_decompress));
								pTransformVideo.tv.vh_decompress = pActiveMem_from.avStream.obj.tranInfo.video.vh_decompress;
							}
							
							{  //  2014/09/23. 用vh_compress的信息来填充vh_decompress
								switch (pTransformVideo.tv.compressor.ucCompressors)
								{
									case Consts.CONST_videoCompressors_vcm:
									case Consts.CONST_videoCompressors_dmo:
										fixed (BITMAPINFOHEADER* pbih = &pTransformVideo.tv.vh_decompress.bih)
										{
											makeBmpInfoHeader_rgb(24, pTransformVideo.tv.vh_compress.bih.biWidth, pTransformVideo.tv.vh_compress.bih.biHeight, pbih);
										}
										break;
									default:
                                        switch (pTransformVideo.tv.compressor.iFourcc)
                                        {
											case Consts.CONST_fourcc_h264:
											case Consts.CONST_fourcc_HEVC:
												fixed (BITMAPINFOHEADER* pbih = &pTransformVideo.tv.vh_decompress.bih) {
													makeBmpInfoHeader_i420(12, pTransformVideo.tv.vh_decompress.bih.biWidth, pTransformVideo.tv.vh_decompress.bih.biHeight, pbih);
												}
												break;
											default:
												break;
                                        }
										break;
								}
							}


							//  2014/08/05
							pTransformVideo.dwLastmodifiedTickCnt_keyParams = myGetTickCount(null);
						}
					}

					//
					int tttiW = pTransformVideo.tv.vh_decompress.bih.biWidth;
					int ii = 0;


					//  2014/08/30
					/*
					char fourcc[4 + 1] = "";
					iFourcc2Str(pTransformVideo.tv.vh_stream.dwFourcc, fourcc, mycountof(fourcc));
					_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%sfourcc %S."), tHintBuf, fourcc);
					if (pTransformVideo.tv.vh_stream.usCnt)
					{
						_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%sspspps [cnt %d] %d,%d."), tHintBuf, (int)pTransformVideo.tv.vh_stream.usCnt, (int)pTransformVideo.tv.vh_stream.mems[0].usLen, (int)pTransformVideo.tv.vh_stream.mems[1].usLen);
					}
					*/


				}
				catch (Exception e)
				{
					//e->Delete();
					goto errLabel;
				}

				//
				//memset(&pTransformVideo.debugStep, 0, sizeof(pTransformVideo.debugStep)  );   //  2014/08/11
				fixed( MY_DEBUG_STEP * pStep = &pTransformVideo.debugStep)
                {
					mymemset((IntPtr)pStep, 0, sizeof(MY_DEBUG_STEP));
                }
				{
                    int tttiW = pTransformVideo.tv.vh_decompress.bih.biWidth;
                    int ii = 0;

                }
                //memset(&pTransformVideo.status, 0, sizeof(pTransformVideo.status)  );
                fixed (TRANSFORM_VIDEO_procInfo_status * pstatus = &pTransformVideo.status)
                {
					mymemset((IntPtr)pstatus, 0, sizeof(TRANSFORM_VIDEO_procInfo_status));

                }
				{
					int tttiW = pTransformVideo.tv.vh_decompress.bih.biWidth;
					int ii = 0;
				}
				//  2015/01/15
				//memset(&pTransformVideo.decInfo, 0, sizeof(pTransformVideo.decInfo)  );
				/*
                fixed (TRANSFORM_VIDEO_procInfo_decInfo * pdecInfo = &pTransformVideo.decInfo)
                {
					mymemset((IntPtr)pdecInfo, 0, sizeof(TRANSFORM_VIDEO_procInfo_decInfo));
                }
				*/
				pTransformVideo.decInfo.memset0();
				{

					int tttiW = pTransformVideo.tv.vh_decompress.bih.biWidth;
					int ii = 0;
				}
                //  2012/03/19
                M_setMyStep(ref pTransformVideo.debugStep, 33);


				//
				if (0==pActiveMem_from.avStream.obj.tranInfo.video.compressor.ucCompressors) pTransformVideo.tv.compressor.ucCompressors = Consts.CONST_videoCompressors_vcm;       //  2009/03/30

				//  2014/0325
				//memset(&pTransformVideo.u, 0, sizeof(pTransformVideo.u)  );
				pTransformVideo.u_qoi.myQoi.common.memset0();


                //
#if DEBUG
				//showInfo_open(  0,  0,  _T(  "initTaskAvFrom_video ...8885"  )  );
				{

					int tttiW = pTransformVideo.tv.vh_decompress.bih.biWidth;
					int ii = 0;
				}
                
#endif

                //
                if (pTransformVideo.tv.compressor.ucCompressors == Consts.CONST_videoCompressors_vcm)
				{
					/*
					DWORD fccHandler = 0;
					int iFormatSize = 0;
					BITMAPINFO* pBmpInfo = NULL;

					//
					if (!pFrom.video.tv_recvd.compressor.common.fourccStr[0])
					{
						traceLogA("tmpHandler_findOrgMsgAndProc_is err . no fourccStr");
						_sntprintf(tHintBuf, mycountof(tHintBuf), _T(" no fourcc"));
						goto errLabel;
					}
					fccHandler = fourccStr2i((char*)pFrom.video.tv_recvd.compressor.common.fourccStr);
					*/
					traceLog(_T("vcm is not supported in winCe"));
					goto errLabel;
				}
				else if (pTransformVideo.tv.compressor.ucCompressors == Consts.CONST_videoCompressors_dmo)
				{


					{
						//if  (  initDecompressVideo_filterGraph(  &pFrom.video.tv_recvd.compressor,  &pTransformVideo.tv.vh_compress,  &pTransformVideo.tv.vh_decompress,  &pTransformVideo.debugStep,  &pTransformVideo.u.dmo  )  )  
						{
							str=string.Format("initTaskAvFrom_video: initDecVideo_dmo not supported.");
							traceLog(str);
							showNotification(0, 0, 0, 0, 0, 0, str);
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s initDecompressVideo_filterGraph failed."), tHintBuf);
							goto errLabel;
						}
					}

				}

				else
				{
					CUS_MODULE_compress  pModule = null;
					uint uiModuleType = 0;             //  2012/09/09

                    //
                    //
#if DEBUG
					//showInfo_open(  0,  0,  _T(  "initTaskAvFrom_video ...8888"  )  );
					{

					int tttiW = pTransformVideo.tv.vh_decompress.bih.biWidth;
					int ii = 0;
				}
                
#endif


                    //
                    switch (pTransformVideo.tv.compressor.ucCompressors)
					{
						case Consts.CONST_videoCompressors_ipp:                //  2011/07/31
																		//  pModule	=	pQyMc->cusModules.pIpp;
							uiModuleType = Consts.CONST_moduleType_ipp;    //  2012/09/09
							break;
						case Consts.CONST_videoCompressors_hwAccl:     //  2011/11/25
						case Consts.CONST_videoCompressors_ffmpeg:
							//
							{
								//uiModuleType  =  CONST_moduleType_ipp;
								uiModuleType = Consts.CONST_moduleType_mediaSdk_dec;
							}
							break;
						case Consts.CONST_videoCompressors_stream:
							switch (pTransformVideo.tv.vh_stream.dwFourcc)
							{
								case Consts.CONST_fourcc_h264:
								case Consts.CONST_fourcc_HEVC: //  2017/10/08
									break;
								default:
									{
#if __DEBUG__
										char fourccStr[5] = "";
										iFourcc2Str(pTransformVideo.tv.vh_stream.dwFourcc, fourccStr, mycountof(fourccStr));
										_sntprintf(tBuf, mycountof(tBuf), _T("initTaskAvFrom_video failed, stream, fourcc err, %S"), fourccStr);
										traceLog(tBuf);
										showInfo_open(0, 0, tBuf);
#endif
										goto errLabel;
									}
									break;
							}
							uiModuleType = Consts.CONST_moduleType_ipp;
							//
							break;
						case Consts.CONST_videoCompressors_vp8:        //  2014/07/21
							uiModuleType = Consts.CONST_moduleType_dec_vp8;
							break;
						case Consts.CONST_videoCompressors_custom:
							//  pModule	=	getCusModule(  pProcInfo->pQyMc,  CONST_moduleType_compress  );
							uiModuleType = Consts.CONST_moduleType_compress;
							break;
						default:
#if __DEBUG__
							_sntprintf(tBuf, mycountof(tBuf), _T("initTaskAvFrom_video: ucCompressors error, %d"), (int)pTransformVideo.tv.compressor.common.ucCompressors);
							traceLog(tBuf);
							showInfo_open(0, 0, tBuf);
#endif
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s ucCompressors error, %d"), tHintBuf, (int)pTransformVideo.tv.compressor.common.ucCompressors);
							goto errLabel;
							break;
					}
					//
					switch (uiModuleType_suggested)
					{
						case Consts.CONST_moduleType_mediaSdk_dec:
						case Consts.CONST_moduleType_dec_nv:
						case Consts.CONST_moduleType_dec_amf:  //  
							switch (uiModuleType)
							{
								case Consts.CONST_moduleType_ipp:
								case Consts.CONST_moduleType_mediaSdk_dec:
									uiModuleType = uiModuleType_suggested;
									break;
								default:
									break;
							}
							break;
						default:
							break;
					}
					//
#if false
					switch (uiModuleType)
                    {
						case Consts.CONST_moduleType_mediaSdk_dec:
						case Consts.CONST_moduleType_ipp:
							//uiModuleType = Consts.CONST_moduleType_ffmpeg;
							uiModuleType = Consts.CONST_moduleType_mediaCodec;
							
							break;
						default:
							break;
                    }
#endif
					//
					adjustModuleType_decV(ref uiModuleType);

					//
#if __DEBUG__
					//showInfo_open(  0,  0,  _T(  "initTaskAvFrom_video ...8889"  )  );
#endif

					//
					M_getCompressModule(pCusModules, uiModuleType, ref pModule);    //  2012/09/09
					if (null==pModule || null==pModule.pf_qdcInitDecompressVideo)
					{
						
						//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s pModule or initDecV is null. moduleType %d"), tHintBuf, uiModuleType);
						str=string.Format("pModule or initDecV is null. moduleType {0}", uiModuleType);
						//
						TMP_buf5 fourcc;
						uint cntof_fourcc = 5;
						//char fourcc[4 + 1];
						iFourcc2Str((int)pTransformVideo.tv.vh_stream.dwFourcc, fourcc.buf, cntof_fourcc);
						str=string.Format("decV: {0} not supported. {1}", mytoString(fourcc.buf), str);
						showNotification(0, 0, 0, 0, 0, 0, str);
						//
						goto errLabel;
					}
					pTransformVideo.uiModuleType_toDec = uiModuleType; //  2012/09/09
					pTransformVideo.bUseDecTool = bUseDecTool_suggested;   //  2015/01/19

					//  2017/10/11
					int iFourcc = (int)pTransformVideo.tv.vh_stream.dwFourcc;
					if (0 == iFourcc) {
						//fixed (byte* fourccStr1 = pTransformVideo.tv.compressor.common.fourccStr)
						{
							iFourcc = pTransformVideo.tv.compressor.iFourcc;// fourccStr2i(fourccStr1);
						}
					}

					//  2016/04/28
					ushort real_usPktResType = 0;  //  pProcInfo->get_pktResType_suggested(  CONST_pktUsage_dec,  uiModuleType  );
					if (0!=pProcInfo.get_deced_pktResType(uiModuleType, iFourcc, ref real_usPktResType)) goto errLabel;
					//
					//  2017/10/13
					TMP_buf5 fourccStr;
					//byte fourccStr[4 + 1] = "";
					iFourcc2Str(iFourcc, fourccStr.buf, Consts.cntof_buf5);
					str=string.Format("initTaskAvFrom_video: fourcc {0}, real_usPktResType {1}", mytoString(fourccStr.buf), (int)real_usPktResType);
					showInfo_open(0, null, str);					
					//
					pTransformVideo.usPktResType = real_usPktResType;

					//  2014/03/25
					VIDEO_COMPRESSOR_CFG compressor;
					//
					compressor = new VIDEO_COMPRESSOR_CFG();
					//memcpy(&compressor, &pFrom.video.tv_recvd.compressor, sizeof(compressor));
					compressor.common.head=pActiveMem_from.avStream.obj.tranInfo.video.compressor;
                    //
                    compressor.common.iTaskId = iTaskId;
                    {
						int index_taskInfo = getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
						QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo,"");
						if (pTaskInfo == null) goto errLabel;
						QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
						if (pTaskData == null) goto errLabel;
						if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
						QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                        compressor.shared_eglContext = pTc.shared_eglContext;

						//
                    }

                    //
#if true
                    //compressor.common.pCurPkt1 = pTransformVideo.pCurPkt1;
					compressor.common.size_pCurPkt = sizeof(myDRAW_VIDEO_DATA  );
					compressor.common.pInDataCacheQ = pTransformVideo.pInDataCacheQ;
					compressor.common.pVideoQ2 = pTransformVideo.q2;
					compressor.common.pTmpDataQ = pTransformVideo.pTmpDataQ;
					//
					compressor.common.pParent_transform = pTransform;
					compressor.common.uiTransformType_pParent = Consts.CONST_uiTransformType_dec;
					//
					compressor.common.pf_bTransformVideoQuit = bTransformVideoQuit;
					compressor.common.pf_transformGetSpsPps = transformGetSpsPps;
					compressor.common.pf_transformGetBih = transformGetBih;
					compressor.common.pf_getNextVPkt = getNextVPkt;
					//compressor.common.pf_parseVPkt_i  =  dec_parseEncVPkt_i;
					compressor.common.pf_parseDecVPkt_o = parseDecVPkt_o;

					compressor.common.pf_detachPkt = detachVPkt;
					compressor.common.pf_clean_myDRAW_VIDEO_DATA = (PF_clean_myDRAW_VIDEO_DATA)clean_myDRAW_VIDEO_DATA;

					//  2014/09/07
					compressor.common.iDecParam_index_pMems_from = -1;// index_pMems_from;
					//  2014/07/09
					compressor.common.dec.pf_smplBitstreamReader_ReadNextFrame_func = smplBitstreamReader_ReadNextFrame_func_v;
#endif
					/*
					//  2015/01/17
					compressor.common.dec.pf_smplYUVWriter_WriteNextFrame_pre = smplYUVWriter_WriteNextFrame_pre;   //  2015/01/17
					compressor.common.dec.pf_smplYUVWriter_WriteNextFrame0 = smplYUVWriter_WriteNextFrame0;         //  2014/07/23
					compressor.common.dec.pf_smplYUVWriter_WriteNextFrame1 = smplYUVWriter_WriteNextFrame1;         //  2014/07/23
																													//
					compressor.common.dec.pf_smplYUVWriter_WriteNextFrame_all = smplYUVWriter_WriteNextFrame_all;       //  2014/07/23
																														//
					compressor.common.dec.pf_updateDecVideoParam = updateDecVideoParam;
					//
					//compressor.common.dec.pf_mychkFromPlayerIndex_v  =  (  PF_commonHandler  )chkFromPlayerIndex_v;
					//
					compressor.common.dec.pf_postVPktToRender = (PF_commonHandler)postVPktToRender;
					*/
					//  2014/03/29
					//compressor.common.ucHardwareAccl = pQmcCfg->policy.ucHardwareAccl;
					//  2015/10/04
					compressor.common.usPktResType_suggested = real_usPktResType;   //  2016/04/28	//  pProcInfo->get_pktResType_suggested(  CONST_pktUsage_dec  );
																					//  2014/07/17
					//pTransformVideo.u.qoi.myQoi.common.var.pDebugStep = &pTransform->video.debugStep;
					

					//  2015/01/19
					if (!pTransformVideo.bUseDecTool)
					{
						//
#if DEBUG
						traceLog("initTaskAvFrom_video.547, before qdcInitDecV, moduleType " + uiModuleType);
#endif


						//  2016/04/26
						//pProcInfo->set_qoi_funcs(&pTransformVideo.u.qoi.myQoi);

						//
						if (0!=pModule.pf_qdcInitDecompressVideo(ref compressor, ref pTransformVideo.tv.vh_compress, ref pTransformVideo.tv.vh_decompress, ref pTransformVideo.u_qoi.myQoi.common))
						{
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s qdcInitDecVideo failed."), tHintBuf);
							traceLogA("initTaskAvFrom_video: initQdcDecVideo failed.");
							goto errLabel;
						}

						//
#if DEBUG
						traceLog("initTaskAvFrom_video.564: after initQdcDecVideo");
#endif

					}
					else
					{  //  2015/01/28
						ref QOI_common pQdcObjInfo = ref pTransformVideo.u_qoi.myQoi.common;
						//memcpy(&pQdcObjInfo->cfg.v, &compressor.common, sizeof(pQdcObjInfo->cfg.v)  );
						pQdcObjInfo.cfg.v = compressor.common;
						pQdcObjInfo.var.ucbTransformFuncsOk = true;
					}
				}

				//  2014/08/04
				if (pTask.saveVideo.ucbSaveVideo_req)
				{
					/*
					pTransformVideo.saveVideo.uiSize_pSaveVideoPkt = sizeof(SAVE_video_pkt);
					if (pTransformVideo.saveVideo.pSaveVideoPkt)
					{
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initTaskAvFrom_video failed: pSaveVideoPkt is not null."));
						goto errLabel;
					}
					pTransformVideo.saveVideo.pSaveVideoPkt = mymalloc(pTransformVideo.saveVideo.uiSize_pSaveVideoPkt);
					if (!pTransformVideo.saveVideo.pSaveVideoPkt) goto errLabel;
					memset(pTransformVideo.saveVideo.pSaveVideoPkt, 0, pTransformVideo.saveVideo.uiSize_pSaveVideoPkt);
					*/
				}

				//
				if (pTransformVideo.tv.compressor.ucCompressors != Consts.CONST_videoCompressors_stream)
				{
					switch (pTransformVideo.tv.vh_decompress.bih.biCompression)
					{
						case Consts.BI_RGB:
							if (pTransformVideo.tv.vh_decompress.bih.biPlanes != 1)
							{
								//_sntprintf(tHintBuf, mycountof(tHintBuf), _T(" vh_dec.biPlanes %d"), (int)pTransformVideo.tv.vh_decompress.bih.biPlanes);
								//goto  errLabel;			
							}
							if (pTransformVideo.tv.vh_decompress.bih.biBitCount != 24)
							{
								//_sntprintf(tHintBuf, mycountof(tHintBuf), _T("%s bitCount %d"), tHintBuf, (int)pTransformVideo.tv.vh_decompress.bih.biBitCount);
							}
							break;
						case Consts.CONST_fourcc_YV12:
						case Consts.CONST_fourcc_I420:
							break;

						default:
							//_sntprintf(tHintBuf, mycountof(tHintBuf), _T(" vh_dec not RGB"), pTransformVideo.tv.vh_decompress.bih.biCompression);
							traceLogA("not rgb.");
							goto errLabel;
							break;
					}
				}
#if __DEBUG__
				//  pTransformVideo.biSizeImage_decompress  =  (  pTransformVideo.vh_decompress.bih.biWidth  *  pTransformVideo.vh_decompress.bih.biBitCount  +  31  )  /  32  *  4  *  pTransformVideo.vh_decompress.bih.biHeight;
#endif
				//  pTransformVideo.iDecompressBufSize  =  max(  pTransformVideo.vh_decompress.bih.biSizeImage,  biSizeImage  )  +  iFormatSize  +  4096;	//  ²»ÖªµÀ¾«È·µÄÓ¦¸ÃÊÇ¶àÉÙ£¬ËùÒÔºýÅªÒ»¸öÔÚÕâÀï£®£²£°£°£¸£¯£°£³£¯£±£¹
				//
				//  if  (  !(  pTransformVideo.pDecompressBuf  =  (  char  *  )mymalloc(  pTransformVideo.iDecompressBufSize  )  )  )  goto  errLabel;

				//
				pTransformVideo.index_activeMems_from = index_activeMems_from;

				pTransformVideo.ucbInited = true;
				}

				iErr = 0;

			errLabel:


				//  2012/03/19
				M_setMyStep(ref pTransformVideo.debugStep, 41);


				//
				if (0!=iErr)
				{
					exitTaskAvFrom_video(pProcInfo, pTransform,"initTaskAvFrom_video err");
				}


				//  2012/03/19
				M_setMyStep(ref pTransformVideo.debugStep, 42);

				//  if  (  iErr  ||  tHintBuf[0]  )  
				{
					//int index_activeMems_from = 0;  //  pTransform->index_activeMems_from;
					str=string.Format("initTaskAvFrom_video leaves. {0}: {1} {2}", index_activeMems_from, pActiveMem_from.avStream.idInfo.ui64Id, (0!=iErr ? _T("failed") : _T("")));
				showInfo_open(0, null, str);


				}

				//traceLogA("initTaskAvFrom_video: %I64u leaves with %s", pFrom.idInfo.ui64Id, iErr ? "error" : "ok");

				return iErr;
			}


			public static unsafe void exitTaskAvFrom_video(CCtxQmc pProcInfo, QY_TRANSFORM pTransform,string hint)
			{

				int i;
			string str;

				//
				ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
				CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
				if (null==pCusModules)
				{
					//MACRO_qyAssert(0, _T("exitTaskAvFrom_video: pCusModules is null"));
					return;
				}

			//
			if (hint == null) hint = "";


				//
				TRANSFORM_VIDEO_procInfo pTransformVideo = pTransform.video;


				//  traceLogA(  "exitTaskAvFrom_video: %I64u enters",  pFrom.idInfo.ui64Id  );

				//  2014/07/17
				M_setMyStep(ref pTransformVideo.debugStep, 102);


				//  if  (  pFrom.video.ucbInited  )  
				{
					if (0==pTransformVideo.tv.compressor.ucCompressors) ;
					else if (pTransformVideo.tv.compressor.ucCompressors == Consts.CONST_videoCompressors_vcm)
					{
					/*
						if (pTransformVideo.u.vcm.hicDecompress)
						{
#if __DEBUG__
							traceLogA("ÕâÀïÒª¹Ø±Õ´ò¿ªµÄ½âÑ¹ËõÒýÇæ¡£");
#endif
							//ICClose(pTransformVideo.u.vcm.hicDecompress); pTransformVideo.u.vcm.hicDecompress = NULL;
						}
					*/
					}
					else if (pTransformVideo.tv.compressor.ucCompressors == Consts.CONST_videoCompressors_dmo)
					{

						if (pQmcCfg.ucb__SUPPORT_dmoToDec__)
						{
							//exitDecompressVideo_dmo(&pTransformVideo.u.dmo);
						}
						else
						{
							//exitDecompress_filterGraph(  &pTransformVideo.u.dmo,  &pTransformVideo.debugStep  );
						}

					}

					else
					{
						CUS_MODULE_compress pModule = null;

						M_getCompressModule(pCusModules, pTransformVideo.uiModuleType_toDec, ref pModule); //  2012/09/09

						//  2015/01/19
						if (!pTransformVideo.bUseDecTool)
						{

						//
						if (pModule != null)
						{
							if (null != pModule.pf_qdcExitDecompressVideo)
							{
								pModule.pf_qdcExitDecompressVideo(ref pTransformVideo.u_qoi.myQoi.common);
							}

						}


					}
				}
				//
				//  2014/08/04
				//byte* pPkt = (byte*)pTransformVideo.saveVideo.pSaveVideoPkt;
				//	MACRO_mysafeFree(ref pPkt);
				//pTransformVideo.saveVideo.pSaveVideoPkt = pPkt;

					//  2012/09/09
					pTransformVideo.uiModuleType_toDec = 0;
					pTransformVideo.bUseDecTool = false;   //  2015/01/19

				//  2010/05/04
				//memset(&pTransformVideo.u, 0, sizeof(pTransformVideo.u)  );
				pTransformVideo.u_qoi.myQoi.common.memset0();

				//  if  (  pTransformVideo.pDecompressBuf  )  M_free(  pTransformVideo.pDecompressBuf  );  

				//  2012/03/19
				fixed (TRANSFORM_VIDEO_procInfo_status* pstatus = &pTransformVideo.status)
				{
					mymemset((IntPtr)pstatus, 0, sizeof(TRANSFORM_VIDEO_procInfo_status));
				}

			//
			pTransformVideo.ucbInited = false;
		}

		//  errLabel:


		//  2014/07/17
		M_setMyStep(ref pTransformVideo.debugStep,  108  );


			int index_activeMems_from = pTransform.index_activeMems_from;
			str=string.Format(  "exitTaskAvFrom_video {0}. {1}",  index_activeMems_from,hint  );
			qyFuncs.showInfo_open(0, null, str);

	//  traceLogA(  "exitTaskAvFrom_video: %I64u leaves",  pFrom.idInfo.ui64Id  );
	//traceLogA(  "exitTaskAvFrom_video leaves"  );

	return;
}




}
}
