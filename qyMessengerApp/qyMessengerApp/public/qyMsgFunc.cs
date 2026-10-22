using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;
using System.Collections;

namespace qy
{
 
		public unsafe struct QY_MSG_HEAD
		{
			public ushort usCode;              // 
			public byte ucFlg;                //  2007/04/22
			public fixed byte reserved[1];       //  
			public uint uiLen;             //  
			public Int64 i64StartTime;       //  2007/05/07, 
			public uint uiTranNo;          //  2007/05/02
			public ushort usSeqNo;         //  2007/05/02
			public fixed byte reserved1[2];      //  				 
		}
		;

	partial class qyFuncs
	{
		public static unsafe void MACRO_prepareForTran(out Int64 tStartTran, out uint uiTranNo)
		{
			int tt = 0;
			long l=0;
			tStartTran=mytime(ref l); uiTranNo = getuiNextTranNo(null, 0, ref tt);
			return;
		}



		public static unsafe int qyRecvMsg(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, SOCK_TIMEOUT pTo, QY_MSG_HEAD* pMsgHead, byte* dataBuf, uint bufSize)
		{
			int iErr = -1;
			byte* encBuf = null;
			uint encLen = Consts.CONST_maxReqDataLen_enc_is;

			if (qyRecvEx(ref pSock, (byte*)pMsgHead, sizeof(QY_MSG_HEAD), pTo.iRead) != 0) goto errLabel;
			pMsgHead->uiLen = qyntohi(pMsgHead->uiLen);
			pMsgHead->usCode=qyntohs(pMsgHead->usCode);
			pMsgHead->i64StartTime=(long)qyntohl64((ulong)pMsgHead->i64StartTime);  //  2007/05/07
			pMsgHead->uiTranNo=qyntohi(pMsgHead->uiTranNo);
			pMsgHead->usSeqNo=qyntohs(pMsgHead->usSeqNo);

			if (pMsgHead->uiLen!=0)
			{

				if (pSession.service.encType == Consts.CONST_qyEncType_qwm)
				{
					/*

					if (!(encBuf = (char*)mymalloc(encLen))) goto errLabel;

					if (pMsgHead->uiLen > encLen)
					{
						traceLogA("qyRecvMsg err, dataLen [%d] > encLen [%d]", pMsgHead->uiLen, encLen);
						goto errLabel;
					}

					if (qyRecvEx(pSock, encBuf, pMsgHead->uiLen, pTo->iRead)) goto errLabel;

					if (qnmDecData(&pSession->commEncCtx, encBuf, pMsgHead->uiLen, dataBuf, &bufSize))
					{
						traceLogA("qyRecvReq里qnmDecData failed");
						goto errLabel;
					}
					pMsgHead->uiLen = bufSize;
					*/
					Console.WriteLine("qyRecvMsg failed, enc_qwm not supported");
					goto errLabel;
				}
				else
				{

					if (pMsgHead->uiLen > bufSize)
					{
						//traceLogA("qyRecvMsg err, dataLen [%d] > bufSize [%d]", pMsgHead->uiLen, bufSize);
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", _T("IsClient"), _T(""), _T(""), _T("qyRecvMsg err. "), _T("收到数据过大, dataLen [%d] "), pMsgHead->uiLen);
						iErr = -2; goto errLabel;
					}

					if (qyRecvEx(ref pSock, dataBuf, (int)pMsgHead->uiLen, pTo.iRead)!=0) goto errLabel;
				}
			}

			//
			if (isUcFlgResp(pMsgHead->ucFlg)) pSession.usLastRespCode_i = pMsgHead->usCode;
			else pSession.usLastReqCode_i = pMsgHead->usCode;

			iErr = 0;

		errLabel:

			if (encBuf!=null) myfree((IntPtr)encBuf);

			return iErr;

		}

		public static unsafe  int qySendMsg(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, SOCK_TIMEOUT pTo, byte ucFlg, ushort cmdCode, long lStartTime, uint uiTranNo, ushort usSeqNo, byte* data, uint dataLen)
		{
			int iErr = -1;
			QY_MSG_HEAD tmpHead;
			byte* encBuf = null;
			uint encLen = Consts.CONST_maxReqDataLen_enc_is;

			byte* p = (byte * )&tmpHead;
			mymemset((IntPtr)p, 0, sizeof(QY_MSG_HEAD));
			tmpHead.usCode = qyhtons(cmdCode);
			tmpHead.ucFlg = ucFlg;

			if (null==data) dataLen = 0;

			if (dataLen!=0)
			{       //  2004/01/02里加入了加密的处理
			/*
				if (pSession->service.encType == Consts.CONST_qyEncType_qwm)
				{

					if (!(encBuf = (char*)mymalloc(encLen))) goto errLabel;

					if (qnmEncData(&pSession->commEncCtx, data, dataLen, encBuf, &encLen))
					{
						traceLogA("qySendReq里qnmEncData failed.");
						goto errLabel;
					}

					//  {
					//		 char  buf1[8192];  unsigned  int  len1  =  sizeof(  buf1  );
					//		 qnmDecData(  &pSession->commEncCtx,  encBuf,  encLen,  buf1,  &len1  );
					//	 }


					data = encBuf; dataLen = encLen;
				}
			*/
			}

			tmpHead.uiLen = (uint  )dataLen;
			if (tmpHead.uiLen != dataLen)
			{
				//traceLogA("dataLen");
				goto errLabel;      //  这里要防止dataLen是个过大的数
			}
			tmpHead.uiLen = qyhtoni(  tmpHead.uiLen);
			tmpHead.i64StartTime = (long)qyhtonl64((ulong)lStartTime);  //  2007/05/07
			tmpHead.uiTranNo = qyhtoni(uiTranNo);
			tmpHead.usSeqNo = qyhtons(usSeqNo);         //  2007/05/02
			if (GuiShare.pf_qySend(ref pSock, (byte*)&tmpHead, sizeof(QY_MSG_HEAD), pTo.iWrite)!=0)
			{
				//traceLogA("qySend msgHead failed"); 
				goto errLabel;
			}
			if (dataLen!=0)
			{
				if (GuiShare.pf_qySend(ref pSock, data, (int)dataLen, pTo.iWrite)!=0)
				{
					//qyDisplayLastError("qySend data failed"); 
					goto errLabel;
				}
			}

			iErr = 0;

		errLabel:

			if (encBuf!=null) myfree((IntPtr)encBuf);

			return iErr;

		}


		//  2008/06/09, usCmd固定使用CONST_qyCmd_tellService。
		//  extern  "C"  int  qyTellService_msg(  unsigned  int  serviceId,  char * version,  unsigned  short  usCmd,  void  *  pCommEncCtx,  char * data,  int dataLen,  QY_SOCK * pSock,  SOCK_TIMEOUT * pTo,  QY_COMM_SESSION * pSession )
		public static unsafe int qyTellService_msg(uint serviceId, string version, QY_ENC_CTX pCommEncCtx, ushort usCmd, long tStartTime, uint uiTranNo, byte* data, int dataLen, QY_SOCK pSock, SOCK_TIMEOUT pTo, QY_COMM_SESSION pSession)
		{
			int iErr = -1;
			QY_COMM_SERVICE service;
			int len;
			//  unsigned  int			uiStartTime		=		0;
			//  unsigned  int			uiTranNo		=		0;		//

			//  traceLogA(  "qyTellService(  ) enters."  );

			if (null==data || dataLen <= 0) return -1;

			//  清空pSession->service
			//mymemset(&pSession->service, 0, sizeof(pSession->service)  );

			//  重新组织service
			//memset(&service, ' ', sizeof(service));
			service = new QY_COMM_SERVICE();
			service.serviceId = serviceId;
			MACRO_safeStrMemCpy(version, service.version, Consts.CONST_qyMaxVerLen);
			//if (pCommEncCtx)
			{
				service.encType = (pCommEncCtx).common.type;
			}
			//else service.encType = 0;

			switch (service.encType)
			{
				case Consts.CONST_qyEncType_rsa:
                    /*
					{   //  
						ref ENC_CTX_session pEncCtx = ref pCommEncCtx;

						//  这里应该用hPublicKey加密。
						if (!pEncCtx || !pEncCtx->key_rsa.hKey) goto errLabel;
						DWORD dwBlockLen = pEncCtx->key_rsa.dwBlockLen_encrypt;
						int nIn = 0;
						int nOut = 0;
						int bufLen = 0;

						if (!dwBlockLen) goto errLabel;

						for (nIn = 0, nOut = 0; nIn < (int)dataLen && nOut <= sizeof(service.data);)
						{
							len = min(dataLen - nIn, (int)dwBlockLen);

							memcpy(service.data + nOut, data + nIn, len);
							bufLen = len;

							if (!CryptEncrypt(pEncCtx->key_rsa.hKey, 0, (len >= (int)(dataLen - nIn)), 0, (BYTE*)(service.data + nOut), (DWORD*)&bufLen, sizeof(service.data) - nOut))
							{
								//qyDisplayLastError("qyTellService_msg CryptEncrypt failed.");
								goto errLabel;
							}

							nIn += len;
							nOut += bufLen;
						}

						dataLen = nOut;
					}
					*/
                    //
                    {
						ENC_CTX_session pEncCtx = (ENC_CTX_session)pCommEncCtx;

						//  这里应该用hPublicKey加密。
						if (null==pEncCtx 
							//|| !pEncCtx.key_rsa.hKey
							) goto errLabel;
						//
						uint dwBlockLen = pEncCtx.m_rsa_dwBlockLen_encrypt;
						int nIn = 0;
						int nOut = 0;
						//int bufLen = 0;
						//
						if (0==dwBlockLen) goto errLabel;

						//
						byte[] tmp_data = new byte[dwBlockLen];
						for (nIn = 0, nOut = 0; nIn < (int)dataLen && nOut <= Consts.bufSize_qyCommServiceData;)
						{
							len = Math.Min(dataLen - nIn, (int)dwBlockLen);
							System.Runtime.InteropServices.Marshal.Copy((IntPtr)(data + nIn), tmp_data, 0, len);
							byte[] encData = pEncCtx.m_rsaProvider.Encrypt(tmp_data, false);
							//
							Array.Reverse(encData);
							//
							if (encData.Length > Consts.bufSize_qyCommServiceData - nOut) goto errLabel;
							System.Runtime.InteropServices.Marshal.Copy(encData, 0, (IntPtr)(service.data  +  nOut), encData.Length);
							//
							nIn += len;
							nOut += encData.Length;
						}
						dataLen = nOut;
					}
					//
					break;
				default:
					dataLen = Math.Min(dataLen, Consts.bufSize_qyCommServiceData);
					mymemcpy((IntPtr)service.data, (IntPtr)data, (uint)dataLen);
					break;
			}

			service.serviceId=qyhtoni(service.serviceId);
			service.encType=qyhtoni(service.encType);

			//int tmp = (int)System.Runtime.InteropServices.Marshal.OffsetOf<QY_COMM_SERVICE>("data");
			len = (int)System.Runtime.InteropServices.Marshal.OffsetOf<QY_COMM_SERVICE>("data") + dataLen;        //  2007/04/19, 原来发送的len为sizeof(  service  ), 现在做了修改。主要为了让发送的数据能灵活控制
			if (qySendMsg(ref pSession, ref pSock, pTo, 0, usCmd, tStartTime, uiTranNo, 0, (byte*)&service, (uint)len)!=0) goto errLabel;

			//  将service赋入pSession中
			service.serviceId = serviceId;          //  
			service.encType = pCommEncCtx.common.type;
			//memcpy(&pSession->service, &service, sizeof(service));
			pSession.service = service;
			//if (pCommEncCtx) memcpy(&pSession->commEncCtx, pCommEncCtx, sizeof(pSession->commEncCtx)  );    //  2004/01/03添加对加密的支持
			pSession.commEncCtx = pCommEncCtx;

			iErr = 0;

		errLabel:

			//  traceLogA(  "qyTellService(  ) leaves, %s",  iErr  ?  "失败"  :  "成功"  );

			return iErr;

		}



		public static unsafe void qnmCloseSession_msg(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, SOCK_TIMEOUT pTo)
		{
			QY_MSG_HEAD tmpResp;

			//traceLogA("qnmCloseSession_msg enters");

			if (GuiShare.pf_bQySockOpen(ref pSock))
			{
				if (isRcOk(pSession.usLastRespCode_i) || pSession.usLastRespCode_i == Consts.CONST_qyRc_user)
				{
					if (0==qySendMsg(ref pSession, ref pSock, pTo, 0, Consts.CONST_qyCmd_end, 0, 0, 0, null, 0))
					{
						qyRecvMsg(ref pSession, ref pSock, pTo, &tmpResp, null, 0);
					}
				}
				//
				pSock.buf_r = null;
				pSock.buf_w = null;
				//
				GuiShare.pf_qyDisconnect(ref pSock);
			}

			//traceLogA("qnmCloseSession_msg leaves");

			return;
		}


	}
}
