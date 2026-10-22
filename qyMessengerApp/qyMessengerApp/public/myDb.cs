using System;
using System.Collections.Generic;
using System.Text;

public partial class Consts
{
	//  2014/05/19
	public const int CONST_uiMaxQNodes_qyImObjTab = 3000;
	public const int CONST_uiMaxQNodes_qyImObjRegInfoTab = 3000;
	public const int CONST_uiMaxQNodes_qyImObjRegInfoTab1 = 3000;
	public const int CONST_uiMaxQNodes_qyImGrpInfoTab = 3000;
	public const int CONST_uiMaxQNodes_qyImGrpMemTab = 10000;
	public const int CONST_uiMaxQNodes_qyImObjRuleTab = 1000;
	public const int CONST_uiMaxQNodes_qyDynBmpTab = 1000;


#if DEBUG
	public const int CONST_uiMaxQNodes_qyImTaskTab = 100;   //10	//  1000
	public const int CONST_uiMaxQNodes_qyImMsgTab = 100;
#else
public const int  CONST_uiMaxQNodes_qyImTaskTab		=	1000;
public const int  CONST_uiMaxQNodes_qyImMsgTab		=	1000;
#endif

}

namespace qy
{
	public unsafe struct QMEM_qyImObj
	{
		public QY_MESSENGER_INFO messengerInfo;
		public fixed char desc[64];
	}
	;

	public unsafe struct QMEM_qyImObjRegInfo
	{

		public 			 uint uiType;
		//
		public int id;

		public int iCustomId;

		public MSGR_ADDR addr;

		public fixed char regTime[Consts.CONST_qyTimeLen + 1];
		public fixed char auditTime[Consts.CONST_qyTimeLen + 1];
		public fixed char tLastModifiedTime[Consts.CONST_qyTimeLen + 1];
		public fixed char tCommitTime[Consts.CONST_qyTimeLen + 1];

		/*
		unsigned int uiFieldIds[CONST_qnmMaxRegFields];
		union												{
						 //  WCHAR											cols[CONST_qnmMaxRegFields][256];
						 WCHAR cols[CONST_qnmMaxRegFields][64  +  1];		//  2007/12/01, 因为MIS_MSGU过大，造成stack overflow,故将字段大小限制在64个字符.
					 }
	u;
		*/
		public fixed char dw[Consts.CONST_maxNameLen + 1];
		public fixed char bm[Consts.CONST_maxNameLen + 1];
		public fixed char syr[Consts.CONST_maxNameLen + 1];


	};


public unsafe delegate bool PF_bGetImAmRcdBySth(object pDbParam, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, string messengerName, int id, QY_imAm_RCD* pObj);


public unsafe delegate bool PF_bGetMessengerRegInfoBySth  (object pDbParam, int iDbType, ref QY_DMITEM[] pFieldIdTable, string pTabName, string misServName, QY_MESSENGER_ID* pIdInfo, int iRcdId, QY_MESSENGER_REGINFO* pRegInfo);
public unsafe delegate bool PF_bGetImOtherRegInfoBySth  (object pDbParam, int iDbType, ref QY_DMITEM[] pFieldIdTable, string pTabName, MSGR_ADDR* pAddr, int iRcdId, QY_MESSENGER_REGINFO* pRegInfo);
public unsafe delegate bool PF_bGetMeInfoBySth  (object pDbParam, int iDbType, string misServName, QY_MESSENGER_INFO* pObj);
public unsafe delegate bool PF_bGetMessengerInfoBySth  (QM_dbFuncs pDbFuncs, object pDbParam, int iDbType, ref QY_DMITEM[] pFieldIdTable, string misServName, ref QY_MESSENGER_ID pIdInfo, int iRcdId, ref QY_MESSENGER_INFO pObjParam, ref QY_MESSENGER_REGINFO pRegInfo);
public unsafe delegate int   PF_insertImObj_recoverMessenger  (QM_dbFuncs pDbFuncs, object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj, Int64 tLastModifiedTime);
public unsafe delegate int   PF_updateImObj_recoverMessenger   (object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj, Int64 tLastModifiedTime, int iRcdId);
public unsafe delegate int   PF_updateImObj_recoverMessenger2   (object pDbParam, int iDbType, QY_MESSENGER_INFO* pObj, Int64 tLastModifiedTime, int iRcdId);

public unsafe delegate int   PF_insertImObj_recoverImGrp  (object pDb, int iDbType, uint uiObjType, IM_GRP_INFO  * pGrpInfo, Int64  tLastModifiedTime  );
public unsafe delegate int   PF_updateImObj_recoverImGrp   (object pDb, int iDbType, uint uiObjType, IM_GRP_INFO  * pGrpInfo, Int64  tLastModifiedTime,  int iRcdId  );
public unsafe delegate int   PF_updateImObj_recoverImGrp2   (object pDb, int iDbType, uint uiObjType, IM_GRP_INFO  * pGrpInfo, Int64  tLastModifiedTime,  int iRcdId  );

public unsafe delegate bool PF_bGetImGrpInfoBySth  (object pDbParam, int iDbType, string misServName, ref QY_MESSENGER_ID pIdInfo, ref IM_GRP_INFO pRcd);
public unsafe delegate int   PF_insertImGrp_recoverImGrp   (object pDbParam, int iDbType, IM_GRP_INFO* pGrpInfo);
public unsafe delegate int   PF_updateImGrp_recoverImGrp   (object pDbParam, int iDbType, IM_GRP_INFO* pGrpInfo, int iRcdId);
public unsafe delegate bool PF_bGetImGrpMemBySth  (object pDbParam, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo_grp, QY_MESSENGER_ID* pIdInfo_mem, int iRcdId, IM_GRP_MEM* pRcd);
//  public unsafe delegate  int   PF_insertImGrpMem_recoverImGrpMem  (  void  *  pDb,  int  iDbType,  IM_GRP_MEM  *  pGrpMem  );
public unsafe delegate int   PF_insertImGrpMem_recoverImGrpMem   (object pDb, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime);
//  public unsafe delegate  int   PF_updateImGrpMem_recoverImGrpMem  (  void  *  pDb,  int  iDbType,  IM_GRP_MEM  *  pGrpMem,  int  iRcdId  );
public unsafe delegate int   PF_updateImGrpMem_recoverImGrpMem   (object pDb, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime, int iRcdId);
public unsafe delegate int   PF_updateImGrpMem_recoverImGrpMem2   (object pDbParam, int iDbType, IM_GRP_MEM* pGrpMem, Int64 tLastModifiedTime, int iRcdId);
public unsafe delegate bool PF_bIdInfoExistedInImObjRegInfoTab  (object pDbParam, int iDbType, QY_MESSENGER_ID* pIdInfo, string tabName);
public unsafe delegate int   PF_insertImObjRegInfo_qisRegUsrInfoInTab   (object pDbParam, int iDbType, QY_MESSENGER_REGINFO* pRegInfo, string tabName);
public unsafe delegate int   PF_updateImObjRegInfo_qisRegUsrInfoInTab   (object pDbParam, int iDbType, QY_MESSENGER_REGINFO* pRegInfo, string tabName);

	
	
//public unsafe delegate  int   PF_qnmRegUsrInfo  (QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QNM_PC_INFO* pPcInfo, QNM_REG_INFO* pRegInfo);
public unsafe delegate  int   PF_qisRegUsrInfo  (QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM[] pFieldIdTable, QY_MESSENGER_INFO* pMessengerInfo, QY_MESSENGER_REGINFO* pRegInfo, bool bLogReRegEvent);
	/*
public unsafe delegate  bool PF_bSelectImMsg_logImMsg  (object pDb, int iDbType, ref QY_DMITEM pFieldIdTable, IM_MSG_RCD* pRcd);
public unsafe delegate  int   PF_insertImMsg_logImMsg  (QM_dbFuncs pDbFuncs, object pDb, int iDbType, ref QY_DMITEM pFieldIdTable, IM_MSG_RCD* pRcd);
public unsafe delegate  int   PF_auditNewMessenger  (void* pReserved, QY_MESSENGER_ID* pLastIdInfo, void* pLicenseCtx, object pDb, int iDbType, POLICY_imAuthCond* pAuthCond, QY_MESSENGERAUTHTMP_RCD* pNewMessenger);
public unsafe delegate  bool PF_bGetImMsgRcdBySth  (object pDbParam, int iDbType, int id, IM_MSG_RCD* pObj);
public unsafe delegate  bool PF_bGetPcInfoByMac  (object pDbParam, char* mac, bool ucbMac0, QY_DMITEM  * pFieldIdTable, QNM_PC_INFO  * pPcInfo, QNM_REG_INFO  * pRegInfo  );
public unsafe delegate  bool PF_bGetPcInfoByMacEx  (object pDbParam, char* mac, bool ucbMac0, QY_DMITEM  * pFieldIdTable, QNM_PC_INFO  * pPcInfo, QNM_REG_INFO  * pRegInfo, QNM_REG_INFO  * pRegInfoInTab1  );
public unsafe delegate  bool PF_bLogEventFunc_is  (QM_dbFuncs pDbFuncs, object pDb, int iDbType, QIS_EVENT* pEvent);
public unsafe delegate  int   PF_getEventInfoBySth  (object pDbParam, int iDbType, QIS_EVENT* pEvent);
public unsafe delegate  int   PF_getPcEventInfoByMac0  (object pDbParam, QNM_PC_EVENT* pEvent);
public unsafe delegate  bool PF_bLogPcEventFunc  (object pDb, int iDbType, QNM_PC_EVENT* pEvent);
public unsafe delegate  int   PF_qisRegOtherInfoInTab  (object pDb, int iDbType, ref QY_DMITEM pFieldIdTable, MSGR_ADDR* pAddr, QY_MESSENGER_REGINFO* pRegInfo, string tabName);
public unsafe delegate  bool PF_bClearupImMsg_isCli  (object pDb, int iDbType, string wherePart);
public unsafe delegate  bool PF_bClearupEvent_is  (QY_CLEARUPINFO* pClearupInfo  );
public unsafe delegate  bool PF_bClearupImMsg  (QY_CLEARUPINFO* pClearupInfo  );
public unsafe delegate  bool PF_bClearupImHisTask  (QY_CLEARUPINFO* pClearupInfo  );
public unsafe delegate  int   PF_getObjIndex  (QY_OBJ_DB* pObjDb, int iObjId, QY_OBJINDEX_RCD* pObjIndex);
	*/

//public unsafe delegate  int   PF_listObjs_dlgMsgList  (object pDbParam, void* pQueryParam, HWND hListCtrlParam, QY_MESSENGER_ID* pIdInfoParam);
//
public unsafe delegate  bool PF_bGetMessengerPhoneInfoBySth  (object pDbParam, int iDbType, string misServName, uint uiDevType, string  wDevIdStr,  QY_MESSENGER_ID* pIdInfo,  int id, QY_MESSENGER_phoneInfo  * pRcd  );
public unsafe delegate  int   PF_recoverMessengerPhoneInfo  (QM_dbFuncs pDbFuncs, object pDb, int iDbType, MSGR_ADDR* pAddr, void* p1);

public unsafe delegate  int   PF_updateImObj_me  (object pDb, int iDbType, QY_MESSENGER_INFO* pObj_me);
public unsafe delegate  bool PF_bGetImObjRulesReq  (object pDbParam, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, QY_MESSENGER_ID* pIdInfo_related, string condPart, ushort usMaxMemsPerSnd, REFRESH_imObjRules_req  * pReq  );
public unsafe delegate  bool PF_bGetImObjRuleRcdBySth  (object pDbParam, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRule_reqMem* pMem, int iRcdId, QY_imObjRule_rcd* pRcd);
public unsafe delegate  int   PF_insertImObjRule_recoverImObjRules  (object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRule_reqMem* pReqMem, Int64 tLastModifiedTime);
public unsafe delegate  int   PF_deleteImObjRule_recoverImObjRules  (object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRule_reqMem* pReqMem, int rcdId);
public unsafe delegate  int   PF_updateImObjRule_recoverImObjRules  (object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRule_reqMem* pReqMem, Int64 tLastModifiedTime, int rcdId);
public unsafe delegate  int   PF_updateImObjRule_recoverImObjRules2  (object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, REFRESH_imObjRule_reqMem* pReqMem, Int64 tLastModifiedTime, int rcdId);

//
public unsafe delegate  int   PF_updateImObj_refreshImGrps_end  (object pDb, int iDbType, string misServName, uint uiObjType, Int64  tLastModifiedTime  );


public unsafe delegate  bool PF_bNeedRetrieveImGrpMems  (object pDb, int iDbType, string misServName);
public unsafe delegate  int   PF_updateImGrpMem_refreshImGrpMems_end  (object pDb, int iDbType, string misServName, Int64 tLastModifiedTime);


public unsafe delegate  int   PF_updateImObj_refreshContacts_end  (object pDb, int iDbType, string misServName, Int64 tLastModifiedTime);

//
public unsafe delegate  int   PF_deleteOldImObjRule_mcProcResp_refreshImObjRulesReq  (void* pSubThreadInfoParam, void* pSciParam, void* pMisCntParam, MIS_MSG_INPUT* pMsgInput, void* pCtxParam);
public unsafe delegate  int   PF_selectNextImObjRule_mcProcResp_refreshImObjRulesReq  (void* pSubThreadInfoParam, void* pSciParam, void* pMisCntParam, MIS_MSG_INPUT* pMsgInput, void* pCtxParam, QY_MESSENGER_ID* pIdInfo_related);

	/*
public unsafe delegate  void   PF_fillImObjs_msgr  (HWND hDlg, void* pVar, object pDb);
public unsafe delegate  void   PF_fillImObjs_grp  (HWND hDlg, void* pVar, object pDb);
public unsafe delegate  int   PF_selectImTask_dlgTalk_loadTasks  (HWND hDlgTalk, void* pTmpParam, object pDbParam);
*/	
public unsafe delegate  bool PF_bSelectImTask_storeTaskInDb  (object pDb, int iDbType, object pMisCntParam, IM_TASK_RCD* pRcd, bool bInHis);
public unsafe delegate  int   PF_insertImTask_storeTaskInDb  (object pDb, int iDbType, object pMisCntParam, int iTaskId_suggested, IM_TASK_RCD* pRcd, bool bInHis);
public unsafe delegate  int   PF_updateTaskStatus  (object pDb, int iStatus, int iTaskId);
/*
public unsafe delegate  bool PF_bMoveTask2His  (void* pMisCntParam, object pDb, int iDbType);
public unsafe delegate  bool PF_bClearupDeletedImObjs_isClient  (void* pMisCntParam, object pDb, int iDbType);
*/
public unsafe delegate  bool PF_bGetImTaskRcdBySth  (object pDb, int iDbType, bool bHisTab, string misServName, QY_MESSENGER_ID pIdInfo, Int64 tSendTime, uint uiTranNo,  int id, ref IM_TASK_RCD   pRcd  );
	
//
//
//public unsafe delegate  bool PF_bGetQyDynBmpRcdBySth  (object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, uint uiObjType, ushort usIndex,  int id, QY_dynBmp_RCD  * pRcd  );
//
public unsafe delegate  bool PF_bViewImTaskList_unproced  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);
public unsafe delegate  bool PF_bViewImTaskList_cur  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);
public unsafe delegate  bool PF_bViewImTaskList_his  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);
public unsafe delegate  bool PF_bViewImObjList  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);
public unsafe delegate  bool PF_bViewRegInfoList  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);
public unsafe delegate  bool PF_bViewImGrpList  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);
public unsafe delegate  bool PF_bViewImGrpMemList_isClient  (ref QY_OBJ_DB pObjDb, string docName, string wherePart);

//
public unsafe delegate  bool PF_bViewImObjRuleList_is  (ref QY_OBJ_DB pObjDb  );
//public unsafe delegate   bool   PF_bViewSharedDynBmpList_isClient  (  QY_OBJ_DB  *  pObjDb,  string  docName,  string  whereClause  );

//
public unsafe delegate  bool PF_bInsertPhoneGuest_procMsgTalk_work  (object pDbParam, int iDbType, void* pQyMcParam, MIS_MSG_TALK* pMsg, void* pGuestParam);
public unsafe delegate  bool PF_bUpdateImTask_bChkQyMcEnv_mis  (object pDb, int iStatus_set, int iStatus_0, int iStatus_1);
//  public unsafe delegate   int   PF_deleteDynBmp_tmpHandler_delOld_dynBmpQ  (  void  *  pDb,  int  id  );
public unsafe delegate  int   PF_deleteOldDynBmp_mcProcResp_retrieveDynBmps  (object pDb, int iDbType, char* pTimeBuf);
//public unsafe delegate   int   PF_init_mcProcResp_retrieveDynBmps  (  void  *  pDb,  void  *  pCtxParam  );
public unsafe delegate  bool PF_bInsertDynBmp_mcProcResp_retrieveDynBmps  (object pDb, void* pMisCntParam, QIS_dynBmp_info* pMem, char* pTimeBuf);
public unsafe delegate  bool PF_bUpdateDynBmp_mcProcResp_retrieveDynBmps  (object pDb, QIS_dynBmp_info* pMem, char* pTimeBuf, int rcdId);
public unsafe delegate  bool PF_bUpdateImObjRegInfo_procMsgInput_scheduler  (object pDb, void* pSciParam, void* pMisCntParam, QY_MESSENGER_ID* pIdInfo);

public unsafe delegate  bool PF_bStatImTasks  (void* pProcInfoParam, void* pMisCntParam, object pDb, int iDbType);
public unsafe delegate  bool PF_bDeleteFromTabById  (object pDb, int iDbType, string pTabName, int id);
public unsafe delegate  bool PF_bDeleteFromTabByIdInfo  (object pDb, int iDbType, string pTabName, string misServName, QY_MESSENGER_ID* pIdInfo);
public unsafe delegate  bool PF_bDeleteImOtherRegInfoByAddr  (object pDb, int iDbType, string misServName, string pTabName, MSGR_ADDR* pAddr);
	/*
public unsafe delegate  int   PF_iSelectImGrpMem_dlgSelect_isCli  (object pDb, int iDbType, string misServName, QY_MESSENGER_ID* pIdInfo, HWND idc_hWnd);
public unsafe delegate  int   PF_iSelectImObj_dlgSelect_isCli  (object pDb, int iDbType, string misServName, HWND idc_hWnd);
#if  0
public unsafe delegate   int   PF_iSelectImObjAuth2_dlgSelect_isCli  (  void  *  pDb, int  iDbType,  string  misServName,  HWND  idc_hWnd  );
#endif
public unsafe delegate  int   PF_iSelectDynBmp_dlgSelect_isCli  (object pDb, int iDbType, string misServName, HWND idc_hWnd);
*/


	public class QM_dbFuncs
	{

		public PF_bGetImAmRcdBySth pf_bGetImAmRcdBySth;
		
		public PF_bGetMessengerRegInfoBySth pf_bGetMessengerRegInfoBySth;
		public PF_bGetImOtherRegInfoBySth pf_bGetImOtherRegInfoBySth;
		public PF_bGetMeInfoBySth pf_bGetMeInfoBySth;
		public PF_bGetMessengerInfoBySth pf_bGetMessengerInfoBySth;
		public PF_insertImObj_recoverMessenger pf_insertImObj_recoverMessenger;
		public PF_updateImObj_recoverMessenger pf_updateImObj_recoverMessenger;
		public PF_updateImObj_recoverMessenger2 pf_updateImObj_recoverMessenger2;

		public PF_insertImObj_recoverImGrp pf_insertImObj_recoverImGrp;
		public PF_updateImObj_recoverImGrp pf_updateImObj_recoverImGrp;
		public PF_updateImObj_recoverImGrp2 pf_updateImObj_recoverImGrp2;
		public PF_bGetImGrpInfoBySth pf_bGetImGrpInfoBySth;
		public PF_insertImGrp_recoverImGrp pf_insertImGrp_recoverImGrp;
		public PF_updateImGrp_recoverImGrp pf_updateImGrp_recoverImGrp;
		public PF_bGetImGrpMemBySth pf_bGetImGrpMemBySth;
		public PF_insertImGrpMem_recoverImGrpMem pf_insertImGrpMem_recoverImGrpMem;
		public PF_updateImGrpMem_recoverImGrpMem pf_updateImGrpMem_recoverImGrpMem;
		public PF_updateImGrpMem_recoverImGrpMem2 pf_updateImGrpMem_recoverImGrpMem2;
		public PF_bIdInfoExistedInImObjRegInfoTab pf_bIdInfoExistedInImObjRegInfoTab;
		public PF_insertImObjRegInfo_qisRegUsrInfoInTab pf_insertImObjRegInfo_qisRegUsrInfoInTab;
		public PF_updateImObjRegInfo_qisRegUsrInfoInTab pf_updateImObjRegInfo_qisRegUsrInfoInTab;

		
		//PF_qnmRegUsrInfo pf_qnmRegUsrInfo;
		public PF_qisRegUsrInfo pf_qisRegUsrInfo;
		/*
		PF_bSelectImMsg_logImMsg pf_bSelectImMsg_logImMsg;
		PF_insertImMsg_logImMsg pf_insertImMsg_logImMsg;
		PF_auditNewMessenger pf_auditNewMessenger;
		PF_bGetImMsgRcdBySth pf_bGetImMsgRcdBySth;
		PF_bLogEventFunc_is pf_bLogEventFunc_is;
		PF_bGetPcInfoByMac pf_bGetPcInfoByMac;
		PF_bGetPcInfoByMacEx pf_bGetPcInfoByMacEx;
		PF_getEventInfoBySth pf_getEventInfoBySth;
		PF_getPcEventInfoByMac0 pf_getPcEventInfoByMac0;
		PF_bLogPcEventFunc pf_bLogPcEventFunc;
		PF_qisRegOtherInfoInTab pf_qisRegOtherInfoInTab;
		PF_bClearupImMsg_isCli pf_bClearupImMsg_isCli;
		PF_bClearupEvent_is pf_bClearupEvent_is;
		PF_bClearupImMsg pf_bClearupImMsg;
		PF_bClearupImHisTask pf_bClearupImHisTask;
		PF_getObjIndex pf_getObjIndex;

		PF_bGetMessengerPhoneInfoBySth pf_bGetMessengerPhoneInfoBySth;
		PF_recoverMessengerPhoneInfo pf_recoverMessengerPhoneInfo;
		*/
		public PF_updateImObj_me pf_updateImObj_me;
		/*
		PF_bGetImObjRulesReq pf_bGetImObjRulesReq;
		PF_bGetImObjRuleRcdBySth pf_bGetImObjRuleRcdBySth;
		PF_insertImObjRule_recoverImObjRules pf_insertImObjRule_recoverImObjRules;
		PF_deleteImObjRule_recoverImObjRules pf_deleteImObjRule_recoverImObjRules;
		PF_updateImObjRule_recoverImObjRules pf_updateImObjRule_recoverImObjRules;
		PF_updateImObjRule_recoverImObjRules2 pf_updateImObjRule_recoverImObjRules2;

		*/
		//  isCli
		public PF_updateImObj_refreshImGrps_end pf_updateImObj_refreshImGrps_end;
		//
		public PF_bNeedRetrieveImGrpMems pf_bNeedRetrieveImGrpMems;
		//
		public PF_updateImGrpMem_refreshImGrpMems_end pf_updateImGrpMem_refreshImGrpMems_end;
		//
		public PF_updateImObj_refreshContacts_end pf_updateImObj_refreshContacts_end;

		/*
		//
		PF_deleteOldImObjRule_mcProcResp_refreshImObjRulesReq pf_deleteOldImObjRule_mcProcResp_refreshImObjRulesReq;
		PF_selectNextImObjRule_mcProcResp_refreshImObjRulesReq pf_selectNextImObjRule_mcProcResp_refreshImObjRulesReq;

		PF_fillImObjs_msgr pf_fillImObjs_msgr;
		PF_fillImObjs_grp pf_fillImObjs_grp;
		PF_selectImTask_dlgTalk_loadTasks pf_selectImTask_dlgTalk_loadTasks;
		*/
		public PF_bSelectImTask_storeTaskInDb pf_bSelectImTask_storeTaskInDb;
		public PF_insertImTask_storeTaskInDb pf_insertImTask_storeTaskInDb;
		public PF_updateTaskStatus pf_updateTaskStatus;
		/*
		PF_bMoveTask2His pf_bMoveTask2His;
		PF_bClearupDeletedImObjs_isClient pf_bClearupDeletedImObjs_isClient;
		*/
		public PF_bGetImTaskRcdBySth pf_bGetImTaskRcdBySth;
		/*
		//
		//PF_bSelectImGrpMem_dlgTalk_OnInitDialog	pf_bSelectImGrpMem_dlgTalk_OnInitDialog;

		PF_listObjs_dlgMsgList pf_listObjs_dlgMsgList;
		PF_bViewImTaskList_unproced pf_bViewImTaskList_unproced;
		PF_bViewImTaskList_cur pf_bViewImTaskList_cur;
		PF_bViewImTaskList_his pf_bViewImTaskList_his;

		PF_bViewImObjList pf_bViewImObjList;
		PF_bViewRegInfoList pf_bViewRegInfoList;
		PF_bViewImGrpList pf_bViewImGrpList;
		PF_bViewImGrpMemList_isClient pf_bViewImGrpMemList_isClient;        //  2014/04/18
		PF_bViewImObjRuleList_is pf_bViewImObjRuleList_is;          //  2014/04/18

		//
		PF_bGetQyDynBmpRcdBySth pf_bGetQyDynBmpRcdBySth;
		PF_bInsertPhoneGuest_procMsgTalk_work pf_bInsertPhoneGuest_procMsgTalk_work;
		PF_bUpdateImTask_bChkQyMcEnv_mis pf_bUpdateImTask_bChkQyMcEnv_mis;
		PF_deleteOldDynBmp_mcProcResp_retrieveDynBmps pf_deleteOldDynBmp_mcProcResp_retrieveDynBmps;
		//
		PF_bInsertDynBmp_mcProcResp_retrieveDynBmps pf_bInsertDynBmp_mcProcResp_retrieveDynBmps;
		PF_bUpdateDynBmp_mcProcResp_retrieveDynBmps pf_bUpdateDynBmp_mcProcResp_retrieveDynBmps;
		PF_bUpdateDynBmp_mcProcResp_retrieveDynBmps pf_bUpdateDynBmp_mcProcResp_retrieveDynBmps2;
		//
		PF_bUpdateImObjRegInfo_procMsgInput_scheduler pf_bUpdateImObjRegInfo_procMsgInput_scheduler;
		PF_bStatImTasks pf_bStatImTasks;
		PF_bDeleteFromTabById pf_bDeleteFromTabById;
		PF_bDeleteFromTabByIdInfo pf_bDeleteFromTabByIdInfo;
		PF_bDeleteImOtherRegInfoByAddr pf_bDeleteImOtherRegInfoByAddr;
		PF_iSelectImGrpMem_dlgSelect_isCli pf_iSelectImGrpMem_dlgSelect_isCli;
		PF_iSelectImObj_dlgSelect_isCli pf_iSelectImObj_dlgSelect_isCli;
		//
		PF_iSelectDynBmp_dlgSelect_isCli pf_iSelectDynBmp_dlgSelect_isCli;
		*/

	};

	public class CMyDb
{
		bool m_var_bInited;

		GENERIC_Q[] m_var_q_tabs;	//[20];

		//
		public GENERIC_Q m_var_pQ_qyImObjTab;
		public GENERIC_Q m_var_pQ_qyImObjRegInfoTab;
		public GENERIC_Q m_var_pQ_qyImObjRegInfoTab1;
		public GENERIC_Q m_var_pQ_qyImGrpInfoTab;
		public GENERIC_Q m_var_pQ_qyImGrpMemTab;
		public GENERIC_Q m_var_pQ_qyImObjRuleTab;
		public GENERIC_Q m_var_pQ_qyImTaskTab;
		public GENERIC_Q m_var_pQ_qyImMsgTab;
		public GENERIC_Q m_var_pQ_qyDynBmpTab;


		//
		public CMyDb(  ) 
		{  
			//memset(  &m_var,  0,  sizeof(m_var  )  );
		}

	 ~CMyDb()
	{
		if (m_var_bInited) qyCloseDb();
	}




	public unsafe bool bQyOpenDb(string connectStr)
	{
		bool bRet = false;

		string pTabName;
		uint uiMaxQNodes;
		int index = 0;

		//
		if (m_var_q_tabs != null) return false;
		m_var_q_tabs = new GENERIC_Q[15];
		for ( int i = 0; i< m_var_q_tabs.Length; i ++ )
        {
				m_var_q_tabs[i] = new GENERIC_Q();
        }

		//
		pTabName = Consts.CONST_tabName_qyImObjTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImObjTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImObjTab = m_var_q_tabs[index];
		index++;
		pTabName = Consts.CONST_tabName_qyImObjRegInfoTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImObjRegInfoTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImObjRegInfoTab = m_var_q_tabs[index];
		index++;
		pTabName = Consts.CONST_tabName_qyImObjRegInfoTab1;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImObjRegInfoTab1;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImObjRegInfoTab1 = m_var_q_tabs[index];
		index++;
		pTabName = Consts.CONST_tabName_qyImGrpInfoTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImGrpInfoTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImGrpInfoTab = m_var_q_tabs[index];
		index++;
		pTabName = Consts.CONST_tabName_qyImGrpMemTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImGrpMemTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImGrpMemTab = m_var_q_tabs[index];
		index++;

		pTabName = Consts.CONST_tabName_qyImObjRuleTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImObjRuleTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImObjRuleTab = m_var_q_tabs[index];
		index++;

		pTabName = Consts.CONST_tabName_qyImTaskTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImTaskTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImTaskTab = m_var_q_tabs[index];
		index++;

		pTabName = Consts.CONST_tabName_qyImMsgTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyImMsgTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyImMsgTab = m_var_q_tabs[index];
		index++;

		pTabName = Consts.CONST_tabName_qyDynBmpTab;
		uiMaxQNodes = Consts.CONST_uiMaxQNodes_qyDynBmpTab;
		if (qyFuncs.initGenericQ(pTabName, pTabName, 0, uiMaxQNodes, qyFuncs.mymalloc, qyFuncs.myfree, m_var_q_tabs[index])!=0) goto errLabel;
		m_var_pQ_qyDynBmpTab = m_var_q_tabs[index];
		index++;
		//
		if (index > m_var_q_tabs.Length) goto errLabel;

		bRet = true;

	errLabel:
		if (bRet)
		{
			m_var_bInited = true;
		}
		if (!bRet)
		{
			qyCloseDb();
		}
		return bRet;
	}


	public unsafe void qyCloseDb()
	{
		int i;

		for (i = 0; i < m_var_q_tabs.Length; i++)
		{
			qyFuncs.exitGenericQ(m_var_q_tabs[i]);
		}

		m_var_bInited = false;
	}


}




}
