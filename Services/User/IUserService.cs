using CarStory.Data.Models;
using CarStory.Models.DTO.Car;
using CarStory.Models.User;

namespace CarStory.Services.User
{
    public interface IUserService
    {
        AppUser GetUser(string username);

        ProfileUserViewModel UserWithRole(string userId);

        bool IsShopApproved(string username);

        Task CreateShopAsync(CarRepairShop shop);

        Task<string> GetUserShopIdAsync(string username);

        Task<bool> AddCarToMyCarAsync(string id, string username);

        Task<List<CarDTO>> MyCarsAsync(string username);

        Task<bool> DeleteCarAsync(string id, string username);
    }
}
