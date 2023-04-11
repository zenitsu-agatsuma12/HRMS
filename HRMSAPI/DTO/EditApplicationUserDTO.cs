using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace HRMSAPI.DTO
{
    public class EditApplicationUserDTO
    {
        public string Guid { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        [MinLength(1)]
        [Required]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [MinLength(2)]
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("(09)[0-9]{9}", ErrorMessage = "This is not a valid phone number")]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        // Foreign Key

        public string? EmployeeType { get; set; }

        //Benefits
        [RegularExpression("[0-9]{13}", ErrorMessage = "This is not a valid SSS Number")]
        [Display(Name = "SSS Number")]
        public string? SSSNumber { get; set; }

        [RegularExpression("[0-9]{12}", ErrorMessage = "This is not a valid PagIbig Number")]
        [Display(Name = "PagIbig Number")]
        public string? PagIbigId { get; set; }
        [RegularExpression("[0-9]{12}", ErrorMessage = "This is not a valid PhilHealth Number")]
        [Display(Name = "Philhealth Number")]
        public string? PhilHealthId { get; set; }

        //Addrress
        [Required]
        public string Street { get; set; }
        [Required]
        public string Barangay { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int PostalCode { get; set; }

        //Account Status
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        public bool ActiveStatus { get; set; }

    }
}
