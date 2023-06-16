using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    public class RepairShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
