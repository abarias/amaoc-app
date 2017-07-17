using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.ViewModels;
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
    public partial class EventFeedbackPage : ContentPage
    {
        EventFeedbackViewModel vm;

        public EventFeedbackPage(Event ocEvent)
        {
            InitializeComponent();
            BindingContext = vm = new EventFeedbackViewModel(Navigation, ocEvent);

            ToolbarDone.Command = new Command(async () =>
            {
                if (vm.IsBusy)
                    return;

                await Navigation.PopModalAsync();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var items = StarGrid.Behaviors.Count;
            for (int i = 0; i < items; i++)
                StarGrid.Behaviors.RemoveAt(i);
        }
    }
}