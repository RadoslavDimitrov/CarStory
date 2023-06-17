using CarStory.Models;
using CarStory.Services.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarStory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISearchService searchService;

        public HomeController(ILogger<HomeController> logger, ISearchService searchService)
        {
            _logger = logger;
            this.searchService = searchService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string? vinNumber)
        {
            if (String.IsNullOrEmpty(vinNumber))
            {
                return View();
            }


            var car = searchService.SearchCar(vinNumber);

            return View(car);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}