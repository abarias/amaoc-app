using Chevron.ITC.AMAOC.ViewModels;
using FormsToolkit;
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
    public partial class RankingPage : ContentPage
    {
        RankingViewModel vm;
        RankingViewModel ViewModel => vm ?? (vm = BindingContext as RankingViewModel);

        public RankingPage()
        {
            InitializeComponent();

            BindingContext = vm = new RankingViewModel(Navigation);


        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }

        //async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        //{
        //    var item = args.SelectedItem as Event;
        //    if (item == null)
        //        return;

        //    await Navigation.PushAsync(new EventDetailPage(item));

        //    // Manually deselect item
        //    EventsListView.SelectedItem = null;
        //}

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewEventPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RankingListView.ItemTapped += ListViewTapped;

            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Subscribe("filter_changed", (d) => UpdatePage());

            UpdatePage();

        }

        void UpdatePage()
        {

            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow));

            //Load if none, or if 45 minutes has gone by
            if ((ViewModel?.Employees?.Count ?? 0) == 0 || forceRefresh)
            {
                ViewModel?.LoadEmployeesCommand?.Execute(forceRefresh);
            }
        }

        protected override void OnDisappearing()

        {
            base.OnDisappearing();
            RankingListView.ItemTapped -= ListViewTapped;
            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Unsubscribe("filter_changed");
        }

        public void OnResume()
        {
            UpdatePage();
        }
    }
}