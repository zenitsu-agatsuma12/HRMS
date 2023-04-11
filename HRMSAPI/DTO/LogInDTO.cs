using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HRMSAPI.DTO
{
    public class LogInDTO
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
