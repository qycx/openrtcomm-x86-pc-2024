using Android.App;
using Android.Content;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using qy;


//
namespace qyMessengerApp.Droid
{
    class PreviewCaptureStateCallback : CameraCaptureSession.StateCallback
    {


        public override void OnConfigured(CameraCaptureSession session)
        {
            bool bDbg = false;
            string str;
#if DEBUG
            bDbg = true;
#endif
            //
            if (bDbg)
            {
                str = string.Format("PreviewCaptrueStateCallback.OnConfigured");
                qyFuncs.showInfo_open(0, null, str);
            }


        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {


        }

    }
}