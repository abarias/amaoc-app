using Chevron.ITC.AMAOC.Backend.Models;
using Chevron.ITC.AMAOC.DataObjects;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace Chevron.ITC.AMAOC.Backend.Controllers
{
    [Authorize]
    public class EventRatingCommentController : TableController<EventRatingComment>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AMAOCContext context = new AMAOCContext();
            DomainManager = new EntityDomainManager<EventRatingComment>(context, Request, true);
        }

        [EnableQuery(MaxTop = 500)]
        public IQueryable<EventRatingComment> GetAllEventRatingComment()
        {
            var items = Query();
            var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            var final = items.Where(e => e.EmployeeId == userId);

            return final;            
        }

        
        public SingleResult<EventRatingComment> GetEventRatingComment(string id)
        {
            return Lookup(id);
        }

        public Task<EventRatingComment> PatchEventRatingComment(string id, Delta<EventRatingComment> patch)
        {
            return UpdateAsync(id, patch);
        }

        public async Task<IHttpActionResult> PostEventRatingComment(EventRatingComment item)
        {
            EventRatingComment current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteEventRatingComment(string id)
        {
            return DeleteAsync(id);
        }
    }
}