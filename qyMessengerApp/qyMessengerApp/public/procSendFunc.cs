using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {


        //
        public static unsafe int procSendAvInfo(ref CTX_caller ctxCaller, MIS_CNT pMisCnt, object hWnd_task, bool bDlgTalk, uint uiTaskType, ref COMPRESS_VIDEO pCompressVideo, ref COMPRESS_AUDIO pCompressAudio, int iIndex_sharedObj, int iIndex_sharedObjUsr, TALKER_dynBmp* pTalkerDynBmp, QY_MESSENGER_ID* p_content_idInfo_logicalPeer, int* piTaskId, ref MSGR_ADDR pAddr, ref P_sendAvInfo_taskInfo pOst)
        {
            uint uiTranNo_avTask_starter = 0;
            //
            TASK_av_props* pTaskAvProps = null;

            //
            AV_COMPRESSOR_CFG acc = new AV_COMPRESSOR_CFG();
            //
            QMC_mosaic_maker qmm = new QMC_mosaic_maker();

            //  
            return procSendAvInfoFunc(ref ctxCaller, pMisCnt, hWnd_task, bDlgTalk, uiTaskType, null, null, ref acc, ref qmm, ref pCompressVideo, ref pCompressAudio, iIndex_sharedObj, iIndex_sharedObjUsr, pTalkerDynBmp, p_content_idInfo_logicalPeer, uiTranNo_avTask_starter, pTaskAvProps, piTaskId, ref pAddr, ref pOst);
        }


        //  2015/03/11
        public static unsafe int procSendAvInfo(ref CTX_caller ctxCaller, MIS_CNT pMisCnt, object hWnd_task, bool bDlgTalk, uint uiTaskType, ref COMPRESS_VIDEO pCompressVideo, ref COMPRESS_AUDIO pCompressAudio, int iIndex_sharedObj, int iIndex_sharedObjUsr, TALKER_dynBmp* pTalkerDynBmp, QY_MESSENGER_ID* p_content_idInfo_logicalPeer, TASK_av_props* pTaskAvProps, int* piTaskId, ref MSGR_ADDR pAddr)
        {
            uint uiTranNo_avTask_starter = 0;
            //
            //TASK_av_props  *  pTaskAvProps  =  null;
            P_sendAvInfo_taskInfo pOst = new P_sendAvInfo_taskInfo();

            //
            AV_COMPRESSOR_CFG acc = new AV_COMPRESSOR_CFG();
            //
            QMC_mosaic_maker qmm = new QMC_mosaic_maker();

            //  
            return procSendAvInfoFunc(ref ctxCaller, pMisCnt, hWnd_task, bDlgTalk, uiTaskType, null, null, ref acc, ref qmm, ref pCompressVideo, ref pCompressAudio, iIndex_sharedObj, iIndex_sharedObjUsr, pTalkerDynBmp, p_content_idInfo_logicalPeer, uiTranNo_avTask_starter, pTaskAvProps, piTaskId, ref pAddr, ref pOst);
        }



        //
        public static unsafe int procSendAvInfo_conf(ref CTX_caller ctxCaller, MIS_CNT pMisCnt, object hWnd_task, bool bDlgTalk, uint uiTaskType, VIDEO_CONFERENCE_CFG* pVideoConferenceCfg, TALKER_policy* pTalkerPolicy, ref AV_COMPRESSOR_CFG pAvCompressor_conf, ref QMC_mosaic_maker pMosaic, ref COMPRESS_VIDEO pCompressVideo, ref COMPRESS_AUDIO pCompressAudio, int iIndex_sharedObj, int iIndex_sharedObjUsr, TALKER_dynBmp* pTalkerDynBmp, QY_MESSENGER_ID* p_content_idInfo_logicalPeer, int* piTaskId, ref MSGR_ADDR pAddr, ref P_sendAvInfo_taskInfo pOst)
        {
            uint uiTranNo_avTask_starter = 0;
            //
            TASK_av_props* pTaskAvProps = null;

            return procSendAvInfoFunc(ref ctxCaller, pMisCnt, hWnd_task, bDlgTalk, uiTaskType, pVideoConferenceCfg, pTalkerPolicy, ref pAvCompressor_conf, ref pMosaic, ref pCompressVideo, ref pCompressAudio, iIndex_sharedObj, iIndex_sharedObjUsr, pTalkerDynBmp, p_content_idInfo_logicalPeer, uiTranNo_avTask_starter, pTaskAvProps, piTaskId, ref pAddr, ref pOst);
        }

        // 2014/11/22
        public static unsafe int procSendAvInfo(ref CTX_caller ctxCaller, MIS_CNT pMisCnt, object hWnd_task, bool bDlgTalk, uint uiTaskType, ref COMPRESS_VIDEO pCompressVideo, ref COMPRESS_AUDIO pCompressAudio, int iIndex_sharedObj, int iIndex_sharedObjUsr, TALKER_dynBmp* pTalkerDynBmp, QY_MESSENGER_ID* p_content_idInfo_logicalPeer, uint uiTranNo_avTask_starter, int* piTaskId, ref MSGR_ADDR pAddr)
        {
            //
            TASK_av_props* pTaskAvProps = null;
            P_sendAvInfo_taskInfo pOst = new P_sendAvInfo_taskInfo();

            AV_COMPRESSOR_CFG acc = new AV_COMPRESSOR_CFG();
            QMC_mosaic_maker qmm = new QMC_mosaic_maker();

            //  
            return procSendAvInfoFunc(ref ctxCaller, pMisCnt, hWnd_task, bDlgTalk, uiTaskType, null, null, ref acc, ref qmm, ref pCompressVideo, ref pCompressAudio, iIndex_sharedObj, iIndex_sharedObjUsr, pTalkerDynBmp, p_content_idInfo_logicalPeer, uiTranNo_avTask_starter, pTaskAvProps, piTaskId, ref pAddr, ref pOst);
        }


        public static unsafe int getMosaicTranInfo(ref SHARE_dyn_bmp pShareDynBmp, MOSAIC_TRAN_INFO* p)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            p->resObj.uiObjType = pShareDynBmp.resObj.uiObjType;
            p->resObj.usIndex_obj = pShareDynBmp.resObj.usIndex_obj;
            //			  
            if (!pShareDynBmp.var.ucbLocalVideoOpen) goto errLabel;
            QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, pShareDynBmp.var.iIndex_sharedObj);
            if (null == pSharedObj) goto errLabel;
            int index_capBmp = pSharedObj.var.iIndex_capBmp;
            CAP_procInfo_bmpCommon pCapBmp = getCapBmpBySth(pProcInfo, index_capBmp, 0);
            if (null == pCapBmp) goto errLabel;
            ref COMPRESS_VIDEO pCompressVideo = ref pCapBmp.compressVideo;

            //memcpy(&p->video.compressor, &pCompressVideo.compressor, sizeof(p->video.compressor)  );
            p->video.compressor = pCompressVideo.compressor.common.head;
            SHARED_OBJ_USR pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
            if (null == pSharedObjUsr) goto errLabel;
            p->video.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;
            //
            //memcpy(&p->video.vh_compress, &pCompressVideo.vh_compress, sizeof(p->video.vh_compress)  );
            p->video.vh_compress = pCompressVideo.vh_compress;

            //memcpy(&p->video.vh_stream, &pCompressVideo.vh_stream, sizeof(p->video.vh_stream)  );      //  2014/03/30			
            p->video.vh_stream = pCompressVideo.vh_stream;
            //
            //memcpy(&p->video.vh_decompress, &pCompressVideo.vh_decompress, sizeof(p->video.vh_decompress)  );
            p->video.vh_decompress = pCompressVideo.vh_decompress;


            iErr = 0;
        errLabel:

            return iErr;
        }


        public static unsafe int procSendAvInfoFunc(ref CTX_caller ctxCaller, MIS_CNT pMisCnt, object hWnd_task, bool bDlgTalk, uint uiTaskType, VIDEO_CONFERENCE_CFG* pVideoConferenceCfg, TALKER_policy* pTalkerPolicy, ref AV_COMPRESSOR_CFG pAvCompressor_conf, ref QMC_mosaic_maker pMosaic, ref COMPRESS_VIDEO pCompressVideo, ref COMPRESS_AUDIO pCompressAudio, int iIndex_sharedObj, int iIndex_sharedObjUsr, TALKER_dynBmp* pTalkerDynBmp, QY_MESSENGER_ID* p_content_idInfo_logicalPeer, uint uiTranNo_avTask_starter, TASK_av_props* pTaskAvProps, int* piTaskId, ref MSGR_ADDR pAddr, ref P_sendAvInfo_taskInfo pPst)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //TCHAR tBuf[256] = _T("");
            CQyMalloc mallocObj;
            QY_SHARED_OBJ pSharedObj = null;
            //  int				iIndex_sharedObjUsr		=	0;
            SHARED_OBJ_USR pSharedObjUsr = null;

            bool ucbSendLocalAv = false;

            //  2013/02/11
            bool bReSend_viewDynBmp = false;
            DYN_BMP_RULE* pDynBmp = null;

            //  2014/10/29
            int iTaskId_suggested = 0;

            //
            TALKER_dynBmp* pRuleU = (TALKER_dynBmp*)pTalkerDynBmp;

#if __DEBUG__
			traceLog(_T("procSendAvInfo called"));
#endif

            //
            if (null == pMisCnt) return -1;

            //  2017/08/02
            bool ucbVideoConference = false;
            //  2017/07/17
            bool ucbAvConsole = false;
            //  2017/08/25
            ushort usConfType = 0;



            //  2016/03/13
            //if (pPst != null)
            {
                //  2017/08/02
                ucbVideoConference = pPst.i.ucbVideoConference;
                //  2017/07/17
                ucbAvConsole = pPst.i.ucbAvConsole;
                //  2017/08/25
                usConfType = pPst.i.usConfType;
                //
                //memset(&pPst->o, 0, sizeof(pPst->o)  );
                pPst.o = new P_sendAvInfo_taskInfo_o();
            }


            //  2013/02/11
            if (pRuleU != null)
            {
                if (pRuleU->uiType1 == Consts.CONST_talkerObjType_dynBmp)
                {
                    pDynBmp = &pRuleU->dynBmpRule;
                    //
                    if (0 != pRuleU->taskInfo.iTaskId)
                    {
                        if (uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                        {
                            bReSend_viewDynBmp = true;
                        }
                    }
                }
            }

            //
            int iIndex_taskInfo = -1;




            //  2014/10/29
            if (null != piTaskId)
            {
                if (0 != *piTaskId)
                {
                    iTaskId_suggested = *piTaskId;
                }
            }
            if (bReSend_viewDynBmp)
            {
                iTaskId_suggested = pRuleU->taskInfo.iTaskId;
            }

            //
            if (!bReSend_viewDynBmp)
            {
                uint uiTaskDataType = 0;
                switch (uiTaskType)
                {
                    case Consts.CONST_imTaskType_transferAvInfo:
                        uiTaskDataType = Consts.CONST_taskDataType_conf;
                        break;
                    default:
                        break;
                }
                iIndex_taskInfo = newTaskInfoIndex(ctxCaller, pProcInfo, uiTaskDataType, hWnd_task, iTaskId_suggested, "procSendAvInfoFunc");
                if (iIndex_taskInfo < 0) goto errLabel;
            }
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller, pProcInfo, iIndex_taskInfo, "procSendAvInfoFunc");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;
            if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            //
            switch (uiTaskType)
            {
                case Consts.CONST_imTaskType_transferAvInfo:
                    ucbSendLocalAv = true;
                    break;
                default:
                    break;
            }


            pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
            if (null == pSharedObj)
            {
                if (uiTaskType != Consts.CONST_imTaskType_viewDynBmp) goto errLabel;
            }
            else
            {       //  2010/09/04
                pSharedObjUsr = getSharedObjUsr(pSharedObj, iIndex_sharedObjUsr);
                if (null == pSharedObjUsr) goto errLabel;
                //
                if (0 != pCompressAudio.uiTranNo_openAvDev_org)
                {
                    pSharedObjUsr.uiTranNo_openAvDev_a = (uint)(pCompressAudio.uiTranNo_openAvDev_org + iIndex_sharedObjUsr); //  pSharedObj->iIndex_curUsr;
                }
                if (0 != pCompressVideo.uiTranNo_openAvDev_org)
                {   //  
                    pSharedObjUsr.uiTranNo_openAvDev_v = (uint)(pCompressVideo.uiTranNo_openAvDev_org + iIndex_sharedObjUsr); //  pSharedObj->iIndex_curUsr;
                }
            }

            Int64 tStartTran; uint uiTranNo;
            MACRO_prepareForTran(out tStartTran, out uiTranNo);

            //  2013/02/13
            if (bReSend_viewDynBmp)
            {
                tStartTran = pRuleU->taskInfo.tStartTran;
                uiTranNo = pRuleU->taskInfo.uiTranNo;
            }

            //
            TASK_transferAv task = new TASK_transferAv();
            task.m_index_taskInfo = iIndex_taskInfo;

            //  
            fixed (MIS_MSG_TASK* pMsg = &pTaskData.msgU.task)
            {
                //if (!(pMsg = (MIS_MSG_TASK*)mallocObj.mallocf(sizeof(MIS_MSG_TASK)))) goto errLabel;
                //TRANSFER_AV_task* pContent = null;
                TRANSFER_AV_INFO* pContent_req = null;
                ref PROC_TASK_AV pContent_myTask = ref task.myTask;

                //
                uint lenInBytes;
                IM_TASK_RCD rcd;
                //
                MACRO_memset_misMsgTask(ref *pMsg);
                pMsg->uiType = Consts.CONST_misMsgType_task;
                //
                //pMsg->pMisCnt = pMisCnt;
                //
                pMsg->ucFlg = Consts.CONST_commFlg_talkData;
                //
                pMsg->usCode = Consts.CONST_qyCmd_sendMedia;
                pMsg->tStartTime = tStartTran;
                pMsg->uiTranNo = uiTranNo;
                //
                //if (sizeof(TRANSFER_AV_task) > sizeof(pMsg->data.buf)  )  goto errLabel;
                pContent_req = (TRANSFER_AV_INFO*)&pMsg->data.buf;
                mymemset((IntPtr)pContent_req, 0, sizeof(TRANSFER_AV_INFO));
                pContent_req->uiType = Consts.CONST_imCommType_transferAvInfo;
                //  2016/04/18
                pContent_req->tmp_usSubtype = Consts.CONST_transferAvInfo_subtype_task;
                //
                pContent_req->uiTaskType = uiTaskType;

                pContent_myTask.ucbSendLocalAv = ucbSendLocalAv;                                   //  2008/06/12
                pContent_myTask.iIndex_sharedObj = iIndex_sharedObj;
                //  2014/12/11
                pContent_myTask.iIndex_sharedObjUsr = iIndex_sharedObjUsr;                         //  2014/12/11

                //  2018/09/08
                pContent_myTask.uiTaskType = uiTaskType;
                //
                pContent_myTask.ucbStarter = true;                                             //  2010/08/31
                                                                                               //  2014/09/23
                string verStr = qnmVerStr(pQyMc.iServiceId);
                TMP_buf256 buf256;
                int bufSize = 256;
                myWChar2Utf8(verStr, buf256.buf, (uint)bufSize);
                if (0 != parse_qnmVerStr(buf256.buf, &pContent_req->ass.tranInfo.head.shClientVer, null, null)) goto errLabel;
                //
                if (null == pVideoConferenceCfg)
                {
                    switch (uiTaskType)
                    {
                        case Consts.CONST_imTaskType_transferAvInfo:
                            //
                            videoConference.usMaxSpeakers = 2;
                            //
                            videoConference.usCntLimit_activeMems_from = (ushort)(videoConference.usMaxSpeakers + Consts.MAX_resourceMems_taskAv_p2p);

                            //
                            videoConference.usCntLimit_mems_from = 2;                      //  2010/08/31


                            break;
                        default:

                            //																					   //
                            videoConference.usCntLimit_activeMems_from = 1;

                            //
                            videoConference.usCntLimit_mems_from = 2;                  //  2010/09/03

                            break;
                    }
                }
                else
                {
                    //memcpy(&pContent_myTask.videoConferenceCfg, pVideoConferenceCfg, sizeof(pVideoConferenceCfg[0]));
                    pContent_myTask.videoConferenceCfg = *pVideoConferenceCfg;

                    //
                    videoConference.usMaxSpeakers = pContent_myTask.videoConferenceCfg.usMaxSpeakers;
                    //
                    videoConference.usCntLimit_activeMems_from = (ushort)(pContent_myTask.videoConferenceCfg.usMaxSpeakers + Consts.MAX_mosaicMems_taskAv + Consts.MAX_resourceMems_taskAv_conf);

                    //
                    videoConference.usCntLimit_mems_from = (ushort)(pContent_myTask.videoConferenceCfg.iRows * pContent_myTask.videoConferenceCfg.iCols);

                    //
                    pTc.ucbVideoConference = true;                 //  2009/05/30
                    pContent_myTask.ucbVideoConferenceStarter = true;
                }
                if (0 == videoConference.usCntLimit_mems_from || videoConference.usCntLimit_mems_from > Consts.MAX_mems_taskAv) goto errLabel;
                //  2017/08/02
                if (pContent_myTask.bVideoConference() != ucbVideoConference)
                {
                    //showInfo_open(0, 0, _T("procSendAvInfoFunc: ucbVideoConference err"));
                    goto errLabel;
                }

                //
                //if (pAddr)
                {
                    if (pAddr.uiObjType == Consts.CONST_objType_imGrp)
                    {
                        pContent_req->bGrpTask = true; //  2007/09/05, ¸³ÉÏ×é±êÊ¶									   
                    }
                }

                //  2017/08/02
                pContent_req->confCfg.usMaxSpeakers = pContent_myTask.videoConferenceCfg.usMaxSpeakers;

                pContent_req->confCfg.ucbVideoConference = (byte)(ucbVideoConference ? 1 : 0);
                //  2017/07/17
                //pContent_req->confCfg.ucbAvConsole = (byte)(ucbAvConsole?1:0);
                //  2017/08/25
                pContent_req->confCfg.usConfType = usConfType;

                //  2017/08/26
                //if (null!=pPst)
                {
                    /*
					int i;
					int j;
					for (i = 0; i < mycountof(pPst->i.info_consult_yz.mems); i++)
					{
						if (!pPst->i.info_consult_yz.mems[i].idInfo.ui64Id) break;
						//
						for (j = 0; j < mycountof(pContent_req->confCfg.info_consult_yz.mems); j++)
						{
							if (pContent_req->confCfg.info_consult_yz.mems[j].idInfo.ui64Id) continue;
							pContent_req->confCfg.info_consult_yz.mems[j] = pPst->i.info_consult_yz.mems[i];
							break;
						}
						//
						continue;
					}
					*/

                    //
                    //pContent_myTask.shared_eglContext = pPst.i.shared_eglContext;
                    pTc.shared_eglContext = pPst.i.shared_eglContext;
                }


                // audio
                if (!pContent_myTask.ucbVideoConferenceStarter)
                {
                    if (0 != pCompressVideo.uiTranNo_openAvDev_org)
                    {   //  ×Ô¼ºÃ»ÓÐÉãÏàÍ¶Ê±£¬ÕâÀïÊÇÓÃÀ´ÇëÇó¿´Ô¶³ÌµÄÊÓÆµÍ·
                        if (null == pSharedObjUsr) goto errLabel;
                        //  pContent->tranInfo.video.uiTranNo_openAvDev  =  pCompressVideo.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;
                        pContent_req->ass.tranInfo.video.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;

                        //			
                        //  safeStrnCpy(  pCompressVideo.compressor.fourccStr,  pContent->tranInfo.video.fourccStr,  mycountof(  pContent->tranInfo.video.fourccStr  )  );
                        //memcpy(&pContent_req->tranInfo.video.compressor, &pCompressVideo.compressor, sizeof(pContent_req->tranInfo.video.compressor)  );
                        pContent_req->ass.tranInfo.video.compressor = pCompressVideo.compressor.common.head;

                        //memcpy(&pContent_req->tranInfo.video.vh_compress, &pCompressVideo.vh_compress, sizeof(pContent_req->tranInfo.video.vh_compress)  );
                        pContent_req->ass.tranInfo.video.vh_compress = pCompressVideo.vh_compress;
                        //memcpy(&pContent_req->tranInfo.video.vh_stream, &pCompressVideo.vh_stream, sizeof(pContent_req->tranInfo.video.vh_stream)  );      //  2014/03/30
                        pContent_req->ass.tranInfo.video.vh_stream = pCompressVideo.vh_stream;
                        //
                        //memcpy(&pContent_req->tranInfo.video.vh_decompress, &pCompressVideo.vh_decompress, sizeof(pContent_req->tranInfo.video.vh_decompress)  );
                        pContent_req->ass.tranInfo.video.vh_decompress = pCompressVideo.vh_decompress;
                    }

                    if (0 != pCompressAudio.uiTranNo_openAvDev_org)
                    {
                        if (null == pSharedObjUsr) goto errLabel;
                        //  pContent->tranInfo.audio.uiTranNo_openAvDev  =  pCompressAudio.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;
                        pContent_req->ass.tranInfo.audio.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_a;
                        //
                        //  memcpy(  &pContent->tranInfo.audio.wf_compress,  &pCompressAudio.policy.compressor.wf_compress,  sizeof(  pContent->tranInfo.audio.wf_compress  )  );
                        //  memcpy(  &pContent->tranInfo.audio.wf_decompress,  &pCompressAudio.waveFormatEx,  sizeof(  pContent->tranInfo.audio.wf_decompress  )  );	//  2009/04/27
                        //memcpy(&pContent_req->tranInfo.audio.compressor, &pCompressAudio.compressor, sizeof(pContent_req->tranInfo.audio.compressor)  );
                        pContent_req->ass.tranInfo.audio.compressor = pCompressAudio.compressor.common.head;

                        //memcpy(&pContent_req->tranInfo.audio.ah_compress, &pCompressAudio.ah_compress, sizeof(pContent_req->tranInfo.audio.ah_compress)  );
                        pContent_req->ass.tranInfo.audio.ah_compress = pCompressAudio.ah_compress;

                        //memcpy(&pContent_req->tranInfo.audio.ah_stream, &pCompressAudio.ah_stream, sizeof(pContent_req->tranInfo.audio.ah_stream)  );          //  2016/12/23
                        //pContent_req->ass.tranInfo.audio.ah_stream = pCompressAudio.ah_stream;
                        //
                        //memcpy(&pContent_req->tranInfo.audio.ah_decompress, &pCompressAudio.ah_decompress, sizeof(pContent_req->tranInfo.audio.ah_decompress)  );
                        pContent_req->ass.tranInfo.audio.ah_decompress = pCompressAudio.ah_decompress;
                    }

                    if (pDynBmp != null)
                    {       //  2009/09/07
                            //
                            //  2016/07/24
                        pContent_req->ass.resObj = pDynBmp->dynBmp.resObj;
                        //
                        //  2014/08/04
                        pContent_req->viewDynBmp.ucbSaveVideo = (byte)(pDynBmp->ucbSaveVideo ? 1 : 0);
                    }

                    //
                    switch (uiTaskType)
                    {
                        case Consts.CONST_imTaskType_viewDynBmp:
                            break;
                        default:
                            //pTc->videoConference.activeMems_from[0].avStream.idInfo  =  pMisCnt->idInfo;		
                            //pTc->videoConference.activeMems_from[0].avStream.obj.tranInfo  =  pContent_req->tranInfo;
                            AV_stream_simple ass; mymemset((IntPtr)(byte*)&ass, 0, sizeof(AV_stream_simple));
                            ass.tranInfo = pContent_req->ass.tranInfo;
                            addTo_activeMems_from(pMisCnt, pMisCnt.idInfo, &ass, pTc);
                            break;
                    }


                }
                else
                {
                    /*
					if (null==pAvCompressor_conf)
					{
#if __DEBUG__
					traceLogA("procSendAvInfo: pVideoCompressor_conf can't be null for a conference starter");
#endif
						goto errLabel;
					}
					*/
                    //  2014/11/07
                    /*
					if (!pCompressAudio || !pCompressVideo)
					{
#if __DEBUG__
				traceLogA("procSendAvInfo: pVideoConferenceCompressor can't be null for a conference starter");
#endif
						goto errLabel;
					}
					*/


                    //  2009/04/19
                    pContent_req->confCfg.conf.usMaxFps_toShareBmp_conf = pAvCompressor_conf.video.common.head.usMaxFps_toShareBmp; //  pProcInfo->cfg.policy.video.usFrames_perSecond_videoConference;
                                                                                                                                    //  2014/08/15
                    pContent_req->confCfg.conf.iWidth_pic_conf = pAvCompressor_conf.video.common.head.iWidth_pic;
                    pContent_req->confCfg.conf.iHeight_pic_conf = pAvCompressor_conf.video.common.head.iHeight_pic;
                    //
                    //  2014/11/09
                    //pContent->confCfg.audioPlayCfg  =  pAvCompressor_conf->audio.playCfg;
                    //
                    //  2016/03/26
                    if (pTalkerPolicy != null)
                    {

                        //  2017/07/06
                        pContent_req->confCfg.conf.confCtrl.ucbConfCtrl_starter = (byte)(pTalkerPolicy->ucbConfCtrl_starter ? 1 : 0);

                    }

                    //
                    //  Note: 会议里的tranInfo起的是组优先策略的作用，不是发一个视频信息. 2014/11/13

                    //
                    int tt = 0;
                    pContent_req->ass.tranInfo.video.uiTranNo_openAvDev = getuiNextTranNo(null, 0, ref tt);
                    //

                    //
                    pContent_req->ass.tranInfo.audio.uiTranNo_openAvDev = pContent_req->ass.tranInfo.video.uiTranNo_openAvDev;
                    //
                    //  2014/11/13
                    //AUDIO_PLAY_CFG* pAudioPlayCfg_conf = &pAvCompressor_conf->audio.playCfg;
                    //pContent_req->tranInfo.audio.compressor.playCfg = *pAudioPlayCfg_conf;


                    //  2009/06/03
                    //if (pMosaic)
                    {
                        if (0 != pMosaic.video.resObj.uiObjType)
                        {
                            if (0 != getMosaicTranInfo(ref pMosaic.video, &pContent_req->confMosaicTranInfo_video)) goto errLabel;
                        }
                        if (0 != pMosaic.resource.resObj.uiObjType)
                        {
                            if (0 != getMosaicTranInfo(ref pMosaic.resource, &pContent_req->confMosaicTranInfo_resource)) goto errLabel;
                        }
                    }

                }
                //  2011/12/10

                //  2014/09/27
                if (p_content_idInfo_logicalPeer != null)
                {
                    pContent_req->idInfo_logicalPeer.ui64Id = p_content_idInfo_logicalPeer->ui64Id;
                }
                //  2014/11/22
                pContent_req->uiTranNo_avTask_starter = uiTranNo_avTask_starter;

                //  2015/03/11
                if (pTaskAvProps != null)
                {
                    //memcpy(&pContent_req->tranInfo.taskAvProps, pTaskAvProps, sizeof(pContent_req->tranInfo.taskAvProps)  );
                    pContent_req->ass.tranInfo.taskAvProps = *pTaskAvProps;
                }

                //  
                pContent_myTask.mem0_from.idInfo.ui64Id = pMisCnt.idInfo.ui64Id;
                pContent_myTask.mem0_from.tStartTime_task = tStartTran;
                //
                if (0 != pCompressVideo.uiTranNo_openAvDev_org)
                {
                    //													 
                    if (null == pSharedObjUsr) goto errLabel;
                    //  pContent_myTask.mem0_from.video.uiTranNo_openAvDev  =  pCompressVideo.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;						//  
                    pContent_myTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;                                             //  
                                                                                                                                                                  //

                    //  safeStrnCpy(  pCompressVideo.compressor.fourccStr,  pContent_myTask.mem0_from.video.fourccStr,  mycountof(  pContent_myTask.mem0_from.video.fourccStr  )  );
                    //memcpy(&pContent_myTask.mem0_from.video.tv_recvd.compressor, &pCompressVideo.compressor, sizeof(pContent_myTask.mem0_from.video.tv_recvd.compressor)  );
                    pContent_myTask.mem0_from.video.tv_recvd.compressor = pCompressVideo.compressor.common.head;

                    //memcpy(&pContent_myTask.mem0_from.video.tv_recvd.vh_compress, &pCompressVideo.vh_compress, sizeof(pContent_myTask.mem0_from.video.tv_recvd.vh_compress)  );
                    pContent_myTask.mem0_from.video.tv_recvd.vh_compress = pCompressVideo.vh_compress;
                    //  
                    if (bSupported_sendVhDec())
                    {
                        //memcpy(&pContent_myTask.mem0_from.video.tv_recvd.vh_decompress.bih, &pCompressVideo.vh_decompress, sizeof(pContent_myTask.mem0_from.video.tv_recvd.vh_decompress.bih)  );
                        pContent_myTask.mem0_from.video.tv_recvd.vh_decompress.bih = pCompressVideo.vh_decompress.bih;
                    }

                    //  2014/02/25
                    //memcpy(&pContent_myTask.mem0_from.video.tv_recvd.vh_stream, &pCompressVideo.vh_stream, sizeof(pContent_myTask.mem0_from.video.tv_recvd.vh_stream)  );
                    pContent_myTask.mem0_from.video.tv_recvd.vh_stream = pCompressVideo.vh_stream;
                }
                //	
                if (0 != pCompressAudio.uiTranNo_openAvDev_org)
                {
                    if (null == pSharedObjUsr) goto errLabel;
                    //  pContent_myTask.mem0_from.audio.uiTranNo_openAvDev  =  pCompressAudio.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;
                    pContent_myTask.mem0_from.audio.ta_recvd.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_a;
                    //		
                    //  memcpy(  &pContent_myTask.mem0_from.audio.wf_compress_recvd,  &pCompressAudio.policy.compressor.wf_compress,  sizeof(  pContent_myTask.mem0_from.audio.wf_compress_recvd  )  );
                    //  memcpy(  &pContent_myTask.mem0_from.audio.wf_decompress_recvd,  &pCompressAudio.waveFormatEx,  sizeof(  pContent_myTask.mem0_from.audio.wf_decompress_recvd  )  );
                    //memcpy(&pContent_myTask.mem0_from.audio.ta_recvd.compressor, &pCompressAudio.compressor, sizeof(pContent_myTask.mem0_from.audio.ta_recvd.compressor)  );
                    pContent_myTask.mem0_from.audio.ta_recvd.compressor = pCompressAudio.compressor.common.head;
                    //memcpy(&pContent_myTask.mem0_from.audio.ta_recvd.ah_compress, &pCompressAudio.ah_compress, sizeof(pContent_myTask.mem0_from.audio.ta_recvd.ah_compress)  );
                    pContent_myTask.mem0_from.audio.ta_recvd.ah_compress = pCompressAudio.ah_compress;
                    //memcpy(&pContent_myTask.mem0_from.audio.ta_recvd.ah_decompress, &pCompressAudio.ah_decompress, sizeof(pContent_myTask.mem0_from.audio.ta_recvd.ah_decompress)  );
                    pContent_myTask.mem0_from.audio.ta_recvd.ah_decompress = pCompressAudio.ah_decompress;

                    //  2016/12/23
                    //memcpy(&pContent_myTask.mem0_from.audio.ta_recvd.ah_stream, &pCompressAudio.ah_stream, sizeof(pContent_myTask.mem0_from.audio.ta_recvd.ah_stream)  );
                    pContent_myTask.mem0_from.audio.ta_recvd.ah_stream = pCompressAudio.ah_stream; ;
                }

                if (!pContent_myTask.ucbVideoConferenceStarter)
                {

                    //  2012/01/27
                    //memcpy(&pContent_myTask.audio.playCfg, &pContent_req->tranInfo.audio.compressor.playCfg, sizeof(pContent_myTask.audio.playCfg)  );
                    //pContent_myTask.audio.playCfg = pContent_req->tranInfo.audio.compressor.playCfg;

                }
                else
                {

                    //  2009/06/03		
                    //memcpy(&pContent_myTask.audio.playCfg, &pContent_req->tranInfo.audio.compressor.playCfg, sizeof(pContent_myTask.audio.playCfg)  );
                    //pContent_myTask.audio.playCfg = pContent_req->tranInfo.audio.compressor.playCfg;

                    //  2009/06/04
                    pContent_myTask.video.usMaxFps_toShareBmp_conf = pContent_req->confCfg.conf.usMaxFps_toShareBmp_conf;
                }
                pContent_myTask.hWnd_task = hWnd_task; //  2009/09/09
                                                       //  2014/11/11
                                                       //if (pMosaic)
                {
                    if (0 != pMosaic.video.resObj.uiObjType)
                    {
                        pContent_myTask.confMosaicMaker.iIndex_sharedObj_video = pMosaic.video.var.iIndex_sharedObj;
                        pContent_myTask.confMosaicMaker.iIndex_sharedObj_resource = pMosaic.resource.var.iIndex_sharedObj;
                        //
                        pContent_myTask.confMosaicMaker.bExists_confMosaic = true;
                    }
                }

                //  2007/07/18
                pMsg->uiTaskType = uiTaskType;
                //
                pMsg->idInfo_taskSender.ui64Id = pMisCnt.idInfo.ui64Id;
                //if (pAddr)
                {
                    pMsg->idInfo_taskReceiver.ui64Id = pAddr.idInfo.ui64Id;
                    pMsg->addr_logicalPeer.idInfo.ui64Id = pAddr.idInfo.ui64Id;
                }
                if (null != p_content_idInfo_logicalPeer && 0 != p_content_idInfo_logicalPeer->ui64Id)
                {       //  2014/11/22
                    pMsg->addr_logicalPeer.idInfo.ui64Id = p_content_idInfo_logicalPeer->ui64Id;
                }
                //
                pMsg->iStatus = Consts.CONST_imTaskStatus_req;
                qyFuncs.traceLog("##666");

                //  ÕâÀïµÄ¸³ÖµËÆºõÓ¦ÔÚ·¢ËÍÊ±Ê¹ÓÃ, 2007/07/24
                pMsg->data.route.idInfo_from.ui64Id = pMsg->idInfo_taskSender.ui64Id;
                pMsg->data.route.idInfo_to.ui64Id = pMsg->idInfo_taskReceiver.ui64Id;
                //

                lenInBytes = (uint)sizeof(TRANSFER_AV_INFO);
                pMsg->lenInBytes = (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf") + lenInBytes;

                mymemset((IntPtr)(byte*)&rcd, 0, sizeof(IM_TASK_RCD));
                if (0!= msg2TaskRcd(pMisCnt, ref *pMsg, Consts.CONST_qyRc_unknown, ref rcd)) goto errLabel;

                {
                    object pDb = null;

                    using (CQnmDb db = new CQnmDb())
                    {
                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                        pDb = db.m_pDbMem.pDb;
                        //
                        if (storeTaskInDb(pDb, pQyMc.cfg.db.iDbType, pMisCnt, iTaskId_suggested, ref rcd, false, "procSendAvInfoFunc") < 0) goto errLabel;
                    }
                }

                //  2013/02/13
                if (iTaskId_suggested != 0)
                {       //  2014/10/29
                    if (rcd.id != iTaskId_suggested)
                    {
#if __DEBUG__
		traceLog(_T("procSendAvInfo failed, rcd.id != taskInfo.iTaskId"));
#endif
                        goto errLabel;
                    }
                }
                //
                if (uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                {
                    if (pRuleU != null && pRuleU->uiType1 == Consts.CONST_talkerObjType_dynBmp)
                    {
                        if (pRuleU->taskInfo.bNeedProgress)
                        {
                            //startProgress(uiTranNo);
                        }
                    }
                }
                //  2014/09/24
                /*
				if (!bReSend_viewDynBmp)
				{
					uint uiTaskDataType = 0;
					switch (uiTaskType)
					{
						case Consts.CONST_imTaskType_transferAvInfo:
							uiTaskDataType = Consts.CONST_taskDataType_conf;
							break;
						default:
							break;
					}
					pContent_myTask.iIndex_taskInfo = newTaskInfoIndex(pProcInfo, uiTaskDataType, hWnd_task, rcd.id);
					if (pContent_myTask.iIndex_taskInfo < 0) goto errLabel;
				}
				*/
                pContent_myTask.iIndex_taskInfo = iIndex_taskInfo;

                //
                pMsg->iTaskId = rcd.id;
                pMsg->uiTaskMode_unused = rcd.uiMode;
                pMsg->uiChannelType = Consts.CONST_channelType_media;      //  2009/10/17

                //



                //
                if (bDlgTalk)
                {
                    /*
					CHelp_getDlgTalkVar help_getDlgTalkVar;
					DLG_TALK_var* pm_var = (DLG_TALK_var*)help_getDlgTalkVar.getVar(hWnd_task);
					if (!pm_var) goto errLabel;
					DLG_TALK_var & m_var = *pm_var;
					*/

                    if (uiTaskType != Consts.CONST_imTaskType_viewDynBmp)
                    {   //  2014/09/26

                        //
                        MIS_MSGU* pMsgU = (MIS_MSGU*)pMsg;
                        if (0 != GuiShare.pf_dlgTalk_qPostMsg(hWnd_task, ref *pMsgU, (uint)Marshal.OffsetOf<MIS_MSG_TASK>("data") + pMsg->lenInBytes)) goto errLabel;

                        /*
						//  2016/02/27
						PARAM_PROC_ROW param;
						memset(&param, 0, sizeof(param));
						param.bScrollIntoView = true;
						if (dyn_tmpHandler_showMsg(hWnd_task, &param, pMsg)) goto errLabel;
						*/
                    }

                    //


                }


                ///////////////
                //

                //  2009/04/28
                if (pSharedObj != null)
                {
                    if (null == pSharedObjUsr) goto errLabel;

                    pSharedObjUsr.iTaskId = rcd.id;
                    //
                    //if (pAddr)
                    {
                        pSharedObjUsr.idInfo_to.ui64Id = pAddr.idInfo.ui64Id;         //  2010/09/02
                        pSharedObjUsr.ucbVideoConferenceStarter = pContent_myTask.ucbVideoConferenceStarter;
                        pSharedObjUsr.ucbVideoConference = pContent_myTask.bVideoConference();        //  2018/11/10
                                                                                                      //
                        pTc.ucbNotSendData = (ucbSendLocalAv && pAddr.uiObjType == Consts.CONST_objType_imGrp);
                    }
                }
                if (piTaskId != null) *piTaskId = rcd.id;

                //
                if (0 != pProcInfo.processQ_media.qPostMsg(task, 0))
                {
                    traceLogA("procMsgTask_mc_robot: 1. qPostMsg failed");
                    goto errLabel;
                }
                if (task.myTask.ucbVideoConferenceStarter)
                {
                    //
                    PARAM_findOrgMsg param = new PARAM_findOrgMsg();

                    //  ÔÙ²éÑ¯Ò»´ÎprocessQ_media, ÒòÎªÊÓÆµ»áÒéµÄbmpInfo_compressµÈ£¬ÊÇÔÚÈë¶ÓÁÐÊ±²ÅÉú³ÉµÄ¡£Òª´ÓÀïÃæÈ¡³öÀ´
                    if (pProcInfo.processQ_media.qTraverse(ctxCaller, tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsg, param) <= 0) goto errLabel;
                }

                //
#if DEBUG
                ref TRANSFER_AV_INFO content_req = ref *pContent_req;
#endif


                //  2015/09/06
                uint uiChannelType = Consts.CONST_channelType_media;
                //  
                uiChannelType = Consts.CONST_channelType_talking;
                //
                QY_MESSENGER_ID idInfo_dst; idInfo_dst.ui64Id = pAddr.idInfo.ui64Id;
                if (0 != postMsgTask2Mgr_mc(ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (byte*)pContent_req, lenInBytes, pMsg->addr_logicalPeer.idInfo.ui64Id, pMsg->idInfo_taskSender.ui64Id, pMsg->idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;

                //  2009/02/20
                if (bDlgTalk)
                {

                    if (uiTaskType == Consts.CONST_imTaskType_transferAvInfo
                        //&& pPst!=null
                        )
                    {
                        //
                        //  2016/03/14

                        //
                        pPst.o.iTaskId = rcd.id;
                        //  m_var.av.taskInfo.iIndex_sharedObj  =  iIndex_sharedObj;
                        pPst.o.idInfo_starter.ui64Id = rcd.idInfo_send.ui64Id;

                        pPst.o.tStartTime_org = rcd.tSendTime;
                        pPst.o.uiTranNo_org = rcd.uiTranNo;
                        pPst.o.uiContentType_org = rcd.uiContentType;

                        pPst.o.ucbStarter = pContent_myTask.ucbStarter;                                   //  2010/08/31
                        pPst.o.ucbVideoConference = pContent_myTask.bVideoConference();                   //  2010/08/31
                                                                                                          //  m_var.av.taskInfo.ucbVideoConferenceStarter  =  pContent_myTask.ucbVideoConferenceStarter;

                        //
                        pPst.o.uiInitW = (uint)pContent_req->ass.tranInfo.video.vh_decompress.bih.biWidth;
                        pPst.o.uiInitH = (uint)pContent_req->ass.tranInfo.video.vh_decompress.bih.biHeight;

                        //  2014/09/25
                        pPst.o.iIndex_taskInfo = pContent_myTask.iIndex_taskInfo;


                        //  2010/08/31

                        //

                        //
                        //  if  (  pContent_myTask.ucbVideoConferenceStarter  )  
                        {
                            //
                            pPst.o.local_avStream.tranInfo.audio = pContent_myTask.mem0_from.audio.ta_recvd;
                            pPst.o.local_avStream.tranInfo.video = pContent_myTask.mem0_from.video.tv_recvd;

                        }


                    }
                }

                //  2013/02/13
                if (uiTaskType == Consts.CONST_imTaskType_viewDynBmp)
                {
                    if (pRuleU != null && pRuleU->uiType1 == Consts.CONST_talkerObjType_dynBmp)
                    {
                        if (!bReSend_viewDynBmp)
                        {  //  新任务才需要设置
                            pRuleU->taskInfo.iTaskId = rcd.id;
                            pRuleU->taskInfo.tStartTran = tStartTran;
                            pRuleU->taskInfo.uiTranNo = uiTranNo;
                        }
                    }
                }

            }

            //
            /*
            _sntprintf(tBuf, mycountof(tBuf), _T("Start AV:"));
            if (pCompressVideo) _sntprintf(tBuf, mycountof(tBuf), _T("%s video %d, maxFps %d, minFps %d."), tBuf, pCompressVideo.uiTranNo_openAvDev_org, (int)pCompressVideo.compressor.common.usMaxFps_toShareBmp, (int)pCompressVideo.compressor.common.usMinFps_toShareBmp);
            if (pCompressAudio) _sntprintf(tBuf, mycountof(tBuf), _T("%s audio %d"), tBuf, pCompressAudio.uiTranNo_openAvDev_org);
            qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("%s"), tBuf);
            */


            iErr = 0;
        errLabel:

            //  2013/02/13
            if (iErr != 0)
            {
                if (bReSend_viewDynBmp)
                {  //  重发viewDynBmp如果失败
                }
            }

            return iErr;
        }


        //

        // 
        public static unsafe int procSendAvReplyInfo(MIS_CNT pMisCnt, object hWnd_task, MIS_MSG_TASK* pMsgTask, bool ucbSendLocalAv, int iIndex_sharedObj, DYN_BMP_RULE* pDynBmp, TASK_av_props* pTaskAvProps, MSGR_ADDR* pAddr)
        {
            int iErr = -1;
            CCtxQmc pProcInfo = null;
            CCtxQyMc pQyMc = g.g_pQyMc;
            IM_CONTENTU* pContent = null;

            if (null == pMisCnt) return -1;
            pProcInfo = pQyMc.get_pProcInfo();    //  (  MC_VAR_isCli  *  )pMisCnt.pProcInfoParam;

            pContent = (IM_CONTENTU*)&pMsgTask->data.buf;

            //TRANSFER_AV_reply_task transferAvReplyInfo;
            int lenInBytes = 0;
            QY_SHARED_OBJ pSharedObj = null;
            SHARED_OBJ_USR pSharedObjUsr = null;

            /*

	pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
	if (ucbSendLocalAv)
	{
		if (!pSharedObj) goto errLabel;
		pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj->iIndex_curUsr);
		if (!pSharedObjUsr) goto errLabel;
		//
		if (!pDynBmp || pDynBmp->dynBmp.resObj.uiObjType != CONST_objType_avStream)
		{
			if (pSharedObjUsr.hWnd_starter != hWnd_task)
			{
				showInfo_open(0, 0, _T("procSendAvReplyInfo failed, cur_sharedObjUsr.hWnd != hWnd_task"));
				goto errLabel;
			}
		}
	}

	//  ·¢ËÍ½ÓÊÕÔÊÐí¸ø¶Ô·½
	memset(&transferAvReplyInfo, 0, sizeof(transferAvReplyInfo));
	transferAvReplyInfo.req.uiType = CONST_imCommType_transferAvReplyInfo;
	//  2016/04/18
	transferAvReplyInfo.req.tmp_usSubtype = CONST_transferAvReplyInfo_subtype_task;
	//
	transferAvReplyInfo.req.usOp = CONST_imOp_recv_accept;
	transferAvReplyInfo.req.tStartTime_org = pMsgTask->tStartTime;
	transferAvReplyInfo.req.uiTranNo_org = pMsgTask->uiTranNo;
	//
	if (ucbSendLocalAv)
	{   //  ±¾µØÊÓÆµÒ²ÔÊÐí·¢ËÍµÄÇé¿öÏÂ£¬Ò²·¢ËÍ¸ø¶Ô·½¡£
		CAP_procInfo_bmpU* pCapBmp = getCapBmpBySth(pProcInfo, pSharedObj->iIndex_capBmp, 0);
		CAP_procInfo_audioU* pCapAudio = getCapAudioBySth(pProcInfo, pSharedObj->iIndex_capAudio, 0);
		COMPRESS_VIDEO* pCompressVideo = null;  //  &pProcInfo->av.localAv.videoCaptureProcInfo.compressVideo;
		COMPRESS_AUDIO* pCompressAudio = null;  //  &pProcInfo->av.localAv.recordSoundProcInfo.compressAudio;

		if (pCapBmp) pCompressVideo = &pCapBmp->common.compressVideo;
		if (pCapAudio) pCompressAudio = &pCapAudio->common.compressAudio;

		if (pCompressVideo)
		{
			//  2009/04/19
			if (pContent->transferAvInfo.confCfg.conf.usMaxFps_toShareBmp_conf)
			{
				//  pCompressVideo.usMaxFps_toShareBmp  =  min(  pContent->transferAvInfo.tranInfo.video.usMaxFps_toShareBmp_videoConference,  pCompressVideo.usMaxFps_toShareBmp  );    
				//  2009/06/04
				transferAvReplyInfo.myTask.video.usMaxFps_toShareBmp_conf = pCompressVideo.compressor.common.usMaxFps_toShareBmp;
			}

			//
			//  transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev  =  pCompressVideo.uiTranNo_openAvDev;
			transferAvReplyInfo.req.tranInfo.video.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;
			//
#if  0	//  2014/11/07
				 transferAvReplyInfo.tranInfo.video.usCapDriverIndex  =  pCompressVideo.usCapDriverIndex;
#endif
			//  safeStrnCpy(  pCompressVideo.compressor.fourccStr,  transferAvReplyInfo.tranInfo.video.fourccStr,  mycountof(  transferAvReplyInfo.tranInfo.video.fourccStr  )  );
			memcpy(&transferAvReplyInfo.req.tranInfo.video.compressor, &pCompressVideo.compressor, sizeof(transferAvReplyInfo.req.tranInfo.video.compressor));
			memcpy(&transferAvReplyInfo.req.tranInfo.video.vh_compress, &pCompressVideo.vh_compress, sizeof(transferAvReplyInfo.req.tranInfo.video.vh_compress));
			//  2014/03/31
			memcpy(&transferAvReplyInfo.req.tranInfo.video.vh_stream, &pCompressVideo.vh_stream, sizeof(transferAvReplyInfo.req.tranInfo.video.vh_stream));
			//  
			memcpy(&transferAvReplyInfo.req.tranInfo.video.vh_decompress.bih, &pCompressVideo.vh_decompress.bih, sizeof(transferAvReplyInfo.req.tranInfo.video.vh_decompress.bih));

			//  

		}
		if (pCompressAudio)
		{
			//
			//  transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev  =  pCompressAudio.uiTranNo_openAvDev;
			transferAvReplyInfo.req.tranInfo.audio.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_a;
			//  memcpy(  &transferAvReplyInfo.tranInfo.audio.wf_compress,  &pProcInfo->av.localAv.recordSoundProcInfo.compressAudio.policy.compressor.wf_compress,  sizeof(  transferAvReplyInfo.tranInfo.audio.wf_compress  )  );
			//  memcpy(  &transferAvReplyInfo.tranInfo.audio.wf_decompress,  &pProcInfo->av.localAv.recordSoundProcInfo.compressAudio.waveFormatEx,  sizeof(  transferAvReplyInfo.tranInfo.audio.wf_decompress  )  );
			memcpy(&transferAvReplyInfo.req.tranInfo.audio.compressor, &pCompressAudio.compressor, sizeof(transferAvReplyInfo.req.tranInfo.audio.compressor));
			memcpy(&transferAvReplyInfo.req.tranInfo.audio.ah_compress, &pCompressAudio.ah_compress, sizeof(transferAvReplyInfo.req.tranInfo.audio.ah_compress));
			memcpy(&transferAvReplyInfo.req.tranInfo.audio.ah_decompress, &pCompressAudio.ah_decompress, sizeof(transferAvReplyInfo.req.tranInfo.audio.ah_decompress));
		}
		//  2014/09/04
		if (pDynBmp)
		{
			//transferAvReplyInfo.dynBmp.usIndex_obj  =  pDynBmp->usIndex_obj;
			transferAvReplyInfo.req.viewDynBmpReply.ucbSaveVideo = pDynBmp->ucbSaveVideo;       //  2014/09/07
		}
		//  2015/10/23
		if (pTaskAvProps)
		{
			memcpy(&transferAvReplyInfo.req.tranInfo.taskAvProps, pTaskAvProps, sizeof(transferAvReplyInfo.req.tranInfo.taskAvProps));
		}
		//
		qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("AvReply: maxFps %d, video %d, audio %d, ucbSaveVmm %d"), (pCompressVideo ? (int)pCompressVideo.compressor.common.usMaxFps_toShareBmp : 0), transferAvReplyInfo.req.tranInfo.video.uiTranNo_openAvDev, transferAvReplyInfo.req.tranInfo.audio.uiTranNo_openAvDev, (int)transferAvReplyInfo.req.viewDynBmpReply.ucbSaveVideo);
	}

	//  2008/06/12
	transferAvReplyInfo.myTask.ucbSendLocalAv = ucbSendLocalAv;
	transferAvReplyInfo.myTask.iIndex_sharedObj = iIndex_sharedObj;     //  ²»·¢±¾µØÊÓÆµµÄ£¬²»ÐèÒªÓÃsharedObj¿Õ¼ä

	//  ½«·¢¹ýÀ´µÄÑ¹ËõÐÅÏ¢´æÈëcontentµÄinternalÀï£¬ÓÃÔÚ½âÑ¹ËõÊ±
	//  int  index;  index  =  0;
	//
	transferAvReplyInfo.myTask.mem0_from.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
	transferAvReplyInfo.myTask.mem0_from.tStartTime_task = pMsgTask->tStartTime;
	//
	if (pMsgTask->uiTaskType == CONST_imTaskType_transferAvInfo)
	{                           //  2009/06/01
		if (pAddr->uiObjType == CONST_objType_imGrp)
		{
			transferAvReplyInfo.myTask.ucbVideoConference = true;                           //  2009/05/30									
			transferAvReplyInfo.myTask.mem0_from.ucbVideoConferenceStarter = true;      //  2009/04/02
		}
	}
	//
	transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.uiTranNo_openAvDev = pContent->transferAvInfo.tranInfo.video.uiTranNo_openAvDev;
#if  0  //  2014/11/07
	transferAvReplyInfo.myTask.mem0_from.video.usCapDriverIndex  =  0;	//  2014/11/07. pContent->transferAvInfo.tranInfo.video.usCapDriverIndex;
#endif

	//  safeStrnCpy(  pContent->transferAvInfo.tranInfo.video.fourccStr,  transferAvReplyInfo.myTask.mem0_from.video.fourccStr,  mycountof(  transferAvReplyInfo.myTask.mem0_from.video.fourccStr  )  );
	memcpy(&transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.compressor, &pContent->transferAvInfo.tranInfo.video.compressor, sizeof(transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.compressor));

	memcpy(&transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.vh_compress, &pContent->transferAvInfo.tranInfo.video.vh_compress, sizeof(transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.vh_compress));
	if (bSupported_sendVhDec())
	{
		memcpy(&transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.vh_decompress, &pContent->transferAvInfo.tranInfo.video.vh_decompress, sizeof(transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.vh_decompress));
	}
	//  2014/03/31
	memcpy(&transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.vh_stream, &pContent->transferAvInfo.tranInfo.video.vh_stream, sizeof(transferAvReplyInfo.myTask.mem0_from.video.tv_recvd.vh_stream));
	//
	transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.uiTranNo_openAvDev = pContent->transferAvInfo.tranInfo.audio.uiTranNo_openAvDev;
	//  memcpy(  &transferAvReplyInfo.myTask.mem0_from.audio.wf_compress_recvd,  &pContent->transferAvInfo.tranInfo.audio.wf_compress,  sizeof(  WAVEFORMATEX  )  );
	//  memcpy(  &transferAvReplyInfo.myTask.mem0_from.audio.wf_decompress_recvd,  &pContent->transferAvInfo.tranInfo.audio.wf_decompress,  sizeof(  WAVEFORMATEX  )  );
	memcpy(&transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.compressor, &pContent->transferAvInfo.tranInfo.audio.compressor, sizeof(transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.compressor));
	memcpy(&transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.ah_compress, &pContent->transferAvInfo.tranInfo.audio.ah_compress, sizeof(transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.ah_compress));
	memcpy(&transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.ah_decompress, &pContent->transferAvInfo.tranInfo.audio.ah_decompress, sizeof(transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd.ah_decompress));
	//

	if (transferAvReplyInfo.myTask.ucbVideoConference) transferAvReplyInfo.myTask.usCntLimit_mems_from = min(pContent->transferAvInfo.confCfg.usCntLimit_mems_from, MAX_mems_taskAv);
	if (!transferAvReplyInfo.myTask.usCntLimit_mems_from) transferAvReplyInfo.myTask.usCntLimit_mems_from = 1;  //  index  +  1;

	if (!transferAvReplyInfo.myTask.ucbVideoConference)
	{
		//  2011/08/08
#if  0
			POLICY_av	policyAv;
			if  (  getPolicyAv(  true,  0,  &policyAv  )  )  goto  errLabel;
#endif
		AV_COMPRESSOR_CFG tmpAvCompressor;
		if (pMsgTask->uiTaskType == CONST_imTaskType_transferAvInfo)
		{
			if (myGetAvCompressorCfg(CONST_capType_av, 0, 0, 0, &tmpAvCompressor)) goto errLabel;
		}
		else
		{
			if (myGetAvCompressorCfg(CONST_capType_mediaFile, 0, 0, 0, &tmpAvCompressor)) goto errLabel;
		}

		//
		//  memcpy(  &transferAvReplyInfo.myTask.audio.playCfg,  (  pMsgTask->uiTaskType  ==  CONST_imTaskType_transferAvInfo  )  ?  &policyAv.avCompressor.audio.playCfg  :  &policyAv.mediaFileCompressor.audio.playCfg,  sizeof(  transferAvReplyInfo.myTask.audio.playCfg  )  );
		memcpy(&transferAvReplyInfo.myTask.audio.playCfg, &tmpAvCompressor.audio.playCfg, sizeof(transferAvReplyInfo.myTask.audio.playCfg));
	}
	else
	{   //  2009/05/31
		transferAvReplyInfo.myTask.usCntLimit_activeMems_from = pContent->transferAvInfo.confCfg.usCntLimit_activeMems_from;
		if (!transferAvReplyInfo.myTask.usCntLimit_activeMems_from) transferAvReplyInfo.myTask.usCntLimit_activeMems_from = min(transferAvReplyInfo.myTask.usCntLimit_mems_from, mycountof(transferAvReplyInfo.myTask.activeMems_from));

		//  2009/06/03					
		memcpy(&transferAvReplyInfo.myTask.audio.playCfg, &pContent->transferAvInfo.tranInfo.audio.compressor.playCfg, sizeof(transferAvReplyInfo.myTask.audio.playCfg));
	}
	transferAvReplyInfo.myTask.hWnd_task = hWnd_task;       //  2009/09/09

	//
	lenInBytes = sizeof(transferAvReplyInfo);

	MACRO_prepareForTran();
	//
	//  if  (  postMsg2TaskQ_mc(  pQyMc,  pDlg->m_var.pSci,  pDlg->m_var.pMisCnt,  null,  0,  CONST_qyCmd_sendMedia,  tStartTran,  uiTranNo,  0,  pMsgTask->iTaskId,  0,  (  char  *  )&transferAvReplyInfo,  lenInBytes,  &pDlg->m_var.addr.idInfo,  &pMsgTask->idInfo_taskSender,  CONST_channelType_media  )  )  goto  errLabel;  
	//
	if (ucbSendLocalAv)
	{
		if (!pSharedObj || !pSharedObjUsr) goto errLabel;

		pSharedObjUsr.ucbNotSendData = (pAddr->uiObjType == CONST_objType_imGrp);

		if (addToMsgrs_sendLocalAv(pProcInfo, pMisCnt, &pMsgTask->idInfo_taskSender, false, &pSharedObj->curRoute_sendLocalAv)) goto errLabel;
	}
	//  2015/09/21
	int channelType = CONST_channelType_media;
	//  2015/09/21
	channelType = 0;
	//  2012/04/01
	if (postMsgTask2Mgr_mc(pMisCnt, CONST_misMsgType_task, 0, CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pMsgTask->iTaskId, pMsgTask->uiTaskType, (char*)&transferAvReplyInfo, lenInBytes, &pAddr->idInfo, &pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, &pMsgTask->idInfo_taskSender, channelType, null, false)) goto errLabel;

			*/

            iErr = 0;
        errLabel:
            return iErr;
        }


        //  2014/08/28.
        public static unsafe int procSendTaskProcReq(MIS_CNT pMisCnt, ushort usCode, ushort usOp, Int64 tStartTime_org, uint uiTranNo_org, uint uiContentType_org, int iTaskId, uint uiTaskType, QIS_RES_OBJ* pDynBmp, uint uiChannelType, bool bOutputDirectly, string pHint, MSGR_ADDR* pAddr)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pMisCnt.pProcInfoParam;
            TASK_PROC_REQ taskProcReq;
            int lenInBytes;


            /*
	//
	memset(&taskProcReq, 0, sizeof(taskProcReq));
	taskProcReq.uiType = CONST_imCommType_taskProcReq;
	taskProcReq.usOp = usOp;
	taskProcReq.tStartTime_org = tStartTime_org;
	taskProcReq.uiTranNo_org = uiTranNo_org;
	//  taskProcReq.uiSerialNo_org  =  uiSerialNo_org;
	taskProcReq.uiContentType_org = uiContentType_org;
	//
	taskProcReq.uiTaskType = uiTaskType;
	if (pDynBmp)
	{
		taskProcReq.viewDynBmp.uiObjType = pDynBmp->resObj.uiObjType;
		taskProcReq.viewDynBmp.usIndex_obj = pDynBmp->resObj.usIndex_obj;
	}
	//
	lenInBytes = sizeof(taskProcReq);
	//					
	MACRO_prepareForTran();
	//
	uint uiMisMsgType = CONST_misMsgType_task;
	if (bOutputDirectly) uiMisMsgType = CONST_misMsgType_outputTask;

	//
	QY_DMITEM* pTable = getResTable(0, &pQyMc.cusRes, CONST_resId_objTypeTable);
	if (!pHint) pHint = _T("");
	qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, pHint, _T(""), _T("sendTaskProcReq: %s %s %d. -> %I64u. tn %d"), qyGetDesByType1(CONST_imOpTable_en, taskProcReq.usOp), qyGetDesByType1(pTable, taskProcReq.viewDynBmp.uiObjType), taskProcReq.viewDynBmp.usIndex_obj, pAddr->idInfo.ui64Id, uiTranNo);

	//
	if (postMsgTask2Mgr_mc(pMisCnt, uiMisMsgType, 0, usCode, tStartTran, uiTranNo, 0, iTaskId, uiTaskType, (char*)&taskProcReq, lenInBytes, &pAddr->idInfo, null, null, &pAddr->idInfo, uiChannelType, null, false)) goto errLabel;

			*/

            //
            iErr = 0;
        errLabel:
            return iErr;
        }



    }
}



