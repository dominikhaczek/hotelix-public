using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Reservations.Exceptions;
using Hotelix.Reservations.Models.Api;
using Hotelix.Reservations.Models.Database;

namespace Hotelix.Reservations.Services
{
    public interface IOfferService
    {
        /** <exception cref="NotFoundException"></exception> */
        Task<LocationApiModel> GetLocation(int id);
        
        
        
        /** <exception cref="NotFoundException"></exception> */
        Task<RoomApiModel> GetRoom(int id);
        
        /** <exception cref="NotFoundException"></exception> */
        Task<bool> IsRoomAvailable(int id, DateTime startTime, DateTime endTime);
    }
}