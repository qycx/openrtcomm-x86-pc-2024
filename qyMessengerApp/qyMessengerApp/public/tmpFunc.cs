using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace qy
{
    partial class qyFuncs
    {
        public static unsafe int mystrlen(byte* str)
        {

            if (str == null) return 0;
            int len = 0;
            for (; ; )
            {
                if (str[len] == 0) break;
                //
                len++;
            }

            return len;
        }

        public static unsafe int mywcslen(char* str)
        {

            if (str == null) return 0;
            int len = 0;
            for (; ; )
            {
                if (str[len] == 0) break;
                len++;
            }

            return len;
        }

        public static unsafe int mylstrlen(char* lpString)
        {
            return mywcslen(lpString);
        }


        //
        public static unsafe char* mylstrcpyn(char* dst, char* src, int iMaxLength)
        {
            return mytcsncpy(dst, src, iMaxLength);
        }

        public static unsafe char* mylstrcpyn(char* dst, string str_src, int iMaxLength)
        {
            return mytcsncpy(dst, str_src, iMaxLength);
        }


        public static unsafe char* mytcsncpy(char* dst, string str_src, int n)
        {
            if (dst == null) return null;
            if (str_src == null) return null;

            //
            char[] src = str_src.ToCharArray();
            /*
  if(n != 0)
  {
   char* d = dst;

                char* s = src;
 
   do
   {
    if((* d ++ = *s ++) == 0)
    {
     while (-- n != 0) *d ++ = (char)0;
     break;
    }

    
}
while (--n != 0) ;
  }*/

            if (dst == null) return null;
            if (src == null) return null;
            if (n != 0)
            {
                int ns = src.Length;
                //
                int i;
                for (i = 0; i < n; i++)
                {
                    if (i == ns)
                    {
                        dst[i] = (char)0;
                        break;
                    }
                    dst[i] = src[i];
                    if (src[i] == 0) break;
                }
                if (i == n) dst[i - 1] = (char)0;


            }

            return dst;
        }

        public static unsafe char* mytcsncpy(char* dst, char* str_src, int n)
        {
            if (dst == null || str_src == null) return null;
            return mytcsncpy(dst, new string(str_src), n);
        }

        //
        public static unsafe byte* mystrncpy(byte* destination, byte* source, int num)
        {
            if (destination == null || source == null || num == 0) return null;

            safeStrnCpy(source, destination, (uint)num);
            return destination;
        }


        public static unsafe void safeStrnCpy(string strIn, byte* strOut, uint size)
        {
            if (size == 0 || strIn == null || strOut == null) return;
            byte[] b_strIn = System.Text.Encoding.UTF8.GetBytes(strIn);
            int len = Math.Min((int)(size - 1), b_strIn.Length);
            Marshal.Copy(b_strIn, 0, (IntPtr)strOut, len);
            strOut[len] = (byte)0;
            return;
        }

        public static unsafe void safeStrnCpy(byte* strIn, byte* strOut, uint size)
        {
            if (size == 0 || strIn == null || strOut == null) return;
            int i;
            for (i = 0; i < size - 1; i++)
            {
                strOut[i] = strIn[i];
                if (strIn[i] == 0) break;
            }
            if (i == size - 1) strOut[i] = (byte)0;
            return;
        }


        public static unsafe void safeWcsnCpy(char* strIn, char* strOut, uint cntof_strOut)
        {
            if (cntof_strOut == 0 || strIn == null || strOut == null) return;
            int i;
            for (i = 0; i < cntof_strOut - 1; i++)
            {
                strOut[i] = strIn[i];
                if (strIn[i] == 0) break;
            }
            if (i == cntof_strOut - 1) strOut[i] = (char)0;
            return;
        }

        public static unsafe void safeTcsnCpy(char* strIn, char* strOut, uint cntof_strOut)
        {
            safeWcsnCpy(strIn, strOut, cntof_strOut);
        }


        //
        public static unsafe void safeTcsnCpy(string strIn, char* strOut, uint cntof_strOut)
        {
            if (strIn == null) return;
            var len = strIn.Length;
            var ptr = Marshal.StringToHGlobalUni(strIn);
            if (null != ptr)
            {
                char* ch = (char*)ptr.ToPointer();
                safeTcsnCpy(ch, strOut, cntof_strOut);

                Marshal.FreeHGlobal(ptr);
            }
            return;
        }



        public static unsafe int mylstrcmpi(char* lpString1, char* lpString2)
        {
            if (lpString1 == null || lpString2 == null) return -1;
            string str1 = new string(lpString1);
            string str2 = new string(lpString2);
            return string.Compare(str1, str2, true);
        }

        public static unsafe int mytcsicmp(char* string1, char* string2)
        {
            return mylstrcmpi(string1, string2);

        }
        public static unsafe int mytcsicmp(string string1, string string2)
        {
            return string.Compare(string1, string2, true);

        }





        public static DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);

        public static unsafe Int64 mytime(ref long seconds)
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan diff = now - dt1970;
            long l = (long)diff.TotalSeconds;
            //if (seconds != null)
            {
                seconds = l;
            }
            return l;
        }

        public static unsafe uint myinet_addr(byte* cp)
        {
            if (cp == null) return 0;
            return (uint)IPAddress.Parse(cp->ToString()).Address;
        }

        public static unsafe string mytoString(byte* p)
        {
            int len = mystrlen(p);
            byte[] a = new byte[len];
            for (int i = 0; i < len; i++)
            {
                a[i] = p[i];
            }
            return Encoding.UTF8.GetString(a);
        }


        public static bool myWaitOne(WaitHandle s, int millseconds)
        {
            bool bRet = false;

            if (s == null) return false;
            try
            {
                bRet = s.WaitOne(millseconds);
            }
            catch (Exception e)
            {
                goto errLabel;

            }
        errLabel:

            return bRet;
        }

        public static bool myEventSet(EventWaitHandle evt)
        {
            bool bRet = false;
            try
            {
                bRet = evt.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                goto errLabel;
            }
        errLabel:
            return bRet;
        }
        public static bool mySetEvent(EventWaitHandle evt)
        {
            return myEventSet(evt);
        }




        public static int mySemaRelease(Semaphore s, int releaseCount)
        {
            int iRet = -1;
            try
            {
                iRet = s.Release(releaseCount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                goto errLabel;
            }
        errLabel:
            return iRet;
        }

        public static uint myGetTickCount(CTX_qm_thread cqt)
        {
            //uint aa = (uint)System.Environment.TickCount & Int32.MaxValue;
            if (null == GuiShare.pf_myGetTickCount) return 0;
            return (uint)GuiShare.pf_myGetTickCount();
        }

        public static long myGetTickCount64(CTX_qm_thread cqt)
        {
            //uint aa = (uint)System.Environment.TickCount & Int32.MaxValue;
            if (null == GuiShare.pf_myGetTickCount) return 0;
            return GuiShare.pf_myGetTickCount();
        }


        public static uint mytimeGetTime()
        {
            return myGetTickCount(null);
        }

        public static uint myGetCurrentThreadId()
        {

            if (null == GuiShare.pf_myGetTid) return 0;

            return (uint)GuiShare.pf_myGetTid();


        }

        public static uint myGetCurrentProcessId()
        {
            //Thread.
            if (null == GuiShare.pf_myGetPid) return 0;
            return (uint)GuiShare.pf_myGetPid();
        }


        public static unsafe void mySleep(uint dwMilliseconds)
        {
            Thread.Sleep((int)dwMilliseconds);
        }

        public static bool bEqual_byteArray(byte[] b1, byte[] b2)
        {
            //1：

            //2：
            if (b1.Length != b2.Length)
            {
                return false;
            }

            //3：
            bool isEqual = true;

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                {
                    //System.out.println("different");
                    isEqual = false;
                    break;
                }
            }
            return isEqual;
        }

        //
        public static unsafe void MACRO_mysafeFree(ref byte* x)
        {
            if ((x != null)) { myfree((IntPtr)x); (x) = null; }
        }

        //
        public static unsafe long myatol(byte* bstr)
        {
            long lVal = 0;
            string str = mytoString(bstr);
            return long.Parse(str);

        }

        //
        public static string _T(string str)
        {
            if (str == null) str = "";
            return str;
        }

        public static void OutputDebugString(string str)
        {

        }

        public static bool byte2bool(byte b)
        {
            return b != 0;
        }

        public static byte bool2byte(bool b)
        {
            return (byte)(b ? 1 : 0);
        }

        /*
        public static void copyClass<T>(T copyFrom, T copyTo)
        {
            if (copyFrom == null || copyTo == null)
                throw new Exception("Must not specify null parameters");

            var properties = copyFrom.GetType().GetProperties();

            //foreach (var p in properties.Where(prop => prop.CanRead && prop.CanWrite))
            foreach (var p in properties)
            {
                object copyValue = p.GetValue(copyFrom);
                p.SetValue(copyTo, copyValue);
            }
        }
        */

        /////////////////////////
        ///

        public static bool myCopyFile(string lpExistingFileName, string lpNewFileName, bool bFailIfExists)
        {
            return false;
        }


        public static bool myDeleteFile(string lpFileName)
        {
            return false;
        }



        //////////////////////////////////////////
        /// <summary>
        public static void myGetLocalTime(out mySYSTEMTIME p)
        {
            DateTime dt = DateTime.Now;
            {
                p.wYear = (ushort)dt.Year;
                p.wMonth = (ushort)dt.Month;
                p.wDayOfWeek = (ushort)dt.DayOfWeek;
                p.wDay = (ushort)dt.Day;
                p.wHour = (ushort)dt.Hour;
                p.wMinute = (ushort)dt.Minute;
                p.wSecond = (ushort)dt.Second;
                p.wMilliseconds = (ushort)dt.Millisecond;
            }
            return;
        }



    }
}
