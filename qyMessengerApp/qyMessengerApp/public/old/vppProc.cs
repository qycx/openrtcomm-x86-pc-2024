using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

#if false

	public unsafe struct VPP_helpInfo
	{
		//
		public IntPtr pBuf;
		public uint uiBufSize;

		/*
		
		//
		struct                         {
					 bool bProcessed_step0;
		bool bProcessed_step1;
		bool bProcessed_step2;
	}
	procFlgs;
				 //
				 unsigned int uiCnt_vpp0;
		*/

		//
#if __DEBUG__
	char buf[100];
#endif

	}
;
	public unsafe struct CTX_vpp_resize
	{
		public BITMAPINFOHEADER bih_rgb;
		public byte* pBuf;
		public uint uibufSize;
	};

	public unsafe struct CTX_vpp_vpp
	{
		//  2011/12/05
		public BITMAPINFOHEADER bih_vpp;
		public int iVppBufSize;
		public byte* pVppBuf;
	};

	public unsafe struct CTX_vpp
	{
		//
		public bool ucbSingleThread_vpp;

		
		//
		public CTX_addTxt addTxt;
		//
		public CTX_vpp_resize resize;
				 //
		public CTX_vpp_vpp vpp;
		

//  2015/05/07
public QY_qThreadProcInfo_common vppThread1_proc;
public QY_qThreadProcInfo_common vppThread2_beforeEnc;
		

//
public int iIndex_sharedObj;

		//
		public byte ucCnt_vppHelpInfos;
		//public VPP_helpInfo* pVppHelpInfos;

		//
		public uint uiCnt_vpp0;
		public uint uiCnt_vpp1;
		public uint uiCnt_vpp2;



}		 ;

	partial class qyFuncs
    {
		public static unsafe int old_doVpp(CCtxQmc pProcInfo, ref CTX_vpp pCtx, QY_SHARED_OBJ pSharedObj, CAP_procInfo_bmpCommon pCapBmp, myDRAW_VIDEO_DATA* p, int pktLen)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;// QY_GET_GBUF();
			//  MC_VAR_isCli  *  pProcInfo	=	QY_GET_procInfo_isCli(  );

			//
			bool bNeedVppBuf = false;
			int iFourcc = Consts.CONST_fourcc_YV12;

			//
			if (pCapBmp.compressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_ipp
				|| pCapBmp.compressVideo.compressor.common.head.ucCompressors == Consts.CONST_videoCompressors_hwAccl
				//||  pCapBmp.compressVideo.compressor.common.ucCompressors  ==  CONST_videoCompressors_cuda  
				)
			{
				bNeedVppBuf = true;
			}
			//  2014/07/21
			if (pCapBmp.compressVideo.vh_decompress.bih.biCompression == Consts.CONST_fourcc_I420)
			{
				bNeedVppBuf = false;
				iFourcc = Consts.CONST_fourcc_I420;
			}

			//
			if ( !bNeedVppBuf )
            {
				ref myDRAW_VIDEO_DATA tmp_pkt = ref *p;
				iErr = 0; goto errLabel;
            }

			//
			switch (pCapBmp.uiType)
			{
				case Consts.CONST_capType_av:
				case Consts.CONST_capType_mediaDevice:
				case Consts.CONST_capType_screen:
				case Consts.CONST_capType_mediaFile:
				case Consts.CONST_capType_mosaic:
					{
						//  
						CAP_procInfo_bmpCommon pVc = (CAP_procInfo_bmpCommon)pCapBmp;
						byte* pInput = null;
						//BITMAPINFOHEADER * pBmpInfo_input = null;

						//					
						if (!pVc.bOrg2IntBufInited)
						{

							pInput = (byte*)p->memory.m_pBuf;
							BITMAPINFOHEADER* pBmpInfo_input = (BITMAPINFOHEADER*)&p->bih;


							//  2013/09/20							
							//doPreChromaKey(  pProcInfo,  &pBmpInfo_input->bmiHeader,  (  byte  *  )pInput,  0  );


							//
							if (bNeedVppBuf)
							{
								if (p->bih.biWidth == pCapBmp.compressVideo.vh_decompress.bih.biWidth && p->bih.biHeight == pCapBmp.compressVideo.vh_decompress.bih.biHeight
									//||  pCapBmp.compressVideo.compressor.common.ucCompressors  ==  CONST_videoCompressors_hwAccl  
									)
								{
									//
									if (0!=chkVppBuf(ref pCtx, pCapBmp, p->bih.biWidth, p->bih.biHeight, bNeedVppBuf, iFourcc)) goto errLabel;
									//
									//  rgb24ToYv12(  (  byte  *  )pInput,  (  byte  *  )pCtx.vpp.pVppBuf,  p->bih.biWidth,  p->bih.biHeight,  pCapBmp.compressVideo.compressor.common.ucbOutputFlippedImg  );									
									rgb24ToVppBuf((byte*)pInput, (byte*)pCtx.vpp.pVppBuf, p->bih.biWidth, p->bih.biHeight, false, iFourcc);
									//
									mymemcpy(p->memory.m_pBuf, (IntPtr)pCtx.vpp.pVppBuf, (uint)pCtx.vpp.bih_vpp.biSizeImage);
									//memcpy(&p->bih, &pCtx.vpp.bih_vpp, sizeof(p->bih)  );
									p->bih = pCtx.vpp.bih_vpp;
								}
								else
								{  //  2014/04/24
									if (0!=chkResizeBuf(ref pCtx, pCapBmp)) goto errLabel;
									if (0!=pProcInfo.resizeRgb24((byte *  )p->memory.m_pBuf,  p->bih.biWidth,  p->bih.biHeight,  (byte *  )pCtx.resize.pBuf,  pCapBmp.compressVideo.vh_decompress.bih.biWidth,  pCapBmp.compressVideo.vh_decompress.bih.biHeight  )  )  goto errLabel;

									if (0!=chkVppBuf(ref pCtx, pCapBmp, pCtx.resize.bih_rgb.biWidth, pCtx.resize.bih_rgb.biHeight, bNeedVppBuf, iFourcc)) goto errLabel;
									if (pCtx.vpp.iVppBufSize > p->memory.uiBufSize) goto errLabel;
									//
									//  rgb24ToYv12(  (  unsigned  char  *  )pCtx.resize.pBuf,  (  unsigned  char  *  )p->memory.m_pBuf,  pCtx.vpp.bih_vpp.biWidth,  pCtx.vpp.bih_vpp.biHeight,  pCapBmp.compressVideo.compressor.common.ucbOutputFlippedImg  );
									rgb24ToVppBuf((byte *  )pCtx.resize.pBuf,  (byte *  )p->memory.m_pBuf,  pCtx.vpp.bih_vpp.biWidth,  pCtx.vpp.bih_vpp.biHeight,  false,  iFourcc  );
									//
									//memcpy(&p->bih, &pCtx.vpp.bih_vpp, sizeof(p->bih)  );
									p->bih = pCtx.vpp.bih_vpp;
								}
							}
							else
							{   //  还有一种情况是，vh_org的长度不对，所以，需要调整到pCapBmp.compressVideo.vh_decompress
								if (p->bih.biWidth != pCapBmp.compressVideo.vh_decompress.bih.biWidth || p->bih.biHeight != pCapBmp.compressVideo.vh_decompress.bih.biHeight)
								{
									if (0!=chkResizeBuf(ref pCtx, pCapBmp)) goto errLabel;
									if (0!=pProcInfo.resizeRgb24((byte *  )p->memory.m_pBuf,  p->bih.biWidth,  p->bih.biHeight,  (byte *  )pCtx.resize.pBuf,  pCtx.resize.bih_rgb.biWidth,  pCtx.resize.bih_rgb.biHeight  )  )  goto errLabel;
									if (pCtx.resize.bih_rgb.biSizeImage > p->memory.uiBufSize) goto errLabel;
									mymemcpy(p->memory.m_pBuf, (IntPtr)(byte *  )pCtx.resize.pBuf,  (uint)pCtx.resize.bih_rgb.biSizeImage  );
									//
									//memcpy(&p->bih, &pCtx.resize.bih_rgb, sizeof(p->bih)  );
									p->bih = pCtx.resize.bih_rgb;
								}
							}

						}
						else
						{
							if (0!=org2Rgb(pProcInfo, pVc, (byte*)p->memory.m_pBuf, (byte*)pVc.pOrg2IntBuf))
							{
#if __DEBUG__
								traceLogA("Video capture: org2Rgb failed.");
#endif
								//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Video capture: Decompress failed"));
								goto errLabel;
							}
							pInput = (byte*)pVc.pOrg2IntBuf;
							fixed (BITMAPINFOHEADER* pBmpInfo_input = &pVc.vh_int.bih) {


								//  2013/09/20							
								//doPreChromaKey(  pProcInfo,  &pBmpInfo_input->bmiHeader,  (  byte  *  )pInput,  0  );


								//  2010/09/13

								//

								//
								if (bNeedVppBuf)
								{
									if (p->bih.biWidth == pCapBmp.compressVideo.vh_decompress.bih.biWidth && p->bih.biHeight == pCapBmp.compressVideo.vh_decompress.bih.biHeight
										//  ||  pCapBmp.compressVideo.compressor.common.ucCompressors  ==  CONST_videoCompressors_hwAccl  
										)
									{
										//
										if (0!=chkVppBuf(ref pCtx, pCapBmp, p->bih.biWidth, p->bih.biHeight, bNeedVppBuf, iFourcc)) goto errLabel;
										//
										//  rgb24ToYv12(  (  byte  *  )pInput,  (  byte  *  )p->memory.m_pBuf,  p->bih.biWidth,  p->bih.biHeight,  pCapBmp.compressVideo.compressor.common.ucbOutputFlippedImg  );									
										rgb24ToVppBuf((byte*)pInput, (byte*)p->memory.m_pBuf, p->bih.biWidth, p->bih.biHeight, false, iFourcc);
										//
										//memcpy(&p->bih, &pCtx.vpp.bih_vpp, sizeof(p->bih)  );
										p->bih = pCtx.vpp.bih_vpp;
									}
									else
									{
										if (0!=chkResizeBuf(ref pCtx, pCapBmp)) goto errLabel;
										if (0!=pProcInfo.resizeRgb24((byte*)pInput, p->bih.biWidth, p->bih.biHeight, (byte*)pCtx.resize.pBuf, pCapBmp.compressVideo.vh_decompress.bih.biWidth, pCapBmp.compressVideo.vh_decompress.bih.biHeight)) goto errLabel;
										//
										if (0!=chkVppBuf(ref pCtx, pCapBmp, pCtx.resize.bih_rgb.biWidth, pCtx.resize.bih_rgb.biHeight, bNeedVppBuf, iFourcc)) goto errLabel;
										//
										if (pCtx.vpp.iVppBufSize > p->uiSize_dec)
										{
											goto errLabel;
										}
										//
										//  rgb24ToYv12(  (  unsigned  char  *  )pCtx.resize.pBuf,  (  unsigned  char  *  )p->memory.m_pBuf,  pCtx.vpp.bih_vpp.biWidth,  pCtx.vpp.bih_vpp.biHeight,  pCapBmp.compressVideo.compressor.common.ucbOutputFlippedImg  );
										rgb24ToVppBuf((byte*)pCtx.resize.pBuf, (byte*)p->memory.m_pBuf, pCtx.vpp.bih_vpp.biWidth, pCtx.vpp.bih_vpp.biHeight, false, iFourcc);
										//
										//memcpy(&p->bih, &pCtx.vpp.bih_vpp, sizeof(p->bih)  );
										p->bih = pCtx.vpp.bih_vpp;
									}
								}
								else
								{
									//
									if (p->bih.biWidth != pCapBmp.compressVideo.vh_decompress.bih.biWidth || p->bih.biHeight != pCapBmp.compressVideo.vh_decompress.bih.biHeight)
									{
										if (0 != chkResizeBuf(ref pCtx, pCapBmp)) goto errLabel;
										if (0 != pProcInfo.resizeRgb24((byte*)pInput, pBmpInfo_input->biWidth, pBmpInfo_input->biHeight, (byte*)pCtx.resize.pBuf, pCtx.resize.bih_rgb.biWidth, pCtx.resize.bih_rgb.biHeight)) goto errLabel;
										if (pCtx.resize.bih_rgb.biSizeImage > p->uiSize_dec) goto errLabel;
										mymemcpy(p->memory.m_pBuf, (IntPtr)(byte*)pCtx.resize.pBuf, (uint)pCtx.resize.bih_rgb.biSizeImage);
										//
										//memcpy(&p->bih, &pCtx.resize.bih_rgb, sizeof(p->bih)  );
										p->bih = pCtx.resize.bih_rgb;
									}
									else
									{
										mymemcpy(p->memory.m_pBuf, (IntPtr)pInput, (uint)pBmpInfo_input->biSizeImage);
										//memcpy(&p->bih, pBmpInfo_input, sizeof(p->bih)  );
										p->bih = *pBmpInfo_input;
									}
								}
							}

						}


					}
					break;
				default:
					//TCHAR tBuf[128];
					//_sntprintf(tBuf, mycountof(tBuf), _T("doVpp: unprocessed capType %d"), pCapBmp.uiType);
#if __DEBUG__
					traceLog(_T("%s"), tBuf);
#endif
					//showInfo_open(0, 0, tBuf);
					goto errLabel;
			}



			iErr = 0;

		errLabel:

			return iErr;
		}

		//
		public static unsafe int doVpp0(CCtxQmc pProcInfo, ref CTX_vpp pCtx, QY_SHARED_OBJ pSharedObj, CAP_procInfo_bmpCommon pCapBmp, myDRAW_VIDEO_DATA* p, int pktLen)
        {
			return -1;
        }

	}

#endif


}
