using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Opengl;
using Android.OS;
using Com.Qycx.Gleslibrary;
using Java.Nio;
using qy;
using qy.Droid;
using qyMessengerApp.Android;
using qyMessengerApp.Droid;
using Matrix = Android.Opengl.Matrix;

namespace MyGLE
{
    public class MyDraw
    {
        Activity mOwner;
        WindowSurface mWindowSurface;
      public  static int textureId;
        static string VERTEX_SHADER = "uniform mat4 mMatrix;" +
                    "uniform mat4 vMatrix;" +
                    "uniform mat4 pMatrix;" +
                    "attribute vec4 vPosition;" +
                    "attribute vec2 a_texCoord;" +
                    "varying vec2 v_texCoord;" +
                    "void main(){" +
                    " gl_Position=pMatrix * vMatrix * mMatrix * vPosition;" +
                    " v_texCoord=a_texCoord;" +
                    "}";
        static string FRAGMENT_SHADER = "precision mediump float;" +
                    "varying vec2 v_texCoord;" +
                    "uniform sampler2D s_texture;" +
                    " void main(){" +
                    "  gl_FragColor = texture2D(s_texture,v_texCoord);" +
                    "}";



        static float[] TEX_VERTEX = {
         //1,1,0,
            -4.966f,15.19f,-99.898f,
            //-1, 1, 0,
            -50,15,-86,
            //-1, -1, 0,
            -44,-64,-64,
            //1, -1, 0,
            1.6f,-64,-78,

    };

        static short[] VERTEX_INDEX = { 0, 1, 2, 0, 2, 3 };

        static float[] VERTEX = {
           1,0,
            0,0,
            0,1,
            1,1,
    };

        FloatBuffer mVertexBuffer;
        ShortBuffer mVertexIndexBuffer;

        FloatBuffer mTexVertexBuffer;
        float[] mMMatrix = new float[16];
        float[] mVMatrix = new float[16];
        float[] mPMatrix = new float[16];
        int mProgram;
        int mPositionHandle;
        int mMatrixHandle;
        int vMatrixHandle;
        int pMatrixHandle;
        int mTexCoordHandle;
        int mTexSamplerHandle;
        int mTexName;
        EglCore eglCore1;
        public MyDraw(Activity owner, WindowSurface eglSurface)
        {
            //
            mOwner = owner;
            //
            mWindowSurface = eglSurface;
            //
         //   EglCore eglCore1;
         //   eglCore1 = new EglCore(eglSurface.mEglCore.mEGLContext, EglCore.FLAG_TRY_GLES3);
            eglCore1 = new EglCore(eglSurface.MEglCore.MEGLContext, EglCore.FlagTryGles3);

            //
            //
          new TextureLoader(eglCore1, owner).Start();

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
            Android.Opengl.Matrix.SetIdentityM(mMMatrix, 0);
            Android.Opengl.Matrix.SetLookAtM(mVMatrix, 0, 3, 3, 10, 0, 0, 0, 0, 1, 0);
        }

        static int LoadShader(int type, string shaderCode)
        {
            int shader = GLES20.GlCreateShader(type);
            GLES20.GlShaderSource(shader, shaderCode);
            GLES20.GlCompileShader(shader);
            return shader;
        }

        static Bitmap GetBitmap(Activity context, int vectorDrawableId)
        {
            Bitmap bitmap = null;
            if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
            {
                try
                {
                    Drawable vectorDrawable = context.GetDrawable(vectorDrawableId);
                    bitmap = Bitmap.CreateBitmap(vectorDrawable.IntrinsicWidth,
                        vectorDrawable.IntrinsicHeight, Bitmap.Config.Argb8888);
                    Canvas canvas = new Canvas(bitmap);
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
                bitmap = BitmapFactory.DecodeResource(context.Resources, vectorDrawableId);
            }
            return bitmap;
        }

        public int Init()
        {
            mProgram = GLES20.GlCreateProgram();
            int vertexShaner = LoadShader(GLES20.GlVertexShader, VERTEX_SHADER);
            int fragmentShader = LoadShader(GLES20.GlFragmentShader, FRAGMENT_SHADER);
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
            mTexSamplerHandle = GLES20.GlGetUniformLocation(mProgram, "s_texture");

            GLES20.GlEnableVertexAttribArray(mPositionHandle);
            GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mTexVertexBuffer);
            //
            GLES20.GlEnableVertexAttribArray(mTexCoordHandle);
            GLES20.GlVertexAttribPointer(mTexCoordHandle, 2, GLES20.GlFloat, false, 0, mTexVertexBuffer);

            int[] texNames = new int[1];
            GLES20.GlGenTextures(1, texNames, 0);

            //
            mTexName = texNames[0];
            //
            Bitmap bitmap = GetBitmap(mOwner, Resource.Drawable.bj852x480);
            //
            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter,
                GLES20.GlLinear);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter,
                GLES20.GlLinear);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS,
                GLES20.GlRepeat);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT,
                GLES20.GlRepeat);
            GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);

            bitmap.Recycle();
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) return 0;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            Activity_dlgTalk hTalk_av = ((Activity_dlgTalk_av)mOwner).mOwner_hTalk;
          //  DLG_TALK_var m_var = dlgTalk.m_var;
            if (hTalk_av != null)
            {
                ((Activity_dlgTalk_av)mOwner).save_dlgTalk_share_eglContext(eglCore1.MEGLContext, "talkDraw.init");

                //

                //Fragment_dlgTalk_av2 fg = (Fragment_dlgTalk_av2)mOwner;
              ((Activity_dlgTalk_av)mOwner).mOwner_hTalk.gui_PostMessage(Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_chkTalkDraw, 0);
                   
                
            }


            return 0;
        }
        public int OnSize(int w, int h)
        {
            if (w == 0 || h == 0) return -1;

            GLES20.GlViewport(0, 0, w, h);

            Matrix.PerspectiveM(mPMatrix, 0, 45, (float)w / h, 0.1f, 100.0f);
            return 0;
        }

        public void Destroy()
        {
            GLES20.GlDeleteTextures(1, new int[] { mTexName }, 0);
        }

        public void Draw()
        {






            GLES20.GlClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);

            GLES20.GlUniformMatrix4fv(mMatrixHandle, 1, false, mMMatrix, 0);
            GLES20.GlUniformMatrix4fv(vMatrixHandle, 1, false, mVMatrix, 0);
            GLES20.GlUniformMatrix4fv(pMatrixHandle, 1, false, mPMatrix, 0);


            GLES20.GlEnableVertexAttribArray(mPositionHandle);
            GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer);


            GLES20.GlActiveTexture(GLES20.GlTexture0);

            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity =(MainActivity) pQyMc.gui.hMainWnd; 
            //if (pQyMc == null) return;
            //CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //Activity_dlgTalk dlgTalk =((Activity_dlgTalk_av) mOwner).mOwner_hTalk;
            //DLG_TALK_var m_var = dlgTalk.m_var;
            //int t1_texId = 0;
            //if (m_var.av.taskInfo.bTaskExists)
            //{
            //    TMP_tex_info tti = null;// ref texes.tti_peer;
            //                            //
            //    get_tti_peer(pProcInfo, dlgTalk, ref tti);
            //    //
            //    if (tti != null)
            //    {
            //        t1_texId = tti.texId;
            //    }
            //    qyFuncs.traceLog("TAG", t1_texId+"-------------");

            //}
            //qyFuncs.traceLog("TAG", t1_texId + "++++++++++++++");
            if (mainActivity.textureId==0)
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, MyDraw.textureId);
            
            }
            else
                GLES20.GlBindTexture(GLES20.GlTexture2d, mainActivity.textureId);
            qyFuncs.traceLog("TAG",   "textureId :"+ mainActivity.textureId);
            //
            GLES20.GlUniform1i(mTexSamplerHandle, 0);
            //GLES20.glDrawArrays(GLES20.GL_TRIANGLES,0,3);
            GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                    GLES20.GlUnsignedShort, mVertexIndexBuffer);
        }

        int get_tti_peer(CCtxQmc pProcInfo, Activity_dlgTalk hTalk, ref TMP_tex_info tti)
        {
            int iErr = -1;
            DLG_TALK_var m_var = hTalk.get_pm_var();

            //
            ref TMP_texes texes = ref pProcInfo.av.texes;
            if (texes == null) return -1;

            TMP_task_texes ttt = texes.taskAvTexes;
            int i;
            ushort usCnt_activeMems = ttt.usCnt_activeMems;
            if (usCnt_activeMems > ttt.tti_peers.Length) goto errLabel;
            if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
            {
                for (i = 0; i < usCnt_activeMems; i++)
                {
                    if (ttt.tti_peers[i].uiObjType == Consts.CONST_objType_mosaicStream_video
                        || ttt.tti_peers[i].uiObjType == Consts.CONST_objType_mosaicStream_resource)
                    {
                        tti = ttt.tti_peers[i];
                        break;
                    }
                }
            }
            else
            {
                // 暂时不处理屏幕共享
                for (i = 0; i < usCnt_activeMems; i++)
                {
                    if (ttt.tti_peers[i].idInfo.ui64Id == m_var.addr.idInfo.ui64Id
                        && ttt.tti_peers[i].uiObjType == 0)
                    {
                        tti = ttt.tti_peers[i];
                        break;
                    }
                }

            }

            iErr = 0;
        errLabel:

            return iErr;
        }

    }
}
