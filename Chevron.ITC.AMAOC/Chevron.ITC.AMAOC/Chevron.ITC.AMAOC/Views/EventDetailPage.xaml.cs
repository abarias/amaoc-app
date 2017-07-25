
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Helpers;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Services;
using Chevron.ITC.AMAOC.ViewModels;
using FormsToolkit;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Chevron.ITC.AMAOC.Views
{
    public partial class EventDetailPage : ContentPage
    {        
        EventDetailViewModel ViewModel => vm ?? (vm = BindingContext as EventDetailViewModel);
        EventDetailViewModel vm;
        ZXingScannerPage scanPage;
        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public EventDetailPage()
        {
            InitializeComponent();
        }

        public EventDetailPage(Event ocEvent)
        {
            InitializeComponent();

            ButtonAttendance.Clicked += ButtonAttendance_Clicked;

            scanPage = new ZXingScannerPage(new MobileBarcodeScanningOptions { AutoRotate = false, })
            {
                DefaultOverlayTopText = "Align the barcode within the frame",
                DefaultOverlayBottomText = string.Empty
            };

            scanPage.OnScanResult += ScanPage_OnScanResult;


            scanPage.Title = "Scan Code";


            var item = new ToolbarItem
            {
                Text = "Cancel",
                Command = new Command(async () =>
                {
                    scanPage.IsScanning = false;
                    await Navigation.PopAsync();
                })
            };

            if (Device.OS != TargetPlatform.iOS)
                item.Icon = "toolbar_close.png";

            scanPage.ToolbarItems.Add(item);

            ButtonFeedback.Clicked += async (sender, e) =>
            {                
                await NavigationService.PushModalAsync(Navigation, new AMAOCNavigationPage(new EventFeedbackPage(ViewModel.Event)));
            };
            BindingContext = new EventDetailViewModel(Navigation, ocEvent);
            ViewModel.LoadEventCommand.Execute(null);
        }

        async void ButtonAttendance_Clicked(object sender, EventArgs e)
        {

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            bool request = false;
            if (status == PermissionStatus.Denied)
            {
                if (Device.OS == TargetPlatform.iOS)
                {
                    MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                    {
                        Title = "Camera Permission",
                        Question = "To confirm attendance and gain points you will need to scan a code and the camera permission is required. Please go into Settings and turn on Camera for AMA OC Event Tracker.",
                        Positive = "Settings",
                        Negative = "Maybe Later",
                        OnCompleted = (result) =>
                        {
                            if (result)
                            {
                                DependencyService.Get<IPushNotifications>().OpenSettings();
                            }
                        }
                    });
                    return;
                }
                else
                {
                    request = true;
                }
            }

            if (request || status != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                if (newStatus.ContainsKey(Permission.Camera) && newStatus[Permission.Camera] != PermissionStatus.Granted)
                {
                    MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                    {
                        Title = "Camera Permission",
                        Question = "To confirm attendance and gain points you will need to scan a code and the camera permission is required.",
                        Positive = "Settings",
                        Negative = "Maybe Later",
                        OnCompleted = (result) =>
                        {
                            if (result)
                            {
                                DependencyService.Get<IPushNotifications>().OpenSettings();
                            }
                        }
                    });
                    return;
                }
            }




            await Navigation.PushAsync(scanPage);
        }

        void ScanPage_OnScanResult(ZXing.Result result)
        {
            // Stop scanning
            scanPage.IsScanning = false;
            // Pop the page and show the result
            Device.BeginInvokeOnMainThread(async () => {

                await Navigation.PopAsync();

                //admin app will append on evolve to the end to it is unique.
                try
                { 
                    if (vm.Event.Name + "amaoc" == result.Text)
                    {
                        await vm.AttendEvent();
                        App.Logger.Track("AttendedEvent", "Name", vm.Event.Name);
                        await DisplayAlert("Congratulations", "You have confirmed your attendance for this event!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Event Issue", "That doesn't seem to be the right code. Please see your speaker to confirm your attendance.", "OK");
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}
