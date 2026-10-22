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
		public static unsafe int sendConfReq(void* p, object hDlgTalk, QY_MESSENGER_ID idInfo_conf, ushort usOp, ushort usAvLevel, int conf_iFourcc, ushort conf_usMaxSpeakers)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//(  MC_VAR_isCli  *  )m_var.pMisCnt->pProcInfoParam;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            //using (CHelp_getDlgTalkVar help_getDlgTalkVar = new CHelp_getDlgTalkVar())
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hDlgTalk;

            {
				DLG_TALK_var pm_var = dlgTalk.m_var;
				//(DLG_TALK_var)help_getDlgTalkVar.getVar(hDlgTalk);
				if (null==pm_var) return -1;
				DLG_TALK_var  m_var = pm_var;

				int lenInBytes = 0;

				//
				QY_MESSENGER_ID idInfo_to;

				//
				if (0==idInfo_conf.ui64Id) return -1;

				//
				CONF_req req;
				req.uiType = Consts.CONST_imCommType_confReq;
				req.idInfo_imGrp_related = idInfo_conf;
				req.usOp = usOp;
				req.policy.usAvLevel = usAvLevel;
				req.policy.conf_iFourcc = conf_iFourcc;
				req.policy.usMaxSpeakers = conf_usMaxSpeakers;

				//
				lenInBytes = sizeof(CONF_req);
				//					
				Int64 tStartTran;
				uint uiTranNo;
				qyFuncs.MACRO_prepareForTran(out tStartTran,out uiTranNo);


				//	
				MSG_ROUTE msgRoute = new MSG_ROUTE();
				pProcInfo.postMsg2Mgr_mc(mainActivity.var_common.ctxCaller,pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_sendReq, tStartTran, uiTranNo, 0, (byte*)&req, (uint)lenInBytes, 0, 0, 0, null, false);
			}

			iErr = 0;
		errLabel:

			//
			string str;
			str=string.Format("sendReq: conf {0} {1} ", idInfo_conf.ui64Id, usOp);
			qy.TMP_buf5 buf5;
			qyFuncs.iFourcc2Str(conf_iFourcc, buf5.buf, Consts.cntof_buf5);
			str += qyFuncs.mytoString(buf5.buf);
			//_sntprintf(tBuf, mycountof(tBuf), _T("%s conf_iFourcc %S"), tBuf, buf);
			qyFuncs.showInfo_open(0, null, str);

			//
			return iErr;
		}


	}
}