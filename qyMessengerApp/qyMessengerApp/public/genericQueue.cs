
#define __DUAL_LINK_LIST__


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

static partial class Consts
{
	public const int cntof_qCfg_name = 24;

}


namespace qy
{

	public unsafe struct Q_NODE
	{

		//  Q_ELEM_T						*	pQElem;
		public QY_MEMORY qElemMemory;
		public uint size;
		//
		public bool ucbQElemInited;

#if __DUAL_LINK_LIST__
		public Q_NODE * pPrev;
#endif

		public Q_NODE * pNext;

		//  unsigned  int						uiSeqNo;				//  下次加入连表的节点的序号；


	};

	//
	public delegate IntPtr PF_qElemNew(int size);
	public delegate void PF_qElemFree(IntPtr p);
	//
	public unsafe delegate IntPtr PF_qElemNewEx(int iAllocType, int size, ref QY_MEMORY p);   //  2010/04/24
	public unsafe delegate void PF_qElemFreeEx(ref QY_MEMORY p);                              //  2010/04/24
																						   //
	public delegate int PF_qElemInit(IntPtr p);
	public delegate void PF_qElemExit(IntPtr p);
	public unsafe delegate void PF_qElemRemove(IntPtr p, string hint);
	//
	public unsafe delegate void PF_qElemRemoveEx(IntPtr p, int size, string hint); //  2012/11/10


	public unsafe struct GENERIC_Q_CFG
	{
		public fixed char name[Consts.cntof_qCfg_name];                 //  队列名称, 用在调试等工作用
		public uint uiMaxQNodes;               //  对队列中的节点数做限制，为0时不限制

		public fixed char mutexName_prefix[Consts.cntof_mutexName_prefix];     //  队列中的增删处理节点同步用

		//
		public uint uiMaxCnt_semaTrigger;      //  对用在线程处理做触发用的semaphore, 为0时不用

		//
		public bool bUseEvt;
		public fixed char evt0Name[24];             //  2009/12/22

		//
		public bool b_showInfoQ;

	};

	//  2012/11/10
	public struct GENERIC_Q_cfgEx
	{
		public GENERIC_Q_CFG common;
		//
		public PF_qElemNewEx pfQElemNewEx;
		public PF_qElemNew pfQElemNew;
		public PF_qElemInit pfQElemInit;
		public PF_qElemExit pfQElemExit;
		public PF_qElemFreeEx pfQElemFreeEx;
		public PF_qElemFree pfQElemFree;
		public PF_qElemRemoveEx pf_QElemRemoveEx;
		public PF_qElemRemove pfQElemRemove;

	};


	public unsafe class GENERIC_Q
	{

		public GENERIC_Q_CFG cfg;


		public Q_NODE * pFront;
		public Q_NODE * pRear;
		//				 
		public PF_qElemNew pfQElemNew;
		public PF_qElemFree pfQElemFree;
		//
		public PF_qElemNewEx pfQElemNewEx;             //  2010/04/24
		public PF_qElemFreeEx pfQElemFreeEx;               //  2010/04/24
														   //
		public PF_qElemInit pfQElemInit;               //  2007/06/10
		public PF_qElemExit pfQElemExit;               //  
													   //
		public PF_qElemRemove pfQElemRemove;               //  2009/12/08
														   //
		public PF_qElemRemoveEx pfqElemRemoveEx;           //  2012/11/10

		//
		public uint uiQNodes;

		//
		public Semaphore hSemaTrigger;                //  2007/05/08				 
		public long lPrevCnt_semaTrigger;      //  2007/05/08, 没有严格意义，用来debug用。

		//
		public EventWaitHandle hEvent;

		//				 	
		public bool ucbTriggerErr;                //  2008/11/21. 在使用触发机制的队列中，当触发器满了，设置此位。有利于及时调用相关函数将触发器恢复

		//
		public bool bNeedSync;

		//CRITICAL_SECTION cs;
		//BOOL bCsInited;

		public SYNC_OBJ_PARAM syncObjParam;
		public Mutex hMutex;

		//  2012/07/14
		public bool bInited;

		//  2009/09/20 
		public Q_NODE pLastQNode;

	};

	unsafe delegate int PF_qPostMsg(IntPtr pQElem, uint size, ref GENERIC_Q pQ);
	unsafe delegate int PF_qGetMsg(ref GENERIC_Q pQ, IntPtr pQElem, uint* puiSize);

	partial class qyFuncs
	{ 
		//
		public static bool isQFull( GENERIC_Q pQ)
		{
			if (pQ == null) return false;
			if ( pQ.cfg.uiMaxQNodes!=0 && pQ.uiQNodes >= pQ.cfg.uiMaxQNodes) return true;
			return false;			
		}

		public static bool isQWarning( GENERIC_Q pQ, int uiQNodes_warningInterval)
		{
			if (pQ == null) return false;
			if (pQ.cfg.uiMaxQNodes!=0 && pQ.uiQNodes + uiQNodes_warningInterval >= pQ.cfg.uiMaxQNodes) return true;
			return false;
		}

		public static unsafe bool isQEmpty( GENERIC_Q pQ)
		{
			if (pQ == null) return false;
			if (pQ.pFront == null) return false;
			//
			if (pQ.pFront->pNext!=null) return false;
			return true;
		}
		



		public static IntPtr objNew(int size)
		{
			IntPtr p = (IntPtr)null;

			p = mymalloc(size);
			//if (p) memset(p, 0, size);
			if (p != null)
			{
				//Array.Clear((bytep, 0, size);
				mymemset(p, 0, size);
			}

			return p;

		}


		public static void objFree(IntPtr p)
		{
			myfree(p);
			return;
		}


		static unsafe Q_NODE * qNodeNew()
		{
			Q_NODE *  p = (Q_NODE *)objNew(sizeof(Q_NODE));
			if (p == null) return p;
			return p;
		}



		static unsafe void qNodeFree( GENERIC_Q pQ, Q_NODE * pQNode, bool bElemRemove)
		{

			if (IntPtr.Zero == (IntPtr)pQNode) return;

			//  2007/04/17, qNodeFreeÒªÊÇÊÍ·ÅËùÓÐºóÐøµÄ½Úµã»áÔì³É¶Ô¶ÑÕ»²»Àû£¬ËùÒÔ¸ÄÎª½öÊÍ·ÅÒ»¸ö

			if (pQNode->qElemMemory.m_pBuf != IntPtr.Zero)
			{
				if (pQNode->ucbQElemInited)
				{
					if (pQ.pfQElemExit != null) pQ.pfQElemExit(pQNode->qElemMemory.m_pBuf);
					pQNode->ucbQElemInited = false;
				}
				if (bElemRemove)
				{   //  2009/12/08
					string hint;
					fixed (char* Name = pQ.cfg.name)
					{
						hint = new string(Name);
					}
					//
					if (pQ.pfqElemRemoveEx != null) pQ.pfqElemRemoveEx(pQNode->qElemMemory.m_pBuf, (int)pQNode->size, hint); //  2012/11/10
					else
					{
						if (pQ.pfQElemRemove != null) pQ.pfQElemRemove(pQNode->qElemMemory.m_pBuf, hint);
					}
				}
				if (pQ.pfQElemFreeEx != null) pQ.pfQElemFreeEx(ref pQNode->qElemMemory);
				else
				{
					if (pQ.pfQElemFree != null) pQ.pfQElemFree(pQNode->qElemMemory.m_pBuf);
				}
			}
			//myfree((IntPtr)pQNode);
			objFree((IntPtr)pQNode);

			return;

		}


		public static unsafe int initGenericQ(string qName, string mutexName_prefix, uint uiMaxCnt_semaTrigger, uint uiMaxQNodes, PF_qElemNewEx pfQElemNewEx, PF_qElemNew pfQElemNew, PF_qElemInit pfQElemInit, PF_qElemExit pfQElemExit, PF_qElemFreeEx pfQElemFreeEx, PF_qElemFree pfQElemFree, PF_qElemRemoveEx pfQElemRemoveEx, PF_qElemRemove pfQElemRemove, GENERIC_Q pQ)
		{
			int iErr = -1;

			if (pQ==null) return -1;

			//memset(pQ, 0, sizeof(GENERIC_Q));

			if (qName != null)				
			{
				fixed (char* pName = pQ.cfg.name)
				{
					mytcsncpy(pName, qName, Consts.cntof_qCfg_name);
				}		
			}
			if (mutexName_prefix != null)
			{
				fixed (char* pName = pQ.cfg.mutexName_prefix)
				{
					mytcsncpy(pName, mutexName_prefix, Consts.cntof_mutexName_prefix);
				}
				//  2009/05/14
				pQ.bNeedSync = true;

				/*
				__try  {
					InitializeCriticalSection(&pQ->cs);
				}
				__except(GetExceptionCode() == STATUS_NO_MEMORY)  {
					traceLogA("initGenericQ: InitializeCriticalSection failed.");
					goto errLabel;
				}				
				pQ.bCsInited = TRUE;
				*/

				//pQ.syncObjParam.pMutexName = pQ.cfg.mutexName_prefix;
				fixed (char* pName = pQ.cfg.mutexName_prefix,pBuf = pQ.syncObjParam.mutexName) {			
					mytcsncpy(pBuf, new string(pName), Consts.cntof_mutexName_prefix);			
				}
				//pQ->syncObjParam.lpCriticalSection = &pQ->cs;

				//
				pQ.hMutex = new Mutex();
				if (pQ.hMutex == null) goto errLabel;
			}
			if (uiMaxCnt_semaTrigger != 0)
			{
				pQ.cfg.uiMaxCnt_semaTrigger = uiMaxCnt_semaTrigger;
				//if (!(pQ->hSemaTrigger = CreateSemaphore(NULL, 0, pQ->cfg.uiMaxCnt_semaTrigger, NULL))) goto errLabel;
				pQ.hSemaTrigger = new Semaphore(0, (int)pQ.cfg.uiMaxCnt_semaTrigger);
				if (pQ.hSemaTrigger == null) goto errLabel;
				//traceLog(_T("CreateSemaphore: %s sema"), pQ->cfg.name);
			}
			pQ.cfg.uiMaxQNodes = uiMaxQNodes;

			pQ.pfQElemNewEx = pfQElemNewEx;
			pQ.pfQElemFreeEx = pfQElemFreeEx;
			//
			pQ.pfQElemNew = pfQElemNew;
			pQ.pfQElemFree = pfQElemFree;
			//
			pQ.pfQElemInit = pfQElemInit;
			pQ.pfQElemExit = pfQElemExit;
			//
			pQ.pfqElemRemoveEx = pfQElemRemoveEx;  //  2012/11/10
												   //
			pQ.pfQElemRemove = pfQElemRemove;

			//
			pQ.pFront = qNodeNew();
			if (pQ.pFront == null) goto errLabel;
			pQ.pRear = pQ.pFront;

			pQ.uiQNodes = 0;           //  ×¢Òâ£ºÕâÀï²»ÐèÒª¿¼ÂÇfront½Úµã£¬ËùÒÔ¼ÇÊýÆðµãÎª0

			//  2012/07/14
			pQ.bInited = true;


			//
			iErr = 0;

		errLabel:

			
			if (iErr!=0)
			{
				exitGenericQ(pQ);
			}
			



			//traceLogA("initGenericQ: %S %s", pQ->cfg.name, iErr ? "failed" : "ok");

			return iErr;

		}







		//
		public static unsafe int initGenericQ(string qName, string mutexName_prefix, uint uiMaxCnt_semaTrigger, uint uiMaxQNodes, PF_qElemNewEx pfQElemNewEx, PF_qElemNew pfQElemNew, PF_qElemInit pfQElemInit, PF_qElemExit pfQElemExit, PF_qElemFreeEx pfQElemFreeEx, PF_qElemFree pfQElemFree, PF_qElemRemove pfQElemRemove,  GENERIC_Q pQ)
		{
			PF_qElemRemoveEx pfQElemRemoveEx = null;

			return initGenericQ(qName, mutexName_prefix, uiMaxCnt_semaTrigger, uiMaxQNodes, pfQElemNewEx, pfQElemNew, pfQElemInit, pfQElemExit, pfQElemFreeEx, pfQElemFree, pfQElemRemoveEx, pfQElemRemove, pQ);
		}


		public static unsafe int initGenericQ(string qName, string mutexName_prefix, uint uiMaxCnt_semaTrigger, uint uiMaxQNodes, PF_qElemNew pfQElemNew, PF_qElemFree pfQElemFree,  GENERIC_Q pQ)
		{
			return initGenericQ(qName, mutexName_prefix, uiMaxCnt_semaTrigger, uiMaxQNodes, null, pfQElemNew, null, null, null, pfQElemFree, null,  pQ);
		}


		public static unsafe   int initGenericQ(GENERIC_Q_CFG pCfg, PF_qElemNew pfQElemNew, PF_qElemInit pfQElemInit, PF_qElemExit pfQElemExit, PF_qElemFree pfQElemFree, PF_qElemRemove pfQElemRemove,  GENERIC_Q pQ)
		{
			return initGenericQ(new string(pCfg.name), new string(pCfg.mutexName_prefix), pCfg.uiMaxCnt_semaTrigger, pCfg.uiMaxQNodes, null, pfQElemNew, pfQElemInit, pfQElemExit, null, pfQElemFree, pfQElemRemove,  pQ);
		}


		public static unsafe int initGenericQ(GENERIC_Q_CFG pCfg, PF_qElemNewEx pfQElemNewEx, PF_qElemNew pfQElemNew, PF_qElemInit pfQElemInit, PF_qElemExit pfQElemExit, PF_qElemFreeEx pfQElemFreeEx, PF_qElemFree pfQElemFree, PF_qElemRemove pfQElemRemove,  GENERIC_Q pQ)
		{
			return initGenericQ(new string(pCfg.name), new string(pCfg.mutexName_prefix), pCfg.uiMaxCnt_semaTrigger, pCfg.uiMaxQNodes, pfQElemNewEx, pfQElemNew, pfQElemInit, pfQElemExit, pfQElemFreeEx, pfQElemFree, pfQElemRemove,  pQ);
		}



		public static unsafe  int initGenericQ(GENERIC_Q_cfgEx pCfg,  GENERIC_Q pQ)
		{
			//if  (  initGenericQ(  &pCfg->common,  pCfg->pfQElemNewEx,  pCfg->pfQElemNew,  pCfg->pfQElemInit,  pCfg->pfQElemExit,  pCfg->pfQElemFreeEx,  pCfg->pfQElemFree,  pCfg->pfQElemRemove,  pQ  )  )  return  -1;

			//
			// __declspec(  dllexport  )  int  initGenericQ(  LPCTSTR  qName,  LPCTSTR  mutexName_prefix,  unsigned  int  uiMaxCnt_semaTrigger,  unsigned  int  uiMaxQNodes,  PF_qElemNewEx  pfQElemNewEx,  PF_qElemNew  pfQElemNew,  PF_qElemInit  pfQElemInit,  PF_qElemExit  pfQElemExit,  PF_qElemFreeEx  pfQElemFreeEx,  PF_qElemFree  pfQElemFree,  PF_qElemRemoveEx  pfQElemRemoveEx,  PF_qElemRemove  pfQElemRemove,  GENERIC_Q  *  pQ  )


			int iErr = -1;

			//
			char * qName = pCfg.common.name;
			char * mutexName_prefix = pCfg.common.mutexName_prefix;
			uint uiMaxCnt_semaTrigger = pCfg.common.uiMaxCnt_semaTrigger;
			uint uiMaxQNodes = pCfg.common.uiMaxQNodes;
			PF_qElemNewEx pfQElemNewEx = pCfg.pfQElemNewEx;
			PF_qElemNew pfQElemNew = pCfg.pfQElemNew;
			PF_qElemInit pfQElemInit = pCfg.pfQElemInit;
			PF_qElemExit pfQElemExit = pCfg.pfQElemExit;
			PF_qElemFreeEx pfQElemFreeEx = pCfg.pfQElemFreeEx;
			PF_qElemFree pfQElemFree = pCfg.pfQElemFree;
			PF_qElemRemoveEx pfQElemRemoveEx = pCfg.pf_QElemRemoveEx;
			PF_qElemRemove pfQElemRemove = pCfg.pfQElemRemove;

			string str_qName = new string(qName);
			char[] a_qName = str_qName.ToCharArray();
			string str_mutexName_prefix = new string(mutexName_prefix);
			char[] a_mutexName_prefix = str_mutexName_prefix.ToCharArray();

			if (pQ==null) return -1;

			//memset(pQ, 0, sizeof(GENERIC_Q));

			if (qName != null)
			{
				fixed (char* pName = pQ.cfg.name)
				{
					mytcsncpy(pName, str_qName, Consts.cntof_qCfg_name);
				}
			}
			pQ.cfg.uiMaxQNodes = uiMaxQNodes;

			//
			if (mutexName_prefix!=null)
			{
				fixed (char* pName = pQ.cfg.mutexName_prefix)
				{
					mytcsncpy(pName, str_mutexName_prefix, Consts.cntof_mutexName_prefix);
				}

				//  2009/05/14
				pQ.bNeedSync = true;

				/*
				__try  {
					InitializeCriticalSection(&pQ->cs);
				}
				__except(GetExceptionCode() == STATUS_NO_MEMORY)  {
					traceLogA("initGenericQ: InitializeCriticalSection failed.");
					goto errLabel;
				}
				pQ->bCsInited = TRUE;
				*/

				//pQ.syncObjParam.pMutexName = pQ.cfg.mutexName_prefix;
				fixed (char * pBuf = pQ.syncObjParam.mutexName, pName = pQ.cfg.mutexName_prefix)
                {
					mytcsncpy(pBuf, new string(pName), Consts.cntof_mutexName);
                }
				//pQ->syncObjParam.lpCriticalSection = &pQ->cs;

				//
				pQ.hMutex = new Mutex();
				if (pQ.hMutex == null) goto errLabel;
			}
			if (uiMaxCnt_semaTrigger!=0)
			{
				pQ.cfg.uiMaxCnt_semaTrigger = uiMaxCnt_semaTrigger;
				//if (!(pQ->hSemaTrigger = CreateSemaphore(NULL, 0, pQ->cfg.uiMaxCnt_semaTrigger, NULL))) goto errLabel;
				pQ.hSemaTrigger = new Semaphore(0, (int)pQ.cfg.uiMaxCnt_semaTrigger);
				if (pQ.hSemaTrigger == null) goto errLabel;
				//traceLog(_T("CreateSemaphore: %s sema"), pQ->cfg.name);
			}

			//  2015/08/16
			if (pCfg.common.bUseEvt)
			{
				pQ.cfg.bUseEvt = pCfg.common.bUseEvt;
				//
				//pQ->hEvent = CreateEvent(NULL, FALSE, FALSE, (pQ->cfg.evt0Name[0] ? pQ->cfg.evt0Name : NULL));
				fixed (char* pName = pQ.cfg.evt0Name)
				{
                    //pQ.hEvent = new EventWaitHandle(false, EventResetMode.AutoReset, new string(pName));
                    pQ.hEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
                    //
                    if (null == pQ.hEvent) goto errLabel;
				}
			}


			//
			pQ.pfQElemNewEx = pfQElemNewEx;
			pQ.pfQElemFreeEx = pfQElemFreeEx;
			//
			pQ.pfQElemNew = pfQElemNew;
			pQ.pfQElemFree = pfQElemFree;
			//
			pQ.pfQElemInit = pfQElemInit;
			pQ.pfQElemExit = pfQElemExit;
			//
			pQ.pfqElemRemoveEx = pfQElemRemoveEx;  //  2012/11/10
													//
			pQ.pfQElemRemove = pfQElemRemove;

			//
			pQ.pfqElemRemoveEx = pCfg.pf_QElemRemoveEx;


			//
			pQ.pFront = qNodeNew();
			if (pQ.pFront == null) goto errLabel;
			pQ.pRear = pQ.pFront;

			pQ.uiQNodes = 0;           //  ×¢Òâ£ºÕâÀï²»ÐèÒª¿¼ÂÇfront½Úµã£¬ËùÒÔ¼ÇÊýÆðµãÎª0

			//  2012/07/14
			pQ.bInited = true;

			//
			iErr = 0;

		errLabel:

			
			if(iErr != 0)
			{
				exitGenericQ(pQ);
			}
			

			//traceLogA("initGenericQ: %S %s", pQ->cfg.name, iErr ? "failed" : "ok");

			return iErr;

		}


		//
		public static unsafe  void exitGenericQ( GENERIC_Q pQ)
		{
			Q_NODE *pQNode;
			Q_NODE  *tmppQNode;

			if (pQ==null) return;
			if (pQ.pFront == null) return;

			for (pQNode = pQ.pFront->pNext; pQNode!=null;)
			{
				tmppQNode = pQNode->pNext;
				qNodeFree(pQ, pQNode, true);
				pQNode = tmppQNode;
			}
			pQ.pFront->pNext = null;
			qNodeFree(pQ, pQ.pFront, true);  pQ.pFront = null;
			
			//  2015/08/16
			if (pQ.hEvent!=null)
			{
				//CloseHandle(pQ->hEvent); pQ->hEvent = NULL;
				pQ.hEvent.Close();
				pQ.hEvent = null;
			}

			//
			if (pQ.hSemaTrigger!=null)
			{
				//CloseHandle(pQ->hSemaTrigger); pQ->hSemaTrigger = NULL;
				//traceLog(_T("CloseHandle: %s sema"), pQ->cfg.name);
				pQ.hSemaTrigger.Close();
				pQ.hSemaTrigger = null;
			}

			/*
			if (pQ->bCsInited)
			{
				DeleteCriticalSection(&pQ->cs);
			}
			*/
			if (pQ.hMutex!=null)
            {
				pQ.hMutex.Close();
				pQ.hMutex = null;
            }

			//traceLogA("exitGenericQ: %S", pQ->cfg.name);

			//memset(pQ, 0, sizeof(GENERIC_Q));

			return;

		}

		public static unsafe int qPostMsg(IntPtr pQElem, uint size, GENERIC_Q pQ)
		{
			return qPostMsgEx(pQElem, 0, size, pQ);
		}

		public static unsafe int qPostMsgEx(IntPtr pQElem, int iAllocType, uint size, GENERIC_Q pQ)
		{
			int iErr = -1;
			Q_NODE *pQNode = null;
			uint uiSeqNo = 0;

			//
			if (pQ == null)
			{
				traceLog("qPostMsgEx failed, pQ is null");
				return -1;
			}

			//
			using (  CQySyncObj syncObj = new CQySyncObj())
			{ 

				//if (pQ==null return -1;

				if (isQFull( pQ))
				{

					if (!pQ.cfg.b_showInfoQ)
					{
						string str;
						fixed (char* pName = pQ.cfg.name)
						{
							str = string.Format("qPostMsg failed: {0} is full, qNodes {1}", new string(pName), pQ.uiQNodes);
							showInfo_open(0, null, str);
						}
					}
					/*
	# ifdef  _UNICODE
					TCHAR tBuf[128];
					//_sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "qPostMsg failed: q is full. pid %d, threadId %d"  ),  GetCurrentProcessId(  ),  GetCurrentThreadId(  )  );
					_sntprintf(tBuf, mycountof(tBuf), _T("qPostMsg failed: q is full. nodes %d"), pQ->uiQNodes);
					qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("%s"), tBuf);
	#endif
					*/

					return Consts.CONST_qyRet_qFull;
				}

				if (null == (pQNode = qNodeNew())) return -1;

				if (pQ.pfQElemNewEx != null)
				{
					if (null == (pQ.pfQElemNewEx(iAllocType, (int)size, ref pQNode->qElemMemory))) goto errLabel;
				}
				else
				{
					if (pQ.pfQElemNew != null)
					{           //  2007/08/15, Ôö¼Ó¶ÔpfQElemNewµÄ·Ç0µÄÐ£Ñé
						if (null == (pQNode->qElemMemory.m_pBuf = pQ.pfQElemNew((int)size))) goto errLabel;
					}
				}
				if (pQNode->qElemMemory.m_pBuf != IntPtr.Zero && size != 0)
				{
					mymemcpy(pQNode->qElemMemory.m_pBuf, pQElem, size);
					pQNode->size = size;
				}
				//
				if (pQ.pfQElemInit != null && pQ.pfQElemInit(pQNode->qElemMemory.m_pBuf) != 0) goto errLabel;
				pQNode->ucbQElemInited = true;

				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex ) != 0)
				{
					//qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qPostMsg failed: sync failed"));
					goto errLabel;
				}

				if (isQFull( pQ))
				{
					//qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qPostMsg failed: q is full"));
					iErr = Consts.CONST_qyRet_qFull; goto errLabel;
				}


#if __DUAL_LINK_LIST__
				pQNode->pPrev = pQ.pRear;
#endif
				pQNode->pNext = null;

				pQ.pRear->pNext = pQNode;
				pQ.pRear = pQNode;

				pQ.uiQNodes++;

				return 0;

			}

		errLabel:

			if (pQNode!=null) qNodeFree(pQ, pQNode, false);

			return iErr;

		}


		//
		public static int qPostMsgAndTrigger(IntPtr pQElem, uint size, GENERIC_Q pQ)
		{
			int tmpiRet = -1;
			bool ucbTriggerErr = false;

			if (pQElem !=null&& size!= 0)
			{
				if ((tmpiRet = qPostMsg(pQElem, size, pQ))!=0) return tmpiRet;
			}
			/*
			if (pQ->hSemaTrigger && !ReleaseSemaphore(pQ->hSemaTrigger, 1, &pQ->lPrevCnt_semaTrigger))
			{
				qyDisplayLastError("ReleaseSemaphore failed");

				ucbTriggerErr = TRUE;

				traceLogA("%S qPostMsgAndTrigger err: uiQNodes %d, lPrevCnt_semaTrigger %d", pQ->cfg.name, pQ->uiQNodes, pQ->lPrevCnt_semaTrigger);
# ifdef  _UNICODE
				qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qPostMsgAndTrigger failed: trigger err, lPrevCnt %d"), pQ->lPrevCnt_semaTrigger);
#endif
			}
			else
			{
			}
		*/
				if (pQ.hSemaTrigger != null)
				{
				mySemaRelease(pQ.hSemaTrigger, 1);
				}
				//  2015/08/16
				if (pQ.hEvent != null)
				{
				//SetEvent(pQ->hEvent);
				myEventSet(pQ.hEvent);
				}
			
			//  2008/11/21
			if (ucbTriggerErr != pQ.ucbTriggerErr)
			{
				pQ.ucbTriggerErr = ucbTriggerErr;
			}

			return 0;
		}


		
		unsafe int qSendMsg(IntPtr pQElem, int size, GENERIC_Q pQ)
		{
			Q_NODE * pQNode = null;
			using (CQySyncObj syncObj = new CQySyncObj())
			{

				//if (null ==pQ) return -1;

				if (pQ.cfg.uiMaxQNodes != 0 && pQ.uiQNodes >= pQ.cfg.uiMaxQNodes)
				{
					//traceLogA("%S ÒÑ´ï½Úµã×ÜÊý×î´óÏÞ£¬qPostMsg failed", pQ->cfg.name);
					return -1;
				}

				if (null == (pQNode = qNodeNew())) return -1;

				if (pQ.pfQElemNewEx != null)
				{
					if (null == (pQ.pfQElemNewEx(0, size, ref pQNode->qElemMemory))) goto errLabel;
				}
				else
				{
					if (pQ.pfQElemNew != null)
					{
						if (null == (pQNode->qElemMemory.m_pBuf = pQ.pfQElemNew(size))) goto errLabel;
					}
				}
				if (pQNode->qElemMemory.m_pBuf != null && size != 0)
				{
					mymemcpy(pQNode->qElemMemory.m_pBuf, pQElem, (uint)size);
					pQNode->size = (uint)size;
				}
				//
				if (pQ.pfQElemInit != null && pQ.pfQElemInit(pQNode->qElemMemory.m_pBuf) != 0) goto errLabel;
				pQNode->ucbQElemInited = true;

				//  if  (  pQ->cfg.mutexName_prefix[0]  &&  syncObj.sync(  pQ->cfg.mutexName_prefix  )  )  goto  errLabel;
				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0) goto errLabel;

# if  __DUAL_LINK_LIST__
				pQNode->pPrev = pQ.pFront;
#endif
				pQNode->pNext = pQ.pFront->pNext;
				pQ.pFront->pNext = pQNode;

				pQ.uiQNodes++;

				return 0;
			}

		errLabel:

			if (pQNode!=null) qNodeFree(pQ, pQNode, false);
			return -1;

		}


		static unsafe int tmp_qGetMsg(GENERIC_Q pQ, IntPtr pQElem, ref uint puiSize, bool bNotShowInfo)
		{
			Q_NODE *pQNode = null;

			if ( null==pQElem || null==puiSize) return -1;

			{

				if ( null  ==  pQ.pFront->pNext) return -1;

				pQNode = pQ.pFront->pNext;
				//
				if (puiSize < pQNode->size)
				{
					if (!bNotShowInfo)
					{
						//qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qGetMsg failed:  *puiSize %d < pQNode->size %d"), *puiSize, pQNode->size);
					}
					return Consts.CONST_qyRet_bufTooSmall; //  -1;
				}
				//
				if (pQ.pFront->pNext->pNext!= null)
				{
					pQ.pFront->pNext = pQ.pFront->pNext->pNext;
#if  __DUAL_LINK_LIST__
					pQ.pFront->pNext->pPrev = pQ.pFront;
#endif
				}
				else
				{
					pQ.pFront->pNext = null;
					pQ.pRear = pQ.pFront;
				}

				pQ.uiQNodes--;

			}

			mymemcpy(pQElem, pQNode->qElemMemory.m_pBuf, pQNode->size);
			puiSize = pQNode->size;

			pQNode->ucbQElemInited = false;     //  2007/06/10, ÕâÀï½«ucbQElemInitedÖÃÎªFALSE£¬¾ÍÊÇ½«qElemExitµÄ¹¤×÷½»¸øÈ¡³ömsgµÄ³ÌÐòÀ´½øÐÐ

			qNodeFree(pQ, pQNode, false);
		
			return 0;

		}



		public static  unsafe int qGetMsg(GENERIC_Q pQ, IntPtr pQElem, uint* puiSize)
		{
			if (pQ == null) return -1;
			if ( null==pQElem || null==puiSize) return -1;

			using (CQySyncObj syncObj = new CQySyncObj())
			{

				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam,pQ.hMutex)!=0) return -1;

				return tmp_qGetMsg(pQ, pQElem, ref *puiSize, false);
			}
		}

		
		
		//  2011/07/15. 一次性读取多个包。
		//  pQElems[i]为buf
		//  *puiSizes[i]为size,输出为长度
		//  *pnQElems为最多一次性读取多少个包，输出真实读取的包数
		  public static unsafe int qGetMsgs(GENERIC_Q pQ, IntPtr[] pQElems, uint[] puiSizes, uint* pnQElems)
		{
			if (pQ == null) return -1;
			if ( null ==pQElems || null==puiSizes || null==pnQElems) return -1;

			int i;

			using (CQySyncObj syncObj = new CQySyncObj())
			{

				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam,pQ.hMutex)!=0) return -1;

				for (i = 0; i < *pnQElems; i++)
				{
					if (tmp_qGetMsg(pQ, pQElems[i], ref puiSizes[i], false)!=0) break;
				}
				*pnQElems = (uint)i;

			}
			//  OutputDebugString(  _T(  "qGetMsgs: Not finished\n"  )  );
			return ( i != 0 ) ? 0 : -1;
		}

		

		//  2011/12/10, 一次性读取多个包，并存放在一个pkg里
		//  *puiSizes[i]输出为长度
		//  *pnQElems为puiSizes的成员数，也因此表示最多一次性读取多少个包，输出真实读取的包数. 
		public static unsafe int qGetMsgPkg(GENERIC_Q pQ, IntPtr pPkg, uint uiSize_pkg, uint[] uiLens, uint* pnLens)
		{
			if (null == pQ) return -1;
			if ( null==pPkg || 0==uiSize_pkg || null==uiLens || null==pnLens) return -1;

			int i;
			bool bNotShowInfo = false;

			using (CQySyncObj syncObj = new CQySyncObj())
			{
				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam,pQ.hMutex)!=0) return -1;

				for (i = 0; i < *pnLens && uiSize_pkg != 0; i++)
				{
					uiLens[i] = uiSize_pkg;
					if (tmp_qGetMsg(pQ, pPkg, ref uiLens[i], bNotShowInfo)!=0) break;
					pPkg = IntPtr.Add(pPkg, (int)uiLens[i]);
					uiSize_pkg -= uiLens[i];
					bNotShowInfo = true;
				}
				*pnLens = (uint)i;

			}

			//  OutputDebugString(  _T(  "qGetMsgs: Not finished\n"  )  );
			return (i!=0) ? 0 : -1;
		}



		//
		static unsafe void tmpRemoveAQNode(GENERIC_Q pQ, Q_NODE *pQNode)
		{
			if ( null ==pQNode) return;

			if (pQNode == pQ.pFront) return;       //  ¶Ôfront½Úµã£¬²»ÄÜÒÆ³ý

			if (pQNode->pNext!=null)
			{
				pQNode->pNext->pPrev = pQNode->pPrev;
				if (pQNode->pPrev!=null) pQNode->pPrev->pNext = pQNode->pNext;
				else pQ.pFront->pNext = pQNode->pNext;
			}
			else
			{
				if (pQNode->pPrev!=null)
				{
					pQNode->pPrev->pNext = null;
					pQ.pRear = pQNode->pPrev;
				}
				else
				{
					pQ.pFront->pNext = null;
					pQ.pRear = pQ.pFront;
				}
			}

			pQNode->pNext = null;
			qNodeFree(pQ, pQNode, true);

			pQ.uiQNodes--;

			return;

		}


		  public static unsafe int qRemoveMsg(GENERIC_Q pQ, PF_bCommonHandler pf_bRemoveCond, object p0, object p1)
		{
			int iErr = -1;
			Q_NODE *pQNode = null;
			
			bool tmpbRet = false;
			Q_NODE *tmppQNode = null;

			if ( null==pf_bRemoveCond) return -1;


			using (CQySyncObj syncObj = new CQySyncObj())
			{


				//  if  (  pQ->cfg.mutexName_prefix[0]  &&  syncObj.sync(  pQ->cfg.mutexName_prefix  )  )  return  -1;
				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam,pQ.hMutex)!=0) return -1;

				for (pQNode = pQ.pFront->pNext; pQNode!=null;)
				{
					tmpbRet = pf_bRemoveCond(p0, p1, pQNode->qElemMemory.m_pBuf);
					if (!tmpbRet)
					{
						pQNode = pQNode->pNext; continue;
					}
					tmppQNode = pQNode;
					pQNode = pQNode->pNext;
					tmpRemoveAQNode(pQ, tmppQNode);
					continue;
				}

				iErr = 0;
			}

			//  errLabel:
			return iErr;

		}

		public static unsafe void emptyGenericQ( GENERIC_Q pQ)
		{
			Q_NODE *pQNode;
			Q_NODE  *tmppQNode;

			//if (!pQ) return;


			using (CQySyncObj syncObj = new CQySyncObj())
			{

				//  if  (  pQ->cfg.mutexName_prefix[0]  &&  syncObj.sync(  pQ->cfg.mutexName_prefix  )  )  goto  errLabel;
				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam,pQ.hMutex)!=0) goto errLabel;

				for (pQNode = pQ.pFront->pNext; pQNode!=null;)
				{
					//
					//if (pQ->hSemaTrigger) WaitForSingleObject(pQ->hSemaTrigger, 0);     //  ¶ÔÓÚÊ¹ÓÃ´¥·¢»úÖÆµÄ¶ÓÁÐ£¬ÏÈÒª°Ñ´¥·¢¸ø¼õÉÙÁË
					if (pQ.hSemaTrigger!=null) pQ.hSemaTrigger.WaitOne(0);     //  ¶ÔÓÚÊ¹ÓÃ´¥·¢»úÖÆµÄ¶ÓÁÐ£¬ÏÈÒª°Ñ´¥·¢¸ø¼õÉÙÁË
					//
					tmppQNode = pQNode->pNext;
					qNodeFree(pQ, pQNode, true);
					pQNode = tmppQNode;
				}

				pQ.pFront->pNext = null;
				pQ.pRear = pQ.pFront;
				pQ.uiQNodes = 0;
			}
		errLabel:

			return;

		}


		public static unsafe int qTraverse(GENERIC_Q pQ, PF_commonHandler pf_visit, object p0, object p1)
		{
			int iErr = -1;
			Q_NODE *pQNode = null;
			int tmpiRet = -1;


			if ( null==pf_visit) return -1;

			using (CQySyncObj syncObj = new CQySyncObj())
			{

				//  if  (  pQ->cfg.mutexName_prefix[0]  &&  syncObj.sync(  pQ->cfg.mutexName_prefix  )  )  return  -1;
				if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam,pQ.hMutex)!=0) return -1;

				for (pQNode = pQ.pFront->pNext; pQNode!=null; pQNode = pQNode->pNext)
				{
					tmpiRet = pf_visit(p0, p1, pQNode->qElemMemory.m_pBuf);
					if (tmpiRet!=0)
					{
						iErr = tmpiRet; goto errLabel;
					}
				}

			}

			iErr = 0;
		errLabel:
			return iErr;

		}

		//
		public static unsafe void genericQFree(GENERIC_Q pQ)
		{
			if (null==pQ) return;

			exitGenericQ(pQ);

			//free(pQ);

			return;

		}


	}
}