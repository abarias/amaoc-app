using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Models;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class EventDetailViewModel : BaseViewModel
    {
        public Event Item { get; set; }
        public EventDetailViewModel(Event item = null)
        {
            Title = item.Name;
            Item = item;
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}