using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

 static partial class Consts
{
	public const int CONST_uiMilliSeconds_sema_r = 10000;

	//
	public const int CONST_iAllocType_null = 0;     //  don't used.
													//  #define		CONST_iAllocType_tmpMemory							10		//  2010/09/18
	public const int CONST_iAllocType_user = 1000;	//  2010/09/18


}

namespace qy
{
	public unsafe class CQySyncCnt : IDisposable
	{                   //  2013/07/03. 多线程同步计数用
		bool IsDisposed = false;

		class syncCnt_m_var {
			public int[] plCnt;
			//
			public TMP_tHintBuf tHint;
			//public fixed char tHint[32];        //  2013/08/06. fot debug
			public bool bDebug;

		};

		syncCnt_m_var m_var;


		public CQySyncCnt() {
			m_var = new syncCnt_m_var();
		}

		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			if (!IsDisposed)
			{
				if (Disposing)
				{
					//Clean Up managed resources
				}
				//Clean up unmanaged resources  
				cancelSync();

			}
			IsDisposed = true;
		}




		~CQySyncCnt()
		{
			Dispose(false);
		}

		public int sync( int[] plCnt, string hint = null)
		{
			if (null == plCnt
				|| m_var.plCnt != null)
			{
				return -1;
			}
			if (plCnt.Length <= 0) return -1;
			//
			m_var.plCnt = plCnt;
			//
			long lRet = Interlocked.Increment(ref m_var.plCnt[0]);

			//
			return 0;
		}

		//
		void cancelSync()
		{
			if (m_var.plCnt != null)
			{
				long lRet = Interlocked.Decrement(ref m_var.plCnt[0]);


			}

		}

	};

	public struct RW_lock_param
	{
		public uint uiMaxCnt_sema;
		public uint uiInitCnt_sema;                                                //  2009/05/10				 		
		public uint uiMilliSeconds_mutex_r;
		public uint uiMilliSeconds_sema_r;
		public uint uiMilliSeconds_mutex_w;
		public uint uiMilliSeconds_sema_w;
	}
		;


	partial class qyFuncs
	{

		public static int RW_lock_param_init(ref RW_lock_param p)
		{
			//if (!p) return -1;

			//
			ref RW_lock_param rwLockParam = ref p;

			//	  	 
			rwLockParam.uiMaxCnt_sema = Consts.CONST_uiInitCnt_sema_q2SyncFlg;
			rwLockParam.uiInitCnt_sema = rwLockParam.uiMaxCnt_sema - 1;
			rwLockParam.uiMilliSeconds_mutex_r = 10000;
			rwLockParam.uiMilliSeconds_sema_r = 10000;
			rwLockParam.uiMilliSeconds_mutex_w = 10000;
			rwLockParam.uiMilliSeconds_sema_w = 10000;


			return 0;
		}


		//
		//
		//

		public static void qyExitProcess(string hint)
		{
			if (null==hint) hint = _T("qyExitProcess called");

			//MessageBox(NULL, hint, _T("Serious error."), MB_OK | MB_TASKMODAL);
			//ExitProcess(0);
			Thread.CurrentThread.Abort();

			return;
		}

	}


	public class CMutexRW
	{

		IntPtr m_pEnv;
		bool m_bSupported_srw;      //  2009/07/12

		Semaphore m_semReaders;
		Semaphore m_semWriters;
		int m_nReaders;

		//  2009/07/12
#if false
	SRWLOCK		m_srw;
#endif

#if false
		char buf[333];
#endif
		//
		public string w_hint;

		//
		public CMutexRW()
		{
			m_semReaders = null;
			m_semWriters = null;
			m_nReaders = 0;

			this.m_pEnv = (IntPtr)null;
			this.m_bSupported_srw = false;

			//this->m_pEnv = g_pEnv;

			if (this.m_bSupported_srw)
			{
				return;
			}

			//  initialize the Readers & Writers variables
			m_semReaders = new Semaphore(1, 1); //::CreateSemaphore(NULL, 1, 1, NULL);
			m_semWriters = new Semaphore(1, 1); //::CreateSemaphore(NULL, 1, 1, NULL);
			m_nReaders = 0;

			if (m_semReaders == null || m_semWriters == null)
			{
			}
		}
		~CMutexRW()
		{
			Close();

		}


		public void Close()
		{
			if (this.m_bSupported_srw)
			{
				return;
			}

			if (m_semWriters != null)
			{
				m_semWriters.Close();   //::CloseHandle(m_semWriters);
				m_semWriters = null;
			}
			if (m_semReaders != null)
			{
				m_semReaders.Close();   //(  ::CloseHandle(m_semReaders));
				m_semReaders = null;
			}

		}

		//  2012/03/24. CMutexRW主要作为CQyRWLock的数据存储,所以成员函数不能被独立调用
		//friend class CQyRWLock;


		public int Lock_DataRead(int uiMilliSeconds_sema = Timeout.Infinite)
		{
			int iErr = -1;

			if (this.m_bSupported_srw)
			{
				return 0;
			}

			//uint dwRet = 0;
			bool bRet;
			bool bWaited_r = false;

			//  P(  semReaders  )
			bRet = m_semReaders.WaitOne(Consts.CONST_uiMilliSeconds_sema_r);
			if (bRet == false) goto errLabel;
			//dwRet =  ::WaitForSingleObject(m_semReaders, CONST_uiMilliSeconds_sema_r);
			//if (dwRet == WAIT_FAILED || dwRet == WAIT_TIMEOUT) goto errLabel;
			//assert(dwRet == WAIT_OBJECT_0);
			bWaited_r = true;

			m_nReaders++;

			if (m_nReaders == 1)
			{
				//  P(  semWriters  )
				//dwRet =  ::WaitForSingleObject(m_semWriters, uiMilliSeconds_sema);
				//if (dwRet == WAIT_FAILED || dwRet == WAIT_TIMEOUT) goto errLabel;
				//assert(dwRet == WAIT_OBJECT_0);
				bRet = m_semWriters.WaitOne((int)uiMilliSeconds_sema);
				if (bRet == false) goto errLabel;
				//  if  (  dwRet  !=  WAIT_OBJECT_0  )  goto  errLabel;
			}

			iErr = 0;

		errLabel:

			//  V(  semReaders  )		
			if (bWaited_r) {

				//
				if ( iErr != 0)
                {
					m_nReaders--;
                }

				//
				//::ReleaseSemaphore(m_semReaders, 1, NULL);
				m_semReaders.Release(1);
			}
			return iErr;
		}



		public void Unlock_DataRead()
		{
			if (this.m_bSupported_srw)
			{
				return;
			}



			bool bRet;
			//DWORD dwEvent = WAIT_TIMEOUT;
			//  P(  semReaders  )
			//dwEvent =  ::WaitForSingleObject(m_semReaders, INFINITE);
			//assert(dwEvent == WAIT_OBJECT_0);
			bRet = m_semReaders.WaitOne(Timeout.Infinite);


			m_nReaders--;

			if (m_nReaders == 0)
			{
				//  V(  semWriters  )
				//(  ::ReleaseSemaphore(m_semWriters, 1, NULL));
				m_semWriters.Release(1);
			}
			//  V(  semReaders  )
			//(  ::ReleaseSemaphore(m_semReaders, 1, NULL));
			m_semReaders.Release(1);
		}

		public int Lock_DataWrite(int uiMilliSeconds_sema = Timeout.Infinite)
		{
			int iErr = -1;

			if (this.m_bSupported_srw)
			{
				return 0;
			}

			bool bRet;
			//DWORD dwRet = WAIT_TIMEOUT;

			//  P(  semWriters  )
			//dwRet =  ::WaitForSingleObject(m_semWriters, uiMilliSeconds_sema);
			//if (dwRet == WAIT_FAILED || dwRet == WAIT_TIMEOUT) goto errLabel;
			//assert(dwRet == WAIT_OBJECT_0);
			bRet = m_semWriters.WaitOne(uiMilliSeconds_sema);
			if (bRet == false) goto errLabel;
			//  if  (  dwEvent  !=  WAIT_OBJECT_0  )  goto  errLabel;

			iErr = 0;
		errLabel:
			return iErr;
		}



		public void Unlock_DataWrite()
		{
			if (this.m_bSupported_srw)
			{
				return;
			}

			// V(  semWriters  )
			//(  ::ReleaseSemaphore(m_semWriters, 1, NULL));
			m_semWriters.Release(1);
		}


	};


	public class CQyRWLock : IDisposable
	{
		CMutexRW m_pMutexRW;
		bool m_bIsLocked;

		bool m_bIsRlocked;

		private bool IsDisposed = false;

		public CQyRWLock()
		{
			//memset(&m_var, 0, sizeof(m_var));
		}



		~CQyRWLock()
		{
			Dispose(false);
		}


		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			if (!IsDisposed)
			{
				if (Disposing)
				{
					//Clean Up managed resources
				}
				//Clean up unmanaged resources  
				unlock();

			}
			IsDisposed = true;
		}




		// int  rLock(  LPCTSTR  mutexName,  unsigned  int  uiMilliSeconds_mutex,  HANDLE  hSema,  unsigned  int  uiMilliSeconds_sema  );
		//  int  wLock(  LPCTSTR  mutexName,  unsigned  int  uiMilliSeconds_mutex,  HANDLE  hSema,  unsigned  int  uiMilliSeconds_sema,  unsigned  int  uiInitCnt_sema  );  

		public int rLock(CMutexRW p, int uiMilliSeconds_sema)
		{
			int iErr = -1;

			if (null == p) return -1;

			if (m_bIsLocked) return -1;

			if (p.Lock_DataRead(uiMilliSeconds_sema) != 0) goto errLabel;

			m_pMutexRW = p;
			m_bIsLocked = true;
			m_bIsRlocked = true;

			iErr = 0;
		errLabel:
			return iErr;
		}

		public int wLock(CMutexRW p, int uiMilliSeconds_sema,string hint)
		{
			int iErr = -1;

			if (null == p) return -1;
			if (m_bIsLocked) return -1;

			if (p.Lock_DataWrite(uiMilliSeconds_sema) != 0) goto errLabel;

			//
			p.w_hint = hint;

			//
			m_pMutexRW = p;
			m_bIsLocked = true;

			iErr = 0;
		errLabel:
			return iErr;
		}



		public void unlock()
		{
			if (m_bIsLocked)
			{
				if (m_bIsRlocked) m_pMutexRW.Unlock_DataRead();
				else m_pMutexRW.Unlock_DataWrite();

				//
				if (!m_bIsRlocked)
                {
					m_pMutexRW.w_hint = "";
                }

				//
				//memset(&m_var, 0, sizeof(m_var));
				m_pMutexRW = null;
				m_bIsLocked = false;

				m_bIsRlocked = false;

			}
			return;
		}


	};



	public class CQyMalloc : IDisposable
	{                                   //  2008/03/29, ¿ÉÒÔ×Ô¶¯ÊÍ·ÅµÄ¶¯Ì¬·ÖÅäÀà¡£
		bool IsDisposed = false;

		private QY_MEMORY m_var;
		public unsafe CQyMalloc() {
			fixed (QY_MEMORY* pm_var = &m_var)
			{
				qyFuncs.mymemset((IntPtr)pm_var, 0, sizeof(QY_MEMORY));
			}
		}

		~CQyMalloc()
		{
			Dispose(false);
		}


		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			if (!IsDisposed)
			{
				if (Disposing)
				{
					//Clean Up managed resources
				}
				//Clean up unmanaged resources  
				qyFuncs.freeMemory(ref m_var);

			}
			IsDisposed = true;
		}


		public IntPtr mallocf(int size)
		{
			return qyFuncs.mallocMemory(0, size, ref m_var);
		}
		public IntPtr mallocf(int iAllocType, int size)
		{
			return qyFuncs.mallocMemory(iAllocType, size, ref m_var);
		}

		public void freef()
		{
			qyFuncs.freeMemory(ref m_var);
		}

		public int get(ref QY_MEMORY p)       //  2010/04/23
		{
			//if (!p) return -1;
			if (IntPtr.Zero == m_var.m_pBuf) return -1;
			//memcpy(p, &m_var, sizeof(p[0]));
			p = m_var;
			return 0;
		}

		public unsafe int detach()  //  2009/05/03
		{
			if (IntPtr.Zero == m_var.m_pBuf) return -1;
			//memset(&m_var, 0, sizeof(m_var));
			fixed (QY_MEMORY* pm_var = &m_var)
			{
				qyFuncs.mymemset((IntPtr)pm_var, 0, sizeof(QY_MEMORY));
			}
			return 0;
		}

		public int attach(ref QY_MEMORY p)  //  2009/12/07
		{
			//if (!p) return -1;
			if (m_var.m_pBuf != IntPtr.Zero || m_var.m_pVirtualData != IntPtr.Zero) return -1;
			//memcpy(&m_var, p, sizeof(m_var));
			m_var = p;
			return 0;
		}

	};


	/// <summary>
	/// /////////////////////////////////////////////////////////
	/// </summary>
	/// <param name="p0"></param>
	/// <param name="val"></param>
	/// <param name="puiTranNo"></param>
	/// <returns></returns>

	public class CQyMalloc_array : IDisposable
	{                                   //  2008/03/29, ¿ÉÒÔ×Ô¶¯ÊÍ·ÅµÄ¶¯Ì¬·ÖÅäÀà¡£
		bool IsDisposed = false;

		//
		public CQyMalloc[] mems;
		public uint m_n;

		public unsafe CQyMalloc_array(uint n)
		{
			if (n == 0) return;
			m_n = n;
			this.mems = new CQyMalloc[m_n];
			for (int i = 0; i < m_n; i++)
			{
				mems[i] = new CQyMalloc();
			}

		}

		~CQyMalloc_array()
		{
			Dispose(false);
		}


		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			if (!IsDisposed)
			{
				if (Disposing)
				{
					//Clean Up managed resources
				}
				//Clean up unmanaged resources  
				for (int i = 0; i < m_n; i++)
				{
					mems[i].Dispose();
				}
			}
			IsDisposed = true;
		}
	}




		//
		public unsafe delegate uint PF_getuiNextTranNo(object p0, int val, ref int puiTranNo);  //  2014/11/28
	

}
