using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int smplBitstreamReader_ReadNextFrame_func_a(ref QOI_common pQdcObjInfoParam, ref int pold_lPktId, uint uiToInMs, byte[] pBsBuf, ref uint puiBsBufSize, ref uint puiSampleTimeInMs,  object p0)
		{
			uint uiPts = 0;
			SMPL_bsRead_param param;
			mymemset((IntPtr)(byte*)&param, 0, sizeof(SMPL_bsRead_param));

			return smplBitstreamReader_ReadNextFrame_func_a_new(ref pQdcObjInfoParam, ref pold_lPktId, uiToInMs, pBsBuf, ref puiBsBufSize, ref puiSampleTimeInMs, ref uiPts, &param, p0);
		}


		// 
		public static unsafe int smplBitstreamReader_ReadNextFrame_func_a_new(ref QOI_common pQdcObjInfoParam, ref int pold_lPktId, uint uiToInMs, byte[] pBsBuf, ref uint puiBsBufSize, ref uint puiSampleTimeInMs, ref uint puiPts, SMPL_bsRead_param* pParam, object p0)
		{
			//qyFuncs.traceLog("TAG", "smplBitstreamReader_ReadNextFrame_func_a_new enter");
			
			//
			int iErr = -1;

			string str;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//CHECK_POINTER(pBS, MFX_ERR_NULL_PTR);
			//  CHECK_ERROR(m_bInited, false, MFX_ERR_NOT_INITIALIZED);



			//
			uint nBytesRead = 0;



#if  false
    nBytesRead = (mfxU32)fread(pBS->Data + pBS->DataLength, 1, pBS->MaxLength - pBS->DataLength, m_fSource);
#else
			//IntPtr pInput = IntPtr.Zero;    //  m_var.inputInfo.pInput;
			uint uiInputSize = 0;  //  m_var.inputInfo.uiInputSize;
			uint uiSampleTimeInMs = 0; //  2015/01/28
			uint uiPts = 0;                //  2015/02/09
			//TCHAR tBuf[256];

			ref QOI_common pQdcObjInfo = ref pQdcObjInfoParam;
#if false
			if (null==pQdcObjInfo)
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

			//  2016/12/13
			QY_TRANSFORM pTransform = (QY_TRANSFORM)pQdcObjInfo.cfg.a.pParent_transform;

			//  2014/07/26
			uint dwTickCnt_start = mytimeGetTime();
			//
			for (; ; )
			{
				if (pQdcObjInfo.cfg.a.pf_bTransformAudioQuit(pQdcObjInfo.cfg.a.pParent_transform, IntPtr.Zero, IntPtr.Zero)) return -1;

				//
				//  2016/12/13
				if (pProcInfo.cfg.b_useAudioQ2_toDecA)
				{
					if (0 == getNextPktInfo_toDec(ref pTransform.audio.curPktInfo_toDec, pTransform.audio.q2)) break;
				}
				else
                {
					if (0 == getNextPktInfo_toDec(ref pTransform.audio.curPktInfo_toDec, pTransform.audio.qBuf2)) break;

				}


				//  2014/07/26
				if (0!=uiToInMs)
				{
					uint dwTickCnt = mytimeGetTime();
					if (dwTickCnt - dwTickCnt_start > uiToInMs)
					{
						str=string.Format("SmplBsReader::LoadNextFrame failed, timeout. toInMs %d", uiToInMs);
						showInfo_open(0, null, str);
						goto errLabel;
					}
				}

				//
#if  false
			  Sleep(  40  );
#else
				//
				int nWaitInMs = 40;
				//
				nWaitInMs = 100;    //  2015/02/06. 是不是放大点能让cpu更悠闲点。但是，不能影响获取包的实时性
									//
				uint dwRet;
				if (pProcInfo.cfg.b_useAudioQ2_toDecA)
				{
					QY_Q2 pAudioQ2 = (QY_Q2)pQdcObjInfo.cfg.a.pAudioQ2;
					myWaitOne(pAudioQ2.hEvent0, nWaitInMs);
				}
				else
                {
					myWaitOne(pTransform.audio.qBuf2.hEvent0, nWaitInMs);

				}
#if __DEBUG__
#endif
#endif

			}
          
            //
            if (pProcInfo.cfg.b_useAudioQ2_toDecA)
			{
				//if (0 != dec_parseEncAPkt_i_useAudioQ2(pQdcObjInfo.cfg.a.uiTransformType_pParent, pQdcObjInfo.cfg.a.pParent_transform, ref pTransform.audio.curPktInfo_toDec, pold_lPktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts))
				//{
				//	showInfo_open(0, null, _T("doTrans_default failed, parsePkt failed"));
				//	return -1;
				//}
			}
			else
            {
			
				if (0 != GuiShare.pf_dec_parseEncAPkt_i(pQdcObjInfo.cfg.a.uiTransformType_pParent, pQdcObjInfo.cfg.a.pParent_transform, ref pTransform.audio.curPktInfo_toDec, ref pold_lPktId,  ref uiInputSize, ref uiSampleTimeInMs, ref uiPts,p0))
				{
					showInfo_open(0, null, _T("doTrans_default failed, parsePkt failed"));
					return -1;
				}

			}
			if (0==uiInputSize)
			{
				showInfo_open(0, null, _T("CMySmpl::ReadNextFrame failed, uiInputSize 0"));
				return -1;
			}
			if (uiInputSize > puiBsBufSize)
			{
				str=string.Format("CMySmpl~::ReadNextFrame failed, inputSize %d, dataSize %d:", uiInputSize, puiBsBufSize);
				showInfo_open(0, null, str);
				return -1;
			}

			//
#if _DEBUG
#endif
			//  2014/08/05
			//postAPktToSave(pQdcObjInfo.cfg.a.uiTransformType_pParent, pQdcObjInfo->cfg.a.pParent_transform, pQdcObjInfo->cfg.a.iDecParam_index_pMems_from, uiSampleTimeInMs, uiPts, (char*)pInput, uiInputSize);
			//
			//  memcpy(  pBS->Data + pBS->DataLength, pInput,  uiInputSize  );  			
			//mymemcpy((IntPtr)pBsBuf, (IntPtr)pInput, uiInputSize);
			fixed (byte* pInput = pTransform.audio.curPktInfo_toDec.pkt_encA.buf)
			{
				System.Runtime.InteropServices.Marshal.Copy((IntPtr)pInput, pBsBuf, 0, (int)uiInputSize);
			}
			//
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
			if (null!=puiSampleTimeInMs) puiSampleTimeInMs = uiSampleTimeInMs;
			if (null!=puiPts) puiPts = uiPts;    //  2015/02/09

			//
			//  2016/12/13
			clean_PKT_info_toDec(ref pTransform.audio.curPktInfo_toDec, _T(""));




			//
			iErr = 0;

		errLabel:
			//
			return iErr;
		}


	}


}
