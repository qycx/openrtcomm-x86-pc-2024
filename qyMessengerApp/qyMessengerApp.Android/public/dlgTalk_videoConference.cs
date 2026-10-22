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
using static Android.Media.Audiofx.DynamicsProcessing;
using static Android.Provider.Contacts.Intents;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {




        //int  requestToSpeak_videoConferenceStarter(  HWND  hDlgTalk,  QY_MESSENGER_ID  *  pIdInfo,  bool  bRequestToSpeak  )
        public static unsafe int confStarter_requestToSpeak(object hDlgTalk, QY_MESSENGER_ID idInfo, ref AV_stream_simple pAss, string talkerDesc, bool bRequestToSpeak)

        {
            int iErr = -1;
            QY_MESSENGER_ID* pIdInfo = &idInfo;

            if (0 == pIdInfo->ui64Id) return -1;
            if (null == talkerDesc) talkerDesc = _T("");

            string str;


            //
            // using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            {
                ;
                DLG_TALK_var pm_var = dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pMisCnt->pProcInfoParam;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
                //
                FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
                if (null == pFuncs) return -1;

                //
                str = string.Format("confStarter_requestToSpeak called");
                qyFuncs.showInfo_open(0, null, str);

                //
                if (!m_var.av.taskInfo.bTaskExists) return -1;
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "confStarter_requestToSpeak");
                if (null == pTaskInfo) return -1;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null) return -1;
                if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
                QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                //
                //DLG_TALK_videoConference pVc = videoConference; ;
                int i;
                int index_activeMems_from;
                bool bNeedRefresh_requestingMems_from = false;
                bool bNeedRefresh_display = false;

                if (0 == pIdInfo->ui64Id) return -1;

                if (refreshTaskAv_videoConferenceStarter(hDlgTalk, (IntPtr)null) < 0) return -1;

#if __DEBUG__
				printDlgTalkVideoConference(&m_var.videoConference);
#endif
                //
                RES_obj_simple ros;
                qyFuncs.mymemset((IntPtr)(byte*)&ros, 0, sizeof(RES_obj_simple));
                //if (pAss != null)
                {
                    ros = pAss.resObj;
                }

                //
                if (bRequestToSpeak)
                {
                    int index_active = 0;

                    if (!qyFuncs.isMemFromActive(pTc, *pIdInfo, ref ros, &index_active))
                    {
                        for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
                        {
                            if (0 == videoConference.activeMems_from[i].avStream.idInfo.ui64Id) break;
                        }
                        if (i == videoConference.usCntLimit_activeMems_from)
                        {
#if __DEBUG__
							traceLogA("ativeMems_from is full, try to add to requestingMems");
#endif
                            /*
							TCHAR tBuf[128];
							_sntprintf(tBuf, mycountof(tBuf), _T("%I64u %s. %s %d"), pIdInfo->ui64Id, getResStr(0, &pQyMc->cusRes, CONST_resId_cannotSpeak), getResStr(0, &pQyMc->cusRes, CONST_resId_micListFull), (int)pVc.usCntLimit_activeMems_from);
							showNotification_open(0, 0, 0, tBuf);
							showNotification_d3d(hDlgTalk, tBuf);
							*/
                            //
                            if (!isMemFromRequesting(pTc, *pIdInfo))
                            {
                                if (0 == addTo_requestingMems_from(pMisCnt, pTc, *pIdInfo)) bNeedRefresh_requestingMems_from = true;
                            }
                        }
                        else
                        {
                            index_activeMems_from = i;

                            //
                            DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];

                            //
                            pActiveMem_from.memset0();

                            //
                            videoConference.activeMems_from[index_activeMems_from].avStream.idInfo.ui64Id = pIdInfo->ui64Id;
                            //
                            //if (pAss != null)
                            {
                                pActiveMem_from.avStream.obj = pAss;
                            }
                            fixed (char* pDesc = pActiveMem_from.desc.desc)

                                if (talkerDesc.Length != 0)
                                {
                                    qyFuncs.safeTcsnCpy(talkerDesc, pDesc, Consts.cntof_desc);
                                }
                                else
                                {
                                    //qyFuncs.M_getActiveMemDesc(pMisCnt, pVc.activeMems_from[index_activeMems_from].mem.idInfo, pVc.activeMems_from[index_activeMems_from].desc, mycountof(pVc.activeMems_from[index_activeMems_from].desc));
                                    qyFuncs.M_getActiveMemDesc(pMisCnt, videoConference.activeMems_from[index_activeMems_from].avStream.idInfo, pDesc, Consts.cntof_desc);
                                }
                            //
                            videoConference.bNeedRefresh_activeMems_from = true;

                            //traceLogA("alloc activeMems_from[%d] for %I64u, %d", index_activeMems_from, pIdInfo->ui64Id, i);

                            if (isMemFromRequesting(pTc, *pIdInfo))
                            {
                                removeFrom_requestingMems_from(pTc, *pIdInfo);
                                bNeedRefresh_requestingMems_from = true;
                            }
                        }

                    }
                    else
                    {
                        DLG_TALK_videoConferenceActiveMemFrom pActive = videoConference.activeMems_from[index_active];
                        //if (pAss != null)
                        {
                            if (pAss.tranInfo.audio.uiTranNo_openAvDev == 0 && pAss.tranInfo.video.uiTranNo_openAvDev == 0)
                            {
                                goto errLabel;
                            }
                            if (pActive.avStream.obj.tranInfo.audio.uiTranNo_openAvDev != pAss.tranInfo.audio.uiTranNo_openAvDev
                                || pActive.avStream.obj.tranInfo.video.uiTranNo_openAvDev != pAss.tranInfo.video.uiTranNo_openAvDev)
                            {
                                pActive.avStream.obj.tranInfo = pAss.tranInfo;
                                //
                                qyFuncs.showInfo_open(0, null, _T("update activeMem"));
                                //
                                videoConference.bNeedRefresh_activeMems_from = true;
                            }
                        }
                    }

                }
                else
                {  //  stop speaking
                    for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
                    {
                        if (videoConference.activeMems_from[i].avStream.idInfo.ui64Id == pIdInfo->ui64Id) break;
                    }
                    if (i < videoConference.usCntLimit_activeMems_from)
                    {
                        DLG_TALK_videoConferenceActiveMemFrom p = videoConference.activeMems_from[i];
                        {
                            //memset(&pVc.activeMems_from[i], 0, sizeof(pVc.activeMems_from[i]));
                            //qyFuncs.mymemset((IntPtr)p, 0, sizeof(DLG_TALK_videoConference_activeMemFrom));
                            p.memset0();

                        }
                        //
                        videoConference.bNeedRefresh_activeMems_from = true;
                    }
                    //
                    if (isMemFromRequesting(pTc, *pIdInfo))
                    {
                        removeFrom_requestingMems_from(pTc, *pIdInfo);
                        bNeedRefresh_requestingMems_from = true;
                    }
                }

                if (videoConference.bNeedRefresh_activeMems_from)
                {
                    if (refreshTaskAv_videoConferenceStarter(hDlgTalk, (IntPtr)null) < 0) goto errLabel;
                    bNeedRefresh_display = true;

                    //  2010/09/12
                    if (!bRequestToSpeak)
                    {
                        //dlgTalk_freeCapImageBySth_mgr(hDlgTalk, m_var.av.taskInfo.iTaskId, pIdInfo);    //  2010/09/12
                    }
                }
                if (bNeedRefresh_requestingMems_from) bNeedRefresh_display = true;

                if (pIdInfo->ui64Id == pMisCnt.idInfo.ui64Id)
                {   //  It is me
                    QY_SHARED_OBJ pSharedObj = null;
                    SHARED_OBJ_USR pSharedObjUsr = null;
                    RES_obj_simple ros1 = new RES_obj_simple();
                    bool ucbNotSendData = !qyFuncs.isMemFromActive(pTc, *pIdInfo, ref ros1, null);

#if __DEBUG__
					traceLogA("requestToSpeak: iIndex_sharedObj %d", m_var.av.iIndex_sharedObj_localAv);
#endif

                    pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                    if (null == pSharedObj) goto errLabel;
                    pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, m_var.av.iIndex_usr_localAv);
                    if (null == pSharedObjUsr) goto errLabel;

                    if (pTc.ucbNotSendData != ucbNotSendData)
                    {
                        pTc.ucbNotSendData = ucbNotSendData;
                        bNeedRefresh_display = true;
                    }
                    //
                    if (!ucbNotSendData)
                    {

                        //  2009/05/30	
#if false
						fixed (int* p_iIndex_activeMems_from = &pSharedObjUsr.iIndex_activeMems_from_videoConferenceStarter)
						{
							isMemFromActive(ref pVc, *pIdInfo, p_iIndex_activeMems_from);
						}
#endif

                        //
                        if (pSharedObj.var.iIndex_curUsr == m_var.av.iIndex_usr_localAv)
                        {
                            //
                            if (!m_var.av.taskInfo.ucbVideoConference)
                            {
#if __DEBUG__
								if (!pSharedObj.curRoute_sendLocalAv.route.idInfo_to.ui64Id)
								{
									traceLog(_T("requestToSpeak_videoConferenceStarter: inInfo_to error"));
								}
#endif
                            }
                            else
                            {
                                if (0 == pTaskInfo.var.curRoute_sendLocalAv.videoConference_idInfo_to.ui64Id)
                                {
                                    if (0 != qyFuncs.addToMsgrs_sendLocalAv(pProcInfo, pMisCnt, m_var.addr.idInfo, true, ref pTaskInfo.var.curRoute_sendLocalAv))
                                    {
#if __DEBUG__
										traceLogA("requestToSpeak_videoConferenceStarter: addToMsgrs_sendLocalAv failed.");     //  对视频会议的情况，如果已经有了，就不能再加进去了。但是不妨碍其他人参与视频会议。所以不能退出。
#endif
                                        //
                                        goto errLabel;
                                    }
                                }
                            }

                            //  2014/08/25
                            //int  setFlg_forceKeyFrame(  int  iIndex_sharedObj  )
                            qyFuncs.setFlg_forceKeyFrame(pProcInfo, m_var.av.iIndex_sharedObj_localAv);

                            //
                            //pFuncs.pf_resumeLocalAudioRecorder(m_var.av.iIndex_sharedObj_localAv);

                            //  2017/09/13
                            //pFuncs.pf_resumeLocalVideo(pProcInfo, m_var.av.iIndex_sharedObj_localAv);

                        }
                    }

                }

                if (bNeedRefresh_display)
                {
                    refreshTalkerList(hDlgTalk);

                }

            }
            //  2013/07/20
            //  refreshImgs_d3dWall(  hDlgTalk  );
            //dyn_d3dWall_refreshWallMemByTalker(hDlgTalk);

            //
            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int confOthers_requestToSpeak(object hDlgTalk, bool bRequestToSpeak)
        {
            int iErr = -1;

            //using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            {
                ;
                DLG_TALK_var pm_var = dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

                //
                //
                if (!m_var.av.taskInfo.bTaskExists) return -1;
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "confOthers_requestToSpeak");
                if (null == pTaskInfo) return -1;

                //
                int iTaskId = m_var.av.taskInfo.iTaskId;

                //
                FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
                if (null == pFuncs) return -1;
                //
                TASK_INTERACTION_REQ req;
                int lenInBytes = 0;

                if (bRequestToSpeak)
                {
                    QY_SHARED_OBJ pSharedObj = null;

                    if (null == (pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv))) goto errLabel;

                    if (pSharedObj.var.iIndex_curUsr == m_var.av.iIndex_usr_localAv)
                    {
                        if (0 == pTaskInfo.var.curRoute_sendLocalAv.route.idInfo_to.ui64Id)
                        {
#if __DEBUG__
							traceLog(_T("Warning: requestToSpeak_videoConferenceOthers: idInfo_to is 0"));
#endif
                            //
                            if (0 != qyFuncs.addToMsgrs_sendLocalAv(pProcInfo, pMisCnt, m_var.av.taskInfo.idInfo_starter, false, ref pTaskInfo.var.curRoute_sendLocalAv))
                            {
#if __DEBUG__
								traceLogA("requestToSpeak_videoConferenceOthers: addToMsgrs_sendLocalAv failed.");      //  对视频会议的情况，如果已经有了，就不能再加进去了。但是不妨碍其他人参与视频会议。所以不能退出。
#endif
                                goto errLabel;
                            }
                        }

                        //  2014/08/25			
                        qyFuncs.setFlg_forceKeyFrame(pProcInfo, m_var.av.iIndex_sharedObj_localAv);

                        //
                        //pFuncs->pf_resumeLocalAudioRecorder(m_var.av.iIndex_sharedObj_localAv);

                    }
                }

                qyFuncs.mymemset((IntPtr)(byte*)&req, 0, sizeof(TASK_INTERACTION_REQ));
                req.uiType = Consts.CONST_imCommType_taskInteractionReq;
                req.usOp = (ushort)(bRequestToSpeak ? Consts.CONST_imOp_requestToSpeak : Consts.CONST_imOp_stopSpeaking);
                req.tStartTime_org = m_var.av.taskInfo.tStartTime_org;
                req.uiTranNo_org = m_var.av.taskInfo.uiTranNo_org;
                //req.uiContentType_org  =  m_var.av.taskInfo.uiContentType_org;
                //
                //  2015/02/17
                if (bRequestToSpeak)
                {
                    req.avStream.obj.tranInfo = m_var.av.taskInfo.local_avStream.tranInfo;

                    //
                    qyFuncs.getTalkerDesc(pMisCnt, pMisCnt.idInfo, req.talkerDesc, Consts.cntof_talkerDesc, null, 0);

                    //
#if __DEBUG__
#endif
                }
                //
                if (m_var.av.taskInfo.ucbVideoConference) req.idInfo_imGrp_related.ui64Id = m_var.addr.idInfo.ui64Id;
                //	
                lenInBytes = sizeof(TASK_INTERACTION_REQ);
                //
                Int64 tStartTran; uint uiTranNo;
                qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
                //	
                QY_MESSENGER_ID idInfo_dst = m_var.av.taskInfo.idInfo_starter;
                if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendTask, tStartTran, uiTranNo, 0, 0, 0, (byte*)&req, (uint)lenInBytes, 0, 0, 0, idInfo_dst.ui64Id, 0, null, false)) goto errLabel;

                //
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("%s"), bRequestToSpeak ? _T("Request to speak") : _T("Request to stop speaking"));
            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        //


        //
        public static unsafe int refreshTaskAv_videoConferenceStarter(object hDlgTalk, IntPtr pMsgTask)
        {
            //using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            {
                DLG_TALK_var pm_var = dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk, _T("refreshTaskAv_videoConferenceStarter"));
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //  (  MC_VAR_isCli  *  )pMisCnt->pProcInfoParam;

                if (!m_var.av.taskInfo.bTaskExists) return 0;
                //  if  (  !m_var.av.taskInfo.ucbVideoConferenceStarter  )  return  0;
                if (!m_var.av.taskInfo.ucbStarter) return 0;

                //return pProcInfo.processQ_media.qTraverse(tmpHandler_refreshTaskAv_videoConferenceStarter, hDlgTalk, pMsgTask);
                return 0;
            }
        }





        //  2009/05/31
        public static unsafe int refreshTaskAv_videoConferenceOthers(object hDlg)
        {
            // using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlg = (Activity_dlgTalk)hDlg;

            {

                DLG_TALK_var pm_var = dlg.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlg); ;
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;

                if (!m_var.av.taskInfo.bTaskExists) return 0;
                //  if  (  m_var.av.taskInfo.ucbVideoConferenceStarter  )  return  0;
                if (m_var.av.taskInfo.ucbStarter) return 0;

                //return pProcInfo.processQ_media.qTraverse(tmpHandler_refreshTaskAv_videoConferenceOthers, 0, (object)hDlg);
                return 0;
            }
        }


        //
        public static unsafe int getConfKey(object hDlgTalk, int iTaskId, CONF_KEY* pConfKey, string hint)
        {
            int iErr = -1;

            //
            //  using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            {
                ;
                DLG_TALK_var pm_var = dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

                //
                if (null == pConfKey) return -1;
                ref CONF_KEY status = ref *pConfKey;
                int i;
                int lenInBytes;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                if (null == hint) hint = _T("");

                //
                //TCHAR tBuf[128];
                string str;

                //
                //if  (  !pIdInfo_to  ||  !pIdInfo_to->ui64Id  )  return  -1;

                if (!m_var.av.taskInfo.bTaskExists) return -1;
                if (!m_var.av.taskInfo.ucbStarter) return -1;

                //
                if (m_var.av.taskInfo.iTaskId != iTaskId) return -1;

                //
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "getConfKey");
                if (null == pTaskInfo) return -1;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (null == pTaskData) return -1;
                if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
                QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;

                //		
                DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                //
                qyFuncs.mymemset((IntPtr)(byte*)pConfKey, 0, sizeof(CONF_KEY));
                status.uiType = Consts.CONST_imCommType_confKey;
                if (m_var.av.taskInfo.ucbVideoConference) status.idInfo_imGrp_related.ui64Id = m_var.addr.idInfo.ui64Id;

                //  2010/12/29
                if (m_var.av.taskInfo.bTaskExists && m_var.av.taskInfo.ucbStarter)
                {
                    status.usLayoutType_starter = m_var.m_layout.usLayoutType_cur;//  dlgTalk_get_usLayoutType_local(  hDlgTalk  );	//  m_var.av.usLayoutType_local;
                }

                //
                if (m_var.av.taskInfo.ucbVideoConference
                    && m_var.av.taskInfo.ucbStarter)
                {
#if false
					QMC_mosaic_maker* pMosaicMaker = &m_var.av.taskInfo.mosaicMaker;
					MOSAIC_TRAN_INFO mti;
					switch (status.usLayoutType_starter)
					{
						case CONST_usLayoutType_training:
							status.mems[status.usCnt].avStream.idInfo = pMisCnt->idInfo;
							status.mems[status.usCnt].avStream.obj.resObj = pMosaicMaker->resource.resObj;
							getMosaicTranInfo(&pMosaicMaker->resource, &mti);
							status.mems[status.usCnt].avStream.obj.tranInfo.video = mti.video;
							//status.mems[status.usCnt].ucbActive  =  true;
							status.usCnt++;
							break;
						case CONST_usLayoutType_dialog:
						default:
							status.mems[status.usCnt].avStream.idInfo = pMisCnt->idInfo;
							status.mems[status.usCnt].avStream.obj.resObj = pMosaicMaker->video.resObj;
							getMosaicTranInfo(&pMosaicMaker->video, &mti);
							status.mems[status.usCnt].avStream.obj.tranInfo.video = mti.video;
							//status.mems[status.usCnt].ucbActive  =  true;
							status.usCnt++;
							break;
					}
#endif
                }


                //
                VIDEO_CONFERENCE_MEM* pstatus_mems = (VIDEO_CONFERENCE_MEM*)pConfKey->buf_VIDEO_CONFERENCE_MEM_mems;
                for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
                {
                    if (0 == videoConference.activeMems_from[i].avStream.idInfo.ui64Id) continue;
                    //
                    if (status.usCnt >= Consts.MAX_conf_activeMems)
                    {
                        qyFuncs.showInfo_open(0, null, _T("sendConfKey err: status too many mems, can't add a activeMem"));
                        break;
                    }
                    //  status.mems[status.usCnt].idInfo.ui64Id  =  pVc->activeMems_from[i].mem.idInfo.ui64Id;
                    //memcpy(  &status.mems[status.usCnt].avStream.idInfo,  &pVc->activeMems_from[i].avStream.idInfo,  sizeof(  status.mems[status.usCnt].avStream.idInfo  )  );
                    //memcpy(  &status.mems[status.usCnt].avStream.obj.tranInfo,  &pVc->activeMems_from[i].avStream.obj.tranInfo,  sizeof(  status.mems[status.usCnt].avStream.obj.tranInfo  )  );	//  2009/05/31
                    pstatus_mems[status.usCnt].avStream = videoConference.activeMems_from[i].avStream;
                    //
                    //status.mems[status.usCnt].ucbActive  =  TRUE;
                    //
                    //  2013/07/18. 需要将第几个话筒的属性传给所有的成员,以便协调显示格局
                    pstatus_mems[status.usCnt].usIndex_speaker = (ushort)i;
                    //
                    status.usCnt++;
                }

                //
#if false
				ref CONF_requesting_mems  requestingMems = ref status.requestingMems;
				for (i = 0; i < Consts.MAX_conf_requestingMems; i++)
				{
					if (0==pVc.requestingMems_from[i].avStream.idInfo.ui64Id) continue;
					//
					if (requestingMems.usCnt >= Consts.MAX_conf_requestingMems)
					{
						qyFuncs.showInfo_open(0, null, _T("sendConfKey err: status too many mems, can't add a requestingMem"));
						break;
					}
					//
					CONF_requesting_mem* prequestingMems = (CONF_requesting_mem*)pStatus->requestingMems.buf_CONF_requesting_mems;
					prequestingMems[requestingMems.usCnt].idInfo.ui64Id = pVc.requestingMems_from[i].avStream.idInfo.ui64Id;
					//
					requestingMems.usCnt++;
				}
#endif

            }


            iErr = 0;
        errLabel:


            return iErr;
        }



        //
        /*
        public static unsafe int getStatusConfLayoutParam(object hDlgTalk, int iTaskId, StatusConfLayoutParam* pStatus, string hint)
        {

            int iErr = -1;

            using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            {
                ;
                DLG_TALK_var pm_var = (DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                if (null == pm_var) return -1;

                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pMisCnt->pProcInfoParam;
                CTX_caller ctxCaller = mainWnd_getCtxCaller(pQyMc.gui.hMainWnd);                  //     MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                                                                                                  //  MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

                //  MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
                if (null == pStatus) return -1;
                StatusConfLayoutParam status = *pStatus;

                int i;
                int lenInBytes;

                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                //  MIS_CNT* pMisCnt = m_var.pMisCnt;
                if (null == hint) hint = _T("");

                //
                //  byte tBuf[128];

                //
                //if  (  !pIdInfo_to  ||  !pIdInfo_to->ui64Id  )  return  -1;

                if (!m_var.av.taskInfo.bTaskExists) return -1;
                if (!m_var.av.taskInfo.ucbStarter) return -1;

                //
                if (m_var.av.taskInfo.iTaskId != iTaskId) return -1;

                //
                // QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pMgrVar.av.taskInfo.iIndex_taskInfo, "dlgTalk_requestToSpeak");
              


            }
            //
            iErr = 0;
        errLabel:


            return iErr;
        }
        */



        //
        public static unsafe int sendConfKey(object hDlgTalk, QY_MESSENGER_ID idInfo_to, string hint)
        {
            int iErr = -1;

            /*
            CONF_KEY status;

            QY_MESSENGER_ID* pIdInfo_to = &idInfo_to;

            int lenInBytes;

            //
            if (null == pIdInfo_to || 0 == pIdInfo_to->ui64Id) return -1;


            //  using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            {
                ;
                DLG_TALK_var pm_var = dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pMisCnt->pProcInfoParam;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

                //
                if (!m_var.av.taskInfo.bTaskExists) return -1;
                if (!m_var.av.taskInfo.ucbStarter) return -1;

                if (0 != getConfKey(hDlgTalk, m_var.av.taskInfo.iTaskId, &status, _T("sendVideoConferenctStatus"))) goto errLabel;


                lenInBytes = sizeof(CONF_KEY);
                //
                Int64 tStartTran; uint uiTranNo;
                qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
                //	
                if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendTask, tStartTran, uiTranNo, 0, 0, 0, (byte*)&status, (uint)lenInBytes, 0, 0, 0, pIdInfo_to->ui64Id, 0, null, false)) goto errLabel;


            }
            */

            iErr = 0;
        errLabel:
            return iErr;
        }

        //public static unsafe bool isMemFromRequesting(DLG_TALK_videoConference pVc, QY_MESSENGER_ID idInfo_from)
        public static unsafe bool isMemFromRequesting(QMC_taskData_conf pTc, QY_MESSENGER_ID idInfo_from)
        {
            int i;
            QY_MESSENGER_ID* pIdInfo_from = &idInfo_from;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            for (i = 0; i < videoConference.requestingMems_from.Length; i++)
            {
                if (pIdInfo_from->ui64Id == videoConference.requestingMems_from[i].avStream.idInfo.ui64Id) break;
            }
            if (i == videoConference.requestingMems_from.Length) return false;

            return true;
        }

        //public static unsafe int addTo_requestingMems_from(MIS_CNT pMisCnt, DLG_TALK_videoConference pVc, QY_MESSENGER_ID idInfo)
        public static unsafe int addTo_requestingMems_from(MIS_CNT pMisCnt, QMC_taskData_conf pTc, QY_MESSENGER_ID idInfo)
        {
            int i;
            QY_MESSENGER_ID* pIdInfo = &idInfo;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            //
            if (null == pIdInfo || 0 == pIdInfo->ui64Id) return -1;

            if (isMemFromRequesting(pTc, *pIdInfo)) return 0;
            for (i = 0; i < videoConference.requestingMems_from.Length; i++)
            {
                if (0 == videoConference.requestingMems_from[i].avStream.idInfo.ui64Id) break;
            }
            if (i == (videoConference.requestingMems_from.Length)) return -1;

            videoConference.requestingMems_from[i].avStream.idInfo.ui64Id = pIdInfo->ui64Id;

            fixed (char* pDesc = videoConference.requestingMems_from[i].desc.desc)
            {
                //  getTalkerDesc(  pMisCnt,  pIdInfo,  pVc.requestingMems_from[i].desc,  mycountof(  pVc.requestingMems_from[i].desc  ),  0,  0  );
                qyFuncs.M_getActiveMemDesc(pMisCnt, *pIdInfo, pDesc, Consts.cntof_desc);
            }

            return 0;
        }

        //public static unsafe int removeFrom_requestingMems_from(DLG_TALK_videoConference pVc, QY_MESSENGER_ID pIdInfo)
        public static unsafe int removeFrom_requestingMems_from(QMC_taskData_conf pTc, QY_MESSENGER_ID pIdInfo)
        {
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

            int i;
            for (i = 0; i < (videoConference.requestingMems_from.Length); i++)
            {
                if (videoConference.requestingMems_from[i].avStream.idInfo.ui64Id == pIdInfo.ui64Id)
                {
                    DLG_TALK_videoConference_requestingMems_from p = videoConference.requestingMems_from[i];
                    {
                        //mymemset(&pVc.requestingMems_from[i], 0, sizeof(pVc.requestingMems_from[i]));
                        //qyFuncs.mymemset((IntPtr)p, 0, sizeof(DLG_TALK_videoConference_requestingMems_from));
                        p.memset0();
                    }
                    break;
                }
            }

            return 0;
        }


        public static unsafe int dlgTalk_requestToSpeak(object hDlgTalk_mgr, bool bEnable)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            CTX_caller ctxCaller = mainWnd_getCtxCaller(pQyMc.gui.hMainWnd);
            string str;

            //
            //using (CHelp_getDlgTalkVar help_getDlgTalkVar_mgr = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk_mgr = (Activity_dlgTalk)hDlgTalk_mgr;

            {
                ;
                DLG_TALK_var pMgrVar = dlgTalk_mgr.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar_mgr.getVar(hDlgTalk_mgr);
                if (null == pMgrVar) return -1;
                if (!isTalkerShadowMgr(ref pMgrVar.addr)) return -1;
                //TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)pMgrVar.pShadowInfo;
                //if (!pShadowMgr) return -1;
                //TCHAR tBuf[128];

                //
                if (!pMgrVar.av.taskInfo.bTaskExists) return -1;
                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pMgrVar.av.taskInfo.iIndex_taskInfo, "dlgTalk_requestToSpeak");
                if (null == pTaskInfo) goto errLabel;
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (null == pTaskData) goto errLabel;
                QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;


                //  2017/07/11
                if (bEnable)
                {
                    if (pMgrVar.av.taskInfo.bTaskExists
                          && pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp
                          && pMgrVar.av.taskInfo.ucbConfCtrl
                          && !pMgrVar.av.taskInfo.ucbStarter
                          //&& !pShadowMgr->av.ucbMeConfMgr
                          )
                    {
                        if (!pMgrVar.av.taskInfo.confMem_confCtrl.ucbPermitToSpeak)
                        {
                            //_sntprintf(tBuf, mycountof(tBuf), getResStr(0, &pQyMc->cusRes, CONST_resId_cannotSpeak));
                            str = new string(qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_cannotSpeak));
                            qyFuncs.showNotification(0, 0, 0, 0, 0, 0, str);
                            goto errLabel;
                        }
                    }

                    //
                    if (pMgrVar.av.taskInfo.ucbVideoConference
                        && !pMgrVar.av.taskInfo.ucbStarter)
                    {
                        //
                        if (!qyFuncs.isActiveMem(ctxCaller, pMgrVar.av.taskInfo.iTaskId, pMisCnt.idInfo))
                        {
                            if (qyFuncs.isActiveMemsFull(ctxCaller, pMgrVar.av.taskInfo.iTaskId, true))
                            {
                                str = string.Format(qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_tooManySpeaks));
                                qyFuncs.showNotification(0, 0, 0, 0, 0, 0, str);
                                goto errLabel;
                            }
                        }

                    }

                }






                //  2015/12/08
#if DEBUG
                if (bEnable) qyFuncs.showInfo_open(0, null, _T("requestToSpeak"));
#endif

                //
                //  BOOL							bEnableState	=	m_var.canSpeak;
                bool bEnableState = dlgTalk_canSpeak(hDlgTalk_mgr);

                //  2010/12/19
                //pMgrVar.av.taskInfo.bEnableToSpeak = bEnable;
                pTc.ucbNotSendData = !bEnable;


                if (pMgrVar.av.taskInfo.ucbStarter)
                {
                    AV_stream_simple ass = pMgrVar.av.taskInfo.local_avStream;
                    confStarter_requestToSpeak(hDlgTalk_mgr, pMisCnt.idInfo, ref ass, null, bEnable);
                    //if ((bEnable && !bEnableState) || (!bEnable && bEnableState))
                    {
                        sendConfKey(hDlgTalk_mgr, pMgrVar.addr.idInfo, "");
                    }
                }
                else
                {
                    //  if  (  (  bEnable  &&  !bEnableState  )  ||  (  !bEnable  &&  bEnableState  )  )  
                    {
                        confOthers_requestToSpeak(hDlgTalk_mgr, bEnable);
                    }
                }

            }

            iErr = 0;
        errLabel:
            return iErr;
        }





        public static unsafe int sendConfLayout(object hDlgTalk, byte ucbResp, short usLayoutType, short usOp, ref ConfLayoutParam pParam, QY_MESSENGER_ID idInfo_imGrp_related, QY_MESSENGER_ID idInfo_requester, QY_MESSENGER_ID idInfo_to, string hint)

        {
            int iErr = -1;
            //CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar();
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            DLG_TALK_var pm_var = dlgTalk.m_var;
            //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
            if (null == pm_var) return -1;
            DLG_TALK_var m_var = pm_var;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )pMisCnt->pProcInfoParam;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            CONF_LAYOUT layout;
            int i;
            int lenInBytes;

            if (null == hint) hint = _T("");


            //
            QY_MESSENGER_ID* pIdInfo_to = &idInfo_to;
            if (null == pIdInfo_to || 0 == pIdInfo_to->ui64Id) return -1;

            //
            //if  (  !m_var.av.taskInfo.bTaskExists  )  return  -1;
            //
            //if  (  !m_var.av.taskInfo.ucbStarter  )  return  -1;

            //
            //memset(&layout, 0, sizeof(layout));
            layout.uiType = Consts.CONST_imCommType_confLayout;
            //if  (  m_var.av.taskInfo.ucbVideoConference  )  layout.idInfo_imGrp_related.ui64Id  =  m_var.addr.idInfo.ui64Id;
            layout.idInfo_imGrp_related.ui64Id = idInfo_imGrp_related.ui64Id;

            layout.ucbResp = ucbResp;

            layout.usLayoutType = usLayoutType;
            //
            layout.usOp = usOp;
            //
            ConfLayoutParam pPParam = pParam;
            layout.confLayoutParam = pParam;

            //
            lenInBytes = sizeof(CONF_LAYOUT);
            //	
            Int64 tStartTran; uint uiTranNo;
            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
            //	
            if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, Consts.CONST_qyCmd_sendTask, tStartTran, uiTranNo, 0, 0, 0, (byte*)&layout, (uint)lenInBytes, 0, 0, 0, pIdInfo_to->ui64Id, 0, null, false)) goto errLabel;

            iErr = 0;

        errLabel:
            return iErr;
        }





    }
}