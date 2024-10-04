using MovieBookingAuthApi.Models;

namespace MovieBookingAuthApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User userObj);

    }
}
