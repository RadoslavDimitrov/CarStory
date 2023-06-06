using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarStory.Data.Models
{
    public class CarRepairShop
    {
        public CarRepairShop()
        {
            this.Id = Guid.NewGuid().ToString();

            this.PendingRepairs = new List<Repair>();
            this.RepairsHistory = new List<Repair>();
            this.AllRepairs = new List<Repair>();
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Repair> AllRepairs { get; set; }
        [NotMapped]
        private ICollection<Repair> PendingRepairs { get; set; }
        [NotMapped]
        private ICollection<Repair> RepairsHistory { get; set; }
    }
}
