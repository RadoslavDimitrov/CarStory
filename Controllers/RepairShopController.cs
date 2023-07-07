﻿using CarStory.Infrastructure;
using CarStory.Models.DTO.Repair;
using CarStory.Models.Shared;
using CarStory.Services.RepairShop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    [Authorize(Roles = RoleConstants.ShopRoleName)]
    public class RepairShopController : Controller
    {
        private readonly IRepairShopService repairShopService;

        public RepairShopController(IRepairShopService repairShopService)
        {
            this.repairShopService = repairShopService;
        }

        [AllowAnonymous]
        public IActionResult Index(string name, string location)
        {
            var shops = this.repairShopService.Shops(name, location);

            return View(shops);
        }

        //Delete button not used?
        public IActionResult Menu()
        {
            var cards = new List<MenuCardsViewModel>
            {
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.AddCarImagePath,
                    ImageText = MenuTextConstants.AddCar,
                    ButtonController = MenuTextConstants.AddCarController,
                    ButtonAction = MenuTextConstants.AddCarAction
                },
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.PendingRepairsImgPath,
                    ImageText = MenuTextConstants.PendingRepairs,
                    ButtonController = MenuTextConstants.PendingRepairsController,
                    ButtonAction = MenuTextConstants.PendingRepairsAction
                },
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.FinishedRepairsImgPath,
                    ImageText = MenuTextConstants.FinishedRepairs,
                    ButtonController = MenuTextConstants.FinishedRepairsController,
                    ButtonAction = MenuTextConstants.FinishedRepairsAction
                },
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.DeleteImagePath,
                    ImageText = MenuTextConstants.DeleteCar,
                    ButtonController = MenuTextConstants.DeleteCarController,
                    ButtonAction = MenuTextConstants.DeleteCarAction
                }
            };

            return View(cards);
        }

        public IActionResult FinishedRepairs(string vinNumber)
        {
            var repairs = this.repairShopService.FinishedRepairs(vinNumber, this.User.Identity.Name);

            return View(repairs);
        }

        public IActionResult PendingRepairs(string vinNumber)
        {
            var repairs = this.repairShopService.PendingRepairs(vinNumber, this.User.Identity.Name);

            return View(repairs);
        }

        public IActionResult FinishRepair(int id)
        {
            var isFinished = this.repairShopService.FinishRepair(id);

            if (isFinished == false)
            {
                return this.RedirectToAction("ShopRepairs", "RepairShop");
            }

            //not working
            return this.RedirectToAction("ViewRepair", "Car", new { id });
        }

        public IActionResult ShopRepairs()
        {
            var shopRepairs = this.repairShopService.GetAllRepairs(this.User.Identity.Name);

            return this.View(shopRepairs);
        }

        public IActionResult EditRepair(int id)
        {
            var repair = this.repairShopService.GetRepair(id);

            if(repair == null)
            {
                //return not found repair
            }

            //make a view and post action
            return this.View(repair);
        }

        [HttpPost]
        public IActionResult EditRepair(RepairDTO repair)
        {
            return View();
        }
    }
}
