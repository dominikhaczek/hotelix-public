using System;

namespace Hotelix.Client.Models.Api
{
    public class ReservationToCreate
    {
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}