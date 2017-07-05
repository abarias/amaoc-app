using System.Collections.Generic;

using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Services;
using Chevron.ITC.AMAOC.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Chevron.ITC.AMAOC
{
    public partial class App : Application
    {
        //MUST use HTTPS, neglecting to do so will result in runtime errors on iOS
        public static bool AzureNeedsSetup => AzureMobileAppUrl == "http://cvxitcamaocapp.azurewebsites.net";
        public static string AzureMobileAppUrl = "http://cvxitcamaocapp.azurewebsites.net";
        // Azure AD B2C Coordinates
        public static string Tenant = "chevronitcama.onmicrosoft.com";
        public static string ClientID = "7493eff3-078c-4f58-a5f2-effaa18acbfb";
        public static string PolicySignUpSignIn = "B2C_1_AMAOCAppSignUp";
        public static string PolicyEditProfile = "B2C_1_AMAOCAppEditProfile";
        public static string PolicyResetPassword = "B2C_1_AMAOCAppPassReset";
        
        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";

        public static IDictionary<string, string> LoginParameters => null;

        public App()
        {
            InitializeComponent();

            DependencyService.Register<AzureDataStore>();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            if (!AzureNeedsSetup && !Settings.IsLoggedIn)
            {
                Current.MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = (Color)Current.Resources["Primary"],
                    BarTextColor = Color.White
                };
            }
            else
            {
                GoToMainPage();
            }
        }

        public static void GoToMainPage()
        {
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                }
            };
        }
    }
}
