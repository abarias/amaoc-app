using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsTBA(this DateTimeOffset date)
        {

            if (date.Year == DateTime.MinValue.Year)
                return true;

            return false;
        }


        public static string GetSortName(this DateTimeOffset e)
        {           
            if (DateTime.Today.Year == e.Year)
            {
                if (DateTime.Today.DayOfYear == e.DayOfYear)
                    return $"Today";

                if (DateTime.Today.DayOfYear - 1 == e.DayOfYear)
                    return $"Yesterday";

                if (DateTime.Today.DayOfYear + 1 == e.DayOfYear)
                    return $"Tomorrow";
            }
            var monthDay = e.ToString("M");
            return $"{monthDay}";
        }

        public static DateTime GetStartDay(this DateTime date)
        {          
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
    }

}
