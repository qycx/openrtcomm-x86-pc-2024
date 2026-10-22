using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
    public const int CONST_usRunningStatus_null = 0;
    public const int CONST_usRunningStatus_online = 1;
    public const int CONST_usRunningStatus_offline = 2;
    public const int CONST_usRunningStatus_away = 3;
    public const int CONST_usRunningStatus_hide = 4;                                //  2007/06/07, ÔÚÏß£¬ÀëÏß£¬Àë¿ª£¬ÒþÉíµÈ
    public const int CONST_usRunningStatus_neverLogon = 5;                              //  Î´ÉÏÏß¡£Ö¸´Ó·þÎñÆ÷¿ª»úºóÃ»ÓÐÉÏ¹ýÏß

    //  
    public const int CONST_imTaskType_null = 0;
    //
    public const int CONST_imTaskType_sendFile = 11;
    public const int CONST_imTaskType_recvFile = 12;
    //
    public const int CONST_imTaskType_avSetup = 15;
    //  public const int		CONST_imTaskType_listening							16
    //  public const int		CONST_imTaskType_avCall								17			//  ºóÃæÓ¦¸Ã¸úÒ»¸öÄ¿µÄIP
    //  public const int		CONST_imTaskType_whiteboard							18			//  
    //  public const int		CONST_imTaskType_appCall							19			//  
    //
    public const int CONST_imTaskType_transferAvInfo = 25;          //  
    public const int CONST_imTaskType_shareScreen = 26;         //  2008/10/06
    public const int CONST_imTaskType_remoteAssist = 27;            //  2008/11/10
    public const int CONST_imTaskType_shareMediaFile = 28;          //  2009/04/23
    public const int CONST_imTaskType_shareDynBmp = 29;         //  2009/08/21
    public const int CONST_imTaskType_viewDynBmp = 30;          //  2009/09/07
    public const int CONST_imTaskType_queryCustomerServiceOfficer = 31;         //  2011/04/06

    //  2012/04/19
    public const int CONST_imTaskType_shareGps = 40;            //  2012/04/19
    public const int CONST_imTaskType_viewGps = 41;         //  2012/04/19

    //
    //
    public const int CONST_policyAvLevel_240p = 10;         //  2014/04/23
                                                            //
    public const int CONST_policyAvLevel_480p = 21;
    public const int CONST_policyAvLevel_848x480 = 22;
    public const int CONST_policyAvLevel_576p = 23;

    //
    public const int CONST_policyAvLevel_720p = 32;         //  2014/04/23
                                                            //
    public const int CONST_policyAvLevel_1080p = 42;            //  2014/04/23

    //
    public const int CONST_policyAvLevel_1080p60 = 45;          //  2023/12/31


    //
    public const int CONST_policyAvLevel_2160p = 52;			//  2023/12/31

    //
    public const int CONST_policyAvLevel_2160p60 = 55;			//  2023/12/31


    //  2012/02/14
    public const int CONST_policyAvLevel_dev_2fps = 110;            //  2fps, dmo.		screen
    public const int CONST_policyAvLevel_dev_5fps = 112;            //  5fps-300k, ipp. screen
                                                                    //  2012/10/29
    public const int CONST_policyAvLevel_dev_5fps1 = 113;           //  5fps-450k, ipp. screen
                                                                    //  2015/11/06
    public const int CONST_policyAvLevel_dev_5fps2 = 114;			//  5fps-700k, screen
    //  2012/11/3



}


namespace qy
{
    class qyCusResPublic
{
}
}
