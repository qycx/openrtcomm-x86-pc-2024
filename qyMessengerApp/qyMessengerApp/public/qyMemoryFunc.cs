using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace qy
{
	public struct QY_MEMORY
	{
		public int iAllocType;
		public uint uiBufSize;             //  2011/12/04
											//  int			iIndex_tmpMemory;		//  2010/09/18
		public IntPtr m_pVirtualData;
		public IntPtr m_pBuf;
	};


	partial class qyFuncs
	{

		public static IntPtr mymalloc(int size)
		{
			IntPtr p;
			int i = 0;

			p = (IntPtr)null;

			//
			try
			{
				//p = malloc(size);
				p = Marshal.AllocHGlobal(size);
			}
			catch (Exception e)
            {
				p = IntPtr.Zero;
				goto errLabel;
            }

			errLabel:

			return p;

		}



		public static void myfree(IntPtr memblock)
		{
			if (memblock != null)
			{
				Marshal.FreeHGlobal(memblock);
				//free(memblock);
			}
		}

		public static unsafe void mymemset(IntPtr p, int val, int size )
        {
			byte* array = (byte *)p;

			
				for (var i = 0; i < size; i++)
				{
					array[i] = (byte)val;
				}
			
		}

		public static unsafe IntPtr mymemcpy(IntPtr destin, IntPtr source, uint n)
        {
			/*
			byte* d = (byte*)destin;
			byte* s = (byte*)source;

			for  ( var i =0; i < n; i ++)
            {
				d[i] = s[i];
            }

			return destin;
			*/
			//static unsafe void CustomCopy(void* dest, void* src, int count)
			if(destin==IntPtr.Zero||source==IntPtr.Zero||n==0 )
            {
				return IntPtr.Zero;
            }

			void* dest = (void*)destin;
			void* src = (void *)source;
			int count = (int)n;
			
			//
				int block;

				block = count >> 3;

				long* pDest = (long*)dest;
				long* pSrc = (long*)src;

				for (int i = 0; i < block; i++)
				{
					*pDest = *pSrc; pDest++; pSrc++;
				}
				dest = pDest;
				src = pSrc;
				count = count - (block << 3);

				if (count > 0)
				{
					byte* pDestB = (byte*)dest;
					byte* pSrcB = (byte*)src;
					for (int i = 0; i < count; i++)
					{
						*pDestB = *pSrcB; pDestB++; pSrcB++;
					}
				}

			return destin;
			
		}

		//
		public static unsafe int mymemcmp( IntPtr ptr1, IntPtr ptr2, int num )
        {
			byte* p1 = (byte*)ptr1;
			byte* p2 = (byte*)ptr2;

			for (var i = 0; i < num; i++)
			{
				if (p1[i] != p2[i]) return -1;
			}

			return 0;
		}

		//
		public static IntPtr mallocMemory(int iAllocType, int size, ref QY_MEMORY p)
		{
			
			if (0==size)
			{
				//OutputDebugString(_T("CQyMalloc::malloc failed, size 0.\n"));
				return (IntPtr)null;
			}
			if (p.m_pBuf != (IntPtr)null || p.m_pVirtualData!=(IntPtr)null)
			{
				//OutputDebugString(_T("CQyMalloc::malloc failed, m_pBuf is allocated already.\n"));
				return (IntPtr)null;
			}

			p.iAllocType = 0;  //  must be reset

# if  __DEBUG__
			if (size > 10000 && size != 2764800)
			{
				//  traceLogA(  "QyMalloc %d",  size  );
			}
#endif

			//  2011/12/04
			p.uiBufSize = (uint)size;

			p.m_pBuf = mymalloc(size); return p.m_pBuf;
		}


		public static void freeMemory(ref QY_MEMORY p)
		{
			//if (p==null) return;

			if (p.iAllocType==0)
			{
				if (p.m_pBuf != (IntPtr)null)
				{
					myfree(p.m_pBuf); p.m_pBuf = (IntPtr)null;
				}
				return;
			}

			//  2010/09/18
			return;

 
		}


	}
}