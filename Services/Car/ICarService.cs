using CarStory.Models.Car;
using CarStory.Models.DTO.Car;

namespace CarStory.Services.Car
{
    public interface ICarService
    {
        string AddCar(AddCarViewModel carModel);

        CarDTO GetCar(string carId);

        List<CarDTO> GetAllCars();
    }
}
