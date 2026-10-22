using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
	partial class qyFuncs_Droid
{

        public static int newIndex_DLG_TALK_videoConference(int index_taskInfo)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            //
            int index = -1;

            if (index_taskInfo < 0 || index_taskInfo >= m_tmp_videoConferences.Length) goto errLabel;

            //
            if (m_tmp_videoConferences[index_taskInfo] == null) goto errLabel;

            //              
            //m_tmp_videoConferences[index_taskInfo] = new DLG_TALK_videoConference();

            //DLG_TALK_videoConference videoConference = m_tmp_videoConferences[index_taskInfo];// new DLG_TALK_videoConference();


            //
            index = index_taskInfo;

        errLabel:

            //
            return index;
        }

        public static DLG_TALK_videoConference getVideoConference(int ind_videoConference)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            //
            DLG_TALK_videoConference videoConference = null;
            int index = (int)ind_videoConference;

            if (index < 0 || index >= m_tmp_videoConferences.Length) return null;

            videoConference = m_tmp_videoConferences[index];

            return videoConference;

        }

        public static void freeIndex_DLG_TALK_videoConference(ref int ind_videoConference)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            int index = (int)ind_videoConference;

            /*
            if (m_tmp_videoConferences[index] != null)
            {
                //
                m_tmp_videoConferences[index] = null;
            }
            */
            //
            ind_videoConference = -1;

            return;
        }


        /// <summary>
        /// ////////////////
        /// </summary>
        /// <param name="pMsgParam"></param>
        /// <returns></returns>

        public static int new_QMC_taskData(int index_taskInfo, int taskDataType)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;
                 
            //
            int index = -1;

            //
#if DEBUG
            qyFuncs.traceLog("new_QMC_taskData enters");
#endif


            //
            if (index_taskInfo < 0 || index_taskInfo >= mainWnd.m_tmp_taskDatas.Length) goto errLabel;

            //
            if (mainWnd.m_tmp_taskDatas[index_taskInfo] != null) goto errLabel;

            //              
            switch (taskDataType)
            {
                case Consts.CONST_taskDataType_conf:
                    //
#if DEBUG
                    qyFuncs.traceLog("new_QMC_taskData.113, befoe new QMC_taskData_conf");
#endif
                    //
                    mainWnd.m_tmp_taskDatas[index_taskInfo] = new QMC_taskData_conf(index_taskInfo);
                     break;
                case Consts.CONST_taskDataType_transferFile:
                     mainWnd.m_tmp_taskDatas[index_taskInfo] = new QMC_taskData_transferFile();
                     break;
                default:
                    qyFuncs.traceLog("new_QMC_taskData failed: unprocessed taskDataType");
                    goto errLabel;
            }
            //
#if DEBUG
            qyFuncs.traceLog("new_QMC_taskData.127");
#endif

            //
            QMC_taskData_common taskData = mainWnd.m_tmp_taskDatas[index_taskInfo];// new DLG_TALK_videoConference();
            //
            if ( taskData.uiType != (uint)taskDataType )
            {
                int ii = 0;
            }
            //
            taskData.tmpStatus = new QmcTaskDataCommon_tmpStatus();  //  clear 
            //
            index = index_taskInfo;

        errLabel:

#if DEBUG
            qyFuncs.traceLog("new_QMC_taskData leaves.");
#endif


            //
            return index;
        }

        public static QMC_taskData_common getTaskData(int ind_taskData)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            //
            QMC_taskData_common taskData = null;
            int index = (int)ind_taskData;

            if (index < 0 || index >= mainWnd.m_tmp_taskDatas.Length) return null;

            taskData = mainWnd.m_tmp_taskDatas[index];

            return taskData;

        }

        public static void free_QMC_taskData(ref int ind_taskData)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            int index = (int)ind_taskData;
            if (index >= 0 && index < mainWnd.m_tmp_taskDatas.Length)
            {

                if (mainWnd.m_tmp_taskDatas[index] != null)
                {
                    //
                    mainWnd.m_tmp_taskDatas[index].free();

                    //
                    mainWnd.m_tmp_taskDatas[index] = null;
                }
            }
            //
            ind_taskData = -1;

            return;
        }

        ////////////////////////////////
        ///
        public static bool bIndexValid_myThread( int index)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            if (index < 1 || index >= mainWnd.m_tmp_threads.Length) return false;
            return true;

        }

        public static int newMyThread(object p0, PF_threadProc pf, object param, ref int index_o)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            using (CQySyncObj syncObj = new CQySyncObj())
            {

                //
                if (syncObj.sync("", mainWnd.hMutex_syncThreads) != 0)
                {
                    goto errLabel;
                }

                //
                int index = -1;

                for (index = 1; index < mainWnd.m_tmp_threads.Length; index++)
                {
                    if (mainWnd.m_tmp_threads[index] == null) break;
                }
                if (index == mainWnd.m_tmp_threads.Length)
                {
                    qyFuncs.showNotification_open(0, 0, 0, "new_androidThread failed: too many android threads");
                    goto errLabel;
                }
                //              
                mainWnd.m_tmp_threads[index] = new MyThread(p0, pf, param);


                index_o = index;
            }


            //
            string str;
            str = string.Format("newMyThread ok, {0}", index_o);
            qyFuncs.showInfo_open(0, null, str);


            //
            iErr = 0;

        errLabel:

            //
            return iErr;
        }

        public static MyThread getMyThread(int obj_myThread)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            //
            MyThread mt = null;
            int index = (int)obj_myThread;

            if (index < 1 || index >= mainWnd.m_tmp_threads.Length) return null;

            mt = mainWnd.m_tmp_threads[index];

            return mt;

        }

        public static void freeMyThread(ref int obj_myThread)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            int index = (int)obj_myThread;

            using (CQySyncObj syncObj = new CQySyncObj())
            {

                //
                if (syncObj.sync("", mainWnd.hMutex_syncThreads) != 0)
                {
                    goto errLabel;
                }

                if (mainWnd.m_tmp_threads[index] != null)
                {
                    //
                    mainWnd.m_tmp_threads[index] = null;
                }
            }

            //
            string str;
            str = string.Format("freeMyThread {0}", obj_myThread);
            qyFuncs.showInfo_open(0, null, str);
            //
            obj_myThread = -1;
            errLabel:

            return;
        }


    }


}