using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface IPhilHealthRepository
    {
            // Add PhilHealth
            PhilHealth AddPhilHealth(PhilHealth newPhilHealth);
            // GetAllPhilHealth
            List<PhilHealth> ListOfPhilHealth();
            // GetId
            PhilHealth GetPhilHealthById(string Id);
            // Update
            PhilHealth UpdatePhilHealth(string PhilHealthId, PhilHealth newPhilHealth);
            // Delete
            PhilHealth DeletePhilHealth(string PhilHealthId);
         
            List<SelectListItem> GetEmployeeList();
            List<PhilHealth> ListOfPhilHealth(string searchValue);
            PhilHealth ConfirmID(int ID, string value);
            PhilHealth ValueSet(int ID, string value);
            PhilHealth GetID(int ID);

    }
}
