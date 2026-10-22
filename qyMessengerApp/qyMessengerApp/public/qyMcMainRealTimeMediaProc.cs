using System;
using System.Collections.Generic;
using System.Text;

partial class Consts {
    // #define DEFAULT_usMaxCnt_taskInfos									500
    //public const int DEFAULT_usMaxCnt_taskInfos = 10;   //100;
#if DEBUG
    public const int DEFAULT_usMaxCnt_taskInfos = 5;   //100;
#else
    public const int DEFAULT_usMaxCnt_taskInfos = 5;   //100;
#endif

    //
    public const int DEFAULT_maxusCnt_players = 20;     //16;

    //  2012/05/07
    public const int DEFAULT_maxusCnt_players_gps = 10;

    public const int DEFAULT_usMaxCnt_pSharedObjs = 5;  //9;

    //
    public const int DEFAULT_usMaxCnt_androidThreads = 30;  //300;


    //  音量缓存，表明每次读取多少字节发送单元。这个是程序根据采样测试的结果自动调整。
    public const int MIN_uiBytesRecorded_perBlockAlign = 320;                       //  2009/05/11
    public const int MAX_uiBytesRecorded_perBlockAlign = 44100;

    public const int MIN_audio_uiSample = 8000;                 //  2014/05/24
    public const int MAX_audio_uiSample = 48000;                    //  2014/05/24

    //
    public const int DEFAULT_iHertz_pcm_av = 16000;     //  2013/05/25. aec's internal nSamples
    public const int DEFAULT_nChannels_pcm_av = 1;
    public const int DEFAULT_wBitsPerSample_pcm_av = 16;



}


namespace qy
{
    partial class qyFuncs
{
}
}
