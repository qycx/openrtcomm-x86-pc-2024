using Android.App;
using Android.Content;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using qy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qyMessengerApp.Droid
{
    class MyCameraStateCallback : CameraDevice.StateCallback
    {




        public override void OnOpened(CameraDevice cameraDevice)
        {
            bool bDbg = false;
            string str;

#if DEBUG
            bDbg = true;
#endif
            //
            if (bDbg)
            {
                str = string.Format("MyCamperaStateCallback.33.OnOpened");
                qyFuncs.showInfo_open(0, null, str);
            }



        }

        public override void OnDisconnected(CameraDevice camera)
        {

        }

        public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
        {
            //throw new NotImplementedException();

        }

    }
}