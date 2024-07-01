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
            var ip = await GetIp();
            var userLocation =  await GetUserLocation(ip);
            return new GreetingDto
            {
                ClientIp = ip,
                Location = userLocation,
                Greeting = "Hello, " + name + "!"
            };
        }

        private Task<string> GetIp()
        {
            var context = _httpContextAccessor.HttpContext;
            string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(ip))
            {
                ip = ip.Split(',').First().Trim();
            }
            else
            {
                ip = context.Connection.RemoteIpAddress.ToString();
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
            }

            // Convert IPv6-mapped IPv4 address to regular IPv4
            if (ip.StartsWith("::ffff:"))
            {
                ip = ip.Substring(7);
            }

            return Task.FromResult(ip);
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
