using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface ISSSPaymentRepository
    {
        // GetAllSSSPayment
        List<SSSPayment> ListOfSSSPayment();
        // AddSSSPayment
        SSSPayment AddSSSPayment(SSSPayment newSSSPayment);
        // GetId
        SSSPayment GetSSSPaymentById(int No);
        // Update
        SSSPayment UpdateSSSPayment( SSSPayment newSSSPayment);
        // Delete
        SSSPayment DeleteSSSPayment(int No);
    }
}
