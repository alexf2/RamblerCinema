using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Cinema: BaseEntity
    {        
        [Key]
        public virtual int CinemaId { get; set; }

        [Required]
        [MaxLength(64)]
        [Index("UX_Cinema_Name", IsClustered = false, IsUnique = true)]
        public virtual string Name { get; set; }

        [Required]
        [DefaultValue(1)]
        [Range(typeof(int), "1", "10")]
        public virtual int HallsNumber { get; set; }

        public virtual int AddressId { get; set; }
        [Required]
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

        ICollection<Phone> _phones;
        public virtual ICollection<Phone> Phones {
            get { return _phones ?? (_phones = new List<Phone>()); }
            set { _phones = value; }
        }

        public virtual int SupervisorId { get; set; }
        [Required]
        [ForeignKey("SupervisorId")]
        public virtual Person Supervisor { get; set; }
        
        
        ICollection<ContactPerson> _contacts;
        public virtual ICollection<ContactPerson> Contacts
        {
            get { return _contacts ?? (_contacts = new List<ContactPerson>()); }
            set { _contacts = value; }
        }

        ICollection<Session> _filmSessions;
        public virtual ICollection<Session> FilmSessions
        {
            get { return _filmSessions ?? (_filmSessions = new List<Session>()); }
            set { _filmSessions = value; }
        }

        string DebuggerDisplay => $"{Name}, {HallsNumber} Halls; Supervised: [{Supervisor?.GivenName}, {Supervisor?.MiddleName}, {Supervisor?.SurName}]";
    }
}
