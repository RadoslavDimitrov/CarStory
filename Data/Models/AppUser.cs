using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarStory.Data.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.Cars = new List<Car>();
        }
        public ICollection<Car> Cars { get; set; }
    }
}
