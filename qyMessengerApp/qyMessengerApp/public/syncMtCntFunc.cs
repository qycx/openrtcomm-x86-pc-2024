using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
	public const string CONST_mtxName_syncMtCnt_wLock = ("syncMtCnt_wLock");

}

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int syncMtCnt_start(SYNC_mt_cnt pMtCnt, PF_getuiNextTranNo pf_getuiNextTranNo)
		{
			if (null==pf_getuiNextTranNo) return -1;

			int tt = 0;
			pMtCnt.ulTranNo = pf_getuiNextTranNo(null, 0, ref tt);
			if (0==pMtCnt.ulTranNo) pMtCnt.ulTranNo = pf_getuiNextTranNo(null, 0, ref tt);
			if (0==pMtCnt.ulTranNo) return -1;

			return 0;
		}

		public static unsafe int syncMtCnt_rLock(SYNC_mt_cnt pMtCnt, CQySyncCnt psyncCnt, string hint)
		{
			int iErr = -1;

			if (null==pMtCnt) return -1;
			if (null==psyncCnt) return -1;

			ulong tn_taskInfo = pMtCnt.ulTranNo;
			if (0==tn_taskInfo) goto errLabel;

			if (psyncCnt.sync(pMtCnt.lCnt_used, hint)!=0) goto errLabel;

			if (tn_taskInfo != pMtCnt.ulTranNo) goto errLabel;

			iErr = 0;

		errLabel:
			return iErr;
		}




		//  2015/08/31
		//
		static unsafe int tmpFunc_syncMtCnt_wLock(SYNC_mt_cnt pMtCnt)
		{
			CQySyncObj syncObj;

			//
			if (null == pMtCnt) return -1;
			//if  (  !ptn  )  return  -1;

			//
			pMtCnt.ulTranNo = 0;
			if (pMtCnt.lCnt_used[0]!=0) return -1;
			return 0;
		}


		//  2015/08/31
		//
		public static unsafe int syncMtCnt_wLock_noWait(SYNC_mt_cnt pMtCnt, string mutexName, Mutex hMutex, CQySyncObj pSyncObj, ulong* ptn)

		{
			//CQySyncObj	syncObj;

			//
			if (null==pMtCnt) return -1;
			if (null==pSyncObj) return -1;
			//if  (  !ptn  )  return  -1;

			//
			if (null==mutexName) mutexName = Consts.CONST_mtxName_syncMtCnt_wLock;  //  _T(  "syncMtCnt_wLock"  );

			if (pSyncObj.sync(mutexName,hMutex)!=0) return -1;

			//
			if (ptn!=null)
			{
				*ptn = pMtCnt.ulTranNo;
			}


			return tmpFunc_syncMtCnt_wLock(pMtCnt);
		}





		//
		public static unsafe int syncMtCnt_wLock_wait(SYNC_mt_cnt pMtCnt, string mutexName, Mutex hMutex, CQySyncObj pSyncObj, uint* ptn_unused, string hint)
		{
			int iErr = -1;

			if (null==pSyncObj) return -1;

			if (null==hint) hint = ("");


			//
			if (null==mutexName) mutexName = Consts.CONST_mtxName_syncMtCnt_wLock;  //  _T(  "syncMtCnt_wLock"  );

			if (pSyncObj.sync(mutexName,hMutex)!=0) return -1;


			//
			for (; ; )
			{
				if (0==tmpFunc_syncMtCnt_wLock(pMtCnt)) break;
				Thread.Sleep(50);
				//
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsCli"), 0, _T("syncMt"), _T(""), _T("%s"), hint);
			}

			iErr = 0;

			return iErr;
		}
	}


}
