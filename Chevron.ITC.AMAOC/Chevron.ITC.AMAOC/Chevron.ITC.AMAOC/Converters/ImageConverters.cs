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
    class EventStatusImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string statusImage = String.Empty;
            try
            {
                var eventStatus = (Event.EventStatus)value;
                switch (eventStatus)
                {
                    case Event.EventStatus.Missed:
                        statusImage = "minus_big.png";
                        break;
                    case Event.EventStatus.NotStarted:
                        statusImage = "detail_big.png";
                        break;                    
                    default:
                        statusImage = "check_big.png";
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert image to URI: " + ex);
            }

            return ImageSource.FromFile(statusImage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class RankingAvatarImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if (value == null)
                return ImageSource.FromFile("profile_generic.png");
            try
            {
                var emp = value as Employee;
                    return ImageSource.FromUri(new Uri(Gravatar.GetURL(emp.Email)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert image to URI: " + ex);
            }

            return ImageSource.FromFile("profile_generic.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
