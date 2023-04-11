using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class PhilHealthDBRepository: IPhilHealthRepository
    {
        HRMSDBContext _dbcontext;
        public PhilHealthDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public PhilHealth AddPhilHealth(PhilHealth newPhilHealth)
        {
            _dbcontext.Add(newPhilHealth);
            _dbcontext.SaveChanges();
            return newPhilHealth;
        }

        public PhilHealth DeletePhilHealth(string PhilHealthId)
        {
            var Pos = GetPhilHealthById(PhilHealthId);
            if (Pos != null)
            {
                _dbcontext.PhilHealths.Remove(Pos);
                _dbcontext.SaveChanges();
            }
            return Pos;
        }

        public PhilHealth GetPhilHealthById(string Id)
        {
            return _dbcontext.PhilHealths.Include(e => e.Employee).AsNoTracking().ToList().FirstOrDefault(x => x.PhilHealthId == Id);
        }

        public List<PhilHealth> ListOfPhilHealth()
        {
            return _dbcontext.PhilHealths.AsNoTracking().ToList();
        }

        public PhilHealth UpdatePhilHealth(string PhilHealthId, PhilHealth newPhilHealth)
        {
            _dbcontext.PhilHealths.Update(newPhilHealth);
            _dbcontext.SaveChanges();
            return newPhilHealth;
        }

        public List<SelectListItem> GetEmployeeList()
        {

            var listEmp = new List<SelectListItem>();
            List<Employee> employees = _dbcontext.Employees.ToList();
            listEmp = employees.Select(emp => new SelectListItem
            {
                Value = (emp.EmpId).ToString(),
                Text = emp.FirstName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "---Select Employee---"
            };
            listEmp.Insert(0, defItem);
            return listEmp;
        }

        public List<PhilHealth> ListOfPhilHealth(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                List<PhilHealth> philHealths = _dbcontext.PhilHealths
                   .Include(d => d.Employee)
                   .Where(e => e.PhilHealthId.Contains(searchValue))
                   .ToList();
                return philHealths;
            }

            List<PhilHealth> PhilHealths = _dbcontext.PhilHealths
                .Include(d => d.Employee)
                .ToList();
            return PhilHealths;
        }

        /*public PhilHealth CreateOrUpdate(int empId, string startdate)
        {
           
           
        }*/

        public PhilHealth ConfirmID(int empId, string value)
        {
            PhilHealth PhilHealth = _dbcontext.PhilHealths.FirstOrDefault(p => p.EmpId == empId);
            bool exists = _dbcontext.PhilHealths.Any(PhilHealth => PhilHealth.EmpId == empId);


            return PhilHealth;

        }

        public PhilHealth ValueSet(int empId, string startdate)
        {
            PhilHealth PhilHealth = new PhilHealth();
            PhilHealth.EmpId = empId;
            PhilHealth.StartDate = startdate;

            return PhilHealth;
        }

        public PhilHealth GetID(int empId)
        {
            var PhilHealth = _dbcontext.PhilHealths
           .Where(p => p.EmpId == empId)
           .Select(p => new PhilHealth { PhilHealthId = p.PhilHealthId })
           .FirstOrDefault();

            return PhilHealth;
        }
    }

}

