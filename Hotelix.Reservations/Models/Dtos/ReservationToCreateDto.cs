using System;

namespace Hotelix.Reservations.Models.Dtos
{
    public class ReservationToCreateDto
    {
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}