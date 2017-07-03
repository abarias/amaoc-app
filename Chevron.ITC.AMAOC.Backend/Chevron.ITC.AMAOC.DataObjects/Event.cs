using System;
using System.Collections.Generic;

namespace Chevron.ITC.AMAOC.DataObjects
{
    public class Event : BaseDataObject
    {
        public Event() {
            this.Attendees = new List<EventAttendee>();
        }

        public string Name { get; set; }

        public string Abstract { get; set; }

        public int Points { get; set; }

        public string Location { get; set; }

        public int CreatedByEmployeeId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public virtual Employee CreatedByEmployee { get; set; }

        public virtual ICollection<EventAttendee> Attendees { get; set; }
    }
}
