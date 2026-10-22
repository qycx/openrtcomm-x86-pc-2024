using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static unsafe int getAvLevelByWh(int wParam, int hParam)
        {
            int level = 0;

            //
            int w = hParam;
            int h = wParam;


            //
            if (0 == h) return 0;

            //			
            if (h <= 240) level = Consts.CONST_policyAvLevel_240p;
            else if (h <= 480)
            {
                if (w >= 848) level = Consts.CONST_policyAvLevel_848x480;
                else level = Consts.CONST_policyAvLevel_480p;
            }
            else if (h <= 576)
            {
                level = Consts.CONST_policyAvLevel_576p;
            }
            else if (h <= 720)
            {
                level = Consts.CONST_policyAvLevel_720p;
            }
            else if (h <= 1080)
            {
                level = Consts.CONST_policyAvLevel_1080p;
            }
            else if (h <= 2160)
            {
                level = Consts.CONST_policyAvLevel_2160p;
            }
            else
            {
                level = Consts.CONST_policyAvLevel_1080p;
            }


            return level;
        }


        //
        //
        public static unsafe int getAvLevel_reply(TRANSFER_AV_INFO* pReq)
        {
            int avLevel = Consts.CONST_policyAvLevel_480p;

            //
            if (null == pReq) return 0;
            int iVal = 0;
            if (0 == pReq->confCfg.ucbVideoConference)
            {
                iVal = pReq->ass.tranInfo.video.vh_decompress.bih.biWidth * pReq->ass.tranInfo.video.vh_decompress.bih.biHeight;
            }
            else
            {
                iVal = pReq->confMosaicTranInfo_video.video.vh_decompress.bih.biWidth * pReq->confMosaicTranInfo_video.video.vh_decompress.bih.biHeight;
            }

            //
            if (iVal > (1920 + 200) * (1080 + 200)) avLevel = Consts.CONST_policyAvLevel_2160p;
            else if (iVal >= 1920 * 1080) avLevel = Consts.CONST_policyAvLevel_1080p;
            else if (iVal >= 1280 * 720) avLevel = Consts.CONST_policyAvLevel_720p;
            else if (iVal >= 640 * 480) avLevel = Consts.CONST_policyAvLevel_480p;
            else avLevel = Consts.CONST_policyAvLevel_240p;


            return avLevel;

        }


#if true
        //接收任务
        public static unsafe int dlgTalk_procTask_transferAvInfo(object hCurTalk, DLG_TALK_var pCurVar, object hMgr, DLG_TALK_var pMgrVar, int iTaskId, ushort usOp, string paramStrBuf,
            ref IM_TASK_RCD rcd, MIS_MSG_TASK* pMsgTask, IM_CONTENTU* pContent,
            ref int iStatus, ref int iStatus_old,
            QM_dbFuncs g_dbFuncs,
            object pDb,
            //TALKER_shadow_mgr* pShadowMgr,
            ref PARAM_dlgTalk_procTask pParam
            )
        {
            int iErr = -1;
          
            //
            bool bDbg = false;
            string str;

            object hDlgTalk = hMgr;
            DLG_TALK_var m_var = pMgrVar;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            bool bPrepareSet = false;
            //
            int i;
            int lenInBytes;

            //
            bool ucbGot_iIndex_sharedObj_localAv = false;


            //if (!pParam) return -1;
            //
            if (rcd.uiContentType != Consts.CONST_imCommType_transferAvInfo) return -1;

            //
            int iIndex_taskInfo = -1;
            iIndex_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(mainActivity.var_common.ctxCaller, pProcInfo, iTaskId);
            if (iIndex_taskInfo < 0)
            {
                qyFuncs.traceLog("dlgTalk_procTask_transferAvInfo failed, index_taskInfo <0");
                goto errLabel;
            }
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, iIndex_taskInfo, "dlgTalk_procTask_transferAvInfo");
            if (pTaskInfo == null)
            {

                qyFuncs.traceLog("pTaskInfo == null 137  ");
                goto errLabel;
            }
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null)
            {

                qyFuncs.traceLog("pTaskData == null  ");
                goto errLabel;
            }

            if (pTaskData.uiType != Consts.CONST_taskDataType_conf)
            {

                qyFuncs.traceLog("pTaskData.uiType != Consts.CONST_taskDataType_conf ");
                goto errLabel;
            }

            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            //
            str = string.Format("dlgTalk_procTask_transferAvInfo enters");
            qyFuncs.showInfo_open(0, null, str);


            try
            {
                //
                switch (usOp)
                {
                    case Consts.CONST_imOp_recv_accept:
                    case Consts.CONST_imOp_recvFile_saveas:
                    case Consts.CONST_imOp_dual_permit:
                        {

                            //  
                            bool ucbSendLocalAv = false;
                            int iIndex_sharedObj = -1;
                            int iIndex_sharedObjUsr = -1;
                            QY_SHARED_OBJ pSharedObj = null;
                            //
                            bool bFirstUsr = false;
                            SHARED_OBJ_USR pSharedObjUsr = null;
                            TALKER_policy talkerPolicy;

                            //
                            if (pContent->uiType != Consts.CONST_imCommType_transferAvInfo) goto errLabel;
                            int level = getAvLevel_reply(&pContent->transferAvInfo);

                            if (level >= Consts.CONST_policyAvLevel_2160p)
                            {
                                qyFuncs.showInfo_open(0, "", "not support 4k");
                                m_var.is4k = true;
                                goto errLabel;
                            }
                            else
                            {
                                m_var.is4k = false;
                            }


                            //  2011/08/08
                            AV_COMPRESSOR_CFG avCompressor_reply = new AV_COMPRESSOR_CFG();       //  响应要取得compressor, 目前只有av才有需要. 2012/02/16
                                                                                                  //  if  (  myGetAvCompressorCfg(  CONST_capType_av,  0,  CONST_capUsage_conf,  0,  &avCompressor_reply  )  )  goto  errLabel;
                            if (0 != myGetAvCompressorCfg(Consts.CONST_capType_av, 0, 0, level, ref avCompressor_reply))
                            {

                                qyFuncs.traceLog("0 != myGetAvCompressorCfg(Consts.CONST_capType_av, 0, 0, level, ref avCompressor_reply)");
                                goto errLabel;
                            }

                            //  2017/08/08
#if true  //  for compatibility
                            if (rcd.uiType == Consts.CONST_imTaskType_transferAvInfo)
                            {
                                if (0 == pContent->transferAvInfo.confCfg.ucbVideoConference)
                                {
                                    if (pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp)
                                    {
                                        pContent->transferAvInfo.confCfg.ucbVideoConference = qyFuncs.bool2byte(true);
                                    }
                                }
                            }
#endif
                            //
#if DEBUG
                            ref TRANSFER_AV_INFO tmp_av = ref pContent->transferAvInfo;
#endif

                            //  2016/04/17
                            //  2014/08/15
                            if (  //  pMgrVar.addr.uiObjType  ==  CONST_objType_imGrp						  
                                rcd.uiType == Consts.CONST_imTaskType_transferAvInfo
                                && 0 != pContent->transferAvInfo.confCfg.ucbVideoConference
                                )
                            {
                                int iWidth_pic_real = Math.Min(avCompressor_reply.video.common.head.iWidth_pic, pContent->transferAvInfo.confCfg.conf.iWidth_pic_conf);
                                int iHeight_pic_real = Math.Min(avCompressor_reply.video.common.head.iHeight_pic, pContent->transferAvInfo.confCfg.conf.iHeight_pic_conf);
                                //
                                int level_real = getAvLevelByWh(iWidth_pic_real, iHeight_pic_real);
                                if (0 != myGetAvCompressorCfg(Consts.CONST_capType_av, 0, 0, level_real, ref avCompressor_reply))
                                {
                                    qyFuncs.showInfo_open(0, null, _T("dlgTalk_procTask_transferAvInfo failed, myGetAvCompressorCfg failed. real"));
                                    goto errLabel;  //  2014/08/15
                                }
                                //
                                ushort usMaxFps_toShareBmp_real = avCompressor_reply.video.common.head.usMaxFps_toShareBmp;
                                if (0 != pContent->transferAvInfo.confCfg.conf.usMaxFps_toShareBmp_conf)
                                {
                                    usMaxFps_toShareBmp_real = Math.Min(usMaxFps_toShareBmp_real, pContent->transferAvInfo.confCfg.conf.usMaxFps_toShareBmp_conf);
                                    //
                                    if (0 != avCompressor_reply.video.common.head.iAvgBitrate)
                                    {
                                        if (usMaxFps_toShareBmp_real < avCompressor_reply.video.common.head.usMaxFps_toShareBmp)
                                        {
                                            if (0 != avCompressor_reply.video.common.head.usMaxFps_toShareBmp)
                                            {
                                                avCompressor_reply.video.common.head.iAvgBitrate = avCompressor_reply.video.common.head.iAvgBitrate * usMaxFps_toShareBmp_real / avCompressor_reply.video.common.head.usMaxFps_toShareBmp;
                                            }
                                        }
                                    }
                                }
                                //
                                /*
                                TCHAR tBuf[128];
                                _sntprintf(tBuf, mycountof(tBuf), _T("compressor of reply: %dX%d %dfps bitrate %d"), avCompressor_reply.video.common.iWidth_pic, avCompressor_reply.video.common.iHeight_pic, (int)avCompressor_reply.video.common.usMaxFps_toShareBmp, avCompressor_reply.video.common.iAvgBitrate);
                                showInfo_open(0, 0, tBuf);
                                */
                            }


                            //
                            getTalkerPolicy(m_var.addr.idInfo, out talkerPolicy);
                            //
                            //m_var.m_layout.ucbNeverJoinInBgWall  =  talkerPolicy.ucbNeverJoinInBgWall;
                            //
                            //m_var.m_layout.usPollingIntervalInS1  =  pShadowMgr->bgWall.bgWallCfg.usPollingIntervalInS;
                            /*
                            m_var.m_layout.bgWallLayoutCfg.usRows_bg = pShadowMgr->bgWall.bgWallCfg.bgWallLayoutCfg.usRows_bg;
                            m_var.m_layout.bgWallLayoutCfg.usCols_bg = pShadowMgr->bgWall.bgWallCfg.bgWallLayoutCfg.usCols_bg;
                            */
                            //					
                            if (rcd.uiContentType == Consts.CONST_imCommType_transferAvInfo)
                            {

                                if (rcd.uiType == Consts.CONST_imTaskType_transferAvInfo)
                                {
                                    if (m_var.av.taskInfo.bTaskExists)
                                    {


                                        qyFuncs.traceLog("m_var.av.taskInfo.bTaskExists");


                                        uint step = 0;
                                        qyFuncs.qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, _T(""), _T(""), _T("av is already active"));
                                        goto errLabel;
                                    }
                                    bPrepareSet = true;
                                    m_var.av.taskInfo.bPrepare = true;
                                }

                                //  
                                //  chkMsgrs_sendLocalAv(  pDlg->m_var.pProcInfo  );

                                //
                                if (usOp == Consts.CONST_imOp_dual_permit)
                                {
                                    ucbSendLocalAv = true;

                                    using (CQySyncObj syncObj = new CQySyncObj())
                                    {
                                        ;
                                        fixed (char* pName = pProcInfo.cfg.mutexName_syncStartAv)
                                        {
                                            if (0 != syncObj.sync(new string(pName), pMisCnt.hMutex_syncStartAv))
                                            {
                                                qyFuncs.traceLog("0 != syncObj.sync(new string(pName), pMisCnt.hMutex_syncStartAv)");
                                                goto errLabel;
                                            }
                                        }

                                        //
                                        if (0 != qyFuncs.getNewSharedObjUsr_localAv(pProcInfo, hDlgTalk, Consts.CONST_qyWndContentType_talker, &iIndex_sharedObj, &iIndex_sharedObjUsr, &bFirstUsr))
                                        {
                                            //
                                            /*
                                            TCHAR tHint[128];
                                            HWND hParent = hCurTalk;
                                            //
                                            _sntprintf(tHint, mycountof(tHint), getResStr(0, &pQyMc.cusRes, CONST_resId_notAcceptAv));
                                            showInfo_open(0, 0, tHint);
                                            showNotification_open(0, 0, 0, tHint);
                                            //
                                            if (pParam->bManualCmd)
                                            {
                                                if (pParam->hParent) hParent = pParam->hParent;
                                                myMessageBox(hParent, tHint, _T("www.qycx.com"), MB_OK);
                                            }
                                            */
                                            qyFuncs.traceLog("0 != qyFuncs.getNewSharedObjUsr_localAv(");
                                            goto errLabel;
                                        }
                                        //
                                        if (null == (pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, iIndex_sharedObj))) goto errLabel;
                                        m_var.av.ucbGot_iIndex_sharedObj_localAv = ucbGot_iIndex_sharedObj_localAv = true;
                                        m_var.av.iIndex_sharedObj_localAv = iIndex_sharedObj;
                                        m_var.av.iIndex_usr_localAv = iIndex_sharedObjUsr;
                                        //
                                        pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, m_var.av.iIndex_usr_localAv);
                                        if (null == pSharedObjUsr)
                                        {
                                            qyFuncs.traceLog("null == pSharedObjUsr");
                                            goto errLabel;
                                        }
                                        //
                                        if (bFirstUsr)
                                        {

                                            //
                                            int iIndex_capBmp = -1;
                                            CAP_procInfo_video pCapBmp = null;
                                            int iIndex_capAudio = -1;
                                            CAP_procInfo_audioCommon pCapAudio = null;
                                            int iIndex_shmCmd = -1;
                                            //QM_SHM_CMD* pShmCmd = null;

                                            //
                                            iIndex_capBmp = qyFuncs.newCapProcInfoBmpIndex(pProcInfo, iIndex_sharedObj);
                                            pCapBmp = (CAP_procInfo_video)qyFuncs.getCapBmpBySth(pProcInfo, iIndex_capBmp, 0);
                                            if (null == pCapBmp) goto errLabel;
                                            pCapBmp.uiType = Consts.CONST_capType_av;
                                            pCapBmp.iIndex_sharedObj = iIndex_sharedObj;
                                            pCapBmp.uiTranNo_sharedObj = pSharedObj.var.uiTranNo;
                                            //  pCompressVideo  =  &pCapBmp.compressVideo;
                                            //  2010/12/29

                                            //memset(&pCapBmp.compressVideo.debugInfo, 0, sizeof(pCapBmp.compressVideo.debugInfo)  );
                                            fixed (CompressVideo_debugInfo* pDebugInfo = &pCapBmp.compressVideo.debugInfo)
                                            {
                                                qyFuncs.mymemset((IntPtr)pDebugInfo, 0, sizeof(CompressVideo_debugInfo));
                                            }
                                            //qyFuncs.mytime(&pCapBmp.compressVideo.debugInfo.tStart);
                                            fixed (Int64* ptStart = &pCapBmp.compressVideo.debugInfo.tStart)
                                            {
                                                qyFuncs.mytime(ref *ptStart);
                                            }
                                            pCapBmp.compressVideo.debugInfo.nStep = 1;
                                            //
                                            iIndex_capAudio = qyFuncs.newCapProcInfoAudioIndex(pProcInfo, iIndex_sharedObj);
                                            pCapAudio = (CAP_procInfo_audioCommon)qyFuncs.getCapAudioBySth(pProcInfo, iIndex_capAudio, 0);
                                            if (null == pCapAudio)
                                            {
                                                qyFuncs.traceLog("null == pCapAudio");
                                                goto errLabel;
                                            }
                                            pCapAudio.uiType = Consts.CONST_capType_av;
                                            pCapAudio.iIndex_sharedObj = iIndex_sharedObj;
                                            pCapAudio.uiTranNo_sharedObj = pSharedObj.var.uiTranNo;
                                            //  pCompressAudio  =  &pCapAudio.compressAudio;
                                            //
                                            //
                                            /*
                                            iIndex_shmCmd = newShmCmdIndex(pProcInfo, iIndex_sharedObj);
                                            pShmCmd = getShmCmdByIndex((QM_SHM_CMDS*)pProcInfo.cmdsShm.pBuf, iIndex_shmCmd);
                                            if (!pShmCmd) goto errLabel;
                                            */

                                            //
                                            pSharedObj.var.iIndex_capBmp = iIndex_capBmp;
                                            pSharedObj.var.iIndex_capAudio = iIndex_capAudio;
                                            pSharedObj.var.iIndex_shmCmd = iIndex_shmCmd;


                                            {     //  老的工作模式. 2013/03/19
                                                bool bNoAv = false;

                                                //
#if DEBUG
                                                if  (  pProcInfo.cfg.debugStatusInfo.bUse_test_noLocalAv)
                                                {
                                                    bNoAv = true;
                                                }
#endif

                                                //
                                                if (!bNoAv)
                                                {
                                                    ref AUDIO_COMPRESSOR_CFG pAudioCompressor = ref avCompressor_reply.audio;

                                                    //  2013/08/03
                                                    //
                                                    /*
                                                    TCHAR displayName[MAX_monikerDisplayNameLen + 1] = _T("");
                                                    if (m_var.bIncludeACap)
                                                    {
                                                        getMonikerDisplayName(getMoniker_a(m_var.pCapStuff, m_var.iMenuId_selectedAudio), displayName, mycountof(displayName));
                                                    }
                                                    TCHAR capDevDesc[64] = _T("");
                                                    void* pADev = m_var.bIncludeACap ? displayName : getADeviceGuid(m_var.pCapStuff, m_var.iMenuId_selectedAudio, capDevDesc, mycountof(capDevDesc));
                                                    */
                                                    string capDevDesc = "";

                                                    //  2008/4/17, 
                                                    if (0 == startLocalAudioRecorder(pProcInfo.cfg.policy.audio.uiRecordType, null, capDevDesc, iIndex_sharedObj, ref pAudioCompressor))
                                                    {
                                                        //  m_var.av.ucbLocalAudioRecorderStarted  =  true;
                                                    }
                                                }

                                                //
                                                if (!bNoAv
                                                    /*!(pMisCnt->imRules.conferenceId_shareWebcam.ui64Id
                                                        && pMisCnt->imRules.conferenceId_shareWebcam.ui64Id == m_var.addr.idInfo.ui64Id
                                                        && pMisCnt->imRules.ucbNoLocalVideoInThisConference
                                                        )
                                                        && pQyMc.iCustomId != CONST_qyCustomId_hzj
                                                    */
                                                    )
                                                {
                                                    //
                                                    VIDEO_COMPRESSOR_CFG videoCompressor;
                                                    //memcpy(&videoCompressor, &avCompressor_reply.video, sizeof(videoCompressor));
                                                    videoCompressor = avCompressor_reply.video;
                                                    if (0 != pContent->transferAvInfo.confCfg.conf.usMaxFps_toShareBmp_conf) videoCompressor.common.head.usMaxFps_toShareBmp = Math.Min(videoCompressor.common.head.usMaxFps_toShareBmp, pContent->transferAvInfo.confCfg.conf.usMaxFps_toShareBmp_conf);
                                                    //  2014/08/15
                                                    if (0 != pContent->transferAvInfo.confCfg.conf.iWidth_pic_conf) videoCompressor.common.head.iWidth_pic = Math.Min(videoCompressor.common.head.iWidth_pic, pContent->transferAvInfo.confCfg.conf.iWidth_pic_conf);
                                                    if (0 != pContent->transferAvInfo.confCfg.conf.iHeight_pic_conf) videoCompressor.common.head.iHeight_pic = Math.Min(videoCompressor.common.head.iHeight_pic, pContent->transferAvInfo.confCfg.conf.iHeight_pic_conf);

                                                    //
                                                    if (pContent->transferAvInfo.confCfg.hgInfo.bFlow512k)
                                                    {
                                                        //
                                                        videoCompressor.common.head.iAvgBitrate = 422000;// 412000;// 512000;
                                                    }
                                                    else
                                                    {
                                                        videoCompressor.common.head.iAvgBitrate = 1300000;
                                                    }

                                                    //
#if DEBUG
                                                    qyFuncs.traceLog("dlgTalk_procTask_transferAvInfo.468");
#endif

                                                    //  2015/10/11												
                                                    if (0 != qyFuncs.newstartQThreadToShareAv(pProcInfo, iIndex_sharedObj, false))
                                                    {
                                                        qyFuncs.traceLog("0 != qyFuncs.newstartQThreadToShareAv");
                                                        goto errLabel;
                                                    }

#if DEBUG
                                                    qyFuncs.traceLog("dlgTalk_procTask_transferAvInfo.479,after newstartQThreadToShareAv");
#endif

                                                    //  								
                                                    if (!pProcInfo.av.localAv.videoCaptureProcInfo.bCapDevConnected)
                                                    {


                                                        if (pProcInfo.cfg.policy.video.uiCamCapType == Consts.CONST_camCapType_rtsp)
                                                        {       //  2014/02/23					  					  

                                                            /*
                                                            //  2014/03/15
                                                            int size = sizeof(TRANSINFO_stream);
                                                            if (!pSharedObj.pTransInfo_stream)
                                                            {
                                                                pSharedObj.pTransInfo_stream = mymalloc(size);
                                                                if (!pSharedObj.pTransInfo_stream) goto errLabel;
                                                                memset(pSharedObj.pTransInfo_stream, 0, size);
                                                                //
                                                                TRANSINFO_stream* pTransInfo = (TRANSINFO_stream*)pSharedObj.pTransInfo_stream;
                                                                //
                                                                pTransInfo->myTask.ucbLocalSource_rtspStream = true;
                                                                //
                                                                pTransInfo->myTask.usCntLimit_transforms = 1;
                                                                if (initTransforms(pProcInfo, &pTransInfo->addr_logicalPeer, 0, 0, &pTransInfo->tranInfo, &pTransInfo->myTask, null)) goto errLabel;
                                                                //
                                                                pTransInfo->myTask.usCntLimit_mems_from = 1;
                                                                size = sizeof(TASK_AV_FROM) * pTransInfo->myTask.usCntLimit_mems_from;
                                                                pTransInfo->myTask.pMems_from = (TASK_AV_FROM*)mymalloc(size);
                                                                if (!pTransInfo->myTask.pMems_from) goto errLabel;
                                                                memset(pTransInfo->myTask.pMems_from, 0, size);
                                                            }

                                                            //						  
                                                            TCHAR url[256] = _T("");    //  _T(  "rtsp://127.0.0.1:8554/video.264"  );
                                                            QY_REG reg;
                                                            reg.hKeyRoot0 = HKEY_CURRENT_USER;
                                                            lstrcpyn(reg.rootKey, pQyMc.cfg.pSysCfg->rootKey_qnmScheduler, sizeof(reg.rootKey));

                                                            if (qyGetRegCfgT(reg.hKeyRoot0, reg.rootKey, _T(CONST_regValName_rtspUrl_selected), (char*)url, sizeof(url), 0))
                                                            {
                                                                qyShowInfo1(CONST_qyShowType_warning, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("doCmd_startAvCall: rtsp url is wrong!"));
                                                                url[0] = 0;
                                                            }
                                                            //  2014/12/12. for test
                                                            if (pQyMc.appParams.rtspUrl[0])
                                                            {
                                                                safeTcsnCpy(pQyMc.appParams.rtspUrl, url, mycountof(url));
                                                                showInfo_open(0, 0, CQyString(_T("TEST: dlgTalk_procTask_transferAvInfo: use rtsp url: ")) + url);
                                                            }
                                                            //
                                                            if (!openLocalVideo_rtsp((MC_VAR_common*)pProcInfo, url, &videoCompressor, iIndex_sharedObj))
                                                            {
                                                                //  m_var.av.u.av.uc.ucbLocalVideoOpen  =  true;										  
                                                            }
                                                            */
                                                        }
                                                        else
                                                        {

#if DEBUG
                                                            qyFuncs.traceLog("dlgTalk_procTask_transferAvInfo.541. before openLocalVideo");
#endif

                                                            /*
                                                            TCHAR displayName[MAX_monikerDisplayNameLen + 1] = _T("");
                                                            if (getMonikerDisplayName(getMoniker_v(m_var.pCapStuff, m_var.iMenuId_selectedVideo), displayName, mycountof(displayName))) displayName[0] = 0;
                                                            */
                                                            string displayName = "";

                                                            uint uiCamCapType = pProcInfo.cfg.policy.video.uiCamCapType;
                                                            //
                                                            videoCompressor.shared_eglContext = m_var.m_share_eglContext;
                                                            //
                                                            int tmpiRet = qyFuncs.openLocalVideo(pProcInfo, uiCamCapType, displayName, hDlgTalk, Consts.CONST_qyWndContentType_talker, ref videoCompressor, iIndex_sharedObj);
                                                            //
                                                            if (0 == tmpiRet)
                                                            {
                                                                //  m_var.av.ucbLocalVideoOpen  =  true;  																		
                                                            }
                                                        }

                                                        //

#if __DEBUG__
												//  if  (  !m_var.av.ucbLocalVideoOpen  )  traceLogA(  "procQmInternalUrl: openLocalVideo failed."  );																		 
#endif
                                                    }
                                                    if (pProcInfo.av.localAv.videoCaptureProcInfo.bCapDevConnected)
                                                    {
                                                        pProcInfo.av.localAv.videoCaptureProcInfo.curhWnd = hDlgTalk;

                                                        //
                                                        //  2014/09/23. 建立多流
                                                        level = 0;
                                                        if (0 != makeSlaveStream(hDlgTalk, iIndex_sharedObj, level))
                                                        {
                                                            qyFuncs.traceLog("0 != makeSlaveStream(hDlgTalk, iIndex_sharedObj, level)");

                                                            goto errLabel;
                                                        }

                                                        //													
                                                        qyFuncs.startCapture(hDlgTalk, Consts.CONST_qyWndContentType_talker, pProcInfo.av.localAv.videoCaptureProcInfo);

                                                    }

                                                }

                                            }

                                        }

                                        //  2017/09/17
                                        dlgTalk_init_respInfo(hMgr, pMgrVar, qyFuncs.byte2bool(pContent->transferAvInfo.confCfg.ucbVideoConference));

                                        //  2010/09/04
                                        ref COMPRESS_AUDIO pCompressAudio = ref pProcInfo.av.localAv.recordSoundProcInfo.compressAudio;
                                        ref COMPRESS_VIDEO pCompressVideo = ref pProcInfo.av.localAv.videoCaptureProcInfo.compressVideo;

                                        if (0 != pCompressAudio.uiTranNo_openAvDev_org)
                                        {
                                            pSharedObjUsr.uiTranNo_openAvDev_a = (uint)(pCompressAudio.uiTranNo_openAvDev_org + iIndex_sharedObjUsr); //  pSharedObj.iIndex_curUsr;
                                        }
                                        if (0 != pCompressVideo.uiTranNo_openAvDev_org)
                                        {
                                            pSharedObjUsr.uiTranNo_openAvDev_v = (uint)(pCompressVideo.uiTranNo_openAvDev_org + iIndex_sharedObjUsr); //  pSharedObj.iIndex_curUsr;
                                        }

                                        //
                                        //  2016/03/28
                                        /*
                                        if (qyFuncs.bExists_slave(pProcInfo, iIndex_sharedObj))
                                        {

                                            int iIndex_sharedObj_slave;
                                            iIndex_sharedObj_slave = pSharedObj.var.master.slaveKey.iIndex_sharedObj;
                                            //
                                            ref SHARE_dyn_bmp pShareDynBmp_slave = ref pSharedObj.var.master.pShareDynBmp_slave;
                                            //if (!pShareDynBmp_slave) goto errLabel;
                                            //
                                            int index_sharedObjUsr_slave = 0;
                                            if (0 != qyFuncs.getNewSharedObjUsr(pProcInfo, hDlgTalk, Consts.CONST_qyWndContentType_talker, iIndex_sharedObj_slave, &index_sharedObjUsr_slave, null))
                                            {
                                                qyFuncs.showInfo_open(0, null, _T("dlgTalk_procTask_transferAvInfo failed: newSharedObjUsr failed"));
                                                goto errLabel;
                                            }
                                            //
                                            QY_SHARED_OBJ pSharedObj_slave = qyFuncs.getSharedObjByIndex(pProcInfo, iIndex_sharedObj_slave);
                                            if (null == pSharedObj_slave)
                                            {

                                                qyFuncs.traceLog("null == pSharedObj_slave");
                                                goto errLabel;
                                            }
                                            //
                                            SHARED_OBJ_USR pUsr_slave = qyFuncs.getSharedObjUsr(pSharedObj_slave, index_sharedObjUsr_slave);
                                            if (null == pUsr_slave)
                                            {
                                                qyFuncs.traceLog("null == pUsr_slave");
                                                goto errLabel;
                                            }
                                            //
                                            int iIndex_capBmp = pSharedObj_slave.var.iIndex_capBmp;
                                            CAP_procInfo_bmpCommon pCapBmp = qyFuncs.getCapBmpBySth(pProcInfo, iIndex_capBmp, 0);
                                            if (null == pCapBmp)
                                            {
                                                qyFuncs.traceLog("null == pCapBmp");

                                                goto errLabel;
                                            }
                                            ref COMPRESS_VIDEO pCompressVideo_slave = ref pCapBmp.compressVideo;
                                            //
                                            if (0 != pCompressVideo_slave.uiTranNo_openAvDev_org)
                                            {
                                                pUsr_slave.uiTranNo_openAvDev_v = (uint)(pCompressVideo_slave.uiTranNo_openAvDev_org + index_sharedObjUsr_slave);
                                            }
                                            //
                                            pUsr_slave.iTaskId = pShareDynBmp_slave.var.iTaskId;



                                        }
                                        */

                                        int ii = 0;
                                        syncObj.m_bDbg = true;

                                    }
                                }


                                //

                            }

                            //
                            //if (!(pMsgTask = findTaskInProcessQ(m_var, iTaskId, 0))) goto errLabel;
                            if (null == pMsgTask) goto errLabel;

                            // 
                            iStatus = Consts.CONST_imTaskStatus_waitToRecv;
                            iStatus_old = pMsgTask->iStatus;
                            //					
                            if (0 != g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, rcd.id)) goto errLabel;
                            pMsgTask->iStatus = iStatus;

                            //
                            /*
                            for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
                            {
                                if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
                                {
                                    CHelp_getDlgTalkVar help_getDlgTalkVar;
                                    dlgTalk_procTask_display_op(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
                                }
                            }
                            //
                            dlgTalk_procTask_display_op(hDlgTalk, &m_var, iTaskId, usOp, pMsgTask, rcd, iStatus_old, iStatus, paramStrBuf);
                            */

                            //
                            //  2014/11/19	
                            if (rcd.uiType != Consts.CONST_imTaskType_transferAvInfo)
                            {
                                bool bDo_avProxy = false;
                                if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
                                {
                                    if (pMsgTask->data.route.idInfo_to.ui64Id == pMisCnt.idInfo.ui64Id
                                        && pContent->transferAvInfo.idInfo_logicalPeer.ui64Id == m_var.addr.idInfo.ui64Id)
                                    {
                                        if (m_var.av.taskInfo.bTaskExists
                                            && m_var.av.taskInfo.ucbStarter)
                                        {
                                            if (0 != m_var.av.taskInfo.mosaicMaker.video.resObj.uiObjType
                                                || 0 != m_var.av.taskInfo.mosaicMaker.resource.resObj.uiObjType)
                                            {
                                                bDo_avProxy = true;
                                            }
                                            //
                                        }

                                        //
#if __DEBUG__
									if (b__TEST_avProxy__())
									{
#if true
												bDo_avProxy  =  true;
												traceLog(  _T(  "For test: bExists_avProxy"  )  );
#endif
									}
#endif

                                        //  2014/11/22
                                    }
                                }

                                //  2014/11/20
                                if (bDo_avProxy)
                                {
#if __DEBUG__
								traceLog(_T("bExists_avProxy is true"));
#endif

                                    /*
                                    //
                                    using (CQyMalloc mallocObj_transferAvInfo = new CQyMalloc())
                                    {
                                        ;
                                        TRANSFER_AV_task* pTransferAvInfo = (TRANSFER_AV_task*)mallocObj_transferAvInfo.mallocf(sizeof(TRANSFER_AV_task));
                                        if (!pTransferAvInfo) goto errLabel;
                                        TRANSFER_AV_task & transferAvInfo = *pTransferAvInfo;

                                        memset(&transferAvInfo, 0, sizeof(transferAvInfo));
                                        transferAvInfo.req.uiType = CONST_imCommType_transferAvInfo;
                                        //  2016/04/23
                                        transferAvInfo.req.tmp_usSubtype = CONST_transferAvInfo_subtype_task;

                                        //
                                        transferAvInfo.req.uiTaskType = pContent->transferAvInfo.uiTaskType;

                                        //
                                        transferAvInfo.req.tranInfo.video.uiTranNo_openAvDev = getuiNextTranNo(0, 0, 0);
                                        transferAvInfo.req.tranInfo.audio.uiTranNo_openAvDev = transferAvInfo.req.tranInfo.video.uiTranNo_openAvDev;

                                        //  
                                        transferAvInfo.req.proxiedTranInfo.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
                                        transferAvInfo.req.proxiedTranInfo.uiObjType = CONST_objType_proxiedStream_video;
                                        //
                                        transferAvInfo.req.proxiedTranInfo.ti = pContent->transferAvInfo.tranInfo;

                                        //
                                        PROC_TASK_AV* pTask = &transferAvInfo.myTask;   //  get_transferAvInfo_pTask(  &transferAvInfo,  _T(  ""  )  );
                                        if (!pTask) goto errLabel;
                                        //
                                        pTask->doProxy.bExists_avProxy = true;

                                        //  							
                                        //  int  index;  index  =  0;								
                                        //
                                        pTask->mem0_from.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
                                        pTask->mem0_from.tStartTime_task = pMsgTask->tStartTime;
                                        //
                                        //  2014/09/23							
                                        pTask->mem0_from.shClientVer = (short)pContent->transferAvInfo.tranInfo.head.shClientVer;
                                        //
                                        //  2014/11/11							
                                        //memcpy(&pTask->mem0_from.video.tv_recvd, &pContent->transferAvInfo.tranInfo.video, sizeof(pTask->mem0_from.video.tv_recvd)  );
                                        pTask->mem0_from.video.tv_recvd = pContent->transferAvInfo.tranInfo.video;
                                        //
                                        //  2014/11/11							
                                        //memcpy(&pTask->mem0_from.audio.ta_recvd, &pContent->transferAvInfo.tranInfo.audio, sizeof(pTask->mem0_from.audio.ta_recvd)  );
                                        pTask->mem0_from.audio.ta_recvd = pContent->transferAvInfo.tranInfo.audio;


                                        //
                                        if (pTask->usCntLimit_mems_from < 2) pTask->usCntLimit_mems_from = 2;   //  index  +  1;

                                        //							
                                        if (!pTask->ucbVideoConference)
                                        {

                                            tmpGetMaxAudioPlayCfg(&pContent->transferAvInfo.tranInfo.audio.compressor.playCfg, &avCompressor_reply.audio.playCfg, &pTask->audio.playCfg);
                                        }
                                        else
                                        {   //  2009/05/31

                                            //  2009/06/03. //  2011/11/28, 因为高性能的策略在一些老式电脑上无法运行，所以，声音策略不能取从视频发起方下发的策略，而是用本地的策略和下发的策略的最大值	
                                            AUDIO_PLAY_CFG* pAudioPlayCfg_conf = &pContent->transferAvInfo.tranInfo.audio.compressor.playCfg;
                                            tmpGetMaxAudioPlayCfg(pAudioPlayCfg_conf, &avCompressor_reply.audio.playCfg, &pTask->audio.playCfg);
                                        }
                                        pTask->hWnd_task = hDlgTalk;        //  2009/09/09



                                        //
                                        lenInBytes = sizeof(transferAvInfo);

                                        MACRO_prepareForTran();

                                        //if  (  postMsgTask2Mgr_mc(  pMisCnt,  CONST_misMsgType_task,  0,  CONST_qyCmd_sendMedia,  tStartTran,  uiTranNo,  0,  pMsg->iTaskId,  pMsg->uiTaskType,  (  char  *  )pContent,  lenInBytes,  (  pAddr  ?  &pAddr->idInfo  :  null  ),  &pMsg->idInfo_taskSender,  &pMsg->idInfo_taskReceiver,  (  pAddr  ?  &pAddr->idInfo  :  null  ),  CONST_channelType_media,  null,  false  )  )  goto  errLabel;

                                        QY_MESSENGER_ID idInfo_taskSender, idInfo_taskReceiver;
                                        QY_MESSENGER_ID idInfo_dst;
                                        idInfo_taskSender.ui64Id = pMisCnt->idInfo.ui64Id;
                                        idInfo_taskReceiver.ui64Id = m_var.addr.idInfo.ui64Id;
                                        idInfo_dst.ui64Id = m_var.addr.idInfo.ui64Id;
                                        //
                                        uint uiChannelType = Consts.CONST_channelType_media;
                                        //
                                        uiChannelType = 0;
                                        //
                                        if (postMsgTask2Mgr_mc(m_var.pMisCnt, CONST_misMsgType_task, 0, CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pMsgTask->iTaskId, pMsgTask->uiTaskType, (char*)&transferAvInfo, lenInBytes, &m_var.addr.idInfo, &idInfo_taskSender, &idInfo_taskReceiver, &idInfo_dst, uiChannelType, null, false)) goto errLabel;

                                        //  2014/11/22
                                        pTask->dlgTalkInProxy.bExists = true;
                                        pTask->dlgTalkInProxy.proxy.tStartTran = tStartTran;
                                        pTask->dlgTalkInProxy.proxy.uiTranNo = uiTranNo;

                                        //
                                        iErr = 0; goto errLabel;
                                        break;
                                    }
                                    */
                                }
                            }

                            //						
                            if (rcd.uiContentType == Consts.CONST_imCommType_transferAvInfo)
                            {

                                /*
                                CQyMalloc mallocObj_transferAvReplyInfo;
                                TRANSFER_AV_reply_task* pTransferAvReplyInfo = (TRANSFER_AV_reply_task*)mallocObj_transferAvReplyInfo.mallocf(sizeof(TRANSFER_AV_reply_task));
                                if (!pTransferAvReplyInfo) goto errLabel;
                                TRANSFER_AV_reply_task & transferAvReplyInfo = *pTransferAvReplyInfo;
                                */

                                Int64 tStartTran; uint uiTranNo;
                                qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);

                                TASK_transferAv task = new TASK_transferAv();
                                //
                                task.m_index_taskInfo = iIndex_taskInfo;
                                pTaskInfo.var.m_bReplyTask = true;
                                //
                                //fixed (MIS_MSG_TASK* pMsg = &task.msgU.task) 
                                {
                                    ;
                                    /*
                                    pMsg->uiType = Consts.CONST_misMsgType_task;

                                    pMsg->ucFlg = Consts.CONST_commFlg_talkData;
                                    //
                                    pMsg->usCode = Consts.CONST_qyCmd_sendMedia;
                                    pMsg->tStartTime = tStartTran;
                                    pMsg->uiTranNo = uiTranNo;

                                    //
                                    pMsg->iTaskId = iTaskId;


                                    //
                                    TRANSFER_AV_replyInfo* pContent_reply = (TRANSFER_AV_replyInfo*)pMsg->data.buf;
                                    //
                                    ref TRANSFER_AV_replyInfo transferAvReplyInfo = ref *pContent_reply;
                                    */
                                    TRANSFER_AV_replyInfo transferAvReplyInfo;
                                    ref PROC_TASK_AV transferAvReplyInfo_myTask = ref task.myTask;

                                    //  
                                    //memset(&transferAvReplyInfo, 0, sizeof(transferAvReplyInfo));
                                    qyFuncs.mymemset((IntPtr)(byte*)&transferAvReplyInfo, 0, sizeof(TRANSFER_AV_replyInfo));
                                    transferAvReplyInfo.uiType = Consts.CONST_imCommType_transferAvReplyInfo;
                                    //  2016/04/18
                                    transferAvReplyInfo.tmp_usSubtype = Consts.CONST_transferAvReplyInfo_subtype_task;
                                    //
                                    transferAvReplyInfo.usOp = Consts.CONST_imOp_recv_accept;
                                    transferAvReplyInfo.tStartTime_org = pMsgTask->tStartTime;
                                    transferAvReplyInfo.uiTranNo_org = pMsgTask->uiTranNo;
                                    //
                                    if (ucbSendLocalAv)
                                    {   //  
                                        ref COMPRESS_VIDEO pCompressVideo = ref pProcInfo.av.localAv.videoCaptureProcInfo.compressVideo;
                                        ref COMPRESS_AUDIO pCompressAudio = ref pProcInfo.av.localAv.recordSoundProcInfo.compressAudio;

                                        //  2009/04/19
                                        if (0 != pContent->transferAvInfo.confCfg.conf.usMaxFps_toShareBmp_conf)
                                        {
                                            //  pCompressVideo.usMaxFps_toShareBmp  =  min(  pContent->transferAvInfo.tranInfo.video.usMaxFps_toShareBmp_videoConference,  pCompressVideo.usMaxFps_toShareBmp  );    
                                            //  2009/06/04
                                            transferAvReplyInfo_myTask.video.usMaxFps_toShareBmp_conf = pCompressVideo.compressor.common.head.usMaxFps_toShareBmp;
                                        }

                                        //
                                        if (null == pSharedObjUsr) goto errLabel;
                                        transferAvReplyInfo.tranInfo.video.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_v;    //  pCompressVideo.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;
                                                                                                                                       //  
                                                                                                                                       //memcpy(&transferAvReplyInfo.req.tranInfo.video.compressor, &pCompressVideo.compressor, sizeof(transferAvReplyInfo.req.tranInfo.video.compressor));
                                        transferAvReplyInfo.tranInfo.video.compressor = pCompressVideo.compressor.common.head;
                                        //memcpy(&transferAvReplyInfo.req.tranInfo.video.vh_compress, &pCompressVideo.vh_compress, sizeof(transferAvReplyInfo.req.tranInfo.video.vh_compress));
                                        transferAvReplyInfo.tranInfo.video.vh_compress = pCompressVideo.vh_compress;
                                        //  2014/03/31
                                        //memcpy(&transferAvReplyInfo.req.tranInfo.video.vh_stream, &pCompressVideo.vh_stream, sizeof(transferAvReplyInfo.req.tranInfo.video.vh_stream));
                                        transferAvReplyInfo.tranInfo.video.vh_stream = pCompressVideo.vh_stream;
                                        //
                                        //memcpy(&transferAvReplyInfo.req.tranInfo.video.vh_decompress.bih, &pCompressVideo.vh_decompress.bih, sizeof(transferAvReplyInfo.req.tranInfo.video.vh_decompress.bih));
                                        transferAvReplyInfo.tranInfo.video.vh_decompress.bih = pCompressVideo.vh_decompress.bih;
                                        //
                                        transferAvReplyInfo.tranInfo.audio.uiTranNo_openAvDev = pSharedObjUsr.uiTranNo_openAvDev_a;    //  pCompressAudio.uiTranNo_openAvDev  +  iIndex_sharedObjUsr;
                                                                                                                                       //memcpy(&transferAvReplyInfo.req.tranInfo.audio.compressor, &pCompressAudio.compressor, sizeof(transferAvReplyInfo.req.tranInfo.audio.compressor));
                                        transferAvReplyInfo.tranInfo.audio.compressor = pCompressAudio.compressor.common.head;
                                        //memcpy(&transferAvReplyInfo.req.tranInfo.audio.ah_compress, &pCompressAudio.ah_compress, sizeof(transferAvReplyInfo.req.tranInfo.audio.ah_compress));
                                        transferAvReplyInfo.tranInfo.audio.ah_compress = pCompressAudio.ah_compress;
                                        //  2016/12/23
                                        //memcpy(&transferAvReplyInfo.req.tranInfo.audio.ah_stream, &pCompressAudio.ah_stream, sizeof(transferAvReplyInfo.req.tranInfo.audio.ah_stream));
                                        transferAvReplyInfo.tranInfo.audio.ah_stream = pCompressAudio.ah_stream;
                                        //
                                        //memcpy(&transferAvReplyInfo.req.tranInfo.audio.ah_decompress, &pCompressAudio.ah_decompress, sizeof(transferAvReplyInfo.req.tranInfo.audio.ah_decompress));
                                        transferAvReplyInfo.tranInfo.audio.ah_decompress = pCompressAudio.ah_decompress;
                                        //
                                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("AvReply: maxFps %d, video %d, audio %d"), (int)pCompressVideo.compressor.common.usMaxFps_toShareBmp, transferAvReplyInfo.req.tranInfo.video.uiTranNo_openAvDev, transferAvReplyInfo.req.tranInfo.audio.uiTranNo_openAvDev);
                                    }

                                    //  2017/09/08
                                    transferAvReplyInfo_myTask.uiTaskType = rcd.uiType;

                                    //  2008/06/12
                                    transferAvReplyInfo_myTask.ucbSendLocalAv = ucbSendLocalAv;
                                    transferAvReplyInfo_myTask.iIndex_sharedObj = iIndex_sharedObj;     //  sharedObj

                                    //
                                    transferAvReplyInfo_myTask.iIndex_taskInfo = iIndex_taskInfo;

                                    //  
                                    //  int  index;  index  =  0;
                                    //
                                    transferAvReplyInfo_myTask.mem0_from.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
                                    transferAvReplyInfo_myTask.mem0_from.tStartTime_task = pMsgTask->tStartTime;
                                    //
                                    if (rcd.uiType == Consts.CONST_imTaskType_transferAvInfo)
                                    {                           //  2009/06/01
                                        if (  //m_var.addr.uiObjType  ==  CONST_objType_imGrp  
                                            0 != pContent->transferAvInfo.confCfg.ucbVideoConference
                                            )
                                        {
                                            pTc.ucbVideoConference = true;                           //  2009/05/30									
                                            transferAvReplyInfo_myTask.mem0_from.ucbVideoConferenceStarter = true;      //  2009/04/02
                                        }
                                    }
                                    //  2014/09/23
                                    transferAvReplyInfo_myTask.mem0_from.shClientVer = (short)pContent->transferAvInfo.ass.tranInfo.head.shClientVer;
                                    //
                                    //  2014/11/11
                                    //memcpy(&transferAvReplyInfo.myTask.mem0_from.video.tv_recvd, &pContent->transferAvInfo.tranInfo.video, sizeof(transferAvReplyInfo.myTask.mem0_from.video.tv_recvd));
                                    transferAvReplyInfo_myTask.mem0_from.video.tv_recvd = pContent->transferAvInfo.ass.tranInfo.video;
                                    //
                                    //  2014/11/11
                                    //memcpy(&transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd, &pContent->transferAvInfo.tranInfo.audio, sizeof(transferAvReplyInfo.myTask.mem0_from.audio.ta_recvd));
                                    transferAvReplyInfo_myTask.mem0_from.audio.ta_recvd = pContent->transferAvInfo.ass.tranInfo.audio;

                                    //  2014/11/11
                                    uint uiTranNo_openAvDev_confMosaic_video = 0;                              //  Video
                                    uint uiTranNo_openAvDev_confMosaic_resource = 0;                           //  Video

                                    //
                                    if (transferAvReplyInfo_myTask.bVideoConference())
                                    {

                                        //PROC_conf_mosaic pProcConfMosaic = null;

                                        if (0 != pContent->transferAvInfo.confMosaicTranInfo_video.resObj.uiObjType)
                                        {
#if false
										pProcConfMosaic = transferAvReplyInfo_myTask.confMosaicTrans.confMosaic_video1;
										pProcConfMosaic.taskAvFrom.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
										pProcConfMosaic.taskAvFrom.video.tv_recvd = pContent->transferAvInfo.confMosaicTranInfo_video.video;
										uiTranNo_openAvDev_confMosaic_video = pProcConfMosaic.taskAvFrom.video.tv_recvd.uiTranNo_openAvDev;
#endif
                                            //
                                            uiTranNo_openAvDev_confMosaic_video = pContent->transferAvInfo.confMosaicTranInfo_video.video.uiTranNo_openAvDev;
                                        }
                                        //
                                        if (0 != pContent->transferAvInfo.confMosaicTranInfo_resource.resObj.uiObjType)
                                        {
#if false
										pProcConfMosaic = transferAvReplyInfo_myTask.confMosaicTrans.confMosaic_resource;
										pProcConfMosaic.taskAvFrom.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
										pProcConfMosaic.taskAvFrom.video.tv_recvd = pContent->transferAvInfo.confMosaicTranInfo_resource.video;
										uiTranNo_openAvDev_confMosaic_resource = pProcConfMosaic.taskAvFrom.video.tv_recvd.uiTranNo_openAvDev;
#endif
                                            //
                                            uiTranNo_openAvDev_confMosaic_resource = pContent->transferAvInfo.confMosaicTranInfo_resource.video.uiTranNo_openAvDev;
                                        }
                                    }

                                    //  2014/11/21
                                    if (!transferAvReplyInfo_myTask.bVideoConference())
                                    {
                                        if (0 != pContent->transferAvInfo.proxiedTranInfo.idInfo.ui64Id
                                            && 0 != pContent->transferAvInfo.proxiedTranInfo.uiObjType)
                                        {
                                            transferAvReplyInfo_myTask.receiver.bRecvd_proxiedTranInfo = true;
                                            //
                                            transferAvReplyInfo_myTask.receiver.proxied_from.idInfo.ui64Id = pContent->transferAvInfo.proxiedTranInfo.idInfo.ui64Id;
                                            transferAvReplyInfo_myTask.receiver.proxied_from.shClientVer = (short)pContent->transferAvInfo.proxiedTranInfo.ti.head.shClientVer;
                                            transferAvReplyInfo_myTask.receiver.proxied_from.video.tv_recvd = pContent->transferAvInfo.proxiedTranInfo.ti.video;
                                            transferAvReplyInfo_myTask.receiver.proxied_from.audio.ta_recvd = pContent->transferAvInfo.proxiedTranInfo.ti.audio;
                                        }
                                    }

                                    //
                                    if (transferAvReplyInfo_myTask.bVideoConference()) videoConference.usCntLimit_mems_from = Math.Min((ushort)30, (ushort)Consts.MAX_mems_taskAv);
                                    if (videoConference.usCntLimit_mems_from < 2) videoConference.usCntLimit_mems_from = 2;   //  index  +  1;

                                    if (!transferAvReplyInfo_myTask.bVideoConference())
                                    {
                                        videoConference.usMaxSpeakers = 2;
                                        videoConference.usCntLimit_activeMems_from = (ushort)(videoConference.usMaxSpeakers + Consts.MAX_resourceMems_taskAv_p2p);

                                        //tmpGetMaxAudioPlayCfg(&pContent->transferAvInfo.tranInfo.audio.compressor.playCfg, &avCompressor_reply.audio.playCfg, &transferAvReplyInfo.myTask.audio.playCfg);
                                    }
                                    else
                                    {   //  2009/05/31
                                        videoConference.usMaxSpeakers = pContent->transferAvInfo.confCfg.usMaxSpeakers;
                                        if (0 == videoConference.usMaxSpeakers)
                                        {
                                            str = string.Format("dlgTalk_procTask_transferAvInfo: talker{0} failed: usMaxSpeakers is 0", m_var.addr.idInfo.ui64Id);
                                            qyFuncs.showInfo_open(0, null, str);
                                            //
                                            goto errLabel;
                                        }

                                        videoConference.usCntLimit_activeMems_from = (ushort)(pContent->transferAvInfo.confCfg.usMaxSpeakers + Consts.MAX_mosaicMems_taskAv + Consts.MAX_resourceMems_taskAv_conf);
                                        //if  (  !pTc->videoConference.usCntLimit_activeMems_from  )  pTc->videoConference.usCntLimit_activeMems_from  =  min(  pTc->videoConference.usCntLimit_mems_from,  mycountof(  pTc->videoConference.activeMems_from  )  );
                                        //pTc->videoConference.usCntLimit_activeMems_from  =  min(  pTc->videoConference.usCntLimit_activeMems_from,  mycountof(  pTc->videoConference.activeMems_from  )  );
                                        if (videoConference.usCntLimit_activeMems_from > Consts.MAX_activeMems_from)
                                        {
                                            qyFuncs.showInfo_open(0, null, _T("dlgTalk_procTask_transferAvInfo failed: usCntLimit_activeMems_from too big"));
                                            goto errLabel;
                                        }

                                        //  
                                        //AUDIO_PLAY_CFG* pAudioPlayCfg_conf = &pContent->transferAvInfo.tranInfo.audio.compressor.playCfg;
                                        //tmpGetMaxAudioPlayCfg(pAudioPlayCfg_conf, &avCompressor_reply.audio.playCfg, &transferAvReplyInfo.myTask.audio.playCfg);
                                    }
                                    transferAvReplyInfo_myTask.hWnd_task = hDlgTalk;        //  2009/09/09

                                    //
                                    lenInBytes = sizeof(TRANSFER_AV_replyInfo);

                                    //
                                    //  if  (  postMsg2TaskQ_mc(  pQyMc,  pDlg->m_var.pSci,  pDlg->m_var.pMisCnt,  null,  0,  CONST_qyCmd_sendMedia,  tStartTran,  uiTranNo,  0,  pMsgTask->iTaskId,  0,  (  char  *  )&transferAvReplyInfo,  lenInBytes,  &pDlg->m_var.addr.idInfo,  &pMsgTask->idInfo_taskSender,  CONST_channelType_media  )  )  goto  errLabel;  
                                    //
                                    if (ucbSendLocalAv)
                                    {
                                        if (null == pSharedObj || null == pSharedObjUsr) goto errLabel;

                                        pTc.ucbNotSendData = (m_var.addr.uiObjType == Consts.CONST_objType_imGrp);
                                        if (pParam.av_autoAnswer_av_task_bTaskNeedAutoAnswered) pTc.ucbNotSendData = true;  //  2010/09/07

                                        //  if  (  addToMsgrs_sendLocalAv(  pProcInfo,  m_var.pMisCnt,  &pMsgTask->idInfo_taskSender,  false,  pSharedObj  )  )  goto  errLabel;
                                        //  if  (  setROUTE_sendLocalAv(  pProcInfo,  m_var.pMisCnt,  &pMsgTask->idInfo_taskSender,  false,  pSharedObj  )  )  goto  errLabel;

                                        //  2010/09/03
                                        pSharedObjUsr.iTaskId = pMsgTask->iTaskId;
                                        pSharedObjUsr.idInfo_to.ui64Id = pMsgTask->idInfo_taskSender.ui64Id;
                                        pSharedObjUsr.ucbVideoConference = qyFuncs.byte2bool(pContent->transferAvInfo.confCfg.ucbVideoConference);    //  2018/11/10 

                                    }
                                    //
                                    transferAvReplyInfo_myTask.reply_avTranInfo = transferAvReplyInfo.tranInfo;
                                    //
                                    pTc.shared_eglContext = m_var.m_share_eglContext;
                                    //
                                    //
                                    if (0 != pProcInfo.processQ_media.qPostMsg(task, 0))
                                    {
                                        goto errLabel;
                                    }
                                    //  2015/09/07
                                    uint uiChannelType = Consts.CONST_channelType_media;
                                    //  2015/09/07
                                    uiChannelType = 0;
                                    //  2012/04/01
                                    QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;
                                    //  if  (  postMsgTask2Mgr_mc(  m_var.pMisCnt,  CONST_misMsgType_task,  0,  CONST_qyCmd_sendMedia,  tStartTran,  uiTranNo,  0,  pMsgTask->iTaskId,  0,  (  char  *  )&transferAvReplyInfo,  lenInBytes,  &m_var.addr.idInfo,  &pMsgTask->idInfo_taskSender,  &pMsgTask->idInfo_taskReceiver,  &pMsgTask->idInfo_taskSender,  CONST_channelType_media,  null,  false  )  )  goto  errLabel;
                                    if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendMedia, tStartTran, uiTranNo, 0, pMsgTask->iTaskId, pMsgTask->uiTaskType, (byte*)&transferAvReplyInfo, (uint)lenInBytes, idInfo_logicalPeer.ui64Id, pMsgTask->idInfo_taskSender.ui64Id, pMsgTask->idInfo_taskReceiver.ui64Id, pMsgTask->idInfo_taskSender.ui64Id, uiChannelType, null, false)) goto errLabel;

                                    //  209/02/20
                                    if (rcd.uiType == Consts.CONST_imTaskType_transferAvInfo)
                                    {
                                        //assert(!m_var.av.taskInfo.bTaskExists);
                                        //memset(&m_var.av.taskInfo, 0, sizeof(m_var.av.taskInfo));
                                        m_var.av.taskInfo.memset0();
                                        m_var.av.taskInfo.bTaskExists = true;
                                        m_var.av.taskInfo.iTaskId = iTaskId;
                                        //  m_var.av.taskInfo.iIndex_sharedObj  =  ucbSendLocalAv  ?  iIndex_sharedObj  :  -1;
                                        m_var.av.taskInfo.idInfo_starter.ui64Id = rcd.idInfo_send.ui64Id;

                                        //
                                        m_var.av.taskInfo.uiInitW = (uint)pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth;
                                        m_var.av.taskInfo.uiInitH = (uint)pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight;

                                        //
                                        m_var.av.taskInfo.iIndex_taskInfo = iIndex_taskInfo;
                                        if (m_var.av.taskInfo.iIndex_taskInfo < 0)
                                        {
                                            qyFuncs.traceLog("dlgTalk_procTask_transferAvInfo failed, index_taskInfo <0");
                                            goto errLabel;
                                        }


                                        //
                                        m_var.av.taskInfo.tStartTime_org = rcd.tSendTime;
                                        m_var.av.taskInfo.uiTranNo_org = rcd.uiTranNo;
                                        m_var.av.taskInfo.uiContentType_org = rcd.uiContentType;

                                        //  2015/02/17
                                        m_var.av.taskInfo.local_avStream.tranInfo = transferAvReplyInfo.tranInfo;

                                        //  2010/08/31
                                        m_var.av.taskInfo.ucbVideoConference = transferAvReplyInfo_myTask.bVideoConference();

                                        //  2017/08/25
                                        m_var.av.taskInfo.usConfType = pContent->transferAvInfo.confCfg.usConfType;
                                        m_var.av.taskInfo.idInfo_initiator = pContent->transferAvInfo.confCfg.idInfo_initiator;
                                        int ii = (int)m_var.av.taskInfo.idInfo_initiator.ui64Id;
                                        //  2017/08/27
                                        //m_var.av.taskInfo.info_consult_yz = pContent->transferAvInfo.confCfg.info_consult_yz;

                                        //
                                        if (m_var.addr.uiObjType != Consts.CONST_objType_imGrp)
                                        {
                                            m_var.av.taskInfo.bShow_localVideo = true;
                                        }


                                        //
                                        fixed (TALK_drawLayout* pLayout = &m_var.m_drawLayout)
                                        {
                                            qyFuncs.mymemset((IntPtr)pLayout, 0, sizeof(TALK_drawLayout));
                                        }

                                        //  2014/11/12
                                        if (m_var.av.taskInfo.ucbVideoConference)
                                        {
                                            if (0 != uiTranNo_openAvDev_confMosaic_video || 0 != uiTranNo_openAvDev_confMosaic_resource)
                                            {
                                                m_var.av.taskInfo.mosaicTrans.bExists_confMosaic = true;
                                                m_var.av.taskInfo.mosaicTrans.uiTranNo_openAvDev_confMosaic_video = uiTranNo_openAvDev_confMosaic_video;
                                                m_var.av.taskInfo.mosaicTrans.uiTranNo_openAvDev_confMosaic_resource = uiTranNo_openAvDev_confMosaic_resource;
                                            }
                                        }

                                        //  2016/03/26
                                        if (m_var.av.taskInfo.ucbVideoConference)
                                        {
                                            //
                                            //  2017/07/06
                                            if (0 != pContent->transferAvInfo.confCfg.conf.confCtrl.ucbConfCtrl_starter)
                                            {
                                                m_var.av.taskInfo.ucbConfCtrl = true;
                                            }

                                            //  2018/11/01
                                            /*
                                            if (pQyMc.iCustomId == CONST_qyCustomId_hzj)
                                            {
                                                pProcInfo.hzj.bConfStarted = true;
                                                pProcInfo.hzj.idInfo_tmpGrp.ui64Id = m_var.addr.idInfo.ui64Id;
                                                //
                                                PostMessage(pProcInfo.hWall_hzj, CONST_qyWm_postComm, CONST_qyWmParam_confAccepted, 0);

                                            }
                                            */
                                        }

                                        //  2010/08/31

                                        //
                                        //  2017/09/03
                                        qyFuncs.resetTalkerList_mgr(hMgr);

                                        //
                                        uint uiTaskType = rcd.uiType;
                                        //MIS_CNT pMisCnt = m_var.pMisCnt;
                                        if (uiTaskType == Consts.CONST_imTaskType_transferAvInfo)
                                        {
                                            if (!m_var.av.taskInfo.ucbVideoConference)
                                            {
                                                if (0 != qyFuncs.addToMsgrs_sendLocalAv(pProcInfo, pMisCnt, m_var.addr.idInfo, false, ref pTaskInfo.var.curRoute_sendLocalAv)) goto errLabel;
                                            }
                                        }

                                        //
                                        if (!m_var.av.taskInfo.ucbVideoConference)
                                        {       //  2012/05/03. 在一对一接收时,自动将对方加入话筒列表,以便显示.
                                            {
                                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[0];

                                                pActiveMem_from.avStream.idInfo.ui64Id = m_var.addr.idInfo.ui64Id;
                                                pActiveMem_from.avStream.obj.tranInfo = pContent->transferAvInfo.ass.tranInfo;
                                                //_sntprintf(pActiveMem_from->desc, mycountof(pActiveMem_from->desc), _T("%s(%I64u)"), m_var.displayName, m_var.addr.idInfo.ui64Id);

                                                //_sntprintf(m_var.videoConference.activeMems_from[0].desc, mycountof(m_var.videoConference.activeMems_from[0].desc), _T("%s(%I64u)"), m_var.displayName, m_var.addr.idInfo.ui64Id);
                                            }
                                            //  2013/04/10
                                            if (!pParam.av_autoAnswer_av_task_bTaskNeedAutoAnswered)
                                            {
                                                DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[1];
                                                pActiveMem_from.avStream.idInfo.ui64Id = pMisCnt.idInfo.ui64Id;
                                                //_sntprintf(pActiveMem_from->desc, mycountof(pActiveMem_from->desc), _T("%s(%I64u)"), _T(""), m_var.pMisCnt->idInfo.ui64Id);
                                            }

                                        }
                                        //  2010/09/07
                                        if (ucbSendLocalAv)
                                        {               //
                                            if (!pParam.av_autoAnswer_av_task_bTaskNeedAutoAnswered)
                                            {
                                                qyFuncs.setCurSharedObjUsr_localAv(pProcInfo, iIndex_sharedObj, hDlgTalk, "dlgTalk_procTask_transferAvInfo.1260");
                                            }
                                            else
                                            {
                                                //
                                                bool bNeed_setCurSharedObjUsr = false;
                                                //
                                                /*
                                                ZONE_objs* pGZone = pProcInfo.pGZone_activeTalkers;
                                                if (isZoneObjIndexValid(pGZone->iIndex_enlarged1))
                                                {
                                                    ZONE_obj* pZoneObj = &pGZone->mems[pGZone->iIndex_enlarged1];
                                                    if (pZoneObj->hWndWallMem == hDlgTalk) bNeed_setCurSharedObjUsr = true;
                                                }
                                                //  2018/12/26
                                                if (pQyMc.iCustomId == CONST_qyCustomId_hbwj)
                                                {
                                                    bNeed_setCurSharedObjUsr = true;
                                                }
                                                */
                                                //
                                                if (bNeed_setCurSharedObjUsr)
                                                {
                                                    qyFuncs.setCurSharedObjUsr_localAv(pProcInfo, iIndex_sharedObj, hDlgTalk,"dlgTalk_procTask_transferAvInfo.1283");
                                                }
                                                else
                                                {
                                                    //
                                                    //InvalidateRect_mgr(hDlgTalk, 0, null, true);
                                                }
                                            }
                                        }

                                        //  2010/12/25
                                        //if  (  IsWindow(  m_var.av.hWnd_remoteAv_resourcesInfo  )  )  SendMessage(  m_var.av.hWnd_remoteAv_resourcesInfo,  WM_CLOSE,  0,  0  );
                                        closeInstantAssistant(hDlgTalk);

                                        //  2013/04/08
                                        //  sizeAllControls_dlgTalk(  hDlgTalk,  m_var,  null  );				//  
                                        dlgTalk_doLayout(hDlgTalk);

                                    }

                                    //
                                    if (rcd.uiType == Consts.CONST_imTaskType_transferAvInfo)
                                    {
                                        if (ucbSendLocalAv &&
                                            //m_var.addr.uiObjType  ==  CONST_objType_imGrp  
                                            0 != pContent->transferAvInfo.confCfg.ucbVideoConference
                                            )
                                        {
                                            //  2017/07/14
                                            if (pProcInfo.cfg.policy.avRules.ucbLetConfMgrSetMicOn)
                                            {
                                                /*
                                                if (!canJoinInBgWall(hMgr, pMgrVar))
                                                {
                                                    HWND hParent = null;
                                                    dlgTalk_doCmd_joinInBgWall(hMgr);
                                                }
                                                */
                                            }
                                            //
                                            //int get_ConfMem_confCtrl(QY_MESSENGER_ID* pIdInfo, ConfMem_confCtrl* pConfCtrl);
                                            //
                                            ConfMem_confCtrl confCtrl = new ConfMem_confCtrl();//{ 0 };
                                                                                               //get_ConfMem_confCtrl(&pMgrVar.addr.idInfo, &confCtrl);
                                                                                               //
                                            if (pMgrVar.av.taskInfo.bTaskExists
                                                && pMgrVar.av.taskInfo.idInfo_starter.ui64Id == confCtrl.idInfo_starter.ui64Id
                                                && pMgrVar.av.taskInfo.tStartTime_org == confCtrl.tStartTime
                                                && pMgrVar.av.taskInfo.uiTranNo_org == confCtrl.uiTranNo)
                                            {
                                                pMgrVar.av.taskInfo.confMem_confCtrl = confCtrl;
                                            }
                                            //
                                            if (pMgrVar.av.taskInfo.confMem_confCtrl.ucbPermitToSpeak)
                                            {
                                                //
                                                if (pProcInfo.cfg.policy.avRules.ucbLetConfMgrSetMicOn)
                                                {

                                                    //  2017/07/13
                                                    qyFuncs.setCurSharedObjUsr_localAv(pProcInfo, pMgrVar.av.iIndex_sharedObj_localAv, hMgr,"dlgTalk_procTask_transferAvInfo.1343");

                                                    dlgTalk_requestToSpeak(hMgr, true);
                                                }
                                            }

                                        }
                                        //
                                        setProcessingAvTask(pMgrVar.av.taskInfo.iIndex_taskInfo, pMgrVar.av.taskInfo.iTaskId);
                                        //  2017/08/28
                                        /*
                                        if (pMgrVar.av.taskInfo.usConfType == CONST_usConfType_consult)
                                        {
                                            HWND tmphShadow = null;
                                            getTalker_inputShadow(hCurTalk, &tmphShadow);
                                                //										
                                                ::PostMessage(tmphShadow, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                                        }
                                        */
                                        //

                                        //
                                    }
                                    else if (rcd.uiType == Consts.CONST_imTaskType_remoteAssist)
                                    {
                                        /*
                                        PARAM_dlgVideos   params  =  { 0};
                                                  //
                                                  params.iTaskId = rcd.id;
                                                  //  params.m_var.uiTaskType  =  rcd.uiType;	//  2009/08/05
                                                  params.idInfo_sender.ui64Id = pMsgTask->idInfo_taskSender.ui64Id;
                                                  params.uiTranNo_openAvDev = pContent->transferAvInfo.tranInfo.video.uiTranNo_openAvDev;
                                                  //
                                                  params.pImgs = &m_var.av.peerZone.images;
                                                  params.iIndex_capImage = -1;
                                                  params.usCnt_monPics_row = 1;
                                                  params.usCnt_monPics_col = 1;
                                                  params.uiInitW = pContent->transferAvInfo.tranInfo.video.vh_decompress.bih.biWidth;
                                                  params.uiInitH = pContent->transferAvInfo.tranInfo.video.vh_decompress.bih.biHeight;
                                                  params.bScrollBarEnabled = true; 
                                                  params.bRemoteAssist = true;

                                        //  2012/05/10
                                        //params.hParent  =  hCurTalk;

                                        //  2017/08/23
                                        if (pQyMc.iCustomId == CONST_qyCustomId_bjyz)
                                        {
                                            break;
                                        }

                                        //
                                        if (showRemoteAssistWnd(hCurTalk, hDlgTalk, true, &params, &m_var.subWnds)) goto errLabel;
                                        */

                                    }

                                }
                            }

                            //
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                qyFuncs.showInfo_open(0, "", "dlgTalk_procTask_transferAvInfo exception");
                goto errLabel;
            }

            //
            refreshTalkToInfo_mgr(hDlgTalk);

            iErr = 0;
        errLabel:

            if (0 != iErr)
            {
                if (ucbGot_iIndex_sharedObj_localAv)
                {
                    //  
                    //freeSharedObjUsr_localAv(  m_var.av.iIndex_sharedObj_localAv,  hDlgTalk  );
                    //m_var.av.ucbGot_iIndex_sharedObj_localAv  =  false;
                    dlgTalk_closeTaskAv(hDlgTalk, m_var, "dlgTalk_procTask_transferAvInfo.failed");
                }
            }

            //  2010/12/24	
            chkResources(hDlgTalk, false);
            if (bPrepareSet)
            {
                m_var.av.taskInfo.bPrepare = false;

            }
            //
            str = string.Format("dlgTalk_procTask_transferAvInfo leaves "  +  iErr );
            qyFuncs.showInfo_open(0, null, str);


            return iErr;

        }

#endif

    }
}