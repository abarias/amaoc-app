using System.Collections.Generic;
using System;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Services;
using Chevron.ITC.AMAOC.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Identity.Client;
using Chevron.ITC.AMAOC.ViewModels;
using Chevron.ITC.AMAOC.Interfaces;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Chevron.ITC.AMAOC
{
    public partial class App : Application
    {
        public static PublicClientApplication PCA = null;
        //MUST use HTTPS, neglecting to do so will result in runtime errors on iOS
        public static bool AzureNeedsSetup => AzureMobileAppUrl == "https://cvxitcamaocapp.azurewebsites.net";
        public static string AzureMobileAppUrl = "https://cvxitcamaocapp.azurewebsites.net";
        // Azure AD B2C Coordinates
        public static string Tenant = "chevronitcama.onmicrosoft.com";
        public static string ClientId = "7493eff3-078c-4f58-a5f2-effaa18acbfb";
        public static string PolicySignUpSignIn = "B2C_1_AMAOCAppSignUp";
        public static string PolicyEditProfile = "B2C_1_AMAOCAppEditProfile";
        public static string PolicyResetPassword = "B2C_1_AMAOCAppPassReset";        
        public static string DefaultPolicy = PolicySignUpSignIn;


        public static string[] Scopes = { "https://chevronitcama.onmicrosoft.com/amaocapp/read" };

        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";        
        public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";

        public static UIParent UiParent = null;

        public static IDictionary<string, string> LoginParameters => null;

        static ILogger logger;
        public static ILogger Logger => logger ?? (logger = DependencyService.Get<ILogger>());

        public App()
        {            
            InitializeComponent();

            BaseViewModel.Init(true);

            PCA = new PublicClientApplication(ClientId, Authority);
            
            PCA.RedirectUri = $"com.onmicrosoft.chevronitcama.amaocapp://auth";

            SetMainPage();
        }

        public void SetMainPage()
        {
            if (!Settings.Current.IsLoggedIn)
            {
                MainPage = new AMAOCNavigationPage(new LoginPage());                
            }
            else
            {
                GoToMainPage();
            }
        }

        public static void GoToMainPage()
        {
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    Current.MainPage = new RootPageAndroid();
                    break;
                case TargetPlatform.iOS:
                    Current.MainPage = new AMAOCNavigationPage(new RootPageiOS());
                    break;
                case TargetPlatform.Windows:
                case TargetPlatform.WinPhone:                    
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
