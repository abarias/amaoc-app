using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Abstractions
{
    public interface IEventStore : IBaseStore<Event>
    {
        Task<Event> GetAppIndexEvent(string id);
    }
}
