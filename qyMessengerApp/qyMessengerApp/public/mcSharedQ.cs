using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
	public const int cntof_qMem_mcTask_tDesc = 16;
}


namespace qy
{
	//
	public struct QELEM_mcTask
	{
		public object m_pBuf;
		//public object m_obj;
	}

	public  struct QMEM_mcTask
	{
		//public SYNC_mt_cnt syncMtCnt;

		//
		public bool bUsed;                         //  赋值true表明new

		//
#if false
		public QY_MEMORY qElemMemory;
		public uint size;
#else
		public QELEM_mcTask qElemMemory;
		public uint size;
#endif

		//
		public bool ucbQElemInited;                   //  表明是否初始化

		//
		public bool bNeedRemoved;                  //  是否准备放弃

		//
		//public fixed char tDesc[Consts.cntof_qMem_mcTask_tDesc + 1];
		public string tDesc;

	}
	;

	//
	//
	public delegate object PF_qElemNewEx_mcTask(int iAllocType, int size, ref QELEM_mcTask p);   //  2010/04/24
	public delegate void PF_qElemFreeEx_mcTask(ref QELEM_mcTask p);                              //  2010/04/24
																							  //
	public delegate int PF_qElemInit_mcTask(object p);
	public delegate void PF_qElemExit_mcTask(object p);
	//
	public delegate void PF_qElemRemoveEx_mcTask(object p, int size, string hint); //  2012/11/10

    //
    //public delegate int PF_enumTask( CTX_caller ctxCaller, object p0, object p1, TASK_common taskElem);
    public delegate int PF_enumTask(CTX_caller ctxCaller, ref MIS_MSGU p0, PARAM_findOrgMsg p1, TASK_common taskElem);
    public delegate bool PF_bEnumTask( CTX_caller ctxCaller, ref MIS_MSGU p0, PARAM_findOrgMsg p1, TASK_common taskElem);        //  TRUE or FALSE



	public struct MC_sharedQ_var
	{

		public GENERIC_Q_CFG cfg;

		//
		//
		public PF_qElemNewEx_mcTask pfQElemNewEx;				//  2010/04/24
		public PF_qElemFreeEx_mcTask pfQElemFreeEx;            //  2010/04/24
														   //
		public PF_qElemInit_mcTask pfQElemInit;				//  2007/06/10
		public PF_qElemExit_mcTask pfQElemExit;				//  
														
		//														   //
		public PF_qElemRemoveEx_mcTask pfqElemRemoveEx;           //  2012/11/10

		//
		public RW_lock_param rwLockParam_sync;

		//
		public CMutexRW pRw_sync;

		//
		public QMEM_mcTask[] pMems;
		public Mutex[] hMutexs;
		//
		public uint uiQNodes;

		//
		public uint dwThreadId_mgr;


	}
	;


    public class CMcSharedQ
    {
		public MC_sharedQ_var m_var;

		public bool bSetQInfo(string mutexName_sync, uint uiMaxCnt_sema, uint uiMilliSeconds_mutex_r, uint uiMilliSeconds_sema_r, uint uiMilliSeconds_mutex_w, uint uiMilliSeconds_sema_w)
		{
			return true;
		}

		public int initQ(GENERIC_Q_CFG pCfg, RW_lock_param pRwLockParam_sync, PF_qElemNewEx_mcTask pfQElemNewEx, PF_qElemInit_mcTask pfQElemInit, PF_qElemExit_mcTask pfQElemExit, PF_qElemFreeEx_mcTask pfQElemFreeEx )
		{
			int iErr = -1;

			//memcpy(&m_var.cfg, pCfg, sizeof(m_var.cfg));
			m_var.cfg = pCfg;
			//
			m_var.pfQElemNewEx = pfQElemNewEx;
			m_var.pfQElemFreeEx = pfQElemFreeEx;
			//
			m_var.pfQElemInit = pfQElemInit;
			m_var.pfQElemExit = pfQElemExit;

			//
			m_var.rwLockParam_sync = pRwLockParam_sync;

			//
			m_var.pRw_sync = new CMutexRW();

			//
			//int size = (int)(sizeof(QMEM_mcTask) * m_var.cfg.uiMaxQNodes);
			m_var.pMems = new QMEM_mcTask[m_var.cfg.uiMaxQNodes];// (QMEM_mcTask*)qyFuncs.mymalloc(size);
			if (null==m_var.pMems) goto errLabel;
			//qyFuncs.mymemset((IntPtr)m_var.pMems, 0, size);

			//
			m_var.hMutexs = new Mutex[m_var.cfg.uiMaxQNodes];
			if (m_var.hMutexs == null) goto errLabel;
			for ( int i = 0; i < m_var.cfg.uiMaxQNodes; i++)
            {
				m_var.hMutexs[i] = new Mutex(false);
				if (m_var.hMutexs[i] == null) goto errLabel;
            }

			iErr = 0;

		errLabel:

			return iErr;
		}


		string getMutexName(int i)
		{
			string str = string.Format("mcSharedObj{0}", i);
			return str;
		}



		void qNodeFree(ref QMEM_mcTask pMem, bool bElemRemove)
		{
			ref MC_sharedQ_var pQ = ref m_var;			

			//if (pQ != &m_var) return;
			ref QMEM_mcTask pQNode =ref pMem;

			if (pQNode.qElemMemory.m_pBuf!=null)
			{
				if (pQNode.ucbQElemInited)
				{
					if (pQ.pfQElemExit!=null) pQ.pfQElemExit(pQNode.qElemMemory.m_pBuf);
					pQNode.ucbQElemInited = false;
				}
				if (bElemRemove)
				{   //  2009/12/08
					string str="";
					/*
					fixed(char *pName=pQ.cfg.name)
                    {
						str = new string(pName);
                    }
					*/
					if (pQ.pfqElemRemoveEx!=null) pQ.pfqElemRemoveEx(pQNode.qElemMemory.m_pBuf, (int)pQNode.size, str); //  2012/11/10
				}
				if (pQ.pfQElemFreeEx!=null) pQ.pfQElemFreeEx(ref pQNode.qElemMemory);
				//
				pQNode.qElemMemory.m_pBuf = IntPtr.Zero;
			}
			//
			pQNode.ucbQElemInited = false;
			pQNode.bNeedRemoved = false;
			pQNode.bUsed = false;

			//
			//TCHAR tBuf[128];
			//_sntprintf(tBuf, mycountof(tBuf), _T("mcSharedQ::qNodeFree %s"), pQNode->tDesc);
			//showInfo_open(0, 0, tBuf);

			//
			return;
		}


		
		//	
		int recalc_uiQNodes()
		{
			int iErr = -1;
			int i;
			int iQNodes = 0;

			for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
			{
				ref QMEM_mcTask pMem = ref m_var.pMems[i];
				//
				if (!pMem.bUsed) continue;
				//
				iQNodes++;
			}

			if (m_var.uiQNodes != iQNodes)
			{
				m_var.uiQNodes = (uint)iQNodes;
			}

			iErr = 0;
		errLabel:
			return iErr;
		}


		//
		public void exitQ()
		{
			int i;

			//

				for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
				{
					ref QMEM_mcTask pMem = ref m_var.pMems[i];
					if (!pMem.bUsed) continue;
					{
						using (CQySyncObj syncObj = new CQySyncObj())
						{
							string mutexName;

							//
							pMem.bNeedRemoved = true;

							//
							mutexName = getMutexName(i);
							//
							//
							this.qNodeFree(ref pMem, true);
						}
					}
				}
				//
				this.recalc_uiQNodes();

			//
			if (isEmpty())
			{
				//MACRO_safeFree(m_var.pMems);
				if (m_var.pMems != null)
				{
					//qyFuncs.myfree((IntPtr)m_var.pMems); 
					m_var.pMems = null;
				}
				//
				for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
				{
					if (m_var.hMutexs[i] != null)
					{
						m_var.hMutexs[i].Close(); m_var.hMutexs[i] = null;
					}
				}
				m_var.hMutexs = null;
			}

			return;
		}


		public int qPostMsg(object pQElem, uint size_unused)
		{
			int iErr = -1;
			int i;
			ref MC_sharedQ_var pQ = ref m_var;
			int iAllocType = 0;
			uint size = size_unused;

			//
			using (CQyRWLock rwLock = new CQyRWLock())
			{
				string hint = "mcSharedQ.qPostMsg";
				//
				if (0!=rwLock.wLock(m_var.pRw_sync, (int)m_var.rwLockParam_sync.uiMilliSeconds_sema_w,hint)) return -1;

				//
				for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
				{
					if (m_var.pMems[i].bUsed) continue;
					break;
				}
				if (i == m_var.cfg.uiMaxQNodes)
				{
					//showInfo_open(0, 0, _T("mcSharedQ qPostMsg failed, full"));
					return Consts.CONST_qyRet_qFull;
				}

				//
				ref QMEM_mcTask pQNode = ref m_var.pMems[i];

				//
				{
					string mutexName;
					mutexName = getMutexName(i);
					if (mutexName == null) return -1;

					//
				
					//
					pQNode.bUsed = true;

					//
					string str = string.Format("sharedQ.mems{0}", i);
					//_sntprintf(pQNode->tDesc, mycountof(pQNode->tDesc), _T("sharedQ.mems[%d]"), i);

					/*
					 * fixed (char* tDesc = pQNode.tDesc)
					{
						qyFuncs.mytcsncpy(tDesc, str, Consts.cntof_qMem_mcTask_tDesc);
					}
					*/
					pQNode.tDesc = str;

					//
					if (pQ.pfQElemNewEx != null)
					{
						if (null == pQ.pfQElemNewEx( iAllocType, (int)size, ref pQNode.qElemMemory)) goto errLabel;
					}
					/*
					if (null != pQNode.qElemMemory.m_pBuf && size != 0)
					{
						qyFuncs.mymemcpy(pQNode->qElemMemory.m_pBuf, pQElem, size);
						pQNode->size = size;
					}
					*/
					pQNode.qElemMemory.m_pBuf = pQElem;
					pQNode.size = size;
					//
					if (pQ.pfQElemInit != null && pQ.pfQElemInit(pQNode.qElemMemory.m_pBuf) != 0) goto errLabel;
					pQNode.ucbQElemInited = true;

					//
					
				}


				//pQ->uiQNodes  ++  ;
				this.recalc_uiQNodes();





				iErr = 0;

			errLabel:

				//
				if (iErr != 0)
				{
					qNodeFree(ref pQNode, false);
				}

			}
			
			//
			return iErr;
		}

		 int qGetMsg(IntPtr pQElem, ref uint puiSize)
		{

			return -1;
		}

		public  int qRemoveMsg( CTX_caller ctxCaller,PF_bEnumTask pf_bRemoveCond, ref MIS_MSGU p0, PARAM_findOrgMsg p1)
		{
			int iErr = -1;
			int i;
			bool tmpbRet;
			bool bExists_needRemoved = false;

			//
			using (CQyRWLock rwLock = new CQyRWLock())
			{

				if (0 != rwLock.rLock(m_var.pRw_sync, (int)m_var.rwLockParam_sync.uiMilliSeconds_sema_r)) return -1;

				//
				for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
				{
					ref QMEM_mcTask pMem = ref m_var.pMems[i];
					if (!pMem.bUsed) continue;
					//
					if (pMem.bNeedRemoved)

					{
						bExists_needRemoved = true;
						continue;

					}
					//
					tmpbRet = pf_bRemoveCond(ctxCaller,ref p0, p1, (TASK_common)pMem.qElemMemory.m_pBuf);
					if (!tmpbRet) continue;
					pMem.bNeedRemoved = true;
					bExists_needRemoved = true;
					continue;
				}


			}

			if (bExists_needRemoved)
			{
				//
				this.chkMsgs_toBeRemoved();
			}
		
		
			iErr = 0;
		errLabel:

			return iErr;
		}



		public void emptyQ()
		{
			int iErr = -1;
			int i;
			int tmpbRet;

			using (CQyRWLock rwLock = new CQyRWLock())
			{
				//
				string w_hint = "mcSharedQ.emptyQ";

				//
				if (0 != rwLock.wLock(m_var.pRw_sync, (int)m_var.rwLockParam_sync.uiMilliSeconds_mutex_w,w_hint)) return;

				for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
				{
					ref QMEM_mcTask pMem = ref m_var.pMems[i];
					if (!pMem.bUsed) continue;
					//

					pMem.bNeedRemoved = true;


					//
					qNodeFree(ref pMem, true);


				}
				//
				this.recalc_uiQNodes();

			}

			iErr = 0;
		errLabel:

			return;
		}



		public int qTraverse( CTX_caller ctxCaller, PF_enumTask pf_visit, ref MIS_MSGU p0, PARAM_findOrgMsg p1)
		{
			int iErr = -1;
			int i;
			int tmpiRet;

			//
			int nNodes = (int)m_var.uiQNodes;

			using (CQyRWLock rwLock = new CQyRWLock())
			{
				if (0 != rwLock.rLock(m_var.pRw_sync, (int)m_var.rwLockParam_sync.uiMilliSeconds_sema_r))
				{
					string str;
					str = "mcSharedQ.qTraverse: rLock failed:" + m_var.pRw_sync.w_hint;
					qyFuncs.showInfo_open(0, null, str);
					return -1;
				}
				//
				for (i = 0; i < m_var.cfg.uiMaxQNodes && nNodes != 0; i++)
				{
					ref QMEM_mcTask pMem = ref m_var.pMems[i];
					if (!pMem.bUsed) continue;
									
					//					
					if (!pMem.ucbQElemInited || pMem.bNeedRemoved) continue;
					
					//				
					nNodes--;
						
					//					
					tmpiRet = pf_visit(ctxCaller, ref p0, p1, (TASK_common)pMem.qElemMemory.m_pBuf);						
					if (tmpiRet != 0)							
					{					
						iErr = tmpiRet; goto errLabel;							
					}					
				}

			}

			if (nNodes!=0)
			{
			}

			//
			iErr = 0;
		errLabel:

			return iErr;

			return -1;
		}


		public int qTraverse_mono( CTX_caller ctxCaller, PF_enumTask pf_visit, ref MIS_MSGU p0, PARAM_findOrgMsg p1)
		{
			//return qTraverse(pf_visit, p0, p1);
			
			//
				int iErr = -1;
				int i;
				int tmpiRet;

			//
			uint dwTickCnt0 = qyFuncs.myGetTickCount(null);

				//
				int nNodes = (int)m_var.uiQNodes;

				using (CQyRWLock rwLock = new CQyRWLock())
				{
				string w_hint = "mcSharedQ.qTraverse_mono";

				//
				if (0 != rwLock.wLock(m_var.pRw_sync, (int)m_var.rwLockParam_sync.uiMilliSeconds_sema_w, w_hint))
				{
					qyFuncs.showInfo_open(0, null, "mcSharedQ.qTraverse_mono: wLock failed");
					return -1;
				}
					//
					for (i = 0; i < m_var.cfg.uiMaxQNodes && nNodes != 0; i++)
					{
						ref QMEM_mcTask pMem = ref m_var.pMems[i];
						if (!pMem.bUsed) continue;

						//					
						if (!pMem.ucbQElemInited || pMem.bNeedRemoved) continue;

						//				
						nNodes--;

						//					
						tmpiRet = pf_visit(ctxCaller, ref p0, p1, (TASK_common)pMem.qElemMemory.m_pBuf);
						if (tmpiRet != 0)
						{
							iErr = tmpiRet; goto errLabel;
						}
					}

				}

				if (nNodes != 0)
				{
				}

				//
				iErr = 0;
		errLabel:

			//
			uint dwTickCnt1 = qyFuncs.myGetTickCount(null);
			int iDiffInMs = (int)(dwTickCnt1 - dwTickCnt0);
			if  (  iDiffInMs>500)
            {
				int ii = 0;
            }


			//
				return iErr;

		
		}

		


		bool isFull()
		{
			if (m_var.uiQNodes >= m_var.cfg.uiMaxQNodes) return true;
			return false;
		}

		public bool isWarning(uint uiQNodes_warningInterval)      //  判断是否队列的成员空闲空间〈uiQNodes_interval
		{
			if (m_var.uiQNodes + uiQNodes_warningInterval >= m_var.cfg.uiMaxQNodes) return true;

			return false;
		}


		public bool isEmpty()
		{
			if (0==m_var.uiQNodes) return true;

			return false;
		}


		int chkMsgs_toBeRemoved()
		{
			int iErr = -1;
			int i;
#if false
			int iThreadId = GetCurrentThreadId();

			if (iThreadId != m_var.dwThreadId_mgr)
			{
			return -1;
			}
#endif
			//
			for ( i=0;i<m_var.cfg.uiMaxQNodes;i++)
            {
				ref QMEM_mcTask pMem = ref m_var.pMems[i];
				if (!pMem.bUsed) continue;
				if (pMem.bNeedRemoved) break;
            }
			if (i == m_var.cfg.uiMaxQNodes) return 0;


			using (CQyRWLock rwLock = new CQyRWLock())
			{
				//
				string w_hint = "mcSharedQ.chkMsgs_toBeRemoved";
				//
				if (0 != rwLock.wLock(m_var.pRw_sync, (int)m_var.rwLockParam_sync.uiMilliSeconds_sema_w,w_hint)) return -1;
				//
				for (i = 0; i < m_var.cfg.uiMaxQNodes; i++)
				{
					ref QMEM_mcTask pMem = ref m_var.pMems[i];
					if (!pMem.bUsed) continue;
					//
					if (pMem.ucbQElemInited
						&& !pMem.bNeedRemoved
						)
					{
						continue;
					}
					//			
					pMem.bNeedRemoved = true;
						
					//					
					qNodeFree(ref pMem, true);
					
				}
				//
				this.recalc_uiQNodes();
			}

			iErr = 0;
		errLabel:
			return iErr;

		}

		

	}


}
