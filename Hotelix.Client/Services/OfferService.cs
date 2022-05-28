using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hotelix.Client.Extensions;
using Hotelix.Client.Models.Api;
using IdentityModel.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Hotelix.Client.Services
{
    public class OfferService : IOfferService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public OfferService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task<string> GetToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                return _accessToken;
            }

            var discoveryDocumentResponse = await _httpClient
                .GetDiscoveryDocumentAsync(_configuration["ApiConfigs:IdentityServer:Uri"]);
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = "hotelix", //"hotelixm2m",
                ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0", //"511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "offer.read offer.write"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }
        
        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync("locations");
            return await response.ReadContentAs<List<Location>>();
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync("rooms");
            return await response.ReadContentAs<List<Room>>();
        }
        
        public async Task<IEnumerable<Room>> GetRoomsByLocationId(int locationId)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync($"rooms/?locationId={locationId}");
            return await response.ReadContentAs<List<Room>>();
        }

        public async Task<IEnumerable<Room>> GetDeletedRooms()
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync("rooms/deleted");
            return await response.ReadContentAs<List<Room>>();
        }

        public async Task<IEnumerable<Room>> GetFeaturedRooms()
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync("rooms/featured");
            return await response.ReadContentAs<List<Room>>();
        }
        
        public async Task<Room> GetRoom(int id)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync($"rooms/{id}");
            return await response.ReadContentAs<Room>();
        }

        public async Task<Room> AddRoom(Room room)
        {
            //var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            //addEditRoomModel.Car.Vehicle.ImageUrl = ImageManager.SaveImage(addEditRoomModel.Image, uploads);

            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.PostAsJson($"rooms", room);
            return await response.ReadContentAs<Room>();
        }

        public async Task UpdateRoom(Room room)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.PutAsJson($"rooms/{room.Id}", room);
            response.CheckResponseStatusCode();
        }

        public async Task HideRoom(int id)
        {
            _httpClient.SetBearerToken(await GetToken());
            var jsonPatchDoc = new JsonPatchDocument<Room>();
            jsonPatchDoc.Replace(p => p.IsHidden, true);

            var response = await _httpClient.PatchAsJson($"rooms/{id}", jsonPatchDoc);
            response.CheckResponseStatusCode();
        }

        public async Task UnHideRoom(int id)
        {
            _httpClient.SetBearerToken(await GetToken());
            var jsonPatchDoc = new JsonPatchDocument<Room>();
            jsonPatchDoc.Replace(p => p.IsHidden, false);

            var response = await _httpClient.PatchAsJson($"rooms/{id}", jsonPatchDoc);
            response.CheckResponseStatusCode();
        }
        
        public async Task<IEnumerable<Room>> FilterAvailableRooms(IEnumerable<Room> rooms, DateTime startTime, DateTime endTime)
        {
            _httpClient.SetBearerToken(await GetToken());
            var query = $"startTime={startTime.ToString("yyyy-MM-dd")}&endTime={endTime.ToString("yyyy-MM-dd")}";
            var response = await _httpClient.PostAsJson($"rooms/filter-available?{query}", rooms);
            return await response.ReadContentAs<IEnumerable<Room>>();
        }
    }
}
