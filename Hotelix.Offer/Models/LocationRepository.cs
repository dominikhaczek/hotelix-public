using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Offer.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotelix.Offer.Models
{
    public class LocationRepository : ILocationRepository
    {

        private readonly OfferDbContext _context;

        public LocationRepository(OfferDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
           return await _context.Location.ToListAsync();
        }
        
        public async Task<Location> GetLocation(int id)
        {
            return await _context.Location.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
