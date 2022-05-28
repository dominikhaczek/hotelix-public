using System.Threading.Tasks;
using Hotelix.Reservations.Models.Database;

namespace Hotelix.Reservations.Models
{
    public interface ILocationRepository
    {
        Task<Location> GetLocation(int id);
        Task<int> CreateLocation(Location location);
        Task<bool> Save();
    }
}