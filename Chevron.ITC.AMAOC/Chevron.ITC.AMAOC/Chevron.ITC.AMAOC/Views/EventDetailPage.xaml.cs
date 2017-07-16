
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Services;
using Chevron.ITC.AMAOC.ViewModels;
using FormsToolkit;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class EventDetailPage : ContentPage
    {        
        EventDetailViewModel ViewModel => vm ?? (vm = BindingContext as EventDetailViewModel);
        EventDetailViewModel vm;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public EventDetailPage()
        {
            InitializeComponent();
        }

        public EventDetailPage(Event ocEvent)
        {
            InitializeComponent();

            ButtonFeedback.Clicked += async (sender, e) =>
            {                
                await NavigationService.PushModalAsync(Navigation, new AMAOCNavigationPage(new EventFeedbackPage(ViewModel.Event)));
            };
            BindingContext = new EventDetailViewModel(Navigation, ocEvent);
            ViewModel.LoadEventCommand.Execute(null);
        }
    }
}
