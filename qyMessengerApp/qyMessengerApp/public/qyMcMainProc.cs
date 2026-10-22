using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;

namespace qy
{


    public class Param_tmp_showState_task
    {
        public ushort usStateType;
        public ushort usStateSubtype;
        public mySYSTEMTIME when;
        public int iPos;
    };



    public unsafe class PARAM_findOrgMsg
    {
        public bool bNeedUpdate;
        //
        //public void* pMsgTask_o;
        public CMIS_MSGU pMsgTask_o;

        //
        //public TRANSFER_FILEDATA_REQ* pFileDataReq_o;
        public CTRANSFER_FILEDATA_REQ pFileDataReq_o;

        //
        //public TRANSFER_FILEDATA_RESP* pFileDataResp_o;
        public CTRANSFER_FILEDATA_RESP pFileDataResp_o;

        //
        //public IM_CONTENTU* pContent_o;
        public CIM_CONTENTU pContent_o;

        //
        public TRANSFER_FILEEND_REQ* pTransferFileEndReq;
        //public TRANSFER_FILEEND_RESP* pTransferFileEndResp;
        public CTRANSFER_FILEEND_RESP pTransferFileEndResp;

        //
        //public TASK_PROC_RESP* pTaskProcResp;


        //
        public COMMON_PARAM commonParam0;
        public COMMON_PARAM commonParam1;

        //
        public int iTaskId;

        //
        public Param_tmp_showState_task param_tmp_showState_task;

        //
        public string hint;

        //
        public bool bBreak;

    }
    ;


    //
    partial class qyFuncs
    {
        public static unsafe bool bChkQyMcEnv(CCtxQyMc pQyMc)
        {
            int size;

            int bufSize = Consts.bufSize_routeTalkData;
            size = sizeof(IM_CONTENTU);
            if ( bufSize!=sizeof(IM_CONTENTU))
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_routeTalkData error");
                Thread.CurrentThread.Abort();

            }
            size = sizeof(IM_GRP_INFO);
            if (Consts.CONST_sizeof_imGrpInfo < size)
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_imGrpInfo error");
                Thread.CurrentThread.Abort();

            }
            size = sizeof(IM_GRP_MEM);
            if (Consts.CONST_sizeof_imGrpMem < size)
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_imGrpMem error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(RETRIEVE_contact_mem);
            if (Consts.CONST_sizeof_retrieveContaceMem < size)
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_retrieveContaceMem error");
                Thread.CurrentThread.Abort();

            }
            size = sizeof(REFRESH_imObjRule_reqMem);
            if (Consts.CONST_sizeof_refreshImObjRuleReqMem < size)
            {
                Console.WriteLine("bChkQyMcEnv: CONST_sizeof_refreshImObjRuleReqMem error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(TRANSFER_VIDEO_dataMemHead);
            if (Consts.CONST_sizeof_TRANSFER_VIDEO_dataMemHead < size)
            {
                Console.WriteLine("bChkQyMcEnv: CONST_sizeof_TRANSFER_VIDEO_dataMemHead error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(myDRAW_VIDEO_DATA);
            if (Consts.CONST_sizeof_myDRAW_VIDEO_DATA < size)
            {
                Console.WriteLine("bChkQyMcEnv: CONST_sizeof_myDRAW_VIDEO_DATA error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(TRANSFER_AUDIO_dataMemHead);
            if (Consts.CONST_sizeof_TRANSFER_AUDIO_dataMemHead < size)
            {
                Console.WriteLine("bChkQyMcEnv: CONST_sizeof_TRANSFER_AUDIO_dataMemHead error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(myPLAY_AUDIO_DATA);
            if (Consts.CONST_sizeof_myPLAY_AUDIO_DATA < size)
            {
                Console.WriteLine("bChkQyMcEnv: CONST_sizeof_myPLAY_AUDIO_DATA error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(VIDEO_CONFERENCE_MEM);
            if (Consts.sizeof_videoConferenceMem < size)
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_videoConferenceMem error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(CONF_requesting_mem);
            if (Consts.sizeof_CONF_requesting_mem < size)
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_requesting_mem error");
                Thread.CurrentThread.Abort();
            }
            size = sizeof(IpcInfo);
            if (Consts.sizeof_ipcInfo < size)
            {
                Console.WriteLine("bChkQyMcEnv: sizeof_ipcInfo error");
                Thread.CurrentThread.Abort();

            }



            //
            return true;

        }

        public static unsafe int getDynCfg_isClient(CCtxQyMc pQyMc, ref QNM_CNT_CFG pDynCfg)
        {
            int iErr = -1;
            //QNM_CNT_CFG cntCfg_sys;
            //QNM_CNT_CFG cntCfg;
            int i;
            //
            //if (!pDynCfg) return -1;

            fixed (QNM_CNT_CFG* pCfg = &pDynCfg)
            {

                pCfg->usCntPort = 8768;
                //safeStrnCpy("127.0.0.1", pCfg->cntIp, Consts.CONST_qyMaxIpLen + 1);			
            }
            //mymemset(&pDynCfg, 0, sizeof(QNM_CNT_CFG));
            //memset(&cntCfg_sys, 0, sizeof(cntCfg_sys));
            //memset(&cntCfg, 0, sizeof(cntCfg));
            //
            /*
			if (getQnmCntCfg(0, FALSE, pQyMc->iServiceId, HKEY_LOCAL_MACHINE, CQyString(pQyMc->cfg.pSysCfg->rootKey_qnmScheduler), &cntCfg_sys)) goto errLabel;
			if (!bSupported_devAuth())
			{
				if (getQnmCntCfg(0, FALSE, pQyMc->iServiceId, HKEY_CURRENT_USER, CQyString(pQyMc->cfg.pSysCfg->rootKey_qnmScheduler), &cntCfg)) goto errLabel;
			}
			//
			if (bIpValid(cntCfg_sys.cntIp))
			{
				safeStrnCpy(cntCfg_sys.cntIp, pDynCfg->cntIp, mycountof(pDynCfg->cntIp));
				pDynCfg->ucbSys_cntIp = TRUE;
			}
			else if (bIpValid(cntCfg.cntIp))
			{
				safeStrnCpy(cntCfg.cntIp, pDynCfg->cntIp, mycountof(pDynCfg->cntIp));
			}
			//
			for (i = 0; i < mycountof(cntCfg.cntAddrs); i++)
			{
				if (cntCfg_sys.cntAddrs[i][0])
				{
					safeStrnCpy(cntCfg_sys.cntAddrs[i], pDynCfg->cntAddrs[i], mycountof(pDynCfg->cntAddrs[i]));
					pDynCfg->ucbSys_cntAddrs[i] = TRUE;
				}
				else if (cntCfg.cntAddrs[i][0])
				{
					safeStrnCpy(cntCfg.cntAddrs[i], pDynCfg->cntAddrs[i], mycountof(pDynCfg->cntAddrs[i]));
				}
			}

			//  
			if (cntCfg_sys.usCntPort)
			{
				pDynCfg->usCntPort = cntCfg_sys.usCntPort;
				pDynCfg->ucbSys_cntPort = TRUE;
			}
			else if (cntCfg.usCntPort)
			{
				pDynCfg->usCntPort = cntCfg.usCntPort;
			}
			else
			{
				pDynCfg->usCntPort = qnmCntPort(pQyMc->iServiceId);
			}

			//  2010/07/27
			if (pQyMc->cfg.bGetPolicyFromIni)
			{
				char buf[32];

				if (!getCfgValByName(pQyMc->cfg.iniFullFileName, CONST_regValName_cntIp, buf, sizeof(buf)))
				{
					trim(buf);
					if (bIpValid(buf))
					{
						safeStrnCpy(buf, pDynCfg->cntIp, mycountof(pDynCfg->cntIp));
					}
				}
				if (!getCfgValByName(pQyMc->cfg.iniFullFileName, CONST_regValName_cntAddr1, buf, sizeof(buf)))
				{
					tri(buf);
					if (buf[0])
					{
						safeStrnCpy(buf, pDynCfg->cntAddrs[0], mycountof(pDynCfg->cntAddrs[0]));
					}
				}
				//
				if (!getCfgValByName(pQyMc->cfg.iniFullFileName, CONST_regValName_cntPort, buf, sizeof(buf)))
				{
					trim(buf);
					if (buf[0])
					{
						pDynCfg->usCntPort = atol(buf);
					}
				}
				if (!pDynCfg->usCntPort) pDynCfg->usCntPort = qnmCntPort(pQyMc->iServiceId);

			}
			*/

            iErr = 0;
        errLabel:
            return iErr;
        }


        static void exitAllDaemonThreads(CCtxQyMc pQM)
        {
            int i;
            string str;

            //
            CCtxQmc pProcInfo = pQM.get_pProcInfo();

            //
            traceLogA("exitAllDaemonThreads(  ) enters.");

            //
            if (pQM.bLogon)
            {
                //traceLogA("exitAllDaemonThreads: ÕâÀïµÄbQuit±êÖ¾ÎªÊ²Ã´Ã»ÉèÖÃÄØ");
                Console.WriteLine("Err: exitAllDaemonThreads: bLogon is true");
            }

            //
            mySetEvent(pProcInfo.hEvt_notify_thread_isCli);

            //
            for (i = 0; i < pQM.hDaemonThreads.Length; i++)
            {
                if (pQM.hDaemonThreads[i] == null) continue;
#if false
				for (int j = 0; j < 100; j++)
				{
 
					if (pQM.hDaemonThreads[i].IsAlive)
					{
						Console.WriteLine("Waiting for daemon thread {0}", i);
						//waitForObject(&pQM->hDaemonThreads[i], 20000);
						Thread.Sleep(100);
						continue;
					}
					//
					pQM.hDaemonThreads[i] = null;
					//
					str = string.Format("daemon thread {0} waited", i);
					showInfo_open(0, null, str);
					//
					break;
				}
#endif
                //
                GuiShare.pf_waitForThread1(ref pQM.hDaemonThreads[i], 20000);

                //
                continue;
            }



            traceLogA("exitAllDaemonThreads(  ) leaves.");

            return;
        }



    }
}
