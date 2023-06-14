using CarStory.Data.Models;
using CarStory.Models.User;

namespace CarStory.Services.User
{
    public interface IUserService
    {
        AppUser GetUser(string username);

        ProfileUserViewModel UserWithRole(string userId);

        bool IsShopApproved(string username);

        void CreateShop(CarRepairShop shop);
    }
}
