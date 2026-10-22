using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
{
		static int siTranNo = 0;

        //public static unsafe uint getuiNextTranNo(void* p0, int val, int* puiTranNo)
        public static uint getuiNextTranNo(object p0, int val, ref int puiTranNo)
        {
			
			if (0==val) val = 1;

			/*
			if (null != puiTranNo)
			{
				return (uint)Interlocked.Add(ref puiTranNo, val) + 1;
			}
			*/

			//
			return (uint)Interlocked.Add(ref siTranNo, val) + 1;

		}

	}
}
