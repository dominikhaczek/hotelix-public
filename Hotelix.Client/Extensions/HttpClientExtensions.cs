using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Hotelix.Client.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PutAsync(url, content);
        }

        public static Task<HttpResponseMessage> PatchAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PatchAsync(url, content);
        }
        
        /**
         * <exception cref="BadRequestException"></exception>
         * <exception cref="NotFoundException"></exception>
         * <exception cref="MethodNotAllowedException"></exception>
         * <exception cref="ServiceUnavailableException"></exception>
         * <exception cref="ApplicationException"></exception>
         */
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            var dataAsString = await response.ReadContentAsString();

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        
        /**
         * <exception cref="BadRequestException"></exception>
         * <exception cref="NotFoundException"></exception>
         * <exception cref="MethodNotAllowedException"></exception>
         * <exception cref="ServiceUnavailableException"></exception>
         * <exception cref="ApplicationException"></exception>
         */
        public static async Task<string> ReadContentAsString(this HttpResponseMessage response)
        {
            response.CheckResponseStatusCode();

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return dataAsString;
        }

        /**
         * <exception cref="BadRequestException"></exception>
         * <exception cref="NotFoundException"></exception>
         * <exception cref="MethodNotAllowedException"></exception>
         * <exception cref="ServiceUnavailableException"></exception>
         * <exception cref="ApplicationException"></exception>
         */
        public static void CheckResponseStatusCode(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new BadRequestException();
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException();
                    case HttpStatusCode.MethodNotAllowed:
                        throw new MethodNotAllowedException();
                    case HttpStatusCode.ServiceUnavailable:
                        throw new ServiceUnavailableException();
                    default:
                        throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                }
            }
        }
    }
}
