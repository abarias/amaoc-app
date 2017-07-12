using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class EmployeeStore : BaseStore<Employee>, IEmployeeStore
    {
        List<Employee> Employees { get; }

        public EmployeeStore()
        {
            Employees = new List<Employee>();
        }

        public override async Task<IEnumerable<Employee>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Employees);
            return Employees;
        }

        public async Task<Employee> GetEmployeeByUserId(string userId)
        {            
            var emps = await GetItemsAsync();
            return emps.FirstOrDefault(e => e.UserId == userId);
        }

        public override Task<bool> InsertAsync(Employee item)
        {
            return Task.FromResult(true);
        }

        public override async Task InitializeStore()
        {
            if (Employees.Count != 0)
                return;

            Employees.Add(new Employee
            {
                UserId = "001",
                CAI = "BOPD",
                Email = "ABArias@chevron.com",
                FullName = "Anthony Bernard C. Arias",
                TotalPointsEarned = 10,
                Id = "001"
            });

            Employees.Add(new Employee
            {
                UserId = "002",
                CAI = "BLYO",
                Email = "Benjamin.Carpena@chevron.com",
                FullName = "Benjamin Carpena",
                TotalPointsEarned = 20,
                Id = "002"
            });

            Employees.Add(new Employee
            {
                UserId = "003",
                CAI = "RZSO",
                Email = "Ryan.Dalawis@chevron.com",
                FullName = "Ryan Dalawis",
                TotalPointsEarned = 20,
                Id = "003"
            });
        }
    }
}
