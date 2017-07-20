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
    public class RootPageAndroid : MasterDetailPage
    {
        Dictionary<int, AMAOCNavigationPage> pages;
        DeepLinkPage page;
        bool isRunning = false;

        public RootPageAndroid()
        {
            pages = new Dictionary<int, AMAOCNavigationPage>();
            Master = new MenuPage(this);

            pages.Add(0, new AMAOCNavigationPage(new FeedPage()));

            Detail = pages[0];
            MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
            {
                page = p;

                if (isRunning)
                    await GoToDeepLink();
            });
        }

        public async Task NavigateAsync(int menuId)
        {
            AMAOCNavigationPage newPage = null;
            if (!pages.ContainsKey(menuId))
            {
                //only cache specific pages
                switch (menuId)
                {
                    case (int)AppPage.Feed: //Feed
                        pages.Add(menuId, new AMAOCNavigationPage(new FeedPage()));
                        break;
                    case (int)AppPage.Events://events
                        pages.Add(menuId, new AMAOCNavigationPage(new EventsPage()));
                        break;
                    //case (int)AppPage.EventsInfo://Mini-Hacks
                    //    newPage = new AMAOCNavigationPage(new MiniHacksPage());
                    //    break;
                    //case (int)AppPage.://sponsors
                    //    newPage = new AMAOCNavigationPage(new SponsorsPage());
                    //    break;
                    //case (int)AppPage.Venue: //venue
                    //    newPage = new AMAOCNavigationPage(new VenuePage());
                    //    break;
                    //case (int)AppPage.ConferenceInfo://Conference info
                    //    newPage = new AMAOCNavigationPage(new ConferenceInformationPage());
                    //    break;
                    //case (int)AppPage.FloorMap://Floor Maps
                    //    newPage = new AMAOCNavigationPage(new FloorMapsPage());
                    //    break;
                    //case (int)AppPage.Settings://Settings
                    //    newPage = new AMAOCNavigationPage(new SettingsPage());
                    //    break;
                    //case (int)AppPage.Evals:
                    //    newPage = new AMAOCNavigationPage(new EvaluationsPage());
                    //    break;
                }
            }

            if (newPage == null)
                newPage = pages[menuId];

            if (newPage == null)
                return;

            //if we are on the same tab and pressed it again.
            if (Detail == newPage)
            {
                await newPage.Navigation.PopToRootAsync();
            }

            Detail = newPage;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            if (Settings.Current.FirstRun)
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }

            isRunning = true;

            await GoToDeepLink();

        }
        async Task GoToDeepLink()
        {
            if (page == null)
                return;
            var p = page.Page;
            var id = page.Id;
            page = null;
            switch (p)
            {
                case AppPage.Events:
                    await NavigateAsync((int)AppPage.Events);
                    break;
                case AppPage.Event:
                    await NavigateAsync((int)AppPage.Events);
                    if (string.IsNullOrWhiteSpace(id))
                        break;

                    var ocEvent = await DependencyService.Get<IEventStore>().GetAppIndexEvent(id);
                    if (ocEvent == null)
                        break;
                    await Detail.Navigation.PushAsync(new EventDetailPage(ocEvent));
                    break;
                //case AppPage.Events:
                //    await NavigateAsync((int)AppPage.Events);
                //    break;
                //case AppPage.MiniHacks:
                //    await NavigateAsync((int)AppPage.MiniHacks);
                //    break;
            }

        }
    }
}
