using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.RepairShop;

namespace CarStory.Services.Admin
{
    public interface IAdminService
    {

        RepairShopDTO GetRepairShop(string id);

        bool ApproveShop(string id);

        
    }
}
