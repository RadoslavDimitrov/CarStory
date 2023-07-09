﻿namespace CarStory.Models.Repair
{
    public class AddRepairViewModel
    {
        public AddRepairViewModel()
        {
            this.PartsChanged = new List<RepairPartsViewModel>();
        }
        public string CarId { get; set; }

        public int CarMilleage { get; set; }

        public string Description { get; set; }

        public string CarRepairShopId { get; set; }

        public IList<RepairPartsViewModel> PartsChanged { get; set; }
    }
}
