using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRMS.Models
{
    public class EmploymentType
    {
        [Key]
        public int EmpTypeId { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("Type")]
        public string EmpTypeName { get; set; }
        [Required]
        [MinLength(10)]
        [DisplayName("Description")]
        public string EmpTypeDescription { get; set; }

        public EmploymentType() { }

        public EmploymentType(int empTypeId, string empTypeName, string empTypeDescription)
        {
            EmpTypeId = empTypeId;
            EmpTypeName = empTypeName;
            EmpTypeDescription = empTypeDescription;
        }
    }
}
