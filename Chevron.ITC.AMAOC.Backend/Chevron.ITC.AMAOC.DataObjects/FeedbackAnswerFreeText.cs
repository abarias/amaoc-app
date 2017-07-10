using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chevron.ITC.AMAOC.DataObjects
{
    public class FeedbackAnswerFreeText : BaseDataObject
    {
        public string EventId { get; set; }

        public string EmployeeId { get; set; }

        public string Comments { get; set; }

        public virtual Event Event { get; set; }       
    }
}
