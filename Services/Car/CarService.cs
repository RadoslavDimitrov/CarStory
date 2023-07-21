﻿using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.Car;
using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.RepairParts;
using CarStory.Models.Repair;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarStory.Services.Car
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext data;

        public CarService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<string> AddCar(AddCarViewModel carModel)
        {
            bool isInDB = await data.Cars.AnyAsync(c => c.VinNumber == carModel.VinNumber);

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

            await this.data.Cars.AddAsync(car);
            await this.data.SaveChangesAsync();

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
                        CarRepairShopName = r.CarRepairShop.Name,
                        currCarMilleage = r.currCarMilleage,
                        DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                        DateFinished = r.DateFinished == null ? "" : Convert.ToDateTime(r.DateFinished).ToString("dd/MM/yyyy"),
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
            var cars = this.data.Cars.Include(c => c.Repairs).ThenInclude(r => r.PartsChanged)
                .Select(c => new CarDTO
                {
                    Id = c.Id,
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
                        CarRepairShopName = r.CarRepairShop.Name,
                        currCarMilleage = r.currCarMilleage,
                        DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                        DateFinished = r.DateFinished == null ? "" : Convert.ToDateTime(r.DateFinished).ToString("dd/MM/yyyy"),
                        CarRepairShopId = r.CarRepairShopId,
                        Status = r.Status,
                        PartsChanged = r.PartsChanged.Select(p => new RepairPartsDTO
                        {
                            Description = p.Part.Description,
                            Number = p.Part.Number
                        }).ToList()
                    }).ToList()

                })
                .ToList();

            return cars;
        }

        public int AddRepair(AddRepairViewModel model)
        {
            var car = this.data.Cars.Where(c => c.Id == model.CarId).FirstOrDefault();

            if(car == null) 
            {
                return -1;
            }

            if(car.Milleage > model.CarMilleage)
            {
                return -2;
            }

            car.Milleage = model.CarMilleage;

            var newRepair = new Repair
            {
                CarId = model.CarId,
                CarRepairShopId = model.CarRepairShopId,
                Description = model.Description,
                currCarMilleage = model.CarMilleage,
                DateCreated = DateTime.UtcNow,
                Status = RepairStatusEnum.Pending.ToString(),
                PartsChanged = new List<RepairParts>()
            };

            

            for (int i = 0; i < model.PartsChanged.Count; i++)
            {
                var newPart = new Part();
                //add repair to newpart list<repairs>
                if(this.data.Parts.Any(p => p.Number == model.PartsChanged[i].Number))
                {
                    newPart = this.data.Parts.Where(p => p.Number == model.PartsChanged[i].Number).FirstOrDefault();
                }
                else
                {
                    newPart.Number = model.PartsChanged[i].Number;
                    newPart.Description = model.PartsChanged[i].Description;
                    this.data.Parts.Add(newPart);
                }

                var newRepairParts = new RepairParts
                {
                    Part = newPart,
                    Repair = newRepair
                };

                newPart.Repairs.Add(newRepairParts);

                this.data.RepairParts.Add(newRepairParts);

                newRepair.PartsChanged.Add(newRepairParts);
            }


            this.data.Repairs.Add(newRepair);

            this.data.SaveChanges();

            return newRepair.Id;
        }

        public RepairDTO GetRepair(int repairId)
        {
            var repair = this.data.Repairs.Where(r => r.Id == repairId)
                .Select(r => new RepairDTO
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    CarRepairShopId = r.CarRepairShopId,
                    CarRepairShopName = r.CarRepairShop.Name,
                    DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                    DateFinished = r.DateFinished == null ? "" : Convert.ToDateTime(r.DateFinished).ToString("dd/MM/yyyy"),
                    currCarMilleage = r.currCarMilleage,
                    Description = r.Description,
                    Status = r.Status,
                    PartsChanged = r.PartsChanged.Select(p => new RepairPartsDTO
                    {
                        Number = p.Part.Number,
                        Description = p.Part.Description,
                    }).ToList()
                }).FirstOrDefault();

            return repair;
        }
    }
}
