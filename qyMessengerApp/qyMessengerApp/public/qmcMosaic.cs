using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

    public struct QMC_mosaic_maker
	{

		/*
	struct                     {
					 int iTaskId;
}
param;
		*/
				 
				 //
				 public SHARE_dyn_bmp video;
public SHARE_dyn_bmp resource;
		/*
//
BOOL bQuit;             //  让readRtt线程退出标志
						//
HANDLE hEvent_syncQuit;
//
unsigned char ucCnt_threads;
struct                      {
					 HANDLE hThread;
DWORD dwThreadId;
}
threads[1];

//
struct                     {

}
status;
		*/

//  2016/02/14
public bool bInited;





}		 ;


	partial class qyFuncs
    {
		public static unsafe int stopMosaicThreads(ref QMC_mosaic_maker pMosaic)
		{
			int i;
			
			//
			//

			/*
			//
			pMosaic->bQuit = TRUE;

			//
			for (i = 0; i < pMosaic->ucCnt_threads; i++)
			{
				SetEvent(pMosaic->hEvent_syncQuit);
			}
			//
			BOOL bThreadExists = FALSE;
			int j;
			for (j = 0; j < 20; j++)
			{
				bThreadExists = FALSE;
				for (i = 0; i < pMosaic->ucCnt_threads; i++)
				{
					if (pMosaic->threads[i].hThread)
					{
						SetEvent(pMosaic->hEvent_syncQuit);
						waitForObject(&pMosaic->threads[i].hThread, 200);
						if (pMosaic->threads[i].hThread)
						{
							bThreadExists = TRUE;
						}
					}
				}
				if (!bThreadExists) break;
			}
			//
			if (pMosaic->hEvent_syncQuit)
			{
				CloseHandle(pMosaic->hEvent_syncQuit); pMosaic->hEvent_syncQuit = NULL;
			}

			//
			if (bThreadExists)
			{
				qyExitProcess(_T("stopMosaicThreads failed"));
			}
			*/

			return 0;

		}


	}

}
