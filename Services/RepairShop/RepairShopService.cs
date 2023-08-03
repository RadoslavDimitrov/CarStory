using CarStory.Data;
using CarStory.Data.Models;
using CarStory.Infrastructure;
using CarStory.Models.CarRepairShop;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.RepairParts;
using CarStory.Models.DTO.RepairShop;
using CarStory.Models.DTO.Review;
using CarStory.Models.Repair;
using Microsoft.EntityFrameworkCore;

namespace CarStory.Services.RepairShop
{
    public class RepairShopService : IRepairShopService
    {
        private readonly ApplicationDbContext data;

        public RepairShopService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<bool> AddReviewAsync(ReviewDTO review)
        {
            var shop = await this.data.CarRepairShops.Where(sh => sh.Id == review.ShopId).FirstOrDefaultAsync();

            if(shop == null)
            {
                return false;
            }

            var reviewDb = new Review
            {
                CarRepairShopId = review.ShopId,
                CarRepairShop = shop,
                Description = review.Description,
                Username = review.Username,
                DateCreated = DateTime.UtcNow
            };

            await this.data.Reviews.AddAsync(reviewDb);
            shop.Reviews.Add(reviewDb);
            await this.data.SaveChangesAsync();

            return true;
        }

        public int EditRepair(RepairDTO repair)
        {
            var repairDb = this.data.Repairs.Where(r => r.Id == repair.Id).FirstOrDefault();

            if(repairDb == null)
            {
                return -1;
            }

            var car = this.data.Cars.Where(c => c.Id == repairDb.CarId).FirstOrDefault();

            if(repair.currCarMilleage < repairDb.currCarMilleage)
            {
                return -2;
            }

            repairDb.currCarMilleage = repair.currCarMilleage;
            repairDb.Description = repair.Description;

            var repairParts = data.RepairParts.Where(r => r.RepairId == repairDb.Id).ToList();

            foreach (var repairPart in repairParts)
            {
                repairPart.Part.Repairs.Remove(repairPart);
                repairPart.Repair.PartsChanged.Remove(repairPart);
            }

            data.RepairParts.RemoveRange(repairParts);

            repairDb.PartsChanged.Clear();
            car.Milleage = repair.currCarMilleage;

            foreach (var partDTO in repair.PartsChanged)
            {
                if(partDTO.Number == null)
                {
                    continue;
                }

                var newPart = new Part();
                var newRepairParts = new RepairParts();

                if(!this.data.Parts.Any(p => p.Number.ToLower() == partDTO.Number.ToLower()))
                {
                    newPart.Number = partDTO.Number;
                    newPart.Description = partDTO.Description;
                    this.data.Parts.Add(newPart);

                    newRepairParts.Part = newPart;
                    newRepairParts.Repair = repairDb;

                    newPart.Repairs.Add(newRepairParts);

                    this.data.RepairParts.Add(newRepairParts);

                    repairDb.PartsChanged.Add(newRepairParts);
                }
                else
                {
                    if(!repairDb.PartsChanged.Any(p => p.Part.Number.ToLower() == partDTO.Number.ToLower()))
                    {
                        var part = this.data.Parts.Where(p => p.Number == partDTO.Number).First();
                        var repairPart = new RepairParts { Part = part, Repair = repairDb };
                        part.Repairs.Add(repairPart);

                        this.data.RepairParts.Add(repairPart);

                        repairDb.PartsChanged.Add(repairPart);
                    }
                }

            }

            this.data.SaveChanges();
            return repairDb.Id;
            
        }

        public List<FinishedRepairsDTO> FinishedRepairs(string vinNumber, string shopName)
        {
            var repairs = this.data.Repairs
                .Where(r => r.CarRepairShop.Name == shopName)
                .Where(r => r.Status == RepairStatusEnum.Finished.ToString())
                .OrderBy(r => r.DateFinished)
                .AsQueryable();

            if (!string.IsNullOrEmpty(vinNumber))
            {
                repairs = repairs.Where(r => r.Car.VinNumber.Contains(vinNumber));
            }

            var result = repairs.Select(r => new FinishedRepairsDTO
            {
                VinNumber = r.Car.VinNumber,
                CarId = r.CarId,
                CurrCarMilleage = r.currCarMilleage,
                DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                DateFinished = r.DateFinished == null ? "" : r.DateFinished.ToString(),
                Description = r.Description,
                RepairId = r.Id,
                Status = r.Status,
                PartsChanged = r.PartsChanged.Select(p => new RepairPartsDTO
                {
                    Number = p.Part.Number,
                    Description = p.Part.Description
                }).ToList()
            }).ToList();

            return result;
        }

        public bool FinishRepair(int id)
        {
            var repair = this.data.Repairs.Where(r => r.Id == id).FirstOrDefault();

            if(repair == null)
            {
                return false;
            }

            if(repair.Status == RepairStatusEnum.Finished.ToString())
            {
                return false;
            }

            repair.DateFinished = DateTime.UtcNow;
            repair.Status = RepairStatusEnum.Finished.ToString();

            this.data.Update(repair);
            this.data.SaveChanges();

            return true;
        }

        public ShopRepairsViewModel GetAllRepairs(string username)
        {
            var shopRepairs = new ShopRepairsViewModel();

            shopRepairs.PendingRepairs = this.data.Repairs
                .Where(r => r.CarRepairShop.Name == username)
                .Where(r => r.Status == RepairStatusEnum.Pending.ToString())
                .Select(r => new Models.DTO.Repair.RepairDTO
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    VinNumber = r.Car.VinNumber,
                    CarRepairShopId = r.CarRepairShopId,
                    CarRepairShopName = r.CarRepairShop.Name,
                    currCarMilleage = r.currCarMilleage,
                    DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                    DateFinished = r.DateFinished.ToString(),
                    Description = r.Description,
                    Status = r.Status,
                    PartsChanged = r.PartsChanged.Select(rp => new RepairPartsDTO
                    {
                        Number = rp.Part.Number,
                        Description = rp.Part.Description
                    }).ToList()
                }).ToList();

            shopRepairs.FinishedRepairs = this.data.Repairs
                .Where(r => r.CarRepairShop.Name == username)
                .Where(r => r.Status == RepairStatusEnum.Finished.ToString())
                .Select(r => new Models.DTO.Repair.RepairDTO
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    VinNumber = r.Car.VinNumber,
                    CarRepairShopId = r.CarRepairShopId,
                    CarRepairShopName = r.CarRepairShop.Name,
                    currCarMilleage = r.currCarMilleage,
                    DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                    Description = r.Description,
                    Status = r.Status,
                    PartsChanged = r.PartsChanged.Select(rp => new RepairPartsDTO
                    {
                        Number = rp.Part.Number,
                        Description = rp.Part.Description
                    }).ToList()
                }).ToList();

            return shopRepairs;
        }

        public RepairDTO GetRepair(int id)
        {
            var repair = this.data.Repairs
                .Where(r => r.Id == id)
                .Where(r => r.Status != RepairStatusEnum.Finished.ToString())
                .Select(r => new RepairDTO
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    CarRepairShopId = r.CarRepairShopId,
                    CarRepairShopName = r.CarRepairShop.Name,
                    currCarMilleage = r.currCarMilleage,
                    DateCreated = r.DateCreated.ToShortDateString(),
                    Description = r.Description,
                    Status = r.Status,
                    PartsChanged = r.PartsChanged.Select(p => new RepairPartsDTO
                    {
                        Description = p.Part.Description,
                        Number = p.Part.Number
                    }).ToList()
                }).FirstOrDefault();

            return repair;
        }

        public List<PendingRepairDTO> PendingRepairs(string vinNumber, string shopName)
        {
            var repairs = this.data.Repairs
                .Where(r => r.CarRepairShop.Name == shopName)
                .Where(r => r.Status == RepairStatusEnum.Pending.ToString())
                .OrderBy(r => r.DateCreated)
                .AsQueryable();

            if(!string.IsNullOrEmpty(vinNumber))
            {
                repairs = repairs.Where(r => r.Car.VinNumber.Contains(vinNumber));
            }

            var result = repairs.Select(r => new PendingRepairDTO
            {
                VinNumber = r.Car.VinNumber,
                CarId = r.CarId,
                CurrCarMilleage = r.currCarMilleage,
                DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                Description = r.Description,
                RepairId = r.Id,
                Status = r.Status,
                PartsChanged = r.PartsChanged.Select(p => new RepairPartsDTO
                {
                    Number = p.Part.Number,
                    Description = p.Part.Description
                }).ToList()
            }).ToList();

            return result;
        }

        public RepairShopDTO Shop(string id)
        {
            //change reviewDTO with other DTO that has Id
            var shop = this.data.CarRepairShops.Where(sh => sh.Id == id)
                .Select(sh => new RepairShopDTO
                {
                    Id = sh.Id,
                    Description = sh.Description,
                    Email = sh.Email,
                    Location = sh.Location,
                    Name = sh.Name,
                    PhoneNumber = sh.PhoneNumber,
                    ReviewCount = sh.Reviews.Count,
                    Reviews = sh.Reviews.Select(r => new ReviewDTO
                    {
                        Username = r.Username,
                        Description = r.Description,
                        DateCreated = r.DateCreated.ToString("HH:mmh. dd MMM yyyy")
                    }).ToList()
                }).FirstOrDefault();

            return shop;
        }

        public List<RepairShopDTO> Shops(string name, string location)
        {
            var shops = this.data.CarRepairShops.AsQueryable();

            if (!String.IsNullOrEmpty(name))
            {
                shops = shops.Where(sh => sh.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(location))
            {
                shops = shops.Where(sh => sh.Location.Contains(location));
            }

            var result = shops.Select(sh => new RepairShopDTO
            {
                Id = sh.Id,
                Description = sh.Description,
                Email = sh.Email,
                Name = sh.Name,
                Location = sh.Location,
                PhoneNumber = sh.PhoneNumber,
                ReviewCount = sh.Reviews.Count,
                IsApproved = sh.IsApproved
            }).ToList();



            return result;
        }
    }
}
