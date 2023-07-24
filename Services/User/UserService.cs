using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.DTO.Car;
using CarStory.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        public async Task<bool> AddCarToMyCarAsync(string id, string username)
        {
            var car = await data.Cars.Where(c => c.Id == id).FirstOrDefaultAsync();

            if(car == null)
            {
                return false;
            }

            var currUser = await data.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();

            if(currUser == null)
            {
                return false;
            }

            if(currUser.Cars.Any(c => c.Id == id))
            {
                return false;
            }

            currUser.Cars.Add(car);
            await data.SaveChangesAsync();

            return true;
        }

        public async Task CreateShopAsync(CarRepairShop shop)
        {
            await data.CarRepairShops.AddAsync(shop);
            await data.SaveChangesAsync();
        }

        public async Task<bool> DeleteCarAsync(string id, string username)
        {
            var user = await data.Users.Include(u => u.Cars).Where(u => u.UserName == username).FirstOrDefaultAsync();

            if(user == null)
            {
                return false;
            }

            var car = await data.Cars.Where(c => c.Id ==id).FirstOrDefaultAsync();

            if(car == null)
            {
                return false;
            }

            user.Cars.Remove(car);
            await data.SaveChangesAsync();

            return true;
        }

        public AppUser GetUser(string username)
        {
            return data.Users.Where(u => u.UserName == username).FirstOrDefault();
        }

        public async Task<string> GetUserShopIdAsync(string username)
        {
            var userShop = await data.CarRepairShops.Where(s => s.Name == username).FirstOrDefaultAsync();

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

        public async Task<List<CarDTO>> MyCarsAsync(string username)
        {
            var user = await data.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();

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
                YearManufactured = c.YearManufactured              
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
