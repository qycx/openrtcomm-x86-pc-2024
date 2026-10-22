using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace qy
{
    unsafe struct TMP_buf_myPLAY_AUDIO_DATA_pkts_encAudio
    {
        public fixed byte buf[Consts.bufSize_myPLAY_AUDIO_DATA_pkts_encAudio];
    };

    partial class qyFuncs
    {


        //
        public static unsafe int doEncodeAudio_default(ref CTX_caller ctxCaller, CCtxQmc pProcInfo, CAP_procInfo_audioCommon pCapProcInfo, MIS_CNT pMisCnt, MIS_MSGU* pMsgBuf)
        {
            int iErr = -1;


            //  MC_VAR_isCli  *  pProcInfo  =  QY_GET_procInfo_isCli(  );
            CCtxQyMc pQyMc = g.g_pQyMc;

            //
            int iIndex_sharedObj = pCapProcInfo.iIndex_sharedObj;
            QY_sharedObj_sync pSharedObjSync = pProcInfo.getSharedObjSyncByIndex(iIndex_sharedObj);
            if (null == pSharedObjSync) return -1;
            QY_SHARED_OBJ pSharedObj = pSharedObjSync.sharedObj;//getSharedObjByIndex(  pProcInfo,  iIndex_sharedObj  );
            if (null == pSharedObj) return -1;
            //
            ref QMC_cfg pCfg = ref pProcInfo.cfg;
            //if (!pCfg) return -1;

#if true

            int old_pktId = 0;
            //
            for (; ; )
            {
                if (bCapAudioQuit(pCapProcInfo, null, null))
                {
#if __DEBUG__
						  traceLog(  _T(  "doEncodeAudio_default failed：quit is true"  )  );
#endif
                    goto errLabel;
                }
                //
                for (; ; )
                {
                    if (bCapAudioQuit(pCapProcInfo, null, null)) goto errLabel;
                    if (0 == getNextAPkt(pCapProcInfo.compressAudio.pCurPkt, pCapProcInfo.compressAudio.pInDataCacheQ, pCapProcInfo.thread.q2)) break;
                    mySleep(40);
                }
                //
#if true
                byte* pInput = null;
                uint uiInputSize = 0;
                bool bNeed_forceKeyFrame = false;   //  2014/09/25
                if (0 != enc_parseOrgAPkt_i(Consts.CONST_uiTransformType_encAudio, pCapProcInfo, pCapProcInfo.compressAudio.pCurPkt, &old_pktId, &pInput, &uiInputSize, &bNeed_forceKeyFrame))
                {
#if __DEBUG__
						  traceLog(  _T(  "doEncodeAudio_default failed, parsePkt failed"  )  );
#endif
                    goto errLabel;
                }
#endif

                myPLAY_AUDIO_DATA* pPkt = pCapProcInfo.compressAudio.pCurPkt;
                int pktLen = sizeof(myPLAY_AUDIO_DATA);

                //
#if DEBUG
                if (pCfg.debugStatusInfo.ucb__Dump_pcm__)
                {
                    pCfg.debugStatusInfo.dump.bufSize = 256 * 1024;
                    if (IntPtr.Zero == pCfg.debugStatusInfo.dump.pBuf)
                    {
                        pCfg.debugStatusInfo.dump.pBuf = mymalloc(pCfg.debugStatusInfo.dump.bufSize);
                    }
                    //
                    if (pPkt->uiSize_dec < pCfg.debugStatusInfo.dump.bufSize - pCfg.debugStatusInfo.dump.len)
                    {
                        mymemcpy(pCfg.debugStatusInfo.dump.pBuf + pCfg.debugStatusInfo.dump.len, pPkt->memory.m_pBuf, pPkt->uiSize_dec);
                        pCfg.debugStatusInfo.dump.len += (int)pPkt->uiSize_dec;
                    }
                    //
                    if (pCfg.debugStatusInfo.dump.len > 200 * 1024)
                    {
                        /*
		FILE* fp = fopen(CONST_fn_dump_pcm, "ab+");
		if (fp)
		{
			fwrite(pBuf, len, 1, fp);
			fclose(fp);
		}
						*/
                        //
                        GuiShare.pf_dumpData1(pCfg.debugStatusInfo.dump.pBuf, pCfg.debugStatusInfo.dump.len, 0, 0, false, false, "android_dump.pcm");
                        //

                        pCfg.debugStatusInfo.dump.len = 0;

                    }
                }
#endif


                //  toShareAudio(  pProcInfo,  (  CAP_procInfo_audioU  *  )pCapProcInfo,  pPkt->head.uiSampleTimeInMs,  (  BYTE  *  )pPkt->memory.m_pBuf,  pPkt->head.uiLen,  &pCapProcInfo.compressAudio,  getSharedObjByIndex(  pProcInfo,  pCapProcInfo.iIndex_sharedObj  ),  null,  pMsgBuf  );
                toShareAudio(ref ctxCaller, pProcInfo, pCapProcInfo, (int)pPkt->head.uiSampleTimeInMs, (byte*)pPkt->memory.m_pBuf, pPkt->uiSize_dec, ref pCapProcInfo.compressAudio, getSharedObjByIndex(pProcInfo, pCapProcInfo.iIndex_sharedObj), pMsgBuf);
                //
                bool bPlayLocal = false;
                switch (pSharedObj.var.uiType)
                {
                    case Consts.CONST_sharedObjType_mediaFile:
                    case Consts.CONST_sharedObjType_mediaDevice:
                        bPlayLocal = true;
                        break;
                    case Consts.CONST_sharedObjType_webcam_av: //  2015/05/18.
                        if (pCfg.ucbPlayLocalAudio_captured)
                        {
                            bPlayLocal = true;
                        }
                        break;
                    default:
                        break;
                }
                //
                bool bPktRedirected = false;
                if (bPlayLocal)
                {
                    //			  	
                    SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
                    if (null == pSharedObjUsr)
                    {
#if __DEBUG__
#endif
                        //goto  errLabel;
                        //continue;
                    }
                    else
                    {
                        //  

                        pProcInfo.playLocalAudioData(pSharedObjUsr.iTaskId, pSharedObjUsr.uiTranNo_openAvDev_a, ref pCapProcInfo.compressAudio.ah_decompress.wfx, pPkt, pktLen, &bPktRedirected, pCapProcInfo.uiType, pSharedObj.var.iIndex_capAudio);
                    }
                }
                //
                if (bPktRedirected)
                {
                    //mallocObj_tmpPkt.detach(  );
                    detachAPkt(pPkt, null);
                }
                else
                {
                    clean_myPLAY_AUDIO_DATA((IntPtr)pPkt, _T(""));
                }



                //
                continue;
            }


#endif

            iErr = 0;
        errLabel:

            return iErr;
        }




        /////////////////////
        public static unsafe int doEncodeAudio(ref CTX_caller ctxCaller, CCtxQmc pProcInfo, CAP_procInfo_audioCommon pCapProcInfo, MIS_CNT pMisCnt, MIS_MSGU* pMsgBuf)
        {
            int iErr = -1;
            //  MC_VAR_isCli	*	pProcInfo		=	QY_GET_procInfo_isCli(  );
            ref COMPRESS_AUDIO pCompressAudio = ref pCapProcInfo.compressAudio;


            //
            /*
            switch (pCompressAudio.uiModuleType_toEnc)
            {
                case CONST_moduleType_qdc_vorbis_enc:   //  2014/07/19
                    return doEncodeAudio_vorbis(pCapProcInfo, pMisCnt, pMsgBuf);
                    break;
                default:
                    break;
            }
            */


            //	
            doEncodeAudio_default(ref ctxCaller, pProcInfo, pCapProcInfo, pMisCnt, pMsgBuf);

            iErr = 0;
        errLabel:
            return iErr;

        }




        //
        public static unsafe void mcThreadProc_toShareAudio(object lpParameter)
        {
            int iIndex_sharedObj = (int)lpParameter;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return;

            QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
            if (null == pSharedObj) return;

            CAP_procInfo_audioCommon pCapProcInfo = (CAP_procInfo_audioCommon)getCapAudioBySth(pProcInfo, pSharedObj.var.iIndex_capAudio, 0);
            if (null == pCapProcInfo) return;

            //  2009/12/24

            //
            uint dwThreadId = myGetCurrentThreadId();

            //
            CTX_MC_thread ctx = new CTX_MC_thread();
            ctx.ctxCaller.type = Consts.CONST_ctxCallerType_thread;
            ctx.ctxCaller.dwThreadId = myGetCurrentThreadId();
            fixed (char* pName = ctx.ctxCaller.tNameBuf.tNameBuf)
            {
                mytcsncpy(pName, "thread_toShareA", Consts.cntof_ctxCaller_tNameBuf);
            }

            //
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
            if (null == pMisCnt) return;

            using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
            {
                MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                if (null == pMsgBuf) return;

                uint dwTickCnt;

                //  2010/09/18
                //  TMP_MEMORY_GRP					tmpMemoryGrp;
                //  bool							bInited_tmpMemoryGrp		=	false;
                int tmpiRet;

                //	 
                //traceLogA("mcThreadProc_toShareAudio %S enters", pCapProcInfo.thread.q2.cfg.name);

                pCapProcInfo.thread.bQuit = false;
                pCapProcInfo.thread.bRunning = true;

                //  qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "%s, %I64u starts"  ),  pPlay->q2.cfg.name,  pPlayer->idInfo_recorder.ui64Id  );

                /////////////////

                for (; !pCapProcInfo.thread.bQuit;)
                {

                    /*
					uint dwEvt = MsgWaitForMultipleObjects(
									  pCapProcInfo.thread.q2.cfg.usCnt_hEvents,    //  mycountof(  pPlay->hEvents  ),				// Number of events.
									   pCapProcInfo.thread.q2.hEvents,                         // Location of handles.
									   false,                                       // Wait for all?
									   5000,                                    // How long to wait?
									   0);                                      // Any message is an 
																				// event.
					if (dwEvt == WAIT_FAILED) goto errLabel;
					*/
                    myWaitOne(pCapProcInfo.thread.q2.hEvent0, 5000);
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
								//  traceLogA(  "mcThreadProcInfo_toShareAudio: event 0."  );
								break;
							case 1:
								traceLogA("mcThreadProcInfo_toShareAudio: event 1.");
								break;
							default:
								traceLogA("unknown event");
								goto errLabel;
								break;
						}
					}
					*/

                    //  2015/10/11
                    if (!pCapProcInfo.compressAudio.bCompressInited)
                    {
#if __DEBUG__
						//traceLog(  _T(  "thread_toShareAudio: bCompressInited false, continued"  )  );
#endif
                        continue;
                    }


                    //
                    for (; !pCapProcInfo.thread.bQuit;)
                    {


                        //
                        if (0 != getNextAPkt(pCapProcInfo.compressAudio.pCurPkt, pCapProcInfo.compressAudio.pInDataCacheQ, pCapProcInfo.thread.q2)) break;

                        //
                        if (0 == pCapProcInfo.compressAudio.pCurPkt->lPktId_getNewPkt)
                        {
                            //MACRO_qyAssert(0, _T("mcThreadProc_toShareAudio: id_pkt is 0"));
                            traceLog("mcThreadProc_toShareAudio err: id_pkt is 0");
                        }
                        //
                        long old_pktId = pCapProcInfo.compressAudio.pCurPkt->lPktId_getNewPkt;
                        doEncodeAudio(ref ctx.ctxCaller, pProcInfo, pCapProcInfo, pMisCnt, pMsgBuf);
                        if (pCapProcInfo.compressAudio.pCurPkt->lPktId_getNewPkt == old_pktId)
                        {   //  如果没有处理掉就需要释放
                            clean_myPLAY_AUDIO_DATA((IntPtr)pCapProcInfo.compressAudio.pCurPkt, _T(""));
                        }

                        continue;
                    }


                    continue;
                }
            }

            traceLogA("mcThreadProc_toShareAudio will quit");

        errLabel:

            pCapProcInfo.thread.bRunning = false;


            //traceLogA("mcThreadProc_toShareAudio %S leaves", pCapProcInfo.thread.q2.cfg.name);

            return;

        }

#if true  //  2018/10/24
        public static unsafe int toShareAudio(ref CTX_caller ctxCaller, CCtxQmc pProcInfo1, CAP_procInfo_audioCommon pRecordSoundProcInfo1, int iSampleTimeInMs, byte* pInput, uint dwBytesRecorded, ref COMPRESS_AUDIO pCompressAudio, QY_SHARED_OBJ pSharedObj, MIS_MSGU* pMsgBuf)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;//QY_GET_GBUF(  );
                                       //
            uint dwBytes_input = dwBytesRecorded;
            Int64 tStartTran; uint uiTranNo;
            MACRO_prepareForTran(out tStartTran, out uiTranNo);
            byte ucFlg = Consts.CONST_commFlg_routeTalkData;    //  0;
            int lenInBytes = 0;
            //
            uint uiChannelType = Consts.CONST_channelType_realTimeMedia;
            bool ucbNeedShowFrameInfo = false;
            int i;
            //  2010/09/04
            SHARED_OBJ_USR pSharedObjUsr = null;



            //
            if (null == pProcInfo1) return -1;
            CUS_MODULES pCusModules = pProcInfo1.getCusModules();
            ref QMC_cfg pCfg = ref pProcInfo1.cfg;
            //if  (  !pCfg  )  return  -1;

            if (null == pSharedObj) goto errLabel;
            pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
            if (null == pSharedObjUsr)
            {
                traceLog("toShareAudio failed, pSharedObjUsr is null");
                goto errLabel;
            }


            if (pCompressAudio.tLastTime_sendAudio != tStartTran)
            {       //  表明是一秒的开始
                pCompressAudio.tLastTime_sendAudio = tStartTran;

                //  定期向客户端下发帧速信息		
                if ((int)(tStartTran - pCompressAudio.tLastTime_showFrameInfo) > pCfg.usNeedShowFrameInfoIntervalInS)
                {
                    //
                    ucbNeedShowFrameInfo = true;
                }
#if __DEBUG__ //  for test             
				 //  ucbNeedShowFrameInfo  =  true;						
#endif

                //  2008/05/13, 不用调整了，用固定设置值
                /*
                uint  uiBytesRecorded_perBlockAlign;	uiBytesRecorded_perBlockAlign  =  pProcInfo1.cfg.policy.audio.uiBytesRecorded_perBlockAlign;
                if  (  uiBytesRecorded_perBlockAlign  >=  MIN_uiBytesRecorded_perBlockAlign  &&  uiBytesRecorded_perBlockAlign  <=  MAX_uiBytesRecorded_perBlockAlign  )  {
                    pCompressAudio.uiBytesRecorded_perBlockAlign_expected  =  uiBytesRecorded_perBlockAlign;
                }
                */
            }

            //
            QY_Q2 p_realTimeMediaQ2 = pProcInfo1.get_realTimeMediaQ2();
            if (null == p_realTimeMediaQ2) goto errLabel;
            bool bQWarning = isQ2Warning(p_realTimeMediaQ2, pCfg.envVar.usQNodes_warningInterval_rtMediaQ);
            if (bQWarning)
            {
                uint step = 0;
                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, "", pProcInfo1.str_who_showInfo, null, _T(""), _T(""), _T("Warning:  realTimeMediaQ too many nodes. audio data will be lossed"));
            }

            //
#if false
			if (!pProcInfo1.bRealtimeMediaChannelReady())
{
	//static uint sdwTickCnt_applyForChkChannels = 0;
	//  QY_SHARED_OBJ  *  pSharedObj  =  getSharedObjByIndex(  pProcInfo,  pRecordSoundProcInfo->iIndex_sharedObj  );

	//
	if (null!=pSharedObj
		&& !pSharedObjUsr.ucbNotSendData
		   && (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id
				 || 0!=pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id))
	{
		MIS_CNT pMisCnt = null;
		MIS_MSG_applyForChkChannels_qmc msg_applyForChkChannels;
		int dwTickCnt; dwTickCnt = (int)myGetTickCount();
		//int i;
		//CQyQ2Help q2Help;

		if (dwTickCnt - pProcInfo1.sdwTickCnt_applyForChkChannels > 4000)
		{
			//for  (  i  =  0;  i  <  mycountof(  pProcInfo1.misCnts  );  i  ++  )  
			//for (i = 0; i < pProcInfo1.get_ucCnt_misCnts(); i++)
			{
				pMisCnt = pProcInfo1.pMisCnt;//&pProcInfo1.misCnts[i];				

				traceLogA("toShareAudio: applyForChkChannels");
				qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, "", pProcInfo1.str_who_showInfo, null, _T(""), _T(""), _T("toShareAudio: rtMedia Channel not Ready, applyForChkChannels"));

				//
				mymemset((IntPtr)(byte*)&msg_applyForChkChannels, 0, sizeof(MIS_MSG_applyForChkChannels_qmc));
				msg_applyForChkChannels.uiType = Consts.CONST_misMsgType_applyForChkChannels_qmc;
				//msg_applyForChkChannels.pMisCnt = pMisCnt;
				//
				//  qPostMsgAndTrigger(  &msg_applyForChkChannels,  sizeof(  msg_applyForChkChannels  ),  &pProcInfo1.realTimeMediaQ  );
				q2PostMsgAndTrigger((IntPtr)(byte*)&msg_applyForChkChannels, (uint)sizeof(MIS_MSG_applyForChkChannels_qmc), p_realTimeMediaQ2);
			}
			pProcInfo1.sdwTickCnt_applyForChkChannels = (uint)dwTickCnt;
		}
	}

	iErr = 0; goto errLabel;
}
#endif


            //
            TRANSFER_AUDIO_DATA* pReq = (TRANSFER_AUDIO_DATA*)pCompressAudio.pReqCache;
            if (null != pReq && !bQWarning)
            {
                mymemset((IntPtr)pReq, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));
                pReq->uiType = Consts.CONST_imCommType_transferAudioData;
                pReq->uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_a; //  pCompressAudio.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;					

                //
                TRANSFER_AUDIO_dataMemHead* pReq_mems1 = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
                int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;


#if __DEBUG__
	if (dwBytes_input > sizeof(pReq->buf)  )  {
		qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo1.who_showInfo, 0, _T(""), _T(""), _T("sizeof(  audioBuf  ) %d too small, should be >= %d"), sizeof(pReq->buf),  dwBytesRecorded  );
	}
#endif


                if (pCompressAudio.bCompressInited)
                {
                    if (pCompressAudio.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_acm)
                    {
                        traceLog("not supported");
                        goto errLabel;

                    }
                    else if (pCompressAudio.compressor.common.head.ucCompressors == Consts.CONST_audioCompressors_dmo)
                    {
                        traceLog("not supported");
                        goto errLabel;

                    }
                    else
                    {
                        CUS_MODULE_compress pModule = null;
                        uint uiModuleType = 0;

                        uiModuleType = Consts.CONST_moduleType_ffmpeg;

                        /*
						switch (pCompressAudio.compressor.common.ucCompressors)
						{
							case CONST_audioCompressors_ipp:
								pModule = pQyMc->cusModules.pIpp;
								break;
							case CONST_audioCompressors_custom:
								pModule = getCusModule(&pQyMc->cusModules, CONST_moduleType_compress);
								break;
							default:

								traceLogA("toShareAudio failed: unprocessed ucCompressors %d", pCompressAudio.compressor.common.ucCompressors);
								qyShowInfo1(CONST_qyShowType_qwmComm, null, (""), _T("IsClient"), 0, _T(""), _T(""), _T("toShareAudio failed: unprocessed ucCompressors %d"), pCompressAudio.compressor.common.ucCompressors);
								goto errLabel;
								break;
						}
						*/
                        M_getCompressModule(pCusModules, uiModuleType, ref pModule);

                        //
                        if (null == pModule
            || null == pModule.pf_qdcCompressAudio
             || null == pModule.pf_qdcGetOutputByIndex) goto errLabel;

                        uint dwDstBytes = pCompressAudio.u_qoi.myQoi.common.var.cbOutputBuffer;
                        if (null == pCompressAudio.pDstBuf || dwDstBytes >= pCompressAudio.dwDstBufSize)
                        {
                            if (null != pCompressAudio.pDstBuf)
                            {
                                myfree((IntPtr)pCompressAudio.pDstBuf); pCompressAudio.pDstBuf = null;
                            }
                            pCompressAudio.dwDstBufSize = dwDstBytes * 3 / 2 + 256;
                            pCompressAudio.pDstBuf = (byte*)mymalloc((int)pCompressAudio.dwDstBufSize);
                            if (null == pCompressAudio.pDstBuf) goto errLabel;
                        }

                        dwDstBytes = pCompressAudio.dwDstBufSize;
                        if (pModule.pf_qdcCompressAudio(ref pCompressAudio.u_qoi.myQoi.common, ref pCompressAudio.ah_decompress, (IntPtr)pInput, dwBytes_input, iSampleTimeInMs) < 0) goto errLabel;

                        if (null != pSharedObj && pSharedObj.var.iDiffInMs_tStartAudio != pCompressAudio.u_qoi.myQoi.common.var.iDiffInMs_tStartAudio) pSharedObj.var.iDiffInMs_tStartAudio = pCompressAudio.u_qoi.myQoi.common.var.iDiffInMs_tStartAudio;

                        for (i = 0; i < Consts.CONST_maxTransferAvDataMems; i++)
                        {
                            //  2009/05/20
                            if (Consts.bufSize_transferAudioData <= pReq->uiLen)
                            {

                                //traceLogA("toShareAudio: sizeof(  pReq->buf  ) %d  <=  pReq->len %d", sizeof(pReq->buf),  pReq->uiLen  );
                                goto errLabel;
                            }
                            TRANSFER_AUDIO_dataMemHead* pMem = getMem(pReq_mems1, bufSize, i);

                            pMem->uiLen = Consts.bufSize_transferAudioData - pReq->uiLen;
                            if (0 != pModule.pf_qdcGetOutputByIndex(ref pCompressAudio.u_qoi.myQoi.common, i, (IntPtr)((byte*)pReq->buf + pReq->uiLen), ref pMem->uiLen, ref pMem->uiSampleTimeInMs, ref pMem->ui_rtTimeLen)) break;
                            //
                            if (pCfg.debugStatusInfo.ucb__Dump_mp3_enc__)
                            {
                                GuiShare.pf_dumpAudio((IntPtr)(pReq->buf + pReq->uiLen), (int)pMem->uiLen);
                            }
                            //
                            pReq->uiLen += pMem->uiLen;
                            pReq->usCnt++;
                        }


                    }

                }

                //  2010/09/12				  
                if (0 == pReq->usCnt)
                {
                    traceLog(_T("toShareAudio failed: req.usCnt is 0"));
                    goto errLabel;
                }
                //  2015/09/10
                for (i = 0; i < pReq->usCnt; i++)
                {
                    uint myuiPts = 0;  //  2015/01/15
                                       //								
                    bool bNeedPts = true;
                    if (bNeedPts)
                    {
                        myuiPts = pCompressAudio.encInfo.cnt_postEnc;
                    }
                    //				
                    pCompressAudio.encInfo.cnt_postEnc++;
                    //
                    //
                    TRANSFER_AUDIO_dataMemHead* pMem = getMem(pReq_mems1, bufSize, i);
                    pMem->uiPts = myuiPts;
                }

                //  2010/09/12
                pCompressAudio.usCnt_pkts++;
                if (ucbNeedShowFrameInfo)
                {
                    ucFlg |= Consts.CONST_commFlg_routeTalkData;
                    if (pCompressAudio.tRecvTime_resp_fromCli >= pCompressAudio.tFirstTime_showFrameInfo_withoutResp)
                    {   //  这是为了超时
                        pCompressAudio.tFirstTime_showFrameInfo_withoutResp = tStartTran;
                    }
                    pReq->tLastTime_showFrameInfo_020534 = pCompressAudio.tLastTime_showFrameInfo;                             //  设置标记						
                    pReq->usCnt_pkts = pCompressAudio.usCnt_pkts;
                    //
                    pCompressAudio.tLastTime_showFrameInfo = tStartTran;
                    pCompressAudio.usCnt_pkts = 0;
                    //
                    //  qyShowInfo1(  CONST_qyShowType_debug,  0,  "",  pProcInfo1.who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "tn %d, %d bytes audio data. %s"  ),  uiTranNo,  dwBytesRecorded,  ucbNeedShowFrameInfo  ?  _T(  "含音频取样信息"  )  :  _T(  ""  )  );					
                }


#if __DEBUG__
if (pCfg->debugStatusInfo.ucbPrint_a_out)
{
	traceLogA("toShareAudio: tn %d, req.usCnt %d, len %d, mems[0].uiSampleTimeInMs %d, iSampleTimeInMs %d", uiTranNo, pReq->usCnt, pReq->uiLen, pReq->mems[0].uiSampleTimeInMs, iSampleTimeInMs);
	//  qyShowInfo1(  CONST_qyShowType_debug,  (  ""  ),  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "will send tn [%d]"  ),  uiTranNo  );
}
#endif

                //  
                lenInBytes = (int)((uint)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf") + pReq->uiLen);
                //			
                //bool					bHaveMsgrs_sendLocalAv		=	false;
                //  QY_SHARED_OBJ		*	pSharedObj					=	null;	
                MIS_CNT pMisCnt = null;

                //  pSharedObj  =  getSharedObjByIndex(  pProcInfo,  pRecordSoundProcInfo->iIndex_sharedObj  );
                if (null != pSharedObj)
                {

                    pMisCnt = pProcInfo1.pMisCnt;// (MIS_CNT*)pProcInfo1.getMisCntByIndex(pSharedObj.curRoute_sendLocalAv.iIndex_misCnt);
                                                 //if (pMisCnt && pMisCnt->cfg.dynCfg.cnt.usCntPort)
                    {

                        //  这里的将配置换成当前的方式不好，应该将curRoute_sendLocalAv放在m_var里。这样，就用同步可以确保数据一致性了.暂时不改

                        //							   
#if __DEBUG__
//  test为了测试
//  if  (  (  uiTranNo  %  30  )  )  break;
#endif

                        //if (!pSharedObjUsr.ucbNotSendData)
                        {
#if false

							pSharedObj.curRoute_sendLocalAv.route.debugInfo.dwTickCnt_me_start = myGetTickCount();
	if (ucbNeedShowFrameInfo)
	{   //  2008/05/16									 
		Int64 tNow; mytime(&tNow); pSharedObj.curRoute_sendLocalAv.route.debugInfo.tTime_start = tNow;
	}

	if (0!=pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id)
	{   //  
								fixed (MSG_ROUTE* pRoute = &pSharedObj.curRoute_sendLocalAv.route)
								{
									QY_MESSENGER_ID idInfo_to = pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to;
									if (0!=postMsg2MgrFunc_mc(pMisCnt, pRoute, Consts.CONST_misMsgType_outputTask_toMix, ucFlg, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pSharedObjUsr.iTaskId, 0, (byte*)pReq, (uint)lenInBytes, &idInfo_to, null, null, null, uiChannelType, pMsgBuf, false))
									{
										traceLogA("CQyRecordSound::OnSoundData: postMsg2MgrFunc_mc to videoConference failed");     //  
									}
								}
	}
	else
	{
		if (0!=pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id)
		{
									fixed (MSG_ROUTE* pRoute = &pSharedObj.curRoute_sendLocalAv.route)
									{
										if (0!=postMsg2MgrFunc_mc(pMisCnt, pRoute, Consts.CONST_misMsgType_outputTask_toMix, ucFlg, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pSharedObjUsr.iTaskId, 0, (byte*)pReq, (uint)lenInBytes, null, null, null, null, uiChannelType, pMsgBuf, false))
										{
											traceLogA("CQyRecordSound::OnSoundData: postMsg2MgrFunc_mc failed");        //  
										}
									}
		}
	}
#endif

                            //

                            int tmpiRet;
                            //
                            int msgLen = 0;

                            //
                            MSG_ROUTE msgRoute = new MSG_ROUTE();
                            if (0 != makeMsg2MgrFunc_mc(pMisCnt, ref msgRoute, Consts.CONST_misMsgType_task, ucFlg, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pSharedObjUsr.iTaskId, 0, (byte*)pReq, (uint)lenInBytes, 0, 0, 0, 0, uiChannelType, pMsgBuf, &msgLen))
                            {
                                traceLog(_T("makeMsg2MgrFunc_mc failed"));
                            }

                            if (msgLen > 0)
                            {

                                //
                                pMsgBuf->task.bFrom_sharedObj = true;
                                pMsgBuf->task.index_sharedObj = pSharedObj.var.index_sharedObj;

                                //
                                PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                                //
                                if ((tmpiRet = pProcInfo1.processQ_media.qTraverse(ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *pMsgBuf, param)) <= 0)
                                {
#if __DEBUG__
									//traceLog(  _T(  "toShareAudio: postMsg2MgrFunc_mc: msg_task_toMix %s"  ),  tmpiRet  ?  _T(  "failed"  )  :  _T(  "not found"  )  );
#endif
                                }

                            }

                            /////////////////
                        }


#if __DEBUG__
if (pSharedObjUsr.ucbNotSendData)
{
	//  traceLogA(  "toShareAudio: not send data"  );
}
#endif

                        //
                        //if  (  pSharedObj.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id  ||  pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id  )  bHaveMsgrs_sendLocalAv  =  true;

                    }
                }

                //
                //
                //

                //if  (  pbHaveMsgrs_sendLocalAv  )  *pbHaveMsgrs_sendLocalAv  =  bHaveMsgrs_sendLocalAv;

            }

            iErr = 0;
        errLabel:
            return iErr;
        }
#endif


    }
}
