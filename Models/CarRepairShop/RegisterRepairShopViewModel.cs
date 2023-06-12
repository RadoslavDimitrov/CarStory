using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.CarRepairShop
{
    public class RegisterRepairShopViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Usarname must be between 3 and 20 characters long", MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Password must be between 6 and 20 characters long", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
