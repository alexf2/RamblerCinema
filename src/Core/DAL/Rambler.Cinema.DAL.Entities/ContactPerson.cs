using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ContactPerson: Person
    {
        
        ICollection<Phone> _phones;
        public virtual ICollection<Phone> Phones
        {
            get { return _phones ?? (_phones = new List<Phone>()); }
            set { _phones = value; }
        }

        [Required]
        [MaxLength(64)]
        public virtual string Title { get; set; }

        public virtual int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [Required]
        public virtual Department Department { get; set; }      
                

        ICollection<Cinema> _cinemas;
        public virtual ICollection<Cinema> Cinemas
        {
            get { return _cinemas ?? (_cinemas = new List<Cinema>()); }
            set { _cinemas = value; }
        }

        string DebuggerDisplay => $"{GivenName} {MiddleName} {SurName}: {Title} in {Department?.Name}";
    }
}
