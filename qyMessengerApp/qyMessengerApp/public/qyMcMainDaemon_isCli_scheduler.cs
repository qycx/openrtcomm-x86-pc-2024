using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace qy
{
    partial class qyFuncs
    {
        public static bool bRefreshAllContacts(MIS_CNT pMisCnt)
        {
            return 0 == pMisCnt.refreshContactList.tLastRefreshedTime_misServ;
        }

        public static unsafe bool bMeInfoNeedReg(object pDb, int iDbType, MIS_CNT pMisCnt, QY_MESSENGER_REGINFO* pRegInfo)
        {
            return false;

        }


        static unsafe int mcProcResp_getCfgs(ref MT_SUBTHREADINFO pSubThreadInfoParam, MIS_CNT pMisCnt, MIS_MSG_INPUT* pMsgInput, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;
            CCtxQmc pProcInfo = pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;
            IM_CONTENTU* pContent = null;

            //TCHAR tBuf[255 + 1] = ("");

            if (isUcFlgRouteTalkData(pMsgInput->ucFlg)) goto errLabel;
            if (isUcFlgTalkData(pMsgInput->ucFlg)) goto errLabel;
            pContent = (IM_CONTENTU*)&pMsgInput->data;

            if (null == pContent) goto errLabel;

            switch (pContent->uiType)
            {
                //  case  CONST_imCommType_intervalParams:
                case Consts.CONST_imCommType_getCfgsReq:       //  2011/01/30
                    if (pContent->getCfgsReq.uiSizePerSnd_media != 0
                        && pContent->getCfgsReq.uiSizePerSnd_media != pMisCnt.cfg.uiSizePerSnd_media)
                    {
                        pMisCnt.cfg.uiSizePerSnd_media = Math.Min(pProcInfo.cfg.uiSizePerSnd_media, pContent->getCfgsReq.uiSizePerSnd_media);
                    }

                    pMisCnt.cfg.usIntervalInS_refreshRecentFriends = Math.Max((ushort)2, pContent->getCfgsReq.intervalParams.usIntervalInS_refreshRecentFriends);
                    pMisCnt.cfg.usIntervalInS_refreshContactList = Math.Max((ushort)2, pContent->getCfgsReq.intervalParams.usIntervalInS_refreshContactList);

                    //  2015/07/28

                    //
                    pMisCnt.bIntervalParamsGot = true;
                    break;
                /*
            case Consts.CONST_imCommType_retrieveToPaths:
                //
                if (pContent->retrieveToPaths.usCnt > mycountof(pContent->retrieveToPaths.mems)) goto errLabel;
                memcpy(&pMisCnt->toPaths, &pContent->retrieveToPaths, sizeof(pMisCnt->toPaths)  );
                //
                pMisCnt->bToPathsRetrieved = TRUE;
                //
                break;
                */
                default:
                    {
                        string str = string.Format("mcProcResp_getCfgs err: unprocessed uiContentType ( {0} )", pContent->uiType);
                        traceLog(str);
                    }
                    goto errLabel;
                    break;
            }

            iErr = 0;

        errLabel:

            return iErr;

        }





        public static unsafe int mcProcResp_refreshImGrps(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, RETRIEVE_IMGRPLIST_RESP* pResp, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;

            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;
            if (null == pQyMc) return -1;

            int i;

            //TCHAR sqlBuf[1024];
            //  CDBVariant							varVals[50];
            COMMON_PARAM commonParam0;
            COMMON_PARAM commonParam1;
            TRAN_INFO_MIS tranInfo;

            //byte idStr_lastRefreshed[Consts.CONST_qyMessengerIdStrLen + 1] = "";
            int tmpiRet;

            uint uiObjType = Consts.CONST_objType_imGrp;

            if (pResp->idInfo_lastRefreshed.ui64Id != 0)
            {
                //if (!idInfo2Str(&pResp->idInfo_lastRefreshed, idStr_lastRefreshed, mycountof(idStr_lastRefreshed))) goto errLabel;
            }

            //  if  (  !bRefreshAllContacts(  pMisCnt  )  )  
            {  //  ÕâÀïÊÇÔöÁ¿Ë¢ÐÂ
               //民粹派ro
                for (i = 0; i < pResp->usCnt; i++)
                {
                    IM_GRP_INFO* pMems = (IM_GRP_INFO*)pResp->buf_imGrpInfo_mems;
                    int bufSize = Consts.bufSize_imGrpInfo_mems;
                    IM_GRP_INFO* pMem = getMem(pMems, bufSize, i);

                    if (0 != pProcInfo.recoverImGrp(pQyMc.p_g_dbFuncs, pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, ref CONST_fieldIdTable_en, uiObjType, pMem, pMisCnt.refreshContactList.tStartTime, false))
                    {
                        traceLogA("recoverImGrp(  ) failed");
                        goto errLabel;
                    }

                    //
                    postImObjToIntQ(pMem->idInfo);
                }

                //
                triggerToDisplayImObjIntQ();

            }


            iErr = 0;

        errLabel:

            return iErr;

        }



        //  2013/01/30
        //int  mcProcResp_refreshImGrps_end(  void  *  pSubThreadInfoParam,  QY_SERVICEGUI_INFO  *  pSci,  MIS_CNT  *  pMisCnt,  RETRIEVE_IMGRPLIST_RESP  *  pResp,  CTX_MC_THREAD  *  pCtx  )
        public static unsafe int mcProcResp_refreshImGrps_end(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, RETRIEVE_IMGRPLIST_RESP* pResp, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;
            CCtxQmc pProcInfo = pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;

            //TCHAR sqlBuf[1024];

            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            if (bRefreshAllContacts(pMisCnt))
            {  //  ±íÃ÷ÊÇÈ«ÃæË¢ÐÂ,ÒªÉ¾³ý´ÓidInfo_lastRefreshedºóµÄËùÓÐµÄ·Ï×é

                //char idStr_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";

                if (0 != pResp->idInfo_lastRefreshed.ui64Id)
                {
                    //if (!idInfo2Str(&pResp->idInfo_lastRefreshed, idStr_lastRefreshed, mycountof(idStr_lastRefreshed))) goto errLabel;
                }

                if (0 != g_dbFuncs.pf_updateImObj_refreshImGrps_end(pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, "", 0, pMisCnt.refreshContactList.tStartTime)) goto errLabel;

            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        //  2013/02/01
        //  int  mcProcResp_refreshImGrpMems_odbc(  void  *  pSubThreadInfoParam,  void  *  pSciParam,  void  *  pMisCntParam,  RETRIEVE_IMGRPMEMLIST_RESP  *  pResp,  void  *  pCtxParam  )
        public static unsafe int mcProcResp_refreshImGrpMems(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, RETRIEVE_IMGRPMEMLIST_RESP* pResp, ref CTX_MC_thread pCtxParam)
        {
            int iErr = -1;

            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;


            //TCHAR sqlBuf[1024];
            //  CDBVariant							varVals[50];
            COMMON_PARAM commonParam0;
            COMMON_PARAM commonParam1;
            TRAN_INFO_MIS tranInfo;

            //byte` idStr_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";
            //byte idStr_grp_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";
            int tmpiRet;
            int i;


            //if (!idInfo2Str(&pResp->idInfo_grp_lastRefreshed, idStr_grp_lastRefreshed, mycountof(idStr_grp_lastRefreshed))) goto errLabel;
            //if (!idInfo2Str(&pResp->idInfo_lastRefreshed, idStr_lastRefreshed, mycountof(idStr_lastRefreshed))) goto errLabel;

            //  if  (  !bRefreshAllContacts(  pMisCnt  )  )  
            {  //  ÕâÀïÊÇÔöÁ¿Ë¢ÐÂ
               //
                for (i = 0; i < pResp->usCnt; i++)
                {
                    IM_GRP_MEM* pMems = (IM_GRP_MEM*)pResp->buf_imGrpMem_mems;
                    int bufSize = Consts.bufSize_imGrpMem_mems;
                    IM_GRP_MEM* pMem = getMem(pMems, bufSize, i);

                    if (0 != pProcInfo.recoverImGrpMem(pQyMc.p_g_dbFuncs, pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, pMem, pMisCnt.refreshContactList.tStartTime))
                    {
                        traceLogA("recoverImGrpMem(  ) failed");
                        goto errLabel;
                    }
                }
                //
            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        // 2013/02/01
        //  int  mcProcResp_refreshImGrpMems_end_odbc(  void  *  pSubThreadInfoParam,  void  *  pSciParam,  void  *  pMisCntParam,  RETRIEVE_IMGRPMEMLIST_RESP  *  pResp,  void  *  pCtxParam  )
        public static unsafe int mcProcResp_refreshImGrpMems_end(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, RETRIEVE_IMGRPMEMLIST_RESP* pResp, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;

            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;

            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            //  TCHAR								sqlBuf[1024];

            if (bRefreshAllContacts(pMisCnt))
            {  //  ±íÃ÷ÊÇÈ«ÃæË¢ÐÂ,ÒªÉ¾³ý´ÓidInfo_lastRefreshedºóµÄËùÓÐµÄ·Ï×é
                /*
				char idStr_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";
				char idStr_grp_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";

				if (pResp->usCnt)
				{
					idInfo2Str(&pResp->mems[pResp->usCnt - 1].idInfo_grp, idStr_grp_lastRefreshed, mycountof(idStr_grp_lastRefreshed));
					idInfo2Str(&pResp->mems[pResp->usCnt - 1].idInfo_mem, idStr_lastRefreshed, mycountof(idStr_lastRefreshed));
				}
				else
				{   //  2011/08/05
					idInfo2Str(&pResp->idInfo_grp_lastRefreshed, idStr_grp_lastRefreshed, mycountof(idStr_grp_lastRefreshed));
					idInfo2Str(&pResp->idInfo_lastRefreshed, idStr_lastRefreshed, mycountof(idStr_lastRefreshed));
				}
				*/

                if (0 != g_dbFuncs.pf_updateImGrpMem_refreshImGrpMems_end(pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, "", pMisCnt.refreshContactList.tStartTime)) goto errLabel;

            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        // 	
        public static unsafe int mcProcResp_refreshContacts(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, RETRIEVE_CONTACTLIST_RESP* pResp, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;

            bool bDbg = false;
            string str;

#if DEBUG
            //bDbg = true;
#endif

            //
            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;

            //
            QM_dbFuncs p_g_dbFuncs = pQyMc.p_g_dbFuncs;

            int i;


            //TCHAR sqlBuf[1024];
            //  CDBVariant							varVals[50];
            COMMON_PARAM commonParam0;
            COMMON_PARAM commonParam1;
            TRAN_INFO_MIS tranInfo;

            //byte idStr_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";
            int tmpiRet;

            if (0 != pResp->idInfo_lastRefreshed.ui64Id)
            {
                //if (!idInfo2Str(&pResp->idInfo_lastRefreshed, idStr_lastRefreshed, mycountof(idStr_lastRefreshed))) goto errLabel;
            }

            //  if  (  !bRefreshAllContacts(  pMisCnt  )  )  
            {   //  
                //  QY_MESSENGER_REGINFO	regInfo;
                //
                for (i = 0; i < pResp->usCnt; i++)
                {

                    //
                    RETRIEVE_contact_mem* pMems = (RETRIEVE_contact_mem*)pResp->buf_retrieveContactMem_mems;
                    int bufSize = Consts.bufSize_retrieveContaceMem_mems;
                    RETRIEVE_contact_mem* pMem = getMem(pMems, bufSize, i);// &pMems[i];

                    //
#if DEBUG
                    ref RETRIEVE_contact_mem tmp_pMem = ref *pMem;
                    //
                    if (bDbg)
                    {
                        fixed (char* pdw = tmp_pMem.regInfo.dw, pbm = tmp_pMem.regInfo.bm, syr = tmp_pMem.regInfo.syr)
                        {
                            str = string.Format("contact {0}:{1} {2} {3}", tmp_pMem.regInfo.addr.idInfo.ui64Id, new string(pdw), new string(pbm), new string(syr));
                        }
                        traceLog(str);
                    }
#endif

                    //
                    QY_MESSENGER_INFO messengerInfo;
                    //
                    //lstrcpyn(messengerInfo.misServName, pMem->regInfo.addr.misServName, mycountof(messengerInfo.misServName));
                    messengerInfo.idInfo.ui64Id = pMem->regInfo.addr.idInfo.ui64Id;
                    messengerInfo.uiType = pMem->regInfo.addr.uiObjType;        //  CONST_objType_messenger;		 
                    messengerInfo.iStatus = 0;
                    //
                    messengerInfo.iRole = pMem->iRole;
                    //
                    if (pMem->regInfo.addr.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                    {
                        messengerInfo.uiType = Consts.CONST_objType_me;
                    }
                    //
                    if (0 != pProcInfo.recoverMessenger(p_g_dbFuncs, pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, ref CONST_fieldIdTable_en, &messengerInfo, &pMem->regInfo, pMisCnt.refreshContactList.tStartTime, false, pProcInfo.schedulerQ))
                    {
                        traceLogA("recoverMessenger(  ) failed");
                        goto errLabel;
                    }

                    if (pMem->regInfo.addr.idInfo.ui64Id == pMisCnt.idInfo.ui64Id)
                    {
                        updateMeDisplayInfo(pProcInfo, pMisCnt, &pMem->regInfo, null, null);
                    }

                    //
                    postImObjToIntQ(pMem->regInfo.addr.idInfo);

                    //

                }

                //
                triggerToDisplayImObjIntQ();
            }


            iErr = 0;
        errLabel:
            return iErr;

        }

        //  2013/02/02. 要注意,要清理的时候要把me排除在外.因为me的更新在startchannel里也有.
        public static unsafe int mcProcResp_refreshContacts_end(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, RETRIEVE_CONTACTLIST_RESP* pResp, ref CTX_MC_thread pCtxParam)
        {
            int iErr = -1;
            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;

            //TCHAR sqlBuf[1024];
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;
            //  
            if (bRefreshAllContacts(pMisCnt))
            {   //  Õâ±íÃ÷ÊÇÈ«ÃæË¢ÐÂ£¬·ÇÔöÁ¿Ë¢ÐÂ£¬ËùÒÔ£¬ÒªÉ¾³ý´ÓidInfo_lastRefreshedºóµÄËùÓÐµÄ·Ïid
                /*
				char idStr_lastRefreshed[CONST_qyMessengerIdStrLen + 1] = "";

				if (pResp->usCnt)
				{
					idInfo2Str(&pResp->mems[pResp->usCnt - 1].regInfo.addr.idInfo, idStr_lastRefreshed, mycountof(idStr_lastRefreshed));
				}
				else
				{   //  2011/08/05
					idInfo2Str(&pResp->idInfo_lastRefreshed, idStr_lastRefreshed, mycountof(idStr_lastRefreshed));
				}
				*/

                //  if  (  !idStr_lastRefreshed[0]  )  ÔÚ·Ö×éÄ£Ê½µ«ÊÇÎ´¼ÓÈëÄ³¸ö×éÊ±£¬»á³öÏÖÕâÇé¿ö

                if (0 != g_dbFuncs.pf_updateImObj_refreshContacts_end(pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, "", pMisCnt.refreshContactList.tStartTime)) goto errLabel;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        static unsafe int mcProcResp_retrieveImObjListReq(ref MT_SUBTHREADINFO pSubThreadInfo, MIS_CNT pMisCnt, MIS_MSG_INPUT* pMsgInput, ref CTX_MC_thread pCtx)
        {
            int iErr = -1;
            CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;
            //  
            IM_CONTENTU* pContent = null;

            uint size;
            //  int								tmpiRet;
            int i;
            //TCHAR tBuf[255 + 1] = ("");

            RETRIEVE_IMOBJLIST_REQ req;
            //  CQyMalloc							mallocObj;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            if (sizeof(IM_L_STREAM) > sizeof(MIS_MSGU)) return -1;
            fixed (MIS_MSGU* pMsgBuf_forInput = &pCtx.pMsgBuf_forInput.msgU)
            {
                IM_L_STREAM* pStream = (IM_L_STREAM*)pMsgBuf_forInput;    //  mallocObj.malloc(  sizeof(  IM_STREAM_CONTENT  )  );
                if (null == pStream) return -1;
                int len;

                if (isUcFlgRouteTalkData(pMsgInput->ucFlg)) return -1;
                pContent = M_getMsgContent(pMsgInput->uiType, pMsgInput->ucFlg, &pMsgInput->data);
                if (null == pContent) return -1;

                //  2014/02/11
                pMisCnt.refreshContactList.dwLastTickCnt_respRecvd = myGetTickCount(null);

                //
                if (pContent->uiType == Consts.CONST_imCommType_retrieveImGrpListResp)
                {

                    RETRIEVE_IMGRPLIST_RESP* pResp = &pContent->retrieveImGrpListResp;
                    uint uiObjType = Consts.CONST_objType_imGrp;
                    IM_GRP_INFO* pMems = (IM_GRP_INFO*)pResp->buf_imGrpInfo_mems;

                    /*

                    memset(  &resp,  0,  sizeof(  resp  )  );
                    if  (  stream2Data(  &ptr,  &size,  tmpHandler_stream2RetrieveImGrpListResp,  0,  &resp  )  )  goto  errLabel;
                    */

                    if (pResp->usCnt > Consts.CONST_maxContactUpdateInfos_grp) goto errLabel;

                    //  2014/02/06
#if DEBUG
                    printRetrieveImGrpListResp(pResp);
#endif


                    //
                    for (i = 0; i < pResp->usCnt; i++)
                    {   //  Ìî³äÐÅÏ¢
                        //lstrcpyn(pResp->mems[i].misServName, pMisCnt->cfg.misServName, mycountof(pResp->mems[i].misServName));
                    }

                    if (pResp->usCnt != 0)
                    {

                        string str;
                        str = string.Format("{0} groups retrieved, tn {1}", pResp->usCnt, pMsgInput->uiTranNo);
                        uint step = 0;
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);

                        //	 
                        size = (uint)Marshal.OffsetOf<RETRIEVE_IMGRPLIST_RESP>("pInternal");    //  
                        mymemset((IntPtr)(((byte*)pResp) + size), 0, (int)(sizeof(RETRIEVE_IMGRPLIST_RESP) - size));

                        if (0 != mcProcResp_refreshImGrps(ref pSubThreadInfo, pMisCnt, pResp, ref pCtx)) goto errLabel;

                        //  2011/04/10
                        pProcInfo.status.retrieveImObjList.ulbImGrpExists = true;

                    }

                    if (0 == pResp->ucbEnd)
                    {

                        if (0 == pResp->usCnt) goto errLabel;

                        //  发送请求以获取剩余的分组信息
                        //
                        mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                        req.uiType = Consts.CONST_imCommType_retrieveImGrpListReq;
                        req.ucbNext = 1;
                        req.idInfo_lastRefreshed.ui64Id = pMems[pResp->usCnt - 1].idInfo.ui64Id;
                        //
                        MACRO_memset_imLStream(pStream);
                        pStream->uiType = Consts.CONST_imCommType_lStream;
                        pStream->uiContentType = req.uiType;
                        len = Consts.bufSize_imLStream;   // sizeof(pStream->buf  );
                        if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint*)&len)) goto errLabel;
                        pStream->ulStreamLen = (uint)len;
                        len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
                        //
                        MSG_ROUTE msgRoute1 = new MSG_ROUTE();
                        if (0 != pProcInfo.postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute1, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, Consts.CONST_channelType_talking, pCtx.pMsgBuf, false)) goto errLabel;

                        iErr = 0; goto errLabel;

                    }

                    //  ±íÃ÷½áÊøÁË£¬Ó¦¸Ã°Ñ´Ó>idInfo_lastRefreshedµÄ¶¼É¾³ý
                    //
                    if (0 != mcProcResp_refreshImGrps_end(ref pSubThreadInfo, pMisCnt, pResp, ref pCtx)) goto errLabel;
                    {
                        string str = string.Format("    retrieveGrps end,  {0}", pMsgInput->uiTranNo);
                        uint step = 0;
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);
                    }
                    //  ÕâÀïÒªÈ¡Ò»ÏÂ·Ö×é,ÒªÍùÉÏ·¢·Ö×éµÄ³ÉÔ±ÇëÇó
                    bool bNeedRetrieveImGrpMems = false;
                    //
                    bNeedRetrieveImGrpMems = g_dbFuncs.pf_bNeedRetrieveImGrpMems(pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, "");
                    if (!bNeedRetrieveImGrpMems)
                    {       //  ÕâÀïÊÇÎÞ×éµÄÇé¿ö£¬Ö±½Ó·¢retrieveContactListReq
                        mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                        req.uiType = Consts.CONST_imCommType_retrieveContactListReq;
                        req.tLastRefreshedTime_contactList_misServ = pMisCnt.refreshContactList.tLastRefreshedTime_misServ;
                        req.usMaxContactUpdateInfosPerSnd = pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_contact;         //  2014/02/06
                    }
                    else
                    {                       //  ÓÐ×éµÄÇé¿ö£¬·¢ËÍretrieveImGrpMemListReq
                        mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                        req.uiType = Consts.CONST_imCommType_retrieveImGrpMemListReq;
                        req.tLastRefreshedTime_contactList_misServ = pMisCnt.refreshContactList.tLastRefreshedTime_misServ;
                        req.usMaxContactUpdateInfosPerSnd = pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_grpMem;          //  2014/02/06
                    }
                    //			 
                    MACRO_memset_imLStream(pStream);
                    pStream->uiType = Consts.CONST_imCommType_lStream;
                    pStream->uiContentType = req.uiType;
                    len = Consts.bufSize_imLStream;// sizeof(pStream->buf  );
                    if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint*)&len)) goto errLabel;
                    pStream->ulStreamLen = (uint)len;
                    len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
                    //
                    MSG_ROUTE msgRoute = new MSG_ROUTE();
                    if (0 != pProcInfo.postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, Consts.CONST_channelType_talking, pCtx.pMsgBuf, false)) goto errLabel;

                    iErr = 0; goto errLabel;

                }

                if (pContent->uiType == Consts.CONST_imCommType_retrieveImGrpMemListResp)
                {

                    RETRIEVE_IMGRPMEMLIST_RESP* pResp = &pContent->retrieveImGrpMemListResp;
                    IM_GRP_MEM* pMems = (IM_GRP_MEM*)pResp->buf_imGrpMem_mems;
                    //  char								idStr_grp_lastRefreshed[CONST_qyMessengerIdStrLen  +  1]				=	"";

                    if (pResp->usCnt > Consts.CONST_maxContactUpdateInfos_grpMem) goto errLabel;

#if DEBUG
                    printRetrieveImGrpMemListResp(pResp);
#endif

                    for (i = 0; i < pResp->usCnt; i++)
                    {   //  Ìî³äÐÅÏ¢
                        //lstrcpyn(pResp->mems[i].misServName, pMisCnt->cfg.misServName, mycountof(pResp->mems[i].misServName));
                    }

                    if (0 != pResp->usCnt)
                    {

                        {
                            string str = string.Format("{0} grpMems retrieved, tn {1}", pResp->usCnt, pMsgInput->uiTranNo);
                            uint step = 0;
                            qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);
                        }
                        //
                        if (0 == pResp->usCnt)
                        {
                            traceLogA("no grpMem, resp error"); goto errLabel;
                        }

                        size = (uint)Marshal.OffsetOf<RETRIEVE_IMGRPMEMLIST_RESP>("pInternal"); //  
                        mymemset((IntPtr)(((byte*)pResp) + size), 0, (int)(sizeof(RETRIEVE_IMGRPMEMLIST_RESP) - size));

                        if (0 != mcProcResp_refreshImGrpMems(ref pSubThreadInfo, pMisCnt, pResp, ref pCtx)) goto errLabel;

                    }

                    if (0 == pResp->ucbEnd)
                    {

                        if (0 == pResp->usCnt) goto errLabel;

                        //  发送请求以获取剩余的分组成员信息
                        //
                        mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                        req.uiType = Consts.CONST_imCommType_retrieveImGrpMemListReq;
                        req.ucbNext = 1;
                        req.idInfo_grp_lastRefreshed.ui64Id = pMems[pResp->usCnt - 1].idInfo_grp.ui64Id;
                        req.idInfo_lastRefreshed.ui64Id = pMems[pResp->usCnt - 1].idInfo_mem.ui64Id;
                        //
                        MACRO_memset_imLStream(pStream);
                        pStream->uiType = Consts.CONST_imCommType_lStream;
                        pStream->uiContentType = req.uiType;
                        len = Consts.bufSize_imLStream;// sizeof(pStream->buf  );
                        if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint*)&len)) goto errLabel;
                        pStream->ulStreamLen = (uint)len;
                        len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
                        //
                        MSG_ROUTE msgRoute1 = new MSG_ROUTE();
                        if (0 != pProcInfo.postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute1, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, Consts.CONST_channelType_talking, pCtx.pMsgBuf, false)) goto errLabel;

                        iErr = 0; goto errLabel;

                    }

                    //
                    if (0 != mcProcResp_refreshImGrpMems_end(ref pSubThreadInfo, pMisCnt, pResp, ref pCtx)) goto errLabel;

                    {
                        string str = string.Format("    retrieveGrpMems end,  {0}", pMsgInput->uiTranNo);
                        uint step = 0;
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);


                    }

                    mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                    req.uiType = Consts.CONST_imCommType_retrieveContactListReq;
                    req.tLastRefreshedTime_contactList_misServ = pMisCnt.refreshContactList.tLastRefreshedTime_misServ;
                    req.usMaxContactUpdateInfosPerSnd = pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_contact;     //  2014/02/06
                                                                                                               //			 
                    MACRO_memset_imLStream(pStream);
                    pStream->uiType = Consts.CONST_imCommType_lStream;
                    pStream->uiContentType = req.uiType;
                    len = Consts.bufSize_imLStream; // sizeof(pStream->buf  );
                    if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint*)&len)) goto errLabel;
                    pStream->ulStreamLen = (uint)len;
                    len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
                    //
                    MSG_ROUTE msgRoute = new MSG_ROUTE();
                    if (0 != pProcInfo.postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, Consts.CONST_channelType_talking, pCtx.pMsgBuf, false)) goto errLabel;

                    iErr = 0; goto errLabel;

                }

                if (pContent->uiType == Consts.CONST_imCommType_retrieveContactListResp)
                {

                    RETRIEVE_CONTACTLIST_RESP* pResp = (RETRIEVE_CONTACTLIST_RESP*)&pContent->retrieveContactListResp;
                    RETRIEVE_contact_mem* pMems = (RETRIEVE_contact_mem*)pResp->buf_retrieveContactMem_mems;

                    if (pResp->usCnt > Consts.CONST_maxContactUpdateInfos_contact) goto errLabel;

                    //  2014/02/06
#if DEBUG
                    printRetrieveContactListResp(pResp);
#endif

                    //
                    for (i = 0; i < pResp->usCnt; i++)
                    {
                        //lstrcpyn(pResp->mems[i].regInfo.addr.misServName, pMisCnt->cfg.misServName, mycountof(pResp->mems[i].regInfo.addr.misServName));
                        //MACRO_setPtrs(getResTable(0, &pQyMc->cusRes, CONST_resId_fieldIdTable), pResp->mems[i].regInfo, pResp->mems[i].regInfo.fields);
                    }

                    if (0 != pResp->usCnt)
                    {

                        {
                            string str = string.Format("{0} contacts retrieved, tn {1}", pResp->usCnt, pMsgInput->uiTranNo);
                            uint step = 0;
                            qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);
                        }
                        //
                        if (0 == pResp->usCnt)
                        {
                            traceLogA("no contact, resp error"); goto errLabel;
                        }

                        size = (uint)Marshal.OffsetOf<RETRIEVE_CONTACTLIST_RESP>("pInternal");  //  
                        mymemset((IntPtr)(((byte*)pResp) + size), 0, (int)(sizeof(RETRIEVE_CONTACTLIST_RESP) - size));

                        if (0 != mcProcResp_refreshContacts(ref pSubThreadInfo, pMisCnt, pResp, ref pCtx)) goto errLabel;

                        //  2011/04/10
                        pProcInfo.status.retrieveImObjList.ulbContactExists = true;
                    }

                    if (0 == pResp->ucbEnd)
                    {

                        if (0 == pResp->usCnt) goto errLabel;

                        //  发送请求以获取剩余的contact
                        //
                        mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                        req.uiType = Consts.CONST_imCommType_retrieveContactListReq;
                        req.ucbNext = 1;
                        req.idInfo_lastRefreshed.ui64Id = pMems[pResp->usCnt - 1].regInfo.addr.idInfo.ui64Id;
                        //
                        MACRO_memset_imLStream(pStream);
                        pStream->uiType = Consts.CONST_imCommType_lStream;
                        pStream->uiContentType = req.uiType;
                        len = Consts.bufSize_imLStream; // sizeof(pStream->buf  );
                        if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint*)&len)) goto errLabel;
                        pStream->ulStreamLen = (uint)len;
                        len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
                        //
                        MSG_ROUTE msgRoute1 = new MSG_ROUTE();
                        if (0 != pProcInfo.postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute1, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, Consts.CONST_channelType_talking, pCtx.pMsgBuf, false)) goto errLabel;

                        iErr = 0; goto errLabel;

                    }

                    //  
                    if (0 != mcProcResp_refreshContacts_end(ref pSubThreadInfo, pMisCnt, pResp, ref pCtx)) goto errLabel;

                    //  RETRIEVE_IMOBJLISTEND_REQ, tLastRefreshedTime_contactList_misServ
                    //  pMisCnt->refreshContactList.tLastRefreshedTime_misServ  =  (  time_t  )resp.tLastRefreshedTime_contactList_misServ;
                    mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                    req.uiType = Consts.CONST_imCommType_retrieveImObjListEndReq;
                    //
                    MACRO_memset_imLStream(pStream);
                    pStream->uiType = Consts.CONST_imCommType_lStream;
                    pStream->uiContentType = req.uiType;
                    len = Consts.bufSize_imLStream; // sizeof(pStream->buf  );
                    if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint*)&len)) goto errLabel;
                    pStream->ulStreamLen = (uint)len;
                    len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");               //
                                                                                    //
                    MSG_ROUTE msgRoute = new MSG_ROUTE();
                    if (0 != pProcInfo.postMsg2Mgr_mc(pCtx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, pMsgInput->tStartTime, pMsgInput->uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, Consts.CONST_channelType_talking, pCtx.pMsgBuf, false)) goto errLabel;

                    iErr = 0; goto errLabel;

                }

                if (pContent->uiType == Consts.CONST_imCommType_retrieveImObjListEndResp)
                {

                    RETRIEVE_IMOBJLISTEND_RESP* pResp = &pContent->retrieveImObjListEndResp;

                    //  2014/02/10
                    bool bRetrieveAll = bRefreshAllContacts(pMisCnt);

                    //  tLastRefreshedTime_misServ, 
                    pMisCnt.refreshContactList.tLastRefreshedTime_misServ = (Int64)pResp->tLastRefreshedTime_contactList_misServ;

                    {
                        string str = bRetrieveAll ? ("All im objects retrieved. ") : ("ImObjList retrieved");
                        uint step = 0;
                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);
                    }

#if DEBUG
                    traceLogA("retrieveImObjListEndResp");
                    //printQmObjQ((QM_OBJQ*)pMisCnt->pObjQ);
#endif

                    //
                    pProcInfo.status.retrieveImObjList.ulbAllContactsRetrieved = true;
                    /*
                    if (bSupported_devAuth())
                    {
                        PostMessage(pQyMc->gui.hMainWnd, WM_COMMAND, MAKEWPARAM(ID_refreshLayout, 0), 0);
                    }

                    //  2013/08/01
                    PostMessage(pQyMc->gui.hMainWnd, WM_COMMAND, MAKEWPARAM(ID_qyRefresh, 0), 0);
                    */
                    //
                    if (bRetrieveAll)
                    {
                        //
                        //GuiShare.pf_fillList_imObj();
                        //
                        GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_allContactsRetrieved, 0);
                    }
                    //
                    iErr = 0; goto errLabel;

                }

                {
                    string str = string.Format("mcProcResp_retrieveImObjListReq: unprocessed uiContentType ( {0} )", pContent->lStream.uiContentType);
                    traceLog(str);
                }
                goto errLabel;

            }

            iErr = 0;

        errLabel:

            return iErr;
        }


        public static unsafe int procMsgInput_scheduler_req(ref MT_SUBTHREADINFO pSubThreadInfo, CCtxQmc pProcInfo, MIS_CNT pMisCnt, MIS_MSG_INPUT* pMsgInput, uint lenInBytes_input, CTX_MC_thread pCtx)
        {
            int iErr = -1;
            /*
			IM_CONTENTU* pContent = NULL;
			TCHAR sqlBuf[CONST_maxSqlBufLen + 1] = ("");
			char timeBuf[CONST_qyTimeLen + 1] = "";
			//
			if (!pProcInfo) return -1;
			CCtxQyMc pQyMc = pProcInfo.pQyMc;
			QM_dbFuncs* pDbFuncs = pQyMc->p_g_dbFuncs;
			if (!pDbFuncs) return -1;
			QM_dbFuncs & g_dbFuncs = *pDbFuncs;

			if (!pMisCnt) return -1;

# ifdef  __DEBUG__
			traceLogA("procMsgInput_scheduler_req called");
#endif

			if (isUcFlgResp(pMsgInput->ucFlg))
			{
# ifdef  __DEBUG__
				traceLogA("procMsgInput_scheduler_req failed, not req");
#endif
				goto errLabel;
			}
			if (!pMsgInput->ucbConvrted)
			{
# ifdef  __DEBUG__
				traceLogA("procMsgInput_scheduler_req: not converted");
#endif
				goto errLabel;
			}
			pContent = M_getMsgContent(pMsgInput->ucFlg, &pMsgInput->data);


			switch (pContent->uiType)
			{

				//  2015/08/01
				case CONST_imCommType_procOfflineRes:
					if (mcProcReq_procOfflineRes(pSubThreadInfo, pSci, pMisCnt, pMsgInput, (CTX_MC_scheduler*)pCtx)) goto errLabel;
					break;
				//
				default:
# ifdef  __DEBUG__
					traceLogA("procMsgInput_scheduler_req: unprocessed contentType, %s", pContent->uiType);
#endif
					break;

			}
			*/

            iErr = 0;
        errLabel:

            if (0 != iErr)
            {
            }


            return iErr;
        }

        //
        public static unsafe int procMsgInput_scheduler_resp(ref MT_SUBTHREADINFO pSubThreadInfo, CCtxQmc pProcInfo, MIS_CNT pMisCnt, MIS_MSG_INPUT* pMsgInput, uint lenInBytes_input, CTX_MC_thread pCtx)
        {
            int iErr = -1;

            IM_CONTENTU* pContent = null;
            //TCHAR sqlBuf[CONST_maxSqlBufLen + 1] = ("");
            //char timeBuf[CONST_qyTimeLen + 1] = "";
            //
            if (null == pProcInfo) return -1;
            CCtxQyMc pQyMc = pProcInfo.pQyMc;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) return -1;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            if (null == pMisCnt) return -1;

#if DEBUG
            traceLogA("procMsgInput_scheduler_resp called");
#endif

            if (!isUcFlgResp(pMsgInput->ucFlg))
            {
                traceLogA("procMsgInput_scheduler_resp failed, not resp");
                goto errLabel;
            }
            if (!pMsgInput->ucbConvrted)
            {
                traceLogA("procMsgInput_scheduler_resp: not converted");
                goto errLabel;
            }
            if (isUcFlgRouteTalkData(pMsgInput->ucFlg) || isUcFlgTalkData(pMsgInput->ucFlg))
            {
                traceLogA("procMsgInput_scheduler_resp: not addrTalkData or talkData");
                goto errLabel;
            }
            pContent = (IM_CONTENTU*)&pMsgInput->data.buf;



            switch (pContent->uiType)
            {
                /*

				case Consts.CONST_imCommType_simpleResp:
					switch (pContent->simpleResp.uiContentType_org)
					{
						case Consts.CONST_imCommType_messengerRegInfo:
							{
								traceLogA("messengerRegInfo");

#if  0
								 char						idStr[Consts.CONST_qyMessengerIdStrLen  +  1]	=	"";	
								 if  (  !idInfo2Str(  &pMisCnt->idInfo,  idStr,  mycountof(  idStr  )  )  )  goto  errLabel;
#endif

# ifndef  __NOTSUPPORT_DB__
#if  0
								 getCurTime(  timeBuf  );
								 _sntprintf(  sqlBuf,  mycountof(  sqlBuf  ),  (  "update %s set tCommitTime='%s' where misServName=%s and idStr='%s'"  ),  Consts.CONST_tabName_qyImObjRegInfoTab1,  CString(  timeBuf  ),  getDbValStr(  pSci->cfg.iDbType,  Consts.CONST_iDataType_misServName,  pMisCnt->cfg.misServName  ),  CString(  idStr  )  );
								 if  (  !bQyExecSql(  pSubThreadInfo->pDb,  sqlBuf  )  )  {
									 traceLogA(  ""  );  goto  errLabel;
								 }
#endif
								if (!g_dbFuncs.pf_bUpdateImObjRegInfo_procMsgInput_scheduler(pSubThreadInfo->pDb, pSci, pMisCnt, &pMisCnt->idInfo)) goto errLabel;
#endif
								//
							}
							break;
						case Consts.CONST_imCommType_messengerPcInfo:
							traceLogA("procMsgInput_scheduler: resp, messengerPcInfo");
							//
							pMisCnt->ucbNeedSndDevInfo = FALSE;
							break;
						case Consts.CONST_imCommType_messengerAccount:
							traceLogA("procMsgInput_scheduler: resp, messengerAccount");
							//
							//  pProcInfo.bInProgress_toSetPasswd  =  FALSE;
							notifyProgressEnd(pMsgInput->uiTranNo, 0);
							break;
						default:
							break;

					}
					//
					break;
				*/
                case Consts.CONST_imCommType_simpleResp:
                    switch (pContent->simpleResp.uiContentType_org)
                    {
                        case Consts.CONST_imCommType_messengerAccount:
                            traceLogA("procMsgInput_scheduler: resp, messengerAccount");

                            //notifyProgressEnd(pMsgInput->uiTranNo, 0);
                            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_showHint, 0);
                            //
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_retrieveImGrpListResp:
                case Consts.CONST_imCommType_retrieveImGrpMemListResp:
                case Consts.CONST_imCommType_retrieveContactListResp:
                case Consts.CONST_imCommType_retrieveImObjListEndResp:
                    if (0 != mcProcResp_retrieveImObjListReq(ref pSubThreadInfo, pMisCnt, pMsgInput, ref pCtx)) goto errLabel;
                    break;

                //  case  Consts.CONST_imCommType_intervalParams:
                case Consts.CONST_imCommType_getCfgsReq:
                case Consts.CONST_imCommType_retrieveToPaths:
                    if (0 != mcProcResp_getCfgs(ref pSubThreadInfo, pMisCnt, pMsgInput, ref pCtx)) goto errLabel;
                    break;
                /*
            case Consts.CONST_imCommType_retrievePhoneMsgrs:
                if (mcProcResp_retrievePhoneMsgrs(pSubThreadInfo, pSci, pMisCnt, pMsgInput, pCtx)) goto errLabel;
                break;
            case Consts.CONST_imCommType_retrieveDynBmps:
                if (mcProcResp_retrieveDynBmps(pSubThreadInfo, pSci, pMisCnt, pMsgInput, (CTX_MC_scheduler*)pCtx)) goto errLabel;
                break;
            case Consts.CONST_imCommType_refreshImObjRulesReq:
                if (mcProcResp_refreshImObjRulesReq(pSubThreadInfo, pSci, pMisCnt, pMsgInput, (CTX_MC_scheduler*)pCtx)) goto errLabel;
                break;
            //  2015/08/01
            case Consts.CONST_imCommType_procOfflineRes:
                if (mcProcResp_procOfflineRes(pSubThreadInfo, pSci, pMisCnt, pMsgInput, (CTX_MC_scheduler*)pCtx)) goto errLabel;
                break;
            */

                //
                default:
                    {
                        string str;
                        str = string.Format("procMsgInput_scheduler_resp: unprocessed contentType, {0}", pContent->uiType);
                        traceLog(str);
                    }
                    break;

            }

            iErr = 0;
        errLabel:

            if (0 != iErr)
            {
            }


            return iErr;
        }

        public static unsafe int testSendMsg(CCtxQyMc pQyMc)
        {
            int loopCtrl = 0;
            CCtxQmc pProcInfo = (CCtxQmc)pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
            MIS_CHANNEL pChannel = null;

            return 0;       //  2007/05/12


            pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_talking);
            if (pChannel == null) return -1;

            for (; loopCtrl < 3000;)
            {

                if (pProcInfo.mgrQ.uiQNodes > 90)
                {
                    traceLogA("¶ÔÁÐÖÐÊý¾ÝÌ«¶àÁË£¬ÒªµÄµÈÒ»»áÔÙ¼Ó");
                    Thread.Sleep(1000); continue;
                }

                /*
				if  (  pChannel.status.nMsgSent  >  150  +  pChannel.status.nMsgRecved  )  {
					traceLogA(  "Ì«¿ìÀ­£¬ÒªµÈËüÀ´µÃ¼°ÊÕ°¡"  );
					Sleep(  2000  );  continue;
				}
				*/

                {

                    string str;
                    str = string.Format("hehe {0} times", loopCtrl);
                    TMP_buf4096 buf4096;
                    myWChar2Utf8(str, buf4096.buf, 4096);


                    loopCtrl++;

                    //  ÔÚÕâÀï,¿ÉÒÔ³é¿Õ×öÐ©ÐèÒªÖÜÆÚÐÔ×öµÄÊÂÇé, ±ÈÈçÒªÇó·þÎñÆ÷¸üÐÂÓÃ»§ÐÅÏ¢
                    traceLog("testSendMsg: ", str);

                    //  test,  ÕâÀïÓ¦¸ÃÏÈÅÐ¶ÏÊÇ·ñ
                    QY_MESSENGER_ID dst;
                    dst.ui64Id = 105;
                    uint uiTranNo;
                    int tt = 0;
                    uiTranNo = getuiNextTranNo(null, 0, ref tt);
                    if (uiTranNo != -1)
                    {
                        //postMsg2Mgr_mc(  pMisCnt,  null,  Consts.CONST_misMsgType_req,  0,  Consts.CONST_qyCmd_talkTo,  0,  uiTranNo,  0,  buf4096.buf,  (uint)mystrlen(  buf4096.buf  ),  null,  &dst,  0, null, false  );  //  test
                    }

                    Thread.Sleep(1000);


                }

            }


            //  errLabel:
            return 0;

        }

        //


        //
        public static unsafe int procImMsgLogQ_scheduler(ref MT_SUBTHREADINFO pSubThreadInfo, CCtxQmc pProcInfo, MIS_MSGU* pMsg)
        {
            int iErr = -1;
#if true
            CCtxQyMc pQyMc = g.g_pQyMc;// (QY_MC*)pProcInfo->pQyMc;
            int i;
            uint len;

            IM_MSG_RCD rcd;
            IM_CONTENTU* pContent;

            for (i = 0; i < 3; i++)
            {
                len = (uint)sizeof(MIS_MSGU);
                if (0 != qGetMsg(pProcInfo.imMsgLogQ, (IntPtr)pMsg, &len)) break;
                mymemset((IntPtr)(((byte*)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));
                //
                switch (pMsg->uiType)
                {
                    case Consts.CONST_misMsgType_talk:
                        pContent = (IM_CONTENTU*)&pMsg->talk.data.buf;

                        if (pContent->uiType == Consts.CONST_imCommType_htmlContent)
                        {
                            mymemset((IntPtr)(byte*)&rcd, 0, sizeof(IM_MSG_RCD));
                            rcd.idInfo_send.ui64Id = pMsg->talk.data.route.idInfo_from.ui64Id;
                            rcd.tSendTime = pMsg->talk.tStartTime;
                            rcd.uiTranNo = pMsg->talk.uiTranNo;
                            //  uiSeqNo
                            //
                            rcd.idInfo_logicalPeer = pMsg->talk.addr_logicalPeer.idInfo;
                            //
                            rcd.idInfo_recv.ui64Id = pMsg->talk.data.route.idInfo_to.ui64Id;
                            //
                            rcd.uiType = pContent->uiType;
                            mylstrcpyn(rcd.logMsgContent, pContent->html.wBuf, Consts.cntof_logMsgContent);
                            rcd.tRecvTime = pMsg->talk.tRecvTime;
                            mytime(ref rcd.firstTime);
                            mytime(ref rcd.lastTime);
                            mytime(ref rcd.tLastModifiedTime);

                            logImMsg(ref pQyMc.p_g_dbFuncs, pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, CONST_fieldIdTable_en, ref rcd);

                        }

                        break;
                    case Consts.CONST_misMsgType_task:
                        pContent = (IM_CONTENTU*)&pMsg->task.data.buf;

                        if (true)
                        {
                            mymemset((IntPtr)(byte*)&rcd, 0, sizeof(IM_MSG_RCD));
                            rcd.idInfo_send.ui64Id = pMsg->task.data.route.idInfo_from.ui64Id;
                            rcd.tSendTime = pMsg->task.tStartTime;
                            rcd.uiTranNo = pMsg->task.uiTranNo;
                            //  uiSeqNo
                            //
                            rcd.idInfo_logicalPeer = pMsg->task.addr_logicalPeer.idInfo;
                            rcd.iTaskId = pMsg->task.iTaskId;
                            //
                            rcd.idInfo_recv.ui64Id = pMsg->task.data.route.idInfo_to.ui64Id;
                            //
                            rcd.uiType = pContent->uiType;
                            //  lstrcpyn(  rcd.content,  pContent->html.wBuf,  mycountof(  rcd.content  )  );
                            switch (rcd.uiType)
                            {
                                case Consts.CONST_imCommType_transferFileReq:
                                    mylstrcpyn(rcd.logMsgContent, pContent->transferFileReq.fileName, Consts.cntof_logMsgContent);
                                    break;
                                case Consts.CONST_imCommType_transferAvInfo:
                                    if (0 != pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth && 0 != pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight)
                                    {
                                        //_sntprintf(rcd.content, mycountof(rcd.content), _T("%d * %d"), pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth, pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            rcd.tRecvTime = pMsg->task.tRecvTime;
                            mytime(ref rcd.firstTime);
                            mytime(ref rcd.lastTime);
                            mytime(ref rcd.tLastModifiedTime);

                            logImMsg(ref pQyMc.p_g_dbFuncs, pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, CONST_fieldIdTable_en, ref rcd);
                        }
                        break;
                    //
                    case Consts.CONST_misMsgType_input:
                        if (!isUcFlgTalkData(pMsg->input.ucFlg) && !isUcFlgRouteTalkData(pMsg->input.ucFlg)) break;
                        //
                        pContent = (IM_CONTENTU*)M_getMsgContent(pMsg->input.uiType,pMsg->input.ucFlg, &pMsg->input.data);


                        if (true)
                        {
                            mymemset((IntPtr)(byte*)&rcd, 0, sizeof(IM_MSG_RCD));
                            rcd.idInfo_send.ui64Id = pMsg->input.data.route.idInfo_from.ui64Id;
                            rcd.tSendTime = pMsg->input.tStartTime;
                            rcd.uiTranNo = pMsg->input.uiTranNo;
                            //  uiSeqNo
                            //
                            rcd.idInfo_logicalPeer = pMsg->input.addr_logicalPeer.idInfo;
                            rcd.iTaskId = pMsg->input.iTaskId;
                            //
                            rcd.idInfo_recv.ui64Id = pMsg->input.data.route.idInfo_to.ui64Id;
                            //
                            rcd.uiType = pContent->uiType;
                            //  lstrcpyn(  rcd.content,  pContent->html.wBuf,  mycountof(  rcd.content  )  );
                            switch (rcd.uiType)
                            {
                                case Consts.CONST_imCommType_transferFileReq:
                                    //mylstrcpyn(rcd.content, pContent->transferFileReq.fileName, Consts.cntof_log_msgContentLen);
                                    break;
                                case Consts.CONST_imCommType_transferAvInfo:
                                    if (0 != pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth && 0 != pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight)
                                    {
                                        //_sntprintf(rcd.content, mycountof(rcd.content), _T("%d * %d"), pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biWidth, pContent->transferAvInfo.ass.tranInfo.video.vh_decompress.bih.biHeight);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            mylstrcpyn(rcd.logMsgContent, getMsgDesc(ref *pMsg), Consts.cntof_logMsgContent);
                            //
                            rcd.tRecvTime = pMsg->input.tRecvTime;
                            mytime(ref rcd.firstTime);
                            mytime(ref rcd.lastTime);
                            mytime(ref rcd.tLastModifiedTime);

                            logImMsg(ref pQyMc.p_g_dbFuncs, pSubThreadInfo.pDb, pQyMc.cfg.db.iDbType, CONST_fieldIdTable_en, ref rcd);
                        }
                        break;



                    default:
                        //traceLog(_T("scheduler: unprocessed msgType %d"), pMsg->uiType);
                        break;

                }

            }
#endif

            iErr = 0;
        errLabel:
            return iErr;
        }






        //
        public static unsafe void mcThreadProc_mis_scheduler(object lpParameter)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            int loopCtrl = 0;
            //uint dwThreadId = GetCurrentThreadId();
            MIS_MSGU* pMsg = null;
            uint len;
            uint dwRet;
            //CQnmDb db=new CQnmDb();
            MT_SUBTHREADINFO subThreadInfo;
            uint dwCurTickCnt = 0;
            int i;
            CTX_MC_scheduler ctx;
            //
            uint uiChannelType = Consts.CONST_channelType_talking;
            //
            Int64 tStartTran; uint uiTranNo;

            //traceLogA("mcThreadProc_mis_scheduler:  enters");

            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                using (CQyMalloc mallocObj_pMsgBuf_forInput = new CQyMalloc())
                {
                    using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
                    {
                        using (CQnmDb db = new CQnmDb())
                        {

                            //mymemset((IntPtr)&subThreadInfo, 0, sizeof(MT_SUBTHREADINFO));
                            //mymemset(&ctx, 0, sizeof(CTX_MC_scheduler));
                            subThreadInfo = new MT_SUBTHREADINFO();
                            ctx = new CTX_MC_scheduler();
                            ctx.pMsgBuf_forInput = new CMIS_MSGU();// (MIS_MSGU*)mallocObj_pMsgBuf_forInput.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                            if (null == ctx.pMsgBuf_forInput) goto errLabel;
                            ctx.pMsgBuf = new CMIS_MSGU();// (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                            if (null == ctx.pMsgBuf) goto errLabel;

                            if (null == (pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;



                            if (!bObjDbAvail(pQyMc.procedObjDb))
                            {
                                goto errLabel;
                            }
                            if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;

                            subThreadInfo.pParentParam = pQyMc;
                            subThreadInfo.iServiceId = pQyMc.iServiceId;
                            subThreadInfo.pDb = pQyMc.gui.pDb;  // db.m_pDbMem->pDb;


#if DEBUG
                            testSendMsg(pQyMc);
#endif


                            //
                            for (; !pProcInfo.bQuit_threadProc_isCli; loopCtrl++)
                            {

                                //dwRet = WaitForSingleObject(pProcInfo->schedulerQ.hSemaTrigger, 1000);
                                //if (dwRet == WAIT_FAILED) goto errLabel;
                                myWaitOne(pProcInfo.schedulerQ.hSemaTrigger, 1000);

                                //
                                for (; !pProcInfo.bQuit_threadProc_isCli;)
                                {

                                    len = (uint)sizeof(MIS_MSGU);
                                    if (0 != qGetMsg(pProcInfo.schedulerQ, (IntPtr)pMsg, &len)) break;
                                    mymemset((IntPtr)(((byte*)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));

                                    switch (pMsg->uiType)
                                    {
                                        case Consts.CONST_misMsgType_input:

                                            if (!isUcFlgResp(pMsg->input.ucFlg))
                                            {
                                                procMsgInput_scheduler_req(ref subThreadInfo, pProcInfo, pMisCnt, &pMsg->input, len, ctx);
                                            }
                                            else
                                            {
                                                procMsgInput_scheduler_resp(ref subThreadInfo, pProcInfo, pMisCnt, &pMsg->input, len, ctx);
                                            }
                                            //
                                            break;
#if false
										case  Consts.CONST_misMsgType_imMsgRcd:
													  logImMsg_isCli(  subThreadInfo.pDb,  pQyMc.cfg.db.iDbType,  qyFuncs.CONST_fieldIdTable_en,  ref pMsg->imMsgRcd.rcd  );
											break;
#endif
#if false

										case CONST_misMsgType_event:
											logEvent_is(pQyMc->p_g_dbFuncs, subThreadInfo.pDb, pSci->cfg.iDbType, NULL, &pMsg->event.event  );
											 break;
#endif
                                        default:
                                            break;

                                    }
                                }


                                //  to log imMsg in db
                                if (!isQEmpty(pProcInfo.imMsgLogQ))
                                {
                                    procImMsgLogQ_scheduler(ref subThreadInfo, pProcInfo, pMsg);
                                }




                                //
                                dwCurTickCnt = myGetTickCount(null);


                                //

                                MIS_CHANNEL pChannel = null;
                                //
                                if (null == (pChannel = getChannelByType(pMisCnt, uiChannelType))) continue;

                                if (!bChannelOk(pChannel)) continue;
                                //
                                if (!pMisCnt.bIntervalParamsGot && dwCurTickCnt > pMisCnt.dwLastTickCnt_getIntervalParams + pMisCnt.cfg.usIntervalInS_getIntervalParams * 1000)
                                {
                                    MACRO_prepareForTran(out tStartTran, out uiTranNo);

                                    QIS_getCfgs_req req;
                                    //
                                    mymemset((IntPtr)(byte*)&req, 0, sizeof(QIS_getCfgs_req));
                                    req.uiType = Consts.CONST_imCommType_getCfgsReq;
                                    req.uiSizePerSnd_media = pProcInfo.cfg.uiSizePerSnd_media;
                                    //
                                    len = (uint)sizeof(QIS_getCfgs_req);
                                    MSG_ROUTE msgRoute = new MSG_ROUTE();
                                    pProcInfo.postMsg2Mgr_mc(ctx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_getCfgs, tStartTran, uiTranNo, 0, (byte*)&req, len, 0, 0, 0, null, false);
                                    //
                                    pMisCnt.dwLastTickCnt_getIntervalParams = dwCurTickCnt;
                                }

#if false
								if (pMisCnt->bIntervalParamsGot && !pMisCnt->bToPathsRetrieved && dwCurTickCnt > pMisCnt->dwLastTickCnt_retrieveToPaths + pMisCnt->cfg.usIntervalInS_getIntervalParams * 1000)
								{
								MACRO_prepareForTran();
								RETRIEVE_TO_PATHS req;
								//
								memset(&req, 0, sizeof(req));
								req.uiType = CONST_imCommType_retrieveToPaths;
								req.usMaxMemsPerSnd = mycountof(req.mems);
								//
								len = offsetof(RETRIEVE_TO_PATHS, mems) + req.usCnt * sizeof(req.mems[0]);
								pProcInfo->postMsg2Mgr_mc(pMisCnt, NULL, CONST_misMsgType_req, 0, CONST_qyCmd_getCfgs, tStartTran, uiTranNo, 0, (char*)&req, len, NULL, 0, 0, NULL, 0);
								pMisCnt->dwLastTickCnt_retrieveToPaths = dwCurTickCnt;
								}
#endif

#if true
                                uint usIntervalInS_refreshContactList = pMisCnt.cfg.usIntervalInS_refreshContactList;
#if true    //  2014/05/03. def  __WINCE__
                                if (0 == pMisCnt.refreshContactList.tLastRefreshedTime_misServ) usIntervalInS_refreshContactList = 20;
                                else usIntervalInS_refreshContactList = pMisCnt.cfg.usIntervalInS_refreshContactList;
                                //
#if DEBUG
#if false
															traceLog(  (  "for test."  )  );
															usIntervalInS_refreshContactList  =  20;
#endif
#endif
                                //
#endif
                                //  2014/02/11. 需要考虑正在处理中的retrieve, 当下载的messenger非常多时，就下载过程会很长。判断下dwLastTickCnt_respRecvd可以避免重复发起请求
                                if (dwCurTickCnt - pMisCnt.refreshContactList.dwLastTickCnt_respRecvd > 20000
                                && dwCurTickCnt > pMisCnt.refreshContactList.dwLastTickCnt_startToRetrieve + usIntervalInS_refreshContactList * 1000)
                                {
                                    RETRIEVE_IMOBJLIST_REQ req;
                                    
                                    MACRO_prepareForTran(out tStartTran, out uiTranNo);
                                    //
#if DEBUG
                                    traceLogA("scheduler: start to refreshContactList");
#endif

                                    {
                                        string str;
                                        str = string.Format("Start to retrieveImObjList, tn {0}, {1}", uiTranNo, bRefreshAllContacts(pMisCnt) ? ("全面") : ("增量"));
                                        uint step = 0;
                                        qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, (""), (""), str);

                                    }

                                    //
                                    pMisCnt.refreshContactList.tStartTime = tStartTran;
                                    pMisCnt.refreshContactList.uiTranNo = uiTranNo;
                                    pMisCnt.refreshContactList.idInfo_lastRefreshed.ui64Id = 0;    //  ±íÃ÷ÊÇ´ÓÍ·¿ªÊ¼Ë¢ÐÂ
                                                                                                   //
                                    mymemset((IntPtr)(byte*)&req, 0, sizeof(RETRIEVE_IMOBJLIST_REQ));
                                    req.uiType = Consts.CONST_imCommType_retrieveImGrpListReq;
                                    req.tLastRefreshedTime_contactList_misServ = pMisCnt.refreshContactList.tLastRefreshedTime_misServ;
                                    req.usMaxContactUpdateInfosPerSnd = pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_grp;
                                    //				   
                                    if (sizeof(IM_L_STREAM) > sizeof(MIS_MSGU)) goto errLabel;
                                    fixed (MIS_MSGU* pMsgBuf_forInput = &ctx.pMsgBuf_forInput.msgU)
                                    {
                                        IM_L_STREAM* pStream = null;

                                        pStream = (IM_L_STREAM*)pMsgBuf_forInput;    //  tmpMallocObj.malloc(  sizeof(  IM_STREAM_CONTENT  )  );
                                        if (pStream != null)
                                        {
                                            MACRO_memset_imLStream(pStream);
                                            pStream->uiType = Consts.CONST_imCommType_lStream;
                                            pStream->uiContentType = req.uiType;
                                            len = Consts.bufSize_imLStream; // sizeof(pStream->buf  );
                                            if (0 != retrieveImObjListReq2Stream(pStream->uiContentType, &req, pStream->buf, &len)) goto errLabel;
                                            pStream->ulStreamLen = len;
                                            len += (uint)Marshal.OffsetOf<IM_L_STREAM>("buf");
                                            //
                                            MSG_ROUTE msgRoute = new MSG_ROUTE();
                                            pProcInfo.postMsg2Mgr_mc(ctx.ctxCaller, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, tStartTran, uiTranNo, 0, (byte*)pStream, len, 0, 0, 0, ctx.pMsgBuf, false);
                                        }
                                    }
                                    //
                                    pMisCnt.refreshContactList.dwLastTickCnt_startToRetrieve = dwCurTickCnt;
                                }
#endif

#if false
									if (pMisCnt->refreshContactList.tLastRefreshedTime_misServ)
								{
								if (!pMisCnt->refreshImObjRules.tLastRefreshedTime)
								{
								if (dwCurTickCnt > pMisCnt->refreshImObjRules.dwLastTickCnt + 20000)
								{
									pMisCnt->refreshImObjRules.dwLastTickCnt = dwCurTickCnt;
									//
									mytime(&pMisCnt->refreshImObjRules.tStart_toRetrieveAll);       //  2011/11/12
																									//
									MACRO_prepareForTran();

									//
									qyShowInfo1(Consts.CONST_qyShowType_qwmComm, 0, (""), pProcInfo.str_who_showInfo, 0, (""), (""), ("Start to refreshImObjRules, tn %d"), uiTranNo);
								
									//
									REFRESH_imObjRules_req req;
									IM_L_STREAM* pStream = NULL;


									memset(&req, 0, sizeof(req));
									req.uiType = CONST_imCommType_refreshImObjRulesReq;
									req.ucbRetrieveAll = TRUE;
									req.usMaxMemsPerSnd = mycountof(req.mems);

									if (sizeof(IM_L_STREAM) > sizeof(ctx.common.pMsgBuf_forInput[0])) goto errLabel;
									pStream = (IM_L_STREAM*)ctx.common.pMsgBuf_forInput;    //  tmpMallocObj.malloc(  sizeof(  IM_STREAM_CONTENT  )  );
									MACRO_memset_imLStream(pStream);
									pStream->uiType = CONST_imCommType_lStream;
									pStream->uiContentType = req.uiType;
									len = sizeof(pStream->buf  );
									if (refreshImObjRulesReq2Stream(pStream->uiContentType, &req, pStream->buf, (uint *  ) & len)  )  goto errLabel;
									pStream->ulStreamLen = len;
									len += offsetof(IM_L_STREAM, buf);

									//
									pProcInfo->postMsg2Mgr_mc(pMisCnt, NULL, CONST_misMsgType_req, 0, CONST_qyCmd_refreshImObjListReq, tStartTran, uiTranNo, 0, (char*)pStream, len, NULL, 0, 0, NULL, 0);

								}
								}
								}
#endif

#if false
								if (pMisCnt->ucTriesToReg)
								{
								if (dwCurTickCnt > pMisCnt->dwLastTickCnt_tryToReg + pMisCnt->cfg.usIntervalInS_tryToReg * 1000)
								{
								sndMsgNeedReg_qmc(&subThreadInfo, pSci, pProcInfo, pMisCnt);
								pMisCnt->dwLastTickCnt_tryToReg = dwCurTickCnt;
								}
								}
								
								if (pMisCnt->ucbNeedSndDevInfo)
								{
								if (dwCurTickCnt > pMisCnt->dwLastTickCnt_tryToSndDevInfo + pMisCnt->cfg.usIntervalInS_tryToSndDevInfo * 1000)
								{
								sndDevInfo_qmc(&subThreadInfo, pSci, pProcInfo, pMisCnt);
								pMisCnt->dwLastTickCnt_tryToSndDevInfo = dwCurTickCnt;
								}
								}
#endif





#if true
                                //  for cpu usage. 2009/10/11
                                if (dwCurTickCnt > pProcInfo.status.dwTickCnt_toStat + 1000)
                                {
                                    getCpuUsage(ref pQyMc.env, pProcInfo);
                                    getNetStat_isCli(pProcInfo, dwCurTickCnt - pProcInfo.status.dwTickCnt_toStat);
                                    //  2015/06/18
                                    //getPtsStat_isCli(pProcInfo, dwCurTickCnt - pProcInfo->status.dwTickCnt_toStat);
                                    //
                                    //GlobalMemoryStatus(&pProcInfo->status.memory);


                                    pProcInfo.status.dwTickCnt_toStat = dwCurTickCnt;
                                }
#endif

                                //  if  (  !(  loopCtrl  %  20  )  )  traceLogA(  "ÕâÀï»¹Ó¦¸ÃÓÐÒ»¸ö¶¨ÆÚ£¨±ÈÈç30·ÖÖÓÒ»´ÎµÄ£¬ÇåÀíprocessQµÄ¹¤×÷£¬ÒÔ·ÀÖ¹processQÀïÓÐµÄ½ÚµãÊ§È¥±»´¦ÀíµÄ¿ÉÄÜµÄ¡£¾ÍÒª°ÑÕâÐ©À¬»ø½ÚµãÇåµô"  );

                            }

                            //  traceLogA(  "mcThreadProc_mis_scheduler:  Î´Íê³É"  );


                        }

                    }
                }
            }

        errLabel:


            pProcInfo.status.threadsStatus.ulbSchedulerQuit = true;

            Console.WriteLine("mcThreadProc_mis_scheduler:  leaves");

            return;
        }

        //
        public static int getCpuUsage(ref QY_ENV pEnv, CCtxQmc pProcInfo)
        {
            return 0;
        }

        public static unsafe int getNetStat_isCli(CCtxQmc pProcInfo, uint nIntervalInMs)
        {
            int iErr = -1;

            if (null == pProcInfo) return -1;
            //
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            if (null == pMisCnt) return -1;
            //
            IM_NET_STAT netStat;
            int i;
            Int64 i64InOctets;
            Int64 i64OutOctets;

            if (0 == nIntervalInMs) return -1;

            mymemset((IntPtr)(byte*)&netStat, 0, sizeof(IM_NET_STAT));
            for (i = 0; i < (pMisCnt.channels.Length); i++)
            {
                MIS_CHANNEL pChannel = pMisCnt.channels[i];
                //
                i64InOctets = (Int64)(pChannel.status.netStat.ui64MsgDataLenRecvd - pChannel.status.oldNetStat.ui64MsgDataLenRecvd);
                i64OutOctets = (Int64)(pChannel.status.netStat.ui64MsgDataLenSent - pChannel.status.oldNetStat.ui64MsgDataLenSent);
                if (i64InOctets < 0 || i64OutOctets < 0)
                {
                    //memcpy(&pChannel->status.oldNetStat, &pChannel->status.netStat, sizeof(pChannel->status.oldNetStat)  );
                    pChannel.status.oldNetStat = pChannel.status.netStat;
                    goto errLabel;
                }

                pChannel.status.netStat.uiInSpeedInKbps = (uint)(i64InOctets * 8.0f / nIntervalInMs);
                pChannel.status.netStat.uiOutSpeedInKbps = (uint)(i64OutOctets * 8.0f / nIntervalInMs);
                //
                //memcpy(  &pChannel->status.oldNetStat,  &pChannel->status.netStat,  sizeof(pChannel->status.oldNetStat  )  );
                pChannel.status.oldNetStat = pChannel.status.netStat;

                //
                netStat.ui64MsgDataLenRecvd += pChannel.status.netStat.ui64MsgDataLenRecvd;
                netStat.ui64MsgDataLenSent += pChannel.status.netStat.ui64MsgDataLenSent;

                //
                switch (pChannel.uiType)
                {
                    case Consts.CONST_channelType_media:
                        pProcInfo.status.netStat.uiInSpeedInKbps_v = pChannel.status.netStat.uiInSpeedInKbps;
                        pProcInfo.status.netStat.uiOutSpeedInKbps_v = pChannel.status.netStat.uiOutSpeedInKbps;
                        break;
                    case Consts.CONST_channelType_realTimeMedia:
                        pProcInfo.status.netStat.uiInSpeedInKbps_a = pChannel.status.netStat.uiInSpeedInKbps;
                        pProcInfo.status.netStat.uiOutSpeedInKbps_a = pChannel.status.netStat.uiOutSpeedInKbps;
                        break;
                    case Consts.CONST_channelType_robot:
                        pProcInfo.status.netStat.uiInSpeedInKbps_f = pChannel.status.netStat.uiInSpeedInKbps;
                        pProcInfo.status.netStat.uiOutSpeedInKbps_f = pChannel.status.netStat.uiOutSpeedInKbps;
                        break;
                    default:
                        break;
                }

                //
#if __DEBUG__
#endif
            }

            i64InOctets = (Int64)(netStat.ui64MsgDataLenRecvd - pProcInfo.status.netStat.ins.ui64MsgDataLenRecvd);
            i64OutOctets = (Int64)(netStat.ui64MsgDataLenSent - pProcInfo.status.netStat.ins.ui64MsgDataLenSent);
            if (i64InOctets < 0 || i64OutOctets < 0)
            {
                //memcpy(&pProcInfo.status.netStat, &netStat, sizeof(pProcInfo.status.netStat)  );
                pProcInfo.status.netStat.ins = netStat;
                goto errLabel;
            }
            netStat.uiInSpeedInKbps = (uint)(i64InOctets * 8.0f / nIntervalInMs);
            netStat.uiOutSpeedInKbps = (uint)(i64OutOctets * 8.0f / nIntervalInMs);

            pProcInfo.status.netStat.ins = netStat;

            //
            iErr = 0;
        errLabel:
            return iErr;
        }


    }
}

