using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe int regInfo2Desc(void* p0, QY_MESSENGER_REGINFO* pRegInfo, out MY_REG_DESC pDesc, char* talkerDesc, uint uiCnt_talkerDesc, char * displayName, uint uiCnt_displayName)
		{
			int iErr = -1;
			int index;
			//  unsigned  int			uiFieldId;

			/*
			if ((index = getRegIndexByFieldId(CONST_fieldId_zb)) < 0) goto errLabel;
			if (index >= mycountof(pRegInfo->u.cols)) goto errLabel;
			pDesc->pZb = pRegInfo->fields[index].ptr;
			//
			if ((index = getRegIndexByFieldId(CONST_fieldId_bb)) < 0) goto errLabel;
			if (index >= mycountof(pRegInfo->u.cols)) goto errLabel;
			pDesc->pBb = pRegInfo->fields[index].ptr;
			//
			if ((index = getRegIndexByFieldId(CONST_fieldId_dw)) < 0) goto errLabel;
			if (index >= mycountof(pRegInfo->u.cols)) goto errLabel;
			pDesc->pDw = pRegInfo->fields[index].ptr;
			//
			if ((index = getRegIndexByFieldId(CONST_fieldId_bm)) < 0) goto errLabel;
			if (index >= mycountof(pRegInfo->u.cols)) goto errLabel;
			pDesc->pBm = pRegInfo->fields[index].ptr;
			//
			if ((index = getRegIndexByFieldId(CONST_fieldId_syr)) < 0) goto errLabel;
			if (index >= mycountof(pRegInfo->u.cols)) goto errLabel;
			pDesc->pSyr = pRegInfo->fields[index].ptr;
			
			if (!pDesc->pZb) pDesc->pZb = _T("");
			if (!pDesc->pBb) pDesc->pBb = _T("");
			if (!pDesc->pDw) pDesc->pDw = _T("");
			if (!pDesc->pBm) pDesc->pBm = _T("");
			if (!pDesc->pSyr) pDesc->pSyr = _T("");

			 */
			pDesc.pZb = "";
			pDesc.pBb = "";
			pDesc.pDw = "";
			pDesc.pBm = "";
			pDesc.pSyr = "";
			//
			pDesc.pDw = new string(pRegInfo->dw);
			pDesc.pBm = new string(pRegInfo->bm);
			pDesc.pSyr = new string(pRegInfo->syr);
			//			
			string str;
			//
			if (pDesc.pSyr.Length!=0)
			{
				if (talkerDesc != null)
				{
					str = string.Format("{0} - {1} - {2}",
						//_sntprintf(talkerDesc, uiCnt_talkerDesc, _T("%s - %s - %s"), 
						pDesc.pDw, pDesc.pBm, pDesc.pSyr);
					mytcsncpy(talkerDesc, str, (int)uiCnt_talkerDesc);
				}

				if (displayName != null)
				{
					//_sntprintf(displayName, uiCnt_displayName, _T("%s"), pDesc->pSyr);
					mytcsncpy(displayName, pDesc.pSyr, (int)uiCnt_displayName);
				}
			}
			else
			{
				if (talkerDesc != null)
				{
					//_sntprintf(talkerDesc, uiCnt_talkerDesc, _T("(%I64u)"), pRegInfo->addr.idInfo.ui64Id);
					str = string.Format("{0}", pRegInfo->addr.idInfo.ui64Id);
					mytcsncpy(talkerDesc, str, (int)uiCnt_talkerDesc);
				}
				if (displayName != null)
				{
					//_sntprintf(displayName, uiCnt_displayName, _T("(%I64u)"), pRegInfo->addr.idInfo.ui64Id);
					str = string.Format("{0}", pRegInfo->addr.idInfo.ui64Id);
					mytcsncpy(displayName, str, (int)uiCnt_displayName);

				}
			}
			//if (talkerDesc) tTrim(talkerDesc);
			//if (displayName) tTrim(displayName);

			iErr = 0;
		errLabel:

			if (iErr!=0)
			{
				/*
				pDesc->pZb = _T("");
				pDesc->pBb = _T("");
				pDesc->pDw = _T("");
				pDesc->pBm = _T("");
				pDesc->pSyr = _T("");
				*/
			}

			return iErr;
		}



		public static unsafe int grpInfo2Desc(void* p0, IM_GRP_INFO* pGrpInfo, char* talkerDesc, uint uiCnt_talkerDesc, char* displayName, uint uiCnt_displayName)
		{
			int iErr = -1;
			string str;

			if (pGrpInfo->name[0]!=0)
			{
				if (talkerDesc != null)
				{
					//_sntprintf(talkerDesc, uiCnt_talkerDesc, _T("%s"), pGrpInfo->name);
					mytcsncpy(talkerDesc, pGrpInfo->name, (int)uiCnt_talkerDesc);
				}
				if (displayName != null)
				{
					//_sntprintf(displayName, uiCnt_displayName, _T("%s"), pGrpInfo->name);
					mytcsncpy(displayName, pGrpInfo->name, (int)uiCnt_displayName);
				}
			}
			else
			{
				if (talkerDesc != null)
				{
					//_sntprintf(talkerDesc, uiCnt_talkerDesc, _T("(%I64u)"), pGrpInfo->idInfo.ui64Id);
					str = string.Format("({0})", pGrpInfo->idInfo.ui64Id);
					mytcsncpy(talkerDesc, str, (int)uiCnt_talkerDesc);
				}
				if (displayName != null)
				{
					//_sntprintf(displayName, uiCnt_displayName, _T("(%I64u)"), pGrpInfo->idInfo.ui64Id);
					str = string.Format("({0})", pGrpInfo->idInfo.ui64Id);
					mytcsncpy(displayName, str, (int)uiCnt_displayName);
				}
			}

			iErr = 0;
			//  errLabel:
			return iErr;
		}

	}
}
