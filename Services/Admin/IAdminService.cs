using CarStory.Models.DTO.RepairShop;

namespace CarStory.Services.Admin
{
    public interface IAdminService
    {
        List<RepairShopDTO> GetAllShops();
    }
}
