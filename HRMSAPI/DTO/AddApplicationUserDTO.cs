﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace HRMSAPI.DTO
{
    public class AddApplicationUserDTO
    {
 
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
        public string PostalCode { get; set; }

        //Account Status
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        //Account UserName
        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirm password doesnt match")]
        public string ConfirmPassword { get; set; } 

    }
}
