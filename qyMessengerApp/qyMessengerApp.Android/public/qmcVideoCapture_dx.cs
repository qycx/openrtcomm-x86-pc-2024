using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using qy;

//
namespace qy.Droid
{
	partial class qyFuncs_Droid
{

		public static unsafe int BufferCB_av(CCtxQmc pProcInfoParam, int iIndex_capProcInfo, void* pSampleGrabberCbVar, int iSampleTimeInMs, byte* pBuffer, int BufferLen,int w,int h,int iTestNo)
		{
			int hr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			bool bDbg = false;
			string str;
			//
			if (bDbg)
			{
				str = string.Format("BufferCB_av called {0}", iTestNo);
				qyFuncs.traceLog(str);
			}

			
			QY_SHARED_OBJ pSharedObj = null;

			//if (!pSampleGrabberCbVar) return -1;
			//SAMPLE_grabberCb_var & m_var = *(SAMPLE_grabberCb_var*)pSampleGrabberCbVar;

			//  CAP_procInfo_video	*	pVc;
			CAP_procInfo_bmpCommon pVc;

			pVc = (CAP_procInfo_bmpCommon)qyFuncs.getCapBmpBySth(pProcInfo, iIndex_capProcInfo, 0);
			if (null==pVc) goto errLabel;
			pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, pVc.iIndex_sharedObj);     //  ÕâÀïÒªÓÃindexÀ´»ñÈ¡
			if (null==pSharedObj) goto errLabel;

			SHARED_OBJ_USR pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
			if (null==pSharedObjUsr) goto errLabel;



			//  

			//
			CAP_procInfo_bmpCommon pCapProcInfo = pVc;
			
			//
			using (CQyMalloc mallocObj = new CQyMalloc()) {

				myDRAW_VIDEO_DATA* p;
				int len;

				//  len  =  offsetof(  myDRAW_VIDEO_DATA,  buf  )  +  pCapProcInfo->vh_org.bih.biSizeImage;
				len = sizeof(myDRAW_VIDEO_DATA);
				myDRAW_VIDEO_DATA tmp_myDRAW_VIDEO_DATA;
				p = &tmp_myDRAW_VIDEO_DATA; //  (  myDRAW_VIDEO_DATA  *  )m_var.pBuf;
				if (null == p) goto errLabel;
				qyFuncs.mymemset((IntPtr)p, 0, sizeof(myDRAW_VIDEO_DATA));    //  memset(  p,  0,  m_var.uiBufSize_pBuf  );

				//
#if __DEBUG__
			safeTcsnCpy(_T("BuffCB_av: "), pVc->compressVideo.input.tBuf, mycountof(pVc->compressVideo.input.tBuf));
#endif
				//
				
				if (0!=qyFuncs.setPktInputInfo(pProcInfo, ref pVc.compressVideo.input, pVc.compressVideo.usFrames_perSecond_expected, false, p))
				{
					goto errLabel;
				}
				
				//
				//byte* pInput = pBuffer;
				uint uiSize_dec = (uint)BufferLen;
				/*
				BITMAPINFO* pBmpInfo_input = (BITMAPINFO*)&pVc->vh_org.bih;
				//
				uiSize_dec=max(pVc->vh_org.bih.biSizeImage, BufferLen);
				if (pVc->bOrg2RgbBufInited) uiSize_dec = max(uiSize_dec, pVc->vh_rgb.bih.biSizeImage);
				//  if  (  pVc->iPicBufSize  )  uiSize_dec  =  max(  uiSize_dec,  pVc->vh_pic.bih.biSizeImage  );
				//  2013/10/06
				*/
				uiSize_dec  =  (uint)Math.Max(  uiSize_dec,  qyFuncs.M_sizeImage(  32,  w,  h  )  );
				

				//  2011/12/06
				uint uiSize_enc = (uint)sizeof(TRANSFER_VIDEO_DATA);

				//
				p->head.uiLen = uiSize_dec + uiSize_enc;        //  pBmpInfo_input->bmiHeader.biSizeImage;	
				p->head.uiSampleTimeInMs = (uint)iSampleTimeInMs;
				//
				p->uiSize_dec = uiSize_dec;
				//
				if (null==mallocObj.mallocf(Consts.CONST_iAllocType_video, (int)(uiSize_dec + uiSize_enc))) goto errLabel;
				if (0!=mallocObj.get(ref p->memory)) goto errLabel;

				//  2011/12/04
				//memcpy(&p->bih, &pVc->vh_org.bih, sizeof(p->bih)  );
				qyFuncs.makeBmpInfoHeader_i420(12, w, h, &p->bih);
				//
				qyFuncs.mymemcpy(p->memory.m_pBuf, (IntPtr)pBuffer, (uint)BufferLen);
				//System.Runtime.InteropServices.Marshal.Copy(pBuffer, 0, p->memory.m_pBuf, BufferLen);


#if false
			 p->debugInfo.dwTickCnt_start  =  GetTickCount(  );
			 M_pkt_setNo(  m_var.tn_pktNo,  m_var.lLastPktNo,  *p  );
#endif
				p->debugInfo.lPktNo = iTestNo;

				//
				qyFuncs.M_set_pktId_alloc(p);


#if DEBUG
			if (0==p->input.tStartTran)
			{
				qyFuncs.traceLog(("kk"));
			}
				if (bDbg)
				{
					str = string.Format("BufferCB_av: pktNo {0}", p->debugInfo.lPktNo);
					qyFuncs.traceLog(str);
				}
#endif
				//
				//GuiShare.pf_dumpData1(p->memory.m_pBuf, p->bih.biSizeImage, p->bih.biWidth, p->bih.biHeight, "bufferCb_avkk.yuv");


				//
				if (0!=qyFuncs.q2PostMsgAndTrigger((IntPtr)p, (uint)len, pCapProcInfo.vppThread.q2)) goto errLabel;
				/*
				if ( 0!=qyFuncs.q2PostMsg((IntPtr)p, (uint)len, pProcInfo.av.localAv.player.video.q2) )
                {
					str=string.Format("BufferCB_av, q2PostMsg failed {0}",iTestNo);
					qyFuncs.traceLog(str);

                }
				else
                {
					if (bDbg)
					{
						str = string.Format("BufferCB_av, q2PostMsg ok {0}", iTestNo);
						qyFuncs.traceLog(str);
					}

				}
				*/
				mallocObj.detach();
			}
			


			hr = 0;

		errLabel:

			return hr;
		}

	}
}



