using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.DTO.RepairParts
{
    public class RepairPartsDTO
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
