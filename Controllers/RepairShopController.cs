using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    public class RepairShopController : Controller
    {
        public IActionResult Menu()
        {
            return View();
        }
    }
}
