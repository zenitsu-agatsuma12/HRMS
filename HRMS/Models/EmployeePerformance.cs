using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Models
{
    public class EmployeePerformance
    {
        [Key]
        public int No { get; set; }
        public string userID { get; set; }
        public string EmployeeName { get; set; }
        [Required]
        [MinLength(3)]
        [DisplayName("About")]
        public string About { get; set; }
        [Required]
        [MinLength(5)]
        [DisplayName("Performance Review")]
        public string PerformanceReview { get; set; }
        public string ReviewBy { get; set; }
        public bool Status { get; set; }
        public DateTime DateReview { get; set; }

        public EmployeePerformance() { }

    }
}
