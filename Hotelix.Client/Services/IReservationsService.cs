using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Hotelix.Client.Models.Api;

namespace Hotelix.Client.Services
{
    public interface IReservationsService
    {
        /** <exception cref="NotFoundException"></exception> */
        Task<Models.Api.Client> GetClient(string userId);
        
        
        
        /** <exception cref="NotFoundException"></exception> */
        Task<ReservationsLocation> GetLocation(int id);

        
        
        Task<IEnumerable<Reservation>> GetAllReservations();
        
        /** <exception cref="NotFoundException"></exception> */
        Task<Reservation> GetReservation(int id);
        
        /**
         * <exception cref="BadRequestException"></exception>
         * <exception cref="NotFoundException"></exception>
         * <exception cref="MethodNotAllowedException"></exception>
         * <exception cref="ServiceUnavailableException"></exception>
         */
        Task<int> CreateReservation(ReservationToCreate reservation);
        
        /** <exception cref="NotFoundException"></exception> */
        Task RemoveReservation(int id);
    }
}