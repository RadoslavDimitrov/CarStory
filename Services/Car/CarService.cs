using CarStory.Data;
using CarStory.Models.Car;
using CarStory.Models.DTO.Car;

namespace CarStory.Services.Car
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext data;

        public CarService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public string AddCar(AddCarViewModel carModel)
        {
            bool isInDB = this.data.Cars.Any(c => c.VinNumber == carModel.VinNumber);

            if(isInDB) 
            {
                return null;
            }

            var car = new Data.Models.Car
            {
                Id = Guid.NewGuid().ToString(),
                VinNumber = carModel.VinNumber,
                Make = carModel.Make,
                Model = carModel.Model,
                Milleage = carModel.Milleage,
                YearManufactured = carModel.YearManufactured,
                NextRepair = carModel.NextRepair,
                NextRepairInfo = carModel.NextRepairInfo,
            };

            this.data.Cars.Add(car);
            this.data.SaveChanges();

            return car.Id;
        }

        public CarDTO GetCar(string carId)
        {
            var car = this.data.Cars.Where(c => c.Id == carId).Select(c => new CarDTO
            {
                Id=carId,
                Make = c.Make,
                Milleage=c.Milleage,
                Model = c.Model,
                NextRepair=c.NextRepair,
                NextRepairInfo = c.NextRepairInfo,
                VinNumber = c.VinNumber,
                YearManufactured = c.YearManufactured
                
            })
                .FirstOrDefault();

            return car;
        }

        public List<CarDTO> GetAllCars()
        {
            var cars = this.data.Cars.Select(c => new CarDTO
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Milleage = c.Milleage,
                NextRepair = c.NextRepair,
                NextRepairInfo = c.NextRepairInfo,
                VinNumber = c.VinNumber,
                YearManufactured = c.YearManufactured
            }).ToList();

            return cars;
        }
    }
}
