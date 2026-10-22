using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static unsafe int gui_displayNotification(CCtxQmc pProcInfo, IntPtr pMIS_MSG_taskStatus)
        {
            MIS_MSG_taskStatus* pStatus = (MIS_MSG_taskStatus*)pMIS_MSG_taskStatus;
            //
            //   Toast.MakeText(Application.Context, new string(pStatus->u_notification.content), ToastLength.Long).Show();

            return 0;
        }

    }
}