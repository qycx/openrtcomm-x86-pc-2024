using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int startPlayAudio_webRtc(QY_PLAYER pPlayerParam)
		{
			int iErr = -1;
			QY_PLAYER pPlayer = (QY_PLAYER)pPlayerParam;
			PLAY_AUDIO_procInfo pPlayAudio = pPlayer.audio;


			//
			pPlayAudio.cache.uiBufSize = Math.Max((Consts.bufSize_transferAudioData + 256) * 2, pPlayAudio.ah.wfx.nAvgBytesPerSec * 2);
			pPlayAudio.cache.pBuf = (byte*)mymalloc((int)pPlayAudio.cache.uiBufSize);
			if (0==pPlayAudio.cache.uiBufSize || null==pPlayAudio.cache.pBuf) goto errLabel;

			//
			pPlayAudio.cache.pLeft = pPlayAudio.cache.pBuf;
			pPlayAudio.cache.nLeft = 0;
			pPlayAudio.cache.dwTickCnt_lastAudioData = myGetTickCount(null);

			//
			pPlayAudio.dwTickCnt_start = M_GetTickCount_audio(null);        //  2010/05/04
			pPlayer.uiSampleTimeInMs_start = 0;
			fixed (PLAY_AUDIO_procInfo_var* pm_var = &pPlayAudio.m_var)
			{
				mymemset((IntPtr)(byte*)pm_var, 0, sizeof(PLAY_AUDIO_procInfo_var));
			}

			//  2018/06/16
			pPlayAudio.bNeedStop_webRtc_my_inStream = false;

			//
			pPlayAudio.bRunning = true;

			//
			/*
			if (voe_play_init(pPlayer, my_inStream, &pPlayAudio->webRtc.pVoePlay))
			{
				goto errLabel;
			}
			*/


			iErr = 0;
		errLabel:
			return iErr;
		}


		public static unsafe int stopPlayAudio_webRtc(QY_PLAYER pPlayerParam)
		{
			int iErr = -1;
			QY_PLAYER pPlayer = (QY_PLAYER)pPlayerParam;
			PLAY_AUDIO_procInfo pPlayAudio = pPlayer.audio;



			//
			//voe_play_exit(&pPlayAudio->webRtc.pVoePlay);

			//
			pPlayAudio.bRunning = false;

			//
			MACRO_mysafeFree(ref pPlayAudio.cache.pBuf);


			iErr = 0;
		errLabel:
			return iErr;


		}

	}
}
