using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qyMessengerApp.Droid
{
    public class MyTextureView:TextureView
    {
		public MyTextureView(Context context)
		: this(context, null)
		{

		}
		public MyTextureView(Context context, IAttributeSet attrs)
			: this(context, attrs, 0)
		{

		}
		public MyTextureView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{

		}

		public void SetAspectRatio(int width, int height)
        {

        }


		//
	}

	//


}