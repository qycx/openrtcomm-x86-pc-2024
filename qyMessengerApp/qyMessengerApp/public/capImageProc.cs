using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe int new_myGetImageInfo(bool ucbFixedSize,            //  
				   bool ucbAutoClip,                                          //  根据目标矩形自动裁剪，使目标矩形被充满
				   bool ucb3d,                                                    //  3d显示用， 2014/11/14
				   int iW_org, int iH_org,
				   int iX, int iY, uint iW, uint iH,
				   uint* piW_i, uint* piH_i,
				   int* piX_src_i, int* piY_src_i,
				   int* piX_dst, int* piY_dst, uint* piW_dst, uint* piH_dst)
		{
			//TCHAR tBuf[256];

			if (null==piX_dst || null==piY_dst || null==piW_dst || null==piH_dst) return -1;
			if (0==iW_org || 0==iH_org) return -1;
			if (0==iW || 0==iH)
			{
				*piW_i = *piH_i = 0;
				*piX_src_i = *piY_src_i = 0;
				*piX_dst = *piY_dst = 0; *piW_dst = *piH_dst = 0;
				return 0;
			}

			if (ucbFixedSize)
			{                   //  2??ü·?′ó??D?
								//
				*piW_i = (uint)iW_org;
				*piH_i = (uint)iH_org;
				//
				if (iW_org > (int)iW) *piX_dst = 0;
				else *piX_dst = (int)(iX + (iW - iW_org) / 2);

				if (iH_org > (int)iH) *piY_dst = 0;
				else *piY_dst = (int)(iY + (iH - iH_org) / 2);
				//
				*piX_src_i = *piY_src_i = 0;
				//
				*piW_dst = (uint)iW_org;
				*piH_dst = (uint)iH_org;

				return 0;
			}


			//
			

			double f;

			//
			f = iW_org / (iH_org * 1.0f  );
			if (f * iH > iW)
			{               //  ì??-
							//
				int tmp_iW = (int)iW;
				if (!ucb3d)
				{
					//tmp_iW  =  tmp_iW  /  4  *  4;
					//tmp_iW  =  tmp_iW  /  4  *  4;
				}
				//
				*piX_src_i = *piY_src_i = 0;
				*piW_i = (uint)tmp_iW;
				*piH_i = (uint)(tmp_iW / f);            //  iW  /  f;
												//
				*piW_dst = *piW_i;  //  iW;
				*piH_dst = *piH_i;  //  (  int  )(  iW  /  f  );
				*piX_dst = (int)(iX + (iW - *piW_dst) / 2.0f  );
				*piY_dst = (int)(iY + (iH - *piH_dst) / 2.0f  );
				return 0;
			}

			//
			*piX_src_i = *piY_src_i = 0;
			*piH_i = iH;
			*piW_i = (uint)(iH * f);
			//  ì??í
			*piH_dst = iH;
			*piW_dst = (uint)(iH * f);
			*piY_dst = iY;
			*piX_dst = (int)(iX + (iW - *piW_dst) / 2.0f  );

			return 0;
		}

	}
}
