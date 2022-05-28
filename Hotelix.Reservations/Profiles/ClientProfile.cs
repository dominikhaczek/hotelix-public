using AutoMapper;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;

namespace Hotelix.Reservations.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Models.Database.Client, ClientDto>();
            CreateMap<ClientDto, Models.Database.Client>();
        }
    }
}