using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
   partial class qyFuncs
{
		public unsafe struct TMP_idStr {
			public fixed byte idStr[Consts.CONST_qyMessengerIdStrLen + 1];
		};

		public static unsafe int storeTaskInDb(object pDb, int iDbType, MIS_CNT pMisCnt, int iTaskId_suggested, ref IM_TASK_RCD pRcd1, bool bInHis,string hint)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			string pTabName = bInHis ? Consts.CONST_tabName_qyImTaskHisTab : Consts.CONST_tabName_qyImTaskTab;
			//byte idStr_send[CONST_qyMessengerIdStrLen + 1] = "";
			TMP_idStr idStr_send;
			//
			QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
			if (null==pDbFuncs) goto errLabel;
			QM_dbFuncs  g_dbFuncs = pDbFuncs;

			string str;

			//
			if (hint == null) hint = "";

			//
			str = string.Format("storeTaskIdDb called, {0}", hint);
			qyFuncs.showInfo_open(0, null, str);

			//
			fixed (IM_TASK_RCD* pRcd = &pRcd1)
			{

				//
				using (CQySyncObj syncObj = new CQySyncObj())
				{

					if (null == idInfo2Str(&pRcd->idInfo_send, idStr_send.idStr, Consts.CONST_qyMessengerIdStrLen + 1)) goto errLabel;

					if (g_dbFuncs.pf_bSelectImTask_storeTaskInDb(pDb, iDbType, pMisCnt, pRcd, bInHis))
					{
						traceLog("storeTaskInDb: selectImTask true");
						//
						iErr = 1; goto errLabel;
					}
					//  
					if (!bInHis)
					{
						QY_MESSENGER_INFO messengerInfo = new QY_MESSENGER_INFO();
						QY_MESSENGER_REGINFO regInfo=new QY_MESSENGER_REGINFO();
						QY_MESSENGER_REGINFO regInfo1 = new QY_MESSENGER_REGINFO();
						QY_MESSENGER_phoneInfo phoneInfo = new QY_MESSENGER_phoneInfo();
						MY_REG_DESC desc;
						//
						if (!g_dbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pDb, iDbType, ref CONST_fieldIdTable_en, "", ref pRcd->idInfo_send, 0, ref messengerInfo, ref regInfo)) mymemset((IntPtr)(byte*)&regInfo, 0, sizeof(QY_MESSENGER_REGINFO));
						if (0 != regInfo2Desc(null, &regInfo, out desc, null, 0, null, 0)) goto errLabel;
						if (desc.pSyr.Length != 0)
						{
							//_sntprintf(pRcd->senderDesc, mycountof(pRcd->senderDesc), _T("%s - %s - %s"), desc.pDw, desc.pBm, desc.pSyr);
							str = string.Format("{0} - {1} -{2}", desc.pDw, desc.pBm, desc.pSyr);
							mytcsncpy(pRcd->senderDesc, str, Consts.cntof_senderDesc);
						}
						if (!g_dbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pDb, iDbType, ref CONST_fieldIdTable_en, "", ref pRcd->idInfo_recv, 0, ref messengerInfo, ref regInfo))
						{
							mymemset((IntPtr)(byte*)&regInfo, 0, sizeof(QY_MESSENGER_REGINFO));
						}
						if (0 != regInfo2Desc(null, &regInfo, out desc, null, 0, null, 0)) goto errLabel;
						if (desc.pSyr.Length != 0)
						{
							//_sntprintf(pRcd->receiverDesc, mycountof(pRcd->receiverDesc), _T("%s - %s - %s"), desc.pDw, desc.pBm, desc.pSyr);
							str = string.Format("{0} - {1} -{2}", desc.pDw, desc.pBm, desc.pSyr);
							mytcsncpy(pRcd->receiverDesc, str, Consts.cntof_receiverDesc);
						}
						//
						mytime(ref pRcd->firstTime);
						mytime(ref pRcd->lastTime);
					}

					//if (syncObj.sync(CQyString(pMisCnt.cfg.mutexNamePrefix_syncTask) + idStr_send)) goto errLabel;
					fixed (char* pName = pMisCnt.cfg.mutexNamePrefix_syncTask)
					{
						if (0 != syncObj.sync(new string(pName), pMisCnt.hMutex_syncTask)) goto errLabel;
					}
					if (0 != g_dbFuncs.pf_insertImTask_storeTaskInDb(pDb, iDbType, pMisCnt, iTaskId_suggested, pRcd, bInHis)) goto errLabel;
					if (!g_dbFuncs.pf_bSelectImTask_storeTaskInDb(pDb, iDbType, pMisCnt, pRcd, bInHis)) goto errLabel;
				}
			}


			iErr = 0;

		errLabel:

			if (iErr >= 0)
			{

			}
			return iErr;
		}

	}
}
