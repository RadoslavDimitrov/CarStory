using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.DTO.Car;
using CarStory.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarStory.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<AppUser> UserManager;


        public UserService(ApplicationDbContext data, UserManager<AppUser> userManager) 
        {
            this.data = data;
            this.UserManager = userManager;
        }

        public bool AddCarToMyCar(string id, string username)
        {
            var car = this.data.Cars.Where(c => c.Id == id).FirstOrDefault();

            if(car == null)
            {
                return false;
            }

            var currUser = this.data.Users.Where(u => u.UserName == username).FirstOrDefault();

            if(currUser == null)
            {
                return false;
            }

            if(currUser.Cars.Any(c => c.Id == id))
            {
                return false;
            }

            currUser.Cars.Add(car);
            this.data.SaveChanges();

            return true;
        }

        public void CreateShop(CarRepairShop shop)
        {
            this.data.CarRepairShops.Add(shop);
            this.data.SaveChanges();
        }

        public AppUser GetUser(string username)
        {
            return data.Users.Where(u => u.UserName == username).FirstOrDefault();
        }

        public string GetUserShopId(string username)
        {
            var userShop = this.data.CarRepairShops.Where(s => s.Name == username).FirstOrDefault();

            if (userShop == null) 
            {
                return null;
            }

            return userShop.Id;
        }

        public bool IsShopApproved(string username)
        {
            var shop = this.data.CarRepairShops.Where(sh => sh.Name == username).FirstOrDefault();

            if(shop == null)
            {
                return false;
            }

            return shop.IsApproved;
        }

        public List<CarDTO> MyCars(string username)
        {
            var user = this.data.Users.Where(u => u.UserName == username).FirstOrDefault();

            if(user == null)
            {
                return null;
            }

            var cars = user.Cars.Select(c => new CarDTO
            { 
                Id = c.Id,
                Make = c.Make,
                Milleage = c.Milleage,
                Model = c.Model,
                NextRepair = c.NextRepair,
                NextRepairInfo = c.NextRepairInfo,
                VinNumber = c.VinNumber,
                YearManufactured = c.YearManufactured,
                repairs = c.Repairs.Where(r => r.Status.ToString() == RepairStatusEnum.Finished.ToString())
                    .Select(r => new Models.DTO.Repair.RepairDTO
                    {
                        Status = r.Status,
                        CarId = r.CarId,
                        CarRepairShopId = r.CarRepairShopId,
                        CarRepairShopName = r.CarRepairShop.Name,
                        currCarMilleage = r.currCarMilleage,
                        DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                        DateFinished = r.DateFinished == null ? "" : r.DateFinished.ToString(),
                        Description = r.Description,
                        Id = r.Id,
                        PartsChanged = r.PartsChanged.Select(p => new Models.DTO.RepairParts.RepairPartsDTO
                        {
                            Description = p.Part.Description,
                            Number = p.Part.Number
                        }).ToList()
                    }).ToList()
            }).ToList();


            return cars;
        }

        public ProfileUserViewModel UserWithRole(string userId)
        {
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();

            ProfileUserViewModel model = new ProfileUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RoleName = this.GetUserRoleAsync(user).GetAwaiter().GetResult()
            };

            return model;
        }

        private async Task<string> GetUserRoleAsync(AppUser user)
        {
            var roles = await this.UserManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                return null;
            }

            return roles[0];
        }
    }
}
