using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IO;


partial class Consts
{
    public const string CONST_cspContainer_qmd = ("QmdKeyContainer");           //  name of the key container to be used

    public const string CONST_cspContainer_qmc = ("QmcKeyContainer");           //  name of the key container to be used
    public const string CONST_mutexName_syncKey_qmc = ("QmcSyncKey");

    //
    //#define MS_ENHANCED_PROV_W      L"Microsoft Enhanced Cryptographic Provider v1.0"
    //#define MS_ENHANCED_PROV        MS_ENHANCED_PROV_W

    //#define DEFAULT_cspProvider									MS_ENHANCED_PROV
    public const string DEFAULT_cspProvider = "Microsoft Enhanced Cryptographic Provider v1.0";

    public const int CONST_encryptBlockSize = 8;
}



namespace qy
{

    public unsafe class ENC_CTX_session : QY_ENC_CTX
    {
        //public QY_ENC_CTX common;

        /*
		//
		HCRYPTPROV hCryptProv;                                      // handle for a cryptographic

		struct                                         {

					 HCRYPTKEY hKey;                                        //  在messenger, 表示公钥. //  在daemon, 表示private/public pairs
			DWORD dwBlockLen_encrypt;                           //  加密的blockLen, 密钥长度的字节数 - 11
			DWORD dwBlockLen_decrypt;                           //  解密的blockLen, 密钥长度的字节数
		} key_rsa;

		struct                                         {
				 	 HCRYPTKEY hKey;
			DWORD dwBlockLen_encrypt;                           //  加密的blockLen 
			DWORD dwBlockLen_decrypt;                           //  解密的blockLen
		}  key_session;
		*/

        //
        public RSACryptoServiceProvider m_rsaProvider;
        public uint m_rsa_dwBlockLen_encrypt;

        //	
        public Aes m_aes;
        //
        public byte[] m_plainBuf_encrypt;
        //
        public byte[] m_cipherData_decrypt;
        public byte[] m_buffer_decrypt;

        //
        public struct ENC_CTX_session_var
        {
            //public uint uiBlobLen_sessionKey;
            //public fixed byte sessionKeyBlob[Consts.bufSize_sessionKeyBlob];



            public fixed char mutexName_syncCrypt[Consts.cntof_mutexName];                  //  2008/10/12, 对每个会话密钥，要保证线程同步。微软的api不支持在2个线程里同时用一个密钥
        };
        public ENC_CTX_session_var m_var;

    };

    partial class qyFuncs
    {
        //public static unsafe	int  encryptData(  ref ENC_CTX_session    pEncCtx,  ref QY_BUF  pBuf_help, ref MemoryStream pmemoryStream,  byte  *  data,  uint  *  puiDataLen,  uint  uiBufLen  )
        public static unsafe int encryptData(ref ENC_CTX_session pEncCtx, ref QY_BUF pBuf_help, byte[] encBuf, ref RecyclableMemoryStreamManager p_recyclableMemoryStreamManager, byte* data, uint* puiDataLen, uint uiBufLen)
        {
            int iErr = -1;
            uint uiDataLen = 0;

            //		
            using (CQySyncObj syncObj = new CQySyncObj())
            {

                if (null == data || null == puiDataLen) return -1;
                uiDataLen = *puiDataLen;

                //  2015/08/28
                if (pEncCtx.common.type == Consts.CONST_qyEncType_qwm)
                {

                    /*

			//
			char  *  pBuf  =  data;
			int  len  =  uiDataLen;
			unsigned  int  uiBufSize_pBuf  =  uiBufLen;

			CQyMalloc				mallocObj_encBuf;		
			unsigned  int			encLen			=	CONST_maxReqDataLen_enc_is;												
			char				*	encBuf			=	mallocObj_encBuf.mallocf(  encLen  );													
			if  (  !encBuf  )  goto  errLabel;


			char  *  data  =  pBuf;		
			int  dataLen  =  len;


			//												    				
			if  (  qnmEncData(  pEncCtx,  data,  dataLen,  encBuf,  &encLen  )  )  {	
				traceLogA(  "qySendReqÀïqnmEncData failed."  );  
				goto  errLabel;				 													
			}

			//		
			if  (  encLen  >  uiBufSize_pBuf  )  {			
				goto  errLabel;											
			}

			memcpy(  pBuf,  encBuf,  encLen  );		
			len  =  encLen;
			//
			uiDataLen  =  len;

			#ifdef  __DEBUG__
					#if  0
						char  output[1024];
						unsigned  int  outputLen  =  sizeof(  output  );
						qnmDecData(  &pSession->commEncCtx,  pBuf,  len,  output,  &outputLen  );
					#endif		
			#endif

			//
			iErr  =  0;  goto  errLabel;													
					*/
                    Console.WriteLine("not supported");
                    goto errLabel;

                }


                //
                if (0 == pEncCtx.common.type
        || 0 == uiDataLen
            //||  !pEncCtx.key_session.hKey  
            )
                {                   //  没有加密，所以不需要解密
                    iErr = 0; goto errLabel;
                }


                //  2015/08/28
                if (pEncCtx.common.type != Consts.CONST_qyEncType_rsa)
                {
                    goto errLabel;
                }


#if false
	//
	if  (  pEncCtx->mutexName_syncCrypt  &&  pEncCtx->mutexName_syncCrypt[0]  )  {
        if  (  syncObj.sync(  pEncCtx->mutexName_syncCrypt  )  )  goto  errLabel;
	}
#endif

#if false
				/*
				// Determine the number of bytes to encrypt at a time. 
				// This must be a multiple of ENCRYPT_BLOCK_SIZE.
				// ENCRYPT_BLOCK_SIZE is set by a #define statement.

				dwBlockLen = 1000 - 1000 % ENCRYPT_BLOCK_SIZE; 

				//-------------------------------------------------------------------
				// Determine the block size. If a block cipher is used, 
				// it must have room for an extra block. 

				if(ENCRYPT_BLOCK_SIZE > 1) 
					dwBufferLen = dwBlockLen + ENCRYPT_BLOCK_SIZE; 
				else 
					dwBufferLen = dwBlockLen; 
				*/

				if (!pEncCtx->key_session.dwBlockLen_encrypt) { //  流式加密的情况


					try {
						if (!CryptEncrypt(pEncCtx->key_session.hKey, 0, TRUE, 0, (BYTE*)data, (DWORD*)&uiDataLen, uiBufLen)) {
							traceLogA("encryptData: CryptEncrypt failed.");
							goto errLabel;
						}
					}
					catch (  ...  )  {
						traceLogA("encryptData: CryptEncrypt failed. exception caught.");
						goto errLabel;
					}

					iErr = 0; goto errLabel;
					}

					//  块式加密的情况
					//traceLogA(  "encryptData: 暂时使用流式加密, 这里不应该有. 未完成"  );  
					goto errLabel;

				
#endif
                try
                {
                    if (pEncCtx.m_plainBuf_encrypt.Length < uiDataLen)
                    {
                        pEncCtx.m_plainBuf_encrypt = new byte[uiDataLen];
                        showInfo_open(0, null, "encryptData> new plainBuf");
                    }
                    byte[] plainData = pEncCtx.m_plainBuf_encrypt;// new byte[uiDataLen];
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)data, plainData, 0, (int)uiDataLen);
                    //using (MemoryStream memoryStream = new MemoryStream())
                    using (RecyclableMemoryStream memoryStream = p_recyclableMemoryStreamManager.GetStream())
                    {
                        using (ICryptoTransform encryptor = pEncCtx.m_aes.CreateEncryptor())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainData, 0, (int)uiDataLen);
                                cryptoStream.FlushFinalBlock();

                                /*
								byte[] cipherData = memoryStream.ToArray();
								if (cipherData.Length > uiBufLen) goto errLabel;
								System.Runtime.InteropServices.Marshal.Copy(cipherData, 0, (IntPtr)data, cipherData.Length);
								uiDataLen = (uint)cipherData.Length;
								*/
                                //int maxEncBufLen = 1000000;
                                //byte[] cipherData = new byte[maxEncBufLen];
                                //
                                int maxEncBufLen = encBuf.Length;
                                byte[] cipherData = encBuf;
                                //
                                memoryStream.Seek(0, 0);
                                int cipherLen = memoryStream.Read(cipherData, 0, maxEncBufLen);
                                if (cipherLen < 0 || cipherLen >= maxEncBufLen || cipherLen > uiBufLen)
                                {
                                    goto errLabel;
                                }
                                System.Runtime.InteropServices.Marshal.Copy(cipherData, 0, (IntPtr)data, cipherLen);
                                uiDataLen = (uint)cipherLen;

                            }
                        }


                    }
                }
                catch (Exception e)
                {
                    showInfo_open(0, null, "encryptData exception");
                    goto errLabel;
                }



            }
            iErr = 0;

        errLabel:
            if (0 == iErr)
            {
                *puiDataLen = uiDataLen;
            }
            return iErr;
        }

        public static unsafe int decryptData(ref ENC_CTX_session pEncCtx, ref QY_BUF pBuf_help, ref RecyclableMemoryStreamManager p_recyclableMemoryStreamManager,  byte* data, uint* puiDataLen)
        {
            int iErr = -1;
            uint uiDataLen = 0;

            using (CQySyncObj syncObj = new CQySyncObj())
            {

                if (null == data || null == puiDataLen) return -1;
                uiDataLen = *puiDataLen;

                //
                if (pEncCtx.common.type == Consts.CONST_qyEncType_qwm)
                {
#if false
				unsigned  int		encLen		=	uiDataLen;
		CQyMalloc			mallocObj_encBuf;
		char			*	encBuf		=	mallocObj_encBuf.mallocf(  encLen  );
		//
		memcpy(  encBuf,  data,  uiDataLen  );
		//
		if  (  qnmDecData(  pEncCtx,  encBuf,  encLen,  data,  &uiDataLen  )  )  {
			traceLogA(  "qyRecvReq里qnmDecData failed"  );  
			goto  errLabel;			 
		}
		//
		iErr  =  0;  goto  errLabel;
#endif
                    Console.WriteLine("not supported");
                    goto errLabel;
                }

                //
                if (0 == pEncCtx.common.type
        || 0 == uiDataLen
            || null == pEncCtx.m_aes)
                {
                    iErr = 0; goto errLabel;
                }

#if false

	//
	if  (  pEncCtx->mutexName_syncCrypt  &&  pEncCtx->mutexName_syncCrypt[0]  )  {
        if  (  syncObj.sync(  pEncCtx->mutexName_syncCrypt  )  )  goto  errLabel;
	}
#endif


#if false

				if (0==pEncCtx->key_session.dwBlockLen_decrypt) {


					/*
					dwBlockLen  =  1000;  

					for  (  nIn  =  0, nOut  =  0;  nIn  <  (  int  )uiDataLen  &&  nOut  <=  (  int  )uiDataLen;  )  {
						 len  =  min(  (  int  )dwBlockLen,  uiDataLen  -  nIn  );	
						 tmpLen  =  len;

						 if  (  !CryptDecrypt(  pEncCtx->key_session.hKey,  0,  (  len  >=  (  int  )(  uiDataLen  -  nIn  )  ),  0,  (  BYTE  *  )data  +  nOut,  (  DWORD  *  )&tmpLen  )  )  goto  errLabel;
						 if  (  tmpLen  !=  len  )  {
							 qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  _T(  "IsClient"  ),  _T(  ""  ),  _T(  ""  ),  _T(  "decryptData err. "  ),  _T(  "流式解密：输出数据和输入长度不同"  )  );
							 goto  errLabel;				 
						 }

						 nIn  +=  len;
						 nOut  +=  tmpLen;
					}

					uiDataLen  =  nOut;
					*/

					try {
						if (!CryptDecrypt(pEncCtx->key_session.hKey, 0, TRUE, 0, (BYTE*)data, (DWORD*)&uiDataLen)) {
							traceLogA("decryptData: CryptDecrypt failed.");
							goto errLabel;
						}
					}
					catch (  ...  )  {
						traceLogA("decryptData: CryptDecrypt failed. exception caught.");
						goto errLabel;
					}

				iErr = 0; goto errLabel;
					}
#endif

                //
                if (pEncCtx.m_cipherData_decrypt.Length < uiDataLen)
                {
                    pEncCtx.m_cipherData_decrypt = new byte[uiDataLen];
                    showInfo_open(0, null, "decryptData: new cipherData");
                }
                byte[] cipherData = pEncCtx.m_cipherData_decrypt;// new byte[uiDataLen];
                System.Runtime.InteropServices.Marshal.Copy((IntPtr)data, cipherData, 0, (int)uiDataLen);

                //  块式解密
                string plaintext;

#if true//false
                try
                {
                    //  using (MemoryStream memoryStream = new MemoryStream(cipherData, 0, (int)uiDataLen))
                    //  public RecyclableMemoryStream GetStream(Guid id, string? tag, byte[] buffer, int offset, int count)

                    //   using (MemoryStream memoryStream = new MemoryStream(cipherData, 0, (int)uiDataLen))
                    using (RecyclableMemoryStream memoryStream = p_recyclableMemoryStreamManager.GetStream(Guid.Empty, "", cipherData, 0, (int)uiDataLen))
                    {
                        using (ICryptoTransform decryptor = pEncCtx.m_aes.CreateDecryptor())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                Byte[] Buffer = pEncCtx.m_buffer_decrypt;// new Byte[1024];
                                int nOut = 0;
                                Int32 readBytes = 0;
                                while ((readBytes = cryptoStream.Read(Buffer, 0, Buffer.Length)) > 0)
                                {
                                    //int ii = 0;
                                    if (readBytes + nOut > uiDataLen) goto errLabel;
                                    System.Runtime.InteropServices.Marshal.Copy(Buffer, 0, (IntPtr)(data + nOut), readBytes);
                                    nOut += readBytes;
                                }
                                //
                                uiDataLen = (uint)nOut;
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    showInfo_open(0, null, "decryptData. exception");
                    goto errLabel;
                }

#endif




            }


            iErr = 0;

        errLabel:
            if (0 == iErr)
            {
                *puiDataLen = uiDataLen;
            }
            return iErr;

        }



    }

}



