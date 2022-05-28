using System.Collections.Generic;
using Hotelix.Client.Models.Api;

namespace Hotelix.Client.Models
{
    public class OfferManagementModel
    {
        public IEnumerable<Room> RoomEnum { get; }

        public OfferManagementModel(IEnumerable<Room> roomEnum)
        {
            RoomEnum = roomEnum;
        }
    }
}
