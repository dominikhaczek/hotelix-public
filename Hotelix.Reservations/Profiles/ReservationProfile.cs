using AutoMapper;
using Hotelix.Reservations.Models.Api;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;

namespace Hotelix.Reservations.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationDto>()
                .ForMember(
                    destination => destination.ClientName,
                    options => options.MapFrom(
                        source => source.Client.Name))
                .ForMember(
                    destination => destination.ClientSurname,
                    options => options.MapFrom(
                        source => source.Client.Surname))
                .ForMember(
                    destination => destination.LocationName,
                    options => options.MapFrom(
                        source => source.Location.Name));

            CreateMap<ReservationToCreateDto, Reservation>();
            
            CreateMap<RoomApiModel, Reservation>()
                .ForSourceMember(
                    source => source.Id,
                    options => options.DoNotValidate())
                .ForSourceMember(
                    source => source.Location,
                    options => options.DoNotValidate())
                .ForSourceMember(
                    source => source.StartTime,
                    options => options.DoNotValidate())
                .ForSourceMember(
                    source => source.EndTime,
                    options => options.DoNotValidate())
                .ForSourceMember(
                    source => source.IsHidden,
                    options => options.DoNotValidate());
        }
    }
}