using Android.App;
using Android.Content;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thread = Java.Lang.Thread;
using qyMessengerApp.Android;
using qy;

//
using qyMessengerApp.Droid;
using Com.Qycx.Gleslibrary;
using Java.Lang;


//
namespace qy.Droid
{

    public class CQmcDrawTmpl
    {
        //
        public Fragment mOwner;
        public int miFgType;
        //
        public WindowSurface mWindowSurface;
        //
        public EglCore mEglCore_shared;
        //
        public bool bQuit;


        //
        static string VERTEX_SHADER =
                            "uniform mat4 mMatrix;" +
                    "uniform mat4 vMatrix;" +
                    "uniform mat4 pMatrix;" +
            "attribute vec4 vPosition;" +
                    "attribute vec2 a_texCoord;" +
                    "varying vec2 v_texCoord;" +
             "void main(){" +
             " gl_Position=pMatrix * vMatrix * mMatrix * vPosition;" +
                    " v_texCoord=a_texCoord;" +
             "}";

        static string FRAGMENT_SHADER =
            "precision mediump float;" +
                    "varying vec2 v_texCoord;" +
                    "uniform sampler2D s_texture;" +
            " void main(){" +
            "  gl_FragColor = texture2D(s_texture,v_texCoord);" +
            "}";


        static float[] VERTEX = {
                //1,1,0,
                //-4.966f,15.19f,-99.898f,
                -3.27258f,15.18121f,-100.2942f,
                //-1, 1, 0,
                //-50,15,-86,
                -52.47561f,15.18121f,-85.53316f,
                //-1, -1, 0,
                //-44,-64,-64,
                -45.90787f,-64.36166f,-63.64063f,
                //1, -1, 0,
                //1.6f,-64,-78,
                3.29516f,-64.36166f,-78.40168f,


        };
        
        //
        public short[] VERTEX_INDEX = { 0, 1, 2, 0, 2, 3 };
        
        //
        public float[] TEX_VERTEX ={
                1,0,
                0,0,
                0,1,
                1,1,
        };

        //
        public FloatBuffer mVertexBuffer;
        //
        public ShortBuffer mVertexIndexBuffer;

        FloatBuffer mTexVertexBuffer;

        //
        public QIS_mats mMats;

        //
        public float[] mMMatrix = new float[16];
        public float[] mVMatrix = new float[16];
        public float[] mPMatrix = new float[16];

        int mProgram;
        public int mPositionHandle;

        public int mMatrixHandle;
        public int vMatrixHandle;
        public int pMatrixHandle;

        public int mTexCoordHandle;
        public int mTexSmaplerHandle;
        //
        public int mTexName_default;
        public int mTexName_default_iW;
        public int mTexName_default_iH;

        //
        public int m_lastFps_tickCnt;
        public int m_lastFps_loop_ctrl;
        public int m_loop_ctrl;


        //
  

        //
        public class TextureLoader : Thread
        {
            EGLContext mTextureContext_shared;
            EGLConfig mEglConfig;
            EGLDisplay mDisplay;
            //
            Context mAndroidContext;
            //
            public CQmcDrawTmpl mMyDraw;
            //
            public int m_texId_0;
            public int m_texId_1;

            //
            bool m_bInited;

            //
            public int iTestNo;
            public string m_tag;

            //
            //
            public YUV_RGB_info m_yuvRgbInfo;

            //
            EglCore mEglCore;

            //
            public Android.Graphics.Bitmap m_bitmap;
                        //
            public TextureLoader(EglCore eglCore_shared, Context androidContext, CQmcDrawTmpl myDraw)
            {
                mDisplay = eglCore_shared.MEGLDisplay;
                this.mEglConfig = eglCore_shared.MEGLConfig;
                //
                mTextureContext_shared = eglCore_shared.MEGLContext;
                //
                this.mAndroidContext = androidContext;
                //
                mMyDraw = myDraw;
                //
                this.m_yuvRgbInfo = new YUV_RGB_info();
                //
                int ii = 0;
                //
            }

            //
            public int init()
            {
                int iErr = -1;

                //
                mEglCore = new EglCore(mTextureContext_shared, EglCore.FlagTryGles3);

                //
                int[] textureHandle = new int[2];
                GLES20.GlGenTextures(2, textureHandle, 0);
                //
                qyFuncs.showInfo_open(0, "", "TextureLoader.init, glGenTextures 2");

                int tmpiRet = GLES20.GlGetError();
                if (textureHandle[0] == 0 || textureHandle[1] == 0)
                {
                    qyFuncs.traceLog("TextureLoader.Run failed, GlGenTextures failed");
                    goto errLabel;
                }
                m_texId_0 = textureHandle[0];
                m_texId_1 = textureHandle[1];

                this.m_bInited = true;

                iErr = 0;
                errLabel:
                return iErr;
            }

            public void exit()
            {
                if (m_texId_0 != 0 || m_texId_1 != 0)
                {
                    int[] textureHandle = new int[2];
                    textureHandle[0] = m_texId_0;
                    textureHandle[1] = m_texId_1;
                    GLES20.GlDeleteTextures(2, textureHandle, 0);
                }

                //
                if(mEglCore!=null)
                {
                    mEglCore.Release();
                }

                //
                m_bInited = false;

                return;
            }

            public override  void Run()
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                TextureLoader tl = this;
                int iTestNo = tl.iTestNo;
                string str;

                //
                if ( !m_bInited)
                {
                    return;
                }

                //using (CQySyncObj syncObj = new CQySyncObj())
                {
                    //if (syncObj.sync("", pProcInfo.hMutex_syncQdcUnit) != 0) goto errLabel;

                    //
                    int[] pbufferAttribs = { EGL14.EglWidth,1,EGL14.EglHeight,1,EGL14.EglTextureTarget,    
                        EGL14.EglNoTexture,EGL14.EglTextureFormat,EGL14.EglNoTexture,
                        EGL14.EglNone};
                    EGLSurface localSurface = EGL14.EglCreatePbufferSurface(mDisplay, mEglConfig, pbufferAttribs, 0);
                    if ( localSurface==null||localSurface==EGL14.EglNoSurface)
                    {
                        goto errLabel;
                    }
                    //bool tmpbRet = EGL14.EglMakeCurrent(mDisplay, localSurface, localSurface, mTextureContext);
                    bool tmpbRet = EGL14.EglMakeCurrent(mDisplay, localSurface, localSurface, this.mEglCore.MEGLContext);
                    if (!tmpbRet)
                    {
                        str=Android.Opengl.GLUtils.GetEGLErrorString(EGL14.EglGetError());
                        goto errLabel;
                    }
                    if (tmpbRet)
                    {
                        int ii = iTestNo;
                    }

                    /*
                    int[] textureHandle = new int[2];
                    GLES20.GlGenTextures(2, textureHandle, 0);
                    int tmpiRet = GLES20.GlGetError();
                    if (textureHandle[0] == 0 || textureHandle[1] == 0)
                    {
                        qyFuncs.traceLog("TextureLoader.Run failed, GlGenTextures failed");
                        goto errLabel;
                    }
                    m_texId_0 = textureHandle[0];
                    m_texId_1 = textureHandle[1];
                    */

                }

                //
                this.fillTex_run();


                //
                /*
                if (mMyDraw.m_textureId_0 == 0)
                {
                    int texId = loadTexture(Resource.Drawable.waterfalls, m_texId_0);
                    if (texId != 0)
                    {
                        mMyDraw.m_textureId_0 = texId;
                    }
                    //
                }
                */

            //
            errLabel:

                if(m_bitmap!=null)
                {
                    m_bitmap.Recycle();
                    m_bitmap = null;
                }

                return;

            }

            public void halt()
            {
                bool bDbg = false;
                string str;
#if DEBUG
                bDbg=true;
#endif

                if (bDbg) {
                    str = string.Format("texLoader.halt, {0} before waitForThread", m_tag);
                    qyFuncs.traceLog(str);                        
                }
                //
                //Java.Lang.Thread h = this;
                object h = this;
                qyFuncs_Droid.waitForThread1(ref h, 10000);
                //
                if (bDbg)
                {
                    str = string.Format("texLoader.halt, {0} after waitForThread", m_tag);
                    qyFuncs.traceLog(str);
                }

                //
                exit();

            }

            int loadTexture(int resourceId, int texId)
            {
                int iErr = -1;


                Android.Graphics.BitmapFactory.Options options = new Android.Graphics.BitmapFactory.Options();
                options.InScaled = false;

                Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeResource(mAndroidContext.Resources,
                    resourceId, options);
                GLES20.GlBindTexture(GLES20.GlTexture2d, texId);

                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter,
                    GLES20.GlNearest);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter,
                    GLES20.GlNearest);

                //
                GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);
                bitmap.Recycle();


                iErr = 0;
            errLabel:
                return iErr == 0 ? texId : 0;
            }


            public int loadTexture(byte[] rgb1, int w, int h, int texId)
            {
                return -1;
            }


            public int loadTexture(int w, int h, int texId)
            {
                int iErr = -1;
                //Android.Graphics.Bitmap bitmap = null;


                if (w == 0 || h == 0) return -1;


                //Android.Graphics.BitmapFactory.Options options = new Android.Graphics.BitmapFactory.Options();
                //options.InScaled = false;

                try
                {
                    if (null!=m_bitmap)
                    {
                        if(m_bitmap.Width!=w || m_bitmap.Height!=h)
                        {
                            m_bitmap.Recycle();
                            m_bitmap = null;
                        }
                    }
                    if (null == m_bitmap)
                    {
                        m_bitmap = Android.Graphics.Bitmap.CreateBitmap(w, h, Android.Graphics.Bitmap.Config.Argb8888);
                        //
                        qyFuncs.traceLog("loadTexture: CreateBitmap");
                    }
                }
                catch (Java.Lang.Exception e)
                {
                    goto errLabel;
                }
                catch (System.Exception e)
                {
                    goto errLabel;
                }
                //
                int ii = 0;
                //
                ByteBuffer b = this.m_yuvRgbInfo.mbbRgb32; // ByteBuffer.Wrap(rgb);
                b.Rewind();

                m_bitmap.CopyPixelsFromBuffer(b);


                //
#if DEBUG
                int rgb32Size = qyFuncs.M_sizeImage(32, w, h);
                //GuiShare.pf_dumpData1(b.GetDirectBufferAddress(), rgb32Size, w, h, false, true, "loadTexture_bbRgb32.raw");
                ///GuiShare.pf_dumpBmp(bitmap, "loadTexture");
#endif


                //
                GLES20.GlBindTexture(GLES20.GlTexture2d, texId);

                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter,
                    GLES20.GlNearest);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter,
                    GLES20.GlNearest);

                //
                GLUtils.TexImage2D(GLES20.GlTexture2d, 0, m_bitmap, 0);
                int tmpiRet;
                tmpiRet = GLES20.GlGetError();
                if (tmpiRet != 0)
                {
                    ii = 0;
                }


                iErr = 0;
            errLabel:

                /*
                if (bitmap != null)
                {
                    bitmap.Recycle();
                }
                */

                return iErr == 0 ? texId : 0;
            }


            //
            public virtual int fillTex_run()
            {
                return 0;
            }

        }

        //
        public CQmcDrawTmpl(Activity_dlgTalk_av activity_DlgTalk_Av, int iFgType, WindowSurface windowSurface)
        {
            //
            mOwner = activity_DlgTalk_Av.m_fg_dlgTalk_av2;
            miFgType = iFgType;
            //
            mWindowSurface = windowSurface;


            //
            mVertexBuffer = ByteBuffer.AllocateDirect(VERTEX.Length * 4)
                .Order(ByteOrder.NativeOrder())
                .AsFloatBuffer()
                .Put(VERTEX);
            mVertexBuffer.Position(0);


            //
            mVertexIndexBuffer = ByteBuffer.AllocateDirect(VERTEX_INDEX.Length * 2)
                .Order(ByteOrder.NativeOrder())
                .AsShortBuffer()
                .Put(VERTEX_INDEX);
            mVertexIndexBuffer.Position(0);

            //
            mTexVertexBuffer = ByteBuffer.AllocateDirect(TEX_VERTEX.Length * 4)
                .Order(ByteOrder.NativeOrder())
                .AsFloatBuffer()
                .Put(TEX_VERTEX);
            mTexVertexBuffer.Position(0);

            //
            Matrix.SetIdentityM(mMMatrix, 0);
            Matrix.SetLookAtM(mVMatrix, 0, 3, 3, 10, 0, 0, 0, 0, 1, 0);

            //
          
            qyFuncs_Droid.matrix2MY_MATRIX(ref mMMatrix, ref this.mMats.matWorld);
            qyFuncs_Droid.matrix2MY_MATRIX(ref mVMatrix, ref this.mMats.matView);

        }

        static int loadShader(int type, string shaderCode)
        {
            int shader = GLES20.GlCreateShader(type);
            GLES20.GlShaderSource(shader, shaderCode);
            GLES20.GlCompileShader(shader);
            return shader;
        }

        static Android.Graphics.Bitmap getBitmap(Activity context, int vectorDrawableId)
        {
            Android.Graphics.Bitmap bitmap = null;
            if (Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.Lollipop)
            {
                try
                {
                    Android.Graphics.Drawables.Drawable vectorDrawable = context.GetDrawable(vectorDrawableId);
                    bitmap = Android.Graphics.Bitmap.CreateBitmap(vectorDrawable.IntrinsicWidth,
                        vectorDrawable.IntrinsicHeight, Android.Graphics.Bitmap.Config.Argb8888);
                    Android.Graphics.Canvas canvas = new Android.Graphics.Canvas(bitmap);
                    vectorDrawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                    vectorDrawable.Draw(canvas);

                }
                catch (Java.Lang.Exception e)
                {
                    int ii = 0;
                }
            }
            else
            {
                bitmap = Android.Graphics.BitmapFactory.DecodeResource(context.Resources, vectorDrawableId);
            }
            return bitmap;

        }

        //
        public virtual int init()
        {            //
            mEglCore_shared = new EglCore(mWindowSurface.MEglCore.MEGLContext, EglCore.FlagTryGles3);
            //
            //mTextureLoader = new TextureLoader(mEglCore_shared, mOwner.Activity, this);
            //mTextureLoader.Start();

            //
            mProgram = GLES20.GlCreateProgram();
            int vertexShaner = loadShader(GLES20.GlVertexShader, VERTEX_SHADER);
            int fragmentShader = loadShader(GLES20.GlFragmentShader, FRAGMENT_SHADER);
            GLES20.GlAttachShader(mProgram, vertexShaner);
            GLES20.GlAttachShader(mProgram, fragmentShader);
            GLES20.GlLinkProgram(mProgram);

            GLES20.GlUseProgram(mProgram);

            mPositionHandle = GLES20.GlGetAttribLocation(mProgram, "vPosition");
            mTexCoordHandle = GLES20.GlGetAttribLocation(mProgram, "a_texCoord");
            //
            mMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "mMatrix");
            vMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "vMatrix");
            pMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "pMatrix");
            //
            mTexSmaplerHandle = GLES20.GlGetUniformLocation(mProgram, "s_texture");

            //GLES20.GlEnableVertexAttribArray(mPositionHandle);
            //GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer_me);
            //
            GLES20.GlEnableVertexAttribArray(mTexCoordHandle);
            GLES20.GlVertexAttribPointer(mTexCoordHandle, 2, GLES20.GlFloat, false, 0, mTexVertexBuffer);

            int[] texNames = new int[1];
            GLES20.GlGenTextures(1, texNames, 0);
            //
            qyFuncs.showInfo_open(0, "", "drawTmpl.init,glGentexure 1");
            //
            mTexName_default = texNames[0];
            //
            Android.Graphics.Bitmap bitmap = getBitmap(mOwner.Activity, Resource.Drawable.bj852x480);
            //
            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName_default);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter,
                GLES20.GlLinear);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter,
                GLES20.GlLinear);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS,
                GLES20.GlRepeat);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT,
                GLES20.GlRepeat);
            GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);
            //
            this.mTexName_default_iW = bitmap.Width;
            this.mTexName_default_iH = bitmap.Height;
            //
            bitmap.Recycle();



            return 0;
        }


        //
        public virtual void exit()
        {
            this.bQuit = true;
            //
            //mTextureLoader.halt();
            //
            if (mEglCore_shared!=null)
            {
                mEglCore_shared.Release();
            }
        

            //
            GLES20.GlDeleteTextures(1, new int[] { mTexName_default }, 0);
        }


        //
 


        //
        public virtual int onSize(int w, int h)
        {
            if (w == 0 || h == 0) return -1;

            GLES20.GlViewport(0, 0, w, h);

            Matrix.PerspectiveM(mPMatrix, 0, 45, (float)w / h, 0.1f, 100.0f);

            //
            qyFuncs_Droid.matrix2MY_MATRIX(ref mPMatrix, ref this.mMats.matProj);

            //
            this.mMats.vp.Width = w;
            this.mMats.vp.Height = h;

            //
            int width = w;
            int height = h;
            //
            float fz=Consts.CONST_fz_peer;// = 1f - 0.000001f;
            //
            System.Numerics.Vector3 v3 = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, 0, 0);
            System.Numerics.Vector3 v31 = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, width, 0);
            System.Numerics.Vector3 v32 = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, width, height);
            System.Numerics.Vector3 v33 = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, 0, height);

            //
            System.Numerics.Vector4 v4;
            qyFuncs_Droid.get_pt_far_mats(null, 0, 0, ref this.mMats, fz, out v4);


            //
            fz = Consts.CONST_fz_me;// 1f - 0.000002f;

            //
            System.Numerics.Vector3 v3_me = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, width*2/3, height*2/3);
            System.Numerics.Vector3 v31_me = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, width, height*2/3);
            System.Numerics.Vector3 v32_me = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, width, height);
            System.Numerics.Vector3 v33_me = qyFuncs_Droid.projectTouchToWorld(width, height, fz,mVMatrix, mPMatrix, width*2/3, height);


            //
            System.Numerics.Vector3 v3_me_l = qyFuncs_Droid.projectTouchToWorld(width, height, fz, mVMatrix, mPMatrix, 0, height * 2 / 3);
            System.Numerics.Vector3 v31_me_l = qyFuncs_Droid.projectTouchToWorld(width, height, fz, mVMatrix, mPMatrix, width/3, height * 2 / 3);
            System.Numerics.Vector3 v32_me_l = qyFuncs_Droid.projectTouchToWorld(width, height, fz, mVMatrix, mPMatrix, width/3, height);
            System.Numerics.Vector3 v33_me_l = qyFuncs_Droid.projectTouchToWorld(width, height, fz, mVMatrix, mPMatrix, 0, height);






            //
            return 0;
        }

 
        public virtual void draw()
        {
            GLES20.GlClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);
            //
            GLES20.GlUniformMatrix4fv(mMatrixHandle, 1, false, mMMatrix, 0);
            GLES20.GlUniformMatrix4fv(vMatrixHandle, 1, false, mVMatrix, 0);
            GLES20.GlUniformMatrix4fv(pMatrixHandle, 1, false, mPMatrix, 0);

            //
            GLES20.GlEnableVertexAttribArray(mPositionHandle);
            GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer);

            //
            GLES20.GlActiveTexture(GLES20.GlTexture0);
            /*
            if (this.m_textureId_0 != 0 || this.m_textureId_1 != 0)
            {
                if (m_cur_textureId == 0)
                {
                    if (m_textureId_0 != 0)
                    {
                        m_cur_textureId = m_textureId_0;
                    }
                    else
                    {
                        m_cur_textureId = m_textureId_1;
                    }
                }
                else
                {
                    if (m_cur_textureId == m_textureId_0
                        && m_textureId_1 != 0)
                    {
                        m_cur_textureId = m_textureId_1;
                        m_textureId_0 = 0;
                        //
                        qyFuncs.traceLog("myDraw: draw, free tex0");

                    }
                    else if (m_cur_textureId == m_textureId_1
                        && m_textureId_0 != 0)
                    {
                        m_cur_textureId = m_textureId_0;
                        m_textureId_1 = 0;
                        //
                        qyFuncs.traceLog("myDraw: draw, free tex1");

                    }
                }
                //
            }
            if (m_cur_textureId != 0)
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, this.m_cur_textureId);
            }
            else
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName);
            }
            */
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName_default);

            //
            GLES20.GlUniform1i(mTexSmaplerHandle, 0);

            //
            GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                GLES20.GlUnsignedShort, mVertexIndexBuffer);


            //
            /*
            GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer_me);

            //
            if (this.m_textureId_0 != 0 || this.m_textureId_1 != 0)
            {
                if (m_cur_textureId == 0)
                {
                    if (m_textureId_0 != 0)
                    {
                        m_cur_textureId = m_textureId_0;
                    }
                    else
                    {
                        m_cur_textureId = m_textureId_1;
                    }
                }
                else
                {
                    if (m_cur_textureId == m_textureId_0
                        && m_textureId_1 != 0)
                    {
                        m_cur_textureId = m_textureId_1;
                        m_textureId_0 = 0;
                        //
                        qyFuncs.traceLog("myDraw: draw, free tex0");

                    }
                    else if (m_cur_textureId == m_textureId_1
                        && m_textureId_0 != 0)
                    {
                        m_cur_textureId = m_textureId_0;
                        m_textureId_1 = 0;
                        //
                        qyFuncs.traceLog("myDraw: draw, free tex1");

                    }
                }
                //
            }
            if (m_cur_textureId != 0)
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, this.m_cur_textureId);
            }
            else
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName);
            }

            //
            GLES20.GlUniform1i(mTexSmaplerHandle, 0);

            //
            GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                GLES20.GlUnsignedShort, mVertexIndexBuffer);
            */

            //
            this.m_loop_ctrl++;
            int curTickCnt = (int)qyFuncs.myGetTickCount(null);
            int iElapseInMs = Java.Lang.Math.Abs((int)(this.m_lastFps_tickCnt - curTickCnt));
            if (iElapseInMs >= 1000)
            {
                float fps = (this.m_loop_ctrl - m_lastFps_loop_ctrl) * 1000.0f / (float)iElapseInMs;
                string str = string.Format("myDraw: fps {0}", fps);
                qyFuncs.traceLog(str);
                //
                this.m_lastFps_loop_ctrl = m_loop_ctrl;
                this.m_lastFps_tickCnt = curTickCnt;
            }


            return;
        }







    }

}