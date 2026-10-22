
#if false
using System;
using System.Collections.Generic;
using System.Text;
#endif
using File = Java.IO.File;

//
using Java.IO;

namespace qy.Droid
{
    partial class qyFuncs_Droid
{

    public static unsafe int getPolicyIsClient(CCtxQmc pProcInfo, out POLICY_isClient p)
    {
			string str;

            p = new POLICY_isClient();

			//
			File path;
			path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
			string str1 = path.AbsolutePath;
			fixed (char* pDir = p.dirToSaveFile)
			{
				qyFuncs.mylstrcpyn(pDir, str1, Consts.MAX_PATH + 1);
				qyFuncs_Droid.trailDir(pDir, Consts.MAX_PATH + 1);
				//
#if DEBUG
				str = new string(pDir);
				int ii = 0;
#endif

			}




			//
			p.usMax_nPlayers = Consts.DEFAULT_maxusCnt_players; //  64;
            p.usMax_nPlayers_gps = Consts.DEFAULT_maxusCnt_players_gps; //  2012/05/07

			//
			p.usTimeoutInMsToSwitch = Consts.DEFAULT_usTimeoutInMsToSwitch_isClient;                          //  2008/10/28, ¶ÔmediaºÍrealTimeMediaÍ¨µÀ£¬°üÔÚ daemon ÖÐµÄ¶ÁÈëºÍ·¢³ö£¬²»ÄÜ³¬¹ýÕâ¸öÊ±¼ä¡£·ñÔò£¬¶ªÆú¡£
			p.usTimeoutInMsToSwitch_rt = Consts.DEFAULT_usTimeoutInMsToSwitch_rt_isClient;                            //  2008/10/28, ¶ÔmediaºÍrealTimeMediaÍ¨µÀ£¬°üÔÚ daemon ÖÐµÄ¶ÁÈëºÍ·¢³ö£¬²»ÄÜ³¬¹ýÕâ¸öÊ±¼ä¡£·ñÔò£¬¶ªÆú¡£

			//
			//  2009/03/20
			p.video.uiCamCapType = (uint)getCamCapType(pProcInfo);
			p.audio.uiRecordType = (uint)getRecordType_default(pProcInfo);       //  2010/08/02



			return 0;
    }


		public static unsafe int getCamCapType(CCtxQmc pProcInfo)
        {
			return Consts.CONST_camCapType_android;
        }

		public static int getRecordType_default(CCtxQmc pProcInfo)
        {
			return Consts.CONST_recordType_android;
        }



		public static unsafe int getVideoConferenceCfg(object hKeyRoot0, string pRootKey_qnmScheduler, string misServName, QY_MESSENGER_ID pIdInfo, out VIDEO_CONFERENCE_CFG pCfg)
		{
			fixed(VIDEO_CONFERENCE_CFG * p = &pCfg)
            {
				qyFuncs.mymemset((System.IntPtr)p, 0, sizeof(VIDEO_CONFERENCE_CFG));

            }
			/*
			TCHAR tBuf[256] = _T("");

			if (!pRootKey_qnmScheduler || !pCfg) return -1;

			memset(pCfg, 0, sizeof(pCfg[0]));

		
			TCHAR* pRegVal;
			unsigned  long usCnt_monPics_row = 0;
			unsigned  long usCnt_monPics_col = 0;
			int nTotal = 0;
			unsigned  short usCntLimit_activeMems_from = 0;

			//
			pRegVal = _T(CONST_regValName_monPics_row);
			if (!qyGetRegCfgT(hKeyRoot0, pRootKey_qnmScheduler, pRegVal, (char*)tBuf, sizeof(tBuf), NULL))
			{
				usCnt_monPics_row = _ttol(tBuf);
			}
			pRegVal = _T(CONST_regValName_monPics_col);
			if (!qyGetRegCfgT(hKeyRoot0, pRootKey_qnmScheduler, pRegVal, (char*)tBuf, sizeof(tBuf), NULL))
			{
				usCnt_monPics_col = _ttol(tBuf);
			}
			usCnt_monPics_col = 1;  //  2008/11/01, ÒòÎª²¼¾ÖÏÖÔÚÊÇ¶¯Ì¬µÄ¡£ËùÒÔ£¬ÕâÀï¹Ì¶¨Îª1

			nTotal = usCnt_monPics_row * usCnt_monPics_col;
			if (!nTotal || nTotal > MAX_mems_taskAv)
			{
				//  
				usCnt_monPics_row = DEFAULT_mems_taskAv;    //  
				usCnt_monPics_col = 1;
				//
				nTotal = usCnt_monPics_row * usCnt_monPics_col;
			}

			//  2009/02/19
			usCntLimit_activeMems_from = DEFAULT_usCntLimit_activeMems_from;
			pRegVal = _T(CONST_regValName_usCntLimit_activeMems_from);
			if (!qyGetRegCfgT(hKeyRoot0, pRootKey_qnmScheduler, pRegVal, (char*)tBuf, sizeof(tBuf), NULL))
			{
				usCntLimit_activeMems_from = (unsigned  short  )_ttol(tBuf);
			}
			//  usCntLimit_activeMems_from  =  min(  nTotal,  min(  (  bLikeXp(  )  ?  MAX_activeMems_taskAv_xp  :  MAX_activeMems_taskAv  ),  usCntLimit_activeMems_from  )  );
			usCntLimit_activeMems_from = min(nTotal, min(MAX_activeMems_taskAv, usCntLimit_activeMems_from));


			//
			pCfg->iRows = (unsigned  short  )usCnt_monPics_row;
			pCfg->iCols = (unsigned  short  )usCnt_monPics_col;
			pCfg->usCntLimit_activeMems_from = usCntLimit_activeMems_from;


			//
			getAecCfg(hKeyRoot0, pRootKey_qnmScheduler, &pCfg->aecCfg);
			*/


			return 0;
		}

	}
}
