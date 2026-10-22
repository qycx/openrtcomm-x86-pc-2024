using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    public struct QMem_imObj_intQ
    {
        public QY_MESSENGER_ID idInfo;
    }

    partial class qyFuncs
{

        public static unsafe int postImObjToIntQ(QY_MESSENGER_ID idInfo)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            if (idInfo.ui64Id == 0) return -1;

            QMem_imObj_intQ mem;
            mymemset((IntPtr)(byte*)&mem, 0, sizeof(QMem_imObj_intQ));
            mem.idInfo = idInfo;

            //
            qPostMsg((IntPtr)(byte*)&mem, (uint)sizeof(QMem_imObj_intQ), pMisCnt.imObj_intQ);


            iErr = 0;
        errLabel:
            return iErr;

        }

        public static unsafe int triggerToDisplayImObjIntQ()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_imObj_needRefreshed, 0);


            //
            return 0;
        }



    }
}
