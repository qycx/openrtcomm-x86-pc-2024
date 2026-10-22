using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using qyMessengerApp.Android;
//
using qy.Droid;


//
namespace qyMessengerApp.Droid
{
    public class Adapter_imObjList : BaseAdapter<TableItem_imObj>
    {
        List<TableItem_imObj> items;
        LayoutInflater context;

        public Adapter_imObjList(LayoutInflater context,List<TableItem_imObj>items):base()
        {
            this.context = context;
            this.items = items;
        }


        public override TableItem_imObj this[int position]
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

            if(item.uiObjType==Consts.CONST_objType_imGrp)
            {
                view = context.Inflate(Resource.Layout.layout_grpRow, null);
                //
                view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
                str = string.Format("{0} {1}", "(分组)", item.ui64Id);
                view.FindViewById<TextView>(Resource.Id.Text1).Text = str;
                string displayName = string.Format("{0}", item.grpName);
                view.FindViewById<TextView>(Resource.Id.Text2).Text = displayName;

            }
            else { 
                view = context.Inflate(Resource.Layout.layout_contactRow, null);
                //
                view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
                str = string.Format("{0} {1} {2}", item.dw, item.bm, item.ui64Id );
                view.FindViewById<TextView>(Resource.Id.Text1).Text = str;
                string displayName = string.Format("{0}",item.syr);
                view.FindViewById<TextView>(Resource.Id.Text2).Text = displayName;

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