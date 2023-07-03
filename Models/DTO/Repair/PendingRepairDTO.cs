using CarStory.Data.Models;
using CarStory.Models.DTO.RepairParts;

namespace CarStory.Models.DTO.Repair
{
    public class PendingRepairDTO
    {
        public string VinNumber { get; set; }
        public string CarId { get; set; }
        public int CurrCarMilleage { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public int RepairId { get; set; }
        public string Status { get; set; }

        public virtual List<RepairPartsDTO> PartsChanged { get; set; }

    }
}
