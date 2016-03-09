using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Film: BaseEntity
    {
        [Key]
        public virtual int FilmId { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("IX_Film_Name", IsClustered = false, IsUnique = false)]
        public virtual string Name { get; set; }

        [Required]
        [Range(typeof(int), "1900", "2999")]
        [Index("IX_Film_YearFilmed", IsClustered = false, IsUnique = false)]
        public virtual int YearFilmed { get; set; }

        public virtual int ProducerId { get; set; }
        [ForeignKey("ProducerId")]
        public virtual Person Producer { get; set; }

        [Required]
        [Range(typeof(int), "20", "300")]        
        public virtual int DurationMinutes { get; set; }

        public virtual int GenreId { get; set; }
        [ForeignKey("GenreId")]
        [Required]
        public virtual Genre Genre { get; set; }
        

        ICollection<Session> _cinemaSessions;
        public virtual ICollection<Session> CinemaSessions
        {
            get { return _cinemaSessions ?? (_cinemaSessions = new List<Session>()); }
            set { _cinemaSessions = value; }
        }

        string DebuggerDisplay => $"{Genre?.Name} [{Name}]; Filed: {YearFilmed} by {Producer?.GivenName} {Producer?.SurName}; Lasts: {DurationMinutes}";
    }
}
