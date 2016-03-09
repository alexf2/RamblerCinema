using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    public enum PhoneType
    {
        Work = 0,
        Home = 1,
        Cell = 2,
        Main = 3,
        Additional = 4
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Phone: BaseEntity
    {
        [Key]
        public virtual int PhoneId { get; set; }

        [Required, Phone]
        [MaxLength(32)]
        [Index("UX_Phone_Number", IsClustered = false, IsUnique = false)]
        public virtual string Number { get; set; }

        public virtual PhoneType? PhoneType { get; set; }

        string DebuggerDisplay => $"{Number} is {PhoneType}";


        public virtual int? PersonId { get; set; }
        [ForeignKey("PersonId")]        
        public virtual ContactPerson Person { get; set; }

        public virtual int? CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public virtual Cinema Cinema { get; set; }
    }
}
