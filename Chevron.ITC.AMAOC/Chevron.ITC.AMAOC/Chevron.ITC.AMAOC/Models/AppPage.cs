using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }
        public string Id { get; set; }
    }
    public enum AppPage
    {        
        Feed,
        Events,
        Ranking,
        EventsInfo,
        Settings,        
        Login,
        Event,
        EventInfo,
        Notification,
        YammerImage,                                
        Yammer,
        FeedbackRating,
        FeedbackComments
    }
}
