using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HRMS.Models
{
    public class Position
    {
        [Key]
        public int PosId { get; set; }
        [Required]
        [MinLength(2)]
        [DisplayName("Position Name")]
        public string Name { get; set; }

        public Position() { }

        public Position(int id, string name)
        {
            PosId = id;
            Name = name;
        }
    }
}
