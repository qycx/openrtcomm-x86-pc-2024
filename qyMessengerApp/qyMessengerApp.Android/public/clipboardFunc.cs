

using Android.App;
using Android.Content;
using qyMessengerApp;


namespace qy.Droid
{

    partial class qyFuncs_Droid
    {


        public static int copyToClipboard(string text)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            Activity activity = (Activity)pQyMc.gui.hMainWnd;

            if (activity == null) return -1;

            ClipboardManager mClipboardManager;
            mClipboardManager = (ClipboardManager)activity.GetSystemService(Context.ClipboardService);


            ClipData mClipData;
            mClipData = ClipData.NewPlainText("test", text);
            mClipboardManager.PrimaryClip=(mClipData);

            return 0;
        }

        public static int pasteFromClipboard()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            Activity activity = (Activity)pQyMc.gui.hMainWnd;

            if (activity == null) return -1;

            ClipboardManager mClipboardManager;
            mClipboardManager = (ClipboardManager)activity.GetSystemService(Context.ClipboardService);

            ClipData clipData = mClipboardManager.PrimaryClip;
            ClipData.Item item = clipData.GetItemAt(0);
            string text = item.Text.ToString();

            return 0;
        }




    }


}


