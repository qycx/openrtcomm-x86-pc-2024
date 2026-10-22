using System;
using System.Collections.Generic;
using System.Text;

public partial class Consts
{
	public const int QY_MC_TOTAL_DAEMON_THREADS = 10;			// ºóÌ¨Ïß³ÌÊý


}

namespace qy
{

	 public struct QY_MC_GUI
	{

		//  2015/12/08
		public object pDb;


		//
		public GENERIC_Q processQ;				               //  
		public bool ucbProcessQInited;		                  //  2007/06/01

		//
		public object hMainWnd;                              //  2009/03/25

		//
		public bool bForeground;
		//
		public object hActiveWnd;
		//
		public object hLastActiveTalk;

	}

	public struct QY_SERVICEGUI_CFG
	{
		public int iServiceId;

		//  2015/01/20
		public PF_newVar pfNewVar;                          //  2015/01/20
		public PF_freeVar pfFreeVar;

		//
		public uint uiVarSize;                         //  
		public PF_initVar pfInitVar;
		public PF_init_afterLogon pfInit_afterLogon;                 //  201003/10
		public PF_exitVar pfExitVar;                         //

		//
		public int iDbType;                            //  Obsolete

	};


 public unsafe struct QY_SERVICEGUI_INFO
	{
		public QY_SERVICEGUI_CFG cfg;
		//
		public fixed char name[32 + 1];
		//
		public CCtxQyTmpl pParent;          //  2016/09/19Ö¸Ïòdaemon
										//
		public CCtxQmcTmpl pVar;
		public bool ucbVarInited;
		//

	};


}
