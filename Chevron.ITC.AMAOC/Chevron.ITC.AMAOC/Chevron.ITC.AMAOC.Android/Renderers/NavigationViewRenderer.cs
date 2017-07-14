using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Chevron.ITC.AMAOC.Droid;
using Android.Support.Design.Widget;
using FormsToolkit;


[assembly: ExportRenderer(typeof(Chevron.ITC.AMAOC.NavigationView), typeof(NavigationViewRenderer))]
namespace Chevron.ITC.AMAOC.Droid
{
    public class NavigationViewRenderer : ViewRenderer<Chevron.ITC.AMAOC.NavigationView, NavigationView>
    {
        NavigationView navView;
        ImageView profileImage;
        TextView profileName;
        protected override void OnElementChanged(ElementChangedEventArgs<Chevron.ITC.AMAOC.NavigationView> e)
        {

            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            var view = Inflate(Forms.Context, Resource.Layout.nav_view, null);
            navView = view.JavaCast<NavigationView>();


            navView.NavigationItemSelected += NavView_NavigationItemSelected;

            Settings.Current.PropertyChanged += SettingsPropertyChanged;
            SetNativeControl(navView);

            var header = navView.GetHeaderView(0);
            profileImage = header.FindViewById<ImageView>(Resource.Id.profile_image);
            profileName = header.FindViewById<TextView>(Resource.Id.profile_name);

            profileImage.Click += (sender, e2) => NavigateToLogin();
            profileName.Click += (sender, e2) => NavigateToLogin();

            UpdateName();
            UpdateImage();

            navView.SetCheckedItem(Resource.Id.nav_feed);
        }

        void NavigateToLogin()
        {
            if (Settings.Current.IsLoggedIn)
                return;

            XamarinEvolve.Clients.UI.App.Logger.TrackPage(AppPage.Login.ToString(), "navigation");
            MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
        }

        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.Current.Email))
            {
                UpdateName();
                UpdateImage();
            }
        }

        void UpdateName()
        {
            profileName.Text = Settings.Current.UserDisplayName;
        }

        void UpdateImage()
        {
            Koush.UrlImageViewHelper.SetUrlDrawable(profileImage, Settings.Current.UserAvatar, Resource.Drawable.profile_generic);
        }

        public override void OnViewRemoved(Android.Views.View child)
        {
            base.OnViewRemoved(child);
            navView.NavigationItemSelected -= NavView_NavigationItemSelected;
            Settings.Current.PropertyChanged -= SettingsPropertyChanged;
        }

        IMenuItem previousItem;

        void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {


            if (previousItem != null)
                previousItem.SetChecked(false);

            navView.SetCheckedItem(e.MenuItem.ItemId);

            previousItem = e.MenuItem;

            int id = 0;
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_feed:
                    id = (int)AppPage.Feed;
                    break;
                case Resource.Id.nav_sessions:
                    id = (int)AppPage.Sessions;
                    break;
                case Resource.Id.nav_events:
                    id = (int)AppPage.Events;
                    break;
                case Resource.Id.nav_sponsors:
                    id = (int)AppPage.Sponsors;
                    break;
                case Resource.Id.nav_venue:
                    id = (int)AppPage.Venue;
                    break;
                case Resource.Id.nav_floor_map:
                    id = (int)AppPage.FloorMap;
                    break;
                case Resource.Id.nav_conference_info:
                    id = (int)AppPage.ConferenceInfo;
                    break;
                case Resource.Id.nav_mini_hacks:
                    id = (int)AppPage.MiniHacks;
                    break;
                case Resource.Id.nav_settings:
                    id = (int)AppPage.Settings;
                    break;
                case Resource.Id.nav_evals:
                    id = (int)AppPage.Evals;
                    break;
            }
            this.Element.OnNavigationItemSelected(new XamarinEvolve.Clients.UI.NavigationItemSelectedEventArgs
            {

                Index = id
            });
        }

    }