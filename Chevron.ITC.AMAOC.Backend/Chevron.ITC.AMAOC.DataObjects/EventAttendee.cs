using System;
namespace Chevron.ITC.AMAOC.DataObjects
{
    public class EventAttendee
    {
        public int EventId { get; set; }

        public int EmployeeId { get; set; }

        public bool hasGivenFeedback { get; set; }

        public virtual Event Event { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
