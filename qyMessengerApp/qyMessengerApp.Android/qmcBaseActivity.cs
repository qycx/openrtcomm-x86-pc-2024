using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using AndroidX.AppCompat.App;
using Java.Lang;
using Org.Aviran.Cookiebar2;
using Pub.Devrel.Easypermissions;
using qy;
using Uri = Android.Net.Uri;


namespace qyMessengerApp.Droid
{
    [Activity(Label = "BaseActivity")]
    public class BaseActivity : AppCompatActivity
    {
        Handler m_timerHandler;
        myTimerRunnable m_timerRunnable;
        int m_delayInMs;
        PF_commonHandler m_pf;

        //
        public string mTag;

        //
        public int m_tn_dbg = 0;

        //       
        class myTimerRunnable : Java.Lang.Object, IRunnable
        {
            BaseActivity mOwner;
            public myTimerRunnable(BaseActivity owner)
            {
                mOwner = owner;
            }

            public void Run()
            {
                //throw new NotImplementedException();

                //
#if DEBUG
                if (mOwner.m_tn_dbg == 999)
                {
                    if (Consts.CONST_Status_Window_Function_Open == 0)
                    {
                        qyFuncs.traceLog("timer999.run starts");


                    }
                }

                //
                //qyFuncs.chkAllTc("timer starts");
#endif

                //
                mOwner.m_pf(null, null, null);

                //
#if DEBUG
                //qyFuncs.chkAllTc("timer ends");
                //
                if (mOwner.m_tn_dbg == 999)
                {
                    if (Consts.CONST_Status_Window_Function_Open == 0)
                    {

                        qyFuncs.traceLog("timer999.run ends");

                    }

                }
#endif

                //
                if (mOwner.m_timerHandler != null)
                {
                    mOwner.m_timerHandler.PostDelayed(mOwner.m_timerRunnable, mOwner.m_delayInMs);
                }
            }
        }



        public int setTimer(int delayInMs, PF_commonHandler pf)
        {
            if (delayInMs == 0) return -1;
            if (pf == null) return -1;

            if (m_timerHandler != null || m_timerRunnable != null) return -1;

            //
            this.m_timerHandler = new Handler();
            this.m_timerRunnable = new myTimerRunnable(this);
            this.m_delayInMs = delayInMs;
            m_pf = pf;

            //
#if DEBUG
            if (m_tn_dbg == 999)
            {
                qyFuncs.traceLog("setTimer....");
            }
#endif

            //
            this.m_timerHandler.PostDelayed(this.m_timerRunnable, delayInMs);

            return 0;
        }

        public void killTimer(string str)
        {
            if (this.m_timerHandler == null && this.m_timerRunnable == null) return;
            //
            this.m_timerHandler.RemoveCallbacks(this.m_timerRunnable);
            this.m_timerRunnable = null;
            this.m_timerHandler = null;

            //
            qyFuncs.showInfo_open(0, str, "killTimer");


            //
            return;
        }




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            qyFuncs.traceLog("BaseActivity  OnDestroy");
            if (m_timerHandler != null)
            {
                //   m_timerHandler.RemoveCallbacksAndMessages(null);
            }
            // Create your application here

        }

        //
        public int doFinish(string hint)
        {
            string str;

            //
            if (hint == null) hint = "";

            str = string.Format("{0}.doFinish, {1}", mTag, hint);
            qyFuncs.showInfo_open(0, null, str);

            //
            Finish();
            return 0;
        }


    }

    //
    public class qmcBaseActivity : BaseActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //
            //  RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
        }

        public static Intent getAppDetailSettingIntent(Activity activity)
        {
            Intent localIntent = new Intent();
            localIntent.AddFlags(ActivityFlags.NewTask);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                localIntent.SetAction("android.settings.APPLICATION_DETAILS_SETTINGS");
                localIntent.SetData(Uri.FromParts("package", activity.PackageName, null));
            }
            else if (Build.VERSION.SdkInt <= BuildVersionCodes.OMr1)
            {
                localIntent.SetAction(Intent.ActionView);
                localIntent.SetClassName("com.android.settings", "com.android.settings.InstalledAppDetails");
                localIntent.PutExtra("com.android.settings.ApplicationPkgName", activity.PackageName);
            }
            return localIntent;
        }

    }



}


