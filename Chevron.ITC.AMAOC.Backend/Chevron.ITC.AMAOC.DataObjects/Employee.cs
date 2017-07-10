using System;
using System.Collections.Generic;

namespace Chevron.ITC.AMAOC.DataObjects
{
    public class Employee : BaseDataObject
    {
        public string FullName { get; set; }
        
        public string UserId { get; set; }

        public string CAI { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }

        public int TotalPointsEarned { get; set; }

        public virtual FeedbackAnswerFreeText EventComments { get; set; }

        public virtual ICollection<EventAttendee> AttendedEvents { get; set; }

        public virtual ICollection<FeedbackAnswer> FeedbackAnswers { get; set; }

        public Employee()
        {
            this.AttendedEvents = new List<EventAttendee>();
            this.FeedbackAnswers = new List<FeedbackAnswer>();
        }
    }
}
