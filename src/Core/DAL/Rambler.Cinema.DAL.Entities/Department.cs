using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Department: BaseEntity
    {
        [Key]
        public virtual int DepartmentId { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("UX_Department_Name", IsClustered = false, IsUnique = true)]
        public virtual string Name { get; set; }

        string DebuggerDisplay => $"{Name}";
    }
}
