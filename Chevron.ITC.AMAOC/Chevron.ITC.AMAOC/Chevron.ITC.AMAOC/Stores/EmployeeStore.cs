using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using Chevron.ITC.AMAOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Stores
{
    public class EmployeeStore : BaseStore<Employee>, IEmployeeStore
    {
        public override string Identifier => "Employee";
        public EmployeeStore()
        {

        }

        public async Task<Employee> GetEmployeeByUserId(string userId)
        {
            await InitializeStore().ConfigureAwait(false);
            var emps = await GetItemsAsync().ConfigureAwait(false);
            return emps.FirstOrDefault(e => e.UserId == userId);
        }

        public Task<IEnumerable<Employee>> GetEmployeesPrevAndNext(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetEmployeesTopTenByPoints(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
