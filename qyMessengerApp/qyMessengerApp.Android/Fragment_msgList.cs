
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using qy;
using qy.Droid;
using Java.IO;
using Xamarin.Forms;
using View = Android.Views.View;
using ListView = Android.Widget.ListView;
using AndroidX.Fragment.App;
using System.Threading.Tasks;
using qyMessengerApp.Android;
using IOException = Java.IO.IOException;
namespace qyMessengerApp.Droid
{
    public class Fragment_msgList : AndroidX.Fragment.App.Fragment
    {
        List<TableItem_msg> mTableItems = new List<TableItem_msg>();
        ListView listView;
        TextView tv_topMenu;
        //
        TextView m_tv_txtTitle;

        //

        //
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

            /*
            tableItems.Add(new TableItem() { Heading = "Vegetables", SubHeading = "65 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Fruits", SubHeading = "17 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Flower Buds", SubHeading = "5 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Legumes", SubHeading = "33 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Bulbs", SubHeading = "18 items", ImageResourceId = Resource.Drawable.Vegetables });
            tableItems.Add(new TableItem() { Heading = "Tubers", SubHeading = "43 items", ImageResourceId = Resource.Drawable.Vegetables });
            */
#if false
            mTableItems.Add(new TableItem_msg() { ui64Id=108,dw = "清扬创新", syr = "user9", ImageResourceId = Resource.Drawable.Vegetables, speaker = "张三", msg = "今天快递了吗" });
            mTableItems.Add(new TableItem_msg() { ui64Id = 107, dw = "清扬创新", syr = "user8", ImageResourceId = Resource.Drawable.Vegetables, speaker = "王五", msg = "今天跑步了吗" });
            mTableItems.Add(new TableItem_msg() { ui64Id=111,uiObjType = Consts.CONST_objType_imGrp, grpName = "userGrp", ImageResourceId = Resource.Drawable.Vegetables, speaker = "李四", msg = "任务完成了吗" });
#endif


        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            //
            View view = inflater.Inflate(Resource.Layout.layout_fg_msgList, container, false);

            //
            tv_topMenu = (TextView)view.FindViewById(Resource.Id.txt_topMenu);
            tv_topMenu.Click += Tv_topMenu_Click;

            //
            MainActivity mainWnd = (MainActivity)this.Activity;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) goto errLabel;
            m_tv_txtTitle = (TextView)view.FindViewById(Resource.Id.txt_title);
            m_tv_txtTitle.Text = mainWnd.m_str_title;


            //
            listView = (ListView)view.FindViewById(Resource.Id.list);

            //
            listView.Adapter = new Adapter_msgList(inflater, mTableItems);

            //
            listView.ItemClick += ListView_ItemClick; ;


        errLabel:

            //
            return view;

        }

        int procMenu(Context context, int iMenuCmd)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;

            //
            switch (iMenuCmd)
            {
                case Resource.Id.quit:

                    GuiShare.pf_PostMessage_new(pQyMc.gui.hMainWnd, Consts.CONST_qyWndContentType_mainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);

                    break;
            }
            return 0;
        }


        private void Tv_topMenu_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int ii = 0;
            Context context = this.Activity;

            qyFuncs_Droid.showPopupMenu(context, (View)sender, Resource.Layout.layout_msgList_topMenu, procMenu);
        }


        //
        string fileName_recentContacts = "recentContacts.txt";

        //
        public int saveRecentContacts()
        {
            Context context = qyMessengerApp.Droid.MyApplication.Context;
            string str;
            string fileName = fileName_recentContacts;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (pProcInfo == null) return -1;
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            fileName = fileName_recentContacts + pMisCnt.idInfo.ui64Id;


            //
            try
            {
                OutputStreamWriter outputStreamWriter = new OutputStreamWriter(context.OpenFileOutput(fileName,
                     FileCreationMode.Private));

                foreach (var e in mTableItems)
                {
                    str = string.Format("peer={0}|speaker={1}|t={2}|txt={3}|\n", e.ui64Id, e.speaker, e.tRecvTime, e.msg);
                    outputStreamWriter.Write(str);

                }

                //
                outputStreamWriter.Close();

            }
            catch (Java.IO.FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }


            return 0;
        }

        //
        public async Task<int> loadRecentContacts()
        {

            int iErr = -1;

            Context context = qyMessengerApp.Droid.MyApplication.Context;
            string fileName = fileName_recentContacts;
            string str;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

            //
            fileName = fileName_recentContacts + pMisCnt.idInfo.ui64Id;


            try
            {
                var inputStream = context.OpenFileInput(name: fileName);
                if (inputStream != null)
                {
                    InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
                    BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
                    String temp = "";
                    StringBuilder stringBuilder = new StringBuilder();

                    while ((temp = bufferedReader.ReadLine()) != null)
                    {
                        //stringBuilder.Append(temp);
                        //stringBuilder.Append("\n");
                        QY_MESSENGER_ID idInfo_peer;
                        string speaker;
                        Int64 tSendTime;
                        string txt;


                        //
                        string key;
                        int pos0;
                        int pos1;

                        //
                        key = "peer=";
                        pos0 = temp.IndexOf(key);
                        if (pos0 < 0) goto errLabel;
                        pos1 = temp.IndexOf('|', pos0);
                        if (pos1 < 0) goto errLabel;
                        str = temp.Substring(pos0 + key.Length, pos1 - (pos0 + key.Length));
                        idInfo_peer.ui64Id = (ulong)long.Parse(str);
                        //
                        key = "speaker=";
                        pos0 = temp.IndexOf(key);
                        if (pos0 < 0) goto errLabel;
                        pos1 = temp.IndexOf('|', pos0);
                        if (pos1 < 0) goto errLabel;
                        str = temp.Substring(pos0 + key.Length, pos1 - (pos0 + key.Length));
                        speaker = str;
                        //
                        key = "t=";
                        pos0 = temp.IndexOf(key);
                        if (pos0 < 0) goto errLabel;
                        pos1 = temp.IndexOf('|', pos0);
                        if (pos1 < 0) goto errLabel;
                        str = temp.Substring(pos0 + key.Length, pos1 - (pos0 + key.Length));
                        tSendTime = long.Parse(str);
                        //
                        key = "txt=";
                        pos0 = temp.IndexOf(key);
                        if (pos0 < 0) goto errLabel;
                        pos1 = temp.IndexOf('|', pos0);
                        if (pos1 < 0) goto errLabel;
                        str = temp.Substring(pos0 + key.Length, pos1 - (pos0 + key.Length));
                        txt = (str);

                        //
                        TableItem_msg item = new TableItem_msg();

                        //
                        QY_MESSENGER_ID idInfo_from;
                        idInfo_from.ui64Id = 0;
                        TableItem_msg_fillInfo(idInfo_peer, idInfo_from, ref item);
                        //
                        if (item.uiObjType == 0) continue;
                        //
                        item.speaker = speaker;
                        item.msg = txt;// new string(pContent->html.wBuf);
                        item.tRecvTime = tSendTime;
                        //
                        TableItem_msg item1 = this.mTableItems.Find(x => x.ui64Id == item.ui64Id);
                        if (item1 == null)
                        {
                            mTableItems.Add(item);
                        }

                        //
                        continue;
                    }

                    inputStream.Close();
                }
            }
            catch (Exception e)
            {
                goto errLabel;
            }


            //
            mTableItems.Sort((x, y) => y.tRecvTime.CompareTo(x.tRecvTime));
            //
            qyFuncs.traceLog("异步Notify msg_list adapter");
            Adapter_msgList adapter = (Adapter_msgList)listView.Adapter;
            adapter.NotifyDataSetChanged();




            //
            iErr = 0;

        //
        errLabel:


            return iErr;


        }



        //
        public static unsafe int TableItem_msg_fillInfo(QY_MESSENGER_ID addr_logicalPeer_idInfo, QY_MESSENGER_ID idInfo_from, ref TableItem_msg item)
        {
            int iErr = -1;
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;

            //
            item.ui64Id = addr_logicalPeer_idInfo.ui64Id;
            //
            QY_MESSENGER_INFO messengerInfo=new QY_MESSENGER_INFO();
            QY_MESSENGER_REGINFO regInfo=new QY_MESSENGER_REGINFO();
            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en, "", ref addr_logicalPeer_idInfo,
                0, ref messengerInfo, ref regInfo))
            {
                goto errLabel;
            }
            item.uiObjType = messengerInfo.uiType;
            if (messengerInfo.uiType == Consts.CONST_objType_imGrp)
            {
                IM_GRP_INFO grpInfo = new IM_GRP_INFO(); ;
                if (!pDbFuncs.pf_bGetImGrpInfoBySth(pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, "", ref addr_logicalPeer_idInfo, ref grpInfo))
                {
                    qyFuncs.mymemset((IntPtr)(byte*)&grpInfo, 0, sizeof(IM_GRP_INFO));
                }
                item.grpName = qyFuncs_Droid.get_grpInfo_name(ref grpInfo);// new string(grpInfo.name);
                //
                if (grpInfo.idInfo_creator.ui64Id != 0) item.ImageResourceId = Resource.Drawable.tmpGrp;
                else item.ImageResourceId = Resource.Drawable.grp;
            }
            else
            {
                item.dw = new string(regInfo.dw);
                item.bm = new string(regInfo.bm);
                item.syr = new string(regInfo.syr);
                //
                item.ImageResourceId = Resource.Drawable.contact;

            }
            //
            //
            QY_MESSENGER_ID idInfo;
            idInfo.ui64Id = idInfo_from.ui64Id;
            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en, "", ref idInfo,
                0, ref messengerInfo, ref regInfo))
            {
                goto errLabel;
            }
            item.speaker = new string(regInfo.syr);




            iErr = 0;

        errLabel:

            return iErr;
        }

        //
        public static unsafe string get_syr_by_idInfo(QY_MESSENGER_ID idInfo)
        {
            string str = "";
            CCtxQyMc pQyMc = g.g_pQyMc;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;

            QY_MESSENGER_INFO messengerInfo=new QY_MESSENGER_INFO();
            QY_MESSENGER_REGINFO regInfo=new QY_MESSENGER_REGINFO();
            //QY_MESSENGER_ID idInfo;
            //idInfo.ui64Id = pMsg->data.route.idInfo_from.ui64Id;
            if (!pDbFuncs.pf_bGetMessengerInfoBySth(pDbFuncs, pQyMc.gui.pDb, pQyMc.cfg.db.iDbType, ref qyFuncs.CONST_fieldIdTable_en, "", ref idInfo,
                0, ref messengerInfo, ref regInfo))
            {
                goto errLabel;
            }

            str = new string(regInfo.syr);

        errLabel:
            return str;
        }

        //



        //
        public unsafe int gui_procMsg(ref MIS_MSGU pMsgU)
        {
            int iErr = -1;
            string str;
            bool bDbg = false;

            //
#if DEBUG
            bDbg = true;

            //
            if (bDbg)
            {
                qyFuncs.showInfo_open(0, "", "fg_msgList.gui_procMsg enters. ");
            }
#endif

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            QM_dbFuncs pDbFuncs = pQyMc.p_g_dbFuncs;

            //
            QY_MESSENGER_ID addr_logicalPeer_idInfo; addr_logicalPeer_idInfo.ui64Id = 0;
            QY_MESSENGER_ID idInfo_from; idInfo_from.ui64Id = 0;
            string msg = "";
            Int64 tRecvTime = 0;
 
#if DEBUG
#endif


            //
            if (bDbg)
            {
                if (false)
                {
                    str = string.Format("fg_msglist.gui_procMsg.428");
                    qyFuncs.showInfo_open(0, null, str);
                }
            }

            try
            {

                //
                int lenInBytes_msg = 0;
                //
                switch (pMsgU.uiType)
                {
                    case Consts.CONST_misMsgType_talk:
                        {
                            ref MIS_MSG_TALK pMsg = ref pMsgU.talk;
                            ref IM_CONTENTU pContent = ref pMsg.data.buf;
                            //
                            addr_logicalPeer_idInfo = pMsg.addr_logicalPeer.idInfo;
                            idInfo_from = pMsg.data.route.idInfo_from;
                            tRecvTime = pMsg.tRecvTime;
                            //
                            lenInBytes_msg = (int)((uint)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_TALK>("data") + pMsg.lenInBytes);
                            //
                            switch (pContent.uiType)
                            {
                                case Consts.CONST_imCommType_htmlContent:
                                    {
                                        fixed (char* wBuf = pContent.html.wBuf)
                                        {
                                            msg = new string(wBuf);
                                        }
                                    }
                                    break;
                            }


                        }
                        break;
                    case Consts.CONST_misMsgType_input:
                        {
                            ref MIS_MSG_INPUT pMsg = ref pMsgU.input;
                            ref IM_CONTENTU pContent = ref pMsg.data.buf;
                            //
                            addr_logicalPeer_idInfo = pMsg.addr_logicalPeer.idInfo;
                            idInfo_from = pMsg.data.route.idInfo_from;
                            tRecvTime = pMsg.tRecvTime;
                            //
                            lenInBytes_msg = (int)((uint)System.Runtime.InteropServices.Marshal.OffsetOf<MIS_MSG_INPUT>("data") + pMsg.lenInBytes);
                            //
                            switch (pContent.uiType)
                            {
                                case Consts.CONST_imCommType_transferAvInfo:
                                    {
                                        if (pContent.transferAvInfo.confCfg.idInfo_initiator.ui64Id != 0)
                                        {
                                            idInfo_from.ui64Id = pContent.transferAvInfo.confCfg.idInfo_initiator.ui64Id;
                                        }
                                        //
                                        msg = qyFuncs.getMsgDesc(ref pMsgU);
                                    }
                                    break;
                                case Consts.CONST_imCommType_transferFileReq:
                                    msg = qyFuncs.getMsgDesc(ref pMsgU);
                                    break;
                                default:
                                    break;
                            }

                        }
                        break;
                    case Consts.CONST_misMsgType_taskStatus:
                        {
                            ref MIS_MSG_taskStatus pMsg = ref pMsgU.taskStatus;
                            int index_taskInfo = qyFuncs.getQmcTaskInfoIndexBySth(null, pProcInfo, pMsg.iTaskId);
                            QMC_TASK_INFO pTaskInfo = qyFuncs.getQmcTaskInfoByIndex(null, pProcInfo, index_taskInfo, "");
                            if (pTaskInfo == null)
                            {
                                qyFuncs.showInfo_open(0, "", "fg_msgList.gui_procMsg failed.taskStatus, pTaskInfo is null. iTaskId "  +  pMsg.iTaskId);
                                goto errLabel;
                            }
                            pTaskInfo.var.dwTickCnt_recv_lastRefreshed1 = qyFuncs.myGetTickCount(null);
                            //
                        }
                        iErr = 0; goto errLabel;
                        break;
                    default:
                        //
                        qyFuncs.showInfo_open(0, "", "fg_msgList.gui_procMsg, msg.type err " + pMsgU.uiType);
                        //
                        goto errLabel;
                        break;


                }
                //

                //
                pProcInfo.postImMsg2Log_isClient(ref pMsgU, (int)lenInBytes_msg);

                //
                str = string.Format("fg_msgList.gui_procMsg, recvd {0}", msg);
                qyFuncs.showInfo_open(0, null, str);
                //
                TableItem_msg item = this.mTableItems.Find(x => x.ui64Id == addr_logicalPeer_idInfo.ui64Id);
                if (item == null)
                {
                    int kk = 0;
                    item = new TableItem_msg();
                    //
                    TableItem_msg_fillInfo(addr_logicalPeer_idInfo, idInfo_from, ref item);
                    //
                    item.msg = msg;// new string(pContent->html.wBuf);
                    item.tRecvTime = tRecvTime;
                    //
                    mTableItems.Add(item);
                    //
                    mTableItems.Sort((x, y) => y.tRecvTime.CompareTo(x.tRecvTime));
                    //
                    Adapter_msgList adapter = (Adapter_msgList)listView.Adapter;
                    adapter.NotifyDataSetChanged();


                }
                else
                {
                    //
                    if (item.uiObjType == 0)
                    {
                        TableItem_msg tmp_tableItem_msg = new TableItem_msg();

                        TableItem_msg_fillInfo(addr_logicalPeer_idInfo, idInfo_from, ref tmp_tableItem_msg);

                        //
                        item.uiObjType = tmp_tableItem_msg.uiObjType;
                        item.grpName = tmp_tableItem_msg.grpName;
                        item.dw = tmp_tableItem_msg.dw;
                        item.bm = tmp_tableItem_msg.bm;
                        item.syr = tmp_tableItem_msg.syr;
                        item.ImageResourceId = tmp_tableItem_msg.ImageResourceId;
                    }
                    //
                    item.speaker = get_syr_by_idInfo(idInfo_from);
                    item.msg = msg;// new string(pContent->html.wBuf);
                    item.tRecvTime = tRecvTime;
                    //
                    //
                    mTableItems.Sort((x, y) => y.tRecvTime.CompareTo(x.tRecvTime));
                    //
                    Adapter_msgList adapter = (Adapter_msgList)listView.Adapter;
                    adapter.NotifyDataSetChanged();
                    //
                    int kk = 0;
                }
            }
            catch(Java.Lang.Exception e)
            {
                qyFuncs.showInfo_open(0, "", "fg_msgList.gui_procMsg, exception");
                goto errLabel;
            }


            iErr = 0;

        errLabel:


            //
            if (bDbg)
            {
                str = string.Format("fg_msglist.gui_procMsg leaves, iErr {0}", iErr);
                qyFuncs.showInfo_open(0, null, str);
            }

            //
            return iErr;

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
        private unsafe void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            //Console.WriteLine("{0} clicked", mTableItems[e.Position].ui64Id);
            //
            TableItem_msg item_sel = mTableItems[e.Position];

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;

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

    }
}


