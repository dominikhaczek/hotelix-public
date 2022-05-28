using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Offer.Models.Database;

namespace Hotelix.Offer.Models
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRooms(int? locationId);
        Task<IEnumerable<Room>> GetDeletedRooms();
        Task<IEnumerable<Room>> GetFeaturedRooms();
        Task<Room> GetRoomById(int Id);
        Task<IEnumerable<Room>> GetRoomsAvailableForSelectedDate(DateTime startTime, DateTime endTime, int locationId);

        Task<bool> RoomExists(int roomId);

        Task AddRoom(Room room);
        void UpdateRoom(Room room);

        public Task<bool> Save();
    }
}
