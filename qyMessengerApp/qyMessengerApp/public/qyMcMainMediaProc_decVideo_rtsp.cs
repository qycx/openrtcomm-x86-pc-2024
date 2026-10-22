
//

//
using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const string CONST_mutexNamePrefix_decV_inDataCacheQ = ("decV_in");
public const string CONST_mutexNamePrefix_decV_tmpDataQ = ("decV_tmp");

}

namespace qy
{

	partial class qyFuncs
{

		public static unsafe void postVPktToRender(QY_TRANSFORM pTransform, void* pmyDRAW_VIDEO_DATA, bool* pbPktRedirected)
		{
			int iErr = -1;
			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pmyDRAW_VIDEO_DATA;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return;

			bool bPktsRedirected = false;

			//
			//if (null!=pTransform.pCapDev_rtsp)
			if (false)
			{
				/*
				//VIDEO_capDev_rtsp				*	pRtsp							=	(  VIDEO_capDev_rtsp  *  )pTransform.pCapDev_rtsp;
				CAP_dev_rtspCommon* pRtsp = (CAP_dev_rtspCommon*)pTransform.pCapDev_rtsp;

				QY_SHARED_OBJ* pSharedObj = getSharedObjByIndex(pProcInfo, pRtsp->iIndex_sharedObj);
				if (!pSharedObj) goto errLabel;
				CAP_procInfo_video pVc = (CAP_procInfo_video)getCapBmpBySth(pProcInfo, pSharedObj->iIndex_capBmp, 0);
				if (!pVc) goto errLabel;
				TRANSINFO_stream* pTransInfo = (TRANSINFO_stream*)pSharedObj->pTransInfo_stream;
				if (!pTransInfo) goto errLabel;
				if (!pTransInfo->myTask.pTransforms) goto errLabel;


				//  dec可能是异步的，所以可能没输出数据。这时候，pkt.bih没赋值，所以需要判断下. 2014/03/16					 
				if (pPkt->bih.biWidth && pPkt->bih.biHeight)
				{
					//
					if (pPkt->bih.biWidth != pVc->vh_org.bih.biWidth || pPkt->bih.biHeight != pVc->vh_org.bih.biHeight)
					{
# if  __DEBUG__
						traceLog(_T("postVPktToRender failed, pkt %dx%d != pVc.vh_org %dx%d"), pPkt->bih.biWidth, pPkt->bih.biHeight, pVc->vh_org.bih.biWidth, pVc->vh_org.bih.biHeight);
#endif
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("postVPktToRender failed, pkt %dx%d != pVc.vh_org %dx%d"), pPkt->bih.biWidth, pPkt->bih.biHeight, pVc->vh_org.bih.biWidth, pVc->vh_org.bih.biHeight);
						goto errLabel;
					}

					//
					if (pRtsp->uiType == CONST_rtspType_decV)
					{
						VIDEO_capDev_rtsp* pRtsp1 = (VIDEO_capDev_rtsp*)pRtsp;
						//  2015/02/01
						if (!pRtsp1->bQInited_dvt_decV)
						{
							showInfo_open(0, 0, _T("postVPktTotender failed: cacheQ not inited"));
							goto errLabel;
						}
						//
						//  2015/10/06
						ref QMC_cfg pCfg = ref pProcInfo.cfg;
						if (pCfg && pCfg->debugStatusInfo.ucbShowRenderStatus)
						{
							if (pPkt->usPktResType == CONST_pktResType_sharedTex)
							{
								TCHAR tBuf[128];
								_sntprintf(tBuf, mycountof(tBuf), _T("postVPktToRender: sn %d ok"), pPkt->sharedTex.pktSharedTexInfo.uiSeqNo);
								showInfo_open(0, 0, tBuf);
							}
						}
						//
# ifdef  __DEBUG__  //  2015/10/06
						//showInfo_open(  0,  0,  _T(  "postVPktTotender: haha"  )  );			
#endif
						//
						if (0==q2PostMsgAndTrigger(pPkt, 1 * sizeof(myDRAW_VIDEO_DATA), &pRtsp1->cacheQ2_dvt_decV))
						{
							bPktsRedirected = true;
						}

					}
					else
					{

						//  2014/09/08
						if (!pVc->vppThread.bQInited)
						{
							showInfo_open(0, 0, _T("postVPktToRender failed: vpp.q not inited"));
							goto errLabel;
						}

						//
						bool bNoSkip = false;
						//
						if (pSharedObj->uiType == CONST_sharedObjType_qvcf)
						{   //  2015/05/25. 本地播放，不需要帧速控制
							bNoSkip = true;
						}
						//				 
						if (!setPktInputInfo(pProcInfo, ref pVc->compressVideo.input, pVc->compressVideo.usFrames_perSecond_expected, bNoSkip, pPkt))
						{
							//
							if (!q2PostMsgAndTrigger(pPkt, 1 * sizeof(pPkt[0]), &pVc->vppThread.q2))
							{
								bPktsRedirected = true;
							}
						}

					}
				}

				*/
			}
			else
			{
				//  drawVideoData(  pProcInfo,  pPkt,  1  *  sizeof(  pPkt[0]  ),  &bPktsRedirected,  pTransform.iIndex_player  );		
				pProcInfo.drawVideoData(pPkt, 1 * sizeof(myDRAW_VIDEO_DATA), &bPktsRedirected, pTransform);
			}

			//
			iErr = 0;

		errLabel:
			if (null!=pbPktRedirected)
			{
				*pbPktRedirected = bPktsRedirected;
			}
			return;

		}

		public static unsafe bool bTransformAudioQuit(object pQY_TRANSFORM, IntPtr p1, IntPtr p2)
		{
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;

# if  __DEBUG__
			if (pTransform.audio.bQuit)
			{
				int i;
				i = 1;
			}
#endif

			return pTransform.audio.bQuit;
		}


		public static unsafe bool bTransformVideoQuit(object pQY_TRANSFORM, void* p1, void* p2)
		{
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;

# if  __DEBUG__
			if (pTransform.video.bQuit)
			{
				int i;
				i = 1;
			}
#endif

			return pTransform.video.bQuit;
		}

		// int  transformGetSpsPps(  void  *  pQY_TRANSFORM,  char  *  pBuf,  unsigned  int  *  puiSize  )
		public static unsafe int transformGetSpsPps(uint uiTransformType, object pQY_TRANSFORM, int index_pMems_from, byte* pBuf, uint* puiSize)
		{
			int iErr = -1;
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;
			TRANSFORM_VIDEO_procInfo pTransformVideo = pTransform.video;

			int total = 0;
			int i;

			//
			if (pTransformVideo.tv.vh_stream.usCnt > Consts.cntof_VH_stream_mems) return -1;
			//
			for (i = 0; i < pTransformVideo.tv.vh_stream.usCnt; i++)
			{
				VH_stream_mem mem;
				switch (i)
				{
					case 0:
						mem = pTransformVideo.tv.vh_stream.mem0;
						break;
					case 1:
						mem = pTransformVideo.tv.vh_stream.mem1;
						break;
					case 2:
						mem = pTransformVideo.tv.vh_stream.mem2;
						break;
					default:
						goto errLabel;
				}
				if (total + mem.usLen > *puiSize) return -1;
				mymemcpy((IntPtr)(pBuf + total), (IntPtr)mem.spsPps, mem.usLen);
				total += mem.usLen;

				//  2014/08/31
				
			}

			//
			string str;
			str=string.Format("transformGetSpsPps: %I64u, cnt %d", pTransform.video.idInfo.ui64Id, (int)pTransformVideo.tv.vh_stream.usCnt);
			for (i = 0; i < pTransformVideo.tv.vh_stream.usCnt; i++)
			{
				//str=string.Format("%s,%d", str, (int)pTransformVideo->tv.vh_stream.mems[i].usLen);
			}
			showInfo_open(0, null, str);

			//
			*puiSize = (uint)total;

			iErr = 0;
			errLabel:

			return iErr;
		}



		//
		public static unsafe int transformGetBih(object pQY_TRANSFORM, BITMAPINFOHEADER* pBih, void* p2)
		{
			int iErr = -1;
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQY_TRANSFORM;
			TRANSFORM_VIDEO_procInfo pTransformVideo = pTransform.video;

			//memcpy(pBih, &pTransformVideo->tv.vh_decompress.bih, sizeof(pBih[0]));
			*pBih = pTransformVideo.tv.vh_decompress.bih;

			//
			iErr = 0;
			return iErr;
		}


        //
#if false
		public static unsafe int dec_parseEncAPkt_i_useAudioQ2(uint uiTransformType, object pParent_transform, ref PKT_info_toDec pPktInfo_toDec_param, int* p_old_lPktId_getNewPkt, byte** ppInput, uint* puiSize, uint* puiSampleTimeInMs, uint* puiPts)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();    //  QY_GET_procInfo_isCli(  );
														  //TCHAR tBuf[256] = _T("");


			//  myDRAW_VIDEO_DATA  *  pPkt  =  pTransform.video.pCurPkt;
			fixed (PKT_info_toDec* pPktInfo_toDec = &pPktInfo_toDec_param)
			{
				;

				//  检查一下。每个包都只能处理一次，否则，容易死循环
				if (null == p_old_lPktId_getNewPkt) return -1;
				if (0 == pPktInfo_toDec->lPktId_getNewPkt)
				{
					showInfo_open(0, null, _T("failed: lPktId_alloc 0"));
					return -1;
				}
				if (pPktInfo_toDec->lPktId_getNewPkt == *p_old_lPktId_getNewPkt)
				{
					showInfo_open(0, null, _T("failed: lPkdId_alloc==old"));
					return -1;
				}
				*p_old_lPktId_getNewPkt = pPktInfo_toDec->lPktId_getNewPkt;

				//
				if (uiTransformType != Consts.CONST_uiTransformType_dec)
				{
					//MACRO_qyAssert(0, _T("dec_parseEncVPkt failed, transformType err"));
					return -1;
				}
				QY_TRANSFORM pTransform = (QY_TRANSFORM)pParent_transform;
				//pProcInfo = (MC_VAR_common*)pTransform.pProcInfo;
				if (null == pProcInfo) return -1;

				ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
				//if (!pQmcCfg) return -1;

				//
				if (!pPktInfo_toDec->bGot_msg) return -1;
				if (!pPktInfo_toDec->info.bGot_pkt) return -1;



				//  2015/09/11
				MIS_MSGU* pMsg = (MIS_MSGU*)pPktInfo_toDec->pMIS_MSGU;
				if (pMsg->uiType != Consts.CONST_misMsgType_input)
				{
					traceLog("dec_parseEncAPkt_i failed, only input msg accepted");
					goto errLabel;
				}
				IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->input.data.buf;
				if (pContent->uiType != Consts.CONST_imCommType_transferAudioData) goto errLabel;

				//
				TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
				TRANSFER_AUDIO_dataMemHead* pMem;
				int i;
				int iPos;

				//
				iPos = pPktInfo_toDec->info.iPos;
				if (pPktInfo_toDec->info.index_mems < 0 || pPktInfo_toDec->info.index_mems >= pReq->usCnt) goto errLabel;
				i = pPktInfo_toDec->info.index_mems;

				TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
				int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
				//				
				pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

				if (iPos + pMem->uiLen > pReq->uiLen)
				{
#if __DEBUG__
				traceLogA("procTasAudio: ");
#endif
					goto errLabel;
				}
				if (0 == pMem->uiLen)
				{
#if __DEBUG__
				traceLogA("procTaskAudio warning: mem.uiLen is 0");
#endif
					goto errLabel;
				}
				if (true)
				{
					if (0 != pMem->uiPts)
					{
						if (0 == pTransform.audio.decInfo.pts.uiPts_lastRecvd)
						{
							pTransform.audio.decInfo.pts.uiPts_first = pMem->uiPts;
							pTransform.audio.decInfo.pts.dwTickCnt_start = myGetTickCount(null);
						}
						else
						{
							if (pMem->uiPts != pTransform.audio.decInfo.pts.uiPts_lastRecvd + 1)
							{
								string str;
								str=string.Format("dec_parseEncAPkt_i: {0}: pts err, pkt {1}, last {2}", pMem->idInfo.ui64Id,  pMem->uiPts, pTransform.audio.decInfo.pts.uiPts_lastRecvd);
								showInfo_open(0, null, str);
								//  2015/02/20
								goto errLabel;
							}
							//
						}
						//pTransform.video.decInfo.pts.uiPts_lastRecvd  =  pPkt->head.uiPts;		
					}
				}
				//  2015/06/18
				if (0 != pMem->uiPts)
				{
					pTransform.audio.decInfo.pts.uiPts_lastRecvd = pMem->uiPts;
				}
				//
				ref QMC_cfg_debugStatusInfo pDbgStatusInfo = ref pProcInfo.cfg.debugStatusInfo;
				//if (pDbgStatusInfo)
				{
					if (pDbgStatusInfo.ucbShowPreDecAStatus)
					{
						/*
						TCHAR tBufff[129];
						_sntprintf(tBufff, mycountof(tBufff), _T("dec_parseEncAPkt_i: %I64u, mems[%d], pts %d"), pMem->idInfo.ui64Id, i, pMem->uiPts);
						showInfo_open(0, 0, tBufff);
						*/
					}
				}

				//
				byte* encData = pReq->buf + iPos;


				//
				*ppInput = (byte*)encData;
				*puiSize = pMem->uiLen;
				//  2015/01/28
				if (null != puiSampleTimeInMs)
				{
					*puiSampleTimeInMs = pMem->uiSampleTimeInMs;
				}
				//  2015/02/09
				if (null != puiPts)
				{
					*puiPts = pMem->uiPts;
				}

				//  2016/12/15
				pTransform.audio.decInfo.st.last_iSampleTimeInMs_preDec = (int)pMem->uiSampleTimeInMs;

			}

			iErr = 0;
		errLabel:

			return iErr;
		}
#endif


    

        //
        public static unsafe int dec_parseEncVPkt_i(uint uiTransformType, object pParent_transform, ref PKT_info_toDec pPktInfo_toDec_param, ref int p_old_lPktId_getNewPkt, byte** ppInput, uint* puiSize, uint* puiSampleTimeInMs, uint* puiPts)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();    //  QY_GET_procInfo_isCli(  );
                                                          //TCHAR tBuf[256] = _T("");


            //  myDRAW_VIDEO_DATA  *  pPkt  =  pTransform.video.pCurPkt;
            fixed (PKT_info_toDec* pPktInfo_toDec = &pPktInfo_toDec_param)
            {
                ;

                //  检查一下。每个包都只能处理一次，否则，容易死循环
                if (null == p_old_lPktId_getNewPkt) return -1;
                if (0 == pPktInfo_toDec->lPktId_getNewPkt)
                {
                    showInfo_open(0, null, _T("failed: lPktId_alloc 0"));
                    return -1;
                }
                if (pPktInfo_toDec->lPktId_getNewPkt == p_old_lPktId_getNewPkt)
                {
                    showInfo_open(0, null, _T("failed: lPkdId_alloc==old"));
                    return -1;
                }
                p_old_lPktId_getNewPkt = pPktInfo_toDec->lPktId_getNewPkt;

                //
                if (uiTransformType != Consts.CONST_uiTransformType_dec)
                {
                    //MACRO_qyAssert(0, _T("dec_parseEncVPkt failed, transformType err"));
                    return -1;
                }
                QY_TRANSFORM pTransform = (QY_TRANSFORM)pParent_transform;
                //pProcInfo = (MC_VAR_common*)pTransform.pProcInfo;
                if (null == pProcInfo) return -1;

                ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
                //if (!pQmcCfg) return -1;

                //
                if (!pPktInfo_toDec->bGot_msg) return -1;
                if (!pPktInfo_toDec->info.bGot_pkt) return -1;


                TRANSFER_VIDEO_dataMemHead* pMem;

#if false
				//  2015/09/11
				MIS_MSGU* pMsg = (MIS_MSGU*)pPktInfo_toDec->pMIS_MSGU;
				if (pMsg->uiType != Consts.CONST_misMsgType_input)
				{
					traceLog("dec_parseEncAPkt_i failed, only input msg accepted");
					goto errLabel;
				}
				IM_CONTENTU* pContent = (IM_CONTENTU*)pMsg->input.data.buf;
				if (pContent->uiType != Consts.CONST_imCommType_transferAudioData) goto errLabel;

				//
				TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
				int i;
				int iPos;

				//
				iPos = pPktInfo_toDec->info.iPos;
				if (pPktInfo_toDec->info.index_mems < 0 || pPktInfo_toDec->info.index_mems >= pReq->usCnt) goto errLabel;
				i = pPktInfo_toDec->info.index_mems;

				TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
				int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
				//				
				pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

				if (iPos + pMem->uiLen > pReq->uiLen)
				{
#if __DEBUG__
				traceLogA("procTasAudio: ");
#endif
					goto errLabel;
				}
#endif
                pMem = &pPktInfo_toDec->pkt_encV.memHead_v;
                if (0 == pMem->uiLen)
                {
#if __DEBUG__
				traceLogA("procTaskAudio warning: mem.uiLen is 0");
#endif
                    goto errLabel;
                }
                if (true)
                {
                    if (0 != pMem->uiPts)
                    {
                        if (0 == pTransform.video.decInfo.pts.uiPts_lastRecvd)
                        {
                            pTransform.video.decInfo.pts.uiPts_first = pMem->uiPts;
                            pTransform.video.decInfo.pts.dwTickCnt_start = myGetTickCount(null);
                        }
                        else
                        {
                            if (pMem->uiPts != pTransform.video.decInfo.pts.uiPts_lastRecvd + 1)
                            {
                                string str;
                                str = string.Format("dec_parseEncVPkt_i: {0}: pts err, pkt {1}, last {2}", pMem->idInfo.ui64Id, pMem->uiPts, pTransform.video.decInfo.pts.uiPts_lastRecvd);
                                showInfo_open(0, null, str);
                                //  2015/02/20
                                goto errLabel;
                            }
                            //
                        }
                        //pTransform.video.decInfo.pts.uiPts_lastRecvd  =  pPkt->head.uiPts;		
                    }
                }
                //  2015/06/18
                if (0 != pMem->uiPts)
                {
                    pTransform.video.decInfo.pts.uiPts_lastRecvd = pMem->uiPts;
                }
                //
                ref QMC_cfg_debugStatusInfo pDbgStatusInfo = ref pProcInfo.cfg.debugStatusInfo;
                //if (pDbgStatusInfo)
                {
                    if (pDbgStatusInfo.ucbShowPreDecAStatus)
                    {
                        /*
						TCHAR tBufff[129];
						_sntprintf(tBufff, mycountof(tBufff), _T("dec_parseEncAPkt_i: %I64u, mems[%d], pts %d"), pMem->idInfo.ui64Id, i, pMem->uiPts);
						showInfo_open(0, 0, tBufff);
						*/
                    }
                }

                //
                byte* encData = null;// pReq->buf + iPos;
                encData = pPktInfo_toDec->pkt_encV.buf;
                if (pPktInfo_toDec->pkt_encV.len != pMem->uiLen)
                {
                    int ii = 0;
                }


				//
				if (false)
				{
					string fn = Consts.CONST_fn_dump_h264_enc;
					//GuiShare.pf_dumpData1(IntPtr.Zero, 0, 0, 0, false, false, fn);
					GuiShare.pf_dumpData1((IntPtr)encData, (int)pMem->uiLen, 0, 0, true, false, fn);
				}

                //  2014/12/06
                /*
                uint dwTickCnt = myGetTickCount(null);
                bool bNeedChk = false;
                if (0 == pTransform.video.decInfo.cnt_postDec) bNeedChk = true;
                else
                {
                    if (dwTickCnt < pTransform.video.decInfo.dwLastTickCnt_postDec + 2000)
                    {
                        pTransform.video.decInfo.nTimes_needKeyFrame = 0;
                    }
                    else
                    {
                        pTransform.video.decInfo.nTimes_needKeyFrame++;
                        if (pTransform.video.decInfo.nTimes_needKeyFrame > 20)
                        {
                            bNeedChk = true;
                        }
                    }
                }

                //  2014/08/26
                if (bNeedChk)
                {

                    //
                    chkToForceKeyFrame(null, pTransform);

                    //
#if __DEBUG__
				//  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "dec_parseEncVPkt_i : cnt_preDec %d, cnt_postDec %d"  ),  pTransform.video.decInfo.cnt_preDec,  pTransform.video.decInfo.cnt_postDec  );
#endif


                }
				*/

                //
                uint iFourcc_enc = pTransform.video.tv.vh_stream.dwFourcc;
				if (iFourcc_enc == Consts.CONST_fourcc_HEVC)
				{

					//
					int nalType = M_nalType_h265(encData);

					//
#if __DEBUG__
#if false
				 _sntprintf(tBuf, mycountof(tBuf), _T("dec_parseEncVPkt_i: nalType %d, pts %d, len %d"), nalType, pPkt->head.uiPts, pPkt->uiLen_enc);
				 showInfo_open(0, 0, tBuf);
#endif
#endif

					//
					if (!pTransform.video.decInfo.bGot_keyFrame)
					{
						bool ucbKeyFrame = false;
						//
						switch (nalType)
						{
							case Consts.NAL_IDR_W_RADL:
							case Consts.NAL_IDR_N_LP:
								ucbKeyFrame = true;
								break;
							case Consts.NAL_VPS:
								ucbKeyFrame = true;
								break;
							default:
								{
									int ii = 0;
								}
								break;
						}
						//
						if (!ucbKeyFrame)
						{
							//
							string str;
							str = ("dec_parseEncVPkt_i failed, %I64u, %d, not a keyFrame: need a keyFrame first") +
								pTransform.video.idInfo.ui64Id + " " + pTransform.video.tv.uiTranNo_openAvDev;
							showInfo_open(0, "", str);
							//
							chkToForceKeyFrame(null, pTransform);
							//
							iErr = Consts.CONST_qyRet_needKeyFrameFirst;
							//
							goto errLabel;
						}
						pTransform.video.decInfo.bGot_keyFrame = true;
					}

				}


            


            //  2014/






            //
            *ppInput = (byte*)encData;
                *puiSize = pMem->uiLen;
                //  2015/01/28
                if (null != puiSampleTimeInMs)
                {
                    *puiSampleTimeInMs = pMem->uiSampleTimeInMs;
                }
                //  2015/02/09
                if (null != puiPts)
                {
                    *puiPts = pMem->uiPts;
                }

                //  2016/12/15
                //pTransform.video.decInfo..st.last_iSampleTimeInMs_preDec = (int)pMem->uiSampleTimeInMs;

            }

            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public static unsafe int doDecodeVideo(QY_TRANSFORM pTransform, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;

			int i;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;

			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;


			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			//if (null==pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			/*
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null==pTranInfo_unused) return -1;
			*/
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return -1;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller,(CCtxQmc)pProcInfo, pTask.iIndex_taskInfo,"doDecodeVideo");
			if (null==pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();



			//
			TRANSFORM_VIDEO_procInfo pTransformVideo = pTransform.video;
			int index_activeMems_from = -1;// pTransform.index_activeMems_from;
			index_activeMems_from = pTransform.get_index_activeMems_from_decV();

			//
			//pTransform.index_activeMems_from_decV_bak = index_activeMems_from;

			//
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from)
			{
				qyFuncs.showInfo_open(0, null, "doDecodeVideo failed, index_activeMems_from err");
				//
				return -1;
			}
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];



			//  2015/04/23
#if __DEBUG__
//showInfo_open(  0,  0,  _T(  "kk"  )  );
#endif


			//
			uint uiModuleType_toDec = 0;
switch (pActiveMem_from.avStream.obj.tranInfo.video.compressor.ucCompressors)
{
	case Consts.CONST_videoCompressors_stream:
	case Consts.CONST_videoCompressors_hwAccl:
	case Consts.CONST_videoCompressors_ffmpeg:
		uiModuleType_toDec = Consts.CONST_moduleType_mediaSdk_dec;
		break;
	case Consts.CONST_videoCompressors_ipp:
		uiModuleType_toDec = Consts.CONST_moduleType_ipp;
		break;
	case Consts.CONST_videoCompressors_vp8:                            //  2014/07/21
		uiModuleType_toDec = Consts.CONST_moduleType_dec_vp8;
		break;
	default:
		break;
}
			//


			/////////
			//
			//
			adjustModuleType_decV(ref uiModuleType_toDec);







//  2015/01/19

//
#if __DEBUG__
#endif



//

switch (uiModuleType_toDec)
{
				/*
								case CONST_moduleType_mediaSdk_dec:
						return doDecodeVideo_mediaSdk(pTransform, index_pMems_from, pMsgBuf);
						break;
					case CONST_moduleType_dec_nv:   //  2014/06/25
						return doDecodeVideo_nv(pTransform, index_pMems_from, pMsgBuf);
						break;
					case CONST_moduleType_dec_amf:  //  2015/05/06
						return doDecodeVideo_amf(pTransform, index_pMems_from, pMsgBuf);
						break;
					case CONST_moduleType_dec_vp8:
						return doDecodeVideo_vp8(pTransform, index_pMems_from, pMsgBuf);
						break;
				*/
				case Consts.CONST_moduleType_mediaCodec:
					return GuiShare.pf_doDecodeVideo_mediaCodec(pTransform, index_activeMems_from, (IntPtr)pMsgBuf_unused);

	default:
		break;
}

			string str;
			str = string.Format("doDecodeVideo failed, unsupported moduleType {0}", uiModuleType_toDec);
			qyFuncs.showInfo_open(0, null,str);
			if (uiModuleType_toDec != 0)
			{
				qyFuncs.showNotification_open(0, 0, 0, str);
			}

			//
#if __USE_doDecodeVideo_default__
			doDecodeVideo_default(pTransform, index_pMems_from, index_activeMems_from,  pMsgBuf_unused);
#endif

			//
iErr = 0;

errLabel:

//  exit

return iErr;
		}






		//



	}
}
