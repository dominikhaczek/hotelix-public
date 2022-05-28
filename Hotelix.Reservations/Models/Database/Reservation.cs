using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hotelix.Reservations.Models.Database
{
    public partial class Reservation
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }
        public int LocationId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(11, 4)")]
        public decimal PricePerNight { get; set; }
        public int GuestLimit { get; set; }
        [Required]
        [StringLength(50)]
        public string BedType { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [StringLength(2048)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Reservation")]
        public virtual Client Client { get; set; }
        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Reservation")]
        public virtual Location Location { get; set; }
    }
}
