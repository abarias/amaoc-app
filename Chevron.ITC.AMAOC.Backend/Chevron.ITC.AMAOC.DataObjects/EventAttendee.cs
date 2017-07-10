using System;
namespace Chevron.ITC.AMAOC.DataObjects
{
    public class EventAttendee : BaseDataObject
    {
        public string EventId { get; set; }

        public string EmployeeId { get; set; }

        public bool hasGivenFeedback { get; set; }

        public virtual Event Event { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
