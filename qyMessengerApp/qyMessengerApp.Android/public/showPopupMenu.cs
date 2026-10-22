
using Android.App;
using Android.Content;

using Android.Views;
using Android.Widget;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {

        public delegate int PF_procMenu(Context context, int menuId);




        class Tmp_menu : Java.Lang.Object, PopupMenu.IOnMenuItemClickListener
        {
            Context m_context;
            PF_procMenu m_pf_procMenu;

            public Tmp_menu(Context context, PF_procMenu pf)
            {
                m_context = context;
                m_pf_procMenu = pf;
            }


            public bool OnMenuItemClick(IMenuItem? item)
            {
                Context context = m_context;
                CCtxQyMc pQyMc = g.g_pQyMc;
                //Toast.MakeText(context, item.ItemId, Toast.LENGTH_SHORT).show();
                int ii = item.ItemId;
#if false
                switch (item.ItemId)
                {
                    case Resource.Id.quit:

                        GuiShare.pf_PostMessage_new(pQyMc.gui.hMainWnd, Consts.CONST_qyWndContentType_mainWnd, Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_closeWnd, 0);

                        break;
                    default:
                        break;
                }
#endif
                //
                if (m_pf_procMenu != null)
                {
                    m_pf_procMenu(context, item.ItemId);
                }
                int iii = 0;
                //throw new NotImplementedException();
                return true;
            }
        }


        //
        public static void showPopupMenu(Context context, View view, int menuRes, PF_procMenu pf)
        {
            // View当前PopupMenu显示的相对View的位置
            //Context context = this.Activity;
            PopupMenu popupMenu = new PopupMenu(context, view);
            // menu布局
            popupMenu.MenuInflater.Inflate(menuRes, popupMenu.Menu);
            // menu的item点击事件
            popupMenu.SetOnMenuItemClickListener(new Tmp_menu(context, pf));

            //
            popupMenu.Show();

            return;
        }

    }



}
