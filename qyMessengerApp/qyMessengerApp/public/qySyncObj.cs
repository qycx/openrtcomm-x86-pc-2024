using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;



namespace qy
{


	partial class qyFuncs
	{

		public static bool bSyncQmObjQ(string mutexName, Mutex hMutex, uint uiTimeoutInMilliSeconds)
		{
			int iErr = -1;
			//Mutex hMutex = null;
			uint dwRet = 0;

			//if (mutexName == null) goto errLabel;
			if (hMutex == null)
			{
				goto errLabel;
			}

			//string strMutexName = new string(mutexName);
			bool createdNew;
			//
			//hMutex = new Mutex(false, mutexName, out createdNew);
			if (hMutex == null)
			{
				goto errLabel;
			}
			if (!hMutex.WaitOne((int)uiTimeoutInMilliSeconds))
			{
				goto errLabel;
			}


			iErr = 0;

		errLabel:

			if (iErr != 0)
			{

				/*
				if (hMutex != null)
				{
					hMutex.ReleaseMutex(); hMutex.Close(); hMutex = null;
					//ReleaseMutex(hMutex); CloseHandle(hMutex); hMutex = NULL;
				}
				*/

			}

			//return hMutex;
			return iErr!=0 ? false : true;
		}

		public static void cancelSyncQmObjQ(Mutex hMutex)
		{

			if (hMutex != null)
			{
				hMutex.ReleaseMutex(); 

				//ReleaseMutex(hMutex); CloseHandle(hMutex); hMutex = NULL;
			}
		}

	};


	public unsafe struct SYNC_OBJ_PARAM
	{
		public IntPtr lpCriticalSection;
		public fixed char mutexName[Consts.cntof_mutexName];
	}
	;


	public class CQySyncObj: IDisposable
	{
		//public struct  m_var 
		//{ 
		public IntPtr m_var_lpCriticalSection;
		public Mutex m_var_hMutex;
		public bool m_var_bLocked;
		private bool IsDisposed = false;
		//};

		//
		public bool m_bDbg = false;

		//
		public CQySyncObj() { }


		//  int  sync(  LPCTSTR  mutexName  )  {  m_var.hMutex  =  syncQmObjQ(  mutexName,  DEFAULT_milliseconds_syncQmObjQ  );  return  m_var.hMutex  ?  0  :  -1;  };
		public int sync(string mutexName, Mutex hMutex, uint uiMilliSeconds = 10 * 1000)
		{
			if (m_var_hMutex != null)
			{
				return -1;
			}
			//m_var_hMutex = qyFuncs.syncQmObjQ(mutexName, uiMilliSeconds);
			if (!qyFuncs.bSyncQmObjQ(mutexName, hMutex, uiMilliSeconds))
			{
				return -1;
			}
			m_var_hMutex = hMutex;
			return 0;	// (m_var_hMutex != null) ? 0 : -1;
		}


		public unsafe int sync(ref SYNC_OBJ_PARAM p, Mutex hMutex, uint uiMilliSeconds = 10 * 1000)
		{
			//if (p == null) return -1;
			if (m_var_bLocked) return -1;
			/*
						if (p->m_var_lpCriticalSection!= null)
					{
						if (0!=uiMilliSeconds)
						{
							if (!TryEnterCriticalSection(p->lpCriticalSection)) return -1;
						}
						else
						{
							EnterCriticalSection(p->lpCriticalSection);
						}
						m_var.lpCriticalSection = p->lpCriticalSection;
						m_var.bLocked = TRUE;
						return 0;
					}
			*/
			string str;
			fixed(char *pName=p.mutexName)
            {
				str = new string(pName);
            }
			return sync(str, hMutex, uiMilliSeconds);
		}

		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			//
			if  (  m_bDbg)
			{
				int ii = 0;
			}


			//
			if (!IsDisposed)
			{
				if (Disposing)
				{
					//Clean Up managed resources
				}
				//Clean up unmanaged resources  
				if (m_var_hMutex != null)
				{
					qyFuncs.cancelSyncQmObjQ(m_var_hMutex);
					m_var_hMutex = null;
				}

			}
			IsDisposed = true;
		}


		~CQySyncObj()
		{
			/*
		if (m_var.lpCriticalSection) LeaveCriticalSection(m_var.lpCriticalSection);
			*/

		
			Dispose(false);

		}

	};


}