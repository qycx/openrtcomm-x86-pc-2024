using System;
using System.Collections.Generic;
using System.Text;

partial class Consts {
	public const string CONST_mutexNamePrefix_encA = ("mtxEncA");
};


namespace qy
{
    partial class qyFuncs
{

	// int  initCompressVideo(  void  *  pProcInfoParam,  BITMAPINFO  *  pBmpInfo_input,  VIDEO_COMPRESSOR_CFG  *  pCompressor,  BOOL  bCapDev,  unsigned  short  usCapDriverIndex,  unsigned  int  uiBufSize_pData,  COMPRESS_VIDEO  *  pCompressVideo  )
	public static unsafe int initCompressAudio(CCtxQmc pProcInfo, ref WAVEFORMATEX pWf_input, ref AUDIO_COMPRESSOR_CFG pCompressor, ref COMPRESS_AUDIO p)
	{
		int iErr = -1;
		//
		if (null==pProcInfo) return -1;
		CCtxQyMc	pQyMc				=	g.g_pQyMc;//
		//if  (  !pQyMc  )  return  -1;
		CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
		if (null==pCusModules) return -1;
		//
		WAVEFORMATEX* pWfDst = null;
		WAVEFORMATEX* pWfPcm = null;

			bool bDbg = false;
			string str;

			/*
		HACMDRIVERID hadid = null;

		MMRESULT mmr = 0;
		HACMDRIVER had = null;
		HACMSTREAM hstr1 = null;
		HACMSTREAM hstr2 = null;
			*/

		//if (!pWf_input) return -1;
		if (p.bCompressInited) return 0;

		traceLogA("initAudioCompress  enters");

		//  if  (  syncObj.sync(  pProcInfo->cfg.mutexName_syncAv  )  )  goto  errLabel;

		//
		p.ucTimeoutInS_showFrameInfoResp = Consts.MAX_ucTimeoutInS_showFrameInfoResp;
		//  p.uiBytesRecorded_perBlockAlign_expected  =  pWf_input->nSamplesPerSec  /  5;
		//  ³õÊ¼»¯¿ØÖÆ²ÎÊý
		p.ucTimeoutInS_showFrameInfoResp = Consts.MAX_ucTimeoutInS_showFrameInfoResp;

			//
			//  makeWaveFormat_pcm(  DEFAULT_audio_nChannels,  DEFAULT_audio_iHertz,  DEFAULT_audio_wBitsPerSample,  &p.waveFormatEx  );
			//memcpy(&p.ah_decompress.wfx, pWf_input, sizeof(p.ah_decompress.wfx)  );
			p.ah_decompress.wfx= pWf_input;

			//  ×÷Ò»µãÐ£Ñé
			if (Consts.MAX_uiBytesRecorded_perBlockAlign * p.ah_decompress.wfx.nBlockAlign > Consts.bufSize_transferAudioData)
		{
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("bufSize_transferAudioData %d too small, should be >= %d"), CONST_bufSize_transferAudioData, MAX_uiBytesRecorded_perBlockAlign * p.ah_decompress.wfx.nBlockAlign);
			goto errLabel;
		}
		if (0==p.ah_decompress.wfx.nAvgBytesPerSec) goto errLabel;


			/*
			unsigned  int  uiBytesRecorded_perBlockAlign;	uiBytesRecorded_perBlockAlign  =  pProcInfo->cfg.policy.audio.uiBytesRecorded_perBlockAlign;
			if  (  uiBytesRecorded_perBlockAlign  >=  MIN_uiBytesRecorded_perBlockAlign  &&  uiBytesRecorded_perBlockAlign  <=  MAX_uiBytesRecorded_perBlockAlign  )  {
				p.uiBytesRecorded_perBlockAlign_expected  =  uiBytesRecorded_perBlockAlign;
			}
			*/

			//  
			//memcpy(&p.compressor, pCompressor, sizeof(p.compressor)  );
			p.compressor= pCompressor;

			//  2015/02/13
			ref COMPRESS_AUDIO pCompressAudio = ref p;

			//  2014/04/02		 
			//  2014/03/23	 	
			GENERIC_Q_cfgEx qCfgEx; //  2012/11/10
		GENERIC_Q_CFG tmpCfg;

		//TCHAR tBuf[128];
		int tn = (int)myGetTickCount(null);
			int size;
			/*
			size = sizeof(GENERIC_Q);
		pCompressAudio.pInDataCacheQ = (GENERIC_Q*)mymalloc(size);
		if (!pCompressAudio.pInDataCacheQ) goto errLabel;
		memset(pCompressAudio.pInDataCacheQ, 0, size);
			*/
			pCompressAudio.pInDataCacheQ = new GENERIC_Q();

		//
		mymemset((IntPtr)(byte*)&tmpCfg, 0, sizeof(GENERIC_Q_CFG));
			//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-in-%d"), CONST_mutexNamePrefix_encA, tn);
			str=string.Format("{0}-in-{1}", Consts.CONST_mutexNamePrefix_encA, tn);
			mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
			//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-in-%d"), CONST_mutexNamePrefix_encA, tn);
			str=string.Format("{0}-in-{1}", Consts.CONST_mutexNamePrefix_encA, tn);
			mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
			tmpCfg.uiMaxQNodes = 100;

			//mymemset(&qCfgEx, 0, sizeof(qCfgEx));
			qCfgEx = new GENERIC_Q_cfgEx();
			//memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
			qCfgEx.common=tmpCfg;
			qCfgEx.pfQElemNewEx = mallocMemory;
		qCfgEx.pfQElemNew = mymalloc;
		qCfgEx.pfQElemFreeEx = freeMemory;
		qCfgEx.pfQElemFree = myfree;
		qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myPLAY_AUDIO_DATA;
		qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_myPLAY_AUDIO_DATA;

		if (0!=initGenericQ(qCfgEx, (GENERIC_Q)pCompressAudio.pInDataCacheQ)) goto errLabel;



		//
		size = sizeof(myPLAY_AUDIO_DATA);
		pCompressAudio.pCurPkt = (myPLAY_AUDIO_DATA*)mymalloc(size);
		if (null==pCompressAudio.pCurPkt) goto errLabel;
		mymemset((IntPtr)pCompressAudio.pCurPkt, 0, size);






			////////////////////////////////////
			int tt = 0;
		p.uiTranNo_openAvDev_org = getuiNextTranNo(null, Consts.MAX_sharedObjUsrs, ref tt);   //  2010/09/04

		//
		if (p.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_acm)
		{
				traceLog("not supported");
				goto errLabel;


			}
	else if (p.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_dmo)
			{

				//if (initCompressAudio_dmo(&p.compressor, &p.ah_decompress, &p.ah_compress, &p.u.dmo)) goto errLabel;
				traceLog("not supported");
				goto errLabel;

			}
			else
			{
				CUS_MODULE_compress pModule = null;
				uint uiModuleType = 0;             //  2012/09/09

				//
				uiModuleType = Consts.CONST_moduleType_ffmpeg;

				/*
				switch (p.compressor.common.ucCompressors)
				{
					case CONST_audioCompressors_ipp:
						//
						uiModuleType = CONST_moduleType_ipp;
						//
						pModule = pCusModules->pIpp;
						break;
					//  2016/12/05
					case CONST_audioCompressors_vorbis:
						//
						uiModuleType = CONST_moduleType_qdc_vorbis_enc;
						//
						pModule = pCusModules->pQdc_vorbis_enc;
						break;
					case CONST_audioCompressors_custom:
						//
						uiModuleType = CONST_moduleType_compress;
						//
						pModule = getCusModule(pCusModules, CONST_moduleType_compress);
						break;
					default:
# ifdef  __DEBUG__
						traceLogA("initCompressAudio: ucCompressors err: %d", p.compressor.common.ucCompressors);
#endif
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initCompressAudio: ucCompressors err: %d"), p.compressor.common.ucCompressors);
						goto errLabel;
						break;
				}
				*/

				//
				M_getCompressModule(pCusModules, uiModuleType, ref pModule);    //  2012/09/09
				if (null==pModule || null==pModule.pf_qdcInitCompressAudio) goto errLabel;
				p.uiModuleType_toEnc = uiModuleType;



				///////////////////////////////
				//  2016/12/09

				//  2014/04/02
				AUDIO_COMPRESSOR_CFG compressor;
				//memcpy(&compressor, &pCompressAudio.compressor, sizeof(compressor));
				compressor=pCompressAudio.compressor;
				/*
				compressor.common.pCurPkt = pCompressAudio.pCurPkt;
				compressor.common.size_pCurPkt = sizeof(pCompressAudio.pCurPkt[0]  );
				//
				compressor.common.pInDataCacheQ = pCompressAudio.pInDataCacheQ;
				//
				compressor.common.pAudioQ2 = pCompressAudio.compressor.common.pAudioQ2;    //  2014/04/03. pVideoQ2这时尚未初始化。等数据流运行时，pVideoQ2已经ready
				if (!compressor.common.pAudioQ2)
				{
					showInfo_open(0, 0, _T("initCompressVideo failed, pVideoQ2 is null"));
					goto errLabel;
				}
				//compressor.common.pTmpDataQ  =  pCompressAudio.pTmpDataQ;
				//
				compressor.common.pParent_transform = pCompressAudio.compressor.common.pParent_transform;
				if (!compressor.common.pParent_transform)
				{
					goto errLabel;
				}
				compressor.common.uiTransformType_pParent = CONST_uiTransformType_encAudio;
				//
				compressor.common.pf_bTransformAudioQuit = bCapAudioQuit;


				//
				//  compressor.common.pf_transformGetSpsPps  =  capBmpGetSpsPps;
				compressor.common.pf_transformGetAh = (PF_commonHandler)capAudioGetAh;


				//
				compressor.common.pf_getNextAPkt = (PF_commonHandler)getNextAPkt;

				//compressor.common.pf_parseVPkt_i  =  enc_parseOrgVPkt_i;
				compressor.common.pf_parseDecAPkt_o = parseDecAPkt_o;


				compressor.common.pf_detachPkt = detachAPkt;
				compressor.common.pf_clean_myPLAY_AUDIO_DATA = (PF_clean_myPLAY_AUDIO_DATA)clean_myPLAY_AUDIO_DATA;


				//
				compressor.common.enc.pf_smplPCMReader_LoadNextFrame = smplPCMReader_LoadNextFrame;
				compressor.common.enc.pf_doPostEnc = doPostEnc_audio;
				*/

				//  2014/03/29
				//  compressor.common.ucHardwareAccl  =  pQmcCfg->policy.ucHardwareAccl;

				//
				ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
				//if (pQmcCfg)
				{
					if (pQmcCfg.debugStatusInfo.ucb__Dump_mp3_enc__)
					{
						GuiShare.pf_dumpAudio_init();
					}
				}


				////////////////////////

				//
				p.u_qoi.myQoi.common.uiType = Consts.CONST_qoiType_encA;
				//			
				//  2016/04/26							
				//pProcInfo->set_qoi_funcs(&pCompressAudio.u.qoi.myQoi);

				//
#if DEBUG
				traceLog("initCompressAudio.276, before qdcInitCompressAudio, moduleType " + uiModuleType);
#endif

				//
				if (0 != pModule.pf_qdcInitCompressAudio(ref compressor, ref p.ah_decompress, ref p.ah_compress, ref p.u_qoi.myQoi.common))
				{
					traceLog("initCompressAudio.282, qdcInitCompressAudio failed");
					goto errLabel;
				}

				//
#if DEBUG
				traceLog("initComprssAudio.288, after qdcInitCompressAudio");
#endif


				//  
				if (p.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_vorbis)
				{
					/*
					if (!p.u.qoi.pf_qdcGetVorbisHeaders)
					{
						goto errLabel;
					}
					//
					memset(&p.ah_stream, 0, sizeof(p.ah_stream)  );
					AH_stream* pAh_stream = &p.ah_stream;
					char buf0[256] = "";
					char buf1[256] = "";
					char buf2[5 * 1024] = "";
					//
					unsigned  int header0Len = sizeof(buf0);
					unsigned  int header1Len = sizeof(buf1);
					unsigned  int header2Len = sizeof(buf2);
					if (p.u.qoi.pf_qdcGetVorbisHeaders(&p.u.qoi.myQoi.common, buf0, &header0Len, buf1, &header1Len, buf2, &header2Len))
					{
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("initCompressAudio failed, qdcGetVorbisHeaders failed."));
						goto errLabel;
					}
					//
					if (header0Len + header1Len + header2Len > sizeof(p.ah_stream.buf)  )  goto errLabel;
					//				
					int iPos = 0;
					pAh_stream->mems[0].usLen = header0Len; memcpy(pAh_stream->buf, buf0, header0Len);
					iPos += header0Len;
					pAh_stream->mems[1].usLen = header1Len; memcpy(pAh_stream->buf + iPos, buf1, header1Len);
					iPos += header1Len;
					pAh_stream->mems[2].usLen = header2Len; memcpy(pAh_stream->buf + iPos, buf2, header2Len);
					//
					//pVh_stream->dwFourcc  =  pCompressVideo->vh_compress.bih.biCompression;				
					//  2014/05/02				  
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("qdcGetSpsPps: header0Len %d, header1Len %d, header2Len %d"), header0Len, header1Len, header2Len);

					//
# if  __DEBUG__
#if  true
			  int  iii;
			  for  (  iii  =  0;  iii  <  58;  iii  ++  )  {
				   TCHAR  tt[128];
				   _sntprintf(  tt,  mycountof(  tt  ),  _T(  "header[%d] %02x\n"  ),  iii,  (  unsigned  char  )pAh_stream->buf[iii]  );
				   OutputDebugString(  tt  );
			  }
			  iii  =  iii;
#endif
#endif
					*/

				}

			}

			//
			size = sizeof(TRANSFER_AUDIO_DATA);
			p.pReqCache = (TRANSFER_AUDIO_DATA*)mymalloc(size);
			if (null==p.pReqCache) goto errLabel;
			mymemset((IntPtr)p.pReqCache, 0, size);


			p.bCompressInited = true;

			iErr = 0;

		errLabel:


			if (null!=pWfDst) myfree((IntPtr)pWfDst);
			if (null!=pWfPcm) myfree((IntPtr)pWfPcm);

			if (0!=iErr)
			{
				exitCompressAudio(pProcInfo, ref p);
			}

			//
			traceLog("initAudioCompress  leaves ");

			//
			return iErr;
			}

			public static unsafe int exitCompressAudio(CCtxQmc pProcInfo, ref COMPRESS_AUDIO pCompressAudio)
			{
				int iErr = -1;
				//QY_MC	*	pQyMc	=	(  QY_MC  *  )pProcInfo->pQyMc;//
				//  CQySyncObj			syncObj;
				if (null==pProcInfo) return -1;
				CUS_MODULES pCusModules = (CUS_MODULES)pProcInfo.getCusModules();
				if (null==pCusModules) return -1;


				traceLogA("exitAudioCompress  enters");

				//  if  (  syncObj.sync(  pProcInfo->cfg.mutexName_syncAv  )  )  goto  errLabel;

				if (0==pCompressAudio.compressor.common.head.ucCompressors) ;
				else if (pCompressAudio.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_acm)
				{


				}
				else if (pCompressAudio.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_dmo)
				{
					//exitCompressAudio_dmo(&pCompressAudio.u.dmo);
				}
				else
				{
					CUS_MODULE_compress pModule = null;
				uint uiModuleType = pCompressAudio.uiModuleType_toEnc;

				/*
					switch (pCompressAudio.compressor.common.ucCompressors)
					{
						case CONST_audioCompressors_ipp:
							pModule = pCusModules->pIpp;
							break;
						case CONST_audioCompressors_vorbis: //  2016/12/05
							pModule = pCusModules->pQdc_vorbis_enc;
							break;
						case CONST_audioCompressors_custom:
							pModule = getCusModule(pCusModules, CONST_moduleType_compress);
							break;
						default:
							traceLogA("exitCompressAudio: ucCompressors err: %d", pCompressAudio.compressor.common.ucCompressors);
							qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("exitCompressAudio: ucCompressors err: %d"), pCompressAudio.compressor.common.ucCompressors);
							break;
					}
				*/

				M_getCompressModule(pCusModules, uiModuleType, ref pModule);    //  2012/09/09


				if (null != pModule)
				{
					if (null != pModule.pf_qdcExitCompressAudio)
					{
						pModule.pf_qdcExitCompressAudio(ref pCompressAudio.u_qoi.myQoi.common);
					}

				}

				//
				ref QMC_cfg pCfg = ref pProcInfo.cfg;
					//if (pCfg)
					{
						if (pCfg.debugStatusInfo.ucb__Dump_mp3_enc__)
						{
							GuiShare.pf_dumpAudio_exit();
						}
					}

				}


				//  2015/02/13
				//  2014/03/25
				clean_myPLAY_AUDIO_DATA((IntPtr)pCompressAudio.pCurPkt, _T("exitCompressAudio"));
			IntPtr tmpp = (IntPtr)pCompressAudio.pCurPkt;
				MACRO_safeFree(ref tmpp);
			pCompressAudio.pCurPkt = (myPLAY_AUDIO_DATA*)tmpp;

				//	 
				//  2014/03/24
				if (null!=pCompressAudio.pInDataCacheQ)
				{
				//genericQFree(pCompressAudio.pInDataCacheQ); pCompressAudio.pInDataCacheQ = null;
				pCompressAudio.pInDataCacheQ = null;
				}

				//
				if (null!=pCompressAudio.pReqCache)
				{   //  2009/02/11
					myfree((IntPtr)pCompressAudio.pReqCache); pCompressAudio.pReqCache = null;
				}

				if (null!=pCompressAudio.pDstBuf)
				{
					myfree((IntPtr)pCompressAudio.pDstBuf); pCompressAudio.pDstBuf = null;
				}
				if (null!=pCompressAudio.pPcmBuf)
				{
					myfree((IntPtr)pCompressAudio.pPcmBuf); pCompressAudio.pPcmBuf = null;
				}

				//  if  (  p.pWfDst  )  {
				//  	free(  p.pWfDst  );  p.pWfDst  =  null;
				//  }
				//  if  (  p.pWfPcm  )  {
				//  	free(  p.pWfPcm  );  p.pWfPcm  =  null;
				//  }

				pCompressAudio.bCompressInited = false;
			//memset(pCompressAudio, 0, sizeof(pCompressAudio[0]));
			pCompressAudio.memset0();

				iErr = 0;

				//  errLabel:

				traceLogA("exitAudioCompress  leaves ");

				//
				return iErr;
			}

		}
	}
