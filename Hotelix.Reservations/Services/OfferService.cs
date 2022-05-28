using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hotelix.Reservations.Extensions;
using Hotelix.Reservations.Models.Api;
using Hotelix.Reservations.Models.Database;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace Hotelix.Reservations.Services
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
        
        public async Task<LocationApiModel> GetLocation(int id)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync($"locations/{id}");
            return await response.ReadContentAs<LocationApiModel>();
        }

        public async Task<RoomApiModel> GetRoom(int id)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync($"rooms/{id}");
            return await response.ReadContentAs<RoomApiModel>();
        }

        public async Task<bool> IsRoomAvailable(int id, DateTime startTime, DateTime endTime)
        {
            _httpClient.SetBearerToken(await GetToken());
            var query = $"startTime={startTime.ToString("yyyy-MM-dd")}&endTime={endTime.ToString("yyyy-MM-dd")}";
            var response = await _httpClient.GetAsync($"rooms/{id}/is-available?{query}");
            return await response.ReadContentAs<bool>();
        }
    }
}