using System;
using System.ComponentModel.DataAnnotations;

namespace Rambler.Cinema.DAL.Entities
{
    public class BaseEntity
    {
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
