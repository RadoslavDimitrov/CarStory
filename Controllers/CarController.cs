using CarStory.Models.Car;
using CarStory.Services.Car;
using Microsoft.AspNetCore.Mvc;

namespace CarStory.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService carService;

        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        public IActionResult AddCar()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AddCar(AddCarViewModel car)
        {
            if(!ModelState.IsValid) 
            {
                return View(car);
            }

            var CarId = this.carService.AddCar(car);

            if(CarId == null) 
            {
                return View(car);
            }

            return this.View(car);
        }

        public IActionResult ViewCar(string id)
        {
            var car = this.carService.GetCar(id);

            if(car == null) 
            {
                ModelState.AddModelError(string.Empty, "Wrong car");
                return this.View(id);
            }

            //Update the View with the info about repairs 
            return this.View(car);
        }

        public IActionResult Cars()
        {
            var cars = this.carService.GetAllCars();

            return this.View(cars);
        }
    }
}
