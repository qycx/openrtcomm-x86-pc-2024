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
//
using qy.Droid;

//
namespace qyMessengerApp.Droid
{
    class Adapter_msgList : BaseAdapter<TableItem_msg>
    {
        List<TableItem_msg> items;
        LayoutInflater context;


        public Adapter_msgList(LayoutInflater context, List<TableItem_msg> items) : base()
        {
            this.context = context;
            this.items = items;
        }


        public override TableItem_msg this[int position]
        {
            get { return items[position]; }
        }

   
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            var view = convertView;


            string str;

            if (item.uiObjType == Consts.CONST_objType_imGrp)
            {
                view = context.Inflate(Resource.Layout.layout_grpMsgRow, null);

                view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
                
                string displayName = item.grpName;                
                //
                view.FindViewById<TextView>(Resource.Id.Text2).Text = displayName;

                //
                str= qyFuncs_Droid.displayShortTime(item.tRecvTime);
                //
                view.FindViewById<TextView>(Resource.Id.Time).Text = str;

                //
                str = string.Format("{0}：{1}",item.speaker, item.msg);
                //
                view.FindViewById<TextView>(Resource.Id.Text3).Text = str;
            }
            else
            {
                view = context.Inflate(Resource.Layout.layout_contactMsgRow, null);

                view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
                view.FindViewById<TextView>(Resource.Id.Text1).Text = item.dw + " " + item.bm;

                string displayName = item.syr;
                //
                view.FindViewById<TextView>(Resource.Id.Text2).Text = displayName;

                //
                str=  qyFuncs_Droid.displayShortTime(item.tRecvTime);
                view.FindViewById<TextView>(Resource.Id.Time).Text = str;


                //
                str = string.Format("{0}：{1}", item.speaker, item.msg);
                //
                view.FindViewById<TextView>(Resource.Id.Text3).Text = str;

            }


            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

    }

 
}