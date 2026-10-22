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

    
//
using Com.Qycx.Tmpffmpeglibrary;
using Java.Nio;


//
namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static int newCusModuleIndex()
        {
            int index = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;

            for (index = 0; index < pQyMc.cusModules.mems.Length; index++)
            {
                if (pQyMc.cusModules.mems[index] == null) break;
                continue;
            }

            if (index == pQyMc.cusModules.mems.Length) return -1;

            return index;

        }

        public static unsafe int loadCusModule_android()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CUS_MODULE_compress pModule = null;
            int index;

            //
            index = newCusModuleIndex();
            if (index < 0) return -1;

            //
            pQyMc.cusModules.mems[index] = new CUS_MODULE_compress();
            pQyMc.cusModules.mems[index].common.uiType = Consts.CONST_moduleType_ffmpeg;

            //
            pModule = pQyMc.cusModules.mems[index];

            //
            pModule.common.bLoaded = true;

            //
            pModule.pf_qdcInit = qdcInit_ffmpeg;
            pModule.pf_qdcExit = qdcExit_ffmpeg;


            pModule.pf_qdcInitCompressVideo = qdcInitCompressVideo_ffmpeg;
            pModule.pf_qdcExitCompressVideo = qdcExitCompressVideo_ffmpeg;
            pModule.pf_qdcGetSpsPps = qdcGetSpsPps_ffmpeg;
            pModule.pf_qdcCompressVideo = qdcCompressVideo_ffmpeg;
            //
            pModule.pf_qdcInitDecompressVideo = qdcInitDecompressVideo_ffmpeg;
            pModule.pf_qdcExitDecompressVideo = qdcExitDecompressVideo_ffmpeg;
            pModule.pf_qdcDecompressVideo = qdcDecompressVideo_ffmpeg;
            //
            pModule.pf_qdcGetOutputByIndex = qdcGetOutputByIndex_ffmpeg;
            pModule.pf_qdcClearOutputs = qdcClearOutputs_ffmpeg;

            //
            pModule.pf_qdcInitCompressAudio=qdcInitCompressAudio_ffmpeg;
            pModule.pf_qdcExitCompressAudio=qdcExitCompressAudio_ffmpeg;
            pModule.pf_qdcCompressAudio=qdcCompressAudio_ffmpeg;
            pModule.pf_qdcCompressAudioRun=qdcCompressAudioRun_ffmpeg;                      //  2016/12/05

            //
            pModule.pf_qdcInitDecompressAudio=qdcInitDecompressAudio_ffmpeg;
            pModule.pf_qdcExitDecompressAudio=qdcExitDecompressAudio_ffmpeg;
            pModule.pf_qdcDecompressAudio=qdcDecompressAudio_ffmpeg;
            pModule.pf_qdcDecompressAudioRun=qdcDecompressAudioRun_ffmpeg;                  //  2016/12/05


            //
            //
            index = newCusModuleIndex();
            if (index < 0) return -1;

            //
            pQyMc.cusModules.mems[index] = new CUS_MODULE_compress();
            pQyMc.cusModules.mems[index].common.uiType = Consts.CONST_moduleType_mediaCodec;

            //
            pModule = pQyMc.cusModules.mems[index];

            //
            pModule.common.bLoaded = true;

            //
            pModule.pf_qdcInit = qdcInit_mediaCodec;
            pModule.pf_qdcExit = qdcExit_mediaCodec;

            //
            pModule.pf_qdcInitDecompressVideo = qdcInitDecompressVideo_mediaCodec;
            pModule.pf_qdcExitDecompressVideo = qdcExitDecompressVideo_mediaCodec;
            pModule.pf_qdcDecompressVideo = qdcDecompressVideo_mediaCodec;
            pModule.pf_qdcDecompressVideoRun = qdcDecompressVideoRun_mediaCodec;
            //
            pModule.pf_qdcInitCompressVideo = qdcInitCompressVideo_mediaCodec;
            pModule.pf_qdcExitCompressVideo = qdcExitCompressVideo_mediaCodec;
            pModule.pf_qdcGetSpsPps = qdcGetSpsPps_mediaCodec;
            pModule.pf_qdcCompressVideoRun = qdcCompressVideoRun_mediaCodec;
            //
            //
            pModule.pf_qdcInitDecompressAudio = qdcInitDecompressAudio_mediaCodec;
            pModule.pf_qdcExitDecompressAudio = qdcExitDecompressAudio_mediaCodec;
            pModule.pf_qdcDecompressAudio = qdcDecompressAudio_mediaCodec;
            pModule.pf_qdcDecompressAudioRun = qdcDecompressAudioRun_mediaCodec;                  //  2016/12/05

            //
            iErr = 0;

            return iErr;

        }

        

    }
}