using System;

namespace Hotelix.Reservations.Models.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        public decimal PricePerNight { get; set; }
        public int GuestLimit { get; set; }
        public string BedType { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}