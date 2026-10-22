using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
 		//
		public struct NotifyTaskStatus_curNotify
        {
			public bool bShown;
			public bool bForegrounded;
			public object hParent;


		};



		//
		public unsafe struct NOTIFY_task_status
        {
			public bool bExists_task;
			public bool bAvCall;
			//
			public int index_taskInfo;

			//
			public NotifyTaskStatus_curNotify curNotify;

		};

		public unsafe struct PROCESSING_avTask
        {
			public bool bTaskExists;
			public int index_taskInfo;
			public int iTaskId;
        };



		//
		public class QY_MC_mainWndVar
		{                               //  2010/04/13
			//CCtxQyMc pQyMc;                           //  2016/09/21
													//  void						*				pProcInfo;

			//
			public uint uiTimerId;
				
			//
			public CMIS_MSGU pMsgBuf_msgArrive;                //  2009/12/10



			//WND_guiData_main guiData;

			//
			//
			public NOTIFY_task_status notifyTaskStatus;

			//
			public PROCESSING_avTask processingAvTask;

			//
			//unsigned char ucbVisible;                       //  2012/07/17

			public int loopCtrl;

			//

		//
		public CTX_caller ctxCaller;

		}
		;

	partial class qyFuncs
	{

	}
}

