﻿using System;
namespace Chevron.ITC.AMAOC.DataObjects
{
    public class FeedbackQuestion : BaseDataObject
    {
        public int EventId { get; set; }

        public string Question { get; set; }

        public int SortOrder { get; set; }

        public virtual Event Event { get; set; }
    }
}