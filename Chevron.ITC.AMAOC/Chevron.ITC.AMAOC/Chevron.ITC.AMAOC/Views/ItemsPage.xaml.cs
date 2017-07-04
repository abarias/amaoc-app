using System;

using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.ViewModels;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class ItemsPage : ContentPage
    {
        EventsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new EventsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Event;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new EventDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Events.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
