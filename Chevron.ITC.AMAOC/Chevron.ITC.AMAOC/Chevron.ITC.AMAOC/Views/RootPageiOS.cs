using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Helpers;
using FormsToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Views
{
    public class RootPageiOS : TabbedPage
    {
        public RootPageiOS()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            Children.Add(new AMAOCNavigationPage(new FeedPage()));            
            Children.Add(new AMAOCNavigationPage(new EventsPage()));            
            Children.Add(new AMAOCNavigationPage(new SettingsPage()));

            MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
            {
                switch (p.Page)
                {
                    case AppPage.Notification:
                        NavigateAsync(AppPage.Notification);
                        await CurrentPage.Navigation.PopToRootAsync();
                        await CurrentPage.Navigation.PushAsync(new NotificationsPage());
                        break;
                    case AppPage.Events:
                        NavigateAsync(AppPage.Events);
                        await CurrentPage.Navigation.PopToRootAsync();
                        break;
                    case AppPage.Event:
                        NavigateAsync(AppPage.Events);
                        var ocEvent = await DependencyService.Get<IEventStore>().GetAppIndexEvent(p.Id);
                        if (ocEvent == null)
                            break;
                        await CurrentPage.Navigation.PushAsync(new EventDetailPage(ocEvent));
                        break;
                }

            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            switch (Children.IndexOf(CurrentPage))
            {
                case 0:
                    App.Logger.TrackPage(AppPage.Feed.ToString());
                    break;                
                case 1:
                    App.Logger.TrackPage(AppPage.Events.ToString());
                    break;                
                case 4:
                    App.Logger.TrackPage(AppPage.Settings.ToString());
                    break;
            }
        }

        public void NavigateAsync(AppPage menuId)
        {
            switch ((int)menuId)
            {
                case (int)AppPage.Feed: CurrentPage = Children[0]; break;
                case (int)AppPage.Events: CurrentPage = Children[1]; break;                
                case (int)AppPage.Notification: CurrentPage = Children[0]; break;
                case (int)AppPage.Settings: CurrentPage = Children[2]; break;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();



            if (Settings.Current.FirstRun)
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
        }
    }
}
