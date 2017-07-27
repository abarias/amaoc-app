using System;

using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.ViewModels;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;
using FormsToolkit;
using Chevron.ITC.AMAOC.Helpers;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class EventsPage : ContentPage
    {        
        EventsViewModel vm;
        EventsViewModel ViewModel => vm ?? (vm = BindingContext as EventsViewModel);

        public EventsPage()
        {
            InitializeComponent();

            BindingContext = vm = new EventsViewModel(Navigation);

            EventsListView.ItemSelected += async (sender, e) =>
            {
                var ocEvent = EventsListView.SelectedItem as Event;
                if (ocEvent == null)
                    return;

                var eventDetails = new EventDetailPage(ocEvent);

                App.Logger.TrackPage(AppPage.Event.ToString(), ocEvent.Name);
                await NavigationService.PushAsync(Navigation, eventDetails);
                EventsListView.SelectedItem = null;
            };
        }

        void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewEventPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EventsListView.ItemTapped += ListViewTapped;

            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Subscribe("eventstatus_changed", (d) => UpdatePage());

            UpdatePage();

        }

        void UpdatePage()
        {

            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow));
            
            //Load if none, or if 45 minutes has gone by
            if ((ViewModel?.Events?.Count ?? 0) == 0 || forceRefresh || EventsViewModel.ForceRefresh)
            {
                ViewModel?.LoadEventsCommand?.Execute(forceRefresh);
            }
            EventsViewModel.ForceRefresh = false;
        }

        protected override void OnDisappearing()

        {
            base.OnDisappearing();
            EventsListView.ItemTapped -= ListViewTapped;
            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Unsubscribe("eventstatus_changed");
        }

        public void OnResume()
        {
            UpdatePage();
        }

    }
}
