using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Abstractions
{
    public interface IStoreManager
    {
        bool IsInitialized { get; }
        Task InitializeAsync();

        IEventStore EventStore { get; }
        IEmployeeStore EmployeeStore { get; }
        IEventAttendeeStore EventAttendeeStore { get; }
        IFeedbackQuestionStore FeedbackQuestionStore { get; }       
        IFeedbackAnswerStore FeedbackAnswerStore { get; }
        IFeedbackAnswerFreeTextStore FeedbackAnswerFreeTextStore { get; }
        IEventRatingCommentStore EventRatingCommentStore { get; }
        INotificationStore NotificationStore { get; }

        Task<bool> SyncAllAsync(bool syncUserSpecific);
        Task DropEverythingAsync();
    }
}
