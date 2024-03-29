﻿using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.Car;
using CarStory.Models.DTO.Car;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.RepairParts;
using CarStory.Models.Repair;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarStory.Services.Car
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<AppUser> userManager;

        public CarService(ApplicationDbContext data, UserManager<AppUser> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public async Task<string> AddCarAsync(AddCarViewModel carModel)
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

        public async Task<CarDTO> GetCarAsync(string carId)
        {
            var car = await data.Cars.Include(c => c.Repairs).ThenInclude(r => r.PartsChanged)
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
                .FirstOrDefaultAsync();
         

            return car;
        }

        public async Task<List<CarDTO>> GetAllCarsAsync()
        {
            var cars = await data.Cars
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
                .ToListAsync();

            return cars;
        }

        public async Task<int> AddRepairAsync(AddRepairViewModel model)
        {
            var car = await data.Cars.Where(c => c.Id == model.CarId).FirstOrDefaultAsync();

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
                if(data.Parts.Any(p => p.Number == model.PartsChanged[i].Number))
                {
                    newPart = data.Parts.Where(p => p.Number == model.PartsChanged[i].Number).FirstOrDefault();
                }
                else
                {
                    newPart.Number = model.PartsChanged[i].Number;
                    newPart.Description = model.PartsChanged[i].Description;
                    data.Parts.Add(newPart);
                }

                var newRepairParts = new RepairParts
                {
                    Part = newPart,
                    Repair = newRepair
                };

                newPart.Repairs.Add(newRepairParts);

                await data.RepairParts.AddAsync(newRepairParts);

                newRepair.PartsChanged.Add(newRepairParts);
            }

            var shop = this.data.CarRepairShops.Where(sh => sh.Id == model.CarRepairShopId).FirstOrDefault();
            shop.AllRepairs.Add(newRepair);

            await data.Repairs.AddAsync(newRepair);

            await data.SaveChangesAsync();

            return newRepair.Id;
        }

        public async Task<RepairDTO> GetRepairAsync(int repairId)
        {
            var repair = await data.Repairs.Where(r => r.Id == repairId)
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
                }).FirstOrDefaultAsync();

            return repair;
        }

        public string HasPendingRepair(string carId)
        {
            var repair = this.data.Repairs.Where(r => r.CarId == carId && r.Status == RepairStatusEnum.Pending.ToString()).FirstOrDefault();
            if (repair == null)
            {
                return null;
            }

            return repair.CarRepairShop.Name;
        }
    }
}
