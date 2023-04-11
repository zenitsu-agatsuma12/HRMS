using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRMSAPI.DTO
{
    public class EditEmployeePerformanceDTO
    {
        [Required]
        [MinLength(3)]
        [DisplayName("About")]
        public string About { get; set; }
        [Required]
        [MinLength(5)]
        [DisplayName("Performance Review")]
        public string PerformanceReview { get; set; }
        public DateTime DateReview { get; set; }
    }
}
