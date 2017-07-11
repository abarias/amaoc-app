using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Views;
using Chevron.ITC.AMAOC.DataObjects;

using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Event> Events { get; set; }
        public Command LoadItemsCommand { get; set; }

        public EventsViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Your OC Journey";
            Events = new ObservableRangeCollection<Event>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewEventPage, Event>(this, "AddItem", async (obj, ocevent) =>
            {
                var _item = ocevent as Event;
                Events.Add(_item);
                await StoreManager.EventStore.InsertAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Events.Clear();
                var items = await StoreManager.EventStore.GetItemsAsync(true);
                Events.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}