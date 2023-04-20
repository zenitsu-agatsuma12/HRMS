using HRMS.Models;

namespace HRMS.Repository
{
    public interface IPagIbigPaymentRepository
    {
        // Add PagIbigPayment
        PagIbigPayment AddPagIbigPayment(PagIbigPayment newPagIbigPayment);
        // GetAllPagIbigPayment
        List<PagIbigPayment> ListOfPagIbigPayment(string searchValue);
        // GetId
        PagIbigPayment GetPagIbigPaymentById(int No);
        // Update
        PagIbigPayment UpdatePagIbigPayment(PagIbigPayment newPagIbigPayment);
        // Delete
        PagIbigPayment DeletePagIbigPayment(int No);
    }
}
