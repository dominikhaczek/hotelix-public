using System.Threading.Tasks;
using Hotelix.Reservations.Models.Database;

namespace Hotelix.Reservations.Models
{
    public interface IClientRepository
    {
        Task<Database.Client> GetClient(string userId);
        Task<string> CreateClient(Client client);
        Task<bool> UpdateClient(Client client);
        Task<bool> Save();
    }
}