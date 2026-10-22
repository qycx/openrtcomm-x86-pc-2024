using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
  partial  class qyFuncs
{

		//  2012/01/30
		public static unsafe int setQmDbFuncs_qm(int iDbType, QM_dbFuncs pDbFuncs)
		{
			int iErr = -1;

			//memset(pDbFuncs, 0, sizeof(pDbFuncs[0]));

			//
			//setQmDb_default_Funcs(pDbFuncs);

			//
			switch (iDbType)
			{
				case Consts.CONST_dbType_myDb:
					//
					pDbFuncs.pf_bGetImAmRcdBySth = bGetImAmRcdBySth_myDb;
					pDbFuncs.pf_bGetMessengerRegInfoBySth = bGetMessengerRegInfoBySth_myDb;
					
					pDbFuncs.pf_bGetImOtherRegInfoBySth = bGetImOtherRegInfoBySth_myDb;
					pDbFuncs.pf_bGetMeInfoBySth = bGetMeInfoBySth_myDb;
					
					pDbFuncs.pf_bGetMessengerInfoBySth = bGetMessengerInfoBySth_myDb;
					
					pDbFuncs.pf_insertImObj_recoverMessenger = insertImObj_recoverMessenger_myDb;
					pDbFuncs.pf_updateImObj_recoverMessenger = updateImObj_recoverMessenger_myDb;
					pDbFuncs.pf_updateImObj_recoverMessenger2 = updateImObj_recoverMessenger2_myDb;

					
					pDbFuncs.pf_insertImObj_recoverImGrp = insertImObj_recoverImGrp_myDb;
					pDbFuncs.pf_updateImObj_recoverImGrp = updateImObj_recoverImGrp_myDb;
					pDbFuncs.pf_updateImObj_recoverImGrp2 = updateImObj_recoverImGrp2_myDb;
					
					pDbFuncs.pf_bGetImGrpInfoBySth = bGetImGrpInfoBySth_myDb;
					pDbFuncs.pf_insertImGrp_recoverImGrp = insertImGrp_recoverImGrp_myDb;
					pDbFuncs.pf_updateImGrp_recoverImGrp = updateImGrp_recoverImGrp_myDb;
					//
					pDbFuncs.pf_bGetImGrpMemBySth = bGetImGrpMemBySth_myDb;

					pDbFuncs.pf_insertImGrpMem_recoverImGrpMem = insertImGrpMem_recoverImGrpMem_myDb;
					pDbFuncs.pf_updateImGrpMem_recoverImGrpMem = updateImGrpMem_recoverImGrpMem_myDb;
					pDbFuncs.pf_updateImGrpMem_recoverImGrpMem2 = updateImGrpMem_recoverImGrpMem2_myDb;
					//
					pDbFuncs.pf_bIdInfoExistedInImObjRegInfoTab = bIdInfoExistedInImObjRegInfoTab_myDb;
					pDbFuncs.pf_insertImObjRegInfo_qisRegUsrInfoInTab = insertImObjRegInfo_qisRegUsrInfoInTab_myDb;
					pDbFuncs.pf_updateImObjRegInfo_qisRegUsrInfoInTab = updateImObjRegInfo_qisRegUsrInfoInTab_myDb;
					/*
					pDbFuncs.pf_qnmRegUsrInfo = qnmRegUsrInfo_myDb;
					*/
					pDbFuncs.pf_qisRegUsrInfo = qisRegUsrInfo_myDb;
					/*
					pDbFuncs.pf_bSelectImMsg_logImMsg = bSelectImMsg_logImMsg_myDb;
					pDbFuncs.pf_insertImMsg_logImMsg = insertImMsg_logImMsg_myDb;
					pDbFuncs.pf_auditNewMessenger = auditNewMessenger_myDb;
					pDbFuncs.pf_bGetImMsgRcdBySth = bGetImMsgRcdBySth_myDb;
					pDbFuncs.pf_bLogEventFunc_is = bLogEventFunc_is_myDb;
					pDbFuncs.pf_bGetPcInfoByMac = bGetPcInfoByMac_myDb;
					pDbFuncs.pf_bGetPcInfoByMacEx = bGetPcInfoByMacEx_myDb;
					pDbFuncs.pf_getEventInfoBySth = getEventInfoBySth_myDb;
					pDbFuncs.pf_getPcEventInfoByMac0 = getPcEventInfoByMac0_myDb;
					pDbFuncs.pf_bLogPcEventFunc = bLogPcEventFunc_myDb;
					pDbFuncs.pf_qisRegOtherInfoInTab = qisRegOtherInfoInTab_myDb;
					pDbFuncs.pf_bClearupImMsg_isCli = bClearupImMsg_isCli_myDb;
					pDbFuncs.pf_bClearupEvent_is = bClearupEvent_is_myDb;
					pDbFuncs.pf_bClearupImMsg = bClearupImMsg_myDb;
					pDbFuncs.pf_bClearupImHisTask = bClearupImHisTask_myDb;
					pDbFuncs.pf_getObjIndex = getObjIndex_myDb;


					pDbFuncs.pf_bGetMessengerPhoneInfoBySth = bGetMessengerPhoneInfoBySth_myDb;
					pDbFuncs.pf_recoverMessengerPhoneInfo = recoverMessengerPhoneInfo_myDb;
					//
					*/
					pDbFuncs.pf_updateImObj_me = updateImObj_me_myDb;
					/*
					pDbFuncs.pf_bGetImObjRulesReq = bGetImObjRulesReq_myDb;
					pDbFuncs.pf_bGetImObjRuleRcdBySth = bGetImObjRuleRcdBySth_myDb;
					pDbFuncs.pf_insertImObjRule_recoverImObjRules = insertImObjRule_recoverImObjRules_myDb;
					pDbFuncs.pf_deleteImObjRule_recoverImObjRules = deleteImObjRule_recoverImObjRules_myDb;
					pDbFuncs.pf_updateImObjRule_recoverImObjRules = updateImObjRule_recoverImObjRules_myDb;

					pDbFuncs.pf_updateImObjRule_recoverImObjRules2 = updateImObjRule_recoverImObjRules2_myDb;
					//
					//  pDbFuncs.pf_mcProcResp_refreshImGrps  =  mcProcResp_refreshImGrps_myDb;
					//  pDbFuncs.pf_mcProcResp_refreshImGrps_end  =  mcProcResp_refreshImGrps_end_myDb;
					*/
					pDbFuncs.pf_updateImObj_refreshImGrps_end = updateImObj_refreshImGrps_end_myDb;					
					//
					pDbFuncs.pf_bNeedRetrieveImGrpMems = bNeedRetrieveImGrpMems_myDb;
					/*
					//  pDbFuncs.pf_mcProcResp_refreshImGrpMems  =  mcProcResp_refreshImGrpMems_myDb;
					//  pDbFuncs.pf_mcProcResp_refreshImGrpMems_end  =  mcProcResp_refreshImGrpMems_end_myDb;
					*/
					pDbFuncs.pf_updateImGrpMem_refreshImGrpMems_end = updateImGrpMem_refreshImGrpMems_end_myDb;

					//  pDbFuncs.pf_mcProcResp_refreshContacts  =  mcProcResp_refreshContacts_myDb;
					//  pDbFuncs.pf_mcProcResp_refreshContacts_end  =  mcProcResp_refreshContacts_end_myDb;
					pDbFuncs.pf_updateImObj_refreshContacts_end = updateImObj_refreshContacts_end_myDb;
					/*
					//
					pDbFuncs.pf_deleteOldImObjRule_mcProcResp_refreshImObjRulesReq = deleteOldImObjRule_mcProcResp_refreshImObjRulesReq_myDb;
					pDbFuncs.pf_selectNextImObjRule_mcProcResp_refreshImObjRulesReq = selectNextImObjRule_mcProcResp_refreshImObjRulesReq_myDb;
					pDbFuncs.pf_fillImObjs_msgr = fillImObjs_msgr_myDb;
					pDbFuncs.pf_fillImObjs_grp = fillImObjs_grp_myDb;
					
					pDbFuncs.pf_selectImTask_dlgTalk_loadTasks = selectImTask_dlgTalk_loadTasks_myDb;
					*/
					pDbFuncs.pf_bSelectImTask_storeTaskInDb = bSelectImTask_storeTaskInDb_myDb;
					pDbFuncs.pf_insertImTask_storeTaskInDb = insertImTask_storeTaskInDb_myDb;					
					pDbFuncs.pf_updateTaskStatus = updateTaskStatus_myDb;
					/*
					pDbFuncs.pf_bMoveTask2His = bMoveTask2His_myDb;
					pDbFuncs.pf_bClearupDeletedImObjs_isClient = bClearupDeletedImObjs_isClient_myDb;
					*/
					pDbFuncs.pf_bGetImTaskRcdBySth = bGetImTaskRcdBySth_myDb;
					//
					//pDbFuncs.pf_bSelectImGrpMem_dlgTalk_OnInitDialog  =  bSelectImGrpMem_dlgTalk_OnInitDialog_myDb;
					/*
					//
					pDbFuncs.pf_bGetQyDynBmpRcdBySth = bGetQyDynBmpRcdBySth_myDb;

					pDbFuncs.pf_listObjs_dlgMsgList = listObjs_dlgMsgList_myDb;
					pDbFuncs.pf_bViewImTaskList_unproced = bViewImTaskList_unproced_myDb;
					pDbFuncs.pf_bViewImTaskList_cur = bViewImTaskList_cur_myDb;
					pDbFuncs.pf_bViewImTaskList_his = bViewImTaskList_his_myDb;

					pDbFuncs.pf_bViewImObjList = bViewImObjList_myDb;
					pDbFuncs.pf_bViewRegInfoList = bViewRegInfoList_myDb;
					pDbFuncs.pf_bViewImGrpList = bViewImGrpList_myDb;
					pDbFuncs.pf_bViewImGrpMemList_isClient = bViewImGrpMemList_isClient_myDb;
					pDbFuncs.pf_bViewImObjRuleList_is = bViewImObjRuleList_is_myDb;
					//pDbFuncs.pf_bViewSharedDynBmpList_isClient  =  bViewSharedDynBmpList_isClient_myDb;

					pDbFuncs.pf_bInsertPhoneGuest_procMsgTalk_work = bInsertPhoneGuest_procMsgTalk_work_myDb;
					pDbFuncs.pf_bUpdateImTask_bChkQyMcEnv_mis = bUpdateImTask_bChkQyMcEnv_mis_myDb;
					pDbFuncs.pf_deleteOldDynBmp_mcProcResp_retrieveDynBmps = deleteOldDynBmp_mcProcResp_retrieveDynBmps_myDb;
					//  pDbFuncs.pf_deleteDynBmp_tmpHandler_delOld_dynBmpQ  = deleteDynBmp_tmpHandler_delOld_dynBmpQ_myDb;
					//pDbFuncs.pf_init_mcProcResp_retrieveDynBmps  =  init_mcProcResp_retrieveDynBmps_myDb;
					pDbFuncs.pf_bInsertDynBmp_mcProcResp_retrieveDynBmps = bInsertDynBmp_mcProcResp_retrieveDynBmps_myDb;
					pDbFuncs.pf_bUpdateDynBmp_mcProcResp_retrieveDynBmps = bUpdateDynBmp_mcProcResp_retrieveDynBmps_myDb;
					pDbFuncs.pf_bUpdateDynBmp_mcProcResp_retrieveDynBmps2 = bUpdateDynBmp_mcProcResp_retrieveDynBmps2_myDb;

					pDbFuncs.pf_bUpdateImObjRegInfo_procMsgInput_scheduler = bUpdateImObjRegInfo_procMsgInput_scheduler_myDb;
					pDbFuncs.pf_bStatImTasks = bStatImTasks_myDb;
					pDbFuncs.pf_bDeleteFromTabById = bDeleteFromTabById_myDb;
					pDbFuncs.pf_bDeleteFromTabByIdInfo = bDeleteFromTabByIdInfo_myDb;
					pDbFuncs.pf_bDeleteImOtherRegInfoByAddr = bDeleteImOtherRegInfoByAddr_myDb;
					pDbFuncs.pf_iSelectImGrpMem_dlgSelect_isCli = iSelectImGrpMem_dlgSelect_isCli_myDb;
					pDbFuncs.pf_iSelectImObj_dlgSelect_isCli = iSelectImObj_dlgSelect_isCli_myDb;
#if  false
						pDbFuncs.pf_iSelectImObjAuth2_dlgSelect_isCli  =  iSelectImObjAuth2_dlgSelect_isCli_myDb;
#endif
					pDbFuncs.pf_iSelectDynBmp_dlgSelect_isCli = iSelectDynBmp_dlgSelect_isCli_myDb;

#endif
					*/
					//
					int ii = 0;
					//
					break;
				default:
					/*
					pDbFuncs->pf_bGetImAmRcdBySth = bGetImAmRcdBySth_odbc;
					pDbFuncs->pf_bGetMessengerRegInfoBySth = bGetMessengerRegInfoBySth_odbc;
					pDbFuncs->pf_bGetImOtherRegInfoBySth = bGetImOtherRegInfoBySth_odbc;
					pDbFuncs->pf_bGetMeInfoBySth = bGetMeInfoBySth_odbc;
					pDbFuncs->pf_bGetMessengerInfoBySth = bGetMessengerInfoBySth_odbc;
					pDbFuncs->pf_insertImObj_recoverMessenger = insertImObj_recoverMessenger_odbc;
					pDbFuncs->pf_updateImObj_recoverMessenger = updateImObj_recoverMessenger_odbc;
					pDbFuncs->pf_updateImObj_recoverMessenger2 = updateImObj_recoverMessenger2_odbc;


					pDbFuncs->pf_insertImObj_recoverImGrp = insertImObj_recoverImGrp_odbc;
					pDbFuncs->pf_updateImObj_recoverImGrp = updateImObj_recoverImGrp_odbc;
					pDbFuncs->pf_updateImObj_recoverImGrp2 = updateImObj_recoverImGrp2_odbc;

					pDbFuncs->pf_bGetImGrpInfoBySth = bGetImGrpInfoBySth_odbc;
					pDbFuncs->pf_insertImGrp_recoverImGrp = insertImGrp_recoverImGrp_odbc;
					pDbFuncs->pf_updateImGrp_recoverImGrp = updateImGrp_recoverImGrp_odbc;
					pDbFuncs->pf_bGetImGrpMemBySth = bGetImGrpMemBySth_odbc;
					pDbFuncs->pf_insertImGrpMem_recoverImGrpMem = insertImGrpMem_recoverImGrpMem_odbc;
					pDbFuncs->pf_updateImGrpMem_recoverImGrpMem = updateImGrpMem_recoverImGrpMem_odbc;
					pDbFuncs->pf_updateImGrpMem_recoverImGrpMem2 = updateImGrpMem_recoverImGrpMem2_odbc;
					pDbFuncs->pf_bIdInfoExistedInImObjRegInfoTab = bIdInfoExistedInImObjRegInfoTab_odbc;
					pDbFuncs->pf_insertImObjRegInfo_qisRegUsrInfoInTab = insertImObjRegInfo_qisRegUsrInfoInTab_odbc;
					pDbFuncs->pf_updateImObjRegInfo_qisRegUsrInfoInTab = updateImObjRegInfo_qisRegUsrInfoInTab_odbc;

					pDbFuncs->pf_qnmRegUsrInfo = qnmRegUsrInfo_odbc;
					pDbFuncs->pf_qisRegUsrInfo = qisRegUsrInfo_odbc;
					pDbFuncs->pf_bSelectImMsg_logImMsg = bSelectImMsg_logImMsg_odbc;
					pDbFuncs->pf_insertImMsg_logImMsg = insertImMsg_logImMsg_odbc;
					pDbFuncs->pf_auditNewMessenger = auditNewMessenger_odbc;
					pDbFuncs->pf_bGetImMsgRcdBySth = bGetImMsgRcdBySth_odbc;
					pDbFuncs->pf_bLogEventFunc_is = bLogEventFunc_is_odbc;
					pDbFuncs->pf_bGetPcInfoByMac = bGetPcInfoByMac_odbc;
					pDbFuncs->pf_bGetPcInfoByMacEx = bGetPcInfoByMacEx_odbc;
					pDbFuncs->pf_getEventInfoBySth = getEventInfoBySth_odbc;
					pDbFuncs->pf_getPcEventInfoByMac0 = getPcEventInfoByMac0_odbc;
					pDbFuncs->pf_bLogPcEventFunc = bLogPcEventFunc_odbc;
					pDbFuncs->pf_qisRegOtherInfoInTab = qisRegOtherInfoInTab_odbc;
					pDbFuncs->pf_bClearupImMsg_isCli = bClearupImMsg_isCli_odbc;
					pDbFuncs->pf_bClearupEvent_is = bClearupEvent_is_odbc;
					pDbFuncs->pf_bClearupImMsg = bClearupImMsg_odbc;
					pDbFuncs->pf_bClearupImHisTask = bClearupImHisTask_odbc;
					pDbFuncs->pf_getObjIndex = getObjIndex_odbc;

					pDbFuncs->pf_bGetMessengerPhoneInfoBySth = bGetMessengerPhoneInfoBySth_odbc;
					pDbFuncs->pf_recoverMessengerPhoneInfo = recoverMessengerPhoneInfo_odbc;

					//
					pDbFuncs->pf_updateImObj_me = updateImObj_me_odbc;
					pDbFuncs->pf_bGetImObjRulesReq = bGetImObjRulesReq_odbc;
					pDbFuncs->pf_bGetImObjRuleRcdBySth = bGetImObjRuleRcdBySth_odbc;
					pDbFuncs->pf_insertImObjRule_recoverImObjRules = insertImObjRule_recoverImObjRules_odbc;
					pDbFuncs->pf_deleteImObjRule_recoverImObjRules = deleteImObjRule_recoverImObjRules_odbc;
					pDbFuncs->pf_updateImObjRule_recoverImObjRules = updateImObjRule_recoverImObjRules_odbc;
					pDbFuncs->pf_updateImObjRule_recoverImObjRules2 = updateImObjRule_recoverImObjRules2_odbc;

					//
					pDbFuncs->pf_updateImObj_refreshImGrps_end = updateImObj_refreshImGrps_end_odbc;
					//
					pDbFuncs->pf_bNeedRetrieveImGrpMems = bNeedRetrieveImGrpMems_odbc;
					//
					pDbFuncs->pf_updateImGrpMem_refreshImGrpMems_end = updateImGrpMem_refreshImGrpMems_end_odbc;
					//
					pDbFuncs->pf_updateImObj_refreshContacts_end = updateImObj_refreshContacts_end_odbc;

					//
					pDbFuncs->pf_deleteOldImObjRule_mcProcResp_refreshImObjRulesReq = deleteOldImObjRule_mcProcResp_refreshImObjRulesReq_odbc;
					pDbFuncs->pf_selectNextImObjRule_mcProcResp_refreshImObjRulesReq = selectNextImObjRule_mcProcResp_refreshImObjRulesReq_odbc;

					pDbFuncs->pf_fillImObjs_msgr = fillImObjs_msgr_odbc;
					pDbFuncs->pf_fillImObjs_grp = fillImObjs_grp_odbc;
					pDbFuncs->pf_selectImTask_dlgTalk_loadTasks = selectImTask_dlgTalk_loadTasks_odbc;
					pDbFuncs->pf_bSelectImTask_storeTaskInDb = bSelectImTask_storeTaskInDb_odbc;
					pDbFuncs->pf_insertImTask_storeTaskInDb = insertImTask_storeTaskInDb_odbc;
					pDbFuncs->pf_updateTaskStatus = updateTaskStatus_odbc;
					pDbFuncs->pf_bMoveTask2His = bMoveTask2His_odbc;
					pDbFuncs->pf_bClearupDeletedImObjs_isClient = bClearupDeletedImObjs_isClient_odbc;
					pDbFuncs->pf_bGetImTaskRcdBySth = bGetImTaskRcdBySth_odbc;
					//
					//pDbFuncs->pf_bSelectImGrpMem_dlgTalk_OnInitDialog  =  bSelectImGrpMem_dlgTalk_OnInitDialog_odbc;
					//
					pDbFuncs->pf_bGetQyDynBmpRcdBySth = bGetQyDynBmpRcdBySth_odbc;

					pDbFuncs->pf_listObjs_dlgMsgList = listObjs_dlgMsgList_odbc;
					pDbFuncs->pf_bViewImTaskList_unproced = bViewImTaskList_unproced_odbc;
					pDbFuncs->pf_bViewImTaskList_cur = bViewImTaskList_cur_odbc;
					pDbFuncs->pf_bViewImTaskList_his = bViewImTaskList_his_odbc;

					pDbFuncs->pf_bViewImObjList = bViewImObjList_odbc;
					pDbFuncs->pf_bViewRegInfoList = bViewRegInfoList_odbc;
					pDbFuncs->pf_bViewImGrpList = bViewImGrpList_odbc;
					pDbFuncs->pf_bViewImGrpMemList_isClient = bViewImGrpMemList_isClient_odbc;
					pDbFuncs->pf_bViewImObjRuleList_is = bViewImObjRuleList_is_odbc;
					//pDbFuncs->pf_bViewSharedDynBmpList_isClient  =  bViewSharedDynBmpList_isClient_odbc;

					pDbFuncs->pf_bInsertPhoneGuest_procMsgTalk_work = bInsertPhoneGuest_procMsgTalk_work_odbc;
					pDbFuncs->pf_bUpdateImTask_bChkQyMcEnv_mis = bUpdateImTask_bChkQyMcEnv_mis_odbc;

					pDbFuncs->pf_deleteOldDynBmp_mcProcResp_retrieveDynBmps = deleteOldDynBmp_mcProcResp_retrieveDynBmps_odbc;
					//  pDbFuncs->pf_deleteDynBmp_tmpHandler_delOld_dynBmpQ  =  deleteDynBmp_tmpHandler_delOld_dynBmpQ_odbc;
					//pDbFuncs->pf_init_mcProcResp_retrieveDynBmps  =  init_mcProcResp_retrieveDynBmps_odbc;
					pDbFuncs->pf_bInsertDynBmp_mcProcResp_retrieveDynBmps = bInsertDynBmp_mcProcResp_retrieveDynBmps_odbc;
					pDbFuncs->pf_bUpdateDynBmp_mcProcResp_retrieveDynBmps = bUpdateDynBmp_mcProcResp_retrieveDynBmps_odbc;
					pDbFuncs->pf_bUpdateDynBmp_mcProcResp_retrieveDynBmps2 = bUpdateDynBmp_mcProcResp_retrieveDynBmps2_odbc;

					pDbFuncs->pf_bUpdateImObjRegInfo_procMsgInput_scheduler = bUpdateImObjRegInfo_procMsgInput_scheduler_odbc;
					pDbFuncs->pf_bStatImTasks = bStatImTasks_odbc;
					pDbFuncs->pf_bDeleteFromTabById = bDeleteFromTabById_odbc;
					pDbFuncs->pf_bDeleteFromTabByIdInfo = bDeleteFromTabByIdInfo_odbc;
					pDbFuncs->pf_bDeleteImOtherRegInfoByAddr = bDeleteImOtherRegInfoByAddr_odbc;
					pDbFuncs->pf_iSelectImGrpMem_dlgSelect_isCli = iSelectImGrpMem_dlgSelect_isCli_odbc;
					pDbFuncs->pf_iSelectImObj_dlgSelect_isCli = iSelectImObj_dlgSelect_isCli_odbc;
#if  0
					pDbFuncs->pf_iSelectImObjAuth2_dlgSelect_isCli  =  iSelectImObjAuth2_dlgSelect_isCli_odbc;
#endif
					pDbFuncs->pf_iSelectDynBmp_dlgSelect_isCli = iSelectDynBmp_dlgSelect_isCli_odbc;
					*/

					break;
			}
			

			iErr = 0;
		errLabel:
			return iErr;
		}


	}
}
