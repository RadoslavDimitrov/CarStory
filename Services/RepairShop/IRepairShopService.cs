using CarStory.Models.CarRepairShop;
using CarStory.Models.DTO.RepairShop;

namespace CarStory.Services.RepairShop
{
    public interface IRepairShopService
    {
        bool FinishRepair(int id);

        ShopRepairsViewModel GetAllRepairs(string username);

        List<RepairShopDTO> Shops(string name, string location);
    }
}
