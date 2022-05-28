using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;

namespace Hotelix.Reservations.Models
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllReservations();
        Task<Reservation> GetReservation(int id);
        Task<bool> IsReservationAvailable(ReservationToCreateDto reservation);
        Task<int> CreateReservation(Reservation reservation);
        Task<bool> RemoveReservation(int id);
        Task<bool> Save();
    }
}