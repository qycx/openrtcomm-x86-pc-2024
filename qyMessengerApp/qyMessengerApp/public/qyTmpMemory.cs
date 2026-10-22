using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe void clean_myPLAY_AUDIO_DATA(IntPtr p_p, string hint)
		{
			myPLAY_AUDIO_DATA* p = (myPLAY_AUDIO_DATA*)p_p;
			if (null==p) return;

			//  traceLogA(  "clean_myPLAY_AUDIO_DATA"  );

			//  MACRO_safeFree(  p->pBuf  );
			freeMemory(ref p->memory);

			return;
		}

		//
		//  2015/02/12
		public static unsafe void cleanEx_myPLAY_AUDIO_DATA(IntPtr p_p, int size, string hint)
		{
			myPLAY_AUDIO_DATA* p = (myPLAY_AUDIO_DATA*)p_p;
			int nPkts = size / sizeof(myPLAY_AUDIO_DATA);

			//TCHAR tBuf[128] = _T("");
			//_sntprintf(tBuf, mycountof(tBuf), _T("%s %s"), hint ? hint : _T(""), _T("Ex"));

			int i;
			for (i = 0; i < nPkts; i++)
			{
				void* p1 = &p[i];
				clean_myPLAY_AUDIO_DATA((IntPtr)p1, hint);
			}
			return;
		}



		//
		public static unsafe void clean_myDRAW_VIDEO_DATA(IntPtr p_p, string hint)
		{
			myDRAW_VIDEO_DATA* p = (myDRAW_VIDEO_DATA*)p_p;
			if (null==p) return;

			
			//  MACRO_safeFree(  p->pBuf  );
			freeMemory(ref p->memory);

			return;
		}

		//  2012/11/10
		public static unsafe void cleanEx_myDRAW_VIDEO_DATA(IntPtr p_p, int size, string hint)
		{
			myDRAW_VIDEO_DATA* p = (myDRAW_VIDEO_DATA*)p_p;
			int nPkts = size / sizeof(myDRAW_VIDEO_DATA);

			
			int i;
			for (i = 0; i < nPkts; i++)
			{
				void* pv = &p[i];
				clean_myDRAW_VIDEO_DATA((IntPtr)pv, hint);
			}
			return;
		}

		//  2011/12/15. player.displayQ用到了
		public static unsafe void clean_MIS_MSG_procVideo_qmc(IntPtr p_p, string hint)
		{
			MIS_MSG_procVideo_qmc* p = (MIS_MSG_procVideo_qmc*)p_p;
			if (null==p) return;

			//TCHAR tBuf[128] = _T("");
			//_sntprintf(tBuf, mycountof(tBuf), _T("%s %s"), hint ? hint : _T(""), _T("clean_MIS_MSG_procVideo_qmc: free"));


			if (p->uiType == Consts.CONST_misMsgType_procVideo_qmc)
			{
				//  2012/11/09
				//M_msg_showNo(*p, tBuf);
				//
				freeMemory(ref p->pkt.memory);
			}
			return;
		}

		public static unsafe void cleanEx_MIS_MSG_procVideo_qmc(IntPtr p_p, int size, string hint)
		{
			MIS_MSG_procVideo_qmc* p = (MIS_MSG_procVideo_qmc*)p_p;
			int nPkts = size / sizeof(MIS_MSG_procVideo_qmc);

			//TCHAR tBuf[128] = _T("");
			//_sntprintf(tBuf, mycountof(tBuf), _T("%s %s"), hint ? hint : _T(""), _T("Ex"));

			int i;
			for (i = 0; i < nPkts; i++)
			{
				MIS_MSG_procVideo_qmc* p1 = &p[i];
				clean_MIS_MSG_procVideo_qmc((IntPtr)p1, hint);
			}
			return;
		}




	}
}
