using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class PhilHealth
    {
        [Key]
        [RegularExpression("[0-9]{12}", ErrorMessage = "This is not a valid PhilHealth Number")]
        [Display(Name = "Philhealth Number")]
        public string PhilHealthId { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Employee? Employee { get; set; }
        public string StartDate { get; set; }
        public bool Status { get; set; }

        public PhilHealth() { }

        public PhilHealth(string philHealthId, int? empId, string startdate, bool status)
        {
            PhilHealthId = philHealthId;
            EmpId = empId;
            StartDate = startdate;
            Status = status;
        }

    }
}
