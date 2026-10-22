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
using qyMessengerApp.Android;
namespace qyMessengerApp.Droid
{
    [Activity(Label = "Activity_changePassword")]
    public class Activity_changePassword : qmcBaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_change_passwd);
            // Create your application here
        }
    }
}