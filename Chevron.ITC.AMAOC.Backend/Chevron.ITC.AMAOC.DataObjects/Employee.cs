using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        [IgnoreDataMember]
        public virtual FeedbackAnswerFreeText EventComments { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<EventAttendee> AttendedEvents { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FeedbackAnswer> FeedbackAnswers { get; set; }

        public Employee()
        {
            this.AttendedEvents = new List<EventAttendee>();
            this.FeedbackAnswers = new List<FeedbackAnswer>();
        }

#if MOBILE
        int rank;
        [Newtonsoft.Json.JsonIgnore]
        public int Rank
        {
            get { return rank; }
            set
            {
                SetProperty(ref rank, value);
            }
        }

        bool isLoggedInUser;
        [Newtonsoft.Json.JsonIgnore]
        public bool IsLoggedInUser
        {
            get { return isLoggedInUser; }
            set
            {
                SetProperty(ref isLoggedInUser, value);
            }
        }

        int rankCounter;
        [Newtonsoft.Json.JsonIgnore]
        public int RankCounter
        {
            get { return rankCounter; }
            set
            {
                SetProperty(ref rankCounter, value);
            }
        }
#endif
    }
}
