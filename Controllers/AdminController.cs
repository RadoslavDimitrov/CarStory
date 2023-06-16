using CarStory.Infrastructure;
using CarStory.Models.CarRepairShop;
using CarStory.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRoleName)]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult Shops()
        {
            var shops = this.adminService.GetAllShops();

            return View(shops);
        }

        public IActionResult Cars()
        {
            var cars = this.adminService.GetAllCars();

            return this.View(cars);
        }

        [HttpPost]
        public IActionResult ApproveShop(string id)
        {
            var result = this.adminService.ApproveShop(id);

            if(result == false) 
            {
                return NotFound();
            }

            return this.RedirectToAction("Shops","Admin");
        }
    }
}
