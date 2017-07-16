using System;
using System.Collections.Generic;
using System.Text;

namespace Chevron.ITC.AMAOC.DataObjects
{
    public class EventRatingComment : BaseDataObject
    {
        public string EmployeeId { get; set; }
        public string EventId { get; set; }
        public int EventRating { get; set; }
        public string EventComment { get; set; }
    }
}
