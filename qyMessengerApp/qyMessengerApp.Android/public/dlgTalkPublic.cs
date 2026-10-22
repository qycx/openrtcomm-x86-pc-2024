using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
	public struct DLG_talk_layout
	{

		//
		public ushort usCnt_row;                   //  
		public ushort usCnt_col;                   //  

		//
		public uint uiW_pic;
		public uint uiH_pic;

		//				
		public ushort usLayoutType_cur;            //  2010/12/29

		//
		public bool ucbExists_localScreenSharing;         //  2015/11/14

		//
		public bool ucbShowDlgDesc;
		public bool ucbShowPeerDesc;
		public bool ucbShowMeDesc;
		public bool ucbShowOtherDesc;
		public bool ucbShow_peer_tool;            //  2017/08/21

		//
		public int iH_dlgDesc;
		public int iW_memberList;
		public int iW_taskList;
		public int iW_ptz;
		public int iW_subtitles;               //  2013/04/05
		public int iW_offlineRes;              //  2015/08/01
											   //
		public int iW_peerInfo;                //  2017/08/21
		public int iH_staticPeerInfo;
		public int iH_peerInfo;
		public int iH_shareScreen;

		//
		public int iW_meDesc;                  //  2008/03/27
		public int iH_meDesc;

		//
		//  2017/11/03
		public bool ucbShowBgWall;
		//

		public BG_wall_layout_cfg bgWallLayoutCfg;         //  2017/11/03
														   //
		public BG_wall_layout bgWallLayout;                //  2014/10/01
													//unsigned  short									usPollingIntervalInS1;		//  2014/10/06

	}
	;

	public unsafe struct PARAM_dlgTalk_procTask
	{
		public bool av_autoAnswer_av_task_bTaskNeedAutoAnswered;
		//
		public bool bManualCmd;            //  2017/02/12
		public object hParent;

		//
		public bool bAutoCancel;
	}
	;

	//
	public unsafe struct PARAM_startAvCall
	{
		public bool ucbNoLocalAv;
		//
		public QY_MESSENGER_ID idInfo_initiator;
		//
	}
	;

}