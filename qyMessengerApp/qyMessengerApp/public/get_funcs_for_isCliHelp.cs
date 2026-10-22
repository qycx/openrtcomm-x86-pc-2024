using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static FUNCS_for_isCliHelp QY_GET_FUNCS_for_isCliHelp(CCtxQmc pProcInfo)
		{
			//  (  FUNCS_for_isCliHelp  *  )(  (  (  MC_VAR_isCli  *  )  QY_GET_procInfo_isCli(  )  )->pFuncs_for_isCliHelp  )
			if (null==pProcInfo) return null;
			return (FUNCS_for_isCliHelp)pProcInfo.pFuncs_for_isCliHelp;
		}

	}
}
