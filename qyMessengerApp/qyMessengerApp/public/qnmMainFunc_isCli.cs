using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		//  2014/10/29
		public static unsafe int newTaskId(object pDb, int iDbType)
		{
			int tt = 0;
			return (int)getuiNextTranNo(null, 0, ref tt);
		}

		public static MIS_CHANNEL getChannelByType(MIS_CNT pMisCnt, uint uiChannelType)
		{
			int i;

			if (null==pMisCnt) return null;

			if (0==uiChannelType) return pMisCnt.channels[0];

			for (i = 0; i < pMisCnt.channels.Length; i++)
			{
				if (pMisCnt.channels[i].uiType == uiChannelType) return pMisCnt.channels[i];
			}

			return pMisCnt.channels[0];

		}


		public static bool bChannelOk(MIS_CHANNEL pChannel)
		{
			if (null==pChannel) return false;
			MIS_CNT pMisCnt = pChannel.pMisCnt;
			if (null==pMisCnt) return false;
			CCtxQmc pProcInfo = (CCtxQmc)pMisCnt.pProcInfoParam;
			if (null==pProcInfo) return false;
			CCtxQyMc pQyMc = pProcInfo.pQyMc;
			if (null==pQyMc) return false;
			//
			bool bDbg = false;

			//
			if (pChannel.bSessionOpen)
			{
				uint dwTickCnt = myGetTickCount(null);
				//
				if (pChannel.uiType != Consts.CONST_channelType_talking)
				{
					//
#if DEBUG
					if (bDbg)
					{
						int iE_r = (int)(dwTickCnt - pChannel.status.dwTickCnt_lastStartToRecv);
						int iE_s = (int)(dwTickCnt - pChannel.status.dwTickCnt_lastEndSending);
						int n_outputQ = getQ2Nodes(pChannel.outputQ2);
						int n_toSndQ = getQ2Nodes(pChannel.toSendQ2);
						string str = string.Format("bChannelOk: iE_r {0},iE_s {1}, nQNodes_o {2},nQNodes_toSnd {3}\n", iE_r, iE_s,n_outputQ,n_toSndQ);
						traceLog(str);
					}
#endif


					//
					if (dwTickCnt - pChannel.status.dwTickCnt_lastStartToRecv > pQyMc.cfg.iTimeoutInMs_channelIdle_others
						&& dwTickCnt - pChannel.status.dwTickCnt_lastEndSending > pQyMc.cfg.iTimeoutInMs_channelIdle_others)
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Channel %s timeout, idle too long, iTimeoutInMs_channelsIdle_others %d"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pQyMc->cfg.iTimeoutInMs_channelIdle_others);
						return false;
					}
#if  false
				if  (  dwTickCnt  -  pChannel->status.dwTickCnt_lastStartToRecv  >  pQyMc->cfg.iTimeoutInMs_channelIdle_recv_others  )  {
					qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  (  ""  ),  _T(  "IsClient"  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "Channel %s timeout, idle too long, iTimeoutInMs_channelsIdle_recv_others %d"  ),  qyGetDesByType1(  CONST_channelTypeTable,  pChannel->uiType  ),  pQyMc->cfg.iTimeoutInMs_channelIdle_recv_others  );
					return  FALSE;
				}
#endif
				}
				else
				{
					if (dwTickCnt - pChannel.status.dwTickCnt_lastStartToRecv > pQyMc.cfg.iTimeoutInMs_channelIdle_recv_talk)
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Channel %s timeout, idle too long, iTimeoutInMs_channelsIdle_recv_talk %d"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pQyMc->cfg.iTimeoutInMs_channelIdle_recv_talk);
						return false;
					}
					//  2017/10/03. 当talk通道堵塞时，应尽快重新连接
					if (dwTickCnt - pProcInfo.status.dwTickCnt_sendTo_applyForOrgReq < 5000
						&& pProcInfo.status.dwTickCnt_sendTo_applyForOrgReq > pChannel.status.dwTickCnt_lastStartToRecv + 10000)
					{
						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Channel %s timeout, idle too long, dwTickCnt_sendTo_applyForOrgReq > dwTickCnt_lastStartToRecv + 10000"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType));
						return false;
					}
					//  2012/05/21.以下为了防止误判,用另一个变量做了一个辅助. 这样让判断至少在超时2次后才生效
					bool tmp_bOk = true;
					if (!isQ2Empty(pChannel.toSendQ2))
					{   //  2012/05/21
						if (dwTickCnt - pChannel.status.dwTickCnt_lastEndSending > pQyMc.cfg.iTimeoutInMs_channelIdle_others)
						{
							tmp_bOk = false;
						}
					}
					if (tmp_bOk) pChannel.status.dwTickCnt_noSending_start = 0;
					if (pChannel.status.dwTickCnt_noSending_start!=0)
					{
						if (dwTickCnt - pChannel.status.dwTickCnt_noSending_start > pQyMc.cfg.iTimeoutInMs_channelIdle_others + 5000)
						{
							//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), _T("IsClient"), 0, _T(""), _T(""), _T("Channel %s timeout, idle too long, iTimeoutInMs_channelsIdle_others %d"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), pQyMc->cfg.iTimeoutInMs_channelIdle_others);
							return false;
						}
					}
					if (!tmp_bOk)
					{
						if (0==pChannel.status.dwTickCnt_noSending_start)
						{
							pChannel.status.dwTickCnt_noSending_start = dwTickCnt;
						}
					}
				}
			}

#if false
			//  2015/9/11
			if (pProcInfo!=null)
			{
				if (pProcInfo.status.waitToQuit.dwTickCnt_start)
				{
					return FALSE;
				}
			}
#endif
			//
			if (pQyMc.b_notifyPanic) return false;

			//
			return (!pQyMc.bQuit && !pProcInfo.bQuit_threadProc_isCli && pChannel.bSessionOpen && !pChannel.status.ulbSessionErr);

		}


		//
		//  2014/06/04
		public static unsafe bool isEmpty_ROUTE_sendLocalAv(ref ROUTE_sendLocalAv pRoute)
		{
			bool bRet = false;

			if (0!=pRoute.videoConference_idInfo_to.ui64Id) goto errLabel;
			if (0!=pRoute.route.idInfo_to.ui64Id) goto errLabel;
			int i;
			for (i = 0; i < Consts.CONST_maxMsgrs_sendTo; i++)
			{
				if (0!=pRoute.route.mems_to_idInfo_ui64Id[i]) goto errLabel;
			}

			bRet = true;
		errLabel:

			return bRet;
		}

		public static unsafe int chkToApplyForChannel(CCtxQmc pProcInfo, MIS_CHANNEL pChannel, int channelType)
		{
			int iErr = -1;
			string str;

			bool bChannelReady = false;
			QY_Q2 pQ2 = null;

			if (null==pChannel)
			{
				MIS_CNT pMisCnt = pProcInfo.pMisCnt;
				if (null==pMisCnt) return -1;
				pChannel = getChannelByType(pMisCnt, (uint)channelType);
				if (null==pChannel) return -1;
			}
			channelType = (int)pChannel.uiType;

			//
			switch (channelType)
			{
				case Consts.CONST_channelType_media:
					bChannelReady = pProcInfo.av.video.bChannelReady;
					pQ2 = pProcInfo.mediaQ2;
					break;

				case Consts.CONST_channelType_realTimeMedia:
					bChannelReady = pProcInfo.av.audio.bChannelReady;
					pQ2 = pProcInfo.realTimeMediaQ2;
					break;
				default:
					break;
			}
			if (null==pQ2) goto errLabel;

			//
			if (bChannelReady)
			{
				iErr = 0; goto errLabel;
			}

			//	
			MIS_MSG_applyForChkChannels_qmc msg_applyForChkChannels;
			int dwTickCnt; dwTickCnt = (int)myGetTickCount(null);
			int i;

			//  	
			if (Math.Abs((int)(dwTickCnt - pChannel.status.dwLastTickCnt_applyForChkChannels)) > 2000)
			{


				//  traceLogA(  "toShareBmp: applyForChkChannels"  );		
				str  = string.Format("Channel {0} not Ready, applyForChkChannels", qyGetDesByType1(CONST_channelTypeTable, channelType));
				showInfo_open(0, null, str);


				//
				mymemset((IntPtr)(byte*)&msg_applyForChkChannels, 0, sizeof(MIS_MSG_applyForChkChannels_qmc));
				msg_applyForChkChannels.uiType = Consts.CONST_misMsgType_applyForChkChannels_qmc;
				//msg_applyForChkChannels.pMisCnt = pChannel->pMisCnt;

				//		
				if (0!=q2PostMsgAndTrigger((IntPtr)(byte*)&msg_applyForChkChannels, (uint)sizeof(MIS_MSG_applyForChkChannels_qmc), pQ2))
				{
					showNotification(0, 0, 0, 0, 0, 0, _T("chkToApplyForChannels: applyForChkChannels failed"));
				}

				//					   	
				pChannel.status.dwLastTickCnt_applyForChkChannels = (uint)dwTickCnt;
			}


			//  2015/05/12
# if  __DEBUG__
			//traceLog(  _T(  "toShareVideo_post: bVideoChannelReady false. skipped"  )  );
#endif


			iErr = 0;

		errLabel:
			return iErr;
		}


	}
}
