using AutoMapper;

namespace Hotelix.Offer.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Models.Database.Location, Models.Dtos.LocationDto>();
        }
    }
}
