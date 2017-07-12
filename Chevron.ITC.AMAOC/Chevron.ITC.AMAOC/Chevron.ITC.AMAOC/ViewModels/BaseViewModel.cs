using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Backend.Helpers;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        //public IBaseStore<Event> DataStore => DependencyService.Get<IBaseStore<Event>>();
        protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();

        protected INavigation Navigation { get; }
       
        public BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public static void Init(bool mock = false)
        {            
            if (mock)
            {
                DependencyService.Register<IEventStore, Chevron.ITC.AMAOC.MockStores.EventStore>();
                DependencyService.Register<IEmployeeStore, Chevron.ITC.AMAOC.MockStores.EmployeeStore>();
                DependencyService.Register<IEventAttendeeStore, Chevron.ITC.AMAOC.MockStores.EventAttendeeStore>();
                DependencyService.Register<ISSOClient, Chevron.ITC.AMAOC.MockStores.SSOClient>();
                DependencyService.Register<IStoreManager, Chevron.ITC.AMAOC.MockStores.StoreManager>();
            }
            else
            { 
                DependencyService.Register<IEventStore, Chevron.ITC.AMAOC.Stores.EventStore>();
                DependencyService.Register<IEmployeeStore, Chevron.ITC.AMAOC.Stores.EmployeeStore>();
                DependencyService.Register<IEventAttendeeStore, Chevron.ITC.AMAOC.Stores.EventAttendeeStore>();
                DependencyService.Register<ISSOClient, Chevron.ITC.AMAOC.Services.SSOClient>();
                DependencyService.Register<IStoreManager, Chevron.ITC.AMAOC.Services.StoreManager>();
            }
        }

        public Settings Settings
        {
            get { return Settings.Current; }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}

