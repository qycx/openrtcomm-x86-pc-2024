using Android.App;
using Android.Content;
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
		//
		public static unsafe int get_pt_far_mats(object p0_reserved, float x_screen, float y_screen, ref QIS_mats pMats, float z_1,out System.Numerics.Vector4 pPt_far)
		{
			int iErr = -1;


			//
			//System.Numerics.Vector4 pt_in;

			//
			//pt_in = new System.Numerics.Vector4(x_screen, y_screen, z_1,0);

			//

			int width = pMats.vp.Width;
			int height = pMats.vp.Height;

			float x = x_screen;
			float y = y_screen;
			float fz = z_1;
			float[] modelView = new float[16];
			MY_MATRIX2matrix(ref pMats.matView, ref modelView);
			float[] projection = new float[16];
			MY_MATRIX2matrix(ref pMats.matProj, ref projection);


			//
			int[] view = new int[] { 0, 0, width, height };
			float[] touch_position = new float[4];
			int r = GLU.GluUnProject(x, view[3] - y,
					fz,//1f - 0.000001f,
					modelView, 0,
					projection, 0,
					view, 0,
					touch_position, 0);

			touch_position[0] /= touch_position[3];
			touch_position[1] /= touch_position[3];
			touch_position[2] /= touch_position[3];
			touch_position[3] /= touch_position[3];

			pPt_far.X = touch_position[0];
			pPt_far.Y = touch_position[1];
			pPt_far.Z = touch_position[2];
			pPt_far.W = touch_position[3];


#if DEBUG

#endif



			iErr = 0;

			errLabel:



			return iErr;

		}


		//
		public static System.Numerics.Vector3 projectTouchToWorld(int width, int height, float fz, float[] modelView, float[] projection,
							float x, float y)
		{
			int[] view = new int[] { 0, 0, width, height };
			float[] touch_position = new float[4];
			int r = GLU.GluUnProject(x, view[3] - y,
					fz,//1f - 0.000001f,
					modelView, 0,
					projection, 0,
					view, 0,
					touch_position, 0);

			touch_position[0] /= touch_position[3];
			touch_position[1] /= touch_position[3];
			touch_position[2] /= touch_position[3];
			touch_position[3] /= touch_position[3];

			System.Numerics.Vector3 rv = new System.Numerics.Vector3(touch_position[0], touch_position[1], touch_position[2]);

			return rv;

		}


		//
		public static int get_pt_screen_mats(object pCtx, ref System.Numerics.Vector4 pPt_far, ref QIS_mats pMats, ref float pfx_screen,ref float pfy_screen)
		{
			int iErr = -1;


			

			return iErr;

		}

		//




	}
}