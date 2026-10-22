using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using Com.Qycx.Tmpjavalibrary;
using Java.Nio;
using Java.IO;


//
namespace qy.Droid
{
    public class YUV_RGB_info
    {
        public ByteBuffer mbbYuv;
        public ByteBuffer mbbRgb32;
    };


    //
	partial class qyFuncs_Droid
{
		public static unsafe int i420ToRgb(IntPtr yuvData, object yriParam, int rgbBits, bool bBgr, int width, int height, bool bFlipImg)
        {
            int iErr = -1;
            int tmpiRet = 0;
            string str;


            //
            if (yriParam == null)
            {
                qyFuncs.traceLog("i420ToRgb failed, yri is null");
                return -1;
            }
            YUV_RGB_info yri = (YUV_RGB_info)yriParam;

            //
            int rgb32Size = qyFuncs.M_sizeImage(32, width, height);
            int iVal;
            if (yri.mbbRgb32 != null)
            {
                iVal = yri.mbbRgb32.Capacity();
            }
            if (yri.mbbRgb32 == null || yri.mbbRgb32.Capacity() < rgb32Size)
            {                  
                yri.mbbRgb32 = ByteBuffer.AllocateDirect(rgb32Size);                
                if (yri.mbbRgb32 == null) goto errLabel;
                //
                str=string.Format("i420ToRgb: bbRgb32 alloc {0}",rgb32Size);
                qyFuncs.showInfo_open(0, null, str);
            }
            int yuvSize = qyFuncs.M_sizeImage(12, width, height);
            if (yri.mbbYuv == null || yri.mbbYuv.Capacity() < yuvSize)
            {
                yri.mbbYuv = ByteBuffer.AllocateDirect(yuvSize);
                if (yri.mbbYuv == null) goto errLabel;
                //
                str=string.Format("i420ToRgb: bbYuv alloc {0}",yuvSize);
                qyFuncs.showInfo_open(0, null, str);
            }

            byte * yuvBuf = (byte*)yri.mbbYuv.GetDirectBufferAddress();
            //IntPtr[] tmpsrc = new IntPtr[1];
            //tmpsrc[0] = (IntPtr)yuvData;
            //System.Runtime.InteropServices.Marshal.Copy(tmpsrc, 0,(IntPtr)yuvBuf, yuvSize);
            qyFuncs.mymemcpy((IntPtr)yuvBuf, yuvData, (uint)yuvSize);

            //
            ByteBuffer bbYuv=yri.mbbYuv;
            ByteBuffer bbRgb=yri.mbbRgb32;

            //
#if DEBUG
            //GuiShare.pf_dumpData1((IntPtr)yuvBuf, yuvSize, width, height, false, "before_i420torgb.yuv");
#endif 

            //
            try
            {
                //tmpiRet = TmpJniUtil.I420ToRgb(yuv, rgb, rgbBits, true, width, height, bFlipImg);
                tmpiRet = TmpJniUtil.I420ToRgb(bbYuv, bbRgb, rgbBits, bBgr, width, height, bFlipImg);

                //
#if DEBUG
                byte* rgbBuf = (byte*)bbRgb.GetDirectBufferAddress();
                //GuiShare.pf_dumpData1((IntPtr)rgbBuf, rgb32Size, width, height, false, "after_i420toRgb.raw");
#endif


                //Java.Lang.Object
                object kk;

            }
            catch(Exception e)
            {
                goto errLabel;
            }
#if false
            //
            Android.Graphics.Bitmap bitmap = Android.Graphics.Bitmap.CreateBitmap(width, height, Android.Graphics.Bitmap.Config.Argb8888);

            //Java.Nio.Buffer buffer=Java.Nio.Buffer.
            ByteBuffer b = ByteBuffer.Wrap(rgb);

            bitmap.CopyPixelsFromBuffer(b);

            //
            File path;
            path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            string str = path.AbsolutePath;
            string str1 = str + "/hrgb32" + width + "X" + height + "." + Java.Lang.JavaSystem.CurrentTimeMillis();

            string yuvPath=str1+ ".bmp";
            //
            FileUtil.SaveBitmap(bitmap, yuvPath);

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            Context context = (Context)pQyMc.gui.hMainWnd;
            //
            Android.Net.Uri uri = Android.Net.Uri.FromFile(new File(yuvPath));
            //
            context.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));




            //
            yuvPath = str1 + ".yuv";
            FileUtil.SaveBytes(yuv, yuvPath);
            //
            int ii = 0;
            
            //
            uri = Android.Net.Uri.FromFile(new File(yuvPath));
            //
            context.SendBroadcast(new Intent(Intent.ActionMediaScannerScanFile, uri));

            //
            bitmap.Recycle();
#endif

            iErr = 0;
            errLabel:


            // 
            return iErr !=0 ? iErr : tmpiRet;

        }


}
}