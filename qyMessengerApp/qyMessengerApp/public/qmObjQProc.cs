using System;
using System.Collections.Generic;
using System.Text;


partial class Consts
{
    public const int CONST_objQType_null = 0;
    public const int CONST_objQType_netMc = 1;
    public const int CONST_objQType_mis = 2;
    public const int CONST_objQType_misClient = 3;						//  对在messenger的管理端和客户端由于使用的不同，存储方式也不同。故用不同的类型来区分

}


namespace qy
{
 partial class qyFuncs
    {
		public static unsafe int tmpHandler_procMisMsg_moreData(object p0, object p1, object pMsgElemParam)
		{
			int iErr = -1;
			bool bFindOnly = (IntPtr)p0!=IntPtr.Zero ? true :false;
			MIS_MSG_INPUT* pMsgInput = (MIS_MSG_INPUT*)(IntPtr)p1;
			MIS_MSG_INPUT* pMsgElem = (MIS_MSG_INPUT*)(IntPtr)pMsgElemParam;
			//

			if (pMsgInput->tStartTime == pMsgElem->tStartTime
				&& pMsgInput->uiTranNo == pMsgElem->uiTranNo)
			{
				//  found

				if (bFindOnly)
				{           //  这里用来寻找某类包。可以用来删除包等
					iErr = 1; goto errLabel;
				}

				if (pMsgInput->usSeqNo != pMsgElem->usSeqNo + 1)
				{   //  
					//traceLogA("tmpHandler_procMisMsg_moreData: usSeqNo error");
					goto errLabel;
				}
				if (pMsgInput->lenInBytes + pMsgElem->lenInBytes > sizeof(MIS_MSG_routeTalkData))
				{
					string str =string.Format(("procMoreData failed: 存储空间不够，数据太长了， tranNo [%d], seqNo [%d]"), pMsgInput->uiTranNo, (int)pMsgInput->usSeqNo);
					qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ("127.0.0.1"), null, "", str);
						
					//traceLogA("tmpHandler_procMisMsg_moreData: failed"); 
					goto errLabel;
				}
				mymemcpy((IntPtr)(((byte*)&pMsgElem->data) + pMsgElem->lenInBytes), (IntPtr)(byte*)&pMsgInput->data, pMsgInput->lenInBytes);
				pMsgElem->lenInBytes += pMsgInput->lenInBytes;
				pMsgElem->usSeqNo = pMsgInput->usSeqNo; //  

				//  traceLogA(  "uiSeqNo [%d] ",  pMsgInput->uiSeqNo  );

				if (!isUcFlgMoreData(pMsgInput->ucFlg))
				{   //  
					mymemcpy((IntPtr)(byte*)&pMsgInput->data, (IntPtr)(byte*)&pMsgElem->data, pMsgElem->lenInBytes);
					pMsgInput->lenInBytes = pMsgElem->lenInBytes;
					pMsgInput->usSeqNo = 0;

					//  traceLogA(  "tmpHandler_procMisMsg_moreData;  "  );
				}

				iErr = 1; goto errLabel;
			}

			iErr = 0;
		errLabel:
			return iErr;
		}

		public static unsafe bool b_tmpHandler_procMisMsg_moreData(object p0, object p1, object pMsgElemParam)
        {
			int iRet = tmpHandler_procMisMsg_moreData(p0, p1, pMsgElemParam);
			return iRet != 0 ? true : false;
        }

		//  当ucFlg的CONST_commFlg_moreData为0时，表明包发完了
		//  当不为0时，放在cacheQ里
		//  当为0时，如uiSeqNo为0，则是独立包。如不为0，则从cacheQ里取出所有的tStartTime和tranNo相同的包，按uiSeqNo组成一个完整包。
		// 
		//  这里的包，限制在一个MIS_MSG_TALK的buf里。
		//  对更大的包，也许要用队列或其他来表示了。暂不考虑. 或者，应该在客户端自己在内容数据里来分拆合并了
		public static  unsafe int procMisMsg_moreData(GENERIC_Q pCacheQ, MIS_MSG_INPUT* pMsgInput, bool* pbMsgSkipped)
		{
			int iErr = -1;
			bool bMsgSkipped = false;

			int tmpiRet;

			//  当Q满时，简单的处理是将队列清空
			if (isQFull(pCacheQ)) emptyGenericQ(pCacheQ);

			if (isUcFlgMoreData(pMsgInput->ucFlg))
			{       //  找到前面的包加进去

				tmpiRet = qTraverse(pCacheQ, tmpHandler_procMisMsg_moreData, IntPtr.Zero, (IntPtr)pMsgInput);
				if (tmpiRet < 0)
				{
					qRemoveMsg(pCacheQ, b_tmpHandler_procMisMsg_moreData, (IntPtr)1, (IntPtr)pMsgInput);
				}
				if (0==tmpiRet)
				{  //  没找到就要加进去. 注意，在这里要将整个MIS_MSG_INPUT都放进去，是因为需要将后面的数据都跟在msg的data里
					if (0!=qPostMsg((IntPtr)pMsgInput, (uint)sizeof(MIS_MSG_INPUT), pCacheQ)) goto errLabel;
				}
				iErr = 0; goto errLabel;
			}

			if (0==pMsgInput->usSeqNo)
			{   //  这是指不需要在这里处理的
				iErr = 0; goto errLabel;
			}

			//  要准备把数据取出来，放到pMsgInput中

			tmpiRet = qTraverse(pCacheQ, tmpHandler_procMisMsg_moreData, (IntPtr)0, (IntPtr)pMsgInput);
			if (tmpiRet!=0) qRemoveMsg(pCacheQ, b_tmpHandler_procMisMsg_moreData, (IntPtr)1, (IntPtr)pMsgInput);
			if (tmpiRet <= 0)
			{
				bMsgSkipped = true;
				iErr = 0; goto errLabel;
			}

			iErr = 0;

		errLabel:

			if (0==iErr)
			{
				if (pbMsgSkipped!=null) *pbMsgSkipped = bMsgSkipped;
			}

			if (iErr!=0)
			{   //  2011/01/24
				traceLog(("procMisMsg_moreData failed"));
			}

			return iErr;
		}



	}
}
