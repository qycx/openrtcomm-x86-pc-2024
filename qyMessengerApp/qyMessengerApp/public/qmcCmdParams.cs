using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    public unsafe struct QMC_APP_PARAMS
	{

	//  appObjPrefix= "qm"  +  iSeqNoSelected_appObjPrefix. 2014/02/10
	public fixed char appObjPrefix[32];
	//
	int iSeqNoSelected_appObjPrefix;    //  
										//  2015/02/01
	int tn_cliPipe;                     //  2015/02/02


		//
		public fixed char serverAddr[64];
		public int port;

		//
		public fixed char user[32];                     //  2014/02/10
	public fixed char passwd[32];
	//
	public fixed char rtspUrl[128];                 //  2014/12/12
										//
	//  2015/04/28
	bool ucbNoDvt;                     //  2015/04/28
	bool ucbNoEvt;                     //  2015/10/28

	//  2016/04/26
	bool ucbDbgDvt;                        //  2016/04/26
	bool ucbDbgEvt;                        //  2016/05/05
													//
	bool ucbDbgOnvif;                  //  2016/0


	//  2015/07/18
	int iTestNo;

		//
		bool bMcu;
		//				 	
		QY_MESSENGER_ID idInfo_mcu;

		//
		bool bService;

		//
		bool bConfServer;
		//
		bool bFileServer;


		//
		bool bUseMachineKey;
		bool bUseAuthTypeMcu;
		bool ucbNotNeedLocked;                         //  

		//
		public bool canStartConf;
		public bool bNoPrompt;
		bool bNoAcceptAv;

		//  2015/10/04
		ushort usPktResType_suggested;

}
;

}
