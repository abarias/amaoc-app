using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Views;
using Chevron.ITC.AMAOC.DataObjects;

using Xamarin.Forms;
using System.Windows.Input;
using FormsToolkit;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Event> Events { get; }  = new ObservableRangeCollection<Event>();
        public Command LoadItemsCommand { get; set; }
        public DateTime NextForceRefresh { get; set; }

        bool noEventsFound;
        public bool NoEventsFound
        {
            get { return noEventsFound; }
            set { SetProperty(ref noEventsFound, value); }
        }

        string noEventsFoundMessage;
        public string NoEventsFoundMessage
        {
            get { return noEventsFoundMessage; }
            set { SetProperty(ref noEventsFoundMessage, value); }
        }


        public EventsViewModel(INavigation navigation) : base(navigation)
        {
            NextForceRefresh = DateTime.UtcNow.AddMinutes(45);

            //Title = "Your OC Journey";
            
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());


            MessagingCenter.Subscribe<NewEventPage, Event>(this, "AddItem", async (obj, ocevent) =>
            {
                var _item = ocevent as Event;
                Events.Add(_item);
                await StoreManager.EventStore.InsertAsync(_item);
            });
        }


        ICommand forceRefreshCommand;
        public ICommand ForceRefreshCommand =>
        forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

        async Task ExecuteForceRefreshCommandAsync()
        {
            await ExecuteLoadEventsAsync(true);
        }


        ICommand loadEventsCommand;
        public ICommand LoadEventsCommand =>
            loadEventsCommand ?? (loadEventsCommand = new Command<bool>(async (f) => await ExecuteLoadEventsAsync()));

        async Task<bool> ExecuteLoadEventsAsync(bool force = false)
        {
            if (IsBusy)
                return false;

            try
            {
                NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                IsBusy = true;
                NoEventsFound = false;
                

#if DEBUG
                await Task.Delay(1000);
#endif

                Events.ReplaceRange(await StoreManager.EventStore.GetItemsAsync(force));                                            

                if (Events.Count == 0)
                {                    
                    NoEventsFoundMessage = "No Events Found";

                    NoEventsFound = true;
                }
                else
                {
                    NoEventsFound = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventsAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
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