using Android.Widget;
using Chevron.ITC.AMAOC.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Chevron.ITC.AMAOC.Droid;

[assembly: Dependency(typeof(Toaster))]
namespace Chevron.ITC.AMAOC.Droid
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(context, message, ToastLength.Long).Show();
            });

        }
    }
}