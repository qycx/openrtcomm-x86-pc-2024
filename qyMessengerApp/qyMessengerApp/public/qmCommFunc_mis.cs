using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{

	public unsafe class ENC_CTX_isCnt:QY_ENC_CTX
	{
		//public QY_ENC_CTX common;                                          //  2007/06/20

		//
		public struct ENC_CTX_isCnt_var
		{
			public uint uiBlobLen_publicKey;
			public fixed byte publicKeyBlob[Consts.bufSize_publicKeyBlob];
		};
		public ENC_CTX_isCnt_var m_var;

		//
		public bool ucbPublicKeyDownloaded;                           //  
		public bool ucbPublicKeyVerified;                         //  
																	//
	}
	;

	public unsafe struct CE_TASK_MEM
	{
		public QY_MESSENGER_ID idInfo_send;
		public Int64 tStartTime;
		public uint uiTranNo;
		//
		public int iTaskId;
		public int iStatus;

		public QY_MESSENGER_ID idInfo_recv;            //  ½ÓÊÕ·½  

	}
	;



	public unsafe struct MIS_CNT_cfg
    {
		

		public			fixed char misServName[Consts.CONST_maxMisServNameLen + 1];

		public QWM_DYN_CFG dynCfg;

		//
		public int iObjQType;                                      //  2007/04/05
		public fixed char mutexName_syncQmObjQ[Consts.cntof_mutexName];
		public fixed char semaphoreName_syncQmObjQ[Consts.cntof_semName];
		public uint uiMaxCnt_semaphore_qmObjQ;



		//
		public GENERIC_Q_CFG inCacheQ;                                     //  2008/04/02
																		   //GENERIC_Q_CFG								inputQ;											//  2015/09/09
																		   //
		public GENERIC_Q_CFG toSendQ;
		public GENERIC_Q_CFG outputQ;
		//

		public GENERIC_Q_CFG talkingFriendQ, recentFriendQ;

		//
		public GENERIC_Q_CFG imObj_intQ;

		//
		public uint uiSizePerSnd_media;                                //  2011/01/30

		public ushort usIntervalInS_getIntervalParams;             //  
																   //
		public ushort usMaxMsgrRecentFriends;
		public ushort usIntervalInS_refreshRecentFriends;              //  
																	   //
		public ushort usIntervalInS_refreshContactList;                //  

		//  2015/07/28
		//unsigned  char								ucbNotUseP2pCall;								

		//
		public ushort usMaxContactUpdateInfosPerSnd_grp;               //  2014/02/06
		public ushort usMaxContactUpdateInfosPerSnd_grpMem;            //  
		public ushort usMaxContactUpdateInfosPerSnd_contact;           //  

		//
		public ushort usIntervalInS_tryToReg;                          //  
		public ushort usIntervalInS_tryToSndDevInfo;

		public fixed char mutexNamePrefix_syncTask[Consts.cntof_mutexName_prefix];             //  

		//
		

	};


	public struct MIS_CHANNEL_status
	{
		//  unsigned  long							ulbServNotConnected:1;
		public bool ulbAuthed;
		public bool ulbNeedVerified;

		//
		public bool ulbSessionErr;                              //  

		public IM_NET_STAT netStat;
		public IM_NET_STAT oldNetStat;

		public bool ulbIoQuit;                                  //  IO
																//
		public bool ulbInputQuit;                                   //  2015/09/09
		public bool ulbToSendQuit;                              //  
		public bool ulbSndQuit;                                 //  
		public bool ulbRecvQuit;                                    //  
																	//

		public uint dwTickCnt_lastStartToRecv;                        //  2009/02/25
		public uint dwTickCnt_lastEndSending;                     //  2011/01/23

		//
		public uint dwTickCnt_noSending_start;                        //  2012/05/21

		//
		public uint dwLastTickCnt_applyForChkChannels;

	};

	public unsafe class MIS_CHANNEL
	{

		//
		public MIS_CNT pMisCnt;                                       //  2016/09/09

		//
		public uint uiType;
		//
		
		public ENC_CTX_session commEncCtx;                                     //  2008/06/08
		public bool ucbEncInited;
		//
		
		
		public QY_COMM_SESSION session;
		public QY_SOCK sock;
		public bool bSessionOpen;
		

		//
		public GENERIC_Q inCacheQ;                                     //  recv
																	   //QY_Q2											inputQ2;										//  2015/09/09
																	   //
		public QY_Q2 toSendQ2;
		public QY_Q2 outputQ2;

		//
		public Thread hThread_io;                                      //  
		public uint dwThreadId_io;                                    //  

		public void* p0;                                               //  可以传给thread_work_iocp参数. 2012/02/24

		//
		public MIS_CHANNEL_status status;

}
;

	//
	public struct MIS_CNT_refreshContactList
	{                                                //  2997/08/02
		public uint dwLastTickCnt_startToRetrieve;                   //  2007/07/26
		public uint dwLastTickCnt_respRecvd;                      //  2014/02/11

		//
		public Int64 tLastRefreshedTime_misServ;                      //  
		public Int64 tStartTime;                                      //  
		public uint uiTranNo;
		//
		public QY_MESSENGER_ID idInfo_lastRefreshed;                           //  
																		//
	};


	public struct MIS_CNT_refreshImObjRules
	{                                                //  2010/08/17
		public uint dwLastTickCnt;
		public Int64 tLastRefreshedTime;
		//
		public Int64 tStart_toRetrieveAll;                         //  2011/11/12
	};


	public struct MIS_CNT_status
	{
		//
		public bool ulbSeriousErr;                             //  
																   //
		public ushort usRunningStatus;                             //  2007/06/07

		//IM_TASK_STAT netStat;

	};


	//
	public struct MIS_MSG_TALKINGFRIEND_QMC
	{
		public uint uiType;
		//
		public object                                           hWnd;
				
		//  		
		public MSGR_ADDR addr;
		//
	}
	;

	public struct MisCnt_refreshVideoConferenceActiveMems
	{
		public uint dwLastTickCnt_refreshed;                     //  2009/06/01
		public bool bNeedRefreshed;                                    //  2009/06/01
		public int iTaskId;
	};

	//
	public unsafe struct TMP_talkerDesc
    {
		public fixed char talkerDesc[Consts.cntof_talkerDesc];
    }

	public unsafe struct TMP_displayName
	{
		public fixed char displayName[Consts.cntof_displayName];
	}



	//
	public unsafe class MIS_CNT
	{


		//
		public CCtxQmc pProcInfoParam;                                   //  2009/09/07

		//
		public MIS_CNT_cfg cfg;

		
		public ENC_CTX_isCnt commEncCtx;                                      //  2007/06/20
		public bool ucbEncInited;
		

	//QY_COMM_AUTHINFO_MIS* pAuthInfo;
	
		
		  //
		  public QY_LOGICAL_SERVER server;
		/*
	//
	void* pObjQ;                                            //  2007/06/03
	unsigned char ucbQmObjQInited;                              //  2007/06/03
		*/

		public ushort usConnectCntInterval;


		//
		public MIS_CHANNEL[] channels;	//[Consts.CONST_maxConnsPerCli_mis];

		
	//
		public QY_MESSENGER_ID idInfo;                                         //  Me idInfo
																			   //public string displayName;                          //  Me 
		public TMP_displayName displayName;
		public TMP_talkerDesc talkerDesc;
		public TMP_displayName displayName_withIdInfo;

		//
		public bool bIntervalParamsGot;                                //  2007/09/20
		public uint dwLastTickCnt_getIntervalParams;

		//
		//RETRIEVE_TO_PATHS toPaths;                                      //  2008/02/09
		//BOOL bToPathsRetrieved;
		//DWORD dwLastTickCnt_retrieveToPaths;

		//

		//
		public List<MIS_MSG_TALKINGFRIEND_QMC> talkingFriendQ;	
		//public GENERIC_Q talkingFriendQ;                                  //  
																			//
	
		public GENERIC_Q recentFriendQ;                                     //  pRecentFriendsºÍrecentFriendQµÄ¹ØÏµ£ºpRecentFriendsºÍ·þÎñÆ÷ÊÇÍ¬ÑùµÄÄÚ´æÇø£¬
																			//		recentFriend
																			//		recentFriend pRecentFriends
																			//		pRecentFriends

		//
		public GENERIC_Q imObj_intQ;

		//
		public Mutex hMutex_syncTask;
		public Mutex hMutex_syncStartAv;
		public Mutex hMutex_syncSendAv;
		public Mutex hMutex_syncActiveMem;
		public Mutex hMutex_syncIndex_dec;
		public Mutex hMutex_syncShareEglContext;

		/*
		struct                                         {

					 time_t tLastRefreshedTime_misServ;
	DWORD dwLastTickCnt;                                    //  
}
refreshRecentFriends;
//

unsigned char ucTriesToReg;                                 //  2007/08/05
DWORD dwLastTickCnt_tryToReg;

unsigned char ucbNeedSndDevInfo;                                //  2007/08/03
DWORD dwLastTickCnt_tryToSndDevInfo;

//  DWORD											dwLastTickCnt_chkChannels;					//  channel channel. 2008/06/01

//
TCHAR whoBuf_showInfo[32];                          //  2007/05/28
	*/

		public MIS_CNT_refreshContactList refreshContactList;
		/*

struct                                         {
					 DWORD dwLastTickCnt;
				 }												retrieveCustomerServiceObjList;                 //  2011/04/10

struct                                         {                                                //  2008/02/01
					 DWORD dwLastTickCnt;
//
WCHAR wDevIdStr_lastRefreshed[32 + 1];
					 //
				 }												retrievePhoneMsgrs;

QY_im_rules imRules;                                        //  2011/11/13. 将根本messenger相关的存服务器数据库里的全局规则放在这里
		*/
															//
		public MIS_CNT_refreshImObjRules refreshImObjRules;
		
		/*
//
struct                                         {                                                //  2014/04/19
					 time_t tStartTime;
unsigned int uiTranNo;
				 }												retrieveDynBmps;
		*/

//  2009/06/01
		public MisCnt_refreshVideoConferenceActiveMems refreshVideoConferenceActiveMems;
		/*

//  2015/08/01
struct                                         {
					 QY_MESSENGER_ID idInfo_resServ;
				 }												procOfflineRes;


//			
CAP_subWnds subWnds;                                        //  2009/09/09

//
IM_TASK_STAT taskStat;
		*/

//
		public MIS_CNT_status status;

		public MIS_CNT()
        {
			int ii = 0;
			//
			this.commEncCtx = new ENC_CTX_isCnt();
        }
		
};



}
