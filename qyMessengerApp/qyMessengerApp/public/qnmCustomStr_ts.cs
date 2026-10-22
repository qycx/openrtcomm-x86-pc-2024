using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy
{


    partial class qyFuncs
    {
        public static QY_DMITEM[] CONST_strTable_ts = new QY_DMITEM[23]
 {
        new QY_DMITEM((int)enumRes.CONST_resId_anotherLogonExists, "在另一处登录"  ),// another logon exists"),
        new QY_DMITEM((int) enumRes.CONST_resId_anotherLogon,"另一处登录"),// another logon"),
        //
        new QY_DMITEM((int)enumRes.CONST_resId_appName,"清扬视频通信"),
        //
        new QY_DMITEM((int)enumRes.CONST_resId_allImObjectsRetrieved,"所有用户信息已下载"),  //("All im objects retrieved. ")
        new QY_DMITEM((int)enumRes.CONST_resId_imObjListRetrieved,"用户信息已下载"),  //("ImObjList retrieved")
        //
        new QY_DMITEM((int)enumRes.CONST_resId_staticUsrName,"名称:"),  //
        new QY_DMITEM((int)enumRes.CONST_resId_staticServer,"服务器:"),  //
        new QY_DMITEM((int)enumRes.CONST_resId_staticStatusServer,"状态服务器:"),  //
        new QY_DMITEM((int)enumRes.CONST_resId_apply,"应用"),  //
        new QY_DMITEM((int)enumRes.CONST_resId_staticCreatorIdInfo,"创建者:"  ),

        //
        new QY_DMITEM((int)enumRes.CONST_resId_cancel,"取消"  ),
        new QY_DMITEM((int)enumRes.CONST_resId_cancel_AV,"退出会议"  ),
        new QY_DMITEM((int)enumRes.CONST_resId_requestToSpeak,"发言"  ),
        new QY_DMITEM((int)enumRes.CONST_resId_stopSpeaking,"停止发言"  ),


        new QY_DMITEM((int)enumRes.CONST_resId_changeCamera,"切换镜头"  ),
        new QY_DMITEM((int)enumRes.CONST_resId_localVideo,"本地视频"  ),

        new QY_DMITEM((int)enumRes.CONST_resId_yes,"是"  ),
        new QY_DMITEM((int)enumRes.CONST_resId_accept,"接收"  ),


        new QY_DMITEM((int)enumRes.CONST_resId_waitToQuit,"等待退出..."  ),
        new QY_DMITEM((int)enumRes.CONST_resId_progress,"进度条"  ),

        //
        new QY_DMITEM((int)enumRes.CONST_resId_tooManySpeaks,"发言人数已到上限"), //"already too many speakers. you cant speak now"
        new QY_DMITEM((int)enumRes.CONST_resId_cantTalkToMySelf,"不能和自己对话"), //"already too many speakers. you cant speak now"

        



        new QY_DMITEM(-1,""),
 };

    }
}