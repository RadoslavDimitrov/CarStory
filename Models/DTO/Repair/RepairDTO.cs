
using CarStory.Infrastructure;
using CarStory.Models.DTO.RepairParts;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.DTO.Repair
{
    public class RepairDTO
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

        public string CarId { get; set; }

        public int currCarMilleage { get; set; }

        public string Status { get; set; } = RepairStatusEnum.Pending.ToString();
        public string CarRepairShopId { get; set; }

        public string CarRepairShopName { get; set; }

        public string DateCreated { get; set; }
        public string? DateFinished { get; set; }

        public ICollection<RepairPartsDTO> PartsChanged { get; set; }
    }
}
