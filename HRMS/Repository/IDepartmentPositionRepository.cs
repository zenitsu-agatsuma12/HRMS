using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface IDepartmentPositionRepository
    {
        DepartmentPositioncs AddDepartmentPositioncs(DepartmentPositioncs newDepartmentPosition);
        // GetAllDepartmentPosition
        List<DepartmentPositioncs> ListOfDepartmentPosition();
        // GetId
        DepartmentPositioncs GetDepartmentPositionById(int Id);
        // Update
        DepartmentPositioncs UpdateDepartmentPosition(int DepartmentPositionId, DepartmentPositioncs newDepartmentPosition);
        // Delete
        DepartmentPositioncs DeleteDepartmentPosition(int DepartmentPositionId);
        List<SelectListItem> GetDepartmentList();
        List<SelectListItem> GetPosition();
        List<DepartmentPositioncs> ListDepartmentPositioncs();
        List<DepartmentPositioncs> GetFilter(string a, string b);
    }
}
