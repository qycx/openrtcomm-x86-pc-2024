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
using qy;
using qy.Droid;


//
namespace qyMessengerApp.Droid
{
    [Activity(Label = "Activity_dlgTalk_detail")]
    public class Activity_dlgTalk_detail : qmcBaseActivity
    {
        //
        public Activity_dlgTalk mOwner_hTalk;

        //
        public Fragment_dlgTalk_msgr_detail m_fg_dlgTalk_msgr_detail;
        public Fragment_dlgTalk_imGrp_detail m_fg_dlgTalk_imGrp_detail;


        //
        //
        public MyHandler mHandler;


        //
        public class MyHandler : Handler
        {
            Activity_dlgTalk_detail mOwner;

            //
            public MyHandler(Activity_dlgTalk_detail owner)
            {
                mOwner = owner;
            }


            public override void HandleMessage(Message msg)
            {
                //base.HandleMessage(msg);

                switch (msg.What)
                {
                    case Consts.CONST_qyWm_postComm:

                        //
                        switch (msg.Arg1)
                        {

                            default:
                                break;
                        }
                        break;
                    default:
                        break;

                }

                return;
            }

            //
        }

        //
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            //
            this.mHandler = new MyHandler(this);

            int iErr = -1;
            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MIS_CNT pMisCnt = pProcInfo.pMisCnt;
            //
            Intent intent = this.Intent;
            UInt64 ui64Id = (UInt64)intent.GetLongExtra("ui64Id", 0);
            //
            Console.WriteLine("ui64Id {0}", ui64Id);
            QY_MESSENGER_ID idInfo; idInfo.ui64Id = ui64Id;
            object hTalk = null;
            if (0 != GuiShare.pf_findTalker(pQyMc, pMisCnt, idInfo, ref hTalk))
            {
                goto errLabel;
            }
            mOwner_hTalk = (Activity_dlgTalk)hTalk;
            DLG_TALK_var pMgrVar = mOwner_hTalk.m_var;
            //




            // Create your application here
            SetContentView(Resource.Layout.layout_activity_dlgTalk_detail);


            //
            if (pMgrVar.addr.uiObjType == Consts.CONST_objType_imGrp)
            {
                m_fg_dlgTalk_imGrp_detail = new Fragment_dlgTalk_imGrp_detail();
            }
            else
            {
                m_fg_dlgTalk_msgr_detail = new Fragment_dlgTalk_msgr_detail();
            }

            //
            //
            //
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //
            if (m_fg_dlgTalk_imGrp_detail != null)
            {
                ft.Replace(Resource.Id.container, m_fg_dlgTalk_imGrp_detail);
            }
            if (m_fg_dlgTalk_msgr_detail != null)
            {
                ft.Replace(Resource.Id.container, m_fg_dlgTalk_msgr_detail);
            }
            //              
            ft.Commit();

            //
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);


            //
            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                doFinish("dlgTalk_av.OnCreate failed");
            }






        }

        protected override void OnDestroy()
        {
            qyFuncs.traceLog("Activity_dlgTalk_detail  OnDestroy");
            if (mHandler != null)
            {
                //   mHandler.RemoveCallbacksAndMessages(null);

            }
            base.OnDestroy();


        }
    }


}