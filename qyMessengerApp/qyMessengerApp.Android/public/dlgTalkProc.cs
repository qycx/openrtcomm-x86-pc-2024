using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using qy;
using qyMessengerApp.Droid;



//
namespace qy.Droid
{
    public unsafe class PARAM_PROC_ROW
    {
        public int lRowIndex;
        //  unsigned  int					uiTranNo;
        public int iTaskId;
        public QY_MESSENGER_ID idInfo_talker;
        /*

TCHAR talker[256];
unsigned short usOp;                                                //  
int iStatus;
char timeBuf[CONST_qyTimeLen + 1];

TCHAR rowIdStr[CONST_maxRowIdStrLen + 1];               //  
														//
TCHAR content[CONST_maxCnt_content];                        //  2010/12/05
TCHAR txtContent[CONST_maxCnt_txtContent];              //  2010/12/05

//
PF_commonHandler pf;
int iCol;

long lRowIndex_who_gotByRowId;
//
bool bAddOnRowIndex;                                        //  2007/07/13
bool bFollowingRows;                                        //  
*/
        public int nItem;

        // 
        public bool bScrollIntoView;                                   //  
        public int lColor;                                                //  

        public bool bNewMsg;                                           //  2007/08/13

        public bool ucbNeedAsk;                                           //  cancelTasks(  )
        public bool ucbAsked;                                         //  
        public bool bAutoCancel;                                       //  2014/07/30

        /*
int iRetVal_id_messageBox;                              //  2016/02/03

//
long scrollTop_body, clientHeight_body;
long lRowIndex_top;                                     //  
long lRowIndex_bottom;
		*/

        public bool bMsgNeedRemoved;                                   //  2009/09/07

        //
        public uint nMsgTalks_processed_inProcessQ;                        //  2012/05/20

        //
        public object hListCtrl;                                         //  2017/06/25

        //  2017/08/08
        public bool ucbNoMe;

    }
    ;


    public unsafe struct CONF_mgr_info
    {
        public short usCnt;
        /*
		struct                         {
					 QY_MESSENGER_ID idInfo;
	}
	mems[20];
		*/
        public fixed UInt64 mems_idInfo_ui64Id[Consts.CONST_maxConfMgrMems];
    }
;


    //  2016/03/15
    //  2014/08/04
    public class TALKER_threadProcInfo : QY_qThreadProcInfo_common
    {


        //
        object hDlgTalk_mgr;

        object pMgrVarParam;

        //  2015/05/19
        //  2014/09/07
        /*
		struct                                                     {
		}
		status;
		*/

    }
        ;


    public struct DlgTalkVar_av_taskInfo_mosaicTrans
    {
        public bool bExists_confMosaic;
        //
        public uint uiTranNo_openAvDev_confMosaic_video;
        public uint uiTranNo_openAvDev_confMosaic_resource;

        //  2014/11/16
        //DLG_talk_mosaicTransStatus status;
    };

    public struct DlgTalkVar_av_taskInfo_confState
    {

        //
        public uint uiTranNo_retrieveAll;
        public ushort usSeqNo_retrievePart;

        //
        public uint dwTickCnt_sent;
        //
        public bool ucbNeed_sendAll;

    };


    //
    public struct DlgTalkVar_av_taskInfo
    {
        public bool bTaskExists;
        public bool bPrepare;
        public int iTaskId;
        //
        public QMC_mosaic_maker mosaicMaker;                                           //  2014/11/01					
                                                                                       //
        public DlgTalkVar_av_taskInfo_mosaicTrans mosaicTrans;

        //
        public QY_MESSENGER_ID idInfo_starter;
        public QY_MESSENGER_ID idInfo_initiator;
        public bool ucbStarter;                                       //  2010/08/31
                                                                      //  2010/08/31
        public bool ucbVideoConference;                               //  2010/08/31
                                                                      //
        public ushort usConfType;                                      //  2017/08/25

        //
        //INFO_consult_yz info_consult_yz;                                //  2017/08/27

        //
        public uint uiInitW;
        public uint uiInitH;

        public Int64 tStartTime_org;                                  //  
        public uint uiTranNo_org;                                  //
                                                                   //  unsigned  int									uiSerialNo_org;									//  
        public uint uiContentType_org;

        //  					
        public AV_stream_simple local_avStream;                                    //  2015/02/17public 

        //					
        public int iIndex_taskInfo;                                //  2014/09/25

        //					
        //public bool bEnableToSpeak;                                    //  2010/12/29

        //
        public bool bTaskInited;                                   //  2010/08/31

        //  
        public ushort usLayoutType_starter;                            //  2010/12/21

        //
        public uint dwLastTickCnt_confStarter_sendReq_toStopSending;              //  2014/10/09

        //  2017/07/03
        public bool ucbConfCtrl;

        //
        public ConfMem_confCtrl confMem_confCtrl;

        //  2017/09/17
        public DlgTalkVar_av_taskInfo_confState confState;


        //
        public bool bUse_backCamera;
        public bool bShow_localVideo;


        //
        public void memset0()
        {
#if false
			bTaskExists = false;
			iTaskId = 0;
#endif
            //
            this = new DlgTalkVar_av_taskInfo();

        }


    };





    //
    public struct DlgTalkVar_av
    {
        //
        int idc_peer, idc_other, idc_me, idc_bgWall_conf;                  //  2013/02/24
        object hDc_peer, hDc_other, hDc_me, hDc_bgWall_conf;                   //  2013/02/22
                                                                               //
        /*
																					//
				myZONE peerZone, otherZone, meZone;             //  2017/09/26
																//
																//
# ifdef  __DEBUG__
				//CAP_IMAGES		  bgWallImages;			//  2014/10/01
#endif
			SUB_V_WALL bgWall;          //  2017/09/25

				//  2012/05/05				
				unsigned short usCnt_zoneParams;
				ZONE_PARAM* pZoneParams;
				//
				ZONE_PARAM zoneParams_localOnPeer[CONST_maxCapImages];
				ZONE_PARAM zoneParam_meOnBgWall;
		*/

        //
        public object hDc_av_rect;               //  2009/12/28
        public int iIndex_sharedObj_localAv;               //  2009/12/28
        public int iIndex_usr_localAv;                     //  2010/09/06
        public bool ucbGot_iIndex_sharedObj_localAv;      //  2009/08/16


        //  2016/03/15
        public SYNC_mt_cnt syncMtCnt_taskInfo;


        //  2009/02/20

        public DlgTalkVar_av_taskInfo taskInfo;

        //
        public bool bNeedDoTaskAv;

        //
        public bool bNeedAvCall;
        //
        public bool bAutoCancel_lostVideo;
        public uint dwTickCnt_autoCancel_lostVideo;
        public bool bDoneOnce_autoCancel_lostVideo;

        //
        public bool m_bState_canSpeak;



        //
        public uint dwTickCnt_closeTaskAv;   //  2017/10/03

        //  2019/02/23
        object hWall;

        //
        //	2017/08/30			 
        public CONF_KEY videoConferenceStatus;



    };

    //
    public unsafe struct TALK_drawMem_param
    {
        public PLAYER_ID playerId;
    }

    public unsafe struct TALK_drawMem
    {
        public TALK_drawMem_param param;
        public uint tn_modified;
    }

    public unsafe struct TALK_drawLayout_param
    {
        public TALK_drawMem me;
        public TALK_drawMem peer;
    }

    //
    public unsafe struct TALK_drawLayout
    {
        public TALK_drawLayout_param param;
        //
        public uint tn_modified;
    };


    //
    public  class DLG_TALK_var
    {
        public int currentSelectQiuJiChannel;
        public long currentSelectQiuJiIdInfo;
        public bool is4k;
        public uint tickCountOnCreate;
        /*
		MACRO_objVarData_commonMems

				//  2015/11/12	
				int iIndex_talkerInfo;                      //  2015/11/12

		//
		HWND m_hParent;
		bool bIncludeACap;                      //  2013/03/19

		//				
		bool bInitFailed;               //  2007/09/04

		//				
		DWORD dwTickCnt_created;                        //  2014/02/04

		//			
		bool bUseDirectX;               //  2009/08/06		
		bool ucbUseHtmlEdit;               //  2012/05/17. 只有需要输入的窗口才使用HtmlEdit, 主窗口和墙上的小talk窗口都不需要使用
		bool ucbAutoClip;              //  2014/04/06
		bool ucbAutoClip_me;               //  2016/02/17


		//
# ifndef  __WINCE__
		void* pm_LargeImageList;            //  CImageList
		void* pm_SmallImageList;
#endif


		//
		MIS_CNT* pMisCnt;                   //  misServName

		*/
        public int loopCtrl_timer;             //  2009/10/18

        /*
		//
		int iTaskId_activated;          //  2007/08/11, iTaskId_activated Îª0Ê±²»²Ù×÷¡£²»Îª0Ê±£¬±íÃ÷Òª¼¤»îÕâ¸öÈÎÎñ¡£ 2007/08/11
		bool ucbTaskIdSet;             //  2007/08/11

		//
		unsigned short usMaxDisplayedTasks;     //  2007/06/26
		*/

        //
        public MSGR_ADDR addr;

        //
        public bool bActivityStarted_toDoTask;
        public ushort toDoTask_usOp;


        //
        public bool ucbGuest;                 //  			
                                              //
        public bool ucbTmpMsgr;                   //  2012/05/23. 是否临时msgr, 是的话主要为非同分组成员.但是有授权的vieeviewDynBmp服务.
                                                  //
        public string displayName;
        public string talkerDesc;
        //
        //WCHAR wLocation[64 + 1];        //  2007/12/23

        //
        public int iTalkerSubType;             //  2012/04/30

        //  		
        public object hFore;                     //  2007/09/09, 

        /*
		//
		GENERIC_Q tmpGrpMemQ;                   //  2008/01/22
		BOOL bTmpGrpMemQInited;
		//
		int iLvType_memList;            //  
		uint uiDefaultCmd_memList;      //  2004/04/17

		//
		MC_CTX_DISPLAYEDWND displayCtx;
		BOOL bDisplayCtxInited;
		*/

        //	
        //GENERIC_Q* m_pProcessQ;                 //  
        public List<TableItem_dlgTalk_msg> m_pProcessQ;

        /*
		//  2017/07/01
		int idc_talkerList;
		QY_COLUMNINFO* pColumns_talkers;
		//
		struct                                                     {
					 int iSelectItem;
	}
	talkerList;

				 //
				 int idcList_members;
	QY_COLUMNINFO* pColumns_members;
	struct                                                     {
					 int iSelectItem;
}
listMembers;


//
int iRowId_internal;                                    //  

int iCol_content;                           //  2007/07/02
											//
int nSubItem_status;                        //  2007/07/17

//  
long lColor_org;
long lColor_active;
TCHAR rowIdStr_active[64 + 1];
//
BOOL bCellActivated;

//
HMENU hMenu;                                    //  2012/04/09

		*/
        //
        public object pCapStuff;                                //  2009/08/19

        /*
		 //
int iMenuId_selectedAudio;                  //  2011/10/25
int iMenuId_selectedVideo;

//  2011/12/25
struct                                                     {
					 struct                                                 {
						 TCHAR name_aDev[128];
TCHAR name_vDev[128];			
					 }														recentDevs[1];
				 }															shareMediaDevice;
*/
        //  2012/05/20
        public uint nMsgTalks_processed_inProcessQ;

        //  2013/02/26
        public DLG_talk_layout m_layout;
        /*
DLG_scroll_info m_scrollInfo;

//  2011/10/02			
DLG_layout_param m_layoutParam;
//
WINDOWPLACEMENT m_oldWndPlacement;
struct                                                     {
					 HMENU hOldMenu;                                //  2012/07/07
long lOldStyle;								//  2012/07/07
				 }															m_fullScreenInfo;
//
BOOL m_bFullScreen;
RECT m_fullScreenRect;
DLG_layout_param m_oldLayoutParam;
//
BOOL bDone_endFullscreen;                   //  此标志位在点击一次endFullscreen设置, 2011/10/16

//  2007/12/22
uint uiTimerId;

//  2012/04/03
bool ucbOnGlobalVWall;                     //  是否在GlobalVWall上
		*/

        //

        public DlgTalkVar_av av;

        /*


//				
struct                                                     {
							 //
QY_MESSENGER_ID idInfo_sel;							 
					
				 }															confMgr_talkerListInfo;

//					
struct                                                     {
							 //
QY_MESSENGER_ID idInfo_sel;
//
QY_MESSENGER_ID idInfo_talkTo;					
				 }															confMgr_grpMemListInfo;


//  2017/09/07
struct                                                     {
					 //
GrpMemList_status grpMemListStatus;
//
GrpMem_status_indexes indexes;
					 //
				 }															grpMemList;
		*/

        //				 
        //  2016/03/15
        public TALKER_threadProcInfo talkerThreadInfo;
        /*

//  2015/11/15			
D3D_talkerData_cfg d3d_talkerData_cfg;
		*/
        //  2009/02/22
        //public DLG_TALK_videoConference videoConference;
        /*
//  2015/08/05
//  2014/12/09
struct                                                     {
					BOOL bTaskExists;
//
//
BOOL bInProgress_autoAnswer;			
				 }															autoAnswer;
//  2015/08/08
struct                                                     {
					 //  2014/12/08
BOOL bNeedChkRecvdTasks;
				 }															needChkTasks;

//
void* pShadowInfo;                          //  2012/04/24

//
HWND hWnd_instantAssistant;                 //  2009/02/18

//  2014/01/24
struct                                                     {
					 RECT rc;               //  相对窗口的坐标
HWND hWnd_msgTool;
//
DWORD dwModifiedTickCnt_showMsgTalk;			//  2016/03/25
					 //
				 }															msgTool;

//
struct                                                     {
					 PTZ_control_mem cfg;
TCHAR tHint[255 + 1];
//
HWND hWnd_ptzControl;						//  2012/08/01
				 }															ptzControl;

//
BOOL bSwapMouse;                                //  2008/11/16
												//  
struct                                                     {                                        //  2017/08/23					 
					 //  
DWORD dwTickCnt_sendLastMouseMove;     //  200811/22
															//
struct                                                 {
						 int x;
int y;
UINT nFlags;
BOOL bMouseMove;		
					 }														lastMouseEvent;
BOOL bNeedSend_lastMouseEvent;			//  是否需要发送

				 }															remoteAssist;

//
struct                                                     {
					 HWND hWnd_subtitles;							//  2013/04/05
				 }															subtitles;

//  2015/08/01
struct                                                     {
					 HWND hTool_offlineRes;
				 }															offlineRes;

//  2014/12/23
struct                                                     {
					 TCHAR tHint[128];
				 }															promptToClose;

//	
CAP_subWnds subWnds;

//
TCHAR tHint[256];
MIS_MSGU* pMsgBuf_doWnd_guiMsgArrive;               //  2009/12/08
		*/

        //
        public DLG_guiData_talk guiData;                               //  2010/07/14


        //  2012/05/18
        public uint dwTickCnt_start_noShadow;

        //
        public bool bCancelTasks;                          //  2011/02/03

        //  2017/09/23
        public bool bShouldQuit;


        //
        public DLG_syncFlgs syncFlgs;

        //
        public TALK_drawLayout m_drawLayout;

        //
        public object m_share_eglContext;

        //
        /*
        public Java.Lang.Object[] tmp_hTools_dlgTalk_av = new Java.Lang.Object[3];
        //
        public Java.Lang.Object[] tmp_share_eglContexts = new Java.Lang.Object[3];
        */
       
        public Java.Lang.Object tmp_share_eglContext = null;


        //
        public uint dwTickCnt_waitForOk_dlgTalk_av1 = 0;

        //
        //public bool m_tool_bOrientation_landscape;

        //
        public bool m_bNeedResumeTaskAv;


        //
        public DLG_TALK_var()
        {
            this.syncFlgs.lCnt_getDlgVar = new int[1];
            //
            //int ii  =  tmp_hTools_dlgTalk_av.GetLength(0);
            //ii++;

        }

    };

    partial class qyFuncs_Droid
    {

        //
        public static unsafe int dlgTalk_OnInitDialog(object hDlgTalk, ref DLG_TALK_var m_var)
        {
            // TODO: Add extra initialization here
            int iErr = -1;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            int idc;

            object hWnd = null;
            if (0 == findTalker(pQyMc, pMisCnt, m_var.addr.idInfo, ref hWnd))
            {
                qyFuncs.showInfo_open(0, "", "dlgTalk_OnInitDialog failed, talker already exists");
                goto errLabel;
            }
            //  2013/07/22. 移到函数起始处
            {
                MIS_MSG_TALKINGFRIEND_QMC pMsg = new MIS_MSG_TALKINGFRIEND_QMC();

                //  ´ÓÕâÀïÆð£¬m_hWnd²ÅÓÐÐ§
                //memset(pMsg, 0, sizeof(pMsg[0]));//
                pMsg.uiType = Consts.CONST_misMsgType_talkingFriend_qmc;
                //memcpy(&pMsg->addr, &m_var.addr, sizeof(pMsg->addr)  );
                pMsg.addr = m_var.addr;
                //
                pMsg.hWnd = hDlgTalk;
                //if (  ::qPostMsg(pMsg, sizeof(pMsg[0]), &m_var.pMisCnt->talkingFriendQ)) goto errLabel;
                pMisCnt.talkingFriendQ.Add(pMsg);

            }


            //
            dlgTalk_loadRecentMsgs(hDlgTalk, ref m_var);


            //
            iErr = 0;
        errLabel:
            return iErr;
        }

        //
        public static unsafe int tmpHandler_loadRecentMsgs(object p0, object p1, object p2)
        {
            object hDlgTalk = p0;
            //p1
            IM_MSG_RCD* pRcd = (IM_MSG_RCD*)(IntPtr)p2;
            //p2

            //CHelp_getDlgTalkVar getVar = new CHelp_getDlgTalkVar();
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            DLG_TALK_var m_var = dlgTalk.m_var;
            //getVar.getVar(hDlgTalk, "");
            if (m_var == null) return -1;

            if (pRcd->idInfo_logicalPeer.ui64Id == m_var.addr.idInfo.ui64Id)
            {
                GuiShare.pf_talk_addNewMsg(hDlgTalk, pRcd->idInfo_send, pRcd->tSendTime, pRcd->uiTranNo, 0, pRcd->uiType, new string(pRcd->logMsgContent), pRcd->iTaskId);
            }


            //
            return 0;
        }


        //
        public static unsafe int dlgTalk_loadRecentMsgs(object hDlgTalk, ref DLG_TALK_var m_var)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CMyDb myDb = (CMyDb)pQyMc.gui.pDb;

            qyFuncs.qTraverse(myDb.m_var_pQ_qyImMsgTab, tmpHandler_loadRecentMsgs, hDlgTalk, 0);


            return 0;
        }



        //
        public static unsafe int dlgTalk_OnDestroy(object hDlgTalk, ref DLG_TALK_var m_var)
        {
            int iErr = -1;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            //
            removeTalker(pQyMc, hDlgTalk, 0);



            //
            iErr = 0;
        errLabel:
            return iErr;

        }

        //
        public static bool isTalkerShadowMgr(ref MSGR_ADDR addr)
        {
            return (0 == (addr).uiTranNo_shadow);
        }

        //  2012/05/14
        public static bool isInvisibleMgr(DLG_TALK_var m_var)
        {
            return (isTalkerShadowMgr(ref (m_var).addr) && (m_var).guiData.param.bNeedNotShowWnd);
        }


        //
        public static int dlgTalk_closeTaskAv(object hDlgTalk, DLG_TALK_var m_var, string hint)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
            bool bDbg = false;
            string str;

            //
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;


#if DEBUG
            bDbg = true;
#endif
            //
            if (null == hint) hint = "";

            //
            if (bDbg)
            {
                str = string.Format("dlgTalk_closeTaskAv: {0} enters, {1}", m_var.addr.idInfo.ui64Id, hint);
                qyFuncs.showInfo_open(0, null, str);
            }

            try
            {


                //
                if (isTalkerShadowMgr(ref m_var.addr))
                {
                    if (m_var.av.taskInfo.bTaskExists)
                    {
                        m_var.av.taskInfo.bPrepare = true;
                    }
                }


                //  2016/03/15
                exitTalkerThread(pProcInfo, m_var.talkerThreadInfo);

                //  2009/08/16
                if (m_var.av.ucbGot_iIndex_sharedObj_localAv)
                {

                    //  2017/09/11
                    if (isTalkerShadowMgr(ref m_var.addr))
                    {
                        get_conf_stat(hDlgTalk);
                    }

                    //
                    if (bDbg)
                    {
                        qyFuncs.traceLog("dlgTalk_closeTaskAv: after get_conf_stat");
                    }

                    //
                    //  2016/03/28
                    int iIndex_sharedObj = m_var.av.iIndex_sharedObj_localAv;
                    if (qyFuncs.bExists_slave(pProcInfo, iIndex_sharedObj))
                    {
                        QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
                        if (pSharedObj != null)
                        {
                            //
                            if (bDbg)
                            {
                                qyFuncs.traceLog("dlgTalk_closeTaskAv: before freeSharedObjUsr for slave");
                            }

                            //
                            int iIndex_sharedObj_slave;
                            iIndex_sharedObj_slave = pSharedObj.var.master.slaveKey.iIndex_sharedObj;
                            //
                            qyFuncs.freeSharedObjUsr(pProcInfo, iIndex_sharedObj_slave, hDlgTalk, Consts.CONST_qyWndContentType_talker);
                        }
                    }
                    //
                    if (bDbg)
                    {
                        qyFuncs.traceLog("dlgTalk_closeTaskAv: before freeSharedObjUsr_localAv");
                    }


                    //  askSharedObjToStop(  pProcInfo,  getSharedObjByIndex(  pProcInfo,  m_var.av.iIndex_sharedObj_localAv  ),  pQyMc->gui.hMainWnd  );
                    qyFuncs.freeSharedObjUsr_localAv(pProcInfo, m_var.av.iIndex_sharedObj_localAv, hDlgTalk, Consts.CONST_qyWndContentType_talker);
                    m_var.av.ucbGot_iIndex_sharedObj_localAv = false;

                    //
                    if (bDbg)
                    {
                        qyFuncs.traceLog("dlgTalk_closeTaskAv: after freeSharedObjUsr_localAv");
                    }

                }
                //
                //freeSharedObjUsr(  pProcInfo,  


                //  2011/03/14

                object hMgr = null;
                hMgr = hDlgTalk;
                /*
                CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;
                //DLG_TALK_var  *		pMgrVar		=  NULL;		
                if (isTalkerShadowMgr(m_var.addr))
                {
                    hMgr = hDlgTalk;
                    //pMgrVar  =  &m_var;
                }
                else
                {
                    TALKER_shadow* pShadow = (TALKER_shadow*)m_var.pShadowInfo;
                    if (pShadow)
                    {
                        hMgr = pShadow->hMgr;
                        //pMgrVar  =  (  DLG_TALK_var	*	)help_getDlgTalkVar_mgr.getVar(  pShadow->hMgr  );
                        //if  (  !pMgrVar  )  goto  errLabel;
                    }
                }
                */


                //
                //closeInstantAssistant(hDlgTalk);

                //  2014/11/01
                if (isTalkerShadowMgr(ref m_var.addr))
                {
                    if (m_var.av.taskInfo.bTaskExists)
                    {
                        //
                        qyFuncs.stopMosaicThreads(ref m_var.av.taskInfo.mosaicMaker);
                        //
                        qyFuncs.shareDynBmp_closeTaskAv(ref m_var.av.taskInfo.mosaicMaker.video);
                        qyFuncs.shareDynBmp_closeTaskAv(ref m_var.av.taskInfo.mosaicMaker.resource);
                        //
                        qyFuncs.removeTask(mainActivity.var_common.ctxCaller, m_var.av.taskInfo.iTaskId);
                        //
                        qyFuncs.qmcTaskInfo_setClosed(mainActivity.var_common.ctxCaller, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "dlgTalk_closeTaskAv");
                    }
                }

                //

                //
                //memset(&m_var.av.taskInfo, 0, sizeof(m_var.av.taskInfo));
                m_var.av.taskInfo.memset0();
                //  2017/10/03
                m_var.av.dwTickCnt_closeTaskAv = qyFuncs.myGetTickCount(null);


                //  2017/09/06
                qyFuncs.resetTalkerList_mgr(hDlgTalk);

                //  2017/10/02
                if (isTalkerShadowMgr(ref m_var.addr))
                {
                    /*
                    TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)m_var.pShadowInfo;
                    if (pShadowMgr)
                    {
                        if (IsWindow(pShadowMgr->secondView.hWnd_dlgVideos))
                        {
                            QY_WMBUF_COMM wmBuf;
                            memset(&wmBuf, 0, sizeof(wmBuf));
                            wmBuf.uiType = CONST_qyWmParam_freeCapImages;
                            SendMessageTimeout(pShadowMgr->secondView.hWnd_dlgVideos, CONST_qyWm_comm, 0, (LPARAM) & wmBuf, 0, 1000, NULL);
                        }
                    }
                    */
                }


                //  2010/09/03
                //  InvalidateRect(  hDlgTalk,  NULL,  true  );
                GuiShare.pf_PostMessage(hMgr, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_curSharedObjUsrChanged_mgr, 0);


            }
            catch (Exception e)
            {
                qyFuncs.showInfo_open(0, "", "dlgTalk_closeTaskAv exception");
            }

            //  2011/04/10
            /*
			if (m_var.addr.idInfo.ui64Id == pProcInfo.customerService.called.idInfo.ui64Id)
			{
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("dlgTalk, customer service, no av exists, closed automatically"));
				PostMessage(hDlgTalk, WM_CLOSE, 0, 0);
			}
			*/

            if (bDbg)
            {
                qyFuncs.showInfo_open(0, null, "dlgTalk_closeTaskAv leaves");
            }


            return 0;
        }

        //
        public static int dlgTalk_init_respInfo(object hDlgTalk_mgr, DLG_TALK_var pMgrVar, bool ucbVideoConference)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            if (!isTalkerShadowMgr(ref pMgrVar.addr)) return -1;
            if (!pMgrVar.av.ucbGot_iIndex_sharedObj_localAv) return -1;
            QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, pMgrVar.av.iIndex_sharedObj_localAv);
            if (null == pSharedObj) return -1;
            SHARED_OBJ_USR pSharedObjUsr = qyFuncs.getSharedObjUsr(pSharedObj, pMgrVar.av.iIndex_usr_localAv);
            if (null == pSharedObjUsr) return -1;
            /*
			//
			TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)pMgrVar.pShadowInfo;

			//  2017/09/06	
			TASK_respInfo* pRespInfo = &pSharedObjUsr->taskRespInfo;
			//
			memset(pRespInfo, 0, sizeof(pRespInfo[0]));
			//
			if (ucbVideoConference)
			{
				//
				unsigned  int tmp_usCnt = 0;
				//
				int i;
				for (i = 0; i < pShadowMgr->confCtrl.grpConfCtrl.usCnt; i++)
				{
					GrpMem_confCtrl* pMem = &pShadowMgr->confCtrl.grpConfCtrl.pMems[i];
					//  traceLog(  _T(  "mem[%d] %I64u"  ),  i,  pMem->idInfo.ui64Id  );
					//
					if (!pMem->idInfo.ui64Id) continue;
					//
					if (tmp_usCnt >= mycountof(pRespInfo->mems)) break;
					pRespInfo->mems[tmp_usCnt].idInfo.ui64Id = pMem->idInfo.ui64Id;
					tmp_usCnt++;

					continue;
				}
				//
				pRespInfo->usCnt = tmp_usCnt;

				//
# ifdef  __DEBUG__
				print_TASK_respInfo(pRespInfo, _T("before qsort"));
#endif

				//
				qsort(pRespInfo->mems, tmp_usCnt, sizeof(TASK_respInfo_mem), qCmp_taspRespInfoMem);

				//
# ifdef  __DEBUG__
				print_TASK_respInfo(pRespInfo, _T("after qsort"));
#endif


			}
			*/

            return 0;

        }


        //
        public static unsafe int tmpHandler_cancelTask(object hDlgTalk, object p1, object p2)
        {

            int iErr = -1;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            //MIS_MSGU* pMsgElem = (MIS_MSGU*)p2;
            TableItem_dlgTalk_msg pMsgElem = (TableItem_dlgTalk_msg)p2;

            //  2012/05/09
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

          //  using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            {
                ;
                DLG_TALK_var pm_var = dlgTalk.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
                if (null == pm_var) return -1;

                object hMgr = hDlgTalk;
                //CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;
                DLG_TALK_var pMgrVar = pm_var;
                /*
				if (!isTalkerShadowMgr(pm_var->addr))
				{
					TALKER_shadow* pShadow = (TALKER_shadow*)pm_var->pShadowInfo;
					if (!pShadow) return -1;
					hMgr = pShadow->hMgr;
					pMgrVar = (DLG_TALK_var*)help_getDlgTalkVar_mgr.getVar(hMgr);
					if (!pMgrVar) return -1;
				}
				*/

                //
                //  DLG_TALK_var	&	m_var		=	*getDlgTalkVar(  hDlg  );
                //
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
                //TCHAR tBuf[256] = _T("");
                //char buf[256] = "";
                CQyMalloc mallocObj;

                if (null == hDlgTalk || null == pParam || null == pMsgElem) return -1;
                QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
                if (null == pDbFuncs) goto errLabel;
                QM_dbFuncs g_dbFuncs = pDbFuncs;
                Int64 tStartTran;
                uint uiTranNo;
                QY_MESSENGER_ID idInfo_dst; idInfo_dst.ui64Id = 0;
                QY_MESSENGER_ID idInfo_logicalPeer; idInfo_logicalPeer.ui64Id = 0;

                //
                //
                int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(mainActivity.var_common.ctxCaller, pProcInfo, pMsgElem.iTaskId);
                QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "tmpHandler_cancelTask");
                if (pTaskInfo == null)
                {
                    iErr = 0; goto errLabel;
                }
                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                if (pTaskData == null)
                {
                    iErr = 0; goto errLabel;
                }

                //if (pMsgElem->uiType == CONST_misMsgType_task)
                //MIS_MSG_TASK* pMsg = &pMsgElem->task;
                //
                fixed (MIS_MSG_TASK* pMsg = &pTaskData.msgU.task)
                {
                    if (pMsg->uiType != Consts.CONST_misMsgType_task) goto errLabel;

                    IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->data.buf;
                    int iStatus;
                    int lenInBytes;

                    if (pParam.iTaskId != 0)
                    {
                        if (pMsg->iTaskId != pParam.iTaskId)
                        {
                            iErr = 0; goto errLabel;
                        }
                    }

                    object pDb = null;
                    using (CQnmDb db = new CQnmDb())
                    {
                        ;
                        if (null == db.getAvailableDb(pQyMc.iDsnIndex_mainSys)) goto errLabel;
                        pDb = db.m_pDbMem.pDb;

                        switch (pContent->uiType)
                        {
                            case Consts.CONST_imCommType_transferAvInfo:
                            case Consts.CONST_imCommType_transferFileReq:

                                //
                                switch (pMsg->iStatus)
                                {
                                    case Consts.CONST_imTaskStatus_req:
                                    case Consts.CONST_imTaskStatus_applyToSend:
                                    case Consts.CONST_imTaskStatus_waitToSend:
                                    case Consts.CONST_imTaskStatus_acceptedByReceiver:
                                    case Consts.CONST_imTaskStatus_dualByReceiver:
                                        {

#if false  //  ndef  __APP_qyMc_touchscreen__
											 if  (  pParam->ucbNeedAsk  &&  !pParam->ucbAsked  )  {					
												 G_guiData_qyMc	*	pGuiData	=	get_g_guiData(  );
												 PF_MessageBox	pf  =  (  PF_MessageBox  )pGuiData->pf_MessageBox;
												 MACRO_qyAssert(  pf,  _T(  "pf_MessageBox is null"  )  );
												 int  tmpiRet  =  pf(  hDlg,  getResStr(  0,  &pQyMc->cusRes,  CONST_resId_ifExitSession  ),  _T(  "qycx.com"  ),  MB_YESNOCANCEL  );
												 //
												 pParam->iRetVal_id_messageBox  =  tmpiRet;
												 //
												 if  (  tmpiRet  ==  IDCANCEL  )  goto  errLabel;
												 //
												 if  (  tmpiRet  ==  IDNO  )  {
													 iErr  =  1;  goto  errLabel;
												 }
												 //
												 pParam->ucbAsked  =  true;
											 }
#endif

                                            iStatus = Consts.CONST_imTaskStatus_canceledBySender;
                                            g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsg->iTaskId);
                                            pMsg->iStatus = iStatus;        //  2012/05/15

                                            //  °ÑÏÔÊ¾×´Ì¬ÐÞ¸ÄÏÂ.2008/11/14
                                            //qyFuncs.showTaskStatus(pMisCnt, pMgrVar.addr.idInfo, pMsg.idInfo_taskSender, &pMsg->idInfo_taskReceiver, false, pContent->uiType, pMsg->iTaskId, iStatus, 0, 0, _T(""), _T(""));

                                            //  Í¨Öª¶Ô·½
                                            TASK_PROC_REQ taskProcReq;
                                            qyFuncs.mymemset((IntPtr)(byte*)&taskProcReq, 0, sizeof(TASK_PROC_REQ));
                                            taskProcReq.uiType = Consts.CONST_imCommType_taskProcReq;
                                            taskProcReq.usOp = Consts.CONST_imOp_send_cancel;
                                            taskProcReq.tStartTime_org = pMsg->tStartTime;
                                            taskProcReq.uiTranNo_org = pMsg->uiTranNo;
                                            taskProcReq.uiContentType_org = pContent->uiType;
                                            //
                                            lenInBytes = sizeof(TASK_PROC_REQ);
                                            //					  
                                            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
                                            //  2015/09/08
                                            uint uiChannelType = Consts.CONST_channelType_talking;// pMsg->uiChannelType;
                                                                                                  //
                                            uiChannelType = 0;
                                            idInfo_dst.ui64Id = pMgrVar.addr.idInfo.ui64Id;
                                            idInfo_logicalPeer.ui64Id = pMgrVar.addr.idInfo.ui64Id;
                                            //
                                            //if (postMsgTask2Mgr_mc(pMisCnt, Consts.CONST_misMsgType_task, 0, pMsg->usCode, tStartTran, uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (char*)&taskProcReq, lenInBytes, &pMgrVar.addr.idInfo, &pMsg->idInfo_taskSender, &pMsg->idInfo_taskReceiver, &pMsg->idInfo_taskReceiver, uiChannelType, NULL, false)) goto errLabel;
                                            if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, pMsg->usCode, tStartTran, uiTranNo, 0, pMsg->iTaskId, pMsg->uiTaskType, (byte*)&taskProcReq, (uint)lenInBytes, idInfo_logicalPeer.ui64Id, 0, 0, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;
                                        }
                                        break;
                                    case Consts.CONST_imTaskStatus_resp:
                                    case Consts.CONST_imTaskStatus_applyToRecv:
                                    case Consts.CONST_imTaskStatus_waitToRecv:
                                        {

#if false  //  ndef  __APP_qyMc_touchscreen__
											 if  (  pParam->ucbNeedAsk  &&  !pParam->ucbAsked  )  {
												 //  if  (  myMessageBox(  hDlg,  getResStr(  0,  &pQyMc->cusRes,  CONST_resId_ifExitSession  ),  _T(  "qycx.com"  ),  MB_YESNO  |  MB_DEFBUTTON2  )  !=  IDYES  )  goto  errLabel;
												 //  2016/02/12
												 int  tmpiRet  =  myMessageBox(  hDlg,  getResStr(  0,  &pQyMc->cusRes,  CONST_resId_ifExitSession  ),  _T(  "qycx.com"  ),  MB_YESNOCANCEL  );
												 //
												 if  (  tmpiRet  ==  IDCANCEL  )  goto  errLabel;
												 //
												 if  (  tmpiRet  ==  IDNO  )  {
													 iErr  =  1;   goto  errLabel;
												 }
												 //
												 pParam->ucbAsked  =  true;
											 }
#endif

                                            iStatus = pParam.bAutoCancel ? Consts.CONST_imTaskStatus_autoCanceledByReceiver : Consts.CONST_imTaskStatus_canceledByReceiver;
                                            qyFuncs.traceLog("##1205" + iStatus);

                                            g_dbFuncs.pf_updateTaskStatus(pDb, iStatus, pMsg->iTaskId);
                                            pMsg->iStatus = iStatus;        //  2012/05/15

                                            //  °ÑÏÔÊ¾×´Ì¬ÐÞ¸ÄÏÂ.2008/11/14
                                            //showTaskStatus(pMisCnt, &pMgrVar.addr.idInfo, &pMsg->idInfo_taskSender, &pMsg->idInfo_taskReceiver, false, pContent->uiType, pMsg->iTaskId, iStatus, 0, 0, _T(""), _T(""));

                                            //  Í¨Öª¶Ô·½
                                            TASK_PROC_REQ taskProcReq;
                                            qyFuncs.mymemset((IntPtr)(byte*)&taskProcReq, 0, sizeof(TASK_PROC_REQ));
                                            taskProcReq.uiType = Consts.CONST_imCommType_taskProcReq;
                                            taskProcReq.usOp = Consts.CONST_imOp_recv_cancel;
                                            taskProcReq.tStartTime_org = pMsg->tStartTime;
                                            taskProcReq.uiTranNo_org = pMsg->uiTranNo;
                                            taskProcReq.uiContentType_org = pContent->uiType;// pContent->uiType;
                                                                                             //
                                            lenInBytes = sizeof(TASK_PROC_REQ);
                                            //					  
                                            qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
                                            //  2015/09/08
                                            uint uiChannelType = 0;// Consts.pMsg->uiChannelType;
                                                                   //
                                            uiChannelType = 0;
                                            idInfo_dst.ui64Id = pMsg->idInfo_taskSender.ui64Id;
                                            idInfo_logicalPeer.ui64Id = pMgrVar.addr.idInfo.ui64Id;

                                            //	
                                            if (0 != qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_task, 0, pMsg->usCode, tStartTran, uiTranNo, 0, pMsg->iTaskId, 0, (byte*)&taskProcReq, (uint)lenInBytes, idInfo_logicalPeer.ui64Id, 0, 0, idInfo_dst.ui64Id, uiChannelType, null, false)) goto errLabel;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                //
                                if (pMgrVar.av.taskInfo.bTaskExists
                                    && pMgrVar.av.taskInfo.iTaskId == pMsg->iTaskId)
                                {
                                    dlgTalk_closeTaskAv(hMgr, pMgrVar, "tmpHandler_cancelTask");
                                }
                                //
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            iErr = 0;

        errLabel:

            return iErr;
        }



        //
        public static unsafe int dlgTalk_cancelTasks(object hCurTalk, DLG_TALK_var pCurVar, int iTaskId, bool bAutoCancel)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //
            //char idStr[CONST_qyMessengerIdStrLen + 1] = "";
            //char idStr_me[CONST_qyMessengerIdStrLen + 1] = "";
            int tmpiRet = -1;
            //  CListCtrl		*	pListCtrl									=	(  CListCtrl  *  )CWnd::FromHandle(  GetDlgItem(  hDlg,  m_var.idcList_task  )  );
            PARAM_PROC_ROW param;
            CQySyncObj syncObj;

            if (null == pCurVar) return -1;

            //
            object hMgr = hCurTalk;
          //  CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;
            DLG_TALK_var pMgrVar = pCurVar;

            /*
			if (!isTalkerShadowMgr(ref pCurVar.addr))
			{
				TALKER_shadow* pShadow = (TALKER_shadow*)pCurVar->pShadowInfo;
				if (!pShadow) return -1;
				hMgr = pShadow->hMgr;
				pMgrVar = (DLG_TALK_var*)help_getDlgTalkVar_mgr.getVar(hMgr);
				if (!pMgrVar) return -1;
			}
			*/

            //
            //  DLG_TALK_var  &  m_var  =  *pm_var;

            /*
			if (!pMgrVar.m_pProcessQ) return -1;
			GENERIC_Q & m_processQ = *pMgrVar.m_pProcessQ;
			*/

            //  2011/02/03
            if (pMgrVar.bCancelTasks) return -1;
            pMgrVar.bCancelTasks = true;

            //
            //idInfo2Str(&pMgrVar.addr.idInfo, idStr, mycountof(idStr));
            //idInfo2Str(&pMgrVar.pMisCnt->idInfo, idStr_me, mycountof(idStr_me));

#if __DEBUG__
			traceLogA("cancelTasks: usMaxDisplayedTasks %d", pMgrVar.usMaxDisplayedTasks);
#endif

            //if (syncObj.sync(CQyString(pMgrVar.pMisCnt->cfg.mutexNamePrefix_syncTask) + idStr)) goto errLabel;

            //memset(&param, 0, sizeof(param));
            param = new PARAM_PROC_ROW();
            if (!pQyMc.bQuit) param.ucbNeedAsk = true;

            //  2013/09/08
            /*
			if (!IsWindowVisible(hCurTalk))
			{
				param.ucbNeedAsk = false;
			}
			*/
            //  2015/08/04
            bool bNeedClose = false;
            /*
			if (pProcInfo.cfg.policy.ucbRunAsAdvertisingPlayer)
			{
				//
				bNeedClose = false;
				if (!dlgTalk_chkAsAdvertisingPlayer(hMgr, pMgrVar, &bNeedClose, NULL, 0))
				{
					if (bNeedClose)
					{
						param.ucbNeedAsk = false;
					}
				}
			}
			if (bSupported_fileServer(pQyMc))
			{
				//
				bNeedClose = false;
				if (!dlgTalk_chkFileServer(hMgr, pMgrVar, &bNeedClose, NULL, 0))
				{
					if (bNeedClose)
					{
						param.ucbNeedAsk = false;
					}
				}
			}
			if (pQyMc->iCustomId == CONST_qyCustomId_bjyz)
			{  //  2017/08/27
				bNeedClose = false;
				if (!dlgTalk_chk_yz(hCurTalk, pCurVar, &bNeedClose, NULL, 0))
				{
					if (bNeedClose)
					{
						param.ucbNeedAsk = false;
					}
				}
			}
			*/

            //
            if (0 != iTaskId)
            {
                param.iTaskId = iTaskId;
                param.ucbNeedAsk = false;           //  
                param.bAutoCancel = bAutoCancel;    //  2014/07/30
            }
            //tmpiRet = qTraverse(&m_processQ, (PF_commonHandler)tmpHandler_cancelTask, hCurTalk, &param);
            foreach (var item in pMgrVar.m_pProcessQ)
            {
                tmpiRet = tmpHandler_cancelTask(hMgr, param, item);
                if (tmpiRet < 0) goto errLabel;

            }

            iErr = 0;
        errLabel:

            pMgrVar.bCancelTasks = false;  //  2011/02/03

            return iErr;
        }

        public static bool bReady_toShowMsg(object hCurTalk)
        {
            bool bRet = false;
            // 
            bRet = true;

        errLabel:

            return bRet;

        }

        public static int activateWnd(object hDlgTalk, DLG_TALK_var m_var)
        {
            return 0;
        }



        public static unsafe int tmpHandler_showMsg_talk(object hDlgTalkParam, DLG_TALK_var m_var, object p1, IntPtr pMsgParam)
        {
            int iErr = -1;

            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            MIS_MSG_TALK* pMsg = (MIS_MSG_TALK*)pMsgParam;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
            if (null == pProcInfo) return -1;
            FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
            if (null == pFuncs) return -1;

            //TCHAR txtContent[1024] = _T("");
            //TCHAR senderDesc[255 + 1] = _T("");
            //TCHAR receiverDesc[255 + 1] = _T("");
            Int64 t;

            int tmpiRet = 0;
            //
            //TCHAR tBuf[1024] = _T("");
            int i;
            //

            if (null == hDlgTalk || null == pParam || null == pMsg) return -1;






            MIS_MSG_routeTalkData* pTalkData = &pMsg->data;
            IM_CONTENTU* pContent = (IM_CONTENTU*)&pTalkData->buf;
            //char timeBuf[CONST_qyTimeLen + 1] = "";
            //TCHAR displayContent[1024] = _T("");

            activateWnd(hDlgTalk, m_var);       //  ÕâÀïÒªÉÁÁÁ¶Ô»°¿ò¡£ÌáÐÑÓÃ»§µÄ×¢Òâ
                                                //  m_var.ucbNewMsgArrives  =  true;
            dlgTalk_set_newMsgArrives(hDlgTalk, m_var, true);
            //  2016/03/25
            //m_var.msgTool.dwModifiedTickCnt_showMsgTalk = myGetTickCount(&pQyMc->gui.ctx_gui_thread);

            //


            //
            switch (pContent->uiType)
            {
                case Consts.CONST_imCommType_htmlContent:

                    /*
					//  2014/07/12
					if (m_var.addr.uiObjType == CONST_objType_messenger)
					{
						if (pMsg->talk.data.route.idInfo_from.ui64Id != m_var.pMisCnt->idInfo.ui64Id)
						{
							mySendTalkResp(pProcInfo, &pMsg->talk, NULL);
						}
					}
					*/


                    //
                    //if (pMsg->talk.tStartTime) getTimeStr(pMsg->talk.tStartTime, timeBuf, mycountof(timeBuf));
                    //tmpMakeHtmlStr_talk(hDlg, &m_var, &pMsg->talk, displayContent, mycountof(displayContent));
                    //if  (  !m_var.guiData.param.bNeedNotShowWnd  )  
                    {
                        //addToRecentMsg(hDlg, &m_var, -1, 0, &pTalkData->route.idInfo_from, senderDesc, CONST_imOp_says, 0, timeBuf, rowIdStr, displayContent, pParam->bFollowingRows, pParam->bScrollIntoView, txtContent, mycountof(txtContent));

                        //
                    }
                    UInt64 id = pMsg->addr_logicalPeer.idInfo.ui64Id;
                    //
                    GuiShare.pf_talk_addNewMsg(hDlgTalk, pMsg->data.route.idInfo_from, pMsg->tStartTime, pMsg->uiTranNo, pMsg->usCode, pContent->uiType, new string(pContent->html.wBuf), pMsg->iTaskId);



                    //
                    break;
                default:
                    //  traceLogA(  "tmpHandler_showMsg: Î´´¦Àí%SµÄÊý¾ÝÄÚÈÝ",  qyGetDesByType1(  CONST_pcCommTypeTable,  pContent->uiType  )  );
                    break;
            }




            iErr = 0;
        errLabel:
            return iErr;
        }


        //  2013/09/11
        public static unsafe int tmpHandler_showMsg_task(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgParam)
        {
            int iErr = -1;

            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            ref MIS_MSG_TASK pMsg = ref pMsgParam.task;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();  //
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            if (null == pMisCnt) return -1;

            int i;
            //TCHAR tBuf[128] = _T("");

            //
            //CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;
            object hMgr = hDlgTalk;
            DLG_TALK_var pMgrVar = m_var;

            string str;

            //
            switch (pMsg.uiType)
            {
                case Consts.CONST_misMsgType_task:

                    {
                        ref MIS_MSG_routeTalkData pTalkData = ref pMsg.data;
                        ref IM_CONTENTU pContent = ref pTalkData.buf;
                        QY_MESSENGER_ID idInfo_talker;
                        //
                        //char timeBuf[CONST_qyTimeLen + 1] = "";
                        //TASK_DISPLAY taskDisplay;
                        //char buf[255 + 1] = "";
                        int nItem;
                        int iStatus;

                        //
                        if (pContent.uiType == Consts.CONST_imCommType_transferAvInfo)
                        {
                            str = qyFuncs.getMsgDesc(ref pMsgParam);
                            GuiShare.pf_talk_addNewMsg(hDlgTalk, pMsg.data.route.idInfo_from, pMsg.tStartTime, pMsg.uiTranNo, pMsg.usCode, pContent.uiType, str, pMsg.iTaskId);
                            break;

                        }
                        else if (pContent.uiType == Consts.CONST_imCommType_transferFileReq)
                        {
                            str = qyFuncs.getMsgDesc(ref pMsgParam);
                            GuiShare.pf_talk_addNewMsg(hDlgTalk, pMsg.data.route.idInfo_from, pMsg.tStartTime, pMsg.uiTranNo, pMsg.usCode, pContent.uiType, str, pMsg.iTaskId);
                            break;

                        }


                    }
                    break;

                default:
                    break;
            }

            iErr = 0;

        errLabel:


            //  2013/09/09	
            //dyn_d3dWall_refreshWallMemByTalker(hDlg);

            return iErr;

        }

        //
        public static unsafe int tmpHandler_showMsg_input_taskInteractionReq(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgParam,
                                                   ref IM_CONTENTU pContent)
        {
            int iErr = -1;
            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            ref MIS_MSG_INPUT pMsg = ref pMsgParam.input;
            if (pMsg.uiType != Consts.CONST_misMsgType_input)
            {
                return -1;
            }

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
            if (null == pFuncs) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            int i;
            //TCHAR tBuf[128];

            bool bDbg = true;
            string str;


            if (!isTalkerShadowMgr(ref m_var.addr)) return -1;
            object hMgr = hDlgTalk;
            DLG_TALK_var pMgrVar = m_var;


            //
            using (CQySyncObj syncObj = new CQySyncObj())
            {
                ;
                //TCHAR mutexName[128];

                //
                if (0 != syncObj.sync("", pMisCnt.hMutex_syncActiveMem, 1000)) goto errLabel;


                //
                switch (pContent.uiType)
                {
                    case Consts.CONST_imCommType_taskInteractionReq:
                        {
                            if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
                            QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "tmpHandler_showMsg_input_taskInteractionReq");
                            if (null == pTaskInfo) goto errLabel;
                            QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                            if (pTaskData == null) goto errLabel;
                            if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
                            QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                            DLG_TALK_videoConference videoConference = pTc.getVideoConference();

                            //DLG_TALK_videoConference pVc = pTc.videoConference;
                            bool bBroadcasted = false;

#if DEBUG
                            qyFuncs.traceLogA("taskInteractionReq");
                            ref IM_CONTENTU tmp_pContent = ref pContent;
#endif

                            if (pContent.taskInteractionReq.usOp == Consts.CONST_imOp_stopSpeaking)
                            {
                                QY_MESSENGER_ID idInfo_speaker; idInfo_speaker.ui64Id = 0;
                                QY_MESSENGER_ID idInfo_mgr; idInfo_mgr.ui64Id = 0;
                                //
                                if (0 == pContent.taskInteractionReq.avStream.idInfo.ui64Id) idInfo_speaker.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                else
                                {
                                    idInfo_speaker.ui64Id = pContent.taskInteractionReq.avStream.idInfo.ui64Id;
                                    idInfo_mgr.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                }

                                //
                                bool bActive = qyFuncs.isMemFromActive(pTc, idInfo_speaker, ref pContent.taskInteractionReq.avStream.obj.resObj, null);
                                if (bActive)
                                {
                                    confStarter_requestToSpeak(hDlgTalk, idInfo_speaker, ref pContent.taskInteractionReq.avStream.obj, "", false);
                                    //
                                    //
                                    bool bModified = false;
                                    dlgTalk_refresh_usLayoutType_cur(hDlgTalk, &bModified);
                                    if (bModified)
                                    {
                                        askToRefreshLayout_mgr(hDlgTalk);
                                    }

                                }
                                //

                                if (0 == idInfo_mgr.ui64Id) str = string.Format("{0} request to stop speaking", idInfo_speaker.ui64Id);
                                else str = string.Format("{0} let {1} stop speaking", idInfo_mgr.ui64Id, idInfo_speaker.ui64Id);
                                qyFuncs.showInfo_open(0, null, str);

                                //
                                if (bActive && !qyFuncs.isMemFromActive(pTc, idInfo_speaker, ref pContent.taskInteractionReq.avStream.obj.resObj, null))
                                {
                                    //traceLogA("broadcast all mems info in group when %I64u stop speaking.", idInfo_speaker.ui64Id);
                                    str = string.Format("broadcast all mems info in group when {0} stop speaking.", idInfo_speaker.ui64Id);

                                    //
                                    sendConfKey(hDlgTalk, m_var.addr.idInfo, str);

                                    //
                                    bBroadcasted = true;
                                }
                            }
                            else if (pContent.taskInteractionReq.usOp == Consts.CONST_imOp_requestToSpeak)
                            {

                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%I64u request to speak"), pMsg->task.data.route.idInfo_from.ui64Id);

                                //  2015/02/17
                                if (!m_var.av.taskInfo.bTaskExists
                                    || !m_var.av.taskInfo.ucbStarter)
                                {
                                    qyFuncs.showInfo_open(0, null, _T("failed, not the conf starter"));
                                    goto errLabel;
                                }
#if false

										if (m_var.av.taskInfo.ucbVideoConference)
										{

											//  2015/02/17. 新的版本才在taskInteractionReq里包含tranInfo.
											if (0 != pContent->taskInteractionReq.tranInfo.audio.uiTranNo_openAvDev || 0 != pContent->taskInteractionReq.tranInfo.video.uiTranNo_openAvDev)
											{

												//  2015/07/30
												if (0 != pContent->taskInteractionReq.idInfo_speaker.ui64Id)
												{
													qyFuncs.showInfo_open(0, null, _T("err: mgr can't send tranInfo"));
													goto errLabel;
												}

												//
												bool bErr_inFromList = false;
												DLG_TALK_videoConferenceMemFrom pFrom = null;
												QY_MESSENGER_ID idInfo_from;
												idInfo_from.ui64Id = pMsg->task.data.route.idInfo_from.ui64Id;
												for (i = 0; i < m_var.videoConference.usCntLimit_mems_from; i++)
												{
													pFrom = m_var.videoConference.pMems_from[i];
													//
													if (pFrom.idInfo.ui64Id == idInfo_from.ui64Id) break;
												}
												if (i == m_var.videoConference.usCntLimit_mems_from)
												{
													bErr_inFromList = true;
													//
													qyFuncs.showInfo_open(0, null, _T("proc requestToSpeak: not in from list"));
												}
												else
												{
													fixed (AV_TRAN_INFO* pTranInfo = &pFrom.tranInfo)
													{
														if (0 != qyFuncs.mymemcmp((IntPtr)pTranInfo, (IntPtr)(byte*)&pContent->taskInteractionReq.tranInfo, sizeof(AV_TRAN_INFO)))
														{
															bErr_inFromList = true;
															//											  
															qyFuncs.showInfo_open(0, null, _T("proc requestToSpeak: tranInfo not matched"));
														}
													}
												}
												//
												if (bErr_inFromList)
												{
													//										
													m_var.videoConference.bNeedRefresh_fromList = true;
													//
													if (refreshTaskAv_videoConferenceStarter(hDlgTalk, (IntPtr)(byte*)&pMsg->task) < 0)
													{
														qyFuncs.showInfo_open(0, null, _T("proc request: refreshTaskAv_videoConferenceStarter failed"));
														goto errLabel;
													}
												}
											}
										}
#endif

                                //
#if DEBUG
                                ref IM_CONTENTU tmp_pContentU = ref pContent;
#endif

                                //  2015/07/30													
                                QY_MESSENGER_ID idInfo_speaker; idInfo_speaker.ui64Id = 0;
                                QY_MESSENGER_ID idInfo_mgr; idInfo_mgr.ui64Id = 0;
                                //
                                if (0 == pContent.taskInteractionReq.avStream.idInfo.ui64Id) idInfo_speaker.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                else
                                {
                                    idInfo_speaker.ui64Id = pContent.taskInteractionReq.avStream.idInfo.ui64Id;
                                    idInfo_mgr.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                }

                                //  first, add to the requestingMems
                                int index_active = 0;
                                if (!qyFuncs.isMemFromActive(pTc, idInfo_speaker, ref pContent.taskInteractionReq.avStream.obj.resObj, &index_active))
                                {
                                    //
                                    bool ucbManual = pProcInfo.cfg.policy.ucbManullyPermitToSpeakOnVideoConference;
                                    //
                                    if (!isTalkerShadowMgr(ref m_var.addr)) goto errLabel;
                                    //TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)m_var.pShadowInfo;
                                    //if (!pShadowMgr) goto errLabel;
                                    //
                                    CONF_mgr_info confMgrInfo; // pShdowMgr->av.confMgrInfo
                                    qyFuncs.mymemset((IntPtr)(byte*)&confMgrInfo, 0, sizeof(CONF_mgr_info));

                                    //
                                    bool bVal = false;
                                    if (0 == getVal_bMsgrConfMgr(pMsg.data.route.idInfo_from, ref confMgrInfo, ref bVal)
                                        && bVal)
                                    {
                                        ucbManual = false;
                                    }
                                    //
                                    if (ucbManual)
                                    {
                                        if (!isMemFromRequesting(pTc,  pMsg.data.route.idInfo_from))
                                        {
                                            if (0 == addTo_requestingMems_from(pMisCnt, pTc, pMsg.data.route.idInfo_from))
                                            {
                                                //  trigger the instantAssistant to refresh
                                                //
                                                refreshTalkerList(hDlgTalk);
                                            }
                                        }
                                        /*
										//  2015/02/16
										tBuf[0] = 0;
										//  _sntprintf(  tBuf,  mycountof(  tBuf  ),  _T(  "%s : "  ),  getResStr(  0,  &pQyMc->cusRes,  CONST_resId_systemMessage  )  );
										_sntprintf(tBuf, mycountof(tBuf), _T("%s %I64u %s. %s"), tBuf, pMsg->task.data.route.idInfo_from.ui64Id, getResStr(0, &pQyMc->cusRes, CONST_resId_requestToSpeak), getResStr(0, &pQyMc->cusRes, CONST_resId_waitingForPermitted));
										showNotification_open(0, 0, 0, tBuf);
										sendNotification(tBuf, &m_var.addr.idInfo, &pMsg->task.data.route.idInfo_from);
										showNotification_d3d(hDlg, tBuf);
										*/
                                        //
                                    }
                                    else
                                    {
                                        confStarter_requestToSpeak(hDlgTalk, idInfo_speaker, ref pContent.taskInteractionReq.avStream.obj, "", true);
                                    }
                                    //
                                    //
                                    if (0 != pContent.taskInteractionReq.avStream.obj.resObj.uiObjType)
                                    {
                                        bool bModified = false;
                                        dlgTalk_refresh_usLayoutType_cur(hDlgTalk, &bModified);
                                        if (bModified)
                                        {
                                            askToRefreshLayout_mgr(hDlgTalk);
                                        }
                                    }
                                    //
                                    if (qyFuncs.isMemFromActive(pTc, idInfo_speaker, ref pContent.taskInteractionReq.avStream.obj.resObj, null)
                                        || isMemFromRequesting(pTc, idInfo_speaker)     //  2015/07/30. 因为需要发给管理员来启停发言，所以，即使是手动允许，也要发送一遍
                                        )
                                    {
                                        str = string.Format("broadcast all mems info in group when {0} is speaking.", idInfo_speaker.ui64Id);
                                        //
                                        //
                                        sendConfKey(hDlgTalk, m_var.addr.idInfo, str);

                                        bBroadcasted = true;
                                    }
                                }
                                else
                                {
                                    DLG_TALK_videoConferenceActiveMemFrom pActive = videoConference.activeMems_from[index_active];
                                    if (pContent.taskInteractionReq.avStream.obj.tranInfo.audio.uiTranNo_openAvDev == 0
                                        && pContent.taskInteractionReq.avStream.obj.tranInfo.video.uiTranNo_openAvDev == 0)
                                    {
                                        qyFuncs.showInfo_open(0, null, _T("requestToSpeak failed, a_tn and v_tn are 0"));
                                        goto errLabel;
                                    }
                                    if (pActive.avStream.obj.tranInfo.audio.uiTranNo_openAvDev != pContent.taskInteractionReq.avStream.obj.tranInfo.audio.uiTranNo_openAvDev
                                        || pActive.avStream.obj.tranInfo.video.uiTranNo_openAvDev != pContent.taskInteractionReq.avStream.obj.tranInfo.video.uiTranNo_openAvDev)
                                    {
                                        //  update avStream
                                        pActive.avStream.obj.tranInfo = pContent.taskInteractionReq.avStream.obj.tranInfo;
                                        //
                                        sendConfKey(hMgr, m_var.addr.idInfo, _T("talk_msgTask_taskInteraction, update activeMem"));
                                        iErr = 0; goto errLabel;
                                    }

                                }
                            }
                            //								
                            else if (pContent.taskInteractionReq.usOp == Consts.CONST_imOp_prohibitSpeaking)
                            {
                                i = 0;
                                //
                                pMgrVar.av.taskInfo.confMem_confCtrl.ucbPermitToSpeak = qyFuncs.byte2bool(0);
                                //
                                pMgrVar.av.taskInfo.confMem_confCtrl.idInfo_starter.ui64Id = pMgrVar.av.taskInfo.idInfo_starter.ui64Id;
                                pMgrVar.av.taskInfo.confMem_confCtrl.tStartTime = pMgrVar.av.taskInfo.tStartTime_org;
                                pMgrVar.av.taskInfo.confMem_confCtrl.uiTranNo = pMgrVar.av.taskInfo.uiTranNo_org;
                                //
                                store_ConfMem_confCtrl(ref pMgrVar.av.taskInfo.confMem_confCtrl, pMgrVar.addr.idInfo);
                                //
                                dlgTalk_requestToSpeak(hMgr, false);
                                //
                                refreshTalkerList(hMgr);
                                //
                                qyFuncs.showNotification(0, pMsg.data.route.idInfo_from.ui64Id, 0, 0, 0, 0, _T("主持人停止了你的发言"));
                                //
                                break;
                            }
                            else if (pContent.taskInteractionReq.usOp == Consts.CONST_imOp_permitToSpeak)
                            {
                                //
                                pMgrVar.av.taskInfo.confMem_confCtrl.ucbPermitToSpeak = true;
                                //
                                //
                                pMgrVar.av.taskInfo.confMem_confCtrl.idInfo_starter.ui64Id = pMgrVar.av.taskInfo.idInfo_starter.ui64Id;
                                pMgrVar.av.taskInfo.confMem_confCtrl.tStartTime = pMgrVar.av.taskInfo.tStartTime_org;
                                pMgrVar.av.taskInfo.confMem_confCtrl.uiTranNo = pMgrVar.av.taskInfo.uiTranNo_org;
                                //
                                store_ConfMem_confCtrl(ref pMgrVar.av.taskInfo.confMem_confCtrl, pMgrVar.addr.idInfo);
                                //
                                if (pProcInfo.cfg.policy.avRules.ucbLetConfMgrSetMicOn)
                                {

                                    //  2017/07/13
                                    qyFuncs.setCurSharedObjUsr_localAv(pProcInfo, pMgrVar.av.iIndex_sharedObj_localAv, hMgr, "tmp_showMsg_input_taskInteractionReq.1956");

                                    dlgTalk_requestToSpeak(hMgr, true);
                                }
                                //
                                refreshTalkerList(hMgr);
                                //
                                QY_MESSENGER_ID idInfo_from;idInfo_from.ui64Id = 0 ;
                                qyFuncs.showNotification(0, pMsg.data.route.idInfo_from.ui64Id,  idInfo_from.ui64Id, 0, 0, 0, _T("主持人请你发言"));
                                //
                                break;
                            }
                            //
                            /*
							else if (pContent->taskInteractionReq.usOp == CONST_imOp_requestToControlDesktop_yz)
							{
								if (pQyMc->iCustomId != CONST_qyCustomId_bjyz) goto errLabel;
								//
								bool bRemoteAssist = true;
								//	
								dlgTalk_OnCmd_shareScreen(hMgr, bRemoteAssist);
								//
								showNotification(0, &pMsg->task.data.route.idInfo_from, 0, 0, 0, 0, _T("请你共享屏幕"));
								//
								break;
							}
							else if (pContent->taskInteractionReq.usOp == CONST_imOp_stopControlDesktop_yz)
							{
								if (pQyMc->iCustomId != CONST_qyCustomId_bjyz) goto errLabel;
								//
								bool bRemoteAssist = true;
								//	
								dlgTalk_OnCmd_shareScreen(hMgr, bRemoteAssist);
								//
								showNotification(0, &pMsg->task.data.route.idInfo_from, 0, 0, 0, 0, _T("请你取消共享"));
								//
								break;
							}
							*/
                            else
                            {
#if __DEBUG__
										traceLogA("%I64u request to refresh activeMems info", pMsg->task.data.route.idInfo_from.ui64Id);
#endif
                            }

                            //
                            if (!videoConference.bNeedRefresh_activeMems_from || !bBroadcasted)
                            {
                                sendConfKey(hDlgTalk, pMsg.data.route.idInfo_from, "");
                                //
#if __DEBUG__
										traceLogA("send back all mems info to the requester");
#endif
                                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Send back all mems info to %I64u"), pMsg->task.data.route.idInfo_from.ui64Id);
                            }

                            //  2013/07/18
                            qyFuncs.dyn_d3dWall_refreshImgs(hDlgTalk);

                        }
                        break;
                    default:
                        break;
                }

            }

            iErr = 0;
        errLabel:

            //
            if (m_var.av.taskInfo.bTaskExists)
            {
                //chk_index_activeMems_from_decV(pProcInfo, m_var.av.taskInfo.iTaskId);
                //
                refreshTaskAvTexes(m_var.av.taskInfo.iTaskId);

            }

            //
            return iErr;
        }

        //
        public static unsafe int tmpHandler_showMsg_input_confKey(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgParam,
                                                   ref IM_CONTENTU pContent)
        {
            int iErr = -1;

            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            ref MIS_MSG_INPUT pMsg = ref pMsgParam.input;
            if (pMsg.uiType != Consts.CONST_misMsgType_input)
            {
                qyFuncs.showInfo_open(0, null, "tmpHandler_showMsg_input_confKey failed, msg.type is not input");
                return -1;
            }

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
            if (null == pFuncs) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            bool bDbg = true;
            string str;

            //
            if (!isTalkerShadowMgr(ref m_var.addr)) return -1;
            object hMgr = hDlgTalk;
            DLG_TALK_var pMgrVar = m_var;

            //这个值拿到了哈     1就是 放大了    0就是没放大 
            int bAmplification = pContent.confKey.confLayoutParam.respVideoStatus.bAmplification;
            int i, j;
            qyFuncs.traceLog("初始视频状态：" + bAmplification);

            //
            str = string.Format("tmpHandler_showMsg_input_confKey enters: talker{0}: recv confKey from {1}", m_var.addr.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id);
            qyFuncs.showInfo_open(0, null, str);

            try
            {

                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (0 != syncObj.sync("", pMisCnt.hMutex_syncActiveMem, 1000)) goto errLabel;

                    //
                    switch (pContent.uiType)
                    {
                        case Consts.CONST_imCommType_confKey:

                            {
                                bool bNeedRefresh_activeMems_from = false;  //  2010/09/12


#if __DEBUG__
						traceLogA("confKey");
#endif
                                str = string.Format("showMsg_input_confKey: talker{0}: recv confKey from {1}", m_var.addr.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id);
                                qyFuncs.showInfo_open(0, null, str);


                                //
                                //
                                if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
                                if (m_var.av.taskInfo.ucbStarter)
                                {
#if __DEBUG__
							traceLogA("Can't send status to starter.");
#endif
                                    goto errLabel;
                                }
                                QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, m_var.av.taskInfo.iIndex_taskInfo, "tmpHandler_showMsg_input_confKey");
                                if (null == pTaskInfo) goto errLabel;
                                QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                                if (pTaskData == null) goto errLabel;
                                if (pTaskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
                                QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
                                DLG_TALK_videoConference videoConference = pTc.getVideoConference();

#if DEBUG
                                ref IM_CONTENTU tmp_pContent = ref pContent;
#endif

#if true   //  2013/07/18
                                int bufSize = 0;
                                int a = 0;
                                fixed ( byte * ppp= pContent.confKey.buf_VIDEO_CONFERENCE_MEM_mems)
                                {
                                    VIDEO_CONFERENCE_MEM* pContent_confKey_mems = (VIDEO_CONFERENCE_MEM*)ppp;
                                    bufSize = Consts.bufSize_videoConferenceMem_mems;
                                    //
                                    for (i = 0; i < pContent.confKey.usCnt; i++)
                                    {
                                        VIDEO_CONFERENCE_MEM* pMem = qyFuncs.getMem(pContent_confKey_mems, bufSize, i);
                                        //
                                        if (0 == pMem->avStream.idInfo.ui64Id) break;
                                        //if (0 == pContent_videoConferenceStatus_mems[i].ucbActive) break;
                                        if (i >= (videoConference.activeMems_from.Length)) break;
                                        //

                                        //
                                        //
                                        DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[a];
                                        if (pActiveMem_from.avStream.idInfo.ui64Id != pMem->avStream.idInfo.ui64Id
                                            || pActiveMem_from.avStream.obj.tranInfo.audio.uiTranNo_openAvDev != pMem->avStream.obj.tranInfo.audio.uiTranNo_openAvDev
                                            || pActiveMem_from.avStream.obj.tranInfo.video.uiTranNo_openAvDev != pMem->avStream.obj.tranInfo.video.uiTranNo_openAvDev
                                            )
                                        {
                                            pActiveMem_from.avStream = pMem->avStream;
                                            //  getTalkerDesc(  m_var.pMisCnt,  &pVc.activeMems_from[i].mem.idInfo,  pVc.activeMems_from[i].desc,  mycountof(  pVc.activeMems_from[i].desc  ),  0,  0  );
                                            TMP_tBuf128 tBuf_desc;
                                            int cntof_desc = Consts.cntof_tBuf128;
                                            qyFuncs.M_getActiveMemDesc(pMisCnt, pActiveMem_from.avStream.idInfo, tBuf_desc.tBuf, cntof_desc);
                                            fixed (char* pDesc = pActiveMem_from.desc.desc)
                                            {
                                                qyFuncs.mytcsncpy(pDesc, tBuf_desc.tBuf, Consts.cntof_desc);
                                            }
                                            //
                                            bNeedRefresh_activeMems_from = true;            //  2010/09/12

                                            //
                                            str = string.Format("Conference status changed, {0} modified.", pActiveMem_from.avStream.idInfo.ui64Id);
                                            qyFuncs.traceLog(str);
                                            uint step = 0;
                                            qyFuncs.qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, _T(""), _T(""), str);

                                        }
                                        a++;
                                    }
                                    //  j  =  i;
                                    for (; a < (videoConference.activeMems_from.Length); a++)
                                    {
                                        DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[a];
                                        if (0 != pActiveMem_from.avStream.idInfo.ui64Id)
                                        {
                                            pActiveMem_from.avStream.idInfo.ui64Id = 0;
                                            //
                                            bNeedRefresh_activeMems_from = true;            //  2010/09/12
                                        }
                                    }
                                }
                                //  i  =  j;
#endif

                                //
                                for (j = 0; j < pContent.confKey.requestingMems.usCnt; j++)
                                {
                                    fixed (byte * ppp=pContent.confKey.requestingMems.buf_CONF_requesting_mems1)
                                    {
                                        CONF_requesting_mem* pContent_requestingMems_from = (CONF_requesting_mem*)ppp;
                                        bufSize = Consts.bufSize_CONF_requesting_mems;
                                        //
                                        CONF_requesting_mem* pMem = qyFuncs.getMem(pContent_requestingMems_from, bufSize, j);
                                        //
                                        if (0 == pMem->idInfo.ui64Id) break;
                                        if (j >= (videoConference.requestingMems_from.Length)) break;
                                    }
                                }
                                for (; j < (videoConference.requestingMems_from.Length); j++)
                                {
                                    //if (0 != pVc.requestingMems_from[j].avStream.idInfo.ui64Id) pVc.requestingMems_from[j].avStream.idInfo.ui64Id = 0;
                                }

#if __DEBUG__
										printDlgTalkVideoConference(pVc);
#endif
                                if (!GuiShare.pf_IsWindow(hDlgTalk)) return iErr;
                                Activity_dlgTalk pDlgTalk = (Activity_dlgTalk)(hDlgTalk);
                                qyFuncs.traceLog("pDlgTalk.respVideoStatusIsenlargement:" + pContent.confKey.confLayoutParam.respVideoStatus.bAmplification);
                                pDlgTalk.respVideoStatusIsenlargement = pContent.confKey.confLayoutParam.respVideoStatus.bAmplification;

                                //球机号选中的
                                int qiuji = pContent.confKey.confLayoutParam.ipcParam.iChannel;
                                Int64 qiuji_ScaleId = pContent.confKey.confLayoutParam.enlargeParam.ui64Id;
                                //这里拿到那个ID号了哈
                                pDlgTalk.m_var.currentSelectQiuJiIdInfo = qiuji_ScaleId;
                                qyFuncs.traceLog("##球机qiuji_ScaleId:" + qiuji_ScaleId);
                                pDlgTalk.m_var.currentSelectQiuJiChannel = qiuji;

                                ((Activity_dlgTalk_av)(pDlgTalk.m_hTool_dlgTalk_av)).m_fg_dlgTalk_av2.changeNvrControlStatus(qiuji);
                                qyFuncs.traceLog("##球机号选中的" + qiuji);
                                int ii = 0;






                                //  2009/05/31
                                refreshTaskAv_videoConferenceOthers(hDlgTalk);

                                //
                                refreshTaskAvTexes(m_var.av.taskInfo.iTaskId);

                                //
                                /*
                                QY_SHARED_OBJ* pSharedObj;
                                pSharedObj = getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                                if (pSharedObj)
                                {
                                    SHARED_OBJ_USR* pSharedObjUsr = getSharedObjUsr(pSharedObj, m_var.av.iIndex_usr_localAv);
                                    if (pSharedObjUsr && pSharedObjUsr->iTaskId == m_var.av.taskInfo.iTaskId)
                                    {
                                        if (isMemFromActive(pVc, &m_var.pMisCnt->idInfo, NULL))
                                        {
                                            if (pSharedObjUsr->ucbNotSendData)
                                            {
                                                //  2010/12/29, 为了加强客户端的安全性，要求视频只能在用户点击时，才能进入发言态。从主持人的列表里下来的信息不能打开发言开关。
                                                if (m_var.av.taskInfo.bEnableToSpeak)
                                                {
                                                    pSharedObjUsr->ucbNotSendData = FALSE;
                                                    qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Conference status refreshed. Start to speak."));
                                                }
                                                else
                                                {
                                                    qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Conference status refreshed. This messenger is already in active list but in fact do not speak."));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!pSharedObjUsr->ucbNotSendData)
                                            {
                                                pSharedObjUsr->ucbNotSendData = TRUE;
                                                qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Conference status refreshed. Speaking stopped."));
                                            }
                                        }
                                    }
                                }
                                */

                                if (bNeedRefresh_activeMems_from)
                                {
                                    //dlgTalk_freeCapImageBySth_mgr(hDlgTalk, m_var.av.taskInfo.iTaskId, null);       //  2010/09/12
                                }

                                //
                                //  2017/07/05
                                //refreshTalkerList(hDlgTalk);


                                //  2010/12/29
                                m_var.av.taskInfo.usLayoutType_starter = pContent.confKey.usLayoutType_starter;

                                if (0 == dlgTalk_get_usLayoutType_local(hDlgTalk))
                                {
                                    askToRefreshLayout_mgr(hDlgTalk);
                                }

                                break;
                            }
                            break;
                        default:
                            break;
                    }

                }

            }
            catch (Java.Lang.Exception e)
            {
                qyFuncs.showInfo_open(0, "", "tmpHandler_showMsg_input_confKey, exception");
            }

            iErr = 0;
        errLabel:

            //
            str = string.Format("tmpHandler_showMsg_input_confKey leaves: talker{0}", m_var.addr.idInfo.ui64Id);
            qyFuncs.showInfo_open(0, null, str);

            //
            return iErr;

        }


        //
        public static int refreshTaskAvTexes(int iTaskId)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            int i;

            //
            int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, iTaskId);
            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
            if (pTaskInfo == null) goto errLabel;
            QMC_taskData_common taskData = pTaskInfo.getTaskData();
            if (taskData == null) goto errLabel;
            if (taskData.uiType != Consts.CONST_taskDataType_conf) goto errLabel;
            QMC_taskData_conf pTc = (QMC_taskData_conf)taskData;
            DLG_TALK_videoConference videoConference = pTc.getVideoConference();
            if (videoConference == null) goto errLabel;





            //
            TMP_task_texes ttt = pProcInfo.av.texes.taskAvTexes;

            //
            if (videoConference.activeMems_from.Length > ttt.tti_peers.Length)
            {
                goto errLabel;
            }
            if (ttt.iTaskId != iTaskId)
            {
                ttt.iTaskId = iTaskId;
            }
            if (ttt.usCnt_activeMems != videoConference.usCntLimit_activeMems_from)
            {
                ttt.usCnt_activeMems = videoConference.usCntLimit_activeMems_from;
            }
            //
            for (i = 0; i < videoConference.usCntLimit_activeMems_from; i++)
            {
                if (ttt.tti_peers[i].idInfo.ui64Id != videoConference.activeMems_from[i].avStream.idInfo.ui64Id)
                {
                    ttt.tti_peers[i].idInfo = videoConference.activeMems_from[i].avStream.idInfo;
                }
                if (ttt.tti_peers[i].uiObjType != videoConference.activeMems_from[i].avStream.obj.resObj.uiObjType)
                {
                    ttt.tti_peers[i].uiObjType = videoConference.activeMems_from[i].avStream.obj.resObj.uiObjType;
                }
            }

            //
            iErr = 0;
        errLabel:

            return iErr;
        }




        //
        public static unsafe int tmpHandler_showMsg_input_transferAvInfo(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgParam,
                                           ref IM_CONTENTU pContent)
        {
            int iErr = -1;

            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            fixed (MIS_MSG_INPUT* pMsg = &pMsgParam.input)
            {
                if (pMsg->uiType != Consts.CONST_misMsgType_input)
                {
                    qyFuncs.showInfo_open(0, null, "tmpHandler_showMsg_input_transferAvInfo failed, msg.type is not input");
                    return -1;
                }

                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                if (null == pProcInfo) return -1;
                FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
                if (null == pFuncs) return -1;
                MIS_CNT pMisCnt = pProcInfo.pMisCnt;
                MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

                //
                bool bDbg = true;
                string str;

                //
                if (!isTalkerShadowMgr(ref m_var.addr)) return -1;
                object hMgr = hDlgTalk;
                DLG_TALK_var pMgrVar = m_var;


                //			
                int i, j;

                //
                str = string.Format("talker{0}: recv transferAvInfo from {1}", m_var.addr.idInfo.ui64Id, pMsg->data.route.idInfo_from.ui64Id);
                qyFuncs.showInfo_open(0, null, str);

                //
#if DEBUG
                ref MIS_MSG_INPUT tmp_pMsg = ref *pMsg;
#endif

                //
                switch (pContent.uiType)
                {
                    case Consts.CONST_imCommType_transferAvInfo:
                        //
                        QY_MESSENGER_ID idInfo_from;
                        idInfo_from = pMsg->data.route.idInfo_from;
                        if (pContent.transferAvInfo.confCfg.idInfo_initiator.ui64Id != 0)
                        {
                            idInfo_from.ui64Id = pContent.transferAvInfo.confCfg.idInfo_initiator.ui64Id;
                        }
                        //
                        str = "视频呼叫";
                        //
                        GuiShare.pf_talk_addNewMsg(hDlgTalk, idInfo_from, pMsg->tStartTime, pMsg->uiTranNo, pMsg->usCode, pContent.uiType, str, pMsg->iTaskId);

                        //
                        if (pMgrVar.av.taskInfo.bTaskExists) goto errLabel;
                        //
                        if (((Activity_dlgTalk)hMgr).m_hTool_dlgTalk_av == null) goto errLabel;
                        Activity_dlgTalk_av dlgTalk_av = ((Activity_dlgTalk)hMgr).m_hTool_dlgTalk_av;
                        //
                        //dlgTalk_av.doTask_av(tmpResource.IDC_BUTTON_av_accept, pMsg->iTaskId);
                        GuiShare.pf_PostMessage_new(dlgTalk_av, Consts.CONST_qyWndContentType_dlgTalk_av, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_doTask_av_accept, pMsg->iTaskId);

                        //
                        break;
                    default:
                        break;
                }
            }

            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public static unsafe int tmpHandler_showMsg_input_transferFileReq(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgU,
                                   ref IM_CONTENTU pContent)
        {
            int iErr = -1;

            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            ref MIS_MSG_INPUT pMsg = ref pMsgU.input;
            if (pMsg.uiType != Consts.CONST_misMsgType_input)
            {
                qyFuncs.showInfo_open(0, null, "tmpHandler_showMsg_input_transferFileReq failed, msg.type is not input");
                return -1;
            }

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
            if (null == pFuncs) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            bool bDbg = true;
            string str;

            //
            if (!isTalkerShadowMgr(ref m_var.addr)) return -1;
            object hMgr = hDlgTalk;
            DLG_TALK_var pMgrVar = m_var;


            //			
            int i, j;

            //
            str = string.Format("talker{0}: recv transferFileReq from {1}", m_var.addr.idInfo.ui64Id, pMsg.data.route.idInfo_from.ui64Id);
            qyFuncs.showInfo_open(0, null, str);

            //
#if DEBUG
            ref MIS_MSG_INPUT tmp_pMsg = ref pMsg;
#endif

            //
            switch (pContent.uiType)
            {
                case Consts.CONST_imCommType_transferFileReq:
                    //
                    QY_MESSENGER_ID idInfo_from;
                    idInfo_from = pMsg.data.route.idInfo_from;
                    //
                    str = qyFuncs.getMsgDesc(ref pMsgU);
                    //
                    GuiShare.pf_talk_addNewMsg(hDlgTalk, idInfo_from, pMsg.tStartTime, pMsg.uiTranNo, pMsg.usCode, pContent.uiType, str, pMsg.iTaskId);


                    //
                    break;
                default:
                    break;
            }


            iErr = 0;
        errLabel:

            return iErr;
        }





        //  2014/09/05
        public static unsafe int tmpHandler_showMsg_input(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgParam)
        {
            int iErr = -1;

            object hDlgTalk = (object)hDlgTalkParam;

            PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
            ref MIS_MSGU pMsg = ref pMsgParam;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (null == pProcInfo) return -1;
            FUNCS_for_isCliHelp pFuncs = qyFuncs.QY_GET_FUNCS_for_isCliHelp(pProcInfo);
            if (null == pFuncs) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            int i;
            //TCHAR tBuf[128];

            bool bDbg = true;
            string str;


            if (!isTalkerShadowMgr(ref m_var.addr)) return -1;
            object hMgr = hDlgTalk;
            DLG_TALK_var pMgrVar = m_var;


            switch (pMsg.uiType)
            {
                case Consts.CONST_misMsgType_input:
                    {

                        ref IM_CONTENTU pContent = ref pMsg.input.data.buf;// qyFuncs.M_getMsgContent(pMsg->input.uiType,pMsg->input.ucFlg, &pMsg->input.data);


                        switch (pContent.uiType)
                        {
                            case Consts.CONST_imCommType_taskInteractionReq:

                                tmpHandler_showMsg_input_taskInteractionReq(hDlgTalkParam, m_var, p1, ref pMsgParam, ref pContent);

                                break;

                            case Consts.CONST_imCommType_confKey:

                                tmpHandler_showMsg_input_confKey(hDlgTalkParam, m_var, p1, ref pMsgParam, ref pContent);

                                break;

                            case Consts.CONST_imCommType_transferAvInfo:

                                tmpHandler_showMsg_input_transferAvInfo(hDlgTalkParam, m_var, p1, ref pMsgParam, ref pContent);

                                break;
                            case Consts.CONST_imCommType_transferFileReq:

                                tmpHandler_showMsg_input_transferFileReq(hDlgTalkParam, m_var, p1, ref pMsgParam, ref pContent);

                                break;

                            default:
                                break;
                        }

                    }
                    break;

                default:
                    break;
            }

            iErr = 0;

        errLabel:



            return iErr;

        }


        //
        public static unsafe int tmpHandler_showMsg_taskStatus(object hDlgTalkParam, DLG_TALK_var m_var, object p1, ref MIS_MSGU pMsgParam)
        {
            int iErr = -1;
            ref MIS_MSGU pMsg = ref pMsgParam;


            if (pMsg.uiType != Consts.CONST_misMsgType_taskStatus) return -1;

            fixed (MIS_MSG_taskStatus* pMsgTaskStatus = &pMsg.taskStatus)
            {
                ;            //  ÓÐÊ±¾ÍÓÃÕâ¸ö½á¹¹£¬ 2007/07/17

                //MIS_MSG_TASK* pMsgTask = null;

                GuiShare.pf_talk_displayTaskStatus(hDlgTalkParam, pMsgTaskStatus);
            }

            return 0;


#if false
			//IM_CONTENTU* pContent = null;
			int iStatus = 0;
				//int tmpiRet;
				//

#if false
							if (!(pMsgTask = findTaskInProcessQ(&m_var, pMsgTaskStatus->iTaskId))) goto errLabel;
							pContent = (IM_CONTENTU*)pMsgTask->data.buf;

							//  2015/08/07
							if (pContent)
							{
								if (!bSupported_fileServer(pQyMc))
								{
									if (pContent->uiType == CONST_imCommType_transferFileReq)
									{
										if (pContent->transferFileReq.usSubtype == CONST_procOfflineResSubtype_upload
											|| pContent->transferFileReq.usSubtype == CONST_procOfflineResSubtype_download)
										{
											tmpiRet = SendMessage(pContent->transferFileReq.hTool_offlineRes, CONST_qyWm_comm, 0, (LPARAM)pMsgTaskStatus);
											if (tmpiRet != CONST_qyWmRc_ok)
											{
												showInfo_open(0, 0, _T("taskStatus: to tool_offlineRes, not ok"));
												//
												pContent->transferFileReq.bNeedCalcel = true;
												m_var.needChkTasks.bNeedChkRecvdTasks = true;
											}
											else
											{
#if __DEBUG__
												traceLog(_T("taskStatus: to tool_offlineRes: ok"));
#endif
											}
										}
									}
								}
							}


							//
							if (pMsgTaskStatus->iStatus == CONST_imTaskStatus_sendFinished && pMsgTaskStatus->bImGrpMem)
							{   //  
								//	
								for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
								{
									if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
									{
										CHelp_getDlgTalkVar help_getDlgTalkVar;
										tmpHandler_showMsg_displayTaskStatus(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), pMsgTaskStatus, pMsgTask, pContent, iStatus);
									}
								}
								tmpHandler_showMsg_displayTaskStatus(hDlg, &m_var, pMsgTaskStatus, pMsgTask, pContent, iStatus);

								break;
							}
#endif

				//
				switch (pMsgTaskStatus->iStatus)
				{
					case Consts.CONST_imTaskStatus_sending:
					case Consts.CONST_imTaskStatus_receiving:

						//  2015/08/04
#if false
									pShadowMgr->fileTaskInfo.dwLastModifiedTickCnt_taskStatus = GetTickCount();
									pShadowMgr->fileTaskInfo.usTimes_noFileTask = 0;

									//
									for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
									{
										if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
										{
											CHelp_getDlgTalkVar help_getDlgTalkVar;
											tmpHandler_showMsg_displayTaskStatus(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), pMsgTaskStatus, pMsgTask, pContent, iStatus);
										}
									}
#endif

						tmpHandler_showMsg_displayTaskStatus(hDlg, m_var, pMsgTaskStatus, null, null, iStatus);

#if false
									//
									if (pContent && pContent->uiType == CONST_imCommType_transferFileReq)
									{       //  2010/12/04
										if (pMsgTask->idInfo_taskSender.ui64Id != m_var.pMisCnt->idInfo.ui64Id)
										{
											mytime(&pMsgTask->tStartTime_processed);                                        //
										}
									}
#endif

						//
						break;
					case Consts.CONST_imTaskStatus_acceptedByReceiver:
					case Consts.CONST_imTaskStatus_dualByReceiver:
#if false
									if (pMsgTask->iStatus == Consts.CONST_imTaskStatus_req)
									{
										if (!pMsgTaskStatus->bImGrpMem)
										{
											pMsgTask->iStatus = pMsgTaskStatus->iStatus;
										}
										//
										dlgTalk_displayAvStatus(hDlg, m_var, pMsgTask->iTaskId, pMsgTask->iStatus, null);
									}
#endif
						break;
					case Consts.CONST_imTaskStatus_sendFinished:
					case Consts.CONST_imTaskStatus_recvFinished:
					case Consts.CONST_imTaskStatus_canceledBySender:
					case Consts.CONST_imTaskStatus_canceledByReceiver:
					case Consts.CONST_imTaskStatus_autoCanceledByReceiver:     //  2014/07/30

						//
						iStatus = pMsgTaskStatus->iStatus;
						//
#if false
									pMsgTask->iStatus = pMsgTaskStatus->iStatus;    //  2009/10/17

									//
									for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
									{
										if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
										{
											CHelp_getDlgTalkVar help_getDlgTalkVar;
											tmpHandler_showMsg_displayTaskStatus(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), pMsgTaskStatus, pMsgTask, pContent, iStatus);
										}
									}
#endif
						tmpHandler_showMsg_displayTaskStatus(hDlgTalk, m_var, pMsgTaskStatus, null, null, iStatus);


						//  2014/11/22
						if (pMsgTaskStatus->iStatus == Consts.CONST_imTaskStatus_canceledBySender)
						{
#if false
										if (pContent->uiType == CONST_imCommType_transferAvInfo)
										{
											TRANSFER_AV_INFO* pTransferAvInfo = &pContent->transferAvInfo;
											PROC_TASK_AV* pTask = get_transferAvInfo_pTask(pTransferAvInfo, _T(""));
											if (!pTask) goto errLabel;
											//
											if (pTask->dlgTalkInProxy.bExists)
											{
												TASK_PROC_REQ taskProcReq;
												QY_MESSENGER_ID idInfo_dst;
												int lenInBytes;

												memset(&taskProcReq, 0, sizeof(taskProcReq));
												taskProcReq.uiType = CONST_imCommType_taskProcReq;
												taskProcReq.usOp = CONST_imOp_send_cancel;

												taskProcReq.tStartTime_org = pTask->dlgTalkInProxy.proxy.tStartTran;
												taskProcReq.uiTranNo_org = pTask->dlgTalkInProxy.proxy.uiTranNo;
												taskProcReq.uiSerialNo_org = 0;
												taskProcReq.uiContentType_org = pContent->uiType;
												//
												idInfo_dst.ui64Id = m_var.addr.idInfo.ui64Id;
												//
												lenInBytes = sizeof(taskProcReq);
												//					
												MACRO_prepareForTran();
												//
												if (postMsgTask2Mgr_mc(m_var.pMisCnt, CONST_misMsgType_task, 0, pMsgTask->usCode, tStartTran, uiTranNo, 0, pMsgTask->iTaskId, pMsgTask->uiTaskType, (char*)&taskProcReq, lenInBytes, &m_var.addr.idInfo, &pMsgTask->idInfo_taskSender, &pMsgTask->idInfo_taskReceiver, &idInfo_dst, pMsgTask->uiChannelType, NULL, false))
												{
													//goto  errLabel;
												}

											}
										}
#endif
						}


						//
#if false
									dlgTalk_displayAvStatus(hDlg, m_var, pMsgTask->iTaskId, pMsgTask->iStatus, null);
#endif
						//  freeCapImageByTaskId(  hDlg,  0,  pMsgTaskStatus->iTaskId,  &m_var.av.peerImages  );
						dlgTalk_freeCapImageBySth_mgr(hDlgTalk, pMsgTaskStatus->iTaskId, null);


						//  2009/02/20
#if false
									if (m_var.av.taskInfo.bTaskExists)
									{
										if (m_var.av.taskInfo.iTaskId == pMsgTaskStatus->iTaskId) dlgTalk_closeTaskAv(hDlg, m_var);
									}
									//  2013/11/18
									if (pShadowMgr->remoteAssist.iTaskId == pMsgTaskStatus->iTaskId)
									{
										SendMessage(pShadowMgr->remoteAssist.hWnd_remoteAssist, WM_CLOSE, 0, 0);
										pShadowMgr->remoteAssist.hWnd_remoteAssist = NULL;
										pShadowMgr->remoteAssist.iTaskId = 0;
									}
#endif

						//  2010/12/23
						chkResources(hDlgTalk, true);

						//  2012/03/24
						qmcApplyForChkingTasks_gui(pQyMc);

						//  2013/09/09
#if flase
									dyn_d3dWall_refreshWallMemByTalker(hDlg);
#endif

						break;
					default:

						//
#if false
									for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
									{
										if (pShadowMgr->shadows[i].hShadow && isTalkerSubtype_canInput(pShadowMgr->shadows[i].iTalkerSubType))
										{
											CHelp_getDlgTalkVar help_getDlgTalkVar;
											tmpHandler_showMsg_displayTaskStatus(pShadowMgr->shadows[i].hShadow, (DLG_TALK_var*)help_getDlgTalkVar.getVar(pShadowMgr->shadows[i].hShadow), pMsgTaskStatus, pMsgTask, pContent, iStatus);
										}
									}
									tmpHandler_showMsg_displayTaskStatus(hDlg, &m_var, pMsgTaskStatus, pMsgTask, pContent, iStatus);
#endif

						break;
				}

#endif
            //


            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        public static unsafe int tmpHandler_showMsg(object hDlgTalkParam, object p1, ref MIS_MSGU pMsgParam)
        {

            int iErr = -1;
            object hDlg = (object)hDlgTalkParam;
            bool bDbg = false;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

#if DEBUG
            //bDbg = true;
#endif

            //
            if (bDbg)
            {
                qyFuncs.showInfo_open(0, null, "tmpHandler_showMsg enters");
                //qyFuncs.showAllTc(mainActivity.var_common.ctxCaller, "tmpHandler_showMsg enters");
            }

            //
            Activity_dlgTalk dlg = (Activity_dlgTalk)hDlg;

            //    using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            {
                ;
                DLG_TALK_var pm_var = dlg.m_var;
                //(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlg);
                if (null == pm_var) return -1;
                DLG_TALK_var m_var = pm_var;

                PARAM_PROC_ROW pParam = (PARAM_PROC_ROW)p1;
                ref MIS_MSGU pMsg = ref pMsgParam;

                Int64 t=0;
                int tmpiRet = 0;
                //
                //TCHAR tBuf[1024] = _T("");
                int i;
                //

                if (null == hDlg || null == pParam ) return -1;

                if (!isTalkerShadowMgr(ref m_var.addr)) goto errLabel;
                //TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)m_var.pShadowInfo;
                //if (!pShadowMgr) goto errLabel;

                if (!bReady_toShowMsg(hDlg)) goto errLabel;

                qyFuncs.mytime(ref t);

                switch (pMsg.uiType)
                {
                    case Consts.CONST_misMsgType_talk:
                        if (0 != pMsg.talk.tStartTime_processed) break;
                        qyFuncs.mytime(ref pMsg.talk.tStartTime_processed);   //  2007/10/13
                        m_var.nMsgTalks_processed_inProcessQ++;             //  2012/05/20

                        pParam.bMsgNeedRemoved = true;     //  2009/10/18

                        //  2011/03/15
                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("show msg_talk"));

                        //
                        fixed (MIS_MSGU* pMsg0 = &pMsgParam)
                        {
                            if (0 != tmpHandler_showMsg_talk(hDlgTalkParam, m_var, p1, (IntPtr)pMsg0))
                            {
#if __DEBUG__
							traceLog(_T("tmpHandler_showMsg_talk failed"));
#endif
                                goto errLabel;
                            }
                        }


                        break;
                    case Consts.CONST_misMsgType_task:
                        if (0 != pMsg.task.tStartTime_processed) break;
                        qyFuncs.mytime(ref pMsg.task.tStartTime_processed);  //  2007/10/13, ´Ë¸³ÖµÊÇÎªÁË±ÜÃâÏûÏ¢±»ÖØ¸´´¦Àí¡£Ô­À´ÊÇÔÚ´ËcaseµÄ×îÏÂÃæ£¬ÏÖÔÚÒÆµ½ÕâÀï¡£ÊÇÒòÎª£¬Èç¹ûÖÐ¼ä´¦Àí³ö´í£¬break»ògoto µÄ»°£¬¾ÍÎÞ·¨¸³ÖµÁË£¬ÓÚÊÇ¾ÍÒªÒ»ÔÙÖØ¸´´¦Àí´ËÏûÏ¢£¬ÓÖÖØ¸´Ìø³ö£¬Ôì³ÉºóÐøÏûÏ¢ÎÞ·¨´¦ÀíµÄÏÖÏó¡£

                        //
                        if (0 == pMsg.task.iTaskId) pParam.bMsgNeedRemoved = true;        //  2009/10/18

                        //
                        if (0 != tmpHandler_showMsg_task(hDlgTalkParam, m_var, p1, ref pMsgParam)) goto errLabel;

                        break;

                    case Consts.CONST_misMsgType_input:    //  2014/09/05
                        if (0 != pMsg.input.tStartTime_processed) break;
                        qyFuncs.mytime(ref pMsg.input.tStartTime_processed);

                        //
                        pParam.bMsgNeedRemoved = true;

                        //
                        if (0 != tmpHandler_showMsg_input(hDlgTalkParam, m_var, p1, ref pMsgParam)) goto errLabel;

                        break;

                    case Consts.CONST_misMsgType_taskStatus:
                        {

                            if (0 != pMsg.taskStatus.tStartTime_processed) break;
                            qyFuncs.mytime(ref pMsg.taskStatus.tStartTime_processed); //  2007/10/13, ´Ë¸³ÖµÊÇÎªÁË±ÜÃâÏûÏ¢±»ÖØ¸´´¦Àí¡£Ô­À´ÊÇÔÚ´ËcaseµÄ×îÏÂÃæ£¬ÏÖÔÚÒÆµ½ÕâÀï¡£ÊÇÒòÎª£¬Èç¹ûÖÐ¼ä´¦Àí³ö´í£¬break»ògoto µÄ»°£¬¾ÍÎÞ·¨¸³ÖµÁË£¬ÓÚÊÇ¾ÍÒªÒ»ÔÙÖØ¸´´¦Àí´ËÏûÏ¢£¬ÓÖÖØ¸´Ìø³ö£¬Ôì³ÉºóÐøÏûÏ¢ÎÞ·¨´¦ÀíµÄÏÖÏó¡£

                            pParam.bMsgNeedRemoved = true;     //  2009/10/18

                            //
                            if (0 != tmpHandler_showMsg_taskStatus(hDlgTalkParam, m_var, p1, ref pMsgParam)) goto errLabel;

                        }
                        break;

                    case Consts.CONST_misMsgType_notifyTaskEnd_qmc:
                        /*
						if (pMsg->notifyTaskEnd.tStartTime_processed) break;
						mytime(&pMsg->notifyTaskEnd.tStartTime_processed);  //  2007/10/13, 

						pParam->bMsgNeedRemoved = true;     //  2009/10/18

						{
#if true
				  MIS_MSG_notifyTaskEnd_qmc	*	pMsgNotifyTaskEnd  =  (  MIS_MSG_notifyTaskEnd_qmc  *  )&pMsg->notifyTaskEnd;
				  if  (  pMsgNotifyTaskEnd->bToCancel  )  {
					  if  (  !pMsgNotifyTaskEnd->iTaskId  )  {
						  traceLogA(  "iTaskId error"  );
						  goto  errLabel;
					  }
					  dlgTalk_cancelTasks(  hDlg,  &m_var,  pMsgNotifyTaskEnd->iTaskId,  false  );
					  break;
				  }
				  //  
				  //  freeCapImageByTaskId(  hDlg,  0,  pMsgNotifyTaskEnd->iTaskId,  &m_var.av.meImages  );  
				  int  idc_dst  =  m_var.av.idc_me;
				  freeCapImageByTaskId(  hDlg,  m_var.av.idc_me,  pMsgNotifyTaskEnd->iTaskId,  &m_var.av.meZone.images,  M_GetDlgItem(  hDlg,  idc_dst  )  );  
#endif

						}
						*/
                        break;

                    case Consts.CONST_misMsgType_applyForWork:
                        /*
						if (pMsg->applyForWork.tStartTime_processed) break;
						mytime(&pMsg->applyForWork.tStartTime_processed);   //  2007/10/13, 

						pParam->bMsgNeedRemoved = true;     //  2009/10/18

						if (pMsg->applyForWork.uiContentType == CONST_imCommType_queryCustomerServiceOfficerReplyReq)
						{
							if (m_var.av.taskInfo.bTaskExists) break;
							PostMessage(hDlg, WM_COMMAND, MAKEWPARAM(ID_startAvCall, 0), 0);
						}
						*/

                        break;

                    case Consts.CONST_misMsgType_applyForTalkerShadow_qmc:
                        {  //  2012/04/23
                            /*
							if (pMsg->applyForTalkerShadow_qmc.tStartTime_processed) break;
							mytime(&pMsg->applyForTalkerShadow_qmc.tStartTime_processed);   //  2007/10/13, 

							pParam->bMsgNeedRemoved = true;     //  2009/10/18

							//  2014/12/03
							dlgTalk_doApplyForTalkerShadow_qmc(hDlg, &pMsg->applyForTalkerShadow_qmc);
							*/

                        }

                        break;


                    default:
#if __DEBUG__
						traceLog(_T("tmpHandler_showMsg failed, unprocessed msgType %d"), pMsg->uiType);
#endif
                        //
                        //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("tmpHandler_showMsg failed, unprocessed msgType %d"), pMsg->uiType);
                        goto errLabel;
                        break;
                }

            }

            iErr = 0;

        errLabel:

            //
            if (bDbg)
            {
                qyFuncs.showInfo_open(0, null, "tmpHandler_showMsg leaves");
                //qyFuncs.showAllTc(mainActivity.var_common.ctxCaller, "tmpHandler_showMsg leaves");
            }

            return iErr;
        }


        //
#if false
		public static unsafe int tmpHandler_showMsg_displayTaskStatus(object hDlg, DLG_TALK_var pm_var, MIS_MSG_taskStatus* pMsgTaskStatus, MIS_MSG_TASK* pMsgTask, IM_CONTENTU* pContent, int iStatus)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;

			if (null==pm_var) return -1;
			DLG_TALK_var  m_var = pm_var;

			//  2016/03/13
			GuiShare.pf_talk_displayTaskStatus(hDlg, pMsgTaskStatus);
			return 0;

			//






			iErr = 0;
		errLabel:
			return iErr;
		}
#endif






        //
        public static unsafe int getVal_bMsgrConfMgr(QY_MESSENGER_ID idInfo, ref CONF_mgr_info pConfMgrInfo, ref bool pbRet)
        {
            int iErr = -1;
            bool bRet = false;

            QY_MESSENGER_ID* pIdInfo = &idInfo;

            if (null == pIdInfo || 0 == pIdInfo->ui64Id) return -1;
            //if (!pConfMgrInfo) return -1;

            int i;
            //
            for (i = 0; i < pConfMgrInfo.usCnt; i++)
            {
                if (pConfMgrInfo.mems_idInfo_ui64Id[i] == pIdInfo->ui64Id)
                {
                    bRet = true;
                    break;
                }
            }



            iErr = 0;

        errLabel:

            if (0 == iErr)
            {
                if (pbRet) pbRet = bRet;
            }

            return iErr;
        }

        public static unsafe int store_ConfMem_confCtrl(ref ConfMem_confCtrl pConfCtrl, QY_MESSENGER_ID pIdInfo)
        {
            int iErr = -1;
            /*
			QY_MC* pQyMc = QY_GET_GBUF();
			MC_VAR_isCli* pProcInfo = QY_GET_procInfo_isCli();
			MIS_CNT* pMisCnt = pProcInfo->getMisCntByName(_T(""));
			if (!pMisCnt) return -1;

			QM_OBJQ_MEM mem;
			QMEM_MESSENGER_CLI* pQMem = NULL;
			QY_MESSENGER_INFO messengerInfo;
			void* pDb = NULL;
			int i;

			CQnmDb tmpObjDb;
			pDb = tmpObjDb.getAvailableDb(pQyMc->iDsnIndex_mainSys);

			//
			if (!pConfCtrl) return -1;



			//  ÏÈÓÃregInfoÌî³äÒ»ÏÂmessengerInfo, ÒòÎªÏÂÃæµÄpClientÖ»ÄÜÖ¸ÏòmessengerInfo
			memset(&messengerInfo, 0, sizeof(messengerInfo));
			//
			//lstrcpyn(  messengerInfo.misServName,  regInfo.addr.misServName,  mycountof(  messengerInfo.misServName  )  );
			messengerInfo.idInfo.ui64Id = pIdInfo->ui64Id;//  regInfo.addr.idInfo.ui64Id;

			//               
			if (procQmObjQ(pProcInfo->pSgiParam, (QM_OBJQ*)pMisCnt->pObjQ, pDb, pQyMc->cfg.db.iDbType, CONST_objType_messenger, &messengerInfo, &mem)) goto errLabel;
			if (!mem.pQMemObj) goto errLabel;

			pQMem = (QMEM_MESSENGER_CLI*)mem.pQMemObj;

			//
			if (memcmp(pConfCtrl, &pQMem->confMem_confCtrl, sizeof(ConfMem_confCtrl)))
			{
				memcpy(&pQMem->confMem_confCtrl, pConfCtrl, sizeof(ConfMem_confCtrl));
			}
			*/

            iErr = 0;
        errLabel:


            return iErr;
        }

        public static unsafe int dlgTalk_displayAvStatus(object hDlg, DLG_TALK_var m_var, int iTaskId, int iStatus, string statusDesc)

        {
            return 0;
        }

        public static unsafe int chkResources(object hDlgTalk, bool bFreeCapImages)
        {
            /*
			CHelp_getDlgTalkVar help_getDlgTalkVar;
			HWND hCurTalk = hDlgTalk;
			DLG_TALK_var* pCurVar = (DLG_TALK_var*)help_getDlgTalkVar.getVar(hCurTalk, _T("chkRecvdResources"));
			if (!pCurVar) return -1;
			//
			CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;
			HWND hMgr = hCurTalk;
			DLG_TALK_var* pMgrVar = pCurVar;
			if (!isTalkerShadowMgr(pMgrVar->addr))
			{
				TALKER_shadow* pShadow = (TALKER_shadow*)pCurVar->pShadowInfo;
				hMgr = pShadow->hMgr;
				pMgrVar = (DLG_TALK_var*)help_getDlgTalkVar_mgr.getVar(hMgr);
				if (!pMgrVar) return -1;
			}
			//
			TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)pMgrVar->pShadowInfo;
			if (!pShadowMgr) return -1;
			//
			DLG_TALK_var & m_var = *pCurVar;

			//
			//
			INFO_recvResource info_recvdResource = { 0 };
			INFO_sendResource info_sendResource = { 0 };
			//
			BOOL bTaskExists = FALSE;

			//  bTaskExists  =  bTaskExists_recvdResource(  &m_var,  &nScreens,  &nMediaFiles,  NULL,  &iW_screen,  &iH_screen,  NULL  );
			bTaskExists = bTaskExists_recvResource1(pMgrVar, &info_recvdResource);
			if (bTaskExists != pShadowMgr->resourcesInfo.bTaskExists_recvd_screenAndMediaFile) pShadowMgr->resourcesInfo.bTaskExists_recvd_screenAndMediaFile = bTaskExists;
			//  2017/08/23
			if (memcmp(&pShadowMgr->resourcesInfo.info_recv, &info_recvdResource, sizeof(info_recvdResource))) pShadowMgr->resourcesInfo.info_recv = info_recvdResource;
			//
			bTaskExists_sendingResource1(pMgrVar, &info_sendResource);
			if (memcmp(&pShadowMgr->resourcesInfo.info_send, &info_sendResource, sizeof(info_sendResource))) pShadowMgr->resourcesInfo.info_send = info_sendResource;

			//
			//  2014/11/13		
			if (isTalkerShadowMgr(m_var.addr))
			{
				TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)m_var.pShadowInfo;
				if (pShadowMgr)
				{
					pShadowMgr->shareScreen.ucbExists_localScreenSharing = findTaskInProcessQ(&m_var, 0, 0, TRUE) != NULL;
				}
			}

			//  2014/12/09
			if (isTalkerShadowMgr(m_var.addr))
			{
				dlgTalk_chkAutoAnswer(hDlgTalk);
			}

			//  2012/05/22
			askToRefreshLayout_mgr(hDlgTalk);
			*/

            return 0;
        }

        //
        //public static unsafe int dlgTalk_OnQyComm(object hTalk, uint wParam, MIS_MSGU* lParam)
        public static unsafe int dlgTalk_OnQyComm(object hTalk, uint wParam, ref MIS_MSGU pComm)
        {
            int lRet = -1;

            if (hTalk == null) return -1;
            Activity_dlgTalk hDlg = (Activity_dlgTalk)hTalk;


            DLG_TALK_var m_var = hDlg.m_var;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            //
            //MIS_MSGU* pComm = (MIS_MSGU*)lParam;
            //TCHAR tBuf[512] = _T("");

            //
            //if (null == pComm) return -1;


            //
            //
            switch (pComm.uiType)
            {


                case Consts.CONST_misMsgType_taskStatus:
                    /*
					{
						MIS_MSG_taskStatus* pMsgTaskStatus = (MIS_MSG_taskStatus*)pComm;            //  ÓÐÊ±¾ÍÓÃÕâ¸ö½á¹¹£¬ 2007/07/17
						MIS_MSG_TASK* pMsgTask = null;
						IM_CONTENTU* pContent = null;
						int nItem;
						PARAM_PROC_ROW param;
						MIS_MSG_imMsgRcd msgImMsgRcd;
						int tmpiRet;
						//
						if (pMsgTaskStatus->iStatus == Consts.CONST_imTaskStatus_err_missingOrgReq)
						{
							//
							if (pMsgTaskStatus->uiContentType == CONST_imCommType_transferAudioData
								|| pMsgTaskStatus->uiContentType == CONST_imCommType_transferVideoData)
							{
								//  2016/02/17
								PARAM_findTaskAvOrgReq param_findTaskAvOrgReq;
								memset(&param_findTaskAvOrgReq, 0, sizeof(param_findTaskAvOrgReq));
								//
								tmpiRet = qTraverse(m_var.m_pProcessQ, tmpHandler_findTaskAvOrgReq, pMsgTaskStatus, &param_findTaskAvOrgReq);
								if (tmpiRet < 0) goto errLabel;
								if (tmpiRet)
								{

#if __DEBUG__
									traceLog(_T("dlgTalk::OnQyComm: taskAvOrgReq found. iStatus %d"), param_findTaskAvOrgReq.iStatus_o);
#endif
									//  2014/11/21
									pMsgTaskStatus->ucbFound = TRUE;
									pMsgTaskStatus->idInfo_logicalPeer.ui64Id = m_var.addr.idInfo.ui64Id;

									//  2014/07/30
									if (param_findTaskAvOrgReq.iStatus_o == CONST_imTaskStatus_autoCanceledByReceiver)
									{
										pMsgTaskStatus->ucbNeeded_applyForOrgReq = TRUE;
										//
										showInfo_open(0, 0, _T("dlgTalk_OnQyComm: taskAvOrgReq found. autoCanceledByReceiver, need apply for org req"));
									}
									else
									{  //  2015/10/08
									   //  2016/05/09
										if (bTaskAlive(param_findTaskAvOrgReq.iStatus_o))
										{
											if (param_findTaskAvOrgReq.uiTaskType == CONST_imTaskType_transferAvInfo)
											{
												if (!m_var.av.taskInfo.bTaskExists)
												{
													m_var.autoAnswer.bTaskExists = TRUE;
													//
													_sntprintf(tBuf, mycountof(tBuf), _T("dlgTalk_OnQyComm: taskAvOrgReq found. %s. transferAvInfo. no active avTask. auto answer"), qyGetDesByType1(CONST_qyStatusTable_en, param_findTaskAvOrgReq.iStatus_o));
													showInfo_open(0, 0, tBuf);
													//
													break;
												}
											}
										}

										//
										_sntprintf(tBuf, mycountof(tBuf), _T("dlgTalk_OnQyComm: taskAvOrgReq found. %s. do nothing"), qyGetDesByType1(CONST_qyStatusTable_en, param_findTaskAvOrgReq.iStatus_o));
										showInfo_open(0, 0, tBuf);

										//
										if (!bTaskAlive(param_findTaskAvOrgReq.iStatus_o))
										{
											if (!m_var.av.taskInfo.bTaskExists)
											{
												if (param_findTaskAvOrgReq.uiTaskType != CONST_imTaskType_transferAvInfo)
												{
													showInfo_open(0, 0, _T("taskType err: not av. "));
												}
												else
												{
													m_var.guiData.av.canceledTask_av.iTaskId = param_findTaskAvOrgReq.iTaskId;
													m_var.guiData.av.canceledTask_av.iStatus = param_findTaskAvOrgReq.iStatus_o;
													m_var.guiData.av.canceledTask_av.dwTickCnt_recentlyRefreshed = GetTickCount();
													//
													//  2017/09/14. 这里不让SHADOW最小化的原因是，当会议呼叫时，由于最小化和取消会议，导致用户无法察觉被呼叫的状态。
													DWORD dwTickCnt = GetTickCount();
													int iDiffInMs = dwTickCnt - m_var.guiData.av.canceledTask_av.dwTickCnt_chkShadowIconic;
													if (abs(iDiffInMs) > 3000)
													{
														m_var.guiData.av.canceledTask_av.dwTickCnt_chkShadowIconic = dwTickCnt;
														//
														if (isTalkerShadowMgr(m_var.addr))
														{
															TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)m_var.pShadowInfo;
															if (!pShadowMgr) goto errLabel;
															int i;
															for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
															{
																TALKER_shadow_mgrMem* pShadow = &pShadowMgr->shadows[i];
																if (!IsWindow(pShadow->hShadow)) continue;
																if (isTalkerSubtype_canInput(pShadow->iTalkerSubType))
																{
																	if (IsIconic(pShadow->hShadow))
																	{
																		PostMessage(pShadow->hShadow, WM_SYSCOMMAND, SC_RESTORE, 0);
																		break;
																	}
																}
															}
														}
													}
												}
												//
											}
											else
											{
												showInfo_open(0, 0, _T("Note: task waits to be re-accepted"));
												//
												//
												MIS_MSG_TASK* pMsgTask = findTaskInProcessQ(&m_var, param_findTaskAvOrgReq.iTaskId);
												if (pMsgTask)
												{
													IM_CONTENTU* pContent = M_getMsgContent(pMsgTask->ucFlg, &pMsgTask->data);
													if (pContent
														&& pContent->uiType == CONST_imCommType_transferAvInfo)
													{
														PROC_TASK_AV* pTask = get_transferAvInfo_pTask(&pContent->transferAvInfo, _T(""));
														if (!pTask) goto errLabel;
														//
														pTask->canceledTask_status.dwTickCnt_recentlyRefreshed = GetTickCount();
														//
														if (!pTask->canceledTask_status.bWaitingTo_beRe_accepted)
														{
															pTask->canceledTask_status.bWaitingTo_beRe_accepted = TRUE;
														}
														//
														D3D_talkerData_cfg* pDtc = &m_var.d3d_talkerData_cfg;
														int i;
														for (i = 0; i < pDtc->taskList.usCnt; i++)
														{
															TASK_list_mem* pTlm = &pDtc->taskList.mems[i];
															if (pTlm->iTaskId == param_findTaskAvOrgReq.iTaskId) break;
														}
														if (i == pDtc->taskList.usCnt)
														{
															dyn_d3dWall_refreshWallMemByTalker(hDlg);
														}
														//
														if (!m_var.guiData.av.canceledTask_screenOrMediaFile.bTaskExists_waitingTo_beRe_accepted)
														{
															m_var.guiData.av.canceledTask_screenOrMediaFile.bTaskExists_waitingTo_beRe_accepted = TRUE;
															dyn_d3dWall_askToRefreshIa();
														}
													}
												}
											}
										}

									}

									break;
								}

#if __DEBUG__
								traceLog(_T("dlgTalk::OnQyComm: taskAvOrgReq not found. need apply for org req"));
#endif
								showInfo_open(0, 0, _T("dlgTalk_OnQyComm: taskAvOrgReq not found. need apply for org req"));
								//
								pMsgTaskStatus->ucbNeeded_applyForOrgReq = TRUE;

							}
							//
							break;
						}
						else if (pMsgTaskStatus->iStatus == CONST_imTaskStatus_canceledByReceiver)
						{
							int ii = 0;
						}

						//
						dlgTalk_qPostMsg(hDlg, pMsgTaskStatus, sizeof(pMsgTaskStatus[0]));  //  2011/03/10.将可能改变任务状态的消息都要用postMsg的方式，发送到processQ.在那边统一顺序处理，有利于同步
				  ::PostMessage(hDlg, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);

						//
					}
					*/
                    break;
                case Consts.CONST_misMsgType_task:
                    {
                        ref MIS_MSG_TASK pMsg = ref pComm.task;
                        ref IM_CONTENTU pContent = ref pMsg.data.buf;// qyFuncs.M_getMsgContent(pMsg->uiType,pMsg->ucFlg, &pMsg->data);
                        int i;
                        //
                        /*
						switch (pContent->uiType)
						{
							case Consts.CONST_imCommType_transferAvInfo:
								if (pMsg->uiTaskType != Consts.CONST_imTaskType_viewDynBmp) goto errLabel;
								if (0==pContent->transferAvInfo.idInfo_logicalPeer.ui64Id) goto errLabel;
								//
								//confMem_procReq_transferAvInfo(  hDlg,  &m_var,  (  MIS_MSGU  *  )pMsg  );
								if (!isTalkerShadowMgr(ref m_var.addr)) goto errLabel;
								if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
								if (m_var.av.taskInfo.ucbStarter) goto errLabel;
								if (!m_var.av.ucbGot_iIndex_sharedObj_localAv) goto errLabel;
								confMem_procReq_transferAvInfo(hDlg, m_var.av.iIndex_sharedObj_localAv, (MIS_MSGU*)pMsg);

								//
								break;

							default:
#if __DEBUG__
								traceLogA("dlgTalk::OnQyComm: unprocessed contentType %d", pContent->uiType);
#endif
								break;
						}
						*/
                        //
                    }
                    break;
                case Consts.CONST_misMsgType_input:
                    {
                        ref MIS_MSG_INPUT pMsg = ref pComm.input;
                        ref IM_CONTENTU pContent = ref pMsg.data.buf; ;
                        int i;
                        //
                        //if (qyFuncs.isUcFlgRouteTalkData(pMsg->ucFlg) || qyFuncs.isUcFlgTalkData(pMsg->ucFlg)) pContent = (IM_CONTENTU*)&pMsg->data.buf;
                        //else pContent = (IM_CONTENTU*)&pMsg->data;
                        //
                        switch (pContent.uiType)
                        {
                            case Consts.CONST_imCommType_taskProcReq:
                                if (pContent.taskProcReq.usOp == Consts.CONST_imOp_recv_applyForOrgReq)
                                {
                                    using (CQyMalloc malloc_pMsgTask_o = new CQyMalloc())
                                    {
                                        ;
                                        int size = sizeof(MIS_MSG_TASK);
                                        //MIS_MSG_TASK* pMsgTask_o = (MIS_MSG_TASK*)malloc_pMsgTask_o.mallocf(size);
                                        CMIS_MSGU pMsgTask_o = new CMIS_MSGU();
                                        if (null == pMsgTask_o) goto errLabel;
                                        //qyFuncs.MACRO_memset_misMsgTask(pMsgTask_o);

                                        //
                                        int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(mainActivity.var_common.ctxCaller, pProcInfo, pMsg.iTaskId);
                                        if (index_taskInfo < 0) goto errLabel;
                                        QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(mainActivity.var_common.ctxCaller, pProcInfo, index_taskInfo, "dlgTalk_onQyComm");
                                        if (pTaskInfo == null) goto errLabel;
                                        QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
                                        if (pTaskData == null) goto errLabel;

                                        //
                                        fixed (MIS_MSG_TASK* pMsgTask_i = &pTaskData.msgU.task)
                                        {
                                            ;// findTaskInProcessQ(&m_var, pMsg->iTaskId);
                                            if (null == pMsgTask_i) goto errLabel;
                                            if (pMsgTask_i->uiType != Consts.CONST_misMsgType_task) goto errLabel;

                                            IM_CONTENTU* pContentTask_i = (IM_CONTENTU*)&pMsgTask_i->data.buf;
                                            if (pContentTask_i->uiType != Consts.CONST_imCommType_transferAvInfo) goto errLabel;
                                            //
                                            if (true)
                                            {   //  2012/05/25
                                                bool bPermitted = false;
#if __DEBUG__
										traceLog(_T("recvd applyForOrgReq. to check whether the from is a valid user. %I64u"), pMsg->data.route.idInfo_from.ui64Id);
#endif

                                                if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
                                                {
                                                    //  要检查是否是组成员
                                                    /*
													BOOL bVal = FALSE;
													if (!getVal_bImGrpMem(&m_var.addr.idInfo, &pMsg->data.route.idInfo_from, &bVal)
														&& bVal)
													{
														bPermitted = TRUE;
													}
													*/
                                                    bPermitted = true;
                                                }
                                                else
                                                {
                                                    if (pMsg.data.route.idInfo_from.ui64Id == m_var.addr.idInfo.ui64Id)
                                                    {
                                                        bPermitted = true;
                                                    }
                                                }
                                                //
                                                if (!bPermitted)
                                                {
#if __DEBUG__
											traceLog(_T("applyForOrgReq denied. Not a valid user."));
#endif
                                                    goto errLabel;
                                                }
                                            }
                                            //
                                            if (m_var.addr.uiObjType != Consts.CONST_objType_imGrp
                                                && pTaskInfo.var.m_bReplyTask)
                                            {
                                                //
                                                if (dlgTalk_canSpeak(hTalk))
                                                {
                                                    dlgTalk_requestToSpeak(hTalk, true);
                                                }

                                                break;
                                            }
                                            //
                                            PARAM_findOrgMsg param = new PARAM_findOrgMsg();
                                            //memset(&param, 0, sizeof(param));
                                            param.pMsgTask_o = pMsgTask_o;
                                            if (pProcInfo.processQ_media.qTraverse(mainActivity.var_common.ctxCaller, qyFuncs.tmpHandler_findOrgMsgAndProc_is, ref *(MIS_MSGU*)pMsgTask_i, param) <= 0)
                                            {
#if __DEBUG__
										traceLogA("dlgTalk_OnQyComm: taskProcReq: applyForOrgReq failed.");
#endif
                                                goto errLabel;
                                            }

                                            if (pMsgTask_o.msgU.uiType != Consts.CONST_misMsgType_task)
                                            {
#if __DEBUG__
										traceLogA("dlgTalk_OnQyComm: taskProcReq: applyForOrgReq failed. msg.type %d err", pMsgTask_o->uiType);
#endif
                                                goto errLabel;
                                            }
                                            ref IM_CONTENTU pContentTask_o = ref pMsgTask_o.msgU.task.data.buf;
                                            if (pContentTask_o.uiType != Consts.CONST_imCommType_transferAvInfo) goto errLabel;

#if __DEBUG__
									traceLogA("dlgTalk_OnQyComm: send transferAvInfo again.");
#endif
                                            //
                                            int lenInBytes = (int)pMsgTask_o.msgU.task.lenInBytes - (int)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_routeTalkData>("buf");
                                            if (lenInBytes < 0) goto errLabel;
                                            //
                                            QY_MESSENGER_ID idInfo_dst;
                                            idInfo_dst.ui64Id = pMsg.data.route.idInfo_from.ui64Id;
                                            //  set idInfo_logicalPeer temporiary.
                                            if (m_var.addr.idInfo.ui64Id != idInfo_dst.ui64Id)
                                            {
                                                pContentTask_o.transferAvInfo.idInfo_logicalPeer.ui64Id = m_var.addr.idInfo.ui64Id;
                                            }
                                            QY_MESSENGER_ID idInfo_logicalPeer = m_var.addr.idInfo;
                                            fixed (IM_CONTENTU* buf = &pMsgTask_o.msgU.task.data.buf)
                                            {
                                                qyFuncs.postMsgTask2Mgr_mc(mainActivity.var_common.ctxCaller, pMisCnt, Consts.CONST_misMsgType_outputTask, 0, pMsgTask_o.msgU.task.usCode,
                                                    pMsgTask_o.msgU.task.tStartTime, pMsgTask_o.msgU.task.uiTranNo, 0, pMsgTask_o.msgU.task.iTaskId,
                                                    pMsgTask_o.msgU.task.uiTaskType, (byte*)buf, (uint)lenInBytes, idInfo_logicalPeer.ui64Id,
                                                    pMsgTask_o.msgU.task.idInfo_taskSender.ui64Id, pMsgTask_o.msgU.task.idInfo_taskReceiver.ui64Id, idInfo_dst.ui64Id, 0, null, false);
                                            }
                                            //pContentTask_o->transferAvInfo.idInfo_logicalPeer.ui64Id  =  0;

                                            //
                                            if (isTalkerShadowMgr(ref m_var.addr))
                                            {
                                                if (m_var.av.taskInfo.bTaskExists
                                                    && m_var.av.taskInfo.iTaskId == pMsg.iTaskId)
                                                {
                                                    sendConfKey(hDlg, idInfo_dst, _T("send orgReq"));
                                                }
                                            }

                                            //
                                            string str;
                                            str = string.Format("dlgTalk: send transferAvInfo again to {0}. tn {1}", idInfo_dst.ui64Id, pMsgTask_o.msgU.task.uiTranNo);
                                            uint step = 0;
                                            qyFuncs.qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), pProcInfo.str_who_showInfo, null, _T(""), _T(""), str);

                                            //
                                            break;
                                        }
                                    }
                                }
                                //  2014/10/09
                                /*
								if (pContent->taskProcReq.usOp == CONST_imOp_stopSending
									|| pContent->taskProcReq.usOp == CONST_imOp_requestToSend)
								{
									if (!m_var.av.taskInfo.bTaskExists) goto errLabel;
									if (!m_var.av.ucbGot_iIndex_sharedObj_localAv) goto errLabel;
									QY_SHARED_OBJ* pSharedObj = getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
									if (!pSharedObj) goto errLabel;
									if (pSharedObj->bSlave) goto errLabel;
									SHARE_dyn_bmp* pDynBmpMem = (SHARE_dyn_bmp*)pSharedObj->master.pShareDynBmp_slave;
									if (!pDynBmpMem) goto errLabel;

									if (pDynBmpMem->resObj.uiObjType != pContent->taskProcReq.viewDynBmp.uiObjType
										|| pDynBmpMem->resObj.usIndex_obj != pContent->taskProcReq.viewDynBmp.usIndex_obj)
									{
										goto errLabel;
									}

									QY_SHARED_OBJ* pSharedObj_slave = getSharedObjByIndex(pProcInfo, pSharedObj->master.slaveKey.iIndex_sharedObj);
									if (!pSharedObj_slave) goto errLabel;
									if (!pSharedObj_slave->bUsed) goto errLabel;
									if (!pSharedObj_slave->bSlave) goto errLabel;
									if (pSharedObj_slave->uiTranNo != pSharedObj->master.slaveKey.uiTranNo) goto errLabel;

									if (pContent->taskProcReq.usOp == CONST_imOp_stopSending)
									{
										removeFromMsgrs_sendLocalAv(pProcInfo, m_var.pMisCnt, &pMsg->data.route.idInfo_from, &pSharedObj_slave->curRoute_sendLocalAv);
										qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("dlgTalk_OnQyComm: slave: removeFromMsgrs_sendLocalAv, %I64u"), pMsg->data.route.idInfo_from);
									}

									break;
								}
								*/
                                break;
                            case Consts.CONST_imCommType_taskProcResp:
                                /*
								if (pContent->taskProcResp.usOp_org == Consts.CONST_imOp_recv_applyForChkTaskAlive)
								{
									Q_NODE* pQNode;
									MIS_MSGU* pMsg;

									if (!m_var.m_pProcessQ) goto errLabel;

									for (pQNode = m_var.m_pProcessQ->front.pNext; pQNode; pQNode = pQNode->pNext)
									{
										pMsg = (MIS_MSGU*)pQNode->qElemMemory.m_pBuf;
										if (pMsg->uiType != CONST_misMsgType_task) continue;
										if (pMsg->task.tStartTime == pContent->taskProcResp.tStartTime_org
											&& pMsg->task.uiTranNo == pContent->taskProcResp.uiTranNo_org)
										{
											if (bTaskAlive(pContent->taskProcResp.iStatus))
											{
												//
												_sntprintf(tBuf, mycountof(tBuf), _T("resp recvd, task %d is alive"), pMsg->task.iTaskId);
												showInfo_open(0, 0, tBuf);
#if __DEBUG__
												traceLog(tBuf);
#endif
												//
												//  mytime(  &pMsg->task.tStartTime_processed  );
												mytime(&pMsg->task.tTime_alive);    //  2014/12/09
											}
											break;
										}
									}
								}
								*/
                                break;
                            default:
                                break;
                        }

                        break;

                    }
                /*

case CONST_misMsgType_notifyTaskEnd_qmc:
    {
        MIS_MSG_notifyTaskEnd_qmc* pMsg = (MIS_MSG_notifyTaskEnd_qmc*)pComm;

        dlgTalk_qPostMsg(hDlg, pMsg, sizeof(pMsg[0]));  //  2011/03/10.将可能改变任务状态的消息都要用postMsg的方式，发送到processQ.在那边统一顺序处理，有利于同步
  ::PostMessage(hDlg, CONST_qyWm_postComm, CONST_qyWmParam_msgArrive, 0);


    }
    break;

case CONST_qyWmParam_chkTask:
    if (dlgTalk_toChkTask(hDlg, m_var, pComm)) goto errLabel;
    break;
case CONST_qyWmParam_chkSharedObject:       //  2014/11/30
    if (dlgTalk_toChkSharedObj(hDlg, m_var, pComm)) goto errLabel;
    break;

case CONST_qyWmParam_requestToSpeak:
    {       //  2010/09/02
        if (!m_var.av.taskInfo.bTaskExists) break;
        //
        if (!pComm->u.requestToSpeak.bManager && pComm->u.requestToSpeak.uiCmd == ID_stopSpeaking)
        {
            dlgTalk_requestToSpeak(hDlg, FALSE);
        }
    }
    break;


case CONST_misMsgType_applyForTalkerShadow_qmc:
    {   //  2012/04/23
        MIS_MSG_applyForTalkerShadow_qmc* pMsg = (MIS_MSG_applyForTalkerShadow_qmc*)lParam;
        //
        if (isTalkerShadowMgr(m_var.addr))
        {       //  这里的处理是影子窗口的放大窗口或者变幻窗口
            goto errLabel;
        }
        TALKER_shadow* pTalkerShadow = (TALKER_shadow*)m_var.pShadowInfo;
        if (!pTalkerShadow) goto errLabel;

        if (!IsWindow(pTalkerShadow->hEnlargedWnd))
        {
            pTalkerShadow->hEnlargedWnd = pMsg->hShadow;
            MACRO_SetForegroundWindow(pTalkerShadow->hEnlargedWnd);
        }

    }
    break;

                */

                default:
#if __DEBUG__
					traceLog(_T("dlgTalk_OnQyComm failed, unknown commType %d"), pComm->uiType);
#endif
                    goto errLabel;
            }


            lRet = Consts.CONST_qyWmRc_ok;

        errLabel:
            return lRet;

        }

        //
        public static unsafe int dlgTalk_refresh_usLayoutType_cur(object hDlgTalk, bool* pbModified)
        {
            int iErr = -1;
#if false
			QY_MC* pQyMc = QY_GET_GBUF();
			CHelp_getDlgTalkVar help_getDlgTalkVar;
			DLG_TALK_var* pDlgTalkVar = (DLG_TALK_var*)help_getDlgTalkVar.getVar(hDlgTalk, _T("dlgTalk_refresh_usLayoutType_cur"));
			if (!pDlgTalkVar) return -1;
			unsigned  short usLayoutType_cur = 0;
			//
			BOOL bScroll = FALSE;
			//  2013/06/25. 如果有远程视频的指令,就算一个活动任务
			BOOL bExists_remoteVideo = FALSE;
			//
			BOOL bModified = FALSE;

			CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;

			HWND hMgr = hDlgTalk;
			DLG_TALK_var* pMgrVar = pDlgTalkVar;
			if (!isTalkerShadowMgr(pDlgTalkVar->addr))
			{
				TALKER_shadow* pShadow = (TALKER_shadow*)pDlgTalkVar->pShadowInfo;
				if (!pShadow) goto errLabel;
				hMgr = pShadow->hMgr;
				pMgrVar = (DLG_TALK_var*)help_getDlgTalkVar_mgr.getVar(pShadow->hMgr, _T("dlgTalk_refresh_usLayoutType_cur,hMgr"));
				if (!pMgrVar) goto errLabel;
			}

			//
			TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)pMgrVar->pShadowInfo;
			if (!pShadowMgr) goto errLabel;

			//  2013/02/28
			unsigned  short usLayoutType_local = dlgTalk_get_usLayoutType_local(hDlgTalk);

			//
			int nScreens = 0, nMediaFiles = 0;
			int nMediaFiles_unresizable = 0;                                            //  2015/03/12
			int iW_unresizable = 0, iH_unresizable = 0;
			BOOL bExists_recvdResource = FALSE;
			//
			int nScreens_sendingResource = 0, nMediaFiles_sendingResource = 0;  //  2015/03/12
			int nMediaFiles_unresizable_sendingResource = 0;
			int iW_unresizable_sendingResource = 0, iH_unresizable_sendingResource = 0; //  2015/03/12
			BOOL bExists_sharingResource = FALSE;   //  2014/11/14


			//
			//
			int nTasks_remoteVideo = 0;
			int nTasks_unresizable_remoteVideo = 0;
			int iW_unresizable_remoteVideo = 0;
			int iH_unresizable_remoteVideo = 0;
			bExists_remoteVideo = bTaskExists_remoteVideo_manual(pMgrVar, &nTasks_remoteVideo, &nTasks_unresizable_remoteVideo, &iW_unresizable_remoteVideo, &iH_unresizable_remoteVideo);


			//  2014/11/18
			BOOL bDone = FALSE;

			//  2014/11/16
			if (pMgrVar->av.taskInfo.bTaskExists)
			{
				if (pMgrVar->av.taskInfo.ucbVideoConference
					&& !pMgrVar->av.taskInfo.ucbStarter)
				{
					if (pMgrVar->av.taskInfo.mosaicTrans.bExists_confMosaic)
					{
						usLayoutType_cur = CONST_usLayoutType_dialog;
						if (pMgrVar->av.taskInfo.mosaicTrans.status.bDisplayResource
							&& !bExists_remoteVideo
							&& !pShadowMgr->shareScreen.ucbExists_localScreenSharing    //  2016/04/24
							)
						{
							usLayoutType_cur = CONST_usLayoutType_resourceOnly;

							//
							bScroll = TRUE;
							//
							iW_unresizable = pMgrVar->av.taskInfo.mosaicTrans.status.iW_resource;
							iH_unresizable = pMgrVar->av.taskInfo.mosaicTrans.status.iH_resource;

							//			
							if (iW_unresizable != pDlgTalkVar->m_layout.uiW_pic || iH_unresizable != pDlgTalkVar->m_layout.uiH_pic)
							{
								dlgTalk_SetPicRect(hDlgTalk, iW_unresizable, iH_unresizable, FALSE);
								bModified = TRUE;
							}

						}
						//
						bDone = TRUE;
					}
				}
			}

			//
			if (!bDone)
			{
				//
				bExists_recvdResource = bTaskExists_recvResource(pMgrVar, &nScreens, &nMediaFiles, &nMediaFiles_unresizable, &iW_unresizable, &iH_unresizable, NULL);

				//  2014/11/15
				bExists_sharingResource = bTaskExists_sendingResource(pMgrVar, &nScreens_sendingResource, &nMediaFiles_sendingResource, &nMediaFiles_unresizable_sendingResource, &iW_unresizable_sendingResource, &iH_unresizable_sendingResource, NULL);

				//  2014/11/14
				if (pShadowMgr->shareScreen.ucbExists_localScreenSharing)
				{
					bExists_sharingResource = TRUE;
				}

				//
				int i;

				for (i = 0; i < 1; i++)
				{

					//
					if (pMgrVar->av.taskInfo.bTaskExists)
					{

						//
						if (usLayoutType_local)
						{
							usLayoutType_cur = usLayoutType_local;
							break;
						}

						//
						if (pMgrVar->av.taskInfo.usLayoutType_starter)
						{
							usLayoutType_cur = pMgrVar->av.taskInfo.usLayoutType_starter;
							break;
						}

						//  2015/10/25
						if (bExists_remoteVideo)
						{
							usLayoutType_cur = CONST_usLayoutType_training;
							break;
						}

						//
						if (bExists_recvdResource || bExists_sharingResource)
						{
							usLayoutType_cur = CONST_usLayoutType_training;
							break;
						}


						//
						usLayoutType_cur = CONST_usLayoutType_dialog;
						break;
					}

					//  2012/05/24		
					if (usLayoutType_local)
					{
						usLayoutType_cur = usLayoutType_local;
						break;
					}

					//  2015/10/23
					if (bExists_remoteVideo)
					{
						usLayoutType_cur = CONST_usLayoutType_resourceOnly;
						break;
					}

					//
					if (bExists_recvdResource || bExists_sharingResource)
					{
						usLayoutType_cur = CONST_usLayoutType_resourceOnly;
						break;
					}

					//
					usLayoutType_cur = CONST_usLayoutType_dialog;
					break;
				}

				//
				if (usLayoutType_cur == CONST_usLayoutType_resourceOnly
					|| usLayoutType_cur == CONST_usLayoutType_training)
				{
					BOOL bNeedScroll = FALSE;
					int iW_scroll = 0;
					int iH_scroll = 0;
					//
					if (!bExists_remoteVideo
						&& !bExists_sharingResource
						&& bExists_recvdResource
						&& ((nScreens == 1 && !nMediaFiles) || (!nScreens && nMediaFiles == 1 && nMediaFiles_unresizable == 1))
						&& iW_unresizable
						&& iH_unresizable)
					{
						bNeedScroll = TRUE;
						iW_scroll = iW_unresizable;
						iH_scroll = iH_unresizable;
					}
					//
					if (!bNeedScroll)
					{
						if (!bExists_remoteVideo
							&& bExists_sharingResource
							&& !bExists_recvdResource
							&& !nScreens_sendingResource
							&& nMediaFiles_sendingResource == 1
							&& nMediaFiles_unresizable_sendingResource == 1
							&& iW_unresizable_sendingResource
							&& iH_unresizable_sendingResource)
						{
							bNeedScroll = TRUE;
							iW_scroll = iW_unresizable_sendingResource;
							iH_scroll = iH_unresizable_sendingResource;
						}
					}
					//  2015/10/23
					if (!bNeedScroll)
					{
						if (bExists_remoteVideo
							&& !bExists_recvdResource
							&& !bExists_sharingResource)
						{
							if (nTasks_remoteVideo == 1
								&& nTasks_unresizable_remoteVideo == 1)
							{
								bNeedScroll = TRUE;
								iW_scroll = iW_unresizable_remoteVideo;
								iH_scroll = iH_unresizable_remoteVideo;
							}

						}
					}
					//
					if (bNeedScroll)
					{
						bScroll = TRUE;

						if (iW_scroll != pDlgTalkVar->m_layout.uiW_pic || iH_scroll != pDlgTalkVar->m_layout.uiH_pic)
						{
							dlgTalk_SetPicRect(hDlgTalk, iW_scroll, iH_scroll, FALSE);
							bModified = TRUE;
						}
					}
				}

			}

			//  result:
			if (bScroll != pDlgTalkVar->m_scrollInfo.bScrollBarEnabled)
			{
				dlgTalk_bEnableScrollBar(hDlgTalk, bScroll);
				bModified = TRUE;
			}
			//
#if __DEBUG__
#endif

			//
			if (pDlgTalkVar->m_layout.usLayoutType_cur != usLayoutType_cur)
			{
				pDlgTalkVar->m_layout.usLayoutType_cur = usLayoutType_cur;
				bModified = TRUE;
			}

			//  2015/11/14
			if (pDlgTalkVar->m_layout.ucbExists_localScreenSharing != pShadowMgr->shareScreen.ucbExists_localScreenSharing)
			{
				pDlgTalkVar->m_layout.ucbExists_localScreenSharing = pShadowMgr->shareScreen.ucbExists_localScreenSharing;
				bModified = TRUE;
			}

			//  2014/11/15
			if (!bExists_sharingResource && !pMgrVar->av.taskInfo.bTaskExists)
			{
				HWND hCtrl_meDesc = GetDlgItem(hDlgTalk, pDlgTalkVar->guiData.iIDC_STATIC_meDesc);
				if (IsWindowVisible(hCtrl_meDesc))
				{
					bModified = TRUE;
#if __DEBUG__
					traceLog(_T("refresh_usLayoutType_cur: meDesc need be invisible"));
#endif
				}
			}

			//  2015/10/25
			//  if  (  !bExists_sharingResource  )  
			{
				HWND hCtrl_peerDesc = GetDlgItem(hDlgTalk, pDlgTalkVar->guiData.iIDC_STATIC_peerDesc);
				if (IsWindowVisible(hCtrl_peerDesc))
				{
					bModified = TRUE;
#if __DEBUG__
					traceLog(_T("refresh_usLayoutType_cur: peerDesc need be invisible"));
#endif
				}
			}

			//  2014/11/18
			if (pShadowMgr->shareScreen.ucbExists_localScreenSharing)
			{
				TCHAR tBuf[128] = _T("");
				_sntprintf(tBuf, mycountof(tBuf), _T("\n\n    %s"), getResStr(0, &pQyMc->cusRes, CONST_resId_sharingLetWinBeSmaller));
				SetDlgItemText(hDlgTalk, pDlgTalkVar->guiData.iIDC_STATIC_peerDesc, tBuf);
			}
			else
			{
				SetDlgItemText(hDlgTalk, pDlgTalkVar->guiData.iIDC_STATIC_peerDesc, _T(""));
			}

			//
			if (bModified)
			{
				dlgTalk_doLayout(hDlgTalk);
			}

			iErr = 0;

		errLabel:

			if (!iErr)
			{
				if (pbModified) *pbModified = bModified;
				//
				if (bModified)
				{
					dyn_d3dWall_refreshImgs(hMgr);
				}
			}
#endif

            return iErr;
        }


        //
        public static void dlgTalk_myTimerProc(object hDlgTalk, uint uMsg, uint idEvent, uint dwTime)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            int len = 0;

            bool bDbg = false;
            string str;

#if DEBUG
            //bDbg = true;
#endif
            //bDbg = true;


            //
            if (bDbg)
            {
                str = string.Format("dlgTalk_myTimeProc: talker{0}, enters", dlgTalk.m_var.addr.idInfo.ui64Id);
                qyFuncs.showInfo_open(0, null, str);
            }


            //
            //using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            {
                ;
                //Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;
                DLG_TALK_var pm_var = dlgTalk.m_var;// (DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk, _T("dlgTalk_mytimeProc"));
                if (null == pm_var)
                {
          return;
                }
                ref DLG_TALK_var m_var = ref pm_var;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
                if (null == pProcInfo) return;


                uint dwTickCnt_start_myTimerProc = qyFuncs.myGetTickCount(null);


                m_var.loopCtrl_timer++;

            errLabel:
                //
                if (isTalkerShadowMgr(ref m_var.addr))
                {
                    if (pProcInfo.cfg.debugStatusInfo.ucbShowDlgTalkStatus)
                    {
                        uint dwTickCnt_end_myTimerProc = qyFuncs.myGetTickCount(null);
                        int iDiffInMs = (int)(dwTickCnt_end_myTimerProc - dwTickCnt_start_myTimerProc);
                        str = string.Format("dlgTalk_myTimeProc: talker{0}, nElapseInMs {1}ms", m_var.addr.idInfo.ui64Id, iDiffInMs);
                        qyFuncs.showInfo_open(0, null, str);
                    }
                }


                if (bDbg)
                {
                    qyFuncs.showInfo_open(0, null, _T("dlgTalk_myTimeProc leaves"));
                }

                
            }

            
            return;
        }



    }

}





