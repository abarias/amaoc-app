using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class NotificationStore : BaseStore<Notification>, INotificationStore
    {
        public NotificationStore()
        {
        }

        public async Task<Notification> GetLatestNotification()
        {
            var items = await GetItemsAsync();
            return items.ElementAt(0);
        }

        public override Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
        {
            var items = new[]
            {
                new Notification
                {
                    Date = DateTime.UtcNow,
                    Text = "Welcome to AMA OC Event Tracker!"
                }
            };
            return Task.FromResult(items as IEnumerable<Notification>);
        }
    }
}
