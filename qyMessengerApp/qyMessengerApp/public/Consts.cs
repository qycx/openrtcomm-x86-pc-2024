using System;
using System.Collections.Generic;
using System.Text;


static partial class Consts
{

    //  下面的值为函数间返回值，跟CONST_qyRc_ok不同。
    //
    public const int CONST_qyRet_err = -1;
    public const int CONST_qyRet_ok = 0;
    public const int CONST_qyRet_redirect = 1;
    public const int CONST_qyRet_update = 2;
    public const int CONST_qyRet_quit = 3;
    public const int CONST_qyRet_hint = 4;
    public const int CONST_qyRet_needMoreLicense = 5;
    public const int CONST_qyRet_accessDenied = 6;      //  
    public const int CONST_qyRet_skip = 7;              //	
    public const int CONST_qyRet_needRefresh = 1000;    //  
    public const int CONST_qyRet_syncConflict = 1001;   //  
    public const int CONST_qyRet_outOfService = 1002;   //  
    public const int CONST_qyRet_needVerified = 1003;   //  2007/04/03, 需要审核
    public const int CONST_qyRet_serverHello = 1004;    //  2008/06/09, server要求返回公钥等信息
                                                        //
    public const int CONST_qyRet_qFull = -2000;         //  2007/05/29, 表明队列满了
                                                        //
    public const int CONST_qyRet_bufTooSmall = -2101;   //  2010/06/18
    public const int CONST_qyRet_exception = -2102;     //  2014/08/11

    //
    //  -3000
    //
    public const int CONST_qyRet_cantGet_index_activeMems_from = -3101;
    public const int CONST_qyRet_needKeyFrameFirst = -3102;


    //
    public const int CONST_qySystemId_messenger = 9;        //  qyMsg,		//  2007/03/03

    public const int CONST_qyAppType_client = 6;            //  ¿Í»§¶Ë




    public const int CONST_qyAppAvLevel_fullHd = 10;                            //  高级版。利用h264的high profile等，可支持1920*1080.面向视频要求较高的企业，对视频清晰度要求很高。

    //

    public const int MAX_messengerNameLen = 16;
    public const int MAX_messengerPasswdLen = 16;
    public const int CONTS_passwdSaltLen = 4;

    public const int CONST_passwdType_null = 0;
    public const int CONST_passwdType_md5 = 1;


    public const int CONST_maxTimeoutInS_sendMsg_is = 20;                           //  2007/12/09, 
    public const int CONST_maxSendMsgs_is = 20;                         //  2015/09/04  //20							//  2007/12/09, iocp
    public const int CONST_maxSendMsgs_isClient = 40;                           //  2007/12/09, iocp

    public const int CONST_usMaxMsgrRecentFriends = 230;                        //  2007/06/04, 2007/09/09


    public const int DEFAULT_usNeedShowFrameInfoIntervalInS = 30;                           //  





    public const int MAX_PATH = 260;
    public const int cntof_fileName = 256;
    public const int cntof_channelName = 128;

    //
    public const int CONST_langId_tw = 0x0404;  //  Chinese (Taiwan Region) 
    public const int CONST_langId_PRC = 0x0804; //  Chinese (PRC) 
    public const int CONST_langId_hk = 0x0c04;      //  Chinese (Hong Kong SAR, PRC) 
    public const int CONST_langId_Singapore = 0x1004;       //  Chinese (Singapore) 

    public const int CONST_langId_US = 0x0409;//  English (United States) 
    public const int CONST_langId_UK = 0x0809;          //	English (United Kingdom) 
    public const int CONST_langId_Australian = 0x0c09;      //  English (Australian) 
    public const int CONST_langId_Candian = 0x1009;     //  English (Canadian) 
    public const int CONST_langId_NewZealand = 0x1409;          //  English (New Zealand) 

    public const int CONST_authType_null = 0;                                       //  2011/03/22
    public const int CONST_authType_logon = 1;

    public const int CONST_maxCntAddrLen = 67;              //  2006/06/29


    public const int cntof_mutexName_prefix = 16;//32;
    public const int cntof_mutexName = 16;//32;
    public const int cntof_semName = 32;


    public const int CONST_maxConnsPerCli_mis = 7;                  //  

    public const int CONST_channelType_null = 0;
    public const int CONST_channelType_talking = 1;                 //  
    public const int CONST_channelType_robot = 2;
    public const int CONST_channelType_webSending = 3;                  //  2007/12/12, wemQm
    public const int CONST_channelType_media = 4;                   //  2008/03/15
    public const int CONST_channelType_realTimeMedia = 5;                   //  2008/04/17
    public const int CONST_channelType_rtOp = 6;					//  2016/12/26




}




