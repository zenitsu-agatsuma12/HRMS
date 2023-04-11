using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface IPagIbigRepository
    {
        // Add Employee
        PagIbig AddPagIbig(PagIbig newPagIbig);
        // GetAllPagIbig
        List<PagIbig> ListOfPagIbig();
        // GetId
        PagIbig GetPagIbigById(string Id);
        // Update
        PagIbig UpdatePagIbig(string PagIbigId, PagIbig newPagIbig);
        // Delete
        PagIbig DeletePagIbig(string PagIbigId);
        List<SelectListItem> GetEmployeeList();
        List<PagIbig> ListOfPagIbig(string searchValue);
        PagIbig ConfirmID(int ID, string value);
        PagIbig ValueSet(int ID, string value);
        PagIbig GetID(int ID);
    }
}
