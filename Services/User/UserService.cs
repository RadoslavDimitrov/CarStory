using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Models.User;
using Microsoft.AspNetCore.Identity;

namespace CarStory.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext data;
        private readonly RoleManager<AppUser> roleManager;
        private readonly UserManager<AppUser> userManager;

        public UserService(ApplicationDbContext data, 
            RoleManager<AppUser> roleManager,
            UserManager<AppUser> userManager) 
        {
            this.data = data;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        public AppUser GetUser(string username)
        {
            return data.Users.Where(u => u.UserName == username).FirstOrDefault();
        }

        public ProfileUserViewModel UserWithRole(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
