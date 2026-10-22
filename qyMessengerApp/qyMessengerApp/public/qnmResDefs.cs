using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    public enum enumRes               // Declare enum type Days
    {
        //enum enumResIds {
        CONST_resId_null = 80000,   //  ÕâÀïÈ¡ÖµÊÇÎªÁË±ÜÃâ·ÖÅäÖµµ½Î¢ÈíµÄSTUDIO²úÉúµÄIDÖµÓòÀï
                                    //
        CONST_resId_seqNo,              //  ÐòºÅ
                                        //
        CONST_resId_logon_wndTitle,
        CONST_resId_logon_staticUsrName,
        CONST_resId_logon_staticPasswd,
        CONST_resId_cancel_AV,
        //
        CONST_resId_gpBhbLogQueryStruct,
        CONST_resId_gpBhbOfflineLogQueryStruct,
        CONST_resId_gpScannedIpListQueryStruct,
        //
        CONST_resId_gpSimpleUsrQueryStruct,
        CONST_resId_gpPcListQueryStruct,
        CONST_resId_gpPcListQueryStruct_netMc2,
        CONST_resId_gpPcAssetListQueryStruct,
        CONST_resId_gpPcAssetReportListQueryStruct,
        CONST_resId_gpPcAssetReportListQueryStruct_en,
        CONST_resId_gpPcSpListQueryStruct,
        CONST_resId_gpPcSpSpecialListQueryStruct,
        //  CONST_resId_gpUnchkedPcListQueryStruct,			
        //  CONST_resId_gpWillAttackPcListQueryStruct,		
        //
        CONST_resId_gpPcUsrListQueryStruct,
        CONST_resId_gpPcUsrInTab1ListQueryStruct,
        CONST_resId_gpClientListQueryStruct_netMc,
        //
        CONST_resId_gpPcSecChkEventAllQueryStruct,
        //
        CONST_resId_gpPcRuleListQueryStruct0,
        CONST_resId_gpPcRuleListQueryStruct1,
        CONST_resId_gpPcAdapterListQueryStruct,
        CONST_resId_gpPcProxyCfgListQueryStruct,
        //  CONST_resId_gpPcComputerPropListQueryStruct,	
        //  CONST_resId_gpPcComputerRuleListQueryStruct,	
        CONST_resId_gpPcWindowsUpdatePolicyListQueryStruct,
        CONST_resId_gpPcPrinterPropListQueryStruct,
        //
        CONST_resId_gpPcRegistryListQueryStruct,
        CONST_resId_gpPcRegistryRuleStatusListQueryStruct,
        CONST_resId_gpPcEventLogListQueryStruct,
        CONST_resId_gpPcOsUsrListQueryStruct,
        CONST_resId_gpPcNetStatListQueryStruct,
        CONST_resId_gpPcProcessPropListQueryStruct,
        CONST_resId_gpPcProcessModulePropListQueryStruct,
        CONST_resId_gpPcProcessRuleListQueryStruct,
        CONST_resId_gpPcProcessKnowledgeListQueryStruct,
        CONST_resId_gpPcRegistryKnowledgeListQueryStruct,
        CONST_resId_gpPcNetFlowListQueryStruct,
        CONST_resId_gpPcNetShareListQueryStruct,
        //  CONST_resId_gpPcAutoLogonListQueryStruct,
        //
        CONST_resId_gpSnmpObjRuleListQueryStruct,
        CONST_resId_gpConflictConnListQueryStruct,
        CONST_resId_gpDirectConnListQueryStruct,
        CONST_resId_gpNetDevConnListQueryStruct,
        CONST_resId_gpSearchListQueryStruct,
        CONST_resId_gpOpEventListQueryStruct_netMc,
        CONST_resId_gpPcEventAllQueryStruct,
        CONST_resId_gpNetDevEventAllQueryStruct,
        CONST_resId_gpPcRasEventQueryStruct,
        //	CONST_resId_gpSecChkEventAllQueryStruct,		

        CONST_resId_mis_base = 90000,                       //  2007/04/07

        //
        CONST_resId_is_qmStr_start = 91000,             //  2011/08/14. //  这个区间专门定义一些daemon和gui共用的resId. 定义和qmStr.h要保持一致
        CONST_resId_is_qmStr_end = 95000,

        //
        CONST_resId_servName,                                   //  2007/08/28
        CONST_resId_idStr,                                      //  2007/08/10
        CONST_resId_idStr_grp,
        //
        CONST_resId_gpSimpleUsrQueryStruct_is,                  //  2007/09/15
        CONST_resId_gpMessengerAuthListQueryStruct,
        CONST_resId_gpMessengerAuthNeedAuditedListQueryStruct,  //  2007/09/12
        CONST_resId_gpImAmListQueryStruct,                      //  2010/01/05
        CONST_resId_gpPhoneMessengerListQueryStruct,            //  2007/09/12
        CONST_resId_gpImGrpAuthListQueryStruct_isMgr,
        CONST_resId_gpImGrpAuthListQueryStruct_isClient,
        CONST_resId_gpImGrpListQueryStruct_isMgr,
        CONST_resId_gpImGrpListQueryStruct_isClient,
        CONST_resId_gpImGrpAuthMemListQueryStruct_isMgr,
        CONST_resId_gpImGrpMemListQueryStruct_isMgr,
        CONST_resId_gpImGrpMemListQueryStruct_isClient,
        CONST_resId_gpImObjListQueryStruct_isMgr,               //  2007/07/29
        CONST_resId_gpImObjListQueryStruct_isClient,            //  2007/07/29
        CONST_resId_gpImObjUsrListQueryStruct_isMgr,            //  2007/07/30
        CONST_resId_gpImObjUsrInTab1ListQueryStruct_isMgr,
        CONST_resId_gpImObjUsrListQueryStruct_isClient,         //  2007/07/30
        CONST_resId_gpImObjUsrInTab1ListQueryStruct_isClient,
        CONST_resId_gpClientListQueryStruct_is,                 //  2007/09/16	
        CONST_resId_gpCustomerServiceListQueryStruct_is,        //  2011/04/03
        CONST_resId_gpWebContactListQueryStruct_is,             //  2007/11/20
        CONST_resId_gpIpKnowledgeListQueryStruct_is,            //  2007/12/23
        CONST_resId_gpPhoneGuestListQueryStruct_is,             //  2007/12/23
        CONST_resId_gpGsmModemListQueryStruct_isMgr,            //  2008/01/27
        CONST_resId_gpSmContactListQueryStruct_isMgr,           //  2008/01/27
        CONST_resId_gpSharedDynBmpListQueryStruct_isMgr,        //  2009/08/31
        CONST_resId_gpSharedDynBmpRuleListQueryStruct_isMgr,    //  2009/09/17
        CONST_resId_gpImObjRuleListQueryStruct_isMgr,           //  2010/08/16
        CONST_resId_gpImObjRuleListQueryStruct_isClient,        //  2010/09/06


        //
        CONST_resId_gpImTaskListQueryStruct,
        CONST_resId_gpImMsgListQueryStruct_isMgr,
        CONST_resId_gpImMsgListQueryStruct_isClient,


        //
        CONST_resId_gpEventListQueryStruct_is,
        CONST_resId_gpOpEventListQueryStruct_is,


        //  ´°¿ÚÏÔÊ¾µÄÒ»Ð©¶ÔÏó  
        //
        CONST_resId_status,                 //  ×´Ì¬£¬2008/01/23
        CONST_resId_ipAddr,                     //  2016/07/30
        CONST_resId_deviceAddr,
        CONST_resId_ptzAddr,
        CONST_resId_rtspAddr,
        //
        CONST_resId_pcMgrClass,
        CONST_resId_pcUsrInfo,
        CONST_resId_pcOpNode,
        CONST_resId_pcOsClass,
        //
        CONST_resId_pcOpNode_desc,

        //  2008/12/14
        CONST_resId_objIdStr_unRegGrp,      //			_T(  "(Î´×¢²áÓÃ»§)"  )
        CONST_resId_objIdStr_othersGrp,     //			_T(  "(ÆäËûÓÃ»§)"  )
        CONST_resId_objIdStr_me,            //			_T(  "ÎÒ"  )
        CONST_resId_objIdStr_imGrps,        //			_T(  "×é¼¯ºÏ"  )
        CONST_resId_objIdStr_imGrps_av,     //			2014/01/31
        CONST_resId_objIdStr_imGrps_msg,    //			2014/01/31
        CONST_resId_objIdStr_imGrps_tmp,    //			2017/07/28




        //  Í³¼Æ
        CONST_resId_statIpScan,
        CONST_resId_statPcInfo,
        CONST_resId_statUsrInfo,
        CONST_resId_statGeneral,
        CONST_resId_statOthers,
        //
        CONST_resId_statMessengerInfo,  //  2007/09/22

        //  
        CONST_resId_COLUMNINFO_sQyMcRootListColumns,
        CONST_resId_COLUMNINFO_networkViewTable,
        CONST_resId_COLUMNINFO_switchViewTable,
        CONST_resId_COLUMNINFO_switchPortViewTable,
        CONST_resId_COLUMNINFO_switchUsrConnectionViewTable,
        CONST_resId_COLUMNINFO_switchPortUsrConnectionViewTable,
        CONST_resId_COLUMNINFO_switchNetworkSpeedViewTable,
        CONST_resId_COLUMNINFO_switchPortNetworkSpeedViewTable,
        //
        CONST_resId_COLUMNINFO_col4ListColumns_netMc,
        CONST_resId_COLUMNINFO_col4ListColumns_is,
        //
        CONST_resId_COLUMNINFO_unRegGrpMemListColumns_netMc,
        CONST_resId_COLUMNINFO_unRegGrpMemListColumns_is,
        CONST_resId_COLUMNINFO_othersGrpMemListColumns,
        CONST_resId_COLUMNINFO_pcMgrClassListColumns,
        CONST_resId_COLUMNINFO_pcOpNodeMemListColumns_netMc,
        CONST_resId_COLUMNINFO_pcOpNodeMemListColumns_is,
        CONST_resId_COLUMNINFO_pcEnvListColumns,
        CONST_resId_COLUMNINFO_pcOsListColumns,
        CONST_resId_COLUMNINFO_pcAssetListColumns,
        //
        CONST_resId_COLUMNINFO_qnmCfgIpScanColumns,
        CONST_resId_COLUMNINFO_qnmCfgPcGrpsColumns,
        CONST_resId_COLUMNINFO_pcRegistryColumns,
        //
        CONST_resId_COLUMNINFO_imGrpListColumns,
        CONST_resId_COLUMNINFO_imGrpMemListColumns_isMgr,
        CONST_resId_COLUMNINFO_imGrpMemListColumns_isCli,

        //  2008/12/16
        CONST_resId_COLUMNINFO_qyCfgSystemsColumns,
        CONST_resId_COLUMNINFO_procedObjsViewTable,
        //
        CONST_resId_COLUMNINFO_qnmCfgPcGrpsColumns_nms,
        CONST_resId_qnmCfgSwitchPolicyGrpsColumns,
        CONST_resId_qnmMgrSwitchPolicyColumns,
        CONST_resId_cfgStandardNameColumns,
        CONST_resId_realTimeImTaskListColumns,                      //  2008/12/29
        CONST_resId_taskInfoListColumns,                            //  2014/09/25
        CONST_resId_sharingObjectListColumns,                       //  2009/09/13
        CONST_resId_playerListColumns,                              //  2009/02/26		
        CONST_resId_wallListColumns,                                //  2009/10/04
        CONST_resId_ipCamListColumns,                               //  2016/07/30
        CONST_resId_imNetStatListColumns,                           //  2009/10/11
        CONST_resId_talkerListColumns,                              //  2009/10/18
        CONST_resId_messengerStatusColumns,                         //  2010/08/17
        CONST_resId_qListColumns,                                   //  2009/12/18
        CONST_resId_imTaskListColumns,                              //  2013/02/17
        CONST_resId_imObjListColumns,                               //	2014/04/17
        CONST_resId_regInfoListColumns,                             //  2014/04/18
        CONST_resId_imGrpListColumns,                               //	2014/04/17
        CONST_resId_imGrpMemListColumns,                                //	2014/04/17
        CONST_resId_imObjRuleListColumns,                               //	2014/04/17
        CONST_resId_dynBmpListColumns,                                  //  2014/04/18

        CONST_resId_taskListColumns,                                //  2008/12/31
                                                                    //  CONST_resId_taskListColumns_showMembers,					//  2008/12/31
        CONST_resId_instantAssistant_talkerListColumns,             //  2009/02/23
        CONST_resId_deamonStatus_listColumns,                       //  2011/01/16
        CONST_resId_deamonStatus_listColumnsOfChannel,              //  2011/01/28

        CONST_resId_autoScan,                                       //  2012/08/14
        CONST_resId_rainBrush,                                      //  2012/08/14
        CONST_resId_searchLight,                                    //  2012/08/14

        CONST_resId_ok,                                             //  2013/09/08
        CONST_resId_cancel,                                         //  2013/09/08

        CONST_resId_versionErr,                                     //  2014/11/18

        //
        CONST_resId_sharingNotSupportFullScreen,                    //  2014/11/18
        CONST_resId_sharingLetWinBeSmaller,                                 //  2014/11/18
                                                                            //
        CONST_resId_gotoMeeting,
        CONST_resId_wouldYouLikeToMeeting,
        CONST_resId_speakingOrNotSpeaking,
        CONST_resId_useMeetingFunction,

        CONST_resId_preTotal,
        //  
        CONST_resId_total = CONST_resId_preTotal + 1000,
        // }


        //
        //public enum enumResTemp 	{
        CONST_resId_temp_start = CONST_resId_total,
        CONST_resId_y,
        CONST_resId_n,
        //
        CONST_resId_menu_wenJian,
        CONST_resId_menu_guanLi,
        //  CONST_resId_menu_scannedIpList,		
        CONST_resId_menu_jiSuanJiGuanLi,

        //  ID_qyPcList,	这些从resource.h里取出来的ID，不能在enum里定义，为清晰起见，在这里放着，但是需要注释掉
        //  ID_qyPcSpList,	
        CONST_resId_menu_yongHuGuanLi,
        CONST_resId_menu_keHuDuanGuanLi,
        CONST_resId_menu_jiSuanJiAnQuanGuanLi,
        CONST_resId_menu_yuanChengZhuoMianGuanLi,
        CONST_resId_menu_jianKongYuShenJi,
        CONST_resId_menu_shiJianChaKan,
        CONST_resId_menu_ziChanGuanLi,
        CONST_resId_menu_baoBiao,
        CONST_resId_menu_yiJianZiChanZhongWenBaoBiao,
        CONST_resId_menu_anJiSuanJi,
        CONST_resId_menu_anCaoZuoXiTong,
        CONST_resId_menu_yiJianZiChanYingWenBaoBiao,
        CONST_resId_menu_anJiSuanJi1,
        CONST_resId_menu_anCaoZuoXiTong1,
        CONST_resId_menu_wangLuoGuanLi,
        CONST_resId_menu_chaXun,
        CONST_resId_menu_lianXiRenGuanLi,               //  2007/03/07
        CONST_resId_menu_fenZuGuanLi,                   //  2007/09/01
                                                        //
        CONST_resId_menu_phoneGuest,                    //  
        CONST_resId_menu_customerService,               //  2011/04/03
        CONST_resId_menu_wwwQm,                         //  2007/11/20
        CONST_resId_menu_sharedResources,               //  2009/09/03
                                                        //
        CONST_resId_menu_modulesGuanLi_isMgr,           //  2007/12/30
        CONST_resId_menu_modulesGuanLi_isClient,        //  2007/12/30
                                                        //
        CONST_resId_menu_setLocalSharing,               //  2009/10/09
                                                        //
        CONST_resId_menu_subtitles,                     //  2013/03/09
                                                        //
        CONST_resId_menu_renWuChaKan,                   //  2007/08/10
        CONST_resId_menu_xiaoXiChaKan,                  //  2007/04/07
        CONST_resId_menu_xiTong = CONST_resId_total + 1000,
        //
        CONST_resId_menu_peiZhi = CONST_resId_total + 2000,
        //
        CONST_resId_skippedMacsHint,
        //
        CONST_resId_menu_chaKan = CONST_resId_total + 3000,
        //
        CONST_resId_menu_chuangKou = CONST_resId_total + 4000,
        //
        CONST_resId_menu_bangZhu = CONST_resId_total + 5000,
        //
        CONST_resId_menu_popup = CONST_resId_total + 10000,
        CONST_resId_anQuanGuanLi,
        CONST_resId_yuanChengZhuoMianGuanLi,
        CONST_resId_jianKongYuShenJi,
        CONST_resId_yongHuXinXiBianJiShenHe,
        CONST_resId_monIe,
        CONST_resId_pcSecChkEventList_netMc2,

        //
        CONST_resId_menu_avCall,                    //  2007/10/04
        CONST_resId_menu_share,                     //  2008/10/04
        CONST_resId_menu_remoteAssist,
        //
        CONST_resId_menu_op,                        //  2008/11/06
        CONST_resId_menu_op_ptz,                        //  2012/08/01

        CONST_resId_3dWallMustBeOpen,               //  2017/09/19
                                                    //
        CONST_resId_menu_vWall,                     //  2009/09/19
        CONST_resId_menu_3dWall,                    //  2013/12/25
        CONST_resId_menu_2dWall,                    //  2013/12/25
        CONST_resId_statusVWallEditRule,            //  2009/10/09
        CONST_resId_statusVWallRestart,             //	2009/10/09
        CONST_resId_hintToStopScreensharing,        //  2014/12/11
                                                    //
        CONST_resId_menu_decStatus,                 //  2015/06/18
        CONST_resId_menu_playStatus,                //  2015/09/21
                                                    //  
        CONST_resId_menu_runningStatus,             //  2013/02/06
                                                    //
        CONST_resId_menu_onvif,                     //  2016/07/31

        //
        CONST_resId_views = CONST_resId_total + 13000,
        CONST_resId_zuiJinJianCeShiJian,

        CONST_resId_dlgStatic = CONST_resId_total + 15000,
        //  对话框都加在下面
        //
        CONST_resId_STATIC_nDwLevels,
        CONST_resId_STATIC_nZcbhs,
        CONST_resId_STATIC_qrtTitle,
        CONST_resId_STATIC_hint,
        CONST_resId_STATIC_clientWelcomeMsg,        //  2016/08/10
                                                    //
                                                    //  CONST_resId_STATIC_getCfgInterval,
                                                    //  CONST_resId_STATIC_showPcNetworkPropInterval,
                                                    //  CONST_resId_STATIC_sndEventLogInterval,
                                                    //  CONST_resId_STATIC_sndUsrInfoInterval,
                                                    //  CONST_resId_STATIC_sndNetStatInterval,
                                                    //  CONST_resId_STATIC_showProcessInterval,
                                                    //  CONST_resId_STATIC_maxLogs,
                                                    //  
        CONST_resId_STATIC_type,
        CONST_resId_STATIC_name,
        CONST_resId_STATIC_cusName,
        CONST_resId_STATIC_des0,
        CONST_resId_STATIC_langId,
        CONST_resId_STATIC_supportedOs,
        CONST_resId_STATIC_url,
        //
        CONST_resId_IDD_passwdForStopMon,
        CONST_resId_STATIC_passwdForStopMon,
        //
        CONST_resId_STATIC_phoneNum,
        //
        CONST_resId_IDD_qnmCfgPcProxyCfg,
        CONST_resId_STATIC_staticDw,
        CONST_resId_STATIC_staticBm,
        CONST_resId_STATIC_staticSyr,
        CONST_resId_STATIC_staticIp,
        CONST_resId_STATIC_staticType,
        CONST_resId_STATIC_dialSetting,
        CONST_resId_RADIO_ucbEnableAutodial,
        CONST_resId_RADIO_ucbNoNetAutodial,
        CONST_resId_RADIO_defaultDial,
        CONST_resId_STATIC_lanSetting,
        CONST_resId_CHECK_autoDetect,
        CONST_resId_CHECK_enableProxy,
        CONST_resId_STATIC_staticIpAddress,
        CONST_resId_STATIC_staticPort,
        CONST_resId_CHECK_proxyOverride,
        //
        CONST_resId_dbNameOrDsnName,
        //
        CONST_resId_STATIC_centerIp,
        CONST_resId_STATIC_qyRegToolDir,
        //
        CONST_resId_fileMon_bMonAll,
        CONST_resId_fileMon_monOnly,
        CONST_resId_fileMon_suffix,
        CONST_resId_fileMon_str,
        CONST_resId_hint_addFileMonRule,
        //
        CONST_resId_IDD_qnmCfgRasOptions,
        CONST_resId_STATIC_floppyCmd,
        CONST_resId_STATIC_cdromCmd,
        CONST_resId_STATIC_usbCmd,
        CONST_resId_STATIC_rasCmd,
        //  CONST_resId_STATIC_warnToInternetCmd,
        CONST_resId_STATIC_sndEventLogCmd,
        CONST_resId_STATIC_sndUsrInfoCmd,
        CONST_resId_STATIC_sndPrinterCmd,
        CONST_resId_STATIC_sndNetStatCmd,
        CONST_resId_STATIC_auditDir,
        CONST_resId_STATIC_stopMon,
        CONST_resId_CHECK_unchk,
        CONST_resId_STATIC_specialPhoneNum,
        CONST_resId_add,
        CONST_resId_del,
        CONST_resId_CHECK_bNotChkOnInternet,
        CONST_resId_STATIC_monIp,
        CONST_resId_STATIC_monPort,
        CONST_resId_IDD_qnmCfgPcNetworkOptions,
        CONST_resId_STATIC_bindPcNetworkOptions,
        CONST_resId_BUTTON_bindPcNetworkOptions,
        CONST_resId_STATIC_cancelPcNetworkOptionsBindings,
        CONST_resId_BUTTON_cancelPcNetworkOptionsBindings,
        CONST_resId_IDD_qnmCfgPcRule,
        CONST_resId_IDD_qnmEditPcUsrInfo,
        CONST_resId_hint0,
        CONST_resId_hint1,
        CONST_resId_qyProperties1,
        CONST_resId_qyEdit1,
        ID_qyMgrPcGrp1,
        //  2005/12/10
        ID_qyEventAll1,
        ID_qyStopRegScan1,
        ID_qyStartRegScan1,
        //  2005/11/09
        CONST_resId_IDD_ABOUTBOX,
        CONST_resId_ver,
        CONST_resId_qycxCopyRight,
        CONST_resId_IDD_CFGUSR,
        CONST_resId_name,
        CONST_resId_IDD_CFGUSRS,
        CONST_resId_HINT,
        CONST_resId_ADD,
        CONST_resId_MODIFY,
        CONST_resId_DEL,
        CONST_resId_IDD_CFGWARN,
        CONST_resId_WARNON,
        CONST_resId_STATIC_cfgWarn,
        CONST_resId_WARNUNTILUSRSTOPIT,
        CONST_resId_STATIC_maxWarnElapse,
        CONST_resId_IDD_clearupEventCond,
        CONST_resId_IDD_cusPcSp,
        CONST_resId_STATIC_buDing,
        CONST_resId_STATIC_bieMing,
        CONST_resId_IDD_qnmCfgIpScan,
        CONST_resId_IDD_qnmCfgPcComputer,
        CONST_resId_STATIC_leiXing,
        CONST_resId_STATIC_jiSuanJiMing,
        CONST_resId_STATIC_zhuYi,
        CONST_resId_CHECK_ucbAutoRestartIfChanged,
        //
        CONST_resId_importWSusTmpl,
        CONST_resId_IDD_qnmCfgPcGrp,
        CONST_resId_STATIC_qiShiIP,
        CONST_resId_STATIC_jieShuIP,
        CONST_resId_STATIC_danWei,
        CONST_resId_STATIC_buMen,
        CONST_resId_STATIC_scanedObj,
        //
        CONST_resId_STATIC_shiYongRen,
        //
        CONST_resId_IDD_shhgPcProperties,
        CONST_resId_pc0_zongBu,
        CONST_resId_pc2_benBu,
        CONST_resId_pc4_danWei,
        CONST_resId_pc6_qiTaDanWei,
        CONST_resId_pc8_buMen,
        CONST_resId_pc10_shiYongRen,
        CONST_resId_pc12_jiQiLeiXing,
        CONST_resId_pc14_ziChanBianHao,
        CONST_resId_pc16_zhuYaoYongTu,
        CONST_resId_pc18_lianXiDianHua,
        CONST_resId_pc20_shiYongDiDian,
        CONST_resId_pc22_shiYongDiDian,
        CONST_resId_pc24_ziChanBianHao1,
        CONST_resId_pc26_ziChanShuoMing1,
        CONST_resId_pc28_ziChanBianHao2,
        CONST_resId_pc30_ziChanShuoMing2,
        //
        CONST_resId_STATIC_usrInfoZone,
        CONST_resId_STATIC_pcName,
        CONST_resId_STATIC_ip,
        CONST_resId_STATIC_osName,
        CONST_resId_STATIC_servicePack,
        CONST_resId_STATIC_mac0,
        CONST_resId_STATIC_mac1,
        CONST_resId_STATIC_mac2,
        CONST_resId_STATIC_ip0,
        CONST_resId_STATIC_ip1,
        CONST_resId_STATIC_ip2,
        //
        CONST_resId_STATIC_qwmVer,
        CONST_resId_STATIC_regTime,
        CONST_resId_STATIC_auditTime,
        CONST_resId_STATIC_clientStartTime,
        CONST_resId_STATIC_lastCommTime,
        //	CONST_resId_IDD_spProperties,
        /*	CONST_resId_STATIC_type,
			CONST_resId_STATIC_name,
			CONST_resId_STATIC_cusName,
			CONST_resId_STATIC_des0,
			CONST_resId_STATIC_langId,
			CONST_resId_STATIC_supportedOs,
			CONST_resId_STATIC_url,
		*/
        CONST_resId_IDD_qyExport,
        CONST_resId_RADIO_exportCurQuery,
        CONST_resId_RADIO_exportBakData,
        CONST_resId_RADIO_exportCfg,
        CONST_resId_STATIC_exportHint,
        CONST_resId_RADIO_exportBhbCfg,
        CONST_resId_RADIO_exportRegInfo,
        CONST_resId_RADIO_procDj,
        CONST_resId_IDD_qnmClearup,
        CONST_resId_STATIC_clearHint,
        CONST_resId_RADIO_clearupScannedIp,
        CONST_resId_RADIO_clearupEvent,
        CONST_resId_IDD_pcAssetProperties,
        CONST_resId_STATIC_staticMac0,
        CONST_resId_STATIC_staticAssetType,
        CONST_resId_STATIC_staticDes0,
        CONST_resId_STATIC_staticStartTime,
        CONST_resId_STATIC_staticEndTime,
        CONST_resId_IDD_qnmSearch,
        CONST_resId_STATIC_queryType,
        //
        CONST_resId_qnmCfgPcGrps_ADD,
        CONST_resId_qnmCfgPcGrps_MODIFY,
        CONST_resId_qnmCfgPcGrps_DEL,

        //
        CONST_resId_IDD_editSwitchInfo_only,    //		_T(  "仅编辑审核网络设备"  )
        CONST_resId_IDD_editSwitchInfo,         //		_T(  "编辑审核网络设备(及端口联接)"  )  
                                                //
        CONST_resId_stat,                       //    2017/09/18
        CONST_resId_play,
        CONST_resId_pause,
        //
        CONST_resId_iRootKey,
        CONST_resId_subKey,
        CONST_resId_cfgName,
        //
        CONST_resId_startIp,
        CONST_resId_endIp,
        //
        CONST_resId_misServName,
        CONST_resId_uiDevType,
        CONST_resId_wDevIdStr,
        //
        CONST_resId_absolutePcRegistry,
        CONST_resId_editPcRegistryKnowledge,
        CONST_resId_prop_pcRegistryKnowledge,
        CONST_resId_unAudited_pcRegistry,
        CONST_resId_audited_pcRegistry,

        CONST_resId_importFile,
        CONST_resId_hideList,
        CONST_resId_utf8,
        CONST_resId_conferencingSubtitles,
        CONST_resId_screensharingSubtitles,
        CONST_resId_webcamSharingSubtitles,
        CONST_resId_shadow,
        CONST_resId_note,

        //  others, (杂项)
        // 2008/12/15
        CONST_resId_network,                //  "网络"
        CONST_resId_baseSystem,             //  (基本系统)
        CONST_resId_qyMessengerView,        //_T(  "联系人树状图"  )
        CONST_resId_qyMessengerList,        //
        CONST_resId_qyMessengerAuthList,
        CONST_resId_qyImAmList,
        CONST_resId_qyPhoneMessengerList,
        CONST_resId_qyPcUsrList2,
        CONST_resId_qyMessengerAuthList_needAudited,
        CONST_resId_qyImGrpAuthList,
        CONST_resId_qyImGrpList,
        CONST_resId_qyImGrpMemList,
        CONST_resId_qyImObjRuleList,

        CONST_resId_customerServiceList,
        CONST_resId_wwwQmCfgList,
        CONST_resId_procIpKnowledge,
        CONST_resId_qyGsmModemList,
        CONST_resId_qySmContactList,
        CONST_resId_qyPhoneGuestList,
        CONST_resId_sharedDynBmpList,
        CONST_resId_sharedDynBmpRuleList,
        CONST_ID_qyPcList,          //暂时没用起来，因为不知道效果如何能看到
        CONST_resId_qyClientList,
        CONST_resId_loaded,
        CONST_resId_dataSourceList,
        CONST_resId_loadDataSourceFailed,
        CONST_resId_unloadDataSourceFailed,
        CONST_resId_selectDbFile,
        CONST_resId_cannotEmpty,
        CONST_resId_systemTypeErr,
        CONST_resId_systemNameExisted,
        CONST_resId_addSystemErr,
        CONST_resId_maximumNumberOfObject,
        CONST_resId_systemCfgErr,
        CONST_resId_databaseformat,
        CONST_resId_cannotModifyIfLoaded,
        CONST_resId_cannotDeleteIfLoaded,
        CONST_resId_askIfDelete,
        CONST_resId_qyPcList,
        CONST_resId_qyPcView,
        //
        CONST_resId_qyAssetsView,           // 2009/08/09
                                            //
        CONST_resId_qyRealTimeTaskList,
        CONST_resId_taskInfoList,           //  2014/09/25
        CONST_resId_sharingObjectList,
        CONST_resId_playerList,
        CONST_resId_wallList,
        CONST_resId_ipCamList,              //  2016/07/30
        CONST_resId_imNetStatList,
        CONST_resId_talkerList,
        CONST_resId_messengerStatusList,
        CONST_resId_qList,
        CONST_resId_title,
        CONST_resId_logMsg,
        CONST_resId_saveMsgAsTextInDir,
        CONST_resId_saveMsgAsText,
        CONST_resId_sender,                 //  2016/02/29
                                            //
        CONST_resId_eConnTypeUnknown,
        CONST_resId_eConnType3Point5mm,
        CONST_resId_eConnTypeQuarter,
        CONST_resId_eConnTypeAtapiInternal,
        CONST_resId_eConnTypeRCA,
        CONST_resId_eConnTypeOptical,
        CONST_resId_eConnTypeOtherDigital,
        CONST_resId_eConnTypeOtherAnalog,
        CONST_resId_eConnTypeMultichannelAnalogDIN,
        CONST_resId_eConnTypeXlrProfessional,
        CONST_resId_eConnTypeRJ11Modem,
        CONST_resId_eConnTypeCombination,

        CONST_resId_eGeoLocRear,
        CONST_resId_eGeoLocFront,
        CONST_resId_eGeoLocLeft,
        CONST_resId_eGeoLocRight,
        CONST_resId_eGeoLocTop,
        CONST_resId_eGeoLocBottom,
        CONST_resId_eGeoLocRearPanel,
        CONST_resId_eGeoLocRiser,
        CONST_resId_eGeoLocInsideMobileLid,
        CONST_resId_eGeoLocDrivebay,
        CONST_resId_eGeoLocHDMI,
        CONST_resId_eGeoLocOutsideMobileLid,
        CONST_resId_eGeoLocATAPI,

        //
        CONST_resId_setPassword,
        CONST_resId_newMessenger,
        CONST_resId_renameMessenger,
        CONST_resId_sameMessenger,
        CONST_resId_rule,                       //  2016/04/07
        CONST_resId_autoAnswerlimited,
        //
        CONST_resId_channelType,
        CONST_resId_inOctets,
        CONST_resId_outOctets,
        CONST_resId_inSpeed,
        CONST_resId_outSpeed,
        //
        CONST_resId_qyTaskList,
        CONST_resId_qyTaskHisList,
        CONST_resId_msgList,
        CONST_resId_qyMsgToday,
        CONST_resId_today,
        CONST_resId_logList,
        CONST_resId_unfinished,
        //
        CONST_resId_dialUpLogList,
        CONST_resId_statIpScanList,
        CONST_resId_statPcInfoList,
        CONST_resId_statUsrInfoList,
        CONST_resId_statGeneralList,
        CONST_resId_statOthersList,
        CONST_resId_statMessengerInfoList,
        //
        CONST_resId_statCondition,
        CONST_resId_askForContentOfStat,
        CONST_resId_ipCount,
        CONST_resId_pcCount,
        CONST_resId_computer,
        CONST_resId_otherOs,
        CONST_resId_networkCard,
        CONST_resId_multiNetworkCard,
        CONST_resId_userCount,
        //
        CONST_resId_selectContentOfImport,
        CONST_resId_importBakData,
        CONST_resId_importRegInfo,
        CONST_resId_importBhbInfo,
        CONST_resId_selectFilenameFormat,
        CONST_resId_qyBakFileFormat,
        CONST_resId_qyBhbBakFileFormat,
        CONST_resId_selectFolder,
        CONST_resId_importOk,
        CONST_resId_importOkHint,
        CONST_resId_importWizard,
        CONST_resId_exportWizard,
        CONST_resId_selectExportPcSecChkFolder,
        CONST_resId_exportOk,
        CONST_resId_exportFailed,
        CONST_resId_emptyWizard,
        CONST_resId_fileFormat,
        CONST_resId_timeErr,
        CONST_resId_timeErr2,
        CONST_resId_clearVerify,
        CONST_resId_clearBhbVerify,
        //
        CONST_resId_pathErr,
        CONST_resId_current,
        CONST_resId_history,
        CONST_resId_paramErrMakeClientFailed,
        CONST_resId_folderErrMakeClientFailed,
        CONST_resId_makeClientOk,
        CONST_resId_makeClientFailed,
        CONST_resId_stopDialupDetectVerify,
        CONST_resId_cannotOpenRegistry,
        CONST_resId_err,
        CONST_resId_writeRegistryFailed,
        CONST_resId_stopServiceWarning,
        CONST_resId_qyStopIpScan,
        CONST_resId_qyStartIpScan,
        CONST_resId_qyStopRegScan,
        CONST_resId_qyStartRegScan,
        CONST_resId_stopUpdating,
        CONST_resId_startUpdating,
        //	qyMcMainProc1.cpp
        CONST_resId_NoPrivilegesToOpenPcList,
        CONST_resId_warning,
        CONST_resId_searchReg,
        CONST_resId_searchRegProm,
        CONST_resId_searchIp,
        CONST_resId_chaXun,
        CONST_resId_theOsIs,
        CONST_resId_thePatchIs,
        CONST_resId_belongToPc,
        CONST_resId_searchResultBelongTo,
        CONST_resId_port,
        CONST_resId_belongToUnknownDevice,
        //	CONST_resId_resultBelongto,
        CONST_resId_searchOfResult,
        //  CONST_resId_searchMac,
        CONST_resId_connection,
        CONST_resId_resultOfPort,
        CONST_resId_searchResult,
        CONST_resId_daysCannotBeFound,
        // qyStatusTool
        CONST_resId_communicationStatus,
        CONST_resId_communicationStatusWithFilters,
        CONST_resId_export,
        //
        CONST_resId_patchList,
        CONST_resId_userInfoList,
        CONST_resId_qyPcUsrInTab1List,
        //
        CONST_resId_qyNetworkView,
        CONST_resId_qyConnectionView,
        CONST_resId_qyNetworkSpeedView,
        CONST_resId_snmpObjRuleList,
        CONST_resId_conflictConnList,
        CONST_resId_directConnList,
        CONST_resId_connDevice,
        CONST_resId_searchList,
        CONST_resId_qySearch,
        CONST_resId_askIfPortDown,
        CONST_resId_cmdErr,
        CONST_resId_cmdOk,
        CONST_resId_tryAgainLater,
        CONST_resId_ifDeleteMac,
        //
        CONST_resId_ifDeleteDevice,
        //
        CONST_resId_all,
        CONST_resId_none,
        CONST_resId_fieldCannotSame,
        CONST_resId_qnmHint_startIp0,                               //  2008/12/27
        CONST_resId_qnmHint_endIpLtStartIp,                         //  2008/12/27
        CONST_resId_ifDeleteNetworkSegment,                         //  2008/12/27
        CONST_resId_ifDeleteNetworkSegmentSuffix,                   //  2008/12/27
        CONST_resId_ifModifyDefaultSnmpPolicy,                      //  2008/12/27
        CONST_resId_hintOfConfigSnmpFirst,                          //  2008/12/27
        CONST_resId_dbErrorResultCannotDelete,                      //  2008/12/27
        CONST_resId_theMeaningOfCfgSnmpGrpPolicy,                   //  2008/12/27
        CONST_resId_defaultPolicy,                                  //  2008/12/27
        CONST_resId_onlyReadCommunity,                              //  2008/12/27
        CONST_resId_writeCommunity,                                 //  2008/12/27
                                                                    //
        CONST_resId_grpNameCannotEmpty,                             //  2008/12/28
        CONST_resId_messengerNameCannotEmpty,                       //  2010/01/04
        CONST_resId_passwdCannotEmpty,                              //  2010/01/05
        CONST_resId_passwdNotConfirmed,                             //  2010/01/05
        CONST_resId_nameExists,                                     //  2010/01/05
                                                                    //
        CONST_resId_intervalBetween,                                //  2008/12/27
        CONST_resId_minuteAnd,                                      //	2008/12/27
        CONST_resId_uptoMinute,                                     //  2008/12/27
        CONST_resId_concurrentBetween,                              //	2008/12/27
        CONST_resId_and,                                            //	2008/12/27
        CONST_resId_upto,                                           //	2008/12/27
        CONST_resId_nMaxConns,                                      //  2008/12/28
        CONST_resId_ipWrong,                                        //  2008/12/28
        CONST_resId_default_qrtTitle,                               //	2008/12/28
        CONST_resId_default_qrtHint,                                //	2008/12/28
        CONST_resId_cliThreads,                                     //  2015/09/02
        CONST_resId_mgrThreads_cs,                                  //  2015/09/02
        CONST_resId_mgrThreads_route,                               //  2015/09/02
        CONST_resId_mgrThreads_sw,                                  //  2015/09/03
                                                                    //
        CONST_resId_ppExportClientHint,                             //  2015/11/10
                                                                    //
        CONST_resId_afterStandard,                                  //  2008/12/28
        CONST_resId_cannotBeEmpty,                                  //  2008/12/28
        CONST_resId_ifStandard,                                     //  2008/12/28
        CONST_resId_standardFinished,                               //	2008/12/28
        CONST_resId_standardFailed,                                 //  2008/12/28
        CONST_resId_ifSaveStandardPolicy,                           //	2008/12/28
        CONST_resId_standardPolicyCannotBeEmpty,                    //  2008/12/28
        CONST_resId_cannotFindSendChannel,                          //  2008/12/29
        CONST_resId_sendFile,                                       //  2009/01/01
        CONST_resId_recvFile,                                       //	2009/01/01
        CONST_resId_file,                                           //  2017/08/13
        CONST_resId_qingqiu,                                        //  2008/12/31
        CONST_resId_tasks,                                          //  2008/12/31
        CONST_resId_tempGroup,                                      //  2008/12/31
        CONST_resId_ifStopScreenSharingRemoteAssist,                //  2009/01/01
        CONST_resId_howToEnlarge,                                   //  2009/01/01
        CONST_resId_notUse2DConference,                             //  2015/08/07
        CONST_resId_notUseP2pCall,                                  //  2015/08/07
        CONST_resId_notConferenceManager,                           //  2015/08/07
        CONST_resId_notStartAv,                                     //  2017/02/13
        CONST_resId_notTestAv,                                      //  2017/02/13
        CONST_resId_notAcceptAv,                                    //  2017/02/13
        CONST_resId_quXiao,
        CONST_resId_canceled,
        CONST_resId_xiangYing,
        CONST_resId_pleaseSelect,
        CONST_resId_canSelect,
        CONST_resId_select,
        CONST_resId_toInterupt,
        CONST_resId_toRequestAgain,
        CONST_resId_notArrived,                                     //  2014/07/14
        CONST_resId_sendAgain,
        CONST_resId_faSong,
        CONST_resId_jieShou,
        CONST_resId_reqToSend,
        CONST_resId_accept,
        CONST_resId_deny,
        CONST_resId_interupted,
        CONST_resId_accepted,
        CONST_resId_denied,
        CONST_resId_ifExitSession,
        CONST_resId_localVideo,
        CONST_resId_ifAcceptVideo,
        CONST_resId_willSendFile,
        CONST_resId_pleaseRecvFile,
        CONST_resId_recvFinished,
        CONST_resId_sendFinished,
        CONST_resId_saveAs,
        CONST_resId_saved,
        CONST_resId_zhongDe,
        CONST_resId_pcRegistration,
        CONST_resId_askIfDeleteInfo,
        CONST_resId_message,
        CONST_resId_allSystemEvent,
        CONST_resId_uninstallClient,
        CONST_resId_allClientsAllowed,
        CONST_resId_onlyMgrRegList,
        CONST_resId_mgrAnyOne,
        CONST_resId_serviceIsRunning,
        CONST_resId_serviceHaveStopped,
        CONST_resId_hintToEmpty,
        CONST_resId_resetWhenUpTo,
        CONST_resId_ifChangeDb,                                         //  2009/01/02
        CONST_resId_cannotConnectToDb,
        CONST_resId_buildDbErr,
        CONST_resId_restart,
        CONST_resId_restoreAccess,
        CONST_resId_confMgr,                                                //  2015/08/10
        CONST_resId_confControl,                            //  2017/07/19
        CONST_resId_filterKeywords,                         //  2017/07/19
        CONST_resId_2all,                                   //  2017/07/19
        CONST_resId_2manager,                               //  2017/07/19
        CONST_resId_confManager,                            //  2017/07/19
        CONST_resId_confStarter,                            //  2017/07/19
        CONST_resId_confMember,                             //  2017/07/19
        CONST_resId_me,
        CONST_resId_confManaging,                           //  2017/07/19
        CONST_resId_conferencing,                           //  2017/07/19

        //  CONST_resId_setSlowSettings,									//  2009/10/27
        //  CONST_resId_setAdvancedSettings,								//  2009/07/15
        CONST_resId_cantSupportResolution,                                  //  2011/08/13
        CONST_resId_noAvExist,                                              //  2013/11/18

        CONST_resId_scope,
        //
        CONST_resId_displayOn,                                              //  2016/06/08
        CONST_resId_3dWall,                                                 //  2016/06/08
                                                                            //
        CONST_resId_desOfAdminGrp,
        CONST_resId_desOfLogGrp,
        CONST_resId_desOfCfgGrp,
        //	CONST_resId_beyondTheScopeOfManagement,
        CONST_resId_clearupAuditedKnowledge,
        CONST_resId_clearupUnAuditedKnowledge,
        CONST_resId_clearupKnowledge,
        CONST_resId_clearupPhoneBook,
        CONST_resId_importRecords,
        CONST_resId_countOfRecords,
        CONST_resId_importFinished,
        CONST_resId_deleteLogBetween,
        CONST_resId_cannotFindVideoDevice,
        CONST_resId_rangeOfCorporateStructureLevel,
        CONST_resId_rangeOfAssetNoNumber,
        CONST_resId_reallyToDelete,
        CONST_resId_rangeOfNumber,
        CONST_resId_whetherToModify,
        CONST_resId_hintToRestartProgram,
        CONST_resId_needAuthCondition,
        CONST_resId_createGroupFailed,
        CONST_resId_hintToAddMember,
        CONST_resId_modifyGroupNameFailed,
        CONST_resId_hintToCreateGroup,
        CONST_resId_hintToModifyGroup,
        CONST_resId_askIfRemoveFromGroup,
        CONST_resId_numberIsWrong,
        CONST_resId_selectGroup,
        CONST_resId_selectMessenger,
        CONST_resId_askIfDeletePhoneNumber,
        CONST_resId_needToSelect,
        CONST_resId_online,
        CONST_resId_offline,
        CONST_resId_hintOfModifyId,
        CONST_resId_newId,
        CONST_resId_hintOfReinput,
        CONST_resId_hintOfPhoneReinput,
        CONST_resId_hintOfRetype,
        CONST_resId_modifyFailed,
        CONST_resId_askIfDeleteContact,
        CONST_resId_askIfDeleteRegInfo,
        CONST_resId_askIfDeleteTmpRegInfo,
        CONST_resId_whetherOnline,
        CONST_resId_standardUsrInfoFailed,
        CONST_resId_whetherToSaveStandardRule,
        CONST_resId_saveStandardRuleFailed,
        CONST_resId_invaildIp,
        CONST_resId_invaildEndIp,
        CONST_resId_ipKnowledge,
        CONST_resId_registryKnowledge,
        CONST_resId_qyCfgModuleInfo,
        CONST_resId_phoneGuest,
        CONST_resId_errOfImportFmt,
        CONST_resId_unicodeImportFmt,
        CONST_resId_tooMoreColumn,
        CONST_resId_needColumn,
        CONST_resId_passwordLength,
        CONST_resId_passwordWrong,
        CONST_resId_passwordComposed,
        CONST_resId_passwordEmpty,
        CONST_resId_passwordCannotMatch,
        CONST_resId_passwordSaved,
        CONST_resId_userExisted,
        CONST_resId_userBelongToOtherGroup,
        CONST_resId_dbErr,
        CONST_resId_dbErrHint,  //  2009/01/27
        CONST_resId_addNewUser,
        CONST_resId_userEmpty,
        CONST_resId_invalidNumberAndMinIs,
        CONST_resId_invalidNumberAndMaxIs,
        //  2009/01/27
        CONST_resId_runAsAdmin,
        CONST_resId_installedProgramDb,
        CONST_resId_verIs,
        CONST_resId_updateProgramDbVer,
        CONST_resId_renameDbIs,
        CONST_resId_whetherToUpdate,
        CONST_resId_isWrong,
        CONST_resId_mightBe,
        CONST_resId_updateTo,
        CONST_resId_Import,
        CONST_resId_Export,
        CONST_resId_completed,
        CONST_resId_folderDoesNotExist,
        CONST_resId_pathIsNotAFolder,
        CONST_resId_beingProcessed,
        CONST_resId_interrupted,
        CONST_resId_incorrectUserOrPwd,
        CONST_resId_limitedUserAccountCannotLogon,
        CONST_resId_videoConference,
        CONST_resId_conferenceResources,
        CONST_resId_warn_tooManyVConferenceMems,
        CONST_resId_warn_onlyQuadCoreCanSupport,
        CONST_resId_licenseTimeout,
        CONST_resId_trialPeriod,
        CONST_resId_days,
        CONST_resId_whetherToSave,
        // netMc2
        CONST_resId_qyScannedIpList,
        CONST_resId_assetList,
        CONST_resId_pcSecChkEventList,
        CONST_resId_pcRuleList2,
        CONST_resId_pcNetworkPropList,
        CONST_resId_pcNetworkRuleList2,
        CONST_resId_pcProxyCfgPropList2,
        CONST_resId_pcProxyCfgRuleList2,
        CONST_resId_pcComputerPropList2,
        CONST_resId_pcComputerRuleList2,
        CONST_resId_pcWindowsUpdatePolicyPropList,
        CONST_resId_pcWindowsUpdatePolicyRuleList,
        CONST_resId_pcProcessPropList2,
        CONST_resId_pcProcessModulePropList,
        CONST_resId_pcProcessRuleList2,
        CONST_resId_pcNetFlowPropList,
        CONST_resId_pcNetFlowPropList_ethernet,
        CONST_resId_pcNetSharePropList,
        CONST_resId_pcPrinterPropList2,
        CONST_resId_pcRegistryList,
        CONST_resId_pcRegistryRuleStatusList,
        CONST_resId_pcEventLogList,
        CONST_resId_pcOsUsrList,
        CONST_resId_qyPcNetStatList,
        //
        CONST_resId_wuserverAddr,                           //  2009/04/11
        CONST_resId_overall,
        CONST_resId_disable,
        CONST_resId_useDefault,                             //  2014/07/31
        CONST_resId_baoJingLiuLiang,
        CONST_resId_detectionFrequency,
        CONST_resId_liuLiangBaoJingShiShiFouZuDuan,
        CONST_resId_monPics,
        CONST_resId_cancelMonDesktop,
        //
        CONST_resId_qyHint_netMcDelPcs,
        CONST_resId_qyHint_netMcDelPcUsr,
        CONST_resId_qyHint_netMcDelPcRule,
        CONST_resId_qyHint_netMcDelProxy,
        CONST_resId_qyHint_netMcDelPcSp,
        CONST_resId_qyHint_netMcDelPcUsrInTab1,
        CONST_resId_qyHint_netMcDelPcRegistryInfo,
        CONST_resId_cancelFailed,
        CONST_resId_deleteConnectionManually,
        CONST_resId_thePortandMacIs,
        CONST_resId_askIfDeleteId,
        CONST_resId_askIfDeleteTmpId,
        CONST_resId_askIfDeleteMsgr,
        //
        CONST_resId_askIfDeleteBrowsingHistory,
        CONST_resId_askIfDeleteRegistry,
        CONST_resId_askIfDeleteWindowsLog,
        CONST_resId_askIfDeleteUserAccount,
        CONST_resId_askIfDeleteNetstat,
        CONST_resId_askIfDeletePrinter,
        CONST_resId_askIfDeleteProcess,
        CONST_resId_askIfDeleteRule,
        CONST_resId_askIfDeleteFlow,
        CONST_resId_askIfDeleteNetSharing,
        CONST_resId_askIfDeleteConnection,
        COSNT_resId_warningToKillProcess,
        CONST_resId_existRule,
        CONST_resId_askIfChangeDesignetedToProhibit,
        CONST_resId_askIfChangeUniformToProhibit,
        CONST_resId_askIfKillDesignetedPcProcess,
        CONST_resId_askIfKillProcess,
        CONST_resId_macChanged,
        CONST_resId_invalidQwm,
        CONST_resId_checkForProxyServer,
        CONST_resId_qyIntraConn,
        CONST_resId_qyOnInternet,
        CONST_resId_changeCapScreenPolicy,
        CONST_resId_changeCapScreenDir,
        CONST_resId_changeCapScreenInterval,


        CONST_resId_startTesting,           //  2014/06/06
        CONST_resId_stopTesting,
        //
        CONST_resId_procRtsp,                   //  2017/06/10
        CONST_resId_hide,                       //  2017/06/10
        CONST_resId_remoteStorage,              //  2017/06/10
        CONST_resId_autoPopupAndHideOnStartup,  //  2017/06/10
                                                //  CONST_resId_autoOpenOnStartup,			//  2017/06/10
                                                //
        CONST_resId_menu,                   //  2016/04/07
                                            //
        CONST_resId_useBgWall,                                      //  2014/10/07
        CONST_resId_showBgWall,                                     //  2014/10/08
        CONST_resId_never,                                          //  2014/10/08
        CONST_resId_bgWallSettingsErr_0,                            //  2014/10/13
        CONST_resId_bgWallSettingsErr_1,                            //  2014/10/12
                                                                    //
        CONST_resId_bgWall,                                         //  2018/01/01
                                                                    //
        CONST_resId_ucbHideImGrpMems,       //  2017/09/02	
        CONST_resId_ucbShowImGrpMems,       //  2017/09/02
                                            //
        CONST_resId_startAV,                //  2013/12/20
        CONST_resId_acceptAV,               //  2013/12/20
        CONST_resId_denyAV,                 //  2013/12/20
        CONST_resId_requestToSpeak,         //  2013/12/20
        CONST_resId_speak,                  //  2017/07/19
        CONST_resId_stopSpeaking,           //  2017/07/19
        CONST_resId_fullScreenSharing,      //  2009/09/14
        CONST_resId_fullScreen,             //  2011/03/06
        CONST_resId_fullWindow,             //  2016/02/16
        CONST_resId_notFullWindow,          //  2016/02/16
        CONST_resId_endFullScreen,
        CONST_resId_2ndScreen,              //  2017/10/02
                                            //
        CONST_resId_playRemoteAudio,        //  2016/02/22
        CONST_resId_notPlayRemoteAudio,     //  2016/02/22
                                            //
                                            //CONST_resId_closeAndClean,		//  2016/02/16
                                            //
        CONST_resId_gotoWorkWindow,         //  2016/02/16	
                                            //
                                            //CONST_resId_locked,				//  2016/02/16
                                            //CONST_resId_toLock,				//  2016/02/16
                                            //
        CONST_resId_close,                  //  2016/02/16
        CONST_resId_joinInBgWall,           //  2016/03/12
        CONST_resId_notJoinInBgWall,        //  2016/03/12
        CONST_resId_hideBgWall,             //  2016/03/27
        CONST_resId_showBgWall1,            //  2016/03/27
        CONST_resId_closeWall,              //  2014/12/07
        CONST_resId_exit,                   //  2014/12/12
        CONST_resId_startSharing,           //  2011/10/25
        CONST_resId_stopSharing,            //  2011/10/25
        CONST_resId_startPlayingAudio,      //  2015/05/18
        CONST_resId_stopPlayingAudio,       //  2015/05/18
        CONST_resId_running,                //  2011/10/25
        CONST_resId_started,                //  2013/04/06
        CONST_resId_manuallyStarted,        //  2014/06/02
        CONST_resId_start,                  //  2013/04/06
        CONST_resId_stop,                   //  2013/04/06
        CONST_resId_unresizable,            //  2015/10/24
        CONST_resId_stopUnresizable,        //  2015/10/24
        CONST_resId_opAvDev,                //  2017/06/10
        CONST_resId_opScreen,               //  2017/06/10
        CONST_resId_save,                   //  2017/06/10
                                            //
        CONST_resId_micOff,                 //  2014/12/11
        CONST_resId_micOn,
        CONST_resId_remoteMicOnOff,         //  2017/07/19
        CONST_resId_selectedConfManage,     //  2017/07/19
        CONST_resId_cannotSpeak,            //  2015/02/16
        CONST_resId_micListFull,            //  2015/02/16
        CONST_resId_waitingForPermitted,    //  2015/02/16
        CONST_resId_systemMessage,          //  2015/02/16
                                            //
        CONST_resId_lianJieMAC,             //  "联接MAC"
        CONST_resId_caoZuoXiTong,
        CONST_resId_buDing,                 //  补丁
        CONST_resId_leiXing,
        CONST_resId_wangLuoSheBei,          //  "网络设备"
        CONST_resId_likeSearch,             //  
        CONST_resId_switchPort,             //	"端口"
        CONST_resId_mingChen,               //  名称
        CONST_resId_idStr_related,
        CONST_resId_jinChengMingChen,       //  进程名称
                                            //
        CONST_resId_moKuaiMingChen,         //  模块名称
        CONST_resId_cmdLine,                //  命令行参数
                                            //
                                            //
        CONST_resId_yingJianReport,         //  计算机硬件资产报表
                                            //  
        CONST_resId_policy_isClient,        //  个性化策略, 2007/09/08
        CONST_resId_imAuthCond_hint,        //  提示
        CONST_resId_viewOwnedImGrpList,     //  查看所在分组
        CONST_resId_removeFrom,             //  移出分组
        CONST_resId_viewOwnedPhoneList,     //  查看手机
        CONST_resId_ucbDefaultSmContact,    //  是否缺省接收
        CONST_resId_smContactMsgrId,
        CONST_resId_verifyPhoneListHint,
        CONST_resId_staticToPaths,          //  发送通道
        CONST_resId_localToPaths,           //  本地发送通道

        //
        CONST_resId_selectCompressor,       //  2009/05/22
        CONST_resId_warning_restrictSharedDymBmps,  //
        CONST_resId_ucbUseDynBmpRule,
        CONST_resId_ucbUseDynBmpRule_demo,
        CONST_resId_anotherLogon,           // 2011/02/03
                                            //
        CONST_resId_anotherLogonExists,     //  2011/03/20
        CONST_resId_areYouSure,             //  2011/03/20
        CONST_resId_askIfUseDefault,        //  2013/04/06
        CONST_resId_willYouClose,
        CONST_resId_cancelAV,
        CONST_resId_re_accept,              //  2016/02/17
                                            //
        CONST_resId_cancelTask,         //  2016/02/29
        CONST_resId_taskCanBeReaccepted,                //  2016/02/29
        CONST_resId_waitToRecv,             //  2016/02/29
                                            //
        CONST_resId_screenSharing,
        CONST_resId_cancelScreenSharing,
        CONST_resId_hideMsgBox,
        CONST_resId_showMsgBox,
        CONST_resId_resetMsgBox,            //  2011/10/04
        CONST_resId_avExclusively,
        CONST_resId_avNonExclusively,
        CONST_resId_hideMore,
        CONST_resId_more,
        CONST_resId_loggedInto,             //  2016/08/15
        CONST_resId_disconnectedFrom,       //  2016/08/15
        CONST_resId_viewDlg,                //  2013/09/08
        CONST_resId_closeDlgTalk,           //  2013/09/08
        CONST_resId_toggleFullScreen,       //  2013/09/08
        CONST_resId_remoteVideo,            //  2016/04/07
        CONST_resId_ptz,                    //  2013/02/16
        CONST_resId_hidePtz,                //  2013/02/16
        CONST_resId_subtitles,              //  2013/04/05
        CONST_resId_hideSubtitles,          //  2013/04/05
        CONST_resId_offlineRes,             //  2015/08/01
        CONST_resId_hideOfflineRes,         //  2015/08/01
        CONST_resId_playMediaFile,          //  2016/04/07
        CONST_resId_showMenu,               //  2012/04/09
        CONST_resId_hideMenu,
        CONST_resId_seconds,
        CONST_resId_automaticallyCanceled,
        CONST_resId_after,
        CONST_resId_doYouWantToExit,
        CONST_resId_searchingServer,
        CONST_resId_tryToConnect,
        CONST_resId_cannotFindServer,
        CONST_resId_failedToLogon,
        CONST_resId_conferenceTimeout,      //  2011/08/02
        CONST_resId_subtitleLimited,        //  2013/03/21
        CONST_resId_imGrps_av,              //  2014/02/01
        CONST_resId_imGrps_msg,             //  2014/02/01
        CONST_resId_imGrps_resource,


        //
        CONST_resId_imObjRuleList,          //  2010/08/16
                                            //
        CONST_resId_name_isMgr_plus,        //  2011/03/27
                                            //
        CONST_resId_testAvHint,             //  2013/04/09
                                            //
        CONST_resId_notSupportRtsp,             //  2014/06/04

        CONST_resId_welcomeToUse,               //  2013/09/11
        CONST_resId_qycxVideoconference,        //  2013/09/11
        CONST_resId_wizard_welcome,         //  2013/09/11
                                            // 3D wall
        CONST_resId_viewDlgTalk_note,       // 2013/12/21

        CONST_resId_3dFont,
        //
        CONST_resId_avLevel_1080p,
        CONST_resId_avLevel_720p,
        CONST_resId_avLevel_576p,
        CONST_resId_avLevel_480p,
        CONST_resId_avLevel_240p,
        //
        CONST_resId_avLevel_dev_5fps2,
        CONST_resId_avLevel_dev_5fps1,
        CONST_resId_avLevel_dev_5fps,
        CONST_resId_avLevel_dev_2fps,

        //
        CONST_resId_noPrompt,                       //  2017/07/19

        //  dlg_offlineRes
        CONST_resId_downloading,
        CONST_resId_uploading,
        CONST_resId_cannotConnectFileServer,
        CONST_resId_mayBeLocked,                    //  2015/08/11
                                                    //
                                                    //
        CONST_resId_appName,
        CONST_resId_allImObjectsRetrieved,
        CONST_resId_imObjListRetrieved,
        CONST_resId_staticUsrName,
        CONST_resId_staticServer,
        CONST_resId_staticStatusServer,
        CONST_resId_apply,
        CONST_resId_staticCreatorIdInfo,
        //
        CONST_resId_changeCamera,
        //
        CONST_resId_yes,
        //
        CONST_resId_waitToQuit,
        CONST_resId_progress,
        //
        CONST_resId_tooManySpeaks,
        CONST_resId_cantTalkToMySelf,


        //
        CONST_resId_tempTotal,
        // };



    };

    partial class qyFuncs
    {
    }
}
