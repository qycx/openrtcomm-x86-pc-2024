using System;
using System.Collections.Generic;
using qyMessengerApp.Android;
using Android.App;
using Manifest=Android.Manifest;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Pub.Devrel.Easypermissions;
using qy;
using qy.Droid;




//
namespace qyMessengerApp.Droid
{
    [Activity(Label = "Activity_qmcLogin")]
    public class Activity_qmcLogin : qmcBaseActivity, View.IOnClickListener, EasyPermissions.IPermissionCallbacks
    {
        private static int REQUEST_CODE_STORAGE_PERMISSION = 10;
        //

        private static int RC_CAMERA_PERM = 123;
        EditText edit_server;
        EditText edit_usrName;
        EditText edit_passwd;
        TextView btn_login;
        TextView change_passwd;
        LinearLayout linear_layout;
        Context mContext;
        AppSettingsDialog appSettingsDialog;
        //
        AlertDialog m_alert = null;
        String[] mPermissionList = new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.Vibrate, Manifest.Permission.ReceiveBootCompleted, Manifest.Permission.WakeLock, Manifest.Permission.AccessNotificationPolicy, Manifest.Permission.RequestIgnoreBatteryOptimizations };
        ISharedPreferences sp;

        //
        private void isPermissionOk()
        {


            if (hasStoragePermission())
            {
                startLogin();

            }
            else
            {
                // Ask for one permission
                EasyPermissions.RequestPermissions(
                        this,
                        Resources.GetString(Resource.String.permission_content_storage_first),
                        REQUEST_CODE_STORAGE_PERMISSION,
                        mPermissionList);
            }



        }
        private bool hasStoragePermission()
        {
            return EasyPermissions.HasPermissions(this, mPermissionList);
        }


        //授权结果，分发下去
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            // Forward results to EasyPermissions
            //跳转到onPermissionsGranted或者onPermissionsDenied去回调授权结果
            int[] gr = new int[grantResults.Length];
            for (int i = 0; i < gr.Length; i++)
            {
                gr[i] = (int)grantResults[i];
            }
            //
            EasyPermissions.OnRequestPermissionsResult(requestCode, permissions, gr, this);
        }

        public void OnPermissionsGranted(int p0, IList<string> p1)
        {
            startLogin();


        }

        //拒绝授权
        public void OnPermissionsDenied(int p0, IList<string> p1)
        {
            //Log.i(TAG, "onPermissionsDenied:" + requestCode + ":" + perms.size());
            //    CCtxQyMc pQyMc = g.g_pQyMc;

            //   GuiShare.pf_PostMessage(pQyMc.gui.hMainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_login_canceled, 0);

            //
            if (EasyPermissions.SomePermissionPermanentlyDenied(this, p1))
            {
                AppSettingsDialog.Builder buider = new AppSettingsDialog.Builder(this);
                buider.SetRationale(Resources.GetString(Resource.String.permission_content_storage));
                appSettingsDialog = buider.Build();

                appSettingsDialog.Show();
            }


        }

        //
        protected override void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.layout_qmcLogin);
            sp = GetSharedPreferences("SP", FileCreationMode.Private);
            //
            qyFuncs.showInfo_open(0, null, "qmcLogin: onCreate called");

            //
            mContext = this;

            edit_server = (EditText)FindViewById(Resource.Id.editText_server);
            edit_usrName = (EditText)FindViewById(Resource.Id.editText_usrName);
            edit_passwd = (EditText)FindViewById(Resource.Id.editText_passwd);
            btn_login = (TextView)FindViewById(Resource.Id.button_login);
            change_passwd = (TextView)FindViewById(Resource.Id.change_passwd);
            change_passwd.SetOnClickListener(this);
            if (btn_login != null)
            {
                btn_login.SetOnClickListener(this);
            }

            //
            string str_server = "", str_name = "", str_passwd = "";
            // qyFuncs_Droid.readLoginInfo(ref str_server, ref str_name, ref str_passwd);
            str_server = sp.GetString("str_server", "218.240.128.210");
            str_name = sp.GetString("str_name", "zs1");
            str_passwd = sp.GetString("str_passwd", "123");
            edit_server.Text = str_server;
            edit_usrName.Text = str_name;
            edit_passwd.Text = str_passwd;

            return;
        }


        //
        protected override void OnDestroy()
        {
            if (m_alert != null)
            {
                m_alert.Dismiss();
            }
            //
            edit_server.Text = "";
            edit_server.Dispose();
            edit_usrName.Text = "";
            edit_usrName.Dispose();
            edit_passwd.Text = "";
            edit_passwd.Dispose();
            //
            btn_login.Dispose();
            //
            base.OnDestroy();
            qyFuncs.traceLog("Activity_qmcLogin  OnDestroy");
            //
            //  qyFuncs_Droid.doGC("qmcLogin.OnDestroy");

        }




        public void OnClick(View? v)
        {
            string str;

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null)
            {
                qyFuncs.traceLog("qmcLogin.onClick err:pQyMc is null");
                return;
            }

            //
            switch (v.Id)
            {
                case Resource.Id.button_login:
                    qyFuncs.traceLog("登录按钮被点击");
                    isPermissionOk();


                    break;
                case Resource.Id.button_close:
                    if (m_alert != null)
                    {
                        m_alert.Dismiss();
                        m_alert = null;
                    }
                    break;
                case Resource.Id.change_passwd:

                    Intent intent = new Intent(this, typeof(Activity_changePassword));

                    intent.PutExtra("params", "");
                    StartActivity(intent);
                    break;
                default:
                    break;
            }
        }
        private void startLogin()
        {
            string str;

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null)
            {
                qyFuncs.traceLog("qmcLogin.onClick err:pQyMc is null");
                return;
            }
            string str_server = edit_server.Text;
            string str_usrName = edit_usrName.Text;
            string str_passwd = edit_passwd.Text;
            ISharedPreferencesEditor edit = sp.Edit();
            edit.PutString("str_server", str_server);
            edit.PutString("str_name", str_usrName);
            edit.PutString("str_passwd", str_passwd);
            edit.Apply();

            //
            qyFuncs.traceLog("qmcLogin.onClick, btn_login");

            //
            if (pQyMc.bLogon) return;


            if (0 != qy.qyFuncs.doMcClientLogonOK(IntPtr.Zero, str_server, str_usrName, str_passwd))
            {
                displayHint(this, "登录失败", "服务器地址错，或者用户名密码错");
                return;
            }
            //
            str = string.Format("Activity_qmcLogin: logon ok");
            qyFuncs.showInfo_open(0, null, str);

            qyFuncs_Droid.saveLoginInfo(str_server, str_usrName, str_passwd);
            doFinish("after isPermissionOk");
        }

        public void displayHint(View.IOnClickListener lsn, string title, string hint)
        {
            AlertDialog.Builder builder = null;

            //
            if (m_alert != null) return;

            //
            builder = new AlertDialog.Builder(mContext);

            LayoutInflater inflater = (LayoutInflater)mContext.GetSystemService(Context.LayoutInflaterService);
            View view_custom = inflater.Inflate(Resource.Layout.layout_alert1, null, false);
            builder.SetView(view_custom);
            builder.SetCancelable(false);
            m_alert = builder.Create();
            Window wnd = m_alert.Window;
            wnd.SetGravity(GravityFlags.Center | GravityFlags.Bottom);
            //this.WindowManager.LayoutParams lp = wnd.Attributes;

            TextView txt_title = (TextView)view_custom.FindViewById(Resource.Id.textView_title);
            if (txt_title != null)
            {
                txt_title.Text = title;
            }

            TextView txt_hint = (TextView)view_custom.FindViewById(Resource.Id.textView_hint);
            if (txt_hint != null)
            {
                txt_hint.Text = hint;
            }

            //
            Button btn_close = (Button)view_custom.FindViewById(Resource.Id.button_close);
            if (btn_close != null) btn_close.SetOnClickListener(lsn);




            //    
            m_alert.Show();


        }

        public override void OnBackPressed()
        {
            int i = 0;
            CCtxQyMc pQyMc = g.g_pQyMc;

            qyFuncs.traceLog("qmcLogin. OnBackPressed");

            //
            base.OnBackPressed();
            //
            return;
        }


    }

}
