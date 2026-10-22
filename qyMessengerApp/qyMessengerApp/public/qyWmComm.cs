using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

partial class Consts
{
    public const int WM_USER = 0x0400;
    public const int CONST_Status_Window_Function_Open = -1; //  ，网络连接日志打开0.其余状态窗关闭
    public const int CONST_REQUEST_VIDEO_PERMISSIONS = 0; //  ，网络连接日志打开0.其余状态窗关闭
    public const int REQUEST_AUDIO_PERMISSIONS = 1; //  ，网络连接日志打开0.其余状态窗关闭
    public const int REQUEST_VIDEO_PERMISSIONS = 2; //  ，网络连接日志打开0.其余状态窗关闭

    public const int CONST_qyWm_comm = (WM_USER + 11);
    public const int QY_WM_COMM = CONST_qyWm_comm;          //  只能用SendMessage( )发送
    public const int QY_WM_PUTDATA = WM_USER + 13;          //  只能用SendMessage( )发送
    public const int CONST_qyWm_postComm = (WM_USER + 14);  //  主要用PostMessenger(  )发送，用wParam, lParam来表示发送信息
    public const int CONST_qyWm_trayNotify = (WM_USER + 15);    //  系统托盘

    public const int CONST_qyWm_graphNotify_mediaFile = (WM_USER + 16); //  2009/05/05
    public const int CONST_qyWm_graphNotify_mediaDevice = (WM_USER + 17);   //  2012/02/27


    public const int CONST_qyWmParam_requestToSpeak = 38;           //  2009/02/24
    public const int CONST_qyWmParam_chkTask = 39;          //  2009/09/10
    public const int CONST_qyWmParam_chkSharedObject = 40;          //  2009/09/13
    public const int CONST_qyWmParam_freeCapImage = 41;         //  2010/09/12
    public const int CONST_qyWmParam_freeCapImages = 42;            //  2010/12/27

    //  2015/08/03
    public const int CONST_qyWmParam_procOfflineRes = 70;           //  2015/08/03
    public const int CONST_qyWmParam_command = 71;          //  2016/05/24

    //
    //  CONST_qyWm_postComm wParam
    public const int CONST_qyWmParam_setActiveWindow = 201;         //  
    public const int CONST_qyWmParam_idcInited = 202;           //  
    public const int CONST_qyWmParam_refreshContent = 203;          //  
    public const int CONST_qyWmParam_refreshLayout = 204;           //  2010/12/22

    //
    public const int CONST_qyWmParam_msgArrive = 210;           //  
    public const int CONST_qyWmParam_guiMsgArrive = 211;            //  2009/03/24
    public const int CONST_qyWmParam_gpsMsgArrive = 212;            //  2012/04/20

    //
    public const int CONST_qyWmParam_avRecord_start = 360;          //  2008/10/05, 
    public const int CONST_qyWmParam_task_end = 362;            //

    //
    //  public const int		CONST_qyWmParam_chkTmpMemorys						363			//  2010/09/17

    public const int CONST_qyWmParam_broadcastRecorderStatus = 365;         //  2009/02/27
    public const int CONST_qyWmParam_notifyQmShmCmdStatus = 366;            //  2009/12/21
    public const int CONST_qyWmParam_mouseMove = 367;           //  2009/12/29
    public const int CONST_qyWmParam_notifyProgressEnd = 368;           //  2010/08/14
    public const int CONST_qyWmParam_bNeedRefresh_activeMems_from = 369;            //  2012/04/01
    public const int CONST_qyWmParam_gps = 370;         //  2012/04/18
    public const int CONST_qyWmParam_curSharedObjUsrChanged_mgr = 371;          //  2012/05/12
    public const int CONST_qyWmParam_curSharedObjUsrChanged_shadow = 372;           //  2012/05/12
    public const int CONST_qyWmParam_toStartLocalAudioPlayer = 373;         //  2013/05/26
    public const int CONST_qyWmParam_doPost_drawCapImages = 374;            //  2014/04/12
    public const int CONST_qyWmParam_invalidate_bgWall = 375;           //  2016/03/15
    public const int CONST_qyWmParam_freeCapImageBySth_bgWall = 376;
    public const int CONST_qyWmParam_misMsgInited = 377;            //  2018/10/30
    public const int CONST_qyWmParam_confAccepted = 378;            //  2018/11/01

    //
    public const int CONST_qyWmParam_endThread = 402;           //  2010/05/03

    //
    public const int CONST_qyWmParam_login_canceled = 500;
    public const int CONST_qyWmParam_closeWnd = 501;
    public const int CONST_qyWmParam_threadProc_isCli_end = 502;
    public const int CONST_qyWmParam_doClean = 503;

    //
    public const int CONST_qyWmParam_allContactsRetrieved = 511;
    public const int CONST_qyWmParam_imObj_needRefreshed = 512;
    //
    public const int CONST_qyWmParam_doTask = 522;
    public const int CONST_qyWmParam_chkTalkDraw = 523;
    //
    public const int CONST_qyWmParam_showHint = 524;

    public const int CONST_qyWmParam_doTask_av_accept = 525;



    //  2016/06/29. qisMsg_open.h
    //  CONST_qisMsg_ex_min		3000
    //  CONST_qisMsg_ex_max		3999


    //

    public const int CONST_qyWmParam_misMsg_base = 5000;        //  misMsg wmBuf iType

    // 以下QY_WM_*的返回值
    public const int CONST_qyWmRc_ok = 6543;
    public const int CONST_qyWmRc_err = -1;
    public const int CONST_qyWmRc_delay = 6544;
    public const int QY_RC_WMOK = CONST_qyWmRc_ok;      // 成功
    public const int QY_RC_WMDELAY = CONST_qyWmRc_delay;	// 随后再发响应信息
    public const int Video_Status_Enlargement = 1;//视频放大
    public const int Video_Status_Normal = 0;//视频正常

}

namespace qy
{

}
