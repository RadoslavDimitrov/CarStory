using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.User
{
    public class RegisterUserFormModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
