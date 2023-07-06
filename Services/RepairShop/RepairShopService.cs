using CarStory.Data;
using CarStory.Infrastructure;
using CarStory.Models.CarRepairShop;
using CarStory.Models.DTO.Repair;
using CarStory.Models.DTO.RepairParts;
using CarStory.Models.DTO.RepairShop;
using CarStory.Models.Repair;

namespace CarStory.Services.RepairShop
{
    public class RepairShopService : IRepairShopService
    {
        private readonly ApplicationDbContext data;

        public RepairShopService(ApplicationDbContext data)
        {
            this.data = data;
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
                    CarRepairShopId = r.CarRepairShopId,
                    CarRepairShopName = r.CarRepairShopId,
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
                    CarRepairShopId = r.CarRepairShopId,
                    CarRepairShopName = r.CarRepairShopId,
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
                IsApproved = sh.IsApproved
            }).ToList();


            return result;
        }
    }
}
