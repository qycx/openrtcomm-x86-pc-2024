using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
    public const int cntof_buf1024 = 1024;
    public const int cntof_buf256 = 256;
}


//
namespace qy
{
    unsafe struct TMP_buf4
    {
        public fixed byte buf[4];
    }
    unsafe struct TMP_buf2
    {
        public fixed byte buf[2];
    }
    unsafe struct TMP_buf8
    {
        public fixed byte buf[8];
    }

    public unsafe struct TMP_buf256
    {
        public fixed byte buf[Consts.cntof_buf256];
    }

    public unsafe struct TMP_buf1024
    {
        public fixed byte buf[Consts.cntof_buf1024];
    }


    //
    partial class qyFuncs
    {

        public static unsafe uint qyhtoni(uint x)
        {
            TMP_buf4 buf4;
            //fixed byte buf[4];

            buf4.buf[0] = (byte)((x & 0xff000000) >> 24);
            buf4.buf[1] = (byte)((x & 0xff0000) >> 16);
            buf4.buf[2] = (byte)((x & 0xff00) >> 8);
            buf4.buf[3] = (byte)((x & 0xff));

            return *(uint*)buf4.buf;
        }
        public static unsafe int qyhtoni(int x)
        {
            return (int)qyhtoni((uint)x);
        }

        public static unsafe uint qyntohi(uint x)
        {
            uint iRet;
            //unsigned  char buf[4];
            TMP_buf4 buf4;

            byte* buf = buf4.buf;
            int* px = (int*)&x;
            mymemcpy((IntPtr)buf4.buf, (IntPtr)px, 4);
            iRet = (uint)((buf4.buf[0] << 24) + (buf4.buf[1] << 16) + (buf4.buf[2] << 8) + buf4.buf[3]);

            return iRet;
        }
        public static unsafe int qyntohi(int x)
        {
            return (int)qyntohi((uint)x);
        }

        public static unsafe ushort qyhtons(ushort x)
        {
            //unsigned  char buf[2];
            TMP_buf2 buf2;

            buf2.buf[0] = (byte)((x & 0xff00) >> 8);
            buf2.buf[1] = (byte)((x & 0xff));

            return *(ushort*)buf2.buf;
        }

        public static unsafe ushort qyntohs(ushort x)
        {
            ushort usRet;
            //unsigned  char buf[2];
            TMP_buf2 buf2;

            byte* buf = buf2.buf;
            short* px = (short*)&x;
            mymemcpy((IntPtr)buf, (IntPtr)px, 2);
            usRet = (ushort)((buf[0] << 8) + (buf[1]));

            return usRet;
        }


        public static unsafe UInt64 qyhtonl64(UInt64 x)
        {
            //unsigned  char buf[8];
            TMP_buf8 buf8;

            buf8.buf[0] = (byte)((x & 0xff00000000000000) >> 56);
            buf8.buf[1] = (byte)((x & 0xff000000000000) >> 48);
            buf8.buf[2] = (byte)((x & 0xff0000000000) >> 40);
            buf8.buf[3] = (byte)((x & 0xff00000000) >> 32);
            buf8.buf[4] = (byte)((x & 0xff000000) >> 24);
            buf8.buf[5] = (byte)((x & 0xff0000) >> 16);
            buf8.buf[6] = (byte)((x & 0xff00) >> 8);
            buf8.buf[7] = (byte)((x & 0xff));


            return *(UInt64*)buf8.buf;
        }

        public static unsafe UInt64 qyntohl64(UInt64 x)
        {
            UInt64 l64Ret = 0;
            UInt64 l64 = 0;
            //unsigned  char buf[sizeof(x)];
            TMP_buf8 buf8;
            int i;

            byte* buf = buf8.buf;
            UInt64* px = &x;
            mymemcpy((IntPtr)buf, (IntPtr)px, 8);
            //  l64Ret  =  (  buf[0]  <<  56  )  +  (  buf[1]  <<  48  )  +  (  buf[2]  <<  40  )  +  (  buf[3]  <<  32  )
            //	 +  (  buf[4]  <<  24  )  +  (  buf[5]  <<  16  )  +  (  buf[6]  <<  8  )  +  buf[7];;
            for (i = 0; i < 8; i++)
            {
                l64 = buf[i];
                l64Ret += l64 << ((8 - 1 - i) * 8);
            }

            return l64Ret;
        }

        //
        public static void MACRO_htons(ref ushort s)
        { s = qyhtons(s); }
        public static void MACRO_htons(ref short s)
        { s = (short)qyhtons((ushort)s); }

        public static void MACRO_htoni(ref uint l)
        { l = qyhtoni(l); }
        public static void MACRO_htoni(ref int l)
        { l = (int)qyhtoni((uint)l); }
        public static void MACRO_htonl64(ref UInt64 l64)
        { l64 = qyhtonl64(l64); }
        public static void MACRO_htonl64(ref Int64 l64)
        { l64 = (Int64)qyhtonl64((UInt64)l64); }

        public static void MACRO_ntohs(ref ushort s)
        { s = qyntohs(s); }
        public static void MACRO_ntohs(ref short s)
        { s = (short)qyntohs((ushort)s); }
        public static void MACRO_ntohi(ref uint l)
        { l = qyntohi(l); }
        public static void MACRO_ntohi(ref int l)
        { l = (int)qyntohi((uint)l); }

        public static void MACRO_ntohl64(ref UInt64 l64)
        { l64 = qyntohl64(l64); }       //  2007/05/10
        public static void MACRO_ntohl64(ref Int64 l64)
        { l64 = (Int64)qyntohl64((UInt64)l64); }       //  2007/05/10






    }
}




