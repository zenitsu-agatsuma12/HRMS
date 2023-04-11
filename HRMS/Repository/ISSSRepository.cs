using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface ISSSRepository
    {
        // Add SSS
        SSS AddSSS(SSS newSSS);
        // GetAllSSS
        List<SSS> ListOfSSS();
        // GetId
        SSS GetSSSById(string Id);
        // Update
        SSS UpdateSSS(string SSSId, SSS newSSS);
        // Delete
        SSS DeleteSSS(string SSSId);
        List<SelectListItem> GetEmployeeList();
        List<SSS> ListOfSSS(string searchValue);
        SSS ConfirmID(int ID, string value);
        SSS ValueSet(int ID, string value);
        SSS GetID(int ID);
    }
}
