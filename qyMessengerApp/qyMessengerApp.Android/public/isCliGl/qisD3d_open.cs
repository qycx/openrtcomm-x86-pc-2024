using Android.App;
using Android.Content;
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
	//
	public struct myPOINT
	{
		public int x;
		public int y;
	};

	//
	//  2014/10/26
	public unsafe struct MY_MATRIX
	{
		public fixed float m[16];				 

	};


	//  2013/10/21
	public struct QIS_mats
	{
		public MY_MATRIX matWorld, matView, matProj;
		//
		public struct QIS_mats_vp
        {
			public int Width;
			public int Height;
        };
		public QIS_mats_vp vp;
		//D3D11_VIEWPORT vp;
	}
	;


	partial class qyFuncs_Droid
	{
		public static unsafe int MY_MATRIX2matrix(ref MY_MATRIX p, ref float[] m)
		{
			if (m.Length != 16) return -1;

			int i;
			for (i = 0; i < m.Length; i++)
			{
				m[i] = p.m[i];
			}
			return 0;
		}

		public static unsafe int matrix2MY_MATRIX(ref float[]m,ref MY_MATRIX p)
        {
			if (m.Length != 16) return -1;
			int i;
            for (i = 0; i < m.Length; i++)
            {
				p.m[i] = m[i];
            }

			return 0;
        }

	}

}



