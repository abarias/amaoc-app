using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC
{
    class EventNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var ocEvent = value as Event;
                switch (ocEvent.OCEventStatus)
                {
                    case Event.EventStatus.Completed:
                        return "You have completed " + ocEvent.Name;
                    case Event.EventStatus.Missed:
                        return "You missed the event " + ocEvent.Name;
                    case Event.EventStatus.NotStarted:
                        if (DateTime.Today.DayOfYear == ocEvent.StartTime?.DayOfYear)
                        {
                            return ocEvent.Name + " is scheduled today!";
                        }
                        else
                        {
                            return ocEvent.Name + " is scheduled on";
                        }
                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class EventLocationVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((Event.EventStatus)value == Event.EventStatus.Completed ||
                    (Event.EventStatus)value == Event.EventStatus.Missed)
                {
                    return false;
                }
                    
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class EventPointsLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventStatus = (Event.EventStatus)value;
            try
            {
                switch (eventStatus)
                {
                    case Event.EventStatus.Completed:
                        return $"Points gained: ";
                    case Event.EventStatus.NotStarted:
                        return $"Possible points: ";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
