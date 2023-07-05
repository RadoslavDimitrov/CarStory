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

        void CreateShop(CarRepairShop shop);

        string GetUserShopId(string username);

        bool AddCarToMyCar(string id, string username);

        List<CarDTO> MyCars(string username);

        bool DeleteCar(string id, string username);
    }
}
