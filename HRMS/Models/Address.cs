using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("Street Name")]
        public string Street { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("Barangay Name")]
        public string Barangay { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("City Name")]
        public string City { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("State Name")]
        public string State { get; set; }
        [RegularExpression("[0-9]{4}", ErrorMessage = "This is not a valid postal code")]
        [DisplayName("Postal Code")]
        public int PostalCode { get; set; }
        [DisplayName("Address Type")]
        public int AddressTypeId { get; set; }
        [ForeignKey("AddressTypeId")]
        public AddressType AddressType { get; set; }

        public Address() { }

        public Address(int addressId, string street, string barangay, string city, string state, int postal)
        { 
            AddressId = addressId;
            Street = street;
            Barangay = barangay;
            City = city;
            State = state;
            PostalCode = postal;
        }
    }
}
