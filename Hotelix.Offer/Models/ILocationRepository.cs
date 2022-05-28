using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Offer.Models.Database;

namespace Hotelix.Offer.Models
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllLocations();
        Task<Location> GetLocation(int id);
    }
}
