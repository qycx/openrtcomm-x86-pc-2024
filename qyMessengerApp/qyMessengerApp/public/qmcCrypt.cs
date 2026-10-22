
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
        //

        //
        public static unsafe int initCrypt_qmc(CCtxQmc pVar, MIS_CNT pMisCnt, string szContainer, string szProvider, bool bNewKeySetIfNotExists, ref ENC_CTX_session pEncCtx)
        {
            int iErr = -1;
            try
            {
                //
                if (pMisCnt.commEncCtx.m_var.uiBlobLen_publicKey == 0) return 0;

                //
                byte[] publicKey = new byte[pMisCnt.commEncCtx.m_var.uiBlobLen_publicKey];
                fixed (byte* p = pMisCnt.commEncCtx.m_var.publicKeyBlob)
                {
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)p, publicKey, 0,  (int)pMisCnt.commEncCtx.m_var.uiBlobLen_publicKey);
                }
                //pMisCnt.commEncCtx.uiBlobLen_publicKey
                //pEncCtx.common.
                CspParameters cspParams = new CspParameters { ProviderType = 1 };
                //cspParams.KeyContainerName = "kk";
                string kk = Encoding.UTF8.GetString(publicKey);
                //
                /*
                RsaPublicKeyStructure rsaPubStructure = RsaPublicKeyStructure.GetInstance(
        Asn1Object.FromByteArray(publicKey));
                RsaKeyParameters rsaKeyParameters =
                new RsaKeyParameters(
        false, // not private
        rsaPubStructure.Modulus,
        rsaPubStructure.PublicExponent);
                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)rsaKeyParameters);


                pEncCtx.m_rsaProvider = new RSACryptoServiceProvider();
                pEncCtx.m_rsaProvider.ImportParameters(rsaParams);
                */




                pEncCtx.m_rsaProvider = new RSACryptoServiceProvider(cspParams);
                //
                pEncCtx.m_rsaProvider.ImportCspBlob(publicKey);

                //
                int keyLen = pEncCtx.m_rsaProvider.KeySize;
                keyLen /=  8;
                if (keyLen <= 11) goto errLabel;
                keyLen -= 11;

                pEncCtx.m_rsa_dwBlockLen_encrypt = (uint)keyLen;


                //
                pEncCtx.m_aes = Aes.Create();
                pEncCtx.m_aes.GenerateKey();//.Key = CreateKey("123");
                pEncCtx.m_aes.GenerateIV();
                //byte[] myIV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                //pEncCtx.m_aes.IV = myIV;


                //
#if false
                pEncCtx.m_aes.Mode = CipherMode.CBC;
                pEncCtx.m_aes.Padding = PaddingMode.PKCS7;
                pEncCtx.m_aes.KeySize = 128;
                pEncCtx.m_aes.BlockSize = 128;
#endif
                //                
                pEncCtx.m_plainBuf_encrypt = new byte[2048];
                pEncCtx.m_cipherData_decrypt = new byte[2048];
                pEncCtx.m_buffer_decrypt = new byte[2048];



                //
#if false
                if (pEncCtx.m_aes.Key.Length > Consts.bufSize_sessionKeyBlob) goto errLabel;
                fixed (byte* p = pEncCtx.m_var.sessionKeyBlob)
                {
                    System.Runtime.InteropServices.Marshal.Copy(pEncCtx.m_aes.Key, 0, (IntPtr)p, pEncCtx.m_aes.Key.Length);
                }
                pEncCtx.m_var.uiBlobLen_sessionKey = (uint)pEncCtx.m_aes.Key.Length;
#endif

                //
            }
            catch (Exception e)
            {
                goto errLabel;
            }

            iErr = 0;
            
        errLabel:

            return iErr;
        }


        public static void exitCrypt_qmc(CCtxQmc pVar, MIS_CNT pMisCnt, ref ENC_CTX_session pEncCtx)
        {
            int iErr = -1;

            try
            {
                if (pEncCtx.m_aes != null)
                {
                    pEncCtx.m_aes.Dispose();
                    pEncCtx.m_aes = null;
                }
                pEncCtx.m_plainBuf_encrypt = null;
                pEncCtx.m_cipherData_decrypt = null;
                pEncCtx.m_buffer_decrypt = null;

                //
                if (pEncCtx.m_rsaProvider != null)
                {
                    pEncCtx.m_rsaProvider.Dispose();
                    pEncCtx.m_rsaProvider = null;
                }
            }
            catch(Exception e)
            {

            }

            iErr = 0;
            errLabel:
            return;
        }


    }
}
