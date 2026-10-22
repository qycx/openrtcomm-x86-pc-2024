using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

//
using qy;

partial class Consts
{
	public const int bufSize_bPrivateData = 1024 + 256;
	public const int bufSize_bExtraData = 512;

	//
	public const int CONST_fourccStrLen = 4;


	//
	public const int MAX_spsPpsLen = 256;

	//
	public const int cntof_VH_stream_mems = 3;

	//
	public const int CONST_fourcc_YUY2 = 0x32595559;	//(int)qyFuncs.myMAKEFOURCC('Y', 'U', 'Y', '2');        //  2010/05/31
	public const int CONST_fourcc_YV12 = 0x32315659;    // (int)qyFuncs.myMAKEFOURCC('Y', 'V', '1', '2');        //  2010/07/11
	public const int CONST_fourcc_I420 = 0x30323449;    // (int)qyFuncs.myMAKEFOURCC('I', '4', '2', '0');        //  2010/07/11
	public const int CONST_fourcc_HDYC = 0x43594448;    // (int)qyFuncs.myMAKEFOURCC('H', 'D', 'Y', 'C');        //  2012/01/03
														//
	public const int CONST_fourcc_v210 = 0x30313276;    // (int)qyFuncs.myMAKEFOURCC('v', '2', '1', '0');        //  2012/02/23

	//  
	public const int CONST_fourcc_h264 = 0x34363268;    // (int)qyFuncs.myMAKEFOURCC('h', '2', '6', '4');        //  2014/02/24
														//
	public const int CONST_fourcc_AVC = 0x20435641;		// (int)qyFuncs.myMAKEFOURCC('A', 'V', 'C', ' ');                 //  2017/10/08
	public const int CONST_fourcc_HEVC = 0x43564548;    // (int)qyFuncs.myMAKEFOURCC('H', 'E', 'V', 'C');                    //  2017/10/08

	//
	public const int CONST_fourcc_NV21 = 0x3132564e;

	//
	//  public const int		CONST_fourcc_MP2T	MAKEFOURCC(  'M',  'P',  '2',  'T'	)			//  2014/03/05

	//
	//  public const int		CONST_fourcc_vp8		0x00385056									//  2014/07/19
	public const int CONST_fourcc_vp80 = 0x30385056;                                   //  2014/08/06
	public const int CONST_fourcc_vp8_bad = 0x00385056;                                 //  2014/07/19

	//
	public const int WAVE_FORMAT_MPEGLAYER3 = 0x0055; /* ISO/MPEG Layer3 Format Tag */


	//
	//
	//  2015/09/30
	public const int CONST_pktResType_null = 0;                         //  2015/09/30
	public const int CONST_pktResType_memory = 1;                           //  2015/09/30
	public const int CONST_pktResType_sharedTex = 2;                            //  2015/09/30

	//
	public const int CONST_uiTransformType_null = 0;
	public const int CONST_uiTransformType_encVideo = 1;
	//  
	public const int CONST_uiTransformType_dec = 2;
	//
	public const int CONST_uiTransformType_encAudio = 3;

	//
	public const string MIME_TYPE_avc = "video/avc";    // H.264 Advanced Video Coding

	//
	public const string MIME_TYPE_hevc = "video/hevc";

	//
	public const string MIME_TYPE_mp3 = "audio/mpeg";    // 


}

namespace qy
{
	public unsafe delegate bool PF_bTransformVideoQuit(object pQY_TRANSFORM, void* p1, void* p2);

	public unsafe delegate  int PF_transformGetSpsPps(uint uiTransformType,  object pQY_TRANSFORM,  int index_pMems_from,  byte* pBuf, uint* puiSize  );
	public unsafe delegate int PF_transformGetBih(object pQY_TRANSFORM, BITMAPINFOHEADER* pBih, void* p2);

	//
	public unsafe delegate int PF_getNextVPkt(IntPtr pCurPkt_myDRAW_VIDEO_DATA, GENERIC_Q pInDataCacheQ, QY_Q2 pVideoQ2);


	//
	public unsafe delegate int PF_parseVPkt_i(uint uiTransformType,  object pParent_transform,  void* pPkt_myDRAW_VIDEO_DATA,  int* p_old_lPktId_alloc, byte** ppInput, uint* puiSize  );

	public unsafe delegate int PF_parseDecAPkt_o(void* pPktParam, void** ppWfx, byte** ppOutput, uint* puiSize  );
	public unsafe delegate int PF_parseDecVPkt_o(IntPtr pPktParam, void** ppBih, byte** ppOutput, uint* puiSize  );

	//
	public unsafe delegate void PF_clean_myPLAY_AUDIO_DATA(IntPtr pmyPLAY_AUDIO_DATA, string hint);
	public unsafe delegate void PF_clean_myDRAW_VIDEO_DATA(IntPtr pmyDRAW_VIDEO_DATA, string hint);

	//
	public unsafe delegate int PF_detachPkt(void* pPkt_myDRAW_VIDEO_DATA, void* p1);

	public unsafe delegate int PF_updateDecVideoParam(BITMAPINFOHEADER* pBih, double dFrameRate, void* pParent_transform, uint uiTransformType  );

	//
	public unsafe delegate int PF_doPostEnc(void* pQDC_OBJ_INFO, char* pInput, uint inputSize,  int iSampleTimeInMs, uint uiPts,  void* pBuf_pkt  );


//  2015/10/29
public unsafe delegate int PF_myLoadNextFrame(ushort usPktResType,  void* p0_pInput,  void* p1_uiInputSize,  void* p2_myLoadNextFrame  );		


//
public unsafe delegate int PF_smplPCMReader_LoadNextFrame(void* pQdcObjInfoParam, long* pold_lPktId, PF_myLoadNextFrame pf_myLoadNextFrame, void* p2_myLoadNextFrame, bool* pbNeed_forceKeyFrame);

//  2014/07/08
public unsafe delegate int PF_smplYUVReader_LoadNextFrame(void* pQdcObjInfoParam, long* pold_lPktId, PF_myLoadNextFrame pf_myLoadNextFrame, void* p2_myLoadNextFrame, bool* pbNeed_forceKeyFrame);
	//
	//public unsafe delegate int PF_smplBitstreamReader_ReadNextFrame_func(ref QOI_common pQdcObjInfoParam, int *pold_lPktId, uint uiToInMs, byte* pBsBuf, uint* puiBsBufSize, uint* puiSampleTimeInMs);
	public unsafe delegate int PF_smplBitstreamReader_ReadNextFrame_func(ref QOI_common pQdcObjInfoParam, ref int pold_lPktId, uint uiToInMs, byte[] pBsBuf, ref uint puiBsBufSize, ref uint puiSampleTimeInMs,  object p0);


	//
	public unsafe delegate int PF_data_to_pcm(void* pQdcObjInfoParam, void* pData, void* pYUVWriter_var);


//
public unsafe delegate int PF_img_to_yuv(void* pQdcObjInfoParam, void* pImg, uint cropW, uint cropH, uint pitch, uint w, uint h,  void* pYUVWriter_var  );


//
public unsafe delegate int PF_smplPCMWriter_WriteNextFrame(void* pQdcObjInfoParam, void* pQDC_OBJ_transCommon, int iFourcc, void* pYUVWriter_var, void* pImg, PF_data_to_pcm pf_data_to_pcm, double dFrameRate, string hint);

//
public unsafe delegate int PF_smplYUVWriter_WriteNextFrame(void* pQdcObjInfoParam, void* pQDC_OBJ_transCommon, int iFourcc, void* pYUVWriter_var, void* pImg, uint cropW, uint cropH, uint pitch, PF_img_to_yuv  pf_img_to_yuv,  double dFrameRate, string  hint  );

	//
	public unsafe delegate bool PF_bTransformAudioQuit(object pQY_TRANSFORM, IntPtr p1, IntPtr p2);
	public unsafe delegate int PF_getNextAPkt(myPLAY_AUDIO_DATA* pCurPkt, GENERIC_Q pInDataCacheQ, QY_Q2 pAudioQ2);



	public struct AUDIO_COMPRESSOR_cfgCommon_head
	{
		public byte ucCompressors;                                //  acm or dmo


		//  2014/05/20
		public int iAvgBitrateInKbps;
	};



		//
		public struct AUDIO_COMPRESSOR_cfgCommon
	{
#if false
		public byte ucCompressors;                                //  acm or dmo


		//  2014/05/20
		public int iAvgBitrateInKbps;
#endif
		//
		public AUDIO_COMPRESSOR_cfgCommon_head head;

		
		/////////////////////
		//  2016/12/09
		//  2014/03/25
		//public void* pCurPkt1;
		//public int size_pCurPkt;
		public GENERIC_Q pInDataCacheQ;
		public QY_Q2 pAudioQ2;
		//
		public GENERIC_Q unused_pTmpDataQ;
		//
		public uint uiTransformType_pParent;
		public object pParent_transform;
		//
		public PF_bTransformAudioQuit pf_bTransformAudioQuit;       //  bTransformQuit(  void  *  pTransform,  0,  0  )
														//



		//
		//  PF_transformGetSpsPps								pf_transformGetSpsPps;		// 

		public PF_commonHandler pf_transformGetAh;         //  (  void  *  pQY_TRANSFORM, QY_AUDIO_HEADER * pAh, void *  p2  )
													//

		//
		//public PF_getNextAPkt pf_getNextAPkt;                //   int  getNextVPkt(  myDRAW_VIDEO_DATA  *  pCurPkt,  GENERIC_Q  *	pInDataCacheQ,  QY_Q2  *	pVideoQ2  )

		//
		public PF_parseDecAPkt_o pf_parseDecAPkt_o;

		//
		//public PF_detachPkt pf_detachPkt;              //  pPkt,  0

		//public PF_clean_myPLAY_AUDIO_DATA pf_clean_myPLAY_AUDIO_DATA;
		//				 
		public int iDecParam_index_pMems_from;                         //  2014/09/07
																//
																//
		public struct AUDIO_COMPRESSOR_CFG_dec                                             {
					 //
		public PF_smplBitstreamReader_ReadNextFrame_func pf_smplBitstreamReader_ReadNextFrame_func;   //  2014/07/9
																											//
																											//  2015/01/17
		public PF_commonHandler pf_smplPCMWriter_WriteNextFrame_pre;       //  void  *  pQdcObjInfoParam,  void  *  pQDC_OBJ_transCommon,  void  *  pYUVWriter_var					 
		public PF_smplPCMWriter_WriteNextFrame pf_smplPCMWriter_WriteNextFrame0;
		public PF_commonHandler pf_smplPCMWriter_WriteNextFrame1;          //  void  *  pQdcObjInfoParam,  void  *  pQDC_OBJ_transCommon,  void  *  pYUVWriter_var

		//
		public PF_smplPCMWriter_WriteNextFrame pf_smplPCMWriter_WriteNextFrame_all;
		//
		public PF_commonHandler pf_postAPktToRender;                       //  void  *  pQY_TRANSFORM,  void  *  pmyPLAY_PLAY_DATA,  BOOL  *  unused_pbPktRedirected  
	};
		public AUDIO_COMPRESSOR_CFG_dec	dec;

				 //
				 /*
				 struct                                             {
					 //
	PF_smplPCMReader_LoadNextFrame pf_smplPCMReader_LoadNextFrame;             //  2014/07/09
																								//
	PF_doPostEnc pf_doPostEnc;                              //  void  *  pParent,  void  *  pInput,  uiSize  )
}
enc;
				 */
		
		
				 /////////////////



}		 ;





//  2014/11/13
public struct VIDEO_input_info
	{
	public Int64 tStartTran;                                                  //  2011/08/30
		public ushort usFramesThisSecond;                                          //  2011/08/30
		public ushort usFps;                                                       //  2011/08/30				
																				//
	//TCHAR tBuf[32];                                                 //  2014/11/26
																	//
}
;






//  2014/12/02
//
public unsafe struct VIDEO_COMPRESSOR_cfgCommon_head
	{
	public byte ucCompressors;                                //  vcm or dmo

		//public fixed byte fourccStr[Consts.CONST_fourccStrLen + 1];
		public int iFourcc;

		//
		public byte ucbNeedKeyFrameFirst;		
		
		//
		public byte ucSeconds_perFrame;                           //  2011/08/08
																  //
		public ushort usMaxFps_toShareBmp;                     //  2008/12/06
		public ushort usMinFps_toShareBmp;                     //  2009/12/09

		//  2014/04/20. 
		public int iWidth_pic;                                 //  2011/08/07
		public int iHeight_pic;                                //  2011/08/07

		//
		public int profile_idc;                                //  2011/07/30\

		//
		public uint uiCapType;                                 //  2012/02/08.
		public uint uiSubCapType;                              //  2012/06/12
		public int iCapUsage;                                  //  2014/12/02

		//
		public int iTargetUsage;                               //  2012/02/08

		//  2012/06/27
		public int iAvgBitrate;
		public int iPeekBitrate;

		
	//  2014/03/29
	public byte ucHardwareAccl;

		
	//  2014/03/25
	/*
	public void* pCurPkt;
		public int size_pCurPkt;
		public GENERIC_Q pInDataCacheQ;
		public void* pVideoQ2;
		public void* pTmpDataQ;
		//
		public uint uiTransformType_pParent;
		public void* pParent_transform;
		
	//
	PF_bCommonHandler pf_bTransformVideoQuit;       //  bTransformQuit(  void  *  pTransform,  0,  0  )
													//
	PF_transformGetSpsPps pf_transformGetSpsPps;        //  
	PF_commonHandler pf_transformGetBih;            //  (  void  *  pQY_TRANSFORM, bitmapinfoheader * pBih, void *  p2  )
													//
	PF_commonHandler pf_getNextVPkt;                //   int  getNextVPkt(  myDRAW_VIDEO_DATA  *  pCurPkt,  GENERIC_Q  *	pInDataCacheQ,  QY_Q2  *	pVideoQ2  )
													//
	PF_parseDecVPkt_o pf_parseDecVPkt_o;
	PF_detachPkt pf_detachPkt;              //  pPkt,  0
	PF_clean_myDRAW_VIDEO_DATA pf_clean_myDRAW_VIDEO_DATA;
	//				 
	int iDecParam_index_pMems_from;                         //  2014/09/07
															//
	struct                                             {
					 //
	PF_smplBitstreamReader_ReadNextFrame_func pf_smplBitstreamReader_ReadNextFrame_func;   //  2014/07/9
																											//
																											//  2015/01/17			
	PF_commonHandler pf_smplYUVWriter_WriteNextFrame_pre;       //  void  *  pQdcObjInfoParam,  void  *  pQDC_OBJ_transCommon,  void  *  pYUVWriter_var
	PF_smplYUVWriter_WriteNextFrame pf_smplYUVWriter_WriteNextFrame0;
	PF_commonHandler pf_smplYUVWriter_WriteNextFrame1;          //  void  *  pQdcObjInfoParam,  void  *  pQDC_OBJ_transCommon,  void  *  pYUVWriter_var
																//
	PF_smplYUVWriter_WriteNextFrame pf_smplYUVWriter_WriteNextFrame_all;

	//
	PF_updateDecVideoParam pf_updateDecVideoParam;                      //  pBih,  pTransform,  transformType
	PF_commonHandler pf_postVPktToRender;                       //  void  *  pQY_TRANSFORM,  void  *  pmyDRAW_VIDEO_DATA,  BOOL  *  pbPktRedirected  
}
dec;
//
struct                                             {
					 //
unsigned char ucbResizable;                                //  2014/04/20
int iW_org;
int iH_org;
//
PF_smplYUVReader_LoadNextFrame pf_smplYUVReader_LoadNextFrame;              //  2014/07/09
																			//
PF_doPostEnc pf_doPostEnc;								//  void  *  pParent,  void  *  pInput,  uiSize  )
				 }													enc;
		*/

//
int tmp_max_dec_fps;
VIDEO_input_info inputInfo;
//  2015/10/04
ushort usPktResType_suggested;						//  2015/10/04. 


}		 ;


	//
	public struct VIDEO_COMPRESSOR_cfgCommon
	{

		//
		public VIDEO_COMPRESSOR_cfgCommon_head head;

		//
		public int iTaskId;

		//  2014/03/25
		//
		//public void* pCurPkt1;
		public int size_pCurPkt;
		public GENERIC_Q pInDataCacheQ;
		public QY_Q2 pVideoQ2;
		public GENERIC_Q pTmpDataQ;
		//
		public uint uiTransformType_pParent;
		public object pParent_transform;
		//
		public PF_bTransformVideoQuit pf_bTransformVideoQuit;       //  bTransformQuit(  void  *  pTransform,  0,  0  )
															   //
		public PF_transformGetSpsPps pf_transformGetSpsPps;        //  
		public PF_transformGetBih pf_transformGetBih;            //  (  void  *  pQY_TRANSFORM, bitmapinfoheader * pBih, void *  p2  )
															   //
		public PF_getNextVPkt pf_getNextVPkt;                //   int  getNextVPkt(  myDRAW_VIDEO_DATA  *  pCurPkt,  GENERIC_Q  *	pInDataCacheQ,  QY_Q2  *	pVideoQ2  )
															   //
		public PF_parseDecVPkt_o pf_parseDecVPkt_o;
		public PF_detachPkt pf_detachPkt;              //  pPkt,  0
		public PF_clean_myDRAW_VIDEO_DATA pf_clean_myDRAW_VIDEO_DATA;

		//				 
		public int iDecParam_index_pMems_from;                         //  2014/09/07

		public struct VIDEO_COMPRESSOR_CFG_dec
		{
			//
			public PF_smplBitstreamReader_ReadNextFrame_func pf_smplBitstreamReader_ReadNextFrame_func;   //  2014/07/9
																										  //
																										  //  2015/01/17			
			public PF_commonHandler pf_smplYUVWriter_WriteNextFrame_pre;       //  void  *  pQdcObjInfoParam,  void  *  pQDC_OBJ_transCommon,  void  *  pYUVWriter_var
			public PF_smplYUVWriter_WriteNextFrame pf_smplYUVWriter_WriteNextFrame0;
			public PF_commonHandler pf_smplYUVWriter_WriteNextFrame1;          //  void  *  pQdcObjInfoParam,  void  *  pQDC_OBJ_transCommon,  void  *  pYUVWriter_var
																			   //
			public PF_smplYUVWriter_WriteNextFrame pf_smplYUVWriter_WriteNextFrame_all;

			//
			public PF_updateDecVideoParam pf_updateDecVideoParam;                      //  pBih,  pTransform,  transformType
			public PF_commonHandler pf_postVPktToRender;                       //  void  *  pQY_TRANSFORM,  void  *  pmyDRAW_VIDEO_DATA,  BOOL  *  pbPktRedirected  
		};
		public VIDEO_COMPRESSOR_CFG_dec dec;

																//
		/*
	//
	struct                                             {
						 //
	unsigned char ucbResizable;                                //  2014/04/20
	int iW_org;
	int iH_org;
	//
	PF_smplYUVReader_LoadNextFrame pf_smplYUVReader_LoadNextFrame;              //  2014/07/09
																				//
	PF_doPostEnc pf_doPostEnc;								//  void  *  pParent,  void  *  pInput,  uiSize  )
					 }													enc;
			*/

		//
		int tmp_max_dec_fps;
		VIDEO_input_info inputInfo;
		//  2015/10/04
		public ushort usPktResType_suggested;                      //  2015/10/04. 


	};







	//
	[StructLayout(LayoutKind.Sequential,  Pack = 1)]
public struct BITMAPINFOHEADER
	{ 
  public int biSize;
	public int biWidth;
	public int biHeight;
	public short biPlanes;
	public short biBitCount;
	public int biCompression;
	public int biSizeImage;
	public int biXPelsPerMeter;
	public int biYPelsPerMeter;
	public int biClrUsed;
	public int biClrImportant;
};

	public struct WAVEFORMATEX
	{
		public ushort wFormatTag;
		public ushort nChannels;
		public uint nSamplesPerSec;
		public uint nAvgBytesPerSec;
		public ushort nBlockAlign;
		public ushort wBitsPerSample;
		public ushort cbSize;
	}

	//
	public class CWAVEFORMATEX
    {
		public WAVEFORMATEX wafeFormatEx;
	};

	public unsafe struct QY_VIDEO_HEADER
	{                       //  2009/03/29
		public BITMAPINFOHEADER bih;
		public fixed byte bPrivateData[Consts.bufSize_bPrivateData];
	}
	;

	[StructLayout(LayoutKind.Explicit,Size =530 )]	
public unsafe struct QY_AUDIO_HEADER
	{                       //  2009/05/16
		[FieldOffset(0)]public WAVEFORMATEX wfx;
		[FieldOffset(18)]public fixed byte bExtraData[Consts.bufSize_bExtraData];
	}
	;


	//
	//  2016/12/17. especially for vorbis.
	//
//#define MAX_aHeaderLen											256
	
	
	//
	public unsafe struct AH_stream
	{
		public int kkunused;
		/*
		//
		struct                                             {
					 unsigned short usLen;
	}
	mems[3];
				 //
				 char buf[6 * 1024];
		*/

}
;


	//
	public unsafe struct VH_stream_mem
	{
		public ushort usLen;
		public fixed byte spsPps[Consts.MAX_spsPpsLen + 1];
	};


	//
	//  2014/02/25
	//
	public unsafe struct VH_stream
	{
	public uint dwFourcc;
	//
	public ushort usCnt;
		//	VH_stream_mem mems[6];
		public VH_stream_mem mem0, mem1,mem2;
		//cntof_VH_stream_mems =3;

	}		 ;

	//
	partial class qyFuncs
    {
		uint i = myMAKEFOURCC(0, 0, 0, 0);

    }
 
}
