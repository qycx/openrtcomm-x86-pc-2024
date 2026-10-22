
//#define  __USE_leak__

using qyMessengerApp.Android;
using Res = Android.Content.Res;
using Content=Android.Content;
using qy;
using Android.Media;

using Uri = Android.Net.Uri;
using ME.Jessyan.Autosize;
using ME.Jessyan.Autosize.Utils;

using Android.Graphics;
using AndroidX.Lifecycle;
using Android.Runtime;
using Android.App;
using System;


#if __USE_leak__
using Square.LeakCanary;
#endif

//
namespace qyMessengerApp.Droid
{
    [Application]
    public class MyApplication : Application, ILifecycleObserver, IOnAdaptListener
    {
#if __USE_leak__
        private RefWatcher _refWatcher;
#endif
        public Ringtone r;

        public NotificationManager mNotificationManager;
        Uri notification;

        public MediaPlayer mediaPlayer;
        public MyApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            mediaPlayer = MediaPlayer.Create(this, Resource.Raw.receive);
            mediaPlayer.Looping = true;



            /*
                        Uri uri = Uri.Parse("android.resource://com.qycx.qymessenger/" + Resource.Raw.receive);
                        RingtoneManager.SetActualDefaultRingtoneUri(this, RingtoneType.Ringtone, uri);

                        notification = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
                        r = RingtoneManager.GetRingtone(ApplicationContext, notification);
                        r.Looping = true;*/
            AutoSizeConfig.Instance.SetOnAdaptListener(this);

            //
            bool bNoLeakCannary = false;
            bNoLeakCannary = true;
            if (!bNoLeakCannary)
            {
                SetupLeakCanary();
            }

            //
            var lifecycleOwner = ProcessLifecycleOwner.Get();
            var lifecycle = lifecycleOwner.Lifecycle;
            lifecycle.AddObserver(this);
            initNotification();

        }

        private void initNotification()
        {

            AudioAttributes audioAttributes = new AudioAttributes.Builder()
            .SetContentType(AudioContentType.Sonification)
            .SetUsage(AudioUsageKind.Notification)
            .Build();
            //首先，我们需要创建一个NotificationChannel实例:
            NotificationChannel mChannel = new NotificationChannel("清扬通信", "视频会议通知", NotificationImportance.High);

            //然后，我们可以设置描述，灯光颜色，振动模式等属性：
            mChannel.Description = "清扬视频视频会议通知";
            // Enable lights and vibration.
            mChannel.EnableLights(true);
            mChannel.LightColor = Color.Red;
            mChannel.EnableVibration(true);
            mChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });
            mChannel.LockscreenVisibility = NotificationVisibility.Public;
            mChannel.SetSound(notification, audioAttributes);
            // 最后，我们必须将通道添加到NotificationManager：
            mNotificationManager = (NotificationManager)GetSystemService(Content.Context.NotificationService);
            mNotificationManager.CreateNotificationChannel(mChannel);
        }

        protected void SetupLeakCanary()
        {
#if __USE_leak__
            if (LeakCanaryXamarin.IsInAnalyzerProcess(this))
            {
                return;
            }
            _refWatcher = LeakCanaryXamarin.Install(this);
#endif
        }



        [Lifecycle.Event.OnStop]
        [Java.Interop.Export]
        public void onAppBackgrounded()
        {
            Console.WriteLine("background");
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc != null)
            {
                pQyMc.gui.bForeground = false;
            }
        }

        [Lifecycle.Event.OnStart]
        [Java.Interop.Export]
        public void onAppForegrounded()
        {
            Console.WriteLine("foreground");
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc != null)
            {
                pQyMc.gui.bForeground = true;
            }
        }

        public void OnAdaptAfter(Java.Lang.Object p0, Activity p1)
        {
        }

        public void OnAdaptBefore(Java.Lang.Object p0, Activity activity)
        {
            AutoSizeConfig.Instance.SetScreenWidth(ScreenUtils.GetScreenSize(activity)[0]);
            AutoSizeConfig.Instance.SetScreenHeight(ScreenUtils.GetScreenSize(activity)[1]);
        
            if (activity.Resources.Configuration.Orientation == Res.Orientation.Landscape)
            {
                AutoSizeConfig.Instance.SetDesignWidthInDp(1100);
                AutoSizeConfig.Instance.SetDesignHeightInDp(540);
            }
            else
            {
                AutoSizeConfig.Instance.SetDesignWidthInDp(440);
                AutoSizeConfig.Instance.SetDesignHeightInDp(960);

            }
        }
    }
}