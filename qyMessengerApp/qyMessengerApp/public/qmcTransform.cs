using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
	public const string CONST_mutexName_syncIndex_dec = ("syncInd_dec");
	public const int MAX_cnt_preDec_array = 30;

}

namespace qy
{
	public struct TRANSFORM_AUDIO_procInfo_decInfo_pts
	{
		public uint dwTickCnt_start;         //  2015/01/15
		public uint uiPts_first;
		//
		//
		public uint uiPts_lastRecvd;           //  2015/01/15
	};

	public struct TRANSFORM_AUDIO_procInfo_decInfo_st
	{
		public int last_iSampleTimeInMs_preDec;
		//
		public int lastEnd_iSampleTimeInMs_postDec;

	};

	public struct TRANSFORM_AUDIO_procInfo_decInfo
	{

		//  2015/01/15
		public TRANSFORM_AUDIO_procInfo_decInfo_pts pts;

		//  2016/12/15
		public TRANSFORM_AUDIO_procInfo_decInfo_st st;

	public void	memset0(){

			pts = new TRANSFORM_AUDIO_procInfo_decInfo_pts();

            st=new TRANSFORM_AUDIO_procInfo_decInfo_st ();


        }

	};

	public unsafe struct TRANSFORM_AUDIO_procInfo_saveAudio
	{
		//
		public uint uiSize_pSaveAudioPkt;
		public IntPtr pSaveAudioPkt;
	};

	public struct TRANSFORM_AUDIO_procInfo_status
	{
		public bool ucbDecFailed;
	};

	//
	
	//  2009/05/12
	public  class TRANSFORM_AUDIO_procInfo:QY_qThreadProcInfo_common
	{
		//MACRO_qyQThreadProcInfo_common

		//
		public QY_qBuf2 qBuf2;


		//  uint								uiTranNo;										//  2009/04/27
		//  unsigned  short							usIndex;										//  2009/04/27
		//

		//public 	int index_pMems_from;                               //  2009/05/28
		public int index_activeMems_from;

		//
		public QY_MESSENGER_ID idInfo;
		public TRAN_audio ta;                                              //  2014/11/26


		//
		public uint uiModuleType_toDec;                                //  2016/12/05

		//
		/*
		union											{
					 struct                                     {
						 HACMDRIVERID hadid;

		HACMSTREAM hstr1;
		HACMSTREAM hstr2;
	}
	acm;
					 MY_DMO_INFO dmo;                                           //  2010/06/21

	//  QDC_OBJ_INFO							qdcObjInfo;		//  2015/09/30	//  custom;
	QOI_decA qoi;           //  2016/04/19

}
u;
		*/
		public QOI_decA u_qoi;


		//
		//  2016/12/10
		//public myPLAY_AUDIO_DATA* pCurPkt1_unused;
		public CmyPLAY_AUDIO_DATA pCurPkt1_unused;
//  

//  2015/02/14
public PKT_info_toDec curPktInfo_toDec;

        //
        //public WAVEFORMATEX* pWf_compress_unused;
        public CWAVEFORMATEX pWf_compress_unused;

        //public WAVEFORMATEX* pWf_pcm_unused;                                      //  pcm¸ñÊ½
        public CWAVEFORMATEX pWf_pcm_unused;                                      //  pcm¸ñÊ½
                                                                                  //public WAVEFORMATEX* pWf_decompress_unused;                                   //  pcm¸ñÊ½
        public CWAVEFORMATEX pWf_decompress_unused;                                   //  pcm¸ñÊ½

        public uint dwBufSize_pcm_unused;
		//public byte* pBuf_pcm_unused;
		public IntPtr pBuf_pcm_unused;
        
		
        //
        //myPLAY_AUDIO_DATA								pkt_dec;

        //  2015/05/19
        public TRANSFORM_AUDIO_procInfo_decInfo decInfo_unused;

//  2015/05/19
		public TRANSFORM_AUDIO_procInfo_saveAudio saveAudio;


//  2012/03/19
		public TRANSFORM_AUDIO_procInfo_status status;

//
public bool ucbInited;
		public TRANSFORM_AUDIO_procInfo()
        {
            decInfo_unused =new TRANSFORM_AUDIO_procInfo_decInfo();
        }
    }		 ;


	public unsafe struct TRANSFORM_VIDEO_procInfo_saveVideo
	{
		//
		public uint uiSize_pSaveVideoPkt;
		public void* pSaveVideoPkt;
	};


	public struct TRANSFORM_VIDEO_procInfo_status
	{
		public bool ucbSpsPpsImported;
		public bool ucbDecOk;
		public bool ucbDecFailed;
		//
		public bool bDumpStarted;
		//
		public bool ucbNeedReinited;    //  2014/04/07
	};


	public struct TRANSFORM_VIDEO_procInfo_decInfo_pts
	{
		public uint dwTickCnt_start;         //  2015/01/15
		public uint uiPts_first;
		//
		//
		public uint uiPts_lastRecvd;           //  2015/01/15
	};

	public struct TRANSFORM_VIDEO_procInfo_decInfo_lowLatency
	{
		public int min_lDiff_pre_post;
		public uint dwTickCnt_start;
	};

	public struct TRANSFORM_VIDEO_procInfo_decInfo_applyForKeyFrame
	{
		public uint dwLastTickCnt_applyForKeyFrame;
	};


	public unsafe class TRANSFORM_VIDEO_procInfo_decInfo
	{
		//
		public double dFrameRate_dec;             //  2014/04/22

		//
		public bool bGot_keyFrame;

		//  2015/01/15
		public TRANSFORM_VIDEO_procInfo_decInfo_pts pts;

		//
		//#define		MAX_iDiff_index_preDec								20
		//
		//public fixed int iSampleTimeInMs_preDec_array[Consts.MAX_cnt_preDec_array];
		//public fixed uint uiPts_preDec_array[Consts.MAX_cnt_preDec_array];      //  2015/02/19
																				//
		//public fixed uint dwTickCnt_doPretrans[Consts.MAX_cnt_preDec_array];       //  2015/02/24
																				   //
		public int iNextIndex_preDec;
		//
		public int cnt_preDec;
		//
		public int nTimes_needKeyFrame;                //  如果有多次出现 dwLastTickCnt_postDec  +  2000ms < GetTickCount(  ). 就认为是keyFrame丢了 。需要重新申请. 2014/12/06

		//  2014/06/24
		public int lDiff_pre_post;

		//  
		public int iSampleTimeInMs_postDec;
		public int cnt_postDec;
		//  2014/12/06
		public uint dwLastTickCnt_postDec;

		//  2014/06/28
		public TRANSFORM_VIDEO_procInfo_decInfo_lowLatency lowLatency;


		//  2014/08/28
		public TRANSFORM_VIDEO_procInfo_decInfo_applyForKeyFrame applyForKeyFrame;

		//
		public void memset0()
		{
			dFrameRate_dec = 0;
			bGot_keyFrame = false;
			pts = new TRANSFORM_VIDEO_procInfo_decInfo_pts();
			iNextIndex_preDec = 0;
			cnt_preDec = 0;
			nTimes_needKeyFrame = 0;
			lDiff_pre_post = 0;
			iSampleTimeInMs_postDec = 0;
			cnt_postDec = 0;
			dwLastTickCnt_postDec = 0;
			lowLatency = new TRANSFORM_VIDEO_procInfo_decInfo_lowLatency();
			applyForKeyFrame = new TRANSFORM_VIDEO_procInfo_decInfo_applyForKeyFrame();
        }
	
        };


	//
	//
	public  class TRANSFORM_VIDEO_procInfo:QY_qThreadProcInfo_common
{
        //MACRO_qyQThreadProcInfo_common

  
        //
        //public int index_pMems_from;                               //  2009/05/28
        public int index_activeMems_from;

		public QY_MESSENGER_ID idInfo;
		public TRAN_video tv;                                              //  2014/11/26


		//
		public uint dwLastmodifiedTickCnt_keyParams;              //  2014/08/05

		//
#if __DEBUG__
	//  uint						biSizeImage_decompress;							//  2009/06/01
#endif

		//  2012/09/09
		public uint uiModuleType_toDec;
		public bool bUseDecTool;                                   //  2015/01/19
		public ushort usPktResType;                                    //  2016/04/28

		//
		/*
		union											{
						 struct                                     {
							 HIC hicDecompress;
	}
	vcm;
	MY_DMO_INFO dmo;                                            //  2010/06/21

	//  QDC_OBJ_INFO							qdcObjInfo;		//  2015/09/30	
	QOI_decV qoi;			//  2016/04/19

					 }												u;
		*/
		public QOI_decV u_qoi;

		//
		//  2014/03/24
		//public myDRAW_VIDEO_DATA* pCurPkt1;

		//
		public PKT_info_toDec curPktInfo_toDec;


		//  
		public GENERIC_Q pInDataCacheQ;
		public GENERIC_Q pTmpDataQ;

//  2015/01/19
//DVT_cli dvtCli;


//  2014/04/22
public TRANSFORM_VIDEO_procInfo_decInfo decInfo;


		//  2014/08/04
		public TRANSFORM_VIDEO_procInfo_saveVideo saveVideo;

//  2012/03/19
		public TRANSFORM_VIDEO_procInfo_status status;

//
public bool ucbInited;



        public TRANSFORM_VIDEO_procInfo()
		{
			int ii = 0;

			//
			decInfo = new TRANSFORM_VIDEO_procInfo_decInfo();
		}

    }		 ;


public  class QY_TRANSFORM
	{
		//public void* pProcInfo;
		public MSGR_ADDR pAddr_logicalPeer1;
		public int iTaskId;
		public uint uiTaskType;                                        //  2012/03/25
		//public void* pTranInfo_unused;                             //  AV_TRAN_INFO  *
		public object pTask;                                            //  PROC_TASK_AV  *
                                                                        //public void* pMsgTask;                                     //  2014/08/27
        //public IntPtr pMsgTask1;                                     //  2014/08/27
		public int index_taskInfo;

        //
        //public void* tmp_pMsgU;
        public CMIS_MSGU tmp_pMsgU;
		public int sizeof_tmp_pMsgU;

		//
		//  void								*		pVIDEO_capDev_rtsp;								//  2014/03/25
		//public void* pCapDev_rtsp;                                 //  2014/03/25

		//
		//  2014/11/11					
		public bool bMosaicVideo;
		public bool bMosaicResource;
		//  2009/05/29
		public int index_activeMems_from;

		//
		public bool bNeed_decV;

		//
		public bool bDecV_forOther;

		//
		//public int index_activeMems_from_decV_bak;

		//
		public TRANSFORM_AUDIO_procInfo audio;

		//
		//  QY_qThreadProcInfo_common					v_preTransThread;								//  2011/12/09
		public PRETRANS_procInfo v_preTransThread;                             //  2015/02/25
																			   //
		public TRANSFORM_VIDEO_procInfo video;

		//  2014/08/04
		public SAVE_av_procInfo saveThread;                                        //  2014/08/04

		//  2011/07/03
		public int iIndex_player;                                  //  2009/05/02

		//  2015/06/18
		public TRANS_pts_info transPtsInfo;                                        //  2015/06/18


		//
		public bool m_bNoV;

		//
		public QY_TRANSFORM(bool bNoV )
        {
			this.audio = new TRANSFORM_AUDIO_procInfo();

			//
			m_bNoV = bNoV;

			//
			if (!bNoV)
			{
				this.v_preTransThread = new PRETRANS_procInfo();
				this.video = new TRANSFORM_VIDEO_procInfo();
			}

			//
			//this.saveThread = new SAVE_av_procInfo();

        }

		public int get_index_activeMems_from_decV()
        {
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			PROC_TASK_AV myTask = (PROC_TASK_AV)this.pTask;
			if (myTask.bVideoConference()) return this.index_activeMems_from;
			else
            {
#if false
				if ( this.bDecV_forOther)
                {
					int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, this.iTaskId);
					QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
					if (pTaskInfo == null) goto errLabel;
					QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
					if (pTaskData == null) goto errLabel;
					if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
					QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
					DLG_TALK_videoConference videoConference = pTc.getVideoConference();
					if (videoConference == null) goto errLabel;

					return videoConference.index_activeMems_from_forOther;
                }
#endif
				return this.index_activeMems_from;
            }

			//
			errLabel:
			//
			return -1;
        }
	}
	;



	//
	public class QisTrans
	{
		//
		public bool bUsed;

        //
        public ushort usMaxCntLimit_transforms;
        //
        public ushort usCntLimit_transforms;							//  2009/05/12
        public QY_TRANSFORM[] pTransforms;                              //  2009/05/12

		//


    };


	//
	






    /// <summary>
    /// ////////
    /// </summary>
#if false
	public class PROC_conf_mosaic
	{
		//MOSAIC_TRAN_INFO									mosaicTranInfo;
		//
		public TASK_AV_FROM taskAvFrom;
		//				
		public QY_TRANSFORM transform;

		//
		public PROC_conf_mosaic()
        {
			this.taskAvFrom = new TASK_AV_FROM();
			this.transform = new QY_TRANSFORM();
			return;
        }
	}
	;
#endif


    partial class qyFuncs
{
        //
        //public static unsafe int initTransform(CCtxQmc pProcInfo, MSGR_ADDR* pAddr_logicalPeer, int iTaskId, uint uiTaskType, ref PROC_TASK_AV pTask, MIS_MSG_TASK* pMsgTask, int i, QY_TRANSFORM p)
        public static int initTransform(CCtxQmc pProcInfo, ref MSGR_ADDR pAddr_logicalPeer, int iTaskId, uint uiTaskType, ref PROC_TASK_AV pTask, int index_taskInfo, int i, QY_TRANSFORM p)
        {
			int iErr = -1;
			CCtxQyMc  pQyMc  =  g.g_pQyMc;

			GENERIC_Q_CFG tmpCfg;

			CCtxQmcTmpl pCtx = pProcInfo;
			ref RW_lock_param pQyMc_rwLockParam = ref pQyMc.cfg.rwLockParam;// pCtx->get_qyMc_rwLockParam();
																	//if (!pQyMc_rwLockParam) return -1;
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;// (QMC_cfg*)pCtx->get_qmc_cfg();
			//if (!pQmcCfg) return -1;
			ref GENERIC_Q_CFG p_cfg_transformQ = ref pQmcCfg.transformQ;//Ctx->get_cfg_transformQ(  );
																		//if (!p_cfg_transformQ) return -1;
			string str;

			//
			//memcpy(&tmpCfg, p_cfg_transformQ, sizeof(tmpCfg));
			tmpCfg = p_cfg_transformQ;
			//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-a-%d-%d"), tmpCfg.name, iTaskId, i);
			/*
			str=string.Format("{0}-a-{1}-{2}", new string(tmpCfg.name), iTaskId, i);
			mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
			//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-a-%d-%d"), tmpCfg.mutexName_prefix, iTaskId, i);
			str=string.Format("{0}-a-{1}-{2}", new string(tmpCfg.mutexName_prefix), iTaskId, i);
			mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
			*/
			//  if  (  initGenericQ(  &tmpCfg,  mymalloc,  0,  0,  free,  &p.audio.q  )  )  goto  errLabel;
			if (0!=initQyQ2(tmpCfg, pQyMc_rwLockParam, 2, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, p.audio.q2)) goto errLabel;
			p.audio.bQInited = true;
			//
			p.audio.qBuf2 = new QY_qBuf2();
			int maxDataBufLen = (int)(128000 * 2.5 / 8);//  3 secends mp3
			if (0!=p.audio.qBuf2.initQyQ2(Consts.CONST_qType_decA, tmpCfg.uiMaxQNodes, maxDataBufLen, pQyMc_rwLockParam, true,"")) goto errLabel;


			if (!p.m_bNoV)
			{

				//
				//memcpy(&tmpCfg, p_cfg_transformQ, sizeof(tmpCfg));
				tmpCfg = p_cfg_transformQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-v-%d-%d"), tmpCfg.name, iTaskId, i);
				/*
				str=string.Format("{0}-v-{1}-{2}", new string(tmpCfg.name), iTaskId, i);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-v-%d-%d"), tmpCfg.mutexName_prefix, iTaskId, i);
				str=string.Format("{0}-v-{1}-{2}", new string(tmpCfg.mutexName_prefix), iTaskId, i);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
				*/
				//  if  (  initGenericQ(  &tmpCfg,  mymalloc,  0,  0,  myfree,  &p.video.q  )  )  goto  errLabel;
				if (0 != initQyQ2(tmpCfg, pQyMc_rwLockParam, 2, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, (PF_qElemRemove)clean_myDRAW_VIDEO_DATA, p.video.q2)) goto errLabel;
				p.video.bQInited = true;

				//

				//  2011/12/09
				//  memcpy(  &tmpCfg,  &pProcInfo->cfg.preTransformQ,  sizeof(  tmpCfg  )  );
				//memcpy(&tmpCfg, p_cfg_transformQ, sizeof(tmpCfg));
				tmpCfg = p_cfg_transformQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-pv-%d-%d"), tmpCfg.name, iTaskId, i);
				/*
				str=string.Format("{0}-pv-{1}-{2}", new string(tmpCfg.name), iTaskId, i);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-pv-%d-%d"), tmpCfg.mutexName_prefix, iTaskId, i);
				str=string.Format("{0}-pv-{1}-{2}", new string(tmpCfg.mutexName_prefix), iTaskId, i);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
				*/
				//  
				if (0 != initQyQ2(tmpCfg, pQyMc_rwLockParam, 2, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, p.v_preTransThread.q2)) goto errLabel;
				p.v_preTransThread.bQInited = true;

				//
				p.v_preTransThread.qBuf2 = new QY_qBuf2();
				maxDataBufLen = (int)(2000000 * 1.2 / 8);//  1.2 * 2 secends video
														 //
				if (pProcInfo.cfg.b_useVideoQ2_toDecV) maxDataBufLen = 1;
				//
				if (0 != p.v_preTransThread.qBuf2.initQyQ2(Consts.CONST_qType_decV, tmpCfg.uiMaxQNodes, maxDataBufLen, pQyMc_rwLockParam, true, "")) goto errLabel;
			}


            //  2014/0804
            if (pTask.saveVideo.ucbSaveVideo_req)
			{
				//memcpy(&tmpCfg, p_cfg_transformQ, sizeof(tmpCfg));
				tmpCfg = p_cfg_transformQ;
				//_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s-s-%d-%d"), tmpCfg.name, iTaskId, i);
				/*
				str=string.Format("{0}-s-{1}-{2}", new string(tmpCfg.name), iTaskId, i);
				mytcsncpy(tmpCfg.name, str, Consts.cntof_qCfg_name);
				//_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s-s-%d-%d"), tmpCfg.mutexName_prefix, iTaskId, i);
				str=string.Format("{0}-s-{1}-{2}", new string(tmpCfg.mutexName_prefix), iTaskId, i);
				mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
				*/
				//  
				if (0!=initQyQ2(tmpCfg, pQyMc_rwLockParam, 2, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, p.saveThread.q2)) goto errLabel;
				p.saveThread.bQInited = true;
			}

			//
			//p.pProcInfo = pProcInfo;
			p.pAddr_logicalPeer1 = pAddr_logicalPeer;
			p.iTaskId = iTaskId;
			p.uiTaskType = uiTaskType; //  2012/03/25
			//p.pTranInfo_unused = pTranInfo_unused;     //  2014/11/11
			p.pTask = pTask;
			//p.pMsgTask = (IntPtr)pMsgTask;     //  2014/08/27
			p.index_taskInfo = index_taskInfo;
										//
			p.index_activeMems_from = i;

			//
			if ( p.tmp_pMsgU!=null)
            {
				showInfo_open(0, "", "initTransform failed, tmp_pMsgU is not null");
				goto errLabel;
            }
			p.sizeof_tmp_pMsgU = sizeof_MIS_MSGU();
			p.tmp_pMsgU = new CMIS_MSGU();// (void*)mymalloc(p.sizeof_tmp_pMsgU);
			if (p.tmp_pMsgU == null) goto errLabel;


			//
			if (pTask.bVideoConference())
            {
				if ( p.index_activeMems_from==0)
                {
					p.bNeed_decV = true;
                }
            }
			else
            {
				// let the last transform to dec video
				//if ( i==pTask.usCntLimit_transforms - 1)
                {
					p.bNeed_decV = true;
					//
					p.bDecV_forOther = true;
                }				
            }

			//
			iErr = 0;
		errLabel:
			return iErr;

		}


		public static void exitTransform(ref PROC_TASK_AV pTask, QY_TRANSFORM p)
		{
			if (p == null) return;

			//
			if (p.tmp_pMsgU != null)
            {
				//myfree((IntPtr)p.tmp_pMsgU); 
				p.tmp_pMsgU = null;
            }


			//  2014/08/04
			if (p.saveThread != null)
			{
				if (p.saveThread.bQInited)
				{
					exitQyQ2(p.saveThread.q2);
					p.saveThread.bQInited = false;
				}
			}

			//  2011/12/09
			if (p.v_preTransThread != null)
			{
				if (p.v_preTransThread.bQInited)
				{
					exitQyQ2(p.v_preTransThread.q2);
					p.v_preTransThread.bQInited = false;
				}

				//
				if (p.v_preTransThread.qBuf2 != null)
				{
					p.v_preTransThread.qBuf2.exitQyQ2();
					p.v_preTransThread.qBuf2 = null;
				}
			}

			//
			if (p.audio != null)
			{
				if (p.audio.qBuf2 != null)
				{
					p.audio.qBuf2.exitQyQ2();
					p.audio.qBuf2 = null;
				}
				if (p.audio.bQInited)
				{
					exitQyQ2(p.audio.q2);
					p.audio.bQInited = false;
				}
			}
			if (p.video != null)
			{
				if (p.video.bQInited)
				{
					exitQyQ2(p.video.q2);
					p.video.bQInited = false;
				}
			}

			return;
		}


        //void  exitTransforms(  PROC_TASK_AV  *  pTask  );

        // int  initTransforms(  MC_VAR_isCli  *  pProcInfo,  MSGR_ADDR  *  pAddr_logicalPeer,  int  iTaskId,  uint  uiTaskType,  AV_TRAN_INFO  *  pTranInfo,  PROC_TASK_AV  *  pTask  )
        //public static unsafe int initTransforms(CCtxQmc pProcInfo, MSGR_ADDR* pAddr_logicalPeer, int iTaskId, uint uiTaskType, ref PROC_TASK_AV pTask, MIS_MSG_TASK* pMsgTask)
        public static  int initTransforms(CCtxQmc pProcInfo, ref MSGR_ADDR pAddr_logicalPeer, int iTaskId, uint uiTaskType, ref PROC_TASK_AV pTask, int index_taskInfo)
        {
			int iErr = -1;

			//

			//QY_MC	*	pQyMc	=	(  QY_MC  *  )pProcInfo->pQyMc;
			int i;
			int len;
			//  DWORD		dwThreadDaemonId;

			if (0==pTask.trans.usCntLimit_transforms) return 0;
			if (pTask.trans.pTransforms==null)
			{
				traceLogA("initTransforms failed, pTransforms is not null");
				//
				return -1;
			}
			if  (  pTask.trans.usCntLimit_transforms  >pTask.trans.usMaxCntLimit_transforms)					
			{
				traceLog("initTransforms failed, cntLimit_trans > usMaxCntLimit_trans");
				//
				return -1;
			}

			//
#if DEBUG
			qyFuncs.traceLog("initTransforms.616");
#endif

			/*
			 len = pTask.usCntLimit_transforms * sizeof(QY_TRANSFORM);
			if (!(pTask.pTransforms = (QY_TRANSFORM*)mymalloc(len))) goto errLabel;
			memset(pTask.pTransforms, 0, len);
			*/
			/*
			pTask.trans.pTransforms = new QY_TRANSFORM[pTask.trans.usCntLimit_transforms];
            qyFuncs.traceLog("initTransforms.643");
            for ( i=0;i < pTask.trans.usCntLimit_transforms; i++)
            {
                qyFuncs.traceLog("initTransforms.647  "+i);
                pTask.trans.pTransforms[i] = new QY_TRANSFORM();
            }
            qyFuncs.traceLog("initTransforms.647");
			*/

            //
#if DEBUG
            qyFuncs.traceLog("initTransforms.633");
#endif

			//
            for (i = 0; i < pTask.trans.usCntLimit_transforms; i++)
			{
				QY_TRANSFORM p = pTask.trans.pTransforms[i];

				if (0!=initTransform(pProcInfo, ref pAddr_logicalPeer, iTaskId, uiTaskType, ref pTask, index_taskInfo, i, p)) goto errLabel;
			}

			//  2014/11/11
#if false
			if (pTask.ucbVideoConference
				&& !pTask.ucbVideoConferenceStarter)
			{
				//  2014/11/11
				if (0!=initTransform(pProcInfo, pAddr_logicalPeer, iTaskId, uiTaskType, pTranInfo_unused, ref pTask, pMsgTask, 0, pTask.confMosaicTrans.confMosaic_video1.transform)) goto errLabel;
				pTask.confMosaicTrans.confMosaic_video1.transform.bMosaicVideo = true;

				//
				if (0!=initTransform(pProcInfo, pAddr_logicalPeer, iTaskId, uiTaskType, pTranInfo_unused, ref pTask, pMsgTask, 0, pTask.confMosaicTrans.confMosaic_resource.transform)) goto errLabel;
				pTask.confMosaicTrans.confMosaic_resource.transform.bMosaicResource = true;

				//
				pTask.confMosaicTrans.bExists_confMosaic = true;
			}
#endif

			//  
			iErr = 0;

		errLabel:
			if (0!=iErr)
			{
				exitTransforms(pProcInfo, ref pTask);
			}

			return iErr;
		}

		public static void exitTransforms(CCtxQmc pProcInfo, ref PROC_TASK_AV pTask)
		{
			int i;

			if (0==pTask.trans.usCntLimit_transforms) return;
			if (null==pTask.trans.pTransforms) return;

			stopTransformThreads(pProcInfo, ref pTask,"exitTransforms");

			//  2014/11/11
#if false
			exitTransform(ref pTask, pTask.confMosaicTrans.confMosaic_video1.transform);
			exitTransform(ref pTask, pTask.confMosaicTrans.confMosaic_resource.transform);
#endif


			//
			for (i = 0; i < pTask.trans.usCntLimit_transforms; i++)
			{
				QY_TRANSFORM p = pTask.trans.pTransforms[i];

				exitTransform(ref pTask, p);
			}

			//free(pTask.pTransforms); pTask.pTransforms = null;
			//pTask.trans.pTransforms = null;

			return;
		}






		///////////////
		/// <summary>
		/// 
		public static int startTransformThread_v(CCtxQmc pProcInfo, QY_TRANSFORM p,string hint)
		{
			string str;

			//
			ref QMC_status pStatus = ref pProcInfo.status;// Ctx.get_qmc_status();
			if (hint == null) hint = "";

			//
			if (!p.m_bNoV)
			{
				//
				if (null == p.video.hThread)
				{
					str = string.Format("startTrans_v {0}", hint);
					qyFuncs.showInfo_open(0, null, str);

					//
					p.video.bQuit = false;
					/*
					p.video.hThread = CreateThread(NULL, 0, mcThreadProc_decVideo, p, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!p.video.hThread) goto errLabel;
					p.video.dwThreadId = dwThreadDaemonId;
					if (ResumeThread(p.video.hThread) == -1) goto errLabel;
					*/
#if false
				p.video.hThread = new Thread(new ParameterizedThreadStart(mcThreadProc_decVideo));
				p.video.hThread.Start(p);
#endif
					GuiShare.pf_startThread1(ref p.video.hThread, mcThreadProc_decVideo, p);

					//  2012/03/25
					//InterlockedIncrement(&pStatus->nThreads_dec);
					Interlocked.Increment(ref pStatus.nThreads_dec);

				}
			}

			return 0;
		}

		public static int stopTransformThread_v(CCtxQmc pProcInfo, QY_TRANSFORM p,string hint)
        {
			string str;

			ref QMC_status pStatus = ref pProcInfo.status;// Ctx.get_qmc_status();

			if (hint == null) hint = "";

			if (null != p.video)
			{
				if (null != p.video.hThread)
				{
					str = string.Format("stopTrans_v {0}", hint);
					qyFuncs.showInfo_open(0, null, str);

					//
					p.video.bQuit = true;
					//
					mySetEvent(p.video.q2.hEvent0);
					//waitForThread(ref p.video.hThread, 20000);
					GuiShare.pf_waitForThread1(ref p.video.hThread, 20000);
					//
					//tmp_getHint_video(pTask, p, tBuf, mycountof(tBuf));
					//
					if (null != p.video.hThread)
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T("Failed"), tBuf);
					}
					else Interlocked.Decrement(ref pStatus.nThreads_dec);

				}
				if (null != p.video.hThread)
				{
					p.video.bQuit = true;
					//
					mySetEvent(p.video.q2.hEvent0);
					//waitForThread(ref p.video.hThread, 20000);
					GuiShare.pf_waitForThread1(ref p.video.hThread, 20000);
					//
					//tmp_getHint_video(pTask, p, tBuf, mycountof(tBuf));
					//
					if (null != p.video.hThread)
					{
						//qyExitProcess(tBuf);
					}
					else Interlocked.Decrement(ref pStatus.nThreads_dec);
				}
			}

			return 0;

		}
		/// </summary>
		/// <param name="pProcInfo"></param>
		/// <param name="pTask"></param>
		/// <param name="p"></param>
		/// <returns></returns>

		//
		public static int stopTransformThread(CCtxQmc pProcInfo, ref PROC_TASK_AV pTask, QY_TRANSFORM p)
		{
			//MC_VAR_isCli  *  pProcInfo	=	QY_GET_procInfo_isCli(  );
			int i;
			//TCHAR tBuf[512];
			//  char			buf[128];

			CCtxQmcTmpl pCtx = pProcInfo;
			if (null==pCtx) return -1;
			ref QMC_status pStatus = ref pProcInfo.status;// Ctx.get_qmc_status();
														  //if (!pStatus) return -1;
			if (p == null) return -1;


			//  2012/03/20
			//for  (  i  =  0;  i  <  pTask.usCntLimit_transforms;  i  ++  )  
			{
				//QY_TRANSFORM	*	p	=	&pTask.pTransforms[i];

				//  2011/12/09
				if (null != p.v_preTransThread)
				{
					if (null != p.v_preTransThread.hThread)
					{
						p.v_preTransThread.bQuit = true;
					}
				}
				if (null != p.audio)
				{
					if (null != p.audio.hThread)
					{
						p.audio.bQuit = true;
					}
				}
				if (null != p.video)
				{
					if (null != p.video.hThread)
					{
						p.video.bQuit = true;
					}
				}
				if (null != p.saveThread)
				{
					if (null != p.saveThread.hThread)
					{   //  2014/08/04
						p.saveThread.bQuit = true;
					}
				}
			}


			//
			//for  (  i  =  0;  i  <  pTask.usCntLimit_transforms;  i  ++  )  
			{
				//	 QY_TRANSFORM	*	p	=	&pTask.pTransforms[i];

				//  2011/12/09
				if (null != p.v_preTransThread)
				{
					if (null != p.v_preTransThread.hThread)
					{
						p.v_preTransThread.bQuit = true;
						//
						myEventSet(p.v_preTransThread.q2.hEvent0);
						//waitForThread(ref p.v_preTransThread.hThread, 20000);
						GuiShare.pf_waitForThread1(ref p.v_preTransThread.hThread, 20000);
						//			
						/*
						_sntprintf(tBuf, mycountof(tBuf), _T("stopTransformThreads: wait for v_preTrans thread %d, nStep %d, elapse %dms. "), p.v_preTransThread.dwThreadId, p.v_preTransThread.debugStep.nStep_debug, GetTickCount() - p.v_preTransThread.debugStep.dwTickCnt_step_debug);
						_sntprintf(tBuf, mycountof(tBuf), _T("%s  errs: notAccept %d, Input %d, output %d, index %d."), tBuf, p.v_preTransThread.debugStep.uiCnt_DMO_E_NOTACCEPTING, p.v_preTransThread.debugStep.nErrs_ProcessInput, p.v_preTransThread.debugStep.nErrs_ProcessOutput, p.v_preTransThread.debugStep.nErrs_pOutputBuffers_index);
						_sntprintf(tBuf, mycountof(tBuf), _T("%s %s"), tBuf, pTask.debugHint);
						tBuf[mycountof(tBuf) - 1] = 0;
						OutputDebugString(tBuf); OutputDebugString(_T("\n"));
						*/
						//
						if (null != p.v_preTransThread.hThread)
						{
							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T("Failed"), tBuf);
						}
						else Interlocked.Decrement(ref pStatus.nThreads_dec);
					}
					if (null != p.v_preTransThread.hThread)
					{
						p.v_preTransThread.bQuit = true;
						//
						myEventSet(p.v_preTransThread.q2.hEvent0);
						//waitForThread(ref p.v_preTransThread.hThread, 20000);
						GuiShare.pf_waitForThread1(ref p.v_preTransThread.hThread, 20000);
						//
						/*
						_sntprintf(tBuf, mycountof(tBuf), _T("stopTransformThreads: wait for v_preTrans thread %d, nStep %d, elapse %dms. "), p.v_preTransThread.dwThreadId, p.v_preTransThread.debugStep.nStep_debug, GetTickCount() - p.v_preTransThread.debugStep.dwTickCnt_step_debug);
						_sntprintf(tBuf, mycountof(tBuf), _T("%s  errs: notAccept %d, Input %d, output %d, index %d."), tBuf, p.v_preTransThread.debugStep.uiCnt_DMO_E_NOTACCEPTING, p.v_preTransThread.debugStep.nErrs_ProcessInput, p.v_preTransThread.debugStep.nErrs_ProcessOutput, p.v_preTransThread.debugStep.nErrs_pOutputBuffers_index);
						_sntprintf(tBuf, mycountof(tBuf), _T("%s %s"), tBuf, pTask.debugHint);
						tBuf[mycountof(tBuf) - 1] = 0;
						OutputDebugString(tBuf); OutputDebugString(_T("\n"));
						*/
						//
						if (null != p.v_preTransThread.hThread)
						{
							//qyExitProcess(tBuf);
						}
						else Interlocked.Decrement(ref pStatus.nThreads_dec);

					}
				}


				//
				if (null != p.audio)
				{
					if (null != p.audio.hThread)
					{
						p.audio.bQuit = true;
						//
						if (pProcInfo.cfg.b_useAudioQ2_toDecA)
						{
							myEventSet(p.audio.q2.hEvent0);
						}
						else
						{
							myEventSet(p.audio.qBuf2.hEvent0);

						}
						//waitForThread(ref p.audio.hThread, 20000);
						GuiShare.pf_waitForThread1(ref p.audio.hThread, 20000);
						//
						/*
						_sntprintf(tBuf, mycountof(tBuf), _T("stopTransformThreads: wait for audio thread %d, nStep %d, elapse %dms."), p.audio.dwThreadId, p.audio.debugStep.nStep_debug, GetTickCount() - p.audio.debugStep.dwTickCnt_step_debug);
						tBuf[mycountof(tBuf) - 1] = 0;
						OutputDebugString(tBuf); OutputDebugString(_T("\n"));
						*/
						//
						if (null != p.audio.hThread)
						{
							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T("Failed"), tBuf);
						}
						else Interlocked.Decrement(ref pStatus.nThreads_dec);

					}
					if (null != p.audio.hThread)
					{       //  2009/07/09. retry to wait for audio thread.
						p.audio.bQuit = true;
						//
						if (pProcInfo.cfg.b_useAudioQ2_toDecA)
						{
							mySetEvent(p.audio.q2.hEvent0);
						}
						else
						{
							mySetEvent(p.audio.qBuf2.hEvent0);
						}
						//waitForThread(ref p.audio.hThread, 20000);
						GuiShare.pf_waitForThread1(ref p.audio.hThread, 20000);
						//
						/*
						_sntprintf(tBuf, mycountof(tBuf), _T("stopTransformThreads: wait for audio thread %d, nStep %d, elapse %dms."), p.audio.dwThreadId, p.audio.debugStep.nStep_debug, GetTickCount() - p.audio.debugStep.dwTickCnt_step_debug);
						tBuf[mycountof(tBuf) - 1] = 0;
						OutputDebugString(tBuf); OutputDebugString(_T("\n"));
						*/
						//
						if (null != p.audio.hThread)
						{
							//qyExitProcess(tBuf);
						}
						else Interlocked.Decrement(ref pStatus.nThreads_dec);
					}
				}

#if false
				if (null!=p.video.hThread)
				{
					p.video.bQuit = true;
					//
					mySetEvent(p.video.q2.hEvent0);
					waitForThread(ref p.video.hThread, 20000);
					//
					//tmp_getHint_video(pTask, p, tBuf, mycountof(tBuf));
					//
					if (null!=p.video.hThread)
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T("Failed"), tBuf);
					}
					else Interlocked.Decrement(ref pStatus.nThreads_dec);

				}
				if (null!=p.video.hThread)
				{
					p.video.bQuit = true;
					//
					mySetEvent(p.video.q2.hEvent0);
					waitForThread(ref p.video.hThread, 20000);
					//
					//tmp_getHint_video(pTask, p, tBuf, mycountof(tBuf));
					//
					if (null!=p.video.hThread)
					{
						//qyExitProcess(tBuf);
					}
					else Interlocked.Decrement(ref pStatus.nThreads_dec);
				}
#endif
				//
				stopTransformThread_v(pProcInfo, p,"called in stopTrans");

				//2014/08/04
				if (null != p.saveThread)
				{
					if (null != p.saveThread.hThread)
					{
						p.saveThread.bQuit = true;
						//
						mySetEvent(p.saveThread.q2.hEvent0);
						//waitForThread(ref p.saveThread.hThread, 20000);
						GuiShare.pf_waitForThread1(ref p.saveThread.hThread, 20000);
						//
						//tmp_getHint_video(pTask, p, tBuf, mycountof(tBuf));
						//
						if (null != p.saveThread.hThread)
						{
							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T("Failed"), tBuf);
						}
						else Interlocked.Decrement(ref pStatus.nThreads_dec);

					}
					if (null != p.saveThread.hThread)
					{
						p.saveThread.bQuit = true;
						//
						mySetEvent(p.saveThread.q2.hEvent0);
						//waitForThread(ref p.saveThread.hThread, 20000);
						GuiShare.pf_waitForThread1(ref p.saveThread.hThread, 20000);
						//
						//tmp_getHint_video(pTask, p, tBuf, mycountof(tBuf));
						//
						if (null != p.saveThread.hThread)
						{
							//qyExitProcess(tBuf);
						}
						else Interlocked.Decrement(ref pStatus.nThreads_dec);
					}
				}


				//////////////


			}

			//OutputDebugString(_T("stopTransformThreads\n"));

			return 0;
		}



		//
		public static int stopTransformThreads(CCtxQmc pProcInfo, ref PROC_TASK_AV pTask,string hint)
		{
			//  MC_VAR_isCli  *  pProcInfo	=	QY_GET_procInfo_isCli(  );
			int i;
			//TCHAR tBuf[512];
			//  char			buf[128];

			CCtxQmcTmpl pCtx = pProcInfo;
			if (null==pCtx) return -1;

			//
			string str;

			//
			if (hint == null) hint = "";


			str = string.Format("stopTransformThreads enters, {0}",hint);
			showInfo_open(0, null, str);

            //
            for (i = 0; i < pTask.trans.usCntLimit_transforms; i++)
            {
				QY_TRANSFORM p = pTask.trans.pTransforms[i];
				if (p == null) continue;

				//
				if (null != p.v_preTransThread)
				{
					if (null != p.v_preTransThread.hThread)
					{
						p.v_preTransThread.bQuit = true;
						mySetEvent(p.v_preTransThread.q2.hEvent0);
					}
				}
				if (null != p.audio)
				{
					if (null != p.audio.hThread)
					{
						p.audio.bQuit = true;
						mySetEvent(p.audio.q2.hEvent0);
					}
				}
				if (null != p.video)
				{
					if (null != p.video.hThread)
					{
						p.video.bQuit = true;
						mySetEvent(p.video.q2.hEvent0);
					}
				}
				if (p.saveThread != null)
				{
					if (null != p.saveThread.hThread)
					{   //  2014/08/04
						p.saveThread.bQuit = true;
						mySetEvent(p.saveThread.q2.hEvent0);
					}
				}

				//
				continue;
			}


			//  2014/11/11
			for (i = 0; i < pTask.trans.usCntLimit_transforms; i++)
			{
				QY_TRANSFORM p = pTask.trans.pTransforms[i];

				stopTransformThread(pProcInfo, ref pTask, p);
			}

			//  2014/11/11
#if false
			stopTransformThread(pProcInfo, ref pTask, pTask.confMosaicTrans.confMosaic_video1.transform);
			stopTransformThread(pProcInfo, ref pTask, pTask.confMosaicTrans.confMosaic_resource.transform);
#endif


			//
			str = string.Format("stopTransformThreads leaves");
			showInfo_open(0, null, str);


			return 0;
		}

		//
		public static unsafe bool isPkt_sharedTex(QY_TRANSFORM pTransform)
		{
			bool bRet = false;

			CCtxQyMc pQyMc= g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

#if false
			if (pTransform.video.tv.compressor.ucCompressors != Consts.CONST_videoCompressors_vcm
			   && pTransform.video.tv.compressor.ucCompressors != Consts.CONST_videoCompressors_dmo
			   && pTransform.video.u_qoi.myQoi.common.var.usPktResType_dec == Consts.CONST_pktResType_sharedTex)
			{
				bRet = true;
			}
#endif
			//
			if ( !pProcInfo.cfg.ucb_USE_Fragment_dlgTalk_av  )
            {
				bRet = true;
            }


			return bRet;
		}


	}
}
