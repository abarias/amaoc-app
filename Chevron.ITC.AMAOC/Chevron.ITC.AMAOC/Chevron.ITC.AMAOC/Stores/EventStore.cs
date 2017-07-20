using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Stores
{
    public class EventStore : BaseStore<Event>, IEventStore
    {
        public override async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore().ConfigureAwait(false);
            if (forceRefresh)
                await PullLatestAsync().ConfigureAwait(false);

            var events = await Table.OrderBy(s => s.StartTime).ToListAsync().ConfigureAwait(false);
            var eventAttendeeStore = DependencyService.Get<IEventAttendeeStore>();
            await eventAttendeeStore.GetItemsAsync(true).ConfigureAwait(false);            

            foreach (var ev in events)
            {
                string statusImage = "check.png";
                var isAttended = await eventAttendeeStore.IsAttended(ev.Id).ConfigureAwait(false);
                if (!isAttended && ev.IsCompleted)
                    statusImage = "minus.png";
                else if (!isAttended && !ev.IsCompleted)
                    statusImage = "detail.png";

                ev.StatusImage = statusImage;
            }

            return events;
        
        }

        public Task<Event> GetAppIndexEvent(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetNextEvents()
        {
            throw new NotImplementedException();
        }

        public override string Identifier => "Event";
        public EventStore()
        {

        }
    }
}
