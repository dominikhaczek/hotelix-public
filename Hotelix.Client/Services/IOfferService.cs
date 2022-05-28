using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Hotelix.Client.Models.Api;

namespace Hotelix.Client.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<Location>> GetAllLocations();
        
        
        
        Task<IEnumerable<Room>> GetAllRooms();
        
        Task<IEnumerable<Room>> GetRoomsByLocationId(int locationId);

        Task<IEnumerable<Room>> GetDeletedRooms();
        
        Task<IEnumerable<Room>> GetFeaturedRooms();
        
        /** <exception cref="NotFoundException"></exception> */
        Task<Room> GetRoom(int id);
        
        Task<Room> AddRoom(Room room);
        
        /** <exception cref="NotFoundException"></exception> */
        Task UpdateRoom(Room room);
        
        /**
         * <exception cref="BadRequestException"></exception>
         * <exception cref="NotFoundException"></exception>
         */
        Task HideRoom(int id);
        
        /**
         * <exception cref="BadRequestException"></exception>
         * <exception cref="NotFoundException"></exception>
         */
        Task UnHideRoom(int id);
        
        /** <exception cref="BadRequestException"></exception> */
        Task<IEnumerable<Room>> FilterAvailableRooms(IEnumerable<Room> rooms, DateTime startTime, DateTime endTime);
    }
}
