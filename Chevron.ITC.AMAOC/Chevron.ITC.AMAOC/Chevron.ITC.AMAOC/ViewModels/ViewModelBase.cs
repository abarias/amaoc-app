using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Backend.Helpers;
using MvvmHelpers;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        //public IBaseStore<Event> DataStore => DependencyService.Get<IBaseStore<Event>>();
        protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();

        protected ILogger Logger { get; } = DependencyService.Get<ILogger>();

        protected IToast Toast { get; } = DependencyService.Get<IToast>();

        protected INavigation Navigation { get; }
       
        public ViewModelBase(INavigation navigation = null)
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
                DependencyService.Register<IFeedbackQuestionStore, Chevron.ITC.AMAOC.MockStores.FeedbackQuestionStore>();
                DependencyService.Register<IFeedbackAnswerStore, Chevron.ITC.AMAOC.MockStores.FeedbackAnswerStore>();
                DependencyService.Register<IFeedbackAnswerFreeTextStore, Chevron.ITC.AMAOC.MockStores.FeedbackAnswerFreeTextStore>();
                DependencyService.Register<IEventRatingCommentStore, Chevron.ITC.AMAOC.MockStores.EventRatingCommentStore>();
                DependencyService.Register<INotificationStore, Chevron.ITC.AMAOC.MockStores.NotificationStore>();
                DependencyService.Register<ISSOClient, Chevron.ITC.AMAOC.MockStores.SSOClient>();
                DependencyService.Register<IStoreManager, Chevron.ITC.AMAOC.MockStores.StoreManager>();
            }
            else
            { 
                DependencyService.Register<IEventStore, Chevron.ITC.AMAOC.Stores.EventStore>();
                DependencyService.Register<IEmployeeStore, Chevron.ITC.AMAOC.Stores.EmployeeStore>();
                DependencyService.Register<IEventAttendeeStore, Chevron.ITC.AMAOC.Stores.EventAttendeeStore>();
                DependencyService.Register<IEventRatingCommentStore, Chevron.ITC.AMAOC.Stores.EventRatingCommentStore>();
                DependencyService.Register<INotificationStore, Chevron.ITC.AMAOC.Stores.NotificationStore>();
                DependencyService.Register<ISSOClient, Chevron.ITC.AMAOC.Services.SSOClient>();
                DependencyService.Register<IStoreManager, Chevron.ITC.AMAOC.Services.StoreManager>();
            }
        }

        public Settings Settings
        {
            get { return Settings.Current; }
        }        
    }
}

