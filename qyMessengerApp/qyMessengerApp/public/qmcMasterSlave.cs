using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		//
		public static  bool bExists_slave(CCtxQmc pProcInfo, int iIndex_sharedObj_master)
		{
			bool bRet = false;
			//  QY_MC					*	pQyMc					=	QY_GET_GBUF(  );
			//  MC_VAR_isCli			*	pProcInfo				=	QY_GET_procInfo_isCli(  );


			//
			QY_sharedObj_sync pSharedObjInfo_master = null;
			QY_SHARED_OBJ pSharedObj_master = null;
			pSharedObjInfo_master = getSharedObjSyncByIndex(pProcInfo, iIndex_sharedObj_master);
			if (null==pSharedObjInfo_master) goto errLabel;

			pSharedObj_master = pSharedObjInfo_master.sharedObj;
			if (!pSharedObj_master.bUsed) goto errLabel;

			if (pSharedObj_master.var.bSlave) goto errLabel;

			int iIndex_sharedObj_slave = pSharedObj_master.var.master.slaveKey.iIndex_sharedObj;

			//
			QY_sharedObj_sync pSharedObjInfo_slave = null;
			QY_SHARED_OBJ pSharedObj_slave = null;
			pSharedObjInfo_slave = getSharedObjSyncByIndex(pProcInfo, iIndex_sharedObj_slave);
			if (null==pSharedObjInfo_slave) goto errLabel;

			pSharedObj_slave = pSharedObjInfo_slave.sharedObj;
			if (!pSharedObj_slave.bUsed) goto errLabel;

			if (!pSharedObj_slave.var.bSlave) goto errLabel;

			//
			if (!isSlaveOfMaster(iIndex_sharedObj_slave, pSharedObj_slave, iIndex_sharedObj_master, pSharedObj_master))
			{
				goto errLabel;
			}

			bRet = true;


		errLabel:
			return bRet;
		}

		public static unsafe bool bExists_master(CCtxQmc pProcInfo, int iIndex_sharedObj_slave)
		{
			bool bRet = false;
			//  QY_MC						*	pQyMc					=	QY_GET_GBUF(  );
			//  MC_VAR_isCli				*	pProcInfo				=	QY_GET_procInfo_isCli(  );

			//
			QY_sharedObj_sync pSharedObjInfo_slave = null;
			QY_SHARED_OBJ pSharedObj_slave = null;
			pSharedObjInfo_slave = getSharedObjSyncByIndex(pProcInfo, iIndex_sharedObj_slave);
			if (null==pSharedObjInfo_slave) goto errLabel;

			pSharedObj_slave = pSharedObjInfo_slave.sharedObj;
			if (!pSharedObj_slave.bUsed) goto errLabel;

			if (!pSharedObj_slave.var.bSlave) goto errLabel;

			//
			int iIndex_sharedObj_master = pSharedObj_slave.var.slave.masterKey.iIndex_sharedObj;

			//
			QY_sharedObj_sync pSharedObjInfo_master = null;
			QY_SHARED_OBJ pSharedObj_master = null;
			pSharedObjInfo_master = getSharedObjSyncByIndex(pProcInfo, iIndex_sharedObj_master);
			if (null==pSharedObjInfo_master) goto errLabel;

			pSharedObj_master = pSharedObjInfo_master.sharedObj;
			if (!pSharedObj_master.bUsed) goto errLabel;

			if (pSharedObj_master.var.bSlave) goto errLabel;


			//
			if (!isSlaveOfMaster(iIndex_sharedObj_slave, pSharedObj_slave, iIndex_sharedObj_master, pSharedObj_master))
			{
				goto errLabel;
			}

			bRet = true;


		errLabel:
			return bRet;
		}


		public static unsafe int removeSlaveFromMaster(CCtxQmc pProcInfo, int iIndex_sharedObj_slave, int iIndex_sharedObj_master)
		{
			//
			int iErr = -1;
			//QY_MC						*	pQyMc					=	QY_GET_GBUF(  );
			//MC_VAR_isCli				*	pProcInfo				=	QY_GET_procInfo_isCli(  );


# if  __DEBUG__
			traceLog(_T("removeSlaveFromMaster"));
#endif

			bool bInited = false;
			uint tn_master = 0;
			uint tn_slave = 0;
			//
			CQySyncObj syncObj_master;
			CQySyncObj syncObj_slave;
			//
			QY_sharedObj_sync pSharedObjInfo_master = null;
			QY_SHARED_OBJ pSharedObj_master = null;
			pSharedObjInfo_master = getSharedObjSyncByIndex(pProcInfo, iIndex_sharedObj_master);
			if (null==pSharedObjInfo_master) goto errLabel;

			pSharedObj_master = pSharedObjInfo_master.sharedObj;

			//
			QY_sharedObj_sync pSharedObjInfo_slave = null;
			QY_SHARED_OBJ pSharedObj_slave = null;
			pSharedObjInfo_slave = getSharedObjSyncByIndex(pProcInfo, iIndex_sharedObj_slave);
			if (null==pSharedObjInfo_slave) goto errLabel;

			pSharedObj_slave = pSharedObjInfo_slave.sharedObj;

			//
			//MACRO_qyAssert(!pSharedObj_master->bSlave, _T("remoteSlaveFromMaster, master is not a master"));
			//MACRO_qyAssert(pSharedObj_slave->bSlave, _T("remoteSlaveFromMaster, slave is not a slave"));

			bInited = true;

			//
			//if (syncMtCnt_wLock_noWait(&pSharedObjInfo_master->syncMtCnt, null, &syncObj_master, &tn_master)) goto errLabel;
			//if (syncMtCnt_wLock_noWait(&pSharedObjInfo_slave->syncMtCnt, null, &syncObj_slave, &tn_slave)) goto errLabel;


			//
			if (isSlaveOfMaster(iIndex_sharedObj_slave, pSharedObj_slave, iIndex_sharedObj_master, pSharedObj_master))
			{
				//memset(&pSharedObj_master->master.slaveKey, 0, sizeof(pSharedObj_master->master.slaveKey)  );
				pSharedObj_master.var.master.slaveKey = new SHARED_obj_key();
				//memset(&pSharedObj_slave->slave.masterKey, 0, sizeof(pSharedObj_slave->slave.masterKey)  );
				pSharedObj_slave.var.slave.masterKey = new SHARED_obj_key();
			}

			iErr = 0;
		errLabel:
			if (bInited)
			{
				/*
				if (tn_master)
				{
					syncMtCnt_start(&pSharedObjInfo_master->syncMtCnt, getuiNextTranNo);
				}
				if (tn_slave)
				{
					syncMtCnt_start(&pSharedObjInfo_slave->syncMtCnt, getuiNextTranNo);
				}
				*/
			}
			return iErr;

		}

	}
}
