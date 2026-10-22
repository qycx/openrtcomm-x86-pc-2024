using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static string qyGetDesByType(QY_DMITEM[] pTable, long type)
		{
			int i;

			if (null==pTable) return null;           //  2005/02/08

			for (i = 0; pTable[i].type != -1; i++)
			{
				if (pTable[i].type == type) return pTable[i].des;
			}
			return null;

		}

		//  该函数专门为提供显示用，防止返回NULL导致指针错误
		public static string qyGetDesByType1(QY_DMITEM[] pTable, long type)
		{
			string p = null;
			return (null!=(p = qyGetDesByType(pTable, type)) ? p : (""));
		}

		//
		public static int qyGetTypeByDes(QY_DMITEM[] pTable,string des)
        {
			int i;
			
			if (null == pTable) return -1;
			if (null == des) return -1;

			//
			for (i = 0; pTable[i].type != -1; i++)
            {
				if (pTable[i].des == des) return pTable[i].type;
            }
			return -1;
        }


	}
}
