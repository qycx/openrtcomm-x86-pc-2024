using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Com.Qycx.Gleslibrary;
using Android.Graphics;
using Java.Nio;
using Android.Opengl;
using Android.OS;
using qy;
using Android.Graphics;
using Matrix = Android.Opengl.Matrix;

namespace qyMessengerApp.Android
{
    public class MyDraw
    {
        Activity mOwner;
        WindowSurface mWindowSurface;
        static int textureId;
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

        public MyDraw(Activity owner, WindowSurface eglSurface)
        {
            //
            mOwner = owner;
            //
            mWindowSurface = eglSurface;


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
            if (Build.VERSION.SdkInt >BuildVersionCodes.Lollipop)
            {
                try
                {
                   Drawable vectorDrawable = context.GetDrawable(vectorDrawableId);
                    bitmap =Bitmap.CreateBitmap(vectorDrawable.IntrinsicWidth,
                        vectorDrawable.IntrinsicHeight,Bitmap.Config.Argb8888);
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
                bitmap =BitmapFactory.DecodeResource(context.Resources, vectorDrawableId);
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

            //GLES20.GlEnableVertexAttribArray(mPositionHandle);
            //GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer_me);
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
            //
            GLES20.GlUniformMatrix4fv(mMatrixHandle, 1, false, mMMatrix, 0);
            GLES20.GlUniformMatrix4fv(vMatrixHandle, 1, false, mVMatrix, 0);
            GLES20.GlUniformMatrix4fv(pMatrixHandle, 1, false, mPMatrix, 0);

            //
            GLES20.GlEnableVertexAttribArray(mPositionHandle);
            GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer);

            //
            GLES20.GlActiveTexture(GLES20.GlTexture0);

            if (MyDraw.textureId != 0)
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, MyDraw.textureId);
            }
            else
                GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName);

            //
            GLES20.GlUniform1i(mTexSamplerHandle, 0);
            //GLES20.glDrawArrays(GLES20.GL_TRIANGLES,0,3);
            GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                    GLES20.GlUnsignedShort, mVertexIndexBuffer);
        }
    }
}
