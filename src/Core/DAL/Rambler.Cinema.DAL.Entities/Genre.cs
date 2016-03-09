using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Genre: BaseEntity
    {
        [Key]
        public virtual int GenreId { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("UX_Genre_Name", IsClustered = false, IsUnique = true)]
        public virtual string Name { get; set; }

        string DebuggerDisplay => $"{Name}";
    }
}
