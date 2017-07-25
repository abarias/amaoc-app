using Chevron.ITC.AMAOC.Backend.Models;
using Chevron.ITC.AMAOC.DataObjects;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace Chevron.ITC.AMAOC.Backend.Controllers
{
    [Authorize]
    public class NotificationController : TableController<Notification>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AMAOCContext context = new AMAOCContext();
            DomainManager = new EntityDomainManager<Notification>(context, Request, true);
        }

        public IQueryable<Notification> GetAllNotification()
        {
            return Query();
        }

        public SingleResult<Notification> GetNotification(string id)
        {
            return Lookup(id);
        }
        
        public Task<Notification> PatchNotification(string id, Delta<Notification> patch)
        {
            return UpdateAsync(id, patch);
        }
        
        public async Task<IHttpActionResult> PostNotification(Notification item)
        {
            Notification current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        
        public Task DeleteNotification(string id)
        {
            return DeleteAsync(id);
        }
    }
}