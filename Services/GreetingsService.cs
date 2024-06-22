﻿using hngstageone.Dto;
using hngstageone.Interfaces;

namespace hngstageone.Services
{
    public class GreetingsService : IGreetingsRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GreetingsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GreetingDto> GreetAsync(string name)
        {
            var ip = await GetIp();
            return new GreetingDto
            {
                ClientIp = ip,
                Greeting = "Hello, " + name + "!"
            };
        }

        public Task<string> GetIp()
        {
            var context = _httpContextAccessor.HttpContext;
            string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return Task.FromResult(ip);
        }
    }
}
