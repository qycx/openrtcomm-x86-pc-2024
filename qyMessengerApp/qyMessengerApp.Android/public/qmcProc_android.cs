
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using File = Java.IO.File;
using Thread = Java.Lang.Thread;
namespace qy.Droid
{
    partial class qyFuncs_Droid
    {


        //public static unsafe int dec_parseEncAPkt_i(uint uiTransformType, object pParent_transform, ref PKT_info_toDec pPktInfo_toDec_param, int* p_old_lPktId_getNewPkt, byte** ppInput, uint* puiSize, uint* puiSampleTimeInMs, uint* puiPts)
        public static int dec_parseEncAPkt_i(uint uiTransformType, object pParent_transform, ref PKT_info_toDec pPktInfo_toDec_param, ref int p_old_lPktId_getNewPkt, ref uint puiSize, ref uint puiSampleTimeInMs, ref uint puiPts,object p0)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();    //  QY_GET_procInfo_isCli(  );
                                                          //TCHAR tBuf[256] = _T("");


            //  myDRAW_VIDEO_DATA  *  pPkt  =  pTransform.video.pCurPkt;
            //fixed (PKT_info_toDec* pPktInfo_toDec = &pPktInfo_toDec_param)
            ref PKT_info_toDec pPktInfo_toDec = ref pPktInfo_toDec_param;
            {
                ;

                //  检查一下。每个包都只能处理一次，否则，容易死循环
                if (null == p_old_lPktId_getNewPkt) return -1;
                if (0 == pPktInfo_toDec.lPktId_getNewPkt)
                {
                    qyFuncs.showInfo_open(0, null, _T("failed: lPktId_alloc 0"));
                    return -1;
                }
                if (pPktInfo_toDec.lPktId_getNewPkt == p_old_lPktId_getNewPkt)
                {
                    qyFuncs.showInfo_open(0, null, _T("failed: lPkdId_alloc==old"));
                    return -1;
                }
                p_old_lPktId_getNewPkt = pPktInfo_toDec.lPktId_getNewPkt;

                //
                if (uiTransformType != Consts.CONST_uiTransformType_dec)
                {
                    //MACRO_qyAssert(0, _T("dec_parseEncVPkt failed, transformType err"));
                    return -1;
                }
                QY_TRANSFORM pTransform = (QY_TRANSFORM)pParent_transform;
                //pProcInfo = (MC_VAR_common*)pTransform.pProcInfo;
                if (null == pProcInfo) return -1;

                ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
                //if (!pQmcCfg) return -1;

                //
                if (!pPktInfo_toDec.bGot_msg) return -1;
                if (!pPktInfo_toDec.info.bGot_pkt) return -1;


                //TRANSFER_AUDIO_dataMemHead* pMem;

#if false
				//  2015/09/11
				MIS_MSGU* pMsg = (MIS_MSGU*)pPktInfo_toDec->pMIS_MSGU;
				if (pMsg->uiType != Consts.CONST_misMsgType_input)
				{
					traceLog("dec_parseEncAPkt_i failed, only input msg accepted");
					goto errLabel;
				}
				IM_CONTENTU* pContent = (IM_CONTENTU*)pMsg->input.data.buf;
				if (pContent->uiType != Consts.CONST_imCommType_transferAudioData) goto errLabel;

				//
				TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
				int i;
				int iPos;

				//
				iPos = pPktInfo_toDec->info.iPos;
				if (pPktInfo_toDec->info.index_mems < 0 || pPktInfo_toDec->info.index_mems >= pReq->usCnt) goto errLabel;
				i = pPktInfo_toDec->info.index_mems;

				TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
				int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
				//				
				pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

				if (iPos + pMem->uiLen > pReq->uiLen)
				{
#if __DEBUG__
				traceLogA("procTasAudio: ");
#endif
					goto errLabel;
				}
#endif
                ref TRANSFER_AUDIO_dataMemHead pMem = ref pPktInfo_toDec.pkt_encA.memHead_a;
                if (0 == pMem.uiLen)
                {
#if __DEBUG__
				traceLogA("procTaskAudio warning: mem.uiLen is 0");
#endif
                    goto errLabel;
                }

                //
                if  (  p0==null)
                {
                    goto errLabel;
                }


                //decObj_a_mediaCodec_async dm = (decObj_a_mediaCodec_async)p0;
                decObj_a_mediaCodec dm = (decObj_a_mediaCodec)p0;
                ref TRANSFORM_AUDIO_procInfo_decInfo decInfo = ref dm.m_decInfo;



                if (true)
                {


                    if (0 != pMem.uiPts)
                    {
                        //  注意这里，出错分界线.
                        
                        ///
                        if (0 == decInfo.pts.uiPts_lastRecvd)
                        {


                           
                            //
                            decInfo.pts.uiPts_first = pMem.uiPts;


                            
                            //
                            decInfo.pts.dwTickCnt_start = qyFuncs.myGetTickCount(null);
                        }
                        else
                        {

                            if (pMem.uiPts != decInfo.pts.uiPts_lastRecvd + 1)
                            {

                                //  2015/02/20
                                goto errLabel;
                            }
                            //
                        }
                        //pTransform.video.decInfo.pts.uiPts_lastRecvd  =  pPkt->head.uiPts;		
                    }
                }

                //  2015/06/18
                if (0 != pMem.uiPts)
                {
                    decInfo.pts.uiPts_lastRecvd = pMem.uiPts;
                }
                //
                ref QMC_cfg_debugStatusInfo pDbgStatusInfo = ref pProcInfo.cfg.debugStatusInfo;
                //if (pDbgStatusInfo)
                {
                    if (pDbgStatusInfo.ucbShowPreDecAStatus)
                    {
                        /*
						TCHAR tBufff[129];
						_sntprintf(tBufff, mycountof(tBufff), _T("dec_parseEncAPkt_i: %I64u, mems[%d], pts %d"), pMem->idInfo.ui64Id, i, pMem->uiPts);
						showInfo_open(0, 0, tBufff);
						*/
                    }
                }

                //
                //byte* encData = null;// pReq->buf + iPos;
                //encData = pPktInfo_toDec->pkt_encA.buf;
                if (pPktInfo_toDec.pkt_encA.len != pMem.uiLen)
                {
                    int ii = 0;
                }


                //
                //ppInput = (IntPtr)(byte*)encData;
                puiSize = pMem.uiLen;
                //  2015/01/28
                if (null != puiSampleTimeInMs)
                {
                    puiSampleTimeInMs = pMem.uiSampleTimeInMs;
                }
                //  2015/02/09
                if (null != puiPts)
                {
                    puiPts = pMem.uiPts;
                }

                //  2016/12/15
                decInfo.st.last_iSampleTimeInMs_preDec = (int)pMem.uiSampleTimeInMs;

            }

            iErr = 0;
        errLabel:

            return iErr;
        }


    }


}