using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        //seed user into db
        public static async Task SeedUserAsync(UserManager<AppUser>userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "MerhanHesham",
                    Email = "merhan100.hesham@gmail.com",
                    UserName = "merhanhesham",
                    PhoneNumber = "01003106587"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
            
        }
    }
}
