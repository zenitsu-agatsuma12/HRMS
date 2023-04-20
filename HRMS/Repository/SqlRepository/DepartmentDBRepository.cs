using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class DepartmentDBRepository : IDepartmentRepository
    {
        HRMSDBContext _dbcontext;

        public DepartmentDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Department AddDepartment(Department newDepartment)
        {
            _dbcontext.Add(newDepartment);
            _dbcontext.SaveChanges();
            return newDepartment;
        }

        public Department DeleteDepartment(int DepartmentId)
        {
            var Dept = GetDepartmentById(DepartmentId);
            if (Dept != null)
            {
                _dbcontext.Departments.Remove(Dept);
                _dbcontext.SaveChanges();
            }
            return Dept;
        }

        public List<Department> Filter(string searchValue)
        {
         if (!string.IsNullOrEmpty(searchValue))
            {
             List<Department> departments = _dbcontext.Departments
                       .Where(e => e.DeptName.Contains(searchValue))
                       .ToList();

                return departments;
            }
              List<Department> Departments = _dbcontext.Departments.ToList();
                return Departments;
        }

        public Department GetDepartmentById(int Id)
        {
            return _dbcontext.Departments.AsNoTracking().ToList().FirstOrDefault(x => x.DeptId == Id);
        }

        public List<Department> ListOfDepartment()
        {
            return _dbcontext.Departments.AsNoTracking().ToList();
        }

        public Department UpdateDepartment(int DepartmentId, Department newDepartment)
        {
            _dbcontext.Departments.Update(newDepartment);
            _dbcontext.SaveChanges();
            return newDepartment;
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
    }
}
