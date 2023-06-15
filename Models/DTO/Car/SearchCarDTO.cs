using CarStory.Data.Models;
using CarStory.Data.Models.ModelConstants;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.DTO.Car
{
    public class SearchCarDTO
    {
        public string Id { get; set; }

        [RegularExpression(ModelConstants.VinNumberRegex)]
        public string VinNumber { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

    }
}
