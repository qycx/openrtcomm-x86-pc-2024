using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
 
    partial class qyFuncs
{

        public static QY_DMITEM[] CONST_imOpTable_en = new QY_DMITEM[19]
{
    new QY_DMITEM  (   Consts.CONST_imOp_says,                                        (  "says"  )                         ),
 
 new QY_DMITEM(Consts.   CONST_imOp_tryToSendFile,                               (  "tryToSendFile"  )                ),
 //
 new QY_DMITEM(Consts.   CONST_imOp_recv_accept,                                 (  "recv_accept"  )                  ),
 new QY_DMITEM(Consts.   CONST_imOp_recv_deny,                                   (  "recv_deny"  )                    ),
 new QY_DMITEM(Consts.   CONST_imOp_recvFile_saveas,                             (  "recvFile_saveas"  )              ),
 //
 new QY_DMITEM(Consts.   CONST_imOp_recv_cancel,                                 (  "recv_cancel"  )                  ),
 new QY_DMITEM(Consts.   CONST_imOp_send_cancel,                                 (  "send_cancel"  )                  ),
 new QY_DMITEM(Consts.   CONST_imOp_send_permit,                                 (  "send_permit"  )                  ),
 //
 new QY_DMITEM(Consts.   CONST_imOp_dual_permit,                                 (  "dual_permit"  )                  ),
 //
 new QY_DMITEM(Consts.   CONST_imOp_recv_applyForOrgReq,                         (  "recv_applyForOrgReq"  )          ),
 new QY_DMITEM(Consts.   CONST_imOp_recv_applyForChkTaskAlive,                   (  "recv_applyForChkTaskAlive"  )    ),		//  2009/10/19
 new QY_DMITEM(Consts.CONST_imOp_recv_applyForKeyFrame,                         ("recv_applyForKeyFrame")               ),
 //
 //  2009/02/21
 new QY_DMITEM(Consts.   CONST_imOp_requestToSpeak,                              (  "request to speak"  )             ),
 new QY_DMITEM(Consts.   CONST_imOp_stopSpeaking,                                (  "stop speaking"  )                ),

 //  2014/10/09
 new QY_DMITEM(Consts.   CONST_imOp_requestToSend,                               (  "request to send"  )              ),		//  2014/10/09
 new QY_DMITEM(Consts.   CONST_imOp_stopSending,                                 (  "stop sending"  )                 ),		//  2014/10/09

 //

 //
 new QY_DMITEM(Consts.   CONST_imOp_openFile,                                    (  "openFile"  )                     ),
 //
 new QY_DMITEM(Consts.   CONST_imOp_myMax,                                       (  "imOp_myMax"  )                   ),
 //
 new QY_DMITEM(-1,                                                                ""                             ),
 
};


        public static unsafe bool tmpHandler_bTask( CTX_caller ctxCaller,ref MIS_MSGU p0,PARAM_findOrgMsg p1, object p2)
        {
            bool bRet = false;
            int iTaskId = (int)p1.iTaskId;
            //p1
            TASK_common taskMem = (TASK_common)p2;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo, taskMem.m_index_taskInfo,"tmpHandler_bTask");
            if (pTaskInfo == null) return false;
            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
            if (pTaskData == null) return false;

            if ( pTaskData.msgU.uiType==Consts.CONST_misMsgType_task
                && pTaskData.msgU.task.iTaskId==iTaskId)
            {
                bRet = true;
            }

            return bRet;
        }

        public static unsafe int removeTask(CTX_caller ctxCaller, int iTaskId)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            CMIS_MSGU p0_unused = new CMIS_MSGU();
            PARAM_findOrgMsg param = new PARAM_findOrgMsg();
            param.iTaskId = iTaskId;
            //
            //pProcInfo.processQ_media.qRemoveMsg(ctxCaller, tmpHandler_bTask, (object)iTaskId, null);
            pProcInfo.processQ_media.qRemoveMsg(ctxCaller, tmpHandler_bTask, ref p0_unused.msgU, param);


            return 0;

        }

        public static unsafe void clearShiftKeys()
        {
            traceLog(_T("clearShiftKeys,  test_touchscreen"));
            return;
        }

        //
        public static bool bSupported_fileServer(CCtxQyMc pQyMc)
        {
            return false;
        }


        //
        public static unsafe int getStatusByTaskId(int iTaskId)
        {
            int iErr = -1;
            int iStatus = 0;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            if (iTaskId == 0) return -1;
            int index_taskInfo = getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
            QMC_TASK_INFO pTaskInfo = getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common taskData = pTaskInfo.getTaskData();
            if (taskData == null) goto errLabel;
            fixed (MIS_MSGU* pMsg = &taskData.msgU)
            {
                switch (pMsg->uiType)
                {
                    case Consts.CONST_misMsgType_task:
                        iStatus = pMsg->task.iStatus;
                        break;
                    default:
                        goto errLabel;
                }

            }


            iErr = 0;
        errLabel:
            return iErr  !=  0  ?  iErr  :  iStatus;
        }

    }
}
