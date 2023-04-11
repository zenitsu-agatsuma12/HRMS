using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMSAPI.Models
{
    public class SSSPayment
    {
        [Key]
        public int No { get; set; }
        public string FullName { get; set; }
        public string? SSSNumber { get; set; }

        public int Payment { get; set; }

        public string? Month { get; set; } = DateTime.Now.Month.ToString();
        public string? Year { get; set; } = DateTime.Now.Year.ToString();

        public SSSPayment() { }

        public SSSPayment(int no, string? sSSNumber, int payment, string? month, string? year)
        {
            No = no;
            SSSNumber = sSSNumber;
            Payment = payment;
            Month = month;
            Year = year;
        }
    }
}
