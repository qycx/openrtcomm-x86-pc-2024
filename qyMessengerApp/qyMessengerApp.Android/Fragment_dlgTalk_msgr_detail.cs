using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using qy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using qyMessengerApp.Android;
namespace qyMessengerApp.Droid
{
    public class Fragment_dlgTalk_msgr_detail : Fragment
    {
        Activity_dlgTalk_detail mOwner;
        //
        TextView tv_back;
        TextView tv_title;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            mOwner = (Activity_dlgTalk_detail)this.Activity;

        }

        public override unsafe View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            //
            View view = inflater.Inflate(Resource.Layout.layout_fg_dlgTalk_msgr_detail, container, false);

            tv_back = (TextView)view.FindViewById(Resource.Id.txt_back);
            tv_back.Click += Tv_back_Click;

            tv_title = (TextView)view.FindViewById(Resource.Id.txt_title);

            Activity_dlgTalk_detail owner = (Activity_dlgTalk_detail)this.Activity;
            Activity_dlgTalk hTalk = owner.mOwner_hTalk;

            tv_title.Text = hTalk.m_str_title;


            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            string str;

            //
            TextView tv_staticUsrName = (TextView)view.FindViewById(Resource.Id.staticUsrName);
            tv_staticUsrName.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_staticUsrName);

            //
            QY_MESSENGER_INFO msgrInfo = new QY_MESSENGER_INFO(); ;
            QY_MESSENGER_REGINFO regInfo = new QY_MESSENGER_REGINFO(); ;

            QY_MESSENGER_ID idInfo = hTalk.m_var.addr.idInfo;
            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en, "", ref idInfo, 0, ref msgrInfo, ref regInfo))
            {
                qyFuncs.mymemset((IntPtr)(byte*)&regInfo, 0, sizeof(QY_MESSENGER_REGINFO));
            }

            TextView tv_dwmc = (TextView)view.FindViewById(Resource.Id.dwmc);
            TextView tv_usrName = (TextView)view.FindViewById(Resource.Id.usrName);
            TextView tv_idInfo = (TextView)view.FindViewById(Resource.Id.idInfo);

            //
            str = string.Format("{0} {1}", new string(regInfo.dw), new string(regInfo.bm));
            tv_dwmc.Text = str;

            tv_usrName.Text = new string(regInfo.syr);

            str = string.Format("{0}", idInfo.ui64Id);
            tv_idInfo.Text = str;



            //
            return view;

        }

        private void Tv_back_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mOwner.OnBackPressed();
        }
    }
}