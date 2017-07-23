using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chevron.ITC.AMAOC.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        static Settings settings;        

        /// <summary>
        /// Gets or sets the current settings. This should always be used
        /// </summary>
        /// <value>The current.</value>
        public static Settings Current
        {
            get { return settings ?? (settings = new Settings()); }
        }

        #region Setting Constants
        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;
        #endregion

        public static string AuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(AuthTokenKey, value);
            }
        }

        public void SaveReminderId(string id, string calId)
        {
            AppSettings.AddOrUpdateValue<string>(GetReminderId(id), calId);
        }

        string GetReminderId(string id)
        {
            return "reminder_" + id;
        }

        public string GetEventId(string id)
        {
            return AppSettings.GetValueOrDefault(GetReminderId(id), string.Empty);
        }

        public void RemoveReminderId(string id)
        {
            AppSettings.Remove(GetReminderId(id));
        }

        const string HasSetReminderKey = "set_a_reminder";
        static readonly bool HasSetReminderDefault = false;

        public bool HasSetReminder
        {
            get { return AppSettings.GetValueOrDefault<bool>(HasSetReminderKey, HasSetReminderDefault); }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(HasSetReminderKey, value);
            }
        }

        const string AMAOCCalendarIdKey = "amaoc_calendar";
        static readonly string AMAOCCalendarIdDefault = string.Empty;
        public string AMAOCCalendarId
        {
            get { return AppSettings.GetValueOrDefault<string>(AMAOCCalendarIdKey, AMAOCCalendarIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(AMAOCCalendarIdKey, value); }
        }


        const string PushNotificationsEnabledKey = "push_enabled";
        static readonly bool PushNotificationsEnabledDefault = false;

        public bool PushNotificationsEnabled
        {
            get { return AppSettings.GetValueOrDefault<bool>(PushNotificationsEnabledKey, PushNotificationsEnabledDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<bool>(PushNotificationsEnabledKey, value))
                    OnPropertyChanged();
            }
        }

        const string FirstRunKey = "first_run";
        static readonly bool FirstRunDefault = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to see favorites only.
        /// </summary>
        /// <value><c>true</c> if favorites only; otherwise, <c>false</c>.</value>
        public bool FirstRun
        {
            get { return AppSettings.GetValueOrDefault<bool>(FirstRunKey, FirstRunDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<bool>(FirstRunKey, value))
                    OnPropertyChanged();
            }
        }

        const string GooglePlayCheckedKey = "play_checked";
        static readonly bool GooglePlayCheckedDefault = false;

        public bool GooglePlayChecked
        {
            get { return AppSettings.GetValueOrDefault<bool>(GooglePlayCheckedKey, GooglePlayCheckedDefault); }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(GooglePlayCheckedKey, value);
            }
        }

        const string AttemptedPushKey = "attempted_push";
        static readonly bool AttemptedPushDefault = false;

        public bool AttemptedPush
        {
            get { return AppSettings.GetValueOrDefault<bool>(AttemptedPushKey, AttemptedPushDefault); }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(AttemptedPushKey, value);
            }
        }


        const string PushRegisteredKey = "push_registered";
        static readonly bool PushRegisteredDefault = false;

        public bool PushRegistered
        {
            get { return AppSettings.GetValueOrDefault<bool>(PushRegisteredKey, PushRegisteredDefault); }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(PushRegisteredKey, value);
            }
        }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(UserIdKey, value);
            }
        }

        const string EmailKey = "email_key";
        readonly string EmailDefault = string.Empty;
        public string Email
        {
            get { return AppSettings.GetValueOrDefault<string>(EmailKey, EmailDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue(EmailKey, value))
                {
                    OnPropertyChanged();
                    //OnPropertyChanged(nameof(UserAvatar));
                }
            }
        }

        const string FullNameKey = "fullname_key";
        readonly string FullNameDefault = string.Empty;
        public string FullName
        {
            get { return AppSettings.GetValueOrDefault<string>(FullNameKey, FullNameDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue(FullNameKey, value))
                {
                    OnPropertyChanged();
                    //OnPropertyChanged(nameof(UserDisplayName));
                }
            }
        }

        const string CAIKey = "cai_key";
        readonly string CAIDefault = string.Empty;
        public string CAI
        {
            get { return AppSettings.GetValueOrDefault<string>(CAIKey, CAIDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue(CAIKey, value))
                {
                    OnPropertyChanged();
                    //OnPropertyChanged(nameof(UserDisplayName));
                }
            }
        }

        const string TotalPointsKey = "totalpoints_key";
        readonly string TotalPointsDefault = string.Empty;
        public string TotalPoints
        {
            get { return AppSettings.GetValueOrDefault<string>(TotalPointsKey, TotalPointsDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue(TotalPointsKey, value))
                {
                    OnPropertyChanged();
                    //OnPropertyChanged(nameof(UserDisplayName));
                }
            }
        }

        const string DatabaseIdKey = "azure_database";
        static readonly int DatabaseIdDefault = 0;

        public static int DatabaseId
        {
            get { return AppSettings.GetValueOrDefault<int>(DatabaseIdKey, DatabaseIdDefault); }
            set
            {
                AppSettings.AddOrUpdateValue<int>(DatabaseIdKey, value);
            }
        }

        public static int UpdateDatabaseId()
        {
            return DatabaseId++;
        }

        const string NeedsSyncKey = "needs_sync";
        const bool NeedsSyncDefault = true;
        public bool NeedsSync
        {
            get { return AppSettings.GetValueOrDefault<bool>(NeedsSyncKey, NeedsSyncDefault) || LastSync < DateTime.Now.AddDays(-1); }
            set { AppSettings.AddOrUpdateValue<bool>(NeedsSyncKey, value); }

        }

        const string LoginAttemptsKey = "login_attempts";
        const int LoginAttemptsDefault = 0;
        public int LoginAttempts
        {
            get
            {
                return AppSettings.GetValueOrDefault<int>(LoginAttemptsKey, LoginAttemptsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(LoginAttemptsKey, value);
            }
        }

        const string HasSyncedDataKey = "has_synced";
        const bool HasSyncedDataDefault = false;
        public bool HasSyncedData
        {
            get { return AppSettings.GetValueOrDefault<bool>(HasSyncedDataKey, HasSyncedDataDefault); }
            set { AppSettings.AddOrUpdateValue<bool>(HasSyncedDataKey, value); }

        }

        const string LastSyncKey = "last_sync";
        static readonly DateTime LastSyncDefault = DateTime.Now.AddDays(-30);
        public DateTime LastSync
        {
            get
            {
                return AppSettings.GetValueOrDefault<DateTime>(LastSyncKey, LastSyncDefault);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue<DateTime>(LastSyncKey, value))
                    OnPropertyChanged();
            }
        }

        bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (isConnected == value)
                    return;
                isConnected = value;
                OnPropertyChanged();
            }
        }

        #region Helpers

        public string UserDisplayName => IsLoggedIn ? $"{FullName}" : "Sign In";

        public string UserAvatar => IsLoggedIn ? Gravatar.GetURL(Email) : "profile_generic.png";

        //public bool HasFilters => (ShowPastSessions || FavoritesOnly || (!string.IsNullOrWhiteSpace(FilteredCategories) && !ShowAllCategories));

        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}