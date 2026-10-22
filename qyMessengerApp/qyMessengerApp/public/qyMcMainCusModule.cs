using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

	public unsafe class CUS_MODULES
	{
		public CUS_MODULE_compress[] mems;

		/*
		unsigned short usCnt;
		CUS_MODULE_U mems[CONST_maxCusModules];

		CUS_MODULE_U* pIpp;             //  2011/07/31
										//
		CUS_MODULE_U* pMediaSdk_dec;        //  2014/03/27
		CUS_MODULE_U* pMediaSdk_enc;        //  2014/03/27
		CUS_MODULE_U* pMediaSdk_enc_conf;   //  2014/07/01

		//  
		CUS_MODULE_U* pDec_nv;          //  2014/06/19
		CUS_MODULE_U* pEnc_nv;          //  2014/06/19

		//
		CUS_MODULE_U* pDec_amf;         //  2015/04/30
		CUS_MODULE_U* pEnc_amf;         //  2015/04/30


		//
		CUS_MODULE_U* pQdc_vorbis_dec;  //  2015/02/13
		CUS_MODULE_U* pQdc_vorbis_enc;  //  2015/02/13
										//  
		CUS_MODULE_U* pDec_vp8;         //  2014/07/18
		CUS_MODULE_U* pEnc_vp8;         //  2014/07/18
		*/
	}
	;

	//  cusModule doesnt need be dynLoaded, it can also be loaded by being linked. 

	public  struct CUS_MODULE_common
	{
//#define MACRO_cusModuleCommon	
		public uint					uiType;								
		//TCHAR name[32 + 1];						
		public bool bLoaded;							
		//										TCHAR path[MAX_PATH + 1];				
		//										HMODULE hModule;
		//  2014/06/19
	}
	;


	public unsafe class CUS_MODULE_compress
	{                                           //  2010/04/28
		public CUS_MODULE_common common;
		
		//
		public QDC_MGR                        mgr;
			
		//
		public PF_qdcInit pf_qdcInit;
		public PF_qdcExit pf_qdcExit;

		/*
		//
		PF_qdcGetProp pf_qdcGetProp;                                //  2015/10/20
		PF_qdcSetProp pf_qdcSetProp;                                //  2016/04/09

		//
		PF_qdcGetCompressAudioCfg pf_qdcGetCompressAudioCfg;
		PF_qdcGetCompressVideoCfg pf_qdcGetCompressVideoCfg;
		*/

		public PF_qdcInitCompressAudio pf_qdcInitCompressAudio;
		public PF_qdcExitCompressAudio pf_qdcExitCompressAudio;
		public PF_qdcCompressAudio pf_qdcCompressAudio;
		public PF_qdcCompressAudioRun pf_qdcCompressAudioRun;                      //  2016/12/05

		//
		public PF_qdcInitDecompressAudio pf_qdcInitDecompressAudio;
		public PF_qdcExitDecompressAudio pf_qdcExitDecompressAudio;
		public PF_qdcDecompressAudio pf_qdcDecompressAudio;
		public PF_qdcDecompressAudioRun pf_qdcDecompressAudioRun;                  //  2016/12/05
		
		//
		public PF_qdcInitCompressVideo pf_qdcInitCompressVideo;
		public PF_qdcExitCompressVideo pf_qdcExitCompressVideo;
		//  2014/03/30
		public PF_qdcGetSpsPps pf_qdcGetSpsPps;

		public PF_qdcCompressVideo pf_qdcCompressVideo;
		public PF_qdcCompressVideoRun pf_qdcCompressVideoRun;                      //  2014/04/02


		
		//
		public PF_qdcInitDecompressVideo pf_qdcInitDecompressVideo;
		public PF_qdcExitDecompressVideo pf_qdcExitDecompressVideo;
		public PF_qdcDecompressVideo pf_qdcDecompressVideo;

		//  2011/12/10
		public PF_qdcDecompressVideoAndOutput pf_qdcDecompressVideoAndOutput;

		//  2014/03/28
		public PF_qdcDecompressVideoRun pf_qdcDecompressVideoRun;
		

		//
		public PF_qdcGetOutputByIndex pf_qdcGetOutputByIndex;
		public PF_qdcClearOutputs pf_qdcClearOutputs;

		//
		public bool ucbInited;

	}
	;



    partial class qyFuncs
	{
		public static  void 		M_getCompressModule(  CUS_MODULES pCusModules,  uint uiModuleType,  ref CUS_MODULE_compress pModule  )  
		{													
								  pModule	=	getCusModule((pCusModules  ), uiModuleType,0,0,null  );				
			}

public static unsafe int loadCusModules(CCtxQyMc pQyMc)
		{
			int iErr = -1;
			int index = 0;
			CUS_MODULE_compress pModule = null;
			//TCHAR tBuf[256] = _T("");
			//unsigned  int uiVDevId = DEFAULT_vDevId_base;
			//
			CQyRWLock syncObj;
			//

			//  2015/10/30

			//  
			index = 0;




			//
			//  

			//

			//
			pQyMc.cusModules.mems = new CUS_MODULE_compress[2];

			/*
			for ( index = 0; index <pQyMc.cusModules.mems.Length;index++)
            {

				//
				pQyMc.cusModules.mems[index] = new CUS_MODULE_compress();
				pQyMc.cusModules.mems[index].common.uiType = Consts.CONST_moduleType_android;

				//
            	pModule = pQyMc.cusModules.mems[index];

				//
				pModule.common.bLoaded = true;
				
				//
				switch (pModule.common.uiType)
				{
					case Consts.CONST_moduleType_android:
						
						 //
						pModule->compress.pf_qdcInit = (PF_qdcInit)myGetProcAddress(pModule->common.hModule, "qdcInit");
						pModule->compress.pf_qdcExit = (PF_qdcExit)myGetProcAddress(pModule->common.hModule, "qdcExit");

						//  2015/10/20
						pModule->compress.pf_qdcGetProp = (PF_qdcGetProp)myGetProcAddress(pModule->common.hModule, "qdcGetProp");
						//  2016/04/09
						pModule->compress.pf_qdcSetProp = (PF_qdcSetProp)myGetProcAddress(pModule->common.hModule, "qdcSetProp");
						if (pModule->compress.pf_qdcSetProp)
						{
							int ii = 0;
						}

						//
						pModule->compress.pf_qdcGetCompressAudioCfg = (PF_qdcGetCompressAudioCfg)myGetProcAddress(pModule->common.hModule, "qdcGetCompressAudioCfg");
						pModule->compress.pf_qdcGetCompressVideoCfg = (PF_qdcGetCompressVideoCfg)myGetProcAddress(pModule->common.hModule, "qdcGetCompressVideoCfg");

						pModule->compress.pf_qdcInitCompressAudio = (PF_qdcInitCompressAudio)myGetProcAddress(pModule->common.hModule, "qdcInitCompressAudio");
						pModule->compress.pf_qdcExitCompressAudio = (PF_qdcExitCompressAudio)myGetProcAddress(pModule->common.hModule, "qdcExitCompressAudio");
						pModule->compress.pf_qdcCompressAudio = (PF_qdcCompressAudio)myGetProcAddress(pModule->common.hModule, "qdcCompressAudio");
						//  2016/12/05
						pModule->compress.pf_qdcCompressAudioRun = (PF_qdcCompressAudioRun)myGetProcAddress(pModule->common.hModule, "qdcCompressAudioRun");

						//
						pModule->compress.pf_qdcInitDecompressAudio = (PF_qdcInitDecompressAudio)myGetProcAddress(pModule->common.hModule, "qdcInitDecompressAudio");
						pModule->compress.pf_qdcExitDecompressAudio = (PF_qdcExitDecompressAudio)myGetProcAddress(pModule->common.hModule, "qdcExitDecompressAudio");
						pModule->compress.pf_qdcDecompressAudio = (PF_qdcDecompressAudio)myGetProcAddress(pModule->common.hModule, "qdcDecompressAudio");
						//  2016/12/05
						pModule->compress.pf_qdcDecompressAudioRun = (PF_qdcDecompressAudioRun)myGetProcAddress(pModule->common.hModule, "qdcDecompressAudioRun");


						//
						pModule->compress.pf_qdcInitCompressVideo = (PF_qdcInitCompressVideo)myGetProcAddress(pModule->common.hModule, "qdcInitCompressVideo");
						pModule->compress.pf_qdcExitCompressVideo = (PF_qdcExitCompressVideo)myGetProcAddress(pModule->common.hModule, "qdcExitCompressVideo");
						pModule->compress.pf_qdcCompressVideo = (PF_qdcCompressVideo)myGetProcAddress(pModule->common.hModule, "qdcCompressVideo");
						pModule->compress.pf_qdcCompressVideoRun = (PF_qdcCompressVideoRun)myGetProcAddress(pModule->common.hModule, "qdcCompressVideoRun");    //  2014/04/02
						pModule->compress.pf_qdcGetSpsPps = (PF_qdcGetSpsPps)myGetProcAddress(pModule->common.hModule, "qdcGetSpsPps"); //  2014/03/30
																																		//
						pModule->compress.pf_qdcInitDecompressVideo = (PF_qdcInitDecompressVideo)myGetProcAddress(pModule->common.hModule, "qdcInitDecompressVideo");
						pModule->compress.pf_qdcExitDecompressVideo = (PF_qdcExitDecompressVideo)myGetProcAddress(pModule->common.hModule, "qdcExitDecompressVideo");
						pModule->compress.pf_qdcDecompressVideo = (PF_qdcDecompressVideo)myGetProcAddress(pModule->common.hModule, "qdcDecompressVideo");

						//  2011/12/10
						pModule->compress.pf_qdcDecompressVideoAndOutput = (PF_qdcDecompressVideoAndOutput)myGetProcAddress(pModule->common.hModule, "qdcDecompressVideoAndOutput");

						//  2014/03/28
						pModule->compress.pf_qdcDecompressVideoRun = (PF_qdcDecompressVideoRun)myGetProcAddress(pModule->common.hModule, "qdcDecompressVideoRun");

						//
						pModule->compress.pf_qdcGetOutputByIndex = (PF_qdcGetOutputByIndex)myGetProcAddress(pModule->common.hModule, "qdcGetOutputByIndex");
						pModule->compress.pf_qdcClearOutputs = (PF_qdcClearOutputs)myGetProcAddress(pModule->common.hModule, "qdcClearOutputs");
						
						 

						break;
					default:
						break;
				}

				//
				continue;
			}
			*/

			//
			GuiShare.pf_loadCusModule_android();

			/*
			//
			pQyMc->cusModules.pIpp = getCusModule(&pQyMc->cusModules, CONST_moduleType_ipp);
			pQyMc->cusModules.pMediaSdk_dec = getCusModule(&pQyMc->cusModules, CONST_moduleType_mediaSdk_dec);
			pQyMc->cusModules.pMediaSdk_enc = getCusModule(&pQyMc->cusModules, CONST_moduleType_mediaSdk_enc);
			pQyMc->cusModules.pMediaSdk_enc_conf = getCusModule(&pQyMc->cusModules, CONST_moduleType_mediaSdk_enc_conf);

			//  
			pQyMc->cusModules.pDec_nv = getCusModule(&pQyMc->cusModules, CONST_moduleType_dec_nv);
			pQyMc->cusModules.pEnc_nv = getCusModule(&pQyMc->cusModules, CONST_moduleType_enc_nv);

			//  2015/04/30
			pQyMc->cusModules.pDec_amf = getCusModule(&pQyMc->cusModules, CONST_moduleType_dec_amf);
			pQyMc->cusModules.pEnc_amf = getCusModule(&pQyMc->cusModules, CONST_moduleType_enc_amf);


			//  2015/02/13
			pQyMc->cusModules.pQdc_vorbis_dec = getCusModule(&pQyMc->cusModules, CONST_moduleType_qdc_vorbis_dec);
			pQyMc->cusModules.pQdc_vorbis_enc = getCusModule(&pQyMc->cusModules, CONST_moduleType_qdc_vorbis_enc);


			//  2014/07/18 
			pQyMc->cusModules.pDec_vp8 = getCusModule(&pQyMc->cusModules, CONST_moduleType_dec_vp8);
			pQyMc->cusModules.pEnc_vp8 = getCusModule(&pQyMc->cusModules, CONST_moduleType_enc_vp8);
			*/

			//  2014/06/19
			for (index = 0; index < pQyMc.cusModules.mems.Length; index++)
            {
				if (pQyMc.cusModules.mems[index] != null
					&& pQyMc.cusModules.mems[index].common.bLoaded)
				{
					pModule = pQyMc.cusModules.mems[index];
					pModule.pf_qdcInit(ref pModule.mgr);
				}
            }

			//
			iErr = 0;

		errLabel:

# if  __DEBUG__
			traceLog(_T("loadCusModule leaves %s"), iErr ? _T("failed") : _T("ok"));
#endif

			return iErr;
		}


		//
		public static unsafe int unloadCusModules(CCtxQyMc pQyMc)
        {
			return 0;
        }

	}
}



