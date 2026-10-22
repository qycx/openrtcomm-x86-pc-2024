using System;
using System.Collections.Generic;
using System.Text;

//
partial class Consts
{
    //
    //public const string CONST_subDir_isCli = "isCli";
    public const string CONST_cfgFile_login_isCli = "login_isCli.cfg";
    public const string CONST_cfgFile_dbg_isCli = "dbg_isCli.cfg";


    //
    public const string CONST_cfgName_server = "server";
    public const string CONST_cfgName_usr = "usr";
    public const string CONST_cfgName_passwd = "passwd";

    //
    public const string CONST_cfgName_statusServer = "statusServer";


    //
    public const int CONST_msg_cameraOpen = 10000;




    //
    public const int CONST_iFgType_dlgTalk_av = 1;
    public const int CONST_iFgType_dlgTalk_av2 = 2;

    //
    public const float CONST_fz_z1 = CONST_fz_me;//1f - 0.0000001f;
    public const float CONST_fz_peer = 1f - 0.000001f;
    public const float CONST_fz_me = 1f - 0.000002f;




}

//
namespace qy
{
    public delegate long PF_myGetTickCount();
    public delegate void PF_myShowChangePassWordSuccess();

    public delegate int PF_myGetPid();
    public delegate int PF_myGetTid();

    public delegate int PF_startThread1(ref object obj_ph, PF_threadProc pf, object param);
    public delegate void PF_waitForThread1(ref object obj_ph, uint dwMilliseconds);

    //
    public delegate void PF_getSystemInfo(ref int iManufactor, ref int iModel, ref bool b_isHarmony);
    public delegate int PF_customSystem();


    //
    public delegate bool PF_bQySockOpen(ref QY_SOCK pSock);
    public delegate int PF_qyConnect(string ipaddr, int port, ref SOCK_PARAM pSockParam, ref QY_SOCK pSock);
    public delegate void PF_qyDisconnect(ref QY_SOCK pSock);
    public unsafe delegate int PF_qySend(ref QY_SOCK pSock, byte* buf, int len, int timeoutVal);
    public unsafe delegate int PF_qyRecv(ref QY_SOCK pSock, byte* buf, int* pLen, int timeoutVal);
    public delegate int PF_qyWaitForRead(ref QY_SOCK pSock, int timeoutInMs);
    public delegate int PF_qyWaitForWrite(ref QY_SOCK pSock, int timeoutInMs);

    //
    public delegate int PF_PostMessage(object wnd, int msgId, int wParm, int lParam);
    public delegate bool PF_IsWindow(object hWnd);
    //
    public delegate int PF_PostMessage_new(object wnd, int iWndContenType, int msgId, int wParm, int lParam);
    //
    public delegate object PF_mainWnd_getVar();
    //
    public delegate int PF_gui_procMsg(ref MIS_MSGU pMsgParam);
    public delegate int PF_gui_dlgTalk_procMsg(object hTalkParam, ref MIS_MSGU pMsgParam);
    //
    public delegate int PF_gui_notify_cancel();
    public delegate int PF_gui_notify_chk();
    //
    public delegate int PF_gui_notify_doCmd(ushort usOp);
    //
    public delegate int PF_mainWnd_setTimer();
    public delegate void PF_mainWnd_killTimer();

    //
    public delegate int PF_findTalker(CCtxQyMc pQyMc, MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, ref object phWnd);
    public delegate int PF_dlgTalk_qPostMsg(object hDlgTalk, ref MIS_MSGU  pMsg, uint size);

    //
    public delegate int PF_gui_displayNotification(CCtxQmc pProcInfo, IntPtr pMIS_MSG_taskStatus);

    //
    public delegate int PF_testInMain(CCtxQyMc pQyMc);

    //
    public delegate int PF_dumpData(byte[] data, int w, int h, bool bAppend, bool bFileName_haveTime, string pureFileName);
    public delegate int PF_dumpData1(IntPtr data, int len, int w, int h, bool bAppend, bool bFileName_haveTime, string pureFileName);
    public delegate int PF_dumpRgb32(byte[] data, int w, int h, string pureFileName);
    public delegate int PF_dumpRgb32_1(IntPtr data, int w, int h, string pureFileName);
    public delegate int PF_dumpBmp(object bitmap, string pureFileName);

    //
    public delegate int PF_dumpAudio_init();
    public delegate int PF_dumpAudio_exit();
    public delegate int PF_dumpAudio(IntPtr data, int size);




    //
    public delegate int PF_logd(string tag, string str);


    //
    public delegate int PF_initPolicyAvParams();  //  要在登陆后立即运行此函数. 2011/10/22
    public delegate int PF_getPolicyIsClient(CCtxQmc pProcInfo, out POLICY_isClient p);
    public delegate int PF_getPolicyAvLevel(CCtxQyMc pQyMc, uint uiCapType, uint uiSubCapType, int iCapUsage);


    //
    public delegate int PF_initVideoCapDev(object hTalk, int iWndContentType, string displayName_selected, ref BITMAPINFOHEADER pBih_suggested, CAP_procInfo_bmpCommon pCapBmp);
    public delegate int PF_exitVideoCapDev(object hTalk, int iWndContentType);
    public delegate bool PF_bGetCapturePara(object hTalk, int iWndContentType, ref QY_VIDEO_HEADER pVh_org);
    public delegate int PF_startCapture(object hTalk, int iWndContentType);
    public delegate int PF_stopCapture(object hTalk, int iWndContentType);


    //
    public delegate int PF_i420ToRgb(IntPtr yuv, object yri, int rgbBits, bool bBgr, int width, int height, bool bFlipImg);

    //
    public delegate int PF_talk_updatePlayerInfo(object hTalk, IntPtr pMsg, PLAYER_ID playerId);
    public delegate int PF_talk_addNewMsg(object hTalkParam, QY_MESSENGER_ID pIdInfo_from, Int64 tSendTime, uint uiTranNo, ushort usCode, uint uiContentType, string msg, int iTaskId);
    public unsafe delegate int PF_talk_displayTaskStatus(object hDlgTalk, MIS_MSG_taskStatus* pMsgTaskStatus);


    //
    public delegate int PF_loadCusModule_android();

    public delegate int PF_doDecodeVideo_mediaCodec(QY_TRANSFORM pTransform, int index_activeMems_from, IntPtr pMsgBuf);
    public delegate int PF_doEncodeVideo_mediaCodec(CAP_procInfo_bmpCommon pCapProcInfo, MIS_CNT pMisCnt, IntPtr pMsgBuf);
    public delegate int PF_doDecodeAudio_mediaCodec(QY_TRANSFORM pTransform, int index_pMems_from1, int index_activeMems_from, IntPtr pMsgBuf);

    //
    public delegate int PF_stopLocalAudioRecorder(CCtxQmc pProcInfo, int index_sharedObj, int nTries);

    public delegate int PF_startPlayAudio_android(QY_PLAYER pPlayerParam);
    public delegate int PF_stopPlayAudio_android(QY_PLAYER pPlayerParam);

    public delegate int PF_startPlayVideo_android(QY_PLAYER pPlayerParam);
    public delegate int PF_stopPlayVideo_android(QY_PLAYER pPlayerParam);


    public delegate int PF_readDbgCfg(ref string m_statusServer);

    //
    public delegate int PF_newIndex_DLG_TALK_videoConference(int index_taskInfo);
    public delegate void PF_freeIndex_DLG_TALK_videoConference(ref int ind_videoConference);
    public delegate DLG_TALK_videoConference PF_getVideoConference(int ind_videoConference);

    //
    public delegate int PF_new_QMC_taskData(int index_taskInfo, int taskDataType);
    public delegate void PF_free_QMC_taskData(ref int ind_taskData);
    public delegate QMC_taskData_common PF_getTaskData(int ind_taskData);

    //
    public delegate object PF_tfopen(string filename, string mode);
    public delegate int PF_fclose(object stream);
    public delegate int PF_fseeki64(object stream, Int64 offset, int origin);
    public unsafe delegate Int64 PF_fread(void* ptr, int size, int nmemb, object stream);
    public unsafe delegate Int64 PF_fwrite(void* buffer, Int64 size, Int64 count, object stream);
    //
    public delegate bool PF_CopyFile(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);
    public delegate bool PF_DeleteFile(string lpFileName);

    //
    public delegate int PF_newIndex_qisTrans();
    public delegate int PF_freeIndex_qisTrans(int index);
    public delegate QisTrans PF_getQisTransByIndex(int index);

    //
    public delegate int PF_dec_parseEncAPkt_i(uint uiTransformType, object pParent_transform, ref PKT_info_toDec pPktInfo_toDec_param, ref int p_old_lPktId_getNewPkt, ref uint puiSize, ref uint puiSampleTimeInMs, ref uint puiPts,object p0);




    //
    public static class GuiShare
    {
        public static PF_myGetTickCount pf_myGetTickCount;
        public static PF_myGetPid pf_myGetPid;
        public static PF_myGetTid pf_myGetTid;
        public static PF_startThread1 pf_startThread1;
        public static PF_waitForThread1 pf_waitForThread1;

        //
        public static PF_getSystemInfo pf_getSystmInfo;
        public static PF_customSystem pf_customSystem;

        //
        public static PF_bQySockOpen pf_bQySockOpen;
        public static PF_qyConnect pf_qyConnect;
        public static PF_qyDisconnect pf_qyDisconnect;
        public static PF_qySend pf_qySend;
        public static PF_qyRecv pf_qyRecv;
        public static PF_qyWaitForRead pf_qyWaitForRead;
        public static PF_qyWaitForWrite pf_qyWaitForWrite;

        //        
        public static PF_PostMessage pf_PostMessage;
        public static PF_IsWindow pf_IsWindow;
        //            
        public static PF_PostMessage_new pf_PostMessage_new;
        //
        public static PF_mainWnd_getVar pf_mainWnd_getVar;
        //
        public static PF_gui_procMsg pf_gui_procMsg;
        public static PF_gui_dlgTalk_procMsg pf_gui_dlgTalk_procMsg;
        //
        public static PF_gui_notify_cancel pf_gui_notify_cancel;
        public static PF_gui_notify_chk pf_gui_notify_chk;
        //
        public static PF_gui_notify_doCmd pf_gui_notify_doCmd;
        //
        public static PF_mainWnd_setTimer pf_mainWnd_setTimer;
        public static PF_mainWnd_killTimer pf_mainWnd_killTimer;

        //
        public static PF_findTalker pf_findTalker;
        public static PF_dlgTalk_qPostMsg pf_dlgTalk_qPostMsg;

        //
        public static PF_gui_displayNotification pf_gui_displayNotification;

        //
        public static PF_testInMain pf_testInMain;

        //
        public static PF_dumpData pf_dumpData;
        public static PF_dumpData1 pf_dumpData1;
        public static PF_dumpRgb32 pf_dumpRgb32;
        public static PF_dumpRgb32_1 pf_dumpRgb32_1;
        public static PF_dumpBmp pf_dumpBmp;

        //
        public static PF_dumpAudio_init pf_dumpAudio_init;
        public static PF_dumpAudio_exit pf_dumpAudio_exit;
        public static PF_dumpAudio pf_dumpAudio;

        //
        public static PF_logd pf_logd;

        //
        public static PF_initPolicyAvParams pf_initPolicyAvParams;
        public static PF_getPolicyIsClient pf_getPolicyIsClient;
        public static PF_getPolicyAvLevel pf_getPolicyAvLevel;

        //
        public static PF_initVideoCapDev pf_initVideoCapDev;
        public static PF_exitVideoCapDev pf_exitVideoCapDev;
        public static PF_bGetCapturePara pf_bGetCapturePara;
        public static PF_startCapture pf_startCapture;
        public static PF_stopCapture pf_stopCapture;


        //
        public static PF_i420ToRgb pf_i420ToRgb;

        //
        public static PF_talk_updatePlayerInfo pf_talk_updatePlayerInfo;
        public static PF_talk_addNewMsg pf_talk_addNewMsg;
        public static PF_talk_displayTaskStatus pf_talk_displayTaskStatus;


        //
        public static PF_loadCusModule_android pf_loadCusModule_android;
        public static PF_doDecodeVideo_mediaCodec pf_doDecodeVideo_mediaCodec;
        public static PF_doEncodeVideo_mediaCodec pf_doEncodeVideo_mediaCodec;
        public static PF_doDecodeAudio_mediaCodec pf_doDecodeAudio_MediaCodec;

        //
        public static PF_stopLocalAudioRecorder pf_stopLocalAudioRecorder;
        public static PF_startPlayAudio_android pf_startPlayAudio_android;
        public static PF_stopPlayAudio_android pf_stopPlayAudio_android;

        public static PF_startPlayVideo_android pf_startPlayVideo_android;
        public static PF_stopPlayVideo_android pf_stopPlayVideo_android;

        //
        public static PF_readDbgCfg pf_readDbgCfg;

        //
        public static PF_newIndex_DLG_TALK_videoConference pf_newIndex_DLG_TALK_videoConference;
        public static PF_freeIndex_DLG_TALK_videoConference pf_freeIndex_DLG_TALK_videoConference;
        public static PF_getVideoConference pf_getVideoConference;

        //
        public static PF_new_QMC_taskData pf_new_QMC_taskData;
        public static PF_free_QMC_taskData pf_free_QMC_taskData;
        public static PF_getTaskData pf_getTaskData;

        //
        public static PF_tfopen pf_tfopen;
        public static PF_fclose pf_fclose;
        public static PF_fseeki64 pf_fseeki64;
        public static PF_fread pf_fread;
        public static PF_fwrite pf_fwrite;
        //
        public static PF_CopyFile pf_CopyFile;
        public static PF_DeleteFile pf_DeleteFile;

        //
        public static PF_newIndex_qisTrans pf_newIndex_qisTrans;
        public static PF_freeIndex_qisTrans pf_freeIndex_qisTrans;
        public static PF_getQisTransByIndex pf_getQisTransByIndex;


        //
        public static PF_dec_parseEncAPkt_i pf_dec_parseEncAPkt_i;


        //
    };




}
