using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class EventRatingCommentStore : BaseStore<EventRatingComment>, IEventRatingCommentStore
    {
        public Task<bool> LeftFeedback(Event ocEvent)
        {
            return Task.FromResult(Settings.LeftFeedback(ocEvent.Id));
        }

        public async Task DropFeedback()
        {
            await Settings.ClearFeedback();
        }

        public override Task<bool> InsertAsync(EventRatingComment item)
        {
            Settings.LeaveFeedback(item.EventId, true);
            return Task.FromResult(true);
        }

        public override Task<bool> RemoveAsync(EventRatingComment item)
        {
            Settings.LeaveFeedback(item.EventId, false);
            return Task.FromResult(true);
        }
    }
}
