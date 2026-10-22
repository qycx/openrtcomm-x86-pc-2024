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

using qy;

namespace qyMessengerApp.Droid
{
    [Activity(Label = "Activity_notification")]
    public class Activity_notification : BaseActivity
    {
        //
        public static string NOTIFICATION_ID = "NOTIFICATION_ID";
        public static string STR_cmd = "cmd";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
            manager.Cancel(Intent.GetIntExtra(NOTIFICATION_ID, -1));

            //
            int iCmd = Intent.GetIntExtra(STR_cmd, 0);

            //
            GuiShare.pf_gui_notify_doCmd((ushort)iCmd);

            //
            qyFuncs.traceLog("Activity_notification.onCreate, finish");
            //
            Finish();
        }

        //
        public static PendingIntent getDismissIntent(int notificationId, int iCmd, MainActivity context)
        {
            Intent intent = new Intent(context, typeof(Activity_notification));
            //intent.SetFlags(ActivityFlags..NewTask | ActivityFlags.ClearTask);
            intent.PutExtra(NOTIFICATION_ID, notificationId);
            intent.PutExtra(STR_cmd, iCmd);
            PendingIntent dismissIntent = null;

            try
            {
                dismissIntent = PendingIntent.GetActivity(context, iCmd, intent, PendingIntentFlags.CancelCurrent);
            }
            catch (Exception e)
            {
                dismissIntent = null;

            }
            return dismissIntent;


        }
    }
}