using qy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace qyMessengerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QmcMainPage : TabbedPage
    {
        public QmcMainPage()
        {
            InitializeComponent();
            LoginOut.Text = "退出";
            PageMe.Title = "我";
            PageFriend.Title = "联系人";
            PageMes.Title = "消息";
        }

        async void Login_Out()
        {
            int i = 0;
        
        }

        async private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            qyFuncs.qyMcLogoff();

            // 退出登录按钮
            App.isLogin = false;
            Navigation.InsertPageBefore(new QmcLoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}