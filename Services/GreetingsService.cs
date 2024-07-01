using API.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using API.Dto;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;

namespace API.Services
{
    public class GreetingsService : IGreetingsRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public GreetingsService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<GreetingDto> GreetAsync(string name)
        {
            var ip = GetIp();
            var userLocation =  await GetUserLocation(ip);
            return new GreetingDto
            {
                ClientIp = ip,
                Location = userLocation,
                Greeting = "Hello, " + name + "!"
            };
        }

        private string GetIp()
        {
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;
            return ip?.ToString();
        }

        private async Task<string> GetUserLocation(string ip)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
                var locationData = JsonConvert.DeserializeObject<LocationData>(response);

                return locationData.city;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error fetching user location: {ex.Message}");
                return "Unknown";
            }
        }
    }
}
