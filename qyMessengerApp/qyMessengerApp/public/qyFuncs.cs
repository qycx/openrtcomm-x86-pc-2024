using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Xamarin.Forms.Markup;
using System.Net;

static partial class Consts
{
	public const double Pi = 3.14159;
	public const int SpeedOfLight = 300000; // km per sec.
	public const int CONST_qyMaxIpLen = 15;

}


namespace qy
{
	unsafe struct MyBuffer
	{
		public fixed byte fixedBuffer[128];
		public fixed byte fixedBuffer1[Consts.CONST_qyMaxIpLen];

	}

	




	//
	public class testCls
	{
		int i;
		QWM_DYN_CFG cnt;

		public testCls()
        {
			cnt.cnt.usCntPort = 0;

        }
	
		~testCls()
        {
			int i = 0;
        }
	
	};


	public class MyClass : IDisposable
	{
		private bool IsDisposed = false;

		//
		testCls a;
		//
		public int init()
        {
			a = new testCls();
			return 0;
        }

		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		protected void Dispose(bool Disposing)
		{
			if (!IsDisposed)
			{
				if ( Disposing   )
				{
					//Clean Up managed resources
					a = null;
				}
				//Clean up unmanaged resources  
			}
			IsDisposed = true;
		}
		~MyClass()
		{
			Dispose(false);
		}
	}




	partial class qyFuncs
{
		internal static IntPtr ArrayToIntptr(byte[] source)
		{
			if (source == null)
				return IntPtr.Zero;
			unsafe
			{
				fixed (byte* point = source)
				{
					IntPtr ptr = new IntPtr(point);
					return ptr;
				}
			}
		}

		//
		unsafe struct testChar
		{
			public fixed char buf[128];

		};


		public static string CreateMD5(string input)
		{
			// Use input string to calculate MD5 hash
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				// Convert the byte array to hexadecimal string
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}

		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			return null;
		}


		

		public struct  mm{
			public object jj;
		};

		public static unsafe int test()
		{

            int i = 0;
            int j = 9;

            ref int kk = ref i;
            kk = ref j;

			mm m;
			//m.jj = ref kk;

			 //object bb = m.jj;

			kk = 10;

            return 0;
		}

		public static testCls  get_cls()
        {
			testCls b = new testCls();
			b = null;
			return b;

        }



}
}
