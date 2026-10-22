using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

	public  struct DLG_syncFlgs
	{
		public int [] lCnt_getDlgVar;                                    //  2013/07/15
																//
																//  BOOL										bSubWndExists;									//  2011/03/28
		public bool bInSequence;                                   //  2013/07/15
															//

		//  BOOL										bLocked_bQuitDlg;								//  2012/05/09				
	}
	;


	public struct BG_wall_layout_cfg
	{
		//
		public ushort usRows_bg;                                       //  2014/10/01
		public ushort usCols_bg;                                       //  2014/10/01

		//int											iW_bgWallMem;									//  2014/10/03
		//int											iH_bgWallMem;									//  2014/10/03

	}
	;




//  2014/10/02
public struct BG_wall_layout
	{
		//
		//unsigned  short								usRows_bg;										//  2014/10/01
		//unsigned  short								usCols_bg;										//  2014/10/01

		int iW_bgWallMem;                                   //  2014/10/03
		int iH_bgWallMem;                                   //  2014/10/03

	}
	;

	public struct TALKER_policy
	{


		//  2017/06/25
		//
		public bool ucbConfCtrl_starter;


	}
	;


    partial class qyFuncs
{
}
}
