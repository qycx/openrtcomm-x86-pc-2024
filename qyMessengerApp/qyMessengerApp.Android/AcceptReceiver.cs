using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using qy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qyMessengerApp.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "com.qycx.ACTION_ACCEPT_DONE" })]
    public class AcceptReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // 在这里处理接收到的消息
            qyFuncs.traceLog("接受视频消息");
            MainActivity.gui_notify_doCmd(Consts.CONST_imOp_recv_accept);
            //获取notificationId
            int notificationId = intent.GetIntExtra("notificationId", -1);
            if (notificationId != -1)
            {
                //获取NotificationManagerCompat
                var notificationManager = NotificationManagerCompat.From(context);
                //取消通知
                notificationManager.Cancel(notificationId);
            }
        }
    }
}