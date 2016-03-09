using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Address: BaseEntity
    {
        [Key]        
        public virtual int AddressId { get; set; }
    
        [Required]
        [MaxLength(512)]
        public virtual string Line1 { get; set; }

        [MaxLength(512)]
        public virtual string Line2 { get; set; }
        
        public virtual int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [Required]        
        [MinLength(5), MaxLength(10)]
        [Index("IX_Address_ZipCode", IsClustered = false, IsUnique = false)]
        public virtual string ZipCode { get; set; }

        string DebuggerDisplay => $"City: {City?.Name}/{CityId}; [{Line1}], [{Line2}]; Zip: {ZipCode}";
    }
}
