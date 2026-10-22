using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int setROUTE_sendLocalAv(CCtxQmc pProcInfo, MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, bool ucbVideoConferenceStarter, ref ROUTE_sendLocalAv pRouteParam)
		{
			int iErr = -1;
			CQySyncObj syncObj;
			int i = 0;
			//ROUTE_sendLocalAv	*	pRoute		=		NULL;

			
			if (null==pProcInfo) return -1;
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;

			if (null==pMisCnt) return -1;    //  这里允许pIdInfo或者pIdInfo->ui64Id为0，表示将不是在处理dlgTalk.不需要发送数据.

			//pRoute  =  &pSharedObj->curRoute_sendLocalAv;

			//if (syncObj.sync(pQmcCfg.mutexName_syncSendAv)) goto errLabel;

			//mymemset((IntPtr)pRoute, 0, sizeof(ROUTE_sendLocalAv));
			fixed (ROUTE_sendLocalAv* pRoute = &pRouteParam)
			{
				mymemset((IntPtr)pRoute, 0, sizeof(ROUTE_sendLocalAv));


				//if (pIdInfo!=null)
				{
					if (ucbVideoConferenceStarter)
					{
						pRoute->videoConference_idInfo_to.ui64Id = pIdInfo.ui64Id;
					}
					else
					{
						pRoute->route.idInfo_to.ui64Id = pIdInfo.ui64Id;
						mytime(ref pRoute->routeInfo.tModifiedTime);                                           //  2009/09/11
					}
				}

				//  
				pRoute->route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;


				//  
				int tn = 0;
				pRoute->route.uiMsgRouteId = getuiNextTranNo(null, 0, ref tn);

			}


			iErr = 0;

		errLabel:

			if (0==iErr)
			{
			}
			return iErr;
		}


		//

		public static unsafe int addToMsgrs_sendLocalAv(CCtxQmc pProcInfo, MIS_CNT pMisCnt, QY_MESSENGER_ID idInfo, bool ucbVideoConferenceStarter, ref ROUTE_sendLocalAv route)
		{
			int iErr = -1;
			CQySyncObj syncObj;
			int i = 0;
			//ROUTE_sendLocalAv	*	pRoute		=		NULL;

			QY_MESSENGER_ID* pIdInfo = &idInfo;
			fixed (ROUTE_sendLocalAv* pRoute = &route)
			{



				if (null == pMisCnt || null == pIdInfo || 0 == pIdInfo->ui64Id) return -1;

				ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;


				//pRoute  =  &pSharedObj1->curRoute_sendLocalAv;

				//if (syncObj.sync(pQmcCfg->mutexName_syncSendAv)) goto errLabel;

				if (ucbVideoConferenceStarter)
				{
					if (0 == pRoute->videoConference_idInfo_to.ui64Id) pRoute->videoConference_idInfo_to.ui64Id = pIdInfo->ui64Id;
					else if (pRoute->videoConference_idInfo_to.ui64Id != pIdInfo->ui64Id)
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("addToMsgrs_sendLocalAv failed: ±¾µØÊÓÆµÒÑ¾­¼ÓÈëÊÓÆµ»áÒéÁË£¬Ö»ÄÜ¼ÓÈëÒ»¸ö."));
						goto errLabel;
					}
					//
				}
				else
				{
					if (pRoute->videoConference_idInfo_to.ui64Id == pIdInfo->ui64Id)
					{               //  ÒÑÔÚ·¢ËÍÕßÖÐÁË
						iErr = 0; goto errLabel;
					}

					if (pRoute->route.idInfo_to.ui64Id == pIdInfo->ui64Id)
					{                           //  ÒÑÔÚ·¢ËÍÕßÖÐÁË
						iErr = 0; goto errLabel;
					}
					for (i = 0; i < Consts.CONST_maxMsgrs_sendTo; i++)
					{
						if (pIdInfo->ui64Id == pRoute->route.mems_to_idInfo_ui64Id[i])
						{               //  ÒÑÔÚ·¢ËÍÕßÖÐÁË
							iErr = 0; goto errLabel;
						}
					}

					//  ÏÂÃæÒªÕÒ¸ö¿ÕÎ»ÖÃ´æ½øÈ¥
					if (0 == pRoute->route.idInfo_to.ui64Id)
					{                                               //  ÕÒµ½·¢ËÍµÄÎ»ÖÃ
						pRoute->route.idInfo_to.ui64Id = pIdInfo->ui64Id;
						mytime(ref pRoute->routeInfo.tModifiedTime);                                       //  2009/09/11
					}
					else
					{
						for (i = 0; i < Consts.CONST_maxMsgrs_sendTo; i++)
						{           //  
							if (0 == pRoute->route.mems_to_idInfo_ui64Id[i]) break;
						}
						if (i == Consts.CONST_maxMsgrs_sendTo)
						{
#if __DEBUG__
						traceLogA("¶¼ÂúÁË");
#endif
							goto errLabel;
						}
						pRoute->route.mems_to_idInfo_ui64Id[i] = pIdInfo->ui64Id;
						//mytime(&pRoute->routeInfo.mems[i].tModifiedTime);                           //  2009/
					}

				}

				//  ÉèÖÃ·¢ËÍÕß
				pRoute->route.idInfo_from.ui64Id = pMisCnt.idInfo.ui64Id;

				//  ÐÞ¸ÄÒ»ÏÂuiMsgRouteId. ±íÃ÷ÊÇÕâ¸ömsgRoute±ä»¯ÁË
				int tt = 0;
				pRoute->route.uiMsgRouteId = getuiNextTranNo(null, 0, ref tt);
			}

			iErr = 0;

		errLabel:

			if (0==iErr)
			{
			}
			return iErr;
		}

	}
}
