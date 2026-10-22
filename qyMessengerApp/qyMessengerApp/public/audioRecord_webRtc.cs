using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

	partial class qyFuncs
	{
		//
		static bool g_bInited_webRtc = false;

		//
		public static int init_webRtc()
		{

			//
			if (!g_bInited_webRtc)
			{
				g_bInited_webRtc = true;
				//
				//voe_init();
			}

			//
			/*
			char* pVer = voe_ver();
			if (!pVer) pVer = "";
			TCHAR tBuf[128];
			_sntprintf(tBuf, mycountof(tBuf), _T("init_webRtc: ver %S"), pVer);
			showInfo_open(0, 0, tBuf);
			*/

			return 0;
		}

		//
		public static int exit_webRtc()
		{
			if (g_bInited_webRtc)
			{
				g_bInited_webRtc = false;
				//
				//voe_exit();
			}

			return 0;
		}

	}
}
