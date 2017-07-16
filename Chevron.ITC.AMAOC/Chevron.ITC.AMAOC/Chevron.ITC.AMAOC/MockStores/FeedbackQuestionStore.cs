using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class FeedbackQuestionStore : BaseStore<FeedbackQuestion>, IFeedbackQuestionStore
    {
        List<FeedbackQuestion> Questions { get; }
        bool initialized = false;

        public FeedbackQuestionStore()
        {
            Questions = new List<FeedbackQuestion>();
        }

        public async Task<IEnumerable<FeedbackQuestion>> GetFeedbackQuestions(string eventId)
        {
            await InitializeStore();

            return Questions.Where(e => e.EventId == eventId).OrderBy(s => s.SortOrder).ToList();
        }

        public override async Task InitializeStore()
        {
            if (initialized)
                return;

            initialized = true;
            if (Questions.Count != 0)
                return;

            Questions.Add(new FeedbackQuestion
            {
                EventId = "005",
                Question = "I will recommend this session and similiar AMA OC info sharing initiatives.",
                SortOrder = 3
            });
            Questions.Add(new FeedbackQuestion
            {
                EventId = "005",
                Question = "The speaker was clear, articulate, and delivered the topic clearly.",
                SortOrder = 2
            });
            Questions.Add(new FeedbackQuestion
            {
                EventId = "005",
                Question = "The event has definitely increased my awareness on the topic.",
                SortOrder = 1
            });
            Questions.Add(new FeedbackQuestion
            {
                EventId = "006",
                Question = "I will recommend this session and similiar AMA OC info sharing initiatives.",
                SortOrder = 3
            });
            Questions.Add(new FeedbackQuestion
            {
                EventId = "006",
                Question = "The speaker was clear, articulate, and delivered the topic clearly.",
                SortOrder = 2
            });
            Questions.Add(new FeedbackQuestion
            {
                EventId = "006",
                Question = "The event has definitely increased my awareness on the topic.",
                SortOrder = 1
            });
        }
    }
}
