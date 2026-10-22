using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	public unsafe struct QY_BUF
	{
		public uint resourceType;     // 资源类型
		public uint len;
		public byte* data;
	}
	;

public unsafe struct QY_STR
	{
		public uint len;
		public uint resourceType;
		public byte* szData;
	}
	;

	public struct QY_DMITEM
	{
		public int type;
		public string des;
		public string pData;
		//  long				flg;
		//  long				data;

		public QY_DMITEM(int t1)
		{
			type = t1; des = null; pData = null;
		}

		public QY_DMITEM(int t1, string d1)
		{
			type = t1; des = d1; pData = null;
		}
		public QY_DMITEM(int t1,string d1,string p1)
        {
			type = t1;des = d1;pData = p1;
        }
	}
	;

 
}
