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

namespace qy.Droid
{
    public class TableItem_imObj
    {
        //
        public UInt64 ui64Id;

        //
        public uint uiObjType;

        public int ImageResourceId { get; set; }

        public string dw { get; set; }
        public string bm { get; set; }
        public string syr { get; set; }

        //
        public string grpName { get; set; }

        //
        public TableItem_imObj()
        {
            ui64Id = 0;
            uiObjType = 0;
            ImageResourceId = 0;
            dw = "";
            bm = "";
            syr = "";
            grpName = "";
        }


    }

    public class TableItem_msg
    {
        public UInt64 ui64Id;
        
        //
        public uint uiObjType;

        //
        public int ImageResourceId { get; set; }

        public string dw { get; set; }
        public string bm { get; set; }
        public string syr { get; set; }
        public string grpName { get; set; }

        //
        public string speaker { get; set; }
        public string msg { get; set; }
        public Int64 tRecvTime { get; set; }

        //
        public TableItem_msg()
        {
            ui64Id = 0;
            uiObjType = 0;
            ImageResourceId = 0;
            dw = "";
            bm = "";
            syr = "";
            grpName = "";
            //
            speaker = "";
            msg = "";
            tRecvTime = 0;

        }


    }


    public class TableItem_dlgTalk_msg
    {
        // speaker
        public UInt64 ui64Id_from;
                
        //
        public int ImageResourceId { get; set; }

        public string dw { get; set; }
        public string bm { get; set; }
        public string syr { get; set; }
        public string msg { get; set; }
        public Int64 tStartTime { get; set; }
        public uint uiTranNo { get; set; }
        //
        public uint uiMsgType { get; set; }
        public ushort usCode { get; set; }
        public uint uiContentType { get; set; }
        //
        public uint uiTaskType { get; set; }
        public int iTaskId { get; set; }
        public string statusStr { get; set; }

        //
        public TableItem_dlgTalk_msg()
        {
            ui64Id_from = 0;
            ImageResourceId = 0;
            dw = "";
            bm = "";
            syr = "";
            //
            msg = "";
            tStartTime = 0;
            uiTranNo = 0;
            //
            uiMsgType = 0;
            usCode = 0;
            uiContentType = 0;
            //
            uiTaskType = 0;
            iTaskId = 0;
            //iStatus = 0;
            statusStr = "";


        }


    }



}

