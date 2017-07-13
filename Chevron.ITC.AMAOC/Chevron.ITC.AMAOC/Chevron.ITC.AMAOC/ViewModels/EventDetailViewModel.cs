using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Models;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class EventDetailViewModel : BaseViewModel
    {
        public Event Event { get; set; }
        public EventDetailViewModel(Event ocEvent = null)
        {
           Event = ocEvent;
        }        
    }
}