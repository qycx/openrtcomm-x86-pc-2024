using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
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

namespace qy.Droid
{
	partial class qyFuncs_Droid
{
        public static Bitmap texture2Bmp(int texture, int width, int height)
        {
            int[] frame = new int[1];
            GLES20.GlGenFramebuffers(1, frame, 0);
            checkGlError("glGenFramebuffers");
            GLES20.GlBindFramebuffer(GLES20.GlFramebuffer, frame[0]);
            checkGlError("glBindFramebuffer");
            GLES20.GlFramebufferTexture2D(GLES20.GlFramebuffer,
                    GLES20.GlColorAttachment0, GLES20.GlTexture2d, texture,
                    0);
            checkGlError("glFramebufferTexture2D");

            ByteBuffer buffer = ByteBuffer.Allocate(width * height * 4);
            GLES20.GlReadPixels(0, 0, width, height, GLES20.GlRgba,
                    GLES20.GlUnsignedByte, buffer);
            checkGlError("glReadPixels");
            Bitmap bitmap = Bitmap.CreateBitmap(width, height,
                    Bitmap.Config.Argb8888);
            bitmap.CopyPixelsFromBuffer(buffer);

            GLES20.GlBindFramebuffer(GLES20.GlFramebuffer, 0);
            checkGlError("glBindFramebuffer");
            GLES20.GlDeleteFramebuffers(1, frame, 0);
            checkGlError("glDeleteFramebuffer");
            return bitmap;
        }

        public static void checkGlError(string op)
        {
            int error;
            while ((error = GLES20.GlGetError()) != GLES20.GlNoError)
            {
                //throw new RuntimeException(op + ": glError " + error);
#if DEBUG
                qyFuncs.traceLog(op + ": glError " + error);
#endif
            }
        }
    }



   
}



