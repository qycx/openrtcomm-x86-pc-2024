using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int setFlg_forceKeyFrame(CCtxQmc pProcInfo, int iIndex_sharedObj)
		{
			int iErr = -1;

			bool bDbg = false;
			string str;

			//  MC_VAR_isCli	*	pProcInfo		=	QY_GET_procInfo_isCli(  );
			if (null==pProcInfo) return -1;

			QY_SHARED_OBJ pSharedObj = null;
			CAP_procInfo_bmpCommon pCapBmp = null;
			//TCHAR tBuf[128] = _T("");


			pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
			if (null==pSharedObj) goto errLabel;
			pCapBmp = getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
			if (null==pCapBmp) goto errLabel;


			//
			uint dwTickCnt = myGetTickCount(null);

			//
			int iElapseInMs = (int)(dwTickCnt - pCapBmp.compressVideo.encInfo.dwTickCnt_startToForceKeyFrame);

			if (Math.Abs(iElapseInMs) > 500)
			{
				//	
				pCapBmp.compressVideo.encInfo.dwTickCnt_startToForceKeyFrame = dwTickCnt;

				//  2017/10/14
				str=string.Format("{0}, iIndex_sharedObj {1}", _T("setFlg_forceKeyFrame ok"), iIndex_sharedObj);
				showInfo_open(0, null, str);

			}


			iErr = 0;

		errLabel:

			if (0!=iErr)
			{
				str=string.Format("%s, iIndex_sharedObj %d", _T("setFlg_forceKeyFrame failed"), iIndex_sharedObj);
				showInfo_open(0, null, str);
			}

			return iErr;

		}

	}
}
