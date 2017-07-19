using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.Identity.Client;
using Xamarin.Forms;
using FormsToolkit.iOS;
using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using Refractored.XamForms.PullToRefresh.iOS;

namespace Chevron.ITC.AMAOC.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
            var tint = UIColor.FromRGB(33, 150, 243);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(250, 250, 250); //bar background
            UINavigationBar.Appearance.TintColor = tint; //Tint color of button items

            UIBarButtonItem.Appearance.TintColor = tint; //Tint color of button items

            UITabBar.Appearance.TintColor = tint;

            UISwitch.Appearance.OnTintColor = tint;

            UIAlertView.Appearance.TintColor = tint;

            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = tint;
            //UIView.AppearanceWhenContainedIn(typeof(SLComposeViewController)).TintColor = tint;

#if !ENABLE_TEST_CLOUD
            if (!string.IsNullOrWhiteSpace(ApiKeys.HockeyAppiOS) && ApiKeys.HockeyAppiOS != nameof(ApiKeys.HockeyAppiOS)))
            {
               
                var manager = BITHockeyManager.SharedHockeyManager;
                manager.Configure(ApiKeys.HockeyAppiOS);

                //Disable update manager
                manager.DisableUpdateManager = true;

                manager.StartManager();
                //manager.Authenticator.AuthenticateInstallation();
                   
            }
#endif

            Forms.Init();            
            Toolkit.Init();

            //AppIndexing.SharedInstance.RegisterApp(618319027);

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            SetMinimumBackgroundFetchInterval();

            //Random Inits for Linking out.
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            SQLitePCL.CurrentPlatform.Init();

            ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            NonScrollableListViewRenderer.Initialize();
            SelectedTabPageRenderer.Initialize();
            TextViewValue1Renderer.Init();
            PullToRefreshLayoutRenderer.Init();
            LoadApplication(new App());




            // Process any potential notification data from launch
            //ProcessNotification(options);

            //NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, DidBecomeActive);            		                      

			return base.FinishedLaunching(app, options);
		}

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }

        #region Background Refresh

        private void SetMinimumBackgroundFetchInterval()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
        }

        // Minimum number of seconds between a background refresh this is shorter than Android because it is easily killed off.
        // 20 minutes = 20 * 60 = 1200 seconds
        private const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 1200;

        // Called whenever your app performs a background fetch
        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Do Background Fetch
            var downloadSuccessful = false;
            try
            {
                Xamarin.Forms.Forms.Init();//need for dependency services
                // Download data
                var manager = DependencyService.Get<IStoreManager>();

                downloadSuccessful = await manager.SyncAllAsync(Settings.Current.IsLoggedIn);
            }
            catch (Exception ex)
            {
                var logger = DependencyService.Get<ILogger>();
                ex.Data["Method"] = "PerformFetch";
                logger.Report(ex);
            }

            // If you don't call this, your application will be terminated by the OS.
            // Allows OS to collect stats like data cost and power consumption
            if (downloadSuccessful)
            {
                completionHandler(UIBackgroundFetchResult.NewData);
                Settings.Current.HasSyncedData = true;
                Settings.Current.LastSync = DateTime.UtcNow;
            }
            else
            {
                completionHandler(UIBackgroundFetchResult.Failed);
            }
        }

        #endregion
    }
}
