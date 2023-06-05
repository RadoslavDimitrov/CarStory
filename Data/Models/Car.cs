using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class Car
    {
        public Car()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Repairs = new List<Repair>();
        }
        [Key]
        public string Id { get; set; }

        [RegularExpression("^[A-Z0-9]{17}$")]
        public string VinNumber { get; set; }

        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }

        [Required]
        public DateTime NextRepair { get; set; }
        [Required]
        public string NextRepairInfo { get; set; }

        public ICollection<Repair> Repairs { get; set; }
    }
}
