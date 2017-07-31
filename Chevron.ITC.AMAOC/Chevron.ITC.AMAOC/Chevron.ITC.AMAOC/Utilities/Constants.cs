using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC
{
    public static class ApiKeys
    {
        public const string HockeyAppiOS = "HockeyAppiOS";
        public const string HockeyAppAndroid = "c28a234ebba34b3b8507b08b0c5dbf27";
        public const string HockeyAppUWP = "HockeyAppUWP";

        public const string AzureServiceBusName = "Endpoint=sb://com-chevron-itc-amaoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MddcFqufUATN+M5sD/3Mr2XOPvQ3OAGRQv13YEqGC0g=";
        public const string AzureServiceBusUrl = "AzureServiceBusUrl";
        public const string AzureKey = "AAAAxZiLqjk:APA91bFo_JGv80UVInkMkrAKFncUUS2XtJvvBz_rx9g6_s613o4NyCrRaEBbRVO0Mz_VDzwwuhzxROGmY0Y1uNZTL37nLHl183z_heu82QY";
        public const string GoogleSenderId = "848667847225";
        public const string AzureHubName = "cvxitcamaocevents";
        public const string AzureListenConneciton = "AzureListenConneciton";

        public const string SenderID = "848667847225"; // Google API Project Number
        public const string ListenConnectionString = "Endpoint=sb://com-chevron-itc-amaoc.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=ybwh2jTTsZBNIhC1WUZ7Oxuoxxpb9h/+z1HwlZGdJ/o=";
        public const string NotificationHubName = "cvxitcamaocevents";
    }
    public static class MessageKeys
    {
        public const string NavigateToEvent = "navigate_event";
        public const string NavigateToSession = "navigate_session";
        public const string NavigateToSpeaker = "navigate_speaker";
        public const string NavigateToSponsor = "navigate_sponsor";
        public const string NavigateToImage = "navigate_image";
        public const string NavigateLogin = "navigate_login";
        public const string Error = "error";
        public const string Connection = "connection";
        public const string LoggedIn = "loggedin";
        public const string Message = "message";
        public const string Question = "question";
        public const string Choice = "choice";
    }

    public static class AzureB2CCoordinates
    {                        
        public const string Tenant = "chevronitcama.onmicrosoft.com";
        public const string ClientId = "7493eff3-078c-4f58-a5f2-effaa18acbfb";
        public const string PolicySignUpSignIn = "B2C_1_AMAOCAppSignUp";
        public const string PolicyEditProfile = "B2C_1_AMAOCAppEditProfile";
        public const string PolicyResetPassword = "B2C_1_AMAOCAppPassReset";
        public const string DefaultPolicy = PolicySignUpSignIn;
        public const string RedirectUri = "com.onmicrosoft.chevronitcama.amaocapp://auth";

        public static string[] Scopes = { "https://chevronitcama.onmicrosoft.com/amaocapp/read" };

        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";
    }

    public static class AppConfig
    {
        public const string AzureMobileAppUrl = "https://cvxitcamaocapp.azurewebsites.net";
    }
}
