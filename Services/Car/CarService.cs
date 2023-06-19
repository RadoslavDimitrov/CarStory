using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.Car;
using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.RepairParts;
using CarStory.Models.Repair;
using Microsoft.EntityFrameworkCore;

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
            var car = this.data.Cars.Include(c => c.Repairs).ThenInclude(r => r.PartsChanged)
                .Where(c => c.Id == carId).Select(c => new CarDTO
                {
                    Id = carId,
                    Make = c.Make,
                    Milleage = c.Milleage,
                    Model = c.Model,
                    NextRepair = c.NextRepair,
                    NextRepairInfo = c.NextRepairInfo,
                    VinNumber = c.VinNumber,
                    YearManufactured = c.YearManufactured,
                    repairs = c.Repairs.Select(r => new RepairDTO
                    {
                        Id = r.Id,
                        CarId = r.CarId,
                        Description = r.Description,
                        CarRepairShopId = r.CarRepairShopId,
                        Status = r.Status,
                        PartsChanged = r.PartsChanged.Select(p => new RepairPartsDTO
                        {
                            Description = p.Part.Description,
                            Number = p.Part.Number
                        }).ToList()
                    }).ToList()

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

        public int AddRepair(AddRepairViewModel model)
        {
            var newRepair = new Repair
            {
                CarId = model.CarId,
                CarRepairShopId = model.CarRepairShopId,
                Description = model.Description,
                Status = RepairStatusEnum.Pending.ToString(),
                PartsChanged = new List<RepairParts>()
            };

            for (int i = 0; i < model.PartsChanged.Count; i++)
            {
                var newPart = new Part
                {
                    Number = model.PartsChanged[i].Number,
                    Description = model.PartsChanged[i].Description
                };

                if(!this.data.Parts.Any(p => p.Number == model.PartsChanged[i].Number))
                {
                    this.data.Parts.Add(newPart);
                }

                newRepair.PartsChanged.Add(new RepairParts { PartId = newPart.Id, RepairId = newRepair.Id });
            }

            this.data.SaveChanges();

            return newRepair.Id;
        }
    }
}
