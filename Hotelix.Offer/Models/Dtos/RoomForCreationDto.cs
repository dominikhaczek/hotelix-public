using System;

namespace Hotelix.Offer.Models.Dtos
{
    public class RoomForCreationDto
    { 
        public int LocationId { get; set; }
        public string Name { get; set; }
        public decimal PricePerNight { get; set; }
        public int GuestLimit { get; set; }
        public string BedType { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsHidden { get; set; }
    }
}
