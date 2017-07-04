using Chevron.ITC.AMAOC.Backend.Helpers;
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
    public class EventController : TableController<Event>
   {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AMAOCContext context = new AMAOCContext();
            DomainManager = new EntityDomainManager<Event>(context, Request, true);
        }
                
        [EnableQuery(MaxTop = 500)]
        public IQueryable<Event> GetAllEvent()
        {
            return Query();
        }

        [QueryableExpand("FeedbackQuestions")]
        public SingleResult<Event> GetEvent(string id)
        {
            return Lookup(id);
        }
        
        public Task<Event> PatchEvent(string id, Delta<Event> patch)
        {
            return UpdateAsync(id, patch);
        }
        
        public async Task<IHttpActionResult> PostEvent(Event item)
        {
            Event current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        
        public Task DeleteEvent(string id)
        {
            return DeleteAsync(id);
        }
    }
}