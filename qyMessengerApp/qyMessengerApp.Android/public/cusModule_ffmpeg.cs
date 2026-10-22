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

using Com.Qycx.Tmpffmpeglibrary;
using Java.Nio;

namespace qy.Droid
{
    //
    class CQdcObj_ffmpeg
    {
        public int m_index_qdcUnit;
        //
        public bool bDec;
        //
        /*
        struct {

        char* pBuf;
        int iBufSize;
        int len;
    }
    output;
        */
	//
public CQdcObj_ffmpeg()
    {
        m_index_qdcUnit = -1;
        //
        bDec = false;
        //
        //memset(&output, 0, sizeof(output));
    }



};


partial class qyFuncs_Droid
{

        //
        public static int qdcInit_ffmpeg(ref QDC_MGR p)
        {
            //Com.Qycx.Tmpffmpeglibrary.TmpFfmpeg.
            //Com.Qycx.Tmpffmpeglibrary.TmpFfmpeg.
            int iRet = TmpFfmpeg.Qdc_init();
            return iRet;

        }

        public static int qdcExit_ffmpeg(ref QDC_MGR p)
        {
            int iRet = TmpFfmpeg.Qdc_exit();
            return iRet;

        }

        public static int newQdcUnitIndex_ffmpeg()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            int iRet = -1;
            bool bDbg = true;

            using (CQySyncObj syncObj = new CQySyncObj())
            {
                if (syncObj.sync("", pProcInfo.hMutex_syncQdcUnit) != 0)
                {
                    qyFuncs.traceLog("newQdcUnitIndex_ffmpeg failed: sync failed");
                    goto errLabel;
                }
                iRet = TmpFfmpeg.NewQdcUnitIndex(bDbg);
            }

        errLabel:
            return iRet;

        }
        public static void freeQdcUnitIndex_ffmpeg(int index)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            bool bDbg = true;

            using (CQySyncObj syncObj = new CQySyncObj())
            {
                if (syncObj.sync("", pProcInfo.hMutex_syncQdcUnit) != 0) goto errLabel;

                TmpFfmpeg.FreeQdcUnitIndex(index,bDbg);

            }

        errLabel:
            return;
        }


        public static unsafe int qdcInitCompressVideo_ffmpeg(ref VIDEO_COMPRESSOR_CFG pVideoCompressorCfg, ref QY_VIDEO_HEADER pVh_decompress, ref QY_VIDEO_HEADER pVh_compress, ref QOI_common pQdcObjInfo)
        {
            int iErr = -1;
            int iFourcc;
            bool bDbg = false;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            //
            if (pQdcObjInfo.var.pObj != null) return -1;

            //
            pQdcObjInfo.cfg.v = pVideoCompressorCfg.common;

            //
            pQdcObjInfo.var.pObj = new CQdcObj_ffmpeg();
            if (pQdcObjInfo.var.pObj == null) goto errLabel;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

                pObj.m_index_qdcUnit = newQdcUnitIndex_ffmpeg();
                if (pObj.m_index_qdcUnit < 0)
                {
                    qyFuncs.traceLog("qdcInitCompressV failed, newQdcUnitIndex failed");
                    goto errLabel;
                }





            //fixed (byte* fourccStr = pVideoCompressorCfg.common.fourccStr)
            {
                iFourcc = pVideoCompressorCfg.common.head.iFourcc;
            }

            if (TmpFfmpeg.EncV_init(pObj.m_index_qdcUnit, iFourcc, pVh_decompress.bih.biWidth, pVh_decompress.bih.biHeight, bDbg) != 0) return -1;

            //
            //pQdcObjInfo.v.iFourcc = iFourcc;

            //
            pQdcObjInfo.bb_input = ByteBuffer.AllocateDirect(pVh_decompress.bih.biSizeImage);
            pQdcObjInfo.bb_output = ByteBuffer.AllocateDirect(Consts.bufSize_transferVideoData);
            pQdcObjInfo.nLeft_output = 0;
            pQdcObjInfo.bHeadLen = 1 + 12;


            iErr = 0;
            errLabel:

            if(iErr != 0)
            {
                qdcExitCompressVideo_ffmpeg(ref pQdcObjInfo);
            }

            return iErr;
        }
        public static int qdcExitCompressVideo_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            bool bDbg = true;

#if DEBUG
            if (bDbg)
            {
                qyFuncs.traceLog("qdcExitCompressVideo_android enters");
            }
#endif
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            if (null == pQdcObjInfo.var.pObj) return 0;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;


            //
            if (0 != TmpFfmpeg.EncV_exit(pObj.m_index_qdcUnit, bDbg))
            {
                qyFuncs.traceLog("qdcExitCompressVideo_android: enc_exit failed");
            }

            //
            pQdcObjInfo.bb_output = null;
            pQdcObjInfo.bb_input = null;

            //
            freeQdcUnitIndex_ffmpeg(pObj.m_index_qdcUnit);
                    pObj.m_index_qdcUnit = -1;

            //
            pQdcObjInfo.var.pObj = null;


#if DEBUG
            if (bDbg)
            {
                qyFuncs.traceLog("qdcExitCompressVideo_android leaves");
            }
#endif

            return 0;

        }

        public static unsafe int qdcGetSpsPps_ffmpeg(ref QOI_common pQdcObjInfo, IntPtr spsBuf, ref uint puiSpsSize, IntPtr ppsBuf, ref uint puiPpsSize, IntPtr vps, ref uint puiVpsSize)
        {
            int iErr = -1;
            ByteBuffer bb;
            bb = ByteBuffer.AllocateDirect(1024);
            int[] lens = new int[3];
            byte* p;
            int k;
            bool bDbg = false;

            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            if (0 != TmpFfmpeg.EncV_getSpsPps(pObj.m_index_qdcUnit, bb, lens, bDbg)) return -1;

            p = (byte*)bb.GetDirectBufferAddress();
            k = 0;
            //
            switch (pQdcObjInfo.cfg.v.head.iFourcc)
            {
                case Consts.CONST_fourcc_h264:
                    //
                    qyFuncs.mymemcpy(spsBuf, (IntPtr)p, (uint)lens[k]);
                    puiSpsSize = (uint)lens[k];
                    p += lens[k];
                    k++;
                    qyFuncs.mymemcpy(ppsBuf, (IntPtr)p, (uint)lens[k]);
                    puiPpsSize = (uint)lens[k];
                    p += lens[k];
                    //
                    puiVpsSize = 0;
                    //
                    break;
                default:
                    goto errLabel;
            }

            iErr = 0;
        errLabel:

            return iErr;

        }


        //
        public static unsafe int qdcCompressVideo_ffmpeg(ref QOI_common pQdcObjInfo, ref BITMAPINFOHEADER pBmpInfo_input, IntPtr pInput, int iSampleTimeInMs)
        {
            int iErr = -1;
            //
            bool bDbg = false;
#if DEBUG
            string str;
#endif
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            //
            if (pQdcObjInfo.bb_input == null || pQdcObjInfo.bb_output == null) return -1;
            ByteBuffer bb_input = (ByteBuffer)pQdcObjInfo.bb_input;
            ByteBuffer bb_output = (ByteBuffer)pQdcObjInfo.bb_output;
            byte* pBbInput = (byte*)bb_input.GetDirectBufferAddress();
            if (null == pBbInput) goto errLabel;
            byte* pBbOutput = (byte*)bb_output.GetDirectBufferAddress();
            if (null == pBbOutput) goto errLabel;

            if (pBmpInfo_input.biSizeImage > bb_input.Capacity()) goto errLabel;
            qyFuncs.mymemcpy((IntPtr)pBbInput, pInput, (uint)pBmpInfo_input.biSizeImage);

            if (pQdcObjInfo.nLeft_output != 0)
            {
                qyFuncs.showNotification_open(0, 0, 0, "Err: qdcCompressVideo_android; nLeft is not 0");
                goto errLabel;
            }
            int bufSize_output = bb_output.Capacity();
            if (bufSize_output <= pQdcObjInfo.bHeadLen) goto errLabel;
            int len_output0;
            int len_output1;
            int len_output2;
            //
            len_output0 = bufSize_output - pQdcObjInfo.bHeadLen;
            //
            uint len = 0;
            *pBbOutput = pQdcObjInfo.bHeadLen; len = sizeof(byte);
            *(int*)(pBbOutput + len) = len_output0;
            //
            if (0 != TmpFfmpeg.EncV_data(pObj.m_index_qdcUnit, bb_input, pBmpInfo_input.biSizeImage, bb_output, bufSize_output, bDbg))
            {
                qyFuncs.traceLog("qdcCompressVideo_android: enc_data failed");
                goto errLabel;
            }
            //
            len = sizeof(byte);
            len_output0 = *(int*)(pBbOutput + len); len += sizeof(int);
            len_output1 = *(int*)(pBbOutput + len); len += sizeof(int);
            len_output2 = *(int*)(pBbOutput + len); len += sizeof(int);
            if (len > pQdcObjInfo.bHeadLen)
            {
                goto errLabel;
            }
            //
            pQdcObjInfo.nLeft_output = len_output0;
            if (pQdcObjInfo.nLeft_output != 0)
            {
                int ii = 0;
            }
            //
#if DEBUG
            if (bDbg)
            {
                str = string.Format("qdcCompressVideo_android: enc_data ok,len_output {0}", pQdcObjInfo.nLeft_output);
                qyFuncs.traceLog(str);
            }
#endif
            //
            int kk;
            switch (len_output1)
            {
                case 888:
                    break;
                case 889:
                case 890:
                    kk = 0;
                    break;
                default:
                    kk = 1;
                    break;
            }

            iErr = 0;
        errLabel:

            return iErr;

        }

        //
        public static unsafe int qdcGetOutputByIndex_ffmpeg(ref QOI_common pQdcObjInfo, int index, IntPtr pbOutData, ref uint puiDataSize, ref uint puiSampleTimeInMs, ref uint pui_rtTimeLen)
        {
            int iErr = -1;
            bool bDbg = false;

            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            //
            if (pQdcObjInfo.bb_output == null)
            {
                qyFuncs.traceLog("qdcGetOutputByIndex_android failed, bb_output is null");
                return -1;
            }

            //
            ByteBuffer bb_output = (ByteBuffer)pQdcObjInfo.bb_output;
            byte* pBbOutput = (byte*)bb_output.GetDirectBufferAddress();
            if (null == pBbOutput) goto errLabel;

            //
            if (pObj.bDec)
            {
                //TmpFfmpeg.Dec_getOutputByIndex()
                if (pQdcObjInfo.nPktsDeced == 0)
                {
                    puiDataSize = 0;
                    return -1;
                }
                if (index == 0)
                {

                    if (puiDataSize < pQdcObjInfo.iDecBufSize)
                    {
                        goto errLabel;
                    }

                    qyFuncs.mymemcpy(pbOutData, (IntPtr)pBbOutput + pQdcObjInfo.bHeadLen, (uint)pQdcObjInfo.iDecBufSize);
                    puiDataSize = (uint)pQdcObjInfo.iDecBufSize;
                    pQdcObjInfo.iDecBufSize = 0;
                    iErr = 0; goto errLabel;
                }
                if (0 != TmpFfmpeg.Dec_getOutputByIndex(pObj.m_index_qdcUnit, index - 1, bb_output, bb_output.Capacity(), bDbg)) goto errLabel;
                //
                byte* p;
                p = pBbOutput + 1;
                int iDecBufSize = *(int*)p;
                if (iDecBufSize == 0) goto errLabel;
                if (puiDataSize < iDecBufSize) goto errLabel;
                qyFuncs.mymemcpy(pbOutData, (IntPtr)pBbOutput + pQdcObjInfo.bHeadLen, (uint)iDecBufSize);
                puiDataSize = (uint)iDecBufSize;
                iErr = 0; goto errLabel;
            }
            else
            {

                if (0 == pQdcObjInfo.nLeft_output)
                {
                    puiDataSize = 0;
                    return -1;
                }

                if (pQdcObjInfo.nLeft_output > puiDataSize) goto errLabel;

                qyFuncs.mymemcpy(pbOutData, (IntPtr)pBbOutput + pQdcObjInfo.bHeadLen, (uint)pQdcObjInfo.nLeft_output);
                puiDataSize = (uint)pQdcObjInfo.nLeft_output;
                pQdcObjInfo.nLeft_output = 0;
            }

            iErr = 0;
        errLabel:

            return iErr;
        }

        public static int qdcClearOutputs_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            bool bDbg = false;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            if (pObj.bDec)
            {
                TmpFfmpeg.Dec_clearOutputs(pObj.m_index_qdcUnit, bDbg);

            }
            else
            {

            }

            //
            return 0;
        }

        /*
    //
    public static native int dec_init(int index_qdcUnit, int iFourcc, boolean bDbg);
    public static native int dec_exit(int index_qdcUnit,boolean bDbg);
    public static native int dec_data(int index_qdcUnit, ByteBuffer data,int data_size, int iInputBufferPaddingSize, ByteBuffer dec, int decSize, boolean bDbg);
    public static native int dec_getOutputByIndex(int index_qdcUnit, int index, ByteBuffer dec, int decSize,boolean bDbg);
    public static native int dec_clearOutputs(int index_qdcUnit,boolean bDbg);
*/

        //       
        public static unsafe int qdcInitDecompressVideo_ffmpeg(ref VIDEO_COMPRESSOR_CFG pVideoCompressorCfg, ref QY_VIDEO_HEADER pVh_compress, ref QY_VIDEO_HEADER pVh_decompress, ref QOI_common pQdcObjInfo)
        {
            int iErr = -1;
            bool bDbg = true;



            int iFourcc;


            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            if (pQdcObjInfo.var.pObj != null) return -1;

            //
            pQdcObjInfo.var.pObj = new CQdcObj_ffmpeg();
            if (pQdcObjInfo.var.pObj == null) goto errLabel;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

                pObj.m_index_qdcUnit = newQdcUnitIndex_ffmpeg();
                if (pObj.m_index_qdcUnit < 0)
                {
                    qyFuncs.traceLog("qdcInitDeCompressV failed, newQdcUnitIndex failed");
                    goto errLabel;
                }

            pObj.bDec = true;


            //fixed (byte* fourccStr = pVideoCompressorCfg.common.fourccStr) 
            {
                iFourcc = pVideoCompressorCfg.common.head.iFourcc;
            }

            if (0 != TmpFfmpeg.DecV_init(pObj.m_index_qdcUnit, iFourcc, bDbg)) goto errLabel;

            //
            pQdcObjInfo.bHeadLen = 1 + 7 * sizeof(int);
            pQdcObjInfo.nPktsDeced = 0;


            //
            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                qdcExitDecompressVideo_ffmpeg(ref pQdcObjInfo);
            }

            return iErr;
        }
        public static unsafe int qdcExitDecompressVideo_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            int iErr = -1;
            bool bDbg = true;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            if (null == pQdcObjInfo.var.pObj) return 0;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            TmpFfmpeg.DecV_exit(pObj.m_index_qdcUnit, bDbg);

            //
            pQdcObjInfo.bb_input = null;
            pQdcObjInfo.bb_output = null;

            freeQdcUnitIndex_ffmpeg(pObj.m_index_qdcUnit);
            pObj.m_index_qdcUnit = -1;

            //
            pQdcObjInfo.var.pObj = null;


            //
            iErr = 0;
            return iErr; ;
        }

        public static unsafe int qdcDecompressVideo_ffmpeg(ref QOI_common pQdcObjInfo, ref BITMAPINFOHEADER pBmpInfo_input, IntPtr pInput, int iSampleTimeInMs)    //  2012/06/17
        {
            int iErr = -1;
            int iPaddingSize = 128;

            bool bDbg = false;
            string str;
#if DEBUG
            //
            //bDbg = true;
#endif
            //
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            //
            if (pQdcObjInfo.bb_input == null)
            {
                pQdcObjInfo.bb_input = ByteBuffer.AllocateDirect(Consts.bufSize_transferVideoData);
                if (pQdcObjInfo.bb_input == null) goto errLabel;
                //
                qyFuncs.traceLog("qdcDecVideo_android: bb_input alloc");
            }
            if (pQdcObjInfo.bb_output == null)
            {
                pQdcObjInfo.bb_output = ByteBuffer.AllocateDirect(128);
                if (pQdcObjInfo.bb_output == null) goto errLabel;
                //
                qyFuncs.traceLog("qdcDecVideo_android: bb_outpt alloc");
            }
            ByteBuffer bb_input = (ByteBuffer)pQdcObjInfo.bb_input;
            ByteBuffer bb_output = (ByteBuffer)pQdcObjInfo.bb_output;
            byte* pBbInput = (byte*)bb_input.GetDirectBufferAddress();
            if (null == pBbInput) goto errLabel;
            byte* pBbOutput = (byte*)bb_output.GetDirectBufferAddress();
            if (null == pBbOutput) goto errLabel;

            //
            if (pBmpInfo_input.biSizeImage + iPaddingSize > bb_input.Capacity())
            {
                qyFuncs.traceLog("qdcDecVideo failed: bb_input is too small");
                goto errLabel;
            }
            qyFuncs.mymemcpy((IntPtr)pBbInput, pInput, (uint)pBmpInfo_input.biSizeImage);

            //
            int total_nums = 7;
            byte len = (byte)(1 + total_nums * 4);
            //int head_array[7];
            if (pQdcObjInfo.bHeadLen < len)
            {
                qyFuncs.traceLog("qdcDecVideo failed, bHeadLen error");
                goto errLabel;
            }

            //
            *pBbOutput = pQdcObjInfo.bHeadLen;

            //
            if (0 != TmpFfmpeg.DecV_data(pObj.m_index_qdcUnit, bb_input, pBmpInfo_input.biSizeImage, iPaddingSize, bb_output, bb_output.Capacity(), bDbg)) goto errLabel;

            //
            byte* p = pBbOutput;
            p++;
            //
            int k;
            k = 0;
            int tmpiPaddingSize = *(int*)(p + k * sizeof(int));
            k++;
            int nPktsDeced = *(int*)(p + k * sizeof(int));
            k++;
            int iFourcc_dec = *(int*)(p + k * sizeof(int)); ;
            k++;
            int iW = *(int*)(p + k * sizeof(int));
            k++;
            int iH = *(int*)(p + k * sizeof(int)); ;
            k++;
            int iDecBufSize = *(int*)(p + k * sizeof(int)); ;
            k++;
            int bBufTooSmall = *(int*)(p + k * sizeof(int)); ;
            k++;

            //
            if (bDbg)
            {
                str = string.Format("qdcDecV: bBufTooSmall {0}, nPktDeced {1}", bBufTooSmall, nPktsDeced);
                qyFuncs.traceLog(str);
            }

            //
            if (0 != bBufTooSmall)
            {
                switch (iFourcc_dec)
                {
                    case Consts.CONST_fourcc_I420:
                        BITMAPINFOHEADER bih;
                        qyFuncs.makeBmpInfoHeader_i420(12, iW, iH, &bih);
                        int bufSize_output = bih.biSizeImage + pQdcObjInfo.bHeadLen;
                        if (bufSize_output > bb_output.Capacity())
                        {
                            pQdcObjInfo.bb_output = null;
                            //
                            pQdcObjInfo.bb_output = ByteBuffer.AllocateDirect(bufSize_output);
                            if (pQdcObjInfo.bb_output == null) goto errLabel;
                            qyFuncs.traceLog("qdcDecVideo_android: bb realloc");
                        }
                        break;
                    case Consts.CONST_fourcc_YV12:
                    default:
                        qyFuncs.traceLog("qdcDecVideo_android failed, iFourcc_dec unsupported");
                        goto errLabel;
                }

            }

            //
            pQdcObjInfo.nPktsDeced = nPktsDeced;
            pQdcObjInfo.iDecBufSize = iDecBufSize;



        errLabel:

            return 0;
        }



        ///////////////////////////
        /// <summary>
        ///     public static native int encA_init(int index_qdcUnit, short wFormatTag, boolean bDbg);
        //public static native int encA_exit(int index_qdcUnit, boolean bDbg);
        //public static native int encA_data(int index_qdcUnit, ByteBuffer pcm, int pcmLen, ByteBuffer raw, int rawSize, boolean bDbg);

        /// </summary>
        /// <param name="pAudioCompressorCfg_reserved"></param>
        /// <param name="pAh_decompress"></param>
        /// <param name="pAh_compress"></param>
        /// <param name="pQdcObjInfo"></param>
        /// <returns></returns>
        //
        public static int qdcInitCompressAudio_ffmpeg(ref AUDIO_COMPRESSOR_CFG pAudioCompressorCfg_reserved, ref QY_AUDIO_HEADER pAh_decompress, ref QY_AUDIO_HEADER pAh_compress, ref QOI_common pQdcObjInfo)
        {
            int iErr = -1;

            bool bDbg = true;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            qyFuncs.traceLog("qdcInitCompressAudio_ffmpeg enters");


            //
            if (pQdcObjInfo.var.pObj != null) return -1;

            //
            pQdcObjInfo.var.pObj = new CQdcObj_ffmpeg();
            if (pQdcObjInfo.var.pObj == null) goto errLabel;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

                pObj.m_index_qdcUnit = newQdcUnitIndex_ffmpeg();
                if (pObj.m_index_qdcUnit < 0)
                {
                    qyFuncs.traceLog("qdcInitCompressA failed, newQdcUnitIndex failed");
                    goto errLabel;
                }

            //
            if (pAh_decompress.wfx.nAvgBytesPerSec == 0)
            {
                qyFuncs.traceLog("initEncA failed, nAvgBytesPerSec is 0");
                return -1;
            }

            //
            if (0 != TmpFfmpeg.EncA_init(pObj.m_index_qdcUnit, (short)pAudioCompressorCfg_reserved.wf_compress.wFormatTag, bDbg)) return -1;

            //
            //
            pAh_compress = pAh_decompress;
            pAh_compress.wfx.wFormatTag = Consts.WAVE_FORMAT_MPEGLAYER3;


            //
            //
            pQdcObjInfo.bb_input = ByteBuffer.AllocateDirect((int)pAh_decompress.wfx.nAvgBytesPerSec);
            pQdcObjInfo.bb_output = ByteBuffer.AllocateDirect(Consts.bufSize_transferAudioData);
            pQdcObjInfo.nLeft_output = 0;
            pQdcObjInfo.bHeadLen = 1 + 12;


            iErr = 0;
            errLabel:

            if(iErr != 0)
            {
                qdcExitCompressAudio_ffmpeg(ref pQdcObjInfo);
            }

            //
            qyFuncs.traceLog("qdcInitCompressAudio_ffmpeg leaves");

            //
            return iErr;
        }

        public static int qdcExitCompressAudio_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            bool bDbg = true;

            //
            qyFuncs.traceLog("qdcExitCompressAudio_ffmpeg enters");

            //
            if (null==pQdcObjInfo.var.pObj) return 0;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            TmpFfmpeg.EncA_exit(pObj.m_index_qdcUnit, bDbg);

            //
            pQdcObjInfo.bb_input = null;
            pQdcObjInfo.bb_output = null;


            freeQdcUnitIndex_ffmpeg(pObj.m_index_qdcUnit);
                    pObj.m_index_qdcUnit = -1;

            //
            pQdcObjInfo.var.pObj = null;

            //
            qyFuncs.traceLog("qdcExitCompressAudio_ffmpeg leaves");

            //
            return 0;
        }

        public static unsafe int qdcCompressAudio_ffmpeg(ref QOI_common pQdcObjInfo, ref QY_AUDIO_HEADER pAh_decompress, IntPtr pInput, uint uiInputSize, int iSampleTimeInMs)
        {

            int iErr = -1;
            //
            bool bDbg = false;
            string str;

            //
#if DEBUG
            //
            //bDbg = true;
            //
            if (bDbg)
            {
                str = string.Format("encA_android: enters");
                qyFuncs.traceLog(str);
            }
#endif 
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

            //
            if (pQdcObjInfo.bb_input == null || pQdcObjInfo.bb_output == null) return -1;
            ByteBuffer bb_input = (ByteBuffer)pQdcObjInfo.bb_input;
            ByteBuffer bb_output = (ByteBuffer)pQdcObjInfo.bb_output;
            byte* pBbInput = (byte*)bb_input.GetDirectBufferAddress();
            if (null == pBbInput) goto errLabel;
            byte* pBbOutput = (byte*)bb_output.GetDirectBufferAddress();
            if (null == pBbOutput) goto errLabel;

            if (uiInputSize > bb_input.Capacity()) goto errLabel;
            qyFuncs.mymemcpy((IntPtr)pBbInput, pInput, (uint)uiInputSize);

            if (pQdcObjInfo.nLeft_output != 0)
            {
                qyFuncs.showNotification_open(0, 0, 0, "Err: qdcCompressVideo_android; nLeft is not 0");
                goto errLabel;
            }
            int bufSize_output = bb_output.Capacity();
            if (bufSize_output <= pQdcObjInfo.bHeadLen) goto errLabel;
            int len_output0;
            int len_output1;
            int len_output2;
            //
            len_output0 = bufSize_output - pQdcObjInfo.bHeadLen;
            //
            uint len = 0;
            *pBbOutput = pQdcObjInfo.bHeadLen; len = sizeof(byte);
            *(int*)(pBbOutput + len) = len_output0;
            //
            if (0 != TmpFfmpeg.EncA_data(pObj.m_index_qdcUnit, bb_input, (int)uiInputSize, bb_output, bufSize_output, bDbg))
            {
                qyFuncs.traceLog("qdcCompressVideo_android: enc_data failed");
                goto errLabel;
            }
            //
            len = sizeof(byte);
            len_output0 = *(int*)(pBbOutput + len); len += sizeof(int);
            len_output1 = *(int*)(pBbOutput + len); len += sizeof(int);
            len_output2 = *(int*)(pBbOutput + len); len += sizeof(int);
            if (len > pQdcObjInfo.bHeadLen)
            {
                goto errLabel;
            }
            //
            pQdcObjInfo.nLeft_output = len_output0;
            if (pQdcObjInfo.nLeft_output != 0)
            {
                int ii = 0;
            }
            //
#if DEBUG
            if (bDbg)
            {
                str = string.Format("qdcCompressAudio_android: enc_data ok,len_output {0}", pQdcObjInfo.nLeft_output);
                qyFuncs.traceLog(str);
            }
#endif
            //
            int kk;
            switch (len_output1)
            {
                case 888:
                    break;
                case 889:
                case 890:
                    kk = 0;
                    break;
                default:
                    kk = 1;
                    break;
            }

            iErr = 0;
        errLabel:

            if (iErr != 0)
            {
                str = string.Format("encA_android failed");
                qyFuncs.traceLog(str);
            }

            return iErr;



        }

        //  2016/12/05
        public static int qdcCompressAudioRun_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            qyFuncs.traceLog("qdcCompressAudioRun_android: not finished");
            return 0;

        }


        //
        public static int qdcInitDecompressAudio_ffmpeg(ref AUDIO_COMPRESSOR_CFG pAudioCompressorCfg, ref QY_AUDIO_HEADER pAh_compress, ref QY_AUDIO_HEADER pAh_decompress, ref QOI_common pQdcObjInfo)
        {
            int iErr = -1;
            bool bDbg = true;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            if (pQdcObjInfo.var.pObj != null) return -1;

            //
            pQdcObjInfo.cfg.a = pAudioCompressorCfg.common;


            //
            pQdcObjInfo.var.pObj = new CQdcObj_ffmpeg();
            if (pQdcObjInfo.var.pObj == null) goto errLabel;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;

                pObj.m_index_qdcUnit = newQdcUnitIndex_ffmpeg();
                if (pObj.m_index_qdcUnit < 0)
                {
                    qyFuncs.traceLog("qdcInitDeCompressA failed, newQdcUnitIndex failed");
                    goto errLabel;
                }

            //
            pObj.bDec = true;

            //
            //
            if (qyFuncs.isQ2Warning(pQdcObjInfo.cfg.a.pAudioQ2, 5))
            {
                qyFuncs.emptyQ2(pQdcObjInfo.cfg.a.pAudioQ2);
                //
                string str;
                str = string.Format("qdcInitDecA: pAudioQ2 is full, empty it");
                qyFuncs.showInfo_open(0, "", str);
            }





            //
            if (0 != TmpFfmpeg.DecA_init(pObj.m_index_qdcUnit, (short)pAudioCompressorCfg.wf_compress.wFormatTag, bDbg)) return -1;

            //
            pQdcObjInfo.bHeadLen = 1 + 7 * sizeof(int);
            pQdcObjInfo.nPktsDeced = 0;


            iErr = 0;
            errLabel:

            if(iErr != 0)
            {
                qdcExitDecompressAudio_ffmpeg(ref pQdcObjInfo);
            }

            return iErr;

        }

        public static int qdcExitDecompressAudio_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            bool bDbg = true;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            if (null == pQdcObjInfo.var.pObj) return 0;
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;


            TmpFfmpeg.DecA_exit(pObj.m_index_qdcUnit, bDbg);

            //
            pQdcObjInfo.bb_input = null;
            pQdcObjInfo.bb_output = null;

            freeQdcUnitIndex_ffmpeg(pObj.m_index_qdcUnit);
                    pObj.m_index_qdcUnit = -1;

            //
            pQdcObjInfo.var.pObj = null;


            return 0;

        }

        public static unsafe int qdcDecompressAudio_ffmpeg(ref QOI_common pQdcObjInfo, ref QY_AUDIO_HEADER pAh_compress, IntPtr pInput, uint uiInputLen, int iSampleTimeInMs, uint ui_rtTimeLen)
        {
            int iErr = -1;
            int iPaddingSize = 128;

            bool bDbg = false;
#if DEBUG
            string str;
#endif
            CQdcObj_ffmpeg pObj = (CQdcObj_ffmpeg)pQdcObjInfo.var.pObj;
            //
            if (pQdcObjInfo.bb_input == null)
            {
                pQdcObjInfo.bb_input = ByteBuffer.AllocateDirect(Consts.bufSize_transferAudioData);
                if (pQdcObjInfo.bb_input == null) goto errLabel;
                //
                qyFuncs.traceLog("qdcDecAudio_android: bb_input alloc");
            }
            if (pQdcObjInfo.bb_output == null)
            {
                int nAvgBytesPerSec = (int)pAh_compress.wfx.nSamplesPerSec * pAh_compress.wfx.wBitsPerSample / 8 * pAh_compress.wfx.nChannels;
                if (nAvgBytesPerSec == 0)
                {
                    qyFuncs.traceLog("qdcDecA_android faile, nAvgBytesPerSec is 0");
                    goto errLabel;
                }
                pQdcObjInfo.bb_output = ByteBuffer.AllocateDirect(nAvgBytesPerSec + pQdcObjInfo.bHeadLen);
                if (pQdcObjInfo.bb_output == null) goto errLabel;
                //
                qyFuncs.traceLog("qdcDecAudio_android: bb_outpt alloc");
            }
            ByteBuffer bb_input = (ByteBuffer)pQdcObjInfo.bb_input;
            ByteBuffer bb_output = (ByteBuffer)pQdcObjInfo.bb_output;
            byte* pBbInput = (byte*)bb_input.GetDirectBufferAddress();
            if (null == pBbInput) goto errLabel;
            byte* pBbOutput = (byte*)bb_output.GetDirectBufferAddress();
            if (null == pBbOutput) goto errLabel;

            //
            if (uiInputLen + iPaddingSize > bb_input.Capacity())
            {
                qyFuncs.traceLog("qdcDecVideo failed: bb_input is too small");
                goto errLabel;
            }
            qyFuncs.mymemcpy((IntPtr)pBbInput, pInput, (uint)uiInputLen);

            //
            int total_nums = 7;
            byte len = (byte)(1 + total_nums * 4);
            //int head_array[7];
            if (pQdcObjInfo.bHeadLen < len)
            {
                qyFuncs.traceLog("qdcDecVideo failed, bHeadLen error");
                goto errLabel;
            }

            //
            *pBbOutput = pQdcObjInfo.bHeadLen;

            //
            //qyFuncs.traceLog("decA_android: before ffmpeg.decA_data");

            //
            if (0 != TmpFfmpeg.DecA_data(pObj.m_index_qdcUnit, bb_input, (int)uiInputLen, iPaddingSize, bb_output, bb_output.Capacity(), bDbg))
            {
                goto errLabel;
            }

            //
            //qyFuncs.traceLog("decA_android: after ffmpeg.decA_data");



            //
            byte* p = pBbOutput;
            p++;
            //
            int k;
            k = 0;
            int tmpiPaddingSize = *(int*)(p + k * sizeof(int));
            k++;
            int nPktsDeced = *(int*)(p + k * sizeof(int));
            k++;
            int iFmt_dec = *(int*)(p + k * sizeof(int)); ;
            k++;
            int iDecBufSize = *(int*)(p + k * sizeof(int)); ;
            k++;
            int bBufTooSmall = *(int*)(p + k * sizeof(int)); ;
            k++;

            //
            if (0 != bBufTooSmall)
            {
                qyFuncs.traceLog("qdcDecAudio_android failed, buf too small");
                goto errLabel;

            }
            if (nPktsDeced != 0)
            {
                int kk = 0;
            }

            //
            pQdcObjInfo.nPktsDeced = nPktsDeced;
            pQdcObjInfo.iDecBufSize = iDecBufSize;


            iErr = 0;

        errLabel:

            return iErr;
        }


        //
        //  2016/12/05
        public static int qdcDecompressAudioRun_ffmpeg(ref QOI_common pQdcObjInfo)
        {
            qyFuncs.traceLog("not finished");
            return 0;

        }


    }


}