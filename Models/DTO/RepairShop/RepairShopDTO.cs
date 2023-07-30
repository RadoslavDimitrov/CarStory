using CarStory.Models.DTO.Review;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.DTO.RepairShop
{
    public class RepairShopDTO
    {
        public RepairShopDTO()
        {
            this.Reviews = new List<ReviewDTO>();
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int? ReviewCount { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}
