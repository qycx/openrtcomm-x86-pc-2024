using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

partial class Consts
{

    //
    public const int CONST_stateType_sys = 1;
    //
    public const int CONST_stateType_tasks = 10;
    public const int CONST_stateType_players = 11;
    public const int CONST_stateType_talks = 12;
    public const int CONST_stateType_qs = 13;
    //
    public const int CONST_stateType_others = 20;


    //  ÒÔÏÂÊÇÊý¾ÝÀàÐÍ, ÓÃÔÚÍ¨ÐÅÀï£¬ÑÏ½ûÐÞ¸Ä
    public const int CONST_qyDataType_l32 = 1;             //  netMcÀïÒÑÊ¹ÓÃ
    public const int CONST_qyDataType_long = CONST_qyDataType_l32;
    public const int CONST_qyDataType_str = 2;              //  netMcÀïÒÑÊ¹ÓÃ, ´ËÀàÐÍÊµ¼Ê·ÇÒÔ0½ØÎ²µÄ×Ö·û´®¡£Êµ¼ÊÊÇÒ»¸ö¶þ½øÖÆÁ÷¡£ÒòÎªÄÚÍø¹ÜÀíÒÑ¾­Ê¹ÓÃ£¬¹ÊÔÝÊ±²»¸Ä¶¯ 
                                                            //
    public const int CONST_qyDataType_wStr = 3;             //  2007/03/31
    public const int CONST_qyDataType_utf8Str = 4;              //  2007/03/31
    public const int CONST_qyDataType_data = 5;             //  2007/03/31, Êý¾Ý³¤¶ÈÓÃshortÀ´±íÊ¾.
    public const int CONST_qyDataType_l64 = 6;              //  2007/08/01
    public const int CONST_qyDataType_byte = 7;             //  2007/08/05
    public const int CONST_qyDataType_char = CONST_qyDataType_byte;             //  2007/08/05
    public const int CONST_qyDataType_short = 8;                //  2007/08/05
    public const int CONST_qyDataType_lData = 9;                //  2008/04/01, Õâ¸öÀàÐÍ¿ÉÌá¹©¸ü³¤µÄÊý¾ÝÃèÊö¡£Êý¾Ý³¤¶ÈÓÃlongÀ´±íÊ¾

    //
    public const int bufSize_cfgItemData = 32 * 1024;     //  2007/03/31, ½«»º³åµÄ³¤¶È±ä³¤ÁË
    public const int bufSize_cfgItemLData = 1024 * 1024;  //  2008/03/22

    public const int CONST_cntof_cfgItemStr = 4096;
    public const int CONST_cntof_cfgItemWstr = 4096;

    //
    public const int CONST_qnmCfgId_null = 0;                   //  È¡cfgsµÄ½áÊø·û

    // 2007/04/02
    public const int CONST_qnmCfgId_base_common = 1000;
    //
    //  CONST_qnmCfgId_base_common  ->  CONST_qnmCfgId_base_common  +  999
    public const int CONST_qnmCfgId_start = (CONST_qnmCfgId_base_common + 100);
    public const int CONST_qnmCfgId_mem = (CONST_qnmCfgId_base_common + 101);               //  2007/08/05, 只用在小而快的数据包。一般的包的成员要使用CONST_imCommType_mem
                                                                                            //
                                                                                            //

    //
    //
    //
    //  
    public const int CONST_qnmCfgId_clientVer = (CONST_qnmCfgId_base_common + 2009);
    public const int CONST_qnmCfgId_authCmd = (CONST_qnmCfgId_base_common + 2010);
    //  public const int 		CONST_qnmCfgId_uiQwmType							(  CONST_qnmCfgId_base_common  +  2011  )
    public const int CONST_qnmCfgId_uiObjType = (CONST_qnmCfgId_base_common + 2011);                //  2007/12/07, qwmType¸objType
    public const int CONST_qnmCfgId_iCustemId = (CONST_qnmCfgId_base_common + 2012);
    public const int CONST_qnmCfgId_uiChannelType = (CONST_qnmCfgId_base_common + 2013);                //  2007/07/08
                                                                                                        //  public const int 		CONST_qnmCfgId_uiDevType						=(  CONST_qnmCfgId_base_common  +  2014  );				//  2007/08/03, messenger
    public const int CONST_qnmCfgId_uiDevType_from = (CONST_qnmCfgId_base_common + 2014);               //  2007/08/03, messenger
    public const int CONST_qnmCfgId_uiDevType_to = (CONST_qnmCfgId_base_common + 2015);             //  2008/01/06, messenger
    public const int CONST_qnmCfgId_wDevIdStr_from = (CONST_qnmCfgId_base_common + 2016);               //  2008/01/06, from from uiDevType_from uiDevType
    public const int CONST_qnmCfgId_wDevIdStr_to = (CONST_qnmCfgId_base_common + 2017);             //  2008/01/06
    public const int CONST_qnmCfgId_wReversePath = (CONST_qnmCfgId_base_common + 2018);             //  2008/02/08
    public const int CONST_qnmCfgId_wForwardPath = (CONST_qnmCfgId_base_common + 2019);             //  2008/02/08
    public const int CONST_qnmCfgId_usAuthType = (CONST_qnmCfgId_base_common + 2020);               //  2011/03/22
    public const int CONST_qnmCfgId_iAppType = (CONST_qnmCfgId_base_common + 2021);             //  2011/03/27
                                                                                                //
    public const int CONST_qnmCfgId_shVer = (CONST_qnmCfgId_base_common + 2023);                //  2014/09/23
    public const int CONST_qnmCfgId_subSystemId = (CONST_qnmCfgId_base_common + 2024);              //  2017/06/21

    //
    public const int CONST_qnmCfgId_uiFieldId = (CONST_qnmCfgId_base_common + 2100);                //  2007/07/31
    public const int CONST_qnmCfgId_usCnt = (CONST_qnmCfgId_base_common + 2101);                //  2007/08/01
    public const int CONST_qnmCfgId_ucbNext = (CONST_qnmCfgId_base_common + 2102);              //  2007/08/03
    public const int CONST_qnmCfgId_ucbEnd = (CONST_qnmCfgId_base_common + 2103);               //  2007/08/03
                                                                                                //public const int 		CONST_qnmCfgId_tStartTime_serv						=(  CONST_qnmCfgId_base_common  +  2104  );				//  tStartTime_serv uiSerialNo
    public const int CONST_qnmCfgId_uiSerialNo = (CONST_qnmCfgId_base_common + 2105);               //  2007/09/01
    public const int CONST_qnmCfgId_ucbResp = (CONST_qnmCfgId_base_common + 2106);              //  2007/12/22, 
    public const int CONST_qnmCfgId_ucbNeedDetail = (CONST_qnmCfgId_base_common + 2107);                //  2007/12/22, 
    public const int CONST_qnmCfgId_ucbNeedResp = (CONST_qnmCfgId_base_common + 2108);              //  2008/04/21, 
    public const int CONST_qnmCfgId_index = (CONST_qnmCfgId_base_common + 2109);                //  2008/10/30. 这里的index应理解为某项数据的唯一的标识，不应理解为数组的索引。在网络协议里，数组的索引没有意义。
    public const int CONST_qnmCfgId_uiLen = (CONST_qnmCfgId_base_common + 2110);                //  2008/10/30
    public const int CONST_qnmCfgId_ucbRetrieveAll = (CONST_qnmCfgId_base_common + 2111);               //  2010/08/17
                                                                                                        //  
    public const int CONST_qnmCfgId_subtype = (CONST_qnmCfgId_base_common + 2112);              //  2011/01/28
    public const int CONST_qnmCfgId_ucbReply = (CONST_qnmCfgId_base_common + 2113);             //  2012/08/06
    public const int CONST_qnmCfgId_ucbRetrieveOne = (CONST_qnmCfgId_base_common + 2114);               //  2014/09/04. 
    public const int CONST_qnmCfgId_help_subIndex = (CONST_qnmCfgId_base_common + 2115);                //  2016/07/23
                                                                                                        //
                                                                                                        //public const int  CONST_qnmCfgId_ucbAvConsole							=(  CONST_qnmCfgId_base_common  +  2116  );				//  2017/06/11
                                                                                                        //
    public const int CONST_qnmCfgId_ucbVideoConference = (CONST_qnmCfgId_base_common + 2117);               //  2017/08/02
    public const int CONST_qnmCfgId_usConfType = (CONST_qnmCfgId_base_common + 2118);               //  2017/08/25
    public const int CONST_qnmCfgId_usAvLevel = (CONST_qnmCfgId_base_common + 2119);                //  2011/08/15

    //
    //
    public const int CONST_qnmCfgId_mac = (CONST_qnmCfgId_base_common + 2300);              //  
    public const int CONST_qnmCfgId_ulIp = (CONST_qnmCfgId_base_common + 2301);             //  ulIp 
    public const int CONST_qnmCfgId_ip6 = (CONST_qnmCfgId_base_common + 2302);              //  reserved
    public const int CONST_qnmCfgId_pcName = (CONST_qnmCfgId_base_common + 2303);
    public const int CONST_qnmCfgId_domainName = (CONST_qnmCfgId_base_common + 2304);
    public const int CONST_qnmCfgId_osUsrName = (CONST_qnmCfgId_base_common + 2305);
    public const int CONST_qnmCfgId_langId = (CONST_qnmCfgId_base_common + 2306);
    public const int CONST_qnmCfgId_iPlatformId = (CONST_qnmCfgId_base_common + 2307);
    public const int CONST_qnmCfgId_categoryId = (CONST_qnmCfgId_base_common + 2308);               //  2011/04/05

    //
    public const int CONST_qnmCfgId_hint = (CONST_qnmCfgId_base_common + 2400);             //  2007/04/22
    public const int CONST_qnmCfgId_clientWelcomeMsg = (CONST_qnmCfgId_base_common + 2401);             //  2016/08/10
                                                                                                        //
    public const int CONST_qnmCfgId_ucbDaemonDemo = (CONST_qnmCfgId_base_common + 2410);                //  2009/10/11
    public const int CONST_qnmCfgId_usMaxVideoConferenceMems = (CONST_qnmCfgId_base_common + 2411);             //  2009/11/05
    public const int CONST_qnmCfgId_ucDays_left = (CONST_qnmCfgId_base_common + 2412);              //  2010/03/22
                                                                                                    //
    public const int CONST_qnmCfgId_uiCommEncCtxType = (CONST_qnmCfgId_base_common + 2450);             //  2008/ 06/09
    public const int CONST_qnmCfgId_publicKeyBlob = (CONST_qnmCfgId_base_common + 2451);                //  2008/06/09
    public const int CONST_qnmCfgId_sessionKeyBlob = (CONST_qnmCfgId_base_common + 2452);             //  2008/06/10
    public const int CONST_qnmCfgId_usSessionEncType = (CONST_qnmCfgId_base_common + 2453);               //  2008/06/10
    public const int CONST_qnmCfgId_sessionIv = (CONST_qnmCfgId_base_common + 2454);               //  2008/06/10

    //
    public const int CONST_qnmCfgId_messengerId = (CONST_qnmCfgId_base_common + 2500);
    public const int CONST_qnmCfgId_idInfo_grp1 = (CONST_qnmCfgId_base_common + 2501);              //  2007/08/26
                                                                                                    //
    public const int CONST_qnmCfgId_idInfo_logicalPeer = (CONST_qnmCfgId_base_common + 2502);               //  file server  需要使用logicalPeer的概念。其他的应该使用grp_related. 
                                                                                                            //
    public const int CONST_qnmCfgId_idInfo_grp_related = (CONST_qnmCfgId_base_common + 2503);               //  2015/07/30. 
    public const int CONST_qnmCfgId_idInfo_resServ = (CONST_qnmCfgId_base_common + 2504);               //  2015/08/01
    public const int CONST_qnmCfgId_idInfo_sender = (CONST_qnmCfgId_base_common + 2505);                //  2015/08/07
    public const int CONST_qnmCfgId_idInfo_creator = (CONST_qnmCfgId_base_common + 2506);               //  2017/07/27
    public const int CONST_qnmCfgId_idInfo_initiator = (CONST_qnmCfgId_base_common + 2507);
    public const int CONST_qnmCfgId_idInfo_mcu = (CONST_qnmCfgId_base_common + 2508);
    public const int CONST_qnmCfgId_idInfo_conf = (CONST_qnmCfgId_base_common + 2509);
    public const int CONST_qnmCfgId_idInfo_compere = (CONST_qnmCfgId_base_common + 2510);
    //
    public const int CONST_qnmCfgId_idInfo_requester = (CONST_qnmCfgId_base_common + 2511);


    public const int CONST_qnmCfgId_idInfo_nvr = (CONST_qnmCfgId_base_common + 2512);




    //
    public const int CONST_qnmCfgId_messengerName = (CONST_qnmCfgId_base_common + 2610);                //  messengerName
    public const int CONST_qnmCfgId_messengerPasswd = (CONST_qnmCfgId_base_common + 2611);              //  messengerPassword
                                                                                                        //
    public const int CONST_qnmCfgId_wRndName_serv = (CONST_qnmCfgId_base_common + 2616);                //  2007/11/22
    public const int CONST_qnmCfgId_uiRnd_cli = (CONST_qnmCfgId_base_common + 2617);                //  2007/11/22
                                                                                                    //  public const int 		CONST_qnmCfgId_uiWebSessId						=(  CONST_qnmCfgId_base_common  +  2618  );				//  2007/11/22
    public const int CONST_qnmCfgId_uiLogonId = (CONST_qnmCfgId_base_common + 2618);                //  2007/11/22
                                                                                                    //
    public const int CONST_qnmCfgId_grpDisplayName = (CONST_qnmCfgId_base_common + 2620);               //  2007/11/22
    public const int CONST_qnmCfgId_displayName = (CONST_qnmCfgId_base_common + 2621);              //  2007/11/22
    public const int CONST_qnmCfgId_wAdvertisement = (CONST_qnmCfgId_base_common + 2622);               //  2008/03/02
    public const int CONST_qnmCfgId_talkerDesc = (CONST_qnmCfgId_base_common + 2623);               //  2011/04/07
                                                                                                    //
                                                                                                    //  MSG_ROUTE
    public const int CONST_qnmCfgId_messengerId_from = (CONST_qnmCfgId_base_common + 2640);             //  2007/11/29
    public const int CONST_qnmCfgId_messengerId_to = (CONST_qnmCfgId_base_common + 2641);               //  2007/11/29
    public const int CONST_qnmCfgId_messengerId_replyTo = (CONST_qnmCfgId_base_common + 2642);              //  2007/11/29
                                                                                                            //
    public const int CONST_qnmCfgId_uiMsgRouteId = (CONST_qnmCfgId_base_common + 2650);             //  2008/05/27

    //
    //  3000 - 3499
    //
    public const int CONST_qnmCfgId_tCreationTime = (CONST_qnmCfgId_base_common + 3098);                //  2015/08/12
                                                                                                        //
    public const int CONST_qnmCfgId_tLastModifiedTime = (CONST_qnmCfgId_base_common + 3099);                //  2009/08/24
    public const int CONST_qnmCfgId_tLastRefreshedTime = (CONST_qnmCfgId_base_common + 3100);
    public const int CONST_qnmCfgId_idInfo_lastRefreshed = (CONST_qnmCfgId_base_common + 3101);
    public const int CONST_qnmCfgId_idInfo_grp_lastRefreshed = (CONST_qnmCfgId_base_common + 3102);             //  2007/08/26, ½ÇÉ«
                                                                                                                //
    public const int CONST_qnmCfgId_usRunningStatus = (CONST_qnmCfgId_base_common + 3110);
    public const int CONST_qnmCfgId_ulDetectedIp = (CONST_qnmCfgId_base_common + 3111);
    public const int CONST_qnmCfgId_tLastCommTime = (CONST_qnmCfgId_base_common + 3112);                //  2007/12/23, webMessengerÓÃµÄ£®
    public const int CONST_qnmCfgId_wLocation = (CONST_qnmCfgId_base_common + 3113);                //  2007/12/23, webMessengerÓÃµÄ£¬±íIP¶ÔÓ¦µÄµØ·½£®
                                                                                                    //
    public const int CONST_qnmCfgId_usMaxMemsPerSnd = (CONST_qnmCfgId_base_common + 3120);              //  2007/08/26
    public const int CONST_qnmCfgId_iRole = (CONST_qnmCfgId_base_common + 3122);                //  2007/08/26, ½ÇÉ«


    // 3500 - 3999
    public const int CONST_qnmCfgId_ui64FileLen = (CONST_qnmCfgId_base_common + 3500);              //  2007/09/03
    public const int CONST_qnmCfgId_fileName = (CONST_qnmCfgId_base_common + 3501);             //  2007/09/03
    public const int CONST_qnmCfgId_usOp = (CONST_qnmCfgId_base_common + 3502);             //  2007/09/03
    public const int CONST_qnmCfgId_tStartTime_org = (CONST_qnmCfgId_base_common + 3503);               //  2007/09/03
    public const int CONST_qnmCfgId_uiTranNo_org = (CONST_qnmCfgId_base_common + 3504);             //  2007/09/03
    public const int CONST_qnmCfgId_uiSerialNo_org = (CONST_qnmCfgId_base_common + 3505);               //  2007/09/03
    public const int CONST_qnmCfgId_uiSizePerSnd = (CONST_qnmCfgId_base_common + 3506);             //  2007/09/03
    public const int CONST_qnmCfgId_uiDataBlockIndex = (CONST_qnmCfgId_base_common + 3507);             //  2007/09/04
                                                                                                        //  public const int 		CONST_qnmCfgId_fileData								=(  CONST_qnmCfgId_base_common  +  3508  );				//  2007/09/04
    public const int CONST_qnmCfgId_rawData = (CONST_qnmCfgId_base_common + 3508);              //  2008/03/23
    public const int CONST_qnmCfgId_uiVerifyMethod = (CONST_qnmCfgId_base_common + 3509);               //  2007/09/04
    public const int CONST_qnmCfgId_uiContentType_org = (CONST_qnmCfgId_base_common + 3510);                //  2008/05/31
    public const int CONST_qnmCfgId_usCmd_org = (CONST_qnmCfgId_base_common + 3511);                //  2009/10/19
    public const int CONST_qnmCfgId_usOp_org = (CONST_qnmCfgId_base_common + 3512);             //  2009/10/19
                                                                                                //
    public const int CONST_qnmCfgId_ucbSaveVideo = (CONST_qnmCfgId_base_common + 3513);             //  2014/08/04. 这个值其实不应该放在这里.

    //
    public const int CONST_qnmCfgId_usIntervalInS_refreshRecentFriends = (CONST_qnmCfgId_base_common + 3530);               //  2007/09/20
    public const int CONST_qnmCfgId_usIntervalInS_refreshContactList = (CONST_qnmCfgId_base_common + 3531);             //  2007/09/20


    // 4000 - 4999
    public const int CONST_qnmCfgId_usMaxFps_toShareBmp = (CONST_qnmCfgId_base_common + 4035);              //  2014/08/07
                                                                                                            //
                                                                                                            //public const int 		CONST_qnmCfgId_usCntLimit_mems_from					=(  CONST_qnmCfgId_base_common  +  4036  );				//  2009/05/30
    public const int CONST_qnmCfgId_uiBytesRecorded_perBlockAlign = (CONST_qnmCfgId_base_common + 4037);                //  2009/05/24
    public const int CONST_qnmCfgId_usMaxFps_toShareBmp_conf = (CONST_qnmCfgId_base_common + 4038);             //  2009/04/19
    public const int CONST_qnmCfgId_ucVideoCompressors = (CONST_qnmCfgId_base_common + 4039);               //  2009/03/30
    public const int CONST_qnmCfgId_uiTaskType = (CONST_qnmCfgId_base_common + 4040);               //  2007/10/07							//  
    public const int CONST_qnmCfgId_ulIp_caller = (CONST_qnmCfgId_base_common + 4041);              //  IP
    public const int CONST_qnmCfgId_usCapDriverIndex = (CONST_qnmCfgId_base_common + 4042);             //  2008/03/23
                                                                                                        //public const int  CONST_qnmCfgId_fourccStr							=(  CONST_qnmCfgId_base_common  +  4043  );				//  
    public const int CONST_qnmCfgId_iFourcc = (CONST_qnmCfgId_base_common + 4043);                //  
    public const int CONST_qnmCfgId_bih_compress = (CONST_qnmCfgId_base_common + 4044);             //  bitmapInfoHeader
    public const int CONST_qnmCfgId_bih_decompress = (CONST_qnmCfgId_base_common + 4045);               //  bitmapInfoHeader
    public const int CONST_qnmCfgId_usFps = (CONST_qnmCfgId_base_common + 4046);                //  
    public const int CONST_qnmCfgId_wf_compress = (CONST_qnmCfgId_base_common + 4047);              //  2008/06/15
    public const int CONST_qnmCfgId_wf_decompress = (CONST_qnmCfgId_base_common + 4048);                //  2009/04/27
    public const int CONST_qnmCfgId_ucAudioCompressors = (CONST_qnmCfgId_base_common + 4049);               //  2009/05/17
    public const int CONST_qnmCfgId_uiTranNo_openVideoDev = (CONST_qnmCfgId_base_common + 4050);                //  
    public const int CONST_qnmCfgId_uiTranNo_openAudioDev = (CONST_qnmCfgId_base_common + 4051);                //  
                                                                                                                //public const int 		CONST_qnmCfgId_ucbFlippedImg						=(  CONST_qnmCfgId_base_common  +  4052  );				//  2011/12/11
    public const int CONST_qnmCfgId_spsPps = (CONST_qnmCfgId_base_common + 4053);               //  2014/03/30
                                                                                                //
    public const int CONST_qnmCfgId_uiSampleTimeInMs = (CONST_qnmCfgId_base_common + 4054);             //  2009/05/04
    public const int CONST_qnmCfgId_ui_rtTimeLen = (CONST_qnmCfgId_base_common + 4055);             //  2009/05/27
                                                                                                    //public const int 	CONST_qnmCfgId_uiSampleTimeInMs_org					=(  CONST_qnmCfgId_base_common  +  4056  );				//  2009/05/04
                                                                                                    //
    public const int CONST_qnmCfgId_uiPts = (CONST_qnmCfgId_base_common + 4057);             //  2015/01/15
    public const int CONST_qnmCfgId_ucbKeyFrame = (CONST_qnmCfgId_base_common + 4058);

    //
    public const int CONST_qnmCfgId_tLastTime_showFrameInfo_020534 = (CONST_qnmCfgId_base_common + 4060);               //  
    public const int CONST_qnmCfgId_usCnt_pkts = (CONST_qnmCfgId_base_common + 4061);               //  
    public const int CONST_qnmCfgId_tSelectTime = (CONST_qnmCfgId_base_common + 4062);              //  2008/12/05
    public const int CONST_qnmCfgId_usDiff_pkts = (CONST_qnmCfgId_base_common + 4063);             //  2009/04/18

    //
    public const int CONST_qnmCfgId_ucbNeedKeyFrameFirst = (CONST_qnmCfgId_base_common + 4070);

    //
    public const int CONST_qnmCfgId_usMaxSpeakers = (CONST_qnmCfgId_base_common + 4201);                //  2009/05/31
    public const int CONST_qnmCfgId_usIndex_speaker = (CONST_qnmCfgId_base_common + 4202);             //  2013/07/18
    public const int CONST_qnmCfgId_conf_iFourcc = (CONST_qnmCfgId_base_common + 4203);             //  

    //
    public const int CONST_qnmCfgId_usLayoutType = (CONST_qnmCfgId_base_common + 4205);             //  2010/12/27	

    //
    public const int CONST_qnmCfgId_bFlow512k_hg = (CONST_qnmCfgId_base_common + 4218);


    //  2014/08/15
    public const int CONST_qnmCfgId_iWidth_pic_conf = (CONST_qnmCfgId_base_common + 4230);          //  2014/08/15
    public const int CONST_qnmCfgId_iHeight_pic_conf = (CONST_qnmCfgId_base_common + 4231);         //  2014/08/15
                                                                                                    //
                                                                                                    //public const int 		CONST_qnmCfgId_ucbShowBgWall_starter					=(  CONST_qnmCfgId_base_common  +  4232  );			//  2016/03/26
                                                                                                    //
    public const int CONST_qnmCfgId_width = (CONST_qnmCfgId_base_common + 4233);            //  2017/06/12
    public const int CONST_qnmCfgId_height = (CONST_qnmCfgId_base_common + 4234);           //  2017/06/12
    public const int CONST_qnmCfgId_ucbConfCtrl_starter = (CONST_qnmCfgId_base_common + 4235);          //  2017/07/06


    //
    public const int CONST_qnmCfgId_uiTranNo_avTask_starter = (CONST_qnmCfgId_base_common + 4250);          //  2014/11/22
    public const int CONST_qnmCfgId_ucAvFlg = (CONST_qnmCfgId_base_common + 4251);          //  2015/03/12


    //
    public const int CONST_qnmCfgId_startTime_task = (CONST_qnmCfgId_base_common + 4300);
    public const int CONST_qnmCfgId_usMcuType = (CONST_qnmCfgId_base_common + 4301);
    //
    public const int CONST_qnmCfgId_ca_sendData = (CONST_qnmCfgId_base_common + 4492);
    public const int CONST_qnmCfgId_ca_usrName = (CONST_qnmCfgId_base_common + 4493);
    public const int CONST_qnmCfgId_ca_tmp_passwd = (CONST_qnmCfgId_base_common + 4494);                //  2023/05/16
                                                                                                        //
    public const int CONST_qnmCfgId_sxrz_servReply = (CONST_qnmCfgId_base_common + 4500);
    public const int CONST_qnmCfgId_sxrz_cliData = (CONST_qnmCfgId_base_common + 4501);
    public const int CONST_qnmCfgId_sHgCmd = (CONST_qnmCfgId_base_common + 4502);
    public const int CONST_qnmCfgId_ca_random = (CONST_qnmCfgId_base_common + 4503);
    public const int CONST_qnmCfgId_caGwCfg = (CONST_qnmCfgId_base_common + 4504);


    //
    public const int CONST_qnmCfgId_usEnlargeType = (CONST_qnmCfgId_base_common + 4600);                //  
    public const int CONST_qnmCfgId_iChannel = (CONST_qnmCfgId_base_common + 4601);
    public const int CONST_qnmCfgId_ip = (CONST_qnmCfgId_base_common + 4602);
    public const int CONST_qnmCfgId_mouseStatus = (CONST_qnmCfgId_base_common + 4603);
    public const int CONST_qnmCfgId_ucbOneBigLayout = (CONST_qnmCfgId_base_common + 4604);
    public const int CONST_qnmCfgId_messengerId_oneBig = (CONST_qnmCfgId_base_common + 4605);
    public const int CONST_qnmCfgId_ii = (CONST_qnmCfgId_base_common + 4606);
    public const int CONST_qnmCfgId_curr_idinfo = (CONST_qnmCfgId_base_common + 4607);
    public const int CONST_qnmCfgId_enlarge_x = (CONST_qnmCfgId_base_common + 4608);
    public const int CONST_qnmCfgId_enlarge_y = (CONST_qnmCfgId_base_common + 4609);
    public const int CONST_qnmCfgId_phone_usEnlargeType = (CONST_qnmCfgId_base_common + 4610);
    public const int CONST_qnmCfgId_amplificationType = (CONST_qnmCfgId_base_common + 4611);
    public const int CONST_qnmCfgId_channelName = (CONST_qnmCfgId_base_common + 4612);

    // 5000 - 5999
    public const int CONST_qnmCfgId_uiVDevId = (CONST_qnmCfgId_base_common + 5000);             //  2008/01/25
    public const int CONST_qnmCfgId_name = (CONST_qnmCfgId_base_common + 5001);             //  2009/08/25
    public const int CONST_qnmCfgId_model = (CONST_qnmCfgId_base_common + 5002);                //  2008/01/25
    public const int CONST_qnmCfgId_smsc = (CONST_qnmCfgId_base_common + 5003);             //  2008/01/25
    public const int CONST_qnmCfgId_status = (CONST_qnmCfgId_base_common + 5004);               //  2008/01/25
    public const int CONST_qnmCfgId_cusName = (CONST_qnmCfgId_base_common + 5005);              //  2008/01/25
    public const int CONST_qnmCfgId_ucbDefaultSmServer = (CONST_qnmCfgId_base_common + 5006);               //  2008/01/25

    //
    public const int CONST_qnmCfgId_wDevIdStr_lastRefreshed = (CONST_qnmCfgId_base_common + 5020);              //  2008/02/01

    //
    public const int CONST_qnmCfgId_usRaType = (CONST_qnmCfgId_base_common + 5050);             //  2008/11/15
    public const int CONST_qnmCfgId_ucButtonMask = (CONST_qnmCfgId_base_common + 5051);
    public const int CONST_qnmCfgId_x = (CONST_qnmCfgId_base_common + 5052);                //  2008/11/12. x×ø±ê
    public const int CONST_qnmCfgId_y = (CONST_qnmCfgId_base_common + 5053);                //  2008/11/12  y×ø±ê
                                                                                            //  2016/12/26
                                                                                            //public const int 		CONST_qnmCfgId_uiEventId_unused							=(  CONST_qnmCfgId_base_common  +  2854  );				//  ÊÂ¼þµÄ±êÊ¶¡£
                                                                                            //
    public const int CONST_qnmCfgId_key = (CONST_qnmCfgId_base_common + 5056);              //  ¼üÖµ
    public const int CONST_qnmCfgId_ucbDown = (CONST_qnmCfgId_base_common + 5057);              //  ÊÇ·ñ°´ÏÂ


    //
    public const int CONST_qnmCfgId_uiTranNo_retrieveAll = (CONST_qnmCfgId_base_common + 5170);         //  2017/09/17
    public const int CONST_qnmCfgId_usSeqNo_retrievePart = (CONST_qnmCfgId_base_common + 5171);         //  2017/09/17
    public const int CONST_qnmCfgId_usSeqNo = (CONST_qnmCfgId_base_common + 5172);          //  2017/09/17


    // 6000 - 
    //  public const int 		CONST_qnmCfgId_										=(  =(  CONST_qnmCfgId_base_common  +  6000  );		//  
    public const int CONST_qnmCfgId_uiRuleType = (CONST_qnmCfgId_base_common + 6001);
    public const int CONST_qnmCfgId_nCmd = (CONST_qnmCfgId_base_common + 6002);

    //  gps
    public const int CONST_qnmCfgId_iLaInM10000f = (CONST_qnmCfgId_base_common + 6200);         //  4200. 2012/04/20
    public const int CONST_qnmCfgId_iLoInM10000f = (CONST_qnmCfgId_base_common + 6201);         //  

    //  ptz
    public const int CONST_qnmCfgId_nAddr = (CONST_qnmCfgId_base_common + 6300);            //  4300, 2012/08/07
    public const int CONST_qnmCfgId_nPelcoType = (CONST_qnmCfgId_base_common + 6301);           //  


    //  2014/08/28
    public const int CONST_qnmCfgId_usTaskProcObjType = (CONST_qnmCfgId_base_common + 6400);            //  4400. 2014/08/28


    //  2015/08/13
    public const int CONST_qnmCfgId_ucbP2p = (CONST_qnmCfgId_base_common + 6504);               //  4504.  //  2015/08/05
    public const int CONST_qnmCfgId_index_page = (CONST_qnmCfgId_base_common + 6505);
    public const int CONST_qnmCfgId_max_usCnt = (CONST_qnmCfgId_base_common + 6506);                //  max of usCnt
    public const int CONST_qnmCfgId_keyword = (CONST_qnmCfgId_base_common + 6507);              //  


    //
    public const int CONST_qnmCfgId_testSpeed_data = (CONST_qnmCfgId_base_common + 6620);   //  2016/12/01

    //
    public const int CONST_qnmCfgId_mcuUsrName = (CONST_qnmCfgId_base_common + 6700);
    public const int CONST_qnmCfgId_mcuPasswd = (CONST_qnmCfgId_base_common + 6701);
    public const int CONST_qnmCfgId_mcuDomainName = (CONST_qnmCfgId_base_common + 6702);



    //  2008/05/15
    // 30000 - 31999
    public const int CONST_qnmCfgId_debug_base = 30000;                                                 //  2015/09/01. 把dbg的定义放高点。以后就固定下来。

    //
    public const int CONST_qnmCfgId_tTime_start = (CONST_qnmCfgId_debug_base + 1);
    public const int CONST_qnmCfgId_usElapsed_toSend = (CONST_qnmCfgId_debug_base + 2);
    //  public const int 		CONST_qnmCfgId_sElapsed_videoConferenceRecved		=(  CONST_qnmCfgId_debug_base  +  3  );

    public const int CONST_qnmCfgId_tTime_serv_startToRecv = (CONST_qnmCfgId_debug_base + 20);
    public const int CONST_qnmCfgId_usElapsed_serv_recved = (CONST_qnmCfgId_debug_base + 21);
    public const int CONST_qnmCfgId_usElapsed_serv_startToProcess = (CONST_qnmCfgId_debug_base + 22);
    public const int CONST_qnmCfgId_usElapsed_serv_toFindDst = (CONST_qnmCfgId_debug_base + 23);
    public const int CONST_qnmCfgId_usElapsed_serv_toBeSwitched = (CONST_qnmCfgId_debug_base + 24);
    public const int CONST_qnmCfgId_usElapsed_serv_toSend = (CONST_qnmCfgId_debug_base + 25);

    public const int CONST_qnmCfgId_dwTickCnt_peer_recved = (CONST_qnmCfgId_debug_base + 40);
    public const int CONST_qnmCfgId_usElapsed_peer_resp_toSend = (CONST_qnmCfgId_debug_base + 41);
    public const int CONST_qnmCfgId_tTime_peer_recved = (CONST_qnmCfgId_debug_base + 42);

    public const int CONST_qnmCfgId_tTime_serv_resp_startToRecv = (CONST_qnmCfgId_debug_base + 60);
    public const int CONST_qnmCfgId_usElapsed_serv_resp_recved = (CONST_qnmCfgId_debug_base + 61);
    public const int CONST_qnmCfgId_usElapsed_serv_resp_startToProcess = (CONST_qnmCfgId_debug_base + 62);
    public const int CONST_qnmCfgId_usElapsed_serv_resp_toFindDst = (CONST_qnmCfgId_debug_base + 63);
    public const int CONST_qnmCfgId_usElapsed_serv_resp_toBeSwitched = (CONST_qnmCfgId_debug_base + 64);
    public const int CONST_qnmCfgId_usElapsed_serv_resp_toSend = (CONST_qnmCfgId_debug_base + 65);


    //  
    //  #define		CONST_qnmCfgId_dwTickCnt_resp_recved				=(  CONST_qnmCfgId_debug_base  +  80  );
    //  #define		CONST_qnmCfgId_usElapsed_resp_end					=(  CONST_qnmCfgId_debug_base  +  81  );

    public const int CONST_qnmCfgId_ucbGetTotal = (CONST_qnmCfgId_debug_base + 200);
    public const int CONST_qnmCfgId_netStats_total = (CONST_qnmCfgId_debug_base + 201);
    public const int CONST_qnmCfgId_qmdStatus_msgrMem = (CONST_qnmCfgId_debug_base + 202);
    public const int CONST_qnmCfgId_qmdStatus_channelMem = (CONST_qnmCfgId_debug_base + 203);
    //
    public const int CONST_qnmCfgId_showInfoFlgs = (CONST_qnmCfgId_debug_base + 210);   //  2015/09/01

    // 32000 - 
    public const int CONST_qnmCfgId_user = 32000;





    //
    public const int CONST_qyCmd_getCfgs = (CONST_qyCmd_base + 2);
    //  2007/04/23, mis
    //public const int CONST_qyCmd_showInfo = (CONST_qyCmd_base + 200);   //  
    public const int CONST_qyCmd_showInfo_small = (CONST_qyCmd_base + 201);   //  


    //
    public const int CONST_qyCmd_refreshImObjListReq = (CONST_qyCmd_base + 205);    //  ??????????2007/05/06????????????imCommType???
                                                                                    //
    public const int CONST_qyCmd_refreshRecentFriendsReq = (CONST_qyCmd_base + 207);    //  ??????????2007/05/10
                                                                                        //
    public const int CONST_qyCmd_mcComm = (CONST_qyCmd_base + 210); //  2011/01/08
                                                                    //
    public const int CONST_qyCmd_sendReq = (CONST_qyCmd_base + 220);

    //
    public const int CONST_qyCmd_talkTo = (CONST_qyCmd_base + 300); //  ???2007/04/09

    public const int CONST_qyCmd_sendTask = (CONST_qyCmd_base + 320);   //  ???????2007/06/23
    public const int CONST_qyCmd_sendTaskReply = (CONST_qyCmd_base + 321);  //  ???????2007/06/23
    public const int CONST_qyCmd_sendRobotTask = (CONST_qyCmd_base + 322);  //  ????????
    public const int CONST_qyCmd_sendRobotTaskReply = (CONST_qyCmd_base + 323); //  ????????
    public const int CONST_qyCmd_sendRobotTaskData = (CONST_qyCmd_base + 324);  //  ????????????????channel????????????????????????channel????, ????????channel???; ?????????????channel, ???channel????
                                                                                //
    public const int CONST_qyCmd_sendMedia = (CONST_qyCmd_base + 330);  //  ??????
                                                                        //
    public const int CONST_qyCmd_lastMsgInSession = (CONST_qyCmd_base + 340);   //  webMessenger?cgi????????????????????????daemon?????????????????????????????????????2007/12/11
                                                                                //
    public const int CONST_qyCmd_sendVDevReq = (CONST_qyCmd_base + 350);    //  ??????????????????,2008/01/24



    //
    public const int CONST_qnmMaxRegFields = 30;                    //  ÒÔÏÂµÄÉèÖÃÖµ²»ÄÜ³¬¹ý´ËÏî, Ö¸cols, »¹ÓÐ¼¸ÏîÈçmac0µÈ£¬²»°üº¬ÔÚÄÚ

    //	
    //  ×¢²áÐÅÏ¢µÄ×Ö¶Î¶¨Òå
    public const int CONST_fieldId_null = 0;
    public const int CONST_fieldId_yhlx = 100;
    public const int CONST_fieldId_syr = 200;
    public const int CONST_fieldId_bm = 201;
    public const int CONST_fieldId_dw = 202;
    public const int CONST_fieldId_qtdw = 203;
    public const int CONST_fieldId_bb = 204;
    public const int CONST_fieldId_zb = 205;
    //
    public const int CONST_fieldId_zcbh0 = 220;
    public const int CONST_fieldId_zcbh1 = 221;
    public const int CONST_fieldId_zcbh2 = 222;
    //  public const int 		CONST_fieldId_zcsm0								223
    public const int CONST_fieldId_zcsm1 = 224;
    public const int CONST_fieldId_zcsm2 = 225;
    public const int CONST_fieldId_jqlx = 240;
    public const int CONST_fieldId_lxdh = 241;
    public const int CONST_fieldId_sydd = 242;
    public const int CONST_fieldId_zyyt = 243;
    public const int CONST_fieldId_bz1 = 244;
    public const int CONST_fieldId_bz2 = 245;

    /////////////////////
    ///
    public const int CONST_qyEventType_is_base = 20000;         //  2007/05/08, mis
                                                                //
    public const int CONST_qyEventType_im_newMessenger = (CONST_qyEventType_is_base + 100);
    //
    public const int CONST_qyEventType_im_messengerLogon = (CONST_qyEventType_is_base + 110);
    public const int CONST_qyEventType_im_messengerLogoff = (CONST_qyEventType_is_base + 111);
    //
    public const int CONST_qyEventType_im_connErr = (CONST_qyEventType_is_base + 115);  //  
    public const int CONST_qyEventType_im_memoryErr = (CONST_qyEventType_is_base + 116);    //  2011/02/05

    //
    //  ÒÔÏÂÍ¨ÐÅµÄÊý¾Ý°ü¸ñÊ½¡£

    public const int CONST_qnmAuthCmd_startSession = 0;                 //  ÊÜ¿Ø¶Ë£¨»òÆäËû£©Á¬½Ó·þÎñÆ÷£¬ÒÔÆô¶¯Ò»¸öÕý³£µÄ»á»°  
    public const int CONST_qnmAuthCmd_getHint = 1;                  //  ×¢²á½çÃæÁ¬½Ó·þÎñÆ÷£¬ÒÔÈ¡µÃÌáÊ¾ÐÅÏ¢£¬ÏÞÓÃÔÚnetMc2
    public const int CONST_qnmAuthCmd_startProxySession = 2;                    //  ·þÎñÆ÷Á¬½Ó´úÀí£¬ÒÔÆô¶¯´úÀí»á»°
    public const int CONST_qnmAuthCmd_guiTalk = 3;                  //  ½çÃæÁ¬½ÓDaemon, ÒÔÍê³ÉÄ³Ð©Ö¸Áî
    public const int CONST_qnmAuthCmd_cgiTalk = 4;                  //  cgiÁª½ÓDaemon, ÒÔÍê³ÉwebÏà¹ØµÄ¹¤×÷£¬2007/11/21
    public const int CONST_qnmAuthCmd_clientHello = 5;					//  2008/06/09


}



namespace qy
{
    public struct QY_CFGITEM
    {
        public ushort cfgId;
        public ushort dataType;
    }
    ;

    public struct QY_CFGITEM_L32
    {
        public ushort cfgId;
        public ushort dataType;
        public uint ulVal;
    }
        ;


    public unsafe struct QY_CFGITEM_L64
    {           //  ÒòÎª´Ó×Ö½ÚË³ÐòÉÏ£¬¿ÉÄÜ__int64ºÍunsigned  short²»ÄÜË³×Ö½Ú´æ·Å£¬¹ÊÖ»ÄÜÓÃÒ»¸öchar»º´æÀ´Ìæ»»
        public ushort cfgId;
        public ushort dataType;
        public fixed byte buf_l64Val[8];   //  unsigned  __int64								ui64Val;
    }
        ;

    public struct QY_CFGITEM_BYTE
    {           //  2007/08/05
        public ushort cfgId;
        public ushort dataType;
        public byte ucVal;
    }
        ;

    public struct QY_CFGITEM_SHORT
    {           //  2007/08/05
        public ushort cfgId;
        public ushort dataType;
        public ushort usVal;
    }
        ;


    /*
	typedef  struct  __qyCfgItemStr_t								{				//  netMcÀïÊ¹ÓÃµÄ£¬ÒòÎªstrµÄ³¤¶È¶ÔisÀ´Ëµ²»¹»ÓÃ£¬¹ÊÀ©µ½ÏÂÃæµÄ³¤¶È¡£µ«ÊÇÒª×¢Òâ£ºÔ­À´µÄ³¤¶ÈÊÇ¡¶1024µÄ
					 unsigned  short								cfgId;
					 unsigned  short								dataType;
					 unsigned  short								len;
					 char											str[1024];
	}		 QY_CFGITEM_STR;
	*/


    public unsafe struct QY_CFGITEM_STR
    {
        public ushort cfgId;
        public ushort dataType;
        public ushort len;
        public fixed byte str[Consts.CONST_cntof_cfgItemStr];     //  2007/03/31, ½«»º³åµÄ³¤¶È±ä³¤ÁË
    }
    ;

    public unsafe struct QY_CFGITEM_WSTR
    {               //  2007/03/31
        public ushort cfgId;
        public ushort dataType;
        public ushort len;
        public fixed char wStr[Consts.CONST_cntof_cfgItemWstr];
    }
        ;



    public unsafe struct QY_CFGITEM_DATA
    {
        public ushort cfgId;
        public ushort dataType;
        public ushort len;
        public fixed byte buf[Consts.bufSize_cfgItemData + 1];
    }
        ;

#if false
	public unsafe struct QY_CFGITEM_LDATA
	{
		public ushort cfgId;
		public ushort dataType;
		public uint ulLen;
		public fixed byte buf[Consts.bufSize_cfgItemLData + 1];
	};
#endif
    public unsafe struct QY_CFGITEM_LDATA
    {
        public ushort cfgId;
        public ushort dataType;
        public uint ulLen;
        public fixed byte buf1[1];
        public byte* p;
    };


    //  2010/07/09
    [StructLayout(LayoutKind.Explicit)]
    public struct QY_CFGITEM_ntoh_U
    {
        [FieldOffset(0)] public QY_CFGITEM head;
        [FieldOffset(0)] public QY_CFGITEM_L32 l32;
        [FieldOffset(0)] public QY_CFGITEM_L64 l64;
        [FieldOffset(0)] public QY_CFGITEM_BYTE c;
        [FieldOffset(0)] public QY_CFGITEM_SHORT s;
        public unsafe struct QY_CFGITEM_ntoh_U_data
        {
            public ushort cfgId;
            public ushort dataType;
            public ushort len;
            public fixed byte buf[1];
            public byte* p;
        };
        [FieldOffset(0)] public QY_CFGITEM_ntoh_U_data data;
        public unsafe struct QY_CFGITEM_ntoh_U_lData
        {
            public ushort cfgId;
            public ushort dataType;
            public uint ulLen;
            public fixed byte buf[1];
            public byte* p;
        };
        [FieldOffset(0)] public QY_CFGITEM_ntoh_U_lData lData;
        public unsafe struct QY_CFGITEM_ntoh_U_wStr
        {               //  2007/03/31
            public ushort cfgId;
            public ushort dataType;
            public ushort len;
            public fixed char wBuf[1];
            public char* pWStr;
        };
        [FieldOffset(0)] public QY_CFGITEM_ntoh_U_wStr wStr;

    };

















    public unsafe struct QNM_CNT_CFG
    {
        public bool ucbAutoCntIp;     //  2006/06/29
        public fixed byte cntIp[Consts.CONST_qyMaxIpLen + 1];
        public bool ucbSys_cntIp;                         //  ÊÇ·ñhkey_local_machineÀïµÄ¡£2009/01/16
        public fixed byte cntAddr1[Consts.CONST_maxCntAddrLen + 1];
        public fixed byte cntAddr2[Consts.CONST_maxCntAddrLen + 1];
        public fixed bool ucbSys_cntAddrs[2];                      //  ÊÇ·ñhkey_local_machineÀïµÄ¡£2009/01/16
                                                                   //
        public ushort usCntPort;
        public bool ucbSys_cntPort;                           //  hkey_local_machine;  2010/09/24
                                                              //
        public fixed char usr[256];                             //  2019/04/08
        public fixed char passwd[256];                          //  2019/04/08
        public bool ucbNoSavePasswd;                      //  2019/04/08
        public bool ucbNoAutoLogon;                           //  2019/04/08
    }
   ;

    public struct QWM_DYN_CFG
    {                   //  2006/07/01	
        public QNM_CNT_CFG cnt;
    }
    public struct QY_ENV
    {
        public int iEnvType;
        public uint iPlatformId;
        public ushort usLangId;                            //  ²Ù×÷ÏµÍ³ÓïÑÔÀàÐÍ£¬2005/11/03
                                                           //
        public bool bDebugInfo;                            //  2008/11/19. ÊÇ·ñÏÔÊ¾Ò»Ð©debugÐÅÏ¢£¬¶ÔÔÚrelease°æ±¾ÏÂÏÔÊ¾Ä³Ð©ÐÅÏ¢ÓÐÓÃ


    };


    public unsafe struct QNM_COMM_AUTHINFO
    {

        public fixed byte ver[Consts.CONST_qyMaxVerLen];
        //public byte macs[CONST_qnmMaxMacs][12];
        //		 char otherMacs[255];
        public fixed byte ip[Consts.CONST_qyMaxIpLen];

        //  ÒÔÉÏÊÇ¾É°æ±¾µÄÊý¾Ý³ÉÔ±
        public byte cmd;               //  (1.4)°æ±¾Ç°Óëº¬(1.4)°æ±¾ºóÖ®¼äµÄÖ÷ÒªÊý¾Ý½á¹¹ÉÏµÄÇø±ð
        public uint uiQwmType;         //  (0208)ºóÖ§³Ö 

    }
    ;



    partial class qyFuncs
    {


        public static unsafe byte* myWChar2Utf8(string wStr, byte* buf, uint bufSize)
        {
            int iErr = -1;
            int bufSizeNeeded = 0;
            /*
						UINT uiCodePage = CP_UTF8;

						bufSizeNeeded = WideCharToMultiByte(uiCodePage, 0, wStr, -1, buf, 0, NULL, NULL);
						if (bufSizeNeeded > (int)bufSize) goto errLabel;
						if (bufSizeNeeded != WideCharToMultiByte(uiCodePage, 0, wStr, -1, buf, bufSize, NULL, NULL)) goto errLabel;
			*/
            char[] pw = wStr.ToCharArray();
            string str = Encoding.UTF8.GetString(
                Encoding.Convert(
                    Encoding.Unicode,
                    Encoding.UTF8,
                    Encoding.Unicode.GetBytes(pw)));

            safeStrnCpy(str, buf, bufSize);

            iErr = 0;
        errLabel:
            return (iErr != 0) ? null : buf;
        }


        //
        public static unsafe char* myUtf82WChar(byte* pBuf, char* wBuf, uint cntof_wBuf)
        {
            return myUtf82WChar(mytoString(pBuf), wBuf, cntof_wBuf);
        }



        public static unsafe char* myUtf82WChar(string str, char* wBuf, uint cntof_wBuf)
        {
            int iErr = -1;
            int wBufSizeNeeded = 0;
            /*
			UINT uiCodePage = CP_UTF8;

			wBufSizeNeeded = MultiByteToWideChar(uiCodePage, 0, str, -1, wBuf, 0);
			if (wBufSizeNeeded > (int)wBufSize) goto errLabel;
			if (wBufSizeNeeded != MultiByteToWideChar(uiCodePage, 0, str, -1, wBuf, wBufSize)) goto errLabel;
			*/
            char[] pw = str.ToCharArray();
            string wstr = Encoding.Unicode.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.Unicode,
                    Encoding.UTF8.GetBytes(pw)));
            mytcsncpy(wBuf, wstr, (int)cntof_wBuf);

            iErr = 0;
        errLabel:
            return iErr != 0 ? null : wBuf;
        }




        //  ·µ»Ø×ª»¯ºóµÄ×Ö½ÚÁ÷³¤¶È, 0 ±íÊ§°Ü
        public static unsafe int qyhtonCfgItem(QY_CFGITEM* pItem, uint* puiPart0Len, byte** ppPart1Data, uint* puiPart1DataLen)
        {
            int len = 0;

            if (null == puiPart0Len) return -1;
            if (null == ppPart1Data) return -1;
            if (null == puiPart1DataLen) return -1;

            *puiPart0Len = 0;
            *ppPart1Data = null;
            *puiPart1DataLen = 0;

            bool bGot_puiPart0Len = false;

            //
            pItem->cfgId = qyhtons(pItem->cfgId);

            switch (pItem->dataType)
            {
                case Consts.CONST_qyDataType_long:
                    {
                        QY_CFGITEM_L32* pLong = (QY_CFGITEM_L32*)pItem;

                        len = 2 + 2 + 4;
                        pLong->dataType = qyhtons(pLong->dataType);
                        pLong->ulVal = qyhtoni(pLong->ulVal);
                    }
                    break;
                case Consts.CONST_qyDataType_char:
                    {
                        QY_CFGITEM_BYTE* pChar = (QY_CFGITEM_BYTE*)pItem;
                        //
                        len = 2 + 2 + 1;
                        pChar->dataType = qyhtons(pChar->dataType);
                        //
                    }
                    break;
                case Consts.CONST_qyDataType_short:
                    {
                        QY_CFGITEM_SHORT* pShort = (QY_CFGITEM_SHORT*)pItem;
                        //
                        len = 2 + 2 + 2;
                        pShort->dataType = qyhtons(pShort->dataType);
                        pShort->usVal = qyhtons(pShort->usVal);
                    }
                    break;
                case Consts.CONST_qyDataType_l64:
                    {
                        QY_CFGITEM_L64* pL64 = (QY_CFGITEM_L64*)pItem;
                        //
                        len = 2 + 2 + 8;
                        pL64->dataType = qyhtons(pL64->dataType);
                        *(UInt64*)pL64->buf_l64Val = qyhtonl64(*(UInt64*)pL64->buf_l64Val);
                        //				   
                    }
                    break;
                case Consts.CONST_qyDataType_data:
                case Consts.CONST_qyDataType_str:
                case Consts.CONST_qyDataType_utf8Str:
                    {
                        QY_CFGITEM_STR* pStr = (QY_CFGITEM_STR*)pItem;

                        len = 2 + 2 + 2 + pStr->len;
                        pStr->dataType = qyhtons(pStr->dataType);
                        pStr->len = qyhtons(pStr->len);
                    }
                    break;
                case Consts.CONST_qyDataType_lData:
                    {       //  2008/04/01
                        QY_CFGITEM_LDATA* pLData = (QY_CFGITEM_LDATA*)pItem;

                        //
                        *puiPart0Len = 2 + 2 + 4;
                        bGot_puiPart0Len = true;
                        *ppPart1Data = pLData->p;
                        *puiPart1DataLen = pLData->ulLen;

                        //
                        len = (int)(2 + 2 + 4 + pLData->ulLen);
                        pLData->dataType = qyhtons(pLData->dataType);
                        pLData->ulLen = qyhtoni(pLData->ulLen);


                    }
                    break;
                default:
                    len = 0;
                    break;
            }

            if (!bGot_puiPart0Len)
            {
                *puiPart0Len = (uint)len;
            }

            return len;

        }

        //  

        //  2010/07/09

        public static unsafe int qyntohCfgItem(byte* p, QY_CFGITEM_ntoh_U* pItem)
        {
            int len = 0;
            int headLen = sizeof(QY_CFGITEM);
            int dataLen = 0;

            //  pItem->cfgId  =  qyntohs(  pItem->cfgId  );
            //  pItem->dataType  =  qyntohs(  pItem->dataType  );
            mymemset((IntPtr)pItem, 0, sizeof(QY_CFGITEM_ntoh_U));
            mymemcpy((IntPtr)pItem, (IntPtr)p, (uint)sizeof(QY_CFGITEM));
            pItem->head.cfgId = qyntohs(pItem->head.cfgId);
            pItem->head.dataType = qyntohs(pItem->head.dataType);

            switch (pItem->head.dataType)
            {
                case Consts.CONST_qyDataType_long:
                    {

                        dataLen = 4;
                        mymemcpy((IntPtr)(((byte*)pItem) + headLen), (IntPtr)p + headLen, (uint)dataLen);

                        QY_CFGITEM_L32* pLong = (QY_CFGITEM_L32*)pItem;

                        //  pLong->ulVal  =  qyntohl(  pLong->ulVal  );
                        pLong->ulVal = qyntohi(pLong->ulVal);
                        //
                        int iVal = (int)pLong->ulVal;

                        //
                        len = 2 + 2 + 4;
                    }
                    break;
                case Consts.CONST_qyDataType_char:
                    {

                        dataLen = 1;
                        mymemcpy((IntPtr)(((byte*)pItem) + headLen), (IntPtr)p + headLen, (uint)dataLen);

                        QY_CFGITEM_BYTE* pChar = (QY_CFGITEM_BYTE*)pItem;
                        //
                        len = 2 + 2 + 1;
                        //
                    }
                    break;
                case Consts.CONST_qyDataType_short:
                    {

                        dataLen = 2;
                        mymemcpy((IntPtr)((byte*)pItem) + headLen, (IntPtr)p + headLen, (uint)dataLen);

                        QY_CFGITEM_SHORT* pShort = (QY_CFGITEM_SHORT*)pItem;
                        //
                        pShort->usVal = qyntohs(pShort->usVal);

                        len = 2 + 2 + 2;
                    }
                    break;
                case Consts.CONST_qyDataType_l64:
                    {

                        dataLen = 8;
                        mymemcpy((IntPtr)((byte*)pItem) + headLen, (IntPtr)p + headLen, (uint)dataLen);

                        QY_CFGITEM_L64* pL64 = (QY_CFGITEM_L64*)pItem;
                        //
                        *(UInt64*)pL64->buf_l64Val = qyntohl64(*(UInt64*)pL64->buf_l64Val);

                        len = 2 + 2 + 8;
                    }
                    break;
                case Consts.CONST_qyDataType_data:
                case Consts.CONST_qyDataType_str:
                case Consts.CONST_qyDataType_utf8Str:
                    {

                        dataLen = 2;
                        mymemcpy((IntPtr)((byte*)pItem) + headLen, (IntPtr)p + headLen, (uint)dataLen);
                        pItem->data.p = p + headLen + dataLen;

                        QY_CFGITEM_STR* pStr = (QY_CFGITEM_STR*)pItem;

                        //  pStr->len  =  qyntohs(  pStr->len  );
                        pStr->len = qyntohs(pStr->len);


                        //  len  =  2  +  2  +  2  +  pStr->len;
                        ushort tmp_us;
                        //memcpy(&tmp_us, &pStr->len, sizeof(short));
                        tmp_us = pStr->len;
                        len = 2 + 2 + 2 + tmp_us;
                    }
                    break;
                case Consts.CONST_qyDataType_lData:
                    {       //  2008/04/01

                        dataLen = 4;
                        mymemcpy((IntPtr)((byte*)pItem) + headLen, (IntPtr)p + headLen, (uint)dataLen);
                        pItem->lData.p = p + headLen + dataLen;

                        QY_CFGITEM_LDATA* pLData = (QY_CFGITEM_LDATA*)pItem;
                        //
                        pLData->ulLen = qyntohi(pLData->ulLen);

                        //  len  =  2  +  2  +  4  +  pLData->ulLen;
                        uint tmp_ul;
                        //memcpy(&tmp_ul, &pLData->ulLen, sizeof(long));
                        tmp_ul = pLData->ulLen;
                        len = (int)(2 + 2 + 4 + tmp_ul);
                    }
                    break;
                default:
                    len = 0;
                    break;
            }


            return len;

        }


        public static unsafe int getIpFromName(string name, byte* ip, uint size)
        {
            int iErr = -1;

            if (name == null) return -1;
            if (ip == null || size <= Consts.CONST_qyMaxIpLen) return -1;

            //
            name.Trim();
            if (name.Length == 0) return -1;

            //
            try
            {
                IPHostEntry hostInfo = Dns.GetHostByName(name);
                IPAddress[] address = hostInfo.AddressList;
                if (address == null || address.Length == 0) goto errLabel;
                safeStrnCpy(address[0].ToString(), ip, size);
            }
            catch (Exception e)
            {
                goto errLabel;
            }

            iErr = 0;
        errLabel:
            return iErr;
        }


        public static int getQyEnv(IntPtr pDynLibs, ref QY_ENV pEnv)
        {
            pEnv.usLangId = Consts.CONST_langId_PRC;

            return 0;
        }

        //
        public static QY_DMITEM[] CONST_fieldIdTable_en = new QY_DMITEM[19]
{   new QY_DMITEM(Consts.CONST_fieldId_yhlx,                 (  "用户类型"  )             ),
     new QY_DMITEM(Consts.CONST_fieldId_syr,                  (  "Username"  )             ),
     new QY_DMITEM(Consts.CONST_fieldId_bm,                   (  "Department"  )                   ),
     new QY_DMITEM(Consts.CONST_fieldId_dw,                   (  "Company"  )                  ),
     new QY_DMITEM(Consts.CONST_fieldId_bb,                   (  "Local branch"  )                 ),
     new QY_DMITEM(Consts.CONST_fieldId_zb,                   (  "Group"  )                    ),
     new QY_DMITEM(Consts.CONST_fieldId_qtdw,                 (  "Other"  )                ),
	//
	 new QY_DMITEM(Consts.CONST_fieldId_zcbh0,                (  "Asset no"  )             ),
     new QY_DMITEM(Consts.CONST_fieldId_zcbh1,                (  "Asset no1"  )            ),
     new QY_DMITEM(Consts.CONST_fieldId_zcbh2,                (  "Asset no2"  )            ),
	//  #define		CONST_fieldId_zcsm0								123
	 new QY_DMITEM(Consts.CONST_fieldId_zcsm1,                (  "Asset note1"  )          ),
     new QY_DMITEM(Consts.CONST_fieldId_zcsm2,                (  "Asset note2"  )          ),
     new QY_DMITEM(Consts.CONST_fieldId_jqlx,                 (  "Type"  )             ),
     new QY_DMITEM(Consts.CONST_fieldId_lxdh,                 (  "Telephone"  )                ),
     new QY_DMITEM(Consts.CONST_fieldId_sydd,                 (  "Address"  )              ),
     new QY_DMITEM(Consts.CONST_fieldId_zyyt,                 (  "Use for"  )              ),
     new QY_DMITEM(Consts.CONST_fieldId_bz1,                  (  "Remarks1"  )             ),
     new QY_DMITEM(Consts.CONST_fieldId_bz2,                  (  "Remarks2"  )             ),
	//
	  new QY_DMITEM( -1,                                 ""                   ),

};

        //
        public static unsafe int makeQwmAuthInfo(byte* ver, QWM_MACS_INFO* pMacsInfo, QNM_COMM_AUTHINFO* pAuthInfo)
        {
            int i = 0;

            if (null == pAuthInfo) return -1;

            if (null != ver) mymemcpy((IntPtr)pAuthInfo->ver, (IntPtr)ver, Consts.CONST_qyMaxVerLen);
            if (null != pMacsInfo)
            {
#if false
				for (i = 0; i < min(pMacsInfo->nMacs, CONST_qnmMaxMacs); i++)
				{   //  2006/06/02, ¶ÔnMacs¼ÓÁËºÍCONST_qnmMaxMacsµÄ±È½Ï
					memcpy(pAuthInfo->macs[i], pMacsInfo->macs[i], CONST_qyMacLen);
				}
#endif
            }
            return 0;
        }



        public static unsafe int qnmOpenSession(string servIp, ushort usPort, QWM_MACS_INFO* pMacsInfo, byte ucCmd, int iServiceId, byte* ver, void* pCommEncCtx, ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, ref SOCK_PARAM pSockParam, byte* data, uint* piDataSize)
        {
            int iErr = -1;
            //char ip[16];
            TMP_ip ip;
            QNM_COMM_AUTHINFO authInfo;
            QY_COMM_RESP tmpResp = new QY_COMM_RESP();
            //char tmpBuf[4096];
            TMP_buf4096 tmpBuf;
            int sizeof_tmpBuf = Consts.bufSize_4096;
            string str;

            //
            if (0 == ucCmd)
            {
                if (null == pMacsInfo || 0 == pMacsInfo->nMacs) return -1;
            }

            if (!GuiShare.pf_bQySockOpen(ref pSock))
            {       //  Èç¹ûÇ°ÃæÒÑ½¨Á¢Á¬½Ó£¬Ôò²»ÐèÒª½¨Á¢ÐÂÁ¬½Ó
                if (0 != GuiShare.pf_qyConnect(servIp, usPort, ref pSockParam, ref pSock))
                {

                    if (Consts.CONST_Status_Window_Function_Open == 0)
                    {
                        str = string.Format("qnmOpenSession: qyConnect {0} failed", servIp);
                        traceLog(str);

                    }

                    //
                    goto errLabel;
                }
            }
            if (0 != getLocalIpBySocket(ref pSock, ip.ip)) goto errLabel;
            mymemset((IntPtr)(byte*)&authInfo, 0, sizeof(QNM_COMM_AUTHINFO));
            if (0 != makeQwmAuthInfo(ver, pMacsInfo, &authInfo)) goto errLabel;
            mymemcpy((IntPtr)authInfo.ip, (IntPtr)ip.ip, Consts.CONST_qyMaxIpLen);
            authInfo.cmd = ucCmd;


            //
            if (0 != qyTellService((uint)iServiceId, ver, pCommEncCtx, (byte*)&authInfo, sizeof(QNM_COMM_AUTHINFO), ref pSock, ref pSockParam.to, ref pSession)) goto errLabel;
            if (0 != qyRecvResp(ref pSession, ref pSock, ref pSockParam.to, &tmpResp, tmpBuf.buf, (uint)sizeof_tmpBuf)) goto errLabel;
            //  traceLogA(  "qwmOpenSession: get respCode of qyTellService is %s",  qyGetDesByType1(  CONST_qyRcTable,  tmpResp.head.usCode  )  );
            //  ÕâÀï²»ÄÜÓÃÀ´ÅÐ¶Ï·µ»ØÂë£¬ÒòÎªÊÇÍ¨ÓÃµÄ½Ó¿Ú£¬Ó¦ÔÚÆäËûµØ·½ÅÐ¶Ï£¬2005/06/25
            //  if  (  !isRcOk(  tmpResp.head.usCode  )  )  goto  errLabel; 

            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
                if (GuiShare.pf_bQySockOpen(ref pSock)) GuiShare.pf_qyDisconnect(ref pSock);
            }
            else
            {
                if (null != piDataSize && null != data)
                {
                    *piDataSize = Math.Min(*piDataSize, tmpResp.head.len);
                    mymemcpy((IntPtr)data, (IntPtr)tmpBuf.buf, *piDataSize);
                }
            }

#if DEBUG
            if (Consts.CONST_Status_Window_Function_Open == 0)
            {
                str = string.Format("qnmOpenSession leaves iErr {0}", iErr);
                qyFuncs.traceLog(str);

            }

#endif

            return iErr;
        }



        //
        public static unsafe void qnmCloseSession(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, ref SOCK_TIMEOUT pTo)
        {
            QY_COMM_RESP tmpResp;

            if (GuiShare.pf_bQySockOpen(ref pSock))
            {
                if (isRcOk(pSession.usLastRespCode_i) || pSession.usLastRespCode_i == Consts.CONST_qyRc_user)
                {
                    if (0 == qySendReq(ref pSession, ref pSock, ref pTo, Consts.CONST_qyCmd_end, null, 0))
                    {
                        qyRecvResp(ref pSession, ref pSock, ref pTo, &tmpResp, null, 0);
                    }
                }
                //
                GuiShare.pf_qyDisconnect(ref pSock);
            }
            return;
        }

        //
        public static unsafe int qyRecvResp(ref QY_COMM_SESSION pSession, ref QY_SOCK pSock, ref SOCK_TIMEOUT pTo, QY_COMM_RESP* pResp, byte* dataBuf, uint bufSize)
        {
            int iErr = -1;
            byte* encBuf = null;
            uint encLen = Consts.CONST_maxReqDataLen_enc_netMc;

            if (0 != qyRecvEx(ref pSock, (byte*)pResp, sizeof(QY_COMM_HEAD), pTo.iRead)) goto errLabel;
            pResp->head.len = qyntohi(pResp->head.len);

#if DEBUG
            ref QY_COMM_RESP tmp_pResp = ref *pResp;
            if (pResp->head.len != 0)
            {
                int ii = 0;
            }
#endif
            //
            if (null == (encBuf = (byte*)mymalloc((int)encLen))) goto errLabel;

            if (pResp->head.len > encLen) goto errLabel;
            if (0 != pResp->head.len)
            {
                if (0 != qyRecvEx(ref pSock, encBuf, (int)pResp->head.len, pTo.iRead)) goto errLabel;

                if (pSession.service.encType == Consts.CONST_qyEncType_qwm)
                {
#if false
					if (qnmDecData(&pSession->commEncCtx, encBuf, pResp->head.len, dataBuf, &bufSize))
					{
						traceLogA("qyRecvReq里qnmDecData failed"); goto errLabel;
					}
					pResp->head.len = bufSize;
#endif
                }
                else if (pResp->head.len > bufSize) { iErr = -2; goto errLabel; }
                else mymemcpy((IntPtr)dataBuf, (IntPtr)encBuf, pResp->head.len);
            }


            pResp->head.usCode = qyntohs(pResp->head.usCode);
            pSession.usLastRespCode_i = pResp->head.usCode;

            iErr = 0;

        errLabel:

            if (null != encBuf) myfree((IntPtr)encBuf);

            return iErr;

        }

        //
        public delegate bool PF_bServConnected(ref QY_ENV pEnv, string serverIp, ushort serverPort, object pParam, int serviceId, string ver, ref QY_ENC_CTX pCommEncCtx, ref SOCK_PARAM pSockParam);


        //
        public static unsafe int getCorrectCnt(ref QY_ENV pEnv, ref QWM_DYN_CFG pDynCfg, PF_bServConnected pf_bServConnected, object pParam, int serviceId, string ver, QY_ENC_CTX pCommEncCtx, ref SOCK_PARAM pSockParam, ref QY_LOGICAL_SERVER pCnt)
        {
            int iErr = -1;
            //char ip[CONST_qyMaxIpLen + 1];
            TMP_ip ip;
            //char* pAddrs[mycountof(pDynCfg->cnt.cntAddrs) + 1];
            string[] pAddrs = new string[3];
            int i;

            if (//!pDynCfg || !pCnt || 
                null == pf_bServConnected) goto errLabel;

#if false
			memset(pAddrs, 0, sizeof(pAddrs));
			pAddrs[0] = pDynCfg->cnt.cntIp;
			pAddrs[1] = pDynCfg->cnt.cntAddrs[0];
			pAddrs[2] = pDynCfg->cnt.cntAddrs[1];
			MACRO_qyAssert(3 <= mycountof(pAddrs), _T("overflow"));
#endif
            fixed (byte* p = pDynCfg.cnt.cntIp)
            {
                pAddrs[0] = mytoString(p);
            }
            fixed (byte* p = pDynCfg.cnt.cntAddr1)
            {
                pAddrs[1] = mytoString(p);
            }
            fixed (byte* p = pDynCfg.cnt.cntAddr2)
            {
                pAddrs[2] = mytoString(p);
            }
            //
            for (i = 0; i < (pAddrs.Length); i++)
            {
                if (0 != getIpFromName(pAddrs[i], ip.ip, Consts.CONST_qyMaxIpLen + 1)) continue;

#if false //  2013/04/06. ip的末尾为0或者255时,不一定表明是广播地址,要看掩码.所以这里不能过滤掉
				if  (  !bIpValid(  ip  )  )  continue;
#endif

                //  2007/02/21, ÔÚÉ½¶«Ì©±¦£¬·¢ÏÖÈç¹û¶¯Ì¬ÓòÃûÓÐÊ±»á½âÎö³É127.0.0.1, Îª·ÀÖ¹ÕâÖÖÏÖÏó¡£Òª¶ÔIP½øÐÐ¹ýÂË¡£µ«ÊÇÈç¹ûpAddrs[i]Îª127.0.0.1Ôò²»¹ýÂË
#if false
				if (!_stricmp(ip, CONST_ip_localhost))
				{
					if (_stricmp(pAddrs[i], CONST_ip_localhost)) continue;
				}
#endif
                if (pf_bServConnected(ref pEnv, mytoString(ip.ip), pDynCfg.cnt.usCntPort, pParam, serviceId, ver, ref pCommEncCtx, ref pSockParam)) break;
            }

            if (i == (pAddrs.Length)) goto errLabel;

            //mymemset((IntPtr)pCnt, 0, sizeof(QY_LOGICAL_SERVER));
            pCnt = new QY_LOGICAL_SERVER();
            fixed (byte* p = pCnt.ip)
            {
                safeStrnCpy(ip.ip, p, Consts.CONST_qyMaxIpLen + 1);
            }
            pCnt.port = pDynCfg.cnt.usCntPort;

            iErr = 0;

        errLabel:

            if (iErr != 0)
            {
            }

            return iErr;

        }



    }
}
