using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe  int recoverImGrp(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM []pFieldIdTable, uint uiObjType, IM_GRP_INFO* pGrpInfo, Int64 tLastModifiedTime, bool bNoGrpName)
		{
			int iErr = -1;

			if (null==pDbFuncs) return -1;
			QM_dbFuncs  g_dbFuncs = pDbFuncs;

			if (null==pGrpInfo) return -1;

			//  再在qyImObjTab中记录
			QY_MESSENGER_INFO objInfo=new QY_MESSENGER_INFO();
			QY_MESSENGER_REGINFO regInfo = new QY_MESSENGER_REGINFO();
			//
			if (!g_dbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pDb, iDbType, ref pFieldIdTable, new string(pGrpInfo->misServName), ref pGrpInfo->idInfo, 0, ref objInfo, ref regInfo))
			{

				if (0!=g_dbFuncs.pf_insertImObj_recoverImGrp(pDb, iDbType, uiObjType, pGrpInfo, tLastModifiedTime)) goto errLabel;
			}
			else
			{
				if (objInfo.uiType != uiObjType || objInfo.iStatus != Consts.CONST_qyStatus_ok)
				{

					if (0!=g_dbFuncs.pf_updateImObj_recoverImGrp(pDb, iDbType, uiObjType, pGrpInfo, tLastModifiedTime, objInfo.iRcdId)) goto errLabel;
				}
				else
				{
					if (0!=tLastModifiedTime)
					{
						if (0!=g_dbFuncs.pf_updateImObj_recoverImGrp2(pDb, iDbType, uiObjType, pGrpInfo, tLastModifiedTime, objInfo.iRcdId)) goto errLabel;
					}

				}
			}


			//	 如果没有组名，那就只修复上面的imObjTab. 2014/02/03
			if (bNoGrpName)
			{
				traceLog(("recoverImGrp: no grpInfo, skipped"));
			}
			else
			{

				IM_GRP_INFO grpInfo=new IM_GRP_INFO();

				//  再在qyImGrpInfoTab留下组名
				if (!g_dbFuncs.pf_bGetImGrpInfoBySth(pDb, iDbType, new string(pGrpInfo->misServName), ref pGrpInfo->idInfo, ref grpInfo))
				{

					g_dbFuncs.pf_insertImGrp_recoverImGrp(pDb, iDbType, pGrpInfo);

				}
				else
				{
					if ((0!=pGrpInfo->name[0] && 0!=mylstrcmpi(pGrpInfo->name, grpInfo.name))
						|| pGrpInfo->usSubtype != grpInfo.usSubtype
						|| pGrpInfo->idInfo_creator.ui64Id != grpInfo.idInfo_creator.ui64Id)
					{
						g_dbFuncs.pf_updateImGrp_recoverImGrp(pDb, iDbType, pGrpInfo, grpInfo.id);
					}
				}
			}

			iErr = 0;
		errLabel:
			return iErr;
		}

		//  
		public static unsafe int recoverImGrpMem(QM_dbFuncs pDbFuncs, object pDb, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime)
		{
			int iErr = -1;

			if (null==pDbFuncs) return -1;
			QM_dbFuncs  g_dbFuncs = pDbFuncs;
			//
			//TCHAR sqlBuf[CONST_maxSqlBufLen + 1] = _T("");
			//byte timeBuf[CONST_qyTimeLen + 1];

			if (null==pGrpMem) return -1;

			//getCurTime(timeBuf);

			//  
			IM_GRP_MEM objInfo;
			//
			if (!g_dbFuncs.pf_bGetImGrpMemBySth(pDb, iDbType, new string(pGrpMem->misServName), &pGrpMem->idInfo_grp, &pGrpMem->idInfo_mem, 0, &objInfo))
			{

				//  if  (  g_dbFuncs.pf_insertImGrpMem_recoverImGrpMem(  pDb,  iDbType,  pGrpMem  )  )  goto  errLabel;
				if (0!=g_dbFuncs.pf_insertImGrpMem_recoverImGrpMem(pDb, iDbType, pGrpMem, tLastModifiedTime)) goto errLabel;
			}
			else
			{
				if (objInfo.iRole != pGrpMem->iRole || objInfo.iStatus != pGrpMem->iStatus)
				{

					//  if  (  g_dbFuncs.pf_updateImGrpMem_recoverImGrpMem(  pDb,  iDbType,  pGrpMem,  objInfo.id  )  )  goto  errLabel;
					if (0!=g_dbFuncs.pf_updateImGrpMem_recoverImGrpMem(pDb, iDbType, pGrpMem, tLastModifiedTime, objInfo.id)) goto errLabel;

				}
				else
				{

					if (0!=tLastModifiedTime)
					{
						if (0!=g_dbFuncs.pf_updateImGrpMem_recoverImGrpMem2(pDb, iDbType, pGrpMem, tLastModifiedTime, objInfo.id)) goto errLabel;
					}

				}
			}

			iErr = 0;
		errLabel:
			return iErr;
		}


		public static unsafe  int recoverMessenger(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QY_MESSENGER_INFO* pObj, QY_MESSENGER_REGINFO* pRegInfo, Int64 tLastModifiedTime, bool bLog, GENERIC_Q pLogQ)
		{
			int iErr = -1;
			//
			//
	 if  (  null==pDbFuncs  )  {
				 traceLog(  (  "recoverMessenger failed, p_g_dbFuncs is null"  )  );
			return -1;
		}
		QM_dbFuncs    g_dbFuncs  =  (QM_dbFuncs  ) pDbFuncs;
		//
		QY_MESSENGER_INFO messengerInfo;

			/*
		char ip12Buf[CONST_qyMaxIpLen + 1] = "";                    //  2007/02/20
		char detectedIp12Buf[CONST_qyMaxIpLen + 1] = "";
		char idStr[CONST_qyMessengerIdStrLen + 1] = "";
		char startTimeBuf[CONST_qyTimeLen + 1] = "";
		char tLastModifiedTimeBuf[CONST_qyTimeLen + 1] = "";
			*/

	 if  (  null==pObj  &&  null==pRegInfo  )  return  -1;


	 //
	 if  (  null==pObj  )  {
		 mymemset(  (IntPtr)(byte*)&messengerInfo,  0,  sizeof(QY_MESSENGER_INFO  )  );
		//
		//lstrcpyn(messengerInfo.misServName, pRegInfo->addr.misServName, mycountof(messengerInfo.misServName  )  );
		messengerInfo.idInfo.ui64Id  =  pRegInfo->addr.idInfo.ui64Id;
		 messengerInfo.uiType  =  pRegInfo->addr.uiObjType;		//  CONST_objType_messenger;
		 messengerInfo.iStatus  =  0;			
		 //
		 pObj  =  &messengerInfo;
	 }

	 if  (  null==pObj  ||  !bMessengerIdValid(  &pObj->idInfo  )  )  goto  errLabel;

	 if  (  0==pObj->startTime  )  mytime(  ref pObj->startTime  );
	 if  (  0==pObj->tLastModifiedTime  )  mytime(  ref pObj->tLastModifiedTime  );

			//
			/*
			if  (  !idInfo2Str(  &pObj->idInfo, idStr, mycountof(idStr  )  )  )  goto  errLabel;
			//
			getTimeStr(pObj->startTime, startTimeBuf, mycountof(startTimeBuf  )  );
		   getTimeStr(pObj->tLastModifiedTime, tLastModifiedTimeBuf, mycountof(tLastModifiedTimeBuf  )  );
		   //		    
		   qyStdToIp12(pObj->ip, ip12Buf  );   //  2007/02/20
		   qyStdToIp12(pObj->detectedIp, detectedIp12Buf  );
			*/

			//
			//  2012/07/15

			//
			QY_MESSENGER_INFO rcd = new QY_MESSENGER_INFO(); ;
			QY_MESSENGER_REGINFO regInfo = new QY_MESSENGER_REGINFO();

	 if  (  !g_dbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pDb, iDbType, ref pFieldIdTable, new string(pObj->misServName),  ref pObj->idInfo,  0,  ref rcd,  ref regInfo  )  )  {  

		 if  (0!=g_dbFuncs.pf_insertImObj_recoverMessenger(pDbFuncs, pDb, iDbType, pObj, tLastModifiedTime)  )  goto  errLabel;

		 if  (bLog  )  {
			 if  (0!=postEvent2Log_is(pObj->misServName,  Consts.CONST_qyEventType_im_newMessenger,  pObj->detectedIp,  pObj->ip,  &pObj->idInfo,  null,  pLogQ  )  )  traceLogA(  "postEvent2Log_is failed."  );
}
	 
		}
	 else
{
	//if  (  rcd.uiType  !=  CONST_objType_me  )  
	{
		if (rcd.uiType != pObj->uiType || rcd.iRole != pObj->iRole || pObj->iStatus != rcd.iStatus)
		{

			if (0!=g_dbFuncs.pf_updateImObj_recoverMessenger(pDb, iDbType, pObj, tLastModifiedTime, rcd.iRcdId)) goto errLabel;
		}
		else
		{
			if (0!=tLastModifiedTime)
			{
				if (0!=g_dbFuncs.pf_updateImObj_recoverMessenger2(pDb, iDbType, pObj, tLastModifiedTime, rcd.iRcdId)) goto errLabel;
			}
		}
	}
}

if (pRegInfo!=null)
{

	g_dbFuncs.pf_qisRegUsrInfo(pDbFuncs, pDb, iDbType, ref pFieldIdTable, pObj, pRegInfo, bLog);

	//g_dbFuncs.pf_recoverMessengerPhoneInfo(pDbFuncs, pDb, iDbType, &pRegInfo->addr, 0);

}

iErr = 0;

errLabel:

if (0!=iErr)
{
	traceLogA("recoverMessenger failed.");
}
return iErr;

}

		public static unsafe int qisRegUsrInfoInTab(QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QY_MESSENGER_INFO* pMessengerInfo, QY_MESSENGER_REGINFO* pRegInfo, string tabName)
		{
			int iErr = -1;

			//
			//traceLogA("qisRegUsrInfoInTab enters");


			//
			if (null==pDbFuncs) goto errLabel;
			QM_dbFuncs  g_dbFuncs = pDbFuncs;


			if (null==pRegInfo) goto errLabel;
			if (null==pMessengerInfo)
			{
				if (!bMessengerIdValid(&pRegInfo->addr.idInfo)) goto errLabel;
			}
			else
			{
				if (!bMessengerIdValid(&pMessengerInfo->idInfo)) goto errLabel;
				pRegInfo->addr.idInfo.ui64Id = pMessengerInfo->idInfo.ui64Id;
			}

			//  if  (  !idInfo2Str(  &pRegInfo->addr.idInfo,  idStr,  mycountof(  idStr  )  )  )  goto  errLabel;

			getCurTime(pRegInfo->tLastModifiedTime);

			/*
			MACRO_setPtrs(pFieldIdTable, pRegInfo[0], pRegInfo->fields);
			for (int i = 0; i < CONST_qnmMaxRegFields; i++)
			{
				if (pRegInfo->fields[i].bufSize) pRegInfo->fields[i].ptr[pRegInfo->fields[i].bufSize - 1] = 0;
			}
			*/


			//

			if (!g_dbFuncs.pf_bIdInfoExistedInImObjRegInfoTab(pDb, iDbType, &pRegInfo->addr.idInfo, tabName))
			{
				if (0!=g_dbFuncs.pf_insertImObjRegInfo_qisRegUsrInfoInTab(pDb, iDbType, pRegInfo, tabName)) goto errLabel;
				iErr = 0; goto errLabel;
			}


			if (0!=g_dbFuncs.pf_updateImObjRegInfo_qisRegUsrInfoInTab(pDb, iDbType, pRegInfo, tabName)) goto errLabel;

			iErr = 0;

		errLabel:

			//traceLogA("qisRegUsrInfoInTab leaves");

			return iErr;

		}


		public static unsafe int postEvent2Log_is(char * misServName, int lEventType, byte * detectedIp, byte * ip, QY_MESSENGER_ID* pIdInfo, QIS_EVENT* pEventInput, GENERIC_Q pLogQ)
        {
			return 0;
        }


	}
}
