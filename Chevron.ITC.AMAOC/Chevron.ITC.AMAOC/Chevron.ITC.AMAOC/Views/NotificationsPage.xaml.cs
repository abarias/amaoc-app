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
    public partial class NotificationsPage : ContentPage
    {
        NotificationsViewModel vm;
        public NotificationsPage()
        {
            InitializeComponent();
            BindingContext = vm = new NotificationsViewModel();
            ListViewNotifications.ItemTapped += (sender, e) => ListViewNotifications.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (vm.Notifications.Count == 0)
                vm.LoadNotificationsCommand.Execute(false);
        }
    }
}