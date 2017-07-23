using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chevron.ITC.AMAOC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPageiOS : ContentPage
    {
        AboutViewModel vm;
        IPushNotifications push;
        public AboutPageiOS()
        {
            InitializeComponent();
            BindingContext = vm = new AboutViewModel();
            push = DependencyService.Get<IPushNotifications>();


            ListViewAccount.HeightRequest = (vm.AccountItems.Count * ListViewAccount.RowHeight);
            ListViewAccount.ItemTapped += (sender, e) => ListViewAccount.SelectedItem = null; 
            isRegistered = push.IsRegistered;
        }

        bool isRegistered;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!isRegistered && Settings.Current.AttemptedPush)
            {
                push.RegisterForNotifications();
            }
            isRegistered = push.IsRegistered;
            vm.UpdateItems();
        }

        public void OnResume()
        {
            OnAppearing();
        }
    }
}