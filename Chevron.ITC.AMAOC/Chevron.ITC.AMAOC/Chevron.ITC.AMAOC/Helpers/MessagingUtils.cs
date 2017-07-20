using FormsToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Helpers
{
    public static class MessagingUtils
    {
        public static void SendOfflineMessage()
        {
            MessagingService.Current.SendMessage(MessageKeys.Message, new MessagingServiceAlert
            {
                Title = "Offline",
                Message = "You are currently offline, please connect to the internet and try again.",
                Cancel = "OK"
            });
        }

        public static void SendAlert(string title, string message)
        {
            MessagingService.Current.SendMessage(MessageKeys.Message, new MessagingServiceAlert
            {
                Title = title,
                Message = message,
                Cancel = "OK"
            });
        }
    }
}
