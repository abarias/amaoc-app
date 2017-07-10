
using Chevron.ITC.AMAOC.ViewModels;

using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class EventDetailPage : ContentPage
    {
        EventDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public EventDetailPage()
        {
            InitializeComponent();
        }

        public EventDetailPage(EventDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
