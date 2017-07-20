using System;
using Xamarin.Forms;
using Chevron.ITC.AMAOC.DataObjects;
using System.Globalization;
using System.Diagnostics;
using Chevron.ITC.AMAOC.Extensions;
using Humanizer;

namespace Chevron.ITC.AMAOC
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
                if (!(value is DateTimeOffset))
                    return string.Empty;

                return ((DateTimeOffset)value).Day;
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
                if (!(value is DateTimeOffset))
                    return string.Empty;

                return ((DateTimeOffset)value).DayOfWeek.ToString().Substring(0, 3).ToUpperInvariant();
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

    class HumanizeDateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is DateTime)
                {
                    var date = ((DateTime)value);
                    if (date.Kind == DateTimeKind.Local)
                    {
                        return date.Humanize(false);
                    }

                    return date.Humanize();
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
