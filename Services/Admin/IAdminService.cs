using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.RepairShop;

namespace CarStory.Services.Admin
{
    public interface IAdminService
    {
        List<RepairShopDTO> GetAllShops();

        RepairShopDTO GetRepairShop(string id);

        List<CarDTO> GetAllCars();
    }
}
