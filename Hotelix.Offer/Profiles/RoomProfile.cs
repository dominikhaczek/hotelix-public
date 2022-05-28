using AutoMapper;

namespace Hotelix.Offer.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Models.Database.Room, Models.Dtos.RoomDto>()
                .ForMember(
                    dest => dest.Location,
                    opt => opt.MapFrom(src => src.Location.Name));
            
            CreateMap<Models.Dtos.RoomDto, Models.Database.Room>()
                .ForSourceMember(
                    source => source.Location,
                    options => options.DoNotValidate());

            CreateMap<Models.Dtos.RoomForCreationDto, Models.Database.Room>();

            CreateMap<Models.Dtos.RoomForUpdateDto, Models.Database.Room>();

            CreateMap<Models.Database.Room, Models.Dtos.RoomForUpdateDto>();

        }
    }
}
