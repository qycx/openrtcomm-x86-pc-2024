using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
	partial class qyFuncs_Droid
{
        public static int bmp2Texture(Bitmap bmptexture)
        {
            int[] mTextures = new int[1];
            GLES20.GlGenTextures(1, mTextures, 0);
            //
            qyFuncs.showInfo_open(0, "", "bmp2Texture, GlGenTextures 1 called");
            //
            GLES20.GlBindTexture(GLES20.GlTexture2d, mTextures[0]);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS,
                    GLES20.GlClampToEdge);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT,
                    GLES20.GlClampToEdge);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter,
                    GLES20.GlNearest);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter,
                    GLES20.GlNearest);
            GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bmptexture, 0);//通过bmp 图像填充这个纹理对象
            //bmptexture.Recycle();
            return mTextures[0];
        }

        //
        public static int get_tex_o_info(int cameraSensorOrientation, SurfaceOrientation m_surfaceOrientation, int previewW, int previewH, ref int angle_z,ref int tex_o_w,ref int tex_o_h)
        {
            bool bGot = false;
            CCtxQyMc pQyMc = g.g_pQyMc;

            //
            if (pQyMc.fake_bFolder)
            {
                if (cameraSensorOrientation == 90)
                {
                    angle_z = 270;
                    //
                    switch (m_surfaceOrientation)
                    {
                        case SurfaceOrientation.Rotation90:
                            angle_z = 180;
                            break;
                        case SurfaceOrientation.Rotation180:
                            angle_z = 90;
                            break;
                        case SurfaceOrientation.Rotation270:
                            angle_z = 0;
                            break;
                        default:
                            break;
                    }

                }

                //
                bGot = true;
            }


            if (!bGot)
            {
                //
                if (cameraSensorOrientation == 90)
                {
                    angle_z = 270;
                    //
                    switch (m_surfaceOrientation)
                    {
                        case SurfaceOrientation.Rotation90:
                            angle_z = 0;
                            break;
                        case SurfaceOrientation.Rotation180:
                            angle_z = 90;
                            break;
                        case SurfaceOrientation.Rotation270:
                            angle_z = 180;
                            break;
                        default:
                            break;
                    }

                }
                else  //  270
                {
                    //
                    angle_z = 90;
                    //
                    switch (m_surfaceOrientation)
                    {
                        case SurfaceOrientation.Rotation90:
                            angle_z = 0;
                            break;
                        case SurfaceOrientation.Rotation180:
                            angle_z = 270;
                            break;
                        case SurfaceOrientation.Rotation270:
                            angle_z = 180;
                            break;
                        default:
                            break;
                    }
                }
            }

            //
            switch (angle_z)
            {
                case 90:
                case 270:
                    tex_o_w = previewH;
                    tex_o_h = previewW;
                    break;
                default:
                    tex_o_w = previewW;
                    tex_o_h = previewH;
                    break;
            }

            return 0;

        }


    }


}