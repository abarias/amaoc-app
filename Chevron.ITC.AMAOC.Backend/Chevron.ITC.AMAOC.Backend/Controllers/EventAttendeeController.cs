using Chevron.ITC.AMAOC.Backend.Helpers;
using Chevron.ITC.AMAOC.Backend.Models;
using Chevron.ITC.AMAOC.DataObjects;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Owin.Security.Jwt;
using System.IdentityModel.Tokens;

namespace Chevron.ITC.AMAOC.Backend.Controllers
{
    [Authorize]
    public class EventAttendeeController : TableController<EventAttendee>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AMAOCContext context = new AMAOCContext();
            DomainManager = new EntityDomainManager<EventAttendee>(context, Request, true);
        }
        
        public IQueryable<EventAttendee> GetAllEventAttendee()
        {                        
            var items = Query();                        
            var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;                      
            var final = items.Where(e => e.EmployeeId == userId);

            return final;
        }
        
        public SingleResult<EventAttendee> GetEventAttendee(string id)
        {
            return Lookup(id);
        }

        public Task<EventAttendee> PatchEventAttendee(string id, Delta<EventAttendee> patch)
        {
            return UpdateAsync(id, patch);
        }

        public async Task<IHttpActionResult> PostEventAttendee(EventAttendee item)
        {
            EventAttendee current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteEventAttendee(string id)
        {
            return DeleteAsync(id);
        }
    }
}