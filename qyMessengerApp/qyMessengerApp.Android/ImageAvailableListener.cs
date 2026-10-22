using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using qy;
using qy.Droid;
using Java.IO;
using Environment = Android.OS.Environment;

//
using Com.Qycx.Tmpjavalibrary;
using Exception = Java.Lang.Exception;
using Android.Graphics;

//
namespace qyMessengerApp.Droid
{
    class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {

        //private byte[] mYuvBytes;
        //
        private ByteBuffer mbbYuv_org;
        //
        private byte[] mYPlaneBytes;
        private byte[] mUPlaneBytes;
        private byte[] mVPlaneBytes;
        int m_w_org;
        int m_h_org;
        int m_iTestNo;
        //
        private ByteBuffer mbbYuv;

        //

        public unsafe void OnImageAvailable(ImageReader reader)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //
            bool bDbg = false;
            string str;

            //
#if DEBUG
            bDbg = true;
#endif


            //
            m_iTestNo++;

            if (bDbg)
            {
                str = string.Format("OnImageAvailable {0}", m_iTestNo);
                qyFuncs.showInfo_open(0, null, str);
            }


            //
            Image image = null;
            try
            {
                image = reader.AcquireNextImage();

                //
                //str = string.Format("OnImageAvailable: after AcquireNextImage {0}", m_iTestNo);
                //qyFuncs.traceLog(str);
            }
            catch (IllegalStateException e)
            {
                int ii = 0;
                str = string.Format("OnImageAvailable: illegalStateException {0}", m_iTestNo);
                qyFuncs.traceLog(str);
                return;
            }
            catch (Exception e)
            {
                int jjj = 0;
                str = string.Format("OnImageAvailable: Exception {0}", m_iTestNo);
                qyFuncs.traceLog(str);
                return;

            }
            if (image == null)
            {
                str = string.Format("OnImageAvailable: image null {0}", m_iTestNo);
                qyFuncs.traceLog(str);
                return;
            }

            //
            /*
            ImageSaver saver= new ImageSaver(this, image);
            saver.m_iTestNo = this.m_iTestNo;
            mOwner.mBackgroundHandler.Post(saver);
            return;
            */

            //

            //image = reader.AcquireLatestImage();
            //if (image == null) return;


            try
            {


                //

                //

#if true
                if (image.Format == ImageFormatType.Yuv420888)
                {
                    int width_org = image.Width;
                    int height_org = image.Height;
                    //
                    int yuvSize_org = width_org * height_org * 3 / 2;
                    //
                    if (mbbYuv_org == null)
                    {
                        m_w_org = width_org;
                        m_h_org = height_org;
                        //
                        // YUV420 大小总是 width * height * 3 / 2
                        //mOwner.mYuvBytes = new byte[width * height * 3 / 2];
                        mbbYuv_org = ByteBuffer.AllocateDirect(yuvSize_org);
                        //
                        //mYPlaneBytes = new byte[width * height];
                        //mUPlaneBytes = new byte[width * height / 2];
                        //mVPlaneBytes = new byte[width * height / 2];
                    }
                    else if (m_w_org != width_org || m_h_org != height_org)
                    {
                        qyFuncs.traceLog("imageAvailableListener, OnImageAvailable failed, m_w!=w or m_h!-h");
                        goto errLabel;
                    }
                    //byte[] mYuvBytes = mYuvBytes;
                    byte* mYuvBytes_org = (byte*)mbbYuv_org.GetDirectBufferAddress();
                    if (mYuvBytes_org == null) goto errLabel;

                    //
                    //str = string.Format("OnImageAvailable: 11 {0}", m_iTestNo);
                    //qyFuncs.traceLog(str);

                    /*

                    // YUV_420_888
                    Image.Plane[] planes = image.GetPlanes();

                    // Y通道，对应planes[0]
                    // Y size = width * height
                    // yBuffer.remaining() = width * height;
                    // pixelStride = 1
                    ByteBuffer yBuffer = planes[0].Buffer;
                    int yLen = width * height;
                    yBuffer.Get(mYuvBytes, 0, yLen);

                    //
                    //str = string.Format("OnImageAvailable: 12 {0}", m_iTestNo);
                    //qyFuncs.traceLog(str);

                    // U通道，对应planes[1]
                    // U size = width * height / 4;
                    // uBuffer.remaining() = width * height / 2;
                    // pixelStride = 2
                    ByteBuffer uBuffer = planes[1].Buffer;
                    int pixelStride = planes[1].PixelStride; // pixelStride = 2
                    int uBuffer_Remaining = uBuffer.Remaining();
                    if ( uBuffer_Remaining>mUPlaneBytes.Length)
                    {
                        goto errLabel;
                    }
                    uBuffer.Get(mUPlaneBytes, 0, uBuffer_Remaining);
                    for (int i = 0; i < uBuffer_Remaining; i += pixelStride)
                    {
                        //mYuvBytes[yLen++] = (byte)uBuffer.Get(i);
                        mYuvBytes[yLen++] = (byte)mUPlaneBytes[i];
                    }

                    //
                    //str = string.Format("OnImageAvailable: 13 {0}", m_iTestNo);
                    //qyFuncs.traceLog(str);

                    // V通道，对应planes[2]
                    // V size = width * height / 4;
                    // vBuffer.remaining() = width * height / 2;
                    // pixelStride = 2
                    ByteBuffer vBuffer = planes[2].Buffer;
                    pixelStride = planes[2].PixelStride; // pixelStride = 2
                    int vBuffer_Remaining = vBuffer.Remaining();
                    if ( vBuffer_Remaining>mVPlaneBytes.Length)
                    {
                        goto errLabel;
                    }
                    vBuffer.Get(mVPlaneBytes, 0, vBuffer_Remaining);
                    for (int i = 0; i < vBuffer_Remaining; i += pixelStride)
                    {
                        //mYuvBytes[yLen++] = (byte)vBuffer.Get(i);
                        mYuvBytes[yLen++] = (byte)mVPlaneBytes[i];
                    }

                    //
                    //str = string.Format("OnImageAvailable: 14 {0}", m_iTestNo);
                    //qyFuncs.traceLog(str);

                    */

                    //byte[] rowBytes = new byte[width];
                    getDataFromImage(image, Consts.CONST_fourcc_I420, m_iTestNo, ref mYPlaneBytes, ref mUPlaneBytes, ref mVPlaneBytes, mYuvBytes_org, yuvSize_org);

                    //GuiShare.pf_dumpData1(mbbYuv_org.GetDirectBufferAddress(), yuvSize, width, height, "org.yuv");


                    int width = height_org;
                    int height = width_org;
                    int yuvSize = width * height * 3 / 2;
                    //
                    if (mbbYuv == null)
                    {
                        mbbYuv = ByteBuffer.AllocateDirect(yuvSize);
                    }
                    //
                    //  TmpJniUtil.RotateI420(mbbYuv_org, mbbYuv, width_org, height_org, mOwner.mSensorOrientation, false);
                    byte* mYuvBytes = (byte*)mbbYuv.GetDirectBufferAddress();

                    //
                    //GuiShare.pf_dumpData1(mbbYuv.GetDirectBufferAddress(), yuvSize, height, width, "good.yuv");

                    //
                    // QY_SHARED_OBJ sharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, mOwner.mOwner_wnd.mOwner_hTalk.m_var.av.iIndex_sharedObj_localAv);
                    //  if (sharedObj == null) goto errLabel;
                    //  int iIndex_capBmp = sharedObj.var.iIndex_capBmp;

                    //
                    int iSampleTimeInMs = (int)qyFuncs.myGetTickCount(null);

                    //qyFuncs_Droid.BufferCB_av(null, 0, null, 0, mYuvBytes, yuvSize, width, height, m_iTestNo);
                    //    qyFuncs_Droid.BufferCB_av(pProcInfo, iIndex_capBmp, null, iSampleTimeInMs, (byte*)mYuvBytes, yuvSize, width, height, m_iTestNo);


                    //
#if false
                    if (mOwner.mIsShutter)
                    {
                        mOwner.mIsShutter = false;
                        //
                        GuiShare.pf_dumpData1((IntPtr)mYuvBytes_org,yuvSize,width_org,height_org,false, false,"haha_org.yuv");
                        GuiShare.pf_dumpData1((IntPtr)mYuvBytes, yuvSize, width, height, false, false, "haha.yuv");
                            
                        //                        
                        int rgb32Size_org = qyFuncs.M_sizeImage(32, width_org, height_org);
                        ByteBuffer tmpbbRgb32_org = ByteBuffer.AllocateDirect(rgb32Size_org);
                        TmpJniUtil.I420ToRgb(mbbYuv_org, tmpbbRgb32_org, 32, true, width_org, height_org, false);
                        //
                        GuiShare.pf_dumpRgb32_1(tmpbbRgb32_org.GetDirectBufferAddress(), width_org, height_org, "haha_org");

                        //
                        int rgb32Size = qyFuncs.M_sizeImage(32, width, height);
                        ByteBuffer tmpbbReg32 = ByteBuffer.AllocateDirect(rgb32Size);
                        TmpJniUtil.I420ToRgb(mbbYuv, tmpbbReg32, 32, true, width, height, false);
                        //
                        GuiShare.pf_dumpRgb32_1(tmpbbReg32.GetDirectBufferAddress(), width, height, "haha");

                        //
                        int jjj = 0;
                    }
#endif


                }
#endif
            }
            catch (Java.Lang.Exception e)
            {
                goto errLabel;
            }


        errLabel:


            //
            image.Close();
            return;
        }
        //





        //
        private static int COLOR_FormatI420 = Consts.CONST_fourcc_I420;
        private static int COLOR_FormatNV21 = Consts.CONST_fourcc_NV21;

        private static bool isImageFormatSupported(Image image)
        {
            ImageFormatType format = image.Format;
            switch (format)
            {
                case ImageFormatType.Yuv420888:
                case ImageFormatType.Nv21:
                case ImageFormatType.Yv12:
                    return true;
            }
            return false;
        }



        private static int getDataFromImage(Image image, int colorFormat, int iTestNo, byte[] plane0Bytes, byte[] plane1Bytes, byte[] plane2Bytes, byte[] data)
        {
            int iErr = -1;
            bool bDbg = false;
            //bDbg = true;

            //
            /*
            byte[] rowData1 = new byte[rowData.Length];
            byte[] data1 = new byte[data.Length];
            System.Array.Copy(rowData1, rowData, rowData1.Length);
            System.Array.Copy(data1, data, data1.Length);
            if (bEqual_byteArray(rowData1,rowData))
            {
                int ii = 0;
                ii++;
            }
            */

            //
            string str;
            //
            if (bDbg)
            {
                str = string.Format("getDataFromImage t1 {0}", iTestNo);
                qyFuncs.traceLog(str);
            }

            //
            if (colorFormat != COLOR_FormatI420 && colorFormat != COLOR_FormatNV21)
            {
                //throw new IllegalArgumentException("only support COLOR_FormatI420 " + "and COLOR_FormatNV21");
                return -1;
            }
            if (!isImageFormatSupported(image))
            {
                //throw new RuntimeException("can't convert Image to byte array, format " + image.getFormat());
                return -1;
            }
            Rect crop = image.CropRect;
            ImageFormatType format = image.Format;
            int width = crop.Width();
            int height = crop.Height();
            Image.Plane[] planes = image.GetPlanes();
            //byte[] data = new byte[width * height * ImageFormat.GetBitsPerPixel(format) / 8];
            int dataSize = width * height * ImageFormat.GetBitsPerPixel(format) / 8;
            if (data.Length < dataSize) goto errLabel;
            //byte[] rowData = new byte[planes[0].RowStride];
            /*
            int rowDataSize = planes[0].RowStride;
            if (rowData.Length < rowDataSize) goto errLabel;
            */
            //
            //if (VERBOSE) Log.v(TAG, "get data from " + planes.length + " planes");
            int channelOffset = 0;
            int outputStride = 1;
            for (int i = 0; i < planes.Length; i++)
            {
                ByteBuffer buffer = planes[i].Buffer;
                int buffer_Remaining = buffer.Remaining();
                //
                byte[] planeBytes = null;
                //
                switch (i)
                {
                    case 0:
                        channelOffset = 0;
                        outputStride = 1;
                        //
                        if (buffer_Remaining > plane0Bytes.Length)
                        {
                            goto errLabel;
                        }
                        planeBytes = plane0Bytes;
                        break;
                    case 1:
                        if (colorFormat == COLOR_FormatI420)
                        {
                            channelOffset = width * height;
                            outputStride = 1;
                        }
                        else if (colorFormat == COLOR_FormatNV21)
                        {
                            channelOffset = width * height + 1;
                            outputStride = 2;
                        }
                        //
                        if (buffer_Remaining > plane1Bytes.Length)
                        {
                            goto errLabel;
                        }
                        planeBytes = plane1Bytes;
                        break;
                    case 2:
                        if (colorFormat == COLOR_FormatI420)
                        {
                            channelOffset = (int)(width * height * 1.25);
                            outputStride = 1;
                        }
                        else if (colorFormat == COLOR_FormatNV21)
                        {
                            channelOffset = width * height;
                            outputStride = 2;
                        }
                        //
                        if (buffer_Remaining > plane2Bytes.Length)
                        {
                            goto errLabel;
                        }
                        planeBytes = plane2Bytes;
                        break;
                    default:
                        goto errLabel;
                        break;
                }
                buffer.Get(planeBytes, 0, buffer_Remaining);
                int buffer_Position = 0;
                //
                int rowStride = planes[i].RowStride;
                int pixelStride = planes[i].PixelStride;
                if (bDbg)
                {
                    /*
                    Log.v(TAG, "pixelStride " + pixelStride);
                    Log.v(TAG, "rowStride " + rowStride);
                    Log.v(TAG, "width " + width);
                    Log.v(TAG, "height " + height);
                    Log.v(TAG, "buffer size " + buffer.remaining());
                    */
                }
                int shift = (i == 0) ? 0 : 1;
                int w = width >> shift;
                int h = height >> shift;
                //
                //buffer.Position(rowStride * (crop.Top >> shift) + pixelStride * (crop.Left >> shift));
                buffer_Position = (rowStride * (crop.Top >> shift) + pixelStride * (crop.Left >> shift));
                //
                for (int row = 0; row < h; row++)
                {
                    int length;
                    if (pixelStride == 1 && outputStride == 1)
                    {
                        length = w;
                        //
                        //buffer.Get(data, channelOffset, length);
                        System.Array.Copy(planeBytes, buffer_Position, data, channelOffset, length);
                        buffer_Position += length;
                        /*
                        if (!bEqual_byteArray(data, data1))
                        {
                            int ii = 0;
                        }
                        */
                        //
                        channelOffset += length;
                    }
                    else
                    {
                        length = (w - 1) * pixelStride + 1;
                        //
                        //buffer.Get(rowData, 0, length);
                        //System.Array.Copy(planeBytes, buffer_Position, rowData, 0, length);
                        //
                        for (int col = 0; col < w; col++)
                        {
                            //data[channelOffset] = rowData[col * pixelStride];
                            data[channelOffset] = planeBytes[buffer_Position + col * pixelStride];
                            //
                            //data1[channelOffset] = rowData1[col * pixelStride];
                            //
                            channelOffset += outputStride;
                        }
                        //
                        buffer_Position += length;
                        //
                        /*
                        if (!bEqual_byteArray(data, data1))
                        {
                            int ii = 0;
                        }
                        */
                    }
                    if (row < h - 1)
                    {
                        //buffer.Position(buffer.Position() + rowStride - length);
                        buffer_Position = (buffer_Position + rowStride - length);
                    }
                }

                if (bDbg)
                {
                    //Log.v(TAG, "Finished reading data from plane " + i);
                    str = string.Format("getDataFromImage t2 finish palne{0}, {1}", i, iTestNo);
                    qyFuncs.traceLog(str);

                }
            }

            //
            /*
            if (!bEqual_byteArray(data, data1))
            {
                int ii = 0;
            }
            */

            iErr = 0;
        errLabel:

            return iErr;
        }

        //
        private static unsafe int getDataFromImage(Image image, int colorFormat, int iTestNo, ref byte[] plane0Bytes, ref byte[] plane1Bytes, ref byte[] plane2Bytes, byte* data, int dataBufSize)
        {
            int iErr = -1;
            bool bDbg = false;
            //bDbg = true;

            //
            /*
            byte[] rowData1 = new byte[rowData.Length];
            byte[] data1 = new byte[data.Length];
            System.Array.Copy(rowData1, rowData, rowData1.Length);
            System.Array.Copy(data1, data, data1.Length);
            if (bEqual_byteArray(rowData1,rowData))
            {
                int ii = 0;
                ii++;
            }
            */

            //
            string str;
            //
            if (bDbg)
            {
                str = string.Format("getDataFromImage t1 {0}", iTestNo);
                qyFuncs.traceLog(str);
            }

            //
            if (colorFormat != COLOR_FormatI420 && colorFormat != COLOR_FormatNV21)
            {
                //throw new IllegalArgumentException("only support COLOR_FormatI420 " + "and COLOR_FormatNV21");
                return -1;
            }
            if (!isImageFormatSupported(image))
            {
                //throw new RuntimeException("can't convert Image to byte array, format " + image.getFormat());
                return -1;
            }
            Rect crop = image.CropRect;
            ImageFormatType format = image.Format;
            int width = crop.Width();
            int height = crop.Height();
            Image.Plane[] planes = image.GetPlanes();
            //byte[] data = new byte[width * height * ImageFormat.GetBitsPerPixel(format) / 8];
            int dataSize = width * height * ImageFormat.GetBitsPerPixel(format) / 8;
            if (dataBufSize < dataSize) goto errLabel;
            //byte[] rowData = new byte[planes[0].RowStride];
            /*
            int rowDataSize = planes[0].RowStride;
            if (rowData.Length < rowDataSize) goto errLabel;
            */
            //
            //if (VERBOSE) Log.v(TAG, "get data from " + planes.length + " planes");
            int channelOffset = 0;
            int outputStride = 1;
            for (int i = 0; i < planes.Length; i++)
            {
                ByteBuffer buffer = planes[i].Buffer;
                int buffer_Remaining = buffer.Remaining();
                //
                byte[] planeBytes = null;
                //
                switch (i)
                {
                    case 0:
                        channelOffset = 0;
                        outputStride = 1;
                        //
                        if (plane0Bytes == null || buffer_Remaining > plane0Bytes.Length)
                        {
                            plane0Bytes = new byte[buffer_Remaining];
                        }
                        planeBytes = plane0Bytes;
                        break;
                    case 1:
                        if (colorFormat == COLOR_FormatI420)
                        {
                            channelOffset = width * height;
                            outputStride = 1;
                        }
                        else if (colorFormat == COLOR_FormatNV21)
                        {
                            channelOffset = width * height + 1;
                            outputStride = 2;
                        }
                        //
                        if (plane1Bytes == null || buffer_Remaining > plane1Bytes.Length)
                        {
                            plane1Bytes = new byte[buffer_Remaining];
                        }
                        planeBytes = plane1Bytes;
                        break;
                    case 2:
                        if (colorFormat == COLOR_FormatI420)
                        {
                            channelOffset = (int)(width * height * 1.25);
                            outputStride = 1;
                        }
                        else if (colorFormat == COLOR_FormatNV21)
                        {
                            channelOffset = width * height;
                            outputStride = 2;
                        }
                        //
                        if (plane2Bytes == null || buffer_Remaining > plane2Bytes.Length)
                        {
                            plane2Bytes = new byte[buffer_Remaining];
                        }
                        planeBytes = plane2Bytes;
                        break;
                    default:
                        goto errLabel;
                        break;
                }
                buffer.Get(planeBytes, 0, buffer_Remaining);
                int buffer_Position = 0;
                //
                int rowStride = planes[i].RowStride;
                int pixelStride = planes[i].PixelStride;
                if (bDbg)
                {
                    /*
                    Log.v(TAG, "pixelStride " + pixelStride);
                    Log.v(TAG, "rowStride " + rowStride);
                    Log.v(TAG, "width " + width);
                    Log.v(TAG, "height " + height);
                    Log.v(TAG, "buffer size " + buffer.remaining());
                    */
                }
                int shift = (i == 0) ? 0 : 1;
                int w = width >> shift;
                int h = height >> shift;
                //
                //buffer.Position(rowStride * (crop.Top >> shift) + pixelStride * (crop.Left >> shift));
                buffer_Position = (rowStride * (crop.Top >> shift) + pixelStride * (crop.Left >> shift));
                //
                for (int row = 0; row < h; row++)
                {
                    int length;
                    if (pixelStride == 1 && outputStride == 1)
                    {
                        length = w;
                        //
                        //buffer.Get(data, channelOffset, length);
                        //System.Array.Copy(planeBytes, buffer_Position, data, channelOffset, length);
                        System.Runtime.InteropServices.Marshal.Copy(planeBytes, buffer_Position, (IntPtr)(data + channelOffset), length);
                        buffer_Position += length;
                        /*
                        if (!bEqual_byteArray(data, data1))
                        {
                            int ii = 0;
                        }
                        */
                        //
                        channelOffset += length;
                    }
                    else
                    {
                        length = (w - 1) * pixelStride + 1;
                        //
                        //buffer.Get(rowData, 0, length);
                        //System.Array.Copy(planeBytes, buffer_Position, rowData, 0, length);
                        //
                        for (int col = 0; col < w; col++)
                        {
                            //data[channelOffset] = rowData[col * pixelStride];
                            data[channelOffset] = planeBytes[buffer_Position + col * pixelStride];
                            //
                            //data1[channelOffset] = rowData1[col * pixelStride];
                            //
                            channelOffset += outputStride;
                        }
                        //
                        buffer_Position += length;
                        //
                        /*
                        if (!bEqual_byteArray(data, data1))
                        {
                            int ii = 0;
                        }
                        */
                    }
                    if (row < h - 1)
                    {
                        //buffer.Position(buffer.Position() + rowStride - length);
                        buffer_Position = (buffer_Position + rowStride - length);
                    }
                }

                if (bDbg)
                {
                    //Log.v(TAG, "Finished reading data from plane " + i);
                    str = string.Format("getDataFromImage t2 finish palne{0}, {1}", i, iTestNo);
                    qyFuncs.traceLog(str);

                }
            }

            //
            /*
            if (!bEqual_byteArray(data, data1))
            {
                int ii = 0;
            }
            */

            iErr = 0;
        errLabel:

            return iErr;
        }


        //

    }
}


