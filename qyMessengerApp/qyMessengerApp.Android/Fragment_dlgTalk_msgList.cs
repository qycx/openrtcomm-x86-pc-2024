
using Android.Content;
using Net=Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

//
using System;

using qy;
using qy.Droid;
using System.Runtime.InteropServices;
using Pub.Devrel.Easypermissions;
using System.Collections.Generic;
using AndroidX.Fragment.App;
using Android.Content.PM;
using qyMessengerApp.Android;
using AndroidX.AppCompat.App;


//
namespace qyMessengerApp.Droid
{
    public class Fragment_dlgTalk_msgList : AndroidX.Fragment.App.Fragment, View.IOnTouchListener
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        Adapter_dlgTalk_msgList mAdapter;
        //
        TextView m_txt_back;
        TextView m_txt_title;
        TextView m_txt_more;
        //
        EditText m_editText_input;
        Button m_button_snd;
        Button m_button_avCall;
        Button m_button_file;
        //
        Activity_dlgTalk mhTalk;
        //
        RelativeLayout m_relativeLayout_topMenu;

        //
        struct TMP_sel
        {

            public UInt64 ui64Id_from;

            public string msg;
            public Int64 tStartTime;
            public uint uiTranNo;
            public int iTaskId;

        };

        //
        TMP_sel m_curSel;

        //
        public static unsafe int talk_addNewMsg(object hTalkParam, QY_MESSENGER_ID pIdInfo_from, Int64 tSendTime, uint uiTranNo, ushort usCode, uint uiContentType, string msg, int iTaskId)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;

            if (null == hTalkParam) return -1;
            if (null == msg || msg.Length == 0) return -1;

            //
            Activity_dlgTalk hTalk = (Activity_dlgTalk)hTalkParam;

            //
#if DEBUG
            DateTime dt = new DateTime(1970, 1, 1);
            DateTime dt1 = dt.AddSeconds(tSendTime);
            //
            DateTime dt2 = dt1.ToLocalTime();
            //
            int ii = 0;
#endif


            //
            TableItem_dlgTalk_msg item = null;
            item = hTalk.m_var.m_pProcessQ.Find(x => x.ui64Id_from == pIdInfo_from.ui64Id && x.tStartTime == tSendTime && x.uiTranNo == uiTranNo);
            if (item == null)
            {
                int kk = 0;
                item = new TableItem_dlgTalk_msg();
                item.ui64Id_from = pIdInfo_from.ui64Id;
                //
                uint uiObjType = 0;
                string grpName = "";
                UInt64 idInfo_creator = 0;
                string dw = "";
                string bm = "";
                string syr = "";

                qyFuncs.getTalkerDesc(pIdInfo_from, ref uiObjType, ref grpName, ref idInfo_creator, ref dw, ref bm, ref syr);
                item.dw = dw;
                item.bm = bm;
                item.syr = syr;
                //
                item.ImageResourceId = Resource.Drawable.contact;
                //
                //
                item.tStartTime = tSendTime;
                item.uiTranNo = uiTranNo;
                item.usCode = usCode;
                item.uiContentType = uiContentType;
                item.msg = (msg);
                item.iTaskId = iTaskId;
                //item.iStatus = iStatus;
                //
                hTalk.m_var.m_pProcessQ.Add(item);
                //
                //mTableItems.Sort((x, y) => y.tSendTime.CompareTo(x.tSendTime));
                //
                if (hTalk.mAdapter.mFragment_dlgTalk_msgList1 == null) goto errLabel;
                Fragment_dlgTalk_msgList fg = hTalk.mAdapter.mFragment_dlgTalk_msgList1;

                Adapter_dlgTalk_msgList adapter = (Adapter_dlgTalk_msgList)fg.mAdapter;
                if (adapter != null)
                {
                    adapter.NotifyDataSetChanged();
                }

                //
                if (fg.mLayoutManager != null)
                {
                    fg.mLayoutManager.ScrollToPosition(hTalk.m_var.m_pProcessQ.Count - 1);
                }
            }
        errLabel:
            return 0;

        }



        //
        public static unsafe int talk_displayTaskStatus(object hDlgTalkParam, MIS_MSG_taskStatus* pMsgTaskStatus)
        {
            Activity_dlgTalk hTalk = (Activity_dlgTalk)hDlgTalkParam;
            DLG_TALK_var m_var = hTalk.m_var;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            string str;

            //
            if (pMsgTaskStatus->iTaskId == 0) return -1;
            TableItem_dlgTalk_msg item = m_var.m_pProcessQ.Find(x => x.iTaskId == pMsgTaskStatus->iTaskId);
            if (item == null) return -1;

            //
            Fragment_dlgTalk_msgList fg = hTalk.mAdapter.mFragment_dlgTalk_msgList1;

            Adapter_dlgTalk_msgList adapter = (Adapter_dlgTalk_msgList)fg.mAdapter;
            if (adapter == null) return -1;


            //            
            switch (pMsgTaskStatus->iStatus)
            {
                case Consts.CONST_imTaskStatus_sending:
                case Consts.CONST_imTaskStatus_receiving:
                    //
                    str = string.Format("{0}%", pMsgTaskStatus->u_transferFileStatus.ucPercent);
                    item.statusStr = str;


                    //
                    adapter.NotifyDataSetChanged();


                    //
                    break;
                case Consts.CONST_imTaskStatus_sendFinished:
                case Consts.CONST_imTaskStatus_recvFinished:
                case Consts.CONST_imTaskStatus_canceledBySender:
                case Consts.CONST_imTaskStatus_canceledByReceiver:
                case Consts.CONST_imTaskStatus_autoCanceledByReceiver:
                    {

                        //
#if __DEBUG__
					showInfo_open(0, _T(""), _T("tmpHandler_showMsg_displayTaskStatus: finished...."));
#endif
                        //
                        int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, pMsgTaskStatus->iTaskId);
                        QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
                        if (pTaskInfo == null) goto errLabel;
                        QMC_taskData_common taskData = pTaskInfo.getTaskData();
                        if (taskData == null) goto errLabel;
                        fixed (MIS_MSGU* pMsg = &taskData.msgU)
                        {
                            if (pMsg->uiType != Consts.CONST_misMsgType_task) goto errLabel;
                            if (m_var.addr.uiObjType != Consts.CONST_objType_imGrp
                                || pMisCnt.idInfo.ui64Id != pMsg->task.data.route.idInfo_from.ui64Id)
                            {
                                pMsg->task.iStatus = pMsgTaskStatus->iStatus;
                            }
                        }

                        //
                        str = "";// string.Format("{0}", qyFuncs.qyGetDesByType1(qyFuncs.CONST_qyStatusTable_cn, pMsgTaskStatus->iStatus));
                        item.statusStr = str;

                        //
                        adapter.NotifyDataSetChanged();

                    }
                    //  				
                    //activateWnd(hDlg, m_var);
                    break;

                default:

                    //					
                    break;

            }



        errLabel:

            return 0;

        }

        //
        public unsafe int gui_procMsg(ref MIS_MSGU pMsgU)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            bool bDbg = false;
            string str;

            //
            Activity_dlgTalk hTalk = this.mhTalk;
            DLG_TALK_var m_var = hTalk.get_pm_var();

            //
            switch (pMsgU.uiType)
            {
                /*
                case Consts.CONST_misMsgType_talk:
                    {
                        MIS_MSG_TALK* pMsg = &pMsgU->talk;
                        IM_CONTENTU* pContent = (IM_CONTENTU*)pMsg->data.buf;
                        switch (pContent->uiType)
                        {
                            case Consts.CONST_imCommType_htmlContent:
                                {
                                    //mainActivity.mAdapter.myFragment_msg
                                    UInt64 id = pMsg->addr_logicalPeer.idInfo.ui64Id;
                                        //
                                        talk_addNewMsg(this.mhTalk, pMsg->data.route.idInfo_from, pMsg->tStartTime,pMsg->uiTranNo,pMsg->usCode, pContent->uiType, new string(pContent->html.wBuf),pMsg->iTaskId,pMsg->iStatus);
                                    
                                }
                                break;
                        }


                    }
                    break;
                */
                case Consts.CONST_misMsgType_input:
                    {
                        ref MIS_MSG_INPUT pMsg = ref pMsgU.input;
                        ref IM_CONTENTU pContent = ref pMsg.data.buf;
                        switch (pContent.uiType)
                        {

                            case Consts.CONST_imCommType_confLayout:
                                {
                                    //  2010/12/27

                                    //
                                    if (!m_var.av.taskInfo.bTaskExists) goto errLabel;

                                    if (m_var.av.taskInfo.ucbStarter) break;


                                    //  以下为参会人

                                    if (0 != pContent.confLayout.ucbResp)
                                    {
                                        //
                                        ref IM_CONTENTU tmpContent = ref pContent;



                                        if (pContent.confLayout.usOp == Consts.CONST_imOp_phone_respCurr)
                                        {

                                            QY_MESSENGER_ID idInfo_curr;
                                            idInfo_curr.ui64Id = (ulong)pContent.confLayout.confLayoutParam.resp.curr_idinfo;


                                            int ii = 0;


                                        }

                                        if (pContent.confLayout.usOp == Consts.CONST_imOp_respChannel)
                                        {
                                            int ichannel = pContent.confLayout.confLayoutParam.ipcParam.iChannel;

                                            fixed (char* channelName = pContent.confLayout.confLayoutParam.ipcParam.channelName)
                                            {
                                                str = new string(channelName);
                                            }
                                            int ii = 0;
                                        }

                                    }

                                }
                                break;
                            case Consts.CONST_imCommType_statusConfLayoutParam:
                                {

                                    //这里接收 放大缩小的改变变化
                                    int bAmplification = pContent.statusConfLayoutParam.confLayoutParam.respVideoStatus.bAmplification;
                                    qyFuncs.traceLog("通用通道放大缩小变化 1 为放大:" + bAmplification);
                                    int qiuji = pContent.statusConfLayoutParam.confLayoutParam.ipcParam.iChannel;
                                    string str1;
                                    fixed (char* channelName = pContent.statusConfLayoutParam.confLayoutParam.ipcParam.channelName)
                                    {
                                        str1 = new string(channelName);
                                    }
                                    int i = 0;


                                    if (null != hTalk.m_hTool_dlgTalk_av)
                                    {
                                        Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)hTalk.m_hTool_dlgTalk_av;
                                        hTalk_av.m_fg_dlgTalk_av2.changeCurrentVideoStatus(bAmplification);
                                        qyFuncs.traceLog("##球机放大缩小变化通道数从>=1开始,其他为非球机通道画面,当前通道:" + qiuji);
                                        hTalk.m_var.currentSelectQiuJiChannel = qiuji;
                                        Int64 qiuji_ScaleId = pContent.confKey.confLayoutParam.enlargeParam.ui64Id;
                                        //这里拿到那个ID号了哈
                                        qyFuncs.traceLog("##球机qiuji_ScaleId  qiuji" + qiuji_ScaleId + "    " + qiuji);
                                        hTalk.m_var.currentSelectQiuJiIdInfo = qiuji_ScaleId;

                                        hTalk_av.m_fg_dlgTalk_av2.changeNvrControlStatus(qiuji);

                                    }



                                }
                                break;
                            case Consts.CONST_imCommType_confNvrInfo:
                                {
                                    //if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
                                    if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
                                    //
                                    if (m_var.av.taskInfo.ucbStarter) break;


                                    //
                                    if (0 != pContent.confLayout.ucbResp)
                                    { //  发给requester


                                        QY_MESSENGER_ID idInfo_to; idInfo_to = pContent.confNvrInfo.idInfo_requester;
                                        QY_MESSENGER_ID idInfo_requester; idInfo_requester = pContent.confNvrInfo.idInfo_requester;
                                        //

                                        pProcInfo.m_ipcProc.op.nvrInfo = pContent.confNvrInfo.nvrInfo;

                                        qyFuncs.traceLog("pProcInfo.m_ipcProc.op.nvrInfo:" + pProcInfo.m_ipcProc.op.nvrInfo);

                                        //Activity_dlgTalk hTalk = this.mhTalk;
                                        if (null != hTalk.m_hTool_dlgTalk_av)
                                        {
                                            Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)hTalk.m_hTool_dlgTalk_av;
                                            hTalk_av.m_fg_dlgTalk_av2.showNvrList();

                                        }

                                    }
                                }
                                break;
                            case Consts.CONST_imCommType_taskProcReq:
                                if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForOrgReq)
                                {
                                    using (CQyMalloc malloc_pMsgTask_o = new CQyMalloc())
                                    {
                                        ;
                                        int size = sizeof(MIS_MSG_TASK);
                                        //MIS_MSG_TASK* pMsgTask_o = (MIS_MSG_TASK*)malloc_pMsgTask_o.mallocf(size);
                                        CMIS_MSGU pMsgTask_o = new CMIS_MSGU();
                                        if (null == pMsgTask_o) goto errLabel;
                                        //qyFuncs.MACRO_memset_misMsgTask(pMsgTask_o);

                                        if (pMsg.iTaskId == 0) goto errLabel;
                                        int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(mainActivity.var_common.ctxCaller, pProcInfo, pMsg.iTaskId);
                                        if (index_taskInfo < 0) goto errLabel;
                                        QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "fg_dlgTalk_msgList.gui_procMsg");
                                        if (pTaskInfo == null) goto errLabel;
                                        QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                                        if (pTaskData == null) goto errLabel;

                                        //MIS_MSG_TASK* pMsgTask_i = findTaskInProcessQ(&m_var, pMsg->iTaskId);
                                        fixed (MIS_MSG_TASK* pMsgTask_i = &pTaskData.msgU.task)
                                        {
                                            if (null == pMsgTask_i) goto errLabel;
                                            if (pMsgTask_i->uiType != Consts.CONST_misMsgType_task) goto errLabel;
                                            //
                                            IM_CONTENTU* pContentTask_i = (IM_CONTENTU*)&pMsgTask_i->data.buf;
#if DEBUG
                                            ref IM_CONTENTU tmp_pContentTask_i = ref *pContentTask_i;
#endif
                                            if (pContentTask_i->uiType != Consts.CONST_imCommType_transferAvInfo) goto errLabel;
                                            //
                                            if (true)
                                            {   //  2012/05/25
                                                bool bPermitted = false;
#if DEBUG
                                                str = string.Format("recvd applyForOrgReq. to check whether the from is a valid user. {0}", pMsg.data.route.idInfo_from.ui64Id);
                                                qyFuncs.traceLog(str);
#endif
                                                /*
                                                if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
                                                {
                                                    //  要检查是否是组成员
                                                    bool bVal = false;
                                                    if (!getVal_bImGrpMem(&m_var.addr.idInfo, &pMsg->data.route.idInfo_from, &bVal)
                                                        && bVal)
                                                    {
                                                        bPermitted = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (pMsg->data.route.idInfo_from.ui64Id == m_var.addr.idInfo.ui64Id)
                                                    {
                                                        bPermitted = true;
                                                    }
                                                }
                                                */
                                                bPermitted = true;
                                                //
                                                if (!bPermitted)
                                                {
#if DEBUG
                                                    qyFuncs.traceLog(("applyForOrgReq denied. Not a valid user."));
#endif
                                                    goto errLabel;
                                                }
                                            }
                                            //
                                            PARAM_findOrgMsg param=new PARAM_findOrgMsg();
                                            param.pMsgTask_o = pMsgTask_o;
                                            if (pProcInfo.processQ_media.qTraverse(mainActivity.var_common.ctxCaller, qyFuncs.tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsgTask_i, param) <= 0)
                                            {
#if DEBUG
                                                qyFuncs.traceLogA("dlgTalk_OnQyComm: taskProcReq: applyForOrgReq failed.");
#endif
                                                goto errLabel;
                                            }

                                            if (pMsgTask_o.msgU.uiType != Consts.CONST_misMsgType_task)
                                            {
#if DEBUG
                                                str = string.Format("dlgTalk_OnQyComm: taskProcReq: applyForOrgReq failed. msg.type {0} err", pMsgTask_o.msgU.uiType);
                                                qyFuncs.traceLog(str);
#endif
                                                goto errLabel;
                                            }
                                            ref IM_CONTENTU pContentTask_o = ref pMsgTask_o.msgU.task.data.buf;
                                            if (pContentTask_o.uiType != Consts.CONST_imCommType_transferAvInfo) goto errLabel;

#if DEBUG
                                            qyFuncs.traceLogA("dlgTalk_OnQyComm: send transferAvInfo again.");
#endif
                                            //
                                            int lenInBytes = (int)(pMsgTask_o.msgU.task.lenInBytes - (uint)Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf"));
                                            if (lenInBytes < 0) goto errLabel;
                                            //
                                            QY_MESSENGER_ID idInfo_dst;
                                            idInfo_dst.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                            //  set idInfo_logicalPeer temporiary.
                                            if (m_var.addr.idInfo.ui64Id != idInfo_dst.ui64Id)
                                            {
                                                pContentTask_o.transferAvInfo.idInfo_logicalPeer.ui64Id = m_var.addr.idInfo.ui64Id;
                                            }
                                            QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;
                                            fixed (IM_CONTENTU* buf = &pMsgTask_o.msgU.task.data.buf)
                                            {
                                                qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, 0, pMsgTask_o.msgU.task.usCode,
                                                    pMsgTask_o.msgU.task.tStartTime, pMsgTask_o.msgU.task.uiTranNo, 0, pMsgTask_o.msgU.task.iTaskId,
                                                    pMsgTask_o.msgU.task.uiTaskType, (byte*)buf, (uint)lenInBytes,
                                                    idInfo_logicalPeer.ui64Id, pMsgTask_o.msgU.task.idInfo_taskSender.ui64Id, pMsgTask_o.msgU.task.idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, 0, null, false);
                                            }
                                            //pContentTask_o->transferAvInfo.idInfo_logicalPeer.ui64Id  =  0;
                                            //
                                            str = string.Format("dlgTalk: send transferAvInfo again to {0}. tn {1}", idInfo_dst.ui64Id, pMsgTask_o.msgU.task.uiTranNo);
                                            uint step = 0;
                                            qyFuncs.qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);

                                            //
                                            break;
                                        }
                                    }
                                }
                                //  2014/10/09
                                /*
                                if (pContent->taskProcReq.usOp == CONST_imOp_stopSending
                                    || pContent->taskProcReq.usOp == CONST_imOp_requestToSend)
                                {
                                    if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
                                    if (!m_var.av.ucbGot_iIndex_sharedObj_localAv) goto errLabel;
                                    QY_SHARED_OBJ* pSharedObj = getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                                    if (!pSharedObj) goto errLabel;
                                    if (pSharedObj->bSlave) goto errLabel;
                                    SHARE_dyn_bmp* pDynBmpMem = (SHARE_dyn_bmp*)pSharedObj->master.pShareDynBmp_slave;
                                    if (!pDynBmpMem) goto errLabel;

                                    if (pDynBmpMem->resObj.uiObjType != pContent->taskProcReq.viewDynBmp.uiObjType
                                        || pDynBmpMem->resObj.usIndex_obj != pContent->taskProcReq.viewDynBmp.usIndex_obj)
                                    {
                                        goto errLabel;
                                    }

                                    QY_SHARED_OBJ* pSharedObj_slave = getSharedObjByIndex(pProcInfo, pSharedObj->master.slaveKey.iIndex_sharedObj);
                                    if (!pSharedObj_slave) goto errLabel;
                                    if (!pSharedObj_slave->bUsed) goto errLabel;
                                    if (!pSharedObj_slave->bSlave) goto errLabel;
                                    if (pSharedObj_slave->uiTranNo != pSharedObj->master.slaveKey.uiTranNo) goto errLabel;

                                    if (pContent->taskProcReq.usOp == CONST_imOp_stopSending)
                                    {
                                        removeFromMsgrs_sendLocalAv(pProcInfo, m_var.pMisCnt, &pMsg->data.route.idInfo_from, &pSharedObj_slave->curRoute_sendLocalAv);
                                        qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("dlgTalk_OnQyComm: slave: removeFromMsgrs_sendLocalAv, %I64u"), pMsg->data.route.idInfo_from);
                                    }
                                    break;
                                }
                                */
                                break;
                            case Consts.CONST_imCommType_taskProcResp:
                                if (pContent.taskProcResp.usOp_org == Consts.CONST_imOp_recv_applyForChkTaskAlive)
                                {
                                    /*
                                    Q_NODE* pQNode;
                                    MIS_MSGU* pMsg;

                                    if (!m_var.m_pProcessQ) goto errLabel;

                                    for (pQNode = m_var.m_pProcessQ->front.pNext; pQNode; pQNode = pQNode->pNext)
                                    {
                                        pMsg = (MIS_MSGU*)pQNode->qElemMemory.m_pBuf;
                                        if (pMsg->uiType != CONST_misMsgType_task) continue;
                                        if (pMsg->task.tStartTime == pContent->taskProcResp.tStartTime_org
                                            && pMsg->task.uiTranNo == pContent->taskProcResp.uiTranNo_org)
                                        {
                                            if (bTaskAlive(pContent->taskProcResp.iStatus))
                                            {
                                                //
                                                _sntprintf(tBuf, mycountof(tBuf), _T("resp recvd, task %d is alive"), pMsg->task.iTaskId);
                                                showInfo_open(0, 0, tBuf);
#if DEBUG
                                                traceLog(tBuf);
#endif
                                                //
                                                //  mytime(  &pMsg->task.tStartTime_processed  );
                                                mytime(&pMsg->task.tTime_alive);    //  2014/12/09
                                            }
                                            break;
                                        }
                                    }
                                    */
                                }
                                break;

                                break;
                            default:
                                break;
                        }


                    }
                    break;
                default:
                    break;


            }

            PARAM_PROC_ROW ppr = new PARAM_PROC_ROW();

            qyFuncs_Droid.tmpHandler_showMsg(mhTalk, ppr, ref pMsgU);


            iErr = 0;

        errLabel:


            return iErr;

        }




        //
        public unsafe int chkIfOldAvTaskExists()
        {
            int iErr = -1;
            //
            if (mhTalk == null) return -1;
            if (mhTalk.m_var == null) return -1;
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            int i;
            string str;

            for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
            {
                QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, i, "");
                if (pTaskInfo == null) goto errLabel;
                if (!pTaskInfo.bUsed) continue;
                QMC_taskData_common taskData = pTaskInfo.getTaskData();
                if (taskData.uiType != Consts.CONST_taskDataType_conf) continue;
                QMC_taskData_conf pTc = (QMC_taskData_conf)taskData;
                fixed (MIS_MSG_TASK* pMsgTask = &pTc.msgU.task)
                {
                    if (pMsgTask->uiType != Consts.CONST_misMsgType_task) continue;
                    //
                    if (pMsgTask->addr_logicalPeer.idInfo.ui64Id == mhTalk.m_var.addr.idInfo.ui64Id)
                    {
                        IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsgTask->data.buf;
                        if (pContent->uiType != Consts.CONST_imCommType_transferAvInfo) continue;

#if DEBUG
                        ref IM_CONTENTU tmp_pContent = ref *pContent;
#endif

                        QY_MESSENGER_ID idInfo_initiator;
                        idInfo_initiator = pContent->transferAvInfo.confCfg.idInfo_initiator;
                        if (idInfo_initiator.ui64Id == 0) idInfo_initiator.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
                        int tmp_iTaskId = pTaskInfo.var.iTaskId;

                        //
                        QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
                        QY_MESSENGER_ID idInfo = idInfo_initiator;



                        QY_MESSENGER_INFO msgrInfo=new QY_MESSENGER_INFO();
                        QY_MESSENGER_REGINFO regInfo = new QY_MESSENGER_REGINFO(); ;
                        if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en,
                            "", ref idInfo, 0, ref msgrInfo, ref regInfo))
                        {
                            qyFuncs.mymemset((IntPtr)(byte*)&regInfo, 0, sizeof(QY_MESSENGER_REGINFO));
                        }

                        //
                        TableItem_dlgTalk_msg item = new TableItem_dlgTalk_msg() { ui64Id_from = idInfo_initiator.ui64Id, dw = new string(regInfo.dw), bm = new string(regInfo.bm), syr = new string(regInfo.syr), ImageResourceId = Resource.Drawable.contact, msg = "视频呼叫", iTaskId = tmp_iTaskId };
                        //
                        item.ImageResourceId = Resource.Drawable.contact;
                        //
                        item.tStartTime = pMsgTask->tStartTime;
                        item.uiTranNo = pMsgTask->uiTranNo;
                        //
                        mhTalk.m_var.m_pProcessQ.Add(item);

                    }


                }


            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        public unsafe int gui_onTimer(object p0, object p1, object p2)
        {
            Activity_dlgTalk hTalk = this.mhTalk;
            if (hTalk == null) return 0;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
#if  DEBUG
            //qyFuncs.showInfo_open(0,"","fg_dlgTalk_msgList.gui_onTimer called");
#endif  

            //
            if (this.mhTalk.m_hTool_dlgTalk_av == null)
            {
                int iTaskId = 0;
                int iElapseInMs = 0;
                if (hTalk.findOldAvTaskActive(ref iTaskId, ref iElapseInMs) == 0)
                {
                    //
                    qyFuncs.showInfo_open(0, "", "任务活跃中. e: " + iElapseInMs);

                    //
                    this.m_button_avCall.Text = "接收";
                }
                else
                {
                    this.m_button_avCall.Text = "视频通话";
                }


                //
                if (!pProcInfo.av.talk.bChannelReady)
                {
                    m_txt_title.Text = hTalk.m_str_title + "(离线)";
                }
                else
                {
                    m_txt_title.Text = hTalk.m_str_title;

                }



            }


            //
            return 0;
        }



        //
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            mhTalk = (Activity_dlgTalk)this.Activity;

            //
            if (mhTalk.m_var != null)
            {
#if false
                mhTalk.m_var.m_pProcessQ.Add(new TableItem_dlgTalk_msg() { ui64Id_from = 106, dw = "清扬创新", bm = "技术部", syr = "王五", ImageResourceId = Resource.Drawable.Vegetables, msg = "今天快递了吗" });
                mhTalk.m_var.m_pProcessQ.Add(new TableItem_dlgTalk_msg() { ui64Id_from = 105, dw = "微软", bm = "销售部门", syr = "张三", ImageResourceId = Resource.Drawable.Vegetables, msg = "任务完成了吗？我说，今天任务没完成，我的时间都花在看抖音了。这是不对的。要说了很多话怎么办？你吃饭了吗？吃了是什么呢，吃的是鱼还是肉？" });
#endif

                //
                chkIfOldAvTaskExists();

            }

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);


            View view = inflater.Inflate(Resource.Layout.layout_fg_dlgTalk_msgList, container, false);

            //

            //
            m_txt_back = view.FindViewById<TextView>(Resource.Id.txt_back);
            m_txt_title = view.FindViewById<TextView>(Resource.Id.txt_title);
            m_txt_more = view.FindViewById<TextView>(Resource.Id.txt_more);

            m_txt_back.Click += M_txt_back_Click;
            m_txt_more.Click += M_txt_more_Click;


            //
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            //
            mLayoutManager = new LinearLayoutManager(inflater.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);




            mAdapter = new Adapter_dlgTalk_msgList(this.Activity, mhTalk.m_var.m_pProcessQ);

            //
            mAdapter.ItemClick += MAdapter_ItemClick;

            //
            mRecyclerView.SetAdapter(mAdapter);


            //
            m_editText_input = view.FindViewById<EditText>(Resource.Id.editText_input);
            m_button_snd = view.FindViewById<Button>(Resource.Id.button_snd);
            m_button_avCall = view.FindViewById<Button>(Resource.Id.btn_av);
            m_button_file = view.FindViewById<Button>(Resource.Id.btn_file);

            //
            m_button_snd.Click += M_button_snd_Click;
            m_button_avCall.Click += M_button_avCall_Click;
            m_button_file.Click += M_button_file_Click;


            //
            Activity_dlgTalk hTalk = (Activity_dlgTalk)this.Activity;
            m_txt_title.Text = hTalk.m_str_title;

            //
            m_relativeLayout_topMenu = (RelativeLayout)view.FindViewById(Resource.Id.topMenu);


        //
        errLabel:

            return view;
        }

        private void M_button_file_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int ii = 0;
            openSystemFile();
        }

        //
        public void openSystemFile()
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            //intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE,true);//打开多个文件
            //intent.addCategory(Intent.CATEGORY_DEFAULT);
            //intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            intent.AddCategory(Intent.CategoryOpenable);
            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "请选择文件"), 1);
            }
            catch (ActivityNotFoundException e)
            {
                e.PrintStackTrace();
                //Toast.MakeText(this, "请 安装文件管理器", ToastLength.Short);
            }
        }

        string TAG = "";


        public override unsafe void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (data == null) goto errLabel;

            if (requestCode == 1)
            {
                //Get the Uri of the selected file


                if (data.ClipData != null)
                {//有选择多个文件
#if false
                    int count = data.getClipData().getItemCount();
                    Log.i(TAG, "url count ：  " + count);
                    int currentItem = 0;

                    while (currentItem < count)
                    {
                        Uri imageUri = data.getClipData().getItemAt(currentItem).getUri();
                        //Log.i(TAG, "url " + imgpath);

                        //do something with the image (save it to some directory or whatever you need to do with it here)
                        currentItem = currentItem + 1;
                    }
#endif
                }
                else if (data.Data != null)
                {//只有一个文件咯
                    Net.Uri uri = data.Data;
                    string path = null;// uri.Path;
                    //String imagePath = ContentUriUtil.getPath(this,data.getData());
                    //Log.i(TAG, "Single image path ---- " + path);

                    //
                    //
                    try
                    {
                        Com.Qycx.Tmpjavalibrary.FileUtils fu = new Com.Qycx.Tmpjavalibrary.FileUtils(this.mhTalk);
                        path = fu.GetPath(uri);
                        //path = PathUtil.getPath(this, uri);
                        if (path == null) goto errLabel;
                        //
                        int iTaskId;
                        qyFuncs_Droid.procSendFile(path, null, null, mhTalk, mhTalk.m_var, &iTaskId);
                    }
                    catch (Java.Lang.Exception e)
                    {
                        goto errLabel;
                    }
                    int i;
                    i = 9;

                    //do something with the image (save it to some directory or whatever you need to do with it here)
                }

            }
        errLabel:
            return;
        }





        //
        private void M_txt_more_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mhTalk.startActivity_dlgTalk_detail();
        }

        private void M_txt_back_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mhTalk.OnBackPressed();

        }

        private void MAdapter_ItemClick(object sender, Adapter_dlgTalk_msgListClickEventArgs e)
        {
            //throw new NotImplementedException();
            TableItem_dlgTalk_msg item_sel = mhTalk.m_var.m_pProcessQ[e.Position];

            //
            Console.WriteLine("{0} clicked", item_sel.ui64Id_from);

            //
            this.m_curSel = new TMP_sel();
            m_curSel.ui64Id_from = item_sel.ui64Id_from;
            m_curSel.tStartTime = item_sel.tStartTime;
            m_curSel.uiTranNo = item_sel.uiTranNo;
            m_curSel.msg = item_sel.msg;
            m_curSel.iTaskId = item_sel.iTaskId;

            //
            showListDialog();

        }

        //
        public override void OnDestroyView()
        {
            base.OnDestroyView();
        }

        public override void OnDestroy()
        {
            if (this.m_txt_title != null)
            {
                m_txt_title.Dispose();
            }
            if (this.m_editText_input != null)
            {
                this.m_editText_input.Dispose();
            }
            if (this.m_button_snd != null)
            {
                m_button_snd.Dispose();
            }
            if (this.m_button_avCall != null)
            {
                m_button_avCall.Dispose();
            }
            if (mRecyclerView != null)
            {
                mRecyclerView.Dispose();
            }
            if (mLayoutManager != null)
            {
                mLayoutManager.Dispose();
            }
            if (mAdapter != null)
            {
                mAdapter.Dispose();
            }
            if (m_relativeLayout_topMenu != null)
            {
                m_relativeLayout_topMenu.Dispose();
            }

            //
            base.OnDestroy();
            qyFuncs.traceLog("Fragment_dlgTalk_msgList  OnDestroy");
            Dispose();
        }


        private void M_button_avCall_Click(object sender, EventArgs e)
        {
            //
            qyFuncs.showInfo_open(0, "", "M_button_avCall_Click");

            //
            //int ii = mhTalk.m_var.tmp_hTools_dlgTalk_av.GetLength(0);


            //
            if (mhTalk.hasAllPermission())
            {
                startActivityDlgTalk();
            }
            else
            {
                mhTalk.isPermissionOk();
            }
            //throw new NotImplementedException();


        }

        public void startActivityDlgTalk()
        {
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)this.Activity;

            //
            int iTaskId = 0;
            int iElapseInMs = 0;
            if (0 == dlgTalk.findOldAvTaskActive(ref iTaskId, ref iElapseInMs))
            {
                string str;
                str = "任务 " + iTaskId + " 仍在活跃中";
                qyFuncs.traceLog(str);
                //  qyFuncs.showNotification_open(0, 0, 0, str);

                doAvRecover(iTaskId);
                return;
            }


            //
            dlgTalk.doTask_startAvCall();
        }

        private unsafe void M_button_snd_Click(object sender, EventArgs e)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
#if DEBUG
            //TopSnackBar snack = TopSnackBar.Make(m_relativeLayout_topMenu, "测试top snackbar", Com.Qycx.Top_snackbar_library.Top_snackbar.BaseTransientBottomBar.LengthLong);
            //snack.Show();
#endif

            //
            string str = m_editText_input.Text;
            str.Trim();
            if (str.Length == 0) return;
            //
            QY_MESSENGER_ID idInfo_from;
            idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
            //
            Int64 tStartTran; uint uiTranNo;
            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);

            //
            m_editText_input.Text = "";
            m_editText_input.RequestFocus();

            //
            CIM_HTML_CONTENT obj_content = new CIM_HTML_CONTENT();

            //
            //  Æä´ÎÔÙ·¢ÍùmgrÊ¹·¢ËÍ
            byte ucFlg;
            ref IM_HTML_CONTENT content  =  ref obj_content.content;
            int lenInBytes;
            CMIS_MSGU pMsgBuf = null;
            //
            ucFlg = 0;
            //
            //qyFuncs.mymemset((IntPtr)(byte*)&content, 0, sizeof(IM_HTML_CONTENT));
            content.uiType = Consts.CONST_imCommType_htmlContent;
            fixed (IM_HTML_CONTENT* pcontent = &content)
            {
                qyFuncs.mylstrcpyn(pcontent->wBuf, str, Consts.cntof_imHtmlContent_wBuf);
                lenInBytes = (int)Marshal.OffsetOf<IM_HTML_CONTENT>("wBuf") + (qyFuncs.mylstrlen(pcontent->wBuf) + 1) * sizeof(char);

                //
                MSG_ROUTE route;
                qyFuncs.mymemset((IntPtr)(byte*)&route, 0, sizeof(MSG_ROUTE));
                //
                route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;
                //
                route.idInfo_to.ui64Id = mhTalk.m_var.addr.idInfo.ui64Id;
                //route.uiDevType_to = pCurVar->addr.uiDevType;

                //
                QY_MESSENGER_ID talk_idInfo;
                talk_idInfo.ui64Id = mhTalk.m_var.addr.idInfo.ui64Id;


                //
                if (mhTalk.m_var.addr.uiObjType != Consts.CONST_objType_imGrp)
                {

                    if (0 != qyFuncs.postMsg2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, ref route, Consts.CONST_misMsgType_talk, ucFlg, Consts.CONST_qyCmd_talkTo, tStartTran, uiTranNo, 0, (byte*)pcontent, (uint)lenInBytes, talk_idInfo.ui64Id, talk_idInfo.ui64Id, 0, pMsgBuf, true)) goto errLabel;
                }
                else
                {
                    QY_MESSENGER_ID idInfo_dst;
                    //
                    idInfo_dst.ui64Id = mhTalk.m_var.addr.idInfo.ui64Id;
                    //
                    if (idInfo_dst.ui64Id != mhTalk.m_var.addr.idInfo.ui64Id) content.idInfo_logicalPeer.ui64Id = mhTalk.m_var.addr.idInfo.ui64Id;
                    //

                    if (0 != qyFuncs.postMsg2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, ref route, Consts.CONST_misMsgType_talk, ucFlg, Consts.CONST_qyCmd_talkTo, tStartTran, uiTranNo, 0, (byte*)pcontent, (uint)lenInBytes, talk_idInfo.ui64Id, idInfo_dst.ui64Id, 0, pMsgBuf, true)) goto errLabel;


                }
                //
                talk_addNewMsg(this.mhTalk, idInfo_from, tStartTran, uiTranNo, Consts.CONST_qyCmd_talkTo, content.uiType, str, 0);
            }

        errLabel:
            return;



        }

        public bool OnTouch(View? v, MotionEvent? e)
        {
            //throw new NotImplementedException();
            // MotionEvent object holds X-Y values
            if (e.Action == MotionEventActions.Down)
            {
#if false
                string text = "You click at x = " + e.GetX() + " and y = " + e.GetY();
                Toast.MakeText(Activity, text, ToastLength.Long).Show();
#endif
            }


            //
            return false;
        }

        //
        const int iCmd_copy = 1;
        const int iCmd_av_alive = 10;
        const int iCmd_av_recover = 11;
        const int iCmd_task_gone = 12;
        const int iCmd_file_accept = 20;
        const int iCmd_file_cancel = 21;


        //
        QY_DMITEM[] menu_conf_alive = new QY_DMITEM[2]
        {
            new QY_DMITEM(iCmd_av_alive,"会议正进行"),
            new QY_DMITEM(-1,""),
        };



        QY_DMITEM[] menu_conf_bClosed = new QY_DMITEM[2]
        {
            new QY_DMITEM(iCmd_av_recover,"重新连接"),
            new QY_DMITEM(-1,""),
        };

        //
        QY_DMITEM[] menu_file_wait = new QY_DMITEM[3]
        {
            new QY_DMITEM(iCmd_file_accept,"接收"),
            new QY_DMITEM(iCmd_file_cancel,"取消"),
            new QY_DMITEM(-1,""),
        };

        //
        QY_DMITEM[] menu_file_alive = new QY_DMITEM[2]
        {
            new QY_DMITEM(iCmd_file_cancel,"取消"),
            new QY_DMITEM(-1,""),
        };




        //
        QY_DMITEM[] menu_task_gone = new QY_DMITEM[2]
        {
            new QY_DMITEM(iCmd_task_gone,"任务已终止"),
            new QY_DMITEM(-1,""),
        };


        //
        QY_DMITEM[] menu_msg = new QY_DMITEM[]
        {
            new QY_DMITEM(iCmd_copy,"复制"),
            new QY_DMITEM(-1,""),
        };

        //
        int getCnt(QY_DMITEM[] items)
        {
            if (items == null) return 0;
            int i;
            int cnt = 0;
            for (i = 0; i < items.Length; i++)
            {
                if (items[i].type == -1) break;
                cnt++;
            }
            //
            return cnt;
        }

        //
        string[] getMenuList(QY_DMITEM[] menu)

        {
            if (menu == null) return null;

            //
            string[] items = null;
            int n = getCnt(menu);
            if (n == 0) return null;
            items = new string[n];
            int i;
            for (i = 0; i < n; i++)
            {
                items[i] = new string(menu[i].des);
            }
            return items;
        }


        //
        unsafe QY_DMITEM[] getTaskMenu(int iTaskId)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QY_DMITEM[] menu = null;

            if (iTaskId == 0) return null;

            //
            int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common pTaskData = (pTaskInfo.getTaskData());
            if (pTaskData == null) goto errLabel;
            fixed (MIS_MSGU* pMsgU = &pTaskData.msgU)
            {
                MIS_MSG_TASK* pMsgTask = &pMsgU->task;
                if (pMsgTask->uiType != Consts.CONST_misMsgType_task) goto errLabel;
                //
#if DEBUG
                ref MIS_MSG_TASK tmp_pMsgTask = ref *pMsgTask;
#endif
                //
                if (!pTaskInfo.var.bClosed && qyFuncs.bTaskAlive(pMsgTask->iStatus))
                {
                    IM_CONTENTU* pContent = qyFuncs.M_getMsgContent(pMsgTask->uiType,pMsgTask->ucFlg, &pMsgTask->data);
                    if (pContent == null) goto errLabel;

                    switch (pContent->uiType)
                    {
                        case Consts.CONST_imCommType_transferAvInfo:
                            menu = menu_conf_alive;
                            break;
                        case Consts.CONST_imCommType_transferFileReq:
                            {
                                if (qyFuncs.bTaskNeedAcception(pMsgTask->iStatus))
                                {
                                    menu = menu_file_wait;
                                }
                                else
                                {
                                    menu = menu_file_alive;
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    uint dwTickCnt = qyFuncs.myGetTickCount(null);
                    int iDiffInMs = (int)(dwTickCnt - pTaskInfo.var.dwTickCnt_recv_lastRefreshed1);
                    if (iDiffInMs < 500)
                    {
                        menu = menu_conf_bClosed;
                    }
                    else
                    {
                        menu = menu_task_gone;
                    }

                }

            }

            iErr = 0;
        errLabel:

            if (menu == null)
            {
                menu = menu_task_gone;
            }
            return menu;

        }


        //

        QY_DMITEM[] m_curMenu = null;

        //
        unsafe void showListDialog()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            string[] items = null;// new string[]{ "我是1","我是2","我是3"};
            //
            m_curMenu = null;
            if (m_curSel.iTaskId == 0)
            {
                m_curMenu = menu_msg;

            }
            else
            {
                m_curMenu = getTaskMenu(m_curSel.iTaskId);
            }
            items = getMenuList(m_curMenu);
            if (items == null) goto errLabel;




            //
            AlertDialog.Builder listDialog = new AlertDialog.Builder(Activity);
            listDialog.SetIcon(Resource.Drawable.icon);//图标
            listDialog.SetTitle("菜单");
            listDialog.SetItems(items, ListClicked);

            listDialog.Show();

        //
        errLabel:
            return;

        }

        //
        int doAvRecover(int iTaskId)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;

            //
            pTaskInfo.var.bClosed = false;

            //
            qyFuncs.showInfo_open(0, "", "doAvRecover: bNeedRemoved_toRecover set to true");

            //
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) goto errLabel;

            ref MIS_MSGU tmp_pMsg = ref pTaskData.msgU;
            if (tmp_pMsg.uiType != Consts.CONST_misMsgType_task) goto errLabel;
            tmp_pMsg.task.iStatus = Consts.CONST_imTaskStatus_resp;
            qyFuncs.traceLog("##1347 " + tmp_pMsg.task.iStatus);


            //
            DLG_TALK_var m_var = this.mhTalk.m_var;


            //
            m_var.av.bNeedDoTaskAv = true;

            //
            //mainActivity.var_common.processingAvTask
            if (this.mhTalk.hasAllPermission())
            {
                this.mhTalk.startActivity_dlgTalk_av(tmpResource.IDC_BUTTON_av_accept, index_taskInfo, "doAvRecover");

            }
            else
            {
                this.mhTalk.isPermissionOk();
            }


            iErr = 0;
        errLabel:

            return iErr;
        }

        //
        private void ListClicked(object sender, DialogClickEventArgs e)
        {
            int iErr = -1;
            int ii = 0;

            //
            Activity_dlgTalk hTalk = (Activity_dlgTalk)this.Activity;

            //
#if DEBUG
            string str = string.Format("ListClicked {0}", e.Which);
            qyFuncs.traceLog(str);
#endif
            if (m_curMenu == null) goto errLabel;
            if (e.Which >= m_curMenu.Length) goto errLabel;

            if (m_curSel.iTaskId == 0)
            {
                switch (m_curMenu[e.Which].type)
                {
                    case iCmd_copy:
                        qyFuncs_Droid.copyToClipboard(m_curSel.msg);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (m_curMenu[e.Which].type)
                {
                    case iCmd_av_recover:
                        doAvRecover(m_curSel.iTaskId);
                        break;
                    case iCmd_file_accept:
                        {
                            ii = 0;
                            PARAM_dlgTalk_procTask param = new PARAM_dlgTalk_procTask();
                            qyFuncs_Droid.dlgTalk_procTask1(hTalk, m_curSel.iTaskId, Consts.CONST_imOp_recv_accept, "", ref param);
                        }
                        break;
                    default:
                        break;
                }

            }

            //
            iErr = 0;
        errLabel:

            return;
        }

    }
}