using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using HRMSAPI.Models;

namespace HRMSAPI.DTO
{
    public class AddPhilHealthDTO
    {
        [Key]
        [RegularExpression("[0-9]{12}", ErrorMessage = "This is not a valid PhilHealth Number")]
        [Display(Name = "Philhealth Number")]
        public string PhilHealthId { get; set; }
        public int EmpId { get; set; }
        public string StartDate { get; set; }
        public bool Status { get; set; }

        public AddPhilHealthDTO() { }

        public AddPhilHealthDTO(string philHealthId, int empId, string startDate, bool status)
        {
            PhilHealthId = philHealthId;
            EmpId = empId;
            StartDate = startDate;
            Status = status;
        }
    }
}
