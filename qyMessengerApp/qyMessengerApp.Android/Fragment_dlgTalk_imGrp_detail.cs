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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using qyMessengerApp.Android;
namespace qyMessengerApp.Droid
{
    public class Fragment_dlgTalk_imGrp_detail : Fragment
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
            View view = inflater.Inflate(Resource.Layout.layout_fg_dlgTalk_imGrp_detail, container, false);

            tv_back = (TextView)view.FindViewById(Resource.Id.txt_back);
            tv_back.Click += Tv_back_Click;

            tv_title = (TextView)view.FindViewById(Resource.Id.txt_title);

            Activity_dlgTalk_detail owner = (Activity_dlgTalk_detail)this.Activity;
            Activity_dlgTalk hTalk = owner.mOwner_hTalk;

            //
            tv_title.Text = hTalk.m_str_title;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            QM_dbFuncs g_dbFuncs = pQyMc.p_g_dbFuncs;
            object pDb = pQyMc.gui.pDb;
            int iDbType = pQyMc.cfg.db.iDbType;


            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            //
            TextView tv_staticGreatorIdInfo = (TextView)view.FindViewById(Resource.Id.staticCreatorIdInfo);
            tv_staticGreatorIdInfo.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_staticCreatorIdInfo);


            //
            IM_GRP_INFO grpInfo = new IM_GRP_INFO(); ;
            QY_MESSENGER_ID idInfo = hTalk.m_var.addr.idInfo;

            //  再在qyImGrpInfoTab留下组名
            if (!g_dbFuncs.pf_bGetImGrpInfoBySth(pDb, iDbType, (""), ref idInfo, ref grpInfo))
            {
                qyFuncs.mymemset((IntPtr)(byte*)&grpInfo, 0, sizeof(IM_GRP_INFO));
            }

            //
            TextView tv_idInfo = (TextView)view.FindViewById(Resource.Id.idInfo);
            tv_idInfo.Text = string.Format("{0}", idInfo.ui64Id);

            //
            TextView tv_creatorIdInfo = (TextView)view.FindViewById(Resource.Id.creatorIdInfo);
            tv_creatorIdInfo.Text = string.Format("{0}", grpInfo.idInfo_creator.ui64Id);

            //            
            return view;

        }


        //
        private void Tv_back_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mOwner.OnBackPressed();
        }

    }
}