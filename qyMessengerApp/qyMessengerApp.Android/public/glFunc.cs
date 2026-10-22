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
        public static void myGLClearError()
        {
            while (GLES20.GlGetError() != GLES20.GlNoError) ;
        }

    }
}