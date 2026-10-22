using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Nio;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using File = Java.IO.File;
using qyMessengerApp.Android;


//
namespace qy.Droid
{
	class testCls_Droid
{

        //
        static CMutexRW mtxRw = new CMutexRW();

        class Foo
        {
            public int A { get; set; }
            public string B;
        };

        //
        unsafe struct TMP_tBuf5
        {
            public fixed char tBuf[5];
        };

        //
        public static unsafe int testInMain(CCtxQyMc pQyMc)
        {
            if (pQyMc == null) return -1;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (pProcInfo == null) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            string str;

            str = "";
            int len = str.Length;

            len = sizeof(QY_SHOW_SMALL);

            //
            DateTime dt;
            //dt.AddSeconds

            //
            TMP_tBuf5 tt;
            str = "1234567890";
            qyFuncs.mytcsncpy(tt.tBuf, str, 5);
            string sss = new string(tt.tBuf);

            //
            //qyFuncs_Droid.copyToClipboard("我在测试剪贴板");
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            CameraManager cameraManager = (CameraManager)mainActivity.GetSystemService(Context.CameraService);
            String[] cameraIdList = cameraManager.GetCameraIdList();
            if (cameraIdList.Length > 0)
            {
                //cameraIdList.length 是否有摄像头和摄像头个数

            }


            //Foo foo = new Foo { A = 1, B = "abc" };
            Foo foo = new Foo { A = 1, };
foreach(var prop in foo.GetType().GetProperties()) {
    str=string.Format("{0}={1}", prop.Name, prop.GetValue(foo, null));
                qyFuncs.traceLog(str);
}

            List<object> persons = new List<object>();

            persons.Add("11");
            persons.Add("22");
            persons.Add(foo);

            int ii  =  persons.Count;

            persons.Remove("11");
            persons.Remove(foo);

            ii = persons.Count;



            //
            int i;
            /*
    for (i = 0; i < 1000; i++)
            {
                uint t0 = qyFuncs.myGetTickCount();
                qyFuncs.mySleep(1);
                uint t1 = qyFuncs.myGetTickCount();
                str = string.Format("{0}: {1}ms", i, t1 - t0);
                qyFuncs.traceLog(str);

            }
            */


    object ooo = (IntPtr)null;
            IntPtr p1 = (IntPtr)null;
            IntPtr p2 = (IntPtr)0;
           if ( p1==IntPtr.Zero)
            {
                ii = 0;                
            }
           if ( p2 == IntPtr.Zero)
                {
                ii = 9;

            }
            short sVal = -1;
            int iVal = (int)sVal;

            //
            //str= Com.Qycx.Tmpffmpeglibrary.TmpFfmpeg.AvCodecInfo();

            //
            //object obj = this;
            MainActivity wnd = (MainActivity)pQyMc.gui.hMainWnd;

            //
            testTime();
            



            //
            //testRwLock();


            //
            byte[] byteArrayIn1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ByteBuffer buffer = ByteBuffer.Wrap(byteArrayIn1);
            byte[] vv = new byte[10];

            buffer.Get(vv, 2, 3);

            //
            File path;

            ActivityManager activityManager = wnd.GetSystemService(Context.ActivityService) as ActivityManager;
            iVal = activityManager.LargeMemoryClass;
            int jVal = activityManager.MemoryClass;


            long lVal0 = Debug.NativeHeapAllocatedSize;

            //
            /*
            BitmapFactory.Options options = new BitmapFactory.Options();

            // options.inJustDecodeBounds设置为true,不会真正的将图片加载到内存中去
            options.InJustDecodeBounds = true;
            */

            Bitmap bitmap = BitmapFactory.DecodeResource(wnd.Resources, Resource.Drawable.bj);

            //


            //
            int w = bitmap.Width; //80;
            int h = bitmap.Height;  // 60;





            //int line = w * 3;

            //
            int yuvSize = w * h * 3 / 2;
            byte[] yuv = new byte[yuvSize];

            int rgb32Size = w * h * 4;
            byte[] rgb32_org = new byte[rgb32Size];
            byte[] rgb32 = new byte[rgb32Size];


            //
            int bmpBufSize = bitmap.RowBytes * bitmap.Height;

            //
            ByteBuffer bb_rgb32 = ByteBuffer.AllocateDirect(bmpBufSize);
            i = bitmap.ByteCount;
            bitmap.CopyPixelsToBuffer(bb_rgb32);
            bb_rgb32.Rewind();
            System.Runtime.InteropServices.Marshal.Copy(bb_rgb32.GetDirectBufferAddress(), rgb32_org, 0, rgb32Size);

            //
#if false
            GuiShare.pf_dumpBmp(bitmap, "bmp.org");
#endif
            //GuiShare.pf_dumpRgb32_1(bb_rgb32.GetDirectBufferAddress(), w, h, "rgb32");

            //long lVal = Java.Lang.JavaSystem.NanoTime;
            //qyFuncs.traceLog("before rgb32To24");
            long lVal1 = Java.Lang.JavaSystem.NanoTime();



            //long lNano2=Java.Lang.JavaSystem.NanoTime；
            long lVal2 = Java.Lang.JavaSystem.NanoTime();

            int iDiffInMs = (int)(lVal2 - lVal1) / 1000000;
            str = string.Format("nIntervalInMs of rgb32To24 {0}ms", iDiffInMs);
            qyFuncs.traceLog(str);


            ByteBuffer bb_yuv = ByteBuffer.AllocateDirect(yuvSize);
            ByteBuffer bb_yuv_rotate = ByteBuffer.AllocateDirect(yuvSize);


            //
#if true
            Com.Qycx.Tmpjavalibrary.TmpJniUtil.RgbToI420(bb_rgb32, 32, true, bb_yuv, w, h, false);
            //
            Com.Qycx.Tmpjavalibrary.TmpJniUtil.RotateI420(bb_yuv, bb_yuv_rotate, w, h, 90, true);
            //
            IntPtr pYuv = bb_yuv.GetDirectBufferAddress();
            IntPtr pYuv_rotate = bb_yuv_rotate.GetDirectBufferAddress();

            //
            //GuiShare.pf_dumpData1(pYuv, yuvSize, w, h, "kk.yuv");
            //GuiShare.pf_dumpData1(pYuv_rotate, yuvSize, h, w, "rotate_kk.yuv");


#endif

            //


            lVal1 = Debug.NativeHeapAllocatedSize;

            ByteBuffer kk = ByteBuffer.AllocateDirect(10000000);
            lVal2 = Debug.NativeHeapAllocatedSize;
            //
            long lDiff = lVal2 - lVal1;


            //
            long lVal = Debug.NativeHeapAllocatedSize;


            int iii = 0;


            //
            /*

            string yuvPath= str1 + ".yuv";


            //
            str = yuvPath;
            Com.Qycx.Tmpjavalibrary.FileUtil.SaveBytes(yuv, str);

            File file = new File(str);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            this.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));
            */




            path = Android.OS.Environment.GetExternalStoragePublicDirectory(
                            Android.OS.Environment.DirectoryPictures);
            //
            path = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDcim);
            path = new File(path.AbsolutePath + "/Camera");
            //
            path = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDownloads);

            //
            QY_MESSENGER_ID idInfo; idInfo.ui64Id = 10000;
            object hWnd = null;
            GuiShare.pf_findTalker(pQyMc, pMisCnt, idInfo, ref hWnd);

            /*
            //
            Int64 t = qyFuncs.mytime(null);
            string str = string.Format("DemoPic{0}.jpg", t);
            File file = new File(path, str);

            try
            {
                // Make sure the Pictures directory exists.
                path.Mkdirs();

                // Very simple code to copy a picture from the application's
                // resource into the external file.  Note that this code does
                // no error checking, and assumes the picture is small (does not
                // try to copy it in chunks).  Note that if external storage is
                // not currently mounted this will silently fail.
                var is1 = Resources.OpenRawResource(Resource.Drawable.kk);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                is1.CopyTo(ms);
                OutputStream os = new FileOutputStream(file);
                byte[] data = ms.ToArray();
                //int iRead = is1.Read(data);
                os.Write(data);
                    is1.Close();
                os.Flush();
                os.Close();

                //
                Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
                this.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));

            }
            catch (IOException e)
            {
                int kk = 0;
            }
            */


            return 0;
        }

        public static int testTime()
        {
            string str;
            
            //
            SimpleDateFormat alldate = new SimpleDateFormat("yyyyMMdd_HHmmss");
            Java.Util.Date date = new Java.Util.Date();
            str = alldate.Format(date);
            TimeZoneInfo tzi = TimeZoneInfo.Utc;
            DateTime dt = new DateTime(date.Time);

            long l = Java.Lang.JavaSystem.CurrentTimeMillis();
            DateTime dt0 = new DateTime(1970, 1, 1);
            DateTime dt1 = dt0.AddMilliseconds(l);

            dt = DateTime.Now;
            string str2 = dt.ToLocalTime().ToString("yyyyMMdd_HHmmss");
            string str3 = dt.ToUniversalTime().ToString("yyyyMMdd_HHmmss");



            //
            Java.Util.Date d1 = new Java.Util.Date(l);
            str = alldate.Format(d1);

            return 0;
        }


        //
        public static int testRwLock()
        {
            Thread []trs = new Thread[10];
            Thread []tws=new Thread[5];

            int i;
            for ( i=0;i< trs.Length; i++)
            {
                trs[i] = new Thread(new ParameterizedThreadStart(threadProc_testR));
                trs[i].Start(i);
            }
            for (i = 0; i < tws.Length; i++)
            {
                tws[i] = new Thread(new ParameterizedThreadStart(threadProc_testW));
                tws[i].Start(i);
            }

            for (; ; )
            {
                qyFuncs.mySleep(5000);
            }

            return 0;
            
        }


        //
        static void threadProc_testR(object l)
        {
            int id = (int)l;
            string str;
            int i;

            //
            //qyFuncs.mySleep(1000);

            //
            for (i=0; ;i++)          
            {

                using(CQyRWLock rLock=new CQyRWLock())
                {
                    if (rLock.rLock(testCls_Droid.mtxRw, 1000) != 0) continue;

                    str = string.Format("rlock {0} starts", id);
                    qyFuncs.traceLog(str);

                    qyFuncs.mySleep(3000);

                    str = string.Format("rlock {0} ends", id);
                    qyFuncs.traceLog(str);
                }

                //
                qyFuncs.mySleep(100);
                continue;
            }

        }

        //
        static void threadProc_testW(object l)
        {
            int id = (int)l;
            string str;
            int i;

            for (i = 0; ; i++)
            {

                using (CQyRWLock rwLock = new CQyRWLock())
                {
                    string w_hint = "threadProc_testW";

                    if (rwLock.wLock(mtxRw, 1000,w_hint) != 0) continue;

                    str = string.Format("wlock {0} starts", id);
                    qyFuncs.traceLog(str);

                    qyFuncs.mySleep(3000);

                    str = string.Format("wlock {0} ends", id);
                    qyFuncs.traceLog(str);
                }

                //
                qyFuncs.mySleep(100);
                continue;
            }

        }



    }



}