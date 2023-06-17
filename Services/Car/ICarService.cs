using CarStory.Models.Car;

namespace CarStory.Services.Car
{
    public interface ICarService
    {
        string AddCar(AddCarViewModel carModel);
    }
}
