using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy
{
	partial class qyFuncs
{
		public static unsafe  string getResStr(object p0, object pQnmCusResInfo, int iResId)
		{
			string p = null;
			/*
			TCHAR* p = NULL;

			QNM_CUSRES_INFO* pInfo = (QNM_CUSRES_INFO*)pQnmCusResInfo;
			//
			if (!pInfo->hDll) goto errLabel;
			//
			QNM_cusRes_moduleMem* pResMem = getCurCusResModuleMem(pInfo);
			if (!pResMem) goto errLabel;

			if (pInfo)
			{
				p = (TCHAR*)qyGetDesByType(pResMem->pCusStrTable, iResId);
			}
			*/
			p= qyGetDesByType1(CONST_strTable_ts, iResId);

		errLabel:

			return p!=null ? p : _T("");
		}

	}
}