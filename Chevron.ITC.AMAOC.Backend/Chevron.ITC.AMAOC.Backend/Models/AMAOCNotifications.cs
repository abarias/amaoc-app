using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Chevron.ITC.AMAOC.Backend.Models
{
    public class AMAOCNotifications
    {
        public static AMAOCNotifications Instance = new AMAOCNotifications();

        public NotificationHubClient Hub { get; }

        public AMAOCNotifications()
        {
            //Hub = NotificationHubClient.CreateClientFromConnectionString(ConfigurationManager.AppSettings["HubConnection"], ConfigurationManager.AppSettings["HubEndpiont"]);
            Hub = NotificationHubClient.CreateClientFromConnectionString(ConfigurationManager.ConnectionStrings["MS_NotificationHubConnectionString"].ConnectionString, ConfigurationManager.AppSettings["HubEndpoint"]);
        }
    }
}