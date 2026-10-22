
using Android.Content;
using Android.Views;
using qy;
using qy.Droid;
using AndroidX.Fragment.App;
using qyMessengerApp.Android;
using Android.Widget;
using System.Collections.Generic;
using Android.OS;
using System;
namespace qyMessengerApp.Droid
{
    public class Fragment_imObjList : AndroidX.Fragment.App.Fragment
    {
        MainActivity mainWnd;
        //List<TableItem_imObj> mTableItems = new List<TableItem_imObj>();
        ListView listView;
        TextView tv_topMenu;
        //
        TextView m_tv_txtTitle;

        //
        //
        public List<TableItem_imObj> mTableItems_imObj = new List<TableItem_imObj>();


        unsafe int th_fillList_imObj(QMem_imObj_intQ* pMem)
        {
            int iErr = -1;
            //QMEM_qyImObj* pMem = (QMEM_qyImObj*)(IntPtr)p2;
            CCtxQyMc pQyMc = g.g_pQyMc;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;

            bool bDbg = false;
            string str;

#if DEBUG
            bDbg = true;

            //
            ref QMem_imObj_intQ tmp_pMem = ref *pMem;
#endif


            //
            MainActivity pMainWnd = (MainActivity)pQyMc.gui.hMainWnd;

            //
            QY_MESSENGER_INFO msgrInfo=new QY_MESSENGER_INFO();
            QY_MESSENGER_REGINFO regInfo=new QY_MESSENGER_REGINFO();

            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en, "", ref pMem->idInfo, 0, ref msgrInfo, ref regInfo))
            {
                goto errLabel;
            }

            //
            TableItem_imObj item = new TableItem_imObj();
            item.ui64Id = msgrInfo.idInfo.ui64Id;
            item.uiObjType = msgrInfo.uiType;

            //
            if (msgrInfo.uiType == Consts.CONST_objType_imGrp)
            {
                IM_GRP_INFO grpInfo = new IM_GRP_INFO();
                if (!pDbFuncs.pf_bGetImGrpInfoBySth(pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, "", ref msgrInfo.idInfo, ref grpInfo))
                {
                    qyFuncs.mymemset((IntPtr)(byte*)&grpInfo, 0, sizeof(IM_GRP_INFO));
                }
                //
                item.grpName = new string(grpInfo.name);

                //
                if (bDbg)
                {
                    str = string.Format("{0}: {1}", msgrInfo.idInfo.ui64Id, item.grpName);
                    qyFuncs.traceLog(str);
                }

                //
                if (grpInfo.idInfo_creator.ui64Id == 0) item.ImageResourceId = Resource.Drawable.grp;
                else item.ImageResourceId = Resource.Drawable.tmpGrp;
            }
            else
            {

                //           
                item.dw = new string(regInfo.dw);
                item.bm = new string(regInfo.bm);
                item.syr = new string(regInfo.syr);

                //
                if (bDbg)
                {
                    str = string.Format("{0}: {1} {2} {3}", msgrInfo.idInfo.ui64Id, item.dw, item.bm, item.syr);
                    qyFuncs.traceLog(str);
                }

                //
                item.ImageResourceId = Resource.Drawable.contact;
            }



            //TableItem
            mTableItems_imObj.Add(item);


            iErr = 0;
        errLabel:

            return iErr;
        }

        //
        public unsafe int fillList_imObj()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //CMyDb pDb = (CMyDb)pQyMc.gui.pDb;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            string str;

            str = string.Format("fillList_imObj enters");
            qyFuncs.showInfo_open(0, null, str);

            //
            //qyFuncs.qTraverse(pDb.m_var_pQ_qyImObjTab, th_fillList_imObj, null, null);
            for (; ; )
            {
                QMem_imObj_intQ mem;
                uint size = (uint)sizeof(QMem_imObj_intQ);
                if (0 != qyFuncs.qGetMsg(pMisCnt.imObj_intQ, (IntPtr)(byte*)&mem, &size)) break;
                //
                th_fillList_imObj(&mem);

            }


        //
        //Adapter_imObjList adapter = (Adapter_imObjList)listView.Adapter;
        //adapter.NotifyDataSetChanged();

        errLabel:

            str = string.Format("fillList_imObj leaves");
            qyFuncs.showInfo_open(0, null, str);

            return 0;
        }


        //
        public int display_imObjList()
        {
            string str;

            //
            str = string.Format("fg_imObjList.display_imObjList enters");
            qyFuncs.showInfo_open(0, null, str);

            //
            if (listView != null)
            {
                Adapter_imObjList adapter = (Adapter_imObjList)listView.Adapter;

                adapter.NotifyDataSetChanged();
            }


            //
            str = string.Format("fg_imObjList.display_imObjList leaves");
            qyFuncs.showInfo_open(0, null, str);


            return 0;
        }


        //
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            mainWnd = (MainActivity)(this.Activity);

            int ii = 0;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            //
            View view = inflater.Inflate(Resource.Layout.layout_fg_imObjList, container, false);

            //
            tv_topMenu = (TextView)view.FindViewById(Resource.Id.txt_topMenu);
            tv_topMenu.Click += Tv_topMenu_Click; ;

            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) goto errLabel;

            //
            m_tv_txtTitle = (TextView)view.FindViewById(Resource.Id.txt_title);
            m_tv_txtTitle.Text = mainWnd.m_str_title;


            //
            listView = (ListView)view.FindViewById(Resource.Id.list);

            /*
            tableItems.Add(new TableItem() { Heading = "Vegetables", SubHeading = "65 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Fruits", SubHeading = "17 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Flower Buds", SubHeading = "5 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Legumes", SubHeading = "33 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Bulbs", SubHeading = "18 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Tubers", SubHeading = "43 items", ImageResourceId = Resource.Drawable.Vegetables });
            */

            listView.Adapter = new Adapter_imObjList(inflater, mTableItems_imObj);

            //
            listView.ItemClick += ListView_ItemClick;

        errLabel:

            return view;

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
        public unsafe int gui_onTimer(object p0, object p1, object p2)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainWnd = (MainActivity)this.Activity;
            if (mainWnd == null) return 0;

            if (pProcInfo.av.talk.bChannelReady)
            {
                m_tv_txtTitle.Text = mainWnd.m_str_title;
            }
            else
            {
                m_tv_txtTitle.Text = mainWnd.m_str_title + "(离线)";

            }

            return 0;
        }



        //
        private void Tv_topMenu_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Context context = this.Activity;
            qyFuncs_Droid.showPopupMenu(context, (View)sender, Resource.Layout.layout_imObjList_topMenu, procMenu);

        }

        private unsafe void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;


            //
            //using (CQySyncObj syncObj = new CQySyncObj())
            {
                //
                //if (0!=syncObj.sync("", pProcInfo.hMutex_sync_mTableItems_imObj)) goto errLabel;


                if (mTableItems_imObj.Count <= e.Position)
                {
                    goto errLabel;
                }
                Console.WriteLine("{0} clicked", mTableItems_imObj[e.Position].ui64Id);

                //
                TableItem_imObj item_sel = mTableItems_imObj[e.Position];

                //

                //
                qyFuncs.traceLog("这里应该提示一下，如果有视频任务在进行，就不能激活新窗口");

                //
                QY_MESSENGER_ID idInfo; idInfo.ui64Id = item_sel.ui64Id;

                //
                MSGR_ADDR addr;
                addr.idInfo.ui64Id = item_sel.ui64Id;
                PARAM_talkToMessenger param = new PARAM_talkToMessenger();

                qyFuncs_Droid.talkToMessenger(pQyMc, &addr, false, true, ref param);

            }


        errLabel:

            return;
        }



    }
}