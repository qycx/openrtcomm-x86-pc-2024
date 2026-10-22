using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Nio;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using qyMessengerApp.Android;
namespace qy.Droid
{
#if true
    public abstract class SampleBase
    {

        private string TAG = "SampleBase";

        private string []vertexShaderSource =
                {
                    "precision mediump float;\n" +
                    "attribute vec3 aPosition;\n" +

                    "void main() {\n" +
                    "	gl_Position = vec4(aPosition, 1.0);\n" +
                    "}\n",

                    "precision mediump float;\n" +
                    "attribute vec3 aPosition;\n" +
                    "uniform mat4 uMatrix;\n" +
                    "void main() {\n" +
                    "	gl_Position = uMatrix * vec4(aPosition, 1.0);\n" +
                    "}\n",

                    "precision mediump float;\n" +
                    "attribute vec3 aPosition;\n" +
                    "uniform mat4 uProjMatrix;\n" +
                    "uniform mat4 uMVMatrix;\n" +
                    "void main() {\n" +
                    "	gl_Position = uProjMatrix * uMVMatrix * vec4(aPosition, 1.0);\n" +
                    "}\n",

                    "precision mediump float;\n" +
                    "attribute vec3 aPosition;\n" +
                    "attribute vec3 aNormal;\n" +
                    "uniform mat4 uProjMatrix;\n" +
                    "uniform mat4 uViewMatrix;\n" +
                    "uniform mat4 uModelMatrix;\n" +
                    "uniform vec3 uLightColor;\n" +
                    "uniform vec3 uLightDir;\n" +
                    "uniform vec3 uMaterialDiffuse;\n" +
                    "varying vec3 vColor;\n" +
                    "void main() {\n" +
                    "	gl_Position = uProjMatrix * uViewMatrix * uModelMatrix * vec4(aPosition, 1.0);\n" +
                    "   vec3 normal_cameraspace = (uViewMatrix * uModelMatrix * vec4(aNormal, 1.0)).xyz;\n" +
                    "   vec3 lightdir_cameraspace = (uViewMatrix * vec4(uLightDir, 1.0)).xyz;" +
                    "   vec3 n = normalize(normal_cameraspace);\n" +
                    "   vec3 l = normalize(lightdir_cameraspace);\n" +
                    "   float cosTheta = clamp( dot(n, l), 0.0, 1.0);" +
                    "   vColor = uMaterialDiffuse * uLightColor * cosTheta;" +
                    "}\n",

                "precision mediump float;\n" +
                "attribute vec3 aPosition;\n" +
                "attribute vec3 aNormal;\n" +
                "attribute vec2 aTexCoord;\n" +
                "uniform mat4 uProjMatrix;\n" +
                "uniform mat4 uViewMatrix;\n" +
                "uniform mat4 uModelMatrix;\n" +
                "uniform vec3 uLightColor;\n" +
                "uniform vec3 uLightDir;\n" +
                "uniform vec3 uMaterialDiffuse;\n" +
                "varying vec3 vColor;\n" +
                "varying vec2 vTexCoord;\n" +
                "void main() {\n" +
                "   vTexCoord = aTexCoord;\n" +
                "	gl_Position = uProjMatrix * uViewMatrix * uModelMatrix * vec4(aPosition, 1.0);\n" +
                "   vec3 normal_cameraspace = (uViewMatrix * uModelMatrix * vec4(aNormal, 1.0)).xyz;\n" +
                "   vec3 lightdir_cameraspace = (uViewMatrix * vec4(uLightDir, 1.0)).xyz;" +
                "   vec3 n = normalize(normal_cameraspace);\n" +
                "   vec3 l = normalize(lightdir_cameraspace);\n" +
                "   float cosTheta = clamp( dot(n, l), 0.0, 1.0);" +
                "   vColor = uMaterialDiffuse * uLightColor * cosTheta;" +
                "}\n",

                // test
                                "precision mediump float;\n" +
                "uniform mat4 uProjMatrix;\n" +
                "uniform mat4 uViewMatrix;\n" +
                "uniform mat4 uModelMatrix;\n" +
                "attribute vec3 aPosition;\n" +
                "attribute vec2 aTexCoord;\n" +
                "varying vec2 vTexCoord;\n" +
                "void main() {\n" +
                "   vTexCoord = aTexCoord;\n" +
                "	gl_Position = uProjMatrix * uViewMatrix * uModelMatrix * vec4(aPosition, 1.0);\n" +
                "}\n"



            };

        private string []fragmentShaderSource =
                {
                    "precision mediump float;\n" +
                    "uniform vec3 uColor;\n" +
                    "void main() {\n" +
                    "   gl_FragColor = vec4(uColor, 1.0);\n" +
                    "}\n",

                    "precision mediump float;\n" +
                    "uniform vec3 uColor;\n" +
                    "void main() {\n" +
                    "   gl_FragColor = vec4(uColor, 1.0);\n" +
                    "}\n",

                    "precision mediump float;\n" +
                    "uniform vec3 uColor;\n" +
                    "void main() {\n" +
                    "   gl_FragColor = vec4(uColor, 1.0);\n" +
                    "}\n",

                    "precision mediump float;\n" +
                    "varying vec3 vColor;\n" +
                    "void main() {\n" +
                    "   gl_FragColor = vec4(vColor, 1.0);\n" +
                    "}\n",

    "precision mediump float;\n" +
    "uniform sampler2D sTexture0;\n" +
    "varying vec3 vColor;\n" +
    "varying vec2 vTexCoord;\n" +
    "void main() {\n" +
    "   vec4 texColor = texture2D(sTexture0, vTexCoord);\n" +
    "   gl_FragColor = vec4(vColor, 1.0) * texColor;\n" +
    "}\n",

    // test
        "precision mediump float;\n" +
    "uniform sampler2D sTexture0;\n" +
    "varying vec2 vTexCoord;\n" +
    "void main() {\n" +
    "   vec4 texColor = texture2D(sTexture0, vTexCoord);\n" +
    "   gl_FragColor = texColor;\n" +
    "}\n"

            };

        protected int SHADER_TYPE_DEFAULT = 0;
        protected int SHADER_TYPE_MATRIX = 1;
        protected int SHADER_TYPE_PROJECTION = 2;
        protected int SHADER_TYPE_LIGHTING = 3;
        protected int SHADER_TYPE_TEXTURE = 4;
        protected int SHADER_TYPE_TEST = 5;

        protected int createProgram(int shaderType, ref int vertexShader_o, ref int fragmentShader_o )
        {
            int program = GLES20.GlCreateProgram();

            int vertexShader = getShader(GLES20.GlVertexShader, vertexShaderSource[shaderType]);
            int fragmentShader = getShader(GLES20.GlFragmentShader, fragmentShaderSource[shaderType]);

            GLES20.GlAttachShader(program, vertexShader);
            GLES20.GlAttachShader(program, fragmentShader);
            GLES20.GlLinkProgram(program);

            //
            vertexShader_o = vertexShader;
            fragmentShader_o=fragmentShader;


            //
            return program;
        }

        private int getShader(int type, String shaderSource)
        {
            int shader = GLES20.GlCreateShader(type);
            GLES20.GlShaderSource(shader, shaderSource);
            GLES20.GlCompileShader(shader);

            int[] compiled = new int[1];
            GLES20.GlGetShaderiv(shader, GLES20.GlCompileStatus, compiled, 0);
            if (compiled[0] == 0)
            {
                Log.Error(TAG, "Could not compile shader");
                Log.Error(TAG, GLES20.GlGetShaderInfoLog(shader));
                Log.Error(TAG, shaderSource);
            }
            else
            {
                Log.Debug(TAG, "Shader = " + shader);
            }

            return shader;
        }

        protected FloatBuffer createFloatBuffer(float[] values)
        {
            int Float_SIZE = 4;
            FloatBuffer buffer = ByteBuffer.AllocateDirect(values.Length * Float_SIZE).
                    Order(ByteOrder.NativeOrder()).AsFloatBuffer();

            buffer.Put(values);
            buffer.Position(0);

            return buffer;
        }


        public abstract void resize(int width, int height, float angle_z_unused);
        public abstract void draw(float angle_z);
        public abstract void destroy(); 

    }

        public class SimpleTexture : SampleBase
        {

    private String TAG = "SimpleTexture";

        private int mProgram = -1;
        private int mVertexShader = -1;
        private int mFragmentShader = -1;

        //
        private FloatBuffer mPosition;
        private FloatBuffer mNormal;
        private FloatBuffer mTexCoord;

        //
        public ShortBuffer mVertexIndexBuffer;


        // texture name
        public int mTextureName;
        bool bCreated_mTextureName;

        // 4x4 matrix
        private float[] mModelMatrix = new float[16];
        private float[] mViewMatrix = new float[16];
        private float[] mProjMatrix = new float[16];

        //
        float[] position = {
                -0.5f, 0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                -0.5f, 0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.5f, 0.5f, 0.0f
        };

        //
        public short[] VERTEX_INDEX = { 0, 1, 2, 3, 4, 5 };


        //
        public SimpleTexture(Context context,int texId, bool b_test, string hint)
        {
            string str;

            if (hint == null) hint = "";

            str = string.Format("SimpleTexture: SimpleTexture() called, texId {0}, {1}",texId,hint);
            qyFuncs.showInfo_open(0, null, str);


            // create shader program
            int shaderType = SHADER_TYPE_TEXTURE;
#if DEBUG
            if (b_test)
            {
                shaderType = SHADER_TYPE_TEST;
            }

#endif
        mProgram = createProgram(shaderType,ref mVertexShader, ref mFragmentShader);

#if DEBUG
#if false
            qyFuncs_Droid.checkGlError("1");
            GLES20.GlUseProgram(mProgram);
            qyFuncs_Droid.checkGlError("2");
#endif
#endif


            // same direction (z direction)
            float[] normal = {
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
        };

            // texture coordinate
            float[] texCoord = {
                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 0.0f
        };

            mPosition = createFloatBuffer(position);
            mNormal = createFloatBuffer(normal);
            mTexCoord = createFloatBuffer(texCoord);

            //
            //
            mVertexIndexBuffer = ByteBuffer.AllocateDirect(VERTEX_INDEX.Length * 2)
                .Order(ByteOrder.NativeOrder())
                .AsShortBuffer()
                .Put(VERTEX_INDEX);
            mVertexIndexBuffer.Position(0);



            // init matrix value
            Android.Opengl.Matrix.SetIdentityM(mModelMatrix, 0);
            Android.Opengl.Matrix.SetIdentityM(mViewMatrix, 0);
            Android.Opengl.Matrix.TranslateM(mViewMatrix, 0, 0.0f, 0.0f, -2.5f);
            Android.Opengl.Matrix.SetIdentityM(mProjMatrix, 0);

            // intializing the texture
            if (texId > 0)
            {
                mTextureName = texId;
            }
            else
            {
                Bitmap texImage = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.bj852x480);

                initTexture(texImage);
                texImage.Recycle();
                //
                bCreated_mTextureName = true;
            }
        }

        private void initTexture(Bitmap image)
        {
            int[] name = new int[1];
            GLES20.GlGenTextures(1, name, 0);
            //
            qyFuncs.showInfo_open(0, "", "simpleTex.initTex,glGenTex 1");

            //
            bool bDbg = false;
            string str;
            bDbg = true;
            if (bDbg)
            {
                str = string.Format("initTexture,genTex {0}", name[0]);
                qyFuncs.showInfo_open(0, null, str);
            }


            //
            if (name[0] > 0)
            {
                GLES20.GlBindTexture(GLES20.GlTexture2d, name[0]);

                // setting parameters
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlLinear);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlLinear);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);

                // use gl utils to load texture by Bitmap
                GLUtils.TexImage2D(GLES20.GlTexture2d, 0, image, 0);

                // texture binding to 0
                GLES20.GlBindTexture(GLES20.GlTexture2d, 0);

                mTextureName = name[0];
            }
            else
            {
                Log.Error(TAG, "Fail to bind texture");
            }

        }

        
    public override void resize(int width, int height, float angle_z_unused)
        {

            GLES20.GlViewport(0, 0, width, height);

            // perspective projection
            //Android.Opengl.Matrix.PerspectiveM(mProjMatrix, 0, 45.0f, (float)width / (float)height, 1.0f, 10.0f);
            Android.Opengl.Matrix.PerspectiveM(mProjMatrix, 0, 45.0f, (float)width / (float)height, 0.1f, 100.0f);

            //
            Android.Opengl.Matrix.SetIdentityM(mModelMatrix, 0);
             

            //
            int ix = 0;
            int iy = 0;
            int iw = width;
            int ih = height;

            //
            //
            myPOINT realPoint_lt, realPoint_rt, realPoint_lb, realPoint_rb;

            //
            realPoint_lt.x = (int)ix;
            realPoint_lt.y = (int)iy;

            realPoint_rt.x = (int)(ix + iw);
            realPoint_rt.y = (int)iy;

            realPoint_lb.x = (int)ix;
            realPoint_lb.y = (int)(iy + ih);

            realPoint_rb.x = (int)(ix + iw);
            realPoint_rb.y = (int)(iy + ih);




            //
#if true
            //
            QIS_mats mats = new QIS_mats();
            qyFuncs_Droid.matrix2MY_MATRIX(ref this.mModelMatrix, ref mats.matWorld);
            qyFuncs_Droid.matrix2MY_MATRIX(ref this.mProjMatrix, ref mats.matProj);
            qyFuncs_Droid.matrix2MY_MATRIX(ref this.mViewMatrix, ref mats.matView);
            mats.vp.Width = iw;
            mats.vp.Height = ih;

            //
            float z_1 = Consts.CONST_fz_z1;
            object pCtx = null;

            //
            System.Numerics.Vector4 pt_far;
            int index = 0;

            //  get_pt_far(  pCtx,  realPoint_rt.x,  realPoint_rt.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_lt.x, realPoint_lt.y, ref mats, z_1, out pt_far);

            this.position[index] = qyFuncs_Droid.XMVectorGetX(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetY(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetZ(pt_far);
            index++;

            //  get_pt_far(  pCtx,  realPoint_lt.x,  realPoint_lt.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_lb.x, realPoint_lb.y, ref mats, z_1, out pt_far);

            this.position[index] = qyFuncs_Droid.XMVectorGetX(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetY(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetZ(pt_far);
            index++;

            //  get_pt_far(  pCtx,  realPoint_lb.x,  realPoint_lb.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_rb.x, realPoint_rb.y, ref mats, z_1, out pt_far);

            this.position[index] = qyFuncs_Droid.XMVectorGetX(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetY(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetZ(pt_far);
            index++;

            //  get_pt_far(  pCtx,  realPoint_rt.x,  realPoint_rt.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_lt.x, realPoint_lt.y, ref mats, z_1, out pt_far);

            this.position[index] = qyFuncs_Droid.XMVectorGetX(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetY(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetZ(pt_far);
            index++;

            //  get_pt_far(  pCtx,  realPoint_lb.x,  realPoint_lb.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_rb.x, realPoint_rb.y, ref mats, z_1, out pt_far);

            this.position[index] = qyFuncs_Droid.XMVectorGetX(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetY(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetZ(pt_far);
            index++;

            //  get_pt_far(  pCtx,  realPoint_lb.x,  realPoint_lb.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_rt.x, realPoint_rt.y, ref mats, z_1, out pt_far);

            this.position[index] = qyFuncs_Droid.XMVectorGetX(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetY(pt_far);
            index++;
            this.position[index] = qyFuncs_Droid.XMVectorGetZ(pt_far);
            index++;

            //
            this.mPosition.Put(position);
            this.mPosition.Position(0);


#endif


        }


        public override void draw(float angle_z)
        {

            GLES20.GlUseProgram(mProgram);

            // screen clear color
            GLES20.GlClearColor(0.6f, 0.6f, 0.6f, 1.0f);

            // clear screen
            GLES20.GlClear(GLES20.GlColorBufferBit);

            int aPositionLoc = GLES20.GlGetAttribLocation(mProgram, "aPosition");
            GLES20.GlVertexAttribPointer(aPositionLoc, 3, GLES20.GlFloat, false, 0, mPosition);

            int aNormalLoc = GLES20.GlGetAttribLocation(mProgram, "aNormal");
            GLES20.GlVertexAttribPointer(aNormalLoc, 3, GLES20.GlFloat, false, 0, mNormal);

            int aTexCoordLoc = GLES20.GlGetAttribLocation(mProgram, "aTexCoord");
            GLES20.GlVertexAttribPointer(aTexCoordLoc, 2, GLES20.GlFloat, false, 0, mTexCoord);

            // light color
            //GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightColor"), 0.8f, 0.8f, 0.8f);
            GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightColor"), 1.0f, 1.0f, 1.0f);

            // triangle material color
            GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uMaterialDiffuse"), 1.0f, 1.0f, 1.0f);

            // light dir
            GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightDir"), 1.0f, -1.0f, -1.0f);

            // triangle model matrix
            // no transform (identity matrix)
            // y-axis, 1 degree rotation every frames
            //Matrix.rotateM(mModelMatrix, 0, 1.0f, 0.0f, 1.0f, 0.0f);
            //
            Android.Opengl.Matrix.RotateM(mModelMatrix, 0, angle_z, 0.0f, 0.0f, 1.0f);

            //
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uModelMatrix"), 1, false, mModelMatrix, 0);

            // view matrix
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uViewMatrix"), 1, false, mViewMatrix, 0);

            // projection matrix
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uProjMatrix"), 1, false, mProjMatrix, 0);

            GLES20.GlEnableVertexAttribArray(aPositionLoc);
            GLES20.GlEnableVertexAttribArray(aNormalLoc);
            GLES20.GlEnableVertexAttribArray(aTexCoordLoc);

            // texture 0
            GLES20.GlUniform1i(GLES20.GlGetUniformLocation(mProgram, "sTexture0"), 0);

            // binding texture
            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTextureName);

            // rendering
            GLES20.GlDrawArrays(GLES20.GlTriangles, 0, 6);

            // binding texture to 0
            GLES20.GlBindTexture(GLES20.GlTexture2d, 0);

            GLES20.GlDisableVertexAttribArray(aPositionLoc);
            GLES20.GlDisableVertexAttribArray(aNormalLoc);
            GLES20.GlDisableVertexAttribArray(aTexCoordLoc);

            // starting gl command buffer, non-block
            GLES20.GlFlush();

        }


        public void test_draw(int w, int h, float angle_z)
        {
            //test_draw_shaderType_default(w, h, angle_z);

#if true
            resize(w, h,0);
            test_draw_test(0);
#endif

            return;
        }


        public void test_draw_test(float angle_z)
        {

            GLES20.GlUseProgram(mProgram);

            // screen clear color
            GLES20.GlClearColor(0.6f, 0.6f, 0.6f, 1.0f);

            // clear screen
            GLES20.GlClear(GLES20.GlColorBufferBit);

            int aPositionLoc = GLES20.GlGetAttribLocation(mProgram, "aPosition");
            GLES20.GlVertexAttribPointer(aPositionLoc, 3, GLES20.GlFloat, false, 0, mPosition);


            int aTexCoordLoc = GLES20.GlGetAttribLocation(mProgram, "aTexCoord");
            GLES20.GlVertexAttribPointer(aTexCoordLoc, 2, GLES20.GlFloat, false, 0, mTexCoord);


            // triangle model matrix
            // no transform (identity matrix)
            // y-axis, 1 degree rotation every frames
            //Matrix.rotateM(mModelMatrix, 0, 1.0f, 0.0f, 1.0f, 0.0f);
            //
            Android.Opengl.Matrix.RotateM(mModelMatrix, 0, angle_z, 0.0f, 0.0f, 1.0f);

            //
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uModelMatrix"), 1, false, mModelMatrix, 0);

            // view matrix
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uViewMatrix"), 1, false, mViewMatrix, 0);

            // projection matrix
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uProjMatrix"), 1, false, mProjMatrix, 0);

            GLES20.GlEnableVertexAttribArray(aPositionLoc);
            GLES20.GlEnableVertexAttribArray(aTexCoordLoc);

            // texture 0
            GLES20.GlUniform1i(GLES20.GlGetUniformLocation(mProgram, "sTexture0"), 0);

            // binding texture
            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTextureName);

            // rendering
            //GLES20.GlDrawArrays(GLES20.GlTriangles, 0, 6);
            GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,    
                GLES20.GlUnsignedShort, mVertexIndexBuffer);

            //


            // binding texture to 0
            GLES20.GlBindTexture(GLES20.GlTexture2d, 0);

            GLES20.GlDisableVertexAttribArray(aPositionLoc);
            GLES20.GlDisableVertexAttribArray(aTexCoordLoc);

            // starting gl command buffer, non-block
            GLES20.GlFlush();

        }


        //
        private void test_draw_shaderType_default(int w, int h, float angle_z)
        {

            GLES20.GlUseProgram(mProgram);

            // screen clear color
            GLES20.GlClearColor(1.0f, 0.0f, 0.0f, 1.0f);

            // clear screen
            GLES20.GlClear(GLES20.GlColorBufferBit);

            int aPositionLoc = GLES20.GlGetAttribLocation(mProgram, "aPosition");
            GLES20.GlVertexAttribPointer(aPositionLoc, 3, GLES20.GlFloat, false, 0, mPosition);
#if false

            int aNormalLoc = GLES20.GlGetAttribLocation(mProgram, "aNormal");
            GLES20.GlVertexAttribPointer(aNormalLoc, 3, GLES20.GlFloat, false, 0, mNormal);

            int aTexCoordLoc = GLES20.GlGetAttribLocation(mProgram, "aTexCoord");
            GLES20.GlVertexAttribPointer(aTexCoordLoc, 2, GLES20.GlFloat, false, 0, mTexCoord);

            // light color
            //GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightColor"), 0.8f, 0.8f, 0.8f);
            GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightColor"), 1.0f, 1.0f, 1.0f);

            // triangle material color
            GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uMaterialDiffuse"), 1.0f, 1.0f, 1.0f);

            // light dir
            GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightDir"), 1.0f, -1.0f, -1.0f);

            // triangle model matrix
            // no transform (identity matrix)
            // y-axis, 1 degree rotation every frames
            //Matrix.rotateM(mModelMatrix, 0, 1.0f, 0.0f, 1.0f, 0.0f);
            //
            Android.Opengl.Matrix.RotateM(mModelMatrix, 0, angle_z, 0.0f, 0.0f, 1.0f);

            //
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uModelMatrix"), 1, false, mModelMatrix, 0);

            // view matrix
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uViewMatrix"), 1, false, mViewMatrix, 0);

            // projection matrix
            GLES20.GlUniformMatrix4fv(GLES20.GlGetUniformLocation(mProgram, "uProjMatrix"), 1, false, mProjMatrix, 0);

            GLES20.GlEnableVertexAttribArray(aPositionLoc);
            GLES20.GlEnableVertexAttribArray(aNormalLoc);
            GLES20.GlEnableVertexAttribArray(aTexCoordLoc);

            // texture 0
            GLES20.GlUniform1i(GLES20.GlGetUniformLocation(mProgram, "sTexture0"), 0);

            // binding texture
            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTextureName);

            // rendering
            GLES20.GlDrawArrays(GLES20.GlTriangles, 0, 6);

            // binding texture to 0
            GLES20.GlBindTexture(GLES20.GlTexture2d, 0);

            GLES20.GlDisableVertexAttribArray(aPositionLoc);
            GLES20.GlDisableVertexAttribArray(aNormalLoc);
            GLES20.GlDisableVertexAttribArray(aTexCoordLoc);
#endif

            //
            // light color
            //GLES20.GlUniform3f(GLES20.GlGetUniformLocation(mProgram, "uLightColor"), 0.8f, 0.8f, 0.8f);
            int i_uColor = GLES20.GlGetUniformLocation(mProgram, "uColor");
            qyFuncs_Droid.checkGlError("kk");
            GLES20.GlUniform3f(i_uColor, 0.0f, 1.0f, 0.0f);
            qyFuncs_Droid.checkGlError("kk");

            GLES20.GlEnableVertexAttribArray(aPositionLoc);

            // rendering
            GLES20.GlDrawArrays(GLES20.GlTriangles, 0, 6);

            GLES20.GlDisableVertexAttribArray(aPositionLoc);

            // starting gl command buffer, non-block
            GLES20.GlFlush();

        }



        //
        public override void destroy()
        {

            if (mProgram > 0)
            {
                GLES20.GlDeleteProgram(mProgram);
                mProgram = -1;
            }
            if (mVertexShader>0)
            {
                GLES20.GlDeleteShader(mVertexShader);
                mVertexShader = -1;
            }
            if (mFragmentShader > 0)
            {
                GLES20.GlDeleteShader(mFragmentShader);
                mFragmentShader = -1;
            }
            //
            if (bCreated_mTextureName)
            {
                if (mTextureName > 0)
                {                //
                    int[] texes = new int[1];
                    texes[0] = mTextureName;
                    GLES20.GlDeleteTextures(1, texes, 0);
                    mTextureName = 0;
                }
                //
                bCreated_mTextureName = false;
            }



            //
            mPosition = null;

        }

    }

#endif



}

