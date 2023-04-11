using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface IPhilHealthPaymentDBRepository
    {
        // Add PhilHealthPayment
        PhilHealthPayment AddPhilHealthPayment(PhilHealthPayment newPhilHealthPayment);
        // GetAllPhilHealthPayment
        List <PhilHealthPayment> ListOfPhilHealthPayment();
        // GetId
        PhilHealthPayment GetPhilHealthPaymentById(int No);
        // Update
        PhilHealthPayment UpdatePhilHealthPayment(PhilHealthPayment newPhilHealthPayment);
        // Delete
        PhilHealthPayment DeletePhilHealthPayment(int No);
    }
}
