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

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(Employees);
            return Employees;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesTopTenByPoints(string userId)
        {
            var emps = await GetItemsAsync();

            int counter = 0;
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

            //(from emp in emps
            // where emp.TotalPointsEarned == e.Key.TotalPointsEarned
            // && emp.Id == e.Key.Id
            // select emp.FullName).FirstOrDefault(),                        

            var rankedWithCounter = rankedEmps.Select((a, index) => new Employee {
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
            //return (from emp in emps
            //        orderby emp.TotalPointsEarned descending, emp.FullName
            //        select emp).Take(10);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesPrevAndNext(string userId)
        {
            var emps = await GetItemsAsync();
            var currentEmp = await GetEmployeeByUserId(userId);

            var empPrevNext = new List<Employee>();            
            empPrevNext.Add(emps.OrderByDescending(e => e.TotalPointsEarned).LastOrDefault(e => e.TotalPointsEarned > currentEmp.TotalPointsEarned));
            empPrevNext.Add(currentEmp);
            empPrevNext.Add(emps.OrderByDescending(e => e.TotalPointsEarned).FirstOrDefault(e => e.TotalPointsEarned < currentEmp.TotalPointsEarned));

            return empPrevNext;
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
                Email = "abarias23@gmail.com",
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
                TotalPointsEarned = 30,
                Id = "003"
            });

            Employees.Add(new Employee
            {
                UserId = "004",
                CAI = "ABDC",
                Email = "John.Doe@chevron.com",
                FullName = "John Doe",
                TotalPointsEarned = 40,
                Id = "004"
            });

            Employees.Add(new Employee
            {
                UserId = "005",
                CAI = "DERG",
                Email = "Jane.Doe@chevron.com",
                FullName = "Jane Doe",
                TotalPointsEarned = 50,
                Id = "005"
            });

            Employees.Add(new Employee
            {
                UserId = "006",
                CAI = "OPER",
                Email = "Juan.DelaCruz@chevron.com",
                FullName = "Juan Dela Cruz",
                TotalPointsEarned = 60,
                Id = "006"
            });

            Employees.Add(new Employee
            {
                UserId = "007",
                CAI = "WWEF",
                Email = "John.Snow@chevron.com",
                FullName = "John Snow",
                TotalPointsEarned = 70,
                Id = "007"
            });

            Employees.Add(new Employee
            {
                UserId = "008",
                CAI = "KJAS",
                Email = "Arya.Stark@chevron.com",
                FullName = "Arya Stark",
                TotalPointsEarned = 80,
                Id = "008"
            });

            Employees.Add(new Employee
            {
                UserId = "009",
                CAI = "WLLA",
                Email = "Tyrion.Lannister@chevron.com",
                FullName = "Tyrion Lannister",
                TotalPointsEarned = 90,
                Id = "009"
            });

            Employees.Add(new Employee
            {
                UserId = "010",
                CAI = "LLAD",
                Email = "Dany.Targaryen@chevron.com",
                FullName = "Dany Targaryen",
                TotalPointsEarned = 100,
                Id = "010"
            });

            Employees.Add(new Employee
            {
                UserId = "011",
                CAI = "SADD",
                Email = "Samwell.Tarly@chevron.com",
                FullName = "Sam Tarly",
                TotalPointsEarned = 110,
                Id = "011"
            });

            Employees.Add(new Employee
            {
                UserId = "012",
                CAI = "LDDE",
                Email = "Sandor.Cleagane@chevron.com",
                FullName = "Sandor Cleagane",
                TotalPointsEarned = 120,
                Id = "012"
            });

            Employees.Add(new Employee
            {
                UserId = "013",
                CAI = "LLEA",
                Email = "Bran.Stark@chevron.com",
                FullName = "Bran Stark",
                TotalPointsEarned = 130,
                Id = "013"
            });

            Employees.Add(new Employee
            {
                UserId = "014",
                CAI = "DDAE",
                Email = "Rickon.Stark@chevron.com",
                FullName = "Rickon Stark",
                TotalPointsEarned = 140,
                Id = "014"
            });

            Employees.Add(new Employee
            {
                UserId = "015",
                CAI = "DLAE",
                Email = "Sansa.Stark@chevron.com",
                FullName = "Sansa Stark",
                TotalPointsEarned = 150,
                Id = "015"
            });
        }

        public Task<bool> UpdateEmployeeAsyncWithoutSync(Employee employee)
        {
            return Task.FromResult(true);
        }
    }
}
