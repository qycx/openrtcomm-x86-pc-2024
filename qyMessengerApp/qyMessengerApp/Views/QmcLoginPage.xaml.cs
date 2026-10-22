using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using qy;

namespace qyMessengerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QmcLoginPage : ContentPage
    {
        

        public QmcLoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); //标题栏取消
            login_title.Text = "登录";
            server_ip_address_title.Text = "服务器地址：";
            server_ip_address.Placeholder = "请输入服务器地址";
            server_ip_address.Text = "127.0.0.1";
            username_title.Text = "账号：";
            username.Placeholder = "请输入账号";
            username.Text = "user";
            password_title.Text = "密码：";
            password.Placeholder = "请输入密码";
            password.Text = "111";
            login_btn.Text = "登录";
        }

        async void Login_do(object sender, EventArgs e)
        {

            //接值
            string username_val = username.Text;
            string password_val = password.Text;
            string server_ip_adderss_val = server_ip_address.Text;

            Console.WriteLine("账号：{0}" , username_val);
            Console.WriteLine("密码：{0}" , password_val);
            Console.WriteLine("服务器地址：{0}" , server_ip_adderss_val);

            //
             if ( 0==qyFuncs.doMcClientLogonOK(IntPtr.Zero, server_ip_adderss_val, username_val, password_val) )
            //if (username_val == "user" && password_val == "111" && server_ip_adderss_val == "127.0.0.1")
            {



                //
                App.isLogin = true; //记录登录成功
                Navigation.InsertPageBefore(new QmcMainPage() , Navigation.NavigationStack.First());
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("温馨提示", "账号或密码错误", "OK");
                
            }
        }
    }
}
