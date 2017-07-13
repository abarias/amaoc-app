
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.ViewModels;

using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class EventDetailPage : ContentPage
    {
        EventDetailViewModel vm;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public EventDetailPage()
        {
            InitializeComponent();
        }

        public EventDetailPage(Event ocEvent)
        {
            InitializeComponent();

            BindingContext = vm = new EventDetailViewModel(ocEvent);
        }
    }
}
