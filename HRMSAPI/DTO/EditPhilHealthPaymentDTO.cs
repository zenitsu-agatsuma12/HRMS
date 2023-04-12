using System.ComponentModel.DataAnnotations;

namespace HRMSAPI.DTO
{
    public class EditPhilHealthPaymentDTO
    {
        public int Payment { get; set; }
        public string Month { get; set; } = DateTime.Now.Month.ToString();
        public string Year { get; set; } = DateTime.Now.Year.ToString();
    }
}
