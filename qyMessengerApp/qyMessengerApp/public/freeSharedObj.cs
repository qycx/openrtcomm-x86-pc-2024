using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{


		public static unsafe int freeSharedObjByIndex(object hWnd, int iWndContentType, int index)
		{
			int iErr = -1;

			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null==pProcInfo) return -1;
			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;
			MIS_CNT pMisCnt = pProcInfo.pMisCnt;
			
			//
			QY_SHARED_OBJ pSharedObj = null;
			//TCHAR tHint[128] = _T("");      //  2014/05/27
			string tHint = "";

			//
			bool bDbg = true;
			string str;

			if (bDbg)
            {
				traceLog("freeSharedObjByIndex enters");
            }

			//
			if (index < 0 || index >= pQmcCfg.usMaxCnt_pSharedObjs) return -1;
			//
			pSharedObj = getSharedObjByIndex(pProcInfo, index); //  &pProcInfo.pSharedObjs[index];

			//
			//  2014/05/28
			str=string.Format("freeSharedObjByIndex {0} {1} enters", index, qyGetDesByType1(qyFuncs.CONST_sharedObjTypeTable, pSharedObj.var.uiType));
			//  2012/11/06
			showInfo_open(0, null, str);


			//if (bSharedObjActive(pProcInfo, index)) return -1;

			/*
			using (CQySyncObj syncObj = new CQySyncObj()) {				//
				if (syncObj.sync(new string(pQmcCfg.mutexName_syncStartAv),pMisCnt.hMutex_syncStartAv))
				{
					//  2012/11/06
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("freeSharedObjByIndex %d, sync failed"), index);
					goto errLabel;
				}

			}
			*/



			//  2014/09/29
			if (pSharedObj.var.bSlave)
			{
				removeSlaveFromMaster(pProcInfo, index, pSharedObj.var.slave.masterKey.iIndex_sharedObj);
				if (bExists_master(pProcInfo, index)) goto errLabel;
			}

			/*
			//  2014/10/31
			switch (pSharedObj.usSubtype)
			{
				case CONST_sharedObjSubtype_mosaic:
					{
						pProcInfo.removeMosaicFromD3dWall(index);
						BOOL tmp_bExists = false;
						if (pProcInfo.getVal_bExists_mosaic(index, &tmp_bExists)) goto errLabel;
						if (tmp_bExists) goto errLabel;
					}
					break;
				default:
					break;
			}
			*/

			//  2014/05/28
			//_sntprintf(tHint, mycountof(tHint), _T("freeSharedObjByIndex %d %s enters"), index, qyGetDesByType1(CONST_sharedObjTypeTable, pSharedObj.uiType));
			//  2012/11/06
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("%s"), tHint);



			//  2013/08/23
			if (pSharedObj.var.bSoundRecorder)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("freeSharedObj: try to stopLocalAudioRecorder"));
				pProcInfo.stopLocalAudioRecorder(index, 100);
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("stopLocalAudioRecorder ends"));

				//
				CAP_procInfo_audioCommon pU = getCapAudioBySth(pProcInfo, pSharedObj.var.iIndex_capAudio, 0);
				if (pU!=null)
				{

					//
					
					if (pU.compressAudio.bCompressInited)
					{
						qyExitProcess(_T("freeSharedObjByIndex failed: can't wait for audio recorder."));
					}


					//  2013/03/21
					//memset(&pU.recordSound, 0, sizeof(pU.recordSound)  );
					pU.memset0();

				}
			}


			//
			if (!pSharedObj.var.bCapDev)
			{
				
				int i;

#if false
				switch (pSharedObj.var.uiType)
				{
					case Consts.CONST_sharedObjType_mediaFile:
						if (pSharedObj.pShareMediaObj)
						{
#if __DEBUG__
							traceLogA("freeSharedObjByIndex: mediaFile: exitShareMediaFile %d", index);
#endif
							if (exitShareMediaFile(&pSharedObj.pShareMediaObj)) goto errLabel;
						}
						break;
					case CONST_sharedObjType_av:
					case CONST_sharedObjType_mediaDevice:
						if (pSharedObj.pShareMediaObj)
						{
#if __DEBUG__
							traceLogA("freeSharedObjByIndex: mediaDevice: exitShareMediaDevice %d", index);
#endif
							if (exitShareMediaDevice(&pSharedObj.pShareMediaObj)) goto errLabel;
						}
						break;
					case CONST_sharedObjType_qvcf:
						if (pSharedObj.pShareMediaObj)
						{
							exitQvcf_rtsp(pProcInfo, &pSharedObj.pShareMediaObj);

						}
						break;
					case CONST_sharedObjType_webcam_av:  //  2015/05/18
						if (pSharedObj.pShareMediaObj)
						{
							exitSharedMediaObj(pProcInfo, pSharedObj);
						}
						break;
					case CONST_sharedObjType_rtsp:
						{//  2015/10/11
							exitObj_rtsp(pProcInfo, index);
						}
						break;
					default:
						break;
				}
#endif

				//
				for (i = 0; i < pQmcCfg.usMaxCnt_capProcInfoBmps; i++)
				{
					CAP_procInfo_bmpCommon pU = getCapBmpBySth(pProcInfo, i, 0);
					if (null!=pU && pU.uiTranNo_sharedObj == pSharedObj.var.uiTranNo)
					{

						//  2011/12/04
						stopQThread(pU.vppThread, tHint);
						//
						stopQThread(pU.thread, tHint);
						stopQThread(pU.postThread, tHint);
						//
						if (pU.vppThread.bQInited)
						{
							exitQyQ2(pU.vppThread.q2); pU.vppThread.bQInited = false;
						}
						//  2009/12/07
						if (pU.thread.bQInited)
						{
							exitQyQ2(pU.thread.q2); pU.thread.bQInited = false;
						}
						//  2011/12/06
						if (pU.postThread.bQInited)
						{
							exitQyQ2(pU.postThread.q2); pU.postThread.bQInited = false;
						}

						//
						//  2009/04/28
						//
						//  2015/05/15. 这个清理应该是通用的吧
						exitOrg2IntBuf(pProcInfo, pU);  //  2014/09/23

						//
#if __DEBUG__
						traceLogA("freeSharedObjByIndex: av: exitCompressVideo %d", i);
#endif
						pProcInfo.exitCompressVideo(pSharedObj.var.bCapDev, ref pU.compressVideo);

						//  2013/03/21
						//memset(pU, 0, sizeof(pU[0]));
						pU.memset0();
					}
				}
				for (i = 0; i < pQmcCfg.usMaxCnt_capProcInfoAudios; i++)
				{
					CAP_procInfo_audioCommon pU = getCapAudioBySth(pProcInfo, i, 0);
					if (null!=pU && pU.uiTranNo_sharedObj == pSharedObj.var.uiTranNo)
					{

						//  2017/01/29
						stopQThread(pU.appThread, tHint);
						//  2009/12/07
						stopQThread(pU.thread, tHint);
						//
						//  2017/01/29
						if (pU.appThread.bQInited)
						{
							exitQyQ2(pU.appThread.q2); pU.appThread.bQInited = false;
						}
						//
						if (pU.thread.bQInited)
						{
							exitQyQ2(pU.thread.q2); pU.thread.bQInited = false;
						}
						//
#if __DEBUG__
						traceLogA("freeSharedObjByIndex: mediaFile: exitCompressAudio");
#endif
						exitCompressAudio(pProcInfo, ref pU.compressAudio);

						//  2013/03/21
						//memset(pU, 0, sizeof(pU[0]));
						pU.memset0();
					}
				}

#if false
				if (pSharedObj.hProcess)
				{
					CloseHandle(pSharedObj.hProcess); pSharedObj.hProcess = NULL;
				}
				if (pSharedObj.dataShm.pBuf)
				{
					UnmapViewOfFile(pSharedObj.dataShm.pBuf); pSharedObj.dataShm.pBuf = NULL;
				}
				if (pSharedObj.dataShm.hMap)
				{
					CloseHandle(pSharedObj.dataShm.hMap); pSharedObj.dataShm.hMap = NULL;
				}
				QM_SHM_CMD* pShmCmd = (QM_SHM_CMD*)pProcInfo.getQmShmCmdByIndex(pSharedObj.iIndex_shmCmd);
				if (pShmCmd) memset(pShmCmd, 0, sizeof(pShmCmd[0]));
#endif


			}
			else
			{
				CAP_procInfo_video pU = (CAP_procInfo_video)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
				uint uiCapType = pU.uiType;

				//
				if ( bDbg)
                {
					traceLog("freeSharedObjByIndex: before stopCapture");
                }

				//
				stopCapture(hWnd,iWndContentType,pU); //  2011/07/29

				//  2011/12/04
				stopQThread(pU.vppThread, tHint);
				//
				stopQThread(pU.thread, tHint);
				stopQThread(pU.postThread, tHint);

				//
				closeLocalVideo(hWnd,iWndContentType, index);

				//
				if (pU.vppThread.bQInited)
				{
					exitQyQ2(pU.vppThread.q2); pU.vppThread.bQInited = false;
				}
				//
				if (pU.thread.bQInited)
				{
					exitQyQ2(pU.thread.q2); pU.thread.bQInited = false;
				}
				//  2011/12/06
				if (pU.postThread.bQInited)
				{
					exitQyQ2(pU.postThread.q2); pU.postThread.bQInited = false;
				}

				//
				/*
				for (int j = 0; j < mycountof(pSharedObj.usrs); j++)
				{
					if (!pSharedObj.usrs[j].iTaskId) continue;
					notifyCapEnd(pProcInfo.pQyMc, uiCapType, pSharedObj.iIndex_capBmp, pSharedObj.usrs[j].iTaskId, pSharedObj.usrs[j].hWnd_starter);
					if (pProcInfo.hWnd_testWebcam == pSharedObj.usrs[j].hWnd_starter)
					{
						pProcInfo.hWnd_testWebcam = NULL;
					}
				}
				*/

				//  2013/03/21
				//memset(pU, 0, sizeof(pU[0]));
				pU.memset0();
			}

			//
			/*
			if (pSharedObj.pTransInfo_stream!=null)
			{
				TRANSINFO_stream* pTransInfo = (TRANSINFO_stream*)pSharedObj.pTransInfo_stream;
				exitTransforms(pProcInfo, &pTransInfo->myTask);
				MACRO_safeFree(pTransInfo->myTask.pMems_from);
			}
			MACRO_safeFree(pSharedObj.pTransInfo_stream);
			*/

			//
			if (!pSharedObj.var.bSlave)
			{
				/*
				if (pSharedObj.master.pShareDynBmp_slave)
				{
				}
				MACRO_safeFree(pSharedObj.master.pShareDynBmp_slave);
				*/
			}

			//
			//memset(pSharedObj, 0, sizeof(pSharedObj[0]));
			pSharedObj.memset0();

			iErr = 0;

		errLabel:

			str = string.Format("freeSharedObjByIndex {0} leaves with {1}", index, 0 != iErr ? _T("Failed") : _T("OK"));
			showInfo_open(0, null, str);


			if (bDbg)
			{
				traceLog("freeSharedObjByIndex leaves");
			}
			//  2012/11/06
			//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("freeSharedObjByIndex %d leaves with %s"), index, iErr ? _T("Failed") : _T("OK"));

			return iErr;
		}

	}
}
