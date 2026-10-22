using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	public struct PLAY_AUDIO_procInfo_var
	{
		public uint uiSampleTimeInMs;                             //  2009/05/03
		public uint dwTickCnt_uiSampleTimeInMs_internal;           //  2012/02/24. audioPlayer????
		public uint dwTickCnt_uiSampleTimeInMs_syncAv;             //  2012/02/18. ?video???

		//
		public uint dwTickCnt_uiSampleTimeInMs_syncRecv;           //  2012/02/24. ?newRecv????, ????????

		//
		public uint uiSampleTimeInMs_newRecvd;                     //  2009/05/03
		public uint dwTickCnt_uiSampleTimeInMs_newRecvd;

		//  
		public int nTimes_noAudioData;

		public bool bVideoReady;

		//  2012/02/19
		public uint uiTimeInMs_audio_remains;                          //  ???????????
		public uint dwTickCnt_GetCurrentPosition;                     //  ??GetCurrentPosition???

		//  2015/07/29
		public uint dwTickCnt_beforePlay;


	};

	//  2012/02/24
	public unsafe class PLAY_AUDIO_procInfo: QY_qThreadProcInfo_common
	{
		//
		public QY_qBuf2 qBuf2;

		//
		public uint uiTranNo;                                     //  2009/04/27

		//
		public QY_AUDIO_HEADER ah;                                             //  2010/06/17

		public AUDIO_PLAY_CFG playCfg;                                     //  2009/06/03

		//  2013/05/24
		//public fixed char playbackDevDesc[48];                          //  ??????????

		//  2017/06/15
		public uint uiPlayType;
		public ulong dwLastTickCnt_startPlayer;

		//
		/*
		public struct             VOE_PLAY_webRtc                            {
					 void* pVoePlay;
		};
		public VOE_PLAY_webRtc webRtc;
		*/

		
		//		
		public PLAY_AUDIO_procInfo_var m_var;

		//				
		public bool bNeedStop_webRtc_my_inStream;                      //  2018/06/16

		//
		public int nTimes_noAudioData_webRtc;

		//
		public struct PLAY_AUDIO_procInfo_cache
		{
			//
			public uint uiBufSize;
			public byte* pBuf;
			//
			public byte* pLeft;    //					=	NULL;									//  2009/04/27
			public uint nLeft; //						=	0;

			//
			public ulong dwTickCnt_lastAudioData;

				 };
		public PLAY_AUDIO_procInfo_cache cache;

		//  2017/06/22				 
		public struct PLAY_AUDIO_procInfo_lowLatency		{
			public		 int mql;
			public int xql;
			//
			public int last_mql;
			public int last_xql;
			//
			public int last_dangerousMql;
			public uint dwTickCnt_last_dangerousMql;

			//
			public uint dwTgt_start1;

			//
			public bool bNeedAdjust;
			public ulong dwTickCnt_last_adjust;

			//  2017/10/13
			public struct PLAY_AUDIO_procInfo_ptsInfo
			{
						 public uint uiPts_lastRecvd;
						public uint nErr_ptsMissed;
			};
			public PLAY_AUDIO_procInfo_ptsInfo ptsInfo;
			//
			public struct PLAY_AUDIO_procInfo_avg_pkt
			{
						 public int nBytes_ptks;
							public int n_pkts;
						//
						public int nAvg_pkt_inMs;
			};
			public PLAY_AUDIO_procInfo_avg_pkt avg_pkt;
			//
			public int xql_ok;
			public int min_mql_ok;
			public int max_mql_ok;

			//
			public struct PLAY_AUDIO_procInfo_lowLatency_stat
            {
				public int xql;
				public uint dwTickCnt_xql;
				public int mql;
				public uint dwTickCnt_mql;
				//
				public uint dwTickCnt_start;
				//
				public uint iIntervalInMs_xql_mql;

			};
			public PLAY_AUDIO_procInfo_lowLatency_stat stat;

		};
		public PLAY_AUDIO_procInfo_lowLatency	lowLatency;


}		 ;



//
//
public struct PARAM_fillD3d
	{
	public uint nIntervalInMs_fillData;
	public bool bFailed_getTextureInfo_toFill;                 //  2014/11/25
														//bool										bFailed_devCtx_stillUsed;						//  2014/11/25
														//bool										bFailed_FinishCommandList;

}
;


//
//
public struct TMP_fps_info
	{
	public int iCount;                                         //  帧数.2014/07/02.用d3d的get_fps( )来得到player的fps.然后计算帧间隔，限制各时间在realInterval - 3, realInterval + 3间。fps放在player里，在playList里显示出来	
		public uint curTimeInMs;
		public uint lastTimeInMs;
		public float fps_real;
		//
		public uint avgTimePerFrameInMs;
		//
		public uint avgTimePerFrameInMs_real;                     //  2015/09/10

}
;

	public struct PLAY_VIDEO_procInfo_mql
	{
		public ulong dwTgt_start1;
		public int mql;
		public int xql;

		//
		public int last_mql;                                       //  last_min_qNodes_left;
		public int last_xql;

		//
		public int mql_ok;
		public int mql_big_ok;
		public int xql_ok;

		//
		public bool bNeedReset;
	};

	public struct PLAY_VIDEO_procInfo_calc
	{
		public uint uiPts_start;
		public uint dwTgt_start;
	};

	public struct PLAY_VIDEO_procInfo_lowLatency
	{

		public PLAY_VIDEO_procInfo_mql mql;

		//
		public bool bNoAudio;                                      //  2016/04/10
																   //
		public bool bAccurate;                                     //  2016/04/11

		//
		public bool bNeedAdjust;                                   //  2015/09/13			
																   //
		public int nAheadInMs;
		public int nAheadInMs_gt_mql;                              //  2015/03/02. greater than
		public int nDelayInMs_lt_mql;                              //  less than

		//
		public int nTimes_sleep;
		public uint dwToInMs_toPostDraw;                          //  2012/02/07
																   //
		public uint dwToInMs_1740;

		//
		public PLAY_VIDEO_procInfo_calc calc;


	};

	public struct PLAY_VIDEO_procInfo_fillD3d
	{
		public bool bNeedFill;                                        //  2014/11/25
																	  //
		public PARAM_fillD3d param0, param1;
		//
		public bool bFilled_d3dData;                               //  2013/08/08
	};

	public struct PLAY_VIDEO_procInfo_status
	{
		public uint dwLastTgt_postToDraw;                            //  2012/01/26
		public int nInterval_real_postToDraw;
		//
		public int iSampleTimeInMs_last;
		public uint uiPts_last;                                        //  2015/02/19

		//
		//int									last_nQNodes_postToDraw;						//  2015/09/16

		//
		public bool bNoMsgAndWait;                                 //  2012/03/01
		public int nTimes_wait;                                    //  2015/09/11
															//
		public PLAY_VIDEO_procInfo_fillD3d fillD3d;
		//							
		public byte ucSkipped_for_d3dData;                            //  2013/09/09

		//  2015/02/25
		public TMP_fps_info fpsInfo;                                       //  2015/09/10

		//
	};


	public unsafe struct PLAY_VIDEO_procInfo_var
	{
		public uint uiSampleTimeInMs;                             //  2009/05/03
		public uint dwTgt_uiSampleTimeInMs_internal;
		//
		public uint dwTickCnt_uiSampleTimeInMs_syncRecv;

		public uint uiSampleTimeInMs_newRecvd;                     //  2009/05/03
		public uint dwTickCnt_uiSampleTimeInMs_newRecvd;

		//  2015/09/30
		public ushort usPktResType;                                    //  2015/09/30. 

		//
		//  2015/02/20
		//
		public IntPtr hWnd_display;                                  //  2009/07/25
		public bool ucbDisplayAllFrames;                          //  2009/07/25

		//
		public fixed char tWhere[64];                                       //  2015/09/13

		//
		public PLAY_VIDEO_procInfo_lowLatency lowLatency;


		//
		public PLAY_VIDEO_procInfo_status status;

	};

	//
	//
	public unsafe class PLAY_VIDEO_procInfo: QY_qThreadProcInfo_common
	{                                               //  2009/05/02


		//
		public uint uiTranNo;                                     //  2009/04/27

		//
		public QY_VIDEO_HEADER vh;
		public ushort usFps;                                           //  2015/02/20. 这个来自于传输过来的策略。

		//
		public uint uiPlayType;
		public ulong dwLastTickCnt_startPlayer;

		//
		public int nTimes_noData;

		//
		public PLAY_VIDEO_procInfo_var m_var;

		public QY_Q2 displayQ2;                                        //  2009/07/25
		public bool ucbDisplayQInited;					

}		 ;



//


public class QY_PLAYER
{                                               //  2009/05/02
	public QY_MESSENGER_ID idInfo_recorder;                                //  

		public uint uiSampleTimeInMs_start;                            //  2009/05/27

		public PLAY_AUDIO_procInfo audio;
		public PLAY_VIDEO_procInfo video;

		public int iTaskId;                                        //  2009/09/17

		//
		public uint uiTranNo_player;                               //  2016/02/18


		//  2014/12/10
		public ulong dwLastTickCnt_doApplyForPlayer;                       //  2014/12/10

		//
		public QY_PLAYER()
        {
			audio = new PLAY_AUDIO_procInfo();
			video = new PLAY_VIDEO_procInfo();
        }

		//
		public unsafe PLAYER_ID getPlayerId()
        {
			PLAYER_ID playerId;
			qyFuncs.mymemset((IntPtr)(byte*)&playerId, 0, sizeof(PLAYER_ID));
			int i;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

			if (this.uiTranNo_player == 0) return playerId;

			for ( i = 0;i<pProcInfo.av.usCnt_players;i++)
            {
				QY_PLAYER pPlayer = qyFuncs.getPlayerByIndex(pProcInfo, i);
				if (pPlayer.uiTranNo_player == this.uiTranNo_player)
				{
					playerId.index_player = i;
					playerId.uiTranNo_player = this.uiTranNo_player;
					break;
				}
            }
			return playerId;

        }
}
;



    
}
