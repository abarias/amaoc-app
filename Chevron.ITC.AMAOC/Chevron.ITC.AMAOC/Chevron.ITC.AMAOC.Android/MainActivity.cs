using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Identity.Client;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using FormsToolkit.Droid;
using Chevron.ITC.AMAOC;
using Chevron.ITC.AMAOC.Helpers;
using Plugin.Permissions;
using Refractored.XamForms.PullToRefresh.Droid;
using System.Reflection;
using Gcm;
using Android.Gms.Common;

namespace Chevron.ITC.AMAOC.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Toolkit.Init();

            PullToRefreshLayoutRenderer.Init();
            typeof(Color).GetProperty("Accent", BindingFlags.Public | BindingFlags.Static).SetValue(null, Color.FromHex("#757575"));

            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

#if !ENABLE_TEST_CLOUD
            InitializeHockeyApp();
#endif

            LoadApplication(new App());

            var gpsAvailable = IsPlayServicesAvailable();
            Settings.Current.PushNotificationsEnabled = gpsAvailable;            

            App.UiParent = new UIParent(Xamarin.Forms.Forms.Context as Activity);

            if (!Settings.Current.PushNotificationsEnabled)
                return;
#if ENABLE_TEST_CLOUD
#else
            RegisterWithGCM();
#endif

            DataRefreshService.ScheduleRefresh(this);
        }

        void InitializeHockeyApp()
        {
            if (string.IsNullOrWhiteSpace(ApiKeys.HockeyAppAndroid) || ApiKeys.HockeyAppAndroid == nameof(ApiKeys.HockeyAppAndroid))
                return;


            HockeyApp.Android.CrashManager.Register(this, ApiKeys.HockeyAppAndroid);
            //HockeyApp.Android.UpdateManager.Register(this, ApiKeys.HockeyAppAndroid);

            HockeyApp.Android.Metrics.MetricsManager.Register(Application, ApiKeys.HockeyAppAndroid);

        }

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            System.Diagnostics.Debug.WriteLine("MainActivity", "Registering...");
            GcmService.Initialize(this);
            GcmService.Register(this);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    if (Settings.Current.GooglePlayChecked)
                        return false;

                    Settings.Current.GooglePlayChecked = true;
                    Toast.MakeText(this, "Google Play services is not installed, push notifications have been disabled.", ToastLength.Long).Show();
                }
                else
                {
                    Settings.Current.PushNotificationsEnabled = false;
                }
                return false;
            }
            else
            {
                Settings.Current.PushNotificationsEnabled = true;
                return true;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
