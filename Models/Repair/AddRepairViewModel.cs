using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.Repair
{
    public class AddRepairViewModel
    {
        public AddRepairViewModel()
        {
            this.PartsChanged = new List<RepairPartsViewModel>();
        }
        public string CarId { get; set; }

        public int CarMilleage { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Description must be between 5 and 2000 characters long", MinimumLength = 5)]
        public string Description { get; set; }

        public string CarRepairShopId { get; set; }

        public IList<RepairPartsViewModel> PartsChanged { get; set; }
    }
}
