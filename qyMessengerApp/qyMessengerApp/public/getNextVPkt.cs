using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		//
		public static unsafe int getNextAPkt(myPLAY_AUDIO_DATA* pCurPkt, GENERIC_Q pInDataCacheQ, QY_Q2 pAudioQ2)
		{
			int iErr = -1;

			//
			//  if  (  pTransformVideo->bQuit  )  return  -1;
			if (null==pCurPkt || null==pInDataCacheQ)
			{  //  2015/10/11
				return -1;
			}

			//
			if (IntPtr.Zero==pCurPkt->memory.m_pBuf)
			{
				if (isQEmpty(pInDataCacheQ))
				{
					//myPLAY_AUDIO_DATA pkts[CONST_pkts_decAudio];
					TMP_buf_myPLAY_AUDIO_DATA_pkts_encAudio buf_pkts;
					uint []uiLens=new uint[Consts.CONST_pkts_decAudio];      //  
					int nLens = uiLens.Length;// mycountof(uiLens);      //  一次最多处理的包数。

					int nPkts;
					int i;

					for (; ; )
					{
						//  if  (  pTransformVideo->bQuit  )  goto  errLabel;

						if (isQ2Empty(pAudioQ2))
						{
							goto errLabel;
						}

						nLens = uiLens.Length;// mycountof(uiLens);
						if (0!=q2GetMsgPkg(pAudioQ2, (IntPtr)buf_pkts.buf, Consts.bufSize_myPLAY_AUDIO_DATA_pkts_encAudio, uiLens,  (uint*  )&nLens  )  )  {
						showInfo_open(0, null, _T("getValidPkt failed: q2GetMsgPkg failed"));
						goto errLabel;
					}

						//
						myPLAY_AUDIO_DATA* pkts = (myPLAY_AUDIO_DATA*)buf_pkts.buf;

						//
						//
						nPkts = 0;
					for (i = 0; i < nLens; i++)
					{
						nPkts += (int)(uiLens[i] / sizeof(myPLAY_AUDIO_DATA));
						if (0!=(uiLens[i] % sizeof(myPLAY_AUDIO_DATA)))
						{
# if  __DEBUG__
							traceLog(_T("vpp: uiLen err"));
							assert(0);
#endif
						}
					}

					//
					for (i = 0; i < nPkts; i++)
					{
						if (0!=qPostMsg((IntPtr)(byte*)&pkts[i], (uint)sizeof(myPLAY_AUDIO_DATA), pInDataCacheQ))
						{
							clean_myPLAY_AUDIO_DATA((IntPtr)(byte*)&pkts[i], _T("decVideo_rtsp, qPostMsg failed"));
						}
					}

					//
					break;
				}
			}
			//
			uint size = (uint)sizeof(myPLAY_AUDIO_DATA);
			if (0!=qGetMsg(pInDataCacheQ, (IntPtr)pCurPkt, &size)) goto errLabel;
			//  2015/02/13
			M_set_pktId_getNewPkt(pCurPkt);
		}
	//
	if  (  IntPtr.Zero==pCurPkt->memory.m_pBuf  )  goto  errLabel;

	//
	myPLAY_AUDIO_DATA* pPkt = pCurPkt;


		//  if  (  pQdcObjInfo->cfg.pCurPkt
		iErr  =  0;
errLabel:

	return  iErr;
}



	//
	public static unsafe int getNextVPkt(IntPtr pCurPkt_param, GENERIC_Q pInDataCacheQ, QY_Q2 pVideoQ2)
		{
			int iErr = -1;

			myDRAW_VIDEO_DATA* pCurPkt = (myDRAW_VIDEO_DATA*)pCurPkt_param;


			//
			//  if  (  pTransformVideo->bQuit  )  return  -1;
			if (null==pCurPkt || null==pInDataCacheQ)
			{   //  2015/10/11
				return -1;
			}

			//
#if DEBUG
			ref myDRAW_VIDEO_DATA tmp_pPkt = ref *pCurPkt;
#endif

			//
			if (IntPtr.Zero==pCurPkt->memory.m_pBuf)
			{
				if (isQEmpty(pInDataCacheQ))
				{
					//myDRAW_VIDEO_DATA pkts[Consts.CONST_pkts_decVideo];
					TMP_buf_myDRAW_VIDEO_DATA_pkts_encVideo buf_pkts;
					//unsigned  int uiLens[Consts.CONST_pkts_encVideo];      //  
					uint []uiLens = new uint[Consts.CONST_pkts_encVideo];
					int nLens = uiLens.Length;// mycountof(uiLens);      //  一次最多处理的包数。

					int nPkts;
					int i;

					for (; ; )
					{
						//  if  (  pTransformVideo->bQuit  )  goto  errLabel;

						if (isQ2Empty(pVideoQ2))
						{
							goto errLabel;
						}

						nLens = uiLens.Length;// mycountof(uiLens);
						if (0!=q2GetMsgPkg(pVideoQ2, (IntPtr)buf_pkts.buf, Consts.bufSize_myDRAW_VIDEO_DATA_pkts_encVideo, uiLens,  (uint*  )&nLens  )  )  {
							showInfo_open(0, null, ("getNextVPkt failed: q2GetMsgPkg failed"));
							goto errLabel;
						}

						//
						//myDRAW_VIDEO_DATA* pkts = (myDRAW_VIDEO_DATA*)buf_pkts.buf;
						byte* pStart = buf_pkts.buf;
						int bufSize = Consts.bufSize_myDRAW_VIDEO_DATA_pkts_encVideo;

					//
					//
					nPkts = 0;
					for (i = 0; i < nLens; i++)
					{
						nPkts += (int)(uiLens[i] / sizeof(myDRAW_VIDEO_DATA));
						if (0!=(uiLens[i] % sizeof(myDRAW_VIDEO_DATA)))
						{
# if  __DEBUG__
							traceLog(_T("vpp: uiLen err"));
							assert(0);
#endif
						}
					}

					//  2015/10/16
# if  __DEBUG__

#endif

					//
					for (i = 0; i < nPkts; i++)
					{
							myDRAW_VIDEO_DATA* tmp_p = getMem((myDRAW_VIDEO_DATA*)pStart,bufSize, i);
						if (0!=qPostMsg((IntPtr)(byte*)tmp_p, (uint)sizeof(myDRAW_VIDEO_DATA), pInDataCacheQ))
						{
							clean_myDRAW_VIDEO_DATA((IntPtr)(byte*)tmp_p, ("decVideo_rtsp, qPostMsg failed"));
						}
					}

					//
					break;
				}
			}
			//
			uint size = (uint)sizeof(myDRAW_VIDEO_DATA);
			if (0!=qGetMsg(pInDataCacheQ, (IntPtr)pCurPkt, &size)) goto errLabel;
			//  
		}
	//
	if  (  IntPtr.Zero==pCurPkt->memory.m_pBuf  )  goto  errLabel;

	//
	myDRAW_VIDEO_DATA* pPkt = pCurPkt;


		//  if  (  pQdcObjInfo->cfg.pCurPkt
		iErr  =  0;
errLabel:

	return  iErr;
}


		//
		//
		public static unsafe int parseDecVPkt_o(IntPtr pPktParam, void** ppBih, byte** ppOutput, uint* puiSize)
		{
			int iErr = -1;
			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pPktParam;

			*ppBih = &pPkt->bih;
			*ppOutput = (byte*  )pPkt->memory.m_pBuf;
			*puiSize = pPkt->uiSize_dec;

			//
			iErr = 0;
		errLabel:
			return iErr;
		}



		public static unsafe int detachAPkt(void* pPkt_myPLAY_AUDIO_DATA, void* p1)
		{
			myPLAY_AUDIO_DATA* pPkt = (myPLAY_AUDIO_DATA*)pPkt_myPLAY_AUDIO_DATA;

			mymemset((IntPtr)(byte*)&pPkt->memory, 0, sizeof(QY_MEMORY)  );

			return 0;
		}


		//

		public static unsafe int detachVPkt(void* pPkt_myDRAW_VIDEO_DATA, void* p1)
		{
			myDRAW_VIDEO_DATA* pPkt = (myDRAW_VIDEO_DATA*)pPkt_myDRAW_VIDEO_DATA;

			mymemset((IntPtr)(byte*)&pPkt->memory, 0, sizeof(QY_MEMORY)  );

			return 0;
		}


	}
}
