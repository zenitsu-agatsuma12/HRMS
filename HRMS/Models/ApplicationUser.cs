﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class ApplicationUser:IdentityUser
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
        [DisplayName("Full Name")]
        public string? FullName { get; set; }
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
        //Account UserName
        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        // Foreign Key
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        [DisplayName("Position")]
        public int? PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position? Position { get; set; }

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


        public ApplicationUser()
        {
        }

        public ApplicationUser(string firstName, string middleName, string lastName, string? fullName, string gender, DateTime dateOfBirth, string phone, string email, int? departmentId, Department? department, int? positionId, Position? position, string? employeeType, string? sSSNumber, string? pagIbigId, string? philHealthId, string street, string barangay, string city, string state, int postalCode, DateTime dateHired, bool activeStatus)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            FullName = fullName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Phone = phone;
            Email = email;
            DepartmentId = departmentId;
            Department = department;
            PositionId = positionId;
            Position = position;
            EmployeeType = employeeType;
            SSSNumber = sSSNumber;
            PagIbigId = pagIbigId;
            PhilHealthId = philHealthId;
            Street = street;
            Barangay = barangay;
            City = city;
            State = state;
            PostalCode = postalCode;
            DateHired = dateHired;
            ActiveStatus = activeStatus;
        }
    }
}
