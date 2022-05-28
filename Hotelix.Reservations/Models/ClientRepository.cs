using System.Threading.Tasks;
using Hotelix.Reservations.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotelix.Reservations.Models
{
    public class ClientRepository : IClientRepository
    {
        private readonly ReservationsDbContext _context;

        public ClientRepository(ReservationsDbContext context)
        {
            _context = context;
        }
        
        public async Task<Database.Client> GetClient(string userId)
        {
            return await _context.Client.FirstOrDefaultAsync(a => a.UserId == userId);
        }
        
        public async Task<string> CreateClient(Client client)
        {
            if (client == null)
            {
                return null;
            }

            await _context.AddAsync(client);
            return await Save() ? client.UserId : null;
        }
        
        public async Task<bool> UpdateClient(Client client)
        {
            if (client == null)
            {
                return false;
            }
            
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}