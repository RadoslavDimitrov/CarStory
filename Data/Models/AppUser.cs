using Microsoft.AspNetCore.Identity;

namespace CarStory.Data.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.Cars = new List<Car>();
        }

        public int test { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}
