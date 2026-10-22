using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
    public const int CONST_maxTriesToStartSession_mis = 5;

    //
    public const int CONST_iAllocType_msg = (CONST_iAllocType_user + 1);
    public const int CONST_iAllocType_audio = (CONST_iAllocType_user + 2);
    public const int CONST_iAllocType_video = (CONST_iAllocType_user + 3);
    public const int CONST_iAllocType_longTimeMsg = (CONST_iAllocType_user + 4);

	//
	//  2014/09/03
	public const int CONST_talkerObjType_null = 0;
	public const int CONST_talkerObjType_dynBmp = 1;


}

namespace qy
{
	public struct P_sendAvInfo_taskInfo_i
	{
		public bool ucbVideoConference;      //  2017/08/02
											 //
		public bool ucbAvConsole;         //   2017/07/17
										  //
		public ushort usConfType;              //  2017/08/25

		//
		public object shared_eglContext;
	};

	public struct P_sendAvInfo_taskInfo_o
	{

		//
		public int iTaskId;
		//  
		public QY_MESSENGER_ID idInfo_starter;         //.ui64Id  =  rcd.idInfo_send.ui64Id;

		//
		public Int64 tStartTime_org;          //  =  rcd.tSendTime;
		public uint uiTranNo_org;          //  =  rcd.uiTranNo;
		public uint uiContentType_org;     //  =  rcd.uiContentType;

		//
		public bool ucbStarter;               //  =  pContent->myTask.ucbStarter;									//  2010/08/31
		public bool ucbVideoConference;       //  =  pContent->myTask.ucbVideoConference;					//  2010/08/31

		//
		public uint uiInitW;               //  =  pContent->tranInfo.video.vh_decompress.bih.biWidth;
		public uint uiInitH;               //  =  pContent->tranInfo.video.vh_decompress.bih.biHeight;

		//  2014/09/25
		public int iIndex_taskInfo;        //  =  pContent->myTask.iIndex_taskInfo;

		//
		public AV_stream_simple local_avStream;


	};



	public struct P_sendAvInfo_taskInfo
	{

		//  i
		public P_sendAvInfo_taskInfo_i i;

				 //  o
		public P_sendAvInfo_taskInfo_o o;

}		 ;



	public struct TALKER_dynBmp_taskInfo_retrieveOne
	{
		uint uiTranNo;
		//
		uint dwLastTickCnt_retrieveOne;
	};

	public struct TALKER_dynBmp_taskInfo_srcInfo
	{
		int iW, iH;
		bool ucbUnresizable;
	};


	public struct TALKER_dynBmp_taskInfo
	{
		public int iTaskId;
		//  2013/02/12
		public Int64 tStartTran;
		public uint uiTranNo;

		//  2013/02/24
		public Int64 tStartTime_processed;

		//  2013/02/05
		//Int64										tLastTime_active;

		//
		public bool bNeedProgress;

		//  2014/09/04
		public TALKER_dynBmp_taskInfo_retrieveOne retrieveOne;

		//  2015/10/23
		public TALKER_dynBmp_taskInfo_srcInfo srcInfo;

				 };


//  2013/02/12
public unsafe struct TALKER_dynBmp
	{
		public uint uiType1;

		public DYN_BMP_RULE dynBmpRule;

		//				 
		public TALKER_dynBmp_taskInfo taskInfo;

}		 ;


	public struct ConfMem_confCtrl
	{
		public 		 bool ucbPermitToSpeak;

		//
		public QY_MESSENGER_ID idInfo_starter;
		public Int64 tStartTime;
		public uint uiTranNo;
}
;


partial class qyFuncs
{
		public static bool isVideoNormalState(ref COMPRESS_VIDEO pCompressVideo)
		{
			return (0!=pCompressVideo.usFps_ok && pCompressVideo.bTopFrames && pCompressVideo.usFps_ok == pCompressVideo.usFrames_perSecond_expected);
		}

		public static unsafe void M_getActiveMemDesc(MIS_CNT pMisCnt, QY_MESSENGER_ID pIdInfo, char* desc, int descCnt)
		{
			string str;
			getTalkerDesc(pMisCnt, pIdInfo, null, 0, desc, (uint)descCnt);
			str = string.Format("{0}({1})", new string(desc), pIdInfo.ui64Id);
			//_sntprintf(desc, descCnt, _T("%s(%I64u)"), desc, (pIdInfo)->ui64Id);
			mytcsncpy(desc, str, descCnt);
			return;
		}

		//
		public static uint MACRO_getnWaitingInMs_audio_syncRecv(PLAY_AUDIO_procInfo player_audio, uint dwTickCnt_now  ) {
			return ((player_audio).m_var.uiSampleTimeInMs_newRecvd - (player_audio).m_var.uiSampleTimeInMs + (player_audio).m_var.dwTickCnt_uiSampleTimeInMs_syncRecv - dwTickCnt_now);
				
		}

	}
}
