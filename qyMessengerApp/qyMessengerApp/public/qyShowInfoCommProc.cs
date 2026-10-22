using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
	public const int DEFAULT_qyPort_showInfo = 8765;
}


namespace qy
{
	public delegate  int   PF_qyShowInfo0(  IntPtr pShowStructParam,  IntPtr  pReserved  );

    partial class qyFuncs
{


		public class ShowInfo_struct
		{
				public bool bInited;
			//
			public string errHintWndCls;
			public PF_qyShowInfo0 errHintPf;
			//
			public uint dwThreadId;
			//public Thread hThread;
			public object hThread;
			public bool bQuit;
			//
			public bool bShowMore;
			//
			public string mutexName_syncQ;
			//
			public bool bUsePipe;          //  2012/02/03
			public bool bUseSock;
			public string servIp;
			public ushort usPort;
			//
			public bool bCli;
			//
			public RW_lock_param rwLockParam;      //  2009/05/14

			//  
			//public QY_Q2 q2;
			public QY_qBuf2 q2;
			public bool bQInited;
			//
			//public QY_Q2 state_q2;         //  2015/08/23
			public QY_qBuf2 state_q2;
			public bool bQInited_state;

			//
			public string cfg_serverIp;

			//
			public bool status_bSessionOpen;
	
	}
	;



		public static ShowInfo_struct sShowInfoStruct  = null;

		//
		public static ShowInfo_struct g_pShowInfoStruct = null;//&sShowInfoStruct;//NULL;


		//


		public static int setErrHintPf(PF_qyShowInfo0 pf)
		{
			sShowInfoStruct.errHintPf = pf;

			return 0;
		}


		public static unsafe void exitShowInfo(ref object  ppCtx_showInfo)
		{
			CQySyncObj syncObj;

			//
			traceLog(_T("exitShowInfo enters"));

			
			//sShowInfoStruct.

			//
			int i;

			//
			if (sShowInfoStruct.status_bSessionOpen)
			{
				for (i = 0; i < 10; i++)
				{
					if (sShowInfoStruct.q2.isQ2Empty()) break;
					mySleep(10);
					continue;
				}
			}
			//
#if DEBUG
			int nQNodes = sShowInfoStruct.q2.getQ2Nodes();
			string str;
			str=string.Format("before waitFor showInfoThread, qNodes of showInfo.q2 "+nQNodes);
			qyFuncs.traceLog(str);
#endif
			//
			sShowInfoStruct.bQuit = true;
			sShowInfoStruct.q2.q2PostMsgAndTrigger(IntPtr.Zero,IntPtr.Zero, 0);
			//MACRO_qyNullThread(ref sShowInfoStruct.hThread);
			GuiShare.pf_waitForThread1(ref sShowInfoStruct.hThread, 10000);

			if (sShowInfoStruct.bQInited)
			{
				sShowInfoStruct.q2.exitQyQ2(); sShowInfoStruct.bQInited = false;
			}
			//  2015/08/23
			if (sShowInfoStruct.bQInited_state)
			{
				sShowInfoStruct.state_q2.exitQyQ2(); sShowInfoStruct.bQInited_state = false;
			}

			//
			sShowInfoStruct.bInited = false;
			//
			ppCtx_showInfo = null;

			//
			g_pShowInfoStruct = null;

			//
			traceLog(_T("exitShowInfo leaves"));


		}

		





	

		//
		public static unsafe int initShowInfo_cli(IntPtr p0, string mutexName_syncQ, string servIp, ref object ppCtx_showInfo)
		{
			return initShowInfo_func(p0, mutexName_syncQ, ("unused"), false, true, servIp, true, ref ppCtx_showInfo);
		}


		//
		public static unsafe int initShowInfo_func(IntPtr p0, string mutexName_syncQ, string errHintWndCls, bool bUsePipe, bool bUseSock, string servIp, bool bCli, ref object ppCtx_showInfo)
		{
			int iErr = -1;
			uint dwThreadDaemonId;

			if (null!=g_pShowInfoStruct) return -1;
			//
												//
			if (null==mutexName_syncQ || 0==mutexName_syncQ.Length) return -1;
			if (null==errHintWndCls || 0==errHintWndCls.Length) return -1;

			//
			//memset(&sShowInfoStruct, 0, sizeof(sShowInfoStruct));
			sShowInfoStruct = new ShowInfo_struct();
			g_pShowInfoStruct = sShowInfoStruct;

			//
			//lstrcpyn(sShowInfoStruct.mutexName_syncQ, mutexName_syncQ, mycountof(sShowInfoStruct.mutexName_syncQ));
			sShowInfoStruct.mutexName_syncQ = mutexName_syncQ;
			//if (0!=setErrHintWndCls(errHintWndCls)) goto errLabel;
			sShowInfoStruct.bUsePipe = bUsePipe;
			//
			if (!sShowInfoStruct.bUsePipe)
			{
				sShowInfoStruct.bUseSock = bUseSock;
			}
			if (sShowInfoStruct.bUseSock)
			{
				//
				sShowInfoStruct.cfg_serverIp = servIp;

				//safeStrnCpy(servIp, sShowInfoStruct.servIp, mycountof(sShowInfoStruct.servIp));
				sShowInfoStruct.servIp = (servIp);
				sShowInfoStruct.usPort = Consts.DEFAULT_qyPort_showInfo;
			}

			//
			sShowInfoStruct.bCli = bCli;

			//  memset(  &rwLockParam,  0,  sizeof(  rwLockParam  )  );
			sShowInfoStruct.rwLockParam.uiMaxCnt_sema = Consts.CONST_uiInitCnt_sema_q2SyncFlg;
			sShowInfoStruct.rwLockParam.uiInitCnt_sema = sShowInfoStruct.rwLockParam.uiMaxCnt_sema - 1;
			sShowInfoStruct.rwLockParam.uiMilliSeconds_mutex_r = 10000;
			sShowInfoStruct.rwLockParam.uiMilliSeconds_sema_r = 10000;
			sShowInfoStruct.rwLockParam.uiMilliSeconds_mutex_w = 10000;
			sShowInfoStruct.rwLockParam.uiMilliSeconds_sema_w = 10000;


			//
			GENERIC_Q_CFG qCfg;
			mymemset((IntPtr)(byte*)&qCfg, 0, sizeof(GENERIC_Q_CFG));
			//_sntprintf(qCfg.name, mycountof(qCfg.name), _T("%s"), _T("showInfoQ"));
			mytcsncpy(qCfg.name, "showInfoQ", Consts.cntof_q2Cfg_name);
			//_sntprintf(qCfg.mutexName_prefix, mycountof(qCfg.mutexName_prefix), _T("%s"), sShowInfoStruct.mutexName_syncQ);
			//  qCfg.uiMaxCnt_semaTrigger  =  CONST_maxCnt_sema_showInfoQ_is;
			qCfg.uiMaxQNodes = Consts.CONST_uiMaxQNodes_showInfoQ_is;
			//
			qCfg.b_showInfoQ = true;

			//  
			sShowInfoStruct.q2 = new QY_qBuf2();// QY_Q2();
												 //if (0!=initQyQ2(qCfg, sShowInfoStruct.rwLockParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, sShowInfoStruct.q2)) goto errLabel;
			int maxDataBufLen = (int)(qCfg.uiMaxQNodes * sizeof(QY_SHOW_SMALL));
			if (0 != sShowInfoStruct.q2.initQyQ2(Consts.CONST_qType_showInfo, (ushort)qCfg.uiMaxQNodes, maxDataBufLen, sShowInfoStruct.rwLockParam,true,"showInfo.q2")) goto errLabel;
			sShowInfoStruct.bQInited = true;

			//  2015/08/23
			mymemset((IntPtr)(byte*)&qCfg, 0, sizeof(GENERIC_Q_CFG));
			//_sntprintf(qCfg.name, mycountof(qCfg.name), _T("%s"), _T("showInfo_state_Q"));
			mytcsncpy(qCfg.name, ("showInfo_state_Q"), Consts.cntof_qCfg_name);
			mytcsncpy(qCfg.mutexName_prefix, sShowInfoStruct.mutexName_syncQ, Consts.cntof_mutexName_prefix);
			//  qCfg.uiMaxCnt_semaTrigger  =  CONST_maxCnt_sema_showInfoQ_is;
			qCfg.uiMaxQNodes = Consts.CONST_uiMaxQNodes_showInfoQ_state_is;

			//  
			sShowInfoStruct.state_q2 = new QY_qBuf2();// QY_Q2();
													   //if (0!=initQyQ2(qCfg, sShowInfoStruct.rwLo0ckParam, 1, null, mallocMemory, mymalloc, null, null, freeMemory, myfree, null, sShowInfoStruct.state_q2)) goto errLabel;
			maxDataBufLen = (int)(qCfg.uiMaxQNodes * sizeof(QY_SHOW_SMALL));
			if (0 != sShowInfoStruct.state_q2.initQyQ2(Consts.CONST_qType_showInfo, (ushort)qCfg.uiMaxQNodes, maxDataBufLen, sShowInfoStruct.rwLockParam,true,"showInfo.stateQ2")) goto errLabel;
			sShowInfoStruct.bQInited_state = true;



			//  2014/03/23

			//
#if false
			sShowInfoStruct.hThread = new Thread(new System.Threading.ParameterizedThreadStart(showInfoThreadProc));
			if (null==sShowInfoStruct.hThread) goto errLabel;
			sShowInfoStruct.dwThreadId = 0;// dwThreadDaemonId;  //  2007/03/29, 为了引入这个标记，以便mtCliSockThreadProc能够在pMtSock里找到存放的物理位置，故要先挂起一下，然后再恢复
										   //if (ResumeThread(sShowInfoStruct.hThread) == -1) goto errLabel;
			sShowInfoStruct.hThread.Start();
#endif
			GuiShare.pf_startThread1(ref sShowInfoStruct.hThread, showInfoThreadProc, 0);
			//
			sShowInfoStruct.bInited = true;

			//  2013/07/31
			//set_g_pShowInfoStruct(  &sShowInfoStruct  );
			//
			ppCtx_showInfo = sShowInfoStruct;

			//
			iErr = 0;

		errLabel:

			if (0!=iErr)
			{

				exitShowInfo(ref ppCtx_showInfo);

			}

			return iErr;
		}


		public static unsafe void showInfoThreadProc(object lpParameter)
		{
			uint dwRet;
			QY_SHOW_SMALL showStruct;
			uint len;
			//HWND firstWnd = NULL;
			bool bUsePipe = sShowInfoStruct.bUsePipe;
			//HANDLE hPipe = NULL;
			//
			//LPTSTR				lpszPipename		=		sShowInfoStruct.bCli  ?  CONST_statusPipeName_cli  :  CONST_statusPipeName_serv;
			string lpszPipename = "";// sShowInfoStruct.bCli ? CONST_statusPipeName_cli_pipeCli : CONST_statusPipeName_serv_pipeCli;
			//
			bool fSuccess = false;
			uint cbRead = 0, cbWritten = 0, dwMode = 0;
			//
			uint dwTickCnt_lastData = myGetTickCount(null);

			//
			QY_SOCK sock = new QY_SOCK();
			QY_COMM_SESSION session=new QY_COMM_SESSION();
			bool bSessionOpen = false;
			SOCK_PARAM sockParam;

			//
			//int setSockTimeout(SOCK_TIMEOUT* p);

			sockParam = new SOCK_PARAM();
			sockParam.capacity_buf_r = 1024;
			sockParam.capacity_buf_w = 1024;
			setSockTimeout(&sockParam.to);


			//
			traceLogA("showInfoThreadProc starts");

			//
			bool bLastGotState = false;

			//
			while (!sShowInfoStruct.bQuit)
			{

				//  2006/06/03
				//dwRet = WaitForSingleObject(sShowInfoStruct.q2.hEvents[0], 5000);
				//if (dwRet == WAIT_FAILED) goto errLabel;
				myWaitOne(sShowInfoStruct.q2.hEvent0, 1000);
				
				//  超时也要接着处理，以getMsg为准
				for (; !sShowInfoStruct.bQuit;)
				{

					//q2AvoidTriggerTooHigh(null, sShowInfoStruct.q2);

					//
					bool bMsgGot = false;

					if (!bLastGotState)
					{
						if (!sShowInfoStruct.state_q2.isQ2Empty())
						{
							QY_qBuf2.HeadData_showInfo headData;
							len = (uint)sizeof(QY_SHOW_SMALL);
							if (0 != sShowInfoStruct.state_q2.q2GetMsg((IntPtr)(byte*)&headData,(IntPtr)(byte*)&showStruct, &len)) break;
							bMsgGot = true;

							//
							bLastGotState = true;

							//
							//
							byte ucPercent = 0;
							int nQNodes = (sShowInfoStruct.state_q2.getQ2Nodes());
							ucPercent = (byte)(nQNodes * 100.0f / (sShowInfoStruct.state_q2.cfg.uiMaxQNodes * 2));
							//
							showStruct.ucPercent_showInfoQ = ucPercent;

						}
					}

					//
					if (!bMsgGot)
					{
						//
						bLastGotState = false;

						//
						QY_qBuf2.HeadData_showInfo headData;
						len = (uint)sizeof(QY_SHOW_SMALL);
						if (0!= sShowInfoStruct.q2.q2GetMsg((IntPtr)(byte*)&headData,(IntPtr)(byte*)&showStruct, &len)) break;

						//
						//
						byte ucPercent = 0;
						int nQNodes = (sShowInfoStruct.q2.getQ2Nodes());
						ucPercent = (byte)(nQNodes * 100.0f / (sShowInfoStruct.q2.cfg.uiMaxQNodes * 2));
						//
						showStruct.ucPercent_showInfoQ = ucPercent;


						//
						int ii = 0;
					}

					//
					if (bUsePipe)
					{

						traceLogA("pipe is not supported in ce");
						continue;
					}
					if (sShowInfoStruct.bUseSock)
					{
						//
						if (bSessionOpen != sShowInfoStruct.status_bSessionOpen)
                        {
							sShowInfoStruct.status_bSessionOpen = bSessionOpen;
                        }

						//
						uint dwTickCnt = myGetTickCount(null);
						if (dwTickCnt - dwTickCnt_lastData > Consts.MAX_pipeCli_idleInMs)
						{
							if (bSessionOpen)
							{
								qnmCloseSession(ref session, ref sock, ref sockParam.to);
								bSessionOpen = false;
							}
						}
						dwTickCnt_lastData = dwTickCnt;

						//
						if (0 != mytcsicmp(sShowInfoStruct.cfg_serverIp, sShowInfoStruct.servIp))
                        {
							if (bSessionOpen)
							{
								qnmCloseSession(ref session, ref sock, ref sockParam.to);
								bSessionOpen = false;
							}
							sShowInfoStruct.servIp = sShowInfoStruct.cfg_serverIp;
						}


						//
						if (!bSessionOpen)
						{
							QWM_MACS_INFO macsInfo;
							mymemset((IntPtr)(byte*)&macsInfo, 0, sizeof(QWM_MACS_INFO));
							macsInfo.nMacs = 1;
							TMP_buf2 ver = new TMP_buf2();
							if (0!=qnmOpenSession(sShowInfoStruct.servIp, sShowInfoStruct.usPort, &macsInfo, 0, Consts.CONST_qyServiceId_showInfo, ver.buf, null, ref session, ref sock, ref sockParam, null, null))
							{
								continue;
							}
							bSessionOpen = true;
						}
						//
						//
						if (0!=qySendReq(ref session, ref sock, ref sockParam.to, Consts.CONST_qyCmd_showInfo_small, (byte*)&showStruct, (uint)sizeof(QY_SHOW_SMALL)))
						{
							qnmCloseSession(ref session, ref sock, ref sockParam.to);
							bSessionOpen = false;
							continue;
						}
						continue;
					}



					//
#if false
					if (sShowInfoStruct.errHintWndCls[0])
					{

						if (!IsWindow(firstWnd))
						{
							firstWnd = FindWindow(sShowInfoStruct.errHintWndCls, NULL);
						}
						if (firstWnd)
						{
							COPYDATASTRUCT tmpCopyData;
							LRESULT lRet = 0;

							tmpCopyData.lpData = &showStruct;
							tmpCopyData.cbData = sizeof(showStruct);
							lRet = SendMessage(firstWnd, WM_COPYDATA, NULL, (LPARAM) & tmpCopyData);

							//  traceLogA(  "lRet is %d",  lRet  );
							//  OutputDebugString(  _T(  "kk\n"   )  );
						}

						//  traceLogA(  "%s %s %s %s %s",  showStruct.when,  showStruct.who,  showStruct.where,  showStruct.doStr,  showStruct.what  );

					}
#endif


				}


			}

		errLabel:

#if false
			if (isHandleValid(hPipe))
			{
				CloseHandle(hPipe); hPipe = NULL;
			}
#endif
			if (bSessionOpen)
			{
				qnmCloseSession(ref session, ref sock, ref sockParam.to);
			}

			//
			sShowInfoStruct.status_bSessionOpen = false;

			//
			traceLogA("showInfoThreadProc ends");

			return;
		}

		/*

	   */


		public static int showInfo_getQ2Nodes()
        {
			int nQ2Nodes = sShowInfoStruct.q2.getQ2Nodes();
			return nQ2Nodes;
        }

		//
		 public static int showInfo_getStatus(ref string status )
        {
			status = string.Format("{0} {1} {2}", sShowInfoStruct.servIp, sShowInfoStruct.status_bSessionOpen ? "connected" : "not connected", showInfo_getQ2Nodes());
			return 0;

        }

		public static int showInfo_cfgServer(string serv)
        {
			sShowInfoStruct.cfg_serverIp = serv;
			return 0;
        }

		public static int showInfo_get_cfgServer(ref string serv)
        {
			if (sShowInfoStruct == null) return -1;
			serv = sShowInfoStruct.cfg_serverIp;
			return 0;			
        }

		//
		public static unsafe int errLog(string fmt  )
		{

			return 0;

		}





	}


}
