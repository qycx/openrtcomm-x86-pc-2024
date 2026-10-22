using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int tryToFreeSharedObjs(CCtxQmc pProcInfo)
		{

			if (null==pProcInfo) return -1;
			CCtxQyMc pQyMc = pProcInfo.pQyMc;    //  
			if (null==pQyMc) return -1;

			/*

# ifdef  __DEBUG__
			traceLogA("tryToFreeSharedObjs(  ) enters");
#endif

			//
			for (int index = 0; index < pProcInfo->cfg.usMaxCnt_pSharedObjs; index++)
			{
				QY_SHARED_OBJ* pSharedObj = getSharedObjByIndex(pProcInfo, index);

				if (!pSharedObj->bUsed) continue;

				//  try to find sharedObj whick need be stopped
				if (bSharedObjActive(pProcInfo, index))
				{

					//
					BOOL tmp_bExists = FALSE;
					if (!getVal_bExists_sharedObjUsr(pProcInfo, index, &tmp_bExists))
					{
						if (!tmp_bExists)
						{
# ifdef  __DEBUG__
							traceLogA("tryToFreeSharedObjs: objs[%d] no hWnd_starter, shall be stopped.", index);
#endif
							askSharedObjToStop(pProcInfo, pSharedObj, NULL, _T("tryToFreeSharedObjs. no usr"));
						}
					}

				}

				//
				if (!bSharedObjActive(pProcInfo, index))
				{
# ifdef  __DEBUG__
					traceLog(_T("freeSharedObjByIndex(  ): index is %d\n"), index);
#endif

					//  2014/09/28
					if (!pSharedObj->bSlave)
					{
						if (bExists_slave(pProcInfo, index))
						{
							QY_SHARED_OBJ* pSharedObj_master = pSharedObj;
							int iIndex_slave = pSharedObj_master->master.slaveKey.iIndex_sharedObj;
							QY_SHARED_OBJ* pSharedObj_slave = getSharedObjByIndex(pProcInfo, iIndex_slave);

							//
							shareDynBmp_closeTaskAv((SHARE_dyn_bmp*)pSharedObj_master->master.pShareDynBmp_slave);
							//
							askSharedObjToStop(pProcInfo, pSharedObj_slave, NULL, _T("tryToFreeSharedObjs. slave exists ."));
							continue;
						}
					}

					//
					freeSharedObjByIndex(pProcInfo, index);
					continue;
				}

				//
				continue;
			}

# ifdef  __DEBUG__
			traceLogA("tryToFreeSharedObjs(  ) leaves");
#endif
			*/

			return 0;
		}


		//
		public static unsafe int freeSharedObjAndSlave(object hWnd, int iWndContentType, int index)
		{
			int iErr = -1;

			CCtxQyMc pQyMc = g.g_pQyMc;    //  
			if (null == pQyMc) return -1;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			bool bDbg = true;



#if __DEBUG__
			traceLogA("tryToFreeSharedObjs(  ) enters");
#endif

			//

			QY_SHARED_OBJ pSharedObj = getSharedObjByIndex(pProcInfo, index);
				if (pSharedObj == null) goto errLabel;

				if (!pSharedObj.bUsed) {
					iErr = 0; goto errLabel;
				}

				//  try to find sharedObj whick need be stopped
				/*
				if (bSharedObjActive(pProcInfo, index))
				{

					//
					BOOL tmp_bExists = FALSE;
					if (!getVal_bExists_sharedObjUsr(pProcInfo, index, &tmp_bExists))
					{
						if (!tmp_bExists)
						{
# if  __DEBUG__
							traceLogA("tryToFreeSharedObjs: objs[%d] no hWnd_starter, shall be stopped.", index);
#endif
							askSharedObjToStop(pProcInfo, pSharedObj, NULL, _T("tryToFreeSharedObjs. no usr"));
						}
					}

				}
				*/

				//
				//if (!bSharedObjActive(pProcInfo, index))
				{
# if  __DEBUG__
					traceLog(_T("freeSharedObjByIndex(  ): index is %d\n"), index);
#endif

					//  2014/09/28
					if (!pSharedObj.var.bSlave)
					{
						if (bExists_slave(pProcInfo, index))
						{
							QY_SHARED_OBJ pSharedObj_master = pSharedObj;
							int iIndex_slave = pSharedObj_master.var.master.slaveKey.iIndex_sharedObj;
							QY_SHARED_OBJ pSharedObj_slave = getSharedObjByIndex(pProcInfo, iIndex_slave);

							//
							if (bDbg)
							{
								traceLog("freeSharedObjAndSlave: before shareDynBmp_closeTaskAv for slave");
							}

							//
							shareDynBmp_closeTaskAv(ref pSharedObj_master.var.master.pShareDynBmp_slave);
							//
							//askSharedObjToStop(pProcInfo, pSharedObj_slave, NULL, _T("tryToFreeSharedObjs. slave exists ."));
							//continue;
							freeSharedObjByIndex(hWnd,iWndContentType, iIndex_slave);
						}
					}

					//
					if (bDbg)
                {
					traceLog("freeSharedObjAndSlave: before freeSharedObjByIndex");
				}

				//
				freeSharedObjByIndex(hWnd,iWndContentType, index);

				//
				if (bDbg)
				{
					traceLog("freeSharedObjAndSlave: after freeSharedObjByIndex");
				}
			}

			//


			iErr = 0;

			errLabel:

# if  __DEBUG__
			traceLogA("tryToFreeSharedObjs(  ) leaves");
#endif
			

			return iErr;
		}


		public static unsafe int newCapProcInfoBmpIndex(CCtxQmc pProcInfo, int iIndex_sharedObj)
		{
			if (iIndex_sharedObj < 0 || iIndex_sharedObj >= pProcInfo.cfg.usMaxCnt_capProcInfoBmps) return -1;
			return iIndex_sharedObj;
		}

		public static unsafe int newCapProcInfoAudioIndex(CCtxQmc pProcInfo, int iIndex_sharedObj)
		{
			if (iIndex_sharedObj < 0 || iIndex_sharedObj >= pProcInfo.cfg.usMaxCnt_capProcInfoAudios) return -1;
			return iIndex_sharedObj;
		}

	}
}
