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
    public class EmployeeController : TableController<Employee>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AMAOCContext context = new AMAOCContext();
            DomainManager = new EntityDomainManager<Employee>(context, Request, true);
        }

        [EnableQuery(MaxTop = 500)]
        public IQueryable<Employee> GetAllEmployee()
        {
            return Query();
        }

        [QueryableExpand("EventAttendees")]
        public SingleResult<Employee> GetEmployee(string id)
        {
            return Lookup(id);
        }

        public Task<Employee> PatchEmployee(string id, Delta<Employee> patch)
        {
            return UpdateAsync(id, patch);
        }

        public async Task<IHttpActionResult> PostEmployee(Employee item)
        {
            Employee current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteEmployee(string id)
        {
            return DeleteAsync(id);
        }
    }
}