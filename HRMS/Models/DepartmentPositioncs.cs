using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class DepartmentPositioncs
    {
        [Key]
        public int No { get; set; }
        [DisplayName("Department")]
        public int DepartmentId { get; set; }
        
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        [DisplayName("Position")]
        public int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position? Position { get; set; }

        public DepartmentPositioncs() { }
        public DepartmentPositioncs(int no, int departmentId, int positionId)
        {
            No = no;
            DepartmentId = departmentId;
            PositionId = positionId;
        }
    }
}
