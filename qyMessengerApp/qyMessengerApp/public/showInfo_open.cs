using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
        //
        static string sWho_showInfo = _T("999");

        //
        public static int set_who_showInfo(string name)
        {
            if (null==name) return -1;
            //safeTcsnCpy(name, sWho_showInfo, mycountof(sWho_showInfo));
            sWho_showInfo = name;

            return 0;
        }

        public static int showInfo_open(object p0, string tWhere, string hint)
    {
#if false
            if (tWhere == null) tWhere = "";
            if (hint == null) hint = "";
            
            string who_from = sWho_showInfo;

            if (null==hint) return -1;
            if (null==tWhere) tWhere = _T("");
            
            qyShowInfo1(Consts.CONST_qyShowType_qwmComm, null, (""), who_from, null, tWhere, _T(""), hint);

            //Console.WriteLine("{0}:{1}", tWhere, hint);
#endif
            //
            uint step = 0;
            showInfo_open(p0, ref step, tWhere, hint);

            return 0;
    }

        
        public static int showInfo_open(object p0, ref uint puiStep,  string tWhere, string hint)
        {
            if (tWhere == null) tWhere = "";
            if (hint == null) hint = "";

            string who_from = sWho_showInfo;

            if (null == hint) return -1;
            if (null == tWhere) tWhere = _T("");

#if true //DEBUG
            uint threadId = myGetCurrentThreadId();
            string str = hint + " ." + threadId;
            Console.WriteLine("{0}:{1}", tWhere, str);
#endif

            //
            qyShowInfo1(Consts.CONST_qyShowType_qwmComm, ref puiStep, (""), who_from, null, tWhere, _T(""), hint);

            //
            return 0;
        }





        //
        public static int showState_open(object p0, string tWhere, string hint, ushort usStateType, ushort usStateSubtype,  ref mySYSTEMTIME when, int iPos_toDisplay,int iTaskId)
        {
            //MC_VAR_isCli	*	pProcInfo  =  QY_GET_procInfo_isCli(  );
            //if  (  !pProcInfo  )  return  -1;

            string who_from = sWho_showInfo;

            if (null==hint) return -1;
            if (null==tWhere) tWhere = _T("");

            PARAM_showInfo param = new PARAM_showInfo();
            param.usStateType = usStateType;
            param.usStateSubtype = usStateSubtype;
            param.when = when;
            param.iPos_toDisplay = iPos_toDisplay;
            param.iTaskId = iTaskId;

            uint step = 0;
            qyShowInfo2(ref param, Consts.CONST_qyShowType_qwmState, ref step, (""), who_from, null, tWhere, hint);

            return 0;
        }


    }
}
