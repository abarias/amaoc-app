using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.iOS;
using Foundation;
using Xamarin.Forms;
using System.Threading.Tasks;
using UIKit;

[assembly: Dependency(typeof(PushNotifications))]
namespace Chevron.ITC.AMAOC.iOS
{
    public class PushNotifications : IPushNotifications
    {
        #region IPushNotifications implementation

        public Task<bool> RegisterForNotifications()
        {
            Settings.Current.PushNotificationsEnabled = true;
            Settings.Current.AttemptedPush = true;

            var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            return Task.FromResult(true);
        }
        public bool IsRegistered
        {
            get
            {
                return UIApplication.SharedApplication.IsRegisteredForRemoteNotifications &&
                    UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None;
            }
        }


        public void OpenSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }
        #endregion
    }
}
