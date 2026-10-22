using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public partial class Consts
{
    public const int CONST_qType_decA = 1;
    public const int CONST_qType_playA = 2;
    //
    public const int CONST_qType_decV = 5;

    //
    public const int CONST_qType_showInfo = 10;


};



namespace qy
{
    // not used
    public unsafe class QY_qBuf2
{
        //
        public struct QY_qBuf2_cfg
        {
            public int m_qType;

            //
            public uint uiMaxQNodes;

            //
            public RW_lock_param rwLockParam_syncFlg;
        };
        public QY_qBuf2_cfg cfg;
        public string name;

        //
        bool bFlag_toPostMsg_q1 =false;
        public CMutexRW pRw_syncFlg;

        //
        public uint dwTickCnt_lastData;

        //
        public class QY_qBuf {

            public struct Tmp_qBuf_cfg
            {
                public int m_qType;

                public uint uiMaxQNodes;
                public int maxDataBufLen;        //  

                //
                public bool b_showInfoQ;
            };

            //
            Tmp_qBuf_cfg cfg;
            string name;

            //
            public Tmp_qBuf_memHead[] memHeads;

            //
            public byte* pBuf;

            //
            public int startIndex_memHead;
            public int uiQNodes;

            //            
            public int startPos_data;
            public int dataLen;

            //
            //
            public bool bNeedSync;

            //CRITICAL_SECTION cs;
            //BOOL bCsInited;

            public SYNC_OBJ_PARAM syncObjParam;
            public Mutex hMutex;



            //
            public bool isQFull()
            {
                QY_qBuf pQ = this;
                if (pQ == null) return false;
                if (pQ.cfg.uiMaxQNodes != 0 && pQ.uiQNodes >= pQ.cfg.uiMaxQNodes) return true;
                return false;
            }

            public bool isQWarning( int uiQNodes_warningInterval)
            {
                QY_qBuf pQ = this;
                if (pQ == null) return false;
                if (pQ.cfg.uiMaxQNodes != 0 && pQ.uiQNodes + uiQNodes_warningInterval >= pQ.cfg.uiMaxQNodes) return true;
                return false;
            }


            //
            public unsafe bool isQEmpty()
            {
                if (this.uiQNodes == 0) return true;
                return false;

            }

            //
            public int init(int qType, uint uiMaxQNodes, int maxDataBufLen,bool bAlloc, string name)
            {
                int iErr = -1;

                QY_qBuf bufInfo = this;
                QY_qBuf pQ = this;

                if (maxDataBufLen == 0) return -1;
                if (bufInfo.pBuf != null) return -1;

                //
                if (name == null) name = "";
                pQ.name = name;
                pQ.cfg.m_qType = qType;

                //
                bufInfo.cfg.uiMaxQNodes = uiMaxQNodes;
                

                //
                pQ.bNeedSync = true;

                //
                pQ.hMutex = new Mutex();
                if (pQ.hMutex == null) goto errLabel;

                //
                if (qType == Consts.CONST_qType_showInfo)
                {
                    pQ.cfg.b_showInfoQ = true;
                }

                //
                if (bAlloc)
                {
                    if (0 != alloc(maxDataBufLen))
                    {
                        goto errLabel;
                    }
                }


                iErr = 0;


                errLabel:
                if (iErr != 0)
                {
                    exit();
                }
                return iErr;
            }

            public void exit()
            {
                QY_qBuf qBuf = this;
                QY_qBuf pQ = this;

                //
                free();

                //
                if (pQ.hMutex != null)
                {
                    pQ.hMutex.Close();
                    pQ.hMutex = null;
                }


            }

            //
            public int alloc(int maxDataBufLen)
            {
                int iErr = -1;
                QY_qBuf pQ = this;
                QY_qBuf bufInfo = this;

                //
                bufInfo.cfg.maxDataBufLen = maxDataBufLen;

                //
                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0)
                    {
                        //qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qPostMsg failed: sync failed"));
                        goto errLabel;
                    }

                    //
                    if ( pQ.pBuf!=null||pQ.memHeads!=null)
                    {
                        goto errLabel;
                    }

                    //
                    bufInfo.memHeads = new Tmp_qBuf_memHead[bufInfo.cfg.uiMaxQNodes];

                    int i;
                    for (i = 0; i < bufInfo.cfg.uiMaxQNodes; i++)
                    {
                        switch (bufInfo.cfg.m_qType)
                        {
                            case Consts.CONST_qType_decA:
                                bufInfo.memHeads[i] = new Tmp_qBuf_memHead_decA();
                                break;
                            case Consts.CONST_qType_showInfo:
                                bufInfo.memHeads[i] = new Tmp_qBuf_memHead_showInfo();
                                break;
                            case Consts.CONST_qType_playA:
                                bufInfo.memHeads[i] = new Tmp_qBuf_memHead_playA();
                                break;
                            case Consts.CONST_qType_decV:
                                bufInfo.memHeads[i] = new Tmp_qBuf_memHead_decV(); 
                                break;
                            default:
                                qyFuncs.traceLog("qBuf.alloc failed, type error");
                                goto errLabel;
                        }
                    }

                    bufInfo.pBuf = (byte*)qyFuncs.mymalloc(bufInfo.cfg.maxDataBufLen);
                    if (bufInfo.pBuf == null) goto errLabel;

                }

                iErr = 0;
            errLabel:
                if (0 != iErr)
                {
                    free();
                }
                return iErr;
            }

            public void free()
            {
                QY_qBuf pQ = this;
                QY_qBuf qBuf = this;

                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0)
                    {
                        //qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qPostMsg failed: sync failed"));
                        goto errLabel;
                    }

                    //
                    qBuf.memHeads = null;
                    if (qBuf.pBuf != null)
                    {
                        qyFuncs.myfree((IntPtr)qBuf.pBuf);
                        qBuf.pBuf = null;
                    }


                }

            errLabel:

                    return;
            }

                //
                public unsafe int qPostMsg(IntPtr pHeadData, IntPtr pQElem0, byte[]pQElem1, uint size)
            {
                int iErr = -1;
                QY_qBuf pQ = this;
                string str;

                //
                if (pHeadData == IntPtr.Zero) return -1;
                if (pQElem0 == IntPtr.Zero&&pQElem1==null) return -1;
                if ( size == 0) return -1;

                //
                using (CQySyncObj syncObj = new CQySyncObj())
                {
                    if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0)
                    {
                        //qyShowInfo_internal(CONST_qyShowType_warning, 0, "", pQ->cfg.name, NULL, NULL, _T(""), _T("qPostMsg failed: sync failed"));
                        goto errLabel;
                    }

                    //
                    if (pQ.pBuf == null)
                    {
                        if (!pQ.cfg.b_showInfoQ)
                        {
                            qyFuncs.showInfo_open(0, "", "qbuf.qPostMsg failed, pBuf is null");

                        }
                        goto errLabel;
                    }

                    //
                    if ( pQ.uiQNodes>=pQ.cfg.uiMaxQNodes)
                    {
                        str = string.Format(pQ.name + " qFull");
                        if (pQ.cfg.b_showInfoQ)
                        {
                            qyFuncs.traceLog(str);
                        }
                        else
                        {
                            qyFuncs.showInfo_open(0, "", str);
                        }
                        //
                        iErr = Consts.CONST_qyRet_qFull;  goto errLabel;
                    }

                    //
                    if ( pQ.dataLen+size>pQ.cfg.maxDataBufLen )
                    {
                        str = string.Format(pQ.name + " qFull");
                        if (pQ.cfg.b_showInfoQ)
                        {
                            qyFuncs.traceLog(str);
                        }
                        else
                        {
                            qyFuncs.showInfo_open(0, "", str);
                        }
                        //
                        goto errLabel;
                    }

                    //
                    switch (pQ.cfg.m_qType)
                    {
                        case Consts.CONST_qType_showInfo:
                            {
                                Tmp_qBuf_memHead_showInfo head = (Tmp_qBuf_memHead_showInfo)pQ.memHeads[pQ.uiQNodes];
                                //
                                head.headData.len_showInfo = (int)size;
                                //

                            }
                            break;
                        case Consts.CONST_qType_decA:
                            {
                                Tmp_qBuf_memHead_decA head = (Tmp_qBuf_memHead_decA)pQ.memHeads[pQ.uiQNodes];
                                //
                                head.dataMemHead = *(TRANSFER_AUDIO_dataMemHead*)pHeadData;

                            }
                            break;
                        case Consts.CONST_qType_playA:
                            {
                                Tmp_qBuf_memHead_playA head = (Tmp_qBuf_memHead_playA)pQ.memHeads[pQ.uiQNodes];
                                //
                                head.headData = *(HeadData_playA*)pHeadData;

                            }
                            break;
                        case Consts.CONST_qType_decV:
                            {
                                Tmp_qBuf_memHead_decV head = (Tmp_qBuf_memHead_decV)pQ.memHeads[pQ.uiQNodes];
                                //
                                head.dataMemHead = *(TRANSFER_VIDEO_dataMemHead*)pHeadData;
                            }
                            break;
                        default:
                            qyFuncs.traceLog("qPostMsg failed: unprocessed qType");
                            goto errLabel;
                    }
                    pQ.uiQNodes++;
                    //
                    if (pQElem0 != IntPtr.Zero)
                    {
                        qyFuncs.mymemcpy((IntPtr)(pQ.pBuf + pQ.dataLen), pQElem0, size);
                    }
                    else
                    {
                        System.Runtime.InteropServices.Marshal.Copy(pQElem1, 0, (IntPtr)(pQ.pBuf+pQ.dataLen), (int)size);
                    }
                    //
                    pQ.dataLen += (int)size;

                }

                iErr = 0;
                errLabel:

                return iErr;
            }

            //
            unsafe int tmp_qGetMsg( IntPtr pHeadData,IntPtr pQElem, ref uint puiSize, bool bNotShowInfo)
            {
                int iErr = -1;
                QY_qBuf pQ = this;

                if (pQ.uiQNodes == 0) return -1;
                if (pQ.uiQNodes + pQ.startIndex_memHead > pQ.cfg.uiMaxQNodes)
                {
                    qyFuncs.traceLog("qBuf.tmp_qGetMsg failed, err");
                    return -1;
                }

                int dataLen = 0;
                switch (pQ.cfg.m_qType)
                {
                    case Consts.CONST_qType_showInfo:
                        {
                            Tmp_qBuf_memHead_showInfo head = (Tmp_qBuf_memHead_showInfo)pQ.memHeads[pQ.startIndex_memHead];
                            dataLen = head.headData.len_showInfo;
                        }
                        break;
                    case Consts.CONST_qType_decA:
                        {
                            Tmp_qBuf_memHead_decA head = (Tmp_qBuf_memHead_decA)pQ.memHeads[pQ.startIndex_memHead];
                            dataLen = (int)head.dataMemHead.uiLen;
                            *(TRANSFER_AUDIO_dataMemHead*)pHeadData = head.dataMemHead;
                        }
                        break;
                    case Consts.CONST_qType_playA:
                        {
                            Tmp_qBuf_memHead_playA head = (Tmp_qBuf_memHead_playA)pQ.memHeads[pQ.startIndex_memHead];
                            dataLen = (int)head.headData.uiSize_dec;
                            *(HeadData_playA*)pHeadData = head.headData;
                        }
                        break;
                    case Consts.CONST_qType_decV:
                        {
                            Tmp_qBuf_memHead_decV head = (Tmp_qBuf_memHead_decV)pQ.memHeads[pQ.startIndex_memHead];
                            dataLen = (int)head.dataMemHead.uiLen;
                            *(TRANSFER_VIDEO_dataMemHead*)pHeadData = head.dataMemHead;
                        }
                        break;
                    default:
                        qyFuncs.traceLog("tmp_qGetMsg failed: unprocessed qType");
                        goto errLabel;
                }
                if (dataLen > puiSize) goto errLabel;
                if ( dataLen+pQ.startPos_data>pQ.cfg.maxDataBufLen )
                {
                    goto errLabel;
                }
                qyFuncs.mymemcpy(pQElem, (IntPtr)pQ.pBuf + pQ.startPos_data, (uint)dataLen);
                puiSize = (uint)dataLen;
                pQ.startIndex_memHead++;
                pQ.startPos_data += dataLen;
                pQ.dataLen -= dataLen;
                pQ.uiQNodes--;

                //
                if ( pQ.uiQNodes==0)
                {
                    if (pQ.dataLen != 0)
                    {
                        int ii = 0;
                    }
                    //
                    pQ.startIndex_memHead = 0;
                    pQ.startPos_data = 0;
                }


                //
                iErr = 0;
            errLabel:

                return iErr;

            }

            public unsafe int qGetMsg(IntPtr pHeadData,IntPtr pQElem, uint* puiSize)
            {
                QY_qBuf pQ = this;

                if (pQ == null) return -1;
                if (null == pQElem || null == puiSize) return -1;

                using (CQySyncObj syncObj = new CQySyncObj())
                {

                    if (pQ.bNeedSync && syncObj.sync(ref pQ.syncObjParam, pQ.hMutex) != 0) return -1;

                    if (pQ.pBuf == null)
                    {
                        if (!pQ.cfg.b_showInfoQ)
                        {
                            qyFuncs.showInfo_open(0, "", "qBuf.getMsg failed, pBuf is null");
                        }
                        //
                        return -1;
                    }

                    return tmp_qGetMsg( pHeadData,pQElem, ref *puiSize, false);
                }
            }


        };

        //
        QY_qBuf qs0, qs1;

        //
        public EventWaitHandle hEvent0;


        //
        public class Tmp_qBuf_memHead
        {
        };

        //
        class Tmp_qBuf_memHead_decA :Tmp_qBuf_memHead
        {
            public TRANSFER_AUDIO_dataMemHead dataMemHead;
        };

        class Tmp_qBuf_memHead_decV : Tmp_qBuf_memHead
        {
            public TRANSFER_VIDEO_dataMemHead dataMemHead;
        };


        public struct HeadData_playA
        {
            public uint uiPts;         //  2017/10/13
            public uint uiSize_dec;     //  2017/10/13
        };

        //
        class Tmp_qBuf_memHead_playA:Tmp_qBuf_memHead
        {
            public HeadData_playA headData;
        };

        //
        public struct HeadData_showInfo
        {
            public int len_showInfo;

        }

        //
        public class Tmp_qBuf_memHead_showInfo:Tmp_qBuf_memHead
        {
            public HeadData_showInfo headData;
        };



        //
        public int initQyQ2(int qType,uint uiMaxQNodes,int maxDataBufLen, RW_lock_param pRwLockParam_syncFlg,bool bAlloc, string name)
        {
            int iErr = -1;

            QY_qBuf2 pQ2 = this;

            //
            if (0 == uiMaxQNodes) return -1;

            //
            this.cfg.m_qType = qType;
            this.cfg.uiMaxQNodes = uiMaxQNodes;
            this.cfg.rwLockParam_syncFlg = pRwLockParam_syncFlg;

            if (name == null) name = "";
            this.name = name;
            //
            this.pRw_syncFlg = new CMutexRW();
            if (null == this.pRw_syncFlg) goto errLabel;

            //
            qs0 = new QY_qBuf();
            qs1 = new QY_qBuf();

            string str;
            str = name + "0";
            if (qs0.init(qType, uiMaxQNodes, maxDataBufLen,bAlloc,str) != 0) goto errLabel;
            str = name + "1";
            if (qs1.init(qType, uiMaxQNodes, maxDataBufLen,bAlloc,str) != 0) goto errLabel;

            //
            pQ2.hEvent0 = new EventWaitHandle(false, EventResetMode.AutoReset);
            if (pQ2.hEvent0 == null) goto errLabel;



            //
            iErr = 0;

            errLabel:
            if ( iErr != 0)
            {
                exitQyQ2();
            }
            return iErr;
        }

        public void exitQyQ2()
        {
            QY_qBuf2 pQ2 = this;

            if (qs0 != null)
            {
                qs0.exit();
                qs0 = null;
            }
            if (qs1 != null)
            {
                qs1.exit();
                qs1 = null;
            }

            if (pQ2.hEvent0 != null)
            {
                //CloseHandle(pQ2->hEvents[j]); pQ2->hEvents[j] = NULL;
                pQ2.hEvent0.Close(); pQ2.hEvent0 = null;

            }


            if (this.pRw_syncFlg != null)
            {
                this.pRw_syncFlg.Close();
                this.pRw_syncFlg = null;
            }

            return;
        }

        //
        public int alloc(int maxDataBufLen)
        {
            int iErr = -1;

            if (0 != this.qs0.alloc(maxDataBufLen)) goto errLabel;
            if (0 != this.qs1.alloc(maxDataBufLen)) goto errLabel;

            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                free();
            }
            return iErr;
        }

        public void free()
        {
            this.qs0.free();
            this.qs1.free();
        }

        //
        public bool isQ2Warning( int uiQNodes_warningInterval)
        {
            QY_qBuf2 pQ2 = this;

            if (null == pQ2) return false;
            if (pQ2.bFlag_toPostMsg_q1) return pQ2.qs1.isQWarning( uiQNodes_warningInterval);
            else return pQ2.qs0.isQWarning( uiQNodes_warningInterval);
        }
        public bool isQ2Empty()
        {
            QY_qBuf2 pQ2 = this;

            if (null == pQ2) return false;
            return (pQ2.qs0.isQEmpty() && pQ2.qs1.isQEmpty());
        }

        //
        public int q2PostMsg(IntPtr pHeadData, IntPtr pQElem, uint size)
        {
            int iErr = -1;

            QY_qBuf2 pQ2 = this;

            //
            pQ2.dwTickCnt_lastData = qyFuncs.myGetTickCount(null);


            //
            using (CQyQ2Help q2Help = new CQyQ2Help())
            {
                if (q2Help.getQ_toPostMsg(pQ2) == null)
                {
                    qyFuncs.traceLog("q2PostMsg failed, getQ_toPostMsg returns null");
                    return -1;
                }


                return q2Help.m_pQ_toPostMsg.qPostMsg(pHeadData, pQElem, null,size);
            }
        }


        //
        public int q2PostMsg(IntPtr pHeadData, byte[] pQElem, uint size)
        {
            int iErr = -1;

            QY_qBuf2 pQ2 = this;

            //
            pQ2.dwTickCnt_lastData = qyFuncs.myGetTickCount(null);


            //
            using (CQyQ2Help q2Help = new CQyQ2Help())
            {
                if (q2Help.getQ_toPostMsg(pQ2) == null)
                {
                    qyFuncs.traceLog("q2PostMsg failed, getQ_toPostMsg returns null");
                    return -1;
                }


                return q2Help.m_pQ_toPostMsg.qPostMsg(pHeadData, IntPtr.Zero,pQElem, size);
            }
        }


        //
        public int q2PostMsgAndTrigger(IntPtr pHeadData, IntPtr pQElem, uint size)
        {
            QY_qBuf2 pQ2 = this;
            //
            pQ2.dwTickCnt_lastData = qyFuncs.myGetTickCount(null);

            //
            using (CQyQ2Help q2Help = new CQyQ2Help())
            {

                return q2Help.qPostMsgAndTrigger(pHeadData,pQElem, size, pQ2);
            }
        }

        //
        public unsafe int q2GetMsg( IntPtr pHeadData,IntPtr pQElem, uint* puiSize)
        {
            QY_qBuf2 pQ2 = this;

            using (CQyQ2Help q2Help = new CQyQ2Help())
            {
                if (q2Help.getQ_toGetMsg(pQ2) == null) return -1;

                return q2Help.getQ_toGetMsg(pQ2).qGetMsg( pHeadData, pQElem, puiSize);
            }
        }


        //
        public  int getQ2Nodes()
        {
            QY_qBuf2 pQ2 = this;

            if (null == (pQ2)) return -1;
            if (null == pQ2.qs0 || null == pQ2.qs1) return -1;
            return (int)((pQ2).qs0.uiQNodes + (pQ2).qs1.uiQNodes);
        }



        //
        public class CQyQ2Help : CQyRWLock
        {

            public QY_qBuf2 m_pQ2;
            //
            public QY_qBuf m_pQ_toGetMsg;
            public QY_qBuf m_pQ_toPostMsg;


            public CQyQ2Help()
            {
                m_pQ2 = null;

            }
            ~CQyQ2Help()
            { }

            public QY_qBuf getQ_toPostMsg(QY_qBuf2 pQ2)
            {
                if (null == pQ2) return null;
                if (m_pQ2 != null)
                {
                    if (m_pQ2 != pQ2) return null;
                }
                else
                    m_pQ2 = pQ2;

                if (m_pQ_toPostMsg != null) return m_pQ_toPostMsg;

                if (this.rLock(pQ2.pRw_syncFlg, (int)pQ2.cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_r) != 0) return null;

                m_pQ_toPostMsg = pQ2.bFlag_toPostMsg_q1 ? pQ2.qs1 : pQ2.qs0;
                m_pQ_toGetMsg = pQ2.bFlag_toPostMsg_q1 ? pQ2.qs0 : pQ2.qs1;

                return m_pQ_toPostMsg;
            }
            public QY_qBuf getQ_toGetMsg(QY_qBuf2 pQ2)
            {
                if (null == pQ2)
                {
                    return null;
                }

                if (m_pQ2 != null)
                {
                    if (m_pQ2 != pQ2)
                    {
                        return null;
                    }
                }
                else
                    m_pQ2 = pQ2;

                if (m_pQ_toGetMsg != null) return m_pQ_toGetMsg;

                QY_qBuf pQ_toGetMsg;
                QY_qBuf pQ_toPostMsg;
                if (pQ2.bFlag_toPostMsg_q1)
                {
                    pQ_toGetMsg = pQ2.qs0;
                    pQ_toPostMsg = pQ2.qs1;
                }
                else
                {
                    pQ_toGetMsg = pQ2.qs1;
                    pQ_toPostMsg = pQ2.qs0;
                }

                if ((pQ_toGetMsg.isQEmpty()))
                {
                    if (!(pQ_toPostMsg.isQEmpty()))
                    {
                        //
                        string hint = "qBuf2.qyq2Help.getQ_toGetMsg";
                        //  try to switch outputQ
                        if (this.wLock(pQ2.pRw_syncFlg, (int)pQ2.cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_w,hint) != 0)
                        {
                            return null;
                        }
                        pQ2.bFlag_toPostMsg_q1 = !pQ2.bFlag_toPostMsg_q1;
                        this.unlock();
                    }
                }

                /*
            #if  0
                if  (  this->rLock(  pQ2->mutexName_syncFlg,  pQ2->cfg.rwLockParam_syncFlg.uiMilliSeconds_mutex_r,  pQ2->hSema_syncFlg,  pQ2->cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_r  )  )  return  NULL;
            #else
                if  (  this->rLock(  pQ2->pRw_syncFlg,  pQ2->cfg.rwLockParam_syncFlg.uiMilliSeconds_sema_r  )  )  return  NULL;
            #endif
                */

                /*
                m_var.pQ_toPostMsg  =  pQ2->bFlag_toPostMsg_q1  ?  &pQ2->qs[1]  :  &pQ2->qs[0];
                m_var.pQ_toGetMsg  =  pQ2->bFlag_toPostMsg_q1  ?  &pQ2->qs[0]  :  &pQ2->qs[1];
                */
                if (pQ2.bFlag_toPostMsg_q1)
                {
                    m_pQ_toGetMsg = pQ2.qs0;
                    m_pQ_toPostMsg = pQ2.qs1;
                }
                else
                {
                    m_pQ_toGetMsg = pQ2.qs1;
                    m_pQ_toPostMsg = pQ2.qs0;
                }

                return m_pQ_toGetMsg;

            }


            public int qPostMsgAndTrigger(IntPtr pHeadData,IntPtr pQElem, uint size, QY_qBuf2 pQ2)
            {

                int tmpiRet = 0;
                bool ucbTriggerErr = false;

                if (pQ2 == null) return -1;

                if (pQElem != IntPtr.Zero && size != 0)
                {
                    QY_qBuf q_toPostMsg = getQ_toPostMsg(pQ2);
                    if (q_toPostMsg != null)
                    {
                        tmpiRet = q_toPostMsg.qPostMsg(pHeadData, pQElem, null,size);
                        if ( tmpiRet !=0)
                        {
                            //traceLogA("qyQ2Help::qPostMsgAndTrigger: qPostMsg returns %d", tmpiRet);
                        }
                    }
                }

                try
                {
#if false
                    //if (pQ2->hSemaTrigger1 && !ReleaseSemaphore(pQ2->hSemaTrigger1, 1, &pQ2->lPrevCnt_semaTrigger))
                    if (pQ2.hSemaTrigger1 != null)
                    {
                        qyFuncs.mySemaRelease(pQ2.hSemaTrigger1, 1);

                        //qyDisplayLastError("ReleaseSemaphore failed");

                        //ucbTriggerErr = true;

                        //  traceLogA(  "%S qPostMsgAndTrigger err: uiQNodes %d, lPrevCnt_semaTrigger %d",  pQ2->cfg.name,  pQ->uiQNodes,  pQ2->lPrevCnt_semaTrigger  );
                    }
                    else
                    {
                    }
#endif
                    if (pQ2.hEvent0 != null)
                    {
                        //SetEvent(pQ2->hEvents[0]);
                        qyFuncs.myEventSet(pQ2.hEvent0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            //  2008/11/21
#if false
                if (ucbTriggerErr != pQ2.ucbTriggerErr)
                {
                    pQ2.ucbTriggerErr = ucbTriggerErr;
                }
#endif

            errLabel:

                return tmpiRet;

            }

        };



    }


}
