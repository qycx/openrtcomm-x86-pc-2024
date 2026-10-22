using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
	public unsafe struct MY_DEBUG_STEP_showInfo
	{                                            //  2014/05/08
		public fixed char tWhere[16];
		public uint uiStep;
	};

	public struct MY_DEBUG_STEP
	{
		//M_debugStep_common
		//#define		M_debugStep_common										
		public int		nStep_debug;	
		public uint	dwTickCnt_step_debug;						//  2012/03/21

		/*
		//
		unsigned  int uiCnt_DMO_E_NOTACCEPTING;
		unsigned int nErrs_ProcessInput;
		unsigned int nErrs_ProcessOutput;
		unsigned int nErrs_pOutputBuffers_index;
		//
		struct                                             {
					 BOOL bInited;
		BOOL bStarted;
		//
		HRESULT hr_GetState_beforeStart;
		int fs_beforeStart;
		//
		BOOL bBeforeRun;
		HRESULT hr_run;
		//
		DWORD dwTickCnt_startToQuit;
		//
		HRESULT hr_GetState_beforeStop;
		int fs_beforeStop;
		//
		struct                                         {
						M_debugStep_common
						//
						HRESULT                                     hr;
						BOOL bPktGot;
		BOOL bParentQuit;
		//
		BOOL bFailed;
		//
	}
	FillBuffer;


					 //
				 }
fg;
//  2014/07/17
DEBUG_step_dec dec;
		*/
//
public int iErr_thread;                                //  2012/09/08
		
		//
		public MY_DEBUG_STEP_showInfo showInfo;
				 //
		

	};


	//
	public class MACRO_qyQThreadProcInfo_common1 
	{
		public QY_Q2 q2;
		public bool bQInited;
		public uint dwThreadId;
		//
		//public Thread hThread;
		public object hThread;
		//
		public int ind_hThread;
		//
		public bool bQuit;
		public uint dwTickCnt_start;
		public bool bRunning;
		public MY_DEBUG_STEP debugStep;

		//
		public MACRO_qyQThreadProcInfo_common1()
        {
			this.q2 = new QY_Q2();
        }

		//
		public void memset0()
        {

        }

	};



	public struct AUDIO_PLAY_CFG
	{
		public ushort usMinTimeInMsOfAudioDataPlaying;                 //  2009/06/14
																	   //  
		public ushort usAudioDataAddedToPlayEveryTime;                 //  2009/06/16
																	   //
		public ushort usNotifyIntervalInMs_needMoreAudioData;              //  2009/06/14. = CONST_notifyInterval
																		   //
		public ushort usMinTimeInMsToPlay;                             //  2012/01/27
																	   //
		public ushort usMaxTimeInMsToPlay;                             //  2009/06/03
		public ushort usMaxTimeInMsToBeRemoved;                            //  2009/06/03
	}
	;

}
