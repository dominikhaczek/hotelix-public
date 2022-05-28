using System.Threading.Tasks;
using AutoMapper;
using Hotelix.Reservations.Models;
using Hotelix.Reservations.Models.Database;
using Hotelix.Reservations.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Reservations.Controllers
{
    [Route("clients")]
    [ApiController]
    [Authorize(Policy = "CanRead")]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }
        
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientDto>> GetClient(string userId)
        {
            var client = await _clientRepository.GetClient(userId);

            if (client == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<ClientDto>(client));
        }
        
        [HttpPost]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<string>> CreateClient(ClientDto client)
        {
            var clientEntity = _mapper.Map<Client>(client);
            var createdClientUserId = await _clientRepository.CreateClient(clientEntity);
            if (createdClientUserId == null)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return Ok(createdClientUserId);
        }
        
        [HttpPut("{userId}")]
        [Authorize(Policy = "CanWrite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult> UpdateClient(string userId, ClientDto client)
        {
            var clientEntity = await _clientRepository.GetClient(userId);

            if (client == null || userId != client.UserId)
            {
                return NotFound();
            }

            _mapper.Map(client, clientEntity);
            var success = await _clientRepository.UpdateClient(clientEntity);
            return success ? NoContent() : StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}