using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.User
{
    public class RegisterUserFormModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Usarname must be between 3 and 20 characters long", MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(20,ErrorMessage = "Password must be between 6 and 20 characters long", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
