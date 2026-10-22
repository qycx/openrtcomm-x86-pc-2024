using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	//  all service_ids
	//  #define		CONST_qyServiceId_rasMonoring			ASR_SERVICE_RAS_MONITORING
	public const int CONST_qyServiceId_base = 10;                       // service_id的基数
																		//  public const int 		CONST_qyServiceId_imc					(  CONST_qyServiceId_base  +  1  )
	public const int CONST_qyServiceId_netMc = (CONST_qyServiceId_base + 2);        //  2007/03/31
	public const int CONST_qyServiceId_qwm = CONST_qyServiceId_netMc;
	//  public const int 		CONST_qyServiceId_internetMgr			(  CONST_qyServiceId_base  +  110  )
	public const int CONST_qyServiceId_is = (CONST_qyServiceId_base + 120); //  2007/03/07
	public const int CONST_qyServiceId_mis = CONST_qyServiceId_is;

	public const int CONST_qyServiceId_showInfo = (CONST_qyServiceId_base + 130);


	//  命令码, !!! 严重警告：以下命令码，只能添加，不能修改，以避免出现各产品不兼容的局面
	//			 类型：ushort
	//			 低端保留：
	public const int CONST_qyCmd_tellService = 0;                       //  "启动服务"
	public const int CONST_qyCmd_end = 255;                 //  "结束会话"


	//			 用户定义:
	public const int CONST_qyCmd_base = 2000;
	//					  其他定义在qyCommProc.h中，须>CONST_qyCmd_base大，<CONST_qyCmd_upReserved

	//			 高端保留：
public const int  CONST_qyCmd_upReserved			=		(  (  ushort  )0xe000  );	//  57344
public const int  CONST_qyCmd_update				=		(  (  ushort  )0xe001  );
public const int  CONST_qyCmd_getFile				=		(  (  ushort  )0xe002  );
public const int  CONST_qyCmd_getDigest				=	(  (  ushort  )0xe003  );


	//  
	//			 ushort
	//			 
	public const int CONST_qyRc_ok = ((ushort)0);
	public const int CONST_qyRc_err = ((ushort)255);
	//			 
	public const int CONST_qyRc_user = ((ushort)2000);
	public const int CONST_qyRc_redirect = ((ushort)(CONST_qyRc_user + 1));
	public const int CONST_qyRc_needAutoReg = ((ushort)(CONST_qyRc_user + 2));
	//  public const int 		CONST_qyRc_notFound						(  (  ushort  )(  CONST_qyRc_user  +  3  )  )	//  2005/02/01
	public const int CONST_qyRc_needVerified = ((ushort)(CONST_qyRc_user + 4));     //  2007/04/22
	public const int CONST_qyRc_anotherLogonExists = ((ushort)(CONST_qyRc_user + 5));       //  2011/02/02


	public const int CONST_qyRc_peerOffline = ((ushort)CONST_qyRc_user + 100);          //  2007/05/29
	public const int CONST_qyRc_servBusy = ((ushort)CONST_qyRc_user + 101);         //  2007/05/29
	public const int CONST_qyRc_unknown = ((ushort)CONST_qyRc_user + 102);          //  2007/07/01

	//			 
	public const int CONST_qyRc_upReserved = ((ushort)0xe000);  //  57344
	public const int CONST_qyRc_quit = ((ushort)0xe001);
	public const int CONST_qyRc_update = ((ushort)0xe002);
	public const int CONST_qyRc_hint = ((ushort)0xe003);

	public const int CONST_commFlg_resp = (1 << 7);
	public const int CONST_commFlg_moreData	=				(  1  <<  6  );                                 //  2007/05/07
	public const int CONST_commFlg_talkData = (1 << 5);                                  //  数据区的格式为MIS_MSG_TALKDATA，否则为IM_MSG_CONTENTU
	public const int CONST_commFlg_routeTalkData = (1 << 4);                                    //  数据区的格式为用MSG_ROUTE+IM_MSG_CONTENTU

	//
	public const int bufSize_qyCommServiceData = 8192;

}

namespace qy
{


	/*  //  2007/04/19
	typedef  struct  __qyCommService_t					{
					 unsigned  int						serviceId;
					 char								version[CONST_qyMaxVerLen];			//  
					 unsigned  int						encType;
					 char								data[1024];
	}		 QY_COMM_SERVICE;
	*/
	//
	public unsafe struct QY_COMM_SERVICE
	{
		public uint serviceId;
		public fixed byte version[Consts.CONST_qyMaxVerLen];            //  
		public uint encType;
		public fixed byte data[Consts.bufSize_qyCommServiceData];
	}
	;


 public unsafe struct QY_COMM_BUFSIZE
	{
		uint len;
		byte* data;
	}
	;

/*
typedef  struct  __qyCommHead_t  {
				 ushort					usCode;				// 
				 char								reserved[2];			
				 unsigned  int						len;				// 
}		 QY_COMM_HEAD;
*/

//  2007/04/22, 
 public unsafe struct QY_COMM_HEAD
	{
		public ushort usCode;              // 
		public byte ucFlg;                //  2007/04/22, 
		public fixed byte reserved[1];
		public uint len;               //  
	}
	;




//#define		QY_COMM_REQHEAD							QY_COMM_HEAD
//#define		QY_COMM_RESPHEAD						QY_COMM_HEAD


public struct QY_COMM_REQ
	{
		public QY_COMM_HEAD head;
		//union			{
						 QY_COMM_SERVICE service;       //  这个service好象没有什么用处。2007/04/18
	//}	u;
}
;


public struct QY_COMM_RESP
	{
	public QY_COMM_HEAD head;
}
;


public unsafe struct QY_COMM_SESSION
	{
	//
		public uint uiType;                                //  2015/08/24
													   //
		public QY_COMM_SERVICE service;
		public uint lenInBytes_service;                    //  接收到的QY_COMM_SERVICE 的字节熟. 2008/06/10

		//
		/*
		QY_ENC_CTX encCtx;
		QY_ENC_CTX quitEncCtx;
		QY_ENC_CTX updateEncCtx;
		*/
		public QY_ENC_CTX commEncCtx;                          //  2004/01/03
		
		//
		public fixed byte startTime[Consts.CONST_qyTimeLen + 1];    //  2004/07/26
		public fixed byte clientIp[Consts.CONST_qyMaxIpLen + 1];    //  2007/04/01
																	//
		public uint uiSessionId;                       //  会话序号，2004/09/11
													   //
		public ushort usLastReqCode_i;                 //  2007/04/24, 输入的
		public ushort usLastRespCode_i;                    //  最后一个响应的响应码
		public ushort usLastReqCode_o;                 //  2007/04/24, 输出的
		public ushort usLastRespCode_o;                    //  最后一个响应的响应码
														   //
		public uint nTalks;                                //  与客户机交流的次数
														   //
		public bool bAdmin;                                //  2011/01/10

}
;


partial class qyFuncs
{

		//			
		public static bool isRcQuit(int x  ) { return ((x) == Consts.CONST_qyRc_quit); }
		public static bool isRcOk(int x) { return ((x) == Consts.CONST_qyRc_ok); }
		public static bool isRcRedirect(int x) { return ((x) == Consts.CONST_qyRc_redirect); }

		public static bool isRcUpdate(int x) { return ((x) == Consts.CONST_qyRc_update); }
		public static bool isRcHint(int x) { return ((x) == Consts.CONST_qyRc_hint); }
		public static bool isRcNeedVerified(int x) {	return ((x) == Consts.CONST_qyRc_needVerified); }

		//	
		public static bool isUcFlgResp(byte ucFlg) { return (0!=(ucFlg & Consts.CONST_commFlg_resp)); }                //  2007/04/22
		public static bool isUcFlgMoreData(byte ucFlg) { return (0!=(ucFlg & Consts.CONST_commFlg_moreData)); }           //  2007/05/07
		public static bool isUcFlgTalkData(byte ucFlg) { return (0!=(ucFlg & Consts.CONST_commFlg_talkData)); }           //  2007/07/11
		public static bool isUcFlgRouteTalkData(byte ucFlg) { return (0!=(ucFlg & Consts.CONST_commFlg_routeTalkData)); }		//  2007/11/29



}
}


