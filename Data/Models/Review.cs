using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string CarRepairShopId { get; set; }
        [Required]
        public virtual CarRepairShop CarRepairShop { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
