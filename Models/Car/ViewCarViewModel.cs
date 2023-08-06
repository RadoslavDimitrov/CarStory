using CarStory.Models.DTO.Car;

namespace CarStory.Models.Car
{
    public class ViewCarViewModel
    {
        public CarDTO Car { get; set; }
        public bool IsUserAdded { get; set; }
    }
}
