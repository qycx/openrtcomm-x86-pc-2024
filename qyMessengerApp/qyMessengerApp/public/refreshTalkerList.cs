using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe  int resetTalkerList_mgr(object hDlgTalk_mgr)
		{
			int iErr = -1;
			/*
			CHelp_getDlgTalkVar help_getDlgTalkVar_mgr;
			HWND hMgr = hDlgTalk_mgr;
			DLG_TALK_var* pMgrVar = (DLG_TALK_var*)help_getDlgTalkVar_mgr.getVar(hDlgTalk_mgr);
			if (!pMgrVar) return -1;
			if (!isTalkerShadowMgr(pMgrVar->addr)) return -1;
			TALKER_shadow_mgr* pShadowMgr = (TALKER_shadow_mgr*)pMgrVar->pShadowInfo;
			if (!pShadowMgr) return -1;

			//
			int i;
			for (i = 0; i < mycountof(pShadowMgr->shadows); i++)
			{
				TALKER_shadow_mgrMem* pMem = &pShadowMgr->shadows[i];
				if (!IsWindow(pMem->hShadow)) continue;
				//
				{
					CHelp_getDlgTalkVar help;
					DLG_TALK_var* pVar = (DLG_TALK_var*)help.getVar(pMem->hShadow);
					if (!pVar) continue;
					//
					myListCtrl_DeleteAllItems(GetDlgItem(pMem->hShadow, pVar->guiData.iIDC_talkerList));
					//
					memset(&pVar->av.videoConferenceStatus, 0, sizeof(pVar->av.videoConferenceStatus)  );

			//
			if (pVar->guiData.ucbShowImGrpMems)
			{
				SetDlgItemText(pMem->hShadow, pVar->guiData.iIDC_EDIT_filter, _T(""));
				dlgTalk_loadGrpMems(pMem->hShadow, pVar, _T(""));
			}
		}
	}
	myListCtrl_DeleteAllItems(GetDlgItem(hMgr, pMgrVar->guiData.iIDC_talkerList  )  );
	memset(  &pMgrVar->av.videoConferenceStatus,  0,  sizeof(pMgrVar->av.videoConferenceStatus  )  );

	//
	if  (pMgrVar->guiData.ucbShowImGrpMems  )  {
		SetDlgItemText(hMgr, pMgrVar->guiData.iIDC_EDIT_filter, _T(  ""  )  );
		dlgTalk_loadGrpMems(hMgr, pMgrVar, _T(  ""  )  );
	}
			*/

iErr = 0;
errLabel:
return iErr;
}

}
}
