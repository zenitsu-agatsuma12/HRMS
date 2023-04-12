using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSAPI.Repository
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
        SSSPayment UpdateSSSPayment( SSSPayment newSSSPayment, int no);
        // Delete
        SSSPayment DeleteSSSPayment(int No);
    }
}
