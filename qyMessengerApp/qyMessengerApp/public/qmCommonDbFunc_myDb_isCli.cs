using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
   partial class qyFuncs
{

		public static unsafe int tmpHandler_bNeedRetrieveImGrpMems_myDb(object p0, object p1, object p2)
		{
			int iErr = -1;
			//  p0
			//  p1
			QMEM_qyImObj* pMem = (QMEM_qyImObj*)(IntPtr)p2;

			if (pMem->messengerInfo.uiType == Consts.CONST_objType_imGrp
				&& pMem->messengerInfo.iStatus != Consts.CONST_qyStatus_del)
			{
				iErr = 1; goto errLabel;
			}

			iErr = 0;
		errLabel:
			return iErr;
		}

		public static unsafe bool bNeedRetrieveImGrpMems_myDb(object pDbParam, int iDbType, string misServName)
		{
			bool bRet = false;
			CMyDb pDb = (CMyDb)pDbParam;


			bool bImGrpExists = false;
			if (qTraverse(pDb.m_var_pQ_qyImObjTab, tmpHandler_bNeedRetrieveImGrpMems_myDb, 0, 0) > 0) bRet = true;

			return bRet;
		}


		public static unsafe int tmpHandle_bGetImTaskRcdBySth_myDb(object p0, object p1, object p2)
		{
			int iRet = -1;

			COMMON_PARAM pCommonParam = (COMMON_PARAM)p0;
			//
			QY_MESSENGER_ID* pIdInfo = (QY_MESSENGER_ID*)(IntPtr)pCommonParam.p0;
			uint uiTranNo = (uint  )(IntPtr)pCommonParam.p1;
			int id = (int)(IntPtr)pCommonParam.p2;
			//
			IM_TASK_RCD* pRcd = (IM_TASK_RCD*)(IntPtr)p1;
			IM_TASK_RCD* pQMem = (IM_TASK_RCD*)(IntPtr)p2;

			if (null!=pIdInfo && pIdInfo->ui64Id!=0)
			{
				if (pQMem->idInfo_send.ui64Id == pIdInfo->ui64Id
					&& pQMem->uiTranNo == uiTranNo)
				{
					if (pRcd!=null) mymemcpy((IntPtr)pRcd, (IntPtr)pQMem, (uint)sizeof(IM_TASK_RCD));
					iRet = 1; goto errLabel;
				}
			}
			else if (pQMem->id == id)
			{
				if (pRcd!=null) mymemcpy((IntPtr)pRcd, (IntPtr)pQMem, (uint)sizeof(IM_TASK_RCD));
				iRet = 1; goto errLabel;
			}

			iRet = 0;
		errLabel:
			return iRet;
		}

		//  2013/02/06
		public static unsafe bool bGetImTaskRcdBySth_myDb(object pDb, int iDbType, bool bHisTab, string misServName, QY_MESSENGER_ID pIdInfo, Int64 tSendTime, uint uiTranNo, int id, ref IM_TASK_RCD pRcd1)
		{
			bool bRet = false;
			CMyDb pMyDb = (CMyDb)pDb;
			COMMON_PARAM commonParam0=new COMMON_PARAM();

			if (bHisTab) goto errLabel;
			MACRO_makeCommonParam3((IntPtr)(byte*)&pIdInfo, (IntPtr)uiTranNo, (IntPtr)id, commonParam0);

			fixed (IM_TASK_RCD* pRcd = &pRcd1)
			{
				if (qTraverse(pMyDb.m_var_pQ_qyImTaskTab, tmpHandle_bGetImTaskRcdBySth_myDb, commonParam0, (IntPtr)pRcd) <= 0) goto errLabel;
			}

			//
			bRet = true;

		errLabel:

			return bRet;
		}


		////////////////////////////////////////////
		///

		public static unsafe int tmpHandle_selectImTask_storeTaskInDb_myDb(object p0, object p1, object p2)
		{
			int iRet = -1;

			//  p0
			IM_TASK_RCD* pRcd = (IM_TASK_RCD*)(IntPtr)p1;
			IM_TASK_RCD* pQMem = (IM_TASK_RCD*)(IntPtr)p2;

			//  2015/09/09
			//  在视频任务被自动中断，然后自动恢复时，初始的idInfo_recv可能是分组，但是后续重发的idInfo_recv是个人，所以，这里不能用idInfo_recv当判断任务的条件
#if DEBUG
			ref IM_TASK_RCD tmp_pRcd = ref *pRcd;
			ref IM_TASK_RCD tmp_pQMem = ref *pQMem;

#endif

			//
			if (pQMem->idInfo_send.ui64Id == pRcd->idInfo_send.ui64Id
				//&&  pQMem->idInfo_recv.ui64Id  ==  pRcd->idInfo_recv.ui64Id	//  2015/09/09. 
				&& pQMem->tSendTime == pRcd->tSendTime
				&& pQMem->uiTranNo == pRcd->uiTranNo)
			{
				pRcd->id = pQMem->id;
				pRcd->iStatus = pQMem->iStatus;

				iRet = 1; goto errLabel;
			}

			iRet = 0;
		errLabel:
			return iRet;
		}

		public static unsafe bool bSelectImTask_storeTaskInDb_myDb(object pDb, int iDbType, object pMisCntParam, IM_TASK_RCD* pRcd, bool bInHis)
		{
			bool bRet = false;
			CMyDb pMyDb = (CMyDb)pDb;

			if (bInHis) goto errLabel;

			if (qTraverse(pMyDb.m_var_pQ_qyImTaskTab, tmpHandle_selectImTask_storeTaskInDb_myDb, null, (IntPtr)pRcd) <= 0) goto errLabel;

			bRet = true;
		errLabel:

			return bRet;
		}




		public static unsafe int insertImTask_storeTaskInDb_myDb(object pDb, int iDbType, object pMisCntParam, int iTaskId_suggested, IM_TASK_RCD* pRcd, bool bInHis)
		{
			int iErr = -1;
			CMyDb pMyDb = (CMyDb)pDb;

			if (bInHis) return 0;
			if (null==pRcd) return -1;

			//  2013/02/05
			if (iTaskId_suggested!=0) pRcd->id = iTaskId_suggested;
			else
			{
				//  pRcd->id  =  getuiNextTranNo(  0,  0,  0  );
				pRcd->id = newTaskId(pDb, iDbType);
			}

			//
			if (0!=qPostMsg((IntPtr)pRcd, (uint)sizeof(IM_TASK_RCD), pMyDb.m_var_pQ_qyImTaskTab)) goto errLabel;


			iErr = 0;
		errLabel:
			return iErr;
		}

		public static unsafe int tmpHandler_updateTaskStatus_myDb(object p0, object p1, object p2)
		{
			int iRet = -1;

			int iStatus = (int)(IntPtr)p0;
			int iTaskId = (int)(IntPtr)p1;
			IM_TASK_RCD* pQMem = (IM_TASK_RCD*)(IntPtr)p2;

			if (pQMem->id == iTaskId)
			{
				pQMem->iStatus = iStatus;
				//  2014/05/01
				mytime(ref pQMem->tLastModifiedTime);
				//
				iRet = 1; goto errLabel;
			}

			iRet = 0;
		errLabel:

			return iRet;
		}

		//  2013/02/05
		public static unsafe int updateTaskStatus_myDb(object pDb, int iStatus, int iTaskId)
		{
			int iErr = -1;
			CMyDb pMyDb = (CMyDb)pDb;

			if (qTraverse(pMyDb.m_var_pQ_qyImTaskTab, tmpHandler_updateTaskStatus_myDb, (IntPtr)iStatus, (IntPtr)iTaskId) < 0) goto errLabel;


			iErr = 0;
		errLabel:

			return iErr;
		}





	}
}
