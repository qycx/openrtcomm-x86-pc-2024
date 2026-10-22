using System;
using System.Collections.Generic;
using System.Text;


partial class Consts
{
	public const string CONST_fn_dump_h264_enc = "dump_h264_enc.264";

	//
	public const string CONST_fn_dump_h264_dec = "dump_h264_dec.264";

	//
	public const string CONST_fn_dump_mp3_enc = "dump_mp3_enc.mp3";
	public const string CONST_fn_dump_mp3_dec = "dump_mp3_dec.mp3";


}



namespace qy
{
	public struct QMC_threads_status
    {
		public bool ulbMgrQuit;                                             //  
		public bool ulbDisplayQuit;                                         //  display
		public bool ulbWorkQuit;                                                //  work
		public bool ulbSchedulerQuit;                                           //  scheduler
		public bool ulbRobotQuit;                                               //  svr
		public bool ulbMediaQuit;                                               //  media
		public bool ulbRealTimeMediaQuit;                                       //  realTimeMedia
		public bool ulbRtOpQuit;                                                //  2016/12/26
		public bool ulbLogQuit;                                             //  log

	}

	public struct QMC_status_retrieveImObjList
	{
		public bool ulbImGrpExists;                                            //  是否下载了分组信息。2011/04/10.
		public bool ulbContactExists;                                           //  是否下载了contact信息
																				//
		public bool ulbAllContactsRetrieved;                                    //  是否用户信息都下载了. 2013/08/01
																				//
		public ushort nTimes_refresh;                                              //  因为access写入较慢,所以,在所有用户信息得到后,需要自动的刷新几次. 比如3次
		public uint dwTickCnt_lastRefreshed;
	};


	public struct QMC_status_progress
	{
		public object hWnd;
		public uint uiTranNo;
		//
		public bool ucbProgessEnd;
		public int nResult;
		//
	};


	public struct QmcStatus_ptsStat
	{
		public TRANS_pts_info transPtsInfo_sum;
		public TRANS_pts_info transPtsInfo_sum_last;
		//
		public bool bTooManyPtsErr;
		public byte ucCnt_bTooManyPtsErr;
		public uint dwTickCnt_lastModified_bTooManyPtsErr;
		//
		public bool bWarning;

	};

	public struct QMC_status_cpu
	{                                                            //  for cpu usage. 2009/10/10
#if false
			LARGE_INTEGER oldIdleTime;
LARGE_INTEGER oldKernelTime;
LARGE_INTEGER oldUsrTime;
//
DWORD dwTickCnt_start_ce;
DWORD dwIdleTime_start_ce;
//
#endif
		public int iCpuUsage;
	};


	//
	public struct RENDER_status
	{
		public float fps;
		public uint dwTickCnt_calcFps;
	}
	;


	public struct QMC_status
	{
		public bool ulbThreadProcInited;                                        //  mcThreadProc_mis 2007/09/02

		public QMC_threads_status threadsStatus;                                                                                    //

		//
		public QMC_status_retrieveImObjList retrieveImObjList;

		/*
					 //
					 struct                                     {
						 DWORD dwTickCnt_start;
	DWORD dwTimeInMs;
}
waitToQuit;                                                 //  2011/02/02
		*/
//
public uint dwTickCnt_toStat;                                         //  2009/10/11
		

																//
		public QMC_status_cpu cpu;
		
public QMC_net_stat netStat;                                                    //  2009/10/10
																		 //MEMORYSTATUS memory;                                                        //  201007/28
		public RENDER_status render;


		//
#if false
		struct                                     {
						 uint dwTickCnt_lastDisplayed;
TCHAR tBuf[256];
int len;
					 }											notification;
#endif


		//  2010/08/14
		public QMC_status_progress progress;
public bool bInSequence_showDlgProgress;           //  2014/05/07

	
//  2012/03/25
public int  nThreads_dec;
public int  nThreads_dispatch;                     //  2013/08/26
public int  nThreads_player;
public int  nThreads_aTrans;                       //  2014/03/15
public int  nThreads_vTrans;                       //  2014/03/15
public int  nThreads_talkerProc;                   //  2016/03/15
public int  nThreads_shareDynBmps;                 //  2016/06/15

//  2013/03/07
public uint dwTickCnt_chkDb;
		

//  2017/10/03
public uint dwTickCnt_sendTo_applyForOrgReq;      //  2017/10/03

		/*
//  2013/07/19
struct                                     {
						 int xPos_screen;
int yPos_screen;
//
HWND hParent_ia;								//  2012/05/03					
						 //
					 }											param_viewInstantAssistant;

//  2014/02/04
struct                                     {
						 struct                                 {
							 int xPos;
int yPos;
//
DWORD dwLastTickCnt;
						 }										mousemove,  lButtonDown,  command;
//  2016/01/15
struct                                 {
							 BOOL bSetCaptured;
unsigned int uiTranNo_wm;

//
RECT bgScreenRect_wm;

//
int sx_mouse, sy_mouse;


						 }										SetCapture;
						 //
					 }											param_d3dWall;

//  2014/02/11
struct                                     {
						 int uiQNodes_displayedObjQ;
					 }											display;                                //  thread_display的状态.
																										//  2014/05/27
struct                                     {
						 int maxiTotal;
int nTotal;
					 }											hashTbl_leftView_db;

//  2014/12/22
struct                                     {
						 BOOL bFailed_newSharedObj;
					 }											sysErr;
		*/

//  2015/06/18
		public QmcStatus_ptsStat ptsStat;
		

//  2018/10/30
//char vName_1st_webcam[256];	

}		;

	//
	public struct QMC_cfg_debugStatusInfo
	{                                                           //  2009/02/27
		public bool ucbViewAudioStatus;
		public bool ucbViewVideoStatus;                                           //  2009/04/16
		public bool ucbViewIoStatus;                                          //  2009/04/16
																				//
		public bool ucbViewMgrStatus;                                         //  2017/10/11
																				//
		public bool ucbPrint_a_in;                                                //  2010/06/25
		public bool ucbPrint_a_out;
		public bool ucbPrint_v_in;
		public bool ucbPrint_v_out;
		//
		public bool ucbPrint_v_draw;                                          //  2010/09/12
																				//
		public bool ucbPrint_a_mt;                                                //  2013/03/21
																					//
		public bool ucbShowVideoCapStatus;                                        //  2012/11/11
		public bool ucbShowAudioCapStatus;                                        //  2012/11/11
																					//
		public bool ucbShowPreDrawStatus;                                     //  2012/01/26
		public bool ucbShowToDrawStatus;                                      //  2012/01/26
																				//
		public bool ucbShowRenderStatus;                                      //  2015/10/07
																				//
		public bool ucbShowPlayAudioStatus;                                       //  2012/11/11

		//
		public bool ucb_chkInputPts;

		//																			//
		public bool ucbShowPktInputInfo;                                      //  2012/03/16
		public bool ucbShowEncVStatus;                                            //  2014/06/23
																					//
		public bool ucbShowDec_pipeStatus;                                        //  2016/04/13
																					//
		public bool ucbShowPreDecV_pts;                                           //  2015/06/18
		public bool ucbShowPreDecAStatus;                                     //  2015/09/11
		public bool ucbShowPreDecVStatus;                                     //  2014/06/23
		public bool ucbShowPostDecAStatus;                                        //  2015/09/11
		public bool ucbShowPostDecVStatus;                                        //  2014/06/23
																					//
		public bool ucbShowDec_sharedTexStatus;                                   //  2016/04/08
																					//
		public bool ucbPrint_rtt;                                             //  2014/11/26
																				//
		public bool ucbPrint_d3d;                                             //  2014/12/09
																				//
		public bool ucbShowDlgTalkStatus;                                     //  2012/05/25

		//
		public bool ucbShow_onvif;                                                //  2016/07/31
		public bool ucbPrint_ptz;                                             //  2015/09/19

		//
		public bool ucbShowRtspCliControl;                                        //  2014/03/23

		//
		public bool ucbPrint_rtsp;                                                //  2014/09/29

		//
		public bool ucbDebugSubtitle;                                         //  2014/11/07
		public bool ucbDebugIa;                                                   //  2014/11/30
																					//
		public bool ucbDebug_newMsgArrives;                                       //  2014/12/03
		public bool ucbDebug_fillData_d3d;                                        //  2014/12/03

		//
		public bool ucbViewFileStatus;


		//
		//#define		CONST_fn_dump_h264_enc		"dump_h264_enc.264"
		//#define CONST_fn_dump_h264_enc		"c:\\tttbbb\\test\\dump_h264_enc.264"
		public bool ucb__Dump_h264_enc__;                                     //  2014/03/30

		//
//#define CONST_fn_dump_h264_dec	"dump_h264_dec.264"
		public bool ucb__Dump_h264_dec__;                                     //  2014/03/30

//#define CONST_fn_dump_pcm		"c:\\tttbbb\\test\\dump_pcm.pcm"
		public bool ucb__Dump_pcm__;
#if DEBUG
		public struct DEBUG_dump {
			public int bufSize;
			public IntPtr pBuf;
			public int len;
		};
		public DEBUG_dump dump;
#endif

		//
		public bool ucb__Dump_mp3_enc__;
		public bool ucb__Dump_mp3_dec__;

		//
		public bool ucb__Dump_deced_pcm__;

		//
		public bool ucb_draw_talkers_frm;                                     //  2016/06/02
		public bool ucb_draw_walls_frm;

		//
		public bool ucb_notPrint_my_inStream;

		//
		public bool bPrint_encObj_mediaCodec;

		//
		public bool bNoRemove_err_missing;

		//
		public bool bUse_test_noLocalAv;

		//
	}
	;

//
public abstract class CCtxQmcTmpl : CCtxQyTmpl
    {
           public CCtxQyMc pQyMc;
            //void* pSgiParam;



        public CCtxQmcTmpl()
        {

        }

        ~CCtxQmcTmpl()
        {

        }

        //
        //MACRO_mcVar_common

        public abstract int setQmDbFuncs(int iDbType, QM_dbFuncs pDbFuncs);

    }


}
