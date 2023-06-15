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

        public List<CarDTO> GetAllCars()
        {
            var cars = this.data.Cars.Select(c => new CarDTO
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Milleage = c.Milleage,
                NextRepair = c.NextRepair,
                NextRepairInfo = c.NextRepairInfo,
                VinNumber = c.VinNumber,
                YearManufactured = c.YearManufactured
            }).ToList();

            return cars;
        }

        public List<RepairShopDTO> GetAllShops()
        {
            var shops = this.data.CarRepairShops.Select(sh => new RepairShopDTO
            {
                Id = sh.Id,
                Name = sh.Name,
                Description = sh.Description,
                Email = sh.Email,
                Location = sh.Location,
                PhoneNumber = sh.PhoneNumber,
                IsApproved = sh.IsApproved,
            }).ToList();

            return shops;
        }

        public RepairShopDTO GetRepairShop(string id)
        {
            var shop = this.data.CarRepairShops.Select(sh => new RepairShopDTO
            {

                Name = sh.Name,
                Description = sh.Description,
                Email = sh.Email,
                PhoneNumber = sh.PhoneNumber,
                Location = sh.Location,
                IsApproved = sh.IsApproved
            })
                .Where(sh => sh.Id == id)
                .FirstOrDefault();

            return shop;
        }
    }
}
