using CarStory.Infrastructure;
using CarStory.Models.Car;
using CarStory.Models.Repair;
using CarStory.Services.Car;
using CarStory.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService carService;
        private readonly IUserService userService;

        public CarController(ICarService carService, IUserService userService)
        {
            this.carService = carService;
            this.userService = userService;
        }
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult AddCar()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public async Task<IActionResult> AddCar(AddCarViewModel car)
        {
            if(!ModelState.IsValid) 
            {
                return View(car);
            }

            var CarId = await this.carService.AddCarAsync(car);

            if(CarId == null) 
            {
                return View(car);
            }

            return this.RedirectToAction("Cars");
        }

        public async Task<IActionResult> ViewCar(string id)
        {
            var car = await this.carService.GetCarAsync(id);

            if(car == null) 
            {
                ModelState.AddModelError(string.Empty, "Wrong car");
                return this.View(id);
            }

            if (this.User.IsInRole(RoleConstants.ShopRoleName))
            {
                var shopId = await userService.GetUserShopIdAsync(User.Identity.Name);

                if(shopId != null)
                {
                    TempData["ShopId"] = shopId;
                }

            }

           
            return this.View(car);
        }

        public async Task<IActionResult> Cars()
        {
            var cars = await carService.GetAllCarsAsync();

            return this.View(cars);
        }

        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult AddRepair(string carId, string carRepairShopId)
        {
            var model = new AddRepairViewModel
            {
                PartsChanged = new List<RepairPartsViewModel>()
            };
            return this.View(model);
        }


        //fix so admin can add shop ID and car repair
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult AddRepair(AddRepairViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var newRepairId = this.carService.AddRepair(model);

            if(newRepairId == -1)
            {
                ModelState.AddModelError(string.Empty, "Car does not exist");
                return this.View(model);
            }

            if(newRepairId == -2)
            {
                ModelState.AddModelError(nameof(model.CarMilleage), "New car milleage cannot be lower to old car milleage");
                return this.View(model);
            }

            return this.RedirectToAction("ViewRepair", new {id = newRepairId});
        }


        public IActionResult ViewRepair(int id)
        {
            var repair = this.carService.GetRepair(id);

            return this.View(repair);
        }
    }
}
