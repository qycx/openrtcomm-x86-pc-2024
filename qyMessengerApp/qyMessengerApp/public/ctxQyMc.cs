using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
    public const int CONST_aPlayType_null = 0;
    public const int CONST_aPlayType_wave = 1;
    public const int CONST_aPlayType_dSound = 2;
    public const int CONST_aPlayType_webRtc = 3;
    //
    public const int CONST_aPlayType_android = 10;
    //
    public const int CONST_vPlayType_v = 100;
    //
    public const int CONST_vPlayType_android = 110;

}


namespace qy
{

    public delegate int PF_newVar(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2);
    public delegate int PF_freeVar(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2);
    public delegate int PF_initVar(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2);
    public delegate int PF_init_afterLogon(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2);
    public delegate int PF_exitVar(CCtxQyMc pQyMc, ref QY_SERVICEGUI_INFO pSci, IntPtr p2);


    //
    public struct PARAM_initQyMc
    {
        //  2015/07/09. pInfo->cfg.pfNewVar(  pQM,  pInfo,  0  )  
        public PF_newVar pfNewVar;                                                  //  2015/01/20
        public PF_freeVar pfFreeVar;
        public bool bCalledByQmd;                                              //  2015/07/17
    }
    ;


    public unsafe struct QY_MC_CFG
    {
        int i;

        /*
		TCHAR rootKey[QY_MAXKEYLEN + 1];
		WCHAR installDir[QY_MAXPATHLEN + 1];
		WCHAR appDataDir[CONST_qyMaxPathLen + 1];       //  applicationData\\qycx\\·
		WCHAR workDir[QY_MAXPATHLEN + 1];
		WCHAR cusModuleDir[CONST_qyMaxPathLen + 1];     //  
		WCHAR singleInstanceStr[32 + 1];

		QY_MC_BTNCFG btn;
		QY_MC_IMAGECFG image;
		UINT nIdToolBar;
		QNM_SYSCFG_INFO* pSysCfg;                                   //  2007/03/11
		QNM_GUICFG_INFO* pGuiCfg;
		*/
        //  
        public bool bGetPolicyFromIni;                         //  2010/07/27. especially for winCe

        //
        //QNM_MODULES_LOADEDINFO modulesLoadedInfo;                           //  2004/08/16
        //
        public bool ucbThinkPad;                              //  
        public bool ucbBhqj;                                  //  2005/04/20
        public bool ucbHaveDj;                                    //  2005/08/27
        public bool ucbAdvancedOptionEnabled;                 //  2005/11/18
        public bool ucbAutoLogonSupported;                        //  2006/03/08£¬__SUPPORT_AUTOLOGON__
        public bool ucbRemoteDesktopSupported;                    //  2006/03/08£¬__NOTSUPPORT_REMOTEDESKTOP__
        public bool ucbPcMonSupported;                            //  2006/03/08£¬__NOTSUPPORT_PCMON__
        public bool ucbFileMonSupported;                      //  2006/03/25
        public bool ucbRasControlNotSupported;                    //  2009/03/05
                                                                  //


        //  2011/03/26
        public bool ucbNotSupported_dblClk;                       //  在触摸屏时，最好不支持双击。

        //
        public bool ucbNotNeedLocked;                         //  
        public bool ucbUseTrayIcon;                               //  
        public int iIDR_TRAY;
        //
        //
        public bool ucbDsnless;                                   //  DNSLESS2007/06/13
        public fixed char defaultDbName[Consts.MAX_PATH + 1];              //  ACCESS DSN

        //char name[QIM_MAXNAMELEN + 1];
        public uint mutexTimeout;

        public QNM_DB_CFG db;                                          //  


        public SOCK_TIMEOUT to;
        public SOCK_PARAM sockParam;
        public int iTimeoutInMs_channelIdle_others;            //  2009/02/25
                                                               //  int										iTimeoutInMs_channelIdle_recv_others;		//  2011/01/31
        public int iTimeoutInMs_channelIdle_recv_talk;         //  2011/01/31

        public ushort usDelayInMsToViewToolbar;                    //  2010/09/02

        //
        public RW_lock_param rwLockParam;                              //  2009/05/13

        //  
        public bool ucbStartRegScan;

        //
        public int iType_qnmTmpTab;

        //WCHAR cfgFullFileName[CONST_qyMaxPathLen + 1];  //  added on 2003/09/19. 不提供给用户编辑 
        //WCHAR iniFullFileName[CONST_qyMaxPathLen + 1];  //  2009/07/29。提供给用户编辑
        //WCHAR qyMcTitle[256 + 1];

        public bool ucbAdvancedVer;                               //  2005/08/30

        //QNM_CUSTOM_COMM custom;

        public ushort usMaxDisplayedElemsPerWnd;                   //  2007/06/09
        public ushort usMaxDisplayedWnds;                          //
                                                                   //
        public ushort usMaxDisplayedTasksInDlgTalk;                //  2007/06/25

        //
        public QWM_DYN_CFG dynCfg;                                     //  2007/06/16
        public fixed char seqNo[64 + 1];
        public int iSystemId_wanted;                           //  ÎªÈ·±£ÏµÍ³Éú³ÉµÄÕýÈ·ÐÔ£¬¼ÓÈëÁË¶ÔiSystemIdµÄºË¶Ô

        //  2009/03/05
        //QY_MC_logonCfg logon;

    };

    //
    public struct CCtxQyMc_globals
    {
        public int lTranNo_wallMem;                                          //  2016/01/05
                                                                             //
        public int lTranNo_player;                                                //  2016/02/18
                                                                                  //
    };

    //
    public partial class CCtxQyMc : CCtxQyMcTmpl
    {

        //
        public int iSystemType_reserved;
        public int iSystemId;
        public int iAppType;
        public int iAppId;
        public int iCustomId;
        public int iServiceId;
        public byte[] ver;

        //
        public QY_ENV env;                         //  2005/11/03

        //
        public uint uiAPlayType;

        public QMC_APP_PARAMS appParams;

        public QY_MC_CFG cfg;


        public QNM_CUSRES_INFO cusRes;                         //  2005/11/03
        bool ucbResInited;



        //
        public QY_MC_GUI gui;

        //
        public QY_OBJ_DB procedObjDb;             //  2006/12/17
        public int iDsnIndex_mainSys;

        //
        public bool bLogon;
        public uint dwTickCnt_logon;              //  2013/04/06
                                                  //QYMC_USR curUsrInfo;

        //
        public bool bQuit;
        public bool bGuiQuit;                      //  2008/10/03
        public ushort usTimes_tryToQuit;
        //
        public bool bAskingToQuit;                 //  2011/03/18
        public bool bQuitVerified;                 //  2011/03/16
                                                   //
        public bool bPrepareToGuiQuit;             //  2016/03/28

        //
        public bool b_notifyPanic;
        public uint dwLastTickCnt_notifyPanic;
        public int nTimes_notifyPanic;


        //
        public object pShowInfoStruct;

        //
        public QM_dbFuncs p_g_dbFuncs;                                                //  2016/09/04

        //  2009/10/11
        public bool ucbDaemonDemo;
        public ushort usMaxVideoConferenceMems;
        public byte ucDays_left;                                              //  2010/03/22
                                                                              //
        public ushort usAppAvLevel_default;                                        //  2013/06/28
        public ushort usAppAvLevel_daemon;                                     //  2011/10/22
                                                                               //
        public ushort usSubSystemId_daemon;                                        //  2017/06/21

        //
        public bool ucbSupported_remoteStorage_default;                           //  2014/08/03
        public bool ucbSupported_fileServer_default;                          //  2015/08/04

        //
        public bool ucbSupported_gps_default;                                 //  2013/06/28

        //
        //
        public uint uiMaxTimeInS_videoConference;                              //  2011/08/01
        public uint uiMaxWarningInS_videoConference;                           //  2013/08/20

        //
        public uint uiMaxTimeInS_videoConference_general;                      //  2011/08/01
        public uint uiMaxTimeInS_videoConference_freeVer;                      //  2011/08/01


        //  2015/07/09. pInfo->cfg.pfNewVar(  pQM,  pInfo,  0  )  
        public PF_newVar pfNewVar;                                                  //  2015/01/20
        public PF_freeVar pfFreeVar;
        public bool bCalledByQmd;                                              //  2015/07/17

        //
        public CCtxQyMc_globals globals;


        //
        public CUS_MODULES cusModules;



        public QY_SERVICEGUI_INFO serviceGuiInfo;

        public object[] hDaemonThreads; //[Consts.QY_MC_TOTAL_DAEMON_THREADS];
        public PF_threadProc[] threadProcs; //[Consts.QY_MC_TOTAL_DAEMON_THREADS];

        //
        public bool b_isHarmony;
        public int iManufacturer;
        public int iModel;
        //
        public bool fake_bFolder;

        //
        public CCtxQyMc()
        { }

        ~CCtxQyMc()
        {

        }

        public CCtxQmc get_pProcInfo()
        {
            return (CCtxQmc)this.serviceGuiInfo.pVar;
        }

        public int setQmDbFuncs(int iDbType, QM_dbFuncs pDbFuncs)
        {
            CCtxQmc pProcInfo = get_pProcInfo();
            if (null == pProcInfo) return -1;

            return pProcInfo.setQmDbFuncs(iDbType, pDbFuncs);
        }

        //
        public unsafe int yv12ToRgb24(byte* yuv, byte* rgb, int width, int height, bool bFlipImg)
        {
            qyFuncs.traceLog("not finised");
            return -1;
        }



    }

    partial class qyFuncs
    {

        public static unsafe int initQyMc(IntPtr g_hInst, PARAM_initQyMc pParam_initQyMc, CCtxQyMc pQyMc)
        {
            int iErr = -1;

            //
            string str;

            //
            CCtxQyMc g_pQyMc = pQyMc;

            //
            //  2015/07/08. 调用前，应先new
            if (g_pQyMc == null)
            {
                Console.WriteLine("initQyMc failed, g_pQyMc is not newed");
                return -1;
            }

            Console.WriteLine("initQyMc entersm");


            //
            //PARAM_initQyMc param_initQyMc;
            //memset(&param_initQyMc, 0, sizeof(param_initQyMc));
            //if (pPARAM_initQyMc) memcpy(&param_initQyMc, pPARAM_initQyMc, sizeof(param_initQyMc));





            //
            g_pQyMc.iSystemId = Consts.CONST_qySystemId_messenger;
            //  g_pQyMc->iAppType   =   CONST_qyAppType_mc;
            g_pQyMc.iAppType = Consts.CONST_qyAppType_client;      //  2004/05/23ÐÞ¸Ä
            g_pQyMc.iCustomId = Consts.CONST_qyCustomId_general;
            g_pQyMc.iServiceId = Consts.CONST_qyServiceId_is;                                       //  2007/03/07
                                                                                                    //
                                                                                                    //g_pQyMc.g_hInst = g_hInst;                                                                         // Store instance handle in our global variable
                                                                                                    //
            g_pQyMc.usAppAvLevel_default = Consts.CONST_qyAppAvLevel_fullHd;                                          //  2013/06/28
                                                                                                                      //
                                                                                                                      //  2015/07/17
            g_pQyMc.pfNewVar = pParam_initQyMc.pfNewVar;
            g_pQyMc.pfFreeVar = pParam_initQyMc.pfFreeVar;
            g_pQyMc.bCalledByQmd = pParam_initQyMc.bCalledByQmd;

            //

            string errHintWndCls = "";// CONST_qnmStatusToolWndClass_client;
            string statusTool_servIp = "192.168.123.161";

            //
            GuiShare.pf_readDbgCfg(ref statusTool_servIp);

            //
#if DEBUG
            //statusTool_servIp="192.168.111.224";
#endif


            //setDebugLogFileModuleName();
            //  setErrHintWndCls(  errHintWndCls  );
            initShowInfo_cli(IntPtr.Zero, _T("syncQ_showInfo_qmc"), statusTool_servIp, ref g_pQyMc.pShowInfoStruct);

            //  2013/07/31
            //  g_pQyMc->pShowInfoStruct  =  get_pShowInfoStruct(  );

            //
            showInfo_open(0, null, "initQyMc, after initShowInfo_cli");


            //
            CCtxQyMc pQM = g_pQyMc;
            int i;
            uint dwThreadDaemonId;                     //ÓÃÓÚºóÌ¨Ë¢ÐÂ£¬ÔÝ²»Ê¹ÓÃ
                                                       //  CFile			*	pLckFile				=	NULL;
                                                       //  CQyString			lckCStr;
            /*
QY_DMITEM* pItem = NULL;        //  2005/11/07
QY_DYN_LIBS* pDynLib = NULL;
TCHAR systemDir[MAX_PATH+1] = _T("");

myGetSystemDirectory(systemDir, mycountof(systemDir));
tTrailDir(systemDir, mycountof(systemDir));

//  if  (  qyInitSnmp(  &ghDll_InetMib1  )  )  goto  errLabel;
if (initDynLib((void**)&pDynLib)) goto errLabel;    //  2007/01/21
if (initDynLib_dx(systemDir, &pDynLib->pLib_dx)) goto errLabel;

//  2010/07/05
if (qyTcpStart()) goto errLabel;    //  

//  È¡»·¾³²ÎÊý,  2005/11/03
			*/
            qyFuncs.getQyEnv((IntPtr)null, ref pQM.env);

            /*
//  pQM->env.pDynLibs  =  pDynLib;
pQM->ucbDynLibInited = TRUE;

//  2009/07/12
g_pEnv = &pQM->env;

//  È¡ÄÚ´æ´óÐ¡. 2008/10/08
MEMORYSTATUS memStatus;
memset(&memStatus, 0, sizeof(memStatus));
memStatus.dwLength = sizeof(MEMORYSTATUS);
GlobalMemoryStatus(&memStatus);
pQM->uiTotalPhys_memStatus = memStatus.dwTotalPhys;
if (isMem512(pQM)) OutputDebugString(_T("Memory is <=  512M\n"));
//  2010/06/24
# ifdef  __SUPPORT_waveformFirst__
traceLog(_T("initQyMc: Waveform is supported first"));
# ifdef  __DEBUG__
//  pQM->bUseDirectSound_toRecord  =  bSupported_directSound(  );
//  pQM->bUseDirectSound_toPlay  =  bSupported_directSound(  );
#endif
#else
//  
//  pQM->bUseDirectSound_toPlay  =  bSupported_directSound(  );
if (bSupported_directSound()) pQM->uiAPlayType = CONST_aPlayType_dSound;  //  2017/02/16
else pQM->uiAPlayType = CONST_aPlayType_wave;
#endif


//
TCHAR* pCmdLine = GetCommandLine();
if (parseCmdLine_qmc(pCmdLine, &pQM->appParams)) goto errLabel; //  2011/05/31

//  2013/06/13
# ifdef  __APP_qyMc_touchscreen__
pQM->ucbApp_ts = TRUE;
#endif

# ifdef  __DEBUG__
pQM->ucbUseNamedShm = TRUE;
pQM->ucbDebugSeparately = TRUE;
traceLogA("initQyMc: ucbUseNamedShm and ucbDebugSeparately is set for debug");

//
#if  0
pQM->env.iPlatformId  =  CONST_qyPlatform_win7;
traceLog(  _T(  "WARNING: Fake os to be win7"  )  );
showInfo_open(  0,  0,  _T(  "WARNING: Fake os to be win7"  )  );
#endif

#endif
			#endif
			*/

            //
            set_who_showInfo(_T("999"));

            ////
            //
            if (getQyMcInitialCfg(pQyMc) != 0) goto errLabel;



            //
            //  2016/09/04	 
            //extern QM_dbFuncs global_dbFuncs;
            //
            pQM.p_g_dbFuncs = new QM_dbFuncs();// &global_dbFuncs;
                                               //memset(pQM->p_g_dbFuncs, 0, sizeof(global_dbFuncs));

            /*
				
				 //  2005/11/03
				 #ifdef  __DEBUG__
						 traceLogA(  "before initQnmRes"  );
			#endif
					int iResId_sys = pQM->cfg.pGuiCfg->iResId_sys;
				 //
				 if  (g_pQyMc->bCalledByQmd  )  {
					 iResId_sys  =  CONST_resId_sys_qmdCliHelp;
					 //  2015/07/17
					 //下面是要在cusRes里建立sys_qmdCliHelp对应的资源文件
				 }
				 //  
				 if  (pQM->initQnmRes(  0, pQM->env.usLangId, iResId_sys  )  )  
				 {
					 #ifdef  __DEBUG__
							 traceLogA(  "initQyMc failed, initQnmRes failed"  );
			#endif
				qyShowInfo1(CONST_qyShowType_qwmComm,  0, (  ""  ),  _T(  "Client"  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "initQyMc failed, initQnmRes failed."  )  );
					 goto  errLabel;
				 }
			pQM->ucbResInited = TRUE;
			//
			//  2015/06/28
			set_g_pQnmCusResInfo_qisGuiPublic(&pQM->cusRes);
			//
			if (pQM->addModuleMem_qnmRes(CONST_resId_sys_qisGuiPublic))
			{
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("Client"), 0, _T(""), _T(""), _T("initQyMc failed, addModuleMem_qnmRes failed."));
				goto errLabel;
			}
			//
			# ifdef  __isCli__
			if (pQM->addModuleMem_qnmRes(CONST_resId_sys_isCliHelp)) goto errLabel;
			if (pQM->addModuleMem_qnmRes(CONST_resId_sys_qyMessengerHelp)) goto errLabel;
			#endif
			# ifndef  __isCli__	//  2013/06/15
			set_cur_iResId_sys(pQM->cfg.pGuiCfg->iResId_sys);
			#endif



			//  2010/10/02

			//  2013/10/03
			# ifdef  __isCli__
			init_imgProcess();
			#endif

			//  2009/12/21

			//  2009/01/04
			setAllHints(&pQM->cusRes, GetSystemDefaultLangID());
			//
			//  2017/01/11
			if (bAppClient(pQM->iAppType))
			{
				if (getCfgValByNameT(pQM->cfg.iniFullFileName, _T(CONST_cfgName_qyClientTitle), pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle))) pQM->cfg.qyMcTitle[0] = 0;
			}
			else
			{
				if (getCfgValByNameT(pQM->cfg.iniFullFileName, _T(CONST_cfgName_qyMcTitle), pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle))) pQM->cfg.qyMcTitle[0] = 0;
			}

			//
			tTrim(pQM->cfg.qyMcTitle);
			if (!pQM->cfg.qyMcTitle[0]) lstrcpyn(pQM->cfg.qyMcTitle, getResStr(0, &pQM->cusRes, pQM->cfg.pGuiCfg->iResId_sys), mycountof(pQM->cfg.qyMcTitle));
			if (pQM->iServiceId == CONST_qyServiceId_is)
			{
				if (!bAppClient(pQM->iAppType))
				{
					//
					_sntprintf(pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle), _T("%s %s"), pQM->cfg.qyMcTitle, getTableResStr(0, CONST_subSystemIdTable_en, &pQM->cusRes, qyGetSubSystemId_mgr()));

					//
					_sntprintf(pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle), _T("%s %s"), pQM->cfg.qyMcTitle, getTableResStr(0, CONST_qyAppAvLevelTable_en, &pQM->cusRes, qyGetAppAvLevel_qyMc(pQM)));
					//
					if (bSupported_devAuth())
					{
						_sntprintf(pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle), _T("%s %s"), pQM->cfg.qyMcTitle, getResStr(0, &pQM->cusRes, CONST_resId_name_isMgr_plus));
					}
				}
				//
				if (bAppClient(pQM->iAppType))
				{
					_sntprintf(pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle), _T("%s %s"), pQM->cfg.qyMcTitle, getTableResStr(0, CONST_qyAppAvLevelTable_en, &pQM->cusRes, qyGetAppAvLevel_qyMc(pQM)));
				}
			}
			# ifndef  __WINCE__
			if (bQnmDemo()) _sntprintf(pQM->cfg.qyMcTitle, mycountof(pQM->cfg.qyMcTitle), _T("%s( Demo )"), pQM->cfg.qyMcTitle);
			#endif

		
			*/

            //pQM->env.iEnvType = QY_ENVTYPE_MCGUI;
            if (!bChkQyMcEnv(pQM))
            {
                qyFuncs.showInfo_open(0, "", "bChkQyMcEnv failed");
                goto errLabel;
            }



            //  2008/01/25

            //
            //pQM->gui.ctx_gui_thread.dwThreadId = GetCurrentThreadId();

            if (CQnmDb.initDbPool() != 0) goto errLabel;

            /*
			//  2013/06/08
			QNM_db_pool* pPool = CQnmDb::getDbPool();
			if (!pPool) goto errLabel;

			//
			# ifdef  __isCli__
			if (pQM->set_g_dbPool_isCliHelp(pPool)) goto errLabel;
			//
			# ifdef  __USE_qyMessengerHelp__
			if (set_g_dbPool_qyMessengerHelp(pPool)) goto errLabel;
			#endif
			#endif
			*/


            /*
			# ifndef  __WINCE__
			if (initQyMcMainHook())
			{
			# ifdef  __DEBUG__
				traceLog(_T("initQyMc: initQyMcMainHook failed"));
			#endif
				goto errLabel;  //  2005/10/01
			}
			pQM->gui.ucbMainHooked = TRUE;
			#endif
			*/

            pQM.gui.processQ = new GENERIC_Q();
            if (qyFuncs.initGenericQ("guiProcQ", Consts.CONST_mutexName_guiProcessQ, 0, Consts.CONST_maxQNodes_guiProcessQ, qyFuncs.mymalloc, qyFuncs.myfree, pQM.gui.processQ) != 0) goto errLabel;
            pQM.gui.ucbProcessQInited = true;



            //
            //
            //
            pQM.cusModules = new CUS_MODULES();


            //  2016/09/19
            pQM.serviceGuiInfo.pParent = pQM;

            //  2015/01/20
            if (pQM.serviceGuiInfo.cfg.pfNewVar == null) goto errLabel;

            if (pQM.serviceGuiInfo.cfg.pfNewVar(pQM, ref pQM.serviceGuiInfo, (IntPtr)null) != 0) goto errLabel;
            if (pQM.serviceGuiInfo.pVar == null) goto errLabel;
            //	 			
            if (pQM.serviceGuiInfo.cfg.pfInitVar != null)
            {
                if (pQM.serviceGuiInfo.cfg.pfInitVar(pQM, ref pQM.serviceGuiInfo, (IntPtr)null) != 0)
                {
                    goto errLabel;
                }
            }
            pQM.serviceGuiInfo.ucbVarInited = true;



            /*
			# ifndef  __WINCE__
			//  
			if (initInputMgr(pQM->g_hInst, &pQM->inputMgr)) goto errLabel;
			pQM->bInputMgrInited = TRUE;
			#endif

			# ifdef  __SUPPORT_childFrmMenu__
			if (!(pQM->gui.hMenuChildFrm = LoadMenu(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDR_DOCTYPE_QYMC))))
			{
			# ifdef  __DEBUG__
				traceLog(_T("initQyMc failed, LoadMenu IDR_DOCTYPE_QYMC failed"));
			#endif
				goto errLabel;
			}

			//  
			if (!pQM->cfg.ucbAdvancedVer)
			{
				//  if  (  pQM->iSystemId  ==  CONST_qySystemId_netMc2  )  RemoveMenu(  pQM->gui.hSubMenuMcMgr,  8,  MF_BYPOSITION  );
			}

			QNM_cusRes_moduleMem* pResMem = getCurCusResModuleMem(&pQM->cusRes);
			if (!pResMem) goto errLabel;

			//  
			if (!(pItem = (QY_DMITEM*)qyGetDesByType(pResMem->pCusMenuTable, CONST_resId_childFrmMenu))) goto errLabel;
			if (makeDynaMenu(&pQM->cusRes, pQM->gui.hMenuChildFrm, pItem, getMenuStatus_frame, 0)) goto errLabel;   //  2005/11/06
			#endif
			//  2017/06/21


			//  2010/08/03
			G_guiData_qyMc* pGuiData = get_g_guiData();
			pGuiData->pCusRes = &pQM->cusRes;

			//
			if (gGuiData_init(pQM->env.usLangId, pGuiData)) goto errLabel;
			*/
            //



            iErr = 0;

        errLabel:

            //  if  (  pLckFile  )  delete  pLckFile;
            if (iErr != 0)
            {
                exitQyMc(pQyMc);
            }

            str = string.Format("initQyMc leaves, iErr is {0}", iErr);
            showInfo_open(0, null, str);

            //
            return iErr;
        }

        //
        public static int startToExitQyMc(CCtxQyMc pQyMc)
        {
            if (pQyMc == null) return -1;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            pQyMc.bQuit = true;

            //
            mySetEvent(pProcInfo.hEvt_notify_thread_isCli);

            //
            return 0;
        }


        public static unsafe void exitQyMc(CCtxQyMc pQyMc)
        {

            CCtxQyMc pQM = pQyMc;
            if (pQM == null)
            {
                Console.WriteLine("exitQyMc enters, pQM is null");

                return;
            }

            //
            traceLog("exitQyMc enters");
            showInfo_open(0, null, "exitQyMc enters");

            //
            pQM.bQuit = true;


            //
            if (pQM.bLogon) qyMcLogoff();

#if false
			//
			stopCusModules(pQM);

	
	//  2011/07/26
	G_guiData_qyMc* pGuiData = get_g_guiData();
	gGuiData_exit(pGuiData);

	//
	if (pQM->gui.hMenuChildFrm) { DestroyMenu(pQM->gui.hMenuChildFrm); pQM->gui.hMenuChildFrm = NULL; }

	//  2008/11/22
	if (pQM->bInputMgrInited) { exitInputMgr(&pQM->inputMgr); pQM->bInputMgrInited = FALSE; }
	//
#endif

            //



            //
            if (pQM.serviceGuiInfo.ucbVarInited)
            {
                if (pQM.serviceGuiInfo.cfg.pfExitVar != null)
                {
                    pQM.serviceGuiInfo.cfg.pfExitVar(pQM, ref pQM.serviceGuiInfo, (IntPtr)null);
                }
                pQM.serviceGuiInfo.ucbVarInited = false;
            }
            if (pQM.serviceGuiInfo.pVar != null)
            {
                if (pQM.serviceGuiInfo.cfg.pfFreeVar != null)
                {  //  2015/01/20
                    pQM.serviceGuiInfo.cfg.pfFreeVar(pQM, ref pQM.serviceGuiInfo, (IntPtr)null);
                }

                //
            }


            if (pQM.gui.ucbProcessQInited) qyFuncs.exitGenericQ(pQM.gui.processQ);   //  2007/06/01
            pQM.gui.processQ = null;

            //
            CQnmDb.exitDbPool();

            //
            if (pQM.p_g_dbFuncs != null) pQM.p_g_dbFuncs = null;


            //
            //if (pQM->ucbEncInited) { exitQnmEnc(&pQM->encCtx, NULL, NULL, NULL); pQM->ucbEncInited = FALSE; }

            //
            showInfo_open(0, null, _T("All exit."));

            //
            exitShowInfo(ref pQM.pShowInfoStruct);




            //  2015/07/08
            traceLog("exitQyMc leaves");

            return;
        }


        //
        static unsafe int getQyMcInitialCfg(CCtxQyMc pQyMc)
        {
            int iErr = -1;
            //QY_MC_CFG* pCfg = &pQyMc.cfg;

            //if (initQnmEnc(&pQyMc->encCtx, NULL, NULL, &pQyMc->commEncCtx)) goto errLabel;
            //pQyMc->ucbEncInited = TRUE;

            //pQyMc.cfg.bGetPolicyFromIni = true;

            //

            if (getDynCfg_isClient(pQyMc, ref pQyMc.cfg.dynCfg.cnt) != 0) goto errLabel;
            //



            //
            fixed (SOCK_TIMEOUT* pTo = &pQyMc.cfg.to)
            {
                setSockTimeout(pTo);
            }
            pQyMc.cfg.sockParam = new SOCK_PARAM();
            pQyMc.cfg.sockParam.capacity_buf_r = sizeof(MIS_MSGU);
            pQyMc.cfg.sockParam.capacity_buf_w = sizeof(MIS_MSGU);
            pQyMc.cfg.sockParam.to = pQyMc.cfg.to;


            //
            pQyMc.cfg.iTimeoutInMs_channelIdle_others = Consts.DEFAULT_iTimeoutInMs_channelIdle_others;
            //  pQyMc->cfg.iTimeoutInMs_channelIdle_recv_others  =  DEFAULT_iTimeoutInMs_channelIdle_recv_others;
            pQyMc.cfg.iTimeoutInMs_channelIdle_recv_talk = Consts.DEFAULT_iTimeoutInMs_channelIdle_recv_talk;

            //
            pQyMc.cfg.rwLockParam.uiMaxCnt_sema = Consts.CONST_uiInitCnt_sema_q2SyncFlg;
            pQyMc.cfg.rwLockParam.uiInitCnt_sema = pQyMc.cfg.rwLockParam.uiMaxCnt_sema - 1;
            pQyMc.cfg.rwLockParam.uiMilliSeconds_mutex_r = 10000;
            pQyMc.cfg.rwLockParam.uiMilliSeconds_sema_r = 10000;
            pQyMc.cfg.rwLockParam.uiMilliSeconds_mutex_w = 10000;
            pQyMc.cfg.rwLockParam.uiMilliSeconds_sema_w = 10000;


            //
            int index;
            QY_SERVICEGUI_CFG serviceClientCfg;

            //
            pQyMc.threadProcs = new PF_threadProc[Consts.QY_MC_TOTAL_DAEMON_THREADS];
            pQyMc.hDaemonThreads = new object[Consts.QY_MC_TOTAL_DAEMON_THREADS];

            //
            index = 0;
            pQyMc.threadProcs[index] = mcThreadProc; index++;

            {
                pQyMc.serviceGuiInfo.cfg.iServiceId = pQyMc.iServiceId;
                //  serviceClientCfg.iDbType  =  pQyMc->cfg.db.iDbType;

                //		 
                //serviceClientCfg.uiVarSize = getVarSize_is(pQyMc);
                pQyMc.serviceGuiInfo.cfg.pfInitVar = initVar_isGui;
                pQyMc.serviceGuiInfo.cfg.pfInit_afterLogon = init_afterLogon_isGui;
                pQyMc.serviceGuiInfo.cfg.pfExitVar = exitVar_isGui;

                //
                {
                    //  2015/01/20
                    if (pQyMc.pfNewVar == null) goto errLabel;

                    if (pQyMc.pfFreeVar == null) goto errLabel;
                    //
                    pQyMc.serviceGuiInfo.cfg.pfNewVar = pQyMc.pfNewVar;
                    pQyMc.serviceGuiInfo.cfg.pfFreeVar = pQyMc.pfFreeVar;
                }

                //
                //if  (  !isQmCmd(  pQyMc  )  )  
                {
                    pQyMc.threadProcs[index] = mcThreadProc_isCli; index++;
                }
            }
            if (index > pQyMc.threadProcs.Length)
            {
                //_T("threadProcs overflowed"));
                goto errLabel;
            }

            //

            iErr = 0;

        errLabel:


            return iErr;
        }

    }



}
