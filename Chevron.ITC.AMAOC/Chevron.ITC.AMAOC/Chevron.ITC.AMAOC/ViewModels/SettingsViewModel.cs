﻿using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using FormsToolkit;
using Humanizer;
using MvvmHelpers;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {        
        public SettingsViewModel()
        {
            //This will be triggered wen 
            Settings.PropertyChanged += async (sender, e) =>
            {
                if (e.PropertyName == "Email")
                {
                    Settings.NeedsSync = true;                    
                    OnPropertyChanged("AccountItems");
                    //if logged in you should go ahead and sync data.
                    if (Settings.IsLoggedIn)
                    {
                        await ExecuteSyncCommandAsync();
                    }
                }
            };
        }        

        public string LastSyncDisplay
        {
            get
            {
                if (!Settings.HasSyncedData)
                    return "Never";

                return Settings.LastSync.Humanize();
            }
        }

        ICommand logoutCommand;
        public ICommand LogoutCommand =>
            logoutCommand ?? (logoutCommand = new Command(ExecuteLogoutCommand));

        void ExecuteLogoutCommand()
        {
            if (IsBusy)
                return;
        
            MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
            {
                Title = "Logout?",
                Question = "Are you sure you want to logout? You can only view and participate in AMA OC Events when logged in.",
                Positive = "Yes, Logout",
                Negative = "Cancel",
                OnCompleted = async (result) =>
                {
                    if (!result)
                        return;

                    await Logout();
                }
            });                
        }

        async Task Logout()
        {
            Logger.Track(AMAOCLoggerKeys.Logout);

            try
            {
                ISSOClient ssoClient = DependencyService.Get<ISSOClient>();
                if (ssoClient != null)
                {
                    await ssoClient.LogoutAsync();
                }
                
                Settings.TotalPoints = string.Empty;
                Settings.FullName = string.Empty;
                Settings.CAI = string.Empty;
                Settings.Email = string.Empty; //this triggers login text changed!
                
                await StoreManager.EventAttendeeStore.DropAttended();
                await StoreManager.EventRatingCommentStore.DropFeedback();
                await StoreManager.DropEverythingAsync();                
                App.SetMainPage();
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteLoginCommandAsync";
                //TODO validate here.
                Logger.Report(ex);
            }
        }

        string syncText = "Sync Now";
        public string SyncText
        {
            get { return syncText; }
            set { SetProperty(ref syncText, value); }
        }

        ICommand syncCommand;
        public ICommand SyncCommand =>
            syncCommand ?? (syncCommand = new Command(async () => await ExecuteSyncCommandAsync()));

        async Task ExecuteSyncCommandAsync()
        {

            if (IsBusy)
                return;

            if (!CrossConnectivity.Current.IsConnected)
            {
                MessagingUtils.SendOfflineMessage();
                return;
            }

            Logger.Track(AMAOCLoggerKeys.ManualSync);

            SyncText = "Syncing...";
            IsBusy = true;

            try
            {
#if DEBUG
                await Task.Delay(1000);
#endif

                Settings.HasSyncedData = true;
                Settings.LastSync = DateTime.UtcNow;
                OnPropertyChanged("LastSyncDisplay");

                await StoreManager.SyncAllAsync(Settings.Current.IsLoggedIn);                

            }
            catch (Exception ex)
            {
                ex.Data["method"] = "ExecuteSyncCommandAsync";
                MessagingUtils.SendAlert("Unable to sync", "Uh oh, something went wrong with the sync, please try again. \n\n Debug:" + ex.Message);
                Logger.Report(ex);
            }
            finally
            {
                SyncText = "Sync Now";
                IsBusy = false;
            }
        }
    }
}
