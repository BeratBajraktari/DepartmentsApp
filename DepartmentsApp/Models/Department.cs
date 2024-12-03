using System.ComponentModel.DataAnnotations;

namespace DepartmentsApp.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ParentId { get; set; }
    }
}
