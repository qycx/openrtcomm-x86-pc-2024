using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace qy
{
	partial class qyFuncs
	{

		//
		class PARAM_doMcClientLogon
        {
			public string m_server;

			public int iRet;
        };
	

		public static unsafe int dlgMcClientLogon_OnOK(IntPtr hDlg, string m_server, string m_name, string m_passwd)
		{
			int iErr = -1;

			// TODO: Add your control notification handler code here
			//char tBuf[128];

			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null == pProcInfo) return -1;
			if (pProcInfo.m_iCtxType != Consts.CONST_ctxType_qmc) return -1;

			//
			if (null == m_server) goto errLabel;
			if (null == m_name) goto errLabel;
			if (null == m_passwd) goto errLabel;



			m_server.Trim();
			m_name.Trim();
			m_passwd.Trim();
			//
			if (m_server.Length == 0) goto errLabel;
			if (m_name.Length == 0)
			{
				goto errLabel;
			}
			if (m_passwd.Length == 0)
			{
				goto errLabel;
			}

			//
			fixed (char* pName = pProcInfo.authInfo.messengerName)
			{
				mytcsncpy(pName, m_name, Consts.CONST_maxMessengerNameLen + 1);

				//
			}
			fixed (byte* pBuf = pProcInfo.authInfo.messengerPasswd)
			{
				uint len = Consts.CONST_maxMessengerPasswdLen + 1;
				bQyGetStrMd5_x(m_passwd, pBuf, &len);
			}
			//  CWaitCursor		cur;

			//

			//  2010/01/09
			//
			int i;


			object hThread = null;
			PARAM_doMcClientLogon param = new PARAM_doMcClientLogon();
			param.m_server = m_server;

			//param.pMisCnt = pMisCnt;
			//param.pChannel = pChannel;
			//param.hDlg = hDlg;
			//param.pm_var = pm_var;

			if (0 != GuiShare.pf_startThread1(ref hThread, doMcClientLogon_run, param)) return -1;
			GuiShare.pf_waitForThread1(ref hThread, 10000);
			return param.iRet;

			
			iErr = 0;

			errLabel:

				return iErr;
			}




		//
		public static unsafe void doMcClientLogon_run(object param1)
        {
		int iErr = -1;

			PARAM_doMcClientLogon param = (PARAM_doMcClientLogon)param1;
			string m_server = param.m_server;

			//
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();



		MIS_CNT pMisCnt = pProcInfo.pMisCnt;

			//
			fixed (byte* p = pMisCnt.cfg.dynCfg.cnt.cntAddr1)
			{
				myWChar2Utf8(m_server, p,Consts.CONST_maxCntAddrLen);				
			}
			pMisCnt.cfg.dynCfg.cnt.usCntPort = 8768;



		//
		MIS_CHANNEL pChannel = getChannelByType(pMisCnt, Consts.CONST_channelType_talking);
		if (null == pChannel) goto errLabel;


		if (pChannel.bSessionOpen)
		{
			qnmCloseSession_msg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to);
			pChannel.bSessionOpen = false;
		}

		
			

			//pMisCnt.pAuthInfo = pProcInfo.authInfo;
			clearQySock(ref pChannel.sock);

			//

//
#if true//false
			fixed (byte* pIp = pMisCnt.server.ip)
			{
				getIpFromName(m_server, pIp, Consts.CONST_qyMaxIpLen + 1);

				//
			}
			pMisCnt.server.port = 8768;
#endif

#if true//false
			QY_LOGICAL_SERVER tmpCnt;
			tmpCnt = new QY_LOGICAL_SERVER();
			if (0!=getCorrectCnt(ref pQyMc.env, ref pMisCnt.cfg.dynCfg, bServConnected_mis, pMisCnt, pQyMc.iServiceId, qnmVerStr(pQyMc.iServiceId),pMisCnt.commEncCtx, ref pQyMc.cfg.sockParam, ref tmpCnt))
			{
				//_sntprintf(tBuf, mycountof(tBuf), getResStr(0, &pQyMc->cusRes, CONST_resId_cannotFindServer));
				//m_var.guiData.pf_SetDlgItemText(pDlg->m_hWnd, m_var.guiData.iIDC_STATIC_hint, CQyString(tPrefix) + tBuf);
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (char*)(""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("getCorrectCnt failed"));
				goto errLabel;
			}
			else //if (memcmp(&pMisCnt->server, &tmpCnt, sizeof(tmpCnt)))
			{
				//memcpy(&pMisCnt->server, &tmpCnt, sizeof(pMisCnt->server));
				pMisCnt.server = tmpCnt;

			}


			//  
			if (!pMisCnt.commEncCtx.ucbPublicKeyDownloaded || !pMisCnt.commEncCtx.ucbPublicKeyVerified)
			{
				goto errLabel;
			}

#endif

			if (doMisCntLogon(pMisCnt, pChannel, IntPtr.Zero, IntPtr.Zero) != 0) goto errLabel;

			iErr = 0;

		errLabel:

			param.iRet = iErr;
			//
			return;
        }




		//
		public static unsafe int doMisCntLogon(MIS_CNT pMisCnt, MIS_CHANNEL pChannel, IntPtr hDlg, IntPtr pm_var)
        {
      
			int iErr = -1;

			if (null == pMisCnt) return -1;
			CCtxQmc pProcInfo = pMisCnt.pProcInfoParam;
			if (null == pProcInfo) return -1;
			CCtxQyMc pQyMc = pProcInfo.pQyMc;
			bool bCryptInited = false;
			int i;
			//TCHAR tBuf[256];

			//
			if  (  pMisCnt.commEncCtx.common.type == Consts.CONST_qyEncType_rsa  )
            {
				pProcInfo.authInfo.usSessionEncType = Consts.CONST_qyEncType_aes256;
            }

			//
			pChannel.commEncCtx = new ENC_CTX_session();
			//memcpy(&pChannel->commEncCtx.common, &pMisCnt->commEncCtx.common, sizeof(pChannel->commEncCtx.common)  );
			pChannel.commEncCtx.common = pMisCnt.commEncCtx.common;
			if (initCrypt_qmc(pProcInfo, pMisCnt, Consts.CONST_cspContainer_qmc, Consts.DEFAULT_cspProvider, (pChannel.uiType == Consts.CONST_channelType_talking), ref pChannel.commEncCtx)!=0) goto errLabel;
			bCryptInited = true;

			//
			int tt = 0;
			string str = string.Format(("qmcCrypt{0}"), getuiNextTranNo(null, 0, ref tt));
			fixed (char* pName = pChannel.commEncCtx.m_var.mutexName_syncCrypt) { 
			mytcsncpy(pName, str, Consts.cntof_mutexName);		
			}
			//_sntprintf(pChannel->commEncCtx.mutexName_syncCrypt, mycountof(pChannel->commEncCtx.mutexName_syncCrypt), _T("qmcCrypt%u"), getuiNextTranNo(0, 0, 0));

			//  2011/02/02
			pProcInfo.authInfo.uiLogonId = 0;

            //
            //
            RecyclableMemoryStreamManager recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();



            //  
            for (i = 0; i < Consts.CONST_maxTriesToStartSession_mis; i++)
			{
				//
				//_sntprintf(tBuf, mycountof(tBuf), _T("%s %s, %d..."), getResStr(0, &pQyMc->cusRes, CONST_resId_tryToConnect), CQyString(pMisCnt->server.ip), i);
				//m_var.guiData.pf_SetDlgItemText(hDlg, m_var.guiData.iIDC_STATIC_hint, tBuf);
				//
				RESULT_startChannel result;
				if (0==tryToStartChannel(pMisCnt, pChannel, ref recyclableMemoryStreamManager,  ref result)) break;
			}
			if (i == Consts.CONST_maxTriesToStartSession_mis)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("Logon"), _T(""), _T("failed"));
				goto errLabel;
			}
			pChannel.status.ulbSessionErr = false;
			//

			iErr = 0;

		errLabel:

			pChannel.status.ulbSessionErr = true;  //  此标志位通知recv和snd两线程退出

			if (pChannel.bSessionOpen)
			{
				//traceLogA("to close session");
				qnmCloseSession_msg(ref pChannel.session, ref pChannel.sock, pQyMc.cfg.to);
				pChannel.bSessionOpen = false;
				//traceLogA("session closed");

				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T("Logon"), _T(""), _T("Session is closed"));

			}
			if (bCryptInited) exitCrypt_qmc(pProcInfo, pMisCnt, ref pChannel.commEncCtx);

			pChannel.status.ulbIoQuit = true;

			//
			//param.iRet = iErr;
			return iErr;
		}


	}
}

