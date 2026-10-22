using AndroidX.Fragment.App;



namespace qyMessengerApp.Droid
{
    public class FragmentPagerAdapter_dlgTalk : FragmentPagerAdapter
    {
        private int PAGER_COUNT = 1;//2;
        public Fragment_dlgTalk_msgList mFragment_dlgTalk_msgList1 = null;
        //public Fragment_dlgTalk_detail  mFragment_dlgTalk_detail1 = null;
#if DEBUG
        Fragment_null mFg_test0;
        Fragment_null mFg_test1;

#endif

        public FragmentPagerAdapter_dlgTalk(AndroidX.Fragment.App.FragmentManager fm) : base(fm)
        {
            mFragment_dlgTalk_msgList1 = new Fragment_dlgTalk_msgList();
            //mFragment_dlgTalk_detail1 = new Fragment_dlgTalk_detail();
            //
#if DEBUG
            mFg_test0 = new Fragment_null();
            mFg_test1 = new Fragment_null();

#endif
        }

        public override int Count
        {
            get
            {
                return PAGER_COUNT;
            }

        }

        public override AndroidX.Fragment.App.Fragment  GetItem(int position)
        {
            AndroidX.Fragment.App.Fragment fragment = null;

#if DEBUG
            switch (position)
            {
                case 0:
                    //return this.mFg_test0;//
                    return this.mFragment_dlgTalk_msgList1;
                    break;
#if false
                case 1:
                    return this.mFragment_dlgTalk_detail1;
                    break;
#endif
                default:
                    return null;
                    break;

            }
#endif


            //
            switch (position)
            {
                case Activity_dlgTalk.PAGE_ONE:
                    fragment = mFragment_dlgTalk_msgList1;
                    break;
#if false
                case Activity_dlgTalk.PAGE_TWO:
                    fragment = mFragment_dlgTalk_detail1;
                    break;
#endif
                default:
                    break;
            }
            return fragment;
        }

        public override long GetItemId(int position)
        {
            return position;
        }


    }

}