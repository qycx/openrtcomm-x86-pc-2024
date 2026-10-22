using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

	public unsafe struct PROC_TASK_AV_video
	{
		public ushort usMaxFps_toShareBmp_conf;                   //  2009/06/04

        //public MIS_MSGU * pMsgU_toTransform;
        public CMIS_MSGU pMsgU_toTransform;
    };

	public unsafe struct PROC_TASK_AV_audio
	{

		public AUDIO_PLAY_CFG playCfg;                                    //  2009/06/14

		//
		//public MIS_MSGU* pMsgU_toTransform;
		public CMIS_MSGU pMsgU_toTransform;
	};

	public struct PROC_TASK_AV_confMosaicMaker
	{
		//
		public bool bExists_confMosaic;
		//
		public int iIndex_sharedObj_video;
		public int iIndex_sharedObj_resource;
	};

	public struct ProcTaskAv_applyForProxy_proxyReply_proxy
	{
		public uint uiTranNo_openAvDev_a;
		public uint uiTranNo_openAvDev_v;
	};

	public struct ProcTaskAv_applyForProxy_proxyReply
	{
		public ProcTaskAv_applyForProxy_proxyReply_proxy proxy;
	};

	public struct ProcTaskAv_applyForProxy
	{                                            //  2014/11/21. 要求proxy服务的客户端. 申请proxy的特点是有一个content_idInfo_logicalPeer
												 //
		public bool bRecvd_proxyReply;                            //  得到了响应
																  //
		public ProcTaskAv_applyForProxy_proxyReply proxyReply;

	};


	public struct ProcTaskAv_photomosaic
	{

		public QY_Q2 pOutputQ2;                                  //  2009/07/26

#if __DEBUG__
		TRANSFER_videoData_stat stat;										//  2011/01/26
#endif


	};

	public struct ProcTaskAv_mixer
	{


		public QY_Q2 pOutputQ2;

		public Int64 tLastTime_showFrameInfo;
		public ushort usCnt_pkts;


	};

#if false
	public struct PROC_TASK_AV_confMosaicTrans
	{
		//
		public bool bExists_confMosaic;                           //  for conference mems
														   //
		public PROC_conf_mosaic confMosaic_video1, confMosaic_resource;
	};
#endif

	//
	public struct ProcTaskAv_saveVideo
	{
		public uint uiObjType;                                    //  2014/08/08
		public ushort usIndex_obj;
		//
		public bool ucbSaveVideo_req;
		//unsigned  char								ucbSaveVideo_reply;							//  2014/09/07
		//
		public uint dwLastTickCnt_notification;
	};

	public struct ProcTaskAv_remoteAssist
	{
		public int maxxScreen;
		public int maxyScreen;

		//RA_POINTER_EVENT pointerEvent;	

	};


	public unsafe class PROC_TASK_AV
	{                                           //
		

		public object hWnd_task;                                 //  2009/09/09

		public VIDEO_CONFERENCE_CFG videoConferenceCfg;
		

		//
		public uint uiTaskType;                                    //  2017/09/08

		//
		public bool ucbSendLocalAv;                               //  2008/06/12
		public int iIndex_sharedObj;
		//  2014/12/11
		public int iIndex_sharedObjUsr;                        //  2014/12/11


		//  2014/09/24
		public int iIndex_taskInfo;                            //  2014/09/24

		//
		public bool ucbLocalSource_rtspStream;                    //  2014/03/16

		//
		public bool ucbStarter;                                   //  2010/08/31
		//public bool ucbVideoConference;                           //  2009/05/30
		public bool ucbVideoConferenceStarter;                    //

		//
		
		//
		public TASK_AV_FROM mem0_from;                                 //  the first member in mems_from array. 2009/02/15
		
		

		//  2014/08/04
		public ProcTaskAv_saveVideo 	saveVideo;
		


				 //  2014/11/10
		public PROC_TASK_AV_confMosaicMaker confMosaicMaker;


		//
#if false
		public PROC_TASK_AV_confMosaicTrans confMosaicTrans;
#endif


//
//  2014/11/21
public ProcTaskAv_applyForProxy applyForProxy;

/*
//
struct                                             {
					 BOOL bExists;
//
struct                                         {
						 time_t tStartTran;
uint uiTranNo;
					 }												proxy;					 
				 }													dlgTalkInProxy;
*/
//  2014/11/19
public struct ProcTaskAv_doProxy                                             {
					 //
public bool bExists_avProxy;
					 //
				 };
		public ProcTaskAv_doProxy doProxy;                                    //  2014/11/19. 用一个客户端为另一个客户端做转发。在mosaic需要这么做

		
//  2014/11/21
public struct  ProcTaskAv_receiver                                           {
					 //
public bool bRecvd_proxiedTranInfo;
//				
public TASK_AV_FROM proxied_from;									//  2014/11/21
				 };
		public ProcTaskAv_receiver receiver;

		//
		/*
		public ushort usCntLimit_transforms;						//  2009/05/12
		public QY_TRANSFORM[] pTransforms;							//  2009/05/12
		*/
		public int index_trans;
		public QisTrans trans;												

		//
		public ushort usCntLimit_dispatchs;							//  2011/12/08
		public QIS_DISPATCH[] pDispatchs;                                   //  2009/06/17

		
//
		public PROC_TASK_AV_video video;
		public PROC_TASK_AV_audio audio;


		//  
		public ProcTaskAv_photomosaic photomosaic;                                //  

//  
		public ProcTaskAv_mixer mixer;




		//  
		public ProcTaskAv_remoteAssist remoteAssist;

		/*
//  2016/02/29
struct                                             {
					 DWORD dwTickCnt_recentlyRefreshed;
//
BOOL bWaitingTo_beRe_accepted;
				 }													canceledTask_status;

		*/


//
bool bTaskInvalid;                              //  2009/09/14

		//
		public AV_TRAN_INFO reply_avTranInfo;


//fixed char debugHint[128];								//  2009/10/15

		public PROC_TASK_AV()
        {
			int ii = 0;

			//
			this.mem0_from = new TASK_AV_FROM();
			//
#if false
			this.confMosaicTrans.confMosaic_video1 = new PROC_conf_mosaic();
			this.confMosaicTrans.confMosaic_resource = new PROC_conf_mosaic();
#endif
			//
			return;
        }

		//
		public bool bVideoConference()
        {
			bool bRet = false;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, this.iIndex_taskInfo, "");
			if (pTaskInfo == null) goto errLabel;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) goto errLabel;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;

			bRet = pTc.ucbVideoConference;

			errLabel:

			return bRet;

		}

		//
#if false
		public QY_TRANSFORM getLastTransform()
        {
			if (bVideoConference())
			{
				return null;
			}
			//
			if (this.usCntLimit_transforms == 0) return null;
			//
			return this.pTransforms[this.usCntLimit_transforms - 1];
        }
#endif

}		 ;



//
//  2016/04/18.
/*
public unsafe struct TRANSFER_av_task
	{

	//
	//
	public TRANSFER_AV_INFO req;

	//
	public PROC_TASK_AV myTask;

}
;
*/



	public unsafe class TASK_transferAv:TASK_common
    {
		public PROC_TASK_AV myTask;

		//
		public TASK_transferAv()
        {
			myTask = new PROC_TASK_AV();

			return;
        }

    }


	public unsafe class TASK_transferFile:TASK_common
	{
		//M_taskCommon

		//
		public PROC_TASK_FILE  myTask;										

		//
		public TASK_transferFile()
        {
			myTask = new PROC_TASK_FILE();

        }
}
	;


//
//  
/*
public unsafe struct TRANSFER_AV_reply_task
	{                                   //  2007/07/06
									//
	public TRANSFER_AV_replyInfo req;

	//
	public PROC_TASK_AV myTask;

};
	*/
	


	partial class qyFuncs
{
		public static unsafe bool bApplyForAvProxy(MIS_CNT pMisCnt, ref TRANSFER_AV_INFO pTRANSFER_AV_INFO)
		{
			return ((pTRANSFER_AV_INFO).idInfo_logicalPeer.ui64Id !=0 && pMisCnt.idInfo.ui64Id != (pTRANSFER_AV_INFO).idInfo_logicalPeer.ui64Id);
		}

		//
		public static unsafe void M_getMutexName_syncActiveMem(ref QMC_cfg pQmcCfg, int iTaskId, char * mutexName, int mutexNameCnt  )
		{
			string str;
			fixed (char* pName = pQmcCfg.mutexName_syncActiveMem_prefix1)
			{
				//_sntprintf(mutexName, mutexNameCnt, _T("%s%d.%d"), (pQmcCfg)->mutexName_syncActiveMem_prefix1, GetCurrentProcessId(), iTaskId)
				str = string.Format("{0}{1}.{2}", new string(pName), 0, iTaskId);
			}
			return;
				
		}

		public static unsafe void M_getMutexName_syncActiveMem(ref QMC_cfg pQmcCfg, int iTaskId, ref TMP_tBuf128 mtx128)
		{
			fixed (char* mutexName = mtx128.tBuf)
			{
				int mutexNameCnt = Consts.cntof_tBuf128;

				M_getMutexName_syncActiveMem(ref pQmcCfg, iTaskId, mutexName, mutexNameCnt);
			}
			return;
		}





    }
}
