using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;
using qy;

namespace qyMessengerApp.Views
{
    partial class Contss
    {
        public const int init = 1;
    }

    public partial class AboutPage : ContentPage

    {
        public AboutPage(int test_val = 0)
        {
            InitializeComponent();
            if (test_val == 1)
            {
                A_btn.Text = "已登录";
            }
        }


       async private void Button_Clicked(object sender, EventArgs e)
        {
            //微软队列实现  入队
            int init = Contss.init;
            init++;
            Queue<int> q = new Queue<int>();
            q.Enqueue(init);
            Button_Clicked_1(q);

            //去登录按钮
            //await Navigation.PushAsync(new QmcLoginPage());


        }
        private void Button_Clicked_1(Queue<int> list)
        {
            //出队 
            Console.WriteLine("读出来了一个数据是{0}" , list.Peek());
            Console.WriteLine("队列中还有{0}个", list.Count);

        }
    }
}