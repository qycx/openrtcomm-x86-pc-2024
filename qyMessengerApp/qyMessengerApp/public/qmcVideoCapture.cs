
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//
partial class Consts
{
    public const int CONST_videoCompressors_null = 0;
    public const int CONST_videoCompressors_vcm = 1;    //  "vcm"
    public const int CONST_videoCompressors_dmo = 2;    //  "dmo"
    public const int CONST_audioCompressors_acm = 3;    //  "acm"					//  2009/05/16
    public const int CONST_audioCompressors_dmo = 4;    //  "dmo"					//  2009/05/16
    public const int CONST_videoCompressors_custom = 5; //  "custom"				//  2010/04/28
    public const int CONST_audioCompressors_custom = 6; //  "custom"				//  2010/04/28
                                                        //
    public const int CONST_audioCompressors_ipp = 7;    //
    public const int CONST_videoCompressors_ipp = 8;    //	"h264"					//  2011/07/30
                                                        //
    public const int CONST_audioCompressors_hwAccl = 9; //
    public const int CONST_videoCompressors_hwAccl = 10;    //	"h264"					//  2011/11/25
                                                            //
                                                            //
                                                            //  13
    public const int CONST_videoCompressors_stream = 14;    //  "h264"					//  for rtsp. 2014/02/25
                                                            //
    public const int CONST_audioCompressors_vorbis = 15;    //
    public const int CONST_videoCompressors_vp8 = 16;   //	"vp8"					//  2014/07/19

    //
    public const int CONST_audioCompressors_ffmpeg = 21;
    public const int CONST_videoCompressors_ffmpeg = 22;







}



//
namespace qy
{

    public struct SMPL_bsRead_param
    {
        public uint dwTickCnt_doPretrans;
    }
    ;


    partial class qyFuncs
    {
        public static unsafe int qmcChkTasks_gui(CCtxQmc pProcInfo)           //  2009/09/10
        {
            /*
			if (!pProcInfo) return -1;
			return pProcInfo.qisChkTasks_gui();
			*/
            return 0;
        }




        public static unsafe int qmcApplyForRemovingInvalidTasks(CCtxQmc pProcInfo, uint uiChannelType)
        {
            /*
			if (!pProcInfo) return -1;
			return pProcInfo.applyForRemovingInvalidTasks(uiChannelType);
			*/
            return 0;
        }

        public static unsafe int org2Rgb(CCtxQmc pProcInfo, CAP_procInfo_bmpCommon pVc, byte* pInput, byte* pOrg2RgbBuf)
        {
            return -1;
        }

        //
        public static unsafe int refreshWebCamSettings(ref VIDEO_COMPRESSOR_CFG pVideoCompressorCfg, CAP_procInfo_video pVc)
        {
            //QY_MC* pQyMc = QY_GET_GBUF();

            //memset(&pVc->policy, 0, sizeof(pVc->policy)  );
            fixed (CAP_procInfo_video_policy* p = &pVc.policy)
            {
                qyFuncs.mymemset((IntPtr)p, 0, sizeof(CAP_procInfo_video_policy));
            }

            /*
			if (pVideoCompressorCfg.dev.ucbSetDevResolution)
			{   //  2014/04/26
				pVc->policy.bih.biWidth = pVideoCompressorCfg->dev.iWidth;
				pVc->policy.bih.biHeight = pVideoCompressorCfg->dev.iHeight;
			}
			
			else
			*/
            {
                pVc.policy.bih.biWidth = pVideoCompressorCfg.common.head.iWidth_pic;
                pVc.policy.bih.biHeight = pVideoCompressorCfg.common.head.iHeight_pic;
            }



#if false
	pVc->policy.bih.biCompression  =  CONST_fourcc_I420;	//  YUY2;
	myMessageBox(  NULL,  _T(  "Warning: refreshWebCampSettings. video format is fixed."  ),  _T(  "Qycx"  ),  MB_OK  );
#endif

            return 0;
        }



        //  
        public static unsafe int openLocalVideo(CCtxQmc pProcInfoCommon, uint uiCamCapType, string displayName, object hTalk, int iWndContentType, ref VIDEO_COMPRESSOR_CFG pVideoCompressor, int iIndex_sharedObj)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            PolicyAvParams pAvParams = (PolicyAvParams)pProcInfoCommon.p_gAvParams;  //  QY_GET_PolicyAvParams(  );
            if (null == pAvParams) return -1;

            int usCapDriverIndex = 0;
            //TCHAR devName[100] = ("");
            //TCHAR devVersion[100] = ("");
            //  
            int iIndex_capBmp = -1;
            uint uiTranNo_sharedObj = 0;
            //
            //IMoniker* pMoniker = NULL;
            //TCHAR friendlyName[256] = ("");

            //TCHAR tBuf[128] = ("");

            //  2014/09/10
            //
            /*
#if !defined(  __WINCE__  )  ||  defined(  __TEST__  )
			//  enumAvDevices(  NULL,  NULL,  devicePath,  &pMoniker  );	
			if (getMonikerByDisplayName(displayName, (void**)&pMoniker)
				|| !pMoniker)
			{
# ifdef  __DEBUG__
				traceLogA("openLocalVideo failed:  pMoniker null");
#endif
				qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("openLocalVideo failed, pMoniker null"));
				return -1;
			}

			//  2012/02/22
			getMonikerFriendlyName(pMoniker, friendlyName, mycountof(friendlyName));
#endif
			*/

            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("start to openLocalVideo: %s"), friendlyName);
#if DEBUG
			qyFuncs.traceLog(("openLocalVideo enters ")  +  displayName );
#endif


            if (null == pProcInfoCommon) return -1;
            CCtxQmc pProcInfo = pProcInfoCommon;
            QY_SHARED_OBJ pSharedObj = null;
            CAP_procInfo_video pVc = null;


            pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
            if (null == pSharedObj) goto errLabel;
            pSharedObj.var.bCapDev = true;

            int iIndex_sharedObj_used = -1;
            /*
			if (pProcInfo.bWebcamUsing(0, pMoniker, null, null, &iIndex_sharedObj_used)
				&& iIndex_sharedObj_used != iIndex_sharedObj)
			{
# if  __DEBUG__
				traceLogA("openLocalVideo failed. webcam is already used.");
#endif

				_sntprintf(tBuf, mycountof(tBuf), ("Failed: webcam is used"));
				showNotification(null, null, null, 0, 0, 0, tBuf);

				goto errLabel;
			}
			*/

            //
            iIndex_capBmp = pSharedObj.var.iIndex_capBmp;
            uiTranNo_sharedObj = pSharedObj.var.uiTranNo;      //  2011/05/31

            //
            pVc = (CAP_procInfo_video)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
            if (null == pVc) goto errLabel;


            if (pVc.bCapDevConnected)
            {
#if __DEBUG__
				traceLogA("openAv: capDriver already is connected");
#endif
                goto errLabel;
            }

            //
            pSharedObj.var.hMutex_syncEncFrame = new System.Threading.Mutex();
            string evtName = string.Format("encFrame{0}", iIndex_sharedObj);
            //pSharedObj.var.hEvt_notifyFrameAvail = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, evtName);
            pSharedObj.var.hEvt_notifyFrameAvail = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);



            //
            pVc.uiType = Consts.CONST_capType_av;
            pVc.iIndex_sharedObj = iIndex_sharedObj;
            pVc.uiCamCapType = uiCamCapType;

            //  2017/09/13
            //safeTcsnCpy(displayName, pVc.displayName, mycountof(pVc.displayName));
            pVc.displayName = displayName;
            pVc.bCaptureStarted_openLocalVideo = false;

            //  2014/09/10

            //  if  (  !pVc.policy.bih.biWidth  ||  !pVc.policy.bih.biHeight  )  
            {
                refreshWebCamSettings(ref pVideoCompressor, pVc);
            }

            switch (uiCamCapType)
            {
#if false
				case CONST_camCapType_directX:

					if (initVideoCapDev_dx(null, displayName, ppCapStuff, &pVc.policy.bih, false, pQyMc->gui.hMainWnd, iIndex_sharedObj, &pVc.capDev.pVideoCapDev)) goto errLabel;
					pVc.bCapDevConnected = true;

					if (!bGetCapturePara_dx(pProcInfoCommon, iIndex_capBmp, BufferCB_av, sizeof(MIS_MSGU), pVc.capDev.pVideoCapDev, &pVc.vh_org)) goto errLabel;

					break;
#endif
                case Consts.CONST_camCapType_android:
                    //
                    qyFuncs.traceLog("openLocalVideo.249, before initVideoCapDev");

                    //
                    if (0 != GuiShare.pf_initVideoCapDev(hTalk, iWndContentType, "", ref pVc.policy.bih, pVc))
                    {
                        qyFuncs.traceLog("openLocalVideo.254, initVideoCapDev failed");
                        goto errLabel;
                    }
                    pVc.bCapDevConnected = true;

                    //
                    qyFuncs.traceLog("openLocalVideo.260, after initVideoCapDev");

                    //
                    if (!GuiShare.pf_bGetCapturePara(hTalk, iWndContentType, ref pVc.vh_org)) goto errLabel;

                    //
                    qyFuncs.traceLog("openLocalVideo.266, after bGetCapturePara");

                    //
                    break;
                default:
#if DEBUG
                    string str;
                    str = string.Format(("openLocalVideo failed: unknown uiCamCapType {0}"), uiCamCapType);
                    traceLog(str);
#endif
                    goto errLabel;
            }
            if (!pVc.bCapDevConnected) goto errLabel;
            //  2017/09/15

            //  2011/08/13
            if (pVc.policy.bih.biWidth != 0 && pVc.policy.bih.biHeight != 0)
            {
                if (  //  pVc.vh_org.bih.biWidth  !=  pVc.policy.bih.biWidth  ||  
                    pVc.vh_org.bih.biHeight != pVc.policy.bih.biHeight)
                {
                    //_sntprintf(tBuf, mycountof(tBuf), ("%s: %dx%d"), getResStr(0, &pQyMc->cusRes, CONST_resId_cantSupportResolution), pVc.policy.bih.biWidth, pVc.policy.bih.biHeight);
                    //showNotification(null, null, null, 0, 0, 0, tBuf);
                }
            }

            //
            bool bOk_org = false;
#if false
			if ( (pVc.vh_org.bih.biCompression == Consts.BI_RGB && pVc.vh_org.bih.biBitCount == 24)
				|| pVc.vh_org.bih.biCompression==Consts.CONST_fourcc_I420  )
            {
				bOk_org = true;
            }
#endif
            if (!bOk_org)
            {
                if (0 != initOrg2IntBuf(pProcInfoCommon, ref pVideoCompressor, (CAP_procInfo_bmpCommon)pVc))
                {
#if __DEBUG__
					traceLogA("initOrg2RgbBuf failed.");
#endif
                    goto errLabel;
                }
            }

            //
            QY_VIDEO_HEADER pVh_dec = pVc.bOrg2IntBufInited ? pVc.vh_int : pVc.vh_org;

            //
            //  2010/09/13
            //
            //
            //
            {
                //  2014/04/03
                pVideoCompressor.common.pVideoQ2 = pVc.thread.q2;
                pVideoCompressor.common.pParent_transform = pVc;

                //
                pVideoCompressor.index_sharedObj = iIndex_sharedObj;

                //
                pVideoCompressor.m_bFront = pVc.m_bFront;

                //
                //
                qyFuncs.traceLog("openLocalVideo.333, before initCompressVideo");


                //  2016/04/02
                //_sntprintf(pVc.compressVideo.tag_dbg, mycountof(pVc.compressVideo.tag_dbg), ("capBmp%d"), iIndex_capBmp);
                //
                if (0 != pProcInfoCommon.initCompressVideo((BITMAPINFOHEADER*)&pVh_dec.bih, Consts.CONST_capType_av, ref pVideoCompressor, true, 0, ref pVc.compressVideo))
                {
                    //
                    qyFuncs.traceLog("openLocalVideo.342, initCompressVideo failed");

                    /*
					//TCHAR tBuf[128] = ("");
					CUS_MODULE_U* pModule = null;

					//_sntprintf(tBuf, mycountof(tBuf), ("openLocalVideo: initCompressVideo failed, %s, fourcc %s."), qyGetDesByType1(pAvParams->pVideoCompressorsTable, pVideoCompressor->common.ucCompressors), CQyString(pVideoCompressor->common.fourccStr));
					if (pVideoCompressor->common.ucCompressors == CONST_videoCompressors_hwAccl)
					{
						pModule = pQyMc->cusModules.pMediaSdk_enc;
						if (pModule
							&& pModule->common.hModule
							&& pModule->compress.mgr.ucbNotSupported_hw)
						{
							_sntprintf(tBuf, mycountof(tBuf), ("%s Hardware accelerator not supported"), tBuf);
						}
					}
					//  qyShowHint(  (  "%s"  ),  tBuf  );  
					//  myMessageBox(  null,  tBuf,  (  "www.qycx.com"  ),  MB_OK  );
					//showNotification(0, 0, 0, 0, 0, 0, tBuf);
					*/
                    goto errLabel;
                }

            }

            //
            qyFuncs.traceLog("openLocalVideo.369, after initCompressVideo");


            //
#if false
			char fourccStr_org[5 + 1]; fourccStr_org[0] = 0;
			iFourcc2Str(pVc.vh_org.bih.biCompression, fourccStr_org, mycountof(fourccStr_org));
			qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("w [%d], h [%d], org [%s],  sizeImg [%d]"), pVc.vh_org.bih.biWidth, pVc.vh_org.bih.biHeight, CQyString(fourccStr_org), pVc.vh_org.bih.biSizeImage);
			qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("rgb [%d]"), pVc.vh_dec.bih.biSizeImage);
			qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("fourcc [%s], com [%d], Quality [%d]"), CQyString(pVc.compressVideo.compressor.common.fourccStr), pVc.compressVideo.iCompressBufSize, pVc.compressVideo.u.vcm.iQuality);
#endif

            iErr = 0;

        errLabel:

            //MACRO_safeRelease(pMoniker);

            if (0 != iErr)
            {
            }
            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("openLocalVideo %s"), (iErr ? ("failed") : ("OK")));

#if DEBUG
			qyFuncs.traceLog("openLocalVideo leaves with "  +  iErr  );
#endif

            return iErr;

        }


        //
        public static unsafe int closeLocalVideo(object hWnd, int iWndContentType, int iIndex_sharedObj)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;//QY_GET_GBUF(  );
            if (null == pQyMc) return -1;

            CCtxQmc pProcInfoCommon = pQyMc.get_pProcInfo();
            if (null == pProcInfoCommon) return -1;
            int iIndex_capBmp = -1;

            //
#if __DEBUG__
			traceLogA("closeLocalVideo enters, index_sharedObj %d", iIndex_sharedObj);
#endif
            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfoCommon.who_showInfo, 0, (""), (""), ("closeLocalVideo enters. index_sharedObj %d"), iIndex_sharedObj);



            CCtxQmc pProcInfo = pProcInfoCommon;
            QY_SHARED_OBJ pSharedObj = null;
            CAP_procInfo_video pVc = null;

            pSharedObj = getSharedObjByIndex(pProcInfo, iIndex_sharedObj);
            if (null == pSharedObj) goto errLabel;
            //
            iIndex_capBmp = pSharedObj.var.iIndex_capBmp;

            //
            pVc = (CAP_procInfo_video)getCapBmpBySth(pProcInfo, pSharedObj.var.iIndex_capBmp, 0);
            if (null == pVc) goto errLabel;

            //  2017/09/13
            pVc.bCaptureStarted_openLocalVideo = false;

            //
            stopCapture(hWnd, iWndContentType, pVc);
            if (pVc.bCaptureStarted)
            {
#if __DEBUG__
				traceLogA("closeLocalVideo failed: capture is not stopped.");
#endif
                goto errLabel;
            }
            if (pVc.bCapDevConnected)
            {
                // Finally disconnect the driver
                switch (pVc.uiCamCapType)
                {
#if false
					case CONST_camCapType_directX:
						if (exitVideoCapDev_dx(&pVc.capDev.pVideoCapDev))
						{
#if __DEBUG__
							traceLog(("closeLocalVideo failed, exitVideoCapDev_dx failed"));
							MACRO_qyAssert(0, ("closeLocalVideo failed, exitVideoCapDev_dx failed"));
#endif
							goto errLabel;
						}
						pVc.bCapDevConnected = false;
						break;
#endif
                    case Consts.CONST_camCapType_android:
                        GuiShare.pf_exitVideoCapDev(hWnd, iWndContentType);
                        pVc.bCapDevConnected = false;
                        break;
                    default:
                        goto errLabel;
                }
                //memset(&pVc.capDev, 0, sizeof(pVc.capDev)  );
                pVc.capDev.memset0();

            }
            if (pVc.bCapDevConnected)
            {
#if __DEBUG__
				traceLogA("closeLocalVideo failed. bCapDevConnected is true");
#endif
                goto errLabel;
            }

            //
            if (0 != pProcInfoCommon.exitCompressVideo(true, ref pVc.compressVideo)) goto errLabel;



            //  2009/05/30
            exitOrg2IntBuf(pProcInfoCommon, (CAP_procInfo_bmpCommon)pVc);


            //
            if (pSharedObj.var.hMutex_syncEncFrame != null)
            {
                pSharedObj.var.hMutex_syncEncFrame.Close();
                pSharedObj.var.hMutex_syncEncFrame = null;
            }
            if (pSharedObj.var.hEvt_notifyFrameAvail != null)
            {
                pSharedObj.var.hEvt_notifyFrameAvail.Close();
                pSharedObj.var.hEvt_notifyFrameAvail = null;
            }



            //
            iErr = 0;
        errLabel:

#if __DEBUG__
			traceLogA("closeLocalVideo leaves");
#endif
            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfoCommon.who_showInfo, 0, (""), (""), ("closeLocalVideo leaves with %s"), (iErr ? ("failed") : ("OK")));

            return iErr;
        }


        public static unsafe void exitOrg2IntBuf(CCtxQmc pProcInfo, CAP_procInfo_bmpCommon pVc)
        {
            //
            /*
			if (pVc.hicOrg2Rgb)
			{
				ICClose(pVc.hicOrg2Rgb); pVc.hicOrg2Rgb = NULL;
			}
			*/

            MACRO_mysafeFree(ref pVc.pOrg2IntBuf);
            pVc.iOrg2IntBufSize = 0;

            pVc.bOrg2IntBufInited = false;
        }



        // to intermediate buf
        public static unsafe int initOrg2IntBuf(CCtxQmc pProcInfo, ref VIDEO_COMPRESSOR_CFG pVideoCompressor, CAP_procInfo_bmpCommon pVc)
        {
            int iErr = -1;
            //BITMAPINFOHEADER* pBmpInfo = null;
            int iFormatSize = 0;
            uint biSizeImage = 0;

            //
            if (pVc.bOrg2IntBufInited) return -1;

            //memset(&pVc.vh_dec, 0, sizeof(pVc.vh_dec)  );
            pVc.vh_int = new QY_VIDEO_HEADER();
            //
            /*
			pVc.vh_dec.bih.biSize = sizeof(BITMAPINFOHEADER);
			pVc.vh_dec.bih.biCompression = Consts.BI_RGB;
			pVc.vh_dec.bih.biBitCount = 24;
			pVc.vh_dec.bih.biPlanes = 1;
			//
			pVc.vh_dec.bih.biWidth = pVc.vh_org.bih.biWidth;
			pVc.vh_dec.bih.biHeight = pVc.vh_org.bih.biHeight;
			//
			biSizeImage = (uint)((pVc.vh_dec.bih.biWidth * pVc.vh_dec.bih.biBitCount + 31) / 32 * 4 * pVc.vh_dec.bih.biHeight);
			pVc.vh_dec.bih.biSizeImage = (int)biSizeImage;
			*/

            fixed (QY_VIDEO_HEADER* pVh_dec = &pVc.vh_int)
            {
                switch (pVc.mSensorOrientation)
                {
                    case 90:
                    case 270:
                        makeBmpInfoHeader_i420(12, pVc.vh_org.bih.biHeight, pVc.vh_org.bih.biWidth, &pVh_dec->bih);
                        break;
                    case 0:
                    case 180:
                    case 360:
                        makeBmpInfoHeader_i420(12, pVc.vh_org.bih.biWidth, pVc.vh_org.bih.biHeight, &pVh_dec->bih);
                        break;
                    default:
                        goto errLabel;
                        break;
                }
            }


            /*
			if (pVc.vh_org.bih.biCompression != Consts.BI_RGB)
			{

				if (pVc.vh_org.bih.biCompression == Consts.CONST_fourcc_YUY2)
				{
					traceLog(("call yuy2rgb directly"));
				}
				else if (pVc.vh_org.bih.biCompression == Consts.CONST_fourcc_YV12)
				{
					traceLog(("call yv12rgb directly"));
				}
				else if (pVc.vh_org.bih.biCompression == Consts.CONST_fourcc_I420)
				{
					traceLog(("call yv12rgb directly"));
				}
				else if (pVc.vh_org.bih.biCompression == Consts.CONST_fourcc_HDYC)
				{
					traceLog(("call hdycrgb directly"));
				}
				else if (pVc.vh_org.bih.biCompression == Consts.CONST_fourcc_v210)
				{       //  2012/02/23
					traceLog(("call v210rgb directly"));
				}
				else if (pVc.vh_org.bih.biCompression == Consts.BI_BITFIELDS && pVc.vh_org.bih.biBitCount == 16)
				{
					traceLog(("call rgb565ToRgb directly"));
				}
				else
				{
 				traceLog(("vcm is not supported in winCe"));
					goto errLabel;
				}
			}
			else
			{
				if (pVc.vh_org.bih.biBitCount != 32)
				{
					//traceLogA("initOrg2RgbBuf: not supported, biBitCount %d", pVc.vh_org.bih.biBitCount);
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("initOrg2RgbBuf failed: org.biBitCount is %d"), pVc.vh_org.bih.biBitCount);
					goto errLabel;
				}
			}
			*/
            if (pVc.vh_int.bih.biWidth == pVc.vh_org.bih.biWidth
                && pVc.vh_int.bih.biHeight == pVc.vh_org.bih.biHeight
                && pVc.mSensorOrientation == 0
                && pVc.vh_int.bih.biCompression == pVc.vh_org.bih.biCompression
                )
            {
                iErr = 0; goto errLabel;
            }


            //
            pVc.iOrg2IntBufSize = (int)Math.Max(pVc.vh_int.bih.biSizeImage, biSizeImage) + iFormatSize + 4096;  //  ²»ÖªµÀ¾«È·µÄÓ¦¸ÃÊÇ¶àÉÙ£¬ËùÒÔºýÅªÒ»¸öÔÚÕâÀï£®£²£°£°£¸£¯£°£³£¯£±£¹
            if (pVc.iOrg2IntBufSize <= 0) goto errLabel;

            //  
            if (null == (pVc.pOrg2IntBuf = (byte*)mymalloc(pVc.iOrg2IntBufSize))) goto errLabel;

            pVc.bOrg2IntBufInited = true;

            iErr = 0;

        errLabel:

            //if (pBmpInfo!=null) { myfree(pBmpInfo); pBmpInfo = NULL; }
            if (0 != iErr)
            {
                exitOrg2IntBuf(pProcInfo, pVc);
                //
                //char buf[128] = "";
                //iFourcc2Str(pVc.vh_org.bih.biCompression, buf, mycountof(buf));
                //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), ("IsClient"), 0, (""), (""), ("initOrg2RgbBuf failed, fourcc %s"), CQyString(buf));
            }

            return iErr;

        }

        public static unsafe int startCapture(object hTalk, int iWndContentType, CAP_procInfo_video pVc)
        {
            int iErr = -1;


            CCtxQyMc pQyMc = g.g_pQyMc;

#if false
	CQySyncObj		syncObj;

	if  (  syncObj.sync(  pProcInfo->cfg.mutexName_syncAv  )  )  {
		traceLogA(  "bStartCapture sync faile.d"  );  goto  errLabel;
	}
#endif

            if (!pVc.bCapDevConnected) goto errLabel;

            if (pVc.bCaptureStarted)
            {       //  ÒÑ¾­Æô¶¯ÁË
                iErr = 0; goto errLabel;
            }

            switch (pVc.uiCamCapType)
            {
                /*
				case CONST_camCapType_directX:
					if (startCapture_dx(pVc->capDev.pVideoCapDev)) goto errLabel;
					break;
				*/
                case Consts.CONST_camCapType_android:
                    if (0 != GuiShare.pf_startCapture(hTalk, iWndContentType)) goto errLabel;
                    break;
                default:
                    goto errLabel;
            }
            pVc.bCaptureStarted = true;
            pVc.dwTickCnt_captureStarted = myGetTickCount(null);  //  2017/09/19

            /*
			//  2017/09/17
			switch (pVc->uiCamCapType)
			{
				case CONST_camCapType_directX:
					pVc->bCaptureStarted_openLocalVideo = TRUE;
					break;
				default:
					break;
			}
			*/

            //
            iErr = 0;
        errLabel:
            return iErr;

        }

        public static unsafe int stopCapture(object hTalk, int iWndContentType, CAP_procInfo_video pVc)
        {
            int iErr = -1;

            CCtxQyMc pQyMc = g.g_pQyMc;


#if __DEBUG__
			traceLogA("stopCapture enters");
#endif
            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("stopCapture enters"));


            if (pVc.bCaptureStarted)
            {

                switch (pVc.uiCamCapType)
                {
#if false
					case CONST_camCapType_directX:
						stopCapture_dx(pVc.capDev.pVideoCapDev);
						break;
#endif
                    case Consts.CONST_camCapType_android:
                        GuiShare.pf_stopCapture(hTalk, iWndContentType);
                        break;

                    default:
                        goto errLabel;
                }

                pVc.bCaptureStarted = false;

            }

            iErr = 0;

        errLabel:

#if __DEBUG__
			traceLogA("stopCapture leaves");
#endif
            //qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("stopCapture leaves"));


            return iErr;
        }


        public static unsafe int newSharedObjIndex(CCtxQmc pProcInfo, object hWnd, int iSharedObjType, int* piIndex_sharedObjUsr, bool* pbFirstUsr)
        {
            int iErr = -1;
            int index = 0;

            return -1;

#if false
			//
			if (!pProcInfo) return -1;
			QMC_cfg* pQmcCfg = (QMC_cfg*)pProcInfo->get_qmc_cfg();
			if (!pQmcCfg) return -1;
			QMC_status* pStatus = (QMC_status*)pProcInfo->get_qmc_status();
			if (!pStatus)
			{
				MACRO_qyAssert(0, _T("newSharedObjIndex: qmcStatus is null"));
				return -1;
			}

			QY_MC* pQyMc = (QY_MC*)pProcInfo->pQyMc;    //  QY_GET_GBUF(  );
			if (!pQyMc) return -1;


			if (!IsWindow(hWnd)) return -1;

			//QY_sharedObj_sync  *  pSharedObjInfos  =  (  QY_sharedObj_sync  *  )pProcInfo->pSharedObjInfos;
			QY_sharedObj_sync* pSharedObjInfo = NULL;
			QY_SHARED_OBJ* pSharedObj = NULL;

			//  
			for (index = 1; index < pQmcCfg->usMaxCnt_pSharedObjs; index++)
			{
				pSharedObjInfo = getSharedObjSyncByIndex(pProcInfo, index);
				pSharedObj = &pSharedObjInfo->sharedObj;    //  getSharedObjByIndex(  pProcInfo,  index  );
															//
				if (!pSharedObj->bUsed) break;
			}
			if (index == pQmcCfg->usMaxCnt_pSharedObjs)
			{
				//  2014/12/22
				pStatus->sysErr.bFailed_newSharedObj = TRUE;
				//
				goto errLabel;
			}

			memset(pSharedObj, 0, sizeof(pSharedObj[0]));
			pSharedObj->index_sharedObj = index;
			pSharedObj->uiTranNo = getuiNextTranNo(0, 0, 0);
			pSharedObj->bUsed = TRUE;

			//
			pSharedObj->uiType1 = iSharedObjType;

			//  2015/10/15
			pSharedObj->pProcInfo = pProcInfo;      //  2015/10/15

			//
			pSharedObj->iIndex_curUsr = -1;         //  2010/09/09

			//
			int iIndex_usr = 0;
			SHARED_OBJ_USR* pSharedObjUsr = getSharedObjUsr(pSharedObj, iIndex_usr);
			pSharedObjUsr->hWnd_starter = hWnd;                     //  2010/09/01

			//
			pSharedObj->iIndex_capAudio = -1;       //  2009/09/16
			pSharedObj->iIndex_capBmp = -1;         //  2009/09/16

			//pSharedObj->iIndex_shmCmd  =  -1;			//  2009/12/22

			pSharedObj->iIndex_localPlayer = -1;    //  2009/05/02

			//  2014/09/30
			syncMtCnt_start(&pSharedObjInfo->syncMtCnt, getuiNextTranNo);


			iErr = 0;
		errLabel:

			if (!iErr)
			{
				if (piIndex_sharedObjUsr) *piIndex_sharedObjUsr = iIndex_usr;
				if (pbFirstUsr) *pbFirstUsr = TRUE;
			}

			return iErr ? iErr : index;
#endif
        }


        public static unsafe int setCurSharedObjUsrFunc(CCtxQmc pMC_VAR_isCli, int index_sharedObj, bool bLocalAv, object hWnd)
        {

            CCtxQmc pProcInfo = (CCtxQmc)pMC_VAR_isCli;

            //  int								index				=	0;
            //MC_VAR_isCli				*		pProcInfo			=	QY_GET_procInfo_isCli(  );
            //
            QY_SHARED_OBJ pSharedObj = null;

            if (!GuiShare.pf_IsWindow(hWnd)) return -1;

            //  pSharedObj  =  &pProcInfo->pSharedObjs[index];
            pSharedObj = getSharedObjByIndex(pProcInfo, index_sharedObj);
            if (null == pSharedObj) return -1;
            if (pSharedObj.var.bQuit) return -1;
            if (!pSharedObj.bUsed) return -1;

            int i;
            for (i = 0; i < (pSharedObj.usrs.Length); i++)
            {
                if (pSharedObj.usrs[i].hWnd_starter == hWnd) break;
            }
            if (i == (pSharedObj.usrs.Length)) return -1;

            if (pSharedObj.var.iIndex_curUsr != i)
            {
                //
                SHARED_OBJ_USR pSharedObjUsr = null;
                /*
				HWND hWnd_starter_old = NULL;
				pSharedObjUsr getSharedObjUsr(pSharedObj, pSharedObj.iIndex_curUsr);
				if (pSharedObjUsr && IsWindow(pSharedObjUsr->hWnd_starter))
				{
					QY_WMBUF_COMM wmBuf;
					memset(&wmBuf, 0, sizeof(wmBuf));

					wmBuf.uiType = CONST_qyWmParam_requestToSpeak;
					wmBuf.u.requestToSpeak.uiCmd = ID_stopSpeaking;
					SendMessage(pSharedObjUsr->hWnd_starter, CONST_qyWm_comm, 0, (LPARAM) & wmBuf);
					//
					hWnd_starter_old = pSharedObjUsr->hWnd_starter;
				}
				*/
                //		
                pSharedObj.var.iIndex_curUsr = i;
                //
                pSharedObjUsr = getSharedObjUsr(pSharedObj, pSharedObj.var.iIndex_curUsr);
                MIS_CNT pMisCnt = (MIS_CNT)pProcInfo.pMisCnt;
                //
                //setROUTE_sendLocalAv(pProcInfo, pMisCnt, pSharedObjUsr.idInfo_to, pSharedObjUsr.ucbVideoConferenceStarter, ref pSharedObj.curRoute_sendLocalAv);
                //
                //  2016/02/10
                /*
				if (bLocalAv)
				{
					pProcInfo->set_enlarged_gZone(pSharedObjUsr->hWnd_starter);
				}
				//
				//  2012/05/12
				PostMessage(pSharedObjUsr->hWnd_starter, CONST_qyWm_postComm, CONST_qyWmParam_curSharedObjUsrChanged_mgr, 0);
				if (IsWindow(hWnd_starter_old)) PostMessage(hWnd_starter_old, CONST_qyWm_postComm, CONST_qyWmParam_curSharedObjUsrChanged_mgr, 0);
				*/
            }



            return 0;

        }

        //
        public static unsafe int setCurSharedObjUsr_localAv(CCtxQmc pMC_VAR_isCli, int index_sharedObj, object hWnd,string hint)
        {

            if (null == pMC_VAR_isCli) return -1;
            CCtxQmc pProcInfo = (CCtxQmc)pMC_VAR_isCli;

            //
            qyFuncs.traceLog("setCurSharedObjUsr_localAv called"  +  hint );


            //
            bool bLocalAv = false;
            if (0 != setCurSharedObjUsrFunc(pMC_VAR_isCli, index_sharedObj, bLocalAv, hWnd))
            {
                resetCurSharedObjUsr_localAv(pProcInfo, index_sharedObj);
                //
                //pProcInfo->set_enlarged_gZone(hWnd);
            }

            //
            if (bExists_slave(pProcInfo, index_sharedObj))
            {
                QY_SHARED_OBJ pSharedObj_master = getSharedObjByIndex(pProcInfo, index_sharedObj);
                if (pSharedObj_master != null)
                {
                    int iIndex_sharedObj_slave = pSharedObj_master.var.master.slaveKey.iIndex_sharedObj;
                    //
                    if (0 != setCurSharedObjUsr(pProcInfo, iIndex_sharedObj_slave, hWnd))
                    {
                        resetCurSharedObjUsr(pProcInfo, iIndex_sharedObj_slave);
                    }
                }
            }

            //
            qyFuncs.traceLog("setCurSharedObjUsr_localAv leaves");

            //
            return 0;
        }


        //
        public static unsafe int setCurSharedObjUsr(CCtxQmc pProcInfo, int index_sharedObj, object hWnd)
        {
            bool bLocalAv = false;
            return setCurSharedObjUsrFunc(pProcInfo, index_sharedObj, bLocalAv, hWnd);
        }


    }


}