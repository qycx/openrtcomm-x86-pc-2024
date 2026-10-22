using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using File = Java.IO.File;
using Thread = Java.Lang.Thread;
namespace qy.Droid
{
    public class Thread_android : Thread
    {
        PF_threadProc m_pf;
        object m_param;

        public Thread_android(PF_threadProc pf, object param)
        {
            m_pf = pf;
            m_param = param;
        }


        public override void Run()
        {
            m_pf(m_param);
        }


    }

    partial class qyFuncs_Droid
    {

        public static long myGetTickCount_android()
        {
            long lVal = Java.Lang.JavaSystem.NanoTime() / 1000000;
            return lVal;
        }

        public static int myGetPid()
        {
            return Android.OS.Process.MyPid();
        }

        public static int myGetTid()
        {
            return Android.OS.Process.MyTid();
        }



        //
        public static int startThread1(ref object obj_ph, PF_threadProc pf, object param)
        {
            int iErr = -1;

            Java.Lang.Thread ph = (Java.Lang.Thread)obj_ph;

            if (ph != null) return -1;
            ph = new Thread_android(pf, param);
            if (ph == null) goto errLabel;
            ph.Start();

            obj_ph = ph;

            iErr = 0;
        errLabel:
            return iErr;

        }

        public static void waitForThread1(ref object obj_ph, uint dwMilliseconds)
        {
            uint dwRet = 0;

            //
            Java.Lang.Thread ph = (Java.Lang.Thread)obj_ph;
            if (null == ph) return;

#if false
			/*
			dwRet = WaitForSingleObject(*ph, dwMilliseconds);
			if (dwRet != WAIT_TIMEOUT && dwRet != WAIT_FAILED)
			{

				CloseHandle(*ph); *ph = NULL;

			}
			*/

			if (ph != null)
		{
			if (dwMilliseconds == 0)
			{
				if (!ph.IsAlive)
				{
					ph = null;
					return;
				}
				return;
			}

			//			
			uint msPerStep = 100;
			msPerStep = 10;
			uint maxStep = dwMilliseconds / msPerStep;
			if (maxStep == 0) maxStep = 1;
			//
			for (int i = 0; i < maxStep; i++)
			{
				if (!ph.IsAlive)
				{
					ph = null;
					return;
				}
				System.Threading.Thread.Sleep((int)msPerStep);
				continue;
			}

		}
#endif
            if (ph != null)
            {

                try
                {
                    ph.Join();

                }
                catch (UnsupportedOperationException ie)
                {

                }

                ph = null;
            }

            //			
            obj_ph = ph;

            return;

        }

        //

        // 
        public static int dumpData(byte[] data, int w, int h, bool bAppend, bool bFileName_haveTime, string pureFileName)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            File path;
            Context context = Application.Context;
            bool bDbg = true;

            path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string str1 = path.AbsolutePath;

            //
            str1 = str1 + "/test" + w + "X" + h + ".";// + Java.Lang.JavaSystem.CurrentTimeMillis();
            if (bFileName_haveTime)
            {
                DateTime dt = DateTime.Now;
                //
                str1 = str1 + dt.ToString("yyyy_MM_dd_HH_mm_ss_") + dt.Millisecond;
            }


            //
            string fileName = str1 + pureFileName;

            //
            Com.Qycx.Tmpjavalibrary.FileUtil.SaveBytes(data, bAppend, fileName, bDbg);
            //Com.Qycx.Tmpjavalibrary.TmpJniUtil.SaveBytes(data, bAppend, fileName);

            //
            //if (data == null)
            {
                File file;
                Android.Net.Uri uri;

                file = new File(fileName);
                uri = Android.Net.Uri.FromFile(file);
                context.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));
            }

            //
            return 0;

        }

        public static unsafe int dumpData1(IntPtr data, int len, int w, int h, bool bAppend, bool bFileName_haveTime, string pureFileName)
        {

            byte[] data_array = null;

            if (data != null && len != 0)
            {
                data_array = new byte[len];
                System.Runtime.InteropServices.Marshal.Copy((IntPtr)data, data_array, 0, len);
            }

            dumpData(data_array, w, h, bAppend, bFileName_haveTime, pureFileName);

            return 0;
        }

        public static unsafe int dumpRgb32(byte[] data, int w, int h, string pureFileName)
        {
            /*
			int rgb24Size = w * h * 3;
			//byte[] rgb24 = new byte[rgb24Size];
			ByteBuffer bb_rgb24 = ByteBuffer.AllocateDirect(rgb24Size);
			int rgb32Size = w * h * 4;
			ByteBuffer bb_rgb32 = ByteBuffer.AllocateDirect(rgb32Size);
			bb_rgb32.Put(data);

			//qyFuncs.g_colorSpace.rgb32To24(data, true, rgb24, false, w, h);
			Com.Qycx.Tmpjavalibrary.TmpJniUtil.Rgb32To24(bb_rgb32, true, bb_rgb24, false, w, h);
			*/

            byte[] rgb32 = data;

            Android.Graphics.Bitmap bitmap = Android.Graphics.Bitmap.CreateBitmap(w, h, Android.Graphics.Bitmap.Config.Argb8888);

            //Java.Nio.Buffer buffer=Java.Nio.Buffer.
            ByteBuffer b = ByteBuffer.Wrap(rgb32);

            bitmap.CopyPixelsFromBuffer(b);

            //
            DateTime dt = DateTime.Now;


            //
            File path;
            path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string str = path.AbsolutePath;

            //
            //string str1 = str + "/rgb32." + w + "X" + h + "." + Java.Lang.JavaSystem.CurrentTimeMillis();
            string str1 = str + "/rgb32." + w + "X" + h + "." + dt.ToString("yyyy_MM_dd_HH_mm_ss_") + dt.Millisecond;

            //
            string fileName = str1 + pureFileName + ".bmp";

            //
            Com.Qycx.Tmpjavalibrary.FileUtil.SaveBitmap(bitmap, fileName);

            File file;
            Android.Net.Uri uri;
            Context context = Application.Context;

            file = new File(fileName);
            uri = Android.Net.Uri.FromFile(file);
            context.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));



            return 0;
        }
        public static unsafe int dumpRgb32_1(IntPtr data, int w, int h, string pureFileName)
        {
            if (data == null) return -1;

            int rgb32Size = w * h * 4;
            byte[] rgb32 = new byte[rgb32Size];
            System.Runtime.InteropServices.Marshal.Copy((IntPtr)data, rgb32, 0, rgb32Size);
            dumpRgb32(rgb32, w, h, pureFileName);
            return 0;
        }

        public static unsafe int dumpBmp(object bmp, string pureFileName)
        {
            Android.Graphics.Bitmap bitmap = (Android.Graphics.Bitmap)bmp;

            int w = bitmap.Width;
            int h = bitmap.Height;

            File path;
            path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string str1 = path.AbsolutePath;

            //
            DateTime dt = DateTime.Now;

            //
            //str1 = str1 + "/tt." + w + "X" + h + "." + Java.Lang.JavaSystem.CurrentTimeMillis();
            str1 = str1 + "/test" + w + "X" + h + "." + dt.ToString("yyyy_MM_dd_HH_mm_ss_") + dt.Millisecond;


            //
            string fileName = str1 + pureFileName + ".bmp";
            File file;
            Android.Net.Uri uri;
            Context context = Application.Context;



            Com.Qycx.Tmpjavalibrary.FileUtil.SaveBitmap(bitmap, fileName);

            file = new File(fileName);
            uri = Android.Net.Uri.FromFile(file);
            context.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));

            return 0;

        }
        private static bool printRandomLog=false;
        //
        public static int logd(string tag, string str)
        {
            string str1;

            DateTime dt = DateTime.Now;
            //
            //str1 = dt.ToString("yyyy_MM_dd_HH_mm_ss_") + dt.Millisecond + " " + str;
            str1 = dt.ToString("HH:mm:ss_") + dt.Millisecond + " " + str;
       
            if (!tag.Contains("r"))
            {
                Android.Util.Log.Error(tag, str1);
            }
            else
            {
                if (printRandomLog)
                {
                    Android.Util.Log.Error(tag, str1);

                }
            }

            return 0;
        }
        public static string _T(string str)
        {
            if (str == null) str = "";
            return str;
        }

        //
        //
        public static float XMVectorGetX(System.Numerics.Vector4 V)
        {
            return V.X;
        }

        public static float XMVectorGetY(System.Numerics.Vector4 V)
        {
            return V.Y;
        }

        public static float XMVectorGetZ(System.Numerics.Vector4 V)
        {
            return V.Z;
        }




        //
        public static unsafe int trailDir(char* dir, uint size)
        {
            uint len;

            if (dir == null) return -1;
            //tTrim(dir);
            string str = new string(dir);
            str.Trim();
            qyFuncs.safeTcsnCpy(str, dir, size);

            len = (uint)qyFuncs.mylstrlen(dir);
            //
            if (len >= 1 && size >= len + 1)
            {
                if (dir[len - 1] == '/') return 0;
            }
            //
            if (len + 2 > size) return -1;
            dir[len] = '/';
            dir[len + 1] = (char)0;
            return 0;

        }

        //
        public static UInt64 getFileLen(string fullFilePath)
        {
            UInt64 len = 0;

            if (fullFilePath == null) return 0;

            try
            {
                File file = new File(fullFilePath);
                len = (UInt64)file.Length();
            }
            catch (Java.Lang.Exception e)
            {
                goto errLabel;
            }
        errLabel:
            return len;
        }

        public static int getDirAndFinalName(string svFileName, ref string dirName, ref string finalName)
        {
            //TCHAR* pFinalModuleName = NULL;

            if (null == svFileName || 0 == (svFileName.Length)) return -1;

#if false
			pFinalModuleName = (TCHAR*)_tcsrchr(svFileName, _T('\\'));
			if (pFinalModuleName)
			{
				if (finalName && finalNameSize) lstrcpyn(finalName, pFinalModuleName + 1, finalNameSize);
				if (dirName && size) lstrcpyn(dirName, svFileName, min(size, (unsigned  int)(pFinalModuleName - svFileName + 2))  );
				return 0;
			}
#endif
            int pos = svFileName.LastIndexOf("/");
            if (pos < 0) return -1;
            pos++;
            string str0 = svFileName.Substring(0, pos);
            string str1 = svFileName.Substring(pos);
            if (str0 == null || str1 == null) return -1;
            dirName = str0;
            finalName = str1;


            return 0;
        }


        //
#if DEBUG
        public static int printChoices(Size[] choices)
        {
            string str;
            int i;
            for (i = 0; i < choices.Length; i++)
            {
                Size option = choices[i];
                str = string.Format("choices[{0}]: {1}x{2}", i, option.Width, option.Height);
                qyFuncs.traceLog(str);
            }

            return 0;
        }
#endif


    }
}