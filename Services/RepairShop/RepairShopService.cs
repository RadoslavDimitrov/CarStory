using CarStory.Data;
using CarStory.Infrastructure;

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

            repair.Status = RepairStatusEnum.Finished.ToString();

            this.data.Update(repair);
            this.data.SaveChanges();

            return true;
        }
    }
}
