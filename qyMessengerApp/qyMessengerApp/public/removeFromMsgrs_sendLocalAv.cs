using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int removeFromMsgrs_sendLocalAv(CCtxQmc pProcInfo, MIS_CNT pMisCnt, ref QY_MESSENGER_ID pIdInfo, ref ROUTE_sendLocalAv pRoute)
		{
			int iErr = -1;
			int i = 0;
			//ROUTE_sendLocalAv	*	pRoute		=		NULL;


			if (0==pIdInfo.ui64Id) return -1;

			//QMC_cfg* pQmcCfg = (QMC_cfg*)pProcInfo->get_qmc_cfg();
			//if (!pQmcCfg) return -1;


			//pRoute  =  &pSharedObj1->curRoute_sendLocalAv;

			using (CQySyncObj syncObj = new CQySyncObj())
			{
				;
				if (0 != syncObj.sync("", pMisCnt.hMutex_syncSendAv)) goto errLabel;

				if (pRoute.videoConference_idInfo_to.ui64Id == pIdInfo.ui64Id)
				{
					pRoute.videoConference_idInfo_to.ui64Id = 0;
				}
				else
				{

					if (pRoute.route.idInfo_to.ui64Id == pIdInfo.ui64Id) pRoute.route.idInfo_to.ui64Id = 0;
					else
					{
						for (i = 0; i < Consts.CONST_maxMsgrs_sendTo; i++)
						{
							if (pIdInfo.ui64Id == pRoute.route.mems_to_idInfo_ui64Id[i])
							{
								pRoute.route.mems_to_idInfo_ui64Id[i] = 0;
								break;
							}
						}
					}

					if (0==pRoute.route.idInfo_to.ui64Id)
					{       //  如果idInfo_to为空,则从mems_to[]中提取一个到idInfo_to中
						for (i = 0; i < Consts.CONST_maxMsgrs_sendTo; i++)
						{
							if (pRoute.route.mems_to_idInfo_ui64Id[i]!=0)
							{
								pRoute.route.idInfo_to.ui64Id = pRoute.route.mems_to_idInfo_ui64Id[i];
								pRoute.route.mems_to_idInfo_ui64Id[i] = 0;
								//
								fixed (Int64* p = &pRoute.routeInfo.tModifiedTime)
								{
									mytime(ref *p);   //  2009/09/11
								}
								break;
							}
						}
					}

				}

				//  更新一下修改时间戳
				int tt = 0;
				pRoute.route.uiMsgRouteId = getuiNextTranNo(null, 0, ref tt);
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
