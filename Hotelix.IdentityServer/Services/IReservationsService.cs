using System.Threading.Tasks;
using Hotelix.IdentityServer.Exceptions;
using Hotelix.IdentityServer.Models.Api;

namespace Hotelix.IdentityServer.Services
{
    public interface IReservationsService
    {
        /** <exception cref="NotFoundException"></exception> */
        Task<Client> GetClient(string userId);
        
        /** <exception cref="ServiceUnavailableException"></exception> */
        Task<string> CreateClient(Client client);
        
        /**
         * <exception cref="NotFoundException"></exception>
         * <exception cref="ServiceUnavailableException"></exception>
         */
        Task UpdateClient(Client client);
    }
}