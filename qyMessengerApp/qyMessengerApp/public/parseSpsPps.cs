using System;
using System.Collections.Generic;
using System.Text;

partial class Consts {
	//
	public const int CONST_h264nalType_idr = 5;
	public const int CONST_h264nalType_sps = 7;
	public const int CONST_h264nalType_pps = 8;

	//
	public const int CONST_h265naleType_CODED_SLICE_IDR = 19;       //  NAL_UNIT_CODED_SLICE_IDR, // 19 // Current name in the spec: IDR_W_DLP
	public const int CONST_h265nalType_CODED_SLICE_IDR_N_LP = 20;       //  NAL_UNIT_CODED_SLICE_IDR_N_LP, // 20
																		//
	public const int CONST_h265nalType_vps = 32;
	public const int CONST_h265nalType_sps = 33;
	public const int CONST_h265nalType_pps = 34;
};


namespace qy
{
    partial class qyFuncs
{
		//
		public static unsafe int parseSpsPps(int iFourcc, byte* spsPps, int spsPpsLen, byte* spsBuf, uint* puiSpsSize, byte* ppsBuf, uint* puiPpsSize, byte* vps, uint* puiVpsSize)
		{
			int tmpiRet = -1;
			//
			switch (iFourcc)
			{
				case Consts.CONST_fourcc_h264:
					tmpiRet = parseSpsPps_h264(spsPps, spsPpsLen, spsBuf, puiSpsSize, ppsBuf, puiPpsSize);
					if (null!=puiVpsSize) *puiVpsSize = 0;
					return tmpiRet;
					break;
				case Consts.CONST_fourcc_HEVC:
					return parseSpsPps_h265(spsPps, spsPpsLen, spsBuf, puiSpsSize, ppsBuf, puiPpsSize, vps, puiVpsSize);
					break;
				default:
# if  _DEBUG
					//assert(  0  );
					//
					OutputDebugString(_T("parseSpsPps warning, unprocessed iFourcc\n"));
#endif
					//
					//
					break;
			}

			//
			if (null!=puiVpsSize) *puiVpsSize = 0;
			if (null!=puiSpsSize) *puiSpsSize = 0;
			if (null!=puiPpsSize) *puiPpsSize = 0;

			return 0;
		}


		//
		public static unsafe int parseSpsPps1(int iFourcc, byte* spsPps, int spsPpsLen, VH_stream* pVh_stream)
		{
			int iErr = -1;
			//
			if (null==spsPps) return -1;
			if (null==pVh_stream) return -1;

			//
			mymemset((IntPtr)pVh_stream, 0, sizeof(VH_stream));

			//
			pVh_stream->dwFourcc = (uint)iFourcc;

			//
			uint vpsLen = 0;
			byte* pVps = null;
			uint spsLen = 0;
			byte* pSps = null;
			uint ppsLen = 0;
			byte* pPps = null;

			//
			switch (iFourcc)
			{
				case Consts.CONST_fourcc_HEVC:
					{
						//
						int k = 0;
						vpsLen = Consts.MAX_spsPpsLen + 1; pVps = pVh_stream->mem0.spsPps; 
						k++;
						spsLen = Consts.MAX_spsPpsLen+1; pSps = pVh_stream->mem1.spsPps; 
						k++;
						ppsLen = Consts.MAX_spsPpsLen+1; pPps = pVh_stream->mem2.spsPps;
						//								
						if (0!=parseSpsPps(iFourcc, spsPps, spsPpsLen, pSps, &spsLen, pPps, &ppsLen, pVps, &vpsLen)) goto errLabel;
						//							
						k = 0;
						pVh_stream->mem0.usLen = (ushort)vpsLen; 
						k++;
						pVh_stream->mem1.usLen = (ushort)spsLen; 
						k++;
						pVh_stream->mem2.usLen = (ushort)ppsLen; 
						k++;
						pVh_stream->usCnt = (ushort)k;
						//
					}
					break;
				case Consts.CONST_fourcc_h264:
				default:
					{
						int k = 0;
						spsLen = Consts.MAX_spsPpsLen+1; pSps = pVh_stream->mem0.spsPps; 
						k++;
						ppsLen = Consts.MAX_spsPpsLen+1; pPps = pVh_stream->mem1.spsPps;
						//				  
						if (0!=parseSpsPps(iFourcc, spsPps, spsPpsLen, pSps, &spsLen, pPps, &ppsLen, pVps, &vpsLen)) goto errLabel;
						//						
						pVh_stream->mem0.usLen = (ushort)spsLen;
						pVh_stream->mem1.usLen = (ushort)ppsLen;
						pVh_stream->usCnt = 2;

					}
					break;
			}


			iErr = 0;
		errLabel:


			return iErr;
		}

		

		//
		public static unsafe int parseSpsPps_h264(byte* spsPps, int spsPpsLen, byte* spsBuf, uint* puiSpsSize, byte* ppsBuf, uint* puiPpsSize)
		{
			int iErr = -1;
			//byte buf[2048];
			TMP_buf2048 buf;
			int bufSize = Consts.bufSize_2048;
			uint naluLen = 0;
			uint uiSpsLen = 0;
			uint uiPpsLen = 0;

			//
			if (null==spsPps) return -1;
			if (null==spsBuf || null==puiSpsSize || null==ppsBuf || null==puiPpsSize)
			{
				return -1;
			}

			//
			if (spsPpsLen > bufSize)
			{
				//  return  -1;
				//OutputDebugString(_T("parseSpsPps :inputLen > sizeof(  buf  )"));
			}
			//  int  len  =  spsPpsLen;
			int len = Math.Min(spsPpsLen, bufSize);
			if (0==len)
			{
				*puiSpsSize = 0;
				*puiPpsSize = 0;
				return 0;
			}
			mymemcpy((IntPtr)buf.buf, (IntPtr)spsPps, (uint)len);


			int ii;

			byte* pStart = buf.buf;
			bool bStart = false;
			bool bNextNalu = false;
			bool bStartCode_4 = false;
			for (ii = 0; ii < len - 3; ii++)
			{
				if (buf.buf[ii] == 0
					&& buf.buf[ii + 1] == 0
					&& buf.buf[ii + 2] == 0
					&& buf.buf[ii + 3] == 1)
				{
					if (!bStart)
					{
						bStart = true;
						pStart = buf.buf + ii;
						ii += 3;
						continue;
					}
					else
					{
						bNextNalu = true;
						bStartCode_4 = true;
					}
				}
				if (buf.buf[ii] == 0
					&& buf.buf[ii + 1] == 0
					&& buf.buf[ii + 2] == 1)
				{
					if (!bStart)
					{
						bStart = true;
						pStart = buf.buf + ii;
						ii += 2;
						continue;
					}
					else
					{
						bNextNalu = true;
						bStartCode_4 = false;
					}
				}
				//
				if (!bNextNalu) continue;
				//
				naluLen = (uint)(buf.buf + ii - pStart);
				if (naluLen > 3)
				{
					int nalType = M_nalType_h264(pStart);
					switch (nalType)
					{
						case 7:
							if (naluLen > *puiSpsSize) goto errLabel;
							mymemcpy((IntPtr)spsBuf, (IntPtr)pStart, naluLen);
							uiSpsLen = naluLen;
							break;
						case 8:
							if (naluLen > *puiPpsSize) goto errLabel;
							mymemcpy((IntPtr)ppsBuf, (IntPtr)pStart, naluLen);
							uiPpsLen = naluLen;
							break;
						default:
							break;
					}
				}
				//
				bNextNalu = false;
				bStart = true;
				pStart = buf.buf + ii;
				if (bStartCode_4) ii += 3;
				else ii += 2;
				continue;
			}

			//
			if (bStart)
			{
				naluLen = (uint)(buf.buf + len - pStart);
				if (naluLen > 3)
				{
					int nalType = M_nalType_h264(pStart);
					switch (nalType)
					{
						case 7:
							if (naluLen > *puiSpsSize) goto errLabel;
							mymemcpy((IntPtr)spsBuf, (IntPtr)pStart, naluLen);
							uiSpsLen = naluLen;
							break;
						case 8:
							if (naluLen > *puiPpsSize) goto errLabel;
							mymemcpy((IntPtr)ppsBuf, (IntPtr)pStart, naluLen);
							uiPpsLen = naluLen;
							break;
						default:
							break;
					}
				}
			}

			*puiSpsSize = uiSpsLen;
			*puiPpsSize = uiPpsLen;

			iErr = 0;
		errLabel:

			//
			return iErr;

		}


		//  2017/10/10
		public static unsafe int nalType_h265(byte* pBuf, int bufLen)
		{
			uint code = 0;
			//
			if (bufLen <= 3) return 0;
			int iType = 0;
			int i;

			for (i = 0; i < bufLen - 1; i++)
			{
				code = (code << 8) + pBuf[i];
				if ((code & 0xffffff00) == 0x100)
				{
					byte na12 = pBuf[i + 1];
					//
					int type = (int)((code & 0x7E) >> 1);
					//
					if (0!=(code & 0x81)) return 0;  //  forbidden and reserved zero bits
												//
					if (0!=(na12 & 0xf8)) return 0;  //  reserved zero

					//
					return type;
				}
			}


			return iType;
		}





		//
		//  2017/10/10
		public static unsafe int parseSpsPps_h265(byte* spsPps, int spsPpsLen, byte* spsBuf, uint* puiSpsSize, byte* ppsBuf, uint* puiPpsSize, byte* vpsBuf, uint* puiVpsSize)
		{
			int iErr = -1;
			//byte buf[2048];
			TMP_buf2048 buf;
			int bufSize = Consts.bufSize_2048;
			uint naluLen = 0;
			//
			uint uiVpsLen = 0;
			uint uiSpsLen = 0;
			uint uiPpsLen = 0;

			//
			if (null==spsPps) return -1;
			if (null==spsBuf || null==puiSpsSize || null==ppsBuf || null==puiPpsSize || null==vpsBuf || null==puiVpsSize)
			{
				return -1;
			}

			//
			if (spsPpsLen > bufSize)
			{
				//  return  -1;
				//OutputDebugString(_T("parseSpsPps :inputLen > sizeof(  buf  )"));
			}
			//  int  len  =  spsPpsLen;
			int len = Math.Min(spsPpsLen, bufSize);
			if (0==len)
			{
				*puiSpsSize = 0;
				*puiPpsSize = 0;
				return 0;
			}
			mymemcpy((IntPtr)buf.buf, (IntPtr)spsPps, (uint)len);


			int ii;

			byte* pStart = buf.buf;
			bool bStart = false;
			bool bNextNalu = false;
			bool bStartCode_4 = false;
			for (ii = 0; ii < len - 3; ii++)
			{
				if (buf.buf[ii] == 0
					&& buf.buf[ii + 1] == 0
					&& buf.buf[ii + 2] == 0
					&& buf.buf[ii + 3] == 1)
				{
					if (!bStart)
					{
						bStart = true;
						pStart = buf.buf + ii;
						ii += 3;
						continue;
					}
					else
					{
						bNextNalu = true;
						bStartCode_4 = true;
					}
				}
				if (buf.buf[ii] == 0
					&& buf.buf[ii + 1] == 0
					&& buf.buf[ii + 2] == 1)
				{
					if (!bStart)
					{
						bStart = true;
						pStart = buf.buf + ii;
						ii += 2;
						continue;
					}
					else
					{
						bNextNalu = true;
						bStartCode_4 = false;
					}
				}
				//
				if (!bNextNalu) continue;
				//
				naluLen = (uint)(buf.buf + ii - pStart);
				if (naluLen > 3)
				{
					int nalType = nalType_h265(pStart, (int)naluLen);
					switch (nalType)
					{
						case Consts.CONST_h265nalType_vps:
							if (naluLen > *puiVpsSize) goto errLabel;
							mymemcpy((IntPtr)vpsBuf, (IntPtr)pStart, naluLen);
							uiVpsLen = naluLen;
							break;
						case Consts.CONST_h265nalType_sps:
							if (naluLen > *puiSpsSize) goto errLabel;
							mymemcpy((IntPtr)spsBuf, (IntPtr)pStart, naluLen);
							uiSpsLen = naluLen;
							break;
						case Consts.CONST_h265nalType_pps:
							if (naluLen > *puiPpsSize) goto errLabel;
							mymemcpy((IntPtr)ppsBuf, (IntPtr)pStart, naluLen);
							uiPpsLen = naluLen;
							break;
						default:
							break;
					}
				}
				//
				bNextNalu = false;
				bStart = true;
				pStart = buf.buf + ii;
				if (bStartCode_4) ii += 3;
				else ii += 2;
				continue;
			}

			//
			if (bStart)
			{
				naluLen = (uint)(buf.buf + len - pStart);
				if (naluLen > 3)
				{
					int nalType = nalType_h265(pStart, (int)naluLen);
					switch (nalType)
					{
						case Consts.CONST_h265nalType_vps:
							if (naluLen > *puiVpsSize) goto errLabel;
							mymemcpy((IntPtr)vpsBuf, (IntPtr)pStart, naluLen);
							uiVpsLen = naluLen;
							break;
						case Consts.CONST_h265nalType_sps:
							if (naluLen > *puiSpsSize) goto errLabel;
							mymemcpy((IntPtr)spsBuf, (IntPtr)pStart, naluLen);
							uiSpsLen = naluLen;
							break;
						case Consts.CONST_h265nalType_pps:
							if (naluLen > *puiPpsSize) goto errLabel;
							mymemcpy((IntPtr)ppsBuf, (IntPtr)pStart, naluLen);
							uiPpsLen = naluLen;
							break;
						default:
							break;
					}
				}
			}

			//
			*puiVpsSize = uiVpsLen;
			*puiSpsSize = uiSpsLen;
			*puiPpsSize = uiPpsLen;

			iErr = 0;
		errLabel:

			//
			return iErr;

		}






		//
		public static unsafe int makeSpsPps(VH_stream* pVh_stream, byte* spsPpsParam, uint* puiSize)
		{
			int iErr = -1;
			//byte spsPps[512] = "";
			TMP_buf1024 spsPps;
			int bufSize_spsPps = Consts.bufSize_1024;
			int spsLen = 0;
			int i;

			if (null==pVh_stream || null==spsPpsParam || null==puiSize) return -1;

			if (0!=pVh_stream->usCnt)
			{
				if (pVh_stream->usCnt > Consts.cntof_VH_stream_mems) goto errLabel;
				//
				for (i = 0; i < pVh_stream->usCnt; i++)
				{
					VH_stream_mem* pMem = null;
					switch ( i )
                    {
						case 0:
							pMem = &pVh_stream->mem0;
							break;
						case 1:
							pMem = &pVh_stream->mem1;
							break;
						case 2:
							pMem = &pVh_stream->mem2;
							break;
						default:
							goto errLabel;
							 
                    }
					if (spsLen + pMem->usLen > bufSize_spsPps)
					{
						//qyShowInfo1(  CONST_qyShowType_qwmComm,  NULL,  (  ""  ),  _T(  "IsClient"  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "htonAvTranInfo failed: spsLen too small"  )  );
						goto errLabel;
					}
					mymemcpy((IntPtr)(spsPps.buf + spsLen), (IntPtr)pMem->spsPps, pMem->usLen);
					spsLen += pMem->usLen;
				}
			}
			if (*puiSize < spsLen) goto errLabel;
			mymemcpy((IntPtr)spsPpsParam, (IntPtr)spsPps.buf, (uint)spsLen);
			*puiSize = (uint)spsLen;

			iErr = 0;
		errLabel:

			if (0==iErr)
			{

			}

			return iErr;

		}
	}
}
