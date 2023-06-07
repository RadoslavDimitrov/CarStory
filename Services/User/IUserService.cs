using CarStory.Data.Models;

namespace CarStory.Services.User
{
    public interface IUserService
    {
        AppUser GetUser(string username);
    }
}
