using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class FeedbackAnswerStore : BaseStore<FeedbackAnswer>, IFeedbackAnswerStore
    {
        public Task<bool> LeftFeedback(Event ocEvent)
        {
            return Task.FromResult(Settings.LeftFeedback(ocEvent.Id));
        }

        public async Task DropFeedback()
        {
            await Settings.ClearFeedback();
        }

        public override Task<bool> InsertAsync(FeedbackAnswer item)
        {
            Settings.LeaveFeedback(item.FeedbackQuestion.EventId, true);
            return Task.FromResult(true);
        }

        public override Task<bool> RemoveAsync(FeedbackAnswer item)
        {
            Settings.LeaveFeedback(item.FeedbackQuestion.EventId, false);
            return Task.FromResult(true);
        }
    }
}
