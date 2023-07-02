using CarStory.Data;
using CarStory.Models.DTO.Car;
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

        public bool ApproveShop(string id)
        {
            var repairShop = this.data.CarRepairShops.Where(sh => sh.Id == id).FirstOrDefault();

            if (repairShop == null)
            {
                return false;
            }

            if(repairShop.IsApproved == false)
            {
                repairShop.IsApproved = true;
            }
            else
            {
                repairShop.IsApproved = false;
            }

            this.data.SaveChanges();

            return true;
        }

        public RepairShopDTO GetRepairShop(string id)
        {
            var shop = this.data.CarRepairShops.Where(sh => sh.Id == id).Select(sh => new RepairShopDTO
            {

                Name = sh.Name,
                Description = sh.Description,
                Email = sh.Email,
                PhoneNumber = sh.PhoneNumber,
                Location = sh.Location,
                IsApproved = sh.IsApproved
            })
                .FirstOrDefault();

            return shop;
        }
    }
}
