using Chevron.ITC.AMAOC.Backend.Models;
using Chevron.ITC.AMAOC.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Chevron.ITC.AMAOC.Backend.Controllers
{
    [MobileAppController]
    public class AnnouncementController : ApiController
    {
        public async Task<HttpResponseMessage> Post(string password, [FromBody]string message)
        {

            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            if (string.IsNullOrWhiteSpace(message) || password != ConfigurationManager.AppSettings["NotificationsPassword"])
                return Request.CreateResponse(ret);


            try
            {
                var announcement = new Notification
                {
                    Date = DateTime.UtcNow,
                    Text = message
                };

                var context = new AMAOCContext();

                context.Notifications.Add(announcement);

                await context.SaveChangesAsync();

            }
            catch
            {
                return Request.CreateResponse(ret);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
