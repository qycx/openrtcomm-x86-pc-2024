using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace qy.Droid
{
    partial class qyFuncs_Droid
    {

        public const int DEFAULT_dwToInMs_playAudio = 200;

        //
        public static unsafe int initAudioPlayDev_android(void* pDynLib_dx, PLAY_AUDIO_procInfo pPlayAudio, uint dwBufferBytes, ref myPLAY_SOUND_dx p)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            if (null != p.m_playDev_android.m_audioTrack) return -1;

            //
            qyFuncs.traceLog("startPlayAudio_android called");

            //
            int SAMPLE_RATE_INHZ = (int)pPlayAudio.ah.wfx.nSamplesPerSec;
            ChannelOut channelConfig;
            Android.Media.Encoding AUDIO_FORMAT = Android.Media.Encoding.Pcm16bit;
            //
            int iRecorder_audioSessionId = AudioManager.AudioSessionIdGenerate;
            if (pProcInfo.av.localAv.recordSoundProcInfo.bCapDevConnected)
            {
                iRecorder_audioSessionId = pProcInfo.av.localAv.recordSoundProcInfo.iRecorder_audioSessionId;
            }


            //
            switch (pPlayAudio.ah.wfx.nChannels)
            {
                case 1:
                    channelConfig = ChannelOut.Mono;
                    break;
                case 2:
                    channelConfig = ChannelOut.Stereo;
                    break;
                default:
                    qyFuncs.traceLog("startPlayAudio_android failed, unsuported nChannels");
                    goto errLabel;
            }
            int minBufferSize = AudioTrack.GetMinBufferSize(SAMPLE_RATE_INHZ, channelConfig, AUDIO_FORMAT);
            p.m_playDev_android.m_audioTrack = new AudioTrack(
                new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.Media)
                    .SetContentType(AudioContentType.Music)
                    .Build(),
                new AudioFormat.Builder().SetSampleRate(SAMPLE_RATE_INHZ)
                    .SetEncoding(AUDIO_FORMAT)
                    .SetChannelMask(channelConfig)
                    .Build(),
                minBufferSize,
                AudioTrackMode.Stream,
                iRecorder_audioSessionId);
            AudioTrack audioTrack = (AudioTrack)p.m_playDev_android.m_audioTrack;
            audioTrack.Play();

            //
            p.minBufferSize = minBufferSize;


            iErr = 0;

        errLabel:
            if (iErr != 0)
            {
                exitAudioPlayDev_android(pDynLib_dx, pPlayAudio, ref p);
            }

            return iErr;
        }

        public static unsafe int exitAudioPlayDev_android(void* pDynLib_dx, PLAY_AUDIO_procInfo pPlayAudio, ref myPLAY_SOUND_dx p)
        {
            if (p.m_playDev_android.m_audioTrack == null) return 0;
            AudioTrack audioTrack = (AudioTrack)p.m_playDev_android.m_audioTrack;

            //
            try
            {
                audioTrack.Stop();
            }
            catch (IllegalStateException)
            {

            }

            audioTrack.Release();
            //
            p.m_playDev_android.m_audioTrack = null;


            return 0;
        }


        public static int getnWrite_toFillAudio(int nAvgBytesPerSec)
        {
            if (0 == (nAvgBytesPerSec % 100)) return nAvgBytesPerSec / 100 * 4;

            return 0;
        }

        public static int MACRO_getnWrite_toFillSilence(int nAvgBytesPerSec)
        {
            //
            if (0 == (nAvgBytesPerSec % 100)) return nAvgBytesPerSec / 100 * 4;
            //
            if (0 == (nAvgBytesPerSec % 10)) return nAvgBytesPerSec / 10;
            //
            return ((0 == (nAvgBytesPerSec % 5)) ? (nAvgBytesPerSec / 5) : nAvgBytesPerSec);
        }


        //public static unsafe int processOutput(void * pMgr, PLAY_AUDIO_procInfo pPlayAudio, myPLAY_SOUND_dx* p, bool bFillWithSilence, byte * pbInput1, uint dwNumBytes)
        public static unsafe int processOutput(void* pMgr, PLAY_AUDIO_procInfo pPlayAudio, ref myPLAY_SOUND_dx p, bool bFillWithSilence, byte[] pbInput1, ref int offsetInBytes, ref int nLeft, int nWrite, ref int nWritten_o)
        {
            int iErr = -1;

            AudioTrack audioTrack = (AudioTrack)p.m_playDev_android.m_audioTrack;

            //
            bool bDbg = false;
            string str;
#if DEBUG
            //bDbg = true;
#endif
            int tickCnt0 = 0;
            int tickCnt1 = 0;

            if (bDbg)
            {
                tickCnt0 = (int)qyFuncs.myGetTickCount(null);
            }

            //
            if (nLeft < nWrite)
            {
                qyFuncs.traceLog("processOutput failed, nLeft < nWrite");
                return -1;
            }

            int nWritten = 0;
            int n_toWrite = 0;
            for (; ; )
            {
                if (nWrite - nWritten < p.minBufferSize)
                {
                    if (0 != nWritten) break;
                    //
                    n_toWrite = nWrite;
                }
                else
                {
                    n_toWrite = p.minBufferSize;
                }


                //audioTrack.Write(pbInput1, offsetInBytes, n_toWrite, WriteMode.NonBlocking);
                audioTrack.Write(pbInput1, offsetInBytes, n_toWrite);

                p.dwMyWriteCursor += (uint)n_toWrite;
                //
                offsetInBytes += n_toWrite;
                nLeft -= n_toWrite;

                //
                nWritten += n_toWrite;
            }

            //
            nWritten_o = nWritten;

            iErr = 0;
        errLabel:

            if (bDbg)
            {
                tickCnt1 = (int)qyFuncs.myGetTickCount(null);
                str = string.Format("processOutput {0}ms, nWrite {1}, nWritten {2}", tickCnt1 - tickCnt0, nWrite, nWritten);
                qyFuncs.traceLog(str);
            }


            return iErr;
        }


        public static unsafe int fillSilence(QY_PLAYER pPlayer, int nWrite, byte[] pBuf, int offset_pLeft, ref int nLeft, bool bDbg)
        {

            if (offset_pLeft + nLeft + nWrite > pBuf.Length) return -1;
            for (int i = 0; i < nWrite; i++)
            {
                pBuf[offset_pLeft + nLeft + i] = 0;
            }
            nLeft += (int)nWrite;

            //
            //if (bDbg)
            {
                string str;
                str = string.Format("fillSilence: {0}: {1}bytes", pPlayer.idInfo_recorder.ui64Id, nWrite);
                qyFuncs.showInfo_open(0, null, str);
            }


            //
            return 0;
        }



        public static unsafe void mcThreadProc_playAudio(object lpParameter)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            bool bDbg = false;
            string str;
#if DEBUG
            bDbg = true;
#endif

            //
            int iErr = -1;


            QY_PLAYER pPlayer = null;
            PLAY_AUDIO_procInfo pPlayAudio = null;

            myPLAY_SOUND_dx myPs;
            void* pMgr = null;
            void* pDynLib = null;

            uint dwThreadId = qyFuncs.myGetCurrentThreadId();
            int i;
            int hr;
            int len;

            //
            uint dwTickCnt_start = 0;

            //

            //
            using (CQyMalloc mallocObj_tmp_pBuf = new CQyMalloc())
            {
                ;
                byte[] pBuf = null;
                uint uiBufSize;

                byte* tmp_pBuf = null;

                //byte* pLeft = null;                                 //  2009/04/27
                int offset_pLeft = 0;
                int nLeft = 0;

                uint uiSampleTimeInMs_pLeft = 0;                                       //  2009/05/04
                uint dwBytes_cur2Write = 0;

                uint nAvgBytesPerSec = 0;

                uint dwCurPlayCursor = 0;

                //
                CQyTick tickObj = new CQyTick();
                uint dwTickCnt_lastAudioData = qyFuncs.M_GetTickCount_audio(tickObj); ;                        // 

                //  TRANSFER_AUDIO_dataMem			pkt;
                //CQyMalloc						mallocObj_pPkt;
                //uint					uiBufSize_pPkt				=	offsetof(  myPLAY_AUDIO_DATA,  buf  )  +  pProcInfo.cfg.uiBufSize_myPlayAudioData;	//  sizeof(  myPLAY_AUDIO_DATA  );
                //int							iAllocType					=	0;	//  CONST_iAllocType_audio
                myPLAY_AUDIO_DATA tmp_pkt;
                myPLAY_AUDIO_DATA* pPkt = &tmp_pkt;//  (  myPLAY_AUDIO_DATA  *  )mallocObj_pPkt.mallocf(  iAllocType,  uiBufSize_pPkt  );
                if (null == pPkt) return;
                int pktLen;

                int nWrite = 0;
                int nWritten = 0;
                uint uiTimeInMs_nWrite;
                bool bNeedCheck_oldAudio = false;
                uint dwTickCnt_bNeedCheck_oldAudio = 0;    //  2015/07/03


                //

                uint dwToInMs = DEFAULT_dwToInMs_playAudio;
                uint iNotifyInterval;

                //qyFuncs.mymemset((IntPtr)(byte*)&myPs, 0, sizeof(myPLAY_SOUND_dx));
                myPs = new myPLAY_SOUND_dx();

                //
                pPlayer = (QY_PLAYER)lpParameter;
                if (pPlayer == null) return;
                pPlayAudio = pPlayer.audio;

                //AudioTrack audioTrack = (AudioTrack)pPlayAudio.m_playDev_android.m_audioTrack;
                //audioTrack.

                //
                //traceLogA("playSound %S enters", pPlayAudio.q2.cfg.name);
                fixed (char* pName = pPlayAudio.q2.cfg.name)
                {
                    str = string.Format("playAudio {0} enters", new string(pName));
                    qyFuncs.showInfo_open(0, "", str);
                }

                //
                nAvgBytesPerSec = pPlayer.audio.ah.wfx.nAvgBytesPerSec;
                if (0 == nAvgBytesPerSec)
                {
#if __DEBUG__
				traceLogA("mcThreadProc_playAudio failed: nAvgBytesPerSec is 0");
#endif
                    qyFuncs.showInfo_open(0, null, _T("playAudio failed: nAvgBytesPerSec is 0"));
                    goto errLabel;
                }

                int maxDataBufLen = 48000 * 2 * 3 / 2;
                if (pPlayer.audio.qBuf2.alloc(maxDataBufLen) != 0) goto errLabel;

                // Create the events. We make an extra one for later use 
                // by the output buffer.
                /*
                for  (  i  =  0;  i  <  mycountof(  pPlayAudio.hEvents  );  i  ++  )  {
                     pPlayAudio.hEvents[i] = CreateEvent( null, false, false, null );
                     if ( null == pPlayAudio.hEvents[i] ) goto  errLabel;
                }
                */

                //  fix playCfg. 2009/06/14
                fixPlayCfg(pProcInfo, ref pPlayAudio.playCfg);
                iNotifyInterval = pPlayAudio.playCfg.usNotifyIntervalInMs_needMoreAudioData;


#if false
				if (iNotifyInterval >= pPlayAudio.playCfg.usAudioDataAddedToPlayEveryTime || iNotifyInterval >= pPlayAudio.playCfg.usMinTimeInMsOfAudioDataPlaying)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, %I64u iNotifyInterval too big %d"), pPlayAudio.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id, iNotifyInterval);
				goto errLabel;
			}
#endif

                //
                //pPlayAudio.bQuit  =  false;
                pPlayAudio.dwTickCnt_start = qyFuncs.M_GetTickCount_audio(tickObj);        //  2010/05/04
                                                                                           //pPlayAudio.bRunning = true;
                pPlayer.uiSampleTimeInMs_start = 0;
                //memset(&pPlayAudio.m_var, 0, sizeof(pPlayAudio.m_var)  );
                pPlayAudio.m_var = new PLAY_AUDIO_procInfo_var();

                //
                /*
                if (bLocalAudio) qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, Local audio, starts"), pPlayAudio.q2.cfg.name);
                else qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, %I64u starts"), pPlayAudio.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id);
                */

                //  getMsg from q

                uiBufSize = System.Math.Max((Consts.bufSize_transferAudioData + 256) * 2, pPlayAudio.ah.wfx.nAvgBytesPerSec * 2);
                //pBuf = (byte*)mallocObj_pBuf.mallocf((int)uiBufSize);
                pBuf = new byte[uiBufSize];
                if (0 == uiBufSize || null == pBuf) goto errLabel;

                //
                tmp_pBuf = (byte*)mallocObj_tmp_pBuf.mallocf((int)uiBufSize);
                if (tmp_pBuf == null) goto errLabel;

                //
                //pLeft = pBuf; nLeft = 0;
                offset_pLeft = 0; nLeft = 0;

                //  runBuffers_play(  pMgr,  &myPs  );
                //  bPlayStarted  =  true;

                for (; !pPlayAudio.bQuit;)
                {

                    if (0 == dwToInMs || dwToInMs > DEFAULT_dwToInMs_playAudio)
                    {
                        dwToInMs = DEFAULT_dwToInMs_playAudio;
#if __DEBUG__
					//  traceLog(  _T(  "Warning: mcThreadProc_playAudio: dwToInMs %d"  ),  dwToInMs  );
#endif
                    }

                    uint dwTickCnt0 = qyFuncs.M_GetTickCount_audio(tickObj);
                    uint dwTickCnt;

                    //
                    if (bDbg)
                    {
                        //str = string.Format("playAudio:dwToInMs {0}, before waitOne", dwToInMs);
                        //qyFuncs.traceLog(str);
                    }

                    //
                    //qyFuncs.myWaitOne(pPlayAudio.q2.hEvent0, (int)dwToInMs);
                    //
                    dwTickCnt = qyFuncs.M_GetTickCount_audio(tickObj);

                    //  2013/05/26. 本地音频为了aec,需要一直保持. 
#if false
					{
					if (dwTickCnt - dwTickCnt_lastAudioData > (uint)pProcInfo.cfg.usTimeoutInMs_player_a)
					{
#if DEBUG
							str=string.Format("audio data may not arrives, player exits. timeout is {0}", pProcInfo.cfg.usTimeoutInMs_player_a);
							qyFuncs.showInfo_open(0,"",str);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("audio data may not arrives, player exits. timeout is %d"), pProcInfo.cfg.usTimeoutInMs_player_a);
						break;
					}
				}
#endif

                    //
                    bNeedCheck_oldAudio = false;

                    //  if  (  dwEvt  ==  0  )  
                    {

                        {

                            if (!myPs.bPlayInited || !myPs.bPlayStarted)
                            {
#if false

								dwToInMs = DEFAULT_dwToInMs_playAudio;

							len = (int)uiBufSize;
							//  if  (  qGetMsg(  &pPlayAudio.q,  pBuf,  (  uint  *  )&len  )  )  continue;
							pktLen = sizeof(myPLAY_AUDIO_DATA);
							{
								if (0 != qyFuncs.q2GetMsg((pPlayAudio.q2), (IntPtr)pPkt, (uint*)&pktLen)) continue;
							}
							if (IntPtr.Zero == pPkt->memory.m_pBuf)
							{
								qyFuncs.traceLogA("mcThreadProc_playAudio failed: 1. pPkt->pBuf is null");
								continue;
							}
							//
							//  len  =  min(  (  uint  )len,  pPkt->head.uiLen  );
							len = (int)Math.Min((uint)len, pPkt->uiSize_dec);     //  2016/12/14
																				  //
							if (0 != len)
							{
								//qyFuncs.mymemcpy((IntPtr)pBuf, pPkt->memory.m_pBuf, (uint)len);
								System.Runtime.InteropServices.Marshal.Copy((IntPtr)pPkt->memory.m_pBuf, pBuf, 0, len);
							}
							qyFuncs.clean_myPLAY_AUDIO_DATA((IntPtr)pPkt, _T("playAudio"));

#if DEBUG
#if true
								str=string.Format(  "playSound not inited: add audio data len {0}, size_dec {1}, bufSize {2}\n",  len,  pPkt->uiSize_dec,  uiBufSize  );
								qyFuncs.traceLog(str);
#endif
#endif

							if (0 == len)
							{
#if __DEBUG__
							  traceLogA(  "len can't be 0, error"  );  
#endif
								continue;
							}

							//
							//pLeft = pBuf; nLeft = (uint)len;
							offset_pLeft = 0; nLeft = (int)len;
#endif

                            }
                        }

                        if (!myPs.bPlayInited)
                        {

                            uint dwOffset_notify_unit = pPlayAudio.ah.wfx.nAvgBytesPerSec;

                            //qyFuncs.mymemset((IntPtr)(byte*)&myPs, 0, sizeof(myPLAY_SOUND_dx));


                            if (0 != initAudioPlayDev_android(pDynLib, pPlayAudio, dwOffset_notify_unit * 4, ref myPs)) goto errLabel;
                            myPs.bPlayInited = true;
                            dwTickCnt_start = qyFuncs.myGetTickCount(null);

                            //
                            myPs.dwOffset_notify_unit = dwOffset_notify_unit;
                            /*
                            myPs.dwNotifyCount = 2 * myPs.dsbDesc.dwBufferBytes / myPs.dwOffset_notify_unit;
                            myPs.lpdsbpns = (LPDSBPOSITIONNOTIFY)mymalloc(myPs.dwNotifyCount * sizeof(DSBPOSITIONNOTIFY));
                            if (!myPs.lpdsbpns) goto errLabel;
                            memset(myPs.lpdsbpns, 0, myPs.dwNotifyCount * sizeof(DSBPOSITIONNOTIFY));

                            for (i = 0; i < (int)myPs.dwNotifyCount; i++)
                            {
                                myPs.lpdsbpns[i].dwOffset = (i + 1) * myPs.dwOffset_notify_unit / 2 - 1;
                                myPs.lpdsbpns[i].hEventNotify = pPlayAudio.q2.hEvents[1];

                                traceLogA("lpdsbpns[%d].dwOffset %d,  dwBufferBytes %d", i, myPs.lpdsbpns[i].dwOffset, myPs.dsbDesc.dwBufferBytes);
                            }
                            */

                            //
                            if (0 == pPlayAudio.ah.wfx.nAvgBytesPerSec) goto errLabel;

                            //  
                            myPs.uiMaxMsgWaiting = (uint)(10);
#if __DEBUG__ //  for test
	//  traceLogA(  "Warning: myPs.usMaxMsgWaiting is set to 100 for test. 2009/04/27"  );
	//  myPs.uiMaxMsgWaiting  =  100;
#endif
                            if (0 == myPs.uiMaxMsgWaiting)
                            {
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, %I64u, uiMaxMsgWaiting error: %d."), pPlayAudio.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id, myPs.uiMaxMsgWaiting);
                                goto errLabel;
                            }

                        }

                        if (!myPs.bPlayStarted)
                        {
                            //
                            nWrite = MACRO_getnWrite_toFillSilence((int)myPs.dwOffset_notify_unit);
                            if (0 != fillSilence(pPlayer, nWrite, pBuf, offset_pLeft, ref nLeft, bDbg))
                            {
                                goto errLabel;
                            }
                            if (0 != processOutput(null, pPlayAudio, ref myPs, true, pBuf, ref offset_pLeft, ref nLeft, (int)nWrite, ref nWritten))
                            {
                                qyFuncs.traceLogA("playSound: processOutput failed, try to restart");
                                goto errLabel;
                            }
                            uiTimeInMs_nWrite = (uint)(nWritten * 1000 / nAvgBytesPerSec);
                            pPlayAudio.m_var.uiTimeInMs_audio_remains += uiTimeInMs_nWrite;


                            myPs.bPlayStarted = true;

                            //  continue;
                        }


                        //  continue;
                    }


                    if (!myPs.bPlayInited || !myPs.bPlayStarted)
                    {

                        dwToInMs = DEFAULT_dwToInMs_playAudio;

                        qyFuncs.traceLogA("event1, play not inited,  not started, or in silence. skipped");
                        continue;
                    }

                    /*
                    //  traceLogA(  "Playing sound event. dwMyWriteCursor %d, dwNotifyCount * dwOffset_notify_unit %d, dwBufferBytes %d",  myPs.dwMyWriteCursor,  myPs.dwNotifyCount  *  myPs.dwOffset_notify_unit,  myPs.dsbDesc.dwBufferBytes  );
                    if (myPs.dwMyWriteCursor >= myPs.dsbDesc.dwBufferBytes)
                    {
                        traceLogA("playSound: error: dwMyWriteCursor >= myPs.dsbDesc.dwBufferBytes.");
                        goto errLabel;
                    }

                    if (FAILED(hr = myPs.lpdsb->GetCurrentPosition(&dwCurPlayCursor, null)))
                    {
                        traceLogA("playSound: GetCurrentPosition failed.1");
                        goto errLabel;
                    }
                    */
                    uint dwTickCnt_GetCurrentPosition = qyFuncs.M_GetTickCount_audio(tickObj);

                    AudioTrack audioTrack = (AudioTrack)myPs.m_playDev_android.m_audioTrack;
                    dwCurPlayCursor = (uint)(dwTickCnt_GetCurrentPosition - dwTickCnt_start) * pPlayAudio.ah.wfx.nAvgBytesPerSec / 1000;


                    int iDiff_dwTickCnt_GetCurrentPosition = 0;
                    pPlayAudio.m_var.dwTickCnt_GetCurrentPosition = dwTickCnt_GetCurrentPosition;


                    //
                    /*
					if (myPs.dwMyWriteCursor < dwCurPlayCursor)
					{
							int iDiff = (int)(myPs.dwMyWriteCursor - dwCurPlayCursor);
							str = string.Format("playAudio_android failed, dwMyWriteCursof < dwCurPlayCursor. writeCursof - playCursor {0}", iDiff);
						qyFuncs.traceLog(str);
						goto errLabel;
					}
					*/

                    if (myPs.dwMyWriteCursor >= dwCurPlayCursor) dwBytes_cur2Write = myPs.dwMyWriteCursor - dwCurPlayCursor;
                    else
                    {
                        //dwBytes_cur2Write = myPs.dwMyWriteCursor + myPs.dwBufferBytes - dwCurPlayCursor;
                        dwBytes_cur2Write = 0;
                    }

                    //  2012/02/19
                    uint uiTimeInMs_audio_remains = dwBytes_cur2Write * 1000 / nAvgBytesPerSec;

#if __DEBUG__
#endif

                    //
#if false
					if (uiTimeInMs_audio_remains <= pPlayAudio.m_var.uiTimeInMs_audio_remains) pPlayAudio.m_var.uiTimeInMs_audio_remains = uiTimeInMs_audio_remains;
				else
				{
						//pPlayAudio.m_var.uiTimeInMs_audio_remains = 0;

						//
						str = string.Format("playAudio err: uiTimeInMs_audio_remains {0} > pPlayAudio.uiTimeInMs_audio_remains {1}", uiTimeInMs_audio_remains, pPlayAudio.m_var.uiTimeInMs_audio_remains);
						qyFuncs.qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), _T("IsClient"), null, _T(""), _T(""), str);
				}
#endif
                    pPlayAudio.m_var.uiTimeInMs_audio_remains = uiTimeInMs_audio_remains;



                    //
#if false
					if (pPlayAudio.m_var.uiTimeInMs_audio_remains >= pPlayAudio.playCfg.usMinTimeInMsOfAudioDataPlaying)
				{
					//
					if (bDbg)
					{
						str = string.Format("{0}: remains {1},continue", pPlayer.idInfo_recorder.ui64Id,  pPlayAudio.m_var.uiTimeInMs_audio_remains);
						qyFuncs.traceLog(str);
					}

					//
					dwToInMs = pPlayAudio.m_var.uiTimeInMs_audio_remains;
					if (dwToInMs <= iNotifyInterval)
					{
						qyFuncs.traceLogA("mcThreadProc_playAudio: minPlaying < iNotifyInterval");
						goto errLabel;
					}
					dwToInMs -= iNotifyInterval;
					continue;
				}
#endif


                    //
                    //  for  (  ;  nLeft  <  myPs.dwOffset_notify_unit;  )  
                    //  for  (  ;  nLeft  <  myPs.dwOffset_notify_unit  *  2  /  5;  )  
                    for (; nLeft < myPs.dwOffset_notify_unit * pPlayAudio.playCfg.usAudioDataAddedToPlayEveryTime / 1000;)
                    {
                        //if (pLeft != pBuf)
                        if (offset_pLeft != 0)
                        {
                            if (0 != nLeft)
                            {
                                //qyFuncs.mymemcpy((IntPtr)pBuf, (IntPtr)pLeft, nLeft);
                                Buffer.BlockCopy(pBuf, offset_pLeft, pBuf, 0, (int)nLeft);
                            }
                            //pLeft = pBuf;
                            offset_pLeft = 0;
                        }

                        //
                        int iTimeInMs_nLeft = 0;
                        iTimeInMs_nLeft = (int)(nLeft * 1000.0f / nAvgBytesPerSec);

                        //
                        len = (int)(uiBufSize - nLeft);
                        if (len <= 0)
                        {
                            //traceLogA("mcThreadproc_playSound failed: len %d < 0", len);
                            goto errLabel;
                        }
#if false
						//  if  (  qGetMsg(  &pPlayAudio.q,  pBuf  +  nLeft,  (  uint  *  )&len  )  )  break;
						pktLen = sizeof(myPLAY_AUDIO_DATA);
					{
						if (0 != qyFuncs.q2GetMsg(pPlayAudio.q2, (IntPtr)pPkt, (uint*)&pktLen)) break;
					}
					if (IntPtr.Zero == pPkt->memory.m_pBuf)
					{
						qyFuncs.traceLogA("mcThreadProc_playAudio failed: 2. pPkt->pBuf is null");
						break;
					}
					//
					if (bDbg)
					{
						int nQ2kk = qyFuncs.getQ2Nodes(pPlayAudio.q2);

						str = string.Format("playAudio: {0}: get a pkt, size_dec {1}. qNodes {2}", pPlayer.idInfo_recorder.ui64Id,  pPkt->uiSize_dec, nQ2kk);
						qyFuncs.traceLog(str);
					}
					//
					//  len  =  min(  (  uint  )len,  pPkt->head.uiLen  );
					len = (int)Math.Min((uint)len, pPkt->uiSize_dec);         //  2016/12/14
																			  //
																			  //qyFuncs.mymemcpy((IntPtr)(pBuf + nLeft), pPkt->memory.m_pBuf, (uint)len);
					System.Runtime.InteropServices.Marshal.Copy(pPkt->memory.m_pBuf, pBuf, (int)nLeft, len);
					qyFuncs.clean_myPLAY_AUDIO_DATA((IntPtr)pPkt, _T("playAudio"));
#endif
                        //
                        int len_suggest = getnWrite_toFillAudio((int)pPlayAudio.ah.wfx.nAvgBytesPerSec);
                        if (len_suggest == 0) goto errLabel;
                        //
                        len = System.Math.Min(len, len_suggest);

                        //
                        len = my_inStream(pPlayer, tmp_pBuf, len);
                        if (len == 0)
                        {
                            //
                            str = string.Format("{0}:doPlayAudio will quit, my_inStream returns 0", pPlayer.idInfo_recorder.ui64Id);
                            qyFuncs.showInfo_open(0, "", str);
                            //
                            //*pbQuit = true;
                            iErr = 0; goto errLabel;
                        }
                        System.Runtime.InteropServices.Marshal.Copy((IntPtr)tmp_pBuf, pBuf, (int)nLeft, len);

                        //
                        nLeft += (int)len;

                        //  2011/11/30
                        dwTickCnt_lastAudioData = dwTickCnt;        //  audio data received

                        //  pPlayer.audio.uiSampleTimeInMs  =  pkt.uiSampleTimeInMs;
                        if (0 != pPkt->head.uiSampleTimeInMs)
                        {
                            uiSampleTimeInMs_pLeft = (uint)((pPkt->head.uiSampleTimeInMs >= iTimeInMs_nLeft) ? (pPkt->head.uiSampleTimeInMs - iTimeInMs_nLeft) : 0);
#if __DEBUG__
	//  traceLogA(  "playAudio: iSampleTime_pLeft: %u. iTimeInMs_nLeft %d",  uiSampleTimeInMs_pLeft,  iTimeInMs_nLeft  );
#endif
                        }

#if __DEBUG__  //  2016/12/15
#endif


                    }



                    nWrite = 0;

                    if (0 != nLeft)
                    {

                        nWrite = (int)System.Math.Min(myPs.dwOffset_notify_unit, nLeft);
                        //
                        if (0 != processOutput(pMgr, pPlayAudio, ref myPs, false, pBuf, ref offset_pLeft, ref nLeft, (int)nWrite, ref nWritten))
                        {
                            qyFuncs.traceLogA("playSound: processOutput failed, try to restart");
                            goto errLabel;
                        }
                        uiTimeInMs_nWrite = (uint)(nWritten * 1000 / nAvgBytesPerSec);
                        pPlayAudio.m_var.uiTimeInMs_audio_remains += uiTimeInMs_nWrite;

                        //offset_pLeft += nWrite;
                        //nLeft -= (uint)nWrite;
                    }
                    else
                    {


                        pPlayer.audio.m_var.nTimes_noAudioData++;

                        if (pPlayer.audio.m_var.nTimes_noAudioData > 1)
                        {
                            pPlayer.audio.m_var.bVideoReady = false;                   //  this means to check video again
                        }

                        //  nWrite  =  myPs.dwOffset_notify_unit;
                        nWrite = MACRO_getnWrite_toFillSilence((int)myPs.dwOffset_notify_unit);
                        if (0 != fillSilence(pPlayer, nWrite, pBuf, offset_pLeft, ref nLeft, bDbg))
                        {
                            goto errLabel;
                        }
                        if (0 != processOutput(pMgr, pPlayAudio, ref myPs, true, pBuf, ref offset_pLeft, ref nLeft, (int)nWrite, ref nWritten))
                        {
                            qyFuncs.traceLogA("playSound: processOutput failed, try to restart");
                            goto errLabel;
                        }
                        uiTimeInMs_nWrite = (uint)(nWritten * 1000 / nAvgBytesPerSec);
                        pPlayAudio.m_var.uiTimeInMs_audio_remains += uiTimeInMs_nWrite;

                        dwToInMs += uiTimeInMs_nWrite;
#if false
						//  if  (  dwToInMs  >  CONST_notifyInterval  )  dwToInMs  -=  CONST_notifyInterval;
						if (dwToInMs <= iNotifyInterval)
						{
							qyFuncs.traceLogA("mcThreadProc_playAudio failed: fillSilence, dwToInMs <= iNotifyInterval");
							goto errLabel;
						}
						dwToInMs -= iNotifyInterval;
#endif

                        //
                        {
#if DEBUG
                            str = string.Format("playAudio: no enough audio data to play. fill silence, dwToInMs {0}. elapse_fromGetCurPos {1}, qNodes {2}", dwToInMs, 0, qyFuncs.getQ2Nodes_toGetMsg(pPlayAudio.q2));
                            qyFuncs.traceLog(str);
#endif
                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("%s, %I64u: no enough audio data to play. dwToInMs %d. fill silence, tickCnt %d"), pPlayAudio.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id, dwToInMs, M_GetTickCount_audio(tickObj));
                        }

                    }


                    //  2015/07/05. 不应该在wait后处理，因为这时候可能音频数据已经播放完了，所以remains要是总小于usMinTimeInMsOfAudioDataPlaying,就无法清理队列了，要等填完数据就处理比较好
                    //
                    if (pPlayAudio.m_var.uiTimeInMs_audio_remains >= pPlayAudio.playCfg.usMinTimeInMsOfAudioDataPlaying)
                    {
                        //  2015/07/03
                        if (!bNeedCheck_oldAudio)
                        {
                            //
                            int nQ2 = qyFuncs.getQ2Nodes_toGetMsg(pPlayAudio.q2);
                            //
                            if (nQ2 > 4)
                            {  //  2015/07/03
                                uint tmp_dwTickCnt = qyFuncs.myGetTickCount(null);
                                if (tmp_dwTickCnt - dwTickCnt_bNeedCheck_oldAudio > 1000)
                                {
                                    dwTickCnt_bNeedCheck_oldAudio = tmp_dwTickCnt;
                                    //
                                    bNeedCheck_oldAudio = true;
                                }
                            }
                        }
                        //
                        if (bNeedCheck_oldAudio)
                        {
                            if (0 != pPlayAudio.m_var.uiSampleTimeInMs && qyFuncs.getQ2Nodes_toGetMsg(pPlayAudio.q2) > 1)
                            {
                                /*
                                    uint tmpTickCnt_now = M_GetTickCount_audio(tickObj);
                                    int nWaitingInMs_audio = MACRO_getnWaitingInMs_audio_internal(pPlayer.audio, tmpTickCnt_now);
                                    if (pPlayAudio.m_var.uiSampleTimeInMs_newRecvd < pPlayAudio.m_var.uiSampleTimeInMs
                                        || pPlayAudio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd > pPlayAudio.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv + 1000
                                        || (pPlayer.audio.playCfg.usMaxTimeInMsToPlay && nWaitingInMs_audio > (int)(pPlayer.audio.playCfg.usMaxTimeInMsToPlay + pPlayer.audio.playCfg.usMinTimeInMsOfAudioDataPlaying))
                                        )
                                    {
    #if __DEBUG__
    #if true
                                      if  (  pPlayAudio.m_var.uiSampleTimeInMs_newRecvd  <  pPlayAudio.m_var.uiSampleTimeInMs  )  {
                                          traceLog(  _T(  "playSound: uiSampleTimeInMs_newRecvd %d, uiSampleTimeInMs %d"  ),  pPlayAudio.m_var.uiSampleTimeInMs_newRecvd,  pPlayAudio.m_var.uiSampleTimeInMs  );
                                      }
                                      if  (  pPlayAudio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd  >  pPlayAudio.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv  +  1000  )  {
                                          traceLog(  _T(  "playSound: dwTickCnt_newRecvd %d, dwTickCnt %d. SampleTimeInMs %d"  ),  pPlayAudio.m_var.dwTickCnt_uiSampleTimeInMs_newRecvd,  pPlayAudio.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv,  pPlayAudio.m_var.uiSampleTimeInMs  );
                                      }
                                      if  (  pPlayer.audio.playCfg.usMaxTimeInMsToPlay  &&  MACRO_getnWaitingInMs_audio_internal(  pPlayer.audio,  tmpTickCnt_now  )  >  pPlayer.audio.playCfg.usMaxTimeInMsToPlay  +  pPlayer.audio.playCfg.usMinTimeInMsOfAudioDataPlaying  )  {
                                          traceLog(  _T(  "playSound: nWaiting_audio %d > uiMaxTimeInMsToplay %d"  ),  MACRO_getnWaitingInMs_audio_internal(  pPlayer.audio,  tmpTickCnt_now  ),  pPlayer.audio.playCfg.usMaxTimeInMsToPlay  +  pPlayer.audio.playCfg.usMinTimeInMsOfAudioDataPlaying  );
                                      }								  
    #endif
    #endif

                                        CQyQ2Help q2Help;
                                        qRemoveMsg(q2Help.getQ_toGetMsg(&pPlayAudio.q2), (PF_bCommonHandler)tmpHandler_bOldAudio, (void*)tmpTickCnt_now, pPlayer);
                                    }
                                */
                            }
                        }

                    }




                    //
                    dwToInMs = pPlayAudio.m_var.uiTimeInMs_audio_remains;
                    //
                    int iElapseInMs_fromGetCurrentPosition = 0;
                    /*
					iElapseInMs_fromGetCurrentPosition = (int)(qyFuncs.myGetTickCount() - pPlayAudio.m_var.dwTickCnt_GetCurrentPosition);
					if (dwToInMs > iElapseInMs_fromGetCurrentPosition) dwToInMs -= (uint)iElapseInMs_fromGetCurrentPosition;
					else dwToInMs = 0;
					*/

#if false
					//  if  (  dwToInMs  >  MIN_dwToInMs  )  
					if (dwToInMs > iNotifyInterval)
				{

					if (0 != nWrite) pPlayer.audio.m_var.nTimes_noAudioData = 0;

					//  if  (  dwToInMs  >  CONST_notifyInterval  )  dwToInMs  -=  CONST_notifyInterval;
					dwToInMs -= iNotifyInterval;

					//
#if DEBUG
						if (bDbg)
						{
							str = string.Format("elapseInMs from getCurPos {0}", iElapseInMs_fromGetCurrentPosition);
							qyFuncs.traceLog(str);
						}
#endif


				}
				else
				{
					pPlayer.audio.m_var.nTimes_noAudioData++;

					if (pPlayer.audio.m_var.nTimes_noAudioData > 1)
					{
						pPlayer.audio.m_var.bVideoReady = false;                   //  this means to check video again
					}

					//  nWrite  =  myPs.dwOffset_notify_unit;
					nWrite = MACRO_getnWrite_toFillSilence((int)myPs.dwOffset_notify_unit);
					if (0 != fillSilence(nWrite, pBuf, offset_pLeft, ref nLeft, bDbg))
					{
						goto errLabel;
					}
					if (0 != processOutput(pMgr, pPlayAudio, ref myPs, true, pBuf, ref offset_pLeft, ref nLeft, (int)nWrite, ref nWritten))
					{
						qyFuncs.traceLogA("playSound: processOutput failed, try to restart");
						goto errLabel;
					}
					uiTimeInMs_nWrite = (uint)(nWritten * 1000 / nAvgBytesPerSec);
					pPlayAudio.m_var.uiTimeInMs_audio_remains += uiTimeInMs_nWrite;

					dwToInMs += uiTimeInMs_nWrite;
					//  if  (  dwToInMs  >  CONST_notifyInterval  )  dwToInMs  -=  CONST_notifyInterval;
					if (dwToInMs <= iNotifyInterval)
					{
						qyFuncs.traceLogA("mcThreadProc_playAudio failed: fillSilence, dwToInMs <= iNotifyInterval");
						goto errLabel;
					}
					dwToInMs -= iNotifyInterval;

					//
					{
#if DEBUG
							str=string.Format("playAudio: no enough audio data to play. fill silence, dwToInMs {0}. elapse_fromGetCurPos {1}, qNodes {2}", dwToInMs, iElapseInMs_fromGetCurrentPosition, qyFuncs.getQ2Nodes_toGetMsg(pPlayAudio.q2));
							qyFuncs.traceLog(str);
#endif
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("%s, %I64u: no enough audio data to play. dwToInMs %d. fill silence, tickCnt %d"), pPlayAudio.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id, dwToInMs, M_GetTickCount_audio(tickObj));
					}

				}
#endif

                    //  
                    if (0 != nWrite && 0 != uiSampleTimeInMs_pLeft)
                    {
                        int iTimeInMs_cur2Write = (int)(dwBytes_cur2Write * 1000.0f / nAvgBytesPerSec);
                        pPlayer.audio.m_var.uiSampleTimeInMs = uiSampleTimeInMs_pLeft;
                        pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_internal = (uint)(dwTickCnt + iTimeInMs_cur2Write);
                        pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_syncAv = (uint)(qyFuncs.M_GetTickCount_av(tickObj) + iTimeInMs_cur2Write);      //  2012/02/19
                        pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs_syncRecv = qyFuncs.myGetTickCount(null);

                        //
                        uiSampleTimeInMs_pLeft = 0;
                        //
                        //  

#if __DEBUG__
	//  traceLogA(  "playAudio: iSampleTimeInMs: %d, iTimeInMs_cur2Write %d",  pPlayer.audio.uiSampleTimeInMs,  iTimeInMs_cur2Write  );
	//  traceLogA(  "playAudio: iSampleTimeInMs_pLeft: %d, dwTickCnt %d",  pPlayer.audio.m_var.uiSampleTimeInMs,  pPlayer.audio.m_var.dwTickCnt_uiSampleTimeInMs  );
#endif
                        if (pProcInfo.cfg.debugStatusInfo.ucbShowPlayAudioStatus)
                        {   //  2015/05/24
                            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("playAudio: %I64u,%d: iSampleTimeInMs: %u, iTimeInMs_cur2Write %d, audio_remains %d"), pPlayer.idInfo_recorder.ui64Id, pPlayer.audio.uiTranNo, pPlayer.audio.m_var.uiSampleTimeInMs, iTimeInMs_cur2Write, pPlayer.audio.m_var.uiTimeInMs_audio_remains);
                        }
                    }


                    continue;
                }

            }

            qyFuncs.traceLogA("mcThreadProc_playAudio will quit");

        errLabel:

            if (myPs.bPlayInited)
            {
                exitAudioPlayDev_android(pDynLib, pPlayAudio, ref myPs);
                myPs.bPlayInited = false;
            }

            //
            pPlayAudio.qBuf2.free();

            //
            /*
            if (bLocalAudio) qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, Local audio, ends."), pPlayAudio.q2.cfg.name);
            else qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s, %I64u ends."), pPlayAudio.q2.cfg.name, pPlayer.idInfo_recorder.ui64Id);
            */

            //  if  (  pBuf  )  free(  pBuf  );

            pPlayAudio.bRunning = false;

            /*
            for  (  i  =  0;  i  <  mycountof(  pPlayAudio.hEvents  );  i  ++  )  {
                 if  (  pPlayAudio.hEvents[i]  )  {
                     CloseHandle(  pPlayAudio.hEvents[i]  );  pPlayAudio.hEvents[i]  =  null;
                 }
            }
            */

            fixed (char* pName = pPlayAudio.q2.cfg.name)
            {
                str = string.Format("playAudio {0}:{1}: leaves", new string(pName), pPlayer.idInfo_recorder.ui64Id);
                qyFuncs.showInfo_open(0, "", str);
            }

            return;
        }
    }


}


