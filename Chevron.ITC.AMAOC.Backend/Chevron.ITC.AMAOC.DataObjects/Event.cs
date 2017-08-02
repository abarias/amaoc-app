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

        public bool IsCompleted { get; set; }

        public string CreatedByEmployeeId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public virtual Employee CreatedByEmployee { get; set; }

        public virtual ICollection<EventAttendee> Attendees { get; set; }

#if MOBILE
        string statusImage;
        [Newtonsoft.Json.JsonIgnore]
        public string StatusImage
        {
            get { return statusImage; }
            set
            {
                SetProperty(ref statusImage, value);
            }
        }

        bool isAttended;
        [Newtonsoft.Json.JsonIgnore]
        public bool IsAttended
        {
            get { return isAttended; }
            set
            {
                SetProperty(ref isAttended, value);
            }
        }

        bool feedbackLeft;
        [Newtonsoft.Json.JsonIgnore]
        public bool FeedbackLeft
        {
            get { return feedbackLeft; }
            set
            {
                SetProperty(ref feedbackLeft, value);
            }
        }

        EventStatus eventStatus;
        [Newtonsoft.Json.JsonIgnore]
        public EventStatus OCEventStatus
        {
            get { return eventStatus; }
            set
            {
                SetProperty(ref eventStatus, value);
            }
        }

        public enum EventStatus
        {
            Completed,
            Missed,
            NotStarted
        } 
        
        public enum EventViewState
        {
            NotStarted,
            Missed,            
            Today,
            AttendedNoFeedback,
            AttendedHasFeedback
        }
#endif
    }
}
