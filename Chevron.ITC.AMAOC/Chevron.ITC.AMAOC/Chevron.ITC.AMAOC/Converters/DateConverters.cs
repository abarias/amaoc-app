using System;
using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;
using System.Globalization;
using System.Diagnostics;

namespace Chevron.ITC.AMAOC.Converters
{
    class EventDateDisplayConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var featured = value as Event;
                if (featured == null)
                    return string.Empty;

                return featured.GetDisplayName();
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

    class EventTimeDisplayConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var featured = value as Event;
                if (featured == null)
                    return string.Empty;

                return featured.GetDisplayTime();
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

    class EventDayNumberDisplayConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DateTime))
                    return string.Empty;

                return ((DateTime)value).ToEasternTimeZone().Day;
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

    class EventDayDisplayConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DateTime))
                    return string.Empty;

                return ((DateTime)value).ToEasternTimeZone().DayOfWeek.ToString().Substring(0, 3).ToUpperInvariant();
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
