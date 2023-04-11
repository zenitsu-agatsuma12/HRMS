using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSAPI.Repository
{
    public interface IEmployeePerformanceDBRepository
    {
        // Add Employee
        EmployeePerformance AddEmployeePerformance(EmployeePerformance newEmployeePerformance);
        // GetAllEmployeePerformance
        List<EmployeePerformance> ListOfEmployeePerformance(string employeeID);
        // GetId
        EmployeePerformance GetEmployeePerformanceById(int Id);
        // Update
        EmployeePerformance UpdateEmployeePerformance(int EmployeePerformanceId, EmployeePerformance newEmployeePerformance);
        // Delete
        EmployeePerformance DeleteEmployeePerformance(int EmployeePerformanceId);

    }
}
