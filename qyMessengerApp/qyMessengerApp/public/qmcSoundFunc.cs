using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const int WAVE_FORMAT_PCM = 1;

}

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int makeWaveFormat_pcm(ushort nChannels, int iHertz, ushort wBitsPerSample, ref WAVEFORMATEX pParam)
		{
			fixed (WAVEFORMATEX* p = &pParam)
			{
				mymemset((IntPtr)p, 0x00, sizeof(WAVEFORMATEX));
				p->wFormatTag = Consts.WAVE_FORMAT_PCM;
				p->nChannels = nChannels;                                       //  DEFAULT_audio_nChannels;		//  2;
																				//p->wBitsPerSample = (wBitsPerSample!=0) ? wBitsPerSample : 16;       //  DEFAULT_audio_wBitsPerSample;	//  16;
				if (wBitsPerSample != 0) p->wBitsPerSample = wBitsPerSample;
				else p->wBitsPerSample = 16;

				p->cbSize = 0;
				p->nSamplesPerSec = (uint)iHertz;  //  22.05 KHz

				p->nBlockAlign = (ushort)((p->wBitsPerSample / 8) * p->nChannels);
				//  p->nAvgBytesPerSec	=	p->nSamplesPerSec  *  (  p->wBitsPerSample  /  8  );
				p->nAvgBytesPerSec = p->nSamplesPerSec * p->nBlockAlign;
			}

			return 0;
		}

	}
}
