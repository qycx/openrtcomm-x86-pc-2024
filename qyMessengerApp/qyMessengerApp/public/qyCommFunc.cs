using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

partial class Consts
{
    public const int CONST_maxReqDataLen_enc_netMc = 16384;         //  2004/07/26, 
    public const int CONST_maxReqDataLen_enc_is = 1024 * 1024;	//  2008/03/20, 
    //
    public const int CONST_qnmReqBufSize_netMc = 32768;			//  netMc


}

namespace qy
{
    partial class qyFuncs
{
		//
		public static unsafe int qyTellService(uint serviceId, byte* version, void* pCommEncCtx, byte* data, int dataLen, ref QY_SOCK pSock, ref SOCK_TIMEOUT pTo, ref QY_COMM_SESSION pSession)
		{
			int iErr = -1;
			QY_COMM_SERVICE service;
			uint len;

			//  traceLogA(  "qyTellService(  ) enters."  );

			//  清空pSession->service
			fixed (QY_COMM_SERVICE* p = &pSession.service) {
				mymemset((IntPtr)(byte*)p, 0, sizeof(QY_COMM_SERVICE));
			}

			//  重新组织service
			mymemset((IntPtr)(byte*)&service, ' ', sizeof(QY_COMM_SERVICE));
			service.serviceId = qyhtoni(serviceId);
			MACRO_safeStrMemCpy(mytoString(version), service.version, Consts.CONST_qyMaxVerLen);
			//if (pCommEncCtx) service.encType = qyhtonl(((QY_ENC_CTX*)pCommEncCtx)->type);
			mymemcpy((IntPtr)service.data, (IntPtr)data, (uint)Math.Min(dataLen, Consts.bufSize_qyCommServiceData));

			len = (uint)(Marshal.OffsetOf<QY_COMM_SERVICE>("data") + Math.Min(dataLen, Consts.bufSize_qyCommServiceData)); //  
			if (0!=qySendReq(ref pSession, ref pSock, ref pTo, Consts.CONST_qyCmd_tellService, (byte*)&service, len)) goto errLabel;

			//  将service赋入pSession中
			service.serviceId = serviceId;          //
#if false
			if (pCommEncCtx)
			{
				service.encType = ((QY_ENC_CTX*)pCommEncCtx)->type;
			}
#endif
			//mymemcpy((IntPtr)(byte*)&pSession.service, &service, sizeof(service));
			pSession.service=service;
#if false
			if (pCommEncCtx)
			{
				memcpy(&pSession->commEncCtx, pCommEncCtx, sizeof(pSession->commEncCtx)  ); //  2004/01/03添加对加密的支持
			}
#endif

			iErr = 0;

		errLabel:

			//  traceLogA(  "qyTellService(  ) leaves, %s",  iErr  ?  "失败"  :  "成功"  );

			return iErr;

		}



		public static unsafe int tmpqySendReq(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, ref SOCK_TIMEOUT pTo, bool bResp, ushort cmdCode, byte* data, uint dataLen)
		{
			int iErr = -1;
			QY_COMM_HEAD tmpHead;
			byte* encBuf = null;
			uint encLen = Consts.CONST_maxReqDataLen_enc_netMc;

			mymemset((IntPtr)(byte*)&tmpHead, 0, sizeof(QY_COMM_HEAD));
			tmpHead.usCode = qyhtons(cmdCode);
			if (null==data) dataLen = 0;

			if (0!=dataLen)
			{       //  2004/01/02里加入了加密的处理
#if false
				if (pSession->service.encType == CONST_qyEncType_qwm)
				{

					if (!(encBuf = (char*)malloc(encLen))) goto errLabel;

					if (qnmEncData(&pSession->commEncCtx, data, dataLen, encBuf, &encLen))
					{
						traceLogA("qySendReq里qnmEncData failed."); goto errLabel;
					}

					//  {
					//		 char  buf1[8192];  unsigned  int  len1  =  sizeof(  buf1  );
					//		 qnmDecData(  &pSession->commEncCtx,  encBuf,  encLen,  buf1,  &len1  );
					//	 }


					data = encBuf; dataLen = encLen;
				}
#endif
			}

			tmpHead.len = qyhtoni(dataLen);
			if (0!=GuiShare.pf_qySend(ref pSock, (byte*)&tmpHead, sizeof(QY_COMM_HEAD), pTo.iWrite)) goto errLabel;
			if (0!=dataLen)
			{
				if (0!=GuiShare.pf_qySend(ref pSock, data, (int)dataLen, pTo.iWrite)) goto errLabel;
			}

			//  2007/04/25
			if (!bResp) pSession.usLastReqCode_o = cmdCode;
			else pSession.usLastRespCode_o = cmdCode;

			iErr = 0;

		errLabel:

			if (null!=encBuf) myfree((IntPtr)encBuf);

			return iErr;

		}


		public static unsafe int qySendReq(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, ref SOCK_TIMEOUT pTo, ushort cmdCode, byte* data, uint dataLen)
		{
			return tmpqySendReq(ref pSession, ref pSock, ref pTo, false, cmdCode, data, dataLen);
		}

	}
}
