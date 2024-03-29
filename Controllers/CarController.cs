﻿using CarStory.Infrastructure;
using CarStory.Models;
using CarStory.Models.Car;
using CarStory.Models.DTO.Car;
using CarStory.Models.Repair;
using CarStory.Services.Car;
using CarStory.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            var CarId = await this.carService.AddCarAsync(car);

            if (CarId == null)
            {
                ModelState.AddModelError(ErrorMessageConstants.AlreadyExistCar, ErrorMessageConstants.AlreadyExistCarMsg);
                return View(car);
            }

            return this.RedirectToAction("Cars");
        }

        public async Task<IActionResult> ViewCar(string id)
        {
            var car = await this.carService.GetCarAsync(id);

            if (car == null)
            {
                var error = new ErrorViewModel
                {
                    Description = ErrorMessageConstants.CarDoesNotExist,
                    ActionName = "Cars",
                    ControllerName = "Car"
                };

                return this.RedirectToAction("Error", "Home", error);
            }

            if (this.User.IsInRole(RoleConstants.ShopRoleName))
            {
                var shopId = await userService.GetUserShopIdAsync(User.Identity.Name);

                if (shopId != null)
                {
                    TempData["ShopId"] = shopId;
                }

            }

            var carModel = new ViewCarViewModel
            {
                Car = car,
                IsUserAdded = false
            };

            if (this.User.IsInRole(RoleConstants.UserRoleName))
            {
                var user = this.userService.GetUser(User.Identity.Name);
                bool hasCar = user.Cars.Any(c => c.Id == id);
            }

            return this.View(carModel);
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
        public async Task<IActionResult> AddRepair(AddRepairViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var pendingRepairShopName = this.carService.HasPendingRepair(model.CarId);

            if (pendingRepairShopName != null)
            {

                var error = new ErrorViewModel
                {
                    ControllerName = "RepairShop",
                    ActionName = "ShopRepairs",
                    Description = ErrorMessageConstants.CarHasPendingRepair(pendingRepairShopName)
                };

                TempData["ShopName"] = pendingRepairShopName;


                return this.RedirectToAction("Error", "Home", error);
            }

            var newRepairId = await carService.AddRepairAsync(model);

            if (newRepairId == -1)
            {
                ModelState.AddModelError(string.Empty, "Car does not exist");
                return this.View(model);
            }

            if (newRepairId == -2)
            {
                ModelState.AddModelError(nameof(model.CarMilleage), "New car milleage cannot be lower to old car milleage");
                return this.View(model);
            }

            return this.RedirectToAction("ViewRepair", new { id = newRepairId });
        }


        public async Task<IActionResult> ViewRepair(int id)
        {
            var repair = await carService.GetRepairAsync(id);

            return this.View(repair);
        }

        public async Task<IActionResult> AddNextRepair(string id)
        {
            var carModel = await this.carService.GetCarAsync(id);

            if(carModel == null)
            {
                var error = new ErrorViewModel { ControllerName = "Car", ActionName = "Cars", Description = ErrorMessageConstants.CarDoesNotExist };
                return this.RedirectToAction("Home", "Error", error);
            }

            return this.View(carModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNextRepair(CarDTO model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            return this.View();
        }
    }
}
