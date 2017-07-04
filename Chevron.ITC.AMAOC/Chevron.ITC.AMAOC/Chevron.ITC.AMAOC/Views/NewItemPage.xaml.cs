using System;

using Chevron.ITC.AMAOC.Models;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Event Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Event
            {
                Name = "Item name",
                Abstract = "This is a nice description"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}