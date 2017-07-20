using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Services;
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
    public partial class FeedPage : ContentPage
    {
        FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);
        FeedViewModel vm;
               
        public FeedPage()
        {
            InitializeComponent();            
            BindingContext = new FeedViewModel();           

            ViewModel.Events.CollectionChanged += (sender, e) =>
            {
                var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Events.Count + 1;
                ListViewEvents.HeightRequest = (ViewModel.Events.Count * ListViewEvents.RowHeight) - adjust;
            };

            ListViewEvents.ItemTapped += (sender, e) => ListViewEvents.SelectedItem = null;
            ListViewEvents.ItemSelected += async (sender, e) =>
            {
                var ocEvent = ListViewEvents.SelectedItem as Event;
                if (ocEvent == null)
                    return;
                var eventDetails = new EventDetailPage(ocEvent);

                App.Logger.TrackPage(AppPage.Event.ToString(), ocEvent.Name);
                await NavigationService.PushAsync(Navigation, eventDetails);
                ListViewEvents.SelectedItem = null;
            };

            NotificationStack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    App.Logger.TrackPage(AppPage.Notification.ToString());
                    await NavigationService.PushAsync(Navigation, new NotificationsPage());
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdatePage();       

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();            
        }

        
        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow));
            
            if (forceRefresh)
            {
                ViewModel.RefreshCommand.Execute(null);                
            }
            else
            {
                if (ViewModel.Events.Count == 0)
                {                    
                    ViewModel.LoadEventsCommand.Execute(null);
                }

                if (ViewModel.Notification == null)
                    ViewModel.LoadNotificationsCommand.Execute(null);
            }

        }


        public void OnResume()
        {
            UpdatePage();
        }

    }
}