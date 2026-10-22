using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {

        public static unsafe int closeInstantAssistant(object hDlgTalk)
        {
            return -1;
        }

        public static unsafe int viewInstantAssistant(object hWndEvent, int iWndContentType, int xPos_screen, int yPos_screen, object hCurTalk, bool bNoDelayToView)
        {
            if (hCurTalk == null) return -1;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)hCurTalk;
            if (dlgTalk.m_hTool_dlgTalk_av == null) return -1;
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)dlgTalk.m_hTool_dlgTalk_av;

            return 0;
        }

    }
}