using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRMS.Models
{
    public class AddressType
    {
        [Key]
        public int AddressTypeId { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("Street Name")]
        public string AddressTypeName { get; set; }

        public AddressType() { }

        public AddressType(int addressTypeId, string addressTypeName)
        {
            AddressTypeId = addressTypeId;
            AddressTypeName = addressTypeName;
        }
    }
}
