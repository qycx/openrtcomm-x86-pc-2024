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

namespace qy.Droid
{
	partial class qyFuncs_Droid
{
		public static unsafe int refreshTalkerList(object hDlgTalk)
		{
			/*

			CHelp_getDlgTalkVar help_getDlgTalkVar;
			DLG_TALK_var* pVar = (DLG_TALK_var*)help_getDlgTalkVar.getVar(hDlgTalk);
			if (!pVar) return -1;

			DLG_TALK_var & m_var = *(DLG_TALK_var*)pVar;

			//  trigger the instantAssistant to refresh

			SendMessage(m_var.hWnd_instantAssistant, CONST_qyWm_comm, 0, 0);
			//  2017/06/25												
			//SendMessage(  m_var.confMgr.hWnd_confMgr,  CONST_qyWm_comm,  0,  0  );													
			fillTalkerList_func_mgr(hDlgTalk, hDlgTalk, &m_var.av.videoConferenceStatus, m_var.guiData.iIDC_talkerList);
			*/

			return 0;
		}

	}
}