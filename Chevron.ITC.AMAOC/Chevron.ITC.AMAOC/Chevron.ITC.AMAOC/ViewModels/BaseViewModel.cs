using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;

using Xamarin.Forms;
using Chevron.ITC.AMAOC.Abstractions;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public IBaseStore<Event> DataStore => DependencyService.Get<IBaseStore<Event>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}

