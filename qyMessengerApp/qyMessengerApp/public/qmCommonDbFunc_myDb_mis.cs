using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {
        /////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        /// 
        public static unsafe bool bGetImAmRcdBySth_myDb(object pDbParam, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, string messengerName, int id, QY_imAm_RCD* pObj)
        {
            return false;
        }


        public static unsafe bool bGetImOtherRegInfoBySth_myDb(object pDbParam, int iDbType, ref QY_DMITEM[] pFieldIdTable, string pTabName, MSGR_ADDR* pAddr, int iRcdId, QY_MESSENGER_REGINFO* pRegInfo)
        {
            return false;
        }
        public static unsafe int tmpHandler_bGetMeInfoBySth_myDb(object p0, object p1, object p2)
        {
            int iRet = -1;

            //  p0
            QY_MESSENGER_INFO* pObj = (QY_MESSENGER_INFO*)(IntPtr)p1;
            QMEM_qyImObj* pQMem = (QMEM_qyImObj*)(IntPtr)p2;

            if (pQMem->messengerInfo.uiType == Consts.CONST_objType_me)
            {
                mymemcpy((IntPtr)pObj, (IntPtr)(byte*)&pQMem->messengerInfo, (uint)sizeof(QY_MESSENGER_INFO));
                iRet = 1; goto errLabel;
            }

            iRet = 0;
        errLabel:
            return iRet;

        }

        //  2013/02/05
        public static unsafe bool bGetMeInfoBySth_myDb(object pDbParam, int iDbType, string misServName, QY_MESSENGER_INFO* pObj)
        {
            bool bRet = false;
            CMyDb pDb = (CMyDb)pDbParam;

            if (qTraverse(pDb.m_var_pQ_qyImObjTab, tmpHandler_bGetMeInfoBySth_myDb, null, (IntPtr)pObj) <= 0) goto errLabel;

            bRet = true;

        errLabel:

            return bRet;
        }



        //
        public static unsafe int tmpHandler_bGetMessengerInfoBySth_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            QY_MESSENGER_ID* pIdInfo = (QY_MESSENGER_ID*)(IntPtr)p0;
            QMEM_qyImObj* pMem_o = (QMEM_qyImObj*)(IntPtr)p1;
            QMEM_qyImObj* pMem = (QMEM_qyImObj*)(IntPtr)p2;

            if (pIdInfo->ui64Id == pMem->messengerInfo.idInfo.ui64Id)
            {
                //
#if DEBUG
                ref QMEM_qyImObj tmp_pMem = ref *pMem;
#endif

                //
                //memcpy(pMem_o, pMem, sizeof(pMem_o[0]));
                *pMem_o = *pMem;
                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }




        //bool bGetMessengerRegInfoBySth_myDb(void* pDbParam, int iDbType, QY_DMITEM* pFieldIdTable, LPCTSTR pTabName, LPCTSTR misServName, QY_MESSENGER_ID* pIdInfo, int iRcdId, QY_MESSENGER_REGINFO* pRegInfo);
        public static unsafe bool bGetMessengerInfoBySth_myDb(QM_dbFuncs pDbFuncs, object pDbParam, int iDbType, ref QY_DMITEM[] pFieldIdTable, string misServName, ref QY_MESSENGER_ID pIdInfo1, int iRcdId, ref QY_MESSENGER_INFO pObjParam, ref QY_MESSENGER_REGINFO pRegInfo1)        //  2008/12/28ÐÞ¸Ä
        {

            bool bRet = false;
            CMyDb pDb = (CMyDb)pDbParam;
            QMEM_qyImObj qMem;
            qyFuncs.traceLog("myDb   qMemqMem ");
            QY_MESSENGER_INFO tmpObjInfo;
            //QY_MESSENGER_INFO* pObj = null;

            if (null == misServName || 0 == pIdInfo1.ui64Id)
            {
                traceLog(("bGetMessengerInfoBySth_myDb failed, param err"));
                return false;
            }

            mymemset((IntPtr)(byte*)&qMem, 0, sizeof(QMEM_qyImObj));

            fixed (QY_MESSENGER_ID* pIdInfo = &pIdInfo1)
            {

                if (qTraverse(pDb.m_var_pQ_qyImObjTab, tmpHandler_bGetMessengerInfoBySth_myDb, (IntPtr)pIdInfo, (IntPtr)(byte*)&qMem) <= 0) goto errLabel;
                qyFuncs.traceLog(" myDb  qTraverse");

                //
                //pObj = (pObjParam != null) ? pObjParam : &tmpObjInfo; //  ×¢Òâ£¬ÕâÀï£¬ÒòÎªÏÂÃæµÄregInfo¶¼ÒªÓÃµ½uiType, ËùÒÔ£¬ÕâÀïÊÇÒ»¶¨ÐèÒª°ÑÐÅÏ¢È¡³öÀ´µÄ¡£
                fixed (QY_MESSENGER_INFO  *pObj = &pObjParam) {
                    qyFuncs.traceLog(" myDb  pObj");

                    if (pObj != null)
                    {
                        mymemcpy((IntPtr)pObj, (IntPtr)(byte*)&qMem.messengerInfo, (uint)sizeof(QY_MESSENGER_INFO));
                    }
                    fixed(QY_MESSENGER_REGINFO *pRegInfo = &pRegInfo1)              if (pRegInfo != null)
                    {

                        mymemset((IntPtr)pRegInfo, 0, sizeof(QY_MESSENGER_REGINFO));
                        //
                        if (bGetMessengerRegInfoBySth_myDb(pDbParam, iDbType, ref pFieldIdTable, Consts.CONST_tabName_qyImObjRegInfoTab, misServName, pIdInfo, 0, pRegInfo))
                        {
                            qyFuncs.traceLog(" myDb  bGetMessengerRegInfoBySth_myDb");

                            //lstrcpyn(pRegInfo->addr.misServName, misServName, mycountof(pRegInfo->addr.misServName));
                            pRegInfo->addr.uiObjType = pObj->uiType;
                            pRegInfo->addr.idInfo.ui64Id = pIdInfo->ui64Id;
                            qyFuncs.traceLog(" myDb   pRegInfo->");

                        }
                    }
                }
            }

            bRet = true;

        errLabel:

            return bRet;
        }



        public static unsafe int tmpHandler_bGetMessengerRegInfoBySth_myDb(object p0, object p1, object p2)
        {
            int iRet = -1;
            int i;
            CCtxQyMc pQyMc = g.g_pQyMc;

            QY_MESSENGER_ID* pIdInfo = (QY_MESSENGER_ID*)(IntPtr)p0;
            QY_MESSENGER_REGINFO* pRegInfo = (QY_MESSENGER_REGINFO*)(IntPtr)p1;
            QMEM_qyImObjRegInfo* pQMem = (QMEM_qyImObjRegInfo*)(IntPtr)p2;

            if (pQMem->addr.idInfo.ui64Id == pIdInfo->ui64Id)
            {

                mymemset((IntPtr)pRegInfo, 0, sizeof(QY_MESSENGER_REGINFO));
                pRegInfo->uiType = Consts.CONST_imCommType_messengerRegInfo;
                //  id
                pRegInfo->iCustomId = qyGetCustomId();

                mymemcpy((IntPtr)(byte*)&pRegInfo->addr, (IntPtr)(byte*)&pQMem->addr, (uint)sizeof(MSGR_ADDR));

                //memcpy(pRegInfo->uiFieldIds, pQMem->myRegInfo.uiFieldIds, sizeof(pQMem->myRegInfo.uiFieldIds)  );
                //memcpy(pRegInfo->u.cols, pQMem->myRegInfo.u.cols, sizeof(pRegInfo->u.cols)  );

                //
                //MACRO_setPtrs(CONST_fieldIdTable_en, pRegInfo[0], pRegInfo->fields);
                mytcsncpy(pRegInfo->dw, new string(pQMem->dw), Consts.CONST_maxNameLen + 1);
                mytcsncpy(pRegInfo->bm, new string(pQMem->bm), Consts.CONST_maxNameLen + 1);
                mytcsncpy(pRegInfo->syr, new string(pQMem->syr), Consts.CONST_maxNameLen + 1);


                //
                iRet = 1; goto errLabel;
            }

            iRet = 0;
        errLabel:
            return iRet;
        }


        //  2013/02/04
        public static unsafe bool bGetMessengerRegInfoBySth_myDb(object pDbParam, int iDbType, ref QY_DMITEM[] pFieldIdTable, string pTabName, string misServName, QY_MESSENGER_ID* pIdInfo, int iRcdId, QY_MESSENGER_REGINFO* pRegInfo)
        {
            bool bRet = false;
            CMyDb pDb = (CMyDb)pDbParam;

            if (null == pTabName) return false;
            if (null == misServName || null == pIdInfo || 0 == pIdInfo->ui64Id)
            {
                traceLog(("bGetMessengerRegInfoBySth_myDb failed, param err"));
                return false;
            }

            if (0 == string.Compare(pTabName, Consts.CONST_tabName_qyImObjRegInfoTab))
            {
                if (qTraverse(pDb.m_var_pQ_qyImObjRegInfoTab, tmpHandler_bGetMessengerRegInfoBySth_myDb, (IntPtr)pIdInfo, (IntPtr)pRegInfo) <= 0) goto errLabel;
            }
            else
                goto errLabel;

            bRet = true;

        errLabel:

            return bRet;

        }


        public static unsafe int insertImObj_recoverImGrp_myDb(object pDbParam, int iDbType, uint uiObjType, IM_GRP_INFO* pGrpInfo, Int64 tLastModifiedTime)
        {

            int iErr = -1;

            CMyDb pDb = (CMyDb)pDbParam;

            QMEM_qyImObj mem;
            mymemset((IntPtr)(byte*)&mem, 0, sizeof(QMEM_qyImObj));
            mem.messengerInfo.uiType = uiObjType;
            mem.messengerInfo.idInfo.ui64Id = pGrpInfo->idInfo.ui64Id;
            mem.messengerInfo.iStatus = Consts.CONST_qyStatus_ok;
            if (0 != qPostMsg((IntPtr)(byte*)&mem, (uint)sizeof(QMEM_qyImObj), pDb.m_var_pQ_qyImObjTab)) goto errLabel;

            iErr = 0;

        errLabel:
            return iErr;

        }

        public static unsafe int tmpHandler_updateImObj_recoverImGrp_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            IM_GRP_INFO* pGrpInfo = (IM_GRP_INFO*)(IntPtr)p0;
            uint uiObjType = (uint)p1;
            QMEM_qyImObj* pMem = (QMEM_qyImObj*)(IntPtr)p2;

            if (pMem->messengerInfo.idInfo.ui64Id == pGrpInfo->idInfo.ui64Id)
            {
                pMem->messengerInfo.uiType = uiObjType;
                pMem->messengerInfo.iStatus = Consts.CONST_qyStatus_ok;
                iErr = 1; goto errLabel;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int updateImObj_recoverImGrp_myDb(object pDbParam, int iDbType, uint uiObjType, IM_GRP_INFO* pGrpInfo, Int64 tLastModifiedTime, int iRcdId)
        {
            int iErr = -1;
            CMyDb pDb = (CMyDb)pDbParam;

            if (null == pGrpInfo || 0 == pGrpInfo->idInfo.ui64Id)
            {
                traceLog(("updateImObj_recoverImGrp_myDb failed, param err"));
                return -1;
            }

            if (qTraverse(pDb.m_var_pQ_qyImObjTab, tmpHandler_updateImObj_recoverImGrp_myDb, (IntPtr)pGrpInfo, (IntPtr)uiObjType) < 0) goto errLabel;
            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe int updateImObj_recoverImGrp2_myDb(object pDbParam, int iDbType, uint uiObjType, IM_GRP_INFO* pGrpInfo, Int64 tLastModifiedTime, int iRcdId)
        {
            return 0;
        }

        public static unsafe int tmpHandler_bGetImGrpInfoBySth_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            QY_MESSENGER_ID* pIdInfo = (QY_MESSENGER_ID*)(IntPtr)p0;
            IM_GRP_INFO* pGrpInfo = (IM_GRP_INFO*)(IntPtr)p1;
            IM_GRP_INFO* pMem = (IM_GRP_INFO*)(IntPtr)p2;

            if (pMem->idInfo.ui64Id == pIdInfo->ui64Id)
            {
                mymemcpy((IntPtr)pGrpInfo, (IntPtr)pMem, (uint)sizeof(IM_GRP_INFO));
                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;

        }

        public static unsafe bool bGetImGrpInfoBySth_myDb(object pDbParam, int iDbType, string misServName, ref QY_MESSENGER_ID pIdInfo1, ref IM_GRP_INFO pRcd1)
        {
            bool bRet = false;
            CMyDb pDb = (CMyDb)pDbParam;

            if (null == misServName || null == pIdInfo1.ui64Id )
            {
                traceLog(("bGetImGrpInfoBySth_myDb failed, param err"));
                return false;
            }

            fixed (QY_MESSENGER_ID* pIdInfo = &pIdInfo1)
            {
                fixed (IM_GRP_INFO* pRcd = &pRcd1)
                {
                    if (qTraverse(pDb.m_var_pQ_qyImGrpInfoTab, tmpHandler_bGetImGrpInfoBySth_myDb, (IntPtr)pIdInfo, (IntPtr)pRcd) <= 0) goto errLabel;
                }
            }

            //
            bRet = true;

        errLabel:

            return bRet;

        }


        public static unsafe int insertImGrp_recoverImGrp_myDb(object pDbParam, int iDbType, IM_GRP_INFO* pGrpInfo)
        {
            int iErr = -1;

            CMyDb pDb = (CMyDb)pDbParam;


            if (null == pGrpInfo) return -1;


            if (0 != qPostMsg((IntPtr)pGrpInfo, (uint)sizeof(IM_GRP_INFO), pDb.m_var_pQ_qyImGrpInfoTab)) goto errLabel;

            iErr = 0;
        errLabel:
            return iErr;

        }

        public static unsafe int tmpHandler_updateImGrp_recoverImGrp_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            IM_GRP_INFO* pGrpInfo = (IM_GRP_INFO*)(IntPtr)p0;
            //  p1
            IM_GRP_INFO* pMem = (IM_GRP_INFO*)(IntPtr)p2;

            if (pGrpInfo->idInfo.ui64Id == pMem->idInfo.ui64Id)
            {
                safeTcsnCpy(pGrpInfo->name, pMem->name, Consts.CONST_maxNameLen + 1);
                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe int updateImGrp_recoverImGrp_myDb(object pDbParam, int iDbType, IM_GRP_INFO* pGrpInfo, int iRcdId)
        {

            int iErr = -1;

            CMyDb pDb = (CMyDb)pDbParam;

            if (null == pGrpInfo || 0 == pGrpInfo->idInfo.ui64Id)
            {
                traceLog(("updateImGrp_recoverImGrp_myDb failed, param err"));
                return -1;
            }

            if (qTraverse(pDb.m_var_pQ_qyImGrpInfoTab, tmpHandler_updateImGrp_recoverImGrp_myDb, (IntPtr)pGrpInfo, 0) < 0) goto errLabel;

            iErr = 0;
        errLabel:
            return iErr;

        }

        //
        public static unsafe int updateImObj_refreshImGrps_end_myDb(object pDb, int iDbType, string misServName, uint uiObjType, Int64 tLastModifiedTime)
        {
            return 0;

        }

        public static unsafe int updateImGrpMem_refreshImGrpMems_end_myDb(object pDb, int iDbType, string misServName, Int64 tLastModifiedTime)
        {
            return 0;
        }


        public static unsafe int updateImObj_refreshContacts_end_myDb(object pDb, int iDbType, string misServName, Int64 tLastModifiedTime)
        {
            return 0;
        }


        public static unsafe int tmpHandler_bGetImGrpMemBySth_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            COMMON_PARAM pCommonParam0 = (COMMON_PARAM)p0;
            IM_GRP_MEM* pRcd = (IM_GRP_MEM*)(IntPtr)p1;
            IM_GRP_MEM* pQMem = (IM_GRP_MEM*)(IntPtr)p2;

            QY_MESSENGER_ID* pIdInfo_grp = (QY_MESSENGER_ID*)(IntPtr)pCommonParam0.p0;
            QY_MESSENGER_ID* pIdInfo_mem = (QY_MESSENGER_ID*)(IntPtr)pCommonParam0.p1;
            int iRcdId = (int)(IntPtr)pCommonParam0.p2;
            //

            if (null != pIdInfo_grp && null != pIdInfo_mem)
            {
                if (pQMem->idInfo_grp.ui64Id == pIdInfo_grp->ui64Id
                    && pQMem->idInfo_mem.ui64Id == pIdInfo_mem->ui64Id)
                {
                    mymemcpy((IntPtr)pRcd, (IntPtr)pQMem, (uint)sizeof(IM_GRP_MEM));
                    iErr = 1; goto errLabel;
                }
            }
            else if (pQMem->id == iRcdId)
            {
                mymemcpy((IntPtr)pRcd, (IntPtr)pQMem, (uint)sizeof(IM_GRP_MEM));
                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe bool bGetImGrpMemBySth_myDb(object pDbParam, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo_grp, QY_MESSENGER_ID* pIdInfo_mem, int iRcdId, IM_GRP_MEM* pRcd)
        {
            bool bRet = false;

            CMyDb pDb = (CMyDb)pDbParam;
            COMMON_PARAM commonParam = new COMMON_PARAM();


            MACRO_makeCommonParam3((IntPtr)pIdInfo_grp, (IntPtr)pIdInfo_mem, (IntPtr)iRcdId, commonParam);
            if (qTraverse(pDb.m_var_pQ_qyImGrpMemTab, tmpHandler_bGetImGrpMemBySth_myDb, commonParam, (IntPtr)pRcd) <= 0) goto errLabel;


            bRet = true;
        errLabel:
            return bRet;
        }

        public static unsafe int insertImGrpMem_recoverImGrpMem_myDb(object pDbParam, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime)
        {
            int iErr = -1;

            CMyDb pDb = (CMyDb)pDbParam;


            if (null == pGrpMem) return -1;

            //  2014/04/20
            int tt = 0;
            pGrpMem->id = (int)getuiNextTranNo(null, 0, ref tt);

            if (0 != qPostMsg((IntPtr)pGrpMem, (uint)sizeof(IM_GRP_MEM), pDb.m_var_pQ_qyImGrpMemTab)) goto errLabel;

            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe int tmpHandler_updateImGrpMem_recoverImGrpMem_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            IM_GRP_MEM* pGrpMem = (IM_GRP_MEM*)(IntPtr)p0;
            //  p1
            IM_GRP_MEM* pQMem = (IM_GRP_MEM*)(IntPtr)p2;

            if (pGrpMem->idInfo_grp.ui64Id == pQMem->idInfo_grp.ui64Id && pGrpMem->idInfo_mem.ui64Id == pQMem->idInfo_mem.ui64Id)
            {
                pQMem->iRole = pGrpMem->iRole;
                pQMem->iStatus = pGrpMem->iStatus;
                iErr = 1; goto errLabel;
            }

            iErr = 0;

        errLabel:

            return iErr;
        }

        public static unsafe int updateImGrpMem_recoverImGrpMem_myDb(object pDbParam, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime, int iRcdId)
        {
            int iErr = -1;
            //TCHAR sqlBuf[CONST_maxSqlBufLen + 1] = _T("");
            //char timeBuf[CONST_qyTimeLen + 1];

            if (null == pGrpMem) return -1;
            if (null == pGrpMem->idInfo_grp.ui64Id || 0 == pGrpMem->idInfo_mem.ui64Id)
            {
                traceLog(("updateImGrpMem_recoverImGrpMem_myDb failed, param err"));
                return -1;
            }

            CMyDb pDb = (CMyDb)pDbParam;

            if (qTraverse(pDb.m_var_pQ_qyImGrpMemTab, tmpHandler_updateImGrpMem_recoverImGrpMem_myDb, (IntPtr)pGrpMem, 0) < 0) goto errLabel;

            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe int updateImGrpMem_recoverImGrpMem2_myDb(object pDbParam, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime, int iRcdId)
        {
            return 0;
        }

        //
        public static unsafe int insertImObj_recoverMessenger_myDb(QM_dbFuncs pDbFuncs, object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj, Int64 tLastModifiedTime)
        {
            int iErr = -1;

            CMyDb pDb = (CMyDb)pDbParam;
            QMEM_qyImObj qMem;

            mymemset((IntPtr)(byte*)&qMem, 0, sizeof(QMEM_qyImObj));
            mymemcpy((IntPtr)(byte*)&qMem.messengerInfo, (IntPtr)pObj, (uint)sizeof(QY_MESSENGER_INFO));

            if (0 != qPostMsg((IntPtr)(byte*)&qMem, (uint)sizeof(QMEM_qyImObj), pDb.m_var_pQ_qyImObjTab)) goto errLabel;

            iErr = 0;
        errLabel:

            return iErr;
        }

        public static unsafe int tmpHandler_updateImObj_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            QY_MESSENGER_INFO* pObj = (QY_MESSENGER_INFO*)(IntPtr)p0;
            //  p1
            QMEM_qyImObj* pMem = (QMEM_qyImObj*)(IntPtr)p2;

            if (pMem->messengerInfo.idInfo.ui64Id == pObj->idInfo.ui64Id)
            {
                pMem->messengerInfo.uiType = pObj->uiType;
                //
                pMem->messengerInfo.iRole = pObj->iRole;    //  2017/08/14
                                                            //
                pMem->messengerInfo.iStatus = pObj->iStatus;
                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:

            return iErr;
        }

        public static unsafe int updateImObj_recoverMessenger_myDb(object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj, Int64 tLastModifiedTime, int iRcdId)
        {
            int iErr = -1;
            CMyDb pDb = (CMyDb)pDbParam;

            if (null == pObj || 0 == pObj->idInfo.ui64Id)
            {
                traceLog(("updateImObj_recoverMessenger_myDb failed, param err"));
                return -1;
            }

            if (qTraverse(pDb.m_var_pQ_qyImObjTab, tmpHandler_updateImObj_myDb, (IntPtr)pObj, 0) < 0) goto errLabel;

            iErr = 0;

        errLabel:

            return iErr;
        }

        public static unsafe int updateImObj_recoverMessenger2_myDb(object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj, Int64 tLastModifiedTime, int iRcdId)
        {
            return 0;
        }

        public static unsafe int qisRegUsrInfo_myDb(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QY_MESSENGER_INFO* pMessengerInfo, QY_MESSENGER_REGINFO* pRegInfo, bool bLogReRegEvent)
        {
            int iErr = -1;

            if (0 != qisRegUsrInfoInTab(pDbFuncs, pDb, iDbType, ref pFieldIdTable, pMessengerInfo, pRegInfo, Consts.CONST_tabName_qyImObjRegInfoTab)) goto errLabel;

            iErr = 0;

        errLabel:
            if (Consts.CONST_Status_Window_Function_Open == 0)
            {
                traceLogA("qisRegUsrInfo leaves");

            }

            return iErr;

        }

        public static unsafe int tmpHandler_idInfoExistedInImObjRegInfoTab_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;

            QY_MESSENGER_ID* pIdInfo = (QY_MESSENGER_ID*)(IntPtr)p0;
            // p1
            QMEM_qyImObjRegInfo* pQMem = (QMEM_qyImObjRegInfo*)(IntPtr)p2;

            if (pQMem->addr.idInfo.ui64Id == pIdInfo->ui64Id)
            {

                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe bool bIdInfoExistedInImObjRegInfoTab_myDb(object pDbParam, int iDbType, QY_MESSENGER_ID* pIdInfo, string tabName)
        {
            bool bRet = false;
            CMyDb pDb = (CMyDb)pDbParam;

            if (null == pIdInfo || 0 == pIdInfo->ui64Id) return false;

            string str_name0, str_name1;
            fixed (char* pName = pDb.m_var_pQ_qyImObjRegInfoTab.cfg.name)
            {
                str_name0 = new string(pName);
            }
            fixed (char* pName = pDb.m_var_pQ_qyImObjRegInfoTab1.cfg.name)
            {
                str_name1 = new string(pName);
            }
            if (0 == mytcsicmp(str_name0, tabName))
            {
                if (qTraverse(pDb.m_var_pQ_qyImObjRegInfoTab, tmpHandler_idInfoExistedInImObjRegInfoTab_myDb, (IntPtr)pIdInfo, 0) <= 0) goto errLabel;
            }
            else if (0 == mytcsicmp(str_name1, tabName))
            {
                if (qTraverse(pDb.m_var_pQ_qyImObjRegInfoTab1, tmpHandler_idInfoExistedInImObjRegInfoTab_myDb, (IntPtr)pIdInfo, 0) <= 0) goto errLabel;
            }
            else goto errLabel;

            bRet = true;
        errLabel:
            return bRet;
        }


        public static unsafe int insertImObjRegInfo_qisRegUsrInfoInTab_myDb(object pDbParam, int iDbType, QY_MESSENGER_REGINFO* pRegInfo, string tabName)
        {
            int iErr = -1;
            CMyDb pDb = (CMyDb)pDbParam;
            QMEM_qyImObjRegInfo qMem;

            if (null == pRegInfo) return -1;

            mymemset((IntPtr)(byte*)&qMem, 0, sizeof(QMEM_qyImObjRegInfo));

            qMem.uiType = pRegInfo->uiType;
            //  id
            qMem.iCustomId = pRegInfo->iCustomId;

            mymemcpy((IntPtr)(byte*)&qMem.addr, (IntPtr)(byte*)&pRegInfo->addr, (uint)sizeof(MSGR_ADDR));
            //memcpy(qMem.uiFieldIds, pRegInfo->uiFieldIds, sizeof(qMem.myRegInfo.uiFieldIds));
            //memcpy(qMem.u.cols, pRegInfo->u.cols, sizeof(qMem.myRegInfo.u.cols));
            mytcsncpy(qMem.dw, new string(pRegInfo->dw), Consts.CONST_maxNameLen + 1);
            mytcsncpy(qMem.bm, new string(pRegInfo->bm), Consts.CONST_maxNameLen + 1);
            mytcsncpy(qMem.syr, new string(pRegInfo->syr), Consts.CONST_maxNameLen + 1);

            string str_name0, str_name1;
            fixed (char* pName = pDb.m_var_pQ_qyImObjRegInfoTab.cfg.name)
            {
                str_name0 = new string(pName);
            }
            fixed (char* pName = pDb.m_var_pQ_qyImObjRegInfoTab1.cfg.name)
            {
                str_name1 = new string(pName);
            }

            if (0 == mytcsicmp(str_name0, tabName))
            {
                if (0 != qPostMsg((IntPtr)(byte*)&qMem, (uint)sizeof(QMEM_qyImObjRegInfo), pDb.m_var_pQ_qyImObjRegInfoTab)) goto errLabel;
            }
            else if (0 == mytcsicmp(str_name1, tabName))
            {
                if (0 != qPostMsg((IntPtr)(byte*)&qMem, (uint)sizeof(QMEM_qyImObjRegInfo), pDb.m_var_pQ_qyImObjRegInfoTab1)) goto errLabel;
            }
            else goto errLabel;


            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe int tmpHandler_updateImObjRegInfo_qisRegUsrInfoInTab_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;

            QY_MESSENGER_REGINFO* pRegInfo = (QY_MESSENGER_REGINFO*)(IntPtr)p0;
            // p1
            QMEM_qyImObjRegInfo* pQMem = (QMEM_qyImObjRegInfo*)(IntPtr)p2;

            if (pQMem->addr.idInfo.ui64Id == pRegInfo->addr.idInfo.ui64Id)
            {

                //memcpy(pQMem->myRegInfo.uiFieldIds, pRegInfo->uiFieldIds, sizeof(pQMem->myRegInfo.uiFieldIds)  );
                //memcpy(pQMem->myRegInfo.u.cols, pRegInfo->u.cols, sizeof(pQMem->myRegInfo.u.cols)  );
                mytcsncpy(pQMem->dw, pRegInfo->dw, Consts.CONST_maxNameLen + 1);
                mytcsncpy(pQMem->bm, pRegInfo->bm, Consts.CONST_maxNameLen + 1);
                mytcsncpy(pQMem->syr, pRegInfo->syr, Consts.CONST_maxNameLen + 1);

                iErr = 1; goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;

        }

        public static unsafe int updateImObjRegInfo_qisRegUsrInfoInTab_myDb(object pDbParam, int iDbType, QY_MESSENGER_REGINFO* pRegInfo, string tabName)
        {
            int iErr = -1;
            CMyDb pDb = (CMyDb)pDbParam;

            if (null == pRegInfo) return -1;

            string str_name0, str_name1;
            fixed (char* pName = pDb.m_var_pQ_qyImObjRegInfoTab.cfg.name)
            {
                str_name0 = new string(pName);
            }
            fixed (char* pName = pDb.m_var_pQ_qyImObjRegInfoTab1.cfg.name)
            {
                str_name1 = new string(pName);
            }

            if (0 == mytcsicmp(str_name0, tabName))
            {
                if (qTraverse(pDb.m_var_pQ_qyImObjRegInfoTab, tmpHandler_updateImObjRegInfo_qisRegUsrInfoInTab_myDb, (IntPtr)pRegInfo, 0) < 0) goto errLabel;
            }
            else if (0 == mytcsicmp(str_name1, tabName))
            {
                if (qTraverse(pDb.m_var_pQ_qyImObjRegInfoTab1, tmpHandler_updateImObjRegInfo_qisRegUsrInfoInTab_myDb, (IntPtr)pRegInfo, 0) < 0) goto errLabel;
            }
            else goto errLabel;

            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        /////////////
        public static unsafe int tmpHandler_updateImObj_me_myDb(object p0, object p1, object p2)
        {
            int iErr = -1;
            QY_MESSENGER_INFO* pObj_me = (QY_MESSENGER_INFO*)(IntPtr)p0;
            //  p1
            QMEM_qyImObj* pMem = (QMEM_qyImObj*)(IntPtr)p2;

            if (pObj_me->idInfo.ui64Id == pMem->messengerInfo.idInfo.ui64Id)
            {
                if (pMem->messengerInfo.uiType != Consts.CONST_objType_me)
                {
                    pMem->messengerInfo.uiType = Consts.CONST_objType_me;
                }
            }
            else
            {
                if (pMem->messengerInfo.uiType == Consts.CONST_objType_me)
                {
                    pMem->messengerInfo.uiType = Consts.CONST_objType_messenger;
                }
            }

            iErr = 0;
            return iErr;
        }

        public static unsafe int updateImObj_me_myDb(object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj_me)
        {
            int iErr = -1;
            CMyDb pDb = (CMyDb)pDbParam;

            if (qTraverse(pDb.m_var_pQ_qyImObjTab, tmpHandler_updateImObj_me_myDb, (IntPtr)pObj_me, 0) < 0) goto errLabel;


            iErr = 0;
        errLabel:
            return iErr;

        }


    }
}
