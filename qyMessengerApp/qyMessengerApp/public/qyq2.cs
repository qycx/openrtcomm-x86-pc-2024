using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

static partial class Consts
{
	public const int cntof_q2Cfg_name = 16;
	public const int cntof_q2Cfg_evt0Name = 24;

	//
	public const int CONST_uiInitCnt_sema_q2SyncFlg = 20;									//  2009/05/13

}


namespace qy
{


	public unsafe struct QY_Q2_cfg
	{
		public fixed char name[Consts.cntof_q2Cfg_name];
		public uint uiMaxQNodes;

		public RW_lock_param rwLockParam_syncFlg;

		public uint uiMaxCnt_semaTrigger;

		public ushort usCnt_hEvents;
		public fixed char evt0Name[Consts.cntof_q2Cfg_evt0Name];                     //  2009/12/22
	};


	public class CQyQ2Help : CQyRWLock
	{


		public QY_Q2 m_pQ2;
		//
		public GENERIC_Q m_pQ_toGetMsg;
		public GENERIC_Q m_pQ_toPostMsg;


		public CQyQ2Help()
		{
			m_pQ2 = null;

		}
		~CQyQ2Help()
		{ }

		public GENERIC_Q getQ_toPostMsg(QY_Q2 pQ2)
		{
			if (null == pQ2) return null;
			if (m_pQ2 != null)
			{
				if (m_pQ2 != pQ2) return null;
			}
			else
				m_pQ2 = pQ2;

			if (m_pQ_toPostMsg != null) return m_pQ_toPostMsg;

			if (this.rLock(pQ2.pRw_syncFlg, (int)pQ2.cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_r) != 0) return null;

			m_pQ_toPostMsg = pQ2.bFlag_toPostMsg_q1 ? pQ2.qs1 : pQ2.qs0;
			m_pQ_toGetMsg = pQ2.bFlag_toPostMsg_q1 ? pQ2.qs0 : pQ2.qs1;

			return m_pQ_toPostMsg;
		}
		public GENERIC_Q getQ_toGetMsg(QY_Q2 pQ2)
		{
			if (null == pQ2)
			{
				return null;
			}

			if (m_pQ2 != null)
			{
				if (m_pQ2 != pQ2)
				{
					return null;
				}
			}
			else
				m_pQ2 = pQ2;

			if (m_pQ_toGetMsg != null) return m_pQ_toGetMsg;

			GENERIC_Q pQ_toGetMsg;
			GENERIC_Q pQ_toPostMsg;
			if (pQ2.bFlag_toPostMsg_q1)
			{
				pQ_toGetMsg = pQ2.qs0;
				pQ_toPostMsg = pQ2.qs1;
			}
			else
			{
				pQ_toGetMsg = pQ2.qs1;
				pQ_toPostMsg = pQ2.qs0;
			}

			if (qyFuncs.isQEmpty(pQ_toGetMsg))
			{
				if (!qyFuncs.isQEmpty(pQ_toPostMsg))
				{
					//
					string hint = "qyq2Help.getQ_toGetMsg";

					//  try to switch outputQ
					if (this.wLock(pQ2.pRw_syncFlg, (int)pQ2.cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_w,  hint) != 0)
					{
						return null;
					}
					pQ2.bFlag_toPostMsg_q1 = !pQ2.bFlag_toPostMsg_q1;
					this.unlock();
				}
			}

			/*
		#if  0
			if  (  this->rLock(  pQ2->mutexName_syncFlg,  pQ2->cfg.rwLockParam_syncFlg.uiMilliSeconds_mutex_r,  pQ2->hSema_syncFlg,  pQ2->cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_r  )  )  return  NULL;
		#else
			if  (  this->rLock(  pQ2->pRw_syncFlg,  pQ2->cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_r  )  )  return  NULL;
		#endif
			*/

			/*
			m_var.pQ_toPostMsg  =  pQ2->bFlag_toPostMsg_q1  ?  &pQ2->qs[1]  :  &pQ2->qs[0];
			m_var.pQ_toGetMsg  =  pQ2->bFlag_toPostMsg_q1  ?  &pQ2->qs[0]  :  &pQ2->qs[1];
			*/
			if (pQ2.bFlag_toPostMsg_q1)
			{
				m_pQ_toGetMsg = pQ2.qs0;
				m_pQ_toPostMsg = pQ2.qs1;
			}
			else
			{
				m_pQ_toGetMsg = pQ2.qs1;
				m_pQ_toPostMsg = pQ2.qs0;
			}

			return m_pQ_toGetMsg;

		}


		public int qPostMsgAndTrigger(IntPtr pQElem, uint size, QY_Q2 pQ2)
        {
			
				int tmpiRet = 0;
				bool ucbTriggerErr = false;

				if (pQ2==null) return -1;

				if (pQElem!=IntPtr.Zero && size!=0)
				{
					if ((tmpiRet = qyFuncs.qPostMsg(pQElem, size, getQ_toPostMsg(pQ2)))!=0)
					{
						//traceLogA("qyQ2Help::qPostMsgAndTrigger: qPostMsg returns %d", tmpiRet);
					}
				}
			try
			{
				//if (pQ2->hSemaTrigger1 && !ReleaseSemaphore(pQ2->hSemaTrigger1, 1, &pQ2->lPrevCnt_semaTrigger))
				if (pQ2.hSemaTrigger1 != null ) {
					qyFuncs.mySemaRelease(pQ2.hSemaTrigger1, 1);
				
					//qyDisplayLastError("ReleaseSemaphore failed");

					//ucbTriggerErr = true;

					//  traceLogA(  "%S qPostMsgAndTrigger err: uiQNodes %d, lPrevCnt_semaTrigger %d",  pQ2->cfg.name,  pQ->uiQNodes,  pQ2->lPrevCnt_semaTrigger  );
				}
				else
				{
				}
				if (pQ2.hEvent0 != null)
				{
					//SetEvent(pQ2->hEvents[0]);
					qyFuncs.myEventSet(pQ2.hEvent0);
				}
			}
			catch(Exception e)
            {
				Console.WriteLine(e.ToString());
            }

				//  2008/11/21
				if (ucbTriggerErr != pQ2.ucbTriggerErr)
				{
					pQ2.ucbTriggerErr = ucbTriggerErr;
				}

				return tmpiRet;
			
		}

	};




	public unsafe class QY_Q2
	{                                   //  2009/05/13

		public QY_Q2_cfg cfg;

		//
		public bool bFlag_toPostMsg_q1;
		public CMutexRW pRw_syncFlg;

		//
		public Semaphore hSemaTrigger1;               //  2007/05/08
		public long lPrevCnt_semaTrigger;      //  2007/05/08, 没有严格意义，用来debug用。

		//
		//fixed IntPtr hEvents[1 + 1];
		public EventWaitHandle hEvent0;
		public EventWaitHandle hEvent1;

		public bool ucbTriggerErr;                //  2008/11/21. 在使用触发机制的队列中，当触发器满了，设置此位。有利于及时调用相关函数将触发器恢复

		//GENERIC_Q qs[2];
		public GENERIC_Q qs0;
		public GENERIC_Q qs1;

		//
		public uint dwTickCnt_lastData;

	}


	partial class qyFuncs
	{
		//  int  initQyQ2(  GENERIC_Q_CFG  *  pCfg,  RW_lock_param  *  pRwLockParam_syncFlg,  unsigned  short  usCnt_hEvents,  LPCTSTR  evt0Name,  PF_qElemNew  pfQElemNew,  PF_qElemInit  pfQElemInit,  PF_qElemExit  pfQElemExit,  PF_qElemFree  pfQElemFree,  PF_qElemRemove  pfQElemRemove,  QY_Q2  *  pQ2  )
		public static unsafe int initQyQ2(GENERIC_Q_CFG pCfg, RW_lock_param pRwLockParam_syncFlg, ushort usCnt_hEvents, string evt0Name, PF_qElemNewEx pfQElemNewEx, PF_qElemNew pfQElemNew, PF_qElemInit pfQElemInit, PF_qElemExit pfQElemExit, PF_qElemFreeEx pfQElemFreeEx, PF_qElemFree pfQElemFree, PF_qElemRemove pfQElemRemove, QY_Q2 pQ2)
		{
			//if (pCfg == null ) return -1;

			GENERIC_Q_cfgEx cfgEx = new GENERIC_Q_cfgEx();
			//memset(&cfgEx, 0, sizeof(cfgEx));
			//memcpy(&cfgEx.common, pCfg, sizeof(cfgEx.common));
			cfgEx.common = pCfg;

			cfgEx.pfQElemNewEx = pfQElemNewEx;
			cfgEx.pfQElemNew = pfQElemNew;
			cfgEx.pfQElemInit = pfQElemInit;
			cfgEx.pfQElemExit = pfQElemExit;
			cfgEx.pfQElemFreeEx = pfQElemFreeEx;
			cfgEx.pfQElemFree = pfQElemFree;
			cfgEx.pfQElemRemove = pfQElemRemove;

			return initQyQ2(cfgEx, pRwLockParam_syncFlg, usCnt_hEvents, evt0Name, pQ2);
		}

		//  2012/11/10
		public static unsafe int initQyQ2(GENERIC_Q_cfgEx pCfgEx, RW_lock_param pRwLockParam_syncFlg, ushort usCnt_hEvents, string evt0Name, QY_Q2 pQ2)
		{
			int iErr = -1;
			int j;


			//if (null == pRwLockParam_syncFlg) return -1;
			if (pQ2 == null) return -1;

			GENERIC_Q_CFG* pCfg = &pCfgEx.common;
			fixed (char* pName = pQ2.cfg.name) {
				mytcsncpy(pName, new string(pCfg->name), Consts.cntof_q2Cfg_name);
			}
			pQ2.cfg.uiMaxQNodes = pCfgEx.common.uiMaxQNodes;

			//mymemcpy(&pQ2.cfg.rwLockParam_syncFlg, pRwLockParam_syncFlg, sizeof(pQ2->cfg.rwLockParam_syncFlg)  );
			pQ2.cfg.rwLockParam_syncFlg = pRwLockParam_syncFlg;

			pQ2.pRw_syncFlg = new CMutexRW();
			if (null == pQ2.pRw_syncFlg) goto errLabel;

			if (pCfg->uiMaxCnt_semaTrigger != 0)
			{
				pQ2.cfg.uiMaxCnt_semaTrigger = pCfg->uiMaxCnt_semaTrigger;
				//if (!(pQ2->hSemaTrigger1 = CreateSemaphore(NULL, 0, pQ2->cfg.uiMaxCnt_semaTrigger, NULL))) goto errLabel;
				pQ2.hSemaTrigger1 = new Semaphore(0, (int)pQ2.cfg.uiMaxCnt_semaTrigger);
				if (pQ2.hSemaTrigger1 == null) goto errLabel;
				//traceLogA("%S creates sema", pCfg->name);
			}


			if (usCnt_hEvents != 0)
			{
				if (usCnt_hEvents > 2) goto errLabel;
				pQ2.cfg.usCnt_hEvents = usCnt_hEvents;
				if (evt0Name != null)
				{
					fixed (char* pName = pQ2.cfg.evt0Name)
					{
						mytcsncpy(pName, evt0Name, Consts.cntof_q2Cfg_evt0Name);
					}
				}
				//
				//pQ2->hEvents[0] = CreateEvent(NULL, FALSE, FALSE, (pQ2->cfg.evt0Name[0] ? pQ2->cfg.evt0Name : NULL));
				fixed (char* pName = pQ2.cfg.evt0Name) {
					string str = null;
					if (pName[0] != (char)0) str = new string(pName);
                    //pQ2.hEvent0 = new EventWaitHandle(false, EventResetMode.AutoReset, str);
                    pQ2.hEvent0 = new EventWaitHandle(false, EventResetMode.AutoReset);
                    if (pQ2.hEvent0 == null) goto errLabel;
				}
				//for (j = 1; j < pQ2->cfg.usCnt_hEvents; j++)
				if (pQ2.cfg.usCnt_hEvents > 1)
				{
					//pQ2->hEvents[j] = CreateEvent(NULL, FALSE, FALSE, NULL);
					pQ2.hEvent1 = new EventWaitHandle(false, EventResetMode.AutoReset);
					//if (NULL == pQ2->hEvents[j]) goto errLabel;
					if (pQ2.hEvent1 == null) goto errLabel;
				}
			}


			//char mutexName[64] = ("");
			string str_mutexName;
			//for (j = 0; j < mycountof(pQ2->qs); j++)
			{
				j = 0;
				//_sntprintf(mutexName, mycountof(mutexName), _T("%s-%u"), pCfg->mutexName_prefix, j);
				str_mutexName = string.Format("{0}-{1}", new string(pCfg->mutexName_prefix), j);
				pQ2.qs0 = new GENERIC_Q();
				if (initGenericQ(str_mutexName, str_mutexName, 0, pCfg->uiMaxQNodes, pCfgEx.pfQElemNewEx, pCfgEx.pfQElemNew, pCfgEx.pfQElemInit, pCfgEx.pfQElemExit, pCfgEx.pfQElemFreeEx, pCfgEx.pfQElemFree, pCfgEx.pf_QElemRemoveEx, pCfgEx.pfQElemRemove, pQ2.qs0) != 0) goto errLabel;
				j = 1;
				str_mutexName = string.Format("{0}-{1}", new string(pCfg->mutexName_prefix), j);
				pQ2.qs1 = new GENERIC_Q();
				if (initGenericQ(str_mutexName, str_mutexName, 0, pCfg->uiMaxQNodes, pCfgEx.pfQElemNewEx, pCfgEx.pfQElemNew, pCfgEx.pfQElemInit, pCfgEx.pfQElemExit, pCfgEx.pfQElemFreeEx, pCfgEx.pfQElemFree, pCfgEx.pf_QElemRemoveEx, pCfgEx.pfQElemRemove, pQ2.qs1) != 0) goto errLabel;
			}


			iErr = 0;
		errLabel:

			if (iErr != 0) exitQyQ2(pQ2);

			return iErr;
		}

		public static int exitQyQ2(QY_Q2 pQ2)
		{
			int j;
			if (pQ2 == null) return 0;


			//for (j = 0; j < mycountof(pQ2->qs); j++)
			{
				exitGenericQ(pQ2.qs0);
				pQ2.qs0 = null;
				exitGenericQ(pQ2.qs1);
				pQ2.qs1 = null;
			}

			//for (j = 0; j < mycountof(pQ2->hEvents); j++)
			{
				if (pQ2.hEvent0 != null)
				{
					//CloseHandle(pQ2->hEvents[j]); pQ2->hEvents[j] = NULL;
					pQ2.hEvent0.Close(); pQ2.hEvent0 = null;

				}
				if (pQ2.hEvent1 != null)
				{
					pQ2.hEvent1.Close(); pQ2.hEvent1 = null;
				}

			}

			if (pQ2.hSemaTrigger1 != null)
			{
				//CloseHandle(pQ2->hSemaTrigger1); pQ2->hSemaTrigger1 = NULL;
				pQ2.hSemaTrigger1.Close(); pQ2.hSemaTrigger1 = null;
			}

			//MACRO_safeDelete(pQ2->pRw_syncFlg);
			if (pQ2.pRw_syncFlg != null)
			{
				pQ2.pRw_syncFlg.Close();
				pQ2.pRw_syncFlg = null;
			}

			return 0;
		}

		public static void emptyQ2(QY_Q2 pQ)
		{
			if (pQ == null) return;
			emptyGenericQ(pQ.qs0);
			emptyGenericQ(pQ.qs1);
		}

		public static int q2AvoidTriggerTooHigh(string mutexName_toAvoidTriggerTooHigh, QY_Q2 pQ)
		{
			/*
						if (pQ==null || pQ->hSemaTrigger1==null) return 0;

						long lPrevCnt_semaTrigger; lPrevCnt_semaTrigger = pQ.lPrevCnt_semaTrigger;
						lPrevCnt_semaTrigger = Math.Min((int)pQ->cfg.uiMaxCnt_semaTrigger, lPrevCnt_semaTrigger);
						long minlPrevCnt = Math.Max(10, pQ.cfg.uiMaxCnt_semaTrigger / 10);
						long maxlPrevCnt = Math.Max(50, pQ.cfg.uiMaxCnt_semaTrigger / 2);

						if (lPrevCnt_semaTrigger > minlPrevCnt)
						{
							using (CQySyncObj syncObj = new CQySyncObj())
							{

								if (mutexName_toAvoidTriggerTooHigh != null ) {
									char[] a = mutexName_toAvoidTriggerTooHigh.ToCharArray();
									if ( a[0]!=0)
									//&& mutexName_toAvoidTriggerTooHigh.Substring(0, 1) != 0)
								{
									if (syncObj.sync(mutexName_toAvoidTriggerTooHigh, 0) != 0) return 0;
								}

								if (lPrevCnt_semaTrigger > maxlPrevCnt)
								{               //  要在这里用waitForSingleObject把计数降下来。否则，其他地方会因为计数过大，而触发出错变慢。
									int i;
									i = lPrevCnt_semaTrigger - minlPrevCnt;
									i = min(i, 50);                                     //  一次最多50个，否则，容易时间太长
									for (; i; i--)
									{
										WaitForSingleObject(pQ->hSemaTrigger1, 0);
										pQ->lPrevCnt_semaTrigger--;
									}
								}
								else
								{
									WaitForSingleObject(pQ->hSemaTrigger1, 0); pQ->lPrevCnt_semaTrigger--;
								}
							}

						}
			*/

			return 0;
		}

		public static int q2PostMsg(IntPtr pQElem, uint size, QY_Q2 pQ2)
		{
			int iErr = -1;

			//
			pQ2.dwTickCnt_lastData = myGetTickCount(null);


			//
			using (CQyQ2Help q2Help = new CQyQ2Help())
			{
				if (q2Help.getQ_toPostMsg(pQ2) == null)
				{
					traceLog("q2PostMsg failed, getQ_toPostMsg returns null");
					return -1;
				}


				return qPostMsg(pQElem, size, q2Help.m_pQ_toPostMsg);
			}
		}

		public static int q2PostMsgAndTrigger(IntPtr pQElem, uint size, QY_Q2 pQ2)
		{
			//
			pQ2.dwTickCnt_lastData = myGetTickCount(null);

			//
			using (CQyQ2Help q2Help = new CQyQ2Help())
			{

				return q2Help.qPostMsgAndTrigger(pQElem, size, pQ2);
			}
		}

		public static unsafe int q2TriggerToProcess(QY_Q2 pQ2)
		{
#if false
			if (pQ2.hEvent1 != null) {
				//SetEvent(pQ2->hEvents[1]);
				myEventSet(pQ2.hEvent1);
			}
#endif
			if (pQ2.hEvent0 != null)
            {
				myEventSet(pQ2.hEvent0);
            }
			return 0;
		}

		//  2010/05/03
		public static unsafe int q2GetMsg(QY_Q2 pQ2, IntPtr pQElem, uint* puiSize)
		{
			using (CQyQ2Help q2Help = new CQyQ2Help())
			{
				return qGetMsg(q2Help.getQ_toGetMsg(pQ2), pQElem, puiSize);
			}
		}

		//  2014/03/25
		public static unsafe int q2GetMsgPkg(QY_Q2 pQ2, IntPtr pPkg, uint uiSize_pkg, uint[] uiLens, uint* pnLens)
		{

			using (CQyQ2Help q2Help = new CQyQ2Help())
			{
				return qGetMsgPkg(q2Help.getQ_toGetMsg(pQ2), pPkg, uiSize_pkg, uiLens, pnLens);
			}
		}


		//  2014/04/10
		public static unsafe int q2GetMsgs(QY_Q2 pQ2, IntPtr []pQElems, uint[] puiSizes, uint* pnQElems)
		{
			using (CQyQ2Help q2Help = new CQyQ2Help())
			{
				return qyFuncs.qGetMsgs(q2Help.getQ_toGetMsg(pQ2), pQElems, puiSizes, pnQElems);
			}
		}

		public static bool isQ2Warning(QY_Q2 pQ2, int uiQNodes_warningInterval)
		{
			if (null == pQ2) return false;
			if (pQ2.bFlag_toPostMsg_q1) return isQWarning(pQ2.qs1, uiQNodes_warningInterval);
			else return isQWarning(pQ2.qs0, uiQNodes_warningInterval);
		}
		public static bool isQ2Empty(QY_Q2 pQ2)
		{
			if (null == pQ2) return false;
			return (isQEmpty(pQ2.qs0) && isQEmpty(pQ2.qs1));
		}


		public static int getQ2Nodes_toPostMsg(QY_Q2 pQ2) {
			if (null == (pQ2)) return -1;
			return (int)((pQ2).bFlag_toPostMsg_q1 ? (pQ2).qs1.uiQNodes : (pQ2).qs0.uiQNodes);
		}

		public static int getQ2Nodes_toGetMsg(QY_Q2 pQ2) {
			if (null == (pQ2)) return -1;
			return (int)((pQ2).bFlag_toPostMsg_q1 ? (pQ2).qs0.uiQNodes : (pQ2).qs1.uiQNodes);
		}
		//  2015/02/18
		public static int getQ2Nodes(QY_Q2 pQ2) {
			if (null == (pQ2)) return -1;
			if (null == pQ2.qs0 || null == pQ2.qs1) return -1;
			return (int)((pQ2).qs0.uiQNodes + (pQ2).qs1.uiQNodes);
		}




	}


}

