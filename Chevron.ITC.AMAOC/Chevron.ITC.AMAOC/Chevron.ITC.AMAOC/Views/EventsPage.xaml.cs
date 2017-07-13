using System;

using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.ViewModels;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class EventsPage : ContentPage
    {        
        EventsViewModel vm;
        EventsViewModel viewModel => vm ?? (vm = BindingContext as EventsViewModel);

        public EventsPage()
        {
            InitializeComponent();

            BindingContext = new EventsViewModel(Navigation);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Event;
            if (item == null)
                return;

            await Navigation.PushAsync(new EventDetailPage(item));

            // Manually deselect item
            EventsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewEventPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Events.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
