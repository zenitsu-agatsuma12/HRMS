using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        // public string FullName { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        [MinLength(2)]
        [Required]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        [MinLength(2)]
        [Required]
        public string LastName { get; set; }
        [DisplayName("Full Name")]
        public string FullName => string.Join(" ", FirstName, MiddleName, LastName);
        [Required]
        public string Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(09)[0-9]{9}", ErrorMessage = "This is not a valid phone number")]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Username")]
        [MinLength(2)]
        [Required]
        public string UserName { get; set; }
        [DisplayName("Password")]
        [PasswordPropertyText]
        [Required]
        public string Password { get; set; }
        // Foreign Key
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        [DisplayName("Position")]
        public int? PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position? Position { get; set; }

        public string? EmployeeType {get;set;}
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
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

        public bool ActiveStatus { get; set; }


        public Employee()
        { }

       public Employee(int empId, string firstName, string middleName, string lastName, string fullName, string gender, DateTime dateOfBirth, string email, string phone, string userName, string password, int? departmentId, int? positionId, string street, string barangay, string city, string state, int postalCode, bool activeStatus, string employeeType, DateTime dateHired)
        {
            EmpId = empId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Email = email;
            Phone = phone;
            UserName = userName;
            Password = password;
            DepartmentId = departmentId;
            PositionId = positionId;
            Street = street;
            Barangay = barangay;
            City = city;
            State = state;
            PostalCode = postalCode;
            ActiveStatus = activeStatus;
            EmployeeType = employeeType;
            DateHired = dateHired;
        }
    }
}
