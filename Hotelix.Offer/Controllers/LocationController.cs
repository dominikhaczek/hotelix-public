using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hotelix.Offer.Models;
using Hotelix.Offer.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Offer.Controllers
{
    [Route("locations")]
    [ApiController]
    [Authorize(Policy = "CanRead")]
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetAll()
        {
            var locations = await _locationRepository.GetAllLocations();
            return Ok(_mapper.Map<IEnumerable<LocationDto>>(locations));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
            var location = await _locationRepository.GetLocation(id);

            if (location == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<LocationDto>(location));
        }
    }
}
