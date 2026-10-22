using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qyMessengerApp.Droid
{
    class CompareSizesByArea : Java.Lang.Object, IComparator
    {
        public int Compare(Java.Lang.Object lhs, Java.Lang.Object rhs)
        {
            //throw new NotImplementedException();
            var lhsSize = (Size)lhs;
            var rhsSize = (Size)rhs;
            //
            return Long.Signum((long)lhsSize.Width * lhsSize.Height - (long)rhsSize.Width * rhsSize.Height);

        }
    }
}