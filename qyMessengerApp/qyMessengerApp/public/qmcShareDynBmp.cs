using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

	public struct SHARE_dyn_bmp_var
	{
		public bool bUsing;
		//bool					bShared;
		//
		public int iIndex_sharedObj;
		public int iIndex_sharedObjUsr;                    //  2014/09/11
		public uint uiTaskType;
		public bool ucbUsed_iIndex_sharedObj;             //  2014/09/11.
														  //
		public bool bSupported_pktResType_sharedTex;       //  2015/10/20

		//
		public TASK_av_props taskAvProps;                          //  2015/10/23

		//
		public uint dwTickCnt_start;                      //  2014/09/12

		//
		public bool ucbLocalVideoOpen;
		public int iTaskId;                                //  2009/09/13

		//
		//  2012/05/06
		//ZONE_PARAM zoneParam;

		//
		public struct SHARE_dyn_bmp_var_recvdReq
		{
			uint uiTranNo_storeRecvdReq;                    //  2014/09/11
			int index_recvdReq;
		};
		public SHARE_dyn_bmp_var_recvdReq recvdReq;

		//  2014/09/11
		public struct SHARE_dyn_bmp_var_internalProcess
		{
			uint dwTickCnt_start;
			bool bInProcess;
			int nStep;
			//
			bool bTest;
		};
		public SHARE_dyn_bmp_var_internalProcess internalProcess;

	};

	public unsafe struct SHARE_dyn_bmp
	{
			//	
	//  2016/07/24
	public RES_obj_simple resObj;
		
	//
	int iMenuId;
	//fixed char name[Consts.MAX_rtspUrlLen + 1];
	fixed char cusName[32];

	// 2017/06/12
	ushort usW, usH;                                //  2017/06/12


	//
	bool bAudioExists;                          //  2015/05/18. ÓÐÒôÆµ²¿·Ö
	bool bVideoExists;                          //  2015/05/18. ÓÐÊÓÆµ²¿·Ö				 

	//
	int iIndex_sharedObj_master;                //  2014/09/29. ÓÃ×÷makeSlaveStreamÊ±ÓÐÐèÒª
												//
	BITMAPINFOHEADER bih_mosaic;                                //  2014/10/31. ÓÃ×÷makePhotomosaicµÄÐèÒª



	//
public SHARE_dyn_bmp_var var;

//
//QIS_ptz_wnd ptzWnd;
		

	};

}



