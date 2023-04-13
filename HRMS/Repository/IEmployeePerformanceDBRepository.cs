using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface IEmployeePerformanceDBRepository
    {
        // Add Employee
        EmployeePerformance AddEmployeePerformance(EmployeePerformance newEmployeePerformance);
        // GetAllEmployeePerformance
        List<EmployeePerformance> ListOfEmployeePerformance(string employeeID);
        List<EmployeePerformance> ListOfEmployeePerformanceReviewBy(string employeeID);
        // GetId
        EmployeePerformance GetEmployeePerformanceById(int Id);
        // Update
        EmployeePerformance UpdateEmployeePerformance(int EmployeePerformanceId, EmployeePerformance newEmployeePerformance);
        // Delete
        EmployeePerformance DeleteEmployeePerformance(int EmployeePerformanceId);

    }
}
