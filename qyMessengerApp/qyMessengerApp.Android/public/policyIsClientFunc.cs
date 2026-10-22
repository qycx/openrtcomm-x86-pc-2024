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
		public static unsafe int MACRO_getuiBytesRecorded_perBlockAlign(ref AUDIO_COMPRESSOR_CFG  audioCompressor, ref WAVEFORMATEX wf_org)
		{
			/*
			 * MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();
			if (!pProcInfo) return -1;
			PolicyAvParams & gAvParams = *pProcInfo->p_gAvParams;   //  QY_GET_PolicyAvParams(  );
			*/
			fixed (WAVEFORMATEX* pWf_org = &wf_org)
			{

				//
				int uiBytesRecorded_perBlockAlign = -1;

				//  2013/03/09
				{
					//  2017/10/13
#if true
					if (0 == (pWf_org->nSamplesPerSec % 25))
					{
						uiBytesRecorded_perBlockAlign = (int)(pWf_org->nSamplesPerSec / 25);
						return uiBytesRecorded_perBlockAlign;
					}
#endif

					//
					if (0 == (pWf_org->nSamplesPerSec % 100))
					{
						uiBytesRecorded_perBlockAlign = (int)(pWf_org->nSamplesPerSec / 100 * 12);
						return uiBytesRecorded_perBlockAlign;
					}
				}

				//
				uiBytesRecorded_perBlockAlign = (int)pWf_org->nSamplesPerSec / 5;

				return uiBytesRecorded_perBlockAlign;
			}
		}

		public static unsafe int get_pcm_forAndroid(uint uiCapType, ref WAVEFORMATEX pWfx)
		{
			
			switch (uiCapType)
			{
				case Consts.CONST_capType_av:
					qyFuncs.makeWaveFormat_pcm(Consts.DEFAULT_nChannels_pcm_av, Consts.DEFAULT_iHertz_pcm_av, Consts.DEFAULT_wBitsPerSample_pcm_av, ref pWfx);
					break;
					/*
				case CONST_capType_mediaFile:
					makeWaveFormat_pcm(DEFAULT_nChannels_pcm_mediaFile, DEFAULT_iHertz_pcm_mediaFile, DEFAULT_wBitsPerSample_pcm_mediaFile, pWfx);
					break;
				case CONST_capType_mediaDevice:
					makeWaveFormat_pcm(DEFAULT_nChannels_pcm_mediaDevice, DEFAULT_iHertz_pcm_mediaDevice, DEFAULT_wBitsPerSample_pcm_mediaDevice, pWfx);
					break;
					*/
				default:
					break;
			}
			


			return 0;
		}

		public static unsafe int getPolicyAvLevel(CCtxQyMc pQyMc, uint uiCapType, uint uiSubCapType, int iCapUsage)
		{
			int iLevel = 0;

			if (null==pQyMc) return 0;

#if false
			//
			QY_REG reg;
			TCHAR* pRegVal;
			TCHAR tBuf[128] = _T("");

			QY_DMITEM* pTable = NULL;

			memset(&reg, 0, sizeof(reg));

			reg.hKeyRoot0 = HKEY_CURRENT_USER;
			getRegRootKey_qmc(uiCapType, uiSubCapType, iCapUsage, reg.rootKey, mycountof(reg.rootKey));

			//if  (  bReply  )  pRegVal  =  CONST_regValName_policyAvLevel_conf;
			//else  
			pRegVal = CONST_regValName_policyAvLevel_req;

			if (!qyGetRegCfgT(reg.hKeyRoot0, reg.rootKey, pRegVal, (char*)tBuf, sizeof(tBuf), 0)
				&& _ttol(tBuf))
			{
				iLevel = (unsigned char  )_ttol(tBuf);
			}

			//  2014/05/05
			if (!iLevel) iLevel = getDefaultPolicyAvLevel(pQyMc, uiCapType, uiSubCapType, iCapUsage);
#endif
            //iLevel = Consts.CONST_policyAvLevel_480p;
            iLevel = Consts.CONST_policyAvLevel_1080p;

			//
            return iLevel;

		}




		//
#if false
		QY_DMITEM CONST_conf_fourccTable[] =
		{
	{   CONST_fourcc_h264,  },
	{   CONST_fourcc_vp80,  },
	{   CONST_fourcc_HEVC,  },
	{   -1,                 },

};
#endif

		//
		public static int get_conf_usMaxSpeakers()
        {
			int usMaxSpeakers = Consts.DEFAULT_usMaxSpeakers;
			return usMaxSpeakers;
        }


		//
		public static int get_conf_iFourcc()
		{
#if false
			CCtxQyMc* pQyMc = QY_GET_GBUF();
			TCHAR tBuf[128];
			char buf[128];


			if (qyGetRegCfgT(HKEY_CURRENT_USER, pQyMc->cfg.pSysCfg->rootKey_qnmScheduler, _T(CONST_regValName_conf_fourcc), (char*)tBuf, sizeof(tBuf), NULL))
			{
				tBuf[0] = 0;
			}
			myTChar2Utf8(tBuf, buf, sizeof(buf));
			int iFourcc = fourccStr2i(buf);
			if (!qyGetDmItemByType(CONST_conf_fourccTable, iFourcc, sizeof(QY_DMITEM)))
			{
				//
				if (pQyMc->appParams.bConfServer) iFourcc = DEFAULT_conf_fourcc_confServ;
				else iFourcc = DEFAULT_conf_fourcc_others;
			}
#endif
			int iFourcc = Consts.DEFAULT_conf_fourcc_others;

			return iFourcc;
		}


		//
		public static unsafe int set_conf_iFourcc(int conf_iFourcc, ref AV_COMPRESSOR_CFG pCompressor)
		{
#if false
			QY_DMITEM* pItem;
			for (pItem = CONST_conf_fourccTable; pItem->type != -1; pItem++)
			{
				if (pItem->type == conf_iFourcc) break;
			}
			if (pItem->type == -1) return -1;
#endif

			//
			pCompressor.video.common.head.iFourcc = conf_iFourcc;
			switch (conf_iFourcc)
			{
#if false
				case Consts.CONST_fourcc_vp80:
					pCompressor.video.common.ucCompressors = Consts.CONST_videoCompressors_vp8;
					break;
#endif
				default:
					pCompressor.video.common.head.ucCompressors = Consts.CONST_videoCompressors_ffmpeg;
					break;
			}

			return 0;
		}



	}
}