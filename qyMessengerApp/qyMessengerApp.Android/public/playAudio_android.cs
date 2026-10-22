using Android.App;
using Android.Content;
using Android.Media;
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
		public static unsafe int startPlayAudio_android(QY_PLAYER pPlayerParam)
		{
			int iErr = -1;
			QY_PLAYER pPlayer = (QY_PLAYER)pPlayerParam;
			PLAY_AUDIO_procInfo pPlayAudio = pPlayer.audio;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			string str;

			//
			if ( pProcInfo.cfg.mSpecialDbg.bNo_audioPlayer)
            {
				str = string.Format("for test, startPlayAudio_mediaCodec returns -1");
				qyFuncs.showInfo_open(0, null, str);
				//
				return -1;
            }

			//
			fixed (char* pName = pPlayer.audio.q2.cfg.name)
			{
				str = string.Format("startPlayAudio_android called, {0}", new string(pName));
			}
			qyFuncs.showInfo_open(0, null, str);

			//
			//
			pPlayAudio.cache.uiBufSize = Math.Max((Consts.bufSize_transferAudioData + 256) * 2, pPlayer.audio.ah.wfx.nAvgBytesPerSec * 2);
			pPlayAudio.cache.pBuf = (byte*)qyFuncs.mymalloc((int)pPlayAudio.cache.uiBufSize);
			if (0==pPlayAudio.cache.uiBufSize || null==pPlayAudio.cache.pBuf) goto errLabel;

			//
			pPlayAudio.cache.pLeft = pPlayAudio.cache.pBuf;
			pPlayAudio.cache.nLeft = 0;
			pPlayAudio.cache.dwTickCnt_lastAudioData = qyFuncs.myGetTickCount(null);


			//
			pPlayAudio.bRunning = true;

			//
			//qyFuncs.startQThread(mcThreadProc_playAudio, pPlayer, null, pPlayAudio);
			startQThread_android(mcThreadProc_playAudio, pPlayer, null, pPlayAudio);




			iErr = 0;
		errLabel:
			return iErr;
		}

		public static unsafe int stopPlayAudio_android(QY_PLAYER pPlayerParam)
		{
			int iErr = -1;
			QY_PLAYER pPlayer = (QY_PLAYER)pPlayerParam;
			PLAY_AUDIO_procInfo pPlayAudio = pPlayer.audio;
			string str;

			//
			//
			fixed (char* pName = pPlayer.audio.q2.cfg.name)
			{
				str = string.Format("stopPlayAudio_android called, {0}", new string(pName));
			}
			qyFuncs.showInfo_open(0,null,str);



			//
			//qyFuncs.stopQThread(pPlayAudio, "");
			stopQThread_android(pPlayAudio, "");

			//
			//
			IntPtr pBuf = (IntPtr)pPlayAudio.cache.pBuf;
			qyFuncs.MACRO_safeFree(ref pBuf);
			pPlayAudio.cache.pBuf = (byte*)pBuf;



			iErr = 0;
		errLabel:

			return iErr;
		}
	}

}


