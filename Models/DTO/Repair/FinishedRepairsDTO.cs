using CarStory.Models.DTO.RepairParts;

namespace CarStory.Models.DTO.Repair
{
    public class FinishedRepairsDTO
    {
        public string VinNumber { get; set; }
        public string CarId { get; set; }
        public int CurrCarMilleage { get; set; }
        public string DateCreated { get; set; }
        public string DateFinished { get; set; }
        public string Description { get; set; }
        public int RepairId { get; set; }
        public string Status { get; set; }

        public virtual List<RepairPartsDTO> PartsChanged { get; set; }

    }
}
