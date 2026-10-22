using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
    public const int DEFAULT_usTimeoutInMs_player_a = 5000; //  2017/07/03	//  10000					//  2010/06/25
    public const int DEFAULT_usTimeoutInMs_player_v = 10000;

    public const int DEFAULT_loopCtrl_timer_dlgTalk_chkRecvdTasks = 7;  //  30				//  2010/06/26
    public const int DEFAULT_loopCtrl_chkPlayers = 5;	//10						//  2010/06/26

}



namespace qy
{

    public struct QMC_cfg_envVar
    {
        public ushort usMaxQNodes_mediaQ;
        public ushort usQNodes_warningInterval_mediaQ;
        public ushort usMaxQNodes_rtMediaQ;
        public ushort usQNodes_warningInterval_rtMediaQ;
        public ushort usMaxQNodes_rtOpQ;                                           //  2016/12/26
        public ushort usQNodes_warningInterval_rtOpQ;

        //
        public ushort usMaxQNodes_playSoundQ;
        public ushort usMaxQNodes_playVideoQ;                                      //  2012/11/09
        public ushort usMaxQNodes_displayVideoQ;                                   //  2012/11/09

        //
        public ushort usMaxQNodes_dlgTalk_processQ;                                //  2010/07/22
        public ushort usMaxnItems_dlgTalk_list;                                    //  2010/07/22
                                                                                   //
        public ushort usMaxQNodes_imMsgLogQ;

        //  2017/08/15
        public bool default_ucbLetConfMgrSetMicOn;




    };

    public struct Policy_notification
    {
        public bool ucbPermitEveryOneNotify;                     //  允许任何人呼叫
        public bool ucbNotNotifyDuringConference;                 //  禁止会议中有通知消息
        public bool ucbNotNotify;                                 //  禁止任何通知消息
                                                                  //
        public bool ucbBeep;
        //
        public ushort usTimeInMsToPop;                             //  pop窗口弹出要花多少时间
        public int iW_pic;
        public int iH_pic;
        public int iH_static;
    };

    public struct POLICY_isClient_video
    {
        public uint uiCamCapType;                                     //  2010/07/26
                                                                      //
                                                                      //  unsigned  char								ucSeconds_perFrame_videoConference;				//  ºÜÂýÊ±£¬ÓÃ¼¸ÃëÒ»Ö¡. Õâ¸ö²ÎÊýÖ»ÓÃÔÚ·¢ËÍ¶Ë£¬½ÓÊÕ¶Ë²»ÐèÒªÓÃµÄ¡£
                                                                      //  unsigned  short							usFrames_perSecond_videoConference;				//  Ò»Ãë¼¸èå


    };

    public struct POLICY_isClient_audio
    {
        public uint uiRecordType;                                     //  2010/08/02
                                                                      //

        //  2014/07/31
        public bool ucbNot_selectDefaultRecorder;

    };

    public unsafe struct POLICY_isClient
    {
        public bool ucbRunAsAdvertisingPlayer;                            //  2013/11/14. 是否做为广告机运行

        //  
        public bool ucbDlgShareDynBmps_autopopupandhideOnStartup;

        //
        //
        public bool ucbUseGlobalVWall;                                    //  2012/04/02. 是否使用GlobalVWall
                                                                          //
        public byte ucRuleIfMsgArrive;
        public fixed char dirToSaveFile[Consts.MAX_PATH + 1];

        //
        public bool ucbSaveMsg;
        public fixed char dirToSaveMsg[Consts.MAX_PATH + 1];                       //  2014/04/26

        //
        public uint uiTimeOfVideoFileInS;
        public fixed char dirToSaveVideo[Consts.MAX_PATH + 1];                     //  2014/08/03				

        //  2014/03/29
        public byte ucHardwareAccl;
        //

        //  2011/03/28
        //  unsigned  char									snmpbShowMsgBox;
        public bool ucbShowNotification;
        public bool ucbScrollForEnlagedImg;                               //  2012/04/07. 对双击放大图像是否不滚动

        //
        //
        public POLICY_isClient_video video;
        public POLICY_isClient_audio audio;



        //  2014/07/31
        public bool ucbNot_selectDefaultRecorder;


        //
        public ushort usTimeoutInMsToSwitch;                           //  2008/10/28, media realTimeMedia daemon 
        public ushort usTimeoutInMsToSwitch_rt;                        //  rt

        //  2012/03/31
        public ushort usMax_nPlayers;                                  //  2012/03/30
        public ushort usMax_nPlayers_gps;                              //  2012/05/07
        public ushort usMax_nThreads_dec;
        public uint uiMaxQNodes_vwRuleQ;

        //  2009/02/25
        public bool ucbManullyPermitToSpeakOnVideoConference;     //  2009/02//24

        //  2016/02/15
        public struct PolicyIsClient_avRules
        {
            public uint uiCmd_autoAnswer_av;
            //  2017/07/07
            public bool ucbLetConfMgrSetMicOn;
        };
        public PolicyIsClient_avRules avRules;
        //

        //  2010/08/24
        public Policy_notification notification;

        //  2010/09/03
        public bool dlgTalk_ucbShowImGrpMems;
        //  
        public bool dlgTalk_snmpbShowMsgBox;

        //  2012/08/01
        //  COM_PORT_cfg									comPort_ptz;

        //  2012/06/25
        public bool ucbPreferCuda;                                //  进行视频压缩硬加速时优先采用cuda
                                                                  //  2012/11/06
        public bool ucbLooping_mediaFile;                       //  循环播放媒体文件

    };


    public unsafe struct QMC_cfg
    {

        public uint uiMaxSendMsgs;                                             //  
        public ushort usMaxMsgrRecentFriends;                                      //  2007/06/04

        //
        public uint uiSizePerSnd_file_noAv;                                        //  2014/07/13
        public uint uiSizePerSnd_file_avExists;                                    //  2014/07/13

        //
        public uint uiSizePerSnd_media;                                            //  2008/04/01
        public ushort usNeedShowFrameInfoIntervalInS;                              //  

        public uint uiBufSize_myDrawVideoData;                                 //  2010/04/24
        public uint uiBufSize_myPlayAudioData;                                 //  2010/04/24

        //
        public QMC_cfg_envVar envVar;


        //		
        public GENERIC_Q_cfgEx mgrQEx;
        public GENERIC_Q_CFG displayQ, workQ, schedulerQ, robotQ, mediaQ, realTimeMediaQ, rtOpQ;       //  svrThreadÍ¨³£ÓÃÀ´ÏòÆäËûµÄcontactÌá¹©ÐÅÏ¢£¬±ÈÈç´«ÎÄ¼þµÈ
                                                                                                       //  
        public GENERIC_Q_CFG processQ_robot;
        //
        public GENERIC_Q_CFG processQ_media;                                               //  

        public GENERIC_Q_CFG outputQ_toMix_audio;                                      //  ÒôÆµµÄÊä³ö¶ÓÁÐ¡£
        public GENERIC_Q_CFG outputQ_toMosaic_video;                                      //  2009/07/26

        //
        public GENERIC_Q_CFG playSoundQ;
        public GENERIC_Q_CFG playVideoQ;                                                   //  2009/05/02
        public GENERIC_Q_CFG displayVideoQ;                                                //  2009/07/25

        //
        //GENERIC_Q_CFG								preTransformQ;												//  2011/12/09
        public GENERIC_Q_CFG transformQ;                                                   //  2009/05/12

        //
        //GENERIC_Q_CFG								vCamQ;														//  2010/06/18

        //  2017/01/29
        public GENERIC_Q_CFG appQ;                                                     //  2017/01/29
                                                                                       //
        public GENERIC_Q_CFG vppQ;                                                     //  2011/12/04

        //
        public GENERIC_Q_CFG toShareAudioQ;                                                //  2011/01/21
        public GENERIC_Q_CFG toShareVideoQ;                                                //  2011/01/21

        //
        public GENERIC_Q_CFG toShareVideo_postQ;                                           //  2011/12/06

        //
        public GENERIC_Q_CFG dispatchQ;                                                    //  2009/06/17

        //  2016/03/15
        public GENERIC_Q_CFG talkerThreadQ;                                                //  2016/03/15

        //  2016/06/15
        public GENERIC_Q_CFG shareDynBmpsQ;                                                //  2016/06/15

        //
        public GENERIC_Q_CFG guestQ;                                                       //  2008/01/18, 
                                                                                           //GENERIC_Q_CFG								phoneMsgrQ;													//  2008/02/10, phoneMessengerQ
        public GENERIC_Q_CFG dynBmpQ;                                                  //  2009/09/05



        //
        public POLICY_isClient policy;

        //
        public fixed char mutexName_syncSendAv[Consts.cntof_mutexName];
        public fixed char mutexName_syncActiveMem_prefix1[Consts.cntof_mutexName_prefix];                  //  2009/05/31
        public fixed char mutexName_syncStartAv[Consts.cntof_mutexName];

        //  2014/09/22
        public bool ucb__SUPPORT_shareDynBmps__;

        //  2008/05/17
        //unsigned  char								ucbNeedShowFrameInfo_forVideoConference;
        //
        public bool ucb__SUPPORT_dmoToDec__;                                  //  2010/08/05
                                                                              //  2014/04/03
        public bool ucb__USE_hdEnc_conf__;

        //
        public bool ucb__USE_decObj_a_mediaCodec_async__;

        //  2012/05/15
        public bool ucbTestGps;
        //  2015/05/18
        public bool ucbPlayLocalAudio_captured;                                   //  2015/05/18
        public bool ucbPlayRemoteAudio_viewDynBmp;                                //  2015/05/19		 

        //
        public bool ucb__USE_d2dToRenderTxt__;                                    //  2016/05/18

        //
        public bool ucb_USE_Fragment_dlgTalk_av;

        public struct Special_dbg
        {
            public bool bNo_videoCap;
            public bool bNo_audioRecord;
            public bool bNo_audioPlayer;
            //
            public bool bNo_encV;
            //
            public bool bDoGC_mainWnd_OnTimer;
            //
            public bool bTrace_tmpHandler_findOrgMsgAndProc_is;
        };
        //
        public Special_dbg mSpecialDbg;
        public bool ucb_USE_specialDbg;


        //
        public ushort usMaxCnt_capProcInfoBmps;
        public ushort usMaxCnt_capProcInfoAudios;

        public ushort usMaxCnt_pSharedObjs;                                        //  2008/10/04


        //  2015/11/10
        public ushort usMaxCnt_qmcObjVarInfos;                                     //  2015/11/10
                                                                                   //  2014/09/23
        public ushort usMaxCnt_taskInfos;

        //
        public ushort usTimeoutInMs_player_a;                                      //  2010/06/25
        public ushort usTimeoutInMs_player_v;

        public ushort loopCtrl_chkPlayers;                                     //  default 10. 2010/06/26
        public ushort loopCtrl_timer_dlgTalk_chkRecvdTasks;                        //  default 30. 2010/06/26. 

        //
        public bool notify_avCall_bNoSound;
        public uint uiTimeoutInMs_waitForProcessingOf_avTask;
        public uint uiTimeoutInMs_waitForProcessingOf_otherTask;
        public uint uiTimeoutInMs_waitIfNoData;

        //
        public ushort usMaxN_lostVideo;

        //
        public bool b_useAudioQ2_toDecA;
        public bool b_useQ2_toPlayA;

        //
        public bool b_useVideoQ2_toDecV;

        //
        public bool b_no_KeyLowLatency_a;

        //
        public bool b_no_4k;

        //

        //
        public QMC_cfg_debugStatusInfo debugStatusInfo;    //  2015/01/23

        /*
	//  2016/08/10
	TCHAR clientWelcomeMsg[128];
		*/
    }
    ;


    public struct QMC_av_channel_status
    {
        public bool bChannelReady;
        public uint dwLastModifiedTickCnt;
        //
        public int iThreadsStatus;
    };


    public class TMP_tex_info
    {
        //
        public QY_MESSENGER_ID idInfo;
        public uint uiObjType;

        //
        public int texId;
        public int iW;
        public int iH;

    };

    //
    public class TMP_task_texes
    {
        public int iTaskId;
        //
        public TMP_tex_info[] tti_peers;
        //
        public ushort usCnt_activeMems;
    };


    //
    public class TMP_texes
    {
        //public TMP_tex_info tti_me;

        //
        public TMP_task_texes taskAvTexes;
#if DEBUG
        //public TMP_tex_info tti_peer;
#endif
        //
        public TMP_texes()
        {
            taskAvTexes = new TMP_task_texes();
            taskAvTexes.tti_peers = new TMP_tex_info[Consts.MAX_conf_activeMems];
            for (int i = 0; i < taskAvTexes.tti_peers.Length; i++)
            {
                taskAvTexes.tti_peers[i] = new TMP_tex_info();
            }
            //
            //tti_peer = new TMP_tex_info();

        }
    };



    public unsafe struct QMC_av
    {


        //	'
        public QMC_av_channel_status talk, audio, video, robot, rtOp;

        //
        public bool m_bInSleepMode;


        //
        public uint sdwTickCnt_applyForChkChannels;

        //
        public struct QMC_localAv
        {

            public CAP_procInfo_video videoCaptureProcInfo;                                       //  200903/18

            //
            public QY_MESSENGER_ID idInfo_curConference;                                       //  (idInfo)
            public uint uiTaskType_curConference;                                  //  
                                                                                   //

            public CAP_procInfo_recordSound recordSoundProcInfo;                                       //  2009/02/11

            //  
            public QY_PLAYER player;

            //  2014/04/07
            //HWND hWnd_selectAvCompressor;

        };
        public QMC_localAv localAv;

        //
        public TMP_texes texes;

        //  
        public ushort usCnt_players;
        public QY_PLAYER[] pPlayers;
        //
        public bool bNeedChkPlayers;                   //  2018/06/16
    };

    //


    public abstract class CCtxQmc : CCtxQmcTmpl
    {
        public QMC_cfg cfg;
        //
        public uint uiDevType;                                                 //  2007/08/03

        //
        public QY_COMM_AUTHINFO_MIS authInfo;


        //
        uint uiLastTranNo;

        //
        public bool bQuit_threadProc_isCli;
        public bool bEnd_threadProc_isCli;
        //
        public Thread hThread_mgr;
        public uint dwThreadId_mgr;

        //
        public Thread hThread_display;                                         //  2007/06/09		
                                                                               //DWORD dwThreadId_display;
                                                                               //
        public Thread hThread_work;
        public uint dwThreadId_work;
        //
        public Thread hThread_scheduler;
        //DWORD dwThreadId_scheduler;
        //
        public Thread hThread_robot;                                               //  2007/07/04
        public uint dwThreadId_robot;
        //
        public Thread hThread_media;
        public uint dwThreadId_media;
        //
        public Thread hThread_realTimeMedia;
        public uint dwThreadId_realTimeMedia;
        //  2016/12/26
        public Thread hThread_rtOp;
        //DWORD dwThreadId_rtOp;

        //
        public uint dwThreadId_gui;

        //
        public GENERIC_Q mgrQ;                                                     //  
                                                                                   //
        public GENERIC_Q displayQ;                                                 //  
        public GENERIC_Q workQ;                                                        //  
        public GENERIC_Q schedulerQ;                                                   //  
        public GENERIC_Q robotQ;
        public QY_Q2 mediaQ2;
        public QY_Q2 realTimeMediaQ2;
        public QY_Q2 rtOpQ2;                   //  2016/12/26

        //  2015/12/04
        //ZONE_objs* pGZone_activeTalkers;

        //  2016/06/03
        //ZONE_objs* pGZone_d3dWalls;                                         //  2016/06/03

        //
        public CMcSharedQ processQ_media;                                              //  



        //
        public GENERIC_Q imMsgLogQ;                                                    //  2010/07/17

        //
        public PolicyAvParams p_gAvParams;

        //
        public object pFuncs_for_isCliHelp;                                     //  2013/06/08

        //  2016/09/05
        //void									*		p_g_dbFuncs;

        //
        /*
		unsigned char ucb_doInitRecordSound;                                        //  2010/08/03

		
		*/
        public IpcProc m_ipcProc;

        //  av, 2008/03/18
        public QMC_av av;

        //
        public Mutex hMutex_syncQdcUnit;
        //
        public EventWaitHandle hEvt_notify_thread_isCli;
        //
        //public Mutex hMutex_sync_mTableItems_imObj;


        /*
	//  gps  2012/05/07
	struct                                         {

						 //  2012/05/07
	unsigned short usCnt_players;
	QY_PLAYER_gps* pPlayers;

					 }												gps;

		*/
        //	
        public CAP_procInfo_bmpCommon[] pCapProcInfoBmps;
        public CAP_procInfo_audioCommon[] pCapProcInfoAudios;


        //  
        public QY_sharedObj_sync[] pSharedObjSyncs;                                          //  2014/09/30

        /*
	//  2015/11/10
	void* pQmcObjVarInfos;                                          //  2015/11/10
																	//  2014/09/24
		*/
        public QMC_TASK_INFO[] pQmcTaskInfos;



        //
        //  2015/07/01
        //unsigned char ucCnt_misCnts;
        public MIS_CNT pMisCnt;

        /*
//  2011/04/05
struct                                         {
					 CUSTOMER_service_obj objs[1];
//
struct                                     {
						 QY_MESSENGER_ID idInfo;
TCHAR talkerDesc[32];
					 }											called;
				 }												customerService;

//  2009/12/21
struct                                         {
					 unsigned short usMaxCnt_qmCmds;
unsigned int uiBufSize_qmShm;
//
HANDLE hMap;
char* pBuf;
				 }												cmdsShm;



//
QIS_ptz_wnd ptzWnd;

//  2013/09/26
struct                                         {
					 HWND hWnd_dlgImg;
				 }												chromaKey;

//  2013/03/09
struct                                         {
					 HWND hWnd_videoTagCfg;
//
SUBTITLES_cfg webcam;
SUBTITLES_cfg d3dWall;													//  d3dWall需要设置字体。先这么使用 . 2013/12/10
				 }												subtitles;

//
OfflineRes_var offlineRes;


//  2017/07/18
struct                                         {
					 TCHAR unique_wndTitle_parent[64];          //  必须是独一无二的窗口题头。
																//
BOOL bGotMsg_avConsole;
				 }												avConsole;

//  2017/07/26
struct                                         {
					 HWND hWnd_cfgGrp;
				 }												cfgGrp;

//
HWND hWnd_shareDynBmps;                                         //  2009/08/13								
																//
unsigned char ucbAutoHideOnStartup;
unsigned char ucbAutoOpenChked;                                         //  2017/06/10


//
//  HWND											hFrame_sharedDynBmpList;									//  2009/09/03
DWORD dwTickCnt_toRetrieveSharedDynBmps;
//BOOL											bInited_mcProcResp_retrieveDynBmps;							//  2011/10/31
time_t tLastRefreshedTime_misServ_sharedDynBmps;
//
HWND hWnd_notification;                                         //  2010/08/24

//
HWND hWnd_imObjView;                                                //  2017/01/04

//
HWND g_hVWall;                                                  //  2012/04/02
int iWndContentType_g_hVWall;                                   //  2013/07/11

//  2018/10/28
HWND hWall_hzj;                                                 //  2018/10/28
struct                                         {
					 QY_MESSENGER_ID idInfo_tmpGrp;
BOOL bConfStarted;
				 }												hzj;

		*/

        //
        public struct QMC_waitToQuit
        {
            public uint dwTickCnt_start;
            public uint dwTimeInMs;
            //
            public string hint;
        };
        public QMC_waitToQuit waitToQuit;                                                   //  2011/02/02


        //
        public QMC_status status;


        //
        public uint sdwTickCnt_applyForChkChannels = 0;

        //
        public bool m_bNeedToRemoveTaskAtOnce = false;


        public CCtxQmc()
        {
            ;
            m_iCtxType = Consts.CONST_ctxType_qmc;
        }

        ~CCtxQmc()
        {

        }

        public override unsafe int setQmDbFuncs(int iDbType, QM_dbFuncs pDbFuncs)
        {
            return -1;
        }

        abstract public unsafe int postMsg2Mgr_mc(CTX_caller ctxCaller, MIS_CNT pMIS_CNT,ref  MSG_ROUTE pRoute, uint uiMisMsgType, byte ucFlg, ushort usCode, Int64 tStartTime, uint uiTranNo, uint uiSeqNo, byte* data, uint dataLen, ulong pIdInfo_logicalPeer, ulong pIdInfo_dst, uint uiChannelType, CMIS_MSGU pMsgBuf, bool bLog);

        //
        abstract public unsafe int postImMsg2Log_isClient(ref MIS_MSGU pMsg, int lenInBytes_msg);


        abstract public unsafe int recoverMessenger(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QY_MESSENGER_INFO* pObj, QY_MESSENGER_REGINFO* pRegInfo, Int64 tLastModifiedTime, bool bLog, GENERIC_Q pLogQ);
        //abstract public unsafe int recoverImObjRules(QM_dbFuncs pDbFuncs, object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRules_req* pReq, time_t tLastModifiedTime)  =  NULL;
        abstract public unsafe int recoverImGrp(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, uint uiObjType, IM_GRP_INFO* pGrpInfo, Int64 tLastModifiedTime, bool bNoGrpName);
        abstract public unsafe int recoverImGrpMem(QM_dbFuncs pDbFuncs, object pDb, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime);

        virtual public int tryToTalkToMessenger_any(UInt64 ui64Id, bool bNeedNotShowWnd, bool bActivateWnd, ref object phWnd) { return -1; }

        public QMC_TASK_INFO getQmcTaskInfoByIndex(CTX_caller ctxCaller, int index, string hint)
        {
            CCtxQmc pProcInfo = this;

            return qyFuncs.getQmcTaskInfoByIndex(ctxCaller, pProcInfo, index, hint);
        }

        public QY_sharedObj_sync getSharedObjSyncByIndex(int index)
        {
            CCtxQmc pProcInfo = this;

            if (null == pProcInfo) return null;
            if (null == pProcInfo.pSharedObjSyncs) return null;
            if (index < 0 || index >= pProcInfo.cfg.usMaxCnt_pSharedObjs) return null;

            QY_sharedObj_sync[] pSharedObjInfos = (QY_sharedObj_sync[])pProcInfo.pSharedObjSyncs;

            return pSharedObjInfos[index];

        }

        public CAP_procInfo_bmpCommon getCapBmpBySth(int index, uint uiCapType)
        {
            CCtxQmc pProcInfo = this;

            if (index < 0 || index >= pProcInfo.cfg.usMaxCnt_capProcInfoBmps) return null;
            //  if  (  !index  )  return  null;
            if (0 == index) return (CAP_procInfo_bmpCommon)pProcInfo.av.localAv.videoCaptureProcInfo; //  2009/06/02
            if (0 != uiCapType)
            {
                if (pProcInfo.pCapProcInfoBmps[index].uiType != uiCapType) return null;
            }
            return pProcInfo.pCapProcInfoBmps[index];
        }



        public CAP_procInfo_audioCommon getCapAudioBySth(int index, uint uiCapType)
        {
            CCtxQmc pProcInfo = this;

            if (index < 0 || index >= pProcInfo.cfg.usMaxCnt_capProcInfoAudios) return null;
            //  if  (  !index  )  return  null;
            if (0 == index) return (CAP_procInfo_audioCommon)pProcInfo.av.localAv.recordSoundProcInfo;
            if (0 != uiCapType)
            {
                if (pProcInfo.pCapProcInfoAudios[index].uiType != uiCapType) return null;
            }
            return pProcInfo.pCapProcInfoAudios[index];


        }

        //
        public unsafe int resizeRgb24(byte* src, int srcWidth, int srcHeight, byte* dst, int dstWidth, int dstHeight)
        {
            return -1;
        }

        public unsafe int initCompressVideo(BITMAPINFOHEADER* pBmpInfo_input, uint uiCapType, ref VIDEO_COMPRESSOR_CFG pCompressor, bool bCapDev, uint uiBufSize_pData, ref COMPRESS_VIDEO pCompressVideo)
        {
            return qyFuncs.initCompressVideo(this, pBmpInfo_input, uiCapType, ref pCompressor, bCapDev, uiBufSize_pData, ref pCompressVideo);
        }



        //  2016/04/26
        public unsafe int exitCompressVideo(bool bCapDev, ref COMPRESS_VIDEO pCompressVideo)
        {
            return qyFuncs.exitCompressVideo(this, bCapDev, ref pCompressVideo);
        }

        public unsafe int stopLocalAudioRecorder(int index_sharedObj, int nTries)
        {
            return GuiShare.pf_stopLocalAudioRecorder(this, index_sharedObj, nTries);
        }

        public CUS_MODULES getCusModules()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (null == pQyMc) return null;

            return pQyMc.cusModules;
        }

        public unsafe int toShareVideo_post(ref CTX_caller ctxCaller, uint uiCapType, int iIndex_capProcInfo, myDRAW_VIDEO_DATA* pkts, uint nPkts, ref COMPRESS_VIDEO pCompressVideo, QY_SHARED_OBJ pSharedObj, MIS_MSGU* pMsgBuf)
        {
            return qyFuncs.toShareVideo_post_qmc(ref ctxCaller, this, uiCapType, iIndex_capProcInfo, pkts, nPkts, ref pCompressVideo, pSharedObj, pMsgBuf);
        }

        public unsafe int drawLocalVideoData(uint uiTranNo, myDRAW_VIDEO_DATA* pkts, int pktsLen, bool* pbPktsRedirected, uint uiCapType, int iIndex_capBmp, int iTaskId, MIS_CNT pMIS_CNT, QY_MESSENGER_ID* pIdInfo_peer)
        {
            return -1;
            //return  qyFuncs.drawLocalVideoData(this, uiTranNo, pkts, pktsLen, pbPktsRedirected, uiCapType, iIndex_capBmp, iTaskId, (MIS_CNT)pMIS_CNT, pIdInfo_peer);
        }

        public unsafe int drawVideoData(myDRAW_VIDEO_DATA* pkts, int pktsLen, bool* pbPktsRedirected, QY_TRANSFORM pQY_TRANSFORM)
        {
            CCtxQmc pProcInfo = this;
            return qyFuncs.drawVideoData(pProcInfo, pkts, pktsLen, pbPktsRedirected, pQY_TRANSFORM);
        }

        public unsafe int playLocalAudioData(int iTaskId, uint uiTranNo, ref WAVEFORMATEX pWf, myPLAY_AUDIO_DATA* pPkt, int pktLen, bool* pbPktRedirected, uint uiCapType, int iIndex_capAudio)
        {
            //return  ::playLocalAudioData(  this,  iTaskId,  uiTranNo,  pWf,  pPkt,  pktLen,  pbPktRedirected,  uiCapType,  iIndex_capAudio  );
            return qyFuncs.playLocalAudioData(this, iTaskId, uiTranNo, ref pWf, (int)pPkt->head.uiSampleTimeInMs, (byte*)pPkt->memory.m_pBuf, pPkt->uiSize_dec, uiCapType, iIndex_capAudio);

        }

        //
        public unsafe int playAudioData(int iSampleTimeInMs, uint uiPts, byte* pInput, uint inputLen, int iIndex_player)
        {
            return qyFuncs.playAudioData(this, iSampleTimeInMs, uiPts, (IntPtr)pInput, inputLen, iIndex_player);
        }



        public  int get_deced_pktResType(uint uiModuleType, int iFourcc, ref ushort pusPktResType_o)                //  2015/10/04
        {
            int iErr = -1;
            ushort usPktResType = 0;

            //
            switch (uiModuleType)
            {
                case Consts.CONST_moduleType_mediaCodec:
                    usPktResType = Consts.CONST_pktResType_sharedTex;
                    break;
                default:
                    break;
            }

            //
            iErr = 0;
        errLabel:

            //
            if (0 == iErr)
            {
                if (null != pusPktResType_o)
                {
                    pusPktResType_o = usPktResType;
                }
            }

            return iErr;
        }


        public unsafe int doApplyForPlayer(object hMainWnd, ref MIS_MSGU  pMsgU)
        {
            fixed (MIS_MSGU* pMsg = &pMsgU)
            {
                return qyFuncs.doApplyForPlayer(hMainWnd, pMsg);
            }
        }

        public QY_Q2 get_realTimeMediaQ2()
        {
            return this.realTimeMediaQ2;
        }


        public QY_Q2 get_mediaQ2()
        {
            return this.mediaQ2;
        }

        //
        public bool bRealtimeMediaChannelReady()
        {
            return this.av.audio.bChannelReady;
        }


        //  2015/10/14
        public bool bMediaChannelReady()
        {
            return this.av.video.bChannelReady;
        }

    }



    partial class qyFuncs
    {
        public static bool bQThreadExists(ref PLAY_AUDIO_procInfo p)
        {
            if (p.uiPlayType != 0)
            {
                Console.WriteLine("not finished a.bQThreadExists");
                return true;
            }
            //
            return false;
        }

        public static bool bQThreadExists(ref PLAY_VIDEO_procInfo p)
        {
            if (p.uiPlayType != 0)
            {
                Console.WriteLine("not finished v.bQThreadExists");
                return true;
            }
            //
            return false;
        }


        public unsafe static int initVar_isGui(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2)
        {
            int iErr = -1;
            int i;
            string str;


            if (pQyMc.iServiceId != Consts.CONST_qyServiceId_is) return 0;

            //  2010/10/01

            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            if (null == pDbFuncs) goto errLabel;
            QM_dbFuncs g_dbFuncs = pDbFuncs;

            //  2009/12/21
            //
            CCtxQmc pProcInfo = (CCtxQmc)pSci.pVar;

            ulong len;

            //
            showInfo_open(0, null, "initVar_isGui enters");

            //
            pProcInfo.pQyMc = pQyMc;
            //pProcInfo.pSgiParam = pSci;


            //  2013/06/08
            /*
			pProcInfo.p_gAvParams = get_g_pAvParams();//&gAvParams;
			if (!pProcInfo.p_gAvParams) goto errLabel;
			memset(pProcInfo.p_gAvParams, 0, sizeof(PolicyAvParams));
			*/
            pProcInfo.p_gAvParams = new PolicyAvParams();

            //
            pProcInfo.pFuncs_for_isCliHelp = new FUNCS_for_isCliHelp();
            if (pProcInfo.pFuncs_for_isCliHelp == null) goto errLabel;

            //
            //_sntprintf(pProcInfo.who_showInfo, mycountof(pProcInfo.who_showInfo), _T("%s"), pQyMc.who_showInfo);
            pProcInfo.str_who_showInfo = "qmc";

            //
            pProcInfo.cfg.uiMaxSendMsgs = Consts.CONST_maxSendMsgs_isClient;
            pProcInfo.cfg.usMaxMsgrRecentFriends = Consts.CONST_usMaxMsgrRecentFriends;

            //  2014/07/13
            pProcInfo.cfg.uiSizePerSnd_file_noAv = Consts.DEFAULT_uiSizePerSnd_file_noAv;
            pProcInfo.cfg.uiSizePerSnd_file_avExists = Consts.DEFAULT_uiSizePerSnd_file_avExists;

            //
            pProcInfo.cfg.uiSizePerSnd_media = Consts.DEFAULT_uiSizePerSnd_media_isCli;       //  2008/04/01
            pProcInfo.cfg.usNeedShowFrameInfoIntervalInS = Consts.DEFAULT_usNeedShowFrameInfoIntervalInS;

            //
            pProcInfo.cfg.uiBufSize_myDrawVideoData = Consts.bufSize_myDrawVideoData_4k;    //  2014/06/01
                                                                                            //if (!b4Core() || bLikeXp())
            {
                pProcInfo.cfg.uiBufSize_myDrawVideoData = Consts.bufSize_myDrawVideoData_2k;    //  2014/06/01
            }
            pProcInfo.cfg.uiBufSize_myDrawVideoData = 1;    // for mediaCodec.
                                                            //
            pProcInfo.cfg.uiBufSize_myPlayAudioData = Consts.bufSize_myPlayAudioData1;   //  2010/04/24

            //  2010/06/26	 

            pProcInfo.cfg.envVar.usMaxQNodes_mediaQ = Consts.CONST_uiMaxQNodes_mediaQ_x64;

            pProcInfo.cfg.envVar.usQNodes_warningInterval_mediaQ = Consts.CONST_uiQNodes_warningInterval_mediaQ;
            //
            pProcInfo.cfg.envVar.usMaxQNodes_rtMediaQ = Consts.CONST_uiMaxQNodes_mediaQ_x64;
            pProcInfo.cfg.envVar.usQNodes_warningInterval_rtMediaQ = Consts.CONST_uiQNodes_warningInterval_mediaQ;
            //  2016/12/26
            pProcInfo.cfg.envVar.usMaxQNodes_rtOpQ = pProcInfo.cfg.envVar.usMaxQNodes_rtMediaQ;
            pProcInfo.cfg.envVar.usQNodes_warningInterval_rtOpQ = pProcInfo.cfg.envVar.usQNodes_warningInterval_rtMediaQ;

            //
            //pProcInfo.cfg.envVar.usMaxQNodes_vCamQ  =  CONST_uiMaxQNodes_vCamQ;
            //
            pProcInfo.cfg.envVar.usMaxQNodes_playSoundQ = Consts.CONST_uiMaxQNodes_playSoundQ;
            //  2012/11/09
            pProcInfo.cfg.envVar.usMaxQNodes_playVideoQ = Consts.CONST_uiMaxQNodes_playVideoQ;
            pProcInfo.cfg.envVar.usMaxQNodes_displayVideoQ = Consts.CONST_uiMaxQNodes_displayVideoQ;

            //
            pProcInfo.cfg.envVar.usMaxQNodes_dlgTalk_processQ = 200;       //  

            pProcInfo.cfg.envVar.usMaxQNodes_imMsgLogQ = 200;


            //  2017/08/15
            pProcInfo.cfg.envVar.default_ucbLetConfMgrSetMicOn = false;

            //  2014/09/22
            pProcInfo.cfg.ucb__SUPPORT_shareDynBmps__ = false;

            //
            //
            //

            //  2010/08/05
            //  if  (  bLikeXp(  )  )  
            pProcInfo.cfg.ucb__SUPPORT_dmoToDec__ = false;      //  2012/03/22, 因为filterGraph有死锁现象,所以,这里选择使用dmo.



            //
            pProcInfo.cfg.ucb__USE_hdEnc_conf__ = false;   //  2015/10/05	//  true;	//  FALSE;	//  2014/04/03

            //
            pProcInfo.cfg.ucb__USE_decObj_a_mediaCodec_async__ = false;// true;

            /*
						//  2015/12/04
						int size;
						size = sizeof(ZONE_objs);
						pProcInfo.pGZone_activeTalkers = (ZONE_objs*)mymalloc(size);
						if (!pProcInfo.pGZone_activeTalkers) goto errLabel;
						memset(pProcInfo.pGZone_activeTalkers, 0, size);
						pProcInfo.pGZone_activeTalkers->iIndex_enlarged1 = -1;     //  2016/03/25

						//  2016/03
						size = sizeof(ZONE_objs);
						pProcInfo.pGZone_d3dWalls = (ZONE_objs*)mymalloc(size);
						if (!pProcInfo.pGZone_d3dWalls) goto errLabel;
						memset(pProcInfo.pGZone_d3dWalls, 0, size);
						pProcInfo.pGZone_d3dWalls->iIndex_enlarged1 = -1;
			*/



            //
            //  2017/06/18
            init_webRtc();


            //
            if (GuiShare.pf_getPolicyIsClient(pProcInfo, out pProcInfo.cfg.policy) != 0) goto errLabel;


            /*
			//  2016/04/28
			pProcInfo.m_pf_doDecodeVideo_dvtCli = doDecodeVideo_dvtCli;
			//
			pProcInfo.m_pf_qdcInitCompressVideo_evtCli = qdcInitCompressVideo_evtCli;
			pProcInfo.m_pf_qdcExitCompressVideo_evtCli = qdcExitCompressVideo_evtCli;
			//
			pProcInfo.m_pf_qdcGetSpsPps_evtCli = qdcGetSpsPps_evtCli;
			//
			pProcInfo.m_pf_doEncodeVideo_evtCli_run = doEncodeVideo_evtCli_run;
			*/


            //
            //
            //
            pProcInfo.uiDevType = Consts.CONST_objType_pc;        //  2007/08/03

            //  authInfo

            //				
            fixed (QY_COMM_AUTHINFO_MIS* pAuthInfo = &pProcInfo.authInfo)
            {
                pAuthInfo->iAppType = pQyMc.iAppType;


                qyFuncs.safeStrnCpy(qyFuncs.qnmVerStr(pSci.cfg.iServiceId), pAuthInfo->ver, Consts.CONST_qyMaxVerLen + 1);

                //
                pAuthInfo->usLangId = pQyMc.env.usLangId;
                pAuthInfo->uiDevType = pProcInfo.uiDevType;
                //
                pAuthInfo->usAuthType = Consts.CONST_authType_logon;       //  2011/02/23

            }//

            /*
			//  2015/07/01
			pProcInfo.ucCnt_misCnts = CONST_maxMisCnts;
			size = pProcInfo.ucCnt_misCnts * sizeof(MIS_CNT);
			pProcInfo.pMisCnts = (MIS_CNT*)malloc(size);
			if (!pProcInfo.pMisCnts) goto errLabel;
			memset(pProcInfo.pMisCnts, 0, size);
			*/
            pProcInfo.pMisCnt = new MIS_CNT();
            if (pProcInfo.pMisCnt == null) goto errLabel;


            //
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;    // getMisCntByIndex(0, pProcInfo, i);
            if (null == pMisCnt) goto errLabel;

            {
                i = 0;
                //  pProcInfo.misCnts[index].cfg.dynCfg.cnt.usCntPort  =  DEFAULT_qyPort_mis;
                //  char  *  pServIp  =  "192.168.1.12";  //  CONST_ip_localhost
                //  safeStrnCpy(  pServIp,  pProcInfo.misCnts[index].cfg.dynCfg.cnt.cntIp,  mycountof(  pProcInfo.misCnts[index].cfg.dynCfg.cnt.cntIp  )  );
                //  2015/07/01
                //memcpy(&pMisCnt.cfg.dynCfg, &pQyMc.cfg.dynCfg, sizeof(pMisCnt.cfg.dynCfg)  );
                pMisCnt.cfg.dynCfg = pQyMc.cfg.dynCfg;
            }


            //  2015/09/06
            fixed (char* pName = pProcInfo.cfg.mgrQEx.common.name)
            {
                mytcsncpy(pName, "mgrQ", Consts.cntof_qCfg_name);
            }
            pProcInfo.cfg.mgrQEx.common.uiMaxQNodes = Consts.CONST_uiMaxQNodes_mgrQ_isCli;
            //_sntprintf(pProcInfo.cfg.mgrQEx.common.mutexName_prefix, mycountof(pProcInfo.cfg.mgrQEx.common.mutexName_prefix), _T("qMgrQ_isCli"));
            fixed (char* pName = pProcInfo.cfg.mgrQEx.common.mutexName_prefix)
            {
                mytcsncpy(pName, ("qMgrQ_isCli"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.mgrQEx.common.bUseEvt = true;
            pProcInfo.cfg.mgrQEx.pfQElemNew = mymalloc;
            pProcInfo.cfg.mgrQEx.pfQElemFree = myfree;
            //
            fixed (char* pName = pProcInfo.cfg.displayQ.name)
            {
                mytcsncpy(pName, ("displayQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.displayQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("qDispQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.displayQ.uiMaxCnt_semaTrigger = Consts.CONST_uiMaxCnt_semaTrigger_displayQ;
            pProcInfo.cfg.displayQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_displayQ;
            //			  
            fixed (char* pName = pProcInfo.cfg.workQ.name)
            {
                mytcsncpy(pName, "workQ", Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.workQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("qWorkQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.workQ.uiMaxCnt_semaTrigger = Consts.CONST_uiMaxCnt_semaTrigger_workQ;
            pProcInfo.cfg.workQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_workQ;
            //
            fixed (char* pName = pProcInfo.cfg.schedulerQ.name)
            {
                mytcsncpy(pName, ("schedulerQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.schedulerQ.mutexName_prefix)
            {
                mytcsncpy(pName, "qSchQ", Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.schedulerQ.uiMaxCnt_semaTrigger = Consts.CONST_uiMaxCnt_semaTrigger_schedulerQ;
            pProcInfo.cfg.schedulerQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_schedulerQ;
            //  2007/07/04	 			  
            fixed (char* pName = pProcInfo.cfg.robotQ.name)
            {
                mytcsncpy(pName, "robotQ", Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.robotQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("qRobotQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.robotQ.uiMaxCnt_semaTrigger = Consts.CONST_uiMaxCnt_semaTrigger_robotQ;
            pProcInfo.cfg.robotQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_robotQ;
            //
            fixed (char* pName = pProcInfo.cfg.mediaQ.name)
            {
                mytcsncpy(pName, ("mediaQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.mediaQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("qMediaQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.mediaQ.uiMaxQNodes = pProcInfo.cfg.envVar.usMaxQNodes_mediaQ;
            //
            //  2008/04/25
            fixed (char* pName = pProcInfo.cfg.realTimeMediaQ.name)
            {
                mytcsncpy(pName, ("rtMediaQ"), Consts.cntof_mutexName_prefix);
            }
            fixed (char* pName = pProcInfo.cfg.realTimeMediaQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("qRtMediaQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.realTimeMediaQ.uiMaxQNodes = pProcInfo.cfg.envVar.usMaxQNodes_rtMediaQ;

            //  2016/12/26
            fixed (char* pName = pProcInfo.cfg.rtOpQ.name)
            {
                mytcsncpy(pName, ("rtOpQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.rtOpQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("qRtOpQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.rtOpQ.uiMaxQNodes = pProcInfo.cfg.envVar.usMaxQNodes_rtOpQ;

            //
            //  2007/07/04
            //
            fixed (char* pName = pProcInfo.cfg.processQ_robot.name)
            {
                mytcsncpy(pName, ("processQ_robot"), Consts.cntof_q2Cfg_name);
            }
            pProcInfo.cfg.processQ_robot.uiMaxQNodes = Consts.CONST_uiMaxQNodes_processQ_robot;
            //
            fixed (char* pName = pProcInfo.cfg.processQ_media.name)
            {
                mytcsncpy(pName, ("processQ_media"), Consts.cntof_q2Cfg_name);
            }
            pProcInfo.cfg.processQ_media.uiMaxQNodes = Consts.CONST_uiMaxQNodes_processQ_media;

            //  2008/05/23
            fixed (char* pName = pProcInfo.cfg.outputQ_toMix_audio.name)
            {
                mytcsncpy(pName, ("output_mixQ_a"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.outputQ_toMix_audio.mutexName_prefix)
            {
                mytcsncpy(pName, ("qoutput_mixQ_a"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.outputQ_toMix_audio.uiMaxQNodes = Consts.CONST_uiMaxQNodes_outputQ_toMix_audio;

            //  2009/07/26
            fixed (char* pName = pProcInfo.cfg.outputQ_toMosaic_video.name)
            {
                mytcsncpy(pName, ("output_mixQ_v"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.outputQ_toMosaic_video.mutexName_prefix)
            {
                mytcsncpy(pName, ("qOutput_mixQ_v"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.outputQ_toMosaic_video.uiMaxQNodes = Consts.CONST_uiMaxQNodes_outputQ_toMosaic_video;

            //	 2008/04/17
            fixed (char* pName = pProcInfo.cfg.playSoundQ.name)
            {
                mytcsncpy(pName, ("playSoundQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.playSoundQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("playSoundQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.playSoundQ.uiMaxQNodes = pProcInfo.cfg.envVar.usMaxQNodes_playSoundQ;   //  isMem512(  )  ?  CONST_uiMaxQNodes_playSoundQ_256  :  CONST_uiMaxQNodes_playSoundQ;

            fixed (char* pName = pProcInfo.cfg.playVideoQ.name)
            {
                mytcsncpy(pName, ("playVideoQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.playVideoQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("playVideoQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.playVideoQ.uiMaxQNodes = pProcInfo.cfg.envVar.usMaxQNodes_playVideoQ;   //  isMem512(  )  ?  CONST_uiMaxQNodes_playVideoQ_256  :  CONST_uiMaxQNodes_playVideoQ;

            fixed (char* pName = pProcInfo.cfg.displayVideoQ.name)
            {
                mytcsncpy(pName, ("displayVQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.displayVideoQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("displayVQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.displayVideoQ.uiMaxQNodes = pProcInfo.cfg.envVar.usMaxQNodes_displayVideoQ; //  CONST_uiMaxQNodes_displayVideoQ;


            //  2011/12/09
            //  2009/05/12
            //
            //char tName[128];  //  2015/05/23
            //_sntprintf(  tName,  mycountof(  tName  ),  _T(  "transQ-%d"  ),  GetCurrentProcessId(  )  );
            //getTransformQName(tName, mycountof(tName));
            fixed (char* pName = pProcInfo.cfg.transformQ.name)
            {
                mytcsncpy(pName, "transQ", Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.transformQ.mutexName_prefix)
            {
                mytcsncpy(pName, "transQ", Consts.cntof_mutexName_prefix);
            }
            //
            pProcInfo.cfg.transformQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_transformQ;

            //  2010/06/18

            //  2017/01/29
            fixed (char* pName = pProcInfo.cfg.appQ.name)
            {
                mytcsncpy(pName, ("appQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.appQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("appQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.appQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_appQ;


            //  2011/12/04
            fixed (char* pName = pProcInfo.cfg.vppQ.name)
            {
                mytcsncpy(pName, ("vppQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.vppQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("vppQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.vppQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_vppQ;

            //  2011/01/21
            fixed (char* pName = pProcInfo.cfg.toShareAudioQ.name)
            {
                mytcsncpy(pName, ("shareAQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.toShareAudioQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("shareAQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.toShareAudioQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_toShareAudioQ;
            //  2009/05/12
            fixed (char* pName = pProcInfo.cfg.toShareVideoQ.name)
            {
                mytcsncpy(pName, ("shareVQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.toShareVideoQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("shareVQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.toShareVideoQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_toShareVideoQ;

            //  2011/12/06
            fixed (char* pName = pProcInfo.cfg.toShareVideo_postQ.name)
            {
                mytcsncpy(pName, ("shareVPostQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.toShareVideo_postQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("shareVPostQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.toShareVideo_postQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_toShareVideo_postQ;

            //  2009/06/17
            fixed (char* pName = pProcInfo.cfg.dispatchQ.name)
            {
                mytcsncpy(pName, ("dispatchQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.dispatchQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("dsptchQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.dispatchQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_dispatchQ;

            //  2016/03/15
            fixed (char* pName = pProcInfo.cfg.talkerThreadQ.name)
            {
                mytcsncpy(pName, ("talkerThreadQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.talkerThreadQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("talkerThreadQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.talkerThreadQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_talkerThreadQ;

            //  2016/06/15
            fixed (char* pName = pProcInfo.cfg.shareDynBmpsQ.name)
            {
                mytcsncpy(pName, ("shareDynBmpsQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.shareDynBmpsQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("shareDynBmpsQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.shareDynBmpsQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_shareDynBmpsQ;

            //
            fixed (char* pName = pProcInfo.cfg.guestQ.name)
            {
                mytcsncpy(pName, ("guestQ"), Consts.cntof_q2Cfg_name);
            }
            pProcInfo.cfg.guestQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_guestQ;
            //
            //
            fixed (char* pName = pProcInfo.cfg.dynBmpQ.name)
            {
                mytcsncpy(pName, ("dynBmpQ"), Consts.cntof_q2Cfg_name);
            }
            fixed (char* pName = pProcInfo.cfg.dynBmpQ.mutexName_prefix)
            {
                mytcsncpy(pName, ("dynBmpQ"), Consts.cntof_mutexName_prefix);
            }
            pProcInfo.cfg.dynBmpQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_dynBmpQ;


            //
            fixed (char* pName = pProcInfo.cfg.mutexName_syncSendAv)
            {
                mytcsncpy(pName, ("qySyncSendAv"), Consts.cntof_mutexName);
            }
            fixed (char* pName = pProcInfo.cfg.mutexName_syncActiveMem_prefix1)
            {
                mytcsncpy(pName, ("qySyncAct"), Consts.cntof_mutexName_prefix);
            }
            fixed (char* pName = pProcInfo.cfg.mutexName_syncStartAv)
            {
                mytcsncpy(pName, ("qyStartAv"), Consts.cntof_mutexName);
            }

            //
            //  if  (  initGenericQ((  &pProcInfo.cfg.mgrQ,  mallocMemory,  NULL,  0,  0,  freeMemory,  NULL,  NULL,  &pProcInfo.mgrQ  )  )  goto  errLabel; 
            //  2015/09/06
            pProcInfo.cfg.mgrQEx.pfQElemNewEx = mallocMemory;
            pProcInfo.cfg.mgrQEx.pfQElemFreeEx = freeMemory;
            pProcInfo.mgrQ = new GENERIC_Q();
            if (initGenericQ(pProcInfo.cfg.mgrQEx, pProcInfo.mgrQ) != 0) goto errLabel;
            //
            //  2007/07/04	 			  
            pProcInfo.robotQ = new GENERIC_Q();
            if (initGenericQ(pProcInfo.cfg.robotQ, mymalloc, null, null, myfree, null, pProcInfo.robotQ) != 0) goto errLabel;
            pProcInfo.displayQ = new GENERIC_Q();
            if (initGenericQ(pProcInfo.cfg.displayQ, mymalloc, null, null, myfree, null, pProcInfo.displayQ) != 0) goto errLabel;
            pProcInfo.workQ = new GENERIC_Q();
            if (initGenericQ(pProcInfo.cfg.workQ, mymalloc, null, null, myfree, null, pProcInfo.workQ) != 0) goto errLabel;
            pProcInfo.schedulerQ = new GENERIC_Q();
            if (initGenericQ(pProcInfo.cfg.schedulerQ, mymalloc, null, null, myfree, null, pProcInfo.schedulerQ) != 0) goto errLabel;


            pProcInfo.mediaQ2 = new QY_Q2();
            if (initQyQ2(pProcInfo.cfg.mediaQ, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pProcInfo.mediaQ2) != 0) goto errLabel;
            pProcInfo.realTimeMediaQ2 = new QY_Q2();
            if (initQyQ2(pProcInfo.cfg.realTimeMediaQ, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pProcInfo.realTimeMediaQ2) != 0) goto errLabel;

            //  2016/12/26
            pProcInfo.rtOpQ2 = new QY_Q2();
            if (initQyQ2(pProcInfo.cfg.rtOpQ, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pProcInfo.rtOpQ2) != 0) goto errLabel;


            //
            //  ×¢Òâ£º ÏÂÃæµÄ5±íÊ¾×î¶àÓÐ5¸ö²¢·¢Ïß³Ì¿ÉÒÔÓÃ¡£Ò²Ðí²»¹»ÄØ¡£2008/05/18
            //
            //
            pProcInfo.processQ_media = new CMcSharedQ();
            if (!pProcInfo.processQ_media.bSetQInfo(("mcSyncMedia"), 10, 10000, 10000, 10000, 10000)) goto errLabel;
            if (pProcInfo.processQ_media.initQ(pProcInfo.cfg.processQ_media, pQyMc.cfg.rwLockParam, mcTask_new, initMisMsg, exitMisMsg, mcTask_free) != 0) goto errLabel;


            //  2010/04/22

            //
            pProcInfo.imMsgLogQ = new GENERIC_Q();
            if (0 != initGenericQ(("ImMsgLogQ"), ("ImMsgLogQ"), 0, pProcInfo.cfg.envVar.usMaxQNodes_imMsgLogQ, mymalloc, myfree, pProcInfo.imMsgLogQ)) goto errLabel;

            //
            //  2008/02/01

            //
            pMisCnt = pProcInfo.pMisCnt;

            //
            pMisCnt.pProcInfoParam = pProcInfo;    //  2009/09/07

            //
            pMisCnt.channels = new MIS_CHANNEL[Consts.CONST_maxConnsPerCli_mis];
            for (int iii = 0; iii < pMisCnt.channels.Length; iii++)
            {
                pMisCnt.channels[iii] = new MIS_CHANNEL();
            }
            //
            pMisCnt.channels[0].uiType = Consts.CONST_channelType_talking;
            pMisCnt.channels[1].uiType = Consts.CONST_channelType_robot;
            pMisCnt.channels[2].uiType = Consts.CONST_channelType_media;
            pMisCnt.channels[3].uiType = Consts.CONST_channelType_realTimeMedia;  //  2008/04/17
            pMisCnt.channels[4].uiType = Consts.CONST_channelType_rtOp;   //  2008/04/17


            //
            //if (pMisCnt.cfg.dynCfg.cnt.usCntPort!=0)
            {

                //  2007/06/03
                pMisCnt.cfg.iObjQType = Consts.CONST_objQType_misClient;
                fixed (char* pName = pMisCnt.cfg.mutexName_syncQmObjQ)
                {
                    mytcsncpy(pName, Consts.DEFAULT_mutexName_syncQmObjQ_client, Consts.cntof_mutexName); //  2007/01/16
                }
                fixed (char* pName = pMisCnt.cfg.semaphoreName_syncQmObjQ)
                {
                    mytcsncpy(pName, Consts.DEFAULT_semaphoreName_syncQmObjQ_client, Consts.cntof_semName); //  2007/01/16
                }
                pMisCnt.cfg.uiMaxCnt_semaphore_qmObjQ = Consts.CONST_maxCnt_sema_syncQmObjQ;

                //
                //  pMisCnt.cfg.uiMaxCnt_semaTrigger_input	=	CONST_uiMaxCnt_semaTrigger_input;
                //
                //  2008/04/02			 
                fixed (char* pName = pMisCnt.cfg.inCacheQ.name)
                {
                    mytcsncpy(pName, ("inCacheQ"), Consts.cntof_q2Cfg_name);
                }
                fixed (char* pName = pMisCnt.cfg.inCacheQ.mutexName_prefix)
                {
                    mytcsncpy(pName, ("qmcInCacheQ"), Consts.cntof_mutexName_prefix);
                }
                pMisCnt.cfg.inCacheQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_inCacheQ_isCli;
                //

                //
                fixed (char* pName = pMisCnt.cfg.toSendQ.name)
                {
                    mytcsncpy(pName, ("toSendQ"), Consts.cntof_q2Cfg_name);
                }
                fixed (char* pName = pMisCnt.cfg.toSendQ.mutexName_prefix)
                {
                    mytcsncpy(pName, ("qmcToSendQ"), Consts.cntof_mutexName_prefix);
                }
                //  pMisCnt.cfg.toSendQ.uiMaxCnt_semaTrigger  =  CONST_uiMaxCnt_semaTrigger_toSendQ;				  
                pMisCnt.cfg.toSendQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_toSendQ;

                //
                fixed (char* pName = pMisCnt.cfg.outputQ.name)
                {
                    mytcsncpy(pName, ("outputQ"), Consts.cntof_q2Cfg_name);
                }
                fixed (char* pName = pMisCnt.cfg.outputQ.mutexName_prefix)
                {
                    mytcsncpy(pName, ("qmcOutputQ"), Consts.cntof_mutexName_prefix);
                }
                //  pMisCnt.cfg.outputQ.uiMaxCnt_semaTrigger  =  CONST_uiMaxCnt_semaTrigger_outputQ;
                pMisCnt.cfg.outputQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_outputQ;

                //  2007/07/04
                //
                fixed (char* pName = pMisCnt.cfg.talkingFriendQ.name)
                {
                    mytcsncpy(pName, ("talkingFriendQ"), Consts.cntof_q2Cfg_name);
                }
                fixed (char* pName = pMisCnt.cfg.talkingFriendQ.mutexName_prefix)
                {
                    mytcsncpy(pName, ("qyMcTalkQ"), Consts.cntof_q2Cfg_name);
                };
                pMisCnt.cfg.talkingFriendQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_talkingFriendQ;
                //
                fixed (char* pName = pMisCnt.cfg.recentFriendQ.name)
                {
                    mytcsncpy(pName, ("recentFriendQ"), Consts.cntof_q2Cfg_name);
                }
                fixed (char* pName = pMisCnt.cfg.recentFriendQ.mutexName_prefix)
                {
                    mytcsncpy(pName, ("qyMcRecentQ"), Consts.cntof_mutexName_prefix);
                }
                pMisCnt.cfg.recentFriendQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_recentFriendQ;

                //
                fixed (char* pName = pMisCnt.cfg.imObj_intQ.name)
                {
                    mytcsncpy(pName, "imObj_intQ", Consts.cntof_q2Cfg_name);
                }
                fixed (char* pName = pMisCnt.cfg.imObj_intQ.mutexName_prefix)
                {
                    mytcsncpy(pName, "imObj_intQ", Consts.cntof_mutexName_prefix);
                }
                pMisCnt.cfg.imObj_intQ.uiMaxQNodes = Consts.CONST_uiMaxQNodes_imObj_intQ;


                //  2007/06/06
                pMisCnt.cfg.usIntervalInS_getIntervalParams = Consts.CONST_usIntervalInS_getIntervalParams;
                //
                pMisCnt.cfg.usMaxMsgrRecentFriends = pProcInfo.cfg.usMaxMsgrRecentFriends;
                pMisCnt.cfg.usIntervalInS_refreshRecentFriends = Consts.CONST_usIntervalInS_refreshRecentFriends;
                pMisCnt.cfg.usIntervalInS_refreshContactList = Consts.CONST_usIntervalInS_refreshContactList;
                //
                pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_grp = Consts.CONST_maxContactUpdateInfos_grp;   //  2014/02/06.	CONST_usMaxContactUpdateInfosPerSnd_isCli;			  	
                pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_grpMem = Consts.CONST_maxContactUpdateInfos_grpMem; //  2014/02/06.	CONST_usMaxContactUpdateInfosPerSnd_isCli;
                pMisCnt.cfg.usMaxContactUpdateInfosPerSnd_contact = Consts.CONST_maxContactUpdateInfos_contact;   //  2014/02/06.	CONST_usMaxContactUpdateInfosPerSnd_isCli;

                //
                pMisCnt.cfg.usIntervalInS_tryToReg = Consts.CONST_usIntervalInS_tryToReg;
                pMisCnt.cfg.usIntervalInS_tryToSndDevInfo = Consts.CONST_usIntervalInS_tryToSndDevInfo;   //  2007/

                //  2007/06/27
                fixed (char* pName = pMisCnt.cfg.mutexNamePrefix_syncTask)
                {
                    mytcsncpy(pName, Consts.DEFAULT_mutexNamePrefix_syncTask, Consts.cntof_mutexName_prefix);
                }

                //  2007/07/16
                //  


                /////////////////
                //
                //

#if false
				//  2007/06/03
				if (!(pMisCnt.pObjQ = mymalloc(sizeof(QM_OBJQ))))
				{       //  2007/04/01
					traceLogA("initVar_misClient(  ):  malloc for objQ failed");
					goto errLabel;
				}
				memset(pMisCnt.pObjQ, 0, sizeof(QM_OBJQ));
				if (initQmObjQ(0, &pQyMc.licenseCtx, pMisCnt.cfg.iObjQType, CQyString(pMisCnt.cfg.misServName) + _T("qmObjQ"), pMisCnt.cfg.mutexName_syncQmObjQ, pMisCnt.cfg.semaphoreName_syncQmObjQ, pMisCnt.cfg.uiMaxCnt_semaphore_qmObjQ, myCompare_misClient, (PF_commonHandler)exitQMem_misClient, (PF_fillQmObjMemInfo)fillQmObjMemInfo_misClient, (PF_commonHandler)tmp_addToQmObjQ_misClient, (QM_OBJQ*)pMisCnt.pObjQ))
				{       //  2006/12/07
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("127.0.0.1"), 0, _T("(Server)"), _T("initQmObjQ failed"), _T(""));
					traceLogA("initQmObjQ(  ) failed.");
					goto errLabel;
				}
				((QM_OBJQ*)pMisCnt.pObjQ)->pfPrintQ = (PF_commonHandler)printQmObjQ_isClient;
				pMisCnt.ucbQmObjQInited = true;
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, pMisCnt.cfg.misServName, _T("initQmObjQ OK"), _T(""));
#endif
                //
                int j;
                GENERIC_Q_CFG tmpqCfg;

                for (j = 0; j < pMisCnt.channels.Length; j++)
                {
                    MIS_CHANNEL pChannel = pMisCnt.channels[j];
                    if (pChannel == null) continue;

                    //  2016/09/09
                    pChannel.pMisCnt = pMisCnt;
                    //  
                    //memcpy(&tmpqCfg, &pMisCnt.cfg.inCacheQ, sizeof(tmpqCfg));
                    tmpqCfg = pMisCnt.cfg.inCacheQ;
                    //_sntprintf(tmpqCfg.name, mycountof(tmpqCfg.name), _T("%s-%u"), tmpqCfg.name, j);
                    str = String.Format("{0}-{1}", new string(tmpqCfg.name), j);
                    mytcsncpy(tmpqCfg.name, str, Consts.cntof_q2Cfg_name);
                    //_sntprintf(tmpqCfg.mutexName_prefix, mycountof(tmpqCfg.mutexName_prefix), _T("%s-%u"), tmpqCfg.mutexName_prefix, j);
                    str = string.Format("{0}-{1}", new string(tmpqCfg.mutexName_prefix), j);
                    mytcsncpy(tmpqCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                    pChannel.inCacheQ = new GENERIC_Q();
                    if (initGenericQ(tmpqCfg, mymalloc, null, null, myfree, null, pChannel.inCacheQ) != 0) goto errLabel;

                    //  2015/09/09

                    //
                    //memcpy(&tmpqCfg, &pMisCnt.cfg.toSendQ, sizeof(tmpqCfg));
                    tmpqCfg = pMisCnt.cfg.toSendQ;
                    //_sntprintf(tmpqCfg.name, mycountof(tmpqCfg.name), _T("%s-%u"), tmpqCfg.name, j);
                    //_sntprintf(tmpqCfg.mutexName_prefix, mycountof(tmpqCfg.mutexName_prefix), _T("%s-%u"), tmpqCfg.mutexName_prefix, j);
                    //  if  (  initGenericQ(  &tmpqCfg,  mymalloc,  0,  0,  myfree,  &pChannel->toSendQ  )  )  goto  errLabel;
                    str = String.Format("{0}-{1}", new string(tmpqCfg.name), j);
                    mytcsncpy(tmpqCfg.name, str, Consts.cntof_q2Cfg_name);
                    str = string.Format("{0}-{1}", new string(tmpqCfg.mutexName_prefix), j);
                    mytcsncpy(tmpqCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                    pChannel.toSendQ2 = new QY_Q2();
                    if (initQyQ2(tmpqCfg, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pChannel.toSendQ2) != 0) goto errLabel;

                    //
                    //memcpy(&tmpqCfg, &pMisCnt.cfg.outputQ, sizeof(tmpqCfg));
                    tmpqCfg = pMisCnt.cfg.outputQ;
                    //_sntprintf(tmpqCfg.name, mycountof(tmpqCfg.name), _T("%s-%u"), tmpqCfg.name, j);
                    //_sntprintf(tmpqCfg.mutexName_prefix, mycountof(tmpqCfg.mutexName_prefix), _T("%s-%u"), tmpqCfg.mutexName_prefix, j);
                    //  if  (  initGenericQ(  &tmpqCfg,  mymalloc,  0,  0,  myfree,  &pChannel->outputQ  )  )  goto  errLabel;  
                    str = String.Format("{0}-{1}", new string(tmpqCfg.name), j);
                    mytcsncpy(tmpqCfg.name, str, Consts.cntof_q2Cfg_name);
                    str = string.Format("{0}-{1}", new string(tmpqCfg.mutexName_prefix), j);
                    mytcsncpy(tmpqCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                    pChannel.outputQ2 = new QY_Q2();
                    if (initQyQ2(tmpqCfg, pQyMc.cfg.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, pChannel.outputQ2) != 0) goto errLabel;


                }

                //
                //pMisCnt.talkingFriendQ = new GENERIC_Q();
                //if (initGenericQ(pMisCnt.cfg.talkingFriendQ, mymalloc, null, null, myfree, null, pMisCnt.talkingFriendQ)!=0) goto errLabel;
                pMisCnt.talkingFriendQ = new List<MIS_MSG_TALKINGFRIEND_QMC>();
                //
                pMisCnt.recentFriendQ = new GENERIC_Q();
                if (initGenericQ(pMisCnt.cfg.recentFriendQ, mymalloc, null, null, myfree, null, pMisCnt.recentFriendQ) != 0) goto errLabel;


                //  2007/08/07, ¸³ÎÒµÄÈ±Ê¡Öµ
                //lstrcpyn(pMisCnt.displayName, getResStr(0, &pQyMc.cusRes, CONST_resId_objIdStr_me), mycountof(pMisCnt.displayName));

                //
                pMisCnt.imObj_intQ = new GENERIC_Q();
                if (initGenericQ(pMisCnt.cfg.imObj_intQ, mymalloc, null, null, myfree, null, pMisCnt.imObj_intQ) != 0) goto errLabel;


            }

            //
            pMisCnt.hMutex_syncTask = new Mutex();
            pMisCnt.hMutex_syncStartAv = new Mutex();
            pMisCnt.hMutex_syncSendAv = new Mutex();
            pMisCnt.hMutex_syncActiveMem = new Mutex();
            pMisCnt.hMutex_syncIndex_dec = new Mutex();
            //
            pMisCnt.hMutex_syncShareEglContext = new Mutex();
            //
            pProcInfo.hMutex_syncQdcUnit = new Mutex();
            //
            string pName1 = "notify_thread_isCli";
            //pProcInfo.hEvt_notify_thread_isCli = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, pName1);
            pProcInfo.hEvt_notify_thread_isCli = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
            //
            //pProcInfo.hMutex_sync_mTableItems_imObj = new Mutex();




            //  2012/05/08
            //if (pProcInfo.initCapSubWnds1(DEFAULT_usMaxCnt_capSubWnds, &pMisCnt.subWnds)) goto errLabel;

            //

            //
            pProcInfo.av.localAv.player = new QY_PLAYER();


            //  2009/02/11

            GENERIC_Q_cfgEx qCfgEx = new GENERIC_Q_cfgEx(); //  2012/11/10
                                                            //
                                                            //  if  (  initGenericQ(  &pProcInfo.cfg.playSoundQ,  mymalloc,  0,  0,  myfree,  &pProcInfo.av.localAv.player.audio.q  )  )  goto  errLabel;
            pProcInfo.av.localAv.player.audio.q2 = new QY_Q2();
            if (0 != initQyQ2(pProcInfo.cfg.playSoundQ, pQyMc.cfg.rwLockParam, 2, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, (PF_qElemRemove)clean_myPLAY_AUDIO_DATA, pProcInfo.av.localAv.player.audio.q2)) goto errLabel;
            pProcInfo.av.localAv.player.audio.bQInited = true;
            pProcInfo.av.localAv.player.audio.qBuf2 = new QY_qBuf2();
            int maxDataBufLen_playA = 48000 * 2 * 3 / 2;  // 1.5 seconds
            string name = "";
            fixed (char* pName = pProcInfo.cfg.playSoundQ.name)
            {
                name = new string(pName);
            }
            if (0 != pProcInfo.av.localAv.player.audio.qBuf2.initQyQ2(Consts.CONST_qType_playA, pProcInfo.cfg.playSoundQ.uiMaxQNodes, maxDataBufLen_playA, pQyMc.cfg.rwLockParam, false, name)) goto errLabel;
            //
            //  if  (  initQyQ2(  &pProcInfo.cfg.playVideoQ,  &pQyMc.cfg.rwLockParam,  mycountof(  pProcInfo.av.localAv.player.video.q2.hEvents  ),  null,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  ( PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pProcInfo.av.localAv.player.video.q2  )  )  goto  errLabel;
            //memset(&qCfgEx, 0, sizeof(qCfgEx));
            //memcpy(&qCfgEx.common, &pProcInfo.cfg.playVideoQ, sizeof(qCfgEx.common));
            qCfgEx.common = pProcInfo.cfg.playVideoQ;
            qCfgEx.pfQElemNewEx = mallocMemory;
            qCfgEx.pfQElemNew = mymalloc;
            qCfgEx.pfQElemFreeEx = freeMemory;
            qCfgEx.pfQElemFree = myfree;
            qCfgEx.pf_QElemRemoveEx = (PF_qElemRemoveEx)cleanEx_myDRAW_VIDEO_DATA;
            qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_myDRAW_VIDEO_DATA;
            pProcInfo.av.localAv.player.video.q2 = new QY_Q2();
            if (initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, null, pProcInfo.av.localAv.player.video.q2) != 0) goto errLabel;
            pProcInfo.av.localAv.player.video.bQInited = true;
            //
            //  if  (  initQyQ2(  &pProcInfo.cfg.displayVideoQ,  &pQyMc.cfg.rwLockParam,  0,  null,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_MIS_MSG_procVideo_qmc,  &pProcInfo.av.localAv.player.video.displayQ2  )  )  goto  errLabel;
            //memset(&qCfgEx, 0, sizeof(qCfgEx));
            //memcpy(&qCfgEx.common, &pProcInfo.cfg.displayVideoQ, sizeof(qCfgEx.common));
            qCfgEx.common = pProcInfo.cfg.displayVideoQ;
            qCfgEx.pfQElemNewEx = mallocMemory;
            qCfgEx.pfQElemNew = mymalloc;
            qCfgEx.pfQElemFreeEx = freeMemory;
            qCfgEx.pfQElemFree = myfree;
            qCfgEx.pf_QElemRemoveEx = cleanEx_MIS_MSG_procVideo_qmc;
            qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_MIS_MSG_procVideo_qmc;
            pProcInfo.av.localAv.player.video.displayQ2 = new QY_Q2();
            if (initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, null, pProcInfo.av.localAv.player.video.displayQ2) != 0) goto errLabel;
            pProcInfo.av.localAv.player.video.ucbDisplayQInited = true;


            //  Æô¶¯ÒôÆµ³Ø
            pProcInfo.av.usCnt_players = pProcInfo.cfg.policy.usMax_nPlayers;     //  isWinCe(  )  ?  DEFAULT_maxusCnt_players_ce  :  DEFAULT_maxusCnt_players;	
            pProcInfo.av.pPlayers = new QY_PLAYER[pProcInfo.av.usCnt_players];// (QY_PLAYER*)mymalloc(sizeof(QY_PLAYER) * pProcInfo.av.usCnt_players);
            if (null == pProcInfo.av.pPlayers) goto errLabel;
            //memset(pProcInfo.av.pPlayers, 0, sizeof(QY_PLAYER) * pProcInfo.av.usCnt_players);

            for (i = 0; i < pProcInfo.av.usCnt_players; i++)
            {
                //
                pProcInfo.av.pPlayers[i] = new QY_PLAYER();

                //
                GENERIC_Q_CFG tmpCfg;
                //
                //memcpy(&tmpCfg, &pProcInfo.cfg.playSoundQ, sizeof(tmpCfg));
                tmpCfg = pProcInfo.cfg.playSoundQ;

                str = string.Format("{0}{1}", new string(tmpCfg.name), i);
                //_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s%d"), tmpCfg.name, i);
                mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
                //_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s%d"), tmpCfg.mutexName_prefix, i);
                str = string.Format("{0}{1}", new string(tmpCfg.mutexName_prefix), i);
                mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                //  if  (  initGenericQ(  &tmpCfg,  mymalloc,  0,  0,  free,  &pProcInfo.av.pPlayers[i].audio.q  )  )  goto  errLabel;
                pProcInfo.av.pPlayers[i].audio.q2 = new QY_Q2();
                if (initQyQ2(tmpCfg, pQyMc.cfg.rwLockParam, 2, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, (PF_qElemRemove)clean_myPLAY_AUDIO_DATA, pProcInfo.av.pPlayers[i].audio.q2) != 0) goto errLabel;
                pProcInfo.av.pPlayers[i].audio.bQInited = true;
                pProcInfo.av.pPlayers[i].audio.qBuf2 = new QY_qBuf2();
                name = new string(tmpCfg.name);
                if (pProcInfo.av.pPlayers[i].audio.qBuf2.initQyQ2(Consts.CONST_qType_playA, tmpCfg.uiMaxQNodes, maxDataBufLen_playA, pQyMc.cfg.rwLockParam, false, name) != 0) goto errLabel;
                //
                //memcpy(&tmpCfg, &pProcInfo.cfg.playVideoQ, sizeof(tmpCfg));
                tmpCfg = pProcInfo.cfg.playVideoQ;
                str = string.Format("{0}{1}", new string(tmpCfg.name), i);
                //_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s%d"), tmpCfg.name, i);
                mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
                //_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s%d"), tmpCfg.mutexName_prefix, i);
                str = string.Format("{0}{1}", new string(tmpCfg.mutexName_prefix), i);
                mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                //  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  mycountof(  pProcInfo.av.pPlayers[i].video.q2.hEvents  ),  null,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_myDRAW_VIDEO_DATA,  &pProcInfo.av.pPlayers[i].video.q2  )  )  goto  errLabel;
                //memset(&qCfgEx, 0, sizeof(qCfgEx));
                //memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
                qCfgEx.common = tmpCfg;
                qCfgEx.pfQElemNewEx = mallocMemory;
                qCfgEx.pfQElemNew = mymalloc;
                qCfgEx.pfQElemFreeEx = freeMemory;
                qCfgEx.pfQElemFree = myfree;
                qCfgEx.pf_QElemRemoveEx = cleanEx_myDRAW_VIDEO_DATA;
                qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_myDRAW_VIDEO_DATA;
                pProcInfo.av.pPlayers[i].video.q2 = new QY_Q2();
                if (initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, null, pProcInfo.av.pPlayers[i].video.q2) != 0) goto errLabel;
                pProcInfo.av.pPlayers[i].video.bQInited = true;
                //
                //memcpy(&tmpCfg, &pProcInfo.cfg.displayVideoQ, sizeof(tmpCfg));
                tmpCfg = pProcInfo.cfg.displayVideoQ;
                //_sntprintf(tmpCfg.name, mycountof(tmpCfg.name), _T("%s%d"), tmpCfg.name, i);
                str = string.Format("{0}{1}", new string(tmpCfg.name), i);
                mytcsncpy(tmpCfg.name, str, Consts.cntof_q2Cfg_name);
                //_sntprintf(tmpCfg.mutexName_prefix, mycountof(tmpCfg.mutexName_prefix), _T("%s%d"), tmpCfg.mutexName_prefix, i);
                str = string.Format("{0}{1}", new string(tmpCfg.mutexName_prefix), i);
                mytcsncpy(tmpCfg.mutexName_prefix, str, Consts.cntof_mutexName_prefix);
                //  if  (  initQyQ2(  &tmpCfg,  &pQyMc.cfg.rwLockParam,  0,  null,  mallocMemory,  mymalloc,  0,  0,  freeMemory,  myfree,  (  PF_qElemRemove  )clean_MIS_MSG_procVideo_qmc,  &pProcInfo.av.pPlayers[i].video.displayQ2  )  )  goto  errLabel;
                //memset(&qCfgEx, 0, sizeof(qCfgEx));
                //memcpy(&qCfgEx.common, &tmpCfg, sizeof(qCfgEx.common));
                qCfgEx.common = tmpCfg;
                qCfgEx.pfQElemNewEx = mallocMemory;
                qCfgEx.pfQElemNew = mymalloc;
                qCfgEx.pfQElemFreeEx = freeMemory;
                qCfgEx.pfQElemFree = myfree;
                qCfgEx.pf_QElemRemoveEx = cleanEx_MIS_MSG_procVideo_qmc;
                qCfgEx.pfQElemRemove = (PF_qElemRemove)clean_MIS_MSG_procVideo_qmc;
                pProcInfo.av.pPlayers[i].video.displayQ2 = new QY_Q2();
                if (initQyQ2(qCfgEx, pQyMc.cfg.rwLockParam, 2, null, pProcInfo.av.pPlayers[i].video.displayQ2) != 0) goto errLabel;
                pProcInfo.av.pPlayers[i].video.ucbDisplayQInited = true;
            }

            //  2012/05/07


            //
            pProcInfo.av.localAv.recordSoundProcInfo = new CAP_procInfo_recordSound();
            pProcInfo.av.localAv.videoCaptureProcInfo = new CAP_procInfo_video();



            //if (pProcInfo.bUse_sharedObj())
            {
                //  2008/10/04. 
                pProcInfo.cfg.usMaxCnt_pSharedObjs = Consts.DEFAULT_usMaxCnt_pSharedObjs;
                /*
			len = sizeof(QY_sharedObj_sync) * pProcInfo.cfg.usMaxCnt_pSharedObjs;
			if (!(pProcInfo.pSharedObjSyncs = mymalloc(len))) goto errLabel;
			memset(pProcInfo.pSharedObjSyncs, 0, len);
				*/
                pProcInfo.pSharedObjSyncs = new QY_sharedObj_sync[pProcInfo.cfg.usMaxCnt_pSharedObjs];
                for (i = 0; i < pProcInfo.pSharedObjSyncs.Length; i++)
                {
                    pProcInfo.pSharedObjSyncs[i] = new QY_sharedObj_sync();
                }

                //
                pProcInfo.cfg.usMaxCnt_capProcInfoBmps = pProcInfo.cfg.usMaxCnt_pSharedObjs;      //  DEFAULT_usMaxCnt_pSharedObjs;
                /*
			len = sizeof(CAP_procInfo_bmpU) * pProcInfo.cfg.usMaxCnt_capProcInfoBmps;
			if (!(pProcInfo.pCapProcInfoBmps = (CAP_procInfo_bmpU*)mymalloc(len))) goto errLabel;
			memset(pProcInfo.pCapProcInfoBmps, 0, len);
				*/
                pProcInfo.pCapProcInfoBmps = new CAP_procInfo_video[pProcInfo.cfg.usMaxCnt_capProcInfoBmps];
                for (i = 0; i < pProcInfo.pCapProcInfoBmps.Length; i++)
                {
                    pProcInfo.pCapProcInfoBmps[i] = new CAP_procInfo_video();
                }

                //
                pProcInfo.cfg.usMaxCnt_capProcInfoAudios = pProcInfo.cfg.usMaxCnt_pSharedObjs;        //  DEFAULT_usMaxCnt_pSharedObjs;
                /*
				len = sizeof(CAP_procInfo_audioU) * pProcInfo.cfg.usMaxCnt_capProcInfoAudios;
				if (!(pProcInfo.pCapProcInfoAudios = (CAP_procInfo_audioU*)mymalloc(len))) goto errLabel;
				memset(pProcInfo.pCapProcInfoAudios, 0, len);
				*/
                pProcInfo.pCapProcInfoAudios = new CAP_procInfo_recordSound[pProcInfo.cfg.usMaxCnt_capProcInfoAudios];
                for (i = 0; i < pProcInfo.pCapProcInfoAudios.Length; i++)
                {
                    pProcInfo.pCapProcInfoAudios[i] = new CAP_procInfo_recordSound();
                }

            }


            //
            pProcInfo.cfg.usMaxCnt_taskInfos = Consts.DEFAULT_usMaxCnt_taskInfos;
            //len = getSize_QMC_TASK_INFO() * pProcInfo->cfg.usMaxCnt_taskInfos;
            //if (!(pProcInfo->pQmcTaskInfos = mymalloc(len))) goto errLabel;
            //memset(pProcInfo->pQmcTaskInfos, 0, len);
            pProcInfo.pQmcTaskInfos = new QMC_TASK_INFO[pProcInfo.cfg.usMaxCnt_taskInfos];
            for (i = 0; i < pProcInfo.cfg.usMaxCnt_taskInfos; i++)
            {
                pProcInfo.pQmcTaskInfos[i] = new QMC_TASK_INFO();
            }


            //
            pProcInfo.av.texes = new TMP_texes();



            //  2010/06/25
            pProcInfo.cfg.usTimeoutInMs_player_a = Consts.DEFAULT_usTimeoutInMs_player_a;
            pProcInfo.cfg.usTimeoutInMs_player_v = Consts.DEFAULT_usTimeoutInMs_player_v;

            pProcInfo.cfg.loopCtrl_timer_dlgTalk_chkRecvdTasks = Consts.DEFAULT_loopCtrl_timer_dlgTalk_chkRecvdTasks;
            pProcInfo.cfg.loopCtrl_chkPlayers = Consts.DEFAULT_loopCtrl_chkPlayers;


            //  
            pProcInfo.cfg.notify_avCall_bNoSound = true;
            pProcInfo.cfg.uiTimeoutInMs_waitForProcessingOf_avTask = 15000;
            pProcInfo.cfg.uiTimeoutInMs_waitForProcessingOf_otherTask = 60000;
            pProcInfo.cfg.uiTimeoutInMs_waitIfNoData = 10000;
            //
            pProcInfo.cfg.usMaxN_lostVideo = 20;

            //
            GuiShare.pf_customSystem();

            //


            //
#if DEBUG

#if true
            if (false)
            {
                pProcInfo.cfg.uiTimeoutInMs_waitForProcessingOf_avTask = 600000;
                traceLog("For test: timeout_waitForProc_avTask set to 600s");
                pProcInfo.cfg.usMaxN_lostVideo = 1000;
            }
#endif

            //
#if true
            pProcInfo.cfg.debugStatusInfo.ucb_notPrint_my_inStream = true;
#endif



            //pProcInfo.cfg.debugStatusInfo.ucbShowPktInputInfo = true;
            //pProcInfo.cfg.debugStatusInfo.ucb__Dump_h264_enc__ = true;
            //pProcInfo.cfg.debugStatusInfo.ucb__Dump_h264_dec__ = true;

#if false
			pProcInfo.cfg.debugStatusInfo.ucb__Dump_pcm__ = true;
			traceLog("for test: dump_pcm set to true");
#endif
#if false
			pProcInfo.cfg.debugStatusInfo.ucb__Dump_mp3_enc__ = true;
			traceLog("for test: dump_mp3_enc set to true");
#endif

#if false
			pProcInfo.cfg.debugStatusInfo.ucb__Dump_mp3_dec__ = true;
			traceLog("for test: dump_mp3_dec set to true");
#endif

#if false
			pProcInfo.cfg.debugStatusInfo.ucb__Dump_deced_pcm__ = true;
			traceLog("for test: dump_deced_pcm set to true");
#endif

#if false
			pProcInfo.cfg.ucb_USE_Fragment_dlgTalk_av = true;
			traceLog("for test: USE_Fragment_dlgTalk_av set to true");
#endif

#if false
			pProcInfo.cfg.ucb_USE_specialDbg = true;
			traceLog("for test: USE_specialDbg set to true");
#endif
            //pProcInfo.cfg.mSpecialDbg.bNo_videoCap = true;
            //pProcInfo.cfg.mSpecialDbg.bNo_audioRecord = true;
            //
            //pProcInfo.cfg.mSpecialDbg.bNo_audioPlayer = true;
            //
            //pProcInfo.cfg.mSpecialDbg.bNo_encV = true;
            //
            //pProcInfo.cfg.mSpecialDbg.bDoGC_mainWnd_OnTimer = true;
            //
            //pProcInfo.cfg.mSpecialDbg.bTrace_tmpHandler_findOrgMsgAndProc_is = true;


#if false
			pProcInfo.cfg.debugStatusInfo.ucbShowPlayAudioStatus = true;
			traceLog("for test: ucbShowPlayAudioStatus set to true");
#endif

            //
#if true
            pProcInfo.cfg.debugStatusInfo.ucb_chkInputPts = true;
            traceLog("for test: ucb_chkInputPts set to true");
#endif

            //
            //pProcInfo.cfg.b_useAudioQ2_toDecA = true;
            //pProcInfo.cfg.b_useQ2_toPlayA = true;

            //
            //pProcInfo.cfg.b_useVideoQ2_toDecV = true;


            //
            //pProcInfo.cfg.debugStatusInfo.bPrint_encObj_mediaCodec = true;

            //
            pProcInfo.cfg.debugStatusInfo.bNoRemove_err_missing = true;

            //
            pProcInfo.cfg.debugStatusInfo.bUse_test_noLocalAv = false;

#endif





            //
            iErr = 0;
        errLabel:

            if (iErr != 0) exitVar_isGui(pQyMc, ref pSci, p2);

            //
            str = string.Format("initVar_isGui leaves, iErr {0}", iErr);
            showInfo_open(0, null, str);

            //
            return iErr;
        }


        public static int init_afterLogon_isGui(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2)
        {
            int iErr = -1;
            //QY_MC* pQyMc = (QY_MC*)p0;
            //QY_SERVICEGUI_INFO* pSci = (QY_SERVICEGUI_INFO*)p1;
            //  p2

            /*
			 * 
	if (pQyMc.iServiceId != CONST_qyServiceId_is) return 0;
	if (pQyMc.iAppType != CONST_qyAppType_client) return 0;

	MC_VAR_isCli* pProcInfo = (MC_VAR_isCli*)pSci->pVar;


#if  10  //  2013/08/23
	 HWND		hMainWnd  =  pQyMc.gui.hMainWnd;
	 
	 QNM_cusRes_moduleMem  *  pResMem  =  getCurCusResModuleMem(  &pQyMc.cusRes  );
	 if  (  !pResMem  )  goto  errLabel;

	 HMENU		hMainMenu  =  ::GetMenu(  hMainWnd  );
	 if  (  hMainMenu  )  {
		 QY_DMITEM	*	pItem;
		 int  iResId  =  CONST_resId_mainMenu;
		 //if  (  qyGetAppAvLevel_qyMc(  )  ==  CONST_qyAppAvLevel_mini  )  iResId  =  CONST_resId_mainMenu_mini;
		 //  if  (  !(  pItem  =  (  QY_DMITEM  *  )qyGetDesByType(  pResMem->pCusMenuTable,  CONST_resId_mainMenu  )  )  )  goto  errLabel;
		 if  (  !(  pItem  =  (  QY_DMITEM  *  )qyGetDesByType(  pResMem->pCusMenuTable,  iResId  )  )  )  goto  errLabel;
		 if  (  makeDynaMenu(  &pQyMc.cusRes,  hMainMenu,  pItem,  getMenuStatus_frame,  pQyMc,  0  )  )  goto  errLabel;
	 }

	 //
#if  0  //  2017/06/21
	 if  (  qyGetSubSystemId_isCli(  )  ==  CONST_subSystemId_basic  )  {

		 //
		 TCHAR  tBuf[128];
		 GetWindowText(  hMainWnd,  tBuf,  mycountof(  tBuf  )  );
		 //
		 HMENU  hMenu  =  GetSubMenu(  hMainMenu,  1  );
		 if  (  hMenu  )  {
			 RemoveMenu(  hMainMenu,  2,  MF_BYPOSITION  );

		 }
		 //
		 //RemoveMenu(  hMainMenu,  8,  MF_BYPOSITION  );

	 }
#endif

#endif



	//  2009/12/21	 
# ifdef  __DEBUG__
	if (!pQyMc.ucbUseNamedShm)
	{
		traceLogA("Not finished: unnamed shared memory shall be used");
		goto errLabel;
	}
#endif
				pProcInfo.cmdsShm.usMaxCnt_qmCmds = CONST_maxCnt_qmCmds;
	if (pProcInfo.cmdsShm.usMaxCnt_qmCmds < pProcInfo.cfg.usMaxCnt_pSharedObjs)
	{
		traceLogA(("initVar_isGui failed: usMaxCnt_qmCmds %d < usMaxCnt_pSharedObjs %d"), pProcInfo.cmdsShm.usMaxCnt_qmCmds, pProcInfo.cfg.usMaxCnt_pSharedObjs);
		qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("initVar_isGui failed: usMaxCnt_qmCmds %d < usMaxCnt_pSharedObjs %d"), pProcInfo.cmdsShm.usMaxCnt_qmCmds, pProcInfo.cfg.usMaxCnt_pSharedObjs);
		goto errLabel;
	}
	pProcInfo.cmdsShm.uiBufSize_qmShm = sizeof(QM_SHM_CMDS);
	if (!pProcInfo.cmdsShm.uiBufSize_qmShm) goto errLabel;
	pProcInfo.cmdsShm.hMap = CreateFileMapping(
				INVALID_HANDLE_VALUE,                       // use paging file
				null,                                       // default security 
				PAGE_READWRITE,                         // read/write access
				0,                                          // max. object size 
				pProcInfo.cmdsShm.uiBufSize_qmShm,             // buffer size  
				CQyString(pQyMc.appParams.appObjPrefix) + CONST_shmName_qm_cmd);                       // name of mapping object 
	if (pProcInfo.cmdsShm.hMap == null)
	{
		traceLogA("Could not create file mapping object (%d).\n", GetLastError());
		goto errLabel;
	}
	pProcInfo.cmdsShm.pBuf = (char*)MapViewOfFile(pProcInfo.cmdsShm.hMap,   // handle to map object
											   FILE_MAP_ALL_ACCESS,         // read/write permission
											   0,
											   0,
											   pProcInfo.cmdsShm.uiBufSize_qmShm);
	if (pProcInfo.cmdsShm.pBuf == null)
	{
		traceLogA("Could not map view of file (%d).\n", GetLastError());
		goto errLabel;
	}
	memset(pProcInfo.cmdsShm.pBuf, 0, pProcInfo.cmdsShm.uiBufSize_qmShm);
# ifdef  __DEBUG__
	QM_SHM_CMDS* pShm = (QM_SHM_CMDS*)pProcInfo.cmdsShm.pBuf;
	lstrcpyn(pShm->hint, _T("Hello"), mycountof(pShm->hint));
#endif


	QM_SHM_CMDS* pShmCmds = (QM_SHM_CMDS*)pProcInfo.cmdsShm.pBuf;
	if (pShmCmds)
	{
		pShmCmds->hWnd_main = pQyMc.gui.hMainWnd;
	}

	//	 2013/12/24
	if (pQyMc.usAppAvLevel_daemon != pQyMc.usAppAvLevel_default    )
	{
		//
		//  2015/06/07. hd以上(含hd)支持hwAccl,和sd不完全兼容
		//  
		if ((pQyMc.usAppAvLevel_daemon < CONST_qyAppAvLevel_hd && pQyMc.usAppAvLevel_default >= CONST_qyAppAvLevel_hd)
			|| (pQyMc.usAppAvLevel_daemon >= CONST_qyAppAvLevel_hd && pQyMc.usAppAvLevel_default < CONST_qyAppAvLevel_hd))
		{
			//  2017/07/11
			//  qyMessageBox(  pQyMc.gui.hMainWnd,  getResStr(  0,  &pQyMc.cusRes,  CONST_resId_versionErr  ),  _T(  "www.qycx.com"  ),  MB_OK,  10,  null  );
			//showNotification(  0,  0,  0,  0,  0,  0,  getResStr(  0,  &pQyMc.cusRes,  CONST_resId_versionErr  )  );
			showInfo_open(0, _T("init_afterLogon"), getResStr(0, &pQyMc.cusRes, CONST_resId_versionErr));
		}
	}
	//  2014/07/03
#if  0
	 if  (  qyGetAppAvLevel_qyMc(  )  >=  CONST_qyAppAvLevel_fullHd  )  {
		 if  (  !bAppX64(  )  )  {
			 qyMessageBox(  pQyMc.gui.hMainWnd,  _T(  "high版客户端必须在64位系统上运行"  ),  _T(  "www.qycx.com"  ),  MB_OK,  10,  null  );
			 goto  errLabel;
		 }
	 }
#endif

	//  2013/03/10
	SUBTITLES_cfg tmp_subtitlesCfg;
	//
#if  0
	 getSubtitlesCfg(  CONST_capType_av,  0,  0,  &tmp_subtitlesCfg  );
	 //
	 subtitlesCfg_modify(  &tmp_subtitlesCfg,  GetTickCount(  ),  FALSE,  &pProcInfo.subtitles.localVideoInConf,  null  );
#endif
	//
	getSubtitlesCfg(CONST_capType_av, CONST_subCapType_webcam, 0, &tmp_subtitlesCfg);
	subtitlesCfg_modify(&tmp_subtitlesCfg, GetTickCount(), FALSE, &pProcInfo.subtitles.webcam, null);
	//  2013/12/10

			*/

            //
            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public static unsafe int closeOtherPlayers(CCtxQmc pProcInfo)
        {
            int iErr = -1;

            if (null == pProcInfo) return -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            int i;

            //
            qyFuncs.traceLogA(("closeOtherPlayers enters."));
            qyFuncs.showInfo_open(0, "", ("closeOtherPlayers enters"));

            //
            if (pProcInfo.av.pPlayers != null)
            {

                for (i = 0; i < pProcInfo.av.usCnt_players; i++)
                {
                    QY_PLAYER pPlayer = pProcInfo.av.pPlayers[i];
                    PLAY_AUDIO_procInfo pPlayAudio = pProcInfo.av.pPlayers[i].audio;
                    PLAY_VIDEO_procInfo pPlayVideo = pProcInfo.av.pPlayers[i].video;
                    if (0 != pPlayAudio.uiPlayType)
                    {
                        pPlayAudio.bQuit = true;
                        //if (pPlayAudio->q2.hEvents[0]) SetEvent(pPlayAudio->q2.hEvents[0]);
                        //
                        /*				
                        if (pQyMc->uiAPlayType == CONST_aPlayType_wave)
                        {
                            PostThreadMessage(pPlayAudio->dwThreadId, CONST_qyWm_postComm, CONST_qyWmParam_endThread, 0);
                        }
						*/
                    }
                    if (0 != pPlayVideo.uiPlayType)
                    {
                        pPlayVideo.bQuit = true;
                        //if (pPlayVideo->q2.hEvents[0]) SetEvent(pPlayVideo->q2.hEvents[0]);
                    }
                }
                for (i = 0; i < pProcInfo.av.usCnt_players; i++)
                {
                    QY_PLAYER pPlayer = pProcInfo.av.pPlayers[i];
                    PLAY_AUDIO_procInfo pPlayAudio = pProcInfo.av.pPlayers[i].audio;
                    PLAY_VIDEO_procInfo pPlayVideo = pProcInfo.av.pPlayers[i].video;

                    //
#if false
			  TCHAR  tBuf[128];
			  _sntprintf(tBuf, mycountof(tBuf), _T("closeOtherPlayes: audio.playType %d, video.playType %d"), pPlayAudio->uiPlayType, pPlayVideo->uiPlayType);
			  showInfo_open(0, 0, tBuf);
			  if (pPlayAudio->uiPlayType  ) {
				  //MessageBox(null, _T("hh"), _T("mm"), MB_OK);
			  }
#endif

                    //
                    if (0 != pPlayAudio.uiPlayType)
                    {
                        switch (pPlayAudio.uiPlayType)
                        {
                            /*
                            case CONST_aPlayType_dSound:
                                //myWaitForQThread_infinite(  (  QY_qThreadProcInfo_common  *  )pPlayAudio,  _T(  "closeOtherPlayers"  )  );
                                stopPlayAudio_dSound(pPlayer);
                                break;
                            case CONST_aPlayType_webRtc:
                                //
                                showInfo_open(0, 0, _T("closeOtherPlayers: before stopPlayAudio_webRtc"));

                                //
                                stopPlayAudio_webRtc(pPlayer);
                                break;
							*/
                            case Consts.CONST_aPlayType_android:
                                GuiShare.pf_stopPlayAudio_android(pPlayer);
                                break;
                            default:
                                break;
                        }
                        pPlayAudio.uiPlayType = 0;
                    }

                    //
                    if (0 != pPlayVideo.uiPlayType)
                    {
                        switch (pPlayVideo.uiPlayType)
                        {
                            case Consts.CONST_vPlayType_v:
                                //
# if  __DEBUG__
                                //traceLog((TCHAR*)  _T(  "before waitForVideoThread, %d"  ),  j  );
#endif

                                //
                                myWaitForQThread_infinite(pPlayVideo, _T("closeOtherPlayers"));
                                //
# if  __DEBUG__						  
                                traceLog((TCHAR*)_T("after waitForVideoThread"));
#endif
                                //
                                break;
                            default:
                                break;
                        }
                        //
                        pPlayVideo.uiPlayType = 0;
                    }
                    //
                }

            }

            qyFuncs.traceLogA(("closeOtherPlayers leaves."));
            showInfo_open(0, "", _T("closeOtherPlayers leaves"));


            return 0;
        }




        public static unsafe int exitVar_isGui(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2)
        {
            //QY_MC* pQyMc = (QY_MC*)p0;
            //QY_SERVICEGUI_INFO* pSci = (QY_SERVICEGUI_INFO*)p1;
            //  p2
            int i;



            if (pQyMc.iServiceId != Consts.CONST_qyServiceId_is) return 0;

            //
            showInfo_open(0, null, "exitVar_isGui enters.");

            /*
	//  2010/10/01
	if (pQyMc.iAppType != CONST_qyAppType_client)
	{
		exitVar_isGui_mgr(p0, p1, p2);
		return 0;
	}*/


            //
            CCtxQmc pProcInfo = (CCtxQmc)pSci.pVar;

            //
            closeOtherPlayers(pProcInfo);


            //  2010/08/05
            pProcInfo.processQ_media.emptyQ();
            //pProcInfo.processQ_robot.emptyQ(  );

            /*
			
	//  2009/12/21
	if (pProcInfo.cmdsShm.pBuf)
	{
		UnmapViewOfFile(pProcInfo.cmdsShm.pBuf);
		pProcInfo.cmdsShm.pBuf = null;
	}
	if (pProcInfo.cmdsShm.hMap)
	{
		CloseHandle(pProcInfo.cmdsShm.hMap); pProcInfo.cmdsShm.hMap = null;
	}

	//  2008/10/04
	if (bNoSharedObjExists(pProcInfo))
	{
		MACRO_safeFree(pProcInfo.pCapProcInfoAudios);
		MACRO_safeFree(pProcInfo.pCapProcInfoBmps);
		MACRO_safeFree(pProcInfo.pSharedObjSyncs);
		//
	}
	else
	{
		traceLogA("exitVar_isGui error: shared obj already exists.");
	}
			*/

            //  2014/09/24
            freeTaskInfos(pProcInfo);
            //
            //  2014/09/24
            //if (bNoTaskInfoExists(pProcInfo))
            {
                //MACRO_safeFree(pProcInfo.pQmcTaskInfos);
                pProcInfo.pQmcTaskInfos = null;
            }

            //
            if (pProcInfo.av.texes != null)
            {
                if (pProcInfo.av.texes.taskAvTexes != null)
                {
                    pProcInfo.av.texes.taskAvTexes.tti_peers = null;
                    pProcInfo.av.texes.taskAvTexes = null;
                }
                pProcInfo.av.texes = null;
            }



            /*
            //  2015/11/15
            tryToFreeQmcObjVarInfos(pProcInfo);

            //  2015/11/10
            if (bNoObjVarInfoExists(pProcInfo))
            {
                MACRO_safeFree(pProcInfo.pQmcObjVarInfos);
            }
                    */



            /*
	//  2012/05/07
	if (pProcInfo.gps.pPlayers)
	{
		for (i = 0; i < pProcInfo.gps.usCnt_players; i++)
		{
			if (pProcInfo.gps.pPlayers[i].bQInited) exitQyQ2(&pProcInfo.gps.pPlayers[i].q2);
		}
		free(pProcInfo.gps.pPlayers);
	}
			*/

            //

            //  ÊÍ·ÅÒôÆµ³Ø	 
            if (pProcInfo.av.pPlayers != null)
            {  //  ÕâÀï»¹Ã»ÅªºÃ£¬ÔõÃ´À´²»Ò»¿ªÊ¼Æô¶¯Ïß³ÌÄØ¡£¶ø´ï¡£¡£¡£¡£¡£

                for (i = 0; i < pProcInfo.av.usCnt_players; i++)
                {
                    if (!bQThreadExists(ref pProcInfo.av.pPlayers[i].audio))
                    {
                        if (pProcInfo.av.pPlayers[i].audio.bQInited)
                        {
                            exitQyQ2(pProcInfo.av.pPlayers[i].audio.q2);
                            pProcInfo.av.pPlayers[i].audio.q2 = null;
                        }
                        pProcInfo.av.pPlayers[i].audio.qBuf2.exitQyQ2();
                        pProcInfo.av.pPlayers[i].audio.qBuf2 = null;
                    }
                    else
                    {
                    }
                    if (pProcInfo.av.pPlayers[i].video.bQInited)
                    {
                        exitQyQ2(pProcInfo.av.pPlayers[i].video.q2);
                        pProcInfo.av.pPlayers[i].video.q2 = null;
                    }
                    if (pProcInfo.av.pPlayers[i].video.ucbDisplayQInited)
                    {
                        exitQyQ2(pProcInfo.av.pPlayers[i].video.displayQ2);
                        pProcInfo.av.pPlayers[i].video.displayQ2 = null;
                    }

                    //
                    pProcInfo.av.pPlayers[i] = null;
                }

                //free(pProcInfo.av.pPlayers);
                pProcInfo.av.pPlayers = null;
            }

            if (!bQThreadExists(ref pProcInfo.av.localAv.player.audio))
            {
                if (pProcInfo.av.localAv.player.audio.bQInited)
                {
                    exitQyQ2(pProcInfo.av.localAv.player.audio.q2);
                    pProcInfo.av.localAv.player.audio.q2 = null;
                }
                pProcInfo.av.localAv.player.audio.qBuf2.exitQyQ2();
                pProcInfo.av.localAv.player.audio.qBuf2 = null;
            }
            else
            {
            }
            if (pProcInfo.av.localAv.player.video.bQInited)
            {
                exitQyQ2(pProcInfo.av.localAv.player.video.q2);
                pProcInfo.av.localAv.player.video.q2 = null;
            }
            if (pProcInfo.av.localAv.player.video.ucbDisplayQInited)
            {
                exitQyQ2(pProcInfo.av.localAv.player.video.displayQ2);  //  2009/07/25
                pProcInfo.av.localAv.player.video.displayQ2 = null;
            }
            //
            pProcInfo.av.localAv.player = null;

            //for  (  int  i  =  0;  i  <  mycountof(  pProcInfo.misCnts  );  i  ++  )  
            {
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;   //  getMisCntByIndex(0, pProcInfo, i);//&pProcInfo.misCnts[i];
                if (pMisCnt != null)
                {
                    //
                    //if (pMisCnt.cfg.dynCfg.cnt.usCntPort!=0)
                    {

                        /*

                        //  ¹Ø±Õ¹ÜÀí¶ÓÁÐ
                        if (pMisCnt.ucbQmObjQInited)
                        {       //  2006/12/07
                            exitQmObjQ(0, (QM_OBJQ*)pMisCnt.pObjQ); pMisCnt.ucbQmObjQInited = FALSE;
                        }
                        if (pMisCnt.pObjQ)
                        {   //  2007/04/01
                            free(pMisCnt.pObjQ); pMisCnt.pObjQ = null;
                        }
                        */
#if false
						if (pProcInfo.hMutex_sync_mTableItems_imObj != null)
                        {
							pProcInfo.hMutex_sync_mTableItems_imObj.Close();
							pProcInfo.hMutex_sync_mTableItems_imObj = null;
                        }
#endif
                        //
                        if (pProcInfo.hEvt_notify_thread_isCli != null)
                        {
                            pProcInfo.hEvt_notify_thread_isCli.Close();
                            pProcInfo.hEvt_notify_thread_isCli = null;
                        }

                        if (pProcInfo.hMutex_syncQdcUnit != null)
                        {
                            pProcInfo.hMutex_syncQdcUnit.Close();
                            pProcInfo.hMutex_syncQdcUnit = null;
                        }
                        //
                        if (pMisCnt.hMutex_syncTask != null)
                        {
                            pMisCnt.hMutex_syncTask.Close();
                            pMisCnt.hMutex_syncTask = null;
                        }
                        if (pMisCnt.hMutex_syncStartAv != null)
                        {
                            pMisCnt.hMutex_syncStartAv.Close();
                            pMisCnt.hMutex_syncStartAv = null;
                        }
                        if (pMisCnt.hMutex_syncSendAv != null)
                        {
                            pMisCnt.hMutex_syncSendAv.Close();
                            pMisCnt.hMutex_syncSendAv = null;
                        }
                        if (pMisCnt.hMutex_syncActiveMem != null)
                        {
                            pMisCnt.hMutex_syncActiveMem.Close();
                            pMisCnt.hMutex_syncActiveMem = null;
                        }
                        if (pMisCnt.hMutex_syncIndex_dec != null)
                        {
                            pMisCnt.hMutex_syncIndex_dec.Close();
                            pMisCnt.hMutex_syncIndex_dec = null;
                        }
                        if (pMisCnt.hMutex_syncShareEglContext != null)
                        {
                            pMisCnt.hMutex_syncShareEglContext.Close();
                            pMisCnt.hMutex_syncShareEglContext = null;
                        }


                        //  ¹Ø±Õ´¥·¢ÐÅºÅµÆ
                        int j;
                        for (j = 0; j < pMisCnt.channels.Length; j++)
                        {
                            MIS_CHANNEL pChannel = pMisCnt.channels[j];
                            if (pChannel == null) continue;
                            //  
                            //
                            exitQyQ2(pChannel.outputQ2); pChannel.outputQ2 = null;
                            exitQyQ2(pChannel.toSendQ2); pChannel.toSendQ2 = null;
                            //
                            exitGenericQ(pChannel.inCacheQ); pChannel.inCacheQ = null;

                            //
                            pMisCnt.channels[j] = null;
                        }
                        pMisCnt.channels = null;

                        //
                        exitGenericQ(pMisCnt.recentFriendQ); pMisCnt.recentFriendQ = null;

                        //exitGenericQ(pMisCnt.talkingFriendQ); pMisCnt.talkingFriendQ = null;
                        pMisCnt.talkingFriendQ.Clear();
                        pMisCnt.talkingFriendQ = null;

                        //
                        exitGenericQ(pMisCnt.imObj_intQ); pMisCnt.imObj_intQ = null;

                        //
                    }

                }

                //  2012/05/08
            }
            //  2015/07/02
            //MACRO_safeFree(pProcInfo.pMisCnts);
            pProcInfo.pMisCnt = null;



            //
            exitGenericQ(pProcInfo.imMsgLogQ);        //  2010/07/17

            //
            pProcInfo.processQ_media.exitQ();
            //pProcInfo.processQ_robot.exitQ(  );

            //
            exitGenericQ(pProcInfo.mgrQ);
            exitGenericQ(pProcInfo.robotQ);           //  2007/07/04
            exitGenericQ(pProcInfo.displayQ);
            exitGenericQ(pProcInfo.workQ);
            exitGenericQ(pProcInfo.schedulerQ);
            //  exitGenericQ(  &pProcInfo.mediaQ  );
            //  exitGenericQ(  &pProcInfo.realTimeMediaQ  );
            exitQyQ2(pProcInfo.mediaQ2);
            exitQyQ2(pProcInfo.realTimeMediaQ2);
            exitQyQ2(pProcInfo.rtOpQ2);   //  2016/12/26

            //  2010/04/25


            //  2010/09/16
            //

            //  2017/06/18
            exit_webRtc();


            //  2016/09/08


            //  2016/06/03
            //
            showInfo_open(0, null, "exitVar_isGui leaves.");


            return 0;
        }


        //


    }


}

