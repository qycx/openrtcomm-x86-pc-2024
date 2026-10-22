using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const int MAX_sharedObjUsrs = 8;

	public const int MAX_taskRespInfoMems = 360;

	//
	public const int CONST_capType_null = 0;
	public const int CONST_capType_av = 1;
	public const int CONST_capType_screen = 2;
	public const int CONST_capType_mediaFile = 3;
	public const int CONST_capType_mediaDevice = 4;         //  2011/12/28
															//
	public const int CONST_capType_mosaic = 20;         //  2013/12/10. d3dWall的欢迎字幕，或者note等
														//
	public const int CONST_capType_slave = 25;          //  2016/04/06

	//
	public const int CONST_capType_qvcf = 30;           //  2015/05/25, 本地播放qvcf文件，不需要压缩，不需要resize


	//
	public const int CONST_subCapType_null = 0;
	public const int CONST_subCapType_webcam = 1;
	public const int CONST_subCapType_mosaic_video = 2;         //  2014/12/01
	public const int CONST_subCapType_mosaic_resource = 3;          //  2014/12/01
																	//
	public const int CONST_subCapType_unresizable = 10;         //  2015/03/11
																//
	public const int CONST_subCapType_slave_video = 20;         //  2016/04/06	




	//  2014/11/13
	public const int CONST_capUsage_null = 0;
	public const int CONST_capUsage_conf = 1;
	public const int CONST_capUsage_3dConf = 2;         //  2014/12/15
	public const int CONST_usage_mosaic_sys = 3;            //  2014/12/21

	//
	public const int CONST_camCapType_null = 0;
	public const int CONST_camCapType_vfw = 1;
	public const int CONST_camCapType_directX = 2;
	public const int CONST_camCapType_rtsp = 3;     //  2014/02/23
													//
	public const int CONST_camCapType_qvcf = 4;     //  2015/05/11

	//
	public const int CONST_camCapType_android = 20;

	//
	public const int CONST_camCapType_slave = 50;       //  2014/09/29. 虚拟的流捕获，实际是另一个流的resize

	//
	public const int CONST_camCapType_mosaic = 55;      //  2014/10/30

	//
	public const int CONST_camCapType_custom = 100;     //  2013/03/18. 2014/09/29


	//
	//  2010/08/03
	public const int CONST_recordType_null = 0;
	public const int CONST_recordType_waveform = 1;
	public const int CONST_recordType_dSound = 2;       //  2013/03/18
	public const int CONST_recordType_directX = 3;      //  2013/03/18
	public const int CONST_recordType_aec = 4;      //  2013/05/22
													//
	public const int CONST_recordType_coreAudio = 5;        //  2017/01/28
															//
	public const int CONST_recordType_webRtc = 7;       //  2017/06/15

	public const int CONST_recordType_android = 8;
														//
	public const int CONST_recordType_custom = 10;      //  2013/03/18


	//  2013/03/19
	public const int CONST_sharedObjType_null = 0;
	public const int CONST_sharedObjType_av = 101;
	public const int CONST_sharedObjType_mediaFile = 102;
	public const int CONST_sharedObjType_mediaDevice = 103;
	public const int CONST_sharedObjType_webcam_vOnly = 104;
	public const int CONST_sharedObjType_mosaic = 105;                  //  2014/12/01	
	public const int CONST_sharedObjType_qvcf = 106;                    //  2015/05/11
	public const int CONST_sharedObjType_webcam_av = 107;                   //  2015/05/18
	public const int CONST_sharedObjType_rtsp = 108;                    //  2015/10/11

	//
	public const int CONST_sharedObjSubtype_null = 0;
	public const int CONST_sharedObjSubtype_slave = 10;
	public const int CONST_sharedObjSubtype_mosaic = 11;

	//
	public const int MAX_cnt_preEnc_array = 30;

	//




}



namespace qy
{

	//
	//
	public unsafe struct CompressVideo_encInfo
	{
		//  2014/04/22
		public fixed int iSampleTimeInMs_preEnc_array[Consts.MAX_cnt_preEnc_array];
		public int iNextIndex_preEnc;
		public uint cnt_preEnc;
		//
		public int iNextIndex_postEnc;
		public int iSampleTimeInMs_postEnc;
		public uint cnt_postEnc;

		//  2014/08/15
		public uint dwTickCnt_startToForceKeyFrame;


	};

	public struct CompressVideo_debugInfo
	{
		public Int64 tStart;
		public int nStep;
	};


	public unsafe struct COMPRESS_VIDEO
	{

		public bool bVideoCompressInited;                                      //  

		//  
		public uint uiTranNo_openAvDev_org;                                        //  2010/09/04

		//
		public int left, top;                                                  //  
		public int width, height;

		//
		public VIDEO_COMPRESSOR_CFG compressor;                                                    //  2009/03/27


		//  2012/09/15
		public uint uiModuleType_toEnc;

		//  2015/10/13
		public bool ucbUseEvt;


		//QDC_OBJ_INFO								qdcObjInfo;													//  2015/10/11	//  custom;
		public QOI_encV u_qoi;                                                       //  2016/04/19

		//
		public QY_VIDEO_HEADER vh_decompress;                                              //  2009/03/29

		//
		public QY_VIDEO_HEADER vh_compress;
		public VH_stream vh_stream;                                                    //  2014/02/25

		//  2014/05/03
		public bool ucbResizeByEncoder;

		//
		public int iCompressBufSize;
		public byte* pCompressBuf;

		//
		public TRANSFER_VIDEO_DATA* pReqBuf_myVideoCapture;                                        //  myVideoCapure

		//  2014/04/02
		public myDRAW_VIDEO_DATA* pCurPkt;
		//
		public GENERIC_Q pInDataCacheQ;
		public GENERIC_Q pTmpDataQ;


		// 
		public bool ucbFixedRate;                                             //  

		//
		public byte ucSeconds_perFrame_expected;                              //  
		public ushort usFrames_perSecond_expected;                             //  
		public Int64 tSelectTime;                                             //  
		public uint dwTickCnt_selectTime;                                     //  
		public uint uiCnt_pkts_fromSelect;                                     //  SelectTime							
		public float fFps_sending;                                             //  uiCnt_pkts_fromSelect / (  GetTickCount(  )  -  dwTickCnt_selectTime  )
																			   //
		public bool tmp_bMediaChannelReady;                                        //  
		public bool tmp_ucbNotSendData;                                           //  2011/07/29, 仅用于记录

		public byte ucTimeoutInS_showFrameInfoResp;                               //  showFrameInfo
																				  //
		public VIDEO_input_info input;
		//
		public Int64 tLastTime_sendVideo;                                     //  
		public ushort usFramesThisSecond_toSend;                                   //  
		public ushort usFps_toSend;
		//
		public Int64 tLastTime_post;                                              //  2011/12/07

		//
		public Int64 tLastTime_showFrameInfo;                                 //  

		//  
		public Int64 tFirstTime_showFrameInfo_withoutResp;                        //  
		public Int64 tRecvTime_resp_fromCli;

		//
		public ushort usFps_ok;                                                    //  
		public bool bTopFrames;                                                    //  
																				   //
		public ushort usTimes_ok;
		public ushort usTimes_failure;                                         //  
																			   //
																			   //  
																			   //	
																			   //	Frames_perSecond_expected usFrames_perSecond_ok
																			   //  usFrames_perSecond_ok.ÐèÒªÓÐusTimes_noErr
																			   //  usTimes_err
																			   //  




		public CompressVideo_encInfo encInfo;

//  2010/12/28
//TCHAR tag_dbg[64];      //  2016/04/02
		public CompressVideo_debugInfo debugInfo;
		public int iVal_dbg;           //  2017/09/05

		//
		public void* pData;

		//
		public void memset0()
        {
			this.bVideoCompressInited = false;

			//
			encInfo = new CompressVideo_encInfo();

			//
        }
}		 ;



	public struct CompressAudio_encInfo_st
	{
		public uint last_iSampleTimeInMs_preEnc;
		public uint uiTimeLen_preEnc;
		//
		public uint lastEnd_iSampleTimeInMs_postEnc;
	};

	public struct CompressAudio_encInfo
	{
		//  2014/04/22
		//uint							cnt_preEnc;
		//
		public uint cnt_postEnc;

		//
		//  2016/12/15
		public CompressAudio_encInfo_st st;

				 };

public unsafe struct COMPRESS_AUDIO
	{

		public bool bCompressInited;

		//  uint									uiTranNo_openAvDev;										//  tranNo
		public uint uiTranNo_openAvDev_org;                                        //  tranNo. 2010/09/04
		public AUDIO_COMPRESSOR_CFG compressor;

		public uint uiModuleType_toEnc;                                            //  2015/02/14

		//

		//QDC_OBJ_INFO								custom;
		public QOI_encA u_qoi;               //  2016/04/19

		//
		public QY_AUDIO_HEADER ah_decompress;                                              //  2009/05/16
		public QY_AUDIO_HEADER ah_pcm;                                                     //  2009/05/16
		public QY_AUDIO_HEADER ah_compress;                                                //  2009/05/16
																						   //  2016/12/17
		public AH_stream ah_stream;                                                    //  2016/12/16

		//
		public byte* pPcmBuf;
		public uint dwPcmBufSize;
		public byte* pDstBuf;
		public uint dwDstBufSize;

		public TRANSFER_AUDIO_DATA* pReqCache;                                                 //  2008/05/25


		//  2015/02/13
		public myPLAY_AUDIO_DATA* pCurPkt;
		//
		public GENERIC_Q pInDataCacheQ;


		//
		public byte ucTimeoutInS_showFrameInfoResp;                               //  ´Ó·¢³öshowFrameInfoµ½µÃµ½respµÄ³¬Ê±



		// 
		public Int64 tSelectTime;                                             //  ÐÞ¸ÄÖ¡ËÙÊ±¼ä					

		public ushort usCnt_pkts;                                                  //  ´ÓÉÏÒ»¸ö

		//
		public Int64 tLastTime_sendAudio;                                     //  ·¢°üµÄÊ±¼ä					 
		public Int64 tLastTime_showFrameInfo;                                 //  ÉÏÒ»´ÎµÄ·¢ËÍÖ¡ËÙÐÅÏ¢

		//
		public Int64 tFirstTime_showFrameInfo_withoutResp;
		public Int64 tRecvTime_resp_fromCli;

//
//  2015/09/10
	public CompressAudio_encInfo encInfo;


		//
		public void memset0()
        {
			this.bCompressInited = false;
        }


}		 ;


public unsafe class CAP_procInfo_bmpCommon
	{
		public uint uiType;
		public int iIndex_sharedObj;
		public uint uiTranNo_sharedObj;
		public object curhWnd;
		public QY_VIDEO_HEADER vh_org;
		//
		public int mSensorOrientation;  //  此变量用于，是否需要额外的旋转处理。对于dlgTalk_av，从img得到的需要旋转，所以此变量要赋值。
										// 对dlgTalk_av2, 纹理已经旋转，不需要额外处理，所以此值不能赋值.
		//
		public uint dwTickCnt_lastVideoGot;
		public object hicOrg2Rgb;
		//public QY_VIDEO_HEADER vh_rgb;
		public QY_VIDEO_HEADER vh_int;
		public int iOrg2IntBufSize;
		public byte * pOrg2IntBuf;
		public bool bOrg2IntBufInited;
		public QY_qThreadProcInfo_common vppThread;
		public QY_qThreadProcInfo_common thread;
		public QY_qThreadProcInfo_common postThread;
		public COMPRESS_VIDEO compressVideo;

		//
		public bool m_bFront;

		//
		public CAP_procInfo_bmpCommon()
        {
			//
			vppThread = new QY_qThreadProcInfo_common();
			thread = new QY_qThreadProcInfo_common();
			postThread = new QY_qThreadProcInfo_common();
        }

		//
		public void memset0()
        {
			this.uiType = 0;
			this.iIndex_sharedObj=0;
			this.uiTranNo_sharedObj=0;
			this.curhWnd=null;
			this.vh_org = new QY_VIDEO_HEADER();		
			//
			this.mSensorOrientation=0;		
			//
			this.dwTickCnt_lastVideoGot=0;
			this.hicOrg2Rgb=null;
			//public QY_VIDEO_HEADER vh_rgb;
			this.vh_int = new QY_VIDEO_HEADER(); 
			this.iOrg2IntBufSize=0;
			this.pOrg2IntBuf=null;
			this.bOrg2IntBufInited=false;

			this.vppThread.memset0();
			this.thread.memset0();
			this.postThread.memset0();
			this.compressVideo.memset0();

			//
			m_bFront = false;
	}

};

	/*
	typedef struct __capProcInfo_bmpCommon_t
	{
		MACRO_capProcInfo_bmpMems
	}
	CAP_procInfo_bmpCommon;		
	

typedef struct __capProcInfo_audioCommon_t
	{
		MACRO_capProcInfo_audioMems
	}
	CAP_procInfo_audioCommon;
	*/


	public class CAP_procInfo_audioCommon
	{
		public uint uiType;
		public int iIndex_sharedObj;
		public uint uiTranNo_sharedObj;
		public WAVEFORMATEX wf_org;
		public uint dwTickCnt_lastAudioGot;
		public QY_qThreadProcInfo_common appThread;
		public QY_qThreadProcInfo_common thread;
		public COMPRESS_AUDIO compressAudio;

		//
		public int iRecorder_audioSessionId;

		public CAP_procInfo_audioCommon()
        {
			appThread = new QY_qThreadProcInfo_common();
			thread = new QY_qThreadProcInfo_common();
		}

		//
		public void memset0()
        {
			this.uiType = 0;
			this.iIndex_sharedObj = 0;
			this.uiTranNo_sharedObj = 0;
			this.wf_org = new WAVEFORMATEX();
			this.dwTickCnt_lastAudioGot = 0;
			this.appThread.memset0();
			this.thread.memset0();
			this.compressAudio.memset0();

        }

	};



	public struct CAP_procInfo_video_capDev
	{
		public IntPtr pVideoCapDev;

		//
		public void memset0()
        {
			this.pVideoCapDev = IntPtr.Zero;
        }

	};

	public struct CAP_procInfo_video_policy
	{
		public BITMAPINFOHEADER bih;
		//
	};


	public class CAP_procInfo_video: CAP_procInfo_bmpCommon
	{
		

		//
		public uint uiCamCapType;

		//  2017/09/13
		//TCHAR displayName[256 + 1];
		public string displayName;

		//
		public CAP_procInfo_video_capDev capDev;

//
		public CAP_procInfo_video_policy policy;

		//
		public bool bCapDevConnected;
		public bool bCaptureStarted;
		public uint dwTickCnt_captureStarted;     //  2017/09/19

		//  2017/09/14
		public bool bCaptureStarted_openLocalVideo;


}		 ;

	public unsafe struct CAP_procInfo_recordSound_cache
	{

		int bufSize_pAudioDataBuf; //  =  pProcInfo->cfg.uiBufSize_myPlayAudioData;
		byte* pAudioDataBuf;            //   if  (  !(  pAudioDataBuf  =  (  char  *  )mallocObj_pAudioDataBuf.mallocf(  bufSize_pAudioDataBuf  )  )  )  goto  errLabel;

		//
		int nBytes_toRecord;    //  =  pRecordSoundProcInfo->compressAudio.compressor.uiBytesRecorded_perBlockAlign  *  pRecordSoundProcInfo->compressAudio.ah_decompress.wfx.nBlockAlign;
		int nAvgBytesPerSec;    //  =  pRecordSoundProcInfo->compressAudio.ah_decompress.wfx.nAvgBytesPerSec;

		//
		int dataLen_pAudioDataBuf;

		//
		void* pMsgBuf;

	};

	//
	public struct CapProcInfoRecordSound_u_android
	{
		public object pAudioCapDev;
	};


	public unsafe class CAP_procInfo_recordSound: CAP_procInfo_audioCommon
{


		public uint uiRecordType;                                             //  2010/08/03

		//  2013/03/18
		/*
		union											{
					 struct                                     {
						 void* pAudioCapDev;
	}
	dx;
					 struct                                     {
						 void* pObj;
}
webRtc;
				 }												u;
		*/
		public CapProcInfoRecordSound_u_android u_android;

//  2011/10/24
//GUID guid_aDevice;                                              //  被选中的audio device
//char capDevDesc[64];                                               //  2013/05/25
//char aec_rendDevDesc[64];

//  2013/08/02. 录制的线程是这里, 将audioMems.thread留给压缩
public QY_qThreadProcInfo_common recThread;

		//  2009/02/13
		//object hEvents[CONST_numCaptureEvents + 1];

		//
		public bool bLocalAudioPlaying;                                            //  

		//  
		public uint dwTickCnt_sendLocalAv;                                        //  2010/05/04

		//
		public ushort usErrors_processInput;                                       //  2009/05/09
																				   //
		public bool bInputErr_aec;                                             //  2013/05/26. 如果aec没有本地player在运行,就会出错
		public uint dwStartTickCnt_inputErr;                                  //  2015/02/06. 如果inputErr持久超过5秒，就要退出线程。用户可以在申请发言时，重新初始化aec
																			  //
		public uint dwLastTickCnt_toStartLocalAudioPlayer;                        //  2013/05/26

		//
		public ushort usTimes_ok;                                                  //  
		public ushort usTimes_failure;                                         //  

		//  2013/03/18
		public bool bCapDevConnected;
		public bool bCaptureStarted;

		//
		public CAP_procInfo_recordSound_cache cache;

		//
		public CAP_procInfo_recordSound()
        {
			this.recThread = new QY_qThreadProcInfo_common();
        }

}		 ;


public struct ROUTE_sendLocalAv_routeInfo
	{                                                            //  2009/09/11
		public Int64 tModifiedTime;
		public Int64 tRecvTime_lastResp;
		/*
	struct                                     {
					 Int64 tModifiedTime;
	Int64 tRecvTime_lastResp;
}
mems[CONST_maxMsgrs_sendTo];
		*/
	};

	//
	public struct ROUTE_sendLocalAv
	{

		public int iIndex_misCnt;                                  //  
		public MSG_ROUTE route;
		public QY_MESSENGER_ID videoConference_idInfo_to;                      //  2007/11/30, 

		//  2017/09/17
	public ROUTE_sendLocalAv_routeInfo routeInfo;

}		 ;                                     // 


//
public struct TASK_respInfo_mem
	{

		//
		QY_MESSENGER_ID idInfo;

		/*
		//
		struct                                             {

					 //
		struct                                         {
						 //
		Int64 tRecvTime_lastResp;                         //  2009/09/11
	}
	avReply;
					 //
					 struct                                         {
						 //
	Int64 tRecvTime_lastResp;                         //  2009/09/11
}
taskProcReq_cancel;


//
struct                                         {
						 Int64 tSelectTime;

uint uiCnt_pkts_fromSelect;                     //  ´tSelectTime
int iDiff_pkts_fromSelect;

ushort usDelay;                                 //  
Int64 tRecvTime_lastResp;							//  2009/09/11
					 }												audio;

//				 
struct                                         {
						 Int64 tSelectTime;

uint uiCnt_pkts_fromSelect;                     //  ´tSelectTime
int iDiff_pkts_fromSelect;

ushort usDelay;                                 //  
Int64 tRecvTime_lastResp;                          //  2009/09/11

//
struct                                     {
							 QY_MESSENGER_ID idInfo;
uint uiTran_openAvDev;

Int64 tSelectTime;
//
uint uiCnt_pkts_fromSelect;                     //  ´tSelectTime						 
uint iDiff_pkts_fromSelect;

Int64 tRecvTime_lastResp;
						 }											lastErr;

		
				 	 }												video;


//				
ushort usState_byDlgTalk;
uint dwLastModifiedTickCnt_usState;

//					
bool bNeedCalc_byProcTaskDataResp;

//
CONF_logs logs;

				 }													var;
		*/

}		 ;

//  comments
//  procTaskVideoDataResp计算出iState, 如果与istate_byDlgTalk不符，那么设置taskRespInfoMem里的bNeedCalc
//  然后再设置taskRespInfo里的bNeedCalc
//  dlgTalk.timer, 发现taskRespInfo里的bNeedCalc,就去遍历所有的mems
//  发现taskRespInfoMem里的bNeedcalc, 就去计算iState. 然后清除mem.bNeedcalc.再清除respInfo.bNeedCalc
//  dlgTalk在某个时间，会遍历计算所有的mems,因为在客户端离线后，就不会自动计算了。

//


//  2017/09/06
public  struct TASK_respInfo
{	
		ushort usCnt;
		
		TASK_respInfo_mem[] mems;	//[MAX_taskRespInfoMems];                                   //  2009/09/11. mems_to[3];
			
		//	
		bool bNeedCalc_byProcTaskDataResp;

		public  void memset0()
        {
			this.mems = null;
			this.usCnt = 0;
			this.bNeedCalc_byProcTaskDataResp = false;
        }

}
;


public class SHARED_OBJ_USR
	{
		public object hWnd_starter;                                              //  2009/08/21
		public int iWndContentType;

		//
		public uint uiTaskType;                                                    //  2009/12/23
		public int iTaskId;                                                    //  2009/08/23
																			   //
		public QY_MESSENGER_ID idInfo_to;
		public bool ucbVideoConferenceStarter;
		public bool ucbVideoConference;                                           //  2018/11/10

		//
		public uint uiTranNo_openAvDev_a;                                      //  2010/09/04
		public uint uiTranNo_openAvDev_v;                                      //  2010/09/04

		//
		//public bool ucbNotSendData;                                               //  2009/02/22

		//
		//public int iIndex_activeMems_from_videoConferenceStarter;              //  2009/05/30

		//  2015/05/23
		public bool bIn3DConf;                                                 //  2015/05/23. 在doCmd_startAvCall的sharedObjUsr(非mosaic)里设置。在检测到这个标志位后，doEncode要忽略，因为不需要。压缩工作都转移到了mosaic里去了 

		//
		public bool ucbInConfMosaic;                                          //  2014/11/16

		//  
		public uint dwLastTickCnt_applyForPlayer;                             //  2014/12/10

		//  2017/09/06
		public TASK_respInfo taskRespInfo;

		//
		public TMP_tHintBuf tDbgHint;
		//TCHAR tDbgHint[64];                                             //  2016/03/28

		public void memset0()
        {
				this.hWnd_starter=null;                                              //  2009/08/21
		this.uiTaskType=0;                                                    //  2009/12/23
		this.iTaskId=0;                                                    //  2009/08/23
																			   //
		this.idInfo_to.ui64Id=0;
		this.ucbVideoConferenceStarter=false;
		this.ucbVideoConference=false;                                           //  2018/11/10

		//
		this.uiTranNo_openAvDev_a=0;                                      //  2010/09/04
		this.uiTranNo_openAvDev_v=0;                                      //  2010/09/04

		//
		//this.ucbNotSendData=false;                                               //  2009/02/22

		//
		//this.iIndex_activeMems_from_videoConferenceStarter=0;              //  2009/05/30

		//  2015/05/23
		this.bIn3DConf=false;                                                 //  2015/05/23. 在doCmd_startAvCall的sharedObjUsr(非mosaic)里设置。在检测到这个标志位后，doEncode要忽略，因为不需要。压缩工作都转移到了mosaic里去了 

		//
		this.ucbInConfMosaic=false;                                          //  2014/11/16

		//  
		this.dwLastTickCnt_applyForPlayer=0;                             //  2014/12/10

		//  2017/09/06
		//public TASK_respInfo taskRespInfo;

	}

}
	;


	//
	public struct SHARED_obj_key
	{
		public int iIndex_sharedObj;
		public uint uiTranNo;
	}
	;


	public struct SharedObj_syncAv
	{
		public int iSampleTimeInMs_org_start;
		public int iSampleTimeInMs_start;
	};

	public struct SharedObj_status
	{
		public bool ucbFpsUnmatched;                                           //  2012/01/06,fps unmatched
	};


	public struct SharedObj_master
	{
		//uint								uiObjType_slave;
		public SHARE_dyn_bmp pShareDynBmp_slave;
		//
		public SHARED_obj_key slaveKey;
	};

	public struct SharedObj_slave
	{
		public SHARED_obj_key masterKey;
	};


	//
#if false
	public class QY_SHARED_OBJ
	{
		public uint uiType;                                                        //  2013/03/19

		//
		public bool bUsed;                                                     //  
		public int index_sharedObj;
		public uint uiTranNo;

		//
		//void* pProcInfo;                                                    //  2015/10/15

		//
		public uint dwTickCnt_start;                                          //  2011/08/01

		//
		public ushort usSubtype;                                                   //  2014/10/30
		public bool bSlave;

		
		public SharedObj_master master;
		public SharedObj_slave slave;
		

		//  2010/09/01
		public SHARED_OBJ_USR[] usrs;//[MAX_sharedObjUsrs];
		public int iIndex_curUsr;

		//
		public bool bCapDev;                                                   //  
		public bool bDirectX;
		public bool bRemoteAssist;                                             //  2008/11/10, 
		public bool bSoundRecorder;

		//
		public object pShareMediaObj;                                               //  2009/05/07

		//
		public object pTransInfo_stream;                                            //  2014/03/15. 对rtsp这样的流，需要转换成无压缩视频。pTransInfo_stream是用来解码的结构

		//
		public object pEvt;                                                     //  2015/10/15. evt有shm等提供数据源

		//
		public int iIndex_capBmp;
		public int iIndex_capAudio;                                            //  2009/04/23
																			   //
		public int iIndex_shmCmd;                                              //  2009/12/22

		//
		public int iDiffInMs_tStartAudio;                                      //  2009/05/26. used to sync video and audio

		//
		public int iIndex_localPlayer;                                         //  2009/05/02

		public object hThread;                                                 //  
		public bool bThreadEnd;

		/*
HANDLE hProcess;                                                    //  2009/12/22
struct                                         {
					 uint uiBufSize_qmShm;

HANDLE hMap;
char* pBuf;
				 }												dataShm;
		*/

		//
		public bool bQuit;

		//  2008/10/16
		//public ROUTE_sendLocalAv curRoute_sendLocalAv;                                     //  2014/10/29

		//

		//  2012/11/11
		public SharedObj_syncAv syncAv;

		//  2012/01/06
		public SharedObj_status status;

		//
		public void memset0()
        {
			this.uiType=0;                                                        //  2013/03/19

			this.bUsed=false;                                                     //  
		this.uiTranNo=0;

		//
		//void* pProcInfo;                                                    //  2015/10/15

		//
		this.dwTickCnt_start=0;                                          //  2011/08/01

		//
		this.usSubtype=0;                                                   //  2014/10/30
		this.bSlave=false;


			this.iIndex_curUsr=0;

		//
		this.bCapDev=false;                                                   //  
		this.bDirectX=false;
		this.bRemoteAssist=false;                                             //  2008/11/10, 
		this.bSoundRecorder=false;

			return;
        }
				 
}		 ;
#endif

public class QY_SHARED_OBJ
	{
		//
		public bool bUsed;                                                     //  

        //
        public struct QY_SHARED_OBJ_var
        {
            public uint uiType;                                                        //  2013/03/19

            public int index_sharedObj;
            public uint uiTranNo;

            //
            //void* pProcInfo;                                                    //  2015/10/15

            //
            public uint dwTickCnt_start;                                          //  2011/08/01

            //
            public ushort usSubtype;                                                   //  2014/10/30
            public bool bSlave;


            public SharedObj_master master;
            public SharedObj_slave slave;


            //  2010/09/01
            public int iIndex_curUsr;

            //
            public bool bCapDev;                                                   //  
            public bool bDirectX;
            public bool bRemoteAssist;                                             //  2008/11/10, 
            public bool bSoundRecorder;

            //
            public object pShareMediaObj;                                               //  2009/05/07

            //
            public object pTransInfo_stream;                                            //  2014/03/15. 对rtsp这样的流，需要转换成无压缩视频。pTransInfo_stream是用来解码的结构

            //
            public object pEvt;                                                     //  2015/10/15. evt有shm等提供数据源

            //
            public int iIndex_capBmp;
            public int iIndex_capAudio;                                            //  2009/04/23
                                                                                   //
            public int iIndex_shmCmd;                                              //  2009/12/22

            //
            public int iDiffInMs_tStartAudio;                                      //  2009/05/26. used to sync video and audio

            //
            public int iIndex_localPlayer;                                         //  2009/05/02

            public object hThread;                                                 //  
            public bool bThreadEnd;

            /*
	HANDLE hProcess;                                                    //  2009/12/22
	struct                                         {
						 uint uiBufSize_qmShm;

	HANDLE hMap;
	char* pBuf;
					 }												dataShm;
			*/

            //
            public bool bQuit;

            //  2008/10/16
            //public ROUTE_sendLocalAv curRoute_sendLocalAv;                                     //  2014/10/29

            //

            //  2012/11/11
            public SharedObj_syncAv syncAv;

            //  2012/01/06
            public SharedObj_status status;

			//
			public int m_o_texId;
			public int m_o_iW;                
			public int m_o_iH; 


			// for enc
			public System.Threading.Mutex hMutex_syncEncFrame;
			public System.Threading.EventWaitHandle hEvt_notifyFrameAvail;
			public bool bFrameAvail;			
			
			//
        };

		//
		public QY_SHARED_OBJ_var var;

		//
		public SHARED_OBJ_USR[] usrs;//[MAX_sharedObjUsrs];


		//
		public void memset0()
        {
			this.bUsed = false;                                                     //  
#if false

			this.uiType=0;                                                        //  2013/03/19

		this.uiTranNo=0;

		//
		//void* pProcInfo;                                                    //  2015/10/15

		//
		this.dwTickCnt_start=0;                                          //  2011/08/01

		//
		this.usSubtype=0;                                                   //  2014/10/30
		this.bSlave=false;


			this.iIndex_curUsr=0;

		//
		this.bCapDev=false;                                                   //  
		this.bDirectX=false;
		this.bRemoteAssist=false;                                             //  2008/11/10, 
		this.bSoundRecorder=false;
#endif
			//
			this.var = new QY_SHARED_OBJ_var();
			int i;
			for  (  i = 0; i < this.usrs.Length; i++)
            {
				this.usrs[i].memset0();
            }

			return;
        }
				 
}		 ;






	//
	public class QY_sharedObj_sync
{
	public QY_SHARED_OBJ sharedObj;
	//
	public SYNC_mt_cnt syncMtCnt;

	//
	public QY_sharedObj_sync()
        {
			this.sharedObj = new QY_SHARED_OBJ();
			this.sharedObj.usrs = new SHARED_OBJ_USR[Consts.MAX_sharedObjUsrs];
			for ( int i=0;i <sharedObj.usrs.Length;i++)
            {
				sharedObj.usrs[i] = new SHARED_OBJ_USR();
            }

			this.syncMtCnt = new SYNC_mt_cnt();
        }
}
;


partial class qyFuncs
{
	public static bool isSharedObjUsrIndexValid(int index)
	{
		return ((index) >= 0 && (index) < Consts.MAX_sharedObjUsrs);
	}

		public static QY_sharedObj_sync getSharedObjSyncByIndex(CCtxQmc pProcInfo, int index)
		{
			if (null==pProcInfo) return null;
			return pProcInfo.getSharedObjSyncByIndex(index);
		}


		//
		//
		public static QY_SHARED_OBJ getSharedObjByIndex(CCtxQmc pProcInfo, int index)
		{
			QY_sharedObj_sync pSharedObjInfo = getSharedObjSyncByIndex(pProcInfo, index);
			if (null==pSharedObjInfo) return null;
			//
			return pSharedObjInfo.sharedObj;
		}


		public static CAP_procInfo_audioCommon getCapAudioBySth(CCtxQmc pProcInfo, int index, uint uiCapType)
		{
			bool bRet = false;

			if (null==pProcInfo) return null;

			//
			return pProcInfo.getCapAudioBySth(index, uiCapType);
		}

		//  if  uiCapType is 0, don't need to check capType. and if uiCapType is not 0, ...
		public static CAP_procInfo_bmpCommon getCapBmpBySth(CCtxQmc pProcInfo, int index, uint uiCapType)
		{

			bool bRet = false;

			if (null==pProcInfo) return null;

			//
			return pProcInfo.getCapBmpBySth(index, uiCapType);
		}


	}



}



