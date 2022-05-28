using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Hotelix.Reservations.Models
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationsDbContext _context;

        public ReservationRepository(ReservationsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _context.Reservation
                .Include(a => a.Client)
                .Include(a=>a.Location)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservation(int id)
        {
            return await _context.Reservation
                .Include(a => a.Client)
                .Include(a=>a.Location)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> IsReservationAvailable(ReservationToCreateDto reservation)
        {
            /*var result = await _context.Reservation
                .FirstOrDefaultAsync(a => a.RoomId == reservation.RoomId &&
                                          (a.EndTime < reservation.StartTime) ||
                                          (a.EndTime == reservation.StartTime) ||
                                          (a.StartTime < reservation.StartTime && a.EndTime > reservation.StartTime && a.EndTime < reservation.EndTime) ||
                                          (a.StartTime < reservation.StartTime && a.EndTime > reservation.StartTime && a.EndTime == reservation.EndTime) ||
                                          (a.StartTime < reservation.StartTime && a.EndTime > reservation.StartTime && a.EndTime > reservation.EndTime) ||
                                          (a.StartTime == reservation.StartTime && a.EndTime > reservation.StartTime && a.EndTime < reservation.EndTime) ||
                                          (a.StartTime == reservation.StartTime && a.EndTime > reservation.StartTime && a.EndTime == reservation.EndTime) ||
                                          (a.StartTime == reservation.StartTime && a.EndTime > reservation.StartTime && a.EndTime > reservation.EndTime) ||
                                          (a.StartTime > reservation.StartTime && a.StartTime < reservation.EndTime && a.EndTime < reservation.EndTime) ||
                                          (a.StartTime > reservation.StartTime && a.StartTime < reservation.EndTime && a.EndTime == reservation.EndTime) ||
                                          (a.StartTime > reservation.StartTime && a.StartTime < reservation.EndTime && a.EndTime > reservation.EndTime) ||
                                          (a.StartTime == reservation.EndTime) ||
                                          (a.StartTime > reservation.EndTime));*/
            
            return !await _context.Reservation
                .AnyAsync(a => a.RoomId == reservation.RoomId && (
                    (a.StartTime < reservation.StartTime && a.EndTime > reservation.StartTime) ||
                    (a.StartTime == reservation.StartTime) ||
                    (a.StartTime > reservation.StartTime && a.StartTime < reservation.EndTime)));
        }
        
        public async Task<int> CreateReservation(Reservation reservation)
        {
            if (reservation == null)
            {
                return 0;
            }

            await _context.AddAsync(reservation);
            return await Save() ? reservation.Id : 0;
        }

        public async Task<bool> RemoveReservation(int id)
        {
            var reservation = await GetReservation(id);

            if (reservation == null)
            {
                return false;
            }

            _context.Reservation.Remove(reservation);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}