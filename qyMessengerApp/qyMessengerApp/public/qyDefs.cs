using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Runtime.InteropServices;


static partial class Consts
{

    //  下面的值为函数间返回值，跟CONST_qyRc_ok不同。
    //
    public const int CONST_qyCharSpace = 0x20;
    public const int CONST_qyTimeLen = 14;
    public const int CONST_qyMaxVerLen = 8;


    //

    public const int CONST_objType_null = 0;
    public const int CONST_objType_qySwitch = 1;
    public const int CONST_objType_qySwitchPort = 2;
    public const int CONST_objType_col2 = 3;
    public const int CONST_objType_col3 = 4;
    public const int CONST_objType_col4 = 5;
    public const int CONST_objType_unRegGrp = 6;
    public const int CONST_objType_othersGrp = 7;
    public const int CONST_objType_unRegPc = 8;
    public const int CONST_objType_pc = CONST_objType_unRegPc;                      //  2007/06/09£¬±íÃ÷ÊÇ¸öPC
    public const int CONST_objType_otherPc = 9;
    public const int CONST_objType_mac = CONST_objType_otherPc;                     //  2007/06/09, ±íÃ÷Ã»ÓÐÊ²Ã´ÍêÕûµÄPCÐÅÏ¢£¬Ö»±íÃ÷Ò»¸öMAC
    public const int CONST_objType_os = 10;                                         //  PC»ú²Ù×÷ÏµÍ³
    public const int CONST_objType_pcEnv = 11;                                          //  PC»úÈíÓ²¼þ»·¾³
    public const int CONST_objType_cfgSpOs = 12;                                            //  ²¹¶¡ÅäÖÃÀïµÄ²Ù×÷ÏµÍ³, 2005/01/23
    public const int CONST_objType_cfgSpServicePack = 13;                                           //  ²¹¶¡ÅäÖÃÀïµÄServicePack, 2005/01/23
    public const int CONST_objType_zb = 14;
    public const int CONST_objType_bb = 15;
    public const int CONST_objType_dw = CONST_objType_col2;
    public const int CONST_objType_bm = CONST_objType_col3;
    public const int CONST_objType_syr = CONST_objType_col4;
    public const int CONST_objType_pcStatus = 16;                                           //  			2006/08/14
    public const int CONST_objType_pcUsrInfo = 17;                                          //				2006/08/14
    public const int CONST_objType_pcOsClass = 18;                                      //				2006/08/14
    public const int CONST_objType_pcOpNode = 19;                                   //  			2006/08/15
    public const int CONST_objType_procedObj = 20;                                      //  			2007/01/02


    //  2007/11/24
    //  注：网络管理的objType常数最好是另外起一个范围来定义。1000以内最好留给更重要的通用常数。
    //
    public const int CONST_objType_browser = 501;                                       //  devType, browser							
                                                                                        //  public const int		CONST_objType_phone		=					502			;								//  qnmCommProc_open.h
                                                                                        //  public const int		CONST_objType_dev		=					503			;								//  qnmCommProc_open.h
    public const int CONST_objType_toPath = 504;                    //
    public const int CONST_objType_webcam = 505;                                            //  2009/08/28
    public const int CONST_objType_screen = 506;                                            //  2009/08/28
                                                                                            //
    public const int CONST_objType_rtspStream = 507;                                            //  2014/05/02
                                                                                                //
    public const int CONST_objType_avStream = 530;                                          //  2014/09/25. 通用的stream, 非具体指定
                                                                                            //
    public const int CONST_objType_mosaicStream_video = 535;                                        //  2014/10/30
    public const int CONST_objType_mosaicStream_resource = 536;                                         //  2014/10/30

    //
    public const int CONST_objType_proxiedStream_video = 550;                                           //  2014/11/20

    //
    public const int CONST_objType_cosult_mem = 560;                                //  2017/08/30

    //
    public const int CONST_objType_qvcf = 580;                                      //  2015/05/09

    //
    public const int CONST_objType_offlineFile = 600;                                   //  2015/08/03


    //  2007/05/12, mis
    public const int CONST_objType_mis_base = 1000;
    //
    public const int CONST_objType_admin = (CONST_objType_mis_base + 10);           //  2007/08/23, 系统管理员
                                                                                    //
    public const int CONST_objType_me = (CONST_objType_mis_base + 20);
    //
    public const int CONST_objType_messenger = (CONST_objType_mis_base + 21);
    //  
    public const int CONST_objType_webGuest = (CONST_objType_mis_base + 22);            //  2007/11/21, webGuest
                                                                                        //
    public const int CONST_objType_phoneMessenger = (CONST_objType_mis_base + 23);          //  2008/01/30, 用手机注册，但是没有客户端的
    public const int CONST_objType_phoneGuest = (CONST_objType_mis_base + 24);          //  2008/01/07
                                                                                        //
    public const int CONST_objType_newDevGuest = (CONST_objType_mis_base + 50);         //  2008/01/07, newDev


    //																										//  20-100之间留给各种用户对象；100以上留给各类组
    public const int CONST_objType_imGrps = (CONST_objType_mis_base + 100);     //  2007/08/23
    public const int CONST_objType_imGrp = (CONST_objType_mis_base + 101);
    //
    public const int CONST_objType_tmpGrp = (CONST_objType_mis_base + 102);     //  2008/01/22, 临时组。通常在于临时组成一个Q, 然后传递给dlgTalk
    public const int CONST_objType_tmpMsgr = (CONST_objType_mis_base + 103);		//  2018/11/11. 临时对象，可能是messenger,也可能是组


};


namespace qy
{
    public delegate int PF_commonHandler(object p0, object p1, object p2);
    public delegate bool PF_bCommonHandler(object p0, object p1, object p2);        //  TRUE or FALSE
    public delegate IntPtr PF_pCommonHandler(object p0, object p1, object p2);  //  ?011/07/10

    public struct QY_MESSENGER_ID
    {                               //  
        public UInt64                                  ui64Id;

    };


    public class COMMON_PARAM
    {
        public object p0;
        public object p1;
        public object p2;
        //  void	*			p3;
        //  void	*			p4;
    }
    ;



    partial class qyFuncs
    {

        public static unsafe void MACRO_safeStrMemCpy(  string strIn,  byte * mem,  int size  )				
      { 
            byte[] b_strIn = System.Text.Encoding.UTF8.GetBytes(strIn);
            int len = Math.Min((int)(size), b_strIn.Length);
            Marshal.Copy(b_strIn, 0, (IntPtr)mem, len);
            if (len < size) mem[len] = 0;


            //mymemcpy(  (  mem  ),  (  strIn  ), min( strlen(  (  strIn  )  ),  (  size  )  )  );  
        }

        public static void MACRO_makeCommonParam3(  object p0Param,  object p1Param,  object p2Param,  COMMON_PARAM commonParam  )	{
            if (commonParam == null) return;
            commonParam.p0  =  p0Param;  commonParam.p1  =  p1Param;  commonParam.p2  =  p2Param;  
        }


public static void M_free(  ref IntPtr p  )
        {   
            if  (  IntPtr.Zero!=p  )  {  myfree(  (  p  )  );  (  p  )  =  IntPtr.Zero;  }
            return;
        }

    }


}
