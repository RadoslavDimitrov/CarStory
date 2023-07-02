using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class RepairParts
    {
        public RepairParts()
        {
            
        }
        public int RepairId { get; set; }
        [Required]
        public virtual Repair Repair { get; set; }
        public int PartId { get; set; }
        [Required]
        public virtual Part Part { get; set; }
    }
}
