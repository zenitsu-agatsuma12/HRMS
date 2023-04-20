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

        //Add Employee Performance
        public EmployeePerformance AddEmployeePerformance(EmployeePerformance newEmployeePerformance)
        {
            _dbcontext.Add(newEmployeePerformance);
            _dbcontext.SaveChanges();
            return newEmployeePerformance;
        }

        //Delete Employee Performance
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

        //Get the Employee Performance by ID
        public EmployeePerformance GetEmployeePerformanceById(int Id)
        {
            return _dbcontext.EmployeePerformances.AsNoTracking().ToList().FirstOrDefault(x => x.No == Id);
        }

        //List Of Employee Performance 
        public List<EmployeePerformance> ListOfEmployeePerformance(string employeeID)
        {
            if (employeeID == null)
            {
                return _dbcontext.EmployeePerformances.Where(d=>d.DeleteStatus==false).AsNoTracking().ToList();
            }
            int count = _dbcontext.EmployeePerformances.Count();
            return _dbcontext.EmployeePerformances.Where(e => e.userID == employeeID).Where(d => d.DeleteStatus == false).AsNoTracking().ToList();
        }

        //List Of Employe Performance and get By Reviewer name or Manager Name
        public List<EmployeePerformance> ListOfEmployeePerformanceReviewBy(string employeeID)
        {
            return _dbcontext.EmployeePerformances.Where(e => e.ReviewBy == employeeID).Where(d => d.DeleteStatus == false).AsNoTracking().ToList();
        }

        // Search function for Employee Performance
        public List<EmployeePerformance> SearchListOfEmployeePerformance(string employeeID, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                if (employeeID == null)
                {
                    return _dbcontext.EmployeePerformances.Where(d => d.DeleteStatus == false).AsNoTracking().ToList();
                }
                int count = _dbcontext.EmployeePerformances.Count();
                return _dbcontext.EmployeePerformances.Where(e => e.userID == employeeID).Where(d => d.DeleteStatus == false).AsNoTracking().ToList();
            }
            else
            {
                return _dbcontext.EmployeePerformances.Where(d => d.DeleteStatus == false)
                                                      .Where(e => e.EmployeeName.Contains(searchValue)||
                                                                  e.ReviewBy.Contains(searchValue))
                                                      .AsNoTracking()
                                                      .ToList();
            }
        }

        // Update the Employee Performance
        public EmployeePerformance UpdateEmployeePerformance(int EmployeePerformanceId, EmployeePerformance newEmployeePerformance)
        {
            _dbcontext.EmployeePerformances.Update(newEmployeePerformance);
            _dbcontext.SaveChanges();
            return newEmployeePerformance;
        }
    }
}
