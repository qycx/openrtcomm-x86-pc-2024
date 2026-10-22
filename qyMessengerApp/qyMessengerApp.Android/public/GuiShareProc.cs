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
using qyMessengerApp.Droid;


//
namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static unsafe int GuiShare_init(object hMainWnd)
        {

            //
            MainActivity mainWnd = (MainActivity)hMainWnd;

            //
            //
            GuiShare.pf_myGetTickCount = qyFuncs_Droid.myGetTickCount_android;
            GuiShare.pf_myGetPid = qyFuncs_Droid.myGetPid;
            GuiShare.pf_myGetTid = qyFuncs_Droid.myGetTid;
            //
            GuiShare.pf_startThread1 = qyFuncs_Droid.startThread1;
            GuiShare.pf_waitForThread1 = qyFuncs_Droid.waitForThread1;

            //
            GuiShare.pf_getSystmInfo = qyFuncs_Droid.getSystemInfo;
            GuiShare.pf_customSystem = qyFuncs_Droid.customSystem;

            //
#if false
            GuiShare.pf_bQySockOpen = qyFuncs.bQySockOpen;
            GuiShare.pf_qyConnect = qyFuncs.qyConnect;
            GuiShare.pf_qyDisconnect = qyFuncs.qyDisconnect;
            GuiShare.pf_qySend = qyFuncs.qySend;
            GuiShare.pf_qyRecv = qyFuncs.qyRecv;
            GuiShare.pf_qyWaitForRead=qyFuncs.qyWaitForRead;
            GuiShare.pf_qyWaitForWrite=qyFuncs.qyWaitForWrite;
#endif
            //
#if true
            GuiShare.pf_bQySockOpen = qyFuncs_Droid.bQySockOpen_android;
            GuiShare.pf_qyConnect = qyFuncs_Droid.qyConnect_android;
            GuiShare.pf_qyDisconnect = qyFuncs_Droid.qyDisconnect_android;
            GuiShare.pf_qySend = qyFuncs_Droid.qySend_android;
            GuiShare.pf_qyRecv = qyFuncs_Droid.qyRecv_android;
            GuiShare.pf_qyWaitForRead = qyFuncs_Droid.qyWaitForRead_android;
            GuiShare.pf_qyWaitForWrite = qyFuncs_Droid.qyWaitForWrite_android;

#endif


            //
            GuiShare.pf_PostMessage = mainWnd.gui_PostMessage;
            GuiShare.pf_IsWindow = mainWnd.gui_IsWindow;
            //
            GuiShare.pf_PostMessage_new = mainWnd.gui_PostMessage_new;
            //
            GuiShare.pf_mainWnd_getVar = mainWnd.mainWnd_getVar;
            //
            GuiShare.pf_gui_procMsg = MainActivity.gui_procMsg;
            //
            GuiShare.pf_gui_dlgTalk_procMsg = MainActivity.gui_dlgTalk_procMsg;
            //
            GuiShare.pf_gui_notify_cancel = MainActivity.gui_notify_cancel;
            //
            GuiShare.pf_gui_notify_chk = MainActivity.gui_notify_chk;
            //
            GuiShare.pf_gui_notify_doCmd = MainActivity.gui_notify_doCmd;

            GuiShare.pf_mainWnd_setTimer = MainActivity.mainWnd_setTimer;
            GuiShare.pf_mainWnd_killTimer = MainActivity.mainWnd_killTimer;

            GuiShare.pf_findTalker = qyFuncs_Droid.findTalker;
            GuiShare.pf_dlgTalk_qPostMsg = qyFuncs_Droid.dlgTalk_qPostMsg;

            GuiShare.pf_gui_displayNotification = qyFuncs_Droid.gui_displayNotification;

            //
            GuiShare.pf_testInMain = testCls_Droid.testInMain;
            //
            GuiShare.pf_dumpData = qyFuncs_Droid.dumpData;
            GuiShare.pf_dumpData1 = qyFuncs_Droid.dumpData1;
            GuiShare.pf_dumpRgb32 = qyFuncs_Droid.dumpRgb32;
            GuiShare.pf_dumpRgb32_1 = qyFuncs_Droid.dumpRgb32_1;
            GuiShare.pf_dumpBmp = qyFuncs_Droid.dumpBmp;
            //
            GuiShare.pf_dumpAudio_init = qyFuncs_Droid.dumpAudio_init;
            GuiShare.pf_dumpAudio_exit = qyFuncs_Droid.dumpAudio_exit;
            GuiShare.pf_dumpAudio = qyFuncs_Droid.dumpAudio;

            //
            GuiShare.pf_logd = qyFuncs_Droid.logd;

            //
            GuiShare.pf_initPolicyAvParams = qyFuncs_Droid.initPolicyAvParams;
            GuiShare.pf_getPolicyIsClient = qyFuncs_Droid.getPolicyIsClient;
            GuiShare.pf_getPolicyAvLevel = qyFuncs_Droid.getPolicyAvLevel;

            //
            GuiShare.pf_initVideoCapDev = Activity_dlgTalk_av.initVideoCapDev;
            GuiShare.pf_exitVideoCapDev = Activity_dlgTalk_av.exitVideoCapDev;
            GuiShare.pf_bGetCapturePara = Activity_dlgTalk_av.bGetCapturePara;
            GuiShare.pf_startCapture = Activity_dlgTalk_av.startCapture;
            GuiShare.pf_stopCapture = Activity_dlgTalk_av.stopCapture;

            //
            GuiShare.pf_i420ToRgb = qyFuncs_Droid.i420ToRgb;

            //
            GuiShare.pf_talk_updatePlayerInfo = Activity_dlgTalk.talk_updatePlayerInfo;
            GuiShare.pf_talk_addNewMsg = Fragment_dlgTalk_msgList.talk_addNewMsg;
            GuiShare.pf_talk_displayTaskStatus = Fragment_dlgTalk_msgList.talk_displayTaskStatus;

            //
            GuiShare.pf_loadCusModule_android = qyFuncs_Droid.loadCusModule_android;

            //
            GuiShare.pf_doDecodeVideo_mediaCodec = qyFuncs_Droid.doDecodeVideo_mediaCodec;
            GuiShare.pf_doEncodeVideo_mediaCodec = qyFuncs_Droid.doEncodeVideo_mediaCodec;
            GuiShare.pf_doDecodeAudio_MediaCodec = qyFuncs_Droid.doDecodeAudio_mediaCodec;

            //
            GuiShare.pf_stopLocalAudioRecorder = qyFuncs_Droid.stopLocalAudioRecorder;
            //
            GuiShare.pf_startPlayAudio_android = qyFuncs_Droid.startPlayAudio_android;
            GuiShare.pf_stopPlayAudio_android = qyFuncs_Droid.stopPlayAudio_android;
            //
            GuiShare.pf_startPlayVideo_android = qyFuncs_Droid.startPlayVideo_android;
            GuiShare.pf_stopPlayVideo_android = qyFuncs_Droid.stopPlayVideo_android;

            //

            //
            GuiShare.pf_readDbgCfg = qyFuncs_Droid.readDbgCfg;

            //
            GuiShare.pf_newIndex_DLG_TALK_videoConference = qyFuncs_Droid.newIndex_DLG_TALK_videoConference;
            GuiShare.pf_freeIndex_DLG_TALK_videoConference = qyFuncs_Droid.freeIndex_DLG_TALK_videoConference;
            GuiShare.pf_getVideoConference = qyFuncs_Droid.getVideoConference;

            //
            GuiShare.pf_new_QMC_taskData = qyFuncs_Droid.new_QMC_taskData;
            GuiShare.pf_free_QMC_taskData = qyFuncs_Droid.free_QMC_taskData;
            GuiShare.pf_getTaskData = qyFuncs_Droid.getTaskData;

            //
            GuiShare.pf_tfopen = _tfopen_android;
            GuiShare.pf_fclose = fclose_android;
            GuiShare.pf_fseeki64 = fseeki64_android;
            GuiShare.pf_fread = fread_android;
            GuiShare.pf_fwrite = fwrite_android;
            //
            GuiShare.pf_CopyFile = CopyFile_android;
            GuiShare.pf_DeleteFile = DeleteFile_android;
            //
            GuiShare.pf_newIndex_qisTrans=newIndex_qisTrans;
            GuiShare.pf_freeIndex_qisTrans=freeIndex_qisTrans;
            GuiShare.pf_getQisTransByIndex = getQisTransByIndex; ;
            //
            GuiShare.pf_dec_parseEncAPkt_i = dec_parseEncAPkt_i;


            //
            return 0;
        }

    }
}