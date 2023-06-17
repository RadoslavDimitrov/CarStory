using CarStory.Data.Models.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.Car
{
    public class AddCarViewModel
    {
        public string Id { get; set; }

        [RegularExpression(ModelConstants.VinNumberRegex)]
        public string VinNumber { get; set; }

        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }

        public DateTime YearManufactured { get; set; }

        public int Milleage { get; set; }


        public DateTime NextRepair { get; set; }

        public string NextRepairInfo { get; set; }
    }
}
