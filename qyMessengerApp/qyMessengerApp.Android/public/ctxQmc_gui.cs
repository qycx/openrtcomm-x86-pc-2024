using System;
using System.Collections.Generic;
using System.Text;

namespace qy.Droid
{


	class CCtxQmc_gui : CCtxQmc
    {
        public CCtxQmc_gui()
        {

        }
        ~CCtxQmc_gui()
        {

        }

		public override unsafe int setQmDbFuncs(int iDbType, QM_dbFuncs pDbFuncs)
		{
			return  qyFuncs.setQmDbFuncs_qm(iDbType, pDbFuncs);
		}


		public static unsafe int newVar_isCli_gui(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2)
		{

			CCtxQmc p = null;

			p = new CCtxQmc_gui();

			if (null == p) return -1;

			pSci.pVar = p;

			return 0;
		}

		public static int freeVar_isCli_gui(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2)
		{

			if (pSci.pVar != null)
			{
				CCtxQmc p = (CCtxQmc)pSci.pVar;
				//delete p;
				pSci.pVar = null;
			}

			return 0;
		}

		
		public override unsafe int postMsg2Mgr_mc( CTX_caller ctxCaller, MIS_CNT pMIS_CNT, ref MSG_ROUTE pRoute, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, byte* data, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_dst, uint uiChannelType, CMIS_MSGU pMsgBuf, bool bLog)
		{
			return  qyFuncs.postMsg2Mgr_mc(ctxCaller, pMIS_CNT, ref pRoute, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, uiSeqNo, data, dataLen, pIdInfo_logicalPeer, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);
		}


		//
		/*
		int postMsgTask2Mgr_mc(void* pMIS_CNT, unsigned  int uiMisMsgType, unsigned  char ucFlg, unsigned  short usCode, time_t tStartTime, unsigned  int uiTranNo, unsigned  int uiSeqNo, int iTaskId, unsigned  int uiTaskType, char* data, unsigned  int dataLen, QY_MESSENGER_ID* pIdInfo_logicalPeer, QY_MESSENGER_ID* pIdInfo_taskSender, QY_MESSENGER_ID* pIdInfo_taskReceiver, QY_MESSENGER_ID* pIdInfo_dst, unsigned  int uiChannelType, MIS_MSGU* pMsgBuf, BOOL bLog)
		{
			return  ::postMsgTask2Mgr_mc(pMIS_CNT, uiMisMsgType, ucFlg, usCode, tStartTime, uiTranNo, uiSeqNo, iTaskId, uiTaskType, data, dataLen, pIdInfo_logicalPeer, pIdInfo_taskSender, pIdInfo_taskReceiver, pIdInfo_dst, uiChannelType, pMsgBuf, bLog);
		}
		*/


		public override unsafe int postImMsg2Log_isClient(ref MIS_MSGU pMsgU, int lenInBytes_msg)
		{
			fixed (MIS_MSGU* pMsg = &pMsgU)
			{
				return qyFuncs.postImMsg2Log_isClient(pMsg, lenInBytes_msg);
			}
		}



		public override unsafe int recoverMessenger(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QY_MESSENGER_INFO* pObj, QY_MESSENGER_REGINFO* pRegInfo, Int64 tLastModifiedTime, bool bLog, GENERIC_Q pLogQ)
		{
			return  qyFuncs.recoverMessenger(pDbFuncs, pDb, iDbType, ref pFieldIdTable, pObj, pRegInfo, tLastModifiedTime, bLog, pLogQ);
		}


		//
		/*
		 int recoverImObjRules(QM_dbFuncs* pDbFuncs, void* pDb, int iDbType, LPCTSTR misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRules_req* pReq, time_t tLastModifiedTime)
		{
			return  ::recoverImObjRules(pDbFuncs, pDb, iDbType, misServName, pIdInfo, pReq, tLastModifiedTime);
		}
		*/

		//
		public override unsafe int recoverImGrp(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, uint uiObjType, IM_GRP_INFO* pGrpInfo, Int64 tLastModifiedTime, bool bNoGrpName)
		{
			return  qyFuncs.recoverImGrp(pDbFuncs, pDb, iDbType, ref pFieldIdTable, uiObjType, pGrpInfo, tLastModifiedTime, bNoGrpName);
		}


		//
		public override unsafe int recoverImGrpMem(QM_dbFuncs pDbFuncs, object pDb, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime)
		{
			return  qyFuncs.recoverImGrpMem(pDbFuncs, pDb, iDbType, pGrpMem, tLastModifiedTime);
		}


		//
		public override unsafe int tryToTalkToMessenger_any(UInt64 ui64Id, bool bNeedNotShowWnd, bool bActivateWnd, ref object phWnd) 
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//if (GuiShare.pf_gui_findTalker(ui64Id, 0, ref phWnd) != 0) return -1;
			QY_MESSENGER_ID idInfo;idInfo.ui64Id = ui64Id;

			//
			if ( 0!=GuiShare.pf_findTalker(pQyMc, pMisCnt,  idInfo,ref  phWnd ) ) return -1;
			
			
			return 0; 
		}




	}

}
