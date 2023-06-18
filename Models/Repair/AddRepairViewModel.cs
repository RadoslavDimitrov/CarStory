namespace CarStory.Models.Repair
{
    public class AddRepairViewModel
    {
        public int Id { get; set; }

        public string CarId { get; set; }

        public string Description { get; set; }

        public string CarRepairShopId { get; set; }

        public ICollection<RepairPartsViewModel> PartsChanged { get; set; }
    }
}
