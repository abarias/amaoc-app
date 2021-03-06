﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Helpers;
using Microsoft.Identity.Client;
using System.Net.Http;
using Chevron.ITC.AMAOC.Helpers.Proxy;

namespace Chevron.ITC.AMAOC.Services
{
    public class StoreManager : IStoreManager
    {

        public static MobileServiceClient MobileService { get; set; }

        /// <summary>
        /// Syncs all tables.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="syncUserSpecific">If set to <c>true</c> sync user specific.</param>
        public async Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            if (!IsInitialized)
                await InitializeAsync();

            var taskList = new List<Task<bool>>();
            taskList.Add(EventStore.SyncAsync());
            taskList.Add(EmployeeStore.SyncAsync());
            taskList.Add(EventAttendeeStore.SyncAsync());
            taskList.Add(NotificationStore.SyncAsync());
            taskList.Add(EventRatingCommentStore.SyncAsync());

            var successes = await Task.WhenAll(taskList).ConfigureAwait(false);
            return successes.Any(x => !x);//if any were a failure.
        }

        /// <summary>
        /// Drops all tables from the database and updated DB Id
        /// </summary>
        /// <returns>The everything async.</returns>
        public Task DropEverythingAsync()
        {
            Settings.UpdateDatabaseId();
            EventStore.DropTable();
            EmployeeStore.DropTable();
            EventAttendeeStore.DropTable();
            NotificationStore.DropTable();
            EventRatingCommentStore.DropTable();
            IsInitialized = false;
            return Task.FromResult(true);
        }




        public bool IsInitialized { get; private set; }
        #region IStoreManager implementation
        object locker = new object();
        public async Task InitializeAsync()
        {
            MobileServiceSQLiteStore store;
            lock (locker)
            {

                if (IsInitialized)
                    return;

                IsInitialized = true;
                var dbId = Settings.DatabaseId;
                var path = $"syncstore{dbId}.db";
                //HttpClientHandler handler = new HttpClientHandler()
                //{
                //    Proxy = new WebProxy(new Uri("http://192.168.1.116:8888")),
                //    UseProxy = true
                //};
                //MobileService = new MobileServiceClient(App.AzureMobileAppUrl, handler);
                MobileService = new MobileServiceClient(AppConfig.AzureMobileAppUrl);

                store = new MobileServiceSQLiteStore(path);                
                store.DefineTable<Event>();
                store.DefineTable<Employee>();
                store.DefineTable<StoreSettings>();
                store.DefineTable<EventAttendee>();
                store.DefineTable<Notification>();
                store.DefineTable<EventRatingComment>();
            }

            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler()).ConfigureAwait(false);

            await LoadCachedTokenAsync().ConfigureAwait(false);

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
        #endregion

        public async Task<MobileServiceUser> LoginAsync(string accessToken)
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }

            var credentials = new JObject();
            credentials["access_token"] = accessToken;            
            
            MobileServiceUser user = await MobileService.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, credentials);
            
            await CacheToken(user);

            return user;
        }

        private static IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = JwtUtility.GetDecodedPayload(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }

            await MobileService.LogoutAsync();
            App.PCA.Remove(GetUserByPolicy(App.PCA.Users, AzureB2CCoordinates.PolicySignUpSignIn));
                        
            var settings = await ReadSettingsAsync();

            if (settings != null)
            {
                settings.AuthToken = string.Empty;
                settings.UserId = string.Empty;

                await SaveSettingsAsync(settings);
            }
        }

        async Task SaveSettingsAsync(StoreSettings settings) =>
            await MobileService.SyncContext.Store.UpsertAsync(nameof(StoreSettings), new[] { JObject.FromObject(settings) }, true);

        async Task<StoreSettings> ReadSettingsAsync() =>
            (await MobileService.SyncContext.Store.LookupAsync(nameof(StoreSettings), StoreSettings.StoreSettingsId))?.ToObject<StoreSettings>();


        async Task CacheToken(MobileServiceUser user)
        {
            var settings = new StoreSettings
            {
                UserId = user.UserId,
                AuthToken = user.MobileServiceAuthenticationToken
            };

            await SaveSettingsAsync(settings);

        }

        async Task LoadCachedTokenAsync()
        {
            StoreSettings settings = await ReadSettingsAsync();

            if (settings != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(settings.AuthToken) && JwtUtility.GetTokenExpiration(settings.AuthToken) > DateTime.UtcNow)
                    {
                        MobileService.CurrentUser = new MobileServiceUser(settings.UserId);
                        MobileService.CurrentUser.MobileServiceAuthenticationToken = settings.AuthToken;
                    }
                }
                catch (InvalidTokenException)
                {
                    settings.AuthToken = string.Empty;
                    settings.UserId = string.Empty;

                    await SaveSettingsAsync(settings);
                }
            }
        }

        public class StoreSettings
        {
            public const string StoreSettingsId = "store_settings";

            public StoreSettings()
            {
                Id = StoreSettingsId;
            }

            public string Id { get; set; }

            public string UserId { get; set; }

            public string AuthToken { get; set; }
        }
    }
}
