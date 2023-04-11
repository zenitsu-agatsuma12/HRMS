using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSAPI.Repository
{
    public interface IDepartmentRepository
    {
        Department AddDepartment(Department newDepartment);
        // GetAllDepartment
        List<Department> ListOfDepartment();
        // GetId
        Department GetDepartmentById(int Id);
        // Update
        Department UpdateDepartment(int DepartmentId, Department newDepartment);
        // Delete
        Department DeleteDepartment(int DepartmentId);
        List<Department> Filter(string a);
    }
}
