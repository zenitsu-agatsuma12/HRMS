using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        [PasswordPropertyText]
        [MinLength(8)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        [PasswordPropertyText]
        [MinLength(8)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password and confirm password doesnt match")]
        public string ConfirmPassword { get; set; }
    }
}
