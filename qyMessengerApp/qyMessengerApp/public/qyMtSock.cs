using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	public unsafe struct MT_SUBTHREADINFO
	{
		public int iServiceId;             //  2007/03/12
		public IntPtr hMutex;
		public object pDb;
		public uint cnt;
		public SOCK_TIMEOUT* pTo;
		public object pParentParam;         //  指向全局或生成此线程的父对象
		public void* pCfg;                 //  指向线程的配置数据区
		public void* pThreadBuf;               //  指向线程的存储数据区
		public void* pTaskBuf;             //  指向一次任务的存储数据区
										   //  void					*			pLogHelp;
		public void* pLogCtx;
		//
		public uint uiBufSize_pMsgBuff;        //  2015/09/30
		public void* pMsgBuff;             //  2015/09/04
	}
	;

 


}
