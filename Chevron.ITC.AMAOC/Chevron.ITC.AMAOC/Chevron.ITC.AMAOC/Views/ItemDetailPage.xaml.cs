
using Chevron.ITC.AMAOC.ViewModels;

using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        EventDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(EventDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
