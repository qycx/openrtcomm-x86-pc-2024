using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	public unsafe struct QNM_DB_CFG
	{

		public fixed char sysName[64 + 1];
		public int iSystemId;                                  //  系统id
		public int iAppType;                                   //  应用类型

		public bool bDefault;
		//  char									dsnName[MAX_QY_DSNNAMELEN + 1];
		public int iDbType;                                    //  2006/05/02
														//
		public int iDbConnType;                                //  2012/07/12. 数据库连接方式
														//  
		public bool ucbDsnless;                                   //  2007/06/13,  采用无DSN的连接方式
			
		 //
		public fixed char dbServIp[Consts.CONST_qyMaxIpLen + 1];
		public bool ucbUseWinAuth;                                //  2014/02/07,  缺省useWinAuth = FALSE;
																  //  WCHAR										dbName[64  +  1];
		public fixed char dbName[Consts.MAX_PATH + 1];                     //  2007/06/13, 对无DNS的方式，此变量存储文件名，故空间要长一些
		public fixed char uid[64 + 1];
		public fixed char pwd[64 + 1];
		public fixed char connectStr[Consts.CONST_maxConnectStrLen + 1];
		public uint ulOpenOptions;
		

	}
	;

	public unsafe struct QY_OBJ_DB
	{
		public int iType;
		//
		public QNM_DB_CFG cfg;
		//
		public int iDsnIndex;                                  //  数据源在procedObjs中的Index，在这里也为惟一的标识数据源，也用来标识在注册表中的序号，在表示添加附加的管理单元时才用，系统本身的主数据库设置为0
														//
		public bool ucbUsed;                                  //  是否被使用
															  //
															  //public void* pDb;
		public object pDb;
		public bool ucbLoaded;                                    //  看是否装载，2006/12/21

		//void* pHashTbl_tree;

	}
	;


	partial class qyFuncs
    {
		public static unsafe bool bObjDbAvail(QY_OBJ_DB pObj)
		{
			//if (null==pObj || !pObj->ucbLoaded) return false;
			if (!pObj.ucbLoaded) return false;
			return true;
		}

	}

}
