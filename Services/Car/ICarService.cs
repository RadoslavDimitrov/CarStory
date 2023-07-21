using CarStory.Models.Car;
using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.Repair;
using CarStory.Models.Repair;

namespace CarStory.Services.Car
{
    public interface ICarService
    {
        Task<string> AddCarAsync(AddCarViewModel carModel);

        Task<CarDTO> GetCarAsync(string carId);

        Task<List<CarDTO>> GetAllCarsAsync();

        int AddRepair(AddRepairViewModel model);

        Task<RepairDTO> GetRepairAsync(int  repairId);
    }
}
