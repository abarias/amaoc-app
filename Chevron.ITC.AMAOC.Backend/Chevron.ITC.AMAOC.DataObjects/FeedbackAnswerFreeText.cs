using System;
namespace Chevron.ITC.AMAOC.DataObjects
{
    public class FeedbackAnswerFreeText
    {
        public int EventId { get; set; }

        public int EmployeeId { get; set; }

        public string Comments { get; set; }

        public virtual Event Event { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
