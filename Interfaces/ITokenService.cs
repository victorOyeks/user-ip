using hngstageone.Entities;

namespace hngstageone.Interfaces
{
    public interface ITokenService
    {
        String CreateToken(AppUser user);
    }
}
