using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.CarRepairShop;
using CarStory.Models.User;
using CarStory.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static CarStory.Infrastructure.RoleConstants;

namespace CarStory.Controllers
{
    //TODO remove all magical strings

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;

        public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userService = userService;
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return this.View(model);
            }

            if (await roleManager.RoleExistsAsync(UserRoleName))
            {
                await userManager.AddToRoleAsync(user, UserRoleName);
            }
            else
            {
                var role = new IdentityRole { Name = UserRoleName };

                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, UserRoleName);
            };


            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserFormModel model)
        {

            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return this.InvalidLoginAttempt(model, ErrorMessageConstants.WrongUsarnameOrPassword);
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordIsValid)
            {
                return this.InvalidLoginAttempt(model, ErrorMessageConstants.WrongUsarnameOrPassword);
            }

            if (await userManager.IsInRoleAsync(user, "shop"))
            {
                bool isApproved = this.userService.IsShopApproved(user.UserName);

                if (isApproved == false)
                {
                    return this.InvalidLoginAttempt(model, ErrorMessageConstants.ShopNotApproved);
                }
            }

            await this.signInManager.SignInAsync(user, true);

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult RegisterRepairShop()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRepairShop(RegisterRepairShopViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }


            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Name
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return this.View(model);
            }

            if (await roleManager.RoleExistsAsync(ShopRoleName))
            {
                await userManager.AddToRoleAsync(user, ShopRoleName);
            }
            else
            {
                var role = new IdentityRole { Name = ShopRoleName };

                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, ShopRoleName);
            };

            var repairShop = new CarRepairShop()
            {
                Name = model.Name,
                Email = model.Email,
                Location = model.Location,
                Description = model.Description,
                PhoneNumber = model.PhoneNumber,
            };

            this.userService.CreateShop(repairShop);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }



        [Authorize]
        public IActionResult Profile()
        {
            var user = this.userService.UserWithRole(this.User.GetId());

            if (user == null)
            {
                return View("NotFound", "User does not exist");
            }

            return this.View(user);
        }
        [Authorize]
        public IActionResult AddCarToMyCar(string id)
        {
            var result = this.userService.AddCarToMyCar(id, this.User.Identity.Name);

            if (result == false)
            {
                return RedirectToAction("Search", "Home");
            }

            return RedirectToAction("MyCars");
        }


        [Authorize]
        public IActionResult MyCars()
        {
            var cars = this.userService.MyCars(this.User.Identity.Name);

            return this.View(cars);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return this.View("NotFound", "User does not exist");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return this.View();
            }

            await signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Profile", "User");
        }


        private IActionResult InvalidLoginAttempt(LoginUserFormModel model, string message)
        {
            ModelState.AddModelError(string.Empty, message);

            return this.View(model);
        }

    
    }
}
