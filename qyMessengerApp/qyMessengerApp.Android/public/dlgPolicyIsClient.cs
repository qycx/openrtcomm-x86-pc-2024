using System;
using System.Collections.Generic;
using System.Text;



//
namespace qy.Droid
{
    partial class qyFuncs_Droid
{
			public const int CONST_uiMinTimeInMsOfAudioDataPlaying_05s = 150;
			public const int CONST_uiAudioDataAddedToPlayEveryTime_05s = 120;
			public const int CONST_uiNotifyIntervalInMs_needMoreAudioData_05s = 70;
			public const int CONST_uiMinTimeInMsToPlay_05s = 100;
			public const int CONST_uiMaxTimeInMsToPlay_05s = 150;
			public const int CONST_uiMaxTimeInMsToBeRemoved_05s = 120;

		public const int CONST_uiAudioDataAddedToPlayEveryTime_001s = 10;

			public const int DEFAULT_uiMinTimeInMsOfAudioDataPlaying = CONST_uiMinTimeInMsOfAudioDataPlaying_05s;
		public const int DEFAULT_uiAudioDataAddedToPlayEveryTime = CONST_uiAudioDataAddedToPlayEveryTime_001s;// CONST_uiAudioDataAddedToPlayEveryTime_05s;
			public const int DEFAULT_uiNotifyIntervalInMs_needMoreAudioData = CONST_uiNotifyIntervalInMs_needMoreAudioData_05s;
			public const int DEFAULT_uiMinTimeInMsToPlay = CONST_uiMinTimeInMsToPlay_05s;//  2012/01/27
			public const int DEFAULT_uiMaxTimeInMsToPlay = CONST_uiMaxTimeInMsToPlay_05s;
			public const int DEFAULT_uiMaxTimeInMsToBeRemoved = CONST_uiMaxTimeInMsToBeRemoved_05s;


		public static int initPolicyAvParams()  //  要在登陆后立即运行此函数. 2011/10/22
        {
            return 0;
        }




		public static unsafe int fixPlayCfg(CCtxQmc pProcInfo, ref AUDIO_PLAY_CFG pPlayCfg)
		{
			if (null==pProcInfo) return -1;
			//PolicyAvParams & gAvParams = *pProcInfo->p_gAvParams;   //  QY_GET_PolicyAvParams(  );

			int iNotifyInterval = 0;

			//if (pPlayCfg.usMinTimeInMsOfAudioDataPlaying < MIN_uiMinTimeInMsOfAudioDataPlaying || pPlayCfg.usMinTimeInMsOfAudioDataPlaying > MAX_uiMinTimeInMsOfAudioDataPlaying)
				pPlayCfg.usMinTimeInMsOfAudioDataPlaying = DEFAULT_uiMinTimeInMsOfAudioDataPlaying;
			//if (pPlayCfg.usAudioDataAddedToPlayEveryTime < MIN_uiAudioDataAddedToPlayEveryTime || pPlayCfg.usAudioDataAddedToPlayEveryTime > MAX_uiAudioDataAddedToPlayEveryTime)
				pPlayCfg.usAudioDataAddedToPlayEveryTime = DEFAULT_uiAudioDataAddedToPlayEveryTime;

			iNotifyInterval = pPlayCfg.usNotifyIntervalInMs_needMoreAudioData;
			//if (iNotifyInterval < MIN_uiNotifyIntervalInMs_needMoreAudioData || iNotifyInterval > MAX_uiNotifyIntervalInMs_needMoreAudioData)
				iNotifyInterval = DEFAULT_uiNotifyIntervalInMs_needMoreAudioData;
			pPlayCfg.usNotifyIntervalInMs_needMoreAudioData = (ushort)iNotifyInterval;

			if (0==pPlayCfg.usMinTimeInMsToPlay) pPlayCfg.usMinTimeInMsToPlay = DEFAULT_uiMinTimeInMsToPlay;            //  2012/01/27
			if (0==pPlayCfg.usMaxTimeInMsToPlay) pPlayCfg.usMaxTimeInMsToPlay = DEFAULT_uiMaxTimeInMsToPlay;
			if (0==pPlayCfg.usMaxTimeInMsToBeRemoved) pPlayCfg.usMaxTimeInMsToBeRemoved = DEFAULT_uiMaxTimeInMsToBeRemoved;

			return 0;
		}

		public static unsafe int myGetAvCompressorCfg(uint uiCapType, uint uiSubCapType, int iCapUsage, int level, ref AV_COMPRESSOR_CFG pCompressor)
		{
			int iErr = -1;
			/*
			QY_MC* pQyMc = QY_GET_GBUF();
			MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();
			PolicyAvParams & gAvParams = *pProcInfo->p_gAvParams;   //  QY_GET_PolicyAvParams(  );
			QY_REG reg;
			TCHAR* pRegVal = NULL;
			TCHAR tBuf[128] = _T("");
			TCHAR tRootKey_av[256] = _T("");
			TCHAR* pT = NULL;
			long lVal;

			int iResId = 0;
			//  BOOL  bConf  =  iCapUsage  ==  CONST_capUsage_conf;

			//
			switch (uiCapType)
			{
				case CONST_capType_av:
					switch (uiSubCapType)
					{
						case CONST_subCapType_webcam:
							iResId = gAvParams.iResId_policyAvLevelTable_webcam;
							break;
						default:
#if 0
							     if  (  bConf  )  {
									 iResId  =  gAvParams.iResId_policyAvLevelTable;
								 }
								 else  
#endif
							{
								iResId = gAvParams.iResId_policyAvLevelTable;
							}
							break;
					}
					break;
				case CONST_capType_screen:
					iResId = gAvParams.iResId_policyAvLevelTable_screen;
					break;
				case CONST_capType_mediaFile:
					iResId = gAvParams.iResId_policyAvLevelTable_mediaFile;
					break;
				case CONST_capType_mediaDevice:
					if (uiSubCapType == CONST_subCapType_unresizable)
					{
						iResId = gAvParams.iResId_policyAvLevelTable_unresizable;
						break;
					}
					iResId = gAvParams.iResId_policyAvLevelTable_mediaDevice;
					break;
				case CONST_capType_mosaic:
					if (uiSubCapType == CONST_subCapType_mosaic_video)
					{
						iResId = gAvParams.iResId_policyAvLevelTable_mosaic_video;
					}
					else if (uiSubCapType == CONST_subCapType_mosaic_resource)
					{
						iResId = gAvParams.iResId_policyAvLevelTable_mosaic_resource;
					}
					break;
				case CONST_capType_slave:  //  2016/04/06
					iResId = gAvParams.iResId_policyAvLevelTable_slave_video;
					break;
				case CONST_capType_qvcf:  //  2015/05/25
					break;

				default:
					_sntprintf(tBuf, mycountof(tBuf), _T("Err: myGetAvCompressorCfg failed, unprocessed capType %d"), uiCapType);
# ifdef  __DEBUG__
					traceLog(_T("%s"), tBuf);
#endif
					showInfo_open(0, 0, tBuf);
					goto errLabel;
					break;
			}

			//
			if (!level) level = getPolicyAvLevel(pQyMc, uiCapType, uiSubCapType, iCapUsage);
			if (!qyGetDesByType(getResTable(0, &pQyMc->cusRes, iResId), level)) level = getDefaultPolicyAvLevel(pQyMc, uiCapType, uiSubCapType, iCapUsage);

			//
			getCompressorCfg(NULL, uiCapType, uiSubCapType, iCapUsage, level, pCompressor);
			*/
			//qyFuncs.safeStrnCpy("h264", pCompressor.video.common.fourccStr, Consts.CONST_fourccStrLen + 1);
			//pCompressor.video.common.head.iFourcc = Consts.CONST_fourcc_h264;
			pCompressor.video.common.head.iFourcc = Consts.CONST_fourcc_HEVC;

			//
			pCompressor.video.common.head.ucbNeedKeyFrameFirst = 1;

            //
            pCompressor.video.common.head.usMaxFps_toShareBmp = 30;
            pCompressor.video.common.head.usMinFps_toShareBmp = 5;

            //
            switch (level)
			{
				case  Consts.CONST_policyAvLevel_480p:
					  pCompressor.video.common.head.iWidth_pic = 480;
					  pCompressor.video.common.head.iHeight_pic = 864;// 640;
					  pCompressor.video.common.head.iAvgBitrate = 600000;
					  break;
				case  Consts.CONST_policyAvLevel_720p:
                      pCompressor.video.common.head.iWidth_pic = 720;
                      pCompressor.video.common.head.iHeight_pic = 1280;// 640;
					  pCompressor.video.common.head.iAvgBitrate=1000000;
                      break;
                case  Consts.CONST_policyAvLevel_1080p:
                      pCompressor.video.common.head.iWidth_pic = 1080;
                      pCompressor.video.common.head.iHeight_pic = 1920;// 640;
					  pCompressor.video.common.head.iAvgBitrate = 1800000;
                      break;
                default:
                    pCompressor.video.common.head.iWidth_pic = 480;
                    pCompressor.video.common.head.iHeight_pic = 864;// 640;
					pCompressor.video.common.head.iAvgBitrate = 600000;
                    break;                    
			}			
			
			//
			pCompressor.video.common.head.ucCompressors = Consts.CONST_videoCompressors_ffmpeg;

			//
			get_pcm_forAndroid(uiCapType, ref pCompressor.audio.wf_decompress_default);

			//
			pCompressor.audio.wf_compress.wFormatTag = Consts.WAVE_FORMAT_MPEGLAYER3;
			pCompressor.audio.common.head.ucCompressors = Consts.CONST_audioCompressors_ffmpeg;



			//
			iErr = 0;
		errLabel:
			return iErr;
		}



	}
}
