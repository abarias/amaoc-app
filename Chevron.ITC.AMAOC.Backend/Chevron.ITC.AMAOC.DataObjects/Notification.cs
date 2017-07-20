using System;
using System.Collections.Generic;
using System.Text;

namespace Chevron.ITC.AMAOC.DataObjects
{
    public class Notification : BaseDataObject
    {
        public string Text { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
