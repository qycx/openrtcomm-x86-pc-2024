using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using qyMessengerApp.Services;
using qyMessengerApp.Views;

using qy;

namespace qyMessengerApp
{
    public partial class App : Application
    {
        public static bool isLogin { get; set; } //在此控制主界面切换
        public unsafe App()
        {
            InitializeComponent();

            //
            
            qyFuncs.test();

            //
            //qyFuncs_Droid.InitInstance((IntPtr)null, 0);
            
            //
            DependencyService.Register<MockDataStore>();
            //主屏幕显示  按照是否登录确定
            
            if (!isLogin)
            {
                MainPage = new NavigationPage(new QmcLoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new QmcMainPage());
            }
           
        }


		~App()
        {
			int i = 1;
        }

		protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

		///////////////////////
		///
	
		}
	}
