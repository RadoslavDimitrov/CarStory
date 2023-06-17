using CarStory.Data;
using CarStory.Models.Car;

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
    }
}
