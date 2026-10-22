
//#define __PRINT_imObj__

using System;
using System.Collections.Generic;
using System.Text;


namespace qy
{
    partial class qyFuncs
{

		public static unsafe int printRetrieveImGrpListResp(RETRIEVE_IMGRPLIST_RESP* pResp)
		{
#if DEBUG

#if __PRINT_imObj__
			int i;
			string str;
			str=string.Format("printRetrieveImGrpListResp: enters, usCnt is {0}", pResp->usCnt);

			qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), str);
				
			for (i = 0; i < pResp->usCnt; i++)
			{
				IM_GRP_INFO* pMems = (IM_GRP_INFO*)pResp->buf_imGrpInfo_mems;
				str=string.Format("grp ({0})", pMems[i].idInfo.ui64Id);
				qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), str);
					
			}
			qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), ("printRetrieveImGrpListResp:leaves"));
#endif

#endif

			return 0;
		}


		public static unsafe int printRetrieveImGrpMemListResp(RETRIEVE_IMGRPMEMLIST_RESP* pResp)
		{

# if  DEBUG
# if  __PRINT_imObj__
			int i;
			string str;

			str=string.Format("printRetrieveImGrpMemListResp: enters, usCnt is {0}", pResp->usCnt);

			qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), str);
				
			for (i = 0; i < pResp->usCnt; i++)
			{
				IM_GRP_MEM* pMems = (IM_GRP_MEM*)pResp->buf_imGrpMem_mems;
				str = string.Format("grp ( {0} ), mem ( {1} )", pMems[i].idInfo_grp.ui64Id, pMems[i].idInfo_mem.ui64Id);

				qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), str);
					}

			qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), ("printRetrieveImGrpMemListResp: leaves"));
#endif
#endif


			return 0;
		}

		public static unsafe int printRetrieveContactListResp(RETRIEVE_CONTACTLIST_RESP* p)
		{
# if  DEBUG
# if  __PRINT_imObj__
			int i;
			//TCHAR tBuf[1024] = ("");
			//char idStr[128] = "";
			//char lastModifiedTime_regInfo[128] = "";
			string str;
			str=string.Format("printRetrieveContactListResp: enters, usCnt is {0}", p->usCnt);
			qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), str); 
			for (i = 0; i < p->usCnt; i++)
			{
				RETRIEVE_contact_mem* pMems = (RETRIEVE_contact_mem*)p->buf_retrieveContactMem_mems;

				//tBuf[0] = 0;
				//idInfo2Str(&p->mems[i].addr.idInfo, idStr, mycountof(idStr));
				//_sntprintf(tBuf, mycountof(tBuf), ("第%d条, %S: %S"), i, idStr, lastModifiedTime_regInfo);

				string dw = new string(pMems[i].regInfo.dw);
				string bm = new string(pMems[i].regInfo.bm);
				string syr = new string(pMems[i].regInfo.syr);
				//
				str=string.Format("第{0}条, {1}: {2} {3} {4}", i, pMems[i].regInfo.addr.idInfo.ui64Id, dw,bm,syr);
				qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), str);
			}
			qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), ("isClient"), null, (""), (""), ("printRetrieveContactListResp: leaves"));
#endif

#endif
			return 0;
		}

		//
		public static unsafe int printMisMsg(void* bNetworkDataParam, void* p1, ref MIS_MSGU pMsg)
		{
			/*
			BOOL bNetworkData = (BOOL)bNetworkDataParam;
			TCHAR tBuf[2048] = _T("");
			IM_CONTENTU* pContent = NULL;
			char timeBuf[CONST_qyTimeLen + 1] = "";

# ifdef  __DEBUG__

			_sntprintf(tBuf, mycountof(tBuf), _T("\t%s"), qyGetDesByType1(CONST_misMsgTypeTable, pMsg->uiType));
			OutputDebugString(tBuf);
			switch (pMsg->uiType)
			{
				case CONST_misMsgType_input:
					_sntprintf(tBuf, mycountof(tBuf), _T("%s | %s"), tBuf, isUcFlgResp(pMsg->input.ucFlg) ? _T("Resp") : _T("Req"));
					getTimeStr(pMsg->input.tStartTime, timeBuf, mycountof(timeBuf));
					_sntprintf(tBuf, mycountof(tBuf), _T("%s,  tStartTime  is %S, tranNo is %d"), tBuf, timeBuf, pMsg->input.uiTranNo);
					pContent = M_getMsgContent(pMsg->input.ucFlg, &pMsg->input.data);
					if (!pContent) break;
					if (bNetworkData)
					{
# ifndef  __WINCE__
						printImMsgContent_n(NULL, NULL, pContent, pMsg->input.lenInBytes, tBuf, mycountof(tBuf));
#endif
					}
					else
						snprintContent(0, pContent, tBuf, mycountof(tBuf));
					break;
				case CONST_misMsgType_req:
					_sntprintf(tBuf, mycountof(tBuf), _T("%s | %s"), tBuf, qyGetDesByType1(CONST_qyCmdTable, pMsg->req.usCode));
					break;
				case CONST_misMsgType_recentFriend_qmc:
					_sntprintf(tBuf, mycountof(tBuf), _T("%s, messengerId is %I64u, tickCnt is %u"), tBuf, pMsg->recentFriend_qmc.idInfo.ui64Id, pMsg->recentFriend_qmc.dwTickCnt_op);
					break;
				case CONST_misMsgType_talk:
					_sntprintf(tBuf, mycountof(tBuf), _T("%s, uiTranNo_processed is %d,  uiSessionId is %d"), tBuf, pMsg->talk.uiTranNo_processed, pMsg->talk.uiSessionId);
					if (!isUcFlgResp(pMsg->talk.ucFlg)) _sntprintf(tBuf, mycountof(tBuf), _T("%s, cmd is  %s"), tBuf, qyGetDesByType1(CONST_qyCmdTable, pMsg->talk.usCode));
					else _sntprintf(tBuf, mycountof(tBuf), _T("%s, cmd_org is  %s"), tBuf, qyGetDesByType1(CONST_qyCmdTable, pMsg->talk.usCode_org));
					snprintContent(&pMsg->talk.data, 0, tBuf, mycountof(tBuf));
					break;
				case CONST_misMsgType_task:
				case CONST_misMsgType_outputTask:
					getTimeStr(pMsg->task.tStartTime, timeBuf, mycountof(timeBuf));
					_sntprintf(tBuf, mycountof(tBuf), _T("%s, taskId %d, %s,  st %S, tn %d, sender %I64u, receiver %I64u"), tBuf, pMsg->task.iTaskId, qyGetDesByType1(CONST_qyStatusTable_en, pMsg->task.iStatus), timeBuf, pMsg->task.uiTranNo, pMsg->task.idInfo_taskSender.ui64Id, pMsg->task.idInfo_taskReceiver.ui64Id);
					snprintContent(&pMsg->task.data, 0, tBuf, mycountof(tBuf));
					break;
				case CONST_misMsgType_grp:
					int i;
					MIS_MSG_grpMemData* pMem;

					//  if  (  pMsg->grp.usCnt >=  2  )  {
					//		traceLogA(  "hehe"  );
					// }					

					for (i = 0; i < pMsg->grp.usCnt; i++)
					{
						pMem = (MIS_MSG_grpMemData*)(pMsg->grp.buf + pMsg->grp.memDataHeads[i].uiPos);
						getTimelStr(pMem->tStartTime, timeBuf, mycountof(timeBuf));
						_sntprintf(tBuf, mycountof(tBuf), _T("第%d个, tStartTime is %S, tranNo is %d. "), i, timeBuf, pMem->uiTranNo);
						OutputDebugString(tBuf);
						if (bNetworkData)
						{
# ifndef  __WINCE__
							printImMsgContent_n(NULL, NULL, (IM_CONTENTU*)pMem->buf, pMem->lenInBytes, tBuf, mycountof(tBuf));
#endif
						}
						else
							snprintContent(NULL, (IM_CONTENTU*)pMem->buf, tBuf, mycountof(tBuf));
						//
						OutputDebugString(_T("  ")); OutputDebugString(tBuf); OutputDebugString(_T("\n"));
						tBuf[0] = 0;
					}
					break;
				default:
					break;

			}
			OutputDebugString(tBuf); OutputDebugString(_T("\n"));

#endif
			*/

			return 0;
		}


	}
}
