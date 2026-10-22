using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
	public const int CONST_qnmMaxDbs = 32;  //  128
	public const string CONST_mutexName_qnmDb = "qnmDbMutex";

}

namespace qy
{




	public unsafe class QNM_DB_MEM
	{
		public object pDb;
		public bool ucbUsed;
		//  int			iStatus;
		public uint uiThreadId;
		public int iDsnIndex;      //  2006/12/22
		public int iDbType;        //  2012/07/14
	}
	;


//  2013/07/28
public unsafe class QNM_db_pool
	{
		public QNM_DB_MEM[] m_dbMems;	//[Consts.CONST_qnmMaxDbs];
		public int m_g_iCnt_dbMems;
	}
	;

    public unsafe class CQnmDb:IDisposable
{
	public CQnmDb()
        {
			m_pDbMem = null;
        }
		~CQnmDb()
        {
			Dispose(false);

        }

		//
		public QNM_DB_MEM m_pDbMem;
		bool IsDisposed = false;

		//
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
				if (null != m_pDbMem)
				{
					using (CQySyncObj syncObj = new CQySyncObj())
					{
						if (syncObj.sync(Consts.CONST_mutexName_qnmDb, g_m_hMutex_forPool) != 0) return;

						//
						m_pDbMem.ucbUsed = false;
					}
				}

			}
			IsDisposed = true;
		}





		//
		static Mutex g_m_hMutex_forPool = null;
		static QNM_db_pool g_pQnmDbPool = null;

		//
		public static int initDbPool()
		{
			int iErr = -1;

			//  if  (  m_ucbInited  )  return  0;
			if (null!=g_pQnmDbPool) return 0;

			//int size = sizeof(QNM_db_pool);
			g_pQnmDbPool = new QNM_db_pool();	// (QNM_db_pool*)mymalloc(size);
			if (null==g_pQnmDbPool) goto errLabel;
			//memset(g_pQnmDbPool, 0, size);

			g_pQnmDbPool.m_dbMems = new QNM_DB_MEM[Consts.CONST_qnmMaxDbs];
			g_pQnmDbPool.m_g_iCnt_dbMems = g_pQnmDbPool.m_dbMems.Length;    // mycountof(g_pQnmDbPool->m_dbMems);
			for ( int i = 0; i < g_pQnmDbPool.m_g_iCnt_dbMems; i++)
            {
				g_pQnmDbPool.m_dbMems[i] = new QNM_DB_MEM();
            }

			//
			g_m_hMutex_forPool = new Mutex(false);

			//  memset(  m_dbMems,  0,  sizeof(  m_dbMems  )  );
			//  m_ucbInited  =  TRUE;

			iErr = 0;

		errLabel:

			if (iErr!=0)
			{
				exitDbPool();
			}

			return 0;
		}

		public static unsafe void exitDbPool()
		{
			int i;
			//  CDatabase	*	pDb		=	NULL;
			void* pDb = null;

			//  if  (  !m_ucbInited  )  return;
			if (null==g_pQnmDbPool) return;

#if false
			for (i = 0; i < mycountof(g_pQnmDbPool->m_dbMems); i++)
			{
				if (!(pDb = (void*)g_pQnmDbPool->m_dbMems[i].pDb)) continue;
				//  qyCloseDb(  pDb  );  delete  pDb;
				qyFreeDb(&pDb);
			}
#endif

			//  m_ucbInited  =  FALSE;
			//MACRO_safeFree(g_pQnmDbPool);
			g_pQnmDbPool = null;

			//
			if (g_m_hMutex_forPool != null)
			{
				g_m_hMutex_forPool.Close();
				g_m_hMutex_forPool = null;
			}

			return;
		}


		public static unsafe int makeDbPool_myDb(CMyDb pDb, uint cnt, int iDsnIndex)
		{
			int iErr = -1;

			
			uint i;
			uint index = 0;


			using (CQySyncObj syncObj = new CQySyncObj()) {
				if (0!=syncObj.sync(Consts.CONST_mutexName_qnmDb,g_m_hMutex_forPool)) return -1;

				for (i = 0; i < cnt; i++)
				{

					//  取空闲的dbMems
					for (; index < g_pQnmDbPool.m_dbMems.Length; index++)
					{
						if (null==g_pQnmDbPool.m_dbMems[index].pDb) break;
					}
					if (index == g_pQnmDbPool.m_dbMems.Length)
					{
						//traceLogA("dbPool已满，makeDbPool失败"); 
						goto errLabel;
					}

					g_pQnmDbPool.m_dbMems[index].pDb = pDb;
					//g_pQnmDbPool.m_dbMems[index].uiThreadId = GetCurrentThreadId();
					g_pQnmDbPool.m_dbMems[index].iDsnIndex = iDsnIndex;                    //  2006/12/22
					g_pQnmDbPool.m_dbMems[index].iDbType = Consts.CONST_dbType_myDb;

				}

			}
			

			iErr = 0;

		errLabel:



			return iErr;

		}

		public static unsafe void freeDbPool(int iDsnIndex)  //  2006/12/22, 这个函数用来卸载某个数据源时用的
		{
			int i;

			
			//  if  (  !m_ucbInited  )  return;
			if (null==g_pQnmDbPool) return;

			for (i = 0; i < g_pQnmDbPool.m_dbMems.Length; i++)
			{
				if (g_pQnmDbPool.m_dbMems[i].iDsnIndex != iDsnIndex) continue;
				if (null==g_pQnmDbPool.m_dbMems[i].pDb) continue;
				if (g_pQnmDbPool.m_dbMems[i].iDbType == Consts.CONST_dbType_myDb)
				{
					//  不需要操作. 2012/07/14
					;
				}
				else
				{
#if false
					//  CDatabase	*	pDb		=	NULL;
					void* pDb = NULL;
					pDb = (void*)g_pQnmDbPool->m_dbMems[i].pDb;
					//  qyCloseDb(  pDb  );  delete  pDb;
					qyFreeDb(&pDb);
#endif
				}
				//memset(&g_pQnmDbPool->m_dbMems[i], 0, sizeof(g_pQnmDbPool->m_dbMems[i])  );
				g_pQnmDbPool.m_dbMems[i].pDb = null;
				g_pQnmDbPool.m_dbMems[i].iDbType = 0;

		}

	 return;

}


		public object getAvailableDb(int iDsnIndex)
		{
			int index = -1;

			if (null!=m_pDbMem) return m_pDbMem.pDb;

			//
			//  if  (  !m_g_dbMems  ||  m_g_iCnt_dbMems  <=  0  )  return  NULL;
			if (null==g_pQnmDbPool) return null;


			//
			using (CQySyncObj syncObj = new CQySyncObj()) {
				if (0!=syncObj.sync((Consts.CONST_mutexName_qnmDb),g_m_hMutex_forPool)) return null;

				for (index = 0; index < g_pQnmDbPool.m_g_iCnt_dbMems; index++)
				{
					if (null==g_pQnmDbPool.m_dbMems[index].pDb) continue;
					if (g_pQnmDbPool.m_dbMems[index].iDsnIndex != iDsnIndex) continue;
					//  if  (  m_dbMems[index].uiThreadId  !=  GetCurrentThreadId(  )  )  continue;
					//  traceLogA(  "2007/05/09, CDatabase应该可以支持多线程的调用，只要有同步就可以了, 这里把对threadId的考虑暂时去掉了"  );
					if (!g_pQnmDbPool.m_dbMems[index].ucbUsed) break;
				}

				if (index < g_pQnmDbPool.m_g_iCnt_dbMems)
				{
					m_pDbMem = g_pQnmDbPool.m_dbMems[index];
					m_pDbMem.ucbUsed = true;


				}

				//  errLabel:

			}

			if (null==m_pDbMem)
			{
				//traceLogA("CQnmDb::getAvailableDb failed, return  NULL.");
			}

			return m_pDbMem!=null ? m_pDbMem.pDb : null;
		}



		void showQnmDbInfo()
		{
			int i = 0;
			//  for  (  i  =  0;  i  <  m_
		}



	}
}
