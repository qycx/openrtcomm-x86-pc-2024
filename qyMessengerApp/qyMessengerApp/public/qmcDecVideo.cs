using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

    partial class qyFuncs
    {

        public static unsafe void mcThreadProc_decVideo(object lpParameter)
        {

            QY_TRANSFORM pTransform = (QY_TRANSFORM)lpParameter;
            if (null == pTransform) return;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
            if (null == pProcInfo) return;
            qyFuncs.traceLog("decVideo  pProcInfo");
            //  
            TRANSFORM_VIDEO_procInfo pTransformVideo = pTransform.video;

            //  DYN_LIB_DX						*	pDynLib						=	(  DYN_LIB_DX  *  )(  (  QY_DYN_LIBS  *  )pQyMc->env.pDynLibs  )->pLib_dx;
            //  int								i;


            //  2009/12/24

            //
            uint dwThreadId = myGetCurrentThreadId();
            qyFuncs.traceLog("decVideo  dwThreadId");

            int i;

            //  
            //myDRAW_VIDEO_DATA pkts[CONST_pkts_decVideo];
#if false
			TMP_buf_myDRAW_VIDEO_DATA_pkts_decVideo buf_pkts;
			uint[] uiLens = new uint[Consts.CONST_pkts_decVideo];      //  
			int nLens = Consts.CONST_pkts_decVideo;// mycountof(uiLens);      //  一次最多处理的包数。

			int nPkts;
#endif

            //
            string str1;
            string where_showInfo = "transV";

            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                ;
                MIS_MSGU* pMsg = null;
                qyFuncs.traceLog("decVideo  mallocObj");

                using (CQyMalloc mallocObj_pkt_dec_pBuf = new CQyMalloc())
                {
                    ;
                    myDRAW_VIDEO_DATA pkt_dec;


                    //
                    //using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
                    {
                        ;
                        //MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(sizeof(MIS_MSGU));
                        //if (null == pMsgBuf) return;


                        uint dwTickCnt;

                        bool bPktsRedirected;

                        //CQyCoInit coInit;       //  2010/06/07

                        //
                        str1 = ("mcThreadProc_decVideo enters");
                        showInfo_open(0, where_showInfo, str1);



                        //
                        M_setStep(ref pTransformVideo.debugStep, Consts.CONST_threadStep_start);
                        qyFuncs.traceLog("decVideo  M_setStep");

                        //
                        pTransformVideo.dwTickCnt_start = myGetTickCount(null);
                        pTransformVideo.bRunning = true;
                        //  memset(  &pTransformVideo.m_var,  0,  sizeof(  pTransformVideo.m_var  )  );


                        //  
                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("%s %d starts"), pTransformVideo.q2.cfg.name, 0);

                        if (null == (pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;
                        //
                        //  2015/02/14
                        mymemset((IntPtr)(byte*)&pkt_dec, 0, sizeof(myDRAW_VIDEO_DATA));
                        //
                        uint uiBufSize_pkt_dec = pProcInfo.cfg.uiBufSize_myDrawVideoData;  //  offsetof(  myPLAY_AUDIO_DATA,  buf  )  +  pProcInfo.cfg.uiBufSize_myPlayAudioData;	//  
                        int iAllocType = 0; //  CONST_iAllocType_audio
                                            //  if  (  !(  pTransform.audio.pPkt_dec  =  (  myPLAY_AUDIO_DATA  *  )mallocObj_pPkt_dec.mallocf(  iAllocType,  uiBufSize_pPkt_dec  )  )  )  goto  errLabel;
                                            //
                        if (null == mallocObj_pkt_dec_pBuf.mallocf(iAllocType, (int)uiBufSize_pkt_dec)) goto errLabel;
                        if (0 != mallocObj_pkt_dec_pBuf.get(ref pkt_dec.memory)) goto errLabel;
                        qyFuncs.traceLog("decVideo  mallocObj_pkt_dec_pBuf");


#if __DEBUG__
#endif

                        //
                        //  2014/03/23	 	
                        GENERIC_Q_cfgEx qCfgEx;    //  2012/11/10
                        GENERIC_Q_CFG tmpCfg;
                        string str;

                        //TCHAR tBuf[128];
                        //
                        int tn = (int)myGetTickCount(null);
                        int iTaskId = pTransform.iTaskId;
                        int index_activeMems_from = pTransform.index_activeMems_from;
                        //
                        int size;
                        //sizeof = sizeof(GENERIC_Q);
                        pTransformVideo.pInDataCacheQ = new GENERIC_Q();// (GENERIC_Q*)mymalloc(size);
                        if (null == pTransformVideo.pInDataCacheQ) goto errLabel;
                        //memset(pTransformVideo.pInDataCacheQ, 0, size);
                        //size = sizeof(GENERIC_Q);
                        pTransformVideo.pTmpDataQ = new GENERIC_Q();// (GENERIC_Q*)mymalloc(size);
                        if (null == pTransformVideo.pTmpDataQ) goto errLabel;
                        //memset(pTransformVideo.pTmpDataQ, 0, size);

                        //
                        mymemset((IntPtr)(byte*)&tmpCfg, 0, sizeof(GENERIC_Q_CFG));
                        //_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d-%d-%d"), CONST_mutexNamePrefix_decV_inDataCacheQ, iTaskId, index_activeMems_from, tn);
                        str = string.Format("{0}-{1}-{2}-{3}", Consts.CONST_mutexNamePrefix_decV_inDataCacheQ, iTaskId, index_activeMems_from, tn);
                        mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
                        //_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d-%d-%d"), CONST_mutexNamePrefix_decV_inDataCacheQ, iTaskId, index_activeMems_from, tn);
                        str = string.Format("%s-%d-%d-%d", Consts.CONST_mutexNamePrefix_decV_inDataCacheQ, iTaskId, index_activeMems_from, tn);
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

                        if (0 != initGenericQ(qCfgEx, pTransformVideo.pInDataCacheQ)) goto errLabel;
                        qyFuncs.traceLog("decVideo initGenericQ");

                        //
                        mymemset((IntPtr)(byte*)&tmpCfg, 0, sizeof(GENERIC_Q_CFG));
                        //_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-%d-%d-%d"), CONST_mutexNamePrefix_decV_tmpDataQ, iTaskId, index_activeMems_from, tn);
                        str = string.Format("{0}-{1}-{2}-{3}", Consts.CONST_mutexNamePrefix_decV_tmpDataQ, iTaskId, index_activeMems_from, tn);
                        mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
                        //_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-%d-%d-%d"), CONST_mutexNamePrefix_decV_tmpDataQ, iTaskId, index_activeMems_from, tn);
                        str = string.Format("{0}-{1}-{2}-{3}", Consts.CONST_mutexNamePrefix_decV_tmpDataQ, iTaskId, index_activeMems_from, tn);
                        mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                        tmpCfg.uiMaxQNodes = 100;
                        qyFuncs.traceLog("decVideo uiMaxQNodes");

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

                        if (0 != initGenericQ(qCfgEx, pTransformVideo.pTmpDataQ)) goto errLabel;
                        qyFuncs.traceLog("decVideo initGenericQ");

                        //
#if false
						size = sizeof(myDRAW_VIDEO_DATA);
						pTransformVideo.pCurPkt = (myDRAW_VIDEO_DATA*)mymalloc(size);
						if (null == pTransformVideo.pCurPkt) goto errLabel;
						mymemset((IntPtr)pTransformVideo.pCurPkt, 0, size);
#endif

                        //
                        fixed (PKT_info_toDec* pcurPktInfo_toDec = &pTransformVideo.curPktInfo_toDec)
                        {
                            mymemset((IntPtr)(byte*)pcurPktInfo_toDec, 0, sizeof(PKT_info_toDec));
                        }
                        pTransformVideo.curPktInfo_toDec.pMIS_MSGU = pMsg;
                        pTransformVideo.curPktInfo_toDec.pPkt_dec = &pkt_dec;
                        pMsg->uiType = 0;
                        pTransformVideo.curPktInfo_toDec.pkt_encV.bufSize = Consts.bufSize_tmpPktEncV;
                        qyFuncs.traceLog("decVideo pkt_encV");




                        /////////////////
                        for (; !pTransformVideo.bQuit;)
                        {

                            //  2010/12/28
                            M_setStep(ref pTransformVideo.debugStep, Consts.CONST_threadStep_toWait);
                            qyFuncs.traceLog("pTransformVideo pkt_encV");

                            //
                            int toInMs = 5000;
                            toInMs = 1000;// 33;
                            if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                            {
                                myWaitOne(pTransformVideo.q2.hEvent0, toInMs);
                            }
                            else
                            {
                                myWaitOne(pTransform.v_preTransThread.qBuf2.hEvent0, toInMs);
                            }

                            //
                            if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                            {
                                if (isQ2Empty(pTransform.v_preTransThread.q2)
                                    && isQ2Empty(pTransform.video.q2))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (pTransform.v_preTransThread.qBuf2.isQ2Empty())
                                {
                                    continue;
                                }

                            }

                            //
                            //doDecodeVideo(pTransform, pMsgBuf);
                            doDecodeVideo(pTransform, null);

                            //
                            clean_PKT_info_toDec(ref pTransform.video.curPktInfo_toDec, _T(""));
                            qyFuncs.traceLog("pTransformVideo clean_PKT_info_toDec");


                            //
                            continue;
                        }
                    }
                }
            }

            traceLogA("mcThreadProc_decVideo will quit");

        errLabel:

            //
            //clean_myDRAW_VIDEO_DATA((IntPtr)pTransformVideo.pCurPkt1, _T(""));

            //
            M_setStep(ref pTransformVideo.debugStep, Consts.CONST_threadStep_toExit);

            //  2009/05/28
            exitTaskAvFrom_video(pProcInfo, pTransform, "mcThreadProc_decVideo quit");

            //  2014/03/25
            //clean_myDRAW_VIDEO_DATA((IntPtr)pTransformVideo.pCurPkt1, _T("decVideo"));
            //
            /*
            byte* pCurPkt = (byte*)pTransformVideo.pCurPkt1;
            MACRO_mysafeFree(ref pCurPkt);
            pTransformVideo.pCurPkt1 = (myDRAW_VIDEO_DATA*)pCurPkt;
            */

            //  2014/03/24
            if (null != pTransformVideo.pInDataCacheQ)
            {
                genericQFree(pTransformVideo.pInDataCacheQ); pTransformVideo.pInDataCacheQ = null;
            }
            if (null != pTransformVideo.pTmpDataQ)
            {
                genericQFree(pTransformVideo.pTmpDataQ); pTransformVideo.pTmpDataQ = null;
            }

            //
            fixed (char* pName = pTransformVideo.q2.cfg.name)
            {
                str1 = string.Format("mcThreadProc_decVideo: {0} ends.", new string(pName));
                showInfo_open(0, null, str1);

            }



            pTransformVideo.bRunning = false;


            //
            M_setStep(ref pTransformVideo.debugStep, Consts.CONST_threadStep_end);

            //traceLogA("mcThreadProc_decVideo %S leaves", pTransformVideo.q2.cfg.name);


            return;

        }

    }

}
