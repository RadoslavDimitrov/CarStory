using CarStory.Infrastructure;
using CarStory.Models;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.Review;
using CarStory.Models.Shared;
using CarStory.Services.RepairShop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CarStory.Controllers
{

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

        [AllowAnonymous]
        public IActionResult Visit(string id)
        {
            var shop = this.repairShopService.Shop(id);

            if(shop == null)
            {
                var error = new ErrorViewModel
                {
                    ControllerName = "RepairShop",
                    ActionName = "Index",
                    Description = ErrorMessageConstants.RepairShopNotExist
                };

                return RedirectToAction("Home", "Error", error);
            }

            return this.View(shop);
        }

        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}, {RoleConstants.UserRoleName}")]
        public async Task<IActionResult> AddReview(string id)
        {
            var shop = this.repairShopService.Shop(id);

            if(shop == null)
            {
                var error = new ErrorViewModel
                { ActionName = "Index", ControllerName = "RepairShop", Description = ErrorMessageConstants.RepairShopNotExist };
                return RedirectToAction("Error", "Home", error);
            }

            var model = new ReviewDTO
            {
                ShopId = id
            };

            return this.View(model);
        }
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}, {RoleConstants.UserRoleName}")]
        public async Task<IActionResult> AddReview(ReviewDTO model)
        {
            if (!ModelState.IsValid)
            {
                //TODO fix the model.description 
                ModelState.AddModelError(model.Description, "Description must be between 5 and 250 characters long!");
                return this.View(model);
            }

            var result = await this.repairShopService.AddReviewAsync(model);
            
            if(result == false)
            {
                var error = new ErrorViewModel
                { ActionName = "Index", ControllerName = "RepairShop", Description = ErrorMessageConstants.RepairShopNotExist };
                return RedirectToAction("Error", "Home", error);
            }
            return this.RedirectToAction("Visit", new {id = model.ShopId});
        }

        //Delete button not used?
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
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
                    ImagePath = ImagePathConstants.AllRepairsPath,
                    ImageText = MenuTextConstants.AllRepairs,
                    ButtonController = MenuTextConstants.AllRepairsController,
                    ButtonAction = MenuTextConstants.AllRepairsAction
                }
            };

            return View(cards);
        }
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult FinishedRepairs(string vinNumber)
        {
            var repairs = this.repairShopService.FinishedRepairs(vinNumber, this.User.Identity.Name);

            return View(repairs);
        }
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult PendingRepairs(string vinNumber)
        {
            var repairs = this.repairShopService.PendingRepairs(vinNumber, this.User.Identity.Name);

            return View(repairs);
        }
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult FinishRepair(int id)
        {
            var isFinished = this.repairShopService.FinishRepair(id);

            if (isFinished == false)
            {
                return this.RedirectToAction("ShopRepairs", "RepairShop");
            }

            
            return this.RedirectToAction("ViewRepair", "Car", new { id });
        }

        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult ShopRepairs()
        {
            var shopRepairs = this.repairShopService.GetAllRepairs(this.User.Identity.Name);

            return this.View(shopRepairs);
        }

        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult EditRepair(int id)
        {
            var repair = this.repairShopService.GetRepair(id);

            if(repair == null)
            {
                var error = new ErrorViewModel
                { ActionName = "ShopRepairs", ControllerName = "RepairShop", Description = ErrorMessageConstants.RepairDoesNotExistMsg };
                return RedirectToAction("Error", "Home", error);
            }

            return this.View(repair);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.AdminRoleName}, {RoleConstants.ShopRoleName}")]
        public IActionResult EditRepair(RepairDTO repair)
        {
            //add parts to DB
            var result = this.repairShopService.EditRepair(repair);
            //Add parts to repair
            if(result == -1)
            {
                ModelState.AddModelError(string.Empty, "Repair does not exist");
                return this.View(repair);
            }else if(result == -2)
            {
                ModelState.AddModelError(nameof(repair.currCarMilleage), "current car milleage cannot be lower that the car's millleage");
                return this.View(repair);
            }


            return RedirectToAction("ViewRepair", "Car", new {id = repair.Id});
        }
    }
}
