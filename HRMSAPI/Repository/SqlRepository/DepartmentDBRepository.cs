using HRMSAPI.Data;
using HRMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Repository.SqlRepository
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
