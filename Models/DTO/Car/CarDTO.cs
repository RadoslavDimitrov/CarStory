using CarStory.Data.Models.ModelConstants;
using CarStory.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.DTO.Car
{
    public class CarDTO
    {
        public string Id { get; set; }

        public string VinNumber { get; set; }


        public string Make { get; set; }

        public string Model { get; set; }

        public DateTime YearManufactured { get; set; }

        public int Milleage { get; set; }


        public DateTime NextRepair { get; set; }

        public string NextRepairInfo { get; set; }
    }
}
