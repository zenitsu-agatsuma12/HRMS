using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class EmployeeDBRepository : IEmployeeRepository
    {
        HRMSDBContext _dbcontext;

        public EmployeeDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Employee AddEmployee(Employee newEmployee)
        {
            _dbcontext.Add(newEmployee);
            _dbcontext.SaveChanges();
            return newEmployee;
        }

        public Employee DeleteEmployee(int EmployeeId)
        {
            Employee Emp = GetEmployeeById(EmployeeId);
            if (Emp != null)
            {
                Emp.ActiveStatus = false;
                _dbcontext.Employees.Update(Emp);
                _dbcontext.SaveChanges();
            }
            return Emp;
        }

        public Employee GetEmployeeById(int Id)
        {
            return _dbcontext.Employees.Include(d => d.Department).Include(p => p.Position).AsNoTracking().ToList().FirstOrDefault(x => x.EmpId == Id);
        }

        public List<Employee> ListOfEmployee()
        {
            return _dbcontext.Employees.AsNoTracking().ToList();
        }

        public Employee UpdateEmployee(int EmployeeId, Employee newEmployee)
        {
            _dbcontext.Employees.Update(newEmployee);
            _dbcontext.SaveChanges();
            return newEmployee;
        }
        public List<SelectListItem> GetDepartmentList()
        {
            var listDept = new List<SelectListItem>();
            List<Department> departments = _dbcontext.Departments.ToList();
            listDept = departments.Select(dept => new SelectListItem
            {
                Value = (dept.DeptId).ToString(),
                Text = dept.DeptName
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Department"
            };
            listDept.Insert(0, defItem);
            return listDept;
        }

        public List<SelectListItem> GetPositionList()
        {
            var listDept = new List<SelectListItem>();
            List<Position> Positions = _dbcontext.Positions.ToList();
            listDept = Positions.Select(pos => new SelectListItem
            {
                Value = (pos.PosId).ToString(),
                Text = pos.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Position"
            };
            listDept.Insert(0, defItem);
            return listDept;
        }

        public List<Employee> GetFilter(string searchOption, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchOption) && !string.IsNullOrEmpty(searchValue))
            {
                List<Employee> employees = _dbcontext.Employees
                    .Include(d => d.Department)
                    .Where(e => (e.FirstName.Contains(searchValue)
                             || e.MiddleName.Contains(searchValue)
                             || e.LastName.Contains(searchValue))
                             && e.DepartmentId.ToString().Contains(searchOption))
                                 .ToList();
                return employees;
            }
            else if (!string.IsNullOrEmpty(searchOption) && string.IsNullOrEmpty(searchValue))
            {
                //Return Department
                List<Employee> employees = _dbcontext.Employees
                    .Include(d => d.Department)
                    .Where(e => e.DepartmentId.ToString().Contains(searchOption))
                    .ToList();
                return employees;


            }
            else if (string.IsNullOrEmpty(searchOption) && !string.IsNullOrEmpty(searchValue))
            {
                List<Employee> employees = _dbcontext.Employees
                    .Include(d => d.Department)
                    .Where(e => e.FirstName.Contains(searchValue)
                             || e.MiddleName.Contains(searchValue)
                             || e.LastName.Contains(searchValue))
                                 .ToList();
                return employees;
            }
            else
            {
                //Return All
                List<Employee> employees = _dbcontext.Employees
                    .Include(d => d.Department)
                    //.Where(e => e.ActiveStatus==false)
                    .ToList();
                return employees;

            }
        }
    }
}
