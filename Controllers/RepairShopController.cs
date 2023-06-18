using CarStory.Infrastructure;
using CarStory.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    public class RepairShopController : Controller
    {
        public IActionResult Menu()
        {
            var cards = new List<MenuCardsViewModel>
            {
                new MenuCardsViewModel {ImagePath = ImagePathConstants.AddCarImagePath, ImageText = MenuTextConstants.AddCar},
                new MenuCardsViewModel {ImagePath = ImagePathConstants.AddRepairImagePath, ImageText = MenuTextConstants.AddRepair},
                new MenuCardsViewModel {ImagePath = ImagePathConstants.FinishRepairImagePath, ImageText = MenuTextConstants.FinishRepair},
                new MenuCardsViewModel {ImagePath = ImagePathConstants.DeleteImagePath, ImageText = MenuTextConstants.DeleteCar}
            };

            return View(cards);
        }
    }
}
