using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;


namespace qy
{
    partial class qyFuncs
{


		//
		public static unsafe int doMcClientLogonOK(IntPtr hDlg, string m_server, string m_name, string m_passwd)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			if (pQyMc.bLogon) return -1;

#if DEBUG
#if false
			//
			m_server="117.24.14.196";
			//
			Console.WriteLine("For test. set m_server to " + m_server);
			//m_name = "user7";
			m_name = "ywm12";
			m_passwd = "123";


#endif
#endif
			//
			Console.WriteLine("doMcClientLogonOk enters");

			//
			if (dlgMcClientLogon_OnOK(hDlg, m_server, m_name, m_passwd) != 0) return -1;

			//

		
			//
			pQyMc.bLogon = true;
			//pQyMc.dwTickCnt_logon = GetTickCount();

			//
			pQyMc.cfg.db.iDbType = Consts.CONST_dbType_myDb;

			//
			//
			if (0!=getProcedObjsCfg(pQyMc)) goto errLabel;

			//
			if (0 != initQyMcDb(null, null, ref pQyMc.procedObjDb))
			{
				goto errLabel;
			}
			pQyMc.gui.pDb = pQyMc.procedObjDb.pDb;
			//  2013/01/30
			pQyMc.setQmDbFuncs(pQyMc.cfg.db.iDbType, pQyMc.p_g_dbFuncs);

			//
			loadCusModules(pQyMc);



			//
			if (pQyMc.iAppType == Consts.CONST_qyAppType_client)
			{
				//  要在登陆后立即运行此函数. 2011/10/22
				if (GuiShare.pf_initPolicyAvParams()!=0) goto errLabel;
			}

				if (pProcInfo.m_iCtxType != Consts.CONST_ctxType_qmc) goto errLabel;

				POLICY_isClient policy;
				if (0==GuiShare.pf_getPolicyIsClient(pProcInfo, out policy))
				{
				pProcInfo.cfg.policy = policy;
				}


			//
			showInfo_open(0, null, "doMcClientLogon: before start threadProcs");

			int i;
	for  (  i  =  0;  i  <  pQyMc.hDaemonThreads.Length;  i  ++  )  {
		  if  (  pQyMc.threadProcs[i]!=null  )  {
					//pQyMc.hDaemonThreads[i] = CreateThread( NULL, 0, pQyMc->threadProcs[i], pQyMc, 0, &dwThreadDaemonId );
					//if ( pQyMc->hDaemonThreads[i] == NULL ) goto errLabel;
#if false
					pQyMc.hDaemonThreads[i] = new Thread(new ParameterizedThreadStart(pQyMc.threadProcs[i]));
					pQyMc.hDaemonThreads[i].Start(pQyMc);
#endif
					GuiShare.pf_startThread1(ref pQyMc.hDaemonThreads[i], pQyMc.threadProcs[i], pQyMc);
		  }
	 }

			//
			showInfo_open(0, null, "doMcClientLogon: after start threadProcs");




			iErr = 0;

		errLabel:

			if (  0!=iErr )
            {
				qyMcLogoff();
            }

			Console.WriteLine("doMcClientLogonOk leaves");

			return iErr;
		}


		public static unsafe void qyMcLogoff()
		{
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			//
			Console.WriteLine("qyMcLogoff enters");
			showInfo_open(0, null, "qyMcLogoff enters");

			//
			pQyMc.bLogon = false;

			//
			exitAllDaemonThreads(pQyMc);

			//
			pProcInfo.processQ_media.emptyQ();

			//
			//
			chkPlayers(pProcInfo);


			//
			exitQyMcDb(null, null, ref pQyMc.procedObjDb);

			//
			unloadCusModules(pQyMc);

			//
			Console.WriteLine("qyMcLogoff leaves");
			showInfo_open(0, null, "qyMcLogoff leaves");


			return;
		}

	}
}
