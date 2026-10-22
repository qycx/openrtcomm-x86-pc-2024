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


		public static unsafe int startPlayVideo_android(QY_PLAYER pPlayerParam)
		{
			int iErr = -1;
			QY_PLAYER pPlayer = (QY_PLAYER)pPlayerParam;
			PLAY_VIDEO_procInfo pPlayVideo = pPlayer.video;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


			//
#if false
			qyFuncs.startQThread(mcThreadProc_playVideo, pPlayer, null, pPlayVideo);
#endif


			iErr = 0;
		errLabel:
			return iErr;
		}

		public static unsafe int stopPlayVideo_android(QY_PLAYER pPlayerParam)
		{
			int iErr = -1;
			QY_PLAYER pPlayer = (QY_PLAYER)pPlayerParam;
			PLAY_VIDEO_procInfo pPlayVideo = pPlayer.video;

			string str;

			//
			fixed (char* pName = pPlayer.video.q2.cfg.name)
			{
				str = string.Format("stopPlayVideo_android called, {0}", new string(pName));
			}
			qyFuncs.traceLog(str);


			//
			//qyFuncs.stopQThread(pPlayVideo, "");
			stopQThread_android(pPlayVideo, "");



			iErr = 0;
		errLabel:

			return iErr;
		}

	}
}