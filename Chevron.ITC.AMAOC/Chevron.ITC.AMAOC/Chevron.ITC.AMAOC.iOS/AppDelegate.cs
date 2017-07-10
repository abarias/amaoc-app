using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.Identity.Client;

namespace Chevron.ITC.AMAOC.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            SQLitePCL.CurrentPlatform.Init();
            LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }
    }
}
