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

            this.AllRepairs = new List<Repair>();
            this.IsApproved = false;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsApproved { get; set; }
        public ICollection<Repair> AllRepairs { get; set; }
        
    }
}
