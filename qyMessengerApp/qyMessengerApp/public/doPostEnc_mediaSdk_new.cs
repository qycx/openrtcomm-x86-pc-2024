using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {
        public static unsafe bool bCapBmpQuit(object pCAP_procInfo_bmpCommon, object p1, object p2)
        {
            CCtxQyMc pQyMc = g.g_pQyMc; ;
            if (null == pQyMc) return false;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //  QY_GET_procInfo_isCli(  );
            if (null == pProcInfo) return false;

            CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)pCAP_procInfo_bmpCommon;
            if (null == pCapProcInfo) return false;

            QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pCapProcInfo.iIndex_sharedObj);
            if (null == pSharedObj) return false;

            if (!bSharedObjActive(pProcInfo, pCapProcInfo.iIndex_sharedObj))
            {
                return true;
            }
            return pCapProcInfo.thread.bQuit;
        }


        public static unsafe bool bSharedObjActive(CCtxQmc pProcInfo, int index)
        {
            QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, index);
            if (null == pSharedObj) return false;

            /*
			if (pSharedObj->bThreadEnd) return false;

			DWORD dwExitCode = 0;

			if (GetExitCodeThread(pSharedObj->hThread, &dwExitCode) && dwExitCode == STILL_ACTIVE) return true;

			if (pSharedObj->hProcess)
			{
				QM_SHM_CMD* pShmCmd = (QM_SHM_CMD*)pProcInfo->getQmShmCmdByIndex(pSharedObj->iIndex_shmCmd);
				if (pShmCmd
					&& pShmCmd->uiTranNo_sharedObj == pSharedObj->uiTranNo)
				{
					if (pShmCmd->ucbProcessEnd) return false;
				}

				if (GetExitCodeProcess(pSharedObj->hProcess, &dwExitCode) && dwExitCode == STILL_ACTIVE) return true;   //  2009/12/23
			}
			*/

            if (pSharedObj.var.bQuit) return false;

            //
            return true;
        }


        //
#if false
		public static unsafe   int old_doPostEnc_mediaSdk_new(ref QOI_common pQDC_OBJ_INFO, byte* pInput, uint inputSize, int iSampleTimeInMs, uint uiPts, void* pBuf_pkt)
		{
			int iErr = -1;

			bool bDbg = false;
			string str;

			CCtxQyMc pQyMc = g.g_pQyMc;
			if (null==pQyMc)
			{
				return -1;
			}
			CCtxQmc  pProcInfo = pQyMc.get_pProcInfo();  //  QY_GET_procInfo_isCli(  );
			if (null==pProcInfo)
			{
				return -1;
			}
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
			//if (!pQmcCfg) return -1;
			//
			ref QOI_common pQdcObjInfo = ref pQDC_OBJ_INFO;
			//if (null==pQdcObjInfo) return -1;
			CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)pQdcObjInfo.cfg.v.pParent_transform;

			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pBuf_pkt;
			bool bMsgGot = false;


			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pCapProcInfo.iIndex_sharedObj);
			if (null==pSharedObj) goto errLabel;

#if false
			uint size = sizeof(pPkt[0]);
			if (qGetMsg((GENERIC_Q*)pQdcObjInfo->cfg.v.pTmpDataQ, pPkt, &size))
			{
				showInfo_open(0, 0, _T("doPostEnc_mediaSdk failed, qGetMsg failed"));
				goto errLabel;
			}
#endif
			int len;

			//  len  =  offsetof(  myDRAW_VIDEO_DATA,  buf  )  +  pCapProcInfo->vh_org.bih.biSizeImage;
			len = sizeof(myDRAW_VIDEO_DATA);
			myDRAW_VIDEO_DATA* p;
			p = pPkt; //  (  myDRAW_VIDEO_DATA  *  )m_var.pBuf;
			if (null == p) goto errLabel;
			qyFuncs.mymemset((IntPtr)p, 0, sizeof(myDRAW_VIDEO_DATA));    //  memset(  p,  0,  m_var.uiBufSize_pBuf  );

			uint uiSize_dec = (uint)0;


			//  2011/12/06
			uint uiSize_enc = inputSize;// (uint)sizeof(TRANSFER_VIDEO_DATA);
			if( inputSize>sizeof(TRANSFER_VIDEO_DATA))
            {
				str = string.Format("doPostEnc_mediaSdk_new failed, inputSize too big");
				showInfo_open(0, null, str);
				goto errLabel;
            }

			//
			p->head.uiLen = uiSize_dec + uiSize_enc;        //  pBmpInfo_input->bmiHeader.biSizeImage;	
			p->head.uiSampleTimeInMs = (uint)iSampleTimeInMs;
			//
			p->uiSize_dec = uiSize_dec;

			//
			if (IntPtr.Zero == mallocMemory(0, (int)(uiSize_dec + uiSize_enc), ref pPkt->memory)) goto errLabel;
			
			//
			bMsgGot = true;
						//  注意：这个初始化非常重要，涉及压缩数据有效性					
			TRANSFER_VIDEO_DATA* pReq = (TRANSFER_VIDEO_DATA*)((byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec);
			mymemset((IntPtr)pReq, 0, (int)System.Runtime.InteropServices.Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));


			//	
			SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
			if (null==pSharedObjUsr)
			{
#if __DEBUG__
				traceLog(_T("mcThread_toShareBmp: pSharedObjUsr is null"));
#endif
				goto errLabel;
			}


			bool bInited_pReq = false;
			int myuiSampleTimeInMs = 0;
			uint myuiPts = 0;  //  2015/01/15
										//int  usIndex_activeMems_from  =  0;


			if (!bInited_pReq)
			{
				if (null!=pReq)
				{
					//  memset(  pReq,  0,  offsetof(  TRANSFER_VIDEO_DATA,  buf  )  );
					pReq->uiType = Consts.CONST_imCommType_transferVideoData;
					//  pReq->uiTranNo_openAvDev  =  pCompressVideo->uiTranNo_openAvDev;
					pReq->uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;         //  2010/09/04
																							//
				}
				//
				bInited_pReq = true;
			}

			if (Consts.bufSize_transferVideoData  <= pReq->uiLen  )  {
				//traceLogA("toShareBmp failed: sizeof(  pReq->buf  ) %d  <=  pReq->len %d", Consts.bufSize_transferVideoData,  pReq->uiLen  );
				goto errLabel;
			}

			//  int  tmpiRet  =  pModule->compress.pf_qdcGetOutputByIndex(  &pCompressVideo->u.custom,  index_output,  (  BYTE  *  )pReq->buf  +  pReq->uiLen,  &pReq->mems[pReq->usCnt].uiLen,  NULL,  NULL  );
			if (Consts.bufSize_transferVideoData -pReq->uiLen < inputSize  )  {
				showInfo_open(0, null, _T("doPostEnc_mediaSdk failed, buf too small"));
				goto errLabel;
			}
			mymemcpy((IntPtr)(pReq->buf + pReq->uiLen), (IntPtr)pInput, inputSize);
			TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
			pReq_mems[pReq->usCnt].uiLen = inputSize;

#if DEBUG
			if (pQmcCfg.debugStatusInfo.ucb__Dump_h264_enc__)
			{
				//if ( pCapProcInfo->iIndex_sharedObj==0)
#if false
				{
					FILE* fp = NULL;
					char* fn = CONST_fn_dump_h264_enc;
					fp = fopen(fn, "ab+");
					if (fp)
					{
						char* pBuffer = pReq->buf + pReq->uiLen;
						int BufferLen = pReq->mems[pReq->usCnt].uiLen;
						//
						fwrite(pBuffer, BufferLen, 1, fp);
						fclose(fp);
					}
					static int uiCnt_fn = 0;
					uiCnt_fn++;
					if (uiCnt_fn == 2000)
					{
						int i;
						i = 1;
					}
				}
#endif
				string fn = Consts.CONST_fn_dump_h264_enc;
				byte* pBuffer = pReq->buf + pReq->uiLen;
				int BufferLen = (int)pReq_mems[pReq->usCnt].uiLen;
				//
				//GuiShare.pf_dumpData1((IntPtr)pBuffer, BufferLen, 0, 0, true, false, fn);

				//
				str = string.Format("dump {0} bytes", BufferLen);
				showInfo_open(0, null, str);


			}
#endif


			//  2014/04/22
#if true
			int  nIntervalInMs  =  40;
							   //
							   ushort  usFps  =  Math.Min(  pCapProcInfo.compressVideo.input.usFps,  pCapProcInfo.compressVideo.compressor.common.head.usMaxFps_toShareBmp  );
							   if  (  0==usFps  ||  Math.Abs(  usFps  -  pCapProcInfo.compressVideo.compressor.common.head.usMaxFps_toShareBmp  )  <  3  )  {
								   usFps  =  pCapProcInfo.compressVideo.compressor.common.head.usMaxFps_toShareBmp;
							   }
							   if  (  0!=usFps  )  {
								   nIntervalInMs  =  1000  /  usFps;
							   }
							   //
							   int  iNextIndex_preEnc  =  pCapProcInfo.compressVideo.encInfo.iNextIndex_preEnc;
							   int  iLastIndex_preEnc  =  0;
							   if  (  0==iNextIndex_preEnc  )  iLastIndex_preEnc  =  Consts.MAX_cnt_preEnc_array  -  1;
							   else  iLastIndex_preEnc  =  iNextIndex_preEnc  -  1;
							   //
							   if  (  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  >=  Consts.MAX_cnt_preEnc_array  )  {
								   qyShowInfo1(  Consts.CONST_qyShowType_qwmComm,  null,  "",  pProcInfo.str_who_showInfo,  null,  _T(  ""  ),  _T(  ""  ),  _T(  "doPostEnc failed: iIndex_postEnc too big"  )  );
								   goto  errLabel;
							   }
							   //
							   int  iDiff_index  =  Consts.MAX_cnt_preEnc_array  +  iNextIndex_preEnc  -  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc;
							   iDiff_index  =  iDiff_index  %  Consts.MAX_cnt_preEnc_array;
							   int  max_iDiff_index  =  10;
							   if  (  iDiff_index  >  max_iDiff_index  )  {
								   pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  =  Consts.MAX_cnt_preEnc_array  +  iNextIndex_preEnc  -  6;
								   pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  =  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  %  Consts.MAX_cnt_preEnc_array;
								   //
								   str=string.Format(  "doPostEnc: iDiff_index %d, adjusted: ind_preEnc %d, ind_postEnc %d"  ,  iDiff_index,  iNextIndex_preEnc,  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  );
									showInfo_open(0, null, str);
							   }
							   //
							   iSampleTimeInMs  =  pCapProcInfo.compressVideo.encInfo.iSampleTimeInMs_preEnc_array[pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc];

							   //
							   if  (  !pCapProcInfo.compressVideo.ucbUseEvt  )  {
								   //  2015/01/15
								   bool  bNeedPts  =  true;
								   //

								   //
								   if  (  bNeedPts  )  {
									   myuiPts  =  pCapProcInfo.compressVideo.encInfo.cnt_postEnc;
								   }
								   }
							   else  {  //  2015/10/20
								     myuiPts  =  uiPts;
							   }

							   //
							   if  (  pQmcCfg.debugStatusInfo.ucbShowEncVStatus  )  {
								   int  uiCapType  =  0;
								   bool  bShow  =  true;

#if __DEBUG__   //  2014/12/01
										   //uiCapType  =  CONST_capType_mosaic;
#endif

			if (0!=uiCapType)
			{
				if (uiCapType != pCapProcInfo.compressVideo.compressor.common.head.uiCapType) bShow = false;
			}

			if (bShow)
			{
#if false
					int st_preEnc = pCapProcInfo->compressVideo.encInfo.iSampleTimeInMs_preEnc_array[iLastIndex_preEnc];
				TCHAR tBuf[256] = _T("");
				_sntprintf(tBuf, mycountof(tBuf), _T("st_po %dms. d_po %dms. st_new %dms, d_new_po %dms. i_new %d, i_po %d. encSize %d. pts %d"),
					iSampleTimeInMs, iSampleTimeInMs - pCapProcInfo->compressVideo.encInfo.iSampleTimeInMs_postEnc,
					st_preEnc, st_preEnc - iSampleTimeInMs,
					iLastIndex_preEnc, pCapProcInfo->compressVideo.encInfo.iNextIndex_postEnc,
					inputSize, myuiPts);
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T("postEnc:"), _T(""), _T("%s"), tBuf);
#endif
			}
		}
		pCapProcInfo.compressVideo.encInfo.iSampleTimeInMs_postEnc  =  iSampleTimeInMs;
							   pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  ++  ;
							   if  (pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  >=  Consts.MAX_cnt_preEnc_array  )  {
								   pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  =  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  %  Consts.MAX_cnt_preEnc_array;
	}
	pCapProcInfo.compressVideo.encInfo.cnt_postEnc  ++  ;
#endif

					//  2008/11/12, 
					if  (0!=iSampleTimeInMs  )  {
				//  pReq->mems[0].uiSampleTimeInMs  =  iSampleTimeInMs;				  
				myuiSampleTimeInMs = (int)iSampleTimeInMs;// pSharedObj.iDiffInMs_tStartAudio;	//  2009/05/26
				pReq_mems[pReq->usCnt].uiSampleTimeInMs = (uint)myuiSampleTimeInMs;
						   //pReq->mems[pReq->usCnt].uiSampleTimeInMs_org  =  iSampleTimeInMs;									//  2009/05/27
					   }
//  2015/01/15
pReq_mems[pReq->usCnt].uiPts = myuiPts;
//
//
pReq->uiLen += pReq_mems[pReq->usCnt].uiLen;
pReq->usCnt++;


//
uint dwTickCnt;
#if false
						dwTickCnt  =  myGetTickCount( null );
							pPkt->debugInfo.usEncedInMs  =  dwTickCnt  -  pPkt.debugInfo.dwTickCnt_start;
						//  2012/01/06
						bool  ucbFpsUnmatched  =  false;
						if  (  pCapProcInfo.compressVideo.usFrames_perSecond_expected  >=  pCapProcInfo.compressVideo.input.usFps  +  5  )  {		//  如果设置29.但是输入是24,那么也需要警告提示,所以是 >=
							ucbFpsUnmatched  =  true;							
						}
						if  (  pSharedObj.status.ucbFpsUnmatched  !=  ucbFpsUnmatched  )  pSharedObj.status.ucbFpsUnmatched  =  ucbFpsUnmatched;
#endif

//
bool bPktRedirected = false;
if (0==q2PostMsgAndTrigger((IntPtr)pPkt, (uint)(1 * sizeof(myDRAW_VIDEO_DATA)), pCapProcInfo.postThread.q2))
{
	bPktRedirected = true;
}
if (bPktRedirected)
{
	detachVPkt(pPkt, null);
}
else
{
	clean_myDRAW_VIDEO_DATA((IntPtr)pPkt, _T(""));
}
bMsgGot = false;


iErr = 0;
errLabel:

if (bMsgGot)
{
	clean_myDRAW_VIDEO_DATA((IntPtr)pPkt, _T(""));
}

return iErr;
 }
#endif


        //
        public static unsafe int doPostEnc_mediaSdk_new(ref QOI_common pQDC_OBJ_INFO, byte[] pInput, uint inputSize, int iSampleTimeInMs, uint uiPts, byte ucbKeyFrame, void* pBuf_pkt)
        {
            int iErr = -1;

            bool bDbg = false;
            string str;

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (null == pQyMc)
            {
                return -1;
            }
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //  QY_GET_procInfo_isCli(  );
            if (null == pProcInfo)
            {
                return -1;
            }
            ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
            //if (!pQmcCfg) return -1;
            //
            ref QOI_common pQdcObjInfo = ref pQDC_OBJ_INFO;
            //if (null==pQdcObjInfo) return -1;
            CAP_procInfo_bmpCommon pCapProcInfo = (CAP_procInfo_bmpCommon)pQdcObjInfo.cfg.v.pParent_transform;

            myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pBuf_pkt;
            bool bMsgGot = false;

            //
            //uint inputSize = (uint)pInput.Length;

            //
#if DEBUG
            if (false)
            {
                //if ( pCapProcInfo->iIndex_sharedObj==0)
#if false
				{
					FILE* fp = NULL;
					char* fn = CONST_fn_dump_h264_enc;
					fp = fopen(fn, "ab+");
					if (fp)
					{
						char* pBuffer = pReq->buf + pReq->uiLen;
						int BufferLen = pReq->mems[pReq->usCnt].uiLen;
						//
						fwrite(pBuffer, BufferLen, 1, fp);
						fclose(fp);
					}
					static int uiCnt_fn = 0;
					uiCnt_fn++;
					if (uiCnt_fn == 2000)
					{
						int i;
						i = 1;
					}
				}
#endif
                string fn = Consts.CONST_fn_dump_h264_enc;
                //byte* pBuffer = pReq->buf + pReq->uiLen;
                //int BufferLen = (int)pReq_mems[pReq->usCnt].uiLen;
                //
                GuiShare.pf_dumpData(pInput, 0, 0, true, false, fn);

                //
                str = string.Format("dump {0} bytes", pInput.Length);
                showInfo_open(0, null, str);

                //
                return 0;

            }
#endif



            //
            QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pCapProcInfo.iIndex_sharedObj);
            if (null == pSharedObj) goto errLabel;

#if false
			uint size = sizeof(pPkt[0]);
			if (qGetMsg((GENERIC_Q*)pQdcObjInfo->cfg.v.pTmpDataQ, pPkt, &size))
			{
				showInfo_open(0, 0, _T("doPostEnc_mediaSdk failed, qGetMsg failed"));
				goto errLabel;
			}
#endif
            int len;

            //  len  =  offsetof(  myDRAW_VIDEO_DATA,  buf  )  +  pCapProcInfo->vh_org.bih.biSizeImage;
            len = sizeof(myDRAW_VIDEO_DATA);
            myDRAW_VIDEO_DATA* p;
            p = pPkt; //  (  myDRAW_VIDEO_DATA  *  )m_var.pBuf;
            if (null == p) goto errLabel;
            qyFuncs.mymemset((IntPtr)p, 0, sizeof(myDRAW_VIDEO_DATA));    //  memset(  p,  0,  m_var.uiBufSize_pBuf  );

            uint uiSize_dec = (uint)0;


            //  2011/12/06
            uint uiSize_enc = (uint)System.Runtime.InteropServices.Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf") + inputSize;// (uint)sizeof(TRANSFER_VIDEO_DATA);
            if (uiSize_enc > sizeof(TRANSFER_VIDEO_DATA))
            {
                str = string.Format("doPostEnc_mediaSdk_new failed, inputSize too big");
                showInfo_open(0, null, str);
                goto errLabel;
            }

            //
            p->head.uiLen = uiSize_dec + uiSize_enc;        //  pBmpInfo_input->bmiHeader.biSizeImage;	
            p->head.uiSampleTimeInMs = (uint)iSampleTimeInMs;
            //
            p->uiSize_dec = uiSize_dec;

            //
            p->usPktResType = Consts.CONST_pktResType_sharedTex;

            //
            if (IntPtr.Zero == mallocMemory(0, (int)(uiSize_dec + uiSize_enc), ref pPkt->memory)) goto errLabel;

            //
            bMsgGot = true;
            //  注意：这个初始化非常重要，涉及压缩数据有效性					
            TRANSFER_VIDEO_DATA* pReq = (TRANSFER_VIDEO_DATA*)((byte*)pPkt->memory.m_pBuf + pPkt->uiSize_dec);
            mymemset((IntPtr)pReq, 0, (int)System.Runtime.InteropServices.Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));


            //	
            SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
            if (null == pSharedObjUsr)
            {
#if __DEBUG__
				traceLog(_T("mcThread_toShareBmp: pSharedObjUsr is null"));
#endif
                goto errLabel;
            }
            if (pSharedObjUsr.uiTranNo_openAvDev_v == 0)
            {
                showInfo_open(0, null, "doPostEnc_mediaSdk_new failed, sharedObjUsr.uiTranNo_openAvDev_v is 0");
                goto errLabel;
            }


            //
            bool bInited_pReq = false;
            int myuiSampleTimeInMs = 0;
            uint myuiPts = 0;  //  2015/01/15
                               //int  usIndex_activeMems_from  =  0;


            if (!bInited_pReq)
            {
                if (null != pReq)
                {
                    //  memset(  pReq,  0,  offsetof(  TRANSFER_VIDEO_DATA,  buf  )  );
                    pReq->uiType = Consts.CONST_imCommType_transferVideoData;
                    //  pReq->uiTranNo_openAvDev  =  pCompressVideo->uiTranNo_openAvDev;
                    pReq->uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;         //  2010/09/04
                                                                                           //
                }
                //
                bInited_pReq = true;
            }

            if (Consts.bufSize_transferVideoData <= pReq->uiLen)
            {
                //traceLogA("toShareBmp failed: sizeof(  pReq->buf  ) %d  <=  pReq->len %d", Consts.bufSize_transferVideoData,  pReq->uiLen  );
                goto errLabel;
            }

            //  int  tmpiRet  =  pModule->compress.pf_qdcGetOutputByIndex(  &pCompressVideo->u.custom,  index_output,  (  BYTE  *  )pReq->buf  +  pReq->uiLen,  &pReq->mems[pReq->usCnt].uiLen,  NULL,  NULL  );
            if (Consts.bufSize_transferVideoData - pReq->uiLen < inputSize)
            {
                showInfo_open(0, null, _T("doPostEnc_mediaSdk failed, buf too small"));
                goto errLabel;
            }
            //
            //mymemcpy((IntPtr)(pReq->buf + pReq->uiLen), (IntPtr)pInput, inputSize);
            System.Runtime.InteropServices.Marshal.Copy(pInput, 0, (IntPtr)(pReq->buf + pReq->uiLen), (int)inputSize);
            //
            TRANSFER_VIDEO_dataMemHead* pReq_mems1 = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
            int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
            //
            TRANSFER_VIDEO_dataMemHead* pMem = getMem(pReq_mems1, bufSize, pReq->usCnt);

            //
            pMem->uiLen = inputSize;

#if DEBUG
            if (pQmcCfg.debugStatusInfo.ucb__Dump_h264_enc__)
            {
                //if ( pCapProcInfo->iIndex_sharedObj==0)
#if false
				{
					FILE* fp = NULL;
					char* fn = CONST_fn_dump_h264_enc;
					fp = fopen(fn, "ab+");
					if (fp)
					{
						char* pBuffer = pReq->buf + pReq->uiLen;
						int BufferLen = pReq->mems[pReq->usCnt].uiLen;
						//
						fwrite(pBuffer, BufferLen, 1, fp);
						fclose(fp);
					}
					static int uiCnt_fn = 0;
					uiCnt_fn++;
					if (uiCnt_fn == 2000)
					{
						int i;
						i = 1;
					}
				}
#endif
                string fn = Consts.CONST_fn_dump_h264_enc;
                byte* pBuffer = pReq->buf + pReq->uiLen;
                int BufferLen = (int)pMem->uiLen;
                //
                GuiShare.pf_dumpData1((IntPtr)pBuffer, BufferLen, 0, 0, true, false, fn);

                //
                str = string.Format("dump {0} bytes", BufferLen);
                showInfo_open(0, null, str);


            }
#endif


            //  2014/04/22
#if true
            int nIntervalInMs = 40;
            //
            ushort usFps = Math.Min(pCapProcInfo.compressVideo.input.usFps, pCapProcInfo.compressVideo.compressor.common.head.usMaxFps_toShareBmp);
            if (0 == usFps || Math.Abs(usFps - pCapProcInfo.compressVideo.compressor.common.head.usMaxFps_toShareBmp) < 3)
            {
                usFps = pCapProcInfo.compressVideo.compressor.common.head.usMaxFps_toShareBmp;
            }
            if (0 != usFps)
            {
                nIntervalInMs = 1000 / usFps;
            }
            //
#if false
			//
			int  iNextIndex_preEnc  =  pCapProcInfo.compressVideo.encInfo.iNextIndex_preEnc;
							   int  iLastIndex_preEnc  =  0;
							   if  (  0==iNextIndex_preEnc  )  iLastIndex_preEnc  =  Consts.MAX_cnt_preEnc_array  -  1;
							   else  iLastIndex_preEnc  =  iNextIndex_preEnc  -  1;
							   //
							   if  (  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  >=  Consts.MAX_cnt_preEnc_array  )  {
								   qyShowInfo1(  Consts.CONST_qyShowType_qwmComm,  null,  "",  pProcInfo.str_who_showInfo,  null,  _T(  ""  ),  _T(  ""  ),  _T(  "doPostEnc failed: iIndex_postEnc too big"  )  );
								   goto  errLabel;
							   }
							   //
							   int  iDiff_index  =  Consts.MAX_cnt_preEnc_array  +  iNextIndex_preEnc  -  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc;
							   iDiff_index  =  iDiff_index  %  Consts.MAX_cnt_preEnc_array;
							   int  max_iDiff_index  =  10;
							   if  (  iDiff_index  >  max_iDiff_index  )  {
								   pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  =  Consts.MAX_cnt_preEnc_array  +  iNextIndex_preEnc  -  6;
								   pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  =  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  %  Consts.MAX_cnt_preEnc_array;
								   //
								   str=string.Format(  "doPostEnc: iDiff_index {0}, adjusted: ind_preEnc {1}, ind_postEnc {2}"  ,  iDiff_index,  iNextIndex_preEnc,  pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc  );
									showInfo_open(0, null, str);
							   }
							   //
							   //iSampleTimeInMs  =  pCapProcInfo.compressVideo.encInfo.iSampleTimeInMs_preEnc_array[pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc];
#endif

            //
            if (!pCapProcInfo.compressVideo.ucbUseEvt)
            {
                //  2015/01/15
                bool bNeedPts = true;
                //

                //
                if (bNeedPts)
                {
                    myuiPts = pCapProcInfo.compressVideo.encInfo.cnt_postEnc;
                }
            }
            else
            {  //  2015/10/20
                myuiPts = uiPts;
            }

            //
            if (pQmcCfg.debugStatusInfo.ucbShowEncVStatus)
            {
                int uiCapType = 0;
                bool bShow = true;

#if __DEBUG__   //  2014/12/01
										   //uiCapType  =  CONST_capType_mosaic;
#endif

                if (0 != uiCapType)
                {
                    if (uiCapType != pCapProcInfo.compressVideo.compressor.common.head.uiCapType) bShow = false;
                }

                if (bShow)
                {
#if false
					int st_preEnc = pCapProcInfo->compressVideo.encInfo.iSampleTimeInMs_preEnc_array[iLastIndex_preEnc];
				TCHAR tBuf[256] = _T("");
				_sntprintf(tBuf, mycountof(tBuf), _T("st_po %dms. d_po %dms. st_new %dms, d_new_po %dms. i_new %d, i_po %d. encSize %d. pts %d"),
					iSampleTimeInMs, iSampleTimeInMs - pCapProcInfo->compressVideo.encInfo.iSampleTimeInMs_postEnc,
					st_preEnc, st_preEnc - iSampleTimeInMs,
					iLastIndex_preEnc, pCapProcInfo->compressVideo.encInfo.iNextIndex_postEnc,
					inputSize, myuiPts);
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T("postEnc:"), _T(""), _T("%s"), tBuf);
#endif
                }
            }
            pCapProcInfo.compressVideo.encInfo.iSampleTimeInMs_postEnc = iSampleTimeInMs;
            pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc++;
            if (pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc >= Consts.MAX_cnt_preEnc_array)
            {
                pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc = pCapProcInfo.compressVideo.encInfo.iNextIndex_postEnc % Consts.MAX_cnt_preEnc_array;
            }
            pCapProcInfo.compressVideo.encInfo.cnt_postEnc++;
#endif

            //  2008/11/12, 
            if (0 != iSampleTimeInMs)
            {
                //  pReq->mems[0].uiSampleTimeInMs  =  iSampleTimeInMs;				  
                myuiSampleTimeInMs = (int)iSampleTimeInMs;// pSharedObj.iDiffInMs_tStartAudio;	//  2009/05/26
                pMem->uiSampleTimeInMs = (uint)myuiSampleTimeInMs;
                //pReq->mems[pReq->usCnt].uiSampleTimeInMs_org  =  iSampleTimeInMs;									//  2009/05/27
            }
            //  2015/01/15
            pMem->uiPts = myuiPts;

            //
            pMem->ucbKeyFrame = ucbKeyFrame;


            //
            pReq->uiLen += pMem->uiLen;
            pReq->usCnt++;


            //
            uint dwTickCnt;
#if false
						dwTickCnt  =  myGetTickCount( null );
							pPkt->debugInfo.usEncedInMs  =  dwTickCnt  -  pPkt.debugInfo.dwTickCnt_start;
						//  2012/01/06
						bool  ucbFpsUnmatched  =  false;
						if  (  pCapProcInfo.compressVideo.usFrames_perSecond_expected  >=  pCapProcInfo.compressVideo.input.usFps  +  5  )  {		//  如果设置29.但是输入是24,那么也需要警告提示,所以是 >=
							ucbFpsUnmatched  =  true;							
						}
						if  (  pSharedObj.status.ucbFpsUnmatched  !=  ucbFpsUnmatched  )  pSharedObj.status.ucbFpsUnmatched  =  ucbFpsUnmatched;
#endif

            //
            bool bPktRedirected = false;
            if (0 == q2PostMsgAndTrigger((IntPtr)pPkt, (uint)(1 * sizeof(myDRAW_VIDEO_DATA)), pCapProcInfo.postThread.q2))
            {
                bPktRedirected = true;
            }
            if (bPktRedirected)
            {
                detachVPkt(pPkt, null);
            }
            else
            {
                clean_myDRAW_VIDEO_DATA((IntPtr)pPkt, _T(""));
            }
            bMsgGot = false;


            iErr = 0;
        errLabel:

            if (bMsgGot)
            {
                clean_myDRAW_VIDEO_DATA((IntPtr)pPkt, _T(""));
            }

            return iErr;
        }


        //
    }
}
