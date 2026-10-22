using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


partial class Consts
{
    public const string CONST_filterChars = "'";

    public const int CONST_cntof_utf8Str = 32 * 1024;       //  2010/12/05
    public const int CONST_cntof_wStr = 32 * 1024;       //  2010/12/05

    //
    public const int bufSize_vh = 4096;
    public const int bufSize_ah = 4096;

    //
    public const int cntof_ctx_uiStreamIds = 5;


}


namespace qy
{

    //
    public unsafe struct CTX_stream2Data
    {
        //
        public fixed uint uiStreamIds[Consts.cntof_ctx_uiStreamIds];
        //
        public int index_streamId;
    }
    ;


    public unsafe delegate int PF_handler_stream2Data(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem);	//  

    partial class qyFuncs
    {

        public static unsafe bool bMessengerIdValid(QY_MESSENGER_ID* pIdInfo)
        {
            //  if  (  pIdInfo  &&  pIdInfo->ui64Id  &&  pIdInfo->ui64Id  !=  CONST_invalidMessengerId  )  return  true;
            ulong invalid = unchecked((ulong)Consts.CONST_invalidMessengerId);
            if (pIdInfo != null && pIdInfo->ui64Id != invalid) return true;
            return false;
        }


        //
        public static bool bSupported_sendVhDec()
        {
#if __DEBUG__
			static bool bWarned = FALSE;
			if (!bWarned)
			{
				bWarned = TRUE;
				traceLog(_T("Note: VhDec should not be sent"));
			}

#endif

            return true;
        }


        //  对orgData是WCHAR的情况，orgDataSize表示WCHAR的count
        //  对这个流要求用CONST_qnmCfgId_null结尾
        //  *pptr指向下一个空间，而不是输出串
        //  *pSize为剩余的字节数，不是一共得到了多少
        public static unsafe int data2Stream(ushort iDataType, ushort usCfgId, void* orgData, uint orgDataSize, byte** pptr_usable, uint* pSize_usable)
        {
            int iErr = -1;

            QY_CFGITEM_STR itemStr;
            QY_CFGITEM_L32 itemLong;
            QY_CFGITEM_BYTE itemChar;
            QY_CFGITEM_SHORT itemShort;
            QY_CFGITEM_L64 itemL64;
            QY_CFGITEM* pItem = null;
            uint len;
            uint size = 0;
            byte* ptr = null;


            using (CQyMalloc mallocObj = new CQyMalloc())
            {

                if (null == pptr_usable || null == *pptr_usable || null == pSize_usable) return -1;
                if (null == orgData)
                {
                    switch (iDataType)
                    {
                        case Consts.CONST_qyDataType_l32:
                        case Consts.CONST_qyDataType_char:
                        case Consts.CONST_qyDataType_short:
                            break;
                        default:
                            return -1;  //  非数值的值为地址，不能为空
                    }
                }
                if (iDataType == Consts.CONST_qyDataType_l64)
                {
                    if (orgDataSize != 0 && orgDataSize < 8)
                    {       //  传入0也算了，早期代码都用0。2007/12/11
                            //traceLogA("data2Stream: l64需要传入数据地址，故建议输入数据长度应>=sizeof(  __int64  )");
                    }
                }

                ptr = *pptr_usable;
                size = *pSize_usable;

                switch (iDataType)
                {
                    case Consts.CONST_qyDataType_l32:
                        itemLong.cfgId = usCfgId;
                        itemLong.dataType = iDataType;
                        itemLong.ulVal = (uint)orgData;
                        pItem = (QY_CFGITEM*)&itemLong;
                        break;
                    case Consts.CONST_qyDataType_char:
                        itemChar.cfgId = usCfgId;
                        itemChar.dataType = iDataType;
                        itemChar.ucVal = (byte)orgData;
                        pItem = (QY_CFGITEM*)&itemChar;
                        break;
                    case Consts.CONST_qyDataType_short:
                        itemShort.cfgId = usCfgId;
                        itemShort.dataType = iDataType;
                        itemShort.usVal = (ushort)orgData;
                        pItem = (QY_CFGITEM*)&itemShort;
                        break;

                    case Consts.CONST_qyDataType_data:
                        {
                            QY_CFGITEM_DATA* pItemData = null;
                            //int tmpSize = offsetof(QY_CFGITEM_DATA, buf) + orgDataSize; //  sizeof(  QY_CFGITEM_DATA  )	//  2010/09/10
                            int tmpSize = (int)(6 + orgDataSize); //  sizeof(  QY_CFGITEM_DATA  )	//  2010/09/10
                                                                  //
                            if (null == (pItemData = (QY_CFGITEM_DATA*)mallocObj.mallocf(tmpSize)))
                            {
                                //traceLogA("data2Stream. malloc from QY_CFGITEM_DATA failed."); 
                                goto errLabel;
                            }
                            //
                            pItemData->cfgId = usCfgId;
                            pItemData->dataType = iDataType;
                            //if (orgDataSize > sizeof(pItemData->buf)  )  
                            if (orgDataSize > Consts.bufSize_cfgItemData)
                            {
                                //traceLogA("data2Stream: dataType_data: too big dataSize"); 
                                goto errLabel;
                            }
                            pItemData->len = (ushort)orgDataSize;
                            if (pItemData->len != orgDataSize)
                            {       //  2008/04/01, 
                                    //traceLogA("data2Stream: dataType_data error, buf truncated. data只能装不超过一个short长度的数据。");
                                goto errLabel;
                            }
                            mymemcpy((IntPtr)pItemData->buf, (IntPtr)orgData, pItemData->len);
                            pItem = (QY_CFGITEM*)pItemData;
                        }
                        break;
                    case Consts.CONST_qyDataType_lData:
                        {

                            QY_CFGITEM_LDATA* pItemLData = null;
                            //int tmpSize = offsetof(QY_CFGITEM_LDATA, buf) + orgDataSize;    //  sizeof(  QY_CFGITEM_LDATA  )	//  2010/09/10
                            //int tmpSize = (int)(8 + orgDataSize);    //  sizeof(  QY_CFGITEM_LDATA  )	//  2010/09/10
                            int tmpSize = sizeof(QY_CFGITEM_LDATA);
                            //
                            if (null == (pItemLData = (QY_CFGITEM_LDATA*)mallocObj.mallocf(tmpSize)))
                            {
                                //traceLogA("data2Stream. malloc from QY_CFGITEM_LDATA failed."); 
                                goto errLabel;
                            }
                            //

                            pItemLData->cfgId = usCfgId;

                            pItemLData->dataType = iDataType;

                            //if (orgDataSize > sizeof(pItemLData->buf)  )  
                            if (orgDataSize > Consts.bufSize_cfgItemLData)
                            {
                                //traceLogA("data2Stream: dataType_lData: too big dataSize"); 
                                goto errLabel;
                            }


                            pItemLData->ulLen = orgDataSize;

                            //mymemcpy((IntPtr)pItemLData->buf, (IntPtr)orgData, pItemLData->ulLen);
                            pItemLData->p = (byte*)orgData;

                            //
                            pItem = (QY_CFGITEM*)pItemLData;


                        }
                        break;


                    case Consts.CONST_qyDataType_l64:
                        itemL64.cfgId = usCfgId;
                        itemL64.dataType = iDataType;
                        //mymemcpy(itemL64.buf_l64Val, orgData, sizeof(__int64));
                        mymemcpy((IntPtr)itemL64.buf_l64Val, (IntPtr)orgData, 8);
                        pItem = (QY_CFGITEM*)&itemL64;
                        break;
                    case Consts.CONST_qyDataType_str:
                    case Consts.CONST_qyDataType_utf8Str:
                        {
                            //
                            itemStr.cfgId = usCfgId;
                            itemStr.dataType = iDataType;
                            //itemStr.len = min(sizeof(itemStr.str), orgDataSize);
                            itemStr.len = (ushort)Math.Min(Consts.CONST_cntof_cfgItemStr, orgDataSize);
                            mymemcpy((IntPtr)itemStr.str, (IntPtr)orgData, itemStr.len);
                            pItem = (QY_CFGITEM*)&itemStr;
                        }
                        break;
                    case Consts.CONST_qyDataType_wStr:
                        {
                            char* wStr = (char*)orgData;
                            wStr[orgDataSize] = (char)0;
                            string str_wStr = new string(wStr);
                            //
                            if (orgDataSize < 1024)
                            {
                                itemStr.cfgId = usCfgId;
                                if (null == myWChar2Utf8(str_wStr, itemStr.str, Consts.CONST_cntof_cfgItemStr)) goto errLabel;
                                itemStr.dataType = Consts.CONST_qyDataType_utf8Str;
                                itemStr.len = (ushort)mystrlen(itemStr.str);
                                pItem = (QY_CFGITEM*)&itemStr;
                            }
                            else
                            {  //2010/12/05
                                QY_CFGITEM_DATA* pItemData = null;
                                int tmpBufSize = (int)(orgDataSize + 1) * 2;
                                //int tmpSize = offsetof(QY_CFGITEM_DATA, buf) + tmpBufSize;
                                int tmpSize = 6 + tmpBufSize;
                                ////
                                //if (tmpBufSize > sizeof(pItemData->buf)  )  
                                if (tmpBufSize > Consts.bufSize_cfgItemData)
                                {
                                    //traceLogA("data2Stream: dataType_wStr: too big dataSize");
                                    goto errLabel;
                                }
                                //
                                if (null == (pItemData = (QY_CFGITEM_DATA*)mallocObj.mallocf(tmpSize)))
                                {
                                    //traceLogA("data2Stream. malloc from QY_CFGITEM_DATA failed.");
                                    goto errLabel;
                                }
                                //
                                pItemData->cfgId = usCfgId;
                                if (null == myWChar2Utf8(str_wStr, pItemData->buf, (uint)tmpBufSize)) goto errLabel;
                                pItemData->dataType = Consts.CONST_qyDataType_utf8Str;
                                pItemData->len = (ushort)mystrlen(pItemData->buf);
                                pItem = (QY_CFGITEM*)pItemData;
                            }
                        }
                        break;


                    default:
                        goto errLabel;

                }



                if (null == pItem) goto errLabel;
                //
                uint uiPart0Len = 0;
                byte* pPart1 = null;
                uint uiPart1Len = 0;
                len = (uint)qyhtonCfgItem(pItem, &uiPart0Len, &pPart1, &uiPart1Len);
                if (len <= 0 || size < len)
                {
                    //traceLogA("getStream_wStr  error."); 
                    goto errLabel;
                }

                //mymemcpy((IntPtr)ptr, (IntPtr)pItem, len); 
                mymemcpy((IntPtr)ptr, (IntPtr)pItem, uiPart0Len);
                if (pPart1 != null && uiPart1Len != 0)
                {
                    mymemcpy((IntPtr)(ptr + uiPart0Len), (IntPtr)pPart1, uiPart1Len);
                }
                //				
                ptr += len; size -= len;



            }


            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *pptr_usable = ptr;
                *pSize_usable = size;
            }


            return iErr;
        }



        //

        public static unsafe int stream2Data(byte** pCfgsBuf_usable, uint* pSize_usable, PF_handler_stream2Data pf_filter, PF_handler_stream2Data pf, void* p0, void* p1)
        {
            int iErr = -1;
            uint len;
            byte* ptr = null;
            QY_CFGITEM_ntoh_U item_ntoh_U;
            QY_CFGITEM_ntoh_U* pItem = null;
            //  QY_CFGITEM_WSTR			itemWStr;
            //CQyString wStr;

            //
            CTX_stream2Data ctx;
            mymemset((IntPtr)(byte*)&ctx, 0, sizeof(CTX_stream2Data));

            //
            int nStarts = 0;
            //uint uiStreamIds[5] = { 0, };
            //uint []uiStreamIds = { 0,0,0,0,0 };
            int tmpiRet;
            byte* cfgsBuf = null;
            uint size = 0;
            ushort tmp_cfgId;          //  2010/04/08
            ushort tmp_dataType;       //  2010/04/08

            if (null == pCfgsBuf_usable || null == pSize_usable) return -1;

            cfgsBuf = *pCfgsBuf_usable;
            size = *pSize_usable;
            //
            if (null == cfgsBuf || 0 == size || null == pf_filter || null == pf) return -1;

            //
            using (CQyMalloc mallocObj_wStr = new CQyMalloc())
            {
                int cntof_wStr = Consts.CONST_cntof_wStr;
                char* wStr = (char*)mallocObj_wStr.mallocf(cntof_wStr * sizeof(char));
                if (wStr == null) goto errLabel;
                //
                len = 0;
                ptr = cfgsBuf;
                for (; size != 0;)
                {
                    len = (uint)qyntohCfgItem(ptr, &item_ntoh_U);     //  这里要注意：这里qyntohCfgItem(  )返回的是转换前的字节流长度。和qyhtonCfgItem(  )返回的不同
                    pItem = &item_ntoh_U;

                    if (len <= 0 || len > size) goto errLabel;

                    //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));          //  2010/04/08
                    tmp_cfgId = pItem->head.cfgId;
                    //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));        //  2010/04/08
                    tmp_dataType = pItem->head.dataType;

                    ptr += len; size -= len;

                    if (0 == nStarts)
                    {
                        if (tmp_cfgId != Consts.CONST_qnmCfgId_start) goto errLabel;
                        if (tmp_dataType != Consts.CONST_qyDataType_long) goto errLabel;
                    }

                    if (tmp_cfgId == Consts.CONST_qnmCfgId_start)
                    {

                        nStarts++;
                        if (nStarts > Consts.cntof_ctx_uiStreamIds) goto errLabel;

                        //  uiStreamId  =  (  (  QY_CFGITEM_LONG  *  )pItem  )->ulVal;
                        //memcpy(&uiStreamIds[nStarts - 1], &((QY_CFGITEM_LONG*)pItem)->ulVal, sizeof(uiStreamIds[0]));
                        ctx.uiStreamIds[nStarts - 1] = ((QY_CFGITEM_L32*)pItem)->ulVal;
                        //
                        ctx.index_streamId = nStarts - 1;
                        if (0 != (tmpiRet = pf(&ctx, p0, p1, ctx.uiStreamIds[nStarts - 1], pItem)))
                        {       //  这里有利于处理结构起始时初始化，2007/08/01
                            iErr = tmpiRet; goto errLabel;
                        }
                        continue;
                    }

                    if (tmp_cfgId == Consts.CONST_qnmCfgId_null)
                    {
                        //
                        ctx.index_streamId = nStarts - 1;
                        if (0 != (tmpiRet = pf(&ctx, p0, p1, ctx.uiStreamIds[nStarts - 1], pItem)))
                        {       //  这里有利于处理结构结束时做结尾工作，2007/08/01
                            iErr = tmpiRet; goto errLabel;
                        }
                        nStarts--;
                        if (0 == nStarts) break;
                        continue;
                    }

                    if (tmp_dataType == Consts.CONST_qyDataType_utf8Str)
                    {

                        //  #define		bufSize_utf8Str		4096

                        using (CQyMalloc mallocObj_buf = new CQyMalloc())
                        {
                            int bufSize = Consts.CONST_cntof_utf8Str;
                            byte* pBuf = null;

                            if (pItem->data.len + 1 > Consts.CONST_cntof_utf8Str)
                            {
                                //traceLog(_T("stream2Data: utf8 too long string"));
                                goto errLabel;
                            }
                            bufSize = pItem->data.len + 1;
                            pBuf = (byte*)mallocObj_buf.mallocf(bufSize);
                            if (null == pBuf) goto errLabel;

                            //
                            safeStrnCpy(pItem->data.p, pBuf, (uint)Math.Min(pItem->data.len + 1, bufSize));
                            //
                            //wStr = CQyString(pBuf);
                            myUtf82WChar(pBuf, wStr, (uint)cntof_wStr);
                            pItem->wStr.dataType = Consts.CONST_qyDataType_wStr;
                            pItem->wStr.len = (ushort)mywcslen(wStr);
                            pItem->wStr.pWStr = wStr;
                            //
                        }
                    }
                    ctx.index_streamId = nStarts - 1;
                    if (0 != (tmpiRet = pf_filter(&ctx, p0, p1, ctx.uiStreamIds[nStarts - 1], pItem)))
                    {       //  2008/12/03, 主要是对pItem里的数据进行过滤。不是对结果的
                        iErr = tmpiRet; goto errLabel;
                    }
                    ctx.index_streamId = nStarts - 1;
                    if (0 != (tmpiRet = pf(&ctx, p0, p1, ctx.uiStreamIds[nStarts - 1], pItem)))
                    {
                        iErr = tmpiRet; goto errLabel;
                    }
                }

            }

            iErr = 0;
        errLabel:

            if (0 == iErr)
            {
                *pCfgsBuf_usable = ptr;
                *pSize_usable = size;
            }

            return iErr;
        }


        //

        public static unsafe int getFieldData_l32(QY_CFGITEM_ntoh_U* pItem, int* plVal)
        {
            if (null == pItem || null == plVal) return -1;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(tmp_dataType));
            tmp_dataType = pItem->head.dataType;

            if (tmp_dataType != Consts.CONST_qyDataType_long) return -1;

            //  *plVal  =  (  (  QY_CFGITEM_LONG  *  )pItem  )->ulVal;
            //memcpy(plVal, &((QY_CFGITEM_LONG*)pItem)->ulVal, sizeof(int));
            *plVal = (int)((QY_CFGITEM_L32*)pItem)->ulVal;

            return 0;
        }

        public static unsafe int getFieldData_long(QY_CFGITEM_ntoh_U* pItem, int* plVal)
        {
            return getFieldData_l32(pItem, plVal);
        }

        public static unsafe int getFieldData_short(QY_CFGITEM_ntoh_U* pItem, short* psVal)
        {
            if (null == pItem || null == psVal) return -1;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(tmp_dataType));
            tmp_dataType = pItem->head.dataType;

            if (tmp_dataType != Consts.CONST_qyDataType_short) return -1;

            //  *psVal  =  (  (  QY_CFGITEM_SHORT  *  )pItem  )->usVal;
            //memcpy(psVal, &((QY_CFGITEM_SHORT*)pItem)->usVal, sizeof(short));
            *psVal = (short)((QY_CFGITEM_SHORT*)pItem)->usVal;

            return 0;

        }

        public static unsafe int getFieldData_byte(QY_CFGITEM_ntoh_U* pItem, byte* pcVal)
        {
            if (null == pItem || null == pcVal) return -1;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(tmp_dataType));
            tmp_dataType = pItem->head.dataType;
            if (tmp_dataType != Consts.CONST_qyDataType_char) return -1;

            *pcVal = ((QY_CFGITEM_BYTE*)pItem)->ucVal;

            return 0;

        }


        public static unsafe int getFieldData_l64(QY_CFGITEM_ntoh_U* pItem, Int64* pl64Val)
        {
            if (null == pItem || null == pl64Val) return -1;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));
            tmp_dataType = pItem->head.dataType;
            if (tmp_dataType != Consts.CONST_qyDataType_l64) return -1;

            *pl64Val = *(Int64*)((QY_CFGITEM_L64*)pItem)->buf_l64Val;
            //mymemcpy(pl64Val, ((QY_CFGITEM_L64*)pItem)->buf_l64Val, sizeof(__int64)); //  2010/04/08

            return 0;

        }

        public static unsafe int getFieldData_data(QY_CFGITEM_ntoh_U* pItem, byte* val, uint* pSize)
        {
            if (null == pSize) return -1;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));
            tmp_dataType = pItem->head.dataType;
            if (tmp_dataType != Consts.CONST_qyDataType_data) return -1;

            //  QY_CFGITEM_DATA *	pItemData	=	(  QY_CFGITEM_DATA  *  )pItem;

            ushort tmp_len;
            //memcpy(&tmp_len, &pItem->data.len, sizeof(short));
            tmp_len = pItem->data.len;
            if (tmp_len > *pSize) return -1;            //  2008/03/23

            mymemcpy((IntPtr)val, (IntPtr)pItem->data.p, tmp_len);
            //	*pSize  =  pItemData->len;
            *pSize = tmp_len;

            return 0;
        }

        public static unsafe int getFieldData_lData(QY_CFGITEM_ntoh_U* pItem, byte* val, uint* pSize)
        {
            if (null == pSize) return -1;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));
            tmp_dataType = pItem->head.dataType;
            if (tmp_dataType != Consts.CONST_qyDataType_lData) return -1;

            //  QY_CFGITEM_LDATA *	pItemData	=	(  QY_CFGITEM_LDATA  *  )pItem;

            uint tmp_ulLen;
            //memcpy(&tmp_ulLen, &pItem->lData.ulLen, sizeof(long));
            tmp_ulLen = pItem->lData.ulLen;
            if (tmp_ulLen > *pSize) return -1;          //  2008/03/23

            mymemcpy((IntPtr)val, (IntPtr)pItem->lData.p, tmp_ulLen);
            *pSize = tmp_ulLen;

            return 0;
        }
        public static unsafe int getFieldData_str(QY_CFGITEM_ntoh_U* pItem, byte* val, uint size)
        {
            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));
            tmp_dataType = pItem->head.dataType;
            if (tmp_dataType != Consts.CONST_qyDataType_str) return -1;

            //  QY_CFGITEM_STR	*	pItemStr	=	(  QY_CFGITEM_STR  *  )pItem;

            ushort tmp_len;
            //memcpy(&tmp_len, &pItem->data.len, sizeof(short));
            tmp_len = pItem->data.len;
            safeStrnCpy(pItem->data.p, val, Math.Min((uint)(tmp_len + 1), size));

            return 0;
        }
        public static unsafe int getFieldData_wStr(QY_CFGITEM_ntoh_U* pItem, char* val, uint size)
        {
            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));
            tmp_dataType = pItem->head.dataType;
            if (tmp_dataType != Consts.CONST_qyDataType_wStr) return -1;

            //  QY_CFGITEM_WSTR  *  pItemWStr  =  (  QY_CFGITEM_WSTR  *  )pItem;

            ushort tmp_len;
            //memcpy(&tmp_len, &pItem->wStr.len, sizeof(short));
            tmp_len = pItem->wStr.len;
            safeWcsnCpy(pItem->wStr.pWStr, val, Math.Min((uint)(tmp_len + 1), size));
            return 0;
        }



        //  2008/12/03, 为通信加一个过滤规则。


        public static unsafe int filterStream_is(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            ushort tmp_len;

            ushort tmp_dataType;
            //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(tmp_dataType));
            tmp_dataType = pItem->head.dataType;

            /*
			switch (tmp_dataType)
			{
				case Consts.CONST_qyDataType_str:
				case Consts.CONST_qyDataType_utf8Str:
					memcpy(&tmp_len, &((QY_CFGITEM_STR*)pItem)->len, sizeof(tmp_len));
					fwFilterBuf(Consts.CONST_filterChars, pItem->data.p, tmp_len);
					break;
				case Consts.CONST_qyDataType_wStr:
					switch (uiStreamId)
					{
						case CONST_imCommType_htmlContent:
							switch (pItem->head.cfgId)
							{
								case CONST_qnmCfgId_hint:
									iErr = 0; goto errLabel;
									break;
								default:
									break;
							}
							break;
						default:
							break;
					}
					memcpy(&tmp_len, &((QY_CFGITEM_WSTR*)pItem)->len, sizeof(tmp_len));
					fwFilterTBuf(_T(CONST_filterChars), pItem->wStr.pWStr, tmp_len);
					break;
				default:
					break;
			}
			*/

            iErr = 0;
        errLabel:
            return iErr;
        }



        public static unsafe int commAuthInfo2Stream_mis(QY_COMM_AUTHINFO_MIS* pAuthInfo, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pAuthInfo) return -1;
            if (null == buf) return -1;

            //


            //
            if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_auth, 0, &ptr, &len)) goto errLabel;
            if (pAuthInfo->ver[0] != 0 && 0 != data2Stream(Consts.CONST_qyDataType_str, Consts.CONST_qnmCfgId_clientVer, pAuthInfo->ver, (uint)mystrlen(pAuthInfo->ver), &ptr, &len)) goto errLabel;
            if (pAuthInfo->usAuthCmd != 0 && 0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_authCmd, (void*)pAuthInfo->usAuthCmd, 0, &ptr, &len)) goto errLabel;
            if (pAuthInfo->iAppType != 0 && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iAppType, (void*)pAuthInfo->iAppType, 0, &ptr, &len)) goto errLabel;       //  2011/03/27
            if (pAuthInfo->iCustemId != 0 && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iCustemId, (void*)pAuthInfo->iCustemId, 0, &ptr, &len)) goto errLabel;
            if (pAuthInfo->uiChannelType != 0 && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiChannelType, (void*)pAuthInfo->uiChannelType, 0, &ptr, &len)) goto errLabel;    //  2007/07/08

            //
            if (pAuthInfo->usLangId != 0 && 0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_langId, (void*)pAuthInfo->usLangId, 0, &ptr, &len)) goto errLabel;    //  2007/07/08
            if (pAuthInfo->uiDevType != 0 && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiDevType_from, (void*)pAuthInfo->uiDevType, 0, &ptr, &len)) goto errLabel;   //  2007/07/08

            //
            if (pAuthInfo->usAuthType != 0 && 0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usAuthType, (void*)pAuthInfo->usAuthType, 0, &ptr, &len)) goto errLabel;    //  2011/03/22

            //
            /*
			for (i = 0; i < mycountof(pAuthInfo->macsInfo.macs); i++)
			{
				if (bMacValid((char*)pAuthInfo->macsInfo.macs[i]) && data2Stream(CONST_qyDataType_str, Consts.CONST_qnmCfgId_mac, pAuthInfo->macsInfo.macs[i], sizeof(pAuthInfo->macsInfo.macs[i]),  &ptr,  &len  )  )  goto errLabel;
		}
			*/

            if (bIpValid(mytoString(pAuthInfo->ip)) && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_ulIp, (void*)myinet_addr(pAuthInfo->ip), 0, &ptr, &len)) goto errLabel;
            //if  (pAuthInfo->pcName[0]  &&  data2Stream(CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_pcName, pAuthInfo->pcName, wcslen(pAuthInfo->pcName  ),  &ptr,  &len  )  )  goto  errLabel;
            //if  (pAuthInfo->domainName[0]  &&  data2Stream(CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_domainName, pAuthInfo->domainName, wcslen(pAuthInfo->domainName  ),  &ptr,  &len  )  )  goto  errLabel;
            //if  (pAuthInfo->osUsrName[0]  &&  data2Stream(CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_osUsrName, pAuthInfo->osUsrName, wcslen(pAuthInfo->osUsrName  ),  &ptr,  &len  )  )  goto  errLabel;


            //
            if (pAuthInfo->messengerName[0] != 0 && 0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_messengerName, pAuthInfo->messengerName, (uint)mywcslen(pAuthInfo->messengerName), &ptr, &len)) goto errLabel;
            if (pAuthInfo->messengerPasswd[0] != 0 && 0 != data2Stream(Consts.CONST_qyDataType_str, Consts.CONST_qnmCfgId_messengerPasswd, pAuthInfo->messengerPasswd, (uint)mystrlen(pAuthInfo->messengerPasswd), &ptr, &len)) goto errLabel;

            //
            if (pAuthInfo->wRndName_serv[0] != 0 && 0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_wRndName_serv, (void*)pAuthInfo->wRndName_serv, (uint)mywcslen(pAuthInfo->wRndName_serv), &ptr, &len)) goto errLabel;  //  2007/11/22
            if (pAuthInfo->uiRnd_cli != 0 && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiRnd_cli, (void*)pAuthInfo->uiRnd_cli, 0, &ptr, &len)) goto errLabel;  //  2007/11/22
                                                                                                                                                                                          //  if  (  pAuthInfo->uiWebSessId  &&  data2Stream(  CONST_qyDataType_long,  Consts.CONST_qnmCfgId_uiWebSessId,  (  void  *  )pAuthInfo->uiWebSessId,  0,  &ptr,  &len  )  )  goto  errLabel;  //  2007/11/22
            if (pAuthInfo->uiLogonId != 0 && 0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiLogonId, (void*)pAuthInfo->uiLogonId, 0, &ptr, &len)) goto errLabel;  //  2007/11/22

            //  2008/06/10
            if (pAuthInfo->uiBlobLen_sessionKey != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_sessionKeyBlob, pAuthInfo->sessionKeyBlob, pAuthInfo->uiBlobLen_sessionKey, &ptr, &len)) goto errLabel;
            }

            if (pAuthInfo->usSessionEncType != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usSessionEncType, (void*)pAuthInfo->usSessionEncType, 0, &ptr, &len)) goto errLabel;    //  2011/03/22
            }
            if (pAuthInfo->usLen_sessionIv != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_sessionIv, pAuthInfo->sessionIv, pAuthInfo->usLen_sessionIv, &ptr, &len)) goto errLabel;
            }



            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }



        public static unsafe int tmpHandler_stream2CommAuthInfo_mis(void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            QY_COMM_AUTHINFO_MIS* pAuthInfo = (QY_COMM_AUTHINFO_MIS*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            int lVal;
            short sVal = 0;

            if (uiStreamId != Consts.CONST_imCommType_auth) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:  //  2007/12/11
                    break;
                case Consts.CONST_qnmCfgId_null:       //  2007/12/11
                    break;
                case Consts.CONST_qnmCfgId_clientVer:
                    getFieldData_str(pItem, pAuthInfo->ver, Consts.CONST_qyMaxVerLen + 1);
                    break;
                case Consts.CONST_qnmCfgId_authCmd:
                    if (0 != getFieldData_short(pItem, &sVal)) goto errLabel;
                    pAuthInfo->usAuthCmd = (ushort)sVal;
                    break;
                case Consts.CONST_qnmCfgId_iAppType:                         //  2011/03/27
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    pAuthInfo->iAppType = lVal;
                    break;
                case Consts.CONST_qnmCfgId_iCustemId:
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    pAuthInfo->iCustemId = lVal;
                    break;
                case Consts.CONST_qnmCfgId_uiChannelType:    //  2007/07/08
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    pAuthInfo->uiChannelType = (uint)lVal;
                    break;
                case Consts.CONST_qnmCfgId_langId:
                    ushort tmp_dataType;
                    //memcpy(&tmp_dataType, &pItem->head.dataType, sizeof(short));
                    tmp_dataType = pItem->head.dataType;
                    if (tmp_dataType == Consts.CONST_qyDataType_long)
                    {
                        if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                        pAuthInfo->usLangId = (ushort)lVal;
                    }
                    else if (tmp_dataType == Consts.CONST_qyDataType_short)
                    {           //  2007/12/07, 允许usLangId以long或者short的形式发送.
                        if (0 != getFieldData_short(pItem, (short*)&pAuthInfo->usLangId)) goto errLabel;
                    }
                    else
                    {
                        //traceLogA("CONST_qnmCfgId_usLangId的传输类型不对。"); 
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_qnmCfgId_uiDevType_from:
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    pAuthInfo->uiDevType = (uint)lVal;
                    break;
                case Consts.CONST_qnmCfgId_usAuthType:           //  2011/03/22
                    if (0 != getFieldData_short(pItem, &sVal)) goto errLabel;
                    pAuthInfo->usAuthType = (ushort)sVal;
                    break;
                case Consts.CONST_qnmCfgId_ulIp:
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    ulIp2Str((uint)lVal, pAuthInfo->ip, Consts.CONST_qyMaxIpLen + 1);
                    break;
                case Consts.CONST_qnmCfgId_messengerName:
                    getFieldData_wStr(pItem, pAuthInfo->messengerName, Consts.CONST_maxMessengerNameLen + 1);
                    break;
                case Consts.CONST_qnmCfgId_messengerPasswd:  //  2010/01/02
                    getFieldData_str(pItem, pAuthInfo->messengerPasswd, Consts.CONST_maxMessengerPasswdLen + 1);
                    break;
                case Consts.CONST_qnmCfgId_wRndName_serv:
                    getFieldData_wStr(pItem, pAuthInfo->wRndName_serv, Consts.CONST_cntof_wRndName_serv);
                    break;
                case Consts.CONST_qnmCfgId_uiRnd_cli:    //  2007/11/22
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    pAuthInfo->uiRnd_cli = (uint)lVal;
                    break;
                case Consts.CONST_qnmCfgId_uiLogonId:    //  2007/11/22
                    if (0 != getFieldData_long(pItem, &lVal)) goto errLabel;
                    pAuthInfo->uiLogonId = (uint)lVal;
                    break;
                case Consts.CONST_qnmCfgId_sessionKeyBlob:
                    pAuthInfo->uiBlobLen_sessionKey = Consts.bufSize_sessionKeyBlob;
                    if (0 != getFieldData_data(pItem, pAuthInfo->sessionKeyBlob, &pAuthInfo->uiBlobLen_sessionKey)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usSessionEncType:
                    if (0 != getFieldData_short(pItem, (short*)&pAuthInfo->usSessionEncType)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_sessionIv:
                    uint uiLen;
                    uiLen = Consts.bufSize_sessionIv;
                    if (0 != getFieldData_data(pItem, pAuthInfo->sessionIv, &uiLen)) goto errLabel;
                    pAuthInfo->usLen_sessionIv = (ushort)uiLen;
                    break;
                default:
                    //traceLogA("tmpHandler_stream2CommAuthInfo_mis: 未处理的cfgId %d", pItem->head.cfgId);
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int authResp2Stream_mis(uint uiStreamId, AUTH_RESP_MIS* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            //  serverHello
            if (pReq->uiCommEncCtxType != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiCommEncCtxType, (void*)pReq->uiCommEncCtxType, 0, &ptr, &len)) goto errLabel;
            }
            if (pReq->uiBlobLen_publicKey != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_publicKeyBlob, pReq->publicKeyBlob, pReq->uiBlobLen_publicKey, &ptr, &len)) goto errLabel;
            }

            //  
            if (pReq->idInfo.ui64Id != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, &pReq->idInfo.ui64Id, sizeof(ulong), &ptr, &len)) goto errLabel;
            }

            //
            if (pReq->uiLogonId != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiLogonId, (void*)pReq->uiLogonId, 0, &ptr, &len)) goto errLabel;
            }
            if (pReq->wRndName_serv[0] != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_wRndName_serv, pReq->wRndName_serv, (uint)mywcslen(pReq->wRndName_serv), &ptr, &len)) goto errLabel;
            }

            //  2009/10/11
            if (pReq->ucbDaemonDemo)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbDaemonDemo, (void*)(pReq->ucbDaemonDemo ? 1 : 0), 0, &ptr, &len)) goto errLabel;
            }
            //  2009/11/05
            if (pReq->usMaxVideoConferenceMems != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usMaxVideoConferenceMems, (void*)pReq->usMaxVideoConferenceMems, 0, &ptr, &len)) goto errLabel;
            }
            if (pReq->ucDays_left != 0)
            {   //  2010/03/22
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucDays_left, (void*)pReq->ucDays_left, 0, &ptr, &len)) goto errLabel;
            }

            //  2011/10/22
            //if (pReq->usAppAvLevel_daemon!=0 && 0!=data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usAppAvLevel, (void*)pReq->usAppAvLevel_daemon, 0, &ptr, &len)) goto errLabel;

            //  2017/06/21
            if (pReq->usSubSystemId_daemon != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_subSystemId, (void*)pReq->usSubSystemId_daemon, 0, &ptr, &len)) goto errLabel;
            }

            //  2011/04/07
            if (pReq->talkerDesc[0] != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_talkerDesc, pReq->talkerDesc, (uint)mywcslen(pReq->talkerDesc), &ptr, &len)) goto errLabel;
            }

            //  2016/08/10
            if (pReq->clientWelcomeMsg[0] != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_clientWelcomeMsg, pReq->clientWelcomeMsg, (uint)mywcslen(pReq->clientWelcomeMsg), &ptr, &len)) goto errLabel;
            }

            //
            if (pReq->wBuf_hint[0] != 0)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_hint, pReq->wBuf_hint, (uint)mywcslen(pReq->wBuf_hint), &ptr, &len)) goto errLabel;
            }


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2AuthResp_mis(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0
            AUTH_RESP_MIS* pAuthResp = (AUTH_RESP_MIS*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;

            if (uiStreamId != Consts.CONST_imCommType_authResp) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_uiCommEncCtxType:
                    if (0 != getFieldData_l32(pItem, (int*)&pAuthResp->uiCommEncCtxType)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_publicKeyBlob:
                    pAuthResp->uiBlobLen_publicKey = Consts.bufSize_publicKeyBlob;  // sizeof(pAuthResp->publicKeyBlob  );
                    if (0 != getFieldData_data(pItem, pAuthResp->publicKeyBlob, &pAuthResp->uiBlobLen_publicKey)) goto errLabel;
                    break;
                //
                case Consts.CONST_qnmCfgId_messengerId:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pAuthResp->idInfo.ui64Id)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiLogonId:
                    if (0 != getFieldData_l32(pItem, (int*)&pAuthResp->uiLogonId)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_wRndName_serv:
                    if (0 != getFieldData_wStr(pItem, pAuthResp->wRndName_serv, Consts.CONST_cntof_wRndName_serv)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_ucbDaemonDemo:  //  2009/10/11
                    if (0 != getFieldData_byte(pItem, (byte*)&pAuthResp->ucbDaemonDemo)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_usMaxVideoConferenceMems:   //  2009/11/05
                    if (0 != getFieldData_short(pItem, (short*)&pAuthResp->usMaxVideoConferenceMems)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_ucDays_left:                //  2010/03/22
                    if (0 != getFieldData_byte(pItem, (byte*)&pAuthResp->ucDays_left)) goto errLabel;
                    break;
                //
#if false
				case Consts.CONST_qnmCfgId_usAppAvLevel:       //  2011/10/22
					getFieldData_short(pItem, (short*)&pAuthResp->usAppAvLevel_daemon);
					break;
#endif
                //
                case Consts.CONST_qnmCfgId_subSystemId:
                    getFieldData_short(pItem, (short*)&pAuthResp->usSubSystemId_daemon);
                    break;
                //
                case Consts.CONST_qnmCfgId_talkerDesc:
                    if (0 != getFieldData_wStr(pItem, pAuthResp->talkerDesc, Consts.cntof_talkerDesc)) goto errLabel;
                    break;
                //
                case Consts.CONST_qnmCfgId_clientWelcomeMsg:  //  2016/08/10
                    if (0 != getFieldData_wStr(pItem, pAuthResp->clientWelcomeMsg, Consts.CONST_cntof_clientWelcomMsg)) goto errLabel;
                    break;
                //
                case Consts.CONST_qnmCfgId_hint:
                    getFieldData_wStr(pItem, pAuthResp->wBuf_hint, Consts.cntof_hint);
                    break;
                default:
                    break;

            }
            iErr = 0;
        errLabel:
            return iErr;

        }


        ///////////////////////////////////////////////////////

        public static unsafe int msgRoute2Stream(uint uiStreamId, MSG_ROUTE* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->idInfo_to.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId_to, (void*)&pReq->idInfo_to.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->idInfo_from.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId_from, (void*)&pReq->idInfo_from.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->uiMsgRouteId)
            {       //  2008/05/27
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiMsgRouteId, (void*)pReq->uiMsgRouteId, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->uiLogonId)
            {           //  2007/12/22
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiLogonId, (void*)pReq->uiLogonId, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->usLangId_from)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_langId, (void*)pReq->usLangId_from, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->uiDevType_to)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiDevType_to, (void*)pReq->uiDevType_to, 0, &ptr, &len)) goto errLabel;
            }
            /*
			 * if (0 != pReq->wDevIdStr_to[0])
			{
				if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_wDevIdStr_to, pReq->wDevIdStr_to, lstrlen(pReq->wDevIdStr_to), &ptr, &len)) goto errLabel;
			}
			*/
            if (0 != pReq->uiDevType_from)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiDevType_from, (void*)pReq->uiDevType_from, 0, &ptr, &len)) goto errLabel;
            }
            /*
			if (0 != pReq->wDevIdStr_from[0])
			{
				if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_wDevIdStr_from, pReq->wDevIdStr_from, lstrlen(pReq->wDevIdStr_from), &ptr, &len)) goto errLabel;
			}
			*/
            //
            /*	 
			struct												{
					QY_MESSENGER_ID							idInfo;
			}													mems_to[Consts.CONST_maxMsgrs_sendTo];	//  2008/03/27, 可同时发到的若干联系人.
			*/
            //  2009/09/12
            for (i = 0; i < Consts.CONST_maxMsgrs_sendTo; i++)
            {
                if (0 != pReq->mems_to_idInfo_ui64Id[i])
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId_to, (void*)&pReq->mems_to_idInfo_ui64Id[i], sizeof(UInt64), &ptr, &len)) goto errLabel;
                }
            }
            //
            if (0 != pReq->uiObjType_src)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pReq->uiObjType_src, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->ulDetectedIp_from)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_ulDetectedIp, (void*)pReq->ulDetectedIp_from, 0, &ptr, &len)) goto errLabel;
            }
            /*
            if (0 != pReq->displayName_from[0])
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_displayName, pReq->displayName_from, (uint)mywcslen(pReq->displayName_from), &ptr, &len)) goto errLabel;
            }
            */





            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2MsgRoute(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            MSG_ROUTE* pContent = (MSG_ROUTE*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_msgRoute) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            switch (tmp_cfgId)
            {
                /*
		case Consts.CONST_qnmCfgId_displayName:
			if (0!=getFieldData_wStr(pItem, pContent->displayName_from, mycountof(pContent->displayName_from))) goto errLabel;
			break;
				*/
                case Consts.CONST_qnmCfgId_ulDetectedIp:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->ulDetectedIp_from)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiObjType:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiObjType_src)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiDevType_from:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiDevType_from)) goto errLabel;
                    break;
                /*
    case Consts.CONST_qnmCfgId_wDevIdStr_from:
        if (0 != getFieldData_wStr(pItem, pContent->wDevIdStr_from, mycountof(pContent->wDevIdStr_from))) goto errLabel;
        break;
                */
                case Consts.CONST_qnmCfgId_uiDevType_to:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiDevType_to)) goto errLabel;
                    break;
                /*
    case Consts.CONST_qnmCfgId_wDevIdStr_to:
        if (0 != getFieldData_wStr(pItem, pContent->wDevIdStr_to, mycountof(pContent->wDevIdStr_to))) goto errLabel;
        break;
                */
                case Consts.CONST_qnmCfgId_langId:
                    if (0 != getFieldData_short(pItem, (short*)&pContent->usLangId_from)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiLogonId:      //  2007/12/22
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiLogonId)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiMsgRouteId:       //  2008/05/27
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiMsgRouteId)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_messengerId_from:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_from.ui64Id)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_messengerId_to:
                    if (0 != pContent->idInfo_to.ui64Id)
                    {
                        if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_to.ui64Id)) goto errLabel;
                    }
                    else
                    {
                        if (pContent->tmpInternal_usCnt_mems_to >= Consts.CONST_maxMsgrs_sendTo)
                        {
                            traceLogA("stream2MsgRoute: usCnt_mems_to >= mycountof(  mems_to  )");
                            break;
                        }
                        if (0 != getFieldData_l64(pItem, (Int64*)&pContent->mems_to_idInfo_ui64Id[pContent->tmpInternal_usCnt_mems_to])) goto errLabel;
                        pContent->tmpInternal_usCnt_mems_to++;
                    }
                    break;

                //  下面为一些用于性能评估的参数

                //
                //  case  Consts.CONST_qnmCfgId_dwTickCnt_resp_recved:
                //	   if  (  getFieldData_long(  pItem,  (  long  *  )&pContent->debugInfo.dwTickCnt_resp_recved  )  )  goto  errLabel;
                //	   break;
                //  case  Consts.CONST_qnmCfgId_usElapsed_resp_end:
                //	   if  (  getFieldData_short(  pItem,  (  short  *  )&pContent->debugInfo.usElapsed_resp_end  )  )  goto  errLabel;
                //	   break;


                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int imHtmlContent2Stream(uint uiStreamId, IM_HTML_CONTENT* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            //  2014/07/11
            if (0 != pReq->ucbResp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbResp, (void*)pReq->ucbResp, 0, &ptr, &len)) goto errLabel;
            }

            //  2015/02/16
            if (0 != pReq->idInfo_logicalPeer.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_logicalPeer, (void*)&pReq->idInfo_logicalPeer.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }



            //
            if (0 != pReq->uiSerialNo)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiSerialNo, (void*)pReq->uiSerialNo, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->wBuf[0])
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_wStr, Consts.CONST_qnmCfgId_hint, pReq->wBuf, (uint)mylstrlen(pReq->wBuf), &ptr, &len)) goto errLabel;
            }

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2ImHtmlContent(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            IM_HTML_CONTENT* pContent = (IM_HTML_CONTENT*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pContent) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_htmlContent) goto errLabel;

            ushort tmp_cfgId;
            //mymemcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;
            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_start:
                    pContent->uiType = uiStreamId;
                    break;
                //  2014/07/11
                case Consts.CONST_qnmCfgId_ucbResp:
                    if (0 != getFieldData_byte(pItem, (byte*)&pContent->ucbResp)) goto errLabel;
                    break;
                //  2015/02/14
                case Consts.CONST_qnmCfgId_idInfo_logicalPeer:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_logicalPeer.ui64Id)) goto errLabel;
                    break;

                //
                case Consts.CONST_qnmCfgId_hint:
                    if (0 != getFieldData_wStr(pItem, pContent->wBuf, Consts.cntof_imHtmlContent_wBuf)) goto errLabel;
                    break;
#if false
			 case  Consts.CONST_qnmCfgId_tStartTime_serv:
				   if  (  getFieldData_l64(  pItem,  (  __int64  *  )&pContent->tStartTime_serv  )  )  goto  errLabel;
				   break;
#endif
                case Consts.CONST_qnmCfgId_uiSerialNo:
                    if (0 != getFieldData_long(pItem, (int*)&pContent->uiSerialNo)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="pConfCfg"></param>
        /// <param name="ppBuf"></param>
        /// <param name="puiBufSize"></param>
        /// <returns></returns>
        /// 



        public static unsafe int qyhtonVideoHeader(QY_VIDEO_HEADER* pVh)
        {
            if (null == pVh) return -1;
            BITMAPINFOHEADER* p = &pVh->bih;

            if (0 == p->biSize) p->biSize = sizeof(BITMAPINFOHEADER);
            if (p->biSize < sizeof(BITMAPINFOHEADER))
            {
                //traceLogA("Warning: qyhtonVideoHeader failed: biSize too small, %d", p->biSize);
                //qyShowInfo1(CONST_qyShowType_qwmComm, NULL, (""), _T("IsClient"), 0, _T(""), _T(""), _T("qyhtonVideoHeader: biSize to small %d"), p->biSize);
                return -1;
            }
            if (p->biSize > sizeof(BITMAPINFOHEADER) + Consts.bufSize_bPrivateData)
            {
                //traceLogA("qyhtonVideoHeader failed: biSize too large, %d", p->biSize);
                return -1;
            }

            int biSize = p->biSize;

            MACRO_htoni(ref p->biSize);
            MACRO_htoni(ref p->biWidth);
            MACRO_htoni(ref p->biHeight);
            MACRO_htons(ref p->biPlanes);
            MACRO_htons(ref p->biBitCount);
            MACRO_htoni(ref p->biCompression);
            MACRO_htoni(ref p->biSizeImage);
            MACRO_htoni(ref p->biXPelsPerMeter);
            MACRO_htoni(ref p->biYPelsPerMeter);
            MACRO_htoni(ref p->biClrUsed);
            MACRO_htoni(ref p->biClrImportant);

            return biSize;
        }

        public static unsafe int qyntohVideoHeader(QY_VIDEO_HEADER* pVh)
        {
            if (null == pVh) return -1;
            BITMAPINFOHEADER* p = &pVh->bih;

            MACRO_ntohi(ref p->biSize);

            int tmp_biSize;
            //memcpy(&tmp_biSize, &p->biSize, sizeof(tmp_biSize));
            tmp_biSize = p->biSize;
            if (0 == tmp_biSize) tmp_biSize = sizeof(BITMAPINFOHEADER);
            if (tmp_biSize < sizeof(BITMAPINFOHEADER))
            {
                //traceLogA("Warning: qyntohVideoHeader: biSize too small, %d", tmp_biSize);
                //qyShowInfo1(CONST_qyShowType_qwmComm, NULL, (""), _T("IsClient"), 0, _T(""), _T(""), _T("qyntohVideoHeader: biSize to small %d"), tmp_biSize);
                return -1;
            }
            if (tmp_biSize > sizeof(BITMAPINFOHEADER) + Consts.bufSize_bPrivateData)
            {
                //traceLogA("qyntohVideoHeader: biSize too large, %d", tmp_biSize);
                return -1;
            }
            //memcpy(&p->biSize, &tmp_biSize, sizeof(p->biSize)  );
            p->biSize = tmp_biSize;

            MACRO_ntohi(ref p->biWidth);
            MACRO_ntohi(ref p->biHeight);
            MACRO_ntohs(ref p->biPlanes);
            MACRO_ntohs(ref p->biBitCount);
            MACRO_ntohi(ref p->biCompression);
            MACRO_ntohi(ref p->biSizeImage);
            MACRO_ntohi(ref p->biXPelsPerMeter);
            MACRO_ntohi(ref p->biYPelsPerMeter);
            MACRO_ntohi(ref p->biClrUsed);
            MACRO_ntohi(ref p->biClrImportant);

            return sizeof(QY_VIDEO_HEADER);
        }





        /////////////////

        public static unsafe int qyhtonAudioHeader(QY_AUDIO_HEADER* pAh)
        {
            if (null == pAh) return -1;
            WAVEFORMATEX* p = &pAh->wfx;
            if (p->cbSize > Consts.bufSize_bExtraData)
            {
                //traceLogA("qyhtonAudioHeader failed: cbSize too large, %d", p->cbSize);
                return -1;
            }

            int biSize = sizeof(WAVEFORMATEX) + p->cbSize;

            MACRO_htons(ref p->wFormatTag);        // format type 
            MACRO_htons(ref p->nChannels);         // number of channels (i.e. mono, stereo...) 
            MACRO_htoni(ref p->nSamplesPerSec);    // sample rate 
            MACRO_htoni(ref p->nAvgBytesPerSec);   // for buffer estimation 
            MACRO_htons(ref p->nBlockAlign);       // block size of data 
            MACRO_htons(ref p->wBitsPerSample);    // Number of bits per sample of mono data 
            MACRO_htons(ref p->cbSize);            // The count in bytes of the size of  

            return biSize;
        }

        public static unsafe int qyntohAudioHeader(QY_AUDIO_HEADER* pAh)
        {
            if (null == pAh) return -1;
            WAVEFORMATEX* p = &pAh->wfx;

            MACRO_ntohs(ref p->wFormatTag);        // format type 
            MACRO_ntohs(ref p->nChannels);         // number of channels (i.e. mono, stereo...) 
            MACRO_ntohi(ref p->nSamplesPerSec);    // sample rate 
            MACRO_ntohi(ref p->nAvgBytesPerSec);   // for buffer estimation 
            MACRO_ntohs(ref p->nBlockAlign);       // block size of data 
            MACRO_ntohs(ref p->wBitsPerSample);    // Number of bits per sample of mono data 
            MACRO_ntohs(ref p->cbSize);            // The count in bytes of the size of  

            ushort tmp_cbSize;
            //memcpy(&tmp_cbSize, &p->cbSize, sizeof(tmp_cbSize));
            tmp_cbSize = p->cbSize;
            if (tmp_cbSize > Consts.bufSize_bExtraData)
            {
                //traceLogA("qyntohAudioHeader failed: cbSize too large, %d", tmp_cbSize);
                return -1;
            }

            return sizeof(WAVEFORMATEX);
        }


        //  2014/11/07
        public static unsafe int tmp_htonTranVideo(TRAN_video* pTranVideo, byte** ppBuf, uint* puiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = *ppBuf;
            uint len = *puiBufSize;


#if true    //  2014/11/07
            //
            if (0 != pTranVideo->uiTranNo_openAvDev)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openVideoDev, (void*)pTranVideo->uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pTranVideo->compressor.ucCompressors)
            {  //  200903/30
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucVideoCompressors, (void*)pTranVideo->compressor.ucCompressors, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pTranVideo->compressor.iFourcc)
            {
                //if  (  0!=data2Stream(  Consts.CONST_qyDataType_str,  Consts.CONST_qnmCfgId_fourccStr,  pTranVideo->compressor.common.fourccStr,  (uint)mystrlen(  pTranVideo->compressor.common.fourccStr  ),  &ptr,  &len  )  )  goto  errLabel;
                if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_iFourcc, (void*)pTranVideo->compressor.iFourcc, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pTranVideo->compressor.ucbNeedKeyFrameFirst)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_byte, Consts.CONST_qnmCfgId_ucbNeedKeyFrameFirst, (void*)pTranVideo->compressor.ucbNeedKeyFrameFirst, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pTranVideo->compressor.usMaxFps_toShareBmp)
            {   //  2014/08/07
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usMaxFps_toShareBmp, (void*)pTranVideo->compressor.usMaxFps_toShareBmp, 0, &ptr, &len)) goto errLabel;
            }
            //

            //  
            int tmpLen;
            QY_VIDEO_HEADER vh;
            mymemcpy((IntPtr)(byte*)&vh, (IntPtr)(byte*)&pTranVideo->vh_compress, (uint)sizeof(QY_VIDEO_HEADER));
            if ((tmpLen = qyhtonVideoHeader(&vh)) < 0) goto errLabel;
            if (0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_bih_compress, &vh, (uint)tmpLen, &ptr, &len)) goto errLabel;

            //  2014/09/23
            if (bSupported_sendVhDec())
            {
                mymemcpy((IntPtr)(byte*)&vh, (IntPtr)(byte*)&pTranVideo->vh_decompress, (uint)sizeof(QY_VIDEO_HEADER));
                if ((tmpLen = qyhtonVideoHeader(&vh)) < 0) goto errLabel;
                if (0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_bih_decompress, &vh, (uint)tmpLen, &ptr, &len)) goto errLabel;
            }

            //  2014/03/30
            if (0 != pTranVideo->vh_stream.usCnt)
            {
                //byte  spsPps[512]  =  "";
                TMP_buf1024 spsPps;
                int bufSize_spsPps = Consts.bufSize_1024;
                uint spsLen = 0;
                //  2017/10/10
                spsLen = (uint)bufSize_spsPps;
                if (0 != makeSpsPps(&pTranVideo->vh_stream, spsPps.buf, &spsLen))
                {
                    uint step = 0;
                    qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), ("IsClient"), null, (""), (""), ("htonAvTranInfo failed: makeSpsPps failed"));
                    goto errLabel;
                }
                //
                if (0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_spsPps, spsPps.buf, spsLen, &ptr, &len)) goto errLabel;
            }
#endif


            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *ppBuf = ptr;
                *puiBufSize = len;
            }
            return iErr;

        }

        public unsafe struct TMP_buf_vh
        {
            public fixed byte buf[Consts.bufSize_vh];
        };

        public static unsafe int tmp_ntohTranVideo(QY_CFGITEM_ntoh_U* pItem, TRAN_video* pTranVideo, bool* pbProcessed)
        {
            int iErr = -1;
            int tmpLen;
            //char buf[sizeof(QY_VIDEO_HEADER) + 1024];
            TMP_buf_vh buf_vh;
            int bufSize = Consts.bufSize_vh;
            bool bProcessed = false;

            //
            if (sizeof(QY_VIDEO_HEADER) + 1024 > Consts.bufSize_vh)
            {
                Console.WriteLine("too small buf");
            }


            //
            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;


            //
            if (null != pTranVideo)
            {
                //
                bProcessed = true;
                //
                switch (tmp_cfgId)
                {
                    case Consts.CONST_qnmCfgId_uiTranNo_openVideoDev:
                        if (0 != getFieldData_long(pItem, (int*)&pTranVideo->uiTranNo_openAvDev)) goto errLabel;
                        break;

                    case Consts.CONST_qnmCfgId_ucVideoCompressors: //  2009/03/30
                        if (0 != getFieldData_byte(pItem, (byte*)&pTranVideo->compressor.ucCompressors)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_iFourcc:
                        //if (0!=getFieldData_str(pItem, pTranVideo->compressor.common.fourccStr, Consts.CONST_fourccStrLen+1)) goto errLabel;
                        if (0 != getFieldData_l32(pItem, &pTranVideo->compressor.iFourcc)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_ucbNeedKeyFrameFirst:
                        if (0 != getFieldData_byte(pItem, (byte*)&pTranVideo->compressor.ucbNeedKeyFrameFirst)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_usMaxFps_toShareBmp:    //  2014/08/07
                        if (0 != getFieldData_short(pItem, (short*)&pTranVideo->compressor.usMaxFps_toShareBmp)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_bih_compress:
                        tmpLen = Consts.bufSize_vh;
                        if (0 != getFieldData_data(pItem, buf_vh.buf, (uint*)&tmpLen)) goto errLabel;
                        if (tmpLen > Consts.bufSize_vh) goto errLabel;                                            //  impossible  
                        mymemset((IntPtr)(buf_vh.buf + tmpLen), 0, Consts.bufSize_vh - tmpLen);
                        if (qyntohVideoHeader((QY_VIDEO_HEADER*)buf_vh.buf) < 0) goto errLabel;
                        mymemcpy((IntPtr)(byte*)&pTranVideo->vh_compress, (IntPtr)buf_vh.buf, (uint)sizeof(QY_VIDEO_HEADER));
                        break;
                    case Consts.CONST_qnmCfgId_bih_decompress:
                        tmpLen = Consts.bufSize_vh;
                        if (0 != getFieldData_data(pItem, buf_vh.buf, (uint*)&tmpLen)) goto errLabel;
                        if (tmpLen > Consts.bufSize_vh) goto errLabel;                                            //  impossible  
                        mymemset((IntPtr)(buf_vh.buf + tmpLen), 0, Consts.bufSize_vh - tmpLen);
                        if (qyntohVideoHeader((QY_VIDEO_HEADER*)buf_vh.buf) < 0) goto errLabel;
                        mymemcpy((IntPtr)(byte*)&pTranVideo->vh_decompress, (IntPtr)buf_vh.buf, (uint)sizeof(QY_VIDEO_HEADER));
                        break;
                    case Consts.CONST_qnmCfgId_spsPps:
                        {   //  2014/03/30
                            tmpLen = Consts.bufSize_vh;
                            if (0 != getFieldData_data(pItem, buf_vh.buf, (uint*)&tmpLen)) goto errLabel;
                            if (tmpLen > Consts.bufSize_vh) goto errLabel;
                            //  2017/10/09
                            int iFourcc = pTranVideo->compressor.iFourcc;// fourccStr2i(pTranVideo->compressor.common.fourccStr);
                            if (0 == iFourcc)
                            {
                                iFourcc = (int)pTranVideo->vh_stream.dwFourcc;
                            }
                            //
                            VH_stream* pVh_stream = &pTranVideo->vh_stream;
                            //
                            //  2017/10/10
                            if (0 != parseSpsPps1(iFourcc, buf_vh.buf, tmpLen, pVh_stream))
                            {
                                uint step = 0;
                                qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref step, (""), ("IsClient"), null, (""), (""), ("tmp_ntohTranVideo: parseSpsPps1 failed"));
                                goto errLabel;
                            }
                            //
                        }
                        break;
                    default:
                        //
                        bProcessed = false;

                        break;
                }
            }


            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                if (null != pbProcessed) *pbProcessed = bProcessed;
            }
            return iErr;
        }


        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="pConfCfg"></param>
        /// <param name="ppBuf"></param>
        /// <param name="puiBufSize"></param>
        /// <returns></returns>
        /// 

        public static unsafe int tmp_htonTranAudio(TRAN_audio* pTranAudio, byte** ppBuf, uint* puiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = *ppBuf;
            uint len = *puiBufSize;


            //  2014/11/07
            //
            if (0 != pTranAudio->uiTranNo_openAvDev)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_openAudioDev, (void*)pTranAudio->uiTranNo_openAvDev, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pTranAudio->compressor.ucCompressors)
            {       //  2009/05/18
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucAudioCompressors, (void*)pTranAudio->compressor.ucCompressors, 0, &ptr, &len)) goto errLabel;
            }
            /*
			if (0 != pTranAudio->compressor.uiBytesRecorded_perBlockAlign)
			{
				if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiBytesRecorded_perBlockAlign, (void*)pTranAudio->compressor.uiBytesRecorded_perBlockAlign, 0, &ptr, &len)) goto errLabel;
			}
			*/


            int tmpLen;
            //  WAVEFORMATEX										wf;
            QY_AUDIO_HEADER ah;
            mymemcpy((IntPtr)(byte*)&ah, (IntPtr)(byte*)&pTranAudio->ah_compress, (uint)sizeof(QY_AUDIO_HEADER));
            if ((tmpLen = qyhtonAudioHeader(&ah)) < 0) goto errLabel;
            if ((0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_wf_compress, &ah, (uint)tmpLen, &ptr, &len))) goto errLabel;

            //  2009/04/27
            mymemcpy((IntPtr)(byte*)&ah, (IntPtr)(byte*)&pTranAudio->ah_decompress, (uint)sizeof(QY_AUDIO_HEADER));
            if ((tmpLen = qyhtonAudioHeader(&ah)) < 0) goto errLabel;
            if ((0 != data2Stream(Consts.CONST_qyDataType_data, Consts.CONST_qnmCfgId_wf_decompress, &ah, (uint)tmpLen, &ptr, &len))) goto errLabel;

            //  2016/12/23
            /*
			if (pTranAudio->ah_stream.mems[0].usLen + pTranAudio->ah_stream.mems[1].usLen + pTranAudio->ah_stream.mems[2].usLen)
			{
				AH_stream* pAs = &pTranAudio->ah_stream;
				if (pAs->mems[0].usLen + pAs->mems[1].usLen + pAs->mems[2].usLen > sizeof(pAs->buf)  )  goto errLabel;
				char* pHeader0 = pAs->buf;
				char* pHeader1 = pAs->buf + pAs->mems[0].usLen;
				char* pHeader2 = pAs->buf + pAs->mems[0].usLen + pAs->mems[1].usLen;
				//

				//
#if __DEBUG__
#endif

			}
		*/



            //
            //  2014/11/07
            //if (tmp_htonAudioPlayCfg(&pTranAudio->compressor.playCfg, &ptr, &len)) goto errLabel;



            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *ppBuf = ptr;
                *puiBufSize = len;
            }
            return iErr;

        }



        public unsafe struct TMP_buf_ah
        {
            public fixed byte buf[Consts.bufSize_ah];
        }


        public static unsafe int tmp_ntohTranAudio(QY_CFGITEM_ntoh_U* pItem, TRAN_audio* pTranAudio, bool* pbProcessed)
        {
            int iErr = -1;
            int tmpLen;
            //byte buf[max(sizeof(QY_AUDIO_HEADER), sizeof(AH_stream)) + 1024];
            TMP_buf_ah buf;
            int bufSize = Consts.bufSize_ah;

            bool bProcessed = false;

            //
            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;


            if (null != pTranAudio)
            {
                //
                bProcessed = true;
                //
                switch (tmp_cfgId)
                {
                    //
                    case Consts.CONST_qnmCfgId_uiTranNo_openAudioDev:
                        if (0 != getFieldData_long(pItem, (int*)&pTranAudio->uiTranNo_openAvDev)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_ucAudioCompressors:     //  2009/05/18
                        if (0 != getFieldData_byte(pItem, (byte*)&pTranAudio->compressor.ucCompressors)) goto errLabel;
                        break;
                    /*
                case Consts.CONST_qnmCfgId_uiBytesRecorded_perBlockAlign:
                    if (0!=getFieldData_long(pItem, (int*)&pTranAudio->compressor.uiBytesRecorded_perBlockAlign)) goto errLabel;
                    break;
                    */
                    case Consts.CONST_qnmCfgId_wf_compress:
                        tmpLen = bufSize;// sizeof(buf);
                        if (0 != getFieldData_data(pItem, buf.buf, (uint*)&tmpLen)) goto errLabel;
                        if (tmpLen > bufSize) goto errLabel;
                        mymemset((IntPtr)(buf.buf + tmpLen), 0, bufSize - tmpLen);
                        if (qyntohAudioHeader((QY_AUDIO_HEADER*)buf.buf) < 0) goto errLabel;
                        mymemcpy((IntPtr)(byte*)&pTranAudio->ah_compress, (IntPtr)buf.buf, (uint)sizeof(QY_AUDIO_HEADER));
                        break;
                    case Consts.CONST_qnmCfgId_wf_decompress:  //  2009/04/27
                        tmpLen = bufSize;// sizeof(buf);
                        if (0 != getFieldData_data(pItem, buf.buf, (uint*)&tmpLen)) goto errLabel;
                        if (tmpLen > bufSize) goto errLabel;
                        mymemset((IntPtr)(buf.buf + tmpLen), 0, bufSize - tmpLen);
                        if (qyntohAudioHeader((QY_AUDIO_HEADER*)buf.buf) < 0) goto errLabel;
                        mymemcpy((IntPtr)(byte*)&pTranAudio->ah_decompress, (IntPtr)buf.buf, (uint)sizeof(QY_AUDIO_HEADER));
                        break;

                    //

                    default:
                        bProcessed = false;
                        //
                        //if (tmp_ntohAudioPlayCfg(pItem, &pTranAudio->compressor.playCfg, &bProcessed)) goto errLabel;
                        if (bProcessed)
                        {
                            iErr = 0; goto errLabel;
                        }

                        break;
                }
            }

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                if (null != pbProcessed) *pbProcessed = bProcessed;
            }
            return iErr;
        }



        /////////////////////
        public static unsafe int tmp_htonConfHgInfo(CONF_hg_info* pReq_hgInfo, byte** ppBuf, uint* uiBufSize)
        {
            int iErr = -1;

            byte* ptr = *ppBuf;
            uint len = *uiBufSize;

#if false  //10
	 if (pReq_hgInfo->ui64MeetingId_hg) {
		 if (data2Stream(CONST_qyDataType_l64, CONST_qnmCfgId_meetingId_hg, (void*)&pReq_hgInfo->ui64MeetingId_hg, sizeof(pReq_hgInfo->ui64MeetingId_hg), &ptr, &len))  goto  errLabel;
	 }

	 //
	 if (pReq_hgInfo->iMeetingType_hg) {
		 if (data2Stream(CONST_qyDataType_long, CONST_qnmCfgId_meetingType_hg, (void*)pReq_hgInfo->iMeetingType_hg, 0, &ptr, &len))goto  errLabel;
	 }

	 //
	 if  (  pReq_hgInfo->iMeetingLevel_hg  )  {
		 if  (  data2Stream(CONST_qyDataType_long, CONST_qnmCfgId_meetingLevel_hg, (void*)pReq_hgInfo->iMeetingLevel_hg,0,&ptr,&len))goto  errLabel;
	 }
	 if  (  pReq_hgInfo->iMeetingDurationInMin  )  {
		 if  (  data2Stream(  CONST_qyDataType_long,  CONST_qnmCfgId_meetingDuration_hg,(void*)pReq_hgInfo->iMeetingDurationInMin,0,&ptr,&len))goto  errLabel;
	 }

	 //
	 if  (  pReq_hgInfo->ui64_meetingStartTime  )  {
		 if  (  data2Stream(  CONST_qyDataType_l64,  CONST_qnmCfgId_meetingStartTime_hg,(void*)&pReq_hgInfo->ui64_meetingStartTime, sizeof(pReq_hgInfo->ui64_meetingStartTime),&ptr,&len))goto  errLabel;
	 }
	 if  (  pReq_hgInfo->meetingCompere[0]  )  {
		 if  (  data2Stream(  CONST_qyDataType_wStr,  CONST_qnmCfgId_meetingCompere_hg,(void*)pReq_hgInfo->meetingCompere,lstrlen(pReq_hgInfo->meetingCompere),&ptr,&len))goto  errLabel;
	 }

	 //
	 if (pReq_hgInfo->bRecord) {
		 if (data2Stream(CONST_qyDataType_char, CONST_qnmCfgId_meetingRecord_hg, (void*)pReq_hgInfo->bRecord, 0, &ptr, &len))goto  errLabel;
	 }
	 if (pReq_hgInfo->bShare_2ndStream) {
		 if (data2Stream(CONST_qyDataType_char, CONST_qnmCfgId_meetingShare_2ndStream_hg, (void*)pReq_hgInfo->bShare_2ndStream, 0, &ptr, &len))  goto  errLabel;
	 }
	 int i;
	 for (i = 0; i < mycountof(pReq_hgInfo->shareMems); i++) {
		 if (!pReq_hgInfo->shareMems[i].idInfo.ui64Id) {
			 break;
		 }
		 if (data2Stream(CONST_qyDataType_l64, CONST_qnmCfgId_shareMem_hg, (void*)&pReq_hgInfo->shareMems[i].idInfo.ui64Id, sizeof(pReq_hgInfo->shareMems[i].idInfo.ui64Id), &ptr, &len))goto  errLabel;
	 }
#endif

            //
            if (pReq_hgInfo->bFlow512k)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_bFlow512k_hg, (void*)(pReq_hgInfo->bFlow512k ? 1 : 0), 0, (byte**)&ptr, (uint*)&len)) goto errLabel;
            }

            //
            iErr = 0;
        errLabel:

            if (0 == iErr)
            {
                *ppBuf = ptr;
                *uiBufSize = len;
            }


            return iErr;
        }

        //
        public static unsafe int tmp_ntohConfHgInfo(QY_CFGITEM_ntoh_U* pItem, CONF_hg_info* pContent, bool* pbProcessed)
        {
            int iErr = -1;

            bool bProcessed = false;

            if (null == pContent) goto errLabel;

            short tmp_cfgId;
            short* p_tmp_cfgId = &tmp_cfgId;
            short* p_cfgId = (short*)&pItem->head.cfgId;
            qyFuncs.mymemcpy((IntPtr)p_tmp_cfgId, (System.IntPtr)p_cfgId, sizeof(short));

            //
            bProcessed = true;

            switch (tmp_cfgId)
            {
                /*

                case CONST_qnmCfgId_meetingId_hg:
                    if (getFieldData_l64(pItem, (__int64*)&pContent->ui64MeetingId_hg)) goto errLabel;
                    //
                    if (1)
                    {
                        TCHAR tBuf[128];
                        _sntprintf(tBuf, mycountof(tBuf), _T("stream2ConfReq: meetingId_hg %I64u"), pContent->ui64MeetingId_hg);
                        showInfo_open(0, 0, tBuf);
                    }

                    //
                    break;
                //
                case CONST_qnmCfgId_meetingType_hg:
                    if (getFieldData_long(pItem, (long*)&pContent->iMeetingType_hg)) goto errLabel;
                    break;
                //
                case CONST_qnmCfgId_meetingLevel_hg:
                    if (getFieldData_long(pItem, (long*)&pContent->iMeetingLevel_hg)) goto errLabel;
                    break;
                case CONST_qnmCfgId_meetingDuration_hg:
                    if (getFieldData_long(pItem, (long*)&pContent->iMeetingDurationInMin)) goto errLabel;
                    break;
                case CONST_qnmCfgId_meetingStartTime_hg:
                    if (getFieldData_l64(pItem, (__int64*)&pContent->ui64_meetingStartTime)) goto errLabel;
                    break;
                case CONST_qnmCfgId_meetingCompere_hg:
                    if (getFieldData_wStr(pItem, pContent->meetingCompere, mycountof(pContent->meetingCompere))) goto errLabel;

                    break;
                //
                case CONST_qnmCfgId_meetingRecord_hg:
                    {
                        char tmp_c = 0;
                        if (getFieldData_char(pItem, (char*)&tmp_c)) goto errLabel;
                        pContent->bRecord = tmp_c;
                    }
                    break;
                case CONST_qnmCfgId_meetingShare_2ndStream_hg:
                    {
                        char tmp_c = 0;
                        if (getFieldData_char(pItem, (char*)&tmp_c)) goto errLabel;
                        pContent->bShare_2ndStream = tmp_c;
                    }
                    break;
                case CONST_qnmCfgId_shareMem_hg:
                    if (pContent->tmpInternal.cnt_shareMems >= mycountof(pContent->shareMems)) goto errLabel;
                    if (getFieldData_l64(pItem, (__int64*)&pContent->shareMems[pContent->tmpInternal.cnt_shareMems].idInfo.ui64Id)) goto errLabel;
                    pContent->tmpInternal.cnt_shareMems++;
                    break;
				*/
                case Consts.CONST_qnmCfgId_bFlow512k_hg:
                    {
                        byte tmp_c = (byte)0;
                        if (0 != qyFuncs.getFieldData_byte(pItem, (byte*)&tmp_c)) goto errLabel;
                        pContent->bFlow512k = (tmp_c != 0) ? true : false;
                    }
                    break;
                default:
                    break;
            }

            iErr = 0;
        errLabel:


            return iErr;
        }



        //////////////////
        public static unsafe int tmp_htonTranConfCfg(TRAN_conf_cfg* pConfCfg, byte** ppBuf, int* puiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = *ppBuf;
            uint len = (uint)*puiBufSize;

            if (null != pConfCfg)
            {
                //  2014/09/23
                TRAN_conf_cfg* pHead = pConfCfg;

                //
                //
                if (0 != pHead->usMaxSpeakers)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usMaxSpeakers, (void*)pHead->usMaxSpeakers, 0, &ptr, &len)) goto errLabel;
                }

                //
                if (0 != pHead->conf.usMaxFps_toShareBmp_conf)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usMaxFps_toShareBmp_conf, (void*)pHead->conf.usMaxFps_toShareBmp_conf, 0, &ptr, &len)) goto errLabel;
                }
                //  2014/08/15
                if (0 != pHead->conf.iWidth_pic_conf)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iWidth_pic_conf, (void*)pHead->conf.iWidth_pic_conf, 0, &ptr, &len)) goto errLabel;
                }
                if (0 != pHead->conf.iHeight_pic_conf)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_iHeight_pic_conf, (void*)pHead->conf.iHeight_pic_conf, 0, &ptr, &len)) goto errLabel;

                }

                //
                if (0 != pHead->conf.conf_iFourcc)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_conf_iFourcc, (void*)pHead->conf.conf_iFourcc, 0, &ptr, &len)) goto errLabel;
                }


                //  2016/03/26

                //  2017/07/06
                if (0 != pHead->conf.confCtrl.ucbConfCtrl_starter)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbConfCtrl_starter, (void*)pHead->conf.confCtrl.ucbConfCtrl_starter, 0, &ptr, &len)) goto errLabel;
                }

                //  2017/08/02
                if (0 != pHead->ucbVideoConference)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbVideoConference, (void*)pHead->ucbVideoConference, 0, &ptr, (uint*)&len)) goto errLabel;
                }
                //  2017/08/25
                if (0 != pHead->usConfType)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usConfType, (void*)pHead->usConfType, 0, &ptr, &len)) goto errLabel;
                }

                //  2017/07/17
#if false
				if (0 != pHead->ucbAvConsole)
				{
					if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbAvConsole, (void*)pHead->ucbAvConsole, 0, &ptr, &len)) goto errLabel;
				}
#endif
                //
                //
                if (0 != pHead->idInfo_initiator.ui64Id)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_initiator, &pHead->idInfo_initiator.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
                }


                //  2017/08/26
                //
                if (0 != tmp_htonConfHgInfo(&pHead->hgInfo, &ptr, &len)) goto errLabel;

                //
            }

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *ppBuf = ptr;
                *puiBufSize = (int)len;
            }
            return iErr;

        }


        public static unsafe int tmp_ntohTranConfCfg(QY_CFGITEM_ntoh_U* pItem, TRAN_conf_cfg* pConfCfg, bool* pbProcessed)
        {
            int iErr = -1;
            int tmpLen;
            //char buf[sizeof(QY_VIDEO_HEADER) + 1024];
            bool bProcessed = false;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            if (null != pConfCfg)
            {
                TRAN_conf_cfg* pHead = pConfCfg;
                //
                bProcessed = true;
                //
                switch (tmp_cfgId)
                {
                    //
                    case Consts.CONST_qnmCfgId_usMaxSpeakers:  //  2009/05/31
                        if (0 != getFieldData_short(pItem, (short*)&pHead->usMaxSpeakers)) goto errLabel;
                        break;

                    case Consts.CONST_qnmCfgId_usMaxFps_toShareBmp_conf:
                        if (0 != getFieldData_short(pItem, (short*)&pHead->conf.usMaxFps_toShareBmp_conf)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_iWidth_pic_conf:    //  2014/08/15
                        if (0 != getFieldData_long(pItem, (int*)&pHead->conf.iWidth_pic_conf)) goto errLabel;
                        break;
                    case Consts.CONST_qnmCfgId_iHeight_pic_conf:   //  2014/08/15
                        if (0 != getFieldData_long(pItem, (int*)&pHead->conf.iHeight_pic_conf)) goto errLabel;
                        break;
                    //

                    case Consts.CONST_qnmCfgId_conf_iFourcc:
                        if (0 != getFieldData_long(pItem, (int*)&pHead->conf.conf_iFourcc)) goto errLabel;
                        break;

                    //
                    case Consts.CONST_qnmCfgId_ucbConfCtrl_starter:
                        if (0 != getFieldData_byte(pItem, (byte*)&pHead->conf.confCtrl.ucbConfCtrl_starter)) goto errLabel;
                        break;
                    //
                    case Consts.CONST_qnmCfgId_ucbVideoConference:  //  2017/08/02
                        if (0 != getFieldData_byte(pItem, (byte*)&pHead->ucbVideoConference)) goto errLabel;
                        break;
                    //
                    case Consts.CONST_qnmCfgId_usConfType: //  2017/08/25
                        if (0 != getFieldData_short(pItem, (short*)&pHead->usConfType)) goto errLabel;
                        break;
                    //
#if false
					case Consts.CONST_qnmCfgId_ucbAvConsole:  //  2017/07/17
				if (0!=getFieldData_byte(pItem, (byte*)&pHead->ucbAvConsole)) goto errLabel;
				break;
#endif
                    //
                    //
                    case Consts.CONST_qnmCfgId_idInfo_initiator:
                        if (0 != getFieldData_l64(pItem, (Int64*)&pHead->idInfo_initiator.ui64Id)) goto errLabel;
                        int ii = (int)pHead->idInfo_initiator.ui64Id;
                        int ss = 0;
                        break;

                    //
                    default:
                        bProcessed = false;

                        tmp_ntohConfHgInfo(pItem, &pHead->hgInfo, &bProcessed);

                        if (bProcessed)
                        {
                            iErr = 0; goto errLabel;
                        }

                        break;
                }
            }


            iErr = 0;
        errLabel:

            if (0 == iErr)
            {
                if (null != pbProcessed) *pbProcessed = bProcessed;
            }

            return iErr;
        }



        ///////////////////


        //
        public static unsafe int tmp_htonAvTranInfo(AV_TRAN_INFO* pTranInfo, byte** ppBuf, int* puiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = *ppBuf;
            uint len = (uint)*puiBufSize;

            //
            AV_tranInfo_head* pHead = &pTranInfo->head;
            TRAN_audio* pTranAudio = &pTranInfo->audio;
            TRAN_video* pTranVideo = &pTranInfo->video;


            //
            if (null != pHead)
            {
                //  2014/09/23
                if (0 != pHead->shClientVer)
                {
                    if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_shVer, (void*)pHead->shClientVer, 0, &ptr, (uint*)&len)) goto errLabel;
                }

            }


            //  2014/11/07
            if (null != pTranVideo)
            {
                if (0 != pTranVideo->uiTranNo_openAvDev)
                {
                    if (0 != tmp_htonTranVideo(pTranVideo, &ptr, &len)) goto errLabel;
                }
            }

            //  2014/11/07
            if (null != pTranAudio)
            {
                if (0 != pTranAudio->uiTranNo_openAvDev)
                {
                    if (0 != tmp_htonTranAudio(pTranAudio, &ptr, &len)) goto errLabel;
                }
            }

            //  2015/03/12
            if (0 != pTranInfo->taskAvProps.v.ucAvFlg)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucAvFlg, (void*)pTranInfo->taskAvProps.v.ucAvFlg, 0, &ptr, &len)) goto errLabel;
            }

            //
            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *ppBuf = ptr;
                *puiBufSize = (int)len;
            }
            return iErr;

        }

        //
        public static unsafe int tmp_ntohAvTranInfo(QY_CFGITEM_ntoh_U* pItem, AV_TRAN_INFO* pTranInfo, bool* pbProcessed)
        {
            int iErr = -1;
            int tmpLen;
            //char buf[sizeof(QY_VIDEO_HEADER) + 1024];
            bool bProcessed = false;

            //	 
            AV_tranInfo_head* pHead = &pTranInfo->head;
            TRAN_audio* pTranAudio = &pTranInfo->audio;
            TRAN_video* pTranVideo = &pTranInfo->video;


            //
            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            if (null != pHead)
            {
                switch (tmp_cfgId)
                {
                    case Consts.CONST_qnmCfgId_shVer:  //  2014/09/23
                        if (0 != getFieldData_short(pItem, (short*)&pHead->shClientVer)) goto errLabel;
                        break;
                    default:
                        break;
                }
            }

            //
            if (null != pTranVideo)
            {
                //
                bProcessed = false;
                if (0 != tmp_ntohTranVideo(pItem, pTranVideo, &bProcessed))
                {
                    goto errLabel;
                }
                if (bProcessed)
                {
                    iErr = 0; goto errLabel;
                }
            }


            if (null != pTranAudio)
            {
                //
                bProcessed = false;
                if (0 != tmp_ntohTranAudio(pItem, pTranAudio, &bProcessed))
                {
                    goto errLabel;
                }
                if (bProcessed)
                {
                    iErr = 0; goto errLabel;
                }
            }

            //  2015/03/12
            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_ucAvFlg:
                    if (0 != getFieldData_byte(pItem, (byte*)&pTranInfo->taskAvProps.v.ucAvFlg)) goto errLabel;
                    break;
                default:
                    break;
            }


            iErr = 0;
        errLabel:

            if (null != pbProcessed) *pbProcessed = bProcessed;

            return iErr;
        }


        //
        public static unsafe int tmp_htonAvStream(AV_stream* pObj, byte** ppBuf, uint* puiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = *ppBuf;
            uint len = *puiBufSize;

            //
            if (0 == pObj->idInfo.ui64Id
                && 0 == pObj->obj.resObj.uiObjType
                && 0 == pObj->obj.tranInfo.audio.uiTranNo_openAvDev
                && 0 == pObj->obj.tranInfo.video.uiTranNo_openAvDev)
            {
                return 0;
            }

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_avStream, 0, &ptr, &len)) goto errLabel;

            //
            if (0 != pObj->idInfo.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pObj->idInfo.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }

            //	
            if (0 != pObj->obj.resObj.uiObjType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pObj->obj.resObj.uiObjType, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pObj->obj.resObj.usIndex_obj)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_index, (void*)pObj->obj.resObj.usIndex_obj, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != tmp_htonAvTranInfo(&pObj->obj.tranInfo, &ptr, (int*)&len)) goto errLabel;

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            //
            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *ppBuf = ptr;
                *puiBufSize = len;
            }
            return iErr;

        }

        //
        public static unsafe int tmp_ntohAvStream(QY_CFGITEM_ntoh_U* pItem, AV_stream* pObj, bool* pbProcessed)
        {
            int iErr = -1;
            int tmpLen;
            //char						buf[sizeof(  QY_VIDEO_HEADER  )  +  1024];
            bool bProcessed = false;

            //
            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            //
            bProcessed = true;
            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_messengerId:
                    if (0 != getFieldData_l64(pItem, (Int64*)&pObj->idInfo.ui64Id)) goto errLabel;
                    break;
                case Consts.CONST_qnmCfgId_uiObjType:
                    int iVal;
                    if (0 != getFieldData_long(pItem, (int*)&iVal)) goto errLabel;
                    //
                    pObj->obj.resObj.uiObjType = (uint)iVal;
                    break;
                case Consts.CONST_qnmCfgId_index:
                    short sVal;
                    if (0 != getFieldData_short(pItem, (short*)&sVal)) goto errLabel;
                    //
                    pObj->obj.resObj.usIndex_obj = (ushort)sVal;
                    break;
                default:
                    bProcessed = false;
                    if (0 != tmp_ntohAvTranInfo(pItem, &pObj->obj.tranInfo, &bProcessed)) goto errLabel;
                    //
                    if (bProcessed)
                    {
                        iErr = 0; goto errLabel;
                    }
                    break;
            }

            iErr = 0;
        errLabel:

            if (null != pbProcessed) *pbProcessed = bProcessed;

            return iErr;
        }




        //
        public static unsafe int transferAvInfo2Stream(uint uiStreamId, TRANSFER_AV_INFO* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;


            if (null == pReq) return -1;
            if (null == buf) return -1;

#if DEBUG
            ref TRANSFER_AV_INFO tmpReq = ref *pReq;
#endif


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->ucbResp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbResp, (void*)pReq->ucbResp, 0, &ptr, &len)) goto errLabel;
            }

            if (0 != pReq->uiTaskType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTaskType, (void*)pReq->uiTaskType, 0, &ptr, &len)) goto errLabel;
            }

            //  2014/11/07
            if (0 != tmp_htonTranConfCfg(&pReq->confCfg, &ptr, (int*)&len)) goto errLabel;

            //
            AV_stream tmpAs;
            mymemset((IntPtr)(byte*)&tmpAs, 0, sizeof(AV_stream));
            tmpAs.obj = pReq->ass;
            if (0 != tmp_htonAvStream(&tmpAs, &ptr, &len)) goto errLabel;

            //
            //if (0 != tmp_htonAvTranInfo(&pReq->ass.tranInfo, &ptr, (int*)&len)) goto errLabel;

            //  2014/11/07
            for (i = 0; i < 2; i++)
            {
                MOSAIC_TRAN_INFO* pMosaic = null;
                switch (i)
                {
                    case 0:
                        pMosaic = &pReq->confMosaicTranInfo_video;
                        break;
                    case 1:
                        pMosaic = &pReq->confMosaicTranInfo_resource;
                        break;
                    default:
                        break;
                }
                if (null == pMosaic) break;
                //
                if (0 != pMosaic->resObj.uiObjType && 0 != pMosaic->video.uiTranNo_openAvDev)
                {

#if false
					if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_struct, 0, &ptr, &len)) goto errLabel;

					//
					if (0!=pMosaic->resObj.uiObjType)
					{
						if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pMosaic->resObj.uiObjType, 0, &ptr, &len)) goto errLabel;
					}
					if (0!=pMosaic->usIndex_obj)
					{
						if (0!=data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_index, (void*)pMosaic->usIndex_obj, 0, &ptr, &len)) goto errLabel;
					}
					//
					if (0!=tmp_htonTranVideo(&pMosaic->video, &ptr, &len)) goto errLabel;

					//
					if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
#endif
                    //
                    mymemset((IntPtr)(byte*)&tmpAs, 0, sizeof(AV_stream));
                    tmpAs.obj.resObj = pMosaic->resObj;
                    tmpAs.obj.tranInfo.video = pMosaic->video;
                    //
                    if (0 != tmp_htonAvStream(&tmpAs, &ptr, &len))
                    {
                        goto errLabel;
                    }

                }
            }

#if false
			//  2009/09/07
			if (0!=pReq->viewDynBmp.resObj.uiObjType)
			{
				if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pReq->viewDynBmp.resObj.uiObjType, 0, &ptr, &len)) goto errLabel;
			}
			if (0!=pReq->viewDynBmp.resObj.usIndex_obj)
			{
				if (0!=data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_index, (void*)pReq->viewDynBmp.resObj.usIndex_obj, 0, &ptr, &len)) goto errLabel;
			}
			if (0 != pReq->viewDynBmp.resObj.usHelp_subIndex)
			{   //  2016/07/24
				if (0!=data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_help_subIndex, (void*)pReq->viewDynBmp.resObj.usHelp_subIndex, 0, &ptr, &len)) goto errLabel;
			}
#endif
            //  2014/08/04
            if (0 != pReq->viewDynBmp.ucbSaveVideo)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbSaveVideo, (void*)pReq->viewDynBmp.ucbSaveVideo, 0, &ptr, &len)) goto errLabel;
            }


            //  2009/10/18
            if (0 != pReq->idInfo_logicalPeer.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_logicalPeer, (void*)&pReq->idInfo_logicalPeer.ui64Id, sizeof(UInt64), &ptr, &len)) goto errLabel;
            }
            //  2014/11/22
            if (0 != pReq->uiTranNo_avTask_starter)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_avTask_starter, (void*)pReq->uiTranNo_avTask_starter, 0, &ptr, &len)) goto errLabel;
            }


#if false
			//  2014/11/20
			if (0!=pReq->proxiedTranInfo.idInfo.ui64Id && 0!=pReq->proxiedTranInfo.uiObjType)
			{
				if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)Consts.CONST_imCommType_struct, 0, &ptr, &len)) goto errLabel;

				PROXIED_TRAN_INFO* pProxied = &pReq->proxiedTranInfo;

				if (0!=pProxied->idInfo.ui64Id)
				{
					if (0!=data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pProxied->idInfo.ui64Id, sizeof(UInt64),  &ptr,  &len  )  )  goto errLabel;
				}

				//
				if (0!=pProxied->uiObjType)
				{
					if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pProxied->uiObjType, 0, &ptr, &len)) goto errLabel;
				}
				//		
				if (0!=tmp_htonAvTranInfo(&pProxied->ti, &ptr, (int*)&len)) goto errLabel;

				//		
				if (0!=data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;
			}
#endif

            //  qyShowInfo1(  Consts.CONST_qyShowType_debug,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    transferAvInfo2stream: uiTranNo_openAvDev %d"  ),  pReq->uiTranNo_openAvDev  );

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            //
            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
#if __DEBUG__
			if (iErr)
			{
				int i;
				i = 0;
			}
#endif
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferAvInfo(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_AV_INFO* pContent = (TRANSFER_AV_INFO*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  int						tmpLen;
            //  char						buf[sizeof(  QY_VIDEO_HEADER  )  +  1024];

            if (null == pContent) goto errLabel;

            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_transferAvInfo:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_null:
                            {
#if DEBUG
                                ref TRANSFER_AV_INFO tmpReq = ref *pContent;
#endif
                                int ii = 0;
                            }
                            break;
                        case Consts.CONST_qnmCfgId_ucbResp:
                            if (0 != getFieldData_byte(pItem, (byte*)&pContent->ucbResp)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiTaskType:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTaskType)) goto errLabel;
                            break;
#if false
						case Consts.CONST_qnmCfgId_uiObjType:
							if (0!=getFieldData_long(pItem, (int*)&pContent->viewDynBmp.resObj.uiObjType)) goto errLabel;
							break;
						case Consts.CONST_qnmCfgId_index:
							if (0!=getFieldData_short(pItem, (short*)&pContent->viewDynBmp.resObj.usIndex_obj)) goto errLabel;
							break;
						case Consts.CONST_qnmCfgId_help_subIndex:  //  2016/07/24
							if (0!=getFieldData_short(pItem, (short*)&pContent->viewDynBmp.resObj.usHelp_subIndex)) goto errLabel;
							break;
#endif
                        case Consts.CONST_qnmCfgId_ucbSaveVideo:   //  2014/08/04				   
                            if (0 != getFieldData_byte(pItem, (byte*)&pContent->viewDynBmp.ucbSaveVideo)) goto errLabel;
                            break;
                        //			 
                        case Consts.CONST_qnmCfgId_idInfo_logicalPeer:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->idInfo_logicalPeer.ui64Id)) goto errLabel;
                            break;
                        //
                        case Consts.CONST_qnmCfgId_uiTranNo_avTask_starter:  //  2014/11/22
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_avTask_starter)) goto errLabel;
                            break;
                        //
                        default:
                            //				    
                            bool bProcessed = false;
                            //					
                            if (0 != tmp_ntohTranConfCfg(pItem, &pContent->confCfg, &bProcessed))
                            {
                                goto errLabel;
                            }
                            if (bProcessed) break;
                            //				    
#if false
							if (0 != tmp_ntohAvTranInfo(pItem, &pContent->tranInfo, &bProcessed))
							{
								goto errLabel;
							}
							if (bProcessed) break;
#endif
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_avStream:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            mymemset((IntPtr)(byte*)&pContent->tmp_internal_tmpAs, 0, sizeof(AV_stream));
                            break;
                        case Consts.CONST_qnmCfgId_null:
                            switch (pContent->tmp_internal_tmpAs.obj.resObj.uiObjType)
                            {
                                case Consts.CONST_objType_mosaicStream_video:
                                    pContent->confMosaicTranInfo_video.resObj = pContent->tmp_internal_tmpAs.obj.resObj;
                                    pContent->confMosaicTranInfo_video.video = pContent->tmp_internal_tmpAs.obj.tranInfo.video;
                                    break;
                                case Consts.CONST_objType_mosaicStream_resource:
                                    pContent->confMosaicTranInfo_resource.resObj = pContent->tmp_internal_tmpAs.obj.resObj;
                                    pContent->confMosaicTranInfo_resource.video = pContent->tmp_internal_tmpAs.obj.tranInfo.video;
                                    break;
                                default:
                                    pContent->ass = pContent->tmp_internal_tmpAs.obj;
                                    break;
                            }
                            break;
                        default:
                            bool bProcessed = false;
                            if (0 != tmp_ntohAvStream(pItem, &pContent->tmp_internal_tmpAs, &bProcessed))
                            {
                                goto errLabel;
                            }
                            if (bProcessed)
                            {
                                iErr = 0; goto errLabel;
                            }
                            break;
                    }
                    break;

                    break;
                default:
                    break;
            }



            iErr = 0;
        errLabel:

#if __DEBUG__
			if (iErr)
			{
				int i;
				i = 0;
			}
#endif
            return iErr;
        }



        //  2011/12/24
        public static unsafe int transferAvReplyInfo2Stream(uint uiStreamId, TRANSFER_AV_replyInfo* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;

            if (null == pReq) return -1;
            if (null == buf) return -1;

            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;

            if (0 != pReq->usOp)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_usOp, (void*)pReq->usOp, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->tStartTime_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_tStartTime_org, &pReq->tStartTime_org, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->uiTranNo_org)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiTranNo_org, (void*)pReq->uiTranNo_org, 0, &ptr, &len)) goto errLabel;
            }

            //
            AV_stream tmpAs;
            mymemset((IntPtr)(byte*)&tmpAs, 0, sizeof(AV_stream));
            tmpAs.obj.tranInfo = pReq->tranInfo;
            if (0 != tmp_htonAvStream(&tmpAs, &ptr, (uint*)&len)) goto errLabel;

            //  2014/09/04
            if (0 != pReq->viewDynBmpReply.ucbSaveVideo)
            {   //  2014/09/07
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_ucbSaveVideo, (void*)pReq->viewDynBmpReply.ucbSaveVideo, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;

        }


        public static unsafe int tmpHandler_stream2TransferAvReplyInfo(CTX_stream2Data* pCtx, void* p0, void* p1, uint uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            TRANSFER_AV_replyInfo* pContent = (TRANSFER_AV_replyInfo*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;
            int tmpLen;
            //char buf[sizeof(QY_VIDEO_HEADER) + 1024];
            bool bProcessed = false;

            if (null == pContent) goto errLabel;


            ushort tmp_cfgId;
            //memcpy(&tmp_cfgId, &pItem->head.cfgId, sizeof(short));
            tmp_cfgId = pItem->head.cfgId;

            //
            switch (uiStreamId)
            {
                case Consts.CONST_imCommType_transferAvReplyInfo:
                    //
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            pContent->uiType = uiStreamId;
                            break;
                        case Consts.CONST_qnmCfgId_usOp:
                            if (0 != getFieldData_short(pItem, (short*)&pContent->usOp)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_tStartTime_org:
                            if (0 != getFieldData_l64(pItem, (Int64*)&pContent->tStartTime_org)) goto errLabel;
                            break;
                        case Consts.CONST_qnmCfgId_uiTranNo_org:
                            if (0 != getFieldData_long(pItem, (int*)&pContent->uiTranNo_org)) goto errLabel;
                            break;

                        //  2014/09/04
                        case Consts.CONST_qnmCfgId_ucbSaveVideo:   //  2014/09/07
                            if (0 != getFieldData_byte(pItem, (byte*)&pContent->viewDynBmpReply.ucbSaveVideo)) goto errLabel;
                            break;

                        default:
#if false
							if (0 != tmp_ntohAvTranInfo(pItem, &pContent->tranInfo, &bProcessed)) goto errLabel;
							if (bProcessed)
							{
								iErr = 0; goto errLabel;
							}
#endif
                            break;
                    }
                    break;
                case Consts.CONST_imCommType_avStream:
                    switch (tmp_cfgId)
                    {
                        case Consts.CONST_qnmCfgId_start:
                            mymemset((IntPtr)(byte*)&pContent->tmp_internal.tmpAs, 0, sizeof(AV_stream));
                            break;
                        case Consts.CONST_qnmCfgId_null:
                            pContent->tranInfo = pContent->tmp_internal.tmpAs.obj.tranInfo;
                            break;
                        default:
                            if (0 != tmp_ntohAvStream(pItem, &pContent->tmp_internal.tmpAs, &bProcessed)) goto errLabel;
                            if (bProcessed)
                            {
                                iErr = 0; goto errLabel;
                            }
                            break;
                    }
                    break;

                default:
                    break;
            }


            iErr = 0;
        errLabel:
            return iErr;
        }


        public static unsafe int qyhtonRouteTalkData(MSG_ROUTE* pAddr, byte* pContentParam, uint lenInBytes_content, bool bNeedContentConvrted, byte ucFlg, byte* buf, uint* puiBufSize, char* tHintBuf_showInfo, uint uiCnt_tHintBuf_showInfo)
        {
            int iErr = -1;
            IM_CONTENTU* pContent = (IM_CONTENTU*)pContentParam;
            int lenInBytes = 0;
            byte* ptr;
            //  IM_L_STREAM			*		pStream									=	NULL;
            uint uiBufSize;
            int lenInBytes_addr = 0;
            //  long						tmp_l;


            if (null == buf || null == puiBufSize) return -1;
            uiBufSize = *puiBufSize;

            if (isUcFlgRouteTalkData(ucFlg))
            {

                lenInBytes_addr = (int)uiBufSize;
                if (0 != msgRoute2Stream(Consts.CONST_imCommType_msgRoute, pAddr, buf, (uint*)&lenInBytes_addr))
                {
                    goto errLabel;
                }
            }
            else if (isUcFlgTalkData(ucFlg))
            {       //  仅用来talkData
                if (null == pAddr || uiBufSize < sizeof(MSG_SIMPLE_ROUTE))
                {
                    goto errLabel;
                }
                MSG_SIMPLE_ROUTE talkDataAddr;
                MSG_SIMPLE_ROUTE* pTalkDataAddr = &talkDataAddr;
                //  pTalkDataAddr  =  (  MSG_SIMPLE_ROUTE  *  )buf;
                pTalkDataAddr->idInfo_from.ui64Id = pAddr->idInfo_from.ui64Id;
                pTalkDataAddr->idInfo_to.ui64Id = pAddr->idInfo_to.ui64Id;
                //
                MACRO_htonl64(ref pTalkDataAddr->idInfo_from.ui64Id);
                MACRO_htonl64(ref pTalkDataAddr->idInfo_to.ui64Id);
                //
                mymemcpy((IntPtr)buf, (IntPtr)pTalkDataAddr, (uint)sizeof(MSG_SIMPLE_ROUTE));
                //
                lenInBytes_addr = sizeof(MSG_SIMPLE_ROUTE);
            }


            //
            ptr = buf + lenInBytes_addr;

            //  要求不转换的		  
            //
            if (!bNeedContentConvrted)
            {

                if (lenInBytes_content > uiBufSize - lenInBytes_addr)
                {
                    traceLogA("lenInBytes_content is too large");
                    goto errLabel;
                }
                if (null == pContent) lenInBytes = lenInBytes_addr;
                else
                {
                    mymemcpy((IntPtr)ptr, (IntPtr)pContent, lenInBytes_content);
                    lenInBytes = (int)(lenInBytes_addr + lenInBytes_content);
                }

                iErr = 0; goto errLabel;
            }

            if (null == pContent)
            {
                goto errLabel;
            }


            switch (pContent->uiType)
            {
                case 0:
                    {
                        uint uiType = pContent->uiType;
                        MACRO_htoni(ref uiType);
                        lenInBytes = sizeof(uint);
                        if (uiBufSize - lenInBytes_addr < lenInBytes)
                        {
                            goto errLabel;
                        }
                        mymemcpy((IntPtr)ptr, (IntPtr)(byte*)&uiType, (uint)lenInBytes);
                        lenInBytes += lenInBytes_addr;
                        iErr = 0; goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_simpleResp:
                    {
                        IM_SIMPLE_RESP simpleResp;
                        mymemcpy((IntPtr)(byte*)&simpleResp, (IntPtr)(byte*)&pContent->simpleResp, (uint)sizeof(IM_SIMPLE_RESP));
                        if ((lenInBytes = qyhtonImSimpleResp(0, &simpleResp)) < 0)
                        {
                            goto errLabel;
                        }
                        if (uiBufSize - lenInBytes_addr < lenInBytes)
                        {
                            goto errLabel;
                        }
                        mymemcpy((IntPtr)ptr, (IntPtr)(byte*)&simpleResp, (uint)lenInBytes);
                        lenInBytes += lenInBytes_addr;
                        iErr = 0; goto errLabel;
                    }
                    break;
                default:
                    break;
            }


            //
            IM_L_STREAM_nh stream_hton;
            byte* stream_pBuf = null;
            //  pStream  =  (  IM_L_STREAM  *  )ptr;

            if (uiBufSize - lenInBytes_addr < (int)Marshal.OffsetOf<IM_L_STREAM>("buf"))
            {
                traceLogA("qyhtonRouteTalkData failed: buf is too small");
                goto errLabel;  //  怕空间不够了
            }
            mymemset((IntPtr)(byte*)&stream_hton, 0, (int)Marshal.OffsetOf<IM_L_STREAM>("buf"));
            stream_hton.uiType = Consts.CONST_imCommType_lStream;
            stream_hton.uiContentType = pContent->uiType;
            stream_pBuf = ptr + (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
            lenInBytes = (int)(uiBufSize - lenInBytes_addr);       //  sizeof(  pStream->buf  );				
            switch (pContent->uiType)
            {

                case Consts.CONST_imCommType_htmlContent:
                    if (0 != imHtmlContent2Stream(pContent->uiType, &pContent->html, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_transferFileReq:
                    if (0 != transferFileReq2Stream(pContent->uiType, &pContent->transferFileReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_taskProcReq:
                    if (0 != taskProcReq2Stream(pContent->uiType, &pContent->taskProcReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_taskProcResp:
                    if (0 != taskProcResp2Stream(pContent->uiType, &pContent->taskProcResp, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_transferFileReplyReq:
                    if (0 != transferFileReplyReq2Stream(pContent->uiType, &pContent->transferFileReplyReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_transferFileDataReq:
                    if (0 != transferFileDataReq2Stream(pContent->uiType, &pContent->transferFileDataReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_transferFileDataResp:
                    if (0 != transferFileDataResp2Stream(pContent->uiType, &pContent->transferFileDataResp, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_transferFileEndReq:
                    if (0 != transferFileEndReq2Stream(pContent->uiType, &pContent->transferFileEndReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_transferFileEndResp:
                    if (0 != transferFileEndResp2Stream(pContent->uiType, &pContent->transferFileEndResp, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                /*
            case Consts.CONST_imCommType_rtcCallReq:
                if (rtcCallReq2Stream(pContent->uiType, &pContent->rtcCallReq, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            */
                case Consts.CONST_imCommType_transferAvInfo:
                    if (0 != transferAvInfo2Stream(pContent->uiType, &pContent->transferAvInfo, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_transferAvReplyInfo:
                    if (0 != transferAvReplyInfo2Stream(pContent->uiType, &pContent->transferAvReplyInfo, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_transferVideoData:
                    if (0 != transferVideoData2Stream(pContent->uiType, &pContent->transferVideoData, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_transferVideoDataResp:
                    if (0 != transferVideoDataResp2Stream(pContent->uiType, &pContent->transferVideoDataResp, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_transferAudioData:
                    if (0 != transferAudioData2Stream(pContent->uiType, &pContent->transferAudioData, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                case Consts.CONST_imCommType_transferAudioDataResp:
                    if (0 != transferAudioDataResp2Stream(pContent->uiType, &pContent->transferAudioDataResp, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                /*
            case Consts.CONST_imCommType_remoteAssistReq:
                if (remoteAssistReq2Stream(pContent->uiType, &pContent->remoteAssistReq, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
                */
                case Consts.CONST_imCommType_taskInteractionReq:
                    if (0 != taskInteractionReq2Stream(pContent->uiType, &pContent->taskInteractionReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_confKey:
                    if (0 != confKey2Stream(pContent->uiType, &pContent->confKey, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                //
                case Consts.CONST_imCommType_statusConfLayoutParam:
                    if (0 != statusConfLayoutParam2Stream(pContent->uiType, &pContent->statusConfLayoutParam, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_confLayout:
                    if (0 != confLayout2Stream(pContent->uiType, &pContent->confLayout, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                /*

			case Consts.CONST_imCommType_confState:  //  2017/09/17
				if (confState2Stream(pContent->uiType, &pContent->confState, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
				break;
				*/
                case Consts.CONST_imCommType_confReq:
                    if (0 != confReq2Stream(pContent->uiType, &pContent->confReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                /*
			case Consts.CONST_imCommType_refreshWebContactsInfo:
				if (refreshWebContactsInfo2Stream(pContent->uiType, &pContent->refreshWebContactsInfo, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
				break;
			case Consts.CONST_imCommType_vDevComs:
				if (qyVDevComs2Stream(pContent->uiType, &pContent->vDevComs, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
				break;
			case Consts.CONST_imCommType_retrieveSmPolicy:
				if (retrieveSmPolicy2Stream(pContent->uiType, &pContent->retrieveSmPolicy, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
				break;
			case Consts.CONST_imCommType_retrievePhoneMsgrs:
				if (retrievePhoneMsgrs2Stream(pContent->uiType, &pContent->retrievePhoneMsgrs, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
				break;
			*/
                case Consts.CONST_imCommType_getCfgsReq:   //  2011/01/30
                    if (0 != qisGetCfgsReq2Stream(pContent->uiType, &pContent->getCfgsReq, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;

                case Consts.CONST_imCommType_ptzControlCmd:    //  2012/08/05
                    if (0 != ptzControlCmd2Stream((int)pContent->uiType, &pContent->ptzControlCmd, stream_pBuf, (uint*)&lenInBytes))
                    {
                        goto errLabel;
                    }
                    break;
                /*
            case Consts.CONST_imCommType_retrieveToPaths:
                if (retrieveToPaths2Stream(pContent->uiType, &pContent->retrieveToPaths, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_refreshDynBmps:
                if (refreshDynBmps2Stream(pContent->uiType, &pContent->refreshDynBmps, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_verifyViewDynBmp: //  2011/10/31
                if (verifyViewDynBmp2Stream(pContent->uiType, &pContent->verifyViewDynBmp, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_retrieveDynBmps:
                if (retrieveDynBmps2Stream(pContent->uiType, &pContent->retrieveDynBmps, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_getDaemonStatusReq:   //  2011/01/09
                if (getDaemonStatusReq2Stream(pContent->uiType, &pContent->getDaemonStatusReq, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_queryCustomerServiceOfficerReq:   //  2011/04/06
                if (queryCustomerServiceOfficerReq2Stream(pContent->uiType, &pContent->queryCustomerServiceOfficerReq, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_queryCustomerServiceOfficerReplyReq:  //  2011/04/06
                if (queryCustomerServiceOfficerReplyReq2Stream(pContent->uiType, &pContent->queryCustomerServiceOfficerReplyReq, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_transferGpsInfo:  //  2012/04/20
                if (transferGpsInfo2Stream(pContent->uiType, &pContent->transferGpsInfo, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_transferGpsReplyInfo: //  2012/04/20
                if (transferGpsReplyInfo2Stream(pContent->uiType, &pContent->transferGpsReplyInfo, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_transferGpsData:
                if (transferGpsData2Stream(pContent->uiType, &pContent->transferGpsData, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_transferGpsDataResp:
                if (transferGpsDataResp2Stream(pContent->uiType, &pContent->transferGpsDataResp, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            case Consts.CONST_imCommType_ptzControlReq:    //  2012/08/05
                if (ptzControlReq2Stream(pContent->uiType, &pContent->ptzControlReq, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
        
            //
            case Consts.CONST_imCommType_queryPeerStatus:  //  2014/07/11
                if (queryPeerStatus2Stream(pContent->uiType, &pContent->queryPeerStatus, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            //
            case Consts.CONST_imCommType_procOfflineRes:  //  2015/08/01
                if (procOfflineResU2Stream(pContent->uiType, &pContent->procOfflineResU, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
            //
            case Consts.CONST_imCommType_imGrp:  //  2018/10/27
                if (imGrpEx2Stream(pContent->uiType, &pContent->imGrpEx, stream_pBuf, (uint *  ) & lenInBytes)  )  goto errLabel;
                break;
                         */

                //
                default:
                    {
                        string str = string.Format(("qyhtonRouteTalkData: uiContentType {0} can't be supported"), pContent->uiType);
                        traceLog(str);
                    }
                    goto errLabel;
            }

            stream_hton.ulStreamLen = (uint)lenInBytes;
            lenInBytes += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");

            if ((lenInBytes = qyhtonImLStream(0, (IM_L_STREAM*)&stream_hton)) == -1)
            {
                goto errLabel;
            }
            mymemcpy((IntPtr)ptr, (IntPtr)(byte*)&stream_hton, (uint)Marshal.OffsetOf<IM_L_STREAM>("buf"));

            lenInBytes += lenInBytes_addr;


            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *puiBufSize = (uint)lenInBytes;
            }
            return iErr;

        }


        public static unsafe int ptzControlCmd2Stream(int uiStreamId, PTZ_control_cmd* pReq, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;
            long ulIp = 0;


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;
            //  2014/09/13
            if (0 != pReq->cmdInfo.obj.uiObjType)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_uiObjType, (void*)pReq->cmdInfo.obj.uiObjType, 0, &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->cmdInfo.obj.usIndex_obj)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_index, (void*)pReq->cmdInfo.obj.usIndex_obj, 0, &ptr, &len)) goto errLabel;
            }
            //  2016/07/24
            if (0 != pReq->cmdInfo.obj.usHelp_subIndex)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_short, Consts.CONST_qnmCfgId_help_subIndex, (void*)pReq->cmdInfo.obj.usHelp_subIndex, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->cmdInfo.idInfo.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_messengerId, (void*)&pReq->cmdInfo.idInfo.ui64Id, sizeof(Int64), &ptr, &len)) goto errLabel;
            }
            if (0 != pReq->cmdInfo.iChannel)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l32, Consts.CONST_qnmCfgId_iChannel, (void*)pReq->cmdInfo.iChannel, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->cmdInfo.ucCmd)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_nCmd, (void*)pReq->cmdInfo.ucCmd, 0, &ptr, &len)) goto errLabel;
            }
            //
            if (0 != pReq->cmdInfo.ucMouseStatus)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_char, Consts.CONST_qnmCfgId_mouseStatus, (void*)pReq->cmdInfo.ucMouseStatus, 0, &ptr, &len)) goto errLabel;
            }

            //
            if (0 != pReq->idInfo_imGrp_related.ui64Id)
            {
                if (0 != data2Stream(Consts.CONST_qyDataType_l64, Consts.CONST_qnmCfgId_idInfo_grp_related, (void*)&pReq->idInfo_imGrp_related.ui64Id, sizeof(long), &ptr, &len)) goto errLabel;
            }


            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {
                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }
        /// <summary>
        //  QY_MESSENGER_ACCOUNT
        public static unsafe int messengerAccount2Stream(int uiStreamId, QY_MESSENGER_ACCOUNT* pPcInfo, byte* buf, uint* uiBufSize)
        {
            int iErr = -1;
            int i = 0;
            byte* ptr = buf;
            uint len = *uiBufSize;
            long ulIp = 0;
            if (null == buf || null == pPcInfo) return -1;


            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_start, (void*)uiStreamId, 0, &ptr, &len)) goto errLabel;
            //
            if (0 != data2Stream(Consts.CONST_qyDataType_str, Consts.CONST_qnmCfgId_messengerPasswd, pPcInfo->messengerPasswd, (uint)mystrlen(pPcInfo->messengerPasswd), &ptr, &len)) goto errLabel;
            //
            if (0 != data2Stream(Consts.CONST_qyDataType_long, Consts.CONST_qnmCfgId_null, null, 0, &ptr, &len)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 == iErr)
            {

                *uiBufSize = *uiBufSize - len;
            }
            return iErr;
        }


        public static unsafe int tmpHandler_stream2MessengerAccount(CTX_stream2Data* pCtx, void* p0, void* p1, int uiStreamId, QY_CFGITEM_ntoh_U* pItem)
        {
            int iErr = -1;
            //  p0;
            QY_MESSENGER_ACCOUNT* pPcInfo = (QY_MESSENGER_ACCOUNT*)p1;
            //  QY_CFGITEM				*	pItem		=	(  QY_CFGITEM  *  )p2;
            //  long						lVal;

            if (null == pPcInfo) goto errLabel;

            if (uiStreamId != Consts.CONST_imCommType_messengerAccount) goto errLabel;





            short tmp_cfgId;
            short* p_tmp_cfgId = &tmp_cfgId;
            short* p_cfgId = (short*)&pItem->head.cfgId;

            switch (tmp_cfgId)
            {
                case Consts.CONST_qnmCfgId_messengerPasswd:
                    if (0 != getFieldData_str(pItem, pPcInfo->messengerPasswd, Consts.CONST_maxMessengerPasswdLen + 1)) goto errLabel;
                    break;
                default:
                    break;
            }
            iErr = 0;
        errLabel:
            return iErr;
        }
        /// <returns></returns>


        //  其中ucFlg用来表示要转换成talkData或addrTalkData
        public static unsafe int qyntohRouteTalkData(byte ucFlg, byte* data, uint dataLen, MSG_ROUTE* pAddr, IM_CONTENTU* pContent, char* tHintBuf_showInfo, uint uiCnt_tHintBuf_showInfo)
        {
            int iErr = -1;
            int lenInBytes = 0;
            byte* ptr;
            uint size;
            //TCHAR tHintBuf[255 + 1] = _T("");
            uint uiType = 0;
            uint uiContentType = 0;


            if (null == data || null == pContent) return -1;

            if (isUcFlgRouteTalkData(ucFlg))
            {
                //
                if (null == pAddr) goto errLabel;
                mymemset((IntPtr)pAddr, 0, sizeof(MSG_ROUTE));
                //
                ptr = data;
                size = dataLen;
                //
                if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2MsgRoute, null, pAddr))
                {
                    traceLog(("qyntohRouteTalkData: stream2Data 1 failed.\n"));
                    goto errLabel;
                }
                //  剩下的size就是pStream
            }
            else if (isUcFlgTalkData(ucFlg))
            {       //  仅用来talkData
                if (null == pAddr) goto errLabel;
                mymemset((IntPtr)pAddr, 0, sizeof(MSG_ROUTE));
                if (dataLen < sizeof(MSG_SIMPLE_ROUTE)) goto errLabel;

                MSG_SIMPLE_ROUTE* pTalkDataAddr;
                int lenInBytes_addr;

                pTalkDataAddr = (MSG_SIMPLE_ROUTE*)data;
                pAddr->idInfo_from.ui64Id = pTalkDataAddr->idInfo_from.ui64Id;
                pAddr->idInfo_to.ui64Id = pTalkDataAddr->idInfo_to.ui64Id;
                //
                MACRO_ntohl64(ref pAddr->idInfo_from.ui64Id);
                MACRO_ntohl64(ref pAddr->idInfo_to.ui64Id);
                //
                lenInBytes_addr = sizeof(MSG_SIMPLE_ROUTE);
                //
                ptr = data + lenInBytes_addr;
                size = (uint)(dataLen - lenInBytes_addr);
            }
            else
            {
                //  这里是没有addr的数据
                ptr = data;
                size = dataLen;
            }

            //
            //
            //  这里应该做一个将contentU转换guo来的工作。
            //  注意：在qyMc里，有所有的input的数据转换时，都要修改
            //  另外：要特别注意和老程序的通信的兼容性。老程序的talk和task等结构都是用MIS_MSG_TALKDATA发的。
            //

            if (size < sizeof(uint)) goto errLabel;

            //  uiType  =  qyntohl(  (  (  IM_CONTENTU  *  )ptr  )->uiType  );
            //  memcpy(  &uiType,  &(  (  IM_CONTENTU  *  )ptr  )->uiType,  sizeof(  uiType  )  );
            mymemcpy((IntPtr)(byte*)&uiType, (IntPtr)ptr, sizeof(uint));
            MACRO_ntohi(ref uiType);
            switch (uiType)
            {
                case 0:
                    if (size < sizeof(uint)) goto errLabel;
                    mymemcpy((IntPtr)(byte*)&pContent->uiType, (IntPtr)ptr, sizeof(uint));
                    MACRO_ntohi(ref pContent->uiType);
                    lenInBytes = sizeof(uint);
                    iErr = 0; goto errLabel;
                    break;
                case Consts.CONST_imCommType_simpleResp:
                    if (size < sizeof(IM_SIMPLE_RESP)) goto errLabel;
                    mymemcpy((IntPtr)(byte*)&pContent->simpleResp, (IntPtr)ptr, (uint)sizeof(IM_SIMPLE_RESP));
                    if ((lenInBytes = qyntohImSimpleResp(0, &pContent->simpleResp)) < 0) goto errLabel;
                    iErr = 0; goto errLabel;
                    break;
                /*
            case Consts.CONST_imCommType_refreshRecentFriendsReq:
                //
                mymemcpy((IntPtr)(byte*)&pContent->refreshRecentFriendsReq, ptr, Math.Min(size, sizeof(pContent->refreshRecentFriendsReq))  );
                if ((lenInBytes = qyntohRefreshRecentFriendsReq(0, &pContent->refreshRecentFriendsReq)) < 0) goto errLabel;
                iErr = 0; goto errLabel;
                */
                default:
                    break;
            }



            if (uiType == Consts.CONST_imCommType_lStream)
            {
                //
                IM_L_STREAM_nh stream_ntoh;
                IM_L_STREAM_nh* pStream = &stream_ntoh;

                //  pStream  =  (  IM_L_STREAM  *  )ptr;

                if ((lenInBytes = qyntohImLStream(0, ptr, pStream)) < 0) goto errLabel;

                if (size < (uint)lenInBytes) goto errLabel;

                ptr = pStream->p;
                //  size  =  pStream->ulStreamLen;
                mymemcpy((IntPtr)(byte*)&size, (IntPtr)(byte*)&pStream->ulStreamLen, sizeof(uint));

                if (size < (int)Marshal.OffsetOf<IM_L_STREAM>("buf")) goto errLabel;

                //  uiContentType  =  pStream->uiContentType;
                mymemcpy((IntPtr)(byte*)&uiContentType, (IntPtr)(byte*)&pStream->uiContentType, sizeof(uint));
            }
            else
            {
                string str;
                str = string.Format("qyntohRouteTalkData 得到了非长流的数据包, uiType {0}, size {1}", uiType, size);
                traceLog(str);
                goto errLabel;
            }


            {
                switch (uiContentType)
                {


                    case Consts.CONST_imCommType_htmlContent:
                        mymemset((IntPtr)(byte*)&pContent->html, 0, sizeof(IM_HTML_CONTENT));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2ImHtmlContent, null, pContent)) goto errLabel;
                        break;
                    /*
										case Consts.CONST_imCommType_refreshRecentFriendsResp:
											memset(&pContent->refreshRecentFriendsResp, 0, sizeof(pContent->refreshRecentFriendsResp)  );
											if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RefreshRecentFriendsResp, 0, pContent))
											{       //  这里，对万一有数据包下发不完整的情况下，做截断处理。不简单退出
													//  
													//  qyShowInfo1(  Consts.CONST_qyShowType_qwmComm,  (  ""  ),  _T(  ""  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "    stream2Data failed, usCnt is %d, 截断处理"  ),  pContent->refreshRecentFriendsResp.usCnt  );
												_sntprintf(tHintBuf, mycountof(tHintBuf), _T("stream2Data failed, usCnt is %d, 截断处理"), pContent->refreshRecentFriendsResp.usCnt);
												//
												if (pContent->refreshRecentFriendsResp.usCnt_mems) pContent->refreshRecentFriendsResp.usCnt_mems--; //  因为最后一个包不完整，所以要去掉
												pContent->refreshRecentFriendsResp.usCnt = pContent->refreshRecentFriendsResp.usCnt_mems;
												//
											}
											if (pContent->refreshRecentFriendsResp.usCnt > mycountof(pContent->refreshRecentFriendsResp.mems)) goto errLabel;
											break;
					*/
                    case Consts.CONST_imCommType_getCfgsReq:
                        //  memset(  &pContent->intervalParams,  0,  sizeof(  pContent->intervalParams  )  );
                        mymemset((IntPtr)(byte*)&pContent->getCfgsReq, 0, sizeof(QIS_getCfgs_req));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2QisGetCfgsReq, null, pContent)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_retrieveImGrpListResp:
                        mymemset((IntPtr)(byte*)&pContent->retrieveImGrpListResp, 0, sizeof(RETRIEVE_IMGRPLIST_RESP));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveImGrpListResp, null, &pContent->retrieveImGrpListResp)) goto errLabel;
                        if (pContent->retrieveImGrpListResp.usCnt > Consts.CONST_maxContactUpdateInfos_grp) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_retrieveImGrpMemListResp:
                        mymemset((IntPtr)(byte*)&pContent->retrieveImGrpMemListResp, 0, sizeof(RETRIEVE_IMGRPMEMLIST_RESP));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveImGrpMemListResp, null, &pContent->retrieveImGrpMemListResp)) goto errLabel;
                        if (pContent->retrieveImGrpMemListResp.usCnt > Consts.CONST_maxContactUpdateInfos_grpMem) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_retrieveContactListResp:
                        mymemset((IntPtr)(byte*)&pContent->retrieveContactListResp, 0, sizeof(RETRIEVE_CONTACTLIST_RESP));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveContactListResp, null, &pContent->retrieveContactListResp)) goto errLabel;
                        if (pContent->retrieveContactListResp.usCnt > Consts.CONST_maxContactUpdateInfos_contact) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_retrieveImObjListEndResp:
                        mymemset((IntPtr)(byte*)&pContent->retrieveImObjListEndResp, 0, sizeof(RETRIEVE_IMOBJLISTEND_RESP));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveImObjListEndResp, null, &pContent->retrieveImObjListEndResp)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_transferFileReq:
                        mymemset((IntPtr)(byte*)&pContent->transferFileReq, 0, sizeof(TRANSFER_FILE_REQ));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferFileReq, null, &pContent->transferFileReq)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_transferFileReplyReq:
                        mymemset((IntPtr)(byte*)&pContent->transferFileReplyReq, 0, sizeof(TRANSFER_FILE_REPLYREQ));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferFileReplyReq, null, &pContent->transferFileReplyReq)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_taskProcReq:
                        mymemset((IntPtr)(byte*)&pContent->taskProcReq, 0, sizeof(TASK_PROC_REQ));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TaskProcReq, null, &pContent->taskProcReq)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_taskProcResp:
                        mymemset((IntPtr)(byte*)&pContent->taskProcResp, 0, sizeof(TASK_PROC_RESP));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TaskProcResp, null, &pContent->taskProcResp)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_transferFileDataReq:
                        mymemset((IntPtr)(byte*)&pContent->transferFileDataReq, 0, sizeof(TRANSFER_FILEDATA_REQ));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferFileDataReq, null, &pContent->transferFileDataReq)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_transferFileDataResp:
                        mymemset((IntPtr)(byte*)&pContent->transferFileDataResp, 0, (int)Marshal.OffsetOf<TRANSFER_FILEDATA_RESP>("buf"));  //  把控制数据清空即可
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferFileDataResp, null, &pContent->transferFileDataResp)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_transferFileEndReq:
                        mymemset((IntPtr)(byte*)&pContent->transferFileEndReq, 0, sizeof(TRANSFER_FILEEND_REQ));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferFileEndReq, null, &pContent->transferFileEndReq)) goto errLabel;
                        break;
                    /*
                            case Consts.CONST_imCommType_rtcCallReq:
                                memset(&pContent->rtcCallReq, 0, sizeof(pContent->rtcCallReq)  );
                                if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RtcCallReq, 0, &pContent->rtcCallReq)) goto errLabel;
                                break;
                */
                    case Consts.CONST_imCommType_transferAvInfo:
                        mymemset((IntPtr)(byte*)&pContent->transferAvInfo, 0, sizeof(TRANSFER_AV_INFO));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferAvInfo, null, &pContent->transferAvInfo)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_transferAvReplyInfo:
                        mymemset((IntPtr)(byte*)&pContent->transferAvReplyInfo, 0, sizeof(TRANSFER_AV_replyInfo));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferAvReplyInfo, null, &pContent->transferAvReplyInfo)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_transferVideoData:
                        mymemset((IntPtr)(byte*)&pContent->transferVideoData, 0, (int)Marshal.OffsetOf<TRANSFER_VIDEO_DATA>("buf"));  //  这里，把控制数据清空即可。
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferVideoData, null, &pContent->transferVideoData)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_transferVideoDataResp:
                        mymemset((IntPtr)(byte*)&pContent->transferVideoDataResp, 0, sizeof(TRANSFER_VIDEO_dataResp));  //  这里，把控制数据清空即可。
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferVideoDataResp, null, &pContent->transferVideoDataResp)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_transferAudioData:
                        mymemset((IntPtr)(byte*)&pContent->transferAudioData, 0, (int)Marshal.OffsetOf<TRANSFER_AUDIO_DATA>("buf"));  //  这里，把控制数据清空即可。
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferAudioData, null, &pContent->transferAudioData)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_transferAudioDataResp:
                        mymemset((IntPtr)(byte*)&pContent->transferAudioDataResp, 0, sizeof(TRANSFER_AUDIO_dataResp));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferAudioDataResp, null, &pContent->transferAudioDataResp)) goto errLabel;
                        break;
                    /*
                            case Consts.CONST_imCommType_remoteAssistReq:
                                memset(&pContent->remoteAssistReq, 0, sizeof(pContent->remoteAssistReq)  );
                                if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RemoteAssistReq, 0, &pContent->remoteAssistReq)) goto errLabel;
                                break;
                    */
                    case Consts.CONST_imCommType_taskInteractionReq:
                        mymemset((IntPtr)(byte*)&pContent->taskInteractionReq, 0, sizeof(TASK_INTERACTION_REQ));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TaskInteractionReq, null, &pContent->taskInteractionReq)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_confKey:
                        mymemset((IntPtr)(byte*)&pContent->confKey, 0, sizeof(CONF_KEY));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2confKey, null, &pContent->confKey)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_statusConfLayoutParam:
                        mymemset((IntPtr)(byte*)&pContent->statusConfLayoutParam, 0, sizeof(StatusConfLayoutParam));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2statusConfLayoutParam, null, &pContent->statusConfLayoutParam)) goto errLabel;
                        break;

                    case Consts.CONST_imCommType_confLayout:
                        mymemset((IntPtr)(byte*)&pContent->confLayout, 0, sizeof(CONF_LAYOUT));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2confLayout, null, &pContent->confLayout)) goto errLabel;
                        break;
                    case Consts.CONST_imCommType_confNvrInfo:
                        mymemset((IntPtr)(byte*)&pContent->confNvrInfo, 0, sizeof(ConfNvrInfo));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2confNvrInfo, null, &pContent->confNvrInfo)) goto errLabel;
                        //
#if DEBUG
                        byte* pBuf = pContent->confNvrInfo.nvrInfo.buf_ipcInfo;
                        IpcInfo* pMem = getMem((IpcInfo*)pBuf, Consts.bufSize_ipcInfos, 0);
                        string str2 = new string(pMem->name);
                        //
                        int ii = 0;

#endif
                        //
                        break;
                    /*

						case Consts.CONST_imCommType_confState:  //  2017/09/17
							memset(&pContent->confState, 0, sizeof(pContent->confState)  );
							if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2ConfState, 0, &pContent->confState)) goto errLabel;
							break;
				*/
                    case Consts.CONST_imCommType_confReq:
                        mymemset((IntPtr)(byte*)&pContent->confReq, 0, sizeof(CONF_req));
                        if (0 != stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2ConfReq, null, &pContent->confReq)) goto errLabel;
                        break;

                    /*
							case Consts.CONST_imCommType_refreshWebContactsInfo:
								memset(&pContent->refreshWebContactsInfo, 0, sizeof(pContent->refreshWebContactsInfo)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RefreshWebContactsInfo, 0, &pContent->refreshWebContactsInfo)) goto errLabel;
								break;
							case Consts.CONST_imCommType_vDevComs:
								memset(&pContent->vDevComs, 0, sizeof(pContent->vDevComs)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2QyVDevComs, 0, &pContent->vDevComs)) goto errLabel;
								break;
							case Consts.CONST_imCommType_retrieveSmPolicy:
								memset(&pContent->retrieveSmPolicy, 0, sizeof(pContent->retrieveSmPolicy)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveSmPolicy, 0, &pContent->retrieveSmPolicy)) goto errLabel;
								break;
							case Consts.CONST_imCommType_retrievePhoneMsgrs:
								memset(&pContent->retrievePhoneMsgrs, 0, sizeof(pContent->retrievePhoneMsgrs)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrievePhoneMsgrs, 0, &pContent->retrievePhoneMsgrs)) goto errLabel;
								break;
							case Consts.CONST_imCommType_retrieveToPaths:
								memset(&pContent->retrieveToPaths, 0, sizeof(pContent->retrieveToPaths)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveToPaths, 0, &pContent->retrieveToPaths)) goto errLabel;
								break;
							case Consts.CONST_imCommType_refreshDynBmps:
								memset(&pContent->refreshDynBmps, 0, sizeof(pContent->refreshDynBmps)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RefreshDynBmps, 0, &pContent->refreshDynBmps)) goto errLabel;
								break;
							case Consts.CONST_imCommType_verifyViewDynBmp: //  2011/10/31
								memset(&pContent->verifyViewDynBmp, 0, sizeof(pContent->verifyViewDynBmp)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2VerifyViewDynBmp, 0, &pContent->verifyViewDynBmp)) goto errLabel;
								break;
							case Consts.CONST_imCommType_retrieveDynBmps:
								memset(&pContent->retrieveDynBmps, 0, sizeof(pContent->retrieveDynBmps)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveDynBmps, 0, &pContent->retrieveDynBmps)) goto errLabel;
								break;
							case Consts.CONST_imCommType_refreshImObjRulesReq: //  2010/08/15
								memset(&pContent->refreshImObjRulesReq, 0, sizeof(pContent->refreshImObjRulesReq)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RefreshImObjRulesReq, 0, &pContent->refreshImObjRulesReq)) goto errLabel;
								break;
							case Consts.CONST_imCommType_getDaemonStatusReq:       //  2011/01/09
								memset(&pContent->getDaemonStatusReq, 0, sizeof(pContent->getDaemonStatusReq)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2GetDaemonStatusReq, 0, &pContent->getDaemonStatusReq)) goto errLabel;
								break;
							case Consts.CONST_imCommType_retrieveCustomerServiceObjListReq:    //  2011/04/04
								memset(&pContent->retrieveCustomerServiceObjListReq, 0, sizeof(pContent->retrieveCustomerServiceObjListReq)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2RetrieveCustomerServiceObjListReq, 0, &pContent->retrieveCustomerServiceObjListReq)) goto errLabel;
								break;
							case Consts.CONST_imCommType_queryCustomerServiceOfficerReq:       //  2011/04/06
								memset(&pContent->queryCustomerServiceOfficerReq, 0, sizeof(pContent->queryCustomerServiceOfficerReq)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2QueryCustomerServiceOfficerReq, 0, &pContent->queryCustomerServiceOfficerReq)) goto errLabel;
								break;
							case Consts.CONST_imCommType_queryCustomerServiceOfficerReplyReq:      //  2011/04/06
								memset(&pContent->queryCustomerServiceOfficerReplyReq, 0, sizeof(pContent->queryCustomerServiceOfficerReplyReq)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2QueryCustomerServiceOfficerReplyReq, 0, &pContent->queryCustomerServiceOfficerReplyReq)) goto errLabel;
								break;
							case Consts.CONST_imCommType_transferGpsInfo:      //  2012/04/20
								memset(&pContent->transferGpsInfo, 0, sizeof(pContent->transferGpsInfo)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferGpsInfo, 0, &pContent->transferGpsInfo)) goto errLabel;
								break;
							case Consts.CONST_imCommType_transferGpsReplyInfo:
								memset(&pContent->transferGpsReplyInfo, 0, sizeof(pContent->transferGpsReplyInfo)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferGpsReplyInfo, 0, &pContent->transferGpsReplyInfo)) goto errLabel;
								break;
							case Consts.CONST_imCommType_transferGpsData:
								memset(&pContent->transferGpsData, 0, sizeof(pContent->transferGpsData)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferGpsData, 0, &pContent->transferGpsData)) goto errLabel;
								break;
							case Consts.CONST_imCommType_transferGpsDataResp:
								memset(&pContent->transferGpsDataResp, 0, sizeof(pContent->transferGpsDataResp)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2TransferGpsDataResp, 0, &pContent->transferGpsDataResp)) goto errLabel;
								break;
							case Consts.CONST_imCommType_ptzControlReq:
								memset(&pContent->ptzControlReq, 0, sizeof(pContent->ptzControlReq)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2PtzControlReq, 0, &pContent->ptzControlReq)) goto errLabel;
								break;
							case Consts.CONST_imCommType_ptzControlCmd:
								memset(&pContent->ptzControlCmd, 0, sizeof(pContent->ptzControlCmd)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2PtzControlCmd, 0, &pContent->ptzControlCmd)) goto errLabel;
								break;
							//  
							case Consts.CONST_imCommType_queryPeerStatus:  //  2014/07/11
								memset(&pContent->queryPeerStatus, 0, sizeof(pContent->queryPeerStatus)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2QueryPeerStatus, 0, &pContent->queryPeerStatus)) goto errLabel;
								break;
							//
							case Consts.CONST_imCommType_procOfflineRes:  //  2015/08/01
								memset(&pContent->procOfflineResU, 0, sizeof(pContent->procOfflineResU)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2ProcOfflineResU, 0, &pContent->procOfflineResU)) goto errLabel;
								break;
							//
							case Consts.CONST_imCommType_imGrp:  //  2017/07/26
								memset(&pContent->imGrpEx, 0, sizeof(pContent->imGrpEx)  );
								if (stream2Data(&ptr, &size, filterStream_is, tmpHandler_stream2ImGrpEx, 0, &pContent->imGrpEx)) goto errLabel;
								break;
					*/
                    //
                    default:
                        {
                            string str;
                            //str = string.Format("qyntohRouteTalkData failed: uiContentType {0} {1}, error"), uiContentType, qyGetDesByType1(CONST_qyCommTypeTable_en, uiContentType));
                            str = string.Format("qyntohRouteTalkData failed: uiContentType {0} error", uiContentType);
                            showInfo_open(0, null, str);

                        }
                        goto errLabel;
                }

                iErr = 0; goto errLabel;
            }




            iErr = 0;
        errLabel:
            if (iErr != 0)
            {
                //  _sntprintf(  tHintBuf,  mycountof(  tHintBuf  ),  _T(  "%scontentType [%s]"  ),  tHintBuf,  qyGetDesByType1(  CONST_qyCommTypeTable,  uiContentType  )  );
                //traceLogA("qyntohAddrTalkData failed");
            }
            if (0 == iErr)
            {
            }
            //
            //if (tHintBuf_showInfo) lstrcpyn(tHintBuf_showInfo, tHintBuf, uiCnt_tHintBuf_showInfo);

            return iErr;

        }




    }


}
