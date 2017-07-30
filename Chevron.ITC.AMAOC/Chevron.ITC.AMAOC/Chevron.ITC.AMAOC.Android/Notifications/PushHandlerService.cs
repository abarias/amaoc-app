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
using Gcm;
using WindowsAzure.Messaging;
using Android.Util;
using Chevron.ITC.AMAOC.Helpers;
using Android.Support.V4.App;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is needed only for Android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace Chevron.ITC.AMAOC.Droid
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE },
    Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
    Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
    Categories = new string[] { "@PACKAGE_NAME@" })]
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        public static string[] SENDER_IDS = new string[] { ApiKeys.SenderID };

        public const string TAG = "MyBroadcastReceiver-GCM";
    }

    [Service] // Must use the service tag
    public class PushHandlerService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }
        private NotificationHub Hub { get; set; }

        public PushHandlerService() : base(ApiKeys.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "PushHandlerService() constructor");
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;            

            Hub = new NotificationHub(ApiKeys.NotificationHubName, ApiKeys.ListenConnectionString,
                                        context);

            //var tags = new List<string>() { "falcons" }; // create tags if you want
            var tags = new List<string>() { };

            try
            {
                var hubRegistration = Hub.Register(registrationId, tags.ToArray());
                Settings.Current.PushRegistered = true;
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GCM Message Received!");

            try
            { 
                var msg = new StringBuilder();

                if (intent != null && intent.Extras != null)
                {
                    foreach (var key in intent.Extras.KeySet())
                        msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
                }

                string messageText = intent.Extras.GetString("message");
                if (!string.IsNullOrEmpty(messageText))                
                    createNotification(messageText);
                
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }
        }

        void createNotification(string message)
        {
            try
            {
                Console.WriteLine("SendNotification");
                var notificationManager = NotificationManagerCompat.From(this);

                Console.WriteLine("Created Manager");
                var notificationIntent = new Intent(this, typeof(MainActivity));
                notificationIntent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

                Console.WriteLine("Created Pending Intent");
                /*var wearableExtender =
                    new NotificationCompat.WearableExtender()
                        .SetBackground(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ic_background_evolve));*/

                var style = new NotificationCompat.BigTextStyle();
                style.BigText(message);

                var builder = new NotificationCompat.Builder(this)
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle("AMA OC Event Tracker")
                    .SetAutoCancel(true)
                    .SetStyle(style)
                    .SetSmallIcon(Resource.Drawable.ic_notification)
                    .SetContentText(message);
                //.Extend(wearableExtender);

                // Obtain a reference to the NotificationManager
                var id = Chevron.ITC.AMAOC.Droid.Helpers.Settings.GetUniqueNotificationId();
                Console.WriteLine("Got Unique ID: " + id);
                var notif = builder.Build();
                notif.Defaults = NotificationDefaults.All;
                Console.WriteLine("Notify");
                notificationManager.Notify(id, notif);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            //Create notification
            //var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            ////Create an intent to show UI
            //var uiIntent = new Intent(this, typeof(MainActivity));

            ////Create the notification
            //var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            ////Auto-cancel will remove the notification once the user touches it
            //notification.Flags = NotificationFlags.AutoCancel;

            ////Set the notification info
            ////we use the pending intent, passing our ui intent over, which will get called
            ////when the notification is tapped.
            //notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            ////Show the notification
            //notificationManager.Notify(1, notification);
            //dialogNotify(title, desc);
        }

        protected void dialogNotify(String title, String message)
        {

            MainActivity.CurrentActivity.RunOnUiThread(() => {
                AlertDialog.Builder dlg = new AlertDialog.Builder(MainActivity.CurrentActivity);
                AlertDialog alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Ok", delegate {
                    alert.Dismiss();
                });
                alert.SetMessage(message);
                alert.Show();
            });
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            try
            {
                if (Hub != null)
                    Hub.Unregister();
                    Log.Verbose(MyBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to unregister" + ex);
                Log.Error(MyBroadcastReceiver.TAG, "Unable to unregister" + ex);
            }
            
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(MyBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(MyBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }
    }
}