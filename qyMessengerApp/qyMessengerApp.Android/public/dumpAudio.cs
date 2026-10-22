using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
	partial class qyFuncs_Droid
	{
		public static bool g_bInited_dumpA = false;



		public static unsafe int dumpAudio_init()
		{
			if (g_bInited_dumpA) return -1;

			//
			GuiShare.pf_dumpData1(IntPtr.Zero, 0, 0, 0, false, false, Consts.CONST_fn_dump_mp3_enc);

			//
			g_bInited_dumpA = true;

			return 0;
		}

		//
		public static unsafe int dumpAudio_exit()
		{
			if (!g_bInited_dumpA) return -1;


			g_bInited_dumpA = false;
			return 0;

		}


		public static unsafe int dumpAudio(IntPtr data, int size)
		{

			if (null == data || 0 == size)
			{
				return -1;
			}

			GuiShare.pf_dumpData1(data, size, 0, 0, true, false, Consts.CONST_fn_dump_mp3_enc);

			return 0;
		}


	}
}

