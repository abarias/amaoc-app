﻿using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Stores
{
    public class NotificationStore : BaseStore<Notification>, INotificationStore
    {
        public NotificationStore()
        {
        }

        public async Task<Notification> GetLatestNotification()
        {
            var items = await GetItemsAsync(true);
            return items.OrderByDescending(s => s.Date).ElementAt(0);
        }

        public override async Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
        {
            var server = await base.GetItemsAsync(forceRefresh).ConfigureAwait(false);
            if (server.Count() == 0)
            {
                var items = new[]
                    {
                    new Notification
                    {
                        Date = DateTime.UtcNow.AddDays(-2),
                        Text = "Stay tuned for announcements on everything related to AMA OC Events!"
                    }
                };
                return items;
            }
            return server.OrderByDescending(s => s.Date);
        }

        public override string Identifier => "Notification";
    }
}
