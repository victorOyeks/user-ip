using hngstageone.Dto;

namespace hngstageone.Interfaces
{
    public interface IGreetingsRepository
    {
        Task<GreetingDto> GreetAsync(string name);
        Task<string> GetIp();
    }
}
