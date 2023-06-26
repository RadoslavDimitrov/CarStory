using CarStory.Models.CarRepairShop;

namespace CarStory.Services.RepairShop
{
    public interface IRepairShopService
    {
        bool FinishRepair(int id);

        ShopRepairsViewModel GetAllRepairs(string username);
    }
}
