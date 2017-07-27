using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using FormsToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class EventFeedbackViewModel : ViewModelBase
    {
        Event ocEvent;
        public Event OCEvent
        {
            get { return ocEvent; }
            set { SetProperty(ref ocEvent, value); }
        }

        string eventComments;
        public string EventComments
        {
            get { return eventComments; }
            set { SetProperty(ref eventComments, value); }
        }

        public EventFeedbackViewModel(INavigation navigation, Event e) : base(navigation)
        {
            OCEvent = e;
            EventComments = string.Empty;
        }

        ICommand submitRatingCommand;
        public ICommand SubmitRatingCommand =>
            submitRatingCommand ?? (submitRatingCommand = new Command<int>(async (rating) => await ExecuteSubmitRatingCommandAsync(rating)));

        async Task ExecuteSubmitRatingCommandAsync(int rating)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (rating == 0)
                {

                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                    {
                        Title = "No Rating Selected",
                        Message = "Please select a rating to leave feedback for this session.",
                        Cancel = "OK"
                    });
                    return;
                }

                FeedViewModel.ForceRefresh = true;
                Logger.Track(AMAOCLoggerKeys.LeaveFeedback, "Title", rating.ToString());

                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Feedback Received",
                    Message = "Thanks for the feedback!",
                    Cancel = "OK",
                    OnCompleted = async () =>
                    {
                        await Navigation.PopModalAsync();
                        if (Device.OS == TargetPlatform.Android)
                            MessagingService.Current.SendMessage("eventstatus_changed");
                    }
                });

                OCEvent.FeedbackLeft = true;
                await StoreManager.EventRatingCommentStore.InsertAsync(new EventRatingComment
                {
                    EmployeeId = Settings.UserId,
                    EventId = OCEvent.Id,
                    EventRating = rating,
                    EventComment = EventComments
                });
                var emp = await StoreManager.EmployeeStore.GetEmployeeByUserId(Settings.UserId);
                int totalPoints = emp.TotalPointsEarned + 5;
                Settings.TotalPoints = totalPoints.ToString();
                await StoreManager.EmployeeStore.UpdateAsync(emp);
            }
            catch (Exception ex)
            {
                Logger.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
