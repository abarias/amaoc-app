using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;
using Chevron.ITC.AMAOC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chevron.ITC.AMAOC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventCell : ViewCell
    {
        readonly INavigation navigation;
        public EventCell(INavigation navigation = null)
        {
            Height = 120;
            View = new EventCellView();
            this.navigation = navigation;

        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (navigation == null)
                return;
            var ocEvent = BindingContext as Event;
            if (ocEvent == null)
                return;

            App.Logger.TrackPage(AppPage.Event.ToString(), ocEvent.Name);
            await NavigationService.PushAsync(navigation, new EventDetailPage(ocEvent));
        }
    }

    public partial class EventCellView : ContentView
    {
        public EventCellView()
        {
            InitializeComponent();            
        }        
    }
}