using System.Threading.Tasks;
using Hotelix.Reservations.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotelix.Reservations.Models
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ReservationsDbContext _context;

        public LocationRepository(ReservationsDbContext context)
        {
            _context = context;
        }
        
        public async Task<Location> GetLocation(int id)
        {
            return await _context.Location.FirstOrDefaultAsync(a => a.Id == id);
        }
        
        public async Task<int> CreateLocation(Location location)
        {
            if (location == null)
            {
                return 0;
            }

            await _context.AddAsync(location);
            return await Save() ? location.Id : 0;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}