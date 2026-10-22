

using qy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;



namespace qy
{

    partial class qyFuncs
    {

        public static unsafe int qGetMsg(GENERIC_Q pQ, ref MIS_MSGU pQElem1, uint* puiSize)
        {
            if (pQ == null) return -1;
            if (null == puiSize) return -1;



            using (CQySyncObj syncObj = new CQySyncObj())
            {

                if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0) return -1;

                fixed (MIS_MSGU* pQElem = &pQElem1)
                {
                    return tmp_qGetMsg(pQ, (IntPtr)pQElem, ref *puiSize, false);
                }
            }
        }



        //  2011/07/15. 一次性读取多个包。
        //  pQElems[i]为buf
        //  *puiSizes[i]为size,输出为长度
        //  *pnQElems为最多一次性读取多少个包，输出真实读取的包数
        //public static unsafe int qGetMsgs(GENERIC_Q pQ, IntPtr[] pQElems, uint[] puiSizes, uint* pnQElems)
        public static unsafe int qGetMsgs(GENERIC_Q pQ, ref CMIS_MSGU[] pQElems1, uint[] puiSizes, ref uint pnQElems1)
        {
            if (pQ == null) return -1;
            if ( null == puiSizes ) return -1;

            int i;

            using (CQySyncObj syncObj = new CQySyncObj())
            {

                if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0) return -1;

                for (i = 0; i < pnQElems1; i++)
                {
                    fixed (MIS_MSGU* pQElem = &pQElems1[i].msgU)
                    {
                        if (tmp_qGetMsg(pQ, (IntPtr)pQElem, ref puiSizes[i], false) != 0) break;
                    }
                }
                pnQElems1 = (uint)i;

            }
            //  OutputDebugString(  _T(  "qGetMsgs: Not finished\n"  )  );
            return (i != 0) ? 0 : -1;
        }

        //
        //public static unsafe int q2GetMsg(QY_Q2 pQ2, IntPtr pQElem, uint* puiSize)
        public static unsafe int q2GetMsg(QY_Q2 pQ2, ref MIS_MSGU pQElem1, ref uint puiSize1)
        {
            fixed(MIS_MSGU *pQElem=&pQElem1)
            {
                uint uiSize = puiSize1;
                int tmpiRet;
                tmpiRet = q2GetMsg(pQ2, (IntPtr)pQElem, &uiSize);
                puiSize1 = uiSize;
                return tmpiRet;
            }
        }





    }
}
