using AutoMapper;
using Hotelix.Reservations.Models.Api;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;

namespace Hotelix.Reservations.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationDto>();
            CreateMap<LocationApiModel, Location>()
                .ForSourceMember(
                    source => source.IsHidden,
                    options => options.DoNotValidate());
        }
    }
}