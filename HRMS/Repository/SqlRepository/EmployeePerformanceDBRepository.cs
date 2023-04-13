using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class EmployeePerformanceDBRepository: IEmployeePerformanceDBRepository
    {
        HRMSDBContext _dbcontext;
        private UserManager<ApplicationUser> _userManager { get; }
        public EmployeePerformanceDBRepository(HRMSDBContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
        }

        public EmployeePerformance AddEmployeePerformance(EmployeePerformance newEmployeePerformance)
        {
            _dbcontext.Add(newEmployeePerformance);
            _dbcontext.SaveChanges();
            return newEmployeePerformance;
        }

        public EmployeePerformance DeleteEmployeePerformance(int EmployeePerformanceId)
        {
            var performance = GetEmployeePerformanceById(EmployeePerformanceId);
            if (performance != null)
            {
                _dbcontext.EmployeePerformances.Remove(performance);
                _dbcontext.SaveChanges();
            }
            return performance;
        }

        public EmployeePerformance GetEmployeePerformanceById(int Id)
        {
            return _dbcontext.EmployeePerformances.AsNoTracking().ToList().FirstOrDefault(x => x.No == Id);
        }
        public List<EmployeePerformance> ListOfEmployeePerformance(string employeeID)
        {
            if (employeeID == null)
            {
                return _dbcontext.EmployeePerformances.AsNoTracking().ToList();
            }
            int count = _dbcontext.EmployeePerformances.Count();
            return _dbcontext.EmployeePerformances.Where(e => e.userID == employeeID).AsNoTracking().ToList();
        }

        public EmployeePerformance UpdateEmployeePerformance(int EmployeePerformanceId, EmployeePerformance newEmployeePerformance)
        {
            _dbcontext.EmployeePerformances.Update(newEmployeePerformance);
            _dbcontext.SaveChanges();
            return newEmployeePerformance;
        }

        public List<EmployeePerformance> ListOfEmployeePerformanceReviewBy(string employeeID)
        {
            return _dbcontext.EmployeePerformances.Where(e => e.ReviewBy == employeeID).AsNoTracking().ToList();
        }
    }
}
