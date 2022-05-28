using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hotelix.Offer.Exceptions;
using Hotelix.Offer.Models;
using Hotelix.Offer.Models.Database;
using Hotelix.Offer.Models.Dtos;
using Hotelix.Offer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Offer.Controllers
{
    [Route("rooms")]
    [ApiController]
    [Authorize(Policy = "CanRead")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IReservationsService _reservationsService;

        public RoomController(IRoomRepository roomRepository, IMapper mapper, IReservationsService reservationsService)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _reservationsService = reservationsService;
        }

        [HttpGet(Name = "GetRooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll([FromQuery] int? locationId)
        {
            var rooms = await _roomRepository.GetAllRooms(locationId);
            return Ok(_mapper.Map<IEnumerable<RoomDto>>(rooms));
        }

        [HttpGet("deleted", Name = "GetDeletedRooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetDeletedRooms()
        {
            var rooms = await _roomRepository.GetDeletedRooms();
            return Ok(_mapper.Map<IEnumerable<RoomDto>>(rooms));
        }

        [HttpGet("featured", Name= "GetFeaturedRooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetFeaturedRooms()
        {
            var rooms = await _roomRepository.GetFeaturedRooms();
            return Ok(_mapper.Map<IEnumerable<RoomDto>>(rooms));
        }

        [HttpGet("{roomId}", Name = "GetRoom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto>> GetById(int roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RoomDto>(room));
        }

        [HttpPost(Name = "CreateRoom")]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<RoomDto>> CreateRoom(RoomForCreationDto room)
        {
            var roomEntity = _mapper.Map<Room>(room);
            await _roomRepository.AddRoom(roomEntity);
            await _roomRepository.Save();

            var roomToReturn = _mapper.Map<RoomDto>(roomEntity);

            var links = CreateLinksForRoom(roomToReturn.Id, null);

            return CreatedAtRoute("GetRoom",
                new { roomId = roomToReturn.Id },
                roomToReturn);
        }

        [HttpPut("{roomId}", Name="UpdateRoom")]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int roomId, RoomForUpdateDto room)
        {
            var roomFromRepo = await _roomRepository.GetRoomById(roomId);

            if (roomFromRepo == null)
            {
                return NotFound();
            }

            // ALL IN ONE
            // 1. map the entity to a RoomForUpdateDto
            // 2. apply the updated field values to that dto
            // 3. map the RoomForUpdateDto back to entity
            _mapper.Map(room, roomFromRepo);

            _roomRepository.UpdateRoom(roomFromRepo); // even though this method is empty -> if the persistence method changes in the future we don't have error prone code
            await _roomRepository.Save();

            return NoContent();
        }

        [HttpPatch("{roomId}", Name = "PartiallyUpdateRoom")]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PartiallyUpdateRoom(int roomId, JsonPatchDocument<RoomForUpdateDto> patchDocument)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            
            if (room == null)
            {
                return NotFound();
            }

            var roomToPatch = _mapper.Map<RoomForUpdateDto>(room);

            patchDocument.ApplyTo(roomToPatch, ModelState);

            if (!TryValidateModel(roomToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(roomToPatch, room);

            _roomRepository.UpdateRoom(room);
            await _roomRepository.Save();

            return NoContent();
        }
        
        [HttpPost("filter-available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> FilterAvailableRooms(IEnumerable<RoomDto> rooms, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            if (startTime >= endTime || startTime < DateTime.Today)
            {
                return BadRequest();
            }
            
            var tmpResult = new List<RoomDto>();

            foreach (var room in rooms)
            {
                var tmpRoom = new Room()
                {
                    Id = room.Id,
                    LocationId = room.LocationId,
                    Name = room.Name,
                    PricePerNight = room.PricePerNight,
                    GuestLimit = room.GuestLimit,
                    BedType = room.BedType,
                    Description = room.Description,
                    ImageUrl = room.ImageUrl,
                    StartTime = room.StartTime,
                    EndTime = room.EndTime,
                    IsHidden = room.IsHidden
                };
                
                if (IsRoomAvailable(tmpRoom, startTime, endTime))
                {
                    tmpResult.Add(room);
                }
                
                /*if (IsRoomAvailable(_mapper.Map<Room>(room), startTime, endTime))
                {
                    tmpResult.Add(room);
                }*/
            }
            
            IEnumerable<RoomDto> result;

            try
            {
                result = await _reservationsService.FilterAvailableRooms(tmpResult, startTime, endTime);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            
            return Ok(result);
        }

        [HttpGet("{id}/is-available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> IsRoomAvailable(int id, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            /*var room = (await GetById(id)).Value;

            if (room == null)
            {
                return NotFound();
            }
            
            return Ok(IsRoomAvailable(_mapper.Map<Room>(room), startTime, endTime));*/
            
            
            
            var room = await _roomRepository.GetRoomById(id);
            
            if (room == null)
            {
                return NotFound();
            }

            return Ok(IsRoomAvailable(room, startTime, endTime));
        }
        
        private bool IsRoomAvailable(Room room, DateTime startTime, DateTime endTime)
        {
            return
                (room.StartTime == null && room.EndTime == null) ||
                (room.StartTime == null && room.EndTime >= endTime) ||
                (room.StartTime <= startTime && room.EndTime == null) ||
                (room.StartTime <= startTime && room.EndTime >= endTime);
        }

        private IEnumerable<LinkDto> CreateLinksForRoom(int roomId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(Url.Link("GetRoom", new { roomId = roomId }),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(Url.Link("GetRoom", new { authorId = roomId, fields }),
                        "self",
                        "GET"));
            }

            links.Add(
                new LinkDto(Url.Link("DeleteRoom", new { roomId = roomId }),
                    "delete_room",
                    "DELETE"));

            return links;
        }
    }
}
