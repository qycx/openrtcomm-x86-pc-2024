using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {
        public static unsafe int msg2TaskRcd(MIS_CNT pMisCnt, ref MIS_MSG_INPUT pMsg1, ushort usRespCode, ref IM_TASK_RCD pRcd1)
        {
            int iErr = -1;
            //MIS_MSG_routeTalkData* pTalkData = null;
            //IM_CONTENTU* pContent = null;
            //char buf[255 + 1] = "";
            string str;

            

            if (null == pMisCnt ) return -1;


            fixed (IM_TASK_RCD* pRcd = &pRcd1)
            {
                fixed (MIS_MSG_INPUT* pMsg = &pMsg1)
                {

                    MIS_MSG_routeTalkData  * pTalkData = &pMsg->data;
                    IM_CONTENTU *pContent = &pTalkData->buf;

                    mymemset((IntPtr)pRcd, 0, sizeof(IM_TASK_RCD));
                    //
                    //mylstrcpyn(pRcd->misServName, pMisCnt.cfg.misServName, mycountof(pRcd->misServName));
                    pRcd->idInfo_send.ui64Id = pMsg->idInfo_taskSender.ui64Id;
                    pRcd->tSendTime = pMsg->tStartTime;
                    pRcd->uiTranNo = pMsg->uiTranNo;
                    pRcd->idInfo_recv.ui64Id = pMsg->idInfo_taskReceiver.ui64Id;
                    //
                    pRcd->uiContentType = pContent->uiType; //  2008/05/29
                                                            //
                    mytime(ref pRcd->tRecvTime);
                    pRcd->usRespCode = usRespCode;
                    //
                    pRcd->id = pMsg->iTaskId;                   //  2010/04/23
                    pRcd->uiMode = 0;
                    pRcd->uiType = pMsg->uiTaskType;
                    pRcd->iStatus = pMsg->iStatus;

                    switch (pContent->uiType)
                    {
                        case Consts.CONST_imCommType_transferFileReq:
                            //
                            mylstrcpyn(pRcd->subject, pContent->transferFileReq.fileName, Consts.cntof_subject);
                            mylstrcpyn(pRcd->content, pContent->transferFileReq.fullFilePath, Consts.cntof_content);
                            //
                            //_sntprintf(pRcd->propStrs[0], mycountof(pRcd->propStrs[0]), _T("%I64u"), pContent->transferFileReq.ui64FileLen);
                            str = string.Format("{0}", pContent->transferFileReq.ui64FileLen);
                            mytcsncpy(pRcd->propStr0, str, Consts.cntof_propStr);
                            break;
                        case Consts.CONST_imCommType_rtcCallReq:
                            //
                            //ulIp2Str(pContent->rtcCallReq.ulIp_caller, buf, mycountof(buf));
                            //myStr2TChar(buf, pRcd->subject, mycountof(pRcd->subject));
                            //
                            break;
                        case Consts.CONST_imCommType_transferAvInfo:
                            //
                            //_sntprintf(pRcd->subject, mycountof(pRcd->subject), _T("%d * %d"), pContent->transferAvInfo.tranInfo.video.vh_decompress.bih.biWidth, pContent->transferAvInfo.tranInfo.video.vh_decompress.bih.biHeight);
                            str = string.Format("{0}*{1}", pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth, pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight);
                            mytcsncpy(pRcd->subject, str, Consts.cntof_subject);
                            //
                            break;
                        case Consts.CONST_imCommType_queryCustomerServiceOfficerReq:       //  2011/04/06
                            break;
                        case Consts.CONST_imCommType_transferGpsInfo:                      //  2012/04/19
                            break;
                        default:
#if __DEBUG__
					traceLog(_T("msg2TaskRcd failed, unknown contentType"));
#endif
                            goto errLabel;
                            break;
                    }
                }
            }

            iErr = 0;
        errLabel:

            return iErr;
        }

        public static unsafe int msg2TaskRcd(MIS_CNT pMisCnt, ref MIS_MSG_TASK pMsg1, ushort usRespCode, ref IM_TASK_RCD pRcd1)
        {
            int iErr = -1;
            MIS_MSG_routeTalkData* pTalkData = null;
            IM_CONTENTU* pContent = null;
            //char buf[255 + 1] = "";
            string str;

            if (null == pMisCnt ) return -1;

            fixed (IM_TASK_RCD* pRcd = &pRcd1)
            {

                fixed (MIS_MSG_TASK* pMsg = &pMsg1)
                {


                    pTalkData = &pMsg->data;
                    pContent = (IM_CONTENTU*)&pTalkData->buf;

                    if (null == pTalkData || null == pContent) goto errLabel;

                    mymemset((IntPtr)pRcd, 0, sizeof(IM_TASK_RCD));
                    //
                    //mylstrcpyn(pRcd->misServName, pMisCnt.cfg.misServName, mycountof(pRcd->misServName));
                    pRcd->idInfo_send.ui64Id = pMsg->idInfo_taskSender.ui64Id;
                    pRcd->tSendTime = pMsg->tStartTime;
                    pRcd->uiTranNo = pMsg->uiTranNo;
                    pRcd->idInfo_recv.ui64Id = pMsg->idInfo_taskReceiver.ui64Id;
                    //
                    pRcd->uiContentType = pContent->uiType; //  2008/05/29
                                                            //
                    mytime(ref pRcd->tRecvTime);
                    pRcd->usRespCode = usRespCode;
                    //
                    pRcd->id = pMsg->iTaskId;                   //  2010/04/23
                    pRcd->uiMode = 0;
                    pRcd->uiType = pMsg->uiTaskType;
                    pRcd->iStatus = pMsg->iStatus;
                    qyFuncs.traceLog("##117  " + pMsg->iStatus);

                    switch (pContent->uiType)
                    {
                        case Consts.CONST_imCommType_transferFileReq:
                            //
                            mylstrcpyn(pRcd->subject, pContent->transferFileReq.fileName, Consts.cntof_subject);
                            mylstrcpyn(pRcd->content, pContent->transferFileReq.fullFilePath, Consts.cntof_content);
                            //
                            //_sntprintf(pRcd->propStrs[0], mycountof(pRcd->propStrs[0]), _T("%I64u"), pContent->transferFileReq.ui64FileLen);
                            str = string.Format("{0}", pContent->transferFileReq.ui64FileLen);
                            mytcsncpy(pRcd->propStr0, str, Consts.cntof_propStr);
                            break;
                        case Consts.CONST_imCommType_rtcCallReq:
                            //
                            //ulIp2Str(pContent->rtcCallReq.ulIp_caller, buf, mycountof(buf));
                            //myStr2TChar(buf, pRcd->subject, mycountof(pRcd->subject));
                            //
                            break;
                        case Consts.CONST_imCommType_transferAvInfo:
                            //
                            //_sntprintf(pRcd->subject, mycountof(pRcd->subject), _T("%d * %d"), pContent->transferAvInfo.tranInfo.video.vh_decompress.bih.biWidth, pContent->transferAvInfo.tranInfo.video.vh_decompress.bih.biHeight);
                            str = string.Format("{0}*{1}", pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth, pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight);
                            mytcsncpy(pRcd->subject, str, Consts.cntof_subject);
                            //
                            break;
                        case Consts.CONST_imCommType_queryCustomerServiceOfficerReq:       //  2011/04/06
                            break;
                        case Consts.CONST_imCommType_transferGpsInfo:                      //  2012/04/19
                            break;
                        default:
#if __DEBUG__
					traceLog(_T("msg2TaskRcd failed, unknown contentType"));
#endif
                            goto errLabel;
                            break;
                    }
                }
            }

            iErr = 0;
        errLabel:

            return iErr;
        }

    }
}
