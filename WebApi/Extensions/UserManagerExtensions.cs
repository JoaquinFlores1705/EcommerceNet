using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> SearchUserWithDirectionAsync(this UserManager<User> input, ClaimsPrincipal usr)
        {
            var email = usr?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var user = await input.Users.Include(x => x.Direction).SingleOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public static async Task<User> SearchUserAsync(this UserManager<User> input, ClaimsPrincipal usr)
        {
            var email = usr?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var user = await input.Users.SingleOrDefaultAsync(x => x.Email == email);

            return user;
        }
    }
}
