using System.ComponentModel;
using Xamarin.Forms;
using qyMessengerApp.ViewModels;

namespace qyMessengerApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}