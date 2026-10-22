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

namespace qyMessengerApp.Droid
{

    [Service]
    public class CancelIntentService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // 插入我们要在Service中完成的代码
            Intent broadcastIntent = new Intent();
            broadcastIntent.SetAction("com.qycx.ACTION_CANCEL_DONE");
            SendBroadcast(broadcastIntent);
            return StartCommandResult.Sticky;
        }
    }
}