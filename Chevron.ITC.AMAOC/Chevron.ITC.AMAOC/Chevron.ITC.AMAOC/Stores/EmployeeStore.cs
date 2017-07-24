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

        public async Task<IEnumerable<Employee>> GetEmployeesPrevAndNext(string userId)
        {
            var emps = await GetItemsAsync().ConfigureAwait(false);
            var currentEmp = emps.FirstOrDefault(e => e.UserId == userId);

            var empPrevNext = new List<Employee>();
            empPrevNext.Add(emps.OrderByDescending(e => e.TotalPointsEarned).LastOrDefault(e => e.TotalPointsEarned > currentEmp.TotalPointsEarned));
            empPrevNext.Add(currentEmp);
            empPrevNext.Add(emps.OrderByDescending(e => e.TotalPointsEarned).FirstOrDefault(e => e.TotalPointsEarned < currentEmp.TotalPointsEarned));

            return empPrevNext;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesTopTenByPoints(string userId)
        {
            await InitializeStore().ConfigureAwait(false);
            var emps = await GetItemsAsync().ConfigureAwait(false);

            var rankedEmps = from emp in emps
                             group emp by new { emp.TotalPointsEarned, emp.Id, emp.UserId, emp.FullName, emp.CAI, emp.Email }
                             into e
                             orderby e.Key.TotalPointsEarned descending
                             select new Employee
                             {
                                 UserId = e.Key.UserId,
                                 Id = e.Key.Id,
                                 CAI = e.Key.CAI,
                                 Email = e.Key.Email,
                                 FullName = e.Key.FullName,
                                 TotalPointsEarned = e.Key.TotalPointsEarned,
                                 Rank = (from emp in emps
                                         group emp by emp.TotalPointsEarned into ee
                                         select ee).Count(s => s.Key > e.Key.TotalPointsEarned) + 1,
                                 IsLoggedInUser = e.Key.Id == userId ? true : false
                             };

            //need to reproject for index / counter
            var rankedWithCounter = rankedEmps.Select((a, index) => new Employee
            {
                UserId = a.UserId,
                Id = a.Id,
                CAI = a.CAI,
                Email = a.Email,
                FullName = a.FullName,
                TotalPointsEarned = a.TotalPointsEarned,
                Rank = a.Rank,
                IsLoggedInUser = a.IsLoggedInUser,
                RankCounter = index + 1
            });

            return rankedWithCounter;
        }
    }
}
