using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {

        // #define		getSharedObjUsr(  pSharedObj,  iIndex_usr  )		(  (  (  pSharedObj  )  &&  (  iIndex_usr  )  >=0  &&  (  iIndex_usr  )  <  MAX_sharedObjUsrs  )  ?  &(  pSharedObj  )->usrs[iIndex_usr]  :  NULL  )

        public static SHARED_OBJ_USR getSharedObjUsr(QY_SHARED_OBJ pSharedObj, int iIndex_usr)
        {
            return (((pSharedObj != null) && (iIndex_usr) >= 0 && (iIndex_usr) < Consts.MAX_sharedObjUsrs) ? (pSharedObj).usrs[iIndex_usr] : null);
        }


        public static int getSharedObjIndex_localAv()
        {
            return 0;
        }

        //
        //  void  askSharedObjToStop(  MC_VAR_isCli  *  pProcInfo,  QY_SHARED_OBJ  *  pSharedObj,  HWND  hWnd_notifyEnd  )
        public static void askSharedObjToStop(CCtxQmc pProcInfo, QY_SHARED_OBJ pSharedObj, object hWnd_notifyEnd, string hint)

        {
            if (null == pSharedObj) return;

            /*
			TCHAR tBuf[128] = _T("");

			if (!hint) hint = _T("");
			_sntprintf(tBuf, mycountof(tBuf), _T("askSharedObjToStop called, %s"), hint);
			//
			showInfo_open(0, 0, tBuf);
			*/

            //
            if (!pSharedObj.var.bQuit)
            {
                pSharedObj.var.bQuit = true;
            }

            /*
			QM_SHM_CMD* pShmCmd = NULL;
			//  pShmCmd  =  getShmCmdByIndex(  (  QM_SHM_CMDS  *  )pProcInfo->cmdsShm.pBuf,  pSharedObj->iIndex_shmCmd  );
			pShmCmd = (QM_SHM_CMD*)pProcInfo->getQmShmCmdByIndex(pSharedObj->iIndex_shmCmd);
			if (pShmCmd) pShmCmd->bQuit = TRUE;
			*/


            GuiShare.pf_PostMessage(hWnd_notifyEnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_task_end, 0);

            return;
        }



    }
}
