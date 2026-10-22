using System;
using System.Collections.Generic;
using System.Text;


partial class Consts
{


}



namespace qy
{
	public partial class qyFuncs
	{
		//  #define		CONST_qnmVer_is								"024705"			//  
		//  #define		CONST_qnmVer_is								"025401"				//  
		public const string CONST_qnmVer_is = "02660751";               //  2017/10/19	"02660258"				//  2017/06/15	"02650267"				//  2016/10/12	//  "02630602"				//  2015/08/21	//  "02622960"				//  
																		//


		//
		public static string qnmVerStr(int iServiceId)
		{
			string p = "";

			switch (iServiceId)
			{
				case Consts.CONST_qyServiceId_is:
					p = CONST_qnmVer_is;
					break;
				default:
					break;

			}

			return p;
		}


		public static int qyGetCustomId()
		{
			return 0;

		}

		//
		public static unsafe int parse_qnmVerStr(byte* ver, ushort* pshVer, byte* pblVer, byte* pb_mantissa_ver)
		{
			if (null==ver) return -1;
			if (mystrlen(ver) < 6) return -1;

			int lhVer = 0;
			byte blVer = 0;
			byte b_mantissa_ver = 0;

			//byte buf[4 + 1];
			TMP_buf5 buf5;

			mymemcpy((IntPtr)buf5.buf, (IntPtr)ver, 4);
			buf5.buf[4] = 0;
			lhVer = (int)myatol(buf5.buf);
			//
			mymemcpy((IntPtr)buf5.buf, (IntPtr)(ver + 4), 2);
			buf5.buf[2] = 0;
			blVer = (byte)myatol(buf5.buf);
			//
			mymemcpy((IntPtr)buf5.buf, (IntPtr)(ver + 6), 2);
			buf5.buf[2] = 0;
			b_mantissa_ver = (byte)myatol(buf5.buf);


			//
			if (pshVer!=null) *pshVer = (ushort)lhVer;
			if (pblVer!=null) *pblVer = blVer;
			if (pb_mantissa_ver!=null) *pb_mantissa_ver = b_mantissa_ver; //  2015/02/15

			return 0;
		}

	}

}



