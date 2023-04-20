using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class PhilHealthPayment
    {
        [Key]
        public int No { get; set; }
        public string FullName { get; set; }
        public string? PhilHealthNumber { get; set; }
        [Range(100, int.MaxValue, ErrorMessage = "Payment must be minimum of 100")]
        public int Payment { get; set; }
        public string Month { get; set; } = DateTime.Now.Month.ToString();
        public string Year { get; set; } = DateTime.Now.Year.ToString();
        public bool status { get; set; }

        public PhilHealthPayment() { }

        public PhilHealthPayment(int no, string? philHealthNumber, int payment, string month, string year)
        {
            No = no;
            PhilHealthNumber = philHealthNumber;
            Payment = payment;
            Month = month;
            Year = year;
        }
    }
}
