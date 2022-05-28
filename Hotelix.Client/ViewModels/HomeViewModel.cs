using System.Collections.Generic;
using Hotelix.Client.Models.Api;


namespace Hotelix.Client.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Room> FeaturedRooms { get; set; }
    }
}
