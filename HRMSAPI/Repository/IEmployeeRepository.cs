using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSAPI.Repository
{
    public interface IEmployeeRepository
    {
        // Add Employee
        Employee AddEmployee(Employee newEmployee);
        // GetAllEmployee
        List<Employee> ListOfEmployee();
        // GetId
        Employee GetEmployeeById(int Id);
        // Update
        Employee UpdateEmployee(int EmployeeId, Employee newEmployee);
        // Delete
        Employee DeleteEmployee(int EmployeeId);
        List<SelectListItem> GetDepartmentList();
        List<SelectListItem> GetPositionList();
        List<Employee> GetFilter(string a, string b);
    }
}
