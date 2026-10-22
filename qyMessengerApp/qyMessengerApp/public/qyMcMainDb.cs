using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int initQyMcDb(void* p0, void* p1, ref QY_OBJ_DB pObj)
		{
			int iErr = -1;

			//if (null==pObj) return -1;
			if (pObj.ucbLoaded) return -1;

			//  
			if (pObj.cfg.iDbType == Consts.CONST_dbType_myDb)
			{  //  2012/07/14
				try
				{
					pObj.pDb = new CMyDb();
					if (null == pObj.pDb) goto errLabel;
					CMyDb pDb = (CMyDb)pObj.pDb;
					fixed (char* pConnectStr = pObj.cfg.connectStr) { 
					if (!pDb.bQyOpenDb(new string(pConnectStr))) goto errLabel;
					}
					//
				}
				catch (  Exception e  )  {
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("initQyMcDb failed."));
					goto errLabel;
				}

				//
				if (CQnmDb.makeDbPool_myDb((CMyDb)pObj.pDb, 8, pObj.iDsnIndex)!=0) goto errLabel;

				}
	 else
				{
				/*
					try
					{

						pObj->pDb = qyNewDb();  //  new  CDatabase;
						if (!pObj->pDb) goto errLabel;
						if (!bQyOpenDb(pObj->cfg.connectStr, pObj->pDb))
						{
							debugLog("localDb.OpenEx( %s ) failed.", pObj->cfg.connectStr);
							goto errLabel;
						}
					}
					catch (  Exception e  )  {
						qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("initQyMcDb failed."));
						goto errLabel;
					}

					//
					if (CQnmDb::makeDbPool(pObj->cfg.connectStr, 8, pObj->iDsnIndex)) goto errLabel;
				*/
					}


					//  traceLogA( "localDb.OpenEx( %s ) success.", pQyMc->cfg.db.connectStr  );
					//traceLogA("localDb.OpenEx( ) success.");


					iErr = 0;

				errLabel:

					if (0!=iErr)
					{

						exitQyMcDb(p0, p1, ref pObj);

						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("qyMc"), 0, _T(""), _T(""), _T("initQyMcDb failed"));
					}

					if (0==iErr)
					{
						pObj.ucbLoaded = true;

						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("qyMc"), 0, _T(""), _T(""), _T("initQyMcDb ok"));
					}

					return iErr;

				}



				public static unsafe void exitQyMcDb(void* p0, void* p1, ref QY_OBJ_DB pObj)
				{
					CQnmDb.freeDbPool(pObj.iDsnIndex);    //  2007/01/02

					if (pObj.pDb!=null)
					{
						if (pObj.cfg.iDbType == Consts.CONST_dbType_myDb)
						{
							CMyDb pDb = (CMyDb)pObj.pDb;
							pDb.qyCloseDb();
							//delete pDb;
							pObj.pDb = null;
						}
						else
						{
							//qyFreeDb(&pObj->pDb);
						}
					}
					pObj.ucbLoaded = false;

				}

			}
		}
