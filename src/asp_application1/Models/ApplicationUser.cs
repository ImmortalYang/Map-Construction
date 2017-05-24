using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace asp_application1.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int Balance { get; set; }

        public ICollection<City> Cities { get; set; }
        public ICollection<Road> Roads { get; set; }
        public ICollection<Pass> Passes { get; set; }
    }
}
