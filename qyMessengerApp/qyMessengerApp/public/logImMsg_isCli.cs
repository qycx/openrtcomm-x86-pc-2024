

namespace qy
{
    partial class qyFuncs
    {

        public static int logImMsg(ref QM_dbFuncs pDbFuncs, object pDb, int iDbType, QY_DMITEM[] pFieldIdTable, ref IM_MSG_RCD pRcd)
        {
            int iErr = -1;

            //
            if (null==pDbFuncs) return -1;
            QM_dbFuncs  g_dbFuncs = pDbFuncs;

            return logImMsg_isCli(pDb, iDbType, pFieldIdTable, ref pRcd);

        }

            public static unsafe int logImMsg_isCli(object pDbParam, int iDbType, QY_DMITEM[] pFieldIdTable, ref IM_MSG_RCD pRcdParam)
        {

            //
            CMyDb pDb = (CMyDb)pDbParam;

            fixed (IM_MSG_RCD* pRcd = &pRcdParam) {
                //
                qPostMsg((System.IntPtr)pRcd, (uint)sizeof(IM_MSG_RCD), pDb.m_var_pQ_qyImMsgTab);

                    
            }



            return 0;
        }



    }

}