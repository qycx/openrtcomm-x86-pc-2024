using Android.App;
using Android.Content;
using Android.Media;
using Android.Media.Audiofx;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    public class AudioCapDev_android
    {
        public AudioRecord m_recorder;
        public WAVEFORMATEX m_wfx_org;
        public byte[] data;
        public int minBufferSize;

        //
        public AcousticEchoCanceler m_aec;

        //
        public AudioCapDev_android()
        {
        }
    }

    partial class qyFuncs_Droid
    {
        public static unsafe int initAudioCapDev_android(object p_reserved, string displayName_selected, object ppCapStuff, ref AUDIO_COMPRESSOR_CFG pCompressor, bool bForConfig, object hWnd_notify, object lInstanceData, ref object ppAudioCapDevParam)
        {
            int iErr = -1;
            AudioCapDev_android pDev = new AudioCapDev_android();

            ppAudioCapDevParam = pDev;

            ChannelIn channelConfig;
            Android.Media.Encoding audioFormat;

            switch (pCompressor.wf_decompress_default.nChannels)
            {
                case 1:
                    channelConfig = ChannelIn.Mono;
                    break;
                default:
                    qyFuncs.traceLog("nChannels not supported");
                    goto errLabel;
            }
            switch (pCompressor.wf_decompress_default.wBitsPerSample)
            {
                case 16:
                    audioFormat = Android.Media.Encoding.Pcm16bit;
                    break;
                default:
                    qyFuncs.traceLog("wBits not supported");
                    goto errLabel;
            }

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            Activity mainWnd = (Activity)pQyMc.gui.hMainWnd;
            AudioManager mAudioManager = (AudioManager)mainWnd.GetSystemService(Context.AudioService);
            mAudioManager.Mode = Mode.InCommunication;


            //
            int minBufferSize = AudioRecord.GetMinBufferSize((int)pCompressor.wf_decompress_default.nSamplesPerSec, channelConfig, audioFormat);
            pDev.m_recorder = new AudioRecord(AudioSource.VoiceCommunication, (int)pCompressor.wf_decompress_default.nSamplesPerSec, channelConfig, audioFormat, minBufferSize);
            if (pDev.m_recorder == null) goto errLabel;

            pDev.m_wfx_org = pCompressor.wf_decompress_default;

            pDev.minBufferSize = minBufferSize;
            pDev.data = new byte[minBufferSize];

            //
            if (AcousticEchoCanceler.IsAvailable)
            {
                pDev.m_aec = AcousticEchoCanceler.Create(pDev.m_recorder.AudioSessionId);
                if (pDev.m_aec == null)
                {
                    //qyFuncs.showNotification(0,)

                    goto errLabel;
                }
                pDev.m_aec.SetEnabled(true);
            }




            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe void exitAudioCapDev_android(ref object ppAudioCapDevParam)
        {
            AudioCapDev_android pDev = (AudioCapDev_android)ppAudioCapDevParam;

            if (pDev != null)
            {
                if (pDev.m_aec != null)
                {
                    pDev.m_aec.SetEnabled(false);
                    pDev.m_aec.Release();
                    pDev.m_aec = null;
                }

                if (pDev.m_recorder != null)
                {
                    pDev.m_recorder.Release();
                    pDev.m_recorder = null;
                }
                pDev.data = null;
            }


        errLabel:

            //
            ppAudioCapDevParam = null;

            return;
        }


        //
        public static unsafe bool bGetCapturePara_audioCapdev_android(CCtxQmc pProcInfo, int iIndex_capAudio, uint uiBufSize_pMsgBuf, object pAudioCapDevParam, ref WAVEFORMATEX pWf_org, object pCache, ref int piRecorder_audioSessionId)
        {
            bool bRet = false;
            AudioCapDev_android pDev = (AudioCapDev_android)pAudioCapDevParam;
            AudioRecord record = (AudioRecord)pDev.m_recorder;

            pWf_org = pDev.m_wfx_org;
            piRecorder_audioSessionId = record.AudioSessionId;


            bRet = true;
            return bRet;

        }

        public static unsafe int startAudioCapDev_android(object pAudioCapDevParam)
        {
            int iErr = -1;
            AudioCapDev_android p = (AudioCapDev_android)pAudioCapDevParam;
            if (null == p) return -1;

            //
            qyFuncs.traceLog("startAudioCapDev_android enters");


            //
            try
            {
                p.m_recorder.StartRecording();
            }
            catch (IllegalAccessException e)
            {
                qyFuncs.showInfo_open(0, null, "startAudioCapDev_android,m_recorder.startRecording exception");
            }

            //
            qyFuncs.traceLog("startAudioCapDev_android leaves");

            //
            return 0;
        }

        public static unsafe int stopAudioCapDev_android(object pAudioCapDevParam)
        {
            int iErr = -1;
            AudioCapDev_android p = (AudioCapDev_android)pAudioCapDevParam;
            if (null == p) return -1;
            //if (!p->bInited) return -1;

            try
            {
                p.m_recorder.Stop();
            }
            catch (IllegalStateException e)
            {
                qyFuncs.showInfo_open(0, null, "stopAudioCapDev_android,m_recorder.stop exception");
            }

            return 0;
        }

        //
        public static unsafe int adjustAudio(float gain, byte* audio, int nBytes)
        {
            int ishort_MAX_VALUE = (1 << 15) - 1;
            int ishort_MIN_VALUE = (1 << 15);
            short short_MIN_VALUE = (short)ishort_MIN_VALUE;
            ishort_MIN_VALUE = short_MIN_VALUE;
#if DEBUG
            short sVal = (short)ishort_MAX_VALUE;
            short sVal1 = (short)ishort_MIN_VALUE;
#endif

            //
            if (nBytes % 2 != 0) return -1;

            //float gain = getGain(); // taken from the UI control, perhaps in range from 0.0 to 2.0
            //int numRead = read(audioData, 0, SIZE);
            int numRead = nBytes / 2;
            if (numRead > 0)
            {
                short* audioData = (short*)audio;
                for (int i = 0; i < numRead; ++i)
                {
                    int iVal = (int)audioData[i];
                    iVal = (int)(iVal * gain);
                    //
                    if (iVal < ishort_MIN_VALUE)
                    {
                        iVal = ishort_MIN_VALUE;
                    }
                    else if (iVal > ishort_MAX_VALUE)
                    {
                        iVal = ishort_MAX_VALUE;
                    }
                    //
                    audioData[i] = (short)iVal;
                }
            }

            return 0;
        }



        public static unsafe void recThreadFunc_android(object ignored)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return;
            CAP_procInfo_recordSound pRecordSoundProcInfo = pProcInfo.av.localAv.recordSoundProcInfo;
            QY_qThreadProcInfo_common pRecThread = pRecordSoundProcInfo.recThread;

            //
            AudioCapDev_android pDev = (AudioCapDev_android)pRecordSoundProcInfo.u_android.pAudioCapDev;

            bool bDbg = false;
            string str;

            //
            using (CQyMalloc mallocObj_pAudioDataBuf = new CQyMalloc())
            {
                ;
                byte* pAudioDataBuf = null;
                int dataLen_pAudioDataBuf = 0;

                using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
                {
                    ;
                    MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU));
                    if (null == pMsgBuf) return;

                    qyFuncs.traceLogA("recordSound enters");



                    //
                    //CStaticMediaBuffer mediaBuffer;
                    //const DWORD gBufLen = 32000;    //  16000 samples, 2bytes each
                    //BYTE gBuf[gBufLen];


                    ////////////////////////////

                    ref AUDIO_COMPRESSOR_CFG pCompressor = ref pRecordSoundProcInfo.compressAudio.compressor;





                    //
                    int bufSize_pAudioDataBuf = (int)pProcInfo.cfg.uiBufSize_myPlayAudioData;
                    if (null == (pAudioDataBuf = (byte*)mallocObj_pAudioDataBuf.mallocf(bufSize_pAudioDataBuf))) goto errLabel;

                    //
                    uint uiBytesRecorded_perBlockAlign = (uint)MACRO_getuiBytesRecorded_perBlockAlign(ref pCompressor, ref pDev.m_wfx_org);


                    int iCapTimeInMs = 40;
                    int nBytes_toRecord = (int)(uiBytesRecorded_perBlockAlign * pDev.m_wfx_org.nBlockAlign);
                    int nAvgBytesPerSec = (int)pDev.m_wfx_org.nAvgBytesPerSec;

                    if (0 == nBytes_toRecord) goto errLabel;
                    if (0 == nAvgBytesPerSec) goto errLabel;
                    if (iCapTimeInMs <= 10) goto errLabel;

                    //
                    //pRecThread.bQuit  =  FALSE;
                    pRecThread.bRunning = true;
                    pRecordSoundProcInfo.usErrors_processInput = 0;

                    //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("AEC record sound starts"));

                    //
                    //broadcastRecorderStatus(pQyMc);

                    //
                    int loopin = 0;
                    while (!pRecThread.bQuit)
                    {

                        //  DWORD  waitResult  =  WaitForSingleObject(  hRecEvent,  iCapTimeInMs  );
                        //qyFuncs.Sleep(iCapTimeInMs);



                        int read = pDev.m_recorder.Read(pDev.data, 0, pDev.minBufferSize);
                        if (bDbg)
                        {
                            str = string.Format("recorder.read {0} bytes", read);
                            qyFuncs.traceLog(str);
                        }
                        if (0 < read)
                        {

                            int dataLength = read;

                            //
#if _DEBUG
#endif

                            //
                            if (dataLength + dataLen_pAudioDataBuf < bufSize_pAudioDataBuf)
                            {
                                //memcpy(pAudioDataBuf + dataLen_pAudioDataBuf, gBuf, dataLength);
                                System.Runtime.InteropServices.Marshal.Copy(pDev.data, 0, (IntPtr)(pAudioDataBuf + dataLen_pAudioDataBuf), dataLength);
                                dataLen_pAudioDataBuf += dataLength;
                            }


                        }
                        else
                        {
                            pRecordSoundProcInfo.usErrors_processInput++;
                        }

                        //
                        //if (pRecordSoundProcInfo->bInputErr_aec != bInputErr_aec) pRecordSoundProcInfo->bInputErr_aec = bInputErr_aec;
                        //



                        //
#if _DEBUG
#endif


                        for (; ; )
                        {
                            if (dataLen_pAudioDataBuf < nBytes_toRecord) break;
                            //
                            float fG = 7.0f;
                            adjustAudio(fG, pAudioDataBuf, nBytes_toRecord);
                            //
                            processInput_post(pProcInfo, nAvgBytesPerSec, (byte*)pAudioDataBuf, (uint)nBytes_toRecord, pMsgBuf);
                            qyFuncs.mymemcpy((IntPtr)pAudioDataBuf, (IntPtr)(pAudioDataBuf + nBytes_toRecord), (uint)(dataLen_pAudioDataBuf - nBytes_toRecord));
                            dataLen_pAudioDataBuf -= nBytes_toRecord;
                        }

                    }

                }
            }

            iErr = 0;

        errLabel:

            //

            //
            pRecThread.bRunning = false;

            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("AEC record sound ends"));

            //broadcastRecorderStatus(pQyMc);

            return;

        }


    }
}



