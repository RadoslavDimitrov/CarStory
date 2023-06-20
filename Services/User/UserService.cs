using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Models.User;
using Microsoft.AspNetCore.Identity;

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
