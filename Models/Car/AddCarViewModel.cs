using CarStory.Data.Models.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.Car
{
    public class AddCarViewModel
    {

        [RegularExpression(ModelConstants.VinNumberRegex, ErrorMessage = "Please enter valid Vin Number")]
        public string VinNumber { get; set; }

        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }

        [Required]
        public DateTime YearManufactured { get; set; }

        [Required]
        public int Milleage { get; set; }


        public DateTime? NextRepair { get; set; }

        public string? NextRepairInfo { get; set; }
    }
}
