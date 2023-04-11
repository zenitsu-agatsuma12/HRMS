using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMSAPI.Models
{
    public class PagIbig
    {
        [Key]
        [RegularExpression("[0-9]{12}", ErrorMessage = "This is not a valid PagIbig Number")]
        [Display(Name = "PagIbig Number")]
        public string PagIbigId { get; set; }
        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Employee? Employee { get; set; }
        public string StartDate { get; set; }
        public bool Status { get; set; }

        public PagIbig() { }

        public PagIbig(string pagibigid,  int? empid, string startdate, bool status)
        {
            EmpId = empid;
            StartDate = startdate;
            Status = status;
            EmpId = empid;
        }
    }
}
