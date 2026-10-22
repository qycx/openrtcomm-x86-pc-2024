using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;
using qy;
using Xamarin.Essentials;

partial class Consts
{
    public const int CONST_misMsgType_null = 0;
    public const int CONST_misMsgType_base = CONST_qyWmParam_misMsg_base;
    public const int CONST_misMsgType_resp_onlySock = (CONST_misMsgType_base + 1);  //  
    public const int CONST_misMsgType_quit = (CONST_misMsgType_base + 2);
    public const int CONST_misMsgType_closeSession = (CONST_misMsgType_base + 3);
    public const int CONST_misMsgType_input = (CONST_misMsgType_base + 4);
    public const int CONST_misMsgType_send = (CONST_misMsgType_base + 5);   //  
    public const int CONST_misMsgType_processSelf = (CONST_misMsgType_base + 6);    //  
    public const int CONST_misMsgType_processBt = (CONST_misMsgType_base + 7);  //  2011/04/18
                                                                                //public const int		CONST_misMsgType_processSelf_sendFinished			(  CONST_misMsgType_base  +  8	)	//  2015/08/18
    public const int CONST_misMsgType_processSelf_new = (CONST_misMsgType_base + 9);    //  2015/08/21
    public const int CONST_misMsgType_inputArrive = (CONST_misMsgType_base + 10);   //  2015/08/24
                                                                                    //public const int		CONST_misMsgType_resp1								(  CONST_misMsgType_base  +  11	 )	//  2015/08/25. 暂时用来替换resp,以帮助发现未加密的resp
    public const int CONST_misMsgType_resp2 = (CONST_misMsgType_base + 12); //  2015/08/25. resp的处理标识已经是网络字节流顺序，resp2表明不是网络顺序


    //  
    //  public const int		CONST_misMsgType_resumeToSend					(  CONST_misMsgType_base  +  16  )	//	
    //  
    public const int CONST_misMsgType_req = (CONST_misMsgType_base + 30);   //  messenger·
    public const int CONST_misMsgType_resp = (CONST_misMsgType_base + 31);  //  iocp·
    public const int CONST_misMsgType_talk = (CONST_misMsgType_base + 32);  //  
    public const int CONST_misMsgType_task = (CONST_misMsgType_base + 33);  //  
                                                                            //
                                                                            //
    public const int CONST_misMsgType_isSendOk = (CONST_misMsgType_base + 50);  //  iocp iocp tranNo. messenger outputQ iocp,tranNo starttime
    public const int CONST_misMsgType_sendOk = (CONST_misMsgType_base + 51);    //  MGR tranNo. messenger iocp, tranNoºÍstarttime
                                                                                //  mgr·
                                                                                //
    public const int CONST_misMsgType_sendFinished = (CONST_misMsgType_base + 52);  //  mgr
                                                                                    //
                                                                                    //  public const int		CONST_misMsgType_closeChannel					(  CONST_misMsgType_base  +  53  )	//  2007/12/20, QMemMessenge
    public const int CONST_misMsgType_closeChannelByCliIndex = (CONST_misMsgType_base + 53);    //  2007/12/20, 
    public const int CONST_misMsgType_chkChannelByType_mgr = (CONST_misMsgType_base + 54);  //  2015/08/26, 
                                                                                            //
    public const int CONST_misMsgType_applyForWork = (CONST_misMsgType_base + 55);  //  
                                                                                    //////
    public const int CONST_misMsgType_taskStatus = (CONST_misMsgType_base + 60);    //  
    public const int CONST_misMsgType_imMsgRcd = (CONST_misMsgType_base + 61);  //  
    public const int CONST_misMsgType_event = (CONST_misMsgType_base + 62); //  

    //  ÒÔÉÏÊÇ°üÀàÐÍ
    public const int CONST_misMsgType_talkingFriend_qmc = (CONST_misMsgType_base + 70); //	talkingFriendQ
    public const int CONST_misMsgType_recentFriend_qmc = (CONST_misMsgType_base + 71);  //	recentFriendQ
    public const int CONST_misMsgType_displayedElem_qmc = (CONST_misMsgType_base + 72); //  
    public const int CONST_misMsgType_refreshContactsStatus_qmc = (CONST_misMsgType_base + 73); //  messenger display
    public const int CONST_misMsgType_refreshMeStatus_qmc = (CONST_misMsgType_base + 74);   //  diplay
    public const int CONST_misMsgType_applyForPlayer_qmc = (CONST_misMsgType_base + 75);    //  
    public const int CONST_misMsgType_applyForChkChannels_qmc = (CONST_misMsgType_base + 76);   //  
    public const int CONST_misMsgType_applyForRemovingInvalidTasks_qmc = (CONST_misMsgType_base + 77);  //  2009/09/10
    public const int CONST_misMsgType_applyForTalkerShadow_qmc = (CONST_misMsgType_base + 78);  //  2012/04/23. 由墙发起,请求dlgTalk生成一个影子窗口

    //
    public const int CONST_misMsgType_procVideo_qmc = (CONST_misMsgType_base + 80); //  2009/03/24
    public const int CONST_misMsgType_notifyTaskEnd_qmc = (CONST_misMsgType_base + 81); //  2009/04/28
    public const int CONST_misMsgType_procGps_qmc = (CONST_misMsgType_base + 82);   //  2012/04/20
                                                                                    //
    public const int CONST_misMsgType_getDaemonStatus = (CONST_misMsgType_base + 85);   //  2011/01/12
                                                                                        //
    public const int CONST_misMsgType_outputTask = (CONST_misMsgType_base + 90);    //  robot
    public const int CONST_misMsgType_outputTask_toMix = (CONST_misMsgType_base + 91);  //  2011/01/23


    //  2008/05/26
    public const int CONST_misMsgType_grp = (CONST_misMsgType_base + 200);  //  ×éºÏ°ü¡£×éºÏ°üÓÐÊ±ºÜ´ó.²»Òª½«MIS_MSGUºÍ×éºÏ°ü»ìÆðÀ´¡£·ñÔò£¬Ó°ÏìÍ¨³£µÄMIS_MSGUµÄÄÚ´æ·ÖÅä¡£

    public const int CONST_maxContactUpdateInfos_grp = 128;                                         //  2014/02/06
    public const int CONST_maxContactUpdateInfos_grpMem = 2048;                                     //  2014/02/06
    public const int CONST_maxContactUpdateInfos_contact = 128 * 3;                                 //  2014/02/09

    public const int CONST_maxMessengerNameLen = 255;
    public const int CONST_maxMessengerPasswdLen = 255;

    public const int CONST_maxMisServNameLen = 32;                  //  2010/08/05

    //
    public const int CONST_qyMessengerIdStrLen = 18;                    //  
                                                                        //
    public const Int64 CONST_invalidMessengerId = -1;                   //  2007/07/28 

    public const int CONST_cntof_wRndName_serv = 255 + 1;
    public const int cntof_talkerDesc = 32;
    public const int cntof_displayName = 20;
    public const int CONST_cntof_clientWelcomMsg = 128;
    public const int cntof_hint = 255 + 1;

    //
    public const int CONST_maxMsgrs_sendTo = 32;                    //  

    public const int MAX_mems_taskAv = 512;                 //  
                                                            //  public const int 		MAX_mems_taskAv										32					//  
                                                            //  public const int 		MAX_activeMems_taskAv_xp							4					//  
                                                            //
    public const int MAX_speakers_taskAv = 25;              //  16;                   //  

    //
    public const int DEFAULT_usMaxSpeakers = 9;


    //
    public const int MAX_mosaicMems_taskAv = 2;             //1;
    public const int MAX_resourceMems_taskAv_conf = 4;
    //
    public const int MAX_resourceMems_taskAv_p2p = 2;

    //
    public const int MAX_conf_activeMems = MAX_speakers_taskAv + MAX_mosaicMems_taskAv + MAX_resourceMems_taskAv_conf;
    //
    public const int MAX_conf_requestingMems = 10;                   //  


    //
    public const int MAX_activeMems_from = MAX_conf_activeMems;//20;



    //  public const int 		CONST_maxTaskAvMixerPlayers							32					//  2009/02/16
    //public const int CONST_maxTransferAvDataMems = 32;                  //  2009/02/16
    public const int CONST_maxTransferAvDataMems = 160;     //  32;                  //  2009/02/16


    //  public const int 			DEFAULT_mems_taskAv									128
    public const int DEFAULT_mems_taskAv = 256;             //  2012/02/21

    //public const int 		DEFAULT_usCntLimit_activeMems_from					3					//  
    public const int DEFAULT_usCntLimit_activeMems_from = 4;                    //  2014/12/05

    //
    public const int MAX_usCntLimit_activeMems_from_3d_win32 = 4;



    //
    public const int bufSize_imDataContent = 63 * 1024;
    public const int bufSize_imStreamContent = 63 * 1024;                     //  2008/03/22, 
    public const int bufSize_imLStream = (900 + 32) * 1024;       //  2008/03/22, 这个包应该比别的包略大一点，所以+32


    //
    public const int bufSize_talkData = bufSize_imLStream + 2 * 1024;//pc 2443304;
    public const int bufSize_routeTalkData = 954384;// bufSize_imLStream + 2 * 1024;


    //
    //  public const int 		bufSize_transferFileDataResp				32  *  1024
    //  public const int 		bufSize_transferFileDataResp				64  *  1024						//  2010/12/05
    public const int bufSize_transferFileDataResp = 128 * 1024;                   //  2014/07/13
                                                                                  //
    public const int bufSize_transferVideoData = 900 * 1024;                  //  2012/06/28. mediaSdk压缩出来的一个包就可能600k.cuda压缩出来的一个包可能700多k
    public const int bufSize_transferAudioData = 512 * 1024;                  //  2009/05/20

    //  public const int 		bufSize_myDrawVideoData					8  *  1024  *  1024				//  2009/05/03
    public const int bufSize_myDrawVideoData_2k = 8 * 1024 * 1024;                //  2009/05/03
    public const int bufSize_myDrawVideoData_4k = 36 * 1024 * 1024;           //  2014/06/01. 4096X2304, 3840X2160

    public const int bufSize_myPlayAudioData1 = 48000 * 2 / 10; //100ms //1 * 1024 * 1024;

    //
    public const int cntof_imHtmlContent_wBuf = 16 * 1024;

    //
    public const int CONST_maxContactUpdateInfos_025909 = 10;                                           // 

    //
    public const int CONST_sizeof_imGrpInfo = 248;
    //IM_GRP_INFO mems[Consts.CONST_maxContactUpdateInfos_grp];          //  ·Ç½áÊø°ü	
    public const int bufSize_imGrpInfo_mems = CONST_sizeof_imGrpInfo * Consts.CONST_maxContactUpdateInfos_grp;
    //
    public const int CONST_sizeof_imGrpMem = 128;
    //IM_GRP_MEM mems[CONST_maxContactUpdateInfos_grpMem];            //  ·Ç½áÊø°ü	
    public const int bufSize_imGrpMem_mems = CONST_sizeof_imGrpMem * Consts.CONST_maxContactUpdateInfos_grpMem;

    //
    public const int CONST_sizeof_retrieveContaceMem = 976;
    //RETRIEVE_contact_mem mems[CONST_maxContactUpdateInfos_contact];         //  ·Ç½áÊø°ü	
    public const int bufSize_retrieveContaceMem_mems = CONST_sizeof_retrieveContaceMem * CONST_maxContactUpdateInfos_contact;

    //
    public const int CONST_maxNameLen = 64;

    //
    public const int MAX_imObjRule_reqMems = 10;
    //
    public const int DEFAULT_maxImObjRule_reqMems = 10;                 //  当req.usMaxMemsPerSnd为0时，取此值. 不能修改. 2011/11/13


    //
    public const int CONST_sizeof_refreshImObjRuleReqMem = 280;
    //REFRESH_imObjRule_reqMem mems[MAX_imObjRule_reqMems];				 
    public const int bufSize_REFRESH_imObjRule_reqMem_mems = CONST_sizeof_refreshImObjRuleReqMem * MAX_imObjRule_reqMems;

    public const int CONST_sizeof_TRANSFER_VIDEO_dataMemHead = 56;
    //TRANSFER_VIDEO_dataMemHead mems[Consts.CONST_maxTransferAvDataMems];
    public const int bufSize_TRANSFER_VIDEO_dataMemHead_mems = CONST_sizeof_TRANSFER_VIDEO_dataMemHead * CONST_maxTransferAvDataMems;

    public const int CONST_sizeof_TRANSFER_AUDIO_dataMemHead = 48;
    //TRANSFER_AUDIO_dataMemHead mems[CONST_maxTransferAvDataMems];
    public const int bufSize_TRANSFER_AUDIO_dataMemHead_mems = CONST_sizeof_TRANSFER_AUDIO_dataMemHead * CONST_maxTransferAvDataMems;

    //
    public const int sizeof_videoConferenceMem = 4720;
    public const int bufSize_videoConferenceMem_mems = Consts.sizeof_videoConferenceMem * (MAX_conf_activeMems);
    public const int cntof_videoConferenceMems = MAX_conf_activeMems;

    //
    public const int sizeof_CONF_requesting_mem = 80;
    public const int bufSize_CONF_requesting_mems = Consts.sizeof_CONF_requesting_mem * MAX_conf_requestingMems;


    //
    public const int MAX_ipcInfos = 128;
    public const int sizeof_ipcInfo = 320;
    public const int bufSize_ipcInfos = Consts.sizeof_ipcInfo * MAX_ipcInfos;


    public const int cntof_name = 128;
    public const int cntof_messengerPasswd = 255;
    public const int cntof_ip = 16;

    //
    //
    public const int CONST_pkts_playVideo = 20;                                     //  处理线程中一次性读取的最大包数，要保证下面的具体线程中的取值小于此数. 2011/12/06
                                                                                    //
    public const int CONST_pkts_vpp = 20;
    public const int CONST_pkts_encVideo = 20;
    public const int CONST_pkts_toShareVideo_post = 20;
    //
    public const int CONST_pMsgs_dispatchVideo = 20;
    //
    public const int CONST_pMsgs_preTransformVideo = 10;
    public const int CONST_pkts_preTransformVideo = (CONST_maxTransferAvDataMems + CONST_pkts_playVideo);
    public const int CONST_pkts_out_preTransformVideo = 20;

    public const int CONST_pkts_decVideo = 20;
    public const int CONST_pkts_out_decVideo = 20;

    //
    public const int CONST_pMsgs_transformVideo = 1;
    public const int CONST_pkts_transformVideo = (CONST_maxTransferAvDataMems + CONST_pkts_playVideo);


    //  2015/02/13
    public const int CONST_pkts_decAudio = 20;
    public const int CONST_pkts_encAudio = 20;


    //		
    public const int CONST_sizeof_myDRAW_VIDEO_DATA = 280;
    //sizeof(myDRAW_VIDEO_DATA) * Consts.CONST_pkts_playVideo;
    public const int bufSize_myDRAW_VIDEO_DATA_pkts_playVideo = (CONST_sizeof_myDRAW_VIDEO_DATA * CONST_pkts_playVideo);

    public const int bufSize_myDRAW_VIDEO_DATA_pkts_toShareVideo_post = CONST_sizeof_myDRAW_VIDEO_DATA * CONST_pkts_toShareVideo_post;
    public const int bufSize_myDRAW_VIDEO_DATA_pkts_encVideo = CONST_sizeof_myDRAW_VIDEO_DATA * CONST_pkts_encVideo;
    public const int bufSize_myDRAW_VIDEO_DATA_pkts_vpp = CONST_sizeof_myDRAW_VIDEO_DATA * CONST_pkts_vpp;
    //public const int bufSize_myDRAW_VIDEO_DATA_pkts_preTransformVideo = CONST_sizeof_myDRAW_VIDEO_DATA * CONST_pkts_preTransformVideo;
    //public const int bufSize_myDRAW_VIDEO_DATA_pkts_decVideo = CONST_sizeof_myDRAW_VIDEO_DATA * CONST_pkts_decVideo;

    //
    public const int CONST_sizeof_myPLAY_AUDIO_DATA = 104;
    public const int bufSize_myPLAY_AUDIO_DATA_pkts_encAudio = CONST_sizeof_myPLAY_AUDIO_DATA * CONST_pkts_encAudio;

    public const int CONST_enlargeType_null = 0;
    public const int CONST_enlargeType_img = 1;
    //
    //  2015/08/02
    public const int CONST_procOfflineResSubtype_null = 0;
    public const int CONST_procOfflineResSubtype_getCfgs = 1;
    public const int CONST_procOfflineResSubtype_retrieve = 2;
    public const int CONST_procOfflineResSubtype_upload = 3;
    public const int CONST_procOfflineResSubtype_download = 4;
    public const int CONST_procOfflineResSubtype_del = 5;

    //
    public const int CONST_transferAvInfo_subtype_null = 0;
    public const int CONST_transferAvInfo_subtype_task = 2;
    //
    public const int CONST_transferAvReplyInfo_subtype_task = 3;

    //
    //  2015/03/12
    public const int CONST_avFlg_null = 0x0;
    public const int CONST_avFlg_unresizable = 0x1;

    //
    //  2014/05/31
    public const int CONST_usIndex_screen0 = 1;
    public const int CONST_usIndex_webcam0 = 1;
    public const int CONST_usIndex_webcam1 = 2;
    public const int CONST_usIndex_webcam2 = 3;
    //
    public const int CONST_usIndex_avStream_slave = 2;                                  //  2014/09/28
                                                                                        //
    public const int CONST_usIndex_photomosaic_video = 3;                                   //  2014/11/91
    public const int CONST_usIndex_photomosaic_resource = 4;                                    //  2014/11/01

    //
    public const int CONST_usTaskProcObjType_null = 0;
    public const int CONST_usTaskProcObjType_vStream = 1;

    //
    //
    public const int CONST_imOp_null = 0;
    public const int CONST_imOp_says = 1;       //  
    public const int CONST_imOp_tryToSendFile = 2;      //  
                                                        //
    public const int CONST_imOp_recv_accept = 101;      //  
    public const int CONST_imOp_recv_deny = 102;        //  

    public const int CONST_imOp_recvFile_saveas = 103;
    //
    public const int CONST_imOp_recv_cancel = 104;      //  
    public const int CONST_imOp_send_cancel = 106;      //  
    public const int CONST_imOp_send_permit = 107;      //  
                                                        //
    public const int CONST_imOp_dual_permit = 120;      //  2008/05/29
                                                        //
    public const int CONST_imOp_recv_applyForOrgReq = 123;      //  2009/10/18
    public const int CONST_imOp_recv_applyForChkTaskAlive = 124;        //  2009/10/19
    public const int CONST_imOp_recv_applyForKeyFrame = 125;        //  2014/08/26

    //
    public const int CONST_imOp_requestToSpeak = 130;       //  2009/02/21
    public const int CONST_imOp_stopSpeaking = 131;     //  2009/02/21. 
                                                        //
    public const int CONST_imOp_permitToSpeak = 132;        //  2017/07/07
    public const int CONST_imOp_prohibitSpeaking = 133;     //  2017/07/07. 
                                                            //
    public const int CONST_imOp_requestToControlDesktop_yz = 134;       //  2017/08/26
    public const int CONST_imOp_stopControlDesktop_yz = 135;



    //
    public const int CONST_imOp_requestToSend = 180;        //  2014/10/09
    public const int CONST_imOp_stopSending = 181;        //  2014/10/09

    //
    public const int CONST_imOp_new = 190;      //  2017/07/29
    public const int CONST_imOp_refresh = 191;
    public const int CONST_imOp_del = 192;
    public const int CONST_imOp_modify = 193;       //  2018/11/20
                                                    //


    //
    public const int CONST_imOp_enlargeImg = 200;
    public const int CONST_imOp_enlargeChannel = 201;
    public const int CONST_imOp_setOneBig = 202;

    //
    public const int CONST_imOp_videoTop = 210;
    public const int CONST_imOp_videoBottom = 211;
    public const int CONST_imOp_videoLeft = 212;
    public const int CONST_imOp_videoRight = 213;


    public const int CONST_imOp_loseSel = 214;

    public const int CONST_imOp_phone_enlargeImg = 220;
    public const int CONST_imOp_phone_enlargeChannel = 221;
    public const int CONST_imOp_phone_respCurr = 225;
    public const int CONST_imOp_respChannel = 230;
    //
    public const int CONST_imOp_openFile = 301;     //  
                                                    //
    public const int CONST_imOp_myMax = 10000;  //  
                                                //
    public const int cntof_subject = 128;
    public const int cntof_content = 256;
    public const int cntof_propStr = 32;
    public const int cntof_senderDesc = 64 + 1;
    public const int cntof_receiverDesc = 64 + 1;

    //
    public const int DEFAULT_usTimeoutInMsToSwitch_isClient = 2000;                             //  2008/10/28
    public const int DEFAULT_usTimeoutInMsToSwitch_rt_isClient = 2000;                              //	2009/06/07从100调整为200，//  2008/10/28

    //
    public const int CONST_threadStep_null = 0;
    public const int CONST_threadStep_start = 1;
    public const int CONST_threadStep_toWait = 3;
    public const int CONST_threadStep_toGetMsg = 5;
    public const int CONST_threadStep_msgGot = 9;
    //
    public const int CONST_nStep_enter = 30;                //
    public const int CONST_nStep_leave = 39;
    //
    //  user defined step 100->1000
    //
    public const int CONST_threadStep_msgProced = 1001;
    public const int CONST_threadStep_toExit = 1011;
    public const int CONST_threadStep_end = 1019;

    //
    public const int bufSize_tmpPktEncA = 16000;

    //
    public const int bufSize_tmpPktEncV = 900000;

    //
    public const int cntof_logMsgContent = 256;

    //
    public const int CONST_ptzCmd_null = 0;
    public const int CONST_ptzCmd_up = 1;
    public const int CONST_ptzCmd_down = 2;
    public const int CONST_ptzCmd_left = 3;
    public const int CONST_ptzCmd_right = 4;
    public const int CONST_ptzCmd_topLeft = 5;
    public const int CONST_ptzCmd_topRight = 6;
    public const int CONST_ptzCmd_bottomLeft = 7;
    public const int CONST_ptzCmd_bottomRight = 8;
    public const int CONST_ptzCmd_noop = 9;
    public const int CONST_ptzCmd_focusIn = 10;
    public const int CONST_ptzCmd_focusOut = 11;
    public const int CONST_ptzCmd_irisIn = 12;
    public const int CONST_ptzCmd_irisOut = 13;
    public const int CONST_ptzCmd_zoomIn = 14;
    public const int CONST_ptzCmd_zoomOut = 15;

    //

    public const int IDC_BUTTON_UP = 2280;
    public const int IDC_BUTTON_DOWN = 2281;
    public const int IDC_BUTTON_LEFT = 2282;
    public const int IDC_BUTTON_RIGHT = 2283;
    public const int IDC_BUTTON_ZOOM_OUT = 2272;
    public const int IDC_BUTTON_ZOOM_IN = 2273;
    //
    public const int CONST_mouseStatus_lBtnDown = 1;
    public const int CONST_mouseStatus_lBtnUp = 2;

}



namespace qy
{
    public struct PLAYER_ID
    {
        public int index_player;
        public uint uiTranNo_player;
    }
    ;
    public struct MSG_SIMPLE_ROUTE
    {                                       //  
        public QY_MESSENGER_ID idInfo_to;
        public QY_MESSENGER_ID idInfo_from;
    }
     ;

    public struct MSG_DEBUG_INFO
    {
        Int64 tTime_serv_startToRecv;                 //  iocpÆðÊ¼ÊÕ°üÊ±¼ä( s )
        /*
		ushort usElapsed_serv_recved;                   //  ÊÕÍê
		ushort usElapsed_serv_startToProcess;           //  ¿ªÊ¼´¦Àí		
		ushort usElapsed_serv_toFindDst;                //  ÔÚÑ°ÕÒÄ¿±ê¶ÔÏóÖ®Ç°¡£
		ushort usElapsed_serv_toBeSwitched;         //  ·ÖÈë
		ushort usElapsed_serv_toSend;                   //  ×¼±¸·¢ËÍ
		*/
    }
    ;


    public struct ROUTE_DEBUG_INFO
    {                                       //  ´Ë½á¹¹ÎªÁËÁË½âÊý¾Ý°üµÄ´«Êä¹ý³Ì¶øÉè¼Æ.Í¨³£,dwTickCnt...ÊÇ²»ÓÃ´«ÊäµÄ.


        // 发起端
        public long dwTickCnt_me_start1;                       //  clientÆðÊ¼·¢ËÍÇëÇó
                                                              //
        public Int64 tTime_start;                            //  ¿ªÊ¼·¢ËÍ
        public ushort usElapsed_toSend;                        //  ×¼±¸·¢ËÍ

        //  2008/11/19
        public int index;                                  //  uiDataBlockIndex

        //
        public uint dwTickCnt_serv_startToRecv;               //  iocp ( ms ). 

        //  2015/08/15
        public uint dwTickCnt_processQ;
        public uint dwTickCnt_processQ2Grp;
        //
        public uint dwTickCnt_postMsg2OutputQ_imGrp;
        //
        public uint dwTickCnt_doMisMgr_processSelf_imGrp;
        //
        public uint dwTickCnt_tmpHandler_talkToGrpMem_bt;
        public uint dwTickCnt_postMsg2OutputQ_messenger;

        //  server转发请求
        public MSG_DEBUG_INFO req;


        //  接受并响应端
        public uint dwTickCnt_peer_recved;
        public Int64 tTime_peer_recved;
        public ushort usElapsed_peer_resp_toSend;

        //  server转发响应
        public MSG_DEBUG_INFO resp;

        //  响应回到发起端
        public uint dwTickCnt_resp_recved;
        public ushort usElapsed_resp_end;

        //  2011/12/08
        public DEBUG_myDRAW_VIDEO_DATA debug_myDRAW_VIDEO_DATA;                //  2011/12/08

        //
        public uint uiStep_showInfo;                       //  ÎªÁË¸øshowInfoÌá¹©Çø·ÖÍ¬Ò»¸öÊ±¿Ì(ms)µÄÔç³ÙµÄ²ÎÊý

    }
    ;



    public unsafe struct MSG_ROUTE
    {                               //  
        public QY_MESSENGER_ID idInfo_to;                      //  2007/11/30, TALKDATA_ADDR
        public QY_MESSENGER_ID idInfo_from;                    //	
                                                               //
        public uint uiDevType_to;                  //
                                                   //WCHAR wDevIdStr_to[128];                //	2008/01/04, 
        public uint uiDevType_from;                    //
                                                       //WCHAR wDevIdStr_from[128];          //  2008/01/04
                                                       //  
                                                       //  path mailto:qycx@qycx.com|sm:13001101101|http://www.qycx.com/			
                                                       //																					
                                                       //
                                                       //
        public fixed UInt64 mems_to_idInfo_ui64Id[Consts.CONST_maxMsgrs_sendTo];

        //
        public uint uiMsgRouteId;                 //  2008/05/27, msgRoute

        //																					//  peer.
        //  uint									uiWebSessId;					//  webMessenger
        public uint uiLogonId;                     //  webMessenger
                                                   //
        public uint uiObjType_src;                    //  messengerType, 
                                                      //
        public ushort usLangId_from;                   //  
        public uint ulDetectedIp_from;
        //public fixed char displayName_from[64 + 1];
        //

        //  2008/05/15
        public ROUTE_DEBUG_INFO debugInfo;

        //  2009/09/12
        //struct                                             {
        public ushort tmpInternal_usCnt_mems_to;
        //}tmpInternal;

    };

    public struct MSGR_ADDR
    {
        public QY_MESSENGER_ID idInfo;
        //
        //public fixed char misServName1[Consts.CONST_maxMisServNameLen + 1];
        //
        public uint uiDevType;                     //
                                                   //WCHAR wDevIdStr[128];                   //   2008/01/04,
                                                   //WCHAR wPath[510 + 1];               //  
                                                   //																					//  
                                                   //  uint									uiWebSessId;					//  webMessenger
        public uint uiLogonId;                     //  webMessenger
                                                   //
        public uint uiObjType;
        //
        //public fixed char displayName1[64 + 1];
        //
        //
        //  2008/02/12, MSGR_ADDR messenger
        //  
        public bool bInNoticeMode;                 //  2008/01/22, 
                                                   //				
                                                   //				
        public Int64 tStartTime_notice;               //  tStartTime_notice uiTranNo_notice
        public uint uiTranNo_notice;               //

        //  2012/04/20
        public uint uiTranNo_shadow;               //  用来标识辅助talker窗口,为0时为主窗口

        //  2013/07/31
        //int												iNo_debug;						//  用来标识talker类实例,用来debug用. 2013/07/31

    }
    ;


    public unsafe struct QY_COMM_AUTHINFO_MIS
    {

        public ushort usAuthCmd;

        public int iAppType;                               //  2011/03/27
        public int iCustemId;
        public uint uiChannelType;                         //  2007/07/08

        public fixed byte ver[Consts.CONST_qyMaxVerLen + 1];            //  
                                                                        //

        public ushort usLangId;
        public uint uiDevType;
        //
        public ushort usAuthType;                              //  2011/03/22
                                                               //

        //QWM_MACS_INFO macsInfo;
        public fixed byte ip[Consts.CONST_qyMaxIpLen + 1];
        //WCHAR pcName[128 + 1];
        //WCHAR domainName[128 + 1];
        //WCHAR osUsrName[128 + 1];
        //

        //
        public fixed char messengerName[Consts.CONST_maxMessengerNameLen + 1];               //  messenger
        public fixed byte messengerPasswd[Consts.CONST_maxMessengerPasswdLen + 1];
        public fixed byte messengerNewPasswd[Consts.CONST_maxMessengerPasswdLen + 1];
        //
        public fixed char wRndName_serv[255 + 1];               //  
        public uint uiRnd_cli;                             //  web 2007/11/22
                                                           //
                                                           //  uint								uiWebSessId;							//  web2007/11/22
        public uint uiLogonId;                             //  2011/02/02
                                                           //  


        // ----------------------------------------
        public ushort usSessionEncType;

        public uint uiBlobLen_sessionKey;                  //  »á»°ÃÜÔ¿
        public fixed byte sessionKeyBlob[Consts.bufSize_sessionKeyBlob];

        public ushort usLen_sessionIv;
        public fixed byte sessionIv[Consts.bufSize_sessionIv];

    }
        ;

    //  ÈÏÖ¤ÏìÓ¦
    public unsafe struct AUTH_RESP_MIS
    {
        public QY_MESSENGER_ID idInfo;
        //
        public fixed char wRndName_serv[Consts.CONST_cntof_wRndName_serv];               //  webMessenger
                                                                                         //  uint								uiWebSessId;							//  webMessenger
                                                                                         //
        public uint uiLogonId;                             //  
                                                           //
        public uint uiCommEncCtxType;                      //  
        public uint uiBlobLen_publicKey;
        public fixed byte publicKeyBlob[Consts.bufSize_publicKeyBlob];                   //  serverHello

        //
        public bool ucbDaemonDemo;                            //  2009/10/11
        public ushort usMaxVideoConferenceMems;                //  2009/11/05
        public byte ucDays_left;                          //  2010/03/22
                                                          //
        public ushort usAppAvLevel_daemon;                 //  2011/10/22
                                                           //
        public ushort usSubSystemId_daemon;                    //  2017/06/21

        //
        public fixed char talkerDesc[Consts.cntof_talkerDesc];                           //  2011/04/07

        public fixed char wBuf_hint[Consts.cntof_hint];

        //
        public fixed char clientWelcomeMsg[Consts.CONST_cntof_clientWelcomMsg];                    //  2016/08/10
    }
    ;

    public unsafe struct QY_imAm_RCD
    {
        int id;
        fixed char misServName[255 + 1];
        QY_MESSENGER_ID idInfo;
        fixed char messengerName[255 + 1];
        fixed sbyte messengerPasswd[255 + 1];
        int iPasswdType;
        fixed sbyte salt[Consts.CONTS_passwdSaltLen + 1];
        int iStatus;
        fixed sbyte startTime[14 + 1];
        fixed sbyte tLastModifiedTime[14 + 1];
    }
   ;




    //  2014/09/04


    //  2016/07/23
    public struct RES_obj_simple
    {
        //M_resObj_common
        //

        public uint uiObjType;
        public ushort usIndex_obj;
        public ushort usHelp_subIndex;

    }
    ;

    //
    public struct QIS_RES_OBJ
    {
        //
        public QY_MESSENGER_ID idInfo;                     //  2016/07/24
        public RES_obj_simple resObj;
        //
    }

    //
    public unsafe struct QIS_dynBmp_info
    {
        //
        QIS_RES_OBJ qisResObj;                      //  2016/07/23
                                                    //
        fixed char name[32];                     //  name used to mean device id or unique name for dynBmp
        fixed char cusName[32];                  //  cusName used to mean a friendly comment for dynBmp
                                                 //
        ushort usW, usH;
        //
    }
        ;


    public unsafe struct TRANSFER_VIDEO_dataResp
    {
        public uint uiType;
        //
        public QY_MESSENGER_ID idInfo;
        public uint uiTranNo_openAvDev;                    //  tranNo.
                                                           //  uiTranNo_org
                                                           //
        public Int64 tSelectTime;                        //  2008/12/05
        public ushort usFps;                               //  
        public ushort usCnt_pkts;
        public ushort usDiff_pkts;                     //  2009/04/18

        //
    }
    ;

    public struct TRANSFER_AUDIO_dataMemHead_tmpInternal
    {
        public uint uiTranNo_org;                     //  Ô­Ê¼°üµÄ
        public ulong dwTickCnt_videoConferenceRecved;  //  ÊÓÆµ»áÒé·þÎñÆ÷ÊÕµ½µÄÊ±¼ä¡£ÓÃÀ´µ÷ÊÔºÍÁË½âÒôÆµ´«ÊäÐÔÄÜ¡£
    };


    public struct TRANSFER_AUDIO_dataMemHead
    {
        //unsigned  short									usIndex_ii;							//  2013/07/18. 将被逐渐弃用

        public QY_MESSENGER_ID idInfo;                             //  会议时,发言人的id

        //
        public uint uiTranNo_openAvDev;                    //  2013/08/04. 当会议参会人,断开会议,修改策略,重新参会时,需要有tn来标识不同的视频流,所以要加这个成员


        public TRANSFER_AUDIO_dataMemHead_tmpInternal tmpInternal;                      //  ÁÙÊ±Ê¹ÓÃµÄ±äÁ¿£¬²»ÄÜÓÃÔÚÍ¨ÐÅÖÐ

        public uint uiSampleTimeInMs;                 //  2009/05/02
        public uint ui_rtTimeLen;                      //  2009/05/27

        //  2015/09/10
        public uint uiPts;                             //  2015/09/10

        //
        public uint uiLen;
    }
;

    public struct TRANSFER_AUDIO_DATA_tmpInternal_param
    {                                    //  for procTaskAudio. 2009/05/12
        public int index_pMems_from;
        public int index_activeMems_from;
    };

    public struct TRANSFER_AUDIO_DATA_tmpInternal_result
    {

        public ushort usRespCode;
        public ushort usCnt_pkts;
    };


    public struct TRANSFER_AUDIO_DATA_tmpInternal
    {
        public ushort usCnt_mems;

        public TRANSFER_AUDIO_DATA_tmpInternal_param param;

        //  audio´¦Àí½á¹û£¬2008/04/17
        public TRANSFER_AUDIO_DATA_tmpInternal_result result;                               //  ÐèÒª·¢ËÍÖ¡ËÙÏìÓ¦Ê±£¬ÏÈ½«½á¹û±£´æÔÚÕâÀï¡£ÔÚtmpHandler_toMixerÀï×é°ü·¢ËÍ

    };


    //  »ìÒôÊý¾Ý£¬ÓÐÒ»¸öuiTranNo_openAvDev´ú±í¡£uiTranNo_openAvDev´ú±íÁËÊÓÆµ»áÒé·¢ÆðÕßµÄÒôÆµ±êÊ¶
    //  ±»mixerºó£¬ËùÓÐµÄ·¢ËÍ¶ËµÄÑ¹Ëõ¶¼Òª·ÅÆú£¬²¢±»×ª³ÉÊÓÆµ·þÎñÆ÷µÄÍ³Ò»Ñ¹Ëõ¡£//  ×¢£ºÔÝÊ±»¹Ã»ÓÐ
    public unsafe struct TRANSFER_AUDIO_DATA
    {
        public uint uiType;
        //
        public uint uiTranNo_openAvDev;                    //  ÓÃÀ´Ê¶±ð·¢ÆðÇëÇóµÄtranNo.
                                                           //
        public Int64 tLastTime_showFrameInfo_020534;     //  ÉÏÒ»´ÎµÄ·¢²¼Ö¡ËÙÐÅÏ¢µÄÊ±¼ä
        public ushort usCnt_pkts;                          //  ´ÓtLastTime_showFrameInfo->tStartTimeµÄ°üÊý

        public TRANSFER_AUDIO_DATA_tmpInternal tmpInternal;                        //  ÁÙÊ±Ê¹ÓÃµÄ±äÁ¿¡£²»ÄÜÓÃÔÚÍ¨ÐÅÖÐ

        public ushort usCnt;
        //TRANSFER_AUDIO_dataMemHead mems[CONST_maxTransferAvDataMems];
        public fixed byte buf_TRANSFER_AUDIO_dataMemHead_mems[Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems];

        public uint uiLen;                                 //  2009/05/20
        public fixed byte buf[Consts.bufSize_transferAudioData];    //  2009/05/20			 

    };

    //
    public unsafe struct TRANSFER_AUDIO_dataResp
    {
        public uint uiType;
        //
        public uint uiTranNo_openAvDev;
        public ushort usCnt_pkts;
        //
    }
    ;


    //
    //  2011/01/26
    public struct TRANSFER_videoData_stat
    {
        public Int64 tLastTime_showFrameInfo_020534;     //  

        //
        public Int64 tSelectTime;                        //  2008/12/05
        public ushort usCnt_pkts;                          //  tSelectTime. 020534 tLastTime_showFrameInfo->tStartTime
        public ushort usFps;                               //  

        //
        public int iVal_dbg;                           //  2017/09/04

    }
        ;




    //
    public struct TRANSFER_VIDEO_dataMemHead
    {
        //unsigned  short									usIndex_ii;							//  2013/07/18. 将被逐步弃用

        public QY_MESSENGER_ID idInfo;                             //  2009/05/31
                                                                   //
        public uint uiTranNo_openAvDev;                    //  2012/01/09

        //
        public uint uiSampleTimeInMs;                  //  2009/05/02
                                                       //uint									uiSampleTimeInMs_org;				//  2009/05/27

        //  2015/01/15
        public uint uiPts;                             //  2015/01/15

        //
        public byte ucbKeyFrame;


        //  2016/12/26
        //uint										uiEventId_lastRecvd_unused;				//  2013/11/15, 表示从remoteAssist收到了事件的id

        //
        public uint uiLen;                             //  ¶ÔÓ¦µÄÊý¾Ý³¤¶È

        //
        public TRANSFER_videoData_stat stat;                               //  2017/09/05

        //
    }
        ;

    public struct TRANSFER_VIDEO_DATA_tmpInternal_param
    {                                    //  for procTaskVideo. 2009/05/12

        //
        public int index_pMems_from;
        public int index_activeMems_from;
    };
    public struct TRANSFER_VIDEO_DATA_tmpInternal_result
    {
        public ushort usRespCode;
        public float fFps;
        public Int64 tSelectTime;
        public ushort usCnt_pkts;
        public ushort usDiff_pkts;                     //  2009/04/18
    };
    public struct TRANSFER_VIDEO_DATA_tmpInternal
    {
        public ushort usCnt_mems;

        public TRANSFER_VIDEO_DATA_tmpInternal_param param;

        //  ´¦Àí½á¹û					 

        public TRANSFER_VIDEO_DATA_tmpInternal_result result;


    };

    public unsafe struct TRANSFER_VIDEO_DATA
    {
        public uint uiType;
        //
        public uint uiTranNo_openAvDev;                    //  ÓÃÀ´Ê¶±ð·¢ÆðÇëÇóµÄtranNo.
                                                           //  ÒòÎªvideoÊÇË«Ïò·¢£¬ËùÒÔÔÚ»Ø¸´·½£¬Õâ¸öuiTranNo_orgÊÇÖ¸»Ø¸´µÄÇëÇó
                                                           //  2017/09/05
                                                           //TRANSFER_videoData_stat							stat;

        //
        //uint										uiEventId_lastRecvd_ii;				//  2008/11/22, ×îºóÒ»´ÎÊÕµ½µÄÊÂ¼þid.
        //  该变量不再使用了.放在mems[i]里去了. 2013/11/15

        //
        public TRANSFER_VIDEO_DATA_tmpInternal tmpInternal;

        //
        public ushort usCnt;
        //TRANSFER_VIDEO_dataMemHead mems[Consts.CONST_maxTransferAvDataMems];
        public fixed byte buf_TRANSFER_VIDEO_dataMemHead_mems[Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems];

        //
        public uint uiLen;
        public fixed byte buf[Consts.bufSize_transferVideoData];

    };







    public struct myDRAW_VIDEO_DATA_input
    {
        //
        public bool bRtsp;                                    //  2015/03/14

        //
        public Int64 tStartTran;                              //  2011/08/30

    };

    public struct myDRAW_VIDEO_DATA_vpp
    {
        uint uiCnt_vpp0;
        //
    };

    public struct myDRAW_VIDEO_DATA_decInfo
    {
        public int index_pMems_from;
        //  2016/12/27
        public uint uiEventId_lastRecvd_unused;                    //  2008/11/22

        //  2014/09/04
        public QIS_RES_OBJ dynBmp;                                 //  2014/10/12

        //
        public bool ucbUnresizable;                            //  2015/10/23

    };


    public struct DEBUG_myDRAW_VIDEO_DATA
    {
        public uint dwTickCnt_start;
        public ushort usToEncInMs;
        public ushort usEncedInMs;
        public ushort usPostInMs;
        public ushort usToDispatchInMs;
        public ushort usOutputInMs;
        public ushort usPreTransInMs;
        public ushort usTransInMs;

        //
        public uint dwTickCnt_proc_io_read;                       //  2015/09/05
        public uint dwTickCnt_doPretrans;                     //  2015/02/24
        public uint dwTickCnt_drawVideoData;                  //  2015/09/06
        public uint dwTickCnt_playPkt_start;                  //  2015/09/11

        //  2015/09/05
        public uint uiTranNo_msgTask;                          //  2015/09/05

        //
        //M_debugInfo_pktNo       
        public int lPktNo;
        //
    }
    ;


    public struct myDRAW_VIDEO_DATA
    {
        public uint uiType_unused;
        //
        //uint										uiTranNo_openAvDev_unused;				//  2014/11/26.tranNo.

        //
        public uint uiCapType;
        public int iIndex_capBmp;

        public myDRAW_VIDEO_DATA_input input;

        //  2015/05/07

        public myDRAW_VIDEO_DATA_vpp vpp;

        //  2016/12/27
        public uint uiEventId_lastRecvd_unused;

        //
        //public IntPtr hWnd_task;
        public int iTaskId;
        public uint uiTaskType;                                //  2014/10/01

        //
        public QY_MESSENGER_ID idInfo_peer;

        //  2011/12/09
        public myDRAW_VIDEO_DATA_decInfo decInfo;                                //  

        //  2011/12/08
        public DEBUG_myDRAW_VIDEO_DATA debugInfo;

        //  
        public int lPktId_alloc;                          //  2014/03/26.一个不重的序号. 分配内存的标识

        //
        public TRANSFER_VIDEO_dataMemHead head;                                    //  head.len表示pBuf中的缓冲长度, head.len和bih.biSizeImage不一定相等
        public ushort usIndex_activeMems_from;             //  2013/07/18.

        //  2015/09/30
        public ushort usPktResType;                            //  2015/09/30

        //  2015/09/30
        //PKT_sharedTex_info pktSharedTexInfo;

        //  2011/12/06
        public uint uiSize_dec;                                //  pBuf中分为2个区，一个为未压缩数据区，大小为uiSize_dec. uiSize_dec应<=head.len
        public BITMAPINFOHEADER bih;                                   //  2011/12/04. 此bih表明了未压缩区中的图像格式, bih.sizeImage应该<=uiSize_dec
                                                                       //
        public uint uiOffset_enc;
        public uint uiLen_enc;                             //  另一个为压缩数据区，大小为head.len - uiSize_dec，包含一个包头uiOffset_enc，紧跟一个压缩数据. 真实大小为uiLen_enc
                                                           //
                                                           //  2014/02/24. 当bRtsp为true, p->uiLen_enc  =  offsetof(  TRANSFER_VIDEO_DATA,  buf  )  +  BufferLen;


        //  char											*	pBuf;								//  2009/12/07, len is in head
        public QY_MEMORY memory;                                    //  2010/04/23

        //

    };


    //////////////////////////////////////////////////////////////////////
    ///
    public unsafe struct IM_GRP_INFO
    {
        public uint uiType;                                //  2011/01/29
                                                           //
        public int id;
        public fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        public QY_MESSENGER_ID idInfo;                             //  
                                                                   //
        public fixed char name[Consts.CONST_maxNameLen + 1];                    //  
        public ushort usSubtype;                           //  2014/01/31
                                                           //
        public QY_MESSENGER_ID idInfo_creator;                     // 2017/07/26
                                                                   //
        public Int64 startTime;
        public Int64 tLastModifiedTime;
        //
        /*
		struct                                             {                                    //  2011/01/29
					 HWND hWnd_status;
		//
	}
	tmpInternal;
		*/
    }
;
    public unsafe struct IM_GRP_MEM
    {
        public uint uiType;
        public int id;
        public fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        public QY_MESSENGER_ID idInfo_grp;
        public QY_MESSENGER_ID idInfo_mem;
        public int iRole;
        public Int64 startTime;
        public Int64 tLastModifiedTime;
        public int iStatus;
    }
    ;


    public struct MY_REG_DESC
    {
        public string pZb;
        public string pBb;
        public string pDw;
        public string pBm;
        public string pSyr;
    }
    ;


    public unsafe struct QY_MESSENGER_PCINFO
    {
        int id;

        fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        QY_MESSENGER_ID idInfo;
        //
        //
        uint uiType;
        //
        int iPlatformId;
        //
        fixed char pcName[255 + 1];
        fixed char domainName[255 + 1];
        fixed char osUsrName[255 + 1];
        //
        Int64 tLastModifiedTime;

    }
    ;

    public unsafe struct QY_MESSENGER_phoneInfo
    {

        int id;
        fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        uint uiDevType;
        //TCHAR wDevIdStr[128 + 1];
        //
        fixed char messengerPasswd[255 + 1];
        //
        QY_MESSENGER_ID idInfo;
        //
        int iStatus;
        //																				
        Int64 startTime;
        Int64 tLastModifiedTime;

    }
    ;


    public unsafe struct QY_MESSENGER_INFO
    {

        public uint uiType;                                        //  messengerType, me, messenger, group

        public int iRcdId;                                     //  qyMessengerTab id

        public fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        public QY_MESSENGER_ID idInfo;

        //
        public uint uiLogonId;                                 //  2007/12/22
        public Int64 tTime_logon;                             //  2022/02/02
        public fixed char wLocation[64 + 1];

        //
        public fixed byte ip[Consts.CONST_qyMaxIpLen + 1];
        public fixed byte detectedIp[Consts.CONST_qyMaxIpLen + 1];

        public ushort usLangId;
        //
        public uint uiDevType;
        //
        public fixed char messengerName[64 + 1];

        //
        public int iRole;                                      //  2017/08/14

        //
        public Int64 startTime;
        //
        public ushort usRunningStatus;                         //  2007/06/07 

        public Int64 tLastModifiedTime;

        public fixed byte clientVer[Consts.CONST_qyMaxVerLen + 1];
        //
        public ushort shClientVer;             // clientVer[0-3];
        public byte blClientVer;               // clientVer[4-5];		//  2005/05/23, lhQwmVer  =  atol(  qwmVer  )  /  100;  llQwmVer  =  atol(  qwmVer  )  %  100;
        public byte b_mantissa_clientVer;      // clientVer[6-7]

        //
        public int iStatus;

    }
        ;



    //  ´Ë½á¹¹²»ÓÃÀ´´«Êä£¬Òª×ª»»³ÉÁ÷²Å½øÐÐ´«Êä
    public unsafe struct QY_MESSENGER_REGINFO
    {
        public uint uiType;
        //
        public int id;

        public int iCustomId;

        //  TCHAR												misServName[CONST_maxMisServNameLen  +  1];
        //  QY_MESSENGER_ID									idInfo;
        public MSGR_ADDR addr;

        public fixed byte regTime[Consts.CONST_qyTimeLen + 1];
        public fixed byte auditTime[Consts.CONST_qyTimeLen + 1];
        public fixed byte tLastModifiedTime[Consts.CONST_qyTimeLen + 1];
        public fixed byte tCommitTime[Consts.CONST_qyTimeLen + 1];


        public fixed uint uiFieldIds[Consts.CONST_qnmMaxRegFields];
        /*
		union												{
					 //  WCHAR											cols[CONST_qnmMaxRegFields][256];
					 WCHAR cols[CONST_qnmMaxRegFields][64  +  1];		//  2007/12/01, ÒòÎªMIS_MSGU¹ý´ó£¬Ôì³Éstack overflow,¹Ê½«×Ö¶Î´óÐ¡ÏÞÖÆÔÚ64¸ö×Ö·û.
				 }	u;

				 void* pInternal;                           //  ÒÔÏÂÎªÄÚ²¿±äÁ¿£¬²»ÓÃÀ´Í¨ÐÅ
															//
	QNM_FIELD fields[CONST_qnmMaxRegFields];        //  2007/08/03, ´ú±íÁËÊý¾Ý¿âÀïµÄ´æ´¢Êý¾Ý
		*/
        public fixed char dw[Consts.CONST_maxNameLen + 1];
        public fixed char bm[Consts.CONST_maxNameLen + 1];
        public fixed char syr[Consts.CONST_maxNameLen + 1];

        //
        int index;
        public fixed char tmp_col[Consts.CONST_maxNameLen + 1];

    }
    ;


    public unsafe struct REFRESH_imObjRule_reqMem
    {
        QY_MESSENGER_ID idInfo_related;
        uint uiRuleType;
        ushort usIndex;
        uint uiCmd;
        //  
        fixed char wDesc[128];           //  2011/11/13
    }
    ;





    //
    public unsafe struct REFRESH_imObjRules_req
    {
        uint uiType;

        byte ucbResp;
        byte ucbRetrieveAll;

        ushort usMaxMemsPerSnd;         //  2011/11/13


        //					
        byte req_ucbNext;
        //
        QY_MESSENGER_ID req_idInfo_related;
        uint req_uiRuleType;
        ushort req_usIndex;


        //
        byte ucbEnd;


        ushort usCnt_mems_tmpInternal;

        //
        ushort usCnt;
        //REFRESH_imObjRule_reqMem mems[MAX_imObjRule_reqMems];				 
        fixed byte buf_REFRESH_imObjRule_reqMem[Consts.bufSize_REFRESH_imObjRule_reqMem_mems];
    };


    //  2011/11/13. qm的全局策略，也是存在数据库里
    public struct QY_im_rules
    {
        //  2011/10/15
        //  unsigned  char										ucbShareWebcamInConference;						//  是否在会议中共享摄像机. 如果选是，则在会议自动接收时，要弹出共享窗口
        QY_MESSENGER_ID conferenceId_shareWebcam;
        byte ucbNoLocalVideoInThisConference;              //  是否在这个会议中启动视频. 2011/10/22
                                                           //
        byte ucbDistinguishSharedWebcams;                  //  是否区分被共享的摄像机，（是否使用序号）
                                                           //
    }
    ;

    //  
    public struct QY_imObj_rules
    {
        uint uiCmd_autoAnswer_av;
        uint uiCmd_autoAnswer_screen;   //  2010/12/18
        uint uiCmd_autoAnswer_mediaFile;    //  2010/12/18
    }
    ;


    //int imObjRulesReq2ImRules(REFRESH_imObjRules_req* pReq, QY_im_rules* pRules);
    //int imObjRulesReq2Rules(REFRESH_imObjRules_req* pReq, QY_MESSENGER_ID* pIdInfo_related, QY_imObj_rules* pRules);


    //  2010/08/13
    public unsafe struct QY_imObjRule_rcd
    {
        int id;

        fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        QY_MESSENGER_ID idInfo;
        //
        uint uiRuleType;
        //
        ushort usIndex;
        QY_MESSENGER_ID idInfo_related;
        uint uiCmd;
        //
        Int64 startTime;
        Int64 tLastModifiedTime;
        //
        int iStatus;
        //
        fixed char col0[255 + 1];


    }
    ;



    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    public struct IM_SIMPLE_RESP
    {
        public uint uiType;
        public ushort usCmd_org;
        public uint uiContentType_org;
        public int iStatus;
        //
    }
        ;

    public unsafe struct IM_HTML_CONTENT
    {                                   //  
        public uint uiType;                                //  
                                                           //
        public byte ucbResp;                          //  2014/07/11
                                                      //
                                                      //  2015/02/16
                                                      //unsigned  char										ucbNotification;
        public QY_MESSENGER_ID idInfo_logicalPeer;                 //  2015/02/16

        //  Int64											tStartTime_serv;					//  
        public uint uiSerialNo;                            //  
        public fixed char wBuf[Consts.cntof_imHtmlContent_wBuf];
    }
    ;


    public class CIM_HTML_CONTENT
    {
       public IM_HTML_CONTENT content;
    };


    //  
    //  1 CONST_imCommType_transferFileInfo, 
    //  2 CONST_imCommType_transferFileInfoReply¸OK
    //  3 CONST_imCommType_transferDateReq¸CONST_imCommType_transferFileDataResp¸
    //  4 CONST_imCommType_transferFileDataEndResp¸

    //  2009/10/20
#if false
	public unsafe struct PROC_TASK_FILE
	{                                           //  video

		public void * hWnd_task;                                 //  2009/09/09

		public bool bTaskInvalid;                              //  2009/09/14

		//
		public bool bNeedRemoved;                              //  2015/09/08

	}
	;
#endif

    public unsafe class PROC_TASK_FILE
    {                                           //  video

        public object hWnd_task;                                 //  2009/09/09


        //
        public struct procTaskFile_reply_info
        {
            public Int64 tStartTime_reply;
            public uint uiTranNo_reply;
            //
            public TRANSFER_FILE_REPLYREQ replyReq;

            //
            public Int64 tStartTime_lastFileDataReq;
            public uint uiTranNo_lastFileDataReq;

        };
        public procTaskFile_reply_info reply_info;

        //
        public object fp;

        //
        public bool bNeedRemoved;

        //

#if true
        //
        public UInt64 ui64FileLen;
        //
        public string fileName_transferred; //  
        public string fullFilePath_save;    //  
        public uint uiSizePerSnd_internal;              //  

        //
        public uint uiLastDataBlockIndex;               //  
        public ushort usErrors;                         //  

        //
        public byte ucPercent;                          //  

        //
        public int nTimes_toRestart;

#endif


    }
;


    //  
    public unsafe struct TRANSFER_FILE_REQ
    {                                               //  2007/06/23
        public uint uiType;
        //
        public UInt64 ui64FileLen;
        public fixed char fileName[Consts.cntof_fileName];

        //
        public uint uiSizePerSnd_suggest;                          //  2014/07/13

        //  2015/08/05
        public ushort usSubtype;
        public QY_MESSENGER_ID idInfo_logicalPeer;                             //  2015/08/05

        //
        //  
        public void* pInternal;

        //public PROC_TASK_FILE myTask;

        public fixed char fullFilePath[Consts.MAX_PATH + 1];       //  
                                                                   //
        public bool bGrpTask;                          //  

        //
        //FILE* fp;
        //public void* fp;

        //
        public uint dwStartTickCnt;                       //  
        public uint dwLastReportTickCnt;              //  
                                                      //
        public byte ucPercent;                            //  

        //
        public ushort usErrors;                            //  

        //
        public void* hTool_offlineRes;                  //  2015/08/06
        public bool bNeedCalcel;                       //  2015/08/08

    }
        ;


    //  »Ø¸´ÎÄ¼þ´«ËÍÐÅÏ¢
    public unsafe struct TRANSFER_FILE_REPLYREQ
    {                                   //  2007/07/06
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  
        public uint uiTranNo_org;                      //
        public uint uiSerialNo_org;                        //  
                                                           //  
        public ushort usOp;
        //

        //  ÄÚ²¿´¦Àí
#if false
		public void* pInternal;

		public PROC_TASK_FILE myTask;                              //  2009/10/20

		//
		public UInt64 ui64FileLen;
		//
		public fixed char fileName_transferred[Consts.cntof_fileName];    //  
		public fixed char fullFilePath_save[Consts.MAX_PATH + 1];  //  
		public uint uiSizePerSnd_internal;             //  

		//FILE* fp;
		public void* fp;

		public uint uiLastDataBlockIndex;              //  
		public ushort usErrors;                            //  

		//
		public uint dwStartTickCnt;                       //  
		public uint dwLastReportTickCnt;              //  
													  //
		public byte ucPercent;                            //  

#endif

    }
        ;


    public struct TaskProcReq_vStream
    {
        //  2014/08/26. 也许这里应该引入一个objType，这样，就可以把清晰的指定要对什么做操作了.在要操作的对象多起来的时候，需要objType来区分下
        public uint uiTranNo_openAvDev;
    };


    public struct TaskProcReq_viewDynBmp
    {
        public uint uiObjType;
        public ushort usIndex_obj;                         //  2009/09/06
    };
    /// <summary>

    public unsafe struct QY_MESSENGER_ACCOUNT
    {
        public int uiType;
        // public fixed char name[Consts.cntof_name];			
        public fixed byte messengerPasswd[Consts.cntof_messengerPasswd + 1];


    }
    ;

    /// </summary>

    public unsafe struct TASK_PROC_REQ
    {                                   //  2007/07/06
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  
        public uint uiTranNo_org;                      //
        public uint uiSerialNo_org;                        //  
                                                           //
        public uint uiContentType_org;                 //  2008/05/29
                                                       //  

        //
        public ushort usOp;

        //
        public ushort usTaskProcObjType;                       //  表明下面要操作的对象是什么

        public TaskProcReq_vStream vStream;

        //  2009/09/11.  //  2014/08/26. 注意： 下面的几个变量uiTaskType,viewDynBmp，并没有在使用中，
#if true          //  2014/10/09. 启用
        public uint uiTaskType;
        //
        public TaskProcReq_viewDynBmp viewDynBmp;
#endif

        //  
        public void* pInternal;

        //
        //  char											fileName_save[MAX_PATH  +  1];

        //  2014/10/09
        public void* hWnd_task;

    };
    //TRANSFER_FILE_PROCREQ,  TRANSFER_FILE_procReq,  TASK_PROC_REQ;


    //
    public unsafe struct TASK_PROC_RESP
    {
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  2009/10/19
        public uint uiTranNo_org;                      //
                                                       //
        public ushort usCmd_org;
        //  uint										uiContentType_org;
        //
        public ushort usOp_org;
        //
        public int iStatus;                            //  2009/10/19

        public QY_MESSENGER_ID idInfo_logicalPeer;                 //  2009/10/18

        //
        public void* pInternal;

        public bool bNeedRemoved;

    }
    //TRANSFER_FILE_PROCRESP,  TRANSFER_FILE_procResp,  TASK_PROC_RESP;






    public unsafe struct TRANSFER_FILEDATA_REQ
    {                                   //  2007/07/15
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  ÇëÇó·¢ÆðµÄÊ±¼ä, ÕâÊÇÎªÁËÈ·¶¨ÊÇÄÄ¸öÈÎÎñ±»µÃµ½ÁËÔÊÐí
        public uint uiTranNo_org;                      //
        public uint uiSerialNo_org;                        //  Ã»Ê²Ã´ÓÃ
                                                           //
        public ushort usOp;
        //
        public uint uiSizePerSnd;                      //  Õâ´Î×î¶à·¢ËÍµÄ×Ö½ÚÊý, Ó¦¸ÃºÍReplyReqÖÐµÄÒ»Ñù
                                                       //
        public uint uiDataBlockIndex;                  //  µÚ¼¸¿éÊý¾Ý£¬´Ó0¿ªÊ¼

        //
        public void* pInternal;

        //
        public struct TransferFileDataReq_tmpInternal
        {

            public Int64 tStartTime_new;
            public uint uiTranNo_new;

            //
            public bool ucbFileEnd;

        };
        public TransferFileDataReq_tmpInternal tmpInternal;



        //public bool ucbFileEnd;

    }
    ;


    //
    public class CTRANSFER_FILEDATA_REQ
    {
        public TRANSFER_FILEDATA_REQ    transferFileDataReq;

    };

    public unsafe struct TRANSFER_FILEDATA_RESP
    {                                   //  2007/07/15
        public uint uiType;
        public uint uiDataBlockIndex;                  //  buf pInternal, 
        public uint uiLen;
        public fixed byte buf[Consts.bufSize_transferFileDataResp];
        //

    }
        ;

    public class CTRANSFER_FILEDATA_RESP
    {
        public TRANSFER_FILEDATA_RESP   transferFileDataResp;

    };


    public struct TRANSFER_FILEEND_REQ_tmpInternal
    {
        //  2015/09/07
        public bool bChkToRemove_i;                       //  2015/09/07
                                                          //
        public bool bNeedRemoved_o;
    };


    public unsafe struct TRANSFER_FILEEND_REQ
    {                                   //  2007/07/15
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  
        public uint uiTranNo_org;                      //
        public uint uiSerialNo_org;                        //  

        public TRANSFER_FILEEND_REQ_tmpInternal tmpInternal;


    }
;

    public struct TRANSFER_FILEEND_RESP_tmpInternal
    {
        public bool bChkToRemove_i;
        //
        public bool bNeedRemoved_o;
    };


    public unsafe struct TRANSFER_FILEEND_RESP
    {                                   //  2007/07/15
        public uint uiType;
        public uint uiVerifyMethod;                        //  
        public fixed char buf[2048];

        //  2015/09/07
        //
        public TRANSFER_FILEEND_RESP_tmpInternal tmpInternal;


    };

    public class CTRANSFER_FILEEND_RESP
    {
        public TRANSFER_FILEEND_RESP transferFileEndResp;

    };


    ////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// 
    /// 
    /// 






    /// 
    /// 
    /// 
    /// </summary>
    public struct AEC_cfg
    {
        bool bUseAec;                                   //  2013/05/25

        //
        bool bFeatrModeOn;                              //  2014/08/17
        bool bNoiseSup;                                 //  2014/08/17
        bool bAGC;                                      //  2014/08/17
        bool bCntrClip;                                 //  2014/08/17

        //
        uint uiEchoLengthInMs;

    }
    ;

    //
    public struct VIDEO_CONFERENCE_CFG
    {
        public int iRows;                                      //  ·ÖÊÓÆµ×é³ÉÒ»¸ö¾ØÕó¡£ÐÐÊý
        public int iCols;                                      //  ÁÐÊý

        //
        public ushort usMaxSpeakers;                               //  2009/02/19


        //
        public AEC_cfg aecCfg;

    };


    /// <summary>
    /// ////////////////////
    /// </summary>
    /// 
    //
    //  2016/04/18

    //  2011/07/23
    public unsafe struct VIDEO_COMPRESSOR_CFG
    {
        public VIDEO_COMPRESSOR_cfgCommon common;
        //TCHAR compressorName[64];

        //
        //public int iTaskId;
        public object shared_eglContext;

        //
        public int index_sharedObj;

        //
        public bool m_bFront;

    }
;


    public unsafe struct AUDIO_COMPRESSOR_CFG
    {
        //  unsigned  char										ucCompressors;								//  acm or dmo
        public AUDIO_COMPRESSOR_cfgCommon common;
        //TCHAR compressorName[64];                           //  2009/05/16

        //
        public QY_Q2 pAudioQ2;
        public object pParent_transform;



        //
        public WAVEFORMATEX wf_compress;
        public WAVEFORMATEX wf_decompress_default;

        //
        //  GUID												dmoClsid;

        //AUDIO_ENC_PARAMS									params;

        //
        public uint uiBytesRecorded_perBlockAlign;                //  ÒôÆµµÄ¶ÔÆëµ¥ÔªµÄ»º³å´óÐ¡

        /*
		//  2014/05/20
		struct                                             {
						//
		unsigned char ucbSetOrgAudioFmt;

		//
		WAVEFORMATEX wf_org_suggested;

	}
	dev;
		*/

        //
        public AUDIO_PLAY_CFG playCfg;                                    //  2010/03/11

        //
        //  uint										uiRecordType;								//  2010/08/02

    }
;



    public struct AV_COMPRESSOR_CFG
    {                                           //  2009/05/21
#if true
        public byte ucSeconds_perFrame_videoConference;            //  
        public ushort usFrames_perSecond_videoConference;          //  
#endif

        public AUDIO_COMPRESSOR_CFG audio;
        public VIDEO_COMPRESSOR_CFG video;
    }
    ;



    public struct AV_tranInfo_head
    {

        //  2014/09/23
        public ushort shClientVer;                                     //  2014/09/23. 客户端版本号的高位

    }
    ;


    //
    //
    public struct CONF_hg_info
    {
        public long ui64MeetingId_hg;       //  2023/04/04
        public int iMeetingType_hg;

        //
        public int iMeetingLevel_hg;
        public int iMeetingDurationInMin;      //  分钟

        //
        public long ui64_meetingStartTime;
        //TCHAR meetingCompere[64];

        //
        public bool bRecord;
        public bool bShare_2ndStream;

        //
        /*
#define MAX_hg_shareMems										256
    struct {

        QY_MESSENGER_ID idInfo;
}
shareMems[MAX_hg_shareMems];
		*/


        //
        public bool bFlow512k;


        //
        public struct Hg_tmpInternal
        {
            public int cnt_shareMems;
        };

        //
        public Hg_tmpInternal tmpInternal;


    };


    //
    public struct TRAN_conf_cfg_conf
    {
        public ushort usMaxFps_toShareBmp_conf;                           //  2009/04/19					
                                                                          //  2014/04/20. 				
        public int iWidth_pic_conf;                                    //  2014/08/15
        public int iHeight_pic_conf;                                   //  2014/08/15

        //
        //					
        public int conf_iFourcc;


        //  2017/07/06
        public struct TRAN_conf_cfg_conf_confCtrl
        {
            //
            public byte ucbConfCtrl_starter;
            //
        };
        public TRAN_conf_cfg_conf_confCtrl confCtrl;

    }
;


    //
    public unsafe struct TRAN_conf_cfg
    {

        //  2014/09/23
        //  2014/09/23
        public ushort usMaxSpeakers;

        //				
        public TRAN_conf_cfg_conf conf;

        //  2017/08/02
        public byte ucbVideoConference;

        //  2017/07/17
        //public byte ucbAvConsole;

        //  2017/08/25
        public ushort usConfType;

        //  
        public QY_MESSENGER_ID idInfo_initiator;

        //
        public CONF_hg_info hgInfo;


    };
    public unsafe struct IpcProc
    {
        //

        //				 				 					
        //
        public VH_stream vh_stream;                                            //  2014/02/25
        public Op op;                                                                   //
        public QY_VIDEO_HEADER vh_decompress;

    }
; public struct dev
    {
        public NvrInfo nvrInfo;
    };

    public unsafe struct Op
    {
        public QY_MESSENGER_ID idInfo;
        public NvrInfo nvrInfo;
        public int iChannel;
        public int nID;
        public byte ucMouseStatus;
    };


    //  2014/11/07
    public struct TRAN_video
    {
        //
        public uint uiTranNo_openAvDev;

        //				 				 					
        public VIDEO_COMPRESSOR_cfgCommon_head compressor;                                            //  2009/03/29				
                                                                                                      //
        public QY_VIDEO_HEADER vh_compress;
        public VH_stream vh_stream;                                            //  2014/02/25
                                                                               //
        public QY_VIDEO_HEADER vh_decompress;

    }
    ;


    //  2014/11/07
    public  struct TRAN_audio
    {
        public uint uiTranNo_openAvDev;

        //				
        //public AUDIO_COMPRESSOR_CFG compressor;                                            //  2009/05/16
        public AUDIO_COMPRESSOR_cfgCommon_head compressor;                                            //  2009/05/16

        //				
        public QY_AUDIO_HEADER ah_compress;                                        //  2009/05/16	
        public AH_stream ah_stream;                                            //  2016/12/23

        //
        public QY_AUDIO_HEADER ah_decompress;                                      //  2009/05/16

    }
        ;




    //

    //  2015/03/11
    public struct TASK_v_props
    {
        public byte ucAvFlg;
    }
        ;
    //
    public struct TASK_av_props
    {
        public TASK_v_props v;
    }
        ;



    //
    public struct AV_TRAN_INFO
    {

        //  2014/11/07
        public AV_tranInfo_head head;

        //
        public TRAN_audio audio;
        public TRAN_video video;

        //  2015/03/12
        public TASK_av_props taskAvProps;

    }
        ;


    //  2014/11/07
    public struct MOSAIC_TRAN_INFO
    {

        //
#if false
		public uint uiObjType;                                         //  2014/11/07. 在transferAvInfo.video_conf,resource_conf要用objType和usIndex_obj来和原来的流区分
		public ushort usIndex_obj;                                     //  2014/11/07	
#endif
        //
        public RES_obj_simple resObj;
        //
        public TRAN_video video;

    }
        ;



    public struct TRANSFER_AV_INFO_viewDynBmp
    {
        //  2016/07/24
        //public RES_obj_simple resObj;
        //
        public byte ucbSaveVideo;                     //  2014/08/04
    };


    public struct PROXIED_TRAN_INFO
    {
        public QY_MESSENGER_ID idInfo;
        public uint uiObjType;
        public AV_TRAN_INFO ti;
    }
    ;

    public struct TRANSFER_AV_INFO_tmpInternal_streamInfo
    {
        public QY_MESSENGER_ID idInfo;
        public uint uiObjType;
        public ushort usIndex_obj;
    };

    //  2008/03/15.
    public struct TRANSFER_AV_INFO
    {
        public uint uiType;
        //
        public byte ucbResp;
        //
        public uint uiTaskType;                            //  				 

        //
        public TRAN_conf_cfg confCfg;

        //
        //public AV_TRAN_INFO	tranInfo;
        public AV_stream_simple ass;

        //  2014/11/07
        public MOSAIC_TRAN_INFO confMosaicTranInfo_video, confMosaicTranInfo_resource;

        //
        public TRANSFER_AV_INFO_viewDynBmp viewDynBmp;                          //  2009/09/06

        //
        public QY_MESSENGER_ID idInfo_logicalPeer;                    //  2009/10/18

        //
        public uint uiTranNo_avTask_starter;           //  2014/11/22. 这个tranNo用来在会议和共享资料等任务里建立关联。

        //  2014/11/20
        public PROXIED_TRAN_INFO proxiedTranInfo;


        //
        public IntPtr pInternal;

        //
        public bool bGrpTask;

        //  2016/04/18
        public ushort tmp_usSubtype;

        //  2014/11/09
        public TRANSFER_AV_INFO_tmpInternal_streamInfo tmp_internal_streamInfo;

        //
        public ushort tmp_internal_usCnt_consult_mems;

        //
        public AV_stream tmp_internal_tmpAs;

    };

    //
    public struct TransferAvReplyInfo_viewDynBmpReply
    {
        public byte ucbSaveVideo;
    };

    public struct TransferAvReplyInfo_tmp_internal
    {
        public AV_stream tmpAs;
    };

    //  
    public struct TRANSFER_AV_replyInfo
    {                                   //  2007/07/06
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  
                                                         //  tStartTime_org tranNo
        public uint uiTranNo_org;                      //
                                                       //  
        public ushort usOp;
        //
        public AV_TRAN_INFO tranInfo;

        //  2014/09/04
        public TransferAvReplyInfo_viewDynBmpReply viewDynBmpReply;


        //  
        public IntPtr pInternal;

        //
        //  2016/04/18
        public ushort tmp_usSubtype;

        //
        public TransferAvReplyInfo_tmp_internal tmp_internal;

        //
        //  PROC_TASK_AV										myTask;

    };


    public struct AV_stream_simple
    {
        public RES_obj_simple resObj;
        public AV_TRAN_INFO tranInfo;
    }

    public struct AV_stream
    {
        public QY_MESSENGER_ID idInfo;
        public AV_stream_simple obj;

    }

    public unsafe struct TASK_INTERACTION_REQ
    {                                   //  2009/02/23
        public uint uiType;
        //
        public Int64 tStartTime_org;                     //  
        public uint uiTranNo_org;                      //
                                                       //
                                                       //uint									uiContentType_org;					//  2008/05/29
                                                       //  uiContentType transferFileReq
                                                       //
        public ushort usOp;
        //
        //
        public AV_stream avStream;

        //
        //
        public fixed char talkerDesc[Consts.cntof_talkerDesc];


        //  2015/07/30
        //public QY_MESSENGER_ID idInfo_speaker;                     //  2015/07/30. 增加非发起方的会议主持人，所以需要把idInfo_speaker发给starter来做处理

        //
        public QY_MESSENGER_ID idInfo_imGrp_related;               //  2009/02/23

        //  
        public void* pInternal;

        //

    }
    ;



    /// <summary>
    /// //////////////////////////////////////////////////
    /// </summary>

    public unsafe struct IM_TASK_RCD
    {

        public int id;

        public fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        public QY_MESSENGER_ID idInfo_send;            //  ·¢ËÍÈË
        public fixed char senderDesc[Consts.cntof_senderDesc];
        //
        public Int64 tSendTime;               //  ·¢ËÍÊ±¼ä
        public uint uiTranNo;              //  ·¢ËÍµÄ½»Ò×ºÅ 
        public ushort usSeqNo;
        //

        public uint uiContentType;         //  ·¢Æð·½µÄÇëÇóµÄÄÚÈÝÀàÐÍ
                                           //

        public Int64 tStartTime_serv;     //  È±Ê¡¿ÉÌî"", ÔÚÌÖÂÛ×éÊ±ÓÐÓÃ
        public uint uiSerialNo;                //  È±Ê¡Îª0£¬ÔÚÌÖÂÛ×éÀ´·¢ËÍÊ±£¬Ó¦Ê¹ÓÃtServTimeºÍuiSerialNoÀ´±£Ö¤Ë³ÐòºÍÒ»Ð©ÈÎÎñµÄ²»¿É·Ö¸îÐÔ¡£
                                               //
        public QY_MESSENGER_ID idInfo_recv;            //  ½ÓÊÕ·½  
        public fixed char receiverDesc[Consts.cntof_receiverDesc];
        public int iRole;
        //
        public Int64 tRecvTime;               //  ½ÓÊÕÊ±¼ä
        public ushort usRespCode;              //  Í¨ÐÅµÄÏìÓ¦Âë
                                               //
                                               //
        public int ibReply;                //  ÊÇ·ñ»Ø¸´
                                           //
        public uint uiType;                    //  ÈÎÎñÀàÐÍ£¬±ÈÈç£ºÎÄ¼þ´«ËÍ, ÓÃqyCmd_sendTaskÖÐµÄimCommTypeÀ´±£´æ¾Í¿ÉÒÔÁË
                                               //  2008/05/29, Õâ¸öuiTypeÓÃ´¦²»ÊÇºÜ´ó¡£ÒòÎªÒ»¸öÈÎÎñÒÑ¾­ÓÃidStr_send, tSendTime, uiTranNo À´¾ö¶¨ÁË¡£
                                               //  ¹¤×÷ÄÚÈÝÓÃuiContentType¾ö¶¨ÁË¡£messengerµÄ½ÇÉ«ÓÃÊÇ²»ÊÇidStr_sendÒÑ¾­¾ö¶¨ÁË¡£
                                               //  Õâ¸ö±äÁ¿£¬¸üÏóÒ»¸ö±ãÓÚ´¦ÀíµÄËµÃ÷ÐÔ±äÁ¿¡£

        //
        public uint uiMode;                    //  Ð­Í¬Ä£Ê½,//  Ð­Í¬Ä£Ê½£¬±È·½£ºÁôÑÔ£¬Í¨Öª£¬Òª»Ø¸´µÄÍ¨Öª£¬¿ÉÈ¡ÏûºÍÐÞ¸ÄµÄÍ¨Öª£¨Ö»ÄÜÔÚÏßÌá½»×´Ì¬£©
                                               //				 
        public fixed char subTypeStr[64];           //  ×Ô¶¨ÒåÈÎÎñÀàÐÍ. Ò»°ãÍ¨Öª, ½ô¼±Í¨Öª, ·Å¼ÙÍ¨Öª. »Ø¸´ÀàÐÍÖ¸ ÖªµÀ¡£¾Ü¾ø¡£µÈ´ý¡£¡£¡£
                                                    //
        public fixed char subject[Consts.cntof_subject];         //  Ö÷Ìâ
        public fixed char content[Consts.cntof_content];         //  ÏêÏ¸ÄÚÈÝ£¬ÒÔºó½«Öð½¥¿¼ÂÇÈçºÎ½«Êý¾ÝÓÐÐ§µÄ·ÖÉ¢µ½Èô¸É¸ö±íÖÐ×Ö¶ÎµÄÎÄ¼þ¡£ÒòÎª²»Í¬µÄÊý¾Ý¿âµÄ×Ö¶Î³¤¶È²»Í¬¡£²»¹ý£¬·Ö¶Î´æ´¢Ò²»áÔì³É²éÑ¯²»·½±ã¡£
        public fixed char txtContent[1];        //  Ô­Êý¾Ý°üÄÚÈÝ
                                                //
        public int ibNeedReply;            //  ÊÇ·ñÐèÒª»Ø¸´
        public Int64 tEndTime;                //  ½ØÖ¹Ê±¼ä
                                              //
        public fixed char issuer[32];               //  ·¢²¼ÈË
        public Int64 tIssueTime;              //  ·¢²¼Ê±¼ä

        //
        public Int64 tStartTime_req;          //  ¶ÔÏìÓ¦Ê±µÄÇëÇó°üµÄ¶ÔÓ¦Öµ
        public uint uiTranNo_req;
        //
        public Int64 firstTime;
        public Int64 lastTime;
        public int iStatus;                //  Ö´ÐÐµÄ×´Ì¬£¬±ÈÈç£ºÎ´ÔÄ£¬ÒÑÔÄ£¬ÒÑ»Ø¸´
        public Int64 tLastModifiedTime;       //  ×îºóÖ´ÐÐ¶¯×÷µÄ·¢ÉúÊ±¼ä
                                              //
        public QY_MESSENGER_ID idInfo_auditor;         //  ÉóºËÈË
        public QY_MESSENGER_ID idInfo_replyTo;         //  Çë»Ø¸´ÖÁ
                                                       //
                                                       //int iProps[10];
                                                       //fixed char propStrs[10][32];			
        public int iProp0;
        public fixed char propStr0[Consts.cntof_propStr];
        //

    }
    ;



    public class CIM_TASK_RCD
    {
        public IM_TASK_RCD imTaskRcd;
    };


















    //
    public unsafe struct IM_L_STREAM
    {                                   //  2008/04/01
        public uint uiType;
        public uint uiContentType;
        public uint ulStreamLen;
        public fixed byte buf[Consts.bufSize_imLStream + 1];
    }
        ;

    //  2010/07/10
    public unsafe struct IM_L_STREAM_nh
    {                                   //  2008/04/01
        public uint uiType;
        public uint uiContentType;
        public uint ulStreamLen;
        public fixed byte buf[1];
        public byte* p;
    }
        ;


    public struct QIS_INTERVAL_PARAMS
    {                                       //  2005/06/10
                                            //  uint										uiType;

        //  unsigned  short								usGetCfgInterval;

        public ushort usIntervalInS_refreshRecentFriends;
        public ushort usIntervalInS_refreshContactList;
        //				
    }
    ;


    //  2011/01/30
    public struct QIS_getCfgs_req
    {
        public uint uiType;

        public byte ucbResp;

        public uint uiSizePerSnd_media;

        public QIS_INTERVAL_PARAMS intervalParams;

        //  2015/07/28
        //unsigned  char										ucbNotUseP2pCall;

    }
        ;


    //  ¸
    //  
    //  1 ucbRetrieveImGrpList 
    //  2 RETRIEVE_IMGRPLIST_RESP
    //  3.ucbRetrieveImGrpList FALSE
    //  4 RETRIEVE_CONTACTLIST_RESP
    //	5 ucbRetrieveImGrpList FALSE
    //  6 RETRIEVE_CONTACTLIST_RESP
    //
    public struct RETRIEVE_IMOBJLIST_REQ
    {
        public uint uiType;
        public ushort usMaxContactUpdateInfosPerSnd;               //  Ò»´Î¿ÉÒÔÏÂÔØ×î¶à¶àÉÙ¸öÁªÏµÈË¸üÐÂÐÅÏ¢
        public Int64 tLastRefreshedTime_contactList_misServ;     //  ÉÏ´ÎµÄË¢ÐÂÊ±¼ä£¬Îª0Ê±£¬±íÃ÷È«ÃæË¢ÐÂ
                                                                 //
        public byte ucbNext;                                  //  ÊÇ·ñ¼ÌÐøÇëÇó
        public QY_MESSENGER_ID idInfo_grp_lastRefreshed;                   //  ÉÏ´ÎË¢ÐÂµÄ×éID£¬½öÓÃÔÚË¢ÐÂ³ÉÔ±¹ØÏµ¡£
        public QY_MESSENGER_ID idInfo_lastRefreshed;                       //  ÉÏ´ÎµÄË¢ÐÂ½Úµã
    }
    ;



    //  
    //

    //
    public unsafe struct RETRIEVE_IMGRPLIST_RESP
    {
        public uint uiType;
        //
        public QY_MESSENGER_ID idInfo_lastRefreshed;                       //  ¶¼ÓÐ
                                                                           //
        public byte ucbEnd;                                       //  ÊÇ·ñÏìÓ¦Êý¾ÝÒÑ¾­½áÊø¡£

        public ushort usCnt;                                       //  ·Ç½áÊø°ü
                                                                   //IM_GRP_INFO mems[Consts.CONST_maxContactUpdateInfos_grp];          //  ·Ç½áÊø°ü	
        public fixed byte buf_imGrpInfo_mems[Consts.bufSize_imGrpInfo_mems];

        //
        void* pInternal;                                    //  ÒÔÏÂÎªÄÚ²¿±äÁ¿£¬·ÇÕýÊ½Ê¹ÓÃ
        public int index;

    }
    ;


    public unsafe struct RETRIEVE_IMGRPMEMLIST_RESP
    {
        public uint uiType;
        //
        public QY_MESSENGER_ID idInfo_grp_lastRefreshed;                   //  ÒªË¢ÐÂµÄ×éID, Îª0Ê±±íË¢ÐÂËùÊô×éÐÅÏ¢¡£»òÕß£¬ÔÚ¹ÜÀí¶ËÃ»Æô¶¯×éÄ£Ê½Ê±£¬ÔòÏÂÔØËùÓÐ³ÉÔ±
        public QY_MESSENGER_ID idInfo_lastRefreshed;                       //  ¶¼ÓÐ
                                                                           //
        public byte ucbEnd;                                       //  ÊÇ·ñÏìÓ¦Êý¾ÝÒÑ¾­½áÊø¡£

        public ushort usCnt;                                       //  ·Ç½áÊø°ü
                                                                   //IM_GRP_MEM mems[CONST_maxContactUpdateInfos_grpMem];            //  ·Ç½áÊø°ü	
        public fixed byte buf_imGrpMem_mems[Consts.bufSize_imGrpMem_mems];

        void* pInternal;                                    //  ÒÔÏÂÎªÄÚ²¿±äÁ¿£¬·ÇÕýÊ½Ê¹ÓÃ
        public int index;

    }
        ;


    //  2017/08/14
    public struct RETRIEVE_contact_mem
    {
        public QY_MESSENGER_REGINFO regInfo;
        public int iRole;
    }
        ;


    //
    public unsafe struct RETRIEVE_CONTACTLIST_RESP
    {
        public uint uiType;
        //
        public QY_MESSENGER_ID idInfo_lastRefreshed;                       //  ¶¼ÓÐ
                                                                           //
        public byte ucbEnd;                                       //  ÊÇ·ñÏìÓ¦Êý¾ÝÒÑ¾­½áÊø¡£

        public ushort usCnt;                                       //  ·Ç½áÊø°ü
                                                                   //RETRIEVE_contact_mem mems[CONST_maxContactUpdateInfos_contact];         //  ·Ç½áÊø°ü	
        public fixed byte buf_retrieveContactMem_mems[Consts.bufSize_retrieveContaceMem_mems];

        //
        void* pInternal;            //  ÒÔÏÂÎªÄÚ²¿±äÁ¿£¬·ÇÕýÊ½Ê¹ÓÃ
        public int index;
        public int iFiledIndex;

    }
        ;


    public struct RETRIEVE_IMOBJLISTEND_RESP
    {                                           //  ½áÊø°ü
        public uint uiType;
        //
        public Int64 tLastRefreshedTime_contactList_misServ;     //  ·¢ËÍ¸ø¿Í»§¶ËÈÃÖªµÀÊÇ´ÓÊ²Ã´Ê±¼ä¿ªÊ¼Ë¢ÐÂµÄ
                                                                 //
    }
        ;




    [StructLayout(LayoutKind.Explicit)]

    public struct IM_CONTENTU
    {                                   //  ÄÚ²¿´¦ÀíÊ±£¬¶¼²ÉÓÃwcharÀàÐÍ£¬·¢ËÍÒÔutf8·¢ËÍ
        [FieldOffset(0)] public uint uiType;                               //  ´ËÀàÐÍ¿ÉÒÔ¶à¸öÖµ£¬Çø·Öhtml,txtµÈ¸ñÊ½

        [FieldOffset(0)] public IM_SIMPLE_RESP simpleResp;
        //  IM_STREAM_CONTENT									stream;							//  2011/01/29
        //[FieldOffset(0)] public IM_DATA_CONTENT data;
        [FieldOffset(0)] public IM_L_STREAM lStream;
        //
        [FieldOffset(0)] public QIS_getCfgs_req getCfgsReq;                         //  2011/01/30

        [FieldOffset(0)] public RETRIEVE_IMOBJLIST_REQ retrieveImObjListReq;
        [FieldOffset(0)] public RETRIEVE_IMGRPLIST_RESP retrieveImGrpListResp;
        [FieldOffset(0)] public RETRIEVE_IMGRPMEMLIST_RESP retrieveImGrpMemListResp;
        [FieldOffset(0)] public RETRIEVE_CONTACTLIST_RESP retrieveContactListResp;
        [FieldOffset(0)] public RETRIEVE_IMOBJLISTEND_RESP retrieveImObjListEndResp;


        [FieldOffset(0)] public CONF_req confReq;
        //MCU_cmd mcuCmd;

        //IM_GRP_EX imGrpEx;

        [FieldOffset(0)] public TRANSFER_FILE_REQ transferFileReq;
        [FieldOffset(0)] public TRANSFER_FILE_REPLYREQ transferFileReplyReq;                //  2007/07/06
        [FieldOffset(0)] public TRANSFER_FILEDATA_REQ transferFileDataReq;
        [FieldOffset(0)] public TRANSFER_FILEDATA_RESP transferFileDataResp;
        [FieldOffset(0)] public TRANSFER_FILEEND_REQ transferFileEndReq;
        [FieldOffset(0)] public TRANSFER_FILEEND_RESP transferFileEndResp;

        //
        [FieldOffset(0)] public TASK_PROC_REQ taskProcReq;
        [FieldOffset(0)] public TASK_PROC_RESP taskProcResp;
        [FieldOffset(0)] public PTZ_control_cmd ptzControlCmd;
        //
        /*
        //
        REFRESH_RECENTFRIENDS_REQ refreshRecentFriendsReq;          //  2007/07/12
        REFRESH_RECENTFRIENDS_RESP refreshRecentFriendsResp;
        //
        RTC_CALL_REQ rtcCallReq;                            //  2007/10/07
        */
        //
        [FieldOffset(0)] public TRANSFER_AV_INFO transferAvInfo;                        //  2008/03/15
        [FieldOffset(0)] public TRANSFER_AV_replyInfo transferAvReplyInfo;

        [FieldOffset(0)] public TRANSFER_VIDEO_DATA transferVideoData;

        [FieldOffset(0)] public TRANSFER_VIDEO_dataResp transferVideoDataResp;

        [FieldOffset(0)] public TRANSFER_AUDIO_DATA transferAudioData;                  //  2008/04/16

        [FieldOffset(0)] public TRANSFER_AUDIO_dataResp transferAudioDataResp;              //  2008/04/16
        /*
																//
	REMOTE_ASSIST_REQ remoteAssistReq;                  //  2008/11/15
		*/
        //
        [FieldOffset(0)] public TASK_INTERACTION_REQ taskInteractionReq;                    //  2009/02/23

        [FieldOffset(0)] public CONF_KEY confKey;
        [FieldOffset(0)] public CONF_LAYOUT confLayout;
        [FieldOffset(0)] public ConfNvrInfo confNvrInfo;
        [FieldOffset(0)] public StatusConfLayoutParam statusConfLayoutParam;
        //  2010/12/27
        /*
	CONF_state confState;                           //  2017/09/17
													//
	TRANSFER_GPS_INFO transferGpsInfo;                  //  2012/04/19
	TRANSFER_GPS_replyInfo transferGpsReplyInfo;                //  2012/04/19
	TRANSFER_GPS_DATA transferGpsData;                  //  2012/04/20
	TRANSFER_GPS_dataResp transferGpsDataResp;              //  2012/04/20
															//
															//  stream
															//
		*/
        [FieldOffset(0)] public IM_HTML_CONTENT html;

        //
        //  QIS_INTERVAL_PARAMS							intervalParams;						//  2007/12/01

        /*											//
	//
	//
	RETRIEVE_customerServiceObjList_req retrieveCustomerServiceObjListReq;  //  2011/04/04
	QUERY_customerServiceOfficer_req queryCustomerServiceOfficerReq;        //  2011/04/06
	QUERY_customerServiceOfficer_replyReq queryCustomerServiceOfficerReplyReq;//  2011/04/06
																			  //
	RETRIEVE_WEBCONTACTLIST_RESP retrieveWebContactListResp;
	REFRESH_WEBCONTACTS_INFO refreshWebContactsInfo;                //  2007/12/22
																	//
	QY_VDEV_COMS vDevComs;
	RETRIEVE_SM_POLICY retrieveSmPolicy;
	RETRIEVE_PHONE_MSGRS retrievePhoneMsgrs;                    //  2008/01/02
	RETRIEVE_TO_PATHS retrieveToPaths;                  //  2008/02/09
														//
	REFRESH_DYN_BMPS refreshDynBmps;                        //  2009/08/24
	VERIFY_viewDynBmp verifyViewDynBmp;                 //  2011/10/31
	RETRIEVE_DYN_BMPS retrieveDynBmps;                  //  2009/09/04
	REFRESH_imObjRules_req refreshImObjRulesReq;                //  2010/08/15
																//
	PROC_offlineRes_u procOfflineResU;                  //  2015/08/01


	//
	PTZ_control_req ptzControlReq;                      //  2012/08/05


	//
	QUERY_peer_status queryPeerStatus;                  //  2014/07/11

	//
	QY_VDEV_DATA vDevData;

	//
	GET_daemonStatus_req getDaemonStatusReq;                    //  2011/01/08

		*/
    }
;



    //
    public class CIM_CONTENTU
    {
        public IM_CONTENTU imContentU;

    };



    //
    [StructLayout(LayoutKind.Explicit)]

    public struct IM_CONTENTU_nh
    {                                   //  2010/07/29
        [FieldOffset(0)] public uint uiType;                             //  
        [FieldOffset(0)] public IM_SIMPLE_RESP simpleResp;
        [FieldOffset(0)] public IM_L_STREAM_nh lStream;
    };



    //////////////////////////////////////////
    ///

    public unsafe struct QIS_EVENT
    {
        int id;
        int lHelpRand;                                     //	2003/11/15
                                                           //
        fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        QY_MESSENGER_ID idInfo;
        //				 
        fixed byte detectedIp[15 + 1];                            //  Í¨ÐÅ¼ì²âµ½µÄ¿Í»§¶ËIP
        fixed byte ip[15 + 1];                                    //  ip must be filled before qnmLogPcEvent
        int lEventType;                                        //  eventType must be filled before qnmLogPcEvent
        int iCustomId;                                      //  iCustomId must be filled before qnmLogPcEvent
        fixed byte localEventTime[14 + 1];                                    //  localEventTime must be filled before qnmLogPcEvent
        fixed byte localCntEventTime[14 + 1];                             //  ´«Èë¹ÜÀíÖÐÐÄµÄ±¾µØµÄÊ±¼ä
        fixed byte cntEventTime[14 + 1];                                  //  ´«Èë¹ÜÀíÖÐÐÄµÄ¹ÜÀíÖÐÐÄµÄÊ±¼ä
        fixed char comment[255 + 1];
        //char cols[CONST_qnmMaxEventRegFields][255  +  1];	//  ÊÂ¼þ´¥·¢ÕßµÄÐÅÏ¢10ÁÐ   
        fixed char usrName[255 + 1];
        //
        //TCHAR deses[CONST_qnmMaxEventDesFields][255  +  1];	//  ÊÂ¼þÃèÊö±¾ÉíµÄÐÅÏ¢10ÁÐ				 
        fixed char dsnName[128 + 1];                                //  2007/01/02
        int iStatus;                                        //  2006/06/02, Ôö¼Ó¶ÔÊÂ¼þµÄ´¦Àí×´Ì¬µÄÃèÊö
    }
    ;




    /// <summary>
    /// /////////////////////////////////////////////
    /// </summary>

    public unsafe struct MIS_MSG_TALKDATA
    {
        public MSG_SIMPLE_ROUTE route; //  TALKDATA_ADDR									addr;
                                       //
                                       //fixed char buf[sizeof(IM_CONTENTU) + 1024];   //  ×¢Òâ£¬ÔÚ·ÅÈë¶ÓÁÐÀï£¬²»ÒªÓÃsizeof(  data  ),  Ó¦¸ÃÊ¹ÓÃoffsetof(  ,  data  )  +  dataLen, ÒÔ½ÚÔ¼ÄÚ´æ
        public fixed byte buf[Consts.bufSize_talkData];
    }
    ;

    public unsafe struct MIS_MSG_routeTalkData
    {
        public MSG_ROUTE route;
        //
        //char buf[sizeof(IM_CONTENTU) + 1024];
        //public fixed byte buf[Consts.bufSize_routeTalkData];
        public IM_CONTENTU buf;
    }
    ;





    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>


    public struct MIS_MSG_TALK
    {                                   //  2007/11/29, addressable talk
        public uint uiType;
        //
        public uint uiCliIndex;                           //  input daemon
                                                          //  2008/01/05, vDev_coms uiCliIndex

        public uint uiSessionId;
        public uint uiStep;                                //  2008/04/28

        public bool ucbConvrted;                      //  input 2007/7/14

        public bool ucbConvrted_msgRoute;             //  daemon input 
        public ushort lenInBytes_msgRouteInDataBuf;        //  daemon input data.route route data.buf
                                                           //  lenInBytes_msgRouteInDataBuf data.buf route

        //  mc MisCnt qmd uiCliIndex, 2007/06/10
        //void* pMisCnt;                          //  misCntµÄ, 2007/06/02
        public MSGR_ADDR addr_logicalPeer;                 //  messenger. 

        //
        public byte ucFlg;
        //
        public ushort usCode;
        public ushort usCode_org;                          //  
        public Int64 tStartTime;
        public uint uiTranNo;
        public ushort usSeqNo;

        public Int64 tStartTime_processed;                //  iocp uiTranNo_mgr iocp
        public uint uiTranNo_processed;
        public uint nTimes_tryToSend;                  //  mgr iocp 
                                                       //
        public Int64 tTime_alive;                     //  2014/12/09. 当没有找到新的视频数据时，刷新此变量，也可以表明任务是活跃的
                                                      //
        public Int64 tTime_applyForChkTaskAlive;          //  2010/10/30
        public byte nTimes_applyForChkTaskAlive;      //  2014/12/09

        //
        public uint uiTaskMode_unused;                 //  2009/08/23

        //
        public int iTaskId;                            //  
        public uint uiTaskType;                            //  
        public QY_MESSENGER_ID idInfo_taskSender;                  //  
        public QY_MESSENGER_ID idInfo_taskReceiver;                //  
        public int iStatus;                            //  2007/07/18
        public Int64 tLastModifiedTime;                   //  2014/05/01

        //  
        public Int64 tRecvTime;                           //  
        public uint dwTickCnt_start;                  //  ( ms ). 
        public uint dwTickCnt_toSend;                 //  2011/01/22. start->toSendQ
                                                      //  
        public ushort usElapsed_recved;                    //  
        public ushort usElapsed_serv_startToProcess;       //  mtMgrThreadProc_is 
        public int index_debugInfo;                    //  debugInfo index, debugInfo 2008/11/19

        //
        public uint uiChannelType;                     //  2008/04/17, 
                                                       //
        public uint uiChannelType_input;               //  2017/10/03

        //
        public uint lenInBytes;
        //
        public MIS_MSG_routeTalkData data;                             //  sizeof(  data  ),  offsetof(  ,  data  )  +  offsetof(  MIS_MSG_TALKDATA,  buf  )  +  bufLen
    };
    //MIS_MSG_TALK, MIS_MSG_INPUT, MIS_MSG_TASK, MIS_MSG_REQ;	

    public struct MIS_MSG_INPUT
    {                                   //  2007/11/29, addressable talk
        public uint uiType;
        //
        public uint uiCliIndex;                           //  input daemon
                                                          //  2008/01/05, vDev_coms uiCliIndex

        public uint uiSessionId;
        public uint uiStep;                                //  2008/04/28

        public bool ucbConvrted;                      //  input 2007/7/14

        public bool ucbConvrted_msgRoute;             //  daemon input 
        public ushort lenInBytes_msgRouteInDataBuf;        //  daemon input data.route route data.buf
                                                           //  lenInBytes_msgRouteInDataBuf data.buf route

        //  mc MisCnt qmd uiCliIndex, 2007/06/10
        //void* pMisCnt;                          //  misCntµÄ, 2007/06/02
        public MSGR_ADDR addr_logicalPeer;                 //  messenger. 

        //
        public byte ucFlg;
        //
        public ushort usCode;
        public ushort usCode_org;                          //  
        public Int64 tStartTime;
        public uint uiTranNo;
        public ushort usSeqNo;

        public Int64 tStartTime_processed;                //  iocp uiTranNo_mgr iocp
        public uint uiTranNo_processed;
        public uint nTimes_tryToSend;                  //  mgr iocp 
                                                       //
        public Int64 tTime_alive;                     //  2014/12/09. 当没有找到新的视频数据时，刷新此变量，也可以表明任务是活跃的
                                                      //
        public Int64 tTime_applyForChkTaskAlive;          //  2010/10/30
        public byte nTimes_applyForChkTaskAlive;      //  2014/12/09

        //
        public uint uiTaskMode_unused;                 //  2009/08/23

        //
        public int iTaskId;                            //  
        public uint uiTaskType;                            //  
        public QY_MESSENGER_ID idInfo_taskSender;                  //  
        public QY_MESSENGER_ID idInfo_taskReceiver;                //  
        public int iStatus;                            //  2007/07/18
        public Int64 tLastModifiedTime;                   //  2014/05/01

        //  
        public Int64 tRecvTime;                           //  
        public long dwTickCnt_start1;                  //  ( ms ). 
        public uint dwTickCnt_toSend;                 //  2011/01/22. start->toSendQ
                                                      //  
        public ushort usElapsed_recved;                    //  
        public ushort usElapsed_serv_startToProcess;       //  mtMgrThreadProc_is 
        public int index_debugInfo;                    //  debugInfo index, debugInfo 2008/11/19

        //
        public uint uiChannelType;                     //  2008/04/17, 
                                                       //
        public uint uiChannelType_input;               //  2017/10/03

        //
        public uint lenInBytes;
        //
        public MIS_MSG_routeTalkData data;                             //  sizeof(  data  ),  offsetof(  ,  data  )  +  offsetof(  MIS_MSG_TALKDATA,  buf  )  +  bufLen
    };


    public struct MIS_MSG_TASK
    {                                   //  2007/11/29, addressable talk
        public uint uiType;
        //
        public uint uiCliIndex;                           //  input daemon
                                                          //  2008/01/05, vDev_coms uiCliIndex

        public uint uiSessionId;
        public uint uiStep;                                //  2008/04/28

        public bool ucbConvrted;                      //  input 2007/7/14

        public bool ucbConvrted_msgRoute;             //  daemon input 
        public ushort lenInBytes_msgRouteInDataBuf;        //  daemon input data.route route data.buf
                                                           //  lenInBytes_msgRouteInDataBuf data.buf route

        //  mc MisCnt qmd uiCliIndex, 2007/06/10
        //void* pMisCnt;                          //  misCntµÄ, 2007/06/02
        public MSGR_ADDR addr_logicalPeer;                 //  messenger. 

        //
        public byte ucFlg;
        //
        public ushort usCode;
        public ushort usCode_org;                          //  
        public Int64 tStartTime;
        public uint uiTranNo;
        public ushort usSeqNo;

        public Int64 tStartTime_processed;                //  iocp uiTranNo_mgr iocp
        public uint uiTranNo_processed;
        public uint nTimes_tryToSend;                  //  mgr iocp 
                                                       //
        public Int64 tTime_alive;                     //  2014/12/09. 当没有找到新的视频数据时，刷新此变量，也可以表明任务是活跃的
                                                      //
        public Int64 tTime_applyForChkTaskAlive;          //  2010/10/30
        public byte nTimes_applyForChkTaskAlive;      //  2014/12/09

        //
        public uint uiTaskMode_unused;                 //  2009/08/23

        //
        public int iTaskId;                            //  
        public uint uiTaskType;                            //  
        public QY_MESSENGER_ID idInfo_taskSender;                  //  
        public QY_MESSENGER_ID idInfo_taskReceiver;                //  
        public int iStatus;                            //  2007/07/18
        public Int64 tLastModifiedTime;                   //  2014/05/01

        //
        public bool bFrom_sharedObj;
        public int index_sharedObj;

        //  
        public Int64 tRecvTime;                           //  
        public long dwTickCnt_start1;                  //  ( ms ). 
        public uint dwTickCnt_toSend;                 //  2011/01/22. start->toSendQ
                                                      //  
        public ushort usElapsed_recved;                    //  
        public ushort usElapsed_serv_startToProcess;       //  mtMgrThreadProc_is 
        public int index_debugInfo;                    //  debugInfo index, debugInfo 2008/11/19

        //
        public uint uiChannelType;                     //  2008/04/17, 
                                                       //
        public uint uiChannelType_input;               //  2017/10/03

        //
        public uint lenInBytes;
        //
        public MIS_MSG_routeTalkData data;                             //  sizeof(  data  ),  offsetof(  ,  data  )  +  offsetof(  MIS_MSG_TALKDATA,  buf  )  +  bufLen
    };



    public struct MIS_MSG_REQ
    {                                   //  2007/11/29, addressable talk
        public uint uiType;
        //
        public uint uiCliIndex;                           //  input daemon
                                                          //  2008/01/05, vDev_coms uiCliIndex

        public uint uiSessionId;
        public uint uiStep;                                //  2008/04/28

        public bool ucbConvrted;                      //  input 2007/7/14

        public bool ucbConvrted_msgRoute;             //  daemon input 
        public ushort lenInBytes_msgRouteInDataBuf;        //  daemon input data.route route data.buf
                                                           //  lenInBytes_msgRouteInDataBuf data.buf route

        //  mc MisCnt qmd uiCliIndex, 2007/06/10
        //void* pMisCnt;                          //  misCntµÄ, 2007/06/02
        public MSGR_ADDR addr_logicalPeer;                 //  messenger. 

        //
        public byte ucFlg;
        //
        public ushort usCode;
        public ushort usCode_org;                          //  
        public Int64 tStartTime;
        public uint uiTranNo;
        public ushort usSeqNo;

        public Int64 tStartTime_processed;                //  iocp uiTranNo_mgr iocp
        public uint uiTranNo_processed;
        public uint nTimes_tryToSend;                  //  mgr iocp 
                                                       //
        public Int64 tTime_alive;                     //  2014/12/09. 当没有找到新的视频数据时，刷新此变量，也可以表明任务是活跃的
                                                      //
        public Int64 tTime_applyForChkTaskAlive;          //  2010/10/30
        public byte nTimes_applyForChkTaskAlive;      //  2014/12/09

        //
        public uint uiTaskMode_unused;                 //  2009/08/23

        //
        public int iTaskId;                            //  
        public uint uiTaskType;                            //  
        public QY_MESSENGER_ID idInfo_taskSender;                  //  
        public QY_MESSENGER_ID idInfo_taskReceiver;                //  
        public int iStatus;                            //  2007/07/18
        public Int64 tLastModifiedTime;                   //  2014/05/01

        //  
        public Int64 tRecvTime;                           //  
        public uint dwTickCnt_start;                  //  ( ms ). 
        public uint dwTickCnt_toSend;                 //  2011/01/22. start->toSendQ
                                                      //  
        public ushort usElapsed_recved;                    //  
        public ushort usElapsed_serv_startToProcess;       //  mtMgrThreadProc_is 
        public int index_debugInfo;                    //  debugInfo index, debugInfo 2008/11/19

        //
        public uint uiChannelType;                     //  2008/04/17, 
                                                       //
        public uint uiChannelType_input;               //  2017/10/03

        //
        public uint lenInBytes;
        //
        //public MIS_MSG_routeTalkData data;                             //  sizeof(  data  ),  offsetof(  ,  data  )  +  offsetof(  MIS_MSG_TALKDATA,  buf  )  +  bufLen
        public IM_CONTENTU data;                             //  sizeof(  data  ),  offsetof(  ,  data  )  +  offsetof(  MIS_MSG_TALKDATA,  buf  )  +  bufLen
    };



    public struct MIS_MSG_procVideo_qmc
    {                                   //  2008/03/18
                                        //MACRO_misMsg_common
        public uint uiType;
        //
        //		void* pMisCnt;
        public QY_MESSENGER_ID idInfo_peer;
        //
        public QY_MESSENGER_ID idInfo_sender;
        //
        //
        public uint uiTranNo_openAvDev;
        public ushort usIndex;
        //
        public int iTaskId;
        public IntPtr hWnd_task;                         //  2009/09/14
                                                         //
        public uint uiEventId_lastRecvd_unused;            //  2016/12/26	//  2008/11/22

        //
        public uint uiCapType;
        public int iIndex_capProcInfo;
        public BITMAPINFOHEADER bih;

        //
        //QY_MEMORY									memory;

        //  2014/09/23
        public myDRAW_VIDEO_DATA pkt;

        //
        //  int										index_player;						//  2009/10/03
        public PLAYER_ID playerId;                         //  2016/02/18

        //

        //
        public bool ucbDisplayAllFrames;              //  2009/07/25

        //

        //  

    };


    public unsafe struct myTRANSFER_FILE_STATUS
    {                                   //  robotÓÃÀ´¸ø½çÃæ·¢ÏûÏ¢£¬ÈÃ½ØÃæÏÔÊ¾ÎÄ¼þ·¢ËÍµÈ¹ý³Ì, 2007/07/17
        public byte ucPercent;
        public UInt64 ui64Len_recvd;                        //  ÊÕµ½µÄ×Ö½ÚÊý¡£ 2008/10/25
        public fixed char fileName[Consts.cntof_fileName];
        public fixed char fullFilePath[Consts.MAX_PATH + 1];
    }
    ;

    public unsafe struct myNOTIFICATION
    {                                   //  2010/07/21
        public QY_MESSENGER_ID idInfo_from;
        public fixed char content[Consts.cntof_content];
        //public char tBuf128[Consts.cntof_tBuf128];
    }
        ;


    public unsafe struct MIS_MSG_taskStatus
    {                                   //  2007/07/17
                                        //MACRO_misMsg_common
        public uint uiType;
        //
        //void* pMisCnt;
        public QY_MESSENGER_ID idInfo_logicalPeer;
        //
        public QY_MESSENGER_ID idInfo_taskSender;
        public QY_MESSENGER_ID idInfo_taskReceiver;
        //
        public bool bImGrpMem;

        public ushort cmdCode;
        public Int64 tStartTime;                          //  2009/10/18
        public uint uiTranNo;                          //  2009/10/18
        public uint uiContentType;
        public int iTaskId;
        public uint uiTaskType;                            //  2007/07/22
        public int iStatus;

        //
        public Int64 tStartTime_processed;                //  2011/03/10

        //
        public bool ucbNeeded_applyForOrgReq;         //  2010/07/21
                                                      //
        public bool ucbFound;                         //  2014/11/21
                                                      //
        public bool ucbNotification;                  //  2010/07/21

        //
        public myTRANSFER_FILE_STATUS u_transferFileStatus;
        public myNOTIFICATION u_notification;                        //  2010/07/21

    }


    //
    public unsafe struct MIS_MSG_REFRESHMESTATUS_QMC
    {                               //  
                                    //MACRO_misMsg_common
        public uint uiType;
        //
        //HWND                                           hWnd;
        //		 void* pMisCnt;
        //  
    }
    ;


    //
    public unsafe struct IM_MSG_RCD
    {

        public int id;

        public fixed char misServName[Consts.CONST_maxMisServNameLen + 1];
        public QY_MESSENGER_ID idInfo_send;
        //TCHAR senderDesc[255 + 1];
        //
        public Int64 tSendTime;
        public uint uiTranNo;
        public ushort usSeqNo;

        //
        public QY_MESSENGER_ID idInfo_logicalPeer;

        //
        public int iTaskId;

        //	
        public Int64 tStartTime_serv;
        public uint uiSerialNo;
        //
        public QY_MESSENGER_ID idInfo_recv;
        //TCHAR receiverDesc[255 + 1];
        public int iRole;
        //										
        public Int64 tRecvTime;
        public ushort usRespCode;
        //
        public uint uiType;
        //										
        public fixed char logMsgContent[Consts.cntof_logMsgContent];
        //TCHAR txtContent[2048];
        //
        public Int64 firstTime;
        public Int64 lastTime;
        public int iStatus;
        public Int64 tLastModifiedTime;

    }
    ;




    //
#if false
	public struct MIS_MSG_imMsgRcd
	{                       //  2007/08/12
							//MACRO_misMsg_common
		public uint uiType;
		//
		public IM_MSG_RCD                                     rcd;
}
	;
#endif


    //
#if false
	typedef struct __misMsgImEvent_t
	{                       //  2007/09/13
		MACRO_misMsg_common
		//
		QIS_EVENT                                      event;
		}
		MIS_MSG_EVENT;
#endif

    //
    public unsafe struct MIS_MSG_applyForPlayer_qmc
    {
        //MACRO_misMsg_common
        public uint uiType;
        //
        public QY_MESSENGER_ID idInfo_recorder;
        public int iTaskId;                       //  2009/09/17

        //
        public QY_MESSENGER_ID idInfo_logicalPeer;

        //
        public int index_sharedObj;

        //
        public bool bGps;                          //  2012/05/07

        //
        public struct MIS_MSG_applyForPlayer_qmc_audio
        {
            public uint uiTranNo;                     //  2009/04/27
                                                      //  unsigned  short						usIndex;						//  2009/04/27
                                                      //
                                                      //  WAVEFORMATEX							wf;
            public QY_AUDIO_HEADER ah;                             //  2010/06/17

            public AUDIO_PLAY_CFG playCfg;                     //  2009/06/14

        };
        public MIS_MSG_applyForPlayer_qmc_audio audio;
        public struct MIS_MSG_applyForPlayer_qmc_video
        {
            public uint uiTranNo;
            //  unsigned  short						usIndex;
            public QY_VIDEO_HEADER vh;
            //
            public ushort usFps;                           //  2015/02/20

        };
        public MIS_MSG_applyForPlayer_qmc_video video;


    };

    //  ;
    public struct MIS_MSG_applyForChkChannels_qmc
    {                               //  2008/06/01
                                    //MACRO_misMsg_common
        public uint uiType;
        //
        //void* pMisCnt;

        //
        bool bTaskExists;
        //  BOOL											bTaskNeedRestart;
        bool bTask_sendLocalAv;             //  2008/10/17, ´æÔÚÐèÒª·¢ËÍ±¾µØavµÄÈÎÎñ

    }
        ;




    //
    [StructLayout(LayoutKind.Explicit)]

    public struct MIS_MSGU
    {
        [FieldOffset(0)] public uint uiType;
        /*
		MIS_MSG_RESULT result;
		MIS_MSG_RESP resp;
		*/
        [FieldOffset(0)] public MIS_MSG_INPUT input;
        [FieldOffset(0)] public MIS_MSG_REQ req;
        [FieldOffset(0)] public MIS_MSG_TALK talk;
        [FieldOffset(0)] public MIS_MSG_TASK task;                      //  2007/07/08

        /*
		//
	MIS_MSG_GRP grp;                        //  2008/05/27
											//
	MIS_MSG_inputArrive inputArrive;                //  2015/08/24
	MIS_MSG_send send;                      //  2015/09/02
	MIS_MSG_sendFinished sendFinished;              //  2007/05/30
	MIS_MSG_processSelf processSelf;                //  2007/05/30
	MIS_MSG_processBt processBt;                    //  2011/04/18
	MIS_MSG_isSendOk isSendOk;
	MIS_MSG_sendOk sendOk;
	MIS_MSG_chkChannel chkChannel;                  //  2007/12/20
	MIS_MSG_applyForWork applyForWork;              //  2007/09/03
													//
	MIS_MSG_NEEDREG_QMC needReg_qmc;
		*/
        //
        [FieldOffset(0)] public MIS_MSG_taskStatus taskStatus;                  //  2007/07/17

        //[FieldOffset(0)]public MIS_MSG_imMsgRcd imMsgRcd;                  //  2007/08/12
        /*
	MIS_MSG_EVENT event;
	//
	MIS_MSG_TALKINGFRIEND_QMC talkingFriend_qmc;
	MIS_MSG_RECENTFRIEND_QMC recentFriend_qmc;
	MIS_MSG_DISPLAYEDELEM_QMC displayedElem_qmc;            //  2007/06/10
	MIS_MSG_REFRESHCONTACTSSTATUS_QMC refreshContactsStatus_qmc;
		*/
        [FieldOffset(0)] public MIS_MSG_REFRESHMESTATUS_QMC refreshMeStatus_qmc;        //  2007/06/20

        [FieldOffset(0)] public MIS_MSG_applyForPlayer_qmc applyForPlayer_qmc;          //  2008/05/09

        [FieldOffset(0)] public MIS_MSG_applyForChkChannels_qmc applyForChkChannels_qmc;    //  2008/06/01
        /*
            MIS_MSG_applyForRemovingInvalidTasks_qmc applyForRemovingInvalidTasks_qmc;      //  2009/09/10
        MIS_MSG_applyForTalkerShadow_qmc applyForTalkerShadow_qmc;  //  2012/04/23
                                                                    //
        MIS_MSG_procVideo_qmc procVideo;                    //  2009/03/24
        MIS_MSG_procGps_qmc procGps;                    //  2012/04/20
                                                        //
        MIS_MSG_notifyTaskEnd_qmc notifyTaskEnd;                //  2009/04/28
                                                                //
        MIS_MSG_getDaemonStatus getDaemonStatus;            //  2011/01/12
        */

    }
        ;



    public class CMIS_MSGU
    {
        public MIS_MSGU msgU;
    };



    public struct TASK_AV_FROM_video
    {

        //
        public TRAN_video tv_recvd;                                   //  2014/11/10

        //  2014/11/10
        public bool bGot_vh_stream;

        //
        public ushort usFrames_perSecond_expected;             //  TRANSFER_VIDEO_DATA
                                                               //
        public Int64 tLastTime_showFrameInfo_020534;
        public ushort usCnt_pkts;
        public Int64 tSelectTime;
        public uint dwTickCnt_selectTime;

    };

    public struct TASK_AV_FROM_audio
    {

        //  2014/11/10
        public TRAN_audio ta_recvd;                                   //  2014/11/10

        //
        public Int64 tLastTime_showFrameInfo_020534;
        public ushort usCnt_pkts;

    };

    public struct TASK_AV_FROM_viewDynBmp_reply
    {
        public bool ucbSaveVideo_reply;
    };


    /////////
    public class TASK_AV_FROM
    {

        //
        public QY_MESSENGER_ID idInfo;                                     //  
        public Int64 tStartTime_task;                         //  
                                                              //
        public bool ucbVideoConferenceStarter;                    //  2009/04/02

        //  2014/09/23
        public short shClientVer;

        //
        public TASK_AV_FROM_video video;

        public TASK_AV_FROM_audio audio;

        //
        //  2015/05/19. 2014/09/04
        public TASK_AV_FROM_viewDynBmp_reply viewDynBmp_reply;

        //  2015/10/23
        public TASK_av_props taskAvProps;


        //
        public unsafe struct TaskAvFrom_sth
        {
            public fixed int iDiffInMs_tStartAudios[Consts.MAX_activeMems_from];                      //  2009/05/27
            public fixed int iSampleTimeInMs_audios[Consts.MAX_activeMems_from];                        //  2010/06/19
        };
        public TaskAvFrom_sth sth;

        //
        public void copyTo(TASK_AV_FROM dst)
        {
            TASK_AV_FROM src = this;
            //
            dst.idInfo = src.idInfo;
            dst.tStartTime_task = src.tStartTime_task;
            //
            dst.ucbVideoConferenceStarter = src.ucbVideoConferenceStarter;
            //

            //  2014/09/23
            dst.shClientVer = src.shClientVer;


            //
            dst.video = src.video;
            dst.audio = src.audio;

            //
            dst.viewDynBmp_reply = src.viewDynBmp_reply;
            dst.taskAvProps = src.taskAvProps;
            dst.sth = src.sth;

            return;

        }

    };





    public struct ACTIVE_taskAvFrom
    {                                           //  2009/02/19

        public QY_MESSENGER_ID idInfo;                                     //  ·¢ËÍÕß£¨ÔÚ×é²¥Àï£¬Õâ¸öºÜÖØÒª£©

        public ushort usIndex_pMems_from;

    }
        ;

    public struct PKT_info_toDec_info
    {
        public bool bGot_pkt;
        //
        public int index_mems;
        public int iPos;
    };
    public struct PTZ_control_cmd
    {
        public int uiType;
        //
        public QY_MESSENGER_ID idInfo_imGrp_related;
        public PTZ_cmd_common cmdInfo;
    };
    public struct PTZ_cmd_common
    {
        public Obj obj;
        //
        public QY_MESSENGER_ID idInfo;
        public int iChannel;
        public byte ucCmd;
        public int ucParam;
        public int ucMouseStatus;
    };
    public struct Obj
    {

        public int uiObjType;
        public short usIndex_obj;
        public short usHelp_subIndex;
    };
    //
    public unsafe struct Tmp_pkt_encA
    {
        public TRANSFER_AUDIO_dataMemHead memHead_a;
        public int bufSize;
        public fixed byte buf[Consts.bufSize_tmpPktEncA];   //  128000/8
        public int len;
    };

    public unsafe struct Tmp_pkt_encV
    {
        public TRANSFER_VIDEO_dataMemHead memHead_v;
        public int bufSize;
        public fixed byte buf[Consts.bufSize_tmpPktEncV];   //  128000/8
        public int len;
    };

    //
    public unsafe struct PKT_info_toDec
    {
        public void* pMIS_MSGU;

        //
        public Tmp_pkt_encA pkt_encA;
        public Tmp_pkt_encV pkt_encV;

        //
        public void* pPkt_dec;                       //  用来做解码缓存

        //
        public bool bGot_msg;                                      //  2015/02/14. 表明pMIS_MSGU是否有效数据
                                                                   //
        public int lPktId_getNewPkt;
        //
        public PKT_info_toDec_info info;
    }
;

    public struct PRETRANS_procInfo_helpInfo
    {
        public struct PRETRANS_procInfo_helpInfo_pts
        {

            public uint dwTickCnt_start;         //  2015/01/15
            public uint uiPts_first;
            //
            //
            public uint uiPts_lastRecvd;           //  2015/01/15

        };
        public PRETRANS_procInfo_helpInfo_pts pts;

    };


    public unsafe class PRETRANS_procInfo : QY_qThreadProcInfo_common
    {
        //MACRO_qyQThreadProcInfo_common

        //
        public QY_qBuf2 qBuf2;

        //
        public PRETRANS_procInfo_helpInfo helpInfo;

    };





    //  2014/08/04
    public unsafe class SAVE_av_procInfo : QY_qThreadProcInfo_common
    {
        //MACRO_qyQThreadProcInfo_common

        /*

				//  2015/05/19
				struct                                         {
					 //
	QY_MESSENGER_ID idInfo;
	//uint								uiTranNo_openAvDev;

	//
	TRAN_audio ta;              //  2015/05/19					

	//
	int frame_cnt;

}
audio;

//
struct                                         {
					 //
QY_MESSENGER_ID idInfo;
//uint								uiTranNo_openAvDev;

//
ushort usMaxFps_toShareBmp;
//
TRAN_video tv;

//
DWORD dwLastTickCnt_notification;

//
int frame_cnt;
					 
				 }												video;

//
FILE* fp;


//  2014/09/07
struct                                         {
					 DWORD dwLastTickCnt_showInfo_notSave;
//
unsigned char ucbSave_lastVal;
				 }												status;
		*/

    };

    //
    public struct TRANS_pts_info_v
    {

        //
        public uint dwTickCnt_calc_start;                        //  每隔几秒计算一次。每次都重新清零。

        //
        public int uiCnt_ptsErr;                              //  pts不匹配的次数
        public int uiCnt_notRecvd_pts;                            //  多少pts丢了

    };

    public unsafe struct TRANS_pts_info
    {
        public TRANS_pts_info_v v;
    }
;

    //  2009/06/17
    public unsafe class QIS_DISPATCH_procInfo : QY_qThreadProcInfo_common
    {
        //MACRO_qyQThreadProcInfo_common
    }
        ;

    public unsafe class QIS_DISPATCH
    {
        public void* pProcInfo;
        public MSGR_ADDR* pAddr_logicalPeer;
        public int iTaskId;
        public void* pTranInfo;                                        //  AV_TRAN_INFO  *
        public void* pTask;                                            //  PROC_TASK_AV  *

        //  2011/12/08
        public int index_activeMems_from;

        //
        public QIS_DISPATCH_procInfo audio;
        public QIS_DISPATCH_procInfo video;

    }
        ;


    public unsafe struct VIDEO_CONFERENCE_MEM
    {
        public AV_stream avStream;

        //  2013/07/18. 需要将第几个话筒的属性传给其他用户
        public ushort usIndex_speaker;


        //
        public void* pInternal;

        public fixed char desc[32];
    }
        ;


    //
    public unsafe struct CONF_requesting_mem
    {
        //
        public QY_MESSENGER_ID idInfo;

        //
        public void* pInternal;

        public fixed char desc[32];

    }
    ;

    //
    public unsafe struct CONF_requesting_mems
    {

        public ushort usCnt;
        //CONF_requesting_mem mems[MAX_requestingMems_videoConference];
        public fixed byte buf_CONF_requesting_mems1[Consts.bufSize_CONF_requesting_mems];


    }
        ;



    public unsafe struct CONF_KEY
    {
        public uint uiType;

        public QY_MESSENGER_ID idInfo_imGrp_related;               //  2009/02/23

        //
        public struct ConfKey_tmpInternal
        {
            public ushort usCnt_mems;
        };
        public ConfKey_tmpInternal tmpInternal;

        //
        public ushort usLayoutType_starter;               //  2010/12/29

        //
        public ushort usCnt;
        //VIDEO_CONFERENCE_MEM mems[MAX_activeMems_taskAv + MAX_requestingMems_videoConference];
        public fixed byte buf_VIDEO_CONFERENCE_MEM_mems[Consts.bufSize_videoConferenceMem_mems];

        //
        public CONF_requesting_mems requestingMems;
        public ConfLayoutParam confLayoutParam;

    }
    ;


    //
    public struct StatusConfLayoutParam
    {
        public uint uiType;
        public QY_MESSENGER_ID idInfo_imGrp_related;

        //
        public ConfLayoutParam confLayoutParam;
    }
    ;



    public struct ConfLayoutParam
    {
        //
        public OneBigLayoutParam oneBigLayoutParam;

        //
        public EnlargeParam enlargeParam;

        //
        public IpcParam ipcParam;

        //
        public PhoneEnlargeParam phoneEnlargeParam;
        public Resp resp;

        //
        public RespVideoStatus respVideoStatus;

    };
    public struct ConfNvrInfo
    {
        //
        public uint uiType;

        //
        public byte ucbResp;

        //
        public QY_MESSENGER_ID idInfo_imGrp_related;

        //
        public NvrInfo nvrInfo;
        public QY_MESSENGER_ID idInfo_requester;
        public tmpInternal tmpInternal;
        //

    };

    public unsafe struct NvrInfo
    {

        public Int64 ui64Id;
        public short usCnt;
        //public IpcInfo[128] mems;
        public fixed byte buf_ipcInfo[Consts.bufSize_ipcInfos];


    };

    public unsafe struct IpcInfo
    {


        public int iChannel;
        public fixed char name[Consts.cntof_name];
        public fixed byte ip[Consts.cntof_ip];



    };
    public struct tmpInternal
    {


        public int usCnt_mems;

    };

    public struct PhoneEnlargeParam
    {

        public short x;
        public short y;
        public short usEnlargeType;
    }              //  放大类型

    public struct EnlargeParam
    {

        public short usEnlargeType;
        public Int64 ui64Id;
        public int tn_v;
    }
    public struct Resp

    {

        public int ii;
        public Int64 curr_idinfo;
    }
    public struct RespVideoStatus
    {
        public int bAmplification;
    }
    public struct OneBigLayoutParam
    {
        public byte ucbOneBigLayout;
        public Int64 ui64Id;
    };
    public unsafe struct IpcParam
    {
        public int iChannel;
        public fixed char channelName[Consts.cntof_channelName];
    };
    //  2010/12/27
    public struct CONF_LAYOUT
    {
        public uint uiType;
        public byte ucbResp;
        public QY_MESSENGER_ID idInfo_imGrp_related;               //  2009/02/23
        public short usOp;
        public short usLayoutType;
        public ConfLayoutParam confLayoutParam;

    }
    ;


    //
    //
    public struct CONF_req_policy
    {

        //
        public ushort usAvLevel;

        //
        public int conf_iFourcc;
        //
        public ushort usMaxSpeakers;


    }
    ;


    //
    public struct CONF_req
    {
        public uint uiType;

        //
        public QY_MESSENGER_ID idInfo_imGrp_related;

        //
        public ushort usOp;

        //
        public CONF_req_policy policy;

        //
        public QY_MESSENGER_ID idInfo_initiator;


    }
        ;




    public class QY_qThreadProcInfo_common : MACRO_qyQThreadProcInfo_common1
    {
    }
    ;


    //
    public unsafe struct DYN_BMP_RULE
    {
        //
        public QIS_RES_OBJ dynBmp;
        //
        fixed char name[32];                         //  name used to mean device id or unique name for dynBmp
        fixed char cusName[32];                      //  cusName used to mean a friendly comment for dynBmp


        //
        public bool ucbSaveVideo;

        //
        /*
		struct                                         {
					 unsigned char ucbAddedManually;
		uint tn_addedManuall;
	}
	helpInfo;
		*/

    };



    partial class qyFuncs
    {
        public unsafe struct TMP_ip
        {
            public fixed byte ip[Consts.CONST_qyMaxIpLen + 1];
        }
        public static unsafe int qnmOpenSession_mis(string servIp, ushort usPort, ref QY_COMM_AUTHINFO_MIS pAuthInfo, ushort usAuthCmd, uint uiChannelType, string clientIp, int iServiceId, string serviceVer, ref ENC_CTX_session pCommEncCtx, ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, ref SOCK_PARAM pSockParam, byte* data, uint* piDataSize)
        {
            int iErr = -1;
            QY_COMM_AUTHINFO_MIS authInfo;
            TMP_ip ip;
            byte* pIp = ip.ip;
            QY_MSG_HEAD tmpResp; tmpResp.uiLen = 0;
            TMP_buf4096 tmpBuf4096;
            uint len;

            if (0 == usPort) return -1;
            if (0 == usAuthCmd)
            {
                /*
				if (!pAuthInfo)
				{
					traceLogA("qnmOpenSession_mis failed: pAuthInfo is null");
					return -1;
				}
				*/
            }

            //if (pAuthInfo) memcpy(&authInfo, pAuthInfo, sizeof(authInfo));
            //else memset(&authInfo, 0, sizeof(authInfo));
            authInfo = pAuthInfo;
            //
            if (!GuiShare.pf_bQySockOpen(ref pSock))
            {       //  Èç¹ûÇ°ÃæÒÑ½¨Á¢Á¬½Ó£¬Ôò²»ÐèÒª½¨Á¢ÐÂÁ¬½Ó
                if (GuiShare.pf_qyConnect(servIp, usPort, ref pSockParam, ref pSock) != 0)
                {
                    //
                    string str = string.Format("qnmOpenSession_mis: connect {0} failed", servIp);
                    traceLog(str);
                    showInfo_open(0, null, str);
                    //
                    goto errLabel;
                }
                //
                traceLog("qnmOpenSession_mis: connect ok");
            }
            authInfo.usAuthCmd = usAuthCmd;
            authInfo.uiChannelType = uiChannelType; //  2007/07/08
                                                    //
            if (bIpValid(clientIp)) safeStrnCpy(clientIp, (byte*)authInfo.ip, Consts.CONST_qyMaxIpLen + 1);
            else
            {
                if (getLocalIpBySocket(ref pSock, ip.ip) != 0) goto errLabel;
                safeStrnCpy(mytoString(pIp), authInfo.ip, Consts.CONST_qyMaxIpLen + 1);
            }

            //  ½«»á»°ÃÜÔ¿·ÅÈëauthInfoÁ÷ÖÐ£¬2008/06/10
            //if (pCommEncCtx )
            {
                ref ENC_CTX_session pEncCtx = ref pCommEncCtx;
                if (pEncCtx.common.type == Consts.CONST_qyEncType_rsa
                    //&& pEncCtx.m_var.uiBlobLen_sessionKey != 0
                    )
                {
                    //
                    if (pEncCtx.m_aes.Key.Length > Consts.bufSize_sessionKeyBlob) goto errLabel;
                    authInfo.uiBlobLen_sessionKey = (uint)pEncCtx.m_aes.Key.Length;
                    System.Runtime.InteropServices.Marshal.Copy(pEncCtx.m_aes.Key, 0, (IntPtr)authInfo.sessionKeyBlob, (int)authInfo.uiBlobLen_sessionKey);
                    //
                    if (pEncCtx.m_aes.IV.Length > Consts.bufSize_sessionIv) goto errLabel;
                    authInfo.usLen_sessionIv = (ushort)pEncCtx.m_aes.IV.Length;
                    System.Runtime.InteropServices.Marshal.Copy(pEncCtx.m_aes.IV, 0, (IntPtr)authInfo.sessionIv, authInfo.usLen_sessionIv);
                }
                //  2015/08/28
            }
            //
            len = Consts.bufSize_4096;// sizeof(tmpBuf);
            if (0 != commAuthInfo2Stream_mis(&authInfo, tmpBuf4096.buf, &len)) goto errLabel;

            fixed (byte* pclientIp = pSession.clientIp)
            {
                safeStrnCpy(mytoString(pIp), pclientIp, Consts.CONST_qyMaxIpLen + 1); //  2007/08/03
            }


            //
            Int64 tStartTran; uint uiTranNo;
            MACRO_prepareForTran(out tStartTran, out uiTranNo);

            //  if  (  qyTellService_msg(  iServiceId,  serviceVer,  CONST_qyCmd_tellService,  pCommEncCtx,  tmpBuf,  len,  pSock,  pTo,  pSession  )  )  goto  errLabel;
            if (0 != qyTellService_msg((uint)iServiceId, serviceVer, pCommEncCtx, Consts.CONST_qyCmd_tellService, tStartTran, uiTranNo, tmpBuf4096.buf, (int)len, pSock, pSockParam.to, pSession))
            {
                traceLog("qnmOpenSession_mis: qyTellService_msg failed");
                goto errLabel;
            }
            //
#if DEBUG
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                if (pMisCnt.commEncCtx.ucbPublicKeyDownloaded)
                {
                    int ii = 0;
                }

            }
#endif
            //
            if (0 != qyRecvMsg(ref pSession, ref pSock, pSockParam.to, &tmpResp, tmpBuf4096.buf, Consts.bufSize_4096))
            {
                goto errLabel;
            }
            //  traceLogA(  "qwmOpenSession: get respCode of qyTellService is %s",  qyGetDesByType1(  CONST_qyRcTable,  tmpResp.head.usCode  )  );

            //  2011/10/27
            if (tmpResp.i64StartTime != tStartTran
                || tmpResp.uiTranNo != uiTranNo)
            {

                goto errLabel;
            }

            //  if  (  !isRcOk(  tmpResp.head.usCode  )  )  goto  errLabel; 

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
                if (GuiShare.pf_bQySockOpen(ref pSock))
                {
                    GuiShare.pf_qyDisconnect(ref pSock);
                }

                //
#if DEBUG
                traceLog("qnmOpenSession_mis failed");
#endif
            }
            else
            {
                if (piDataSize != null && data != null)
                {
                    *piDataSize = Math.Min(*piDataSize, tmpResp.uiLen);
                    mymemcpy((IntPtr)data, (IntPtr)tmpBuf4096.buf, *piDataSize);
                }
            }

            return iErr;
        }


        //
        public static unsafe void MACRO_memset_misMsgTalk(MIS_MSG_TALK* pMsgTalk)
        {
            //memset(  (  pMsgTalk  ),  0,  offsetof(  MIS_MSG_TALK,  data  )  +  offsetof(  MIS_MSG_routeTalkData,  buf  )  +  sizeof(  uint  )  );
            mymemset((IntPtr)pMsgTalk, 0, sizeof(MIS_MSG_TALK) - Consts.bufSize_routeTalkData);
        }
        public static unsafe void MACRO_memset_misMsgInput(MIS_MSG_INPUT* pMsgInput)
        {
            //memset(  (  pMsgInput  ),  0,  offsetof(  MIS_MSG_INPUT,  data  )  +  offsetof(  MIS_MSG_routeTalkData,  buf  )  +  sizeof(  uint  )  );
            mymemset((IntPtr)pMsgInput, 0, sizeof(MIS_MSG_INPUT) - Consts.bufSize_routeTalkData);
        }
        public static unsafe void MACRO_memset_misMsgTask(ref MIS_MSG_TASK pMsgTask)
        {
            //memset(  (  pMsgTask  ),  0,  offsetof(  MIS_MSG_TASK,  data  )  +  offsetof(  MIS_MSG_routeTalkData,  buf  )  +  sizeof(  uint  )  );
            int bufSize = sizeof(MIS_MSG_routeTalkData);
            fixed (MIS_MSG_TASK* pMsgTask0 = &pMsgTask)
            {
                mymemset((IntPtr)pMsgTask0, 0, sizeof(MIS_MSG_TASK) - bufSize);
            }
        }
        public static unsafe void MACRO_memset_misMsgReq(MIS_MSG_REQ* pMsgReq)
        {
            //memset(  (  pMsgReq  ),  0,  offsetof(  MIS_MSG_REQ,  data  )  +  offsetof(  MIS_MSG_routeTalkData,  buf  )  +  sizeof(  uint  )  );
            mymemset((IntPtr)pMsgReq, 0, sizeof(MIS_MSG_REQ) - Consts.bufSize_routeTalkData);
        }

        //
        public static unsafe void MACRO_memset_imLStream(IM_L_STREAM* pStream)
        {
            mymemset((IntPtr)(pStream), 0, (int)Marshal.OffsetOf<IM_L_STREAM>("buf"));
        }


        public static unsafe MIS_MSG_routeTalkData* MACRO_msgRouteTalkData(byte ucFlg, MIS_MSG_routeTalkData* pMsgData)
        {
            //(  (  isUcFlgRouteTalkData(  ucFlg  )  ||  isUcFlgTalkData(  ucFlg  )  )  ?  pMsgBuf  :  NULL  )
            return ((isUcFlgRouteTalkData(ucFlg) || isUcFlgTalkData(ucFlg)) ? pMsgData : null);
        }
        //  2014/09/28
        //  #define		M_getMsgContent(  ucFlg,  pMsgBuf  )				(  (  isUcFlgRouteTalkData(  ucFlg  )  ||  isUcFlgTalkData(  ucFlg  )  )  ?  (  (  IM_CONTENTU  *  )(  (  MIS_MSG_routeTalkData  *  )pMsgBuf  )->buf  )  :  (  IM_CONTENTU  *  )pMsgBuf  )
        public static unsafe IM_CONTENTU* M_getMsgContent(uint uiMsgType,  byte ucFlg, void* pMsgData)
        {
            //
            if  (  uiMsgType  ==  Consts.CONST_misMsgType_input)
            {
                if  ( !((isUcFlgRouteTalkData(ucFlg) || isUcFlgTalkData(ucFlg)) )  )  {
                    int ii = 0;
                }
                return (IM_CONTENTU*)&((MIS_MSG_routeTalkData*)pMsgData)->buf;
            }            

            //(  (  isUcFlgRouteTalkData(  ucFlg  )  ||  isUcFlgTalkData(  ucFlg  )  )  ?  (  (  IM_CONTENTU  *  )(  (  MIS_MSG_routeTalkData  *  )pMsgData  )->buf  )  :  (  IM_CONTENTU  *  )pMsgData  )
            return ((isUcFlgRouteTalkData(ucFlg) || isUcFlgTalkData(ucFlg)) ? ((IM_CONTENTU*)&((MIS_MSG_routeTalkData*)pMsgData)->buf) : (IM_CONTENTU*)pMsgData);
        }

        //
        public static unsafe void MACRO_qyNullThread(ref Thread h)
        {
            //if  (  h  )  {	::WaitForSingleObject(h, INFINITE  );	CloseHandle(h  ); h	=	NULL;	}												
            if (h != null)
            {
                int i;
                int maxStep = 100000;
                for (i = 0; i < maxStep; i++)
                {
                    if (!h.IsAlive) break;
                    Thread.Sleep(10);
                    continue;

                }
                if (i == maxStep)
                {
                    int ii = 0;
                    showInfo_open(null, "MACRO_qyNullThread", "failed, serious err");
                }
                h = null;
            }
            return;
        }











        /// <summary>
        /// //////////////////////////////////////////
        /// </summary>
        /// <param name="lhClientVer"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// 

        public static unsafe int qyhtonImSimpleResp(int lhClientVer, IM_SIMPLE_RESP* p)
        {
            p->uiType = qyhtoni(p->uiType);
            p->usCmd_org = qyhtons(p->usCmd_org);
            p->uiContentType_org = qyhtoni(p->uiContentType_org);
            p->iStatus = qyhtoni(p->iStatus);
            //
            //  MACRO_htonl64(  p->tStartTime_org  );		//  2009/10/17
            //  MACRO_htonl(  p->uiTranNo_org  );
            //
            return sizeof(IM_SIMPLE_RESP);
        }

        public static unsafe int qyntohImSimpleResp(int lhClientVer, IM_SIMPLE_RESP* p)
        {
            p->uiType = qyntohi(p->uiType);
            p->usCmd_org = qyntohs(p->usCmd_org);
            p->uiContentType_org = qyntohi(p->uiContentType_org);
            p->iStatus = qyntohi(p->iStatus);
            //
            //  MACRO_ntohl64(  p->tStartTime_org  );		//  2009/10/17
            //  MACRO_ntohl(  p->uiTranNo_org  );
            //
            return sizeof(IM_SIMPLE_RESP);
        }


        /////


        public static unsafe int qyhtonImLStream(int lhClientVer, IM_L_STREAM* p)
        {
            int len = -1;

            int tmp_l;
            //memcpy(&tmp_l, &p->ulStreamLen, sizeof(long));
            tmp_l = (int)p->ulStreamLen;
            if (tmp_l > Consts.bufSize_imLStream) return -1;

            len = (int)Marshal.OffsetOf<IM_L_STREAM>("buf") + tmp_l;   //  p->ulStreamLen;

            p->uiType = qyhtoni(p->uiType);
            p->uiContentType = qyhtoni(p->uiContentType);
            p->ulStreamLen = qyhtoni(p->ulStreamLen);

            return len;
        }

        public static unsafe int qyntohImLStream(long lhClientVer, byte* p, IM_L_STREAM_nh* pStream)
        {
            int len = -1;

            if (null == p || null == pStream)
            {
                traceLog(("qyntohImLStream failed, p or pStream is null"));
                return -1;
            }

            mymemset((IntPtr)pStream, 0, sizeof(IM_L_STREAM_nh));
            mymemcpy((IntPtr)pStream, (IntPtr)p, (uint)Marshal.OffsetOf<IM_L_STREAM_nh>("buf"));

            pStream->uiType = qyntohi(pStream->uiType);
            pStream->uiContentType = qyntohi(pStream->uiContentType);
            pStream->ulStreamLen = qyntohi(pStream->ulStreamLen);

            //
            pStream->p = p + (int)Marshal.OffsetOf<IM_L_STREAM>("buf");

            //  if  (  p->ulStreamLen  >  sizeof(  p->buf  )  )  return  -1;
            uint tmp_ul;
            //memcpy(&tmp_ul, &pStream->ulStreamLen, sizeof(tmp_ul));
            tmp_ul = pStream->ulStreamLen;
            if (tmp_ul > Consts.bufSize_imLStream) return -1;

            //  len  =  offsetof(  IM_L_STREAM,  buf  )  +  p->ulStreamLen;
            len = (int)((uint)Marshal.OffsetOf<IM_L_STREAM>("buf") + tmp_ul);

            return len;
        }


        //
        public static unsafe int retrieveImObjListReq2Stream(uint uiStreamId, RETRIEVE_IMOBJLIST_REQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->usMaxContactUpdateInfosPerSnd)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usMaxMemsPerSnd, (void*)pReq->usMaxContactUpdateInfosPerSnd, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tLastRefreshedTime_contactList_misServ)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tLastRefreshedTime, (void*)&pReq->tLastRefreshedTime_contactList_misServ, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->ucbNext)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbNext, (void*)pReq->ucbNext, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->idInfo_grp_lastRefreshed.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_lastRefreshed, (void*)&pReq->idInfo_grp_lastRefreshed.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->idInfo_lastRefreshed.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_lastRefreshed, (void*)&pReq->idInfo_lastRefreshed.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }


        public static unsafe int tmpHandler_stream2RetrieveImObjListReq(void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            RETRIEVE_IMOBJLIST_REQ* pReq = (RETRIEVE_IMOBJLIST_REQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pReq) goto errLabel;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_retrieveImGrpListReq:
                case Consts.CONST_imCommType_retrieveImGrpMemListReq:
                case Consts.CONST_imCommType_retrieveContactListReq:
                case Consts.CONST_imCommType_retrieveImObjListEndReq:
                case Consts.CONST_imCommType_retrieveWebContactListReq:
                    break;
                default:
                    {
                        string str;
                        str = string.Format("tmpHandler_stream2RetrieveImObjListReq: unknown uiStreamId: {0}", uiStreamId);
                        traceLog(str);
                    }
                    goto errLabel;
            }

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pReq->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_usMaxMemsPerSnd:
                    if (0 != getFieldData_short(pItem, (short*)&pReq->usMaxContactUpdateInfosPerSnd)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_tLastRefreshedTime:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pReq->tLastRefreshedTime_contactList_misServ)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_ucbNext:
                    if (0 != getFieldData_byte(pItem, (byte*)&pReq->ucbNext)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_idInfo_grp_lastRefreshed:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pReq->idInfo_grp_lastRefreshed.ui64Id)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_idInfo_lastRefreshed:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pReq->idInfo_lastRefreshed.ui64Id)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }




        public static unsafe int retrieveImGrpListResp2Stream(uint uiStreamId, RETRIEVE_IMGRPLIST_RESP* pUpdateInfos, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pUpdateInfos) return -1;
            if (null == buf) return -1;
            //
            if (pUpdateInfos->usCnt > Consts.CONST_maxContactUpdateInfos_grp) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pUpdateInfos->idInfo_lastRefreshed.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_lastRefreshed, &pUpdateInfos->idInfo_lastRefreshed.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            if (0 != pUpdateInfos->ucbEnd)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbEnd, (void*)pUpdateInfos->ucbEnd, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pUpdateInfos->usCnt)
            {
                //
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pUpdateInfos->usCnt, 0, &ptr, &len)) goto errLabel;

                //
                IM_GRP_INFO* pMems = (IM_GRP_INFO*)pUpdateInfos->buf_imGrpInfo_mems;
                for (i = 0; i < pUpdateInfos->usCnt; i++)
                {
                    //  ÏÂÃæ½«Ò»¸öQY_MESSENGER_REGINFO×ª»»³ÉÁ÷
                    //IM_GRP_INFO* pRegInfo = &pUpdateInfos->mems[i];
                    IM_GRP_INFO* pRegInfo = &pMems[i];

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;

                    if (0 != pRegInfo->idInfo.ui64Id)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pRegInfo->idInfo.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
                    }
                    if (0 != pRegInfo->name[0])
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_hint, pRegInfo->name, (uint)mywcslen(pRegInfo->name), &ptr, &len)) goto errLabel;
                    }

                    //  2014/01/31
                    if (0 != pRegInfo->usSubtype)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_subtype, (void*)pRegInfo->usSubtype, 0, &ptr, &len)) goto errLabel;
                    }

                    //  2017/07/26
                    if (0 != pRegInfo->idInfo_creator.ui64Id)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_creator, (void*)&pRegInfo->idInfo_creator.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
                    }

                    //
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
                }
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }

        public static unsafe int tmpHandler_stream2RetrieveImGrpListResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            RETRIEVE_IMGRPLIST_RESP* pResp = (RETRIEVE_IMGRPLIST_RESP*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;

            if (null == pResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_retrieveImGrpListResp:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pResp->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_ucbEnd:
                            if (0 != getFieldData_byte(pItem, (byte*)&pResp->ucbEnd)) goto errLabel;
                            break;
                        //
                        case Consts.CONST_qnmCfgId_idInfo_lastRefreshed:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pResp->idInfo_lastRefreshed.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pResp->usCnt)) goto errLabel;
                            //
                            if (pResp->usCnt > Consts.CONST_maxContactUpdateInfos_grp) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:
                    {
                        IM_GRP_INFO* pMem = null;
                        //
                        if (pResp->index >= Consts.CONST_maxContactUpdateInfos_grp)
                        {
                            traceLogA("tmpHandler_stream2RetrieveImGrpListResp: failed"); goto errLabel;
                        }
                        IM_GRP_INFO* pMems = (IM_GRP_INFO*)pResp->buf_imGrpInfo_mems;
                        int bufSize = Consts.bufSize_imGrpInfo_mems;
                        //
                        pMem = getMem(pMems, bufSize, pResp->index);// &pMems[pResp->index];
                                                                    //
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_messengerId:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->idInfo.ui64Id)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_hint:
                                if (0 != getFieldData_wStr(pItem, pMem->name, Consts.CONST_maxNameLen + 1)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_subtype:    //  2014/01/31
                                if (0 != getFieldData_short(pItem, (short*)&pMem->usSubtype)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_idInfo_creator: //  2017/07/27
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->idInfo_creator.ui64Id)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_null:
                                pResp->index++;
                                break;
                            default:
                                break;
                        }
                        //
                    }
                    break;
                default:
                    break;
            }

            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                traceLogA("tmpHandler_stream2RetrieveImGrpListResp: failed");
            }
            return iErr;
        }


        /////////////////////

        public static unsafe int retrieveImGrpMemListResp2Stream(uint uiStreamId, RETRIEVE_IMGRPMEMLIST_RESP* pUpdateInfos, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pUpdateInfos) return -1;
            if (null == buf) return -1;
            //
            if (pUpdateInfos->usCnt > Consts.CONST_maxContactUpdateInfos_grpMem) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pUpdateInfos->idInfo_grp_lastRefreshed.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_lastRefreshed, &pUpdateInfos->idInfo_grp_lastRefreshed.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }
            if (0 != pUpdateInfos->idInfo_lastRefreshed.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_lastRefreshed, &pUpdateInfos->idInfo_lastRefreshed.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            if (0 != pUpdateInfos->ucbEnd)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbEnd, (void*)pUpdateInfos->ucbEnd, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pUpdateInfos->usCnt)
            {
                //
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pUpdateInfos->usCnt, 0, &ptr, &len)) goto errLabel;

                //
                for (i = 0; i < pUpdateInfos->usCnt; i++)
                {
                    //  ÏÂÃæ½«Ò»¸öQY_MESSENGER_REGINFO×ª»»³ÉÁ÷
                    IM_GRP_MEM* pMems = (IM_GRP_MEM*)pUpdateInfos->buf_imGrpMem_mems;
                    IM_GRP_MEM* pRegInfo = &pMems[i];

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;

                    if (0 != pRegInfo->idInfo_grp.ui64Id)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp1, (void*)&pRegInfo->idInfo_grp.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
                    }
                    if (0 != pRegInfo->idInfo_mem.ui64Id)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pRegInfo->idInfo_mem.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
                    }
                    if (0 != pRegInfo->iRole)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iRole, (void*)pRegInfo->iRole, 0, &ptr, &len)) goto errLabel;
                    }

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
                }
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }

        public static unsafe int tmpHandler_stream2RetrieveImGrpMemListResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            RETRIEVE_IMGRPMEMLIST_RESP* pResp = (RETRIEVE_IMGRPMEMLIST_RESP*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;

            if (null == pResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_retrieveImGrpMemListResp:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pResp->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_ucbEnd:
                            if (0 != getFieldData_byte(pItem, (byte*)&pResp->ucbEnd)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_grp_lastRefreshed:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pResp->idInfo_grp_lastRefreshed.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_lastRefreshed:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pResp->idInfo_lastRefreshed.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pResp->usCnt)) goto errLabel;
                            //
                            if (pResp->usCnt > Consts.CONST_maxContactUpdateInfos_grpMem) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:
                    {
                        IM_GRP_MEM* pMem = null;
                        //
                        if (pResp->index >= Consts.CONST_maxContactUpdateInfos_grpMem)
                        {
                            traceLogA("tmpHandler_stream2RetrieveImGrpListResp: failed"); goto errLabel;
                        }
                        //
                        IM_GRP_MEM* pMems = (IM_GRP_MEM*)pResp->buf_imGrpMem_mems;
                        int bufSize = Consts.bufSize_imGrpMem_mems;
                        //
                        pMem = getMem(pMems, bufSize, pResp->index);// &pMems[pResp->index];
                                                                    //
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_idInfo_grp1:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->idInfo_grp.ui64Id)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_messengerId:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->idInfo_mem.ui64Id)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_iRole:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->iRole)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_null:
                                pResp->index++;
                                break;
                            default:
                                break;
                        }
                        //
                    }
                    break;
                default:
                    break;
            }

            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                traceLogA("tmpHandler_stream2RetrieveImGrpMemListResp: failed");
            }
            return iErr;
        }



        public static unsafe int retrieveContactListResp2Stream(uint uiStreamId, RETRIEVE_CONTACTLIST_RESP* pUpdateInfos, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;
            int j;

            if (null == pUpdateInfos) return -1;
            if (null == buf) return -1;
            //
            if (pUpdateInfos->usCnt > Consts.CONST_maxContactUpdateInfos_contact) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pUpdateInfos->idInfo_lastRefreshed.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_lastRefreshed, &pUpdateInfos->idInfo_lastRefreshed.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            if (0 != pUpdateInfos->ucbEnd)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbEnd, (void*)pUpdateInfos->ucbEnd, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pUpdateInfos->usCnt)
            {
                //
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pUpdateInfos->usCnt, 0, &ptr, &len)) goto errLabel;
                //
                for (i = 0; i < pUpdateInfos->usCnt; i++)
                {
                    //  ÏÂÃæ½«Ò»¸öQY_MESSENGER_REGINFO×ª»»³ÉÁ÷
                    RETRIEVE_contact_mem* pMems = (RETRIEVE_contact_mem*)pUpdateInfos->buf_retrieveContactMem_mems;
                    RETRIEVE_contact_mem* pMem = &pMems[i];
                    QY_MESSENGER_REGINFO* pRegInfo = &pMem->regInfo;

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;

                    if (0 != pRegInfo->addr.uiObjType && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pRegInfo->addr.uiObjType, 0, &ptr, &len)) goto errLabel;    //  2008/01/31
                    if (0 != pRegInfo->addr.idInfo.ui64Id && 0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pRegInfo->addr.idInfo.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
                    //
                    if (0 != pMem->iRole)
                    {  //  2017/08/14
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iRole, (void*)pMem->iRole, 0, &ptr, &len)) goto errLabel;
                    }
                    //
                    if (0 != pRegInfo->addr.uiDevType && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiDevType_from, (void*)pRegInfo->addr.uiDevType, 0, &ptr, &len)) goto errLabel;
                    //if (0 != pRegInfo->addr.wDevIdStr[0] && 0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_wDevIdStr_from, pRegInfo->addr.wDevIdStr, lstrlen(pRegInfo->addr.wDevIdStr), &ptr, &len)) goto errLabel;   //  2008/01/31

                    //
#if false
					for (j = 0; j < mycountof(pRegInfo->uiFieldIds); j++)
	{
		if (!pRegInfo->u.cols[j][0]) continue;
		if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiFieldId, (void*)pRegInfo->uiFieldIds[j], 0, &ptr, &len)) goto errLabel;
		if (0!=data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_hint, pRegInfo->u.cols[j], lstrlen(pRegInfo->u.cols[j]), &ptr, &len)) goto errLabel;
	}
#endif

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
                }
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }


        public static unsafe int tmpHandler_stream2RetrieveContactListResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            RETRIEVE_CONTACTLIST_RESP* pResp = (RETRIEVE_CONTACTLIST_RESP*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            int lVal;

            if (null == pResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_retrieveContactListResp:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pResp->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_ucbEnd:
                            if (0 != getFieldData_byte(pItem, (byte*)&pResp->ucbEnd)) goto errLabel;
                            break;
                        //
                        case Consts.CONST_qnmCfgId_idInfo_lastRefreshed:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pResp->idInfo_lastRefreshed.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pResp->usCnt)) goto errLabel;
                            //
                            if (pResp->usCnt > Consts.CONST_maxContactUpdateInfos_contact) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:
                    {
                        RETRIEVE_contact_mem* pMem = null;
                        //
                        if (pResp->index >= Consts.CONST_maxContactUpdateInfos_contact || pResp->iFiledIndex >= Consts.CONST_qnmMaxRegFields)
                        {
                            traceLogA("tmpHandler_stream2RetrieveContactListResp: failed");
                            goto errLabel;
                        }
                        RETRIEVE_contact_mem* pMems = (RETRIEVE_contact_mem*)pResp->buf_retrieveContactMem_mems;
                        int bufSize = Consts.bufSize_retrieveContaceMem_mems;
                        //
                        pMem = getMem(pMems, bufSize, pResp->index);// &pMems[pResp->index];
                                                                    //
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_uiObjType:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->regInfo.addr.uiObjType)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_messengerId:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->regInfo.addr.idInfo.ui64Id)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_iRole:  //  2017/08.14
                                if (0 != getFieldData_long(pItem, (int*)&pMem->iRole)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_uiDevType_from:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->regInfo.addr.uiDevType)) goto errLabel;
                                break;
                            /*
                case Consts.CONST_qnmCfgId_wDevIdStr_from:
                    if (0!=getFieldData_wStr(pItem, pMem->regInfo.addr.wDevIdStr, mycountof(pMem->regInfo.addr.wDevIdStr))) goto errLabel;
                    break;
                            */
                            case Consts.CONST_qnmCfgId_uiFieldId:
                                if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                                pMem->regInfo.uiFieldIds[pResp->iFiledIndex] = (uint)lVal;
                                break;
                            case Consts.CONST_qnmCfgId_hint:
                                {
                                    //if (0 != getFieldData_wStr(pItem, pMem->regInfo.u.cols[pResp->iFiledIndex], mycountof(pMem->regInfo.u.cols[pResp->iFiledIndex]))) goto errLabel;
                                    if (0 != getFieldData_wStr(pItem, pMem->regInfo.tmp_col, Consts.CONST_maxNameLen + 1)) goto errLabel;
                                    switch (pMem->regInfo.uiFieldIds[pResp->iFiledIndex])
                                    {
                                        case Consts.CONST_fieldId_dw:
                                            mytcsncpy(pMem->regInfo.dw, new string(pMem->regInfo.tmp_col), Consts.CONST_maxNameLen + 1);
                                            break;
                                        case Consts.CONST_fieldId_bm:
                                            mytcsncpy(pMem->regInfo.bm, new string(pMem->regInfo.tmp_col), Consts.CONST_maxNameLen + 1);
                                            break;
                                        case Consts.CONST_fieldId_syr:
                                            mytcsncpy(pMem->regInfo.syr, new string(pMem->regInfo.tmp_col), Consts.CONST_maxNameLen + 1);
                                            break;
                                        default:
                                            break;

                                    }
                                }
                                pResp->iFiledIndex++;
                                break;
                            case Consts.CONST_qnmCfgId_null:
                                pResp->index++;
                                pResp->iFiledIndex = 0;
                                break;
                            default:
                                break;
                        }
                        //
                    }
                    break;
                default:
                    break;
            }

            iErr = 0;
        errLabel:
            if (iErr != 0)
            {
                traceLogA("tmpHandler_stream2RetrieveContactListResp: failed");
            }
            return iErr;
        }

        ////////////////////

        /*
						uint										uiType;
						//
						__int64											tLastRefreshedTime_contactList_misServ;		//  ·¢ËÍ¸ø¿Í»§¶ËÈÃÖªµÀÊÇ´ÓÊ²Ã´Ê±¼ä¿ªÊ¼Ë¢ÐÂµÄ
						*/

        public static unsafe int retrieveImObjListEndResp2Stream(uint uiStreamId, RETRIEVE_IMOBJLISTEND_RESP* pResp, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pResp) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pResp->tLastRefreshedTime_contactList_misServ)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tLastRefreshedTime, (void*)&pResp->tLastRefreshedTime_contactList_misServ, sizeof(Int64), &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }



        public static unsafe int tmpHandler_stream2RetrieveImObjListEndResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            RETRIEVE_IMOBJLISTEND_RESP* pResp = (RETRIEVE_IMOBJLISTEND_RESP*)p1;
            //  QY_CFGITEM							*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pResp) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_retrieveImObjListEndResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pResp->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_tLastRefreshedTime:
                    if (0 != getFieldData_l64(pItem, &pResp->tLastRefreshedTime_contactList_misServ)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        /// ////////////////////////////////////////////////////////////////


        //  
        public static unsafe int transferFileReq2Stream(uint uiStreamId, TRANSFER_FILE_REQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->ui64FileLen)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_ui64FileLen, (void*)&pReq->ui64FileLen, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->fileName[0])
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_fileName, pReq->fileName, (uint)mylstrlen(pReq->fileName), &ptr, &len)) goto errLabel;
            }

            //  2014/07/13
            if (0 != pReq->uiSizePerSnd_suggest)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSizePerSnd, (void*)pReq->uiSizePerSnd_suggest, 0, &ptr, &len)) goto errLabel;
            }

            //  2015/08/05
            if (0 != pReq->usSubtype)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_subtype, (void*)pReq->usSubtype, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->idInfo_logicalPeer.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_logicalPeer, (void*)&pReq->idInfo_logicalPeer.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }



            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferFileReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_FILE_REQ* pContent = (TRANSFER_FILE_REQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferFileReq) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_fileName:
                    if (0 != getFieldData_wStr(pItem, pContent->fileName, Consts.cntof_fileName)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_ui64FileLen:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->ui64FileLen)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiSizePerSnd:   //  2014/07/13
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSizePerSnd_suggest)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_subtype:   //  2015/08/05
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usSubtype)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_idInfo_logicalPeer:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_logicalPeer.ui64Id)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }






        public static unsafe int transferFileReplyReq2Stream(uint uiStreamId, TRANSFER_FILE_REPLYREQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->usOp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp, (void*)pReq->usOp, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiSerialNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSerialNo_org, (void*)pReq->uiSerialNo_org, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferFileReplyReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_FILE_REPLYREQ* pContent = (TRANSFER_FILE_REPLYREQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferFileReplyReq) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_usOp:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usOp)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_tStartTime_org:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiTranNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiSerialNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSerialNo_org)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }



        //
        public static unsafe int taskProcReq2Stream(uint uiStreamId, TASK_PROC_REQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->usOp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp, (void*)pReq->usOp, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiSerialNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSerialNo_org, (void*)pReq->uiSerialNo_org, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->uiContentType_org)
            {   //  
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiContentType_org, (void*)pReq->uiContentType_org, 0, &ptr, &len)) goto errLabel;
            }

            //  2014/08/28
            if (0 != pReq->usTaskProcObjType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usTaskProcObjType, (void*)pReq->usTaskProcObjType, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->usTaskProcObjType)
            {

                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;

                switch (pReq->usTaskProcObjType)
                {
                    case Consts.CONST_usTaskProcObjType_vStream:
                        //
                        if (0 != pReq->vStream.uiTranNo_openAvDev)
                        {
                            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openVideoDev, (void*)pReq->vStream.uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
                        }
                        //
                        //
                        break;
                    default:
                        break;
                }


                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
            }



            //  2009/09/11
            if (0 != pReq->uiTaskType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTaskType, (void*)pReq->uiTaskType, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->viewDynBmp.uiObjType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pReq->viewDynBmp.uiObjType, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->viewDynBmp.usIndex_obj)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_index, (void*)pReq->viewDynBmp.usIndex_obj, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TaskProcReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TASK_PROC_REQ* pContent = (TASK_PROC_REQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            //if  (  uiStreamId  !=  Consts.CONST_imCommType_taskProcReq  )  goto  errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_taskProcReq:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_usOp:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usOp)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_tStartTime_org:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiTranNo_org:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiSerialNo_org:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiSerialNo_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiContentType_org:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiContentType_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usTaskProcObjType:  //  2014/08/28
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usTaskProcObjType)) goto errLabel;
                            break;

                        //  2014/10/09							
                        case Consts.CONST_qnmCfgId_uiTaskType: //  2009/09/11
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTaskType)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiObjType:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->viewDynBmp.uiObjType)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_index:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->viewDynBmp.usIndex_obj)) goto errLabel;
                            break;

                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:  //  2014/08/28
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_uiTranNo_openVideoDev:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->vStream.uiTranNo_openAvDev)) goto errLabel;
                            break;
                        //
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        ///////////////

        public static unsafe int taskProcResp2Stream(uint uiStreamId, TASK_PROC_RESP* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->usCmd_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCmd_org, (void*)pReq->usCmd_org, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->usOp_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp_org, (void*)pReq->usOp_org, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->iStatus)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_status, (void*)pReq->iStatus, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->idInfo_logicalPeer.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_logicalPeer, (void*)&pReq->idInfo_logicalPeer.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TaskProcResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TASK_PROC_RESP* pContent = (TASK_PROC_RESP*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_taskProcResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_tStartTime_org:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiTranNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usCmd_org:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usCmd_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usOp_org:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usOp_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_status:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->iStatus)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_idInfo_logicalPeer:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_logicalPeer.ui64Id)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        public static unsafe int transferFileDataReq2Stream(uint uiStreamId, TRANSFER_FILEDATA_REQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiSerialNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSerialNo_org, (void*)pReq->uiSerialNo_org, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->uiSizePerSnd)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSizePerSnd, (void*)pReq->uiSizePerSnd, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->uiDataBlockIndex)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiDataBlockIndex, (void*)pReq->uiDataBlockIndex, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TransferFileDataReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_FILEDATA_REQ* pContent = (TRANSFER_FILEDATA_REQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferFileDataReq) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_tStartTime_org:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiTranNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiSerialNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSerialNo_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiSizePerSnd:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSizePerSnd)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiDataBlockIndex:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiDataBlockIndex)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int transferFileDataResp2Stream(uint uiStreamId, TRANSFER_FILEDATA_RESP* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->uiDataBlockIndex)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiDataBlockIndex, (void*)pReq->uiDataBlockIndex, 0, &ptr, &len)) goto errLabel;
            }
#if false//0	//  2014/07/13
	 if  (  pReq->uiLen  )  {
		 if  (  data2Stream(  Consts.CONST_qyDataType_data,  Consts.CONST_qnmCfgId_rawData,  (  void  *  )pReq->buf,  pReq->uiLen,  &ptr,  &len  )  )  goto  errLabel;
	 }
#endif
            //  2014/07/13
            if (0 != pReq->uiLen)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_lData, Consts.CONST_qnmCfgId_rawData, (void*)pReq->buf, pReq->uiLen, &ptr, &len)) goto errLabel;
            }


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TransferFileDataResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_FILEDATA_RESP* pContent = (TRANSFER_FILEDATA_RESP*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferFileDataResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_uiDataBlockIndex:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiDataBlockIndex)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_rawData:
                    pContent->uiLen = Consts.bufSize_transferFileDataResp;// sizeof(pContent->buf  );
#if false//0
				   if  (  getFieldData_data(  pItem,  pContent->buf,  &pContent->uiLen  )  )  goto  errLabel;
#endif
                    //  2014/07/13
                    if (0 != getFieldData_lData(pItem, pContent->buf, &pContent->uiLen)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }

        public static unsafe int transferFileEndReq2Stream(uint uiStreamId, TRANSFER_FILEEND_REQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiSerialNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSerialNo_org, (void*)pReq->uiSerialNo_org, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TransferFileEndReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_FILEEND_REQ* pContent = (TRANSFER_FILEEND_REQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferFileEndReq) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_tStartTime_org:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiTranNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiSerialNo_org:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSerialNo_org)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }



        public static unsafe int transferFileEndResp2Stream(uint uiStreamId, TRANSFER_FILEEND_RESP* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->uiVerifyMethod)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiVerifyMethod, (void*)pReq->uiVerifyMethod, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TransferFileEndResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_FILEEND_RESP* pContent = (TRANSFER_FILEEND_RESP*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferFileEndResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_uiVerifyMethod:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiVerifyMethod)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }



        //////////////////////////////////////////////////////
        public static unsafe int qisGetCfgsReq2Stream(uint uiStreamId, QIS_getCfgs_req* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->ucbResp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbResp, (void*)pReq->ucbResp, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->uiSizePerSnd_media)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSizePerSnd, (void*)pReq->uiSizePerSnd_media, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usIntervalInS_refreshRecentFriends, (void*)pReq->intervalParams.usIntervalInS_refreshRecentFriends, 0, &ptr, &len)) goto errLabel;
            if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usIntervalInS_refreshContactList, (void*)pReq->intervalParams.usIntervalInS_refreshContactList, 0, &ptr, &len)) goto errLabel;

            //  2015/07/28

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2QisGetCfgsReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            QIS_getCfgs_req* pContent = (QIS_getCfgs_req*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_getCfgsReq) goto errLabel;

            ushort tmp_cfgId;
            tmp_cfgId = pItem->head.cfgId;
            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_ucbResp:
                    if (0 != getFieldData_byte(pItem, (byte*)&pContent->ucbResp)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiSizePerSnd:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSizePerSnd_media)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usIntervalInS_refreshRecentFriends:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->intervalParams.usIntervalInS_refreshRecentFriends)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usIntervalInS_refreshContactList:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->intervalParams.usIntervalInS_refreshContactList)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        /// <summary>
        /// //////////////////////////////////
        /// </summary>
        /// <param name="uiStreamId"></param>
        /// <param name="pReq"></param>
        /// <param name="buf"></param>
        /// <param name="uiBufSize"></param>
        /// <returns></returns>
        /// 
        public static unsafe int taskInteractionReq2Stream(uint uiStreamId, TASK_INTERACTION_REQ* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->usOp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp, (void*)pReq->usOp, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }

            //
#if DEBUG
            ref TASK_INTERACTION_REQ tmp_pReq = ref *pReq;
#endif


            //  2015/02/17
            if (0 != tmp_htonAvStream(&pReq->avStream, &ptr, (uint*)&len)) goto errLabel;

            //
            //
            if (0 != pReq->talkerDesc[0])
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_talkerDesc, pReq->talkerDesc, (uint)mylstrlen(pReq->talkerDesc), &ptr, &len)) goto errLabel;
            }



            //  2015/07/30
#if false
			if (0 != pReq->idInfo_speaker.ui64Id)
			{
				if (0!=data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_speaker, (void*)&pReq->idInfo_speaker.ui64Id, sizeof(UInt64),  &ptr,  &len  )  )  goto errLabel;
			}
#endif

            //
            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        public static unsafe int tmpHandler_stream2TaskInteractionReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TASK_INTERACTION_REQ* pContent = (TASK_INTERACTION_REQ*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;
            bool bProcessed = false;    //  2015/02/17


            if (null == pContent) goto errLabel;


            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            //
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_taskInteractionReq:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_usOp:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usOp)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_tStartTime_org:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiTranNo_org:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_talkerDesc:
                            if (0 != getFieldData_wStr(pItem, pContent->talkerDesc, Consts.cntof_talkerDesc)) goto errLabel;
                            break;

#if false
				case Consts.CONST_qnmCfgId_idInfo_speaker:  //  2015/07/30
					if (0!=getFieldData_l64(pItem, (Int64*)&pContent->idInfo_speaker.ui64Id)) goto errLabel;
					break;
#endif
                        case Consts.CONST_qnmCfgId_idInfo_grp_related:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_imGrp_related.ui64Id)) goto errLabel;
                            break;
                        default:
                            //
                            break;
                    }
                    break;

                case Consts.CONST_imCommType_avStream:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            break;
                        case Consts.CONST_qnmCfgId_null:
                            break;
                        default:
                            //  2015/02/17
                            //if  (  tmp_ntohAvTranInfo(  pItem,  &pContent->tranInfo,  &bProcessed  )  )  goto  errLabel;
                            if (0 != tmp_ntohAvStream(pItem, &pContent->avStream, &bProcessed)) goto errLabel;
                            if (bProcessed)
                            {
                                iErr = 0; goto errLabel;
                            }
                            //
                            break;
                    }
                    break;

                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        /// <summary>
        /// ////////////
        /// </summary>
        /// <param name="uiStreamId"></param>
        /// <param name="pReq"></param>
        /// <param name="buf"></param>
        /// <param name="uiBufSize"></param>
        /// <returns></returns>
        public static unsafe int confKey2Stream(uint uiStreamId, CONF_KEY* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            //  2010/12/29
            if (0 != pReq->usLayoutType_starter)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usLayoutType, (void*)pReq->usLayoutType_starter, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->usCnt)
            {
                //
                if (pReq->usCnt > Consts.cntof_videoConferenceMems) goto errLabel;
                //
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pReq->usCnt, 0, &ptr, &len)) goto errLabel;
                //
                VIDEO_CONFERENCE_MEM* pReq_mems = (VIDEO_CONFERENCE_MEM*)pReq->buf_VIDEO_CONFERENCE_MEM_mems;
                for (i = 0; i < pReq->usCnt; i++)
                {
                    VIDEO_CONFERENCE_MEM* pMem = &pReq_mems[i];

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;


                    //  2009/05/31
                    if (0 != tmp_htonAvStream(&pMem->avStream, &ptr, (uint*)&len)) goto errLabel;

                    //  2013/07/18
                    if (0 != pMem->usIndex_speaker)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usIndex_speaker, (void*)pMem->usIndex_speaker, 0, &ptr, &len)) goto errLabel;
                    }

                    //
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

                }

            }

            //
            /*  if (0 != pReq->confLayoutParam.enlargeParam.ui64Id)
              {}*/

            if (0 != tmp_htonConfLayoutParam(&pReq->confLayoutParam, &ptr, &len)) goto errLabel;



            //	 
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2confKey(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            CONF_KEY* pContent = (CONF_KEY*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;
            bool bProcessed = false;

            if (null == pContent) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            if (uiStreamId == Consts.CONST_imCommType_confLayoutParam)
            {
                tmp_ntohConfLayoutParam(pCtx, (int)uiStreamId, pItem, &pContent->confLayoutParam, &bProcessed);
            }
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_confKey:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_grp_related:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_imGrp_related.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usLayoutType:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usLayoutType_starter)) goto errLabel;
                            break;

                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usCnt)) goto errLabel;
                            //
                            if (pContent->usCnt > Consts.cntof_videoConferenceMems) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:
                    {
                        VIDEO_CONFERENCE_MEM* pMem = null;
                        //
                        if (pContent->tmpInternal.usCnt_mems >= Consts.cntof_videoConferenceMems)
                        {
#if __DEBUG__
					traceLogA("tmpHandler_stream2RefreshWebContactsInfo failed: usCnt_mems is too big");
#endif
                            goto errLabel;
                        }
                        //
                        VIDEO_CONFERENCE_MEM* pContent_mems = (VIDEO_CONFERENCE_MEM*)pContent->buf_VIDEO_CONFERENCE_MEM_mems;
                        int bufSize = Consts.bufSize_videoConferenceMem_mems;
                        //
                        pMem = getMem(pContent_mems, bufSize, pContent->tmpInternal.usCnt_mems);// &pContent_mems[pContent->tmpInternal.usCnt_mems];
                                                                                                //
                        switch (tmp_cfgId)
                        {

                            case Consts.CONST_qnmCfgId_usIndex_speaker:    //  2013/07/18
                                if (0 != getFieldData_short(pItem, (short*)&pMem->usIndex_speaker)) goto errLabel;
                                break;

                            case Consts.CONST_qnmCfgId_null:
                                pContent->tmpInternal.usCnt_mems++;
                                break;
                            default:
                                break;
                        }
                        //
                    }
                    break;
                case Consts.CONST_imCommType_avStream:
                    {
                        VIDEO_CONFERENCE_MEM* pMem = null;
                        //
                        if (pContent->tmpInternal.usCnt_mems >= Consts.cntof_videoConferenceMems)
                        {
#if __DEBUG__
							traceLogA("tmpHandler_stream2RefreshWebContactsInfo failed: usCnt_mems is too big");
#endif
                            goto errLabel;
                        }
                        //
                        VIDEO_CONFERENCE_MEM* pContent_mems = (VIDEO_CONFERENCE_MEM*)pContent->buf_VIDEO_CONFERENCE_MEM_mems;
                        pMem = &pContent_mems[pContent->tmpInternal.usCnt_mems];

                        //
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_start:
                                break;
                            default:
                                if (0 != tmp_ntohAvStream(pItem, &pMem->avStream, &bProcessed)) goto errLabel;
                                if (bProcessed)
                                {
                                    iErr = 0; goto errLabel;
                                }
                                break;
                        }

                        //
                    }
                    break;

                default:
                    break;

            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        ///////////////////////////////////////////////




        public static unsafe int statusConfLayoutParam2Stream(uint uiStreamId, StatusConfLayoutParam* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {                                                                                                       // (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(UInt64), &ptr, &len
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }



            //     if (0 != tmp_htonConfLayoutParam(&pReq->confLayoutParam, &ptr, &len))
            //if (pReq->confLayoutParam.enlargeParam.ui64Id) 
            {
                if (0 != tmp_htonConfLayoutParam(&pReq->confLayoutParam, &ptr, &len)) goto errLabel;
            }


            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;











            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;


        }


        //
        public static unsafe int tmpHandler_stream2statusConfLayoutParam(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            StatusConfLayoutParam* pContent = (StatusConfLayoutParam*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;
            bool bProcessed = false;

            if (null == pContent) goto errLabel;

            ushort tmp_cfgId;
            // memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            //
            uint uiStreamId_1 = 0;
            if (pCtx->index_streamId > 0)
            {
                uiStreamId_1 = pCtx->uiStreamIds[1];
            }



            if (uiStreamId_1 == Consts.CONST_imCommType_confLayoutParam)
            {
                tmp_ntohConfLayoutParam(pCtx, (int)uiStreamId, pItem, &pContent->confLayoutParam, &bProcessed);
            }
            else
            {
                //
                switch (uiStreamId)
                {
                    case Consts.CONST_imCommType_statusConfLayoutParam:
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_start:
                                pContent->uiType = uiStreamId;
                                break;
                            case Consts.CONST_qnmCfgId_idInfo_grp_related:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_imGrp_related.ui64Id)) goto errLabel;
                                break;
                            //
                            default:
                                break;
                        }
                        break;
                }
                //

            }


            iErr = 0;
        errLabel:
            return iErr;
        }










        ///////////////////////////////////////////////


        //
        //  2010/12/27
        public static unsafe int confLayout2Stream(uint uiStreamId, CONF_LAYOUT* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->usLayoutType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usLayoutType, (void*)pReq->usLayoutType, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->usOp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp, (void*)pReq->usOp, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != tmp_htonConfLayoutParam(&pReq->confLayoutParam, &ptr, &len))
            {
                goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }
        public static unsafe int tmp_htonConfLayoutParam(ConfLayoutParam* pReq, byte** ppBuf, uint* uiBufSize)
        {

            int iErr = -1;
            int i = 0;
            byte* ptr = *ppBuf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == ppBuf) return -1;

            //
            int uiStreamId = Consts.CONST_imCommType_confLayoutParam;
            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            //
            if (0 != pReq->oneBigLayoutParam.ucbOneBigLayout)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbOneBigLayout, (void*)pReq->oneBigLayoutParam.ucbOneBigLayout, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->oneBigLayoutParam.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId_oneBig, (void*)&pReq->oneBigLayoutParam.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->enlargeParam.usEnlargeType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usEnlargeType, (void*)pReq->enlargeParam.usEnlargeType, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->enlargeParam.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pReq->enlargeParam.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->enlargeParam.tn_v)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_uiTranNo_openVideoDev, (void*)pReq->enlargeParam.tn_v, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->ipcParam.iChannel)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_iChannel, (void*)pReq->ipcParam.iChannel, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->ipcParam.channelName[0])
            {
                //  data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_name, pMem->name, (uint)mylstrlen(pMem->name), &ptr, &len)
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_channelName, pReq->ipcParam.channelName, (uint)mylstrlen(pReq->ipcParam.channelName), &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->resp.ii)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_ii, (void*)pReq->resp.ii, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->resp.curr_idinfo)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_curr_idinfo, (void*)&pReq->resp.curr_idinfo, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->phoneEnlargeParam.x)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_enlarge_x, (void*)pReq->phoneEnlargeParam.x, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->phoneEnlargeParam.y)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_enlarge_y, (void*)pReq->phoneEnlargeParam.y, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->phoneEnlargeParam.usEnlargeType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_phone_usEnlargeType, (void*)pReq->phoneEnlargeParam.usEnlargeType, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->respVideoStatus.bAmplification)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_amplificationType, (void*)pReq->respVideoStatus.bAmplification, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;


            //
            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *ppBuf = ptr;
                *uiBufSize = len;
            }
            return iErr;

        }

        //
        public static unsafe int tmpHandler_stream2confLayout(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            CONF_LAYOUT* pContent = (CONF_LAYOUT*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;
            //
            int uiStreamId_1 = 0;
            if (pCtx->index_streamId > 0)
            {
                uiStreamId_1 = (int)pCtx->uiStreamIds[1];
            }

            //
            bool bProcessed = false;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            //
            if (uiStreamId_1 == Consts.CONST_imCommType_confLayoutParam)
            {
                tmp_ntohConfLayoutParam(pCtx, (int)uiStreamId, pItem, &pContent->confLayoutParam, &bProcessed);
            }
            else
            {
                //
                switch (uiStreamId)
                {
                    case Consts.CONST_imCommType_confLayout:
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_start:
                                pContent->uiType = uiStreamId;
                                break;
                            case Consts.CONST_qnmCfgId_idInfo_grp_related:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_imGrp_related.ui64Id)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_usLayoutType:
                                if (0 != getFieldData_short(pItem, (short*)&pContent->usLayoutType)) goto errLabel;
                                //
                                break;
                            case Consts.CONST_qnmCfgId_ucbResp:
                                if (0 != getFieldData_byte(pItem, (byte*)&pContent->ucbResp)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_usOp:
                                if (0 != getFieldData_short(pItem, (short*)&pContent->usOp)) goto errLabel;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            iErr = 0;
        errLabel:
            return iErr;
        }
        ///
        public static unsafe int tmp_ntohConfLayoutParam(CTX_stream2Data* pCtx, int uiStreamId, QY_CFGITEM_ntoh_U* pItem, ConfLayoutParam* pContent, bool* pbProcessed)
        {
            int iErr = -1;
            //
            bool bProcessed = false;

            if (null == pContent) goto errLabel;

            short tmp_cfgId;


            tmp_cfgId = (short)pItem->head.cfgId;

            //
            bProcessed = true;
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_confLayoutParam:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            break;
                        case Consts.CONST_qnmCfgId_ucbOneBigLayout:
                            if (0 != getFieldData_byte(pItem, (byte*)&pContent->oneBigLayoutParam.ucbOneBigLayout)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_messengerId_oneBig:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->oneBigLayoutParam.ui64Id)) goto errLabel;
                            break;

                        case Consts.CONST_qnmCfgId_usEnlargeType:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->enlargeParam.usEnlargeType)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_messengerId:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->enlargeParam.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiTranNo_openVideoDev:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->enlargeParam.tn_v)) goto errLabel;
                            break;
                        //
                        case Consts.CONST_qnmCfgId_iChannel:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->ipcParam.iChannel)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_channelName:
                            if (0 != getFieldData_wStr(pItem, pContent->ipcParam.channelName, Consts.cntof_channelName)) goto errLabel;
                            //
                            {
                                string str1 = new string(pContent->ipcParam.channelName);
                                int ii = 0;
                            }
                            //
                            break;
                        case Consts.CONST_qnmCfgId_ii:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->resp.ii)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_curr_idinfo:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->resp.curr_idinfo)) goto errLabel;
                            {
                                ref ConfLayoutParam tmpContent = ref *pContent;
                                //
                                int ii = 0;

                            }
                            break;
                        case Consts.CONST_qnmCfgId_enlarge_x:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->phoneEnlargeParam.x)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_enlarge_y:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->phoneEnlargeParam.y)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_phone_usEnlargeType:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->phoneEnlargeParam.usEnlargeType)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_amplificationType:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->respVideoStatus.bAmplification)) goto errLabel;
                            break;

                        default:
                            break;
                    }
                    break;
                default:
                    break;

            }
            iErr = 0;
        errLabel:

            if (null == pbProcessed) *pbProcessed = bProcessed;

            return iErr;
        }


        public static unsafe int confNvrInfo2Stream(int uiStreamId, ConfNvrInfo* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->ucbResp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbResp, (void*)pReq->ucbResp, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->idInfo_requester.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_requester, (void*)&pReq->idInfo_requester.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->nvrInfo.usCnt)
            {

                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_nvr, (void*)&pReq->nvrInfo.ui64Id, 0, &ptr, &len)) goto errLabel;

                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pReq->nvrInfo.usCnt, 0, &ptr, &len)) goto errLabel;

                for (i = 0; i < pReq->nvrInfo.usCnt; i++)

                {

                    IpcInfo* pMem = getMem((IpcInfo*)pReq->nvrInfo.buf_ipcInfo, Consts.bufSize_ipcInfos, i);// &pReq->nvrInfo.mems[i];
                                                                                                            //
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;

                    //
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iChannel, (void*)pMem->iChannel, 0, &ptr, &len)) goto errLabel;

                    if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_name, pMem->name, (uint)mylstrlen(pMem->name), &ptr, &len)) goto errLabel;

                    if (0 != data2Stream(Consts.CONST_qyDataType_str, Consts.CONST_qnmCfgId_ip, pMem->ip, (uint)mystrlen(pMem->ip), &ptr, &len)) goto errLabel;

                    //
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
                }


            }


            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }

        //(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        public static unsafe int tmpHandler_stream2confNvrInfo(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            ConfNvrInfo* pContent = (ConfNvrInfo*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            //
            int uiStreamId_1 = 0;
            if (pCtx->index_streamId > 0)
            {
                uiStreamId_1 = (int)pCtx->uiStreamIds[1];
            }

            //
            bool bProcessed = false;

            //




            short tmp_cfgId;
            tmp_cfgId = (short)pItem->head.cfgId;
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_confNvrInfo:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_ucbResp:
                            if (0 != getFieldData_byte(pItem, (byte*)&pContent->ucbResp)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_grp_related:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_imGrp_related.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_requester:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_requester.ui64Id)) goto errLabel;
                            break;

                        case Consts.CONST_qnmCfgId_idInfo_nvr:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->nvrInfo.ui64Id)) goto errLabel;
                            break;

                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->nvrInfo.usCnt)) goto errLabel;
                            //
                            if (pContent->nvrInfo.usCnt >= Consts.MAX_ipcInfos)
                            {
                                showInfo_open(0, null, _T("stream2ConfNvrInfo"));
                                goto errLabel;
                            }
                            //
                            break;

                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:

                    if (pContent->tmpInternal.usCnt_mems >= Consts.MAX_ipcInfos)
                    {
                        goto errLabel;
                    }
                    //
                    IpcInfo* pMem;


                    pMem = getMem((IpcInfo*)pContent->nvrInfo.buf_ipcInfo, Consts.bufSize_ipcInfos, pContent->tmpInternal.usCnt_mems);




                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_null:
                            pContent->tmpInternal.usCnt_mems++;
                            break;
                        case Consts.CONST_qnmCfgId_iChannel:
                            if (0 != getFieldData_long(pItem, (int*)&pMem->iChannel)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_name:
                            if (0 != getFieldData_wStr(pItem, pMem->name, Consts.cntof_name)) goto errLabel;




                            break;
                        case Consts.CONST_qnmCfgId_ip:
                            if (0 != getFieldData_str(pItem, pMem->ip, Consts.cntof_ip))
                            {
                                goto errLabel;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }



            //
            iErr = 0;
        errLabel:

#if DEBUG
            {
                IpcInfo* pMem = getMem((IpcInfo*)pContent->nvrInfo.buf_ipcInfo, Consts.bufSize_ipcInfos, 0);

                ref IpcInfo tmpMem = ref *pMem;
                //


            }
#endif

            //
            return iErr;
        }



        //  2010/12/27
        public static unsafe int confReq2Stream(uint uiStreamId, CONF_req* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->usOp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp, (void*)pReq->usOp, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->policy.usAvLevel)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usAvLevel, (void*)pReq->policy.usAvLevel, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->policy.conf_iFourcc)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_conf_iFourcc, (void*)pReq->policy.conf_iFourcc, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->policy.usMaxSpeakers)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usMaxSpeakers, (void*)pReq->policy.usMaxSpeakers, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->idInfo_initiator.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_initiator, (void*)&pReq->idInfo_initiator.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2ConfReq(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            CONF_req* pContent = (CONF_req*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            //
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_confReq:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_grp_related:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_imGrp_related.ui64Id)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usOp:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usOp)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usAvLevel:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->policy.usAvLevel)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_conf_iFourcc:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->policy.conf_iFourcc)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usMaxSpeakers:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->policy.usMaxSpeakers)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_idInfo_initiator:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_initiator.ui64Id)) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        ///////////////




        public static unsafe int transferVideoData2Stream(uint uiStreamId, TRANSFER_VIDEO_DATA* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len))
            {
                goto errLabel;
            }


            //  qyShowInfo1(  Consts.CONST_qyShowType_debug,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    transferVideoData2stream: uiTranNo_openAvDev %d"  ),  pReq->uiTranNo_openAvDev  );


            if (0 != pReq->uiTranNo_openAvDev)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openVideoDev, (void*)pReq->uiTranNo_openAvDev, 0, &ptr, &len))
                {
                    goto errLabel;
                }
            }

            //

            //
            if (0 != pReq->usCnt)
            {

                if (pReq->usCnt > Consts.CONST_maxTransferAvDataMems)
                {
                    ref TRANSFER_VIDEO_DATA tmp_pReq = ref *pReq;

                    goto errLabel;
                }

                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pReq->usCnt, 0, &ptr, &len))
                {
                    goto errLabel;
                }
                //
                TRANSFER_VIDEO_dataMemHead* pMems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
                for (i = 0; i < pReq->usCnt; i++)
                {
                    TRANSFER_VIDEO_dataMemHead* pMem = &pMems[i];

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len))
                    {
                        goto errLabel;
                    }

                    //
                    if (0 != pMem->idInfo.ui64Id)
                    {           //  2009/05/31
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pMem->idInfo.ui64Id, sizeof(UInt64), &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }
                    //  2012/01/09
                    if (0 != pMem->uiTranNo_openAvDev)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openVideoDev, (void*)pMem->uiTranNo_openAvDev, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }

                    //
                    if (0 != pMem->uiSampleTimeInMs)
                    {       //  2009/05/04
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSampleTimeInMs, (void*)pMem->uiSampleTimeInMs, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }
                    //  2015/01/15
                    if (0 != pMem->uiPts)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiPts, (void*)pMem->uiPts, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }

                    //
                    if (0 != pMem->ucbKeyFrame)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_byte, Consts.CONST_qnmCfgId_ucbKeyFrame, (void*)pMem->ucbKeyFrame, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }


                    //  2013/11/15

                    //
                    if (0 != pMem->uiLen)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiLen, (void*)pMem->uiLen, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }

                    //   2017/09/05
                    if (0 != pMem->stat.usFps)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usFps, (void*)pMem->stat.usFps, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }
                    if (0 != pMem->stat.usCnt_pkts)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt_pkts, (void*)pMem->stat.usCnt_pkts, 0, &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }
                    if (0 != pMem->stat.tSelectTime)
                    {   //  2008/12/05	  	 
                        //
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tSelectTime, (void*)&pMem->stat.tSelectTime, sizeof(Int64), &ptr, &len))
                        {
                            goto errLabel;
                        }
                    }



                    //
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len))
                    {
                        goto errLabel;
                    }
                }

            }

            if (0 != pReq->uiLen)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_lData, Consts.CONST_qnmCfgId_rawData, (void*)pReq->buf, pReq->uiLen, &ptr, &len))
                {
                    goto errLabel;
                }
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len))
            {
                goto errLabel;
            }

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferVideoData(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_VIDEO_DATA* pContent = (TRANSFER_VIDEO_DATA*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_transferVideoData:

                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_uiTranNo_openVideoDev:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_openAvDev)) goto errLabel;
                            //  qyShowInfo1(  Consts.CONST_qyShowType_debug,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    tmpHandler_stream2TransferVideoData: uiTranNo_openAvDev %d"  ),  pContent->uiTranNo_openAvDev  );
                            break;
                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usCnt)) goto errLabel;
                            //  if  (  pContent->usCnt  >=  mycountof(  pContent->mems  )  )  
                            if (pContent->usCnt > Consts.CONST_maxTransferAvDataMems)  //  2011/01/17
                            {
#if __DEBUG__
						traceLogA("tmpHandler_stream2TransferVideoDataGrp failed: usCnt is too big");
#endif
                                goto errLabel;
                            }
                            TRANSFER_VIDEO_dataMemHead* pMems = (TRANSFER_VIDEO_dataMemHead*)pContent->buf_TRANSFER_VIDEO_dataMemHead_mems;
                            int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
                            //
                            int i;  //  2009/05/20
                            for (i = 0; i < pContent->usCnt; i++)
                            {
                                TRANSFER_VIDEO_dataMemHead* pMem = getMem(pMems, bufSize, i);

                                //memset(&pContent->mems[i], 0, sizeof(pContent->mems[i])  );								 
                                //mymemset((IntPtr)(byte*)&pMems[i], 0, sizeof(TRANSFER_VIDEO_dataMemHead));
                                mymemset((IntPtr)(byte*)pMem, 0, sizeof(TRANSFER_VIDEO_dataMemHead));

                            }
                            break;
                        case Consts.CONST_qnmCfgId_rawData:
                            pContent->uiLen = Consts.bufSize_transferVideoData;
                            if (0 != getFieldData_lData(pItem, pContent->buf, &pContent->uiLen)) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;

                case Consts.CONST_imCommType_mem:
                    {
                        TRANSFER_VIDEO_dataMemHead* pMem = null;
                        //
                        if (pContent->tmpInternal.usCnt_mems >= Consts.CONST_maxTransferAvDataMems)
                        {
#if __DEBUG__
		traceLogA("tmpHandler_stream2TransferVideoDataGrp failed: usCnt_mems is too big");
#endif
                            goto errLabel;
                        }
                        //
                        TRANSFER_VIDEO_dataMemHead* pMems = (TRANSFER_VIDEO_dataMemHead*)pContent->buf_TRANSFER_VIDEO_dataMemHead_mems;
                        int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
                        //
                        pMem = getMem(pMems, bufSize, pContent->tmpInternal.usCnt_mems);// &pMems[pContent->tmpInternal.usCnt_mems];
                                                                                        //
#if DEBUG
                        ref TRANSFER_VIDEO_dataMemHead tmp_pMem = ref *pMem;
#endif
                        //
                        switch (tmp_cfgId)
                        {
                            case Consts.CONST_qnmCfgId_start:
                                break;
                            //
                            case Consts.CONST_qnmCfgId_messengerId:            //  2009/05/31
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->idInfo.ui64Id)) goto errLabel;
                                break;
                            //  2012/01/09
                            case Consts.CONST_qnmCfgId_uiTranNo_openVideoDev:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiTranNo_openAvDev)) goto errLabel;
                                break;
                            //
                            case Consts.CONST_qnmCfgId_uiSampleTimeInMs:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiSampleTimeInMs)) goto errLabel;
                                break;
                            //  2015/01/15
                            case Consts.CONST_qnmCfgId_uiPts:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiPts)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_ucbKeyFrame:
                                if (0 != getFieldData_byte(pItem, (byte*)&pMem->ucbKeyFrame)) goto errLabel;
                                break;

                            //
                            //
                            case Consts.CONST_qnmCfgId_uiLen:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiLen)) goto errLabel;
                                break;
                            //
                            //  2017/09/05			
                            case Consts.CONST_qnmCfgId_usFps:
                                if (0 != getFieldData_short(pItem, (short*)&pMem->stat.usFps)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_tLastTime_showFrameInfo_020534:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->stat.tLastTime_showFrameInfo_020534)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_usCnt_pkts:
                                if (0 != getFieldData_short(pItem, (short*)&pMem->stat.usCnt_pkts)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_tSelectTime:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->stat.tSelectTime)) goto errLabel;
                                break;

                            //
                            case Consts.CONST_qnmCfgId_null:
                                pContent->tmpInternal.usCnt_mems++;
                                break;
                            default:
                                {
                                    int ii = 0;
                                }
                                break;
                        }
                        //

                    }
                    break;


                default:
                    break;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }



        public static unsafe int transferVideoDataResp2Stream(uint uiStreamId, TRANSFER_VIDEO_dataResp* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;


            //  qyShowInfo1(  Consts.CONST_qyShowType_debug,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    transferVideoData2stream: uiTranNo_openAvDev %d"  ),  pReq->uiTranNo_openAvDev  );


            //  2017/09/05
            if (0 != pReq->idInfo.ui64Id)
            {           //  2009/05/31
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pReq->idInfo.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->uiTranNo_openAvDev)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openVideoDev, (void*)pReq->uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->usFps)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usFps, (void*)pReq->usFps, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tSelectTime)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tSelectTime, (void*)&pReq->tSelectTime, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->usCnt_pkts)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt_pkts, (void*)pReq->usCnt_pkts, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->usDiff_pkts)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usDiff_pkts, (void*)pReq->usDiff_pkts, 0, &ptr, &len)) goto errLabel;
            }


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferVideoDataResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_VIDEO_dataResp* pContent = (TRANSFER_VIDEO_dataResp*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferVideoDataResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_messengerId:  //  2017/0
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo.ui64Id)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiTranNo_openVideoDev:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_openAvDev)) goto errLabel;
                    //  qyShowInfo1(  Consts.CONST_qyShowType_debug,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    tmpHandler_stream2TransferVideoData: uiTranNo_openAvDev %d"  ),  pContent->uiTranNo_openAvDev  );
                    break;
                case Consts.CONST_qnmCfgId_usFps:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usFps)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_tSelectTime:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tSelectTime)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usCnt_pkts:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usCnt_pkts)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usDiff_pkts:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usDiff_pkts)) goto errLabel;
                    break;

                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }



        public static unsafe int transferAudioData2Stream(uint uiStreamId, TRANSFER_AUDIO_DATA* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;


            //  qyShowInfo1(  Consts.CONST_qyShowType_debug,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    transferVideoData2stream: uiTranNo_openAvDev %d"  ),  pReq->uiTranNo_openAvDev  );


            if (0 != pReq->uiTranNo_openAvDev)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openAudioDev, (void*)pReq->uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tLastTime_showFrameInfo_020534)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tLastTime_showFrameInfo_020534, (void*)&pReq->tLastTime_showFrameInfo_020534, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->usCnt_pkts)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt_pkts, (void*)pReq->usCnt_pkts, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->usCnt)
            {

                if (pReq->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt, (void*)pReq->usCnt, 0, &ptr, &len)) goto errLabel;
                //
                for (i = 0; i < pReq->usCnt; i++)
                {
                    TRANSFER_AUDIO_dataMemHead* pMems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
                    TRANSFER_AUDIO_dataMemHead* pMem = &pMems[i];

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_mem, 0, &ptr, &len)) goto errLabel;

                    //

                    //
                    if (0 != pMem->idInfo.ui64Id)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pMem->idInfo.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
                    }

                    //
                    if (0 != pMem->uiTranNo_openAvDev)
                    {   //  2013/08/04
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openAudioDev, (void*)pMem->uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
                    }

                    //
                    if (0 != pMem->uiSampleTimeInMs)
                    {   //  2009/05/04
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSampleTimeInMs, (void*)pMem->uiSampleTimeInMs, 0, &ptr, &len)) goto errLabel;
                    }
                    if (0 != pMem->ui_rtTimeLen)
                    {       //  2009/05/27
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_ui_rtTimeLen, (void*)pMem->ui_rtTimeLen, 0, &ptr, &len)) goto errLabel;
                    }

                    //  2015/09/10
                    if (0 != pMem->uiPts)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiPts, (void*)pMem->uiPts, 0, &ptr, &len)) goto errLabel;
                    }

                    //
                    //  if  (  pMem->uiLen  )  {
                    //	  if  (  data2Stream(  Consts.CONST_qyDataType_lData,  Consts.CONST_qnmCfgId_rawData,  (  void  *  )pMem->buf,  pMem->uiLen,  &ptr,  &len  )  )  goto  errLabel;
                    //  }			  
                    if (0 != pMem->uiLen)
                    {
                        if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiLen, (void*)pMem->uiLen, 0, &ptr, &len)) goto errLabel;
                    }

                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
                }

            }

            if (0 != pReq->uiLen)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_lData, Consts.CONST_qnmCfgId_rawData, (void*)pReq->buf, pReq->uiLen, &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferAudioData(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_AUDIO_DATA* pContent = (TRANSFER_AUDIO_DATA*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_transferAudioData:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_uiTranNo_openAudioDev:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_openAvDev)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_tLastTime_showFrameInfo_020534:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tLastTime_showFrameInfo_020534)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usCnt_pkts:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usCnt_pkts)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_usCnt:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usCnt)) goto errLabel;

                            if (pContent->usCnt > Consts.CONST_maxTransferAvDataMems) goto errLabel;

                            //  ÏÂÃæÒª½øÐÐ³õÊ¼»¯£¬·ÅÔÚÕâÀïÊÇÒòÎªµ£ÐÄÏÂÃæµÄÊý¾ÝÈç¹û³ö´í£¬usCnt½«°üº¬ÁËÒ»Ð©´íÎóÊý¾Ý
                            TRANSFER_AUDIO_dataMemHead* pMems = (TRANSFER_AUDIO_dataMemHead*)pContent->buf_TRANSFER_AUDIO_dataMemHead_mems;
                            int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
                            //
                            int i;
                            for (i = 0; i < pContent->usCnt; i++)
                            {
                                TRANSFER_AUDIO_dataMemHead* pMem = getMem(pMems, bufSize, i);

                                mymemset((IntPtr)(byte*)pMem, 0, sizeof(TRANSFER_AUDIO_dataMemHead));
                            }
                            break;
                        case Consts.CONST_qnmCfgId_rawData:
                            pContent->uiLen = Consts.bufSize_transferAudioData;// sizeof(pContent->buf  );
                            if (0 != getFieldData_lData(pItem, pContent->buf, &pContent->uiLen)) goto errLabel;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_mem:
                    {
                        TRANSFER_AUDIO_dataMemHead* pMem = null;
                        //
                        if (pContent->tmpInternal.usCnt_mems >= Consts.CONST_maxTransferAvDataMems)
                        {
                            traceLogA("tmpHandler_stream2TransferAudioData failed: usCnt_mems is too big");
                            goto errLabel;
                        }
                        //
                        TRANSFER_AUDIO_dataMemHead* pMems = (TRANSFER_AUDIO_dataMemHead*)pContent->buf_TRANSFER_AUDIO_dataMemHead_mems;
                        int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
                        //
                        pMem = getMem(pMems, bufSize, pContent->tmpInternal.usCnt_mems);// &pMems[pContent->tmpInternal.usCnt_mems];
                        switch (tmp_cfgId)
                        {
                            //
                            //
                            case Consts.CONST_qnmCfgId_messengerId:
                                if (0 != getFieldData_l64(pItem, (Int64*)&pMem->idInfo.ui64Id)) goto errLabel;
                                break;

                            //
                            case Consts.CONST_qnmCfgId_uiTranNo_openAudioDev:      //  2013/08/04
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiTranNo_openAvDev)) goto errLabel;
                                break;

                            //
                            case Consts.CONST_qnmCfgId_uiSampleTimeInMs:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiSampleTimeInMs)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_ui_rtTimeLen:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->ui_rtTimeLen)) goto errLabel;
                                break;

                            //  2015/09/10
                            case Consts.CONST_qnmCfgId_uiPts:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiPts)) goto errLabel;
                                break;

                            //
                            //  case  Consts.CONST_qnmCfgId_rawData:
                            //	     pMem->uiLen  =  sizeof(  pMem->buf  );
                            //	     if  (  getFieldData_lData(  pItem,  pMem->buf,  &pMem->uiLen  )  )  goto  errLabel;
                            //	     break;
                            case Consts.CONST_qnmCfgId_uiLen:
                                if (0 != getFieldData_long(pItem, (int*)&pMem->uiLen)) goto errLabel;
                                break;
                            case Consts.CONST_qnmCfgId_null:
                                pContent->tmpInternal.usCnt_mems++;
                                break;
                            default:
                                break;
                        }
                        //

                    }
                    break;
                default:
                    break;
            }


            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int transferAudioDataResp2Stream(uint uiStreamId, TRANSFER_AUDIO_dataResp* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->uiTranNo_openAvDev)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openAudioDev, (void*)pReq->uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->usCnt_pkts)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usCnt_pkts, (void*)pReq->usCnt_pkts, 0, &ptr, &len)) goto errLabel;
            }


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferAudioDataResp(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_AUDIO_dataResp* pContent = (TRANSFER_AUDIO_dataResp*)p1;
            //  QY_CFGITEM					*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_transferAudioDataResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                case Consts.CONST_qnmCfgId_uiTranNo_openAudioDev:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_openAvDev)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usCnt_pkts:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usCnt_pkts)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }



        public static unsafe int qyhtonImContentU(int lhClientVer, IM_CONTENTU* p)
        {
            int len = -1;

            switch (p->uiType)
            {
                case Consts.CONST_imCommType_simpleResp:
                    return qyhtonImSimpleResp(lhClientVer, &p->simpleResp);
                //
#if false
			case  Consts.Consts.CONST_imCommType_streamContent:
				  return  qyhtonImStreamContent(  lhClientVer,  &p->stream  );
#endif

                case Consts.CONST_imCommType_lStream:
                    return qyhtonImLStream(lhClientVer, &p->lStream);

                case Consts.CONST_imCommType_htmlContent:
                    traceLogA("qyhtonImMsgContentU failed, htmlContent");
                    break;
                default:
                    {
                        string str;
                        str = string.Format("qyhtonImMsgContentU failed, unknown uiType {0}", p->uiType);
                        traceLog(str);
                    }
                    break;
            }

            //  errLabel:
            return len;
        }

        public static unsafe int qyntohImContentU(int lhClientVer, byte* src, IM_CONTENTU_nh* p)
        {
            int len = -1;
            uint uiType = 0;

            //  uiType  =  qyntohl(  p->uiType  );
            mymemcpy((IntPtr)(byte*)&uiType, (IntPtr)src, sizeof(uint));
            uiType = qyntohi(uiType);

            switch (uiType)
            {
                case Consts.CONST_imCommType_simpleResp:
                    mymemcpy((IntPtr)(byte*)&p->simpleResp, (IntPtr)src, (uint)sizeof(IM_SIMPLE_RESP));
                    return qyntohImSimpleResp(lhClientVer, &p->simpleResp);
                //
#if false
			case  CONST_imCommType_streamContent:
				  //  return  qyntohImStreamContent(  lhClientVer,  &p->stream  );					//  2010/07/10. 这里应该设计一个专门的IM_CONTENTU_ntoh
				  return  qyntohImStreamContent(  lhClientVer,  src,  &p->stream  );				//  
#endif

                case Consts.CONST_imCommType_htmlContent:
                    traceLogA("qyntohImMsgContentU failed, htmlContent");
                    break;
                case Consts.CONST_imCommType_lStream:
                    //  return  qyntohImLStream(  lhClientVer,  &p->lStream  );					//  2010/07/10. 这里应该设计一个专门的IM_CONTENTU_ntoh
                    return qyntohImLStream(lhClientVer, src, &p->lStream);                  //  
                default:
                    {
                        string str;
                        str = string.Format("qyntohImMsgContentU failed, unprocessed type {0}", uiType);
                        traceLog(str);
                    }
                    break;
            }

            //  errLabel:
            return len;
        }


        public static bool isAvFlg_unresizable(byte ucAvFlg)
        {
            return (0 != ((ucAvFlg) & Consts.CONST_avFlg_unresizable));
        }


        public static unsafe void MACRO_makeImSimpleResp(ushort usCmd_org1, uint uiContentType1, int iStatus1, out IM_SIMPLE_RESP respSimple)
        {
            fixed (IM_SIMPLE_RESP* pResp = &respSimple)
            {
                mymemset((IntPtr)(byte*)pResp, 0, sizeof(IM_SIMPLE_RESP));
            }
            respSimple.uiType = Consts.CONST_imCommType_simpleResp;
            respSimple.usCmd_org = usCmd_org1;
            respSimple.uiContentType_org = uiContentType1;
            respSimple.iStatus = iStatus1;
        }

        //
        public static unsafe void M_set_pktId_alloc(myDRAW_VIDEO_DATA* pPkt)
        {
            int tt = 0;
            (pPkt)->lPktId_alloc = (int)getuiNextTranNo(null, 0, ref tt);
        }

        public static unsafe void M_set_pktId_getNewPkt(myPLAY_AUDIO_DATA* pPkt)
        {
            int tt = 0;
            (pPkt)->lPktId_getNewPkt = (int)getuiNextTranNo(null, 0, ref tt);
        }

        public static unsafe void M_set_pktId_getNewPkt(PKT_info_toDec* pPkt)
        {
            int tt = 0;
            (pPkt)->lPktId_getNewPkt = (int)getuiNextTranNo(null, 0, ref tt);
        }


        public static unsafe bool bQThreadExists(QY_qThreadProcInfo_common qThread)
        {
            if (qThread == null) return false;

            if ((qThread).hThread != null) return true;
            return false;

        }

        public static unsafe void waitForQThread(QY_qThreadProcInfo_common pQThread, uint dwMilliSeconds)
        {
            //waitForThread( ref pQThread.hThread, dwMilliSeconds);
            GuiShare.pf_waitForThread1(ref pQThread.hThread, dwMilliSeconds);
            return;
        }


        public static QY_TRANSFORM MACRO_getTransform(ref PROC_TASK_AV task, int index_activeMems_from)
        {
            return ((index_activeMems_from < 0 || index_activeMems_from >= (task).trans.usCntLimit_transforms) ? null : (task).trans.pTransforms[index_activeMems_from]);
        }


        public static QIS_DISPATCH MACRO_getDispatch(ref PROC_TASK_AV task, int index_activeMems_from)
        {
            return ((index_activeMems_from < 0 || index_activeMems_from >= (task).usCntLimit_dispatchs) ? null : (task).pDispatchs[index_activeMems_from]);
        }

        public static unsafe void M_setStep(ref MY_DEBUG_STEP pDebugStep, int nStep)
        {
            {
                (pDebugStep).nStep_debug = (nStep);
                (pDebugStep).dwTickCnt_step_debug = myGetTickCount(null);
            }
            return;
        }
        //#define M_setStep_fg(  pDebugStep,  nStep  )			{  if  (  pDebugStep  )  {  (  pDebugStep  )->fg.FillBuffer.nStep_debug  =  (  nStep  );  (  pDebugStep  )->fg.FillBuffer.dwTickCnt_step_debug  =  GetTickCount(  );  }  }


        public static void M_setMyStep(ref MY_DEBUG_STEP pDebugStep, int nMyStep)
        {   //MACRO_qyAssert(  nMyStep  <  800,  _T(  "nMyStep err. must < 800"  )  );  
            M_setStep(ref pDebugStep, 100 + nMyStep);
        }






    }


}







