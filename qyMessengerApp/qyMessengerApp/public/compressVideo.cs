using System;
using System.Collections.Generic;
using System.Text;

partial class Consts {
	public const string CONST_mutexNamePrefix_encV = ("mtxEncV");

}

namespace qy
{
	partial class qyFuncs
	{


		//
		public static unsafe int initCompressVideo(CCtxQmc pProcInfoParam, BITMAPINFOHEADER* pBmpInfo_input, uint uiCapType, ref VIDEO_COMPRESSOR_CFG pCompressor, bool bCapDev, uint uiBufSize_pData, ref COMPRESS_VIDEO pCompressVideo)
		{
			int iErr = -1;


			CCtxQmc pProcInfo0 = pProcInfoParam;
			if (null == pProcInfo0) return -1;
			ref QMC_cfg pQmcCfg = ref pProcInfo0.cfg;
			//if (!pQmcCfg) return -1;
			//QY_MC			*	pQyMc			=	(  QY_MC  *  )pProcInfo0.pQyMcParam;//
			//if  (  !pQyMc  )  return  -1;
			//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo0.getCusModules();
			if (null == pCusModules) return -1;
			MIS_CNT pMisCnt = pProcInfo0.pMisCnt;

			//
			uint iFourcc = 0;
			BITMAPINFOHEADER* pBmpInfo = null;
			int iFormatSize = 0;
			uint biSizeImage = 0;
			int iRet;
			bool ucbResizable = false;
			bool ucbResizeByEncoder = false;      //  2014/05/03
			int iW_org = 0;
			int iH_org = 0;

			//  2016/04/02
			//if (null==pCompressVideo) return -1;

			//  2016/04/02
			qyFuncs.showInfo_open(0,"",  "initCompressVideo enters. ");

			//  2014/04/07

			//
			if (pCompressVideo.bVideoCompressInited)
			{
				goto errLabel;            //  Õâ±íÃ÷ÒÑ¾­ºÃÁË
			}

			//  2009/04/19
			//  pCompressVideo.usMaxFps_toShareBmp  =  (  bCapDev  ?  pProcInfo0.cfg.policy.video.usMaxFps_toShareBmp  :  pProcInfo0.cfg.policy.video.usFps_capScreen  );
			//  pCompressVideo.usMaxFps_toShareBmp  =  pCompressor->usMaxFps_toShareBmp;
			//if (memcmp(pCompressor, &pCompressVideo.compressor, sizeof(pCompressor[0]))) memcpy(&pCompressVideo.compressor, pCompressor, sizeof(pCompressVideo.compressor)  );
			pCompressVideo.compressor = pCompressor;
			//
			if (0 == pCompressVideo.compressor.common.head.usMinFps_toShareBmp)
			{
				//  pCompressVideo.compressor.common.usMinFps_toShareBmp  =  1;
				pCompressVideo.compressor.common.head.usMinFps_toShareBmp = pCompressVideo.compressor.common.head.usMaxFps_toShareBmp;  //  2011/07/25
			}
			//pCompressVideo.compressor.common.ucbScreenSharing  =  (  uiCapType  ==  CONST_capType_screen  );
			pCompressVideo.compressor.common.head.uiCapType = uiCapType;                                                //  2012/02/08

			//  2014/03/29
			pCompressVideo.compressor.common.head.ucHardwareAccl = pQmcCfg.policy.ucHardwareAccl;

			//
			pCompressVideo.usFrames_perSecond_expected = pCompressVideo.compressor.common.head.usMinFps_toShareBmp;
			//
			if (0 == pCompressVideo.usFrames_perSecond_expected)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo failed: usFrames_perSecond_expected is 0"));
				goto errLabel;
			}
			//
			pCompressVideo.ucTimeoutInS_showFrameInfoResp = Consts.MAX_ucTimeoutInS_showFrameInfoResp;                        //  È±Ê¡³¬Ê±ÓÃ30Ãë
																															  //
			pCompressVideo.tLastTime_sendVideo = 0;
			pCompressVideo.tLastTime_showFrameInfo = 0;
			pCompressVideo.tFirstTime_showFrameInfo_withoutResp = 0;
			pCompressVideo.tRecvTime_resp_fromCli = 0;

			//
			if (null == pBmpInfo_input)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo failed: pBmpInfo_input is null"));
				goto errLabel;
			}

			//		
			iW_org = pBmpInfo_input->biWidth;
			iH_org = pBmpInfo_input->biHeight;

			//
#if false
			if (0 != pCompressor->common.iWidth_pic
				&& 0 != pCompressor->common.iHeight_pic)
			{
				if (pCompressor->common.iWidth_pic < pBmpInfo_input->biWidth
					|| pCompressor->common.iHeight_pic < pBmpInfo_input->biHeight)
				{
					{  // resize others in vpp. 2014/04/24
						switch (uiCapType)
						{
							case Consts.CONST_capType_av:
							case Consts.CONST_capType_mediaFile:
							case Consts.CONST_capType_mediaDevice:
								ucbResizable = true;
								break;
							case Consts.CONST_capType_screen:
							case Consts.CONST_capType_qvcf:        //  2015/05/25
#if __DEBUG__
#endif
								break;
							case Consts.CONST_capType_mosaic:
								break;
							default:
#if __DEBUG__
								traceLog(_T("Not finished: initCompressVideo: unResizable capType %d"), uiCapType);
								MACRO_qyAssert(0, _T("Not finished: initCompressVideo: unResizable capType"));
#endif
								break;
						}
					}
				}
			}
#endif

			//
			if (!ucbResizable)
			{
				//memcpy(&pCompressVideo.vh_decompress.bih, pBmpInfo_input, sizeof(pCompressVideo.vh_decompress.bih)  );
				pCompressVideo.vh_decompress.bih = *pBmpInfo_input;
			}
			else
			{
				//
				bool ucbAutoClip = false;

				int iX = 0;
				int iY = 0;
				int iW = pCompressor.common.head.iWidth_pic;
				int iH = pCompressor.common.head.iHeight_pic;
				uint iW_i, iH_i;
				int iX_src_i, iY_src_i;
				int iX_dst, iY_dst;
				uint iW_dst, iH_dst;

				//
#if true
				bool ucb3d = true;

				myGetImageInfo(false,                                       //  
						ucbAutoClip,                                    //  根据目标矩形自动裁剪，使目标矩形被充满
						ucb3d,                                          //  2014/11/14
						iW_org, iH_org,
						iX, iY, iW, iH,
						(int*)&iW_i, (int*)&iH_i,
						&iX_src_i, &iY_src_i,
						&iX_dst, &iY_dst, (int*)&iW_dst, (int*)&iH_dst);

#endif
				//
				iW_dst = iW_dst / 4 * 4;
				iH_dst = iH_dst / 4 * 4;
				//
				if (0 == iW_dst || 0 == iH_dst)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo failed: iW_dst or iH_dst is 0"));
					goto errLabel;
				}
				//MACRO_qyAssert(!(iW_dst % 4), _T(""));
				//MACRO_qyAssert(!(iH_dst % 4), _T(""));
				//
				fixed (BITMAPINFOHEADER* pBih = &pCompressVideo.vh_decompress.bih)
				{
					makeBmpInfoHeader_rgb(24, (int)iW_dst, (int)iH_dst, pBih);
				}

				//  下面还要在enc模块里加上resize处理。要把bResizable参数从compressor带进去，

			}
			//  2014/05/03
			pCompressVideo.ucbResizeByEncoder = ucbResizeByEncoder;

			//  2014/05/11
			if (true)
			{
#if false
				if (!bSupported_avLevel(pProcInfo0.pQyMc, uiCapType, (BITMAPINFOHEADER*)&pCompressVideo.vh_decompress.bih, pCompressor))
				{
					showNotification(0, 0, 0, 0, 0, 0, _T("Not supported in this version"));
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Not supported in this version"));
					goto errLabel;
				}
#endif
			}


			//
			//fixed (byte* fourccStr = pCompressVideo.compressor.common.fourccStr)
			{
				iFourcc = (uint)pCompressVideo.compressor.common.head.iFourcc;// fourccStr2i(fourccStr);
			}

			//pCompressVideo.usCapDriverIndex  =  usCapDriverIndex;

			//  2014/04/02		 
			//  2014/03/23	 	
			GENERIC_Q_cfgEx qCfgEx; //  2012/11/10
			GENERIC_Q_CFG tmpCfg;

			//TCHAR tBuf[128];
			int tn = (int)myGetTickCount(null);
			int size;
			//sizeof = sizeof(GENERIC_Q);
			pCompressVideo.pInDataCacheQ = new GENERIC_Q();// (GENERIC_Q*)mymalloc(size);
			if (null == pCompressVideo.pInDataCacheQ) goto errLabel;
			//memset(pCompressVideo.pInDataCacheQ, 0, size);
			//size = sizeof(GENERIC_Q);
			pCompressVideo.pTmpDataQ = new GENERIC_Q();// (GENERIC_Q*)mymalloc(size);
			if (null == pCompressVideo.pTmpDataQ) goto errLabel;
			//memset(pCompressVideo.pTmpDataQ, 0, size);

			string str;

			//
			mymemset((IntPtr)(byte*)&tmpCfg, 0, sizeof(GENERIC_Q_CFG));
			//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-in-%d"), CONST_mutexNamePrefix_encV, tn);
			str = string.Format("{0}-in-{1}", Consts.CONST_mutexNamePrefix_encV, tn);
			mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
			//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-in-%d"), CONST_mutexNamePrefix_encV, tn);
			str = string.Format("{0}-in-{1}", Consts.CONST_mutexNamePrefix_encV, tn);
			mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
			tmpCfg.uiMaxQNodes = 100;

			//memset(&qCfgEx, 0, sizeof(qCfgEx));
			qCfgEx = new GENERIC_Q_cfgEx();
			//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
			qCfgEx.common = tmpCfg;
			qCfgEx.pfQElemNewEx = mallocMemory;
			qCfgEx.pfQElemNew = mymalloc;
			qCfgEx.pfQElemFreeEx = freeMemory;
			qCfgEx.pfQElemFree = myfree;
			qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
			qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_myDRAW_VIDEO_DATA;

			if (0 != initGenericQ(qCfgEx, (GENERIC_Q)pCompressVideo.pInDataCacheQ)) goto errLabel;

			//
			mymemset((IntPtr)(byte*)&tmpCfg, 0, sizeof(GENERIC_Q_CFG));
			//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-tmp-%d"), CONST_mutexNamePrefix_encV, tn);
			str = string.Format("{0}-tmp-{1}", Consts.CONST_mutexNamePrefix_encV, tn);
			mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
			//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-tmp-%d"), CONST_mutexNamePrefix_encV, tn);
			str = string.Format("{0}-tmp-{1}", Consts.CONST_mutexNamePrefix_encV, tn);
			tmpCfg.uiMaxQNodes = 100;

			//memset(&qCfgEx, 0, sizeof(qCfgEx));
			qCfgEx = new GENERIC_Q_cfgEx();
			//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
			qCfgEx.common = tmpCfg;
			qCfgEx.pfQElemNewEx = mallocMemory;
			qCfgEx.pfQElemNew = mymalloc;
			qCfgEx.pfQElemFreeEx = freeMemory;
			qCfgEx.pfQElemFree = myfree;
			qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
			qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_myDRAW_VIDEO_DATA;

			if (0 != initGenericQ(qCfgEx, pCompressVideo.pTmpDataQ)) goto errLabel;

			//
			size = sizeof(myDRAW_VIDEO_DATA);
			pCompressVideo.pCurPkt = (myDRAW_VIDEO_DATA*)mymalloc(size);
			if (null == pCompressVideo.pCurPkt) goto errLabel;
			mymemset((IntPtr)pCompressVideo.pCurPkt, 0, size);


			//
			pCompressVideo.u_qoi.myQoi.common.memset0();
			

			//  
			if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_vcm)
			{
				traceLog(("vcm is not supported "));
				goto errLabel;
			}
			else if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_dmo)
			{
				traceLog(("dmo is not supported "));
				goto errLabel;

			}
			else if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_stream)
			{
				//
#if __DEBUG__
				//  从源里直接得到压缩数据，所以不需要压缩. 2014/02/24
				traceLog(_T("videoCompressors_stream"));
#endif
			}
			else
			{
#if true
				//
				qyFuncs.traceLog("initCompressVideo.315");

				//
				CUS_MODULE_compress pModule = null;
				uint uiModuleType = 0;             //  2012/09/09

				switch (pCompressVideo.compressor.common.head.ucCompressors)
				{
					case Consts.CONST_videoCompressors_ffmpeg:
						uiModuleType = Consts.CONST_moduleType_ffmpeg;
						break;
					case Consts.CONST_videoCompressors_custom:
						//  
						uiModuleType = Consts.CONST_moduleType_compress;
						break;
					default:
#if __DEBUG__
						traceLogA("initCompressVideo: ucCompressors err: %d", pCompressVideo.compressor.common.ucCompressors);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo: ucCompressors err: %d"), pCompressVideo.compressor.common.ucCompressors);
						goto errLabel;
						break;
				}

				//
				adjustModuleType_encV(ref uiModuleType);

				//
				M_getCompressModule(pCusModules, uiModuleType, ref pModule);    //  2012/09/09
				if (null == pModule || !pModule.common.bLoaded || null == pModule.pf_qdcInitCompressVideo)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo failed, pModule or initDecompressVideo is null."));
					goto errLabel;
				}
				pCompressVideo.uiModuleType_toEnc = uiModuleType;



				//
				//  2014/04/02
				VIDEO_COMPRESSOR_CFG compressor;
				//memcpy(&compressor, &pCompressVideo.compressor, sizeof(compressor));
				compressor = pCompressVideo.compressor;
				/*
				compressor.common.pCurPkt = pCompressVideo.pCurPkt;
				compressor.common.size_pCurPkt = sizeof(pCompressVideo.pCurPkt[0]  );
				compressor.common.pInDataCacheQ = pCompressVideo.pInDataCacheQ;
				compressor.common.pVideoQ2 = pCompressVideo.compressor.common.pVideoQ2;    //  2014/04/03. pVideoQ2这时尚未初始化。等数据流运行时，pVideoQ2已经ready
				if (!compressor.common.pVideoQ2)
				{
					showInfo_open(0, 0, _T("initCompressVideo failed, pVideoQ2 is null"));
					goto errLabel;
				}
				compressor.common.pTmpDataQ = pCompressVideo.pTmpDataQ;
				*/
				//
				compressor.common.pParent_transform = pCompressVideo.compressor.common.pParent_transform;
				if (null==compressor.common.pParent_transform)
				{
					goto errLabel;
				}
				compressor.common.uiTransformType_pParent = Consts.CONST_uiTransformType_encVideo;
				//
				/*
				compressor.common.pf_bTransformVideoQuit = bCapBmpQuit;
				compressor.common.pf_transformGetSpsPps = capBmpGetSpsPps;
				compressor.common.pf_transformGetBih = (PF_commonHandler)capBmpGetBih;
				compressor.common.pf_getNextVPkt = (PF_commonHandler)getNextVPkt;
				//compressor.common.pf_parseVPkt_i  =  enc_parseOrgVPkt_i;
				compressor.common.pf_parseDecVPkt_o = parseDecVPkt_o;

				compressor.common.pf_detachPkt = detachVPkt;
				compressor.common.pf_clean_myDRAW_VIDEO_DATA = (PF_clean_myDRAW_VIDEO_DATA)clean_myDRAW_VIDEO_DATA;

				//
				//compressor.common.enc.ucb__USE_hdEnc_conf__  =  pProcInfo0.cfg.ucb__USE_hdEnc_conf__;
				compressor.common.enc.ucbResizable = ucbResizeByEncoder;    //  ucbResizable;	//  2014/04/20
				compressor.common.enc.iW_org = iW_org;
				compressor.common.enc.iH_org = iH_org;
				//
				compressor.common.enc.pf_smplYUVReader_LoadNextFrame = smplYUVReader_LoadNextFrame;
				compressor.common.enc.pf_doPostEnc = doPostEnc_mediaSdk_new;

				//  2014/03/29
				compressor.common.ucHardwareAccl = pQmcCfg->policy.ucHardwareAccl;
				*/

				//
				{

					//
					qyFuncs.traceLog("initCompressVideo: before qdcInitCompressVideo");

					//
					//  2016/04/26				
					//pProcInfo0.set_qoi_funcs(&pCompressVideo.u.qoi.myQoi);

					//
					if (0 != pModule.pf_qdcInitCompressVideo(ref compressor, ref pCompressVideo.vh_decompress, ref pCompressVideo.vh_compress, ref pCompressVideo.u_qoi.myQoi.common))
					{
						qyFuncs.traceLog("initCompressVideo.415, qdcInitCompressVideo failed");
						goto errLabel;
					}
					pCompressVideo.iCompressBufSize = pCompressVideo.vh_compress.bih.biSizeImage;

					//
					qyFuncs.traceLog("initCompressVideo.421, after qdcInitCompressVideo");

#if __DEBUG__       //  2014/10/10
					char fourcc_dec[4 + 1] = "";
					char fourcc_enc[4 + 1] = "";
					iFourcc2Str(pCompressVideo.vh_decompress.bih.biCompression, fourcc_dec, mycountof(fourcc_dec));
					iFourcc2Str(pCompressVideo.vh_compress.bih.biCompression, fourcc_enc, mycountof(fourcc_enc));
					traceLog(_T("fourcc_dec %S, fourcc_enc %S"), fourcc_dec, fourcc_enc);
#endif

					//
					if (pModule.pf_qdcGetSpsPps != null)
					{
						fixed (VH_stream* pVh_stream = &pCompressVideo.vh_stream)
						{
							mymemset((IntPtr)(byte*)pVh_stream, 0, sizeof(VH_stream));

							//
							uint vpsLen = 0;   //  2017/10/10
							byte* pVps = null;
							uint spsLen = 0;
							byte* pSps = null;
							uint ppsLen = 0;
							byte* pPps = null;

							//
							switch (iFourcc)
							{
								case Consts.CONST_fourcc_HEVC:
									{
										//
										int k = 0;
										vpsLen = Consts.MAX_spsPpsLen + 1; pVps = pVh_stream->mem0.spsPps; k++;
										spsLen = Consts.MAX_spsPpsLen + 1; pSps = pVh_stream->mem1.spsPps; k++;
										ppsLen = Consts.MAX_spsPpsLen + 1; pPps = pVh_stream->mem2.spsPps;
										//				  
										//
										if (0 != pModule.pf_qdcGetSpsPps(ref pCompressVideo.u_qoi.myQoi.common, (IntPtr)pSps, ref spsLen, (IntPtr)pPps, ref ppsLen, (IntPtr)pVps, ref vpsLen))
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo failed, qdcGetSpsPps failed."));
											goto errLabel;
										}
										//				  
										k = 0;
										pVh_stream->mem0.usLen = (ushort)vpsLen; k++;
										pVh_stream->mem1.usLen = (ushort)spsLen; k++;
										pVh_stream->mem2.usLen = (ushort)ppsLen; k++;
										pVh_stream->usCnt = (ushort)k;


									}
									break;
								case Consts.CONST_fourcc_h264:
								default:
									{
										int k = 0;
										spsLen = Consts.MAX_spsPpsLen + 1; pSps = pVh_stream->mem0.spsPps; k++;
										ppsLen = Consts.MAX_spsPpsLen + 1; pPps = pVh_stream->mem1.spsPps;
										//
										if (0 != pModule.pf_qdcGetSpsPps(ref pCompressVideo.u_qoi.myQoi.common, (IntPtr)pSps, ref spsLen, (IntPtr)pPps, ref ppsLen, (IntPtr)null, ref vpsLen))
										{
											//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("initCompressVideo failed, qdcGetSpsPps failed."));
											goto errLabel;
										}
										//				  
										pVh_stream->mem0.usLen = (ushort)spsLen;
										pVh_stream->mem1.usLen = (ushort)ppsLen;
										pVh_stream->usCnt = 2;
									}
									break;
							}
							//
							pVh_stream->dwFourcc = (uint)iFourcc;// pCompressVideo.vh_compress.bih.biCompression;
																 //  2014/05/02
																 //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo0.who_showInfo, 0, _T(""), _T(""), _T("qdcGetSpsPps: vps %d, spsLen %d, ppsLen %d"), vpsLen, spsLen, ppsLen);

							//
#if DEBUG
							if (pQmcCfg.debugStatusInfo.ucb__Dump_h264_enc__)
							{
								string fn = Consts.CONST_fn_dump_h264_enc;
								/*
								static bool bReset = false;
								FILE* fp = null;
								if (!bReset)
								{
									fp = fopen(fn, "w");
									if (fp)
									{
										fclose(fp);
									}
									bReset = true;
								}
								fp = fopen(fn, "ab+");
								if (fp)
								{
									int i;
									for (i = 0; i < pVh_stream->usCnt; i++)
									{
										fwrite(pVh_stream->mems[i].spsPps, pVh_stream->mems[i].usLen, 1, fp);
									}
									fclose(fp);
								}
								*/
								GuiShare.pf_dumpData1(IntPtr.Zero, 0, 0, 0, false, false,fn);
								GuiShare.pf_dumpData1((IntPtr)pVh_stream->mem0.spsPps, pVh_stream->mem0.usLen, 0, 0, true, false,fn);
								GuiShare.pf_dumpData1((IntPtr)pVh_stream->mem1.spsPps, pVh_stream->mem1.usLen, 0, 0, true, false,fn);
								GuiShare.pf_dumpData1((IntPtr)pVh_stream->mem2.spsPps, pVh_stream->mem2.usLen, 0, 0, true, false,fn);

							}
#endif
						}
						
					}

					//
				}
#endif

			}




			//		
			if (!pCompressVideo.ucbUseEvt)
			{
				if (pCompressVideo.compressor.common.head.ucCompressors != Consts.CONST_videoCompressors_stream)
				{
					if (pCompressVideo.iCompressBufSize > 0)
					{
						if (null == (pCompressVideo.pCompressBuf = (byte*)mymalloc(pCompressVideo.iCompressBufSize))) goto errLabel;
					}
				}
			}

			//
			size = sizeof(TRANSFER_VIDEO_DATA);
			if (null == (pCompressVideo.pReqBuf_myVideoCapture = (TRANSFER_VIDEO_DATA*)mymalloc(size))) goto errLabel;
			mymemset((IntPtr)pCompressVideo.pReqBuf_myVideoCapture, 0, size);

			int tt = 0;
			pCompressVideo.uiTranNo_openAvDev_org = getuiNextTranNo(null, Consts.MAX_sharedObjUsrs, ref tt);  //  2010/09/04


			if (0 != uiBufSize_pData)
			{
				if (null == (pCompressVideo.pData = (void*)mymalloc((int)uiBufSize_pData))) goto errLabel;
				mymemset((IntPtr)pCompressVideo.pData, 0, (int)uiBufSize_pData);
			}

			pCompressVideo.bVideoCompressInited = true;

			iErr = 0;
		errLabel:
			if (pBmpInfo != null) myfree((IntPtr)pBmpInfo);
			if (iErr != 0)
			{
				exitCompressVideo(pProcInfo0, bCapDev, ref pCompressVideo);
			}

			//  2012/11/06	
			qyFuncs.showInfo_open(0,"","initCompressVideo leaves ");



			return iErr;
		}


		//
		public static unsafe int exitCompressVideo(CCtxQmc pProcInfoParam, bool bCapDev, ref COMPRESS_VIDEO pCompressVideo)
		{
			int iErr = -1;


			CCtxQmc pProcInfo = pProcInfoParam;
			if (null == pProcInfo) return -1;
			//QY_MC	*	pQyMc		=	(  QY_MC  *  )pProcInfo->pQyMc;//
			CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
			if (null == pCusModules) return -1;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//if (!pCompressVideo) return -1;

			//		
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("exitCompressVideo enters. %s"), pCompressVideo.tag_dbg);

			//  2015/10/11
				//
				if (0 == pCompressVideo.compressor.common.head.ucCompressors) ;
				else if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_vcm)
				{
					/*
					if (pCompressVideo.u.vcm.hicLocal)
					{
						ICClose(pCompressVideo.u.vcm.hicLocal); pCompressVideo.u.vcm.hicLocal = null;
					}
					*/
				}
				else if (pCompressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_dmo)
				{
					//exitCompressVideo_dmo(&pCompressVideo.u.dmo);
				}
				else
				{

					CUS_MODULE_compress pModule = null;

					M_getCompressModule(pCusModules, pCompressVideo.uiModuleType_toEnc, ref pModule);  //  2012/09/09

					if (pModule != null)
					{
						if (pModule.pf_qdcExitCompressVideo != null)
						{
							pModule.pf_qdcExitCompressVideo(ref pCompressVideo.u_qoi.myQoi.common);
						}


						//
					}
				}
				//

			//
			//  2014/03/25
			clean_myDRAW_VIDEO_DATA((IntPtr)pCompressVideo.pCurPkt, ("exitCompressVideo"));
			byte* pCurPkt = (byte*)pCompressVideo.pCurPkt;
			MACRO_mysafeFree(ref pCurPkt);
			pCompressVideo.pCurPkt = (myDRAW_VIDEO_DATA*)pCurPkt;

			//	 
			//  2014/03/24
			if (pCompressVideo.pInDataCacheQ != null)
			{
				genericQFree(pCompressVideo.pInDataCacheQ); pCompressVideo.pInDataCacheQ = null;
			}
			if (pCompressVideo.pTmpDataQ != null)
			{
				genericQFree(pCompressVideo.pTmpDataQ); pCompressVideo.pTmpDataQ = null;
			}

			//
			if (pCompressVideo.pReqBuf_myVideoCapture != null)
			{
				myfree((IntPtr)pCompressVideo.pReqBuf_myVideoCapture); pCompressVideo.pReqBuf_myVideoCapture = null;
			}
			if (pCompressVideo.pCompressBuf != null)
			{
				myfree((IntPtr)pCompressVideo.pCompressBuf); pCompressVideo.pCompressBuf = null;
			}



			//
			if (pCompressVideo.pData != null)
			{

				/*
				if (!bCapDev)
				{
					exitAvRecord(pCompressVideo);
				}
				*/

				myfree((IntPtr)pCompressVideo.pData); pCompressVideo.pData = null;
			}

			//  pCompressVideo.uiTranNo_openAvDev  =  0;	//  ×¢ÒâÇå0
			//  pCompressVideo.bVideoCompressInited  =  false;		//  ×¢Òâ¿ª¹ØÁ¿¡£

			//mymemset(pCompressVideo, 0, sizeof(COMPRESS_VIDEO));
			pCompressVideo.memset0();


			iErr = 0;

			//  errLabel:


			//		
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("exitCompressVideo leaves. %s"), pCompressVideo.tag_dbg);



			return iErr;

		}


	}
}

