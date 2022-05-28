using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Hotelix.Reservations.Exceptions;
using Hotelix.Reservations.Models;
using Hotelix.Reservations.Models.Api;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;
using Hotelix.Reservations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Reservations.Controllers
{
    [Route("reservations")]
    [ApiController]
    [Authorize(Policy = "CanRead")]
    public class ReservationController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IOfferService _offerService;

        public ReservationController(
            IClientRepository clientRepository,
            ILocationRepository locationRepository,
            IReservationRepository reservationRepository,
            IMapper mapper,
            IOfferService offerService)
        {
            _clientRepository = clientRepository;
            _locationRepository = locationRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _offerService = offerService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservations();
            
            return Ok(_mapper.Map<IEnumerable<ReservationDto>>(reservations));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            var reservation = await _reservationRepository.GetReservation(id);

            if (reservation == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<ReservationDto>(reservation));
        }

        [HttpPost]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<int>> CreateReservation(ReservationToCreateDto reservation)
        {
            if (reservation.StartTime >= reservation.EndTime || reservation.StartTime < DateTime.Today)
            {
                return BadRequest();
            }
            
            // only client can create reservation, employee has to create client account first
            if (await _clientRepository.GetClient(reservation.UserId) == null)
            {
                return NotFound();
            }
            
            if (!await IsReservationAvailable(reservation) || !await IsRoomAvailable(reservation))
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed);
            }
            
            var reservationEntity = _mapper.Map<Reservation>(reservation);
            RoomApiModel room;
            
            try
            {
                room = await _offerService.GetRoom(reservation.RoomId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            
            var location = await _locationRepository.GetLocation(room.LocationId);

            if (location == null)
            {
                LocationApiModel tmpLocation;

                try
                {
                    tmpLocation = await _offerService.GetLocation(room.LocationId);
                }
                catch (NotFoundException)
                {
                    return NotFound();
                }
                
                //location = _mapper.Map<Location>(await _offerService.GetLocation(room.LocationId));
                location = new Location()
                {
                    Id = tmpLocation.Id,
                    Name = tmpLocation.Name,
                    Description = tmpLocation.Description,
                    Address = tmpLocation.Address,
                    City = tmpLocation.City,
                    PostalCode = tmpLocation.PostalCode
                };

                if (await _locationRepository.CreateLocation(location) == 0)
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable);
                }
            }

            //_mapper.Map(room, reservationEntity);
            reservationEntity.LocationId = room.LocationId;
            reservationEntity.Name = room.Name;
            reservationEntity.PricePerNight = room.PricePerNight;
            reservationEntity.GuestLimit = room.GuestLimit;
            reservationEntity.BedType = room.BedType;
            reservationEntity.Description = room.Description;
            reservationEntity.ImageUrl = room.ImageUrl;

            var createdReservationId = await _reservationRepository.CreateReservation(reservationEntity);
            if (createdReservationId == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return Ok(createdReservationId);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveReservation(int id)
        {
            var success = await _reservationRepository.RemoveReservation(id);
            
            if (!success)
            {
                return NotFound();
            }
            
            return NoContent();
        }
        
        [HttpPost("filter-available-rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RoomApiModel>>> FilterAvailableRooms(IEnumerable<RoomApiModel> rooms, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            if (startTime >= endTime || startTime < DateTime.Today)
            {
                return BadRequest();
            }
            
            var result = new List<RoomApiModel>();

            foreach (var room in rooms)
            {
                if (await IsReservationAvailable(new ReservationToCreateDto()
                {
                    UserId = null,
                    RoomId = room.Id,
                    StartTime = startTime,
                    EndTime = endTime
                }))
                {
                    result.Add(room);
                }
            }
            
            return Ok(result);
        }

        private async Task<bool> IsReservationAvailable(ReservationToCreateDto reservation)
        {
            return await _reservationRepository.IsReservationAvailable(reservation);
        }
        
        private async Task<bool> IsRoomAvailable(ReservationToCreateDto reservation)
        {
            var result = false;
            
            try
            {
                result = await _offerService.IsRoomAvailable(reservation.RoomId, reservation.StartTime, reservation.EndTime);
            }
            catch (NotFoundException) {}

            return result;
        }
    }
}