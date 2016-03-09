using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Person: BaseEntity
    {
        [Key]
        public virtual int PersonId { get; set; }

        [MaxLength(40)]
        public virtual string GivenName { get; set; }

        [MaxLength(40)]
        public virtual string MiddleName { get; set; }

        [Required]
        [MaxLength(40)]
        public virtual string SurName { get; set; }

        string DebuggerDisplay => $"{GivenName} {MiddleName} {SurName}";
    }
}
