using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Offer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Hotelix.Offer.Models.Database;
using Hotelix.Offer.Utilities;

namespace Hotelix.Offer.Models   
{
    public class RoomRepository : IRoomRepository
    {
        private readonly OfferDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomRepository(OfferDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Room>> GetAllRooms(int? locationId)
        {
            return await _context.Room.Where(c => c.IsHidden != true)
                .Include(r => r.Location)
                .Where(x => (x.LocationId == locationId || locationId == null)).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetDeletedRooms()
        {
            return await _context.Room.Where(c => c.IsHidden == true).Include(r => r.Location).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetFeaturedRooms()
        {
            return await _context.Room.Include(r => r.Location).Skip(1).Take(3).ToListAsync();
        }

        public async Task<Room> GetRoomById(int Id)
        {
            return await _context.Room.Include(r => r.Location).FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<IEnumerable<Room>> GetRoomsAvailableForSelectedDate(DateTime startTime, DateTime endTime, int locationId)
        {
            if (startTime >= endTime)
            {
                return new List<Room>();
            }

            return null;
            //return _context.Room.Where
            //    (
            //        c => 
            //            c.IsHidden != true &&
            //            ( // startTime i endTime musi sie zawierac w przedziale <Vehicle.StartTime,Vehicle.EndTime>
            //                (c.Vehicle.StartTime <= startTime && c.Vehicle.EndTime >= startTime) &&
            //                (c.Vehicle.StartTime <= endTime && c.Vehicle.EndTime >= endTime)
            //            ) &&
            //            ( 
            //              //dla kazdej rezerwacji jest spelniony warunek ze albo: startTime i endTime jest mniejsze od Vehicle.Reservations.StartTime
            //              //albo  startTime i endTime jest wieksze od Vehicle.Reservations.EndTime
            //              c.Vehicle.Reservations.All(r => r.StartTime > startTime && r.StartTime > endTime || r.EndTime < startTime && r.EndTime < endTime)
            //    ) &&
            //            (category == "Wszystkie" || c.Vehicle.VehicleCategory.Name == category)
            //        )
            //    .Include(c => c.Vehicle)
            //    .ThenInclude(x => x.VehicleCategory)
            //    .OrderBy(c => c.Id);
        }

        public async Task<bool> RoomExists(int roomId)
        {
            return await _context.Room.AnyAsync(a => a.Id == roomId);
        }

        public async Task AddRoom(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }
            await _context.AddAsync(room);
        }

        public void UpdateRoom(Room room)
        {
            // no implementation needed - EF handles this (this exists only for purpose of repository, if the implementation changes in the future)
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }
    }
}
