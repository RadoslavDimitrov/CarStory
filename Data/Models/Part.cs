using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class Part
    {
        public Part()
        {
            this.Repairs = new List<RepairParts>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual ICollection<RepairParts> Repairs { get; set; }
    }
}
