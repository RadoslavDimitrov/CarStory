using CarStory.Infrastructure;
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

        public IActionResult Menu()
        {
            var cards = new List<MenuCardsViewModel>
            {
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.AddCarImagePath,
                    ImageText = MenuTextConstants.AddCar
                },
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.AddRepairImagePath,
                    ImageText = MenuTextConstants.AddRepair
                },
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.FinishRepairImagePath,
                    ImageText = MenuTextConstants.FinishRepair
                },
                new MenuCardsViewModel
                {
                    ImagePath = ImagePathConstants.DeleteImagePath,
                    ImageText = MenuTextConstants.DeleteCar
                }
            };

            return View(cards);
        }

        public IActionResult FinishRepair(int id)
        {
            var isFinished = this.repairShopService.FinishRepair(id);

            if (isFinished == false)
            {
                return this.RedirectToAction("ShopRepairs", "RepairShop");
            }

            return this.RedirectToAction("ViewRepair", "Car");
        }

        public IActionResult ShopRepairs()
        {
            var shopRepairs = this.repairShopService.GetAllRepairs(this.User.Identity.Name);

            return this.View(shopRepairs);
        }
    }
}
