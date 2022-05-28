using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Client.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotelix.Client.Models
{
    public class ClientRepository : IClientRepository
    {
        private readonly RentalDockDbContext _context;

        public ClientRepository(RentalDockDbContext context)
        {
            _context = context;
        }


        public async Task AddClientAsync(Database.Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Database.Client>> GetAllClientsAsync() {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Database.Client> GetClientByIdAsync(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Database.Client> GetClientByUserIdAsync(string id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task<Database.Client> GetClientByUserIdNoTrackingAsync(string id)
        {
            return await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task UpdateClientAsync(Database.Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteClientByUserIdAsync(string userId)
        {
            var client = await GetClientByUserIdAsync(userId);
            client.UserId = "b75a83d9-4950-4946-ad9c-935f991b3a0f";
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public Database.Client GetClientByUserId(string id)
        {
            return _context.Clients.FirstOrDefault(c => c.UserId == id);
        }


    }
}
