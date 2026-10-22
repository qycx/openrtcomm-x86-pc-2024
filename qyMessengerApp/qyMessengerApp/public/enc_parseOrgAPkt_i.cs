using System;
using System.Collections.Generic;
using System.Text;

partial class Consts {
	public const int CONST_timeInMs_forceKeyFrame = 1800;	//  2015/10/04	//  4000  
		}



namespace qy
{
    partial class qyFuncs
{
		public static unsafe int enc_parseOrgAPkt_i(uint uiTransformType, object pParent_transform, void* pPkt_myPLAY_AUDIO_DATA, int* p_old_lPktId_getNewPkt, byte** ppInput, uint* puiSize, bool* pbNeed_forceKeyFrame)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			if (pQyMc == null) return -1;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//QY_GET_procInfo_isCli(  );
			//
			bool bNeed_forceKeyFrame = false;


			//  myDRAW_VIDEO_DATA  *  pPkt  =  pTransform->video.pCurPkt;
			myPLAY_AUDIO_DATA* pPkt = (myPLAY_AUDIO_DATA*)pPkt_myPLAY_AUDIO_DATA;

			//  检查一下。每个包都只能处理一次，否则，容易死循环
			if (null==p_old_lPktId_getNewPkt) return -1;
			if (0==pPkt->lPktId_getNewPkt)
			{
				showInfo_open(0, null, _T("enc_parseOrgAPkt_i failed: lPktId_alloc 0"));
				return -1;
			}
			if (pPkt->lPktId_getNewPkt == *p_old_lPktId_getNewPkt)
			{
				showInfo_open(0, null, _T("enc_parseOrgAPkt_i failed: lPkdId_alloc==old"));
				return -1;
			}
			*p_old_lPktId_getNewPkt = pPkt->lPktId_getNewPkt;


#if  true
		//  2014/04/22
		if  (  uiTransformType  ==  Consts.CONST_uiTransformType_encAudio  )  {
			CAP_procInfo_audioCommon    pCapAudio  =  (  CAP_procInfo_audioCommon    )pParent_transform;
			//
			//
			//  2016/12/15		
			int nAvgBytesPerSec = (int)pCapAudio.compressAudio.ah_decompress.wfx.nAvgBytesPerSec;
			if (0!=nAvgBytesPerSec)
			{
				pCapAudio.compressAudio.encInfo.st.last_iSampleTimeInMs_preEnc = pPkt->head.uiSampleTimeInMs;
				long llValInMs = pPkt->uiSize_dec * 1000;
				pCapAudio.compressAudio.encInfo.st.uiTimeLen_preEnc = (uint)(llValInMs / nAvgBytesPerSec);
			}

		}
#endif


#if  true		
		 
		 //		
		 if  (  pPkt->head.uiLen  >  pPkt->memory.uiBufSize  
			 //||  pPkt->uiSize_dec  +  pPkt->uiOffset_enc  +  pPkt->uiLen_enc  >  pPkt->head.uiLen  
			 )  			 
		 {
# if  __DEBUG__
						 traceLog(  _T(  "doDecVideo: uiSize_dec or uiLen error"  )  );
#endif
		showInfo_open(  0,  null, _T(  "doDecVideo: uiSize_dec or uiLen error"  )  );
				return  -1;		 			 
		 }
	byte* decData = (byte*)pPkt->memory.m_pBuf;    //    +  pPkt->uiSize_dec  +  pPkt->uiOffset_enc;
	myPLAY_AUDIO_DATA* pPkt_dec = pPkt;

		 //
		 //int  index_pMems_from  =  pPkt->decInfo.index_pMems_from;	


#endif

		 //
		 * ppInput = (byte*  )decData;
		 * puiSize = pPkt->uiSize_dec;  //  head.uiLen;


	iErr  =  0;
errLabel:

		 //  2014/08/25
		 if  (null!=pbNeed_forceKeyFrame  )  {
			 * pbNeed_forceKeyFrame = bNeed_forceKeyFrame;
}

return iErr;
}


		public static unsafe   int enc_parseOrgVPkt_i(uint uiTransformType, object pParent_transform, void* pPkt_myDRAW_VIDEO_DATA, int* p_old_lPktId_alloc, byte** ppInput, uint* puiSize, bool* pbNeed_forceKeyFrame)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			if (null==pQyMc) return -1;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //  QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;
			//
			bool bNeed_forceKeyFrame = false;
			//
			//TCHAR tBuf[128] = _T("");

			//  
			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pPkt_myDRAW_VIDEO_DATA;

			//

			//  检查一下。每个包都只能处理一次，否则，容易死循环
			if (null==p_old_lPktId_alloc) return -1;
			if (0==pPkt->lPktId_alloc)
			{
				showInfo_open(0, null, ("enc_parseOrgVPkt_i failed: lPktId_alloc 0"));
				return -1;
			}
			if (pPkt->lPktId_alloc == *p_old_lPktId_alloc)
			{
				//_sntprintf(tBuf, mycountof(tBuf), _T("enc_parseOrgVPkt_i failed: lPkdId_alloc==old %d"), pPkt->lPktId_alloc);
				//showInfo_open(0, 0, tBuf);
				return -1;
			}
			*p_old_lPktId_alloc = pPkt->lPktId_alloc;


#if  true
		//  2014/04/22
		if  (  uiTransformType  ==  Consts.CONST_uiTransformType_encVideo  )  {
			CAP_procInfo_bmpCommon    pCapBmp  =  (  CAP_procInfo_bmpCommon    )pParent_transform;
			//
# if  __DEBUG__
					int  iNextIndex_preEnc  =  pCapBmp.compressVideo.encInfo.iNextIndex_preEnc;
					int  iLastIndex_preEnc  =  0;
					if  (  !iNextIndex_preEnc  )  iLastIndex_preEnc  =  mycountof(  pCapBmp.compressVideo.encInfo.iSampleTimeInMs_preEnc_array  )  -  1;
					else  iLastIndex_preEnc  =  iNextIndex_preEnc  -  1;
					//
					//traceLog(  _T(  "enc_parseOrgVPkt_i: pkt.sm %dms. iDiff %dms"  ),  pPkt->head.uiSampleTimeInMs,  pPkt->head.uiSampleTimeInMs  -  pCapBmp.compressVideo.encInfo.iSampleTimeInMs_preEnc_array[iLastIndex_preEnc]  );
					//  
					//  2015/10/11. test reset of enc
					if  (  0  )  {
						static  int  nn  =  0;
						nn  ++  ;
						_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "TEST reset of enc, enc_parseOrgVPkt_i: nn: %d"  ),  nn  );
						showInfo_open(  0,  0,  tBuf  );
						if  (  !(  (  nn  +  1  )  %  100  )  )  {
							showInfo_open(  0,  0,  _T(  "TEST reset of enc"  )  );
							nn  =  0;
							goto  errLabel;					
						}
					}
#endif
			//
			if (pCapBmp.compressVideo.encInfo.iNextIndex_preEnc >= Consts.MAX_cnt_preEnc_array)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("enc_parseOrgVPkt_i failed, iNextIndex_preEnc too large"));
				goto errLabel;
			}
			pCapBmp.compressVideo.encInfo.iSampleTimeInMs_preEnc_array[pCapBmp.compressVideo.encInfo.iNextIndex_preEnc] = (int)pPkt->head.uiSampleTimeInMs;
			pCapBmp.compressVideo.encInfo.iNextIndex_preEnc++;
			if (pCapBmp.compressVideo.encInfo.iNextIndex_preEnc >= Consts.MAX_cnt_preEnc_array)
			{
				pCapBmp.compressVideo.encInfo.iNextIndex_preEnc = pCapBmp.compressVideo.encInfo.iNextIndex_preEnc % Consts.MAX_cnt_preEnc_array;
			}
			pCapBmp.compressVideo.encInfo.cnt_preEnc++;

			//
			if (pCapBmp.compressVideo.encInfo.dwTickCnt_startToForceKeyFrame!=0)
			{
				uint dwTickCnt = myGetTickCount(null);
				if (dwTickCnt - pCapBmp.compressVideo.encInfo.dwTickCnt_startToForceKeyFrame <Consts.CONST_timeInMs_forceKeyFrame)
				{
					bNeed_forceKeyFrame = true;
					//
# if  __DEBUG__
					//showInfo_open(  0,  0,  _T(  "enc_parseOrgVPkt_i: bNeed_forceKeyFrame true"  )  );
#endif
				}
			}
		}
#endif

#if  true		
		 
		 //		
		 if  (  pPkt->head.uiLen  >  pPkt->memory.uiBufSize  
			 ||  pPkt->uiSize_dec  +  pPkt->uiOffset_enc  +  pPkt->uiLen_enc  >  pPkt->head.uiLen  )  			 
		 {
# if  __DEBUG__
						 traceLog(  _T(  "enc_parseOrgVPkt_i failed: uiSize_dec or uiLen error"  )  );
#endif
		showInfo_open(  0,  null, (  "enc_parseOrgVPkt_i failed: uiSize_dec or uiLen error"  )  );
				return  -1;		 			 
		 }
		 //  2015/10/29
		 if  (pPkt->usPktResType  !=  Consts.CONST_pktResType_sharedTex  )  {
			 if  (pPkt->bih.biSizeImage  >  pPkt->uiSize_dec  )  {
				 showInfo_open(  0,  null, (  "enc_parseOrgVPkt_i failed, bih.sizeImg > size_dec"  )  );
				 return  -1;
			 }
		 }
		 //
		 byte* decData = (byte*)pPkt->memory.m_pBuf;   //    +  pPkt->uiSize_dec  +  pPkt->uiOffset_enc;
myDRAW_VIDEO_DATA* pPkt_dec = pPkt;

//
int index_pMems_from = pPkt->decInfo.index_pMems_from;


#endif

//
*ppInput = (byte*  )decData;
*puiSize = (uint)pPkt->bih.biSizeImage;


iErr = 0;
errLabel:

//  2014/08/25
if (null!=pbNeed_forceKeyFrame)
{
	*pbNeed_forceKeyFrame = bNeed_forceKeyFrame;
}

return iErr;
}



}
}
