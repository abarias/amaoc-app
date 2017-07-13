using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using MvvmHelpers;
using System.Text;
using System.Threading.Tasks;


namespace Chevron.ITC.AMAOC.Extensions
{
    public static class EventExtensions
    {
        public static IEnumerable<Grouping<string, Event>> GroupByDate(this IEnumerable<Event> events)
        {
            return from e in events
                   orderby e.StartTime
                   group e by e.GetSortName()
                into eventGroup
                   select new Grouping<string, Event>(eventGroup.Key, eventGroup);
        }

        public static string GetSortName(this Event e)
        {
            if (!e.StartTime.HasValue || !e.EndTime.HasValue)
                return "TBA";

            var start = e.StartTime.Value;

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                    return $"Today";

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                    return $"Tomorrow";
            }
            var monthDay = start.ToString("M");
            return $"{monthDay}";
        }

        public static string GetDisplayName(this Event e)
        {
            if (e.OCEventStatus == Event.EventStatus.Missed)
                return $"It's ok, there are other events to attend!";

            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTBA())
                return "To be announced";

            var start = e.StartTime.Value;            

            var startString = start.ToString("t");
            var end = e.EndTime.Value;
            var endString = end.ToString("t");

            var day = start.DayOfWeek.ToString();
            var monthDay = start.ToString("M");
            var fullDate = start.ToString("MMMM dd, yyyy");

            if (e.OCEventStatus == Event.EventStatus.Completed)            
                return $"Completed on {fullDate}";

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                    return $"Today {startString}–{endString}";

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                    return $"Tomorrow {startString}–{endString}";
            }
                      
            return $"{day}, {monthDay}, {startString}–{endString}";
        }




        public static string GetDisplayTime(this Event e)
        {

            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTBA())
                return "To be announced";

            var start = e.StartTime.Value;


            var startString = start.ToString("t");
            var end = e.EndTime.Value;
            var endString = end.ToString("t");

            return $"{startString}–{endString}";
        }
    }
}
