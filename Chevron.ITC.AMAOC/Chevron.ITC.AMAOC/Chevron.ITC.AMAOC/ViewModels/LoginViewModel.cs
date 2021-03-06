﻿using System.Threading.Tasks;
using System.Windows.Input;

using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Services;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;
using Newtonsoft.Json.Linq;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Interfaces;
using FormsToolkit;
using Plugin.Connectivity;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        static ISSOClient client;
        public LoginViewModel()
        {
            SignInCommand = new Command(async () => await SignIn());            

            client = DependencyService.Get<ISSOClient>();
        }

        string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        
        public ICommand SignInCommand { get; }

        async Task SignIn()
        {
            try
            {
                IsBusy = true;
                Message = "Signing In...";

                // Log the user in
                await TryLoginAsync();
            }
            finally
            {
                Message = string.Empty;
                IsBusy = false;

                if (Settings.Current.IsLoggedIn)
                    App.GoToMainPage();
            }
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

        public async Task<bool> TryLoginAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                MessagingUtils.SendOfflineMessage();
                return false;
            }

            AccountResponse result = null;              
            
            try
            {
                //AuthenticationResult ar = await App.PCA.AcquireTokenAsync(AzureB2CCoordinates.Scopes, GetUserByPolicy(App.PCA.Users, AzureB2CCoordinates.PolicySignUpSignIn), App.UiParent);
                //result = await client.LoginAsync(ar.IdToken, ar.AccessToken);
                result = await client.LoginAsync("TestIdToken", "TestAccessToken");

                if (result?.Success ?? false)
                {
                    Settings.UserId = result.User?.UserId ?? string.Empty;
                    Settings.Email = result.User?.Email ?? string.Empty;
                    Settings.FullName = result.User?.FullName ?? string.Empty;
                    Settings.CAI = result.User?.CAI ?? string.Empty;
                    MessagingService.Current.SendMessage(MessageKeys.LoggedIn);
                    Logger.Track(AMAOCLoggerKeys.LoginSuccess);
                    try
                    {
                        await StoreManager.SyncAllAsync(true);
                        Settings.Current.LastSync = DateTime.UtcNow;
                        Settings.Current.HasSyncedData = true;
                        await Finish();                        
                    }
                    catch (Exception ex)
                    {
                        //if sync doesn't work don't worry it is alright we can recover later
                        Logger.Report(ex);
                    }
                    
                    Settings.FirstRun = false;
                }
                else
                {
                    Logger.Track(AMAOCLoggerKeys.LoginFailure, "Reason", result.Error);
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                    {
                        Title = "Unable to Sign in",
                        Message = result.Error,
                        Cancel = "OK"
                    });
                }
            }
            catch(MsalException mex)
            {

            }
            catch (Exception ex)
            {
                // Checking the exception message 
                // should ONLY be done for B2C
                // reset and not any other error.
                if (ex.Message.Contains("AADB2C90118"))
                { }
                //OnPasswordReset();
                // Alert if any exception excludig user cancelling sign-in dialog
                else if (((ex as MsalException)?.ErrorCode != "authentication_canceled")) { }
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }

            //if (user == null)
            //{
            //    MessagingCenter.Send(new MessagingCenterAlert
            //    {
            //        Title = "Sign In Error",
            //        Message = "Unable to sign in, please check your credentials and try again.",
            //        Cancel = "OK"
            //    }, "message");
            //    return false;
            //}

            return true;
        }

        async Task Finish()
        {
            var emp = await StoreManager.EmployeeStore.GetEmployeeByUserId(Settings.UserId);
            
            if (emp == null)
            {
                var newEmp = new Employee
                {
                    CAI = Settings.CAI,
                    Email = Settings.Email,
                    FullName = Settings.FullName,
                    UserId = Settings.UserId                    
                };
                await StoreManager.EmployeeStore.InsertAsync(newEmp);

            }   
            else
            {
                Settings.TotalPoints = emp.TotalPointsEarned.ToString();
            }
        }
    }
}