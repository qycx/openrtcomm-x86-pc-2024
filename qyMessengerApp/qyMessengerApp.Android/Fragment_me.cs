using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using qy;
using qy.Droid;
using qyMessengerApp.Android;
using AndroidX.Fragment.App;
using AndroidX.AppCompat.App;

namespace qyMessengerApp.Droid
{
    public class Fragment_me : AndroidX.Fragment.App.Fragment
    {
        bool m_bInited;
        //
        TextView tv_topMenu;
        //
        TextView tv_staticUsrName;
        //   TextView tv_dwmc;
        TextView tv_usrName;
        TextView tv_idInfo;
        TextView tv_staticIo;
        TextView tv_io;
        TextView tv_staticServer;
        TextView tv_server;
        //
        TextView tv_staticStatusServer;
        EditText edt_statusServer;
        TextView btn_apply;
        TextView tv_statusQInfo;
        TextView tv_ver;
        TextView tv_staticSupport;
        TextView tv_support;
        EditText pw_edit;
        AlertDialog m_alert;
        //
        bool m_bUpdated_afterLogon;

        //
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override unsafe View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            //
            View view = inflater.Inflate(Resource.Layout.layout_fg_me, container, false);

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            string str;


            //
            tv_topMenu = (TextView)view.FindViewById(Resource.Id.txt_topMenu);
            tv_topMenu.Click += Tv_topMenu_Click; ;

            //
            TextView tv_txtTitle = (TextView)view.FindViewById(Resource.Id.txt_title);
            tv_txtTitle.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_appName);

            //
            //  tv_staticUsrName = (TextView)view.FindViewById(Resource.Id.staticUsrName);
            // tv_dwmc = (TextView)view.FindViewById(Resource.Id.dwmc);
            tv_usrName = (TextView)view.FindViewById(Resource.Id.usrName);
            tv_idInfo = (TextView)view.FindViewById(Resource.Id.idInfo);
            // tv_staticIo = (TextView)view.FindViewById(Resource.Id.staticIo);
            tv_io = (TextView)view.FindViewById(Resource.Id.io);
            //   tv_staticServer = (TextView)view.FindViewById(Resource.Id.staticServer);
            tv_server = (TextView)view.FindViewById(Resource.Id.server);
            //
            //   tv_staticStatusServer = (TextView)view.FindViewById(Resource.Id.staticStatusServer);
            edt_statusServer = (EditText)view.FindViewById(Resource.Id.edt_statusServer);
            btn_apply = (TextView)view.FindViewById(Resource.Id.btn_apply);
            tv_statusQInfo = (TextView)view.FindViewById(Resource.Id.statusQInfo);

            //
            btn_apply.Click += Btn_apply_Click;

            //
            //   tv_staticUsrName.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_staticUsrName);
            //   tv_staticServer.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_staticServer);
            //   tv_staticStatusServer.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_staticStatusServer);
            btn_apply.Text = qyFuncs.getResStr(0, pQyMc.cusRes, (int)enumRes.CONST_resId_apply);
            TextView change_password = (TextView)view.FindViewById(Resource.Id.change_password);
            pw_edit = (EditText)view.FindViewById(Resource.Id.pw_edit);

            change_password.Click += (rander, e) =>
            {

                string new_pw = pw_edit.Text;
                if (string.IsNullOrEmpty(new_pw))
                {
                    Toast.MakeText(Activity, "密码不能为空", ToastLength.Long).Show();
                    return;
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    View view = this.LayoutInflater.Inflate(Resource.Layout.layout_yesCancel, null);
                    builder.SetView(view);
                    builder.SetCancelable(false);
                    m_alert = builder.Create();
                    m_alert.Show();
                    Window wnd = m_alert.Window;
                    wnd.SetGravity(GravityFlags.Center);
                    TextView tvTitle = (TextView)view.FindViewById(Resource.Id.title);
                    tvTitle.Text = "清扬视频";
                    TextView tvContent = (TextView)view.FindViewById(Resource.Id.content);
                    tvContent.Text = "确定要修改密码吗？";
                    Button btnYes = (Button)view.FindViewById(Resource.Id.yes);
                    btnYes.Text = "确定";
                    btnYes.Click += BtnYes_Click;
                    Button btnCancel = (Button)view.FindViewById(Resource.Id.cancel);
                    btnCancel.Text = "取消";
                    btnCancel.Click += BtnCancel_Click;

                }

            };

            //
            // tv_dwmc.Text = "";
            tv_usrName.Text = "";

            //
            string statusServer = "";
            qyFuncs.showInfo_get_cfgServer(ref statusServer);
            edt_statusServer.Text = statusServer;

            tv_statusQInfo.Text = "";

            //
            string verBuf = qyFuncs.qnmVerStr(pQyMc.iServiceId);
            bool bDebug = false;
#if DEBUG
            bDebug = true;
#endif
            int iVer = int.Parse(verBuf);
            str = string.Format("V{0}.{1}.{2}.{3} {4}", iVer / 1000000, (iVer / 10000) % 100, (iVer / 100) % 100, iVer % 100, bDebug ? ("Debug") : (""));

            tv_ver = (TextView)view.FindViewById(Resource.Id.ver);
            tv_ver.Text = str;

            //   tv_staticSupport = (TextView)view.FindViewById(Resource.Id.staticSupport);
            //
            //    tv_staticSupport.Text = "支持网站:";
            //
            //   tv_support = (TextView)view.FindViewById(Resource.Id.support);
            //  tv_support.Text = "www.qycx.com";

            //
            updateCtrls();


            //
            m_bInited = true;


        errLabel:

            //
            return view;

        }
        private unsafe void BtnYes_Click(object sender, EventArgs e)

        {
            m_alert.Dismiss();
            string new_pw = pw_edit.Text;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            string str;
            using (CQyMalloc mallocObj = new CQyMalloc())
            {
                IM_L_STREAM* pStream = (IM_L_STREAM*)mallocObj.mallocf(sizeof(IM_L_STREAM));


                if (pStream == null)
                {
                    qyFuncs.traceLog("sndDevInfo_qmc . malloc failed.");
                }

                QY_MESSENGER_ACCOUNT reqPw;

                TMP_buf256 buf;

                qyFuncs.mymemset((IntPtr)(byte*)&reqPw, 0, sizeof(QY_MESSENGER_ACCOUNT));

                //  memset(&req, 0, sizeof(req));
                reqPw.uiType = Consts.CONST_imCommType_messengerAccount;
                int len;
                //我要做的：

                len = Consts.CONST_maxMessengerPasswdLen + 1;
                qyFuncs.bQyGetStrMd5_x(new_pw, reqPw.messengerPasswd, (uint*)&len);


                qyFuncs.MACRO_memset_imLStream(pStream);
                pStream->uiType = Consts.CONST_imCommType_lStream;
                pStream->uiContentType = (uint)reqPw.uiType;
                len = Consts.bufSize_imLStream + 1;


                if (0 != qyFuncs.messengerAccount2Stream((int)pStream->uiContentType, &reqPw, pStream->buf, (uint*)&len)) ;
                pStream->ulStreamLen = (uint)len;


                len += (int)Marshal.OffsetOf<IM_L_STREAM>("buf");
                Int64 tStartTran; uint uiTranNo;
                qyFuncs.MACRO_prepareForTran(out tStartTran, out uiTranNo);
                MSG_ROUTE msgRoute = new MSG_ROUTE();
                qyFuncs.postMsg2Mgr_mc(null, pMisCnt, ref msgRoute, Consts.CONST_misMsgType_req, 0, Consts.CONST_qyCmd_refreshImObjListReq, tStartTran, uiTranNo, 0, (byte*)pStream, (uint)len, 0, 0, 0, null, false);
            }

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            m_alert.Dismiss();
        }
        //
        int procMenu(Context context, int menuId)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;

            switch (menuId)
            {
                case Resource.Id.quit:

                    GuiShare.pf_PostMessage_new(pQyMc.gui.hMainWnd, Consts.CONST_qyWndContentType_mainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);

                    break;

            }

            return 0;
        }


        //
        private void Tv_topMenu_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Context context = this.Activity;

            qyFuncs_Droid.showPopupMenu(context, (View)sender, Resource.Layout.layout_msgList_topMenu, procMenu);

        }

        //
        unsafe int updateCtrls()
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            string str;

            if (pMisCnt == null) return -1;
            if (pDbFuncs == null) return -1;

            if (pQyMc == null) return -1;

            if (null == pQyMc.gui.pDb)
            {
                qyFuncs.traceLog("pQyMc.gui.pDb  null");
                return -1;
            }


            //
            QY_MESSENGER_INFO msgrInfo=new QY_MESSENGER_INFO();
            QY_MESSENGER_REGINFO regInfo = new QY_MESSENGER_REGINFO(); ;

            QY_MESSENGER_ID idInfo = pMisCnt.idInfo;


            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en, "", ref idInfo, 0, ref msgrInfo, ref regInfo))
            {

                goto errLabel;
            }

            str = string.Format("{0} {1} {2}", new string(regInfo.dw), new string(regInfo.bm), new string(regInfo.syr));
            //tv_dwmc.Text = str;
            tv_usrName.Text = str;

            str = string.Format("{0}", idInfo.ui64Id);
            tv_idInfo.Text = str;

            fixed (byte* pIp = pMisCnt.server.ip)
            {
                str = string.Format("{0}:{1}", qyFuncs.mytoString(pIp), pMisCnt.server.port);
            }
            tv_server.Text = str;
            //
            if (!this.m_bUpdated_afterLogon)
            {
                if (pProcInfo.status.retrieveImObjList.ulbAllContactsRetrieved)
                {
                    this.m_bUpdated_afterLogon = true;
                }

            }


            iErr = 0;
        errLabel:

            return iErr;
        }


        private void Btn_apply_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            string statusServer = edt_statusServer.Text;
            //
            qyFuncs_Droid.saveDbgCfg(statusServer);
            //
            qyFuncs.showInfo_cfgServer(statusServer);

            return;
        }

        //
        public unsafe int gui_onTimer(object p0, object p1, object p2)
        {
            if (!m_bInited) return -1;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            string status = "";

            //
            if (!this.m_bUpdated_afterLogon)
            {
                updateCtrls();
            }
            string strOut = string.Format("↑{0}Kbps", pProcInfo.status.netStat.ins.uiOutSpeedInKbps);
            string strIn = string.Format("↓{0}Kbps", pProcInfo.status.netStat.ins.uiInSpeedInKbps);

            tv_io.Text = strOut + "  " + strIn;

            qyFuncs.showInfo_getStatus(ref status);

            tv_statusQInfo.Text = status;

            return 0;
        }

        //
        public override void OnResume()
        {
            base.OnResume();
            //
            //updateCtrls();

            return;
        }

        //
        public override void OnDestroyView()
        {
            base.OnDestroyView();
        }

        //
        public override void OnDestroy()
        {
            if (this.edt_statusServer != null)
            {
                edt_statusServer.Dispose();
            }
            if (this.btn_apply != null)
            {
                this.btn_apply.Dispose();
            }
            if (this.tv_statusQInfo != null)
            {
                this.tv_statusQInfo.Dispose();
            }

            //
            base.OnDestroy();
            //
            qyFuncs.traceLog("Fragment_me  OnDestroy");
            Dispose();

        }

    }
}


