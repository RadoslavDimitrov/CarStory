using CarStory.Models.CarRepairShop;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.RepairShop;

namespace CarStory.Services.RepairShop
{
    public interface IRepairShopService
    {
        bool FinishRepair(int id);

        ShopRepairsViewModel GetAllRepairs(string username);

        List<RepairShopDTO> Shops(string name, string location);
        RepairShopDTO Shop(string id);

        List<PendingRepairDTO> PendingRepairs(string vinNumber, string shopName);
        List<FinishedRepairsDTO> FinishedRepairs(string vinNumber, string shopName);

        RepairDTO GetRepair(int id);

        int EditRepair(RepairDTO repair);
    }
}
