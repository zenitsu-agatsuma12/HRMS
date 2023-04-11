using HRMS.Data;
using HRMS.Models;
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
    }
}
