using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const int CONST_taskDataType_null = 0;
	public const int CONST_taskDataType_conf = 1;
	public const int CONST_taskDataType_transferFile = 2;

}


namespace qy
{
	public unsafe struct QmcTaskDataCommon_tmpStatus
	{
		public uint lastTickCnt_dec;
	}

	public abstract class QMC_taskData_common
	{
		public uint uiType;
		//
		//
		public MIS_MSGU msgU;

		//
		public QmcTaskDataCommon_tmpStatus tmpStatus;

		//
		public abstract void free();

	}
	;

	//
	public unsafe struct  TMP_desc
    {
		public fixed char desc[Consts.cntof_desc];
	};


	//
	public  class DLG_TALK_videoConferenceActiveMemFrom
	{
		//
#if DEBUG
		public TMP_tHintBuf test_tHintBuf0;
#endif

		//
		public AV_stream avStream;
		//
		//public fixed char desc[Consts.cntof_desc];
		public TMP_desc desc;

		//
		public uint dwLastTickCnt_aData;
		public uint dwLastTickCnt_vData;
		//
		public int nTimes_tooLong;

		//
#if DEBUG
		public TMP_tHintBuf test_tHintBuf1;
#endif

		//
#if true
		public int last_iPts_a;
		public int last_iPts_v;
		//
		public uint dwTickCnt_showPts_a;
		public uint dwTickCnt_showPts_v;
#endif

		//
		public void memset0()
        {

			//fixed (AV_stream* pAs = &avStream)
			{
				//qyFuncs.mymemset((IntPtr)(byte*)pAs, 0, sizeof(AV_stream));
				avStream = new AV_stream();
			}
			//fixed(TMP_desc  * pDesc=&desc)
            {
				//qyFuncs.mymemset((IntPtr)pDesc, 0, sizeof(TMP_desc));
				desc = new TMP_desc();
            }
			//
			this.dwLastTickCnt_aData = 0;
			this.dwLastTickCnt_vData = 0;
			this.nTimes_tooLong = 0;

			//
#if DEBUG
			this.test_tHintBuf0 = new TMP_tHintBuf();
			this.test_tHintBuf1 = new TMP_tHintBuf();

			//
			last_iPts_a = 0;
			last_iPts_v = 0;
#endif

			//
			return;
        }
	};

	//
	public struct  DLG_TALK_videoConference_requestingMem_avStream
    {
		public QY_MESSENGER_ID idInfo;
    };

	//
	public class  DLG_TALK_videoConference_requestingMems_from
    {
		//public AV_stream avStream;
		public DLG_TALK_videoConference_requestingMem_avStream avStream;
		//
		//public fixed char desc1[Consts.cntof_desc];
		public TMP_desc desc;

		//
		public void memset0()
        {
			return;
        }

	};


	//
	public  class DLG_TALK_videoConference
	{

		//
		public ushort usMaxSpeakers;
		public ushort usCntLimit_activeMems_from;
		public DLG_TALK_videoConferenceActiveMemFrom[] activeMems_from;//[MAX_speakers_taskAv + MAX_mosaicMems_taskAv + MAX_resourceMems_taskAv_conf];
		public bool bNeedRefresh_activeMems_from;

		//
		public DLG_TALK_videoConference_requestingMems_from[] requestingMems_from;//[MAX_requestingMems_videoConference];

				 
		//		
		//  Note: fromList 仅由界面使用。activeMems_from可以由其他线程使用		
		public ushort usCntLimit_mems_from;                           //  from	
	
		//
		public TASK_AV_FROM[] pMems_from1;

		//
		public uint uiTranNo_lastModified_pMems_from;              //  if  dwTickCnt_lastModified_pMems_from is not the same as the value is myTask, then the pMems_from will be retrieved from myTask...
		public bool bNeedRefresh_fromList;                         //  2015/02/17

        //
        //public int index_activeMems_from_forOther;


        //
#if DEBUG
        //public TMP_tHintBuf tHintBuf;
#endif

        //
        public DLG_TALK_videoConference()
		{
			DLG_TALK_videoConference videoConference = this;

            //
            int i;
            videoConference.activeMems_from = new DLG_TALK_videoConferenceActiveMemFrom[Consts.MAX_conf_activeMems];
            for (i = 0; i < videoConference.activeMems_from.Length; i++)
            {
                videoConference.activeMems_from[i] = new DLG_TALK_videoConferenceActiveMemFrom();
            }
            videoConference.requestingMems_from = new DLG_TALK_videoConference_requestingMems_from[Consts.MAX_conf_requestingMems];
            for (i = 0; i < videoConference.requestingMems_from.Length; i++)
            {
                videoConference.requestingMems_from[i] = new DLG_TALK_videoConference_requestingMems_from();
            }

			return;
        }


        //
        public void memset0()
        {
			this.usMaxSpeakers = 0;
			this.usCntLimit_activeMems_from = 0;
			//
			this.usCntLimit_mems_from = 0;

			//
			qyFuncs.showInfo_open(0, null, "DLG_TALK_videoConference.memset0 called");

			//
			return;
        }
	}
;


public struct QMC_taskData_conf_grp
    {
		public QY_MESSENGER_ID idInfo_initiator;

	};

	public struct QMC_taskData_conf_shareScreen
	{
		bool bTaskExists;
		int index_sharedObj;
		//
	};

	public class QMC_taskData_conf: QMC_taskData_common
	{
		//public uint uiType;

		
		public bool								bInited;

		//
		public bool ucbVideoConference;                           //  2009/05/30

		//
		public QMC_taskData_conf_grp			grp;

				 
		//		
		//public DLG_TALK_videoConference			videoConference;
		public int								ind_videoConference;
			
		//
		public bool								ucbNotSendData;                                               //  2009/02/22

		//
		public QMC_taskData_conf_shareScreen	shareScreen;

		//
		public object shared_eglContext;


        /*
				 //
				 unsigned short usCnt_pMems;
		CONF_mem* pMems;                                            //  是否显示的状态应该存在qmcWallMem里

		//
		unsigned short usMaxToDisplay;
		unsigned short usPollingIntervalInS;                            //  每一个批次显示的时间.

		//
		struct                 {
					 unsigned short usStartMemIndex;                                //  cur表示当前正试图显示，每次都从iStartMemIndex开始。一共显示maxToDisplay个. 		
		unsigned short usCnt;
		DWORD dwTickCnt_adjustWhichToBeDisplayed;               //  试图显示的操作时刻

		//
		DWORD dwTickCnt_chkIfActive;
		

	}
	cur_toDisplay;

		*/

        //  注意：如有需要，下面的动态分配也可以静态化.2022/01/13
        public QMC_taskData_conf(int index_taskInfo)
        {

            QMC_taskData_conf pTc = this;
			//
			pTc.uiType = Consts.CONST_taskDataType_conf;

			//
#if DEBUG
			qyFuncs.traceLog("QMC_taskData_conf() enters");
#endif


			//pTc.videoConference = new DLG_TALK_videoConference();
			pTc.ind_videoConference = GuiShare.pf_newIndex_DLG_TALK_videoConference(index_taskInfo);
			//
		DLG_TALK_videoConference videoConference = pTc.getVideoConference();
            //
            /*
            int i;
            videoConference.activeMems_from = new DLG_TALK_videoConferenceActiveMemFrom[Consts.MAX_conf_activeMems];
            for (i = 0; i < videoConference.activeMems_from.Length; i++)
            {
                videoConference.activeMems_from[i] = new DLG_TALK_videoConferenceActiveMemFrom();
            }
            videoConference.requestingMems_from = new DLG_TALK_videoConference_requestingMems_from[Consts.MAX_conf_requestingMems];
            for (i = 0; i < videoConference.requestingMems_from.Length; i++)
            {
                videoConference.requestingMems_from[i] = new DLG_TALK_videoConference_requestingMems_from();
            }
			*/


            //
#if DEBUG
            qyFuncs.traceLog("QMC_taskData_conf() leaves");
#endif

            //
            return;

        }

        public override void free()
        {
			//throw new NotImplementedException();
			
				QMC_taskData_conf pTc = (QMC_taskData_conf)this;
				DLG_TALK_videoConference videoConference = pTc.getVideoConference();

				//
				/*
				videoConference.activeMems_from = null;
				videoConference.requestingMems_from = null;
				videoConference.pMems_from1 = null;
				*/

				//
				//pTc.videoConference = null;
				GuiShare.pf_freeIndex_DLG_TALK_videoConference(ref pTc.ind_videoConference);
			

		}

        public DLG_TALK_videoConference getVideoConference()
        {
			return GuiShare.pf_getVideoConference(this.ind_videoConference);
        }

	}


	public class QMC_taskData_transferFile : QMC_taskData_common
	{
		//public uint uiType;


		//public bool bInited;

		//
		public uint dwStartTickCnt;                       //  
		public uint dwLastReportTickCnt;              //  

		//
		public QMC_taskData_transferFile()
        {
			this.uiType = Consts.CONST_taskDataType_transferFile;
        }


		//
		public override void free()
        {

        }

	}


    //
	public unsafe struct TmpErrMissing
	{
		public uint dwTickCnt_startToCalc;
		public int nTimes_errMissing;

	};


    /*
public unsafe struct QMC_taskInfo_var
	{
		public uint dwTickCnt_start;
		//
		//HWND hWnd_task;
		//
		public int iTaskId;

		//
		public void* pTaskData;

	};
	*/
    public struct QMC_taskInfo_var
	{
		public uint dwTickCnt_start;
		
		//
		public uint dwTickCnt_recv_lastRefreshed1;        // 最新收到数据的时刻,或者其他被更新状态的时刻
														//
		//
		public int iTaskId;

		//
		public bool m_bReplyTask;

		//				 
		public bool bTaskInvalid;								//  2009/09/14
																//
		public bool bNeedRemoved;								//  2015/09/08

		//
		public bool bNeedRemoved_toRecover;				//  2023/10/03

		//
		public bool bClosed;


		//
		public ROUTE_sendLocalAv curRoute_sendLocalAv;                                     //  2014/10/29



		//
		public TmpErrMissing tmpErrMissing;

		//
#if false
		public unsafe void memset0()
        {
			this = new QMC_taskInfo_var();
			//

			//
			return;
		}
#endif

		};


	public class QMC_TASK_INFO
	{
		public bool bUsed;

		//
		public QMC_taskInfo_var var;
		//
		public object hWnd_task;
		//
		//public QMC_taskData_common pTaskData;
		public int ind_pTaskData;

		//
		public TASK_respInfo taskRespInfo;

		//
		public QMC_taskData_common getTaskData()
        {
			//QMC_taskData_common pTaskData = (QMC_taskData_common)obj_pTaskData;
			QMC_taskData_common pTaskData = GuiShare.pf_getTaskData(ind_pTaskData);
			return pTaskData;
        }


	}
;

	partial class qyFuncs
    {

		//



		//


		//
		

		//
		public static  int newTaskInfoIndex( CTX_caller ctxCaller, CCtxQmc pProcInfo, uint uiTaskDataType, object hWnd_task, int iTaskId, string hint)
		{
			int iErr = -1;

			if (null==pProcInfo) return -1;
			//
			//QMC_taskData_common pTaskData = null;
			QMC_TASK_INFO  pTaskInfo = null; ;
			int i;

			bool bDbg = false;
			string str;

			//
			if (hint == null) hint = "";

			//
			//if (!IsWindow(hWnd_task)) return -1;
			if (0==iTaskId) return -1;

			//
			//tryToFreeTaskInfos(pProcInfo);

			i = getQmcTaskInfoIndexBySth(ctxCaller, pProcInfo, iTaskId);
			if(i>=0)
            {
				str = string.Format("newTaskInfoIndex failed, iTaskId already has a taskInfo");
				traceLog(str);
				return -1;
            }


			//  Note: start from 1.
			for (i = 1; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
			{
				pTaskInfo = pProcInfo.pQmcTaskInfos[i];
				if (null==pTaskInfo) continue;

				if (!pTaskInfo.bUsed) break;
			}
			if (i == pProcInfo.cfg.usMaxCnt_taskInfos) goto errLabel;

			//
			//MACRO_qyAssert(!pTaskInfo->var.pTaskData, _T("taskData is not null"));

			//
#if DEBUG
			qyFuncs.traceLog("newTaskInfoIndex.497. before new_QMC_taskData");
#endif

			//
#if false
			switch (uiTaskDataType)
            {
				case Consts.CONST_taskDataType_conf:
					pTaskInfo.obj_pTaskData = new QMC_taskData_conf(i);
					break;
				default:
					goto errLabel;
            }
			//pTaskData = (QMC_taskData_common)newQmcTaskData(uiTaskDataType);
			if (null==pTaskInfo.obj_pTaskData) goto errLabel;
#endif

			try
			{
				pTaskInfo.ind_pTaskData = GuiShare.pf_new_QMC_taskData(i, (int)uiTaskDataType);

				//
#if DEBUG
				traceLog("newTaskInfoIndex.521, after new_QMC_taskData");
#endif

				//
				//mymemset((IntPtr)&pTaskInfo->var, 0, sizeof(pTaskInfo->var)  );
				//pTaskInfo.var.memset0();
				//fixed( QMC_taskInfo_var * pvar=&pTaskInfo.var)
				{
					//mymemset((IntPtr)pvar, 0, sizeof(QMC_taskInfo_var));
					pTaskInfo.var = new QMC_taskInfo_var();
				}
				//
				pTaskInfo.var.iTaskId = iTaskId;
				pTaskInfo.var.dwTickCnt_start = myGetTickCount(null);
				pTaskInfo.var.dwTickCnt_recv_lastRefreshed1 = myGetTickCount(null);

				//
				pTaskInfo.hWnd_task = hWnd_task;
				//
				//pTaskInfo.pTaskData = pTaskData; pTaskData = null;
				//
				pTaskInfo.taskRespInfo = new TASK_respInfo();

			}
			catch(Exception e )
			{
				qyFuncs.showInfo_open(0, "", "newTaskInfoIndex exception");
				goto errLabel;
			}

			//
			//
			pTaskInfo.bUsed = true;

			//
			str=string.Format("newTaskInfo {0} . {1}", i, hint);
			showInfo_open(0, null, str);
#if __DEBUG__
			traceLog(_T("newTaskInfo, %d. %s"), i, hint);
#endif



			//
			iErr = 0;

		errLabel:

#if false
			if (pTaskData!=null)
			{
				freeQmcTaskData(pTaskData);
			}
#endif

			return iErr!=0 ? iErr : i;
		}


		//
		public static int freeTaskInfo(ref CTX_caller ctxCaller, CCtxQmc pProcInfo, int index_taskInfo, string hint)
		{
			int iErr = -1;

			if (null==pProcInfo) return -1;
			if (index_taskInfo < 0 || index_taskInfo >= pProcInfo.cfg.usMaxCnt_taskInfos) return -1;

			bool bDbg = false;
			string str;

			//
			if (hint == null) hint = "";

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)pProcInfo.getQmcTaskInfoByIndex(ctxCaller,index_taskInfo,"freeTaskInfo");
			if (null==pTaskInfo) return 0;
			//
			if (!pTaskInfo.bUsed) return 0;

			//	 	
			str = string.Format("freeTaskInfo {0} enters, {1}", index_taskInfo,hint);
			showInfo_open(0, null, str);

			//
			removeTask(ctxCaller, pTaskInfo.var.iTaskId);

			//
			GuiShare.pf_free_QMC_taskData(ref pTaskInfo.ind_pTaskData);

			//
			//memset(&pTaskInfo->var, 0, sizeof(pTaskInfo->var)  );

			//
			pTaskInfo.bUsed = false;

			//
			str =string.Format("freeTaskInfo {0} leaves ok.", index_taskInfo);
			showInfo_open(0, null, str);


			return 0;

		}


		public static QMC_TASK_INFO getQmcTaskInfoByIndex( CTX_caller ctxCaller, CCtxQmc pProcInfo, int index, string hint)
		{
			if (hint == null) hint = "";

			//
			//QMC_TASK_INFO* pTaskInfos = (QMC_TASK_INFO*)pProcInfo->pQmcTaskInfos;

			if (index < 0 || index >= pProcInfo.cfg.usMaxCnt_taskInfos) return null;

			QMC_TASK_INFO pTaskInfo = pProcInfo.pQmcTaskInfos[index];

			return pTaskInfo;
		}


		//
		//
		public static int getQmcTaskInfoIndexBySth(CTX_caller ctxCaller, CCtxQmc pProcInfo, int iTaskId)
		{
			int i;
			QMC_TASK_INFO pTaskInfo=null;

			if (iTaskId == 0) return -1;

			for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
			{
				pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo,i,"getQmcTaskInfoBySth");
				if (!pTaskInfo.bUsed) continue;
				if (pTaskInfo.var.iTaskId == iTaskId) break;
			}
			if (i == pProcInfo.cfg.usMaxCnt_taskInfos) return -1;

			return i;

		}

		//
		public static int qmcTaskInfo_setClosed( CTX_caller ctxCaller, CCtxQmc pProcInfo, int index_taskInfo, string hint)
        {
			QMC_TASK_INFO pTaskInfo = null;

			pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo, index_taskInfo,"qmcTaskInfo_setClosed");
			if (pTaskInfo == null) return -1;

			if (!pTaskInfo.bUsed) return -1;

			//
			removeTask(ctxCaller, pTaskInfo.var.iTaskId);

			//
			if (pTaskInfo.var.bClosed) return 0;
			
			//
			pTaskInfo.var.bClosed = true;
			//
			pTaskInfo.var.dwTickCnt_recv_lastRefreshed1 = myGetTickCount(null);

			//
			return 0;
		}

		public static bool qmcTaskInfo_bAlive(CTX_caller ctxCaller, CCtxQmc pProcInfo, int index_taskInfo)
        {
			bool bRet = false;
			QMC_TASK_INFO pTaskInfo = null;

			pTaskInfo = getQmcTaskInfoByIndex(ctxCaller,pProcInfo, index_taskInfo,"qmcTaskInfo_bAlive");
			if (pTaskInfo == null) goto errLabel;

			if (!pTaskInfo.bUsed) goto errLabel;

			if (pTaskInfo.var.bClosed) goto errLabel;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) goto errLabel;

			ref MIS_MSGU tmp_pMsg = ref pTaskData.msgU;

			if (tmp_pMsg.uiType != Consts.CONST_misMsgType_task) goto errLabel;

			if (!bTaskAlive(tmp_pMsg.task.iStatus)) goto errLabel;

			bRet = true;

			errLabel:
			return bRet;
		}

		//
		public static int freeTaskInfos(CCtxQmc pProcInfo)
		{
			int iErr = -1;
			int i;

			if (null==pProcInfo) return -1;
			//QY_MC* pQyMc = (QY_MC*)pProcInfo->pQyMc;
			//if (!pQyMc) return -1;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));
			ctxCaller.type = Consts.CONST_ctxCallerType_thread;
			ctxCaller.dwThreadId = myGetCurrentThreadId();
			ctxCaller.bTmp = true;

			//
			for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
			{
				QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)pProcInfo.getQmcTaskInfoByIndex(ctxCaller,i,"freeTaskInfo");
				if (null==pTaskInfo) continue;
				//
				if (!pTaskInfo.bUsed) continue;
				//
				if (0!=pTaskInfo.var.iTaskId)
				{
					//if  (  !bNotExists_taskInSharedObjs(  pProcInfo,  pTaskInfo->var.iTaskId  )  )  continue;
					//
					//if  (  !bNotExists_taskInTaskQs(  pProcInfo,  pTaskInfo->var.iTaskId  )  )  continue;
					/*
					BOOL  tmp_bExists  =  FALSE;
					if  (  getVal_bExists_taskInWnd(  pQyMc,  pTaskInfo->var.iTaskId,  pTaskInfo->var.hWnd_task,  &tmp_bExists  )  )  continue;
					if  (  tmp_bExists  )  continue;
					*/
				}

				//
				freeTaskInfo(ref ctxCaller, pProcInfo, i, "tryToFreeTaskInfos");
			}


			//
			iErr = 0;
		errLabel:
			return iErr;
		}



	}


}



