using System;
using System.Collections.Generic;
using System.Text;


partial class Consts
{
	//  2011/12/21
	public const int CONST_qdcRet_bufTooSmall = -2101;
	public const int CONST_qdcRet_exception = -2102;    //  2014/08/11


	//  2014/08/28
	public const int DEFAULT_keyDistanceInS = 30;       //  2015/11/06	//  7
														//  2014/12/06
	public const int DEFAULT_keyDistanceInS_mosaic = 30;



	//  2016/12/17
	public const int CONST_qoiType_null = 0;
	public const int CONST_qoiType_encA = 1;
	public const int CONST_qoiType_decA = 2;
	public const int CONST_qoiType_encV = 3;
	public const int CONST_qoiType_decV = 4;

	//
	public const int MAX_nDecFrames_needSkipped_start = 20;
    public const int MAX_nDecFrames_needSkipped_request = 20;


}



namespace qy
{

	//
	public  struct QDC_objInfo_varCommon
	{
		
		//
		public object pObj;

		//
		public bool ucbTransformFuncsOk;                      //  2014/03/26

		//
		public uint cbInputBuffer;
		public uint cbOutputBuffer;

		public int iDiffInMs_tStartAudio;                      //  2009/05/26
		//public void* pDebugStep1;                                   //  2010/04/28

        public uint nDecFrame_needSkipped;


        //  2014/08/25
        public struct QDC_objInfo_varCommon_encInfo
		{	
			public		 bool bNeed_forceKeyFrame;
		
			//
			public uint dwLastTickCnt_insertKeyFrame;
	
		};
		public QDC_objInfo_varCommon_encInfo encInfo;
				 //
				 public struct QDC_objInfo_varCommon_decInfo
		{
			public int nBytes_deced;
		};

		public QDC_objInfo_varCommon_decInfo decInfo;
		

				 //  2015/09/27
				public ushort usPktResType_dec;
	//
	/*
	struct  {
					 unsigned int uiSeqNo_lastRender;                           //  2015/10/06
}
sharedTex;

//  2015/10/17
struct                                             {
					 int min_qNodes_left;
//
DWORD dwTgt_start;
				 }													mql;
		*/

}		 ;

	//
	public struct QOI_common_cfg
	{
		public AUDIO_COMPRESSOR_cfgCommon a;      //  2016/12/06			
		public VIDEO_COMPRESSOR_cfgCommon v;       //  cfg;
	};

	public struct QOI_common_v
    {
		public int iFourcc;

	};

	public struct QOI_common       //QDC_OBJ_INFO
	{
		//
		//public QOI_common_v v;
		
		//
		public uint uiType;                                //  2016/04/18
		public ushort usSubtype;                           //  2016/04/18
		public int iUsage;                             //  2016/04/18

		//
		public object bb_input;
		public object bb_output;
		public byte bHeadLen;

		//
		public int nLeft_output;

		//
		public int nPktsDeced;
		public int iDecBufSize;

		//
		public object hEvent_notifyToQuit;


		/*
		//
		PF_qoi_bGetLongProperty pf_qoi_bGetLongProperty;
		PF_qoi_bSetLongProperty pf_qoi_bSetLongProperty;
		//
		PF_qoi_bGetStrProperty pf_qoi_bGetStrProperty;
		PF_qoi_bSetStrProperty pf_qoi_bSetStrProperty;

		//
		PF_qoi_getPtrProperty pf_qoi_getPtrProperty;                //  dll get sth from qm
		PF_qoi_bSetPtrProperty pf_qoi_bSetPtrProperty;              //  dll set sth in qm
		PF_qoi_getSpecialPtrProperty pf_qoi_getSpecialPtrProperty;
		PF_qoi_bSetSpecialProperty pf_qoi_bSetSpecialProperty;

		*/
		//  2014/03/25
		public QOI_common_cfg cfg;
				
		//		
		public QDC_objInfo_varCommon var;
		

		public unsafe void memset0()
        {
			//
			//
			this.cfg = new QOI_common_cfg();

			//
			var.nDecFrame_needSkipped = Consts.MAX_nDecFrames_needSkipped_start;

			//			
			return;
        }
	};


	public unsafe struct QDC_MGR
	{
		/*
		QIS_guiOpen* pGuiOpen;                      //  2011/12/14
													//
		int iCompressorId;
		TCHAR name[32];
		//
		PF_yuy2ToRgb24 pf_yuy2ToRgb24;                  //  2011/12/03
		PF_yuy2ToYv12 pf_yuy2ToYv12;
		PF_rgb24ToYv12 pf_rgb24ToYv12;
		PF_yv12ToRgb24 pf_yv12ToRgb24;
		PF_uyvyToRgb24 pf_uyvyToRgb24;                  //  2012/02/23	//  void	*			p4_reserved;
		void* p5_reserved;
		void* p6_reserved;
		void* p7_reserved;
		void* p8_reserved;
		void* p9_reserved;
		//
		void* p;                                //  给模块dll保留的一个参数
												//
		unsigned char ucbNotSupported_hw;               //  2011/11/26
														//
		unsigned char ucbNotSupported_dec_h264;     //  2012/09/09
													//
		char ver[8 + 1];                    //  2014/12/02
		*/
	}
	;


	//  2011/06/19
	public delegate int PF_qdcInit(ref QDC_MGR p  );
	public delegate int PF_qdcExit(ref QDC_MGR p  );


	//
	public delegate int PF_qdcInitCompressAudio(ref AUDIO_COMPRESSOR_CFG pAudioCompressorCfg_reserved, ref QY_AUDIO_HEADER pAh_decompress, ref QY_AUDIO_HEADER pAh_compress, ref QOI_common pQdcObjInfo);
	public delegate int PF_qdcExitCompressAudio(ref QOI_common pQdcObjInfo  );
	public delegate int PF_qdcCompressAudio(ref QOI_common pQdcObjInfo, ref QY_AUDIO_HEADER pAh_decompress, IntPtr pInput, uint uiInputSize,  int iSampleTimeInMs  );

	//  2016/12/05
	public delegate int PF_qdcCompressAudioRun(ref QOI_common pQdcObjInfo  );


	//
	public delegate int PF_qdcInitDecompressAudio(ref AUDIO_COMPRESSOR_CFG pAudioCompressorCfg, ref QY_AUDIO_HEADER pAh_compress, ref QY_AUDIO_HEADER pAh_decompress, ref QOI_common pQdcObjInfo);
	public delegate int PF_qdcExitDecompressAudio(ref QOI_common pQdcObjInfo  );
	public delegate int PF_qdcDecompressAudio(ref QOI_common pQdcObjInfo, ref QY_AUDIO_HEADER pAh_compress, IntPtr pInput, uint uiInputLen,  int iSampleTimeInMs, uint ui_rtTimeLen  );

	//
	//  2016/12/05
	public delegate int PF_qdcDecompressAudioRun(ref QOI_common pQdcObjInfo  );





	//
	public delegate int PF_qdcInitCompressVideo(ref VIDEO_COMPRESSOR_CFG pVideoCompressorCfg_reserved, ref QY_VIDEO_HEADER pVh_decompress, ref QY_VIDEO_HEADER pVh_compress, ref QOI_common pQdcObjInfo);
	public delegate int PF_qdcExitCompressVideo(ref QOI_common pQdcObjInfo  );
	//  2014/03/30
	//typedef  int  (  *PF_qdcGetSpsPps  )(  QDC_OBJ_INFO  *  pQdcObjInfo,  char  *  spsBuf,  uint  *  puiSpsSize,  char  *  ppsBuf,  uint  *  puiPpsSize,  char  *  kk,  uint  *  puiKkSize  );
	//  2017/10/10
	public delegate int PF_qdcGetSpsPps(ref QOI_common pQdcObjInfo, IntPtr spsBuf, ref uint puiSpsSize, IntPtr ppsBuf, ref uint puiPpsSize, IntPtr vps, ref uint puiVpsSize);

	public delegate int PF_qdcCompressVideo(ref QOI_common pQdcObjInfo, ref BITMAPINFOHEADER pBmpInfo_input, IntPtr pInput, int iSampleTimeInMs);   //  2012/06/17

	//  2014/04/02
	public delegate int PF_qdcCompressVideoRun(ref QOI_common pQdcObjInfo  );

	//
	public delegate int PF_qdcInitDecompressVideo(ref VIDEO_COMPRESSOR_CFG pVideoCompressorCfg_reserved, ref QY_VIDEO_HEADER pVh_compress, ref QY_VIDEO_HEADER pVh_decompress, ref QOI_common pQdcObjInfo);
	public delegate int PF_qdcExitDecompressVideo(ref QOI_common pQdcObjInfo  );
public delegate int PF_qdcDecompressVideo(ref QOI_common pQdcObjInfo, ref BITMAPINFOHEADER pBmpInfo_input, IntPtr pInput, int iSampleTimeInMs);	//  2012/06/17

//  2011/12/10
public delegate int PF_qdcDecompressVideoAndOutput(ref QOI_common pQdcObjInfo, ref BITMAPINFOHEADER pBmpInfo_input, IntPtr pInput, ref BITMAPINFOHEADER pBmpInfo_output, IntPtr pOutput);

//  2014/03/28
public delegate int PF_qdcDecompressVideoRun(ref QOI_common pQdcObjInfo  );




	//
public delegate int PF_qdcGetOutputByIndex(ref QOI_common pQdcObjInfo, int index, IntPtr pbOutData, ref uint puiDataSize, ref uint puiSampleTimeInMs, ref uint pui_rtTimeLen  );
public delegate int PF_qdcClearOutputs(ref QOI_common pQdcObjInfo  );


}
