using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Data
{
    public class SecurityDbContextData
    {
        public static async Task SeedUserAync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    Name = "Paul",
                    Lastname = "Flores",
                    UserName = "paulflores",
                    Email = "paul_1705@outlook.com",
                    Direction = new Direction
                    {
                        Street = "Martha de Roldos",
                        City = "Manta",
                        PostalCode = "13204",
                        Department = "Manabi"
                    }

                };

                var result = await userManager.CreateAsync(user, "P@ul1705");
            }

            if (!roleManager.Roles.Any())
            {
                var role = new IdentityRole
                {
                    Name = "ADMIN"
                };

                await roleManager.CreateAsync(role);
            }

        }
    }
}
