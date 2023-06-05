using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class Repair
    {
        public Repair()
        {
            this.PartsChanged = new List<RepairParts>();
        }
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

        public string CarId { get; set; }
        [Required]
        public Car Car { get; set; }

        public int CarRepairShopId { get; set; }
        [Required]
        public CarRepairShop CarRepairShop { get; set; }
        public ICollection<RepairParts> PartsChanged { get; set; }
    }
}
