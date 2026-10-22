using System;
using System.Collections.Generic;
using System.Text;

//  2015/02/08
partial class Consts
{
	public const int CONST_ctxType_null = 0;
	//
	public const int CONST_ctxType_qmc = 100;
	public const int CONST_ctxType_dvt = 101;
	public const int CONST_ctxType_evt = 102;
	//
	public const int CONST_ctxType_qmd = 150;


}



namespace qy
{


	public unsafe class CCtxQyTmpl
	{

		public CCtxQyTmpl()
		{

		}


		~CCtxQyTmpl()
		{
		}

		
		public int m_iCtxType;
		public string str_who_showInfo;

		//		
		public virtual bool bGetLongProperty(int propId, long* pVal) { return false; }
		public virtual bool bSetLongProperty(int propId, long val) { return false; }
		//
		public virtual bool bGetStrProperty(int propId, char* tBuf, int tBufCnt) { return false; }
		public virtual bool bSetStrProperty(int propId, char * val) { return false; }


		};

	}

