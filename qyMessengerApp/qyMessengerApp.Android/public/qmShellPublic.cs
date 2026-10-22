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

	public unsafe struct DLG_guiDataParam_talk
	{

	public int iDlgType;
		public void* pAddr;
		public void* pTmpGrpMemQ;
		public bool bNeedNotShowWnd;
		public object hFore;
		//	
		public int iPageType;

}
;


	public struct DLG_guiData_talk
{

	public DLG_guiDataParam_talk param;
	bool bInfoSet;


		/*
	//
	int iIDC_STATIC_dlgDesc;

	//				 
	int iIDC_talkerList;
	int iIDC_LIST_members;
	//
	int iIDC_BUTTON_micOff;
	//
	int iIDC_STATIC_filter;
	int iIDC_EDIT_filter;
	//
	int iIDC_BUTTON_permitToSpeak;
	int iIDC_BUTTON_noSpeaking;


	//  2014/01/27
	int iIDC_STATIC_msgTool;

	//				 	
	int iIDC_STATIC_ptz;                //  2012/09/05
										//
	int iIDC_STATIC_subtitles;          //  2013/04/05
	int iIDC_STATIC_offlineRes;         //  2015/08/01

	//
	int iIDC_STATIC_peerDesc;
	int iIDC_STATIC_meDesc;
	int iIDC_STATIC_otherDesc;          //  2013/02/25
	int iIDC_STATIC_bgWall_conf;                    //  2014/10/01

	//
	int iIDC_STATIC_status;             //  2017/10/02

	//
	int iIDC_STATIC_peerInfo;           //  2017/08/21
	int iIDC_LIST_peerInfo;
	int iIDC_BUTTON_shareScreen;
	int iIDC_BUTTON_controlDesktop;

	//
	void* pf_refreshDlg_talk_ce;            //  (  void  *  pDlg,  HWND  hDlg,  void  *  p2  )
	void* pf_sizeAllControls_talk_ce;       //  (  void  *  pDlg,  HWND  hDlg,  void  *  p2  )
	void* pf_displayNotification_talk_ce;   //  (  void  *  pDlg,  HWND  hDlg,  LPCTSTR  hint  )

	//
	float fx, fy;

	//
	unsigned char ucbShowDlgDesc;
	//
	unsigned char ucbShowImGrpMems;             //  2007/06/22
												//  unsigned  char										ucbShowMsgBox;
	unsigned char snmpbShowMsgBox;              //  2011/10/02
												//
	unsigned char ucbShowMore;


	//
	int idc_hidden;
	bool bListHidden;
	bool bTaskStatusHidden;
	bool bOp0Hidden;
	bool bOp1Hidden;
	//
	bool bChanged_layout;

	//				
	//CAP_RECT											peerCapRect,  meCapRect;
	//CAP_RECT											otherCapRect;					//  2013/02/25. 
	//CAP_RECT											bgWallCapRect;					//  2014/09/30

	//
	bool bImGrp;
	unsigned short usGrpSubtype;

	//
	bool bFinished_createCtrls;
	bool bCreated;

	//
	struct                                             {
					 int nItems_list;
}
msg;
				 struct                                             {
					 int nCurSel;
struct                                         {
						 //
int iTaskId;
unsigned int uiTaskType;
int iStatus;
bool bTaskAlive;
}
curSel;
//
int nItems_list;
				 }													task;
struct                                             {
					 int iTaskId;
int iStatus;
TCHAR statusDesc[64];

//
struct                                         {                        //  2016/02/17.
						 int iTaskId;
int iStatus;
DWORD dwTickCnt_recentlyRefreshed;
//
DWORD dwTickCnt_chkShadowIconic;		//  2017/09/15
						 //
					 }												canceledTask_av;

//
struct                                         {                        //  2016/02/17.

						 //  2016/02/29
bool bTaskExists_waitingTo_beRe_accepted;
						 //
						 //
					 }												canceledTask_screenOrMediaFile;

				 }													av;
bool bChanged_status;
		*/

 }		  ;

class qmShellPublic
{
}
}