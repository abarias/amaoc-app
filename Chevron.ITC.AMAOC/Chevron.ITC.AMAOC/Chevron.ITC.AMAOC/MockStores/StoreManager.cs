using Chevron.ITC.AMAOC.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class StoreManager : IStoreManager
    {
        #region IStoreManager implementation

        public Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            return Task.FromResult(true);
        }

        public bool IsInitialized { get { return true; } }
        public Task InitializeAsync()
        {
            return Task.FromResult(true);
        }

        #endregion

        public Task DropEverythingAsync()
        {
            return Task.FromResult(true);
        }

        IEventStore eventStore;
        IEmployeeStore employeeStore;
        IEventAttendeeStore eventAttendeeStore;
        IFeedbackQuestionStore feedbackQuestionStore;
        IFeedbackAnswerStore feedbackAnswerStore;
        IFeedbackAnswerFreeTextStore feedbackAnswerFreeTextStore;
        IEventRatingCommentStore eventRatingCommentStore;
        INotificationStore notificationStore;

        public IEventStore EventStore => eventStore ?? (eventStore = DependencyService.Get<IEventStore>());
        public IEmployeeStore EmployeeStore => employeeStore ?? (employeeStore = DependencyService.Get<IEmployeeStore>());
        public IEventAttendeeStore EventAttendeeStore => eventAttendeeStore ?? (eventAttendeeStore = DependencyService.Get<IEventAttendeeStore>());
        public IFeedbackQuestionStore FeedbackQuestionStore => feedbackQuestionStore ?? (feedbackQuestionStore = DependencyService.Get<IFeedbackQuestionStore>());
        public IFeedbackAnswerStore FeedbackAnswerStore => feedbackAnswerStore ?? (feedbackAnswerStore = DependencyService.Get<IFeedbackAnswerStore>());
        public IFeedbackAnswerFreeTextStore FeedbackAnswerFreeTextStore => feedbackAnswerFreeTextStore ?? (feedbackAnswerFreeTextStore = DependencyService.Get<IFeedbackAnswerFreeTextStore>());
        public IEventRatingCommentStore EventRatingCommentStore => eventRatingCommentStore ?? (eventRatingCommentStore = DependencyService.Get<IEventRatingCommentStore>());
        public INotificationStore NotificationStore => notificationStore ?? (notificationStore = DependencyService.Get<INotificationStore>());

    }
}
