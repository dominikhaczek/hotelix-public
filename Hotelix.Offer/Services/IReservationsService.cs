using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Offer.Exceptions;
using Hotelix.Offer.Models.Dtos;

namespace Hotelix.Offer.Services
{
    public interface IReservationsService
    {
        /** <exception cref="BadRequestException"></exception> */
        Task<IEnumerable<RoomDto>> FilterAvailableRooms(IEnumerable<RoomDto> rooms, DateTime startTime, DateTime endTime);
    }
}