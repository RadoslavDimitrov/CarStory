using CarStory.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.DTO.Car
{
    public class SearchCarDTO
    {
        public string Id { get; set; }

        [RegularExpression("^[A-Z0-9]{17}$")]
        public string VinNumber { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

    }
}
