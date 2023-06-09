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


        public AppUser GetUser(string username)
        {
            return data.Users.Where(u => u.UserName == username).FirstOrDefault();
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
