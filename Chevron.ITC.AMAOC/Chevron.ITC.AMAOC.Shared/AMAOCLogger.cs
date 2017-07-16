using System;
using System.Collections.Generic;
using System.Text;
using Chevron.ITC.AMAOC;
using Chevron.ITC.AMAOC.Interfaces;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections;

[assembly: Dependency(typeof(AMAOCLogger))]
namespace Chevron.ITC.AMAOC
{
    public class AMAOCLogger : ILogger
    {
        bool enableHockeyApp = false;

        #region ILogger implementation

        public virtual void TrackPage(string page, string id = null)
        {
            Debug.WriteLine("AMA OC Logger: TrackPage: " + page.ToString() + " Id: " + id ?? string.Empty);

            if (!enableHockeyApp)
                return;
#if __ANDROID__

            HockeyApp.Android.Metrics.MetricsManager.TrackEvent($"{page}Page");
#elif __IOS__
            HockeyApp.MetricsManager.TrackEvent($"{page}Page");
#endif
        }


        public virtual void Track(string trackIdentifier)
        {
            Debug.WriteLine("AMA OC Logger: Track: " + trackIdentifier);

            if (!enableHockeyApp)
                return;

#if __ANDROID__
            HockeyApp.Android.Metrics.MetricsManager.TrackEvent(trackIdentifier);
#elif __IOS__
            HockeyApp.MetricsManager.TrackEvent(trackIdentifier);
#endif
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
            Debug.WriteLine("AMA OC Logger: Track: " + trackIdentifier + " key: " + key + " value: " + value);

            if (!enableHockeyApp)
                return;

            trackIdentifier = $"{trackIdentifier}-{key}-{@value}";

#if __ANDROID__
            HockeyApp.Android.Metrics.MetricsManager.TrackEvent(trackIdentifier);
#elif __IOS__
            HockeyApp.MetricsManager.TrackEvent(trackIdentifier);
#endif
        }

        public virtual void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("AMA OC Logger: Report: " + exception);

        }
        public virtual void Report(Exception exception, IDictionary extraData, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("AMA OC Logger: Report: " + exception);
        }
        public virtual void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("AMA OC Logger: Report: " + exception + " key: " + key + " value: " + value);
        }
        #endregion
    }

}
