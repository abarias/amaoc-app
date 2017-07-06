using System.Threading.Tasks;
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

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            SignInCommand = new Command(async () => await SignIn());
            NotNowCommand = new Command(App.GoToMainPage);
        }

        string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        public ICommand NotNowCommand { get; }
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

                if (Settings.IsLoggedIn)
                    App.GoToMainPage();
            }
        }

        private static IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }

            return null;
        }

        public static async Task<bool> TryLoginAsync()
        {
            var authentication = DependencyService.Get<IAuthenticator>();
            authentication.ClearCookies();

            var dataStore = DependencyService.Get<IDataStore<Event>>() as AzureDataStore;
            await dataStore.InitializeAsync();

            if (dataStore.UseAuthentication)
            {
                //var user = await authentication.LoginAsync(dataStore.MobileService, dataStore.AuthProvider, App.LoginParameters);                

                try
                {
                    AuthenticationResult ar = await App.PCA.AcquireTokenAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.UiParent);
                    JObject user = ParseIdToken(ar.IdToken);
                    Settings.AuthToken = ar.AccessToken;
                    Settings.UserId = user["name"]?.ToString() ?? string.Empty;
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

                //Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                //Settings.UserId = user?.UserId ?? string.Empty;
            }

            return true;
        }

        private static string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        static JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

    }
}