using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Client.Models.Database;

namespace Hotelix.Client.Models
{
    public interface IClientRepository
    {
        Task AddClientAsync(Database.Client client);
        Task<IEnumerable<Database.Client>> GetAllClientsAsync();
        Task<Database.Client> GetClientByIdAsync(int id);
        Task<Database.Client> GetClientByUserIdAsync(string id);
        Task<Database.Client> GetClientByUserIdNoTrackingAsync(string id);
        Task UpdateClientAsync(Database.Client client);
        Task SoftDeleteClientByUserIdAsync(string userId);
        Database.Client GetClientByUserId(string id);
    }
}
