
#if false
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
#endif


//
using Java.IO;

namespace qy.Droid
{
    public partial class qyFuncs_Droid
    {


		///////////////////
		///
public static unsafe bool InitInstance(System.IntPtr hInstance, int nCmdShow)
		{
			bool bRet = false;
			string str;

			//
			//set_cur_iResId_sys(CONST_resId_sys_isCli_ts);


			//  2015/07/08
			g.g_pQyMc = new CCtxQyMc();
			if (g.g_pQyMc == null) goto errLabel;

			//
			GuiShare.pf_getSystmInfo(ref g.g_pQyMc.iManufacturer, ref g.g_pQyMc.iModel, ref g.g_pQyMc.b_isHarmony);

			//  2016/08/12
			PARAM_initQyMc param = new PARAM_initQyMc();
			//memset(&param, 0, sizeof(param));
			param.pfNewVar = CCtxQmc_gui.newVar_isCli_gui;
			param.pfFreeVar = CCtxQmc_gui.freeVar_isCli_gui;
			//
			if (qyFuncs.initQyMc((System.IntPtr)null, param, g.g_pQyMc) != 0) goto errLabel;

			//
			CCtxQmc pProcInfo = g.g_pQyMc.get_pProcInfo();
			//pProcInfo.cfg.policy.dirToSaveFile
			//g.g_pQyMc.cfg.




			//
			bRet = true;
		errLabel:

			return bRet;
		}

		public static int ExitInstance(System.IntPtr hInstance)
		{
			int iErr = -1;


			//
			qyFuncs.exitQyMc(g.g_pQyMc);
			//  2015/07/08
			if (g.g_pQyMc != null)
			{
				//QY_MC* pQyMc = (QY_MC*)g_pQyMc;
				//MACRO_safeDelete(pQyMc);
				g.g_pQyMc = null;
			}


			//
			iErr = 0;
		errLabel:
			return iErr;
		}


	}
}