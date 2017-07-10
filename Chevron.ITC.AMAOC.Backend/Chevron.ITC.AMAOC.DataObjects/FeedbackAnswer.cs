using System;
namespace Chevron.ITC.AMAOC.DataObjects
{
    public class FeedbackAnswer : BaseDataObject
    {
        public string EmployeeId { get; set; }

        public string FeedbackQuestionId { get; set; }

        public string Answer { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual FeedbackQuestion FeedbackQuestion { get; set; }
    }
}
