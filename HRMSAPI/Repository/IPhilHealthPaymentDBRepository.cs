using HRMSAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSAPI.Repository
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
        PhilHealthPayment UpdatePhilHealthPayment(PhilHealthPayment newPhilHealthPayment, int no);
        // Delete
        PhilHealthPayment DeletePhilHealthPayment(int No);
    }
}
