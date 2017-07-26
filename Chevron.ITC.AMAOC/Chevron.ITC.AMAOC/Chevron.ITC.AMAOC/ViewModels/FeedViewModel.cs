using Chevron.ITC.AMAOC.DataObjects;
using FormsToolkit;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class FeedViewModel : ViewModelBase
    {
        
        public ObservableRangeCollection<Event> Events { get; } = new ObservableRangeCollection<Event>();
        public DateTime NextForceRefresh { get; set; }
        public FeedViewModel()
        {
            NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
        }


        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommandAsync()));

        async Task ExecuteRefreshCommandAsync()
        {
            try
            {
                NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                IsBusy = true;
                var tasks = new Task[]
                    {
                        ExecuteLoadEmployeeCommandAsync(),
                        ExecuteLoadNotificationsCommandAsync(),                        
                        ExecuteLoadEventsCommandAsync()
                    };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteRefreshCommandAsync";
                Logger.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        string points;
        public string Points
        {
            get { return points; }
            set { SetProperty(ref points, value); }
        }

        string rank;
        public string Rank
        {
            get { return rank; }
            set { SetProperty(ref rank, value); }
        }

        Employee employee;
        public Employee Employee
        {
            get { return employee; }
            set { SetProperty(ref employee, value); }
        }

        bool loadingEmployee;
        public bool LoadingEmployee
        {
            get { return loadingEmployee; }
            set { SetProperty(ref loadingEmployee, value); }
        }

        ICommand loadEmployeeCommand;
        public ICommand LoadEmployeeCommand =>
            loadEmployeeCommand ?? (loadEmployeeCommand = new Command(async () => await ExecuteLoadEmployeeCommandAsync()));

        async Task ExecuteLoadEmployeeCommandAsync()
        {
            if (LoadingEmployee)
                return;
            LoadingEmployee = true;
            Rank = string.Empty;
#if DEBUG
            await Task.Delay(1000);
#endif

            try
            {
                var rankedEmployees = await StoreManager.EmployeeStore.GetEmployeesTopTenByPoints(Chevron.ITC.AMAOC.Helpers.Settings.Current.UserId);
                var currentEmp = rankedEmployees.FirstOrDefault(e => e.UserId == Chevron.ITC.AMAOC.Helpers.Settings.Current.UserId);
                Rank = $"Ranked {currentEmp.Rank} out of {rankedEmployees.Count()}";
                Points = $"{currentEmp.TotalPointsEarned} points";
                Employee = currentEmp;
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteLoadEmployeeCommandAsync";
                Logger.Report(ex);
            }
            finally
            {
                LoadingEmployee = false;
            }
        }

        Notification notification;
        public Notification Notification
        {
            get { return notification; }
            set { SetProperty(ref notification, value); }
        }

        bool loadingNotifications;
        public bool LoadingNotifications
        {
            get { return loadingNotifications; }
            set { SetProperty(ref loadingNotifications, value); }
        }

        ICommand loadNotificationsCommand;
        public ICommand LoadNotificationsCommand =>
            loadNotificationsCommand ?? (loadNotificationsCommand = new Command(async () => await ExecuteLoadNotificationsCommandAsync()));

        async Task ExecuteLoadNotificationsCommandAsync()
        {
            if (LoadingNotifications)
                return;
            LoadingNotifications = true;
#if DEBUG
            await Task.Delay(1000);
#endif

            try
            {
                Notification = await StoreManager.NotificationStore.GetLatestNotification();
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteLoadNotificationsCommandAsync";
                Logger.Report(ex);
                Notification = new Notification
                {
                    Date = DateTime.UtcNow,
                    Text = "Welcome to AMA OC Events!"
                };
            }
            finally
            {
                LoadingNotifications = false;
            }
        }

        bool loadingEvents;
        public bool LoadingEvents
        {
            get { return loadingEvents; }
            set { SetProperty(ref loadingEvents, value); }
        }


        ICommand loadEventsCommand;
        public ICommand LoadEventsCommand =>
            loadEventsCommand ?? (loadEventsCommand = new Command(async () => await ExecuteLoadEventsCommandAsync()));

        async Task ExecuteLoadEventsCommandAsync()
        {
            if (LoadingEvents)
                return;

            LoadingEvents = true;

            try
            {
                NoEvents = false;
                Events.Clear();
                OnPropertyChanged("Events");
#if DEBUG
                await Task.Delay(1000);
#endif
                var events = await StoreManager.EventStore.GetNextEvents();
                if (events != null)
                    Events.AddRange(events);

                NoEvents = Events.Count == 0;
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteLoadEventsCommandAsync";
                Logger.Report(ex);
                NoEvents = true;
            }
            finally
            {
                LoadingEvents = false;
            }

        }

        bool noEvents;
        public bool NoEvents
        {
            get { return noEvents; }
            set { SetProperty(ref noEvents, value); }
        }

        Event selectedEvent;
        public Event SelectedEvent
        {
            get { return selectedEvent; }
            set
            {
                selectedEvent = value;
                OnPropertyChanged();
                if (selectedEvent == null)
                    return;

                MessagingService.Current.SendMessage(MessageKeys.NavigateToEvent, selectedEvent);

                SelectedEvent = null;
            }
        }
        
    }
}
