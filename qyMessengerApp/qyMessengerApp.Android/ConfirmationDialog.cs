using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Legacy.App;
using qy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceApp= qyMessengerApp.Android.Resource;
using SystemResouce= Android.Resource;
namespace qyMessengerApp.Droid
{
    class ConfirmationDialog : DialogFragment
    {
        private static Fragment mParent;

        class PositiveListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public void OnClick(IDialogInterface dialog, int which)
            {
            }
        }

        class NegativeListener : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            public void OnClick(IDialogInterface dialog, int which)
            {
                Activity activity = mParent.Activity;
                if (activity != null)
                {
                    qyFuncs.traceLog("NegativeListener.OnClick,activity.finish");
                    //
                    activity.Finish();
                }

            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mParent = ParentFragment;
            return new AlertDialog.Builder(Activity)
                .SetMessage(ResourceApp.String.request_permission)
                .SetPositiveButton(SystemResouce.String.Ok, new PositiveListener())
                .SetNegativeButton(SystemResouce.String.Cancel, new NegativeListener())
                .Create();
        }
    }
}