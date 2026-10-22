using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
	public const int BI_RGB = 0;
	public const int BI_RLE8 = 1;
	public const int BI_RLE4 = 2;
	public const int BI_BITFIELDS = 3;
	public const int BI_JPEG = 4;
	public const int BI_PNG = 5;

	//
	public const int cntof_buf5 = 5;

}

namespace qy
{
	public unsafe struct TMP_buf5
	{
		public fixed byte buf[Consts.cntof_buf5];
	}

	partial class qyFuncs
{


		/// <summary>
		/// 
		/// </summary>
		/// <param name="fourcc"></param>
		/// <param name="fourccStr"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static unsafe int makeBmpInfoHeader_rgb(uint bits, int iWidth, int iHeight, BITMAPINFOHEADER* p)
		{
			if (null==p) return -1;

			mymemset((IntPtr)p, 0, sizeof(BITMAPINFOHEADER));

			p->biSize = sizeof(BITMAPINFOHEADER);
			p->biWidth = iWidth;
			p->biHeight = iHeight;
			p->biPlanes = 1;
			p->biBitCount = (short)bits;
			p->biCompression = Consts.BI_RGB;
			p->biSizeImage = (p->biWidth * p->biBitCount + 31) / 32 * 4 * p->biHeight;
			p->biClrUsed = (bits <= 8) ? 1 << (int)bits : 0;

			return 0;
		}

		public static unsafe int makeBmpInfoHeader_yuy2(uint bits, int iWidth, int iHeight, BITMAPINFOHEADER* p)
		{
			if (null==p) return -1;

			mymemset((IntPtr)p, 0, sizeof(BITMAPINFOHEADER));

			p->biSize = sizeof(BITMAPINFOHEADER);
			p->biWidth = iWidth;
			p->biHeight = iHeight;
			p->biPlanes = 1;
			p->biBitCount = 16; //  (  WORD  )bits;
			p->biCompression = Consts.CONST_fourcc_YUY2;
			p->biSizeImage = (p->biWidth * p->biBitCount + 31) / 32 * 4 * p->biHeight;
			p->biClrUsed = (bits <= 8) ? 1 << (int)bits : 0;

			return 0;
		}


		public static unsafe int makeBmpInfoHeader_yv12(uint bits, int iWidth, int iHeight, BITMAPINFOHEADER* p)
		{
			if (null==p) return -1;

			//  2014/07/20
			bits = 12;

			//
			mymemset((IntPtr)p, 0, sizeof(BITMAPINFOHEADER));

			p->biSize = sizeof(BITMAPINFOHEADER);
			p->biWidth = iWidth;
			p->biHeight = iHeight;
			p->biPlanes = 1;
			p->biBitCount = 12;
			p->biCompression = Consts.CONST_fourcc_YV12;
			p->biSizeImage = (p->biWidth * p->biBitCount + 31) / 32 * 4 * p->biHeight;
			p->biClrUsed = (bits <= 8) ? 1 << (int)bits : 0;

			return 0;

		}

		public static unsafe int makeBmpInfoHeader_i420(uint bits, int iWidth, int iHeight, BITMAPINFOHEADER* p)
		{
			if (null==p) return -1;

			//  2014/07/20
			bits = 12;

			//
			mymemset((IntPtr)p, 0, sizeof(BITMAPINFOHEADER));

			p->biSize = sizeof(BITMAPINFOHEADER);
			p->biWidth = iWidth;
			p->biHeight = iHeight;
			p->biPlanes = 1;
			p->biBitCount = 12;
			p->biCompression = Consts.CONST_fourcc_I420;
			p->biSizeImage = (p->biWidth * p->biBitCount + 31) / 32 * 4 * p->biHeight;
			p->biClrUsed = (bits <= 8) ? 1 << (int)bits : 0;

			return 0;

		}

		//  2014/07/23
		public static unsafe int makeBmpInfoHeader_yuv(int iFourcc, uint bits, int iWidth, int iHeight, BITMAPINFOHEADER* p)
		{
			switch (iFourcc)
			{
				case Consts.CONST_fourcc_YV12:
					return makeBmpInfoHeader_yv12(bits, iWidth, iHeight, p);
					break;
				case Consts.CONST_fourcc_I420:
					return makeBmpInfoHeader_i420(bits, iWidth, iHeight, p);
					break;
				default:
					break;
			}

			//
			return -1;
		}




		//
		public static unsafe byte* iFourcc2Str(int fourcc, byte* fourccStr, uint size)
		{
			//byte chs[5] = "";
			TMP_buf5 buf5;
			int i = 0;

			i = 0; buf5.buf[i] = (byte  )fourcc;
			i++; buf5.buf[i] = (byte  )(fourcc >> 8);
			i++; buf5.buf[i] = (byte  )(fourcc >> 16);
			i++; buf5.buf[i] = (byte  )(fourcc >> 24);
			i++; buf5.buf[i] = 0;

			//
			if (null==fourccStr)
			{
				//OutputDebugString(  CQyString(  (  char  *  )chs  )  +  _T(  "\n"  )  );
			}

			//
			if (null!=fourccStr && size > 4)
			{
				//
				//safeStrnCpy(  (  char  *  )chs,  fourccStr,  size  );
				mystrncpy(fourccStr, (byte*)buf5.buf, (int)size);
				//
				return fourccStr;
			}

			return null;
		}

		//
		public static uint myMAKEFOURCC(byte ch0, byte ch1, byte ch2, byte ch3)
		{
			return ((uint)(byte)(ch0) | ((uint)(byte)(ch1) << 8) | ((uint)(byte)(ch2) << 16) | ((uint)(byte)(ch3) << 24));
		}

		public static uint myMAKEFOURCC(char ch0, char ch1, char ch2, char ch3)
        {
			return myMAKEFOURCC((byte)ch0, (byte)ch1, (byte)ch2, (byte)ch3);
        }


		//
		public static unsafe int fourccStr2i(  byte* fourccStr  )
{
	if  (  null==fourccStr  )  return  -1;
    
	//return  mmioFOURCC(  fourccStr[0],  fourccStr[1],  fourccStr[2],  fourccStr[3]  );
	return  (int)myMAKEFOURCC(fourccStr[0], fourccStr[1], fourccStr[2], fourccStr[3]  );
	}



		//





}
}
