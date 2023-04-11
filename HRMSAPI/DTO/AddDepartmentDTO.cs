using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRMSAPI.DTO
{
    public class AddDepartmentDTO
    {

        [Required]
        [MinLength(2)]
        [DisplayName("Department Name")]
        public string DeptName { get; set; }

    }
}
