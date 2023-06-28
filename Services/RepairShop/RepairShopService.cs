using CarStory.Data;
using CarStory.Infrastructure;
using CarStory.Models.CarRepairShop;
using CarStory.Models.DTO.RepairParts;

namespace CarStory.Services.RepairShop
{
    public class RepairShopService : IRepairShopService
    {
        private readonly ApplicationDbContext data;

        public RepairShopService(ApplicationDbContext data)
        {
            this.data = data;
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
    }
}
