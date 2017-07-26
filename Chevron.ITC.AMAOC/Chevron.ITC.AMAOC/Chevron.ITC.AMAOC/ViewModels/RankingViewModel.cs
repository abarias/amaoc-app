using Chevron.ITC.AMAOC.DataObjects;
using FormsToolkit;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.ViewModels
{
    public class RankingViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Employee> Employees { get; } = new ObservableRangeCollection<Employee>();        
        public DateTime NextForceRefresh { get; set; }

        bool noEmployeesFound;
        public bool NoEmployeesFound
        {
            get { return noEmployeesFound; }
            set { SetProperty(ref noEmployeesFound, value); }
        }

        bool noPrevNextEmployeesFound;
        public bool NoPrevNextEmployeesFound
        {
            get { return noPrevNextEmployeesFound; }
            set { SetProperty(ref noPrevNextEmployeesFound, value); }
        }

        string noEmployeesFoundMessage;
        public string NoEmployeesFoundMessage
        {
            get { return noEmployeesFoundMessage; }
            set { SetProperty(ref noEmployeesFoundMessage, value); }
        }

        string totalEmployees;
        public string TotalEmployees
        {
            get { return totalEmployees; }
            set { SetProperty(ref totalEmployees, value); }
        }


        public RankingViewModel(INavigation navigation) : base(navigation)
        {
            NextForceRefresh = DateTime.UtcNow.AddMinutes(45);            
        }


        ICommand forceRefreshCommand;
        public ICommand ForceRefreshCommand =>
        forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

        async Task ExecuteForceRefreshCommandAsync()
        {
            await ExecuteLoadEmployeesAsync(true);
        }


        ICommand loadEmployeesCommand;
        public ICommand LoadEmployeesCommand =>
            loadEmployeesCommand ?? (loadEmployeesCommand = new Command<bool>(async (f) => await ExecuteLoadEmployeesAsync()));

        async Task<bool> ExecuteLoadEmployeesAsync(bool force = false)
        {
            if (IsBusy)
                return false;

            bool isCurrentUserInTopTen = false;
            string currentUserId = Chevron.ITC.AMAOC.Helpers.Settings.Current.UserId;
            try
            {
                NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                IsBusy = true;
                NoEmployeesFound = false;
                NoPrevNextEmployeesFound = false;
                TotalEmployees = string.Empty;
                


#if DEBUG
                await Task.Delay(1000);
#endif

                var allEmps = await StoreManager.EmployeeStore.GetEmployeesTopTenByPoints(currentUserId);

                var empRanked = allEmps.Take(10);

                isCurrentUserInTopTen = empRanked.Any(e => e.UserId == currentUserId);

                if (!isCurrentUserInTopTen)                                    
                {                    
                    int currentEmpRank = allEmps.Where(e => e.UserId == currentUserId).SingleOrDefault().RankCounter;
                    if (currentEmpRank <= 12)
                    { 
                        empRanked = allEmps.Take(12);
                    }
                    else
                    {
                        var currentEmp = allEmps.Where(e => e.UserId == currentUserId).SingleOrDefault();
                        var empPrevNext = new List<Employee>();
                        empPrevNext.Add(allEmps.OrderByDescending(e => e.TotalPointsEarned).LastOrDefault(e => e.TotalPointsEarned > currentEmp.TotalPointsEarned));
                        empPrevNext.Add(currentEmp);

                        var lastEmp = allEmps.OrderByDescending(e => e.TotalPointsEarned).FirstOrDefault(e => e.TotalPointsEarned < currentEmp.TotalPointsEarned);
                        if (lastEmp != null)
                            empPrevNext.Add(allEmps.OrderByDescending(e => e.TotalPointsEarned).FirstOrDefault(e => e.TotalPointsEarned < currentEmp.TotalPointsEarned));
                        
                        empRanked = allEmps.Take(11).Concat(empPrevNext);
                    }
                }

                TotalEmployees = $"{allEmps.Count()} Total";
                Employees.ReplaceRange(empRanked);                

                if (Employees.Count == 0)
                {
                    NoEmployeesFoundMessage = "No Employees Found";

                    NoEmployeesFound = true;
                }
                else
                {
                    NoEmployeesFound = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "ExecuteLoadEmployeesAsync");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }
    }
}
