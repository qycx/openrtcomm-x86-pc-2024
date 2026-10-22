using System;
using System.Collections.Generic;
using System.Text;


partial class Consts
{
    //public const int		CONST_uiMaxCnt_semaTrigger_mgrQ_isCli				1110
    public const int CONST_uiMaxQNodes_mgrQ_isCli = 1100;
    //
    public const int CONST_uiMaxCnt_semaTrigger_mgrQ_isMgr = 111;
    public const int CONST_uiMaxQNodes_mgrQ_isMgr = 110;

    //
    public const int CONST_uiMaxQNodes_inCacheQ_isCli = 10;                             //  2008/04/02

    //
    //public const int		CONST_uiMaxQNodes_inputQ_isCli						200									//  2008/04/02


    //  public const int		CONST_uiMaxCnt_semaTrigger_toSendQ_256				40									//  2008/05/23
    public const int CONST_uiMaxQNodes_toSendQ_256 = 20;                                //  2008/05/23
                                                                                        //
                                                                                        //  public const int		CONST_uiMaxCnt_semaTrigger_toSendQ_xp				50									//  2008/05/23
    public const int CONST_uiMaxQNodes_toSendQ_xp = 40;                             //  2008/05/23
                                                                                    //
                                                                                    //  public const int		CONST_uiMaxCnt_semaTrigger_toSendQ					140									//  2008/05/23
    public const int CONST_uiMaxQNodes_toSendQ = 120;                               //  2008/05/23
    public const int CONST_uiQNodes_warningInterval_toSendQ = 10;                               //  200811/22

    //  public const int		CONST_uiMaxCnt_semaTrigger_outputQ_256				20
    public const int CONST_uiMaxQNodes_outputQ_256 = 10;
    //
    //  public const int		CONST_uiMaxCnt_semaTrigger_outputQ_xp				40									//  Êý¾ÝÓÐµÄ¿ÉÒÔÍ¨¹ýtoSendQÈ»ºóÔÙ·¢ËÍ£¬ÓÐµÄÍ¨¹ýoutputQÖ±½Ó·¢ËÍ 2008/10/29
    public const int CONST_uiMaxQNodes_outputQ_xp = 30;
    //
    //  public const int		CONST_uiMaxCnt_semaTrigger_outputQ					80									//  Êý¾ÝÓÐµÄ¿ÉÒÔÍ¨¹ýtoSendQÈ»ºóÔÙ·¢ËÍ£¬ÓÐµÄÍ¨¹ýoutputQÖ±½Ó·¢ËÍ 2008/10/29
    public const int CONST_uiMaxQNodes_outputQ = 60;

    //
    public const int CONST_uiMaxCnt_semaTrigger_workQ = 210;
    public const int CONST_uiMaxQNodes_workQ = 200;
    //
    public const int CONST_uiMaxCnt_semaTrigger_displayQ = 620;
    public const int CONST_uiMaxQNodes_displayQ = 600;
    //
    public const int CONST_uiMaxCnt_semaTrigger_schedulerQ = 310;
    public const int CONST_uiMaxQNodes_schedulerQ = 300;
    //
    public const int CONST_uiMaxCnt_semaTrigger_robotQ = 210;                                   //  2007/07/04
    public const int CONST_uiMaxQNodes_robotQ = 200;
    //

    public const int CONST_uiMaxQNodes_mediaQ_256 = 20;                             //  2008/03/15
                                                                                    //
    public const int CONST_uiMaxQNodes_mediaQ_xp = 80;                              //  2008/03/15
                                                                                    //
    public const int CONST_uiMaxQNodes_mediaQ_win32 = 120;                                  //  2014/05/12. 2008/03/15
    public const int CONST_uiMaxQNodes_mediaQ_x64 = 200;                                    //  2012/05/23
    public const int CONST_uiQNodes_warningInterval_mediaQ = 10;                                    //  2008/03/21				



    public const int CONST_uiMaxQNodes_playSoundQ_256 = CONST_uiMaxQNodes_playSoundQ;   //  2008/03/15
    public const int CONST_uiMaxQNodes_playVideoQ_256 = 30;                 //  2012/11/09
    public const int CONST_uiMaxQNodes_displayVideoQ_256 = 30;                                  //  2012/11/09
                                                                                                //
    public const int CONST_uiMaxQNodes_playSoundQ = 100;    //  2017/10/14	//  60									//  2008/03/15
    public const int CONST_uiMaxQNodes_playVideoQ = 120;    //  150							//  2009/05/05
    public const int CONST_uiMaxQNodes_displayVideoQ = 60;                                  //  2009/07/25

    //public const int		CONST_uiMaxQNodes_preTransformQ					100									//  2011/12/09
    //
    public const int CONST_uiMaxQNodes_transformQ_256 = CONST_uiMaxQNodes_transformQ;   //  2008/05/12
#if DEBUG
    public const int CONST_uiMaxQNodes_transformQ = 45;                    //  2016/08/29	//  100									//  2008/05/12
#else
    public const int CONST_uiMaxQNodes_transformQ = 120;                    //  2016/08/29	//  100									//  2008/05/12
#endif

    //  
    //public const int		CONST_uiMaxQNodes_vCamQ								20									//  2010/06/18

    //
    public const int CONST_uiMaxQNodes_appQ = 100;                          //  2017/01/29
                                                                            //  
    public const int CONST_uiMaxQNodes_vppQ = 100;                              //  2011/12/04

    //
    public const int CONST_uiMaxQNodes_toShareAudioQ_256 = CONST_uiMaxQNodes_toShareAudioQ;     //  2011/01/21
    public const int CONST_uiMaxQNodes_toShareAudioQ = 100;				//  2011/01/21
public const int CONST_uiMaxQNodes_toShareVideoQ_256		=			CONST_uiMaxQNodes_toShareVideoQ;        //  2011/01/21
    public const int CONST_uiMaxQNodes_toShareVideoQ = 100;                     //  2011/01/21

    //
    public const int CONST_uiMaxQNodes_toShareVideo_postQ = 100;                                    //  2011/12/06

    //
    public const int CONST_uiMaxQNodes_dispatchQ = 100;                                 //  2009/06/17

    //
    public const int CONST_uiMaxQNodes_talkerThreadQ = 100;                                 //  2016/03/15
                                                                                            //
    public const int CONST_uiMaxQNodes_shareDynBmpsQ = 100;                 //  2016/03/15


    //
    public const int CONST_uiMaxQNodes_processQ_robot = 300;                                //  2007/07/04
    public const int CONST_uiMaxQNodes_processQ_media = 310;                                    //  2008/03/15

    public const int CONST_uiMaxQNodes_outputQ_toMix_audio_256 = 30;                        //  2008/05/23
                                                                                            //
    public const int CONST_uiMaxQNodes_outputQ_toMix_audio_xp = 50;                         //  2008/05/23
                                                                                            //
    public const int CONST_uiMaxQNodes_outputQ_toMix_audio = 256;                               //  2017/10/14	//  80									//  2008/05/23

    public const int CONST_uiMaxQNodes_outputQ_toMosaic_video = 256;                           //  2010/09/18

    public const int CONST_uiMaxQNodes_guestQ = 5000;                       //  2008/01/18
    public const int CONST_uiMaxQNodes_guestQ_ce = 0;               //  2010/04/15
                                                                    //

    public const int CONST_uiMaxQNodes_dynBmpQ = 256;                           //  2009/09/05

    public const int CONST_uiMaxQNodes_imObj_intQ = 10000;

    //
    public const int CONST_uiMaxQNodes_tmpGrpMemQ = 5000;                           //  2008/01/22

    //
    public const int CONST_uiMaxQNodes_talkingFriendQ = 1000;                   //  2012/05/07

    //  public const int		CONST_uiMaxQNodes_recentFriendQ					200									//  
    public const int CONST_uiMaxQNodes_recentFriendQ = 5000;                        //  2014/02/11. 当messenger很多时，比如几千点时，需要本参数大一点，否则在线状态无法尽快获取



    public const int DEFAULT_uiSizePerSnd_file_avExists = (64 * 1024);                  //  2014/07/13 
    public const int DEFAULT_uiSizePerSnd_file_noAv = (128 * 1024);                 //  2014/07/13 

    //
    public const int MIN_uiSizePerSnd_media = (62 * 1024);                  //  2008/04/01
    public const int MAX_uiSizePerSnd_media = (CONST_dataBufSize_iocp - 1024);  //  2008/04/01

    //  public const int 		DEFAULT_uiSizePerSnd_media_isMgr					(  256  *  1024  )					//  
    public const int DEFAULT_uiSizePerSnd_media_isMgr = 512 * 1024;// (128 * 1024);                   //  2011/02/13
    public const int DEFAULT_uiSizePerSnd_media_isCli = 512 * 1024;// (128 * 1024);					//  


    //
    public const int CONST_usIntervalInS_getIntervalParams = 30;                                    //  È¡²ÎÊýÅäÖÃ£¬ 30
    public const int CONST_usIntervalInS_refreshRecentFriends = 20;                                 //  Ë¢ÐÂ×î½üÁªÏµÈË×´Ì¬µÄ¼ä¸ô, 20
    public const int CONST_usIntervalInS_refreshContactList = 120;                                  //  Ë¢ÐÂÁªÏµÈËÃûµ¥µÄ¼ä¸ô, 120

    //
    public const int CONST_usIntervalInS_tryToReg = 30;                                 //  2007/07/31
    public const int CONST_usIntervalInS_tryToSndDevInfo = 30;                                  //  2007/08/03

    //
    public const string CONST_mutexName_guiProcessQ = ("qyIsGProcQ");
    public const int CONST_maxQNodes_guiProcessQ = 100;

    //
    public const int DEFAULT_iTimeoutInMs_channelIdle_others = 10000;                               //  2009/02/25
                                                                                                    //#define		DEFAULT_iTimeoutInMs_channelIdle_recv_others		30000								//  2011/01/31
    public const int DEFAULT_iTimeoutInMs_channelIdle_recv_talk = 300000;                               //  2011/01/31

    //
    public const int MAX_failures_frames_perSecond = 5;                                 //  ÌáÉýÖ¡ËÙµÄ×î¶à¿ÉÔÊÐíÊ§°Ü´ÎÊý
                                                                                        //
    public const int MAX_ucTimeoutInS_showFrameInfoResp = 30;									//  ÏÂ´«Ö¡ËÙÐÅÏ¢µÄÏìÓ¦×îÐ¡³¬Ê±,²»ÄÜµÍÓÚ´ËÖµ

}

namespace qy
{
    public unsafe struct TRAN_INFO_MIS
    {
        //
        public uint uiMaxSendMsgs;                     //  2011/01/23
                                                        //
        public uint nNodes_processed;                  //  messenger->iocp
        public Int64 tNow;
        public uint uiTranNo;
        public uint uiCliIndex;
        public void* pQMem;

        public bool bNeedRemoveMsg;                        //  2008/04/02, daemon doMisMgr_sendFinished(  ) remove
        public bool bNotResendMsg;                     //  2008/05/15, media rtMedia, msg
                                                       //
        public MIS_MSGU* pMsg_o;                               //  sendFinished buf
                                                               //
        public fixed char whereBuf_showInfo[64];                //  2015/09/04

    }
    ; 

  


}
