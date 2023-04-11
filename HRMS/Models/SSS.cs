using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class SSS
    {
        [Key]
        [RegularExpression("[0-9]{13}", ErrorMessage = "This is not a valid SSS Number")]
        [Display(Name = "SSS Number")]
        public string SSSNumber { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Employee? Employee { get; set; }
        public string StartDate { get; set; }

        
        public bool Status { get; set; }

        public SSS() { }

        public SSS(string sSSNumber, int? empId, string startDate, bool status)
        {
            SSSNumber = sSSNumber;
            EmpId = empId;
            StartDate = startDate;
            Status = status;
        }
    }
}
