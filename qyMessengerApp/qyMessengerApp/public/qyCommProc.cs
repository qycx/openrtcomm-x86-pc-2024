using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

partial class Consts
{
	public const int CONST_maxCnt_sema_syncQmObjQ = 10;									//  qmObjQ

	public const string DEFAULT_mutexName_syncQmObjQ = ("m_syncQmObjQ");
	public const string DEFAULT_semaphoreName_syncQmObjQ = ("sema_syncQmObjQ");
	//
	public const string DEFAULT_mutexName_syncQmObjQ_client = ("m_syncQmObjQ_cli");         //  2007/06/03
	public const string DEFAULT_semaphoreName_syncQmObjQ_client = ("sema_syncQmObjQ_cli");      //  2007/06/03

	public const string DEFAULT_mutexNamePrefix_syncTask = ("m_syncTask");              //  2007/06/27, mutex, prefix + misServName  +  id_send

	//
	public const int bufSize_publicKeyBlob = 2048;
	public const int bufSize_sessionKeyBlob = 1024;
	public const int bufSize_sessionIv = 32;

	//
	public const int CONST_dataBufSize_iocp = 513 * 1024;//129 * 1024;	//  2011/02/13


	public const int CONST_qyShowType_qwmComm = 5;
	public const int CONST_qyShowType_qwmState = 7;
	public const int CONST_qyShowType_debug = 13;       //
	public const int CONST_qyShowType_debugWarning = 14;		//  2007/05/07
	public const int CONST_qyShowType_warning = 15;     //  2007/05/07

	//
	public const int CONST_uiMaxQNodes_showInfoQ_is = 500;                             //  2014/05/12
	public const int CONST_uiMaxQNodes_showInfoQ_state_is = 200;                               //  2015/08/22

	//
	public const int MAX_pipeCli_idleInMs = 5000;

#if false
	public const int QyShowStruct_cntof_who_from = 32;
	public const int QyShowStruct_cntof_who_to = 16;
	public const int QyShowStruct_cntof_whereBuf = 16;
	public const int QyShowStruct_cntof_doStr = 16;
	public const int QyShowStruct_cntof_what = 256;
#endif
	public const int QyShowSmall_cntof_whereBuf = 8;
	public const int QyShowSmall_cntof_doStr = 8;
	public const int QyShowSmall_cntof_what = 96;



}

namespace qy
{
	public struct mySYSTEMTIME
	{
		public ushort wYear;
		public ushort wMonth;
		public ushort wDayOfWeek;
		public ushort wDay;
		public ushort wHour;
		public ushort wMinute;
		public ushort wSecond;
		public ushort wMilliseconds;
	};





#if false
	public unsafe struct QY_SHOW_STRUCT
	{
		public int iType;
		//
		public ushort usStateType;                        //  2022/01/27
		public ushort usStateSubtype;                 //  2022/01/27
		//
		public int iPos_toDisplay;
		//
		public int iTaskId;                            //  2022/01/27
		//
		public uint ulClientIp;                           //  2007/08/21 socket clientIp
		public fixed char who_from[Consts.QyShowStruct_cntof_who_from];                     //  2015/05/23. WCHAR						who_from[16];
		public fixed char who_to[Consts.QyShowStruct_cntof_who_to];
		public fixed char whereBuf[Consts.QyShowStruct_cntof_whereBuf];
		public mySYSTEMTIME when;                                //  2007/06/01, 
		public uint uiStep;                                //  2008/04/28, 
															//
		public uint dwProcessId;                      //  2015/08/30
		public uint dwThreadId;                           //  2015/08/20
													//
		public fixed char doStr[Consts.QyShowStruct_cntof_doStr];
		public fixed char what[Consts.QyShowStruct_cntof_what];
#if __DEBUG__
		char testBytes;                         //  2010/09/06. 这个字节是为了发现一个大小为424字节数的内存泄露。故意增加了一个测试字节。
												//  等问题解决了后，应该去除。
#endif
	}
	;
#endif

	//
	public unsafe struct QY_SHOW_SMALL
	{
		public int iType;
		//
		public ushort usStateType;                     //  2022/01/27
		public ushort usStateSubtype;                      //  2022/01/27
															//
		public int iPos_toDisplay;
		//
		public int iTaskId;                            //  2022/01/27
												//
		public ulong ulClientIp;                           //  2007/08/21 socket clientIp
		public UInt64           who_from;							//  2015/05/23. WCHAR						who_from[16];
		public UInt64           who_to;
		public fixed char whereBuf[8];                      // utf8
		public mySYSTEMTIME when;                                //  2007/06/01, 
																 //
		public ushort usStep;                              //  2008/04/28, 
		public byte ucPercent_showInfoQ;
		public byte uc_reserved;
		//													//
		public uint dwProcessId;                      //  2015/08/30
		public uint dwThreadId;                           //  2015/08/20
													//
		public fixed char doStr[8];                         // utf8
		public fixed char what[Consts.QyShowSmall_cntof_what];                         // utf8
#if DEBUG
		char testBytes;                         //  2010/09/06. 这个字节是为了发现一个大小为424字节数的内存泄露。故意增加了一个测试字节。
												//  等问题解决了后，应该去除。
#endif
	};




	//
	public struct IM_NET_STAT
	{
		//
		public UInt64                                  ui64MsgDataLenRecvd;							//  2009/10/10
				public  UInt64                                  ui64MsgDataLenSent;
				 //
				public uint uiInSpeedInKbps;                              //  bps
		public uint uiOutSpeedInKbps;
		//
		//  unsigned  __int64								ui64MsgDataLenRecvd_dec;
		//  unsigned  __int64								ui64MsgDataLenSent_dec;
		//
		//  unsigned  long									nMsgRecvd;
		//  unsigned  long									nMsgSent;
		//
		//
	}
	;

	//
	public unsafe struct QMC_net_stat
	{
		public IM_NET_STAT ins;
		//
		public uint uiInSpeedInKbps_a;
		public uint uiOutSpeedInKbps_a;
		//
		public uint uiInSpeedInKbps_v;
		public uint uiOutSpeedInKbps_v;
		//
		public uint uiInSpeedInKbps_f;
		public uint uiOutSpeedInKbps_f;

	}
	;


	partial class qyFuncs
    {

		public static QY_DMITEM[] CONST_qyCmdTable = new QY_DMITEM[1]
{   
	//new QY_DMITEM(Consts.CONST_fieldId_yhlx,                 (  "用户类型"  )             ),
	//  ????±?????
	/*
	{   Consts.CONST_qyCmd_tellService,                        _T(  "tellService"  ),              },
	{   Consts.CONST_qyCmd_end,                                _T(  "cmdEnd"  ),                       },
	//  ???§?¨??:
	//
	{   Consts.CONST_qyCmd_talkTo,                             _T(  "talkTo"  ),                       },	//  2007/04/09
	{   Consts.CONST_qyCmd_refreshImObjListReq,                _T(  "refreshImObjListReq"  ),          },	//  2007/05/06
	{   Consts.CONST_qyCmd_refreshRecentFriendsReq,            _T(  "refreshRecentFriendsReq"  ),      },
	//
	{   Consts.CONST_qyCmd_getNmStatus,                        _T(  "getNetworkManagementStatus"  ),               },	//  2011/01/09
	//
	{   Consts.CONST_qyCmd_sendTask,                           _T(  "sndTask"  ),                      },
	{   Consts.CONST_qyCmd_sendTaskReply,                      _T(  "sndTaskReply"  ),             },
	{   Consts.CONST_qyCmd_sendRobotTask,                      _T(  "sndRobotTask"  ),             },
	{   Consts.CONST_qyCmd_sendRobotTaskReply,                 _T(  "sndRobotTaskReply"  ),            },
	{   Consts.CONST_qyCmd_sendRobotTaskData,                  _T(  "sndRobotTaskData"  ),         },
	//
	{   Consts.CONST_qyCmd_sendMedia,                          _T(  "sndMedia"  ),                 },	//  2008/03/15
	//
	{   Consts.CONST_qyCmd_lastMsgInSession,                   _T(  "lastMsgInSession"  ),             },	//  2007/12/11
	//
	{   Consts.CONST_qyCmd_sendVDevReq,                        _T(  "sndVDevReq"  ),                   },	//  2008/01/24
	//
	//  ????±?????
	{   Consts.CONST_qyCmd_upReserved,                         _T(  "upReserved"  ),                   },
	{   Consts.CONST_qyCmd_update,                             _T(  "update"  ),                       },
	{   Consts.CONST_qyCmd_getFile,                            _T(  "getFile"  ),                      },
	{   Consts.CONST_qyCmd_getDigest,                          _T(  "getDigest"  ),                    },
	{   -1,                                             NULL,                       },
*/


		//
		new QY_DMITEM( -1,                                 ""                   ),

};


	public static QY_DMITEM[] CONST_qyRcTable = new QY_DMITEM[1]
{
	/*
	 //  ????±???
	{   CONST_qyRc_ok,                  _T(  "OK"  ),                       },
	{   CONST_qyRc_err,                 _T(  "Err"  ),                      },
	 //			 ???§?¨??
	{   CONST_qyRc_user,                _T(  "User"  ),                 },
	{   CONST_qyRc_redirect,            _T(  "Redirect"  ),                 },
	{   CONST_qyRc_needAutoReg,         _T(  "Need auto req"  ),                },
	//  #define		CONST_qyRc_notFound
	{   CONST_qyRc_needVerified,        _T(  "Need verified"  ),                },
	//
	{   CONST_qyRc_peerOffline,         _T(  "Peer offline"  ),             },
	{   CONST_qyRc_servBusy,            _T(  "Serv busy"  ),                },
	//
	//			 ????±???
	{   CONST_qyRc_upReserved,          _T(  "Up reserved"  ),                  },
	{   CONST_qyRc_quit,                _T(  "Quit"  ),                     },
	{   CONST_qyRc_update,              _T(  "Update"  ),                       },
	*/
	new QY_DMITEM(   -1,                             ""                       ),
};






	/// <summary>
	/// //////////////////////
	/// </summary>
	/// <param name="ph"></param>
	/// <param name="dwMilliseconds"></param>
	public static  void waitForThread(ref Thread ph, uint dwMilliseconds)
		{
			uint dwRet = 0;

			if (null==ph) return;

			/*
			dwRet = WaitForSingleObject(*ph, dwMilliseconds);
			if (dwRet != WAIT_TIMEOUT && dwRet != WAIT_FAILED)
			{

				CloseHandle(*ph); *ph = NULL;

			}
			*/

			if (ph!=null)
			{
				if ( dwMilliseconds==0)
                {
					if ( !ph.IsAlive)
                    {
						ph = null;
						return;
                    }
					return;
                }

				uint msPerStep = 100;
				msPerStep = 10;
				uint maxStep = dwMilliseconds / msPerStep;
				if (maxStep == 0) maxStep = 1;
				//
				for ( int i =0; i < maxStep; i ++)
                {
					if (!ph.IsAlive)
                    {
						ph = null;
						return;
                    }
					Thread.Sleep((int)msPerStep);
					continue;
                }

			}

		
			return;

		}


		//
		public static bool bTaskAlive(int iStatus)
		{
			switch (iStatus)
			{
				case 0:
				case Consts.CONST_imTaskStatus_req:
				case Consts.CONST_imTaskStatus_resp:
				case Consts.CONST_imTaskStatus_applyToSend:
				case Consts.CONST_imTaskStatus_waitToSend:
				case Consts.CONST_imTaskStatus_applyToRecv:
				case Consts.CONST_imTaskStatus_waitToRecv:
				case Consts.CONST_imTaskStatus_acceptedByReceiver:
				case Consts.CONST_imTaskStatus_dualByReceiver:
					return true;
					break;
				default:
					break;
			}
			return false;

		}

		//
		public static bool bTaskNeedAcception(int iStatus)
        {
			switch (iStatus)
			{
				case 0:
				case Consts.CONST_imTaskStatus_resp:
				case Consts.CONST_imTaskStatus_applyToRecv:
					return true;
				default:
					break;
			}
			return false;
        }

	}

}
