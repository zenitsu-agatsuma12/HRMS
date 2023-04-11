using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRMSAPI.DTO
{
    public class AddPositionDTO
    {
        [Required]
        [MinLength(2)]
        [DisplayName("Position Name")]
        public string Name { get; set; }
    }
}
