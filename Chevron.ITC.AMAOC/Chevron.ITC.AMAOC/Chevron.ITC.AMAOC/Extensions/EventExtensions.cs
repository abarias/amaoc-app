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
                   orderby e.StartTimeOrderBy
                   group e by e.GetSortName()
                into eventGroup
                   select new Grouping<string, Event>(eventGroup.Key, eventGroup);
        }

        public static string GetSortName(this Event e)
        {
            if (!e.StartTime.HasValue || !e.EndTime.HasValue)
                return "TBA";

            var start = e.StartTime.Value.ToEasternTimeZone();

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


            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTBA())
                return "To be announced";

            var start = e.StartTime.Value.ToEasternTimeZone();

            if (e.IsAllDay)
                return "All Day";

            var startString = start.ToString("t");
            var end = e.EndTime.Value.ToEasternTimeZone();
            var endString = end.ToString("t");

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                    return $"Today {startString}–{endString}";

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                    return $"Tomorrow {startString}–{endString}";
            }

            var day = start.DayOfWeek.ToString();
            var monthDay = start.ToString("M");
            return $"{day}, {monthDay}, {startString}–{endString}";
        }




        public static string GetDisplayTime(this Event e)
        {

            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTBA())
                return "To be announced";

            var start = e.StartTime.Value.ToEasternTimeZone();


            if (e.IsAllDay)
                return "All Day";

            var startString = start.ToString("t");
            var end = e.EndTime.Value.ToEasternTimeZone();
            var endString = end.ToString("t");

            return $"{startString}–{endString}";
        }
    }
}
