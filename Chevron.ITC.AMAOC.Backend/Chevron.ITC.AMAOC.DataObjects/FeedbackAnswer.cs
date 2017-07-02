using System;
namespace Chevron.ITC.AMAOC.DataObjects
{
    public class FeedbackAnswer
    {
        public int EmployeeId { get; set; }

        public int FeedbackQuestionId { get; set; }

        public string Answer { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual FeedbackQuestion FeedbackQuestion { get; set; }
    }
}
