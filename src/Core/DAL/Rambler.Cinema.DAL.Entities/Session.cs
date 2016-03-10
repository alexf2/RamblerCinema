using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rambler.Cinema.DAL.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Session
    {
        [Required]
        [Key, Column(Order = 0)]
        public virtual DateTime StartTime { get; set; }

        [Key, Column(Order = 1)]
        public virtual int FilmId { get; set; }
        [ForeignKey("FilmId")]
        [Required]
        public virtual Film Film { get; set; }

        [Key, Column(Order = 2)]
        public virtual int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        [Required]
        public virtual Cinema Cinema { get; set; }

        string DebuggerDisplay => $"{StartTime} Film: {Film.Name} in {Cinema.Name}";
    }
}
