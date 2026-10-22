using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
{

		public static int startChannel(CCtxQmc pProcInfo, MIS_CNT pMisCnt, MIS_CHANNEL pChannel, string hint)
		{
			int iErr = -1;
			uint dwThreadDaemonId;

			//
			if (hint == null) hint = "";

			//
			if (null==pProcInfo || null==pChannel)
			{
				//assert(pProcInfo);
				//assert(pChannel);

				//traceLogA("startChannel: pProcInfo or pChannel is null");
				return -1;
			}

			if (pChannel.hThread_io!=null)
			{

				if (bChannelOk(pChannel))
				{
					iErr = 0; goto errLabel;
				}

#if false
				waitForObject(&pChannel->hThread_io, 0);
				if (!pChannel->hThread_io)
				{
					qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("%s is closed"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType));
				}
#endif
				waitForThread(ref pChannel.hThread_io, 0);
				if (null==pChannel.hThread_io)
				{
					string str = string.Format( ("{0} is closed"), qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));
					showInfo_open(null, "startChannel", str);
				}

			}

			//if (pMisCnt.commEncCtx.ucbPublicKeyVerified)
			{       //  2008/06/09, 需要确认后，才可以启动收发线程。				 

				if (null==pChannel.hThread_io)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("Try to start channel %s %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType), (hint ? hint : _T("")));
					string str=string.Format("Try to start channel {0} {1}", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType), hint);
					showInfo_open(null, "startChannel", str);

					//
					switch (pChannel.uiType)
					{                   //  2008/05/14. 对实时数据来说，最重要是当前数据。为了避免未发出的数据积累，先在这里清除了。
						case Consts.CONST_channelType_media:
						case Consts.CONST_channelType_realTimeMedia:
						case Consts.CONST_channelType_rtOp:
							emptyQ2(pChannel.toSendQ2);
							emptyQ2(pChannel.outputQ2);
							break;
						default:
							break;
					}

#if false
					LPTHREAD_START_ROUTINE lpStartAddress = mcThreadProc_mis_io;
				//
				//						
				LPVOID lpParameter = pProcInfo->pSgiParam;
					//  2016/09/09
					lpParameter = pChannel;
					//
					pChannel->hThread_io = CreateThread(NULL, 0, lpStartAddress, lpParameter, CREATE_SUSPENDED, &dwThreadDaemonId);
					if (!pChannel->hThread_io) goto errLabel;
					pChannel->dwThreadId_io = dwThreadDaemonId;
					if (ResumeThread(pChannel->hThread_io) == -1) traceLogA("ResumeThread recv failed");
#endif

					//
					pChannel.hThread_io = new Thread(new ParameterizedThreadStart(mcThreadProc_mis_io));
					pChannel.hThread_io.Start(pChannel);

					//  2008/03/21
					//  bChkMessengerChannelsReady(  (  QY_MC  *  )pProcInfo->pQyMc  );
				}
			}

			iErr = 0;
		errLabel:

			if (iErr!=0)
			{
				//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("startChannel failed. %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType));
				Console.WriteLine("startChannel failed. {0}", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));
			}

			return iErr;
		}


		//
		public static int waitForChannel(CCtxQmc pProcInfo, MIS_CNT pMisCnt, uint uiChannelType, bool bForceQuit)
		{
			MIS_CHANNEL pChannel = null;

			if (null!=(pChannel = getChannelByType(pMisCnt, uiChannelType))
				&& pChannel.hThread_io!=null)
			{
				if (!bForceQuit)
				{
					//waitForObject(&pChannel->hThread_io, 0);
					waitForThread(ref pChannel.hThread_io, 0);
				}
				else
				{
					pChannel.status.ulbSessionErr = true;
					//waitForObject(&pChannel->hThread_io, 20000);
					waitForThread(ref pChannel.hThread_io, 20000);
					if (pChannel.hThread_io!=null)
					{
						//traceLogA("waitForChannel failed.");
						string str=string.Format("waitForChannel failed. {0}", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));
						showInfo_open(null, "waitForChannel", str);
					}
				}

				if (null==pChannel.hThread_io)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, (""), pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("waitForChannel ok. %s"), qyGetDesByType1(CONST_channelTypeTable, pChannel->uiType));
					string str=string.Format("waitForchannel ok. {0}", qyGetDesByType1(CONST_channelTypeTable, pChannel.uiType));
					showInfo_open(null, "waitForChannel", str);
				}
			}

			return 0;
		}

	}
}
