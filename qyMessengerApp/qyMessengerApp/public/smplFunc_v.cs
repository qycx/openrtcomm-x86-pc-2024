using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {


        //
        public static unsafe int smplBitstreamReader_ReadNextFrame_func_v(ref QOI_common pQdcObjInfoParam, ref int pold_lPktId, uint uiToInMs, byte[] pBsBuf, ref uint puiBsBufSize, ref uint puiSampleTimeInMs, object p0)
        {
            uint uiPts = 0;
            SMPL_bsRead_param param;
            mymemset((IntPtr)(byte*)&param, 0, sizeof(SMPL_bsRead_param));

            //				
            return smplBitstreamReader_ReadNextFrame_func_v_new(ref pQdcObjInfoParam, ref pold_lPktId, uiToInMs, pBsBuf, ref puiBsBufSize, ref puiSampleTimeInMs, ref uiPts, &param, p0);
        }



        // 视频流解析:
        public static unsafe int smplBitstreamReader_ReadNextFrame_func_v_new(ref QOI_common pQdcObjInfoParam, ref int pold_lPktId, uint uiToInMs, byte[] pBsBuf, ref uint puiBsBufSize, ref uint puiSampleTimeInMs, ref uint puiPts, SMPL_bsRead_param* pParam, object p0)
        {
            int iErr = -1;

            string str;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            //CHECK_POINTER(pBS, MFX_ERR_NULL_PTR);
            //  CHECK_ERROR(m_bInited, false, MFX_ERR_NOT_INITIALIZED);

#if _DEBUG
			//OutputDebugString(  _T(  "smplBitstreamReader::ReadNextFrame_func called\n"  )  );
#endif

            //
            uint nBytesRead = 0;



#if false
    nBytesRead = (mfxU32)fread(pBS->Data + pBS->DataLength, 1, pBS->MaxLength - pBS->DataLength, m_fSource);
#else
            byte* pInput = null;    //  m_var.inputInfo.pInput;
            uint uiInputSize = 0;  //  m_var.inputInfo.uiInputSize;
            uint uiSampleTimeInMs = 0; //  2015/01/28
            uint uiPts = 0;                //  2015/02/09
                                           //TCHAR tBuf[256];

            ref QOI_common pQdcObjInfo = ref pQdcObjInfoParam;
#if false
			if (!pQdcObjInfo)
			{
				showInfo_open(0, 0, _T("CMySmpl::ReadNextFrame failed, pQdcObjInfo is null"));
				return -1;
			}
#endif
            //  QDC_OBJ_hdDec	*	pObj	=	(  QDC_OBJ_hdDec  *  )pQdcObjInfo->pObj;
            //CMyDecPipeline  *  pPipeline  =  (  CMyDecPipeline  *  )pObj->pPipeline;
            //
            if (!pQdcObjInfo.var.ucbTransformFuncsOk)
            {
                showInfo_open(0, null, _T("CMySmpl::ReadNextFrame failed, ucbTransformFuncsOk false"));
                return -1;
            }

            //
            QY_TRANSFORM pTransform = (QY_TRANSFORM)pQdcObjInfo.cfg.v.pParent_transform;


            for (; ; )
            {

                //  2014/07/26
                uint dwTickCnt_start = mytimeGetTime();
                //
                for (; ; )
                {
                    if (pQdcObjInfo.cfg.v.pf_bTransformVideoQuit(pQdcObjInfo.cfg.v.pParent_transform, null, null)) return -1;
                    //
                    //
                    QY_Q2 pDataQ2 = pTransform.video.q2;
                    pDataQ2 = pTransform.v_preTransThread.q2;
                    if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                    {
                        if (0 == getNextPktInfo_toDec(ref pTransform.video.curPktInfo_toDec, pDataQ2))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (0 == getNextPktInfo_toDec(ref pTransform.video.curPktInfo_toDec, pTransform.v_preTransThread.qBuf2))
                        {
                            break;
                        }
                    }


                    //  2014/07/26
                    if (0 != uiToInMs)
                    {
                        uint dwTickCnt = mytimeGetTime();
                        if (dwTickCnt - dwTickCnt_start > uiToInMs)
                        {
                            str = string.Format("SmplBsReader::LoadNextFrame failed, timeout. toInMs %d", uiToInMs);
                            showInfo_open(0, null, str);
                            //
                            goto errLabel;
                        }
                    }

                    //
#if false
			  Sleep(  40  );
#else
                    //
                    int nWaitInMs = 40;
                    //
                    nWaitInMs = 100;    //  2015/02/06. 是不是放大点能让cpu更悠闲点。但是，不能影响获取包的实时性
                                        //
                    uint dwRet;
                    QY_Q2 pVideoQ2 = (QY_Q2)pQdcObjInfo.cfg.v.pVideoQ2;
                    pVideoQ2 = pDataQ2;
                    if (pProcInfo.cfg.b_useVideoQ2_toDecV) { 
                        myWaitOne(pVideoQ2.hEvent0, nWaitInMs);                
                    }
                    else
                    {
                        myWaitOne(pTransform.v_preTransThread.qBuf2.hEvent0, nWaitInMs);
                    }


                    //
#if __DEBUG__
#endif
#endif

                }
                //
                int tmpiRet  =  -1;
                if (pProcInfo.cfg.b_useVideoQ2_toDecV)
                {
                    //tmpiRet = dec_parseEncVPkt_i_useVideoQ2(pQdcObjInfo.cfg.v.uiTransformType_pParent, pQdcObjInfo.cfg.v.pParent_transform, ref pTransform.video.curPktInfo_toDec, pold_lPktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts, pParam);
                }
                else
                {
                    tmpiRet = dec_parseEncVPkt_i(pQdcObjInfo.cfg.v.uiTransformType_pParent, pQdcObjInfo.cfg.v.pParent_transform, ref pTransform.video.curPktInfo_toDec, ref pold_lPktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts);


                }

        


                if (tmpiRet == Consts.CONST_qyRet_needKeyFrameFirst)
                {
                    clean_PKT_info_toDec(ref pTransform.video.curPktInfo_toDec, _T(""));
                    continue;
                }
                if (0 != tmpiRet)
                {
                    showInfo_open(0, null, _T("doTrans_default failed, dec_parseEncVPkt_i failed"));
                    return -1;
                }
                //
                break;
            }

            //
            if (0 == uiInputSize)
            {
                showInfo_open(0, null, _T("CMySmpl::ReadNextFrame failed, uiInputSize 0"));
                return -1;
            }
            if (uiInputSize > puiBsBufSize)
            {
                str = string.Format("CMySmpl~::ReadNextFrame failed, inputSize %d, dataSize %d:", uiInputSize, puiBsBufSize);
                showInfo_open(0, null, str);
                return -1;
            }
            //
#if _DEBUG
#endif
            //  2014/08/05
            //postVPktToSave(pQdcObjInfo.cfg.v.uiTransformType_pParent, pQdcObjInfo.cfg.v.pParent_transform, pQdcObjInfo.cfg.v.iDecParam_index_pMems_from, uiSampleTimeInMs, uiPts, (byte*)pInput, uiInputSize);
            //
            //  memcpy(  pBS->Data + pBS->DataLength, pInput,  uiInputSize  );  			
            //mymemcpy((IntPtr)pBsBuf, (IntPtr)pInput, uiInputSize);
            System.Runtime.InteropServices.Marshal.Copy((IntPtr)pInput, pBsBuf, 0, (int)uiInputSize);
            nBytesRead = uiInputSize;
            //
            //m_var.inputInfo.bFinished  =  TRUE;
#endif

            if (0 == nBytesRead)
            {
                return -1;
            }

            //  pBS->DataLength += nBytesRead;    
            puiBsBufSize = nBytesRead;
            //  2015/01/28
            if (null != puiSampleTimeInMs) puiSampleTimeInMs = uiSampleTimeInMs;
            if (null != puiPts) puiPts = uiPts;    //  2015/02/09

            //
            clean_PKT_info_toDec(ref pTransform.video.curPktInfo_toDec, _T(""));



            //
            iErr = 0;

        errLabel:
            //
            return iErr;
        }



#if false
		// 
		public static unsafe int old_smplBitstreamReader_ReadNextFrame_func_v_new(ref QOI_common pQdcObjInfoParam,  int * pold_lPktId, uint uiToInMs, byte[] pBsBuf, uint* puiBsBufSize, uint* puiSampleTimeInMs, uint* puiPts, SMPL_bsRead_param* pParam)
		{
			int iErr = -1;

			string str;


			//CHECK_POINTER(pBS, MFX_ERR_NULL_PTR);
			//  CHECK_ERROR(m_bInited, false, MFX_ERR_NOT_INITIALIZED);

#if _DEBUG
			//OutputDebugString(  _T(  "smplBitstreamReader::ReadNextFrame_func called\n"  )  );
#endif

			//
			uint nBytesRead = 0;



#if false
    nBytesRead = (mfxU32)fread(pBS->Data + pBS->DataLength, 1, pBS->MaxLength - pBS->DataLength, m_fSource);
#else
			byte* pInput = null;    //  m_var.inputInfo.pInput;
			uint uiInputSize = 0;  //  m_var.inputInfo.uiInputSize;
			uint uiSampleTimeInMs = 0; //  2015/01/28
			uint uiPts = 0;                //  2015/02/09
			//TCHAR tBuf[256];

			ref QOI_common pQdcObjInfo = ref pQdcObjInfoParam;
#if false
			if (!pQdcObjInfo)
			{
				showInfo_open(0, 0, _T("CMySmpl::ReadNextFrame failed, pQdcObjInfo is null"));
				return -1;
			}
#endif
			//  QDC_OBJ_hdDec	*	pObj	=	(  QDC_OBJ_hdDec  *  )pQdcObjInfo->pObj;
			//CMyDecPipeline  *  pPipeline  =  (  CMyDecPipeline  *  )pObj->pPipeline;
			//
			if (!pQdcObjInfo.var.ucbTransformFuncsOk)
			{
				showInfo_open(0, null, _T("CMySmpl::ReadNextFrame failed, ucbTransformFuncsOk false"));
				return -1;
			}

			//  2014/07/26
			uint dwTickCnt_start = mytimeGetTime();
			//
			for (; ; )
			{
				if (pQdcObjInfo.cfg.v.pf_bTransformVideoQuit(pQdcObjInfo.cfg.v.pParent_transform, null, null)) return -1;
				if (0==pQdcObjInfo.cfg.v.pf_getNextVPkt((IntPtr)pQdcObjInfo.cfg.v.pCurPkt, pQdcObjInfo.cfg.v.pInDataCacheQ, pQdcObjInfo.cfg.v.pVideoQ2))
				{
					break;
				}

				//  2014/07/26
				if (0!=uiToInMs)
				{
					uint dwTickCnt = mytimeGetTime();
					if (dwTickCnt - dwTickCnt_start > uiToInMs)
					{
						str=string.Format("SmplBsReader::LoadNextFrame failed, timeout. toInMs %d", uiToInMs);
						showInfo_open(0, null, str);
						//
						goto errLabel;
					}
				}

				//
#if false
			  Sleep(  40  );
#else
				//
				int nWaitInMs = 40;
				//
				nWaitInMs = 100;    //  2015/02/06. 是不是放大点能让cpu更悠闲点。但是，不能影响获取包的实时性
									//
				uint dwRet;
				QY_Q2 pVideoQ2 = (QY_Q2)pQdcObjInfo.cfg.v.pVideoQ2;
				myWaitOne(pVideoQ2.hEvent0, nWaitInMs);

				//
#if __DEBUG__
#endif
#endif

		}
			//
			if (0!=dec_parseEncVPkt_i(pQdcObjInfo.cfg.v.uiTransformType_pParent, pQdcObjInfo.cfg.v.pParent_transform, pQdcObjInfo.cfg.v.pCurPkt, pold_lPktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts, pParam))
			{
				showInfo_open(0, null, _T("doTrans_default failed, parsePkt failed"));
				return -1;
			}
			if (0==uiInputSize)
			{
				showInfo_open(0, null, _T("CMySmpl::ReadNextFrame failed, uiInputSize 0"));
				return -1;
			}
			if (uiInputSize > *puiBsBufSize)
			{
				str = string.Format("CMySmpl~::ReadNextFrame failed, inputSize %d, dataSize %d:", uiInputSize, *puiBsBufSize);
				showInfo_open(0, null, str);
				return -1;
			}
			//
#if _DEBUG
#endif
			//  2014/08/05
			//postVPktToSave(pQdcObjInfo.cfg.v.uiTransformType_pParent, pQdcObjInfo.cfg.v.pParent_transform, pQdcObjInfo.cfg.v.iDecParam_index_pMems_from, uiSampleTimeInMs, uiPts, (byte*)pInput, uiInputSize);
			//
			//  memcpy(  pBS->Data + pBS->DataLength, pInput,  uiInputSize  );  			
			//mymemcpy((IntPtr)pBsBuf, (IntPtr)pInput, uiInputSize);
			System.Runtime.InteropServices.Marshal.Copy((IntPtr)pInput, pBsBuf, 0,  (int)uiInputSize);
			nBytesRead = uiInputSize;
			//
			//m_var.inputInfo.bFinished  =  TRUE;
#endif

			if (0 == nBytesRead)
			{
				return -1;
			}

			//  pBS->DataLength += nBytesRead;    
			*puiBsBufSize = nBytesRead;
			//  2015/01/28
			if (null!=puiSampleTimeInMs) *puiSampleTimeInMs = uiSampleTimeInMs;
			if (null!=puiPts) *puiPts = uiPts;    //  2015/02/09

			//
#if true
			//
			bool bUseSharedTex = pQdcObjInfo.var.usPktResType_dec == Consts.CONST_pktResType_sharedTex;

			//
	GENERIC_Q    pTmpDataQ  =  (  GENERIC_Q    )pQdcObjInfo.cfg.v.pTmpDataQ;
	if  (  !bUseSharedTex
				&& pTmpDataQ.uiQNodes  <  60	// 30	//  15	//  2015/02/18	//  15	//  5	//  30  //  2014/04/12
		&&  0==qPostMsg(  (IntPtr)pQdcObjInfo.cfg.v.pCurPkt,  (uint)pQdcObjInfo.cfg.v.size_pCurPkt,  (  GENERIC_Q    )pQdcObjInfo.cfg.v.pTmpDataQ  )  )  
	{
		pQdcObjInfo.cfg.v.pf_detachPkt(  pQdcObjInfo.cfg.v.pCurPkt,  null  );
		}
	else  {
		  pQdcObjInfo.cfg.v.pf_clean_myDRAW_VIDEO_DATA(  (IntPtr)pQdcObjInfo.cfg.v.pCurPkt,  _T(  ""  )  );
	}
#else
			pQdcObjInfo->cfg.pf_clean_myDRAW_VIDEO_DATA(pQdcObjInfo->cfg.pCurPkt, _T(""));
#endif

			iErr = 0;

		errLabel:
			//
			return iErr;
		}
#endif



    }
}
