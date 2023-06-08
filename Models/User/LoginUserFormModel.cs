using System.ComponentModel.DataAnnotations;

namespace CarStory.Models.User
{
    public class LoginUserFormModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
