using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const string CONST_mutexNamePrefix_decA_inDataCacheQ = ("decA_in");
	public const string CONST_mutexNamePrefix_decA_tmpDataQ = ("decA_tmp");

}

namespace qy
{
    partial class qyFuncs
{
		public static unsafe void mcThreadProc_decAudio(object lpParameter)
		{

			QY_TRANSFORM pTransform = (QY_TRANSFORM)lpParameter;
			if (null == pTransform) return;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
			if (null == pProcInfo) return;
			TRANSFORM_AUDIO_procInfo pTransformAudio = pTransform.audio;

			//  int								i;

			using (CQyMalloc mallocObj = new CQyMalloc())
			{
				;
				MIS_MSGU* pMsg = null;
				uint len;

				IM_CONTENTU* pContent = null;

				uint dwToInMs = Consts.DEFAULT_dwToInMs;

				uint dwTickCnt_lastAudioData = myGetTickCount(null); ;                       // 

				using (CQyMalloc mallocObj_pkt_dec_pBuf = new CQyMalloc())
				{
					;
					myPLAY_AUDIO_DATA pkt_dec;

					//using (CQyMalloc mallocObj_pMsgBuf = new CQyMalloc())
					{
						;
						//MIS_MSGU* pMsgBuf = (MIS_MSGU*)mallocObj_pMsgBuf.mallocf(sizeof(MIS_MSGU));
						//if (null == pMsgBuf) return;

						

						traceLog("mcThreadProc_decAudio enters");

						//MACRO_setStep(pTransformAudio.debugStep, CONST_threadStep_start);

						//
						pTransformAudio.bRunning = true;
						//  memset(  &pTransformAudio.m_var,  0,  sizeof(  pTransformAudio.m_var  )  );

						if (null == (pMsg = (MIS_MSGU*)mallocObj.mallocf(Consts.CONST_iAllocType_longTimeMsg, sizeof(MIS_MSGU)))) goto errLabel;
						//
						//  2015/02/14
						mymemset((IntPtr)(byte*)&pkt_dec, 0, sizeof(myPLAY_AUDIO_DATA));
						//
						uint uiBufSize_pkt_dec = pProcInfo.cfg.uiBufSize_myPlayAudioData;  //  offsetof(  myPLAY_AUDIO_DATA,  buf  )  +  pProcInfo.cfg.uiBufSize_myPlayAudioData;	//  
						int iAllocType = 0; //  CONST_iAllocType_audio
											//  if  (  !(  pTransform.audio.pPkt_dec  =  (  myPLAY_AUDIO_DATA  *  )mallocObj_pPkt_dec.mallocf(  iAllocType,  uiBufSize_pPkt_dec  )  )  )  goto  errLabel;
											//
						if (null == mallocObj_pkt_dec_pBuf.mallocf(iAllocType, (int)uiBufSize_pkt_dec)) goto errLabel;
						if (0 != mallocObj_pkt_dec_pBuf.get(ref pkt_dec.memory)) goto errLabel;
						//
						//if  (  pTask.ucbVideoConferenceStarter  )  
						{
							//  if  (  !(  pTransform.audio.pMsgTask_toCompress_videoConference  =  (  MIS_MSG_TALK  *  )mallocObj_pMsgTask_toCompress.malloc(  sizeof(  MIS_MSG_TALK  )  )  )  )  goto  errLabel;
						}

						//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", _T("IsClient"), 0, _T(""), _T(""), _T("%s %d starts"), pTransformAudio.q2.cfg.name, pTransform.index_activeMems_from);

						///////////////////////////
						//  2016/12/10
						//
						//  2014/03/23	 	
						GENERIC_Q_cfgEx qCfgEx; //  2012/11/10
						GENERIC_Q_CFG tmpCfg;

						//TCHAR tBuf[128];
						//
						int tn = (int)myGetTickCount(null);
						int iTaskId = pTransform.iTaskId;
						int index_activeMems_from = pTransform.index_activeMems_from;
						//
						int size = 0;


						//
#if false
						size = sizeof(myPLAY_AUDIO_DATA);
						pTransformAudio.pCurPkt = (myPLAY_AUDIO_DATA*)mymalloc(size);
						if (null==pTransformAudio.pCurPkt) goto errLabel;
						mymemset((IntPtr)pTransformAudio.pCurPkt, 0, size);
#endif




						//////////////////////////

						//
						fixed (PKT_info_toDec* pcurPktInfo_toDec = &pTransformAudio.curPktInfo_toDec)
						{
							mymemset((IntPtr)(byte*)pcurPktInfo_toDec, 0, sizeof(PKT_info_toDec));
						}
						pTransformAudio.curPktInfo_toDec.pMIS_MSGU = pMsg;
						pTransformAudio.curPktInfo_toDec.pPkt_dec = &pkt_dec;
						pMsg->uiType = 0;
						pTransformAudio.curPktInfo_toDec.pkt_encA.bufSize = Consts.bufSize_tmpPktEncA;


						//
						for (; !pTransformAudio.bQuit;)
						{

							if (0==dwToInMs)
							{
								//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo.who_showInfo, 0, _T(""), _T(""), _T("Warning: mcThreadProc_transformAudio: dwToInMs %d"), dwToInMs);
								dwToInMs = Consts.DEFAULT_dwToInMs;
						}

							//  2009/07/14
							M_setStep(ref pTransformAudio.debugStep, Consts.CONST_threadStep_toWait);

							//
							uint dwTickCnt0 = myGetTickCount(null);
							uint dwTickCnt;

							//
							if (pProcInfo.cfg.b_useAudioQ2_toDecA)
							{
								myWaitOne(pTransformAudio.q2.hEvent0, (int)dwToInMs);
							}
							else
                            {
								myWaitOne(pTransformAudio.qBuf2.hEvent0, (int)dwToInMs);
                            }

							//
							for (; !pTransformAudio.bQuit;)
							{

								//
								//
								//if (0!=getNextPktInfo_toDec(ref pTransformAudio.curPktInfo_toDec, pTransformAudio.q2)) break;
								if (pProcInfo.cfg.b_useAudioQ2_toDecA)
								{
									if (isQ2Empty(pTransformAudio.q2)) break;
								}
								else
                                {
									if (pTransformAudio.qBuf2.isQ2Empty()) break;
                                }

								//
								if (0==pTransform.audio.curPktInfo_toDec.lPktId_getNewPkt)
								{
									//MACRO_qyAssert(0, _T("decAudio: id_pkt is 0"));
									traceLog("decAudio err: id_pkt is 0");
								}
								//
								long old_pktId = pTransform.audio.curPktInfo_toDec.lPktId_getNewPkt;
								//doDecodeAudio(pTransform, pMsgBuf);
								doDecodeAudio(pTransform, null);
								if (pTransform.audio.curPktInfo_toDec.lPktId_getNewPkt == old_pktId)
								{   //  如果没有处理掉就需要释放
									clean_PKT_info_toDec(ref pTransform.audio.curPktInfo_toDec, _T(""));
								}




							}

						}


					}
				}
			}

errLabel:

	 //
	 M_setStep(ref pTransformAudio.debugStep, Consts.CONST_threadStep_toExit  );

//  2009/05/28
exitTaskAvFrom_audio(pProcInfo, pTransform,"decAudio quit"  );


//  2016/12/10
//  2014/03/25
/*
clean_myPLAY_AUDIO_DATA((IntPtr)pTransformAudio.pCurPkt1, _T(  "decAudio"  )  );
			IntPtr tmpp = (IntPtr)pTransformAudio.pCurPkt1;
	 MACRO_safeFree(ref tmpp  );
			pTransformAudio.pCurPkt1 = (myPLAY_AUDIO_DATA * )tmpp;
*/
//  2014/03/24



			//
			//qyShowInfo1(CONST_qyShowType_qwmComm,  0,  "", _T(  "IsClient"  ),  0,  _T(  ""  ),  _T(  ""  ),  _T(  "%s %d ends."  ),  pTransformAudio.q2.cfg.name,  pTransform.index_activeMems_from  );

	 pTransformAudio.bRunning  =  false;

	 M_setStep(ref pTransformAudio.debugStep, Consts.CONST_threadStep_end  );

traceLog(  "mcThreadProc_decAudio  leaves"  );

			

	 return;
}


		public static unsafe int getNextPktInfo_toDec(ref PKT_info_toDec pCurPktInfoParam, QY_Q2 pDataQ2)
		{
			int iErr = -1;

			fixed (PKT_info_toDec* pCurPktInfo = &pCurPktInfoParam) {
				MIS_MSGU* pMsg = (MIS_MSGU*)pCurPktInfo->pMIS_MSGU;
				IM_CONTENTU* pContent = null;

				//	
				if (pCurPktInfo->bGot_msg)
				{
					//
					if (!pCurPktInfo->info.bGot_pkt)
					{
						//
						switch (pMsg->uiType)
						{
							case Consts.CONST_misMsgType_task:
								pContent = (IM_CONTENTU*)&pMsg->task.data.buf;
								switch (pContent->uiType)
								{
									case Consts.CONST_imCommType_transferAudioData:
										{
											TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
											TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
											//
											if (pCurPktInfo->info.index_mems >= 0 && pCurPktInfo->info.index_mems + 1 < pReq->usCnt)
											{
												//										
												int i = pCurPktInfo->info.index_mems;
												TRANSFER_AUDIO_dataMemHead* pMem = &pReq_mems[i];
												int iPos = pCurPktInfo->info.iPos;

												if (iPos + pReq_mems[i].uiLen < pReq->uiLen)
												{

													pCurPktInfo->info.iPos += (int)pMem->uiLen;
													pCurPktInfo->info.index_mems++;
													//			  
													pCurPktInfo->info.bGot_pkt = true;

													//		
													M_set_pktId_getNewPkt(pCurPktInfo);

												}
											}
											//
										}
										break;
									default:
										break;
								}
								break;
							case Consts.CONST_misMsgType_input:
								pContent = (IM_CONTENTU*)&pMsg->input.data.buf;
								switch (pContent->uiType)
								{
									case Consts.CONST_imCommType_transferAudioData:
										{
											TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
											TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
											int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
											//
											if (pCurPktInfo->info.index_mems >= 0 && pCurPktInfo->info.index_mems + 1 < pReq->usCnt)
											{
												//										
												int i = pCurPktInfo->info.index_mems;
												TRANSFER_AUDIO_dataMemHead* pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];
												int iPos = pCurPktInfo->info.iPos;

												if (iPos + pMem->uiLen < pReq->uiLen)
												{

													pCurPktInfo->info.iPos += (int)pMem->uiLen;
													pCurPktInfo->info.index_mems++;
													//			  
													pCurPktInfo->info.bGot_pkt = true;

													//		
													M_set_pktId_getNewPkt(pCurPktInfo);

												}
											}
											//
										}
										break;
									case Consts.CONST_imCommType_transferVideoData:
										{
											TRANSFER_VIDEO_DATA* pReq = &pContent->transferVideoData;
											TRANSFER_VIDEO_dataMemHead* pReq_mems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
											int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
											//
#if DEBUG
											 ref TRANSFER_VIDEO_DATA  tmp_pReq = ref *pReq;
											//
											if (false)
											{
												int ii;
												for (ii = 0; ii < pReq->usCnt; ii++)
												{
													TRANSFER_VIDEO_dataMemHead* pMem = getMem(pReq_mems, bufSize, ii);
													string str = "dbg: getNextPktInfo_toDec: " + "pkt" + ii + ".pts is "  +  pMem->uiPts  +  " .len "  +  pMem->uiLen;
													showInfo_open(0, null, str);
												}
											}
											//
#endif
											//
											if (pCurPktInfo->info.index_mems >= 0 && pCurPktInfo->info.index_mems + 1 < pReq->usCnt)
											{
												//										
												int i = pCurPktInfo->info.index_mems;
												TRANSFER_VIDEO_dataMemHead* pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];
												int iPos = pCurPktInfo->info.iPos;


												//
#if DEBUG
												if  (  false )
												{
													string str = "dbg: nextPkt " + i + ".pts is " + pMem->uiPts  +  " .len " +  pMem->uiLen;
													showInfo_open(0, null, str);
													//
												}
#endif

												//
												if (iPos + pMem->uiLen < pReq->uiLen)
												{

													pCurPktInfo->info.iPos += (int)pMem->uiLen;
													pCurPktInfo->info.index_mems++;
													//			  
													pCurPktInfo->info.bGot_pkt = true;

													//		
													M_set_pktId_getNewPkt(pCurPktInfo);

												}
											}
											//
										}
										break;

									default:
										break;
								}
								break;

							default:
								break;
						}

						//
						if (!pCurPktInfo->info.bGot_pkt)
						{
							pCurPktInfo->bGot_msg = false;
						}
					}
				}


				//	
				if (!pCurPktInfo->bGot_msg)
				{
					//
					uint len;
					len = (uint)sizeof(MIS_MSGU);
					if (0!=q2GetMsg(pDataQ2, (IntPtr)pMsg, &len)) goto errLabel;
					mymemset((IntPtr)(((byte*)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));
					//
					switch (pMsg->uiType)
					{
						case Consts.CONST_misMsgType_task:
							pContent = (IM_CONTENTU*)&pMsg->task.data.buf;
							switch (pContent->uiType)
							{
								case Consts.CONST_imCommType_transferAudioData:
									{
										TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;

										//
										pCurPktInfo->bGot_msg = true;
										//
										mymemset((IntPtr)(byte*)&pCurPktInfo->info, 0, sizeof(PKT_info_toDec_info)  );
										//
										if (0==pReq->uiLen) goto errLabel;
										//
										pCurPktInfo->info.bGot_pkt = true;

										//		
										M_set_pktId_getNewPkt(pCurPktInfo);

										//  2017/10/13
#if __DEBUG__
										if (pReq->usCnt > 1)
										{
											int ii = 0;
										}
#endif

									}
									break;
								default:
									break;
							}
							break;
						case Consts.CONST_misMsgType_input:
							pContent = (IM_CONTENTU*)&pMsg->input.data.buf;
							switch (pContent->uiType)
							{
								case Consts.CONST_imCommType_transferAudioData:
									{
										TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;

										//
										pCurPktInfo->bGot_msg = true;
										//
										mymemset((IntPtr)(byte*)&pCurPktInfo->info, 0, sizeof(PKT_info_toDec_info));
										//
										if (0 == pReq->uiLen) goto errLabel;
										//
										pCurPktInfo->info.bGot_pkt = true;

										//		
										M_set_pktId_getNewPkt(pCurPktInfo);

										//  2017/10/13
#if __DEBUG__
										if (pReq->usCnt > 1)
										{
											int ii = 0;
										}
#endif

									}
									break;
								case Consts.CONST_imCommType_transferVideoData:
									{
										TRANSFER_VIDEO_DATA* pReq = &pContent->transferVideoData;

										//
										pCurPktInfo->bGot_msg = true;
										//
										mymemset((IntPtr)(byte*)&pCurPktInfo->info, 0, sizeof(PKT_info_toDec_info));
										//
										if (0 == pReq->uiLen) goto errLabel;
										//
										pCurPktInfo->info.bGot_pkt = true;

										//		
										M_set_pktId_getNewPkt(pCurPktInfo);

										//  2017/10/13
#if DEBUG
										if (pReq->usCnt > 1)
										{
											int ii = 0;
										}
										//
										if (  false  )
										{
                                            TRANSFER_VIDEO_dataMemHead* pReq_mems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
                                            int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
											//
											int ii = 0;
                                            TRANSFER_VIDEO_dataMemHead* pMem = getMem(pReq_mems, bufSize, ii);
											string str = "dbg: getNextPktInfo_toDec: newPkt: " + "pkt" + ii + ".pts is " + pMem->uiPts + " .len " + pMem->uiLen;
											showInfo_open(0, null, str);
										}
#endif

                                    }
                                    break;

								default:
									break;
							}
							break;
							
						default:
							break;
					}
					//	
					if (!pCurPktInfo->bGot_msg) goto errLabel;

				}


				//
				if (!pCurPktInfo->info.bGot_pkt)
				{
					goto errLabel;
				}



				iErr = 0;

			errLabel:

				if (0!=iErr)
				{
					pCurPktInfo->bGot_msg = false;
				}

			}

			return iErr;

		}


		//
		public static unsafe int getNextPktInfo_toDec(ref PKT_info_toDec pCurPktInfoParam, QY_qBuf2 pDataQ2)
		{
			int iErr = -1;

			fixed (PKT_info_toDec* pCurPktInfo = &pCurPktInfoParam)
			{
				MIS_MSGU* pMsg = (MIS_MSGU*)pCurPktInfo->pMIS_MSGU;
				IM_CONTENTU* pContent = null;

				//	
				if (pCurPktInfo->bGot_msg)
				{
					//
					if (!pCurPktInfo->info.bGot_pkt)
					{
#if false
						//
						switch (pMsg->uiType)
						{
							case Consts.CONST_misMsgType_task:
								pContent = (IM_CONTENTU*)pMsg->task.data.buf;
								switch (pContent->uiType)
								{
									case Consts.CONST_imCommType_transferAudioData:
										{
											TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
											TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
											//
											if (pCurPktInfo->info.index_mems >= 0 && pCurPktInfo->info.index_mems + 1 < pReq->usCnt)
											{
												//										
												int i = pCurPktInfo->info.index_mems;
												TRANSFER_AUDIO_dataMemHead* pMem = &pReq_mems[i];
												int iPos = pCurPktInfo->info.iPos;

												if (iPos + pReq_mems[i].uiLen < pReq->uiLen)
												{

													pCurPktInfo->info.iPos += (int)pMem->uiLen;
													pCurPktInfo->info.index_mems++;
													//			  
													pCurPktInfo->info.bGot_pkt = true;

													//		
													M_set_pktId_getNewPkt(pCurPktInfo);

												}
											}
											//
										}
										break;
									default:
										break;
								}
								break;
							case Consts.CONST_misMsgType_input:
								pContent = (IM_CONTENTU*)pMsg->input.data.buf;
								switch (pContent->uiType)
								{
									case Consts.CONST_imCommType_transferAudioData:
										{
											TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
											TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
											int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;
											//
											if (pCurPktInfo->info.index_mems >= 0 && pCurPktInfo->info.index_mems + 1 < pReq->usCnt)
											{
												//										
												int i = pCurPktInfo->info.index_mems;
												TRANSFER_AUDIO_dataMemHead* pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];
												int iPos = pCurPktInfo->info.iPos;

												if (iPos + pMem->uiLen < pReq->uiLen)
												{

													pCurPktInfo->info.iPos += (int)pMem->uiLen;
													pCurPktInfo->info.index_mems++;
													//			  
													pCurPktInfo->info.bGot_pkt = true;

													//		
													M_set_pktId_getNewPkt(pCurPktInfo);

												}
											}
											//
										}
										break;
									case Consts.CONST_imCommType_transferVideoData:
										{
											TRANSFER_VIDEO_DATA* pReq = &pContent->transferVideoData;
											TRANSFER_VIDEO_dataMemHead* pReq_mems = (TRANSFER_VIDEO_dataMemHead*)pReq->buf_TRANSFER_VIDEO_dataMemHead_mems;
											int bufSize = Consts.bufSize_TRANSFER_VIDEO_dataMemHead_mems;
											//
											if (pCurPktInfo->info.index_mems >= 0 && pCurPktInfo->info.index_mems + 1 < pReq->usCnt)
											{
												//										
												int i = pCurPktInfo->info.index_mems;
												TRANSFER_VIDEO_dataMemHead* pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];
												int iPos = pCurPktInfo->info.iPos;

												if (iPos + pMem->uiLen < pReq->uiLen)
												{

													pCurPktInfo->info.iPos += (int)pMem->uiLen;
													pCurPktInfo->info.index_mems++;
													//			  
													pCurPktInfo->info.bGot_pkt = true;

													//		
													M_set_pktId_getNewPkt(pCurPktInfo);

												}
											}
											//
										}
										break;

									default:
										break;
								}
								break;

							default:
								break;
						}
#endif

						//
						if (!pCurPktInfo->info.bGot_pkt)
						{
							pCurPktInfo->bGot_msg = false;
						}
					}
				}


				//	
				if (!pCurPktInfo->bGot_msg)
				{
					//
					uint len;
#if false
					len = (uint)sizeof(MIS_MSGU);
					if (0 != q2GetMsg(pDataQ2, (IntPtr)pMsg, &len)) goto errLabel;
					mymemset((IntPtr)(((byte*)pMsg) + len), 0, (int)Math.Min(256, sizeof(MIS_MSGU) - len));
#endif
					IntPtr pHeadData = IntPtr.Zero;
					switch (pDataQ2.cfg.m_qType)
                    {
						case Consts.CONST_qType_decA:
							pHeadData = (IntPtr)(byte*)&pCurPktInfo->pkt_encA.memHead_a;
							//
                            pCurPktInfo->pkt_encA.len = pCurPktInfo->pkt_encA.bufSize;
                            if (0 != pDataQ2.q2GetMsg(pHeadData, (IntPtr)pCurPktInfo->pkt_encA.buf, (uint*)&pCurPktInfo->pkt_encA.len)) goto errLabel;
							//
                            break;
						case Consts.CONST_qType_decV:
							pHeadData = (IntPtr)(byte*)&pCurPktInfo->pkt_encV.memHead_v;
							//
                            pCurPktInfo->pkt_encV.len = pCurPktInfo->pkt_encV.bufSize;
                            if (0 != pDataQ2.q2GetMsg(pHeadData, (IntPtr)pCurPktInfo->pkt_encV.buf, (uint*)&pCurPktInfo->pkt_encV.len)) goto errLabel;
							//
                            break;
						default:
							showInfo_open(0, "", "getNextPktInfo failed; unprocessed qType");
							goto errLabel;
                    }
					//
					pCurPktInfo->bGot_msg = true;
					pCurPktInfo->info.bGot_pkt = true;
					//
					M_set_pktId_getNewPkt(pCurPktInfo);

				}


				//
				if (!pCurPktInfo->info.bGot_pkt)
				{
					goto errLabel;
				}



				iErr = 0;

			errLabel:

				if (0 != iErr)
				{
					pCurPktInfo->bGot_msg = false;
				}

			}

			return iErr;

		}






		//
		public static unsafe void clean_PKT_info_toDec(ref PKT_info_toDec p, string hint)
		{
			p.info.bGot_pkt = false;
		}


		//
		public static unsafe int doDecodeAudio(QY_TRANSFORM pTransform, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;

			int i;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();// pTransform.pProcInfo;//
			if (null==pProcInfo) return -1;

			ref QMC_cfg pQmcCfg = ref pProcInfo.cfg;//->get_qmc_cfg();


			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			//if (null==pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			/*
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null==pTranInfo_unused) return -1;
			*/
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null==pTask) return -1;

			//
			CTX_caller ctxCaller=new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, (CCtxQmc)pProcInfo, pTask.iIndex_taskInfo,"doDecodeAudio");
			if (null==pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();


#if __DEBUG__
			traceLog(_T("Not finished:  doDecodeAudio"));
#endif

#if true
			TRANSFORM_AUDIO_procInfo pTransformAudio				=	pTransform.audio;

#if false
			//myDRAW_VIDEO_DATA  *     pPkt  =  pTransformVideo->pCurPkt;
			if  (  !pTransformAudio.curPktInfo_toDec.bGot_msg
		||  !pTransformAudio.curPktInfo_toDec.info.bGot_pkt  )  
	{
		return  -1;
	}
	MIS_MSGU  *  pMsg_curPkt  =  (  MIS_MSGU  *  )pTransformAudio.curPktInfo_toDec.pMIS_MSGU;
			//
			if (pMsg_curPkt->uiType != Consts.CONST_misMsgType_input)
			{
				traceLog("doDecodeAudio: pMsg_curPkt.uiType err, only input msg accepted");
				return -1;
			}
	IM_CONTENTU  *  pContent  =  (  IM_CONTENTU  *  )pMsg_curPkt->input.data.buf;
			//
	if  (  pContent->uiType  !=  Consts.CONST_imCommType_transferAudioData  )  return  -1;
	TRANSFER_AUDIO_DATA  *  pReq  =  &pContent->transferAudioData;
	if  (  pReq->tmpInternal.param.index_activeMems_from  !=  pTransform.index_activeMems_from  )  {	
		//traceLogA(  "mcThreadProc_transformAudio: transferAudio.index_activeMems_from %d != transform.%d",  pReq->tmpInternal.param.index_activeMems_from,  pTransform.index_activeMems_from  );
		return  -1;
	}
#endif

	
	int  index_activeMems_from  =  pTransform.index_activeMems_from;

#if false
			int index_pMems_from = pReq->tmpInternal.param.index_pMems_from;

			//
			TASK_AV_FROM pFrom				=	null;

		 //  2014/11/12
		 pFrom  =  getTaskAvFrom(  pTransform,  index_pMems_from,  index_activeMems_from,  _T(  "doDecodeAudio"  )  );
		 if  (  null==pFrom  )  return  -1;
#endif
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];

			//
			uint uiModuleType_toDec  =  0;
		 switch  (  pActiveMem_from.avStream.obj.tranInfo.audio.compressor.ucCompressors  )  {
				 default:
						uiModuleType_toDec = Consts.CONST_moduleType_ffmpeg;
						break;
		 }
			//
			adjustModuleType_decA(ref uiModuleType_toDec);

		 //		 
		 switch  (  uiModuleType_toDec  )  {
				 case  Consts.CONST_moduleType_mediaCodec:
					   return  GuiShare.pf_doDecodeAudio_MediaCodec(  pTransform,  -1, index_activeMems_from,  (IntPtr)pMsgBuf_unused  );
					   break;
				 default:
						break;
		 }


			//
			if (pProcInfo.cfg.b_useAudioQ2_toDecA)
			{
				//doDecodeAudio_default_useAudioQ2(pTransform, -1, index_activeMems_from, pMsgBuf_unused);
				doDecodeAudio_default(pTransform, -1, index_activeMems_from, pMsgBuf_unused);


			}
			else
			{
				doDecodeAudio_default(pTransform, -1, index_activeMems_from, pMsgBuf_unused);
			}

#endif

			iErr = 0;

		errLabel:

			//  exit

			return iErr;
		}


		//
		public static unsafe int doDecodeAudio_default(QY_TRANSFORM pTransform, int index_pMems_from, int index_activeMems_from, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//
			if (null == pProcInfo) return -1;

			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			//if (null == pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			/*
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null == pTranInfo_unused) return -1;
			*/
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null == pTask) return -1;

			//
			CTX_caller ctxCaller = new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "doDecodeAudio_default");
			if (null == pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


#if true
			//  2014/05/08
			//_sntprintf(  pTransform.audio.debugStep.showInfo.tWhere,  mycountof(  pTransform.audio.debugStep.showInfo.tWhere  ),  _T(  "tt=%d"  ),  iTaskId  );
			pTransform.audio.debugStep.showInfo.uiStep = 0;


			//
			M_setMyStep(ref pTransform.audio.debugStep, 21);

			//  if  (  !pFrom->video.ucbInited  )  
			if (!pTransform.audio.ucbInited)
			{
				if (0 != initTaskAvFrom_audio(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform))
				{
					goto errLabel;
				}
				//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "doDecodeAudio_default: initTaskAvFrom_audio %d"  ),  index_pMems_from  );
			}
			if (!pTransform.audio.ucbInited)
			{
#if __DEBUG__
					 traceLogA(  "doDecodeAudio_default failed: audio is not inited"  );
#endif
				goto errLabel;
			}

			int old_pktId = 0;
			//
			for (; ; )
			{
				if (bTransformAudioQuit(pTransform, IntPtr.Zero, IntPtr.Zero))
				{
#if __DEBUG__
						  traceLog(_T(  "doTrans_default failed：quit is true"  )  );
#endif
					goto errLabel;
				}
				//
				M_setMyStep(ref pTransform.audio.debugStep, 100);    //  2014/07/17
																	 //
				for (; ; )
				{
					if (bTransformAudioQuit(pTransform, IntPtr.Zero, IntPtr.Zero)) goto errLabel;
					//
					M_setMyStep(ref pTransform.audio.debugStep, 105);   //  2014/07/17
																		//
					if (pProcInfo.cfg.b_useAudioQ2_toDecA)
					{
						if (0 == getNextPktInfo_toDec(ref pTransform.audio.curPktInfo_toDec, pTransform.audio.q2)) break;
					}
					else
					{
						if (0 == getNextPktInfo_toDec(ref pTransform.audio.curPktInfo_toDec, pTransform.audio.qBuf2)) break;
					}
					//
					M_setMyStep(ref pTransform.audio.debugStep, 106);    //  2014/07/17
																		 //
#if false
						Sleep(  40  );
#else
					//
					int nWaitInMs = 40;
					//
					nWaitInMs = 100;    //  2015/02/06. 是不是放大点能让cpu更悠闲点。但是，不能影响获取包的实时性
										//
					uint dwRet;
					QY_Q2 pAudioQ2 = (QY_Q2)pTransform.audio.q2;
					/*
	dwRet  =  WaitForSingleObject(pAudioQ2->hEvents[0], nWaitInMs  );
						if  (dwRet  ==  WAIT_FAILED  )  {
							showInfo_open(  0,  0, _T(  "SmplBsReader::LoadNextFrame failed, WaitForSingleObject failed"  )  );
							goto  errLabel;			  
						}
			  
					*/
					if (pProcInfo.cfg.b_useAudioQ2_toDecA)
					{
						myWaitOne(pAudioQ2.hEvent0, nWaitInMs);
					}
					else
					{
						myWaitOne(pTransform.audio.qBuf2.hEvent0, nWaitInMs);
					}
#endif
				}
				//
				M_setMyStep(ref pTransform.audio.debugStep, 108);    //  2014/07/17
																	 //
																	 //IntPtr pInput = IntPtr.Zero;
				//fixed (byte* pInput = pTransform.audio.curPktInfo_toDec.pkt_encA.buf) { }
				uint uiInputSize = 0;
				uint uiSampleTimeInMs = 0;
				uint uiPts = 0; //  2015/02/09
				if (pProcInfo.cfg.b_useAudioQ2_toDecA)
				{
					/*
					if (0 != dec_parseEncAPkt_i_useAudioQ2(Consts.CONST_uiTransformType_dec, pTransform, ref pTransform.audio.curPktInfo_toDec, &old_pktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts))
					{
#if __DEBUG__
	traceLog(_T("doDecodeAudio_default failed, parsePkt failed"));
#endif
						goto errLabel;
					}
					*/

				}
				else
				{
					if (0 != GuiShare.pf_dec_parseEncAPkt_i(Consts.CONST_uiTransformType_dec, pTransform, ref pTransform.audio.curPktInfo_toDec, ref old_pktId, ref uiInputSize, ref uiSampleTimeInMs, ref uiPts,null))
					{
#if __DEBUG__
	traceLog(_T("doDecodeAudio_default failed, parsePkt failed"));
#endif
						goto errLabel;
					}
					//pInput = pTransform.audio.curPktInfo_toDec.pkt_encA.buf;
				}

				//			  	
				M_setMyStep(ref pTransform.audio.debugStep, 109);  //  2014/07/17


				//
				ref PKT_info_toDec pPktInfo_toDec = ref pTransform.audio.curPktInfo_toDec;
				fixed (PKT_info_toDec* pPktInfo_toDec1 = &pPktInfo_toDec)
				{
					TRANSFER_AUDIO_dataMemHead* pMem=null;
					TRANSFER_AUDIO_dataMemHead tmp_pMem;
					//
					myPLAY_AUDIO_DATA* pPkt_dec = (myPLAY_AUDIO_DATA*)pPktInfo_toDec.pPkt_dec;
					byte* data = null;

					//
					if (pProcInfo.cfg.b_useAudioQ2_toDecA)
					{

						MIS_MSGU* pMsg = (MIS_MSGU*)pPktInfo_toDec.pMIS_MSGU;
						if (pMsg->uiType != Consts.CONST_misMsgType_input)
						{
							traceLog("doDecodeAudio_default failed, only input msg accepted");
							goto errLabel;
						}
						IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->input.data.buf;
						if (pContent->uiType != Consts.CONST_imCommType_transferAudioData) goto errLabel;

#if DEBUG
						ref myPLAY_AUDIO_DATA tmp_pPkt = ref *pPkt_dec;
#endif
						//
						//
						//
						int iPos = pPktInfo_toDec1->info.iPos;
						TRANSFER_AUDIO_DATA* pReq = &pContent->transferAudioData;
						if (pPktInfo_toDec1->info.index_mems < 0 || pPktInfo_toDec1->info.index_mems >= pReq->usCnt) goto errLabel;
						int i;
						i = pPktInfo_toDec1->info.index_mems;

						//
						TRANSFER_AUDIO_dataMemHead* pReq_mems = (TRANSFER_AUDIO_dataMemHead*)pReq->buf_TRANSFER_AUDIO_dataMemHead_mems;
						int bufSize = Consts.bufSize_TRANSFER_AUDIO_dataMemHead_mems;

						pMem = getMem(pReq_mems, bufSize, i);// &pReq_mems[i];

						if (iPos + pMem->uiLen > pReq->uiLen)
						{
							goto errLabel;
						}
						data = pReq->buf + iPos;

						//
						tmp_pMem = *pMem;
						//
						if (tmp_pMem.idInfo.ui64Id == 0)
						{
							tmp_pMem.idInfo.ui64Id = pMsg->input.data.route.idInfo_from.ui64Id;
						}
						if (tmp_pMem.uiTranNo_openAvDev == 0)
						{
							tmp_pMem.uiTranNo_openAvDev = pContent->transferAudioData.uiTranNo_openAvDev;
						}


					}
					else
                    {
						pMem = &pPktInfo_toDec1->pkt_encA.memHead_a;
						data = pPktInfo_toDec1->pkt_encA.buf;

						//
						tmp_pMem = *pMem;

					}

					//


					//
					transformTaskAudioPkt(pTransform, &tmp_pMem,data, pPkt_dec, pMsgBuf_unused);
				}



				//doDecVideoPkt(  pTransform,  pPkt,  pMsgBuf  );

				//
				M_setMyStep(ref pTransform.audio.debugStep, 110);  //  2014/07/17


				//  2014/08/11
				if (pTransform.audio.status.ucbDecFailed)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("doDecodeAudio_default: decFailed, need to reinited. %d"), index_pMems_from);
					goto errLabel;
				}

				//
				clean_PKT_info_toDec(ref pPktInfo_toDec, _T(""));

				//
				continue;
			}
#endif

			iErr = 0;
		errLabel:

			exitTaskAvFrom_audio(pProcInfo, pTransform, "doDecAudio_default quit");

			return iErr;
		}



		//
		public static unsafe int doDecodeAudio_default_useAudioQ2(QY_TRANSFORM pTransform, int index_pMems_from, int index_activeMems_from, MIS_MSGU* pMsgBuf_unused)
		{
			int iErr = -1;
			CCtxQyMc pQyMc = g.g_pQyMc;
			CCtxQmc pProcInfo = pQyMc.get_pProcInfo();//
			if (null == pProcInfo) return -1;

			ref MSGR_ADDR pAddr = ref pTransform.pAddr_logicalPeer1;
			//if (null == pAddr) return -1;
			int iTaskId = pTransform.iTaskId;
			//
			/*
			AV_TRAN_INFO* pTranInfo_unused = (AV_TRAN_INFO*)pTransform.pTranInfo_unused;
			if (null == pTranInfo_unused) return -1;
			*/
			//
			PROC_TASK_AV pTask = (PROC_TASK_AV)pTransform.pTask;
			if (null == pTask) return -1;

			//
			CTX_caller ctxCaller = new CTX_caller();
			//mymemset((IntPtr)(byte*)&ctxCaller, 0, sizeof(CTX_caller));

			//
			QMC_TASK_INFO pTaskInfo = (QMC_TASK_INFO)getQmcTaskInfoByIndex(ctxCaller, pProcInfo, pTask.iIndex_taskInfo, "doDecodeAudio_default");
			if (null == pTaskInfo) return -1;
			QMC_taskData_common pTaskData = pTaskInfo.getTaskData();
			if (pTaskData == null) return -1;
			if (pTaskData.uiType != Consts.CONST_taskDataType_conf) return -1;
			QMC_taskData_conf pTc = (QMC_taskData_conf)pTaskData;
			DLG_TALK_videoConference videoConference = pTc.getVideoConference();

			//
			if (index_activeMems_from < 0 || index_activeMems_from >= videoConference.usCntLimit_activeMems_from) return -1;
			DLG_TALK_videoConferenceActiveMemFrom pActiveMem_from = videoConference.activeMems_from[index_activeMems_from];


#if true
			//  2014/05/08
			//_sntprintf(  pTransform.audio.debugStep.showInfo.tWhere,  mycountof(  pTransform.audio.debugStep.showInfo.tWhere  ),  _T(  "tt=%d"  ),  iTaskId  );
			pTransform.audio.debugStep.showInfo.uiStep = 0;


			//
			M_setMyStep(ref pTransform.audio.debugStep, 21);

			//  if  (  !pFrom->video.ucbInited  )  
			if (!pTransform.audio.ucbInited)
			{
				if (0 != initTaskAvFrom_audio(pProcInfo, pActiveMem_from, index_activeMems_from, pTransform))
				{
					goto errLabel;
				}
				//qyShowInfo1(  CONST_qyShowType_qwmComm,  0,  "",  pProcInfo->who_showInfo,  0,  _T(  ""  ),  _T(  ""  ),  _T(  "doDecodeAudio_default: initTaskAvFrom_audio %d"  ),  index_pMems_from  );
			}
			if (!pTransform.audio.ucbInited)
			{
#if __DEBUG__
					 traceLogA(  "doDecodeAudio_default failed: audio is not inited"  );
#endif
				goto errLabel;
			}

			int old_pktId = 0;
			//
			for (; ; )
			{
				if (bTransformAudioQuit(pTransform, IntPtr.Zero, IntPtr.Zero))
				{
#if __DEBUG__
						  traceLog(_T(  "doTrans_default failed：quit is true"  )  );
#endif
					goto errLabel;
				}
				//
				M_setMyStep(ref pTransform.audio.debugStep, 100);    //  2014/07/17
																	 //
				for (; ; )
				{
					if (bTransformAudioQuit(pTransform, IntPtr.Zero, IntPtr.Zero)) goto errLabel;
					//
					M_setMyStep(ref pTransform.audio.debugStep, 105);   //  2014/07/17
																		//
					if (pProcInfo.cfg.b_useAudioQ2_toDecA)
					{
						if (0 == getNextPktInfo_toDec(ref pTransform.audio.curPktInfo_toDec, pTransform.audio.q2)) break;
					}
					else
					{
						if (0 == getNextPktInfo_toDec(ref pTransform.audio.curPktInfo_toDec, pTransform.audio.qBuf2)) break;
					}
					//
					M_setMyStep(ref pTransform.audio.debugStep, 106);    //  2014/07/17
																		 //
#if false
						Sleep(  40  );
#else
					//
					int nWaitInMs = 40;
					//
					nWaitInMs = 100;    //  2015/02/06. 是不是放大点能让cpu更悠闲点。但是，不能影响获取包的实时性
										//
					uint dwRet;
					QY_Q2 pAudioQ2 = (QY_Q2)pTransform.audio.q2;
					/*
	dwRet  =  WaitForSingleObject(pAudioQ2->hEvents[0], nWaitInMs  );
						if  (dwRet  ==  WAIT_FAILED  )  {
							showInfo_open(  0,  0, _T(  "SmplBsReader::LoadNextFrame failed, WaitForSingleObject failed"  )  );
							goto  errLabel;			  
						}
			  
					*/
					if (pProcInfo.cfg.b_useAudioQ2_toDecA)
					{
						myWaitOne(pAudioQ2.hEvent0, nWaitInMs);
					}
					else
					{
						myWaitOne(pTransform.audio.qBuf2.hEvent0, nWaitInMs);
					}
#endif
				}
				//
				M_setMyStep(ref pTransform.audio.debugStep, 108);    //  2014/07/17
																	 //
				IntPtr pInput = IntPtr.Zero;
				uint uiInputSize = 0;
				uint uiSampleTimeInMs = 0;
				uint uiPts = 0; //  2015/02/09
				if (pProcInfo.cfg.b_useAudioQ2_toDecA)
				{
					/*
					if (0 != dec_parseEncAPkt_i_useAudioQ2(Consts.CONST_uiTransformType_dec, pTransform, ref pTransform.audio.curPktInfo_toDec, &old_pktId, &pInput, &uiInputSize, &uiSampleTimeInMs, &uiPts))
					{
#if __DEBUG__
	traceLog(_T("doDecodeAudio_default failed, parsePkt failed"));
#endif
						goto errLabel;
					}
					*/

				}
				else
				{
					if (0 != GuiShare.pf_dec_parseEncAPkt_i(Consts.CONST_uiTransformType_dec, pTransform, ref pTransform.audio.curPktInfo_toDec, ref old_pktId, ref uiInputSize, ref uiSampleTimeInMs, ref uiPts,null))
					{
#if __DEBUG__
	traceLog(_T("doDecodeAudio_default failed, parsePkt failed"));
#endif
						goto errLabel;
					}
				}

				//			  	
				M_setMyStep(ref pTransform.audio.debugStep, 109);  //  2014/07/17


				//
				ref PKT_info_toDec pPktInfo_toDec = ref pTransform.audio.curPktInfo_toDec;
				MIS_MSGU* pMsg = (MIS_MSGU*)pPktInfo_toDec.pMIS_MSGU;
				if (pMsg->uiType != Consts.CONST_misMsgType_input)
				{
					traceLog("doDecodeAudio_default failed, only input msg accepted");
					goto errLabel;
				}
				IM_CONTENTU* pContent = (IM_CONTENTU*)&pMsg->input.data.buf;
				if (pContent->uiType != Consts.CONST_imCommType_transferAudioData) goto errLabel;
				myPLAY_AUDIO_DATA* pPkt_dec = (myPLAY_AUDIO_DATA*)pPktInfo_toDec.pPkt_dec;

#if DEBUG
				ref myPLAY_AUDIO_DATA tmp_pPkt = ref *pPkt_dec;
#endif
				//
				fixed (PKT_info_toDec* pPktInfo_toDec1 = &pPktInfo_toDec)
				{
					transformTaskAudioPkt_useAudioQ2(pTransform, &pMsg->input, &pContent->transferAudioData, pPktInfo_toDec1, pContent->transferAudioData.tmpInternal.param.index_pMems_from, pContent->transferAudioData.tmpInternal.param.index_activeMems_from, pPkt_dec, pMsgBuf_unused);
				}



				//doDecVideoPkt(  pTransform,  pPkt,  pMsgBuf  );

				//
				M_setMyStep(ref pTransform.audio.debugStep, 110);  //  2014/07/17


				//  2014/08/11
				if (pTransform.audio.status.ucbDecFailed)
				{
					//qyShowInfo1(CONST_qyShowType_qwmComm, 0, "", pProcInfo->who_showInfo, 0, _T(""), _T(""), _T("doDecodeAudio_default: decFailed, need to reinited. %d"), index_pMems_from);
					goto errLabel;
				}

				//
				clean_PKT_info_toDec(ref pPktInfo_toDec, _T(""));

				//
				continue;
			}
#endif

			iErr = 0;
		errLabel:

			exitTaskAvFrom_audio(pProcInfo, pTransform, "doDecAudio_default quit");

			return iErr;
		}

	}
}
