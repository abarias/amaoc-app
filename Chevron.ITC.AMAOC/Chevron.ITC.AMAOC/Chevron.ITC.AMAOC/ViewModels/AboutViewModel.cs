using Chevron.ITC.AMAOC.Interfaces;
using FormsToolkit;
using MvvmHelpers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class AboutViewModel : SettingsViewModel
    {
        public ObservableRangeCollection<Grouping<string, MenuItem>> MenuItems { get; }
        public ObservableRangeCollection<MenuItem> InfoItems { get; } = new ObservableRangeCollection<MenuItem>();
        public ObservableRangeCollection<MenuItem> AccountItems { get; } = new ObservableRangeCollection<MenuItem>();

        MenuItem syncItem;
        MenuItem accountItem;
        MenuItem pushItem;
        IPushNotifications push;
        public AboutViewModel()
        {
            push = DependencyService.Get<IPushNotifications>();            

            accountItem = new MenuItem
            {
                Name = "Logged in as:"
            };

            syncItem = new MenuItem
            {
                Name = "Last Sync:"
            };

            pushItem = new MenuItem
            {
                Name = "Enable push notifications"
            };

            pushItem.Command = new Command(() =>
            {
                if (push.IsRegistered)
                {
                    UpdateItems();
                    return;
                }

                if (Settings.AttemptedPush)
                {
                    MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                    {
                        Title = "Push Notification",
                        Question = "To enable push notifications, please go into Settings, Tap Notifications, and set Allow Notifications to on.",
                        Positive = "Settings",
                        Negative = "Maybe Later",
                        OnCompleted = (result) =>
                        {
                            if (result)
                            {
                                push.OpenSettings();
                            }
                        }
                    });
                    return;
                }

                push.RegisterForNotifications();
            });

            UpdateItems();

            AccountItems.Add(accountItem);
            AccountItems.Add(syncItem);
            AccountItems.Add(pushItem);

            //This will be triggered wen 
            Settings.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Email" || e.PropertyName == "LastSync" || e.PropertyName == "PushNotificationsEnabled")
                {
                    UpdateItems();
                    OnPropertyChanged("AccountItems");
                }
            };
        }

        public void UpdateItems()
        {
            syncItem.Subtitle = LastSyncDisplay;
            accountItem.Subtitle = Settings.IsLoggedIn ? Settings.UserDisplayName : "Not signed in";

            pushItem.Name = push.IsRegistered ? "Push notifications enabled" : "Enable push notifications";
        }
    }
}
