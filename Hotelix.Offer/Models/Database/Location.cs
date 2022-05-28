using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hotelix.Offer.Models.Database
{
    public partial class Location
    {
        public Location()
        {
            Room = new HashSet<Room>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [StringLength(6)]
        public string PostalCode { get; set; }
        public bool IsHidden { get; set; }

        [InverseProperty("Location")]
        public virtual ICollection<Room> Room { get; set; }
    }
}
