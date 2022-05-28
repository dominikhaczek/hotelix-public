using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hotelix.IdentityServer.Extensions;
using Hotelix.IdentityServer.Models.Api;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace Hotelix.IdentityServer.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public ReservationsService(HttpClient httpClient, IConfiguration configuration)
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
                Scope = "reservations.read reservations.write"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }
        
        public async Task<Client> GetClient(string userId)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync($"clients/{userId}");
            return await response.ReadContentAs<Client>();
        }

        public async Task<string> CreateClient(Client client)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.PostAsJson("clients", client);
            return await response.ReadContentAsString();
        }
        
        public async Task UpdateClient(Client client)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.PutAsJson($"clients/{client.UserId}", client);
            response.CheckResponseStatusCode();
        }
    }
}