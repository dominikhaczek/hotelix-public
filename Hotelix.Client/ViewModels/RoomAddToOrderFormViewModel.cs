using System;
using Hotelix.Client.Models.Api;


namespace Hotelix.Client.ViewModels
{
    public class RoomAddToOrderFormViewModel
    {
        public Room Room { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
