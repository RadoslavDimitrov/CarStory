using CarStory.Models.DTO.Repair;

namespace CarStory.Models.CarRepairShop
{
    public class ShopRepairsViewModel
    {
        public List<RepairDTO> PendingRepairs { get; set; }
        public List<RepairDTO> FinishedRepairs { get; set; }
    }
}
