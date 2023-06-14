using CarStory.Data;
using CarStory.Models.DTO.RepairShop;

namespace CarStory.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext data;

        public AdminService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public List<RepairShopDTO> GetAllShops()
        {
            var shops = this.data.CarRepairShops.Select(sh => new RepairShopDTO
            {
                Name = sh.Name,
                Description = sh.Description,
                Email = sh.Email,
                Location = sh.Location,
                PhoneNumber = sh.PhoneNumber,
                IsApproved = sh.IsApproved,
            }).ToList();

            return shops;
        }
    }
}
