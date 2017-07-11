using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Stores
{
    public class EventAttendeeStore : BaseStore<EventAttendee>, IEventAttendeeStore
    {
        public override string Identifier => "EventAttendee";
        public EventAttendeeStore()
        {

        }

        public async Task<bool> IsAttended(string eventId)
        {
            await InitializeStore().ConfigureAwait(false);
            var items = await Table.Where(e => e.EventId == eventId).ToListAsync().ConfigureAwait(false);
            return items.Count > 0;
        }
    }
}
