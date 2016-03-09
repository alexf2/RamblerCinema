using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rambler.Cinema.DAL.Entities
{
    public class City: BaseEntity
    {
        [Key]
        public virtual int CityId { get; set; }

        [Required]
        [MaxLength(64)]
        [Index("UX_City_Name", IsClustered = false, IsUnique = true)]
        public virtual string Name { get; set; }
    }
}
