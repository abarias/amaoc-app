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
    public class EventStore : BaseStore<Event>, IEventStore
    {
        public override string Identifier => "Event";
        public EventStore()
        {

        }
    }
}
