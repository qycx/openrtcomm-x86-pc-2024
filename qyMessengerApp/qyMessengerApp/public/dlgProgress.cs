using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static int notifyProgressEnd(uint uiTranNo, int nResult)
		{
			/*
			MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();
			if (!pProcInfo) return -1;

# ifdef  __DEBUG__  //  2014/09/05
			traceLog(_T("notifyProgressEnd called, tn %d, cur_progress.tn %d"), uiTranNo, pProcInfo->status.progress.uiTranNo);
#endif

			//
			if (pProcInfo->status.progress.uiTranNo == uiTranNo)
			{
				pProcInfo->status.progress.nResult = nResult;
				pProcInfo->status.progress.ucbProgessEnd = TRUE;
				if (IsWindow(pProcInfo->status.progress.hWnd))
				{
					PostMessage(pProcInfo->status.progress.hWnd, CONST_qyWm_postComm, CONST_qyWmParam_notifyProgressEnd, uiTranNo);
				}
			}
			*/

			return 0;
		}

	}
}
