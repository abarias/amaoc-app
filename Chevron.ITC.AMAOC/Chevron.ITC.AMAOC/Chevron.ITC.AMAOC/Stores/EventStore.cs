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
                Event.EventStatus eventStatus = Event.EventStatus.Completed;
                var isAttended = await eventAttendeeStore.IsAttended(ev.Id).ConfigureAwait(false);
                if (!isAttended && ev.IsCompleted)
                {
                    statusImage = "minus.png";
                    eventStatus = Event.EventStatus.Missed;
                }
                else if (!isAttended && !ev.IsCompleted)
                {
                    statusImage = "detail.png";
                    eventStatus = Event.EventStatus.NotStarted;
                }

                ev.StatusImage = statusImage;
                ev.OCEventStatus = eventStatus;
            }

            return events;
        
        }

        public async Task<Event> GetAppIndexEvent(string id)
        {
            await InitializeStore().ConfigureAwait(false);
            var events = await Table.Where(s => s.Id == id || s.RemoteId == id).ToListAsync();

            if (events == null || events.Count == 0)
                return null;

            return events[0];
        }

        public async Task<IEnumerable<Event>> GetNextEvents()
        {
            var date = DateTime.UtcNow.AddMinutes(-30);

            var events = await GetItemsAsync().ConfigureAwait(false);

            var results = (from ocEvent in events
                           where (ocEvent.StartTime.HasValue && ocEvent.StartTime.Value > date
                           && !ocEvent.IsCompleted)
                           orderby ocEvent.StartTime.Value
                           select ocEvent).Take(2);


            var enumerable = results as Event[] ?? results.ToArray();
            return !enumerable.Any() ? null : enumerable;
        }

        public override string Identifier => "Event";
        public EventStore()
        {

        }
    }
}
