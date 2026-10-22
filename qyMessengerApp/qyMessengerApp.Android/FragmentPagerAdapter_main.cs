
using AndroidX.Fragment.App;


namespace qyMessengerApp.Droid
{
    class FragmentPagerAdapter_main : FragmentPagerAdapter
    {
        private int PAGER_COUNT = 3;
        //
        public Fragment_msgList mFragment_msg1 = null;
        public Fragment_imObjList mFragment_imObj1 = null;
        public Fragment_me mFragment_me = null;



        public FragmentPagerAdapter_main(AndroidX.Fragment.App.FragmentManager fm) : base(fm)
        {
            mFragment_msg1 = new Fragment_msgList();
            mFragment_imObj1 = new Fragment_imObjList();
            mFragment_me = new Fragment_me();
        }

        public override int Count
        {
            get
            {
                return PAGER_COUNT;
            }
        }


        public override AndroidX.Fragment.App.Fragment GetItem(int position)
        {
            AndroidX.Fragment.App.Fragment fragment = null;

            //
            switch (position)
            {
                case MainActivity.PAGE_ONE:
                    fragment = mFragment_msg1;
                    break;
                case MainActivity.PAGE_TWO:
                    fragment = mFragment_imObj1;
                    break;
                case MainActivity.PAGE_THREE:
                    fragment = mFragment_me;
                    break;

                default:
                    break;

            }
            return fragment;
        }

#if true
        public override long GetItemId(int position)
        {
            return position;
        }
#endif

        public int doClean()
        {
            this.mFragment_msg1 = null;
            this.mFragment_imObj1 = null;
            this.mFragment_me = null;
            //
            return 0;
        }

    }


}