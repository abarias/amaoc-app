using Chevron.ITC.AMAOC.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.MockStores
{
    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static bool LeftFeedback(string id) =>
        AppSettings.GetValueOrDefault<bool>("feed_" + id, false);

        public static void LeaveFeedback(string id, bool leave) =>
        AppSettings.AddOrUpdateValue("feed_" + id, leave);

        public static async Task ClearFeedback()
        {
            var events = await DependencyService.Get<IEventStore>().GetItemsAsync();
            foreach (var ocEvent in events)
                AppSettings.Remove("feed_" + ocEvent.Id);
        }

    }
}
