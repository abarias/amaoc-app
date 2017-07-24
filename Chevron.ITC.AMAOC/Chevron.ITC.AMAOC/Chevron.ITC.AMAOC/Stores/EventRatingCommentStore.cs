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
    public class EventRatingCommentStore : BaseStore<EventRatingComment>, IEventRatingCommentStore
    {
        public async Task<bool> LeftFeedback(Event ocEvent)
        {
            await InitializeStore();
            var items = await Table.Where(s => s.EventId == ocEvent.Id).ToListAsync().ConfigureAwait(false);
            return items.Count > 0;
        }

        public Task DropFeedback()
        {
            return Task.FromResult(true);
        }



        public override string Identifier => "EventRatingComment";
    }
}
