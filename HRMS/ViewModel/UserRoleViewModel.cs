using HRMS.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.ViewModel
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Department { get; set; }
        public int deptId { get; set; }
    }
}
