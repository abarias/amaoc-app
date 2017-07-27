using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.Services;
using Chevron.ITC.AMAOC.Extensions;
using FormsToolkit;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Chevron.ITC.AMAOC.Helpers;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class EventDetailViewModel : ViewModelBase
    {
        Event ocEvent;        
        public Event Event
        {
            get { return ocEvent; }
            set { SetProperty(ref ocEvent, value); }
        }

        public EventDetailViewModel(INavigation navigation, Event ocEvent = null) : base(navigation)
        {
            Event = ocEvent;
            if (Event.OCEventStatus == Event.EventStatus.NotStarted)
                ShowReminder = true;
            else
                ShowReminder = false;
        }

        public bool ShowReminder { get; set; }

        bool isReminderSet;
        public bool IsReminderSet
        {
            get { return isReminderSet; }
            set { SetProperty(ref isReminderSet, value); }
        }

        ICommand reminderCommand;
        public ICommand ReminderCommand =>
            reminderCommand ?? (reminderCommand = new Command(async () => await ExecuteReminderCommandAsync()));

        async Task ExecuteReminderCommandAsync()
        {
            if (!IsReminderSet)
            {
                var result = await ReminderService.AddReminderAsync(Event.Id,
                    new Plugin.Calendars.Abstractions.CalendarEvent
                    {
                        AllDay = false,
                        Description = Event.Abstract,
                        Location = Event.Location,
                        Name = Event.Name,
                        Start = Event.StartTime?.DateTime ?? DateTime.Now,
                        End = Event.EndTime?.DateTime ?? DateTime.Now
                    });


                if (!result)
                    return;

                Logger.Track(AMAOCLoggerKeys.ReminderAdded, "Title", Event.Name);
                IsReminderSet = true;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync(Event.Id);
                if (!result)
                    return;
                Logger.Track(AMAOCLoggerKeys.ReminderRemoved, "Title", Event.Name);
                IsReminderSet = false;
            }

        }

        ICommand loadEventCommand;
        public ICommand LoadEventCommand =>
            loadEventCommand ?? (loadEventCommand = new Command(async () => await ExecuteLoadEventCommandAsync()));

        public async Task ExecuteLoadEventCommandAsync()
        {

            if (IsBusy)
                return;

            try
            {


                IsBusy = true;


                IsReminderSet = await ReminderService.HasReminderAsync(Event.Id);
                Event.FeedbackLeft = await StoreManager.EventRatingCommentStore.LeftFeedback(Event);
                Event.IsAttended = Settings.Current.IsEventAttended(Event.Id);
                


            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEventCommandAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

        }

        public async Task AttendEvent()
        {
            EventsViewModel.ForceRefresh = true;
            FeedViewModel.ForceRefresh = true;
            
            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.SendMessage("eventstatus_changed");

            Event.IsAttended = true;
            Event.OCEventStatus = Event.EventStatus.Completed;

            //Settings.Current.AttendEvent(Event.Id);
            await StoreManager.EventAttendeeStore.InsertAsync(new EventAttendee
            {
                EventId = Event.Id,
                EmployeeId = Settings.UserId
            });

            int totalPoints = Convert.ToInt32(Settings.TotalPoints) + Event.Points;
            var emp = await StoreManager.EmployeeStore.GetEmployeeByUserId(Settings.UserId);
            emp.TotalPointsEarned = totalPoints;
            Settings.TotalPoints = totalPoints.ToString();
            await StoreManager.EmployeeStore.UpdateAsync(emp);
        }
    }
}