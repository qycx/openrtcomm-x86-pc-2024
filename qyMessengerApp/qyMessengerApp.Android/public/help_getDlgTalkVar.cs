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
using qy;


namespace qy.Droid
{
//	public class CHelp_getDlgTalkVar : CQySyncCnt
//	{


//		public DLG_TALK_var getVar(object hDlgTalk, string hint="")
//		{
//			//  2017/09/24
//			if (null == hint) hint = ("");

//			//
//			DLG_TALK_var pDlgTalkVar = (DLG_TALK_var)Activity_dlgTalk.tmp_getDlgTalkVar(hDlgTalk);
//			if (null == pDlgTalkVar) return null;

//			if (0!=sync(pDlgTalkVar.syncFlgs.lCnt_getDlgVar, hint))
//			{
//#if __DEBUG__
//					traceLog(_T("help_getDlgTalkVar::getVar failed, sync failed"));
//#endif
//				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("help_getDlgTalkVar::getVar failed, sync failed, %I64u, tn %d, lCnt_getDlgTalkVar %d"), pDlgTalkVar->addr.idInfo.ui64Id, pDlgTalkVar->addr.uiTranNo_shadow, pDlgTalkVar->syncFlgs.lCnt_getDlgVar);

//				return null;
//			}

//#if __DEBUG__
//				//  traceLog(  _T(  "help_getDlgTalkVar::getVar, lCnt %d"  ),  pDlgTalkVar->syncFlgs.lCnt_getDlgVar  );
//				if (pDlgTalkVar->syncFlgs.lCnt_getDlgVar >= 4)
//				{
//					if (!hint)
//					{
//						int i = 1;
//					}
//				}
//				//
//				//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  (  ""  ),  _T(  "IsClient"  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "help_getDlgTalkVar::getVar failed, %I64u, tn %d, bShouldQuit"  ),  pDlgTalkVar->addr.idInfo.ui64Id,  pDlgTalkVar->addr.uiTranNo_shadow  ); 

//#endif

//			//
//			return pDlgTalkVar;
//		}

//	};

	partial class qyFuncs_Droid
{


}
}