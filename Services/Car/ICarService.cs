using CarStory.Models.Car;
using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.Repair;
using CarStory.Models.Repair;

namespace CarStory.Services.Car
{
    public interface ICarService
    {
        Task<string> AddCar(AddCarViewModel carModel);

        CarDTO GetCar(string carId);

        List<CarDTO> GetAllCars();

        int AddRepair(AddRepairViewModel model);

        RepairDTO GetRepair(int  repairId);
    }
}
