using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
using Android.Telecom;
using Com.Qycx.Gleslibrary;
using Java.Lang;
using qyMessengerApp.Android;
using Exception = Java.Lang.Exception;
using Thread = Java.Lang.Thread;


namespace MyGLE
{
    public class TextureLoader :Thread
    {
        private EGLContext textureContext;
        //private EGL14 egl;
        private EGLConfig eglConfig;
        private EGLDisplay display;
        private PerformanceTimer timer = new PerformanceTimer();
        private Context androidContext;
        //... Some of your code omitted for brevity 
        public TextureLoader(EglCore eglCore, Context androidContext)
        {
            //this.egl = egl;
            this.display = eglCore.MEGLDisplay;
            this.eglConfig = eglCore.MEGLConfig;
            this.androidContext = androidContext;

            //textureContext =EGL14.eglCreateContext(display, eglConfig, renderContext, null,0);
            textureContext = eglCore.MEGLContext;
            //
            int ii = 0;
        }
        public override void Run()
        {
            int[] pbufferAttribs = new int[] { EGL14.EglWidth, 1, EGL14.EglHeight, 1, EGL14.EglTextureTarget,
                EGL14.EglNoTexture, EGL14.EglTextureFormat, EGL14.EglNoTexture,
                EGL14.EglNone };

            EGLSurface localSurface = EGL14.EglCreatePbufferSurface(display, eglConfig, pbufferAttribs, 0);

            EGL14.EglMakeCurrent(display, localSurface, localSurface, textureContext);


            Thread.Sleep(2000);  //Replace sleep();

            timer.Start("Loading texture");

            //You'll need to find a way to load the texture in C#
            MyDraw.textureId = LoadTexture(Resource.Drawable.bj852x480);

            timer.Stop();
        }

        private void Sleep()
        {
            Thread.Sleep(2000);
        }

        private int LoadTexture(int resourceId)
        {
            int[] textureHandle = new int[1];

            GLES11.GlGenTextures(1, textureHandle,0);
            if (textureHandle[0] != 0)
            {
                //In C#, you'll need to find a suitable replacement for BitmapFactory and the decodeResource method. Below line is a placeholder.
                Bitmap bitmap = BitmapFactory.DecodeResource(androidContext.Resources, resourceId);

                GLES11.GlBindTexture(GLES11.GlTexture2d, textureHandle[0]);

                GLES11.GlTexParameterf(GLES11.GlTexture2d, GLES11.GlTextureMinFilter, GLES11.GlNearest);
                GLES11.GlTexParameterf(GLES11.GlTexture2d, GLES11.GlTextureMagFilter, GLES11.GlNearest);

                //For following line, also you need to find a way to do this in C#. 
                GLUtils.TexImage2D(GLES11.GlTexture2d, 0, bitmap, 0);

                bitmap.Recycle();
            }

            if (textureHandle[0] == 0)
            {
                throw new Exception("Error loading texture.");
            }

            return textureHandle[0];
        }
    }
}
