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
	//
	public unsafe  struct myPLAY_SOUND_dx
	{
		/*
		DSBUFFERDESC dsbDesc;
		LPDIRECTSOUNDBUFFER lpdsb;

		LPDIRECTSOUNDNOTIFY lpdsnOutput;
		*/
		public struct PlayADev_android
		{
			public object m_audioTrack;
		}
		public PlayADev_android m_playDev_android;

		//
		public int minBufferSize;

		//
		public uint dwOffset_notify_unit;
		public uint dwNotifyCount;
		//LPDSBPOSITIONNOTIFY lpdsbpns;

		//
		public uint dwMyWriteCursor;  //		=  0xFFFFFFFF;


		public uint uiMaxMsgWaiting;

		//
		public bool bPlayInited;
		public bool bPlayStarted;

	}
	;


	partial class qyFuncs_Droid
{
	public static unsafe int processInput_post(CCtxQmc pProcInfo, int nAvgBytesPerSec, byte* pbInput1, uint dwNumBytes, MIS_MSGU* pMsgBuf)
	{
		int iErr = -1;

		bool bHaveMsgrs_sendLocalAv = true;
		uint uiSampleTimeInMs = qyFuncs.mytimeGetTime();
		if (0==nAvgBytesPerSec)
		{
# if  __DEBUG__
			traceLog(_T("Attention: processInput_post: nAvgBytesPerSec not given here"));
#endif
		}
		else
		{
			int nDataInMs = (int)(dwNumBytes * 1000) / nAvgBytesPerSec;
			uiSampleTimeInMs -= (uint)nDataInMs;
# if  __DEBUG__
#endif
		}

		//
		{
				using (CQyMalloc mallocObj = new CQyMalloc())
				{
					;

					CAP_procInfo_audioCommon pCapAudio = (CAP_procInfo_audioCommon)pProcInfo.av.localAv.recordSoundProcInfo;

					myPLAY_AUDIO_DATA* p;
					int len;

					len = sizeof(myPLAY_AUDIO_DATA);


					myPLAY_AUDIO_DATA tmp_myPLAY_AUDIO_DATA;
					p = &tmp_myPLAY_AUDIO_DATA; //  (  myPLAY_AUDIO_DATA  *  )m_var.pBuf;
					if (null==p) goto errLabel;
					qyFuncs.mymemset((IntPtr)p, 0, len);

					//  2016/12/10
					uint uiSize_dec = dwNumBytes;

					//  2011/12/06		
					uint uiSize_enc = 0;   //  sizeof(  TRANSFER_AUDIO_DATA  );

					//
					//  p->head.uiLen  =  dwNumBytes;
					p->head.uiLen = uiSize_dec + uiSize_enc;    //  2016/12/10
					p->uiSize_dec = uiSize_dec;                 //  2016/12/10
																//
					p->head.uiSampleTimeInMs = uiSampleTimeInMs;
					//  
					if (null==mallocObj.mallocf((int)p->head.uiLen)) goto errLabel;
					if (0!=mallocObj.get(ref p->memory)) goto errLabel;
					//memcpy(  p->memory.m_pBuf,  pbInput1,  p->head.uiLen  );
					qyFuncs.mymemcpy(p->memory.m_pBuf, (IntPtr)pbInput1, p->uiSize_dec);  //  2016/12/14

					//
					if (0!=qyFuncs.q2PostMsgAndTrigger((IntPtr)p, (uint)len, pCapAudio.thread.q2)) goto errLabel;
					mallocObj.detach();
				}
		}


		//
		/*
		if (pProcInfo->av.localAv.recordSoundProcInfo.bLocalAudioPlaying)
		{
			//
# if  __DEBUG__
			//  traceLog(  _T(  "%d bytes audio data"  ),  dwNumBytes  );
#endif

			if (pProcInfo->av.localAv.player.audio.bRunning)
			{   //  if thread is active
				CQyMalloc mallocObj;

				//  qPostMsgAndTrigger_playSound(  pbInput1,  dwNumBytes,  &pProcInfo->av.localAv.playSoundProcInfo  );					
				//  TRANSFER_AUDIO_dataMem	pkt;
				myPLAY_AUDIO_DATA pkt;
				myPLAY_AUDIO_DATA* pPkt = &pkt;
				memset(pPkt, 0, sizeof(myPLAY_AUDIO_DATA));
				//  pkt.idInfo.ui64Id  =  0;
				//  pkt.uiSampleTimeInMs  =  (  uint  )(  SampleTime  *  1000  );
				pPkt->head.uiSampleTimeInMs = uiSampleTimeInMs;
				//
				//  pPkt->head.uiLen  =  min(  pProcInfo->cfg.uiBufSize_myPlayAudioData,  dwNumBytes  );

				//  2016/12/10			
				uint uiSize_dec = dwNumBytes;

				//  2011/12/06					
				uint uiSize_enc = 0;

				//			
				//  p->head.uiLen  =  dwNumBytes;
				pPkt->head.uiLen = uiSize_dec + uiSize_enc; //  2016/12/10			
				pPkt->uiSize_dec = uiSize_dec;                  //  2016/12/10

				//
#if  0
				pPkt->pBuf  =  mallocObj.malloc(  pPkt->head.uiLen  );
				if  (  !pPkt->pBuf  )  goto  errLabel;
#else
				if (!mallocObj.mallocf(pPkt->head.uiLen)) goto errLabel;
				if (mallocObj.get(&pPkt->memory)) goto errLabel;
#endif
				memcpy(pPkt->memory.m_pBuf, pbInput1, pPkt->uiSize_dec);

				//  2010/06/21
				pProcInfo->av.localAv.player.audio.m_var.uiSampleTimeInMs_newRecvd = pPkt->head.uiSampleTimeInMs;
				pProcInfo->av.localAv.player.audio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd = GetTickCount();

				//  
#if  0			//  2011/11/30, 不想采用触发机制了
				if  (  q2PostMsgAndTrigger(  pPkt,  offsetof(  myPLAY_AUDIO_DATA,  buf  ),  &pProcInfo->av.localAv.player.audio.q2  )  )  goto  errLabel;
#else
				if (q2PostMsg(pPkt, sizeof(myPLAY_AUDIO_DATA), &pProcInfo->av.localAv.player.audio.q2)) goto errLabel;
#endif
				mallocObj.detach();

				QY_MC* pQyMc = (QY_MC*)pProcInfo->pQyMc;
				//  if  (  !pQyMc->bUseDirectSound_toPlay  )  
				if (pQyMc->uiAPlayType == CONST_aPlayType_wave)
				{
					PostThreadMessage(pProcInfo->av.localAv.player.audio.dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);
				}

				qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("%d bytes audio data."), dwNumBytes);
			}
		}
		*/

		if (bHaveMsgrs_sendLocalAv || pProcInfo.av.localAv.recordSoundProcInfo.bLocalAudioPlaying) pProcInfo.av.localAv.recordSoundProcInfo.dwTickCnt_sendLocalAv = qyFuncs.myGetTickCount(null);
		else
		{
				/*

			if (GetTickCount() - pProcInfo->av.localAv.recordSoundProcInfo.dwTickCnt_sendLocalAv < 4000) qyShowInfo1(CONST_qyShowType_qwmComm, NULL, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Warning: no messenger need to be sent."));
			else
			{
				qyShowInfo1(CONST_qyShowType_qwmComm, NULL, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Warning: no messenger need to be sent, audio shall be stopped."));
				goto errLabel;
			}
				*/
		}


		iErr = 0;

	errLabel:

		return iErr;
	}

}
}