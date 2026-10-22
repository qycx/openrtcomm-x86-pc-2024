using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
   partial class qyFuncs
{
		//
		public static unsafe CUS_MODULE_compress getCusModule(CUS_MODULES pModules, uint uiModuleType, uint uiVDevId, uint uiDevType, string wDevIdStr)
		{
			int index = 0;
			//QY_MC			*	pQyMc	=	(  QY_MC  *  )pQyMcParam;
			CUS_MODULE_compress pModule = null;

			if (null==pModules) return null;
			if (null == pModules.mems) return null;

			//
			for (index = 0; index < pModules.mems.Length; index++)
			{
				pModule = pModules.mems[index];
				//
				if (pModule.common.uiType != uiModuleType) continue;

				//
				break;
			}

			if (index == pModules.mems.Length) return null;

			return pModule;
		}

	}
}
