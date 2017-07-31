using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class EventAttendeeStore : BaseStore<EventAttendee>, IEventAttendeeStore
    {
        List<EventAttendee> EventAttendees { get; }

        public EventAttendeeStore()
        {
            EventAttendees = new List<EventAttendee>();
        }

        public Task<bool> IsAttended(string eventId)
        {
            return Task.FromResult(Settings.IsEventAttended(eventId));
        }

        public override Task<bool> InsertAsync(EventAttendee item)
        {
            Settings.AttendEvent(item.EventId);
            return Task.FromResult(true);
        }

        public override async Task<IEnumerable<EventAttendee>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(EventAttendees);
            return EventAttendees;
        }

        public override async Task InitializeStore()
        {
            if (EventAttendees.Count != 0)
                return;

            EventAttendees.Add(new EventAttendee
            {
                EmployeeId = "001",
                EventId = "005"
            });

            EventAttendees.Add(new EventAttendee
            {
                EmployeeId = "001",
                EventId = "006"
            });
        }

        public async Task DropAttended()
        {
           await Settings.ClearAttended();
        }
    }
}
