using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	public struct myPLAY_AUDIO_DATA
	{
		public uint uiType;

		//  
		public int lPktId_getNewPkt;                      //  2015/02/13.???????. ???????

		//
		public TRANSFER_AUDIO_dataMemHead head;

		//
		public uint uiSize_dec;                                //  2016/12/10
															   //public QY_AUDIO_HEADER ah;                                     //  2016/12/06
		public WAVEFORMATEX wfx;

		//  char									*		pBuf;									//  2009/12/12
		public QY_MEMORY memory;                                   //  2010/04/23

		//
		//char												buf[1];

	}
	;

	//
	public class CmyPLAY_AUDIO_DATA
	{
		public myPLAY_AUDIO_DATA myPlayAudioData;

	};



}
